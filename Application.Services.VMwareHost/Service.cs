using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.ServiceProcess;
using System.Text;
using System.Data.SqlClient;
using System.Timers;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using System.Security.Principal;
using System.Security.Permissions;
using Microsoft.ApplicationBlocks.Data;
using NCC.ClearView.Application.Core;
using System.Threading;
using System.DirectoryServices;
using Vim25Api;
using System.Web.Services;
using System.Net.NetworkInformation;

namespace ClearViewVMWareHost
{
    public enum RpcAuthnLevel
    {
        Default = 0,
        None,
        Connect,
        Call,
        Pkt,
        PktIntegrity,
        PktPrivacy
    }

    public enum RpcImpLevel
    {
        Default = 0,
        Anonymous,
        Identify,
        Impersonate = 3,
        Delegate
    }

    public enum EoAuthnCap
    {
        None = 0x00,
        MutualAuth = 0x01,
        StaticCloaking = 0x20,
        DynamicCloaking = 0x40,
        AnyAuthority = 0x80,
        MakeFullSIC = 0x100,
        Default = 0x800,
        SecureRefs = 0x02,
        AccessControl = 0x04,
        AppID = 0x08,
        Dynamic = 0x10,
        RequireFullSIC = 0x200,
        AutoImpersonate = 0x400,
        NoCustomMarshal = 0x2000,
        DisableAAA = 0x1000
    }
    public partial class Service : ServiceBase
    {
        [DllImport("Ole32.dll",
            ExactSpelling = true,
            EntryPoint = "CoInitializeSecurity",
            CallingConvention = CallingConvention.StdCall,
            SetLastError = false,
            PreserveSig = false)]
        private static extern void CoInitializeSecurity(
            IntPtr pVoid,
            int cAuthSvc,
            IntPtr asAuthSvc,
            IntPtr pReserved1,
            uint dwAuthnLevel,
            uint dwImpLevel,
            IntPtr pAuthList,
            uint dwCapabilities,
            IntPtr pReserved3);
        [DllImport("advapi32.dll")]
        public static extern int LogonUserA(String lpszUserName,
            String lpszDomain,
            String lpszPassword,
            int dwLogonType,
            int dwLogonProvider,
            ref IntPtr phToken);


        private System.Timers.Timer oTimer = null;
        private double dblInterval;
        private string dsn;
        private string dsnAsset;
        private string dsnServiceEditor;
        private string dsnIP;
        private string dsnZeus;
        private int intEnvironment;
        private string strBootServer;
        private string strBootScript;
        private int intDomain;
        private int intLogging;
        private EventLog oEventLog;
        private string strScripts = "E:\\APPS\\CLV\\ClearViewVMWareHost\\";
        private string strSub = "scripts\\";
        private bool boolUseIP = false;
        private int intBuildTest = 0;
        private int intBuildQA = 0;
        private int intBuildQAOffHours = 0;
        private int intBuildProd = 0;
        private int intBuildProdOffHours = 0;
        private string strEMailIdsBCC = "";
        private int intServiceIdAssetSharedEnvAddCluster=0;
        private int intServiceIdAssetSharedEnvAddHost=0;
        private string strHBAJob = "";
        private string strJob = "";
        private string strVLANname = "";
        private string strVLANnameDEFAULT = "";
        private string strSuffix = "";
        private bool boolJoin = true;
        private bool boolMaintenance = false;
        private bool boolVirtualCenter = false;
            

        public Service()
        {
            InitializeComponent();
            CoInitializeSecurity(IntPtr.Zero,
                -1,
                IntPtr.Zero,
                IntPtr.Zero,
                (uint)RpcAuthnLevel.None,
                (uint)RpcImpLevel.Impersonate,
                IntPtr.Zero,
                (uint)EoAuthnCap.None,
                IntPtr.Zero);
            try
            {
                if (EventLog.SourceExists("ClearView") == false)
                {
                    EventLog.CreateEventSource("ClearView", "ClearView");
                    EventLog.WriteEntry(String.Format("ClearView EventLog Created"), EventLogEntryType.Information);
                }
                oEventLog = new EventLog();
                oEventLog.Source = "ClearView";
                oEventLog.ModifyOverflowPolicy(OverflowAction.OverwriteAsNeeded, 0);
                oEventLog.MaximumKilobytes = long.Parse("16384");
                DataSet ds = new DataSet();
                ds.ReadXml(strScripts + "config.xml");
                dblInterval = Convert.ToDouble(ds.Tables[0].Rows[0]["interval"].ToString());
                intEnvironment = Int32.Parse(ds.Tables[0].Rows[0]["environment"].ToString());
                strBootServer = ds.Tables[0].Rows[0]["boot_server"].ToString();
                strBootScript = ds.Tables[0].Rows[0]["boot_script"].ToString();
                intDomain = Int32.Parse(ds.Tables[0].Rows[0]["domain"].ToString());
                string strDSN = ds.Tables[0].Rows[0]["DSN"].ToString();
                string strDSNAsset = ds.Tables[0].Rows[0]["AssetDSN"].ToString();
                string strDSNIP = ds.Tables[0].Rows[0]["IpDSN"].ToString();
                string strDSNZeus = ds.Tables[0].Rows[0]["ZeusDSN"].ToString();
                string strDSNServiceEditor = ds.Tables[0].Rows[0]["ServiceEditorDSN"].ToString();
                dsn = ds.Tables[0].Rows[0][strDSN].ToString();
                dsnAsset = ds.Tables[0].Rows[0][strDSNAsset].ToString();
                dsnIP = ds.Tables[0].Rows[0][strDSNIP].ToString();
                dsnZeus = ds.Tables[0].Rows[0][strDSNZeus].ToString();
                dsnServiceEditor = ds.Tables[0].Rows[0][strDSNServiceEditor].ToString();

                intBuildTest = Int32.Parse(ds.Tables[0].Rows[0]["build_test"].ToString());
                intBuildQA = Int32.Parse(ds.Tables[0].Rows[0]["build_qa"].ToString());
                intBuildQAOffHours = Int32.Parse(ds.Tables[0].Rows[0]["build_qa_off_hours"].ToString());
                intBuildProd = Int32.Parse(ds.Tables[0].Rows[0]["build_prod"].ToString());
                intBuildProdOffHours = Int32.Parse(ds.Tables[0].Rows[0]["build_prod_off_hours"].ToString());
                intLogging = Int32.Parse(ds.Tables[0].Rows[0]["logging"].ToString());
                intServiceIdAssetSharedEnvAddCluster = Int32.Parse(ds.Tables[0].Rows[0]["ServiceIdAssetSharedEnvAddCluster"].ToString());
                intServiceIdAssetSharedEnvAddHost = Int32.Parse(ds.Tables[0].Rows[0]["ServiceIdAssetSharedEnvAddHost"].ToString());
                strHBAJob = ds.Tables[0].Rows[0]["job_hba"].ToString();
                strJob = ds.Tables[0].Rows[0]["job"].ToString();
                strVLANname = ds.Tables[0].Rows[0]["vlan"].ToString();
                strVLANnameDEFAULT = strVLANname;
                boolJoin = (ds.Tables[0].Rows[0]["join"].ToString() == "1");
                boolMaintenance = (ds.Tables[0].Rows[0]["maintenance"].ToString() == "1");
                boolUseIP = (ds.Tables[0].Rows[0]["use_ip"].ToString() == "1");
                strSuffix = ds.Tables[0].Rows[0]["suffix"].ToString();
                boolVirtualCenter = (ds.Tables[0].Rows[0]["virtual_center"].ToString() == "1");

                oTimer = new System.Timers.Timer(dblInterval);
                oTimer.Elapsed += new ElapsedEventHandler(this.ServiceTimer_Tick);
            }
            catch
            {
                EventLog.WriteEntry(String.Format("ClearView VMware Host Service initialization has failed - INVALID XML FILE"), EventLogEntryType.Error);
            }
        }

        protected override void OnStart(string[] args)
        {
            oEventLog.WriteEntry(String.Format("ClearView VMware Host Service started."), EventLogEntryType.Information);
            oTimer.AutoReset = true;
            oTimer.Enabled = true;
            oTimer.Start();
        }

        protected override void OnStop()
        {
            oEventLog.WriteEntry(String.Format("ClearView VMware Host Service stopped."), EventLogEntryType.Information);
            oTimer.AutoReset = false;
            oTimer.Enabled = false;
        }

        private void ServiceTimer_Tick(object sender, System.Timers.ElapsedEventArgs e)
        {
            oTimer.Stop();
            // *********************************************
            // ************  START Processing  *************
            // *********************************************
            if (intLogging > 2)
                oEventLog.WriteEntry(String.Format("ClearView VMware Host Service TICK."), EventLogEntryType.Information);
            ServiceTick();
            //ServiceTickDecom();
            // *******************************************
            // ************  END Processing  *************
            // *******************************************
            oTimer.Start();
        }
        private void ServiceTick()
        {
            VimService _service = new VimService();
            ManagedObjectReference hostFolderRef = null;
            ManagedObjectReference hostRef = null;
            ManagedObjectReference networkSystem = null;
            ManagedObjectReference vmotionSystem = null;
            HostVirtualSwitchBeaconConfig beaconConfig = null;
            ManagedObjectReference clusterRef = null;
            ManagedObjectReference resourcePoolRootRef = null; 
            VMWare oVMWare = new VMWare(0, dsn);
            try
            {
                oVMWare = new VMWare(0, dsn);
                Functions oFunction = new Functions(0, dsn, intEnvironment);
                ModelsProperties oModelsProperties = new ModelsProperties(0, dsn);
                Servers oServer = new Servers(0, dsn);
                Requests oRequest = new Requests(0, dsn);
                ServiceRequests oServiceRequest = new ServiceRequests(0, dsn);
                Variables oVariable = new Variables(intEnvironment);
                Asset oAsset = new Asset(0, dsnAsset, dsn);
                ServerName oServerName = new ServerName(0, dsn);
                IPAddresses oIPAddresses = new IPAddresses(0, dsnIP, dsn);
                Zeus oZeus = new Zeus(0, dsnZeus);
                Classes oClass = new Classes(0, dsn);
                Environments oEnvironment = new Environments(0, dsn);
                Locations oLocation = new Locations(0, dsn);
                BuildLocation oBuildLocation = new BuildLocation(0, dsn);
                Log oLog = new Log(0, dsn);




                //// Check all hosts in all available clusters
                //DataSet dsVirtualCenter = oVMWare.GetVirtualCenters(1);
                //foreach (DataRow drVirtualCenter in dsVirtualCenter.Tables[0].Rows)
                //{
                //    int intVirtualCenter = Int32.Parse(drVirtualCenter["id"].ToString());
                //    DataSet dsDataCenter = oVMWare.GetDatacenters(intVirtualCenter, 1);
                //    foreach (DataRow drDataCenter in dsDataCenter.Tables[0].Rows)
                //    {
                //        int intDataCenter = Int32.Parse(drDataCenter["id"].ToString());
                //        DataSet dsFolders = oVMWare.GetFolders(intDataCenter, 1);
                //        foreach (DataRow drFolder in dsFolders.Tables[0].Rows)
                //        {
                //            int intFolder = Int32.Parse(drFolder["id"].ToString());
                //            string strNotification = drFolder["notification"].ToString();
                //            DataSet dsClusters = oVMWare.GetClusters(intFolder, 1);
                //            foreach (DataRow drCluster in dsClusters.Tables[0].Rows)
                //            {
                //                if (drCluster["auto_provision_off"].ToString() != "1")
                //                {
                //                    int intCluster = Int32.Parse(drCluster["id"].ToString());
                //                    string strCluster = drCluster["name"].ToString();
                //                    ManagedObjectReference[] oHosts = oVMWare.GetHosts(strCluster);
                //                    int intModel = Int32.Parse(drCluster["modelid"].ToString());

                //                    // Calculate Expansion Variables
                //                    double dblA3 = 0.00;
                //                    double dblA4 = 0.00;
                //                    double dblA5 = 0.00;
                //                    double dblA8 = 0.00;
                //                    double dblA10 = 0.00;
                //                    double dblA20 = 0.00;
                //                    double dblA21 = 0.00;
                //                    double dblA22 = 0.00;
                //                    double dblA25 = 0.00;
                //                    double dblA26 = 0.00;
                //                    double dblA29 = 0.00;
                //                    double dblA30 = 0.00;
                //                    if (drCluster["input_failures"].ToString() != "")
                //                        dblA3 = double.Parse(drCluster["input_failures"].ToString());
                //                    if (drCluster["input_cpu_utilization"].ToString() != "")
                //                        dblA4 = double.Parse(drCluster["input_cpu_utilization"].ToString());
                //                    if (drCluster["input_ram_utilization"].ToString() != "")
                //                        dblA5 = double.Parse(drCluster["input_ram_utilization"].ToString());
                //                    if (drCluster["input_max_ram"].ToString() != "")
                //                        dblA8 = double.Parse(drCluster["input_max_ram"].ToString());
                //                    if (drCluster["input_avg_utilization"].ToString() != "")
                //                        dblA10 = double.Parse(drCluster["input_avg_utilization"].ToString());
                //                    if (drCluster["input_lun_size"].ToString() != "")
                //                        dblA20 = double.Parse(drCluster["input_lun_size"].ToString());
                //                    if (drCluster["input_lun_utilization"].ToString() != "")
                //                        dblA21 = double.Parse(drCluster["input_lun_utilization"].ToString());
                //                    if (drCluster["input_vms_per_lun"].ToString() != "")
                //                        dblA22 = double.Parse(drCluster["input_vms_per_lun"].ToString());
                //                    if (drCluster["input_time_lun"].ToString() != "")
                //                        dblA25 = double.Parse(drCluster["input_time_lun"].ToString());
                //                    if (drCluster["input_time_cluster"].ToString() != "")
                //                        dblA26 = double.Parse(drCluster["input_time_cluster"].ToString());
                //                    if (drCluster["input_max_vms_server"].ToString() != "")
                //                        dblA29 = double.Parse(drCluster["input_max_vms_server"].ToString());
                //                    if (drCluster["input_max_vms_lun"].ToString() != "")
                //                        dblA30 = double.Parse(drCluster["input_max_vms_lun"].ToString());
                //                    double dblA13 = 0.00;
                //                    double dblA14 = 0.00;
                //                    double dblA15 = 0.00;
                //                    double dblA16 = 0.00;
                //                    DataSet dsModel = oModelsProperties.Get(intModel);
                //                    if (dsModel.Tables[0].Rows.Count > 0)
                //                    {
                //                        dblA13 = double.Parse(dsModel.Tables[0].Rows[0]["ram"].ToString());
                //                        dblA14 = double.Parse(dsModel.Tables[0].Rows[0]["cpu_count"].ToString());
                //                        dblA15 = 1.00;
                //                        dblA16 = double.Parse(dsModel.Tables[0].Rows[0]["cpu_speed"].ToString());
                //                    }
                //                    double dblA17 = dblA14 * dblA15 * dblA16;   // =A14*A15*A16
                //                    double dblA2 = double.Parse(oHosts.Length.ToString());
                //                    // Maximum CPU allocated
                //                    double dblA9 = dblA13 * dblA8;
                //                    dblA9 = dblA17 / dblA9;
                //                    // Total CPU & RAM
                //                    double dblD2 = dblA2 * dblA17;
                //                    double dblD3 = dblA2 * dblA13;
                //                    // Reserves
                //                    double dblD7 = dblA29 * dblA26 * dblA9;
                //                    double dblD8 = dblA29 * dblA26 * dblA8;
                //                    double dblD9 = dblA10 + dblA8;
                //                    dblD9 = dblA30 * dblA25 * dblD9;
                //                    // Expansion
                //                    // ExpandCPU
                //                    double dblD12 = dblA3 * 17.00;
                //                    dblD12 = dblD2 - dblD7 - dblD12;
                //                    dblD12 = dblD12 / dblD2;
                //                    dblD12 = dblD12 * dblA4;
                //                    dblD12 = Math.Floor(dblD12);        // 70% = 70 (not .70)
                //                    // ExpandRAM
                //                    double dblD13 = dblA3 * 13.00;
                //                    dblD13 = dblD3 - dblD8 - dblD13;
                //                    dblD13 = dblD13 / dblD3;
                //                    dblD13 = dblD13 * dblA5;
                //                    dblD13 = Math.Floor(dblD13);        // 70% = 70 (not .70)
                //                    // ExpandDisk
                //                    double dblD14 = dblA20 * dblA21;
                //                    dblD14 = dblD14 / 100.00;
                //                    dblD14 = dblD14 - dblD9;
                //                    dblD14 = dblD14 / dblA20;
                //                    dblD14 = dblD14 * 100.00;
                //                    dblD14 = Math.Floor(dblD14);        // 70% = 70 (not .70)


                //                    double dblTotalHostCount = 0.00;
                //                    double dblTotalHostCPU = 0.00;
                //                    double dblTotalHostMemory = 0.00;

                //                    foreach (ManagedObjectReference oHost in oHosts)
                //                    {
                //                        dblTotalHostCount += 1.00;

                //                        HostListSummary oHostSummary = (HostListSummary)oVMWare.getObjectProperty(oHost, "summary");
                //                        HostListSummaryQuickStats oHostStats = oHostSummary.quickStats;
                //                        double dblHostUsageCPU = double.Parse(oHostStats.overallCpuUsage.ToString());
                //                        double dblHostUsageMemory = double.Parse(oHostStats.overallMemoryUsage.ToString());

                //                        HostHardwareInfo oHostHardware = (HostHardwareInfo)oVMWare.getObjectProperty(oHost, "hardware");
                //                        HostCpuInfo oHostCPU = oHostHardware.cpuInfo;
                //                        double dblHostCPU = double.Parse(oHostCPU.hz.ToString());
                //                        double dblHostCores = double.Parse(oHostCPU.numCpuCores.ToString());
                //                        double dblHostMemory = double.Parse(oHostHardware.memorySize.ToString());

                //                        dblHostCPU = dblHostCPU / 1024.00;
                //                        dblHostCPU = dblHostCPU / 1024.00;
                //                        dblHostCPU = dblHostCPU * dblHostCores;
                //                        dblHostUsageCPU = dblHostUsageCPU / dblHostCPU;
                //                        dblTotalHostCPU += dblHostUsageCPU;

                //                        dblHostMemory = dblHostMemory / 1024.00;
                //                        dblHostMemory = dblHostMemory / 1024.00;
                //                        dblHostUsageMemory = dblHostUsageMemory / dblHostMemory;
                //                        dblTotalHostMemory += dblHostUsageMemory;
                //                    }

                //                    dblTotalHostCPU = dblTotalHostCPU / dblTotalHostCount;
                //                    dblTotalHostCPU = dblTotalHostCPU * 100.00;

                //                    dblTotalHostMemory = dblTotalHostMemory / dblTotalHostCount;
                //                    dblTotalHostMemory = dblTotalHostMemory * 100.00;

                //                    if (dblD12 <= dblTotalHostCPU || dblD13 <= dblTotalHostMemory)
                //                    {
                //                        if (oHosts.Length <= 7)
                //                        {
                //                            // Add new server (for auto-provisioning of host)
                //                            if (oServer.GetVMwareCluster(intCluster).Tables[0].Rows.Count == 0)
                //                            {
                //                                // If no servers are already being built
                //                                int intRequest = oRequest.Add(0, -999);
                //                                oServiceRequest.Add(intRequest, 1, 1);
                //                                // intDomain = PNC
                //                                oServer.Add(intRequest, 0, intModel, 0, 0, 1, 0, 0, 0, intDomain, 0, 1, 0, 0, 0, "", 0, 0, 1, 1, 1, 1, 0, 1, intCluster, 0);
                //                            }
                //                            else
                //                            {
                //                                // Only 8 hosts per cluster allowed.  Send notification for new cluster build
                //                                string strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_INVENTORY_MANAGER");
                //                                oFunction.SendEmail("VMware Cluster Needed", strNotification, "", strEMailIdsBCC, "VMware Cluster Needed", "<p><b>This message is to notify you that a new cluster is required before a host can be auto-provisioned</b></p><p>Virtual Center Server: " + drVirtualCenter["name"].ToString() + "<br/>Datacenter: " + drDataCenter["name"].ToString() + "<br/>Folder: " + drFolder["name"].ToString() + "<br/>Total # of Hosts in Cluster: 8</p>", true, false);
                //                            }
                //                        }
                //                    }
                //                }
                //            }
                //        }
                //    }
                //}



                // Build queued hosts
                DataSet ds = oServer.GetVMwareClusters();
                if (intLogging > 1)
                    oEventLog.WriteEntry("There are " + ds.Tables[0].Rows.Count.ToString() + " hosts to build", EventLogEntryType.Information);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    bool boolProcess = false;
                    string strError = "";
                    string strResult = "";
                    int intID = Int32.Parse(dr["id"].ToString());
                    int intModel = Int32.Parse(dr["modelid"].ToString());
                    int intRequestId = Int32.Parse(dr["requestid"].ToString());
                    int intStep = Int32.Parse(dr["step"].ToString());
                    int intCluster = Int32.Parse(dr["vmware_clusterid"].ToString());
                    bool boolPNC = (dr["pnc"].ToString() == "1");
                    int intFolder = Int32.Parse(oVMWare.GetCluster(intCluster, "folderid"));
                    int intDatacenter = Int32.Parse(oVMWare.GetFolder(intFolder, "datacenterid"));
                    int intVirtualCenter = Int32.Parse(oVMWare.GetDatacenter(intDatacenter, "virtualcenterid"));
                    string strVirtualCenter = oVMWare.GetVirtualCenter(intVirtualCenter, "name");
                    int intClass = 0;
                    int intEnv = 0;
                    int intAddress = 0;
                    if (intLogging > 1)
                        oEventLog.WriteEntry("Trying SERVERID = " + intID.ToString(), EventLogEntryType.Information);
                    DataSet dsFolder = oVMWare.GetFolder(intFolder);
                    if (dsFolder.Tables[0].Rows.Count > 0)
                    {
                        DataRow drFolder = dsFolder.Tables[0].Rows[0];
                        Int32.TryParse(drFolder["classid"].ToString(), out intClass);
                        Int32.TryParse(drFolder["environmentid"].ToString(), out intEnv);
                        Int32.TryParse(drFolder["addressid"].ToString(), out intAddress);
                    }
                    if (intClass > 0 && intEnv > 0 && intAddress > 0)
                    {
                        if (intLogging > 1)
                            oEventLog.WriteEntry("Found build location...step = " + intStep.ToString(), EventLogEntryType.Information);
                        //int intModel = Int32.Parse(oVMWare.GetCluster(intCluster, "modelid"));
                        int intParent = 0;
                        bool boolBlade = false;
                        bool boolDell = false;
                        int intAsset = Int32.Parse(dr["assetid"].ToString());
                        int intAssetDR = 0;
                        if (dr["assetid_dr"].ToString() != "")
                            intAssetDR = Int32.Parse(dr["assetid_dr"].ToString());
                        int intName = Int32.Parse(dr["nameid"].ToString());
                        string strIP = dr["ipaddressid"].ToString();
                        int intIP = 0;
                        if (strIP != "")
                            intIP = Int32.Parse(strIP);
                        if (intIP > 0)
                            strIP = oIPAddresses.GetName(intIP, 0);

                        string strIPVMotion = dr["ipaddressid_vmotion"].ToString();
                        string strIPVMotionGateway = "";
                        int intIPVMotion = 0;
                        if (strIPVMotion != "")
                            intIPVMotion = Int32.Parse(strIPVMotion);
                        if (intIPVMotion > 0)
                        {
                            strIPVMotion = oIPAddresses.GetName(intIPVMotion, 0);
                            int intNetworkVMotion = Int32.Parse(oIPAddresses.Get(intIPVMotion, "networkid"));
                            strIPVMotionGateway = oIPAddresses.GetNetwork(intNetworkVMotion, "gateway");
                        }
                        DateTime datModified = DateTime.Parse(dr["modified"].ToString());
                        string strName = "";
                        if (intName > 0)
                            strName = oServerName.GetName(intName, 0);
                        string strNameHost = strName.ToLower() + strSuffix;
                        string strSerial = "";
                        string strAsset = "";
                        string strMAC = "NOTHING";
                        string strILO = "";
                        int intVLAN = 0;
                        if (intAsset > 0)
                        {
                            strSerial = oAsset.GetServerOrBlade(intAsset, "serial");
                            strAsset = oAsset.GetServerOrBlade(intAsset, "asset");
                            strMAC = oAsset.GetServerOrBlade(intAsset, "macaddress");
                            strILO = oAsset.GetServerOrBlade(intAsset, "ilo");
                            intModel = Int32.Parse(oAsset.Get(intAsset, "modelid"));
                            intParent = Int32.Parse(oModelsProperties.Get(intModel, "modelid"));
                            boolBlade = oModelsProperties.IsTypeBlade(intModel);
                            boolDell = oModelsProperties.IsDell(intModel);
                            Int32.TryParse(oAsset.GetServerOrBlade(intAsset, "vlan"), out intVLAN);
                        }
                        string[] nics1 = { "vmnic0", "vmnic1" };
                        string[] nics2 = { "vmnic2", "vmnic3" };
                        DataSet dsBuildRDP = oBuildLocation.GetRDPs(intClass, intEnv, intAddress, 0, 1, 0, 0, 0);


                        if (oClass.IsProd(intClass) || oClass.IsDR(intClass))
                        {
                            if (intBuildProd == 1)
                            {
                                if (intBuildProdOffHours == 0)
                                    boolProcess = true;
                                else if (DateTime.Now.Hour < 7 || DateTime.Now.Hour > 19)
                                    boolProcess = true;
                                else if (intLogging > 2)
                                    oEventLog.WriteEntry("SERVERID: " + intID.ToString() + " (VMWare HOST): " + "PROD Classes are not enabled for DURING PEAK HOURS (7AM - 7PM) builds", EventLogEntryType.Information);
                            }
                            else if (intLogging > 2)
                                oEventLog.WriteEntry("SERVERID: " + intID.ToString() + " (VMWare HOST): " + "PROD Classes are not enabled for builds", EventLogEntryType.Information);
                        }
                        if (oClass.IsQA(intClass))
                        {
                            if (intBuildQA == 1)
                            {
                                if (intBuildQAOffHours == 0)
                                {
                                    boolProcess = true;
                                    if (intLogging > 2)
                                        oEventLog.WriteEntry("SERVERID: " + intID.ToString() + " (VMWare HOST): " + "QA Classes have been enabled for builds", EventLogEntryType.Information);
                                }
                                else if (DateTime.Now.Hour < 7 || DateTime.Now.Hour > 19)
                                {
                                    boolProcess = true;
                                    if (intLogging > 2)
                                        oEventLog.WriteEntry("SERVERID: " + intID.ToString() + " (VMWare HOST): " + "QA Classes have been enabled for OFF PEAK HOURS (7AM - 7PM) Current Time : " + DateTime.Now.ToShortTimeString(), EventLogEntryType.Information);
                                }
                                else if (intLogging > 2)
                                    oEventLog.WriteEntry("SERVERID: " + intID.ToString() + " (VMWare HOST): " + "QA Classes are not enabled for DURING PEAK HOURS (7AM - 7PM) builds", EventLogEntryType.Warning);
                            }
                            else if (intLogging > 2)
                                oEventLog.WriteEntry("SERVERID: " + intID.ToString() + " (VMWare HOST): " + "QA Classes are not enabled for builds", EventLogEntryType.Information);
                        }
                        if (oClass.IsTestDev(intClass))
                        {
                            if (intBuildTest == 1)
                                boolProcess = true;
                            else if (intLogging > 2)
                                oEventLog.WriteEntry("SERVERID: " + intID.ToString() + " (VMWare HOST): " + "TEST Classes are not enabled for builds", EventLogEntryType.Information);
                        }

                        if (boolProcess == true)
                        {
                            DateTime _now = DateTime.Now;
                            TimeSpan oSpan = _now.Subtract(datModified);
                            string strNow = _now.Day.ToString() + _now.Month.ToString() + _now.Year.ToString() + _now.Hour.ToString() + _now.Minute.ToString() + _now.Second.ToString() + _now.Millisecond.ToString();

                            if (boolVirtualCenter == true && intStep > 11 && intStep < 19)
                            {
                                string strConnect = oVMWare.ConnectDEBUG(intVirtualCenter, oVMWare.GetDatacenter(intDatacenter, "name"));
                                if (strConnect == "")
                                {
                                    _service = oVMWare.GetService();
                                    ManagedObjectReference datacenterRef = oVMWare.GetDataCenter();
                                    hostFolderRef = (ManagedObjectReference)oVMWare.getObjectProperty(datacenterRef, "hostFolder");
                                    ManagedObjectReference rootFolder = (ManagedObjectReference)oVMWare.getObjectProperty(datacenterRef, "parent");
                                    string strCluster = oVMWare.GetCluster(intCluster, "name");
                                    clusterRef = oVMWare.GetCluster(strCluster);
                                    //resourcePoolRootRef = (ManagedObjectReference)oVMWare.getObjectProperty(clusterRef, "resourcePool");
                                    resourcePoolRootRef = oVMWare.GetResourcePool(clusterRef, oVMWare.GetCluster(intCluster, "resource_pool"));
                                    ManagedObjectReference hostRefConfig = null;
                                    ManagedObjectReference[] hostRefs = null;
                                    HostConfigManager hostConfig = null;
                                    HostListSummary hostSummary = null;
                                    if (intStep == 12 || boolUseIP == true)
                                        hostRefConfig = GetMOR(oVMWare, strIP, "COMPUTERESOURCE");
                                    else
                                        hostRefConfig = GetMOR(oVMWare, strNameHost, "COMPUTERESOURCE");
                                    hostRefs = (ManagedObjectReference[])oVMWare.getObjectProperty(hostRefConfig, "host");
                                    hostRef = hostRefs[0];
                                    hostConfig = (HostConfigManager)oVMWare.getObjectProperty(hostRef, "configManager");
                                    hostSummary = (HostListSummary)oVMWare.getObjectProperty(hostRef, "summary");
                                    networkSystem = hostConfig.networkSystem;
                                    vmotionSystem = hostConfig.vmotionSystem;
                                    beaconConfig = new HostVirtualSwitchBeaconConfig();
                                    beaconConfig.interval = 1;
                                    if (intStep < 18)
                                    {
                                        if (hostSummary.runtime.inMaintenanceMode == false)
                                        {
                                            ManagedObjectReference _task0 = _service.EnterMaintenanceMode_Task(hostRef, 0, false, false, null);
                                            strError = WaitForExit(oVMWare, _task0, intID, intStep);
                                        }
                                    }
                                }
                                else
                                {
                                    strError = "There was a problem connecting to the Virtual Center URL..." + oVMWare.GetVirtualCenter(intVirtualCenter, "url") + " (" + strConnect + ")";
                                    if (intLogging > 0)
                                        oEventLog.WriteEntry(strError, EventLogEntryType.Error);
                                }
                            }
                            if (intLogging > 1)
                                oEventLog.WriteEntry("Initializing step = " + intStep.ToString(), EventLogEntryType.Information);
                            switch (intStep)
                            {
                                case 0:
                                    oVMWare.DeleteHostNewResults(intID);
                                    oServer.UpdateStep(intID, intStep + 1);
                                    break;
                                case 1:
                                    // Assign Asset
                                    if (intLogging > 0)
                                        oEventLog.WriteEntry("VMWare HOST: Starting Step # " + intStep.ToString(), EventLogEntryType.Information);
                                    if (intAsset == 0)
                                    {
                                        // Get Production / Test Asset
                                        DataSet dsAssets = oAsset.GetServerOrBladeAvailable(intClass, intEnv, intAddress, intModel, 0, -1, 0);
                                        if (dsAssets.Tables[0].Rows.Count > 0)
                                            intAsset = Int32.Parse(dsAssets.Tables[0].Rows[0]["id"].ToString());
                                        if (intAsset == 0)
                                            strError = "The HOST inventory has been depleted ~ Server Model: " + oModelsProperties.Get(intModel, "name").ToUpper() + " [" + intModel.ToString() + "], Class: " + oClass.Get(intClass, "name") + " [" + intClass.ToString() + "], Environment: " + oEnvironment.Get(intEnv, "name") + " [" + intEnv.ToString() + "], Location: " + oLocation.GetFull(intAddress) + " [" + intAddress.ToString() + "])";
                                    }
                                    if (strError == "" && oClass.IsProd(intClass) && intAssetDR == 0 && oModelsProperties.IsEnforce1to1Recovery(intModel) == true)
                                    {
                                        // Get DR Asset (if Production)
                                        DataSet dsAssetsDR = oAsset.GetServerOrBladeAvailableDR(intEnv, intModel, intAsset, oModelsProperties.IsDell(intModel), null);
                                        if (dsAssetsDR.Tables[0].Rows.Count > 0)
                                            intAssetDR = Int32.Parse(dsAssetsDR.Tables[0].Rows[0]["id"].ToString());
                                        if (intAssetDR == 0)
                                            strError = "The Disaster Recovery HOST inventory has been depleted ~ Server Model: " + oModelsProperties.Get(intModel, "name").ToUpper() + " [" + intModel.ToString() + "], Environment: " + oEnvironment.Get(intEnv, "name") + " [" + intEnv.ToString() + "], AssetID: " + intAsset.ToString() + ")";
                                        else
                                        {
                                            oServer.AddAsset(intID, intAssetDR, intClass, intEnv, 0, 1);
                                            oAsset.AddStatus(intAssetDR, "", (int)AssetStatus.InUse, -123, DateTime.Now);
                                            oAsset.Update(intAssetDR, (int)AssetAttribute.Ok);
                                        }
                                    }
                                    if (strError == "")
                                    {
                                        oServer.AddAsset(intID, intAsset, intClass, intEnv, 0, 0);
                                        oAsset.AddStatus(intAsset, "", (int)AssetStatus.InUse, -123, DateTime.Now);
                                        oAsset.Update(intAsset, (int)AssetAttribute.Ok);
                                        
                                        oVMWare.AddHostNewResult(intID, "Found asset, Serial Number: " + oAsset.GetServerOrBlade(intAsset, "serial"), 0);
                                        oServer.UpdateStep(intID, intStep + 1);
                                    }
                                    break;
                                case 2:
                                    // Assign Name
                                    if (intLogging > 0)
                                        oEventLog.WriteEntry("VMWare HOST: Starting Step # " + intStep.ToString(), EventLogEntryType.Information);
                                    string strPrefix = "XNV";
                                    if (intName == 0)
                                    {
                                        intName = oServerName.Add(intClass, intEnv, intAddress, strPrefix, 0, oAsset.GetServerOrBlade(intAsset, "serial").ToUpper(), 1, dsnServiceEditor);
                                        if (intName == 0)
                                            strError = "All available server names are in use for the criteria specified";
                                        if (strError == "")
                                            oServer.UpdateServerNamed(intID, intName);
                                    }
                                    if (strError == "")
                                    {
                                        strName = oServerName.GetName(intName, 0);
                                        oAsset.AddStatus(intAsset, strName, (int)AssetStatus.InUse, 0, DateTime.Now);
                                        if (intAssetDR > 0)
                                        {
                                            string strNameDR = strName + "-DR";
                                            if (strName.StartsWith("OHCLE") == true)
                                                strNameDR = strName.Replace("OHCLE", "OHCIN");
                                            oAsset.AddStatus(intAssetDR, strNameDR, (int)AssetStatus.InUse, 0, DateTime.Now);
                                        }

                                        oVMWare.AddHostNewResult(intID, "Server Name: " + strName, 0);
                                        oServer.UpdateStep(intID, intStep + 1);
                                    }
                                    break;
                                case 3:
                                    // Assign IP for Service Console NIC
                                    if (intLogging > 0)
                                        oEventLog.WriteEntry("VMWare HOST: Starting Step # " + intStep.ToString(), EventLogEntryType.Information);
                                    if (intIP == 0)
                                    {
                                        if (intVLAN > 0)
                                        {
                                            intIP = oIPAddresses.Get_VLAN(intClass, intEnv, intAddress, 0, 0, intVLAN, 0, 0, 0, 0, 0, 0, 0, true, intID, 1, 0, -1, intEnvironment, dsnServiceEditor);
                                            oServer.AddOutput(intID, "IP_ASSIGN_SERVICE", oIPAddresses.Results());
                                            oIPAddresses.ClearResults();
                                            if (intIP == 0)
                                                strError = "Unable to assign an IP Address for Service Console ~ (Class = " + intClass.ToString() + ", Env = " + intEnv.ToString() + ", Address = " + intAddress.ToString() + ")";
                                            if (strError == "")
                                                oServer.AddIP(intID, intIP, 1, 1, 0, 0);
                                        }
                                        else
                                            strError = "The VLAN for this asset has not been configured";
                                    }
                                    if (strError == "")
                                    {
                                        strResult = "IP Address: " + oIPAddresses.GetName(intIP, 0);
                                        oVMWare.AddHostNewResult(intID, strResult, 0);
                                        oLog.AddEvent(strName, strSerial, strResult, LoggingType.Information);
                                        oServer.UpdateStep(intID, intStep + 1);
                                    }
                                    break;
                                case 4:
                                    // Assign IP for VMotion NIC
                                    if (intLogging > 0)
                                        oEventLog.WriteEntry("VMWare HOST: Starting Step # " + intStep.ToString(), EventLogEntryType.Information);
                                    if (intIPVMotion == 0)
                                    {
                                        if (intVLAN > 0)
                                        {
                                            intIPVMotion = oIPAddresses.Get_VLAN(intClass, intEnv, intAddress, 0, 0, intVLAN, 0, 0, 0, 0, 0, 0, 0, true, intID, 0, 1, -1, intEnvironment, dsnServiceEditor);
                                            oServer.AddOutput(intID, "IP_ASSIGN_VMOTION", oIPAddresses.Results());
                                            oIPAddresses.ClearResults();
                                            if (intIPVMotion == 0)
                                                strError = "Unable to assign an IP Address for VMkernel ~ (Class = " + intClass.ToString() + ", Env = " + intEnv.ToString() + ", Address = " + intAddress.ToString() + ")";
                                            if (strError == "")
                                                oServer.AddIP(intID, intIPVMotion, 0, 1, 1, 0);
                                        }
                                        else
                                            strError = "The VLAN for this asset has not been configured";
                                    }
                                    if (strError == "")
                                    {
                                        strResult = "IP Address (VMotion): " + oIPAddresses.GetName(intIPVMotion, 0);
                                        oVMWare.AddHostNewResult(intID, strResult, 0);
                                        oLog.AddEvent(strName, strSerial, strResult, LoggingType.Information);
                                        oServer.UpdateStep(intID, intStep + 1);
                                    }
                                    break;
                                case 5:
                                    // Configure RDP
                                    if (intLogging > 0)
                                        oEventLog.WriteEntry("VMWare HOST: Starting Step # " + intStep.ToString(), EventLogEntryType.Information);
                                    // Query the RDP locations for WebServices and VLAN
                                    string strRDPComputerWebService = "";
                                    string strRDPScheduleWebService = "";
                                    string strRDPVLAN = "";
                                    oLog.AddEvent(strName, strSerial, "There are " + dsBuildRDP.Tables[0].Rows.Count.ToString() + " build locations (RDP) for class: " + oClass.Get(intClass, "name") + ", address: " + oLocation.GetFull(intAddress), LoggingType.Information);
                                    if (dsBuildRDP.Tables[0].Rows.Count > 0)
                                    {
                                        strRDPComputerWebService = dsBuildRDP.Tables[0].Rows[0]["rdp_computer_ws"].ToString();
                                        strRDPScheduleWebService = dsBuildRDP.Tables[0].Rows[0]["rdp_schedule_ws"].ToString();
                                        strRDPVLAN = dsBuildRDP.Tables[0].Rows[0]["blade_vlan"].ToString();
                                    }
                                    else
                                    {
                                        strError = "Invalid build location (RDP) configuration ~ Class: " + oClass.Get(intClass, "name") + ", Location: " + oLocation.GetFull(intAddress);
                                        oLog.AddEvent(strName, strSerial, strError, LoggingType.Error);
                                    }
                                    if (strError == "" && boolBlade == true)
                                    {
                                        if (boolDell == false)
                                        {
                                            strResult = "Attempting to preconfigure virtual connect settings (AssetID: " + intAsset.ToString() + ")";
                                            oVMWare.AddHostNewResult(intID, strResult, 0);
                                            oLog.AddEvent(strName, strSerial, strResult, LoggingType.Information);
                                            string strResultVC2 = oAsset.ExecuteVirtualConnectIP(intAsset, 0, intEnvironment, strRDPVLAN, 2, false, false, true);
                                            if (strResultVC2.Contains("ERROR") == true || strResultVC2 == "")
                                            {
                                                oLog.AddEvent(strName, strSerial, "Encountered an error when configuring virtual connect settings ~ build NIC (#2) for " + strRDPVLAN + "... " + strResultVC2, LoggingType.Error);
                                                strError = "There was a problem configuring the Virtual Connect Settings for build ~ NIC (#2)";
                                            }
                                            else
                                                oLog.AddEvent(strName, strSerial, "Successfully preconfigured virtual connect settings (NIC#2) (AssetID: " + intAsset.ToString() + ")", LoggingType.Information);
                                            if (strError == "")
                                            {
                                                // NIC#1 = build network, PXE enabled
                                                string strResultVC1 = oAsset.ExecuteVirtualConnectIP(intAsset, 0, intEnvironment, strRDPVLAN, 1, true, false, false);
                                                if (strResultVC1.Contains("ERROR") == true || strResultVC1 == "")
                                                {
                                                    oLog.AddEvent(strName, strSerial, "Encountered an error when configuring virtual connect settings ~ build NIC (#1) for " + strRDPVLAN + "... " + strResultVC1, LoggingType.Error);
                                                    strError = "There was a problem configuring the Virtual Connect Settings for build ~ NIC (#1)";
                                                }
                                                else
                                                    oLog.AddEvent(strName, strSerial, "Successfully preconfigured virtual connect settings (NIC#1) (AssetID: " + intAsset.ToString() + ")", LoggingType.Information);
                                            }
                                        }
                                    }
                                    if (strError == "")
                                    {
                                        // Strip the :'s AND -'sout of MAC address
                                        string strMACAddress = strMAC;
                                        if (boolBlade == true)
                                        {
                                            strMACAddress = oAsset.GetVirtualConnectMACAddress(intAsset, 0, intEnvironment, 1, strScripts, dsn, strScripts, oLog, strName);
                                            if (strMACAddress == "**ERROR**")
                                            {
                                                strError = "There was a problem retrieving the MAC address of the blade";
                                                oLog.AddEvent(strName, strSerial, strError + " ~ AssetID " + intAsset.ToString() + ". Output file: " + strScripts + intAsset.ToString() + ".txt", LoggingType.Error);
                                            }
                                        }
                                        if (strError == "" && strMACAddress == "")
                                        {
                                            strError = "There is not MAC address configured for this device";
                                            oLog.AddEvent(strName, strSerial, "Invalid MAC address configured for this device ~ AssetID " + intAsset.ToString(), LoggingType.Information);
                                        }
                                        else
                                        {
                                            string strMACAddressStrip = strMACAddress;
                                            while (strMACAddressStrip.Contains(":") == true)
                                                strMACAddressStrip = strMACAddressStrip.Replace(":", "");
                                            while (strMACAddressStrip.Contains("-") == true)
                                                strMACAddressStrip = strMACAddressStrip.Replace("-", "");
                                            // Configure RDP
                                            strResult = "Attempting to configure RDP";
                                            oVMWare.AddHostNewResult(intID, strResult, 0);
                                            oLog.AddEvent(strName, strSerial, strResult, LoggingType.Information);
                                            if (strRDPComputerWebService != "" && strRDPScheduleWebService != "")
                                            {
                                                if (intEnvironment == (int)CurrentEnvironment.CORPDMN || intEnvironment == (int)CurrentEnvironment.PNCNT_PROD)
                                                {
                                                    // Create Computer Object
                                                    System.Net.NetworkCredential oCredentials = new System.Net.NetworkCredential(oVariable.AltirisUsername(), oVariable.AltirisPassword(), oVariable.AltirisDomain());
                                                    NCC.ClearView.Application.Core.AltirisWS_Computer.ComputerManagementService oComputer = new NCC.ClearView.Application.Core.AltirisWS_Computer.ComputerManagementService();
                                                    oComputer.Credentials = oCredentials;
                                                    oComputer.Url = strRDPComputerWebService;
                                                    // Delete Computer Object if it Exists
                                                    int intDeleteComputer = oComputer.GetComputerID(strName, 1);
                                                    if (intDeleteComputer > 0)
                                                    {
                                                        bool boolDelete = oComputer.DeleteComputer(intDeleteComputer);
                                                        strResult = "Found Duplicate Computer Object....Deleting RDP ComputerID " + intDeleteComputer.ToString() + " on " + strRDPComputerWebService + "...result = " + boolDelete.ToString();
                                                        oVMWare.AddHostNewResult(intID, strResult, 0);
                                                        oLog.AddEvent(strName, strSerial, strResult, LoggingType.Information);
                                                    }
                                                    // Add Computer Object
                                                    strResult = "Configuring RDP (MAC: " + strMACAddressStrip + ") on " + strRDPScheduleWebService;
                                                    oVMWare.AddHostNewResult(intID, strResult, 0);
                                                    oLog.AddEvent(strName, strSerial, strResult, LoggingType.Information);
                                                    int intComputer = oComputer.AddBasicVirtualComputer(-1, strName, oAsset.Get(intAsset, "asset"), oAsset.Get(intAsset, "serial"), strMACAddressStrip, 2, "");
                                                    // Assign Schedule
                                                    NCC.ClearView.Application.Core.altirisws.dsjob oJob = new NCC.ClearView.Application.Core.altirisws.dsjob();
                                                    oJob.Credentials = oCredentials;
                                                    oJob.Url = strRDPScheduleWebService;
                                                    // Add HBA Job (since it's LINUX)
                                                    strResult = "Adding RDP Job (" + strHBAJob + ")";
                                                    oVMWare.AddHostNewResult(intID, strResult, 0);
                                                    oLog.AddEvent(strName, strSerial, strResult, LoggingType.Information);
                                                    oJob.ScheduleNow(strName, strHBAJob);
                                                    strResult = "Adding RDP Job (" + strJob + ")";
                                                    oVMWare.AddHostNewResult(intID, strResult, 0);
                                                    oLog.AddEvent(strName, strSerial, strResult, LoggingType.Information);
                                                    oJob.ScheduleNow(strName, strJob);
                                                    strResult = "Finished Configuring RDP";
                                                    oVMWare.AddHostNewResult(intID, strResult, 0);
                                                    oLog.AddEvent(strName, strSerial, strResult, LoggingType.Information);
                                                }
                                                else
                                                {
                                                    strResult = "RDP Configuration Skipped (Environment = " + intEnvironment.ToString() + ")";
                                                    oVMWare.AddHostNewResult(intID, strResult, 0);
                                                    oLog.AddEvent(strName, strSerial, strResult, LoggingType.Information);
                                                }

                                                strResult = "RDP has been configured";
                                                oVMWare.AddHostNewResult(intID, strResult, 0);
                                                oLog.AddEvent(strName, strSerial, strResult, LoggingType.Information);
                                                oServer.UpdateStep(intID, intStep + 1);
                                            }
                                            else
                                            {
                                                strError = "Invalid build location (RDP) configuration ~ Class: " + oClass.Get(intClass, "name") + ", Environment: " + oEnvironment.Get(intEnv, "name") + ", Location: " + oLocation.GetFull(intAddress);
                                                oLog.AddEvent(strName, strSerial, strError, LoggingType.Error);
                                            }
                                        }
                                    }
                                    break;
                                case 6:
                                    // Configure ZEUS table
                                    if (intLogging > 0)
                                        oEventLog.WriteEntry("VMWare HOST: Starting Step # " + intStep.ToString(), EventLogEntryType.Information);
                                    if (strILO == "")
                                        strError = "The ILO IP Address is missing for Asset ID " + intAsset.ToString();
                                    else
                                    {
                                        string strMACAddress = strMAC.ToUpper();
                                        while (strMACAddress.Contains(" ") == true)
                                            strMACAddress = strMACAddress.Replace(" ", "");
                                        if (boolBlade == true)
                                        {
                                            // Query virtual connect for appropriate MAC address
                                            oEventLog.WriteEntry("VMWare HOST: Querying for MAC Address (oAsset.GetVirtualConnectMACAddress) for AssetID " + intAsset.ToString(), EventLogEntryType.Information);
                                            strMACAddress = oAsset.GetVirtualConnectMACAddress(intAsset, 0, intEnvironment, 1, strScripts, dsn, strScripts, oLog, strName);
                                            if (strMACAddress == "**ERROR**")
                                            {
                                                strError = "There was a problem retrieving the MAC address of the blade";
                                                oEventLog.WriteEntry("VMWare HOST: There was a problem retrieving the MAC address of the blade ~ AssetID " + intAsset.ToString() + ". Output file: " + strScripts + intAsset.ToString() + ".txt", EventLogEntryType.Error);
                                            }
                                        }
                                        if (strError == "" && strMACAddress == "")
                                        {
                                            strError = "There is not MAC address configured for this device";
                                            oEventLog.WriteEntry("VMWare HOST: Invalid MAC address configured for this device ~ AssetID " + intAsset.ToString(), EventLogEntryType.Error);
                                        }
                                        else
                                        {
                                            while (strMACAddress.Contains("-") == true)
                                                strMACAddress = strMACAddress.Replace("-", ":");

                                            // SOURCE
                                            string strSource = "SERVER";
                                            if (dsBuildRDP.Tables[0].Rows.Count > 0 && dsBuildRDP.Tables[0].Rows[0]["source"].ToString() != "")
                                                strSource = dsBuildRDP.Tables[0].Rows[0]["source"].ToString();

                                            // DOMAIN
                                            string strDomain = "PNCBANK";
                                            if (boolPNC == false)
                                            {
                                                if (oClass.IsProd(intClass))
                                                    strDomain = "CORPDMN";
                                                else
                                                    strDomain = "CORPTEST";
                                            }
                                            
                                            // ZEUS TABLE
                                            try
                                            {
                                                oZeus.DeleteBuild(strSerial);
                                                oZeus.DeleteBuildName(strName);
                                                oZeus.DeleteApps(strSerial);
                                                oZeus.DeleteLuns(strSerial);
                                                oZeus.DeleteResults(strSerial);
                                                oZeus.AddBuild(0, 0, 0, strSerial, strAsset, strName, "", "", "", 0, "", strDomain, intEnvironment, strSource, 0, strMACAddress, "", strIP, "", "", "", "", "", "", "", "", "", "", "", "", "", 1);
                                            }
                                            catch (Exception exZeus)
                                            {
                                                strError = exZeus.Message;
                                            }
                                            if (strError == "")
                                            {
                                                strResult = "ZEUS table configured";
                                                oVMWare.AddHostNewResult(intID, strResult, 0);
                                                oLog.AddEvent(strName, strSerial, strResult, LoggingType.Information);
                                                oServer.UpdateStep(intID, intStep + 1);
                                            }
                                        }
                                    }
                                    break;
                                case 7:
                                    // Power On
                                    if (intLogging > 0)
                                        oEventLog.WriteEntry("VMWare HOST: Starting Step # " + intStep.ToString(), EventLogEntryType.Information);
                                    if (strILO == "")
                                        strError = "The ILO IP Address is missing for Asset ID " + intAsset.ToString();
                                    else
                                    {
                                        bool boolPower = oFunction.ExecutePower(intID, intAsset, true, "Power", strName, intEnvironment, (boolDell ? strScripts : strScripts + strSub), intLogging, dsnAsset);
                                        if (boolPower == false)
                                            strError = "There was a problem powering on the device ~ probably the ILO password is wrong";
                                    }
                                    if (strError == "")
                                    {
                                        strResult = "Server Powered On (<a href=\"https://" + strILO + "\" target=\"_blank\">" + strILO + "</a>)";
                                        oVMWare.AddHostNewResult(intID, strResult, 0);
                                        oLog.AddEvent(strName, strSerial, strResult, LoggingType.Information);
                                        oServer.UpdateStep(intID, intStep + 1);
                                    }
                                    break;
                                case 8:
                                    // Image
                                    while (strMAC.Contains("-") == true)
                                        strMAC = strMAC.Replace("-", ":");
                                    DataSet dsZeus = SqlHelper.ExecuteDataset(dsnZeus, CommandType.Text, "SELECT * FROM cv_zeus_builds WHERE serial = '" + strSerial + "' AND name = '" + strName + "' AND dhcp IS NOT NULL AND deleted = 0");
                                    if (dsZeus.Tables[0].Rows.Count > 0)
                                    {
                                        // Cleanup RDP
                                        oLog.AddEvent(strName, strSerial, "There are " + dsBuildRDP.Tables[0].Rows.Count.ToString() + " build locations (RDP) for class: " + oClass.Get(intClass, "name") + ", address: " + oLocation.GetFull(intAddress), LoggingType.Information);
                                        if (dsBuildRDP.Tables[0].Rows.Count > 0 && dsBuildRDP.Tables[0].Rows[0]["rdp_computer_ws"].ToString() != "" && dsBuildRDP.Tables[0].Rows[0]["rdp_schedule_ws"].ToString() != "")
                                        {
                                            if (intEnvironment == (int)CurrentEnvironment.CORPDMN || intEnvironment == (int)CurrentEnvironment.PNCNT_PROD)
                                            {
                                                strResult = "Removing computer object from RDP";
                                                oVMWare.AddHostNewResult(intID, strResult, 0);
                                                oLog.AddEvent(strName, strSerial, strResult, LoggingType.Information);
                                                // Delete Computer Object
                                                string strRDPComputerWebServiceRemove = dsBuildRDP.Tables[0].Rows[0]["rdp_computer_ws"].ToString();
                                                System.Net.NetworkCredential oCredentials = new System.Net.NetworkCredential(oVariable.AltirisUsername(), oVariable.AltirisPassword(), oVariable.AltirisDomain());
                                                NCC.ClearView.Application.Core.AltirisWS_Computer.ComputerManagementService oComputer = new NCC.ClearView.Application.Core.AltirisWS_Computer.ComputerManagementService();
                                                oComputer.Credentials = oCredentials;
                                                oComputer.Url = strRDPComputerWebServiceRemove;
                                                int intDeleteComputer = oComputer.GetComputerID(strName, 1);
                                                oLog.AddEvent(strName, strSerial, "Deleting Altiris ComputerID " + intDeleteComputer.ToString() + " on " + strRDPComputerWebServiceRemove, LoggingType.Information);
                                                if (intDeleteComputer > 0)
                                                {
                                                    bool boolDelete = oComputer.DeleteComputer(intDeleteComputer);
                                                    strResult = "Finished Removing RDP Object";
                                                    oVMWare.AddHostNewResult(intID, strResult, 0);
                                                    oLog.AddEvent(strName, strSerial, strResult, LoggingType.Information);
                                                }
                                                else
                                                    oLog.AddEvent(strName, strSerial, "Finished Deleting Altiris (Not Found)", LoggingType.Information);
                                            }
                                            else
                                                oLog.AddEvent(strName, strSerial, "Altiris Deletion Skipped (Environment = " + intEnvironment.ToString() + ")", LoggingType.Information);
                                        }
                                        else
                                        {
                                            strError = "Invalid build location (RDP) configuration ~ Class: " + oClass.Get(intClass, "name") + ", Environment: " + oEnvironment.Get(intEnv, "name") + ", Location: " + oLocation.GetFull(intAddress);
                                            oLog.AddEvent(strName, strSerial, strError, LoggingType.Error);
                                        }

                                        strResult = "Image Process Completed";
                                        oVMWare.AddHostNewResult(intID, strResult, 0);
                                        oLog.AddEvent(strName, strSerial, strResult, LoggingType.Information);
                                        oServer.UpdateStep(intID, intStep + 1);
                                    }
                                    else if (oSpan.Hours > 4)
                                        strError = "Sitting at the IMAGE step for more than FOUR (4) hours!";
                                    break;
                                case 9:
                                    // Change VLAN
                                    if (intLogging > 0)
                                        oEventLog.WriteEntry("VMWare HOST: Starting Step # " + intStep.ToString(), EventLogEntryType.Information);
                                    oVMWare.AddHostNewResult(intID, "Changing IP Address Configuration", 0);
                                    bool boolOKtoAssignIP = false;
                                    DataSet dsSwitch = oAsset.GetSwitchports(intAsset, SwitchPortType.Network);
                                    if (boolBlade == true)
                                        boolOKtoAssignIP = true;
                                    else
                                    {
                                        if (dsSwitch.Tables[0].Rows.Count > 0)
                                            boolOKtoAssignIP = true;
                                        else
                                        {
                                            // Switchport configuration has not been setup for this asset.  For now, just note and continue
                                            oLog.AddEvent(strName, strSerial, "Switchport configuration has not been setup for this asset...skipping IP assignment and continuing the build", LoggingType.Information);
                                            oVMWare.AddHostNewResult(intID, "Automatic IP Address assignment is not available for this asset", 0);
                                            oServer.UpdateStep(intID, intStep + 1);
                                        }
                                    }

                                    if (boolOKtoAssignIP == true)
                                    {
                                        // Check to see if it was already changed from a previous attempt that errored.
                                        oLog.AddEvent(strName, strSerial, "Checking to see if IP was already assigned from a previous attempt", LoggingType.Information);
                                        string strIPChange = oIPAddresses.GetName(intIP, 0);
                                        bool boolPingChanged = false;
                                        for (int ii = 0; ii < 3 && boolPingChanged == false; ii++)
                                        {
                                            Thread.Sleep(3000);
                                            Ping oPingChange = new Ping();
                                            string strStatusChange = "";
                                            try
                                            {
                                                PingReply oReplyChange = oPingChange.Send(strIPChange);
                                                strStatusChange = oReplyChange.Status.ToString().ToUpper();
                                            }
                                            catch { }
                                            boolPingChanged = (strStatusChange == "SUCCESS");
                                        }
                                        if (boolPingChanged == false)
                                        {
                                            oLog.AddEvent(strName, strSerial, "IP address was not assigned from a previous attempt", LoggingType.Information);
                                            if (boolBlade == true && boolDell == false)
                                            {
                                                oLog.AddEvent(strName, strSerial, "Turning off server to reconfigure virtual connect settings", LoggingType.Information);
                                                // Should be shutting down at this point....
                                                AssetPowerStatus powVirtualConnect = oAsset.PowerStatus(intAsset, 0, intEnvironment, strScripts);
                                                if (powVirtualConnect == AssetPowerStatus.Error)
                                                {
                                                    int intEnclosure = Int32.Parse(oAsset.GetServerOrBlade(intAsset, "enclosureid"));
                                                    string strHost = oAsset.GetEnclosure(intEnclosure, "virtual_connect");
                                                    strError = "Could not connect to Virtual Connect Manager IP ~ " + strHost;
                                                }
                                                else
                                                {
                                                    oLog.AddEvent(strName, strSerial, "Checking to see if server is turned off", LoggingType.Information);
                                                    int intAttempt = 0;
                                                    for (intAttempt = 0; intAttempt < 20 && powVirtualConnect != AssetPowerStatus.Off; intAttempt++)
                                                    {
                                                        powVirtualConnect = oAsset.PowerStatus(intAsset, 0, intEnvironment, strScripts);
                                                        int intAttemptLeft = (20 - intAttempt);
                                                        if (intLogging > 1)
                                                            oLog.AddEvent(strName, strSerial, "Server still on...waiting 5 seconds (Power = " + powVirtualConnect.ToString() + ") (" + intAttemptLeft.ToString() + " tries left)", LoggingType.Information);
                                                        Thread.Sleep(3000);
                                                    }
                                                    if (intAttempt == 20)
                                                    {
                                                        // Server is still on....manually shutdown using ILO
                                                        oLog.AddEvent(strName, strSerial, "Server " + strName + " still on...shutting down using ILO", LoggingType.Warning);
                                                        bool boolNetworkPowerOff = oFunction.ExecutePower(intID, intAsset, false, "Power (Network / Power Off)", strName, intEnvironment, (boolDell ? strScripts : strScripts + strSub), intLogging, dsnAsset);
                                                        if (boolNetworkPowerOff == false)
                                                            strError = "There was a problem turning off the server via ILO";
                                                        else
                                                        {
                                                            oLog.AddEvent(strName, strSerial, "Checking to see if server is turned off from ILO", LoggingType.Information);
                                                            powVirtualConnect = oAsset.PowerStatus(intAsset, 0, intEnvironment, strScripts);
                                                            for (intAttempt = 0; intAttempt < 40 && powVirtualConnect != AssetPowerStatus.Off; intAttempt++)
                                                            {
                                                                powVirtualConnect = oAsset.PowerStatus(intAsset, 0, intEnvironment, strScripts);
                                                                int intAttemptLeft = (40 - intAttempt);
                                                                if (intLogging > 1)
                                                                    oLog.AddEvent(strName, strSerial, "Server still on...waiting 5 seconds (Power = " + powVirtualConnect.ToString() + ") (" + intAttemptLeft.ToString() + " tries left)", LoggingType.Information);
                                                                Thread.Sleep(3000);
                                                            }
                                                            if (intAttempt == 40)
                                                            {
                                                                // Server is still on....throw error
                                                                strError = "There was a problem shutting down the server to change virtual connect settings";
                                                                oLog.AddEvent(strName, strSerial, strError, LoggingType.Error);
                                                            }
                                                        }
                                                    }

                                                    if (strError == "")
                                                    {
                                                        // Server is powered off...change VLAN
                                                        // First try to change to strVLANname from web.config (Probably "Main_Blade_Traffic")
                                                        strVLANname = strVLANnameDEFAULT;
                                                        oLog.AddEvent(strName, strSerial, "Server powered off...changing virtual connect settings", LoggingType.Information);
                                                        oLog.AddEvent(strName, strSerial, "Attempting to change virtual connect settings (AssetID: " + intAsset.ToString() + ") to " + strVLANname, LoggingType.Information);
                                                        string strResultVC1 = oAsset.ExecuteVirtualConnectIP(intAsset, 0, intEnvironment, strVLANname, 1, false, false, false);
                                                        if (strResultVC1.Contains("ERROR") == true || strResultVC1 == "")
                                                        {
                                                            oLog.AddEvent(strName, strSerial, "Could not change virtual connect settings ~ (NIC#1) to " + strVLANname + "... " + strResultVC1, LoggingType.Warning);
                                                            strError = "Could not change virtual connect settings ~ (NIC#1): " + strResultVC1;
                                                        }
                                                        else
                                                            oLog.AddEvent(strName, strSerial, "Server successfully changed virtual connect settings (NIC#1) to " + strVLANname, LoggingType.Information);
                                                        if (strError != "" && strResultVC1.ToUpper().Contains("NETWORK NOT FOUND") == true)
                                                        {
                                                            // strVLANname does not exist, try using the default VLAN_<name>
                                                            strError = "";
                                                            int intNetwork = Int32.Parse(oIPAddresses.Get(intIP, "networkid"));
                                                            int intVlan = Int32.Parse(oIPAddresses.GetNetwork(intNetwork, "vlanid"));
                                                            string strVLAN = oIPAddresses.GetVlan(intVlan, "vlan");
                                                            strVLANname = "VLAN_" + strVLAN;

                                                            oLog.AddEvent(strName, strSerial, "Attempting to change virtual connect settings (AssetID: " + intAsset.ToString() + ") to " + strVLANname, LoggingType.Information);
                                                            strResultVC1 = oAsset.ExecuteVirtualConnectIP(intAsset, 0, intEnvironment, strVLANname, 1, false, false, false);
                                                            if (strResultVC1.Contains("ERROR") == true || strResultVC1 == "")
                                                            {
                                                                oLog.AddEvent(strName, strSerial, "Could not change virtual connect settings ~ (NIC#1) to " + strVLANname + "... " + strResultVC1, LoggingType.Error);
                                                                strError = "Could not change virtual connect settings ~ (NIC#1): " + strResultVC1;
                                                            }
                                                            else
                                                                oLog.AddEvent(strName, strSerial, "Server successfully changed virtual connect settings (NIC#1) to " + strVLANname, LoggingType.Information);
                                                        }
                                                        if (strError == "")
                                                        {
                                                            string strResultVC2 = oAsset.ExecuteVirtualConnectIP(intAsset, 0, intEnvironment, strVLANname, 2, false, false, false);
                                                            if (strResultVC2.Contains("ERROR") == true || strResultVC2 == "")
                                                            {
                                                                oLog.AddEvent(strName, strSerial, "Could not change virtual connect settings ~ (NIC#2) to " + strVLANname + "... " + strResultVC2, LoggingType.Error);
                                                                strError = "Could not change virtual connect settings ~ (NIC#2): " + strResultVC2;
                                                            }
                                                            else
                                                                oLog.AddEvent(strName, strSerial, "Server successfully changed virtual connect settings (NIC#2) to " + strVLANname, LoggingType.Information);
                                                        }
                                                        if (strError == "")
                                                        {
                                                            // Change successful, power back on
                                                            bool boolNetworkPowerOn = oFunction.ExecutePower(intID, intAsset, true, "Power (Network / Power On)", strName, intEnvironment, (boolDell ? strScripts : strScripts + strSub), intLogging, dsnAsset);
                                                            if (boolNetworkPowerOn == false)
                                                                strError = "There was a problem turning on the server via ILO";
                                                            else
                                                            {
                                                                // Check for Server to be back on
                                                                powVirtualConnect = oAsset.PowerStatus(intAsset, 0, intEnvironment, strScripts);
                                                                for (intAttempt = 0; intAttempt < 10 && powVirtualConnect != AssetPowerStatus.On; intAttempt++)
                                                                {
                                                                    powVirtualConnect = oAsset.PowerStatus(intAsset, 0, intEnvironment, strScripts);
                                                                    int intAttemptLeft = (10 - intAttempt);
                                                                    oLog.AddEvent(strName, strSerial, "Server still off...waiting 5 seconds (" + intAttemptLeft.ToString() + " tries left)", LoggingType.Information);
                                                                    Thread.Sleep(3000);
                                                                }
                                                                if (intAttempt == 10)
                                                                {
                                                                    // Server is still off....throw error
                                                                    strError = "There was a problem turning on the server after virtual connect settings change";
                                                                    oLog.AddEvent(strName, strSerial, strError, LoggingType.Error);
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                // Currently (8/9/2010) the VLAN is not a number, so cannot change physical switches.
                                                oVMWare.AddHostNewResult(intID, "Skipping IP Assignment (cannot change physical device)", 0);
                                                oServer.UpdateStep(intID, intStep + 1);

                                                //// 1/5/2010: added code for physical devices
                                                //oLog.AddEvent(strName, strSerial, "Attempting to reconfigure the VLAN of a switch", LoggingType.Information);
                                                //// Loop through switchports and change them to new VLAN
                                                //StringBuilder strSwitchOutput = new StringBuilder();
                                                //foreach (DataRow drSwitch in dsSwitch.Tables[0].Rows)
                                                //{
                                                //}
                                                //oServer.AddOutput(intID, "SWITCH", strSwitchOutput.ToString());
                                            }

                                            if (strError == "")
                                            {
                                                strIP = oIPAddresses.GetName(intIP, 0);
                                                // Wait 5 seconds and then ping new address
                                                oLog.AddEvent(strName, strSerial, "Attempting to ping the newly assigned address [" + strIP + "]", LoggingType.Information);
                                                bool boolPinged = false;
                                                for (int ii = 0; ii < 40 && boolPinged == false; ii++)
                                                {
                                                    Thread.Sleep(3000);
                                                    Ping oPing = new Ping();
                                                    string strStatus = "";
                                                    try
                                                    {
                                                        PingReply oReply = oPing.Send(strIP);
                                                        strStatus = oReply.Status.ToString().ToUpper();
                                                    }
                                                    catch { }
                                                    boolPinged = (strStatus == "SUCCESS");
                                                }
                                                if (boolPinged == true)
                                                {
                                                    oVMWare.AddHostNewResult(intID, "IP configuration finished", 0);
                                                    oServer.UpdateStep(intID, intStep + 1);
                                                }
                                                else
                                                {
                                                    strError = "There was a problem assigning the IP Address ~ (ping " + strIP + " did not respond to ping)";
                                                    oLog.AddEvent(strName, strSerial, strError, LoggingType.Error);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            oVMWare.AddHostNewResult(intID, "Server IP configuration has already been changed", 0);
                                            oServer.UpdateStep(intID, intStep + 1);
                                        }
                                    }
                                    break;
                                case 10:
                                    // Move into OU
                                    if (intLogging > 0)
                                        oEventLog.WriteEntry("VMWare HOST: Starting Step # " + intStep.ToString(), EventLogEntryType.Information);
                                    int intEnvironmentAD = 999;   // 999 = PNC Environment
                                    if (boolPNC == false)
                                    {
                                        if (oClass.IsProd(intClass))
                                            intEnvironmentAD = 4;   // 4 = NCB Production
                                        else
                                            intEnvironmentAD = 3;   // 3 = NCB Test
                                    }
                                    oVMWare.AddHostNewResult(intID, "Moving Active Directory Computer Object [" + intEnvironmentAD.ToString() + "]", 0);
                                    try
                                    {
                                        AD oAD = new AD(0, dsn, intEnvironmentAD);
                                        SearchResultCollection oResults = oAD.ComputerSearch(strName);
                                        for (int ii = 0; ii < 3 && oResults.Count != 1; ii++)
                                        {
                                            Thread.Sleep(4000);
                                            oResults = oAD.ComputerSearch(strName);
                                        }
                                        if (oResults.Count == 1)
                                        {
                                            // corptest.ntl-city.net/OUc_DmnCptrs/OUc_UnixSrvs/OUs_UnixStandard/ohclexnv....
                                            string strMove = "OU=OUs_UnixStandard,OU=OUc_UnixSrvs,OU=OUc_DmnCptrs,";
                                            if (strMove == "")
                                                strMove = oAD.GetComputerOU(strName);
                                            try
                                            {
                                                oAD.MoveServer(oResults[0].GetDirectoryEntry(), strMove);
                                            }
                                            catch (Exception exMove)
                                            {
                                                strError = exMove.Message;
                                            }
                                        }
                                        else
                                        {
                                            oVMWare.AddHostNewResult(intID, "Could not find the object in the domain...skipping for now...", 0);
                                            //strError = "Could not find the object in the domain";
                                        }
                                    }
                                    catch (Exception eAD)
                                    {
                                        strError = "There was a problem moving the Active Directory Host Computer Object ~ ERROR: " + eAD.Message + " (Source: " + eAD.Source + ") (Stack Trace: " + eAD.StackTrace + ")";
                                    }
                                    if (strError == "")
                                    {
                                        oVMWare.AddHostNewResult(intID, "Active Directory Computer Object Moved", 0);
                                        oServer.UpdateStep(intID, intStep + 1);
                                    }
                                    break;
                                case 11:
                                    // (Virtual Center) Add Host to Virtual Center
                                    if (intLogging > 0)
                                        oEventLog.WriteEntry("VMWare HOST: Starting Step # " + intStep.ToString(), EventLogEntryType.Information);
                                    if (boolVirtualCenter == false)
                                    {
                                        oVMWare.AddHostNewResult(intID, "Virtual Center Configuration SKIPPED. Moving to initiate work orders...", 0);
                                        oServer.UpdateStep(intID, 19);
                                    }
                                    else
                                    {
                                        HostConnectSpec oSpec = new HostConnectSpec();
                                        oSpec.force = true;
                                        if (boolUseIP == true)
                                            oSpec.hostName = strIP;
                                        else
                                            oSpec.hostName = strNameHost;
                                        oSpec.userName = "root";
                                        oSpec.password = "vm1234";
                                        ManagedObjectReference _task1 = _service.AddStandaloneHost_Task(hostFolderRef, oSpec, null, true, null);
                                        strError = WaitForExit(oVMWare, _task1, intID, intStep);
                                        if (strError == "")
                                        {
                                            oVMWare.AddHostNewResult(intID, "Host Added to Virtual Center: " + strVirtualCenter, 0);
                                            oServer.UpdateStep(intID, intStep + 1);
                                        }
                                    }
                                    break;
                                case 12:
                                    // (Virtual Center) Rename to Host"
                                    if (intLogging > 0)
                                        oEventLog.WriteEntry("VMWare HOST: Starting Step # " + intStep.ToString(), EventLogEntryType.Information);
                                    if (boolUseIP == true)
                                    {
                                        ManagedObjectReference _task_rename = _service.Rename_Task(hostRef, strNameHost);
                                        strError = WaitForExit(oVMWare, _task_rename, intID, intStep);
                                        if (strError == "")
                                        {
                                            oVMWare.AddHostNewResult(intID, "Host Renamed from " + strIP + " to " + strNameHost, 0);
                                            oServer.UpdateStep(intID, intStep + 1);
                                        }
                                    }
                                    else
                                    {
                                        oVMWare.AddHostNewResult(intID, "Host was added with full DNS name - no rename required", 0);
                                        oServer.UpdateStep(intID, intStep + 1);
                                    }
                                    break;
                                case 13:
                                    // (Virtual Center) Remove "VM Network"
                                    if (intLogging > 0)
                                        oEventLog.WriteEntry("VMWare HOST: Starting Step # " + intStep.ToString(), EventLogEntryType.Information);
                                    try
                                    {
                                        _service.RemovePortGroup(networkSystem, "VM Network");
                                    }
                                    catch { }
                                    oServer.UpdateStep(intID, intStep + 1);
                                    oVMWare.AddHostNewResult(intID, "VM Network Port Group Removed", 0);
                                    break;
                                case 14:
                                    // (Virtual Center) Reconfigure vSwitch0 for NIC teaming
                                    if (intLogging > 0)
                                        oEventLog.WriteEntry("VMWare HOST: Starting Step # " + intStep.ToString(), EventLogEntryType.Information);
                                    try
                                    {
                                        HostVirtualSwitchSpec vswitch0Spec = new HostVirtualSwitchSpec();
                                        vswitch0Spec.numPorts = 64;
                                        HostVirtualSwitchBondBridge vswitch0Bridge = new HostVirtualSwitchBondBridge();
                                        vswitch0Bridge.beacon = beaconConfig;
                                        vswitch0Bridge.nicDevice = nics1;
                                        vswitch0Spec.bridge = vswitch0Bridge;
                                        vswitch0Spec.policy = GetHostNetworkPolicy(nics1);
                                        _service.UpdateVirtualSwitch(networkSystem, "vSwitch0", vswitch0Spec);
                                    }
                                    catch (Exception ex13)
                                    {
                                        strError = ex13.Message;
                                    }
                                    if (strError == "")
                                    {
                                        oServer.UpdateStep(intID, intStep + 1);
                                        oVMWare.AddHostNewResult(intID, "vSwitch0 Reconfigured", 0);
                                    }
                                    break;
                                case 15:
                                    // (Virtual Center) Configure VMotion
                                    if (intLogging > 0)
                                        oEventLog.WriteEntry("VMWare HOST: Starting Step # " + intStep.ToString(), EventLogEntryType.Information);
                                    try
                                    {
                                        HostPortGroupSpec oHostPortGroupSpec = new HostPortGroupSpec();
                                        oHostPortGroupSpec.name = "VMkernel";
                                        oHostPortGroupSpec.vlanId = 0;
                                        oHostPortGroupSpec.vswitchName = "vSwitch0";
                                        oHostPortGroupSpec.policy = new HostNetworkPolicy();
                                        _service.AddPortGroup(networkSystem, oHostPortGroupSpec);

                                        HostVirtualNicSpec oHostVirtualNicSpec = new HostVirtualNicSpec();
                                        HostIpConfig oHostIpConfig = new HostIpConfig();
                                        oHostIpConfig.dhcp = false;
                                        oHostIpConfig.ipAddress = strIPVMotion;
                                        oHostIpConfig.subnetMask = "255.255.255.0";
                                        oHostVirtualNicSpec.ip = oHostIpConfig;
                                        _service.AddVirtualNic(networkSystem, "VMkernel", oHostVirtualNicSpec);

                                        HostNetworkInfo networkInfo = (HostNetworkInfo)oVMWare.getObjectProperty(networkSystem, "networkInfo");
                                        HostVirtualNic[] hostVirtualNics = networkInfo.vnic;
                                        HostVirtualNic hostVirtualNic = hostVirtualNics[0];
                                        _service.SelectVnic(vmotionSystem, hostVirtualNic.device);

                                        HostIpRouteConfig oHostIpRouteConfig = new HostIpRouteConfig();
                                        oHostIpRouteConfig.defaultGateway = strIPVMotionGateway;
                                        _service.UpdateIpRouteConfig(networkSystem, oHostIpRouteConfig);
                                    }
                                    catch (Exception ex14)
                                    {
                                        strError = ex14.Message;
                                    }
                                    if (strError == "")
                                    {
                                        oVMWare.AddHostNewResult(intID, "VMotion Configured", 0);
                                        oServer.UpdateStep(intID, intStep + 1);
                                    }
                                    break;
                                case 16:
                                    // (Virtual Center) Add vSwitch1
                                    if (intLogging > 0)
                                        oEventLog.WriteEntry("VMWare HOST: Starting Step # " + intStep.ToString(), EventLogEntryType.Information);
                                    try
                                    {
                                        HostVirtualSwitchSpec vswitch1Spec = new HostVirtualSwitchSpec();
                                        vswitch1Spec.numPorts = 64;
                                        HostVirtualSwitchBondBridge vswitch1Bridge = new HostVirtualSwitchBondBridge();
                                        vswitch1Bridge.beacon = beaconConfig;
                                        vswitch1Bridge.nicDevice = nics2;
                                        vswitch1Spec.bridge = vswitch1Bridge;
                                        vswitch1Spec.policy = GetHostNetworkPolicy(nics2);
                                        _service.AddVirtualSwitch(networkSystem, "vSwitch1", vswitch1Spec);
                                    }
                                    catch (Exception ex15)
                                    {
                                        strError = ex15.Message;
                                    }
                                    if (strError == "")
                                    {
                                        oVMWare.AddHostNewResult(intID, "vSwitch1 Added", 0);
                                        oServer.UpdateStep(intID, intStep + 1);
                                    }
                                    break;
                                case 17:
                                    // (Virtual Center) Add Port Groups to vSwitch1
                                    if (intLogging > 0)
                                        oEventLog.WriteEntry("VMWare HOST: Starting Step # " + intStep.ToString(), EventLogEntryType.Information);
                                    try
                                    {
                                        DataSet dsPort = oVMWare.GetVlanAssociationsCluster(intCluster);
                                        foreach (DataRow drPort in dsPort.Tables[0].Rows)
                                        {
                                            HostPortGroupSpec oHostPortGroupSpecSWITCH1 = new HostPortGroupSpec();
                                            oHostPortGroupSpecSWITCH1.name = drPort["name"].ToString();
                                            oHostPortGroupSpecSWITCH1.vlanId = Int32.Parse(drPort["vlan"].ToString());
                                            oHostPortGroupSpecSWITCH1.vswitchName = "vSwitch1";
                                            oHostPortGroupSpecSWITCH1.policy = GetHostNetworkPolicy(nics2);
                                            _service.AddPortGroup(networkSystem, oHostPortGroupSpecSWITCH1);
                                        }
                                    }
                                    catch (Exception ex16)
                                    {
                                        strError = ex16.Message;
                                    }
                                    if (strError == "")
                                    {
                                        oVMWare.AddHostNewResult(intID, "VLAN Port Groups Added", 0);
                                        oServer.UpdateStep(intID, intStep + 1);
                                    }
                                    break;
                                case 18:
                                    // (Virtual Center) Remove from Maintenance / Join Cluster
                                    if (intLogging > 0)
                                        oEventLog.WriteEntry("VMWare HOST: Starting Step # " + intStep.ToString(), EventLogEntryType.Information);
                                    if (boolJoin == true)
                                    {
                                        ManagedObjectReference _task10 = _service.MoveHostInto_Task(clusterRef, hostRef, resourcePoolRootRef);
                                        strError = WaitForExit(oVMWare, _task10, intID, intStep);
                                    }
                                    if (strError == "" && boolMaintenance == true)
                                    {
                                        HostListSummary hostSummaryEnd = (HostListSummary)oVMWare.getObjectProperty(hostRef, "summary");
                                        if (hostSummaryEnd.runtime.inMaintenanceMode == true)
                                        {
                                            ManagedObjectReference _task11 = _service.ExitMaintenanceMode_Task(hostRef, 0);
                                            strError = WaitForExit(oVMWare, _task11, intID, intStep);
                                        }
                                    }
                                    if (strError == "")
                                    {
                                        oVMWare.AddHostNewResult(intID, "Server Removed from Maintenance / Join Cluster", 0);
                                        oServer.UpdateStep(intID, intStep + 1);
                                    }
                                    break;
                                case 19:
                                    // Generate task to perform manual tasks (storage, join cluster, etc...)
                                    oVMWare.AddHostNewResult(intID, "Generating work order(s)...", 0);
                                    //Pause the server process to complete the workorder for adding Cluster or Host
                                    int intResource = PauseServerProcessAndInitiateWorkOrders(intID, intRequestId);
                                    if (intResource > 0 || intResource == -1)
                                    {
                                        if (intResource > 0)
                                            oVMWare.AddHostNewResult(intID, "Work Order Generated!", 0);
                                        else
                                            oVMWare.AddHostNewResult(intID, "Waiting for all servers to be done building...", 0);
                                        oServer.UpdateStep(intID, intStep + 1);
                                    }
                                    else
                                        strError = "There was a problem generating the work order (" + intResource.ToString() + ")";
                                    break;
                                case 20:
                                    // Cleanup installation files and send notifications
                                    if (intLogging > 0)
                                        oEventLog.WriteEntry("VMWare HOST: Starting Step # " + intStep.ToString(), EventLogEntryType.Information);
                                    string strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_INVENTORY_MANAGER");
                                    oFunction.SendEmail("Auto-Provisioning VMWare Host Noticiation", strEMailIdsBCC, "", "", "Auto-Provisioning VMWare Host Noticiation", "<p><b>This message is to inform you that a VMware Host has finished provisioning!</b><p><p>NAME: " + strName + "<br/>Virtual Center Server: " + strVirtualCenter + "</p><p><b>NOTE:</b> This is automated email sent from the ClearView Auto-Provisioning tool. Please do not respond to this message.</p>", true, false);
                                    oVMWare.AddHostNewResult(intID, "VMWare Host " + strName + " Finished", 0);
                                    oServer.UpdateStep(intID, 999);
                                    break;
                            }
                            // LOGOUT VMWARE SERVICE!!!
                            if (_service != null)
                            {
                                ServiceContent _sic = oVMWare.GetSic();
                                _service.Abort();
                                if (_service.Container != null)
                                    _service.Container.Dispose();
                                try
                                {
                                    _service.Logout(_sic.sessionManager);
                                }
                                catch { }
                                _service.Dispose();
                                _service = null;
                                _sic = null;
                            }
                        }
                        else if (intLogging > 1)
                            oEventLog.WriteEntry("Not able to process at this time", EventLogEntryType.Warning);
                    }
                    else
                    {
                        // Invalid configuration
                        strError = "There is no build location for the VMware Folder ~ " + oVMWare.GetFolder(intFolder, "name");
                    }

                    if (strError != "")
                    {
                        oVMWare.AddHostNewResult(intID, strError, 1);
                        int intError = oServer.AddError(0, 0, 0, intID, intStep, strError);
                        if (intLogging > 0)
                            oEventLog.WriteEntry(strError, EventLogEntryType.Error);
                        string strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DEVELOPER_ERROR,EMAILGRP_PROVISIONING_SUPPORT,EMAILGRP_INVENTORY_MANAGER");
                        oFunction.SendEmail("Auto-Provisioning VMWare Host ERROR", strEMailIdsBCC, "", "", "Auto-Provisioning VMWare Host ERROR", "<p><b>This message is to inform you that a VMware Host has encountered an error and has been stopped!</b><p><p>" + strError + "</p><p><b>NOTE:</b> This is automated email sent from the ClearView Auto-Provisioning tool. Please do not respond to this message.</p>", true, false);
                    }
                }
            }
            catch (Exception ex)
            {
                string strError = "VMWare Host Service: " + "(Error Message: " + ex.Message + ") ~ (Source: " + ex.Source + ") (Stack Trace: " + ex.StackTrace + ") [" + System.Environment.UserName + "]";
                oEventLog.WriteEntry(strError, EventLogEntryType.Error);
                if (_service != null)
                {
                    ServiceContent _sic = oVMWare.GetSic();
                    _service.Abort();
                    if (_service.Container != null)
                        _service.Container.Dispose();
                    try
                    {
                        _service.Logout(_sic.sessionManager);
                    }
                    catch { }
                    _service.Dispose();
                    _service = null;
                    _sic = null;
                }
            }
        }

        public string WaitForExit(VMWare _vmware, ManagedObjectReference _task, int _id, int _step)
        {
            string strReturn = "";
            TaskInfo _info = (TaskInfo)_vmware.getObjectProperty(_task, "info");
            while (_info.state == TaskInfoState.running)
                _info = (TaskInfo)_vmware.getObjectProperty(_task, "info");
            if (_info.state == TaskInfoState.error)
            {
                LocalizedMethodFault _error = _info.error;
                strReturn = "Error on Step # " + _step.ToString() + " ~ " + _error.localizedMessage;
            }
            return strReturn;
        }
        public HostNetworkPolicy GetHostNetworkPolicy(string[] _nics)
        {
            HostNetworkPolicy networkPolicy = new HostNetworkPolicy();
            HostNicOrderPolicy oHostNicOrderPolicy = new HostNicOrderPolicy();
            oHostNicOrderPolicy.activeNic = _nics;
            HostNicFailureCriteria oHostNicFailureCriteria = new HostNicFailureCriteria();
            oHostNicFailureCriteria.checkBeacon = false;
            oHostNicFailureCriteria.checkBeaconSpecified = true;
            oHostNicFailureCriteria.checkDuplex = false;
            oHostNicFailureCriteria.checkDuplexSpecified = true;
            oHostNicFailureCriteria.checkErrorPercent = false;
            oHostNicFailureCriteria.checkErrorPercentSpecified = true;
            oHostNicFailureCriteria.checkSpeed = "minimum";
            oHostNicFailureCriteria.fullDuplex = false;
            oHostNicFailureCriteria.fullDuplexSpecified = true;
            oHostNicFailureCriteria.percentage = 0;
            oHostNicFailureCriteria.percentageSpecified = true;
            oHostNicFailureCriteria.speed = 10;
            oHostNicFailureCriteria.speedSpecified = true;
            HostNicTeamingPolicy oHostNicTeamingPolicy = new HostNicTeamingPolicy();
            oHostNicTeamingPolicy.notifySwitches = true;
            oHostNicTeamingPolicy.notifySwitchesSpecified = true;
            oHostNicTeamingPolicy.reversePolicy = true;
            oHostNicTeamingPolicy.reversePolicySpecified = true;
            oHostNicTeamingPolicy.rollingOrder = false;
            oHostNicTeamingPolicy.rollingOrderSpecified = true;
            oHostNicTeamingPolicy.policy = "failover_explicit";
            oHostNicTeamingPolicy.nicOrder = oHostNicOrderPolicy;
            oHostNicTeamingPolicy.failureCriteria = oHostNicFailureCriteria;
            HostNetOffloadCapabilities oHostNetOffloadCapabilities = new HostNetOffloadCapabilities();
            oHostNetOffloadCapabilities.csumOffload = true;
            oHostNetOffloadCapabilities.csumOffloadSpecified = true;
            oHostNetOffloadCapabilities.tcpSegmentation = true;
            oHostNetOffloadCapabilities.tcpSegmentationSpecified = true;
            oHostNetOffloadCapabilities.zeroCopyXmit = true;
            oHostNetOffloadCapabilities.zeroCopyXmitSpecified = true;
            HostNetworkSecurityPolicy oHostNetworkSecurityPolicy = new HostNetworkSecurityPolicy();
            oHostNetworkSecurityPolicy.allowPromiscuous = false;
            oHostNetworkSecurityPolicy.allowPromiscuousSpecified = true;
            oHostNetworkSecurityPolicy.forgedTransmits = true;
            oHostNetworkSecurityPolicy.forgedTransmitsSpecified = true;
            oHostNetworkSecurityPolicy.macChanges = true;
            oHostNetworkSecurityPolicy.macChangesSpecified = true;
            HostNetworkTrafficShapingPolicy oHostNetworkTrafficShapingPolicy = new HostNetworkTrafficShapingPolicy();
            oHostNetworkTrafficShapingPolicy.enabled = false;
            oHostNetworkTrafficShapingPolicy.enabledSpecified = true;
            networkPolicy.nicTeaming = oHostNicTeamingPolicy;
            networkPolicy.offloadPolicy = oHostNetOffloadCapabilities;
            networkPolicy.security = oHostNetworkSecurityPolicy;
            networkPolicy.shapingPolicy = oHostNetworkTrafficShapingPolicy;
            return networkPolicy;
        }
        public ManagedObjectReference GetMOR(VMWare _vmware, string _name, string _type)
        {
            VimService _service = _vmware.GetService();
            ServiceContent _sic = _vmware.GetSic();
            ManagedObjectReference datacenterRef = _vmware.GetDataCenter();
            ManagedObjectReference returnRef = new ManagedObjectReference();
            // Get Other objects
            TraversalSpec dc2hf = new TraversalSpec();
            dc2hf.type = "Datacenter";
            dc2hf.name = "dc2hf";
            dc2hf.path = "hostFolder";
            dc2hf.skip = false;
            dc2hf.skipSpecified = true;
            TraversalSpec cr2h = new TraversalSpec();
            cr2h.type = "ComputeResource";
            cr2h.name = "cr2h";
            cr2h.path = "host";
            cr2h.skip = false;
            TraversalSpec ccr2h = new TraversalSpec();
            ccr2h.type = "ClusterComputeResource";
            ccr2h.name = "ccr2h";
            ccr2h.path = "host";
            ccr2h.skip = false;
            TraversalSpec f2ce = new TraversalSpec();
            f2ce.type = "Folder";
            f2ce.name = "f2ce";
            f2ce.path = "childEntity";
            f2ce.skip = false;
            dc2hf.skipSpecified = true;
            //Then setup your recursion with the potential "depth" you need. For example a DataCenter only needs to go to a Folder. A folder though needs to be able to goto another folder, a Datacenter, a ComputeResource and a ClusterComputeResource 
            dc2hf.selectSet = new SelectionSpec[] { new SelectionSpec() };
            dc2hf.selectSet[0].name = "f2ce";
            f2ce.selectSet = new SelectionSpec[] { new SelectionSpec(), new SelectionSpec(), new SelectionSpec(), new SelectionSpec() };
            f2ce.selectSet[0].name = "f2ce";
            f2ce.selectSet[1].name = "cr2h";
            f2ce.selectSet[2].name = "dc2hf";
            f2ce.selectSet[3].name = "ccr2h";
            //Then combine your TrversalSpecs into an array
            TraversalSpec[] tsa = new TraversalSpec[] { f2ce, cr2h, dc2hf, ccr2h };
            //Define your properties. Name for this example, but you could do any properties or All
            PropertySpec[] psa = new PropertySpec[] { new PropertySpec() };
            psa[0].all = false;
            psa[0].allSpecified = true;
            psa[0].pathSet = new string[] { "name" };
            psa[0].type = "ManagedEntity";
            //Then combine your traversal spec and your propertyspec into your "object collector"
            PropertyFilterSpec pfs = new PropertyFilterSpec();
            pfs.propSet = psa;
            pfs.objectSet = new ObjectSpec[] { new ObjectSpec() };
            pfs.objectSet[0].obj = _sic.rootFolder;
            pfs.objectSet[0].skip = false;
            pfs.objectSet[0].selectSet = tsa;
            //Run it through VC
            ObjectContent[] oca = _service.RetrieveProperties(_sic.propertyCollector, new PropertyFilterSpec[] { pfs });
            foreach (ObjectContent oc in oca)
            {
                if (oc.obj.type.ToUpper() == _type && ((string)_vmware.getObjectProperty(oc.obj, "name")).ToUpper() == _name.ToUpper())
                    returnRef = oc.obj;
            }
            return returnRef;
        }

        public int PauseServerProcessAndInitiateWorkOrders(int _ServerId,int _RequestId)
        {
            int intResource = 0;
            Servers oServer = new Servers(0, dsn);
            Services oService = new Services(0, dsn);
            ServiceRequests oServiceRequest = new ServiceRequests(0, dsn);
            ResourceRequest oResourceRequest = new ResourceRequest(0, dsn);
            ServiceDetails oServiceDetail = new ServiceDetails(0, dsn);
            AssetSharedEnvOrder oAssetSharedEnvOrder = new AssetSharedEnvOrder(0, dsn, dsnAsset, intEnvironment);
            int intServiceId = 0;
            string strServerTitle = "";
            double dblServiceHours = 0;
            int intItemId = 0;
            int intNumber = 1;
            Boolean boolAllServersForRequestPaused = true;

            oServer.UpdatePause(_ServerId, 1);

            DataSet dsServer = oServer.GetRequests(_RequestId, 1);
            foreach (DataRow dr in dsServer.Tables[0].Rows)
            {
                if (dr["paused"].ToString() == "0")
                {
                    boolAllServersForRequestPaused = false;
                    break;
                }
            }

            if (boolAllServersForRequestPaused == true)
            {
                DataSet dsOrder = oAssetSharedEnvOrder.Get(_RequestId, intNumber);
                if (dsOrder.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = dsOrder.Tables[0].Rows[0];

                    if (Int32.Parse(dr["OrderType"].ToString()) == (int)AssetSharedEnvOrderType.AddCluster)
                    {
                        intServiceId = intServiceIdAssetSharedEnvAddCluster;
                        strServerTitle = "Inventory Management - Shared Environment Add Cluster Asset(s)";
                    }
                    else if (Int32.Parse(dr["OrderType"].ToString()) == (int)AssetSharedEnvOrderType.AddHost)
                    {

                        intServiceId = intServiceIdAssetSharedEnvAddHost;
                        strServerTitle = "Inventory Management - Shared Environment Add Host Asset(s)";
                    }

                    if (intServiceId > 0)
                    {
                        intItemId = oService.GetItemId(intServiceId);
                        dblServiceHours = oServiceDetail.GetHours(intServiceId, 1);
                        if (oServiceRequest.Get(_RequestId).Tables[0].Rows.Count == 0)
                            oServiceRequest.Add(_RequestId, 1, 1);
                        intResource = oServiceRequest.AddRequest(_RequestId, intItemId, intServiceId, 1, dblServiceHours, 2, intNumber, dsnServiceEditor);
                        oServiceRequest.Update(_RequestId, strServerTitle);
                        oResourceRequest.UpdateName(intResource, strServerTitle);
                    }

                }
                else
                    intResource = -10;
            }
            else
                intResource = -1;
            return intResource;
        }
    }
}
