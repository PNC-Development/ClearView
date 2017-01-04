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
using System.Text;
using System.Collections.Generic;

namespace NCC.ClearView.Presentation.Web
{
    public partial class design : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
        private string dsnServiceEditor = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["ServiceEditorDSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intAssignPage = Int32.Parse(ConfigurationManager.AppSettings["AssignWorkload"]);
        protected int intResourceRequestApprove = Int32.Parse(ConfigurationManager.AppSettings["ResourceRequestApprove"]);
        protected int intViewPage = Int32.Parse(ConfigurationManager.AppSettings["ViewWorkload"]);

        protected int intDataPointAvailableService = Int32.Parse(ConfigurationManager.AppSettings["DATAPOINT_AVAILABLE_SERVICE"]);
        protected int intStorageService = Int32.Parse(ConfigurationManager.AppSettings["SERVICEID_STORAGE"]);
        protected int intBackupService = Int32.Parse(ConfigurationManager.AppSettings["SERVICEID_BACKUP"]);
        protected int intServiceCSM = Int32.Parse(ConfigurationManager.AppSettings["SERVICEID_CSM"]);
        protected int intApplicationCitrix = Int32.Parse(ConfigurationManager.AppSettings["APPLICATIONID_CITRIX"]);
        protected DataPoint oDataPoint;
        protected Users oUser;
        protected ResourceRequest oResourceRequest;
        protected Requests oRequest;
        protected Services oService;
        protected ServiceDetails oServiceDetail;
        protected ServiceRequests oServiceRequest;
        protected Applications oApplication;
        protected RequestItems oRequestItem;
        protected Functions oFunction;
        protected StatusLevels oStatusLevel;
        protected int intProfile = 0;
        protected int intApplication = 0;
        protected int intAnswer = 0;
        protected string strMenuTab1 = "";
        protected string strMenuTabForecast1 = "";
        protected string strMenuTabConfig1 = "";
        protected string strMenuTabConfigDevices1 = "";
        protected string strMenuTabConfigUsers1 = "";
        protected string strMenuTabConfigStorage1 = "";
        protected string strForecastGeneral = "";
        protected string strForecastPlatform = "";
        protected string strForecastStorageOS = "";
        protected string strForecastStorage = "";
        protected string strForecastBackup = "";
        protected string strMenuTabBackup1 = "";
        protected string strConfigApplication = "";
        protected string strConfigDevice = "";
        protected string strConfigUser = "";
        protected string strConfigStorage = "";
        protected string strConfigProduction = "";
        protected string strMenuTabExecution1 = "";
        protected string strExecution = "";
        protected string strMenuTabImplementation1 = "";
        protected string strImplementation = "";
        protected double dblStorageAmount = 0.00;
        protected ServerName oServerName;
        protected Platforms oPlatform;
        protected Locations oLocation;
        protected Classes oClass;
        protected Environments oEnvironment;
        protected MaintenanceWindow oMaintenanceWindow;
        protected Confidence oConfidence;
        protected Forecast oForecast;
        protected ModelsProperties oModelsProperties;
        protected Types oType;
        protected Storage oStorage;
        protected Mnemonic oMnemonic;
        protected Servers oServer;
        protected OnDemand oOnDemand;
        protected OnDemandTasks oOnDemandTask;
        protected Holidays oHoliday;
        protected OperatingSystems oOperatingSystem;
        protected ServicePacks oServicePack;
        protected Domains oDomain;
        protected VMWare oVMWare;
        protected ConsistencyGroups oConsistencyGroup;
        protected Asset oAsset;
        protected Projects oProject;
        protected Cluster oCluster;
        protected Design oDesign;
        protected string strPreviousStep;
        protected bool boolDebug = true;
        protected StringBuilder strBackup;

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
            oResourceRequest = new ResourceRequest(intProfile, dsn);
            oRequest = new Requests(intProfile, dsn);
            oService = new Services(intProfile, dsn);
            oServiceDetail = new ServiceDetails(intProfile, dsn);
            oServiceRequest = new ServiceRequests(intProfile, dsn);
            oApplication = new Applications(intProfile, dsn);
            oRequestItem = new RequestItems(intProfile, dsn);
            oFunction = new Functions(intProfile, dsn, intEnvironment);
            oStatusLevel = new StatusLevels();

            oServerName = new ServerName(intProfile, dsn);
            oPlatform = new Platforms(intProfile, dsn);
            oLocation = new Locations(intProfile, dsn);
            oClass = new Classes(intProfile, dsn);
            oEnvironment = new Environments(intProfile, dsn);
            oMaintenanceWindow = new MaintenanceWindow(intProfile, dsn);
            oConfidence = new Confidence(intProfile, dsn);
            oForecast = new Forecast(intProfile, dsn);
            oModelsProperties = new ModelsProperties(intProfile, dsn);
            oType = new Types(intProfile, dsn);
            oStorage = new Storage(intProfile, dsn);
            oMnemonic = new Mnemonic(intProfile, dsn);
            oServer = new Servers(intProfile, dsn);
            oOnDemand = new OnDemand(intProfile, dsn);
            oOnDemandTask = new OnDemandTasks(intProfile, dsn);
            oHoliday = new Holidays(intProfile, dsn);
            oOperatingSystem = new OperatingSystems(intProfile, dsn);
            oServicePack = new ServicePacks(intProfile, dsn);
            oDomain = new Domains(intProfile, dsn);
            oVMWare = new VMWare(intProfile, dsn);
            oConsistencyGroup = new ConsistencyGroups(intProfile, dsn);
            oAsset = new Asset(intProfile, dsnAsset);
            oProject = new Projects(intProfile, dsn);
            oCluster = new Cluster(intProfile, dsn);
            oDesign = new Design(intProfile, dsn);

            cntrlHeader.Visible = (Request.QueryString["readonly"] == null);

            if (oUser.IsAdmin(intProfile) == true || (oDataPoint.GetPagePermission(intApplication, "SERVICE") == true || intDataPointAvailableService == 1))
            {
                panAllow.Visible = true;
                if (Request.QueryString["save"] != null)
                    panSave.Visible = true;
                if (Request.QueryString["error"] != null)
                {
                    panError.Visible = true;
                }
                if (Request.QueryString["close"] != null)
                    Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "close", "<script type=\"text/javascript\">window.close();<" + "/" + "script>");
                else if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
                {
                    string strID = oFunction.decryptQueryString(Request.QueryString["id"]);
                    int intDesign = 0;
                    intAnswer = Int32.Parse(strID);
                    DataSet ds = oDataPoint.GetServiceDesign(intAnswer);
                    if (ds.Tables[0].Rows.Count == 1)
                    {
                        // Load General Information
                        string strHeader = intAnswer.ToString();
                        lblHeader.Text = "&quot;" + strHeader + "&quot;";
                        Master.Page.Title = "DataPoint | Design ID #" + strHeader;
                        lblHeaderSub.Text = "Provides all the information about a design...";
                        int intMenuTab = 0;
                        if (Request.QueryString["menu_tab"] != null && Request.QueryString["menu_tab"] != "")
                            intMenuTab = Int32.Parse(Request.QueryString["menu_tab"]);
                        Tab oTab = new Tab(hdnTab.ClientID, intMenuTab, "divMenu1", true, false);
                        oTab.AddTab("Forecast Details", "");
                        oTab.AddTab("Configuration Details", "");
                        oTab.AddTab("Provisioning Status", "");
                        //oTab.AddTab("Implementation Details", "");

                        if (oDesign.Get(intAnswer).Tables[0].Rows.Count > 0)
                            intDesign = intAnswer;
                        else if (oDesign.GetAnswer(intAnswer).Tables[0].Rows.Count > 0)
                            Int32.TryParse(oDesign.GetAnswer(intAnswer, "id"), out intDesign);

                        if (intDesign > 0)
                        {
                            if (oUser.IsAdmin(intProfile))
                            {
                                oTab.AddTab("Administration", "");
                                Control oControl = (Control)LoadControl("/controls/fore/fore_approve_cfi.ascx");
                                phAdministration.Controls.Add(oControl);
                            }
                        }

                        if (!IsPostBack)
                        {
                            bool boolDeleted = (ds.Tables[0].Rows[0]["deleted"].ToString() != "0");
                            panDeleted.Visible = boolDeleted;
                            int intForecast = Int32.Parse(ds.Tables[0].Rows[0]["forecastid"].ToString());
                            if (ds.Tables[0].Rows[0]["resiliency"].ToString() == "1")
                                imgBIR.ImageUrl = "/images/check.gif";
                            int intRequestID = oForecast.GetRequestID(intAnswer, true);
                            int intProject = oRequest.GetProjectNumber(intRequestID);
                            int intUser = Int32.Parse(ds.Tables[0].Rows[0]["userid"].ToString());
                            int intClass = Int32.Parse(ds.Tables[0].Rows[0]["classid"].ToString());
                            int intEnv = Int32.Parse(ds.Tables[0].Rows[0]["environmentid"].ToString());
                            int intAddress = Int32.Parse(ds.Tables[0].Rows[0]["addressid"].ToString());
                            int intPlatform = 0;
                            Int32.TryParse(ds.Tables[0].Rows[0]["platformid"].ToString(), out intPlatform);
                            int intMaintenance = 0;
                            Int32.TryParse(ds.Tables[0].Rows[0]["maintenanceid"].ToString(), out intPlatform);
                            int intApp = Int32.Parse(ds.Tables[0].Rows[0]["applicationid"].ToString());
                            int intSubApp = Int32.Parse(ds.Tables[0].Rows[0]["subapplicationid"].ToString());
                            int intModel = Int32.Parse(ds.Tables[0].Rows[0]["modelid"].ToString());
                            int intExecutedBy = 0;
                            Int32.TryParse(ds.Tables[0].Rows[0]["executed_by"].ToString(), out intExecutedBy);
                            if (intModel == 0)
                                intModel = oForecast.GetModelAsset(intAnswer);
                            if (intModel == 0)
                                intModel = oForecast.GetModel(intAnswer);
                            int intType = 0;
                            if (intModel > 0)
                                intType = oModelsProperties.GetType(intModel);
                            string strConfidence = ds.Tables[0].Rows[0]["confidence"].ToString();
                            int intQuantity = Int32.Parse(ds.Tables[0].Rows[0]["quantity"].ToString());

                            #region FORECAST
                            Tab oTabForecast = new Tab("", 0, "divMenuForecast1", true, false);
                            // FORECAST  /  General
                            oTabForecast.AddTab("General", "");
                            string strProjectName = oProject.Get(intProject, "name");
                            if (strProjectName != "")
                                strProjectName = "<a href=\"javascript:void(0);\" onclick=\"return OpenNewWindowMenu('/datapoint/projects/datapoint_projects.aspx?id=" + oFunction.encryptQueryString(intProject.ToString()) + "', '800', '600');\" class=\"lookup\">" + strProjectName + "</a>";
                            string strDesignID = intDesign.ToString();
                            if (strDesignID == "" || strDesignID == "0")
                                strDesignID = oDesign.GetAnswer(intAnswer, "id");
                            if (strDesignID != "")
                            {
                                if (intAnswer > 0)
                                    lblAnswer.Text = intAnswer.ToString();
                                    //strForecastGeneral += "<tr><td>Legacy Design ID:</td><td><b>" + intAnswer.ToString() + "</b></td></tr>";
                                lblHeader.Text = "&quot;" + strDesignID + "&quot;";
                                Master.Page.Title = "DataPoint | Design ID #" + strDesignID;
                                btnDesign.Attributes.Add("onclick", "return OpenNewWindowMenu('/frame/design.aspx?id=" + strDesignID + "', '800', '600');");
                            }
                            else
                                btnDesign.Visible = false;
                            strForecastGeneral += "<tr><td>Project Name:</td><td>" + strProjectName + "</td></tr>";
                            strForecastGeneral += "<tr><td>Project Number:</td><td>" + oProject.Get(intProject, "number") + "</td></tr>";
                            strForecastGeneral += "<tr><td>PNC Project Number:</td><td>" + oForecast.Get(intForecast, "pnc_project") + "</td></tr>";
                            strForecastGeneral += "<tr><td>Executed By:</td><td>" + oServer.GetExecutionUser(intExecutedBy) + "</td></tr>";
                            strForecastGeneral += "<tr><td>Executed On:</td><td>" + ds.Tables[0].Rows[0]["executed"].ToString() + "</td></tr>";
                            strForecastGeneral += "<tr><td>Requested By:</td><td>" + oServer.GetExecutionUser(intUser) + "</td></tr>";
                            strForecastGeneral += "<tr><td>Last Updated:</td><td>" + ds.Tables[0].Rows[0]["modified"].ToString() + "</td></tr>";
                            strForecastGeneral += "<tr><td>Nickname:</td><td>" + ds.Tables[0].Rows[0]["name"].ToString() + "</td></tr>";
                            if (ds.Tables[0].Rows[0]["override"].ToString() == "1" || ds.Tables[0].Rows[0]["override"].ToString() == "-1" || ds.Tables[0].Rows[0]["override"].ToString() == "-10")
                            {
                                strForecastGeneral += "<tr><td>Override Selection Matrix:</td><td class=\"note\">Yes</td></tr>";
                                int intOverrideBy = Int32.Parse(ds.Tables[0].Rows[0]["overrideby"].ToString());
                                if (ds.Tables[0].Rows[0]["override"].ToString() == "-1")
                                    strForecastGeneral += "<tr><td>Override Approval:</td><td class=\"hold\">Pending</td></tr>";
                                else if (ds.Tables[0].Rows[0]["override"].ToString() == "-10")
                                {
                                    strForecastGeneral += "<tr><td>Override Approval:</td><td class=\"note\">Denied</td></tr>";
                                    strForecastGeneral += "<tr><td>Override Comments:</td><td class=\"footer\">" + ds.Tables[0].Rows[0]["comments"].ToString() + "</td></tr>";
                                    strForecastGeneral += "<tr><td>Override Denied By:</td><td class=\"note\">" + oServer.GetExecutionUser(intOverrideBy) + "</td></tr>";
                                }
                                else
                                {
                                    strForecastGeneral += "<tr><td>Override Approval:</td><td class=\"approved\">Approved</td></tr>";
                                    strForecastGeneral += "<tr><td>Override Comments:</td><td class=\"footer\">" + ds.Tables[0].Rows[0]["comments"].ToString() + "</td></tr>";
                                    strForecastGeneral += "<tr><td>Override Approved By:</td><td class=\"note\">" + oServer.GetExecutionUser(intOverrideBy) + "</td></tr>";
                                }
                                if (ds.Tables[0].Rows[0]["breakfix"].ToString() == "1")
                                {
                                    strForecastGeneral += "<tr><td>Is this related to a production break-fix issue:</td><td class=\"required\">Yes</td></tr>";
                                    strForecastGeneral += "<tr><td>Change Control #:</td><td class=\"note\">" + ds.Tables[0].Rows[0]["change"].ToString() + "</td></tr>";
                                    int intName = Int32.Parse(ds.Tables[0].Rows[0]["nameid"].ToString());
                                    if (oClass.Get(intClass, "pnc") == "1")
                                        strForecastGeneral += "<tr><td>Device Name:</td><td class=\"note\">" + oServerName.GetNameFactory(intName, 0) + "</td></tr>";
                                    else
                                        strForecastGeneral += "<tr><td>Device Name:</td><td class=\"note\">" + oServerName.GetName(intName, 0) + "</td></tr>";
                                }
                                else
                                    strForecastGeneral += "<tr><td>Is this related to a production break-fix issue:</td><td>No</td></tr>";
                            }
                            else
                                strForecastGeneral += "<tr><td>Override Selection Matrix:</td><td>No</td></tr>";
                            if (ds.Tables[0].Rows[0]["storage_override"].ToString() == "1")
                            {
                                strForecastGeneral += "<tr><td>Override Storage:</td><td class=\"note\">Yes</td></tr>";
                                int intOverrideStorageBy = Int32.Parse(ds.Tables[0].Rows[0]["storage_overrideby"].ToString());
                                strForecastGeneral += "<tr><td>Override Storage By:</td><td class=\"note\">" + oServer.GetExecutionUser(intOverrideStorageBy) + "</td></tr>";
                            }
                            else
                                strForecastGeneral += "<tr><td>Override Storage:</td><td>No</td></tr>";
                            strForecastGeneral += "<tr><td>Model:</td><td>" + oModelsProperties.Get(intModel, "name") + "</td></tr>";
                            strForecastGeneral += "<tr><td>Location:</td><td>" + oLocation.GetFull(intAddress) + "</td></tr>";
                            strForecastGeneral += "<tr><td>Class:</td><td>" + oClass.Get(intClass, "name") + "</td></tr>";
                            if (oClass.IsProd(intClass))
                                strForecastGeneral += "<tr><td>Will this device go through TEST first:</td><td>" + (ds.Tables[0].Rows[0]["test"].ToString() == "1" ? "Yes" : "No") + "</td></tr>";
                            strForecastGeneral += "<tr><td>Environment:</td><td>" + oEnvironment.Get(intEnv, "name") + "</td></tr>";
                            strForecastGeneral += "<tr><td>Maintenance Window:</td><td>" + (intMaintenance == 0 ? "N / A" : oMaintenanceWindow.Get(intMaintenance, "name")) + "</td></tr>";
                            strForecastGeneral += "<tr><td>Server Type:</td><td>" + (intApp == 0 ? "N / A" : oServerName.GetApplication(intApp, "name")) + "</td></tr>";
                            strForecastGeneral += "<tr><td>Server Role:</td><td>" + (intSubApp == 0 ? "N / A" : oServerName.GetSubApplication(intSubApp, "name")) + "</td></tr>";
                            strForecastGeneral += "<tr><td>Quantity:</td><td>" + intQuantity.ToString() + "</td></tr>";
                            if (ds.Tables[0].Rows[0]["implementation"].ToString() == "")
                                strForecastGeneral += "<tr><td>Commitment Date:</td><td>" + "---" + "</td></tr>";
                            else
                                strForecastGeneral += "<tr><td>Commitment Date:</td><td>" + DateTime.Parse(ds.Tables[0].Rows[0]["implementation"].ToString()).ToShortDateString() + "</td></tr>";
                            strForecastGeneral += "<tr><td>Confidence:</td><td>" + strConfidence + "</td></tr>";
                            strForecastGeneral += "<tr><td>Recovery Quantity:</td><td>" + ds.Tables[0].Rows[0]["recovery_number"].ToString() + "</td></tr>";
                            strForecastGeneral = oServer.GetExecutionTable(strForecastGeneral, false, 0);

                            // FORECAST  /  Platform
                            oTabForecast.AddTab("Platform", "");
                            if (ds.Tables[0].Rows[0]["override"].ToString() == "1" || ds.Tables[0].Rows[0]["override"].ToString() == "-1" || ds.Tables[0].Rows[0]["override"].ToString() == "-10")
                            {
                                strForecastPlatform += "<tr><td colspan=\"3\" width=\"100%\" class=\"bold\"><img src=\"/images/bigAlert.gif\" border=\"0\" align=\"absmiddle\" /> Selection Matrix Override!</td></tr>";
                                strForecastPlatform += "<tr><td><img src=\"/images/spacer.gif\" border=\"0\" width=\"25\" height=\"1\"/></td><td><img src=\"/images/down_right.gif\" border=\"0\" align=\"absmiddle\"/></td><td width=\"100%\">Type: " + oType.Get(intType, "name") + "</td></tr>";
                                strForecastPlatform += "<tr><td><img src=\"/images/spacer.gif\" border=\"0\" width=\"25\" height=\"1\"/></td><td><img src=\"/images/down_right.gif\" border=\"0\" align=\"absmiddle\"/></td><td width=\"100%\">Model: " + oModelsProperties.Get(intModel, "name") + "</td></tr>";
                                strForecastPlatform += "<tr><td colspan=\"3\">&nbsp;</td></tr>";
                            }
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
                                    strForecastPlatform += "<tr><td colspan=\"3\" width=\"100%\">" + drQuestion["question"].ToString() + "</td></tr>";
                                    strForecastPlatform += "<tr><td><img src=\"/images/spacer.gif\" border=\"0\" width=\"25\" height=\"1\"/></td><td><img src=\"/images/comment.gif\" border=\"0\" align=\"absmiddle\"/></td><td width=\"100%\">" + strResponsePDF + "</td></tr>";
                                }
                            }
                            strForecastPlatform = oServer.GetExecutionTable(strForecastPlatform, false, 0);

                            // FORECAST  /  Storage
                            oTabForecast.AddTab("Storage", "");
                            bool boolProduction = (oClass.IsProd(intClass));
                            bool boolQA = (oClass.IsQA(intClass));
                            bool boolNone = (ds.Tables[0].Rows[0]["storage"].ToString() == "-2");
                            bool boolRequired = oForecast.IsHACluster(intAnswer);
                            bool boolNoReplication = oForecast.IsDROver48(intAnswer, false);
                            if (ds.Tables[0].Rows[0]["storage"].ToString() == "1")
                            {
                                // OS Volumes
                                DataSet dsStorageOS = oForecast.GetStorageOS(intAnswer);
                                if (dsStorageOS.Tables[0].Rows.Count > 0)
                                {
                                    panForecastStorageOS.Visible = true;
                                    strForecastStorageOS += "<tr bgcolor=\"#EEEEEE\">";
                                    strForecastStorageOS += "<td class=\"bold\">Class</td>";
                                    strForecastStorageOS += "<td class=\"bold\">Performance</td>";
                                    strForecastStorageOS += "<td class=\"bold\">Amount</td>";
                                    strForecastStorageOS += "<td class=\"bold\">Availability</td>";
                                    strForecastStorageOS += "<td class=\"bold\">Replication</td>";
                                    strForecastStorageOS += "<td class=\"bold\">Amount Replicated</td>";
                                    strForecastStorageOS += "<td class=\"bold\">HA</td>";
                                    strForecastStorageOS += "<td class=\"bold\">Amount HA</td>";
                                    strForecastStorageOS += "</tr>";
                                    if (dsStorageOS.Tables[0].Rows[0]["high"].ToString() == "1")
                                    {
                                        strForecastStorageOS += AddForecastStorage("Production", "High", dsStorageOS.Tables[0].Rows[0]["high_total"].ToString(), dsStorageOS.Tables[0].Rows[0]["high_level"].ToString(), dsStorageOS.Tables[0].Rows[0]["high_replicated"].ToString(), dsStorageOS.Tables[0].Rows[0]["high_ha"].ToString());
                                        strForecastStorageOS += AddForecastStorage("QA", "High", dsStorageOS.Tables[0].Rows[0]["high_qa"].ToString(), "", "", "");
                                        strForecastStorageOS += AddForecastStorage("Test", "High", dsStorageOS.Tables[0].Rows[0]["high_test"].ToString(), "", "", "");
                                    }
                                    if (dsStorageOS.Tables[0].Rows[0]["standard"].ToString() == "1")
                                    {
                                        strForecastStorageOS += AddForecastStorage("Production", "Standard", dsStorageOS.Tables[0].Rows[0]["standard_total"].ToString(), dsStorageOS.Tables[0].Rows[0]["standard_level"].ToString(), dsStorageOS.Tables[0].Rows[0]["standard_replicated"].ToString(), dsStorageOS.Tables[0].Rows[0]["standard_ha"].ToString());
                                        strForecastStorageOS += AddForecastStorage("QA", "Standard", dsStorageOS.Tables[0].Rows[0]["standard_qa"].ToString(), "", "", "");
                                        strForecastStorageOS += AddForecastStorage("Test", "Standard", dsStorageOS.Tables[0].Rows[0]["standard_test"].ToString(), "", "", "");
                                    }
                                    if (dsStorageOS.Tables[0].Rows[0]["low"].ToString() == "1")
                                    {
                                        strForecastStorageOS += AddForecastStorage("Production", "Low", dsStorageOS.Tables[0].Rows[0]["low_total"].ToString(), dsStorageOS.Tables[0].Rows[0]["low_level"].ToString(), dsStorageOS.Tables[0].Rows[0]["low_replicated"].ToString(), dsStorageOS.Tables[0].Rows[0]["low_ha"].ToString());
                                        strForecastStorageOS += AddForecastStorage("QA", "Low", dsStorageOS.Tables[0].Rows[0]["low_qa"].ToString(), "", "", "");
                                        strForecastStorageOS += AddForecastStorage("Test", "Low", dsStorageOS.Tables[0].Rows[0]["low_test"].ToString(), "", "", "");
                                    }
                                }
                                // Application / Data Volumes
                                DataSet dsStorage = oForecast.GetStorage(intAnswer);
                                if (dsStorage.Tables[0].Rows.Count > 0)
                                {
                                    strForecastStorage += "<tr bgcolor=\"#EEEEEE\">";
                                    strForecastStorage += "<td class=\"bold\">Class</td>";
                                    strForecastStorage += "<td class=\"bold\">Performance</td>";
                                    strForecastStorage += "<td class=\"bold\">Amount</td>";
                                    strForecastStorage += "<td class=\"bold\">Availability</td>";
                                    strForecastStorage += "<td class=\"bold\">Replication</td>";
                                    strForecastStorage += "<td class=\"bold\">Amount Replicated</td>";
                                    strForecastStorage += "<td class=\"bold\">HA</td>";
                                    strForecastStorage += "<td class=\"bold\">Amount HA</td>";
                                    strForecastStorage += "</tr>";
                                    if (dsStorage.Tables[0].Rows[0]["high"].ToString() == "1")
                                    {
                                        strForecastStorage += AddForecastStorage("Production", "High", dsStorage.Tables[0].Rows[0]["high_total"].ToString(), dsStorage.Tables[0].Rows[0]["high_level"].ToString(), dsStorage.Tables[0].Rows[0]["high_replicated"].ToString(), dsStorage.Tables[0].Rows[0]["high_ha"].ToString());
                                        strForecastStorage += AddForecastStorage("QA", "High", dsStorage.Tables[0].Rows[0]["high_qa"].ToString(), "", "", "");
                                        strForecastStorage += AddForecastStorage("Test", "High", dsStorage.Tables[0].Rows[0]["high_test"].ToString(), "", "", "");
                                    }
                                    if (dsStorage.Tables[0].Rows[0]["standard"].ToString() == "1")
                                    {
                                        strForecastStorage += AddForecastStorage("Production", "Standard", dsStorage.Tables[0].Rows[0]["standard_total"].ToString(), dsStorage.Tables[0].Rows[0]["standard_level"].ToString(), dsStorage.Tables[0].Rows[0]["standard_replicated"].ToString(), dsStorage.Tables[0].Rows[0]["standard_ha"].ToString());
                                        strForecastStorage += AddForecastStorage("QA", "Standard", dsStorage.Tables[0].Rows[0]["standard_qa"].ToString(), "", "", "");
                                        strForecastStorage += AddForecastStorage("Test", "Standard", dsStorage.Tables[0].Rows[0]["standard_test"].ToString(), "", "", "");
                                    }
                                    if (dsStorage.Tables[0].Rows[0]["low"].ToString() == "1")
                                    {
                                        strForecastStorage += AddForecastStorage("Production", "Low", dsStorage.Tables[0].Rows[0]["low_total"].ToString(), dsStorage.Tables[0].Rows[0]["low_level"].ToString(), dsStorage.Tables[0].Rows[0]["low_replicated"].ToString(), dsStorage.Tables[0].Rows[0]["low_ha"].ToString());
                                        strForecastStorage += AddForecastStorage("QA", "Low", dsStorage.Tables[0].Rows[0]["low_qa"].ToString(), "", "", "");
                                        strForecastStorage += AddForecastStorage("Test", "Low", dsStorage.Tables[0].Rows[0]["low_test"].ToString(), "", "", "");
                                    }
                                }
                            }
                            else
                                strForecastStorage += "<tr><td class=\"bold\"><img src=\"/images/bigAlert.gif\" border=\"0\" align=\"absmiddle\" /> There was no storage requested!</td></tr>";
                            strForecastStorageOS = "<table cellpadding=\"8\" cellspacing=\"0\" border=\"0\" style=\"border:solid 1px #CCCCCC\">" + strForecastStorageOS + "</table>";
                            strForecastStorage = "<table cellpadding=\"8\" cellspacing=\"0\" border=\"0\" style=\"border:solid 1px #CCCCCC\">" + strForecastStorage + "</table>";


                            // FORECAST  /  Backup
                            oTabForecast.AddTab("Backup", "");
                            DataSet dsBackup = oForecast.GetBackup(intAnswer);
                            if (dsBackup.Tables[0].Rows.Count > 0)
                            {
                                Tab oTabBackup = new Tab("", 0, "divMenuBackup1", true, false);
                                oTabBackup.AddTab("Backup Information", "");
                                oTabBackup.AddTab("Backup Inclusions", "");
                                oTabBackup.AddTab("Backup Exclusions", "");
                                oTabBackup.AddTab("Archive Requirements", "");
                                oTabBackup.AddTab("Additional Configuration", "");
                                strMenuTabBackup1 = oTabBackup.GetTabs();

                                if (dsBackup.Tables[0].Rows[0]["recoveryid"].ToString() != "")
                                    strForecastBackup += "<tr><td nowrap>Recovery Location:</td><td width=\"100%\">" + oLocation.GetFull(Int32.Parse(dsBackup.Tables[0].Rows[0]["recoveryid"].ToString())) + "</td></tr>";
                                if (dsBackup.Tables[0].Rows[0]["daily"].ToString() == "1")
                                    strForecastBackup += "<tr><td nowrap>Timing/Frequency of Backups:</td><td width=\"100%\">" + "Daily" + "</td></tr>";
                                else if (dsBackup.Tables[0].Rows[0]["weekly"].ToString() == "1")
                                    strForecastBackup += "<tr><td nowrap>Timing/Frequency of Backups:</td><td width=\"100%\">" + "Weekly" + "</td></tr>";
                                else if (dsBackup.Tables[0].Rows[0]["monthly"].ToString() == "1")
                                    strForecastBackup += "<tr><td nowrap>Timing/Frequency of Backups:</td><td width=\"100%\">" + "Monthly" + "</td></tr>";
                                if (dsBackup.Tables[0].Rows[0]["time"].ToString() == "1")
                                    strForecastBackup += "<tr><td nowrap>Start Time:</td><td width=\"100%\">" + dsBackup.Tables[0].Rows[0]["time_hour"].ToString() + " " + dsBackup.Tables[0].Rows[0]["time_switch"].ToString() + "</td></tr>";
                                else
                                    strForecastBackup += "<tr><td nowrap>Start Time:</td><td width=\"100%\">" + "Don't Care" + "</td></tr>";

                                strForecastBackup += "<tr><td nowrap>Total Combined Disk Capacity (GB):</td><td width=\"100%\">" + dblStorageAmount.ToString("0") + " GB" + "</td></tr>";
                                strForecastBackup += "<tr><td nowrap>Current Combined Disk Utilized (GB):</td><td width=\"100%\">" + "5 GB" + "</td></tr>";
                                strForecastBackup += "<tr><td nowrap>Average Size of One Typical Data File:</td><td width=\"100%\">" + dsBackup.Tables[0].Rows[0]["average_one"].ToString() + " GB" + "</td></tr>";
                                if (dsBackup.Tables[0].Rows[0]["documentation"].ToString() == "")
                                    strForecastBackup += "<tr><td nowrap>Production Turnover Documentation:</td><td width=\"100%\">" + "Not Specified" + "</td></tr>";
                                else
                                    strForecastBackup += "<tr><td nowrap>Production Turnover Documentation:</td><td width=\"100%\">" + dsBackup.Tables[0].Rows[0]["documentation"].ToString() + "</td></tr>";

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
                                DataSet dsDesign = oDesign.GetAnswer(intAnswer);
                                if (dsDesign.Tables[0].Rows.Count > 0)
                                {
                                    panCFI.Visible = true;
                                    int intAnswerCFI = Int32.Parse(dsDesign.Tables[0].Rows[0]["id"].ToString());
                                    string strFrequency = oDesign.Get(intAnswerCFI, "backup_frequency");
                                    lblFrequency.Text = (strFrequency == "D" ? "Daily" : (strFrequency == "W" ? "Weekly" : (strFrequency == "M" ? "Monthly" : "Backup Not Requested")));
                                    strBackup = new StringBuilder();
                                    dsBackup = oDesign.GetBackup(intAnswerCFI);
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

                                    rptExclusionsCFI.DataSource = oDesign.GetExclusions(intAnswerCFI);
                                    rptExclusionsCFI.DataBind();
                                    if (rptExclusionsCFI.Items.Count == 0)
                                        lblExclusion.Visible = true;
                                }
                            }

                            strMenuTabForecast1 = oTabForecast.GetTabs();
                            #endregion


                            #region CONFIGURATION
                            Tab oTabConfig = new Tab("", 0, "divMenuConfig1", true, false);
                            // CONFIGURATION  /  Application
                            string strMnemonic = "";
                            oTabConfig.AddTab("Application", "");
                            strConfigApplication += "<tr><td>Application Name:</td><td>" + ds.Tables[0].Rows[0]["appname"].ToString() + "</td><td></td></tr>";
                            if (oClass.Get(intClass, "pnc") == "1")
                            {
                                int intMnemonic = Int32.Parse(ds.Tables[0].Rows[0]["mnemonicid"].ToString());
                                strConfigApplication += "<tr>";
                                strConfigApplication += "<td>Mnemonic:</td><td>" + oMnemonic.Get(intMnemonic, "factory_code") + "</td>";
                                strConfigApplication += "<td><img src=\"/images/more_information.gif\" border=\"0\"/></td>";
                                strConfigApplication += "</tr>";
                                strMnemonic += "<tr><td colspan=\"2\" bgcolor=\"#EEEEEE\"><b>Additional Mnemonic Information</b></td></tr>";
                                DataSet dsMnemonic = oMnemonic.Get(intMnemonic);
                                if (dsMnemonic.Tables[0].Rows.Count > 0)
                                {
                                    DataRow drMnemonic = dsMnemonic.Tables[0].Rows[0];
                                    strMnemonic += "<tr><td>Mnemonic:</td><td>" + drMnemonic["factory_code"].ToString() + "</td></tr>";
                                    strMnemonic += "<tr><td>Application Name:</td><td>" + drMnemonic["name"].ToString() + "</td></tr>";
                                    strMnemonic += "<tr><td>Status:</td><td>" + drMnemonic["Status"].ToString() + "</td></tr>";
                                    strMnemonic += "<tr><td>Resiliency Rating:</td><td>" + drMnemonic["ResRating"].ToString() + "</td></tr>";
                                    strMnemonic += "<tr><td>DR Rating:</td><td>" + drMnemonic["DRRating"].ToString() + "</td></tr>";
                                    strMnemonic += "<tr><td>Infrastructure Device:</td><td>" + drMnemonic["Infrastructure"].ToString() + "</td></tr>";
                                    strMnemonic += "<tr><td>DR Criticality Factor:</td><td>" + drMnemonic["CriticalityFactor"].ToString() + "</td></tr>";
                                    strMnemonic += "<tr><td>Platform:</td><td>" + drMnemonic["Platform"].ToString() + "</td></tr>";
                                    strMnemonic += "<tr><td>CICS:</td><td>" + drMnemonic["CICS"].ToString() + "</td></tr>";
                                    strMnemonic += "<tr><td>Project Manager Name:</td><td>" + drMnemonic["PMName"].ToString() + "</td></tr>";
                                    strMnemonic += "<tr><td>Chief Information Officer (CIO):</td><td>" + drMnemonic["CIO"].ToString() + "</td></tr>";
                                    strMnemonic += "<tr><td>Application Owner:</td><td>" + drMnemonic["AppOwner"].ToString() + "</td></tr>";
                                    strMnemonic += "<tr><td>Application Line Of Business (LOB) Name:</td><td>" + drMnemonic["AppLOBName"].ToString() + "</td></tr>";
                                    strMnemonic += "<tr><td>Segment:</td><td>" + drMnemonic["Segment1"].ToString() + "</td></tr>";
                                    strMnemonic += "<tr><td>Risk Manager:</td><td>" + drMnemonic["RiskManager"].ToString() + "</td></tr>";
                                    strMnemonic += "<tr><td>Application Rating:</td><td>" + drMnemonic["AppRating"].ToString() + "</td></tr>";
                                    /*
                                    string strMnemonicCode = drMnemonic["factory_code"].ToString();
                                    List<string> strMnemonicFeed = oMnemonic.GetFeed(strMnemonicCode);
                                    strMnemonic += "<tr><td>Mnemonic:</td><td>" + strMnemonic + "</td></tr>";
                                    strMnemonic += "<tr><td>Application Name:</td><td>" + drMnemonic["name"].ToString() + "</td></tr>";
                                    strMnemonic += "<tr><td>Status:</td><td>" + oMnemonic.GetFeedValue(strMnemonicFeed, MnemonicFeed.Status, drMnemonic["Status"].ToString()) + "</td></tr>";
                                    strMnemonic += "<tr><td>Resiliency Rating:</td><td>" + oMnemonic.GetFeedValue(strMnemonicFeed, MnemonicFeed.ResRating, drMnemonic["ResRating"].ToString()) + "</td></tr>";
                                    strMnemonic += "<tr><td>DR Rating:</td><td>" + drMnemonic["DRRating"].ToString() + "</td></tr>";
                                    strMnemonic += "<tr><td>Infrastructure Device:</td><td>" + drMnemonic["Infrastructure"].ToString() + "</td></tr>";
                                    strMnemonic += "<tr><td>DR Criticality Factor:</td><td>" + drMnemonic["CriticalityFactor"].ToString() + "</td></tr>";
                                    strMnemonic += "<tr><td>Platform:</td><td>" + drMnemonic["Platform"].ToString() + "</td></tr>";
                                    strMnemonic += "<tr><td>CICS:</td><td>" + drMnemonic["CICS"].ToString() + "</td></tr>";
                                    strMnemonic += "<tr><td>Project Manager Name:</td><td>" + oMnemonic.GetFeedUser(strMnemonicFeed, MnemonicFeed.PM, drMnemonic["PMName"].ToString()) + "</td></tr>";
                                    strMnemonic += "<tr><td>Chief Information Officer (CIO):</td><td>" + oMnemonic.GetFeedUser(strMnemonicFeed, MnemonicFeed.CIO, drMnemonic["CIO"].ToString()) + "</td></tr>";
                                    strMnemonic += "<tr><td>Application Owner:</td><td>" + oMnemonic.GetFeedUser(strMnemonicFeed, MnemonicFeed.AppOwner, drMnemonic["AppOwner"].ToString()) + "</td></tr>";
                                    strMnemonic += "<tr><td>Application Line Of Business (LOB) Name:</td><td>" + drMnemonic["AppLOBName"].ToString() + "</td></tr>";
                                    strMnemonic += "<tr><td>Segment:</td><td>" + drMnemonic["Segment1"].ToString() + "</td></tr>";
                                    strMnemonic += "<tr><td>Risk Manager:</td><td>" + drMnemonic["RiskManager"].ToString() + "</td></tr>";
                                    strMnemonic += "<tr><td>Application Rating:</td><td>" + oMnemonic.GetFeedValue(strMnemonicFeed, MnemonicFeed.AppRating, drMnemonic["AppRating"].ToString()) + "</td></tr>";
                                    */
                                }
                                else
                                    strMnemonic += "<tr><td colspan=\"2\">No additional information...</td></tr>";
                                strMnemonic = oServer.GetExecutionTable(strMnemonic, true, 0);
                            }
                            else
                                strConfigApplication += "<tr><td>Application Code:</td><td>" + ds.Tables[0].Rows[0]["appcode"].ToString() + "</td><td></td></tr>";
                            int intApplicationClient = 0;
                            if (ds.Tables[0].Rows[0]["appcontact"].ToString() != "")
                                intApplicationClient = Int32.Parse(ds.Tables[0].Rows[0]["appcontact"].ToString());
                            strConfigApplication += "<tr><td>Departmental Manager:</td><td>" + oServer.GetExecutionUser(intApplicationClient) + "</td><td></td></tr>";
                            int intApplicationPrimary = 0;
                            if (ds.Tables[0].Rows[0]["admin1"].ToString() != "")
                                intApplicationPrimary = Int32.Parse(ds.Tables[0].Rows[0]["admin1"].ToString());
                            strConfigApplication += "<tr><td>Application Technical Lead:</td><td>" + oServer.GetExecutionUser(intApplicationPrimary) + "</td><td></td></tr>";
                            int intApplicationAdministrative = 0;
                            if (ds.Tables[0].Rows[0]["admin2"].ToString() != "")
                                intApplicationAdministrative = Int32.Parse(ds.Tables[0].Rows[0]["admin2"].ToString());
                            strConfigApplication += "<tr><td>Administrative Contact:</td><td>" + oServer.GetExecutionUser(intApplicationAdministrative) + "</td><td></td></tr>";
                            int intApplicationOwner = 0;
                            if (ds.Tables[0].Rows[0]["appowner"].ToString() != "")
                                intApplicationOwner = Int32.Parse(ds.Tables[0].Rows[0]["appowner"].ToString());
                            strConfigApplication += "<tr><td>Application Owner:</td><td>" + oServer.GetExecutionUser(intApplicationOwner) + "</td><td></td></tr>";
                            int intApplicationEngineer = 0;
                            if (ds.Tables[0].Rows[0]["networkengineer"].ToString() != "")
                                intApplicationEngineer = Int32.Parse(ds.Tables[0].Rows[0]["networkengineer"].ToString());
                            strConfigApplication += "<tr><td>Network Engineer:</td><td>" + oServer.GetExecutionUser(intApplicationEngineer) + "</td><td></td></tr>";
                            strConfigApplication += "<tr><td>DR Criticality:</td><td>" + (oClass.IsProd(intClass) && ds.Tables[0].Rows[0]["dr_criticality"].ToString() != "0" ? (ds.Tables[0].Rows[0]["dr_criticality"].ToString() == "1" ? "1 - High" : "2 - Low") : "---") + "</td><td></td></tr>";
                            strConfigApplication += "<tr><td>Change Control #:</td><td>" + (ds.Tables[0].Rows[0]["change"].ToString() != "" ? ds.Tables[0].Rows[0]["change"].ToString() : "---") + "</td><td></td></tr>";
                            strConfigApplication += "<tr><td>Production Go Live Date:</td><td>" + (oClass.IsProd(intClass) && ds.Tables[0].Rows[0]["production"].ToString() != "" ? DateTime.Parse(ds.Tables[0].Rows[0]["production"].ToString()).ToShortDateString() : "---") + "</td><td></td></tr>";
                            strConfigApplication = oServer.GetExecutionTable(strConfigApplication, false, 0);
                            if (oClass.Get(intClass, "pnc") == "1")
                            {
                                string strMnemonicTable = "<table cellpadding=\"2\" cellspacing=\"2\" border=\"0\">";
                                strMnemonicTable += "<tr><td valign=\"top\">" + strConfigApplication + "<td><td valign=\"top\">" + strMnemonic + "</td></tr>";
                                strMnemonicTable += "</table>";
                                strConfigApplication = strMnemonicTable;
                            }


                            DataSet dsServer = oServer.GetAnswer(intAnswer);

                            // CONFIGURATION  /  Device(s)
                            oTabConfig.AddTab("Device(s)", "");
                            Tab oTabConfigDevices = new Tab("", 0, "divMenuConfigDevices1", true, false);
                            for (int ii = 1; ii <= intQuantity; ii++)
                            {
                                string strConfigDeviceServer = "";
                                string strName = "Device #" + ii.ToString();
                                try
                                {
                                    DataRow drServer = dsServer.Tables[0].Rows[ii - 1];
                                    int intServer = Int32.Parse(drServer["id"].ToString());
                                    int intName = Int32.Parse(drServer["nameid"].ToString());
                                    bool boolPNC = (drServer["pnc"].ToString() == "1");
                                    if (intName > 0)
                                    {
                                        if (boolPNC == true)
                                            strName = oServerName.GetNameFactory(intName, 0);
                                        else
                                            strName = oServerName.GetName(intName, 0);
                                    }
                                    strConfigDeviceServer += "<tr><td valign=\"top\" nowrap>Server Name:</td><td valign=\"top\"><a href=\"javascript:void(0);\" onclick=\"return OpenNewWindowMenu('/datapoint/asset/server.aspx?t=name&q=" + oFunction.encryptQueryString(strName) + "&id=" + oFunction.encryptQueryString(intServer.ToString()) + "', '800', '600');\" class=\"lookup\">" + strName + "</a></td></tr>";
                                    int intCluster = Int32.Parse(drServer["clusterid"].ToString());
                                    if (intCluster > 0)
                                    {
                                        DataSet dsCluster = oCluster.Get(intCluster);
                                        if (dsCluster.Tables[0].Rows.Count > 0)
                                        {
                                            strConfigDeviceServer += "<tr><td valign=\"top\" nowrap>Cluster Name:</td><td valign=\"top\"><a href=\"javascript:void(0);\" onclick=\"return OpenNewWindowMenu('/datapoint/asset/cluster.aspx?id=" + oFunction.encryptQueryString(intCluster.ToString()) + "', '800', '600');\" class=\"lookup\">" + dsCluster.Tables[0].Rows[0]["name"].ToString() + "</a></td></tr>";
                                            strConfigDeviceServer += "<tr><td valign=\"top\" nowrap>Nick Name:</td><td valign=\"top\"><a href=\"javascript:void(0);\" onclick=\"return OpenNewWindowMenu('/datapoint/asset/cluster.aspx?id=" + oFunction.encryptQueryString(intCluster.ToString()) + "', '800', '600');\" class=\"lookup\">" + dsCluster.Tables[0].Rows[0]["nickname"].ToString() + "</a></td></tr>";
                                        }
                                    }
                                    strConfigDeviceServer += "<tr><td valign=\"top\" nowrap>Operating System:</td><td valign=\"top\">" + oOperatingSystem.Get(Int32.Parse(drServer["osid"].ToString()), "name") + "</td></tr>";
                                    if (oModelsProperties.IsConfigServicePack(intModel) == true)
                                        strConfigDeviceServer += "<tr><td valign=\"top\" nowrap>Service Pack:</td><td valign=\"top\">" + oServicePack.Get(Int32.Parse(drServer["spid"].ToString()), "name") + "</td></tr>";
                                    if (oModelsProperties.IsConfigVMWareTemplate(intModel) == true)
                                        strConfigDeviceServer += "<tr><td valign=\"top\" nowrap>VMWare Template:</td><td valign=\"top\">" + oVMWare.GetTemplate(Int32.Parse(drServer["templateid"].ToString()), "name") + "</td></tr>";
                                    if (oModelsProperties.IsConfigMaintenanceLevel(intModel) == true)
                                        strConfigDeviceServer += "<tr><td valign=\"top\" nowrap>Maintenance Level:</td><td valign=\"top\">" + oServicePack.Get(Int32.Parse(drServer["spid"].ToString()), "name") + "</td></tr>";
                                    string strComponents = "";
                                    DataSet dsComponents = oServerName.GetComponentDetailSelected(intServer, 1);
                                    foreach (DataRow drComponent in dsComponents.Tables[0].Rows)
                                    {
                                        if (strComponents != "")
                                            strComponents += ", ";
                                        strComponents += drComponent["name"].ToString();
                                    }
                                    if (strComponents == "")
                                        strComponents = "---";
                                    strConfigDeviceServer += "<tr><td valign=\"top\" nowrap>Component Lists:</td><td valign=\"top\">" + strComponents + "</td></tr>";
                                    int intDBA = 0;
                                    if (drServer["dba"].ToString() != "")
                                        intDBA = Int32.Parse(drServer["dba"].ToString());
                                    if (intDBA > 0)
                                        strConfigDeviceServer += "<tr><td valign=\"top\" nowrap>Database Administrator:</td><td valign=\"top\">" + oServer.GetExecutionUser(intDBA) + "</td></tr>";
                                    strConfigDeviceServer += "<tr><td valign=\"top\" nowrap>Domain:</td><td valign=\"top\">" + oDomain.Get(Int32.Parse(drServer["domainid"].ToString()), "name") + "</td></tr>";
                                    if (oForecast.GetAnswer(intAnswer, "test") == "1")
                                        strConfigDeviceServer += "<tr><td valign=\"top\" nowrap>Domain (Test):</td><td valign=\"top\">" + oDomain.Get(Int32.Parse(drServer["test_domainid"].ToString()), "name") + "</td></tr>";
                                    if (intCluster == 0)
                                    {
                                        strConfigDeviceServer += "<tr><td valign=\"top\" nowrap>Infrastructure:</td><td valign=\"top\">" + (drServer["infrastructure"].ToString() == "1" ? "Yes" : "No") + "</td></tr>";
                                        if (drServer["dr"].ToString() == "1")
                                        {
                                            strConfigDeviceServer += "<tr><td valign=\"top\" nowrap>Has DR Counterpart:</td><td valign=\"top\">Yes</td></tr>";
                                            if (drServer["dr_exist"].ToString() == "1")
                                            {
                                                strConfigDeviceServer += "<tr><td valign=\"top\" nowrap>DR server exists:</td><td valign=\"top\">Yes</td></tr>";
                                                strConfigDeviceServer += "<tr><td valign=\"top\" nowrap>DR server name:</td><td valign=\"top\">" + drServer["dr_name"].ToString() + "</td></tr>";
                                            }
                                            else
                                                strConfigDeviceServer += "<tr><td valign=\"top\" nowrap>DR server exists:</td><td valign=\"top\">No</td></tr>";
                                            int intConsistency = 0;
                                            if (drServer["dr_consistencyid"].ToString() != "" && intConsistency > 0)
                                                intConsistency = Int32.Parse(drServer["dr_consistencyid"].ToString());
                                            if (drServer["dr_consistency"].ToString() == "1")
                                                strConfigDeviceServer += "<tr><td valign=\"top\" nowrap>Consistency Group:</td><td valign=\"top\">" + oConsistencyGroup.Get(intConsistency, "name") + "</td></tr>";
                                            else
                                                strConfigDeviceServer += "<tr><td valign=\"top\" nowrap>Consistency Group:</td><td valign=\"top\">---</td></tr>";
                                        }
                                        else
                                            strConfigDeviceServer += "<tr><td valign=\"top\" nowrap>Has DR Counterpart:</td><td valign=\"top\">No</td></tr>";
                                    }
                                }
                                catch
                                {
                                    strConfigDeviceServer = "<tr><td colspan=\"2\"><img src=\"/images/alert.gif\" border=\"0\" align=\"absmiddle\"/> This device has not been configured.</td></tr>";
                                }
                                oTabConfigDevices.AddTab(strName, "");

                                strConfigDeviceServer = oServer.GetExecutionTable(strConfigDeviceServer, false, 0);
                                strConfigDeviceServer = "<div style=\"display:none\">" + strConfigDeviceServer + "<br/></div>";
                                strConfigDevice += strConfigDeviceServer;
                            }
                            strMenuTabConfigDevices1 = oTabConfigDevices.GetTabs();


                            // CONFIGURATION  /  User(s)
                            oTabConfig.AddTab("User(s)", "");
                            Tab oTabConfigUsers = new Tab("", 0, "divMenuConfigUsers1", true, false);
                            for (int ii = 1; ii <= intQuantity; ii++)
                            {
                                string strConfigUserServer = "<tr bgcolor=\"#EEEEEE\"><td><b>User</b></td><td><b>Permissions</b></td></tr>";
                                string strName = "Device #" + ii.ToString();
                                try
                                {
                                    DataRow drServer = dsServer.Tables[0].Rows[ii - 1];
                                    int intServer = Int32.Parse(drServer["id"].ToString());
                                    int intName = Int32.Parse(drServer["nameid"].ToString());
                                    bool boolPNC = (drServer["pnc"].ToString() == "1");
                                    if (intName > 0)
                                    {
                                        if (boolPNC == true)
                                            strName = oServerName.GetNameFactory(intName, 0);
                                        else
                                            strName = oServerName.GetName(intName, 0);
                                    }

                                    DataSet dsAccount = oServer.GetAccounts(intServer);
                                    if (dsAccount.Tables[0].Rows.Count == 0)
                                        strConfigUserServer += "<tr><td colspan=\"2\"><img src=\"/images/alert.gif\" border=\"0\" align=\"absmiddle\"/> The account configuration was skipped for this device.</td></tr>";
                                    else
                                    {
                                        if (boolPNC == true)
                                        {
                                            foreach (DataRow drAccount in dsAccount.Tables[0].Rows)
                                            {
                                                int intAccount = oUser.GetId(drAccount["xid"].ToString());
                                                string strAccount = "";

                                                // Domain Groups
                                                string strAccountDomains = drAccount["domaingroups"].ToString();
                                                char[] strAccountSplit = { ';' };
                                                string[] strAccountDomainArray = strAccountDomains.Split(strAccountSplit);
                                                for (int jj = 0; jj < strAccountDomainArray.Length; jj++)
                                                {
                                                    string strAccountDomain = strAccountDomainArray[jj].Trim();
                                                    if (strAccountDomain.Contains("_") == true)
                                                    {
                                                        strAccount += strAccountDomain.Substring(0, strAccountDomain.IndexOf("_"));
                                                        strAccountDomain = strAccountDomain.Substring(strAccountDomain.IndexOf("_") + 1);
                                                        if (strAccountDomain == "1")
                                                            strAccount += " (Remote Desktop)";
                                                    }
                                                    else
                                                        strAccount += strAccountDomain;
                                                    strAccount += "<br/>";
                                                }

                                                // Local Groups
                                                string strAccountLocals = drAccount["localgroups"].ToString();
                                                string[] strAccountLocalArray = strAccountLocals.Split(strAccountSplit);
                                                for (int jj = 0; jj < strAccountLocalArray.Length; jj++)
                                                {
                                                    string strAccountLocal = strAccountLocalArray[jj].Trim();
                                                    if (strAccountLocal.Contains("_") == true)
                                                    {
                                                        strAccount += strAccountLocal.Substring(0, strAccountLocal.IndexOf("_"));
                                                        strAccountLocal = strAccountLocal.Substring(strAccountLocal.IndexOf("_") + 1);
                                                        if (strAccountLocal == "1")
                                                            strAccount += " (Remote Desktop)";
                                                    }
                                                    else
                                                        strAccount += strAccountLocal;
                                                    strAccount += "<br/>";
                                                }
                                                if (strAccount == "")
                                                        strAccount = "-----";
                                                strConfigUserServer += "<tr><td valign=\"top\">" + oServer.GetExecutionUser(intAccount) + "</td><td valign=\"top\">" + strAccount + "</td></tr>";
                                            }
                                        }
                                        else
                                        {
                                            foreach (DataRow drAccount in dsAccount.Tables[0].Rows)
                                            {
                                                int intAccount = oUser.GetId(drAccount["xid"].ToString());
                                                string strAccount = "";
                                                if (drAccount["admin"].ToString() == "1")
                                                    strAccount = "Administrator<br/>";
                                                else
                                                {
                                                    string strPermission = drAccount["localgroups"].ToString();
                                                    if (strPermission.Contains("GLCfsaRO_SysVol"))
                                                        strAccount += "SYS_VOL (C:) - Read Only<br/>";
                                                    else if (strPermission.Contains("GLCfsaRW_SysVol"))
                                                        strAccount += "SYS_VOL (C:) - Read / Write<br/>";
                                                    else if (strPermission.Contains("GLCfsaFC_SysVol"))
                                                        strAccount += "SYS_VOL (C:) - Full Control<br/>";

                                                    if (strPermission.Contains("GLCfsaRO_UtlVol"))
                                                        strAccount += "UTL_VOL (E:) - Read Only<br/>";
                                                    else if (strPermission.Contains("GLCfsaRW_UtlVol"))
                                                        strAccount += "UTL_VOL (E:) - Read / Write<br/>";
                                                    else if (strPermission.Contains("GLCfsaFC_UtlVol"))
                                                        strAccount += "UTL_VOL (E:) - Full Control<br/>";

                                                    if (strPermission.Contains("GLCfsaRO_AppVol"))
                                                        strAccount += "APP_VOL (F:) - Read Only<br/>";
                                                    else if (strPermission.Contains("GLCfsaRW_AppVol"))
                                                        strAccount += "APP_VOL (F:) - Read / Write<br/>";
                                                    else if (strPermission.Contains("GLCfsaFC_AppVol"))
                                                        strAccount += "APP_VOL (F:) - Full Control<br/>";

                                                    if (strAccount == "")
                                                        strAccount = "-----";
                                                }
                                                strConfigUserServer += "<tr><td valign=\"top\">" + oServer.GetExecutionUser(intAccount) + "</td><td valign=\"top\">" + strAccount + "</td></tr>";
                                            }
                                        }
                                    }
                                }
                                catch
                                {
                                    strConfigUserServer += "<tr><td colspan=\"2\"><img src=\"/images/alert.gif\" border=\"0\" align=\"absmiddle\"/> This device has not been configured.</td></tr>";
                                }
                                oTabConfigUsers.AddTab(strName, "");

                                strConfigUserServer = oServer.GetExecutionTable(strConfigUserServer, true, 600);
                                strConfigUserServer = "<div style=\"display:none\">" + strConfigUserServer + "<br/></div>";
                                strConfigUser += strConfigUserServer;
                            }
                            strMenuTabConfigUsers1 = oTabConfigUsers.GetTabs();


                            // CONFIGURATION  /  Storage
                            oTabConfig.AddTab("Storage", "");
                            Tab oTabConfigStorage = new Tab("", 0, "divMenuConfigStorage1", true, false);
                            for (int ii = 1; ii <= intQuantity; ii++)
                            {
                                string strConfigStorageServer = "";
                                string strName = "Device #" + ii.ToString();
                                try
                                {
                                    DataRow drServer = dsServer.Tables[0].Rows[ii - 1];
                                    int intServer = Int32.Parse(drServer["id"].ToString());
                                    int intName = Int32.Parse(drServer["nameid"].ToString());
                                    bool boolPNC = (drServer["pnc"].ToString() == "1");
                                    if (intName > 0)
                                    {
                                        if (boolPNC == true)
                                            strName = oServerName.GetNameFactory(intName, 0);
                                        else
                                            strName = oServerName.GetName(intName, 0);
                                    }
                                    int intCluster = Int32.Parse(drServer["clusterid"].ToString());
                                    int intCSM = Int32.Parse(drServer["csmconfigid"].ToString());
                                    int intNumber = Int32.Parse(drServer["number"].ToString());
                                    strConfigStorageServer += GetStorage(intAnswer, intCluster, intCSM, intNumber, intModel, strName) + GetStorageShared(intAnswer, intModel);
                                }
                                catch
                                {
                                    strConfigStorageServer += "<tr><td colspan=\"2\"><img src=\"/images/alert.gif\" border=\"0\" align=\"absmiddle\"/> This device has not been configured.</td></tr>";
                                    strConfigStorageServer = oServer.GetExecutionTable(strConfigStorageServer, false, 0);
                                }
                                oTabConfigStorage.AddTab(strName, "");

                                strConfigStorageServer = "<div style=\"display:none\">" + strConfigStorageServer + "<br/></div>";
                                strConfigStorage += strConfigStorageServer;
                            }
                            strMenuTabConfigStorage1 = oTabConfigStorage.GetTabs();


                            strMenuTabConfig1 = oTabConfig.GetTabs();
                            #endregion



                            DataSet dsSteps = oOnDemand.GetSteps(intType, 1);

                            #region EXECUTION
                            if (ds.Tables[0].Rows[0]["executed"].ToString() != "")
                            {
                                Tab oTabExecution = new Tab("", 0, "divMenuExecution1", true, false);
                                int intDevice = 0;
                                foreach (DataRow drServer in dsServer.Tables[0].Rows)
                                {
                                    string strExecutionServer = oServer.GetExecution(Int32.Parse(drServer["id"].ToString()), intEnvironment, dsnAsset, dsnIP, dsnServiceEditor, intAssignPage, intViewPage);
                                    intDevice++;
                                    int intName = Int32.Parse(drServer["nameid"].ToString());
                                    bool boolPNC = (drServer["pnc"].ToString() == "1");
                                    string strName = "Device #" + intDevice.ToString();
                                    if (intName > 0)
                                    {
                                        if (boolPNC == true)
                                            strName = oServerName.GetNameFactory(intName, 0);
                                        else
                                            strName = oServerName.GetName(intName, 0);
                                    }
                                    oTabExecution.AddTab(strName, "");
                                    strExecutionServer = "<div style=\"display:none\">" + strExecutionServer + "<br/></div>";
                                    strExecution += strExecutionServer;
                                }
                                strMenuTabExecution1 = oTabExecution.GetTabs();
                                btnProvisioning.Attributes.Add("onclick", "return OpenNewWindowMenu('/frame/ondemand/status.aspx?rid=" + ds.Tables[0].Rows[0]["requestid"].ToString() + "', '800', '600');");
                            }
                            else
                            {
                                btnProvisioning.Enabled = false;
                                strExecution = "<tr><td class=\"bold\"><img src=\"/images/bigAlert.gif\" border=\"0\" align=\"absmiddle\" /> This design has not been executed!</td></tr>";
                                strExecution = oServer.GetExecutionTable(strExecution, false, 0);
                            }
                            #endregion



                            #region IMPLEMENTATION
                            if (ds.Tables[0].Rows[0]["finished"].ToString() != "")
                            {
                                Tab oTabImplementation = new Tab("", 0, "divMenuImplementation1", true, false);
                                int intDevice = 0;
                                foreach (DataRow drServer in dsServer.Tables[0].Rows)
                                {
                                    
                                    intDevice++;
                                    int intServer = Int32.Parse(drServer["id"].ToString());
                                    int intRequest = Int32.Parse(drServer["requestid"].ToString());
                                    int intName = Int32.Parse(drServer["nameid"].ToString());
                                    bool boolPNC = (drServer["pnc"].ToString() == "1");
                                    string strName = "Device #" + intDevice.ToString();
                                    if (intName > 0)
                                    {
                                        if (boolPNC == true)
                                            strName = oServerName.GetNameFactory(intName, 0);
                                        else
                                            strName = oServerName.GetName(intName, 0);
                                    }
                                    oTabImplementation.AddTab(strName, "");
                                }
                                strMenuTabImplementation1 = oTabImplementation.GetTabs();
                            }
                            else
                            {
                                strImplementation = "<tr><td class=\"bold\"><img src=\"/images/bigAlert.gif\" border=\"0\" align=\"absmiddle\" /> This design has not been implemented!</td></tr>";
                                strImplementation = oServer.GetExecutionTable(strImplementation, false, 0);
                            }
                            #endregion

                        }
                        strMenuTab1 = oTab.GetTabs();
                    }
                    else
                        Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "close", "<script type=\"text/javascript\">window.close();<" + "/" + "script>");
                }
                else if (Request.QueryString["q"] != null && Request.QueryString["q"] != "")
                {
                    string strQuery = oFunction.decryptQueryString(Request.QueryString["q"]);
                    DataSet ds = oDataPoint.GetServiceDesign(Int32.Parse(strQuery));
                    if (ds.Tables[0].Rows.Count == 1)
                    {
                        intAnswer = Int32.Parse(ds.Tables[0].Rows[0]["id"].ToString());
                        Response.Redirect(Request.Path + "?t=" + Request.QueryString["t"] + "&q=" + Request.QueryString["q"] + "&id=" + oFunction.encryptQueryString(intAnswer.ToString()));
                    }
                }
                else
                    Response.Redirect("/datapoint/service/datapoint_service_search.aspx");
                btnClose.Attributes.Add("onclick", "window.close();return false;");
                btnPrint.Attributes.Add("onclick", "window.print();return false;");
                btnSave.Attributes.Add("onclick", oDataPoint.LoadValidation());
                btnSaveClose.Attributes.Add("onclick", oDataPoint.LoadValidation());
            }
            else
                panDenied.Visible = true;
        }
        private string AddForecastStorage(string _class, string _performance, string _amount, string _level, string _replicated, string _ha)
        {
            StringBuilder sb = new StringBuilder();
            double dblAmount = 0.00;
            if (_amount != "")
                dblAmount = double.Parse(_amount);
            double dblReplicated = 0.00;
            if (_replicated != "")
                dblReplicated = double.Parse(_replicated);
            double dblHA = 0.00;
            if (_ha != "")
                dblHA = double.Parse(_ha);
            if (dblAmount > 0.00)
            {
                sb.Append("<tr>");
                sb.Append("<td>");
                sb.Append(_class);
                sb.Append("</td>");
                sb.Append("<td>");
                sb.Append(_performance);
                sb.Append("</td>");
                sb.Append("<td>");
                sb.Append(dblAmount.ToString("F"));
                sb.Append(" GB");
                sb.Append("</td>");
                sb.Append("<td>");
                sb.Append(_level == "" ? "---" : _level);
                sb.Append("</td>");
                sb.Append("<td>");
                sb.Append(_replicated == "" ? "---" : (dblReplicated > 0.00 ? "Yes" : "No"));
                sb.Append("</td>");
                sb.Append("<td>");
                sb.Append(_replicated == "" ? "---" : (dblReplicated > 0.00 ? dblReplicated.ToString("F") + " GB" : "---"));
                sb.Append("</td>");
                sb.Append("<td>");
                sb.Append(_ha == "" ? "---" : (dblHA > 0.00 ? "High" : "Standard"));
                sb.Append("</td>");
                sb.Append("<td>");
                sb.Append(_ha == "" ? "---" : (dblHA > 0.00 ? dblHA.ToString("F") + " GB" : "---"));
                sb.Append("</td>");
                sb.Append("</tr>");
            }
            dblStorageAmount += dblAmount;
            return sb.ToString();
        }
        private string GetStorage(int intAnswer, int intCluster2, int intCSMConfig2, int intNumber2, int intModel, string strName)
        {
            DataSet dsLuns = new DataSet();
            if (intCluster2 == 0)
                dsLuns = oStorage.GetLuns(intAnswer, 0, intCluster2, intCSMConfig2, intNumber2);
            else
                dsLuns = oStorage.GetLunsClusterNonShared(intAnswer, intCluster2, intNumber2);
            bool boolOverride = (oForecast.GetAnswer(intAnswer, "storage_override") == "1");
            string strStorage = AddStorage(dsLuns, intModel, intAnswer, boolOverride);
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
                        strStorage += AddStorage(dsLuns, intModel, intAnswer, boolOverride);
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
        private string AddStorage(DataSet dsLuns, int intModel, int intAnswer, bool boolOverride)
        {
            StringBuilder sbStorage = new StringBuilder();
            int intRow = 0;
            foreach (DataRow drLun in dsLuns.Tables[0].Rows)
            {
                intRow++;
                sbStorage.Append("<tr>");
                sbStorage.Append("<td>");
                sbStorage.Append(intRow.ToString());
                sbStorage.Append("</td>");
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
                {
                    sbStorage.Append("<td>");
                    sbStorage.Append(drLun["path"].ToString());
                    sbStorage.Append("</td>");
                }
                else
                {
                    sbStorage.Append("<td>");
                    sbStorage.Append(strLetter);
                    sbStorage.Append(":");
                    sbStorage.Append(drLun["path"].ToString());
                    sbStorage.Append("</td>");
                }
                sbStorage.Append("<td>");
                sbStorage.Append(drLun["performance"].ToString());
                sbStorage.Append("</td>");
                sbStorage.Append("<td>");
                sbStorage.Append(drLun["size"].ToString());
                sbStorage.Append(" GB / ");
                sbStorage.Append(drLun["actual_size"].ToString() == "-1" ? "---" : drLun["actual_size"].ToString() + " GB");
                sbStorage.Append("</td>");
                sbStorage.Append("<td>");
                sbStorage.Append(drLun["size_qa"].ToString());
                sbStorage.Append(" GB / ");
                sbStorage.Append(drLun["actual_size_qa"].ToString() == "-1" ? "---" : drLun["actual_size_qa"].ToString() + " GB");
                sbStorage.Append("</td>");
                sbStorage.Append("<td>");
                sbStorage.Append(drLun["size_test"].ToString());
                sbStorage.Append(" GB / ");
                sbStorage.Append(drLun["actual_size_test"].ToString() == "-1" ? "---" : drLun["actual_size_test"].ToString() + " GB");
                sbStorage.Append("</td>");
                sbStorage.Append("<td>");
                sbStorage.Append(drLun["replicated"].ToString() == "0" ? "No" : "Yes");
                sbStorage.Append(" / ");
                sbStorage.Append(drLun["actual_replicated"].ToString() == "-1" ? "---" : (drLun["actual_replicated"].ToString() == "0" ? "No" : "Yes"));
                sbStorage.Append("</td>");
                sbStorage.Append("<td>");
                sbStorage.Append(drLun["high_availability"].ToString() == "0" ? "No" : "Yes (" + drLun["size"].ToString() + " GB)");
                sbStorage.Append(" / ");
                sbStorage.Append(drLun["actual_high_availability"].ToString() == "-1" ? "---" : (drLun["actual_high_availability"].ToString() == "0" ? "No" : "Yes (" + drLun["actual_size"].ToString() + " GB)"));
                sbStorage.Append("</td>");
                sbStorage.Append("</tr>");
                DataSet dsPoints = oStorage.GetMountPoints(Int32.Parse(drLun["id"].ToString()));
                int intPoint = 0;
                foreach (DataRow drPoint in dsPoints.Tables[0].Rows)
                {
                    intRow++;
                    intPoint++;
                    sbStorage.Append("<tr>");
                    sbStorage.Append("<td>");
                    sbStorage.Append(intRow.ToString());
                    sbStorage.Append("</td>");
                    if (oForecast.IsOSMidrange(intAnswer) == true)
                    {
                        sbStorage.Append("<td>");
                        sbStorage.Append(drPoint["path"].ToString());
                        sbStorage.Append("</td>");
                    }
                    else
                    {
                        sbStorage.Append("<td>");
                        sbStorage.Append(strLetter);
                        sbStorage.Append(":\\SH");
                        sbStorage.Append(drLun["driveid"].ToString());
                        sbStorage.Append("VOL");
                        sbStorage.Append(intPoint < 10 ? "0" : "");
                        sbStorage.Append(intPoint.ToString());
                        sbStorage.Append("</td>");
                    }
                    sbStorage.Append("<td>");
                    sbStorage.Append(drPoint["performance"].ToString());
                    sbStorage.Append("</td>");
                    sbStorage.Append("<td>");
                    sbStorage.Append(drPoint["size"].ToString());
                    sbStorage.Append(" GB / ");
                    sbStorage.Append(drPoint["actual_size"].ToString() == "-1" ? "---" : drPoint["actual_size"].ToString() + " GB");
                    sbStorage.Append("</td>");
                    sbStorage.Append("<td>");
                    sbStorage.Append(drPoint["size_qa"].ToString());
                    sbStorage.Append(" GB / ");
                    sbStorage.Append(drPoint["actual_size_qa"].ToString() == "-1" ? "---" : drPoint["actual_size_qa"].ToString() + " GB");
                    sbStorage.Append("</td>");
                    sbStorage.Append("<td>");
                    sbStorage.Append(drPoint["size_test"].ToString());
                    sbStorage.Append(" GB / ");
                    sbStorage.Append(drPoint["actual_size_test"].ToString() == "-1" ? "---" : drPoint["actual_size_test"].ToString() + " GB");
                    sbStorage.Append("</td>");
                    sbStorage.Append("<td>");
                    sbStorage.Append(drPoint["replicated"].ToString() == "0" ? "No" : "Yes");
                    sbStorage.Append(" / ");
                    sbStorage.Append(drPoint["actual_replicated"].ToString() == "-1" ? "---" : (drPoint["actual_replicated"].ToString() == "0" ? "No" : "Yes"));
                    sbStorage.Append("</td>");
                    sbStorage.Append("<td>");
                    sbStorage.Append(drPoint["high_availability"].ToString() == "0" ? "No" : "Yes (" + drPoint["size"].ToString() + " GB)");
                    sbStorage.Append(" / ");
                    sbStorage.Append(drPoint["actual_high_availability"].ToString() == "-1" ? "---" : (drPoint["actual_high_availability"].ToString() == "0" ? "No" : "Yes (" + drPoint["actual_size"].ToString() + " GB)"));
                    sbStorage.Append("</td>");
                    sbStorage.Append("</tr>");
                }
            }
            return sbStorage.ToString();
        }
        protected void btnNew_Click(Object Sender, EventArgs e)
        {
            Response.Redirect("/datapoint/service/datapoint_service_search.aspx");
        }
        protected void btnSave_Click(Object Sender, EventArgs e)
        {
            int intSave = Save();
            Response.Redirect(Request.Path + "?id=" + oFunction.encryptQueryString(intAnswer.ToString()) + "&" + (intSave > 0 ? "save" : "error") + "=" + intSave.ToString());
        }
        protected void btnSaveClose_Click(Object Sender, EventArgs e)
        {
            int intSave = Save();
            Response.Redirect(Request.Path + "?id=" + oFunction.encryptQueryString(intAnswer.ToString()) + "&" + (intSave > 0 ? "close" : "error") + "=" + intSave.ToString());
        }
        protected int Save()
        {
            int intReturn = 1;
            return intReturn;
        }
    }
}
