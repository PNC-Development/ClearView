using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using NCC.ClearView.Application.Core;
namespace NCC.ClearView.Presentation.Web
{
    public partial class frame_support_order : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        private string dsnServiceEditor = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["ServiceEditorDSN"]].ConnectionString;
        protected int intProfile;
        protected Platforms oPlatform;
        protected Organizations oOrganization;
        protected RequestItems oRequestItem;
        protected Users_At oUserAt;
        protected Costs oCost;
        protected Services oService;
        private DataSet ds;
        protected RequestFields oRequestField;
        protected Reports oReport;
        protected Sites oSites;
        protected Types oType;
        protected Models oModel;
        protected Racks oRacks;
        protected Banks oBanks;
        protected Depot oDepot;
        protected Shelf oShelf;
        protected Classes oClasses;
        protected Rooms oRooms;
        protected Floor oFloor;
        protected Environments oEnvironment;
        protected Forecast oForecast;
        protected Solution oSolution;
        protected Confidence oConfidence;
        protected Locations oLocation;
        protected Field oField;
        protected ServiceDetails oServiceDetail;
        protected DomainController oDomainController;
        protected Domains oDomain;
        protected ServerName oServerName;
        protected OperatingSystems oOperatingSystems;
        protected OnDemand oOnDemand;
        protected ServicePacks oServicePack;
        protected Servers oServer;
        protected Host oHost;
        protected VirtualHDD oVirtualHDD;
        protected VirtualRam oVirtualRam;
        protected Restart oRestart;
        protected Segment oSegment;
        protected ServiceEditor oServiceEditor;
        protected ProjectRequest oProjectRequest;
        protected VMWare oVMWare;
        protected Workstations oWorkstation;
        //protected New oNew;
        protected WhatsNew oWhatsNew;
        protected MaintenanceWindow oMaintenanceWindow;
        //protected RecoveryLocations oRecoveryLocations;
        protected TSM oTSM;
        protected DNS oDNS;
        protected Solaris oSolaris;
        protected Zeus oZeus;
        protected Errors oError;
        protected Design oDesign;
        protected Resiliency oResiliency;
        protected Enhancements oEnhancement;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Cookies["adminid"] != null && Request.Cookies["adminid"].Value != "")
                intProfile = Int32.Parse(Request.Cookies["adminid"].Value);
            else
                Reload();
            oPlatform = new Platforms(intProfile, dsn);
            oOrganization = new Organizations(intProfile, dsn);
            oRequestItem = new RequestItems(intProfile, dsn);
            oUserAt = new Users_At(intProfile, dsn);
            oCost = new Costs(intProfile, dsn);
            oService = new Services(intProfile, dsn);
            oRequestField = new RequestFields(intProfile, dsn);
            oReport = new Reports(intProfile, dsn);
            oSites = new Sites(intProfile, dsn);
            oType = new Types(intProfile, dsn);
            oModel = new Models(intProfile, dsn);
            oRacks = new Racks(intProfile, dsn);
            oBanks = new Banks(intProfile, dsn);
            oDepot = new Depot(intProfile, dsn);
            oShelf = new Shelf(intProfile, dsn);
            oClasses = new Classes(intProfile, dsn);
            oRooms = new Rooms(intProfile, dsn);
            oFloor = new Floor(intProfile, dsn);
            oEnvironment = new Environments(intProfile, dsn);
            oForecast = new Forecast(intProfile, dsn);
            oSolution = new Solution(intProfile, dsn);
            oConfidence = new Confidence(intProfile, dsn);
            oLocation = new Locations(intProfile, dsn);
            oField = new Field(intProfile, dsn);
            oServiceDetail = new ServiceDetails(intProfile, dsn);
            oDomainController = new DomainController(intProfile, dsn);
            oDomain = new Domains(intProfile, dsn);
            oServerName = new ServerName(intProfile, dsn);
            oOperatingSystems = new OperatingSystems(intProfile, dsn);
            oOnDemand = new OnDemand(intProfile, dsn);
            oServicePack = new ServicePacks(intProfile, dsn);
            oServer = new Servers(intProfile, dsn);
            oHost = new Host(intProfile, dsn);
            oVirtualHDD = new VirtualHDD(intProfile, dsn);
            oVirtualRam = new VirtualRam(intProfile, dsn);
            oRestart = new Restart(intProfile, dsn);
            oSegment = new Segment(intProfile, dsn);
            oServiceEditor = new ServiceEditor(intProfile, dsnServiceEditor);
            oProjectRequest = new ProjectRequest(intProfile, dsn);
            oVMWare = new VMWare(intProfile, dsn);
            oWorkstation = new Workstations(intProfile, dsn);
            //oNew = new New(intProfile, dsn);
            oWhatsNew = new WhatsNew(intProfile, dsn);
            oMaintenanceWindow = new MaintenanceWindow(intProfile, dsn);
            //oRecoveryLocations = new RecoveryLocations(intProfile, dsn);
            oTSM = new TSM(intProfile, dsn);
            oDNS = new DNS(intProfile, dsn);
            oSolaris = new Solaris(intProfile, dsn);
            oZeus = new Zeus(intProfile, dsn);
            oError = new Errors(intProfile, dsn);
            oDesign = new Design(intProfile, dsn);
            oResiliency = new Resiliency(intProfile, dsn);
            oEnhancement = new Enhancements(intProfile, dsn);
        
            if (Request.QueryString["type"] != null && Request.QueryString["type"] != "")
                lblType.Text = Request.QueryString["type"];
            if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
                lblId.Text = Request.QueryString["id"];
            string strControl = "";
            if (Request.QueryString["control"] != null)
                strControl = Request.QueryString["control"];
            btnSave.Attributes.Add("onclick", "return Update('hdnUpdateOrder','" + strControl + "');");
            btnClose.Attributes.Add("onclick", "return HidePanel();");
            imgOrderUp.Attributes.Add("onclick", "return MoveOrderUp(" + lstOrder.ClientID + ");");
            imgOrderDown.Attributes.Add("onclick", "return MoveOrderDown(" + lstOrder.ClientID + ");");
            LoadList();
        }
        private void LoadList()
        {
            string strBind = "";
            if (lblType.Text == "PLAT")
            {
                ds = oPlatform.Gets(1);
                lstOrder.DataValueField = "platformid";
            }
            if (lblType.Text == "ORG")
            {
                ds = oOrganization.Gets(1);
                lstOrder.DataValueField = "organizationid";
            }
            if (lblType.Text == "COST")
            {
                ds = oCost.Gets(1);
                lstOrder.DataValueField = "costid";
            }
            if (lblType.Text == "AT")
            {
                ds = oUserAt.Gets(1);
                lstOrder.DataValueField = "atid";
            }
            if (lblType.Text == "ITEMS")
            {
                ds = oRequestItem.GetItems(Int32.Parse(lblId.Text), 0, 1);
                lstOrder.DataValueField = "itemid";
            }
            if (lblType.Text == "SERVICEDETAIL")
            {
                ds = oServiceDetail.Gets(Int32.Parse(lblId.Text), 1);
                lstOrder.DataValueField = "id";
            }
            if (lblType.Text == "SERVICES")
            {
                int intFolder = Int32.Parse(oService.GetFolders(Int32.Parse(lblId.Text), "folderid"));
                ds = oService.Gets(intFolder, 1, 1, 1, 0);
                lstOrder.DataValueField = "id";
            }
            if (lblType.Text == "SERVICE_FOLDERS")
            {
                ds = oService.GetFolders(Int32.Parse(lblId.Text), 1);
                lstOrder.DataValueField = "id";
            }
            if (lblType.Text == "REPORTS")
            {
                ds = oReport.Gets(Int32.Parse(lblId.Text), 1);
                lstOrder.DataValueField = "reportid";
                strBind = "title";
            }
            if (lblType.Text == "A_SITE")
            {
                ds = oSites.Gets(1);
                lstOrder.DataValueField = "id";
            }
            if (lblType.Text == "A_TYPE")
            {
                ds = oType.Gets(Int32.Parse(lblId.Text), 1);
                lstOrder.DataValueField = "id";
            }
            if (lblType.Text == "A_MODEL")
            {
                ds = oModel.Gets(Int32.Parse(lblId.Text), 1);
                lstOrder.DataValueField = "id";
            }
            if (lblType.Text == "A_LOCATION_S")
            {
                ds = oDepot.Gets(1);
                lstOrder.DataValueField = "id";
            }
            if (lblType.Text == "A_SHELF")
            {
                ds = oShelf.Gets(1);
                lstOrder.DataValueField = "id";
            }
            if (lblType.Text == "A_RACK")
            {
                ds = oRacks.Gets(1);
                lstOrder.DataValueField = "id";
            }
            if (lblType.Text == "CLASS")
            {
                ds = oClasses.Gets(1);
                lstOrder.DataValueField = "id";
            }
            if (lblType.Text == "A_ROOM")
            {
                ds = oRooms.Gets(1);
                lstOrder.DataValueField = "id";
            }
            if (lblType.Text == "A_FLOOR")
            {
                ds = oFloor.Gets(1);
                lstOrder.DataValueField = "id";
            }
            if (lblType.Text == "ENVIRONMENT")
            {
                ds = oEnvironment.Gets(1);
                lstOrder.DataValueField = "id";
            }
            if (lblType.Text == "S_CODE")
            {
                ds = oSolution.GetCodes(1);
                lstOrder.DataValueField = "id";
                strBind = "code";
            }
            if (lblType.Text == "F_QUESTION")
            {
                ds = oForecast.GetQuestions(1);
                lstOrder.DataValueField = "id";
            }
            if (lblType.Text == "F_RESPONSE")
            {
                ds = oForecast.GetResponses(Int32.Parse(lblId.Text), 1);
                lstOrder.DataValueField = "id";
            }
            if (lblType.Text == "F_LINE_ITEMS")
            {
                ds = oForecast.GetLineItems(1);
                lstOrder.DataValueField = "id";
            }
            if (lblType.Text == "CONFIDENCE")
            {
                ds = oConfidence.Gets(1);
                lstOrder.DataValueField = "id";
            }
            if (lblType.Text == "LOCATION_S")
            {
                ds = oLocation.GetStates(1);
                lstOrder.DataValueField = "id";
            }
            if (lblType.Text == "F_STEPS")
            {
                ds = oForecast.GetSteps(Int32.Parse(lblId.Text), 1);
                lstOrder.DataValueField = "id";
            }
            if (lblType.Text == "F_STEPS_ADD")
            {
                ds = oForecast.GetStepAdditionals(1);
                lstOrder.DataValueField = "id";
            }
            if (lblType.Text == "FIELD")
            {
                ds = oField.Gets(Int32.Parse(lblId.Text), 1);
                lstOrder.DataValueField = "id";
                strBind = "fieldname";
            }
            if (lblType.Text == "DOMAIN_CONTROLLER")
            {
                ds = oDomainController.Gets(1);
                lstOrder.DataValueField = "id";
            }
            if (lblType.Text == "DOMAIN")
            {
                ds = oDomain.Gets(1);
                lstOrder.DataValueField = "id";
            }
            if (lblType.Text == "SERVERNAME_A")
            {
                ds = oServerName.GetApplications(1);
                lstOrder.DataValueField = "id";
            }
            if (lblType.Text == "SERVERNAME_SUBA")
            {
                ds = oServerName.GetSubApplications(Int32.Parse(lblId.Text), 1);
                lstOrder.DataValueField = "id";
            }
            if (lblType.Text == "SERVERNAME_C")
            {
                ds = oServerName.GetComponents(1);
                lstOrder.DataValueField = "id";
            }
            if (lblType.Text == "WORKSTATION_C")
            {
                ds = oWorkstation.GetComponents(1);
                lstOrder.DataValueField = "id";
            }
            if (lblType.Text == "OPERATING_SYSTEM")
            {
                ds = oOperatingSystems.Gets(1);
                lstOrder.DataValueField = "id";
            }
            if (lblType.Text == "OD_W_STEPS")
            {
                ds = oOnDemand.GetWizardSteps(Int32.Parse(lblId.Text), 1);
                lstOrder.DataValueField = "id";
            }
            if (lblType.Text == "OD_STEPS")
            {
                ds = oOnDemand.GetSteps(Int32.Parse(lblId.Text), 1);
                lstOrder.DataValueField = "id";
            }
            if (lblType.Text == "SERVICE_PACK")
            {
                ds = oServicePack.Gets(1);
                lstOrder.DataValueField = "id";
            }
            if (lblType.Text == "COMPONENT_SCRIPTS")
            {
                ds = oServerName.GetComponentDetailScripts(Int32.Parse(lblId.Text), 1);
                lstOrder.DataValueField = "id";
            }
            if (lblType.Text == "HOST")
            {
                ds = oHost.Gets(1);
                lstOrder.DataValueField = "id";
            }
            if (lblType.Text == "VIRTUAL_HDD")
            {
                ds = oVirtualHDD.Gets(1);
                lstOrder.DataValueField = "id";
            }
            if (lblType.Text == "VIRTUAL_RAM")
            {
                ds = oVirtualRam.Gets(1);
                lstOrder.DataValueField = "id";
            }
            if (lblType.Text == "RESTART")
            {
                ds = oRestart.Gets(1);
                lstOrder.DataValueField = "id";
            }
            if (lblType.Text == "SEGMENT")
            {
                ds = oSegment.Gets(1);
                lstOrder.DataValueField = "id";
            }
            if (lblType.Text == "DOMAIN_SUFFIX")
            {
                ds = oDomain.GetSuffixs(Int32.Parse(lblId.Text), 1);
                lstOrder.DataValueField = "id";
            }
            if (lblType.Text == "DOMAIN_ADMIN_GROUP")
            {
                ds = oDomain.GetAdminGroups(Int32.Parse(lblId.Text), 1);
                lstOrder.DataValueField = "id";
            }
            if (lblType.Text == "SERVICE_EDITOR_FIELDS")
            {
                ds = oServiceEditor.GetFields(1);
                lstOrder.DataValueField = "id";
            }
            if (lblType.Text == "PROJECT_REQUEST_QUESTION")
            {
                ds = oProjectRequest.GetQuestions(1);
                lstOrder.DataValueField = "id";
            }
            if (lblType.Text == "PROJECT_REQUEST_RESPONSE")
            {
                ds = oProjectRequest.GetResponses(Int32.Parse(lblId.Text), 1);
                lstOrder.DataValueField = "id";
            }
            if (lblType.Text == "PROJECT_REQUEST_CLASS")
            {
                ds = oProjectRequest.GetClasses(1);
                lstOrder.DataValueField = "id";
            }
            if (lblType.Text == "VMWARE_TEMPLATE")
            {
                ds = oVMWare.GetTemplates(1);
                lstOrder.DataValueField = "id";
            }
            if (lblType.Text == "ORDER_REPORT_DATASOURCE")
            {
                ds = oReport.GetDataSources(1);
                lstOrder.DataValueField = "id";
            }
            if (lblType.Text == "ORDER_REPORT_CHARTS")
            {
                ds = oReport.GetCharts(1);
                lstOrder.DataValueField = "id";
            }
            if (lblType.Text == "PLATFORM_FORM")
            {
                ds = oPlatform.GetForms(Int32.Parse(lblId.Text), 1);
                lstOrder.DataValueField = "id";
            }
            //if (lblType.Text == "NEW")
            //{
            //    ds = oNew.Gets(1);
            //    lstOrder.DataValueField = "id";
            //    strBind = "title";
            //}
            if (lblType.Text == "WHATSNEW")
            {   ds = oWhatsNew.Gets(1);
                lstOrder.DataValueField = "id";
                strBind = "title";
            }
            //if (lblType.Text == "RECOVERY_LOCATIONS")
            //{
            //    ds = oRecoveryLocations.Gets(1);
            //    lstOrder.DataValueField = "id";
            //}
            if (lblType.Text == "MAINTENANCE_WINDOW")
            {
                ds = oMaintenanceWindow.Gets(1);
                lstOrder.DataValueField = "id";
            }
            if (lblType.Text == "TSM")
            {
                ds = oTSM.Gets(1);
                lstOrder.DataValueField = "id";
            }
            if (lblType.Text == "TSM_DOMAINS")
            {
                ds = oTSM.GetDomains(Int32.Parse(lblId.Text), 1);
                lstOrder.DataValueField = "id";
            }
            if (lblType.Text == "TSM_SCHEDULES")
            {
                ds = oTSM.GetSchedules(Int32.Parse(lblId.Text), 1);
                lstOrder.DataValueField = "id";
            }
            if (lblType.Text == "DNS")
            {
                ds = oDNS.Gets(1);
                lstOrder.DataValueField = "id";
            }
            if (lblType.Text == "SOLARIS_BUILD_NETWORKS")
            {
                ds = oSolaris.GetBuildNetworks(1);
                lstOrder.DataValueField = "id";
            }
            if (lblType.Text == "SOLARIS_BUILD_TYPES")
            {
                ds = oSolaris.GetBuildTypes(1);
                lstOrder.DataValueField = "id";
            }
            if (lblType.Text == "SOLARIS_INTERFACES")
            {
                ds = oSolaris.GetInterfaces(1);
                lstOrder.DataValueField = "id";
            }
            if (lblType.Text == "ZEUS_ARRAY_CONFIGS")
            {
                ds = oZeus.GetArrayConfigs(1);
                lstOrder.DataValueField = "id";
            }
            if (lblType.Text == "ZEUS_BUILD_TYPES")
            {
                ds = oZeus.GetBuildTypes(1);
                lstOrder.DataValueField = "id";
            }
            if (lblType.Text == "ERROR_TYPES")
            {
                ds = oError.GetTypes(1);
                lstOrder.DataValueField = "id";
            }
            if (lblType.Text == "ERROR_TYPES_TYPES")
            {
                ds = oError.GetTypeTypes(Int32.Parse(lblId.Text), 1);
                lstOrder.DataValueField = "id";
            }
            if (lblType.Text == "D_PHASES")
            {
                ds = oDesign.GetPhases(1);
                strBind = "title";
                lstOrder.DataValueField = "id";
            }
            if (lblType.Text == "D_QUESTIONS")
            {
                ds = oDesign.GetQuestions(Int32.Parse(lblId.Text), 1);
                strBind = "summary";
                lstOrder.DataValueField = "id";
            }
            if (lblType.Text == "D_RESPONSES")
            {
                ds = oDesign.GetResponses(Int32.Parse(lblId.Text), 0, 1);
                strBind = "admin";
                lstOrder.DataValueField = "id";
            }
            if (lblType.Text == "D_MODELS")
            {
                ds = oDesign.GetModels(1);
                lstOrder.DataValueField = "id";
            }
            if (lblType.Text == "RESILIENCY")
            {
                ds = oResiliency.Gets(1);
                lstOrder.DataValueField = "id";
            }
            if (lblType.Text == "ENHANCEMENT_MODULES")
            {
                ds = oEnhancement.GetModules(1);
                lstOrder.DataValueField = "id";
            }
            if (lblType.Text == "OPERATING_SYSTEM_GROUPS")
            {
                ds = oOperatingSystems.GetGroups(1);
                lstOrder.DataValueField = "id";
            }
            if (lblType.Text == "DESIGN_APPROVE_CONDITION")
            {
                ds = oDesign.GetApprovalConditionals(1);
                lstOrder.DataValueField = "id";
            }
            

           
            
            if (strBind == "")
                strBind = "name";
            lstOrder.DataTextField = strBind;
            lstOrder.DataSource = ds;
            lstOrder.DataBind();
        }
        private void Reload()
        {
            Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "reload", "<script type=\"text/javascript\">window.top.navigate(window.top.location);<" + "/" + "script>");
        }
    }
}
