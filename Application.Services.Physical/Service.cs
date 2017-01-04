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
using System.Web.Services;
using System.Net.NetworkInformation;
using NCC.ClearView.Application.Core.ClearViewWS;
using Tamir.SharpSsh;
using Tamir.Streams;
using System.Net;
using NCC.ClearView.Application.Core.w08r2;
using System.Collections;
using System.Xml;

namespace ClearViewAP_Physical
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
        private string dsnIP;
        private string dsnServiceEditor;
        private string dsnZeus;
        private int intEnvironment;
        private int intProduction;
        private int intCore;
        private string strServerTypesBuild = "";
        private string strServerTypesDecom = "";
        private int intLogging;
        private EventLog oEventLog;
        private Log oLog;
        private string strFile = "";
        private string strFilePath = "";
        private string strScripts = "E:\\APPS\\CLV\\ClearViewAP_Physical\\";
        private string strSub = "scripts\\";
        private string strZeus = "";
        private string strDomainsList = "";
        private string strDomainDefault = "";
        //private int intImplementorDistributed = 0;
        //private int intImplementorMidrange = 0;
        private int intWorkstationPlatform = 0;
        private int intStorageService = 0;
        private int intBackupService = 0;
        private int intDNSService = 0;
        private int intCSMService = 0;
        private int intServerAuditServicePNC = 0;
        private int intServerAuditServiceNCC = 0;
        private int intServerAuditErrorService = 0;
        private int intServerAuditErrorServiceMIS = 0;
        private int intProvisioningErrorService = 0;
        private int intDecomErrorService = 0;
        private int intDestroyErrorService = 0;
        private bool boolProvisioningErrorEmail = false;
        private bool boolDecommissionErrorEmail = false;
        private int intResourceRequestApprove = 0;
        private int intAssignPage = 0;
        private int intViewPage = 0;
        private int intEnvironmentHA = 0;
        private string strBootServer;
        private string strBootScript;
        private string strBootScriptD;
        
        private string strHA = "";
        private string strHBAJob = "";
        private int intBuildTest = 0;
        private int intBuildQA = 0;
        private int intBuildQAOffHours = 0;
        private int intBuildProd = 0;
        private int intBuildProdOffHours = 0;
        private bool boolUsePNCNaming = true;
        private bool boolSSHDebug = false;
        private string strTestProjects = "";
        private bool boolNotifyDecom = false;
        private string strDSMADMC = "";
        private bool boolMultiThreadedAudit = false;
        private bool boolDeleteScriptFiles = false;

        private int intErrorServer = 0;
        private int intErrorStep = 0;
        private int intErrorAsset = 0;
        private int intErrorModel = 0;

        private int intIMDecommServiceId = 0;
        private string strEMailIdsBCC = "";
        private bool boolNewIP = false;
        private int intTimeoutDefault = (5 * 60 * 1000);    // 5 minutes
        private int intTimeoutInstall = (30 * 60 * 1000);   // 30 minutes
        private string strAvamarOSs = "";
        private string strAvamarLocations = "";

        private bool boolForceDNSSuccess = false;       // If set to true, will catch any errors caused by DNS registration and set to "Registered"
        private bool boolInitiateDNSRequest = false;    // If set to true, each result of a DNS request which is not a SUCCESS or DUPLICATE will send an error request.
        private int intAuditCounts = 0;     // The number of audits that can be run at one time
        private int intAuditCount = 0;      // The current number of audits running (compared against intAuditCounts)
        public int AuditCount
        {
            get { return intAuditCount; }
            set { intAuditCount = value; }
        }
        private string strDemos = "USE7355XA4;";

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
                intCore = Int32.Parse(ds.Tables[0].Rows[0]["coreid"].ToString());
                strServerTypesBuild = ds.Tables[0].Rows[0]["types_build"].ToString();
                strServerTypesDecom = ds.Tables[0].Rows[0]["types_decom"].ToString();
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
                strDomainsList = ds.Tables[0].Rows[0]["domains"].ToString();
                strDomainDefault = ds.Tables[0].Rows[0]["domain_default"].ToString();
                //intImplementorDistributed = Int32.Parse(ds.Tables[0].Rows[0]["imp_distributed"].ToString());
                //intImplementorMidrange = Int32.Parse(ds.Tables[0].Rows[0]["imp_midrange"].ToString());
                intWorkstationPlatform = Int32.Parse(ds.Tables[0].Rows[0]["workstation_platform"].ToString());
                intStorageService = Int32.Parse(ds.Tables[0].Rows[0]["storage_service"].ToString());
                intBackupService = Int32.Parse(ds.Tables[0].Rows[0]["backup_service"].ToString());
                intDNSService = Int32.Parse(ds.Tables[0].Rows[0]["dns_service"].ToString());
                intCSMService = Int32.Parse(ds.Tables[0].Rows[0]["csm_service"].ToString());
                intServerAuditServicePNC = Int32.Parse(ds.Tables[0].Rows[0]["server_audit_service_pnc"].ToString());
                intServerAuditServiceNCC = Int32.Parse(ds.Tables[0].Rows[0]["server_audit_service_ncc"].ToString());
                intServerAuditErrorService = Int32.Parse(ds.Tables[0].Rows[0]["server_audit_error_service"].ToString());
                intServerAuditErrorServiceMIS = Int32.Parse(ds.Tables[0].Rows[0]["server_audit_error_service_mis"].ToString());
                intProvisioningErrorService = Int32.Parse(ds.Tables[0].Rows[0]["provisioning_error_service"].ToString());
                intDecomErrorService = Int32.Parse(ds.Tables[0].Rows[0]["decom_error_service"].ToString());
                intDestroyErrorService = Int32.Parse(ds.Tables[0].Rows[0]["destroy_error_service"].ToString());
                boolProvisioningErrorEmail = (ds.Tables[0].Rows[0]["provisioning_error_email"].ToString() == "1");
                boolDecommissionErrorEmail = (ds.Tables[0].Rows[0]["decommission_error_email"].ToString() == "1");
                intResourceRequestApprove = Int32.Parse(ds.Tables[0].Rows[0]["rr_approve"].ToString());
                intAssignPage = Int32.Parse(ds.Tables[0].Rows[0]["assign_page"].ToString());
                intViewPage = Int32.Parse(ds.Tables[0].Rows[0]["view_page"].ToString());
                intEnvironmentHA = Int32.Parse(ds.Tables[0].Rows[0]["environment_ha"].ToString());
                strBootServer = ds.Tables[0].Rows[0]["boot_server"].ToString();
                strBootScript = ds.Tables[0].Rows[0]["boot_script"].ToString();
                strBootScriptD = ds.Tables[0].Rows[0]["boot_script_d"].ToString();
                intBuildTest = Int32.Parse(ds.Tables[0].Rows[0]["build_test"].ToString());
                intBuildQA = Int32.Parse(ds.Tables[0].Rows[0]["build_qa"].ToString());
                intBuildQAOffHours = Int32.Parse(ds.Tables[0].Rows[0]["build_qa_off_hours"].ToString());
                intBuildProd = Int32.Parse(ds.Tables[0].Rows[0]["build_prod"].ToString());
                intBuildProdOffHours = Int32.Parse(ds.Tables[0].Rows[0]["build_prod_off_hours"].ToString());
                intLogging = Int32.Parse(ds.Tables[0].Rows[0]["logging"].ToString());
                intIMDecommServiceId = Int32.Parse(ds.Tables[0].Rows[0]["SERVICEID_SERVER_DECOMMISSION_IM"].ToString());
                strHA = ds.Tables[0].Rows[0]["ha"].ToString();
                strHBAJob = ds.Tables[0].Rows[0]["hba_job"].ToString();
                boolSSHDebug = (ds.Tables[0].Rows[0]["ssh_debug"].ToString() == "1");
                strTestProjects = ds.Tables[0].Rows[0]["test_projects"].ToString();
                boolNotifyDecom = (ds.Tables[0].Rows[0]["NOTIFY_DECOM"].ToString() == "1");
                strDSMADMC = ds.Tables[0].Rows[0]["DSMADMC"].ToString();
                boolMultiThreadedAudit = (ds.Tables[0].Rows[0]["multi_thread_audit"].ToString() == "1");
                boolDeleteScriptFiles = (ds.Tables[0].Rows[0]["delete_script_files"].ToString() == "1");
                boolForceDNSSuccess = (ds.Tables[0].Rows[0]["force_dns_success"].ToString() == "1");
                boolInitiateDNSRequest = (ds.Tables[0].Rows[0]["initiate_dns_request"].ToString() == "1");
                intAuditCounts = Int32.Parse(ds.Tables[0].Rows[0]["audit_count"].ToString());
                strAvamarOSs = ds.Tables[0].Rows[0]["avamar_os"].ToString();
                strAvamarLocations = ds.Tables[0].Rows[0]["avamar_locations"].ToString();
                oLog = new Log(0, dsn);
                bool boolTestD = false;
                if (boolTestD == true)
                    ServiceTick();
                else
                {
                    oTimer = new System.Timers.Timer(dblInterval);
                    oTimer.Elapsed += new ElapsedEventHandler(this.ServiceTimer_Tick);
                }
            }
            catch
            {
                EventLog.WriteEntry(String.Format("ClearView AP Physical Service initialization has failed - INVALID XML FILE"), EventLogEntryType.Error);
            }
        }

        protected override void OnStart(string[] args)
        {
            oEventLog.WriteEntry(String.Format("ClearView AP Physical Service started."), EventLogEntryType.Information);
            oTimer.AutoReset = true;
            oTimer.Enabled = true;
            oTimer.Start();
        }

        protected override void OnStop()
        {
            oEventLog.WriteEntry(String.Format("ClearView AP Physical Service stopped."), EventLogEntryType.Information);
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
                oEventLog.WriteEntry(String.Format("ClearView AP Physical Service TICK."), EventLogEntryType.Information);

            // Start Main Processing
            try
            {
                ServiceTick();

                // Start Installations
                ThreadStart oJobInstall = new ThreadStart(InstallTick);
                Thread oThreadJobInstall = new Thread(oJobInstall);
                oThreadJobInstall.Start();

                // Start Decommissions
                ServiceTickDecom();

                // Execute MIS Audits
                ExecuteMISAudits();
                // *******************************************
                // ************  END Processing  *************
                // *******************************************
                oTimer.Start();
            }
            catch (Exception ex)
            {
                string strError = "Physical Service (TICK): " + "(Error Message: " + ex.Message + ") ~ (Source: " + ex.Source + ") (Stack Trace: " + ex.StackTrace + ") [" + System.Environment.UserName + "]";
                oEventLog.WriteEntry(strError, EventLogEntryType.Error);
            }
        }
        private void ServiceTick()
        {
            try
            {
                Servers oServer = new Servers(0, dsn);
                DataSet ds = oServer.GetTypes(strServerTypesBuild);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    bool boolProcess = false;
                    Projects oProject = new Projects(0, dsn);
                    Requests oRequest = new Requests(0, dsn);
                    Forecast oForecast = new Forecast(0, dsn);
                    Models oModel = new Models(0, dsn);
                    Types oType = new Types(0, dsn);
                    ModelsProperties oModelsProperties = new ModelsProperties(0, dsn);
                    OnDemand oOnDemand = new OnDemand(0, dsn);
                    Asset oAsset = new Asset(0, dsnAsset, dsn);
                    ServerName oServerName = new ServerName(0, dsn);
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
                    Host oHost = new Host(0, dsn);
                    Locations oLocation = new Locations(0, dsn);
                    Mnemonic oMnemonic = new Mnemonic(0, dsn);
                    Resiliency oResiliency = new Resiliency(0, dsn);
                    BuildLocation oBuildLocation = new BuildLocation(0, dsn);
                    ServiceDetails oServiceDetail = new ServiceDetails(0, dsn);
                    Organizations oOrganization = new Organizations(0, dsn);
                    ServiceRequests oServiceRequest = new ServiceRequests(0, dsn);
                    Audit oAudit = new Audit(0, dsn);
                    Cluster oCluster = new Cluster(0, dsn);
                    Settings oSetting = new Settings(0, dsn);

                    char[] strSplit = { ';' };
                    int intServer = Int32.Parse(dr["id"].ToString());
                    intErrorServer = intServer;
                    int intStep = Int32.Parse(dr["step"].ToString());
                    intErrorStep = intStep;
                    bool boolRebuilding = (dr["rebuilding"].ToString() == "1");
                    int intStepSkipStart = Int32.Parse(dr["step_skip_start"].ToString());
                    if (intStepSkipStart > 0 && intStepSkipStart == intStep)
                    {
                        int intStepSkipGoto = Int32.Parse(dr["step_skip_goto"].ToString());
                        oServer.UpdateStep(intServer, intStepSkipGoto);
                        oServer.UpdateStepSkip(intServer, 0, 0);
                    }
                    else
                    {
                        int intAsset = 0;
                        if (dr["assetid"].ToString() != "")
                            intAsset = Int32.Parse(dr["assetid"].ToString());
                        intErrorAsset = intAsset;
                        int intAssetDR = 0;
                        if (dr["assetid_dr"].ToString() != "")
                            intAssetDR = Int32.Parse(dr["assetid_dr"].ToString());
                        string strSerial = oAsset.GetServerOrBlade(intAsset, "serial").ToUpper();
                        string strAsset = oAsset.GetServerOrBlade(intAsset, "asset").ToUpper();
                        int intModel = 0;
                        int intParent = 0;
                        bool boolBlade = false;
                        bool boolDell = false;
                        int intType = 0;
                        int intZone = 0;
                        if (intAsset > 0)
                        {
                            intModel = Int32.Parse(oAsset.Get(intAsset, "modelid"));
                            intErrorModel = intModel;
                            intParent = Int32.Parse(oModelsProperties.Get(intModel, "modelid"));
                            boolBlade = oModelsProperties.IsTypeBlade(intModel);
                            boolDell = oModelsProperties.IsDell(intModel);
                            intType = oModelsProperties.GetType(intModel);
                            if (oAsset.GetServerOrBlade(intAsset, "zoneid") != "")
                                intZone = Int32.Parse(oAsset.GetServerOrBlade(intAsset, "zoneid"));
                        }
                        int intStepID = 0;
                        string strStep = "N / A";
                        DataSet dsSteps = oOnDemand.GetSteps(intType, 1);
                        if (dsSteps.Tables[0].Rows.Count > 0)
                        {
                            strStep = dsSteps.Tables[0].Rows[intStep - 1]["title"].ToString();
                            Int32.TryParse(dsSteps.Tables[0].Rows[intStep - 1]["id"].ToString(), out intStepID);
                        }

                        // Once switches can be automated (to change VLAN) modify this...
                        bool boolOKtoAssignIP = false;
                        bool boolIsNexusSwitch = false;
                        DataSet dsSwitch = oAsset.GetSwitchports(intAsset, SwitchPortType.Network);
                        if (boolBlade == true)
                        {
                            if (boolDell == true)
                            {
                                boolIsNexusSwitch = true;
                                int intEnclosure = Int32.Parse(oAsset.GetServerOrBlade(intAsset, "enclosureid"));
                                int intSlot = Int32.Parse(oAsset.GetServerOrBlade(intAsset, "slot"));
                                string strEnclosure = oAsset.GetStatus(intEnclosure, "name");
                                DataSet dsDellBlade = oAsset.GetDellBladeSwitchports(strEnclosure, intSlot);
                                if (dsDellBlade.Tables[0].Rows.Count == 1)
                                    boolOKtoAssignIP = true;
                                else
                                    boolOKtoAssignIP = false;
                            }
                            else
                                boolOKtoAssignIP = true;
                        }
                        else
                        {
                            if (dsSwitch.Tables[0].Rows.Count > 0)
                            {
                                boolOKtoAssignIP = true;
                                // Check to see if switch is nexus, catalyst, etc...
                                boolIsNexusSwitch = (dsSwitch.Tables[0].Rows[0]["nexus"].ToString() == "1");
                            }
                        }
                        int intDNSAuto = Int32.Parse(dr["dns_auto"].ToString());
                        int intClusterID = Int32.Parse(dr["clusterid"].ToString());
                        int intNumber = Int32.Parse(dr["number"].ToString());
                        int intAnswer = Int32.Parse(dr["answerid"].ToString());
                        int intRequest = oForecast.GetRequestID(intAnswer, true);
                        int intProject = oRequest.GetProjectNumber(intRequest);
                        if (oProject.Get(intProject, "organization") == "")
                        {
                            // This was a new project created in design builder.  Get and set correct project number.
                            int intForecast = Int32.Parse(oForecast.GetAnswer(intAnswer, "forecastid"));
                            int intForecastRequest = Int32.Parse(oForecast.Get(intForecast, "requestid"));
                            int intForecastProject = oRequest.GetProjectNumber(intForecastRequest);
                            intProject = intForecastProject;
                            oRequest.Update(intRequest, intProject);
                        }
                        int intOrganization = Int32.Parse(oProject.Get(intProject, "organization"));
                        Forms oForms = new Forms(strScripts, boolUsePNCNaming, intProject);
                        int intUser = Int32.Parse(oForecast.GetAnswer(intAnswer, "userid"));
                        int intQuantity = Int32.Parse(oForecast.GetAnswer(intAnswer, "quantity"));
                        int intMnemonic = Int32.Parse(oForecast.GetAnswer(intAnswer, "mnemonicid"));
                        int intNetworkHA = Int32.Parse(oForecast.GetAnswer(intAnswer, "ha"));
                        bool boolBIR = (oForecast.GetAnswer(intAnswer, "resiliency") == "1");    // Related to the Business Resiliency Effort (use old data center strategy)
                        int intResiliency = oResiliency.GetIDFromMnemonic(intMnemonic, boolBIR, 0);
                        int intClass = Int32.Parse(dr["classid"].ToString());
                        int intEnv = Int32.Parse(dr["environmentid"].ToString());
                        int intHA = Int32.Parse(dr["ha"].ToString());
                        if (intHA == 10)    // means that the server was added by the SERVERS.START() function
                            intEnv = intEnvironmentHA;
                        int intAddress = Int32.Parse(dr["addressid"].ToString());
                        int intRecoveryNumber = Int32.Parse(dr["recovery_number"].ToString());
                        bool boolZeusError = (dr["zeus_error"].ToString() == "1");
                        bool boolPNC = (dr["pnc"].ToString() == "1" || oClass.Get(intClass, "pnc") == "1");
                        bool boolPNCProd = ((dr["pnc"].ToString() == "1" || oClass.Get(intClass, "pnc") == "1") && oClass.IsProd(intClass));
                        int intBuildClass = 0;
                        int intBuildEnv = 0;
                        int intBuildAddress = 0;
                        int intDomain = Int32.Parse(dr["domainid"].ToString());
                        int intTestDomain = Int32.Parse(dr["test_domainid"].ToString());
                        if (intTestDomain > 0)
                            intDomain = intTestDomain;
                        string strDomain = oDomain.Get(intDomain, "zeus");
                        int intDomainEnvironment = Int32.Parse(oDomain.Get(intDomain, "environment"));
                        Variables oVar = new Variables(intDomainEnvironment);
                        AD oAD = new AD(0, dsn, intDomainEnvironment);
                        DateTime datModified = DateTime.Parse(dr["modified"].ToString());
                        int intRemovable = (dr["test"].ToString() != "1" ? 1 : 0);
                        int intInfrastructure = (dr["infrastructure"].ToString() != "1" ? 0 : 1);
                        int intHost = Int32.Parse(oForecast.GetAnswer(intAnswer, "hostid"));
                        int intApplication = 0;
                        if (dr["applicationid"].ToString() != "")
                            intApplication = Int32.Parse(dr["applicationid"].ToString());
                        int intSubApplication = 0;
                        if (dr["subapplicationid"].ToString() != "")
                            intSubApplication = Int32.Parse(dr["subapplicationid"].ToString());
                        int intOS = Int32.Parse(dr["osid"].ToString());
                        string strOS = oOperatingSystem.Get(intOS, "name");
                        bool IsWin2012 = strOS.Contains("2012");
                        DataSet dsBuild = oBuildLocation.Gets(intClass, intEnv, intAddress, intParent);
                        bool boolOffsite = oLocation.IsOffsite(intAddress);
                        string strName = "";
                        int intServerName = 0;
                        if (dr["nameid"].ToString() != "")
                        {
                            intServerName = Int32.Parse(dr["nameid"].ToString());
                            if (intServerName > 0)
                                strName = oServer.GetName(intServer, boolUsePNCNaming);
                        }
                        string strNameDR = "";
                        if (dr["dr"].ToString() == "1" && dr["dr_name"].ToString() != "")
                            strNameDR = dr["dr_name"].ToString();
                        int intTSMAutomated = 0;
                        Int32.TryParse(oSetting.Get("tsm_automated"), out intTSMAutomated);
                        int intAvamarAutomated = 0;
                        Int32.TryParse(oSetting.Get("avamar_automated"), out intAvamarAutomated);
                        int intRecovery = 0;
                        DataSet dsBackup = oForecast.GetBackup(intAnswer);
                        if (dsBackup.Tables[0].Rows.Count > 0)
                            Int32.TryParse(dsBackup.Tables[0].Rows[0]["recoveryid"].ToString(), out intRecovery);
                        int intTSM = 0;
                        //Int32.TryParse(oLocation.GetAddress(intRecovery, "tsm"), out intTSM);
                        Int32.TryParse(oLocation.GetAddress(intAddress, "tsm"), out intTSM);
                        int intAvamar = Int32.Parse(dr["avamar"].ToString());
                        // Check to see if this is an avamar backup.
                        bool boolAvamar = (intAvamar == 1); // 0 = No, 1 = Yes
                        if (intAvamar < 0 && intTSM != 1)
                        {
                            boolAvamar = true;
                            // All non-TSM builds on PHYSICAL hardware = LEGATO
                            oForecast.UpdateAnswerAvamar(intAnswer, 1);
                        }
                        string strRDPVLAN = "";
                        /*
                        if (intAvamar < 0)
                        {
                            // Avamar flag = -1, meaning it has never been checked.
                            // First, check the Location
                            bool boolAvamarLocation = false;
                            string[] strAvamarLocation = strAvamarLocations.Split(strSplit);
                            for (int ii = 0; ii < strAvamarLocation.Length; ii++)
                            {
                                if (strAvamarLocation[ii].Trim() != "")
                                {
                                    if (strAvamarLocation[ii].Trim() == intAddress.ToString())
                                    {
                                        boolAvamarLocation = true;
                                        break;
                                    }
                                }
                            }
                            if (boolAvamarLocation == true)
                            {
                                // If the Location qualifies, then check the OS
                                string[] strAvamarOS = strAvamarOSs.Split(strSplit);
                                for (int ii = 0; ii < strAvamarOS.Length; ii++)
                                {
                                    if (strAvamarOS[ii].Trim() != "")
                                    {
                                        if (strAvamarOS[ii].Trim() == intOS.ToString())
                                        {
                                            // No physical servers should be avamar (all legato)
                                            //boolAvamar = true;
                                            break;
                                        }
                                    }
                                }
                            }
                            if (boolAvamar == true)
                            {
                                // Update the flag so we don't have to check next time.
                                oForecast.UpdateAnswerAvamar(intAnswer, 1);
                            }
                            else
                            {
                                oForecast.UpdateAnswerAvamar(intAnswer, 0);
                                if (boolAvamarLocation == true)
                                    oLog.AddEvent(intAnswer, strName, strSerial, "The OS (" + intOS.ToString() + ") is not a TSM Backup OS (" + strAvamarOSs + ")", LoggingType.Debug);
                                else
                                    oLog.AddEvent(intAnswer, strName, strSerial, "The Location (" + intAddress.ToString() + ") is not a TSM Backup Location (" + strAvamarLocations + ")", LoggingType.Debug);
                            }
                        }
                        */
                        // Get Admin Account Info
                        string strSource = "SERVER";
                        string strAdminUser = strName + "\\" + oVariable.LocalAdminUsername(boolPNC);
                        string strAdminPass = oVariable.LocalAdminPassword(boolPNC, strName);
                        if (boolPNC == true)
                        {
                            Variables oVarPNC = new Variables(intDomainEnvironment);
                            strAdminUser = oVarPNC.Domain() + "\\" + oVarPNC.ADUser();
                            strAdminPass = oVarPNC.ADPassword();
                        }

                        string strSolarisBuildNetwork = dr["solaris_build_network"].ToString();
                        string strSolarisInterface1 = dr["solaris_interface1"].ToString();
                        string strSolarisInterface2 = dr["solaris_interface2"].ToString();
                        string strSolarisBuildType = dr["solaris_build_type"].ToString();

                        int intIPAddressBuild1 = 0;
                        int intIPAddressBuild2 = 0;
                        int intIPAddressBuild3 = 0;
                        int intIPAddressFinal1 = 0;
                        int intIPAddressFinal2 = 0;
                        int intIPAddressFinal3 = 0;
                        int intNetwork = 0;
                        int intVlan = 0;
                        string strVLAN = "";
                        string strVLANname = "";
                        DataSet dsIPBuild = oServer.GetIP(intServer, 1, 0, 0, 0);
                        foreach (DataRow drIPBuild in dsIPBuild.Tables[0].Rows)
                        {
                            if (intIPAddressBuild1 == 0)
                                intIPAddressBuild1 = Int32.Parse(drIPBuild["ipaddressid"].ToString());
                            else if (intIPAddressBuild2 == 0)
                                intIPAddressBuild2 = Int32.Parse(drIPBuild["ipaddressid"].ToString());
                            else if (intIPAddressBuild3 == 0)
                                intIPAddressBuild3 = Int32.Parse(drIPBuild["ipaddressid"].ToString());
                        }
                        if (intIPAddressBuild1 > 0)
                        {
                            intNetwork = Int32.Parse(oIPAddresses.Get(intIPAddressBuild1, "networkid"));
                            intVlan = Int32.Parse(oIPAddresses.GetNetwork(intNetwork, "vlanid"));
                            strVLAN = oIPAddresses.GetVlan(intVlan, "vlan");
                            strVLANname = "VLAN_" + strVLAN;
                        }
                        DataSet dsIPFinal = oServer.GetIP(intServer, 0, 1, 0, 0);
                        foreach (DataRow drIPFinal in dsIPFinal.Tables[0].Rows)
                        {
                            if (intIPAddressFinal1 == 0)
                                intIPAddressFinal1 = Int32.Parse(drIPFinal["ipaddressid"].ToString());
                            else if (intIPAddressFinal2 == 0)
                                intIPAddressFinal2 = Int32.Parse(drIPFinal["ipaddressid"].ToString());
                            else if (intIPAddressFinal3 == 0)
                                intIPAddressFinal3 = Int32.Parse(drIPFinal["ipaddressid"].ToString());
                        }
                        int intIPAddressBackup = 0;
                        DataSet dsIPBackup = oServer.GetIP(intServer, 0, 0, 0, 1);
                        if (dsIPBackup.Tables[0].Rows.Count > 0)
                            intIPAddressBackup = Int32.Parse(dsIPBackup.Tables[0].Rows[0]["ipaddressid"].ToString());
                        int intIPAddressCluster = 0;
                        DataSet dsIPCluster = oServer.GetIP(intServer, 0, 0, 1, 0);
                        if (dsIPCluster.Tables[0].Rows.Count > 0)
                            intIPAddressCluster = Int32.Parse(dsIPCluster.Tables[0].Rows[0]["ipaddressid"].ToString());

                        bool boolRDPAltiris = (oOperatingSystem.Get(intOS, "rdp_altiris") == "1");
                        bool boolRDPMDT = (oOperatingSystem.Get(intOS, "rdp_mdt") == "1");
                        string strBootEnvironment = oOperatingSystem.Get(intOS, "boot_environment");
                        string strTaskSequence = oOperatingSystem.Get(intOS, "task_sequence");
                        if (boolDell == true && oOperatingSystem.IsSolaris(intOS) == false)
                        {
                            // Windows 2008, Windows 2003 and RHEL on DELLs have to go through the same build VLAN (920).
                            // So, they will have to be kicked off by Windows Deployment Toolkit.
                            boolRDPMDT = true;
                            boolRDPAltiris = false;
                        }
                        int intClassRDP = intClass;
                        int intEnvRDP = intEnv;
                        int intAddressRDP = intAddress;
                        DataSet dsBuildRDP = oBuildLocation.GetRDPs(intClassRDP, intEnvRDP, intAddressRDP, intResiliency, (boolRDPAltiris ? 1 : 0), (boolRDPMDT ? 1 : 0), 0, intZone);
                        if (boolOffsite == true)
                        {
                            intClassRDP = Int32.Parse(oSetting.Get("offsite_build_class"));
                            intEnvRDP = Int32.Parse(oSetting.Get("offsite_build_environment"));
                            intAddressRDP = Int32.Parse(oSetting.Get("offsite_build_location"));
                            dsBuildRDP = oBuildLocation.GetRDPs(intClassRDP, intEnvRDP, intAddressRDP, intResiliency, (boolRDPAltiris ? 1 : 0), (boolRDPMDT ? 1 : 0), 0, intZone);
                        }
                        else if (dsBuild.Tables[0].Rows.Count > 0)
                        {
                            intClassRDP = Int32.Parse(dsBuild.Tables[0].Rows[0]["build_classid"].ToString());
                            intEnvRDP = Int32.Parse(dsBuild.Tables[0].Rows[0]["build_environmentid"].ToString());
                            intAddressRDP = Int32.Parse(dsBuild.Tables[0].Rows[0]["build_addressid"].ToString());
                            dsBuildRDP = oBuildLocation.GetRDPs(intClassRDP, intEnvRDP, intAddressRDP, intResiliency, (boolRDPAltiris ? 1 : 0), (boolRDPMDT ? 1 : 0), 0, intZone);
                        }
                        int intSP = Int32.Parse(dr["spid"].ToString());
                        string strILO = dr["ilo"].ToString();
                        string strDHCP = "";
                        string strIP = "";
                        if (strZeus == "")
                            oLog.AddEvent(intAnswer, strName, strSerial, "ZEUS XML Field is Empty!", LoggingType.Error);
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
                        DataSet dsComponents = oServerName.GetComponentDetailSelected(intServer, 1);
                        DateTime _now = DateTime.Now;
                        TimeSpan oSpan = _now.Subtract(datModified);
                        string strNow = _now.Day.ToString() + _now.Month.ToString() + _now.Year.ToString() + _now.Hour.ToString() + _now.Minute.ToString() + _now.Second.ToString() + _now.Millisecond.ToString();
                        strFile = strScripts + strSub + intAnswer.ToString() + "_" + strNow + ".vbs";
                        strFilePath = strScripts + strSub + intAnswer.ToString() + "_";
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
                        string strMACAddress1 = "";
                        string strMACAddress2 = "";

                        int intImplementorUser = 0;
                        string strImplementor = "";
                        DataSet dsImplementor = oOnDemandTasks.GetPending(intAnswer);
                        if (dsImplementor.Tables[0].Rows.Count > 0)
                        {
                            intImplementorUser = Int32.Parse(dsImplementor.Tables[0].Rows[0]["resourceid"].ToString());
                            intImplementorUser = Int32.Parse(oResourceRequest.GetWorkflow(intImplementorUser, "userid"));
                            strImplementor = oUser.GetName(intImplementorUser);
                        }

                        int intBuildClassProcess = intClass;
                        if (dsBuild.Tables[0].Rows.Count > 0)
                            intBuildClassProcess = Int32.Parse(dsBuild.Tables[0].Rows[0]["build_classid"].ToString());

                        if (oClass.IsProd(intBuildClassProcess) || oClass.IsDR(intBuildClassProcess))
                        {
                            if (intBuildProd == 1)
                            {
                                if (intBuildProdOffHours == 0)
                                    boolProcess = true;
                                else if (DateTime.Now.Hour < 7 || DateTime.Now.Hour > 19)
                                    boolProcess = true;
                                else if (intLogging > 0)
                                    oEventLog.WriteEntry("SERVERID: " + intServer.ToString() + " (Physical): " + "PROD Classes are not enabled for DURING PEAK HOURS (7AM - 7PM) builds", EventLogEntryType.Information);
                            }
                            else if (intLogging > 0)
                                oEventLog.WriteEntry("SERVERID: " + intServer.ToString() + " (Physical): " + "PROD Classes are not enabled for builds", EventLogEntryType.Information);
                        }
                        if (oClass.IsQA(intBuildClassProcess))
                        {
                            if (intBuildQA == 1)
                            {
                                if (intBuildQAOffHours == 0)
                                    boolProcess = true;
                                else if (DateTime.Now.Hour < 7 || DateTime.Now.Hour > 19)
                                    boolProcess = true;
                                else if (intLogging > 0)
                                    oEventLog.WriteEntry("SERVERID: " + intServer.ToString() + " (Physical): " + "QA Classes are not enabled for DURING PEAK HOURS (7AM - 7PM) builds", EventLogEntryType.Warning);
                            }
                            else if (intLogging > 0)
                                oEventLog.WriteEntry("SERVERID: " + intServer.ToString() + " (Physical): " + "QA Classes are not enabled for builds", EventLogEntryType.Information);
                        }
                        if (oClass.IsTestDev(intBuildClassProcess))
                        {
                            if (intBuildTest == 1)
                                boolProcess = true;
                            else if (intLogging > 0)
                                oEventLog.WriteEntry("SERVERID: " + intServer.ToString() + " (Physical): " + "TEST Classes are not enabled for builds", EventLogEntryType.Information);
                        }

                        // ADD CODE FOR HARD-CODED DEMONSTRATIONS //
                        bool boolDemoSAN = false;
                        string[] strDemo = strDemos.Split(strSplit);
                        for (int ii = 0; ii < strDemo.Length; ii++)
                        {
                            if (strDemo[ii].Trim() != "")
                            {
                                if (strDemo[ii].Trim().ToUpper() == strSerial.ToUpper())    // Compare known serial numbers to currently allocated serial number.
                                {
                                    boolDemoSAN = true;
                                    break;
                                }
                            }
                        }
                        // Demo Project?
                        bool boolDemo = false;
                        string strNumber = oProject.Get(intProject, "number");
                        // Check to see if Demo
                        DataSet dsDemo = oFunction.GetSetupValuesByKey("DEMO_PROJECT");
                        foreach (DataRow drDemo in dsDemo.Tables[0].Rows)
                        {
                            if (strNumber == drDemo["Value"].ToString())
                            {
                                boolDemo = true;
                                break;
                            }
                        }
                        if (boolDemo)
                            intTSM = 1;

                        if (boolProcess == true)
                        {
                            // Add Step
                            DataSet dsStepDoneServer = oOnDemand.GetStepDoneServer(intServer, intStep);
                            if (dsStepDoneServer.Tables[0].Rows.Count == 0)
                                oOnDemand.AddStepDoneServer(intServer, intStep, false);

                            bool boolAuditError = false;

                            switch (intStep)
                            {
                                case 1:     // Select Asset
                                    oLog.AddEvent(intAnswer, strName, strSerial, "Starting Step 1 (Select Asset) for server ID " + intServer.ToString(), LoggingType.Information);
                                    oServer.UpdateBuildStarted(intServer, DateTime.Now.ToString());
                                    if (intAsset == 0)
                                    {
                                        // Get Production / Test Asset
                                        intModel = oForecast.GetModel(intAnswer);
                                        if (intModel == 0)
                                        {
                                            strError = "The model is not valid for this design ~ ID: " + intAnswer.ToString();
                                            oLog.AddEvent(intAnswer, strName, strSerial, strError, LoggingType.Error);
                                        }
                                        else
                                        {
                                            intErrorModel = intModel;
                                            intParent = Int32.Parse(oModelsProperties.Get(intModel, "modelid"));
                                            dsBuild = oBuildLocation.Gets(intClass, intEnv, intAddress, intParent);
                                            intType = oModelsProperties.GetType(intModel);
                                            boolBlade = oModelsProperties.IsTypeBlade(intModel);
                                            boolDell = oModelsProperties.IsDell(intModel);
                                        }
                                        if (intParent > 0)
                                        {
                                            // Check to see if this build is in the build environments
                                            oLog.AddEvent(intAnswer, strName, strSerial, "There are " + dsBuild.Tables[0].Rows.Count.ToString() + " build locations for class: " + oClass.Get(intClass, "name") + ", env: " + oEnvironment.Get(intEnv, "name") + ", address: " + oLocation.GetFull(intAddress) + ", model: " + oModel.Get(intParent, "name") + " for server ID " + intServer.ToString(), LoggingType.Information);
                                            if (boolOffsite == true)
                                            {
                                                oLog.AddEvent(intAnswer, strName, strSerial, "Offsite build location : " + oClass.Get(intClass, "name") + ", env: " + oEnvironment.Get(intEnv, "name") + ", address: " + oLocation.GetFull(intAddress) + ", model: " + oModel.Get(intParent, "name"), LoggingType.Information);
                                                intBuildClass = Int32.Parse(oSetting.Get("offsite_build_class"));
                                                intBuildEnv = Int32.Parse(oSetting.Get("offsite_build_environment"));
                                                intBuildAddress = Int32.Parse(oSetting.Get("offsite_build_location"));
                                            }
                                            else if (dsBuild.Tables[0].Rows.Count > 0)
                                            {
                                                oLog.AddEvent(intAnswer, strName, strSerial, "Build location found : " + oClass.Get(intBuildClass, "name") + ", env: " + oEnvironment.Get(intBuildEnv, "name") + ", address: " + oLocation.GetFull(intBuildAddress) + ", model: " + oModel.Get(intParent, "name"), LoggingType.Information);
                                                intBuildClass = Int32.Parse(dsBuild.Tables[0].Rows[0]["build_classid"].ToString());
                                                intBuildEnv = Int32.Parse(dsBuild.Tables[0].Rows[0]["build_environmentid"].ToString());
                                                intBuildAddress = Int32.Parse(dsBuild.Tables[0].Rows[0]["build_addressid"].ToString());
                                            }
                                            else
                                            {
                                                oLog.AddEvent(intAnswer, strName, strSerial, "No Build location : " + oClass.Get(intClass, "name") + ", env: " + oEnvironment.Get(intEnv, "name") + ", address: " + oLocation.GetFull(intAddress) + ", model: " + oModel.Get(intParent, "name"), LoggingType.Debug);
                                                intBuildClass = intClass;
                                                intBuildEnv = intEnv;
                                                intBuildAddress = intAddress;
                                            }
                                            bool boolGetDR = (intAssetDR == 0 && boolPNCProd == true && intRecoveryNumber > 0);
                                            List<int> lstAsset = oAsset.GetServerOrBladeAvailable(intBuildClass, intBuildEnv, intBuildAddress, intModel, intAnswer, dsn, strHA, true, intProject, intResiliency, intOS, strName, boolGetDR, boolDell, dsnServiceEditor);
                                            if (lstAsset[0] > 0)
                                            {
                                                intAsset = lstAsset[0];
                                                intErrorAsset = intAsset;
                                            }
                                            if (intAsset == 0)
                                            {
                                                if (intResiliency == 0)
                                                    intResiliency = oResiliency.GetIDFromMnemonic(intMnemonic, boolBIR, intAnswer);
                                                strError = "The inventory has been depleted ~ (ID: " + intServer.ToString() + "], [DesignID: " + intAnswer.ToString() + "], Server Model: " + oModelsProperties.Get(intModel, "name").ToUpper() + " [" + intModel.ToString() + "], Class: " + oClass.Get(intBuildClass, "name") + " [" + intBuildClass.ToString() + "], Environment: " + oEnvironment.Get(intBuildEnv, "name") + " [" + intBuildEnv.ToString() + "], Location: " + oLocation.GetFull(intBuildAddress) + " [" + intBuildAddress.ToString() + "], Model: " + oModel.Get(intParent, "name") + " [" + intParent.ToString() + "]),PROJECTID = " + intProject.ToString() + ", RESILIENCY = " + oResiliency.Get(intResiliency, "name") + " [" + intResiliency.ToString() + "], OS = " + strOS + " [" + intOS.ToString() + "]";
                                                oLog.AddEvent(intAnswer, strName, strSerial, "Out of Inventory ~ GetServerOrBlade(" + intBuildClass.ToString() + "," + intBuildEnv.ToString() + "," + intBuildAddress.ToString() + "," + intModel.ToString() + "," + intProject.ToString() + "," + intResiliency.ToString() + ")", LoggingType.Error);
                                            }
                                            else
                                            {
                                                bool boolDRisOK = false;
                                                // If PNC Prod and Blade, Reserve DR Asset (if recovery is > 0)
                                                if (boolGetDR)
                                                {
                                                    if (lstAsset[1] != 0)   // -1 = DR not required per the data center
                                                        intAssetDR = lstAsset[1];
                                                    if (intAssetDR == 0)
                                                    {
                                                        strError = "The Disaster Recovery inventory has been depleted ~ (ID: " + intServer.ToString() + "], [DesignID: " + intAnswer.ToString() + "], Server Model: " + oModelsProperties.Get(intModel, "name").ToUpper() + " [" + intModel.ToString() + "], Environment: " + oEnvironment.Get(intEnv, "name") + " [" + intEnv.ToString() + "], Model: " + oModelsProperties.Get(intModel, "name") + " [" + intModel.ToString() + "], AssetID: " + intAsset.ToString() + ", Serial: " + oAsset.Get(intAsset, "serial").ToUpper() + ")";
                                                        oLog.AddEvent(intAnswer, strName, strSerial, "Out of Inventory (DR) ~ GetServerOrBladeAvailableDR(" + intEnv.ToString() + "," + intModel.ToString() + "," + intAsset.ToString() + "," + oAsset.Get(intAsset, "serial").ToUpper() + ")", LoggingType.Error);
                                                    }
                                                    else
                                                        boolDRisOK = true;
                                                }
                                                else
                                                    boolDRisOK = true;

                                                if (boolDRisOK == true)
                                                {
                                                    if (oModel.GetReservationLists(intParent, intClass, intEnv).Tables[0].Rows.Count > 0)
                                                        oServer.AddAsset(intServer, intAsset, intBuildClass, intBuildEnv, 1, 0);
                                                    else
                                                        oServer.AddAsset(intServer, intAsset, intBuildClass, intBuildEnv, intRemovable, 0);
                                                    if (intAssetDR > 0)
                                                    {
                                                        oServer.AddAsset(intServer, intAssetDR, intBuildClass, intEnv, 0, 1);
                                                        oAsset.AddStatus(intAssetDR, "", (int)AssetStatus.InUse, intUser, DateTime.Now);
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            strError = "The selected model does not have a parent model";
                                            oLog.AddEvent(intAnswer, strName, strSerial, strError, LoggingType.Error);
                                        }
                                    }
                                    if (intAsset > 0 && strError == "")
                                    {
                                        oAsset.AddStatus(intAsset, "", (int)AssetStatus.InUse, intUser, DateTime.Now);
                                        string strLocation = oAsset.GetServerOrBlade(intAsset, "CommonName");
                                        if (strLocation == "")
                                            strLocation = oAsset.GetServerOrBlade(intAsset, "Location");
                                        strResult += "Serial Number: " + oAsset.GetServerOrBlade(intAsset, "serial").ToUpper() + "<br/>";
                                        strResult += "Asset Tag: " + oAsset.GetServerOrBlade(intAsset, "asset").ToUpper() + "<br/>";
                                        strResult += "Location: " + strLocation + "<br/>";
                                        if (intAssetDR > 0)
                                        {
                                            strLocation = oAsset.GetServerOrBlade(intAssetDR, "CommonName");
                                            if (strLocation == "")
                                                strLocation = oAsset.GetServerOrBlade(intAssetDR, "Location");
                                            strResult += "Serial Number (DR): " + oAsset.GetServerOrBlade(intAssetDR, "serial").ToUpper() + "<br/>";
                                            strResult += "Asset Tag (DR): " + oAsset.GetServerOrBlade(intAssetDR, "asset").ToUpper() + "<br/>";
                                            strResult += "Location (DR): " + strLocation + "<br/>";
                                        }
                                        strResult += "Model: " + oModelsProperties.Get(intModel, "name").ToUpper() + "<br/>";
                                        strILO = oAsset.GetServerOrBlade(intAsset, "ilo").ToUpper();
                                        if (strILO != "")
                                            strResult += "Remote Management: <a href=\"https://" + strILO + "\" target=\"_blank\">" + strILO + "</a><br/>";
                                    }
                                    AddResult(intServer, intStep, intType, strResult, strError);
                                    break;
                                case 2:     // Server Name 
                                    oLog.AddEvent(intAnswer, strName, strSerial, "Starting Step 2 (Server Name) for asset ID " + intAsset.ToString(), LoggingType.Information);
                                    if (intServerName > 0)
                                    {
                                        strName = oServer.GetName(intServer, boolUsePNCNaming);
                                        strResult = "Server Name: " + strName;
                                        oAsset.AddStatus(intAsset, strName, (int)AssetStatus.InUse, 0, DateTime.Now);
                                        if (intAssetDR > 0)
                                            oAsset.AddStatus(intAssetDR, (strNameDR == "" ? strName + "-DR" : strNameDR), (int)AssetStatus.InUse, 0, DateTime.Now);
                                    }
                                    else
                                    {
                                        string strAdditionalName = "";
                                        string strClusterName = "";
                                        string strInstanceName = "";
                                        string strDTCName = "";
                                        if (boolPNC == true)
                                        {
                                            string _os = oOperatingSystem.Get(intOS, "factory_code");
                                            string _location = oLocation.GetAddress(intAddress, "factory_code");
                                            string _mnemonic = oMnemonic.Get(intMnemonic, "factory_code");
                                            string _environment = oClass.Get(intClass, "factory_code");
                                            // Get Server Function
                                            string _function = "";
                                            if (intSubApplication > 0)
                                                _function = oServerName.GetSubApplication(intSubApplication, "factory_code");
                                            if (_function == "" && intApplication > 0)
                                                _function = oServerName.GetApplication(intApplication, "factory_code");
                                            if (_function == "")
                                            {
                                                foreach (DataRow drComponent in dsComponents.Tables[0].Rows)
                                                {
                                                    _function = drComponent["factory_code"].ToString();
                                                    break;
                                                }
                                            }
                                            if (_function == "")
                                                _function = "A";
                                            // Get Specifics
                                            string _specific = "";
                                            if (intSubApplication > 0)
                                                _specific = oServerName.GetSubApplication(intSubApplication, "factory_code_specific");
                                            if (_specific == "" && intApplication > 0)
                                                _specific = oServerName.GetApplication(intApplication, "factory_code_specific");
                                            if (_specific == "")
                                            {
                                                foreach (DataRow drComponent in dsComponents.Tables[0].Rows)
                                                {
                                                    _specific = drComponent["factory_code_specific"].ToString();
                                                    break;
                                                }
                                            }
                                            // Get Model
                                            if (intParent > 0 && oModel.Get(intParent, "factory_code_specific") != "")
                                                _specific = oModel.Get(intParent, "factory_code_specific");
                                            if (intParent > 0 && oModel.Get(intParent, "factory_code") != "")
                                                _function = oModel.Get(intParent, "factory_code");
                                            // Cluster
                                            if (_specific == "" && oForecast.IsHACluster(intAnswer) == true)
                                                _specific = "Z";
                                            intServerName = oServerName.AddFactory(_os, _location, _mnemonic, _environment, intClass, intEnv, _function, _specific, 0, "VMWARE" + intServer.ToString(), dsnServiceEditor);
                                            strName = oServerName.GetNameFactory(intServerName, 0);
                                        }
                                        else
                                        {
                                            intType = oAsset.GetType(intAsset, dsn);
                                            string strPrefix = "APP";
                                            if (intHost > 0)
                                                strPrefix = oHost.Get(intHost, "prefix");
                                            else if (intSubApplication > 0)
                                                strPrefix = oServerName.GetSubApplication(intSubApplication, "code");
                                            else if (intApplication > 0)
                                                strPrefix = oServerName.GetApplication(intApplication, "code");
                                            if (strPrefix == "APP" && intInfrastructure > 0)
                                                strPrefix = "UTL";
                                            if (strPrefix == "APP")
                                            {
                                                foreach (DataRow drComponent in dsComponents.Tables[0].Rows)
                                                {
                                                    strPrefix = drComponent["code"].ToString();
                                                    break;
                                                }
                                            }
                                            if (intClusterID > 0)
                                            {
                                                DataSet dsNames = oServerName.GetRelated(intAnswer, intClusterID);
                                                if (dsNames.Tables[0].Rows.Count == 0)
                                                {
                                                    DataSet dsCluster = oServer.GetAnswerClusters(intAnswer);
                                                    foreach (DataRow drCluster in dsCluster.Tables[0].Rows)
                                                    {
                                                        int intCluster2 = Int32.Parse(drCluster["clusterid"].ToString());
                                                        if (intCluster2 == intClusterID)
                                                        {
                                                            bool boolSQL = false;
                                                            DataSet dsInstance = oCluster.GetInstances(intCluster2);
                                                            foreach (DataRow drInstance in dsInstance.Tables[0].Rows)
                                                            {
                                                                string strPrefix2 = strPrefix;
                                                                if (drInstance["sql"].ToString() == "1")
                                                                {
                                                                    boolSQL = true;
                                                                    strPrefix2 = "SQL";
                                                                }
                                                                strPrefix2 = "C" + strPrefix2.Substring(0, 2);
                                                                strPrefix2 = strPrefix2.ToUpper().Trim();
                                                                int intClusterName = oServerName.Add(intClass, intEnv, intAddress, strPrefix2, 0, oCluster.Get(intCluster2, "name"), 1, dsnServiceEditor);
                                                                oServerName.AddRelated(intAnswer, intCluster2, intClusterName);
                                                                if (strInstanceName != "")
                                                                    strInstanceName += ", ";
                                                                strInstanceName += oServerName.GetName(intClusterName, 0);
                                                            }
                                                            int intClusterNameCLU = oServerName.Add(intClass, intEnv, intAddress, "CLU", 0, oCluster.Get(intCluster2, "name"), 1, dsnServiceEditor);
                                                            oServerName.AddRelated(intAnswer, intCluster2, intClusterNameCLU);
                                                            if (strClusterName != "")
                                                                strClusterName += ", ";
                                                            strClusterName += oServerName.GetName(intClusterNameCLU, 0);
                                                            if (boolSQL == true)
                                                            {
                                                                int intDTCName = oServerName.Add(intClass, intEnv, intAddress, "DTC", 0, oCluster.Get(intCluster2, "name"), 1, dsnServiceEditor);
                                                                oServerName.AddRelated(intAnswer, intCluster2, intDTCName);
                                                                if (strDTCName != "")
                                                                    strDTCName += ", ";
                                                                strDTCName += oServerName.GetName(intDTCName, 0);
                                                            }
                                                            break;
                                                        }
                                                    }
                                                }
                                                strPrefix = "N" + strPrefix.Substring(0, 2);
                                            }
                                            if (oOperatingSystem.IsMidrange(intOS) == true)
                                                strPrefix = "X" + strPrefix.Substring(0, 2);
                                            strPrefix = strPrefix.ToUpper().Trim();
                                            intServerName = oServerName.Add(intClass, intEnv, intAddress, strPrefix, 0, oAsset.GetServerOrBlade(intAsset, "serial").ToUpper(), 1, dsnServiceEditor);
                                            strName = oServerName.GetName(intServerName, 0);
                                        }
                                        if (intServerName > 0)
                                        {
                                            strResult = "Server Name: " + strName;
                                            oLog.AddEvent(intAnswer, strName, strSerial, "Asset ID " + intAsset.ToString() + ": Server name = " + strName, LoggingType.Information);
                                            if (strInstanceName != "")
                                                strResult += "<br/>" + "Instance Name(s): " + strInstanceName;
                                            if (strClusterName != "")
                                                strResult += "<br/>" + "Cluster Name(s): " + strClusterName;
                                            if (strDTCName != "")
                                                strResult += "<br/>" + "DTC Name(s): " + strDTCName;
                                            if (strAdditionalName != "")
                                                strResult += "<br/>" + "Additional Name(s): " + strAdditionalName;
                                            oAsset.AddStatus(intAsset, strName, (int)AssetStatus.InUse, 0, DateTime.Now);
                                            if (intAssetDR > 0)
                                                oAsset.AddStatus(intAssetDR, (strNameDR == "" ? strName + "-DR" : strNameDR), (int)AssetStatus.InUse, 0, DateTime.Now);
                                            oServer.UpdateServerNamed(intServer, intServerName);
                                        }
                                        else
                                        {
                                            strError = "All available server names are in use for the criteria specified ~ please report this problem to your ClearView administrator.";
                                            oLog.AddEvent(intAnswer, strName, strSerial, strError, LoggingType.Error);
                                        }
                                    }
                                    AddResult(intServer, intStep, intType, strResult, strError);
                                    break;
                                case 3:     // IP Address 
                                    oLog.AddEvent(intAnswer, strName, strSerial, "Starting Step 3 (IP Address)", LoggingType.Information);
                                    int intVLAN = 0;
                                    if (oAsset.GetServerOrBlade(intAsset, "vlan") != "")
                                        intVLAN = Int32.Parse(oAsset.GetServerOrBlade(intAsset, "vlan"));
                                    if (intIPAddressBuild1 > 0 || intIPAddressFinal1 > 0)
                                    {
                                        if (intIPAddressBuild1 == intIPAddressFinal1)
                                            strResult = "IP Address: " + oIPAddresses.GetName(intIPAddressBuild1, 0);
                                        else if (intIPAddressBuild1 > 0)
                                            strResult = "IP Address (Build): " + oIPAddresses.GetName(intIPAddressBuild1, 0);
                                        else
                                            strResult = "IP Address (Final): " + oIPAddresses.GetName(intIPAddressFinal1, 0);
                                        if (intIPAddressBuild2 > 0 || intIPAddressFinal2 > 0)
                                        {
                                            if (intIPAddressBuild2 == intIPAddressFinal2)
                                                strResult += "<br/>IP Address: " + oIPAddresses.GetName(intIPAddressBuild2, 0);
                                            else if (intIPAddressBuild2 > 0)
                                                strResult += "<br/>IP Address (Build): " + oIPAddresses.GetName(intIPAddressBuild2, 0);
                                            else
                                                strResult += "<br/>IP Address (Final): " + oIPAddresses.GetName(intIPAddressFinal2, 0);
                                        }
                                        if (intIPAddressBuild3 > 0 || intIPAddressFinal3 > 0)
                                        {
                                            if (intIPAddressBuild3 == intIPAddressFinal3)
                                                strResult += "<br/>IP Address: " + oIPAddresses.GetName(intIPAddressBuild3, 0);
                                            else if (intIPAddressBuild3 > 0)
                                                strResult += "<br/>IP Address (Build): " + oIPAddresses.GetName(intIPAddressBuild3, 0);
                                            else
                                                strResult += "<br/>IP Address (Final): " + oIPAddresses.GetName(intIPAddressFinal3, 0);
                                        }
                                    }
                                    else if (intVLAN > 0)
                                    {
                                        oLog.AddEvent(intAnswer, strName, strSerial, "There are " + dsBuild.Tables[0].Rows.Count.ToString() + " build locations for class: " + oClass.Get(intClass, "name") + ", env: " + oEnvironment.Get(intEnv, "name") + ", address: " + oLocation.GetFull(intAddress) + ", model: " + oModel.Get(intParent, "name") + " for server ID " + intServer.ToString(), LoggingType.Information);
                                        if (boolOffsite == true)
                                        {
                                            oLog.AddEvent(intAnswer, strName, strSerial, "Offsite build location : " + oClass.Get(intClass, "name") + ", env: " + oEnvironment.Get(intEnv, "name") + ", address: " + oLocation.GetFull(intAddress) + ", model: " + oModel.Get(intParent, "name"), LoggingType.Information);
                                            intBuildClass = Int32.Parse(oSetting.Get("offsite_build_class"));
                                            intBuildEnv = Int32.Parse(oSetting.Get("offsite_build_environment"));
                                            intBuildAddress = Int32.Parse(oSetting.Get("offsite_build_location"));
                                        }
                                        else if (dsBuild.Tables[0].Rows.Count > 0)
                                        {
                                            oLog.AddEvent(intAnswer, strName, strSerial, "Build location found : " + oClass.Get(intBuildClass, "name") + ", env: " + oEnvironment.Get(intBuildEnv, "name") + ", address: " + oLocation.GetFull(intBuildAddress) + ", model: " + oModel.Get(intParent, "name"), LoggingType.Information);
                                            intBuildClass = Int32.Parse(dsBuild.Tables[0].Rows[0]["build_classid"].ToString());
                                            intBuildEnv = Int32.Parse(dsBuild.Tables[0].Rows[0]["build_environmentid"].ToString());
                                            intBuildAddress = Int32.Parse(dsBuild.Tables[0].Rows[0]["build_addressid"].ToString());
                                        }
                                        else
                                        {
                                            oLog.AddEvent(intAnswer, strName, strSerial, "No Build location : " + oClass.Get(intClass, "name") + ", env: " + oEnvironment.Get(intEnv, "name") + ", address: " + oLocation.GetFull(intAddress) + ", model: " + oModel.Get(intParent, "name"), LoggingType.Debug);
                                            intBuildClass = intClass;
                                            intBuildEnv = intEnv;
                                            intBuildAddress = intAddress;
                                        }


                                        // Get Components
                                        bool boolWeb = false;
                                        int intDellWeb = 0;
                                        int intDellMiddleware = 0;
                                        int intDellDatabase = 0;
                                        int intDellFile = 0;
                                        int intDellMisc = 0;
                                        int intDellUnder48 = 0;
                                        if (boolDell == true || (oOperatingSystem.IsSolaris(intOS) == true && boolIsNexusSwitch == true))
                                        {
                                            // Might need to decide at some point which one takes precedence (if both database and web are selected)
                                            foreach (DataRow drComponent in dsComponents.Tables[0].Rows)
                                            {
                                                if (drComponent["sql"].ToString() == "1" || drComponent["dbase"].ToString() == "1")
                                                    intDellDatabase = 1;
                                                if (drComponent["iis"].ToString() == "1" || drComponent["web"].ToString() == "1")
                                                {
                                                    intDellWeb = 1;
                                                    boolWeb = true;
                                                }
                                            }
                                            intDellUnder48 = (oForecast.IsDRUnder48(intAnswer, false) ? 1 : 0);
                                            if (intDellDatabase == 0 && intDellWeb == 0)
                                                intDellMisc = 1;
                                        }
                                        else
                                        {
                                            foreach (DataRow drComponent in dsComponents.Tables[0].Rows)
                                            {
                                                if (drComponent["iis"].ToString() == "1" || drComponent["web"].ToString() == "1")
                                                    boolWeb = true;
                                            }
                                        }

                                        // Check Load Balancing
                                        int intLTM_Web = 0;
                                        int intLTM_App = 0;
                                        int intLTM_Middleware = 0;
                                        if (oForecast.IsHACSM(intAnswer) == true)
                                        {
                                            intLTM_Middleware = (oForecast.IsHACSMMiddleware(intAnswer) ? 1 : 0);
                                            // If Yes, then we know it's a Middleware assignment.
                                            if (intLTM_Middleware == 0)
                                            {
                                                // If No, we need to continue....
                                                if (boolWeb == false && oForecast.IsHACSMWeb(intAnswer) == false)
                                                {
                                                    // If a web component is NOT selected, we know its an App Tier assignment.
                                                    intLTM_App = 1;
                                                }
                                                else
                                                {
                                                    // If a web component is selected (IIS, Apache, etc...), prompt the user...
                                                    if (oForecast.IsHACSMApp(intAnswer) == true)
                                                    {
                                                        // If Yes, then we know it's a Web + App Tier assignment
                                                        intLTM_App = 1;
                                                        intLTM_Web = 1;
                                                    }
                                                    else
                                                    {
                                                        // If No, then we know it's a Web Tier assignment.
                                                        intLTM_Web = 1;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                intDellDatabase = 0;
                                                intDellWeb = 0;
                                                intDellMisc = 0;
                                            }
                                        }


                                        if (boolPNC == true)
                                        {
                                            oLog.AddEvent(intAnswer, strName, strSerial, "PNC IP Addressing", LoggingType.Debug);
                                            bool boolAssign = false;
                                            if (oClass.IsProd(intClass) || oClass.IsQA(intClass))
                                            {
                                                // Change: 7/2/09: include PROD IP Addressing
                                                //boolAssign = false;
                                                boolAssign = true;
                                            }
                                            else
                                            {
                                                // PNC - Non Production
                                                boolAssign = true;
                                            }
                                            string strIP_HA = "";
                                            if (boolAssign == true)
                                            {
                                                oLog.AddEvent(intAnswer, strName, strSerial, "IsNexusSwitch = " + (boolIsNexusSwitch ? "Yes" : "No") + " (" + dsSwitch.Tables[0].Rows.Count.ToString() + ")", LoggingType.Debug);
                                                if (oOperatingSystem.IsWindows(intOS) == true || oOperatingSystem.IsWindows2008(intOS) == true || oOperatingSystem.IsLinux(intOS) == true || boolIsNexusSwitch)
                                                {
                                                    string strOSShort = "WINDOWS";
                                                    if (oOperatingSystem.IsWindows(intOS) == true || oOperatingSystem.IsWindows2008(intOS) == true || oOperatingSystem.IsLinux(intOS) == true)
                                                    {
                                                        if (oOperatingSystem.IsLinux(intOS) == true)
                                                            strOSShort = "LINUX";

                                                        int intClusterNetworkID = 0;
                                                        if (intClusterID > 0
                                                            && (oOperatingSystem.IsWindows(intOS) == true || oOperatingSystem.IsWindows2008(intOS) == true)
                                                            && oSetting.Get("configure_cluster") == "1")
                                                        {
                                                            // Node of a cluster - nodes must all be on the same subnet (CVT123425).  
                                                            Int32.TryParse(oCluster.Get(intClusterID, "networkid"), out intClusterNetworkID);
                                                        }

                                                        if (intClusterNetworkID > 0)
                                                        {
                                                            // Assign based on network of cluster (was assigned when another node was configured).
                                                            string strIPAddressAssign = "(Class = " + intBuildClass.ToString() + ", Env = " + intBuildEnv.ToString() + ", Address = " + intBuildAddress.ToString() + ", NetworkID = " + intClusterNetworkID.ToString() + ", Subnet: " + oIPAddresses.GetNetwork(intClusterNetworkID, "gateway") + ")";
                                                            oLog.AddEvent(intAnswer, strName, strSerial, "Reserving cluster node IP address ~ " + strIPAddressAssign, LoggingType.Information);
                                                            intIPAddressBuild1 = oIPAddresses.Get_Network(intBuildClass, intBuildEnv, intBuildAddress, 0, 0, intClusterNetworkID, true, intEnvironment, dsnServiceEditor);
                                                        }
                                                        else
                                                        {
                                                            // Windows or Linux IP Address
                                                            oLog.AddEvent(intAnswer, strName, strSerial, "OS = " + strOSShort, LoggingType.Debug);
                                                            oLog.AddEvent(intAnswer, strName, strSerial, "Blade = " + (boolBlade ? "Yes" : "No"), LoggingType.Debug);
                                                            oLog.AddEvent(intAnswer, strName, strSerial, "Dell = " + (boolDell ? "Yes" : "No"), LoggingType.Debug);
                                                            if (boolBlade == true)
                                                            {
                                                                if (boolDell == false)
                                                                    oLog.AddEvent(intAnswer, strName, strSerial, "Assigning IP Address for " + strOSShort + " BLADE (Class = " + intBuildClass.ToString() + ", Env = " + intBuildEnv.ToString() + ", Address = " + intBuildAddress.ToString() + ", VLAN = " + intVLAN.ToString() + ", HA = " + (intHA == 1 ? "Yes" : "No") + ")", LoggingType.Information);
                                                                if (intHA == 1)
                                                                {
                                                                    DataSet dsHA = oIPAddresses.GetVlanHAs(intVLAN);
                                                                    for (int ii = 0; ii < dsHA.Tables[0].Rows.Count && intIPAddressBuild1 == 0; ii++)
                                                                    {
                                                                        oLog.AddEvent(intAnswer, strName, strSerial, "Assigning [HA] IP Address for " + strOSShort + " BLADE (Class = " + intBuildClass.ToString() + ", Env = " + intBuildEnv.ToString() + ", Address = " + intBuildAddress.ToString() + ", HA VLAN = " + dsHA.Tables[0].Rows[ii]["ha_vlan"].ToString() + ", Resiliency: " + oResiliency.Get(intResiliency, "name") + ")", LoggingType.Information);
                                                                        if (strIP_HA != "")
                                                                            strIP_HA += ", ";
                                                                        strIP_HA += dsHA.Tables[0].Rows[ii]["ha_vlan"].ToString();
                                                                        intIPAddressBuild1 = oIPAddresses.Get_Blade_HP(intBuildClass, intBuildEnv, intBuildAddress, 0, 0, Int32.Parse(dsHA.Tables[0].Rows[ii]["ha_vlan"].ToString()), 0, 1, true, intServer, intResiliency, intEnvironment, dsnServiceEditor);
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    if (boolDell == true)
                                                                    {
                                                                        oLog.AddEvent(intAnswer, strName, strSerial, "Assigning IP Address for Dell BLADE (Class = " + intBuildClass.ToString() + ", Env = " + intBuildEnv.ToString() + ", Address = " + intBuildAddress.ToString() + ", LTM (Web) = " + intLTM_Web.ToString() + ", LTM (App) = " + intLTM_App.ToString() + ", LTM (Middleware) = " + intLTM_Middleware.ToString() + ", Web = " + intDellWeb.ToString() + ", Middleware = " + intDellMiddleware.ToString() + ", Database = " + intDellDatabase.ToString() + ", File = " + intDellFile.ToString() + ", Misc = " + intDellMisc.ToString() + ", Under48 = " + intDellUnder48.ToString() + ", Resiliency: " + oResiliency.Get(intResiliency, "name") + ", Avamar = 0)", LoggingType.Information);
                                                                        intIPAddressBuild1 = oIPAddresses.Get_Dell(intBuildClass, intBuildEnv, intBuildAddress, 0, 0, intLTM_Web, intLTM_App, intLTM_Middleware, 0, 0, intDellWeb, intDellMiddleware, intDellDatabase, intDellFile, intDellMisc, intDellUnder48, 0, true, intServer, 0, intResiliency, intEnvironment, dsnServiceEditor);
                                                                    }
                                                                    else
                                                                        intIPAddressBuild1 = oIPAddresses.Get_Blade_HP(intBuildClass, intBuildEnv, intBuildAddress, 0, 0, intVLAN, 0, 0, true, intServer, intResiliency, intEnvironment, dsnServiceEditor);
                                                                }
                                                            }
                                                            else
                                                            {
                                                                if (boolDell == false)
                                                                    oLog.AddEvent(intAnswer, strName, strSerial, "Assigning IP Address for " + strOSShort + " PHYSICAL (Class = " + intBuildClass.ToString() + ", Env = " + intBuildEnv.ToString() + ", Address = " + intBuildAddress.ToString() + ", VLAN = " + intVLAN.ToString() + ", HA = " + (intHA == 1 ? "Yes" : "No") + ")", LoggingType.Information);
                                                                if (intHA == 1)
                                                                {
                                                                    DataSet dsHA = oIPAddresses.GetVlanHAs(intVLAN);
                                                                    for (int ii = 0; ii < dsHA.Tables[0].Rows.Count && intIPAddressBuild1 == 0; ii++)
                                                                    {
                                                                        oLog.AddEvent(intAnswer, strName, strSerial, "Assigning [HA] IP Address for " + strOSShort + " PHYSICAL (Class = " + intBuildClass.ToString() + ", Env = " + intBuildEnv.ToString() + ", Address = " + intBuildAddress.ToString() + ", HA VLAN = " + dsHA.Tables[0].Rows[ii]["ha_vlan"].ToString() + ")", LoggingType.Information);
                                                                        if (strIP_HA != "")
                                                                            strIP_HA += ", ";
                                                                        strIP_HA += dsHA.Tables[0].Rows[ii]["ha_vlan"].ToString();
                                                                        intIPAddressBuild1 = oIPAddresses.Get_VLAN(intBuildClass, intBuildEnv, intBuildAddress, 0, 0, Int32.Parse(dsHA.Tables[0].Rows[ii]["ha_vlan"].ToString()), 0, 0, 0, 0, 0, 0, 1, true, intServer, 0, 0, intResiliency, intEnvironment, dsnServiceEditor);
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    if (boolDell == true)
                                                                    {
                                                                        oLog.AddEvent(intAnswer, strName, strSerial, "Assigning IP Address for Dell RACKMOUNT (Class = " + intBuildClass.ToString() + ", Env = " + intBuildEnv.ToString() + ", Address = " + intBuildAddress.ToString() + ", LTM (Web) = " + intLTM_Web.ToString() + ", LTM (App) = " + intLTM_App.ToString() + ", LTM (Middleware) = " + intLTM_Middleware.ToString() + ", Web = " + intDellWeb.ToString() + ", Middleware = " + intDellMiddleware.ToString() + ", Database = " + intDellDatabase.ToString() + ", File = " + intDellFile.ToString() + ", Misc = " + intDellMisc.ToString() + ", Under48 = " + intDellUnder48.ToString() + ", Resiliency: " + oResiliency.Get(intResiliency, "name") + ", Avamar = 0)", LoggingType.Information);
                                                                        intIPAddressBuild1 = oIPAddresses.Get_Dell(intBuildClass, intBuildEnv, intBuildAddress, 0, 0, intLTM_Web, intLTM_App, intLTM_Middleware, 0, 0, intDellWeb, intDellMiddleware, intDellDatabase, intDellFile, intDellMisc, intDellUnder48, 0, true, intServer, 0, intResiliency, intEnvironment, dsnServiceEditor);
                                                                    }
                                                                    else
                                                                        intIPAddressBuild1 = oIPAddresses.Get_VLAN(intBuildClass, intBuildEnv, intBuildAddress, 0, 0, intVLAN, 0, 0, 0, 0, 0, 0, 0, true, intServer, 0, 0, intResiliency, intEnvironment, dsnServiceEditor);
                                                                }
                                                            }
                                                        }
                                                        // Log IP Address Query
                                                        oServer.AddOutput(intServer, "IP_ASSIGN", oIPAddresses.Results());
                                                        oIPAddresses.ClearResults();

                                                        if (intIPAddressBuild1 > 0)
                                                        {
                                                            //if (intTSM == 0)
                                                            //{
                                                            //    // Need to assign an additional IP address for the BACKUP NIC
                                                            //    oLog.AddEvent(intAnswer, strName, strSerial, "Assigning IP Address (Backup) for (Class = " + intBuildClass.ToString() + ", Env = " + intBuildEnv.ToString() + ", Address = " + intBuildAddress.ToString() + ", Type = Dell Blade, Web = 0, Middleware = 0, Database = 0, File = 0, Misc = 0, Under48 = 0, Avamar = 1, ServerID = " + intServer.ToString() + ", Resiliency: " + oResiliency.Get(intResiliency, "name") + ")", LoggingType.Information);
                                                            //    intIPAddressBackup = oIPAddresses.Get_Dell(intBuildClass, intBuildEnv, intBuildAddress, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, true, intServer, 0, intResiliency, intEnvironment, dsnServiceEditor);
                                                            //    // Log IP Address Query
                                                            //    oServer.AddOutput(intServer, "IP_ASSIGN_BACKUP", oIPAddresses.Results());
                                                            //    oIPAddresses.ClearResults();

                                                            //    if (intIPAddressBackup == 0)
                                                            //    {
                                                            //        oIPAddresses.UpdateAvailable(intIPAddressBuild1, 1);    // Clear the assigned IP address
                                                            //        strError = "All available IP addresses are in use for the criteria specified ~ Backup";
                                                            //        oLog.AddEvent(intAnswer, strName, strSerial, strError, LoggingType.Error);
                                                            //    }
                                                            //}


                                                            // Assign Private Cluster Address
                                                            if (strError == "" && intClusterID > 0)
                                                            {
                                                                int intNetworkPrivate = intNetworkHA;
                                                                if (intNetworkPrivate == 0)
                                                                {
                                                                    oLog.AddEvent(intAnswer, strName, strSerial, "Assigning Private Network for (Class = " + intBuildClass.ToString() + ", Env = " + intBuildEnv.ToString() + ", Address = " + intBuildAddress.ToString() + ", Type = Windows Cluster)", LoggingType.Information);
                                                                    intNetworkPrivate = oIPAddresses.Get_ClusterNetwork(intBuildClass, intBuildEnv, intBuildAddress, 0, 0, 1, 0, intQuantity, 0, true, dsnServiceEditor);
                                                                    if (intNetworkPrivate > 0)
                                                                    {
                                                                        oForecast.UpdateAnswerHA(intAnswer, intNetworkPrivate);
                                                                        oIPAddresses.UpdateNetworkCluster(intNetworkPrivate, 1);
                                                                    }
                                                                    // Log IP Address Query
                                                                    oServer.AddOutput(intServer, "IP_ASSIGN_PRIVATE_NETWORK", oIPAddresses.Results());
                                                                    oIPAddresses.ClearResults();
                                                                }
                                                                if (intNetworkPrivate > 0)
                                                                {
                                                                    string strNetworkPrivate = oIPAddresses.GetNetwork(intNetworkPrivate, "add1") + "." + oIPAddresses.GetNetwork(intNetworkPrivate, "add2") + "." + oIPAddresses.GetNetwork(intNetworkPrivate, "add3") + ".";
                                                                    oLog.AddEvent(intAnswer, strName, strSerial, "Network Found (" + strNetworkPrivate + oIPAddresses.GetNetwork(intNetworkPrivate, "min4") + " - " + strNetworkPrivate + oIPAddresses.GetNetwork(intNetworkPrivate, "max4") + ". Assigning Private IP Address (Cluster) for (Class = " + intBuildClass.ToString() + ", Env = " + intBuildEnv.ToString() + ", Address = " + intBuildAddress.ToString() + ", Type = Windows Cluster)", LoggingType.Information);
                                                                    intIPAddressCluster = oIPAddresses.Get_Cluster(intBuildClass, intBuildEnv, intBuildAddress, 0, 0, 1, 0, intNetworkPrivate, true, intEnvironment, dsnServiceEditor);
                                                                    // Log IP Address Query
                                                                    oServer.AddOutput(intServer, "IP_ASSIGN_PRIVATE", oIPAddresses.Results());
                                                                    oIPAddresses.ClearResults();

                                                                    if (intIPAddressCluster == 0)
                                                                    {
                                                                        oIPAddresses.UpdateAvailable(intIPAddressBuild1, 1);    // Clear the assigned IP address
                                                                        if (intIPAddressBackup > 0)
                                                                            oIPAddresses.UpdateAvailable(intIPAddressBackup, 1);    // Clear the assigned IP address
                                                                        strError = "All available IP addresses are in use for the criteria specified ~ Private Cluster";
                                                                        oLog.AddEvent(intAnswer, strName, strSerial, strError, LoggingType.Error);
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    oIPAddresses.UpdateAvailable(intIPAddressBuild1, 1);    // Clear the assigned IP address
                                                                    if (intIPAddressBackup > 0)
                                                                        oIPAddresses.UpdateAvailable(intIPAddressBackup, 1);    // Clear the assigned IP address
                                                                    strError = "All available IP address networks are in use for the criteria specified";
                                                                    oLog.AddEvent(intAnswer, strName, strSerial, strError, LoggingType.Error);
                                                                }
                                                            }

                                                            // Check Clustering and Assign (if applicable)
                                                            if (strError == "")
                                                            {
                                                                if (intClusterID > 0
                                                                    && (oOperatingSystem.IsWindows(intOS) == true || oOperatingSystem.IsWindows2008(intOS) == true))
                                                                {
                                                                    if (oSetting.Get("configure_cluster") == "1")
                                                                    {
                                                                        // The IP address assigned either contains the newly assigned network, or the previously assigned "intClusterNetworkID"
                                                                        intNetwork = Int32.Parse(oIPAddresses.Get(intIPAddressBuild1, "networkid"));
                                                                        oLog.AddEvent(intAnswer, strName, strSerial, "Starting cluster IP address assignments using  NetworkID = " + intNetwork.ToString() + ", Subnet: " + oIPAddresses.GetNetwork(intNetwork, "gateway"), LoggingType.Information);
                                                                        List<int> ClusterInstanceIDs = new List<int>();
                                                                        List<int> ClusterIPsAssigned = new List<int>();
                                                                        List<int> ClusterIPsAlready = new List<int>();
                                                                        string strIPAddressAssignInstance = "(Class = " + intBuildClass.ToString() + ", Env = " + intBuildEnv.ToString() + ", Address = " + intBuildAddress.ToString() + ", NetworkID = " + intNetwork.ToString() + ", Subnet: " + oIPAddresses.GetNetwork(intNetwork, "gateway") + ")";

                                                                        DataSet dsInstances = oCluster.GetInstances(intClusterID);
                                                                        for (int ii = 0; ii < dsInstances.Tables[0].Rows.Count; ii++)
                                                                        {
                                                                            int intInstance = Int32.Parse(dsInstances.Tables[0].Rows[ii]["id"].ToString());
                                                                            int intInstanceIP = 0;
                                                                            Int32.TryParse(dsInstances.Tables[0].Rows[ii]["ipaddressid"].ToString(), out intInstanceIP);
                                                                            if (intInstanceIP == 0)
                                                                            {
                                                                                // Instance not been assigned an IP address.
                                                                                oLog.AddEvent(intAnswer, strName, strSerial, "Reserving cluster instance IP address ~ " + strIPAddressAssignInstance, LoggingType.Information);
                                                                                intInstanceIP = oIPAddresses.Get_Network(intBuildClass, intBuildEnv, intBuildAddress, 0, 0, intNetwork, true, intEnvironment, dsnServiceEditor);
                                                                                // Log IP Address Query
                                                                                oServer.AddOutput(intServer, "IP_ASSIGN_INSTANCE_" + ii.ToString(), oIPAddresses.Results());
                                                                                oIPAddresses.ClearResults();
                                                                                if (intInstanceIP > 0)
                                                                                {
                                                                                    // Add to temporary list
                                                                                    ClusterInstanceIDs.Add(intInstance);
                                                                                    ClusterIPsAssigned.Add(intInstanceIP);
                                                                                    oLog.AddEvent(intAnswer, strName, strSerial, "Found cluster instance IP address = " + oIPAddresses.GetName(intInstanceIP), LoggingType.Information);
                                                                                }
                                                                                else
                                                                                    break;
                                                                            }
                                                                            else
                                                                            {
                                                                                oLog.AddEvent(intAnswer, strName, strSerial, "Instance # " + ii.ToString() + " was already assigned IP address = " + oIPAddresses.GetName(intInstanceIP), LoggingType.Debug);
                                                                                ClusterIPsAlready.Add(intInstanceIP);
                                                                            }
                                                                        }

                                                                        // Will arriver here only if all IPs were successfully assigned, were already assigned, or could not be assigned
                                                                        if (ClusterIPsAlready.Count == dsInstances.Tables[0].Rows.Count)
                                                                        {
                                                                            // All already assigned.
                                                                            oLog.AddEvent(intAnswer, strName, strSerial, "All cluster instance IP addresses have already been assigned", LoggingType.Debug);
                                                                        }
                                                                        else if ((ClusterIPsAssigned.Count + ClusterIPsAlready.Count) == dsInstances.Tables[0].Rows.Count)
                                                                        {
                                                                            // Some new ones have been assigned.
                                                                            oLog.AddEvent(intAnswer, strName, strSerial, ClusterIPsAssigned.Count.ToString() + " new cluster instance IP addresses have been reserved", LoggingType.Debug);
                                                                        }
                                                                        else
                                                                        {
                                                                            // Not enough IPs where found.
                                                                            strError = "All potential cluster instance IP addresses are in use for the criteria specified ~ " + strIPAddressAssignInstance;
                                                                            oLog.AddEvent(intAnswer, strName, strSerial, strError, LoggingType.Error);
                                                                        }

                                                                        if (strError == "")
                                                                        {
                                                                            // Since we have all IPs, assign the cluster an IP address (if not already assigned)
                                                                            int intClusterIP = 0;
                                                                            Int32.TryParse(oCluster.Get(intClusterID, "ipaddressid"), out intClusterIP);
                                                                            if (intClusterIP == 0)
                                                                            {
                                                                                // Not assigned, assign one.
                                                                                oLog.AddEvent(intAnswer, strName, strSerial, "Reserving cluster IP address ~ " + strIPAddressAssignInstance, LoggingType.Information);
                                                                                intClusterIP = oIPAddresses.Get_Network(intBuildClass, intBuildEnv, intBuildAddress, 0, 0, intNetwork, true, intEnvironment, dsnServiceEditor);
                                                                                // Log IP Address Query
                                                                                oServer.AddOutput(intServer, "IP_ASSIGN_CLUSTER", oIPAddresses.Results());
                                                                                oIPAddresses.ClearResults();
                                                                                if (intClusterIP > 0)
                                                                                    oLog.AddEvent(intAnswer, strName, strSerial, "Found cluster IP address = " + oIPAddresses.GetName(intClusterIP), LoggingType.Information);
                                                                            }
                                                                            if (intClusterIP > 0)
                                                                            {
                                                                                // Associate all cluster related IPs now.
                                                                                // Add recently assigned instance addresses (may be none)
                                                                                for (int ii = 0; ii < ClusterIPsAssigned.Count; ii++)
                                                                                    oCluster.UpdateInstanceIP(ClusterInstanceIDs[ii], ClusterIPsAssigned[ii]);
                                                                                // Update IP address and network for cluster.
                                                                                oCluster.UpdateIP(intClusterID, intClusterIP);
                                                                                oCluster.UpdateNetwork(intClusterID, intNetwork);
                                                                                oLog.AddEvent(intAnswer, strName, strSerial, "All cluster IP addresses have been assigned", LoggingType.Information);
                                                                            }
                                                                            else
                                                                            {
                                                                                strError = "All potential cluster IP addresses are in use for the criteria specified ~ " + strIPAddressAssignInstance;
                                                                                oLog.AddEvent(intAnswer, strName, strSerial, strError, LoggingType.Error);
                                                                            }
                                                                        }

                                                                        if (strError != "")
                                                                        {
                                                                            // Error - Release the ones that were marked reserved.
                                                                            for (int ii = 0; ii < ClusterIPsAssigned.Count; ii++)
                                                                                oIPAddresses.UpdateAvailable(ClusterIPsAssigned[ii], 1);
                                                                            // Don't worry about intClusterIP since that's the last one to get assigned and if it errors here, it's because it wasn't able to be assigned.
                                                                            oLog.AddEvent(intAnswer, strName, strSerial, "All reserved cluster IP addresses have been released", LoggingType.Information);
                                                                        }
                                                                    }
                                                                    else
                                                                        oLog.AddEvent(intAnswer, strName, strSerial, "Skipping Cluster IP Configuration", LoggingType.Information);
                                                                }
                                                            }

                                                            if (strError == "")
                                                            {
                                                                if (intIPAddressBackup > 0)
                                                                {
                                                                    oServer.AddIP(intServer, intIPAddressBackup, 0, 0, 0, 1);
                                                                    strResult += "IP Address (Backup): " + oIPAddresses.GetName(intIPAddressBackup, 0) + "<br/>";
                                                                }
                                                                if (intIPAddressCluster > 0)
                                                                {
                                                                    oServer.AddIP(intServer, intIPAddressCluster, 0, 0, 1, 0);
                                                                    strResult += "IP Address (Private): " + oIPAddresses.GetName(intIPAddressCluster, 0) + "<br/>";
                                                                }
                                                                strResult += "IP Address: " + oIPAddresses.GetName(intIPAddressBuild1, 0);
                                                                oLog.AddEvent(intAnswer, strName, strSerial, "IP address = " + oIPAddresses.GetName(intIPAddressBuild1, 0), LoggingType.Information);
                                                                oServer.AddIP(intServer, intIPAddressBuild1, (boolOKtoAssignIP ? 1 : 0), 1, 0, 0);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            strError = "All available IP addresses are in use for the criteria specified ~ (#2) please report this problem to your ClearView administrator ~ (" + (boolBlade ? "Blade" : "Physical") + ": VLAN = " + (strIP_HA == "" ? intVLAN.ToString() : strIP_HA + " [HA]") + ")";
                                                        }
                                                    }
                                                    else
                                                    {
                                                        // Solaris IP Addresses on Nexus
                                                        oLog.AddEvent(intAnswer, strName, strSerial, "OS = Solaris (Nexus)", LoggingType.Information);
                                                        while (intIPAddressBuild1 == 0 && intIPAddressBuild2 == 0 && intIPAddressBuild3 == 0)
                                                        {
                                                            oLog.AddEvent(intAnswer, strName, strSerial, "Assigning IP Address # 1 for SUN on DELL (Class = " + intBuildClass.ToString() + ", Env = " + intBuildEnv.ToString() + ", Address = " + intBuildAddress.ToString() + ", LTM (Web) = " + intLTM_Web.ToString() + ", LTM (App) = " + intLTM_App.ToString() + ", LTM (Middleware) = " + intLTM_Middleware.ToString() + ", Web = " + intDellWeb.ToString() + ", Middleware = " + intDellMiddleware.ToString() + ", Database = " + intDellDatabase.ToString() + ", File = " + intDellFile.ToString() + ", Misc = " + intDellMisc.ToString() + ", Under48 = " + intDellUnder48.ToString() + ", Resiliency: " + oResiliency.Get(intResiliency, "name") + ", Avamar = 0)", LoggingType.Information);
                                                            intIPAddressBuild1 = oIPAddresses.Get_Dell(intBuildClass, intBuildEnv, intBuildAddress, 0, 0, intLTM_Web, intLTM_App, intLTM_Middleware, 0, 0, intDellWeb, intDellMiddleware, intDellDatabase, intDellFile, intDellMisc, intDellUnder48, 0, true, intServer, 0, intResiliency, intEnvironment, dsnServiceEditor);
                                                            oServer.AddOutput(intServer, "IP_ASSIGN_1", oIPAddresses.Results());
                                                            oIPAddresses.ClearResults();
                                                            if (intIPAddressBuild1 == 0)
                                                                break;
                                                            int intIPAddressVlan1 = oIPAddresses.GetNetworkVlan(oIPAddresses.GetAddressNetwork(intIPAddressBuild1));

                                                            oLog.AddEvent(intAnswer, strName, strSerial, "Assigning IP Address # 2 for SUN on DELL (Class = " + intBuildClass.ToString() + ", Env = " + intBuildEnv.ToString() + ", Address = " + intBuildAddress.ToString() + ", LTM (Web) = " + intLTM_Web.ToString() + ", LTM (App) = " + intLTM_App.ToString() + ", LTM (Middleware) = " + intLTM_Middleware.ToString() + ", Web = " + intDellWeb.ToString() + ", Middleware = " + intDellMiddleware.ToString() + ", Database = " + intDellDatabase.ToString() + ", File = " + intDellFile.ToString() + ", Misc = " + intDellMisc.ToString() + ", Under48 = " + intDellUnder48.ToString() + ", Resiliency: " + oResiliency.Get(intResiliency, "name") + ", Avamar = 0)", LoggingType.Information);
                                                            intIPAddressBuild2 = oIPAddresses.Get_Dell(intBuildClass, intBuildEnv, intBuildAddress, 0, 0, intLTM_Web, intLTM_App, intLTM_Middleware, 0, 0, intDellWeb, intDellMiddleware, intDellDatabase, intDellFile, intDellMisc, intDellUnder48, 0, true, intServer, 0, intResiliency, intEnvironment, dsnServiceEditor);
                                                            oServer.AddOutput(intServer, "IP_ASSIGN_2", oIPAddresses.Results());
                                                            oIPAddresses.ClearResults();
                                                            if (intIPAddressBuild2 == 0)
                                                                break;
                                                            int intIPAddressVlan2 = oIPAddresses.GetNetworkVlan(oIPAddresses.GetAddressNetwork(intIPAddressBuild2));

                                                            oLog.AddEvent(intAnswer, strName, strSerial, "Assigning IP Address # 3 for SUN on DELL (Class = " + intBuildClass.ToString() + ", Env = " + intBuildEnv.ToString() + ", Address = " + intBuildAddress.ToString() + ", LTM (Web) = " + intLTM_Web.ToString() + ", LTM (App) = " + intLTM_App.ToString() + ", LTM (Middleware) = " + intLTM_Middleware.ToString() + ", Web = " + intDellWeb.ToString() + ", Middleware = " + intDellMiddleware.ToString() + ", Database = " + intDellDatabase.ToString() + ", File = " + intDellFile.ToString() + ", Misc = " + intDellMisc.ToString() + ", Under48 = " + intDellUnder48.ToString() + ", Resiliency: " + oResiliency.Get(intResiliency, "name") + ", Avamar = 0)", LoggingType.Information);
                                                            intIPAddressBuild3 = oIPAddresses.Get_Dell(intBuildClass, intBuildEnv, intBuildAddress, 0, 0, intLTM_Web, intLTM_App, intLTM_Middleware, 0, 0, intDellWeb, intDellMiddleware, intDellDatabase, intDellFile, intDellMisc, intDellUnder48, 0, true, intServer, 0, intResiliency, intEnvironment, dsnServiceEditor);
                                                            oServer.AddOutput(intServer, "IP_ASSIGN_3", oIPAddresses.Results());
                                                            oIPAddresses.ClearResults();
                                                            if (intIPAddressBuild3 == 0)
                                                                break;
                                                            int intIPAddressVlan3 = oIPAddresses.GetNetworkVlan(oIPAddresses.GetAddressNetwork(intIPAddressBuild3));

                                                            if (intIPAddressVlan1 != intIPAddressVlan2 || intIPAddressVlan1 != intIPAddressVlan3)
                                                            {
                                                                intIPAddressBuild1 = 0;
                                                                oIPAddresses.UpdateAvailable(intIPAddressBuild1, 1);    // Clear the assigned IP address
                                                                intIPAddressBuild2 = 0;
                                                                oIPAddresses.UpdateAvailable(intIPAddressBuild2, 1);    // Clear the assigned IP address
                                                                intIPAddressBuild3 = 0;
                                                                oIPAddresses.UpdateAvailable(intIPAddressBuild3, 1);    // Clear the assigned IP address
                                                            }
                                                        }
                                                        if (intIPAddressBuild1 > 0 && intIPAddressBuild2 > 0 && intIPAddressBuild3 > 0)
                                                        {
                                                            //if (intTSM == 0)
                                                            //{
                                                            //    // Need to assign an additional IP address for the BACKUP NIC
                                                            //    oLog.AddEvent(intAnswer, strName, strSerial, "Assigning IP Address (Backup) for (Class = " + intBuildClass.ToString() + ", Env = " + intBuildEnv.ToString() + ", Address = " + intBuildAddress.ToString() + ", Type = Dell Blade, Web = 0, Middleware = 0, Database = 0, File = 0, Misc = 0, Under48 = 0, Avamar = 1, ServerID = " + intServer.ToString() + ", Resiliency: " + oResiliency.Get(intResiliency, "name") + ")", LoggingType.Information);
                                                            //    intIPAddressBackup = oIPAddresses.Get_Dell(intBuildClass, intBuildEnv, intBuildAddress, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, true, intServer, 0, intResiliency, intEnvironment, dsnServiceEditor);
                                                            //    // Log IP Address Query
                                                            //    oServer.AddOutput(intServer, "IP_ASSIGN_BACKUP", oIPAddresses.Results());
                                                            //    oIPAddresses.ClearResults();

                                                                if (intIPAddressBackup > 0)
                                                                {
                                                                    oServer.AddIP(intServer, intIPAddressBackup, 0, 0, 0, 1);
                                                                    strResult += "IP Address (Backup): " + oIPAddresses.GetName(intIPAddressBackup, 0) + "<br/>";
                                                                }
                                                            //    else
                                                            //    {
                                                            //        oIPAddresses.UpdateAvailable(intIPAddressBuild1, 1);    // Clear the assigned IP address
                                                            //        oIPAddresses.UpdateAvailable(intIPAddressBuild2, 1);    // Clear the assigned IP address
                                                            //        oIPAddresses.UpdateAvailable(intIPAddressBuild3, 1);    // Clear the assigned IP address
                                                            //        strError = "All available IP addresses are in use for the criteria specified";
                                                            //        oLog.AddEvent(intAnswer, strName, strSerial, strError, LoggingType.Error);
                                                            //    }
                                                            //}

                                                            if (strError == "")
                                                            {
                                                                strResult += "IP Address # 1: " + oIPAddresses.GetName(intIPAddressBuild1, 0);
                                                                oLog.AddEvent(intAnswer, strName, strSerial, "IP address # 1= " + oIPAddresses.GetName(intIPAddressBuild1, 0), LoggingType.Information);
                                                                oServer.AddIP(intServer, intIPAddressBuild1, 1, 1, 0, 0);

                                                                strResult += "<br/>";

                                                                strResult += "IP Address # 2: " + oIPAddresses.GetName(intIPAddressBuild2, 0);
                                                                oLog.AddEvent(intAnswer, strName, strSerial, "IP address # 2= " + oIPAddresses.GetName(intIPAddressBuild2, 0), LoggingType.Information);
                                                                oServer.AddIP(intServer, intIPAddressBuild2, 1, 1, 0, 0);

                                                                strResult += "<br/>";

                                                                strResult += "IP Address # 3: " + oIPAddresses.GetName(intIPAddressBuild3, 0);
                                                                oLog.AddEvent(intAnswer, strName, strSerial, "IP address # 3= " + oIPAddresses.GetName(intIPAddressBuild3, 0), LoggingType.Information);
                                                                oServer.AddIP(intServer, intIPAddressBuild3, 1, 1, 0, 0);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            oIPAddresses.UpdateAvailable(intIPAddressBuild1, 1);    // Clear the assigned IP address
                                                            oIPAddresses.UpdateAvailable(intIPAddressBuild2, 1);    // Clear the assigned IP address
                                                            oIPAddresses.UpdateAvailable(intIPAddressBuild3, 1);    // Clear the assigned IP address
                                                            strError = "All available IP addresses are in use for the criteria specified ~ (#2) please report this problem to your ClearView administrator ~ (SUN on Nexus)";
                                                            oLog.AddEvent(intAnswer, strName, strSerial, strError, LoggingType.Error);
                                                        }
                                                    }
                                                }
                                                else if (oOperatingSystem.IsSolaris(intOS) == true)
                                                {
                                                    // Solaris IP Addresses
                                                    oLog.AddEvent(intAnswer, strName, strSerial, "OS = Solaris", LoggingType.Information);
                                                    int intVLANnot = 0;
                                                    while (intIPAddressBuild1 == 0 && intIPAddressBuild2 == 0 && intIPAddressBuild3 == 0)
                                                    {
                                                        oLog.AddEvent(intAnswer, strName, strSerial, "Assigning IP Address # 1 for SUN BLADE (Class = " + intBuildClass.ToString() + ", Env = " + intBuildEnv.ToString() + ", Address = " + intBuildAddress.ToString() + ", VLAN = " + intVLAN.ToString() + ", VLANnot = " + intVLANnot.ToString() + ", HA = " + (intHA == 1 ? "Yes" : "No") + ", Resiliency: " + oResiliency.Get(intResiliency, "name") + ")", LoggingType.Information);
                                                        intIPAddressBuild1 = oIPAddresses.Get_Blade_SUN(intBuildClass, intBuildEnv, intBuildAddress, 0, 0, intVLAN, intVLANnot, 0, 0, true, intServer, intResiliency, intEnvironment, dsnServiceEditor);
                                                        oServer.AddOutput(intServer, "IP_ASSIGN_1", oIPAddresses.Results());
                                                        oIPAddresses.ClearResults();
                                                        if (intIPAddressBuild1 == 0)
                                                            break;
                                                        int intIPAddressVlan1 = oIPAddresses.GetNetworkVlan(oIPAddresses.GetAddressNetwork(intIPAddressBuild1));

                                                        oLog.AddEvent(intAnswer, strName, strSerial, "Assigning IP Address # 2 for SUN BLADE (Class = " + intBuildClass.ToString() + ", Env = " + intBuildEnv.ToString() + ", Address = " + intBuildAddress.ToString() + ", VLAN = " + intVLAN.ToString() + ", VLANnot = " + intVLANnot.ToString() + ", HA = " + (intHA == 1 ? "Yes" : "No") + ", Resiliency: " + oResiliency.Get(intResiliency, "name") + ")", LoggingType.Information);
                                                        intIPAddressBuild2 = oIPAddresses.Get_Blade_SUN(intBuildClass, intBuildEnv, intBuildAddress, 0, 0, intVLAN, intVLANnot, 0, 0, true, intServer, intResiliency, intEnvironment, dsnServiceEditor);
                                                        oServer.AddOutput(intServer, "IP_ASSIGN_2", oIPAddresses.Results());
                                                        oIPAddresses.ClearResults();
                                                        if (intIPAddressBuild2 == 0)
                                                            break;
                                                        int intIPAddressVlan2 = oIPAddresses.GetNetworkVlan(oIPAddresses.GetAddressNetwork(intIPAddressBuild2));

                                                        oLog.AddEvent(intAnswer, strName, strSerial, "Assigning IP Address # 3 for SUN BLADE (Class = " + intBuildClass.ToString() + ", Env = " + intBuildEnv.ToString() + ", Address = " + intBuildAddress.ToString() + ", VLAN = " + intVLAN.ToString() + ", VLANnot = " + intVLANnot.ToString() + ", HA = " + (intHA == 1 ? "Yes" : "No") + ", Resiliency: " + oResiliency.Get(intResiliency, "name") + ")", LoggingType.Information);
                                                        intIPAddressBuild3 = oIPAddresses.Get_Blade_SUN(intBuildClass, intBuildEnv, intBuildAddress, 0, 0, intVLAN, intVLANnot, 0, 0, true, intServer, intResiliency, intEnvironment, dsnServiceEditor);
                                                        oServer.AddOutput(intServer, "IP_ASSIGN_3", oIPAddresses.Results());
                                                        oIPAddresses.ClearResults();
                                                        if (intIPAddressBuild3 == 0)
                                                            break;
                                                        int intIPAddressVlan3 = oIPAddresses.GetNetworkVlan(oIPAddresses.GetAddressNetwork(intIPAddressBuild3));

                                                        if (intIPAddressVlan1 != intIPAddressVlan2 || intIPAddressVlan1 != intIPAddressVlan3)
                                                        {
                                                            intIPAddressBuild1 = 0;
                                                            intIPAddressBuild2 = 0;
                                                            intIPAddressBuild3 = 0;
                                                            intVLANnot = intIPAddressVlan1;
                                                        }
                                                    }
                                                    if (intIPAddressBuild1 > 0 && intIPAddressBuild2 > 0 && intIPAddressBuild3 > 0)
                                                    {
                                                        strResult += "IP Address # 1: " + oIPAddresses.GetName(intIPAddressBuild1, 0);
                                                        oLog.AddEvent(intAnswer, strName, strSerial, "IP address # 1= " + oIPAddresses.GetName(intIPAddressBuild1, 0), LoggingType.Information);
                                                        oServer.AddIP(intServer, intIPAddressBuild1, 1, 1, 0, 0);

                                                        strResult += "<br/>";

                                                        strResult += "IP Address # 2: " + oIPAddresses.GetName(intIPAddressBuild2, 0);
                                                        oLog.AddEvent(intAnswer, strName, strSerial, "IP address # 2= " + oIPAddresses.GetName(intIPAddressBuild2, 0), LoggingType.Information);
                                                        oServer.AddIP(intServer, intIPAddressBuild2, 1, 1, 0, 0);

                                                        strResult += "<br/>";

                                                        strResult += "IP Address # 3: " + oIPAddresses.GetName(intIPAddressBuild3, 0);
                                                        oLog.AddEvent(intAnswer, strName, strSerial, "IP address # 3= " + oIPAddresses.GetName(intIPAddressBuild3, 0), LoggingType.Information);
                                                        oServer.AddIP(intServer, intIPAddressBuild3, 1, 1, 0, 0);
                                                    }
                                                    else
                                                    {
                                                        oIPAddresses.UpdateAvailable(intIPAddressBuild1, 1);
                                                        oIPAddresses.UpdateAvailable(intIPAddressBuild2, 1);
                                                        oIPAddresses.UpdateAvailable(intIPAddressBuild3, 1);
                                                        strError = "All available IP addresses are in use for the criteria specified ~ (#2) please report this problem to your ClearView administrator ~ (" + (boolBlade ? "Blade" : "Physical") + ": VLAN = " + (strIP_HA == "" ? intVLAN.ToString() : strIP_HA + " [HA]") + ")";
                                                        oLog.AddEvent(intAnswer, strName, strSerial, strError, LoggingType.Error);
                                                    }
                                                }
                                                else
                                                {
                                                    strError = "Invalid Operating System for IP Addressing ~ (" + strOS + ")";
                                                    oLog.AddEvent(intAnswer, strName, strSerial, strError, LoggingType.Error);
                                                }
                                            }
                                            else
                                                strResult = "Currently, ClearView can NOT automatically assign IP addresses for the &quot;" + oEnvironment.Get(intBuildEnv, "name") + "&quot; environment in &quot;" + oClass.Get(intBuildClass, "name") + "&quot; at &quot;" + oLocation.GetFull(intBuildAddress) + "&quot;";
                                        }
                                        else
                                        {
                                            // National City
                                            if (intEnv == intCore)
                                            {
                                                bool boolFinal = false;
                                                bool boolTest = false;
                                                if (oClass.IsProd(intClass))
                                                {
                                                    if (intTestDomain > 0)
                                                        boolTest = true;    // Assign IP Address in TEST
                                                    strResult = "PROD IP: Not available at this time<br/>";
                                                    oLog.AddEvent(intAnswer, strName, strSerial, "Production IP Addressing not available", LoggingType.Information);
                                                }
                                                else
                                                {
                                                    boolFinal = true;
                                                    boolTest = true;    // Assign IP Address in TEST
                                                }
                                                if (boolTest == true)
                                                {
                                                    if (oOperatingSystem.IsWindows(intOS) == true || oOperatingSystem.IsWindows2008(intOS) == true)
                                                    {
                                                        oLog.AddEvent(intAnswer, strName, strSerial, "OS = Windows", LoggingType.Information);
                                                        // Windows IP Addresses
                                                        if (boolBlade == true)
                                                        {
                                                            if (boolDell == true)
                                                            {
                                                                oLog.AddEvent(intAnswer, strName, strSerial, "Assigning IP Address for Dell BLADE (Class = " + intBuildClass.ToString() + ", Env = " + intBuildEnv.ToString() + ", Address = " + intBuildAddress.ToString() + ", LTM (Web) = " + intLTM_Web.ToString() + ", LTM (App) = " + intLTM_App.ToString() + ", LTM (Middleware) = " + intLTM_Middleware.ToString() + ", Web = " + intDellWeb.ToString() + ", Middleware = " + intDellMiddleware.ToString() + ", Database = " + intDellDatabase.ToString() + ", File = " + intDellFile.ToString() + ", Misc = " + intDellMisc.ToString() + ", Under48 = " + intDellUnder48.ToString() + ", Resiliency: " + oResiliency.Get(intResiliency, "name") + ", Avamar = 0)", LoggingType.Information);
                                                                intIPAddressBuild1 = oIPAddresses.Get_Dell(intBuildClass, intBuildEnv, intBuildAddress, 0, 0, intLTM_Web, intLTM_App, intLTM_Middleware, 0, 0, intDellWeb, intDellMiddleware, intDellDatabase, intDellFile, intDellMisc, intDellUnder48, 0, true, intServer, 0, intResiliency, intEnvironment, dsnServiceEditor);
                                                            }
                                                            else
                                                            {
                                                                oLog.AddEvent(intAnswer, strName, strSerial, "Assigning IP Address for BLADE (Class = " + intBuildClass.ToString() + ", Env = " + intBuildEnv.ToString() + ", Address = " + intBuildAddress.ToString() + ", VLAN = " + intVLAN.ToString() + ", HA = " + (intHA == 1 ? "Yes" : "No") + ", Resiliency: " + oResiliency.Get(intResiliency, "name") + ")", LoggingType.Information);
                                                                intIPAddressBuild1 = oIPAddresses.Get_Blade_HP(intBuildClass, intBuildEnv, intBuildAddress, 0, 0, intVLAN, 0, 0, true, intServer, intResiliency, intEnvironment, dsnServiceEditor);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            oLog.AddEvent(intAnswer, strName, strSerial, "Assigning IP Address for PHYSICAL (Class = " + intBuildClass.ToString() + ", Env = " + intBuildEnv.ToString() + ", Address = " + intBuildAddress.ToString() + ", VLAN = " + intVLAN.ToString() + ", HA = " + (intHA == 1 ? "Yes" : "No") + ", Resiliency: " + oResiliency.Get(intResiliency, "name") + ")", LoggingType.Information);
                                                            intIPAddressBuild1 = oIPAddresses.Get_VLAN(intBuildClass, intBuildEnv, intBuildAddress, 0, 0, intVLAN, 0, 0, 0, 0, 0, 0, 0, true, intServer, 0, 0, intResiliency, intEnvironment, dsnServiceEditor);
                                                        }
                                                        // Log IP Address Query
                                                        oServer.AddOutput(intServer, "IP_ASSIGN", oIPAddresses.Results());
                                                        oIPAddresses.ClearResults();
                                                        if (intIPAddressBuild1 > 0)
                                                        {
                                                            oServer.AddIP(intServer, intIPAddressBuild1, 1, (boolFinal ? 1 : 0), 0, 0);
                                                            strResult += "TEST IP: " + oIPAddresses.GetName(intIPAddressBuild1, 0);
                                                            oLog.AddEvent(intAnswer, strName, strSerial, "TEST IP: " + oIPAddresses.GetName(intIPAddressBuild1, 0), LoggingType.Information);
                                                        }
                                                        else
                                                        {
                                                            strError = "All available IP addresses are in use for the criteria specified ~ (#1) please report this problem to your ClearView administrator ~ (" + (boolBlade ? "Blade" : "Physical") + ": VLAN = " + intVLAN + ")";
                                                            oLog.AddEvent(intAnswer, strName, strSerial, strError, LoggingType.Error);
                                                        }
                                                    }
                                                    else if (oOperatingSystem.IsSolaris(intOS) == true)
                                                    {
                                                        oLog.AddEvent(intAnswer, strName, strSerial, "OS = Solaris", LoggingType.Information);
                                                        // Solaris IP Addresses
                                                        int intVLANnot = 0;
                                                        while (intIPAddressBuild1 == 0 && intIPAddressBuild2 == 0 && intIPAddressBuild3 == 0)
                                                        {
                                                            oLog.AddEvent(intAnswer, strName, strSerial, "Assigning IP Address # 1 for SUN BLADE (Class = " + intBuildClass.ToString() + ", Env = " + intBuildEnv.ToString() + ", Address = " + intBuildAddress.ToString() + ", VLAN = " + intVLAN.ToString() + ", VLANnot = " + intVLANnot.ToString() + ", HA = " + (intHA == 1 ? "Yes" : "No") + ", Resiliency: " + oResiliency.Get(intResiliency, "name") + ")", LoggingType.Information);
                                                            intIPAddressBuild1 = oIPAddresses.Get_Blade_SUN(intBuildClass, intBuildEnv, intBuildAddress, 0, 0, intVLAN, intVLANnot, 0, 0, true, intServer, intResiliency, intEnvironment, dsnServiceEditor);
                                                            oServer.AddOutput(intServer, "IP_ASSIGN_1", oIPAddresses.Results());
                                                            oIPAddresses.ClearResults();
                                                            if (intIPAddressBuild1 == 0)
                                                                break;
                                                            int intIPAddressVlan1 = oIPAddresses.GetNetworkVlan(oIPAddresses.GetAddressNetwork(intIPAddressBuild1));

                                                            oLog.AddEvent(intAnswer, strName, strSerial, "Assigning IP Address # 2 for SUN BLADE (Class = " + intBuildClass.ToString() + ", Env = " + intBuildEnv.ToString() + ", Address = " + intBuildAddress.ToString() + ", VLAN = " + intVLAN.ToString() + ", VLANnot = " + intVLANnot.ToString() + ", HA = " + (intHA == 1 ? "Yes" : "No") + ", Resiliency: " + oResiliency.Get(intResiliency, "name") + ")", LoggingType.Information);
                                                            intIPAddressBuild2 = oIPAddresses.Get_Blade_SUN(intBuildClass, intBuildEnv, intBuildAddress, 0, 0, intVLAN, intVLANnot, 0, 0, true, intServer, intResiliency, intEnvironment, dsnServiceEditor);
                                                            oServer.AddOutput(intServer, "IP_ASSIGN_2", oIPAddresses.Results());
                                                            oIPAddresses.ClearResults();
                                                            if (intIPAddressBuild2 == 0)
                                                                break;
                                                            int intIPAddressVlan2 = oIPAddresses.GetNetworkVlan(oIPAddresses.GetAddressNetwork(intIPAddressBuild2));

                                                            oLog.AddEvent(intAnswer, strName, strSerial, "Assigning IP Address # 3 for SUN BLADE (Class = " + intBuildClass.ToString() + ", Env = " + intBuildEnv.ToString() + ", Address = " + intBuildAddress.ToString() + ", VLAN = " + intVLAN.ToString() + ", VLANnot = " + intVLANnot.ToString() + ", HA = " + (intHA == 1 ? "Yes" : "No") + ", Resiliency: " + oResiliency.Get(intResiliency, "name") + ")", LoggingType.Information);
                                                            intIPAddressBuild3 = oIPAddresses.Get_Blade_SUN(intBuildClass, intBuildEnv, intBuildAddress, 0, 0, intVLAN, intVLANnot, 0, 0, true, intServer, intResiliency, intEnvironment, dsnServiceEditor);
                                                            oServer.AddOutput(intServer, "IP_ASSIGN_3", oIPAddresses.Results());
                                                            oIPAddresses.ClearResults();
                                                            if (intIPAddressBuild3 == 0)
                                                                break;
                                                            int intIPAddressVlan3 = oIPAddresses.GetNetworkVlan(oIPAddresses.GetAddressNetwork(intIPAddressBuild3));

                                                            if (intIPAddressVlan1 != intIPAddressVlan2 || intIPAddressVlan1 != intIPAddressVlan3)
                                                            {
                                                                intIPAddressBuild1 = 0;
                                                                intIPAddressBuild2 = 0;
                                                                intIPAddressBuild3 = 0;
                                                                intVLANnot = intIPAddressVlan1;
                                                            }
                                                        }
                                                        if (intIPAddressBuild1 > 0 && intIPAddressBuild2 > 0 && intIPAddressBuild3 > 0)
                                                        {
                                                            strResult += "IP Address # 1: " + oIPAddresses.GetName(intIPAddressBuild1, 0);
                                                            oLog.AddEvent(intAnswer, strName, strSerial, "IP address # 1= " + oIPAddresses.GetName(intIPAddressBuild1, 0), LoggingType.Information);
                                                            oServer.AddIP(intServer, intIPAddressBuild1, 1, 1, 0, 0);

                                                            strResult += "<br/>";

                                                            strResult += "IP Address # 2: " + oIPAddresses.GetName(intIPAddressBuild2, 0);
                                                            oLog.AddEvent(intAnswer, strName, strSerial, "IP address # 2= " + oIPAddresses.GetName(intIPAddressBuild2, 0), LoggingType.Information);
                                                            oServer.AddIP(intServer, intIPAddressBuild2, 1, 1, 0, 0);

                                                            strResult += "<br/>";

                                                            strResult += "IP Address # 3: " + oIPAddresses.GetName(intIPAddressBuild3, 0);
                                                            oLog.AddEvent(intAnswer, strName, strSerial, "IP address # 3= " + oIPAddresses.GetName(intIPAddressBuild3, 0), LoggingType.Information);
                                                            oServer.AddIP(intServer, intIPAddressBuild3, 1, 1, 0, 0);
                                                        }
                                                        else
                                                        {
                                                            strError = "All available IP addresses are in use for the criteria specified ~ (#1) please report this problem to your ClearView administrator ~ (" + (boolBlade ? "Blade" : "Physical") + ": VLAN = " + intVLAN + ")";
                                                            oLog.AddEvent(intAnswer, strName, strSerial, strError, LoggingType.Error);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        strError = "Invalid Operating System for IP Addressing ~ (" + strOS + ")";
                                                        oLog.AddEvent(intAnswer, strName, strSerial, strError, LoggingType.Error);
                                                    }
                                                }
                                            }
                                            else
                                                strResult = "Currently, ClearView can only automatically assign IP addresses for the Core";
                                        }
                                    }
                                    else if (intVLAN == -999)
                                        strResult = "IP Addressing has been skipped for this asset";
                                    else
                                    {
                                        strError = "The VLAN for this asset has not been configured";
                                        oLog.AddEvent(intAnswer, strName, strSerial, strError, LoggingType.Error);
                                    }
                                    AddResult(intServer, intStep, intType, strResult, strError);
                                    break;
                                case 4:     // Create Active Directory Groups
                                    if (boolDemoSAN == false)
                                    {
                                        oLog.AddEvent(intAnswer, strName, strSerial, "Starting Step 4 (Active Directory Groups)", LoggingType.Information);
                                        if (oOperatingSystem.IsWindows(intOS) == true || oOperatingSystem.IsWindows2008(intOS) == true)
                                        {
                                            if (oDomain.Get(intDomain, "account_setup") == "1")
                                            {
                                                if (boolPNC == true)
                                                {
                                                    string strMnemonicCode = oMnemonic.Get(intMnemonic, "factory_code").ToUpper();
                                                    string strMnemonicName = oFunction.ToTitleCase(oMnemonic.Get(intMnemonic, "name"));
                                                    if (intClusterID > 0)
                                                    {
                                                        if (oSetting.Get("configure_cluster") == "1")
                                                        {
                                                            // Cluster account(s)
                                                            string strClusterName = "";
                                                            string strClusterNameSuffix = "";
                                                            string strClusterAppInstance = "";
                                                            DataSet dsClusters = oServer.GetClusters(intClusterID);
                                                            DataSet dsInstances = oCluster.GetInstances(intClusterID);
                                                            string strClusterClass = "T";
                                                            if (oClass.IsProd(intClass))
                                                                strClusterClass = "P";
                                                            else if (oClass.IsQA(intClass))
                                                                strClusterClass = "Q";
                                                            // Example: WDRDP103DZ and WDRDP104DZ and WDRDP105DZ...will only get the first two (03 and 04)
                                                            ArrayList oArray = new ArrayList(2);
                                                            bool boolClusterContinue = true;
                                                            for (int ii = 0; ii < 2; ii++)
                                                            {
                                                                int intClusterServerName = Int32.Parse(dsClusters.Tables[0].Rows[ii]["id"].ToString());
                                                                string strClusterServerName = oServer.GetName(intClusterServerName, true);
                                                                if (strClusterServerName.Length < 8)
                                                                    boolClusterContinue = false;
                                                                oArray.Add(strClusterServerName.ToUpper());
                                                            }
                                                            if (boolClusterContinue == true)
                                                            {
                                                                oArray.Sort();
                                                                foreach (string strClusterServerName in oArray)
                                                                {
                                                                    if (strClusterName == "")
                                                                    {
                                                                        strClusterName = strClusterServerName.Substring(0, 8);
                                                                        strClusterNameSuffix = strClusterServerName.Substring(8, 1);
                                                                    }
                                                                    else
                                                                    {
                                                                        strClusterName += strClusterServerName.Substring(6, 2);
                                                                        strClusterAppInstance = strClusterServerName.Substring(0, 8);
                                                                    }
                                                                }
                                                                strClusterName += strClusterNameSuffix.ToLower();
                                                                oLog.AddEvent(intAnswer, strName, strSerial, "Cluster Name = " + strClusterName, LoggingType.Information);
                                                                oCluster.UpdateName(intClusterID, strClusterName);
                                                                bool boolWin2008 = oOperatingSystem.IsWindows2008(intOS);

                                                                if (strError == "")
                                                                {
                                                                    // Create the computer object for the Cluster
                                                                    if (oAD.Search(strClusterName, true) == null)
                                                                    {
                                                                        string strResultServerObject = oAD.CreateServer(strClusterName, strClusterName + " - Cluster Virtual Name", "Created by ClearView - " + DateTime.Now.ToShortDateString(), "OU=OUs_ClusterVirtual,OU=OUc_Servers,OU=OUc_Computers,");
                                                                        if (strResultServerObject == "")
                                                                        {
                                                                            string strClusterAD = "The computer object " + strClusterName + " was successfully created in " + oVar.Name();
                                                                            oLog.AddEvent(intAnswer, strName, strSerial, strClusterAD, LoggingType.Information);
                                                                            strResult += strClusterAD + "<br/>";
                                                                            if (boolWin2008 == true)
                                                                            {
                                                                                oLog.AddEvent(intAnswer, strName, strSerial, "Windows 2008 = Disable Cluster Account...Searching for account (" + strClusterName + ")...", LoggingType.Information);
                                                                                // Disable the account
                                                                                DirectoryEntry oClusterName = oAD.Search(strClusterName, true);
                                                                                for (int ii = 0; ii < 10 && oClusterName == null; ii++)
                                                                                {
                                                                                    oLog.AddEvent(intAnswer, strName, strSerial, "Could not find account...waiting 5 seconds...", LoggingType.Debug);
                                                                                    Thread.Sleep(5000);
                                                                                    oClusterName = oAD.Search(strClusterName, true);
                                                                                }
                                                                                if (oClusterName != null)
                                                                                {
                                                                                    oLog.AddEvent(intAnswer, strName, strSerial, "Found account (" + strClusterName + ")! Disabling...", LoggingType.Information);
                                                                                    string strEnable = oAD.Enable(oClusterName, false);
                                                                                    if (strEnable == "")
                                                                                        oLog.AddEvent(intAnswer, strName, strSerial, "Done disabling cluster account (" + strClusterName + ")", LoggingType.Information);
                                                                                    else
                                                                                        strError = "There was a problem disabling the cluster account ~ Error: " + strEnable;
                                                                                }
                                                                                else
                                                                                    strError = "Could not find the cluster account ~ Account: " + strClusterName;
                                                                            }
                                                                            else
                                                                                oLog.AddEvent(intAnswer, strName, strSerial, "Not Windows 2008....leave cluster account enabled and move on.", LoggingType.Information);
                                                                        }
                                                                        else
                                                                            strError = "There was a problem creating the computer object ~ " + strClusterName + " in " + oVar.Name() + " (" + strResultServerObject + ")";
                                                                    }
                                                                    else
                                                                    {
                                                                        string strClusterAD = "The computer object " + strClusterName + " already exists in " + oVar.Name();
                                                                        oLog.AddEvent(intAnswer, strName, strSerial, strClusterAD, LoggingType.Information);
                                                                        strResult += strClusterAD + "<br/>";
                                                                    }
                                                                }

                                                                if (strError == "")
                                                                {
                                                                    bool boolSQL = false;
                                                                    foreach (DataRow drInstance in dsInstances.Tables[0].Rows)
                                                                    {
                                                                        if (drInstance["sql"].ToString() == "1")
                                                                        {
                                                                            boolSQL = true;
                                                                            break;
                                                                        }
                                                                    }

                                                                    string strClusterVirtualName = oCluster.Get(intClusterID, "virtual_name");
                                                                    if (boolSQL == true)
                                                                    {
                                                                        // Create the MSDTC Virtual Name
                                                                        bool boolSQL2008 = false;
                                                                        foreach (DataRow drComponent in dsComponents.Tables[0].Rows)
                                                                        {
                                                                            if (drComponent["sql"].ToString() == "1" || drComponent["dbase"].ToString() == "1")
                                                                            {
                                                                                if (drComponent["name"].ToString().Contains("2008") == true)
                                                                                {
                                                                                    boolSQL2008 = true;
                                                                                    break;
                                                                                }
                                                                            }
                                                                        }
                                                                        // 12/2/2011 - Per Moskal, 2008 now needs the MSDTC drive.
                                                                        //if (boolWin2008 == true && boolSQL2008 == true)
                                                                        //{
                                                                        //    oLog.AddEvent(intAnswer, strName, strSerial, "The MSDTC Virtual Name is not required for SQL Server 2008 on a Windows 2008 cluster", LoggingType.Information);
                                                                        //}
                                                                        //else
                                                                        //{
                                                                            for (int ii = 1; ii < 100; ii++)
                                                                            {
                                                                                string strClusterCounter = ii.ToString();
                                                                                if (strClusterCounter.Length == 1)
                                                                                    strClusterCounter = "0" + strClusterCounter;
                                                                                string strClusterDTCVirtual = strMnemonicCode + strClusterClass + "DTC" + "A" + strClusterCounter;
                                                                                if (strClusterVirtualName != "")
                                                                                    strClusterDTCVirtual = strClusterVirtualName;
                                                                                oLog.AddEvent(intAnswer, strName, strSerial, "Trying MSDTC Virtual Name object = " + strClusterDTCVirtual, LoggingType.Debug);
                                                                                if (oAD.Search(strClusterDTCVirtual, true) == null)
                                                                                {
                                                                                    string strResultServerObject = oAD.CreateServer(strClusterDTCVirtual, strClusterName + " - MSDTC Virtual Name", "Created by ClearView - " + DateTime.Now.ToShortDateString(), "OU=OUs_ClusterVirtual,OU=OUc_Servers,OU=OUc_Computers,");
                                                                                    if (strResultServerObject == "")
                                                                                    {
                                                                                        string strClusterAD = "The MSDTC Virtual Name object " + strClusterDTCVirtual + " was successfully created in " + oVar.Name();
                                                                                        oCluster.UpdateVirtualName(intClusterID, strClusterDTCVirtual);
                                                                                        oLog.AddEvent(intAnswer, strName, strSerial, strClusterAD, LoggingType.Information);
                                                                                        strResult += strClusterAD + "<br/>";
                                                                                    }
                                                                                    else
                                                                                        strError = "There was a problem creating the MSDTC Virtual Name object ~ " + strClusterDTCVirtual + " in " + oVar.Name() + " (" + strResultServerObject + ")";
                                                                                    break;
                                                                                }
                                                                                else
                                                                                {
                                                                                    string strClusterAD = "The MSDTC Virtual Name object " + strClusterDTCVirtual + " already exists in " + oVar.Name();
                                                                                    oLog.AddEvent(intAnswer, strName, strSerial, strClusterAD, LoggingType.Debug);
                                                                                    if (strClusterVirtualName != "")
                                                                                        break;
                                                                                }
                                                                            }
                                                                        //}
                                                                    }
                                                                    else
                                                                    {
                                                                        // Create the App Instance Name
                                                                        if (strClusterVirtualName != "")
                                                                            strClusterAppInstance = strClusterVirtualName;
                                                                        if (oAD.Search(strClusterAppInstance, true) == null)
                                                                        {
                                                                            string strResultServerObject = oAD.CreateServer(strClusterAppInstance, strClusterName + " - App Virtual Name", "Created by ClearView - " + DateTime.Now.ToShortDateString(), "OU=OUs_ClusterVirtual,OU=OUc_Servers,OU=OUc_Computers,");
                                                                            if (strResultServerObject == "")
                                                                            {
                                                                                string strClusterAD = "The App Instance Name object " + strClusterAppInstance + " was successfully created in " + oVar.Name();
                                                                                oCluster.UpdateVirtualName(intClusterID, strClusterAppInstance);
                                                                                oLog.AddEvent(intAnswer, strName, strSerial, strClusterAD, LoggingType.Information);
                                                                                strResult += strClusterAD + "<br/>";
                                                                                if (boolWin2008 == true)
                                                                                {
                                                                                    oLog.AddEvent(intAnswer, strName, strSerial, "Attempting to join the Cluster object (" + strClusterName + ") to the App Instance Name object (" + strClusterAppInstance + ")", LoggingType.Debug);
                                                                                    // If Windows 2008, join the Cluster Object to the Instance Name
                                                                                    DirectoryEntry oClusterName = oAD.Search(strClusterName, true);         // Cluster object
                                                                                    DirectoryEntry oInstanceName = oAD.Search(strClusterAppInstance, true); // Instance object
                                                                                    if (oInstanceName != null)
                                                                                    {
                                                                                        IADsAccessControlEntry newAce = new AccessControlEntryClass();
                                                                                        IADsSecurityDescriptor sd = (IADsSecurityDescriptor)oInstanceName.Properties["ntSecurityDescriptor"].Value;
                                                                                        IADsAccessControlList dacl = (IADsAccessControlList)sd.DiscretionaryAcl;
                                                                                        newAce.Trustee = oAD.GetSID(oClusterName);
                                                                                        newAce.AccessMask = -1;  // full control
                                                                                        newAce.AceType = 0;  //access allowed
                                                                                        //newAce.AceFlags = 2;
                                                                                        dacl.AddAce(newAce);
                                                                                        sd.DiscretionaryAcl = dacl;
                                                                                        oInstanceName.Properties["ntSecurityDescriptor"].Value = sd;
                                                                                        oInstanceName.CommitChanges();
                                                                                        strClusterAD = "The Cluster object (" + strClusterName + ") was given FULL ACCESS to the App Instance Name object (" + strClusterAppInstance + ")";
                                                                                        oLog.AddEvent(intAnswer, strName, strSerial, strClusterAD, LoggingType.Information);
                                                                                        strResult += strClusterAD + "<br/>";
                                                                                    }
                                                                                    else
                                                                                        strError = "The App Instance Name object (" + strClusterAppInstance + ") was not found";
                                                                                }
                                                                            }
                                                                            else
                                                                                strError = "There was a problem creating the App Instance Name object ~ " + strClusterAppInstance + " in " + oVar.Name() + " (" + strResultServerObject + ")";
                                                                        }
                                                                        else
                                                                        {
                                                                            string strClusterAD = "The App Instance Name object " + strClusterAppInstance + " already exists in " + oVar.Name();
                                                                            oLog.AddEvent(intAnswer, strName, strSerial, strClusterAD, LoggingType.Information);
                                                                            strResult += strClusterAD + "<br/>";
                                                                        }
                                                                    }
                                                                }

                                                                // Check the instance(s)
                                                                bool boolInstanceSQL = false;
                                                                foreach (DataRow drInstance in dsInstances.Tables[0].Rows)
                                                                {
                                                                    if (drInstance["sql"].ToString() == "1")
                                                                    {
                                                                        boolInstanceSQL = true;
                                                                        break;
                                                                    }
                                                                }

                                                                if (boolInstanceSQL == true)
                                                                {
                                                                    int intInstances = 0;
                                                                    foreach (DataRow drInstance in dsInstances.Tables[0].Rows)
                                                                    {
                                                                        int intInstance = Int32.Parse(drInstance["id"].ToString());
                                                                        // If the instance NAME is not populated, we know it isn't done.
                                                                        if (drInstance["name"].ToString() == "")
                                                                            intInstances++;
                                                                        else
                                                                            oLog.AddEvent(intAnswer, strName, strSerial, "The instance has already been configured", LoggingType.Debug);
                                                                    }

                                                                    int intInstancesInARow = 0;
                                                                    int intClusterCounterStart = 0;
                                                                    for (int ii = 1; ii < 100 && intInstancesInARow < intInstances; ii++)
                                                                    {
                                                                        string strClusterCounter = ii.ToString();
                                                                        if (strClusterCounter.Length == 1)
                                                                            strClusterCounter = "0" + strClusterCounter;
                                                                        string strClusterSQLInstance = strMnemonicCode + strClusterClass + "CSQ" + "A" + strClusterCounter;
                                                                        if (oAD.Search(strClusterSQLInstance, true) == null)
                                                                        {
                                                                            intInstancesInARow++;
                                                                            oLog.AddEvent(intAnswer, strName, strSerial, "The instance name " + strClusterSQLInstance + " was NOT found. (" + intInstancesInARow.ToString() + " of " + intInstances.ToString() + " in a row.)", LoggingType.Debug);
                                                                            if (intClusterCounterStart == 0)
                                                                                intClusterCounterStart = ii;
                                                                        }
                                                                        else
                                                                        {
                                                                            oLog.AddEvent(intAnswer, strName, strSerial, "The instance name " + strClusterSQLInstance + " was found. (resetting consecutive count)", LoggingType.Debug);
                                                                            intInstancesInARow = 0;
                                                                            intClusterCounterStart = 0;
                                                                        }
                                                                    }
                                                                    if (intInstancesInARow == intInstances)
                                                                    {
                                                                        foreach (DataRow drInstance in dsInstances.Tables[0].Rows)
                                                                        {
                                                                            int intInstance = Int32.Parse(drInstance["id"].ToString());
                                                                            string strClusterInstanceName = drInstance["name"].ToString();
                                                                            // Create the SQL Instance Name
                                                                            for (int ii = intClusterCounterStart; ii < 100; ii++)
                                                                            {
                                                                                string strClusterCounter = ii.ToString();
                                                                                if (strClusterCounter.Length == 1)
                                                                                    strClusterCounter = "0" + strClusterCounter;
                                                                                string strClusterSQLInstance = strMnemonicCode + strClusterClass + "CSQ" + "A" + strClusterCounter;
                                                                                if (strClusterInstanceName != "")
                                                                                    strClusterSQLInstance = strClusterInstanceName;
                                                                                oLog.AddEvent(intAnswer, strName, strSerial, "Trying SQL Instance Name object = " + strClusterSQLInstance, LoggingType.Debug);
                                                                                if (oAD.Search(strClusterSQLInstance, true) == null)
                                                                                {
                                                                                    string strResultServerObject = oAD.CreateServer(strClusterSQLInstance, strClusterName + " - SQL Instance Name", "Created by ClearView - " + DateTime.Now.ToShortDateString(), "OU=OUs_ClusterVirtual,OU=OUc_Servers,OU=OUc_Computers,");
                                                                                    if (strResultServerObject == "")
                                                                                    {
                                                                                        string strClusterAD = "The SQL Instance Name object " + strClusterSQLInstance + " was successfully created in " + oVar.Name();
                                                                                        oLog.AddEvent(intAnswer, strName, strSerial, strClusterAD, LoggingType.Information);
                                                                                        strResult += strClusterAD + "<br/>";
                                                                                        // Update the instance name to signify it being done (so other servers won't try to repeat this)
                                                                                        oCluster.UpdateInstanceName(intInstance, strClusterSQLInstance);
                                                                                        if (boolWin2008 == true)
                                                                                        {
                                                                                            // If Windows 2008, join the Cluster Object to the Instance Name
                                                                                            DirectoryEntry oClusterName = oAD.Search(strClusterName, true);         // Cluster object
                                                                                            DirectoryEntry oInstanceName = oAD.Search(strClusterSQLInstance, true); // Instance object
                                                                                            if (oInstanceName != null)
                                                                                            {
                                                                                                IADsAccessControlEntry newAce = new AccessControlEntryClass();
                                                                                                IADsSecurityDescriptor sd = (IADsSecurityDescriptor)oInstanceName.Properties["ntSecurityDescriptor"].Value;
                                                                                                IADsAccessControlList dacl = (IADsAccessControlList)sd.DiscretionaryAcl;
                                                                                                newAce.Trustee = oAD.GetSID(oClusterName);
                                                                                                newAce.AccessMask = -1;  // full control
                                                                                                newAce.AceType = 0;  //access allowed
                                                                                                //newAce.AceFlags = 2;
                                                                                                dacl.AddAce(newAce);
                                                                                                sd.DiscretionaryAcl = dacl;
                                                                                                oInstanceName.Properties["ntSecurityDescriptor"].Value = sd;
                                                                                                oInstanceName.CommitChanges();
                                                                                                strClusterAD = "The Cluster object (" + strClusterName + ") was given FULL ACCESS to the SQL Instance Name object (" + strClusterSQLInstance + ")";
                                                                                                oLog.AddEvent(intAnswer, strName, strSerial, strClusterAD, LoggingType.Information);
                                                                                                strResult += strClusterAD + "<br/>";
                                                                                            }
                                                                                        }
                                                                                    }
                                                                                    else
                                                                                        strError = "There was a problem creating the SQL Instance Name object ~ " + strClusterSQLInstance + " in " + oVar.Name() + " (" + strResultServerObject + ")";
                                                                                    break;
                                                                                }
                                                                                else
                                                                                {
                                                                                    string strClusterAD = "The SQL Instance Name object " + strClusterSQLInstance + " already exists in " + oVar.Name();
                                                                                    oLog.AddEvent(intAnswer, strName, strSerial, strClusterAD, LoggingType.Debug);
                                                                                    if (strClusterInstanceName != "")
                                                                                        break;
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                    else
                                                                        strError = "There was a problem generating the SQL Instance names ~ possibly out of range?";
                                                                }
                                                            }
                                                            else
                                                                oLog.AddEvent(intAnswer, strName, strSerial, "Apparently, not all the nodes have server names yet...skipping for now...", LoggingType.Information);
                                                        }
                                                        else
                                                            oLog.AddEvent(intAnswer, strName, strSerial, "Skipping Cluster Configuration", LoggingType.Information);
                                                    }
                                                    if (strError == "")
                                                    {
                                                        if (intOrganization == 0)
                                                            intOrganization = 1;    // Set to ECS
                                                        if (intOrganization > 0)
                                                        {
                                                            DataSet dsOrganization = oOrganization.Get(intOrganization);
                                                            if (dsOrganization.Tables[0].Rows.Count > 0)
                                                            {
                                                                string strOrganizationCode = dsOrganization.Tables[0].Rows[0]["code"].ToString().Trim().ToUpper();
                                                                if (strOrganizationCode != "")
                                                                {
                                                                    //int intCustodian = Int32.Parse(dsOrganization.Tables[0].Rows[0]["userid"].ToString());
                                                                    int intCustodian = 0;
                                                                    Int32.TryParse(oForecast.GetAnswer(intAnswer, "appowner"), out intCustodian);
                                                                    if (intCustodian > 0)
                                                                    {
                                                                        // Create DLG OUg_<mnemonic> groups
                                                                        string strMnemonicGroupDLG = "OUg_" + strMnemonicCode;
                                                                        if (oAD.SearchOU(strMnemonicGroupDLG) == null)
                                                                        {
                                                                            string strResultMnemonic = oAD.CreateOU(strMnemonicGroupDLG, "", "OU=OUc_Resources,");
                                                                            if (strResultMnemonic == "")
                                                                                strResult += "The OU " + strMnemonicGroupDLG + " was successfully created in " + oVar.Name() + "<br/>";
                                                                            else
                                                                                strError = "There was a problem creating the OU ~ " + strMnemonicGroupDLG + " in " + oVar.Name();
                                                                        }
                                                                        else
                                                                            strResult += "The OU " + strMnemonicGroupDLG + " already exists in " + oVar.Name() + "<br/>";

                                                                        if (strError == "")
                                                                        {
                                                                            // Assume the GG OU exists and continue creating
                                                                            string strGroupDescription = "Custodian: " + oUser.GetFullName(intCustodian) + " (" + oUser.GetName(intCustodian) + "), LOB-" + strOrganizationCode + ", MIS-" + strMnemonicCode + " " + strMnemonicName;
                                                                            string[] strGroups = oVariable.PNC_AD_Groups();

                                                                            for (int ii = 0; ii < strGroups.Length && strError == ""; ii++)
                                                                            {
                                                                                //string strNameDLG = "DLG_URA_" + strOrganizationCode + "_" + strMnemonicCode + "_" + strGroups[ii];
                                                                                string strNameDLG = "GSLfsaSP_" + strMnemonicCode + "_" + strGroups[ii];
                                                                                //string strNameGG = "GG_URA_" + strOrganizationCode + "_" + strMnemonicCode + "_" + strGroups[ii];
                                                                                string strNameGG = "GSGu_" + strMnemonicCode + "_" + strGroups[ii];

                                                                                if (strError == "")
                                                                                {
                                                                                    if (oAD.Search(strNameDLG, false) == null)
                                                                                    {
                                                                                        string strResultDLG = oAD.CreateGroup(strNameDLG, strGroupDescription, "", "OU=" + strMnemonicGroupDLG + ",OU=OUc_Resources,", "DLG", "S");
                                                                                        if (strResultDLG == "")
                                                                                            strResult += "The group " + strNameDLG + " was successfully created in " + oVar.Name() + "<br/>";
                                                                                        else
                                                                                        {
                                                                                            strError = "There was a problem creating the group ~ " + strNameDLG + " in " + oVar.Name();
                                                                                            break;
                                                                                        }
                                                                                    }
                                                                                    else
                                                                                        strResult += "The group " + strNameDLG + " already exists in " + oVar.Name() + "<br/>";
                                                                                }

                                                                                if (strError == "")
                                                                                {
                                                                                    if (oAD.Search(strNameGG, false) == null)
                                                                                    {
                                                                                        string strResultGG = oAD.CreateGroup(strNameGG, strGroupDescription, "", "OU=OUg_Applications,OU=OUc_AccessGroups,", "GG", "S");
                                                                                        if (strResultGG == "")
                                                                                            strResult += "The group " + strNameGG + " was successfully created in " + oVar.Name() + "<br/>";
                                                                                        else
                                                                                        {
                                                                                            strError = "There was a problem creating the group ~ " + strNameGG + " in " + oVar.Name();
                                                                                            break;
                                                                                        }
                                                                                    }
                                                                                    else
                                                                                        strResult += "The group " + strNameGG + " already exists in " + oVar.Name() + "<br/>";
                                                                                }

                                                                                if (strError == "")
                                                                                {
                                                                                    // Add the GG to the DLG
                                                                                    string strResultJoin = oAD.JoinGroup(strNameGG, strNameDLG, 0);
                                                                                    if (strResultJoin == "")
                                                                                        strResult += "The global group " + strNameGG + " was successfully added to the domain local group " + strNameDLG + " in " + oVar.Name() + "<br/>";
                                                                                    else
                                                                                    {
                                                                                        strError = "There was a problem adding the global group ~ " + strNameGG + " to the domain local group " + strNameDLG + " in " + oVar.Name();
                                                                                        break;
                                                                                    }
                                                                                }
                                                                            }
                                                                            // Add Windows 2012 specific groups
                                                                            if (IsWin2012)
                                                                            {
                                                                                if (strError == "")
                                                                                {
                                                                                    string strNameDLG = "GSLsrvLA_" + strName;
                                                                                    if (oAD.Search(strNameDLG, false) == null)
                                                                                    {
                                                                                        string strResultDLG = oAD.CreateGroup(strNameDLG, strGroupDescription, "", "OU=" + strMnemonicGroupDLG + ",OU=OUc_Resources,", "DLG", "S");
                                                                                        if (strResultDLG == "")
                                                                                            strResult += "The group " + strNameDLG + " was successfully created in " + oVar.Name() + "<br/>";
                                                                                        else
                                                                                        {
                                                                                            strError = "There was a problem creating the group ~ " + strNameDLG + " in " + oVar.Name();
                                                                                            break;
                                                                                        }
                                                                                        oLog.AddEvent(intAnswer, strName, strSerial, "The group " + strNameDLG + " was " + (String.IsNullOrEmpty(strError) ? "" : "NOT ") + "created in " + oVar.Name(), LoggingType.Information);
                                                                                    }
                                                                                    else
                                                                                        strResult += "The group " + strNameDLG + " already exists in " + oVar.Name() + "<br/>";
                                                                                }
                                                                                if (strError == "")
                                                                                {
                                                                                    string strNameDLG = "GSLsrvSP_" + strName + "_RemoteDesktopUsers";
                                                                                    if (oAD.Search(strNameDLG, false) == null)
                                                                                    {
                                                                                        string strResultDLG = oAD.CreateGroup(strNameDLG, strGroupDescription, "", "OU=" + strMnemonicGroupDLG + ",OU=OUc_Resources,", "DLG", "S");
                                                                                        if (strResultDLG == "")
                                                                                            strResult += "The group " + strNameDLG + " was successfully created in " + oVar.Name() + "<br/>";
                                                                                        else
                                                                                        {
                                                                                            strError = "There was a problem creating the group ~ " + strNameDLG + " in " + oVar.Name();
                                                                                            break;
                                                                                        }
                                                                                        oLog.AddEvent(intAnswer, strName, strSerial, "The group " + strNameDLG + " was " + (String.IsNullOrEmpty(strError) ? "" : "NOT ") + "created in " + oVar.Name(), LoggingType.Information);
                                                                                    }
                                                                                    else
                                                                                        strResult += "The group " + strNameDLG + " already exists in " + oVar.Name() + "<br/>";
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        strError = "The APP OWNER was not entered ~ (Design ID = " + intAnswer.ToString() + ")";
                                                                        oLog.AddEvent(intAnswer, strName, strSerial, strError, LoggingType.Error);
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    strError = "The organization is not configured for Active Directory Group naming (CODE is blank) ~ (" + dsOrganization.Tables[0].Rows[0]["name"].ToString() + ")";
                                                                    oLog.AddEvent(intAnswer, strName, strSerial, strError, LoggingType.Error);
                                                                }
                                                            }
                                                            else
                                                            {
                                                                strError = "Invalid Organization";
                                                                oLog.AddEvent(intAnswer, strName, strSerial, strError, LoggingType.Error);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            strError = "There is no organization specified for this project";
                                                            oLog.AddEvent(intAnswer, strName, strSerial, strError, LoggingType.Error);
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    strResult = "ClearView can only configure Active Directory objects in TEST";
                                                }
                                            }
                                            else
                                            {
                                                strResult = "Active Directory Configuration is not available for this domain";
                                            }
                                        }
                                        else
                                        {
                                            strResult = "This step is only available for distributed servers";
                                        }
                                    }
                                    else
                                    {
                                        oLog.AddEvent(intAnswer, strName, strSerial, "Starting Step 4 (Allocate Storage)", LoggingType.Information);
                                        // Configure the SAN switch
                                        try
                                        {
                                            StringBuilder strSAN = new StringBuilder();
                                            oLog.AddEvent(intAnswer, strName, strSerial, "Connecting to 10.49.254.229...", LoggingType.Debug);
                                            SshShell oSSHshella = new SshShell("10.49.254.229", "admin", "nccSAN03");
                                            oSSHshella.RemoveTerminalEmulationCharacters = true;
                                            oSSHshella.Connect();
                                            if (oSSHshella.Connected == true && oSSHshella.ShellOpened == true)
                                            {
                                                string strBanner = oSSHshella.Expect("#");
                                                oLog.AddEvent(intAnswer, strName, strSerial, "Connected to 10.49.254.229...executing commands", LoggingType.Debug);
                                                strSAN.Append("10.49.254.229...");
                                                strSAN.Append(ExecuteSSH("config t", oSSHshella));
                                                strSAN.Append(ExecuteSSH("device-alias database", oSSHshella));
                                                strSAN.Append(ExecuteSSH("device-alias name " + strName + "a pwwn 50:06:0b:00:00:c3:5a:34", oSSHshella));
                                                strSAN.Append(ExecuteSSH("exit", oSSHshella));
                                                strSAN.Append(ExecuteSSH("device-alias commit", oSSHshella));
                                                System.Threading.Thread.Sleep(20000);   // wait 20 seconds
                                                strSAN.Append(ExecuteSSH("zoneset name eng_cert vsan 1100", oSSHshella));
                                                strSAN.Append(ExecuteSSH("zone name " + strName + "a_vmax_0425_02g0", oSSHshella));
                                                strSAN.Append(ExecuteSSH("member device-alias " + strName + "a", oSSHshella));
                                                strSAN.Append(ExecuteSSH("member device-alias vmax_0425_02g0", oSSHshella));
                                                strSAN.Append(ExecuteSSH("exit", oSSHshella));
                                                strSAN.Append(ExecuteSSH("member " + strName + "a_vmax_0425_02g0", oSSHshella));
                                                strSAN.Append(ExecuteSSH("exit", oSSHshella));
                                                strSAN.Append(ExecuteSSH("zoneset activate name eng_cert vsan 1100", oSSHshella));
                                                System.Threading.Thread.Sleep(10000);   // wait 10 seconds
                                                strSAN.Append(ExecuteSSH("zone commit vsan 1100", oSSHshella));
                                                System.Threading.Thread.Sleep(20000);   // wait 20 seconds
                                                strSAN.Append(ExecuteSSH("end", oSSHshella));
                                                strSAN.Append(ExecuteSSH("exit", oSSHshella));
                                                oLog.AddEvent(intAnswer, strName, strSerial, "Disconnecting from 10.49.254.229...", LoggingType.Debug);
                                                //strMAC = oSolaris.ParseOutput(strMAC, "macaddress = ", Environment.NewLine);
                                            }
                                            oSSHshella.Close();


                                            oLog.AddEvent(intAnswer, strName, strSerial, "Connecting to 10.49.254.230...", LoggingType.Debug);
                                            SshShell oSSHshellb = new SshShell("10.49.254.230", "admin", "nccSAN03");
                                            oSSHshellb.RemoveTerminalEmulationCharacters = true;
                                            oSSHshellb.Connect();
                                            if (oSSHshellb.Connected == true && oSSHshellb.ShellOpened == true)
                                            {
                                                string strBanner = oSSHshellb.Expect("#");
                                                oLog.AddEvent(intAnswer, strName, strSerial, "Connected to 10.49.254.230...executing commands", LoggingType.Debug);
                                                strSAN.Append("10.49.254.230...");
                                                strSAN.Append(ExecuteSSH("config t", oSSHshellb));
                                                strSAN.Append(ExecuteSSH("device-alias database", oSSHshellb));
                                                strSAN.Append(ExecuteSSH("device-alias name " + strName + "b pwwn 50:06:0b:00:00:c3:5a:36", oSSHshellb));
                                                strSAN.Append(ExecuteSSH("exit", oSSHshellb));
                                                strSAN.Append(ExecuteSSH("device-alias commit", oSSHshellb));
                                                System.Threading.Thread.Sleep(20000);   // wait 20 seconds
                                                strSAN.Append(ExecuteSSH("zoneset name eng_cert vsan 1101", oSSHshellb));
                                                strSAN.Append(ExecuteSSH("zone name " + strName + "b_vmax_0425_01g0", oSSHshellb));
                                                strSAN.Append(ExecuteSSH("member device-alias " + strName + "b", oSSHshellb));
                                                strSAN.Append(ExecuteSSH("member device-alias vmax_0425_01g0", oSSHshellb));
                                                strSAN.Append(ExecuteSSH("exit", oSSHshellb));
                                                strSAN.Append(ExecuteSSH("member " + strName + "b_vmax_0425_01g0", oSSHshellb));
                                                strSAN.Append(ExecuteSSH("exit", oSSHshellb));
                                                strSAN.Append(ExecuteSSH("zoneset activate name eng_cert vsan 1101", oSSHshellb));
                                                System.Threading.Thread.Sleep(10000);   // wait 10 seconds
                                                strSAN.Append(ExecuteSSH("zone commit vsan 1101", oSSHshellb));
                                                System.Threading.Thread.Sleep(20000);   // wait 20 seconds
                                                strSAN.Append(ExecuteSSH("end", oSSHshellb));
                                                strSAN.Append(ExecuteSSH("exit", oSSHshellb));
                                                oLog.AddEvent(intAnswer, strName, strSerial, "Disconnecting from 10.49.254.230...", LoggingType.Debug);
                                                //strMAC = oSolaris.ParseOutput(strMAC, "macaddress = ", Environment.NewLine);
                                            }
                                            oSSHshellb.Close();


                                            strResult = strSAN.ToString();
                                        }
                                        catch (Exception exSAN)
                                        {
                                            strError = "There was a problem configuring the storage ~ (Error Message: " + exSAN.Message + ") ~ (Source: " + exSAN.Source + ") (Stack Trace: " + exSAN.StackTrace + ")";
                                        }

                                        if (strError == "")
                                        {
                                            // Execute the PERL scripts
                                            bool boolStorageTimeout = false;
                                            Variables oVarEng = new Variables((int)CurrentEnvironment.PNCENG);
                                            string strEngUser = oVarEng.Domain() + "\\" + oVarEng.ADUser();
                                            string strEngPass = oVarEng.ADPassword();

                                            if (boolStorageTimeout == false)
                                            {
                                                oLog.AddEvent(intAnswer, strName, strSerial, "Configuring storage # 1...", LoggingType.Debug);
                                                //psexec \\wcemc400a.pnceng.pvt -u pnceng\xacview -p *** -h -w C:\Perl64\bin cmd.exe /c  create_lun.pl " + strName + " 1 45 000195900425 1 50060b0000c35a34 50060b0000c35a36 test_1h1_2h1_pg
                                                string strStorageOutput = strFilePath + "StorageAuto1.txt";
                                                ProcessStartInfo infoStorage = new ProcessStartInfo(strScripts + "psexec");
                                                infoStorage.WorkingDirectory = strScripts;
                                                infoStorage.Arguments = @"\\wcemc400a.pnceng.pvt -u " + strEngUser + " -p " + strEngPass + @" -h -w C:\Perl64\bin cmd.exe /c create_lun_albert.pl W " + strName + " 1 60 000195900425 1 50060b0000c35a34 50060b0000c35a36";
                                                Process procStorage = Process.Start(infoStorage);
                                                procStorage.WaitForExit(intTimeoutDefault);
                                                if (procStorage.HasExited == false)
                                                {
                                                    procStorage.Kill();
                                                    boolStorageTimeout = true;
                                                }
                                                oLog.AddEvent(intAnswer, strName, strSerial, "Storage # 1 done (Timeout = " + boolStorageTimeout.ToString() + ")", LoggingType.Debug);
                                                procStorage.Close();
                                                //if (boolStorageTimeout == false)
                                                //    oFunction.ReadOutput(intServer, "StorageAuto1", strStorageOutput, strName, strSerial, intLogging, true);
                                            }
                                            if (boolStorageTimeout == false)
                                            {
                                                oLog.AddEvent(intAnswer, strName, strSerial, "Configuring storage # 2...", LoggingType.Debug);
                                                //psexec \\wcemc400a.pnceng.pvt -u pnceng\xacview -p *** -h -w C:\Perl64\bin cmd.exe /c  get_lun_serials.pl 000195900425 fc_raid53 " + strName + " 1 test_1h1_2h1_pg
                                                string strStorageOutput = strFilePath + "StorageAuto2.txt";
                                                ProcessStartInfo infoStorage = new ProcessStartInfo(strScripts + "psexec");
                                                infoStorage.WorkingDirectory = strScripts;
                                                infoStorage.Arguments = @"\\wcemc400a.pnceng.pvt -u " + strEngUser + " -p " + strEngPass + @" -h -w C:\Perl64\bin cmd.exe /c get_lun_serials_v2_albert.pl 000195900425 fc_raid53 " + strName + " 1 test_1g0_2g0_pg";
                                                Process procStorage = Process.Start(infoStorage);
                                                procStorage.WaitForExit(intTimeoutDefault);
                                                if (procStorage.HasExited == false)
                                                {
                                                    procStorage.Kill();
                                                    boolStorageTimeout = true;
                                                }
                                                oLog.AddEvent(intAnswer, strName, strSerial, "Storage # 2 done (Timeout = " + boolStorageTimeout.ToString() + ")", LoggingType.Debug);
                                                procStorage.Close();
                                                //if (boolStorageTimeout == false)
                                                //    oFunction.ReadOutput(intServer, "StorageAuto2", strStorageOutput, strName, strSerial, intLogging, true);
                                            }
                                            if (boolStorageTimeout == false)
                                            {
                                                oLog.AddEvent(intAnswer, strName, strSerial, "Configuring storage # 3...", LoggingType.Debug);
                                                //psexec \\wcemc400a.pnceng.pvt -u pnceng\xacview -p *** -h -w C:\Perl64\bin cmd.exe /c  create_lun.pl " + strName + " 1 10 000195900425 0 50060b0000c35a34 50060b0000c35a36 test_1h1_2h1_pg
                                                string strStorageOutput = strFilePath + "StorageAuto3.txt";
                                                ProcessStartInfo infoStorage = new ProcessStartInfo(strScripts + "psexec");
                                                infoStorage.WorkingDirectory = strScripts;
                                                infoStorage.Arguments = @"\\wcemc400a.pnceng.pvt -u " + strEngUser + " -p " + strEngPass + @" -h -w C:\Perl64\bin cmd.exe /c create_lun_albert.pl W " + strName + " 1 12 000195900425 0 50060b0000c35a34 50060b0000c35a36";
                                                Process procStorage = Process.Start(infoStorage);
                                                procStorage.WaitForExit(intTimeoutDefault);
                                                if (procStorage.HasExited == false)
                                                {
                                                    procStorage.Kill();
                                                    boolStorageTimeout = true;
                                                }
                                                oLog.AddEvent(intAnswer, strName, strSerial, "Storage # 3 done (Timeout = " + boolStorageTimeout.ToString() + ")", LoggingType.Debug);
                                                procStorage.Close();
                                                //if (boolStorageTimeout == false)
                                                //    oFunction.ReadOutput(intServer, "StorageAuto3", strStorageOutput, strName, strSerial, intLogging, true);
                                            }
                                            if (boolStorageTimeout == false)
                                            {
                                                oLog.AddEvent(intAnswer, strName, strSerial, "Configuring storage # 4...", LoggingType.Debug);
                                                //psexec \\wcemc400a.pnceng.pvt -u pnceng\xacview -p *** -h -w C:\Perl64\bin cmd.exe /c   get_lun_serials.pl 000195900425 fc_raid53 " + strName + " 0 test_1h1_2h1_pg
                                                string strStorageOutput = strFilePath + "StorageAuto4.txt";
                                                ProcessStartInfo infoStorage = new ProcessStartInfo(strScripts + "psexec");
                                                infoStorage.WorkingDirectory = strScripts;
                                                infoStorage.Arguments = @"\\wcemc400a.pnceng.pvt -u " + strEngUser + " -p " + strEngPass + @" -h -w C:\Perl64\bin cmd.exe /c get_lun_serials_add_albert.pl 000195900425 fc_raid53 " + strName;
                                                Process procStorage = Process.Start(infoStorage);
                                                procStorage.WaitForExit(intTimeoutDefault);
                                                if (procStorage.HasExited == false)
                                                {
                                                    procStorage.Kill();
                                                    boolStorageTimeout = true;
                                                }
                                                oLog.AddEvent(intAnswer, strName, strSerial, "Storage # 4 done (Timeout = " + boolStorageTimeout.ToString() + ")", LoggingType.Debug);
                                                procStorage.Close();
                                                //if (boolStorageTimeout == false)
                                                //    oFunction.ReadOutput(intServer, "StorageAuto4", strStorageOutput, strName, strSerial, intLogging, true);
                                            }

                                            Storage oStorage = new Storage(0, dsn);
                                            int storage = 4;
                                            DataSet dsStorage = oStorage.GetLuns(intAnswer, 0, intClusterID, 0, intNumber);
                                            foreach (DataRow drStorage in dsStorage.Tables[0].Rows)
                                            {
                                                double dblStorage = double.Parse(drStorage["size"].ToString()) + double.Parse(drStorage["size_qa"].ToString()) + double.Parse(drStorage["size_test"].ToString());
                                                if (boolStorageTimeout == false)
                                                {
                                                    storage++;
                                                    oLog.AddEvent(intAnswer, strName, strSerial, "Configuring storage # " + storage.ToString() + "...", LoggingType.Debug);
                                                    //psexec \\wcemc400a.pnceng.pvt -u pnceng\xacview -p *** -h -w C:\Perl64\bin cmd.exe /c  create_lun.pl " + strName + " 1 10 000195900425 0 50060b0000c35a34 50060b0000c35a36 test_1h1_2h1_pg
                                                    string strStorageOutput = strFilePath + "StorageAuto" + storage.ToString() + ".txt";
                                                    ProcessStartInfo infoStorage = new ProcessStartInfo(strScripts + "psexec");
                                                    infoStorage.WorkingDirectory = strScripts;
                                                    infoStorage.Arguments = @"\\wcemc400a.pnceng.pvt -u " + strEngUser + " -p " + strEngPass + @" -h -w C:\Perl64\bin cmd.exe /c create_lun_albert.pl W " + strName + " 1 " + dblStorage.ToString("0") + " 000195900425 0 50060b0000c35a34 50060b0000c35a36";
                                                    Process procStorage = Process.Start(infoStorage);
                                                    procStorage.WaitForExit(intTimeoutDefault);
                                                    if (procStorage.HasExited == false)
                                                    {
                                                        procStorage.Kill();
                                                        boolStorageTimeout = true;
                                                    }
                                                    oLog.AddEvent(intAnswer, strName, strSerial, "Storage # " + storage.ToString() + " done (Timeout = " + boolStorageTimeout.ToString() + ")", LoggingType.Debug);
                                                    procStorage.Close();
                                                    //if (boolStorageTimeout == false)
                                                    //    oFunction.ReadOutput(intServer, "StorageAuto3", strStorageOutput, strName, strSerial, intLogging, true);
                                                }
                                                if (boolStorageTimeout == false)
                                                {
                                                    storage++;
                                                    oLog.AddEvent(intAnswer, strName, strSerial, "Configuring storage # " + storage.ToString() + "...", LoggingType.Debug);
                                                    //psexec \\wcemc400a.pnceng.pvt -u pnceng\xacview -p *** -h -w C:\Perl64\bin cmd.exe /c   get_lun_serials.pl 000195900425 fc_raid53 " + strName + " 0 test_1h1_2h1_pg
                                                    string strStorageOutput = strFilePath + "StorageAuto" + storage.ToString() + ".txt";
                                                    ProcessStartInfo infoStorage = new ProcessStartInfo(strScripts + "psexec");
                                                    infoStorage.WorkingDirectory = strScripts;
                                                    infoStorage.Arguments = @"\\wcemc400a.pnceng.pvt -u " + strEngUser + " -p " + strEngPass + @" -h -w C:\Perl64\bin cmd.exe /c get_lun_serials_add_albert.pl 000195900425 fc_raid53 " + strName;
                                                    Process procStorage = Process.Start(infoStorage);
                                                    procStorage.WaitForExit(intTimeoutDefault);
                                                    if (procStorage.HasExited == false)
                                                    {
                                                        procStorage.Kill();
                                                        boolStorageTimeout = true;
                                                    }
                                                    oLog.AddEvent(intAnswer, strName, strSerial, "Storage # " + storage.ToString() + " done (Timeout = " + boolStorageTimeout.ToString() + ")", LoggingType.Debug);
                                                    procStorage.Close();
                                                    //if (boolStorageTimeout == false)
                                                    //    oFunction.ReadOutput(intServer, "StorageAuto4", strStorageOutput, strName, strSerial, intLogging, true);
                                                }
                                            }
                                        }
                                    }
                                    AddResult(intServer, intStep, intType, strResult, strError);
                                    break;
                                case 5:     // Configure Active Directory Accounts
                                    oLog.AddEvent(intAnswer, strName, strSerial, "Starting Step 5 (AD Accounts)", LoggingType.Information);
                                    if (intEnvironment == (int)CurrentEnvironment.CORPDMN || intEnvironment == (int)CurrentEnvironment.PNCNT_PROD)
                                    {
                                        if (oOperatingSystem.IsWindows(intOS) == true || oOperatingSystem.IsWindows2008(intOS) == true)
                                        {
                                            if (oDomain.Get(intDomain, "account_setup") == "1")
                                            {
                                                DataSet dsAccounts = oServer.GetAccounts(intServer);
                                                // Copy Accounts to Account Request
                                                if (boolPNC == true)
                                                {
                                                    string strMnemonicCode = oMnemonic.Get(intMnemonic, "factory_code").ToUpper();
                                                    string strNameGG = "GSGu_" + strMnemonicCode + "_";

                                                    foreach (DataRow drAccount in dsAccounts.Tables[0].Rows)
                                                    {
                                                        string strDomainGroups = "";
                                                        string[] strDomainGroupArray = drAccount["domaingroups"].ToString().Split(strSplit);
                                                        for (int ii = 0; ii < strDomainGroupArray.Length; ii++)
                                                        {
                                                            // Add Prefix and Remove Suffix ("_0" / "_1") for remote desktop
                                                            if (strDomainGroupArray[ii].Trim() != "")
                                                            {
                                                                string strDomainGroup = strDomainGroupArray[ii].Trim();
                                                                if (strDomainGroup.Contains("_") == true)
                                                                    strDomainGroup = strDomainGroup.Substring(0, strDomainGroup.IndexOf("_"));
                                                                if (strDomainGroups != "")
                                                                    strDomainGroups += ";";
                                                                strDomainGroups += strNameGG + strDomainGroup;
                                                            }
                                                        }
                                                        oAccountRequest.Add(intRequest, 0, 0, drAccount["xid"].ToString(), Int32.Parse(drAccount["domain"].ToString()), strDomainGroups, drAccount["localgroups"].ToString(), Int32.Parse(drAccount["email"].ToString()), 0);
                                                    }
                                                    strResult = "Active Directory Accounts Configured";
                                                }
                                                else
                                                {
                                                    foreach (DataRow drAccount in dsAccounts.Tables[0].Rows)
                                                        oAccountRequest.Add(intRequest, 0, 0, drAccount["xid"].ToString(), Int32.Parse(drAccount["domain"].ToString()), (drAccount["admin"].ToString() == "1" ? "GSGu_" + strName + "Adm" : ""), drAccount["localgroups"].ToString(), Int32.Parse(drAccount["email"].ToString()), 0);
                                                    strResult = "Active Directory Accounts Configured";
                                                }
                                            }
                                            else
                                            {
                                                strResult = "Active Directory Configuration is not available for this domain";
                                            }
                                        }
                                        else
                                        {
                                            strResult = "Active Directory Configuration Only Available for Distributed Builds";
                                        }
                                    }
                                    else
                                    {
                                        strResult = "Active Directory Configuration Only Available in PRODUCTION environment";
                                    }
                                    AddResult(intServer, intStep, intType, strResult, strError);
                                    break;
                                case 6:     // Configure ZEUS 
                                    oLog.AddEvent(intAnswer, strName, strSerial, "Starting Step 6 (Configure Image)", LoggingType.Information);
                                    oZeus.DeleteBuild(strSerial);
                                    oZeus.DeleteBuildName(strName);
                                    oZeus.DeleteApps(strSerial);
                                    oZeus.DeleteLuns(strSerial);
                                    oZeus.DeleteResults(strSerial);
                                    oZeus.AddLuns(intAnswer, dsn, dsnAsset);
                                    string strArrayConfig = "BASIC";
                                    if (boolPNC == true)
                                        strArrayConfig = "PNCBASIC30";
                                    if (dr["fdrive"].ToString() == "1" && oModelsProperties.IsStorageDE_FDriveCanBeLocal(intModel) == true)
                                        strArrayConfig = "SANCONNECTEDF";

                                    string strArrayConfigComponent = "";
                                    string strZeusBuildTypeComponent = "";
                                    bool boolIIS = false;
                                    if (dsComponents.Tables[0].Rows.Count > 0)
                                    {
                                        DataView dvComponents = dsComponents.Tables[0].DefaultView;

                                        // Get the array config selected from the components table
                                        dvComponents.Sort = "array_config_priority";
                                        if (oForecast.IsHACluster(intAnswer) == true)
                                            strArrayConfigComponent = dvComponents[0]["array_config_cluster"].ToString();
                                        else if (oForecast.IsStorage(intAnswer) == true && oForecast.GetAnswer(intAnswer, "storage") == "1")
                                            strArrayConfigComponent = dvComponents[0]["array_config_san"].ToString();
                                        else
                                            strArrayConfigComponent = dvComponents[0]["array_config"].ToString();

                                        if (dvComponents[0]["iis"].ToString() == "1")
                                            boolIIS = true;

                                        // Get the build type selected from the components table
                                        dvComponents.Sort = "build_type_priority";
                                        strZeusBuildTypeComponent = dvComponents[0]["build_type"].ToString();
                                    }

                                    // Query for WWPNs
                                    if (boolBlade == true || boolDell == true)
                                    {
                                        DataSet dsHBAs = oAsset.GetHBA(intAsset);
                                        if (dsHBAs.Tables[0].Rows.Count == 0)
                                        {
                                            oLog.AddEvent(intAnswer, strName, strSerial, "Querying for WWPNs (oAsset.GetVirtualConnectMACAddress) for AssetID " + intAsset.ToString(), LoggingType.Information);
                                            strError = oAsset.GetWWPNs(intAsset, intAnswer, intEnvironment, strScripts, dsn, strScripts, oLog, strName);
                                        }
                                        else
                                            oLog.AddEvent(intAnswer, strName, strSerial, "Skipping WWPN Query since there are already " + dsHBAs.Tables[0].Rows.Count.ToString() + " HBAs configured", LoggingType.Information);
                                    }
                                    else
                                        oLog.AddEvent(intAnswer, strName, strSerial, "Skipping WWPN Query since it is not a blade or dell", LoggingType.Information);

                                    if (strError == "")
                                    {
                                        strMACAddress1 = oAsset.GetServerOrBlade(intAsset, "macaddress").ToUpper();
                                        if (boolBlade == true || boolDell == true)
                                        {
                                            // Query virtual connect for appropriate MAC address
                                            oLog.AddEvent(intAnswer, strName, strSerial, "Querying for MAC Address (oAsset.GetVirtualConnectMACAddress) for AssetID " + intAsset.ToString(), LoggingType.Information);
                                            strMACAddress1 = oAsset.GetVirtualConnectMACAddress(intAsset, intAnswer, intEnvironment, 1, strScripts, dsn, strScripts, oLog, strName);
                                            if (strMACAddress1 == "**ERROR**")
                                            {
                                                strError = "There was a problem retrieving the MAC address of the blade";
                                                oLog.AddEvent(intAnswer, strName, strSerial, "There was a problem retrieving the MAC address of the blade ~ AssetID " + intAsset.ToString() + ". Output file: " + strScripts + intAsset.ToString() + ".txt", LoggingType.Error);
                                            }
                                            else
                                                oAsset.UpdateMac(intAsset, strMACAddress1);
                                            if (boolDell == true)
                                            {
                                                strMACAddress2 = oAsset.GetVirtualConnectMACAddress(intAsset, intAnswer, intEnvironment, 2, strScripts, dsn, strScripts, oLog, strName);
                                                if (strMACAddress2 == "**ERROR**")
                                                {
                                                    strError = "There was a problem retrieving the MAC address of the blade";
                                                    oLog.AddEvent(intAnswer, strName, strSerial, "There was a problem retrieving the MAC address (#2) of the blade ~ AssetID " + intAsset.ToString() + ". Output file: " + strScripts + intAsset.ToString() + ".txt", LoggingType.Error);
                                                }
                                            }
                                        }
                                        else
                                            oLog.AddEvent(intAnswer, strName, strSerial, "Skipping MAC Address Query since it is not a blade or dell", LoggingType.Information);
                                    }
                                    if (strError == "")
                                    {
                                        string strZeusMAC1 = oFunction.FormatMAC(strMACAddress1, ":");
                                        string strZeusMAC2 = "";
                                        if (strZeusMAC1 == "")
                                        {
                                            strError = "There is not MAC address configured for this device";
                                            oLog.AddEvent(intAnswer, strName, strSerial, "Invalid MAC address configured for this device ~ AssetID " + intAsset.ToString(), LoggingType.Error);
                                        }
                                        else
                                        {
                                            if (strMACAddress2 != "")
                                            {
                                                strZeusMAC2 = oFunction.FormatMAC(strMACAddress2, ":");
                                                if (strZeusMAC2 == "")
                                                {
                                                    strError = "There is not MAC address configured for this device";
                                                    oLog.AddEvent(intAnswer, strName, strSerial, "Invalid MAC address configured for this device ~ AssetID " + intAsset.ToString(), LoggingType.Error);
                                                }
                                            }
                                            // intClass, intEnv, intAddress, intResiliency, (boolRDPAltiris ? 1 : 0), (boolRDPMDT ? 1 : 0), 0, intZone
                                            oLog.AddEvent(intAnswer, strName, strSerial, "There are " + dsBuildRDP.Tables[0].Rows.Count.ToString() + " build locations (RDP) for class: " + oClass.Get(intClassRDP, "name") + " (" + intClassRDP.ToString() + "), env: " + oEnvironment.Get(intEnvRDP, "name") + " (" + intEnvRDP.ToString() + "), address: " + oLocation.GetFull(intAddressRDP) + " (" + intAddressRDP.ToString() + "), resiliency: " + oResiliency.Get(intResiliency, "name") + " (" + intResiliency.ToString() + "), RDPAltiris: " + boolRDPAltiris.ToString() + ", RDPMDT: " + boolRDPMDT.ToString() + ", Zone: " + intZone.ToString() + " for server ID " + intServer.ToString(), LoggingType.Information);
                                            if (dsBuildRDP.Tables[0].Rows.Count == 0)
                                                strError = "There are no RDP configurations for the selected environment";

                                            string strZeusOS = oOperatingSystem.Get(intOS, "zeus_os");
                                            string strZeusOSVersion = oOperatingSystem.Get(intOS, "zeus_os_version");
                                            string strZeusBuildTypeWindows = oOperatingSystem.Get(intOS, "zeus_build_type");

                                            if (strError == "")
                                            {
                                                foreach (DataRow drComponent in dsComponents.Tables[0].Rows)
                                                    oZeus.AddApp(strSerial, drComponent["zeus_code"].ToString(), drComponent["install_path"].ToString());

                                                if (oOperatingSystem.IsWindows(intOS) == true || oOperatingSystem.IsWindows2008(intOS) == true)
                                                {
                                                    if (boolPNC == true)
                                                    {
                                                        strZeusBuildTypeWindows = oOperatingSystem.Get(intOS, "zeus_build_type_pnc");
                                                        if (dsBuildRDP.Tables[0].Rows.Count > 0 && dsBuildRDP.Tables[0].Rows[0]["source"].ToString() != "")
                                                            strSource = dsBuildRDP.Tables[0].Rows[0]["source"].ToString();
                                                        else
                                                            strSource = "DALRDP";
                                                    }
                                                    else
                                                    {
                                                        if (dsBuildRDP.Tables[0].Rows.Count > 0 && dsBuildRDP.Tables[0].Rows[0]["source"].ToString() != "")
                                                            strSource = dsBuildRDP.Tables[0].Rows[0]["source"].ToString();
                                                        else
                                                            strSource = "SERVER";
                                                    }
                                                    if (strArrayConfigComponent != "")
                                                        strArrayConfig = strArrayConfigComponent;
                                                    if (strZeusBuildTypeComponent != "")
                                                        strZeusBuildTypeWindows = strZeusBuildTypeComponent;
                                                    if (intApplication > 0)
                                                    {
                                                        if (oServerName.GetApplication(intApplication, "zeus_array_config") != "")
                                                            strArrayConfig = oServerName.GetApplication(intApplication, "zeus_array_config");
                                                        if (oServerName.GetApplication(intApplication, "zeus_os") != "")
                                                            strZeusOS = oServerName.GetApplication(intApplication, "zeus_os");
                                                        if (oServerName.GetApplication(intApplication, "zeus_os_version") != "")
                                                            strZeusOSVersion = oServerName.GetApplication(intApplication, "zeus_os_version");
                                                        if (oServerName.GetApplication(intApplication, "zeus_build_type") != "")
                                                            strZeusBuildTypeWindows = oServerName.GetApplication(intApplication, "zeus_build_type");
                                                    }
                                                    if (intSubApplication > 0)
                                                    {
                                                        if (oServerName.GetSubApplication(intSubApplication, "zeus_array_config") != "")
                                                            strArrayConfig = oServerName.GetSubApplication(intSubApplication, "zeus_array_config");
                                                        if (oServerName.GetSubApplication(intSubApplication, "zeus_os") != "")
                                                            strZeusOS = oServerName.GetSubApplication(intSubApplication, "zeus_os");
                                                        if (oServerName.GetSubApplication(intSubApplication, "zeus_os_version") != "")
                                                            strZeusOSVersion = oServerName.GetSubApplication(intSubApplication, "zeus_os_version");
                                                        if (oServerName.GetSubApplication(intSubApplication, "zeus_build_type") != "")
                                                            strZeusBuildTypeWindows = oServerName.GetSubApplication(intSubApplication, "zeus_build_type");
                                                    }
                                                    strError = oZeus.AddBuild(intServer, 0, 0, strSerial, strAsset, strName.ToLower(), strArrayConfig, strZeusOS, strZeusOSVersion, Int32.Parse(oServicePack.Get(intSP, "number")), strZeusBuildTypeWindows, strDomain, intEnvironment, strSource, 0, strZeusMAC1, strZeusMAC2, "", "", "", "", "", "", "", "", "", "", "", "", "", "", 1);
                                                    if (strError == "")
                                                    {
                                                        if (strZeusMAC2 != "")
                                                            strError = oZeus.AddBuild(intServer, 0, 0, strSerial, strAsset, strName.ToLower(), strArrayConfig, strZeusOS, strZeusOSVersion, Int32.Parse(oServicePack.Get(intSP, "number")), strZeusBuildTypeWindows, strDomain, intEnvironment, strSource, 0, strZeusMAC2, strZeusMAC1, "", "", "", "", "", "", "", "", "", "", "", "", "", "", 1);
                                                        oLog.AddEvent(intAnswer, strName, strSerial, "Record added to Imaging table", LoggingType.Information);
                                                    }
                                                }
                                                else if (oOperatingSystem.IsLinux(intOS) == true)
                                                {
                                                    string strIPZeus = "";
                                                    string strIPVLAN = "";
                                                    string strIPmask = "";
                                                    string strIPgateway = "";
                                                    if (intIPAddressBuild1 > 0)
                                                    {
                                                        strIPZeus = oIPAddresses.GetName(intIPAddressBuild1, 0);
                                                        int intIPNetwork = Int32.Parse(oIPAddresses.Get(intIPAddressBuild1, "networkid"));
                                                        strIPmask = oIPAddresses.GetNetwork(intIPNetwork, "mask");
                                                        strIPgateway = oIPAddresses.GetNetwork(intIPNetwork, "gateway");
                                                        int intIPVlan = Int32.Parse(oIPAddresses.GetNetwork(intIPNetwork, "vlanid"));
                                                        strIPVLAN = oIPAddresses.GetVlan(intIPVlan, "vlan");
                                                    }
                                                    string strIPZeusBackup = "";
                                                    string strIPBackupVLAN = "";
                                                    string strIPBackupMask = "";
                                                    string strIPBackupGateway = "";
                                                    if (intIPAddressBackup > 0)
                                                    {
                                                        strIPZeusBackup = oIPAddresses.GetName(intIPAddressBackup, 0);
                                                        int intIPNetwork = Int32.Parse(oIPAddresses.Get(intIPAddressBackup, "networkid"));
                                                        strIPBackupMask = oIPAddresses.GetNetwork(intIPNetwork, "mask");
                                                        strIPBackupGateway = oIPAddresses.GetNetwork(intIPNetwork, "gateway");
                                                        int intIPVlan = Int32.Parse(oIPAddresses.GetNetwork(intIPNetwork, "vlanid"));
                                                        strIPBackupVLAN = oIPAddresses.GetVlan(intIPVlan, "vlan");
                                                    }
                                                    string strZeusBuildTypeLinux = "";
                                                    if (strArrayConfigComponent != "")
                                                        strArrayConfig = strArrayConfigComponent;
                                                    if (strZeusBuildTypeComponent != "")
                                                        strZeusBuildTypeLinux = strZeusBuildTypeComponent;
                                                    if (boolPNC == true)
                                                    {
                                                        if (oEnvironment.Get(intEnv, "ecom") == "1")
                                                        {
                                                            strError = oZeus.AddBuild(intServer, 0, 0, strSerial, strAsset, strName.ToLower(), strArrayConfig, "", "", 0, strZeusBuildTypeLinux, "PNCETECH", intEnvironment, "SERVER", 0, strZeusMAC1, strZeusMAC2, strIPZeus, (boolBlade ? strIPVLAN : ""), strIPmask, strIPgateway, strIPZeusBackup, (boolBlade ? strIPBackupVLAN : ""), strIPBackupMask, strIPBackupGateway, "", "", "", "", "", "", 1);
                                                            if (strError == "" && strZeusMAC2 != "")
                                                                strError = oZeus.AddBuild(intServer, 0, 0, strSerial, strAsset, strName.ToLower(), strArrayConfig, "", "", 0, strZeusBuildTypeLinux, "PNCETECH", intEnvironment, "SERVER", 0, strZeusMAC2, strZeusMAC1, strIPZeus, (boolBlade ? strIPVLAN : ""), strIPmask, strIPgateway, strIPZeusBackup, (boolBlade ? strIPBackupVLAN : ""), strIPBackupMask, strIPBackupGateway, "", "", "", "", "", "", 1);
                                                        }
                                                        else
                                                        {
                                                            strError = oZeus.AddBuild(intServer, 0, 0, strSerial, strAsset, strName.ToLower(), strArrayConfig, "", "", 0, strZeusBuildTypeLinux, strDomain, intEnvironment, "SERVER", 0, strZeusMAC1, strZeusMAC2, strIPZeus, (boolBlade ? strIPVLAN : ""), strIPmask, strIPgateway, strIPZeusBackup, (boolBlade ? strIPBackupVLAN : ""), strIPBackupMask, strIPBackupGateway, "", "", "", "", "", "", 1);
                                                            if (strError == "" && strZeusMAC2 != "")
                                                                strError = oZeus.AddBuild(intServer, 0, 0, strSerial, strAsset, strName.ToLower(), strArrayConfig, "", "", 0, strZeusBuildTypeLinux, strDomain, intEnvironment, "SERVER", 0, strZeusMAC2, strZeusMAC1, strIPZeus, (boolBlade ? strIPVLAN : ""), strIPmask, strIPgateway, strIPZeusBackup, (boolBlade ? strIPBackupVLAN : ""), strIPBackupMask, strIPBackupGateway, "", "", "", "", "", "", 1);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        strError = oZeus.AddBuild(intServer, 0, 0, strSerial, strAsset, strName.ToLower(), strArrayConfig, "", "", 0, strZeusBuildTypeLinux, strDomain, intEnvironment, "SERVER", 0, strZeusMAC1, strZeusMAC2, strIPZeus, (boolBlade ? strIPVLAN : ""), strIPmask, strIPgateway, strIPZeusBackup, (boolBlade ? strIPBackupVLAN : ""), strIPBackupMask, strIPBackupGateway, "", "", "", "", "", "", 1);
                                                        if (strError == "" && strZeusMAC2 != "")
                                                            strError = oZeus.AddBuild(intServer, 0, 0, strSerial, strAsset, strName.ToLower(), strArrayConfig, "", "", 0, strZeusBuildTypeLinux, strDomain, intEnvironment, "SERVER", 0, strZeusMAC2, strZeusMAC1, strIPZeus, (boolBlade ? strIPVLAN : ""), strIPmask, strIPgateway, strIPZeusBackup, (boolBlade ? strIPBackupVLAN : ""), strIPBackupMask, strIPBackupGateway, "", "", "", "", "", "", 1);
                                                    }
                                                    oLog.AddEvent(intAnswer, strName, strSerial, "Record added to Imaging table", LoggingType.Information);
                                                }
                                                else if (oOperatingSystem.IsSolaris(intOS) == true)
                                                {
                                                    oLog.AddEvent(intAnswer, strName, strSerial, "There are " + dsBuildRDP.Tables[0].Rows.Count.ToString() + " build locations (RDP) for class: " + oClass.Get(intClassRDP, "name") + " (" + intClassRDP.ToString() + "), env: " + oEnvironment.Get(intEnvRDP, "name") + " (" + intEnvRDP.ToString() + "), address: " + oLocation.GetFull(intAddressRDP) + " (" + intAddressRDP.ToString() + "), resiliency: " + oResiliency.Get(intResiliency, "name") + " (" + intResiliency.ToString() + "), RDPAltiris: " + boolRDPAltiris.ToString() + ", RDPMDT: " + boolRDPMDT.ToString() + ", Zone: " + intZone.ToString() + " for server ID " + intServer.ToString(), LoggingType.Information);
                                                    string strSolarisMAC = oFunction.FormatMAC(strMACAddress1, ":");
                                                    if (strSolarisMAC == "")
                                                    {
                                                        strError = "Invalid MAC Address ~ the MAC address is not in the proper format (" + strMACAddress1 + ")";
                                                        oLog.AddEvent(intAnswer, strName, strSerial, strError, LoggingType.Error);
                                                    }
                                                    else
                                                    {
                                                        strSolarisMAC = strSolarisMAC.ToLower();
                                                        string strJumpstartCGI = "";
                                                        string strJumpstartBuildType = "";
                                                        if (dsBuildRDP.Tables[0].Rows.Count > 0)
                                                        {
                                                            strSource = dsBuildRDP.Tables[0].Rows[0]["source"].ToString();
                                                            strJumpstartCGI = dsBuildRDP.Tables[0].Rows[0]["jumpstart_cgi"].ToString();
                                                            strJumpstartBuildType = dsBuildRDP.Tables[0].Rows[0]["jumpstart_build_type"].ToString();
                                                        }
                                                        if (strSource == "")
                                                        {
                                                            strError = "Invalid SOURCE field for Jumpstart build location (RDP) ~ class: " + oClass.Get(intClass, "name") + ", address: " + oLocation.GetFull(intAddress) + " for server ID " + intServer.ToString();
                                                            oLog.AddEvent(intAnswer, strName, strSerial, strError, LoggingType.Error);
                                                        }
                                                        if (strError == "" && strJumpstartCGI == "")
                                                        {
                                                            strError = "Invalid JUMPSTART CGI field for Jumpstart build location (RDP) ~ class: " + oClass.Get(intClass, "name") + ", address: " + oLocation.GetFull(intAddress) + " for server ID " + intServer.ToString();
                                                            oLog.AddEvent(intAnswer, strName, strSerial, strError, LoggingType.Error);
                                                        }
                                                        if (strError == "" && strJumpstartBuildType == "")
                                                        {
                                                            strError = "Invalid JUMPSTART BUILD TYPE field for Jumpstart build location (RDP) ~ class: " + oClass.Get(intClass, "name") + ", address: " + oLocation.GetFull(intAddress) + " for server ID " + intServer.ToString();
                                                            oLog.AddEvent(intAnswer, strName, strSerial, strError, LoggingType.Error);
                                                        }
                                                        if (strError == "")
                                                        {
                                                            string strIPZeus1 = "";
                                                            string strIPVLAN = "";
                                                            string strIPmask = "";
                                                            string strIPgateway = "";
                                                            if (intIPAddressBuild1 > 0)
                                                            {
                                                                strIPZeus1 = oIPAddresses.GetName(intIPAddressBuild1, 0);
                                                                int intIPNetwork = Int32.Parse(oIPAddresses.Get(intIPAddressBuild1, "networkid"));
                                                                strIPmask = oIPAddresses.GetNetwork(intIPNetwork, "mask");
                                                                strIPgateway = oIPAddresses.GetNetwork(intIPNetwork, "gateway");
                                                                int intIPVlan = Int32.Parse(oIPAddresses.GetNetwork(intIPNetwork, "vlanid"));
                                                                strIPVLAN = oIPAddresses.GetVlan(intIPVlan, "vlan");
                                                            }
                                                            string strIPZeus2 = "";
                                                            if (intIPAddressBuild2 > 0)
                                                                strIPZeus2 = oIPAddresses.GetName(intIPAddressBuild2, 0);
                                                            string strIPZeus3 = "";
                                                            if (intIPAddressBuild3 > 0)
                                                                strIPZeus3 = oIPAddresses.GetName(intIPAddressBuild3, 0);
                                                            string strIPZeusBackup = "";
                                                            string strIPBackupVLAN = "";
                                                            string strIPBackupMask = "";
                                                            string strIPBackupGateway = "";
                                                            if (intIPAddressBackup > 0)
                                                            {
                                                                strIPZeusBackup = oIPAddresses.GetName(intIPAddressBackup, 0);
                                                                int intIPNetwork = Int32.Parse(oIPAddresses.Get(intIPAddressBackup, "networkid"));
                                                                strIPBackupMask = oIPAddresses.GetNetwork(intIPNetwork, "mask");
                                                                strIPBackupGateway = oIPAddresses.GetNetwork(intIPNetwork, "gateway");
                                                                int intIPVlan = Int32.Parse(oIPAddresses.GetNetwork(intIPNetwork, "vlanid"));
                                                                strIPBackupVLAN = oIPAddresses.GetVlan(intIPVlan, "vlan");
                                                            }
                                                            string strZeusBuildTypeSolaris = strSolarisBuildType;
                                                            if (intAddress == 1674 || intAddress == 1675)    // Pittsburgh
                                                                strSource = "PITTSBURGH";
                                                            else if (strSource == "CLERDP" || strSource == "SERVER")
                                                                strSource = "CLEVELAND";
                                                            else if (strSource == "DALRDP")
                                                                strSource = "DALTON";
                                                            else
                                                                strSource = "???" + strSource;
                                                            string strSolarisBuildFlags = "";
                                                            if (boolPNC == true)
                                                                strSolarisBuildFlags += (oModelsProperties.IsStorageDB_BootLocal(intModel) ? "P,T,D" : "P");
                                                            else
                                                                strSolarisBuildFlags += "N";
                                                            if (oEnvironment.Get(intEnv, "ecom") == "1")
                                                                strSolarisBuildFlags += ",E";
                                                            string strSolarisDomain = oClass.Get(intClass, "domain").Trim();
                                                            if (strSolarisDomain == "")
                                                                strSolarisDomain = oEnvironment.Get(intEnv, (boolPNC ? "pnc_domain" : "ncb_domain")).Trim();
                                                            if (strArrayConfigComponent != "")
                                                                strArrayConfig = strArrayConfigComponent;
                                                            if (strZeusBuildTypeComponent != "")
                                                                strZeusBuildTypeSolaris = strZeusBuildTypeComponent;
                                                            if (intApplication > 0)
                                                            {
                                                                if (oServerName.GetApplication(intApplication, "zeus_array_config") != "")
                                                                    strArrayConfig = oServerName.GetApplication(intApplication, "zeus_array_config");
                                                                if (oServerName.GetApplication(intApplication, "zeus_os") != "")
                                                                    strZeusOS = oServerName.GetApplication(intApplication, "zeus_os");
                                                                if (oServerName.GetApplication(intApplication, "zeus_os_version") != "")
                                                                    strZeusOSVersion = oServerName.GetApplication(intApplication, "zeus_os_version");
                                                                if (oServerName.GetApplication(intApplication, "zeus_build_type") != "")
                                                                    strZeusBuildTypeSolaris = oServerName.GetApplication(intApplication, "zeus_build_type");
                                                            }
                                                            if (intSubApplication > 0)
                                                            {
                                                                if (oServerName.GetSubApplication(intSubApplication, "zeus_array_config") != "")
                                                                    strArrayConfig = oServerName.GetSubApplication(intSubApplication, "zeus_array_config");
                                                                if (oServerName.GetSubApplication(intSubApplication, "zeus_os") != "")
                                                                    strZeusOS = oServerName.GetSubApplication(intSubApplication, "zeus_os");
                                                                if (oServerName.GetSubApplication(intSubApplication, "zeus_os_version") != "")
                                                                    strZeusOSVersion = oServerName.GetSubApplication(intSubApplication, "zeus_os_version");
                                                                if (oServerName.GetSubApplication(intSubApplication, "zeus_build_type") != "")
                                                                    strZeusBuildTypeSolaris = oServerName.GetSubApplication(intSubApplication, "zeus_build_type");
                                                            }
                                                            // Add to ZEUS
                                                            string strJumpstartCGIZeus = strJumpstartCGI;
                                                            if (strJumpstartCGIZeus.Contains(":") == true)
                                                                strJumpstartCGIZeus = strJumpstartCGIZeus.Substring(0, strJumpstartCGIZeus.IndexOf(":"));
                                                            strError = oZeus.AddBuild(intServer, 0, 0, strSerial, strAsset, strName.ToLower(), strArrayConfig, strZeusOS, strZeusOSVersion, 0, strZeusBuildTypeSolaris, strSolarisDomain, intEnvironment, strSource, 0, strSolarisMAC, "", strIPZeus1, strIPVLAN, strIPmask, strIPgateway, strIPZeusBackup, strIPBackupVLAN, strIPBackupMask, strIPBackupGateway, strSolarisInterface1, strIPZeus2, strSolarisInterface2, strIPZeus3, strSolarisBuildFlags, oVariable.SolarisFlags(strJumpstartCGIZeus, strSolarisBuildNetwork, strJumpstartBuildType, false), 1);
                                                            if (strError == "")
                                                            {
                                                                oLog.AddEvent(intAnswer, strName, strSerial, "Record added to Imaging table", LoggingType.Information);

                                                                if (intEnvironment == (int)CurrentEnvironment.CORPDMN || intEnvironment == (int)CurrentEnvironment.PNCNT_PROD)
                                                                {
                                                                    // Cleanup JUMPSTART from previous build(s)
                                                                    string strJumpstartURL = "http://" + strJumpstartCGI + "/cgi-bin/jumpstart.cgi?1010,MACADDR=" + strSolarisMAC + ",remove_client";
                                                                    oLog.AddEvent(intAnswer, strName, strSerial, "Beginning Client Removal on jumpstart URL = " + strJumpstartURL, LoggingType.Information);
                                                                    if (RunCGI(strJumpstartURL) == true)
                                                                    {
                                                                        oLog.AddEvent(intAnswer, strName, strSerial, "Success!! Client Removal on jumpstart URL = " + strJumpstartURL, LoggingType.Information);
                                                                    }
                                                                    else
                                                                    {
                                                                        strError = "REMOVE_CLIENT registration ~ Jumpstart URL = " + strJumpstartURL + " failed.";
                                                                        oLog.AddEvent(intAnswer, strName, strSerial, "Failure!! Client Removal on jumpstart URL = " + strJumpstartURL, LoggingType.Error);
                                                                    }
                                                                }

                                                                // Pre-configure JUMPSTART
                                                                if (strError == "")
                                                                {
                                                                    if (intEnvironment == (int)CurrentEnvironment.CORPDMN || intEnvironment == (int)CurrentEnvironment.PNCNT_PROD)
                                                                    {
                                                                        string strJumpstartURL = "http://" + strJumpstartCGI + "/cgi-bin/jumpstart.cgi?1010,MACADDR=" + strSolarisMAC + ",create_host_info";
                                                                        oLog.AddEvent(intAnswer, strName, strSerial, "Beginning HOST.INFO registration on jumpstart URL = " + strJumpstartURL, LoggingType.Information);
                                                                        if (RunCGI(strJumpstartURL) == true)
                                                                        {
                                                                            oLog.AddEvent(intAnswer, strName, strSerial, "Success!! HOST.INFO registration on jumpstart URL = " + strJumpstartURL, LoggingType.Information);
                                                                        }
                                                                        else
                                                                        {
                                                                            strError = "CREATE_HOST_INFO registration ~ Jumpstart URL = " + strJumpstartURL + " failed.";
                                                                            oLog.AddEvent(intAnswer, strName, strSerial, "Failure!! HOST.INFO registration on jumpstart URL = " + strJumpstartURL, LoggingType.Error);
                                                                        }
                                                                    }
                                                                }
                                                            }

                                                            if (strError == "")
                                                            {
                                                                if (intEnvironment == (int)CurrentEnvironment.CORPDMN || intEnvironment == (int)CurrentEnvironment.PNCNT_PROD)
                                                                {
                                                                    string strJumpstartURL = "http://" + strJumpstartCGI + "/cgi-bin/jumpstart.cgi?1010,MACADDR=" + strSolarisMAC + ",add_client";
                                                                    oLog.AddEvent(intAnswer, strName, strSerial, "Beginning Client Configuration on jumpstart URL = " + strJumpstartURL, LoggingType.Information);
                                                                    if (RunCGI(strJumpstartURL) == true)
                                                                    {
                                                                        oLog.AddEvent(intAnswer, strName, strSerial, "Success!! Client Configuration on jumpstart URL = " + strJumpstartURL, LoggingType.Information);
                                                                    }
                                                                    else
                                                                    {
                                                                        strError = "ADD_CLIENT registration ~ Jumpstart URL = " + strJumpstartURL + " failed.";
                                                                        oLog.AddEvent(intAnswer, strName, strSerial, "Failure!! Client Configuration on jumpstart URL = " + strJumpstartURL, LoggingType.Error);
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    strError = "Invalid Operating System for On-Demand ~ (" + strOS + ")";
                                                    oLog.AddEvent(intAnswer, strName, strSerial, strError, LoggingType.Error);
                                                }
                                            }

                                            if (strError == "")
                                            {
                                                if (boolRDPMDT)
                                                {
                                                    // Add to MDT
                                                    string strRDPMDTWebService = dsBuildRDP.Tables[0].Rows[0]["rdp_mdt_ws"].ToString();
                                                    System.Net.NetworkCredential oCredentials = new System.Net.NetworkCredential(oVariable.ADUser(), oVariable.ADPassword(), oVariable.Domain());
                                                    BuildSubmit oMDT = new BuildSubmit();
                                                    oMDT.Credentials = oCredentials;
                                                    oMDT.Url = strRDPMDTWebService;
                                                    oLog.AddEvent(intAnswer, strName, strSerial, "Running ForceCleanup MDT on " + strRDPMDTWebService + " (" + strName + ", " + strZeusMAC1 + ", " + "ServerShare" + ")", LoggingType.Information);
                                                    oMDT.ForceCleanup(strName, strZeusMAC1, "ServerShare");
                                                    if (strZeusMAC2 != "")
                                                    {
                                                        oLog.AddEvent(intAnswer, strName, strSerial, "Waiting 60 seconds for Active Directory to Sychronize...", LoggingType.Debug);
                                                        Thread.Sleep(60000); // Wait 60 seconds
                                                        oLog.AddEvent(intAnswer, strName, strSerial, "Running ForceCleanup MDT on " + strRDPMDTWebService + " (" + strName + ", " + strZeusMAC2 + ", " + "ServerShare" + ")", LoggingType.Information);
                                                        oMDT.ForceCleanup(strName, strZeusMAC2, "ServerShare");
                                                    }
                                                    string strBackup = "NONE";
                                                    if (oForecast.GetAnswer(intAnswer, "backup") == "1")
                                                    {
                                                        strBackup = (intTSM == 1 ? "TSM" : "Legato");
                                                        if (boolAvamar == true)
                                                            strBackup = "Avamar";
                                                    }
                                                    string strHIDs = "NO";
                                                    string strMnemonicCode = oMnemonic.Get(intMnemonic, "factory_code");
                                                    string strAppRating = oMnemonic.GetFeed(strMnemonicCode, MnemonicFeed.AppRating);
                                                    if (strAppRating == "")
                                                        strAppRating = oMnemonic.Get(intMnemonic, "AppRating");
                                                    if (strAppRating.ToUpper().Contains("SOX"))
                                                    {
                                                        if (oClass.IsDev(intClass) == false)
                                                            strHIDs = "YES";
                                                    }
                                                    //string[] strExtendedMDT1 = new string[] { "PNCBACKUPSOFTWARE:" + strBackup, "IISInstall:" + (boolIIS ? "YES" : "NO"), "HWConfig:" + (boolDell && oModelsProperties.IsStorageDB_BootLocal(intModel) ? "DellLD" : "DEFAULT"), "ESMInstall:YES", "ClearViewInstall:YES", "Teamed2:" + strZeusMAC2 };
                                                    string[] strExtendedMDT1 = new string[] { "PNCBACKUPSOFTWARE:" + strBackup, "IISInstall:" + (boolIIS ? "YES" : "NO"), "HWConfig:" + (boolDell && oModelsProperties.IsStorageDB_BootLocal(intModel) ? "DellLD" : "DEFAULT"), "ESMInstall:YES", "ClearViewInstall:YES", "Teamed2:" + strZeusMAC2, "HIDSInstall:" + strHIDs };
                                                    string strExtendedMDTs1 = "";
                                                    foreach (string extendedMDT in strExtendedMDT1)
                                                    {
                                                        if (strExtendedMDTs1 != "")
                                                            strExtendedMDTs1 += ", ";
                                                        strExtendedMDTs1 += extendedMDT;
                                                    }
                                                    oLog.AddEvent(intAnswer, strName, strSerial, "Registering MDT on " + strRDPMDTWebService + ", automatedBuild2 (" + strName + ", " + strZeusMAC1 + ", " + strBootEnvironment + ", " + "provision" + ", " + strDomain + ", " + strTaskSequence + ", " + "ServerShare" + ", ExtendedValues=" + strExtendedMDTs1 + ")", LoggingType.Information);
                                                    oMDT.automatedBuild2(strName, strZeusMAC1, strBootEnvironment, "provision", strDomain, strTaskSequence, "ServerShare", strExtendedMDT1);
                                                    if (strZeusMAC2 != "")
                                                    {
                                                        //string[] strExtendedMDT2 = new string[] { "PNCBACKUPSOFTWARE:" + strBackup, "IISInstall:" + (boolIIS ? "YES" : "NO"), "HWConfig:" + (boolDell && oModelsProperties.IsStorageDB_BootLocal(intModel) ? "DellLD" : "DEFAULT"), "ESMInstall:YES", "ClearViewInstall:YES", "Teamed2:" + strZeusMAC1 };
                                                        string[] strExtendedMDT2 = new string[] { "PNCBACKUPSOFTWARE:" + strBackup, "IISInstall:" + (boolIIS ? "YES" : "NO"), "HWConfig:" + (boolDell && oModelsProperties.IsStorageDB_BootLocal(intModel) ? "DellLD" : "DEFAULT"), "ESMInstall:YES", "ClearViewInstall:YES", "Teamed2:" + strZeusMAC1, "HIDSInstall:" + strHIDs };
                                                        string strExtendedMDTs2 = "";
                                                        foreach (string extendedMDT in strExtendedMDT2)
                                                        {
                                                            if (strExtendedMDTs2 != "")
                                                                strExtendedMDTs2 += ", ";
                                                            strExtendedMDTs2 += extendedMDT;
                                                        }
                                                        oLog.AddEvent(intAnswer, strName, strSerial, "Waiting 60 seconds for Active Directory to Sychronize...", LoggingType.Debug);
                                                        Thread.Sleep(60000); // Wait 60 seconds
                                                        oLog.AddEvent(intAnswer, strName, strSerial, "Registering MDT on " + strRDPMDTWebService + ", automatedBuild2 (" + strName + ", " + strZeusMAC2 + ", " + strBootEnvironment + ", " + "provision" + ", " + strDomain + ", " + strTaskSequence + ", " + "ServerShare" + ", ExtendedValues=" + strExtendedMDTs2 + ")", LoggingType.Information);
                                                        oMDT.automatedBuild2(strName, strZeusMAC2, strBootEnvironment, "provision", strDomain, strTaskSequence, "ServerShare", strExtendedMDT2);
                                                    }
                                                    oLog.AddEvent(intAnswer, strName, strSerial, "Waiting 60 seconds for Active Directory to Sychronize...", LoggingType.Debug);
                                                    Thread.Sleep(60000); // Wait 60 seconds
                                                    oLog.AddEvent(intAnswer, strName, strSerial, "MDT has been configured", LoggingType.Information);
                                                }
                                            }

                                            if (strError == "")
                                            {
                                                // Update table with DHCP address of 0
                                                oServer.UpdateZeus(intServer);
                                                strResult = "The image has been configured";
                                            }
                                        }
                                    }
                                    AddResult(intServer, intStep, intType, strResult, strError);
                                    break;
                                case 7:     // Power on 
                                    oLog.AddEvent(intAnswer, strName, strSerial, "Starting Step 7 (Power on)", LoggingType.Information);

                                    // Query the RDP locations for WebServices and VLAN
                                    string strRDPComputerWebService = "";
                                    string strRDPScheduleWebService = "";
                                    oLog.AddEvent(intAnswer, strName, strSerial, "There are " + dsBuildRDP.Tables[0].Rows.Count.ToString() + " build locations (RDP) for class: " + oClass.Get(intClassRDP, "name") + " (" + intClassRDP.ToString() + "), env: " + oEnvironment.Get(intEnvRDP, "name") + " (" + intEnvRDP.ToString() + "), address: " + oLocation.GetFull(intAddressRDP) + " (" + intAddressRDP.ToString() + "), resiliency: " + oResiliency.Get(intResiliency, "name") + " (" + intResiliency.ToString() + "), RDPAltiris: " + boolRDPAltiris.ToString() + ", RDPMDT: " + boolRDPMDT.ToString() + ", Zone: " + intZone.ToString() + " for server ID " + intServer.ToString(), LoggingType.Information);
                                    if (dsBuildRDP.Tables[0].Rows.Count > 0)
                                    {
                                        strRDPComputerWebService = dsBuildRDP.Tables[0].Rows[0]["rdp_computer_ws"].ToString();
                                        strRDPScheduleWebService = dsBuildRDP.Tables[0].Rows[0]["rdp_schedule_ws"].ToString();
                                        if (boolIsNexusSwitch == false)
                                            strRDPVLAN = dsBuildRDP.Tables[0].Rows[0]["blade_vlan"].ToString();
                                        else
                                            strRDPVLAN = dsBuildRDP.Tables[0].Rows[0]["dell_vlan"].ToString();
                                    }
                                    else
                                    {
                                        strError = "Invalid build location (RDP) configuration ~ Class: " + oClass.Get(intClass, "name") + ", Location: " + oLocation.GetFull(intAddress);
                                        oLog.AddEvent(intAnswer, strName, strSerial, strError, LoggingType.Error);
                                    }

                                    if (strError == "")
                                    {
                                        if (strILO == "")
                                        {
                                            strError = "The ILO IP Address is missing ~ Asset ID " + intAsset.ToString();
                                            oLog.AddEvent(intAnswer, strName, strSerial, strError, LoggingType.Error);
                                        }
                                    }

                                    if (strError == "")
                                    {
                                        bool boolPingILO = false;
                                        for (int ii = 0; ii < 3 && boolPingILO == false; ii++)
                                        {
                                            Thread.Sleep(3000);
                                            Ping oPingChange = new Ping();
                                            string strStatusChange = "";
                                            try
                                            {
                                                PingReply oReplyChange = oPingChange.Send(strILO);
                                                strStatusChange = oReplyChange.Status.ToString().ToUpper();
                                            }
                                            catch { }
                                            boolPingILO = (strStatusChange == "SUCCESS");
                                        }
                                        if (boolPingILO == false)
                                        {
                                            strError = "The ILO IP Address is not responding ~ Asset ID " + intAsset.ToString() + ", Serial # " + strSerial;
                                            oLog.AddEvent(intAnswer, strName, strSerial, strError, LoggingType.Error);
                                        }
                                    }

                                    if (strError == "")
                                    {
                                        if (oOperatingSystem.IsSolaris(intOS) == false)
                                        {
                                            // Strip the :'s AND -'sout of MAC address
                                            strMACAddress1 = oAsset.GetServerOrBlade(intAsset, "macaddress").ToUpper();
                                            if (boolBlade == true || boolDell == true)
                                            {
                                                strMACAddress1 = oAsset.GetVirtualConnectMACAddress(intAsset, intAnswer, intEnvironment, 1, strScripts, dsn, strScripts, oLog, strName);
                                                if (strMACAddress1 == "**ERROR**")
                                                {
                                                    strError = "There was a problem retrieving the MAC address of the blade";
                                                    oLog.AddEvent(intAnswer, strName, strSerial, strError + " ~ AssetID " + intAsset.ToString() + ". Output file: " + strScripts + intAsset.ToString() + ".txt", LoggingType.Error);
                                                }
                                            }
                                            else
                                                oLog.AddEvent(intAnswer, strName, strSerial, "Skipping MAC Address Query since it is not a blade", LoggingType.Information);
                                            string strAltirisMAC = oFunction.FormatMAC(strMACAddress1, "");
                                            if (strError == "" && strAltirisMAC == "")
                                            {
                                                strError = "There is not MAC address configured for this device";
                                                oLog.AddEvent(intAnswer, strName, strSerial, "Invalid MAC address configured for this device ~ AssetID " + intAsset.ToString(), LoggingType.Information);
                                            }
                                            else
                                            {
                                                if (boolRDPMDT)
                                                {
                                                    // Already added to MDT
                                                }
                                                if (boolRDPAltiris)
                                                {
                                                    // Configure Altiris
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
                                                                oLog.AddEvent(intAnswer, strName, strSerial, "Found Duplicate Computer Object....Deleting Altiris ComputerID " + intDeleteComputer.ToString() + " on " + strRDPComputerWebService + "...result = " + boolDelete.ToString(), LoggingType.Information);
                                                            }
                                                            // Add Computer Object
                                                            oLog.AddEvent(intAnswer, strName, strSerial, "Configuring Altiris (MAC: " + strAltirisMAC + ") on " + strRDPScheduleWebService, LoggingType.Information);
                                                            int intComputer = oComputer.AddBasicVirtualComputer(-1, strName, oAsset.Get(intAsset, "asset"), oAsset.Get(intAsset, "serial"), strAltirisMAC, 2, "");
                                                            // Assign Schedule
                                                            NCC.ClearView.Application.Core.altirisws.dsjob oJob = new NCC.ClearView.Application.Core.altirisws.dsjob();
                                                            oJob.Credentials = oCredentials;
                                                            oJob.Url = strRDPScheduleWebService;
                                                            if (oOperatingSystem.Get(intOS, "linux") == "1")
                                                            {
                                                                oLog.AddEvent(intAnswer, strName, strSerial, "Adding Altiris Job (" + strHBAJob + ")", LoggingType.Information);
                                                                oJob.ScheduleNow(strName, strHBAJob);
                                                            }
                                                            oLog.AddEvent(intAnswer, strName, strSerial, "Adding Altiris Job (" + oOperatingSystem.Get(intOS, "altiris") + ")", LoggingType.Information);
                                                            oJob.ScheduleNow(strName, oOperatingSystem.Get(intOS, "altiris"));
                                                            oLog.AddEvent(intAnswer, strName, strSerial, "Finished Configuring Altiris", LoggingType.Information);
                                                        }
                                                        else
                                                            oLog.AddEvent(intAnswer, strName, strSerial, "Altiris Configuration Skipped (Environment = " + intEnvironment.ToString() + ")", LoggingType.Information);
                                                    }
                                                    else
                                                    {
                                                        strError = "Invalid build location (RDP) configuration ~ Class: " + oClass.Get(intClass, "name") + ", Environment: " + oEnvironment.Get(intEnv, "name") + ", Location: " + oLocation.GetFull(intAddress);
                                                        oLog.AddEvent(intAnswer, strName, strSerial, strError, LoggingType.Error);
                                                    }
                                                }
                                            }
                                        }

                                        if (strError == "")
                                        {
                                            if (boolIsNexusSwitch == true)
                                            {
                                                string strSwitchA = "";
                                                string strInterfaceA = "";
                                                string strSwitchB = "";
                                                string strInterfaceB = "";
                                                string strSwitchAdr = "";
                                                string strInterfaceAdr = "";
                                                string strSwitchBdr = "";
                                                string strInterfaceBdr = "";
                                                string strDescription = "BUILD_" + strName + "-";
                                                if (boolBlade == true)
                                                {
                                                    int intEnclosure = Int32.Parse(oAsset.GetServerOrBlade(intAsset, "enclosureid"));
                                                    int intSlot = Int32.Parse(oAsset.GetServerOrBlade(intAsset, "slot"));
                                                    string strEnclosure = oAsset.GetStatus(intEnclosure, "name");
                                                    DataSet dsDellBlade = oAsset.GetDellBladeSwitchports(strEnclosure, intSlot);
                                                    if (dsDellBlade.Tables[0].Rows.Count == 1)
                                                    {
                                                        DataRow drDellBlade = dsDellBlade.Tables[0].Rows[0];
                                                        strSwitchA = drDellBlade["switchA"].ToString();
                                                        strInterfaceA = drDellBlade["interfaceA"].ToString();
                                                        strSwitchB = drDellBlade["switchB"].ToString();
                                                        strInterfaceB = drDellBlade["interfaceB"].ToString();
                                                    }
                                                    else
                                                    {
                                                        strError = "There should only be one (1) record for the Dell Blade Switchport information ~ (there were " + dsDellBlade.Tables[0].Rows.Count.ToString() + " records), AssetID " + intAsset.ToString();
                                                        oLog.AddEvent(intAnswer, strName, strSerial, strError, LoggingType.Error);
                                                    }

                                                    if (intAssetDR > 0)
                                                    {
                                                        int intEnclosureDR = Int32.Parse(oAsset.GetServerOrBlade(intAssetDR, "enclosureid"));
                                                        int intSlotDR = Int32.Parse(oAsset.GetServerOrBlade(intAssetDR, "slot"));
                                                        string strEnclosureDR = oAsset.GetStatus(intEnclosureDR, "name");
                                                        DataSet dsDellBladeDR = oAsset.GetDellBladeSwitchports(strEnclosureDR, intSlotDR);
                                                        if (dsDellBladeDR.Tables[0].Rows.Count == 1)
                                                        {
                                                            DataRow drDellBladeDR = dsDellBladeDR.Tables[0].Rows[0];
                                                            strSwitchAdr = drDellBladeDR["switchA"].ToString();
                                                            strInterfaceAdr = drDellBladeDR["interfaceA"].ToString();
                                                            strSwitchBdr = drDellBladeDR["switchB"].ToString();
                                                            strInterfaceBdr = drDellBladeDR["interfaceB"].ToString();
                                                        }
                                                        else
                                                        {
                                                            strError = "There should only be one (1) record for the Dell Blade Switchport information ~ (there were " + dsDellBladeDR.Tables[0].Rows.Count.ToString() + " records), DR AssetID " + intAssetDR.ToString();
                                                            oLog.AddEvent(intAnswer, strName, strSerial, strError, LoggingType.Error);
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    foreach (DataRow drSwitch in dsSwitch.Tables[0].Rows)
                                                    {
                                                        if (drSwitch["blade"].ToString() != "")
                                                        {
                                                            if (strSwitchA == "")
                                                            {
                                                                strSwitchA = drSwitch["name"].ToString();
                                                                strInterfaceA = drSwitch["blade"].ToString() + "/" + drSwitch["port"].ToString();
                                                            }
                                                            else
                                                            {
                                                                strSwitchB = drSwitch["name"].ToString();
                                                                strInterfaceB = drSwitch["blade"].ToString() + "/" + drSwitch["port"].ToString();
                                                            }
                                                        }
                                                        else
                                                        {
                                                            strError = "The BLADE field of the switch was not defined...possibly a placeholder for physical relocation / configuration";
                                                            break;
                                                        }
                                                    }

                                                    if (intAssetDR > 0)
                                                    {
                                                        DataSet dsSwitchDR = oAsset.GetSwitchports(intAssetDR, SwitchPortType.Network);
                                                        foreach (DataRow drSwitchDR in dsSwitchDR.Tables[0].Rows)
                                                        {
                                                            if (drSwitchDR["blade"].ToString() != "")
                                                            {
                                                                if (strSwitchAdr == "")
                                                                {
                                                                    strSwitchAdr = drSwitchDR["name"].ToString();
                                                                    strInterfaceAdr = drSwitchDR["blade"].ToString() + "/" + drSwitchDR["port"].ToString();
                                                                }
                                                                else
                                                                {
                                                                    strSwitchBdr = drSwitchDR["name"].ToString();
                                                                    strInterfaceBdr = drSwitchDR["blade"].ToString() + "/" + drSwitchDR["port"].ToString();
                                                                }
                                                            }
                                                            else
                                                            {
                                                                strError = "The BLADE field of the switch was not defined...possibly a placeholder for physical relocation / configuration ~ DR";
                                                                break;
                                                            }
                                                        }
                                                    }
                                                }


                                                if (strError == "" && strSwitchA != "")
                                                    strError = Nexus(intAnswer, strName, strSerial, strSwitchA, strInterfaceA, DellBladeSwitchportMode.Access, strRDPVLAN, "", strDescription + "pri", false, oVariable, oAsset, intAsset);

                                                if (strError == "" && strSwitchB != "")
                                                    strError = Nexus(intAnswer, strName, strSerial, strSwitchB, strInterfaceB, DellBladeSwitchportMode.Access, strRDPVLAN, "", strDescription + "sec", false, oVariable, oAsset, intAsset);

                                                if (strError == "" && strSwitchAdr != "")
                                                    strError = Nexus(intAnswer, strName, strSerial, strSwitchAdr, strInterfaceAdr, DellBladeSwitchportMode.Access, strRDPVLAN, "", strDescription + "pri", false, oVariable, oAsset, intAsset);

                                                if (strError == "" && strSwitchBdr != "")
                                                    strError = Nexus(intAnswer, strName, strSerial, strSwitchBdr, strInterfaceBdr, DellBladeSwitchportMode.Access, strRDPVLAN, "", strDescription + "sec", false, oVariable, oAsset, intAsset);
                                            }

                                            if (boolIsNexusSwitch == false && oOperatingSystem.IsSolaris(intOS) == false)
                                            {
                                                if (boolBlade == true)
                                                {
                                                    if (boolDell == false)
                                                    {
                                                        oLog.AddEvent(intAnswer, strName, strSerial, "Attempting to preconfigure virtual connect settings (AssetID: " + intAsset.ToString() + ")", LoggingType.Information);
                                                        string strResultVC2 = oAsset.ExecuteVirtualConnectIP(intAsset, intAnswer, intEnvironment, strRDPVLAN, 2, false, false, true);
                                                        if (strResultVC2.Contains("ERROR") == true || strResultVC2 == "")
                                                        {
                                                            oLog.AddEvent(intAnswer, strName, strSerial, "Encountered an error when configuring virtual connect settings ~ build NIC (#2) for " + strRDPVLAN + "... " + strResultVC2, LoggingType.Error);
                                                            strError = "There was a problem configuring the Virtual Connect Settings for build ~ NIC (#2)";
                                                        }
                                                        else
                                                            oLog.AddEvent(intAnswer, strName, strSerial, "Successfully preconfigured virtual connect settings (NIC#2) (AssetID: " + intAsset.ToString() + ")", LoggingType.Information);
                                                        if (strError == "")
                                                        {
                                                            // NIC#1 = build network, PXE enabled
                                                            string strResultVC1 = oAsset.ExecuteVirtualConnectIP(intAsset, intAnswer, intEnvironment, strRDPVLAN, 1, true, false, false);
                                                            if (strResultVC1.Contains("ERROR") == true || strResultVC1 == "")
                                                            {
                                                                oLog.AddEvent(intAnswer, strName, strSerial, "Encountered an error when configuring virtual connect settings ~ build NIC (#1) for " + strRDPVLAN + "... " + strResultVC1, LoggingType.Error);
                                                                strError = "There was a problem configuring the Virtual Connect Settings for build ~ NIC (#1)";
                                                            }
                                                            else
                                                                oLog.AddEvent(intAnswer, strName, strSerial, "Successfully preconfigured virtual connect settings (NIC#1) (AssetID: " + intAsset.ToString() + ")", LoggingType.Information);
                                                        }
                                                    }
                                                    else
                                                    {
                                                    }
                                                }
                                                else
                                                {
                                                    // Catalyst Environment - nothing to do.
                                                }
                                            }
                                        }


                                        if (strError == "")
                                        {
                                            if (oOperatingSystem.IsSolaris(intOS) == false)
                                            {
                                                // Actually power on the device
                                                bool boolPower = oFunction.ExecutePower(intServer, intAsset, true, "Power", strName, intEnvironment, (boolDell ? strScripts : strScripts + strSub), intLogging, dsnAsset);
                                                if (boolPower == false)
                                                    strError = "There was a problem powering on the device ~ probably the ILO password is wrong";
                                                else
                                                    AddResult(intServer, intStep, intType, oOnDemand.GetStep(intStepID, "done"), strError);
                                            }
                                            else
                                            {
                                                // Solaris
                                                bool boolPingILO = false;
                                                for (int ii = 0; ii < 3 && boolPingILO == false; ii++)
                                                {
                                                    Thread.Sleep(3000);
                                                    Ping oPingChange = new Ping();
                                                    string strStatusChange = "";
                                                    try
                                                    {
                                                        PingReply oReplyChange = oPingChange.Send(strILO);
                                                        strStatusChange = oReplyChange.Status.ToString().ToUpper();
                                                    }
                                                    catch { }
                                                    boolPingILO = (strStatusChange == "SUCCESS");
                                                }
                                                if (boolPingILO == true)
                                                {
                                                    oLog.AddEvent(intAnswer, strName, strSerial, "Starting solaris build using ILO = " + strILO, LoggingType.Information);
                                                    SolarisBuild oBuild = new SolarisBuild(intServer, strName, oVariable, strImplementor, strSerial, strAsset, intAsset, strILO, intStep, dsn, dsnAsset, strScripts, intEnvironment, intLogging, boolSSHDebug, intRequest, intProvisioningErrorService, intResourceRequestApprove, intAssignPage, intViewPage, dsnIP, dsnServiceEditor, boolProvisioningErrorEmail);
                                                    ThreadStart oSSH = new ThreadStart(oBuild.Begin);
                                                    Thread oSSHJob = new Thread(oSSH);
                                                    oSSHJob.Start();
                                                    // The AddResult() function will be applied after the build is finished, or when it errors.
                                                }
                                                else
                                                {
                                                    strError = "The ILO IP Address is not responding ~ Asset ID " + intAsset.ToString() + ", Serial # " + strSerial;
                                                    oLog.AddEvent(intAnswer, strName, strSerial, strError, LoggingType.Error);
                                                }
                                            }
                                        }
                                    }
                                    
                                    if (strError != "")
                                        AddResult(intServer, intStep, intType, oOnDemand.GetStep(intStepID, "done"), strError);
                                    break;
                                case 8:     // ZEUS 
                                    if (intLogging > 1)
                                        oEventLog.WriteEntry(strName + " (Physical): " + "Starting Step 8 (Imaging)", EventLogEntryType.Information);
                                    DataSet dsZeusError = oZeus.GetResult(strSerial, 1);
                                    if (strDHCP != "0" && strDHCP != "")
                                    {
                                        // Configure Altiris
                                        oLog.AddEvent(intAnswer, strName, strSerial, "There are " + dsBuildRDP.Tables[0].Rows.Count.ToString() + " build locations (RDP) for class: " + oClass.Get(intClassRDP, "name") + " (" + intClassRDP.ToString() + "), env: " + oEnvironment.Get(intEnvRDP, "name") + " (" + intEnvRDP.ToString() + "), address: " + oLocation.GetFull(intAddressRDP) + " (" + intAddressRDP.ToString() + "), resiliency: " + oResiliency.Get(intResiliency, "name") + " (" + intResiliency.ToString() + "), RDPAltiris: " + boolRDPAltiris.ToString() + ", RDPMDT: " + boolRDPMDT.ToString() + ", Zone: " + intZone.ToString() + " for server ID " + intServer.ToString(), LoggingType.Information);
                                        if (oOperatingSystem.IsSolaris(intOS) == false)
                                        {
                                            if (dsBuildRDP.Tables[0].Rows.Count > 0 && dsBuildRDP.Tables[0].Rows[0]["rdp_computer_ws"].ToString() != "" && dsBuildRDP.Tables[0].Rows[0]["rdp_schedule_ws"].ToString() != "")
                                            {
                                                if (boolRDPMDT)
                                                {
                                                    // Cleanup MDT
                                                    strMACAddress1 = oAsset.GetServerOrBlade(intAsset, "macaddress").ToUpper();
                                                    if (boolBlade == true || boolDell == true)
                                                    {
                                                        // Query virtual connect for appropriate MAC address
                                                        strMACAddress1 = oAsset.GetVirtualConnectMACAddress(intAsset, intAnswer, intEnvironment, 1, strScripts, dsn, strScripts, oLog, strName);
                                                        if (strMACAddress1 == "**ERROR**")
                                                        {
                                                            strError = "There was a problem retrieving the MAC address of the blade";
                                                            oLog.AddEvent(intAnswer, strName, strSerial, "There was a problem retrieving the MAC address of the blade ~ AssetID " + intAsset.ToString() + ". Output file: " + strScripts + intAsset.ToString() + ".txt", LoggingType.Error);
                                                        }
                                                        if (boolDell == true)
                                                        {
                                                            strMACAddress2 = oAsset.GetVirtualConnectMACAddress(intAsset, intAnswer, intEnvironment, 2, strScripts, dsn, strScripts, oLog, strName);
                                                            if (strMACAddress2 == "**ERROR**")
                                                            {
                                                                strError = "There was a problem retrieving the MAC address of the blade";
                                                                oLog.AddEvent(intAnswer, strName, strSerial, "There was a problem retrieving the MAC address (#2) of the blade ~ AssetID " + intAsset.ToString() + ". Output file: " + strScripts + intAsset.ToString() + ".txt", LoggingType.Error);
                                                            }
                                                        }
                                                    }
                                                    else
                                                        oLog.AddEvent(intAnswer, strName, strSerial, "Skipping MAC Address Query since it is not a blade", LoggingType.Information);
                                                    string strRDPMDTWebService = dsBuildRDP.Tables[0].Rows[0]["rdp_mdt_ws"].ToString();
                                                    System.Net.NetworkCredential oCredentials = new System.Net.NetworkCredential(oVariable.ADUser(), oVariable.ADPassword(), oVariable.Domain());
                                                    BuildSubmit oMDT = new BuildSubmit();
                                                    oMDT.Credentials = oCredentials;
                                                    oMDT.Url = strRDPMDTWebService;
                                                    oLog.AddEvent(intAnswer, strName, strSerial, "Cleaning MDT on " + strRDPMDTWebService + " (" + strName + ", " + strMACAddress1 + ", " + "ServerShare" + ")", LoggingType.Information);
                                                    oMDT.Cleanup(strName, strMACAddress1, "ServerShare");
                                                    if (strMACAddress2 != "")
                                                    {
                                                        oLog.AddEvent(intAnswer, strName, strSerial, "Waiting 60 seconds for Active Directory to Sychronize...", LoggingType.Debug);
                                                        Thread.Sleep(60000); // Wait 60 seconds
                                                        oLog.AddEvent(intAnswer, strName, strSerial, "Cleaning MDT on " + strRDPMDTWebService + " (" + strName + ", " + strMACAddress2 + ", " + "ServerShare" + ")", LoggingType.Information);
                                                        oMDT.Cleanup(strName, strMACAddress2, "ServerShare");
                                                    }
                                                    oLog.AddEvent(intAnswer, strName, strSerial, "MDT has been cleared", LoggingType.Information);
                                                }
                                                if (boolRDPAltiris)
                                                {
                                                    if (intEnvironment == (int)CurrentEnvironment.CORPDMN || intEnvironment == (int)CurrentEnvironment.PNCNT_PROD)
                                                    {
                                                        // Delete Computer Object
                                                        string strRDPComputerWebServiceDelete = dsBuildRDP.Tables[0].Rows[0]["rdp_computer_ws"].ToString();
                                                        System.Net.NetworkCredential oCredentials = new System.Net.NetworkCredential(oVariable.AltirisUsername(), oVariable.AltirisPassword(), oVariable.AltirisDomain());
                                                        NCC.ClearView.Application.Core.AltirisWS_Computer.ComputerManagementService oComputer = new NCC.ClearView.Application.Core.AltirisWS_Computer.ComputerManagementService();
                                                        oComputer.Credentials = oCredentials;
                                                        oComputer.Url = strRDPComputerWebServiceDelete;
                                                        int intDeleteComputer = oComputer.GetComputerID(strName, 1);
                                                        oLog.AddEvent(intAnswer, strName, strSerial, "Deleting Altiris ComputerID " + intDeleteComputer.ToString() + " on " + strRDPComputerWebServiceDelete, LoggingType.Information);
                                                        if (intDeleteComputer > 0)
                                                        {
                                                            bool boolDelete = oComputer.DeleteComputer(intDeleteComputer);
                                                            oLog.AddEvent(intAnswer, strName, strSerial, "Finished Deleting Altiris", LoggingType.Information);
                                                        }
                                                        else
                                                            oLog.AddEvent(intAnswer, strName, strSerial, "Finished Deleting Altiris (Not Found)", LoggingType.Information);
                                                    }
                                                    else
                                                        oLog.AddEvent(intAnswer, strName, strSerial, "Altiris Deletion Skipped (Environment = " + intEnvironment.ToString() + ")", LoggingType.Information);
                                                }
                                            }
                                            else
                                            {
                                                strError = "Invalid build location (RDP) configuration ~ Class: " + oClass.Get(intClass, "name") + ", Environment: " + oEnvironment.Get(intEnv, "name") + ", Location: " + oLocation.GetFull(intAddress);
                                                oLog.AddEvent(intAnswer, strName, strSerial, strError, LoggingType.Error);
                                            }
                                        }
                                        else
                                        {
                                            strMACAddress1 = oAsset.GetServerOrBlade(intAsset, "macaddress").ToUpper();
                                            string strJumpstartCGI = "";
                                            if (dsBuildRDP.Tables[0].Rows.Count > 0)
                                                strJumpstartCGI = dsBuildRDP.Tables[0].Rows[0]["jumpstart_cgi"].ToString();
                                            if (strJumpstartCGI == "")
                                            {
                                                strError = "Invalid JUMPSTART CGI field for Jumpstart build location (RDP) ~ class: " + oClass.Get(intClass, "name") + ", address: " + oLocation.GetFull(intAddress) + " for server ID " + intServer.ToString();
                                                oLog.AddEvent(intAnswer, strName, strSerial, strError, LoggingType.Error);
                                            }
                                            string strSolarisMAC = oFunction.FormatMAC(strMACAddress1, ":");
                                            if (strSolarisMAC == "")
                                            {
                                                strError = "Invalid MAC Address ~ the MAC address is not in the proper format (" + strMACAddress1 + ")";
                                                oLog.AddEvent(intAnswer, strName, strSerial, strError, LoggingType.Error);
                                            }
                                            if (strError == "")
                                            {
                                                strSolarisMAC = strSolarisMAC.ToLower();
                                                // Perform post-image processing
                                                if (intEnvironment == (int)CurrentEnvironment.CORPDMN || intEnvironment == (int)CurrentEnvironment.PNCNT_PROD)
                                                {
                                                    string strJumpstartURL = "http://" + strJumpstartCGI + "/cgi-bin/jumpstart.cgi?1010,MACADDR=" + strSolarisMAC + ",remove_client";
                                                    oLog.AddEvent(intAnswer, strName, strSerial, "Beginning Client Removal on jumpstart URL = " + strJumpstartURL, LoggingType.Information);
                                                    if (RunCGI(strJumpstartURL) == true)
                                                    {
                                                        oLog.AddEvent(intAnswer, strName, strSerial, "Success!! Client Removal on jumpstart URL = " + strJumpstartURL, LoggingType.Information);
                                                    }
                                                    else
                                                    {
                                                        strError = "REMOVE_CLIENT registration ~ Jumpstart URL = " + strJumpstartURL + " failed.";
                                                        oLog.AddEvent(intAnswer, strName, strSerial, "Failure!! Client Removal on jumpstart URL = " + strJumpstartURL, LoggingType.Error);
                                                    }
                                                }
                                            }
                                        }
                                        AddResult(intServer, intStep, intType, oOnDemand.GetStep(intStepID, "done"), strError);
                                    }
                                    else if ((oSpan.Hours > 2 || dsZeusError.Tables[0].Rows.Count > 0) && boolZeusError == false && (intEnvironment == (int)CurrentEnvironment.CORPDMN || intEnvironment == (int)CurrentEnvironment.PNCNT_PROD))
                                    {
                                        string strCodeOwner = "PT43054;";
                                        string strCodeType = "NONE";
                                        if (oOperatingSystem.IsWindows2008(intOS) == true)
                                        {
                                            // Steve Stewart
                                            strCodeOwner = "PT43084;";
                                            strCodeType = "Microsoft Deployment Toolkit (2008)";
                                        }
                                        else if (oOperatingSystem.IsWindows(intOS) == true)
                                        {
                                            if (boolDell == true)
                                            {
                                                // Steve Stewart
                                                strCodeOwner = "PT43084;";
                                                strCodeType = "Microsoft Deployment Toolkit (2003)";
                                            }
                                            else
                                            {
                                                // Zeus - Steve Healy
                                                strCodeOwner = "PT43054;";
                                                strCodeType = "ZEUS";
                                            }
                                        }
                                        else if (oOperatingSystem.IsLinux(intOS) == true)
                                        {
                                            // Tom McGinnis
                                            strCodeOwner = "PT35309;";
                                            strCodeType = "KICKSTART";
                                        }
                                        else if (oOperatingSystem.IsSolaris(intOS) == true)
                                        {
                                            // Paul Ferroni
                                            strCodeOwner = "PT35267;";
                                            strCodeType = "JUMPSTART";
                                        }
                                        bool boolActuallyOK = false;
                                        if (dsZeusError.Tables[0].Rows.Count > 0)
                                        {
                                            strError = "IMAGING ERROR: " + dsZeusError.Tables[0].Rows[0]["message"].ToString();
                                            oLog.AddEvent(intAnswer, strName, strSerial, strError, LoggingType.Error);
                                            strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_PROVISIONING_SUPPORT");
                                            if (intEnvironment == (int)CurrentEnvironment.CORPDMN || intEnvironment == (int)CurrentEnvironment.PNCNT_PROD)
                                                oFunction.SendEmail("Auto-Provisioning " + strCodeType + " ERROR: " + strName, strCodeOwner, strImplementor, strEMailIdsBCC, "Auto-Provisioning " + strCodeType + " ERROR: " + strName, "<p><b>This message is to inform you that the server " + strName + " has encountered an error in " + strCodeType + "!</b><p><p>Error Message: " + strError + "<br/>Serial Number: " + oAsset.Get(intAsset, "serial").ToUpper() + "<br/>Asset Tag: " + oAsset.Get(intAsset, "asset").ToUpper() + "<br/>Model: " + oModelsProperties.Get(intModel, "name").ToUpper() + "<br/>Step #: " + intStep.ToString() + "<br/>Step: " + strStep + "<br/>ILO: <a href=\"https://" + strILO + "\" target=\"_blank\">" + strILO + "</a></p><p>Once you have resolved this issue, please <a href=\"" + oVariable.URL() + "/build_fixed.aspx?type=s&id=" + oFunction.encryptQueryString(intServer.ToString()) + "\" target=\"_blank\">click here</a> to resume the build.</p><p><b>NOTE:</b> This is automated email sent from the ClearView Auto-Provisioning tool. Please do not respond to this message.</p>", true, false);
                                        }
                                        else
                                        {
                                            // Check one last time before sending error
                                            strDHCP = oServer.Get(intServer, strZeus);
                                            if (strDHCP != "0" && strDHCP != "")
                                            {
                                                // Was completed...skip error
                                                boolActuallyOK = true;
                                            }
                                            else
                                            {
                                                strError = "Sitting at the IMAGING step for more than THREE (3) hours!";
                                                oLog.AddEvent(intAnswer, strName, strSerial, strError, LoggingType.Error);
                                                strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_PROVISIONING_SUPPORT");
                                                if (intEnvironment == (int)CurrentEnvironment.CORPDMN || intEnvironment == (int)CurrentEnvironment.PNCNT_PROD)
                                                    oFunction.SendEmail("Auto-Provisioning " + strCodeType + " Problem: " + strName, strCodeOwner, strImplementor, strEMailIdsBCC, "Auto-Provisioning " + strCodeType + " Problem: " + strName, "<p><b>This message is to inform you that the server " + strName + " has been sitting at the " + strCodeType + " step for more than THREE (3) hours!</b><p><p>Serial Number: " + oAsset.Get(intAsset, "serial").ToUpper() + "<br/>Asset Tag: " + oAsset.Get(intAsset, "asset").ToUpper() + "<br/>Model: " + oModelsProperties.Get(intModel, "name").ToUpper() + "<br/>ILO: <a href=\"https://" + strILO + "\" target=\"_blank\">" + strILO + "</a></p><p>Once you have resolved this issue, please <a href=\"" + oVariable.URL() + "/build_fixed.aspx?type=s&id=" + oFunction.encryptQueryString(intServer.ToString()) + "\" target=\"_blank\">click here</a> to resume the build.</p><p><b>NOTE:</b> This is automated email sent from the ClearView Auto-Provisioning tool. Please do not respond to this message.</p>", true, false);
                                            }
                                        }
                                        if (boolActuallyOK == false)
                                            oServer.UpdateZeusError(intServer, 1);
                                        // Add Error message so it displays in provisioning windows
                                        AddResult(intServer, intStep, intType, "", strError);
                                    }
                                    break;
                                case 9:    // Create Active Directory Accounts
                                    oLog.AddEvent(intAnswer, strName, strSerial, "Starting Step 9 (Create AD Accounts)", LoggingType.Information);
                                    if (intEnvironment == (int)CurrentEnvironment.CORPDMN || intEnvironment == (int)CurrentEnvironment.PNCNT_PROD)
                                    {
                                        if (oOperatingSystem.IsWindows(intOS) == true || oOperatingSystem.IsWindows2008(intOS) == true)
                                        {
                                            if (oDomain.Get(intDomain, "account_setup") == "1")
                                            {
                                                oAccountRequest.Process(intRequest, 0, 0, strName, "Abcd1234", intEnvironment, 1);
                                                DataSet dsNotify = oRequest.GetResult(intRequest);
                                                string strNotify = "";
                                                foreach (DataRow drResult in dsNotify.Tables[0].Rows)
                                                    strNotify += drResult["result"].ToString();
                                                if (strNotify != "")
                                                {
                                                    strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_ACTIVE_DIRECTORY");
                                                    oFunction.SendEmail("ClearView Account Request", oUser.GetName(intUser), "", strEMailIdsBCC, "ClearView Account Request", strNotify, true, false);
                                                    strResult = "Active Directory accounts and permissions were created...<br/>" + strNotify;
                                                }
                                                else
                                                    strResult = "Active Directory accounts and permissions were skipped";
                                            }
                                            else
                                            {
                                                strResult = "Active Directory Account Configuration is not available for this domain";
                                            }
                                        }
                                        else
                                        {
                                            strResult = "Active Directory Account Configuration Only Available for Distributed Builds";
                                        }
                                    }
                                    else
                                    {
                                        strResult = "This step is not available in TEST environment";
                                    }
                                    AddResult(intServer, intStep, intType, strResult, strError);
                                    break;
                                case 10:    // Assign IP Address(es)
                                    oLog.AddEvent(intAnswer, strName, strSerial, "Starting Step 10 (Assign IP Address(es))", LoggingType.Information);
                                    bool boolDHCP = true;
                                    DataSet dsDNS = oDomain.GetClassDNS(intDomain, intClass, intAddress);
                                    if (intIPAddressBuild1 > 0)
                                    {
                                        // Check to see if it was already assigned from a previous attempt that errored.
                                        string strIPChange = oIPAddresses.GetName(intIPAddressBuild1, 0);
                                        oLog.AddEvent(intAnswer, strName, strSerial, "Check to see if IP address (" + strIPChange + ") was already assigned from a previous attempt", LoggingType.Information);
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
                                            oLog.AddEvent(intAnswer, strName, strSerial, "IP address (" + strIPChange + ") was NOT already assigned from a previous attempt", LoggingType.Information);
                                            if (oOperatingSystem.IsSolaris(intOS) == true && boolIsNexusSwitch == false)
                                            {
                                                oLog.AddEvent(intAnswer, strName, strSerial, "Since this is a solaris build (non-nexus), it might be rebooting...waiting another 5 minutes", LoggingType.Information);
                                                // Sleep for 5 second intervals for up to 5 minutes
                                                for (int ii = 0; ii < 60 && boolPingChanged == false; ii++)
                                                {
                                                    Thread.Sleep(5000);
                                                    Ping oPingChange2 = new Ping();
                                                    string strStatusChange2 = "";
                                                    try
                                                    {
                                                        PingReply oReplyChange2 = oPingChange2.Send(strIPChange);
                                                        strStatusChange2 = oReplyChange2.Status.ToString().ToUpper();
                                                    }
                                                    catch { }
                                                    boolPingChanged = (strStatusChange2 == "SUCCESS");
                                                }
                                                if (boolPingChanged == true)
                                                {
                                                    oLog.AddEvent(intAnswer, strName, strSerial, "The IP address [" + strIPChange + "] responded to a ping...OK.", LoggingType.Information);
                                                    strResult = oOnDemand.GetStep(intStepID, "done");
                                                }
                                                else
                                                {
                                                    strError = "There was a problem assigning the IP Address ~ (ping " + strIPChange + " did not respond to ping)";
                                                    oLog.AddEvent(intAnswer, strName, strSerial, strError, LoggingType.Error);
                                                }
                                            }
                                            else
                                            {
                                                oLog.AddEvent(intAnswer, strName, strSerial, "There are " + dsBuildRDP.Tables[0].Rows.Count.ToString() + " build locations (RDP) for class: " + oClass.Get(intClassRDP, "name") + " (" + intClassRDP.ToString() + "), env: " + oEnvironment.Get(intEnvRDP, "name") + " (" + intEnvRDP.ToString() + "), address: " + oLocation.GetFull(intAddressRDP) + " (" + intAddressRDP.ToString() + "), resiliency: " + oResiliency.Get(intResiliency, "name") + " (" + intResiliency.ToString() + "), RDPAltiris: " + boolRDPAltiris.ToString() + ", RDPMDT: " + boolRDPMDT.ToString() + ", Zone: " + intZone.ToString() + " for server ID " + intServer.ToString(), LoggingType.Information);
                                                if (dsBuildRDP.Tables[0].Rows.Count > 0)
                                                {
                                                    if (boolIsNexusSwitch == false)
                                                        strRDPVLAN = dsBuildRDP.Tables[0].Rows[0]["blade_vlan"].ToString();
                                                    else
                                                        strRDPVLAN = dsBuildRDP.Tables[0].Rows[0]["dell_vlan"].ToString();
                                                }

                                                oLog.AddEvent(intAnswer, strName, strSerial, "Either not a solaris build or Nexus", LoggingType.Debug);
                                                if (oOperatingSystem.IsWindows(intOS) == true || oOperatingSystem.IsWindows2008(intOS) == true)
                                                {
                                                    if (strIP != "")
                                                    {
                                                        oLog.AddEvent(intAnswer, strName, strSerial, "Check to see if DHCP address (" + strIP + ") responds...", LoggingType.Information);
                                                        bool boolPingOn = false;
                                                        for (int ii = 0; ii < 3 && boolPingOn == false; ii++)
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
                                                            boolPingOn = (strStatus == "SUCCESS");
                                                        }
                                                        if (boolPingOn == true)
                                                        {
                                                            if (dsDNS.Tables[0].Rows.Count > 0)
                                                            {
                                                                oLog.AddEvent(intAnswer, strName, strSerial, "DHCP address (" + strIP + ") is responding...generating scripts...", LoggingType.Information);
                                                                //string strTrunkScriptLocation = "\\\\" + strIP + "\\C$\\PROGRAM FILES\\BROADCOM\\BACS\\";
                                                                string strTrunkScriptLocationBroadcom = "\\PROGRAM FILES\\BROADCOM\\BACS\\";
                                                                string strTrunkScriptLocationIntel = "\\PROGRAM FILES\\INTEL\\";
                                                                bool boolNicBroadcom = false;
                                                                bool boolNicIntel = false;

                                                                if (boolIsNexusSwitch == true && boolBlade == true)
                                                                {
                                                                    oLog.AddEvent(intAnswer, strName, strSerial, "Checking to see if the required trunking script files exist...", LoggingType.Debug);
                                                                    // See if the files exist.
                                                                    /*
                                                                    if (File.Exists(strTrunkScriptLocation + "create_Dell_LACP.vbs") == false)
                                                                        strError = "One or more of the teaming script files does not exist ~ File: " + strTrunkScriptLocation + "create_Dell_LACP.vbs";
                                                                    else if (File.Exists(strTrunkScriptLocation + "cfg_Dell_LACP_vlan.vbs") == false)
                                                                        strError = "One or more of the teaming script files does not exist ~ File: " + strTrunkScriptLocation + "cfg_Dell_LACP_vlan.vbs";
                                                                    else
                                                                        oLog.AddEvent(intAnswer, strName, strSerial, "The trunking script files exist...moving on.", LoggingType.Information);
                                                                    */
                                                                    if (File.Exists("\\\\" + strIP + "\\C$" + strTrunkScriptLocationBroadcom + "cfg_Dell_LACP_vlan.vbs") == true)
                                                                    {
                                                                        oLog.AddEvent(intAnswer, strName, strSerial, "A BROADCOM trunking script file exists...moving on.", LoggingType.Information);
                                                                        boolNicBroadcom = true;
                                                                    }
                                                                    else if (File.Exists("\\\\" + strIP + "\\C$" + strTrunkScriptLocationIntel + "cfg_Dell_Intel_LACP_vlan.vbs") == true)
                                                                    {
                                                                        oLog.AddEvent(intAnswer, strName, strSerial, "An INTEL trunking script file exists...moving on.", LoggingType.Information);
                                                                        boolNicIntel = true;
                                                                    }
                                                                    else
                                                                        strError = "Neither of the teaming script files exist";
                                                                }

                                                                if (strError == "")
                                                                {
                                                                    // 1st part - create VBS's file to copy to server
                                                                    StreamWriter oWriterIP1 = new StreamWriter(strFile);
                                                                    oWriterIP1.WriteLine("Set objWMIService = GetObject(\"winmgmts:\\\\.\\root\\cimv2\")");
                                                                    oWriterIP1.WriteLine("Set colAdapters = objWMIService.ExecQuery(\"Select * from Win32_NetworkAdapterConfiguration Where IPEnabled = True\")");
                                                                    oWriterIP1.WriteLine("Set oClass = objWMIService.Get(\"Win32_NetworkAdapterConfiguration\")");
                                                                    oWriterIP1.WriteLine("For Each objAdapter in colAdapters");
                                                                    oWriterIP1.WriteLine("If (objAdapter.IPAddress(0) = \"" + strIPChange + "\") Then");
                                                                    string strSuffix = "";
                                                                    DataSet dsSuffix = oDomain.GetSuffixs(intDomain, 1);
                                                                    foreach (DataRow drSuffix in dsSuffix.Tables[0].Rows)
                                                                    {
                                                                        if (strSuffix != "")
                                                                            strSuffix += ",";
                                                                        strSuffix += "\"" + drSuffix["name"].ToString() + "\"";
                                                                    }
                                                                    strSuffix = "Array(" + strSuffix + ")";
                                                                    oWriterIP1.WriteLine("oClass.SetDNSSuffixSearchOrder(" + strSuffix + ")");
                                                                    if (oOperatingSystem.IsWindows2008(intOS) == true)
                                                                    {
                                                                        // Windows 2008 ONLY: Uncheck  - "Register this connection's addresses in DNS"
                                                                        oWriterIP1.WriteLine("objAdapter.SetDynamicDNSRegistration(False)");
                                                                    }
                                                                    else
                                                                    {
                                                                        // Windows 2003 ONLY: "Enable NetBIOS over TCP/IP" should be selected
                                                                        oWriterIP1.WriteLine("objAdapter.SetTCPIPNetBIOS(1)");
                                                                    }
                                                                    // Set "DNS Suffix for this Connection"
                                                                    oWriterIP1.WriteLine("objAdapter.SetDNSDomain(\"" + oVar.FullyQualified() + "\")");
                                                                    oWriterIP1.WriteLine("End If");
                                                                    oWriterIP1.WriteLine("Next");
                                                                    oWriterIP1.Flush();
                                                                    oWriterIP1.Close();
                                                                    string strRebootVBS = strScripts + strSub + intAnswer.ToString() + "_" + strNow + "_reboot.vbs";
                                                                    // 1/5/2010: only reboot if a blade
                                                                    if (boolBlade == true)
                                                                    {
                                                                        StreamWriter oRebootIP1 = new StreamWriter(strRebootVBS);
                                                                        oRebootIP1.WriteLine("Set OpSysSet = GetObject(\"winmgmts:{impersonationLevel=impersonate,(Shutdown)}\").ExecQuery(\"select * from Win32_OperatingSystem where Primary=true\")");
                                                                        oRebootIP1.WriteLine("For Each OpSys In OpSysSet");
                                                                        oRebootIP1.WriteLine("Debug \"OpSys.Win32Shutdown return: \" & OpSys.Win32Shutdown(5)");
                                                                        oRebootIP1.WriteLine("Next");
                                                                        oRebootIP1.Flush();
                                                                        oRebootIP1.Close();
                                                                    }
                                                                    // 2nd part - create batch file
                                                                    string strBatchIP1 = strScripts + strSub + intAnswer.ToString() + "_" + strNow + "_ip.bat";
                                                                    StreamWriter oWriterIP2 = new StreamWriter(strBatchIP1);
                                                                    string strDeviceName = "Virtual";
                                                                    if (boolPNC == true)
                                                                        strDeviceName = "PNCNet";

                                                                    if (boolIsNexusSwitch == true && boolBlade == true)
                                                                    {
                                                                        // Trunking Scripts - no need to run the other scripts.
                                                                        /*
                                                                        // Create the LACP team, no paramaters.
                                                                        oWriterIP2.WriteLine("%windir%\\system32\\wscript.exe \"C:\\PROGRAM FILES\\BROADCOM\\BACS\\create_Dell_LACP.vbs\"");
                                                                        */
                                                                        // Add a vlan to the existing team.
                                                                        if (intIPAddressBackup > 0)
                                                                        {
                                                                            int intNetworkBackup = Int32.Parse(oIPAddresses.Get(intIPAddressBackup, "networkid"));
                                                                            int intVlanBackup = Int32.Parse(oIPAddresses.GetNetwork(intNetworkBackup, "vlanid"));
                                                                            string strBackupVLAN = oIPAddresses.GetVlan(intVlanBackup, "vlan");
                                                                            if (boolNicBroadcom == true)
                                                                                oWriterIP2.WriteLine("%windir%\\system32\\wscript.exe \"C:" + strTrunkScriptLocationBroadcom + "cfg_Dell_LACP_vlan.vbs\" Backup " + strBackupVLAN + " " + oIPAddresses.GetName(intIPAddressBackup, 0) + " " + oIPAddresses.GetNetwork(intNetworkBackup, "mask") + " none");
                                                                            if (boolNicIntel == true)
                                                                                oWriterIP2.WriteLine("%windir%\\system32\\wscript.exe \"C:" + strTrunkScriptLocationIntel + "cfg_Dell_Intel_LACP_vlan.vbs\" Backup " + strBackupVLAN + " " + oIPAddresses.GetName(intIPAddressBackup, 0) + " " + oIPAddresses.GetNetwork(intNetworkBackup, "mask") + " none");
                                                                        }
                                                                        if (boolNicBroadcom == true)
                                                                            oWriterIP2.WriteLine("%windir%\\system32\\wscript.exe \"C:" + strTrunkScriptLocationBroadcom + "cfg_Dell_LACP_vlan.vbs\" Vlan_" + strVLAN + " " + strVLAN + " " + strIPChange + " " + oIPAddresses.GetNetwork(intNetwork, "mask") + " " + oIPAddresses.GetNetwork(intNetwork, "gateway"));
                                                                        if (boolNicIntel == true)
                                                                            oWriterIP2.WriteLine("%windir%\\system32\\wscript.exe \"C:" + strTrunkScriptLocationIntel + "cfg_Dell_Intel_LACP_vlan.vbs\" Vlan_" + strVLAN + " " + strVLAN + " " + strIPChange + " " + oIPAddresses.GetNetwork(intNetwork, "mask") + " " + oIPAddresses.GetNetwork(intNetwork, "gateway"));
                                                                        strDeviceName = "PNCNET_Vlan_" + strVLAN;
                                                                    }
                                                                    else
                                                                    {
                                                                        // GET FROM ALBERT!!
                                                                        //if (intIPAddressBackup > 0)
                                                                        //{
                                                                        //    int intNetworkBackup = Int32.Parse(oIPAddresses.Get(intIPAddressBackup, "networkid"));
                                                                        //    int intVlanBackup = Int32.Parse(oIPAddresses.GetNetwork(intNetworkBackup, "vlanid"));
                                                                        //    string strBackupVLAN = oIPAddresses.GetVlan(intVlanBackup, "vlan");
                                                                        //    if (oOperatingSystem.IsWindows2008(intOS) == true)
                                                                        //        oWriterIP2.WriteLine("netsh interface ipv4 set address name=\"Backup\" source=static address=" + oIPAddresses.GetName(intIPAddressBackup, 0) + " mask=" + oIPAddresses.GetNetwork(intNetworkBackup, "mask") + " gateway=none");
                                                                        //    else
                                                                        //        oWriterIP2.WriteLine("netsh interface ip set address name=\"Backup\" source=static addr=" + oIPAddresses.GetName(intIPAddressBackup, 0) + " mask=" + oIPAddresses.GetNetwork(intNetworkBackup, "mask"));
                                                                        //}
                                                                        if (oOperatingSystem.IsWindows2008(intOS) == true)
                                                                            oWriterIP2.WriteLine("netsh interface ipv4 set address name=\"" + strDeviceName + "\" source=static address=" + strIPChange + " mask=" + oIPAddresses.GetNetwork(intNetwork, "mask") + " gateway=" + oIPAddresses.GetNetwork(intNetwork, "gateway"));
                                                                        else
                                                                            oWriterIP2.WriteLine("netsh interface ip set address name=\"" + strDeviceName + "\" source=static addr=" + strIPChange + " mask=" + oIPAddresses.GetNetwork(intNetwork, "mask"));
                                                                    }
                                                                    // Wait 5 seconds to finish
                                                                    oWriterIP2.WriteLine("ping 1.1.1.1 -n 5 -w 1000");
                                                                    if (oOperatingSystem.IsWindows2008(intOS) == true)
                                                                    {
                                                                        if (dsDNS.Tables[0].Rows[0]["dns_ip1"].ToString() != "")
                                                                            oWriterIP2.WriteLine("netsh interface ipv4 add dnsserver name=\"" + strDeviceName + "\" address=" + dsDNS.Tables[0].Rows[0]["dns_ip1"].ToString() + " index=1");
                                                                        if (dsDNS.Tables[0].Rows[0]["dns_ip2"].ToString() != "")
                                                                            oWriterIP2.WriteLine("netsh interface ipv4 add dnsserver name=\"" + strDeviceName + "\" address=" + dsDNS.Tables[0].Rows[0]["dns_ip2"].ToString() + " index=2");
                                                                        if (dsDNS.Tables[0].Rows[0]["dns_ip3"].ToString() != "")
                                                                            oWriterIP2.WriteLine("netsh interface ipv4 add dnsserver name=\"" + strDeviceName + "\" address=" + dsDNS.Tables[0].Rows[0]["dns_ip3"].ToString() + " index=3");
                                                                        if (dsDNS.Tables[0].Rows[0]["dns_ip4"].ToString() != "")
                                                                            oWriterIP2.WriteLine("netsh interface ipv4 add dnsserver name=\"" + strDeviceName + "\" address=" + dsDNS.Tables[0].Rows[0]["dns_ip4"].ToString() + " index=4");
                                                                        if (dsDNS.Tables[0].Rows[0]["wins_ip1"].ToString() != "")
                                                                            oWriterIP2.WriteLine("netsh interface ipv4 add winsserver name=\"" + strDeviceName + "\" address=" + dsDNS.Tables[0].Rows[0]["wins_ip1"].ToString() + " index=1");
                                                                        if (dsDNS.Tables[0].Rows[0]["wins_ip2"].ToString() != "")
                                                                            oWriterIP2.WriteLine("netsh interface ipv4 add winsserver name=\"" + strDeviceName + "\" address=" + dsDNS.Tables[0].Rows[0]["wins_ip2"].ToString() + " index=2");
                                                                        if (dsDNS.Tables[0].Rows[0]["wins_ip3"].ToString() != "")
                                                                            oWriterIP2.WriteLine("netsh interface ipv4 add winsserver name=\"" + strDeviceName + "\" address=" + dsDNS.Tables[0].Rows[0]["wins_ip3"].ToString() + " index=3");
                                                                        if (dsDNS.Tables[0].Rows[0]["wins_ip4"].ToString() != "")
                                                                            oWriterIP2.WriteLine("netsh interface ipv4 add winsserver name=\"" + strDeviceName + "\" address=" + dsDNS.Tables[0].Rows[0]["wins_ip4"].ToString() + " index=4");
                                                                    }
                                                                    else
                                                                    {
                                                                        oWriterIP2.WriteLine("netsh interface ip set address name=\"" + strDeviceName + "\" gateway=" + oIPAddresses.GetNetwork(intNetwork, "gateway") + " gwmetric=0");
                                                                        if (dsDNS.Tables[0].Rows[0]["dns_ip1"].ToString() != "")
                                                                        {
                                                                            if (boolPNC == true)
                                                                                oWriterIP2.WriteLine("netsh interface ip set dns name=\"" + strDeviceName + "\" source=static addr=" + dsDNS.Tables[0].Rows[0]["dns_ip1"].ToString() + " register=NONE");
                                                                            else
                                                                                oWriterIP2.WriteLine("netsh interface ip set dns name=\"" + strDeviceName + "\" source=static addr=" + dsDNS.Tables[0].Rows[0]["dns_ip1"].ToString() + " register=PRIMARY");
                                                                        }
                                                                        if (dsDNS.Tables[0].Rows[0]["dns_ip2"].ToString() != "")
                                                                            oWriterIP2.WriteLine("netsh interface ip add dns name=\"" + strDeviceName + "\" addr=" + dsDNS.Tables[0].Rows[0]["dns_ip2"].ToString() + " index=2");
                                                                        if (dsDNS.Tables[0].Rows[0]["dns_ip3"].ToString() != "")
                                                                            oWriterIP2.WriteLine("netsh interface ip add dns name=\"" + strDeviceName + "\" addr=" + dsDNS.Tables[0].Rows[0]["dns_ip3"].ToString() + " index=3");
                                                                        if (dsDNS.Tables[0].Rows[0]["dns_ip4"].ToString() != "")
                                                                            oWriterIP2.WriteLine("netsh interface ip add dns name=\"" + strDeviceName + "\" addr=" + dsDNS.Tables[0].Rows[0]["dns_ip4"].ToString() + " index=4");
                                                                        if (dsDNS.Tables[0].Rows[0]["wins_ip1"].ToString() != "")
                                                                            oWriterIP2.WriteLine("netsh interface ip set wins name=\"" + strDeviceName + "\" source=static addr=" + dsDNS.Tables[0].Rows[0]["wins_ip1"].ToString());
                                                                        if (dsDNS.Tables[0].Rows[0]["wins_ip2"].ToString() != "")
                                                                            oWriterIP2.WriteLine("netsh interface ip add wins name=\"" + strDeviceName + "\" addr=" + dsDNS.Tables[0].Rows[0]["wins_ip2"].ToString() + " index=2");
                                                                        if (dsDNS.Tables[0].Rows[0]["wins_ip3"].ToString() != "")
                                                                            oWriterIP2.WriteLine("netsh interface ip add wins name=\"" + strDeviceName + "\" addr=" + dsDNS.Tables[0].Rows[0]["wins_ip3"].ToString() + " index=3");
                                                                        if (dsDNS.Tables[0].Rows[0]["wins_ip4"].ToString() != "")
                                                                            oWriterIP2.WriteLine("netsh interface ip add wins name=\"" + strDeviceName + "\" addr=" + dsDNS.Tables[0].Rows[0]["wins_ip4"].ToString() + " index=4");
                                                                    }
                                                                    // Wait 5 seconds to finish
                                                                    oWriterIP2.WriteLine("ping 1.1.1.1 -n 5 -w 1000");
                                                                    // 11/18/2010 : Change IP Code
                                                                    if (boolNewIP == false)
                                                                    {
                                                                        if (boolBlade == true)
                                                                        {
                                                                            // Completed...shutdown
                                                                            oWriterIP2.WriteLine("@ECHO OFF");
                                                                            oWriterIP2.WriteLine("ECHO ****************************************");
                                                                            oWriterIP2.WriteLine("ECHO IP Address Changed Successfully!!!");
                                                                            oWriterIP2.WriteLine("IF [%1] NEQ [] (");
                                                                            oWriterIP2.WriteLine("ECHO ...shutting down...");
                                                                            oWriterIP2.WriteLine("%windir%\\system32\\wscript.exe \"C:\\OPTIONS\\CV_IP_SHUTDOWN.VBS\"");
                                                                            oWriterIP2.WriteLine(") ELSE (");
                                                                            oWriterIP2.WriteLine("ECHO ...skipping shutdown");
                                                                            oWriterIP2.WriteLine(")");
                                                                        }
                                                                    }
                                                                    oWriterIP2.Flush();
                                                                    oWriterIP2.Close();
                                                                    // 11/18/2010 : Change IP Code
                                                                    string strBatchIPExecute = strScripts + strSub + intAnswer.ToString() + "_" + strNow + "_ip_execute.bat";
                                                                    if (boolNewIP == true)
                                                                    {
                                                                        // Create Calling BATCH file
                                                                        StreamWriter oWriterIPExecute = new StreamWriter(strBatchIPExecute);
                                                                        oWriterIPExecute.WriteLine("%windir%\\system32\\cmd.exe /c \"C:\\OPTIONS\\CV_IP.BAT\" >\"C:\\OPTIONS\\CV_IP.TXT\" 2>&1");
                                                                        oWriterIPExecute.WriteLine("%windir%\\system32\\cmd.exe /c \"C:\\OPTIONS\\CV_IP.BAT\" >\"C:\\OPTIONS\\CV_IP_RERUN.TXT\" 2>&1");
                                                                        oWriterIPExecute.WriteLine("%windir%\\system32\\cmd.exe /c \"C:\\OPTIONS\\CV_IP.EXE\" PSEXESVC");
                                                                        oWriterIPExecute.WriteLine("SC DELETE PSEXESVC");
                                                                        oWriterIPExecute.WriteLine("ping 1.1.1.1 -n 5 -w 1000");
                                                                        oWriterIPExecute.WriteLine("%windir%\\system32\\cmd.exe /c DEL %windir%\\PSEXESVC.EXE");
                                                                        oWriterIPExecute.WriteLine("%windir%\\system32\\cmd.exe /c DEL %windir%\\system32\\PSEXESVC.EXE");
                                                                        oWriterIPExecute.WriteLine("IF \"%1\" == \"\" GOTO END");
                                                                        if (boolBlade == true)
                                                                            oWriterIPExecute.WriteLine("%windir%\\system32\\wscript.exe \"C:\\OPTIONS\\CV_IP_SHUTDOWN.VBS\"");
                                                                        oWriterIPExecute.WriteLine(":END");
                                                                        oWriterIPExecute.Flush();
                                                                        oWriterIPExecute.Close();
                                                                    }
                                                                    // 3rd part - create batch file
                                                                    string strBatchIP2 = strScripts + strSub + intAnswer.ToString() + "_" + strNow + "_ip_2.bat";
                                                                    StreamWriter oWriterIP3 = new StreamWriter(strBatchIP2);
                                                                    oWriterIP3.WriteLine("F:");
                                                                    oWriterIP3.WriteLine("cd " + strScripts + strSub);
                                                                    oWriterIP3.WriteLine("net use \\\\" + strIP + "\\C$ /user:" + strAdminUser + " " + strAdminPass + "");
                                                                    oWriterIP3.WriteLine("mkdir \\\\" + strIP + "\\C$\\OPTIONS");
                                                                    oWriterIP3.WriteLine("copy " + strFile + " \\\\" + strIP + "\\C$\\OPTIONS\\CV_IP.VBS");
                                                                    // 1/5/2010: only reboot if a blade
                                                                    if (boolBlade == true)
                                                                        oWriterIP3.WriteLine("copy " + strRebootVBS + " \\\\" + strIP + "\\C$\\OPTIONS\\CV_IP_SHUTDOWN.VBS");
                                                                    // 11/18/2010 : Change IP Code
                                                                    if (boolNewIP == true)
                                                                        oWriterIP3.WriteLine("copy " + strScripts + "pskill.exe \\\\" + strIP + "\\C$\\OPTIONS\\CV_IP.EXE");
                                                                    oWriterIP3.WriteLine("copy " + strBatchIP1 + " \\\\" + strIP + "\\C$\\OPTIONS\\CV_IP.BAT");
                                                                    // 11/18/2010 : Change IP Code
                                                                    if (boolNewIP == true)
                                                                        oWriterIP3.WriteLine("copy " + strBatchIPExecute + " \\\\" + strIP + "\\C$\\OPTIONS\\CV_IP_EXECUTE.BAT");
                                                                    oWriterIP3.Flush();
                                                                    oWriterIP3.Close();
                                                                    // 4th part - run the batch file to perform copy
                                                                    string strFileIP2Out = strScripts + strSub + intAnswer.ToString() + "_" + strNow + "_ip_2.txt";
                                                                    ProcessStartInfo infoIPcopy = new ProcessStartInfo(strScripts + "psexec");
                                                                    infoIPcopy.WorkingDirectory = strScripts;
                                                                    if (oOperatingSystem.IsWindows2008(intOS) == true)
                                                                    {
                                                                        // Have to remove the "-i" parameter or else the script won't run (will come up with "the following helper dll cannot be loaded...XXXX.DLL" error)
                                                                        infoIPcopy.Arguments = "cmd.exe /c " + strBatchIP2 + " > " + strFileIP2Out;
                                                                    }
                                                                    else
                                                                        infoIPcopy.Arguments = "-i cmd.exe /c " + strBatchIP2 + " > " + strFileIP2Out;
                                                                    bool boolIPTimeout = false;
                                                                    Process procIPcopy = Process.Start(infoIPcopy);
                                                                    procIPcopy.WaitForExit(intTimeoutDefault);
                                                                    if (procIPcopy.HasExited == false)
                                                                    {
                                                                        procIPcopy.Kill();
                                                                        boolIPTimeout = true;
                                                                    }
                                                                    procIPcopy.Close();
                                                                    if (boolIPTimeout == false)
                                                                    {
                                                                        bool boolOutputIP = ReadOutput(intServer, "IP", strFileIP2Out, strName, strSerial);
                                                                        if (boolOutputIP == false)
                                                                        {
                                                                            // 5th part - file has been copied, do the PSEXEC to install application
                                                                            ProcessStartInfo info = new ProcessStartInfo(strScripts + "psexec");
                                                                            info.WorkingDirectory = strScripts;
                                                                            // 11/18/2010 : Change IP Code
                                                                            if (boolNewIP == true)
                                                                            {
                                                                                info.Arguments = "\\\\" + strIP + " -u " + strAdminUser + " -p " + strAdminPass + " -i cmd.exe /c \"C:\\OPTIONS\\CV_IP_EXECUTE.BAT" + (boolBlade ? " shutdown" : "") + "\" >\"C:\\OPTIONS\\CV_IP_EXECUTE.TXT\" 2>&1";
                                                                                oLog.AddEvent(intAnswer, strName, strSerial, "PSEXEC Script = " + "\\\\" + strIP + " -u " + strAdminUser + " -p ***** -i cmd.exe /c \"C:\\OPTIONS\\CV_IP_EXECUTE.BAT" + (boolBlade ? " shutdown" : "") + "\" >\"C:\\OPTIONS\\CV_IP_EXECUTE.TXT\" 2>&1", LoggingType.Information);
                                                                            }
                                                                            else
                                                                            {
                                                                                if (oOperatingSystem.IsWindows2008(intOS) == true)
                                                                                {
                                                                                    // Have to remove the "-i" parameter or else the script won't run (will come up with "the following helper dll cannot be loaded...XXXX.DLL" error)
                                                                                    info.Arguments = "\\\\" + strIP + " -u " + strAdminUser + " -p " + strAdminPass + " cmd.exe /c C:\\OPTIONS\\CV_IP.BAT shutdown >C:\\OPTIONS\\CV_IP.TXT 2>&1";
                                                                                    oLog.AddEvent(intAnswer, strName, strSerial, "PSEXEC Script = " + "\\\\" + strIP + " -u " + strAdminUser + " -p ***** cmd.exe /c C:\\OPTIONS\\CV_IP.BAT shutdown >C:\\OPTIONS\\CV_IP.TXT 2>&1", LoggingType.Information);
                                                                                }
                                                                                else
                                                                                {
                                                                                    info.Arguments = "\\\\" + strIP + " -u " + strAdminUser + " -p " + strAdminPass + " -i cmd.exe /c C:\\OPTIONS\\CV_IP.BAT shutdown >C:\\OPTIONS\\CV_IP.TXT 2>&1";
                                                                                    oLog.AddEvent(intAnswer, strName, strSerial, "PSEXEC Script = " + "\\\\" + strIP + " -u " + strAdminUser + " -p ***** -i cmd.exe /c C:\\OPTIONS\\CV_IP.BAT shutdown >C:\\OPTIONS\\CV_IP.TXT 2>&1", LoggingType.Information);
                                                                                }
                                                                            }
                                                                            Process proc = Process.Start(info);
                                                                            oLog.AddEvent(intAnswer, strName, strSerial, "Waiting 3 minutes for IP Script to complete", LoggingType.Information);
                                                                            proc.WaitForExit(180000);   // Wait 3 minutes
                                                                            proc.Close();
                                                                            oLog.AddEvent(intAnswer, strName, strSerial, "PSEXEC Script Finished", LoggingType.Information);
                                                                        }
                                                                        else
                                                                            strError = "There was a problem with the NET USE script for the IP address assignment";
                                                                    }
                                                                    else
                                                                        strError = "A timeout occurred while running the script for the IP address assignment";
                                                                }
                                                            }
                                                            else
                                                                strError = "No DNS Servers have been configured for the domain, class and environment";
                                                        }
                                                        else
                                                        {
                                                            boolDHCP = false;
                                                            oLog.AddEvent(intAnswer, strName, strSerial, "DHCP address (" + strIP + ") is NOT responding (either powered off or unavailable)", LoggingType.Warning);
                                                        }
                                                    }
                                                    else
                                                        strError = "There is no DHCP address for this server";
                                                }

                                                if (strError == "")
                                                {
                                                    if (boolBlade == true || boolIsNexusSwitch == true)
                                                    {
                                                        oLog.AddEvent(intAnswer, strName, strSerial, "This is a blade", LoggingType.Debug);
                                                        int intAttempt = 0;
                                                        if (boolDell == false || boolIsNexusSwitch == true)
                                                        {
                                                            if (boolDell == false)
                                                                oLog.AddEvent(intAnswer, strName, strSerial, "This is not a DELL", LoggingType.Debug);
                                                            if (boolIsNexusSwitch == true)
                                                                oLog.AddEvent(intAnswer, strName, strSerial, "This is NEXUS", LoggingType.Debug);

                                                            if (oOperatingSystem.IsSolaris(intOS) == false)
                                                            {
                                                                oLog.AddEvent(intAnswer, strName, strSerial, "Turning off server to reconfigure switchport settings", LoggingType.Information);
                                                                // Should be shutting down at this point....
                                                                AssetPowerStatus powVirtualConnect = oAsset.PowerStatus(intAsset, intAnswer, intEnvironment, strScripts);
                                                                if (powVirtualConnect == AssetPowerStatus.Error)
                                                                {
                                                                    int intEnclosure = Int32.Parse(oAsset.GetServerOrBlade(intAsset, "enclosureid"));
                                                                    strError = "There was a problem getting the power status of the device";
                                                                    //string strHost = oAsset.GetEnclosure(intEnclosure, "virtual_connect");
                                                                    //strError = "Could not connect to Virtual Connect Manager IP ~ " + strHost;
                                                                }
                                                                else
                                                                {
                                                                    if (boolDHCP == false && powVirtualConnect != AssetPowerStatus.Off)
                                                                    {
                                                                        // The DHCP address did not reply and the server is not powered off...something is wrong here!
                                                                        strError = "The DHCP address did not reply and the server is not powered off ~ DHCP: " + strIP;
                                                                    }
                                                                    else
                                                                    {
                                                                        oLog.AddEvent(intAnswer, strName, strSerial, "Checking to see if IP address script CV_IP.BAT tunrned off server", LoggingType.Information);
                                                                        for (intAttempt = 0; intAttempt < 20 && powVirtualConnect != AssetPowerStatus.Off; intAttempt++)
                                                                        {
                                                                            powVirtualConnect = oAsset.PowerStatus(intAsset, intAnswer, intEnvironment, strScripts);
                                                                            int intAttemptLeft = (20 - intAttempt);
                                                                            oLog.AddEvent(intAnswer, strName, strSerial, "Server still on...waiting 3 seconds (Power = " + powVirtualConnect.ToString() + ") (" + intAttemptLeft.ToString() + " tries left)", LoggingType.Debug);
                                                                            Thread.Sleep(3000);
                                                                        }
                                                                        if (intAttempt == 20)
                                                                        {
                                                                            // Server is still on....manually shutdown using ILO
                                                                            oLog.AddEvent(intAnswer, strName, strSerial, "Server " + strName + " still on...shutting down using ILO", LoggingType.Warning);
                                                                            bool boolNetworkPowerOff = oFunction.ExecutePower(intServer, intAsset, false, "Power (Network / Power Off)", strName, intEnvironment, (boolDell ? strScripts : strScripts + strSub), intLogging, dsnAsset);
                                                                            if (boolNetworkPowerOff == false)
                                                                                strError = "There was a problem turning off the server via ILO";
                                                                            else
                                                                            {
                                                                                oLog.AddEvent(intAnswer, strName, strSerial, "Checking to see if server is turned off from ILO", LoggingType.Information);
                                                                                powVirtualConnect = oAsset.PowerStatus(intAsset, intAnswer, intEnvironment, strScripts);
                                                                                for (intAttempt = 0; intAttempt < 20 && powVirtualConnect != AssetPowerStatus.Off; intAttempt++)
                                                                                {
                                                                                    powVirtualConnect = oAsset.PowerStatus(intAsset, intAnswer, intEnvironment, strScripts);
                                                                                    int intAttemptLeft = (20 - intAttempt);
                                                                                    oLog.AddEvent(intAnswer, strName, strSerial, "Server still on...waiting 3 seconds (Power = " + powVirtualConnect.ToString() + ") (" + intAttemptLeft.ToString() + " tries left)", LoggingType.Debug);
                                                                                    Thread.Sleep(3000);
                                                                                }
                                                                                if (intAttempt == 20)
                                                                                {
                                                                                    // Server is still on....throw error
                                                                                    strError = "There was a problem shutting down the server to change virtual connect settings";
                                                                                    oLog.AddEvent(intAnswer, strName, strSerial, strError, LoggingType.Error);
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            else
                                                                oLog.AddEvent(intAnswer, strName, strSerial, "Solaris on Nexus does not require the server to be off", LoggingType.Information);

                                                            if (strError == "")
                                                            {
                                                                if (boolIsNexusSwitch == false)
                                                                {
                                                                    // Server is powered off...change VLAN
                                                                    oLog.AddEvent(intAnswer, strName, strSerial, "Server powered off...changing virtual connect setting to " + strVLANname, LoggingType.Information);
                                                                    if (oOperatingSystem.IsWindows(intOS) == true || oOperatingSystem.IsWindows2008(intOS) == true)
                                                                    {
                                                                        oLog.AddEvent(intAnswer, strName, strSerial, "Attempting to change virtual connect settings (AssetID: " + intAsset.ToString() + ") to " + strVLANname, LoggingType.Information);
                                                                        string strResultVC1 = oAsset.ExecuteVirtualConnectIP(intAsset, intAnswer, intEnvironment, strVLANname, 1, false, false, false);
                                                                        if (strResultVC1.Contains("ERROR") == true || strResultVC1 == "")
                                                                        {
                                                                            oLog.AddEvent(intAnswer, strName, strSerial, "Could not change virtual connect settings ~ (NIC#1) to " + strVLANname + "... " + strResultVC1 + " (Debug = " + oAsset.VirtualConnect() + ")", LoggingType.Error);
                                                                            strError = "Could not change virtual connect settings ~ (NIC#1): " + strResultVC1;
                                                                        }
                                                                        else
                                                                            oLog.AddEvent(intAnswer, strName, strSerial, "Server successfully changed virtual connect settings (NIC#1) to " + strVLANname, LoggingType.Information);
                                                                        string strResultVC2 = oAsset.ExecuteVirtualConnectIP(intAsset, intAnswer, intEnvironment, strVLANname, 2, false, false, false);
                                                                        if (strResultVC2.Contains("ERROR") == true || strResultVC2 == "")
                                                                        {
                                                                            oLog.AddEvent(intAnswer, strName, strSerial, "Could not change virtual connect settings ~ (NIC#2) to " + strVLANname + "... " + strResultVC2 + " (Debug = " + oAsset.VirtualConnect() + ")", LoggingType.Error);
                                                                            strError = "Could not change virtual connect settings ~ (NIC#2): " + strResultVC2;
                                                                        }
                                                                        else
                                                                            oLog.AddEvent(intAnswer, strName, strSerial, "Server successfully changed virtual connect settings (NIC#2) to " + strVLANname, LoggingType.Information);
                                                                    }
                                                                    else
                                                                    {
                                                                        oLog.AddEvent(intAnswer, strName, strSerial, "Attempting to change virtual connect settings (AssetID: " + intAsset.ToString() + ") to " + strVLANname, LoggingType.Information);
                                                                        string strResultVC1 = oAsset.ExecuteVirtualConnectIP(intAsset, intAnswer, intEnvironment, strVLANname, 1, false, false, false);
                                                                        if (strResultVC1.Contains("ERROR") == true || strResultVC1 == "")
                                                                        {
                                                                            oLog.AddEvent(intAnswer, strName, strSerial, "Could not change virtual connect settings ~ (NIC#1) to " + strVLANname + "... " + strResultVC1, LoggingType.Error);
                                                                            strError = "Could not change virtual connect settings ~ (NIC#1): " + strResultVC1;
                                                                        }
                                                                        else
                                                                            oLog.AddEvent(intAnswer, strName, strSerial, "Server successfully changed virtual connect settings (NIC#1) to " + strVLANname, LoggingType.Information);
                                                                        string strResultVC2 = oAsset.ExecuteVirtualConnectIP(intAsset, intAnswer, intEnvironment, strVLANname, 2, false, false, false);
                                                                        if (strResultVC2.Contains("ERROR") == true || strResultVC2 == "")
                                                                        {
                                                                            oLog.AddEvent(intAnswer, strName, strSerial, "Could not change virtual connect settings ~ (NIC#2) to " + strVLANname + "... " + strResultVC2, LoggingType.Error);
                                                                            strError = "Could not change virtual connect settings ~ (NIC#2): " + strResultVC2;
                                                                        }
                                                                        else
                                                                            oLog.AddEvent(intAnswer, strName, strSerial, "Server successfully changed virtual connect settings (NIC#2) to " + strVLANname, LoggingType.Information);
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    strError = ConfigureNexus(intAnswer, intServer, strName, strSerial, strIPChange, intOS, strRDPVLAN, boolBlade, strVLAN, intIPAddressBackup, intIPAddressCluster, intAsset, intAssetDR, intClass, intModel, dsSwitch);
                                                                }

                                                                if (strError == "")
                                                                {
                                                                    if (oOperatingSystem.IsSolaris(intOS) == true)
                                                                    {
                                                                        oLog.AddEvent(intAnswer, strName, strSerial, "Solaris builds are already on...no need for a power On", LoggingType.Debug);
                                                                    }
                                                                    else
                                                                    {
                                                                        // Change successful, power back on
                                                                        bool boolNetworkPowerOn = oFunction.ExecutePower(intServer, intAsset, true, "Power (Network / Power On)", strName, intEnvironment, (boolDell ? strScripts : strScripts + strSub), intLogging, dsnAsset);
                                                                        if (boolNetworkPowerOn == false)
                                                                            strError = "There was a problem turning on the server via ILO";
                                                                        else
                                                                        {
                                                                            oLog.AddEvent(intAnswer, strName, strSerial, "Power On script has been initiated via ILO = " + strILO, LoggingType.Information);
                                                                            // Check for Server to be back on
                                                                            AssetPowerStatus powVirtualConnectOn = oAsset.PowerStatus(intAsset, intAnswer, intEnvironment, strScripts);
                                                                            for (intAttempt = 0; intAttempt < 10 && powVirtualConnectOn != AssetPowerStatus.On; intAttempt++)
                                                                            {
                                                                                powVirtualConnectOn = oAsset.PowerStatus(intAsset, intAnswer, intEnvironment, strScripts);
                                                                                int intAttemptLeft = (10 - intAttempt);
                                                                                oLog.AddEvent(intAnswer, strName, strSerial, "Server still off...waiting 5 seconds (" + intAttemptLeft.ToString() + " tries left)", LoggingType.Debug);
                                                                                Thread.Sleep(3000);
                                                                            }
                                                                            if (intAttempt == 10)
                                                                            {
                                                                                // Server is still off....throw error
                                                                                strError = "There was a problem turning on the server after virtual connect settings change";
                                                                                oLog.AddEvent(intAnswer, strName, strSerial, strError, LoggingType.Error);
                                                                            }
                                                                            else
                                                                            {
                                                                                // Server is on...ping to make sure we can connect
                                                                                oLog.AddEvent(intAnswer, strName, strSerial, "The server has been powered on", LoggingType.Information);
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        else
                                                            oLog.AddEvent(intAnswer, strName, strSerial, "Skipping automatic switchport configuration (Dell = " + boolDell.ToString() + ", Nexus = " + boolIsNexusSwitch.ToString() + ")", LoggingType.Information);
                                                    }
                                                    else
                                                    {
                                                        // 1/5/2010: added code for physical devices
                                                        oLog.AddEvent(intAnswer, strName, strSerial, "Attempting to reconfigure the VLAN of a switch", LoggingType.Information);
                                                        // Loop through switchports and change them to new VLAN
                                                        StringBuilder strSwitchOutput = new StringBuilder();
                                                        foreach (DataRow drSwitch in dsSwitch.Tables[0].Rows)
                                                        {
                                                            if (drSwitch["interface"].ToString() != "")
                                                            {
                                                                try
                                                                {
                                                                    oLog.AddEvent(intAnswer, strName, strSerial, "Attempting to change interface [" + drSwitch["interface"].ToString() + "] on switch [" + drSwitch["name"].ToString() + "] to VLAN [" + strVLAN + "]...", LoggingType.Information);
                                                                    int intSwitch = Int32.Parse(drSwitch["id"].ToString());
                                                                    string strSwitchName = drSwitch["name"].ToString();
                                                                    string strSwitchInterface = drSwitch["interface"].ToString();
                                                                    int intSwitchNIC = Int32.Parse(drSwitch["nic"].ToString());
                                                                    int intSwitchNewVLAN = Int32.Parse(strVLAN);
                                                                    int intSwitchEnvironment = 1;
                                                                    Functions oFunctionSwitch = new Functions(0, dsn, intSwitchEnvironment);
                                                                    string strSwitchResult = oFunctionSwitch.ChangeVLAN(strSwitchName, strSerial, "", strSwitchInterface, intSwitchNewVLAN.ToString(), (drSwitch["is_ios"].ToString() == "1"), true, true, true, false);
                                                                    strSwitchOutput.Append(strSwitchResult);
                                                                    if (strSwitchResult.StartsWith("ERROR") == true)
                                                                    {
                                                                        strError = "There was a problem changing the VLAN of a switch port..." + strSwitchResult + " ~ Failure!! Interface [" + drSwitch["interface"].ToString() + "] on switch [" + drSwitch["name"].ToString() + "] was NOT changed to VLAN [" + strVLAN + "]";
                                                                        oLog.AddEvent(intAnswer, strName, strSerial, strError, LoggingType.Error);
                                                                    }
                                                                    else
                                                                    {
                                                                        oAsset.UpdateSwitchport(intSwitch, intAsset, SwitchPortType.Network, intSwitchNIC, intSwitchNewVLAN);
                                                                        oLog.AddEvent(intAnswer, strName, strSerial, "Success!! Interface [" + drSwitch["interface"].ToString() + "] on switch [" + drSwitch["name"].ToString() + "] was changed to VLAN [" + strVLAN + "].", LoggingType.Information);
                                                                    }
                                                                }
                                                                catch (Exception exSwitch)
                                                                {
                                                                    string strSwitchError = "Invalid format (is not a number) for either switch assetid (" + drSwitch["id"].ToString() + "), switch interface (" + drSwitch["interface"].ToString() + ") OR switch vlan (" + strVLAN + ")";
                                                                    strSwitchOutput.Append("ERROR: " + strSwitchError);
                                                                    strError = "There was a problem changing the VLAN of a switch port..." + strSwitchError + " ~ Failure!! Interface [" + drSwitch["interface"].ToString() + "] on switch [" + drSwitch["name"].ToString() + "] was NOT changed to VLAN [" + strVLAN + "]";
                                                                    oLog.AddEvent(intAnswer, strName, strSerial, strError, LoggingType.Error);
                                                                    break;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                strError = "The port of the switch was not defined...possibly a placeholder for physical relocation / configuration";
                                                                strSwitchOutput.Append("ERROR: " + strError);
                                                                break;
                                                            }
                                                        }
                                                        oServer.AddOutput(intServer, "SWITCH", strSwitchOutput.ToString());
                                                    }
                                                }

                                                if (strError == "")
                                                {
                                                    // Wait 5 seconds and then ping new address
                                                    strIP = oIPAddresses.GetName(intIPAddressBuild1, 0);
                                                    oLog.AddEvent(intAnswer, strName, strSerial, "Attempting to ping the newly assigned address [" + strIP + "]", LoggingType.Information);
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
                                                        if (oOperatingSystem.IsWindows(intOS) == true || oOperatingSystem.IsWindows2008(intOS) == true)
                                                        {
                                                            // 7th part - create BAT file to delete the copy (install_3.bat)
                                                            string strBatchIP3 = strScripts + strSub + intAnswer.ToString() + "_" + strNow + "_ip_3.bat";
                                                            StreamWriter oWriterIP5 = new StreamWriter(strBatchIP3);
                                                            if (oOperatingSystem.IsWindows2008(intOS) == true)
                                                            {
                                                                // Have to remove the "-i" parameter or else the script won't run (will come up with "the following helper dll cannot be loaded...XXXX.DLL" error)
                                                                oWriterIP5.WriteLine(strScripts + "psexec.exe \\\\" + strIPChange + " -u " + strAdminUser + " -p " + strAdminPass + " -h cmd.exe /c %windir%\\system32\\wscript.exe C:\\OPTIONS\\CV_IP.VBS");
                                                            }
                                                            else
                                                            {
                                                                oWriterIP5.WriteLine(strScripts + "psexec.exe \\\\" + strIPChange + " -u " + strAdminUser + " -p " + strAdminPass + " -i cmd.exe /c %windir%\\system32\\wscript.exe C:\\OPTIONS\\CV_IP.VBS");
                                                            }
                                                            if (boolNewIP == true)
                                                            {
                                                                /*
                                                                oWriterIP5.WriteLine("del \\\\" + strIP + "\\C$\\OPTIONS\\CV_IP.BAT");
                                                                oWriterIP5.WriteLine("del \\\\" + strIP + "\\C$\\OPTIONS\\CV_IP.VBS");
                                                                if (boolBlade == true)
                                                                    oWriterIP5.WriteLine("del \\\\" + strIP + "\\C$\\OPTIONS\\CV_IP_SHUTDOWN.VBS");
                                                                oWriterIP5.WriteLine("del \\\\" + strIP + "\\C$\\OPTIONS\\CV_IP_EXECUTE.BAT");
                                                                oWriterIP5.WriteLine("del \\\\" + strIP + "\\C$\\OPTIONS\\CV_IP.EXE");
                                                                */
                                                            }
                                                            else
                                                            {
                                                                //oWriterIP5.WriteLine("del \\\\" + strIP + "\\C$\\OPTIONS\\CV_IP.BAT");
                                                                //oWriterIP5.WriteLine("del \\\\" + strIP + "\\C$\\OPTIONS\\CV_IP.VBS");
                                                            }
                                                            oWriterIP5.WriteLine("net use \\\\" + strIP + "\\C$ /dele");
                                                            oWriterIP5.WriteLine("net use \\\\" + strIPChange + "\\C$ /dele");
                                                            oWriterIP5.Flush();
                                                            oWriterIP5.Close();
                                                            // 8th part - run the batch file to perform copy
                                                            ProcessStartInfo infoIPdelete = new ProcessStartInfo(strScripts + "psexec");
                                                            infoIPdelete.WorkingDirectory = strScripts;
                                                            if (oOperatingSystem.IsWindows2008(intOS) == true)
                                                            {
                                                                // Have to remove the "-i" parameter or else the script won't run (will come up with "the following helper dll cannot be loaded...XXXX.DLL" error)
                                                                infoIPdelete.Arguments = "cmd.exe /c " + strBatchIP3;
                                                            }
                                                            else
                                                                infoIPdelete.Arguments = "-i cmd.exe /c " + strBatchIP3;
                                                            Process procIPdelete = Process.Start(infoIPdelete);
                                                            bool boolTimeout = false;
                                                            procIPdelete.WaitForExit(intTimeoutDefault);
                                                            if (procIPdelete.HasExited == false)
                                                            {
                                                                procIPdelete.Kill();
                                                                boolTimeout = true;
                                                            }
                                                            procIPdelete.Close();
                                                        }

                                                        strResult = oOnDemand.GetStep(intStepID, "done");
                                                        oLog.AddEvent(intAnswer, strName, strSerial, strResult, LoggingType.Information);
                                                    }
                                                    else
                                                    {
                                                        strError = "There was a problem assigning the IP Address ~ (ping " + strIPChange + " did not respond to ping)";
                                                        oLog.AddEvent(intAnswer, strName, strSerial, strError, LoggingType.Error);
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (boolIsNexusSwitch == true)
                                            {
                                                if (dsBuildRDP.Tables[0].Rows.Count > 0)
                                                {
                                                    if (boolIsNexusSwitch == false)
                                                        strRDPVLAN = dsBuildRDP.Tables[0].Rows[0]["blade_vlan"].ToString();
                                                    else
                                                        strRDPVLAN = dsBuildRDP.Tables[0].Rows[0]["dell_vlan"].ToString();
                                                }
                                                oLog.AddEvent(intAnswer, strName, strSerial, "Server IP configuration has already been changed.  Updating all switchports...", LoggingType.Information);
                                                strError = ConfigureNexus(intAnswer, intServer, strName, strSerial, strIPChange, intOS, strRDPVLAN, boolBlade, strVLAN, intIPAddressBackup, intIPAddressCluster, intAsset, intAssetDR, intClass, intModel, dsSwitch);

                                                if (strError == "")
                                                {
                                                    strResult = oOnDemand.GetStep(intStepID, "done");
                                                    oLog.AddEvent(intAnswer, strName, strSerial, "Switchports have been updated", LoggingType.Information);
                                                }
                                            }
                                            else
                                            {
                                                strResult = oOnDemand.GetStep(intStepID, "done");
                                                oLog.AddEvent(intAnswer, strName, strSerial, "Server IP configuration has already been changed", LoggingType.Information);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (boolBlade == true)
                                        {
                                            strResult = "There was no IP Address allocated for this device";
                                            oLog.AddEvent(intAnswer, strName, strSerial, strResult, LoggingType.Information);
                                        }
                                        else
                                        {
                                            strResult = "There was no IP Address allocated or automatic assignment is not available for this asset";
                                            oLog.AddEvent(intAnswer, strName, strSerial, strResult, LoggingType.Information);
                                        }
                                    }
                                    //oLog.AddEvent(intAnswer, strName, strSerial, strResult, LoggingType.Information);
                                    AddResult(intServer, intStep, intType, strResult, strError);
                                    break;
                                case 11:    // Add Local Admin Groups 
                                    oLog.AddEvent(intAnswer, strName, strSerial, "Starting Step 11 (Add Local Admin Groups)", LoggingType.Information);
                                    if (oOperatingSystem.IsWindows(intOS) == true || oOperatingSystem.IsWindows2008(intOS) == true)
                                    {
                                        string strGroupIP = strIP;
                                        if (intIPAddressBuild1 > 0)
                                            strGroupIP = oIPAddresses.GetName(intIPAddressBuild1, 0);
                                        if (strGroupIP != "")
                                        {
                                            // 1st part - create VBS file to copy to server
                                            StreamWriter oWriter1 = new StreamWriter(strFile);
                                            oWriter1.WriteLine("On Error Resume Next");

                                            string strMnemonicCode = "";
                                            if (boolPNC == true)
                                            {
                                                // PNC GROUPS
                                                strMnemonicCode = oMnemonic.Get(intMnemonic, "factory_code").ToUpper();
                                                string strOrganizationCode = oOrganization.Get(intOrganization, "code");
                                                //string strNameDLG = "DLG_URA_" + strOrganizationCode + "_" + strMnemonicCode + "_";
                                                string strNameDLG = "GSLfsaSP_" + strMnemonicCode + "_";

                                                string strRemoteDesktopGroups = "";
                                                DataSet dsAccounts = oServer.GetAccounts(intServer);
                                                foreach (DataRow drAccount in dsAccounts.Tables[0].Rows)
                                                {
                                                    string[] strDomainGroupArray = drAccount["domaingroups"].ToString().Split(strSplit);
                                                    for (int ii = 0; ii < strDomainGroupArray.Length; ii++)
                                                    {
                                                        // Get all the groups where "_1" was suffixed (for remote desktop)
                                                        if (strDomainGroupArray[ii].Trim() != "")
                                                        {
                                                            string strDomainGroup = strDomainGroupArray[ii].Trim();
                                                            if (strDomainGroup.Contains("_1") == true)
                                                            {
                                                                strDomainGroup = strDomainGroup.Substring(0, strDomainGroup.IndexOf("_"));
                                                                bool boolRemoteDesktopExists = false;
                                                                string[] strRemoteDesktopGroupArray = strRemoteDesktopGroups.Split(strSplit);
                                                                for (int jj = 0; jj < strRemoteDesktopGroupArray.Length; jj++)
                                                                {
                                                                    if (strRemoteDesktopGroupArray[jj].Trim() != "" && strRemoteDesktopGroupArray[jj].Trim() == strDomainGroup)
                                                                    {
                                                                        boolRemoteDesktopExists = true;
                                                                        break;
                                                                    }
                                                                }
                                                                if (boolRemoteDesktopExists == false)
                                                                {
                                                                    if (strRemoteDesktopGroups != "")
                                                                        strRemoteDesktopGroups += ";";
                                                                    strRemoteDesktopGroups += strDomainGroup;
                                                                }
                                                            }
                                                        }
                                                    }
                                                }

                                                bool boolSQL = false;
                                                foreach (DataRow drComponent in dsComponents.Tables[0].Rows)
                                                {
                                                    if (drComponent["sql"].ToString() == "1")
                                                    {
                                                        boolSQL = true;
                                                        break;
                                                    }
                                                }

                                                if (boolSQL == true)
                                                {
                                                    oWriter1.WriteLine("Set objGroup6 = GetObject(\"WinNT://localhost/Administrators\")");
                                                    oWriter1.WriteLine("objGroup6.Add (\"WinNT://" + strDomain + "/GSLfsaSP_DTG_SQLServerOps\")");
                                                    oWriter1.WriteLine("objGroup6.Add (\"WinNT://" + strDomain + "/GSLfsaSP_DTG_SQLServerProjects\")");
                                                    oWriter1.WriteLine("objGroup6.Add (\"WinNT://" + strDomain + "/GSLfsaSP_DTG_Infrastructure\")");
                                                    oWriter1.WriteLine("objGroup6.Add (\"WinNT://" + strDomain + "/GSLfsaSP_DTG_InfraAppAccts\")");
                                                    if (oClass.IsProd(intClass))
                                                        oWriter1.WriteLine("objGroup6.Add (\"WinNT://" + strDomain + "/GSLfsaSP_DTG_TechDev2\")");
                                                    else
                                                        oWriter1.WriteLine("objGroup6.Add (\"WinNT://" + strDomain + "/GSLfsaSP_DTG_TechDev1\")");
                                                    // ArcSight service account
                                                    oWriter1.WriteLine("Set objGroup9 = GetObject(\"WinNT://localhost/Event Log Readers\")");
                                                    oWriter1.WriteLine("objGroup9.Add (\"WinNT://" + strDomain + "/XSASGPARCSIGHT\")");
                                                }
                                                else
                                                {

                                                    oWriter1.WriteLine("Set objGroup1 = GetObject(\"WinNT://localhost/Remote Desktop Users\")");
                                                    string[] strRemoteDesktopGroup = strRemoteDesktopGroups.Split(strSplit);
                                                    for (int ii = 0; ii < strRemoteDesktopGroup.Length; ii++)
                                                    {
                                                        // Add all remote desktop groups
                                                        if (strRemoteDesktopGroup[ii].Trim() != "")
                                                            oWriter1.WriteLine("objGroup1.Add (\"WinNT://" + strDomain + "/" + strNameDLG + strRemoteDesktopGroup[ii].Trim() + "\")");
                                                    }

                                                    oWriter1.WriteLine("Set objGroup2 = GetObject(\"WinNT://localhost/AppSupport\")");
                                                    oWriter1.WriteLine("objGroup2.Add (\"WinNT://" + strDomain + "/" + strNameDLG + "AppSupport\")");

                                                    oWriter1.WriteLine("Set objGroup3 = GetObject(\"WinNT://localhost/AppUsers\")");
                                                    oWriter1.WriteLine("objGroup3.Add (\"WinNT://" + strDomain + "/" + strNameDLG + "AppUsers\")");

                                                    oWriter1.WriteLine("Set objGroup4 = GetObject(\"WinNT://localhost/Developers\")");
                                                    oWriter1.WriteLine("objGroup4.Add (\"WinNT://" + strDomain + "/" + strNameDLG + "Developers\")");

                                                    //oWriter1.WriteLine("Set objGroup5 = GetObject(\"WinNT://localhost/Promoters\")");
                                                    //oWriter1.WriteLine("objGroup5.Add (\"WinNT://" + strDomain + "/" + strNameDLG + "Promoters\")");

                                                    oWriter1.WriteLine("Set objGroup6 = GetObject(\"WinNT://localhost/Administrators\")");
                                                    oWriter1.WriteLine("objGroup6.Add (\"WinNT://" + strDomain + "/" + strNameDLG + "AuthProbMgmt\")");
                                                    oWriter1.WriteLine("objGroup6.Add (\"WinNT://" + strDomain + "/" + strNameDLG + "AuthPromoters\")");
                                                }

                                                foreach (DataRow drComponent in dsComponents.Tables[0].Rows)
                                                {
                                                    if (drComponent["iis"].ToString() == "1")
                                                    {
                                                        oWriter1.WriteLine("objGroup6.Add (\"WinNT://" + strDomain + "/DLG_URA_CIO_MHS_AppSupport\")");
                                                        break;
                                                    }
                                                }
                                                foreach (DataRow drComponent in dsComponents.Tables[0].Rows)
                                                {
                                                    if (drComponent["factory_code"].ToString() == "X")
                                                    {
                                                        oWriter1.WriteLine("objGroup6.Add (\"WinNT://" + strDomain + "/GSLsrvSP_CXT\")");
                                                        break;
                                                    }
                                                }

                                                if (boolSQL == true)
                                                {
                                                    oWriter1.WriteLine("Set objGroup6 = Nothing");
                                                    oWriter1.WriteLine("Set objGroup9 = Nothing");
                                                }
                                                else
                                                {
                                                    oWriter1.WriteLine("Set objGroup1 = Nothing");
                                                    oWriter1.WriteLine("Set objGroup2 = Nothing");
                                                    oWriter1.WriteLine("Set objGroup3 = Nothing");
                                                    oWriter1.WriteLine("Set objGroup4 = Nothing");
                                                    oWriter1.WriteLine("Set objGroup5 = Nothing");
                                                    oWriter1.WriteLine("Set objGroup6 = Nothing");
                                                }
                                                if (intClusterID > 0 && oSetting.Get("configure_cluster") == "1")
                                                {
                                                    oWriter1.WriteLine("Set objGroup7 = Nothing");
                                                    oWriter1.WriteLine("Set objGroup8 = Nothing");
                                                }
                                            }
                                            else
                                            {
                                                // NATIONAL CITY GROUPS
                                                oWriter1.WriteLine("Set objGroup = GetObject(\"WinNT://localhost/Administrators\")");
                                                oWriter1.WriteLine("objGroup.Add (\"WinNT://" + strDomain + "/Domain Admins\")");
                                                oWriter1.WriteLine("objGroup.Add (\"WinNT://" + strDomain + "/GSGu_" + strName + "Adm\")");
                                                oWriter1.WriteLine("objGroup.Add (\"WinNT://" + strDomain + "/GSGu_PSAUT\")");
                                                oWriter1.WriteLine("objGroup.Add (\"WinNT://" + strDomain + "/GSGu_PSNAV\")");
                                                oWriter1.WriteLine("objGroup.Add (\"WinNT://" + strDomain + "/ORGGUXXX_PSNAV\")");
                                                oWriter1.WriteLine("objGroup.Add (\"WinNT://" + strDomain + "/ORGGUXXX_PSIRS\")");
                                                oWriter1.WriteLine("objGroup.Add (\"WinNT://" + strDomain + "/ORGGUXXX_PSIMP\")");
                                                oWriter1.WriteLine("objGroup.Add (\"WinNT://" + strDomain + "/ORGGUXXX_PSTR3\")");
                                                foreach (DataRow drComp in dsComponents.Tables[0].Rows)
                                                {
                                                    if (drComp["sql"].ToString() == "1" || drComp["dbase"].ToString() == "1")
                                                    {
                                                        oWriter1.WriteLine("objGroup.Add (\"WinNT://" + strDomain + "/GSGu_PSDBASQL\")");
                                                        oWriter1.WriteLine("objGroup.Add (\"WinNT://" + strDomain + "/GSGu_AISDBASQL\")");
                                                        break;
                                                    }
                                                }
                                                oWriter1.WriteLine("Set objGroup = Nothing");
                                            }

                                            oWriter1.WriteLine("On Error GoTo 0");
                                            oWriter1.WriteLine("wscript.quit(1)");
                                            oWriter1.Flush();
                                            oWriter1.Close();

                                            //   Execute the Script
                                            bool boolScriptGroupError = true;
                                            for (int kk = 0; kk < 5 && boolScriptGroupError == true; kk++)
                                            {
                                                int intScriptGroup = oFunction.ExecuteVBScript(intServer, false, true, "GROUPS", strName, strSerial, strGroupIP, strFile, strFilePath, "Groups", "%windir%\\system32\\wscript.exe", "OPTIONS\\CV_GROUPS", "VBS", "", strScripts, strAdminUser, strAdminPass, 5, (oOperatingSystem.IsWindows2008(intOS) == false), false, intLogging, boolDeleteScriptFiles);
                                                AuditStatus oScriptGroup = (AuditStatus)intScriptGroup;
                                                if (oScriptGroup == AuditStatus.Success || oScriptGroup == AuditStatus.Warning)
                                                {
                                                    strResult = oOnDemand.GetStep(intStepID, "done");
                                                    oLog.AddEvent(intAnswer, strName, strSerial, strResult, LoggingType.Information);
                                                    strError = "";
                                                    boolScriptGroupError = false;
                                                    if (boolPNC)
                                                    {
                                                        // Send Email for PNC AD Configuration
                                                        oFunction.SendPNCAD("GSGu_" + strMnemonicCode + "_", intDomainEnvironment, false);
                                                    }
                                                }
                                                else if (oScriptGroup == AuditStatus.NetUseError)
                                                    strError = "There was a problem configuring the groups (the NET USE script failed to map a drive)";
                                                else
                                                    strError = "There was an error configuring the groups ~ (Error # " + intScriptGroup.ToString() + ")";

                                                if (boolScriptGroupError == true)
                                                {
                                                    // Wait 30 seconds before trying again
                                                    Thread.Sleep(30000);
                                                }
                                            }
                                        }
                                        else
                                            oLog.AddEvent(intAnswer, strName, strSerial, "Cannot resolve NAME or IP", LoggingType.Information);
                                    }
                                    else
                                        strResult = "This step is only available for distributed servers";
                                    oLog.AddEvent(intAnswer, strName, strSerial, strResult, LoggingType.Information);
                                    AddResult(intServer, intStep, intType, strResult, strError);
                                    break;
                                case 12:    // Install Components 
                                    DataSet dsComponentsInstall = oServerName.GetComponentDetailSelected(intServer, 1);
                                    if (oOperatingSystem.IsWindows(intOS) == true || oOperatingSystem.IsWindows2008(intOS) == true)
                                    {
                                        bool boolFirst = false;
                                        if (dsComponentsInstall.Tables[0].Rows.Count == 0)
                                            AddResult(intServer, intStep, intType, "No components to install", strError);
                                        else
                                        {
                                            bool boolDone = true;
                                            foreach (DataRow drComponent in dsComponentsInstall.Tables[0].Rows)
                                            {
                                                int intDetail = Int32.Parse(drComponent["detailid"].ToString());
                                                if (drComponent["done"].ToString() == "-10")
                                                {
                                                    strError = "There was a problem installing " + drComponent["name"].ToString();
                                                    boolDone = false;
                                                }
                                                else if (drComponent["done"].ToString() == "-2")
                                                {
                                                    boolFirst = true;
                                                    oServerName.UpdateComponentDetailSelected(intServer, intDetail, -1);
                                                    boolDone = false;
                                                }
                                                else if (drComponent["done"].ToString() == "-1" || drComponent["done"].ToString() == "0")
                                                {
                                                    boolDone = false;
                                                    oLog.AddEvent(intAnswer, strName, strSerial, "Still Installing " + drComponent["name"].ToString() + "...", LoggingType.Information);
                                                }
                                                else if (drComponent["done"].ToString() == "1")
                                                {
                                                    oOnDemand.UpdateStepDoneServerResult(intServer, intStep, "Successfully installed " + drComponent["name"].ToString() + "<br/>", true);
                                                    oServerName.UpdateComponentDetailSelected(intServer, intDetail, 2);
                                                }
                                            }
                                            if (boolDone == true)
                                            {
                                                oOnDemand.UpdateStepDoneServer(intServer, intStep, "", 0, true, false);
                                                oServer.NextStep(intServer);
                                            }
                                            if (boolFirst == true)
                                                oLog.AddEvent(intAnswer, strName, strSerial, "Starting Step " + intStep.ToString() + " (Install Components)", LoggingType.Information);
                                        }
                                    }
                                    else
                                    {
                                        oLog.AddEvent(intAnswer, strName, strSerial, "Starting Step " + intStep.ToString() + " (Install Components)", LoggingType.Information);
                                        if (dsComponentsInstall.Tables[0].Rows.Count == 0)
                                            AddResult(intServer, intStep, intType, "No components to install", strError);
                                        else
                                        {
                                            foreach (DataRow drComponent in dsComponentsInstall.Tables[0].Rows)
                                            {
                                                int intDetail = Int32.Parse(drComponent["detailid"].ToString());
                                                oOnDemand.UpdateStepDoneServerResult(intServer, intStep, "Successfully installed " + drComponent["name"].ToString() + "<br/>", true);
                                                oServerName.UpdateComponentDetailSelected(intServer, intDetail, 2);
                                            }
                                            oOnDemand.UpdateStepDoneServer(intServer, intStep, "", 0, true, false);
                                            oServer.NextStep(intServer);
                                        }
                                    }
                                    break;
                                case 13:    // Move to OU
                                    oLog.AddEvent(intAnswer, strName, strSerial, "Starting Step 13 (Move to OU)", LoggingType.Information);
                                    if (oOperatingSystem.IsWindows(intOS) == true || oOperatingSystem.IsWindows2008(intOS) == true)
                                    {
                                        if (boolPNC == true)
                                            strResult = "Currently, this step is not configured for PNC builds";
                                        else
                                        {
                                            //if (intDomainEnvironment == (int)CurrentEnvironment.CORPDEV || intDomainEnvironment == (int)CurrentEnvironment.CORPTEST || intDomainEnvironment == (int)CurrentEnvironment.ECADDEV)
                                            //{
                                            //    SearchResultCollection oResults = oAD.ComputerSearch(strName);
                                            //    for (int ii = 0; ii < 10 && oResults.Count != 1; ii++)
                                            //    {
                                            //        Thread.Sleep(3000);
                                            //        oResults = oAD.ComputerSearch(strName);
                                            //    }
                                            //    if (oResults.Count == 1)
                                            //    {
                                            //        string strMove = "";
                                            //        if (intSubApplication > 0)
                                            //            strMove = oServerName.GetSubApplication(intSubApplication, "ad_move_location");
                                            //        if (strMove == "" && intApplication > 0)
                                            //            strMove = oServerName.GetApplication(intApplication, "ad_move_location");
                                            //        if (dsComponents.Tables[0].Rows.Count > 0)
                                            //        {
                                            //            foreach (DataRow drComponent in dsComponents.Tables[0].Rows)
                                            //            {
                                            //                strMove = drComponent["ad_move_location"].ToString();
                                            //                if (strMove != "")
                                            //                    break;
                                            //            }
                                            //        }
                                            //        if (strMove == "")
                                            //            strMove = oAD.GetComputerOU(strName);
                                            //        oAD.MoveServer(oResults[0].GetDirectoryEntry(), strMove);
                                            //        strResult = "The server object was moved to &quot;" + strMove + "&quot;";
                                            //    }
                                            //    else
                                            //        strResult = "The server object was NOT moved to an OU";
                                            //}
                                            //else
                                                strResult = "ClearView does not have permission to modify OU objects in " + oDomain.Get(intDomain, "name");
                                        }
                                    }
                                    else
                                    {
                                        strResult = "This step is only available for distributed servers";
                                    }
                                    oLog.AddEvent(intAnswer, strName, strSerial, strResult, LoggingType.Information);
                                    AddResult(intServer, intStep, intType, strResult, strError);
                                    break;
                                case 14:    // Audit
                                    if (intAuditCount < intAuditCounts)
                                    {
                                        bool boolSAN = (oForecast.IsStorage(intAnswer) == true && oForecast.GetAnswer(intAnswer, "storage") == "1");
                                        oLog.AddEvent(intAnswer, strName, strSerial, "Starting Step " + intStep.ToString() + " (Audit)", LoggingType.Information);
                                        // RUN AUDIT TASKS
                                        AuditThread oAuditThread = new AuditThread(intServer, strName, strSerial, intIPAddressBuild1, true, strIP, intClass, intEnv, intModel, intOS, intSP, intAddress, boolSAN, oForecast.IsHACluster(intAnswer), (intTSM == 1 && oForecast.GetAnswer(intAnswer, "backup") == "1"), intStep, intRequest, intServerAuditErrorService, intResourceRequestApprove, intAssignPage, intViewPage, strScripts, strSub, strAdminUser, strAdminPass, intEnvironment, intLogging, dsn, dsnAsset, dsnIP, dsnServiceEditor, boolDeleteScriptFiles, boolMultiThreadedAudit, this, false);
                                        ThreadStart oAuditThreadStart = new ThreadStart(oAuditThread.Begin);
                                        Thread oAuditThreadProcess = new Thread(oAuditThreadStart);
                                        oAuditThreadProcess.Start();
                                    }
                                    else
                                    {
                                        // The current number of audits running is at maximum.  Delete the current step to REDO (will keep checking until number goes down).
                                        oOnDemand.DeleteStepDoneServer(intServer, intStep);
                                    }
                                    break;
                                case 15:    // File Cleanup and Notify
                                    oLog.AddEvent(intAnswer, strName, strSerial, "Starting Step 15 (File Cleanup and Notify)", LoggingType.Information);
                                    // Now that the audit is complete, go ahead and decrease the audit count
                                    try
                                    {
                                        if (boolPNC == true)
                                        {
                                            // 10/26/2009: Attempt to auto-register DNS
                                            oLog.AddEvent(intAnswer, strName, strSerial, "Creating DNS Records", LoggingType.Information);
                                            System.Net.NetworkCredential oCredentials = new System.Net.NetworkCredential(oVariable.ADUser(), oVariable.ADPassword(), oVariable.Domain());
                                            ClearViewWebServices oWebService = new ClearViewWebServices();
                                            oWebService.Timeout = Timeout.Infinite;
                                            oWebService.Credentials = oCredentials;
                                            oWebService.Url = oVariable.WebServiceURL();
                                            bool boolDNS_QIP = oSetting.IsDNS_QIP();
                                            bool boolDNS_Bluecat = oSetting.IsDNS_Bluecat();

                                            int intIPAddressDNS = intIPAddressBuild1;
                                            if (intIPAddressDNS == 0 && intIPAddressFinal1 > 0)
                                                intIPAddressDNS = intIPAddressFinal1;
                                            if (intIPAddressDNS > 0)
                                            {
                                                strIP = oIPAddresses.GetName(intIPAddressDNS, 0);
                                                if (boolDNS_QIP == true)
                                                {
                                                    // QIP
                                                    oLog.AddEvent(intAnswer, strName, strSerial, "Calling Web Service Function CreateDNSforPNC(" + strIP + ", " + strName + ", Server, , " + oVariable.DNS_Domain() + ", " + oVariable.DNS_NameService() + ", " + oVariable.DNS_DynamicDNSUpdate() + ", " + intUser.ToString() + ", true) on " + oVariable.WebServiceURL(), LoggingType.Information);
                                                    string strDNS = oWebService.CreateDNSforPNC(strIP, strName, "Server", "", oVariable.DNS_Domain(), oVariable.DNS_NameService(), oVariable.DNS_DynamicDNSUpdate(), intUser, 0, true);
                                                    if (strDNS == "SUCCESS" || strDNS.StartsWith("***DUPLICATE") == true)
                                                    {
                                                        oLog.AddEvent(intAnswer, strName, strSerial, "QIP DNS Record = SUCCESS", LoggingType.Information);
                                                        // The script ran successfully
                                                        if (boolDNS_Bluecat == false)
                                                            oServer.UpdateDNS(intServer, 1, "Completed");
                                                        strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DEVELOPER_ALERT");
                                                        if (intLogging > 1)
                                                            oFunction.SendEmail("QIP DNS Automation Success", strEMailIdsBCC, "", "", "QIP DNS Automation Success", "<p>There was no issue creating the QIP DNS Record for " + strName + " (SERVERID: " + intServer.ToString() + ") (ANSWERID: " + intAnswer.ToString() + ")</p><p>Error: " + strDNS + "</p><p>NOTE: This message is simply to notify you that the record was generated...no action to be taken.</p>", true, false);
                                                    }
                                                    else
                                                    {
                                                        if (strDNS.StartsWith("***CONFLICT") == true)
                                                        {
                                                            oLog.AddEvent(intAnswer, strName, strSerial, "QIP DNS Record = CONFLICT", LoggingType.Warning);
                                                            // A conflict occurred...awaiting the service technician to fix
                                                            if (boolDNS_Bluecat == false)
                                                                oServer.UpdateDNS(intServer, 0, "Conflict");
                                                            strError = "A conflict was encountered when trying to auto-register the QIP DNS object";
                                                            strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DEVELOPER_WARNING");
                                                            if (intLogging > 0)
                                                                oFunction.SendEmail("QIP DNS Automation Conflict", strEMailIdsBCC, "", "", "QIP DNS Automation Conflict", "<p>There was a CONFLICT creating the QIP DNS Record for " + strName + " (SERVERID: " + intServer.ToString() + ") (ANSWERID: " + intAnswer.ToString() + ")</p><p>Error: " + strDNS + "</p><p>NOTE: A request should have been generated and assigned to a DNS technician.</p>", true, false);
                                                        }
                                                        else if (strDNS.ToUpper().Contains("SUBNET FOR") == true)
                                                        {
                                                            oLog.AddEvent(intAnswer, strName, strSerial, "QIP DNS Record = SUBNET ERROR: " + strDNS, LoggingType.Error);
                                                            // This occurs if a subnet does not exist.  Will need to be routed for assignment and continue.
                                                            if (boolForceDNSSuccess == true)
                                                            {
                                                                if (boolDNS_Bluecat == false)
                                                                    oServer.UpdateDNS(intServer, 1, "Registered");
                                                            }
                                                            else
                                                            {
                                                                if (boolDNS_Bluecat == false)
                                                                    oServer.UpdateDNS(intServer, -10, strDNS);
                                                                strError = "A subnet error was encountered when trying to auto-register the QIP DNS object ~ " + strDNS;
                                                                strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DEVELOPER_WARNING");
                                                                oFunction.SendEmail("QIP DNS Automation Subnet Error", strEMailIdsBCC, "", "", "QIP DNS Automation Subnet Error", "<p>There was a SUBNET ERROR creating the QIP DNS Record for " + strName + " (SERVERID: " + intServer.ToString() + ") (ANSWERID: " + intAnswer.ToString() + ")</p><p>Error: " + strDNS + "</p><p>NOTE: A request should have been generated and assigned to a DNS technician.</p>", true, false);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            oLog.AddEvent(intAnswer, strName, strSerial, "QIP DNS Record = ERROR: " + strDNS, LoggingType.Error);
                                                            // An error was encountered...log the error
                                                            if (boolForceDNSSuccess == true)
                                                            {
                                                                if (boolDNS_Bluecat == false)
                                                                    oServer.UpdateDNS(intServer, 1, "Registered");
                                                            }
                                                            else
                                                            {
                                                                if (boolDNS_Bluecat == false)
                                                                    oServer.UpdateDNS(intServer, -1, strDNS);
                                                                strError = "An error was encountered when trying to auto-register the QIP DNS object ~ " + strDNS;
                                                                strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DEVELOPER_ERROR,EMAILGRP_PROVISIONING_SUPPORT");
                                                                oFunction.SendEmail("QIP DNS Automation Error", strEMailIdsBCC, "", "", "QIP DNS Automation Error", "<p>There was an ERROR creating the QIP DNS Record for " + strName + " (SERVERID: " + intServer.ToString() + ") (ANSWERID: " + intAnswer.ToString() + ")</p><p>Error: " + strDNS + "</p><p>NOTE: Since this is an unexpected error, there was no task initiated to fix this problem. You must correct it yourself.</p>", true, false);
                                                            }
                                                        }
                                                    }
                                                    oLog.AddEvent(intAnswer, strName, strSerial, "QIP DNS Record Finished", LoggingType.Information);
                                                }
                                                if (strError == "")
                                                {
                                                    if (boolDNS_Bluecat == true)
                                                    {
                                                        // BlueCat
                                                        strMACAddress1 = oAsset.GetServerOrBlade(intAsset, "macaddress").ToUpper();
                                                        string strIPDescription = oIPAddresses.GetDescription(intIPAddressDNS, strName, intAsset, dsnAsset, "", intEnvironment);
                                                        oLog.AddEvent(intAnswer, strName, strSerial, "Calling Web Service Function CreateBluecatDNS(" + strIP + ", " + strName + ", " + strIPDescription + ", " + strMACAddress1 + ") on " + oVariable.WebServiceURL(), LoggingType.Information);
                                                        string strDNS = oWebService.CreateBluecatDNS(strIP, strName, strIPDescription, strMACAddress1);
                                                        if (strDNS == "SUCCESS" || strDNS.StartsWith("***DUPLICATE") == true)
                                                        {
                                                            oLog.AddEvent(intAnswer, strName, strSerial, "BlueCat DNS Record = SUCCESS", LoggingType.Information);
                                                            // The script ran successfully
                                                            oServer.UpdateDNS(intServer, 1, "Completed");
                                                            strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DEVELOPER_ALERT");
                                                            if (intLogging > 1)
                                                                oFunction.SendEmail("BlueCat DNS Automation Success", strEMailIdsBCC, "", "", "BlueCat DNS Automation Success", "<p>There was no issue creating the BlueCat DNS Record for " + strName + " (SERVERID: " + intServer.ToString() + ") (ANSWERID: " + intAnswer.ToString() + ")</p><p>Error: " + strDNS + "</p><p>NOTE: This message is simply to notify you that the record was generated...no action to be taken.</p>", true, false);
                                                        }
                                                        else
                                                        {
                                                            if (strDNS.StartsWith("***CONFLICT") == true)
                                                            {
                                                                oLog.AddEvent(intAnswer, strName, strSerial, "BlueCat DNS Record = CONFLICT", LoggingType.Warning);
                                                                // A conflict occurred...awaiting the service technician to fix
                                                                oServer.UpdateDNS(intServer, 0, "Conflict");
                                                                strError = "A conflict was encountered when trying to auto-register the BlueCat DNS object ~ " + strDNS;
                                                                strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DEVELOPER_WARNING");
                                                                if (intLogging > 1)
                                                                    oFunction.SendEmail("BlueCat DNS Automation Conflict", strEMailIdsBCC, "", "", "BlueCat DNS Automation Conflict", "<p>There was a CONFLICT creating the BlueCat DNS Record for " + strName + " (SERVERID: " + intServer.ToString() + ") (ANSWERID: " + intAnswer.ToString() + ")</p><p>Error: " + strDNS + "</p><p>NOTE: A request should have been generated and assigned to a DNS technician.</p>", true, false);
                                                            }
                                                            else
                                                            {
                                                                oLog.AddEvent(intAnswer, strName, strSerial, "BlueCat DNS Record = ERROR: " + strDNS, LoggingType.Error);
                                                                // An error was encountered...log the error
                                                                if (boolForceDNSSuccess == true)
                                                                    oServer.UpdateDNS(intServer, 1, "Registered");
                                                                else
                                                                {
                                                                    oServer.UpdateDNS(intServer, -1, strDNS);
                                                                    strError = "An error was encountered when trying to auto-register the BlueCat DNS object ~ " + strDNS;
                                                                    strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DEVELOPER_ERROR,EMAILGRP_PROVISIONING_SUPPORT");
                                                                    oFunction.SendEmail("BlueCat DNS Automation Error", strEMailIdsBCC, "", "", "BlueCat DNS Automation Error", "<p>There was an ERROR creating the BlueCat DNS Record for " + strName + " (SERVERID: " + intServer.ToString() + ") (ANSWERID: " + intAnswer.ToString() + ")</p><p>Error: " + strDNS + "</p><p>NOTE: Since this is an unexpected error, there was no task initiated to fix this problem. You must correct it yourself.</p>", true, false);
                                                                }
                                                            }
                                                        }
                                                        oLog.AddEvent(intAnswer, strName, strSerial, "BlueCat DNS Record Finished", LoggingType.Information);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                oLog.AddEvent(intAnswer, strName, strSerial, "There was no IP address assigned...skipping DNS Registration", LoggingType.Information);
                                            }

                                            if (strError == "" && intClusterID > 0)
                                            {
                                                // Register Cluster Instance Names and IPs
                                                DataSet dsInstances = oCluster.GetInstances(intClusterID);
                                                foreach (DataRow drInstance in dsInstances.Tables[0].Rows)
                                                {
                                                    if (strError != "")
                                                        break;
                                                    if (string.IsNullOrEmpty(drInstance["name"].ToString()) == false)
                                                    {
                                                        string strClusterInstance = drInstance["name"].ToString();
                                                        int intClusterInstanceIP = 0;
                                                        if (Int32.TryParse(drInstance["ipaddressid"].ToString(), out intClusterInstanceIP) == true && intClusterInstanceIP > 0)
                                                        {
                                                            string strClusterInstanceIP = oIPAddresses.GetName(intClusterInstanceIP, 0);
                                                            oLog.AddEvent(intAnswer, strName, strSerial, "Calling Web Service Function CreateBluecatDNS(" + strClusterInstanceIP + ", " + strClusterInstance + "-) on " + oVariable.WebServiceURL(), LoggingType.Information);
                                                            string strDNS = oWebService.CreateBluecatDNS(strClusterInstanceIP, strClusterInstance, strClusterInstance, "");
                                                            if (strDNS == "SUCCESS" || strDNS.StartsWith("***DUPLICATE") == true)
                                                            {
                                                                oLog.AddEvent(intAnswer, strName, strSerial, "BlueCat DNS Record for Cluster Instance = SUCCESS", LoggingType.Information);
                                                            }
                                                            else
                                                            {
                                                                if (strDNS.StartsWith("***CONFLICT") == true)
                                                                {
                                                                    oLog.AddEvent(intAnswer, strName, strSerial, "BlueCat DNS Record for Cluster Instance = CONFLICT", LoggingType.Warning);
                                                                    // A conflict occurred...awaiting the service technician to fix
                                                                    strError = "A conflict was encountered when trying to auto-register the BlueCat DNS object ~ " + strDNS;
                                                                    strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DEVELOPER_WARNING");
                                                                    if (intLogging > 0)
                                                                        oFunction.SendEmail("BlueCat DNS Automation Conflict", strEMailIdsBCC, "", "", "BlueCat DNS Automation Conflict", "<p>There was a CONFLICT creating the BlueCat DNS Record for Cluster Instance " + strClusterInstance + " (SERVERID: " + intServer.ToString() + ") (ANSWERID: " + intAnswer.ToString() + ")</p><p>Error: " + strDNS + "</p><p>NOTE: A request should have been generated and assigned to a DNS technician.</p>", true, false);
                                                                }
                                                                else
                                                                {
                                                                    oLog.AddEvent(intAnswer, strName, strSerial, "BlueCat DNS Record for Cluster Instance = ERROR: " + strDNS, LoggingType.Error);
                                                                    // An error was encountered...log the error
                                                                    if (boolForceDNSSuccess == false)
                                                                    {
                                                                        strError = "An error was encountered when trying to auto-register the BlueCat DNS object ~ " + strDNS;
                                                                        strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DEVELOPER_ERROR,EMAILGRP_PROVISIONING_SUPPORT");
                                                                        oFunction.SendEmail("BlueCat DNS Automation Error", strEMailIdsBCC, "", "", "BlueCat DNS Automation Error", "<p>There was an ERROR creating the BlueCat DNS Record for Cluster Instance " + strClusterInstance + " (SERVERID: " + intServer.ToString() + ") (ANSWERID: " + intAnswer.ToString() + ")</p><p>Error: " + strDNS + "</p><p>NOTE: Since this is an unexpected error, there was no task initiated to fix this problem. You must correct it yourself.</p>", true, false);
                                                                    }
                                                                }
                                                            }
                                                            oLog.AddEvent(intAnswer, strName, strSerial, "BlueCat DNS Record for cluster instance (" + strClusterInstance + ") finished", LoggingType.Information);
                                                        }
                                                        else
                                                            oLog.AddEvent(intAnswer, strName, strSerial, "BlueCat DNS Record for cluster instance (" + strClusterInstance + ") skipped since no IP was found", LoggingType.Information);
                                                    }
                                                }
                                                // Register Cluster Name and IP
                                                if (strError == "")
                                                {
                                                    string strCluster = oCluster.Get(intClusterID, "name");
                                                    int intClusterIP = 0;
                                                    if (Int32.TryParse(oCluster.Get(intClusterID, "ipaddressid"), out intClusterIP) == true && intClusterIP > 0)
                                                    {
                                                        string strClusterIP = oIPAddresses.GetName(intClusterIP, 0);
                                                        oLog.AddEvent(intAnswer, strName, strSerial, "Calling Web Service Function CreateBluecatDNS(" + strClusterIP + ", " + strCluster + "-) on " + oVariable.WebServiceURL(), LoggingType.Information);
                                                        string strDNS = oWebService.CreateBluecatDNS(strClusterIP, strCluster, strCluster, "");
                                                        if (strDNS == "SUCCESS" || strDNS.StartsWith("***DUPLICATE") == true)
                                                        {
                                                            oLog.AddEvent(intAnswer, strName, strSerial, "BlueCat DNS Record for Cluster = SUCCESS", LoggingType.Information);
                                                        }
                                                        else
                                                        {
                                                            if (strDNS.StartsWith("***CONFLICT") == true)
                                                            {
                                                                oLog.AddEvent(intAnswer, strName, strSerial, "BlueCat DNS Record for Cluster = CONFLICT", LoggingType.Warning);
                                                                // A conflict occurred...awaiting the service technician to fix
                                                                strError = "A conflict was encountered when trying to auto-register the BlueCat DNS object ~ " + strDNS;
                                                                strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DEVELOPER_WARNING");
                                                                if (intLogging > 0)
                                                                    oFunction.SendEmail("BlueCat DNS Automation Conflict", strEMailIdsBCC, "", "", "BlueCat DNS Automation Conflict", "<p>There was a CONFLICT creating the BlueCat DNS Record for Cluster " + strCluster + " (SERVERID: " + intServer.ToString() + ") (ANSWERID: " + intAnswer.ToString() + ")</p><p>Error: " + strDNS + "</p><p>NOTE: A request should have been generated and assigned to a DNS technician.</p>", true, false);
                                                            }
                                                            else
                                                            {
                                                                oLog.AddEvent(intAnswer, strName, strSerial, "BlueCat DNS Record for Cluster Instance = ERROR: " + strDNS, LoggingType.Error);
                                                                // An error was encountered...log the error
                                                                if (boolForceDNSSuccess == false)
                                                                {
                                                                    strError = "An error was encountered when trying to auto-register the BlueCat DNS object ~ " + strDNS;
                                                                    strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DEVELOPER_ERROR,EMAILGRP_PROVISIONING_SUPPORT");
                                                                    oFunction.SendEmail("BlueCat DNS Automation Error", strEMailIdsBCC, "", "", "BlueCat DNS Automation Error", "<p>There was an ERROR creating the BlueCat DNS Record for Cluster " + strCluster + " (SERVERID: " + intServer.ToString() + ") (ANSWERID: " + intAnswer.ToString() + ")</p><p>Error: " + strDNS + "</p><p>NOTE: Since this is an unexpected error, there was no task initiated to fix this problem. You must correct it yourself.</p>", true, false);
                                                                }
                                                            }
                                                        }
                                                        oLog.AddEvent(intAnswer, strName, strSerial, "BlueCat DNS Record for cluster (" + strCluster + ") finished", LoggingType.Information);
                                                    }
                                                    else
                                                        oLog.AddEvent(intAnswer, strName, strSerial, "BlueCat DNS Record for cluster (" + strCluster + ") skipped since no IP was found", LoggingType.Information);
                                                }
                                            }
                                            oLog.AddEvent(intAnswer, strName, strSerial, "Finished Creating DNS Records", LoggingType.Information);


                                            if (strError == "")
                                            {
                                                if (intIPAddressBackup > 0)
                                                {
                                                    oLog.AddEvent(intAnswer, strName, strSerial, "Creating DNS Record for BACKUP", LoggingType.Information);
                                                    strIP = oIPAddresses.GetName(intIPAddressBackup, 0);
                                                    if (boolDNS_QIP == true)
                                                    {
                                                        oLog.AddEvent(intAnswer, strName, strSerial, "Calling Web Service Function CreateDNSforPNC(" + strIP + ", " + strName + "-backup, Server, , " + oVariable.DNS_Domain() + ", " + oVariable.DNS_NameService() + ", " + oVariable.DNS_DynamicDNSUpdate() + ", " + intUser.ToString() + ", true) on " + oVariable.WebServiceURL(), LoggingType.Information);
                                                        string strDNS = oWebService.CreateDNSforPNC(strIP, strName + "-backup", "Server", "", oVariable.DNS_Domain(), oVariable.DNS_NameService(), oVariable.DNS_DynamicDNSUpdate(), intUser, 0, true);
                                                        if (strDNS == "SUCCESS" || strDNS.StartsWith("***DUPLICATE") == true)
                                                        {
                                                            oLog.AddEvent(intAnswer, strName, strSerial, "QIP DNS Record for BACKUP = SUCCESS", LoggingType.Information);
                                                        }
                                                        else
                                                        {
                                                            if (strDNS.StartsWith("***CONFLICT") == true)
                                                            {
                                                                oLog.AddEvent(intAnswer, strName, strSerial, "QIP DNS Record for BACKUP = CONFLICT", LoggingType.Warning);
                                                                // A conflict occurred...awaiting the service technician to fix
                                                                strError = "A conflict was encountered when trying to auto-register the QIP DNS object";
                                                                strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DEVELOPER_WARNING");
                                                                if (intLogging > 0)
                                                                    oFunction.SendEmail("QIP DNS Automation Conflict", strEMailIdsBCC, "", "", "QIP DNS Automation Conflict", "<p>There was a CONFLICT creating the QIP DNS Record for BACKUP for " + strName + " (SERVERID: " + intServer.ToString() + ") (ANSWERID: " + intAnswer.ToString() + ")</p><p>Error: " + strDNS + "</p><p>NOTE: A request should have been generated and assigned to a DNS technician.</p>", true, false);
                                                            }
                                                            else if (strDNS.ToUpper().StartsWith("***ERROR: SUBNET FOR") == true)
                                                            {
                                                                oLog.AddEvent(intAnswer, strName, strSerial, "QIP DNS Record for BACKUP = SUBNET ERROR: " + strDNS, LoggingType.Error);
                                                                // This occurs if a subnet does not exist.  Will need to be routed for assignment and continue.
                                                                if (boolForceDNSSuccess == false)
                                                                {
                                                                    strError = "A subnet error was encountered when trying to auto-register the QIP DNS object ~ " + strDNS;
                                                                    strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DEVELOPER_WARNING");
                                                                    oFunction.SendEmail("QIP DNS Automation Subnet Error", strEMailIdsBCC, "", "", "QIP DNS Automation Subnet Error", "<p>There was a SUBNET ERROR creating the QIP DNS Record for BACKUP for " + strName + " (SERVERID: " + intServer.ToString() + ") (ANSWERID: " + intAnswer.ToString() + ")</p><p>Error: " + strDNS + "</p><p>NOTE: A request should have been generated and assigned to a DNS technician.</p>", true, false);
                                                                }
                                                            }
                                                            else
                                                            {
                                                                oLog.AddEvent(intAnswer, strName, strSerial, "QIP DNS Record for BACKUP = ERROR: " + strDNS, LoggingType.Error);
                                                                // An error was encountered...log the error
                                                                if (boolForceDNSSuccess == false)
                                                                {
                                                                    strError = "An error was encountered when trying to auto-register the QIP DNS object ~ " + strDNS;
                                                                    strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DEVELOPER_ERROR,EMAILGRP_PROVISIONING_SUPPORT");
                                                                    oFunction.SendEmail("QIP DNS Automation Error", strEMailIdsBCC, "", "", "QIP DNS Automation Error", "<p>There was an ERROR creating the QIP DNS Record for BACKUP for " + strName + " (SERVERID: " + intServer.ToString() + ") (ANSWERID: " + intAnswer.ToString() + ")</p><p>Error: " + strDNS + "</p><p>NOTE: Since this is an unexpected error, there was no task initiated to fix this problem. You must correct it yourself.</p>", true, false);
                                                                }
                                                            }
                                                        }
                                                        oLog.AddEvent(intAnswer, strName, strSerial, "QIP DNS Record for BACKUP Finished", LoggingType.Information);
                                                    }
                                                    if (strError == "")
                                                    {
                                                        if (boolDNS_Bluecat == true)
                                                        {
                                                            oLog.AddEvent(intAnswer, strName, strSerial, "Calling Web Service Function CreateBluecatDNS(" + strIP + ", " + strName + "-backup, ) on " + oVariable.WebServiceURL(), LoggingType.Information);
                                                            string strDNS = oWebService.CreateBluecatDNS(strIP, strName + "-backup", strName + "-backup", "");
                                                            if (strDNS == "SUCCESS" || strDNS.StartsWith("***DUPLICATE") == true)
                                                            {
                                                                oLog.AddEvent(intAnswer, strName, strSerial, "BlueCat DNS Record for BACKUP = SUCCESS", LoggingType.Information);
                                                            }
                                                            else
                                                            {
                                                                if (strDNS.StartsWith("***CONFLICT") == true)
                                                                {
                                                                    oLog.AddEvent(intAnswer, strName, strSerial, "BlueCat DNS Record for BACKUP = CONFLICT", LoggingType.Warning);
                                                                    // A conflict occurred...awaiting the service technician to fix
                                                                    strError = "A conflict was encountered when trying to auto-register the BlueCat DNS object ~ " + strDNS;
                                                                    strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DEVELOPER_WARNING");
                                                                    if (intLogging > 0)
                                                                        oFunction.SendEmail("BlueCat DNS Automation Conflict", strEMailIdsBCC, "", "", "BlueCat DNS Automation Conflict", "<p>There was a CONFLICT creating the BlueCat DNS Record for BACKUP for " + strName + " (SERVERID: " + intServer.ToString() + ") (ANSWERID: " + intAnswer.ToString() + ")</p><p>Error: " + strDNS + "</p><p>NOTE: A request should have been generated and assigned to a DNS technician.</p>", true, false);
                                                                }
                                                                else
                                                                {
                                                                    oLog.AddEvent(intAnswer, strName, strSerial, "BlueCat DNS Record for BACKUP = ERROR: " + strDNS, LoggingType.Error);
                                                                    // An error was encountered...log the error
                                                                    if (boolForceDNSSuccess == false)
                                                                    {
                                                                        strError = "An error was encountered when trying to auto-register the BlueCat DNS object ~ " + strDNS;
                                                                        strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DEVELOPER_ERROR,EMAILGRP_PROVISIONING_SUPPORT");
                                                                        oFunction.SendEmail("BlueCat DNS Automation Error", strEMailIdsBCC, "", "", "BlueCat DNS Automation Error", "<p>There was an ERROR creating the BlueCat DNS Record for BACKUP for " + strName + " (SERVERID: " + intServer.ToString() + ") (ANSWERID: " + intAnswer.ToString() + ")</p><p>Error: " + strDNS + "</p><p>NOTE: Since this is an unexpected error, there was no task initiated to fix this problem. You must correct it yourself.</p>", true, false);
                                                                    }
                                                                }
                                                            }
                                                            oLog.AddEvent(intAnswer, strName, strSerial, "BlueCat DNS Record for BACKUP Finished", LoggingType.Information);
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    oLog.AddEvent(intAnswer, strName, strSerial, "There was no BACKUP IP address assigned...skipping DNS Registration", LoggingType.Information);
                                                }
                                                oLog.AddEvent(intAnswer, strName, strSerial, "Finished Creating DNS Record for BACKUP", LoggingType.Information);
                                            }

                                            // Create DNS Entry for ILO, ILOM, ALOM, DRAC, etc...
                                            if (boolDNS_QIP == true)
                                            {
                                                if (strILO == "")
                                                {
                                                    oLog.AddEvent(intAnswer, strName, strSerial, "The ILO address is empty...skipping DNS registration", LoggingType.Information);
                                                }
                                                else
                                                {
                                                    oLog.AddEvent(intAnswer, strName, strSerial, "Calling Web Service Function CreateDNSforPNC(" + strILO + ", " + strName + "-RM, Server, , " + oVariable.DNS_Domain() + ", " + oVariable.DNS_NameService() + ", " + oVariable.DNS_DynamicDNSUpdate() + ", " + intUser.ToString() + ", true) on " + oVariable.WebServiceURL(), LoggingType.Information);
                                                    string strDNS = oWebService.CreateDNSforPNC(strILO, strName + "-RM", "Server", "", oVariable.DNS_Domain(), oVariable.DNS_NameService(), oVariable.DNS_DynamicDNSUpdate(), intUser, 0, true);
                                                    if (strDNS == "SUCCESS" || strDNS.StartsWith("***DUPLICATE") == true)
                                                    {
                                                        oLog.AddEvent(intAnswer, strName, strSerial, "QIP DNS ILO Record = SUCCESS", LoggingType.Information);
                                                    }
                                                    else
                                                    {
                                                        if (strDNS.StartsWith("***CONFLICT") == true)
                                                        {
                                                            oLog.AddEvent(intAnswer, strName, strSerial, "QIP DNS ILO Record = CONFLICT", LoggingType.Warning);
                                                        }
                                                        else if (strDNS.ToUpper().Contains("SUBNET FOR") == true)
                                                        {
                                                            oLog.AddEvent(intAnswer, strName, strSerial, "QIP DNS ILO Record = SUBNET ERROR: " + strDNS, LoggingType.Error);
                                                        }
                                                        else
                                                        {
                                                            oLog.AddEvent(intAnswer, strName, strSerial, "QIP DNS ILO Record = ERROR: " + strDNS, LoggingType.Error);
                                                        }
                                                    }
                                                    oLog.AddEvent(intAnswer, strName, strSerial, "QIP DNS ILO Record Finished", LoggingType.Information);
                                                }
                                            }
                                            if (boolDNS_Bluecat == true)
                                            {
                                                if (strILO == "")
                                                {
                                                    oLog.AddEvent(intAnswer, strName, strSerial, "The ILO address is empty...skipping DNS registration", LoggingType.Information);
                                                }
                                                else
                                                {
                                                    oLog.AddEvent(intAnswer, strName, strSerial, "Calling Web Service Function CreateBluecatDNS(" + strILO + ", " + strName + "-RM, ) on " + oVariable.WebServiceURL(), LoggingType.Information);
                                                    string strDNS = oWebService.CreateBluecatDNS(strILO, strName + "-RM", strName + "-RM", "");
                                                    if (strDNS == "SUCCESS" || strDNS.StartsWith("***DUPLICATE") == true)
                                                    {
                                                        oLog.AddEvent(intAnswer, strName, strSerial, "BlueCat DNS ILO Record = SUCCESS", LoggingType.Information);
                                                    }
                                                    else
                                                    {
                                                        if (strDNS.StartsWith("***CONFLICT") == true)
                                                        {
                                                            oLog.AddEvent(intAnswer, strName, strSerial, "BlueCat DNS ILO Record = CONFLICT", LoggingType.Warning);
                                                        }
                                                        else
                                                        {
                                                            oLog.AddEvent(intAnswer, strName, strSerial, "BlueCat DNS ILO Record = ERROR: " + strDNS, LoggingType.Error);
                                                        }
                                                    }
                                                    oLog.AddEvent(intAnswer, strName, strSerial, "BlueCat DNS ILO Record Finished", LoggingType.Information);
                                                }
                                            }
                                        }
                                        else
                                            oLog.AddEvent(intAnswer, strName, strSerial, "DNS registration skipped (NCC build)", LoggingType.Information);
                                    }
                                    catch (Exception exT)
                                    {
                                        strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DEVELOPER_ERROR,EMAILGRP_PROVISIONING_SUPPORT");
                                        strError = "A FATAL ERROR was encountered when trying to auto-register the DNS object";
                                        oFunction.SendEmail("ERROR: Creating DNS Records", strEMailIdsBCC, "", "", "ERROR: Creating DNS Records", "<p>There was a problem creating the DNS Record for " + strName + " (ANSWERID: " + intAnswer.ToString() + ")</p><p>Error Message: " + exT.Message + "</p><p>Source: " + exT.Source + "</p><p>Stack Trace: " + exT.StackTrace + "</p>", true, false);
                                        oLog.AddEvent(intAnswer, strName, strSerial, "There was a problem creating the DNS Record (Error Message: " + exT.Message + ") (Source: " + exT.Source + ") (Stack Trace: " + exT.StackTrace + ")", LoggingType.Error);
                                    }
                                    if (strError == "")
                                    {
                                        foreach (string strDelete in Directory.GetFiles(strScripts + strSub, intAnswer.ToString() + "_*.vbs"))
                                            File.Delete(strDelete);
                                        foreach (string strDelete in Directory.GetFiles(strScripts + strSub, intAnswer.ToString() + "_*.bat"))
                                            File.Delete(strDelete);
                                        string strEmail = oUser.GetName(intUser) + ";";
                                        int intAppContact = Int32.Parse(oForecast.GetAnswer(intAnswer, "appcontact"));
                                        if (intAppContact > 0)
                                            strEmail += oUser.GetName(intAppContact) + ";";
                                        int intAdmin1 = Int32.Parse(oForecast.GetAnswer(intAnswer, "admin1"));
                                        if (intAdmin1 > 0)
                                            strEmail += oUser.GetName(intAdmin1) + ";";
                                        int intAdmin2 = Int32.Parse(oForecast.GetAnswer(intAnswer, "admin2"));
                                        if (intAdmin2 > 0)
                                            strEmail += oUser.GetName(intAdmin2) + ";";
                                        // ** 7/28/08 - Delete the following code when not notifying the II resource anymore
                                        strEmail = strImplementor;
                                        // ** 7/28/08 - Stop deleting here

                                        //oLog.AddEvent(intAnswer, strName, strSerial, "Attempting to send email", LoggingType.Information);
                                        //strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DEVELOPER_ERROR,EMAILGRP_DESIGN_BUILDER");
                                        //if (strEmail != "")
                                        //    oFunction.SendEmail("Auto-Provisioning Notification: " + strName, strEmail, "", strEMailIdsBCC, "Auto-Provisioning Notification: " + strName, "<p><b>This message is to inform you that the server " + strName + " has been auto-provisioned successfully!</b><p><p>Serial Number: " + oAsset.GetServerOrBlade(intAsset, "serial").ToUpper() + "<br/>Asset Tag: " + oAsset.GetServerOrBlade(intAsset, "asset").ToUpper() + "<br/>Model: " + oModelsProperties.Get(intModel, "name").ToUpper() + "</p><p><b>NOTE:</b> This is automated email sent from the ClearView Auto-Provisioning tool. Please do not respond to this message.</p><p>If you would like to learn more about this process, <a href=\"" + oVariable.URL() + "/info.htm\" target=\"_blank\"/>please click here</a>.</p>", true, false);

                                        // Remove files on target server
                                        string strRemoveIP = strIP;
                                        if (intIPAddressBuild1 > 0)
                                            strRemoveIP = oIPAddresses.GetName(intIPAddressBuild1, 0);
                                        oLog.AddEvent(intAnswer, strName, strSerial, "Removing configuration files on target server (\\\\" + strRemoveIP + "\\C$\\OPTIONS\\CV_*.*)", LoggingType.Information);
                                        string strBatchDeleteTarget = strScripts + strSub + intServer.ToString() + "_" + strNow + "_cleanup.bat";
                                        StreamWriter oWriterDeleteTarget = new StreamWriter(strBatchDeleteTarget);
                                        oWriterDeleteTarget.WriteLine("net use \\\\" + strRemoveIP + "\\C$ /user:" + strAdminUser + " " + strAdminPass + "");
                                        oWriterDeleteTarget.WriteLine("del \\\\" + strRemoveIP + "\\C$\\OPTIONS\\CV_*.*");
                                        oWriterDeleteTarget.WriteLine("net use \\\\" + strRemoveIP + "\\C$ /dele");
                                        oWriterDeleteTarget.Flush();
                                        oWriterDeleteTarget.Close();
                                        ProcessStartInfo infoDeleteTarget = new ProcessStartInfo(strScripts + "psexec");
                                        infoDeleteTarget.WorkingDirectory = strScripts;
                                        if (oOperatingSystem.IsWindows2008(intOS) == true)
                                        {
                                            // Have to remove the "-i" parameter or else the script won't run (will come up with "the following helper dll cannot be loaded...XXXX.DLL" error)
                                            infoDeleteTarget.Arguments = "cmd.exe /c " + strBatchDeleteTarget;
                                        }
                                        else
                                            infoDeleteTarget.Arguments = "-i cmd.exe /c " + strBatchDeleteTarget;
                                        bool boolTimeoutTarget = false;
                                        Process procDeleteTarget = Process.Start(infoDeleteTarget);
                                        procDeleteTarget.WaitForExit(intTimeoutDefault);
                                        if (procDeleteTarget.HasExited == false)
                                        {
                                            procDeleteTarget.Kill();
                                            boolTimeoutTarget = true;
                                        }
                                        procDeleteTarget.Close();
                                        oLog.AddEvent(intAnswer, strName, strSerial, "FINISHED = Removing configuration files on target server (\\\\" + strRemoveIP + "\\C$\\OPTIONS\\CV_*.*)", LoggingType.Information);

                                        /*
                                        // Service Center Request Form
                                        oLog.AddEvent(intAnswer, strName, strSerial, "Sending Service Center Request Form", LoggingType.Information);
                                        oForms.ServiceCenter(intServer);
                                        oLog.AddEvent(intAnswer, strName, strSerial, "FINISHED = Sending Service Center Request Form", LoggingType.Information);
                                        */

                                        AddResult(intServer, intStep, intType, oOnDemand.GetStep(intStepID, "done"), strError);
                                        oServer.UpdateStep(intServer, 999);
                                        if (boolRebuilding == true)
                                        {
                                            if (String.IsNullOrEmpty(oServer.Get(intServer, "build_completed")) == true)
                                                oServer.UpdateBuildCompleted(intServer, DateTime.Now.ToString());
                                            oServer.UpdateRebuild(intServer, DateTime.Now.ToString());
                                            strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DEVELOPER_ALERT");
                                            if (strEmail != "")
                                                oFunction.SendEmail("Auto-Provisioning Rebuild Notification: " + strName, strEMailIdsBCC, "", "", "Auto-Provisioning Rebuild Notification: " + strName, "<p><b>This message is to inform you that the server " + strName + " has been rebuilt successfully!</b><p><p>Serial Number: " + oAsset.GetServerOrBlade(intAsset, "serial").ToUpper() + "<br/>Asset Tag: " + oAsset.GetServerOrBlade(intAsset, "asset").ToUpper() + "<br/>Model: " + oModelsProperties.Get(intModel, "name").ToUpper() + "</p><p><b>NOTE:</b> This is automated email sent from the ClearView Auto-Provisioning tool. Please do not respond to this message.</p><p>If you would like to learn more about this process, <a href=\"" + oVariable.URL() + "/info.htm\" target=\"_blank\"/>please click here</a>.</p>", true, false);
                                        }
                                        else
                                            oServer.UpdateBuildCompleted(intServer, DateTime.Now.ToString());
                                        oServer.UpdateRebuilding(intServer, 0);
                                        // Clear the TSM registration output so it will be re-registered on the server
                                        if (intTSM == 1 && intTSMAutomated == 1)
                                        {
                                            oLog.AddEvent(intAnswer, strName, strSerial, "Setting TSM output PENDING status and Date TSM", LoggingType.Debug);
                                            // Update the TSM table to enable automated TSM
                                            oServer.UpdateTSM(intServer, "PENDING");
                                            oServer.UpdateTSMRegistered(intServer, DateTime.Now.ToString());
                                        }
                                        else if (intAvamar == 1 && intAvamarAutomated == 1)
                                        {
                                            oLog.AddEvent(intAnswer, strName, strSerial, "Adding to Avamar automation queue", LoggingType.Debug);
                                            // Update the Avamar table to enable automated Avamar processing
                                            oServer.AddAvamar(intServer);
                                        }
                                        else
                                            oServer.UpdateTSM(intServer, "");   // Kicks out a manual task
                                        oForecast.UpdateAnswerCompleted(intAnswer);
                                        if (oForecast.GetAnswer(intAnswer, "completed") != "")
                                        {
                                            // Remove installation files
                                            oLog.AddEvent(intAnswer, strName, strSerial, "Removing installation files (" + strScripts + strSub + intAnswer.ToString() + "_*.*)", LoggingType.Information);
                                            string[] strFilesToDeleteAnswer = Directory.GetFiles(strScripts + strSub, intAnswer.ToString() + "_*.*");
                                            foreach (string strFileToDelete in strFilesToDeleteAnswer)
                                            {
                                                oLog.AddEvent(intAnswer, strName, strSerial, "Deleting installation file (" + strFileToDelete + ")", LoggingType.Debug);
                                                File.Delete(strFileToDelete);
                                            }
                                            oLog.AddEvent(intAnswer, strName, strSerial, "Removing installation files (" + strScripts + strSub + intServer.ToString() + "_*.*)", LoggingType.Information);
                                            string[] strFilesToDeleteServer = Directory.GetFiles(strScripts + strSub, intServer.ToString() + "_*.*");
                                            foreach (string strFileToDelete in strFilesToDeleteServer)
                                            {
                                                oLog.AddEvent(intAnswer, strName, strSerial, "Deleting installation file (" + strFileToDelete + ")", LoggingType.Debug);
                                                File.Delete(strFileToDelete);
                                            }

                                            // Initiate Pre-Production Online Tasks
                                            oServiceRequest.Add(intRequest, 1, 1);
                                            PNCTasks oPNCTask = new PNCTasks(0, dsn);
                                            oPNCTask.InitiateNextStep(intRequest, intAnswer, intModel, intEnvironment, intAssignPage, intViewPage, dsnAsset, dsnIP, dsnServiceEditor);
                                        }
                                        else
                                            oLog.AddEvent(intAnswer, strName, strSerial, "ANSWERID: " + intAnswer.ToString() + " not done - no forms to send yet for " + strName, LoggingType.Information);
                                    }
                                    if (strError != "")
                                        AddResult(intServer, intStep, intType, oOnDemand.GetStep(intStepID, "done"), strError);
                                    break;
                            }
                            // Send Error if encountered
                            DataSet dsError = oServer.GetError(intServer, intStep);
                            if (dsError.Tables[0].Rows.Count == 0 && (oSpan.Hours > 24 || strError != ""))
                            {
                                // Generic Error Request
                                int intProvisioningErrorItem = oService.GetItemId(intProvisioningErrorService);
                                int intProvisioningErrorNumber = oResourceRequest.GetNumber(intRequest, intProvisioningErrorItem);

                                strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_PROVISIONING_SUPPORT");
                                if (oSpan.Hours > 24)
                                {
                                    strError = "Sitting at step " + strStep + " for more than 24 hours";
                                    int intError = oServer.AddError(intRequest, intProvisioningErrorItem, intProvisioningErrorNumber, intServer, intStep, strError);
                                    if (boolProvisioningErrorEmail == true)
                                        oFunction.SendEmail("Auto-Provisioning INACTIVITY: " + strName, strEMailIdsBCC, strImplementor, "", "Auto-Provisioning INACTIVITY: " + strName, "<p><b>This message is to inform you that the server " + strName + " has been sitting at a step for more than 24 hours!</b><p><p>Serial Number: " + oAsset.Get(intAsset, "serial").ToUpper() + "<br/>Asset Tag: " + oAsset.Get(intAsset, "asset").ToUpper() + "<br/>Model: " + oModelsProperties.Get(intModel, "name").ToUpper() + "<br/>Step #: " + intStep.ToString() + "<br/>Step: " + strStep + "<br/>Error: " + strError + "<br/>ILO: <a href=\"https://" + strILO + "\" target=\"_blank\">" + strILO + "</a></p><p>Once you have resolved this issue, please <a href=\"" + oVariable.URL() + "/build_fixed.aspx?type=s&id=" + oFunction.encryptQueryString(intServer.ToString()) + "\" target=\"_blank\">click here</a> to resume the build.</p><p><b>NOTE:</b> This is automated email sent from the ClearView Auto-Provisioning tool. Please do not respond to this message.</p>", true, false);
                                }
                                else
                                {
                                    int intError = oServer.AddError(intRequest, intProvisioningErrorItem, intProvisioningErrorNumber, intServer, intStep, strError);
                                    if (boolProvisioningErrorEmail == true)
                                        oFunction.SendEmail("Auto-Provisioning ERROR: " + strName, strEMailIdsBCC, strImplementor, "", "Auto-Provisioning ERROR: " + strName, "<p><b>This message is to inform you that the server " + strName + " has encountered an error and has been stopped!</b><p><p>Serial Number: " + oAsset.Get(intAsset, "serial").ToUpper() + "<br/>Asset Tag: " + oAsset.Get(intAsset, "asset").ToUpper() + "<br/>Model: " + oModelsProperties.Get(intModel, "name").ToUpper() + "<br/>Step #: " + intStep.ToString() + "<br/>Step: " + strStep + "<br/>Error: " + strError + "<br/>ILO: <a href=\"https://" + strILO + "\" target=\"_blank\">" + strILO + "</a></p><p>Once you have resolved this issue, please <a href=\"" + oVariable.URL() + "/build_fixed.aspx?type=s&id=" + oFunction.encryptQueryString(intServer.ToString()) + "\" target=\"_blank\">click here</a> to resume the build.</p><p><b>NOTE:</b> This is automated email sent from the ClearView Auto-Provisioning tool. Please do not respond to this message.</p>", true, false);
                                }
                                if (boolProvisioningErrorEmail == true)
                                    oLog.AddEvent(intAnswer, strName, strSerial, "Error Message (" + strError + ") Sent to " + strEMailIdsBCC, LoggingType.Warning);

                                if (boolAuditError == false)
                                {
                                    int intProvisioningError = oResourceRequest.Add(intRequest, intProvisioningErrorItem, intProvisioningErrorService, intProvisioningErrorNumber, "Provisioning Error (" + strName + ")", 1, 0.00, 2, 1, 1, 1);
                                    if (oServiceRequest.NotifyApproval(intProvisioningError, intResourceRequestApprove, intEnvironment, "", dsnServiceEditor) == false)
                                        oServiceRequest.NotifyTeamLead(intProvisioningErrorItem, intProvisioningError, intAssignPage, intViewPage, intEnvironment, "", dsnServiceEditor, dsnAsset, dsnIP, 0);
                                    oLog.AddEvent(intAnswer, strName, strSerial, "Provisioning Error Request has been submitted (" + strError + ")", LoggingType.Warning);
                                }
                            }
                        }
                    }
                }
                if (ds.Tables[0].Rows.Count == 0 && intLogging > 1)
                    oEventLog.WriteEntry("No Physical servers to build", EventLogEntryType.Information);
            }
            catch (Exception ex)
            {
                if (ex.Message.ToUpper().StartsWith("A TRANSPORT-LEVEL ERROR") == false)
                {
                    string strError = "Physical Service: " + "(Error Message: " + ex.Message + ") ~ (Source: " + ex.Source + ") (Stack Trace: " + ex.StackTrace + ") [" + System.Environment.UserName + "]";
                    oEventLog.WriteEntry(strError, EventLogEntryType.Error);
                    SystemError(intErrorServer, intErrorStep, strError, intErrorAsset, intErrorModel);
                }
            }
        }
        private void ServiceTickDecom()
        {
            int intServer = 0;
            string strName = "";
            try
            {
                Servers oServer = new Servers(0, dsn);
                Requests oRequest = new Requests(0, dsn);
                Functions oFunction = new Functions(0, dsn, intEnvironment);
                Variables oVariable = new Variables(intEnvironment);
                IPAddresses oIPAddress = new IPAddresses(0, dsnIP, dsn);
                Classes oClass = new Classes(0, dsn);
                Domains oDomain = new Domains(0, dsn);
                Asset oAsset = new Asset(0, dsnAsset, dsn);
                Users oUser = new Users(0, dsn);
                Projects oProject = new Projects(0, dsn);
                ServiceRequests oServiceRequest = new ServiceRequests(0, dsn);
                Services oService = new Services(0, dsn);
                ResourceRequest oResourceRequest = new ResourceRequest(0, dsn);
                ServiceDetails oServiceDetail = new ServiceDetails(0, dsn);
                ModelsProperties oModelsProperties = new ModelsProperties(0, dsn);
                OperatingSystems oOperatingSystem = new OperatingSystems(0, dsn);
                if (intLogging > 1)
                    oEventLog.WriteEntry("DECOMMISSION (PHYSICAL): Querying.... SERVERTYPES:" + strServerTypesDecom.ToString(), EventLogEntryType.Information);
                DateTime _now = DateTime.Now;
                string strNow = _now.Day.ToString() + _now.Month.ToString() + _now.Year.ToString() + _now.Hour.ToString() + _now.Minute.ToString() + _now.Second.ToString() + _now.Millisecond.ToString();
                DataSet ds = oAsset.GetDecommissions(strServerTypesDecom, DateTime.Now);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    string strError = "";
                    bool boolDecom = false;
                    int intRequest = Int32.Parse(dr["requestid"].ToString());
                    bool boolDemo = false;
                    int intProject = oRequest.GetProjectNumber(intRequest);
                    string strNumber = oProject.Get(intProject, "number");
                    // Check to see if Demo
                    DataSet dsDemo = oFunction.GetSetupValuesByKey("DEMO_PROJECT");
                    foreach (DataRow drDemo in dsDemo.Tables[0].Rows)
                    {
                        if (strNumber == drDemo["Value"].ToString())
                        {
                            boolDemo = true;
                            break;
                        }
                    }
                    int intItem = Int32.Parse(dr["itemid"].ToString());
                    int intNumber = Int32.Parse(dr["number"].ToString());
                    int intAsset = Int32.Parse(dr["assetid"].ToString());
                    int intModel = 0;
                    bool boolBlade = false;
                    bool boolDell = false;
                    if (intAsset > 0)
                    {
                        intModel = Int32.Parse(oAsset.Get(intAsset, "modelid"));
                        boolBlade = oModelsProperties.IsTypeBlade(intModel);
                        boolDell = oModelsProperties.IsDell(intModel);
                    }
                    int intUser = Int32.Parse(dr["userid"].ToString());
                    strName = dr["name"].ToString();
                    string strSerial = oAsset.Get(intAsset, "serial");
                    string strResult = "No information...";
                    DataSet dsAsset = oServer.GetAsset(intAsset);
                    if (dsAsset.Tables[0].Rows.Count > 0)
                        intServer = Int32.Parse(dsAsset.Tables[0].Rows[0]["id"].ToString());

                    if (dr["dr"].ToString() == "1")
                    {
                        // Just set DR servers to completed
                        oAsset.UpdateDecommission(intAsset, DateTime.Now.AddDays(14), 1, strName);
                        oAsset.AddStatus(intAsset, strName, (int)AssetStatus.Decommissioned, 0, DateTime.Now);
                        DataSet dsAssetDR = oServer.GetAssetsAsset(intAsset);
                        foreach (DataRow drAsset in dsAssetDR.Tables[0].Rows)
                        {
                            if (drAsset["dr"].ToString() == "1")
                            {
                                int intServerDR = Int32.Parse(drAsset["serverid"].ToString());
                                oServer.UpdateAssetDecom(intServerDR, intAsset, DateTime.Now.ToString());
                                oLog.AddEvent(strName, strSerial, "DECOMMISSION: Finished Decommissioning DR asset", LoggingType.Information);
                            }
                        }
                    }
                    else
                    {
                        if (oResourceRequest.GetAllService(intRequest, intDecomErrorService, intNumber).Tables[0].Rows.Count == 0)
                        {
                            // Start Decommission
                            oAsset.UpdateDecommissionRunning(intAsset, 1);
                            oLog.AddEvent(strName, strSerial, "DECOMMISSION: Starting Decommission (non-DR asset)", LoggingType.Information);
                            if (dsAsset.Tables[0].Rows.Count > 0)
                            {
                                oServer.UpdateStep(intServer, 999);

                                bool boolOSok = false;
                                int intOS = 0;
                                if (Int32.TryParse(oServer.Get(intServer, "osid"), out intOS) == true)
                                    boolOSok = (oOperatingSystem.IsWindows2008(intOS) || oOperatingSystem.IsWindows(intOS) || oOperatingSystem.IsLinux(intOS));

                                if (boolOSok == false)
                                    strError = "The operating system associated with this server is either unknown or not available for automated decommission";
                                else
                                {
                                    // Ping name to see if it is still on
                                    if (boolDemo == false && oFunction.PingName(strName) == "")
                                    {
                                        // Server is not responding, assume it is off
                                        oLog.AddEvent(strName, strSerial, "DECOMMISSION: Not responding to ping (" + strName + ")...skipping power down commands.", LoggingType.Information);
                                        boolDecom = true;
                                    }
                                    else
                                    {
                                        int intClass = -1;
                                        Int32.TryParse(dsAsset.Tables[0].Rows[0]["classid"].ToString(), out intClass);
                                        //if (intClass == -1 || (oClass.Get(intClass, "pnc") != "1" && oClass.IsProd(intClass)))
                                        // 7-26-10: Enable NCB Production Decommission
                                        if (intClass == -1)
                                        {
                                            if (intClass == -1)
                                                oLog.AddEvent(strName, strSerial, "DECOMMISSION: Class does not exist - skipping automated decommission (should not happen often)", LoggingType.Information);
                                            else
                                                oLog.AddEvent(strName, strSerial, "DECOMMISSION: Class is an NCB Production class and should be powered off by someone else (such as Network Control)", LoggingType.Information);
                                            boolDecom = true;
                                        }
                                        else
                                        {
                                            bool boolIsShutdown = false;

                                            string strIP = "";
                                            DataSet dsIP = oServer.GetIP(intServer, 0, 1, 0, 0);
                                            foreach (DataRow drIP in dsIP.Tables[0].Rows)
                                            {
                                                if (strIP != "")
                                                    break;
                                                string strDecomIP = oIPAddress.GetName(Int32.Parse(drIP["ipaddressid"].ToString()), 0);
                                                for (int ii = 0; ii < 40 && strIP == ""; ii++)
                                                {
                                                    Thread.Sleep(3000);
                                                    Ping oPing = new Ping();
                                                    string strStatus = "";
                                                    try
                                                    {
                                                        PingReply oReply = oPing.Send(strDecomIP);
                                                        strStatus = oReply.Status.ToString().ToUpper();
                                                    }
                                                    catch { }
                                                    if (strStatus == "SUCCESS")
                                                    {
                                                        strIP = strDecomIP;
                                                        oLog.AddEvent(strName, strSerial, "DECOMMISSION: The IP address (" + strIP + ") successfully pinged", LoggingType.Information);
                                                    }
                                                }
                                            }
                                            if (strIP != "")
                                            {
                                                oLog.AddEvent(strName, strSerial, "DECOMMISSION: Attempting to shutdown using the script and IP address (" + strIP + ")", LoggingType.Information);
                                                int intDomain = 0;
                                                Int32.TryParse(oServer.Get(intServer, "domainid"), out intDomain);
                                                if (intDomain > 0)
                                                {
                                                    int intDomainEnvironment = 0;
                                                    Int32.TryParse(oDomain.Get(intDomain, "environment"), out intDomainEnvironment);
                                                    bool boolPNC = (oServer.Get(intServer, "pnc") == "1");
                                                    Variables oVar = new Variables(intDomainEnvironment);
                                                    string strAdminUser = oVar.Domain() + "\\" + oVar.ADUser();
                                                    string strAdminPass = oVar.ADPassword();
                                                    if (boolPNC == true)
                                                    {
                                                        Variables oVarPNC = new Variables(999);
                                                        strAdminUser = oVarPNC.Domain() + "\\" + oVarPNC.ADUser();
                                                        strAdminPass = oVarPNC.ADPassword();
                                                    }
                                                    // If windows, check to make sure it's the correct name.
                                                    if (oOperatingSystem.IsWindows(intOS) || oOperatingSystem.IsWindows2008(intOS))
                                                    {
                                                        oLog.AddEvent(strName, strSerial, "DECOMMISSION: Validating host name - " + strScripts + "psexec \\\\" + strIP + " -u " + strAdminUser + " -p <hidden> -h cmd.exe /c hostname.exe", LoggingType.Information);
                                                        string strHostname = oFunction.HostName(intServer, "HOSTNAME_DECOM", strIP, strScripts, strAdminUser, strAdminPass, 1);
                                                        if (strHostname.ToUpper() != strName.ToUpper())
                                                            strError = "The hostname (" + strHostname.ToUpper() + ") does not match the server name (" + strName.ToUpper() + ")";
                                                        else
                                                            oLog.AddEvent(strName, strSerial, "DECOMMISSION: The hostname (" + strHostname.ToUpper() + ") matches the server name (" + strName.ToUpper() + ")", LoggingType.Information);
                                                    }
                                                    if (strError == "")
                                                    {
                                                        // Attempt first to shutdown using a script
                                                        string strDecomPath = strScripts + strSub + intServer.ToString() + "_" + strNow + "_shutdown_";
                                                        string strDecomScript = strDecomPath + "reboot.vbs";
                                                        StreamWriter oDecomWriter = new StreamWriter(strDecomScript);
                                                        oDecomWriter.WriteLine("Set OpSysSet = GetObject(\"winmgmts:{impersonationLevel=impersonate,(Shutdown)}\").ExecQuery(\"select * from Win32_OperatingSystem where Primary=true\")");
                                                        oDecomWriter.WriteLine("For Each OpSys In OpSysSet");
                                                        oDecomWriter.WriteLine("Debug \"OpSys.Win32Shutdown return: \" & OpSys.Win32Shutdown(5)");
                                                        oDecomWriter.WriteLine("Next");
                                                        oDecomWriter.Flush();
                                                        oDecomWriter.Close();
                                                        oLog.AddEvent(strName, strSerial, "DECOMMISSION: The shutdown script is running...", LoggingType.Information);
                                                        int intDecomReturn = oFunction.ExecuteVBScript(intServer, false, true, "SHUTDOWN", strName, strSerial, strIP, strDecomScript, strDecomPath, "SHUTDOWN", "%windir%\\system32\\wscript.exe", "OPTIONS\\CV_DECOM_SHUTDOWN", "VBS", "", strScripts, strAdminUser, strAdminPass, 5, false, false, intLogging, boolDeleteScriptFiles);
                                                        //AuditStatus oDecomStatus = (AuditStatus)intDecomReturn;
                                                        //if (oDecomStatus == AuditStatus.Success || oDecomStatus == AuditStatus.Warning)
                                                        //{
                                                        oLog.AddEvent(strName, strSerial, "DECOMMISSION: The shutdown script finished...waiting for the IP address (" + strIP + ") to stop responding...", LoggingType.Information);
                                                        // Check IP address for response
                                                        bool boolPinged = true;
                                                        for (int ii = 0; ii < 60 && boolPinged == true; ii++)
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
                                                        if (boolPinged == false)
                                                            boolIsShutdown = true;
                                                        //}
                                                        //else
                                                        //    oLog.AddEvent(strName, strSerial, "DECOMMISSION: The shutdown script finished with an error / problem...", LoggingType.Warning);
                                                    }
                                                }
                                                else
                                                    oLog.AddEvent(strName, strSerial, "DECOMMISSION: The domain of the server is invalid", LoggingType.Warning);
                                            }
                                            else
                                                oLog.AddEvent(strName, strSerial, "DECOMMISSION: Could not find an IP address for the server", LoggingType.Warning);

                                            if (strError == "")
                                            {
                                                if (boolIsShutdown == false)
                                                {
                                                    string strILO = oAsset.GetServerOrBlade(intAsset, "ilo").ToUpper();
                                                    oLog.AddEvent(strName, strSerial, "DECOMMISSION: Server is still on, attempt to shutdown using Remote Management address (" + strILO + ")", LoggingType.Information);
                                                    if (strILO == "")
                                                        strError = "There was a problem powering off the device (The Remote Management IP Address is missing for Asset ID " + intAsset.ToString() + ")";
                                                    else
                                                    {
                                                        bool boolPower = oFunction.ExecutePower(intServer, intAsset, false, "ILO_POWEROFF", strName, intEnvironment, (boolDell ? strScripts : strScripts + strSub), intLogging, dsnAsset);
                                                        if (boolPower == false)
                                                            strError = "There was a problem powering off the device (Power off using the Remote Management address failed)";
                                                        else
                                                            boolDecom = true;
                                                    }
                                                }
                                                else
                                                    boolDecom = true;
                                            }
                                        }
                                    }
                                }
                            }
                            else
                                strError = "There was a problem powering off the device (Could not find a server for " + strName + ")";
                        }
                        else
                        {
                            oLog.AddEvent(strName, strSerial, "Decommission: Manual Intervention Completed", LoggingType.Information);
                            boolDecom = true;
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
                                oFunction.SendEmail("Auto-Provisioning DECOMMISSION Error", strEMailIdsBCC, "", "", "Auto-Provisioning DECOMMISSION Error", "<p><b>This message is to inform you that there was a problem with the DECOMMISSION process for server " + strName + ".</b><p><p>Error Message: " + strError + "</p>", true, false);
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
                            strResult = "The server was successfully decommissioned. Setting cooldown exit date to " + DateTime.Now.AddDays(14).ToString();
                            oLog.AddEvent(strName, strSerial, strResult, LoggingType.Information);
                            oRequest.AddResult(intRequest, intItem, intNumber, strResult);
                            oServer.UpdateDecommissioned(intServer, DateTime.Now.ToString());
                            oServer.UpdateAssetDecom(intServer, intAsset, DateTime.Now.ToString());
                            oAsset.UpdateDecommission(intAsset, DateTime.Now.AddDays(14), 0, strName);
                            oAsset.AddStatus(intAsset, strName, (int)AssetStatus.Decommissioned, 0, DateTime.Now);
                            oAsset.UpdateDecommissionRunning(intAsset, 0);
                            oRequest.AddResult(intRequest, intItem, intNumber, "Finished Decommissioning " + strName + " [" + DateTime.Now.ToLongDateString() + "]");
                            oLog.AddEvent(strName, strSerial, "DECOMMISSION: Finished Decommissioning", LoggingType.Information);

                            // Add to Avamar /Decom Group and remove from other groups
                            DataSet dsKey = oFunction.GetSetupValuesByKey("AVAMAR_REGISTRATIONS");
                            if (dsKey.Tables[0].Rows.Count > 0)
                            {
                                string registrations = dsKey.Tables[0].Rows[0]["Value"].ToString();
                                if (String.IsNullOrEmpty(registrations) == false)
                                {
                                    oLog.AddEvent(strName, strSerial, "DECOMMISSION: Checking Avamar CSV file located at " + registrations, LoggingType.Debug);
                                    StreamReader theReader = new StreamReader(registrations);
                                    string theContents = theReader.ReadToEnd();
                                    string[] theLines = theContents.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
                                    bool found = false;
                                    foreach (string theLine in theLines)
                                    {
                                        if (theLine.ToUpper().Contains(strName.ToUpper()))
                                        {
                                            found = true;
                                            oLog.AddEvent(strName, strSerial, "DECOMMISSION: Found the line = " + theLine, LoggingType.Debug);
                                            string[] theFields = theLine.Split(new char[] { ',' }, StringSplitOptions.None);
                                            if (theFields.Length >= 3)
                                            {
                                                string grid = theFields[0];
                                                string domain = theFields[1];
                                                string client = theFields[2];
                                                if (client.EndsWith("\r"))
                                                    client = client.Replace("\r", "");

                                                Avamar oAvamar = new Avamar(0, dsn);
                                                AvamarRegistration oAvamarRegistration = new AvamarRegistration(0, dsn);
                                                ClearViewWebServices oWebService = new ClearViewWebServices();
                                                oWebService.Timeout = Timeout.Infinite;
                                                System.Net.NetworkCredential oCredentialsDNS = new System.Net.NetworkCredential(oVariable.ADUser(), oVariable.ADPassword(), oVariable.Domain());
                                                oWebService.Credentials = oCredentialsDNS;
                                                oWebService.Url = oVariable.WebServiceURL();

                                                oLog.AddEvent(strName, strSerial, "DECOMMISSION: Querying for group(s) currently configured...", LoggingType.Debug);
                                                // First, query for groups.
                                                AvamarReturnType groups = oAvamarRegistration.API(oWebService.GetAvamarClient(grid, domain, client));
                                                if (groups.Error == false)
                                                {
                                                    List<string> members = new List<string>();
                                                    foreach (XmlNode node in groups.Nodes)
                                                    {
                                                        if (node["Attribute"].InnerText == "Member of Group")
                                                            members.Add(node["Value"].InnerText);
                                                    }
                                                    oLog.AddEvent(strName, strSerial, "DECOMMISSION: There are " + members.Count.ToString() + " group(s) currently configured", LoggingType.Debug);
                                                    if (members.Count > 0)
                                                    {
                                                        // Second, add /Decom group (so there will always be at least one group)
                                                        AvamarReturnType decom = oAvamarRegistration.API(oWebService.AddAvamarGroup(grid, domain, client, oAvamar.DecomGroup));
                                                        if (decom.Error == false)
                                                        {
                                                            oLog.AddEvent(strName, strSerial, "DECOMMISSION: Added group = " + oAvamar.DecomGroup, LoggingType.Information);
                                                            oAvamar.DeleteDecom(client);
                                                            foreach (string member in members)
                                                            {
                                                                if (String.IsNullOrEmpty(strError) == false)
                                                                    break;
                                                                // Third, remove groups (one at a time)
                                                                AvamarReturnType remove = oAvamarRegistration.API(oWebService.DeleteAvamarGroup(grid, domain, client, member));
                                                                if (remove.Error == false)
                                                                {
                                                                    oLog.AddEvent(strName, strSerial, "DECOMMISSION: Removed group = " + member, LoggingType.Information);
                                                                    // Fourth, save groups.
                                                                    oAvamar.AddDecom(client, grid, domain, member);
                                                                }
                                                                else
                                                                {
                                                                    strError = remove.Message;
                                                                    oLog.AddEvent(strName, strSerial, "DECOMMISSION: There was a problem removing the group (" + member + ") = " + strError, LoggingType.Error);
                                                                }
                                                            }
                                                            oLog.AddEvent(strName, strSerial, "DECOMMISSION: Finished with Avamar.", LoggingType.Information);
                                                        }
                                                        else
                                                        {
                                                            strError = decom.Message;
                                                            oLog.AddEvent(strName, strSerial, "DECOMMISSION: There was a problem adding the group (" + oAvamar.DecomGroup + ") = " + strError, LoggingType.Error);
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    strError = groups.Message;
                                                    oLog.AddEvent(strName, strSerial, "DECOMMISSION: There was a problem querying for groups = " + strError, LoggingType.Error);
                                                }
                                            }
                                            break;
                                        }
                                    }
                                    if (found == false)
                                        oLog.AddEvent(strName, strSerial, "DECOMMISSION: Could not find a line in the CSV file", LoggingType.Information);
                                }
                                else
                                    oLog.AddEvent(strName, strSerial, "DECOMMISSION: Skipping Avamar since the CSV location has not been set", LoggingType.Information);
                            }
                            else
                                oLog.AddEvent(strName, strSerial, "DECOMMISSION: Skipping Avamar since AVAMAR_REGISTRATIONS is not configured", LoggingType.Information);


                            //Email Notification on Successful Decomm
                            string strEmail = "";
                            string strCC = "";
                            //string strCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DECOM_PHYSICAL");
                            //string strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_INVENTORY_MANAGER");
                            string strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DECOM_PHYSICAL,EMAILGRP_INVENTORY_MANAGER");
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
            }
            catch (Exception ex)
            {
                if (ex.Message.ToUpper().StartsWith("A TRANSPORT-LEVEL ERROR") == false)
                {
                    string strError = "Physical Service (DECOMMISSION): " + strName + " (SERVERID = " + intServer.ToString() + ") (Error Message: " + ex.Message + ") ~ (Source: " + ex.Source + ") (Stack Trace: " + ex.StackTrace + ") [" + System.Environment.UserName + "]";
                    oEventLog.WriteEntry(strError, EventLogEntryType.Error);
                    SystemError(strError);
                }
            }
            ServiceTickDecomRedeploy();
        }
        private void ServiceTickDecomRedeploy()
        {
            int intServer = 0;
            string strName = "";
            try
            {
                Servers oServer = new Servers(0, dsn);
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
                OperatingSystems oOperatingSystem = new OperatingSystems(0, dsn);

                DataSet ds = oAsset.GetDecommissionDestroys(strServerTypesDecom, DateTime.Now);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    // 7 days after the poweroff, send tasks
                    string strError = "";
                    bool boolDestroy = false;
                    int intRequest = Int32.Parse(dr["requestid"].ToString());
                    int intItem = Int32.Parse(dr["itemid"].ToString());
                    int intNumber = Int32.Parse(dr["number"].ToString());
                    int intAsset = Int32.Parse(dr["assetid"].ToString());
                    int intModel = Int32.Parse(oAsset.Get(intAsset, "modelid"));
                    bool boolDR = (dr["dr"].ToString() == "1");
                    int intAnswer = 0;
                    int intOS = 0;
                    DataSet dsAsset = oServer.GetAsset(intAsset);
                    if (dsAsset.Tables[0].Rows.Count > 0)
                    {
                        intServer = Int32.Parse(dsAsset.Tables[0].Rows[0]["id"].ToString());
                        DataSet dsServer = oServer.Get(intServer);
                        if (dsServer.Tables[0].Rows.Count > 0)
                        {
                            Int32.TryParse(dsServer.Tables[0].Rows[0]["answerid"].ToString(), out intAnswer);
                            Int32.TryParse(dsServer.Tables[0].Rows[0]["osid"].ToString(), out intOS);
                        }
                    }
                    int intUser = Int32.Parse(dr["userid"].ToString());
                    strName = dr["name"].ToString();
                    string strSerial = oAsset.Get(intAsset, "serial");
                    string strResult = "No information...";
                    bool boolMissedFix = (dr["missed_fix"].ToString() != "");

                    if (dr["dr"].ToString() == "1")
                    {
                        // Just set DR servers to completed
                        oAsset.UpdateDecommissionDestroy(intAsset);
                        oLog.AddEvent(strName, strSerial, "DESTROY: Finished Destroy DR asset", LoggingType.Information);
                    }
                    else
                    {
                        if (boolMissedFix == false && oResourceRequest.GetAllService(intRequest, intDestroyErrorService, intNumber).Tables[0].Rows.Count == 0)
                        {
                            // Start Destroy
                            oAsset.UpdateDecommissionRunning(intAsset, 1);
                            oLog.AddEvent(strName, strSerial, "DESTROY: Starting Destroy (non-DR)", LoggingType.Information);

                            bool boolOSok = (oOperatingSystem.IsWindows2008(intOS) || oOperatingSystem.IsWindows(intOS) || oOperatingSystem.IsLinux(intOS));
                            if (boolOSok == false)
                                strError = "The operating system associated with this server is either unknown or not available for automated decommission";
                            else
                            {
                                oLog.AddEvent(strName, strSerial, "DESTROY: Attempting to ping name (" + strName + ")...", LoggingType.Information);
                                // Ping name to see if it is still off
                                if (oFunction.PingName(strName) == "")
                                {
                                    oLog.AddEvent(strName, strSerial, "DESTROY: name (" + strName + ") did not respond to a ping", LoggingType.Information);
                                    if (dsAsset.Tables[0].Rows.Count > 0)
                                    {
                                        boolDestroy = true;
                                        strResult = "Finished Destroying " + strName + " [" + DateTime.Now.ToString() + "]";
                                        oLog.AddEvent(strName, strSerial, "DESTROY: Finished...sending Initiating Decom Request", LoggingType.Information);
                                    }
                                    else
                                        strError = "There was a problem destroying the asset (No asset information found for physical server " + strName + ")";
                                }
                                else
                                {
                                    // Server is responding, was turned back on
                                    strError = "There was a problem destroying the asset (The server has been powered on)";
                                    oLog.AddEvent(strName, strSerial, "DESTROY: Server has been turned back on", LoggingType.Information);
                                }
                            }
                        }
                        else
                        {
                            strResult = "Destroy: Manual Intervention completed";
                            boolDestroy = true;
                        }

                        if (strError != "")
                        {
                            oAsset.UpdateDecommissionRunning(intAsset, -1);
                            oRequest.AddResult(intRequest, intItem, intNumber, strError);
                            oLog.AddEvent(strName, strSerial, "DESTROY: " + strError, LoggingType.Error);
                            oLog.AddEvent(strName, strSerial, "DESTROY: Generating error request", LoggingType.Information);
                            if (boolDecommissionErrorEmail == true)
                            {
                                // Send Email with Error
                                strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DEVELOPER_ERROR");
                                oFunction.SendEmail("Auto-Provisioning DESTROY Error", strEMailIdsBCC, "", "", "Auto-Provisioning DESTROY Error", "<p><b>This message is to inform you that there was a problem with the DESTROY process for server " + strName + ".</b><p><p>Error Message: " + strError + "</p>", true, false);
                            }
                            // Send Auto-Decom support task
                            int intDecomErrorItem = oService.GetItemId(intDestroyErrorService);
                            //int intDecomErrorNumber = oResourceRequest.GetNumber(intRequest, intDecomErrorItem);
                            int intDecomErrorNumber = intNumber;
                            double dblServiceHours = oServiceDetail.GetHours(intDestroyErrorService, 1);
                            int intDecomError = oServiceRequest.AddRequest(intRequest, intDecomErrorItem, intDestroyErrorService, 1, dblServiceHours, 2, intDecomErrorNumber, dsnServiceEditor);
                            oServiceRequest.NotifyTeamLead(intDecomErrorItem, intDecomError, intAssignPage, intViewPage, intEnvironment, "", dsnServiceEditor, dsnAsset, dsnIP, 0);
                            oLog.AddEvent(strName, strSerial, "DESTROY: Error service request generated!", LoggingType.Information);
                        }
                        else if (boolDestroy == true)
                        {
                            oLog.AddEvent(strName, strSerial, strResult, LoggingType.Information);
                            oRequest.AddResult(intRequest, intItem, intNumber, strResult);
                            oAsset.UpdateDecommissionDestroy(intAsset);
                            oAsset.UpdateDecommissionRunning(intAsset, 0);
                            //Initialize Server Decomm Process
                            Forecast oForecast = new Forecast(0, dsn);
                            strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_INVENTORY_MANAGER");
                            bool boolStorage = false;
                            bool boolLTM = false;
                            if (intAnswer > 0)
                            {
                                boolStorage = (oForecast.GetAnswer(intAnswer, "storage") == "1");
                                boolLTM = oForecast.IsHACSM(intAnswer);
                            }
                            if (boolDR == false)
                            {
                                oLog.AddEvent(strName, strSerial, "Destroy: Initiating Tasks (Storage = " + (boolStorage ? "Y" : "N") + ", Load Balancing = " + (boolLTM ? "Y" : "N") + ")", LoggingType.Information);
                                // Initiate Service Center Request
                                oServerDecommission.InitiateDecom(intServer, intModel, strName, intRequest, intItem, intNumber, (boolStorage ? 1 : 0), (boolLTM ? 1 : 0),
                                                    intAssignPage, intViewPage, intEnvironment,
                                                    intIMDecommServiceId,
                                                    dsnServiceEditor, dsnAsset, dsnIP, strDSMADMC, boolMissedFix);
                                //Forms oForm = new Forms(strScripts, boolUsePNCNaming, 0);
                                //oLog.AddEvent(strName, strSerial, "Destroy: Sending Service Center Form", LoggingType.Information);
                                //oForm.ServiceCenterDecom(intRequest, intItem, intNumber);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.ToUpper().StartsWith("A TRANSPORT-LEVEL ERROR") == false)
                {
                    string strError = "Physical Service (DESTROY): " + strName + " (SERVERID = " + intServer.ToString() + ") (Error Message: " + ex.Message + ") ~ (Source: " + ex.Source + ") (Stack Trace: " + ex.StackTrace + ") [" + System.Environment.UserName + "]";
                    oEventLog.WriteEntry(strError, EventLogEntryType.Error);
                    SystemError(strError);
                }
            }
        }
        private bool ReadOutput(int _serverid, string _type, string _file, string _name, string _serial)
        {
            Servers oServer2 = new Servers(0, dsn);
            Functions oFunction2 = new Functions(0, dsn, intEnvironment);
            bool boolContent = false;
            bool boolError = false;
            for (int ii = 0; ii < 10 && boolContent == false; ii++)
            {
                if (File.Exists(_file) == true)
                {
                    oLog.AddEvent(_name, _serial, "Server output file " + _file + " exists...reading...", LoggingType.Information);
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
                            oServer2.AddOutput(_serverid, _type, strContent);
                            oReader.Close();
                            if (File.Exists(_file) == true)
                                File.Delete(_file);
                            if (intLogging > 0)
                                oLog.AddEvent(_name, _serial, "Server output file " + _file + " finished updating...deleted files...", LoggingType.Information);
                            // Check for NET USE command
                            if (strContent.ToUpper().Contains("NET USE") == true && strContent.ToUpper().Contains(" /DELE") == false)
                            {
                                boolError = (strContent.ToUpper().Contains("THE COMMAND COMPLETED SUCCESSFULLY") == false);
                                if (boolError == true)
                                    oLog.AddEvent(_name, _serial, "The NET USE statement [" + strContent + "] does not contain [THE COMMAND COMPLETED SUCCESSFULLY]", LoggingType.Error);
                            }
                        }
                        else
                        {
                            if (intLogging > 1)
                                oLog.AddEvent(_name, _serial, "Found server output file " + _file + "...but it is blank...waiting 5 seconds...", LoggingType.Information);
                            oReader.Close();
                            Thread.Sleep(5000);
                        }
                    }
                    catch
                    {
                        if (intLogging > 1)
                            oLog.AddEvent(_name, _serial, "Cannot open server output file " + _file + "...waiting 5 seconds...", LoggingType.Information);
                        Thread.Sleep(5000);
                    }
                }
                else
                {
                    if (intLogging > 1)
                        oLog.AddEvent(_name, _serial, "Server output file " + _file + " does not exist...waiting 5 seconds...", LoggingType.Information);
                    Thread.Sleep(5000);
                }
            }
            if (boolContent == false)
            {
                oLog.AddEvent(_name, _serial, "Could Not Find Server output file " + _file, LoggingType.Error);
                boolError = true;
            }
            return boolError;
        }
        private void AddResult(int intServer, int intStep, int intType, string strResult, string strError)
        {
            OnDemand oOnDemand = new OnDemand(0, dsn);
            Servers oServer = new Servers(0, dsn);
            if (strError == "")
            {
                oOnDemand.UpdateStepDoneServer(intServer, intStep, strResult, 0, false, false);
                oServer.NextStep(intServer);
            }
            else if (oOnDemand.GetStep(intType, intStep, "resume_error") == "1")
            {
                oOnDemand.UpdateStepDoneServer(intServer, intStep, strError, 1, false, false);
                oServer.NextStep(intServer);
            }
            else
                oOnDemand.UpdateStepDoneServer(intServer, intStep, strError, 1, false, false);
        }
        private void InstallTick()
        {
            string strName = "";
            try
            {
                Servers oServer = new Servers(0, dsn);
                ServerName oServerName = new ServerName(0, dsn);
                Functions oFunction = new Functions(0, dsn, 0);
                IPAddresses oIPAddresses = new IPAddresses(0, dsnIP, dsn);
                Audit oAudit = new Audit(0, dsn);
                Domains oDomain = new Domains(0, dsn);
                DataSet dsInstalls = oServerName.GetComponentDetailSelected(strServerTypesBuild);
                if (dsInstalls.Tables[0].Rows.Count > 0)
                {
                    int intServer = Int32.Parse(dsInstalls.Tables[0].Rows[0]["serverid"].ToString());
                    string strIP = "";
                    strName = oServer.GetName(intServer, boolUsePNCNaming);
                    string strIPAddress = oServer.GetIPBuild(intServer);
                    int intIPAddressBuild1 = 0;
                    if (strIPAddress != "")
                        intIPAddressBuild1 = Int32.Parse(strIPAddress);
                    if (intIPAddressBuild1 > 0)
                        strIP = oIPAddresses.GetName(intIPAddressBuild1, 0);
                    else
                    {
                        string strDHCP = oServer.Get(intServer, strZeus);
                        if (strDHCP == "SUCCESS")
                            strIP = strName;
                        else if (strDHCP == "" || strDHCP == "0")
                            strIP = "";
                        else
                            strIP = strDHCP;
                    }
                    if (strIP != "")
                    {
                        DataSet dsActive = oServerName.GetComponentDetailSelectedActive(intServer);
                        if (dsActive.Tables[0].Rows.Count == 0)
                        {
                            DataSet dsServer = oServer.Get(intServer);
                            int intAnswer = Int32.Parse(dsServer.Tables[0].Rows[0]["answerid"].ToString());
                            int intMHS = 0;
                            Int32.TryParse(dsServer.Tables[0].Rows[0]["mhs"].ToString(), out intMHS);
                            int intDomain = Int32.Parse(dsServer.Tables[0].Rows[0]["domainid"].ToString());
                            int intDomainEnvironment = Int32.Parse(oDomain.Get(intDomain, "environment"));
                            bool boolPNC = (dsServer.Tables[0].Rows[0]["pnc"].ToString() == "1");
                            string strSource = "SERVER";
                            Variables oVariable = new Variables(intEnvironment);
                            string strAdminUser = strName + "\\" + oVariable.LocalAdminUsername(boolPNC);
                            string strAdminPass = oVariable.LocalAdminPassword(boolPNC, strName);
                            if (boolPNC == true)
                            {
                                Variables oVarPNC = new Variables(999);
                                strAdminUser = oVarPNC.Domain() + "\\" + oVarPNC.ADUser();
                                strAdminPass = oVarPNC.ADPassword();
                            }
                            int intDetail = Int32.Parse(dsInstalls.Tables[0].Rows[0]["detailid"].ToString());
                            int intScript = 0;
                            Int32.TryParse(dsInstalls.Tables[0].Rows[0]["scriptid"].ToString(), out intScript);
                            bool boolPinged = false;
                            for (int ii = 0; ii < 60 && boolPinged == false; ii++)
                            {
                                Thread.Sleep(3000);
                                Ping oPinged = new Ping();
                                string strPinged = "";
                                try
                                {
                                    PingReply oPingReply = oPinged.Send(strIP);
                                    strPinged = oPingReply.Status.ToString().ToUpper();
                                }
                                catch { }
                                boolPinged = (strPinged == "SUCCESS");
                            }
                            if (boolPinged == false)
                            {
                                // Set DONE = -10 which will cause a controlled error when checking the status above
                                oServerName.UpdateComponentDetailSelected(intServer, intDetail, -10);
                            }
                            else
                            {
                                bool boolSuccess = false;
                                oServerName.UpdateComponentDetailSelected(intServer, intDetail, 0);
                                oLog.AddEvent(strName, "", "Starting installation (" + dsInstalls.Tables[0].Rows[0]["name"].ToString() + ")", LoggingType.Information);
                                DataSet dsScripts = oServerName.GetComponentDetailScripts(intDetail, 1);
                                if (dsScripts.Tables[0].Rows.Count > 0 && oServerName.GetComponentDetail(intDetail, "install") == "1")
                                {
                                    string strBatch1 = strScripts + strSub + intServer.ToString() + "_" + intDetail.ToString() + "_install_1.bat";
                                    // ********** START : CHANGED CODE ON 7/31/2008 TO BATCH FILE COPY *******************
                                    // 1st part - create BAT file to copy to server (install_1.bat)
                                    StreamWriter oWriter1 = new StreamWriter(strBatch1);
                                    foreach (DataRow drScript in dsScripts.Tables[0].Rows)
                                        oWriter1.WriteLine(oFunction.ProcessLine(drScript["script"].ToString(), dsServer.Tables[0].Rows[0]));
                                    oWriter1.Flush();
                                    oWriter1.Close();
                                    // 2nd part - create BAT file to do the copy (install_2.bat)
                                    string strBatch2 = strScripts + strSub + intServer.ToString() + "_" + intDetail.ToString() + "_install_2.bat";
                                    StreamWriter oWriter2 = new StreamWriter(strBatch2);
                                    oWriter2.WriteLine("F:");
                                    oWriter2.WriteLine("cd " + strScripts + strSub);
                                    oWriter2.WriteLine("net use \\\\" + strIP + "\\C$ /user:" + strAdminUser + " " + strAdminPass + "");
                                    oWriter2.WriteLine("mkdir \\\\" + strIP + "\\C$\\OPTIONS");
                                    oWriter2.WriteLine("copy " + strBatch1 + " \\\\" + strIP + "\\C$\\OPTIONS\\CV_INSTALL_" + intDetail.ToString() + ".BAT");
                                    oWriter2.Flush();
                                    oWriter2.Close();
                                    // 3rd part - run the batch file to perform copy
                                    string strFile1 = strScripts + strSub + intServer.ToString() + "_" + intDetail.ToString() + "_install_1.vbs";
                                    string strFile1Out = strScripts + strSub + intServer.ToString() + "_" + intDetail.ToString() + "_install_1.txt";
                                    StreamWriter oWriter3 = new StreamWriter(strFile1);
                                    oWriter3.WriteLine("Dim objShell");
                                    oWriter3.WriteLine("Set objShell = CreateObject(\"WScript.Shell\")");
                                    oWriter3.WriteLine("objShell.Run(\"cmd.exe /c " + strBatch2 + " > " + strFile1Out + "\")");
                                    oWriter3.WriteLine("Set objShell = Nothing");
                                    oWriter3.Flush();
                                    oWriter3.Close();
                                    ILaunchScript oScript1 = new SimpleLaunchWsh(strFile1, "", true, 5) as ILaunchScript;
                                    oScript1.Launch();
                                    bool boolOutputStart = ReadOutput(intServer, "INSTALL_" + dsInstalls.Tables[0].Rows[0]["name"].ToString() + "_START", strFile1Out, strName, "");
                                    if (boolOutputStart == false)
                                    {
                                        // 4th part - file has been copied, do the PSEXEC to install application
                                        System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo(strScripts + "psexec");
                                        info.WorkingDirectory = strScripts;
                                        info.Arguments = "\\\\" + strIP + " -u " + strAdminUser + " -p " + strAdminPass + " -i cmd.exe /c C:\\OPTIONS\\CV_INSTALL_" + intDetail.ToString() + ".BAT";
                                        oLog.AddEvent(strName, "", "PSEXEC Script = " + "\\\\" + strIP + " -u " + strAdminUser + " -p ***** -i cmd.exe /c C:\\OPTIONS\\CV_INSTALL_" + intDetail.ToString() + ".BAT", LoggingType.Information);
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
                                            string strBatch3 = strScripts + strSub + intServer.ToString() + "_" + intDetail.ToString() + "_install_3.bat";
                                            StreamWriter oWriter4 = new StreamWriter(strBatch3);
                                            oWriter4.WriteLine("del \\\\" + strIP + "\\C$\\OPTIONS\\CV_INSTALL_" + intDetail.ToString() + ".BAT");
                                            oWriter4.WriteLine("net use \\\\" + strIP + "\\C$ /dele");
                                            oWriter4.Flush();
                                            oWriter4.Close();
                                            // 6th part - run the batch file to perform copy
                                            string strFile2 = strScripts + strSub + intServer.ToString() + "_" + intDetail.ToString() + "_install_2.vbs";
                                            string strFile2Out = strScripts + strSub + intServer.ToString() + "_" + intDetail.ToString() + "_install_2.txt";
                                            StreamWriter oWriter5 = new StreamWriter(strFile2);
                                            oWriter5.WriteLine("Dim objShell");
                                            oWriter5.WriteLine("Set objShell = CreateObject(\"WScript.Shell\")");
                                            oWriter5.WriteLine("objShell.Run(\"cmd.exe /c " + strBatch3 + " > " + strFile2Out + "\")");
                                            oWriter5.WriteLine("Set objShell = Nothing");
                                            oWriter5.Flush();
                                            oWriter5.Close();
                                            ILaunchScript oScript2 = new SimpleLaunchWsh(strFile2, "", true, 30) as ILaunchScript;
                                            oScript2.Launch();
                                            ReadOutput(intServer, "INSTALL_" + dsInstalls.Tables[0].Rows[0]["name"].ToString() + "_END", strFile2Out, strName, "");
                                            oLog.AddEvent(strName, "", "Finished installation (" + dsInstalls.Tables[0].Rows[0]["name"].ToString() + ")", LoggingType.Information);

                                            boolSuccess = true;
                                        }
                                        else
                                            oServerName.UpdateComponentDetailSelected(intServer, intDetail, -10);
                                    }
                                    else
                                        oServerName.UpdateComponentDetailSelected(intServer, intDetail, -10);
                                }
                                else
                                {
                                    // No scripts, or install flag not set.
                                    boolSuccess = true;
                                }

                                if (boolSuccess == true)
                                {
                                    // Check for Audit Task associated
                                    DataSet dsAudit = oAudit.GetScript(intScript);
                                    if (dsAudit.Tables[0].Rows.Count > 0)
                                    {
                                        DataRow drAudit = dsAudit.Tables[0].Rows[0];
                                        int intAuditIDError = 0;
                                        string strAuditName = drAudit["name"].ToString();
                                        string strEXE = drAudit["exe"].ToString();
                                        string strExtension = drAudit["extension"].ToString();
                                        string strPath = drAudit["path"].ToString();    // The path to the script
                                        int intAuditTimeout = Int32.Parse(drAudit["timeout"].ToString());
                                        bool boolRemote = (drAudit["local"].ToString() == "0");
                                        string strAuditScriptPath = strScripts + strSub + intServer.ToString() + "_" + intDetail.ToString() + "_audit_";
                                        int intAuditReturn = -10;

                                        oLog.AddEvent(strName, "", "Starting Audit Script (" + strAuditName + ")", LoggingType.Information);

                                        // Delete all data from TEST and PROD
                                        //oAudit.DeleteServer(intServer);
                                        //oAudit.DeleteServerDetailRemote(intServer);

                                        // Add data to TEST and PROD
                                        int intAuditID = oAudit.AddServer(intServer, -999, intScript, false, AuditStatus.Running);
                                        string strParameters = oAudit.GetScriptParameters(drAudit["parameters"].ToString(), intAuditID, strName, strIP, 0, intMHS);

                                        if (boolRemote == true)
                                        {
                                            // Copy the script local and run from this server.
                                            strAuditName = "Audit Script # " + intScript.ToString() + "(" + strAuditName + ")";
                                            intAuditReturn = oFunction.ExecuteVBScript(intServer, true, false, strAuditName, strName, "", strIP, strPath, strAuditScriptPath, "Script" + intScript.ToString() + "_", strEXE, "OPTIONS\\CV_AUDIT_SCRIPT_" + intScript.ToString() + "_", strExtension, strParameters, strScripts, strAdminUser, strAdminPass, intAuditTimeout, false, false, intLogging, boolDeleteScriptFiles);
                                        }
                                        else
                                        {
                                            strAuditName = "Audit Script # " + intScript.ToString() + "(" + strAuditName + ")";
                                            intAuditReturn = oFunction.ExecuteVBScript(intServer, false, false, strAuditName, strName, "", strIP, strPath, strAuditScriptPath, "Script" + intScript.ToString() + "_", strEXE, "OPTIONS\\CV_AUDIT_SCRIPT_" + intScript.ToString() + "_", strExtension, strParameters, strScripts, strAdminUser, strAdminPass, intAuditTimeout, false, false, intLogging, boolDeleteScriptFiles);
                                        }

                                        oLog.AddEvent(strName, "", "Finished Audit Script (" + strAuditName + " = " + intAuditReturn.ToString() + ")", LoggingType.Information);

                                        AuditStatus oAuditStatus = (AuditStatus)intAuditReturn;
                                        if (oAuditStatus == AuditStatus.Success || oAuditStatus == AuditStatus.Warning)
                                        {
                                            oAudit.UpdateServer(intAuditID, oAuditStatus, DateTime.Now.ToString());
                                            oServerName.UpdateComponentDetailSelected(intServer, intDetail, 1);
                                        }
                                        else
                                        {
                                            string strAuditError = "";
                                            Services oService = new Services(0, dsn);
                                            ResourceRequest oResourceRequest = new ResourceRequest(0, dsn);
                                            ServiceRequests oServiceRequest = new ServiceRequests(0, dsn);
                                            Forecast oForecast = new Forecast(0, dsn);

                                            oAudit.UpdateServer(intAuditID, oAuditStatus, "");
                                            if (oAuditStatus == AuditStatus.Error)
                                                strAuditError = "Failed Audit: " + strAuditName;
                                            else if (oAuditStatus == AuditStatus.TimedOut)
                                                strAuditError = "Failed Audit (Timeout): " + strAuditName;
                                            else if (oAuditStatus == AuditStatus.NetUseError)
                                                strAuditError = "Failed Audit (NET USE ERROR): " + strAuditName;
                                            else
                                                strAuditError = "Unexpected Audit Error (" + intAuditReturn.ToString() + "): " + strAuditName;

                                            // Initiate Workflow
                                            int intRequest = oForecast.GetRequestID(intAnswer, true);
                                            int intServerAuditErrorItem = oService.GetItemId(intServerAuditErrorService);
                                            int intServerAuditErrorNumber = oResourceRequest.GetNumber(intRequest, intServerAuditErrorItem);
                                            oAudit.AddError(intRequest, intServerAuditErrorService, intServerAuditErrorNumber, intAuditID, -1, false);
                                            int intServerAuditError = oResourceRequest.Add(intRequest, intServerAuditErrorItem, intServerAuditErrorService, intServerAuditErrorNumber, "Server Audit Exception (" + strName + ")", 1, 0.00, 2, 1, 1, 1);
                                            if (oServiceRequest.NotifyApproval(intServerAuditError, intResourceRequestApprove, intEnvironment, "", dsnServiceEditor) == false)
                                                oServiceRequest.NotifyTeamLead(intServerAuditErrorItem, intServerAuditError, intAssignPage, intViewPage, intEnvironment, "", dsnServiceEditor, dsnAsset, dsnIP, 0);
                                        }
                                    }
                                    else
                                        oServerName.UpdateComponentDetailSelected(intServer, intDetail, 1);
                                }
                            }
                        }
                        else
                            oLog.AddEvent(strName, "", "Installation already running", LoggingType.Warning);
                    }
                    else
                        oLog.AddEvent(strName, "", "Cannot connect to server " + strName + " to start installations", LoggingType.Warning);
                }
                else if (intLogging > 1)
                    oEventLog.WriteEntry("No installations to run", EventLogEntryType.Information);
            }
            catch (Exception ex)
            {
                if (ex.Message.ToUpper().StartsWith("A TRANSPORT-LEVEL ERROR") == false)
                {
                    string strError = "Physical Service (INSTALLATION): " + strName + " (Error Message: " + ex.Message + ") ~ (Source: " + ex.Source + ") (Stack Trace: " + ex.StackTrace + ") [" + System.Environment.UserName + "]";
                    oEventLog.WriteEntry(strError, EventLogEntryType.Error);
                    SystemError(strError);
                }
            }
        }

        private bool RunCGI(string _url)
        {
            bool boolReturn = false;
            HttpWebRequest oWebRequest1 = (HttpWebRequest)WebRequest.Create(_url);
            try
            {
                HttpWebResponse oWebResponse1 = (HttpWebResponse)oWebRequest1.GetResponse();
                oWebResponse1.Close();
                boolReturn = true;
            }
            catch { }
            return boolReturn;
        }

        public void ExecuteMISAudits()
        {
            Functions oFunction = new Functions(0, dsn, intEnvironment);
            string strErrorName = "";
            try
            {
                Audit oAudit = new Audit(0, dsn);
                Domains oDomain = new Domains(0, dsn);
                IPAddresses oIPAddresses = new IPAddresses(0, dsnIP, dsn);
                Servers oServer = new Servers(0, dsn);
                Asset oAsset = new Asset(0, dsnAsset, dsn);

                DateTime _now = DateTime.Now;
                string strNow = _now.Day.ToString() + _now.Month.ToString() + _now.Year.ToString() + _now.Hour.ToString() + _now.Minute.ToString() + _now.Second.ToString() + _now.Millisecond.ToString();
                DataSet ds = oAudit.GetServerScriptsMIS(strServerTypesBuild);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    int intServer = Int32.Parse(dr["id"].ToString());
                    int intRequest = Int32.Parse(dr["requestid"].ToString());
                    string strName = oServer.GetName(intServer, true);
                    strErrorName = strName;
                    int intAsset = 0;
                    Int32.TryParse(dr["assetid"].ToString(), out intAsset);
                    if (intAsset == 0)
                    {
                        // Skip since nothing to do
                        oServer.UpdateMISAudits(intServer, DateTime.Now.ToString());
                        oLog.AddEvent(strName, "Missing", "MIS audit scripts skipped", LoggingType.Information);
                    }
                    else
                    {
                        string strSerial = oAsset.Get(intAsset, "serial");
                        int intIP = 0;
                        Int32.TryParse(dr["ipaddressid"].ToString(), out intIP);
                        string strIP = "";
                        if (intIP > 0)
                            strIP = oIPAddresses.GetName(intIP, 0);
                        else
                            strIP = oServer.Get(intServer, "dhcp");
                        int intClass = Int32.Parse(dr["classid"].ToString());
                        int intEnv = Int32.Parse(dr["environmentid"].ToString());
                        int intModel = Int32.Parse(oAsset.Get(intAsset, "modelid"));
                        int intOS = Int32.Parse(dr["osid"].ToString());
                        int intSP = Int32.Parse(dr["spid"].ToString());
                        int intAddress = Int32.Parse(dr["addressid"].ToString());
                        bool boolSAN = (dr["storage"].ToString() == "1");
                        bool boolCluster = (dr["cluster"].ToString() == "1");
                        bool boolTSM = (dr["tsm"].ToString() == "1");
                        int intDomain = Int32.Parse(dr["domainid"].ToString());
                        int intDomainEnvironment = Int32.Parse(oDomain.Get(intDomain, "environment"));
                        Variables oVar = new Variables(intDomainEnvironment);
                        string strAdminUser = oVar.Domain() + "\\" + oVar.ADUser();
                        string strAdminPass = oVar.ADPassword();
                        if (intAuditCount < intAuditCounts)
                        {
                            oLog.AddEvent(strName, strSerial, "Starting MIS Audits (Physical) for ServerID " + intServer.ToString(), LoggingType.Information);
                            // RUN AUDIT TASKS
                            AuditThread oAuditThread = new AuditThread(intServer, strName, strSerial, intIP, true, strIP, intClass, intEnv, intModel, intOS, intSP, intAddress, boolSAN, boolCluster, boolTSM, 0, intRequest, intServerAuditErrorServiceMIS, intResourceRequestApprove, intAssignPage, intViewPage, strScripts, strSub, strAdminUser, strAdminPass, intEnvironment, 2, dsn, dsnAsset, dsnIP, dsnServiceEditor, true, true, this, true);
                            ThreadStart oAuditThreadStart = new ThreadStart(oAuditThread.Begin);
                            Thread oAuditThreadProcess = new Thread(oAuditThreadStart);
                            oAuditThreadProcess.Start();
                        }
                    }
                }
            }
            catch (Exception oError)
            {
                string strError = "Execute MIS Audit Error: " + "(Error Message: " + oError.Message + ") (Source: " + oError.Source + ") (Stack Trace: " + oError.StackTrace + ")";
                SystemError(strError);
                //strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DEVELOPER_ERROR");
                //oFunction.SendEmail("ERROR: Execute MIS Audit Error", strEMailIdsBCC, "", "", "ERROR: Execute MIS Audit Error", "<p><b>An error occurred when attempting to execute an MIS audit...</b></p><p>Error Message:" + strError + "</p>", true, false);
                oEventLog.WriteEntry(String.Format(strErrorName + ": " + strError), EventLogEntryType.Error);
                oLog.AddEvent(strErrorName, "", strError, LoggingType.Error);
            }
        }

        private void SystemError(string _error)
        {
            SystemError(0, 0, _error, 0, 0);
        }
        private void SystemError(int _server, int _stepid, string _error, int _assetid, int _modelid)
        {
            try
            {
                Settings oSetting = new Settings(0, dsn);
                oSetting.SystemError(_server, 0, _stepid, _error, _assetid, _modelid, false, null, intEnvironment, dsnAsset);
            }
            catch (Exception exSystemError)
            {
                // If database is offline, it will cause a fatal error AND stop the service.  Stop that from happening by using a catch here.
                oEventLog.WriteEntry("The database is currently offline.  Error message = " + exSystemError.Message, EventLogEntryType.Error);
            }
        }
        private string ExecuteSSH(string _command, SshExec _ssh) 
        {
            return _command + " -> " + _ssh.RunCommand(_command) + "<br/>";
        }
        private string ExecuteSSH(string _command, SshShell _shell) 
        {
            _shell.WriteLine(_command);
            string strReturn = _shell.Expect("#");
            return strReturn + "<br/>";
        }

        private string ConfigureNexus(int intAnswer, int intServer, string strName, string strSerial, string strIPChange, int intOS, string strRDPVLAN, bool boolBlade, string strVLAN, int intIPAddressBackup, int intIPAddressCluster, int intAsset, int intAssetDR, int intClass, int intModel, DataSet dsSwitch) 
        {
            OperatingSystems oOperatingSystem = new OperatingSystems(0, dsn);
            Forecast oForecast = new Forecast(0, dsn);
            Servers oServer = new Servers(0, dsn);
            Classes oClass = new Classes(0, dsn);
            ModelsProperties oModelsProperties = new ModelsProperties(0, dsn);
            IPAddresses oIPAddresses = new IPAddresses(0, dsnIP, dsn);
            Asset oAsset = new Asset(0, dsnAsset, dsn);
            Variables oVariable = new Variables(intEnvironment);

            string strError = "";
            oLog.AddEvent(intAnswer, strName, strSerial, "Configuring Nexus Switchports...", LoggingType.Information);
            string strSwitchA = "";
            string strInterfaceA = "";
            string strSwitchB = "";
            string strInterfaceB = "";
            string strSwitchAdr = "";
            string strInterfaceAdr = "";
            string strSwitchBdr = "";
            string strInterfaceBdr = "";
            string strIPDescription = strIPChange;
            int intIP1 = 0;
            if (strIPDescription.Contains(".") == true)
            {
                Int32.TryParse(strIPDescription.Substring(0, strIPDescription.IndexOf(".")), out intIP1);
                strIPDescription = strIPDescription.Substring(strIPDescription.IndexOf(".") + 1);
            }
            int intIP2 = 0;
            if (strIPDescription.Contains(".") == true)
            {
                Int32.TryParse(strIPDescription.Substring(0, strIPDescription.IndexOf(".")), out intIP2);
                strIPDescription = strIPDescription.Substring(strIPDescription.IndexOf(".") + 1);
            }
            int intIP3 = 0;
            int intIP4 = 0;
            if (strIPDescription.Contains(".") == true)
            {
                Int32.TryParse(strIPDescription.Substring(0, strIPDescription.IndexOf(".")), out intIP3);
                strIPDescription = strIPDescription.Substring(strIPDescription.IndexOf(".") + 1);
                Int32.TryParse(strIPDescription, out intIP4);
            }
            string strDescription = intIP3.ToString() + "." + intIP4.ToString() + "_" + strName + "-";
            string strNative = (oOperatingSystem.IsSolaris(intOS) ? strRDPVLAN : "");

            if (boolBlade == true)
            {
                oLog.AddEvent(intAnswer, strName, strSerial, "BLADE! All ports will be trunked...", LoggingType.Information);
                // All blades are trunked over the same 2 or 4 connections)
                DellBladeSwitchportMode oDellMode = DellBladeSwitchportMode.Trunk;

                // Only apply the VLAN(s) needed.
                string strDellVLAN = strVLAN;

                // Add the backup VLAN (not available for Solaris)
                if (intIPAddressBackup > 0)
                {
                    int intIPNetwork = Int32.Parse(oIPAddresses.Get(intIPAddressBackup, "networkid"));
                    int intIPVlan = Int32.Parse(oIPAddresses.GetNetwork(intIPNetwork, "vlanid"));
                    strDellVLAN += ",";
                    strDellVLAN += oIPAddresses.GetVlan(intIPVlan, "vlan");
                    oLog.AddEvent(intAnswer, strName, strSerial, "Adding Backup VLAN to trunk = " + oIPAddresses.GetVlan(intIPVlan, "vlan"), LoggingType.Information);
                }

                // Add the cluster VLAN (Solaris servers have 4 NICs, so the Clustered Network does not have to be configured)
                if (oForecast.IsHACluster(intAnswer) == true)
                {
                    if (intIPAddressCluster > 0)
                    {
                        int intIPNetwork = Int32.Parse(oIPAddresses.Get(intIPAddressCluster, "networkid"));
                        int intIPVlan = Int32.Parse(oIPAddresses.GetNetwork(intIPNetwork, "vlanid"));
                        strDellVLAN += ",";
                        strDellVLAN += oIPAddresses.GetVlan(intIPVlan, "vlan");
                        oLog.AddEvent(intAnswer, strName, strSerial, "Adding Cluster VLAN to trunk = " + oIPAddresses.GetVlan(intIPVlan, "vlan"), LoggingType.Information);
                    }
                    else
                    {
                        oLog.AddEvent(intAnswer, strName, strSerial, "This is cluster, but no private address was assigned...configuring default VLAN", LoggingType.Information);
                        if (oClass.IsProd(intClass))
                            strDellVLAN += ",1100";
                        if (oClass.IsQA(intClass))
                            strDellVLAN += ",2100";
                        if (oClass.IsTestDev(intClass))
                            strDellVLAN += ",2600";
                    }
                }


                int intEnclosure = Int32.Parse(oAsset.GetServerOrBlade(intAsset, "enclosureid"));
                int intSlot = Int32.Parse(oAsset.GetServerOrBlade(intAsset, "slot"));
                string strEnclosure = oAsset.GetStatus(intEnclosure, "name");
                DataSet dsDellBlade = oAsset.GetDellBladeSwitchports(strEnclosure, intSlot);
                if (dsDellBlade.Tables[0].Rows.Count == 1)
                {
                    DataRow drDellBlade = dsDellBlade.Tables[0].Rows[0];
                    strSwitchA = drDellBlade["switchA"].ToString();
                    strInterfaceA = drDellBlade["interfaceA"].ToString();
                    strSwitchB = drDellBlade["switchB"].ToString();
                    strInterfaceB = drDellBlade["interfaceB"].ToString();
                }
                else
                {
                    strError = "There should only be one (1) record for the Dell Blade Switchport information ~ (there were " + dsDellBlade.Tables[0].Rows.Count.ToString() + " records), Enclosure " + strEnclosure + ", Slot " + intSlot.ToString();
                    oLog.AddEvent(intAnswer, strName, strSerial, strError, LoggingType.Error);
                }

                int intSlotDR = 0;
                string strEnclosureDR = "";
                if (intAssetDR > 0)
                {
                    int intEnclosureDR = Int32.Parse(oAsset.GetServerOrBlade(intAssetDR, "enclosureid"));
                    intSlotDR = Int32.Parse(oAsset.GetServerOrBlade(intAssetDR, "slot"));
                    strEnclosureDR = oAsset.GetStatus(intEnclosureDR, "name");
                    DataSet dsDellBladeDR = oAsset.GetDellBladeSwitchports(strEnclosureDR, intSlotDR);
                    if (dsDellBladeDR.Tables[0].Rows.Count == 1)
                    {
                        DataRow drDellBladeDR = dsDellBladeDR.Tables[0].Rows[0];
                        strSwitchAdr = drDellBladeDR["switchA"].ToString();
                        strInterfaceAdr = drDellBladeDR["interfaceA"].ToString();
                        strSwitchBdr = drDellBladeDR["switchB"].ToString();
                        strInterfaceBdr = drDellBladeDR["interfaceB"].ToString();
                    }
                    else
                    {
                        strError = "There should only be one (1) record for the Dell Blade Switchport information ~ (there were " + dsDellBladeDR.Tables[0].Rows.Count.ToString() + " records), DR Enclosure " + strEnclosureDR + ", DR Slot " + intSlotDR.ToString();
                        oLog.AddEvent(intAnswer, strName, strSerial, strError, LoggingType.Error);
                    }
                }

                // Change switchports (half height)
                if (strError == "" && strSwitchA != "" && strInterfaceA != "")
                {
                    if (oServer.GetSwitchport(intServer, strSwitchA, strInterfaceA).Tables[0].Rows.Count == 0)
                    {
                        strError = Nexus(intAnswer, strName, strSerial, strSwitchA, strInterfaceA, oDellMode, strDellVLAN, strNative, strDescription + "pri", true, oVariable, oAsset, intAsset);
                        if (strError == "")
                            oServer.AddSwitchport(intServer, strSwitchA, strInterfaceA);
                    }
                }

                if (strError == "" && strSwitchB != "" && strInterfaceB != "")
                {
                    if (oServer.GetSwitchport(intServer, strSwitchB, strInterfaceB).Tables[0].Rows.Count == 0)
                    {
                        strError = Nexus(intAnswer, strName, strSerial, strSwitchB, strInterfaceB, oDellMode, strDellVLAN, strNative, strDescription + "sec", true, oVariable, oAsset, intAsset);
                        if (strError == "")
                            oServer.AddSwitchport(intServer, strSwitchB, strInterfaceB);
                    }
                }

                if (strError == "" && strSwitchAdr != "" && strInterfaceAdr != "")
                {
                    if (oServer.GetSwitchport(intServer, strSwitchAdr, strInterfaceAdr).Tables[0].Rows.Count == 0)
                    {
                        strError = Nexus(intAnswer, strName, strSerial, strSwitchAdr, strInterfaceAdr, oDellMode, strDellVLAN, strNative, strDescription + "pri", true, oVariable, oAsset, intAsset);
                        if (strError == "")
                            oServer.AddSwitchport(intServer, strSwitchAdr, strInterfaceAdr);
                    }
                }

                if (strError == "" && strSwitchBdr != "" && strInterfaceBdr != "")
                {
                    if (oServer.GetSwitchport(intServer, strSwitchBdr, strInterfaceBdr).Tables[0].Rows.Count == 0)
                    {
                        strError = Nexus(intAnswer, strName, strSerial, strSwitchBdr, strInterfaceBdr, oDellMode, strDellVLAN, strNative, strDescription + "sec", true, oVariable, oAsset, intAsset);
                        if (strError == "")
                            oServer.AddSwitchport(intServer, strSwitchBdr, strInterfaceBdr);
                    }
                }

                if (oModelsProperties.IsFullHeight(intModel) == true)
                {
                    // Full height blade - configure the other ports
                    intSlot = (intSlot >= 9 ? intSlot - 8 : intSlot + 8);
                    oLog.AddEvent(intAnswer, strName, strSerial, "FULL HEIGHT! Configuring other ports (slot # " + intSlot.ToString() + ")...", LoggingType.Information);
                    dsDellBlade = oAsset.GetDellBladeSwitchports(strEnclosure, intSlot);
                    if (dsDellBlade.Tables[0].Rows.Count == 1)
                    {
                        DataRow drDellBlade = dsDellBlade.Tables[0].Rows[0];
                        strSwitchA = drDellBlade["switchA"].ToString();
                        strInterfaceA = drDellBlade["interfaceA"].ToString();
                        strSwitchB = drDellBlade["switchB"].ToString();
                        strInterfaceB = drDellBlade["interfaceB"].ToString();
                    }
                    else
                    {
                        strError = "There should only be one (1) record for the Dell Blade Switchport information ~ (there were " + dsDellBlade.Tables[0].Rows.Count.ToString() + " records), Enclosure " + strEnclosure + ", Slot " + intSlot.ToString();
                        oLog.AddEvent(intAnswer, strName, strSerial, strError, LoggingType.Error);
                    }

                    if (intAssetDR > 0)
                    {
                        intSlotDR = (intSlotDR >= 9 ? intSlotDR - 8 : intSlotDR + 8);
                        oLog.AddEvent(intAnswer, strName, strSerial, "Configuring other ports (DR slot # " + intSlotDR.ToString() + ")...", LoggingType.Information);
                        DataSet dsDellBladeDR = oAsset.GetDellBladeSwitchports(strEnclosureDR, intSlotDR);
                        if (dsDellBladeDR.Tables[0].Rows.Count == 1)
                        {
                            DataRow drDellBladeDR = dsDellBladeDR.Tables[0].Rows[0];
                            strSwitchAdr = drDellBladeDR["switchA"].ToString();
                            strInterfaceAdr = drDellBladeDR["interfaceA"].ToString();
                            strSwitchBdr = drDellBladeDR["switchB"].ToString();
                            strInterfaceBdr = drDellBladeDR["interfaceB"].ToString();
                        }
                        else
                        {
                            strError = "There should only be one (1) record for the Dell Blade Switchport information ~ (there were " + dsDellBladeDR.Tables[0].Rows.Count.ToString() + " records), DR Enclosure " + strEnclosureDR + ", DR Slot " + intSlotDR.ToString();
                            oLog.AddEvent(intAnswer, strName, strSerial, strError, LoggingType.Error);
                        }
                    }

                    // Change switchports (full height)
                    if (strError == "" && strSwitchA != "" && strInterfaceA != "")
                    {
                        if (oServer.GetSwitchport(intServer, strSwitchA, strInterfaceA).Tables[0].Rows.Count == 0)
                        {
                            strError = Nexus(intAnswer, strName, strSerial, strSwitchA, strInterfaceA, oDellMode, strDellVLAN, strNative, strDescription + "3", true, oVariable, oAsset, intAsset);
                            if (strError == "")
                                oServer.AddSwitchport(intServer, strSwitchA, strInterfaceA);
                        }
                    }

                    if (strError == "" && strSwitchB != "" && strInterfaceB != "")
                    {
                        if (oServer.GetSwitchport(intServer, strSwitchB, strInterfaceB).Tables[0].Rows.Count == 0)
                        {
                            strError = Nexus(intAnswer, strName, strSerial, strSwitchB, strInterfaceB, oDellMode, strDellVLAN, strNative, strDescription + "4", true, oVariable, oAsset, intAsset);
                            if (strError == "")
                                oServer.AddSwitchport(intServer, strSwitchB, strInterfaceB);
                        }
                    }

                    if (strError == "" && strSwitchAdr != "" && strInterfaceAdr != "")
                    {
                        if (oServer.GetSwitchport(intServer, strSwitchAdr, strInterfaceAdr).Tables[0].Rows.Count == 0)
                        {
                            strError = Nexus(intAnswer, strName, strSerial, strSwitchAdr, strInterfaceAdr, oDellMode, strDellVLAN, strNative, strDescription + "3", true, oVariable, oAsset, intAsset);
                            if (strError == "")
                                oServer.AddSwitchport(intServer, strSwitchAdr, strInterfaceAdr);
                        }
                    }

                    if (strError == "" && strSwitchBdr != "" && strInterfaceBdr != "")
                    {
                        if (oServer.GetSwitchport(intServer, strSwitchBdr, strInterfaceBdr).Tables[0].Rows.Count == 0)
                        {
                            strError = Nexus(intAnswer, strName, strSerial, strSwitchBdr, strInterfaceBdr, oDellMode, strDellVLAN, strNative, strDescription + "4", true, oVariable, oAsset, intAsset);
                            if (strError == "")
                                oServer.AddSwitchport(intServer, strSwitchBdr, strInterfaceBdr);
                        }
                    }

                }
            }
            else
            {
                oLog.AddEvent(intAnswer, strName, strSerial, "RACKMOUT! Solaris is trunked, Windows and Linux are not trunked. All ports configured individually...", LoggingType.Information);
                // Rackmount servers - have separate ports.
                string strDellVLAN = strVLAN;

                // By default, they are not trunked...
                DellBladeSwitchportMode oDellMode = DellBladeSwitchportMode.Access;

                // ...except for Solaris = TRUNKED individually.
                if (oOperatingSystem.IsSolaris(intOS) == true)
                {
                    oDellMode = DellBladeSwitchportMode.Trunk;
                    // Add build DHCP network to solaris builds
                    //strDellVLAN += "," + strRDPVLAN;
                }

                // Add the backup VLAN
                string strBackupVLAN = "";
                if (intIPAddressBackup > 0)
                {
                    int intIPNetwork = Int32.Parse(oIPAddresses.Get(intIPAddressBackup, "networkid"));
                    int intIPVlan = Int32.Parse(oIPAddresses.GetNetwork(intIPNetwork, "vlanid"));
                    strBackupVLAN = oIPAddresses.GetVlan(intIPVlan, "vlan");
                    oLog.AddEvent(intAnswer, strName, strSerial, "Configuring Backup VLAN = " + strBackupVLAN, LoggingType.Information);

                    if (oDellMode == DellBladeSwitchportMode.Access)
                    {
                        int intSwitchBackup = 0;
                        DataSet dsSwitchBackup = oAsset.GetSwitchports(intAsset, SwitchPortType.Backup);
                        foreach (DataRow drSwitchBackup in dsSwitchBackup.Tables[0].Rows)
                        {
                            if (strError == "")
                            {
                                intSwitchBackup++;
                                string strSwitch = drSwitchBackup["name"].ToString();
                                string strInterface = drSwitchBackup["blade"].ToString() + "/" + drSwitchBackup["port"].ToString();
                                if (oServer.GetSwitchport(intServer, strSwitch, strInterface).Tables[0].Rows.Count == 0)
                                {
                                    strError = Nexus(intAnswer, strName, strSerial, strSwitch, strInterface, oDellMode, strBackupVLAN, strNative, strDescription + "Backup" + (intSwitchBackup > 1 ? intSwitchBackup.ToString() : ""), true, oVariable, oAsset, intAsset);
                                    if (strError == "")
                                        oServer.AddSwitchport(intServer, strSwitch, strInterface);
                                }
                            }
                        }

                        if (intAssetDR > 0)
                        {
                            intSwitchBackup = 0;
                            DataSet dsSwitchBackupDR = oAsset.GetSwitchports(intAssetDR, SwitchPortType.Backup);
                            foreach (DataRow drSwitchBackupDR in dsSwitchBackupDR.Tables[0].Rows)
                            {
                                if (strError == "")
                                {
                                    intSwitchBackup++;
                                    string strSwitch = drSwitchBackupDR["name"].ToString();
                                    string strInterface = drSwitchBackupDR["blade"].ToString() + "/" + drSwitchBackupDR["port"].ToString();
                                    if (oServer.GetSwitchport(intServer, strSwitch, strInterface).Tables[0].Rows.Count == 0)
                                    {
                                        strError = Nexus(intAnswer, strName, strSerial, strSwitch, strInterface, oDellMode, strBackupVLAN, strNative, strDescription + "Backup" + (intSwitchBackup > 1 ? intSwitchBackup.ToString() : ""), true, oVariable, oAsset, intAsset);
                                        if (strError == "")
                                            oServer.AddSwitchport(intServer, strSwitch, strInterface);
                                    }
                                }
                            }
                        }
                    }
                    else
                        oLog.AddEvent(intAnswer, strName, strSerial, "Backup VLAN = " + strBackupVLAN + " was not configured since it is not ACCESS mode enabled", LoggingType.Information);
                }

                // Add the cluster VLAN (Solaris servers have 4 NICs, so the Clustered Network does not have to be configured)
                string strClusterVLAN = "";
                if (oForecast.IsHACluster(intAnswer) == true)
                {
                    if (oClass.IsProd(intClass))
                        strClusterVLAN = "1100";
                    if (oClass.IsQA(intClass))
                        strClusterVLAN = "2100";
                    if (oClass.IsTestDev(intClass))
                        strClusterVLAN = "2600";

                    if (intIPAddressCluster > 0)
                    {
                        int intIPNetwork = Int32.Parse(oIPAddresses.Get(intIPAddressCluster, "networkid"));
                        int intIPVlan = Int32.Parse(oIPAddresses.GetNetwork(intIPNetwork, "vlanid"));
                        strClusterVLAN = oIPAddresses.GetVlan(intIPVlan, "vlan");
                        oLog.AddEvent(intAnswer, strName, strSerial, "Configuring Cluster VLAN = " + strClusterVLAN, LoggingType.Information);
                    }
                    else
                    {
                        oLog.AddEvent(intAnswer, strName, strSerial, "This is cluster, but no private address was assigned...configuring default VLAN", LoggingType.Information);
                    }

                    if (oDellMode == DellBladeSwitchportMode.Access)
                    {
                        int intSwitchCluster = 0;
                        DataSet dsSwitchCluster = oAsset.GetSwitchports(intAsset, SwitchPortType.Cluster);
                        foreach (DataRow drSwitchCluster in dsSwitchCluster.Tables[0].Rows)
                        {
                            if (strError == "")
                            {
                                intSwitchCluster++;
                                string strSwitch = drSwitchCluster["name"].ToString();
                                string strInterface = drSwitchCluster["blade"].ToString() + "/" + drSwitchCluster["port"].ToString();
                                if (oServer.GetSwitchport(intServer, strSwitch, strInterface).Tables[0].Rows.Count == 0)
                                {
                                    strError = Nexus(intAnswer, strName, strSerial, strSwitch, strInterface, oDellMode, strClusterVLAN, strNative, strDescription + "Cluster" + (intSwitchCluster > 1 ? intSwitchCluster.ToString() : ""), true, oVariable, oAsset, intAsset);
                                    if (strError == "")
                                        oServer.AddSwitchport(intServer, strSwitch, strInterface);
                                }
                            }
                        }

                        if (intAssetDR > 0)
                        {
                            intSwitchCluster = 0;
                            DataSet dsSwitchClusterDR = oAsset.GetSwitchports(intAssetDR, SwitchPortType.Backup);
                            foreach (DataRow drSwitchClusterDR in dsSwitchClusterDR.Tables[0].Rows)
                            {
                                if (strError == "")
                                {
                                    intSwitchCluster++;
                                    string strSwitch = drSwitchClusterDR["name"].ToString();
                                    string strInterface = drSwitchClusterDR["blade"].ToString() + "/" + drSwitchClusterDR["port"].ToString();
                                    if (oServer.GetSwitchport(intServer, strSwitch, strInterface).Tables[0].Rows.Count == 0)
                                    {
                                        strError = Nexus(intAnswer, strName, strSerial, strSwitch, strInterface, oDellMode, strClusterVLAN, strNative, strDescription + "Cluster" + (intSwitchCluster > 1 ? intSwitchCluster.ToString() : ""), true, oVariable, oAsset, intAsset);
                                        if (strError == "")
                                            oServer.AddSwitchport(intServer, strSwitch, strInterface);
                                    }
                                }
                            }
                        }
                    }
                    else
                        oLog.AddEvent(intAnswer, strName, strSerial, "Cluster VLAN = " + strClusterVLAN + " was not configured since it is not ACCESS mode enabled", LoggingType.Information);
                }

                // Windows and Linux are all ACCESS mode.
                foreach (DataRow drSwitch in dsSwitch.Tables[0].Rows)
                {
                    if (drSwitch["blade"].ToString() != "")
                    {
                        if (strSwitchA == "")
                        {
                            strSwitchA = drSwitch["name"].ToString();
                            strInterfaceA = drSwitch["blade"].ToString() + "/" + drSwitch["port"].ToString();
                        }
                        else
                        {
                            strSwitchB = drSwitch["name"].ToString();
                            strInterfaceB = drSwitch["blade"].ToString() + "/" + drSwitch["port"].ToString();
                        }
                    }
                    else
                    {
                        strError = "The BLADE field of the switch was not defined...possibly a placeholder for physical relocation / configuration";
                        break;
                    }
                }

                if (oDellMode == DellBladeSwitchportMode.Trunk)
                {
                    // Since it is trunked, add any backup and cluster VLANs to the primary (and secondary) NICs
                    if (strBackupVLAN != "")
                    {
                        strDellVLAN += ",";
                        strDellVLAN += strBackupVLAN;
                        oLog.AddEvent(intAnswer, strName, strSerial, "Backup VLAN = " + strBackupVLAN + " will be added to the primary trunk", LoggingType.Information);
                    }
                    if (strClusterVLAN != "")
                    {
                        strDellVLAN += ",";
                        strDellVLAN += strClusterVLAN;
                        oLog.AddEvent(intAnswer, strName, strSerial, "Cluster VLAN = " + strClusterVLAN + " will be added to the primary trunk", LoggingType.Information);
                    }
                }

                if (strError == "" && strSwitchA != "" && strInterfaceA != "")
                {
                    if (oServer.GetSwitchport(intServer, strSwitchA, strInterfaceA).Tables[0].Rows.Count == 0)
                    {
                        strError = Nexus(intAnswer, strName, strSerial, strSwitchA, strInterfaceA, oDellMode, strDellVLAN, strNative, strDescription + "pri", true, oVariable, oAsset, intAsset);
                        if (strError == "")
                            oServer.AddSwitchport(intServer, strSwitchA, strInterfaceA);
                    }
                }

                if (strError == "" && strSwitchB != "" && strInterfaceB != "")
                {
                    if (oServer.GetSwitchport(intServer, strSwitchB, strInterfaceB).Tables[0].Rows.Count == 0)
                    {
                        strError = Nexus(intAnswer, strName, strSerial, strSwitchB, strInterfaceB, oDellMode, strDellVLAN, strNative, strDescription + "sec", true, oVariable, oAsset, intAsset);
                        if (strError == "")
                            oServer.AddSwitchport(intServer, strSwitchB, strInterfaceB);
                    }
                }

                if (intAssetDR > 0)
                {
                    DataSet dsSwitchDR = oAsset.GetSwitchports(intAssetDR, SwitchPortType.Network);
                    foreach (DataRow drSwitchDR in dsSwitchDR.Tables[0].Rows)
                    {
                        if (drSwitchDR["blade"].ToString() != "")
                        {
                            if (strSwitchAdr == "")
                            {
                                strSwitchAdr = drSwitchDR["name"].ToString();
                                strInterfaceAdr = drSwitchDR["blade"].ToString() + "/" + drSwitchDR["port"].ToString();
                            }
                            else
                            {
                                strSwitchBdr = drSwitchDR["name"].ToString();
                                strInterfaceBdr = drSwitchDR["blade"].ToString() + "/" + drSwitchDR["port"].ToString();
                            }
                        }
                        else
                        {
                            strError = "The BLADE field of the switch was not defined...possibly a placeholder for physical relocation / configuration ~ DR";
                            break;
                        }
                    }

                    if (strError == "" && strSwitchAdr != "" && strInterfaceAdr != "")
                    {
                        if (oServer.GetSwitchport(intServer, strSwitchAdr, strInterfaceAdr).Tables[0].Rows.Count == 0)
                        {
                            strError = Nexus(intAnswer, strName, strSerial, strSwitchAdr, strInterfaceAdr, oDellMode, strDellVLAN, strNative, strDescription + "pri", true, oVariable, oAsset, intAsset);
                            if (strError == "")
                                oServer.AddSwitchport(intServer, strSwitchAdr, strInterfaceAdr);
                        }
                    }

                    if (strError == "" && strSwitchBdr != "" && strInterfaceBdr != "")
                    {
                        if (oServer.GetSwitchport(intServer, strSwitchBdr, strInterfaceBdr).Tables[0].Rows.Count == 0)
                        {
                            strError = Nexus(intAnswer, strName, strSerial, strSwitchBdr, strInterfaceBdr, oDellMode, strDellVLAN, strNative, strDescription + "sec", true, oVariable, oAsset, intAsset);
                            if (strError == "")
                                oServer.AddSwitchport(intServer, strSwitchBdr, strInterfaceBdr);
                        }
                    }

                }
            }
            if (strError == "")
                oLog.AddEvent(intAnswer, strName, strSerial, "Nexus configuration completed successfully!", LoggingType.Information);
            return strError;
        }

        private string Nexus(int _answerid, string _name, string _serial, string _switch, string _interface, DellBladeSwitchportMode _mode, string _vlan, string _native, string _description, bool _override_connected, Variables _variable, Asset _asset, int _assetid)
        {
            /*
            string strError = "";
            Ping oPing = new Ping();
            string strPingStatus = "";
            try
            {
                PingReply oReply = oPing.Send(_switch);
                strPingStatus = oReply.Status.ToString().ToUpper();
            }
            catch { }
            if (strPingStatus == "SUCCESS")
            {
                // Switch the port of strSwitchA, strInterfaceA
                SshShell oSSHshell = new SshShell(_switch, _variable.NexusUsername(), _variable.NexusPassword());
                oSSHshell.RemoveTerminalEmulationCharacters = true;
                oSSHshell.Connect();
                string strLogin = _asset.GetDellSwitchportOutput(oSSHshell);
                if (strLogin != "**INVALID**")
                {
                    oLog.AddEvent(_answerid, _name, _serial, "Successfully logged into Switch (" + _switch + ")...Setting " + (_mode == DellBladeSwitchportMode.Trunk ? "TRUNK" : "ACCESS") + " Switchport (" + _interface + ") to " + _vlan, LoggingType.Information);
                    string strResult = _asset.ChangeDellSwitchport(oSSHshell, _interface, _mode, _vlan, _native, _description, _override_connected, _assetid);
                    if (strResult == "")
                    {
                        oLog.AddEvent(_answerid, _name, _serial, "Successfully changed switchport " + _interface + " on " + _switch, LoggingType.Information);
                        oSSHshell.Close();
                        // Done Configuring Switchports
                    }
                    else
                    {
                        strError = "There was a problem configuring the Dell Blade Switchport  ~ Switch: " + _switch + ", Interface: " + _interface + ", Error: " + strResult;
                        oLog.AddEvent(_answerid, _name, _serial, strError, LoggingType.Error);
                    }
                }
                else
                {
                    strError = "There was a problem logging into the Dell Blade Switch  ~ Switch: " + _switch;
                    oLog.AddEvent(_answerid, _name, _serial, strError, LoggingType.Error);
                }
            }
            else
            {
                strError = "There was a problem pinging the Dell Blade Switch  ~ Switch: " + _switch + ", Status: " + strPingStatus;
                oLog.AddEvent(_answerid, _name, _serial, strError, LoggingType.Error);
            }
            return strError;
            */

            oLog.AddEvent(_answerid, _name, _serial, "Starting Nexus Thread...", LoggingType.Debug);
            NexusThread oNexus = new NexusThread(oLog, _answerid, _name, _serial, _switch, _interface, _mode, _vlan, _native, _description, _override_connected, _variable, _asset, _assetid);
            ThreadStart oNexusStart = new ThreadStart(oNexus.Begin);
            Thread oNexusThread = new Thread(oNexusStart);
            oNexusThread.Start();

            TimeSpan oNexusSpan = DateTime.Now.Subtract(oNexus.Started);
            //oLog.AddEvent(_answerid, _name, _serial, "oNexus.Started = " + oNexus.Started.ToString(), LoggingType.Debug);
            string strError = oNexus.Error;
            if (strError != "")
                strError = "";
            //oLog.AddEvent(_answerid, _name, _serial, "oNexusSpan.TotalSeconds = " + oNexusSpan.TotalSeconds.ToString(), LoggingType.Debug);
            //oLog.AddEvent(_answerid, _name, _serial, "oNexusSpan.TotalSeconds < 90 = " + (oNexusSpan.TotalSeconds < 90.00), LoggingType.Debug);
            while (oNexusSpan.TotalSeconds < 90.00 && oNexus.Complete == false && strError == "")
            {
                // Wait for it to complete
                Thread.Sleep(1000);
                oNexusSpan = DateTime.Now.Subtract(oNexus.Started);
                if (Int32.Parse(oNexusSpan.TotalSeconds.ToString("0")) % 10 == 0)
                    oLog.AddEvent(_answerid, _name, _serial, "...still waiting for nexus..." + oNexusSpan.TotalSeconds.ToString("0") + "...", LoggingType.Debug);
                strError = oNexus.Error;
            }

            if (strError != "")
                return strError;
            else if (oNexus.Complete == false)
            {
                if (oNexus.oSSHshell != null && oNexus.oSSHshell.ShellConnected == true && oNexus.oSSHshell.ShellOpened == true)
                {
                    oLog.AddEvent(_answerid, _name, _serial, "SSH Connection has been closed", LoggingType.Debug);
                    oNexus.oSSHshell.Close();
                }
                return "A timeout has occured when attempting to configure the switch ~ Switch (" + _switch + ")...Setting " + (_mode == DellBladeSwitchportMode.Trunk ? "TRUNK" : "ACCESS") + " Switchport (" + _interface + ") to " + _vlan;
            }
            else
                return "";
        }
    }
}
