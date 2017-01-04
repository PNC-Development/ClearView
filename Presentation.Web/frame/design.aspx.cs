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
using System.Data.SqlTypes;
using System.Text;
using NCC.ClearView.Presentation.Web.Custom;
using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;

namespace NCC.ClearView.Presentation.Web
{
    public partial class design_wizard : System.Web.UI.Page
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
        private string dsnServiceEditor = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["ServiceEditorDSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected double dblCompressionPercentage = double.Parse(ConfigurationManager.AppSettings["SQL2008_COMPRESSION_PERCENTAGE"]);
        protected double dblTempDBOverhead = double.Parse(ConfigurationManager.AppSettings["SQL2008_TEMPDB_OVERHEAD"]);
        protected int intForecastPage = Int32.Parse(ConfigurationManager.AppSettings["ForecastEdit"]);
        protected int intAssignPage = Int32.Parse(ConfigurationManager.AppSettings["AssignWorkload"]);
        protected int intResourceRequestApprove = Int32.Parse(ConfigurationManager.AppSettings["ResourceRequestApprove"]);
        protected int intViewPage = Int32.Parse(ConfigurationManager.AppSettings["ViewWorkload"]);
        protected int intImplementorDistributedService = Int32.Parse(ConfigurationManager.AppSettings["SERVICEID_IMPLEMENTOR_DISTRIBUTED"]);
        protected int intImplementorMidrangeService = Int32.Parse(ConfigurationManager.AppSettings["SERVICEID_IMPLEMENTOR_MIDRANGE"]);
        protected int intWorkstationPlatform = Int32.Parse(ConfigurationManager.AppSettings["WorkstationPlatformID"]);
        protected int intLocation = Int32.Parse(ConfigurationManager.AppSettings["OPSLocationID"]);
        protected int intExceptionServiceFolder = Int32.Parse(ConfigurationManager.AppSettings["SERVICEFOLDER_EXCEPTION"]);
        protected int intServiceRequestPage = Int32.Parse(ConfigurationManager.AppSettings["NewResourceRequest"]);
        
        protected int intProfile;
        protected Design oDesign;
        protected Pages oPage;
        protected Mnemonic oMnemonic;
        protected Users oUser;
        protected Functions oFunction;
        protected Variables oVariable;
        protected CostCenter oCostCenter;
        protected Classes oClass;
        protected Environments oEnvironment;
        protected Locations oLocation;
        protected OperatingSystems oOperatingSystem;
        protected Holidays oHoliday;
        protected ModelsProperties oModelsProperties;
        protected Types oType;
        protected Forecast oForecast;
        protected Servers oServer;
        protected Asset oAsset;
        protected OnDemandTasks oOnDemandTask;
        protected ResourceRequest oResourceRequest;
        protected Requests oRequest;
        protected Projects oProject;
        protected Log oLog;

        protected int intForecast;
        protected int intID;
        protected int intAnswer;
        protected int intPhaseID;
        protected string strSpacer = "45";
        protected string strImageLock = "/images/docshare.gif";
        protected string strImageComplete = "/images/check.gif";
        protected string strImageCurrent = "/images/green_arrow.gif";
        protected string strImageApproval = "/images/required.gif";
        protected string strHighlight = "#FFEE99";
        protected string strWindow = "111111111111111111111111";
        protected string strGridBackup = "";
        protected string strGridBackupTable = "";
        protected string strGridBackupSun = "";
        protected string strGridBackupMon = "";
        protected string strGridBackupTue = "";
        protected string strGridBackupWed = "";
        protected string strGridBackupThu = "";
        protected string strGridBackupFri = "";
        protected string strGridBackupSat = "";
        private int intGridBackups = 0;
        protected string strGridMaintenance = "";
        protected string strGridMaintenanceTable = "";
        protected string strGridMaintenanceSun = "";
        protected string strGridMaintenanceMon = "";
        protected string strGridMaintenanceTue = "";
        protected string strGridMaintenanceWed = "";
        protected string strGridMaintenanceThu = "";
        protected string strGridMaintenanceFri = "";
        protected string strGridMaintenanceSat = "";
        private int intGridMaintenances = 0;
        private double dblSLA = 10.00;
        private string strValidation = "";
        private int intPhaseCount = 0;
        protected string strQuestions = "";
        protected string strHidden = "";
        protected int intStorageQuestion = 0;
        protected string strStorageDisplay = "none";
        protected bool boolWindows = false;
        private string[] strHiddens = new string[25];
        private int intHiddens = 0;
        protected string strExecuteHeader = "";
        protected string strExecuteMiddle = "";
        protected string strExecuteFooter = "";
        protected int intDeviceCount = 1;
        protected string strManualImage = "";
        protected string strManualReason = "";
        protected string strLocation = "";
        protected bool boolDemo = false;

        protected Settings oSetting;
        protected string strFreezeStart = "";
        protected string strFreezeEnd = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oDesign = new Design(intProfile, dsn);
            oPage = new Pages(intProfile, dsn);
            oMnemonic = new Mnemonic(intProfile, dsn);
            oUser = new Users(intProfile, dsn);
            oFunction = new Functions(intProfile, dsn, intEnvironment);
            oVariable = new Variables(intEnvironment);
            oCostCenter = new CostCenter(intProfile, dsn);
            oClass = new Classes(intProfile, dsn);
            oEnvironment = new Environments(intProfile, dsn);
            oLocation = new Locations(intProfile, dsn);
            oOperatingSystem = new OperatingSystems(intProfile, dsn);
            oHoliday = new Holidays(intProfile, dsn);
            oModelsProperties = new ModelsProperties(intProfile, dsn);
            oType = new Types(intProfile, dsn);
            oForecast = new Forecast(intProfile, dsn);
            oServer = new Servers(intProfile, dsn);
            oAsset = new Asset(intProfile, dsnAsset, dsn);
            oOnDemandTask = new OnDemandTasks(intProfile, dsn);
            oResourceRequest = new ResourceRequest(intProfile, dsn);
            oRequest = new Requests(intProfile, dsn);
            oProject = new Projects(intProfile, dsn);
            oLog = new Log(intProfile, dsn);
            oSetting = new Settings(intProfile, dsn);

            if (Request.QueryString["aid"] != null)
                Int32.TryParse(Request.QueryString["aid"], out intAnswer);
            if (intAnswer > 0)
            {
                // Show Execution Page
                int intCount = 0;
                string strMenuTabDivs = "";
                string strDivs = "";
                string strMenuTab1 = "";

                Tab oTabTop = new Tab("", 1, "divMenu1", true, true);
                oTabTop.AddTab("Design Provisioning Status", "");
                oTabTop.AddTab("Design Summary", "");
                strMenuTab1 = oTabTop.GetTabs();
                //bool boolExecuted = false;

                if (oForecast.CanAutoProvision(intAnswer) == true)
                {
                    int intMenuTab = 0;
                    if (Request.QueryString["menu_tab"] != null && Request.QueryString["menu_tab"] != "")
                        intMenuTab = Int32.Parse(Request.QueryString["menu_tab"]);
                    Tab oTabDivs = new Tab("", intMenuTab, "divMenuDivs", true, true);

                    int intRequest = oForecast.GetRequestID(intAnswer, true);
                    DataSet dsServer = oServer.GetRequests(intRequest, 1);
                    foreach (DataRow drServer in dsServer.Tables[0].Rows)
                    {
                        intCount++;
                        int intServer = Int32.Parse(drServer["id"].ToString());
                        int intAsset = 0;
                        if (drServer["assetid"].ToString() != "")
                            intAsset = Int32.Parse(drServer["assetid"].ToString());
                        int intType = 0;
                        int intModel = 0;
                        if (drServer["modelid"].ToString() != "")
                            intModel = Int32.Parse(drServer["modelid"].ToString());
                        if (intModel > 0)
                            intType = oModelsProperties.GetType(intModel);
                        string strName = "Device " + intCount.ToString();
                        if (intAsset > 0)
                        {
                            string strTempName = oAsset.Get(intAsset, "name");
                            if (strTempName != "")
                                strName = strTempName;
                        }
                        if (intCount == 1)
                        {
                            oTabDivs.AddTab(strName, "");
                            strDivs += "<div id=\"divTab" + intCount.ToString() + "\" style=\"display:inline\"><iframe width=\"100%\" height=\"100%\" frameborder=\"0\" scrolling=\"auto\" src=\"" + oType.Get(intType, "ondemand_steps_path") + "?id=" + oFunction.encryptQueryString(intServer.ToString()) + "&c=" + intCount.ToString() + "\"></iframe></div>";
                        }
                        else
                        {
                            oTabDivs.AddTab(strName, "");
                            strDivs += "<div id=\"divTab" + intCount.ToString() + "\" style=\"display:none\"><iframe width=\"100%\" height=\"100%\" frameborder=\"0\" scrolling=\"auto\" src=\"" + oType.Get(intType, "ondemand_steps_path") + "?id=" + oFunction.encryptQueryString(intServer.ToString()) + "&c=" + intCount.ToString() + "\"></iframe></div>";
                        }
                    }

                    strMenuTabDivs = oTabDivs.GetTabs();
                    strExecuteHeader = "<table width=\"100%\" height=\"100%\" cellpadding=\"4\" cellspacing=\"2\" border=\"0\"><tr height=\"1\"><td valign=\"top\">" + strMenuTab1 + "<div id=\"divMenu1\"><br /><div style=\"display:inline\"><br />" + strMenuTabDivs + "<div id=\"divMenuDivs\">" + strDivs + "</div></div><div style=\"display:none\">";
                }
                else
                //if (boolExecuted == false)
                {
                    panNotExecutable.Visible = true;
                    strExecuteHeader = "<table width=\"100%\" height=\"100%\" cellpadding=\"4\" cellspacing=\"2\" border=\"0\"><tr height=\"1\"><td valign=\"top\">" + strMenuTab1 + "<div id=\"divMenu1\"><br /><div style=\"display:inline\"><br />";
                    strExecuteMiddle = "</div><div style=\"display:none\">";
                    lblInitiated.Text = oForecast.GetAnswer(intAnswer, "executed");
                    lblCompleted.Text = oForecast.GetAnswer(intAnswer, "completed");
                    int intImplementorUser = 0;
                    DataSet dsImplementor = oOnDemandTask.GetPending(intAnswer);
                    if (dsImplementor.Tables[0].Rows.Count > 0)
                    {
                        intImplementorUser = Int32.Parse(dsImplementor.Tables[0].Rows[0]["resourceid"].ToString());
                        intImplementorUser = Int32.Parse(oResourceRequest.GetWorkflow(intImplementorUser, "userid"));
                    }
                    strManualImage = "<img src=\"/frame/picture.aspx?xid=" + oUser.GetName(intImplementorUser) + "\" border=\"0\" align=\"absmiddle\" style=\"height:90px;width:90px;border-width:0px;border:solid 1px #999999;\" />";
                    lblImplementor.Text = oUser.GetFullName(intImplementorUser) + " (" + oUser.GetName(intImplementorUser) + ")";
                    strManualReason = oForecast.CanAutoProvisionReason(intAnswer);
                    btnManual.NavigateUrl = "/datapoint/service/design.aspx?t=design&q=" + oFunction.encryptQueryString(intAnswer.ToString());
                    rptServers.DataSource = oServer.GetManual(intAnswer, false);
                    rptServers.DataBind();
                    foreach (RepeaterItem ri in rptServers.Items)
                    {
                        Label lblName = (Label)ri.FindControl("lblName");
                        if (lblName.Text != "--- Pending ---")
                            lblName.Text = "<a href=\"javascript:void(0);\" onclick=\"OpenNewWindow('/datapoint/asset/datapoint_asset_search.aspx?t=name&q=" + oFunction.encryptQueryString(lblName.Text) + "&id=" + oFunction.encryptQueryString(lblName.ToolTip) + "',800,600);\">" + lblName.Text + "</a>";
                        else
                            lblName.Text = "<i>" + lblName.Text + "</i>";
                        Label lblAsset = (Label)ri.FindControl("lblAsset");
                        if (lblAsset.Text != "--- Pending ---")
                            lblAsset.Text = "<a href=\"javascript:void(0);\" onclick=\"OpenNewWindow('/datapoint/asset/datapoint_asset_search.aspx?t=serial&q=" + oFunction.encryptQueryString(lblAsset.Text) + "',800,600);\">" + lblAsset.Text + "</a>";
                        else
                            lblAsset.Text = "<i>" + lblAsset.Text + "</i>";
                        Label lblAssetDR = (Label)ri.FindControl("lblAssetDR");
                        if (lblAssetDR.Text != "--- None ---")
                        {
                            if (lblAssetDR.Text != "--- Pending ---")
                            {
                                if (lblAssetDR.Text != "Missing!!!")
                                    lblAssetDR.Text = "<a href=\"javascript:void(0);\" onclick=\"OpenNewWindow('/datapoint/asset/datapoint_asset_search.aspx?t=serial&q=" + oFunction.encryptQueryString(lblAssetDR.Text) + "',800,600);\">" + lblAssetDR.Text + "</a>";
                                else
                                    lblAssetDR.Text = "<span class=\"highlight\">" + lblAssetDR.Text + "</span>";
                            }
                            else
                                lblAssetDR.Text = "<i>" + lblAssetDR.Text + "</i>";
                        }
                        else
                            lblAssetDR.Text = "<i>Not Required</i>";
                        Label lblIP1 = (Label)ri.FindControl("lblIP1");
                        if (lblIP1.Text == "--- Pending ---")
                            lblIP1.Text = "<i>" + lblIP1.Text + "</i>";
                        Label lblIP2 = (Label)ri.FindControl("lblIP2");
                        if (lblIP2.Text == "--- Pending ---")
                            lblIP2.Text = "<i>" + lblIP2.Text + "</i>";
                        Label lblIP3 = (Label)ri.FindControl("lblIP3");
                        if (lblIP3.Text == "--- Pending ---")
                            lblIP3.Text = "<i>" + lblIP3.Text + "</i>";

                    }
                }
                strExecuteFooter = "</div></div></td></tr></table>";
            }

            if (Request.QueryString["parent"] != null)
                Int32.TryParse(Request.QueryString["parent"], out intForecast);
            if (Request.QueryString["id"] != null)
                Int32.TryParse(Request.QueryString["id"], out intID);

            if (Request.QueryString["phase"] != null)
            {
                Int32.TryParse(Request.QueryString["phase"], out intPhaseID);
                if (!IsPostBack)
                {
                    btnNext.Visible = false;
                    btnBack.Visible = false;
                    DataSet dsSubmitted = oDesign.GetSubmitted(intID);
                    if (dsSubmitted.Tables[0].Rows.Count > 0 && (LoadRejected(dsSubmitted) == false))
                        btnReturn.Visible = true;
                    else
                        btnUpdate.Visible = true;
                }
            }
            Page.Title = "ClearView Design Builder";

            if (!IsPostBack || panStorageLuns.Visible)
            {
                oDesign.SetupDesign(intID);
                if (oDesign.IsExceptionDone(intID) == false)
                    oDesign.LoadDesign(intID);
                if (intID > 0)
                {
                    Int32.TryParse(oDesign.Get(intID, "forecastid"), out intForecast);
                    Int32.TryParse(oDesign.Get(intID, "answerid"), out intAnswer);
                    if (intAnswer > 0 && Request.QueryString["aid"] == null)
                        Redirect("&aid=" + intAnswer.ToString());
                    lblID.Text = "&nbsp;(#" + intID.ToString() + ")";
                    lblID2.Text = lblID.Text;
                }

                if (intForecast > 0)
                {
                    int intRequest = Int32.Parse(oForecast.Get(intForecast, "requestid"));
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
                }

                if (Request.QueryString["refresh"] != null)
                    Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "refreshed", "<script type=\"text/javascript\">if(window.opener!=null)window.opener.navigate('" + oPage.GetFullLink(intForecastPage) + "?id=" + intForecast.ToString() + "');<" + "/" + "script>");
                int intPhase = intPhaseID;
                int intPhaseCurrent = oDesign.GetPhaseID(intID);
                if (intPhase == 0)                      // No phase from querystring
                {
                    intPhase = intPhaseCurrent;         // Get current phase in wizard
                    if (intPhaseCurrent == 999)         // Summary or completion
                    {
                        LoadSummary();
                        btnNext.Visible = false;
                        btnBack.Visible = false;
                    }
                }
                else                                    // Phase came from querystring
                {
                    if (intPhase == intPhaseCurrent)    // This phase = current phase of wizard
                    {
                        btnNext.Visible = true;
                        btnBack.Visible = true;
                        btnUpdate.Visible = false;
                    }
                }
                lblPhase.Text = intPhase.ToString();
                if (intPhase < 999)
                    panRequirements.Visible = true;
                boolWindows = oDesign.IsWindows(intID);
                LoadNavigation(intPhase);
                LoadQuestions(intPhase);
                LoadValidation();

                if (Request.QueryString["saved"] != null)
                    Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "saved", "<script type=\"text/javascript\">if(window.opener!=null)window.opener.navigate('" + oPage.GetFullLink(intForecastPage) + "?id=" + intForecast.ToString() + "');<" + "/" + "script>");
                if (Request.QueryString["duplicate"] != null && Request.QueryString["duplicate"] != "")
                    Page.ClientScript.RegisterStartupScript(typeof(Page), "duplicate", "<script type=\"text/javascript\">alert('The account " + Request.QueryString["duplicate"] + " has already been configured');<" + "/" + "script>");
                if (Request.QueryString["conflict"] != null && Request.QueryString["conflict"] != "")
                    Page.ClientScript.RegisterStartupScript(typeof(Page), "conflict", "<script type=\"text/javascript\">alert('The account " + Request.QueryString["duplicate"] + " has already been configured');<" + "/" + "script>");

                btnBack.Attributes.Add("onclick", "return ProcessButton(this) && LoadWait();");
                btnGenerate.Attributes.Add("onclick", "return ValidateNumber('" + txtDatabaseSize.ClientID + "','Please enter a valid database size')" +
                    " && ValidateNumber('" + txtDatabasePercent.ClientID + "','Please enter a valid number')" +
                    " && ValidateRadioButtons('" + radTempDBNo.ClientID + "','" + radTempDBYes.ClientID + "','Please select whether or not you are using a non-standard TempDB size')" +
                    " && (document.getElementById('" + radTempDBNo.ClientID + "').checked == true || (document.getElementById('" + radTempDBYes.ClientID + "').checked == true && ValidateNumber('" + txtDatabaseTemp.ClientID + "','Please enter a valid size for the TempDB size')))" +
                    " && ValidateRadioButtons('" + radDatabaseNonNo.ClientID + "','" + radDatabaseNonYes.ClientID + "','Please select whether or not you want to store non-database data on the same instance')" +
                    " && (document.getElementById('" + radDatabaseNonNo.ClientID + "').checked == true || (document.getElementById('" + radDatabaseNonYes.ClientID + "').checked == true && ValidateNumber('" + txtDatabaseNon.ClientID + "','Please enter a valid size for the non-database data storage')))" +
                    " && ProcessButton(this) && LoadWait()" +
                    ";");
                btnReturn.Attributes.Add("onclick", "return ProcessButton(this) && LoadWait();");
                btnExecute.Attributes.Add("onclick", "return ProcessButton(this) && LoadWait();");
                btnReview.Attributes.Add("onclick", "return LoadWait();");
                btnReset.Attributes.Add("onclick", "return ProcessButton(this) && LoadWait();");
                btnSubmit.Attributes.Add("onclick", "return ValidateRadioButtons3('" + radComplete.ClientID + "','" + radChange.ClientID + "','" + radException.ClientID + "','Select an option')" +
                    " && (ValidateText('" + txtExceptionID.ClientID + "','') == false || ValidateNumber('" + txtExceptionID.ClientID + "','Enter a valid number'))" +
                    " && ValidateText('" + txtException.ClientID + "','You must provide a description for the exception')" +
                    " && ProcessButton(this) && LoadWait()" +
                    ";");
                btnChange.Attributes.Add("onclick", "return LoadWait();");
                btnStart.Attributes.Add("onclick", "return confirm('NOTE: Billing will begin: " + DateTime.Today.ToShortDateString() + "\\n\\nAre you sure you want to continue?') && LoadWait();");
                btnSchedule.Attributes.Add("onclick", "return ValidateDate('" + txtScheduleDate.ClientID + "','Please enter a valid schedule date')" +
                    " && ValidateDateToday('" + txtScheduleDate.ClientID + "','The scheduled date must occur after today')" +
                    " && ValidateTime('" + txtScheduleTime.ClientID + "','Please enter a valid start time')" +
                    " && confirm('NOTE: Billing will begin: ' + document.getElementById('" + txtScheduleDate.ClientID + "').value + '\\n\\nAre you sure you want to continue?')" +
                    " && ProcessButton(this) && LoadWait()" +
                    ";");
                //btnHelp.Attributes.Add("onclick", "ShowHideDiv2('" + trHelp.ClientID + "');return false;");
                btnHelp.Attributes.Add("onclick", "ShowPanel('/frame/design_list.aspx?id=" + intPhase.ToString() + "&type=HELP',575,475,true);return false;");
                btnPrint.Attributes.Add("onclick", "OpenNewWindow('/frame/design_print.aspx?id=" + intID.ToString() + "',750,600);return false;");
                btnBackup.Attributes.Add("onclick", "ShowHideDiv('" + divBackup.ClientID + "','inline');return false;");
            }
        }

        private void LoadNavigation(int _phaseid)
        {
            tblNavigation.Rows.Clear();
            DataSet dsPhases = oDesign.GetPhases(1);
            foreach (DataRow drPhases in dsPhases.Tables[0].Rows)
            {
                int intPhase = Int32.Parse(drPhases["id"].ToString());
                if (intPhase == 12)
                    intPhase = 12;
                intPhaseCount++;
                TableRow oNavigationRow = new TableRow();
                System.Web.UI.WebControls.Image oImage = new System.Web.UI.WebControls.Image();
                HyperLink oLink = new HyperLink();
                oLink.Text = drPhases["title"].ToString() + ":";
                oLink.NavigateUrl = Request.Path + "?id=" + intID.ToString() + "&phase=" + intPhase.ToString();
                Label oLabel = new Label();
                oLabel.Text = oDesign.GetValue(intID, intPhase);
                string strException = oDesign.GetException(intID, intPhase);
                TableCell oNavigationCell1 = new TableCell();
                TableCell oNavigationCell2 = new TableCell();
                TableCell oNavigationCell3 = new TableCell();

                if (oDesign.IsLocked(intID, intPhase) == true)
                {
                    oImage.ImageUrl = strImageLock;
                    oLink.Enabled = false;
                    if (oLabel.Text != "")
                    {
                        int intSet = oDesign.ResponseID;    // Get selected ResponseID
                        bool boolBlank = true;
                        DataSet dsCheck = oDesign.GetSelectionSet(intSet);
                        foreach (DataRow drCheck in dsCheck.Tables[0].Rows)
                        {
                            int intCheckResponse = Int32.Parse(drCheck["responseid"].ToString());
                            if (oDesign.IsResponseSelected(intID, intCheckResponse) == true)
                            {
                                // Response was auto-selected...leave alone
                                boolBlank = false;
                            }
                        }
                        if (boolBlank == true)
                            oLabel.Text = "";
                    }
                    if (oLabel.Text == "")
                        oLabel.Text = "<i>N / A</i>";
                    if (_phaseid == intPhase)
                    {
                        // The current phase is locked.  Move to next step.
                        int intRedirect = oDesign.Next(intID, true);
                        //oDesign.UpdatePhaseId(intID, intRedirect);
                        Response.Redirect(Request.Path + "?id=" + intID.ToString() + "&phase=" + intRedirect.ToString());
                    }
                }
                else if (_phaseid == intPhase)
                {
                    // Current phase
                    oImage.ImageUrl = strImageCurrent;
                    oLink.CssClass = "bold";
                    lblDescription.Text = drPhases["description"].ToString();
                    string strHelp = drPhases["help"].ToString().Trim();
                    if (strHelp != "")
                    {
                        panHelp.Visible = true;
                        litHelp.Text = strHelp;
                    }
                    if (intID == 0 && intPhaseCount != 1)
                    {
                        // Prevent anyone from using URL to move to another step before step # 1 is done
                        if (intForecast > 0)
                            Response.Redirect(Request.Path + "?id=" + intID.ToString() + "&parent=" + intForecast.ToString());
                        else
                        {
                            btnNext.Enabled = false;
                            btnBack.Enabled = false;
                            btnUpdate.Enabled = false;
                            btnSubmit.Enabled = false;
                        }
                    }
                }
                else if (intID == 0)
                {
                    // If just creating, and not first step, disable
                    oImage.ImageUrl = strImageLock;
                    oLink.Enabled = false;
                }
                else if (oDesign.GetPhaseCompletion(intID, intPhase).Tables[0].Rows.Count > 0 && oLabel.Text != "")
                {
                    // If complete
                    if (strException != "")
                    {
                        // If exception
                        oImage.ImageUrl = strImageApproval;
                        oImage.Attributes.Add("onclick", "alert('Requires Approval: " + strException + "');");
                    }
                    else
                        oImage.ImageUrl = strImageComplete;
                }
                else
                {
                    // Have not started
                    oImage.ImageUrl = "/images/spacer.gif";
                    if (oLabel.Text == "")
                        oLabel.Text = "---";
                }

                if (oLink.Enabled)
                    oLink.Attributes.Add("onclick", "LoadWait();");
                oNavigationCell1.Controls.Add(oImage);
                oNavigationRow.Cells.Add(oNavigationCell1);

                oNavigationCell2.Controls.Add(oLink);
                oNavigationRow.Cells.Add(oNavigationCell2);

                oNavigationCell3.Controls.Add(oLabel);
                oNavigationRow.Cells.Add(oNavigationCell3);

                tblNavigation.Rows.Add(oNavigationRow);
            }
        }
        private void LoadQuestions(int _phaseid)
        {
            //DataSet dsQuestion = oDesign.GetQuestions(_phaseid, 1);
            //foreach (DataRow drQuestion in dsQuestion.Tables[0].Rows)
            DataRow[] drQuestions = oDesign.Questions.Select("PhaseID = " + _phaseid.ToString());
            foreach (DataRow drQuestion in drQuestions)
            {
                int intQuestion = Int32.Parse(drQuestion["id"].ToString());
                bool boolHidden = (drQuestion["available"].ToString() == "0");
                bool boolDropDown = (drQuestion["is_type_drop_down"].ToString() == "1");
                bool boolCheckBox = (drQuestion["is_type_check_box"].ToString() == "1");
                bool boolRadio = (drQuestion["is_type_radio"].ToString() == "1");
                bool boolTextBox = (drQuestion["is_type_textbox"].ToString() == "1");
                bool boolTextArea = (drQuestion["is_type_textarea"].ToString() == "1");

                string strSpecial = oDesign.GetQuestionSpecial(intQuestion);
                switch (strSpecial)
                {
                    case "MNEMONIC":
                        panMnemonic.Visible = true;
                        lblQuestionMnemonic.Text = drQuestion["question"].ToString();
                        txtMnemonic.Attributes.Add("onkeyup", "return AJAXTextBoxGet(this,'300','195','" + divMnemonic.ClientID + "','" + lstMnemonic.ClientID + "','" + hdnMnemonic.ClientID + "','" + oVariable.URL() + "/frame/ajax/ajax_mnemonics_pending.aspx',2);");
                        lstMnemonic.Attributes.Add("ondblclick", "AJAXClickRow();");

                        int intMnemonic = 0;
                        if (Int32.TryParse(oDesign.Get(intID, "mnemonicid"), out intMnemonic) == true && intMnemonic > 0)
                            txtMnemonic.Text = oMnemonic.Get(intMnemonic, "factory_code") + " - " + oMnemonic.Get(intMnemonic, "name");
                        hdnMnemonic.Value = intMnemonic.ToString();
                        break;
                    case "COST_CENTER":
                        panCostCenter.Visible = true;
                        lblQuestionCostCenter.Text = drQuestion["question"].ToString();
                        txtCostCenter.Attributes.Add("onkeyup", "return AJAXTextBoxGet(this,'200','195','" + divCostCenter.ClientID + "','" + lstCostCenter.ClientID + "','" + hdnCostCenter.ClientID + "','" + oVariable.URL() + "/frame/ajax/ajax_cost_centers.aspx',5);");
                        lstCostCenter.Attributes.Add("ondblclick", "AJAXClickRow();");

                        int intCostCenter = 0;
                        if (Int32.TryParse(oDesign.Get(intID, "costid"), out intCostCenter) == true && intCostCenter > 0)
                            txtCostCenter.Text = oCostCenter.GetName(intCostCenter);
                        hdnCostCenter.Value = intCostCenter.ToString();
                        break;
                    case "USER_SI":
                        if (boolDemo == false)
                        {
                            panSI.Visible = true;
                            lblQuestionSI.Text = drQuestion["question"].ToString();
                            txtSI.Attributes.Add("onkeyup", "return AJAXTextBoxGet(this,'300','195','" + divSI.ClientID + "','" + lstSI.ClientID + "','" + hdnSI.ClientID + "','" + oVariable.URL() + "/frame/users.aspx',2);");
                            lstSI.Attributes.Add("ondblclick", "AJAXClickRow();");
                        }

                        int intSI = 0;
                        if (Int32.TryParse(oDesign.Get(intID, "si"), out intSI) == true && intSI > 0)
                            txtSI.Text = oUser.GetFullName(intSI) + " (" + oUser.GetName(intSI) + ")";
                        hdnSI.Value = intSI.ToString();
                        break;
                    case "USER_DTG":
                        panDTG.Visible = true;
                        lblQuestionDTG.Text = drQuestion["question"].ToString();
                        txtDTG.Attributes.Add("onkeyup", "return AJAXTextBoxGet(this,'300','195','" + divDTG.ClientID + "','" + lstDTG.ClientID + "','" + hdnDTG.ClientID + "','" + oVariable.URL() + "/frame/users.aspx',2);");
                        lstDTG.Attributes.Add("ondblclick", "AJAXClickRow();");

                        int intDTG = 0;
                        if (Int32.TryParse(oDesign.Get(intID, "dtg"), out intDTG) == true && intDTG > 0)
                            txtDTG.Text = oUser.GetFullName(intDTG) + " (" + oUser.GetName(intDTG) + ")";
                        hdnDTG.Value = intDTG.ToString();
                        break;
                    case "GRID_BACKUP":
                        panGridBackup.Visible = true;
                        lblQuestionGridBackup.Text = drQuestion["question"].ToString();
                        ddlGridBackup.Attributes.Add("onchange", "SetConflictConfig(this,'tblBackup','hdnBackup');");
                        ddlGridBackup.Attributes.Add("onmousewheel", "return false;");
                        DataSet dsBackup = oDesign.GetBackup(intID);
                        if (dsBackup.Tables[0].Rows.Count > 0)
                        {
                            DataRow drBackup = dsBackup.Tables[0].Rows[0];
                            strGridBackupSun = drBackup["sun"].ToString();
                            strGridBackupMon = drBackup["mon"].ToString();
                            strGridBackupTue = drBackup["tue"].ToString();
                            strGridBackupWed = drBackup["wed"].ToString();
                            strGridBackupThu = drBackup["thu"].ToString();
                            strGridBackupFri = drBackup["fri"].ToString();
                            strGridBackupSat = drBackup["sat"].ToString();

                            StringBuilder strTemp = new StringBuilder();
                            strTemp.Append(oDesign.LoadGrid(strGridBackupSun, 1, ref intGridBackups));
                            strTemp.Append(oDesign.LoadGrid(strGridBackupMon, 2, ref intGridBackups));
                            strTemp.Append(oDesign.LoadGrid(strGridBackupTue, 3, ref intGridBackups));
                            strTemp.Append(oDesign.LoadGrid(strGridBackupWed, 4, ref intGridBackups));
                            strTemp.Append(oDesign.LoadGrid(strGridBackupThu, 5, ref intGridBackups));
                            strTemp.Append(oDesign.LoadGrid(strGridBackupFri, 6, ref intGridBackups));
                            strTemp.Append(oDesign.LoadGrid(strGridBackupSat, 7, ref intGridBackups));
                            strGridBackup = strTemp.ToString();
                        }
                        else
                        {
                            strGridBackupSun = strWindow;
                            strGridBackupMon = strWindow;
                            strGridBackupTue = strWindow;
                            strGridBackupWed = strWindow;
                            strGridBackupThu = strWindow;
                            strGridBackupFri = strWindow;
                            strGridBackupSat = strWindow;
                        }
                        strGridBackupTable = oDesign.LoadMatrix("Backup", "B", strGridBackupSun, strGridBackupMon, strGridBackupTue, strGridBackupWed, strGridBackupThu, strGridBackupFri, strGridBackupSat);
                        break;
                    case "BACKUP_EXCLUSION":
                        panBackupExclusions.Visible = true;
                        lblQuestionBackupExclusions.Text = drQuestion["question"].ToString();
                        btnAddExclusion.Attributes.Add("onclick", "return ValidateText('" + txtPath.ClientID + "','Please enter the path') && ProcessButton(this) && LoadWait();");
                        //AddValidation("ValidateText('" + txt.ClientID + "','Please enter the initiative opportunity')");

                        rptExclusions.DataSource = oDesign.GetExclusions(intID);
                        rptExclusions.DataBind();
                        foreach (RepeaterItem ri in rptExclusions.Items)
                        {
                            LinkButton _delete = (LinkButton)ri.FindControl("btnDeleteExclusion");
                            _delete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this exclusion?');");
                        }
                        if (rptExclusions.Items.Count == 0)
                            lblExclusion.Visible = true;
                        break;
                    case "GRID_MAINTENANCE":
                        panGridMaintenance.Visible = true;
                        lblQuestionGridMaintenance.Text = drQuestion["question"].ToString();
                        ddlGridMaintenance.Attributes.Add("onchange", "SetConflictConfig(this,'tblMaintenance','hdnMaintenance');");
                        ddlGridMaintenance.Attributes.Add("onmousewheel", "return false;");
                        DataSet dsMaintenance = oDesign.GetMaintenance(intID);
                        if (dsMaintenance.Tables[0].Rows.Count > 0)
                        {
                            DataRow drMaintenance = dsMaintenance.Tables[0].Rows[0];
                            strGridMaintenanceSun = drMaintenance["sun"].ToString();
                            strGridMaintenanceMon = drMaintenance["mon"].ToString();
                            strGridMaintenanceTue = drMaintenance["tue"].ToString();
                            strGridMaintenanceWed = drMaintenance["wed"].ToString();
                            strGridMaintenanceThu = drMaintenance["thu"].ToString();
                            strGridMaintenanceFri = drMaintenance["fri"].ToString();
                            strGridMaintenanceSat = drMaintenance["sat"].ToString();

                            StringBuilder strTemp = new StringBuilder();
                            strTemp.Append(oDesign.LoadGrid(strGridMaintenanceSun, 1, ref intGridMaintenances));
                            strTemp.Append(oDesign.LoadGrid(strGridMaintenanceMon, 2, ref intGridMaintenances));
                            strTemp.Append(oDesign.LoadGrid(strGridMaintenanceTue, 3, ref intGridMaintenances));
                            strTemp.Append(oDesign.LoadGrid(strGridMaintenanceWed, 4, ref intGridMaintenances));
                            strTemp.Append(oDesign.LoadGrid(strGridMaintenanceThu, 5, ref intGridMaintenances));
                            strTemp.Append(oDesign.LoadGrid(strGridMaintenanceFri, 6, ref intGridMaintenances));
                            strTemp.Append(oDesign.LoadGrid(strGridMaintenanceSat, 7, ref intGridMaintenances));
                            strGridMaintenance = strTemp.ToString();
                        }
                        else
                        {
                            strGridMaintenanceSun = strWindow;
                            strGridMaintenanceMon = strWindow;
                            strGridMaintenanceTue = strWindow;
                            strGridMaintenanceWed = strWindow;
                            strGridMaintenanceThu = strWindow;
                            strGridMaintenanceFri = strWindow;
                            strGridMaintenanceSat = strWindow;
                        }
                        strGridMaintenanceTable = oDesign.LoadMatrix("Maintenance", "M", strGridMaintenanceSun, strGridMaintenanceMon, strGridMaintenanceTue, strGridMaintenanceWed, strGridMaintenanceThu, strGridMaintenanceFri, strGridMaintenanceSat);
                        break;
                    case "STORAGE_LUNS":
                        panStorageLuns.Visible = true;
                        lblQuestionStorageLuns.Text = drQuestion["question"].ToString();
                        intStorageQuestion = intQuestion;
                        radTempDBYes.Attributes.Add("onclick", "ShowHideDiv('" + divTempDB.ClientID + "','inline');");
                        radTempDBNo.Attributes.Add("onclick", "ShowHideDiv('" + divTempDB.ClientID + "','none');");
                        radDatabaseNonYes.Attributes.Add("onclick", "ShowHideDiv('" + divDatabaseNon.ClientID + "','inline');");
                        radDatabaseNonNo.Attributes.Add("onclick", "ShowHideDiv('" + divDatabaseNon.ClientID + "','none');");
                        if (oDesign.IsStoragePersistent(intID) == true)
                            strStorageDisplay = "inline";
                        if (!IsPostBack)
                        {
                            ddlStorageDriveNew.DataValueField = "id";
                            ddlStorageDriveNew.DataTextField = "path";
                            ddlStorageDriveNew.DataSource = oDesign.GetDrives();
                            ddlStorageDriveNew.DataBind();
                            LoadStorage(intID);
                        }
                        //lblStorage.Text = "Persistent, " + intStorage.ToString() + " GB";
                        break;
                    case "ACCOUNTS":
                        panAccounts.Visible = true;
                        lblQuestionAccounts.Text = drQuestion["question"].ToString();
                        txtAccount.Attributes.Add("onkeyup", "return AJAXTextBoxGet(this,'300','195','" + divAccount.ClientID + "','" + lstAccount.ClientID + "','" + hdnAccount.ClientID + "','" + oVariable.URL() + "/frame/users.aspx',2,null,'PNC');");
                        lstAccount.Attributes.Add("ondblclick", "AJAXClickRow();");
                        btnAddAccount.Attributes.Add("onclick", "return ValidateHidden('" + hdnAccount.ClientID + "','" + txtAccount.ClientID + "','Enter a username, first name or last name') && ValidateDropDown('" + ddlPermission.ClientID + "','Select a permission') && ProcessButton(this) && LoadWait();");
                        ddlPermission.Attributes.Add("onchange", "CheckRemoteDesktop(this, '" + chkRemoteDesktop.ClientID + "');");
                        btnManager.Attributes.Add("onclick", "return OpenWindow('NEW_USER','');");
                        rptAccounts.DataSource = oDesign.GetAccounts(intID);
                        rptAccounts.DataBind();
                        foreach (RepeaterItem ri in rptAccounts.Items)
                        {
                            LinkButton _delete = (LinkButton)ri.FindControl("btnDeleteAccount");
                            _delete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this account?');");
                            Label _permissions = (Label)ri.FindControl("lblPermissions");
                            switch (_permissions.Text)
                            {
                                case "0":
                                    _permissions.Text = "-----";
                                    break;
                                case "D":
                                    _permissions.Text = "Developer";
                                    break;
                                case "P":
                                    _permissions.Text = "Promoter";
                                    break;
                                case "S":
                                    _permissions.Text = "AppSupport";
                                    break;
                                case "U":
                                    _permissions.Text = "AppUsers";
                                    break;
                            }
                            if (_permissions.ToolTip == "1")
                                _permissions.Text += " (R/D)";
                        }
                        if (rptAccounts.Items.Count == 0)
                            lblNone.Visible = true;
                        string strNotify = oDesign.Get(intID, "accounts_email");
                        chkNotify.Checked = (strNotify == "" || strNotify == "1");
                        break;
                    case "DATE":
                        panDate.Visible = true;
                        lblQuestionDate.Text = drQuestion["question"].ToString();
                        imgDate.Attributes.Add("onclick", "return ShowCalendarMin('" + txtDate.ClientID + "',15);");
                        hdnDate.Value = oHoliday.GetDays(15.00, DateTime.Today).ToShortDateString();
                        AddValidation("ValidateDateHidden('" + txtDate.ClientID + "','" + hdnDate.ClientID + "','You must enter a date at least three weeks (15 business days) from today.\\n\\n')");

                        DateTime datDate = DateTime.Now;
                        if (DateTime.TryParse(oDesign.Get(intID, "commitment"), out datDate) == true)
                            txtDate.Text = datDate.ToShortDateString();
                        else
                            txtDate.Text = hdnDate.Value;
                        break;
                    case "LOCATION":
                        panLocation.Visible = true;
                        lblQuestionLocation.Text = drQuestion["question"].ToString();
                        int intAddress = 0;
                        if (Int32.TryParse(oDesign.Get(intID, "addressid"), out intAddress) == true && intAddress > 0)
                            intLocation = intAddress;
                        strLocation = oLocation.LoadDDL("ddlState", "ddlCity", "ddlAddress", hdnLocation.ClientID, intLocation, true, "ddlCommon");
                        hdnLocation.Value = intLocation.ToString();
                        AddValidation("ValidateHidden0('" + hdnLocation.ClientID + "','ddlState','Please select a location')");
                        break;
                    default:
                        bool boolUnder48 = oDesign.IsUnder48(intID, true);
                        //DataSet dsResponse = oDesign.GetResponses(intQuestion, (boolUnder48 ? 1 : 0), (boolUnder48 ? 0 : 1), 1, 1);
                        DataRow[] drResponses = oDesign.Responses.Select("QuestionID = " + intQuestion.ToString() + " AND is_under48 >= " + (boolUnder48 ? "1" : "0") + " AND is_over48 >= " + (boolUnder48 ? "0" : "1") + " AND visible = 1 AND available = 1");
                        strQuestions += "<div id=\"DIV" + intQuestion.ToString() + "\" style=\"display:" + (boolHidden == true ? "none" : "inline") + "\" />";
                        strQuestions += "<br />";
                        strQuestions += "<table cellpadding=\"4\" cellspacing=\"0\" border=\"0\">";
                        strQuestions += "<tr>";
                        strQuestions += "<td colspan=\"2\">" + drQuestion["question"].ToString() + "</td>";
                        strQuestions += "</tr>";
                        strQuestions += "<tr>";
                        strQuestions += "<td><img src=\"/images/spacer.gif\" border=\"0\" height=\"1\" width='" + strSpacer + "' /></td>";
                        strQuestions += "<td width=\"100%\">";
                        // If one or more of the answers to this question are configured based on another answers selection, then disable the responses.
                        bool boolDisabled = oDesign.IsQuestionSelected(intID, intQuestion);
                        string strValue = "";
                        string strFieldQuestion = drQuestion["related_field"].ToString();
                        string strDefaultValueQuestion = drQuestion["default_value"].ToString();
                        string strSuffix = drQuestion["suffix"].ToString();
                        int intResponse = 0;
                        List<int> arrSelected = oDesign.GetResponseSelected(intID, intQuestion);
                        if (arrSelected.Count > 0)
                            intResponse = arrSelected[0];
                        if (intResponse == 0 && IsPostBack)
                        {
                            foreach (DataRow drResponse in drResponses)
                            {
                                int intTemp = Int32.Parse(drResponse["id"].ToString());
                                if (CheckPostBack(intTemp) == true)
                                {
                                    intResponse = intTemp;
                                    strValue = oDesign.GetResponse(intResponse, "related_value");
                                    break;
                                }
                            }
                        }
                        int intResponseOne = 0;
                        int intResponseCount = drResponses.Length;
                        foreach (DataRow drResponse in drResponses)
                        {
                            int intResponseTemp = Int32.Parse(drResponse["id"].ToString());
                            if (drResponse["visible"].ToString() != "1")
                                intResponseCount--;
                            else
                            {
                                if (intResponseOne != 0)    // No longer the default, so must be more than one valid response.
                                    break;
                                intResponseOne = intResponseTemp;
                            }
                        }
                        if (intResponseCount == 1)
                        {
                            // Only one response available?  Make sure to select it.
                            if (oDesign.GetResponse(intResponseOne, "select_if_one") == "1")
                            {
                                intResponse = intResponseOne;
                                strValue = oDesign.GetResponse(intResponseOne, "related_value");
                            }
                            else
                                intResponse = 0;
                        }
                        if (strFieldQuestion != "" && boolTextBox == false && boolTextArea == false)
                        {
                            if (strValue == "")
                                strValue = oDesign.Get(intID, strFieldQuestion);
                            if (strValue == "" && strDefaultValueQuestion != "")
                            {
                                strValue = strDefaultValueQuestion;
                                if (intResponse == 0)
                                {
                                    // Try to locate the response.
                                    foreach (DataRow drResponse in drResponses)
                                    {
                                        int intResponseTemp = Int32.Parse(drResponse["id"].ToString());
                                        if (drResponse["visible"].ToString() == "1")
                                        {
                                            int intComponentID = 0;
                                            Int32.TryParse(drResponse["set_componentid"].ToString(), out intComponentID);
                                            string strSet = (intComponentID == 0 ? drResponse["related_value"].ToString() : "1");
                                            if (strSet == strValue)
                                            {
                                                intResponse = intResponseTemp;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                            AddHidden(strFieldQuestion, (intResponse > 0 ? intResponse.ToString() + "_" + strValue : ""), boolDisabled);
                        }
                        if (boolDropDown)
                        {
                            if (strValue == "" && strDefaultValueQuestion != "")
                                strValue = strDefaultValueQuestion;
                            //strQuestions += "<select onchange=\"LoadEditorAffectsDropDown(this);\" name=\"" + drForm["dbfield"].ToString() + "\" id=\"" + drForm["dbfield"].ToString() + "\" class=\"default\" style=\"width:" + drForm["width"].ToString() + "px;\">";
                            strQuestions += "<select onchange=\"UpdateDDL(this," + intID.ToString() + ",'HDN_" + strFieldQuestion + "');\" name=\"DDL" + intQuestion.ToString() + "\" id=\"DDL" + intQuestion.ToString() + "\">";
                            strQuestions += "<option value=\"0\">-- SELECT --</option>";
                            foreach (DataRow drResponse in drResponses)
                            {
                                int intResponseTemp = Int32.Parse(drResponse["id"].ToString());
                                if (drResponse["visible"].ToString() == "1")
                                {
                                    string strSet = drResponse["related_value"].ToString();
                                    int intTemp = 0;
                                    if (Int32.TryParse(drResponse["set_classid"].ToString(), out intTemp) && intTemp > 0)
                                        strSet = intTemp.ToString();
                                    else if (Int32.TryParse(drResponse["set_osid"].ToString(), out intTemp) && intTemp > 0)
                                        strSet = intTemp.ToString();
                                    else if (Int32.TryParse(drResponse["set_environmentid"].ToString(), out intTemp) && intTemp > 0)
                                        strSet = intTemp.ToString();

                                    strQuestions += "<option " + (strValue == strSet ? "selected=\"selected\"" : "") + " value=\"" + drResponse["id"].ToString() + "_" + strSet + "\">" + drResponse["response"].ToString() + "</option>";
                                }
                            }
                            strQuestions += "</select>";
                            //if (drForm["required"].ToString() == "1")
                            //strRequired += " && ValidateDropDown('" + drForm["dbfield"].ToString() + "','" + drForm["required_text"].ToString() + "')";
                        }
                        else if (boolCheckBox || boolRadio)
                        {
                            strQuestions += "<table id=\"TBL" + intQuestion.ToString() + "\" class=\"default\" border=\"0\">";
                            foreach (DataRow drResponse in drResponses)
                            {
                                int intResponseTemp = Int32.Parse(drResponse["id"].ToString());
                                if (drResponse["visible"].ToString() == "1")
                                {
                                    string strID = intResponseTemp.ToString();
                                    int intComponentID = 0;
                                    Int32.TryParse(drResponse["set_componentid"].ToString(), out intComponentID);
                                    string strField = strFieldQuestion;
                                    if (strFieldQuestion == "")
                                        strField = drResponse["related_field"].ToString();
                                    if (strField == "" && intComponentID == 0)
                                        strQuestions += "<tr><td>*** Missing FIELD configuration (Response = " + drResponse["response"].ToString() + ")</td></tr>";
                                    else
                                    {
                                        string strResponse = drResponse["response"].ToString();
                                        string strSet = "";
                                        if (intComponentID == 0)
                                        {
                                            strValue = oDesign.Get(intID, strField);
                                            if (strValue == "" && strDefaultValueQuestion != "")
                                                strValue = strDefaultValueQuestion;    // Set to default value
                                            if (intResponseCount == 1 && drResponse["select_if_one"].ToString() == "1")
                                            {
                                                // Only one response available?  Make sure to select it.
                                                strValue = drResponse["related_value"].ToString();
                                                strResponse += "&nbsp;&nbsp;&nbsp;(<i>Selected by Default</i>)";
                                            }
                                            strSet = drResponse["related_value"].ToString();
                                        }
                                        else
                                        {
                                            strField = intComponentID.ToString();
                                            strValue = (oDesign.IsSoftwareComponent(intID, intComponentID) ? "1" : "0");
                                            strSet = "1";
                                        }
                                        bool boolPostback = false;
                                        if (IsPostBack)
                                            boolPostback = CheckPostBack(Int32.Parse(strID));
                                        if (strFieldQuestion == "")
                                            AddHidden(strField, (strValue == strSet || boolPostback ? strID + "_" + strValue : ""), false);
                                        strQuestions += "<tr>";
                                        string strValidateDR = "";
                                        if (strField.ToUpper() == "DR")
                                        {
                                            int intMnemonicDR = 0;
                                            if (Int32.TryParse(oDesign.Get(intID, "mnemonicid"), out intMnemonicDR) == true && intMnemonicDR > 0)
                                            {
                                                // Set DR Recovery from Mnemonic
                                                int intHours = oMnemonic.GetResRatingHRs(intMnemonicDR);
                                                if (intHours <= 48 && strSet == "0")
                                                    strValidateDR = "return ConfirmDR(this," + intHours + ") && ";
                                                if (intHours > 48 && strSet == "1")
                                                    strValidateDR = "return ConfirmDR(this," + intHours + ") && ";
                                            }
                                        }
                                        if (boolCheckBox)
                                            strQuestions += "<td><input id=\"" + strID + "\" type=\"checkbox\" name=\"CHK" + intQuestion.ToString() + "\"" + (boolDisabled ? " disabled" : "") + " value=\"" + strSet + "\"" + (strValue == strSet || boolPostback ? " checked" : "") + " onclick=\"" + strValidateDR + "UpdateCheckValue(this," + intID.ToString() + "," + strID + ",'HDN_" + strField + "');\" /><label for=\"" + strID + "\">" + strResponse + "</label></td>";
                                        if (boolRadio)
                                            strQuestions += "<td><input id=\"" + strID + "\" type=\"checkbox\" name=\"RAD" + intQuestion.ToString() + "\"" + (boolDisabled ? " disabled" : "") + " value=\"" + strSet + "\"" + (strValue == strSet || boolPostback ? " checked" : "") + " onclick=\"" + strValidateDR + "UpdateChecksValue(this," + intID.ToString() + "," + strID + ",'HDN_" + strField + "');\" /><label for=\"" + strID + "\">" + strResponse + "</label></td>";
                                        strQuestions += "</tr>";
                                    }
                                }
                            }
                            strQuestions += "</table>";
                            //if (drForm["required"].ToString() == "1")
                            //strRequired += " && ValidateHidden('" + drForm["dbfield"].ToString() + "','TBL" + drForm["dbfield"].ToString() + "','" + drForm["required_text"].ToString() + "')";
                        }
                        else if (boolTextBox)
                        {
                            strValue = oDesign.Get(intID, strFieldQuestion);
                            if (strValue == "" && strDefaultValueQuestion != "")
                                strValue = strDefaultValueQuestion;
                            strQuestions += "<input name=\"" + intQuestion.ToString() + "\" type=\"text\" id=\"" + intQuestion.ToString() + "\" class=\"default\" style=\"width:150px;\" value=\"" + strValue + "\" onblur=\"UpdateTextValue(this,'HDN_" + strFieldQuestion + "');\" />" + (strSuffix != "" ? "&nbsp;" + strSuffix : "");
                            AddHidden(strFieldQuestion, strValue, boolDisabled);
                        }
                        else if (boolTextArea)
                        {
                            strValue = oDesign.Get(intID, strFieldQuestion);
                            if (strValue == "" && strDefaultValueQuestion != "")
                                strValue = strDefaultValueQuestion;
                            strQuestions += "<textarea name=\"" + intQuestion.ToString() + "\" rows=\"5\" id=\"" + intQuestion.ToString() + "\" class=\"default\" style=\"width:500px;\" onblur=\"UpdateTextValue(this,'HDN_" + strFieldQuestion + "');\">" + strValue + "</textarea>";
                            AddHidden(strFieldQuestion, strValue, boolDisabled);
                        }
                        else
                            strQuestions += "*** This question is missing a control type!";
                        strQuestions += "</td>";
                        strQuestions += "</tr>";
                        strQuestions += "</table>";
                        strQuestions += "</div>";
                        break;
                }
            }
        }
        private void LoadValidation()
        {
            if (panMnemonic.Visible == true)
                AddValidation("ValidateHidden0('" + hdnMnemonic.ClientID + "','" + txtMnemonic.ClientID + "','Please enter the mnemonic of this design\\n\\n(Start typing and a list will be presented...)')");
            if (panSI.Visible == true)
                AddValidation("ValidateHidden0('" + hdnSI.ClientID + "','" + txtSI.ClientID + "','Please enter the service integrator of this design\\n\\n(Start typing and a list will be presented...)')");

            // Load Validation
            if (strValidation != "")
            {
                btnNext.Attributes.Add("onclick", "return " + strValidation + " && ProcessButton(this) && LoadWait();");
                btnUpdate.Attributes.Add("onclick", "return " + strValidation + " && ProcessButton(this) && LoadWait();");
            }
            else
            {
                btnNext.Attributes.Add("onclick", "return ProcessButton(this) && LoadWait();");
                btnUpdate.Attributes.Add("onclick", "return ProcessButton(this) && LoadWait();");
            }
        }
        private void AddValidation(string _validation)
        {
            if (strValidation != "")
                strValidation += " && ";
            strValidation += _validation;
        }
        private void AddHidden(string _field, string _value, bool _disabled)
        {
            bool boolHiddenAdd = true;
            foreach (string _hidden in strHiddens)
            {
                if (_hidden == _field)
                {
                    boolHiddenAdd = false;
                    break;
                }
            }
            if (boolHiddenAdd == true)
            {
                strHidden += "<input type=\"hidden\" name=\"HDN_" + _field + "\" id=\"HDN_" + _field + "\"" + (_disabled ? " disabled" : "") + " value=\"" + _value + "\" />";
                strHiddens[intHiddens] = _field;
                intHiddens++;
            }
        }

        private bool CheckPostBack(int _responseid)
        {
            bool boolPostback = false;
            string strSet = oDesign.GetResponse(_responseid, "related_value");
            foreach (string strForm in Request.Form)
            {
                if (strForm.StartsWith("HDN_") == true)
                {
                    string strPostbackField = strForm.Substring(4);
                    // strPostbackField : "persistent"
                    string strPostbackValue = Request.Form[strForm];
                    // strPostbackValue : "29_1"
                    // Get ResponseID
                    if (strPostbackValue.Contains("_"))
                    {
                        string strReponseID = strPostbackValue.Substring(0, strPostbackValue.IndexOf("_"));
                        string strSelected = strPostbackValue.Substring(strPostbackValue.IndexOf("_") + 1);
                        if (strReponseID == _responseid.ToString() && strSelected == strSet)
                        {
                            boolPostback = true;
                            break;
                        }
                    }
                }
            }
            return boolPostback;
        }
        private void LoadSummary() 
        {
            panSummary.Visible = true;
            DataSet dsSummary = oDesign.Get(intID);
            if (dsSummary.Tables[0].Rows.Count > 0)
            {
                DesignControl oControl = (DesignControl)LoadControl("/controls/fore/fore_design.ascx");
                oControl.DesignId = intID;
                oControl.InvalidPanel = (boolDemo ? divValidationDemo : divValidation);
                oControl.ValidPanel = (boolDemo ? divDefault : divValid);
                oControl.RejectPanel = divReject;
                oControl.CompleteRadio = (boolDemo ? radStart : radComplete);
                oControl.ScheduleRadio = (boolDemo ? radSchedule : null);
                oControl.ExceptionRadio = radException;
                oControl.ValidationLabel = (boolDemo ? lblValidDemo : lblValid);
                oControl.ExceptionPanel = divException;
                oControl.ExceptionServiceFolder = intExceptionServiceFolder;
                hypException.NavigateUrl = oPage.GetFullLink(intServiceRequestPage) + "?rid=0&sid=" + intExceptionServiceFolder.ToString();
                oControl.ExceptionServiceFolderPanel = panExceptionServiceFolder;
                oControl.Demo = boolDemo;
                phSummary.Controls.Add(oControl);

                if (boolDemo == true)
                {
                    int intModel = oDesign.GetModelProperty(intID);
                    btnChange.Attributes.Add("oncontextmenu", "alert('Solution Determined = " + oModelsProperties.Get(intModel, "name") + "');return false;");
                    if (dsSummary.Tables[0].Rows[0]["answerid"].ToString() == "" || dsSummary.Tables[0].Rows[0]["answerid"].ToString() == "0")
                    {
                        panDemoStart.Visible = true;
                        radStart.Attributes.Add("onclick", "ShowBuildDivs('" + divStart.ClientID + "','" + divDefault.ClientID + "','" + divSchedule.ClientID + "','" + divApproval.ClientID + "');");
                        radSchedule.Attributes.Add("onclick", "ShowBuildDivs('" + divSchedule.ClientID + "','" + divDefault.ClientID + "','" + divStart.ClientID + "','" + divApproval.ClientID + "');");
                        radApproval.Attributes.Add("onclick", "ShowBuildDivs('" + divApproval.ClientID + "','" + divDefault.ClientID + "','" + divStart.ClientID + "','" + divSchedule.ClientID + "');");
                        imgScheduleDate.Attributes.Add("onclick", "return ShowCalendar('" + txtScheduleDate.ClientID + "');");
                    }
                    else
                    {
                        panDemoExecuted.Visible = true;
                        int intAnswer = Int32.Parse(dsSummary.Tables[0].Rows[0]["answerid"].ToString());
                        DataSet dsAnswer = oForecast.GetAnswer(intAnswer);
                        if (dsAnswer.Tables[0].Rows.Count > 0)
                        {
                            lblExecutedOn.Text = dsAnswer.Tables[0].Rows[0]["executed"].ToString();
                            int intExecutedBy = 0;
                            if (Int32.TryParse(dsAnswer.Tables[0].Rows[0]["executed_by"].ToString(), out intExecutedBy) == true)
                                lblExecutedBy.Text = oUser.GetFullName(intExecutedBy);
                        }
                    }
                }
                else
                {
                    DataSet dsSubmitted = oDesign.GetSubmitted(intID);
                    bool boolRejected = LoadRejected(dsSubmitted);
                    if (dsSubmitted.Tables[0].Rows.Count == 0 || boolRejected == true)
                    {
                        panSubmit.Visible = true;
                        if (dsSubmitted.Tables[0].Rows.Count > 0)
                        {
                            DataRow drSubmitted = dsSubmitted.Tables[0].Rows[0];
                            txtException.Text = drSubmitted["comments"].ToString();
                            txtExceptionID.Text = drSubmitted["exceptionID"].ToString();
                        }
                    }
                    bool boolException = oDesign.IsOther(intID, 0, 0, 1);
                    if (boolException)
                    {
                        trException.Visible = true;
                        string strExceptions = "";
                        DataSet dsPhases = oDesign.GetPhases(1);
                        foreach (DataRow drPhases in dsPhases.Tables[0].Rows)
                        {
                            string strException = oDesign.GetException(intID, Int32.Parse(drPhases["id"].ToString()));
                            if (strException != "")
                                strExceptions += " - " + strException + "\\n";
                        }
                        btnException.Attributes.Add("onclick", "alert('The following responses require approval:\\n\\n" + strExceptions.Replace("'", "") + "');return false;");
                    }
                    radComplete.Attributes.Add("onclick", "ShowDesignDIV(" + (boolException ? "true" : "false") + ",false,'" + divException.ClientID + "','" + divReject.ClientID + "','" + divValid.ClientID + "','" + divValidation.ClientID + "');");
                    radChange.Attributes.Add("onclick", "ShowDesignDIV(false,true,'" + divException.ClientID + "','" + divReject.ClientID + "','" + divValid.ClientID + "','" + divValidation.ClientID + "');");
                    radException.Attributes.Add("onclick", "ShowDesignDIVException('" + divException.ClientID + "','" + divReject.ClientID + "','" + divValid.ClientID + "','" + divValidation.ClientID + "',this,'" + radException.ClientID + "','/frame/design_exception.aspx');");
                }
            }
        }
        private bool LoadRejected(DataSet _submitted)
        {
            bool boolRejected = false;
            if (_submitted.Tables[0].Rows.Count > 0)
            {
                DataSet dsWorkflow = oDesign.LoadWorkflow(intID);
                bool boolComplete = true;
                foreach (DataRow drWorkflow in dsWorkflow.Tables[0].Rows)
                {
                    if (drWorkflow["rejected"].ToString() == "1")
                    {
                        if (boolComplete == true)
                        {
                            // Should not show rejected if one or more of the previous tasks are not complete.
                            boolRejected = true;
                        }
                        boolComplete = false;
                        lblRejectReason.Text = drWorkflow["reason"].ToString();
                        lblRejectBy.Text = drWorkflow["approver"].ToString();
                        lblRejectOn.Text = drWorkflow["completed"].ToString();
                        lblRejectTitle.Text = drWorkflow["title"].ToString();
                        break;
                    }
                    if (drWorkflow["completed"].ToString() == "")
                    {
                        boolComplete = false;
                        boolRejected = false;
                    }
                }
                if (boolRejected == true)
                {
                    divReject.Style["display"] = "inline";
                    //divValid.Style["display"] = "none";
                    //divValidation.Style["display"] = "none";
                    //panValid.Visible = false;
                    //panValidation.Visible = false;
                }
                if (boolRejected == false)
                {
                    panWorkflow.Visible = true;
                    panApproved.Visible = boolComplete;
                    panApproval.Visible = (boolComplete == false);
                    dsWorkflow.Relations.Add("relationship", dsWorkflow.Tables[0].Columns["title"], dsWorkflow.Tables[1].Columns["title"], false);
                    rptWorkflow.DataSource = dsWorkflow;
                    rptWorkflow.DataBind();
                }
                if (panApproved.Visible == true)
                {
                    panApprovers.Visible = false;
                    string strCan = oDesign.CanExecute(intID);
                    if (strCan.ToUpper().Contains("MNEMONIC"))
                    {
                        panPending.Visible = true;
                        panApproved.Visible = false;
                        imgPending.Attributes.Add("onclick", "alert('" + strCan + "');");
                    }
                    else
                    {
                        int intAnswerTemp = 0;
                        Int32.TryParse(oDesign.Get(intID, "answerid"), out intAnswerTemp);
                        if (intAnswerTemp == 0)
                        {
                            int intResourceRequest = 0;
                            int intRequest = 0;
                            int intService = 0;
                            int intNumber = 0;
                            int intWorkflow = oDesign.GetImplementorWorkflow(intID, intImplementorDistributedService, intImplementorMidrangeService, false);
                            int intImplementor = 0;
                            DataSet dsImplementor = oResourceRequest.GetWorkflow(intWorkflow);
                            if (dsImplementor.Tables[0].Rows.Count > 0)
                            {
                                intResourceRequest = Int32.Parse(dsImplementor.Tables[0].Rows[0]["parent"].ToString());
                                intImplementor = Int32.Parse(dsImplementor.Tables[0].Rows[0]["userid"].ToString());
                            }
                            DataSet dsResourceRequest = oResourceRequest.Get(intResourceRequest);
                            if (dsResourceRequest.Tables[0].Rows.Count > 0)
                            {
                                intRequest = Int32.Parse(dsResourceRequest.Tables[0].Rows[0]["requestid"].ToString());
                                intService = Int32.Parse(dsResourceRequest.Tables[0].Rows[0]["serviceid"].ToString());
                                intNumber = Int32.Parse(dsResourceRequest.Tables[0].Rows[0]["number"].ToString());
                            }

                            // Check freeze
                            bool boolFrozen = false;
                            strFreezeStart = oSetting.Get("freeze_start");
                            strFreezeEnd = oSetting.Get("freeze_end");
                            if (strFreezeStart != "" && strFreezeEnd != "" && DateTime.Parse(strFreezeStart) <= DateTime.Now && DateTime.Parse(strFreezeEnd) > DateTime.Now)
                            {
                                int intFreeze = 0;
                                if (Int32.TryParse(oSetting.Get("freeze_skip_requestid"), out intFreeze) == false || intFreeze != intID)
                                {
                                    boolFrozen = true;
                                    btnExecute.Enabled = false;
                                    panFreeze.Visible = true;
                                }
                            }

                            if (oDesign.CanAutoProvision(intID) == true || intImplementor > 0)
                            {
                                panExecute.Visible = true;
                                //btnExecute.Visible = oDesign.CanAutoProvision(intID);
                                // commented since this was preventing midrange requests that were not assigned to a builder at the time the design was fully approved to be executed (after the builder was assigned)
                            }
                            else
                            {
                                if (intImplementor == 0 && intWorkflow == 0)
                                {
                                    if (boolFrozen)
                                    {
                                        // During freeze ONLY - show executtion with disabled.
                                        panExecute.Visible = true;
                                    }
                                    else
                                    {
                                        if (intRequest == 0)
                                            intRequest = Int32.Parse(oForecast.Get(intForecast, "requestid"));
                                        // Send notification
                                        ServiceRequests oServiceRequest = new ServiceRequests(intProfile, dsn);
                                        Services oService = new Services(intProfile, dsn);
                                        int intOS = 0;
                                        Int32.TryParse(oDesign.Get(intID, "osid"), out intOS);
                                        bool boolDistributed = oOperatingSystem.IsDistributed(intOS);
                                        if (oServiceRequest.Get(intRequest, "requestid") == "")
                                            oServiceRequest.Add(intRequest, 1, 1);
                                        int intResource = oServiceRequest.AddRequest(intRequest, (boolDistributed ? oService.GetItemId(intImplementorDistributedService) : oService.GetItemId(intImplementorMidrangeService)), (boolDistributed ? intImplementorDistributedService : intImplementorMidrangeService), 0, 0.00, 2, 1, dsnServiceEditor);
                                        if (oServiceRequest.NotifyApproval(intResource, intResourceRequestApprove, intEnvironment, "", dsnServiceEditor) == false)
                                            oServiceRequest.NotifyTeamLead(oService.GetItemId(intImplementorDistributedService), intResource, intAssignPage, intViewPage, intEnvironment, "", dsnServiceEditor, dsnAsset, dsnIP, 0);
                                    }
                                }
                                lblTechnician.Text = "";
                                panTechnician.Visible = (panExecute.Visible == false);
                                panApproved.Visible = boolFrozen;
                            }
                        }
                    }
                }
                if (panWorkflow.Visible == true && intAnswer == 0 && Request.QueryString["norefresh"] == null)
                    Response.AppendHeader("Refresh", "60");
            }
            return boolRejected;
        }

        protected void btnStart_Click(Object Sender, EventArgs e)
        {
            oLog.AddEvent(intID, "", "", "Design " + intID.ToString() + " was started by " + oUser.GetFullNameWithLanID(intProfile), LoggingType.Information);
            oDesign.UpdateException(intID, false);
            oDesign.UpdateExceptionDone(intID, false);
            // Sunmit
            oDesign.AddSubmitted(intID, intProfile, txtException.Text, txtExceptionID.Text);
            intAnswer = oDesign.Approve(intID, intProfile, false, intEnvironment, intImplementorDistributedService, intWorkstationPlatform, intImplementorMidrangeService, dsnAsset, dsnIP, dsnServiceEditor, intAssignPage, intResourceRequestApprove, intViewPage, true);
            Redirect(intAnswer > 0 ? "&aid=" + intAnswer.ToString() : "");
        }
        protected void btnSchedule_Click(Object Sender, EventArgs e)
        {
        }
        protected void lnkRefresh_Click(Object Sender, EventArgs e)
        {
            Redirect(intID, intForecast, "");
        }
        protected void btnBack_Click(Object Sender, EventArgs e)
        {
            int intPhase = Int32.Parse(lblPhase.Text);
            Save(intPhase);
            oDesign.Next(intID, false);
            Redirect(intID, intForecast, "");
        }
        protected void btnNext_Click(Object Sender, EventArgs e)
        {
            int intPhase = Int32.Parse(lblPhase.Text);
            Save(intPhase);
            oDesign.Next(intID, true);
            Redirect(intID, intForecast, "");
        }
        protected void btnUpdate_Click(Object Sender, EventArgs e)
        {
            int intPhase = Int32.Parse(lblPhase.Text);
            Save(intPhase);
            Redirect(intID, intForecast, "");
        }
        protected void btnReturn_Click(Object Sender, EventArgs e)
        {
            Redirect(intID, intForecast, "");
        }
        protected void btnExecute_Click(Object Sender, EventArgs e)
        {
            oLog.AddEvent(intID, "", "", "Design " + intID.ToString() + " was executed by " + oUser.GetFullNameWithLanID(intProfile), LoggingType.Information);
            intAnswer = oDesign.Approve(intID, intProfile, false, intEnvironment, intImplementorDistributedService, intWorkstationPlatform, intImplementorMidrangeService, dsnAsset, dsnIP, dsnServiceEditor, intAssignPage, intResourceRequestApprove, intViewPage);
            if (intAnswer == 0)
                Redirect(intID, intForecast, "&assignment=true");
            else
                Redirect(intID, intForecast, "");
        }
        private void Save(int _phaseid)
        {
            // Setup Form Variables
            List<List<string>> listValues = new List<List<string>>();
            int intListCounter = 0;
            foreach (string strForm in Request.Form)
            {
                // HDN_classid_7
                if (strForm.StartsWith("HDN_") == true)
                {
                    string strField = strForm.Substring(4);
                    string strValue = Request.Form[strForm];
                    int intResponse = 0;
                    int intQuestion = -1;
                    if (strValue.Contains("_") == true)
                        Int32.TryParse(strValue.Substring(0, strValue.IndexOf("_")), out intResponse);
                    if (intResponse > 0)
                        Int32.TryParse(oDesign.GetResponse(intResponse, "questionid"), out intQuestion);
                    listValues.Add(new List<string>());
                    listValues[intListCounter].Add(strField);
                    listValues[intListCounter].Add(strValue);
                    listValues[intListCounter].Add(intQuestion.ToString());
                    intListCounter++;
                    //oDesign.AddValue(intID, intForecast, _phaseid, intQuestion, strField, strValue);
                }
            }

            DataSet dsQuestions = oDesign.GetQuestions(_phaseid, 1);
            foreach (DataRow drQuestion in dsQuestions.Tables[0].Rows)
//            DataRow[] drQuestions = oDesign.LoadQuestions(intID, _phaseid);
//            foreach (DataRow drQuestion in drQuestions)
            {
                int intQuestion = Int32.Parse(drQuestion["id"].ToString());
                bool boolShowAll = (drQuestion["show_all"].ToString() == "1");
                bool boolShowAny = (drQuestion["show_any"].ToString() == "1");
                //bool boolHideAll = (drQuestion["hide_all"].ToString() == "1");
                //bool boolHideAny = (drQuestion["hide_any"].ToString() == "1");
                bool boolSelectedOne = false;
                bool boolSelectedAll = true;   // Will set to FALSE if one of them is not selected.
                bool boolHidden = false;

                // Get all responses that could affect this question's visibility
                DataSet dsShow = oDesign.GetShows(intQuestion, 0);
                if (dsShow.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow drShow in dsShow.Tables[0].Rows)
                    {
                        // One or more responses have an affect on this question's visibility
                        // ALL responses must be selected for it to be shown / saved
                        int intShowResponse = Int32.Parse(drShow["responseid"].ToString());
                        int intShowQuestion = 0;
                        if (Int32.TryParse(oDesign.GetResponse(intShowResponse, "questionid"), out intShowQuestion) == true)
                        {
                            if (oDesign.IsResponseVisible(intID, intShowResponse, true) == true && oDesign.IsResponseSelected(intID, intShowResponse) == true)
                            {
                                boolSelectedOne = true;
                                // This response is selected - if ANY of these can be selected for it to be shown, break and continue.
                                if (boolShowAny == true)
                                    break;
                            }
                            else if (boolShowAll == true)
                            {
                                // The selected response does not match causing this response to be hidden.
                                boolHidden = true;
                                break;
                            }
                            //if (oDesign.IsResponseVisible(intID, intShowResponse, true) == true && oDesign.IsResponseSelected(intID, intShowResponse) == false)
                            //{
                            //    // The selected response does not match causing this response to be hidden.
                            //    boolHidden = true;
                            //    break;
                            //}
                        }
                    }
                    if (boolShowAny && boolSelectedOne)
                        boolHidden = false;
                }
                if (boolHidden == false)
                {
                    // See if any of the question's responses has an affect on any other question's visibility
                    DataSet dsRelated = oDesign.GetShow(intQuestion, 0);
                    foreach (DataRow drRelated in dsRelated.Tables[0].Rows)
                    {
                        // This question has one or more responses that affect another question's visibility
                        // Get the response that SHOWS the question
                        int intResponse = Int32.Parse(drRelated["responseid"].ToString());
                        // Check to see if this response is selected (if not, we will need to clear the question's visibility)
                        if (oDesign.IsResponseSelected(intID, intResponse) == false)
                        {
                            // Clear responses for the question
                            // NOTE: This will only work for questions that have already been saved....since the above IsSelected function will check
                            //       on the CURRENT dataset, and since this has not saved the current dataset, it could be outdated.
                            oDesign.ClearResponses(intID, Int32.Parse(drRelated["questionid"].ToString()));
                        }
                    }
                    bool boolNew = (intID == 0);
                    // Update Data
                    string strField = drQuestion["related_field"].ToString();
                    string strSpecial = oDesign.GetQuestionSpecial(intQuestion);
                    switch (strSpecial)
                    {
                        case "MNEMONIC":
                            int intMnemonic = 0;
                            Int32.TryParse(Request.Form[hdnMnemonic.UniqueID], out intMnemonic);
                            intID = oDesign.AddValue(intID, intForecast, _phaseid, intQuestion, strField, intMnemonic.ToString());
                            break;
                        case "COST_CENTER":
                            int intCostCenter = 0;
                            Int32.TryParse(Request.Form[hdnCostCenter.UniqueID], out intCostCenter);
                            intID = oDesign.AddValue(intID, intForecast, _phaseid, intQuestion, strField, intCostCenter.ToString());
                            break;
                        case "USER_SI":
                            int intSI = 0;
                            Int32.TryParse(Request.Form[hdnSI.UniqueID], out intSI);
                            intID = oDesign.AddValue(intID, intForecast, _phaseid, intQuestion, strField, intSI.ToString());
                            break;
                        case "USER_DTG":
                            int intDTG = 0;
                            Int32.TryParse(Request.Form[hdnDTG.UniqueID], out intDTG);
                            intID = oDesign.AddValue(intID, intForecast, _phaseid, intQuestion, strField, intDTG.ToString());
                            break;
                        case "GRID_BACKUP":
                            oDesign.AddBackup(intID, Request);
                            break;
                        case "BACKUP_EXCLUSION":
                            // Nothing to do
                            break;
                        case "GRID_MAINTENANCE":
                            oDesign.AddMaintenance(intID, Request);
                            break;
                        case "STORAGE_LUNS":
                            // Save APP Drive
                            if (trStorageApp.Visible == true)
                            {
                                if (btnStorageSaveSizeE.CommandName == "")
                                    oDesign.AddLun(intID, -1000, "", txtStorageSizeE.Text, false);
                                else
                                    oDesign.UpdateLun(Int32.Parse(btnStorageSaveSizeE.CommandName), -1000, "", txtStorageSizeE.Text, false);
                            }
                            // Save NEW Drive
                            if (trStorageDrive.Visible == true)
                            {
                                int intDrive = 0;
                                if (ddlStorageDriveNew.Items.Count > 0)
                                    Int32.TryParse(ddlStorageDriveNew.SelectedItem.Value, out intDrive);
                                if (btnStorageSaveNew.CommandName == "")
                                    oDesign.AddLun(intID, intDrive, txtStoragePathNew.Text, txtStorageSizeNew.Text, chkStorageSizeNew.Checked);
                                else
                                    oDesign.UpdateLun(Int32.Parse(btnStorageSaveNew.CommandName), intDrive, txtStoragePathNew.Text, txtStorageSizeNew.Text, chkStorageSizeNew.Checked);
                            }
                            // Save all the other storage luns
                            foreach (RepeaterItem ri in rptStorage.Items)
                            {
                                DropDownList oDrive = (DropDownList)ri.FindControl("ddlStorageDrive");
                                int intDrive = 0;
                                if (oDrive.Items.Count > 0)
                                    Int32.TryParse(oDrive.SelectedItem.Value, out intDrive);
                                ImageButton oSave = (ImageButton)ri.FindControl("btnStorageSave");
                                TextBox oPath = (TextBox)ri.FindControl("txtStoragePath");
                                TextBox oSize = (TextBox)ri.FindControl("txtStorageSize");
                                CheckBox oShared = (CheckBox)ri.FindControl("chkStorageSize");
                                oDesign.UpdateLun(Int32.Parse(oSave.CommandName), intDrive, oPath.Text, oSize.Text, oShared.Checked);
                            }
                            break;
                        case "ACCOUNTS":
                            oDesign.UpdateAccountEmail(intID, (chkNotify.Checked ? 1 : 0));
                            break;
                        case "DATE":
                            intID = oDesign.AddValue(intID, intForecast, _phaseid, intQuestion, strField, txtDate.Text);
                            break;
                        case "LOCATION":
                            intID = oDesign.AddValue(intID, intForecast, _phaseid, intQuestion, strField, Request.Form[hdnLocation.UniqueID]);
                            break;
                        default:
                            for (int ii = 0; ii < intListCounter; ii++)
                            {
                                int _questionid = Int32.Parse(listValues[ii][2]);
                                if (_questionid < 0 || _questionid == intQuestion)
                                    intID = oDesign.AddValue(intID, intForecast, _phaseid, intQuestion, listValues[ii][0], listValues[ii][1]);
                            }
                            //foreach (string strForm in Request.Form)
                            //{
                            //    // HDN_classid_7
                            //    if (strForm.StartsWith("HDN_") == true)
                            //    {
                            //        strField = strForm.Substring(4);
                            //        string strValue = Request.Form[strForm];
                            //        oDesign.AddValue(intID, intForecast, _phaseid, intQuestion, strField, strValue);
                            //    }
                            //}
                            break;
                    }
                    int intMnemonicDR = 0;
                    if (intID > 0 && oDesign.Get(intID, "dr") == "" && Int32.TryParse(oDesign.Get(intID, "mnemonicid"), out intMnemonicDR) == true && intMnemonicDR > 0)
                    {
                        // Set DR Recovery from Mnemonic
                        int intHours = oMnemonic.GetResRatingHRs(intMnemonicDR);
                        oDesign.AddValue(intID, intForecast, 0, 0, "dr", (intHours <= 48 ? "1" : "0"));
                    }
                }
                else
                {
                    // This question is no longer visible...clear all the responses for the question.
                    oDesign.ClearResponses(intID, intQuestion);
                }
            }

            //// Check all selected responses for updates to global fields
            //// LOCATION
            //int intLocation = 0;
            //DataSet dsLocation = oDesign.GetResponseLocations();
            //foreach (DataRow drLocation in dsLocation.Tables[0].Rows)
            //{
            //    int intResponse = Int32.Parse(drLocation["id"].ToString());
            //    int intQuestion = Int32.Parse(drLocation["questionid"].ToString());
            //    if (oDesign.IsResponseSelected(intID, intResponse) == true)
            //    {
            //        // One or more of the locations have been selected....set the intLocation record for updating
            //        intLocation = Int32.Parse(drLocation["set_addressid"].ToString());
            //        break;
            //    }
            //}
            //oDesign.AddValue(intID, intForecast, 0, 0, "addressid", intLocation.ToString());
            //// MODEL
            //int intModel = 0;
            //int intDesignModel = 0;
            //DataSet dsModel = oDesign.GetResponseModels();
            //foreach (DataRow drModel in dsModel.Tables[0].Rows)
            //{
            //    int intResponse = Int32.Parse(drModel["id"].ToString());
            //    int intQuestion = Int32.Parse(drModel["questionid"].ToString());
            //    if (oDesign.IsResponseSelected(intID, intResponse) == true)
            //    {
            //        // One or more of the models have been selected....set the model
            //        intDesignModel = oDesign.GetModelPriority(intModel, Int32.Parse(drModel["set_modelid"].ToString()));
            //        if (intDesignModel > 0)
            //            Int32.TryParse(oDesign.GetModel(intDesignModel, "modelid"), out intModel);
            //        break;
            //    }
            //}
            //oDesign.AddValue(intID, intForecast, 0, 0, "design_modelid", intDesignModel.ToString());
            //oDesign.AddValue(intID, intForecast, 0, 0, "modelid", intModel.ToString());

            oDesign.AddPhaseCompletion(intID, _phaseid, intProfile);
        }


        // Backup Exclusions
        protected void btnAddExclusion_Click(Object Sender, EventArgs e)
        {
            oDesign.AddExclusion(intID, txtPath.Text);
            Redirect("");
        }
        protected void btnDeleteExclusion_Click(Object Sender, EventArgs e)
        {
            LinkButton oDelete = (LinkButton)Sender;
            oDesign.DeleteExclusions(Int32.Parse(oDelete.CommandArgument));
            Redirect("");
        }


        // Storage
        protected void btnStorageDriveAdd_Click(Object Sender, EventArgs e)
        {
            trStorageDrive.Visible = true;
            tdStorageDrive.Visible = oDesign.IsWindows(intID);
            txtStorageSizeNew.Text = "0";
            trStorageDriveNew.Visible = false;
            chkStorageSizeNew.Enabled = (oDesign.IsCluster(intID) == true);
            strStorageDisplay = "inline";
        }
        protected void btnStorageSave_Click(Object Sender, ImageClickEventArgs e)
        {
            string strInvalidPath = "";
            ImageButton oButton = (ImageButton)Sender;
            if (oButton.CommandArgument == "APP")
            {
                if (oButton.CommandName == "")
                    oDesign.AddLun(intID, -1000, "", txtStorageSizeE.Text, false);
                else
                    oDesign.UpdateLun(Int32.Parse(oButton.CommandName), -1000, "", txtStorageSizeE.Text, false);
            }
            else if (oButton.CommandArgument == "NEW")
            {
                if (oDesign.IsWindows(intID))
                {
                    string checkPath = ddlStorageDriveNew.SelectedItem.Text + txtStoragePathNew.Text;
                    if (Regex.IsMatch(checkPath, @"^(?:[a-zA-Z]\:|\\\\[\w\.]+\\[\w.$]+)\\(?:[\w]+\\)*\w([\w.])+$") == false)
                        strInvalidPath = checkPath;
                }
                if (String.IsNullOrEmpty(strInvalidPath))
                {
                    int intDrive = 0;
                    if (ddlStorageDriveNew.Items.Count > 0)
                        Int32.TryParse(ddlStorageDriveNew.SelectedItem.Value, out intDrive);
                    if (oButton.CommandName == "")
                        oDesign.AddLun(intID, intDrive, txtStoragePathNew.Text, txtStorageSizeNew.Text, chkStorageSizeNew.Checked);
                    else
                        oDesign.UpdateLun(Int32.Parse(oButton.CommandName), intDrive, txtStoragePathNew.Text, txtStorageSizeNew.Text, chkStorageSizeNew.Checked);
                    trStorageDrive.Visible = false;
                    trStorageDriveNew.Visible = true;
                }
            }
            else if (oButton.CommandArgument == "SAVE")
            {
                foreach (RepeaterItem ri in rptStorage.Items)
                {
                    ImageButton oSave = (ImageButton)ri.FindControl("btnStorageSave");
                    if (oButton.CommandName == oSave.CommandName)
                    {
                        DropDownList oDrive = (DropDownList)ri.FindControl("ddlStorageDrive");
                        int intDrive = 0;
                        if (oDrive.Items.Count > 0)
                            Int32.TryParse(oDrive.SelectedItem.Value, out intDrive);
                        TextBox oPath = (TextBox)ri.FindControl("txtStoragePath");
                        TextBox oSize = (TextBox)ri.FindControl("txtStorageSize");
                        CheckBox oShared = (CheckBox)ri.FindControl("chkStorageSize");
                        if (oDesign.IsWindows(intID))
                        {
                            string checkPath = oDrive.SelectedItem.Text + oPath.Text;
                            if (Regex.IsMatch(checkPath, @"^(?:[a-zA-Z]\:|\\\\[\w\.]+\\[\w.$]+)\\(?:[\w]+\\)*\w([\w.])+$") == false)
                                strInvalidPath = oPath.Text;
                        }
                        if (String.IsNullOrEmpty(strInvalidPath))
                        {
                            oDesign.UpdateLun(Int32.Parse(oButton.CommandName), intDrive, oPath.Text, oSize.Text, oShared.Checked);
                        }
                        break;
                    }
                }
            }
            else if (oButton.CommandArgument == "DELETE")
            {
                oDesign.DeleteLun(Int32.Parse(oButton.CommandName));
            }
            else
            {
                // CANCEL the insert
                trStorageDrive.Visible = false;
                trStorageDriveNew.Visible = true;
            }
            strStorageDisplay = "inline";
            // Refresh the storage total in the navigation
            LoadNavigation(Int32.Parse(lblPhase.Text));
            // Refresh the storage repeater
            LoadStorage(intID);
            if (String.IsNullOrEmpty(strInvalidPath) == false)
                Page.ClientScript.RegisterStartupScript(typeof(Page), "checkPath", "alert('The following path is invalid:\\n" + strInvalidPath.Replace("\\", "\\\\").Replace("'", "\\'") + "\\n\\nPlease enter a valid windows path.\\nFor example: \\\\this\\\\is\\\\valid');", true);
        }
        protected void btnGenerate_Click(Object Sender, EventArgs e)
        {
            double dblSize = 0.00;
            double dblNon = 0.00;
            double dblPercent = 0.00;
            double dblTempDB = 0.00;

            double.TryParse(txtDatabaseSize.Text, out dblSize);
            if (radDatabaseNonYes.Checked)
                double.TryParse(txtDatabaseNon.Text, out dblNon);
            double.TryParse(txtDatabasePercent.Text, out dblPercent);
            if (radTempDBYes.Checked)
                double.TryParse(txtDatabaseTemp.Text, out dblTempDB);
            oDesign.AddLunSQLPNC(intID, dblSize, dblNon, dblPercent, dblTempDB, dblCompressionPercentage, dblTempDBOverhead, true, oDesign.IsCluster(intID));
            strStorageDisplay = "inline";
            // Refresh the storage total in the navigation
            LoadNavigation(Int32.Parse(lblPhase.Text));
            // Refresh the storage repeater
            LoadStorage(intID);
        }
        protected void btnReset_Click(Object Sender, EventArgs e)
        {
            oDesign.DeleteLuns(intID, 1);
            strStorageDisplay = "inline";
            // Refresh the storage total in the navigation
            LoadNavigation(Int32.Parse(lblPhase.Text));
            // Refresh the storage repeater
            LoadStorage(intID);
        }
        private void LoadStorage(int _designid)
        {
            bool boolDatabase = oDesign.IsDatabase(_designid);
            bool boolSQL = oDesign.IsSQL(_designid);
            bool boolCluster = oDesign.IsCluster(_designid);

            trStorageDriveNew.Visible = (btnReturn.Visible == false);
            btnStorageSaveSizeE.Visible = (btnReturn.Visible == false);
            

            panStorageDatabase.Visible = false;
            panLUNs.Visible = false;
            btnGenerate.Visible = false;
            btnReset.Visible = false;

            if (boolDatabase && boolSQL)
            {
                panStorageDatabase.Visible = true;
                divDatabaseNon.Style["display"] = (radDatabaseNonYes.Checked ? "inline" : "none");
                divTempDB.Style["display"] = (radTempDBYes.Checked ? "inline" : "none");
                btnGenerate.Visible = (btnReturn.Visible == false);
            }
            else
                panLUNs.Visible = true;

            DataSet dsStorage = oDesign.GetStorageDrives(_designid);
            // First Check to be sure database is correct
            bool boolStorageExists = false;
            int intTotal = 0;
            foreach (DataRow drStorage in dsStorage.Tables[0].Rows)
            {
                int intDrive = 0;
                if (Int32.TryParse(drStorage["driveid"].ToString(), out intDrive) == true)
                {
                    boolStorageExists = true;
                    if (intDrive > 0)
                    {
                        int intDB = -1;
                        if (drStorage["db"].ToString() == "1" && boolDatabase == true && boolSQL == false)
                            intDB = 1;
                        if (drStorage["db"].ToString() == "0" && boolDatabase == true && boolSQL == true)
                            intDB = 0;
                        if (intDB >= 0)
                        {
                            oDesign.DeleteLuns(_designid, intDB);
                            Redirect("&cleanup=true");
                        }
                    }
                }
            }
            if (boolStorageExists == true && boolDatabase && boolSQL)
            {
                panStorageDatabase.Visible = false;
                btnGenerate.Visible = false;
                btnReset.Visible = (btnReturn.Visible == false);
                panLUNs.Visible = true;
            }
            DataSet dsDrives = oDesign.GetDrives();
            rptStorage.DataSource = dsStorage;
            rptStorage.DataBind();
            foreach (RepeaterItem ri in rptStorage.Items)
            {
                ImageButton _delete = (ImageButton)ri.FindControl("btnStorageDelete");
                _delete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this LUN?') && LoadWait();");
                _delete.Visible = (btnReturn.Visible == false);
                ImageButton _save = (ImageButton)ri.FindControl("btnStorageSave");
                _save.Attributes.Add("onclick", "return LoadWait();");
                _save.Visible = (btnReturn.Visible == false);
                CheckBox _shared = (CheckBox)ri.FindControl("chkStorageSize");
                _shared.Enabled = (boolCluster == true && btnReturn.Visible == false);
                _shared.Checked = (_shared.Text == "1");
                _shared.Text = "";
                TextBox _size = (TextBox)ri.FindControl("txtStorageSize");
                TextBox _path = (TextBox)ri.FindControl("txtStoragePath");
                _size.Attributes.Add("onblur", "ChangeLUN('" + _size.Text + "','" + _path.Text.Replace("\\", "\\\\") + "','" + _size.ClientID + "','" + _path.ClientID + "');");
                _path.Attributes.Add("onblur", "ChangeLUN('" + _size.Text + "','" + _path.Text.Replace("\\", "\\\\") + "','" + _size.ClientID + "','" + _path.ClientID + "');");
                DropDownList _drive = (DropDownList)ri.FindControl("ddlStorageDrive");
                if (!IsPostBack || _drive.Items.Count == 0)
                {
                    _drive.DataValueField = "id";
                    _drive.DataTextField = "path";
                    _drive.DataSource = dsDrives;
                    _drive.DataBind();
                }
                _drive.SelectedValue = oDesign.GetStorageID(Int32.Parse(_save.CommandName), "driveid");
            }
            if (boolWindows)
            {
                trStorageApp.Visible = true;
                DataSet dsApp = oDesign.GetStorageDrive(_designid, -1000);
                int intAppDrive = 0;
                if (dsApp.Tables[0].Rows.Count > 0)
                {
                    if (!IsPostBack || txtStorageSizeE.Text == "")
                    {
                        if (Int32.TryParse(dsApp.Tables[0].Rows[0]["size"].ToString(), out intAppDrive) == true)
                        {
                            txtStorageSizeE.Text = intAppDrive.ToString();
                            txtStorageSizeE.Attributes.Add("onblur", "ChangeLUN('" + txtStorageSizeE.Text + "','','" + txtStorageSizeE.ClientID + "',null);");
                            intTotal += intAppDrive;
                        }
                    }
                    btnStorageSaveSizeE.CommandName = dsApp.Tables[0].Rows[0]["id"].ToString();
                }
                if (intAppDrive == 0)
                    txtStorageSizeE.BackColor = ColorTranslator.FromHtml("#ffa0a0");
                //txtStorageSizeE.Style["bgcolor"] = "#FFEE99";
                btnStorageSaveSizeE.Attributes.Add("onclick", "return LoadWait();");
            }
            if (panLUNs.Visible == true)
                AddValidation("CheckLUNs('DIV" + intStorageQuestion.ToString() + "')");
            btnStorageSaveNew.Attributes.Add("onclick", "return LoadWait();");
            btnStorageDeleteNew.Attributes.Add("onclick", "return LoadWait();");
            btnStorageDriveAdd.Attributes.Add("onclick", "return LoadWait();");
        }


        // Accounts
        protected void btnAddAccount_Click(Object Sender, EventArgs e)
        {
            int intAccount = 0;
            if (Int32.TryParse(Request.Form[hdnAccount.UniqueID], out intAccount) == true)
            {
                if (oDesign.AddAccount(intID, intAccount, ddlPermission.SelectedItem.Value, (ddlPermission.SelectedItem.Value != "U" && chkRemoteDesktop.Checked ? 1 : 0)) == true)
                    Redirect("");
                else
                    Redirect("&duplicate=" + oUser.GetName(intAccount));
            }
        }
        protected void btnDeleteAccount_Click(Object Sender, EventArgs e)
        {
            LinkButton oDelete = (LinkButton)Sender;
            oDesign.DeleteAccount(Int32.Parse(oDelete.CommandArgument));
            Redirect("");
        }

        protected void btnSubmit_Click(Object Sender, EventArgs e)
        {
            if (radComplete.Checked == true)
            {
                bool boolException = oDesign.IsOther(intID, 0, 0, 1);
                oLog.AddEvent(intID, "", "", "Design " + intID.ToString() + " was completed by " + oUser.GetFullNameWithLanID(intProfile) + " -> " + (boolException ? "exception" : "standard"), LoggingType.Information);
                oDesign.UpdateException(intID, boolException);
                oDesign.UpdateExceptionDone(intID, false);
                // Clear the rejection(s)
                oDesign.UpdateApproverFieldWorkflows(intID);
                oDesign.UpdateApproverGroupWorkflows(intID);
                oDesign.UpdateSoftwareComponentWorkflows(intID);
                // Sunmit
                oDesign.AddSubmitted(intID, intProfile, txtException.Text, txtExceptionID.Text);
                intAnswer = oDesign.Approve(intID, intProfile, boolException, intEnvironment, intImplementorDistributedService, intWorkstationPlatform, intImplementorMidrangeService, dsnAsset, dsnIP, dsnServiceEditor, intAssignPage, intResourceRequestApprove, intViewPage);
                Redirect(intAnswer > 0 ? "&aid=" + intAnswer.ToString() : "");
            }
            else if (radChange.Checked == true)
            {
                Response.Redirect(Request.Path + "?id=" + intID.ToString() + "&phase=1");
            }
            else if (radException.Checked == true)
            {
                oLog.AddEvent(intID, "", "", "Design " + intID.ToString() + " was requested for exception by " + oUser.GetFullNameWithLanID(intProfile), LoggingType.Information);
                oDesign.UpdateException(intID, true);
                oDesign.UpdateExceptionDone(intID, false);
                // Clear the rejection(s)
                oDesign.UpdateApproverFieldWorkflows(intID);
                oDesign.UpdateApproverGroupWorkflows(intID);
                oDesign.UpdateSoftwareComponentWorkflows(intID);
                // Delete the exception request, since we are requesting another exception
                oDesign.DeleteApproverGroupWorkflow(intID, 1);
                // Sunmit
                oDesign.AddSubmitted(intID, intProfile, txtException.Text, txtExceptionID.Text);
                intAnswer = oDesign.Approve(intID, intProfile, true, intEnvironment, intImplementorDistributedService, intWorkstationPlatform, intImplementorMidrangeService, dsnAsset, dsnIP, dsnServiceEditor, intAssignPage, intResourceRequestApprove, intViewPage);
                Redirect(intAnswer > 0 ? "&aid=" + intAnswer.ToString() : "");
            }
            else
                Page.ClientScript.RegisterStartupScript(typeof(Page), "selection", "<script type=\"text/javascript\">alert('Make a selection from the choices at the bottom of the screen');<" + "/" + "script>");
        }
        protected void btnReview_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(Request.Path + "?id=" + intID.ToString() + "&phase=1");
        }


        // DEMO STUFF

        protected void btnChange_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(Request.Path + "?id=" + intID.ToString() + "&phase=1");
        }
        private void Redirect(string _additional)
        {
            if (Request.QueryString["phase"] != null)
                Response.Redirect(Request.Path + "?id=" + intID.ToString() + "&phase=" + Request.QueryString["phase"] + _additional);
            else
                Response.Redirect(Request.Path + "?id=" + intID.ToString() + "&parent=" + intForecast.ToString() + _additional);
        }
        private void Redirect(int _id, int _forecastid, string _additional)
        {
            Response.Redirect(Request.Path + "?id=" + _id.ToString() + "&parent=" + _forecastid.ToString() + "&saved=true" + _additional);
        }
    }
}
