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
using Microsoft.VirtualServer.Interop;
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
using NCC.ClearView.Application.Core.w08r2;
using System.Collections;
using Vim25Api;
using System.Xml;

namespace ClearViewAP_VMware
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
        private string strScripts = "E:\\APPS\\CLV\\ClearViewAP_VMware\\";
        private string strSub = "scripts\\";
        private string strZeus = "";
        private string strVSG = "";
        private string strRAM = "";
        private string strCPU = "";
        private string strHA = "";
        private string strDomainsList = "";
        private string strDomainDefault = "";
        //private int intImplementorDistributed = 0;
        //private int intImplementorMidrange = 0;
        private int intWorkstationPlatform = 0;
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
        private double dblDefault = 10.00;
        private int intBuildTest = 0;
        private int intBuildQA = 0;
        private int intBuildQAOffHours = 0;
        private int intBuildProd = 0;
        private int intBuildProdOffHours = 0;
        private bool boolUsePNCNaming = true;
        private int intDesignOverride = 0;
        private int intLastStep = 24;
        private double dblBuffer = 0.00;
        private int intTypeVirtual = 0;
        private bool boolSkipVirtual = false;
        private bool boolVirtualCOM = false;
        private bool boolNotifyDecom = false;
        private string strDSMADMC = "";
        private bool boolMultiThreadedAudit = false;
        private double dblDriveSizeNotify = 0.00;
        private string strDriveSizeNotify = "";
        private bool boolOverrideDesign = false;
        private double dblOverrideDriveDefault = 0.00;
        private int intOverrideCpuDefault = 0;
        private int intOverrideRamDefault = 0;
        private bool boolDeleteScriptFiles = false;
        private bool boolVMFound = false;
        private string ScriptEnvironment = "";

        private int intIMDecommServiceId = 0;
        private string strEMailIdsBCC = "";
        private int intErrorServer = 0;
        private int intErrorStep = 0;
        private int intErrorAsset = 0;
        private int intErrorModel = 0;
        private VMWare oErrorVMWare;
        private string strDecomSuffix = "-DECOM";
        private char[] strSplit = { ';' };
        private int intTimeoutDefault = (5 * 60 * 1000);    // 5 minutes
        private int intTimeoutInstall = (30 * 60 * 1000);   // 30 minutes
        private string strAvamarOSs = "";
        private string strAvamarLocations = "";

        private int intStepDatastore = 3;   // Assign Datastore
        private bool boolForceDNSSuccess = false;       // If set to true, will catch any errors caused by DNS registration and set to "Registered"
        private bool boolInitiateDNSRequest = false;    // If set to true, each result of a DNS request which is not a SUCCESS or DUPLICATE will send an error request.
        private int intAuditCounts = 0;     // The number of audits that can be run at one time
        private int intAuditCount = 0;      // The current number of audits running (compared against intAuditCounts)
        public int AuditCount
        {
            get { return intAuditCount; }
            set { intAuditCount = value; }
        }

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
                strRAM = ds.Tables[0].Rows[0]["ram"].ToString();
                strCPU = ds.Tables[0].Rows[0]["cpu"].ToString();
                strVSG = ds.Tables[0].Rows[0]["vsg"].ToString();
                strHA = ds.Tables[0].Rows[0]["ha"].ToString();
                strDomainsList = ds.Tables[0].Rows[0]["domains"].ToString();
                strDomainDefault = ds.Tables[0].Rows[0]["domain_default"].ToString();
                intWorkstationPlatform = Int32.Parse(ds.Tables[0].Rows[0]["workstation_platform"].ToString());
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
                intDesignOverride = Int32.Parse(ds.Tables[0].Rows[0]["design_override"].ToString());
                intLogging = Int32.Parse(ds.Tables[0].Rows[0]["logging"].ToString());
                intIMDecommServiceId = Int32.Parse(ds.Tables[0].Rows[0]["SERVICEID_SERVER_DECOMMISSION_IM"].ToString());
                dblBuffer = double.Parse(ds.Tables[0].Rows[0]["buffer"].ToString());
                intTypeVirtual = Int32.Parse(ds.Tables[0].Rows[0]["TYPEID_VIRTUAL"].ToString());
                boolSkipVirtual = (ds.Tables[0].Rows[0]["SKIP_VIRTUAL"].ToString() == "1");
                boolVirtualCOM = (ds.Tables[0].Rows[0]["COM_VIRTUAL"].ToString() == "1");
                boolNotifyDecom = (ds.Tables[0].Rows[0]["NOTIFY_DECOM"].ToString() == "1");
                strDSMADMC = ds.Tables[0].Rows[0]["DSMADMC"].ToString();
                boolMultiThreadedAudit = (ds.Tables[0].Rows[0]["multi_thread_audit"].ToString() == "1");
                dblDriveSizeNotify = double.Parse(ds.Tables[0].Rows[0]["drive_size"].ToString());
                strDriveSizeNotify = ds.Tables[0].Rows[0]["drive_size_notify"].ToString();
                boolOverrideDesign = (ds.Tables[0].Rows[0]["override_design"].ToString() == "1");
                dblOverrideDriveDefault = double.Parse(ds.Tables[0].Rows[0]["override_drive_default"].ToString());
                intOverrideCpuDefault = Int32.Parse(ds.Tables[0].Rows[0]["override_cpu_default"].ToString());
                intOverrideRamDefault = Int32.Parse(ds.Tables[0].Rows[0]["override_ram_default"].ToString());
                boolDeleteScriptFiles = (ds.Tables[0].Rows[0]["delete_script_files"].ToString() == "1");
                boolForceDNSSuccess = (ds.Tables[0].Rows[0]["force_dns_success"].ToString() == "1");
                boolInitiateDNSRequest = (ds.Tables[0].Rows[0]["initiate_dns_request"].ToString() == "1");
                intAuditCounts = Int32.Parse(ds.Tables[0].Rows[0]["audit_count"].ToString());
                strAvamarOSs = ds.Tables[0].Rows[0]["avamar_os"].ToString();
                strAvamarLocations = ds.Tables[0].Rows[0]["avamar_locations"].ToString();
                ScriptEnvironment = ds.Tables[0].Rows[0]["ScriptEnvironment"].ToString();
                oLog = new Log(0, dsn);
                oTimer = new System.Timers.Timer(dblInterval);
                oTimer.Elapsed += new ElapsedEventHandler(this.ServiceTimer_Tick);
            }
            catch
            {
                EventLog.WriteEntry(String.Format("ClearView AP VMware Service initialization has failed - INVALID XML FILE"), EventLogEntryType.Error);
            }
        }

        protected override void OnStart(string[] args)
        {
            oEventLog.WriteEntry(String.Format("ClearView AP VMware Service started."), EventLogEntryType.Information);
            oTimer.AutoReset = true;
            oTimer.Enabled = true;
            oTimer.Start();
        }

        protected override void OnStop()
        {
            oEventLog.WriteEntry(String.Format("ClearView AP VMware Service stopped."), EventLogEntryType.Information);
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
                oEventLog.WriteEntry(String.Format("ClearView AP VMware Service TICK."), EventLogEntryType.Information);

            // Start Main Processing
            try
            {
                ServiceTick();
                if (intLogging > 2)
                    oEventLog.WriteEntry(String.Format("Finished ServiceTick()."), EventLogEntryType.Information);

                // Start Installations
                ThreadStart oJob = new ThreadStart(InstallTick);
                Thread oThreadJob = new Thread(oJob);
                oThreadJob.Start();
                if (intLogging > 2)
                    oEventLog.WriteEntry(String.Format("Finished InstallTick()."), EventLogEntryType.Information);

                // Start Decommissions
                ServiceTickDecom();
                if (intLogging > 2)
                    oEventLog.WriteEntry(String.Format("Finished ServiceTickDecom()."), EventLogEntryType.Information);

                // Execute MIS Audits
                ExecuteMISAudits();
                if (intLogging > 2)
                    oEventLog.WriteEntry(String.Format("Finished ExecuteMISAudits()."), EventLogEntryType.Information);

                // *******************************************
                // ************  END Processing  *************
                // *******************************************
                oTimer.Start();
            }
            catch (Exception ex)
            {
                string strError = "VMWare Service (TICK): " + "(Error Message: " + ex.Message + ") ~ (Source: " + ex.Source + ") (Stack Trace: " + ex.StackTrace + ") [" + System.Environment.UserName + "]";
                oEventLog.WriteEntry(strError, EventLogEntryType.Error);
            }
        }
        private void ServiceTick()
        {
            VimService _service = new VimService();
            VMWare oVMWare = new VMWare(0, dsn);
            try
            {
                Servers oServer = new Servers(0, dsn);
                DataSet ds = oServer.GetTypes(strServerTypesBuild);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    bool boolProcess = false;
                    bool boolIsOverride = false;
                    oVMWare = new VMWare(0, dsn);
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
                    Storage oStorage = new Storage(0, dsn);
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
                    Design oDesign = new Design(0, dsn);
               
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
                        string strSerial = oAsset.Get(intAsset, "serial").ToUpper();
                        string strAsset = oAsset.Get(intAsset, "asset").ToUpper();
                        int intModel = 0;
                        int intParent = 0;
                        int intType = 0;
                        if (intAsset > 0)
                        {
                            intModel = Int32.Parse(oAsset.Get(intAsset, "modelid"));
                            intErrorModel = intModel;
                            intParent = Int32.Parse(oModelsProperties.Get(intModel, "modelid"));
                            intType = oModelsProperties.GetType(intModel);
                        }
                        int intStepID = 0;
                        string strStep = "N / A";
                        DataSet dsSteps = oOnDemand.GetSteps(intType, 1);
                        if (dsSteps.Tables[0].Rows.Count > 0)
                        {
                            strStep = dsSteps.Tables[0].Rows[intStep - 1]["title"].ToString();
                            Int32.TryParse(dsSteps.Tables[0].Rows[intStep - 1]["id"].ToString(), out intStepID);
                        }

                        int intDNSAuto = Int32.Parse(dr["dns_auto"].ToString());
                        int intClusterID = Int32.Parse(dr["clusterid"].ToString());
                        int intAnswer = Int32.Parse(dr["answerid"].ToString());
                        DataSet dsDesign = oDesign.GetAnswer(intAnswer);
                        int intNumber = Int32.Parse(dr["number"].ToString());
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
                        int intUserExecuted = Int32.Parse(oForecast.GetAnswer(intAnswer, "executed_by"));
                        int intQuantity = Int32.Parse(oForecast.GetAnswer(intAnswer, "quantity"));
                        int intMnemonic = Int32.Parse(oForecast.GetAnswer(intAnswer, "mnemonicid"));
                        int intNetworkHA = Int32.Parse(oForecast.GetAnswer(intAnswer, "ha"));
                        bool boolBIR = (oForecast.GetAnswer(intAnswer, "resiliency") == "1");    // Related to the Business Resiliency Effort (use old data center strategy)
                        int intResiliency = oResiliency.GetIDFromMnemonic(intMnemonic, boolBIR, 0);
                        string strAppCode = oForecast.GetAnswer(intAnswer, "appcode");
                        if (intMnemonic > 0)
                            strAppCode = oMnemonic.Get(intMnemonic, "factory_code");
                        int intClass = Int32.Parse(dr["classid"].ToString());
                        int intEnv = Int32.Parse(dr["environmentid"].ToString());
                        int intHA = Int32.Parse(dr["ha"].ToString());
                        if (intHA == 10)    // means that the server was added by the SERVERS.START() function
                            intEnv = intEnvironmentHA;
                        int intAddress = Int32.Parse(dr["addressid"].ToString());
                        bool boolZeusError = (dr["zeus_error"].ToString() == "1");
                        bool boolPNC = (dr["pnc"].ToString() == "1" || oClass.Get(intClass, "pnc") == "1");

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
                        int intApplication = 0;
                        if (dr["applicationid"].ToString() != "")
                            intApplication = Int32.Parse(dr["applicationid"].ToString());
                        int intSubApplication = 0;
                        if (dr["subapplicationid"].ToString() != "")
                            intSubApplication = Int32.Parse(dr["subapplicationid"].ToString());
                        int intHost2 = Int32.Parse(oForecast.GetAnswer(intAnswer, "hostid"));
                        int intOS = Int32.Parse(dr["osid"].ToString());
                        string strOS = oOperatingSystem.Get(intOS, "name");
                        bool IsWin2012 = strOS.Contains("2012");
                        bool boolOffsite = oLocation.IsOffsite(intAddress);
                        string strName = "";
                        int intServerName = 0;
                        if (dr["nameid"].ToString() != "")
                        {
                            intServerName = Int32.Parse(dr["nameid"].ToString());
                            if (intServerName > 0)
                                strName = oServer.GetName(intServer, boolUsePNCNaming);
                        }
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
                            // All non-TSM builds on VIRTUAL hardware = AVAMAR
                            oForecast.UpdateAnswerAvamar(intAnswer, 1);
                        }
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
                                            boolAvamar = true;
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

                        bool boolRDPAltiris = (oOperatingSystem.Get(intOS, "rdp_altiris") == "1");
                        bool boolRDPMDT = (oOperatingSystem.Get(intOS, "rdp_mdt") == "1");
                        string strBootEnvironment = oOperatingSystem.Get(intOS, "boot_environment");
                        string strTaskSequence = oOperatingSystem.Get(intOS, "task_sequence");
                        int intSP = Int32.Parse(dr["spid"].ToString());
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
                        DataSet dsGuest = oVMWare.GetGuest(strName);
                        int intHost = 0;
                        int intCluster = 0;
                        int intClusterVersion = 0;
                        int intClusterAntiAffinity = 0;
                        int intClusterDell = 0;
                        int intFolder = 0;
                        int intDataCenter = 0;
                        int intVirtualCenter = 0;
                        string strBuildFolder = "";
                        int intClassID = 0;
                        int intEnvironmentID = 0;
                        int intAddressID = 0;
                        int intDatastore = 0;
                        int intDatastoreMax = 0;
                        int intVlan = 0;
                        int intPool = 0;
                        string strMACAddress = "";
                        string strMACAddress2 = "";
                        string strVLAN = "";
                        if (dsGuest.Tables[0].Rows.Count > 0)
                        {
                            intHost = Int32.Parse(dsGuest.Tables[0].Rows[0]["hostid"].ToString());
                            intCluster = Int32.Parse(oVMWare.GetHost(intHost, "clusterid"));
                            if (intCluster > 0)
                            {
                                Int32.TryParse(oVMWare.GetCluster(intCluster, "version"), out intClusterVersion);
                                Int32.TryParse(oVMWare.GetCluster(intCluster, "anti_affinity"), out intClusterAntiAffinity);
                                Int32.TryParse(oVMWare.GetCluster(intCluster, "dell"), out intClusterDell);
                            }
                            intFolder = Int32.Parse(oVMWare.GetCluster(intCluster, "folderid"));
                            intDataCenter = Int32.Parse(oVMWare.GetFolder(intFolder, "datacenterid"));
                            if (intDataCenter > 0)
                            {
                                strBuildFolder = oVMWare.GetDatacenter(intDataCenter, "build_folder");
                                intVirtualCenter = Int32.Parse(oVMWare.GetDatacenter(intDataCenter, "virtualcenterid"));
                            }
                            intClassID = Int32.Parse(dsGuest.Tables[0].Rows[0]["classid"].ToString());
                            intEnvironmentID = Int32.Parse(dsGuest.Tables[0].Rows[0]["environmentid"].ToString());
                            intAddressID = Int32.Parse(dsGuest.Tables[0].Rows[0]["addressid"].ToString());
                            intDatastore = Int32.Parse(dsGuest.Tables[0].Rows[0]["datastoreid"].ToString());
                            Int32.TryParse(oVMWare.GetDatastore(intDatastore, "maximum"), out intDatastoreMax);
                            intVlan = Int32.Parse(dsGuest.Tables[0].Rows[0]["vlanid"].ToString());
                            intPool = Int32.Parse(dsGuest.Tables[0].Rows[0]["poolid"].ToString());
                            strMACAddress = dsGuest.Tables[0].Rows[0]["macaddress"].ToString();
                            strVLAN = oVMWare.GetVlan(intVlan, "name");
                        }
                        if (intClusterDell == 1)
                        {
                            // Windows 2008, Windows 2003 and RHEL on DELLs have to go through the same build VLAN (920).
                            // So, they will have to be kicked off by Windows Deployment Toolkit.
                            boolRDPMDT = true;
                            boolRDPAltiris = false;
                        }
                        DataSet dsBuild = oBuildLocation.GetRDPs(intClassID, intEnvironmentID, intAddressID, intResiliency, (boolRDPAltiris ? 1 : 0), (boolRDPMDT ? 1 : 0), 0, 0);
                        // WORKFLOWFIX
                        string strConnect = oVMWare.Connect(strName);
                        //string strConnect = oVMWare.ConnectDEBUG("https://OHCLEUTL4001/sdk", 3, (boolPNC ? "Dalton" : "Cleveland Ops"));
                        oErrorVMWare = oVMWare;
                        _service = oVMWare.GetService();
                        int intIPAddress = 0;
                        int intIPAddressID = 0;
                        DataSet dsIPBuild = oServer.GetIP(intServer, 1, 0, 0, 0);
                        if (dsIPBuild.Tables[0].Rows.Count > 0)
                        {
                            intIPAddress = Int32.Parse(dsIPBuild.Tables[0].Rows[0]["ipaddressid"].ToString());
                            intIPAddressID = Int32.Parse(dsIPBuild.Tables[0].Rows[0]["id"].ToString());
                        }
                        int intIPAddressFinal = 0;
                        DataSet dsIPFinal = oServer.GetIP(intServer, 0, 1, 0, 0);
                        if (dsIPFinal.Tables[0].Rows.Count > 0)
                            intIPAddressFinal = Int32.Parse(dsIPFinal.Tables[0].Rows[0]["ipaddressid"].ToString());
                        int intIPAddressAvamar = 0;
                        //DataSet dsIPAvamar = oServer.GetIP(intServer, 0, 0, 0, 1);
                        //if (dsIPAvamar.Tables[0].Rows.Count > 0)
                        //    intIPAddressAvamar = Int32.Parse(dsIPAvamar.Tables[0].Rows[0]["ipaddressid"].ToString());
                        int intIPAddressCluster = 0;
                        int intIPAddressIDCluster = 0;
                        DataSet dsIPCluster = oServer.GetIP(intServer, 0, 0, 1, 0);
                        if (dsIPCluster.Tables[0].Rows.Count > 0)
                        {
                            intIPAddressCluster = Int32.Parse(dsIPCluster.Tables[0].Rows[0]["ipaddressid"].ToString());
                            intIPAddressIDCluster = Int32.Parse(dsIPCluster.Tables[0].Rows[0]["id"].ToString());
                        }

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
                        bool boolIsSQL = false;
                        bool boolIsOracle = false;
                        foreach (DataRow drComponent in dsComponents.Tables[0].Rows)
                        {
                            if (drComponent["sql"].ToString() == "1")
                                boolIsSQL = true;
                            else if (drComponent["zeus_code"].ToString().ToUpper() == "ORACLE")
                                boolIsOracle = true;
                        }
                        bool boolIsClustering = oForecast.IsHACluster(intAnswer);

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

                        int intImplementorUser = 0;
                        string strImplementor = "";
                        DataSet dsImplementor = oOnDemandTasks.GetPending(intAnswer);
                        if (dsImplementor.Tables[0].Rows.Count > 0)
                        {
                            intImplementorUser = Int32.Parse(dsImplementor.Tables[0].Rows[0]["resourceid"].ToString());
                            intImplementorUser = Int32.Parse(oResourceRequest.GetWorkflow(intImplementorUser, "userid"));
                            strImplementor = oUser.GetName(intImplementorUser);
                        }

                        DataSet dsStorage = oStorage.GetLuns(intAnswer, 0, 0, 0, intNumber);
                        double dblDriveC_Distributed = 24.00;
                        //if (oOperatingSystem.IsWindows2008(intOS) == true || (oOperatingSystem.IsWindows(intOS) == true && intClusterDell == 1))
                        if (boolRDPMDT == true)
                        {
                            // For all Windows 2008 AND All Dell Hosted Windows (since WABE is building)
                            if (oOperatingSystem.IsWindows2008(intOS))
                            {
                                //dblDriveC_Distributed = 45.00;  // 4/9/12 - changed to 45 GB per Murick
                                //dblDriveC_Distributed = 50.00;  // 9/5/13 - changed to 50 GB per Murick (CVT111950)
                                dblDriveC_Distributed = 60.00;  // 12/9/13 - changed to 50 GB per Stewart (CVT116096)
                            }
                            else
                                dblDriveC_Distributed = 40.00;
                        }
                        double dblDriveC_Midrange = 25.00;
                        if (boolPNC == true)
                        {
                            if (oEnvironment.Get(intEnv, "ecom") == "1")
                                dblDriveC_Midrange = 80.00;
                            else
                                dblDriveC_Midrange = 80.00;
                        }
                        double dblTest = 0.00;
                        double dblQA = 0.00;
                        double dblProd = 0.00;
                        double dblSize = 0.00;
                        double dblSizeRequested = 0.00;
                        if (dblBuffer == 0.00)
                            dblBuffer = 0.10;   // 10%
                        int intStorageType = 10;
                        int intReplicated = (oClass.IsProd(intClass) ? 1 : 0);
                        foreach (DataRow drStorage in dsStorage.Tables[0].Rows)
                        {
                            if (drStorage["performance"].ToString() == "High")
                                intStorageType = 100;
                            if (drStorage["performance"].ToString() == "Low")
                                intStorageType = 1;
                            if (drStorage["replicated"].ToString() == "1")
                                intReplicated = 1;
                            dblProd += double.Parse(drStorage["size"].ToString());
                            dblQA += double.Parse(drStorage["size_qa"].ToString());
                            dblTest += double.Parse(drStorage["size_test"].ToString());
                            DataSet dsPoints = oStorage.GetMountPoints(Int32.Parse(drStorage["id"].ToString()));
                            foreach (DataRow drPoint in dsPoints.Tables[0].Rows)
                            {
                                if (drPoint["performance"].ToString() == "High")
                                    intStorageType = 100;
                                if (drPoint["performance"].ToString() == "Low")
                                    intStorageType = 1;
                                if (drPoint["replicated"].ToString() == "1")
                                    intReplicated = 1;
                                dblProd += double.Parse(drPoint["size"].ToString());
                                dblQA += double.Parse(drPoint["size_qa"].ToString());
                                dblTest += double.Parse(drPoint["size_test"].ToString());
                            }
                        }
                        if (dsDesign.Tables[0].Rows.Count > 0)
                        {
                            // CFI
                            DataRow drDesign = dsDesign.Tables[0].Rows[0];
                            intReplicated = (drDesign["dr"].ToString() == "1" ? 1 : 0);
                        }
                        if (oClass.Get(intClass, "pnc") == "1")
                        {
                            // For PNC, building in TEST or in PROD
                            if (oClass.IsProd(intClass) || oClass.IsDR(intClass))
                            {
                                // If requested test presence, build with that configuration
                                if (dblTest > 0.00)
                                    dblSize = dblTest;
                                else if (dblQA > 0.00)
                                    dblSize = dblQA;
                                else
                                    dblSize = dblProd;
                            }
                            else if (oClass.IsQA(intClass))
                                dblSize = dblQA;
                            else
                                dblSize = dblTest;
                        }
                        else
                        {
                            // For NCB, build only in test
                            dblSize = dblTest;
                        }
                        if (dblSize == 0.00 && (oOperatingSystem.IsWindows(intOS) || oOperatingSystem.IsWindows2008(intOS)))
                            dblSize = dblDefault;

                        // Add a catch for the new requirement by VMware for 20GB appvol for all vmware builds
                        if (boolOverrideDesign == true && dblOverrideDriveDefault < dblSize)
                        {
                            boolIsOverride = true;
                            dblSizeRequested = dblSize;
                            dblSize = dblOverrideDriveDefault;
                        }

                        int intBuildClassProcess = intClassID;
                        if (intBuildClassProcess == 0)
                            intBuildClassProcess = intClass;

                        if (oClass.IsProd(intBuildClassProcess) || oClass.IsDR(intBuildClassProcess))
                        {
                            if (intBuildProd == 1)
                            {
                                if (intBuildProdOffHours == 0)
                                {
                                    boolProcess = true;
                                    if (intLogging > 0)
                                        oEventLog.WriteEntry("SERVERID: " + intServer.ToString() + " (VMware): " + "PROD Classes have been enabled for builds", EventLogEntryType.Information);
                                }
                                else if (DateTime.Now.Hour < 7 || DateTime.Now.Hour > 19)
                                {
                                    boolProcess = true;
                                    if (intLogging > 0)
                                        oEventLog.WriteEntry("SERVERID: " + intServer.ToString() + " (VMware): " + "PROD Classes have been enabled for OFF PEAK HOURS (7AM - 7PM) Current Time : " + DateTime.Now.ToShortTimeString(), EventLogEntryType.Information);
                                }
                                else if (intLogging > 0)
                                    oEventLog.WriteEntry("SERVERID: " + intServer.ToString() + " (VMware): " + "PROD / QA Classes are not enabled for DURING PEAK HOURS (7AM - 7PM) builds", EventLogEntryType.Warning);
                            }
                            else if (intLogging > 0)
                                oEventLog.WriteEntry("SERVERID: " + intServer.ToString() + " (VMware): " + "PROD Classes are not enabled for builds", EventLogEntryType.Warning);
                        }
                        if (oClass.IsQA(intBuildClassProcess))
                        {
                            if (intBuildQA == 1)
                            {
                                if (intBuildQAOffHours == 0)
                                {
                                    boolProcess = true;
                                    if (intLogging > 0)
                                        oEventLog.WriteEntry("SERVERID: " + intServer.ToString() + " (VMware): " + "QA Classes have been enabled for builds", EventLogEntryType.Information);
                                }
                                else if (DateTime.Now.Hour < 7 || DateTime.Now.Hour > 19)
                                {
                                    boolProcess = true;
                                    if (intLogging > 0)
                                        oEventLog.WriteEntry("SERVERID: " + intServer.ToString() + " (VMware): " + "QA Classes have been enabled for OFF PEAK HOURS (7AM - 7PM) Current Time : " + DateTime.Now.ToShortTimeString(), EventLogEntryType.Information);
                                }
                                else if (intLogging > 0)
                                    oEventLog.WriteEntry("SERVERID: " + intServer.ToString() + " (VMware): " + "QA Classes are not enabled for DURING PEAK HOURS (7AM - 7PM) builds", EventLogEntryType.Warning);
                            }
                            else if (intLogging > 0)
                                oEventLog.WriteEntry("SERVERID: " + intServer.ToString() + " (VMware): " + "QA Classes are not enabled for builds", EventLogEntryType.Warning);
                        }
                        if (oClass.IsTestDev(intBuildClassProcess))
                        {
                            if (intBuildTest == 1)
                            {
                                boolProcess = true;
                                if (intLogging > 0)
                                    oEventLog.WriteEntry("SERVERID: " + intServer.ToString() + " (VMware): " + "TEST Classes have been enabled for builds", EventLogEntryType.Information);
                            }
                            else if (intLogging > 0)
                                oEventLog.WriteEntry("SERVERID: " + intServer.ToString() + " (VMware): " + "TEST Classes are not enabled for builds", EventLogEntryType.Warning);
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

                        if (boolProcess == true || intStep <= 2 || intStep >= 19 || (intDesignOverride > 0 && intAnswer == intDesignOverride))
                        {
                            // Add Step
                            DataSet dsStepDoneServer = oOnDemand.GetStepDoneServer(intServer, intStep);
                            if (dsStepDoneServer.Tables[0].Rows.Count == 0)
                                oOnDemand.AddStepDoneServer(intServer, intStep, false);

                            bool boolAuditError = false;

                            switch (intStep)
                            {
                                case -100:      // Power off guest
                                    oLog.AddEvent(intAnswer, strName, strSerial, "Starting Step " + intStep.ToString() + " (Power off guest for rebuild)", LoggingType.Information);
                                    ManagedObjectReference _vm_power_rebuild = oVMWare.GetVM(strName);
                                    VirtualMachineRuntimeInfo run_rebuild = (VirtualMachineRuntimeInfo)oVMWare.getObjectProperty(_vm_power_rebuild, "runtime");
                                    if (run_rebuild.powerState != VirtualMachinePowerState.poweredOff)
                                    {
                                        ManagedObjectReference _task_power_rebuild = _service.PowerOffVM_Task(_vm_power_rebuild);
                                        oLog.AddEvent(intAnswer, strName, strSerial, "Guest shutdown Started", LoggingType.Information);
                                        TaskInfo _info_power_rebuild = (TaskInfo)oVMWare.getObjectProperty(_task_power_rebuild, "info");
                                        while (_info_power_rebuild.state == TaskInfoState.running)
                                            _info_power_rebuild = (TaskInfo)oVMWare.getObjectProperty(_task_power_rebuild, "info");
                                        if (_info_power_rebuild.state == TaskInfoState.success)
                                        {
                                            int intAttempt = 0;
                                            for (intAttempt = 0; intAttempt < 20 && run_rebuild.powerState != VirtualMachinePowerState.poweredOff; intAttempt++)
                                            {
                                                run_rebuild = (VirtualMachineRuntimeInfo)oVMWare.getObjectProperty(_vm_power_rebuild, "runtime");
                                                int intAttemptLeft = (20 - intAttempt);
                                                oLog.AddEvent(intAnswer, strName, strSerial, "Server still on...waiting 5 seconds (" + intAttemptLeft.ToString() + " tries left)", LoggingType.Debug);
                                                Thread.Sleep(3000);
                                            }
                                        }
                                    }
                                    else
                                        oLog.AddEvent(intAnswer, strName, strSerial, "Guest OS was already shutdown (" + run_rebuild.powerState.ToString() + ")", LoggingType.Information);

                                    if (run_rebuild.powerState != VirtualMachinePowerState.poweredOff)
                                    {
                                        strError = "There was a problem shutting down the guest for rebuild";
                                        oLog.AddEvent(intAnswer, strName, strSerial, strError, LoggingType.Error);
                                    }
                                    else
                                        strResult = "Guest OS was shutdown";
                                    AddResult(intServer, intStep, intType, strResult, strError);
                                    break;
                                case -101:     // Force PXE Boot
                                    oLog.AddEvent(intAnswer, strName, strSerial, "Starting Step " + intStep.ToString() + " (Force PXE Boot)", LoggingType.Information);
                                    ManagedObjectReference _vm_boot_order = oVMWare.GetVM(strName);
                                    OptionValue val_boot_order = new OptionValue();
                                    val_boot_order.key = "bios.bootDeviceClasses";
                                    // Set the PXE boot
                                    // COMMENT WHEN READY
                                    //val_boot_order.value = "allow:net";
                                    // Remove the PXE boot
                                    val_boot_order.value = "";
                                    VirtualMachineConfigSpec _cs_boot_order = new VirtualMachineConfigSpec();
                                    _cs_boot_order.extraConfig = new OptionValue[] { val_boot_order };
                                    ManagedObjectReference _task_boot_order = _service.ReconfigVM_Task(_vm_boot_order, _cs_boot_order);
                                    TaskInfo _inf_boot_order = (TaskInfo)oVMWare.getObjectProperty(_task_boot_order, "info");
                                    while (_inf_boot_order.state == TaskInfoState.running)
                                        _inf_boot_order = (TaskInfo)oVMWare.getObjectProperty(_task_boot_order, "info");
                                    if (_inf_boot_order.state == TaskInfoState.success)
                                        strResult = "PXE Boot Forced";
                                    else
                                        strError = "PXE Boot NOT Forced";
                                    AddResult(intServer, intStep, intType, strResult, strError);
                                    break;
                                case 1:     // Asset and Server Name 
                                    oLog.AddEvent(intAnswer, strName, strSerial, "Starting Step " + intStep.ToString() + " (Server Name) for VMware serverID " + intServer.ToString(), LoggingType.Information);
                                    oServer.UpdateBuildStarted(intServer, DateTime.Now.ToString());
                                    if (intAsset == 0)
                                    {
                                        intModel = oForecast.GetModel(intAnswer);
                                        intAsset = oAsset.AddGuest(strName, intModel, "VMWARE" + intServer.ToString(), "VMWARE" + intServer.ToString(), (int)AssetStatus.Available, intUser, DateTime.Now, intHost, 8.0, 2.0, 10.0, intClass, intEnv, intAddress, intClass, intEnv);
                                        intErrorAsset = intAsset;
                                        oServer.AddAsset(intServer, intAsset, intClass, intEnv, intRemovable, 0);
                                        // Get model info for naming
                                        intParent = Int32.Parse(oModelsProperties.Get(intModel, "modelid"));
                                        intType = oModelsProperties.GetType(intModel);
                                    }
                                    if (intServerName > 0)
                                    {
                                        strName = oServer.GetName(intServer, boolUsePNCNaming);
                                        strResult = "Server Name: " + strName;
                                        oAsset.AddStatus(intAsset, strName, (int)AssetStatus.InUse, 0, DateTime.Now);
                                    }
                                    else
                                    {
                                        string strAdditionalName = "";
                                        if (boolPNC == true)
                                        {
                                            string _os = oOperatingSystem.Get(intOS, "factory_code");
                                            string _location = oLocation.GetAddress(intAddress, "factory_code");
                                            string _mnemonic = oMnemonic.Get(intMnemonic, "factory_code");
                                            string _environment = oClass.Get(intClass, "factory_code");
                                            // Get Model
                                            if (intParent > 0 && oModel.Get(intParent, "factory_code_specific") != "")
                                                _specific = oModel.Get(intParent, "factory_code_specific");
                                            if (intParent > 0 && oModel.Get(intParent, "factory_code") != "")
                                                _function = oModel.Get(intParent, "factory_code");
                                            // Cluster
                                            if (_specific == "" && boolIsClustering == true)
                                                _specific = "Z";
                                            intServerName = oServerName.AddFactory(_os, _location, _mnemonic, _environment, intClass, intEnv, _function, _specific, 0, "VMWARE" + intServer.ToString(), dsnServiceEditor);
                                            strName = oServerName.GetNameFactory(intServerName, 0);
                                        }
                                        else
                                        {
                                            intType = oAsset.GetType(intAsset, dsn);
                                            string strPrefix = "APP";
                                            if (intHost2 > 0)
                                                strPrefix = oHost.Get(intHost2, "prefix");
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
                                                            DataSet dsInstance = oCluster.GetInstances(intCluster2);
                                                            foreach (DataRow drInstance in dsInstance.Tables[0].Rows)
                                                            {
                                                                string strPrefix2 = strPrefix;
                                                                if (drInstance["sql"].ToString() == "1")
                                                                    strPrefix2 = "SQL";
                                                                strPrefix2 = "C" + strPrefix2.Substring(0, 2);
                                                                strPrefix2 = strPrefix2.ToUpper().Trim();
                                                                int intClusterName = oServerName.Add(intClass, intEnv, intAddress, strPrefix2, 0, oCluster.Get(intCluster2, "name"), 1, dsnServiceEditor);
                                                                oServerName.AddRelated(intAnswer, intCluster2, intClusterName);
                                                                if (strAdditionalName != "")
                                                                    strAdditionalName += ", ";
                                                                strAdditionalName += oServerName.GetName(intClusterName, 0);
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
                                            intServerName = oServerName.Add(intClass, intEnv, intAddress, strPrefix, 0, "VMWARE" + intServer.ToString(), 1, dsnServiceEditor);
                                            strName = oServerName.GetName(intServerName, 0);
                                        }
                                        if (intServerName > 0)
                                        {
                                            strResult = "Server Name: " + strName;
                                            if (strAdditionalName != "")
                                                strResult += "<br/>" + "Instance Name(s): " + strAdditionalName;
                                            oAsset.AddStatus(intAsset, strName, (int)AssetStatus.InUse, 0, DateTime.Now);
                                            oServer.UpdateServerNamed(intServer, intServerName);
                                        }
                                        else
                                            strError = "All available server names are in use for the criteria specified";
                                    }
                                    AddResult(intServer, intStep, intType, strResult, strError);
                                    break;
                                case 2:     // Assign Host
                                    oLog.AddEvent(intAnswer, strName, strSerial, "Starting Step " + intStep.ToString() + " (Assign Host)", LoggingType.Information);
                                    oLog.AddEvent(intAnswer, strName, strSerial, "Querying for host for intServer: " + intServer.ToString() + ", intModel: " + intModel.ToString() + ", boolWindows: " + (oOperatingSystem.IsWindows(intOS) || oOperatingSystem.IsWindows2008(intOS) ? "true" : "false") + ", Resiliency: " + oResiliency.Get(intResiliency, "name") + " (" + intResiliency.ToString() + "), Oracle: " + (boolIsOracle ? "TRUE" : "FALSE") + ", SQL: " + (boolIsSQL ? "TRUE" : "FALSE") + ", Clustering: " + (boolIsClustering ? "TRUE" : "FALSE") + " for name " + strName, LoggingType.Information);
                                    if (intResiliency == 0)
                                        intResiliency = oResiliency.GetIDFromMnemonic(intMnemonic, boolBIR, intAnswer);
                                    string strAssign = oVMWare.AssignHost(strName, intAnswer, intModel, intOS, intResiliency, (boolPNC ? 1 : 0), boolIsOracle, boolIsSQL, boolIsClustering, 0);
                                    oServer.AddOutput(intServer, "HOST", oVMWare.Results());
                                    oVMWare.ClearResults();
                                    if (strAssign == "")
                                    {
                                        if (oVMWare.Cluster() != "")
                                            strResult = "Connected to Cluster " + oVMWare.Cluster() + " in " + oVMWare.DataCenter() + " on " + oVMWare.VirtualCenter();
                                        else
                                            strResult = "Connected to Datacenter " + oVMWare.DataCenter() + " on " + oVMWare.VirtualCenter();
                                    }
                                    else
                                    {
                                        strError = strAssign;
                                        oLog.AddEvent(intAnswer, strName, strSerial, strError, LoggingType.Error);
                                    }
                                    AddResult(intServer, intStep, intType, strResult, strError);
                                    break;
                                case 3:     // Assign Datastore
                                    oLog.AddEvent(intAnswer, strName, strSerial, "Starting Step " + intStep.ToString() + " (Assign Datastore)", LoggingType.Information);

                                    // For Windows ONLY, add a disk (E drive) if not requested.
                                    if (oOperatingSystem.IsWindows2008(intOS) == true && dsStorage.Tables[0].Rows.Count == 0)
                                    {
                                        oStorage.AddLun(intAnswer, 0, 0, 0, intNumber, -1000, 0, 0, 0);
                                        dsStorage = oStorage.GetLuns(intAnswer, 0, 0, 0, intNumber);
                                    }

                                    if (strConnect == "")
                                    {
                                        if (intDatastore > 0)
                                        {
                                            strResult = "Connected to Datastore " + oVMWare.GetDatastore(intDatastore, "name");
                                            oLog.AddEvent(intAnswer, strName, strSerial, strResult, LoggingType.Information);
                                        }
                                        else
                                        {
                                            if (intDatastore < 0)
                                                intDatastore = intDatastore * -1;
                                            oLog.AddEvent(intAnswer, strName, strSerial, "Refreshing host system for cluster " + oVMWare.GetCluster(intCluster, "name"), LoggingType.Information);
                                            string strRefreshStorage2 = oVMWare.RefreshStorage(oVMWare.GetCluster(intCluster, "name"), intCluster);
                                            string strAssign2 = "";
                                            if (strRefreshStorage2 == "")
                                            {
                                                if (oOperatingSystem.IsWindows(intOS) == true || oOperatingSystem.IsWindows2008(intOS) == true)
                                                {
                                                    double dblSizeLog = dblDriveC_Distributed + dblSize;
                                                    oLog.AddEvent(intAnswer, strName, strSerial, "Querying for datastore for strName: " + strName + ", intCluster: " + intCluster.ToString() + ", dblSizeLog: " + dblSizeLog.ToString() + ", Buffer: " + dblBuffer.ToString() + ", intStorageType: " + intStorageType.ToString() + ", intReplicated: " + intReplicated.ToString() + ", boolWorkstation: " + "false" + ", boolPNC: " + boolPNC.ToString() + ", Resiliency: " + oResiliency.Get(intResiliency, "name") + ", Oracle: " + (boolIsOracle ? "TRUE" : "FALSE") + ", SQL: " + (boolIsSQL ? "TRUE" : "FALSE") + ", Clustering: " + (boolIsClustering ? "TRUE" : "FALSE") + ", boolProd: " + (oClass.Get(intClassID, "prod") == "1" ? "true" : "false"), LoggingType.Information);
                                                    strAssign2 = oVMWare.AssignDatastore(strName, intCluster, dblSizeLog, dblBuffer, intStorageType, intReplicated, true, false, false, boolPNC, (oClass.Get(intClassID, "prod") == "1"), intAnswer, intModel, intOS, intResiliency, boolIsOracle, boolIsSQL, boolIsClustering, intDatastore, 0);
                                                }
                                                else if (oOperatingSystem.IsLinux(intOS) == true)
                                                {
                                                    double dblSizeLog = dblDriveC_Midrange + dblSize;
                                                    oLog.AddEvent(intAnswer, strName, strSerial, "Querying for datastore for strName: " + strName + ", intCluster: " + intCluster.ToString() + ", dblSizeLog: " + dblSizeLog.ToString() + ", Buffer: " + dblBuffer.ToString() + ", intStorageType: " + intStorageType.ToString() + ", intReplicated: " + intReplicated.ToString() + ", boolWorkstation: " + "false" + ", boolPNC: " + boolPNC.ToString() + ", Resiliency: " + oResiliency.Get(intResiliency, "name") + ", Oracle: " + (boolIsOracle ? "TRUE" : "FALSE") + ", SQL: " + (boolIsSQL ? "TRUE" : "FALSE") + ", Clustering: " + (boolIsClustering ? "TRUE" : "FALSE") + ", boolProd: " + (oClass.Get(intClassID, "prod") == "1" ? "true" : "false"), LoggingType.Information);
                                                    strAssign2 = oVMWare.AssignDatastore(strName, intCluster, dblSizeLog, dblBuffer, intStorageType, intReplicated, true, false, false, boolPNC, (oClass.Get(intClassID, "prod") == "1"), intAnswer, intModel, intOS, intResiliency, boolIsOracle, boolIsSQL, boolIsClustering, intDatastore, 0);
                                                }
                                                else
                                                {
                                                    strError = "Invalid Operating System for On-Demand ~ (" + strOS + ")";
                                                    oLog.AddEvent(intAnswer, strName, strSerial, strError, LoggingType.Error);
                                                }
                                            }
                                            else
                                            {
                                                strError = "There was a problem refreshing the host system for the cluster ~ " + strRefreshStorage2;
                                                oLog.AddEvent(intAnswer, strName, strSerial, strError, LoggingType.Error);
                                            }
                                            if (strError == "")
                                            {
                                                oServer.AddOutput(intServer, "DATASTORE", oVMWare.Results());
                                                oVMWare.ClearResults();
                                                if (strAssign2 == "")
                                                {
                                                    strResult = "Connected to Datastore " + oVMWare.DataStore;
                                                    oLog.AddEvent(intAnswer, strName, strSerial, strResult, LoggingType.Information);
                                                }
                                                else
                                                {
                                                    strError = strAssign2;
                                                    oLog.AddEvent(intAnswer, strName, strSerial, strError, LoggingType.Error);
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        strError = strConnect;
                                        oLog.AddEvent(intAnswer, strName, strSerial, strError, LoggingType.Error);
                                    }
                                    AddResult(intServer, intStep, intType, strResult, strError);
                                    break;
                                case 4:     // IP Address and notify about datastores (if applicable) 
                                    oLog.AddEvent(intAnswer, strName, strSerial, "Starting Step " + intStep.ToString() + " (Notify Datastores)", LoggingType.Information);
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
                                            strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_INVENTORY_MANAGER");
                                            int intNotifyDifference = (intDatastoreTotal - intDatastoreNotify);     // 8 total - 6 out of space = 2 left
                                            if (intNotifyLeft > intNotifyDifference && strNotifyEmail != "")             // 3 > 2
                                                oFunction.SendEmail("WARNING: VMware Datastore Notification", strNotifyEmail, "", strEMailIdsBCC, "WARNING: VMware Datastore Notification", "<p><b>This message is to inform you that you are running out of datastore space for the cluster " + oVMWare.GetCluster(intCluster, "name") + " (" + oVMWare.DataCenter() + " on " + oVMWare.VirtualCenter() + ")</b></p><p>There are less than " + intNotifyLeft.ToString() + " datastores with no more than " + dblNotifySize.ToString() + " GB each.</p>", true, false);

                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        strError = "Datastore Query Failed with Error Message ~ " + String.Format(ex.Message) + " [" + System.Environment.UserName + "]";
                                        oLog.AddEvent(intAnswer, strName, strSerial, strError, LoggingType.Error);
                                    }

                                    if (strError == "")
                                    {
                                        if (boolOffsite == false)
                                        {
                                            bool boolIPAssignmentRequired = true;
                                            string strIPAddressAssign = "";
                                            // Don't set to 0: otherwise, NCB prod (with no test) will not work.
                                            oLog.AddEvent(intAnswer, strName, strSerial, "Starting Step " + intStep.ToString() + " (IP Address)", LoggingType.Information);
                                            if (intIPAddress == 0)
                                            {
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
                                                    strIPAddressAssign = "(Class = " + intClass.ToString() + ", Env = " + intEnv.ToString() + ", Address = " + intAddress.ToString() + ", NetworkID = " + intClusterNetworkID.ToString() + ", Subnet: " + oIPAddresses.GetNetwork(intClusterNetworkID, "gateway") + ")";
                                                    oLog.AddEvent(intAnswer, strName, strSerial, "Reserving cluster node IP address ~ " + strIPAddressAssign, LoggingType.Information);
                                                    intIPAddress = oIPAddresses.Get_Network(intClassID, intEnvironmentID, intAddressID, 0, 0, intClusterNetworkID, true, intEnvironment, dsnServiceEditor);
                                                }
                                                else
                                                {
                                                    // Get Components
                                                    bool boolWeb = false;
                                                    int intDellWeb = 0;
                                                    int intDellMiddleware = 0;
                                                    int intDellDatabase = 0;
                                                    int intDellFile = 0;
                                                    int intDellMisc = 0;
                                                    int intDellUnder48 = 0;
                                                    if (intClusterDell == 1)
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


                                                    if (oClass.IsProd(intClass) || oClass.IsQA(intClass))
                                                    {
                                                        // Generate Production IP Address
                                                        if (oOperatingSystem.IsWindows(intOS) == true || oOperatingSystem.IsWindows2008(intOS) == true)
                                                        {
                                                            strIPAddressAssign = "(Class = " + intClass.ToString() + ", Env = " + intEnv.ToString() + ", Address = " + intAddress.ToString() + ", Type = VMWare Windows, LTM (Web) = " + intLTM_Web.ToString() + ", LTM (App) = " + intLTM_App.ToString() + ", LTM (Middleware) = " + intLTM_Middleware.ToString() + ", Web = " + intDellWeb.ToString() + ", Middleware = " + intDellMiddleware.ToString() + ", Database = " + intDellDatabase.ToString() + ", File = " + intDellFile.ToString() + ", Misc = " + intDellMisc.ToString() + ", Under48 = " + intDellUnder48.ToString() + ", Avamar = 0, ServerID = " + intServer.ToString() + ", VMwareClusterID = " + intCluster.ToString() + ", Resiliency: " + oResiliency.Get(intResiliency, "name") + ")";
                                                            oLog.AddEvent(intAnswer, strName, strSerial, "Assigning IP Address for VMWare " + strIPAddressAssign, LoggingType.Information);
                                                            intIPAddress = oIPAddresses.Get_Dell(intClass, intEnv, intAddress, 1, 0, intLTM_Web, intLTM_App, intLTM_Middleware, 0, 0, intDellWeb, intDellMiddleware, intDellDatabase, intDellFile, intDellMisc, intDellUnder48, 0, true, intServer, intCluster, intResiliency, intEnvironment, dsnServiceEditor);
                                                        }
                                                        else
                                                        {
                                                            strIPAddressAssign = "(Class = " + intClass.ToString() + ", Env = " + intEnv.ToString() + ", Address = " + intAddress.ToString() + ", Type = VMWare Linux, LTM (Web) = " + intLTM_Web.ToString() + ", LTM (App) = " + intLTM_App.ToString() + ", LTM (Middleware) = " + intLTM_Middleware.ToString() + ", Web = " + intDellWeb.ToString() + ", Middleware = " + intDellMiddleware.ToString() + ", Database = " + intDellDatabase.ToString() + ", File = " + intDellFile.ToString() + ", Misc = " + intDellMisc.ToString() + ", Under48 = " + intDellUnder48.ToString() + ", Avamar = 0, ServerID = " + intServer.ToString() + ", VMwareClusterID = " + intCluster.ToString() + ", Resiliency: " + oResiliency.Get(intResiliency, "name") + ")";
                                                            oLog.AddEvent(intAnswer, strName, strSerial, "Assigning IP Address for VMWare " + strIPAddressAssign, LoggingType.Information);
                                                            intIPAddress = oIPAddresses.Get_Dell(intClass, intEnv, intAddress, 0, 1, intLTM_Web, intLTM_App, intLTM_Middleware, 0, 0, intDellWeb, intDellMiddleware, intDellDatabase, intDellFile, intDellMisc, intDellUnder48, 0, true, intServer, intCluster, intResiliency, intEnvironment, dsnServiceEditor);
                                                        }

                                                        // Log IP Address Query
                                                        oServer.AddOutput(intServer, "IP_ASSIGN", oIPAddresses.Results());
                                                        oIPAddresses.ClearResults();
                                                    }
                                                    else
                                                    {
                                                        // Generate Test IP Address (and set auto_assign to 1)
                                                        if (oOperatingSystem.IsWindows(intOS) == true || oOperatingSystem.IsWindows2008(intOS) == true)
                                                        {
                                                            strIPAddressAssign = "(Class = " + intClassID.ToString() + ", Env = " + intEnvironmentID.ToString() + ", Address = " + intAddressID.ToString() + ", Type = VMWare Windows, LTM (Web) = " + intLTM_Web.ToString() + ", LTM (App) = " + intLTM_App.ToString() + ", LTM (Middleware) = " + intLTM_Middleware.ToString() + ", Web = " + intDellWeb.ToString() + ", Middleware = " + intDellMiddleware.ToString() + ", Database = " + intDellDatabase.ToString() + ", File = " + intDellFile.ToString() + ", Misc = " + intDellMisc.ToString() + ", Under48 = " + intDellUnder48.ToString() + ", Avamar = 0, ServerID = " + intServer.ToString() + ", VMwareClusterID = " + intCluster.ToString() + ", Resiliency: " + oResiliency.Get(intResiliency, "name") + ")";
                                                            oLog.AddEvent(intAnswer, strName, strSerial, "Assigning IP Address for VMWare " + strIPAddressAssign, LoggingType.Information);
                                                            intIPAddress = oIPAddresses.Get_Dell(intClassID, intEnvironmentID, intAddressID, 1, 0, intLTM_Web, intLTM_App, intLTM_Middleware, 0, 0, intDellWeb, intDellMiddleware, intDellDatabase, intDellFile, intDellMisc, intDellUnder48, 0, true, intServer, intCluster, intResiliency, intEnvironment, dsnServiceEditor);
                                                        }
                                                        else
                                                        {
                                                            strIPAddressAssign = "(Class = " + intClassID.ToString() + ", Env = " + intEnvironmentID.ToString() + ", Address = " + intAddressID.ToString() + ", Type = VMWare Linux, LTM (Web) = " + intLTM_Web.ToString() + ", LTM (App) = " + intLTM_App.ToString() + ", LTM (Middleware) = " + intLTM_Middleware.ToString() + ", Web = " + intDellWeb.ToString() + ", Middleware = " + intDellMiddleware.ToString() + ", Database = " + intDellDatabase.ToString() + ", File = " + intDellFile.ToString() + ", Misc = " + intDellMisc.ToString() + ", Under48 = " + intDellUnder48.ToString() + ", Avamar = 0, ServerID = " + intServer.ToString() + ", VMwareClusterID = " + intCluster.ToString() + ", Resiliency: " + oResiliency.Get(intResiliency, "name") + ")";
                                                            oLog.AddEvent(intAnswer, strName, strSerial, "Assigning IP Address for VMWare " + strIPAddressAssign, LoggingType.Information);
                                                            intIPAddress = oIPAddresses.Get_Dell(intClassID, intEnvironmentID, intAddressID, 0, 1, intLTM_Web, intLTM_App, intLTM_Middleware, 0, 0, intDellWeb, intDellMiddleware, intDellDatabase, intDellFile, intDellMisc, intDellUnder48, 0, true, intServer, intCluster, intResiliency, intEnvironment, dsnServiceEditor);
                                                        }
                                                    }
                                                }
                                                // Log IP Address Query
                                                oServer.AddOutput(intServer, "IP_ASSIGN", oIPAddresses.Results());
                                                oIPAddresses.ClearResults();
                                            }
                                            if (intIPAddress > 0)
                                            {
                                                //if (intTSM == 0)
                                                //{
                                                //    // Need to assign an additional IP address for the AVAMAR NIC
                                                //    strIPAddressAssign = "(Class = " + intClassID.ToString() + ", Env = " + intEnvironmentID.ToString() + ", Address = " + intAddressID.ToString() + ", Type = VMWare Windows, LTM (Web) = 0, LTM (App) = 0, LTM (Middleware) = 0, Web = 0, Middleware = 0, Database = 0, File = 0, Misc = 0, Under48 = 0, Avamar = 1, ServerID = " + intServer.ToString() + ", VMwareClusterID = " + intCluster.ToString() + ", Resiliency: " + oResiliency.Get(intResiliency, "name") + ")";
                                                //    oLog.AddEvent(intAnswer, strName, strSerial, "Assigning IP Address (Avamar) for VMWare " + strIPAddressAssign, LoggingType.Information);
                                                //    intIPAddressAvamar = oIPAddresses.Get_Dell(intClassID, intEnvironmentID, intAddressID, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, true, intServer, intCluster, intResiliency, intEnvironment, dsnServiceEditor);
                                                //    // Log IP Address Query
                                                //    oServer.AddOutput(intServer, "IP_ASSIGN_BACKUP", oIPAddresses.Results());
                                                //    oIPAddresses.ClearResults();

                                                //    if (intIPAddressAvamar > 0)
                                                //    {
                                                //        oServer.AddIP(intServer, intIPAddressAvamar, 0, 0, 0, 1);
                                                //        strResult += "IP Address (Avamar): " + oIPAddresses.GetName(intIPAddressAvamar, 0) + "<br/>";
                                                //    }
                                                //    else
                                                //    {
                                                //        oIPAddresses.UpdateAvailable(intIPAddress, 1);    // Clear the assigned IP address
                                                //        strError = "All available IP addresses are in use for the criteria specified ~ " + strIPAddressAssign;
                                                //        oLog.AddEvent(intAnswer, strName, strSerial, strError, LoggingType.Error);
                                                //    }
                                                //}

                                                // Assign Private Cluster Address
                                                if (strError == "" && intClusterID > 0)
                                                {
                                                    int intNetworkPrivate = intNetworkHA;
                                                    if (intNetworkPrivate == 0)
                                                    {
                                                        oLog.AddEvent(intAnswer, strName, strSerial, "Assigning Private Network for (Class = " + intClass.ToString() + ", Env = " + intEnv.ToString() + ", Address = " + intAddress.ToString() + ", Type = Windows Cluster)", LoggingType.Information);
                                                        intNetworkPrivate = oIPAddresses.Get_ClusterNetwork(intClass, intEnv, intAddress, 0, 0, 1, 0, intQuantity, 0, true, dsnServiceEditor);
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
                                                        oLog.AddEvent(intAnswer, strName, strSerial, "Network Found (" + strNetworkPrivate + oIPAddresses.GetNetwork(intNetworkPrivate, "min4") + " - " + strNetworkPrivate + oIPAddresses.GetNetwork(intNetworkPrivate, "max4") + ". Assigning Private IP Address (Cluster) for (Class = " + intClass.ToString() + ", Env = " + intEnv.ToString() + ", Address = " + intAddress.ToString() + ", Type = Windows Cluster)", LoggingType.Information);
                                                        intIPAddressCluster = oIPAddresses.Get_Cluster(intClass, intEnv, intAddress, 0, 0, 1, 0, intNetworkPrivate, true, intEnvironment, dsnServiceEditor);
                                                        // Log IP Address Query
                                                        oServer.AddOutput(intServer, "IP_ASSIGN_PRIVATE", oIPAddresses.Results());
                                                        oIPAddresses.ClearResults();

                                                        if (intIPAddressCluster == 0)
                                                        {
                                                            oIPAddresses.UpdateAvailable(intIPAddress, 1);    // Clear the assigned IP address
                                                            if (intIPAddressAvamar > 0)
                                                                oIPAddresses.UpdateAvailable(intIPAddressAvamar, 1);    // Clear the assigned IP address
                                                            strError = "All available IP addresses are in use for the criteria specified ~ Private Cluster";
                                                            oLog.AddEvent(intAnswer, strName, strSerial, strError, LoggingType.Error);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        oIPAddresses.UpdateAvailable(intIPAddress, 1);    // Clear the assigned IP address
                                                        if (intIPAddressAvamar > 0)
                                                            oIPAddresses.UpdateAvailable(intIPAddressAvamar, 1);    // Clear the assigned IP address
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
                                                            int intNetwork = Int32.Parse(oIPAddresses.Get(intIPAddress, "networkid"));
                                                            oLog.AddEvent(intAnswer, strName, strSerial, "Starting cluster IP address assignments using  NetworkID = " + intNetwork.ToString() + ", Subnet: " + oIPAddresses.GetNetwork(intNetwork, "gateway"), LoggingType.Information);
                                                            List<int> ClusterInstanceIDs = new List<int>();
                                                            List<int> ClusterIPsAssigned = new List<int>();
                                                            List<int> ClusterIPsAlready = new List<int>();
                                                            string strIPAddressAssignInstance = "(Class = " + intClass.ToString() + ", Env = " + intEnv.ToString() + ", Address = " + intAddress.ToString() + ", NetworkID = " + intNetwork.ToString() + ", Subnet: " + oIPAddresses.GetNetwork(intNetwork, "gateway") + ")";

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
                                                                    intInstanceIP = oIPAddresses.Get_Network(intClassID, intEnvironmentID, intAddressID, 0, 0, intNetwork, true, intEnvironment, dsnServiceEditor);
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
                                                                    intClusterIP = oIPAddresses.Get_Network(intClassID, intEnvironmentID, intAddressID, 0, 0, intNetwork, true, intEnvironment, dsnServiceEditor);
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
                                                    oServer.AddIP(intServer, intIPAddress, (oLocation.GetAddress(intAddress, "vmware_ipaddress") == "1" ? 1 : 0), 1, 0, 0);
                                                    strResult += "IP Address: " + oIPAddresses.GetName(intIPAddress, 0) + "<br/>";
                                                    oLog.AddEvent(intAnswer, strName, strSerial, "IP Address:" + oIPAddresses.GetName(intIPAddress, 0), LoggingType.Information);
                                                    if (intIPAddressCluster > 0)
                                                    {
                                                        oServer.AddIP(intServer, intIPAddressCluster, 0, 0, 1, 0);
                                                        strResult += "IP Address (Private): " + oIPAddresses.GetName(intIPAddressCluster, 0) + "<br/>";
                                                        oLog.AddEvent(intAnswer, strName, strSerial, "IP Address (Private):" + oIPAddresses.GetName(intIPAddressCluster, 0), LoggingType.Information);
                                                    }
                                                    if (intIPAddress > 0)
                                                    {
                                                        int intNetwork = Int32.Parse(oIPAddresses.Get(intIPAddress, "networkid"));
                                                        if (intNetwork > 0)
                                                        {
                                                            int intVLAN = Int32.Parse(oIPAddresses.GetNetwork(intNetwork, "vlanid"));
                                                            if (intVLAN > 0)
                                                            {
                                                                DataSet dsVlan = oVMWare.GetVlanAssociations(intVLAN, intCluster);
                                                                if (dsVlan.Tables[0].Rows.Count > 0)
                                                                {
                                                                    int intVMWareVLAN = Int32.Parse(dsVlan.Tables[0].Rows[0]["vmware_vlanid"].ToString());
                                                                    oVMWare.UpdateGuestVlan(strName, intVMWareVLAN);
                                                                }
                                                                else
                                                                {
                                                                    strError = "There are no VMware associations ~ VLAN " + oIPAddresses.GetVlan(intVLAN, "vlan") + ", VLANID = " + intVLAN.ToString() + ", ClusterID = " + intCluster.ToString();
                                                                    oLog.AddEvent(intAnswer, strName, strSerial, strError, LoggingType.Error);
                                                                }
                                                            }
                                                            else
                                                            {
                                                                strError = "Invalid VLAN";
                                                                oLog.AddEvent(intAnswer, strName, strSerial, strError, LoggingType.Error);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            strError = "Invalid Network";
                                                            oLog.AddEvent(intAnswer, strName, strSerial, strError, LoggingType.Error);
                                                        }
                                                    }
                                                }
                                            }
                                            else if (boolIPAssignmentRequired == true)
                                            {
                                                strError = "All available IP addresses are in use for the criteria specified ~ " + strIPAddressAssign;
                                                oLog.AddEvent(intAnswer, strName, strSerial, strError, LoggingType.Error);
                                            }
                                        }
                                        else
                                            strResult = "IP Addressing is not available for this location";
                                    }
                                    AddResult(intServer, intStep, intType, strResult, strError);
                                    break;
                                case 5:     // Configure Active Directory Accounts
                                    oLog.AddEvent(intAnswer, strName, strSerial, "Starting Step " + intStep.ToString() + " (Configure Active Directory Accounts)", LoggingType.Information);
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
                                        strResult = "This step is only available for distributed servers";
                                    }
                                    AddResult(intServer, intStep, intType, strResult, strError);
                                    break;
                                case 6:     // Create ADM Active Directory Group 
                                    oLog.AddEvent(intAnswer, strName, strSerial, "Starting Step " + intStep.ToString() + " (ADM Group)", LoggingType.Information);
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

                                                            //if (boolInstanceSQL == true)
                                                            //{
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
                                                                    string strClusterInstance = strMnemonicCode + strClusterClass + "CSQ" + "A" + strClusterCounter;
                                                                    if (boolInstanceSQL == false)
                                                                        strClusterInstance = strClusterName + strClusterCounter;
                                                                    if (oAD.Search(strClusterInstance, true) == null)
                                                                    {
                                                                        intInstancesInARow++;
                                                                        oLog.AddEvent(intAnswer, strName, strSerial, "The instance name " + strClusterInstance + " was NOT found. (" + intInstancesInARow.ToString() + " of " + intInstances.ToString() + " in a row.)", LoggingType.Debug);
                                                                        if (intClusterCounterStart == 0)
                                                                            intClusterCounterStart = ii;
                                                                    }
                                                                    else
                                                                    {
                                                                        oLog.AddEvent(intAnswer, strName, strSerial, "The instance name " + strClusterInstance + " was found. (resetting consecutive count)", LoggingType.Debug);
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
                                                                        // Create the Instance Name
                                                                        for (int ii = intClusterCounterStart; ii < 100; ii++)
                                                                        {
                                                                            string strClusterCounter = ii.ToString();
                                                                            if (strClusterCounter.Length == 1)
                                                                                strClusterCounter = "0" + strClusterCounter;
                                                                            string strClusterInstance = strMnemonicCode + strClusterClass + "CSQ" + "A" + strClusterCounter;
                                                                            if (boolInstanceSQL == false)
                                                                                strClusterInstance = strClusterName + strClusterCounter;
                                                                            if (strClusterInstanceName != "")
                                                                                strClusterInstance = strClusterInstanceName;
                                                                            oLog.AddEvent(intAnswer, strName, strSerial, "Trying Instance Name object = " + strClusterInstance, LoggingType.Debug);
                                                                            if (oAD.Search(strClusterInstance, true) == null)
                                                                            {
                                                                                string strResultServerObject = oAD.CreateServer(strClusterInstance, strClusterName + " - Instance Name", "Created by ClearView - " + DateTime.Now.ToShortDateString(), "OU=OUs_ClusterVirtual,OU=OUc_Servers,OU=OUc_Computers,");
                                                                                if (strResultServerObject == "")
                                                                                {
                                                                                    string strClusterAD = "The Instance Name object " + strClusterInstance + " was successfully created in " + oVar.Name();
                                                                                    oLog.AddEvent(intAnswer, strName, strSerial, strClusterAD, LoggingType.Information);
                                                                                    strResult += strClusterAD + "<br/>";
                                                                                    // Update the instance name to signify it being done (so other servers won't try to repeat this)
                                                                                    oCluster.UpdateInstanceName(intInstance, strClusterInstance);
                                                                                    if (boolWin2008 == true)
                                                                                    {
                                                                                        // If Windows 2008, join the Cluster Object to the Instance Name
                                                                                        DirectoryEntry oClusterName = oAD.Search(strClusterName, true);         // Cluster object
                                                                                        DirectoryEntry oInstanceName = oAD.Search(strClusterInstance, true); // Instance object
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
                                                                                            strClusterAD = "The Cluster object (" + strClusterName + ") was given FULL ACCESS to the Instance Name object (" + strClusterInstance + ")";
                                                                                            oLog.AddEvent(intAnswer, strName, strSerial, strClusterAD, LoggingType.Information);
                                                                                            strResult += strClusterAD + "<br/>";
                                                                                        }
                                                                                    }
                                                                                }
                                                                                else
                                                                                    strError = "There was a problem creating the Instance Name object ~ " + strClusterInstance + " in " + oVar.Name() + " (" + strResultServerObject + ")";
                                                                                break;
                                                                            }
                                                                            else
                                                                            {
                                                                                string strClusterAD = "The Instance Name object " + strClusterInstance + " already exists in " + oVar.Name();
                                                                                oLog.AddEvent(intAnswer, strName, strSerial, strClusterAD, LoggingType.Debug);
                                                                                if (strClusterInstanceName != "")
                                                                                    break;
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                                else
                                                                    strError = "There was a problem generating the Instance names ~ possibly out of range?";
                                                            //}
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
                                                                                    oLog.AddEvent(intAnswer, strName, strSerial, "The group " + strNameDLG + " was " + (String.IsNullOrEmpty(strError) ? "" : "NOT ") + "created in " + oVar.Name(), LoggingType.Information);
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
                                                                                    oLog.AddEvent(intAnswer, strName, strSerial, "The group " + strNameGG + " was " + (String.IsNullOrEmpty(strError) ? "" : "NOT ") + "created in " + oVar.Name(), LoggingType.Information);
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
                                                                                oLog.AddEvent(intAnswer, strName, strSerial, "The group " + strNameGG + " was " + (String.IsNullOrEmpty(strError) ? "" : "NOT ") + "added to " + strNameDLG + " in " + oVar.Name(), LoggingType.Information);
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
                                                // National City
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
                                        strResult = "Active Directory Configuration Only Available for Distributed Builds";
                                    }
                                    AddResult(intServer, intStep, intType, strResult, strError);
                                    break;
                                case 7:     // Create Machine
                                    oLog.AddEvent(intAnswer, strName, strSerial, "Starting Step " + intStep.ToString() + " (Create Machine)", LoggingType.Information);
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
                                                oLog.AddEvent(intAnswer, strName, strSerial, "There is enough room for machine # " + intMachines.ToString() + " of " + intDatastoreMax.ToString(), LoggingType.Debug);
                                                ManagedObjectReference datacenterRef = oVMWare.GetDataCenter();
                                                ManagedObjectReference vmFolderRef = oVMWare.GetVMFolder(datacenterRef);
                                                ManagedObjectReference clusterRef = oVMWare.GetCluster(oVMWare.GetCluster(intCluster, "name"));
                                                //ManagedObjectReference resourcePoolRootRef = (ManagedObjectReference)oVMWare.getObjectProperty(clusterRef, "resourcePool");
                                                ManagedObjectReference resourcePoolRootRef = oVMWare.GetResourcePool(clusterRef, oVMWare.GetCluster(intCluster, "resource_pool"));
                                                VirtualMachineConfigSpec oConfig = new VirtualMachineConfigSpec();
                                                oConfig.guestId = oOperatingSystem.Get(intOS, "vmware_os");
                                                if (IsWin2012)
                                                {
                                                    // Windows 2012 - set boot firmware to EFI
                                                    oConfig.firmware = "efi";
                                                }

                                                int intRamConfig = 1;
                                                int intRamConfigRequested = 1;
                                                string strRamConfig = "2048";
                                                if (dsDesign.Tables[0].Rows.Count > 0)
                                                {
                                                    // CFI
                                                    DataRow drDesign = dsDesign.Tables[0].Rows[0];
                                                    if (Int32.TryParse(drDesign["ram"].ToString(), out intRamConfig) == true)
                                                    {
                                                        int intRam = intRamConfig * 1024;
                                                        strRamConfig = intRam.ToString();
                                                    }
                                                }
                                                else
                                                {
                                                    string[] strRams;
                                                    strRams = strRAM.Split(strSplit);
                                                    for (int ii = 0; ii < strRams.Length; ii++)
                                                    {
                                                        if (strRams[ii].Trim() != "")
                                                        {
                                                            DataSet dsPlatform = oForecast.GetAnswerPlatform(intAnswer, Int32.Parse(strRams[ii]));
                                                            if (dsPlatform.Tables[0].Rows.Count > 0)
                                                            {
                                                                int intRam = Int32.Parse(dsPlatform.Tables[0].Rows[0]["responseid"].ToString());
                                                                intRam = Int32.Parse(oForecast.GetResponse(intRam, "name"));
                                                                intRamConfig = intRam;
                                                                intRam = intRam * 1024;
                                                                strRamConfig = intRam.ToString();
                                                                break;
                                                            }
                                                        }
                                                    }
                                                }
                                                // why flip flopped
                                                if (boolOverrideDesign == true && intOverrideRamDefault < intRamConfig)
                                                {
                                                    boolIsOverride = true;
                                                    intRamConfigRequested = intRamConfig;
                                                    intRamConfig = intOverrideRamDefault * 1024;
                                                    strRamConfig = intRamConfig.ToString();
                                                }
                                                oConfig.memoryMB = long.Parse(strRamConfig);
                                                oConfig.memoryMBSpecified = true;
                                                int intCpuConfig = 1;
                                                int intCpuConfigRequested = 1;
                                                if (dsDesign.Tables[0].Rows.Count > 0)
                                                {
                                                    // CFI
                                                    DataRow drDesign = dsDesign.Tables[0].Rows[0];
                                                    Int32.TryParse(drDesign["cores"].ToString(), out intCpuConfig);
                                                }
                                                else
                                                {
                                                    string[] strCpus;
                                                    strCpus = strCPU.Split(strSplit);
                                                    for (int jj = 0; jj < strCpus.Length; jj++)
                                                    {
                                                        if (strCpus[jj].Trim() != "")
                                                        {
                                                            DataSet dsPlatform = oForecast.GetAnswerPlatform(intAnswer, Int32.Parse(strCpus[jj]));
                                                            if (dsPlatform.Tables[0].Rows.Count > 0)
                                                            {
                                                                int intCpu = Int32.Parse(dsPlatform.Tables[0].Rows[0]["responseid"].ToString());
                                                                intCpu = Int32.Parse(oForecast.GetResponse(intCpu, "name"));
                                                                intCpuConfig = intCpu;
                                                                break;
                                                            }
                                                        }
                                                    }
                                                }
                                                if (boolOverrideDesign == true && intOverrideCpuDefault < intCpuConfig)
                                                {
                                                    boolIsOverride = true;
                                                    intCpuConfigRequested = intCpuConfig;
                                                    intCpuConfig = intOverrideCpuDefault;
                                                }
                                                oConfig.numCPUs = intCpuConfig;
                                                oConfig.numCPUsSpecified = true;
                                                oConfig.name = strName.ToLower();
                                                oConfig.files = new VirtualMachineFileInfo();
                                                oConfig.files.vmPathName = "[" + oVMWare.GetDatastore(intDatastore, "name") + "] " + strName.ToLower() + "/" + strName.ToLower() + ".vmx";

                                                // Enable HOT ADD for Memory and CPU (Dell Only)
                                                if (intClusterDell == 1)
                                                {
                                                    oConfig.cpuHotAddEnabled = true;
                                                    oConfig.cpuHotAddEnabledSpecified = true;
                                                    oConfig.cpuHotRemoveEnabled = false;
                                                    oConfig.cpuHotRemoveEnabledSpecified = true;
                                                    oConfig.memoryHotAddEnabled = true;
                                                    oConfig.memoryHotAddEnabledSpecified = true;
                                                    oLog.AddEvent(intAnswer, strName, strSerial, "CPU and Memory Hot Add has been enabled", LoggingType.Information);
                                                }

                                                oLog.AddEvent(intAnswer, strName, strSerial, "Virtual Machine Configuration: Name=" + strName + ", OS=" + oOperatingSystem.Get(intOS, "vmware_os") + ", RAM=" + strRamConfig + ", CPU=" + intCpuConfig.ToString() + ", Datastore=" + oVMWare.GetDatastore(intDatastore, "name") + ", Cluster=" + oVMWare.GetCluster(intCluster, "name") + ", Build Folder = " + (strBuildFolder == "" ? "Root" : strBuildFolder) + ", Override = " + (boolIsOverride ? "Yes" : "No"), LoggingType.Information);

                                                ManagedObjectReference vmBuildIn = oVMWare.GetBuildFolder(vmFolderRef, intDataCenter);
                                                ManagedObjectReference _task = _service.CreateVM_Task(vmBuildIn, oConfig, resourcePoolRootRef, null);
                                                TaskInfo oInfo = (TaskInfo)oVMWare.getObjectProperty(_task, "info");
                                                while (oInfo.state == TaskInfoState.running)
                                                    oInfo = (TaskInfo)oVMWare.getObjectProperty(_task, "info");
                                                if (oInfo.state == TaskInfoState.success)
                                                {
                                                    ManagedObjectReference newVM = (ManagedObjectReference)oInfo.result;
                                                    oVMWare.UpdateGuestVIM(strName, newVM.Value);
                                                    if (boolPNC == true)
                                                        oServerName.UpdateFactory(intServerName, oVMWare.GetSerial(strName));
                                                    else
                                                        oServerName.Update(intServerName, oVMWare.GetSerial(strName));
                                                    strResult = "Virtual Machine " + strName.ToUpper() + " Created (" + newVM.Value + ")";

                                                    if (boolIsOverride == true)
                                                    {
                                                        // Send an email to the VMware team notifying them about the drive size change
                                                        strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_VMWARE_DISK");
                                                        string strVMwareConfig = "";
                                                        strVMwareConfig += "<table border=\"0\" cellpadding=\"4\" cellspacing=\"2\" style=\"" + oVariable.DefaultFontStyle() + "\">";
                                                        strVMwareConfig += "<tr><td>Requested By:</td><td>" + oUser.GetFullName(intUser) + " (" + oUser.GetName(intUser) + ")</td></tr>";
                                                        strVMwareConfig += "<tr><td>Executed By:</td><td>" + oUser.GetFullName(intUserExecuted) + " (" + oUser.GetName(intUserExecuted) + ")</td></tr>";
                                                        strVMwareConfig += "<tr><td colspan=\"2\">&nbsp;</td></tr>";
                                                        strVMwareConfig += "<tr><td>Machine Name:</td><td>" + strName + "</td></tr>";
                                                        strVMwareConfig += "<tr><td>Serial Number:</td><td>" + oAsset.Get(intAsset, "serial").ToUpper() + "</td></tr>";
                                                        strVMwareConfig += "<tr><td>Virtual Center:</td><td>" + oVMWare.VirtualCenter() + "</td></tr>";
                                                        strVMwareConfig += "<tr><td>DataCenter:</td><td>" + oVMWare.DataCenter() + "</td></tr>";
                                                        strVMwareConfig += "<tr><td>Cluster:</td><td>" + oVMWare.GetCluster(intCluster, "name") + "</td></tr>";
                                                        strVMwareConfig += "<tr><td>Datastore:</td><td>" + oVMWare.GetDatastore(intDatastore, "name") + "</td></tr>";
                                                        strVMwareConfig += "<tr><td colspan=\"2\">&nbsp;</td></tr>";
                                                        strVMwareConfig += "<tr><td>Requested App Drive Size:</td><td>" + dblSizeRequested.ToString() + " GB</td></tr>";
                                                        strVMwareConfig += "<tr><td>Actual App Drive Size:</td><td>" + dblSize.ToString() + " GB</td></tr>";
                                                        strVMwareConfig += "<tr><td>Requested RAM:</td><td>" + intRamConfigRequested.ToString() + " GB</td></tr>";
                                                        strVMwareConfig += "<tr><td>Actual RAM:</td><td>" + intRamConfig.ToString() + " GB</td></tr>";
                                                        strVMwareConfig += "<tr><td>Requested CPU(s):</td><td>" + intCpuConfigRequested.ToString() + " CPU(s)</td></tr>";
                                                        strVMwareConfig += "<tr><td>Actual CPU(s):</td><td>" + intCpuConfig.ToString() + " CPU(s)</td></tr>";
                                                        strVMwareConfig += "</table>";
                                                        oFunction.SendEmail("VMware Machine Configuration Request", oUser.GetName(intUser) + ";" + oUser.GetName(intUserExecuted), strDriveSizeNotify, strEMailIdsBCC, "VMware Machine Configuration Request", "<p><b>This message is to inform you that the following VMware machine is being provisioned with one or more configuration settings that differ from your original design...</b></p><p>" + strVMwareConfig + "</p><p>Please contact the VMware administration team (carbon copied on this email) for further instructions on how to increase your default allocations.</p><p><b>NOTE:</b> This is automated email sent from the ClearView Auto-Provisioning tool. Please do not respond to this message.</p>", true, false);
                                                    }
                                                }
                                                else
                                                {
                                                    strError = "Virtual Machine Was NOT Created ~ " + strName.ToUpper() + " (RAM = " + strRamConfig + " MB) (CPUs = " + intCpuConfig.ToString() + ") (Datastore = " + oVMWare.GetDatastore(intDatastore, "name") + ")";
                                                    oLog.AddEvent(intAnswer, strName, strSerial, strError, LoggingType.Error);
                                                }
                                            }
                                            else
                                            {
                                                oLog.AddEvent(intAnswer, strName, strSerial, "The datastore is already at maximum machine count...restarting datastore assignment", LoggingType.Information);
                                                boolCreate = false;
                                            }
                                        }
                                        else
                                        {
                                            strError = "Unable to find a datastore";
                                            oLog.AddEvent(intAnswer, strName, strSerial, strError, LoggingType.Error);
                                        }
                                    }
                                    else
                                    {
                                        strError = strConnect;
                                        oLog.AddEvent(intAnswer, strName, strSerial, strError, LoggingType.Error);
                                    }
                                    if (boolCreate == true)
                                        AddResult(intServer, intStep, intType, strResult, strError);
                                    else
                                    {
                                        oVMWare.UpdateGuestDatastore(strName, (intDatastore * -1), 0.00);
                                        oServer.UpdateStep(intServer, intStepDatastore);
                                        oOnDemand.DeleteStepDoneServer(intServer, intStepDatastore);
                                    }
                                    break;
                                case 8:     // Create SCSI Controller
                                    oLog.AddEvent(intAnswer, strName, strSerial, "Starting Step " + intStep.ToString() + " (Create SCSI Controller)", LoggingType.Information);
                                    ManagedObjectReference _vm_scsi = oVMWare.GetVM(strName);
                                    VirtualMachineConfigSpec _cs_scsi = new VirtualMachineConfigSpec();
                                    VirtualDeviceConfigSpec controlVMSpec = Controller(0, 2, 1000, IsWin2012);
                                    _cs_scsi.deviceChange = new VirtualDeviceConfigSpec[] { controlVMSpec };
                                    ManagedObjectReference _task_scsi = _service.ReconfigVM_Task(_vm_scsi, _cs_scsi);
                                    TaskInfo _inf_scsi = (TaskInfo)oVMWare.getObjectProperty(_task_scsi, "info");
                                    while (_inf_scsi.state == TaskInfoState.running)
                                        _inf_scsi = (TaskInfo)oVMWare.getObjectProperty(_task_scsi, "info");
                                    if (_inf_scsi.state == TaskInfoState.success)
                                        strResult = "SCSI Controller # 1 Created";
                                    else
                                    {
                                        strError = "SCSI Controller # 1 Was Not Created";
                                        oLog.AddEvent(intAnswer, strName, strSerial, strError, LoggingType.Error);
                                    }
                                    AddResult(intServer, intStep, intType, strResult, strError);
                                    break;
                                case 9:     // Create Hard Disk 1
                                    oLog.AddEvent(intAnswer, strName, strSerial, "Starting Step " + intStep.ToString() + " (Create Hard Disk 1)", LoggingType.Information);
                                    bool boolHDD1 = true;
                                    if (intDatastore > 0)
                                    {
                                        ManagedObjectReference _vm_hdd1 = oVMWare.GetVM(strName);
                                        VirtualMachineConfigSpec _cs_hdd1 = new VirtualMachineConfigSpec();
                                        int intDriveC = 0;
                                        if (oOperatingSystem.IsWindows(intOS) == true || oOperatingSystem.IsWindows2008(intOS) == true)
                                            intDriveC = Int32.Parse(dblDriveC_Distributed.ToString("0"));
                                        else
                                            intDriveC = Int32.Parse(dblDriveC_Midrange.ToString("0"));
                                        intDriveC = intDriveC * 1024 * 1024;
                                        VirtualDeviceConfigSpec diskVMSpec1 = Disk(oVMWare, strName, oVMWare.GetDatastore(intDatastore, "name"), intDriveC.ToString(), 0, 1000, "");    // 10485760 KB = 10 GB = 10 x 1024 x 1024
                                        _cs_hdd1.deviceChange = new VirtualDeviceConfigSpec[] { diskVMSpec1 };
                                        ManagedObjectReference _task_hdd1 = _service.ReconfigVM_Task(_vm_hdd1, _cs_hdd1);
                                        TaskInfo _info_hdd1 = (TaskInfo)oVMWare.getObjectProperty(_task_hdd1, "info");
                                        while (_info_hdd1.state == TaskInfoState.running)
                                            _info_hdd1 = (TaskInfo)oVMWare.getObjectProperty(_task_hdd1, "info");
                                        if (_info_hdd1.state == TaskInfoState.success)
                                            strResult = "Hard Drive 1 Created (" + intDriveC.ToString() + ")";
                                        else
                                        {
                                            LocalizedMethodFault _error_hdd1 = _info_hdd1.error;
                                            if (_error_hdd1.localizedMessage.ToUpper().Contains("INSUFFICIENT DISK SPACE") == true)
                                            {
                                                // 3/9/2011: Change to new way of redoing...
                                                // Delete / Destroy the guest.
                                                ManagedObjectReference _task_insufficient = _service.Destroy_Task(_vm_hdd1);
                                                TaskInfo _info_insufficient = (TaskInfo)oVMWare.getObjectProperty(_task_insufficient, "info");
                                                while (_info_insufficient.state == TaskInfoState.running)
                                                    _info_insufficient = (TaskInfo)oVMWare.getObjectProperty(_task_insufficient, "info");
                                                if (_info_insufficient.state == TaskInfoState.success)
                                                {
                                                    oLog.AddEvent(intAnswer, strName, strSerial, "The guest has been destroyed...restarting datastore assignment", LoggingType.Information);
                                                    boolHDD1 = false;
                                                }
                                                else
                                                    strError = "Could not destroy the guest to restart the datastore assignment";
                                            }
                                            else
                                            {
                                                strError = "Hard Drive 1 Was Not Created ~ " + oVMWare.GetDatastore(intDatastore, "name") + " (" + intDriveC.ToString() + ")";
                                                oLog.AddEvent(intAnswer, strName, strSerial, strError, LoggingType.Error);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        strError = "Unable to find a datastore";
                                        oLog.AddEvent(intAnswer, strName, strSerial, strError, LoggingType.Error);
                                    }
                                    if (boolHDD1 == true)
                                        AddResult(intServer, intStep, intType, strResult, strError);
                                    else
                                    {
                                        // 3/9/2011: Change to new way of redoing...
                                        //  - Change DatastoreID to (-)DatastoreID.  Set step to assign datastore again (check code above for rest of changes).
                                        oVMWare.UpdateGuestDatastore(strName, (intDatastore * -1), 0.00);
                                        oServer.UpdateStep(intServer, intStepDatastore);
                                        oOnDemand.DeleteStepDoneServer(intServer, intStepDatastore);
                                    }
                                    break;
                                case 10:    // Additional Hard Disks 
                                    oLog.AddEvent(intAnswer, strName, strSerial, "Starting Step " + intStep.ToString() + " (Create Hard Disk 2)", LoggingType.Information);
                                    if (dblSize > 0.00 || oOperatingSystem.IsWindows(intOS) == true || oOperatingSystem.IsWindows2008(intOS) == true)
                                    {
                                        if (intDatastore > 0)
                                        {
                                            ManagedObjectReference _vm_hdd3 = oVMWare.GetVM(strName);

                                            // Query non-shared storage disks
                                            List<VMwareController> controllers = new List<VMwareController>();
                                            if (_vm_hdd3 != null)
                                            {
                                                VirtualMachineConfigInfo vminfo = (VirtualMachineConfigInfo)oVMWare.getObjectProperty(_vm_hdd3, "config");
                                                VirtualDevice[] devices = vminfo.hardware.device;
                                                foreach (VirtualDevice device in devices)
                                                {
                                                    // Try to cast to Controller
                                                    try
                                                    {
                                                        VirtualController controller = (VirtualController)device;
                                                        VMwareController _controller = new VMwareController();
                                                        _controller.busNumber = controller.busNumber;
                                                        _controller.key = controller.key;
                                                        controllers.Add(_controller);
                                                    }
                                                    catch { }
                                                }
                                            }

                                            int intController = 1000;
                                            int intDriveCount = 0;
                                            if (oOperatingSystem.IsWindows2008(intOS) == true)
                                            {
                                                // Disk # 2 on Controller # 1
                                                VirtualMachineConfigSpec _cs_hdd2 = new VirtualMachineConfigSpec();
                                                intDriveCount++;
                                                int intDriveUnit = (intDriveCount >= 7 ? intDriveCount + 1 : intDriveCount);
                                                int intSize = Int32.Parse("10");
                                                intSize = intSize * 1024 * 1024;
                                                VirtualDeviceConfigSpec diskVMSpec2 = Disk(oVMWare, strName, oVMWare.GetDatastore(intDatastore, "name"), intSize.ToString(), intDriveUnit, 1000, "_" + intDriveUnit.ToString());
                                                _cs_hdd2.deviceChange = new VirtualDeviceConfigSpec[] { diskVMSpec2 };
                                                ManagedObjectReference _task_hdd2 = _service.ReconfigVM_Task(_vm_hdd3, _cs_hdd2);
                                                TaskInfo _info_hdd2 = (TaskInfo)oVMWare.getObjectProperty(_task_hdd2, "info");
                                                while (_info_hdd2.state == TaskInfoState.running)
                                                    _info_hdd2 = (TaskInfo)oVMWare.getObjectProperty(_task_hdd2, "info");
                                                if (_info_hdd2.state == TaskInfoState.success)
                                                    strResult += "Additional Hard Drive # 1 Created (" + intSize.ToString() + ")<br/>";
                                                else
                                                {
                                                    strError = "Additional Hard Drive Was Not Created ~ Drive # 1, Datastore: " + oVMWare.GetDatastore(intDatastore, "name") + ", Size: " + intSize.ToString();
                                                    oLog.AddEvent(intAnswer, strName, strSerial, strError, LoggingType.Error);
                                                }

                                                if (strError == "")
                                                {
                                                    // Controller # 2
                                                    intController = 1001;
                                                    VirtualMachineConfigSpec _cs_scsi2 = new VirtualMachineConfigSpec();
                                                    VirtualDeviceConfigSpec controlVMSpec2 = Controller(1, 3, intController, IsWin2012);
                                                    _cs_scsi2.deviceChange = new VirtualDeviceConfigSpec[] { controlVMSpec2 };
                                                    ManagedObjectReference _task_scsi2 = _service.ReconfigVM_Task(_vm_hdd3, _cs_scsi2);
                                                    TaskInfo _inf_scsi2 = (TaskInfo)oVMWare.getObjectProperty(_task_scsi2, "info");
                                                    while (_inf_scsi2.state == TaskInfoState.running)
                                                        _inf_scsi2 = (TaskInfo)oVMWare.getObjectProperty(_task_scsi2, "info");
                                                    if (_inf_scsi2.state == TaskInfoState.success)
                                                        strResult += "SCSI Controller # 2 Created<br/>";
                                                    else
                                                    {
                                                        strError = "SCSI Controller # 2 Was Not Created";
                                                        oLog.AddEvent(intAnswer, strName, strSerial, strError, LoggingType.Error);
                                                    }

                                                }
                                            }

                                            if (strError == "")
                                            {
                                                int intBusNumber = 0;
                                                // Match up controllers for bus numbers
                                                foreach (VMwareController controller in controllers)
                                                {
                                                    if (intController == controller.key)
                                                    {
                                                        intBusNumber = controller.busNumber;
                                                        break;
                                                    }
                                                }

                                                foreach (DataRow drStorage in dsStorage.Tables[0].Rows)
                                                {
                                                    intDriveCount++;
                                                    int intLunID = Int32.Parse(drStorage["id"].ToString());
                                                    int intDriveUnit = (intDriveCount >= 7 ? intDriveCount + 1 : intDriveCount);
                                                    double dblDriveSize = 0.00;
                                                    if (oClass.IsProd(intClass) && double.Parse(drStorage["size"].ToString()) > 0.00)
                                                        dblDriveSize = double.Parse(drStorage["size"].ToString());
                                                    else if (oClass.IsQA(intClass) && double.Parse(drStorage["size_qa"].ToString()) > 0.00)
                                                        dblDriveSize = double.Parse(drStorage["size_qa"].ToString());
                                                    else if (oClass.IsTestDev(intClass) && double.Parse(drStorage["size_test"].ToString()) > 0.00)
                                                        dblDriveSize = double.Parse(drStorage["size_test"].ToString());
                                                    else if (double.Parse(drStorage["size"].ToString()) > 0.00)
                                                        dblDriveSize = double.Parse(drStorage["size"].ToString());

                                                    if (dblDriveSize == 0.00)
                                                        dblDriveSize = dblDefault;

                                                    VirtualMachineConfigSpec _cs_hdd3 = new VirtualMachineConfigSpec();
                                                    //int intSize = Int32.Parse(dblSize.ToString("0"));
                                                    int intSize = Int32.Parse(dblDriveSize.ToString("0"));
                                                    intSize = intSize * 1024 * 1024;
                                                    VirtualDeviceConfigSpec diskVMSpec3 = Disk(oVMWare, strName, oVMWare.GetDatastore(intDatastore, "name"), intSize.ToString(), intDriveUnit, intController, "_" + intDriveUnit.ToString());
                                                    _cs_hdd3.deviceChange = new VirtualDeviceConfigSpec[] { diskVMSpec3 };
                                                    ManagedObjectReference _task_hdd3 = _service.ReconfigVM_Task(_vm_hdd3, _cs_hdd3);
                                                    TaskInfo _info_hdd3 = (TaskInfo)oVMWare.getObjectProperty(_task_hdd3, "info");
                                                    while (_info_hdd3.state == TaskInfoState.running)
                                                        _info_hdd3 = (TaskInfo)oVMWare.getObjectProperty(_task_hdd3, "info");
                                                    if (_info_hdd3.state == TaskInfoState.success)
                                                    {
                                                        oStorage.AddLunDisk(intLunID, intBusNumber, intDriveUnit, "");
                                                        strResult += "Additional Hard Drive # " + intDriveCount.ToString() + " Created (" + intSize.ToString() + ")<br/>";
                                                    }
                                                    else
                                                    {
                                                        strError = "Additional Hard Drive Was Not Created ~ Drive # " + intDriveCount.ToString() + ", Datastore: " + oVMWare.GetDatastore(intDatastore, "name") + ", Size: " + intSize.ToString();
                                                        oLog.AddEvent(intAnswer, strName, strSerial, strError, LoggingType.Error);
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            strError = "Unable to find a datastore";
                                            oLog.AddEvent(intAnswer, strName, strSerial, strError, LoggingType.Error);
                                        }
                                        AddResult(intServer, intStep, intType, strResult, strError);
                                    }
                                    else
                                        AddResult(intServer, intStep, intType, "Hard Drive 2 Was Not Requested", strError);
                                    break;
                                case 11:    // Create Network Adapter
                                    oLog.AddEvent(intAnswer, strName, strSerial, "Starting Step " + intStep.ToString() + " (Create Network Adapter)", LoggingType.Information);
                                    ManagedObjectReference _vm_net = oVMWare.GetVM(strName);
                                    VirtualMachineConfigInfo _vminfo_net = (VirtualMachineConfigInfo)oVMWare.getObjectProperty(_vm_net, "config");
                                    bool boolNic1 = false;
                                    bool boolNic2 = false;
                                    VirtualDevice[] _device_net = _vminfo_net.hardware.device;
                                    for (int ii = 0; ii < _device_net.Length; ii++)
                                    {
                                        if (_device_net[ii].deviceInfo.label.ToUpper() == "NETWORK ADAPTER 1")
                                        {
                                            boolNic1 = true;
                                            oLog.AddEvent(intAnswer, strName, strSerial, "Found Network Adapter # 1", LoggingType.Debug);
                                            break;
                                        }
                                    }
                                    for (int ii = 0; ii < _device_net.Length; ii++)
                                    {
                                        if (_device_net[ii].deviceInfo.label.ToUpper() == "NETWORK ADAPTER 2")
                                        {
                                            boolNic2 = true;
                                            oLog.AddEvent(intAnswer, strName, strSerial, "Found Network Adapter # 2", LoggingType.Debug);
                                            break;
                                        }
                                    }
                                    if (boolNic1 == false)
                                    {
                                        oLog.AddEvent(intAnswer, strName, strSerial, "There are " + dsBuild.Tables[0].Rows.Count.ToString() + " (VMware) build locations (RDP) for class: " + oClass.Get(intClassID, "name") + " (" + intClassID.ToString() + "), env: " + oEnvironment.Get(intEnvironmentID, "name") + " (" + intEnvironmentID.ToString() + "), address: " + oLocation.GetFull(intAddressID) + " (" + intAddressID.ToString() + "), resiliency: " + oResiliency.Get(intResiliency, "name") + " (" + intResiliency.ToString() + "), RDPAltiris: " + boolRDPAltiris.ToString() + ", RDPMDT: " + boolRDPMDT.ToString() + ", Zone: 0 for server ID " + intServer.ToString(), LoggingType.Information);
                                        if (dsBuild.Tables[0].Rows.Count > 0)
                                        {
                                            // Create build NIC
                                            if (intClusterVersion == 0)
                                            {
                                                oLog.AddEvent(intAnswer, strName, strSerial, "Creating Network Adapter # 1 using Direct VLAN", LoggingType.Debug);
                                                if (dsBuild.Tables[0].Rows[0]["vmware_vlan"].ToString() != "")
                                                {
                                                    string strRDPVLAN = dsBuild.Tables[0].Rows[0]["vmware_vlan"].ToString();
                                                    VirtualDeviceConfigSpec[] configspecarr = configspecarr = new VirtualDeviceConfigSpec[1];
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
                                                        strResult += "Network Adapter # 1 Created<br/>";
                                                    else
                                                        strError += "Network Adapter # 1 Was Not Created";
                                                }
                                                else
                                                {
                                                    strError = "Invalid build location (RDP) configuration ~ Missing VMware VLAN...Class: " + oClass.Get(intClass, "name") + ", Environment: " + oEnvironment.Get(intEnv, "name") + ", Location: " + oLocation.GetFull(intAddress);
                                                    oLog.AddEvent(intAnswer, strName, strSerial, strError, LoggingType.Error);
                                                }
                                            }
                                            if (intClusterVersion == 1)
                                            {
                                                oLog.AddEvent(intAnswer, strName, strSerial, "Creating Network Adapter # 1 using Virtual Port Group", LoggingType.Debug);
                                                string strRDPVLAN = "";
                                                if (intClusterDell == 1)    // Hosts are on DELL Hardware
                                                {
                                                    if (dsBuild.Tables[0].Rows[0]["dell_vmware_vlan"].ToString() != "")
                                                        strRDPVLAN = dsBuild.Tables[0].Rows[0]["dell_vmware_vlan"].ToString();
                                                    else
                                                        strError = "Invalid build location (RDP) configuration ~ Missing DELL VMware VLAN...Class: " + oClass.Get(intClass, "name") + ", Environment: " + oEnvironment.Get(intEnv, "name") + ", Location: " + oLocation.GetFull(intAddress);
                                                }
                                                else
                                                {
                                                    if (dsBuild.Tables[0].Rows[0]["vsphere_vlan"].ToString() != "")
                                                        strRDPVLAN = dsBuild.Tables[0].Rows[0]["vsphere_vlan"].ToString();
                                                    else
                                                        strError = "Invalid build location (RDP) configuration ~ Missing VSphere VLAN...Class: " + oClass.Get(intClass, "name") + ", Environment: " + oEnvironment.Get(intEnv, "name") + ", Location: " + oLocation.GetFull(intAddress);
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
                                                            oLog.AddEvent(intAnswer, strName, strSerial, "Checking if RDP VLAN (" + strRDPVLAN + ") equals network name (" + strNetworkName + ")", LoggingType.Debug);
                                                            if (strRDPVLAN == "" || strRDPVLAN == strNetworkName)
                                                            {
                                                                oLog.AddEvent(intAnswer, strName, strSerial, strNetworkName + " has been found!", LoggingType.Debug);
                                                                object oPortConfig = oVMWare.getObjectProperty(oNetwork, "config");
                                                                if (oPortConfig != null)
                                                                {
                                                                    oLog.AddEvent(intAnswer, strName, strSerial, strNetworkName + "...got config", LoggingType.Debug);
                                                                    DVPortgroupConfigInfo oPort = (DVPortgroupConfigInfo)oPortConfig;
                                                                    if (oPort.key != strPortGroupKey)
                                                                    {
                                                                        strPortGroupKey = oPort.key;
                                                                        ManagedObjectReference oSwitch = oPort.distributedVirtualSwitch;
                                                                        string strSwitchUUID = (string)oVMWare.getObjectProperty(oSwitch, "uuid");
                                                                        oLog.AddEvent(intAnswer, strName, strSerial, "Trying..." + strPortGroupKey + "(" + strSwitchUUID + ")", LoggingType.Information);

                                                                        VirtualDeviceConfigSpec[] configspecarr = configspecarr = new VirtualDeviceConfigSpec[1];
                                                                        VirtualEthernetCardDistributedVirtualPortBackingInfo vecdvpbi = new VirtualEthernetCardDistributedVirtualPortBackingInfo();
                                                                        DistributedVirtualSwitchPortConnection connection = new DistributedVirtualSwitchPortConnection();
                                                                        connection.portgroupKey = strPortGroupKey;
                                                                        connection.switchUuid = strSwitchUUID;
                                                                        vecdvpbi.port = connection;
                                                                        VirtualEthernetCard newethdev;
                                                                        if (oOperatingSystem.Get(intOS, "e1000") == "1")
                                                                            newethdev = new VirtualE1000();
                                                                        else
                                                                            newethdev = new VirtualVmxnet3();
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
                                                                            strResult += "Network Adapter # 1 Created<br/>";
                                                                            boolCompleted = true;
                                                                        }
                                                                        else
                                                                            strError += "Network Adapter # 1 Was Not Created";
                                                                        //break;
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            // Only hits here if it is not a "DistributedVirtualPortgroup" (meaning it is a standard NETWORK object)
                                                            oLog.AddEvent(intAnswer, strName, strSerial, "VLAN (" + strNetworkName + ") must not be a DistributedVirtualPortgroup since it threw an error", LoggingType.Debug);
                                                        }
                                                    }
                                                    if (boolCompleted == false)
                                                    {
                                                        strError = "Network Adapter # 1 Was Not Created ~ Could not find a port group (" + strRDPVLAN + ")";
                                                        oLog.AddEvent(intAnswer, strName, strSerial, strError, LoggingType.Error);
                                                    }
                                                }
                                                else
                                                {
                                                    strError = "Invalid build location (RDP) configuration ~ Missing VSphere VLAN...Class: " + oClass.Get(intClass, "name") + ", Environment: " + oEnvironment.Get(intEnv, "name") + ", Location: " + oLocation.GetFull(intAddress);
                                                    oLog.AddEvent(intAnswer, strName, strSerial, strError, LoggingType.Error);
                                                }
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
                                        strResult += "Network Adapter # 1 was Already Created";
                                    }

                                    if (strError == "")
                                    {
                                        if (boolNic2 == false)
                                        {
                                            if (boolIsClustering)
                                            {
                                                if (intClusterVersion == 0)
                                                {
                                                    oLog.AddEvent(intAnswer, strName, strSerial, "Creating Network Adapter # 2 using Direct VLAN", LoggingType.Debug);
                                                    DataSet dsIP = oServer.GetIP(intServer, 0, 0, 1, 0);
                                                    foreach (DataRow drIP in dsIP.Tables[0].Rows)
                                                    {
                                                        int intClusterNetwork = Int32.Parse(oIPAddresses.Get(intIPAddressCluster, "networkid"));
                                                        if (intClusterNetwork > 0)
                                                        {
                                                            int intClusterVLAN = Int32.Parse(oIPAddresses.GetNetwork(intClusterNetwork, "vlanid"));
                                                            if (intClusterVLAN > 0)
                                                            {
                                                                DataSet dsVlan = oVMWare.GetVlanAssociations(intClusterVLAN, intCluster);
                                                                if (dsVlan.Tables[0].Rows.Count > 0)
                                                                {
                                                                    int intVMWareVLAN = Int32.Parse(dsVlan.Tables[0].Rows[0]["vmware_vlanid"].ToString());
                                                                    //oVMWare.UpdateGuestVlan(strName, intVMWareVLAN);
                                                                    string strClusterVLAN = oVMWare.GetVlan(intVMWareVLAN, "name");

                                                                    VirtualDeviceConfigSpec[] configspecarr2 = new VirtualDeviceConfigSpec[1];
                                                                    VirtualEthernetCardNetworkBackingInfo vecnbi2 = new VirtualEthernetCardNetworkBackingInfo();
                                                                    vecnbi2.deviceName = strClusterVLAN;
                                                                    VirtualEthernetCard newethdev2;
                                                                    if (oOperatingSystem.Get(intOS, "e1000") == "1")
                                                                        newethdev2 = new VirtualE1000();
                                                                    else
                                                                        newethdev2 = new VirtualVmxnet3();
                                                                    newethdev2.backing = vecnbi2;
                                                                    newethdev2.key = 5000;
                                                                    VirtualDeviceConfigSpec newethdevicespec2 = new VirtualDeviceConfigSpec();
                                                                    newethdevicespec2.device = newethdev2;
                                                                    newethdevicespec2.operation = VirtualDeviceConfigSpecOperation.add;
                                                                    newethdevicespec2.operationSpecified = true;
                                                                    configspecarr2[0] = newethdevicespec2;
                                                                    VirtualMachineConfigSpec vmconfigspec2 = new VirtualMachineConfigSpec();
                                                                    vmconfigspec2.deviceChange = configspecarr2;
                                                                    ManagedObjectReference _task_net2 = _service.ReconfigVM_Task(_vm_net, vmconfigspec2);
                                                                    TaskInfo _info_net2 = (TaskInfo)oVMWare.getObjectProperty(_task_net2, "info");
                                                                    while (_info_net2.state == TaskInfoState.running)
                                                                        _info_net2 = (TaskInfo)oVMWare.getObjectProperty(_task_net2, "info");
                                                                    if (_info_net2.state == TaskInfoState.success)
                                                                        strResult += "Network Adapter # 2 Created";
                                                                    else
                                                                        strError += "Network Adapter # 2 Was Not Created";
                                                                }
                                                                else
                                                                {
                                                                    strError += "There are no VMware associations ~ VLAN " + oIPAddresses.GetVlan(intClusterVLAN, "vlan");
                                                                    oLog.AddEvent(intAnswer, strName, strSerial, strError, LoggingType.Error);
                                                                }
                                                            }
                                                            else
                                                            {
                                                                strError += "Invalid VLAN ~ Configuring Cluster";
                                                                oLog.AddEvent(intAnswer, strName, strSerial, strError, LoggingType.Error);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            strError += "Invalid Network ~ Configuring Cluster";
                                                            oLog.AddEvent(intAnswer, strName, strSerial, strError, LoggingType.Error);
                                                        }
                                                    }
                                                }
                                                if (intClusterVersion == 1)
                                                {
                                                    oLog.AddEvent(intAnswer, strName, strSerial, "Creating Network Adapter # 2 using Virtual Port Group", LoggingType.Debug);
                                                    bool boolCompletedCluster = false;
                                                    DataSet dsIP = oServer.GetIP(intServer, 0, 0, 1, 0);
                                                    foreach (DataRow drIP in dsIP.Tables[0].Rows)
                                                    {
                                                        int intClusterNetwork = Int32.Parse(oIPAddresses.Get(intIPAddressCluster, "networkid"));
                                                        if (intClusterNetwork > 0)
                                                        {
                                                            int intClusterVLAN = Int32.Parse(oIPAddresses.GetNetwork(intClusterNetwork, "vlanid"));
                                                            if (intClusterVLAN > 0)
                                                            {
                                                                DataSet dsVlan = oVMWare.GetVlanAssociations(intClusterVLAN, intCluster);
                                                                if (dsVlan.Tables[0].Rows.Count > 0)
                                                                {
                                                                    int intVMWareVLAN = Int32.Parse(dsVlan.Tables[0].Rows[0]["vmware_vlanid"].ToString());
                                                                    //oVMWare.UpdateGuestVlan(strName, intVMWareVLAN);
                                                                    string strClusterVLAN = oVMWare.GetVlan(intVMWareVLAN, "name");

                                                                    string strPortGroupKey = "";
                                                                    ManagedObjectReference datacenterRefNetwork = oVMWare.GetDataCenter();
                                                                    ManagedObjectReference[] oNetworks = (ManagedObjectReference[])oVMWare.getObjectProperty(datacenterRefNetwork, "network");
                                                                    foreach (ManagedObjectReference oNetwork in oNetworks)
                                                                    {
                                                                        if (boolCompletedCluster == true)
                                                                            break;
                                                                        string strNetworkName = oVMWare.getObjectProperty(oNetwork, "name").ToString();
                                                                        try
                                                                        {
                                                                            oLog.AddEvent(intAnswer, strName, strSerial, "Checking if Cluster VLAN (" + strClusterVLAN + ") equals network name (" + strNetworkName + ")", LoggingType.Debug);
                                                                            if (strClusterVLAN == strNetworkName)
                                                                            {
                                                                                oLog.AddEvent(intAnswer, strName, strSerial, strNetworkName + " has been found!", LoggingType.Debug);
                                                                                object oPortConfig = oVMWare.getObjectProperty(oNetwork, "config");
                                                                                if (oPortConfig != null)
                                                                                {
                                                                                    oLog.AddEvent(intAnswer, strName, strSerial, strNetworkName + "...got config", LoggingType.Debug);
                                                                                    DVPortgroupConfigInfo oPort = (DVPortgroupConfigInfo)oPortConfig;
                                                                                    if (oPort.key != strPortGroupKey)
                                                                                    {
                                                                                        strPortGroupKey = oPort.key;
                                                                                        ManagedObjectReference oSwitch = oPort.distributedVirtualSwitch;
                                                                                        string strSwitchUUID = (string)oVMWare.getObjectProperty(oSwitch, "uuid");
                                                                                        oLog.AddEvent(intAnswer, strName, strSerial, "Trying..." + strPortGroupKey + "(" + strSwitchUUID + ")", LoggingType.Information);

                                                                                        VirtualDeviceConfigSpec[] configspecarr = configspecarr = new VirtualDeviceConfigSpec[1];
                                                                                        VirtualEthernetCardDistributedVirtualPortBackingInfo vecdvpbi = new VirtualEthernetCardDistributedVirtualPortBackingInfo();
                                                                                        DistributedVirtualSwitchPortConnection connection = new DistributedVirtualSwitchPortConnection();
                                                                                        connection.portgroupKey = strPortGroupKey;
                                                                                        connection.switchUuid = strSwitchUUID;
                                                                                        vecdvpbi.port = connection;
                                                                                        VirtualEthernetCard newethdev;
                                                                                        if (oOperatingSystem.Get(intOS, "e1000") == "1")
                                                                                            newethdev = new VirtualE1000();
                                                                                        else
                                                                                            newethdev = new VirtualVmxnet3();
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
                                                                                            strResult += "Network Adapter # 2 Created";
                                                                                            boolCompletedCluster = true;
                                                                                        }
                                                                                        else
                                                                                            strError += "Network Adapter # 2 Was Not Created";
                                                                                        //break;
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                        catch (Exception ex)
                                                                        {
                                                                            // Only hits here if it is not a "DistributedVirtualPortgroup" (meaning it is a standard NETWORK object)
                                                                            oLog.AddEvent(intAnswer, strName, strSerial, "VLAN (" + strNetworkName + ") must not be a DistributedVirtualPortgroup since it threw an error", LoggingType.Debug);
                                                                        }
                                                                    }
                                                                    if (boolCompletedCluster == false)
                                                                    {
                                                                        strError = "Network Adapter # 2 Was Not Created ~ Could not find a port group (" + strClusterVLAN + ")";
                                                                        oLog.AddEvent(intAnswer, strName, strSerial, strError, LoggingType.Error);
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    strError = "There are no VMware associations ~ VLAN " + oIPAddresses.GetVlan(intClusterVLAN, "vlan");
                                                                    oLog.AddEvent(intAnswer, strName, strSerial, strError, LoggingType.Error);
                                                                }
                                                            }
                                                            else
                                                            {
                                                                strError = "Invalid VLAN ~ Configuring Cluster";
                                                                oLog.AddEvent(intAnswer, strName, strSerial, strError, LoggingType.Error);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            strError = "Invalid Network ~ Configuring Cluster";
                                                            oLog.AddEvent(intAnswer, strName, strSerial, strError, LoggingType.Error);
                                                        }
                                                    }
                                                    if (strError == "" && boolCompletedCluster == false)
                                                    {
                                                        strError = "Network Adapter # 2 Was Not Created ~ No IP address assigned";
                                                        oLog.AddEvent(intAnswer, strName, strSerial, strError, LoggingType.Error);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                oLog.AddEvent(intAnswer, strName, strSerial, "This vmware guest does not require clustering", LoggingType.Information);
                                                strResult += "Network Adapter # 2 was not needed";
                                            }
                                        }
                                        else
                                        {
                                            strResult += "Network Adapter # 2 already exists";
                                            oLog.AddEvent(intAnswer, strName, strSerial, "Network Adapter # 2 already exists", LoggingType.Information);
                                        }
                                    }
                                    AddResult(intServer, intStep, intType, strResult, strError);
                                    break;
                                case 12:    // Attach Floppy Drive 
                                    oLog.AddEvent(intAnswer, strName, strSerial, "Starting Step " + intStep.ToString() + " (Attach Floppy Drive)", LoggingType.Information);
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
                                    {
                                        strError = "Floppy Drive Was Not Created";
                                        oLog.AddEvent(intAnswer, strName, strSerial, strError, LoggingType.Error);
                                    }
                                    AddResult(intServer, intStep, intType, strResult, strError);
                                    break;
                                case 13:    // Attach CD-ROM Drive
                                    oLog.AddEvent(intAnswer, strName, strSerial, "Starting Step " + intStep.ToString() + " (Attach CD-ROM Drive)", LoggingType.Information);
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
                                    {
                                        strError = "CD-ROM Was Not Created";
                                        oLog.AddEvent(intAnswer, strName, strSerial, strError, LoggingType.Error);
                                    }
                                    AddResult(intServer, intStep, intType, strResult, strError);
                                    break;
                                case 14:     // Configure ZEUS 
                                    oLog.AddEvent(intAnswer, strName, strSerial, "Starting Step " + intStep.ToString() + "a (Configure Image)", LoggingType.Information);
                                    strAsset = oAsset.GetVSG(strName, "SERVER");
                                    if (strAsset == "")
                                        strAsset = oAsset.GetVSG("SERVER");
                                    if (strAsset == "")
                                    {
                                        strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_INVENTORY_MANAGER");

                                        oFunction.SendEmail("Auto-Provisioning VSG Error", strVSG, "", strEMailIdsBCC, "Auto-Provisioning VSG Error", "<p><b>This message is to inform you that there are NO <u>SERVER</u> VSG asset tag numbers left for virtual assets.</b><p><p>Please import more VSG numbers immediately. This problem is preventing builds from finishing.</p><p><b>NOTE:</b> This is automated email sent from the ClearView Auto-Provisioning tool. Please do not respond to this message.</p>", true, false);
                                        strError = "Invalid VSG number";
                                        oLog.AddEvent(intAnswer, strName, strSerial, strError, LoggingType.Error);
                                    }
                                    else
                                    {
                                        strSerial = oVMWare.GetSerial(strName);
                                        oAsset.Update(intAsset, strSerial, strAsset);
                                        DataSet dsVSG = oAsset.UpdateVSG(strAsset, strName, "SERVER");
                                        strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_INVENTORY_MANAGER");
                                        if (dsVSG.Tables[0].Rows.Count < 20)
                                            oFunction.SendEmail("Auto-Provisioning VSG Configuration", strVSG, "", strEMailIdsBCC, "Auto-Provisioning VSG Configuration", "<p><b>This message is to inform you that there are less than 20 <u>SERVER</u> VSG asset tag numbers left for virtual assets.</b><p><p>Please import more VSG numbers immediately to prevent errors.</p><p><b>NOTE:</b> This is automated email sent from the ClearView Auto-Provisioning tool. Please do not respond to this message.</p>", true, false);
                                        oZeus.DeleteBuild(strSerial);
                                        oZeus.DeleteBuildName(strName);
                                        oZeus.DeleteApps(strSerial);
                                        oZeus.DeleteLuns(strSerial);
                                        oZeus.DeleteResults(strSerial);
                                        oZeus.AddLuns(intAnswer, dsn, dsnAsset);
                                        string strArrayConfig = "BASIC";
                                        //if (boolPNC == true)
                                        //    strArrayConfig = "PNCBASIC30";
                                        ManagedObjectReference _vm_mac = oVMWare.GetVM(strName);
                                        VirtualMachineConfigInfo _vminfo = (VirtualMachineConfigInfo)oVMWare.getObjectProperty(_vm_mac, "config");
                                        VirtualDevice[] _device = _vminfo.hardware.device;
                                        for (int ii = 0; ii < _device.Length; ii++)
                                        {
                                            if (_device[ii].deviceInfo.label.ToUpper() == "NETWORK ADAPTER 1")
                                            {
                                                VirtualEthernetCard nic = (VirtualEthernetCard)_device[ii];
                                                strMACAddress = nic.macAddress;
                                                if (boolIsClustering == false || String.IsNullOrEmpty(strMACAddress2) == false)
                                                    break;
                                            }
                                            else if (boolIsClustering && _device[ii].deviceInfo.label.ToUpper() == "NETWORK ADAPTER 2")
                                            {
                                                VirtualEthernetCard nic = (VirtualEthernetCard)_device[ii];
                                                strMACAddress2 = nic.macAddress;
                                                if (String.IsNullOrEmpty(strMACAddress) == false)
                                                    break;
                                            }
                                        }
                                        if (String.IsNullOrEmpty(strMACAddress))
                                        {
                                            strError = "The Image could not be configured with a blank MAC Address";
                                            oLog.AddEvent(intAnswer, strName, strSerial, strError, LoggingType.Error);
                                        }
                                        else if (boolIsClustering && String.IsNullOrEmpty(strMACAddress2))
                                        {
                                            strError = "The Image could not be configured with a blank Cluster MAC Address";
                                            oLog.AddEvent(intAnswer, strName, strSerial, strError, LoggingType.Error);
                                        }
                                        else
                                        {
                                            oVMWare.UpdateGuestMAC(strName, strMACAddress);

                                            // Add public interface connection information
                                            oServer.DeleteIPNicInterface(intIPAddressID);
                                            oServer.AddIPNicInterface(intIPAddressID, strMACAddress, "PNCNet", (boolIsClustering ? 1 : 0), 1);
                                            if (boolIsClustering)
                                            {
                                                oServer.DeleteIPNicInterface(intIPAddressIDCluster);
                                                oServer.AddIPNicInterface(intIPAddressIDCluster, strMACAddress2, "Cluster", 1, 0);
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
                                                oLog.AddEvent(intAnswer, strName, strSerial, "Boot delay and boot retry changed to 10 seconds", LoggingType.Information);
                                            else
                                                oLog.AddEvent(intAnswer, strName, strSerial, "Boot delay and boot retry NOT changed", LoggingType.Warning);

                                            string strArrayConfigComponent = "";
                                            string strZeusBuildTypeComponent = "";
                                            bool boolIIS = false;
                                            if (dsComponents.Tables[0].Rows.Count > 0)
                                            {
                                                DataView dvComponents = dsComponents.Tables[0].DefaultView;

                                                // Get the array config selected from the components table
                                                dvComponents.Sort = "array_config_priority";
                                                if (boolIsClustering)
                                                    strArrayConfigComponent = dvComponents[0]["array_config_cluster"].ToString();
                                                else
                                                    strArrayConfigComponent = dvComponents[0]["array_config"].ToString();

                                                if (dvComponents[0]["iis"].ToString() == "1")
                                                    boolIIS = true;

                                                // Get the build type selected from the components table
                                                dvComponents.Sort = "build_type_priority";
                                                strZeusBuildTypeComponent = dvComponents[0]["build_type"].ToString();
                                            }

                                            foreach (DataRow drComponent in dsComponents.Tables[0].Rows)
                                                oZeus.AddApp(strSerial, drComponent["zeus_code"].ToString(), drComponent["install_path"].ToString());

                                            string strZeusOS = oOperatingSystem.Get(intOS, "zeus_os");
                                            string strZeusOSVersion = oOperatingSystem.Get(intOS, "zeus_os_version");
                                            string strZeusBuildTypeWindows = oOperatingSystem.Get(intOS, "zeus_build_type");

                                            DataSet dsBuildRDP = oBuildLocation.GetRDPs(intClass, intEnv, intAddress, intResiliency, (boolRDPAltiris ? 1 : 0), (boolRDPMDT ? 1 : 0), 0, 0);
                                            if (oOperatingSystem.IsWindows(intOS) == true || oOperatingSystem.IsWindows2008(intOS) == true)
                                            {
                                                oLog.AddEvent(intAnswer, strName, strSerial, "There are " + dsBuildRDP.Tables[0].Rows.Count.ToString() + " build locations (RDP) for class: " + oClass.Get(intClass, "name") + ", address: " + oLocation.GetFull(intAddress) + " for server ID " + intServer.ToString(), LoggingType.Information);
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
                                                strError = oZeus.AddBuild(intServer, 0, 0, strSerial, strAsset, strName.ToLower(), strArrayConfig, strZeusOS, strZeusOSVersion, Int32.Parse(oServicePack.Get(intSP, "number")), strZeusBuildTypeWindows, strDomain, intEnvironment, strSource, 0, strMACAddress, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", 1);
                                                if (strError == "")
                                                    oLog.AddEvent(intAnswer, strName, strSerial, "Record added to Imaging table", LoggingType.Information);
                                            }
                                            else
                                            {
                                                string strIPZeus = "";
                                                string strIPVLAN = "";
                                                string strIPmask = "";
                                                string strIPgateway = "";
                                                if (intIPAddress > 0)
                                                {
                                                    strIPZeus = oIPAddresses.GetName(intIPAddress, 0);
                                                    int intIPNetwork = Int32.Parse(oIPAddresses.Get(intIPAddress, "networkid"));
                                                    strIPmask = oIPAddresses.GetNetwork(intIPNetwork, "mask");
                                                    strIPgateway = oIPAddresses.GetNetwork(intIPNetwork, "gateway");
                                                    int intIPVlan = Int32.Parse(oIPAddresses.GetNetwork(intIPNetwork, "vlanid"));
                                                    strIPVLAN = oIPAddresses.GetVlan(intIPVlan, "vlan");
                                                }
                                                string strIPZeusBackup = "";
                                                string strIPBackupVLAN = "";
                                                string strIPBackupMask = "";
                                                string strIPBackupGateway = "";
                                                //if (intIPAddressAvamar > 0)
                                                //{
                                                //    strIPZeusBackup = oIPAddresses.GetName(intIPAddressAvamar, 0);
                                                //    int intIPNetwork = Int32.Parse(oIPAddresses.Get(intIPAddressAvamar, "networkid"));
                                                //    strIPBackupMask = oIPAddresses.GetNetwork(intIPNetwork, "mask");
                                                //    strIPBackupGateway = oIPAddresses.GetNetwork(intIPNetwork, "gateway");
                                                //    int intIPVlan = Int32.Parse(oIPAddresses.GetNetwork(intIPNetwork, "vlanid"));
                                                //    strIPBackupVLAN = oIPAddresses.GetVlan(intIPVlan, "vlan");
                                                //}
                                                string strZeusBuildTypeLinux = "";
                                                if (strArrayConfigComponent != "")
                                                    strArrayConfig = strArrayConfigComponent;
                                                if (strZeusBuildTypeComponent != "")
                                                    strZeusBuildTypeLinux = strZeusBuildTypeComponent;
                                                if (boolPNC == true)
                                                {
                                                    if (oEnvironment.Get(intEnv, "ecom") == "1")
                                                        strError = oZeus.AddBuild(intServer, 0, 0, strSerial, strAsset, strName.ToLower(), strArrayConfig, "", "", 0, strZeusBuildTypeLinux, "PNCETECH", intEnvironment, "SERVER", 0, strMACAddress, "", strIPZeus, "#" + strIPVLAN, strIPmask, strIPgateway, strIPZeusBackup, "#" + strIPBackupVLAN, strIPBackupMask, strIPBackupGateway, "", "", "", "", "", "", 1);
                                                    else
                                                        strError = oZeus.AddBuild(intServer, 0, 0, strSerial, strAsset, strName.ToLower(), strArrayConfig, "", "", 0, strZeusBuildTypeLinux, strDomain, intEnvironment, "SERVER", 0, strMACAddress, "", strIPZeus, "#" + strIPVLAN, strIPmask, strIPgateway, strIPZeusBackup, "#" + strIPBackupVLAN, strIPBackupMask, strIPBackupGateway, "", "", "", "", "", "", 1);
                                                }
                                                else
                                                    strError = oZeus.AddBuild(intServer, 0, 0, strSerial, strAsset, strName.ToLower(), strArrayConfig, "", "", 0, strZeusBuildTypeLinux, strDomain, intEnvironment, "SERVER", 0, strMACAddress, "", strIPZeus, "#" + strIPVLAN, strIPmask, strIPgateway, strIPZeusBackup, "#" + strIPBackupVLAN, strIPBackupMask, strIPBackupGateway, "", "", "", "", "", "", 1);
                                            }

                                            if (strError == "")
                                            {
                                                oServer.DeleteAsset(intServer);
                                                oServer.AddAsset(intServer, intAsset, intClassID, intEnvironmentID, intRemovable, 0);
                                                oServer.UpdateZeus(intServer);
                                                strResult = "IMAGE has been configured";

                                                if (boolRDPMDT)
                                                {
                                                    // Add to MDT
                                                    if (dsBuild.Tables[0].Rows[0]["rdp_mdt_ws"].ToString() != "")
                                                    {
                                                        string strRDPMDTWebService = dsBuild.Tables[0].Rows[0]["rdp_mdt_ws"].ToString();
                                                        System.Net.NetworkCredential oCredentials = new System.Net.NetworkCredential(oVariable.ADUser(), oVariable.ADPassword(), oVariable.Domain());
                                                        BuildSubmit oMDT = new BuildSubmit();
                                                        oMDT.Credentials = oCredentials;
                                                        oMDT.Url = strRDPMDTWebService;
                                                        oMDT.Timeout = 30000;
                                                        oLog.AddEvent(intAnswer, strName, strSerial, "Running ForceCleanup MDT on " + strRDPMDTWebService + " (" + strName + ", " + strMACAddress + ", " + "ServerShare" + ")", LoggingType.Information);
                                                        oMDT.ForceCleanup(strName, strMACAddress, "ServerShare");
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
                                                        //string[] strExtendedMDT = new string[] { "PNCBACKUPSOFTWARE:" + strBackup, "IISInstall:" + (boolIIS ? "YES" : "NO"), "HWConfig:DEFAULT", "ESMInstall:YES", "ClearViewInstall:YES", "Teamed2:DEFAULT" };
                                                        string[] strExtendedMDT = new string[] { "PNCBACKUPSOFTWARE:" + strBackup, "IISInstall:" + (boolIIS ? "YES" : "NO"), "HWConfig:DEFAULT", "ESMInstall:YES", "ClearViewInstall:YES", "Teamed2:DEFAULT", "HIDSInstall:" + strHIDs };
                                                        string strExtendedMDTs = "";
                                                        foreach (string extendedMDT in strExtendedMDT)
                                                        {
                                                            if (strExtendedMDTs != "")
                                                                strExtendedMDTs += ", ";
                                                            strExtendedMDTs += extendedMDT;
                                                        }
                                                        oLog.AddEvent(intAnswer, strName, strSerial, "Registering MDT on " + strRDPMDTWebService + ", automatedBuild2 (" + strName + ", " + strMACAddress + ", " + strBootEnvironment + ", " + "provision" + ", " + strDomain + ", " + strTaskSequence + ", " + "ServerShare" + ", ExtendedValues=" + strExtendedMDTs + ")", LoggingType.Information);
                                                        try
                                                        {
                                                            oMDT.automatedBuild2(strName, strMACAddress, strBootEnvironment, "provision", strDomain, strTaskSequence, "ServerShare", strExtendedMDT);
                                                        }
                                                        catch (Exception exMDT)
                                                        {
                                                            oLog.AddEvent(intAnswer, strName, strSerial, "MDT has encountered an error ~ " + exMDT.Message, LoggingType.Error);
                                                        }
                                                        oLog.AddEvent(intAnswer, strName, strSerial, "MDT has been configured", LoggingType.Information);
                                                    }
                                                    else
                                                    {
                                                        strError = "Invalid build location (RDP) configuration ~ Missing RDP Config...Class: " + oClass.Get(intClass, "name") + ", Environment: " + oEnvironment.Get(intEnv, "name") + ", Location: " + oLocation.GetFull(intAddress);
                                                        oLog.AddEvent(intAnswer, strName, strSerial, strError, LoggingType.Error);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    AddResult(intServer, intStep, intType, strResult, strError);
                                    break;
                                case 15:    // Power On Virtual Machine 
                                    // Add code for Altiris
                                    oLog.AddEvent(intAnswer, strName, strSerial, "Starting Step " + intStep.ToString() + " (Power On Virtual Machine)", LoggingType.Information);
                                    // Strip the :'s AND -'sout of MAC address
                                    string strAltirisMAC = oFunction.FormatMAC(strMACAddress, "");
                                    // Configure Altiris
                                    oLog.AddEvent(intAnswer, strName, strSerial, "There are " + dsBuild.Tables[0].Rows.Count.ToString() + " build locations (RDP) for class: " + oClass.Get(intClass, "name") + ", address: " + oLocation.GetFull(intAddress) + " for server ID " + intServer.ToString(), LoggingType.Information);
                                    if (dsBuild.Tables[0].Rows.Count > 0)
                                    {
                                        if (boolRDPMDT)
                                        {
                                            // Already added to MDT
                                        }
                                        if (boolRDPAltiris)
                                        {
                                            if (dsBuild.Tables[0].Rows[0]["rdp_computer_ws"].ToString() != "" && dsBuild.Tables[0].Rows[0]["rdp_schedule_ws"].ToString() != "")
                                            {
                                                // Create Computer Object
                                                System.Net.NetworkCredential oCredentials = new System.Net.NetworkCredential(oVariable.AltirisUsername(), oVariable.AltirisPassword(), oVariable.AltirisDomain());
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
                                                    oLog.AddEvent(intAnswer, strName, strSerial, "Found Duplicate Computer Object....Deleting Altiris ComputerID " + intDeleteComputer.ToString() + " on " + strRDPComputerWebService + "...result = " + boolDelete.ToString(), LoggingType.Information);
                                                }
                                                // Add Computer Object
                                                oLog.AddEvent(intAnswer, strName, strSerial, "Configuring Altiris (MAC: " + strAltirisMAC + ") on " + strRDPScheduleWebService, LoggingType.Information);
                                                int intComputer = oComputer.AddBasicVirtualComputer(-1, strName, oAsset.Get(intAsset, "asset"), oAsset.Get(intAsset, "serial"), strAltirisMAC, 2, "");
                                                // Assign Schedule
                                                NCC.ClearView.Application.Core.altirisws.dsjob oJob = new NCC.ClearView.Application.Core.altirisws.dsjob();
                                                oJob.Credentials = oCredentials;
                                                oJob.Url = strRDPScheduleWebService;
                                                oLog.AddEvent(intAnswer, strName, strSerial, "Adding Altiris Job (" + oOperatingSystem.Get(intOS, "altiris") + ")", LoggingType.Information);
                                                oJob.ScheduleNow(strName, oOperatingSystem.Get(intOS, "altiris"));
                                                oLog.AddEvent(intAnswer, strName, strSerial, "Finished Configuring Altiris", LoggingType.Information);
                                            }
                                            else
                                            {
                                                strError = "Invalid build location (RDP) configuration ~ Missing RDP Config...Class: " + oClass.Get(intClass, "name") + ", Environment: " + oEnvironment.Get(intEnv, "name") + ", Location: " + oLocation.GetFull(intAddress);
                                                oLog.AddEvent(intAnswer, strName, strSerial, strError, LoggingType.Error);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        strError = "Invalid build location (RDP) configuration ~ Class: " + oClass.Get(intClass, "name") + ", Environment: " + oEnvironment.Get(intEnv, "name") + ", Location: " + oLocation.GetFull(intAddress);
                                        oLog.AddEvent(intAnswer, strName, strSerial, strError, LoggingType.Error);
                                    }
                                    if (strError == "")
                                    {
                                        ManagedObjectReference _vm_power = oVMWare.GetVM(strName);
                                        VirtualMachineRuntimeInfo run_power = (VirtualMachineRuntimeInfo)oVMWare.getObjectProperty(_vm_power, "runtime");
                                        if (run_power.powerState == VirtualMachinePowerState.poweredOff)
                                        {
                                            ManagedObjectReference _task_power = _service.PowerOnVM_Task(_vm_power, null);
                                            TaskInfo _info_power = (TaskInfo)oVMWare.getObjectProperty(_task_power, "info");
                                            while (_info_power.state == TaskInfoState.running)
                                                _info_power = (TaskInfo)oVMWare.getObjectProperty(_task_power, "info");
                                            if (_info_power.state == TaskInfoState.success)
                                            {
                                                strResult = "Virtual Machine Powered On";
                                                oLog.AddEvent(intAnswer, strName, strSerial, "Virtual Machine Powered On", LoggingType.Information);
                                                oVMWare.UpdateGuestDone(strName, 1);
                                            }
                                            else
                                            {
                                                if (true)
                                                {
                                                    strError = "Virtual Machine Was Not Powered On";
                                                    oLog.AddEvent(intAnswer, strName, strSerial, strError, LoggingType.Error);
                                                }
                                                else
                                                {
                                                    // Wait a few seconds, then try again...
                                                    Thread.Sleep(10000);
                                                    ManagedObjectReference _task_power2 = _service.PowerOnVM_Task(_vm_power, null);
                                                    TaskInfo _info_power2 = (TaskInfo)oVMWare.getObjectProperty(_task_power2, "info");
                                                    while (_info_power2.state == TaskInfoState.running)
                                                        _info_power2 = (TaskInfo)oVMWare.getObjectProperty(_task_power2, "info");
                                                    if (_info_power2.state == TaskInfoState.success)
                                                    {
                                                        strResult = "Virtual Machine Powered On";
                                                        oLog.AddEvent(intAnswer, strName, strSerial, "Virtual Machine Powered On", LoggingType.Information);
                                                        oVMWare.UpdateGuestDone(strName, 1);
                                                    }
                                                    else
                                                    {
                                                        strError = "Virtual Machine Was Not Powered On";
                                                        oLog.AddEvent(intAnswer, strName, strSerial, strError, LoggingType.Error);
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            strResult = "Virtual Machine Was Already Powered On (" + run_power.powerState.ToString() + ")";
                                            oLog.AddEvent(intAnswer, strName, strSerial, strResult, LoggingType.Information);
                                            oVMWare.UpdateGuestDone(strName, 1);
                                        }
                                    }
                                    AddResult(intServer, intStep, intType, strResult, strError);
                                    break;
                                case 16:     // ZEUS 
                                    if (intLogging > 1)
                                        oEventLog.WriteEntry(strName + " (VMware): " + "Starting Step " + intStep.ToString() + "a (Image)", EventLogEntryType.Information);
                                    DataSet dsZeusError = oZeus.GetResult(strSerial, 1);
                                    if (strDHCP != "0" && strDHCP != "")
                                    {
                                        // Configure Altiris
                                        oLog.AddEvent(intAnswer, strName, strSerial, "There are " + dsBuild.Tables[0].Rows.Count.ToString() + " build locations (RDP) for class: " + oClass.Get(intClassID, "name") + ", address: " + oLocation.GetFull(intAddressID) + " for server ID " + intServer.ToString(), LoggingType.Information);
                                        if (dsBuild.Tables[0].Rows.Count > 0)
                                        {
                                            if (boolRDPMDT)
                                            {
                                                if (dsBuild.Tables[0].Rows[0]["rdp_mdt_ws"].ToString() != "")
                                                {
                                                    // Cleanup MDT
                                                    string strRDPMDTWebService = dsBuild.Tables[0].Rows[0]["rdp_mdt_ws"].ToString();
                                                    System.Net.NetworkCredential oCredentials = new System.Net.NetworkCredential(oVariable.ADUser(), oVariable.ADPassword(), oVariable.Domain());
                                                    BuildSubmit oMDT = new BuildSubmit();
                                                    oMDT.Credentials = oCredentials;
                                                    oMDT.Url = strRDPMDTWebService;
                                                    oLog.AddEvent(intAnswer, strName, strSerial, "Cleaning MDT on " + strRDPMDTWebService + " (" + strName + ", " + strMACAddress + ", " + "ServerShare" + ")", LoggingType.Information);
                                                    oMDT.Cleanup(strName, strMACAddress, "ServerShare");
                                                    oLog.AddEvent(intAnswer, strName, strSerial, "MDT has been cleared", LoggingType.Information);
                                                }
                                                else
                                                {
                                                    strError = "Invalid build location (RDP) configuration ~ Missing RDP Config...Class: " + oClass.Get(intClass, "name") + ", Environment: " + oEnvironment.Get(intEnv, "name") + ", Location: " + oLocation.GetFull(intAddress);
                                                    oLog.AddEvent(intAnswer, strName, strSerial, strError, LoggingType.Error);
                                                }
                                            }
                                            if (boolRDPAltiris)
                                            {
                                                if (dsBuild.Tables[0].Rows[0]["rdp_computer_ws"].ToString() != "" && dsBuild.Tables[0].Rows[0]["rdp_schedule_ws"].ToString() != "")
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
                                                        oLog.AddEvent(intAnswer, strName, strSerial, "Finished Deleting Altiris Computer Object", LoggingType.Information);
                                                    }
                                                    else
                                                        oLog.AddEvent(intAnswer, strName, strSerial, "Finished Deleting Altiris Computer Object (Did not exist)", LoggingType.Information);
                                                }
                                                else
                                                {
                                                    strError = "Invalid build location (RDP) configuration ~ Missing RDP Config...Class: " + oClass.Get(intClass, "name") + ", Environment: " + oEnvironment.Get(intEnv, "name") + ", Location: " + oLocation.GetFull(intAddress);
                                                    oLog.AddEvent(intAnswer, strName, strSerial, strError, LoggingType.Error);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            strError = "Invalid build location (RDP) configuration ~ Class: " + oClass.Get(intClass, "name") + ", Environment: " + oEnvironment.Get(intEnv, "name") + ", Location: " + oLocation.GetFull(intAddress);
                                            oLog.AddEvent(intAnswer, strName, strSerial, strError, LoggingType.Error);
                                        }
                                        if (strError == "" && oOperatingSystem.IsLinux(intOS) == true)
                                        {
                                        }
                                        AddResult(intServer, intStep, intType, oOnDemand.GetStep(intStepID, "done"), strError);
                                    }
                                    else if ((oSpan.Hours > 2 || dsZeusError.Tables[0].Rows.Count > 0) && boolZeusError == false)
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
                                            if (intClusterDell == 1)
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
                                        bool boolActuallyOK = false;
                                        if (dsZeusError.Tables[0].Rows.Count > 0)
                                        {
                                            strError = "IMAGING ERROR: " + dsZeusError.Tables[0].Rows[0]["message"].ToString();
                                            oLog.AddEvent(intAnswer, strName, strSerial, strError, LoggingType.Error);
                                            strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_PROVISIONING_SUPPORT");
                                            if (intEnvironment == (int)CurrentEnvironment.CORPDMN || intEnvironment == (int)CurrentEnvironment.PNCNT_PROD)
                                                oFunction.SendEmail("Auto-Provisioning " + strCodeType + " ERROR: " + strName, strCodeOwner, strImplementor, strEMailIdsBCC, "Auto-Provisioning " + strCodeType + " ERROR: " + strName, "<p><b>This message is to inform you that the server " + strName + " has encountered an error in " + strCodeType + "!</b><p><p>Error Message: " + strError + "<br/>Serial Number: " + oAsset.Get(intAsset, "serial").ToUpper() + "<br/>Asset Tag: " + oAsset.Get(intAsset, "asset").ToUpper() + "<br/>Model: " + oModelsProperties.Get(intModel, "name").ToUpper() + "<br/>Step #: " + intStep.ToString() + "<br/>Step: " + strStep + "<br/>DataCenter: " + oVMWare.DataCenter() + "<br/>Virtual Center: " + oVMWare.VirtualCenter() + "</p><p>Once you have resolved this issue, please <a href=\"" + oVariable.URL() + "/build_fixed.aspx?type=s&id=" + oFunction.encryptQueryString(intServer.ToString()) + "\" target=\"_blank\">click here</a> to resume the build.</p><p><b>NOTE:</b> This is automated email sent from the ClearView Auto-Provisioning tool. Please do not respond to this message.</p>", true, false);
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
                                                    oFunction.SendEmail("Auto-Provisioning " + strCodeType + " Problem: " + strName, strCodeOwner, strImplementor, strEMailIdsBCC, "Auto-Provisioning " + strCodeType + " Problem: " + strName, "<p><b>This message is to inform you that the server " + strName + " has been sitting at the " + strCodeType + " step for more than THREE (3) hours!</b><p><p>Serial Number: " + oAsset.Get(intAsset, "serial").ToUpper() + "<br/>Asset Tag: " + oAsset.Get(intAsset, "asset").ToUpper() + "<br/>Model: " + oModelsProperties.Get(intModel, "name").ToUpper() + "<br/>DataCenter: " + oVMWare.DataCenter() + "<br/>Virtual Center: " + oVMWare.VirtualCenter() + "</p><p>Once you have resolved this issue, please <a href=\"" + oVariable.URL() + "/build_fixed.aspx?type=s&id=" + oFunction.encryptQueryString(intServer.ToString()) + "\" target=\"_blank\">click here</a> to resume the build.</p><p><b>NOTE:</b> This is automated email sent from the ClearView Auto-Provisioning tool. Please do not respond to this message.</p>", true, false);
                                            }
                                        }
                                        if (boolActuallyOK == false)
                                            oServer.UpdateZeusError(intServer, 1);
                                        // Add Error message so it displays in provisioning windows
                                        AddResult(intServer, intStep, intType, "", strError);
                                    }
                                    break;
                                case 17:    // Create Active Directory Accounts
                                    oLog.AddEvent(intAnswer, strName, strSerial, "Starting Step " + intStep.ToString() + " (Create AD Accounts)", LoggingType.Information);
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
                                        oLog.AddEvent(intAnswer, strName, strSerial, "Active Directory accounts can only be assigned for distributed builds - " + strName, LoggingType.Information);
                                        strResult = "Active Directory Account Configuration Only Available for Distributed Builds";
                                    }
                                    oLog.AddEvent(intAnswer, strName, strSerial, strResult, LoggingType.Information);
                                    AddResult(intServer, intStep, intType, strResult, strError);
                                    break;
                                case 18:    // Assign IP Address(es)
                                    oLog.AddEvent(intAnswer, strName, strSerial, "Starting Step " + intStep.ToString() + " (Assign IP Address(es))", LoggingType.Information);
                                    bool boolDHCP = true;
                                    string strDeviceName = "Primary";
                                    if (boolPNC == true)
                                        strDeviceName = "PNCNet";
                                    DataSet dsDNS = oDomain.GetClassDNS(intDomain, intClass, intAddress);
                                    if (intIPAddress > 0)
                                    {
                                        // Check to see if it was already assigned from a previous attempt that errored.
                                        string strIPChange = oIPAddresses.GetName(intIPAddress, 0);
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
                                            if (strIP != "")
                                            {
                                                if (oOperatingSystem.IsWindows(intOS) == true || oOperatingSystem.IsWindows2008(intOS) == true)
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
                                                            int intNetwork = Int32.Parse(oIPAddresses.Get(intIPAddress, "networkid"));
                                                            /*
                                                            // Build DNS Suffix list
                                                            string strSuffix = "";
                                                            DataSet dsSuffix = oDomain.GetSuffixs(intDomain, 1);
                                                            foreach (DataRow drSuffix in dsSuffix.Tables[0].Rows)
                                                            {
                                                                if (strSuffix != "")
                                                                    strSuffix += ",";
                                                                strSuffix += "\"" + drSuffix["name"].ToString() + "\"";
                                                            }

                                                            // Build DNS Servers
                                                            List<string> strServersDNS = new List<string>();
                                                            if (dsDNS.Tables[0].Rows[0]["dns_ip1"].ToString() != "")
                                                                strServersDNS.Add(dsDNS.Tables[0].Rows[0]["dns_ip1"].ToString());
                                                            if (dsDNS.Tables[0].Rows[0]["dns_ip2"].ToString() != "")
                                                                strServersDNS.Add(dsDNS.Tables[0].Rows[0]["dns_ip2"].ToString());
                                                            if (dsDNS.Tables[0].Rows[0]["dns_ip3"].ToString() != "")
                                                                strServersDNS.Add(dsDNS.Tables[0].Rows[0]["dns_ip3"].ToString());
                                                            if (dsDNS.Tables[0].Rows[0]["dns_ip4"].ToString() != "")
                                                                strServersDNS.Add(dsDNS.Tables[0].Rows[0]["dns_ip4"].ToString());

                                                            string command = "Serverprocessing.ps1 –AnswerID " + intAnswer.ToString() + " –ServerNumber " + intNumber.ToString() + " –Environment \"" + ScriptEnvironment + "\" –IPAddressToConnect \"" + strIP + "\" –ConfigureIPAddress true –ShutdownAfterNetworkConfiguration  true –ConfigureDNS true –ConfigureDNSServers true –DNSServerAddressList " + strServersDNS.ToArray() + " –ConfigureDNSSuffix –ConnectionSpecificSuffix \"pncbank.com\" –ConfigureDNSRegistration true –SetRegisterThisConnectionAddress false –SetUseSuffixWhenRegistering false –MACAddressToConfigure \"" + strMACAddress + "\" -Log";
                                                            oLog.AddEvent(intAnswer, strName, strSerial, "Executing IP address / DNS script (" + command + ")...", LoggingType.Debug);

                                                            try
                                                            {
                                                                List<PowershellParameter> powershell = new List<PowershellParameter>();
                                                                NCC.ClearView.Application.Core.Powershell oPowershell = new NCC.ClearView.Application.Core.Powershell();
                                                                powershell.Add(new PowershellParameter("AnswerID", intAnswer));
                                                                powershell.Add(new PowershellParameter("ServerNumber", intNumber));
                                                                powershell.Add(new PowershellParameter("Environment", ScriptEnvironment));
                                                                powershell.Add(new PowershellParameter("IPAddressToConnect", strIP));
                                                                powershell.Add(new PowershellParameter("ConfigureIPAddress", true));
                                                                powershell.Add(new PowershellParameter("ShutDownAfterNetworkConfiguration", true));
                                                                powershell.Add(new PowershellParameter("ConfigureDNS", true));
                                                                powershell.Add(new PowershellParameter("ConfigureDNSServers", true));
                                                                powershell.Add(new PowershellParameter("DNSServerAddressList", strServersDNS));
                                                                powershell.Add(new PowershellParameter("ConfigureDNSSuffix", true));
                                                                powershell.Add(new PowershellParameter("ConnectionSpecificSuffix", "pncbank.com"));
                                                                powershell.Add(new PowershellParameter("ConfigureDNSRegistration", true));
                                                                powershell.Add(new PowershellParameter("SetRegisterThisConnectionAddress", false));
                                                                powershell.Add(new PowershellParameter("SetUseSuffixWhenRegistering", false));
                                                                powershell.Add(new PowershellParameter("MACAddressToConfigure", strMACAddress));
                                                                powershell.Add(new PowershellParameter("Log", true));
                                                                List<PowershellParameter> results = oPowershell.Execute(strScripts + "\\Serverprocessing.ps1", powershell, oLog, strName.ToString());
                                                                oLog.AddEvent(intAnswer, strName, strSerial, "Powershell IP address / DNS script completed!", LoggingType.Debug);
                                                                bool PowerShellError = false;
                                                                foreach (PowershellParameter result in results)
                                                                {
                                                                    oLog.AddEvent(intAnswer, strName, strSerial, "PSOBJECT: " + result.Name + " = " + result.Value, LoggingType.Information);
                                                                    if (result.Name == "ResultCode" && result.Value.ToString() != "0")
                                                                        PowerShellError = true;
                                                                    else if (result.Name == "Message" && PowerShellError)
                                                                        strError = result.Value.ToString();
                                                                }
                                                            }
                                                            catch (Exception exPowershell)
                                                            {
                                                                strError = exPowershell.Message;
                                                                Exception exPowershellInner = exPowershell.InnerException;
                                                                while (exPowershellInner != null)
                                                                {
                                                                    strError += " ~ " + exPowershellInner.Message;
                                                                    exPowershellInner = exPowershellInner.InnerException;
                                                                }
                                                                strError = "PowerShell Execution Error = " + strError + " (Source: " + exPowershell.Source + ") (Stack Trace: " + exPowershell.StackTrace + ")";
                                                            }


                                                            if (strError == "")
                                                            {
                                                                oLog.AddEvent(intAnswer, strName, strSerial, "Waiting three minutes...", LoggingType.Debug);
                                                                Thread.Sleep(180000);   // Wait 3 minutes
                                                                oLog.AddEvent(intAnswer, strName, strSerial, "Wait is done!", LoggingType.Debug);
                                                            }
                                                            */

                                                            // RESTORE IP CODE - START
                                                            // 1st part - create VBS file to copy to server
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
                                                            StreamWriter oRebootIP1 = new StreamWriter(strRebootVBS);
                                                            oRebootIP1.WriteLine("Set OpSysSet = GetObject(\"winmgmts:{impersonationLevel=impersonate,(Shutdown)}\").ExecQuery(\"select * from Win32_OperatingSystem where Primary=true\")");
                                                            oRebootIP1.WriteLine("For Each OpSys In OpSysSet");
                                                            oRebootIP1.WriteLine("Debug \"OpSys.Win32Shutdown return: \" & OpSys.Win32Shutdown(5)");
                                                            oRebootIP1.WriteLine("Next");
                                                            oRebootIP1.Flush();
                                                            oRebootIP1.Close();

                                                            // 2nd part - create batch file
                                                            string strBatchIP1 = strScripts + strSub + intAnswer.ToString() + "_" + strNow + "_ip.bat";
                                                            StreamWriter oWriterIP2 = new StreamWriter(strBatchIP1);

                                                            if (oOperatingSystem.IsWindows2008(intOS) == true)
                                                            {
                                                                oWriterIP2.WriteLine("netsh interface ipv4 set address name=\"" + strDeviceName + "\" source=static address=" + strIPChange + " mask=" + oIPAddresses.GetNetwork(intNetwork, "mask") + " gateway=" + oIPAddresses.GetNetwork(intNetwork, "gateway"));
                                                                // Wait 5 seconds to finish
                                                                oWriterIP2.WriteLine("ping 1.1.1.1 -n 5 -w 1000");
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
                                                                oWriterIP2.WriteLine("netsh interface ip set address name=\"" + strDeviceName + "\" source=static addr=" + strIPChange + " mask=" + oIPAddresses.GetNetwork(intNetwork, "mask"));
                                                                // Wait 5 seconds to finish
                                                                oWriterIP2.WriteLine("ping 1.1.1.1 -n 5 -w 1000");
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
                                                            oWriterIP2.Flush();
                                                            oWriterIP2.Close();
                                                            // 3rd part - create batch file
                                                            string strBatchIP2 = strScripts + strSub + intAnswer.ToString() + "_" + strNow + "_ip_2.bat";
                                                            StreamWriter oWriterIP3 = new StreamWriter(strBatchIP2);
                                                            oWriterIP3.WriteLine("F:");
                                                            oWriterIP3.WriteLine("cd " + strScripts + strSub);
                                                            oWriterIP3.WriteLine("net use \\\\" + strIP + "\\C$ /user:" + strAdminUser + " " + strAdminPass + "");
                                                            oWriterIP3.WriteLine("mkdir \\\\" + strIP + "\\C$\\OPTIONS");
                                                            oWriterIP3.WriteLine("copy " + strFile + " \\\\" + strIP + "\\C$\\OPTIONS\\CV_IP.VBS");
                                                            oWriterIP3.WriteLine("copy " + strRebootVBS + " \\\\" + strIP + "\\C$\\OPTIONS\\CV_IP_SHUTDOWN.VBS");
                                                            oWriterIP3.WriteLine("copy " + strBatchIP1 + " \\\\" + strIP + "\\C$\\OPTIONS\\CV_IP.BAT");
                                                            oWriterIP3.Flush();
                                                            oWriterIP3.Close();
                                                            // 4th part - run the batch file to perform copy
                                                            string strFileIP2Out = strScripts + strSub + intAnswer.ToString() + "_" + strNow + "_ip_2.txt";
                                                            ProcessStartInfo infoIPcopy = new ProcessStartInfo(strScripts + "psexec");
                                                            infoIPcopy.WorkingDirectory = strScripts;
                                                            if (oOperatingSystem.IsWindows2008(intOS) == true)
                                                            {
                                                                // Have to remove the "-i" parameter or else the script won't run (will come up with "the following helper dll cannot be loaded...XXXX.DLL" error)
                                                                infoIPcopy.Arguments = "-h cmd.exe /c " + strBatchIP2 + " > " + strFileIP2Out;
                                                            }
                                                            else
                                                                infoIPcopy.Arguments = "-i cmd.exe /c " + strBatchIP2 + " > " + strFileIP2Out;
                                                            // 11/18/2010 : Change IP Code
                                                            /*
                                                            infoIPcopy.Arguments = "-i cmd.exe /c \"" + strBatchIP2 + " > " + strFileIP2Out + "\"";
                                                            */
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
                                                                    if (oOperatingSystem.IsWindows2008(intOS) == true)
                                                                    {
                                                                        // Have to remove the "-i" parameter or else the script won't run (will come up with "the following helper dll cannot be loaded...XXXX.DLL" error)
                                                                        info.Arguments = "\\\\" + strIP + " -u " + strAdminUser + " -p " + strAdminPass + " -h cmd.exe /c C:\\OPTIONS\\CV_IP.BAT shutdown >C:\\OPTIONS\\CV_IP.TXT 2>&1";
                                                                        oLog.AddEvent(intAnswer, strName, strSerial, "PSEXEC Script = " + "\\\\" + strIP + " -u " + strAdminUser + " -p ***** -h cmd.exe /c C:\\OPTIONS\\CV_IP.BAT shutdown >C:\\OPTIONS\\CV_IP.TXT 2>&1", LoggingType.Information);
                                                                    }
                                                                    else
                                                                    {
                                                                        info.Arguments = "\\\\" + strIP + " -u " + strAdminUser + " -p " + strAdminPass + " -i cmd.exe /c C:\\OPTIONS\\CV_IP.BAT shutdown >C:\\OPTIONS\\CV_IP.TXT 2>&1";
                                                                        oLog.AddEvent(intAnswer, strName, strSerial, "PSEXEC Script = " + "\\\\" + strIP + " -u " + strAdminUser + " -p ***** -i cmd.exe /c C:\\OPTIONS\\CV_IP.BAT shutdown >C:\\OPTIONS\\CV_IP.TXT 2>&1", LoggingType.Information);
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
                                                            // RESTORE IP CODE - END
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

                                                if (strError == "")
                                                {
                                                    // Change VLAN of Network Adapter
                                                    if (strVLAN == "")
                                                    {
                                                        DataSet dsIP = oServer.GetIP(intServer, 1, 0, 0, 0);
                                                        foreach (DataRow drIP in dsIP.Tables[0].Rows)
                                                        {
                                                            int intIPAddressAssignFIX = 0;
                                                            if (dsIPBuild.Tables[0].Rows.Count > 0)
                                                                intIPAddressAssignFIX = Int32.Parse(dsIPBuild.Tables[0].Rows[0]["ipaddressid"].ToString());
                                                            oLog.AddEvent(intAnswer, strName, strSerial, "FIXING Auto-Assign IP Address:" + oIPAddresses.GetName(intIPAddressAssignFIX, 0), LoggingType.Information);
                                                            int intNetwork = Int32.Parse(oIPAddresses.Get(intIPAddressAssignFIX, "networkid"));
                                                            if (intNetwork > 0)
                                                            {
                                                                int intVLAN = Int32.Parse(oIPAddresses.GetNetwork(intNetwork, "vlanid"));
                                                                if (intVLAN > 0)
                                                                {
                                                                    DataSet dsVlan = oVMWare.GetVlanAssociations(intVLAN, intCluster);
                                                                    if (dsVlan.Tables[0].Rows.Count > 0)
                                                                    {
                                                                        int intVMWareVLAN = Int32.Parse(dsVlan.Tables[0].Rows[0]["vmware_vlanid"].ToString());
                                                                        oVMWare.UpdateGuestVlan(strName, intVMWareVLAN);
                                                                        strVLAN = oVMWare.GetVlan(intVMWareVLAN, "name");
                                                                    }
                                                                    else
                                                                    {
                                                                        strError = "There are no VMware associations ~ VLAN " + oIPAddresses.GetVlan(intVLAN, "vlan");
                                                                        oLog.AddEvent(intAnswer, strName, strSerial, strError, LoggingType.Error);
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    strError = "Invalid VLAN";
                                                                    oLog.AddEvent(intAnswer, strName, strSerial, strError, LoggingType.Error);
                                                                }
                                                            }
                                                            else
                                                            {
                                                                strError = "Invalid Network";
                                                                oLog.AddEvent(intAnswer, strName, strSerial, strError, LoggingType.Error);
                                                            }
                                                        }
                                                    }
                                                }

                                                if (strError == "")
                                                {
                                                    ManagedObjectReference _vm_net2 = oVMWare.GetVM(strName);
                                                    if (strVLAN != "")
                                                    {
                                                        VirtualMachineRuntimeInfo run_vlan = (VirtualMachineRuntimeInfo)oVMWare.getObjectProperty(_vm_net2, "runtime");
                                                        if (run_vlan.powerState != VirtualMachinePowerState.poweredOff)
                                                        {
                                                            if (boolDHCP == false)
                                                            {
                                                                // The DHCP address did not reply and the server is not powered off...something is wrong here!
                                                                strError = "The DHCP address did not reply and the server is not powered off ~ DHCP: " + strIP;
                                                            }
                                                            else
                                                            {
                                                                oLog.AddEvent(intAnswer, strName, strSerial, "Waiting for script to power down the guest", LoggingType.Information);
                                                                // Wait 20 seconds for IP script to finish (hopefully) before shutting down the guest.
                                                                int intAttempt = 0;
                                                                for (intAttempt = 0; intAttempt < 20 && run_vlan.powerState != VirtualMachinePowerState.poweredOff; intAttempt++)
                                                                {
                                                                    run_vlan = (VirtualMachineRuntimeInfo)oVMWare.getObjectProperty(_vm_net2, "runtime");
                                                                    int intAttemptLeft = (20 - intAttempt);
                                                                    oLog.AddEvent(intAnswer, strName, strSerial, "Guest still powered on (script)...waiting 3 seconds (" + intAttemptLeft.ToString() + " tries left)", LoggingType.Debug);
                                                                    Thread.Sleep(3000);
                                                                }
                                                                if (run_vlan.powerState != VirtualMachinePowerState.poweredOff)
                                                                {
                                                                    // Shutdown guest os cleanly
                                                                    oLog.AddEvent(intAnswer, strName, strSerial, "Shutting down guest OS to change VLAN", LoggingType.Information);
                                                                    intAttempt = 0;
                                                                    try
                                                                    {
                                                                        _service.ShutdownGuest(_vm_net2);
                                                                        // Wait 3 seconds, and then check status
                                                                        Thread.Sleep(3000);
                                                                        for (intAttempt = 0; intAttempt < 20 && run_vlan.powerState != VirtualMachinePowerState.poweredOff; intAttempt++)
                                                                        {
                                                                            run_vlan = (VirtualMachineRuntimeInfo)oVMWare.getObjectProperty(_vm_net2, "runtime");
                                                                            int intAttemptLeft = (20 - intAttempt);
                                                                            oLog.AddEvent(intAnswer, strName, strSerial, "Guest still powered on (" + run_vlan.powerState.ToString() + ")...waiting 3 seconds (" + intAttemptLeft.ToString() + " tries left)", LoggingType.Debug);
                                                                            Thread.Sleep(3000);
                                                                        }
                                                                    }
                                                                    catch (Exception exVlan)
                                                                    {
                                                                        // Got a VMWARE TOOLS error message when trying to run the ShutdownGuest Command on ESX 3.5 (since upgraded the DLLs to 4.0)
                                                                        oLog.AddEvent(intAnswer, strName, strSerial, "There was an error shutting down guest OS to change VLAN ~ " + exVlan.Message, LoggingType.Warning);
                                                                        run_vlan = (VirtualMachineRuntimeInfo)oVMWare.getObjectProperty(_vm_net2, "runtime");
                                                                    }
                                                                }
                                                                if (run_vlan.powerState != VirtualMachinePowerState.poweredOff)
                                                                {
                                                                    oLog.AddEvent(intAnswer, strName, strSerial, "Guest Still Not Powered Off (" + run_vlan.powerState.ToString() + ")", LoggingType.Information);
                                                                    // Guest is still on, let's try forcing it to power off
                                                                    ManagedObjectReference _task_shutdown = _service.PowerOffVM_Task(_vm_net2);
                                                                    oLog.AddEvent(intAnswer, strName, strSerial, "Guest Power OFF Task Started", LoggingType.Information);
                                                                    TaskInfo _info_shutdown = (TaskInfo)oVMWare.getObjectProperty(_task_shutdown, "info");
                                                                    while (_info_shutdown.state == TaskInfoState.running)
                                                                        _info_shutdown = (TaskInfo)oVMWare.getObjectProperty(_task_shutdown, "info");
                                                                    if (_info_shutdown.state == TaskInfoState.success)
                                                                    {
                                                                        for (intAttempt = 0; intAttempt < 20 && run_vlan.powerState != VirtualMachinePowerState.poweredOff; intAttempt++)
                                                                        {
                                                                            run_vlan = (VirtualMachineRuntimeInfo)oVMWare.getObjectProperty(_vm_net2, "runtime");
                                                                            int intAttemptLeft = (20 - intAttempt);
                                                                            oLog.AddEvent(intAnswer, strName, strSerial, "Guest still powered on (powerOff)...waiting 3 seconds (" + intAttemptLeft.ToString() + " tries left)", LoggingType.Debug);
                                                                            Thread.Sleep(3000);
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        else
                                                            oLog.AddEvent(intAnswer, strName, strSerial, "Guest OS was already shutdown (" + run_vlan.powerState.ToString() + ")", LoggingType.Information);

                                                        if (strError == "")
                                                        {
                                                            if (run_vlan.powerState != VirtualMachinePowerState.poweredOff)
                                                            {
                                                                strError = "There was a problem shutting down the guest";
                                                                oLog.AddEvent(intAnswer, strName, strSerial, strError, LoggingType.Error);
                                                            }
                                                            else
                                                            {
                                                                oLog.AddEvent(intAnswer, strName, strSerial, "Guest OS shutdown Finished", LoggingType.Information);
                                                                // Wait 10 seconds (VMware still errors if executed right away)
                                                                Thread.Sleep(10000);
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
                                                                            oLog.AddEvent(intAnswer, strName, strSerial, "Network Adapter Changing to VLAN:" + strVLAN, LoggingType.Information);
                                                                            ManagedObjectReference _task_net = _service.ReconfigVM_Task(_vm_net2, vmconfigspec);
                                                                            TaskInfo _info_net = (TaskInfo)oVMWare.getObjectProperty(_task_net, "info");
                                                                            while (_info_net.state == TaskInfoState.running)
                                                                                _info_net = (TaskInfo)oVMWare.getObjectProperty(_task_net, "info");
                                                                            if (_info_net.state == TaskInfoState.success)
                                                                                oLog.AddEvent(intAnswer, strName, strSerial, "Network Adapter Reconfigured", LoggingType.Information);
                                                                            else
                                                                            {
                                                                                strError = "Network Adapter NOT Reconfigured";
                                                                                oLog.AddEvent(intAnswer, strName, strSerial, strError, LoggingType.Error);
                                                                            }
                                                                        }
                                                                        if (intClusterVersion == 1)
                                                                        {
                                                                            bool boolCompleted = false;
                                                                            string strPortGroupKey = "";
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
                                                                                                oLog.AddEvent(intAnswer, strName, strSerial, "Trying..." + strPortGroupKey + "(" + strSwitchUUID + ")", LoggingType.Information);

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
                                                                                                oLog.AddEvent(intAnswer, strName, strSerial, "Network Adapter Changing to VLAN:" + strVLAN, LoggingType.Information);
                                                                                                ManagedObjectReference _task_net = _service.ReconfigVM_Task(_vm_net2, vmconfigspec);
                                                                                                TaskInfo _info_net = (TaskInfo)oVMWare.getObjectProperty(_task_net, "info");
                                                                                                while (_info_net.state == TaskInfoState.running)
                                                                                                    _info_net = (TaskInfo)oVMWare.getObjectProperty(_task_net, "info");
                                                                                                if (_info_net.state == TaskInfoState.success)
                                                                                                {
                                                                                                    boolCompleted = true;
                                                                                                    oLog.AddEvent(intAnswer, strName, strSerial, "Network Adapter Reconfigured", LoggingType.Information);
                                                                                                }
                                                                                                else
                                                                                                {
                                                                                                    strError = "Network Adapter NOT Reconfigured";
                                                                                                    oLog.AddEvent(intAnswer, strName, strSerial, strError, LoggingType.Error);
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
                                                                            if (boolCompleted == false)
                                                                                strError = "Network Adapter Was Not Created ~ Could not find a port group (" + strVLAN + ")";
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        strError = "No VLAN found for the guest";
                                                        oLog.AddEvent(intAnswer, strName, strSerial, strError, LoggingType.Error);
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
                                                            oLog.AddEvent(intAnswer, strName, strSerial, "Boot retry turned off", LoggingType.Information);
                                                        else
                                                        {
                                                            strError = "Boot retry NOT turned off";
                                                            oLog.AddEvent(intAnswer, strName, strSerial, strError, LoggingType.Error);
                                                        }
                                                    }

                                                    if (strError == "")
                                                    {
                                                        // Turn on the guest if it is off
                                                        oLog.AddEvent(intAnswer, strName, strSerial, "Starting Virtual Machine Power On...", LoggingType.Information);
                                                        VirtualMachineRuntimeInfo run_net2 = (VirtualMachineRuntimeInfo)oVMWare.getObjectProperty(_vm_net2, "runtime");
                                                        if (run_net2.powerState == VirtualMachinePowerState.poweredOff)
                                                        {
                                                            ManagedObjectReference _task_power = _service.PowerOnVM_Task(_vm_net2, null);
                                                            TaskInfo _info_power = (TaskInfo)oVMWare.getObjectProperty(_task_power, "info");
                                                            while (_info_power.state == TaskInfoState.running)
                                                                _info_power = (TaskInfo)oVMWare.getObjectProperty(_task_power, "info");
                                                            if (_info_power.state == TaskInfoState.success)
                                                                oLog.AddEvent(intAnswer, strName, strSerial, "Virtual Machine Powering On", LoggingType.Information);
                                                            else
                                                            {
                                                                // Wait a few seconds, then try again...
                                                                Thread.Sleep(10000);
                                                                ManagedObjectReference _task_power2 = _service.PowerOnVM_Task(_vm_net2, null);
                                                                TaskInfo _info_power2 = (TaskInfo)oVMWare.getObjectProperty(_task_power2, "info");
                                                                while (_info_power2.state == TaskInfoState.running)
                                                                    _info_power2 = (TaskInfo)oVMWare.getObjectProperty(_task_power2, "info");
                                                                if (_info_power2.state == TaskInfoState.success)
                                                                    oLog.AddEvent(intAnswer, strName, strSerial, "Virtual Machine Powering On", LoggingType.Information);
                                                                else
                                                                {
                                                                    strError = "There was a problem powering on the virtual machine after changing network adapter settings";
                                                                    oLog.AddEvent(intAnswer, strName, strSerial, "Virtual Machine was NOT Powered On", LoggingType.Warning);
                                                                }
                                                            }
                                                        }
                                                        else
                                                            oLog.AddEvent(intAnswer, strName, strSerial, "Virtual Machine was already Powered On", LoggingType.Information);

                                                        if (strError == "")
                                                        {
                                                            int intAttempt = 0;
                                                            for (intAttempt = 0; intAttempt < 60 && run_net2.powerState == VirtualMachinePowerState.poweredOff; intAttempt++)
                                                            {
                                                                run_net2 = (VirtualMachineRuntimeInfo)oVMWare.getObjectProperty(_vm_net2, "runtime");
                                                                int intAttemptLeft = (60 - intAttempt);
                                                                oLog.AddEvent(intAnswer, strName, strSerial, "Server still starting...waiting 5 seconds (" + intAttemptLeft.ToString() + " tries left)", LoggingType.Debug);
                                                                Thread.Sleep(5000);
                                                            }
                                                            if (run_net2.powerState == VirtualMachinePowerState.poweredOff)
                                                            {
                                                                strError = "There was a problem turning on the guest";
                                                                oLog.AddEvent(intAnswer, strName, strSerial, strError, LoggingType.Error);
                                                            }
                                                            else
                                                            {
                                                                oLog.AddEvent(intAnswer, strName, strSerial, "Attempting to ping the newly assigned address [" + strIPChange + "]", LoggingType.Information);
                                                                // Wait 5 seconds and then ping new address
                                                                bool boolPinged = false;
                                                                for (int ii = 0; ii < 10 && boolPinged == false; ii++)
                                                                {
                                                                    Thread.Sleep(5000);
                                                                    Ping oPing = new Ping();
                                                                    string strStatus = "";
                                                                    try
                                                                    {
                                                                        PingReply oReply = oPing.Send(strIPChange);
                                                                        strStatus = oReply.Status.ToString().ToUpper();
                                                                    }
                                                                    catch { }
                                                                    boolPinged = (strStatus == "SUCCESS");
                                                                }
                                                                if (boolPinged == true)
                                                                {
                                                                    oLog.AddEvent(intAnswer, strName, strSerial, "Successfully pinged the newly assigned IP address (" + strIPChange + ")", LoggingType.Information);
                                                                    //if (intTSM == 0)
                                                                    //{
                                                                    //    boolPinged = false;
                                                                    //    for (int ii = 0; ii < 10 && boolPinged == false; ii++)
                                                                    //    {
                                                                    //        Thread.Sleep(5000);
                                                                    //        Ping oPing = new Ping();
                                                                    //        string strStatus = "";
                                                                    //        try
                                                                    //        {
                                                                    //            PingReply oReply = oPing.Send(oIPAddresses.GetName(intIPAddressAvamar, 0));
                                                                    //            strStatus = oReply.Status.ToString().ToUpper();
                                                                    //        }
                                                                    //        catch { }
                                                                    //        boolPinged = (strStatus == "SUCCESS");
                                                                    //    }
                                                                    //}
                                                                    if (intTSM == 0 || boolPinged == true)
                                                                    {
                                                                        oLog.AddEvent(intAnswer, strName, strSerial, "Running cleanup scripts...", LoggingType.Information);
                                                                        // Run cleanup scripts...
                                                                        // WINDOWS2008DONE (uncomment next two lines, and then the /* shown before the } bracket)
                                                                        //if (oOperatingSystem.IsWindows(intOS) == true)
                                                                        //{
                                                                        // 11/18/2010 : Change IP Code
                                                                        /* Comment 6th part
                                                                        */
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
                                                                        /*
                                                                        oWriterIP5.WriteLine("del \\\\" + strIPChange + "\\C$\\OPTIONS\\CV_IP.BAT");
                                                                        oWriterIP5.WriteLine("del \\\\" + strIPChange + "\\C$\\OPTIONS\\CV_IP.VBS");
                                                                        oWriterIP5.WriteLine("del \\\\" + strIPChange + "\\C$\\OPTIONS\\CV_IP_EXECUTE.BAT");
                                                                        oWriterIP5.WriteLine("del \\\\" + strIPChange + "\\C$\\OPTIONS\\CV_IP.EXE");
                                                                        */
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
                                                                            infoIPdelete.Arguments = "-h cmd.exe /c " + strBatchIP3;
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
                                                                        /*
                                                                        }
                                                                        else if (oOperatingSystem.IsWindows2008(intOS) == true)
                                                                        {
                                                                            //schtasks /delete /s 10.48.54.87 /tn CV_SCRIPT /f
                                                                            string strBatchIP5 = strScripts + strSub + intAnswer.ToString() + "_" + strNow + "_ip_4.bat";
                                                                            StreamWriter oWriterIP6 = new StreamWriter(strBatchIP5);
                                                                            oWriterIP6.WriteLine("schtasks /delete /s " + strIPChange + " /tn CV_SCRIPT /f");
                                                                            oWriterIP6.Flush();
                                                                            oWriterIP6.Close();
                                                                            ProcessStartInfo info3 = new ProcessStartInfo(strScripts + "psexec");
                                                                            info3.WorkingDirectory = strScripts;
                                                                            info3.Arguments = " -i cmd.exe /c " + strBatchIP5;
                                                                            oLog.AddEvent(intAnswer, strName, strSerial, "SCHTASKS # 4 Script (Delete) Started", LoggingType.Information);
                                                                            Process proc3 = Process.Start(info3);
                                                                            proc3.WaitForExit(300000);   // Wait 5 minutes
                                                                            if (proc3.HasExited == false)
                                                                                proc3.Kill();
                                                                            int intReturnCode = proc3.ExitCode;
                                                                            proc3.Close();
                                                                            oLog.AddEvent(intAnswer, strName, strSerial, "SCHTASKS # 4 Script (Delete) Finished with code (" + intReturnCode.ToString() + ")", LoggingType.Information);

                                                                            //net use \\10.48.54.87\C$ /dele
                                                                            string strBatchIP6 = strScripts + strSub + intAnswer.ToString() + "_" + strNow + "_ip_4.bat";
                                                                            StreamWriter oWriterIP7 = new StreamWriter(strBatchIP6);
                                                                            oWriterIP7.WriteLine("del C:\\OPTIONS\\CV_STATUS.TXT");
                                                                            oWriterIP7.WriteLine("net use \\\\" + strIP + "\\C$ /dele");
                                                                            oWriterIP7.WriteLine("net use \\\\" + strIPChange + "\\C$ /dele");
                                                                            oWriterIP7.Flush();
                                                                            oWriterIP7.Close();
                                                                            ProcessStartInfo info4 = new ProcessStartInfo(strScripts + "psexec");
                                                                            info4.WorkingDirectory = strScripts;
                                                                            info4.Arguments = " -i cmd.exe /c " + strBatchIP6;
                                                                            oLog.AddEvent(intAnswer, strName, strSerial, "SCHTASKS # 5 Script (Cleanup) Started", LoggingType.Information);
                                                                            Process proc4 = Process.Start(info4);
                                                                            proc4.WaitForExit(300000);   // Wait 5 minutes
                                                                            if (proc4.HasExited == false)
                                                                                proc4.Kill();
                                                                            int intReturnCode4 = proc4.ExitCode;
                                                                            proc4.Close();
                                                                            oLog.AddEvent(intAnswer, strName, strSerial, "SCHTASKS # 5 Script (Cleanup) Finished with code (" + intReturnCode4.ToString() + ")", LoggingType.Information);
                                                                        }
                                                                        */
                                                                        strResult = oOnDemand.GetStep(intStepID, "done");
                                                                        //oLog.AddEvent(intAnswer, strName, strSerial, strResult, LoggingType.Information);
                                                                    }
                                                                    else
                                                                    {
                                                                        strError = "There was a problem assigning the IP Address ~ (ping for AVAMAR " + oIPAddresses.GetName(intIPAddressAvamar, 0) + " did not respond)";
                                                                        oLog.AddEvent(intAnswer, strName, strSerial, strError, LoggingType.Error);
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    strError = "There was a problem assigning the IP Address ~ (ping " + strIPChange + " did not respond)";
                                                                    oLog.AddEvent(intAnswer, strName, strSerial, strError, LoggingType.Error);
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            else if (intLogging > 0)
                                                oEventLog.WriteEntry(strName + " (VMware): " + "Waiting to assign IP Address ", EventLogEntryType.Warning);
                                        }
                                        else
                                        {
                                            strResult = oOnDemand.GetStep(intStepID, "done");
                                            oLog.AddEvent(intAnswer, strName, strSerial, "Server IP configuration has already been changed", LoggingType.Information);
                                        }
                                    }
                                    else
                                    {
                                        //if (dsDNS.Tables[0].Rows.Count > 0)
                                        //{
                                        //    if (oOperatingSystem.IsWindows(intOS) == true)
                                        //    {
                                        //        // Even though no IP was assigned, still assign the appropriate DNS entries
                                        //        oLog.AddEvent(intAnswer, strName, strSerial, "There was no IP Address allocated...Applying DNS/WINS configuration", LoggingType.Information);
                                        //        // 2nd part - create batch file
                                        //        string strBatchDNS1 = strScripts + strSub + intAnswer.ToString() + "_" + strNow + "_ip.bat";
                                        //        StreamWriter oWriterDNS2 = new StreamWriter(strBatchDNS1);
                                        //        if (dsDNS.Tables[0].Rows[0]["dns_ip1"].ToString() != "")
                                        //            oWriterDNS2.WriteLine("netsh interface ip set dns name=\"" + strDeviceName + "\" source=static addr=" + dsDNS.Tables[0].Rows[0]["dns_ip1"].ToString() + " register=PRIMARY");
                                        //        if (dsDNS.Tables[0].Rows[0]["dns_ip2"].ToString() != "")
                                        //            oWriterDNS2.WriteLine("netsh interface ip add dns name=\"" + strDeviceName + "\" addr=" + dsDNS.Tables[0].Rows[0]["dns_ip2"].ToString() + " index=2");
                                        //        if (dsDNS.Tables[0].Rows[0]["dns_ip3"].ToString() != "")
                                        //            oWriterDNS2.WriteLine("netsh interface ip add dns name=\"" + strDeviceName + "\" addr=" + dsDNS.Tables[0].Rows[0]["dns_ip3"].ToString() + " index=3");
                                        //        if (dsDNS.Tables[0].Rows[0]["dns_ip4"].ToString() != "")
                                        //            oWriterDNS2.WriteLine("netsh interface ip add dns name=\"" + strDeviceName + "\" addr=" + dsDNS.Tables[0].Rows[0]["dns_ip4"].ToString() + " index=4");
                                        //        if (dsDNS.Tables[0].Rows[0]["wins_ip1"].ToString() != "")
                                        //            oWriterDNS2.WriteLine("netsh interface ip set wins name=\"" + strDeviceName + "\" source=static addr=" + dsDNS.Tables[0].Rows[0]["wins_ip1"].ToString());
                                        //        if (dsDNS.Tables[0].Rows[0]["wins_ip2"].ToString() != "")
                                        //            oWriterDNS2.WriteLine("netsh interface ip add wins name=\"" + strDeviceName + "\" addr=" + dsDNS.Tables[0].Rows[0]["wins_ip2"].ToString() + " index=2");
                                        //        if (dsDNS.Tables[0].Rows[0]["wins_ip3"].ToString() != "")
                                        //            oWriterDNS2.WriteLine("netsh interface ip add wins name=\"" + strDeviceName + "\" addr=" + dsDNS.Tables[0].Rows[0]["wins_ip3"].ToString() + " index=3");
                                        //        if (dsDNS.Tables[0].Rows[0]["wins_ip4"].ToString() != "")
                                        //            oWriterDNS2.WriteLine("netsh interface ip add wins name=\"" + strDeviceName + "\" addr=" + dsDNS.Tables[0].Rows[0]["wins_ip4"].ToString() + " index=4");
                                        //        oWriterDNS2.Flush();
                                        //        oWriterDNS2.Close();
                                        //        // 3rd part - create batch file
                                        //        string strBatchDNS2 = strScripts + strSub + intAnswer.ToString() + "_" + strNow + "_ip_2.bat";
                                        //        StreamWriter oWriterDNS3 = new StreamWriter(strBatchDNS2);
                                        //        oWriterDNS3.WriteLine("F:");
                                        //        oWriterDNS3.WriteLine("cd " + strScripts + strSub);
                                        //        oWriterDNS3.WriteLine("net use \\\\" + strIP + "\\C$ /user:" + strAdminUser + " " + strAdminPass + "");
                                        //        oWriterDNS3.WriteLine("mkdir \\\\" + strIP + "\\C$\\OPTIONS");
                                        //        oWriterDNS3.WriteLine("copy " + strBatchDNS1 + " \\\\" + strIP + "\\C$\\OPTIONS\\CV_DNS.BAT");
                                        //        oWriterDNS3.Flush();
                                        //        oWriterDNS3.Close();
                                        //        // 4th part - run the batch file to perform copy
                                        //        string strFileDNS2Out = strScripts + strSub + intAnswer.ToString() + "_" + strNow + "_ip_2.txt";
                                        //        ProcessStartInfo infoIPdns = new ProcessStartInfo(strScripts + "psexec");
                                        //        infoIPdns.WorkingDirectory = strScripts;
                                        //        if (oOperatingSystem.IsWindows2008(intOS) == true)
                                        //        {
                                        //            // Have to remove the "-i" parameter or else the script won't run (will come up with "the following helper dll cannot be loaded...XXXX.DLL" error)
                                        //            infoIPdns.Arguments = "cmd.exe /c " + strBatchDNS2 + " > " + strFileDNS2Out;
                                        //        }
                                        //        else
                                        //            infoIPdns.Arguments = "-i cmd.exe /c " + strBatchDNS2 + " > " + strFileDNS2Out;
                                        //        bool boolDNSTimeout = false;
                                        //        Process procIPdns = Process.Start(infoIPdns);
                                        //        procIPdns.WaitForExit(intTimeoutDefault);
                                        //        if (procIPdns.HasExited == false)
                                        //        {
                                        //            procIPdns.Kill();
                                        //            boolDNSTimeout = true;
                                        //        }
                                        //        procIPdns.Close();
                                        //        if (boolDNSTimeout == false)
                                        //        {
                                        //            bool boolOutputDNS = ReadOutput(intServer, "DNS", strFileDNS2Out, strName, strSerial);
                                        //            if (boolOutputDNS == false)
                                        //            {
                                        //                // 5th part - file has been copied, do the PSEXEC to install application
                                        //                ProcessStartInfo info = new ProcessStartInfo(strScripts + "psexec");
                                        //                info.WorkingDirectory = strScripts;
                                        //                info.Arguments = "\\\\" + strIP + " -u " + strAdminUser + " -p " + strAdminPass + " -i cmd.exe /c C:\\OPTIONS\\CV_DNS.BAT >C:\\OPTIONS\\CV_DNS.TXT 2>&1";
                                        //                oLog.AddEvent(intAnswer, strName, strSerial, "PSEXEC Script = " + "\\\\" + strIP + " -u " + strAdminUser + " -p ***** -i cmd.exe /c C:\\OPTIONS\\CV_DNS.BAT >C:\\OPTIONS\\CV_DNS.TXT 2>&1", LoggingType.Information);
                                        //                System.Diagnostics.Process proc = System.Diagnostics.Process.Start(info);
                                        //                proc.WaitForExit(300000);     // Wait 5 minutes
                                        //                proc.Close();
                                        //                oLog.AddEvent(intAnswer, strName, strSerial, "PSEXEC Script Finished", LoggingType.Information);
                                        //            }
                                        //            else
                                        //                strError = "There was a problem with the NET USE script for DNS configuration";
                                        //        }
                                        //        else
                                        //            strError = "A timeout occurred while running the script for DNS configuration";
                                        //    }
                                            if (strError == "")
                                                strResult = "There was no IP Address allocated for this device";
                                        //}
                                        //else
                                        //    strError = "No DNS Servers have been configured for the domain, class and environment";
                                    }
                                    AddResult(intServer, intStep, intType, strResult, strError);
                                    break;
                                case 19:    // Add Local Admin Groups 
                                    oLog.AddEvent(intAnswer, strName, strSerial, "Starting Step " + intStep.ToString() + " (Add Local Admin Groups)", LoggingType.Information);
                                    if (oOperatingSystem.IsWindows(intOS) == true || oOperatingSystem.IsWindows2008(intOS) == true)
                                    {
                                        if (intIPAddress > 0)
                                            strIP = oIPAddresses.GetName(intIPAddress, 0);
                                        if (strIP != "")
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
                                            // WINDOWS2008CHANGE
                                            /*
                                            if (oOperatingSystem.IsWindows2008(intOS) == true)
                                            {
                                                //              strType         strRemoteFile               strRemoteFileOut        strExecutable
                                                // ------------------------------------------------------------------------------------------------------------------------------------
                                                // GROUPS       GROUPS          CV_GROUPS.VBS               NULL                    %WinDir%\System32\wscript.exe
                                                int intScriptGroup = ExecuteScheduledTask(intServer, "GROUPS", strFile, "CV_GROUPS.VBS", "", @"%WinDir%\System32\wscript.exe", strName, Environment.MachineName, strIP, strAdminUser, strAdminPass, 0);
                                                AuditStatus oScriptGroup = (AuditStatus)intScriptGroup;
                                                if (oScriptGroup == AuditStatus.Success)
                                                    strResult = oOnDemand.GetStep(intStepID, "done");
                                                else if (oScriptGroup == AuditStatus.NetUseError)
                                                    strError = "There was a problem configuring the groups (the NET USE script failed to map a drive)";
                                                else
                                                    strError = "There was an error configuring the groups ~ (Error # " + intScriptGroup.ToString() + ")";
                                            }
                                            else
                                            {
                                            */
                                                bool boolScriptGroupError = true;
                                                for (int kk = 0; kk < 5 && boolScriptGroupError == true; kk++)
                                                {
                                                    int intScriptGroup = oFunction.ExecuteVBScript(intServer, false, true, "GROUPS", strName, strSerial, strIP, strFile, strFilePath, "Groups", "%windir%\\system32\\wscript.exe", "OPTIONS\\CV_GROUPS", "VBS", "", strScripts, strAdminUser, strAdminPass, 5, (oOperatingSystem.IsWindows2008(intOS) == false), false, intLogging, boolDeleteScriptFiles);
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
                                            //}
                                        }
                                        else
                                            oLog.AddEvent(intAnswer, strName, strSerial, "Cannot resolve NAME or IP - local admins skipped", LoggingType.Warning);
                                    }
                                    else
                                    {
                                        oLog.AddEvent(intAnswer, strName, strSerial, "Local admin groups can only be assigned for distributed builds - " + strName, LoggingType.Information);
                                        strResult = "This step is only available for distributed servers";
                                    }
                                    AddResult(intServer, intStep, intType, strResult, strError);
                                    break;
                                case 20:    // Install Components 
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
                                case 21:    // Move to OU
                                    oLog.AddEvent(intAnswer, strName, strSerial, "Starting Step " + intStep.ToString() + " (Move to OU)", LoggingType.Information);
                                    if (boolPNC == true)
                                        strResult = "Currently, this step is not configured for PNC builds";
                                    else
                                    {
                                        //if (intDomainEnvironment == (int)CurrentEnvironment.CORPDEV || intDomainEnvironment == (int)CurrentEnvironment.CORPTEST || intDomainEnvironment == (int)CurrentEnvironment.ECADDEV)
                                        //{
                                        //    SearchResultCollection oResults = oAD.ComputerSearch(strName);
                                        //    for (int ii = 0; ii < 3 && oResults.Count != 1; ii++)
                                        //    {
                                        //        Thread.Sleep(4000);
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
                                    AddResult(intServer, intStep, intType, strResult, strError);
                                    break;
                                case 22:    // Audit
                                    if (intAuditCount < intAuditCounts)
                                    {
                                        oLog.AddEvent(intAnswer, strName, strSerial, "Starting Step " + intStep.ToString() + " (Audit)", LoggingType.Information);
                                        // RUN AUDIT TASKS
                                        AuditThread oAuditThread = new AuditThread(intServer, strName, strSerial, intIPAddress, true, strIP, intClass, intEnv, intModel, intOS, intSP, intAddress, false, boolIsClustering, (intTSM == 1 && oForecast.GetAnswer(intAnswer, "backup") == "1"), intStep, intRequest, intServerAuditErrorService, intResourceRequestApprove, intAssignPage, intViewPage, strScripts, strSub, strAdminUser, strAdminPass, intEnvironment, intLogging, dsn, dsnAsset, dsnIP, dsnServiceEditor, boolDeleteScriptFiles, boolMultiThreadedAudit, this, false);
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
                                case 23:    // File Cleanup and Notify
                                    // Now that the audit is complete, go ahead and decrease the audit count
                                    if (strBuildFolder != "")
                                    {
                                        oLog.AddEvent(intAnswer, strName, strSerial, "Starting Step " + intStep.ToString() + " (Move Guest out of Build Folder)", LoggingType.Information);
                                        ManagedObjectReference datacenterRef = oVMWare.GetDataCenter();
                                        ManagedObjectReference vmFolderRef = oVMWare.GetVMFolder(datacenterRef);
                                        ManagedObjectReference vmBuildIn = oVMWare.GetBuildFolder(vmFolderRef, 1);
                                        ManagedObjectReference _vm_move = oVMWare.GetVM(strName);
                                        // Move out of Build Folder
                                        ManagedObjectReference _task_move = _service.MoveIntoFolder_Task(vmFolderRef, new ManagedObjectReference[] { _vm_move });
                                        TaskInfo _info_move = (TaskInfo)oVMWare.getObjectProperty(_task_move, "info");
                                        while (_info_move.state == TaskInfoState.running)
                                            _info_move = (TaskInfo)oVMWare.getObjectProperty(_task_move, "info");
                                        if (_info_move.state == TaskInfoState.success)
                                            strResult = "Virtual Machine " + strName.ToUpper() + " moved out of " + strBuildFolder;
                                        else
                                        {
                                            strError = "Virtual Machine was NOT moved out of build folder ~ " + strBuildFolder;
                                            oLog.AddEvent(intAnswer, strName, strSerial, strError, LoggingType.Error);
                                        }
                                        oLog.AddEvent(intAnswer, strName, strSerial, "Finished moving guest out of build folder: " + strBuildFolder, LoggingType.Information);
                                    }
                                    if (strError == "")
                                    {
                                        oLog.AddEvent(intAnswer, strName, strSerial, "Starting Step " + intStep.ToString() + " (File Cleanup and Notify)", LoggingType.Information);
                                        try
                                        {
                                            if (boolPNC == true)
                                            {
                                                System.Net.NetworkCredential oCredentials = new System.Net.NetworkCredential(oVariable.ADUser(), oVariable.ADPassword(), oVariable.Domain());

                                                int intIPAddressDNS = intIPAddress;
                                                if (intIPAddressDNS == 0 && intIPAddressFinal > 0)
                                                    intIPAddressDNS = intIPAddressFinal;

                                                // Attempt to register in ServiceNow
                                                if (intIPAddressDNS > 0)
                                                {
                                                    strIP = oIPAddresses.GetName(intIPAddressDNS, 0);
                                                    ClearViewWebServices oServiceNow = new ClearViewWebServices();
                                                    oServiceNow.Timeout = Timeout.Infinite;
                                                    oServiceNow.Credentials = oCredentials;
                                                    oServiceNow.Url = oVariable.WebServiceURL();

                                                    System.Net.NetworkCredential oCredentialsSN = new System.Net.NetworkCredential(oVariable.ServiceNowUsername(), oVariable.ServiceNowPassword());
                                                    string url = oVariable.ServiceNowHost();
                                                    string domain = oVar.FullyQualified();
                                                    string mnemonic = strAppCode;
                                                    string os = oOperatingSystem.Get(intOS, "zeus_build_type");
                                                    string model = oModel.Get(intParent, "make") + " " + oModel.Get(intParent, "name");
                                                    string manufacturer = oModel.Get(intParent, "make");
                                                    DateTime installed = DateTime.Now;
                                                    string location = oLocation.GetAddress(intAddress, "service_now");
                                                    string building_code = oLocation.GetAddress(intAddress, "building_code");
                                                    string cla = ServiceNowClasses.Windows;
                                                    if (oOperatingSystem.IsLinux(intOS))
                                                        cla = ServiceNowClasses.Linux;
                                                    else if (oOperatingSystem.IsSolaris(intOS))
                                                        cla = ServiceNowClasses.Solaris;
                                                    string env = ServiceNowEnvironments.Development;
                                                    if (oClass.IsTest(intClass))
                                                        env = ServiceNowEnvironments.Test;
                                                    else if (oClass.IsQA(intClass))
                                                        env = ServiceNowEnvironments.QA;
                                                    else if (oClass.IsProd(intClass))
                                                        env = ServiceNowEnvironments.Production;
                                                    string user = oVariable.ServiceNowUsername();
                                                    string pass = oVariable.ServiceNowPassword();
                                                    oLog.AddEvent(intAnswer, strName, strSerial, "Calling Web Service Function CreateServiceNowServer(" + url + ", " + domain + ", " + strIP + ", " + manufacturer + ", " + mnemonic + ", " + model + ", " + strName + ", " + os + ", " + strSerial + ", " + cla + ", " + env + ", true, " + installed.ToString() + ", " + location + ", " + building_code + ", " + user + ", ****) on " + oVariable.WebServiceURL(), LoggingType.Information);
                                                    string result = oServiceNow.CreateServiceNowServer(url, domain, strIP, manufacturer, mnemonic, model, strName, os, strSerial, cla, env, true, installed, location, building_code, user, pass);
                                                    if (String.IsNullOrEmpty(result) == false)
                                                    {
                                                        strError = result;
                                                        oLog.AddEvent(intAnswer, strName, strSerial, "Service Now Error = " + result, LoggingType.Error);
                                                    }
                                                    else
                                                        oLog.AddEvent(intAnswer, strName, strSerial, "Server added to Service Now", LoggingType.Information);
                                                }
                                                else
                                                    oLog.AddEvent(intAnswer, strName, strSerial, "Missing IP address - skipping service now submission", LoggingType.Information);


                                                ClearViewWebServices oWebService = new ClearViewWebServices();
                                                if (boolRebuilding == false)
                                                {
                                                    if (strError == "")
                                                    {
                                                        // 10/26/2009: Attempt to auto-register DNS
                                                        oLog.AddEvent(intAnswer, strName, strSerial, "Creating DNS Records", LoggingType.Information);
                                                        oWebService.Timeout = Timeout.Infinite;
                                                        oWebService.Credentials = oCredentials;
                                                        oWebService.Url = oVariable.WebServiceURL();
                                                        bool boolDNS_QIP = oSetting.IsDNS_QIP();
                                                        bool boolDNS_Bluecat = oSetting.IsDNS_Bluecat();

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
                                                                    oLog.AddEvent(intAnswer, strName, strSerial, "Calling Web Service Function CreateBluecatDNS(" + strIP + ", " + strName + ", " + strMACAddress + ") on " + oVariable.WebServiceURL(), LoggingType.Information);
                                                                    string strDNS = oWebService.CreateBluecatDNS(strIP, strName, strName, strMACAddress);
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
                                                                            if (intLogging > 0)
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
                                                }
                                                else
                                                    oLog.AddEvent(intAnswer, strName, strSerial, "Rebuilding... skipping DNS Record(s)", LoggingType.Information);

                                                if (strError == "")
                                                {
                                                    oLog.AddEvent(intAnswer, strName, strSerial, "Finished Creating DNS Record", LoggingType.Information);

                                                    //if (intTSM == 0)
                                                    //{
                                                    //    oLog.AddEvent(intAnswer, strName, strSerial, "Creating DNS Record for AVAMAR", LoggingType.Information);
                                                    //    if (intIPAddressAvamar > 0)
                                                    //    {
                                                    //        strIP = oIPAddresses.GetName(intIPAddressAvamar, 0);
                                                    //        if (boolDNS_QIP == true)
                                                    //        {
                                                    //            oLog.AddEvent(intAnswer, strName, strSerial, "Calling Web Service Function CreateDNSforPNC(" + strIP + ", " + strName + "-backup, Server, , " + oVariable.DNS_Domain() + ", " + oVariable.DNS_NameService() + ", " + oVariable.DNS_DynamicDNSUpdate() + ", " + intUser.ToString() + ", true) on " + oVariable.WebServiceURL(), LoggingType.Information);
                                                    //            string strDNS = oWebService.CreateDNSforPNC(strIP, strName + "-backup", "Server", "", oVariable.DNS_Domain(), oVariable.DNS_NameService(), oVariable.DNS_DynamicDNSUpdate(), intUser, 0, true);
                                                    //            if (strDNS == "SUCCESS" || strDNS.StartsWith("***DUPLICATE") == true)
                                                    //            {
                                                    //                oLog.AddEvent(intAnswer, strName, strSerial, "QIP DNS Record for AVAMAR = SUCCESS", LoggingType.Information);
                                                    //            }
                                                    //            else
                                                    //            {
                                                    //                if (strDNS.StartsWith("***CONFLICT") == true)
                                                    //                {
                                                    //                    oLog.AddEvent(intAnswer, strName, strSerial, "QIP DNS Record for AVAMAR = CONFLICT", LoggingType.Warning);
                                                    //                    // A conflict occurred...awaiting the service technician to fix
                                                    //                    strError = "A conflict was encountered when trying to auto-register the QIP DNS object";
                                                    //                    strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DEVELOPER_WARNING");
                                                    //                    if (intLogging > 0)
                                                    //                        oFunction.SendEmail("QIP DNS Automation Conflict", strEMailIdsBCC, "", "", "QIP DNS Automation Conflict", "<p>There was a CONFLICT creating the QIP DNS Record for AVAMAR for " + strName + " (SERVERID: " + intServer.ToString() + ") (ANSWERID: " + intAnswer.ToString() + ")</p><p>Error: " + strDNS + "</p><p>NOTE: A request should have been generated and assigned to a DNS technician.</p>", true, false);
                                                    //                }
                                                    //                else if (strDNS.ToUpper().StartsWith("***ERROR: SUBNET FOR") == true)
                                                    //                {
                                                    //                    oLog.AddEvent(intAnswer, strName, strSerial, "QIP DNS Record for AVAMAR = SUBNET ERROR: " + strDNS, LoggingType.Error);
                                                    //                    // This occurs if a subnet does not exist.  Will need to be routed for assignment and continue.
                                                    //                    if (boolForceDNSSuccess == false)
                                                    //                    {
                                                    //                        strError = "A subnet error was encountered when trying to auto-register the QIP DNS object ~ " + strDNS;
                                                    //                        strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DEVELOPER_WARNING");
                                                    //                        oFunction.SendEmail("QIP DNS Automation Subnet Error", strEMailIdsBCC, "", "", "QIP DNS Automation Subnet Error", "<p>There was a SUBNET ERROR creating the QIP DNS Record for AVAMAR for " + strName + " (SERVERID: " + intServer.ToString() + ") (ANSWERID: " + intAnswer.ToString() + ")</p><p>Error: " + strDNS + "</p><p>NOTE: A request should have been generated and assigned to a DNS technician.</p>", true, false);
                                                    //                    }
                                                    //                }
                                                    //                else
                                                    //                {
                                                    //                    oLog.AddEvent(intAnswer, strName, strSerial, "QIP DNS Record for AVAMAR = ERROR: " + strDNS, LoggingType.Error);
                                                    //                    // An error was encountered...log the error
                                                    //                    if (boolForceDNSSuccess == false)
                                                    //                    {
                                                    //                        strError = "An error was encountered when trying to auto-register the QIP DNS object ~ " + strDNS;
                                                    //                        strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DEVELOPER_ERROR,EMAILGRP_PROVISIONING_SUPPORT");
                                                    //                        oFunction.SendEmail("QIP DNS Automation Error", strEMailIdsBCC, "", "", "QIP DNS Automation Error", "<p>There was an ERROR creating the QIP DNS Record for AVAMAR for " + strName + " (SERVERID: " + intServer.ToString() + ") (ANSWERID: " + intAnswer.ToString() + ")</p><p>Error: " + strDNS + "</p><p>NOTE: Since this is an unexpected error, there was no task initiated to fix this problem. You must correct it yourself.</p>", true, false);
                                                    //                    }
                                                    //                }
                                                    //            }
                                                    //            oLog.AddEvent(intAnswer, strName, strSerial, "QIP DNS Record for AVAMAR Finished", LoggingType.Information);
                                                    //        }
                                                    //        if (strError == "")
                                                    //        {
                                                    //            if (boolDNS_Bluecat == true)
                                                    //            {
                                                    //                oLog.AddEvent(intAnswer, strName, strSerial, "Calling Web Service Function CreateBluecatDNS(" + strIP + ", " + strName + "-backup, ) on " + oVariable.WebServiceURL(), LoggingType.Information);
                                                    //                string strDNS = oWebService.CreateBluecatDNS(strIP, strName + "-backup", strName + "-backup", "");
                                                    //                if (strDNS == "SUCCESS" || strDNS.StartsWith("***DUPLICATE") == true)
                                                    //                {
                                                    //                    oLog.AddEvent(intAnswer, strName, strSerial, "BlueCat DNS Record for AVAMAR = SUCCESS", LoggingType.Information);
                                                    //                }
                                                    //                else
                                                    //                {
                                                    //                    if (strDNS.StartsWith("***CONFLICT") == true)
                                                    //                    {
                                                    //                        oLog.AddEvent(intAnswer, strName, strSerial, "BlueCat DNS Record for AVAMAR = CONFLICT", LoggingType.Warning);
                                                    //                        // A conflict occurred...awaiting the service technician to fix
                                                    //                        strError = "A conflict was encountered when trying to auto-register the BlueCat DNS object ~ " + strDNS;
                                                    //                        strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DEVELOPER_WARNING");
                                                    //                        if (intLogging > 0)
                                                    //                            oFunction.SendEmail("BlueCat DNS Automation Conflict", strEMailIdsBCC, "", "", "BlueCat DNS Automation Conflict", "<p>There was a CONFLICT creating the BlueCat DNS Record for AVAMAR for " + strName + " (SERVERID: " + intServer.ToString() + ") (ANSWERID: " + intAnswer.ToString() + ")</p><p>Error: " + strDNS + "</p><p>NOTE: A request should have been generated and assigned to a DNS technician.</p>", true, false);
                                                    //                    }
                                                    //                    else
                                                    //                    {
                                                    //                        oLog.AddEvent(intAnswer, strName, strSerial, "BlueCat DNS Record for AVAMAR = ERROR: " + strDNS, LoggingType.Error);
                                                    //                        // An error was encountered...log the error
                                                    //                        if (boolForceDNSSuccess == false)
                                                    //                        {
                                                    //                            strError = "An error was encountered when trying to auto-register the BlueCat DNS object ~ " + strDNS;
                                                    //                            strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DEVELOPER_ERROR,EMAILGRP_PROVISIONING_SUPPORT");
                                                    //                            oFunction.SendEmail("BlueCat DNS Automation Error", strEMailIdsBCC, "", "", "BlueCat DNS Automation Error", "<p>There was an ERROR creating the BlueCat DNS Record for AVAMAR for " + strName + " (SERVERID: " + intServer.ToString() + ") (ANSWERID: " + intAnswer.ToString() + ")</p><p>Error: " + strDNS + "</p><p>NOTE: Since this is an unexpected error, there was no task initiated to fix this problem. You must correct it yourself.</p>", true, false);
                                                    //                        }
                                                    //                    }
                                                    //                }
                                                    //                oLog.AddEvent(intAnswer, strName, strSerial, "BlueCat DNS Record for AVAMAR Finished", LoggingType.Information);
                                                    //            }
                                                    //        }
                                                    //    }
                                                    //    else
                                                    //    {
                                                    //        oLog.AddEvent(intAnswer, strName, strSerial, "There was no AVAMAR IP address assigned...skipping DNS Registration", LoggingType.Information);
                                                    //    }
                                                    //    oLog.AddEvent(intAnswer, strName, strSerial, "Finished Creating DNS Record for AVAMAR", LoggingType.Information);
                                                    //}
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


                                            //oLog.AddEvent(intAnswer, strName, strSerial, "Attempting to send notification", LoggingType.Information);
                                            //strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DESIGN_BUILDER");
                                            //if (strEmail != "")
                                            //    oFunction.SendEmail("Auto-Provisioning Notification: " + strName, strEmail, "", strEMailIdsBCC, "Auto-Provisioning Notification: " + strName, "<p><b>This message is to inform you that the server " + strName + " has been auto-provisioned successfully!</b><p><p>Serial Number: " + oAsset.Get(intAsset, "serial").ToUpper() + "<br/>Asset Tag: " + oAsset.Get(intAsset, "asset").ToUpper() + "<br/>Datacenter: " + oVMWare.DataCenter() + " on " + oVMWare.VirtualCenter() + "<br/>Datastore: " + oVMWare.GetDatastore(intDatastore, "name") + "<br/>IP Address: " + oIPAddresses.GetName(intIPAddress, 0) + "<br/>Model: " + oModelsProperties.Get(intModel, "name").ToUpper() + "</p><p><b>NOTE:</b> This is automated email sent from the ClearView Auto-Provisioning tool. Please do not respond to this message.</p><p>If you would like to learn more about this process, <a href=\"" + oVariable.URL() + "/info.htm\" target=\"_blank\"/>please click here</a>.</p>", true, false);
                                            //oLog.AddEvent(intAnswer, strName, strSerial, "FINISHED = Attempting to send notification", LoggingType.Information);

                                            // Remove files on target server
                                            string strRemoveIP = strIP;
                                            if (intIPAddress > 0)
                                                strRemoveIP = oIPAddresses.GetName(intIPAddress, 0);
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
                                                infoDeleteTarget.Arguments = "-h cmd.exe /c " + strBatchDeleteTarget;
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
                                            if (strResult == "")
                                                strResult = oOnDemand.GetStep(intStepID, "done");
                                            AddResult(intServer, intStep, intType, strResult, strError);
                                            oServer.UpdateStep(intServer, intLastStep);
                                            if (boolRebuilding == true)
                                            {
                                                if (String.IsNullOrEmpty(oServer.Get(intServer, "build_completed")) == true)
                                                    oServer.UpdateBuildCompleted(intServer, DateTime.Now.ToString());
                                                oServer.UpdateRebuild(intServer, DateTime.Now.ToString());
                                                strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DEVELOPER_ALERT");
                                                DataSet dsRebuilds = oServer.GetRebuild(intServer);
                                                if (dsRebuilds.Tables[0].Rows.Count > 0)
                                                {
                                                    DataRow drRebuild = dsRebuilds.Tables[0].Rows[0];
                                                    int intRebuilder = Int32.Parse(drRebuild["userid"].ToString());
                                                    oFunction.SendEmail("Auto-Provisioning Rebuild Notification: " + strName, oUser.GetName(intRebuilder), "", strEMailIdsBCC, "Auto-Provisioning Rebuild Notification: " + strName, "<p><b>This message is to inform you that the server " + strName + " has been rebuilt successfully!</b><p><p>Serial Number: " + oAsset.Get(intAsset, "serial").ToUpper() + "<br/>Asset Tag: " + oAsset.Get(intAsset, "asset").ToUpper() + "<br/>Model: " + oModelsProperties.Get(intModel, "name").ToUpper() + "<br/>DataCenter: " + oVMWare.DataCenter() + "<br/>Virtual Center: " + oVMWare.VirtualCenter() + "</p><p><b>NOTE:</b> This is automated email sent from the ClearView Auto-Provisioning tool. Please do not respond to this message.</p><p>If you would like to learn more about this tool, <a href=\"" + oVariable.URL() + "/info.htm\" target=\"_blank\"/>please click here</a>.</p>", true, false);
                                                }
                                                else if (strEMailIdsBCC != "")
                                                    oFunction.SendEmail("Auto-Provisioning Rebuild Notification: " + strName, strEMailIdsBCC, "", "", "Auto-Provisioning Rebuild Notification: " + strName, "<p><b>This message is to inform you that the server " + strName + " has been rebuilt successfully!</b><p><p>Serial Number: " + oAsset.Get(intAsset, "serial").ToUpper() + "<br/>Asset Tag: " + oAsset.Get(intAsset, "asset").ToUpper() + "<br/>Model: " + oModelsProperties.Get(intModel, "name").ToUpper() + "<br/>DataCenter: " + oVMWare.DataCenter() + "<br/>Virtual Center: " + oVMWare.VirtualCenter() + "</p><p><b>NOTE:</b> This is automated email sent from the ClearView Auto-Provisioning tool. Please do not respond to this message.</p><p>If you would like to learn more about this process, <a href=\"" + oVariable.URL() + "/info.htm\" target=\"_blank\"/>please click here</a>.</p>", true, false);
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
                                        }
                                    }
                                    if (strError != "")
                                        AddResult(intServer, intStep, intType, strResult, strError);
                                    break;
                                case 24:    // Create Anti-Affinity Rule (if necessary)
                                    if (intLogging > 1)
                                        oEventLog.WriteEntry(strName + " (VMware): " + "Checking Step 24 (Create Anti-Affinity Rule)", EventLogEntryType.Information);
                                    DataSet dsServerAnswer = oServer.GetAnswer(intAnswer);
                                    bool boolContinue = true;
                                    int intNumOfServers = 0;
                                    foreach (DataRow drServerAnswer in dsServerAnswer.Tables[0].Rows)
                                    {
                                        if (drServerAnswer["step"].ToString() != intLastStep.ToString() && drServerAnswer["step"].ToString() != "999")
                                        {
                                            boolContinue = false;
                                            break;
                                        }
                                        else
                                            intNumOfServers++;
                                    }
                                    if (boolContinue == true)
                                    {
                                        if (oForecast.IsHANone(intAnswer) == false)
                                        {
                                            // Setup Anti-Affinity Rule
                                            oLog.AddEvent(intAnswer, strName, strSerial, "Starting Step " + intStep.ToString() + " (Create Anti-Affinity Rule)", LoggingType.Information);

                                            string strServers = "";
                                            string strAAError = "";
                                            string strHAName = oVMWare.GetAntiAffinity(strAppCode, intCluster);

                                            if (intClusterAntiAffinity == 0)
                                            {
                                                // ****************************
                                                // START: IF VMWARE UPDATE 3
                                                // (Current version - new code)
                                                // ****************************
                                                oLog.AddEvent(intAnswer, strName, strSerial, "Using VMWARE UPDATE 3 code", LoggingType.Debug);
                                                int intHACounter = 0;
                                                int intHACounterIncrementTotal = dsServerAnswer.Tables[0].Rows.Count;
                                                int intHACounterTotal = (intHACounterIncrementTotal - 1);
                                                ManagedObjectReference clusterRef2 = oVMWare.GetCluster(oVMWare.GetCluster(intCluster, "name"));
                                                strAAError = "ManagedObjectReference get";
                                                ClusterConfigInfo cinfo = (ClusterConfigInfo)oVMWare.getObjectProperty(clusterRef2, "configuration");
                                                strAAError = "ClusterConfigInfo get";
                                                // If 5 devices, then only 0, 1, 2, 3 (since row count = 5 and 5 -1 = 4) will be processed (since 3 < 4 is last to return true)
                                                while (intHACounter < intHACounterTotal)
                                                {
                                                    int intHACounterIncrement = (intHACounter + 1);
                                                    while (intHACounterIncrement < intHACounterIncrementTotal)
                                                    {
                                                        strServers = dsServerAnswer.Tables[0].Rows[intHACounter]["servername"].ToString() + "-" + dsServerAnswer.Tables[0].Rows[intHACounterIncrement]["servername"].ToString();
                                                        if (strServers.Length > 80)
                                                            strServers = strServers.Substring(0, 80);
                                                        strAAError = "ClusterConfigInfo done";
                                                        try
                                                        {
                                                            int intKey = 1;
                                                            if (cinfo.rule != null)
                                                            {
                                                                strAAError = "cinfo.rule != null";
                                                                ClusterRuleInfo[] rules = cinfo.rule;
                                                                strAAError = "cinfo.rule";
                                                                oLog.AddEvent(intAnswer, strName, strSerial, "There are " + rules.Length.ToString() + " rules", LoggingType.Debug);
                                                                for (int ii = 0; ii < rules.Length; ii++)
                                                                {
                                                                    oLog.AddEvent(intAnswer, strName, strSerial, "Checking rule # " + ii.ToString() + " - " + rules[ii].name, LoggingType.Debug);
                                                                    if (rules[ii].key > intKey)
                                                                        intKey = rules[ii].key;
                                                                    if (rules[ii].name == strServers || rules[ii].name == strHAName)
                                                                    {
                                                                        boolContinue = false;
                                                                        break;
                                                                    }
                                                                }
                                                                strAAError = "rules.Length";
                                                            }
                                                            intKey++;

                                                            if (boolContinue == true)
                                                            {
                                                                oLog.AddEvent(intAnswer, strName, strSerial, "Rule not found!", LoggingType.Debug);
                                                                int intAntiAffinity = oVMWare.AddAntiAffinity(strHAName, intCluster);
                                                                ClusterAntiAffinityRuleSpec oAff = new ClusterAntiAffinityRuleSpec();
                                                                //ClusterAffinityRuleSpec oAff = new ClusterAffinityRuleSpec();
                                                                char[] strDash = { '-' };
                                                                string[] strServer = strServers.Split(strDash);
                                                                ManagedObjectReference[] vmRefs = new ManagedObjectReference[strServer.Length];
                                                                strAAError = "ManagedObjectReference vmRefs";
                                                                for (int ii = 0; ii < strServer.Length; ii++)
                                                                {
                                                                    if (strServer[ii].Trim() != "")
                                                                    {
                                                                        oVMWare.AddAntiAffinity(intAntiAffinity, strServer[ii].Trim());
                                                                        vmRefs[ii] = oVMWare.GetVM(strServer[ii].Trim());
                                                                    }
                                                                }
                                                                strAAError = "strServer.Length";
                                                                oAff.name = strHAName;
                                                                strAAError = "oAff.name";
                                                                oAff.key = intKey;
                                                                oAff.keySpecified = true;
                                                                strAAError = "oAff.keySpecified";
                                                                oAff.enabled = true;
                                                                oAff.enabledSpecified = true;
                                                                strAAError = "oAff.enabledSpecified";

                                                                oLog.AddEvent(intAnswer, strName, strSerial, "Creating Anti-Affinity Rule " + strHAName + " for " + strServers, LoggingType.Information);

                                                                ClusterRuleSpec oRule = new ClusterRuleSpec();
                                                                oRule.operation = ArrayUpdateOperation.add;
                                                                oRule.info = oAff;
                                                                ClusterConfigSpec oSpec = new ClusterConfigSpec();
                                                                oSpec.rulesSpec = new ClusterRuleSpec[] { oRule };
                                                                oAff.vm = vmRefs;
                                                                strAAError = "oAff.vm";
                                                                ManagedObjectReference _task_aff = _service.ReconfigureCluster_Task(clusterRef2, oSpec, true);
                                                                strAAError = "ReconfigureCluster_Task";
                                                                TaskInfo _info_aff = (TaskInfo)oVMWare.getObjectProperty(_task_aff, "info");
                                                                while (_info_aff.state == TaskInfoState.running)
                                                                    _info_aff = (TaskInfo)oVMWare.getObjectProperty(_task_aff, "info");
                                                                strAAError = "TaskInfo _info_aff";
                                                                if (_info_aff.state == TaskInfoState.success)
                                                                    strResult += "Anti-Affinity Rule Was Created (" + strHAName + " : " + strServers + ")<br/>";
                                                                else
                                                                    strError += "Anti-Affinity Rule Was Not Created (" + strHAName + " : " + strServers + ")<br/>";
                                                            }
                                                            else
                                                            {
                                                                strResult += "Anti-Affinity Already Created (" + strHAName + " : " + strServers + ")<br/>";
                                                            }
                                                            strHAName = oVMWare.GetAntiAffinity(strAppCode, intCluster);
                                                        }
                                                        catch (Exception exAAF)
                                                        {
                                                            strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DEVELOPER_ERROR,EMAILGRP_PROVISIONING_SUPPORT");
                                                            oFunction.SendEmail("Anti-Affinity Rule Creation", strEMailIdsBCC, "", "", "Anti-Affinity Rule Creation", "<p><b>While attempting to create an Anti-Affinity Rule, an error was encountered....</b></p><p>Cluster: " + oVMWare.GetCluster(intCluster, "name") + "<br/>Rule: " + strHAName + " : " + strServers + "</p><p>This rule has been skipped to prevent delays in provisioning. Please configure this rule at your earliest convenience.</p><p>Last Known Error: " + strAAError + "</p><p>Error Message: " + exAAF.Message + "</p>", true, false);
                                                        }
                                                        intHACounterIncrement++;
                                                    }
                                                    intHACounter++;
                                                }
                                                foreach (DataRow drServerAnswer in dsServerAnswer.Tables[0].Rows)
                                                    AddResult(Int32.Parse(drServerAnswer["id"].ToString()), intStep, intType, strResult, strError);
                                                // ****************************
                                                // END: IF VMWARE UPDATE 3
                                                // ****************************
                                            }
                                            if (intClusterAntiAffinity == 1)
                                            {
                                                // ****************************
                                                // START: IF VMWARE UPDATE 4
                                                // (New version - old code)
                                                // ****************************
                                                oLog.AddEvent(intAnswer, strName, strSerial, "Using VMWARE UPDATE 4 code", LoggingType.Debug);
                                                ManagedObjectReference clusterRef2 = oVMWare.GetCluster(oVMWare.GetCluster(intCluster, "name"));
                                                strAAError = "ManagedObjectReference get";
                                                ClusterConfigInfo cinfo = (ClusterConfigInfo)oVMWare.getObjectProperty(clusterRef2, "configuration");
                                                strAAError = "ClusterConfigInfo get";
                                                foreach (DataRow drServerAnswer in dsServerAnswer.Tables[0].Rows)
                                                {
                                                    if (strServers != "")
                                                        strServers += "-";
                                                    strServers += drServerAnswer["servername"].ToString();
                                                }
                                                if (strServers.Length > 80)
                                                    strServers = strServers.Substring(0, 80);
                                                strAAError = "ClusterConfigInfo done";
                                                try
                                                {
                                                    int intKey = 1;
                                                    if (cinfo.rule != null)
                                                    {
                                                        strAAError = "cinfo.rule != null";
                                                        ClusterRuleInfo[] rules = cinfo.rule;
                                                        strAAError = "cinfo.rule";
                                                        oLog.AddEvent(intAnswer, strName, strSerial, "There are " + rules.Length.ToString() + " rules", LoggingType.Debug);
                                                        for (int ii = 0; ii < rules.Length; ii++)
                                                        {
                                                            oLog.AddEvent(intAnswer, strName, strSerial, "Checking rule # " + ii.ToString() + " - " + rules[ii].name, LoggingType.Debug);
                                                            if (rules[ii].key > intKey)
                                                                intKey = rules[ii].key;
                                                            if (rules[ii].name == strServers || rules[ii].name == strHAName)
                                                            {
                                                                boolContinue = false;
                                                                break;
                                                            }
                                                        }
                                                        strAAError = "rules.Length";
                                                    }
                                                    intKey++;

                                                    if (boolContinue == true)
                                                    {
                                                        oLog.AddEvent(intAnswer, strName, strSerial, "Rule not found!", LoggingType.Debug);
                                                        int intAntiAffinity = oVMWare.AddAntiAffinity(strHAName, intCluster);
                                                        ClusterAntiAffinityRuleSpec oAff = new ClusterAntiAffinityRuleSpec();
                                                        //ClusterAffinityRuleSpec oAff = new ClusterAffinityRuleSpec();
                                                        char[] strDash = { '-' };
                                                        string[] strServer = strServers.Split(strDash);
                                                        ManagedObjectReference[] vmRefs = new ManagedObjectReference[strServer.Length];
                                                        strAAError = "ManagedObjectReference vmRefs";
                                                        for (int ii = 0; ii < strServer.Length; ii++)
                                                        {
                                                            if (strServer[ii].Trim() != "")
                                                            {
                                                                oVMWare.AddAntiAffinity(intAntiAffinity, strServer[ii].Trim());
                                                                vmRefs[ii] = oVMWare.GetVM(strServer[ii].Trim());
                                                            }
                                                        }
                                                        strAAError = "strServer.Length";
                                                        oAff.name = strHAName;
                                                        strAAError = "oAff.name";
                                                        oAff.key = intKey;
                                                        oAff.keySpecified = true;
                                                        strAAError = "oAff.keySpecified";
                                                        oAff.enabled = true;
                                                        oAff.enabledSpecified = true;
                                                        strAAError = "oAff.enabledSpecified";

                                                        oLog.AddEvent(intAnswer, strName, strSerial, "Creating Anti-Affinity Rule " + strHAName + " for " + strServers, LoggingType.Information);

                                                        ClusterRuleSpec oRule = new ClusterRuleSpec();
                                                        oRule.operation = ArrayUpdateOperation.add;
                                                        oRule.info = oAff;
                                                        ClusterConfigSpec oSpec = new ClusterConfigSpec();
                                                        oSpec.rulesSpec = new ClusterRuleSpec[] { oRule };
                                                        oAff.vm = vmRefs;
                                                        strAAError = "oAff.vm";
                                                        ManagedObjectReference _task_aff = _service.ReconfigureCluster_Task(clusterRef2, oSpec, true);
                                                        strAAError = "ReconfigureCluster_Task";
                                                        TaskInfo _info_aff = (TaskInfo)oVMWare.getObjectProperty(_task_aff, "info");
                                                        while (_info_aff.state == TaskInfoState.running)
                                                            _info_aff = (TaskInfo)oVMWare.getObjectProperty(_task_aff, "info");
                                                        strAAError = "TaskInfo _info_aff";
                                                        if (_info_aff.state == TaskInfoState.success)
                                                            strResult += "Anti-Affinity Rule Was Created (" + strHAName + " : " + strServers + ")";
                                                        else
                                                            strError += "Anti-Affinity Rule Was Not Created (" + strHAName + " : " + strServers + ")";
                                                    }
                                                    else
                                                    {
                                                        strResult += "Anti-Affinity Already Created (" + strHAName + " : " + strServers + ")";
                                                    }
                                                }
                                                catch (Exception exAAF)
                                                {   strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DEVELOPER_ERROR,EMAILGRP_PROVISIONING_SUPPORT");
                                                    oFunction.SendEmail("Anti-Affinity Rule Creation", strEMailIdsBCC, "", "", "Anti-Affinity Rule Creation", "<p><b>While attempting to create an Anti-Affinity Rule, an error was encountered....</b></p><p>Cluster: " + oVMWare.GetCluster(intCluster, "name") + "<br/>Rule: " + strHAName + " : " + strServers + "</p><p>This rule has been skipped to prevent delays in provisioning. Please configure this rule at your earliest convenience.</p><p>Last Known Error: " + strAAError + "</p><p>Error Message: " + exAAF.Message + "</p>", true, false);
                                                }
                                                foreach (DataRow drServerAnswer in dsServerAnswer.Tables[0].Rows)
                                                    AddResult(Int32.Parse(drServerAnswer["id"].ToString()), intStep, intType, strResult, strError);
                                                // ****************************
                                                // END: IF VMWARE UPDATE 4
                                                // ****************************
                                            }
                                        }
                                        else
                                        {
                                            strResult = "Anti-Affinity Rule Not Applicable";
                                            foreach (DataRow drServerAnswer in dsServerAnswer.Tables[0].Rows)
                                                AddResult(Int32.Parse(drServerAnswer["id"].ToString()), intStep, intType, strResult, strError);
                                        }

                                        // Check for rebuild again...should have been set in previous step, but if not, it's because it was skipped so set it now
                                        if (boolRebuilding == true)
                                        {
                                            oServer.UpdateRebuild(intServer, DateTime.Now.ToString());
                                            strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DEVELOPER_ALERT");
                                            DataSet dsRebuilds = oServer.GetRebuild(intServer);
                                            if (dsRebuilds.Tables[0].Rows.Count > 0)
                                            {
                                                DataRow drRebuild = dsRebuilds.Tables[0].Rows[0];
                                                int intRebuilder = Int32.Parse(drRebuild["userid"].ToString());
                                                oFunction.SendEmail("Auto-Provisioning Rebuild Notification: " + strName, oUser.GetName(intRebuilder), "", strEMailIdsBCC, "Auto-Provisioning Rebuild Notification: " + strName, "<p><b>This message is to inform you that the server " + strName + " has been rebuilt successfully!</b><p><p>Serial Number: " + oAsset.Get(intAsset, "serial").ToUpper() + "<br/>Asset Tag: " + oAsset.Get(intAsset, "asset").ToUpper() + "<br/>Model: " + oModelsProperties.Get(intModel, "name").ToUpper() + "<br/>DataCenter: " + oVMWare.DataCenter() + "<br/>Virtual Center: " + oVMWare.VirtualCenter() + "</p><p><b>NOTE:</b> This is automated email sent from the ClearView Auto-Provisioning tool. Please do not respond to this message.</p><p>If you would like to learn more about this tool, <a href=\"" + oVariable.URL() + "/info.htm\" target=\"_blank\"/>please click here</a>.</p>", true, false);
                                            }
                                            else if (strEMailIdsBCC != "")
                                                oFunction.SendEmail("Auto-Provisioning Rebuild Notification: " + strName, strEMailIdsBCC, "", "", "Auto-Provisioning Rebuild Notification: " + strName, "<p><b>This message is to inform you that the server " + strName + " has been rebuilt successfully!</b><p><p>Serial Number: " + oAsset.Get(intAsset, "serial").ToUpper() + "<br/>Asset Tag: " + oAsset.Get(intAsset, "asset").ToUpper() + "<br/>Model: " + oModelsProperties.Get(intModel, "name").ToUpper() + "<br/>DataCenter: " + oVMWare.DataCenter() + "<br/>Virtual Center: " + oVMWare.VirtualCenter() + "</p><p><b>NOTE:</b> This is automated email sent from the ClearView Auto-Provisioning tool. Please do not respond to this message.</p><p>If you would like to learn more about this process, <a href=\"" + oVariable.URL() + "/info.htm\" target=\"_blank\"/>please click here</a>.</p>", true, false);
                                            oServer.UpdateRebuilding(intServer, 0);
                                        }


                                        // Add clustering (won't be processed until backup is completed)
                                        if (boolIsClustering)
                                            oCluster.AddClustering(intAnswer);

                                        // Check for Anti-Affinity Rule Error
                                        if (strError.StartsWith("Anti-Affinity Rule Was Not Created") == true)
                                            strError = "Anti-Affinity Rule(s) Were Not Created ~ " + strError;

                                        oServer.UpdateAnswer(intAnswer, 999);
                                        oForecast.UpdateAnswerCompleted(intAnswer);
                                        if (oForecast.GetAnswer(intAnswer, "completed") != "")
                                        {
                                            oLog.AddEvent(intAnswer, strName, strSerial, "Starting Step " + intStep.ToString() + " (Birth Certificate and Workflow)", LoggingType.Information);

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
                                            //strError = oPNCTask.InitiateNextStep(intRequest, intAnswer, intModel, intEnvironment, intAssignPage, intViewPage, dsnAsset, dsnIP, dsnServiceEditor);
                                            oPNCTask.InitiateNextStep(intRequest, intAnswer, intModel, intEnvironment, intAssignPage, intViewPage, dsnAsset, dsnIP, dsnServiceEditor);
                                        }
                                        else
                                            oLog.AddEvent(intAnswer, strName, strSerial, "ANSWERID: " + intAnswer.ToString() + " not done - no forms to send yet for " + strName, LoggingType.Information);
                                    }
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
                                        oFunction.SendEmail("Auto-Provisioning INACTIVITY: " + strName, strEMailIdsBCC, strImplementor, "", "Auto-Provisioning INACTIVITY: " + strName, "<p><b>This message is to inform you that the server " + strName + " has been sitting at a step for more than 24 hours!</b><p><p>Serial Number: " + oAsset.Get(intAsset, "serial").ToUpper() + "<br/>Asset Tag: " + oAsset.Get(intAsset, "asset").ToUpper() + "<br/>Model: " + oModelsProperties.Get(intModel, "name").ToUpper() + "<br/>Step #: " + intStep.ToString() + "<br/>Step: " + strStep + "<br/>Error: " + strError + "<br/>DataCenter: " + oVMWare.DataCenter() + "<br/>Virtual Center: " + oVMWare.VirtualCenter() + "</p><p>Once you have resolved this issue, please <a href=\"" + oVariable.URL() + "/build_fixed.aspx?type=s&id=" + oFunction.encryptQueryString(intServer.ToString()) + "\" target=\"_blank\">click here</a> to resume the build.</p><p><b>NOTE:</b> This is automated email sent from the ClearView Auto-Provisioning tool. Please do not respond to this message.</p>", true, false);
                                }
                                else
                                {
                                    int intError = oServer.AddError(intRequest, intProvisioningErrorItem, intProvisioningErrorNumber, intServer, intStep, strError);
                                    if (boolProvisioningErrorEmail == true)
                                        oFunction.SendEmail("Auto-Provisioning ERROR: " + strName, strEMailIdsBCC, strImplementor, "", "Auto-Provisioning ERROR: " + strName, "<p><b>This message is to inform you that the server " + strName + " has encountered an error and has been stopped!</b><p><p>Serial Number: " + oAsset.Get(intAsset, "serial").ToUpper() + "<br/>Asset Tag: " + oAsset.Get(intAsset, "asset").ToUpper() + "<br/>Model: " + oModelsProperties.Get(intModel, "name").ToUpper() + "<br/>Step #: " + intStep.ToString() + "<br/>Step: " + strStep + "<br/>Error: " + strError + "<br/>DataCenter: " + oVMWare.DataCenter() + "<br/>Virtual Center: " + oVMWare.VirtualCenter() + "</p><p>Once you have resolved this issue, please <a href=\"" + oVariable.URL() + "/build_fixed.aspx?type=s&id=" + oFunction.encryptQueryString(intServer.ToString()) + "\" target=\"_blank\">click here</a> to resume the build.</p><p><b>NOTE:</b> This is automated email sent from the ClearView Auto-Provisioning tool. Please do not respond to this message.</p>", true, false);
                                }
                                if (boolProvisioningErrorEmail == true)
                                    oLog.AddEvent(intAnswer, strName, strSerial, "Error Message [" + strError + "] Sent to " + strEMailIdsBCC, LoggingType.Warning);

                                if (boolAuditError == false)
                                {
                                    int intProvisioningError = oResourceRequest.Add(intRequest, intProvisioningErrorItem, intProvisioningErrorService, intProvisioningErrorNumber, "Provisioning Error (" + strName + ")", 1, 0.00, 2, 1, 1, 1);
                                    if (oServiceRequest.NotifyApproval(intProvisioningError, intResourceRequestApprove, intEnvironment, "", dsnServiceEditor) == false)
                                        oServiceRequest.NotifyTeamLead(intProvisioningErrorItem, intProvisioningError, intAssignPage, intViewPage, intEnvironment, "", dsnServiceEditor, dsnAsset, dsnIP, 0);
                                    oLog.AddEvent(intAnswer, strName, strSerial, "Provisioning Error Request has been submitted (" + strError + ")", LoggingType.Warning);
                                }
                            }
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
                            //oLog.AddEvent(intAnswer, strName, strSerial, "Logged out of VMware", LoggingType.Information);
                        }
                    }
                }
                if (ds.Tables[0].Rows.Count == 0 && intLogging > 1)
                    oEventLog.WriteEntry("No VMware servers to build", EventLogEntryType.Information);
            }
            catch (Exception ex)
            {
                if (ex.Message.ToUpper().StartsWith("A TRANSPORT-LEVEL ERROR") == false)
                {
                    if (ex.Message.ToUpper().Contains("HTTP STATUS 503") == false)
                    {
                        string strError = "VMWare Service: " + "(Error Message: " + ex.Message + ") ~ (Source: " + ex.Source + ") (Stack Trace: " + ex.StackTrace + ") [" + System.Environment.UserName + "]";
                        oEventLog.WriteEntry(strError, EventLogEntryType.Error);
                        SystemError(intErrorServer, intErrorStep, strError, intErrorAsset, intErrorModel, oErrorVMWare);
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
        private void ServiceTickDecom()
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
                Functions oFunction = new Functions(0, dsn, intEnvironment);
                DataSet ds = oAsset.GetDecommissions(strServerTypesDecom, DateTime.Now);
                Users oUser = new Users(0, dsn);
                Domains oDomain = new Domains(0, dsn);
                Projects oProject = new Projects(0, dsn);
                ServiceRequests oServiceRequest = new ServiceRequests(0, dsn);
                Services oService = new Services(0, dsn);
                ResourceRequest oResourceRequest = new ResourceRequest(0, dsn);
                ServiceDetails oServiceDetail = new ServiceDetails(0, dsn);
                OperatingSystems oOperatingSystem = new OperatingSystems(0, dsn);
                Variables oVariable = new Variables(intEnvironment);

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
                                oLog.AddEvent(strName, strSerial, "DECOMMISSION: Finished Decommissioning (DR)", LoggingType.Information);
                            }
                        }
                    }
                    else
                    {
                        if (oResourceRequest.GetAllService(intRequest, intDecomErrorService, intNumber).Tables[0].Rows.Count == 0)
                        {
                            // Check Decommission
                            if (dsAsset.Tables[0].Rows.Count > 0)
                            {
                                bool boolProcess = false;
                                int intClass = Int32.Parse(dsAsset.Tables[0].Rows[0]["classid"].ToString());
                                int intEnv = Int32.Parse(dsAsset.Tables[0].Rows[0]["environmentid"].ToString());
                                int intAddress = Int32.Parse(dsAsset.Tables[0].Rows[0]["addressid"].ToString());

                                if (oClass.IsProd(intClass))
                                {
                                    if (intBuildProd == 1)
                                    {
                                        if (intBuildProdOffHours == 0)
                                        {
                                            boolProcess = true;
                                            if (intLogging > 0)
                                                oEventLog.WriteEntry("SERVERNAME: " + strName + " (VMware): " + "DECOMMISSION: PROD Classes have been enabled for builds", EventLogEntryType.Information);
                                        }
                                        else if (DateTime.Now.Hour < 7 || DateTime.Now.Hour > 19)
                                        {
                                            boolProcess = true;
                                            if (intLogging > 0)
                                                oEventLog.WriteEntry("SERVERNAME: " + strName + " (VMware): " + "DECOMMISSION: PROD Classes have been enabled for OFF PEAK HOURS (7AM - 7PM) Current Time : " + DateTime.Now.ToShortTimeString(), EventLogEntryType.Information);
                                        }
                                        else if (intLogging > 0)
                                            oEventLog.WriteEntry("SERVERNAME: " + strName + " (VMware): " + "DECOMMISSION: PROD Classes are not enabled for DURING PEAK HOURS (7AM - 7PM) builds", EventLogEntryType.Warning);
                                    }
                                    else if (intLogging > 0)
                                        oEventLog.WriteEntry("SERVERNAME: " + strName + " (VMware): " + "DECOMMISSION: PROD Classes are not enabled for builds", EventLogEntryType.Warning);
                                }
                                if (oClass.IsQA(intClass))
                                {
                                    if (intBuildQA == 1)
                                    {
                                        if (intBuildQAOffHours == 0)
                                        {
                                            boolProcess = true;
                                            if (intLogging > 0)
                                                oEventLog.WriteEntry("SERVERNAME: " + strName + " (VMware): " + "DECOMMISSION: QA Classes have been enabled for builds", EventLogEntryType.Information);
                                        }
                                        else if (DateTime.Now.Hour < 7 || DateTime.Now.Hour > 19)
                                        {
                                            boolProcess = true;
                                            if (intLogging > 0)
                                                oEventLog.WriteEntry("SERVERNAME: " + strName + " (VMware): " + "DECOMMISSION: QA Classes have been enabled for OFF PEAK HOURS (7AM - 7PM) Current Time : " + DateTime.Now.ToShortTimeString(), EventLogEntryType.Information);
                                        }
                                        else if (intLogging > 0)
                                            oEventLog.WriteEntry("SERVERNAME: " + strName + " (VMware): " + "DECOMMISSION: QA Classes are not enabled for DURING PEAK HOURS (7AM - 7PM) builds", EventLogEntryType.Warning);
                                    }
                                    else if (intLogging > 0)
                                        oEventLog.WriteEntry("SERVERNAME: " + strName + " (VMware): " + "DECOMMISSION: QA Classes are not enabled for builds", EventLogEntryType.Warning);
                                }
                                if (oClass.IsTestDev(intClass))
                                {
                                    if (intBuildTest == 1)
                                    {
                                        boolProcess = true;
                                        if (intLogging > 0)
                                            oEventLog.WriteEntry("SERVERNAME: " + strName + " (VMware): " + "DECOMMISSION: TEST Classes have been enabled for builds", EventLogEntryType.Information);
                                    }
                                    else if (intLogging > 0)
                                        oEventLog.WriteEntry("SERVERNAME: " + strName + " (VMware): " + "DECOMMISSION: TEST Classes are not enabled for builds", EventLogEntryType.Warning);
                                }


                                if (boolProcess == true)
                                {
                                    // Start Decommission
                                    oAsset.UpdateDecommissionRunning(intAsset, 1);
                                    oLog.AddEvent(strName, strSerial, "DECOMMISSION: Starting Decommission (non-DR)", LoggingType.Information);
                                    oServer.UpdateStep(intServer, 999);
                                    int intOS = 0;
                                    Int32.TryParse(oServer.Get(intServer, "osid"), out intOS);

                                    string strDecomIP = oFunction.PingName(strName);
                                    // Ping name to see if it is still on
                                    if (strDecomIP == "")
                                    {
                                        // Server is not responding, assume it is off
                                        boolDecom = true;
                                        oLog.AddEvent(strName, strSerial, "DECOMMISSION: Server has already been powered off (" + strName + " is not responding to ping)", LoggingType.Information);
                                    }
                                    else
                                    {
                                        bool boolFound = false;
                                        if (intAssetType == intTypeVirtual)
                                        {
                                            oLog.AddEvent(strName, strSerial, "DECOMMISSION: Beginning Microsoft Virtual decommission process", LoggingType.Information);
                                            if (boolSkipVirtual == true)
                                            {
                                                boolDecom = true;
                                                oLog.AddEvent(strName, strSerial, "DECOMMISSION: Finished Decommission (SKIP)", LoggingType.Information);
                                                strResult = "Finished Decommission " + strName + " [" + DateTime.Now.ToString() + "]";
                                                oRequest.AddResult(intRequest, intItem, intNumber, strResult);
                                            }
                                            else
                                            {
                                                DataSet dsVirtualHosts = oVMWare.GetMicrosoftHosts();
                                                if (boolVirtualCOM == true)
                                                {
                                                    try
                                                    {
                                                        foreach (DataRow drVirtualHost in dsVirtualHosts.Tables[0].Rows)
                                                        {
                                                            Type typeVSClass = typeof(VMVirtualServerClass);
                                                            string strHost = drVirtualHost["name"].ToString();
                                                            oLog.AddEvent(strName, strSerial, "DECOMMISSION: Trying Virtual Host [" + strHost + "]", LoggingType.Information);
                                                            Type typeDCOM = Type.GetTypeFromCLSID(typeVSClass.GUID, strHost, true);
                                                            object objDCOM = Activator.CreateInstance(typeDCOM);
                                                            VMVirtualServerClass oVirtualServer = (VMVirtualServerClass)System.Runtime.InteropServices.Marshal.CreateWrapperOfType(objDCOM, typeVSClass);
                                                            VMVirtualMachine oMachine = oVirtualServer.FindVirtualMachine(strName);
                                                            if (oMachine != null && oMachine.Name.ToUpper() == strName.ToUpper())
                                                            {
                                                                boolFound = true;
                                                                oLog.AddEvent(strName, strSerial, "DECOMMISSION: Found Virtual Object [" + oMachine.ConfigID + "] on " + strHost, LoggingType.Information);
                                                                if (oMachine.State == VMVMState.vmVMState_Running || oMachine.State == VMVMState.vmVMState_Paused)
                                                                {
                                                                    oLog.AddEvent(strName, strSerial, "DECOMMISSION: Attempting to power down", LoggingType.Information);
                                                                    VMTask oMachineTask = oMachine.TurnOff();
                                                                    if (oMachineTask != null)
                                                                    {
                                                                        oMachineTask.WaitForCompletion(-1);
                                                                        boolDecom = true;
                                                                        oLog.AddEvent(strName, strSerial, "DECOMMISSION: Finished Powering Off", LoggingType.Information);
                                                                        strResult = "Finished Powering Off " + strName + " [" + DateTime.Now.ToString() + "]";
                                                                        oRequest.AddResult(intRequest, intItem, intNumber, strResult);
                                                                        break;
                                                                    }
                                                                    else
                                                                        strError = "There was a problem powering off the device (Not ready for a turn off)";
                                                                }
                                                                else
                                                                    strError = "There was a problem powering off the device (Virtual Server is not running...)";
                                                            }
                                                        }
                                                        if (boolFound == false)
                                                        {
                                                            strError = "There was a problem powering off the device (Could Not Find Computer " + strName + " on ANY Microsoft Virtual Hosts)";
                                                        }
                                                    }
                                                    catch (Exception exVirtual)
                                                    {
                                                        strError = "VMWare Service (DECOMMISSION): " + "(Error Message: " + exVirtual.Message + ") ~ (Source: " + exVirtual.Source + ") (Stack Trace: " + exVirtual.StackTrace + ") [" + System.Environment.UserName + "]";
                                                        oEventLog.WriteEntry(strError, EventLogEntryType.Error);
                                                    }
                                                }
                                                else
                                                {
                                                    oLog.AddEvent(strName, strSerial, "DECOMMISSION: Attempting to shutdown using the script and IP address (" + strDecomIP + ")", LoggingType.Information);
                                                    int intDomain = 0;
                                                    Int32.TryParse(oServer.Get(intServer, "domainid"), out intDomain);
                                                    if (intDomain > 0)
                                                    {
                                                        int intTestDomain = 0;
                                                        while (intTestDomain >= 0)
                                                        {
                                                            if (intTestDomain == 0)
                                                            {
                                                                if (intDomain == 1)
                                                                    intTestDomain = 2;
                                                                else if (intDomain == 2)
                                                                    intTestDomain = 1;
                                                                else
                                                                    intTestDomain = -1;
                                                            }
                                                            else
                                                            {
                                                                intDomain = intTestDomain;
                                                                intTestDomain = -1;
                                                            }
                                                            oLog.AddEvent(strName, strSerial, "DECOMMISSION: Trying domain " + oDomain.Get(intDomain, "name"), LoggingType.Information);
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
                                                                string strHostname = oFunction.HostName(intServer, "HOSTNAME_DECOM", strDecomIP, strScripts, strAdminUser, strAdminPass, 1);
                                                                if (strHostname.ToUpper() != strName.ToUpper())
                                                                    strError = "The hostname (" + strHostname.ToUpper() + ") does not match the server name (" + strName.ToUpper() + ")";
                                                                else
                                                                    oLog.AddEvent(strName, strSerial, "DECOMMISSION: The hostname (" + strHostname.ToUpper() + ") matches the server name (" + strName.ToUpper() + ")", LoggingType.Information);
                                                            }
                                                            if (strError == "")
                                                            {
                                                                // Attempt first to shutdown using a script
                                                                DateTime _now = DateTime.Now;
                                                                string strNow = _now.Day.ToString() + _now.Month.ToString() + _now.Year.ToString() + _now.Hour.ToString() + _now.Minute.ToString() + _now.Second.ToString() + _now.Millisecond.ToString();
                                                                string strDecomPath = strScripts + strSub + strName + "_" + strNow + "_shutdown_";
                                                                string strDecomScript = strDecomPath + "reboot.vbs";
                                                                StreamWriter oDecomWriter = new StreamWriter(strDecomScript);
                                                                oDecomWriter.WriteLine("Set OpSysSet = GetObject(\"winmgmts:{impersonationLevel=impersonate,(Shutdown)}\").ExecQuery(\"select * from Win32_OperatingSystem where Primary=true\")");
                                                                oDecomWriter.WriteLine("For Each OpSys In OpSysSet");
                                                                oDecomWriter.WriteLine("Debug \"OpSys.Win32Shutdown return: \" & OpSys.Win32Shutdown(5)");
                                                                oDecomWriter.WriteLine("Next");
                                                                oDecomWriter.Flush();
                                                                oDecomWriter.Close();
                                                                oLog.AddEvent(strName, strSerial, "DECOMMISSION: The shutdown script is running...", LoggingType.Information);
                                                                int intDecomReturn = oFunction.ExecuteVBScript(intServer, false, true, "SHUTDOWN", strName, strSerial, strDecomIP, strDecomScript, strDecomPath, "SHUTDOWN", "%windir%\\system32\\wscript.exe", "OPTIONS\\CV_DECOM_SHUTDOWN", "VBS", "", strScripts, strAdminUser, strAdminPass, 5, true, false, intLogging, boolDeleteScriptFiles);
                                                                oLog.AddEvent(strName, strSerial, "DECOMMISSION: The shutdown script finished...waiting for the IP address (" + strDecomIP + ") to stop responding...", LoggingType.Information);
                                                                // Check IP address for response
                                                                bool boolPinged = true;
                                                                for (int ii = 0; ii < 60 && boolPinged == true; ii++)
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
                                                                    boolPinged = (strStatus == "SUCCESS");
                                                                }
                                                                if (boolPinged == false)
                                                                {
                                                                    intTestDomain = -1;
                                                                    oLog.AddEvent(strName, strSerial, "DECOMMISSION: The server has been powered down", LoggingType.Information);
                                                                    string strVirtualFile = strScripts + strSub + strName + "_" + strNow + ".vbs";
                                                                    string strVirtualFilePath = strScripts + strSub + strName + "_";
                                                                    StreamWriter oWriter1 = new StreamWriter(strVirtualFile);
                                                                    oWriter1.WriteLine("dim strDirectory");
                                                                    oWriter1.WriteLine("dim res");
                                                                    oWriter1.WriteLine("strDirectory = Wscript.Arguments(0)");
                                                                    oWriter1.WriteLine("Set objFSO = CreateObject(\"Scripting.FileSystemObject\")");
                                                                    oWriter1.WriteLine("If objFSO.FolderExists(strDirectory) Then");
                                                                    oWriter1.WriteLine("objFSO.MoveFolder strDirectory, strDirectory & \"" + strDecomSuffix + "\"");
                                                                    oWriter1.WriteLine("res=1");
                                                                    oWriter1.WriteLine("Else");
                                                                    oWriter1.WriteLine("res=0");
                                                                    oWriter1.WriteLine("End If");
                                                                    oWriter1.WriteLine("wscript.quit(res)");
                                                                    oWriter1.Flush();
                                                                    oWriter1.Close();
                                                                    foreach (DataRow drVirtualHost in dsVirtualHosts.Tables[0].Rows)
                                                                    {
                                                                        string strVirtualHostName = drVirtualHost["name"].ToString();
                                                                        int intVirtualHostEnvironment = Int32.Parse(drVirtualHost["environment"].ToString());
                                                                        Variables oVariableVirtualHost = new Variables(intVirtualHostEnvironment);
                                                                        string strVirtualHostUser = oVariableVirtualHost.Domain() + "\\" + oVariableVirtualHost.ADUser();
                                                                        string strVirtualHostPass = oVariableVirtualHost.ADPassword();
                                                                        string strVirtualHostIP = oFunction.PingName(strVirtualHostName); ;
                                                                        string strVirtualHostPath = drVirtualHost["virtual_dir"].ToString() + "\\" + strName;
                                                                        oLog.AddEvent(strName, strSerial, "DECOMMISSION: Trying virtual host [" + strVirtualHostName + "]", LoggingType.Information);
                                                                        //   Execute the Script
                                                                        int intDecomScript = oFunction.ExecuteVBScript(intServer, false, true, "DECOM_VIRTUAL", strName, strSerial, strVirtualHostIP, strVirtualFile, strVirtualFilePath, "Decom_Virtual", "%windir%\\system32\\wscript.exe", "CV_DECOM_VIRTUAL", "VBS", strVirtualHostPath, strScripts, strVirtualHostUser, strVirtualHostPass, 1, true, false, intLogging, boolDeleteScriptFiles);
                                                                        AuditStatus oDecomStatus = (AuditStatus)intDecomScript;
                                                                        if (oDecomStatus == AuditStatus.Success)
                                                                        {
                                                                            boolDecom = true;
                                                                            oLog.AddEvent(strName, strSerial, "DECOMMISSION: Finished Renaming Virtual Files", LoggingType.Information);
                                                                            strResult = "Finished Renaming Virtual Files " + strName + " [" + DateTime.Now.ToString() + "]";
                                                                            oRequest.AddResult(intRequest, intItem, intNumber, strResult);
                                                                            break;
                                                                        }
                                                                        else if (oDecomStatus == AuditStatus.TimedOut)
                                                                        {
                                                                            strError = "There was a problem powering off the device (A timeout error was encountered in the DECOMMISSION script)";
                                                                        }
                                                                    }
                                                                    if (boolDecom == false)
                                                                        strError = "There was a problem powering off the device (Could not locate the virtual server guest in the current virtual server host list)";
                                                                }
                                                                else if (intTestDomain < 0)
                                                                    strError = "There was a problem powering off the device (make sure the domain is correct)";
                                                            }
                                                        }
                                                    }
                                                    else
                                                        strError = "There was a problem powering off the device (The domain of the server is invalid)";
                                                }
                                            }
                                        }
                                        else
                                        {
                                            oLog.AddEvent(strName, strSerial, "DECOMMISSION: Beginning VMware Virtual decommission process", LoggingType.Information);
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
                                                                    oLog.AddEvent(strName, strSerial, "DECOMMISSION: Initiating VMware power down command (PowerOffVM_Task)", LoggingType.Information);
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
                                                                    oLog.AddEvent(strName, strSerial, "DECOMMISSION: Initiating VMware rename command (Rename_Task) to change name to " + strName.ToLower() + strDecomSuffix, LoggingType.Information);
                                                                    ManagedObjectReference _task_rename = _service.Rename_Task(_vm_power, strName.ToLower() + strDecomSuffix);
                                                                    TaskInfo _info_rename = (TaskInfo)oVMWare.getObjectProperty(_task_rename, "info");
                                                                    while (_info_rename.state == TaskInfoState.running)
                                                                        _info_rename = (TaskInfo)oVMWare.getObjectProperty(_task_rename, "info");
                                                                    if (_info_rename.state == TaskInfoState.success)
                                                                    {
                                                                        boolDecom = true;
                                                                        oLog.AddEvent(strName, strSerial, "DECOMMISSION: Finished Rename", LoggingType.Information);
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
                                                            //oLog.AddEvent(strName, strSerial, "DECOMMISSION: Logged out of VMware", LoggingType.Information);
                                                        }
                                                    }
                                                }
                                            }
                                            if (boolFound == false)
                                            {
                                                oLog.AddEvent(strName, strSerial, "DECOMMISSION: VMware machine not found where it should have been. Beginning to search all environments...", LoggingType.Information);
                                                dsVirtualCenter = oVMWare.GetVirtualCenters(1);
                                                foreach (DataRow drVirtualCenter in dsVirtualCenter.Tables[0].Rows)
                                                {
                                                    if (boolFound == true)
                                                        break;
                                                    int intVirtualCenter = Int32.Parse(drVirtualCenter["id"].ToString());
                                                    DataSet dsDataCenter = oVMWare.GetDatacenters(intVirtualCenter, 1);
                                                    foreach (DataRow drDataCenter in dsDataCenter.Tables[0].Rows)
                                                    {
                                                        if (boolFound == true)
                                                            break;
                                                        bool boolAlready = false;
                                                        string[] strAlreadys = strAlready.Split(strSplit);
                                                        for (int ii = 0; ii < strAlreadys.Length; ii++)
                                                        {
                                                            if (strAlreadys[ii].Trim() != "")
                                                            {
                                                                if (strAlreadys[ii].Trim().ToUpper() == drDataCenter["id"].ToString())
                                                                {
                                                                    boolAlready = true;
                                                                    break;
                                                                }
                                                            }
                                                        }
                                                        if (boolAlready == false)
                                                        {
                                                            string strConnect = oVMWare.ConnectDEBUG(drVirtualCenter["url"].ToString(), Int32.Parse(drVirtualCenter["environment"].ToString()), drDataCenter["name"].ToString());
                                                            if (strConnect == "")
                                                            {
                                                                VimService _service = oVMWare.GetService();
                                                                oLog.AddEvent(strName, strSerial, "DECOMMISSION: Trying (ALL) " + drDataCenter["name"].ToString() + " on " + drVirtualCenter["name"].ToString(), LoggingType.Information);
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
                                                                            oLog.AddEvent(strName, strSerial, "DECOMMISSION: Initiating VMware power down command (PowerOffVM_Task)", LoggingType.Information);
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
                                                                            oLog.AddEvent(strName, strSerial, "DECOMMISSION: Initiating VMware rename command (Rename_Task) to change name to " + strName.ToLower() + strDecomSuffix, LoggingType.Information);
                                                                            ManagedObjectReference _task_rename = _service.Rename_Task(_vm_power, strName.ToLower() + strDecomSuffix);
                                                                            TaskInfo _info_rename = (TaskInfo)oVMWare.getObjectProperty(_task_rename, "info");
                                                                            while (_info_rename.state == TaskInfoState.running)
                                                                                _info_rename = (TaskInfo)oVMWare.getObjectProperty(_task_rename, "info");
                                                                            if (_info_rename.state == TaskInfoState.success)
                                                                            {
                                                                                boolDecom = true;
                                                                                oLog.AddEvent(strName, strSerial, "DECOMMISSION: Finished Rename", LoggingType.Information);
                                                                                strResult += "<br/>Finished Decommissioning " + strName + " [" + DateTime.Now.ToString() + "]";
                                                                            }
                                                                            else
                                                                            {
                                                                                strResult += "<br/>Virtual Machine " + strName + " Was Not Renamed";
                                                                                oAsset.UpdateDecommissionRunning(intAsset, -1);
                                                                            }
                                                                        }
                                                                        else
                                                                        {
                                                                            strResult += "<br/>Virtual Machine " + strName + " Was Not Powered Off";
                                                                            oAsset.UpdateDecommissionRunning(intAsset, -1);
                                                                        }
                                                                        oRequest.AddResult(intRequest, intItem, intNumber, strResult);
                                                                        oLog.AddEvent(strName, strSerial, "DECOMMISSION: Completed Decommission", LoggingType.Information);
                                                                    }
                                                                }
                                                                catch (Exception exVmware)
                                                                {
                                                                    oLog.AddEvent(strName, strSerial, "DECOMMISSION: There was a problem searching ALL in " + drDataCenter["name"].ToString() + " on " + drVirtualCenter["name"].ToString() + " ~ " + exVmware.Message, LoggingType.Warning);
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
                                                        else
                                                            oLog.AddEvent(strName, strSerial, "DECOMMISSION: Skipping (ALL) " + drDataCenter["name"].ToString() + " on " + drVirtualCenter["name"].ToString(), LoggingType.Information);
                                                    }
                                                }
                                                if (boolFound == false)
                                                    strError = "There was a problem powering off the device (Could Not Find Computer " + strName + " on ANY Virtual Center servers)";
                                            }
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
                            oAsset.UpdateDecommission(intAsset, DateTime.Now.AddDays(14), 1, strName);
                            oAsset.AddStatus(intAsset, strName, (int)AssetStatus.Decommissioned, 0, DateTime.Now);
                            oServer.UpdateDecommissioned(intServer, DateTime.Now.ToString());
                            oServer.UpdateAssetDecom(intServer, intAsset, DateTime.Now.ToString());
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
            }
            catch (Exception ex)
            {
                if (ex.Message.ToUpper().StartsWith("A TRANSPORT-LEVEL ERROR") == false)
                {
                    if (ex.Message.ToUpper().Contains("HTTP STATUS 503") == false)
                    {
                        string strError = "VMWare Service (DECOMMISSION): " + strName + " (SERVERID = " + intServer.ToString() + ") (Error Message: " + ex.Message + ") ~ (Source: " + ex.Source + ") (Stack Trace: " + ex.StackTrace + ") [" + System.Environment.UserName + "]";
                        oEventLog.WriteEntry(strError, EventLogEntryType.Error);
                        SystemError(strError);
                    }
                }
            }
            ServiceTickDecomDestroy();
        }
        private void ServiceTickDecomDestroy()
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

                DataSet ds = oAsset.GetDecommissionDestroys(strServerTypesDecom, DateTime.Now);
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
                    DataSet dsAsset = oServer.GetAsset(intAsset);
                    if (dsAsset.Tables[0].Rows.Count > 0)
                    {
                        intServer = Int32.Parse(dsAsset.Tables[0].Rows[0]["id"].ToString());
                        DataSet dsServer = oServer.Get(intServer);
                        if (dsServer.Tables[0].Rows.Count > 0)
                            intAnswer = Int32.Parse(dsServer.Tables[0].Rows[0]["answerid"].ToString());
                    }
                    int intAssetType = Int32.Parse(dr["typeid"].ToString());
                    int intUser = Int32.Parse(dr["userid"].ToString());
                    strName = dr["name"].ToString();
                    string strSerial = oAsset.Get(intAsset, "serial");
                    string strResult = "No information...";
                    bool boolMissedFix = (dr["missed_fix"].ToString() != "");

                    if (dr["dr"].ToString() == "1")
                    {
                        // Just set DR servers to completed
                        oAsset.UpdateDecommissionDestroy(intAsset);
                        oAsset.AddStatus(intAsset, strName, (int)AssetStatus.Disposed, 0, DateTime.Now);
                        oLog.AddEvent(strName, strSerial, "DESTROY: DR asset set to disposed", LoggingType.Information);
                    }
                    else
                    {
                        if (boolMissedFix == false && oResourceRequest.GetAllService(intRequest, intDestroyErrorService, intNumber).Tables[0].Rows.Count == 0)
                        {
                            // Check Decommission
                            if (dsAsset.Tables[0].Rows.Count > 0)
                            {
                                bool boolProcess = false;
                                int intClass = Int32.Parse(dsAsset.Tables[0].Rows[0]["classid"].ToString());
                                int intEnv = Int32.Parse(dsAsset.Tables[0].Rows[0]["environmentid"].ToString());
                                int intAddress = Int32.Parse(dsAsset.Tables[0].Rows[0]["addressid"].ToString());

                                if (oClass.IsProd(intClass))
                                {
                                    if (intBuildProd == 1)
                                    {
                                        if (intBuildProdOffHours == 0)
                                        {
                                            boolProcess = true;
                                            if (intLogging > 0)
                                                oEventLog.WriteEntry("SERVERNAME: " + strName + " (VMware): " + "DESTROY: PROD Classes have been enabled for builds", EventLogEntryType.Information);
                                        }
                                        else if (DateTime.Now.Hour < 7 || DateTime.Now.Hour > 19)
                                        {
                                            boolProcess = true;
                                            if (intLogging > 0)
                                                oEventLog.WriteEntry("SERVERNAME: " + strName + " (VMware): " + "DESTROY: PROD Classes have been enabled for OFF PEAK HOURS (7AM - 7PM) Current Time : " + DateTime.Now.ToShortTimeString(), EventLogEntryType.Information);
                                        }
                                        else if (intLogging > 0)
                                            oEventLog.WriteEntry("SERVERNAME: " + strName + " (VMware): " + "DESTROY: PROD Classes are not enabled for DURING PEAK HOURS (7AM - 7PM) builds", EventLogEntryType.Warning);
                                    }
                                    else if (intLogging > 0)
                                        oEventLog.WriteEntry("SERVERNAME: " + strName + " (VMware): " + "DESTROY: PROD Classes are not enabled for builds", EventLogEntryType.Warning);
                                }
                                if (oClass.IsQA(intClass))
                                {
                                    if (intBuildQA == 1)
                                    {
                                        if (intBuildQAOffHours == 0)
                                        {
                                            boolProcess = true;
                                            if (intLogging > 0)
                                                oEventLog.WriteEntry("SERVERNAME: " + strName + " (VMware): " + "DESTROY: QA Classes have been enabled for builds", EventLogEntryType.Information);
                                        }
                                        else if (DateTime.Now.Hour < 7 || DateTime.Now.Hour > 19)
                                        {
                                            boolProcess = true;
                                            if (intLogging > 0)
                                                oEventLog.WriteEntry("SERVERNAME: " + strName + " (VMware): " + "DESTROY: QA Classes have been enabled for OFF PEAK HOURS (7AM - 7PM) Current Time : " + DateTime.Now.ToShortTimeString(), EventLogEntryType.Information);
                                        }
                                        else if (intLogging > 0)
                                            oEventLog.WriteEntry("SERVERNAME: " + strName + " (VMware): " + "DESTROY: QA Classes are not enabled for DURING PEAK HOURS (7AM - 7PM) builds", EventLogEntryType.Warning);
                                    }
                                    else if (intLogging > 0)
                                        oEventLog.WriteEntry("SERVERNAME: " + strName + " (VMware): " + "DESTROY: QA Classes are not enabled for builds", EventLogEntryType.Warning);
                                }
                                if (oClass.IsTestDev(intClass))
                                {
                                    if (intBuildTest == 1)
                                    {
                                        boolProcess = true;
                                        if (intLogging > 0)
                                            oEventLog.WriteEntry("SERVERNAME: " + strName + " (VMware): " + "DESTROY: TEST Classes have been enabled for builds", EventLogEntryType.Information);
                                    }
                                    else if (intLogging > 0)
                                        oEventLog.WriteEntry("SERVERNAME: " + strName + " (VMware): " + "DESTROY: TEST Classes are not enabled for builds", EventLogEntryType.Warning);
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
                                        if (intAssetType == intTypeVirtual)
                                        {
                                            oLog.AddEvent(strName, strSerial, "DESTROY: Beginning Microsoft virtual environment", LoggingType.Information);
                                            if (boolSkipVirtual == true)
                                            {
                                                boolDestroy = true;
                                                oLog.AddEvent(strName, strSerial, "DESTROY: Finished Destroy (SKIP)", LoggingType.Information);
                                                strResult = "Finished Destroy " + strName + " [" + DateTime.Now.ToString() + "]";
                                                oRequest.AddResult(intRequest, intItem, intNumber, strResult);
                                            }
                                            else
                                            {
                                                DataSet dsVirtualHosts = oVMWare.GetMicrosoftHosts();
                                                if (boolVirtualCOM == true)
                                                {
                                                    try
                                                    {
                                                        foreach (DataRow drVirtualHost in dsVirtualHosts.Tables[0].Rows)
                                                        {
                                                            Type typeVSClass = typeof(VMVirtualServerClass);
                                                            string strHost = drVirtualHost["name"].ToString();
                                                            oLog.AddEvent(strName, strSerial, "DESTROY: Trying Virtual Host [" + strHost + "]", LoggingType.Information);
                                                            Type typeDCOM = Type.GetTypeFromCLSID(typeVSClass.GUID, strHost, true);
                                                            object objDCOM = Activator.CreateInstance(typeDCOM);
                                                            VMVirtualServerClass oVirtualServer = (VMVirtualServerClass)System.Runtime.InteropServices.Marshal.CreateWrapperOfType(objDCOM, typeVSClass);
                                                            VMVirtualMachine oMachine = oVirtualServer.FindVirtualMachine(strName);
                                                            if (oMachine != null && oMachine.Name.ToUpper() == strName.ToUpper())
                                                            {
                                                                boolFound = true;
                                                                oLog.AddEvent(strName, strSerial, "DESTROY: Found Virtual Object [" + oMachine.ConfigID + "] on " + strHost, LoggingType.Information);
                                                                if (oMachine.State == VMVMState.vmVMState_TurnedOff)
                                                                {
                                                                    oLog.AddEvent(strName, strSerial, "DESTROY: Attempting to delete the virtual machine object", LoggingType.Information);
                                                                    oVirtualServer.DeleteVirtualMachine(oMachine);
                                                                    oLog.AddEvent(strName, strSerial, "DESTROY: Virtual Machine Object Deleted", LoggingType.Information);
                                                                    oLog.AddEvent(strName, strSerial, "DESTROY: Attempting to delete the virtual machine files", LoggingType.Information);
                                                                    DeleteDirectory(drVirtualHost["virtual_dir"].ToString() + "\\" + strName);
                                                                    oLog.AddEvent(strName, strSerial, "DESTROY: Virtual Machine Files Deleted", LoggingType.Information);
                                                                    boolDestroy = true;
                                                                    strResult = "Finished Destroying " + strName + " [" + DateTime.Now.ToString() + "]";
                                                                    oRequest.AddResult(intRequest, intItem, intNumber, strResult);
                                                                    oLog.AddEvent(strName, strSerial, "DESTROY: Finished...sending Initiating Decom Request", LoggingType.Information);
                                                                    break;
                                                                }
                                                                else
                                                                    strError = "There was a problem destroying the device (Virtual Machine is not turned off...)";
                                                            }
                                                        }
                                                        if (boolFound == false)
                                                        {
                                                            strError = "There was a problem destroying the device (Could Not Find Computer " + strName + " on ANY Microsoft Virtual Hosts)";
                                                            //strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DEVELOPER_ERROR");
                                                            //oFunction.SendEmail("Auto-Provisioning DESTROY Warning", strEMailIdsBCC, "", "", "Auto-Provisioning DESTROY Warning", "<p><b>This message is to inform you that there was a problem DESTROYING (deleting) the computer object " + strName + ".</b><p><p>If this computer does exist, it will need to be destroyed manually.</p>", true, false);
                                                            //boolDestroy = true;
                                                            //strResult = "Finished Destroying " + strName + " [" + DateTime.Now.ToString() + "]";
                                                            //oRequest.AddResult(intRequest, intItem, intNumber, strResult);
                                                            //oLog.AddEvent(strName, strSerial, "DESTROY: Could not find computer...skipping...sending Initiating Decom Request", LoggingType.Information);
                                                        }
                                                    }
                                                    catch (Exception exVirtual)
                                                    {
                                                        strError = "VMWare Service (DESTROY): " + "(Error Message: " + exVirtual.Message + ") ~ (Source: " + exVirtual.Source + ") (Stack Trace: " + exVirtual.StackTrace + ") [" + System.Environment.UserName + "]";
                                                        oEventLog.WriteEntry(strError, EventLogEntryType.Error);
                                                    }
                                                }
                                                else
                                                {
                                                    oLog.AddEvent(strName, strSerial, "DESTROY: Attempting to delete the virtual host files", LoggingType.Information);
                                                    DateTime _now = DateTime.Now;
                                                    string strNow = _now.Day.ToString() + _now.Month.ToString() + _now.Year.ToString() + _now.Hour.ToString() + _now.Minute.ToString() + _now.Second.ToString() + _now.Millisecond.ToString();
                                                    string strVirtualFile = strScripts + strSub + strName + "_" + strNow + ".vbs";
                                                    string strVirtualFilePath = strScripts + strSub + strName + "_";
                                                    StreamWriter oWriter1 = new StreamWriter(strVirtualFile);
                                                    oWriter1.WriteLine("dim strDirectory");
                                                    oWriter1.WriteLine("dim strDirectory2");
                                                    oWriter1.WriteLine("dim res");
                                                    oWriter1.WriteLine("dim dash");
                                                    oWriter1.WriteLine("strDirectory = Wscript.Arguments(0)");
                                                    oWriter1.WriteLine("dash = InStr(1,strDirectory,\"-\")");
                                                    oWriter1.WriteLine("If dash>0 Then");
                                                    oWriter1.WriteLine("strDirectory2 = mid(strDirectory,1,dash-1)");
                                                    oWriter1.WriteLine("End If");
                                                    oWriter1.WriteLine("Set objFSO = CreateObject(\"Scripting.FileSystemObject\")");
                                                    oWriter1.WriteLine("If objFSO.FolderExists(strDirectory) Then");
                                                    oWriter1.WriteLine("objFSO.DeleteFolder strDirectory");
                                                    oWriter1.WriteLine("res=1");
                                                    oWriter1.WriteLine("ElseIf objFSO.FolderExists(strDirectory2) Then");
                                                    oWriter1.WriteLine("objFSO.DeleteFolder strDirectory2");
                                                    oWriter1.WriteLine("res=1");
                                                    oWriter1.WriteLine("Else");
                                                    oWriter1.WriteLine("res=0");
                                                    oWriter1.WriteLine("End If");
                                                    oWriter1.WriteLine("wscript.quit(res)");
                                                    oWriter1.Flush();
                                                    oWriter1.Close();
                                                    foreach (DataRow drVirtualHost in dsVirtualHosts.Tables[0].Rows)
                                                    {
                                                        string strVirtualHostName = drVirtualHost["name"].ToString();
                                                        int intVirtualHostEnvironment = Int32.Parse(drVirtualHost["environment"].ToString());
                                                        Variables oVariableVirtualHost = new Variables(intVirtualHostEnvironment);
                                                        string strVirtualHostUser = oVariableVirtualHost.Domain() + "\\" + oVariableVirtualHost.ADUser();
                                                        string strVirtualHostPass = oVariableVirtualHost.ADPassword();
                                                        string strVirtualHostIP = oFunction.PingName(strVirtualHostName); ;
                                                        string strVirtualHostPath = drVirtualHost["virtual_dir"].ToString() + "\\" + strName + strDecomSuffix;
                                                        oLog.AddEvent(strName, strSerial, "DESTROY: Trying virtual host [" + strVirtualHostName + "]", LoggingType.Information);
                                                        //   Execute the Script
                                                        int intDecomScript = oFunction.ExecuteVBScript(intServer, false, true, "DESTROY_VIRTUAL", strName, strSerial, strVirtualHostIP, strVirtualFile, strVirtualFilePath, "Destroy_Virtual", "%windir%\\system32\\wscript.exe", "CV_DESTROY_VIRTUAL", "VBS", strVirtualHostPath, strScripts, strVirtualHostUser, strVirtualHostPass, 1, true, false, intLogging, boolDeleteScriptFiles);
                                                        AuditStatus oDecomStatus = (AuditStatus)intDecomScript;
                                                        if (oDecomStatus == AuditStatus.Success)
                                                        {
                                                            boolDestroy = true;
                                                            oLog.AddEvent(strName, strSerial, "DESTROY: Finished Deleting Virtual Files", LoggingType.Information);
                                                            strResult = "Finished Deleting Virtual Files " + strName + " [" + DateTime.Now.ToString() + "]";
                                                            oRequest.AddResult(intRequest, intItem, intNumber, strResult);
                                                            break;
                                                        }
                                                        else if (oDecomStatus == AuditStatus.TimedOut)
                                                        {
                                                            strError = "There was a problem destroying the device (A timeout was encountered in the DESTROY script)";
                                                        }
                                                    }
                                                    if (boolDestroy == false)
                                                        strError = "There was a problem destroying the device (Could not locate the virtual server guest in the current virtual server host list)";
                                                }
                                            }
                                        }
                                        else
                                        {
                                            oLog.AddEvent(strName, strSerial, "DESTROY: Beginning VMware virtual environment", LoggingType.Information);
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
                                                            ManagedObjectReference _vm_power = oVMWare.GetVM(strName.ToLower() + strDecomSuffix);
                                                            if (_vm_power != null && _vm_power.Value != "")
                                                            {
                                                                oLog.AddEvent(strName, strSerial, "DESTROY: Found VMware Object [" + _vm_power.Value + "] in " + drDataCenter["name"].ToString() + " on " + drVirtualCenter["name"].ToString(), LoggingType.Information);
                                                                boolFound = true;
                                                                VirtualMachineRuntimeInfo oRuntime = (VirtualMachineRuntimeInfo)oVMWare.getObjectProperty(_vm_power, "runtime");
                                                                if (oRuntime.powerState != VirtualMachinePowerState.poweredOff)
                                                                {
                                                                    oLog.AddEvent(strName, strSerial, "DESTROY: Powered Back On - Destroy Cancelled", LoggingType.Information);
                                                                    strResult = "Virtual Machine " + strName + " Was Powered Back On - Destroy Failed";
                                                                    // Reset status?
                                                                }
                                                                else
                                                                {
                                                                    oLog.AddEvent(strName, strSerial, "DESTROY: Initiating VMware destroy command (Destroy_Task)", LoggingType.Information);
                                                                    ManagedObjectReference _task_power = _service.Destroy_Task(_vm_power);
                                                                    TaskInfo _info_power = (TaskInfo)oVMWare.getObjectProperty(_task_power, "info");
                                                                    while (_info_power.state == TaskInfoState.running)
                                                                        _info_power = (TaskInfo)oVMWare.getObjectProperty(_task_power, "info");
                                                                    if (_info_power.state == TaskInfoState.success)
                                                                    {
                                                                        boolDestroy = true;
                                                                        strResult = "Finished Destroying " + strName + " [" + DateTime.Now.ToString() + "]";
                                                                        oLog.AddEvent(strName, strSerial, "DESTROY: Finished...sending Initiating Decom Request", LoggingType.Information);
                                                                    }
                                                                    else
                                                                    {
                                                                        strResult = "Virtual Machine " + strName + " Was Not Destroyed";
                                                                        oAsset.UpdateDecommissionRunning(intAsset, -1);
                                                                    }
                                                                }
                                                                oRequest.AddResult(intRequest, intItem, intNumber, strResult);
                                                                oLog.AddEvent(strName, strSerial, "DESTROY: Finished Destroy", LoggingType.Information);
                                                            }
                                                        }
                                                        catch (Exception exVmware)
                                                        {
                                                            oLog.AddEvent(strName, strSerial, "DESTROY: There was a problem searching in " + drDataCenter["name"].ToString() + " on " + drVirtualCenter["name"].ToString() + " ~ " + exVmware.Message, LoggingType.Warning);
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
                                                oLog.AddEvent(strName, strSerial, "DESTROY: Server was not found where it should have been. Start searching all known VMware environments...", LoggingType.Information);
                                                dsVirtualCenter = oVMWare.GetVirtualCenters(1);
                                                foreach (DataRow drVirtualCenter in dsVirtualCenter.Tables[0].Rows)
                                                {
                                                    if (boolFound == true)
                                                        break;
                                                    int intVirtualCenter = Int32.Parse(drVirtualCenter["id"].ToString());
                                                    DataSet dsDataCenter = oVMWare.GetDatacenters(intVirtualCenter, 1);
                                                    foreach (DataRow drDataCenter in dsDataCenter.Tables[0].Rows)
                                                    {
                                                        if (boolFound == true)
                                                            break;
                                                        bool boolAlready = false;
                                                        string[] strAlreadys = strAlready.Split(strSplit);
                                                        for (int ii = 0; ii < strAlreadys.Length; ii++)
                                                        {
                                                            if (strAlreadys[ii].Trim() != "")
                                                            {
                                                                if (strAlreadys[ii].Trim().ToUpper() == drDataCenter["id"].ToString())
                                                                {
                                                                    boolAlready = true;
                                                                    break;
                                                                }
                                                            }
                                                        }
                                                        if (boolAlready == false)
                                                        {
                                                            string strConnect = oVMWare.ConnectDEBUG(drVirtualCenter["url"].ToString(), Int32.Parse(drVirtualCenter["environment"].ToString()), drDataCenter["name"].ToString());
                                                            if (strConnect == "")
                                                            {
                                                                VimService _service = oVMWare.GetService();
                                                                oLog.AddEvent(strName, strSerial, "DESTROY: Trying (ALL) " + drDataCenter["name"].ToString() + " on " + drVirtualCenter["name"].ToString(), LoggingType.Information);
                                                                try
                                                                {
                                                                    ManagedObjectReference _vm_power = oVMWare.GetVM(strName.ToLower() + strDecomSuffix);
                                                                    if (_vm_power != null && _vm_power.Value != "")
                                                                    {
                                                                        oLog.AddEvent(strName, strSerial, "DESTROY: Found VMware Object [" + _vm_power.Value + "] in " + drDataCenter["name"].ToString() + " on " + drVirtualCenter["name"].ToString(), LoggingType.Information);
                                                                        boolFound = true;
                                                                        VirtualMachineRuntimeInfo oRuntime = (VirtualMachineRuntimeInfo)oVMWare.getObjectProperty(_vm_power, "runtime");
                                                                        if (oRuntime.powerState != VirtualMachinePowerState.poweredOff)
                                                                        {
                                                                            oLog.AddEvent(strName, strSerial, "DESTROY: Powered Back On - Destroy Cancelled", LoggingType.Information);
                                                                            strResult = "Virtual Machine " + strName + " Was Powered Back On - Destroy Failed";
                                                                        }
                                                                        else
                                                                        {
                                                                            oLog.AddEvent(strName, strSerial, "DESTROY: Initiating VMware destroy command (Destroy_Task)", LoggingType.Information);
                                                                            ManagedObjectReference _task_power = _service.Destroy_Task(_vm_power);
                                                                            TaskInfo _info_power = (TaskInfo)oVMWare.getObjectProperty(_task_power, "info");
                                                                            while (_info_power.state == TaskInfoState.running)
                                                                                _info_power = (TaskInfo)oVMWare.getObjectProperty(_task_power, "info");
                                                                            if (_info_power.state == TaskInfoState.success)
                                                                            {
                                                                                boolDestroy = true;
                                                                                strResult = "Finished Destroying " + strName + " [" + DateTime.Now.ToString() + "]";
                                                                                oLog.AddEvent(strName, strSerial, "DESTROY: Finished...sending Initiating Decom Request", LoggingType.Information);
                                                                            }
                                                                            else
                                                                            {
                                                                                strResult = "Virtual Machine " + strName + " Was Not Destroyed";
                                                                                oAsset.UpdateDecommissionRunning(intAsset, -1);
                                                                            }
                                                                        }
                                                                        oRequest.AddResult(intRequest, intItem, intNumber, strResult);
                                                                        oLog.AddEvent(strName, strSerial, "DESTROY: Finished Destroy", LoggingType.Information);
                                                                    }
                                                                }
                                                                catch (Exception exVmware)
                                                                {
                                                                    oLog.AddEvent(strName, strSerial, "DESTROY: There was a problem searching ALL in " + drDataCenter["name"].ToString() + " on " + drVirtualCenter["name"].ToString() + " ~ " + exVmware.Message, LoggingType.Warning);
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
                                                                    //oLog.AddEvent(strName, strSerial, "DESTROY: Logged out of VMware", LoggingType.Information);
                                                                }
                                                            }
                                                        }
                                                        else
                                                            oLog.AddEvent(strName, strSerial, "DESTROY: Skipping (ALL) " + drDataCenter["name"].ToString() + " on " + drVirtualCenter["name"].ToString(), LoggingType.Information);
                                                    }
                                                }
                                                if (boolFound == false)
                                                {
                                                    strError = "There was a problem destroying the device (Could Not Find Computer " + strName + " on ANY Virtual Center servers)";
                                                    //strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DEVELOPER_ERROR");
                                                    //oFunction.SendEmail("Auto-Provisioning DESTROY Warning", strEMailIdsBCC, "", "", "Auto-Provisioning DESTROY Warning", "<p><b>This message is to inform you that there was a problem DESTROYING (deleting) the computer object " + strName + ".</b><p><p>If this computer does exist, it will need to be destroyed manually.</p>", true, false);
                                                    //boolDestroy = true;
                                                    //strResult = "Finished Destroying " + strName + " [" + DateTime.Now.ToString() + "]";
                                                    //oLog.AddEvent(strName, strSerial, "DESTROY: Could not find computer object...skipping...sending Initiating Decom Request", LoggingType.Information);
                                                    //oRequest.AddResult(intRequest, intItem, intNumber, strResult);
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        // Server is responding, was turned back on
                                        strError = "Server has been turned back on";
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
                                oFunction.SendEmail("Auto-Provisioning DESTROY Error", strEMailIdsBCC, "", "", "Auto-Provisioning DESTROY Error", "<p><b>This message is to inform you that there was a problem with the DESTROY process for server " + strName + ".</b><p><p>Error Message: " + strError + "</p>", true, false);
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
                                //Initialize Server Decomm Process
                                //strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_INVENTORY_MANAGER");
                                oLog.AddEvent(strName, strSerial, "Destroy: Initiating Tasks", LoggingType.Information);
                                oServerDecommission.InitiateDecom(intServer, intModel, strName, intRequest, intItem, intNumber, 0, 0,
                                                    intAssignPage, intViewPage, intEnvironment,
                                                    intIMDecommServiceId,
                                                    dsnServiceEditor, dsnAsset, dsnIP, strDSMADMC, boolMissedFix);
                                // Initiate Service Center Request
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
                    if (ex.Message.ToUpper().Contains("HTTP STATUS 503") == false)
                    {
                        string strError = "VMWare Service (DESTROY): " + strName + " (SERVERID = " + intServer.ToString() + ") (Error Message: " + ex.Message + ") ~ (Source: " + ex.Source + ") (Stack Trace: " + ex.StackTrace + ") [" + System.Environment.UserName + "]";
                        oEventLog.WriteEntry(strError, EventLogEntryType.Error);
                        SystemError(strError);
                    }
                }
            }
        }
        private void AddResult(int intServer, int intStep, int intType, string strResult, string strError)
        {
            OnDemand oOnDemand = new OnDemand(0, dsn);
            Servers oServer = new Servers(0, dsn);
            DataSet dsError = oServer.GetError(intServer, intStep);
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
                    string strName = oServer.GetName(intServer, boolUsePNCNaming);
                    string strIPAddress = oServer.GetIPBuild(intServer);
                    int intIPAddress = 0;
                    if (strIPAddress != "")
                        intIPAddress = Int32.Parse(strIPAddress);
                    if (intIPAddress > 0)
                        strIP = oIPAddresses.GetName(intIPAddress, 0);
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
                        bool boolActive = (dsActive.Tables[0].Rows.Count > 0);
                        if (boolActive == false)
                        {
                            DataSet dsServer = oServer.Get(intServer);
                            int intAnswer = Int32.Parse(dsServer.Tables[0].Rows[0]["answerid"].ToString());
                            int intMHS = 0;
                            Int32.TryParse(dsServer.Tables[0].Rows[0]["mhs"].ToString(), out intMHS);
                            int intDomain = Int32.Parse(dsServer.Tables[0].Rows[0]["domainid"].ToString());
                            int intDomainEnvironment = Int32.Parse(oDomain.Get(intDomain, "environment"));
                            bool boolPNC = (dsServer.Tables[0].Rows[0]["pnc"].ToString() == "1");
                            string strSource = "SERVER";
                            Variables oVariable = new Variables(intDomainEnvironment);
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
                                    ILaunchScript oScript1 = new SimpleLaunchWsh(strFile1, "", true, 30) as ILaunchScript;
                                    oScript1.Launch();
                                    bool boolOutputStart = ReadOutput(intServer, "INSTALL_" + dsInstalls.Tables[0].Rows[0]["name"].ToString() + "_START", strFile1Out, strName, "");
                                    if (boolOutputStart == false)
                                    {
                                        // 4th part - file has been copied, do the PSEXEC to install application
                                        System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo(strScripts + "psexec");
                                        info.WorkingDirectory = strScripts;
                                        info.Arguments = "\\\\" + strIP + " -u " + strAdminUser + " -p " + strAdminPass + " -h cmd.exe /c C:\\OPTIONS\\CV_INSTALL_" + intDetail.ToString() + ".BAT";
                                        oLog.AddEvent(strName, "", "PSEXEC Script = " + "\\\\" + strIP + " -u " + strAdminUser + " -p ***** -h cmd.exe /c C:\\OPTIONS\\CV_INSTALL_" + intDetail.ToString() + ".BAT", LoggingType.Information);
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
                                            // 3rd part - run the batch file to perform copy
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
                    oEventLog.WriteEntry("No Physical installations to run", EventLogEntryType.Information);
            }
            catch (Exception ex)
            {
                if (ex.Message.ToUpper().StartsWith("A TRANSPORT-LEVEL ERROR") == false)
                {
                    string strError = "VMWare Service (INSTALLATION): " + "(Error Message: " + ex.Message + ") ~ (Source: " + ex.Source + ") (Stack Trace: " + ex.StackTrace + ") [" + System.Environment.UserName + "]";
                    oEventLog.WriteEntry(strError, EventLogEntryType.Error);
                    SystemError(strError);
                }
            }
        }
        public VirtualDeviceConfigSpec Controller(int _bus_number, int _unit_number, int _controller_key, bool Is2012)
        {
            VirtualSCSIController scsi = new VirtualSCSIController();
            if (Is2012)
                scsi = new VirtualLsiLogicSASController();
            else
                scsi = new VirtualLsiLogicController();
            VirtualDeviceConnectInfo ci = new VirtualDeviceConnectInfo();
            ci.startConnected = false;
            scsi.key = _controller_key;
            scsi.controllerKey = 100;
            scsi.controllerKeySpecified = true;
            scsi.busNumber = _bus_number;
            scsi.hotAddRemove = true;
            scsi.hotAddRemoveSpecified = true;
            scsi.scsiCtlrUnitNumber = 7;
            scsi.scsiCtlrUnitNumberSpecified = true;
            scsi.sharedBus = VirtualSCSISharing.noSharing;
            scsi.unitNumber = _unit_number;
            scsi.unitNumberSpecified = true;
            scsi.connectable = ci;
            VirtualDeviceConfigSpec dcs = new VirtualDeviceConfigSpec();
            dcs.device = scsi;
            dcs.operation = VirtualDeviceConfigSpecOperation.add;
            dcs.operationSpecified = true;
            return dcs;
        }
        public VirtualDeviceConfigSpec Disk(VMWare oVMWare, string _name, string _datastore, string _size, int _unit_number, int _controller_key, string _suffix)
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

        private void DeleteDirectory(string strFolder)
        {
            DirectoryInfo oDir = new DirectoryInfo(strFolder);
            DirectoryInfo[] oDirs = oDir.GetDirectories();
            System.IO.FileInfo[] oFiles = oDir.GetFiles();
            foreach (System.IO.FileInfo oFile in oFiles)
                File.Delete(strFolder + "\\" + oFile.Name);
            foreach (DirectoryInfo oSubDir in oDirs)
                DeleteDirectory(strFolder + "\\" + oSubDir.Name);
            Directory.Delete(strFolder);
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
                    int intAsset = Int32.Parse(dr["assetid"].ToString());
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
                        oLog.AddEvent(strName, strSerial, "Starting MIS Audits (VMware) for ServerID " + intServer.ToString(), LoggingType.Information);
                        // RUN AUDIT TASKS
                        AuditThread oAuditThread = new AuditThread(intServer, strName, strSerial, intIP, true, strIP, intClass, intEnv, intModel, intOS, intSP, intAddress, boolSAN, boolCluster, boolTSM, 0, intRequest, intServerAuditErrorServiceMIS, intResourceRequestApprove, intAssignPage, intViewPage, strScripts, strSub, strAdminUser, strAdminPass, intEnvironment, 2, dsn, dsnAsset, dsnIP, dsnServiceEditor, true, true, this, true);
                        ThreadStart oAuditThreadStart = new ThreadStart(oAuditThread.Begin);
                        Thread oAuditThreadProcess = new Thread(oAuditThreadStart);
                        oAuditThreadProcess.Start();
                    }
                }
            }
            catch (Exception oError)
            {
                string strError = "Execute MIS Audit Error: " + "(Error Message: " + oError.Message + ") (Source: " + oError.Source + ") (Stack Trace: " + oError.StackTrace + ")";
                strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DEVELOPER_ERROR");
                oFunction.SendEmail("ERROR: Execute MIS Audit Error", strEMailIdsBCC, "", "", "ERROR: Execute MIS Audit Error", "<p><b>An error occurred when attempting to execute an MIS audit...</b></p><p>Error Message:" + strError + "</p>", true, false);
                oEventLog.WriteEntry(String.Format(strErrorName + ": " + strError), EventLogEntryType.Error);
                oLog.AddEvent(strErrorName, "", strError, LoggingType.Error);
            }
        }
        private void SystemError(string _error)
        {
            SystemError(0, 0, _error, 0, 0, null);
        }
        private void SystemError(int _server, int _stepid, string _error, int _assetid, int _modelid, VMWare _vmware)
        {
            try
            {
                Settings oSetting = new Settings(0, dsn);
                oSetting.SystemError(_server, 0, _stepid, _error, _assetid, _modelid, true, _vmware, intEnvironment, dsnAsset);
            }
            catch (Exception exSystemError)
            {
                // If database is offline, it will cause a fatal error AND stop the service.  Stop that from happening by using a catch here.
                oEventLog.WriteEntry("The database is currently offline.  Error message = " + exSystemError.Message, EventLogEntryType.Error);
            }
        }

        /*
        private int ExecuteScheduledTask(int intServer, string strType, string strScript, string strRemoteFile, string strRemoteFileOut, string strExecutable, string strName, string strSerial, string strIP, string strAdminUser, string strAdminPass, int intTimeout)
        {
            //              strType         strRemoteFile               strRemoteFileOut        strExecutable
            // ------------------------------------------------------------------------------------------------------------------------------------
            // IP           IPs             CV_IP.BAT                   CV_IP.TXT               %WinDir%\System32\cmd.exe /c
            // GROUPS       GROUPS          CV_GROUPS.VBS               NULL                    %WinDir%\System32\wscript.exe
            int intReturn = (int)AuditStatus.Running;
            DateTime _now = DateTime.Now;
            string strNow = _now.Day.ToString() + _now.Month.ToString() + _now.Year.ToString() + _now.Hour.ToString() + _now.Minute.ToString() + _now.Second.ToString() + _now.Millisecond.ToString();
            string strFilePrefix = strScripts + strSub + strName + "_" + strType + "_" + strNow;

            // Map drive and copy file
            string strBatch1 = strFilePrefix + "_1.bat";
            StreamWriter oWriter1 = new StreamWriter(strBatch1);
            oWriter1.WriteLine("net use \\\\" + strIP + "\\C$ /user:" + strAdminUser + " " + strAdminPass + "");
            oWriter1.WriteLine("mkdir \\\\" + strIP + "\\C$\\OPTIONS");
            // copy file (VBS) to local computer.  need to create and copy a BAT file to call the VBS.
            oWriter1.WriteLine("copy " + strScript + " \\\\" + strIP + "\\C$\\OPTIONS\\" + strRemoteFile);
            oWriter1.Flush();
            oWriter1.Close();
            string strOut1 = strFilePrefix + "_1.txt";
            ProcessStartInfo info1 = new ProcessStartInfo(strScripts + "psexec");
            info1.WorkingDirectory = strScripts;
            info1.Arguments = "-i cmd.exe /c " + strBatch1 + " > " + strOut1;
            Process proc1 = Process.Start(info1);
            proc1.WaitForExit(300000);   // Wait 5 minutes
            if (proc1.HasExited == false)
                proc1.Kill();
            proc1.Close();
            bool boolOutput1 = ReadOutput(intServer, strType + " (1)", strOut1, strName, strSerial);

            if (boolOutput1 == false)
            {
                // Create and Configure scheduled task on remote machine
                string strBatch2 = strFilePrefix + "_2.bat";
                StreamWriter oWriter2 = new StreamWriter(strBatch2);
                //schtasks /create /s 10.48.54.87 /tn CV_SCRIPT /tr "c:\windows\system32\cmd.exe /c C:\OPTIONS\CV_EXECUTE.BAT > C:\OPTIONS\CV_EXECUTE.TXT" /sc onevent /ec system /np /f /rl highest
                if (strRemoteFileOut == "")
                    oWriter2.WriteLine("schtasks /create /s " + strIP + " /tn " + strType + " /tr \"" + strExecutable + " C:\\OPTIONS\\" + strRemoteFile + "\" /sc onevent /ec system /np /f /rl highest");
                else
                    oWriter2.WriteLine("schtasks /create /s " + strIP + " /tn " + strType + " /tr \"" + strExecutable + " C:\\OPTIONS\\" + strRemoteFile + " > C:\\OPTIONS\\" + strRemoteFileOut + "\" /sc onevent /ec system /np /f /rl highest");
                oWriter2.Flush();
                oWriter2.Close();
                ProcessStartInfo info2 = new ProcessStartInfo(strScripts + "psexec");
                info2.WorkingDirectory = strScripts;
                info2.Arguments = " -i cmd.exe /c " + strBatch2;
                oLog.AddEvent(strName, strSerial, "SCHTASKS # 1 Script (Create) Started", LoggingType.Information);
                Process proc2 = Process.Start(info2);
                proc2.WaitForExit(300000);   // Wait 5 minutes
                if (proc2.HasExited == false)
                    proc2.Kill();
                int intReturnCode = proc2.ExitCode;
                proc2.Close();
                oLog.AddEvent(strName, strSerial, "SCHTASKS # 1 Script (Create) Finished with code (" + intReturnCode.ToString() + ")", LoggingType.Information);

                // Run scheduled task on remote machine
                string strBatch3 = strFilePrefix + "_3.bat";
                StreamWriter oWriter3 = new StreamWriter(strBatch3);
                //schtasks /run /s 10.48.54.87 /tn "CV_SCRIPT"
                oWriter3.WriteLine("schtasks /run /s " + strIP + " /tn \"" + strType + "\"");
                oWriter3.Flush();
                oWriter3.Close();
                ProcessStartInfo info3 = new ProcessStartInfo(strScripts + "psexec");
                info3.WorkingDirectory = strScripts;
                info3.Arguments = " -i cmd.exe /c " + strBatch3;
                oLog.AddEvent(strName, strSerial, "SCHTASKS # 2 Script (Execute) Started", LoggingType.Information);
                Process proc3 = Process.Start(info3);
                proc3.WaitForExit(300000);   // Wait 5 minutes
                if (proc3.HasExited == false)
                    proc3.Kill();
                int intReturnCode2 = proc3.ExitCode;
                proc3.Close();
                oLog.AddEvent(strName, strSerial, "SCHTASKS # 2 Script (Execute) Finished with code (" + intReturnCode2.ToString() + ")", LoggingType.Information);

                // Query scheduled task on remote machine
                bool boolContinue = false;
                int intReturnCode3 = 0;
                string strBatch4 = strFilePrefix + "_4.bat";
                string strOutput4 = strFilePrefix + "_4.txt";
                StreamWriter oWriter4 = new StreamWriter(strBatch4);
                //schtasks /query /s 10.48.54.87 /tn "CV_SCRIPT" > C:\OPTIONS\CV_STATUS.TXT
                oWriter4.WriteLine("schtasks /query /s " + strIP + " /tn \"" + strType + "\" > " + strOutput4);
                oWriter4.Flush();
                oWriter4.Close();
                for (int ii = 0; ii < 10 && boolContinue == false; ii++)
                {
                    ProcessStartInfo info4 = new ProcessStartInfo(strScripts + "psexec");
                    info4.WorkingDirectory = strScripts;
                    info4.Arguments = " -i cmd.exe /c " + strBatch4;
                    oLog.AddEvent(strName, strSerial, "SCHTASKS # 3 Script (Validate) Started", LoggingType.Information);
                    Process proc4 = Process.Start(info4);
                    proc4.WaitForExit(300000);   // Wait 5 minutes
                    if (proc4.HasExited == false)
                        proc4.Kill();
                    intReturnCode3 = proc4.ExitCode;
                    proc4.Close();

                    if (File.Exists(strOutput4) == true)
                    {
                        StreamReader oReader = new StreamReader(strOutput4);
                        string strOutput = oReader.ReadToEnd();
                        oReader.Close();

                        if (strOutput.ToUpper().Contains("READY") == true)
                        {
                            boolContinue = true;
                            break;
                        }
                        else
                        {
                            oLog.AddEvent(strName, strSerial, "SCHTASKS # 3 Script Still Running...", LoggingType.Information);
                        }
                    }
                    else
                        oLog.AddEvent(strName, strSerial, "SCHTASKS # 3 Script Missing Output File (" + strOutput4 + ")...", LoggingType.Information);

                    Thread.Sleep(10000);    //Wait 10 seconds before trying again...
                }

                if (boolContinue == true)
                {
                    oLog.AddEvent(strName, strSerial, "SCHTASKS # 3 Script (Validate) Finished with code (" + intReturnCode3.ToString() + ")", LoggingType.Information);

                    // Delete scheduled task on remote machine
                    string strBatch5 = strFilePrefix + "_5.bat";
                    StreamWriter oWriter5 = new StreamWriter(strBatch5);
                    //schtasks /delete /s 10.48.54.87 /tn CV_SCRIPT /f
                    oWriter5.WriteLine("schtasks /delete /s " + strIP + " /tn " + strType + " /f");
                    oWriter5.Flush();
                    oWriter5.Close();
                    ProcessStartInfo info5 = new ProcessStartInfo(strScripts + "psexec");
                    info5.WorkingDirectory = strScripts;
                    info5.Arguments = " -i cmd.exe /c " + strBatch5;
                    oLog.AddEvent(strName, strSerial, "SCHTASKS # 4 Script (Delete) Started", LoggingType.Information);
                    Process proc5 = Process.Start(info5);
                    proc5.WaitForExit(300000);   // Wait 5 minutes
                    if (proc5.HasExited == false)
                        proc5.Kill();
                    int intReturnCode4 = proc5.ExitCode;
                    proc5.Close();
                    oLog.AddEvent(strName, strSerial, "SCHTASKS # 4 Script (Delete) Finished with code (" + intReturnCode4.ToString() + ")", LoggingType.Information);

                    //net use \\10.48.54.87\C$ /dele
                    string strBatch6 = strFilePrefix + "_6.bat";
                    StreamWriter oWriterIP6 = new StreamWriter(strBatch6);
                    if (boolDeleteScriptFiles == true)
                        oWriterIP6.WriteLine("del " + strFilePrefix + "*.*");
                    oWriterIP6.WriteLine("net use \\\\" + strIP + "\\C$ /dele");
                    oWriterIP6.Flush();
                    oWriterIP6.Close();
                    ProcessStartInfo info6 = new ProcessStartInfo(strScripts + "psexec");
                    info6.WorkingDirectory = strScripts;
                    info6.Arguments = " -i cmd.exe /c " + strBatch6;
                    oLog.AddEvent(strName, strSerial, "SCHTASKS # 5 Script (Cleanup) Started", LoggingType.Information);
                    Process proc6 = Process.Start(info6);
                    proc6.WaitForExit(300000);   // Wait 5 minutes
                    if (proc6.HasExited == false)
                        proc6.Kill();
                    int intReturnCode5 = proc6.ExitCode;
                    proc6.Close();
                    oLog.AddEvent(strName, strSerial, "SCHTASKS # 5 Script (Cleanup) Finished with code (" + intReturnCode5.ToString() + ")", LoggingType.Information);
                    intReturn = (int)AuditStatus.Success;
                }
                else
                {
                    intReturn = (int)AuditStatus.Error;
                }
            }
            else
                intReturn = (int)AuditStatus.NetUseError;
            return intReturn;
        }
        */
        public ManagedObjectReference GetVMs(string _name, Log oLog, VMWare _vmware, ManagedObjectReference[] vmList)
        {
            ManagedObjectReference vmRef = null;
            for (int ii = 0; ii < vmList.Length; ii++)
            {
                if (boolVMFound == true)
                    break;
                if (vmList[ii].type == "VirtualMachine")
                {
                    Object[] vmProps = _vmware.getProperties(vmList[ii], new String[] { "name", "config.name", "config.template" });
                    oLog.AddEvent(_name, "", ((String)vmProps[0]).ToUpper(), LoggingType.Debug);
                    if (((String)vmProps[0]).ToUpper() == _name.ToUpper())
                    {
                        boolVMFound = true;
                        vmRef = vmList[ii];
                        _vmware.UpdateGuestVIM(_name, vmRef.Value);
                        break;
                    }
                }
                else if (vmList[ii].type == "Folder")
                {
                    oLog.AddEvent(_name, "", "FOLDER = " + (ManagedObjectReference[])_vmware.getObjectProperty(vmList[ii], "childEntity"), LoggingType.Debug);
                    vmRef = GetVMs(_name, oLog, _vmware, (ManagedObjectReference[])_vmware.getObjectProperty(vmList[ii], "childEntity"));
                    if (boolVMFound == true)
                        break;
                }
            }
            return vmRef;
        }
    }
}
