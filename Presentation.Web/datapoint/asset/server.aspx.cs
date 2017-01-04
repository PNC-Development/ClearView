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
using NCC.ClearView.Presentation.Web.Custom;
using System.Diagnostics;
using System.DirectoryServices;
using Microsoft.ApplicationBlocks.Data;
using System.Management;
using NCC.ClearView.Application.Core.ClearViewWS;
using System.Threading;
using System.Text;
using Vim25Api;
using System.Collections.Generic;

namespace NCC.ClearView.Presentation.Web
{
    public partial class server : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
        private string dsnZeus = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["ZeusDSN"]].ConnectionString;
        private string dsnServiceEditor = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["ServiceEditorDSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intAssignPage = Int32.Parse(ConfigurationManager.AppSettings["AssignWorkload"]);
        protected int intResourceRequestApprove = Int32.Parse(ConfigurationManager.AppSettings["ResourceRequestApprove"]);
        protected int intViewPage = Int32.Parse(ConfigurationManager.AppSettings["ViewWorkload"]);

        protected int intDataPointAvailableAsset = Int32.Parse(ConfigurationManager.AppSettings["DATAPOINT_AVAILABLE_ASSET"]);
        protected int intDesignBuilder = Int32.Parse(ConfigurationManager.AppSettings["ForecastEdit"]);
        protected int intDesignBuilderOLD = Int32.Parse(ConfigurationManager.AppSettings["ForecastEditOLD"]);
        protected int intStorageService = Int32.Parse(ConfigurationManager.AppSettings["SERVICEID_STORAGE"]);
        protected int intServiceCSM = Int32.Parse(ConfigurationManager.AppSettings["SERVICEID_CSM"]);
        protected int intApplicationCitrix = Int32.Parse(ConfigurationManager.AppSettings["APPLICATIONID_CITRIX"]);
        protected bool boolUseCostCenter = (ConfigurationManager.AppSettings["USE_COST_CENTER"] == "1");
        protected DataPoint oDataPoint;
        protected Users oUser;
        protected Servers oServer;
        protected Asset oAsset;
        protected Forecast oForecast;
        protected Platforms oPlatform;
        protected Types oType;
        protected Models oModel;
        protected ModelsProperties oModelsProperties;
        protected IPAddresses oIPAddresses;
        protected Functions oFunction;
        protected OperatingSystems oOperatingSystem;
        protected ServicePacks oServicePack;
        protected Classes oClass;
        protected Environments oEnvironment;
        protected Mnemonic oMnemonic;
        protected CostCenter oCostCenter;
        protected Variables oVariable;
        protected Locations oLocation;
        protected Storage oStorage;
        protected TSM oTSM;
        protected Requests oRequest;
        protected Projects oProject;
        protected Organizations oOrganization;
        protected Documents oDocument;
        protected Pages oPage;
        protected OnDemandTasks oOnDemandTasks;
        protected ResourceRequest oResourceRequest;
        protected ServerName oServerName;
        protected Zeus oZeus;
        protected Domains oDomain;
        protected Services oService;
        protected ServiceRequests oServiceRequest;
        protected Log oLog;
        protected Audit oAudit;
        protected Solaris oSolaris;
        protected Cluster oCluster;
        protected Design oDesign;
        protected int intProfile = 0;
        protected int intApplication = 0;
        protected string strMenuTab1 = "";
        protected string strMenuTabBackup1 = "";
        protected string strBackupInformation = "";
        protected string strServerStorage = "";
        protected string strServerExecution = "";
        protected string strServerAudit = "";
        protected string strDocuments = "";
        protected string strLinks = "";
        protected string strAdministration = "";
        protected string strAdminsLocal = "";
        protected string strAdminsDomain = "";
        protected string strResponses = "";
        protected string strExecution = "";
        protected string strComponents = "";
        protected int intServer = 0;
        protected int intAnswer = 0;
        protected uint LOCAL_MACHINE = 0x80000002;
        private Variables oVariables;
        private int intID = 0;
        private char[] strIPSplit = { ',' };
        protected string strRebuildStep = "0";
        protected StringBuilder strBackup;


        public enum RegType
        {
            REG_SZ = 1,
            REG_EXPAND_SZ = 2,
            REG_BINARY = 3,
            REG_DWORD = 4,
            REG_MULTI_SZ = 7
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            AuthenticateUser();
            if (Request.Cookies["profileid"] != null && Request.Cookies["profileid"].Value != "")
                intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            if (Request.QueryString["applicationid"] != null && Request.QueryString["applicationid"] != "")
                intApplication = Int32.Parse(Request.QueryString["applicationid"]);
            if (Request.Cookies["application"] != null && Request.Cookies["application"].Value != "")
                intApplication = Int32.Parse(Request.Cookies["application"].Value);
            oDataPoint = new DataPoint(intProfile, dsn);
            oUser = new Users(intProfile, dsn);
            oServer = new Servers(intProfile, dsn);
            oAsset = new Asset(intProfile, dsnAsset);
            oForecast = new Forecast(intProfile, dsn);
            oPlatform = new Platforms(intProfile, dsn);
            oType = new Types(intProfile, dsn);
            oModel = new Models(intProfile, dsn);
            oVariables = new Variables(intEnvironment);
            oModelsProperties = new ModelsProperties(intProfile, dsn);
            oIPAddresses = new IPAddresses(intProfile, dsnIP, dsn);
            oFunction = new Functions(intProfile, dsn, intEnvironment);
            oOperatingSystem = new OperatingSystems(intProfile, dsn);
            oServicePack = new ServicePacks(intProfile, dsn);
            oClass = new Classes(intProfile, dsn);
            oEnvironment = new Environments(intProfile, dsn);
            oMnemonic = new Mnemonic(intProfile, dsn);
            oCostCenter = new CostCenter(intProfile, dsn);
            oVariable = new Variables(intEnvironment);
            oLocation = new Locations(intProfile, dsn);
            oStorage = new Storage(intProfile, dsn);
            oTSM = new TSM(intProfile, dsn);
            oRequest = new Requests(intProfile, dsn);
            oProject = new Projects(intProfile, dsn);
            oOrganization = new Organizations(intProfile, dsn);
            oDocument = new Documents(intProfile, dsn);
            oPage = new Pages(intProfile, dsn);
            oOnDemandTasks = new OnDemandTasks(intProfile, dsn);
            oResourceRequest = new ResourceRequest(intProfile, dsn);
            oServerName = new ServerName(intProfile, dsn);
            oZeus = new Zeus(intProfile, dsnZeus);
            oDomain = new Domains(intProfile, dsn);
            oService = new Services(intProfile, dsn);
            oServiceRequest = new ServiceRequests(intProfile, dsn);
            oLog = new Log(intProfile, dsn);
            oAudit = new Audit(intProfile, dsn, intEnvironment);
            oSolaris = new Solaris(intProfile, dsn);
            oCluster = new Cluster(intProfile, dsn);
            oDesign = new Design(intProfile, dsn);

            if (oUser.IsAdmin(intProfile) == true || (oDataPoint.GetPagePermission(intApplication, "ASSET") == true || intDataPointAvailableAsset == 1))
            {
                panAllow.Visible = true;
                if (Request.QueryString["save"] != null)
                    panSave.Visible = true;
                if (Request.QueryString["error"] != null)
                {
                    panError.Visible = true;
                    switch (Request.QueryString["error"])
                    {
                        case "-1":
                            lblError.Text = "Name Already Exists";
                            break;
                        case "-2":
                            lblError.Text = "Multiple OLD records found";
                            break;
                        case "-3":
                            lblError.Text = "No OLD records found";
                            break;
                    }
                }
                strBackup = new StringBuilder();
                Int32.TryParse(oFunction.decryptQueryString(Request.QueryString["id"]), out intID);
                if (Request.QueryString["close"] != null)
                    Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "close", "<script type=\"text/javascript\">window.close();<" + "/" + "script>");
                else if (Request.QueryString["q"] != null && Request.QueryString["q"] != "")
                {
                    string strQuery = oFunction.decryptQueryString(Request.QueryString["q"]);
                    DataSet ds = oDataPoint.GetAssetName(strQuery, intID, 0, "", "", 0);
                    if (ds.Tables[0].Rows.Count == 1)
                    {
                        string strHeader = (strQuery.Length > 15 ? strQuery.Substring(0, 15) + "..." : strQuery);
                        lblHeader.Text = "&quot;" + strHeader.ToUpper() + "&quot;";
                        Master.Page.Title = "DataPoint | Server (" + strHeader + ")";
                        lblHeaderSub.Text = "Provides all the information about a server...";
                        int intMenuTab = 0;
                        if (Request.QueryString["menu_tab"] != null && Request.QueryString["menu_tab"] != "")
                            intMenuTab = Int32.Parse(Request.QueryString["menu_tab"]);
                        Tab oTab = new Tab(hdnTab.ClientID, intMenuTab, "divMenu1", true, false);
                        oTab.AddTab("Platform Information", "");
                        oTab.AddTab("IP Addresses", "");
                        oTab.AddTab("Software Components", "");
                        oTab.AddTab("Resource Dependencies", "");
                        oTab.AddTab("Application Contacts & Information", "");
                        oTab.AddTab("Design Information", "");
                        oTab.AddTab("Provisioning Status", "");
                        oTab.AddTab("Provisioning Audit Results", "");
                        oTab.AddTab("Storage Information", "");
                        oTab.AddTab("Backup Information", "");
                        oTab.AddTab("Security & Access Information", "");
                        oTab.AddTab("Project Information", "");
                        oTab.AddTab("Documents & Links", "");
                        if (oUser.IsAdmin(intProfile) == true || oDataPoint.GetFieldPermission(intProfile, "SERVER_ADMINISTRATOR") == true)
                        {
                            oTab.AddTab("Administration", "");
                            panAdministration.Visible = true;
                            if (!IsPostBack)
                            {
                                // Load SVE information
                                ddlSVECluster.DataTextField = "name";
                                ddlSVECluster.DataValueField = "id";
                                ddlSVECluster.DataSource = oSolaris.GetSVEClusters(1, 0, -1, 1);
                                ddlSVECluster.DataBind();
                                ddlSVECluster.Items.Insert(0, new ListItem("-- SELECT --", "0"));
                                rptSVE.DataSource = oServer.GetSVE(intID);
                                rptSVE.DataBind();
                                foreach (RepeaterItem ri in rptSVE.Items)
                                {
                                    LinkButton _delete = (LinkButton)ri.FindControl("btnDeleteSVE");
                                    _delete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this exclusion?');");
                                }
                                lblSVE.Visible = (rptSVE.Items.Count == 0);
                            }
                        }
                        //oTab.AddTab("Maintenance & Special Notes", "");
                        //oTab.AddTab("Capacity Performance", "");
                        strMenuTab1 = oTab.GetTabs();

                        btnStorage.Enabled = false;
                        btnApplications.Enabled = false;

                        if (ds.Tables[0].Rows[0]["id"].ToString() != "")
                        {
                            intServer = Int32.Parse(ds.Tables[0].Rows[0]["id"].ToString());
                            if (oServer.Get(intServer, "answerid") != "")
                                intAnswer = Int32.Parse(oServer.Get(intServer, "answerid"));
                        }
                        lblServerID.Text = intServer.ToString();

                        if (!IsPostBack)
                        {
                            rptServiceRequests.DataSource = ds.Tables[1];
                            rptServiceRequests.DataBind();
                            trServiceRequests.Visible = (rptServiceRequests.Items.Count == 0);
                            foreach (RepeaterItem ri in rptServiceRequests.Items)
                            {
                                Label lblServiceID = (Label)ri.FindControl("lblServiceID");
                                int intService = Int32.Parse(lblServiceID.Text);
                                Label lblDetails = (Label)ri.FindControl("lblDetails");
                                Label lblProgress = (Label)ri.FindControl("lblProgress");

                                if (lblProgress.Text == "")
                                    lblProgress.Text = "<i>Unavailable</i>";
                                else
                                {
                                    int intResource = Int32.Parse(lblProgress.Text);
                                    double dblAllocated = 0.00;
                                    double dblUsed = 0.00;
                                    int intStatus = 0;
                                    bool boolAssigned = false;
                                    DataSet dsResource = oDataPoint.GetServiceRequestResource(intResource);
                                    if (dsResource.Tables[0].Rows.Count > 0)
                                        Int32.TryParse(dsResource.Tables[0].Rows[0]["status"].ToString(), out intStatus);
                                    foreach (DataRow drResource in dsResource.Tables[1].Rows)
                                    {
                                        boolAssigned = true;
                                        dblAllocated += double.Parse(drResource["allocated"].ToString());
                                        dblUsed += double.Parse(drResource["used"].ToString());
                                        intStatus = Int32.Parse(drResource["status"].ToString());
                                    }
                                    if (intStatus == (int)ResourceRequestStatus.Closed)
                                        lblProgress.Text = oServiceRequest.GetStatusBar(100.00, "100", "12", true);
                                    else if (intStatus == (int)ResourceRequestStatus.Cancelled)
                                        lblProgress.Text = "Cancelled";
                                    else if (boolAssigned == false)
                                    {
                                        string strManager = "";
                                        DataSet dsManager = oService.GetUser(intService, 1);  // Managers
                                        foreach (DataRow drManager in dsManager.Tables[0].Rows)
                                        {
                                            if (strManager != "")
                                                strManager += "\\n";
                                            int intManager = Int32.Parse(drManager["userid"].ToString());
                                            strManager += " - " + oUser.GetFullName(intManager) + " [" + oUser.GetName(intManager) + "]";
                                        }
                                        lblProgress.Text = "<a href=\"javascript:void(0);\" class=\"lookup\" onclick=\"alert('This request is pending assignment by the following...\\n\\n" + strManager + "');\">Pending Assignment</a>";
                                    }
                                    else if (dblAllocated > 0.00)
                                        lblProgress.Text = oServiceRequest.GetStatusBar((dblUsed / dblAllocated) * 100.00, "100", "12", true);
                                    else
                                        lblProgress.Text = "<i>N / A</i>";
                                    lblDetails.Text = "<a href=\"javascript:void(0);\" class=\"lookup\" onclick=\"OpenNewWindowMenu('/datapoint/service/resource.aspx?id=" + oFunction.encryptQueryString(intResource.ToString()) + "', '800', '600');\">" + lblDetails.Text + "</a>";
                                }
                            }

                            int intProject = 0;
                            int intRequest = 0;
                            int intOrganization = 0;
                            int intModel = 0;
                            if (intServer > 0)
                            {
                                string strName = oServer.GetName(intServer, true);

                                // Administrative Functions
                                if (Request.QueryString["admin"] != null)
                                {
                                    if (Request.QueryString["result"] != null)
                                        strAdministration = "<tr><td>" + oFunction.decryptQueryString(Request.QueryString["result"]) + "</td></tr>";
                                    chkDebug.Checked = (Request.QueryString["debug"] != null);
                                    if (Request.QueryString["output"] != null)
                                    {
                                        DataSet dsOutput = oServer.GetOutput(intServer);
                                        foreach (DataRow drOutput in dsOutput.Tables[0].Rows)
                                        {
                                            strAdministration += "<tr><td><a href=\"javascript:void(0);\" onclick=\"ShowHideDiv2('div" + drOutput["id"].ToString() + "');\">" + drOutput["type"].ToString() + "</a></td></tr>";
                                            strAdministration += "<tr id=\"div" + drOutput["id"].ToString() + "\" style=\"display:none\"><td>" + oFunction.FormatText(drOutput["output"].ToString()) + "</td></tr>";
                                        }
                                        strAdministration += "<tr><td>" + oLog.GetEvents(oLog.GetEventsByName(strName, (chkDebug.Checked ? (int)LoggingType.Debug : (int)LoggingType.Error)), intEnvironment) + "</td></tr>";
                                    }
                                }

                                #region Project Information
                                // PROJECT INFORMATION
                                intRequest = oForecast.GetRequestID(intAnswer, true);
                                intProject = oRequest.GetProjectNumber(intRequest);
                                DataSet dsProject = oProject.Get(intProject);
                                if (dsProject.Tables[0].Rows.Count > 0)
                                {
                                    if (intProject > 0)
                                        txtProjectChange.Text = dsProject.Tables[0].Rows[0]["number"].ToString() + " - " + dsProject.Tables[0].Rows[0]["name"].ToString();
                                    hdnProjectChange.Value = intProject.ToString();
                                    txtProjectChange.Attributes.Add("onkeyup", "return AJAXTextBoxGet(this,'500','195','" + divProjectChange.ClientID + "','" + lstProjectChange.ClientID + "','" + hdnProjectChange.ClientID + "','" + oVariable.URL() + "/frame/ajax/ajax_projects.aspx',2);");
                                    lstProjectChange.Attributes.Add("ondblclick", "AJAXClickRow();");
                                    lblProjectName.Text = dsProject.Tables[0].Rows[0]["name"].ToString();
                                    lblProjectNumber.Text = "<a href=\"javascript:void(0);\">" + dsProject.Tables[0].Rows[0]["number"].ToString() + "</a>"; ;
                                    intOrganization = Int32.Parse(dsProject.Tables[0].Rows[0]["organization"].ToString());
                                    lblProjectPortfolio.Text = oOrganization.GetName(intOrganization);
                                    if (dsProject.Tables[0].Rows[0]["lead"].ToString() != "")
                                        lblProjectLead.Text = oUser.GetFullName(Int32.Parse(dsProject.Tables[0].Rows[0]["lead"].ToString())) + " [" + oUser.GetName(Int32.Parse(dsProject.Tables[0].Rows[0]["lead"].ToString())) + "]";
                                    else
                                        lblProjectLead.Text = "N / A";
                                    if (dsProject.Tables[0].Rows[0]["engineer"].ToString() != "")
                                        lblProjectEngineer.Text = oUser.GetFullName(Int32.Parse(dsProject.Tables[0].Rows[0]["engineer"].ToString())) + " [" + oUser.GetName(Int32.Parse(dsProject.Tables[0].Rows[0]["engineer"].ToString())) + "]";
                                    else
                                        lblProjectEngineer.Text = "N / A";
                                }
                                #endregion

                                int intCluster = 0;
                                int intCSM = 0;
                                int intNumber = 0;
                                int intVMwareCluster = 0;
                                #region General Information
                                // Load General Information
                                int intAsset = 0;
                                int intType = 0;
                                int intAssetClass = 0;
                                int intAssetEnv = 0;
                                DataSet dsAssets = oServer.GetAssets(intServer);
                                foreach (DataRow drAsset in dsAssets.Tables[0].Rows)
                                {
                                    if (drAsset["latest"].ToString() == "1")
                                    {
                                        intAsset = Int32.Parse(drAsset["assetid"].ToString());
                                        intAssetClass = Int32.Parse(drAsset["classid"].ToString());
                                        intAssetEnv = Int32.Parse(drAsset["environmentid"].ToString());
                                        break;
                                    }
                                }
                                //oDataPoint.LoadTextBoxCheckBox(txtName, chkName, intProfile, null, "", lblName, fldName, "SERVER_NAME", strName, "", false, true);
                                oDataPoint.LoadTextBox(txtName, intProfile, null, "", lblName, fldName, "SERVER_NAME", strName, "", true, false);
                                if (intAsset > 0)
                                {
                                    // Asset Information
                                    string strSerial = oAsset.Get(intAsset, "serial");
                                    string strURL = oDataPoint.GetAssetSerialOrTag(strSerial, "", "url");
                                    oDataPoint.LoadTextBox(txtPlatformSerial, intProfile, btnPlatformSerial, "/datapoint/asset/" + strURL + ".aspx?t=serial&q=" + oFunction.encryptQueryString(strSerial) + "&id=" + oFunction.encryptQueryString(intAsset.ToString()), lblPlatformSerial, fldPlatformSerial, "SERVER_SERIAL", strSerial, "", true, false);
                                    string strAsset = oAsset.Get(intAsset, "asset");
                                    oDataPoint.LoadTextBox(txtPlatformAsset, intProfile, null, "", lblPlatformAsset, fldPlatformAsset, "SERVER_ASSET", strAsset, "", true, false);
                                    intModel = Int32.Parse(oAsset.Get(intAsset, "modelid"));
                                    int intModelParent = Int32.Parse(oModelsProperties.Get(intModel, "modelid"));
                                    intType = oModel.GetType(intModelParent);
                                    int intPlatform = oType.GetPlatform(intType);
                                    string strPath = oType.Get(intType, "ondemand_steps_path");
                                    //if (strPath != "")
                                    //    frmProvisioning.Attributes["src"] = strPath + "?id=" + oFunction.encryptQueryString(intServer.ToString());
                                }
                                if (oModelsProperties.IsTypeVMware(intModel) == false)
                                {
                                    chkStepVMWare.Enabled = false;
                                    strRebuildStep = "6";
                                }
                                else
                                    strRebuildStep = "7";

                                // Server Information
                                int intClass = 0;
                                int intEnv = 0;
                                DataSet dsAnswer = oForecast.GetAnswer(intAnswer);
                                if (dsAnswer.Tables[0].Rows.Count > 0)
                                {
                                    intClass = Int32.Parse(dsAnswer.Tables[0].Rows[0]["classid"].ToString());
                                    intEnv = Int32.Parse(dsAnswer.Tables[0].Rows[0]["environmentid"].ToString());
                                }

                                int intDomain = 0;
                                int intDomainEnvironment = 0;
                                DataSet dsServer = oServer.Get(intServer);
                                if (dsServer.Tables[0].Rows.Count > 0)
                                {
                                    txtStep.Text = dsServer.Tables[0].Rows[0]["step"].ToString();
                                    txtStepSkipStart.Text = dsServer.Tables[0].Rows[0]["step_skip_start"].ToString();
                                    txtStepSkipGoto.Text = dsServer.Tables[0].Rows[0]["step_skip_goto"].ToString();
                                    if (dsServer.Tables[0].Rows[0]["rebuilding"].ToString() == "1")
                                    {
                                        chkRebuild.Checked = true;
                                        chkRebuild.Enabled = false;
                                        chkRedo.Enabled = false;
                                        btnStep.Enabled = false;
                                    }
                                    chkRedo.Attributes.Add("onclick", "RedoStep(" + txtStep.Text + ",this,'" + txtStep.ClientID + "','" + txtStepSkipStart.ClientID + "','" + txtStepSkipGoto.ClientID + "','" + chkRebuild.ClientID + "');");
                                    chkRebuild.Attributes.Add("onclick", "RebuildStep(" + txtStep.Text + ",this,'" + txtStep.ClientID + "','" + txtStepSkipStart.ClientID + "','" + txtStepSkipGoto.ClientID + "','" + chkRedo.ClientID + "');");
                                    intRequest = Int32.Parse(dsServer.Tables[0].Rows[0]["requestid"].ToString());
                                    intCluster = Int32.Parse(dsServer.Tables[0].Rows[0]["clusterid"].ToString());
                                    Int32.TryParse(dsServer.Tables[0].Rows[0]["vmware_clusterid"].ToString(), out intVMwareCluster);
                                    intCSM = Int32.Parse(dsServer.Tables[0].Rows[0]["csmconfigid"].ToString());
                                    intNumber = Int32.Parse(dsServer.Tables[0].Rows[0]["number"].ToString());
                                    intDomain = Int32.Parse(dsServer.Tables[0].Rows[0]["domainid"].ToString());
                                    int intOS = Int32.Parse(dsServer.Tables[0].Rows[0]["osid"].ToString());
                                    int intSP = Int32.Parse(dsServer.Tables[0].Rows[0]["spid"].ToString());
                                    if (panAdministration.Visible == true)
                                    {

                                        //ddlSVECluster.SelectedValue = dsServer.Tables[0].Rows[0]["sve_clusterid"].ToString();
                                    }
                                    btnStorage.Enabled = (oOperatingSystem.IsWindows(intOS) == true || oOperatingSystem.IsWindows2008(intOS) == true);
                                    btnApplications.Enabled = (oOperatingSystem.IsWindows(intOS) == true || oOperatingSystem.IsWindows2008(intOS) == true);
                                    if (oOperatingSystem.IsSolaris(intOS) == false)
                                        chkStepBoot.Enabled = false;
                                    strServerStorage += GetStorage(intAnswer, intCluster, intCSM, intNumber, intModel, strName) + GetStorageShared(intAnswer, intModel);
                                    strServerExecution = oServer.GetExecution(intServer, intEnvironment, dsnAsset, dsnIP, dsnServiceEditor, intAssignPage, intViewPage);
                                    int intMenuTabRequest = 0;
                                    DataSet dsRequests = oServer.GetRequests(intRequest, 1);
                                    foreach (DataRow drRequest in dsRequests.Tables[0].Rows)
                                    {
                                        intMenuTabRequest++;
                                        if (drRequest["id"].ToString() == intServer.ToString())
                                            break;
                                    }
                                    if (intMenuTabRequest == 0)
                                        intMenuTabRequest = 1;
                                    btnExecution.Enabled = true;
                                    //btnExecution.Attributes.Add("onclick", "return OpenNewWindowMenu('/frame/ondemand/status.aspx?rid=" + intRequest.ToString() + "&menu_tab=" + intMenuTabRequest.ToString() + "', '800', '600');");
                                    btnExecution.Attributes.Add("onclick", "return OpenNewWindowMenu('/frame/ondemand/status.aspx?rid=" + intRequest.ToString() + "', '800', '600');");

                                    // Audit
                                    string strAuditError = "";
                                    DataSet dsAudit = oAudit.GetServers(intServer);
                                    if (dsAudit.Tables[0].Rows.Count > 0)
                                    {
                                        foreach (DataRow drAudit in dsAudit.Tables[0].Rows)
                                        {
                                            strServerAudit += "<tr>";
                                            int intAuditStatus = 0;
                                            Int32.TryParse(drAudit["status"].ToString(), out intAuditStatus);
                                            AuditStatus oAuditStatus = (AuditStatus)intAuditStatus;
                                            strServerAudit += "<td class=\"header\"><img src=\"" + oAudit.GetImage(oAuditStatus) + "\" border=\"0\" align=\"absmiddle\"/> " + (drAudit["scriptid"].ToString() == "0" ? "*** REBOOT ***" : "<a href=\"javascript:void(0);\" onclick=\"ShowHideDiv2('trAudit" + drAudit["scriptid"].ToString() + "');\">" + drAudit["name"].ToString()) + "</a></td>";
                                            strServerAudit += "<td align=\"right\">&nbsp;&nbsp;&nbsp;[Completed on " + drAudit["completed"].ToString() + "]</td>";
                                            strServerAudit += "</tr>";
                                            if (drAudit["scriptid"].ToString() != "0")
                                            {
                                                strServerAudit += "<tr id=\"trAudit" + drAudit["scriptid"].ToString() + "\" style=\"display:none\">";
                                                int intAudit = Int32.Parse(drAudit["auditid"].ToString());
                                                strServerAudit += "<td colspan=\"2\">";
                                                if (strAuditError == "")
                                                {
                                                    try
                                                    {
                                                        DataSet dsAuditResult = oAudit.GetServerDetailsRemote(intAudit);
                                                        if (dsAudit.Tables[0].Rows.Count > 0)
                                                        {
                                                            string strAuditResult = "";
                                                            foreach (DataRow drAuditResult in dsAuditResult.Tables[0].Rows)
                                                            {
                                                                strAuditResult += "<tr>";
                                                                try
                                                                {
                                                                    int intAuditResultStatus = Int32.Parse(drAuditResult["status"].ToString());
                                                                    AuditStatus oAuditResultStatus = (AuditStatus)intAuditResultStatus;
                                                                    strAuditResult += "<td valign=\"top\" nowrap>" + oAuditResultStatus.ToString() + "</td>";
                                                                }
                                                                catch
                                                                {
                                                                    strAuditResult += "<td valign=\"top\" nowrap>Unknown Status (" + drAuditResult["status"].ToString() + ")</td>";
                                                                }
                                                                strAuditResult += "<td valign=\"top\" nowrap>" + drAuditResult["code"].ToString() + "</td>";
                                                                strAuditResult += "<td valign=\"top\" width=\"100%\">" + drAuditResult["result"].ToString() + "</td>";
                                                                strAuditResult += "<td valign=\"top\" nowrap>" + drAuditResult["created"].ToString() + "</td>";
                                                                strAuditResult += "</tr>";
                                                            }
                                                            strServerAudit += "<table cellpadding=\"7\" cellspacing=\"0\" border=\"0\">" + strAuditResult + "</table>";
                                                        }
                                                        else
                                                            strServerAudit += "There is no information for this audit";
                                                    }
                                                    catch (Exception exAudit)
                                                    {
                                                        strAuditError = "There was a problem locating the data for this audit ~ " + exAudit.Message;
                                                        strServerAudit += strAuditError;
                                                    }
                                                }
                                                else
                                                    strServerAudit += strAuditError;
                                                strServerAudit += "</td>";
                                                strServerAudit += "</tr>";
                                            }
                                        }
                                        strServerAudit = "<table cellpadding=\"7\" cellspacing=\"0\" border=\"0\">" + strServerAudit + "</table>";
                                    }
                                    else
                                        strServerAudit = "<img src=\"/images/bigAlert.gif\" border=\"0\" align=\"absmiddle\" /> No automated audit tasks were executed for this device";
                                    oDataPoint.LoadDropDown(ddlPlatformOS, intProfile, null, "", lblPlatformOS, fldPlatformOS, "SERVER_OS", "name", "id", oOperatingSystem.Gets(0, 1), intOS, false, false, true);
                                    panClusterWindows2012.Visible = (panAdministration.Visible && ddlPlatformOS.SelectedItem.Text.Contains("2012"));
                                    oDataPoint.LoadDropDown(ddlPlatformServicePack, intProfile, null, "", lblPlatformServicePack, fldPlatformServicePack, "SERVER_SP", "name", "id", oOperatingSystem.GetServicePack(intOS), Int32.Parse(dsServer.Tables[0].Rows[0]["spid"].ToString()), false, false, true);
                                    int intRole = 0;
                                    Int32.TryParse(dsServer.Tables[0].Rows[0]["applicationid"].ToString(), out intRole);
                                    oDataPoint.LoadDropDown(ddlPlatformDeviceRole, intProfile, null, "", lblPlatformDeviceRole, fldPlatformDeviceRole, "SERVER_ROLE", "name", "id", oServerName.GetApplicationsForecast(1), intRole, false, false, false);
                                    oDataPoint.LoadPanel(panComponents, intProfile, fldComponents, "SERVER_COMPONENTS");
                                    if (panComponents.Visible == true)
                                        frmComponents.Attributes.Add("src", "/frame/ondemand/config_server_components.aspx?id=" + intServer.ToString());
                                    else
                                    {
                                        DataSet dsSelected = oServerName.GetComponentDetailSelected(intServer, 1);
                                        foreach (DataRow drSelected in dsSelected.Tables[0].Rows)
                                        {
                                            int intDetail = Int32.Parse(drSelected["detailid"].ToString());
                                            strComponents += oServerName.GetComponentDetailName(intDetail) + "<br/>";
                                        }
                                    }
                                    DateTime datStarted = DateTime.Today;
                                    string strStarted = "";
                                    if (DateTime.TryParse(dsServer.Tables[0].Rows[0]["build_started"].ToString(), out datStarted) == true)
                                        strStarted = datStarted.ToShortDateString();
                                    oDataPoint.LoadTextBoxDate(txtPlatformBuildStarted, imgPlatformBuildStarted, intProfile, null, "", lblPlatformBuildStarted, fldPlatformBuildStarted, "SERVER_STARTED", strStarted, "", false, false);
                                    DateTime datCompleted = DateTime.Today;
                                    string strCompleted = "";
                                    if (DateTime.TryParse(dsServer.Tables[0].Rows[0]["build_completed"].ToString(), out datCompleted) == true)
                                        strCompleted = datCompleted.ToShortDateString();
                                    oDataPoint.LoadTextBoxDate(txtPlatformBuildCompleted, imgPlatformBuildCompleted, intProfile, null, "", lblPlatformBuildCompleted, fldPlatformBuildCompleted, "SERVER_COMPLETED", strCompleted, "", false, false);
                                    DateTime datReady = DateTime.Today;
                                    string strReady = "";
                                    if (DateTime.TryParse(dsServer.Tables[0].Rows[0]["build_ready"].ToString(), out datReady) == true)
                                        strReady = datReady.ToShortDateString();
                                    oDataPoint.LoadTextBoxDate(txtPlatformBuildReady, imgPlatformBuildReady, intProfile, null, "", lblPlatformBuildReady, fldPlatformBuildReady, "SERVER_READY", strReady, "", false, false);
                                    DateTime datRebuild = DateTime.Today;
                                    string strRebuild = "";
                                    if (DateTime.TryParse(dsServer.Tables[0].Rows[0]["rebuild"].ToString(), out datRebuild) == true)
                                        strRebuild = datRebuild.ToShortDateString();
                                    oDataPoint.LoadTextBoxDate(txtPlatformRebuild, imgPlatformRebuild, intProfile, null, "", lblPlatformRebuild, fldPlatformRebuild, "SERVER_REBUILD", strRebuild, "", false, false);
                                    DateTime datDecommissioned = DateTime.Today;
                                    string strDecommissioned = "";
                                    if (DateTime.TryParse(dsServer.Tables[0].Rows[0]["decommissioned"].ToString(), out datDecommissioned) == true)
                                        strDecommissioned = datDecommissioned.ToShortDateString();
                                    oDataPoint.LoadTextBoxDate(txtPlatformDecommissioned, imgPlatformDecommissioned, intProfile, null, "", lblPlatformDecommissioned, fldPlatformDecommissioned, "SERVER_DECOMMISSIONED", strDecommissioned, "", false, false);
                                    // DR Counterpart
                                    bool boolDR = false;
                                    string strDR = "";
                                    if (dsServer.Tables[0].Rows[0]["dr"].ToString() == "1")
                                    {
                                        boolDR = true;
                                        if (dsServer.Tables[0].Rows[0]["dr_exist"].ToString() == "1")
                                            strDR = dsServer.Tables[0].Rows[0]["dr_name"].ToString();
                                        if (strDR == "")
                                            strDR = strName + "-DR";
                                    }
                                    if (boolDR == false) 
                                    {
                                        // Search for DR asset to make sure it doesn't exist (by NAME-DR)
                                        strDR = strName + "-DR";
                                        DataSet dsDR = oDataPoint.GetAssetName(strDR, 0, 0, "", "", 0);
                                        if (dsDR.Tables[0].Rows.Count == 1)
                                            boolDR = true;
                                    }
                                    if (boolDR == true) 
                                        oDataPoint.LoadTextBox(txtPlatformDRCounterPart, intProfile, btnPlatformDRCounterPart, "/datapoint/asset/datapoint_asset_search.aspx?t=name&q=" + oFunction.encryptQueryString(strDR), lblPlatformDRCounterPart, fldPlatformDRCounterPart, "SERVER_DR", strDR, "", true, false);
                                    else
                                        oDataPoint.LoadTextBox(txtPlatformDRCounterPart, intProfile, null, "", lblPlatformDRCounterPart, fldPlatformDRCounterPart, "SERVER_DR", "", "", true, false);
                                    // HA Counterpart
                                    string strHA = "";
                                    DataSet dsHA = oServer.GetHA(intServer);
                                    if (dsServer.Tables[0].Rows[0]["ha"].ToString() == "1")
                                    {
                                        lblPlatformHACounterPart.Text = "HA Counterpart:";
                                        dsHA = oServer.GetHAs(intServer);
                                        if (dsHA.Tables[0].Rows.Count > 0)
                                            strHA = oServer.GetName(Int32.Parse(dsHA.Tables[0].Rows[0]["serverid_ha"].ToString()), true);
                                    }
                                    if (dsServer.Tables[0].Rows[0]["ha"].ToString() == "10")
                                    {
                                        lblPlatformHACounterPart.Text = "Original HA Counterpart:";
                                        dsHA = oServer.GetHAs(intServer);
                                        if (dsHA.Tables[0].Rows.Count > 0)
                                            strHA = oServer.GetName(Int32.Parse(dsHA.Tables[0].Rows[0]["serverid"].ToString()), true);
                                    }
                                    if (strHA != "")
                                        oDataPoint.LoadTextBox(txtPlatformHACounterPart, intProfile, btnPlatformHACounterPart, "/datapoint/asset/datapoint_asset_search.aspx?t=name&q=" + oFunction.encryptQueryString(strHA), lblPlatformHACounterPart, fldPlatformHACounterPart, "SERVER_HA", strHA, "", true, false);
                                    else
                                        oDataPoint.LoadTextBox(txtPlatformHACounterPart, intProfile, null, "", lblPlatformHACounterPart, fldPlatformHACounterPart, "SERVER_HA", "N / A", "", true, false);
                                    oDataPoint.LoadTextBox(txtPlatformCluster, intProfile, btnPlatformCluster, "/datapoint/asset/cluster.aspx?id=" + oFunction.encryptQueryString(intCluster.ToString()), lblPlatformCluster, fldPlatformCluster, "SERVER_CLUSTER", (intCluster > 0 ? (oCluster.Get(intCluster, "name") != "" ? oCluster.Get(intCluster, "name") : oCluster.Get(intCluster, "nickname")) : "N / A"), "", true, false);

                                    if (dsServer.Tables[0].Rows[0]["tsm_output"].ToString() != "")
                                    {
                                        panTSMOutput.Visible = true;
                                        lblTSMOutput.Text = dsServer.Tables[0].Rows[0]["tsm_output"].ToString();
                                    }
                                    lblTSMRegister.Text = dsServer.Tables[0].Rows[0]["tsm_register"].ToString();
                                    lblTSMDefine.Text = dsServer.Tables[0].Rows[0]["tsm_define"].ToString();
                                }
                                if (intDomain > 0)
                                    intDomainEnvironment = Int32.Parse(oDomain.Get(intDomain, "environment"));
                                hdnEnvironment.Value = intAssetEnv.ToString();
                                oDataPoint.LoadDropDown(ddlPlatformClass, intProfile, null, "", lblPlatformClass, fldPlatformClass, "SERVER_CLASS", "name", "id", oClass.Gets(1), intAssetClass, false, false, true);
                                oDataPoint.LoadDropDown(ddlPlatformEnvironment, intProfile, null, "", lblPlatformEnvironment, fldPlatformEnvironment, "SERVER_ENVIRONMENT", "name", "id", oClass.GetEnvironment(intAssetClass, 0), intAssetEnv, false, false, true);
                                oDataPoint.LoadDropDown(ddlPlatformDomain, intProfile, null, "", lblPlatformDomain, fldPlatformDomain, "SERVER_DOMAIN", "name", "id", oDomain.GetClassEnvironment(intAssetClass, intAssetEnv), intDomain, false, false, true);
                                #endregion

                                // IP Address Information
                                oDataPoint.LoadPanel(panIPs, intProfile, fldIPs, "SERVER_IPS");
                                //DataSet dsIPVMotion = oServer.GetIP(intServer, 0, 0, 1, 0);
                                trVMotion.Visible = trVMotionView.Visible = (intVMwareCluster > 0);
                                trPrivate.Visible = trPrivateView.Visible = (intVMwareCluster == 0);
                                //panIPs.Visible = false;
                                if (panIPs.Visible == true)
                                {
                                    // DHCP
                                    LoadIP(oServer.Get(intServer, "dhcp"), txtIPDHCP1, txtIPDHCP2, txtIPDHCP3, txtIPDHCP4);
                                    // Others (NON-DHCP)
                                    DataSet dsIP = oServer.GetIP(intServer, 0, 0, 0, 0);
                                    foreach (DataRow drIP in dsIP.Tables[0].Rows)
                                    {
                                        if (drIP["avamar"].ToString() == "1")
                                            LoadIP(drIP["ipaddressid"].ToString(), lstIPAvamar, hdnIPAvamar, hdnIPAvamarExists);
                                        if (drIP["vmotion"].ToString() == "1")
                                        {
                                            if (trPrivate.Visible)
                                                LoadIP(drIP["ipaddressid"].ToString(), lstIPPrivate, hdnIPPrivate, hdnIPPrivateExists);
                                            if (trVMotion.Visible)
                                                LoadIP(drIP["ipaddressid"].ToString(), lstIPVMotion, hdnIPVMotion, hdnIPVMotionExists);
                                        }
                                        if (drIP["final"].ToString() == "1")
                                            LoadIP(drIP["ipaddressid"].ToString(), lstIPFinal, hdnIPFinal, hdnIPFinalExists);
                                        if (drIP["auto_assign"].ToString() == "1")
                                            LoadIP(drIP["ipaddressid"].ToString(), lstIPAssigned, hdnIPAssigned, hdnIPAssignedExists);
                                    }
                                    btnIPAssignedAdd.Attributes.Add("onclick", "return ValidateIP('" + txtIPAssigned1.ClientID + "','" + txtIPAssigned2.ClientID + "','" + txtIPAssigned3.ClientID + "','" + txtIPAssigned4.ClientID + "','Please enter a valid IP address') && ListControlInIP('" + lstIPAssigned.ClientID + "','" + hdnIPAssigned.ClientID + "','" + txtIPAssigned1.ClientID + "','" + txtIPAssigned2.ClientID + "','" + txtIPAssigned3.ClientID + "','" + txtIPAssigned4.ClientID + "');");
                                    btnIPAssignedRemove.Attributes.Add("onclick", "return ListControlOut('" + lstIPAssigned.ClientID + "','" + hdnIPAssigned.ClientID + "');");
                                    btnIPFinalAdd.Attributes.Add("onclick", "return ValidateIP('" + txtIPFinal1.ClientID + "','" + txtIPFinal2.ClientID + "','" + txtIPFinal3.ClientID + "','" + txtIPFinal4.ClientID + "','Please enter a valid IP address') && ListControlInIP('" + lstIPFinal.ClientID + "','" + hdnIPFinal.ClientID + "','" + txtIPFinal1.ClientID + "','" + txtIPFinal2.ClientID + "','" + txtIPFinal3.ClientID + "','" + txtIPFinal4.ClientID + "');");
                                    btnIPFinalRemove.Attributes.Add("onclick", "return ListControlOut('" + lstIPFinal.ClientID + "','" + hdnIPFinal.ClientID + "');");
                                    btnIPPrivateAdd.Attributes.Add("onclick", "return ValidateIP('" + txtIPPrivate1.ClientID + "','" + txtIPPrivate2.ClientID + "','" + txtIPPrivate3.ClientID + "','" + txtIPPrivate4.ClientID + "','Please enter a valid IP address') && ListControlInIP('" + lstIPPrivate.ClientID + "','" + hdnIPPrivate.ClientID + "','" + txtIPPrivate1.ClientID + "','" + txtIPPrivate2.ClientID + "','" + txtIPPrivate3.ClientID + "','" + txtIPPrivate4.ClientID + "');");
                                    btnIPPrivateRemove.Attributes.Add("onclick", "return ListControlOut('" + lstIPPrivate.ClientID + "','" + hdnIPPrivate.ClientID + "');");
                                    btnIPVMotionAdd.Attributes.Add("onclick", "return ValidateIP('" + txtIPVMotion1.ClientID + "','" + txtIPVMotion2.ClientID + "','" + txtIPVMotion3.ClientID + "','" + txtIPVMotion4.ClientID + "','Please enter a valid IP address') && ListControlInIP('" + lstIPVMotion.ClientID + "','" + hdnIPVMotion.ClientID + "','" + txtIPVMotion1.ClientID + "','" + txtIPVMotion2.ClientID + "','" + txtIPVMotion3.ClientID + "','" + txtIPVMotion4.ClientID + "');");
                                    btnIPVMotionRemove.Attributes.Add("onclick", "return ListControlOut('" + lstIPVMotion.ClientID + "','" + hdnIPVMotion.ClientID + "');");
                                    btnIPAvamarAdd.Attributes.Add("onclick", "return ValidateIP('" + txtIPAvamar1.ClientID + "','" + txtIPAvamar2.ClientID + "','" + txtIPAvamar3.ClientID + "','" + txtIPAvamar4.ClientID + "','Please enter a valid IP address') && ListControlInIP('" + lstIPAvamar.ClientID + "','" + hdnIPAvamar.ClientID + "','" + txtIPAvamar1.ClientID + "','" + txtIPAvamar2.ClientID + "','" + txtIPAvamar3.ClientID + "','" + txtIPAvamar4.ClientID + "');");
                                    btnIPAvamarRemove.Attributes.Add("onclick", "return ListControlOut('" + lstIPAvamar.ClientID + "','" + hdnIPAvamar.ClientID + "');");
                                }
                                else
                                {
                                    panIPsNo.Visible = true;
                                    lblIPDHCP.Text = oServer.Get(intServer, "dhcp");
                                    lblIPAssigned.Text = oServer.GetIPs(intServer, 1, 0, 0, 0, dsnIP, ", ", "---", true);
                                    lblIPFinal.Text = oServer.GetIPs(intServer, 0, 1, 0, 0, dsnIP, ", ", "---", true);
                                    lblIPPrivate.Text = oServer.GetIPs(intServer, 0, 0, 1, 0, dsnIP, ", ", "---", true);
                                    lblIPVMotion.Text = oServer.GetIPs(intServer, 0, 0, 1, 0, dsnIP, ", ", "---", true);
                                    lblIPAvamar.Text = oServer.GetIPs(intServer, 0, 0, 0, 1, dsnIP, ", ", "---", true);
                                }

                                int intDesign = 0;
                                int intMnemonic = 0;
                                int intCostCenter = 0;
                                if (intAnswer > 0)
                                {
                                    DataSet dsDesign = oDesign.GetAnswer(intAnswer);
                                    if (dsDesign.Tables[0].Rows.Count > 0)
                                        Int32.TryParse(dsDesign.Tables[0].Rows[0]["id"].ToString(), out intDesign);

                                    #region Application Information
                                    // Load Application Information
                                    string strPNC = "";
                                    string strEngineer = "";
                                    if (dsAnswer.Tables[0].Rows.Count > 0)
                                    {
                                        oDataPoint.LoadTextBox(txtApplicationName, intProfile, null, "", lblApplicationName, fldApplicationName, "SERVER_APPNAME", dsAnswer.Tables[0].Rows[0]["appname"].ToString(), "", false, true);
                                        if (oClass.IsProd(intClass))
                                            panApplicationDR.Visible = true;
                                        if (oClass.Get(intClass, "pnc") == "1")
                                        {
                                            panApplicationMnemonic.Visible = true;
                                            strPNC = " && ValidateHidden0('" + hdnApplicationMnemonic.ClientID + "','" + txtApplicationMnemonic.ClientID + "','Please enter the mnemonic of this design\\n\\n(Start typing and a list will be presented...)')";
                                            intMnemonic = Int32.Parse(dsAnswer.Tables[0].Rows[0]["mnemonicid"].ToString());
                                            oDataPoint.LoadDropDownAJAX(txtApplicationMnemonic, hdnApplicationMnemonic, divApplicationMnemonic, lstApplicationMnemonic, intEnvironment, intProfile, null, "", lblApplicationMnemonic, fldApplicationMnemonic, "SERVER_MNEMONIC", intMnemonic, (intMnemonic > 0 ? oMnemonic.Get(intMnemonic, "factory_code") + " - " + oMnemonic.Get(intMnemonic, "name") : ""), "/frame/ajax/ajax_mnemonics.aspx", "", false, true);

                                            if (boolUseCostCenter == true)
                                            {
                                                panApplicationCostCenter.Visible = true;
                                                strPNC = " && ValidateHidden0('" + hdnApplicationCostCenter.ClientID + "','" + txtApplicationCostCenter.ClientID + "','Please enter the cost center of this design\\n\\n(Start typing and a list will be presented...)')";
                                                Int32.TryParse(dsAnswer.Tables[0].Rows[0]["costcenterid"].ToString(), out intCostCenter);
                                                oDataPoint.LoadDropDownAJAX(txtApplicationCostCenter, hdnApplicationCostCenter, divApplicationCostCenter, lstApplicationCostCenter, intEnvironment, intProfile, null, "", lblApplicationCostCenter, fldApplicationCostCenter, "SERVER_COST_CENTER", intCostCenter, (intCostCenter > 0 ? oCostCenter.GetName(intCostCenter) : ""), "/frame/ajax/ajax_cost_centers.aspx", 5, "", false, false);
                                            }
                                        }
                                        else
                                        {
                                            panApplicationCode.Visible = true;
                                            strPNC = " && ValidateText('" + txtApplicationCode.ClientID + "','Please enter an application code')";
                                            oDataPoint.LoadTextBox(txtApplicationCode, intProfile, null, "", lblApplicationCode, fldApplicationCode, "SERVER_APPCODE", dsAnswer.Tables[0].Rows[0]["appcode"].ToString(), "", false, true);
                                        }
                                        int intApplicationDR = 0;
                                        if (dsAnswer.Tables[0].Rows[0]["dr_criticality"].ToString() != "")
                                            intApplicationDR = Int32.Parse(dsAnswer.Tables[0].Rows[0]["dr_criticality"].ToString());
                                        oDataPoint.LoadDropDown(ddlApplicationDR, intProfile, null, "", lblApplicationDR, fldApplicationDR, "SERVER_DR_CRITICAL", "name", "id", SqlHelper.ExecuteDataset(dsn, CommandType.Text, "SELECT 1 AS id, '1 - High' AS name UNION ALL SELECT 2 AS id, '2 - Low' AS name"), intApplicationDR, false, false, true);
                                        int intApplicationClient = 0;
                                        if (dsAnswer.Tables[0].Rows[0]["appcontact"].ToString() != "")
                                            intApplicationClient = Int32.Parse(dsAnswer.Tables[0].Rows[0]["appcontact"].ToString());
                                        oDataPoint.LoadDropDownAJAX(txtApplicationClient, hdnApplicationClient, divApplicationClient, lstApplicationClient, intEnvironment, intProfile, null, "", lblApplicationClient, fldApplicationClient, "SERVER_CLIENT", intApplicationClient, (intApplicationClient > 0 ? oUser.GetFullName(intApplicationClient) + " (" + oUser.GetName(intApplicationClient) + ")" : ""), "/frame/users.aspx", "", false, true);
                                        int intApplicationPrimary = 0;
                                        if (dsAnswer.Tables[0].Rows[0]["admin1"].ToString() != "")
                                            intApplicationPrimary = Int32.Parse(dsAnswer.Tables[0].Rows[0]["admin1"].ToString());
                                        oDataPoint.LoadDropDownAJAX(txtApplicationPrimary, hdnApplicationPrimary, divApplicationPrimary, lstApplicationPrimary, intEnvironment, intProfile, null, "", lblApplicationPrimary, fldApplicationPrimary, "SERVER_PRIMARY", intApplicationPrimary, (intApplicationPrimary > 0 ? oUser.GetFullName(intApplicationPrimary) + " (" + oUser.GetName(intApplicationPrimary) + ")" : ""), "/frame/users.aspx", "", false, true);
                                        int intApplicationAdministrative = 0;
                                        if (dsAnswer.Tables[0].Rows[0]["admin2"].ToString() != "")
                                            intApplicationAdministrative = Int32.Parse(dsAnswer.Tables[0].Rows[0]["admin2"].ToString());
                                        oDataPoint.LoadDropDownAJAX(txtApplicationAdministrative, hdnApplicationAdministrative, divApplicationAdministrative, lstApplicationAdministrative, intEnvironment, intProfile, null, "", lblApplicationAdministrative, fldApplicationAdministrative, "SERVER_ADMIN", intApplicationAdministrative, (intApplicationAdministrative > 0 ? oUser.GetFullName(intApplicationAdministrative) + " (" + oUser.GetName(intApplicationAdministrative) + ")" : ""), "/frame/users.aspx", "", false, false);
                                        if (oClass.Get(intClass, "pnc") == "1")
                                        {
                                            panApplicationOwner.Visible = true;
                                            int intApplicationOwner = 0;
                                            if (dsAnswer.Tables[0].Rows[0]["appowner"].ToString() != "")
                                                intApplicationOwner = Int32.Parse(dsAnswer.Tables[0].Rows[0]["appowner"].ToString());
                                            oDataPoint.LoadDropDownAJAX(txtApplicationOwner, hdnApplicationOwner, divApplicationOwner, lstApplicationOwner, intEnvironment, intProfile, null, "", lblApplicationOwner, fldApplicationOwner, "SERVER_OWNER", intApplicationOwner, (intApplicationOwner > 0 ? oUser.GetFullName(intApplicationOwner) + " (" + oUser.GetName(intApplicationOwner) + ")" : ""), "/frame/users.aspx", "", false, true);
                                        }
                                        if (oForecast.IsHACSM(intAnswer) == true)
                                        {
                                            panApplicationEngineer.Visible = true;
                                            int intApplicationEngineer = 0;
                                            if (dsAnswer.Tables[0].Rows[0]["networkengineer"].ToString() != "")
                                                intApplicationEngineer = Int32.Parse(dsAnswer.Tables[0].Rows[0]["networkengineer"].ToString());
                                            oDataPoint.LoadDropDownAJAX(txtApplicationEngineer, hdnApplicationEngineer, divApplicationEngineer, lstApplicationEngineer, intEnvironment, intProfile, null, "", lblApplicationEngineer, fldApplicationEngineer, "SERVER_ENGINEER", intApplicationEngineer, (intApplicationEngineer > 0 ? oUser.GetFullName(intApplicationEngineer) + " (" + oUser.GetName(intApplicationEngineer) + ")" : ""), "/frame/users.aspx", "", false, true);
                                            strEngineer = " && ValidateHidden0('" + hdnApplicationEngineer.ClientID + "','" + txtApplicationEngineer.ClientID + "','Please enter the LAN ID of your network engineer')";
                                        }

                                        // Load Design Information
                                        #region Design Information
                                        oDataPoint.LoadTextBox(txtDesignID, intProfile, btnDesignID, "/datapoint/service/design.aspx?t=design&q=" + oFunction.encryptQueryString(intAnswer.ToString()), lblDesignID, fldDesignID, "DESIGN_ID", (intDesign > 0 ? intDesign.ToString() : intAnswer.ToString()), "", false, false);
                                        oDataPoint.LoadTextBox(txtDesignChange, intProfile, null, "", lblDesignChange, fldDesignChange, "DESIGN_CHANGE", dsAnswer.Tables[0].Rows[0]["change"].ToString(), "", false, false);
                                        oDataPoint.LoadTextBoxDate(txtDesignCommitment, imgDesignCommitment, intProfile, null, "", lblDesignCommitment, fldDesignCommitment, "DESIGN_COMMITMENT", dsAnswer.Tables[0].Rows[0]["implementation"].ToString(), "", false, true);
                                        oDataPoint.LoadTextBoxDate(txtDesignCompletion, imgDesignCompletion, intProfile, null, "", lblDesignCompletion, fldDesignCompletion, "DESIGN_COMPLETED", dsAnswer.Tables[0].Rows[0]["completed"].ToString(), "", false, false);
                                        oDataPoint.LoadTextBoxDate(txtDesignProduction, imgDesignProduction, intProfile, null, "", lblDesignProduction, fldDesignProduction, "DESIGN_PRODUCTION", dsAnswer.Tables[0].Rows[0]["production"].ToString(), "", false, false);
                                        oDataPoint.LoadTextBoxDate(txtDesignFinished, imgDesignFinished, intProfile, null, "", lblDesignFinished, fldDesignFinished, "DESIGN_FINISHED", dsAnswer.Tables[0].Rows[0]["finished"].ToString(), "", false, false);

                                        int intPlatform = Int32.Parse(oForecast.GetAnswer(intAnswer, "platformid"));
                                        DataSet dsQuestions = oForecast.GetQuestionPlatform(intPlatform, intClass, intEnv);
                                        foreach (DataRow drQuestion in dsQuestions.Tables[0].Rows)
                                        {
                                            string strResponsePDF = "";
                                            int intQuestion = Int32.Parse(drQuestion["id"].ToString());
                                            DataSet dsAnswers = oForecast.GetAnswerPlatform(intAnswer, intQuestion);
                                            foreach (DataRow drAnswer in dsAnswers.Tables[0].Rows)
                                            {
                                                if (strResponsePDF != "")
                                                    strResponsePDF += ", ";
                                                strResponsePDF += oForecast.GetResponse(Int32.Parse(drAnswer["responseid"].ToString()), "response");
                                            }
                                            if (strResponsePDF != "")
                                            {
                                                strResponses += "<tr><td colspan=\"3\" width=\"100%\">" + drQuestion["question"].ToString() + "</td></tr>";
                                                strResponses += "<tr><td><img src=\"/images/spacer.gif\" border=\"0\" width=\"25\" height=\"1\"/></td><td><img src=\"/images/comment.gif\" border=\"0\" align=\"absmiddle\"/></td><td width=\"100%\">" + strResponsePDF + "</td></tr>";
                                            }
                                        }
                                        #endregion

                                    }
                                    #endregion


                                    #region Backup Information
                                    // BACKUP INFORMATION
                                    DataSet dsBackup = oForecast.GetBackup(intAnswer);
                                    if (dsBackup.Tables[0].Rows.Count > 0)
                                    {
                                        int intMenuBackupTab = 0;
                                        if (Request.QueryString["menu_tab_backup"] != null && Request.QueryString["menu_tab_backup"] != "")
                                            intMenuBackupTab = Int32.Parse(Request.QueryString["menu_tab_backup"]);
                                        Tab oTabBackup = new Tab(hdnTabBackup.ClientID, intMenuBackupTab, "divMenuBackup1", true, false);
                                        oTabBackup.AddTab("Backup Information", "");
                                        oTabBackup.AddTab("Backup Inclusions", "");
                                        oTabBackup.AddTab("Backup Exclusions", "");
                                        oTabBackup.AddTab("Archive Requirements", "");
                                        oTabBackup.AddTab("Additional Configuration", "");
                                        oTabBackup.AddTab("Backup Registration Script", "");
                                        strMenuTabBackup1 = oTabBackup.GetTabs();

                                        //if (dsBackup.Tables[0].Rows[0]["recoveryid"].ToString() != "")
                                        //    strBackupInformation += "<tr><td nowrap>Recovery Location:</td><td width=\"100%\">" + oLocation.GetFull(Int32.Parse(dsBackup.Tables[0].Rows[0]["recoveryid"].ToString())) + "</td></tr>";
                                        if (dsBackup.Tables[0].Rows[0]["daily"].ToString() == "1")
                                            strBackupInformation += "<tr><td nowrap>Timing/Frequency of Backups:</td><td width=\"100%\">" + "Daily" + "</td></tr>";
                                        else if (dsBackup.Tables[0].Rows[0]["weekly"].ToString() == "1")
                                            strBackupInformation += "<tr><td nowrap>Timing/Frequency of Backups:</td><td width=\"100%\">" + "Weekly" + "</td></tr>";
                                        else if (dsBackup.Tables[0].Rows[0]["monthly"].ToString() == "1")
                                            strBackupInformation += "<tr><td nowrap>Timing/Frequency of Backups:</td><td width=\"100%\">" + "Monthly" + "</td></tr>";
                                        if (dsBackup.Tables[0].Rows[0]["time"].ToString() == "1")
                                            strBackupInformation += "<tr><td nowrap>Start Time:</td><td width=\"100%\">" + dsBackup.Tables[0].Rows[0]["time_hour"].ToString() + " " + dsBackup.Tables[0].Rows[0]["time_switch"].ToString() + "</td></tr>";
                                        else
                                            strBackupInformation += "<tr><td nowrap>Start Time:</td><td width=\"100%\">" + "Don't Care" + "</td></tr>";

                                        double dblHighU = 0.00;
                                        double dblStandardU = 0.00;
                                        double dblLowU = 0.00;
                                        double dblHighQAU = 0.00;
                                        double dblStandardQAU = 0.00;
                                        double dblLowQAU = 0.00;
                                        double dblHighTestU = 0.00;
                                        double dblStandardTestU = 0.00;
                                        double dblLowTestU = 0.00;
                                        DataSet dsStorage = oStorage.GetLuns(intAnswer, 0, intCluster, intCSM, intNumber);
                                        foreach (DataRow drStorage in dsStorage.Tables[0].Rows)
                                        {
                                            if (drStorage["size"].ToString() != "")
                                            {
                                                if (drStorage["performance"].ToString() == "High")
                                                    dblHighU += double.Parse(drStorage["size"].ToString());
                                                if (drStorage["performance"].ToString() == "Standard")
                                                    dblStandardU += double.Parse(drStorage["size"].ToString());
                                                if (drStorage["performance"].ToString() == "Low")
                                                    dblLowU += double.Parse(drStorage["size"].ToString());
                                            }
                                            if (drStorage["size_qa"].ToString() != "")
                                            {
                                                if (drStorage["performance"].ToString() == "High")
                                                    dblHighQAU += double.Parse(drStorage["size_qa"].ToString());
                                                if (drStorage["performance"].ToString() == "Standard")
                                                    dblStandardQAU += double.Parse(drStorage["size_qa"].ToString());
                                                if (drStorage["performance"].ToString() == "Low")
                                                    dblLowQAU += double.Parse(drStorage["size_qa"].ToString());
                                            }
                                            if (drStorage["size_test"].ToString() != "")
                                            {
                                                if (drStorage["performance"].ToString() == "High")
                                                    dblHighTestU += double.Parse(drStorage["size_test"].ToString());
                                                if (drStorage["performance"].ToString() == "Standard")
                                                    dblStandardTestU += double.Parse(drStorage["size_test"].ToString());
                                                if (drStorage["performance"].ToString() == "Low")
                                                    dblLowTestU += double.Parse(drStorage["size_test"].ToString());
                                            }
                                            DataSet dsMount = oStorage.GetMountPoints(Int32.Parse(drStorage["id"].ToString()));
                                            foreach (DataRow drMount in dsMount.Tables[0].Rows)
                                            {
                                                if (drMount["size"].ToString() != "")
                                                {
                                                    if (drMount["performance"].ToString() == "High")
                                                        dblHighU += double.Parse(drMount["size"].ToString());
                                                    if (drMount["performance"].ToString() == "Standard")
                                                        dblStandardU += double.Parse(drMount["size"].ToString());
                                                    if (drMount["performance"].ToString() == "Low")
                                                        dblLowU += double.Parse(drMount["size"].ToString());
                                                }
                                                if (drMount["size_qa"].ToString() != "")
                                                {
                                                    if (drMount["performance"].ToString() == "High")
                                                        dblHighQAU += double.Parse(drMount["size_qa"].ToString());
                                                    if (drMount["performance"].ToString() == "Standard")
                                                        dblStandardQAU += double.Parse(drMount["size_qa"].ToString());
                                                    if (drMount["performance"].ToString() == "Low")
                                                        dblLowQAU += double.Parse(drMount["size_qa"].ToString());
                                                }
                                                if (drMount["size_test"].ToString() != "")
                                                {
                                                    if (drMount["performance"].ToString() == "High")
                                                        dblHighTestU += double.Parse(drMount["size_test"].ToString());
                                                    if (drMount["performance"].ToString() == "Standard")
                                                        dblStandardTestU += double.Parse(drMount["size_test"].ToString());
                                                    if (drMount["performance"].ToString() == "Low")
                                                        dblLowTestU += double.Parse(drMount["size_test"].ToString());
                                                }
                                            }
                                        }
                                        double dblTotal = dblHighU + dblStandardU + dblLowU + dblHighQAU + dblStandardQAU + dblLowQAU + dblHighTestU + dblStandardTestU + dblLowTestU;
                                        strBackupInformation += "<tr><td nowrap>Total Combined Disk Capacity (GB):</td><td width=\"100%\">" + dblTotal.ToString("0") + " GB" + "</td></tr>";
                                        strBackupInformation += "<tr><td nowrap>Current Combined Disk Utilized (GB):</td><td width=\"100%\">" + "5 GB" + "</td></tr>";
                                        strBackupInformation += "<tr><td nowrap>Average Size of One Typical Data File:</td><td width=\"100%\">" + dsBackup.Tables[0].Rows[0]["average_one"].ToString() + " GB" + "</td></tr>";
                                        if (dsBackup.Tables[0].Rows[0]["documentation"].ToString() == "")
                                            strBackupInformation += "<tr><td nowrap>Production Turnover Documentation:</td><td width=\"100%\">" + "Not Specified" + "</td></tr>";
                                        else
                                            strBackupInformation += "<tr><td nowrap>Production Turnover Documentation:</td><td width=\"100%\">" + dsBackup.Tables[0].Rows[0]["documentation"].ToString() + "</td></tr>";

                                        rptInclusions.DataSource = oForecast.GetBackupInclusions(intAnswer);
                                        rptInclusions.DataBind();
                                        lblNoneInclusions.Visible = rptInclusions.Items.Count == 0;

                                        rptExclusions.DataSource = oForecast.GetBackupExclusions(intAnswer);
                                        rptExclusions.DataBind();
                                        lblNoneExclusions.Visible = rptExclusions.Items.Count == 0;

                                        rptRetention.DataSource = oForecast.GetBackupRetentions(intAnswer);
                                        rptRetention.DataBind();
                                        lblNoneRetention.Visible = rptRetention.Items.Count == 0;

                                        lblAverage.Text = dsBackup.Tables[0].Rows[0]["average_one"] != DBNull.Value ? dsBackup.Tables[0].Rows[0]["average_one"].ToString() : "0";
                                        lblDocumentation.Text = dsBackup.Tables[0].Rows[0]["documentation"] != DBNull.Value ? dsBackup.Tables[0].Rows[0]["documentation"].ToString() : "NA";
                                        lblCFPercent.Text = dsBackup.Tables[0].Rows[0]["cf_percent"] != DBNull.Value ? dsBackup.Tables[0].Rows[0]["cf_percent"].ToString() + "%" : "";
                                        lblCFCompression.Text = dsBackup.Tables[0].Rows[0]["cf_compression"] != DBNull.Value ? dsBackup.Tables[0].Rows[0]["cf_compression"].ToString() + "%" : "";
                                        lblCFAverage.Text = dsBackup.Tables[0].Rows[0]["cf_average"] != DBNull.Value ? dsBackup.Tables[0].Rows[0]["cf_average"].ToString() : "";
                                        lblCFBackup.Text = dsBackup.Tables[0].Rows[0]["cf_backup"] != DBNull.Value ? dsBackup.Tables[0].Rows[0]["cf_backup"].ToString() : "";
                                        lblCFArchive.Text = dsBackup.Tables[0].Rows[0]["cf_archive"] != DBNull.Value ? dsBackup.Tables[0].Rows[0]["cf_archive"].ToString() : "";
                                        lblCFWindow.Text = dsBackup.Tables[0].Rows[0]["cf_window"] != DBNull.Value ? dsBackup.Tables[0].Rows[0]["cf_window"].ToString() + " (Hours)" : "";
                                        lblCFSets.Text = dsBackup.Tables[0].Rows[0]["cf_sets"] != DBNull.Value ? dsBackup.Tables[0].Rows[0]["cf_sets"].ToString() : "";
                                        lblCDType.Text = dsBackup.Tables[0].Rows[0]["cd_type"] != DBNull.Value ? dsBackup.Tables[0].Rows[0]["cd_type"].ToString() : "";
                                        lblCDPercent.Text = dsBackup.Tables[0].Rows[0]["cd_percent"] != DBNull.Value ? dsBackup.Tables[0].Rows[0]["cd_percent"].ToString() + "%" : "";
                                        lblCDCompression.Text = dsBackup.Tables[0].Rows[0]["cd_compression"] != DBNull.Value ? dsBackup.Tables[0].Rows[0]["cd_compression"].ToString() : "";
                                        lblCDVersions.Text = dsBackup.Tables[0].Rows[0]["cd_versions"] != DBNull.Value ? dsBackup.Tables[0].Rows[0]["cd_versions"].ToString() : "";
                                        lblCDWindow.Text = dsBackup.Tables[0].Rows[0]["cd_window"] != DBNull.Value ? dsBackup.Tables[0].Rows[0]["cd_window"].ToString() : "";
                                        lblCDGrowth.Text = dsBackup.Tables[0].Rows[0]["cd_growth"] != DBNull.Value ? dsBackup.Tables[0].Rows[0]["cd_growth"].ToString() : "";

                                    }
                                    else
                                    {
                                        if (intDesign > 0)
                                        {
                                            panCFI.Visible = true;
                                            string strFrequency = oDesign.Get(intDesign, "backup_frequency");
                                            lblFrequency.Text = (strFrequency == "D" ? "Daily" : (strFrequency == "W" ? "Weekly" : (strFrequency == "M" ? "Monthly" : "Backup Not Requested")));
                                            dsBackup = oDesign.GetBackup(intDesign);
                                            if (dsBackup.Tables[0].Rows.Count > 0)
                                            {
                                                DataRow drBackup = dsBackup.Tables[0].Rows[0];
                                                for (int ii = 0; ii < 7; ii++)
                                                {
                                                    strBackup.Append("<tr>");
                                                    strBackup.Append("<td>");
                                                    string strCheck = "";
                                                    if (ii == 0)
                                                    {
                                                        strBackup.Append("Sunday");
                                                        strCheck = drBackup["sun"].ToString();
                                                    }
                                                    else if (ii == 1)
                                                    {
                                                        strBackup.Append("Monday");
                                                        strCheck = drBackup["mon"].ToString();
                                                    }
                                                    else if (ii == 2)
                                                    {
                                                        strBackup.Append("Tuesday");
                                                        strCheck = drBackup["tue"].ToString();
                                                    }
                                                    else if (ii == 3)
                                                    {
                                                        strBackup.Append("Wednesday");
                                                        strCheck = drBackup["wed"].ToString();
                                                    }
                                                    else if (ii == 4)
                                                    {
                                                        strBackup.Append("Thursday");
                                                        strCheck = drBackup["thu"].ToString();
                                                    }
                                                    else if (ii == 5)
                                                    {
                                                        strBackup.Append("Friday");
                                                        strCheck = drBackup["fri"].ToString();
                                                    }
                                                    else
                                                    {
                                                        strBackup.Append("Saturday");
                                                        strCheck = drBackup["sat"].ToString();
                                                    }
                                                    strBackup.Append("</td>");
                                                    for (int jj = 0; jj < 24; jj++)
                                                    {
                                                        strBackup.Append("<td>");
                                                        if (strCheck[jj] == '1')
                                                            strBackup.Append("<b>B</b>");
                                                        else
                                                            strBackup.Append("-");
                                                        strBackup.Append("</td>");
                                                    }
                                                    strBackup.Append("</tr>");
                                                }
                                            }

                                            rptExclusionsCFI.DataSource = oDesign.GetExclusions(intDesign);
                                            rptExclusionsCFI.DataBind();
                                            if (rptExclusionsCFI.Items.Count == 0)
                                                lblExclusion.Visible = true;
                                        }
                                    }
                                    string strBackupPreview = "";
                                    if (dsServer.Tables[0].Rows.Count > 0)
                                    {
                                        string strResult = dsServer.Tables[0].Rows[0]["tsm_output"].ToString();
                                        int intSchedule = Int32.Parse(dsServer.Tables[0].Rows[0]["tsm_schedule"].ToString());
                                        if (intSchedule > 0)
                                        {
                                            strBackupPreview += "<tr>";
                                            strBackupPreview += "<td>" + strName + "</td>";
                                            int intTSMDomain = Int32.Parse(oTSM.GetSchedule(intSchedule, "domain"));
                                            string strDomain = oTSM.GetDomain(intTSMDomain, "name");
                                            int intTSMServer = Int32.Parse(oTSM.GetDomain(intTSMDomain, "tsm"));
                                            strBackupPreview += "<td>" + oTSM.Get(intTSMServer, "name") + "</td>";
                                            strBackupPreview += "<td>" + oTSM.Get(intTSMServer, "port") + "</td>";
                                            strBackupPreview += "<td>" + oTSM.GetSchedule(intSchedule, "name") + "</td>";
                                            strBackupPreview += "</tr>";
                                            // Preview
                                            lblBackupPreview.Text = "<p>Your servers have been successfully registered to a backup server. Here are the details...</p>";
                                            lblBackupPreview.Text += "<p><table cellpadding=\"2\" cellspacing=\"1\" border=\"0\" style=\"" + oVariable.DefaultFontStyle() + "\"><tr><td><b>Server Name:</b></td><td><b>TCPSERVERADDRESS:</b></td><td><b>TCPPort:</b></td><td><b>Schedule:</b></td>" + strBackupPreview + "</table></p>";
                                            lblBackupPreview.Text += "<p>Please update the backup client software and recycle the backup services to pick up the next scheduled backup time (shown above).</p>";
                                            lblBackupPreview.Text += "<p>Also check that the dsm.opt files have a valid include/exclude list to ensure the machine is only backing up what is required.</p>";
                                            panBackupTSM.Visible = true;
                                        }
                                        else
                                        {
                                            //DataSet dsAvamar = oServer.getav
                                        }
                                    }
                                    #endregion
                                }



                                #region Security
                                // Security
                                rptAccounts.DataSource = oServer.GetAccounts(intServer);
                                rptAccounts.DataBind();
                                foreach (RepeaterItem ri in rptAccounts.Items)
                                {
                                    Label _local = (Label)ri.FindControl("lblLocal");
                                    Label _domain = (Label)ri.FindControl("lblDomain");
                                    string strPermissions = "";
                                    Label _permissions = (Label)ri.FindControl("lblPermissions");
                                    if (oClass.Get(intClass, "pnc") == "1")
                                    {
                                        char[] strAccountSplit = { ';' };

                                        // Domain Groups
                                        string[] strAccountDomainArray = _domain.Text.Split(strAccountSplit);
                                        for (int jj = 0; jj < strAccountDomainArray.Length; jj++)
                                        {
                                            string strAccountDomain = strAccountDomainArray[jj].Trim();
                                            if (strAccountDomain.Contains("_") == true)
                                            {
                                                strPermissions += strAccountDomain.Substring(0, strAccountDomain.IndexOf("_"));
                                                strAccountDomain = strAccountDomain.Substring(strAccountDomain.IndexOf("_") + 1);
                                                if (strAccountDomain == "1")
                                                    strPermissions += " (Remote Desktop)";
                                            }
                                            else
                                                strPermissions += strAccountDomain;
                                            strPermissions += "<br/>";
                                        }

                                        // Local Groups
                                        string[] strAccountLocalArray = _local.Text.Split(strAccountSplit);
                                        for (int jj = 0; jj < strAccountLocalArray.Length; jj++)
                                        {
                                            string strAccountLocal = strAccountLocalArray[jj].Trim();
                                            if (strAccountLocal.Contains("_") == true)
                                            {
                                                strPermissions += strAccountLocal.Substring(0, strAccountLocal.IndexOf("_"));
                                                strAccountLocal = strAccountLocal.Substring(strAccountLocal.IndexOf("_") + 1);
                                                if (strAccountLocal == "1")
                                                    strPermissions += " (Remote Desktop)";
                                            }
                                            else
                                                strPermissions += strAccountLocal;
                                            strPermissions += "<br/>";
                                        }
                                    }
                                    else
                                    {
                                        Label _admin = (Label)ri.FindControl("lblAdmin");
                                        string strPermission = _admin.Text;
                                        if (strPermission == "1")
                                            strPermissions += "ADMINISTRATOR<br/>";
                                        strPermission = _local.Text;
                                        if (strPermission.Contains("GLCfsaRO_SysVol"))
                                            strPermissions += "SYS_VOL (C:) - Read Only<br/>";
                                        else if (strPermission.Contains("GLCfsaRW_SysVol"))
                                            strPermissions += "SYS_VOL (C:) - Read / Write<br/>";
                                        else if (strPermission.Contains("GLCfsaFC_SysVol"))
                                            strPermissions += "SYS_VOL (C:) - Full Control<br/>";
                                        if (strPermission.Contains("GLCfsaRO_UtlVol"))
                                            strPermissions += "UTL_VOL (E:) - Read Only<br/>";
                                        else if (strPermission.Contains("GLCfsaRW_UtlVol"))
                                            strPermissions += "UTL_VOL (E:) - Read / Write<br/>";
                                        else if (strPermission.Contains("GLCfsaFC_UtlVol"))
                                            strPermissions += "UTL_VOL (E:) - Full Control<br/>";
                                        if (strPermission.Contains("GLCfsaRO_AppVol"))
                                            strPermissions += "APP_VOL (F:) - Read Only<br/>";
                                        else if (strPermission.Contains("GLCfsaRW_AppVol"))
                                            strPermissions += "APP_VOL (F:) - Read / Write<br/>";
                                        else if (strPermission.Contains("GLCfsaFC_AppVol"))
                                            strPermissions += "APP_VOL (F:) - Full Control<br/>";
                                    }
                                    if (strPermissions == "")
                                        strPermissions = "-----";
                                    _permissions.Text = strPermissions;
                                }
                                if (rptAccounts.Items.Count == 0)
                                {
                                    lblAccounts.Visible = true;
                                } 
                                if (Request.Cookies["security"] != null && Request.Cookies["security"].Value != "0")
                                {
                                    Response.Cookies["security"].Value = "0";
                                    btnSecurity.Enabled = false;

                                    string strAdminsDomainGroup = "GSGu_" + strName + "Adm";
                                    //strAdminsDomainGroup = "GSGu_OHCLEIIS4569Adm";
                                    try
                                    {
                                        if (oClass.Get(intClass, "pnc") == "1")
                                        {
                                            if (intOrganization > 0 && intMnemonic > 0)
                                            {
                                                string strOrganizationCode = oOrganization.Get(intOrganization, "code");
                                                string strMnemonicCode = oMnemonic.Get(intMnemonic, "factory_code").ToUpper();
                                                string[] strGroups = oVariable.PNC_AD_Groups();
                                                for (int ii = 0; ii < strGroups.Length; ii++)
                                                {
                                                    //string strNameGG = "GG_URA_" + strOrganizationCode + "_" + strMnemonicCode + "_" + strGroups[ii];
                                                    string strNameGG = "GSGu_" + strMnemonicCode + "_" + strGroups[ii];
                                                    strAdminsDomain += "<tr><td><b>" + strNameGG + "</b></td></tr>";
                                                    strAdminsDomain += CheckADGroup(intDomain, intDomainEnvironment, strNameGG, false);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            strAdminsDomain += "<tr><td><b>" + strAdminsDomainGroup + "</b></td></tr>";
                                            strAdminsDomain += CheckADGroup(intDomain, intDomainEnvironment, strAdminsDomainGroup, true);
                                        }
                                        if (strAdminsDomain == "")
                                            strAdminsDomain += "<tr><td><img src=\"/images/alert.gif\" border=\"0\" align=\"absmiddle\"/> There are no domain administrators in the &quot;" + oDomain.Get(intDomain, "name") + "&quot; domain</td></tr>";
                                        strAdminsDomain = "<table cellpadding=\"2\" cellspacing=\"1\" border=\"0\">" + strAdminsDomain + "</table>";

                                        Variables oVar = new Variables(intDomainEnvironment);
                                        try
                                        {
                                            DirectoryEntry oLocalGroup = new DirectoryEntry("WinNT://" + oVar.Domain() + "/" + strName + "/Administrators" + ",Group", oVar.Domain() + "\\" + oVar.ADUser(), oVar.ADPassword());
                                            //DirectoryEntry oLocalGroup = new DirectoryEntry("WinNT://" + oVar.Domain() + "/" + "OHCLEIIS4569/Administrators" + ",Group", oVar.Domain() + "\\" + oVar.ADUser(), oVar.ADPassword());
                                            if (oLocalGroup != null)
                                            {
                                                foreach (object member in (IEnumerable)oLocalGroup.Invoke("Members"))
                                                {
                                                    using (DirectoryEntry memberEntry = new DirectoryEntry(member))
                                                    {
                                                        strAdminsLocal += "<tr><td>" + ((string)memberEntry.Properties["fullName"].Value == "" ? memberEntry.Properties["name"].Value : memberEntry.Properties["name"].Value + " [" + memberEntry.Properties["fullName"].Value + "]") + "</td></tr>";
                                                    }
                                                }
                                            }
                                        }
                                        catch { }
                                        if (strAdminsLocal == "")
                                            strAdminsLocal += "<tr><td><img src=\"/images/alert.gif\" border=\"0\" align=\"absmiddle\"/> There are no local administrators</td></tr>";
                                        strAdminsLocal = "<table cellpadding=\"2\" cellspacing=\"1\" border=\"0\">" + strAdminsLocal + "</table>";
                                    }
                                    catch { }
                                }
                                else
                                {
                                    strAdminsDomain = "<img src='/images/spacer.gif' border='0' height='1' width='25' /><img src='/images/alert.gif' border='0' align='absmiddle'> Please click &quot;Lookup Security&quot; to query the security configuration";
                                    strAdminsLocal = "<img src='/images/spacer.gif' border='0' height='1' width='25' /><img src='/images/alert.gif' border='0' align='absmiddle'> Please click &quot;Lookup Security&quot; to query the security configuration";
                                }
                                #endregion


                                System.IO.FileInfo oFile = new System.IO.FileInfo(oVariables.UploadsFolder() + "birth\\" + "BIRTH_" + intAnswer.ToString() + "_" + oProject.Get(intProject, "number") + ".PDF");
                                if (oFile.Exists == true)
                                {
                                    strDocuments += "<tr><td nowrap colspan=\"2\"><img src=\"/images/icons/pdf.gif\" border=\"0\" align=\"absmiddle\"/> <a href=\"" + oVariables.UploadsFolder() + "birth\\" + "BIRTH_" + intAnswer.ToString() + "_" + oProject.Get(intProject, "number") + ".PDF\">Click here to view the Original Birth Certificate</a></td></tr>";
                                }
                                else
                                {
                                    strDocuments += "<tr><td nowrap colspan=\"2\"><img src=\"/images/alert.gif\" border=\"0\" align=\"absmiddle\"/> There are no documents associated with this device</td></tr>";
                                }
                                //strDocuments += "<tr><td nowrap colspan=\"2\"><img src=\"/images/icons/check.gif\" border=\"0\" align=\"absmiddle\"/> <a href=\"/frame/ondemand/pdf_san.aspx?id=" + intAnswer.ToString() + "\">Click here to view the SAN Registration Form (TEST)</a></td></tr>";
                                //strDocuments += "<tr><td nowrap colspan=\"2\"><img src=\"/images/icons/pdf.gif\" border=\"0\" align=\"absmiddle\"/> <a href=\"/frame/ondemand/pdf_san.aspx?id=" + intAnswer.ToString() + "&prod=true\">Click here to view the SAN Registration Form (PROD)</a></td></tr>";
                                //strDocuments += "<tr><td nowrap colspan=\"2\"><img src=\"/images/icons/html.gif\" border=\"0\" align=\"absmiddle\"/> <a href=\"/frame/ondemand/pdf_sc.aspx?id=" + intServer.ToString() + "\">Click here to view the Service Center Request</a></td></tr>";
                                //strDocuments += "<tr><td nowrap colspan=\"2\"><img src=\"/images/icons/pdf.gif\" border=\"0\" align=\"absmiddle\"/> <a href=\"/frame/ondemand/pdf_birth.aspx?id=" + intAnswer.ToString() + "\">Click here to view the Birth Certificate</a></td></tr>";
                                if (intDesign > 0)
                                    strLinks += "<tr><td nowrap colspan=\"2\"><img src=\"/images/file.gif\" border=\"0\" align=\"absmiddle\"/> <a href=\"javascript:void(0);\" onclick=\"OpenWindow('DESIGN_EQUIPMENT','?id=" + intDesign.ToString() + "');\">Click here to view the design</a></td></tr>";
                                else
                                    strLinks += "<tr><td nowrap colspan=\"2\"><img src=\"/images/file.gif\" border=\"0\" align=\"absmiddle\"/> <a href=\"javascript:void(0);\" onclick=\"OpenWindow('FORECAST_EQUIPMENT','?id=" + intAnswer.ToString() + "');\">Click here to view the design</a></td></tr>";
                                if (intDesign > 0)
                                    strLinks += "<tr><td nowrap colspan=\"2\"><img src=\"/images/file.gif\" border=\"0\" align=\"absmiddle\"/> <a href=\"" + oPage.GetFullLink(intDesignBuilder) + "?id=" + oForecast.GetAnswer(intAnswer, "forecastid") + "&highlight=" + intDesign.ToString() + "\" target=\"_blank\">Click here to view the OVERALL design</a></td></tr>";
                                else
                                    strLinks += "<tr><td nowrap colspan=\"2\"><img src=\"/images/file.gif\" border=\"0\" align=\"absmiddle\"/> <a href=\"" + oPage.GetFullLink(intDesignBuilderOLD) + "?id=" + oForecast.GetAnswer(intAnswer, "forecastid") + "&highlight=" + intAnswer.ToString() + "\" target=\"_blank\">Click here to view the OVERALL design</a></td></tr>";
                                if (intDesign == 0)
                                {
                                    if (intModel == 0)
                                        intType = oModelsProperties.GetType(oForecast.GetModel(intAnswer));
                                    string strExecute = oType.Get(intType, "forecast_execution_path");
                                    if (strExecute != "")
                                    {
                                        strLinks += "<tr><td nowrap colspan=\"2\"><img src=\"/images/config.gif\" border=\"0\" align=\"absmiddle\"/> <a href=\"javascript:void(0);\" onclick=\"OpenWindow('NEW_WINDOW','" + strExecute + "?id=" + intAnswer.ToString() + "&sid=3&view=true');\">Click here to view the device configuration</a></td></tr>";
                                        if (oModelsProperties.IsTypeVMware(intModel) == true)
                                            strLinks += "<tr><td nowrap colspan=\"2\"><img src=\"/images/calendar.gif\" border=\"0\" align=\"absmiddle\"/> <a href=\"javascript:void(0);\" onclick=\"OpenWindow('NEW_WINDOW','" + strExecute + "?id=" + intAnswer.ToString() + "&sid=4&view=true');\">Click here to view the production go live date</a></td></tr>";
                                        else if (oForecast.IsOSDistributed(intAnswer) == true)
                                            strLinks += "<tr><td nowrap colspan=\"2\"><img src=\"/images/calendar.gif\" border=\"0\" align=\"absmiddle\"/> <a href=\"javascript:void(0);\" onclick=\"OpenWindow('NEW_WINDOW','" + strExecute + "?id=" + intAnswer.ToString() + "&sid=6&view=true');\">Click here to view the production go live date</a></td></tr>";
                                        else
                                            strLinks += "<tr><td nowrap colspan=\"2\"><img src=\"/images/calendar.gif\" border=\"0\" align=\"absmiddle\"/> <a href=\"javascript:void(0);\" onclick=\"OpenWindow('NEW_WINDOW','" + strExecute + "?id=" + intAnswer.ToString() + "&sid=5&view=true');\">Click here to view the production go live date</a></td></tr>";
                                    }
                                }

                                #region WMI Call
                                DataTable tblDrives = new DataTable();
                                AddColumn("letter", "System.String", tblDrives);
                                AddColumn("drive", "System.String", tblDrives);
                                AddColumn("size", "System.String", tblDrives);
                                AddColumn("used", "System.String", tblDrives);
                                AddColumn("available", "System.String", tblDrives);
                                DataTable tblApplications = new DataTable();
                                AddColumn("name", "System.String", tblApplications);
                                AddColumn("publisher", "System.String", tblApplications);
                                AddColumn("version", "System.String", tblApplications);
                                AddColumn("installed", "System.String", tblApplications);
                                if (intEnvironment == 1)
                                    strName = "OHCLEIIS4569";

                                if (Request.Cookies["storage"] != null && Request.Cookies["storage"].Value != "0")
                                {
                                    Response.Cookies["storage"].Value = "0";
                                    try
                                    {
                                    //foreach (ManagementObject oItem in oDataPoint.GetWin32Fix(strName, "SELECT * FROM Win32_Processor", "CIMV2", intDomainEnvironment))
                                    //        lblProcessor.Text = oItem["Name"].ToString() + " x " + oItem["CpuStatus"].ToString() + "<br/>";

                                    // Drives
                                    DataRow tblRow;
                                    foreach (ManagementObject oItem in oDataPoint.GetWin32Fix(strName, "SELECT * FROM Win32_LogicalDisk Where DriveType = 3", "CIMV2", intDomainEnvironment))
                                    {
                                        double dblSize = double.Parse(oItem["Size"].ToString()) / 1073741824.00;
                                        double dblFree = double.Parse(oItem["Freespace"].ToString()) / 1073741824.00;
                                        double dblUsed = dblSize - dblFree;
                                        tblRow = tblDrives.NewRow();
                                        tblRow["letter"] = oItem["DeviceID"].ToString();
                                        tblRow["drive"] = oItem["VolumeName"].ToString();
                                        tblRow["size"] = dblSize.ToString("F") + " GB";
                                        tblRow["used"] = dblUsed.ToString("F") + " GB";
                                        tblRow["available"] = dblFree.ToString("F") + " GB";
                                        tblDrives.Rows.Add(tblRow);
                                    }
                                    }
                                    catch
                                    {
                                        lblLocal.Text = "<img src='/images/spacer.gif' border='0' height='1' width='25' /><img src='/images/alert.gif' border='0' align='absmiddle'> There was a problem contacting the server";
                                        btnStorage.Enabled = false;
                                    }
                                }
                                else
                                    lblLocal.Text = "<img src='/images/spacer.gif' border='0' height='1' width='25' /><img src='/images/alert.gif' border='0' align='absmiddle'> Please click &quot;Refresh Data&quot; to contact the server";

                                // Applications
                                if (Request.Cookies["applications"] != null && Request.Cookies["applications"].Value != "0")
                                {
                                    Response.Cookies["applications"].Value = "0";
                                    try
                                    {
                                    DataRow tblRow;
                                    string[] sFolders = null;
                                    
                                    
                                    ManagementClass mc = oDataPoint.GetClassFix(strName, "\\root\\default", "StdRegProv", intDomainEnvironment);
                                    ManagementBaseObject inParams = mc.GetMethodParameters("GetStringValue");
                                    string strLocation = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall";
                                    inParams["hDefKey"] = LOCAL_MACHINE;
                                    inParams["sSubKeyName"] = strLocation;
                                    ManagementBaseObject outParams = mc.InvokeMethod("EnumKey", inParams, null);
                                    if (Convert.ToUInt32(outParams["ReturnValue"]) == 0)
                                        sFolders = outParams["sNames"] as String[];
                                    for (int i = 0; i < sFolders.Length; i++)
                                    {
                                        string strApplicationName = "";
                                        ManagementBaseObject inParams2 = mc.GetMethodParameters("GetStringValue");
                                        inParams["hDefKey"] = LOCAL_MACHINE;

                                        inParams["sSubKeyName"] = strLocation + "\\" + sFolders[i];
                                        inParams["sValueName"] = "DisplayName";
                                        ManagementBaseObject outParams2 = mc.InvokeMethod("GetStringValue", inParams, null);
                                        if (Convert.ToUInt32(outParams2["ReturnValue"]) == 0)
                                            strApplicationName = outParams2["sValue"].ToString();

                                        if (strApplicationName != "")
                                        {
                                            tblRow = tblApplications.NewRow();
                                            tblRow["name"] = strApplicationName;

                                            inParams["sSubKeyName"] = strLocation + "\\" + sFolders[i];
                                            inParams["sValueName"] = "Publisher";
                                            ManagementBaseObject outParams3 = mc.InvokeMethod("GetStringValue", inParams, null);
                                            if (Convert.ToUInt32(outParams3["ReturnValue"]) == 0)
                                                tblRow["publisher"] = outParams3["sValue"].ToString();

                                            inParams["sSubKeyName"] = strLocation + "\\" + sFolders[i];
                                            inParams["sValueName"] = "DisplayVersion";
                                            ManagementBaseObject outParams4 = mc.InvokeMethod("GetStringValue", inParams, null);
                                            if (Convert.ToUInt32(outParams4["ReturnValue"]) == 0)
                                                tblRow["version"] = outParams4["sValue"].ToString();

                                            inParams["sSubKeyName"] = strLocation + "\\" + sFolders[i];
                                            inParams["sValueName"] = "InstallDate";
                                            ManagementBaseObject outParams5 = mc.InvokeMethod("GetStringValue", inParams, null);
                                            if (Convert.ToUInt32(outParams5["ReturnValue"]) == 0)
                                            {
                                                string strInstallDate = outParams5["sValue"].ToString();
                                                if (strInstallDate.Length == 8)
                                                {
                                                    // Format = 20090328
                                                    string strInstallYear = strInstallDate.Substring(0, 4);
                                                    string strInstallMonth = strInstallDate.Substring(4, 2);
                                                    string strInstallDay = strInstallDate.Substring(6, 2);
                                                    strInstallDate = strInstallMonth + "/" + strInstallDay + "/" + strInstallYear;
                                                }
                                                tblRow["installed"] = strInstallDate;
                                            }

                                            tblApplications.Rows.Add(tblRow);
                                        }
                                    }
                                    }
                                    catch
                                    {
                                        lblApplications.Text = "<img src='/images/spacer.gif' border='0' height='1' width='25' /><img src='/images/alert.gif' border='0' align='absmiddle'> There was a problem contacting the server";
                                        btnApplications.Enabled = false;
                                    }
                                }
                                else
                                    lblApplications.Text = "<img src='/images/spacer.gif' border='0' height='1' width='25' /><img src='/images/alert.gif' border='0' align='absmiddle'> Please click &quot;Refresh List&quot; to contact the server";

                                rptLocal.DataSource = tblDrives;
                                rptLocal.DataBind();
                                lblLocal.Visible = (rptLocal.Items.Count == 0);
                                DataView dvApplications = tblApplications.DefaultView;
                                dvApplications.Sort = "name";
                                rptApplications.DataSource = dvApplications;
                                rptApplications.DataBind();
                                lblApplications.Visible = (rptApplications.Items.Count == 0);
                                #endregion
                            }
                        }

                    }
                    else
                    {
                        Response.Redirect("/datapoint/asset/datapoint_asset_search.aspx?multiple=true&t=name&q=" + oFunction.encryptQueryString(strQuery) + "&r=0");
                    }
                }
                btnClose.Attributes.Add("onclick", "window.close();return false;");
                btnPrint.Attributes.Add("onclick", "window.print();return false;");
                btnProjectChange.Enabled = false;
                txtProjectChange.Enabled = false;
                btnSave.Attributes.Add("onclick", oDataPoint.LoadValidation("ProcessControlButton()"));
                btnSaveClose.Attributes.Add("onclick", oDataPoint.LoadValidation("ProcessControlButton()"));
                btnSecurity.Attributes.Add("onclick", "return ProcessButton(this) && ProcessControlButton();");
                btnAddSVE.Attributes.Add("onclick", "return ProcessButton(this) && ProcessControlButton();");
                btnStorage.Attributes.Add("onclick", "return ProcessButton(this) && ProcessControlButton();");
                btnApplications.Attributes.Add("onclick", "return ProcessButton(this) && ProcessControlButton();");
                ddlPlatformClass.Attributes.Add("onchange", "PopulateEnvironments('" + ddlPlatformClass.ClientID + "','" + ddlPlatformEnvironment.ClientID + "',0);");
                ddlPlatformEnvironment.Attributes.Add("onchange", "UpdateDropDownHidden('" + ddlPlatformEnvironment.ClientID + "','" + hdnEnvironment.ClientID + "');");
                btnOutput.Attributes.Add("onclick", "return ProcessButton(this) && ProcessControlButton();");
                btnClusterQuery.Attributes.Add("onclick", "return ProcessButton(this) && ProcessControlButton();");
            }
            else
                panDenied.Visible = true;
        }
        public string CheckADGroup(int intDomain, int intDomainEnvironment, string _group, bool _ncb)
        {
            string strReturn = "";
            char[] strSplit = { ';' };
            AD oAD = new AD(intProfile, dsn, intDomainEnvironment);
            DirectoryEntry oEntry = oAD.GroupSearch(_group);
            if (oEntry != null)
            {
                string strDomains = oAD.GetGroupMembers(oEntry);
                string[] strDomainGroups = strDomains.Split(strSplit);
                if (strDomainGroups.Length > 0)
                {
                    for (int ii = 0; ii < strDomainGroups.Length; ii++)
                    {
                        if (strDomainGroups[ii].Trim() != "")
                        {
                            string strDomainGroup = strDomainGroups[ii].Trim();
                            string strDomainGroupUser = strDomainGroup.ToUpper();
                            int intDomainUser = oUser.GetId(strDomainGroupUser);
                            if (_ncb == true && intDomainUser == 0 && strDomainGroupUser.StartsWith("E") || strDomainGroupUser.StartsWith("T"))
                            {
                                strDomainGroupUser = "X" + strDomainGroupUser.Substring(1);
                                intDomainUser = oUser.GetId(strDomainGroupUser);
                            }
                            if (intDomainUser > 0)
                                strDomainGroup = "<a href=\"javascript:void(0);\" class=\"lookup\" onclick=\"OpenWindow('PROFILE','?userid=" + intDomainUser.ToString() + "');\">" + oUser.GetFullName(intDomainUser) + " [" + oUser.GetName(intDomainUser) + "]</a>";
                            strReturn += "<tr><td>" + strDomainGroup + "</td></tr>";
                        }
                    }
                }
            }
            else
                strReturn += "<tr><td><img src=\"/images/error.gif\" border=\"0\" align=\"absmiddle\"/> The group " + _group + " does not exist in the &quot;" + oDomain.Get(intDomain, "name") + "&quot; domain</td></tr>";
            if (strReturn == "")
                strReturn = "<tr><td><i>There are no members</i></td></tr>";
            return strReturn;
        }
        private void LoadIP(string strIP, ListBox lstIP, HiddenField hdnIP, HiddenField hdnIPExists)
        {
            int intIP = Int32.Parse(strIP);
            string strName = oIPAddresses.GetName(intIP, 0);
            int intNetwork = 0;
            if (Int32.TryParse(oIPAddresses.Get(intIP, "networkid"), out intNetwork) && intNetwork > 0)
                strName += " (VLAN: " + oIPAddresses.GetNetworkVlan(intNetwork).ToString() + ")";
            lstIP.Items.Add(new ListItem(strName, strIP));
            hdnIP.Value += strIP + ",";
            hdnIPExists.Value += strIP + ",";
        }
        private void LoadIP(int intIP, TextBox txtIP1, TextBox txtIP2, TextBox txtIP3, TextBox txtIP4)
        {
            LoadIP(oIPAddresses.GetName(intIP, 0), txtIP1, txtIP2, txtIP3, txtIP4);
        }
        private void LoadIP(string strIP, TextBox txtIP1, TextBox txtIP2, TextBox txtIP3, TextBox txtIP4)
        {
            int intIP1 = 0;
            int intIP2 = 0;
            int intIP3 = 0;
            int intIP4 = 0;
            if (strIP.Contains(".") == true)
            {
                Int32.TryParse(strIP.Substring(0, strIP.IndexOf(".")), out intIP1);
                strIP = strIP.Substring(strIP.IndexOf(".") + 1);
            }
            if (strIP.Contains(".") == true)
            {
                Int32.TryParse(strIP.Substring(0, strIP.IndexOf(".")), out intIP2);
                strIP = strIP.Substring(strIP.IndexOf(".") + 1);
            }
            if (strIP.Contains(".") == true)
            {
                Int32.TryParse(strIP.Substring(0, strIP.IndexOf(".")), out intIP3);
                strIP = strIP.Substring(strIP.IndexOf(".") + 1);
                Int32.TryParse(strIP, out intIP4);
            }
            txtIP1.Text = intIP1.ToString();
            txtIP2.Text = intIP2.ToString();
            txtIP3.Text = intIP3.ToString();
            txtIP4.Text = intIP4.ToString();
        }
        private void AddColumn(string _name, string _type, DataTable _table)
        {
            DataColumn myDataColumn = new DataColumn();
            myDataColumn.DataType = System.Type.GetType(_type);
            myDataColumn.ColumnName = _name;
            _table.Columns.Add(myDataColumn);
        }
        private string GetStorage(int intAnswer, int intCluster2, int intCSMConfig2, int intNumber2, int intModel, string strName)
        {
            DataSet dsLuns = new DataSet();
            if (intCluster2 == 0)
                dsLuns = oStorage.GetLuns(intAnswer, 0, intCluster2, intCSMConfig2, intNumber2);
            else
                dsLuns = oStorage.GetLunsClusterNonShared(intAnswer, intCluster2, intNumber2);
            bool boolOverride = (oForecast.GetAnswer(intAnswer, "storage_override") == "1");
            string strStorage = AddStorage(dsLuns, intModel, boolOverride);
            if (strStorage != "")
            {
                strStorage = "<tr class=\"bold\" bgcolor=\"#EEEEEE\"><td>#</td><td>Path</td><td>Performance</td><td>Size in Prod</td><td>Size in QA</td><td>Size in Test</td><td>Replication</td><td>High Availability</td></tr>" + strStorage;
                strStorage = "<table id=\"tblStorage" + intCluster2.ToString() + "_" + intNumber2.ToString() + "\" width=\"100%\" cellpadding=\"4\" cellspacing=\"0\" border=\"0\" style=\"border:solid 1px #CCCCCC\">" + strStorage + "</table>";
            }
            return strStorage;
        }
        private string GetStorageShared(int intAnswer, int intModel)
        {
            bool boolOverride = (oForecast.GetAnswer(intAnswer, "storage_override") == "1");
            string strClusterNames = "";
            string strStorage = "";
            int intClusterOLD = 0;
            DataSet dsCluster = oServer.GetAnswerClusters(intAnswer);
            foreach (DataRow drCluster in dsCluster.Tables[0].Rows)
            {
                int intClusterID = Int32.Parse(drCluster["clusterid"].ToString());
                if (intClusterOLD != intClusterID)
                {
                    if (intClusterID > 0)
                    {
                        DataSet dsServers = oServer.GetClusters(intClusterID);
                        foreach (DataRow drServer in dsServers.Tables[0].Rows)
                        {
                            int intServer = Int32.Parse(drServer["id"].ToString());
                            if (strClusterNames != "")
                                strClusterNames += ", ";
                            strClusterNames += oServer.GetName(intServer, true);
                        }
                        DataSet dsLuns = oStorage.GetLunsClusterShared(intAnswer, intClusterID);
                        strStorage += AddStorage(dsLuns, intModel, boolOverride);
                    }
                }
                intClusterOLD = intClusterID;
            }
            if (strStorage != "")
            {
                strStorage = "<tr class=\"bold\" bgcolor=\"#EEEEEE\"><td>#</td><td>Path</td><td>Performance</td><td>Size in Prod</td><td>Size in QA</td><td>Size in Test</td><td>Replication</td><td>High Availability</td></tr>" + strStorage;
                strStorage = "<table id=\"tblStorage" + intClusterOLD.ToString() + "\" width=\"100%\" cellpadding=\"4\" cellspacing=\"0\" border=\"0\" style=\"border:solid 1px #CCCCCC\">" + strStorage + "</table>";
                strStorage = "<br/><a href=\"javascript:void(0);\" class=\"redlink\">Shared Storage for " + strClusterNames + "</a><br/>" + strStorage;
            }
            return strStorage;
        }
        private string AddStorage(DataSet dsLuns, int intModel, bool boolOverride)
        {
            string strStorage = "";
            int intRow = 0;
            foreach (DataRow drLun in dsLuns.Tables[0].Rows)
            {
                intRow++;
                strStorage += "<tr>";
                strStorage += "<td>" + intRow.ToString() + "</td>";
                string strLetter = drLun["letter"].ToString();
                if (strLetter == "")
                {
                    if (drLun["driveid"].ToString() == "-1000")
                        strLetter = "E";
                    else if (drLun["driveid"].ToString() == "-100")
                        strLetter = "F";
                    else if (drLun["driveid"].ToString() == "-10")
                        strLetter = "P";
                    else if (drLun["driveid"].ToString() == "-1")
                        strLetter = "Q";
                }
                if ((boolOverride == true && drLun["driveid"].ToString() == "0") || oForecast.IsOSMidrange(intAnswer) == true)
                    strStorage += "<td>" + drLun["path"].ToString() + "</td>";
                else
                    strStorage += "<td>" + strLetter + ":" + drLun["path"].ToString() + "</td>";
                strStorage += "<td>" + drLun["performance"].ToString() + "</td>";
                strStorage += "<td>" + drLun["size"].ToString() + " GB / " + (drLun["actual_size"].ToString() == "-1" ? "---" : drLun["actual_size"].ToString() + " GB") + "</td>";
                strStorage += "<td>" + drLun["size_qa"].ToString() + " GB / " + (drLun["actual_size_qa"].ToString() == "-1" ? "---" : drLun["actual_size_qa"].ToString() + " GB") + "</td>";
                strStorage += "<td>" + drLun["size_test"].ToString() + " GB / " + (drLun["actual_size_test"].ToString() == "-1" ? "---" : drLun["actual_size_test"].ToString() + " GB") + "</td>";
                strStorage += "<td>" + (drLun["replicated"].ToString() == "0" ? "No" : "Yes") + " / " + (drLun["actual_replicated"].ToString() == "-1" ? "---" : (drLun["actual_replicated"].ToString() == "0" ? "No" : "Yes")) + "</td>";
                strStorage += "<td>" + (drLun["high_availability"].ToString() == "0" ? "No" : "Yes (" + drLun["size"].ToString() + " GB)") + " / " + (drLun["actual_high_availability"].ToString() == "-1" ? "---" : (drLun["actual_high_availability"].ToString() == "0" ? "No" : "Yes (" + drLun["actual_size"].ToString() + " GB)")) + "</td>";
                strStorage += "</tr>";
                DataSet dsPoints = oStorage.GetMountPoints(Int32.Parse(drLun["id"].ToString()));
                int intPoint = 0;
                foreach (DataRow drPoint in dsPoints.Tables[0].Rows)
                {
                    intRow++;
                    intPoint++;
                    strStorage += "<tr>";
                    strStorage += "<td>" + intRow.ToString() + "</td>";
                    if (oForecast.IsOSMidrange(intAnswer) == true)
                        strStorage += "<td>" + drPoint["path"].ToString() + "</td>";
                    else
                        strStorage += "<td>" + strLetter + ":\\SH" + drLun["driveid"].ToString() + "VOL" + (intPoint < 10 ? "0" : "") + intPoint.ToString() + "</td>";
                    strStorage += "<td>" + drPoint["performance"].ToString() + "</td>";
                    strStorage += "<td>" + drPoint["size"].ToString() + " GB / " + (drPoint["actual_size"].ToString() == "-1" ? "---" : drPoint["actual_size"].ToString() + " GB") + "</td>";
                    strStorage += "<td>" + drPoint["size_qa"].ToString() + " GB / " + (drPoint["actual_size_qa"].ToString() == "-1" ? "---" : drPoint["actual_size_qa"].ToString() + " GB") + "</td>";
                    strStorage += "<td>" + drPoint["size_test"].ToString() + " GB / " + (drPoint["actual_size_test"].ToString() == "-1" ? "---" : drPoint["actual_size_test"].ToString() + " GB") + "</td>";
                    strStorage += "<td>" + (drPoint["replicated"].ToString() == "0" ? "No" : "Yes") + " / " + (drPoint["actual_replicated"].ToString() == "-1" ? "---" : (drPoint["actual_replicated"].ToString() == "0" ? "No" : "Yes")) + "</td>";
                    strStorage += "<td>" + (drPoint["high_availability"].ToString() == "0" ? "No" : "Yes (" + drPoint["size"].ToString() + " GB)") + " / " + (drPoint["actual_high_availability"].ToString() == "-1" ? "---" : (drPoint["actual_high_availability"].ToString() == "0" ? "No" : "Yes (" + drPoint["actual_size"].ToString() + " GB)")) + "</td>";
                    strStorage += "</tr>";
                }
            }
            return strStorage;
        }
        protected void btnProvisioning_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(Request.Path + "?q=" + Request.QueryString["q"] + "&id=" + oFunction.encryptQueryString(intID.ToString()) + "&menu_tab=7");
        }
        protected void btnAudit_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(Request.Path + "?q=" + Request.QueryString["q"] + "&id=" + oFunction.encryptQueryString(intID.ToString()) + "&menu_tab=8");
        }
        protected void btnSecurity_Click(Object Sender, EventArgs e)
        {
            Response.Cookies["security"].Value = "1";
            Response.Redirect(Request.Path + "?q=" + Request.QueryString["q"] + "&id=" + oFunction.encryptQueryString(intID.ToString()) + "&menu_tab=11");
        }
        protected void btnStorage_Click(Object Sender, EventArgs e)
        {
            Response.Cookies["storage"].Value = "1";
            Response.Redirect(Request.Path + "?q=" + Request.QueryString["q"] + "&id=" + oFunction.encryptQueryString(intID.ToString()) + "&menu_tab=9");
        }
        protected void btnApplications_Click(Object Sender, EventArgs e)
        {
            Response.Cookies["applications"].Value = "1";
            Response.Redirect(Request.Path + "?q=" + Request.QueryString["q"] + "&id=" + oFunction.encryptQueryString(intID.ToString()) + "&menu_tab=13");
        }
        protected void btnPing_Click(Object Sender, EventArgs e)
        {
        }
        protected void btnNew_Click(Object Sender, EventArgs e)
        {
            Response.Redirect("/datapoint/asset/datapoint_asset_search.aspx");
        }
        protected void btnSave_Click(Object Sender, EventArgs e)
        {
            int intSave = Save();
            Response.Redirect(Request.Path + "?q=" + Request.QueryString["q"] + "&id=" + oFunction.encryptQueryString(intID.ToString()) + "&" + (intSave > 0 ? "save" : "error") + "=" + intSave.ToString());
        }
        protected void btnSaveClose_Click(Object Sender, EventArgs e)
        {
            int intSave = Save();
            Response.Redirect(Request.Path + "?q=" + Request.QueryString["q"] + "&id=" + oFunction.encryptQueryString(intID.ToString()) + "&" + (intSave > 0 ? "close" : "error") + "=" + intSave.ToString());
        }
        private string SaveIP(string strHidden, int intAutoAssign, int intFinal, int intVMotion, int intAvamar, int intClass, int intAsset)
        {
            string strNewIPs = "";
            string[] strIPs = Request.Form[strHidden].Split(strIPSplit);
            for (int ii = 0; ii < strIPs.Length; ii++)
            {
                if (strIPs[ii].Trim() != "")
                {
                    int intIP = 0;
                    string strIP = strIPs[ii].Trim();
                    if (Int32.TryParse(strIP, out intIP) == false)
                    {
                        string strAddress = strIP;
                        // New IP Address
                        int intIP1 = 0;
                        int intIP2 = 0;
                        int intIP3 = 0;
                        int intIP4 = 0;
                        if (strIP.Contains(".") == true)
                        {
                            Int32.TryParse(strIP.Substring(0, strIP.IndexOf(".")), out intIP1);
                            strIP = strIP.Substring(strIP.IndexOf(".") + 1);
                        }
                        if (strIP.Contains(".") == true)
                        {
                            Int32.TryParse(strIP.Substring(0, strIP.IndexOf(".")), out intIP2);
                            strIP = strIP.Substring(strIP.IndexOf(".") + 1);
                        }
                        if (strIP.Contains(".") == true)
                        {
                            Int32.TryParse(strIP.Substring(0, strIP.IndexOf(".")), out intIP3);
                            strIP = strIP.Substring(strIP.IndexOf(".") + 1);
                            Int32.TryParse(strIP, out intIP4);
                        }
                        if (intIP1 > 0 && intIP2 > 0 && intIP3 > 0 && intIP4 > 0)
                        {
                            intIP = oIPAddresses.Add(0, intIP1, intIP2, intIP3, intIP4, intProfile);
                            oServer.AddIP(intServer, intIP, intAutoAssign, intFinal, intVMotion, intAvamar);
                            if (chkBluecat.Checked == true)
                            {
                                System.Net.NetworkCredential oCredentials = new System.Net.NetworkCredential(oVariable.ADUser(), oVariable.ADPassword(), oVariable.Domain());
                                ClearViewWebServices oWebService = new ClearViewWebServices();
                                oWebService.Timeout = Timeout.Infinite;
                                oWebService.Credentials = oCredentials;
                                oWebService.Url = oVariable.WebServiceURL();
                                string strName = oServer.GetName(intServer, true);
                                string strDescription = oIPAddresses.GetDescription(intIP, strName, intAsset, dsnAsset, "", intEnvironment);
                                string strSerial = oAsset.Get(intAsset, "serial");
                                string strEMailIdsBCC = "";
                                oLog.AddEvent(strName, strSerial, "Calling Web Service Function CreateBluecatDNS(" + strAddress + ", " + strName + ", " + strDescription + ", " + "" + ") on " + oVariable.WebServiceURL(), LoggingType.Information);
                                string strDNS = oWebService.CreateBluecatDNS(strAddress, strName, strDescription, "");
                                if (strDNS == "SUCCESS" || strDNS.StartsWith("***DUPLICATE") == true)
                                {
                                    oLog.AddEvent(strName, strSerial, "BlueCat DNS Record = SUCCESS", LoggingType.Information);
                                }
                                else
                                {
                                    if (strDNS.StartsWith("***CONFLICT") == true)
                                    {
                                        oLog.AddEvent(strName, strSerial, "BlueCat DNS Record = CONFLICT", LoggingType.Warning);
                                        strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DEVELOPER_WARNING");
                                        oFunction.SendEmail("BlueCat DNS Automation Conflict", strEMailIdsBCC, "", "", "BlueCat DNS Automation Conflict", "<p>There was a CONFLICT creating the BlueCat DNS Record for " + strName + " (SERVERID: " + intServer.ToString() + ")</p><p>Error: " + strDNS + "</p>", true, false);
                                    }
                                    else
                                    {
                                        oLog.AddEvent(strName, strSerial, "BlueCat DNS Record = ERROR: " + strDNS, LoggingType.Error);
                                        strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DEVELOPER_ERROR,EMAILGRP_PROVISIONING_SUPPORT");
                                        oFunction.SendEmail("BlueCat DNS Automation Error", strEMailIdsBCC, "", "", "BlueCat DNS Automation Error", "<p>There was an ERROR creating the BlueCat DNS Record for " + strName + " (SERVERID: " + intServer.ToString() + ")</p><p>Error: " + strDNS + "</p><p>NOTE: Since this is an unexpected error, there was no task initiated to fix this problem. You must correct it yourself.</p>", true, false);
                                    }
                                }
                                oLog.AddEvent(strName, strSerial, "BlueCat DNS Record Finished", LoggingType.Information);

                            }
                            strNewIPs += intIP.ToString() + ",";
                        }
                    }
                    else
                        strNewIPs += strIP + ",";
                }
            }
            return strNewIPs;
        }
        private void RemoveIPs(string strIPExists, int intAutoAssign, int intFinal, int intVMotion, int intAvamar)
        {
            DataSet dsIPs = oServer.GetIP(intServer, intAutoAssign, intFinal, intVMotion, intAvamar);
            string[] strIPs = strIPExists.Split(strIPSplit);
            foreach (DataRow drIP in dsIPs.Tables[0].Rows)
            {
                bool boolFoundIP = false;
                int intIPCompare = Int32.Parse(drIP["ipaddressid"].ToString());
                for (int ii = 0; ii < strIPs.Length; ii++)
                {
                    int intIP = 0;
                    string strIP = strIPs[ii].Trim();
                    if (Int32.TryParse(strIP, out intIP) == true)
                    {
                        if (intIP == intIPCompare)
                        {
                            // The IP Address exists in both places...do not delete
                            boolFoundIP = true;
                            break;
                        }
                    }
                }
                if (boolFoundIP == false)
                {
                    // The IP Address no longer exists...must have been deleted...
                    oServer.UpdateIP(intServer, intIPCompare, intAutoAssign, intFinal, intVMotion, intAvamar, dsnIP);
                }
            }
        }
        protected int Save()
        {
            // Update Server
            DataSet dsServers = oServer.Get(intServer);
            int intTemplate = Int32.Parse(dsServers.Tables[0].Rows[0]["templateid"].ToString());
            int intDomain = Int32.Parse(dsServers.Tables[0].Rows[0]["domainid"].ToString());
            int intDomainTest = Int32.Parse(dsServers.Tables[0].Rows[0]["test_domainid"].ToString());
            int intINF = Int32.Parse(dsServers.Tables[0].Rows[0]["infrastructure"].ToString());
            int intDR = Int32.Parse(dsServers.Tables[0].Rows[0]["dr"].ToString());
            int intHA = Int32.Parse(dsServers.Tables[0].Rows[0]["ha"].ToString());
            int intDRExist = Int32.Parse(dsServers.Tables[0].Rows[0]["dr_exist"].ToString());
            string strDRName = dsServers.Tables[0].Rows[0]["dr_name"].ToString();
            int intDRCons = Int32.Parse(dsServers.Tables[0].Rows[0]["dr_consistency"].ToString());
            int intDRConsID = Int32.Parse(dsServers.Tables[0].Rows[0]["dr_consistencyid"].ToString());
            int intLocal = Int32.Parse(dsServers.Tables[0].Rows[0]["local_storage"].ToString());
            int intAccounts = Int32.Parse(dsServers.Tables[0].Rows[0]["accounts"].ToString());
            int intF = Int32.Parse(dsServers.Tables[0].Rows[0]["fdrive"].ToString());
            int intDBA = Int32.Parse(dsServers.Tables[0].Rows[0]["dba"].ToString());
            int intPNC = Int32.Parse(dsServers.Tables[0].Rows[0]["pnc"].ToString());
            oServer.Update(intServer, Int32.Parse(ddlPlatformOS.SelectedItem.Value), Int32.Parse(ddlPlatformServicePack.SelectedItem.Value), intTemplate, Int32.Parse(ddlPlatformDomain.SelectedItem.Value), intDomainTest, intINF, intHA, intDR, intDRExist, strDRName, intDRCons, intDRConsID, 1, intDBA, intPNC);

            // Administration
            if (panAdministration.Visible == true)
                oServer.UpdateSVECluster(intServer, Int32.Parse(ddlSVECluster.SelectedItem.Value));

            oServer.UpdateBuildStarted(intServer, txtPlatformBuildStarted.Text, true);
            oServer.UpdateBuildCompleted(intServer, txtPlatformBuildCompleted.Text);
            oServer.UpdateBuildReady(intServer, txtPlatformBuildReady.Text, true);

            if (txtDesignCommitment.Text != "")
                oForecast.UpdateAnswerImplementation(intAnswer, DateTime.Parse(txtDesignCommitment.Text));
            oForecast.UpdateAnswerCompleted(intAnswer, txtDesignCompletion.Text);
            if (txtDesignProduction.Text != "")
                oForecast.UpdateAnswerProduction(intAnswer, DateTime.Parse(txtDesignProduction.Text));
            oForecast.UpdateAnswerFinished(intAnswer, txtDesignFinished.Text);

            string strDecom = "";
            bool boolDecom = false;
            // Check Decomission
            if ((oServer.Get(intServer, "decommissioned") == "" && txtPlatformDecommissioned.Text != "") || (oServer.Get(intServer, "decommissioned") != "" && txtPlatformDecommissioned.Text == ""))
            {
                boolDecom = true;
                strDecom = txtPlatformDecommissioned.Text;
            }

            
            // Update Asset
            int intAssetID = 0;
            int intAsset = 0;
            int intAssetClass = 0;
            int intAssetEnv = 0;
            DataSet dsAssets = oServer.GetAssets(intServer);
            foreach (DataRow drAsset in dsAssets.Tables[0].Rows)
            {
                if (drAsset["latest"].ToString() == "1")
                {
                    intAssetID = Int32.Parse(drAsset["id"].ToString());
                    intAsset = Int32.Parse(drAsset["assetid"].ToString());
                    intAssetClass = Int32.Parse(drAsset["classid"].ToString());
                    intAssetEnv = Int32.Parse(drAsset["environmentid"].ToString());
                    break;
                }
            }
            if (boolDecom == true)
            {
                // Update Server
                oServer.UpdateDecommissioned(intServer, strDecom);
                oLog.AddEvent(oServer.GetName(intServer, true), "", "The decommission date was set to " + strDecom + " by " + oUser.GetFullNameWithLanID(intProfile), LoggingType.Information);
                foreach (DataRow drAsset in dsAssets.Tables[0].Rows)
                {
                    if (drAsset["latest"].ToString() == "1" || drAsset["dr"].ToString() == "1")
                    {
                        int intDecomAsset = Int32.Parse(drAsset["assetid"].ToString());
                        // Update Server Asset
                        oServer.UpdateAssetDecom(intServer, intDecomAsset, strDecom);
                        oLog.AddEvent(oServer.GetName(intServer, true), oAsset.Get(intDecomAsset, "serial"), "The decommission date was set to " + strDecom + " by " + oUser.GetFullNameWithLanID(intProfile), LoggingType.Information);
                        // Update Status of Asset
                        if (strDecom == "") // Decom date is cleared...must be back in use
                            oAsset.AddStatus(intDecomAsset, txtName.Text, (int)AssetStatus.InUse, intProfile, DateTime.Now);
                        else
                            oAsset.AddStatus(intDecomAsset, txtName.Text, (int)AssetStatus.Decommissioned, intProfile, DateTime.Parse(strDecom));
                    }
                }
            }
            int intClass = Int32.Parse(ddlPlatformClass.SelectedItem.Value);
            oServer.UpdateAsset(intAssetID, intClass, Int32.Parse(Request.Form[hdnEnvironment.UniqueID]));

            
            // Update IPs
            if (panIPs.Visible == true)
            {
                if (chkDHCP.Checked == true)
                    oServer.UpdateDHCP(intServer, txtIPDHCP1.Text + "." + txtIPDHCP2.Text + "." + txtIPDHCP3.Text + "." + txtIPDHCP4.Text);
                string strIPAssignedExists = SaveIP(hdnIPAssigned.UniqueID, 1, 0, 0, 0, intClass, intAsset);
                string strIPFinalExists = SaveIP(hdnIPFinal.UniqueID, 0, 1, 0, 0, intClass, intAsset);
                string strIPVMotionExists = "";
                if (trVMotion.Visible == true)
                    strIPVMotionExists = SaveIP(hdnIPVMotion.UniqueID, 0, 0, 1, 0, intClass, intAsset);
                string strIPPrivateExists = "";
                if (trPrivate.Visible == true)
                    strIPPrivateExists = SaveIP(hdnIPPrivate.UniqueID, 0, 0, 1, 0, intClass, intAsset);
                string strIPAvamarExists = SaveIP(hdnIPAvamar.UniqueID, 0, 0, 0, 1, intClass, intAsset);
                // Remove Unused IPs
                RemoveIPs(strIPAssignedExists, 1, 0, 0, 0);
                RemoveIPs(strIPFinalExists, 0, 1, 0, 0);
                if (trVMotion.Visible == true)
                    RemoveIPs(strIPVMotionExists, 0, 0, 1, 0);
                if (trPrivate.Visible == true)
                    RemoveIPs(strIPPrivateExists, 0, 0, 1, 0);
                RemoveIPs(strIPAvamarExists, 0, 0, 0, 1);
            }


            // Update Components
            if (panComponents.Visible == true && Request.Form["hdnComponents"] != null)
            {
                string strComponents = Request.Form["hdnComponents"];
                if (strComponents != "")
                {
                    oServerName.DeleteComponentDetailSelected(intServer);
                    while (strComponents != "")
                    {
                        int intDetail = Int32.Parse(strComponents.Substring(0, strComponents.IndexOf("&")));
                        if (intDetail > 0)
                        {
                            oServerName.AddComponentDetailSelected(intServer, intDetail, 0, false);
                            oServerName.AddComponentDetailPrerequisites(intServer, intDetail, false);
                        }
                        strComponents = strComponents.Substring(strComponents.IndexOf("&") + 1);
                    }
                }
            }
            int intRole = 0;
            if (Int32.TryParse(oForecast.GetAnswer(intAnswer, "applicationid"), out intRole) == true)
            {
                // Load Application Prerequisites
                DataSet dsInclude = oServerName.GetComponentDetailSelectedRelated(intRole, 1);
                foreach (DataRow drInclude in dsInclude.Tables[0].Rows)
                {
                    int intDetail = Int32.Parse(drInclude["detailid"].ToString());
                    oServerName.AddComponentDetailSelected(intServer, intDetail, -1, false);
                    oServerName.AddComponentDetailPrerequisites(intServer, intDetail, false);
                }
            }

            
            // Update Answer
            int intOwner = 0;
            if (Request.Form[hdnApplicationOwner.UniqueID] != "")
                intOwner = Int32.Parse(Request.Form[hdnApplicationOwner.UniqueID]);
            int intEngineer = 0;
            if (Request.Form[hdnApplicationEngineer.UniqueID] != "")
                intEngineer = Int32.Parse(Request.Form[hdnApplicationEngineer.UniqueID]);
            int intMnemonic = 0;
            if (Request.Form[hdnApplicationMnemonic.UniqueID] != "")
                intMnemonic = Int32.Parse(Request.Form[hdnApplicationMnemonic.UniqueID]);
            int intCostCenter = 0;
            if (Request.Form[hdnApplicationCostCenter.UniqueID] != "")
                intCostCenter = Int32.Parse(Request.Form[hdnApplicationCostCenter.UniqueID]);
            oForecast.UpdateAnswer(intAnswer, txtApplicationName.Text, txtApplicationCode.Text, intMnemonic, intCostCenter, Int32.Parse(ddlApplicationDR.SelectedItem.Value), Int32.Parse(Request.Form[hdnApplicationClient.UniqueID]), Int32.Parse(Request.Form[hdnApplicationPrimary.UniqueID]), Int32.Parse(Request.Form[hdnApplicationAdministrative.UniqueID]), intOwner, intEngineer, 0, 0);

            oLog.AddEvent(oServer.GetName(intServer, true), "", "The server record was modified by " + oUser.GetFullNameWithLanID(intProfile), LoggingType.Information);

            // Update Name
            //int intReturn = oServer.UpdateName(intServer, 0, txtName.Text, intProfile, txtPlatformSerial.Text, chkName.Checked, dsnAsset);
            return 10;
        }

        // Adminstration Functions
        protected void btnAddSVE_Click(Object Sender, EventArgs e)
        {
            oServer.UpdateSVECluster(intID, Int32.Parse(ddlSVECluster.SelectedItem.Value));
            Response.Redirect(Request.Path + "?q=" + Request.QueryString["q"] + "&id=" + oFunction.encryptQueryString(intID.ToString()) + "&admin=true&menu_tab=14");
        }
        protected void btnDeleteSVE_Click(Object Sender, EventArgs e)
        {
            LinkButton oDelete = (LinkButton)Sender;
            oServer.DeleteSVE(Int32.Parse(oDelete.CommandArgument));
            Response.Redirect(Request.Path + "?q=" + Request.QueryString["q"] + "&id=" + oFunction.encryptQueryString(intID.ToString()) + "&admin=true&menu_tab=14");
        }
        protected void btnAssets_Click(Object Sender, EventArgs e)
        {
            int intRemove = 0;
            DataSet dsAssets = oServer.GetAssets(intServer);
            bool boolAssetTest = false;
            bool boolAssetQA = false;
            bool boolAssetDR = false;
            bool boolAssetProd = false;
            bool boolAssetLatest = false;
            bool boolAssetNotLatest = false;
            foreach (ListItem oItem in chkAssets.Items)
            {
                if (oItem.Value == "Test")
                    boolAssetTest = oItem.Selected;
                if (oItem.Value == "QA")
                    boolAssetQA = oItem.Selected;
                if (oItem.Value == "DR")
                    boolAssetDR = oItem.Selected;
                if (oItem.Value == "Prod")
                    boolAssetProd = oItem.Selected;
                if (oItem.Value == "Latest")
                    boolAssetLatest = oItem.Selected;
                if (oItem.Value == "NotLatest")
                    boolAssetNotLatest = oItem.Selected;
            }

            foreach (DataRow drAsset in dsAssets.Tables[0].Rows)
            {
                if (oClass.IsTestDev(Int32.Parse(drAsset["classid"].ToString())) && boolAssetTest && drAsset["deleted"].ToString() == "0")
                {
                    intRemove++;
                    int intAsset = Int32.Parse(drAsset["assetid"].ToString());
                    oServer.DeleteAsset(intServer, intAsset);
                    oAsset.AddStatus(intAsset, "", (int)AssetStatus.Available, intProfile, DateTime.Now);
                }
                else if (oClass.IsQA(Int32.Parse(drAsset["classid"].ToString())) && boolAssetQA && drAsset["deleted"].ToString() == "0")
                {
                    intRemove++;
                    int intAsset = Int32.Parse(drAsset["assetid"].ToString());
                    oServer.DeleteAsset(intServer, intAsset);
                    oAsset.AddStatus(intAsset, "", (int)AssetStatus.Available, intProfile, DateTime.Now);
                }
                else if (oClass.IsProd(Int32.Parse(drAsset["classid"].ToString())) && boolAssetProd && drAsset["deleted"].ToString() == "0")
                {
                    intRemove++;
                    int intAsset = Int32.Parse(drAsset["assetid"].ToString());
                    oServer.DeleteAsset(intServer, intAsset);
                    oAsset.AddStatus(intAsset, "", (int)AssetStatus.Available, intProfile, DateTime.Now);
                }
                else if (drAsset["dr"].ToString() == "1" && boolAssetProd && drAsset["deleted"].ToString() == "0")
                {
                    intRemove++;
                    int intAsset = Int32.Parse(drAsset["assetid"].ToString());
                    oServer.DeleteAsset(intServer, intAsset);
                    oAsset.AddStatus(intAsset, "", (int)AssetStatus.Available, intProfile, DateTime.Now);
                }
                else if (drAsset["latest"].ToString() == "1" && boolAssetLatest && drAsset["deleted"].ToString() == "0")
                {
                    intRemove++;
                    int intAsset = Int32.Parse(drAsset["assetid"].ToString());
                    oServer.DeleteAsset(intServer, intAsset);
                    oAsset.AddStatus(intAsset, "", (int)AssetStatus.Available, intProfile, DateTime.Now);
                }
                else if (drAsset["latest"].ToString() == "0" && boolAssetNotLatest && drAsset["deleted"].ToString() == "0")
                {
                    intRemove++;
                    int intAsset = Int32.Parse(drAsset["assetid"].ToString());
                    oServer.DeleteAsset(intServer, intAsset);
                    oAsset.AddStatus(intAsset, "", (int)AssetStatus.Available, intProfile, DateTime.Now);
                }
            }
            Response.Redirect(Request.Path + "?q=" + Request.QueryString["q"] + "&id=" + oFunction.encryptQueryString(intID.ToString()) + "&admin=true&menu_tab=14&result=" + oFunction.encryptQueryString("Success: Removed " + intRemove.ToString() + " assets"));
        }
        protected void btnStep_Click(Object Sender, EventArgs e)
        {
            OnDemand oOnDemand = new OnDemand(0, dsn);
            string strError = "";
            if (chkStep.Checked == true)
            {
                int intAsset = 0;
                int intStep = 0;
                DataSet ds = oServer.GetErrors(intServer);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (dr["assetid"].ToString() != "")
                        intAsset = Int32.Parse(dr["assetid"].ToString());
                    if (dr["fixed"].ToString() == "")
                        intStep = Int32.Parse(dr["step"].ToString());
                }
                if (intServer > 0)
                {
                    oServer.UpdateError(intServer, intStep, 0, 0, true, dsnAsset);
                    if (intAsset > 0)
                    {
                        string strSerial = oAsset.Get(intAsset, "serial");
                        try
                        {
                            oZeus.UpdateResults(strSerial);
                        }
                        catch { }
                    }
                    else
                        strError = "Warning: Invalid AssetID";
                }
                else
                    strError = "Error: Invalid ServerID";
            }

            if (chkStepBoot.Checked == true)
                oModel.DeleteBootGroupSteps(Int32.Parse(oServer.Get(intServer, "modelid")), intServer);

            if (chkStepVMWare.Checked == true)
            {
                VMWare oVMWare = new VMWare(0, dsn);
                oVMWare.DeleteGuest(oServer.GetName(intServer, true));
            }

            if (chkAudits.Checked == true)
                oAudit.DeleteServer(intServer, 0);
            
            if (chkAuditsMIS.Checked == true)
                oAudit.DeleteServer(intServer, 1);

            int intNewStep = Int32.Parse(txtStep.Text);
            oServer.UpdateStep(intServer, intNewStep);
            if (chkRedo.Checked == false && chkRebuild.Checked == false)
                oOnDemand.DeleteStepDoneServers(intServer, intNewStep);
            else
            {
                // Redo step...delete current step and update the other step
                oOnDemand.UpdateStepDoneServerRedo(intServer, intNewStep);
                if (chkRebuild.Checked == true)
                {
                    oServer.DeleteSwitchports(intServer);   // Reconfigure switchports
                    oServer.UpdateRebuilding(intServer, 1);
                    // Delete Audits
                    oAudit.DeleteServer(intServer, 0);
                    oAudit.DeleteServer(intServer, 1);
                    // Set installs back
                    DataSet dsInstalls = oServerName.GetComponentDetailSelected(intServer, 0);
                    foreach (DataRow drInstall in dsInstalls.Tables[0].Rows)
                        oServerName.UpdateComponentDetailSelected(intServer, Int32.Parse(drInstall["detailid"].ToString()), -2);
                }
            }
            int intStepSkipStart = Int32.Parse(txtStepSkipStart.Text);
            int intStepSkipGoto = Int32.Parse(txtStepSkipGoto.Text);
            oServer.UpdateStepSkip(intServer, intStepSkipStart, intStepSkipGoto);

            Response.Redirect(Request.Path + "?q=" + Request.QueryString["q"] + "&id=" + oFunction.encryptQueryString(intID.ToString()) + "&admin=true&menu_tab=14&result=" + oFunction.encryptQueryString((strError == "" ? "Success" : strError)));
        }
        protected void btnOutput_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(Request.Path + "?q=" + Request.QueryString["q"] + "&id=" + oFunction.encryptQueryString(intID.ToString()) + "&admin=true&menu_tab=14&output=true" + (chkDebug.Checked ? "&debug=true" : ""));
        }

        protected void btnClusterQuery_Click(object sender, EventArgs e)
        {
            QueryLuns(null);
        }

        protected void btnClusterSave_Click(object sender, EventArgs e)
        {
            List<VMwareDisk> disks = Disks();
            foreach (RepeaterItem ri in rptClusterWindows2012.Items)
            {
                Label lblUUID = (Label)ri.FindControl("lblUUID");
                Label lblLunID = (Label)ri.FindControl("lblLunID");
                DropDownList ddlMapping = (DropDownList)ri.FindControl("ddlMapping");
                int LunID = Int32.Parse(lblLunID.Text);
                VMwareDisk disk = disks.Find(o => o.lunUuid == ddlMapping.SelectedItem.Value);
                if (disk != null)
                    oStorage.AddLunDisk(LunID, disk.busNumber, disk.unitNumber, disk.lunUuid);
            }
            QueryLuns(disks);
        }

        private void QueryLuns(List<NCC.ClearView.Application.Core.VMwareDisk> disks)
        {
            if (disks == null)
                disks = Disks();
            rptClusterWindows2012.DataSource = oStorage.GetSharedMapping(intAnswer);
            rptClusterWindows2012.DataBind();
            foreach (RepeaterItem ri in rptClusterWindows2012.Items)
            {
                DropDownList ddlMapping = (DropDownList)ri.FindControl("ddlMapping");
                ddlMapping.Attributes.Add("onchange", "DropDownMappings(this);");
                ddlMapping.DataTextField = "Name";
                ddlMapping.DataValueField = "lunUuid";
                ddlMapping.DataSource = disks;
                ddlMapping.DataBind();
                ddlMapping.Items.Insert(0, new ListItem("-- NONE / RESET --", "0"));
                Label lblUUID = (Label)ri.FindControl("lblUUID");
                if (String.IsNullOrEmpty(lblUUID.Text) == false)
                    ddlMapping.SelectedValue = lblUUID.Text;
            }
            btnClusterSave.Enabled = true;
        }

        private List<VMwareDisk> Disks()
        {
            List<VMwareDisk> disks = new List<VMwareDisk>();

            VMWare oVMWare = new VMWare(intProfile, dsn);
            string name = lblName.Text;
            if (Request.IsLocal)
            {
                name = "wdclv128az";    // search for WDSVL000A in datapoint.
                intAnswer = 26042;
            }
            DataSet dsGuest = oVMWare.GetGuest(name);
            if (dsGuest.Tables[0].Rows.Count > 0)
            {
                DataRow drGuest = dsGuest.Tables[0].Rows[0];
                int intDatastore = Int32.Parse(drGuest["datastoreid"].ToString());
                int intHost = Int32.Parse(drGuest["hostid"].ToString());
                int intCluster = Int32.Parse(oVMWare.GetHost(intHost, "clusterid"));
                int intFolder = Int32.Parse(oVMWare.GetCluster(intCluster, "folderid"));
                int intDataCenter = Int32.Parse(oVMWare.GetFolder(intFolder, "datacenterid"));
                string strDataCenter = oVMWare.GetDatacenter(intDataCenter, "name");
                int intVirtualCenter = Int32.Parse(oVMWare.GetDatacenter(intDataCenter, "virtualcenterid"));
                string strVirtualCenter = oVMWare.GetVirtualCenter(intVirtualCenter, "name");
                string strVirtualCenterURL = oVMWare.GetVirtualCenter(intVirtualCenter, "url");
                int intVirtualCenterEnv = Int32.Parse(oVMWare.GetVirtualCenter(intVirtualCenter, "environment"));
                string strConnect = oVMWare.ConnectDEBUG(strVirtualCenterURL, intVirtualCenterEnv, strDataCenter);
                VimService _service = oVMWare.GetService();
                ServiceContent _sic = oVMWare.GetSic();
                try
                {
                    ManagedObjectReference oVM = oVMWare.GetVM(name);
                    if (oVM != null)
                    {
                        VirtualMachineConfigInfo vminfo = (VirtualMachineConfigInfo)oVMWare.getObjectProperty(oVM, "config");
                        VirtualDevice[] devices = vminfo.hardware.device;

                        List<VMwareController> controllers = new List<VMwareController>();

                        foreach (VirtualDevice device in devices)
                        {
                            // Try to cast to Controller
                            try
                            {
                                VirtualController controller = (VirtualController)device;
                                VMwareController Controller = new VMwareController();
                                Controller.busNumber = controller.busNumber;
                                Controller.key = controller.key;
                                controllers.Add(Controller);
                            }
                            catch { }
                            // Try to cast to Disk
                            try
                            {
                                VirtualDisk disk = (VirtualDisk)device;
                                bool boolShared = false;
                                string strLunID = null;
                                try
                                {
                                    VirtualDiskRawDiskMappingVer1BackingInfo backingShared = (VirtualDiskRawDiskMappingVer1BackingInfo)disk.backing;
                                    boolShared = true;
                                    strLunID = backingShared.lunUuid;
                                }
                                catch
                                {
                                    //try
                                    //{
                                    //    VirtualDiskFlatVer2BackingInfo backingNonShared = (VirtualDiskFlatVer2BackingInfo)disk.backing;
                                    //    boolShared = false;
                                    //    strLunID = "";
                                    //}
                                    //catch
                                    //{
                                    //}
                                }
                                if (strLunID != null)
                                {
                                    VMwareDisk Disk = new VMwareDisk();
                                    Disk.controllerKey = disk.controllerKey;
                                    Disk.label = disk.deviceInfo.label;
                                    Disk.capacityInKB = disk.capacityInKB;
                                    Disk.unitNumber = disk.unitNumber;
                                    Disk.lunUuid = strLunID;
                                    Disk.Shared = boolShared;
                                    disks.Add(Disk);
                                }
                            }
                            catch { }
                        }

                        // Match up disks with controllers for bus numbers
                        foreach (VMwareDisk disk in disks)
                        {
                            disk.capacityInKB = ((disk.capacityInKB / 1024) / 1024);    // convert KB to GB
                            foreach (VMwareController controller in controllers)
                            {
                                if (disk.controllerKey == controller.key)
                                {
                                    disk.busNumber = controller.busNumber;
                                    disk.Name = disk.capacityInKB.ToString() + " GB - SCSI(" + disk.busNumber.ToString() + ":" + disk.unitNumber.ToString() + ") " + disk.label;
                                    break;
                                }
                            }
                        }
                    }
                }
                catch { }
                finally
                {
                    if (_service != null)
                    {
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

            return disks;
        }
    }
}
