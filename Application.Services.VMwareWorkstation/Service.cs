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
using ActiveDs;
using Vim25Api;
using System.Web.Services;
using System.Net.NetworkInformation;
using System.Net;
using NCC.ClearView.Application.Core.w08r2;

namespace ClearViewAP_VMware_Workstation
{
    public class XenConfig
    {
        public int AddressID { get; set; }
        public string MachineCatalogName { get; set; }
        public string DesktopDeliveryGroup { get; set; }
        public string ADGroup { get; set; }
        public string[] Controllers { get; set; }
        public XenConfig(int _AddressID, string _MachineCatalogName, string _DesktopDeliveryGroup, string _ADGroup, string[] _Controllers)
        {
            AddressID = _AddressID;
            MachineCatalogName = _MachineCatalogName;
            DesktopDeliveryGroup = _DesktopDeliveryGroup;
            ADGroup = _ADGroup;
            Controllers = _Controllers;
        }
    }
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

    public enum XenConfigType
    {
        Add, Remove, MaintOn, MaintOff
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
        private string dsnIP;
        private string dsnServiceEditor;
        private string dsnZeus;
        private int intEnvironment;
        private int intProduction;
        private int intWorkstationType = 0;
        private int intLogging;
        private EventLog oEventLog;
        private Log oLog;
        private string strScripts = "E:\\APPS\\CLV\\ClearViewAP_VMware_Workstation\\";
        private string strSub = "scripts\\";
        private string strZeus = "";
        private string strEMailIdsBCC = "";
        private string strCC = "";
        private string strVSG = "";
        private string strDomainsList = "";
        private string strDomainDefault = "";
        private string strSource = "PROD";
        private string strAdminUser = "";
        private string strAdminPass = "";
        private int intBuildTest = 0;
        private int intBuildQA = 0;
        private int intBuildQAOffHours = 0;
        private int intBuildProd = 0;
        private int intBuildProdOffHours = 0;
        private int intDesignOverride = 0;
        private int intProvisioningErrorService = 0;
        private int intDecomErrorService = 0;
        private int intDestroyErrorService = 0;
        private bool boolProvisioningErrorEmail = false;
        private bool boolDecommissionErrorEmail = false;
        private bool boolNotifyDecom = false;
        private int intResourceRequestApprove = 0;
        private int intAssignPage = 0;
        private int intViewPage = 0;
        private int intJoelsService = 0;

        private int intErrorWorkstation = 0;
        private int intErrorStep = 0;
        private int intErrorAsset = 0;
        private int intErrorModel = 0;
        private VMWare oErrorVMWare;
        private int intTimeoutDefault = (5 * 60 * 1000);    // 5 minutes
        private int intTimeoutInstall = (30 * 60 * 1000);   // 30 minutes
        private double dblDrive2 = 0.00;    // 2.5 or 0.00 to disable
        private char[] strSplit = { ';' };
        private string strDecomSuffix = "-DECOM";
        private string strRebuildSuffix = "-REBUILD";

        private int intStepDatastore = 3;   // Assign Datastore
        private int intStepCreate = 6;      // Create Machine
        private string strINIProd = @"\\citrixfile.ntl-city.com\flexcfg$\vwcfg\VW.INI";
        private string strINITest = @"\\citrixfiletest.tstctr.ntl-city.net\flexcfg$\vwcfg\VW.INI";
        public List<XenConfig> XenConfigs;

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
                intProduction = Int32.Parse(ds.Tables[0].Rows[0]["productionid"].ToString());
                intWorkstationType = Int32.Parse(ds.Tables[0].Rows[0]["typeid"].ToString());
                string strDSN = ds.Tables[0].Rows[0]["DSN"].ToString();
                string strDSNAsset = ds.Tables[0].Rows[0]["AssetDSN"].ToString();
                string strDSNIP = ds.Tables[0].Rows[0]["IpDSN"].ToString();
                string strDSNServiceEditor = ds.Tables[0].Rows[0]["ServiceEditorDSN"].ToString();
                string strDSNZeus = ds.Tables[0].Rows[0]["ZeusDSN"].ToString();
                dsn = ds.Tables[0].Rows[0][strDSN].ToString();
                dsnAsset = ds.Tables[0].Rows[0][strDSNAsset].ToString();
                dsnIP = ds.Tables[0].Rows[0][strDSNIP].ToString();
                dsnServiceEditor = ds.Tables[0].Rows[0][strDSNServiceEditor].ToString();
                dsnZeus = ds.Tables[0].Rows[0][strDSNZeus].ToString();
                strZeus = ds.Tables[0].Rows[0]["zeus"].ToString();
                strVSG = ds.Tables[0].Rows[0]["vsg"].ToString();
                strDomainsList = ds.Tables[0].Rows[0]["domains"].ToString();
                strDomainDefault = ds.Tables[0].Rows[0]["domain_default"].ToString();
                intBuildTest = Int32.Parse(ds.Tables[0].Rows[0]["build_test"].ToString());
                intBuildQA = Int32.Parse(ds.Tables[0].Rows[0]["build_qa"].ToString());
                intBuildQAOffHours = Int32.Parse(ds.Tables[0].Rows[0]["build_qa_off_hours"].ToString());
                intBuildProd = Int32.Parse(ds.Tables[0].Rows[0]["build_prod"].ToString());
                intBuildProdOffHours = Int32.Parse(ds.Tables[0].Rows[0]["build_prod_off_hours"].ToString());
                intDesignOverride = Int32.Parse(ds.Tables[0].Rows[0]["design_override"].ToString());
                intProvisioningErrorService = Int32.Parse(ds.Tables[0].Rows[0]["provisioning_error_service"].ToString());
                intDecomErrorService = Int32.Parse(ds.Tables[0].Rows[0]["decom_error_service"].ToString());
                intDestroyErrorService = Int32.Parse(ds.Tables[0].Rows[0]["destroy_error_service"].ToString());
                boolProvisioningErrorEmail = (ds.Tables[0].Rows[0]["provisioning_error_email"].ToString() == "1");
                boolDecommissionErrorEmail = (ds.Tables[0].Rows[0]["decommission_error_email"].ToString() == "1");
                boolNotifyDecom = (ds.Tables[0].Rows[0]["NOTIFY_DECOM"].ToString() == "1");
                intResourceRequestApprove = Int32.Parse(ds.Tables[0].Rows[0]["rr_approve"].ToString());
                intAssignPage = Int32.Parse(ds.Tables[0].Rows[0]["assign_page"].ToString());
                intViewPage = Int32.Parse(ds.Tables[0].Rows[0]["view_page"].ToString());
                intJoelsService = Int32.Parse(ds.Tables[0].Rows[0]["joels_service"].ToString());
                dblDrive2 = double.Parse(ds.Tables[0].Rows[0]["drive2"].ToString());
                intLogging = Int32.Parse(ds.Tables[0].Rows[0]["logging"].ToString());
                strINIProd = ds.Tables[0].Rows[0]["INI_PROD"].ToString();
                XenConfigs = new List<XenConfig>();
                XenConfigs.Add(new XenConfig(715, "Windows 7 VDI CleOps", "Windows 7 VDI CleOps", "GSLcxiSP_CXI_Windows7_VDI_CleOps", new string[] { "WDCXI109A", "WDCXI110A" }));
                XenConfigs.Add(new XenConfig(1675, "Windows 7 VDI Summit", "Windows 7 VDI Summit", "GSLcxiSP_CXI_Windows7_VDI_Summit", new string[] { "WDCXI109A", "WDCXI110A" }));
                oLog = new Log(0, dsn); 
                oTimer = new System.Timers.Timer(dblInterval);
                oTimer.Elapsed += new ElapsedEventHandler(this.ServiceTimer_Tick);
            }
            catch
            {
                EventLog.WriteEntry(String.Format("ClearView AP VMware Workstation Service initialization has failed - INVALID XML FILE"), EventLogEntryType.Error);
            }
        }

        protected override void OnStart(string[] args)
        {
            oEventLog.WriteEntry(String.Format("ClearView AP VMware Workstation Service started."), EventLogEntryType.Information);
            oTimer.AutoReset = true;
            oTimer.Enabled = true;
            oTimer.Start();
        }

        protected override void OnStop()
        {
            oEventLog.WriteEntry(String.Format("ClearView AP VMware Workstation Service stopped."), EventLogEntryType.Information);
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
                oEventLog.WriteEntry(String.Format("ClearView AP VMware Workstation Service TICK."), EventLogEntryType.Information);

            // Start Main Processing
            try
            {
                ServiceTick();

                // Start Installations
                ThreadStart oJob = new ThreadStart(InstallTick);
                Thread oThreadJob = new Thread(oJob);
                oThreadJob.Start();

                // Start Decommissions
                ServiceTickDecom();

                // *******************************************
                // ************  END Processing  *************
                // *******************************************
                oTimer.Start();
            }
            catch (Exception ex)
            {
                string strError = "VMWare Workstation Service (TICK): " + "(Error Message: " + ex.Message + ") ~ (Source: " + ex.Source + ") (Stack Trace: " + ex.StackTrace + ") [" + System.Environment.UserName + "]";
                oEventLog.WriteEntry(strError, EventLogEntryType.Error);
            }
        }
        private void ServiceTick()
        {
            VimService _service = new VimService();
            VMWare oVMWare = new VMWare(0, dsn);
            try
            {
                Workstations oWorkstation = new Workstations(0, dsn);
                DataSet ds = oWorkstation.GetVirtualTypes(intWorkstationType);
                if (intLogging > 1)
                {
                    if (ds.Tables[0].Rows.Count == 0)
                        oEventLog.WriteEntry("No VMware workstations to build", EventLogEntryType.Information);
                    else
                        oEventLog.WriteEntry("There are " + ds.Tables[0].Rows.Count.ToString() + " VMware workstations to build", EventLogEntryType.Information);
                }
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    bool boolProcess = false;
                    oVMWare = new VMWare(0, dsn);
                    Requests oRequest = new Requests(0, dsn);
                    Forecast oForecast = new Forecast(0, dsn);
                    Models oModel = new Models(0, dsn);
                    Types oType = new Types(0, dsn);
                    ModelsProperties oModelsProperties = new ModelsProperties(0, dsn);
                    OnDemand oOnDemand = new OnDemand(0, dsn);
                    Asset oAsset = new Asset(0, dsnAsset);
                    Classes oClass = new Classes(0, dsn);
                    Environments oEnvironment = new Environments(0, dsn);
                    Domains oDomain = new Domains(0, dsn);
                    OperatingSystems oOperatingSystem = new OperatingSystems(0, dsn);
                    ServicePacks oServicePack = new ServicePacks(0, dsn);
                    Services oService = new Services(0, dsn);
                    Zeus oZeus = new Zeus(0, dsnZeus);
                    Users oUser = new Users(0, dsn);
                    AccountRequest oAccountRequest = new AccountRequest(0, dsn);
                    Functions oFunction = new Functions(0, dsn, intEnvironment);
                    Variables oVariable = new Variables(intEnvironment);
                    OnDemandTasks oOnDemandTasks = new OnDemandTasks(0, dsn);
                    ResourceRequest oResourceRequest = new ResourceRequest(0, dsn);
                    IPAddresses oIPAddresses = new IPAddresses(0, dsnIP, dsn);
                    Storage oStorage = new Storage(0, dsn);
                    Host oHost = new Host(0, dsn);
                    Locations oLocation = new Locations(0, dsn);
                    Mnemonic oMnemonic = new Mnemonic(0, dsn);
                    BuildLocation oBuildLocation = new BuildLocation(0, dsn);
                    VirtualHDD oVirtualHDD = new VirtualHDD(0, dsn);
                    VirtualRam oVirtualRam = new VirtualRam(0, dsn);
                    VirtualCPU oVirtualCPU = new VirtualCPU(0, dsn);
                    Resiliency oResiliency = new Resiliency(0, dsn);
                    
                    int intWorkstation = Int32.Parse(dr["id"].ToString());
                    intErrorWorkstation = intWorkstation;
                    int intAnswer = Int32.Parse(dr["answerid"].ToString());
                    string strManager = oForecast.GetAnswer(intAnswer, "appcontact");
                    if (strManager == "" || strManager == "0")
                        strManager = "N / A";
                    else
                        strManager = oUser.GetName(Int32.Parse(strManager)) + " - " + oUser.GetFullName(Int32.Parse(strManager));
                    bool boolWindows = true;
                    int intNumber = Int32.Parse(dr["number"].ToString());
                    int intRequest = oForecast.GetRequestID(intAnswer, true);
                    int intRequestService = Int32.Parse(dr["serviceid"].ToString());
                    int intProject = oRequest.GetProjectNumber(intRequest);
                    int intUser = Int32.Parse(oForecast.GetAnswer(intAnswer, "userid"));
                    int intMnemonic = Int32.Parse(oForecast.GetAnswer(intAnswer, "mnemonicid"));
                    int intStep = Int32.Parse(dr["step"].ToString());
                    intErrorStep = intStep;
                    int intClass = Int32.Parse(dr["classid"].ToString());
                    int intEnv = Int32.Parse(dr["environmentid"].ToString());
                    int intAddress = Int32.Parse(dr["addressid"].ToString());
                    XenConfig xenConfig = GetXenConfig(intAddress);
                    bool boolZeusError = (dr["zeus_error"].ToString() == "1");
                    int intDomain = Int32.Parse(dr["domainid"].ToString());
                    int intRAM = Int32.Parse(dr["ramid"].ToString());
                    string strRAM = oVirtualRam.Get(intRAM, "value");
                    int intCPU = Int32.Parse(dr["cpuid"].ToString());
                    string strCPU = oVirtualCPU.Get(intCPU, "value");
                    int intHDD = Int32.Parse(dr["hddid"].ToString());
                    string strHDD = oVirtualHDD.Get(intHDD, "value");
                    int intRecovery = Int32.Parse(dr["recovery"].ToString());
                    int intInternal = Int32.Parse(dr["internal"].ToString());
                    int intNetwork = Int32.Parse(dr["networkid"].ToString());
                    string strDomain = oDomain.Get(intDomain, "zeus");
                    int intDomainEnvironment = Int32.Parse(oDomain.Get(intDomain, "environment"));
                    bool GFB = (intDomainEnvironment >= 800 && intDomainEnvironment <= 899);
                    Variables oVar = new Variables(intDomainEnvironment);
                    AD oAD = new AD(0, dsn, intDomainEnvironment);
                    DateTime datModified = DateTime.Parse(dr["modified"].ToString());
                    int intAsset = 0;
                    if (dr["assetid"].ToString() != "")
                        intAsset = Int32.Parse(dr["assetid"].ToString());
                    intErrorAsset = intAsset;
                    string strSerial = oAsset.Get(intAsset, "serial").ToUpper();
                    string strAsset = oAsset.Get(intAsset, "asset").ToUpper();
                    int intModel = Int32.Parse(dr["modelid"].ToString());
                    intErrorModel = intModel;
                    int intParent = 0;
                    int intType = 0;
                    if (intModel > 0)
                    {
                        intParent = Int32.Parse(oModelsProperties.Get(intModel, "modelid"));
                        intType = oModelsProperties.GetType(intModel);
                    }
                    DataSet dsSteps = oOnDemand.GetSteps(intType, 1);
                    string strStep = "N / A";
                    if (intStep == 999)
                        strStep = "Rebuild";
                    else if (dsSteps.Tables[0].Rows.Count > 0)
                        strStep = dsSteps.Tables[0].Rows[intStep - 1]["name"].ToString();
                    string strName = "";
                    int intWorkstationName = 0;
                    if (dr["nameid"].ToString() != "")
                    {
                        intWorkstationName = Int32.Parse(dr["nameid"].ToString());
                        if (intWorkstationName > 0)
                            strName = oWorkstation.GetName(intWorkstationName);
                    }
                    strSource = "PROD";
                    string strGroupRemote = "GSGu_WKS" + strName + "RemoteA";
                    double dblDrivePageFile = dblDrive2;
                    if (dblDrivePageFile > 0.00)   // If new VDI environment
                    {
                        strGroupRemote = "GSLwksRD_LAR_" + strName.ToUpper();
                    }
                    string strGroupAdmin = "GSGu_WKS" + strName + "Adm";
                    string strGroupW = "GSGw_" + strName;
                    DataSet dsGuest = oVMWare.GetGuest(strName);
                    int intHost = 0;
                    string strHost = "";
                    int intCluster = 0;
                    string strCluster = "";
                    int intClusterVersion = 0;
                    int intClusterAntiAffinity = 0;
                    int intClusterDell = 0;
                    string strFolder = "";
                    string strDataCenter = "";
                    string strVirtualCenter = "";
                    string strVirtualCenterURL = "";
                    string strDesktopNetwork = "";
                    int intClassID = 0;
                    int intEnvironmentID = 0;
                    int intAddressID = 0;
                    int intDatastore = 0;
                    int intDatastoreMax = 0;
                    int intDatastore2 = 0;
                    int intVlan = 0;
                    int intPool = 0;
                    string strMACAddress = "";
                    if (dsGuest.Tables[0].Rows.Count > 0)
                    {
                        intHost = Int32.Parse(dsGuest.Tables[0].Rows[0]["hostid"].ToString());
                        strHost = oVMWare.GetHost(intHost, "name");
                        intCluster = Int32.Parse(oVMWare.GetHost(intHost, "clusterid"));
                        strCluster = oVMWare.GetCluster(intCluster, "name");
                        if (intCluster > 0)
                        {
                            Int32.TryParse(oVMWare.GetCluster(intCluster, "version"), out intClusterVersion);
                            Int32.TryParse(oVMWare.GetCluster(intCluster, "anti_affinity"), out intClusterAntiAffinity);
                            Int32.TryParse(oVMWare.GetCluster(intCluster, "dell"), out intClusterDell);
                        }
                        int intFolder = Int32.Parse(oVMWare.GetCluster(intCluster, "folderid"));
                        strFolder = oVMWare.GetFolder(intFolder, "name");
                        int intDatacenter = Int32.Parse(oVMWare.GetFolder(intFolder, "datacenterid"));
                        strDataCenter = oVMWare.GetDatacenter(intDatacenter, "name");
                        strDesktopNetwork = oVMWare.GetDatacenter(intDatacenter, "desktop_network");
                        int intVirtualCenter = Int32.Parse(oVMWare.GetDatacenter(intDatacenter, "virtualcenterid"));
                        strVirtualCenter = oVMWare.GetVirtualCenter(intVirtualCenter, "name");
                        strVirtualCenterURL = oVMWare.GetVirtualCenter(intVirtualCenter, "url");
                        intClassID = Int32.Parse(dsGuest.Tables[0].Rows[0]["classid"].ToString());
                        intEnvironmentID = Int32.Parse(dsGuest.Tables[0].Rows[0]["environmentid"].ToString());
                        intAddressID = Int32.Parse(dsGuest.Tables[0].Rows[0]["addressid"].ToString());
                        intDatastore = Int32.Parse(dsGuest.Tables[0].Rows[0]["datastoreid"].ToString());
                        Int32.TryParse(oVMWare.GetDatastore(intDatastore, "maximum"), out intDatastoreMax);
                        Int32.TryParse(dsGuest.Tables[0].Rows[0]["datastore2id"].ToString(), out intDatastore2);
                        intVlan = Int32.Parse(dsGuest.Tables[0].Rows[0]["vlanid"].ToString());
                        intPool = Int32.Parse(dsGuest.Tables[0].Rows[0]["poolid"].ToString());
                        strMACAddress = dsGuest.Tables[0].Rows[0]["macaddress"].ToString();
                    }
                    int intResiliency = -1;     // Currently, DR and non-DR desktops are on the same cluster.  Exclude resilency from query.
                    //int intResiliency = 3;  // "Non-Replicated" (non-DR) by Default
                    //if (intRecovery == 1)
                    //{
                    //    // DR Recovery Testing...use the new data center stragegy (find the one marked BIR)
                    //    DataSet dsResiliency = oResiliency.Gets(1);
                    //    foreach (DataRow drResiliency in dsResiliency.Tables[0].Rows)
                    //    {
                    //        if (drResiliency["bir"].ToString() == "1")
                    //        {
                    //            intResiliency = Int32.Parse(drResiliency["id"].ToString());
                    //            break;
                    //        }
                    //    }
                    //}
                    DataSet dsBuild = oBuildLocation.GetRDPs(intClassID, intEnvironmentID, intAddressID, intResiliency, 0, 0, 1, 0);
                    string strConnect = oVMWare.Connect(strName);
                    oErrorVMWare = oVMWare;
                    _service = oVMWare.GetService();
                    int intOS = Int32.Parse(dr["osid"].ToString());
                    int intSP = Int32.Parse(dr["spid"].ToString());
                    string strDHCP = "";
                    string strIP = "";
                    if (strZeus == "")
                        oLog.AddEvent(strName, strSerial, "ZEUS XML Field is Empty!", LoggingType.Error);
                    else
                        strDHCP = dr[strZeus].ToString();
                    if (strDHCP == "SUCCESS")
                    {
                        Ping oDHCP = new Ping();
                        string strSuccessStatus = "";
                        try
                        {
                            PingReply oDHCPReply = oDHCP.Send(strName);
                            strSuccessStatus = oDHCPReply.Status.ToString().ToUpper();
                            if (strSuccessStatus == "SUCCESS")
                                strIP = Convert.ToString(oDHCPReply.Address);
                        }
                        catch { }
                    }
                    else if (strDHCP == "" || strDHCP == "0")
                        strIP = "";
                    else
                        strIP = strDHCP;
                    DateTime _now = DateTime.Now;
                    TimeSpan oSpan = _now.Subtract(datModified);
                    string strNow = _now.Day.ToString() + _now.Month.ToString() + _now.Year.ToString() + _now.Hour.ToString() + _now.Minute.ToString() + _now.Second.ToString() + _now.Millisecond.ToString();
                    string strResult = "";
                    string strError = "";
                    bool boolDomain = false;
                    string[] strDomainList;
                    strDomainList = strDomainsList.Split(strSplit);
                    for (int ii = 0; ii < strDomainList.Length; ii++)
                    {
                        if (strDomainList[ii].Trim() != "")
                        {
                            if (strDomainList[ii].Trim().ToUpper() == strDomain.ToUpper())
                            {
                                boolDomain = true;
                                break;
                            }
                        }
                    }
                    if (boolDomain == false)
                        strDomain = strDomainDefault;

                    double dblDriveC = double.Parse(strHDD);
                    double dblBuffer = 0.10;

                    // High Performance = 100
                    // Standard Performance = 10
                    // Low Performance = 1
                    int intStorageType = 10;
                    // Replicated = 1
                    // NON-Replicated = 0
                    int intReplicated = 0;

                    int intBuildClassProcess = intClassID;
                    if (intBuildClassProcess == 0)
                        intBuildClassProcess = intClass;

                    if (intLogging > 0)
                        oEventLog.WriteEntry("WORKSTATIONID: " + intWorkstation.ToString() + " (VMware WKS): " + "Checking ClassID " + intBuildClassProcess.ToString() + " (" + oClass.Get(intBuildClassProcess, "name") + ") for build options (intBuildProd = " + intBuildProd.ToString() + ",intBuildProdOffHours = " + intBuildProdOffHours.ToString() + ",intBuildQA = " + intBuildQA.ToString() + ",intBuildQAOffHours = " + intBuildQAOffHours.ToString() + ",intBuildTest = " + intBuildTest.ToString() + ")", EventLogEntryType.Information);
                    if (oClass.IsProd(intBuildClassProcess))
                    {
                        if (intLogging > 0)
                            oEventLog.WriteEntry("WORKSTATIONID: " + intWorkstation.ToString() + " (VMware WKS): " + "Checking PROD Classes", EventLogEntryType.Information);
                        if (intBuildProd == 1)
                        {
                            if (intLogging > 0)
                                oEventLog.WriteEntry("WORKSTATIONID: " + intWorkstation.ToString() + " (VMware WKS): " + "Checking PROD Off Hours", EventLogEntryType.Information);
                            if (intBuildProdOffHours == 0)
                            {
                                boolProcess = true;
                                if (intLogging > 0)
                                    oEventLog.WriteEntry("WORKSTATIONID: " + intWorkstation.ToString() + " (VMware WKS): " + "PROD Classes have been enabled for builds", EventLogEntryType.Information);
                            }
                            else if (DateTime.Now.Hour < 7 || DateTime.Now.Hour > 19)
                            {
                                boolProcess = true;
                                if (intLogging > 0)
                                    oEventLog.WriteEntry("WORKSTATIONID: " + intWorkstation.ToString() + " (VMware WKS): " + "PROD Classes have been enabled for OFF PEAK HOURS (7AM - 7PM) Current Time : " + DateTime.Now.ToShortTimeString(), EventLogEntryType.Information);
                            }
                            else if (intLogging > 0)
                                oEventLog.WriteEntry("WORKSTATIONID: " + intWorkstation.ToString() + " (VMware WKS): " + "PROD / QA Classes are not enabled for DURING PEAK HOURS (7AM - 7PM) builds", EventLogEntryType.Warning);
                        }
                        else if (intLogging > 0)
                            oEventLog.WriteEntry("WORKSTATIONID: " + intWorkstation.ToString() + " (VMware WKS): " + "PROD Classes are not enabled for builds", EventLogEntryType.Warning);
                    }
                    if (oClass.IsQA(intBuildClassProcess))
                    {
                        if (intLogging > 0)
                            oEventLog.WriteEntry("WORKSTATIONID: " + intWorkstation.ToString() + " (VMware WKS): " + "Checking QA Classes", EventLogEntryType.Information);
                        if (intBuildQA == 1)
                        {
                            if (intLogging > 0)
                                oEventLog.WriteEntry("WORKSTATIONID: " + intWorkstation.ToString() + " (VMware WKS): " + "Checking QA Off Hours", EventLogEntryType.Information);
                            if (intBuildQAOffHours == 0)
                            {
                                boolProcess = true;
                                if (intLogging > 0)
                                    oEventLog.WriteEntry("WORKSTATIONID: " + intWorkstation.ToString() + " (VMware WKS): " + "QA Classes have been enabled for builds", EventLogEntryType.Information);
                            }
                            else if (DateTime.Now.Hour < 7 || DateTime.Now.Hour > 19)
                            {
                                boolProcess = true;
                                if (intLogging > 0)
                                    oEventLog.WriteEntry("WORKSTATIONID: " + intWorkstation.ToString() + " (VMware WKS): " + "QA Classes have been enabled for OFF PEAK HOURS (7AM - 7PM) Current Time : " + DateTime.Now.ToShortTimeString(), EventLogEntryType.Information);
                            }
                            else if (intLogging > 0)
                                oEventLog.WriteEntry("WORKSTATIONID: " + intWorkstation.ToString() + " (VMware WKS): " + "QA Classes are not enabled for DURING PEAK HOURS (7AM - 7PM) builds", EventLogEntryType.Warning);
                        }
                        else if (intLogging > 0)
                            oEventLog.WriteEntry("WORKSTATIONID: " + intWorkstation.ToString() + " (VMware WKS): " + "QA Classes are not enabled for builds", EventLogEntryType.Warning);
                    }
                    if (oClass.IsTestDev(intBuildClassProcess))
                    {
                        if (intLogging > 0)
                            oEventLog.WriteEntry("WORKSTATIONID: " + intWorkstation.ToString() + " (VMware WKS): " + "Checking TEST Classes", EventLogEntryType.Information);
                        if (intBuildTest == 1)
                        {
                            boolProcess = true;
                            if (intLogging > 0)
                                oEventLog.WriteEntry("WORKSTATIONID: " + intWorkstation.ToString() + " (VMware WKS): " + "TEST Classes have been enabled for builds", EventLogEntryType.Information);
                        }
                        else if (intLogging > 0)
                            oEventLog.WriteEntry("WORKSTATIONID: " + intWorkstation.ToString() + " (VMware WKS): " + "TEST Classes are not enabled for builds", EventLogEntryType.Warning);
                    }

                    if (boolProcess == true || intStep == 1 || intStep == 2 || (intDesignOverride > 0 && intAnswer == intDesignOverride))
                    {
                        if (dr["rebuild_scheduled"].ToString() != "")
                        {
                            // REBUILD!
                            if (dr["rebuild_turnedoff"].ToString() == "")
                            {
                                // Turn off and rename
                                ManagedObjectReference _vm_rebuild = oVMWare.GetVM(strName);

                                if (_vm_rebuild != null)
                                {
                                    oLog.AddEvent(strName, strSerial, "Shutting down guest OS to initiate rebuild", LoggingType.Information);
                                    VirtualMachineRuntimeInfo run_rebuild = (VirtualMachineRuntimeInfo)oVMWare.getObjectProperty(_vm_rebuild, "runtime");
                                    if (run_rebuild.powerState != VirtualMachinePowerState.poweredOff)
                                    {
                                        ManagedObjectReference _task_shutdown = _service.PowerOffVM_Task(_vm_rebuild);
                                        oLog.AddEvent(strName, strSerial, "Guest OS shutdown Started", LoggingType.Information);
                                        TaskInfo _info_shutdown = (TaskInfo)oVMWare.getObjectProperty(_task_shutdown, "info");
                                        while (_info_shutdown.state == TaskInfoState.running)
                                            _info_shutdown = (TaskInfo)oVMWare.getObjectProperty(_task_shutdown, "info");
                                        if (_info_shutdown.state == TaskInfoState.success)
                                        {
                                            int intAttempt = 0;
                                            for (intAttempt = 0; intAttempt < 20 && run_rebuild.powerState != VirtualMachinePowerState.poweredOff; intAttempt++)
                                            {
                                                run_rebuild = (VirtualMachineRuntimeInfo)oVMWare.getObjectProperty(_vm_rebuild, "runtime");
                                                int intAttemptLeft = (20 - intAttempt);
                                                oLog.AddEvent(strName, strSerial, "Workstation still on...waiting 5 seconds (" + intAttemptLeft.ToString() + " tries left)", LoggingType.Information);
                                                Thread.Sleep(3000);
                                            }
                                        }
                                    }
                                    else
                                        oLog.AddEvent(strName, strSerial, "Guest OS was already shutdown (" + run_rebuild.powerState.ToString() + ")", LoggingType.Information);

                                    if (run_rebuild.powerState == VirtualMachinePowerState.poweredOff)
                                    {
                                        oLog.AddEvent(strName, strSerial, "Guest OS shutdown Finished", LoggingType.Information);
                                        ManagedObjectReference _task_rename = _service.Rename_Task(_vm_rebuild, strName.ToLower() + strRebuildSuffix);
                                        TaskInfo _info_rename = (TaskInfo)oVMWare.getObjectProperty(_task_rename, "info");
                                        while (_info_rename.state == TaskInfoState.running)
                                            _info_rename = (TaskInfo)oVMWare.getObjectProperty(_task_rename, "info");
                                        if (_info_rename.state == TaskInfoState.success)
                                        {
                                            oLog.AddEvent(strName, strSerial, "REBUILD: Finished Rename", LoggingType.Information);
                                            strResult += "Finished Powering Off " + strName + " [" + DateTime.Now.ToString() + "]";
                                            // Update database to queue for rebuild
                                            oWorkstation.UpdateVirtualRebuildPower(intWorkstation, DateTime.Now.ToString(), DateTime.Now.AddDays(1).ToString());
                                        }
                                        else
                                        {
                                            strError = "Virtual Machine Was Not Renamed ~ " + strName;
                                            oLog.AddEvent(strName, strSerial, "REBUILD: " + strError, LoggingType.Error);
                                            oWorkstation.UpdateVirtualRebuildCompleted(intWorkstation, DateTime.Now.ToString());
                                        }
                                    }
                                    else
                                    {
                                        strError = "There was a problem shutting down the guest ~ " + strName;
                                        oLog.AddEvent(strName, strSerial, "REBUILD: " + strError, LoggingType.Error);
                                        oWorkstation.UpdateVirtualRebuildCompleted(intWorkstation, DateTime.Now.ToString());
                                    }
                                }
                                else
                                {
                                    strError = "There was a problem locating the guest ~ " + strName;
                                    oLog.AddEvent(strName, strSerial, "REBUILD: " + strError, LoggingType.Error);
                                    oWorkstation.UpdateVirtualRebuildCompleted(intWorkstation, DateTime.Now.ToString());
                                }
                            }
                            else
                            {
                                // Has been turned off, now queue for rebuild
                                ManagedObjectReference _vm_rebuild = oVMWare.GetVM(strName.ToLower() + strRebuildSuffix);

                                if (_vm_rebuild != null)
                                {
                                    VirtualMachineRuntimeInfo run_rebuild = (VirtualMachineRuntimeInfo)oVMWare.getObjectProperty(_vm_rebuild, "runtime");
                                    if (run_rebuild.powerState == VirtualMachinePowerState.poweredOff)
                                    {
                                        oLog.AddEvent(strName, strSerial, "REBUILD: Found guest, still powered off, beginning destroy", LoggingType.Information);
                                        ManagedObjectReference _task_destroy = _service.Destroy_Task(_vm_rebuild);
                                        TaskInfo _info_destroy = (TaskInfo)oVMWare.getObjectProperty(_task_destroy, "info");
                                        while (_info_destroy.state == TaskInfoState.running)
                                            _info_destroy = (TaskInfo)oVMWare.getObjectProperty(_task_destroy, "info");
                                        if (_info_destroy.state == TaskInfoState.success)
                                        {
                                            oLog.AddEvent(strName, strSerial, "REBUILD: The guest has been destroyed - deleting computer object", LoggingType.Information);
                                            SearchResultCollection oResults = oAD.ComputerSearch(strName);
                                            if (oResults.Count == 1)
                                            {
                                                string delete = oAD.Delete(oResults[0].GetDirectoryEntry());
                                                if (delete == "")
                                                {
                                                    oLog.AddEvent(strName, strSerial, "REBUILD: The computer object has been deleted - updating database", LoggingType.Information);
                                                    oWorkstation.UpdateVirtualStep(intWorkstation, intStepCreate);
                                                    // Redo step...delete current step and update the other step
                                                    oOnDemand.UpdateStepDoneWorkstationRedo(intWorkstation, intStepCreate);
                                                    // Update database to queue for rebuild
                                                    oWorkstation.UpdateVirtualRebuildStarted(intWorkstation, DateTime.Now.ToString());
                                                    oLog.AddEvent(strName, strSerial, "REBUILD: The database is updated and the machine is queued for rebuild", LoggingType.Information);
                                                }
                                                else
                                                {
                                                    strError = "There was a problem deleting the computer object ~ " + delete;
                                                    oLog.AddEvent(strName, strSerial, "REBUILD: " + strError, LoggingType.Error);
                                                    oWorkstation.UpdateVirtualRebuildCompleted(intWorkstation, DateTime.Now.ToString());
                                                }
                                            }
                                            else
                                            {
                                                strError = "There was a problem finding the computer object ~ " + strName + " returned " + oResults.Count.ToString() + " results in " + oDomain.Get(intDomain, "name");
                                                oLog.AddEvent(strName, strSerial, "REBUILD: " + strError, LoggingType.Error);
                                                oWorkstation.UpdateVirtualRebuildCompleted(intWorkstation, DateTime.Now.ToString());
                                            }
                                        }
                                        else
                                        {
                                            strError = "Virtual Machine Was Not Destroyed ~ " + strName.ToLower() + strRebuildSuffix;
                                            oLog.AddEvent(strName, strSerial, "REBUILD: " + strError, LoggingType.Error);
                                            oWorkstation.UpdateVirtualRebuildCompleted(intWorkstation, DateTime.Now.ToString());
                                        }
                                    }
                                    else
                                    {
                                        strError = "The machine was powered back on ~ " + run_rebuild.powerState.ToString();
                                        oLog.AddEvent(strName, strSerial, "REBUILD: " + strError, LoggingType.Error);
                                        oWorkstation.UpdateVirtualRebuildCompleted(intWorkstation, DateTime.Now.ToString());
                                    }
                                }
                                else
                                {
                                    strError = "There was a problem locating the guest ~ " + strName.ToLower() + strRebuildSuffix;
                                    oLog.AddEvent(strName, strSerial, "REBUILD: " + strError, LoggingType.Error);
                                    oWorkstation.UpdateVirtualRebuildCompleted(intWorkstation, DateTime.Now.ToString());
                                }
                            }
                        }
                        else
                        {
                            if (intLogging > 0)
                                oEventLog.WriteEntry(strName + "...WORKSTATIONID: " + intWorkstation.ToString() + " (VMware WKS): " + "Starting...." + intStep.ToString(), EventLogEntryType.Information);

                            // Add Step
                            DataSet dsStepDoneWorkstation = oOnDemand.GetStepDoneWorkstation(intWorkstation, intStep);
                            if (dsStepDoneWorkstation.Tables[0].Rows.Count == 0)
                                oOnDemand.AddStepDoneWorkstation(intWorkstation, intStep, false);

                            switch (intStep)
                            {
                                case 1:     // Asset and Workstation Name 
                                    oLog.AddEvent(strName, strSerial, "Starting Step 1 (Workstation Name) for VMware workstationID " + intWorkstation.ToString(), LoggingType.Information);
                                    if (intAsset == 0)
                                    {
                                        intAsset = oAsset.AddGuest(strName, oForecast.GetModel(intAnswer), "VMWARE_WKS_" + intWorkstation.ToString(), "VMWARE_WKS_" + intWorkstation.ToString(), (int)AssetStatus.Available, intUser, DateTime.Now, 0, double.Parse(strRAM), 2.0, double.Parse(strHDD), intClass, intEnv, intAddress, 0, 0);
                                        intErrorAsset = intAsset;
                                        oWorkstation.UpdateVirtualAsset(intWorkstation, intAsset);
                                    }
                                    if (intWorkstationName > 0)
                                    {
                                        strName = oWorkstation.GetName(intWorkstationName);
                                        oLog.AddEvent(strName, strSerial, "Workstation Name: " + strName + " found from previous record", LoggingType.Information);
                                        strResult = "Workstation Name: " + strName;
                                        oAsset.AddStatus(intAsset, strName, (int)AssetStatus.InUse, 0, DateTime.Now);
                                    }
                                    else
                                    {
                                        // Create Name
                                        string strCode = oOperatingSystem.Get(intOS, "code");
                                        if (dblDrivePageFile > 0.00)   // If new VDI environment
                                            intWorkstationName = oWorkstation.AddName("VDI", strCode, (intRecovery == 1 ? "D" : (intInternal == 1 ? "I" : "E")), intClass, intEnv, 3, true);
                                        else
                                            intWorkstationName = oWorkstation.AddName((oClass.IsProd(intClass) ? "W" : "T"), strCode, "V", intClass, intEnv, 0, false);
                                        strName = oWorkstation.GetName(intWorkstationName);
                                        if (intWorkstationName > 0)
                                        {
                                            strResult = "Workstation Name: " + strName;
                                            oAsset.AddStatus(intAsset, strName, (int)AssetStatus.InUse, 0, DateTime.Now);
                                            oWorkstation.UpdateVirtualName(intWorkstation, intWorkstationName);
                                        }
                                        else
                                            strError = "All available workstation names are in use for the criteria specified - please report this problem to your ClearView administrator.";
                                    }
                                    AddResult(intWorkstation, intStep, intType, strResult, strError);
                                    break;
                                case 2:     // Assign Host
                                    oLog.AddEvent(strName, strSerial, "Starting Step 2 (Assign Host)", LoggingType.Information);
                                    strCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_PROVISIONING_SUPPORT");
                                    oLog.AddEvent(strName, strSerial, "Querying for host for intWorkstation: " + intWorkstation.ToString() + ", intModel: " + intModel.ToString() + ", boolWindows: " + boolWindows.ToString() + " for name " + strName, LoggingType.Information);
                                    string strAssign = oVMWare.AssignHost(strName, intAnswer, intModel, intOS, intResiliency, (oClass.Get(intClass, "pnc") == "1" ? 1 : 0), false, false, false, 0);
                                    oWorkstation.AddVirtualOutput(intWorkstation, "HOST", oVMWare.Results());
                                    oVMWare.ClearResults();
                                    if (strAssign == "")
                                    {
                                        if (oVMWare.Cluster() != "")
                                            strResult = "Connected to Cluster " + oVMWare.Cluster() + " in " + oVMWare.DataCenter() + " on " + oVMWare.VirtualCenter();
                                        else
                                            strResult = "Connected to Datacenter " + oVMWare.DataCenter() + " on " + oVMWare.VirtualCenter();
                                    }
                                    else
                                        strError = strAssign;
                                    AddResult(intWorkstation, intStep, intType, strResult, strError);
                                    break;
                                case 3:     // Assign Datastore
                                    oLog.AddEvent(strName, strSerial, "Starting Step 3 (Assign Datastore)", LoggingType.Information);
                                    if (strConnect == "")
                                    {
                                        // 3/9/2011: Change to new way of redoing...
                                        //  - Change DatastoreID to (-)DatastoreID.  Set step to assign datastore again (check code above for rest of changes).
                                        bool boolStorageRefreshed = false;
                                        if (intDatastore > 0)
                                            strResult = "Connected to Datastore " + oVMWare.GetDatastore(intDatastore, "name");
                                        else
                                        {
                                            if (intDatastore < 0)
                                                intDatastore = intDatastore * -1;
                                            strCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_PROVISIONING_SUPPORT");
                                            if (intLogging > 1)
                                                oLog.AddEvent(strName, strSerial, "Refreshing host system for cluster " + oVMWare.GetCluster(intCluster, "name"), LoggingType.Information);
                                            boolStorageRefreshed = true;
                                            string strRefreshStorage = oVMWare.RefreshStorage(oVMWare.GetCluster(intCluster, "name"), intCluster);
                                            if (strRefreshStorage == "")
                                            {
                                                double dblSizeLog = dblDriveC;
                                                oLog.AddEvent(strName, strSerial, "Querying for datastore for intClassID: " + intClassID.ToString() + ", intEnvironmentID: " + intEnvironmentID.ToString() + ", intAddressID: " + intAddressID.ToString() + ", Size: " + dblSizeLog.ToString() + ", Buffer: " + dblBuffer.ToString() + ", intStorageType: " + intStorageType.ToString() + ", intReplicated: " + intReplicated.ToString() + ", Server: Yes, Page File: No, boolWindows: " + boolWindows.ToString(), LoggingType.Information);
                                                string strAssign2 = oVMWare.AssignDatastore(strName, intCluster, dblSizeLog, dblBuffer, intStorageType, intReplicated, true, false, true, false, (oClass.Get(intClassID, "prod") == "1"), intAnswer, intModel, intOS, -1, false, false, false, intDatastore, 0);
                                                oWorkstation.AddVirtualOutput(intWorkstation, "DATASTORE", oVMWare.Results());
                                                oVMWare.ClearResults();
                                                if (strAssign2 == "")
                                                    strResult = "Connected to Datastore " + oVMWare.DataStore;
                                                else
                                                    strError = strAssign2;
                                            }
                                            else
                                                strError = "There was a problem refreshing the host system for the cluster ~ " + strRefreshStorage;
                                        }

                                        if (dblDrivePageFile > 0.00)   // If new VDI environment
                                        {
                                            oVMWare.DataStore = "";
                                            if (strError == "")
                                            {
                                                // Query for Page File datastore.
                                                if (intDatastore2 > 0)
                                                    strResult += "<br/>Connected to Datastore " + oVMWare.GetDatastore(intDatastore2, "name") + " (Page File)";
                                                else
                                                {
                                                    if (intDatastore2 < 0)
                                                        intDatastore2 = intDatastore2 * -1;
                                                    strCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_PROVISIONING_SUPPORT");
                                                    string strRefreshStorage = "";
                                                    if (boolStorageRefreshed == false)
                                                    {
                                                        if (intLogging > 1)
                                                            oLog.AddEvent(strName, strSerial, "Refreshing host system for cluster " + oVMWare.GetCluster(intCluster, "name"), LoggingType.Information);
                                                        strRefreshStorage = oVMWare.RefreshStorage(oVMWare.GetCluster(intCluster, "name"), intCluster);
                                                    }
                                                    if (strRefreshStorage == "")
                                                    {
                                                        double dblSizeLog = dblDrivePageFile;
                                                        oLog.AddEvent(strName, strSerial, "Querying for datastore2 for intClassID: " + intClassID.ToString() + ", intEnvironmentID: " + intEnvironmentID.ToString() + ", intAddressID: " + intAddressID.ToString() + ", Size: " + dblSizeLog.ToString() + ", Buffer: " + dblBuffer.ToString() + ", intStorageType: " + intStorageType.ToString() + ", intReplicated: " + intReplicated.ToString() + ", Server: No, Page File: Yes, boolWindows: " + boolWindows.ToString(), LoggingType.Information);
                                                        Int32.TryParse(oVMWare.GetGuest(strName, "datastoreid"), out intDatastore);
                                                        string strAssign2 = oVMWare.AssignDatastore(strName, intCluster, dblSizeLog, dblBuffer, intStorageType, intReplicated, false, true, true, false, (oClass.Get(intClassID, "prod") == "1"), intAnswer, intModel, intOS, -1, false, false, false, intDatastore2, intDatastore);
                                                        oWorkstation.AddVirtualOutput(intWorkstation, "DATASTORE2", oVMWare.Results());
                                                        oVMWare.ClearResults();
                                                        if (strAssign2 == "")
                                                            strResult += "<br/>Connected to Datastore " + oVMWare.DataStore + " (Page File)";
                                                        else
                                                            strError = strAssign2;
                                                    }
                                                    else
                                                        strError = "There was a problem refreshing the host system for the cluster ~ " + strRefreshStorage;
                                                }
                                            }
                                        }
                                    }
                                    else
                                        strError = strConnect;
                                    AddResult(intWorkstation, intStep, intType, strResult, strError);
                                    break;
                                case 4:     // Notify about datastores (if applicable) 
                                    oLog.AddEvent(strName, strSerial, "Starting Step 4 (Notify Datastores)", LoggingType.Information);
                                    try
                                    {
                                        ManagedObjectReference[] oDatastores = oVMWare.GetDatastores(oVMWare.GetCluster(intCluster, "name"));
                                        int intNotifyLeft = -1;
                                        if (oVMWare.GetCluster(intCluster, "datastores_left") != "")
                                            intNotifyLeft = Int32.Parse(oVMWare.GetCluster(intCluster, "datastores_left"));
                                        double dblNotifySize = 0.00;
                                        if (oVMWare.GetCluster(intCluster, "datastores_size") != "")
                                            dblNotifySize = double.Parse(oVMWare.GetCluster(intCluster, "datastores_size"));
                                        string strNotifyEmail = oVMWare.GetCluster(intCluster, "datastores_notify");
                                        if (intNotifyLeft > -1)
                                        {
                                            int intDatastoreTotal = 0;
                                            int intDatastoreNotify = 0;
                                            foreach (ManagedObjectReference oDataStore in oDatastores)
                                            {
                                                intDatastoreTotal++;
                                                DatastoreSummary oSummary = (DatastoreSummary)oVMWare.getObjectProperty(oDataStore, "summary");
                                                double dblFree = double.Parse(oSummary.freeSpace.ToString());
                                                // Change from B -> KB -> MB -> GB
                                                dblFree = dblFree / 1024.00;
                                                dblFree = dblFree / 1024.00;
                                                dblFree = dblFree / 1024.00;
                                                if (dblFree <= dblNotifySize)
                                                    intDatastoreNotify++;
                                            }
                                            int intNotifyDifference = (intDatastoreTotal - intDatastoreNotify);     // 8 total - 6 out of space = 2 left
                                            strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_INVENTORY_MANAGER");
                                            if (intNotifyLeft > intNotifyDifference && strNotifyEmail != "")             // 3 > 2
                                                oFunction.SendEmail("WARNING: VMware Datastore Notification", strNotifyEmail, "", strEMailIdsBCC, "WARNING: VMware Datastore Notification", "<p><b>This message is to inform you that you are running out of datastore space for the cluster " + oVMWare.GetCluster(intCluster, "name") + " (" + oVMWare.DataCenter() + " on " + oVMWare.VirtualCenter() + ")</b></p><p>There are less than " + intNotifyLeft.ToString() + " datastores with no more than " + dblNotifySize.ToString() + " GB each.</p>", true, false);

                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        oLog.AddEvent(strName, strSerial, "Datastore Query Failed ~ " + String.Format(ex.Message) + " [" + System.Environment.UserName + "]", LoggingType.Error);
                                    }
                                    strResult = "VMware Notifications have been sent";
                                    AddResult(intWorkstation, intStep, intType, strResult, strError);
                                    break;
                                case 5:     // Create Active Directory Groups 
                                    oLog.AddEvent(strName, strSerial, "Starting Step 5 (AD Groups)", LoggingType.Information);
                                    oLog.AddEvent(strName, strSerial, "Searching for " + strGroupRemote + " on " + oVar.primaryDC(dsn) + " using " + oVar.Domain() + "\\" + oVar.ADUser(), LoggingType.Debug);
                                    if (oAD.Search(strGroupRemote, false) == null)
                                    {
                                        string strGroupResult = "";
                                        if (dblDrivePageFile > 0.00)   // If new VDI environment
                                            strGroupResult = oAD.CreateGroup(strGroupRemote, "Remote access group for virtual workstation", "Requires approval from one of the following:" + Environment.NewLine + "               " + oUser.GetName(Int32.Parse(oForecast.GetAnswer(intAnswer, "appcontact"))) + " - " + oUser.GetFullName(Int32.Parse(oForecast.GetAnswer(intAnswer, "appcontact"))), "OU=OUg_LAR,OU=OUc_Resources,", "DLG", "S");
                                        else
                                            strGroupResult = oAD.CreateGroup(strGroupRemote, "Remote access group for virtual workstation", "Requires approval from one of the following:" + Environment.NewLine + "               " + oUser.GetName(Int32.Parse(oForecast.GetAnswer(intAnswer, "appcontact"))) + " - " + oUser.GetFullName(Int32.Parse(oForecast.GetAnswer(intAnswer, "appcontact"))), "OU=OUc_WksMgmt,OU=OUg_Resources,", "GG", "S");

                                        if (strGroupResult == "")
                                            strResult = "The group " + strGroupRemote + " was successfully created in " + oVar.Name() + "<br/>";
                                        else
                                            strError = "There was a problem creating the group " + strGroupRemote + " in " + oVar.Name() + " (Error: " + strGroupResult + ")<br/>";
                                    }
                                    else
                                        strResult = "The group " + strGroupRemote + " already exists in " + oVar.Name() + "<br/>";
                                    if (strError == "" && GFB == false)
                                    {
                                        // Add workstation to appropriate citrix group
                                        string strGroupResultJoin = oAD.JoinGroup(strGroupRemote, xenConfig.ADGroup, 0);
                                        if (strGroupResultJoin == "")
                                            strResult += "The group " + strGroupRemote + " was successfully joined to " + xenConfig.ADGroup + " in " + oVar.Name() + "<br/>";
                                        else
                                            strError += "There was a problem joining the " + strGroupRemote + " group to " + strGroupW + " in " + oVar.Name() + " (Error: " + strGroupResultJoin + ")<br/>";
                                    }
                                    if (intDomainEnvironment != (int)CurrentEnvironment.CORPDMN
                                        && intDomainEnvironment != (int)CurrentEnvironment.PNCQA
                                        && intDomainEnvironment != (int)CurrentEnvironment.PNCRND
                                        && intDomainEnvironment != (int)CurrentEnvironment.PNCUAT
                                        && intDomainEnvironment != (int)CurrentEnvironment.PNCNT_PROD) // Not Production
                                    {
                                        // Create Adm group in OUc_Orgs/OUg_Admins
                                        if (oAD.Search(strGroupAdmin, false) == null)
                                        {
                                            string strGroupResult = oAD.CreateGroup(strGroupAdmin, "Virtual Workstation; " + strManager, "", "OU=OUg_Admins,OU=OUc_Orgs,", "GG", "S");
                                            if (strGroupResult == "")
                                                strResult += "The group " + strGroupAdmin + " was successfully created in " + oVar.Name() + "<br/>";
                                            else
                                                strError += "There was a problem creating the group " + strGroupAdmin + " in " + oVar.Name() + " (Error: " + strGroupResult + ")<br/>";
                                        }
                                        else
                                            strResult += "The group " + strGroupAdmin + " already exists in " + oVar.Name() + "<br/>";

                                        // Create GSGw group in OUg_Resources
                                        if (oAD.Search(strGroupW, false) == null)
                                        {
                                            string strGroupResult = oAD.CreateGroup(strGroupW, "Virtual Workstation; " + strManager, "", "OU=OUg_Resources,", "GG", "S");
                                            if (strGroupResult == "")
                                                strResult += "The group " + strGroupW + " was successfully created in " + oVar.Name() + "<br/>";
                                            else
                                                strError += "There was a problem creating the group " + strGroupW + " in " + oVar.Name() + " (Error: " + strGroupResult + ")<br/>";
                                        }
                                        else
                                            strResult += "The group " + strGroupW + " already exists in " + oVar.Name() + "<br/>";

                                        // Create Workstation Object in OUc_DmnCptrs/OUc_Wksts/OUw_Standard
                                        if (oAD.Search(strName, true) == null)
                                        {
                                            string strGroupResult = oAD.CreateWorkstation(strName, "Virtual Workstation; " + strManager, "", "OU=OUw_Standard,OU=OUc_Wksts,OU=OUc_DmnCptrs,");
                                            if (strGroupResult == "")
                                                strResult += "The workstation object " + strName + " was successfully created in " + oVar.Name() + "<br/>";
                                            else
                                                strError += "There was a problem creating the workstation object " + strName + " in " + oVar.Name() + " (Error: " + strGroupResult + ")<br/>";
                                        }
                                        else
                                            strResult += "The workstation object " + strName + " already exists in " + oVar.Name() + "<br/>";

                                        // Add workstation to GSGw group
                                        string strGroupResultJoin = oAD.JoinGroup(strName, strGroupW, 0);
                                        if (strGroupResultJoin == "")
                                            strResult += "The group " + strGroupW + " was successfully joined to the " + strName + " workstation object in " + oVar.Name() + "<br/>";
                                        else
                                            strError += "There was a problem joining the " + strName + " workstation object to the group " + strGroupW + " in " + oVar.Name() + " (Error: " + strGroupResultJoin + ")<br/>";
                                    }
                                    AddResult(intWorkstation, intStep, intType, strResult, strError);
                                    break;
                                case 6:     // Create Machine
                                    oLog.AddEvent(strName, strSerial, "Starting Step 6 (Create Machine)", LoggingType.Information);
                                    bool boolCreate = true;
                                    if (strConnect == "")
                                    {
                                        if (intDatastore > 0)
                                        {
                                            // Check to make sure the maximum number of guests is not reached.
                                            int intMachines = oVMWare.GetDatastoreMachineCount(intCluster, intDatastore);
                                            if (intMachines < intDatastoreMax)
                                            {
                                                intMachines++;
                                                oLog.AddEvent(strName, strSerial, "There is enough room for machine # " + intMachines.ToString() + " of " + intDatastoreMax.ToString(), LoggingType.Debug);
                                                ManagedObjectReference datacenterRef = oVMWare.GetDataCenter();
                                                ManagedObjectReference vmFolderRef = oVMWare.GetVMFolder(datacenterRef);
                                                ManagedObjectReference clusterRef = oVMWare.GetCluster(oVMWare.GetCluster(intCluster, "name"));
                                                //ManagedObjectReference resourcePoolRootRef = (ManagedObjectReference)oVMWare.getObjectProperty(clusterRef, "resourcePool");
                                                ManagedObjectReference resourcePoolRootRef = oVMWare.GetResourcePool(clusterRef, oVMWare.GetCluster(intCluster, "resource_pool"));
                                                VirtualMachineConfigSpec oConfig = new VirtualMachineConfigSpec();
                                                // Build Annotations
                                                string strAnnotation = "";
                                                DataSet dsAnnotations = oWorkstation.GetAccountsVMware(intWorkstation);
                                                foreach (DataRow drAnnotation in dsAnnotations.Tables[0].Rows)
                                                {
                                                    int intAccount = Int32.Parse(drAnnotation["userid"].ToString());
                                                    string strIDx = oUser.Get(intAccount, "xid").Trim().ToUpper();
                                                    string strIDpnc = oUser.Get(intAccount, "pnc_id").Trim().ToUpper();
                                                    int intManager = 0;
                                                    Int32.TryParse(oUser.Get(intAccount, "manager"), out intManager);
                                                    if (oOperatingSystem.Get(intOS, "name").ToUpper().Contains("7") == true && oService.GetSelected(intRequestService, intJoelsService, 1).Tables[0].Rows.Count == 0)
                                                    {
                                                        if (GFB)
                                                            strAnnotation += "GFB " + strIDpnc + ", " + strIDx + ", " + oUser.GetFullName(intAccount) + ", Manager: " + (intManager == 0 ? "" : oUser.GetFullName(intManager)) + Environment.NewLine;
                                                        else
                                                            strAnnotation += "PROD " + strIDpnc + ", " + strIDx + ", " + oUser.GetFullName(intAccount) + ", Manager: " + (intManager == 0 ? "" : oUser.GetFullName(intManager)) + Environment.NewLine;
                                                    }
                                                    else
                                                        strAnnotation += strIDpnc + ", " + strIDx + ", " + oUser.GetFullName(intAccount) + ", Manager: " + (intManager == 0 ? "" : oUser.GetFullName(intManager)) + Environment.NewLine;
                                                }
                                                oConfig.annotation = strAnnotation;
                                                oConfig.guestId = oOperatingSystem.Get(intOS, "vmware_os");
                                                string strRamConfig = strRAM;
                                                oConfig.memoryMB = long.Parse(strRamConfig);
                                                oConfig.memoryMBSpecified = true;
                                                int intCpuConfig = Int32.Parse(strCPU);
                                                oConfig.numCPUs = intCpuConfig;
                                                oConfig.numCPUsSpecified = true;
                                                oConfig.name = strName.ToLower();
                                                oConfig.files = new VirtualMachineFileInfo();
                                                oConfig.files.vmPathName = "[" + oVMWare.GetDatastore(intDatastore, "name") + "] " + strName.ToLower() + "/" + strName.ToLower() + ".vmx";

                                                oLog.AddEvent(strName, strSerial, "Virtual Machine Configuration: Name=" + strName + ", OS=" + oOperatingSystem.Get(intOS, "vmware_os") + ", RAM=" + strRamConfig + ", CPU=" + intCpuConfig.ToString() + ", Datastore=" + oVMWare.GetDatastore(intDatastore, "name") + ", Cluster=" + oVMWare.GetCluster(intCluster, "name"), LoggingType.Information);

                                                ManagedObjectReference _task = _service.CreateVM_Task(vmFolderRef, oConfig, resourcePoolRootRef, null);
                                                TaskInfo oInfo = (TaskInfo)oVMWare.getObjectProperty(_task, "info");
                                                while (oInfo.state == TaskInfoState.running)
                                                    oInfo = (TaskInfo)oVMWare.getObjectProperty(_task, "info");
                                                if (oInfo.state == TaskInfoState.success)
                                                {
                                                    ManagedObjectReference newVM = (ManagedObjectReference)oInfo.result;
                                                    oVMWare.UpdateGuestVIM(strName, newVM.Value);
                                                    strResult = "Virtual Machine " + strName.ToUpper() + " Created (" + newVM.Value + ")";
                                                }
                                                else
                                                    strError = "Virtual Machine Was NOT Created ~ " + strName.ToUpper() + " (RAM = " + strRamConfig + " MB) (CPUs = " + intCpuConfig.ToString() + ") (Datastore = " + oVMWare.GetDatastore(intDatastore, "name") + ")";
                                            }
                                            else
                                            {
                                                oLog.AddEvent(strName, strSerial, "The datastore is already at maximum machine count...restarting datastore assignment", LoggingType.Information);
                                                boolCreate = false;
                                            }
                                        }
                                        else
                                            strError = "Unable to find a datastore";
                                    }
                                    else
                                        strError = strConnect;
                                    if (boolCreate == true)
                                        AddResult(intWorkstation, intStep, intType, strResult, strError);
                                    else
                                    {
                                        oVMWare.UpdateGuestDatastore(strName, (intDatastore * -1), 0.00);
                                        oWorkstation.UpdateVirtualStep(intWorkstation, intStepDatastore);
                                        oOnDemand.DeleteStepDoneWorkstation(intWorkstation, intStepDatastore);
                                    }
                                    break;
                                case 7:     // Create SCSI Controller 
                                    oLog.AddEvent(strName, strSerial, "Starting Step 7 (Create SCSI Controller)", LoggingType.Information);
                                    ManagedObjectReference _vm_scsi = oVMWare.GetVM(strName);
                                    VirtualMachineConfigSpec _cs_scsi = new VirtualMachineConfigSpec();
                                    // 05/15/2009: Per Chuck, he did testing with BusLogic and doesn't think there is a performance difference, so change to BusLogic
                                    if (dblDrivePageFile > 0.00)   // If new VDI environment
                                        _cs_scsi.deviceChange = new VirtualDeviceConfigSpec[] { ControllerLSILogic() };
                                    else
                                        _cs_scsi.deviceChange = new VirtualDeviceConfigSpec[] { ControllerBusLogic() };
                                    ManagedObjectReference _task_scsi = _service.ReconfigVM_Task(_vm_scsi, _cs_scsi);
                                    TaskInfo _inf_scsi = (TaskInfo)oVMWare.getObjectProperty(_task_scsi, "info");
                                    while (_inf_scsi.state == TaskInfoState.running)
                                        _inf_scsi = (TaskInfo)oVMWare.getObjectProperty(_task_scsi, "info");
                                    if (_inf_scsi.state == TaskInfoState.success)
                                        strResult = "SCSI Controller Created";
                                    else
                                        strError = "SCSI Controller Was Not Created";
                                    AddResult(intWorkstation, intStep, intType, strResult, strError);
                                    break;
                                case 8:     // Create Hard Disk 1 
                                    oLog.AddEvent(strName, strSerial, "Starting Step 8 (Create Hard Disk 1)", LoggingType.Information);
                                    bool boolHDD1 = true;
                                    ManagedObjectReference _vm_hdd = oVMWare.GetVM(strName);
                                    VirtualMachineConfigInfo _vminfo_hdd = (VirtualMachineConfigInfo)oVMWare.getObjectProperty(_vm_hdd, "config");
                                    bool boolDrive1 = false;
                                    bool boolDrive2 = false;
                                    VirtualDevice[] _device_hdd = _vminfo_hdd.hardware.device;
                                    for (int ii = 0; ii < _device_hdd.Length; ii++)
                                    {
                                        if (_device_hdd[ii].deviceInfo.label.ToUpper() == "HARD DISK 1")
                                        {
                                            boolDrive1 = true;
                                            oLog.AddEvent(strName, strSerial, "Found Hard Disk # 1", LoggingType.Debug);
                                            break;
                                        }
                                    }
                                    for (int ii = 0; ii < _device_hdd.Length; ii++)
                                    {
                                        if (_device_hdd[ii].deviceInfo.label.ToUpper() == "HARD DISK 2")
                                        {
                                            boolDrive2 = true;
                                            oLog.AddEvent(strName, strSerial, "Found Hard Disk # 2", LoggingType.Debug);
                                            break;
                                        }
                                    }
                                    if (intDatastore > 0)
                                    {
                                        if (boolDrive1 == false)
                                        {
                                            VirtualMachineConfigSpec _cs_hdd1 = new VirtualMachineConfigSpec();
                                            dblDriveC = dblDriveC * 1024.00 * 1024.00;
                                            VirtualDeviceConfigSpec diskVMSpec1 = Disk(oVMWare, strName, oVMWare.GetDatastore(intDatastore, "name"), dblDriveC.ToString("0"), 0, "");    // 10485760 KB = 10 GB = 10 x 1024 x 1024
                                            _cs_hdd1.deviceChange = new VirtualDeviceConfigSpec[] { diskVMSpec1 };
                                            ManagedObjectReference _task_hdd1 = _service.ReconfigVM_Task(_vm_hdd, _cs_hdd1);
                                            TaskInfo _info_hdd1 = (TaskInfo)oVMWare.getObjectProperty(_task_hdd1, "info");
                                            while (_info_hdd1.state == TaskInfoState.running)
                                                _info_hdd1 = (TaskInfo)oVMWare.getObjectProperty(_task_hdd1, "info");
                                            if (_info_hdd1.state == TaskInfoState.success)
                                            {
                                                strResult = "Hard Drive 1 Created (" + dblDriveC.ToString("0") + ")";
                                                oLog.AddEvent(strName, strSerial, strResult, LoggingType.Information);
                                            }
                                            else
                                            {
                                                LocalizedMethodFault _error_hdd1 = _info_hdd1.error;
                                                if (_error_hdd1.localizedMessage.ToUpper().Contains("INSUFFICIENT DISK SPACE") == true)
                                                {
                                                    // 3/9/2011: Change to new way of redoing...
                                                    // Delete / Destroy the guest.
                                                    ManagedObjectReference _task_insufficient = _service.Destroy_Task(_vm_hdd);
                                                    TaskInfo _info_insufficient = (TaskInfo)oVMWare.getObjectProperty(_task_insufficient, "info");
                                                    while (_info_insufficient.state == TaskInfoState.running)
                                                        _info_insufficient = (TaskInfo)oVMWare.getObjectProperty(_task_insufficient, "info");
                                                    if (_info_insufficient.state == TaskInfoState.success)
                                                    {
                                                        oLog.AddEvent(strName, strSerial, "The guest has been destroyed...restarting datastore assignment", LoggingType.Information);
                                                        boolHDD1 = false;
                                                    }
                                                    else
                                                        strError = "Could not destroy the guest to restart the datastore assignment";
                                                }
                                                else
                                                    strError = "Hard Drive 1 Was Not Created on " + oVMWare.GetDatastore(intDatastore, "name") + " (" + dblDriveC.ToString("0") + ")";
                                            }
                                        }
                                        else
                                        {
                                            strResult = "Hard Drive 1 Was Already Created (" + dblDriveC.ToString("0") + ")";
                                            oLog.AddEvent(strName, strSerial, strResult, LoggingType.Information);
                                        }
                                    }
                                    else
                                        strError = "Unable to find a datastore";

                                    if (strError == "")
                                    {
                                        if (boolHDD1 == true)
                                        {
                                            if (dblDrivePageFile > 0.00)   // If new VDI environment
                                            {
                                                bool boolHDD2 = true;
                                                if (intDatastore2 > 0)
                                                {
                                                    if (boolDrive2 == false)
                                                    {
                                                        VirtualMachineConfigSpec _cs_hdd2 = new VirtualMachineConfigSpec();
                                                        dblDrivePageFile = dblDrivePageFile * 1024.00 * 1024.00;
                                                        VirtualDeviceConfigSpec diskVMSpec2 = Disk(oVMWare, strName, oVMWare.GetDatastore(intDatastore2, "name"), dblDrivePageFile.ToString("0"), 1, "");    // 10485760 KB = 10 GB = 10 x 1024 x 1024
                                                        _cs_hdd2.deviceChange = new VirtualDeviceConfigSpec[] { diskVMSpec2 };
                                                        ManagedObjectReference _task_hdd2 = _service.ReconfigVM_Task(_vm_hdd, _cs_hdd2);
                                                        TaskInfo _info_hdd2 = (TaskInfo)oVMWare.getObjectProperty(_task_hdd2, "info");
                                                        while (_info_hdd2.state == TaskInfoState.running)
                                                            _info_hdd2 = (TaskInfo)oVMWare.getObjectProperty(_task_hdd2, "info");
                                                        if (_info_hdd2.state == TaskInfoState.success)
                                                        {
                                                            strResult += "<br/>Hard Drive 2 Created (" + dblDrivePageFile.ToString("0") + ")";
                                                            oLog.AddEvent(strName, strSerial, strResult, LoggingType.Information);
                                                        }
                                                        else
                                                        {
                                                            LocalizedMethodFault _error_hdd2 = _info_hdd2.error;
                                                            if (_error_hdd2.localizedMessage.ToUpper().Contains("INSUFFICIENT DISK SPACE") == true)
                                                            {
                                                                ManagedObjectReference _task_insufficient = _service.Destroy_Task(_vm_hdd);
                                                                TaskInfo _info_insufficient = (TaskInfo)oVMWare.getObjectProperty(_task_insufficient, "info");
                                                                while (_info_insufficient.state == TaskInfoState.running)
                                                                    _info_insufficient = (TaskInfo)oVMWare.getObjectProperty(_task_insufficient, "info");
                                                                if (_info_insufficient.state == TaskInfoState.success)
                                                                {
                                                                    oLog.AddEvent(strName, strSerial, "The guest has been destroyed...restarting datastore assignment", LoggingType.Information);
                                                                    boolHDD2 = false;
                                                                }
                                                                else
                                                                    strError = "Could not destroy the guest to restart the datastore assignment";
                                                            }
                                                            else
                                                                strError = "Hard Drive 2 Was Not Created on " + oVMWare.GetDatastore(intDatastore2, "name") + " (" + dblDrivePageFile.ToString("0") + ")";
                                                        }
                                                    }
                                                    else
                                                    {
                                                        string strTemp = "<br/>Hard Drive 2 Was Already Created (" + dblDrivePageFile.ToString("0") + ")";
                                                        oLog.AddEvent(strName, strSerial, strTemp, LoggingType.Information);
                                                        strResult += strTemp;
                                                    }
                                                }
                                                else
                                                    strError = "Unable to find a datastore";
                                                if (boolHDD2 == true)
                                                    AddResult(intWorkstation, intStep, intType, strResult, strError);
                                                else
                                                {
                                                    // 3/9/2011: Change to new way of redoing...
                                                    //  - Change DatastoreID to (-)DatastoreID.  Set step to assign datastore again (check code above for rest of changes).
                                                    oVMWare.UpdateGuestDatastore2(strName, (intDatastore2 * -1), 0.00);
                                                    oWorkstation.UpdateVirtualStep(intWorkstation, intStepDatastore);
                                                    oOnDemand.DeleteStepDoneWorkstation(intWorkstation, intStepDatastore);
                                                }
                                            }
                                            else
                                                AddResult(intWorkstation, intStep, intType, strResult, strError);
                                        }
                                        else
                                        {
                                            // 3/9/2011: Change to new way of redoing...
                                            //  - Change DatastoreID to (-)DatastoreID.  Set step to assign datastore again (check code above for rest of changes).
                                            oVMWare.UpdateGuestDatastore(strName, (intDatastore * -1), 0.00);
                                            oWorkstation.UpdateVirtualStep(intWorkstation, intStepDatastore);
                                            oOnDemand.DeleteStepDoneWorkstation(intWorkstation, intStepDatastore);
                                        }
                                    }
                                    else
                                        AddResult(intWorkstation, intStep, intType, strResult, strError);
                                    break;
                                case 9:    // Create Network Adapter
                                    oLog.AddEvent(strName, strSerial, "Starting Step 9 (Create Network Adapter)", LoggingType.Information);
                                    ManagedObjectReference _vm_net = oVMWare.GetVM(strName);
                                    oLog.AddEvent(strName, strSerial, "There are " + dsBuild.Tables[0].Rows.Count.ToString() + " (VMware WKS) build locations (RDP) for class: " + oClass.Get(intClassID, "name") + ", address: " + oLocation.GetFull(intAddressID) + " for workstation ID " + intWorkstation.ToString(), LoggingType.Information);
                                    if (dsBuild.Tables[0].Rows.Count > 0 && dsBuild.Tables[0].Rows[0]["vmware_vlan"].ToString() != "")
                                    {
                                        if (intClusterVersion == 0)
                                        {
                                            oLog.AddEvent(strName, strSerial, "Creating Network Adapter # 1 using Direct VLAN", LoggingType.Debug);
                                            if (dsBuild.Tables[0].Rows[0]["vmware_vlan"].ToString() != "")
                                            {
                                                string strRDPVLAN = dsBuild.Tables[0].Rows[0]["vmware_vlan"].ToString();
                                                VirtualDeviceConfigSpec[] configspecarr = new VirtualDeviceConfigSpec[1];
                                                VirtualEthernetCardNetworkBackingInfo vecnbi = new VirtualEthernetCardNetworkBackingInfo();
                                                vecnbi.deviceName = strRDPVLAN;
                                                VirtualEthernetCard newethdev;
                                                if (oOperatingSystem.Get(intOS, "e1000") == "1")
                                                    newethdev = new VirtualE1000();
                                                else
                                                    newethdev = new VirtualVmxnet3();
                                                newethdev.backing = vecnbi;
                                                newethdev.key = 5000;
                                                VirtualDeviceConfigSpec newethdevicespec = new VirtualDeviceConfigSpec();
                                                newethdevicespec.device = newethdev;
                                                newethdevicespec.operation = VirtualDeviceConfigSpecOperation.add;
                                                newethdevicespec.operationSpecified = true;
                                                configspecarr[0] = newethdevicespec;
                                                VirtualMachineConfigSpec vmconfigspec = new VirtualMachineConfigSpec();
                                                vmconfigspec.deviceChange = configspecarr;
                                                ManagedObjectReference _task_net = _service.ReconfigVM_Task(_vm_net, vmconfigspec);
                                                TaskInfo _info_net = (TaskInfo)oVMWare.getObjectProperty(_task_net, "info");
                                                while (_info_net.state == TaskInfoState.running)
                                                    _info_net = (TaskInfo)oVMWare.getObjectProperty(_task_net, "info");
                                                if (_info_net.state == TaskInfoState.success)
                                                    strResult += "Network Adapter Created";
                                                else
                                                    strError += "Network Adapter Was Not Created";
                                            }
                                            else
                                            {
                                                strError = "Invalid build location (RDP) configuration ~ Missing VMware VLAN...Class: " + oClass.Get(intClass, "name") + ", Environment: " + oEnvironment.Get(intEnv, "name") + ", Location: " + oLocation.GetFull(intAddress) + ", Resiliency: " + oResiliency.Get(intResiliency, "name");
                                                oLog.AddEvent(strName, strSerial, strError, LoggingType.Error);
                                            }
                                        }
                                        if (intClusterVersion == 1)
                                        {
                                            oLog.AddEvent(strName, strSerial, "Creating Network Adapter # 1 using Virtual Port Group", LoggingType.Debug);
                                            string strRDPVLAN = "";
                                            if (intClusterDell == 1)    // Hosts are on DELL Hardware
                                            {
                                                if (dsBuild.Tables[0].Rows[0]["dell_vmware_vlan"].ToString() != "")
                                                    strRDPVLAN = dsBuild.Tables[0].Rows[0]["dell_vmware_vlan"].ToString();
                                                else
                                                    strError = "Invalid build location (RDP) configuration ~ Missing DELL VMware VLAN...Class: " + oClass.Get(intClass, "name") + ", Environment: " + oEnvironment.Get(intEnv, "name") + ", Location: " + oLocation.GetFull(intAddress) + ", Resiliency: " + oResiliency.Get(intResiliency, "name");
                                            }
                                            else
                                            {
                                                if (dsBuild.Tables[0].Rows[0]["vsphere_vlan"].ToString() != "")
                                                    strRDPVLAN = dsBuild.Tables[0].Rows[0]["vsphere_vlan"].ToString();
                                                else
                                                    strError = "Invalid build location (RDP) configuration ~ Missing VSphere VLAN...Class: " + oClass.Get(intClass, "name") + ", Environment: " + oEnvironment.Get(intEnv, "name") + ", Location: " + oLocation.GetFull(intAddress) + ", Resiliency: " + oResiliency.Get(intResiliency, "name");
                                            }
                                            if (strError == "")
                                            {
                                                bool boolCompleted = false;
                                                string strPortGroupKey = "";
                                                ManagedObjectReference datacenterRefNetwork = oVMWare.GetDataCenter();
                                                ManagedObjectReference[] oNetworks = (ManagedObjectReference[])oVMWare.getObjectProperty(datacenterRefNetwork, "network");
                                                foreach (ManagedObjectReference oNetwork in oNetworks)
                                                {
                                                    if (boolCompleted == true)
                                                        break;
                                                    string strNetworkName = oVMWare.getObjectProperty(oNetwork, "name").ToString();
                                                    try
                                                    {
                                                        oLog.AddEvent(strName, strSerial, "Checking if RDP VLAN (" + strRDPVLAN + ") equals network name (" + strNetworkName + ")", LoggingType.Debug);
                                                        if (strRDPVLAN == "" || strRDPVLAN == strNetworkName)
                                                        {
                                                            oLog.AddEvent(strName, strSerial, strNetworkName + " has been found!", LoggingType.Debug);
                                                            object oPortConfig = oVMWare.getObjectProperty(oNetwork, "config");
                                                            if (oPortConfig != null)
                                                            {
                                                                oLog.AddEvent(strName, strSerial, strNetworkName + "...got config", LoggingType.Debug);
                                                                DVPortgroupConfigInfo oPort = (DVPortgroupConfigInfo)oPortConfig;
                                                                if (oPort.key != strPortGroupKey)
                                                                {
                                                                    strPortGroupKey = oPort.key;
                                                                    ManagedObjectReference oSwitch = oPort.distributedVirtualSwitch;
                                                                    string strSwitchUUID = (string)oVMWare.getObjectProperty(oSwitch, "uuid");
                                                                    oLog.AddEvent(strName, strSerial, "Trying..." + strPortGroupKey + "(" + strSwitchUUID + ")", LoggingType.Information);

                                                                    VirtualDeviceConfigSpec[] configspecarr = configspecarr = new VirtualDeviceConfigSpec[1];
                                                                    VirtualEthernetCardDistributedVirtualPortBackingInfo vecdvpbi = new VirtualEthernetCardDistributedVirtualPortBackingInfo();
                                                                    DistributedVirtualSwitchPortConnection connection = new DistributedVirtualSwitchPortConnection();
                                                                    connection.portgroupKey = strPortGroupKey;
                                                                    connection.switchUuid = strSwitchUUID;
                                                                    vecdvpbi.port = connection;
                                                                    VirtualEthernetCard newethdev = new VirtualVmxnet3();
                                                                    //VirtualEthernetCard newethdev = new VirtualE1000();
                                                                    newethdev.backing = vecdvpbi;
                                                                    newethdev.key = 5000;
                                                                    VirtualDeviceConfigSpec newethdevicespec = new VirtualDeviceConfigSpec();
                                                                    newethdevicespec.device = newethdev;
                                                                    newethdevicespec.operation = VirtualDeviceConfigSpecOperation.add;
                                                                    newethdevicespec.operationSpecified = true;
                                                                    configspecarr[0] = newethdevicespec;
                                                                    VirtualMachineConfigSpec vmconfigspec = new VirtualMachineConfigSpec();
                                                                    vmconfigspec.deviceChange = configspecarr;
                                                                    ManagedObjectReference _task_net = _service.ReconfigVM_Task(_vm_net, vmconfigspec);
                                                                    TaskInfo _info_net = (TaskInfo)oVMWare.getObjectProperty(_task_net, "info");
                                                                    while (_info_net.state == TaskInfoState.running)
                                                                        _info_net = (TaskInfo)oVMWare.getObjectProperty(_task_net, "info");
                                                                    if (_info_net.state == TaskInfoState.success)
                                                                    {
                                                                        strResult += "Network Adapter Created<br/>";
                                                                        boolCompleted = true;
                                                                    }
                                                                    else
                                                                        strError += "Network Adapter Was Not Created";
                                                                    //break;
                                                                }
                                                            }
                                                        }
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        // Only hits here if it is not a "DistributedVirtualPortgroup" (meaning it is a standard NETWORK object)
                                                        oLog.AddEvent(strName, strSerial, "VLAN (" + strNetworkName + ") must not be a DistributedVirtualPortgroup since it threw an error", LoggingType.Debug);
                                                    }
                                                }
                                                if (boolCompleted == false)
                                                {
                                                    strError = "Network Adapter Was Not Created ~ Could not find a port group (" + strRDPVLAN + ")";
                                                    oLog.AddEvent(strName, strSerial, strError, LoggingType.Error);
                                                }
                                            }
                                            else
                                            {
                                                strError = "Invalid build location (RDP) configuration ~ Missing VSphere VLAN...Class: " + oClass.Get(intClass, "name") + ", Environment: " + oEnvironment.Get(intEnv, "name") + ", Location: " + oLocation.GetFull(intAddress) + ", Resiliency: " + oResiliency.Get(intResiliency, "name");
                                                oLog.AddEvent(strName, strSerial, strError, LoggingType.Error);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        strError = "Invalid build location (RDP) configuration ~ Class: " + oClass.Get(intClass, "name") + ", Environment: " + oEnvironment.Get(intEnv, "name") + ", Location: " + oLocation.GetFull(intAddress) + ", Resiliency: " + oResiliency.Get(intResiliency, "name");
                                        oLog.AddEvent(strName, strSerial, "Invalid build location (RDP) configuration ~ classID:" + intClass.ToString() + ", envID:" + intEnv.ToString() + ", addID:" + intAddress.ToString() + ", Resiliency: " + oResiliency.Get(intResiliency, "name") + ", parentID:" + intParent.ToString(), LoggingType.Error);
                                    }
                                    AddResult(intWorkstation, intStep, intType, strResult, strError);
                                    break;
                                case 10:    // Attach Floppy Drive 
                                    oLog.AddEvent(strName, strSerial, "Starting Step 10 (Attach Floppy Drive)", LoggingType.Information);
                                    if (dblDrivePageFile > 0.00)   // If new VDI environment
                                    {
                                        strResult = "Floppy drive is not required";
                                    }
                                    else
                                    {
                                        ManagedObjectReference _vm_floppy = oVMWare.GetVM(strName);
                                        VirtualMachineConfigSpec _cs_floppy = new VirtualMachineConfigSpec();
                                        VirtualDeviceConfigSpec _dcs_floppy = new VirtualDeviceConfigSpec();
                                        _dcs_floppy.operation = VirtualDeviceConfigSpecOperation.add;
                                        _dcs_floppy.operationSpecified = true;
                                        VirtualDeviceConnectInfo _ci_floppy = new VirtualDeviceConnectInfo();
                                        _ci_floppy.startConnected = false;
                                        VirtualFloppy floppy = new VirtualFloppy();
                                        VirtualFloppyRemoteDeviceBackingInfo floppyBack = new VirtualFloppyRemoteDeviceBackingInfo();
                                        floppyBack.deviceName = "";
                                        floppy.backing = floppyBack;
                                        floppy.key = 8000;
                                        floppy.controllerKey = 400;
                                        floppy.controllerKeySpecified = true;
                                        floppy.connectable = _ci_floppy;
                                        _dcs_floppy.device = floppy;
                                        _cs_floppy.deviceChange = new VirtualDeviceConfigSpec[] { _dcs_floppy };
                                        ManagedObjectReference _task_floppy = _service.ReconfigVM_Task(_vm_floppy, _cs_floppy);
                                        TaskInfo _info_floppy = (TaskInfo)oVMWare.getObjectProperty(_task_floppy, "info");
                                        while (_info_floppy.state == TaskInfoState.running)
                                            _info_floppy = (TaskInfo)oVMWare.getObjectProperty(_task_floppy, "info");
                                        if (_info_floppy.state == TaskInfoState.success)
                                            strResult = "Floppy Drive Created";
                                        else
                                            strError = "Floppy Drive Was Not Created";
                                    }
                                    AddResult(intWorkstation, intStep, intType, strResult, strError);
                                    break;
                                case 11:    // Attach CD-ROM Drive
                                    oLog.AddEvent(strName, strSerial, "Starting Step 11 (Attach CD-ROM Drive)", LoggingType.Information);
                                    ManagedObjectReference _vm_cd = oVMWare.GetVM(strName);
                                    VirtualMachineConfigSpec _cs_cd = new VirtualMachineConfigSpec();
                                    VirtualDeviceConfigSpec _dcs_cd = new VirtualDeviceConfigSpec();
                                    _dcs_cd.operation = VirtualDeviceConfigSpecOperation.add;
                                    _dcs_cd.operationSpecified = true;
                                    VirtualDeviceConnectInfo _ci_cd = new VirtualDeviceConnectInfo();
                                    _ci_cd.startConnected = false;
                                    VirtualCdrom cd = new VirtualCdrom();
                                    VirtualCdromRemotePassthroughBackingInfo cdBack = new VirtualCdromRemotePassthroughBackingInfo();
                                    cdBack.exclusive = false;
                                    cdBack.deviceName = "";
                                    cd.backing = cdBack;
                                    cd.key = 3000;
                                    cd.controllerKey = 200;
                                    cd.controllerKeySpecified = true;
                                    cd.connectable = _ci_cd;
                                    _dcs_cd.device = cd;
                                    _cs_cd.deviceChange = new VirtualDeviceConfigSpec[] { _dcs_cd };
                                    ManagedObjectReference _task_cd = _service.ReconfigVM_Task(_vm_cd, _cs_cd);
                                    TaskInfo _info_cd = (TaskInfo)oVMWare.getObjectProperty(_task_cd, "info");
                                    while (_info_cd.state == TaskInfoState.running)
                                        _info_cd = (TaskInfo)oVMWare.getObjectProperty(_task_cd, "info");
                                    if (_info_cd.state == TaskInfoState.success)
                                        strResult = "CD-ROM Was Created";
                                    else
                                        strError = "CD-ROM Was Not Created";
                                    AddResult(intWorkstation, intStep, intType, strResult, strError);
                                    break;
                                case 12:     // Configure ZEUS 
                                    oLog.AddEvent(strName, strSerial, "Starting Step 12 (Configure ZEUS)", LoggingType.Information);
                                    strAsset = oAsset.GetVSG(strName, "WORKSTATION");
                                    if (strAsset == "")
                                        strAsset = oAsset.GetVSG("WORKSTATION");
                                    if (strAsset == "")
                                    {
                                        strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_INVENTORY_MANAGER");
                                        oFunction.SendEmail("Auto-Provisioning VSG Error", strVSG, "", strEMailIdsBCC, "Auto-Provisioning VSG Error", "<p><b>This message is to inform you that there are NO <u>WORKSTATION</u> VSG asset tag numbers left for virtual assets.</b><p><p>Please import more VSG numbers immediately. This problem is preventing builds from finishing.</p><p><b>NOTE:</b> This is automated email sent from the ClearView Auto-Provisioning tool. Please do not respond to this message.</p>", true, false);
                                        strError = "Invalid VSG number";
                                    }
                                    else
                                    {
                                        strSerial = oVMWare.GetSerial(strName);
                                        oAsset.Update(intAsset, strSerial, strAsset);
                                        DataSet dsVSG = oAsset.UpdateVSG(strAsset, strName, "WORKSTATION");
                                        strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_INVENTORY_MANAGER");
                                        if (dsVSG.Tables[0].Rows.Count < 20)
                                            oFunction.SendEmail("Auto-Provisioning VSG Configuration", strVSG, "", strEMailIdsBCC, "Auto-Provisioning VSG Configuration", "<p><b>This message is to inform you that there are less than 20 <u>WORKSTATION</u> VSG asset tag numbers left for virtual assets.</b><p><p>Please import more VSG numbers immediately to prevent errors.</p><p><b>NOTE:</b> This is automated email sent from the ClearView Auto-Provisioning tool. Please do not respond to this message.</p>", true, false);
                                        oZeus.DeleteBuild(strSerial);
                                        oZeus.DeleteBuildName(strName);
                                        oZeus.DeleteApps(strSerial);
                                        oZeus.DeleteLuns(strSerial);
                                        oZeus.DeleteResults(strSerial);
                                        string strArrayConfig = "BASIC";
                                        ManagedObjectReference _vm_mac = oVMWare.GetVM(strName);
                                        VirtualMachineConfigInfo _vminfo = (VirtualMachineConfigInfo)oVMWare.getObjectProperty(_vm_mac, "config");
                                        VirtualDevice[] _device = _vminfo.hardware.device;
                                        for (int ii = 0; ii < _device.Length; ii++)
                                        {
                                            // 4/29/2009: Change to only one NIC for PNC
                                            if (_device[ii].deviceInfo.label.ToUpper() == "NETWORK ADAPTER 1")
                                            {
                                                VirtualEthernetCard nic = (VirtualEthernetCard)_device[ii];
                                                strMACAddress = nic.macAddress;
                                                break;
                                            }
                                        }
                                        // Change the boot delay to 10 seconds
                                        VirtualMachineBootOptions oBootOptions = new VirtualMachineBootOptions();
                                        oBootOptions.bootDelay = 10000;
                                        oBootOptions.bootDelaySpecified = true;
                                        oBootOptions.bootRetryEnabled = true;
                                        oBootOptions.bootRetryEnabledSpecified = true;
                                        oBootOptions.bootRetryDelay = 10000;
                                        oBootOptions.bootRetryDelaySpecified = true;
                                        VirtualMachineConfigSpec _cs_boot_options = new VirtualMachineConfigSpec();
                                        _cs_boot_options.bootOptions = oBootOptions;
                                        ManagedObjectReference _task_boot_options = _service.ReconfigVM_Task(_vm_mac, _cs_boot_options);
                                        TaskInfo _info_boot_options = (TaskInfo)oVMWare.getObjectProperty(_task_boot_options, "info");
                                        while (_info_boot_options.state == TaskInfoState.running)
                                            _info_boot_options = (TaskInfo)oVMWare.getObjectProperty(_task_boot_options, "info");
                                        if (_info_boot_options.state == TaskInfoState.success)
                                        {
                                            oLog.AddEvent(strName, strSerial, "Boot delay changed to 10 seconds", LoggingType.Information);
                                        }
                                        else
                                            oLog.AddEvent(strName, strSerial, "Boot delay NOT changed", LoggingType.Warning);

                                        VirtualMachineConfigSpec _cs_swap = new VirtualMachineConfigSpec();
                                        _cs_swap.swapPlacement = "hostLocal";
                                        ManagedObjectReference _task_swap = _service.ReconfigVM_Task(_vm_mac, _cs_swap);
                                        TaskInfo _info_swap = (TaskInfo)oVMWare.getObjectProperty(_task_swap, "info");
                                        while (_info_swap.state == TaskInfoState.running)
                                            _info_swap = (TaskInfo)oVMWare.getObjectProperty(_task_swap, "info");
                                        if (_info_swap.state == TaskInfoState.success)
                                            oLog.AddEvent(strName, strSerial, "Swap File Configured", LoggingType.Information);
                                        else
                                            strError = "Swap File Was Not Configured";


                                        if (strError == "")
                                        {
                                            if (strMACAddress == "")
                                                strError = "ZEUS could not be updated with a blank MAC Address";
                                            else
                                            {
                                                oVMWare.UpdateGuestMAC(strName, strMACAddress);
                                                string strZeusBuildType = "";
                                                DataSet dsComponents = oWorkstation.GetComponentsSelected(intWorkstation);
                                                foreach (DataRow drComponent in dsComponents.Tables[0].Rows)
                                                {
                                                    if (strZeusBuildType == "")
                                                    {
                                                        strZeusBuildType = drComponent["zeus_build_type"].ToString();
                                                        break;
                                                    }
                                                    //oZeus.AddApp(strSerial, drComponent["zeus_code"].ToString(), drComponent["location"].ToString());
                                                }
                                                string strZeusOS = oOperatingSystem.Get(intOS, "zeus_os");
                                                string strZeusOSVersion = oOperatingSystem.Get(intOS, "zeus_os_version");
                                                if (strZeusBuildType == "")
                                                    strZeusBuildType = oOperatingSystem.Get(intOS, "zeus_build_type");
                                                oLog.AddEvent(strName, strSerial, "There are " + dsBuild.Tables[0].Rows.Count.ToString() + " (VMware WKS) build locations (RDP) for class: " + oClass.Get(intClassID, "name") + ", address: " + oLocation.GetFull(intAddressID) + " for workstation ID " + intWorkstation.ToString(), LoggingType.Information);
                                                if (dsBuild.Tables[0].Rows.Count > 0 && dsBuild.Tables[0].Rows[0]["source"].ToString() != "")
                                                    strSource = dsBuild.Tables[0].Rows[0]["source"].ToString();
                                                else
                                                    strSource = "DALRDP";
                                                strError = oZeus.AddBuild(0, 0, intWorkstation, strSerial, strAsset, strName.ToLower(), strArrayConfig, strZeusOS, strZeusOSVersion, Int32.Parse(oServicePack.Get(intSP, "number")), strZeusBuildType, strDomain, intEnvironment, strSource, 0, strMACAddress, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", 1);
                                                if (strError == "")
                                                {
                                                    oWorkstation.UpdateVirtualZeus(intWorkstation);
                                                    strResult = "IMAGE has been configured";
                                                }
                                            }
                                        }
                                    }
                                    AddResult(intWorkstation, intStep, intType, strResult, strError);
                                    break;
                                case 13:    // Power On Virtual Machine 
                                    oLog.AddEvent(strName, strSerial, "Starting Step 13 (Power On Virtual Machine)", LoggingType.Information);
                                    if (dblDrivePageFile > 0.00)   // If new VDI environment 
                                    {
                                        if (dsBuild.Tables[0].Rows[0]["rdp_mdt_ws"].ToString() != "")
                                        {
                                            string strRDPMDTWebService = dsBuild.Tables[0].Rows[0]["rdp_mdt_ws"].ToString();
                                            System.Net.NetworkCredential oCredentials = new System.Net.NetworkCredential(oVariable.ADUser(), oVariable.ADPassword(), oVariable.Domain());
                                            BuildSubmit oMDT = new BuildSubmit();
                                            oMDT.Credentials = oCredentials;
                                            oMDT.Url = strRDPMDTWebService;
                                            oMDT.Timeout = 30000;
                                            oLog.AddEvent(strName, strSerial, "Running ForceCleanup MDT on " + strRDPMDTWebService + " (" + strName + ", " + strMACAddress + ", " + "DesktopDeploymentShare" + ")", LoggingType.Information);
                                            oMDT.ForceCleanup(strName, strMACAddress, "DesktopDeploymentShare");
                                            string strHIDs = "NO";
                                            string strAppRating = "";
                                            if (intMnemonic > 0)
                                            {
                                                string strMnemonicCode = oMnemonic.Get(intMnemonic, "factory_code");
                                                strAppRating = oMnemonic.GetFeed(strMnemonicCode, MnemonicFeed.AppRating);
                                            }
                                            if (strAppRating == "")
                                                strAppRating = oMnemonic.Get(intMnemonic, "AppRating");
                                            if (strAppRating.ToUpper().Contains("SOX"))
                                            {
                                                if (oClass.IsDev(intClass) == false)
                                                    strHIDs = "YES";
                                            }
                                            //string[] strExtendedMDT = new string[] { "PNCBACKUPSOFTWARE:NONE", "IISInstall:NO", "HWConfig:DEFAULT", "ESMInstall:NO", "ClearViewInstall:YES", "Teamed2:DEFAULT" };
                                            string[] strExtendedMDT = new string[] { "PNCBACKUPSOFTWARE:NONE", "IISInstall:NO", "HWConfig:DEFAULT", "ESMInstall:NO", "ClearViewInstall:YES", "Teamed2:DEFAULT", "HIDSInstall:" + strHIDs };
                                            string strExtendedMDTs = "";
                                            foreach (string extendedMDT in strExtendedMDT)
                                            {
                                                if (strExtendedMDTs != "")
                                                    strExtendedMDTs += ", ";
                                                strExtendedMDTs += extendedMDT;
                                            }
                                            string strBootEnvironment = oOperatingSystem.Get(intOS, "boot_environment");
                                            string strTaskSequence = oOperatingSystem.Get(intOS, "task_sequence");
                                            oLog.AddEvent(strName, strSerial, "Registering MDT on " + strRDPMDTWebService + ", automatedBuild2 (" + strName + ", " + strMACAddress + ", " + strBootEnvironment + ", " + "provision" + ", " + strDomain + ", " + strTaskSequence + ", " + "DesktopDeploymentShare" + ", ExtendedValues=" + strExtendedMDTs + ")", LoggingType.Information);
                                            try
                                            {
                                                oMDT.automatedBuild2(strName, strMACAddress, strBootEnvironment, "provision", strDomain, strTaskSequence, "DesktopDeploymentShare", strExtendedMDT);
                                                oLog.AddEvent(strName, strSerial, "MDT has been configured", LoggingType.Information);
                                            }
                                            catch (Exception exMDT)
                                            {
                                                strError = "MDT has encountered an error ~ " + exMDT.Message;
                                                oLog.AddEvent(strName, strSerial, strError, LoggingType.Error);
                                            }
                                        }
                                        else
                                        {
                                            strError = "Invalid build location (RDP) configuration ~ Missing RDP Config...Class: " + oClass.Get(intClass, "name") + ", Environment: " + oEnvironment.Get(intEnv, "name") + ", Location: " + oLocation.GetFull(intAddress) + ", Resiliency: " + oResiliency.Get(intResiliency, "name");
                                            oLog.AddEvent(strName, strSerial, strError, LoggingType.Error);
                                        }
                                    }
                                    else
                                    {
                                        // Strip the :'s AND -'sout of MAC address
                                        string strMACAddressStrip = strMACAddress;
                                        while (strMACAddressStrip.Contains(":") == true)
                                            strMACAddressStrip = strMACAddressStrip.Replace(":", "");
                                        while (strMACAddressStrip.Contains("-") == true)
                                            strMACAddressStrip = strMACAddressStrip.Replace("-", "");
                                        if (intEnvironment == (int)CurrentEnvironment.CORPDMN || intEnvironment == (int)CurrentEnvironment.PNCNT_PROD)
                                        {
                                            System.Net.NetworkCredential oCredentials = new System.Net.NetworkCredential(oVariable.AltirisUsername(), oVariable.AltirisPassword(), oVariable.AltirisDomain());
                                            // Configure Altiris
                                            oLog.AddEvent(strName, strSerial, "There are " + dsBuild.Tables[0].Rows.Count.ToString() + " (VMware WKS) build locations (RDP) for class: " + oClass.Get(intClassID, "name") + ", address: " + oLocation.GetFull(intAddressID) + " for workstation ID " + intWorkstation.ToString(), LoggingType.Information);
                                            if (dsBuild.Tables[0].Rows.Count > 0 && dsBuild.Tables[0].Rows[0]["rdp_computer_ws"].ToString() != "" && dsBuild.Tables[0].Rows[0]["rdp_schedule_ws"].ToString() != "")
                                            {
                                                // Create Computer Object
                                                string strRDPComputerWebService = dsBuild.Tables[0].Rows[0]["rdp_computer_ws"].ToString();
                                                string strRDPScheduleWebService = dsBuild.Tables[0].Rows[0]["rdp_schedule_ws"].ToString();
                                                NCC.ClearView.Application.Core.AltirisWS_Computer.ComputerManagementService oComputer = new NCC.ClearView.Application.Core.AltirisWS_Computer.ComputerManagementService();
                                                oComputer.Credentials = oCredentials;
                                                oComputer.Url = strRDPComputerWebService;
                                                // Delete Computer Object if it Exists
                                                int intDeleteComputer = oComputer.GetComputerID(strName, 1);
                                                if (intDeleteComputer > 0)
                                                {
                                                    bool boolDelete = oComputer.DeleteComputer(intDeleteComputer);
                                                    oLog.AddEvent(strName, strSerial, "Found Duplicate Computer Object....Deleting Altiris ComputerID " + intDeleteComputer.ToString() + " on " + strRDPComputerWebService + "...result = " + boolDelete.ToString(), LoggingType.Information);
                                                }
                                                // Add Computer Object
                                                oLog.AddEvent(strName, strSerial, "Configuring Altiris (MAC: " + strMACAddressStrip + ") on " + strRDPScheduleWebService, LoggingType.Information);
                                                int intComputer = oComputer.AddBasicVirtualComputer(-1, strName, oAsset.Get(intAsset, "asset"), oAsset.Get(intAsset, "serial"), strMACAddressStrip, 2, "");
                                                // Assign Schedule
                                                NCC.ClearView.Application.Core.altirisws.dsjob oJob = new NCC.ClearView.Application.Core.altirisws.dsjob();
                                                oJob.Credentials = oCredentials;
                                                oJob.Url = strRDPScheduleWebService;
                                                oLog.AddEvent(strName, strSerial, "Adding Altiris Job (" + oOperatingSystem.Get(intOS, "altiris") + ")", LoggingType.Information);
                                                oJob.ScheduleNow(strName, oOperatingSystem.Get(intOS, "altiris"));
                                                oLog.AddEvent(strName, strSerial, "Finished Configuring Altiris", LoggingType.Information);
                                            }
                                            else
                                            {
                                                strError = "Invalid build location (RDP) configuration ~ Class: " + oClass.Get(intClass, "name") + ", Environment: " + oEnvironment.Get(intEnv, "name") + ", Location: " + oLocation.GetFull(intAddress) + ", Resiliency: " + oResiliency.Get(intResiliency, "name");
                                                oLog.AddEvent(strName, strSerial, "Invalid build location (RDP) configuration ~ classID:" + intClass.ToString() + ", envID:" + intEnv.ToString() + ", addID:" + intAddress.ToString() + ", Resiliency: " + oResiliency.Get(intResiliency, "name") + ", parentID:" + intParent.ToString(), LoggingType.Information);
                                            }
                                        }
                                    }
                                    if (strError == "")
                                    {
                                        oLog.AddEvent(strName, strSerial, "Waiting 60 seconds for Active Directory to Sychronize...", LoggingType.Debug);
                                        Thread.Sleep(60000); // Wait 60 seconds
                                        ManagedObjectReference _vm_power = oVMWare.GetVM(strName);
                                        GuestInfo ginfo_power = (GuestInfo)oVMWare.getObjectProperty(_vm_power, "guest");
                                        if (ginfo_power.guestState.ToUpper() == "NOTRUNNING" || ginfo_power.guestState.ToUpper() == "UNKNOWN")
                                        {
                                            ManagedObjectReference _task_power = _service.PowerOnVM_Task(_vm_power, null);
                                            TaskInfo _info_power = (TaskInfo)oVMWare.getObjectProperty(_task_power, "info");
                                            while (_info_power.state == TaskInfoState.running)
                                                _info_power = (TaskInfo)oVMWare.getObjectProperty(_task_power, "info");
                                            if (_info_power.state == TaskInfoState.success)
                                            {
                                                strResult = "Virtual Machine Powered On";
                                                oLog.AddEvent(strName, strSerial, "Virtual Machine Powered On", LoggingType.Information);
                                                oVMWare.UpdateGuestDone(strName, 1);
                                            }
                                            else
                                            {
                                                strError = "Virtual Machine Was Not Powered On";
                                                oLog.AddEvent(strName, strSerial, "Virtual Machine Was Not Powered On (" + ginfo_power.guestState.ToUpper() + ")", LoggingType.Information);
                                            }
                                        }
                                        else
                                        {
                                            strResult = "Virtual Machine Was Already Powered On (" + ginfo_power.guestState + ")";
                                            oLog.AddEvent(strName, strSerial, "Virtual Machine Was Already Powered On (" + ginfo_power.guestState + ")", LoggingType.Information);
                                            oVMWare.UpdateGuestDone(strName, 1);
                                        }
                                    }
                                    AddResult(intWorkstation, intStep, intType, strResult, strError);
                                    break;
                                case 14:     // ZEUS 
                                    if (intLogging > 1)
                                        oEventLog.WriteEntry(strName + " (VMware WKS): " + "Starting Step 14 (ZEUS)", EventLogEntryType.Information);
                                    DataSet dsZeusError = oZeus.GetResult(strSerial, 1);
                                    if (strDHCP != "0" && strDHCP != "")
                                    {
                                        // Configure Altiris
                                        oLog.AddEvent(strName, strSerial, "There are " + dsBuild.Tables[0].Rows.Count.ToString() + " (VMware WKS) build locations (RDP) for class: " + oClass.Get(intClassID, "name") + ", address: " + oLocation.GetFull(intAddressID) + " for workstation ID " + intWorkstation.ToString(), LoggingType.Information);
                                        if (dsBuild.Tables[0].Rows.Count > 0)
                                        {
                                            if (dblDrivePageFile > 0.00)   // If new VDI environment 
                                            {
                                                if (dsBuild.Tables[0].Rows[0]["rdp_mdt_ws"].ToString() != "")
                                                {
                                                    // Cleanup MDT
                                                    string strRDPMDTWebService = dsBuild.Tables[0].Rows[0]["rdp_mdt_ws"].ToString();
                                                    System.Net.NetworkCredential oCredentials = new System.Net.NetworkCredential(oVariable.ADUser(), oVariable.ADPassword(), oVariable.Domain());
                                                    BuildSubmit oMDT = new BuildSubmit();
                                                    oMDT.Credentials = oCredentials;
                                                    oMDT.Url = strRDPMDTWebService;
                                                    oLog.AddEvent(strName, strSerial, "Cleaning MDT on " + strRDPMDTWebService + " (" + strName + ", " + strMACAddress + ", " + "DesktopDeploymentShare" + ")", LoggingType.Information);
                                                    oMDT.Cleanup(strName, strMACAddress, "DesktopDeploymentShare");
                                                    oLog.AddEvent(strName, strSerial, "MDT has been cleared", LoggingType.Information);
                                                }
                                                else
                                                {
                                                    strError = "Invalid build location (RDP) configuration ~ Missing RDP Config...Class: " + oClass.Get(intClass, "name") + ", Environment: " + oEnvironment.Get(intEnv, "name") + ", Location: " + oLocation.GetFull(intAddress) + ", Resiliency: " + oResiliency.Get(intResiliency, "name");
                                                    oLog.AddEvent(strName, strSerial, strError, LoggingType.Error);
                                                }
                                            }
                                            else
                                            {
                                                if ((intEnvironment == (int)CurrentEnvironment.CORPDMN || intEnvironment == (int)CurrentEnvironment.PNCNT_PROD) && dsBuild.Tables[0].Rows[0]["rdp_computer_ws"].ToString() != "" && dsBuild.Tables[0].Rows[0]["rdp_schedule_ws"].ToString() != "")
                                                {
                                                    // Delete Computer Object
                                                    string strRDPComputerWebService = dsBuild.Tables[0].Rows[0]["rdp_computer_ws"].ToString();
                                                    System.Net.NetworkCredential oCredentials2 = new System.Net.NetworkCredential(oVariable.AltirisUsername(), oVariable.AltirisPassword(), oVariable.AltirisDomain());
                                                    NCC.ClearView.Application.Core.AltirisWS_Computer.ComputerManagementService oComputer = new NCC.ClearView.Application.Core.AltirisWS_Computer.ComputerManagementService();
                                                    oComputer.Credentials = oCredentials2;
                                                    oComputer.Url = strRDPComputerWebService;
                                                    int intDeleteComputer = oComputer.GetComputerID(strName, 1);
                                                    if (intDeleteComputer > 0)
                                                    {
                                                        bool boolDelete = oComputer.DeleteComputer(intDeleteComputer);
                                                        oLog.AddEvent(strName, strSerial, "Finished Deleting Altiris Computer Object", LoggingType.Information);
                                                    }
                                                    else
                                                        oLog.AddEvent(strName, strSerial, "Finished Deleting Altiris Computer Object (Did not exist)", LoggingType.Information);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            strError = "Invalid build location (RDP) configuration ~ Class: " + oClass.Get(intClass, "name") + ", Environment: " + oEnvironment.Get(intEnv, "name") + ", Location: " + oLocation.GetFull(intAddress) + ", Resiliency: " + oResiliency.Get(intResiliency, "name");
                                            oLog.AddEvent(strName, strSerial, "Invalid build location (RDP) configuration ~ classID:" + intClass.ToString() + ", envID:" + intEnv.ToString() + ", addID:" + intAddress.ToString() + ", Resiliency: " + oResiliency.Get(intResiliency, "name") + ", parentID:" + intParent.ToString(), LoggingType.Error);
                                        }
                                        AddResult(intWorkstation, intStep, intType, oOnDemand.GetStep(intStep, "done"), strError);
                                    }
                                    else if ((oSpan.Hours > 4 || dsZeusError.Tables[0].Rows.Count > 0) && boolZeusError == false)
                                    {
                                        //string strCodeOwner = "XCJN736;";
                                        string strCodeOwner = "PP46725@pnc.com;PT43007@pnc.com";
                                        string strCodeType = "ZEUS";
                                        if (dsZeusError.Tables[0].Rows.Count > 0)
                                        {
                                            strError = strCodeType + " ERROR: " + dsZeusError.Tables[0].Rows[0]["message"].ToString();
                                            oLog.AddEvent(strName, strSerial, strError, LoggingType.Error);
                                            strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_PROVISIONING_SUPPORT");
                                            if (intEnvironment == (int)CurrentEnvironment.CORPDMN || intEnvironment == (int)CurrentEnvironment.PNCNT_PROD)
                                                oFunction.SendEmail("Auto-Provisioning " + strCodeType + " ERROR: " + strName, strCodeOwner, "", strEMailIdsBCC, "Auto-Provisioning " + strCodeType + " ERROR: " + strName, "<p><b>This message is to inform you that the workstation " + strName + " has encountered an error in " + strCodeType + "!</b><p><p>Error Message: " + strError + "<br/>Serial Number: " + oAsset.Get(intAsset, "serial").ToUpper() + "<br/>Asset Tag: " + oAsset.Get(intAsset, "asset").ToUpper() + "<br/>Model: " + oModelsProperties.Get(intModel, "name").ToUpper() + "<br/>Step #: " + intStep.ToString() + "<br/>Step: " + strStep + "<br/>DataCenter: " + oVMWare.DataCenter() + "<br/>Virtual Center: " + oVMWare.VirtualCenter() + "</p><p>Once you have resolved this issue, please <a href=\"" + oVariable.URL() + "/build_fixed.aspx?type=w&id=" + oFunction.encryptQueryString(intWorkstation.ToString()) + "\" target=\"_blank\">click here</a> to resume the build.</p><p><b>NOTE:</b> This is automated email sent from the ClearView Auto-Provisioning tool. Please do not respond to this message.</p>", true, false);
                                        }
                                        else
                                        {
                                            strError = "Sitting at the IMAGING step for more than FOUR (4) hours!";
                                            oLog.AddEvent(strName, strSerial, strError, LoggingType.Error);
                                            strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_PROVISIONING_SUPPORT");
                                            if (intEnvironment == (int)CurrentEnvironment.CORPDMN || intEnvironment == (int)CurrentEnvironment.PNCNT_PROD)
                                                oFunction.SendEmail("Auto-Provisioning " + strCodeType + " Problem: " + strName, strCodeOwner, "", strEMailIdsBCC, "Auto-Provisioning " + strCodeType + " Problem: " + strName, "<p><b>This message is to inform you that the workstation " + strName + " has been sitting at the " + strCodeType + " step for more than FOUR (4) hours!</b><p><p>Serial Number: " + oAsset.Get(intAsset, "serial").ToUpper() + "<br/>Asset Tag: " + oAsset.Get(intAsset, "asset").ToUpper() + "<br/>Model: " + oModelsProperties.Get(intModel, "name").ToUpper() + "<br/>DataCenter: " + oVMWare.DataCenter() + "<br/>Virtual Center: " + oVMWare.VirtualCenter() + "</p><p>Once you have resolved this issue, please <a href=\"" + oVariable.URL() + "/build_fixed.aspx?type=w&id=" + oFunction.encryptQueryString(intWorkstation.ToString()) + "\" target=\"_blank\">click here</a> to resume the build.</p><p><b>NOTE:</b> This is automated email sent from the ClearView Auto-Provisioning tool. Please do not respond to this message.</p>", true, false);
                                        }
                                        oWorkstation.UpdateVirtualZeusError(intWorkstation, 1);
                                        // Add Error message so it displays in provisioning windows
                                        AddResult(intWorkstation, intStep, intType, "", strError);
                                    }
                                    break;
                                case 15:    // Create Active Directory Accounts
                                    oLog.AddEvent(strName, strSerial, "Starting Step 15 (Create AD Accounts)", LoggingType.Information);
                                    DataSet dsAccounts = oWorkstation.GetAccountsVMware(intWorkstation);
                                    foreach (DataRow drAccount in dsAccounts.Tables[0].Rows)
                                    {
                                        string strID = oUser.GetName(Int32.Parse(drAccount["userid"].ToString()), true);
                                        string strXID = strID;
                                        if (intDomainEnvironment != (int)CurrentEnvironment.CORPDMN
                                            && intDomainEnvironment != (int)CurrentEnvironment.PNCQA
                                            && intDomainEnvironment != (int)CurrentEnvironment.PNCRND
                                            && intDomainEnvironment != (int)CurrentEnvironment.PNCUAT
                                            && intDomainEnvironment != (int)CurrentEnvironment.PNCNT_PROD) // Not Production
                                        {
                                            strID = "E" + strID.Substring(1);
                                            if (oAD.Search(strID, false) == null)
                                            {
                                                strID = "T" + strID.Substring(1);
                                                if (oAD.Search(strID, false) == null)
                                                {
                                                    strResult = oAD.CreateUser(strID, strID, strID, "Abcd1234", "", "", "");
                                                    oLog.AddEvent(strName, strSerial, "AD CreateUser(" + strID + ") Result: " + strResult, (strResult.Trim() == "" ? LoggingType.Information : LoggingType.Warning));
                                                }
                                            }
                                            if (drAccount["admin"].ToString() == "1")
                                            {
                                                strResult = oAD.JoinGroup(strID, strGroupAdmin, 0);
                                                oLog.AddEvent(strName, strSerial, "AD JoinGroup(" + strID + "," + strGroupAdmin + ") Result: " + strResult, (strResult.Trim() == "" ? LoggingType.Information : LoggingType.Warning));
                                            }
                                        }
                                        if (drAccount["remote"].ToString() == "1")
                                        {
                                            int intAccount = oUser.GetId(strID);
                                            string strIDx = "";
                                            string strIDpnc = "";
                                            if (dblDrivePageFile > 0.00 && intAccount > 0)   // If new VDI environment
                                            {
                                                strIDx = oUser.Get(intAccount, "xid").Trim().ToUpper();
                                                strIDpnc = oUser.Get(intAccount, "pnc_id").Trim().ToUpper();

                                                strResult = oAD.JoinGroup(strIDx, strGroupRemote, 4);
                                                if (strResult == "")
                                                {
                                                    strResult += "The user " + strIDx + " was successfully added to the group " + strGroupRemote;
                                                    oLog.AddEvent(strName, strSerial, strResult, LoggingType.Information);
                                                }
                                                else
                                                {
                                                    strError = "The NCC user was NOT added to the group ~ User: " + strIDx + ", Group: " + strGroupRemote + strResult;
                                                    oLog.AddEvent(strName, strSerial, strError, LoggingType.Error);
                                                }

                                                strResult = oAD.JoinGroup(strIDpnc, strGroupRemote, 0);
                                                if (strResult == "")
                                                {
                                                    strResult += "<br/>The user " + strIDpnc + " was successfully added to the group " + strGroupRemote;
                                                    oLog.AddEvent(strName, strSerial, strResult, LoggingType.Information);
                                                }
                                                else
                                                {
                                                    strError = "The PNC user was NOT added to the group ~ User: " + strIDpnc + ", Group: " + strGroupRemote + strResult;
                                                    oLog.AddEvent(strName, strSerial, strError, LoggingType.Error);
                                                }
                                            }
                                            else
                                            {
                                                strResult = oAD.JoinGroup(strID, strGroupRemote, 0);
                                                oLog.AddEvent(strName, strSerial, "AD JoinGroup(" + strID + "," + strGroupRemote + ") Result: " + strResult, (strResult.Trim() == "" ? LoggingType.Information : LoggingType.Warning));
                                            }

                                            if (intEnvironment == (int)CurrentEnvironment.CORPDMN || intEnvironment == (int)CurrentEnvironment.PNCNT_PROD)
                                            {
                                                // Get Citrix Code for Appending
                                                string strCitrix = oOperatingSystem.Get(intOS, "citrix_config");
                                                // Configure INI File   FORMAT: EMJY13C=TXPVM001,XP,OHCLEVWH4004
                                                if (intDomainEnvironment != (int)CurrentEnvironment.CORPDMN
                                                    && intDomainEnvironment != (int)CurrentEnvironment.PNCNT_PROD) // Not Production
                                                {
                                                    // Test (add to both Prod and Test for Citrix)
                                                    AddFileLine(strName, strINIProd, strXID + "=" + strName + "," + strCitrix + "," + strHost, 4);
                                                    //AddFileLine(strName, strINITest, strID + "=" + strName + "," + strCitrix + "," + strHost, 3);
                                                }
                                                else
                                                {
                                                    // Production
                                                    if (dblDrivePageFile > 0.00 && intAccount > 0)   // If new VDI environment
                                                    {
                                                        if (strIDx != "")
                                                            AddFileLine(strName, strINIProd, strIDx + "=" + strName + "." + oVar.FullyQualified() + "," + strCitrix + "," + strHost, 4);
                                                        else
                                                            oLog.AddEvent(strName, strSerial, "The XID for (" + strID + ") does not exist", LoggingType.Error);
                                                        if (strIDpnc != "")
                                                            AddFileLine(strName, strINIProd, strIDpnc + "=" + strName + "." + oVar.FullyQualified() + "," + strCitrix + "," + strHost, 4);
                                                        else
                                                            oLog.AddEvent(strName, strSerial, "The PNCID for (" + strID + ") does not exist", LoggingType.Error);
                                                    }
                                                    else
                                                        AddFileLine(strName, strINIProd, strID + "=" + strName + "," + strCitrix + "," + strHost, 4);
                                                }
                                            }
                                        }
                                    }
                                    AddResult(intWorkstation, intStep, intType, strResult, strError);
                                    break;
                                case 16:    // Install Components 
                                    oLog.AddEvent(strName, strSerial, "Starting Step 16 (Install Components)", LoggingType.Information);
                                    DataSet dsComponentsInstall = oWorkstation.GetComponentsSelected(intWorkstation);
                                    if (dsComponentsInstall.Tables[0].Rows.Count == 0)
                                        AddResult(intWorkstation, intStep, intType, "No components to install", strError);
                                    else
                                    {
                                        bool boolDone = true;
                                        foreach (DataRow drComponent in dsComponentsInstall.Tables[0].Rows)
                                        {
                                            int intComponent = Int32.Parse(drComponent["componentid"].ToString());
                                            if (drComponent["done"].ToString() == "-2")
                                            {
                                                oWorkstation.UpdateComponents(intWorkstation, intComponent, -1);
                                                boolDone = false;
                                            }
                                            if (drComponent["done"].ToString() == "-1" || drComponent["done"].ToString() == "0")
                                                boolDone = false;
                                            else if (drComponent["done"].ToString() == "1")
                                            {
                                                oOnDemand.UpdateStepDoneWorkstation(intWorkstation, intStep, "Successfully installed " + drComponent["name"].ToString() + "<br/>", 0, true, false);
                                                oWorkstation.UpdateComponents(intWorkstation, intComponent, 2);
                                            }
                                        }
                                        if (boolDone == true)
                                            oWorkstation.NextVirtualStep(intWorkstation);
                                    }
                                    break;
                                case 17:    // Change Network Adapter
                                    oLog.AddEvent(strName, strSerial, "Starting Step 17 (Change Network Adapter)", LoggingType.Information);
                                    ManagedObjectReference mobSwitch = null;
                                    ManagedObjectReference _vm_net2 = oVMWare.GetVM(strName);
                                    if (intVlan == 0)
                                    {
                                        if (intNetwork == 0)
                                        {
                                            ManagedObjectReference datacenterRef = oVMWare.GetDataCenter();
                                            ManagedObjectReference netFolderRef = (ManagedObjectReference)oVMWare.getObjectProperty(datacenterRef, "networkFolder");
                                            ManagedObjectReference[] netList = (ManagedObjectReference[])oVMWare.getObjectProperty(netFolderRef, "childEntity");
                                            ManagedObjectReference net = null;
                                            oLog.AddEvent(strName, strSerial, "Trying to find network..." + strDesktopNetwork, LoggingType.Debug);
                                            foreach (ManagedObjectReference network in netList)
                                            {
                                                if (((string)oVMWare.getObjectProperty(network, "name")) == strDesktopNetwork)
                                                {
                                                    net = network;
                                                    oLog.AddEvent(strName, strSerial, "Found network = " + strDesktopNetwork, LoggingType.Debug);
                                                    break;
                                                }
                                            }

                                            if (net != null)
                                            {
                                                //intNetwork = oIPAddresses.GetNetwork_(intClassID, intEnvironmentID, intAddressID, 0, 0, 0, 0, 0, 0, (intInternal == 0 ? 1 : 0), intInternal, intRecovery, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, intEnvironment);
                                                DataSet dsNetwork = oIPAddresses.GetNetwork_(intClassID, intEnvironmentID, intAddressID, 0, 0, 0, 0, 0, 0, (intInternal == 0 ? 1 : 0), intInternal, intRecovery, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, intEnvironment);
                                                foreach (DataRow drNetwork in dsNetwork.Tables[0].Rows)
                                                {
                                                    int intNetworkID = Int32.Parse(drNetwork["networkid"].ToString());
                                                    //int intMin = Int32.Parse(drNetwork["min4"].ToString());
                                                    //int intMax = Int32.Parse(drNetwork["max4"].ToString());
                                                    string strNotify = drNetwork["ips_notify"].ToString();
                                                    int intNotify = -1;
                                                    if (drNetwork["ips_left"].ToString() != "")
                                                        intNotify = Int32.Parse(drNetwork["ips_left"].ToString());

                                                    string strNetwork = "";
                                                    int intVLAN = Int32.Parse(oIPAddresses.GetNetwork(intNetworkID, "vlanid"));
                                                    DataSet dsVlan = oVMWare.GetVlanAssociations(intVLAN, intCluster);
                                                    if (dsVlan.Tables[0].Rows.Count > 0)
                                                    {
                                                        int intVlanCheck = Int32.Parse(dsVlan.Tables[0].Rows[0]["vmware_vlanid"].ToString());
                                                        strNetwork = oVMWare.GetVlan(intVlanCheck, "name");
                                                        //int intTotalUsed = Int32.Parse(drNetwork["total"].ToString());
                                                        int intPortsAvailable = 0;
                                                        int intPortsUsed = 0;

                                                        ManagedObjectReference[] switchList = (ManagedObjectReference[])oVMWare.getObjectProperty(net, "childEntity");
                                                        foreach (ManagedObjectReference swit in switchList)
                                                        {
                                                            try
                                                            {
                                                                oLog.AddEvent(strName, strSerial, "Checking to see if " + ((string)oVMWare.getObjectProperty(swit, "name")) + " = " + strNetwork, LoggingType.Debug);
                                                                if (((string)oVMWare.getObjectProperty(swit, "name")) == strNetwork)
                                                                {
                                                                    oLog.AddEvent(strName, strSerial, "Found switch = " + strNetwork, LoggingType.Debug);
                                                                    DVPortgroupConfigInfo pgConfig = (DVPortgroupConfigInfo)oVMWare.getObjectProperty(swit, "config");
                                                                    //Response.Write("Num Ports = " + pgConfig.numPorts.ToString() + "<br/>");
                                                                    ManagedObjectReference[] pgVM = (ManagedObjectReference[])oVMWare.getObjectProperty(swit, "vm");
                                                                    intPortsUsed = pgVM.Length;
                                                                    //Response.Write("Num Machines = " + pgVM.Length.ToString() + "<br/>");
                                                                    intPortsAvailable = pgConfig.numPorts - intPortsUsed;
                                                                    if (intPortsAvailable > 0)
                                                                        mobSwitch = swit;
                                                                    //Response.Write("Ports Available = " + intAvailable.ToString() + "<br/>");
                                                                    oLog.AddEvent(strName, strSerial, strNetwork + " has " + intPortsAvailable.ToString() + " of " + pgConfig.numPorts.ToString() + " ports available", LoggingType.Information);
                                                                    break;
                                                                }
                                                            }
                                                            catch (Exception exSwit)
                                                            {
                                                                oLog.AddEvent(strName, strSerial, "Error: " + exSwit.Message, LoggingType.Debug);
                                                            }
                                                        }


                                                        //int intTotalAvailable = (intMax - intMin);
                                                        //if (intTotalUsed < intTotalAvailable)
                                                        if (intPortsAvailable > 0)
                                                        {
                                                            int intTotalLeft = intPortsAvailable - intPortsUsed;
                                                            if (intTotalLeft <= intNotify)
                                                            {
                                                                // Send notification
                                                                string strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_IPADDRESS");
                                                                oLog.AddEvent(strName, strSerial, strNetwork + " has " + intTotalLeft.ToString() + " ports left (less than " + intNotify.ToString() + "). Sending notification to " + strEMailIdsBCC, LoggingType.Information);
                                                                oFunction.SendEmail("WARNING: DHCP Addresses", strNotify, "", strEMailIdsBCC, "WARNING: DHCP Addresses", "<p><b>This message is to inform you that you are running out of DHCP addresses...</b></p><p>VLAN: " + drNetwork["vlan"].ToString() + "<br/>DHCP Range: " + drNetwork["add1"].ToString() + "." + drNetwork["add2"].ToString() + "." + drNetwork["add3"].ToString() + "." + drNetwork["min4"].ToString() + " - " + drNetwork["add1"].ToString() + "." + drNetwork["add2"].ToString() + "." + drNetwork["add3"].ToString() + "." + drNetwork["max4"].ToString() + "</p><p>There are less than " + intNotify.ToString() + " DHCP addresses available.</p>", true, false);
                                                            }
                                                            intNetwork = intNetworkID;
                                                            intVlan = intVlanCheck;
                                                            oWorkstation.UpdateVirtualNetwork(intWorkstation, intNetwork);
                                                            oVMWare.UpdateGuestVlan(strName, intVlan);
                                                            oLog.AddEvent(strName, strSerial, "VLAN " + drNetwork["vlan"].ToString() + " (NetworkID = " + intNetwork.ToString() + ") has been assigned.", LoggingType.Information);
                                                        }
                                                        else
                                                            mobSwitch = null;
                                                        if (intNetwork > 0)
                                                            break;
                                                    }
                                                    else
                                                        oLog.AddEvent(strName, strSerial, "There are no VMware associations ~ (VLAN " + oIPAddresses.GetVlan(intVLAN, "vlan") + ")", LoggingType.Warning);
                                                }

                                                if (intNetwork == 0)
                                                {
                                                    strError = "Could not find a DHCP network range ~ (intClassID:" + intClassID.ToString() + ",intEnvironmentID:" + intEnvironmentID.ToString() + ",intAddressID:" + intAddressID.ToString() + ",external: " + (intInternal == 0 ? "1" : "0") + ",internal: " + intInternal.ToString() + ",recovery: " + intRecovery.ToString() + ")";
                                                    oLog.AddEvent(strName, strSerial, strError, LoggingType.Error);
                                                }
                                            }
                                            else
                                            {
                                                strError = "Could not find the desktop network ~ " + strDesktopNetwork;
                                                oLog.AddEvent(strName, strSerial, strError, LoggingType.Error);
                                            }
                                        }
                                        if (strError == "" && intVlan == 0)
                                        {
                                            strError = "Could not find a DHCP network range ~ (intClassID:" + intClassID.ToString() + ",intEnvironmentID:" + intEnvironmentID.ToString() + ",intAddressID:" + intAddressID.ToString() + ",external: " + (intInternal == 0 ? "1" : "0") + ",internal: " + intInternal.ToString() + ",recovery: " + intRecovery.ToString() + ")";
                                            oLog.AddEvent(strName, strSerial, strError, LoggingType.Error);
                                        }
                                    }
                                    if (intVlan > 0)
                                    {
                                        string strVLAN = oVMWare.GetVlan(intVlan, "name");
                                        if (strVLAN != "")
                                        {
                                            // Shutdown guest os
                                            oLog.AddEvent(strName, strSerial, "Shutting down guest OS to change VLAN to " + strVLAN, LoggingType.Information);
                                            VirtualMachineRuntimeInfo run_vlan = (VirtualMachineRuntimeInfo)oVMWare.getObjectProperty(_vm_net2, "runtime");
                                            if (run_vlan.powerState != VirtualMachinePowerState.poweredOff)
                                            {
                                                ManagedObjectReference _task_shutdown = _service.PowerOffVM_Task(_vm_net2);
                                                oLog.AddEvent(strName, strSerial, "Guest OS shutdown Started", LoggingType.Information);
                                                TaskInfo _info_shutdown = (TaskInfo)oVMWare.getObjectProperty(_task_shutdown, "info");
                                                while (_info_shutdown.state == TaskInfoState.running)
                                                    _info_shutdown = (TaskInfo)oVMWare.getObjectProperty(_task_shutdown, "info");
                                                if (_info_shutdown.state == TaskInfoState.success)
                                                {
                                                    int intAttempt = 0;
                                                    for (intAttempt = 0; intAttempt < 20 && run_vlan.powerState != VirtualMachinePowerState.poweredOff; intAttempt++)
                                                    {
                                                        run_vlan = (VirtualMachineRuntimeInfo)oVMWare.getObjectProperty(_vm_net2, "runtime");
                                                        int intAttemptLeft = (20 - intAttempt);
                                                        oLog.AddEvent(strName, strSerial, "Workstation still on...waiting 5 seconds (" + intAttemptLeft.ToString() + " tries left)", LoggingType.Information);
                                                        Thread.Sleep(3000);
                                                    }
                                                }
                                            }
                                            else
                                                oLog.AddEvent(strName, strSerial, "Guest OS was already shutdown (" + run_vlan.powerState.ToString() + ")", LoggingType.Information);
                                            if (run_vlan.powerState != VirtualMachinePowerState.poweredOff)
                                                strError = "There was a problem shutting down the guest";
                                            else
                                            {
                                                oLog.AddEvent(strName, strSerial, "Guest OS shutdown Finished", LoggingType.Information);
                                                VirtualMachineConfigInfo vminfo = (VirtualMachineConfigInfo)oVMWare.getObjectProperty(_vm_net2, "config");
                                                VirtualDevice[] test = vminfo.hardware.device;
                                                for (int ii = 0; ii < test.Length; ii++)
                                                {
                                                    if (test[ii].deviceInfo.label.ToUpper() == "NETWORK ADAPTER 1")
                                                    {
                                                        if (intClusterVersion == 0)
                                                        {
                                                            VirtualEthernetCard nic = (VirtualEthernetCard)test[ii];
                                                            VirtualDeviceConfigSpec[] configspecarr = new VirtualDeviceConfigSpec[1];
                                                            VirtualEthernetCardNetworkBackingInfo vecnbi = new VirtualEthernetCardNetworkBackingInfo();
                                                            vecnbi.deviceName = strVLAN;
                                                            VirtualDeviceConfigSpec newethdevicespec = new VirtualDeviceConfigSpec();
                                                            newethdevicespec.device = nic;
                                                            nic.backing = vecnbi;
                                                            newethdevicespec.operation = VirtualDeviceConfigSpecOperation.edit;
                                                            newethdevicespec.operationSpecified = true;
                                                            configspecarr[0] = newethdevicespec;
                                                            VirtualMachineConfigSpec vmconfigspec = new VirtualMachineConfigSpec();
                                                            vmconfigspec.deviceChange = configspecarr;
                                                            oLog.AddEvent(strName, strSerial, "Network Adapter Changing to VLAN:" + strVLAN, LoggingType.Information);
                                                            ManagedObjectReference _task_net = _service.ReconfigVM_Task(_vm_net2, vmconfigspec);
                                                            TaskInfo _info_net = (TaskInfo)oVMWare.getObjectProperty(_task_net, "info");
                                                            while (_info_net.state == TaskInfoState.running)
                                                                _info_net = (TaskInfo)oVMWare.getObjectProperty(_task_net, "info");
                                                            if (_info_net.state == TaskInfoState.success)
                                                                oLog.AddEvent(strName, strSerial, "Network Adapter Reconfigured", LoggingType.Information);
                                                            else
                                                            {
                                                                oLog.AddEvent(strName, strSerial, "Network Adapter NOT Reconfigured", LoggingType.Error);
                                                                strError = "Network Adapter NOT Reconfigured";
                                                            }
                                                        }
                                                        if (intClusterVersion == 1)
                                                        {
                                                            bool boolCompleted = false;
                                                            string strPortGroupKey = "";
                                                            if (mobSwitch != null)
                                                            {
                                                                oLog.AddEvent(strName, strSerial, "Trying Switch " + oVMWare.getObjectProperty(mobSwitch, "name"), LoggingType.Information);
                                                                try
                                                                {
                                                                    object oPortConfig = oVMWare.getObjectProperty(mobSwitch, "config");
                                                                    if (oPortConfig != null)
                                                                    {
                                                                        DVPortgroupConfigInfo oPort = (DVPortgroupConfigInfo)oPortConfig;
                                                                        if (oPort.key != strPortGroupKey)
                                                                        {
                                                                            strPortGroupKey = oPort.key;
                                                                            ManagedObjectReference oSwitch = oPort.distributedVirtualSwitch;
                                                                            string strSwitchUUID = (string)oVMWare.getObjectProperty(oSwitch, "uuid");
                                                                            oLog.AddEvent(strName, strSerial, "Trying..." + strPortGroupKey + "(" + strSwitchUUID + ")", LoggingType.Information);

                                                                            VirtualEthernetCard nic = (VirtualEthernetCard)test[ii];
                                                                            VirtualDeviceConfigSpec[] configspecarr = new VirtualDeviceConfigSpec[1];
                                                                            VirtualEthernetCardDistributedVirtualPortBackingInfo vecdvpbi = new VirtualEthernetCardDistributedVirtualPortBackingInfo();
                                                                            DistributedVirtualSwitchPortConnection connection = new DistributedVirtualSwitchPortConnection();
                                                                            connection.portgroupKey = strPortGroupKey;
                                                                            connection.switchUuid = strSwitchUUID;
                                                                            vecdvpbi.port = connection;
                                                                            nic.backing = vecdvpbi;
                                                                            VirtualDeviceConfigSpec newethdevicespec = new VirtualDeviceConfigSpec();
                                                                            newethdevicespec.device = nic;
                                                                            newethdevicespec.operation = VirtualDeviceConfigSpecOperation.edit;
                                                                            newethdevicespec.operationSpecified = true;
                                                                            configspecarr[0] = newethdevicespec;
                                                                            VirtualMachineConfigSpec vmconfigspec = new VirtualMachineConfigSpec();
                                                                            vmconfigspec.deviceChange = configspecarr;
                                                                            oLog.AddEvent(strName, strSerial, "Network Adapter Changing to VLAN:" + strVLAN, LoggingType.Information);
                                                                            ManagedObjectReference _task_net = _service.ReconfigVM_Task(_vm_net2, vmconfigspec);
                                                                            TaskInfo _info_net = (TaskInfo)oVMWare.getObjectProperty(_task_net, "info");
                                                                            while (_info_net.state == TaskInfoState.running)
                                                                                _info_net = (TaskInfo)oVMWare.getObjectProperty(_task_net, "info");
                                                                            if (_info_net.state == TaskInfoState.success)
                                                                            {
                                                                                boolCompleted = true;
                                                                                oLog.AddEvent(strName, strSerial, "Network Adapter Reconfigured", LoggingType.Information);
                                                                            }
                                                                            else
                                                                            {
                                                                                strError = "Network Adapter NOT Reconfigured";
                                                                                oLog.AddEvent(strName, strSerial, strError, LoggingType.Error);
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                                catch (Exception ex)
                                                                {
                                                                    // Only hits here if it is not a "DistributedVirtualPortgroup" (meaning it is a standard NETWORK object)
                                                                }
                                                            }
                                                            else
                                                            {
                                                                ManagedObjectReference datacenterRefNetwork = oVMWare.GetDataCenter();
                                                                ManagedObjectReference[] oNetworks = (ManagedObjectReference[])oVMWare.getObjectProperty(datacenterRefNetwork, "network");
                                                                foreach (ManagedObjectReference oNetwork in oNetworks)
                                                                {
                                                                    if (boolCompleted == true)
                                                                        break;
                                                                    try
                                                                    {
                                                                        if (strVLAN == oVMWare.getObjectProperty(oNetwork, "name").ToString())
                                                                        {
                                                                            object oPortConfig = oVMWare.getObjectProperty(oNetwork, "config");
                                                                            if (oPortConfig != null)
                                                                            {
                                                                                DVPortgroupConfigInfo oPort = (DVPortgroupConfigInfo)oPortConfig;
                                                                                if (oPort.key != strPortGroupKey)
                                                                                {
                                                                                    strPortGroupKey = oPort.key;
                                                                                    ManagedObjectReference oSwitch = oPort.distributedVirtualSwitch;
                                                                                    string strSwitchUUID = (string)oVMWare.getObjectProperty(oSwitch, "uuid");
                                                                                    oLog.AddEvent(strName, strSerial, "Trying..." + strPortGroupKey + "(" + strSwitchUUID + ")", LoggingType.Information);

                                                                                    VirtualEthernetCard nic = (VirtualEthernetCard)test[ii];
                                                                                    VirtualDeviceConfigSpec[] configspecarr = new VirtualDeviceConfigSpec[1];
                                                                                    VirtualEthernetCardDistributedVirtualPortBackingInfo vecdvpbi = new VirtualEthernetCardDistributedVirtualPortBackingInfo();
                                                                                    DistributedVirtualSwitchPortConnection connection = new DistributedVirtualSwitchPortConnection();
                                                                                    connection.portgroupKey = strPortGroupKey;
                                                                                    connection.switchUuid = strSwitchUUID;
                                                                                    vecdvpbi.port = connection;
                                                                                    nic.backing = vecdvpbi;
                                                                                    VirtualDeviceConfigSpec newethdevicespec = new VirtualDeviceConfigSpec();
                                                                                    newethdevicespec.device = nic;
                                                                                    newethdevicespec.operation = VirtualDeviceConfigSpecOperation.edit;
                                                                                    newethdevicespec.operationSpecified = true;
                                                                                    configspecarr[0] = newethdevicespec;
                                                                                    VirtualMachineConfigSpec vmconfigspec = new VirtualMachineConfigSpec();
                                                                                    vmconfigspec.deviceChange = configspecarr;
                                                                                    oLog.AddEvent(strName, strSerial, "Network Adapter Changing to VLAN:" + strVLAN, LoggingType.Information);
                                                                                    ManagedObjectReference _task_net = _service.ReconfigVM_Task(_vm_net2, vmconfigspec);
                                                                                    TaskInfo _info_net = (TaskInfo)oVMWare.getObjectProperty(_task_net, "info");
                                                                                    while (_info_net.state == TaskInfoState.running)
                                                                                        _info_net = (TaskInfo)oVMWare.getObjectProperty(_task_net, "info");
                                                                                    if (_info_net.state == TaskInfoState.success)
                                                                                    {
                                                                                        boolCompleted = true;
                                                                                        oLog.AddEvent(strName, strSerial, "Network Adapter Reconfigured", LoggingType.Information);
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        strError = "Network Adapter NOT Reconfigured";
                                                                                        oLog.AddEvent(strName, strSerial, strError, LoggingType.Error);
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                    catch (Exception ex)
                                                                    {
                                                                        // Only hits here if it is not a "DistributedVirtualPortgroup" (meaning it is a standard NETWORK object)
                                                                    }
                                                                }
                                                            }
                                                            if (boolCompleted == false)
                                                                strError = "Network Adapter Was Not Created ~ Could not find a port group (" + strVLAN + ")";
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        else
                                            strError = "No VLAN found for the guest ~ (for " + oIPAddresses.GetVlan(intVlan, "vlan") + " VLAN)";
                                    }

                                    if (strError == "")
                                    {
                                        // Disable the boot retry option
                                        VirtualMachineBootOptions oBootOptions = new VirtualMachineBootOptions();
                                        oBootOptions.bootRetryEnabled = false;
                                        oBootOptions.bootRetryEnabledSpecified = true;
                                        VirtualMachineConfigSpec _cs_boot_options = new VirtualMachineConfigSpec();
                                        _cs_boot_options.bootOptions = oBootOptions;
                                        ManagedObjectReference _task_boot_options = _service.ReconfigVM_Task(_vm_net2, _cs_boot_options);
                                        TaskInfo _info_boot_options = (TaskInfo)oVMWare.getObjectProperty(_task_boot_options, "info");
                                        while (_info_boot_options.state == TaskInfoState.running)
                                            _info_boot_options = (TaskInfo)oVMWare.getObjectProperty(_task_boot_options, "info");
                                        if (_info_boot_options.state == TaskInfoState.success)
                                            oLog.AddEvent(strName, strSerial, "Boot retry turned off", LoggingType.Information);
                                        else
                                        {
                                            strError = "Boot retry NOT turned off";
                                            oLog.AddEvent(strName, strSerial, strError, LoggingType.Error);
                                        }
                                    }

                                    if (strError == "")
                                    {
                                        // Turn on the guest if it is off
                                        oLog.AddEvent(strName, strSerial, "Starting Virtual Machine Power On...", LoggingType.Information);
                                        VirtualMachineRuntimeInfo run_vlan_boot = (VirtualMachineRuntimeInfo)oVMWare.getObjectProperty(_vm_net2, "runtime");
                                        if (run_vlan_boot.powerState == VirtualMachinePowerState.poweredOff)
                                        {
                                            ManagedObjectReference _task_power = _service.PowerOnVM_Task(_vm_net2, null);
                                            TaskInfo _info_power = (TaskInfo)oVMWare.getObjectProperty(_task_power, "info");
                                            while (_info_power.state == TaskInfoState.running)
                                                _info_power = (TaskInfo)oVMWare.getObjectProperty(_task_power, "info");
                                            if (_info_power.state == TaskInfoState.success)
                                            {
                                                oLog.AddEvent(strName, strSerial, "Virtual Machine Powered On", LoggingType.Information);
                                            }
                                            else
                                                oLog.AddEvent(strName, strSerial, "Virtual Machine was NOT Powered On", LoggingType.Warning);
                                        }
                                        else
                                            oLog.AddEvent(strName, strSerial, "Virtual Machine was already Powered On (" + run_vlan_boot.powerState.ToString() + ")", LoggingType.Information);

                                        int intAttempt = 0;
                                        for (intAttempt = 0; intAttempt < 20 && run_vlan_boot.powerState == VirtualMachinePowerState.poweredOff; intAttempt++)
                                        {
                                            run_vlan_boot = (VirtualMachineRuntimeInfo)oVMWare.getObjectProperty(_vm_net2, "runtime");
                                            int intAttemptLeft = (20 - intAttempt);
                                            oLog.AddEvent(strName, strSerial, "Workstation still starting...waiting 5 seconds (" + intAttemptLeft.ToString() + " tries left)", LoggingType.Information);
                                            Thread.Sleep(5000);
                                        }
                                        if (run_vlan_boot.powerState == VirtualMachinePowerState.poweredOff)
                                        {
                                            strError = "There was a problem turning on the guest";
                                            oLog.AddEvent(strName, strSerial, "There was a problem turning on the guest", LoggingType.Error);
                                        }
                                        else
                                            strResult = oOnDemand.GetStep(intStep, "done");
                                    }
                                    AddResult(intWorkstation, intStep, intType, strResult, strError);
                                    break;
                                case 18:    // File Cleanup and Notify
                                    oLog.AddEvent(strName, strSerial, "Starting Step 18 (Reboot)", LoggingType.Information);
                                    // Xen Desktop stuff
                                    //if (GFB == false)
                                    //    strError = XenDesktop(intAnswer, strName, strSerial, strScripts + strSub, oVMWare, strVirtualCenterURL, xenConfig, XenConfigType.Add, "The workstation was successfully registered in Xen Desktop " + xenConfig.MachineCatalogName);
                                    if (strError == "")
                                    {
                                        ManagedObjectReference _vm_reboot = oVMWare.GetVM(strName);
                                        ManagedObjectReference _task_reboot = _service.ResetVM_Task(_vm_reboot);
                                        TaskInfo _info_reboot = (TaskInfo)oVMWare.getObjectProperty(_task_reboot, "info");
                                        while (_info_reboot.state == TaskInfoState.running)
                                            _info_reboot = (TaskInfo)oVMWare.getObjectProperty(_task_reboot, "info");
                                        if (_info_reboot.state == TaskInfoState.success)
                                        {
                                            oLog.AddEvent(strName, strSerial, "Virtual Machine Reset", LoggingType.Information);
                                        }
                                        else
                                            oLog.AddEvent(strName, strSerial, "Virtual Machine was NOT Reset", LoggingType.Warning);

                                        VirtualMachineRuntimeInfo run_reboot = (VirtualMachineRuntimeInfo)oVMWare.getObjectProperty(_vm_reboot, "runtime");
                                        int intAttempt2 = 0;
                                        for (intAttempt2 = 0; intAttempt2 < 20 && run_reboot.powerState == VirtualMachinePowerState.poweredOff; intAttempt2++)
                                        {
                                            run_reboot = (VirtualMachineRuntimeInfo)oVMWare.getObjectProperty(_vm_reboot, "runtime");
                                            int intAttempt2Left = (20 - intAttempt2);
                                            oLog.AddEvent(strName, strSerial, "Workstation still resetting...waiting 5 seconds (" + intAttempt2Left.ToString() + " tries left)", LoggingType.Information);
                                            Thread.Sleep(5000);
                                        }
                                        if (run_reboot.powerState == VirtualMachinePowerState.poweredOff)
                                        {
                                            strError = "There was a problem resetting the guest";
                                            oLog.AddEvent(strName, strSerial, "There was a problem resetting the guest", LoggingType.Error);
                                        }
                                    }

                                    if (strError == "")
                                    {

                                        oLog.AddEvent(strName, strSerial, "Starting Step 18 (File Cleanup and Notify)", LoggingType.Information);
                                        foreach (string strDelete in Directory.GetFiles(strScripts + strSub, intAnswer.ToString() + "_*.vbs"))
                                            File.Delete(strDelete);
                                        foreach (string strDelete in Directory.GetFiles(strScripts + strSub, intAnswer.ToString() + "_*.bat"))
                                            File.Delete(strDelete);

                                        // Send Email
                                        oLog.AddEvent(strName, strSerial, "Attempting to send email", LoggingType.Information);
                                        string strNotifyCC = "";
                                        string strAccounts = "";
                                        DataSet dsAccount = oWorkstation.GetAccountsVMware(intWorkstation);
                                        foreach (DataRow drAccount in dsAccount.Tables[0].Rows)
                                        {
                                            int intAccount = Int32.Parse(drAccount["userid"].ToString());
                                            if (drAccount["admin"].ToString() == "1")
                                                strAccounts += oUser.GetFullName(intAccount) + " [" + oUser.GetName(intAccount) + "] - Administrator<br/>";
                                            if (drAccount["remote"].ToString() == "1")
                                                strAccounts += oUser.GetFullName(intAccount) + " [" + oUser.GetName(intAccount) + "] - Remote Access<br/>";
                                        }

                                        string strEmail = "";
                                        int intAppContact = Int32.Parse(oForecast.GetAnswer(intAnswer, "appcontact"));
                                        if (intAppContact > 0)
                                            strEmail += oUser.GetName(intAppContact) + ";";
                                        int intRequestor = Int32.Parse(oRequest.Get(intRequest, "userid"));
                                        if (intRequestor > 0)
                                            strEmail += oUser.GetName(intRequestor) + ";";

                                        if (strAccounts == "")
                                            strAccounts = "Accounts were not requested at this time.";
                                        string strKnowledge = "";
                                        if (oClass.IsProd(intClass))
                                        {
                                            strNotifyCC = oVariable.NotifyWorkstationProd();
                                            strAccounts += "<p><b>NOTE:</b> To add additional accounts to this workstation, please visit the <a href=\"http://pncpgha37.pncbank.com/tis/TISRequests.nsf/TIS_Request?OpenForm\" target=\"_blank\">Technology Infrastructure Request Portal</a> and submit a <a href=\"http://pncpgha37.pncbank.com/netsvc/nwsnesreq.nsf/lan+requests+(standard)?openform\" target=\"_blank\">LAN Request Form</a>.  Additional information about this process can be found by <a href=\"http://intranet.pnc.com/corpcomm/employee/employeecenter.nsf/ViewByKey/LAN-Access-?OpenDocument&Menu=ECE_Equipment_Access&Sub=News\" target=\"_blank\">clicking here</a>.</p>";
                                            //strKnowledge = "http://knova.ntl-city.com/selfservice/documentLink.do?externalID=KN10319";
                                        }
                                        if (strKnowledge != "")
                                            strKnowledge = "<p>If you are having problems connecting to your virtual workstation, <a href=\"" + strKnowledge + "\" target=\"_blank\"/>please click here</a> to view a helpful knowledge base article published by the support team.</p>";
                                        strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_WORKSTATION");
                                        oFunction.SendEmail("VMware Workstation Notification: " + strName, strEmail, strNotifyCC, strEMailIdsBCC, "VMware Workstation Notification: " + strName, "<p><b>This message is to inform you that the VMware workstation " + strName + " has been auto-provisioned successfully!</b><p><p>This workstation was created in DataCenter <b>" + strDataCenter + "</b> on <b>" + strVirtualCenter + "</b> and is a member of the <b>" + strDomain + "</b> domain.</p><p>As requested, the following users have been granted rights to this workstation:<br/>" + strAccounts + "</p>" + strKnowledge + "<p><b>NOTE:</b> This is automated email sent from the ClearView Auto-Provisioning tool. Please do not respond to this message.</p><p>If you would like to learn more about this process, <a href=\"" + oVariable.URL() + "/info.htm\" target=\"_blank\"/>please click here</a>.</p>", true, false);
                                        AddResult(intWorkstation, intStep, intType, oOnDemand.GetStep(intStep, "done"), strError);
                                        oWorkstation.UpdateVirtualStep(intWorkstation, 999);
                                        oForecast.UpdateAnswerCompleted(intAnswer);
                                        // check if rebuild
                                        DataSet dsRebuild = oWorkstation.GetVirtualRebuild(intWorkstation);
                                        foreach (DataRow drRebuild in dsRebuild.Tables[0].Rows)
                                            if (drRebuild["submitted"].ToString() != ""
                                                && drRebuild["scheduled"].ToString() != ""
                                                && drRebuild["started"].ToString() != ""
                                                && DateTime.Parse(drRebuild["started"].ToString()) < DateTime.Now
                                                && drRebuild["completed"].ToString() == ""
                                                && drRebuild["cancelled"].ToString() == "")
                                            {
                                                // complete the rebuild record
                                                oWorkstation.UpdateVirtualRebuildCompleted(intWorkstation, DateTime.Now.ToString());
                                                break;
                                            }
                                    }
                                    else
                                        AddResult(intWorkstation, intStep, intType, strResult, strError);
                                    break;
                            }
                            // Send Error if encountered
                            DataSet dsError = oWorkstation.GetVirtualError(intWorkstation, intStep);
                            if (dsError.Tables[0].Rows.Count == 0 && (oSpan.Hours > 24 || strError != ""))
                            {
                                //ResourceRequest oResourceRequest = new ResourceRequest(0, dsn);
                                ServiceRequests oServiceRequest = new ServiceRequests(0, dsn);
                                // Generic Error Request
                                int intProvisioningErrorItem = oService.GetItemId(intProvisioningErrorService);
                                int intProvisioningErrorNumber = oResourceRequest.GetNumber(intRequest, intProvisioningErrorItem);

                                if (oSpan.Hours > 24)
                                {
                                    strError = "Sitting at step " + strStep + " for more than 24 hours";
                                    int intError = oWorkstation.AddVirtualError(intRequest, intProvisioningErrorItem, intProvisioningErrorNumber, intWorkstation, intStep, strError);
                                    strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_PROVISIONING_SUPPORT");
                                    if (boolProvisioningErrorEmail == true)
                                        oFunction.SendEmail("Auto-Provisioning INACTIVITY: " + strName, strEMailIdsBCC, strCC, "", "Auto-Provisioning INACTIVITY: " + strName, "<p><b>This message is to inform you that the workstation " + strName + " has been sitting at a step for more than 24 hours!</b><p><p>Serial Number: " + oAsset.Get(intAsset, "serial").ToUpper() + "<br/>Asset Tag: " + oAsset.Get(intAsset, "asset").ToUpper() + "<br/>Model: " + oModelsProperties.Get(intModel, "name").ToUpper() + "<br/>Step #: " + intStep.ToString() + "<br/>Step: " + strStep + "<br/>Error: " + strError + "<br/>DataCenter: " + oVMWare.DataCenter() + "<br/>Virtual Center: " + oVMWare.VirtualCenter() + "</p><p>Once you have resolved this issue, please <a href=\"" + oVariable.URL() + "/build_fixed.aspx?type=w&id=" + oFunction.encryptQueryString(intWorkstation.ToString()) + "\" target=\"_blank\">click here</a> to resume the build.</p><p><b>NOTE:</b> This is automated email sent from the ClearView Auto-Provisioning tool. Please do not respond to this message.</p>", true, false);
                                }
                                else
                                {
                                    int intError = oWorkstation.AddVirtualError(intRequest, intProvisioningErrorItem, intProvisioningErrorNumber, intWorkstation, intStep, strError);
                                    strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_PROVISIONING_SUPPORT");
                                    if (boolProvisioningErrorEmail == true)
                                        oFunction.SendEmail("Auto-Provisioning ERROR: " + strName, strEMailIdsBCC, strCC, "", "Auto-Provisioning ERROR: " + strName, "<p><b>This message is to inform you that the workstation " + strName + " has encountered an error and has been stopped!</b><p><p>Serial Number: " + oAsset.Get(intAsset, "serial").ToUpper() + "<br/>Asset Tag: " + oAsset.Get(intAsset, "asset").ToUpper() + "<br/>Model: " + oModelsProperties.Get(intModel, "name").ToUpper() + "<br/>Step #: " + intStep.ToString() + "<br/>Step: " + strStep + "<br/>Error: " + strError + "<br/>DataCenter: " + oVMWare.DataCenter() + "<br/>Virtual Center: " + oVMWare.VirtualCenter() + "</p><p>Once you have resolved this issue, please <a href=\"" + oVariable.URL() + "/build_fixed.aspx?type=w&id=" + oFunction.encryptQueryString(intWorkstation.ToString()) + "\" target=\"_blank\">click here</a> to resume the build.</p><p><b>NOTE:</b> This is automated email sent from the ClearView Auto-Provisioning tool. Please do not respond to this message.</p>", true, false);
                                }
                                if (boolProvisioningErrorEmail == true)
                                    oLog.AddEvent(strName, strSerial, "Error Message [" + strError + "] Sent to " + strEMailIdsBCC, LoggingType.Warning);

                                int intProvisioningError = oResourceRequest.Add(intRequest, intProvisioningErrorItem, intProvisioningErrorService, intProvisioningErrorNumber, "Provisioning Error (" + strName + ")", 1, 0.00, 2, 1, 1, 1);
                                if (oServiceRequest.NotifyApproval(intProvisioningError, intResourceRequestApprove, intEnvironment, "", dsnServiceEditor) == false)
                                    oServiceRequest.NotifyTeamLead(intProvisioningErrorItem, intProvisioningError, intAssignPage, intViewPage, intEnvironment, "", dsnServiceEditor, dsnAsset, dsnIP, 0);
                                oLog.AddEvent(strName, strSerial, "Provisioning Error Request has been submitted (" + strError + ")", LoggingType.Warning);
                            }
                        }
                    }
                    else if (intLogging > 0)
                            oEventLog.WriteEntry(strName + "...WORKSTATIONID: " + intWorkstation.ToString() + " (VMware WKS): " + "Skipping (boolProcess = false)...." + intStep.ToString(), EventLogEntryType.Information);
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
            }
            catch (Exception ex)
            {
                if (ex.Message.ToUpper().StartsWith("A TRANSPORT-LEVEL ERROR") == false)
                {
                    if (ex.Message.ToUpper().Contains("HTTP STATUS 503") == false)
                    {
                        string strError = "VMWare Workstation Service: " + "(Error Message: " + ex.Message + ") ~  (Source: " + ex.Source + ") (Stack Trace: " + ex.StackTrace + ") [" + System.Environment.UserName + "]";
                        oEventLog.WriteEntry(strError, EventLogEntryType.Error);
                        SystemError(intErrorWorkstation, intErrorStep, strError, intErrorAsset, intErrorModel, oErrorVMWare);
                    }
                }
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
        private void ReadOutput(int _workstationid, string _type, string _file, string _name, string _serial)
        {
            Workstations oWorkstation2 = new Workstations(0, dsn);
            Functions oFunction2 = new Functions(0, dsn, intEnvironment);
            bool boolContent = false;
            for (int ii = 0; ii < 10 && boolContent == false; ii++)
            {
                if (File.Exists(_file) == true)
                {
                    oLog.AddEvent(_name, _serial, "Workstation output file " + _file + " exists...reading...", LoggingType.Information);
                    string strContent = "";
                    try
                    {
                        StreamReader oReader = new StreamReader(_file);
                        strContent = oReader.ReadToEnd();
                        if (strContent != "")
                        {
                            if (intLogging > 0)
                                oLog.AddEvent(_name, _serial, "Updating Database...", LoggingType.Information);
                            boolContent = true;
                            oWorkstation2.AddVirtualOutput(_workstationid, _type, strContent);
                            oReader.Close();
                            if (File.Exists(_file) == true)
                                File.Delete(_file);
                            if (intLogging > 1)
                                oLog.AddEvent(_name, _serial, "Workstation output file " + _file + " finished updating...deleted files...", LoggingType.Information);
                        }
                        else
                        {
                            if (intLogging > 1)
                                oLog.AddEvent(_name, _serial, "Found workstation output file " + _file + "...but it is blank...waiting 5 seconds...", LoggingType.Information);
                            oReader.Close();
                            Thread.Sleep(5000);
                        }
                    }
                    catch
                    {
                        if (intLogging > 1)
                            oLog.AddEvent(_name, _serial, "Cannot open workstation output file " + _file + "...waiting 5 seconds...", LoggingType.Information);
                        Thread.Sleep(5000);
                    }
                }
                else
                {
                    if (intLogging > 1)
                        oLog.AddEvent(_name, _serial, "Workstation output file " + _file + " does not exist...waiting 5 seconds...", LoggingType.Information);
                    Thread.Sleep(5000);
                }
            }
            if (boolContent == false)
            {
                oLog.AddEvent(_name, _serial, "Could Not Find Workstation output file " + _file, LoggingType.Warning);
            }
        }
        private void AddFileLine(string _name, string strFile, string strLine, int _environment)
        {
            try
            {
                Variables oVariableFile = new Variables(_environment);
                oLog.AddEvent(_name, "", "Attempting to append to [" + strFile + "] the line [" + strLine + "] using the account " + oVariableFile.ADUser() + " in " + oVariableFile.Domain(), LoggingType.Information);

                System.Net.NetworkCredential oCredentials = new System.Net.NetworkCredential(oVariableFile.ADUser(), oVariableFile.ADPassword(), oVariableFile.Domain());
                Uri myUrl = new Uri("file:///" + strFile);

                FileWebRequest oFileWebRequestR = (FileWebRequest)WebRequest.CreateDefault(myUrl);
                oFileWebRequestR.Credentials = oCredentials;
                oFileWebRequestR.Timeout = 20000;
                oFileWebRequestR.Method = "GET";
                Stream oStreamR = oFileWebRequestR.GetResponse().GetResponseStream();
                if (intLogging > 1)
                    oLog.AddEvent(_name, "", "...got the stream for reading...", LoggingType.Information);
                StreamReader oINIr = new StreamReader(oStreamR);
                if (intLogging > 1)
                    oLog.AddEvent(_name, "", "...got the streamreader...", LoggingType.Information);

                bool boolAdd = true;
                string strContents = "";
                while (oINIr.Peek() != -1)
                {
                    string strCompare = oINIr.ReadLine();
                    if (strCompare == strLine)
                    {
                        boolAdd = false;
                        break;
                    }
                    else
                        strContents += strCompare + "|";
                }
                oINIr.Close();
                oStreamR.Dispose();
                oStreamR.Close();
                if (intLogging > 1)
                    oLog.AddEvent(_name, "", "...finished reading...", LoggingType.Information);

                if (boolAdd == true)
                {
                    FileWebRequest oFileWebRequestW = (FileWebRequest)WebRequest.CreateDefault(myUrl);
                    oFileWebRequestW.Credentials = oCredentials;
                    oFileWebRequestW.Timeout = 20000;
                    oFileWebRequestW.Method = "POST";
                    Stream oStreamW = oFileWebRequestW.GetRequestStream();
                    if (intLogging > 1)
                        oLog.AddEvent(_name, "", "...got the stream for writing...", LoggingType.Information);
                    StreamWriter oINI = new StreamWriter(oStreamW);
                    if (intLogging > 1)
                        oLog.AddEvent(_name, "", "...got the streamwriter...", LoggingType.Information);
                    char[] strSplit = { '|' };
                    string[] strContent = strContents.Split(strSplit);
                    for (int ii = 0; ii < strContent.Length; ii++)
                    {
                        if (strContent[ii].Trim() != "")
                            oINI.WriteLine(strContent[ii].Trim());
                    }
                    oINI.WriteLine(strLine);
                    oINI.Flush();
                    oINI.Close();
                    oStreamR.Dispose();
                    oStreamR.Close();
                    if (intLogging > 1)
                        oLog.AddEvent(_name, "", "...finished writing...", LoggingType.Information);
                }
                oLog.AddEvent(_name, "", "Finished appending to [" + strFile + "] the line [" + strLine + "]", LoggingType.Information);
            }
            catch (Exception err)
            {
                oLog.AddEvent(_name, "", "An error was encountered when attempting to configure the INI file [" + strFile + "] - Error Message: " + err.Message + " - Source: " + err.Source + " - Stack Trace: " + err.StackTrace, LoggingType.Error);
            }
        }
        private void RemoveFileLine(string _name, string strFile, int _environment)
        {
            try
            {
                Variables oVariableFile = new Variables(_environment);
                oLog.AddEvent(_name, "", "Attempting to remove from [" + strFile + "] any lines containing [" + _name + "] using the account " + oVariableFile.ADUser() + " in " + oVariableFile.Domain(), LoggingType.Information);

                System.Net.NetworkCredential oCredentials = new System.Net.NetworkCredential(oVariableFile.ADUser(), oVariableFile.ADPassword(), oVariableFile.Domain());
                Uri myUrl = new Uri("file:///" + strFile);

                FileWebRequest oFileWebRequestR = (FileWebRequest)WebRequest.CreateDefault(myUrl);
                oFileWebRequestR.Credentials = oCredentials;
                oFileWebRequestR.Timeout = 20000;
                oFileWebRequestR.Method = "GET";
                Stream oStreamR = oFileWebRequestR.GetResponse().GetResponseStream();
                if (intLogging > 1)
                    oLog.AddEvent(_name, "", "...got the stream for reading...", LoggingType.Information);
                StreamReader oINIr = new StreamReader(oStreamR);
                if (intLogging > 1)
                    oLog.AddEvent(_name, "", "...got the streamreader...", LoggingType.Information);

                bool boolChange = false;
                List<string> strContents = new List<string>();
                while (oINIr.Peek() != -1)
                {
                    string strCompare = oINIr.ReadLine();
                    if (strCompare.ToUpper().Contains(_name.ToUpper()) == false)
                        strContents.Add(strCompare);
                    else
                        boolChange = true;
                }
                oINIr.Close();
                oStreamR.Dispose();
                oStreamR.Close();
                if (intLogging > 1)
                    oLog.AddEvent(_name, "", "...finished reading...", LoggingType.Information);

                if (boolChange == true)
                {
                    FileWebRequest oFileWebRequestW = (FileWebRequest)WebRequest.CreateDefault(myUrl);
                    oFileWebRequestW.Credentials = oCredentials;
                    oFileWebRequestW.Timeout = 20000;
                    oFileWebRequestW.Method = "POST";
                    Stream oStreamW = oFileWebRequestW.GetRequestStream();
                    if (intLogging > 1)
                        oLog.AddEvent(_name, "", "...got the stream for writing...", LoggingType.Information);
                    StreamWriter oINI = new StreamWriter(oStreamW);
                    if (intLogging > 1)
                        oLog.AddEvent(_name, "", "...got the streamwriter...", LoggingType.Information);
                    foreach (string strContent in strContents)
                    {
                        if (strContent != "")
                            oINI.WriteLine(strContent.Trim());
                    }
                    oINI.Flush();
                    oINI.Close();
                    oStreamR.Dispose();
                    oStreamR.Close();
                    if (intLogging > 1)
                        oLog.AddEvent(_name, "", "...finished writing...", LoggingType.Information);
                    oLog.AddEvent(_name, "", "Finished removing [" + _name + "] from the file [" + strFile + "]", LoggingType.Information);
                }
                else
                    oLog.AddEvent(_name, "", "There were no instances of [" + _name + "] in the file[" + strFile + "]", LoggingType.Information);
            }
            catch (Exception err)
            {
                oLog.AddEvent(_name, "", "An error was encountered when attempting to modify the INI file [" + strFile + "] - Error Message: " + err.Message + " - Source: " + err.Source + " - Stack Trace: " + err.StackTrace, LoggingType.Error);
            }
        }
        private void ServiceTickDecom()
        {
            int intWorkstation = 0;
            string strName = "";
            try
            {
                Workstations oWorkstation = new Workstations(0, dsn);
                Classes oClass = new Classes(0, dsn);
                Requests oRequest = new Requests(0, dsn);
                Asset oAsset = new Asset(0, dsnAsset, dsn);
                VMWare oVMWare = new VMWare(0, dsn);
                Functions oFunction = new Functions(0, dsn, intEnvironment);
                DataSet ds = oAsset.GetDecommissions(intWorkstationType.ToString(), DateTime.Now);
                Users oUser = new Users(0, dsn);
                Domains oDomain = new Domains(0, dsn);
                Projects oProject = new Projects(0, dsn);
                ServiceRequests oServiceRequest = new ServiceRequests(0, dsn);
                Services oService = new Services(0, dsn);
                ResourceRequest oResourceRequest = new ResourceRequest(0, dsn);
                ServiceDetails oServiceDetail = new ServiceDetails(0, dsn);

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    string strError = "";
                    bool boolDecom = false;
                    int intRequest = Int32.Parse(dr["requestid"].ToString());
                    int intItem = Int32.Parse(dr["itemid"].ToString());
                    int intNumber = Int32.Parse(dr["number"].ToString());
                    int intAsset = Int32.Parse(dr["assetid"].ToString());
                    int intAssetType = Int32.Parse(dr["typeid"].ToString());
                    int intUser = Int32.Parse(dr["userid"].ToString());
                    strName = dr["name"].ToString();
                    string strSerial = oAsset.Get(intAsset, "serial");
                    string strResult = "No information...";
                    int intAddress = 0;
                    DataSet dsAsset = oWorkstation.GetVirtualAsset(intAsset);
                    if (dsAsset.Tables[0].Rows.Count > 0)
                    {
                        intWorkstation = Int32.Parse(dsAsset.Tables[0].Rows[0]["id"].ToString());
                        intAddress = Int32.Parse(dsAsset.Tables[0].Rows[0]["addressid"].ToString());
                    }

                    if (oResourceRequest.GetAllService(intRequest, intDecomErrorService, intNumber).Tables[0].Rows.Count == 0)
                    {
                        // Check Decommission
                        if (dsAsset.Tables[0].Rows.Count > 0)
                        {
                            bool boolProcess = false;
                            int intClass = Int32.Parse(dsAsset.Tables[0].Rows[0]["classid"].ToString());
                            int intEnv = Int32.Parse(dsAsset.Tables[0].Rows[0]["environmentid"].ToString());

                            if (oClass.IsProd(intClass))
                            {
                                if (intBuildProd == 1)
                                {
                                    if (intBuildProdOffHours == 0)
                                    {
                                        boolProcess = true;
                                        if (intLogging > 0)
                                            oEventLog.WriteEntry("WORKSTATIONNAME: " + strName + " (VMware): " + "DECOMMISSION: PROD Classes have been enabled for builds", EventLogEntryType.Information);
                                    }
                                    else if (DateTime.Now.Hour < 7 || DateTime.Now.Hour > 19)
                                    {
                                        boolProcess = true;
                                        if (intLogging > 0)
                                            oEventLog.WriteEntry("WORKSTATIONNAME: " + strName + " (VMware): " + "DECOMMISSION: PROD Classes have been enabled for OFF PEAK HOURS (7AM - 7PM) Current Time : " + DateTime.Now.ToShortTimeString(), EventLogEntryType.Information);
                                    }
                                    else if (intLogging > 0)
                                        oEventLog.WriteEntry("WORKSTATIONNAME: " + strName + " (VMware): " + "DECOMMISSION: PROD Classes are not enabled for DURING PEAK HOURS (7AM - 7PM) builds", EventLogEntryType.Warning);
                                }
                                else if (intLogging > 0)
                                    oEventLog.WriteEntry("WORKSTATIONNAME: " + strName + " (VMware): " + "DECOMMISSION: PROD Classes are not enabled for builds", EventLogEntryType.Warning);
                            }
                            if (oClass.IsQA(intClass))
                            {
                                if (intBuildQA == 1)
                                {
                                    if (intBuildQAOffHours == 0)
                                    {
                                        boolProcess = true;
                                        if (intLogging > 0)
                                            oEventLog.WriteEntry("WORKSTATIONNAME: " + strName + " (VMware): " + "DECOMMISSION: QA Classes have been enabled for builds", EventLogEntryType.Information);
                                    }
                                    else if (DateTime.Now.Hour < 7 || DateTime.Now.Hour > 19)
                                    {
                                        boolProcess = true;
                                        if (intLogging > 0)
                                            oEventLog.WriteEntry("WORKSTATIONNAME: " + strName + " (VMware): " + "DECOMMISSION: QA Classes have been enabled for OFF PEAK HOURS (7AM - 7PM) Current Time : " + DateTime.Now.ToShortTimeString(), EventLogEntryType.Information);
                                    }
                                    else if (intLogging > 0)
                                        oEventLog.WriteEntry("WORKSTATIONNAME: " + strName + " (VMware): " + "DECOMMISSION: QA Classes are not enabled for DURING PEAK HOURS (7AM - 7PM) builds", EventLogEntryType.Warning);
                                }
                                else if (intLogging > 0)
                                    oEventLog.WriteEntry("WORKSTATIONNAME: " + strName + " (VMware): " + "DECOMMISSION: QA Classes are not enabled for builds", EventLogEntryType.Warning);
                            }
                            if (oClass.IsTestDev(intClass))
                            {
                                if (intBuildTest == 1)
                                {
                                    boolProcess = true;
                                    if (intLogging > 0)
                                        oEventLog.WriteEntry("WORKSTATIONNAME: " + strName + " (VMware): " + "DECOMMISSION: TEST Classes have been enabled for builds", EventLogEntryType.Information);
                                }
                                else if (intLogging > 0)
                                    oEventLog.WriteEntry("WORKSTATIONNAME: " + strName + " (VMware): " + "DECOMMISSION: TEST Classes are not enabled for builds", EventLogEntryType.Warning);
                            }


                            if (boolProcess == true)
                            {
                                // Start Decommission
                                oAsset.UpdateDecommissionRunning(intAsset, 1);
                                oLog.AddEvent(strName, strSerial, "DECOMMISSION: Starting Decommission", LoggingType.Information);
                                oWorkstation.UpdateVirtualStep(intWorkstation, 999);

                                //oLog.AddEvent(strName, strSerial, "DECOMMISSION: Using addressID = " + intAddress.ToString() + " to remove from Xen Desktop", LoggingType.Information);
                                //strError = XenDesktop(0, strName, strSerial, strScripts + strSub, null, "", intAddress, XenConfigType.MaintOn, "Maintenance mode enabled in Xen Desktop");

                                if (strError == "")
                                {
                                    bool boolFound = false;
                                    string strAlready = "";
                                    DataSet dsVirtualCenter = oVMWare.GetVirtualCentersCE(intClass, intEnv, intAddress);
                                    foreach (DataRow drVirtualCenter in dsVirtualCenter.Tables[0].Rows)
                                    {
                                        if (boolFound == true)
                                            break;
                                        int intVirtualCenter = Int32.Parse(drVirtualCenter["id"].ToString());
                                        DataSet dsDataCenter = oVMWare.GetDatacentersCE(intVirtualCenter, intClass, intEnv, intAddress);
                                        foreach (DataRow drDataCenter in dsDataCenter.Tables[0].Rows)
                                        {
                                            if (boolFound == true)
                                                break;
                                            if (strAlready != "")
                                                strAlready += strSplit[0].ToString();
                                            strAlready += drDataCenter["id"].ToString();
                                            string strDecomVLAN = drDataCenter["workstation_decom_vlan"].ToString();
                                            string strConnect = oVMWare.ConnectDEBUG(drVirtualCenter["url"].ToString(), Int32.Parse(drVirtualCenter["environment"].ToString()), drDataCenter["name"].ToString());
                                            if (strConnect == "")
                                            {
                                                VimService _service = oVMWare.GetService();
                                                oLog.AddEvent(strName, strSerial, "DECOMMISSION: Trying " + drDataCenter["name"].ToString() + " on " + drVirtualCenter["name"].ToString(), LoggingType.Information);
                                                try
                                                {
                                                    ManagedObjectReference _vm_power = oVMWare.GetVM(strName);
                                                    if (_vm_power != null && _vm_power.Value != "")
                                                    {
                                                        oLog.AddEvent(strName, strSerial, "DECOMMISSION: Found VMware Object [" + _vm_power.Value + "] in " + drDataCenter["name"].ToString() + " on " + drVirtualCenter["name"].ToString(), LoggingType.Information);
                                                        boolFound = true;
                                                        bool boolShutdown = false;
                                                        VirtualMachineRuntimeInfo oRuntime = (VirtualMachineRuntimeInfo)oVMWare.getObjectProperty(_vm_power, "runtime");
                                                        if (oRuntime.powerState != VirtualMachinePowerState.poweredOff)
                                                        {
                                                            ManagedObjectReference _task_power = _service.PowerOffVM_Task(_vm_power);
                                                            TaskInfo _info_power = (TaskInfo)oVMWare.getObjectProperty(_task_power, "info");
                                                            while (_info_power.state == TaskInfoState.running)
                                                                _info_power = (TaskInfo)oVMWare.getObjectProperty(_task_power, "info");
                                                            if (_info_power.state == TaskInfoState.success)
                                                            {
                                                                boolShutdown = true;
                                                                oLog.AddEvent(strName, strSerial, "DECOMMISSION: Finished Powering Off", LoggingType.Information);
                                                                strResult = "Finished Powering Off " + strName + " [" + DateTime.Now.ToString() + "]";
                                                            }
                                                            else
                                                            {
                                                                oLog.AddEvent(strName, strSerial, "DECOMMISSION: Error Powering Off", LoggingType.Error);
                                                                oAsset.UpdateDecommissionRunning(intAsset, -1);
                                                                strError = "Virtual Machine Was Not Powered Off ~ " + strName;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            boolShutdown = true;
                                                            oLog.AddEvent(strName, strSerial, "DECOMMISSION: Machine Not Running (" + oRuntime.powerState.ToString() + ")", LoggingType.Information);
                                                            strResult = "Virtual Machine " + strName + " Was Not Running (no power off required)";
                                                        }

                                                        if (boolShutdown == true)
                                                        {
                                                            //ManagedObjectReference _vm_rename = oVMWare.GetVM(strName);
                                                            ManagedObjectReference _task_rename = _service.Rename_Task(_vm_power, strName.ToLower() + strDecomSuffix);
                                                            TaskInfo _info_rename = (TaskInfo)oVMWare.getObjectProperty(_task_rename, "info");
                                                            while (_info_rename.state == TaskInfoState.running)
                                                                _info_rename = (TaskInfo)oVMWare.getObjectProperty(_task_rename, "info");
                                                            if (_info_rename.state == TaskInfoState.success)
                                                            {
                                                                boolDecom = true;
                                                                oLog.AddEvent(strName, strSerial, "DECOMMISSION: Finished Rename", LoggingType.Information);
                                                                DecomVLAN(strName, oVMWare, _vm_power, strDecomVLAN, _service);
                                                                oLog.AddEvent(strName, strSerial, "DECOMMISSION: Finished Changing VLAN", LoggingType.Information);
                                                                strResult += "<br/>Finished Decommissioning " + strName + " [" + DateTime.Now.ToString() + "]";
                                                            }
                                                            else
                                                            {
                                                                strError = "<br/>Virtual Machine Was Not Renamed ~ " + strName;
                                                                oAsset.UpdateDecommissionRunning(intAsset, -1);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            strError = "<br/>Virtual Machine Was Not Powered Off ~ " + strName;
                                                            oAsset.UpdateDecommissionRunning(intAsset, -1);
                                                        }
                                                        oRequest.AddResult(intRequest, intItem, intNumber, strResult);
                                                        oLog.AddEvent(strName, strSerial, "DECOMMISSION: Completed Decommission", LoggingType.Information);
                                                    }
                                                }
                                                catch (Exception exVmware)
                                                {
                                                    oLog.AddEvent(strName, strSerial, "DECOMMISSION: There was a problem searching in " + drDataCenter["name"].ToString() + " on " + drVirtualCenter["name"].ToString() + " ~ " + exVmware.Message, LoggingType.Warning);
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
                                        }
                                    }
                                    if (boolFound == false)
                                    {
                                        //strError = "There was a problem powering off the device (Could Not Find Computer " + strName + ")";
                                        boolDecom = true;
                                        oLog.AddEvent(strName, strSerial, "DECOMMISSION: Could Not Find Computer " + strName, LoggingType.Information);
                                        //strError = XenDesktop(0, strName, strSerial, strScripts + strSub, null, "", intAddress, XenConfigType.MaintOn, "Maintenance mode enabled in Xen Desktop");
                                        strResult += "<br/>Finished Decommissioning " + strName + " [" + DateTime.Now.ToString() + "]";
                                    }
                                }
                            }
                        }
                        else
                            strError = "There was a problem powering off the device (No asset information found for vmware guest " + strName + ")";
                    }
                    else
                    {
                        oLog.AddEvent(strName, strSerial, "Decommission: Manual Intervention Completed", LoggingType.Information);
                        boolDecom = true;
                        //strError = XenDesktop(0, strName, strSerial, strScripts + strSub, null, "", intAddress, XenConfigType.MaintOn, "Maintenance mode enabled in Xen Desktop");
                    }

                    if (strError != "")
                    {
                        oAsset.UpdateDecommissionRunning(intAsset, -1);
                        oRequest.AddResult(intRequest, intItem, intNumber, strError);
                        oLog.AddEvent(strName, strSerial, "DECOMMISSION: " + strError, LoggingType.Error);
                        if (boolDecommissionErrorEmail == true)
                        {
                            // Send Email with Error
                            strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DEVELOPER_ERROR");
                            oFunction.SendEmail("Auto-Provisioning DECOMMISSION Error", strEMailIdsBCC, "", "", "Auto-Provisioning DECOMMISSION Error", "<p><b>This message is to inform you that there was a problem with the DECOMMISSION process for workstation " + strName + ".</b><p><p>Error Message: " + strError + "</p>", true, false);
                        }
                        // Send Auto-Decom support task
                        int intDecomErrorItem = oService.GetItemId(intDecomErrorService);
                        //int intDecomErrorNumber = oResourceRequest.GetNumber(intRequest, intDecomErrorItem);
                        int intDecomErrorNumber = intNumber;
                        double dblServiceHours = oServiceDetail.GetHours(intDecomErrorService, 1);
                        int intDecomError = oServiceRequest.AddRequest(intRequest, intDecomErrorItem, intDecomErrorService, 1, dblServiceHours, 2, intDecomErrorNumber, dsnServiceEditor);
                        oServiceRequest.NotifyTeamLead(intDecomErrorItem, intDecomError, intAssignPage, intViewPage, intEnvironment, "", dsnServiceEditor, dsnAsset, dsnIP, 0);
                    }
                    else if (boolDecom == true)
                    {
                        strResult = "The workstation was successfully decommissioned";
                        oAsset.UpdateDecommission(intAsset, DateTime.Now.AddDays(7), 1, strName);
                        oAsset.AddStatus(intAsset, strName, (int)AssetStatus.Decommissioned, 0, DateTime.Now);
                        oWorkstation.UpdateVirtualDecommissioned(intWorkstation, DateTime.Now.ToString());
                        oAsset.UpdateDecommissionRunning(intAsset, 0);
                        oRequest.AddResult(intRequest, intItem, intNumber, "Finished Decommissioning " + strName + " [" + DateTime.Now.ToLongDateString() + "]");
                        oLog.AddEvent(strName, strSerial, "DECOMMISSION: Finished Decommissioning", LoggingType.Information);

                        //Email Notification on Successful Decomm
                        string strEmail = "";
                        string strCC = "";
                        //string strCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DECOM_VMWARE");
                        //string strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_INVENTORY_MANAGER");
                        string strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DECOM_VMWARE,EMAILGRP_INVENTORY_MANAGER");
                        if (boolNotifyDecom == true)
                        {
                            int intRequestUser = oRequest.GetUser(intRequest);
                            if (intRequestUser > 0)
                                strEmail = oUser.GetName(intRequestUser);
                        }
                        oFunction.SendEmail("DECOMMISSION Notification ", strEmail, strCC, strEMailIdsBCC,
                                            "DECOMMISSION Notification: " + strName,
                                            "<p>This message is to notify you that the device " + strName + " was successfully decommissioned</p>", true, false);
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.ToUpper().StartsWith("A TRANSPORT-LEVEL ERROR") == false)
                {
                    if (ex.Message.ToUpper().Contains("HTTP STATUS 503") == false)
                    {
                        string strError = "VMWare Service (DECOMMISSION): " + strName + " (WORKSTATIONID = " + intWorkstation.ToString() + ") (Error Message: " + ex.Message + ") ~ (Source: " + ex.Source + ") (Stack Trace: " + ex.StackTrace + ") [" + System.Environment.UserName + "]";
                        oEventLog.WriteEntry(strError, EventLogEntryType.Error);
                        SystemError(strError);
                    }
                }
            }
            ServiceTickDecomDestroy();
        }
        private void DecomVLAN(string _name, VMWare _vmware, ManagedObjectReference _vm, string _vlan, VimService _service)
        {
            // Set to DECOM VLAN
            oLog.AddEvent(_name, "", "DECOMMISSION: Setting to Decom VLAN (" + _vlan + ") using VMware object " + _vm.Value, LoggingType.Information);
            if (_vlan != "" && _vm != null)
            {
                VirtualMachineConfigInfo vminfo = (VirtualMachineConfigInfo)_vmware.getObjectProperty(_vm, "config");
                VirtualDevice[] test = vminfo.hardware.device;
                for (int ii = 0; ii < test.Length; ii++)
                {
                    if (test[ii].deviceInfo.label.ToUpper() == "NETWORK ADAPTER 1")
                    {
                        bool boolDecomCompleted = false;
                        string strPortGroupKey = "";
                        ManagedObjectReference datacenterRefNetwork = _vmware.GetDataCenter();
                        ManagedObjectReference[] oNetworks = (ManagedObjectReference[])_vmware.getObjectProperty(datacenterRefNetwork, "network");
                        foreach (ManagedObjectReference oNetwork in oNetworks)
                        {
                            if (boolDecomCompleted == true)
                                break;
                            try
                            {
                                if (_vlan == _vmware.getObjectProperty(oNetwork, "name").ToString())
                                {
                                    object oPortConfig = _vmware.getObjectProperty(oNetwork, "config");
                                    if (oPortConfig != null)
                                    {
                                        DVPortgroupConfigInfo oPort = (DVPortgroupConfigInfo)oPortConfig;
                                        if (oPort.key != strPortGroupKey)
                                        {
                                            strPortGroupKey = oPort.key;
                                            ManagedObjectReference oSwitch = oPort.distributedVirtualSwitch;
                                            string strSwitchUUID = (string)_vmware.getObjectProperty(oSwitch, "uuid");
                                            oLog.AddEvent(_name, "", "Trying..." + strPortGroupKey + "(" + strSwitchUUID + ")", LoggingType.Information);

                                            VirtualEthernetCard nic = (VirtualEthernetCard)test[ii];
                                            VirtualDeviceConfigSpec[] configspecarr = new VirtualDeviceConfigSpec[1];
                                            VirtualEthernetCardDistributedVirtualPortBackingInfo vecdvpbi = new VirtualEthernetCardDistributedVirtualPortBackingInfo();
                                            DistributedVirtualSwitchPortConnection connection = new DistributedVirtualSwitchPortConnection();
                                            connection.portgroupKey = strPortGroupKey;
                                            connection.switchUuid = strSwitchUUID;
                                            vecdvpbi.port = connection;
                                            nic.backing = vecdvpbi;
                                            VirtualDeviceConfigSpec newethdevicespec = new VirtualDeviceConfigSpec();
                                            newethdevicespec.device = nic;
                                            newethdevicespec.operation = VirtualDeviceConfigSpecOperation.edit;
                                            newethdevicespec.operationSpecified = true;
                                            configspecarr[0] = newethdevicespec;
                                            VirtualMachineConfigSpec vmconfigspec = new VirtualMachineConfigSpec();
                                            vmconfigspec.deviceChange = configspecarr;
                                            oLog.AddEvent(_name, "", "Network Adapter Changing to VLAN:" + _vlan, LoggingType.Information);
                                            ManagedObjectReference _task_net = _service.ReconfigVM_Task(_vm, vmconfigspec);
                                            TaskInfo _info_net = (TaskInfo)_vmware.getObjectProperty(_task_net, "info");
                                            while (_info_net.state == TaskInfoState.running)
                                                _info_net = (TaskInfo)_vmware.getObjectProperty(_task_net, "info");
                                            if (_info_net.state == TaskInfoState.success)
                                            {
                                                boolDecomCompleted = true;
                                                oLog.AddEvent(_name, "", "Network Adapter Reconfigured", LoggingType.Information);
                                            }
                                            else
                                                oLog.AddEvent(_name, "", "Network Adapter NOT Reconfigured", LoggingType.Error);
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                // Only hits here if it is not a "DistributedVirtualPortgroup" (meaning it is a standard NETWORK object)
                            }
                        }
                        if (boolDecomCompleted == false)
                            oLog.AddEvent(_name, "", "Network Adapter Was Not Reconfigured ~ Could not find a port group (" + _vlan + ")", LoggingType.Error);
                    }
                }
            }
        }
        private XenConfig GetXenConfig(int _addressid)
        {
            XenConfig xenConfig = XenConfigs.Find(o => o.AddressID == _addressid);
            //foreach (XenConfig config in XenConfigs)
            //{
            //    if (config.AddressID == intAddress)
            //    {
            //        xenConfig = config;
            //        break;
            //    }
            //}
            return xenConfig;
        }
        private string XenDesktop(int _answerid, string _name, string _serial, string _scripts, VMWare oVMWare, string _virtual_center_url, int _addressid, XenConfigType type, string _results)
        {
            return XenDesktop(_answerid, _name, _serial, _scripts, oVMWare, _virtual_center_url, GetXenConfig(_addressid), type, _results);
        }
        private string XenDesktop(int _answerid, string _name, string _serial, string _scripts, VMWare oVMWare, string _virtual_center_url, XenConfig config, XenConfigType type, string _results)
        {
            string error = "";
            try
            {
                List<PowershellParameter> powershell = new List<PowershellParameter>();
                Powershell oPowershell = new Powershell();
                switch (type)
                {
                    case XenConfigType.Add:
                        oLog.AddEvent(_answerid, _name, _serial, "Registering in Xen Desktop...(" + _scripts + "\\XenWorkstationCommands.ps1)", LoggingType.Debug);
                        ManagedObjectReference _vm_xen = oVMWare.GetVM(_name);
                        VirtualMachineConfigInfo _vminfo_xen = (VirtualMachineConfigInfo)oVMWare.getObjectProperty(_vm_xen, "config");
                        powershell.Add(new PowershellParameter("Add", true));
                        powershell.Add(new PowershellParameter("WorkstationName", _name));
                        powershell.Add(new PowershellParameter("DomainName", "PNCNT"));
                        powershell.Add(new PowershellParameter("WorkstationUUID", _vminfo_xen.uuid));
                        powershell.Add(new PowershellParameter("VirtualCenterAddress", _virtual_center_url));
                        //powershell.Add(new PowershellParameter("GroupName", config.ADGroup));
                        powershell.Add(new PowershellParameter("GroupName", "GSLwksRD_LAR_" + _name.ToUpper()));
                        powershell.Add(new PowershellParameter("MachineCatalogueName", config.MachineCatalogName));
                        powershell.Add(new PowershellParameter("DesktopGroupName", config.DesktopDeliveryGroup));
                        powershell.Add(new PowershellParameter("ControllerNames", config.Controllers));
                        break;
                    case XenConfigType.MaintOn:
                        oLog.AddEvent(_answerid, _name, _serial, "Enabling maintenance mode in Xen Desktop...(" + _scripts + "\\XenWorkstationCommands.ps1)", LoggingType.Debug);
                        powershell.Add(new PowershellParameter("TurnOnMaintenanceMode", null));
                        powershell.Add(new PowershellParameter("WorkstationName", _name));
                        powershell.Add(new PowershellParameter("DomainName", "PNCNT"));
                        powershell.Add(new PowershellParameter("ControllerNames", config.Controllers));
                        break;
                    case XenConfigType.Remove:
                        oLog.AddEvent(_answerid, _name, _serial, "Removing machine from Xen Desktop...(" + _scripts + "\\XenWorkstationCommands.ps1)", LoggingType.Debug);
                        powershell.Add(new PowershellParameter("Remove", null));
                        powershell.Add(new PowershellParameter("WorkstationName", _name));
                        powershell.Add(new PowershellParameter("DomainName", "PNCNT"));
                        powershell.Add(new PowershellParameter("MachineCatalogueName", config.MachineCatalogName));
                        powershell.Add(new PowershellParameter("DesktopGroupName", config.DesktopDeliveryGroup));
                        powershell.Add(new PowershellParameter("ControllerNames", config.Controllers));
                        break;
                    case XenConfigType.MaintOff:
                        oLog.AddEvent(_answerid, _name, _serial, "Disabling maintenance mode in Xen Desktop...(" + _scripts + "\\XenWorkstationCommands.ps1)", LoggingType.Debug);
                        powershell.Add(new PowershellParameter("TurnOffMaintenanceMode", null));
                        powershell.Add(new PowershellParameter("WorkstationName", _name));
                        powershell.Add(new PowershellParameter("DomainName", "PNCNT"));
                        powershell.Add(new PowershellParameter("ControllerNames", config.Controllers));
                        break;
                }
                oLog.AddEvent(_answerid, _name, _serial, "Executing powershell scripts...", LoggingType.Debug);
                List<PowershellParameter> results = oPowershell.Execute(_scripts + "\\XenWorkstationCommands.ps1", powershell, oLog, _name);
                oLog.AddEvent(_answerid, _name, _serial, "Powershell script completed!", LoggingType.Debug);
                bool PowerShellError = false;
                foreach (PowershellParameter result in results)
                {
                    oLog.AddEvent(_name, _serial, "PSOBJECT: " + result.Name + " = " + result.Value, LoggingType.Information);
                    if (result.Name == "ResultCode" && result.Value.ToString() != "0")
                        PowerShellError = true;
                    else if (result.Name == "Message" && PowerShellError)
                        error = result.Value.ToString();
                }
            }
            catch (Exception exPowershell)
            {
                error = exPowershell.Message;
                Exception exPowershellInner = exPowershell.InnerException;
                while (exPowershellInner != null)
                {
                    error += " ~ " + exPowershellInner.Message;
                    exPowershellInner = exPowershellInner.InnerException;
                }
                error = "PowerShell Script Error = " + error + " (Source: " + exPowershell.Source + ") (Stack Trace: " + exPowershell.StackTrace + ")";
            }
            if (error == "")
                oLog.AddEvent(_answerid, _name, _serial, _results, LoggingType.Debug);
            return error;
        }
        private void ServiceTickDecomDestroy()
        {
            int intWorkstation = 0;
            string strName = "";
            try
            {
                Workstations oWorkstation = new Workstations(0, dsn);
                Classes oClass = new Classes(0, dsn);
                Requests oRequest = new Requests(0, dsn);
                Asset oAsset = new Asset(0, dsnAsset, dsn);
                VMWare oVMWare = new VMWare(0, dsn);
                Customized oCustomized = new Customized(0, dsn);
                ServerDecommission oServerDecommission = new ServerDecommission(0, dsn);
                Functions oFunction = new Functions(0, dsn, intEnvironment);
                ServiceRequests oServiceRequest = new ServiceRequests(0, dsn);
                Services oService = new Services(0, dsn);
                ResourceRequest oResourceRequest = new ResourceRequest(0, dsn);
                ServiceDetails oServiceDetail = new ServiceDetails(0, dsn);

                DataSet ds = oAsset.GetDecommissionDestroys(intWorkstationType.ToString(), DateTime.Now);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    string strError = "";
                    bool boolDestroy = false;
                    int intRequest = Int32.Parse(dr["requestid"].ToString());
                    int intItem = Int32.Parse(dr["itemid"].ToString());
                    int intNumber = Int32.Parse(dr["number"].ToString());
                    int intAsset = Int32.Parse(dr["assetid"].ToString());
                    int intModel = Int32.Parse(oAsset.Get(intAsset, "modelid"));
                    bool boolDR = (dr["dr"].ToString() == "1");
                    int intAnswer = 0;
                    int intName = 0;
                    int intAddress = 0;
                    DataSet dsAsset = oWorkstation.GetVirtualAsset(intAsset);
                    if (dsAsset.Tables[0].Rows.Count > 0)
                    {
                        intWorkstation = Int32.Parse(dsAsset.Tables[0].Rows[0]["id"].ToString());
                        intAnswer = Int32.Parse(dsAsset.Tables[0].Rows[0]["answerid"].ToString());
                        intName = Int32.Parse(dsAsset.Tables[0].Rows[0]["nameid"].ToString());
                        intAddress = Int32.Parse(dsAsset.Tables[0].Rows[0]["addressid"].ToString());
                    }
                    int intAssetType = Int32.Parse(dr["typeid"].ToString());
                    int intUser = Int32.Parse(dr["userid"].ToString());
                    strName = dr["name"].ToString();
                    string strSerial = oAsset.Get(intAsset, "serial");
                    string strResult = "No information...";

                    if (oResourceRequest.GetAllService(intRequest, intDestroyErrorService, intNumber).Tables[0].Rows.Count == 0)
                    {
                        // Check Decommission
                        if (dsAsset.Tables[0].Rows.Count > 0)
                        {
                            bool boolProcess = false;
                            int intClass = Int32.Parse(dsAsset.Tables[0].Rows[0]["classid"].ToString());
                            int intEnv = Int32.Parse(dsAsset.Tables[0].Rows[0]["environmentid"].ToString());

                            if (oClass.IsProd(intClass))
                            {
                                if (intBuildProd == 1)
                                {
                                    if (intBuildProdOffHours == 0)
                                    {
                                        boolProcess = true;
                                        if (intLogging > 0)
                                            oEventLog.WriteEntry("WORKSTATIONNAME: " + strName + " (VMware): " + "DESTROY: PROD Classes have been enabled for builds", EventLogEntryType.Information);
                                    }
                                    else if (DateTime.Now.Hour < 7 || DateTime.Now.Hour > 19)
                                    {
                                        boolProcess = true;
                                        if (intLogging > 0)
                                            oEventLog.WriteEntry("WORKSTATIONNAME: " + strName + " (VMware): " + "DESTROY: PROD Classes have been enabled for OFF PEAK HOURS (7AM - 7PM) Current Time : " + DateTime.Now.ToShortTimeString(), EventLogEntryType.Information);
                                    }
                                    else if (intLogging > 0)
                                        oEventLog.WriteEntry("WORKSTATIONNAME: " + strName + " (VMware): " + "DESTROY: PROD Classes are not enabled for DURING PEAK HOURS (7AM - 7PM) builds", EventLogEntryType.Warning);
                                }
                                else if (intLogging > 0)
                                    oEventLog.WriteEntry("WORKSTATIONNAME: " + strName + " (VMware): " + "DESTROY: PROD Classes are not enabled for builds", EventLogEntryType.Warning);
                            }
                            if (oClass.IsQA(intClass))
                            {
                                if (intBuildQA == 1)
                                {
                                    if (intBuildQAOffHours == 0)
                                    {
                                        boolProcess = true;
                                        if (intLogging > 0)
                                            oEventLog.WriteEntry("WORKSTATIONNAME: " + strName + " (VMware): " + "DESTROY: QA Classes have been enabled for builds", EventLogEntryType.Information);
                                    }
                                    else if (DateTime.Now.Hour < 7 || DateTime.Now.Hour > 19)
                                    {
                                        boolProcess = true;
                                        if (intLogging > 0)
                                            oEventLog.WriteEntry("WORKSTATIONNAME: " + strName + " (VMware): " + "DESTROY: QA Classes have been enabled for OFF PEAK HOURS (7AM - 7PM) Current Time : " + DateTime.Now.ToShortTimeString(), EventLogEntryType.Information);
                                    }
                                    else if (intLogging > 0)
                                        oEventLog.WriteEntry("WORKSTATIONNAME: " + strName + " (VMware): " + "DESTROY: QA Classes are not enabled for DURING PEAK HOURS (7AM - 7PM) builds", EventLogEntryType.Warning);
                                }
                                else if (intLogging > 0)
                                    oEventLog.WriteEntry("WORKSTATIONNAME: " + strName + " (VMware): " + "DESTROY: QA Classes are not enabled for builds", EventLogEntryType.Warning);
                            }
                            if (oClass.IsTestDev(intClass))
                            {
                                if (intBuildTest == 1)
                                {
                                    boolProcess = true;
                                    if (intLogging > 0)
                                        oEventLog.WriteEntry("WORKSTATIONNAME: " + strName + " (VMware): " + "DESTROY: TEST Classes have been enabled for builds", EventLogEntryType.Information);
                                }
                                else if (intLogging > 0)
                                    oEventLog.WriteEntry("WORKSTATIONNAME: " + strName + " (VMware): " + "DESTROY: TEST Classes are not enabled for builds", EventLogEntryType.Warning);
                            }


                            if (boolProcess == true)
                            {
                                // Start Destroy
                                oAsset.UpdateDecommissionRunning(intAsset, 1);
                                oLog.AddEvent(strName, strSerial, "DESTROY: Starting Destroy", LoggingType.Information);

                                // Ping name to see if it is still off
                                if (oFunction.PingName(strName) == "")
                                {
                                    bool boolFound = false;
                                    string strAlready = "";
                                    DataSet dsVirtualCenter = oVMWare.GetVirtualCentersCE(intClass, intEnv, intAddress);
                                    foreach (DataRow drVirtualCenter in dsVirtualCenter.Tables[0].Rows)
                                    {
                                        if (boolFound == true)
                                            break;
                                        int intVirtualCenter = Int32.Parse(drVirtualCenter["id"].ToString());
                                        DataSet dsDataCenter = oVMWare.GetDatacentersCE(intVirtualCenter, intClass, intEnv, intAddress);
                                        foreach (DataRow drDataCenter in dsDataCenter.Tables[0].Rows)
                                        {
                                            if (boolFound == true)
                                                break;
                                            if (strAlready != "")
                                                strAlready += strSplit[0].ToString();
                                            strAlready += drDataCenter["id"].ToString();
                                            string strConnect = oVMWare.ConnectDEBUG(drVirtualCenter["url"].ToString(), Int32.Parse(drVirtualCenter["environment"].ToString()), drDataCenter["name"].ToString());
                                            if (strConnect == "")
                                            {
                                                VimService _service = oVMWare.GetService();
                                                oLog.AddEvent(strName, strSerial, "DESTROY: Trying " + drDataCenter["name"].ToString() + " on " + drVirtualCenter["name"].ToString(), LoggingType.Information);
                                                try
                                                {
                                                    // First, search for the machine name with the suffix (how it should have been powered off)
                                                    ManagedObjectReference _vm_power = oVMWare.GetVM(strName.ToLower() + strDecomSuffix);
                                                    if (_vm_power != null && _vm_power.Value != "")
                                                    {
                                                        oLog.AddEvent(strName, strSerial, "DESTROY: Found name " + strName + strDecomSuffix + ", VMware Object [" + _vm_power.Value + "] in " + drDataCenter["name"].ToString() + " on " + drVirtualCenter["name"].ToString(), LoggingType.Information);
                                                        boolFound = true;
                                                        // Check to see if it is running.
                                                        VirtualMachineRuntimeInfo oRuntime = (VirtualMachineRuntimeInfo)oVMWare.getObjectProperty(_vm_power, "runtime");
                                                        if (oRuntime.powerState != VirtualMachinePowerState.poweredOff)
                                                        {
                                                            oLog.AddEvent(strName, strSerial, "DESTROY: Powered Back On - Recommission", LoggingType.Information);
                                                            // Recommission the machine...
                                                            oWorkstation.UpdateVirtualRecommissioned(strName);
                                                            //strError = XenDesktop(0, strName, strSerial, strScripts + strSub, null, "", intAddress, XenConfigType.MaintOff, "Maintenance mode disabled in Xen Desktop");
                                                            strResult = "Virtual Machine " + strName + " Was Powered Back On - Workstation Recommissioned";
                                                        }
                                                        else
                                                        {
                                                            ManagedObjectReference _task_power = _service.Destroy_Task(_vm_power);
                                                            TaskInfo _info_power = (TaskInfo)oVMWare.getObjectProperty(_task_power, "info");
                                                            while (_info_power.state == TaskInfoState.running)
                                                                _info_power = (TaskInfo)oVMWare.getObjectProperty(_task_power, "info");
                                                            if (_info_power.state == TaskInfoState.success)
                                                            {
                                                                boolDestroy = true;
                                                                strResult = "Finished Destroying " + strName + " [" + DateTime.Now.ToString() + "]";
                                                                //strError = XenDesktop(0, strName, strSerial, strScripts + strSub, null, "", intAddress, XenConfigType.Remove, "Machine removed from Xen Desktop");
                                                                oLog.AddEvent(strName, strSerial, "DESTROY: Success...sending Initiating Decom Request", LoggingType.Information);
                                                            }
                                                            else
                                                            {
                                                                strResult = "Virtual Machine " + strName + " Was Not Destroyed";
                                                                oAsset.UpdateDecommissionRunning(intAsset, -1);
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        // Machine does not exist - now check for original name
                                                        oLog.AddEvent(strName, strSerial, "DESTROY: Name " + strName + strDecomSuffix + " was not found...Trying name " + strName, LoggingType.Information);
                                                        _vm_power = oVMWare.GetVM(strName.ToLower());
                                                        if (_vm_power != null && _vm_power.Value != "")
                                                        {
                                                            // Machine found - either it was recommissioned or not properly decommissioned.
                                                            oLog.AddEvent(strName, strSerial, "DESTROY: Found name " + strName + ", VMware Object [" + _vm_power.Value + "] in " + drDataCenter["name"].ToString() + " on " + drVirtualCenter["name"].ToString(), LoggingType.Information);
                                                            boolFound = true;
                                                            VirtualMachineRuntimeInfo oRuntime = (VirtualMachineRuntimeInfo)oVMWare.getObjectProperty(_vm_power, "runtime");
                                                            if (oRuntime.powerState != VirtualMachinePowerState.poweredOff)
                                                            {
                                                                oLog.AddEvent(strName, strSerial, "DESTROY: Powered Back On - Recommission", LoggingType.Information);
                                                                // Recommission the machine...
                                                                oWorkstation.UpdateVirtualRecommissioned(strName);
                                                                //strError = XenDesktop(0, strName, strSerial, strScripts + strSub, null, "", intAddress, XenConfigType.MaintOff, "Maintenance mode disabled in Xen Desktop");
                                                                strResult = "Virtual Machine " + strName + " Was Powered Back On - Workstation Recommissioned";
                                                            }
                                                            else
                                                            {
                                                                // Not running - workstation was not renamed but is not running...something odd going on here.
                                                                // Without setting boolDestroy = true; the request will throw a support error.
                                                                strError = "The virtual machine " + strName + " was found powered down, but was not renamed.";
                                                            }
                                                        }
                                                        else
                                                        {
                                                            // Machine does not exist.  boolFound is still false and will be used later.
                                                        }
                                                    }
                                                }
                                                catch (Exception exVmware)
                                                {
                                                    oLog.AddEvent(strName, strSerial, "DESTROY: There was a problem searching in " + drDataCenter["name"].ToString() + " on " + drVirtualCenter["name"].ToString() + " ~ " + exVmware.Message, LoggingType.Warning);
                                                }

                                                if (strResult != "")
                                                    oRequest.AddResult(intRequest, intItem, intNumber, strResult);
                                                oLog.AddEvent(strName, strSerial, "DESTROY: Finished Destroy", LoggingType.Information);

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
                                        }
                                    }
                                    if (boolFound == false)
                                    {
                                        //strError = "There was a problem destroying the device (Could Not Find Computer " + strName + strDecomSuffix + ")";
                                        boolDestroy = true;
                                        strResult = "Finished Destroying " + strName + " [" + DateTime.Now.ToString() + "]";
                                        //strError = XenDesktop(0, strName, strSerial, strScripts + strSub, null, "", intAddress, XenConfigType.Remove, "Machine removed from Xen Desktop");
                                        oLog.AddEvent(strName, strSerial, "DESTROY: Could Not Find Computer " + strName + strDecomSuffix + "...sending Initiating Decom Request", LoggingType.Information);
                                    }
                                }
                                else
                                {
                                    // Workstation is responding, was turned back on
                                    strError = "Workstation has been turned back on";
                                }
                            }
                        }
                        else
                            strError = "There was a problem destroying the device (No asset information found for vmware guest " + strName + ")";
                    }
                    else
                    {
                        strResult = "Destroy: Manual Intervention completed";
                        boolDestroy = true;
                        //strError = XenDesktop(0, strName, strSerial, strScripts + strSub, null, "", intAddress, XenConfigType.Remove, "Machine removed from Xen Desktop");
                    }

                    if (strError != "")
                    {
                        oAsset.UpdateDecommissionRunning(intAsset, -1);
                        oRequest.AddResult(intRequest, intItem, intNumber, strError);
                        oLog.AddEvent(strName, strSerial, "DESTROY: " + strError, LoggingType.Error);
                        if (boolDecommissionErrorEmail == true)
                        {
                            // Send Email with Error
                            strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DEVELOPER_ERROR");
                            oFunction.SendEmail("Auto-Provisioning DESTROY Error", strEMailIdsBCC, "", "", "Auto-Provisioning DESTROY Error", "<p><b>This message is to inform you that there was a problem with the DESTROY process for workstation " + strName + ".</b><p><p>Error Message: " + strError + "</p>", true, false);
                        }
                        // Send Auto-Decom support task
                        int intDecomErrorItem = oService.GetItemId(intDestroyErrorService);
                        //int intDecomErrorNumber = oResourceRequest.GetNumber(intRequest, intDecomErrorItem);
                        int intDecomErrorNumber = intNumber;
                        double dblServiceHours = oServiceDetail.GetHours(intDestroyErrorService, 1);
                        int intDecomError = oServiceRequest.AddRequest(intRequest, intDecomErrorItem, intDestroyErrorService, 1, dblServiceHours, 2, intDecomErrorNumber, dsnServiceEditor);
                        oServiceRequest.NotifyTeamLead(intDecomErrorItem, intDecomError, intAssignPage, intViewPage, intEnvironment, "", dsnServiceEditor, dsnAsset, dsnIP, 0);
                    }
                    else if (boolDestroy == true)
                    {
                        oLog.AddEvent(strName, strSerial, strResult, LoggingType.Information);
                        oAsset.UpdateDecommissionDestroy(intAsset);
                        oAsset.AddStatus(intAsset, strName, (int)AssetStatus.Disposed, 0, DateTime.Now);
                        oAsset.UpdateDecommissionRunning(intAsset, 0);
                        if (boolDR == false)
                        {
                            // Clear Name
                            oLog.AddEvent(strName, strSerial, "Destroy: Releasing Name", LoggingType.Information);
                            oWorkstation.UpdateName(intName, 1);

                            // Remove AD group(s)
                            oLog.AddEvent(strName, strSerial, "Destroy: Removing AD Groups", LoggingType.Information);
                            AD oAD = new AD(0, dsn, 999);
                            string strGroupRemote = "GSGu_WKS" + strName + "RemoteA";
                            DirectoryEntry oGroupRemote = oAD.GroupSearch(strGroupRemote);
                            if (oGroupRemote != null) 
                            {
                                string strGroupResult = oAD.Delete(oGroupRemote);
                                if (strGroupResult.Trim() != "")
                                    oLog.AddEvent(strName, strSerial, strGroupRemote + " was NOT successfully deleted ~ " + strGroupResult, LoggingType.Information);
                                else
                                    oLog.AddEvent(strName, strSerial, strGroupRemote + " was successfully deleted", LoggingType.Information);
                            }
                            string strGroupRemoteVDI = "GSLwksRD_LAR_" + strName.ToUpper();
                            DirectoryEntry oGroupRemoteVDI = oAD.GroupSearch(strGroupRemoteVDI);
                            if (oGroupRemoteVDI != null)
                            {
                                string strGroupResultVDI = oAD.Delete(oGroupRemoteVDI);
                                if (strGroupResultVDI.Trim() != "")
                                    oLog.AddEvent(strName, strSerial, strGroupRemoteVDI + " was NOT successfully deleted ~ " + strGroupResultVDI, LoggingType.Information);
                                else
                                    oLog.AddEvent(strName, strSerial, strGroupRemoteVDI + " was successfully deleted", LoggingType.Information);
                            }

                            // Remove the line(s) of the name from the INI file.
                            RemoveFileLine(strName, strINIProd, 4);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.ToUpper().StartsWith("A TRANSPORT-LEVEL ERROR") == false)
                {
                    if (ex.Message.ToUpper().Contains("HTTP STATUS 503") == false)
                    {
                        string strError = "VMWare Service (DESTROY): " + strName + " (WORKSTATIONID = " + intWorkstation.ToString() + ") (Error Message: " + ex.Message + ") ~ (Source: " + ex.Source + ") (Stack Trace: " + ex.StackTrace + ") [" + System.Environment.UserName + "]";
                        oEventLog.WriteEntry(strError, EventLogEntryType.Error);
                        SystemError(strError);
                    }
                }
            }
        }
        private void AddResult(int intWorkstation, int intStep, int intType, string strResult, string strError)
        {
            OnDemand oOnDemand = new OnDemand(0, dsn);
            Workstations oWorkstation = new Workstations(0, dsn);
            DataSet dsError = oWorkstation.GetVirtualError(intWorkstation, intStep);
            if (strError == "")
            {
                oOnDemand.UpdateStepDoneWorkstation(intWorkstation, intStep, strResult, 0, false, false);
                oWorkstation.NextVirtualStep(intWorkstation);
            }
            else if (oOnDemand.GetStep(intType, intStep, "resume_error") == "1")
            {
                oOnDemand.UpdateStepDoneWorkstation(intWorkstation, intStep, strError, 1, false, false);
                oWorkstation.NextVirtualStep(intWorkstation);
            }
            else
                oOnDemand.UpdateStepDoneWorkstation(intWorkstation, intStep, strError, 1, false, false);
        }
        private void InstallTick()
        {
            try
            {
                Workstations oWorkstation = new Workstations(0, dsn);
                Functions oFunction = new Functions(0, dsn, 0);
                DataSet dsInstalls = oWorkstation.GetComponents();
                if (dsInstalls.Tables[0].Rows.Count > 0)
                {
                    int intWorkstation = Int32.Parse(dsInstalls.Tables[0].Rows[0]["workstationid"].ToString());
                    string strName = oWorkstation.GetName(intWorkstation);
                    string strIP = oWorkstation.GetVirtual(intWorkstation, strZeus);
                    if (strIP != "")
                    {
                        DataSet dsActive = oWorkstation.GetComponentsActive(intWorkstation);
                        if (dsActive.Tables[0].Rows.Count == 0)
                        {
                            DataSet dsWorkstation = oWorkstation.GetVirtual(intWorkstation);
                            int intAnswer = Int32.Parse(dsWorkstation.Tables[0].Rows[0]["answerid"].ToString());
                            int intComponent = Int32.Parse(dsInstalls.Tables[0].Rows[0]["componentid"].ToString());
                            oWorkstation.UpdateComponents(intWorkstation, intComponent, 0);
                            oLog.AddEvent(strName, "", "Starting installation (" + dsInstalls.Tables[0].Rows[0]["name"].ToString() + ")", LoggingType.Information);
                            DataSet dsScripts = oWorkstation.GetComponentScripts(intComponent, 1);
                            if (dsScripts.Tables[0].Rows.Count > 0)
                            {
                                DateTime _now = DateTime.Now;
                                string strNow = _now.Day.ToString() + _now.Month.ToString() + _now.Year.ToString() + _now.Hour.ToString() + _now.Minute.ToString() + _now.Second.ToString() + _now.Millisecond.ToString();
                                string strBatch1 = strScripts + strSub + intAnswer.ToString() + "_" + strNow + "_install_1.bat";
                                // ********** START : CHANGED CODE ON 7/31/2008 TO BATCH FILE COPY *******************
                                // 1st part - create BAT file to copy to workstation (install_1.bat)
                                StreamWriter oWriter1 = new StreamWriter(strBatch1);
                                foreach (DataRow drScript in dsScripts.Tables[0].Rows)
                                    oWriter1.WriteLine(oFunction.ProcessLine(drScript["script"].ToString(), dsWorkstation.Tables[0].Rows[0]));
                                oWriter1.Flush();
                                oWriter1.Close();
                                // 2nd part - create BAT file to do the copy (install_2.bat)
                                string strBatch2 = strScripts + strSub + intAnswer.ToString() + "_" + strNow + "_install_2.bat";
                                StreamWriter oWriter2 = new StreamWriter(strBatch2);
                                oWriter2.WriteLine("F:");
                                oWriter2.WriteLine("cd " + strScripts + strSub);
                                oWriter2.WriteLine("net use \\\\" + strIP + "\\C$ /user:" + strAdminUser + " " + strAdminPass + "");
                                oWriter2.WriteLine("mkdir \\\\" + strIP + "\\C$\\OPTIONS");
                                oWriter2.WriteLine("copy " + strBatch1 + " \\\\" + strIP + "\\C$\\OPTIONS\\CV_INSTALL_" + strNow + ".BAT");
                                oWriter2.Flush();
                                oWriter2.Close();
                                // 3rd part - run the batch file to perform copy
                                string strFile1 = strScripts + strSub + intAnswer.ToString() + "_" + strNow + "_install_1.vbs";
                                string strFile1Out = strScripts + strSub + intAnswer.ToString() + "_" + strNow + "_install_1.txt";
                                StreamWriter oWriter3 = new StreamWriter(strFile1);
                                oWriter3.WriteLine("Dim objShell");
                                oWriter3.WriteLine("Set objShell = CreateObject(\"WScript.Shell\")");
                                oWriter3.WriteLine("objShell.Run(\"cmd.exe /c " + strBatch2 + " > " + strFile1Out + "\")");
                                oWriter3.WriteLine("Set objShell = Nothing");
                                oWriter3.Flush();
                                oWriter3.Close();
                                ILaunchScript oScript1 = new SimpleLaunchWsh(strFile1, "", true, 30) as ILaunchScript;
                                oScript1.Launch();
                                ReadOutput(intWorkstation, "INSTALL_" + dsInstalls.Tables[0].Rows[0]["name"].ToString() + "_START", strFile1Out, strName, "");
                                // 4th part - file has been copied, do the PSEXEC to install application
                                System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo(strScripts + "psexec");
                                info.WorkingDirectory = strScripts;
                                info.Arguments = "\\\\" + strIP + " -u " + strAdminUser + " -p " + strAdminPass + " -i cmd.exe /c C:\\OPTIONS\\CV_INSTALL_" + strNow + ".BAT";
                                oLog.AddEvent(strName, "", "PSEXEC Script = " + "\\\\" + strIP + " -u " + strAdminUser + " -p ***** -i cmd.exe /c C:\\OPTIONS\\CV_INSTALL_" + strNow + ".BAT", LoggingType.Information);
                                System.Diagnostics.Process proc = System.Diagnostics.Process.Start(info);
                                bool boolTimeout = false;
                                proc.WaitForExit(intTimeoutInstall);
                                if (proc.HasExited == false)
                                {
                                    proc.Kill();
                                    boolTimeout = true;
                                }
                                proc.Close();
                                if (boolTimeout == false)
                                {
                                    // 5th part - create BAT file to delete the copy (install_3.bat)
                                    string strBatch3 = strScripts + strSub + intAnswer.ToString() + "_" + strNow + "_install_3.bat";
                                    StreamWriter oWriter4 = new StreamWriter(strBatch3);
                                    oWriter4.WriteLine("del \\\\" + strIP + "\\C$\\OPTIONS\\CV_INSTALL_" + strNow + ".BAT");
                                    oWriter4.WriteLine("net use \\\\" + strIP + "\\C$ /dele");
                                    oWriter4.Flush();
                                    oWriter4.Close();
                                    // 3rd part - run the batch file to perform copy
                                    string strFile2 = strScripts + strSub + intAnswer.ToString() + "_" + strNow + "_install_2.vbs";
                                    string strFile2Out = strScripts + strSub + intAnswer.ToString() + "_" + strNow + "_install_2.txt";
                                    StreamWriter oWriter5 = new StreamWriter(strFile2);
                                    oWriter5.WriteLine("Dim objShell");
                                    oWriter5.WriteLine("Set objShell = CreateObject(\"WScript.Shell\")");
                                    oWriter5.WriteLine("objShell.Run(\"cmd.exe /c " + strBatch3 + " > " + strFile2Out + "\")");
                                    oWriter5.WriteLine("Set objShell = Nothing");
                                    oWriter5.Flush();
                                    oWriter5.Close();
                                    ILaunchScript oScript2 = new SimpleLaunchWsh(strFile2, "", true, 30) as ILaunchScript;
                                    oScript2.Launch();
                                    ReadOutput(intWorkstation, "INSTALL_" + dsInstalls.Tables[0].Rows[0]["name"].ToString() + "_END", strFile2Out, strName, "");
                                    oLog.AddEvent(strName, "", "Finished installation (" + dsInstalls.Tables[0].Rows[0]["name"].ToString() + ")", LoggingType.Information);
                                    oWorkstation.UpdateComponents(intWorkstation, intComponent, 1);
                                }
                                else
                                    oWorkstation.UpdateComponents(intWorkstation, intComponent, -10);
                            }
                            else
                            {
                                oLog.AddEvent(strName, "", "Skipping installation (" + dsInstalls.Tables[0].Rows[0]["name"].ToString() + ") - no scripts to run", LoggingType.Information);
                                oWorkstation.UpdateComponents(intWorkstation, intComponent, 1);
                            }
                        }
                        else
                            oLog.AddEvent(strName, "", "Installation already running", LoggingType.Warning);
                    }
                    else
                        oLog.AddEvent(strName, "", "Cannot connect to workstation " + strName + " to start installations", LoggingType.Warning);
                }
                else if (intLogging > 1)
                    oEventLog.WriteEntry("No VMWare installations to run", EventLogEntryType.Information);
            }
            catch (Exception ex)
            {
                if (ex.Message.ToUpper().StartsWith("A TRANSPORT-LEVEL ERROR") == false)
                {
                    string strError = "VMWare Workstation Service (INSTALLATION): " + ex.Message;
                    oEventLog.WriteEntry(strError, EventLogEntryType.Error);
                    SystemError(strError);
                }
            }
        }
        public VirtualDeviceConfigSpec ControllerBusLogic()
        {
            VirtualBusLogicController scsi = new VirtualBusLogicController();
            VirtualDeviceConnectInfo ci = new VirtualDeviceConnectInfo();
            ci.startConnected = false;
            scsi.key = 1000;
            scsi.controllerKey = 100;
            scsi.controllerKeySpecified = true;
            scsi.busNumber = 0;
            scsi.hotAddRemove = true;
            scsi.hotAddRemoveSpecified = true;
            scsi.scsiCtlrUnitNumber = 7;
            scsi.scsiCtlrUnitNumberSpecified = true;
            scsi.sharedBus = VirtualSCSISharing.noSharing;
            scsi.unitNumber = 2;
            scsi.unitNumberSpecified = true;
            scsi.connectable = ci;
            VirtualDeviceConfigSpec dcs = new VirtualDeviceConfigSpec();
            dcs.device = scsi;
            dcs.operation = VirtualDeviceConfigSpecOperation.add;
            dcs.operationSpecified = true;
            return dcs;
        }
        public VirtualDeviceConfigSpec ControllerLSILogic()
        {
            VirtualLsiLogicController scsi = new VirtualLsiLogicController();
            VirtualDeviceConnectInfo ci = new VirtualDeviceConnectInfo();
            ci.startConnected = false;
            scsi.key = 1000;
            scsi.controllerKey = 100;
            scsi.controllerKeySpecified = true;
            scsi.busNumber = 0;
            scsi.hotAddRemove = true;
            scsi.hotAddRemoveSpecified = true;
            scsi.scsiCtlrUnitNumber = 7;
            scsi.scsiCtlrUnitNumberSpecified = true;
            scsi.sharedBus = VirtualSCSISharing.noSharing;
            scsi.unitNumber = 2;
            scsi.unitNumberSpecified = true;
            scsi.connectable = ci;
            VirtualDeviceConfigSpec dcs = new VirtualDeviceConfigSpec();
            dcs.device = scsi;
            dcs.operation = VirtualDeviceConfigSpecOperation.add;
            dcs.operationSpecified = true;
            return dcs;
        }
        public VirtualDeviceConfigSpec Disk(VMWare oVMWare, string _name, string _datastore, string _size, int _unit_number, string _suffix)
        {
            // Get Datastores
            ManagedObjectReference datastoreRef = null;
            PropertySpec[] psDCarray = new PropertySpec[] { new PropertySpec() };
            psDCarray[0].all = false;
            psDCarray[0].allSpecified = true;
            psDCarray[0].pathSet = new string[] { "datastore" };
            psDCarray[0].type = "Datacenter";
            PropertyFilterSpec spec = new PropertyFilterSpec();
            spec.propSet = psDCarray;
            spec.objectSet = new ObjectSpec[] { new ObjectSpec() };
            spec.objectSet[0].obj = oVMWare.GetDataCenter();
            spec.objectSet[0].skip = true;
            ObjectContent[] ocary = oVMWare.GetService().RetrieveProperties(oVMWare.GetSic().propertyCollector, new PropertyFilterSpec[] { spec });
            ManagedObjectReference[] datastores = null;
            foreach (ObjectContent oc in ocary)
                datastores = (ManagedObjectReference[])oc.propSet[0].val;
            if (datastores != null)
                datastoreRef = datastores[0];

            // Create disk
            VirtualDisk disk = new VirtualDisk();
            disk.key = 2000;
            disk.controllerKey = 1000;
            disk.controllerKeySpecified = true;
            disk.unitNumber = _unit_number;
            disk.unitNumberSpecified = true;
            VirtualDiskFlatVer2BackingInfo diskBack = new VirtualDiskFlatVer2BackingInfo();
            diskBack.diskMode = "persistent";
            diskBack.fileName = "[" + _datastore + "] " + _name.ToLower() + "/" + _name.ToLower() + _suffix + ".vmdk";
            diskBack.datastore = datastoreRef;
            diskBack.thinProvisioned = false;
            diskBack.thinProvisionedSpecified = true;
            diskBack.writeThrough = false;
            diskBack.writeThroughSpecified = true;
            disk.backing = diskBack;
            disk.capacityInKB = long.Parse(_size);
            VirtualDeviceConfigSpec dcs = new VirtualDeviceConfigSpec();
            dcs.device = disk;
            dcs.fileOperation = VirtualDeviceConfigSpecFileOperation.create;
            dcs.fileOperationSpecified = true;
            dcs.operation = VirtualDeviceConfigSpecOperation.add;
            dcs.operationSpecified = true;
            return dcs;
        }

        private void SystemError(string _error)
        {
            SystemError(0, 0, _error, 0, 0, null);
        }
        private void SystemError(int _workstation, int _stepid, string _error, int _assetid, int _modelid, VMWare _vmware)
        {
            try
            {
                Settings oSetting = new Settings(0, dsn);
                oSetting.SystemError(0, _workstation, _stepid, _error, _assetid, _modelid, true, _vmware, intEnvironment, dsnAsset);
            }
            catch (Exception exSystemError)
            {
                // If database is offline, it will cause a fatal error AND stop the service.  Stop that from happening by using a catch here.
                oEventLog.WriteEntry("The database is currently offline.  Error message = " + exSystemError.Message, EventLogEntryType.Error);
            }
        }
    }
}
