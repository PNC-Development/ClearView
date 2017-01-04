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
namespace NCC.ClearView.Presentation.Web
{
    public partial class ondemand_server_control : System.Web.UI.UserControl
    {

        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intImplementorDistributed = Int32.Parse(ConfigurationManager.AppSettings["ITEMID_IMPLEMENTOR_DISTRIBUTED"]);
        protected int intImplementorMidrange = Int32.Parse(ConfigurationManager.AppSettings["ITEMID_IMPLEMENTOR_MIDRANGE"]);
        protected int intWorkstationPlatform = Int32.Parse(ConfigurationManager.AppSettings["WorkstationPlatformID"]);
        protected int intProfile;
        protected int intApplication = 0;
        protected int intPage = 0;
        protected Asset oAsset;
        protected Forecast oForecast;
        protected Servers oServer;
        protected ServerName oServerName;
        protected ServiceRequests oServiceRequest;
        protected ModelsProperties oModelsProperties;
        protected Models oModel;
        protected Requests oRequest;
        protected Classes oClass;
        protected Users oUser;
        protected Functions oFunction;
        protected OnDemandTasks oOnDemandTasks;
        protected ResourceRequest oResourceRequest;
        protected Projects oProject;
        protected Settings oSetting;
        protected int intID = 0;
        protected int intRequest = 0;
        protected string strMenuTab1 = "";
        protected string strDivs = "";
        protected bool boolBurnIn = false;
        protected string strManualImage = "";
        protected string strMenuTabApprove = "";
        protected string strApprove = "";
        protected string strManualReason = "";
        protected int intDeviceCount = 1;
        protected string strFreezeStart = "";
        protected string strFreezeEnd = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title = "ClearView Auto-Provisioning";
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oAsset = new Asset(intProfile, dsnAsset);
            oForecast = new Forecast(intProfile, dsn);
            oServer = new Servers(intProfile, dsn);
            oServerName = new ServerName(intProfile, dsn);
            oServiceRequest = new ServiceRequests(intProfile, dsn);
            oModelsProperties = new ModelsProperties(intProfile, dsn);
            oModel = new Models(intProfile, dsn);
            oRequest = new Requests(intProfile, dsn);
            oClass = new Classes(intProfile, dsn);
            oUser = new Users(intProfile, dsn);
            oFunction = new Functions(intProfile, dsn, intEnvironment);
            oOnDemandTasks = new OnDemandTasks(intProfile, dsn);
            oResourceRequest = new ResourceRequest(intProfile, dsn);
            oProject = new Projects(intProfile, dsn);
            oSetting = new Settings(intProfile, dsn);

            //Menu
            int intMenuTab = 0;
            if (Request.QueryString["menu_tab"] != null && Request.QueryString["menu_tab"] != "")
                intMenuTab = Int32.Parse(Request.QueryString["menu_tab"]);
            Tab oTab = new Tab("", intMenuTab, "divMenu1", true, true);
            //End Menu

            if (Request.QueryString["applicationid"] != null && Request.QueryString["applicationid"] != "")
                intApplication = Int32.Parse(Request.QueryString["applicationid"]);
            if (Request.QueryString["pageid"] != null && Request.QueryString["pageid"] != "")
                intPage = Int32.Parse(Request.QueryString["pageid"]);
            if (Request.Cookies["application"] != null && Request.Cookies["application"].Value != "")
                intApplication = Int32.Parse(Request.Cookies["application"].Value);
            if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
            {
                intID = Int32.Parse(Request.QueryString["id"]);
                intRequest = oForecast.GetRequestID(intID, true);
                DataSet dsRequest = oServiceRequest.Get(intRequest);
                if (dsRequest.Tables[0].Rows.Count > 0)
                    Response.Redirect(Request.Path + "?rid=" + intRequest);
            }
            if (Request.QueryString["rid"] != null && Request.QueryString["rid"] != "")
                intRequest = Int32.Parse(Request.QueryString["rid"]);
            if (Request.QueryString["notify"] != null)
            {
                string strRedirect = Request.Url.PathAndQuery;
                strRedirect = strRedirect.Substring(0, strRedirect.IndexOf("&notify"));
                if (Request.QueryString["notify"] == "none")
                    Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "close", "<script type=\"text/javascript\">window.navigate('" + strRedirect + "');<" + "/" + "script>");
                else if (Request.QueryString["notify"] == "true")
                    Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "close", "<script type=\"text/javascript\">alert('Your design implementor has been successfully notified!');window.navigate('" + strRedirect + "');<" + "/" + "script>");
                else if (Request.QueryString["notify"] == "false")
                    Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "close", "<script type=\"text/javascript\">alert('Your design implementor has ALREADY been notified!');window.navigate('" + strRedirect + "');<" + "/" + "script>");
                else
                    Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "close", "<script type=\"text/javascript\">alert('There was a problem notifying your design implementor\\n\\nPlease contact your ClearView administrator\\n\\nRequestID: " + intRequest.ToString() + "');window.navigate('" + strRedirect + "');<" + "/" + "script>");
            }
            if (intID > 0)
            {
                Page.Title = "ClearView Auto-Provisioning | Design # " + intID.ToString();
                btnPrint.Attributes.Add("onclick", "return OpenWindow('DESIGN_PRINT', '?id=" + intID.ToString() + "');");
                bool boolApproveNeeded = false;
                bool boolApproved = false;
                bool boolPending = false;
                bool boolDenied = false;
                Tab oTabServer = new Tab("", 0, "divMenuApprove", true, false);
                DataSet dsServers = oServer.GetAnswer(intID);
                int intServerCount = 0;
                foreach (DataRow drServer in dsServers.Tables[0].Rows)
                {
                    int intServer = Int32.Parse(drServer["id"].ToString());
                    intServerCount++;
                    DataSet dsApprove = oServerName.GetComponentDetailUserApprovalsByServer(intServer);
                    if (dsApprove.Tables[0].Rows.Count > 0)
                    {
                        boolApproveNeeded = true;
                        oTabServer.AddTab("Device # " + intServerCount.ToString(), "");
                        string strApproveItem = "";
                        bool boolOther = true;
                        foreach (DataRow drApprove in dsApprove.Tables[0].Rows) 
                        {
                            boolOther = (boolOther == false);
                            int intDetail = Int32.Parse(drApprove["detailid"].ToString());
                            int intApprover = 0;
                            string strApproverComments = "";
                            int intApproverApproved = -1;
                            DateTime datApproved = DateTime.Now;
                            DataSet dsApprovals = oServerName.GetComponentDetailUserApprovals(intServer, intDetail);
                            if (dsApprovals.Tables[0].Rows.Count == 0)
                                boolPending = true;
                            else
                            {
                                intApprover = Int32.Parse(dsApprovals.Tables[0].Rows[0]["userid"].ToString());
                                strApproverComments = dsApprovals.Tables[0].Rows[0]["comments"].ToString();
                                intApproverApproved = Int32.Parse(dsApprovals.Tables[0].Rows[0]["approved"].ToString());
                                datApproved = DateTime.Parse(dsApprovals.Tables[0].Rows[0]["modified"].ToString());
                                if (intApproverApproved == 0)
                                    boolDenied = true;
                            }
                            string strApprovers = "";
                            DataSet dsApprovers = oServerName.GetComponentDetailUsers(intDetail, 1);
                            foreach (DataRow drApprover in dsApprovers.Tables[0].Rows)
                            {
                                int intUser = Int32.Parse(drApprover["userid"].ToString());
                                strApprovers += "<tr><td><a href=\"javascript:void(0);\" onclick=\"OpenWindow('PROFILE','?userid=" + intUser.ToString() + "');\"><img src=\"/images/" + (intApprover == 0 ? "pending" : (intUser == intApprover ? (intApproverApproved == 1 ? "check" : "cancel") : "user")) + ".gif\" border=\"0\" align=\"absmiddle\"/> " + oUser.GetFullName(intUser) + " (" + oUser.GetName(intUser) + ")</a></td></tr>";
                            }
                            strApprovers = "<table>" + strApprovers + "</table>";
                            strApproveItem += "<tr" + (boolOther ? " bgcolor=\"#F6F6F6\"" : "") + ">";
                            string strLabel = drApprove["name"].ToString();
                            if (intApproverApproved > -1)
                                strLabel = "<a href=\"javascript:void(0);\" onclick=\"ShowHideDiv2('trApproval" + intServerCount.ToString() + "_" + intDetail.ToString() + "');\">" + strLabel + "</a>";
                            strApproveItem += "<td valign=\"top\" width=\"45%\" class=\"biggerbold\">" + strLabel + "</td>";
                            strApproveItem += "<td valign=\"top\" width=\"10%\">" + (intApproverApproved == -1 ? "Pending" : (intApproverApproved == 0 ? "Denied" : "Approved")) + "</td>";
                            strApproveItem += "<td valign=\"top\" width=\"45%\" align=\"right\">" + strApprovers + "</td>";
                            strApproveItem += "</tr>";
                            if (intApproverApproved > -1)
                            {
                                string strApproveTable = "<tr><td colspan=\"2\"><span style=\"width:100%;border-bottom:1 dotted #CCCCCC;\"/></td></tr>";
                                strApproveTable += "<tr>";
                                strApproveTable += "<td nowrap valign=\"top\"><img src=\"/images/" + (intApproverApproved == 0 ? "cancel" : "check") + ".gif\" border=\"0\" align=\"absmiddle\"/></td>";
                                strApproveTable += "<td width=\"100%\" valign=\"top\">" + oUser.GetFullName(intApprover) + " " + (intApproverApproved == 0 ? "DENIED" : "APPROVED") + " this request on " + datApproved.ToString() + "</td>";
                                strApproveTable += "</tr>";
                                if (strApproverComments != "") 
                                {
                                    strApproveTable += "<tr>";
                                    strApproveTable += "<td colspan=\"2\">The following comments were added...</td>";
                                    strApproveTable += "</tr>";
                                    strApproveTable += "<tr>";
                                    strApproveTable += "<td nowrap valign=\"top\"><img src=\"/images/comment.gif\" border=\"0\" align=\"absmiddle\"/></td>";
                                    strApproveTable += "<td width=\"100%\" valign=\"top\" class=\"comment\">" + strApproverComments + "</td>";
                                    strApproveTable += "</tr>";
                                }
                                strApproveTable = "<table width=\"100%\" cellpadding=\"4\" cellspacing=\"3\" border=\"0\">" + strApproveTable + "</table>";
                                strApproveItem += "<tr id=\"trApproval" + intServerCount.ToString() + "_" + intDetail.ToString() + "\" style=\"display:none\"" + (boolOther ? " bgcolor=\"#F6F6F6\"" : "") + "><td colspan=\"3\">" + strApproveTable + "</td></tr>";
                            }
                        }
                        strApproveItem = "<tr bgcolor=\"#EEEEEE\"><td class=\"header\">Component</td><td class=\"header\">Status</td><td class=\"header\" align=\"right\">Approver(s)</td></tr>" + strApproveItem;
                        strApproveItem = "<table width=\"100%\" cellpadding=\"4\" cellspacing=\"0\" border=\"0\" style=\"border:solid 1px #CCCCCC\">" + strApproveItem + "</table>";
                        strApprove += "<div id=\"divTab" + intServerCount.ToString() + "\" style=\"display:none\">" + strApproveItem + "</div>";
                    }
                }
                if (boolApproveNeeded == true)
                {
                    trApprove.Visible = true;
                    if (boolDenied == true)
                    {
                        btnSoftwareBack.Visible = true;
                        imgApproved.ImageUrl = "/images/ico_error.gif";
                        lblApprovedHeader.Text = "Software Component(s) Denied";
                        lblApproved.Text = "One or more of the software components you requested have been denied. You cannot continue until all components have been approved.<br/>You will need to make changes to this design - please click <b>Make Changes</b> to change your software component requirements.<br/>All questions and status inquiries should be sent to the approver(s) listed below.";
                    }
                    else if (boolPending == true)
                    {
                        imgApproved.ImageUrl = "/images/ico_hourglass.gif";
                        lblApprovedHeader.Text = "Software Component(s) Pending";
                        lblApproved.Text = "One or more of the software components you requested are awaiting approval. You cannot continue until all components have been approved.<br/>All questions and status inquiries should be sent to the approver(s) listed below.";
                    }
                    else
                    {
                        boolApproved = true;
                        lblApprovedHeader.Text = "Software Component(s) Approved";
                        lblApproved.Text = "All of the software components you requested have been approved!";
                    }
                    btnApproved.Attributes.Add("onclick", "ShowHideDiv2('" + trApprove.ClientID + "');return false;");
                    strMenuTabApprove = oTabServer.GetTabs();
                }
                else
                {
                    boolApproved = true;
                    lblApprovedHeader.Text = "No Approval Required";
                    lblApproved.Text = "There are no approvals required for your software components";
                    btnApproved.Enabled = false;
                }

                string strSchedule = oForecast.GetAnswer(intID, "execution");
                if (strSchedule != "")
                {
                    DateTime datSchedule = DateTime.Parse(strSchedule);
                    txtScheduleDate.Text = datSchedule.ToShortDateString();
                    txtScheduleTime.Text = datSchedule.ToShortTimeString();
                    btnSchedule.Text = "Update the Build";
                }
                panBegin.Visible = true;
                bool boolAssigned = false;

                int intModel = oForecast.GetModelAsset(intID);
                if (intModel == 0)
                    intModel = oForecast.GetModel(intID);

                string strChange = "true";
                string strFreeze = "";
                // Check Freeze Dates
                bool boolFreeze = false;
                strFreezeStart = oSetting.Get("freeze_start");
                strFreezeEnd = oSetting.Get("freeze_end");
                if (strFreezeStart != "" && strFreezeEnd != "" && DateTime.Parse(strFreezeStart) <= DateTime.Now && DateTime.Parse(strFreezeEnd) > DateTime.Now)
                {
                    boolFreeze = true;
                    divChange.Style["display"] = "inline";
                    if (!IsPostBack)
                        txtChange.Text = oForecast.GetAnswer(intID, "change");
                    strChange = "ValidateTextLength('" + txtChange.ClientID + "', 'Please enter a valid change control number\\n\\n - Must start with \"CHG\" or \"PTM\"\\n - Must be exactly 10 characters in length', 10, 'CHG', 'PTM')";
                }
                int intClass = Int32.Parse(oForecast.GetAnswer(intID, "classid"));

                int intForecast = 0;
                Int32.TryParse(oForecast.GetAnswer(intID, "forecastid"), out intForecast);
                int intRequestID = 0;
                Int32.TryParse(oForecast.Get(intForecast, "requestid"), out intRequestID);
                int intProject = 0;
                if (intRequestID > 0)
                    intProject = oRequest.GetProjectNumber(intRequestID);
                int intProjectStatus = 0;
                if (intProject > 0)
                    Int32.TryParse(oProject.Get(intProject, "status"), out intProjectStatus);

                if (oForecast.CanAutoProvision(intID) == true)
                    boolAssigned = true;
                else
                {
                    if (oOnDemandTasks.GetPending(intID).Tables[0].Rows.Count > 0)
                        boolAssigned = true;
                    else
                    {
                        // Add Resource Request
                        int intImplementor = 0;
                        string strType = "Distributed";
                        DataSet dsResource = oResourceRequest.GetProjectItem(intProject, intImplementorDistributed);
                        if (oForecast.GetPlatformDistributed(intID, intWorkstationPlatform) == false)
                        {
                            dsResource = oResourceRequest.GetProjectItem(intProject, intImplementorMidrange);
                            strType = "Midrange";
                        }
                        if (dsResource.Tables[0].Rows.Count > 0)
                            intImplementor = (dsResource.Tables[0].Rows[0]["userid"].ToString() == "" ? 0 : Int32.Parse(dsResource.Tables[0].Rows[0]["userid"].ToString()));
                        if (intImplementor > 0)
                        {
                            int intNextNumber = oResourceRequest.GetNumber(intRequestID);
                            int intResourceParent = oResourceRequest.Add(intRequestID, -1, -1, intNextNumber, "Provisioning Task (" + strType + ")", 0, 6.00, 2, 1, 1, 1);
                            int intResourceWorkflow = oResourceRequest.AddWorkflow(intResourceParent, 0, "Provisioning Task (" + strType + ")", intImplementor, 0, 6.00, 2, 0);
                            oOnDemandTasks.AddPending(intID, intResourceWorkflow);
                            oResourceRequest.UpdateAssignedBy(intResourceParent, -999);
                        }
                    }
                }

                lblNotify.Visible = boolBurnIn;

                bool boolOKtoExecute = false;
                if (intProject == -100)
                {
                    // Pending project approval
                    btnStart.Attributes.Add("onclick", "alert('This design cannot be executed because the associated project is currently pending approval.\\n\\nMost likely, this is because you just entered a new project into the system\\n\\nPlease contact your ClearView administrator or check back later.');return false;");
                    btnSchedule.Attributes.Add("onclick", "alert('This design cannot be scheduled because the associated project is currently pending approval.\\n\\nMost likely, this is because you just entered a new project into the system\\n\\nPlease contact your ClearView administrator or check back later.');return false;");
                }
                else
                {
                    if (oForecast.CanAutoProvision(intID) == true)
                    {
                        btnSchedule.Enabled = true;
                        boolOKtoExecute = true;
                    }
                    else
                    {
                        if (boolAssigned == false)
                        {
                            btnStart.Attributes.Add("onclick", "alert('You cannot execute a design until a design implementor has been assigned.');return false;");
                            btnSchedule.Attributes.Add("onclick", "alert('You cannot schedule the execution of a design until a design implementor has been assigned.');return false;");
                        }
                        else
                            boolOKtoExecute = true;
                    }
                }

                imgScheduleDate.Attributes.Add("onclick", "return ShowCalendar('" + txtScheduleDate.ClientID + "');");
                if (boolOKtoExecute == true) 
                {
                    btnStart.Attributes.Add("onclick", "return " + strChange + " && confirm('NOTE: Billing will begin: " + DateTime.Today.ToShortDateString() + "\\n\\nAre you sure you want to continue?');");
                    btnSchedule.Attributes.Add("onclick", "return ValidateDate('" + txtScheduleDate.ClientID + "','Please enter a valid schedule date')" +
                        " && ValidateDateToday('" + txtScheduleDate.ClientID + "','The scheduled date must occur after today')" +
                        " && ValidateFreeze('" + txtScheduleDate.ClientID + "','" + divChange.ClientID + "','" + txtChange.ClientID + "'," + (boolFreeze ? "true" : "false") + ")" +
                        " && ValidateTime('" + txtScheduleTime.ClientID + "','Please enter a valid start time')" +
                        " && confirm('NOTE: Billing will begin: ' + document.getElementById('" + txtScheduleDate.ClientID + "').value + '\\n\\nAre you sure you want to continue?')" +
                        " && ProcessButton(this)" +
                        ";");
                }

                if (oModelsProperties.IsInventory(intModel) == true)
                {
                    panInventoryYes.Visible = true;
                    if (boolApproved == false)
                    {
                        divSoftware.Style["display"] = "inline";
                        divDefault.Style["display"] = "none";
                        radStart.Enabled = false;
                        btnStart.Enabled = false;
                        radSchedule.Enabled = false;
                        btnSchedule.Enabled = false;
                        radApproval.Enabled = false;
                        btnApprovals.Enabled = false;
                    }
                }
                else
                {
                    lblInventory.Text = oModelsProperties.Get(intModel, "name");
                    panInventoryNo.Visible = true;
                    divSoftware.Style["display"] = "none";
                    divDefault.Style["display"] = "none";
                    radStart.Enabled = false;
                    btnStart.Enabled = false;
                    radSchedule.Enabled = false;
                    btnSchedule.Enabled = false;
                    radApproval.Enabled = false;
                    btnApprovals.Enabled = false;
                }
            }
            else if (intRequest > 0)
            {
                intRequest = Int32.Parse(Request.QueryString["rid"]);
                DateTime datSchedule = DateTime.Now;
                DataSet ds = oServer.GetRequests(intRequest, 1);
                bool boolPending = false;
                int intAnswer = 0;
                int intModel = 0;
                if (ds.Tables[0].Rows.Count > 0)
                {
                    intAnswer = Int32.Parse(ds.Tables[0].Rows[0]["answerid"].ToString());
                    intModel = oForecast.GetModelAsset(intAnswer);
                    if (intModel == 0)
                        intModel = oForecast.GetModel(intAnswer);
                    string strSchedule = oForecast.GetAnswer(intAnswer, "execution");
                    if (strSchedule != "")
                    {
                        datSchedule = DateTime.Parse(strSchedule);
                        lblScheduleDate.Text = datSchedule.ToShortDateString();
                        lblScheduleTime.Text = datSchedule.ToShortTimeString();
                        if (DateTime.Now < datSchedule)
                            boolPending = true;
                    }
                }
                Page.Title = "ClearView Auto-Provisioning | Design # " + intAnswer.ToString();
                if (oForecast.CanAutoProvision(intAnswer) == false)
                {
                    panNotExecutable.Visible = true;
                    lblInitiated.Text = oForecast.GetAnswer(intAnswer, "executed");
                    lblCompleted.Text = oForecast.GetAnswer(intAnswer, "completed");
                    int intImplementorUser = 0;
                    DataSet dsImplementor = oOnDemandTasks.GetPending(intAnswer);
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
                else
                {
                    if (boolPending == true)
                    {
                        panPending.Visible = true;
                        TimeSpan oSpan = datSchedule.Subtract(DateTime.Now);
                        if (oSpan.Seconds > 0)
                            Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "cdown", "<script type=\"text/javascript\">StartClockCountdown('" + lblCountdown.ClientID + "'," + oSpan.TotalMilliseconds.ToString() + ");<" + "/" + "script>");
                        else
                            Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "cdown", "<script type=\"text/javascript\">StartClockCountdown('" + lblCountdown.ClientID + "',0);<" + "/" + "script>");
                    }
                    else
                    {
                        panStart.Visible = true;
                        int intCount = 0;
                        Types oType = new Types(intProfile, dsn);

                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            intCount++;
                            int intServer = Int32.Parse(dr["id"].ToString());
                            int intAsset = 0;
                            if (dr["assetid"].ToString() != "")
                                intAsset = Int32.Parse(dr["assetid"].ToString());
                            int intType = 0;
                            intModel = 0;
                            if (dr["modelid"].ToString() != "")
                                intModel = Int32.Parse(dr["modelid"].ToString());
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
                                //strTab += "<td><img src=\"/images/TabOnLeftCap.gif\"></td><td nowrap background=\"/images/TabOnBackground.gif\"><a href=\"javascript:void(0);\" onclick=\"ChangeTab(this,'divTab" + intCount.ToString() + "',null,null,true);\" class=\"tabheader\">" + strName + "</a></td><td><img src=\"/images/TabOnRightCap.gif\"></td>";
                                oTab.AddTab(strName, "");
                                strDivs += "<div id=\"divTab" + intCount.ToString() + "\" style=\"display:inline\"><iframe width=\"100%\" height=\"100%\" frameborder=\"0\" scrolling=\"auto\" src=\"" + oType.Get(intType, "ondemand_steps_path") + "?id=" + oFunction.encryptQueryString(intServer.ToString()) + "&c=" + intCount.ToString() + "\"></iframe></div>";
                            }
                            else
                            {
                                // strTab += "<td><img src=\"/images/TabOffLeftCap.gif\"></td><td nowrap background=\"/images/TabOffBackground.gif\"><a href=\"javascript:void(0);\" onclick=\"ChangeTab(this,'divTab" + intCount.ToString() + "',null,null,true);\" class=\"tabheader\">" + strName + "</a></td><td><img src=\"/images/TabOffRightCap.gif\"></td>";
                                oTab.AddTab(strName, "");
                                strDivs += "<div id=\"divTab" + intCount.ToString() + "\" style=\"display:none\"><iframe width=\"100%\" height=\"100%\" frameborder=\"0\" scrolling=\"auto\" src=\"" + oType.Get(intType, "ondemand_steps_path") + "?id=" + oFunction.encryptQueryString(intServer.ToString()) + "&c=" + intCount.ToString() + "\"></iframe></div>";
                            }
                        }

                        strMenuTab1 = oTab.GetTabs();
                        //if (strTab != "")
                        //    strMenuTab1 += "<tr>" + strTab + "<td width=\"100%\" background=\"/images/TabEmptyBackground.gif\">&nbsp;</td></tr>";
                        //strMenuTab1 = "<table cellpadding=\"0\" cellspacing=\"0\" border=\"0\">" + strMenuTab1 + "</table>";
                    }
                }
            }
            radStart.Attributes.Add("onclick", "ShowBuildDivs('" + divStart.ClientID + "','" + divDefault.ClientID + "','" + divSchedule.ClientID + "','" + divApproval.ClientID + "');");
            radSchedule.Attributes.Add("onclick", "ShowBuildDivs('" + divSchedule.ClientID + "','" + divDefault.ClientID + "','" + divStart.ClientID + "','" + divApproval.ClientID + "');");
            radApproval.Attributes.Add("onclick", "ShowBuildDivs('" + divApproval.ClientID + "','" + divDefault.ClientID + "','" + divStart.ClientID + "','" + divSchedule.ClientID + "');");
            btnApprovals.Attributes.Add("onclick", "return OpenWindow('DESIGN_APPROVERS','?id=" + intID.ToString() + "');");
        }
        protected void btnStart_Click(Object Sender, EventArgs e)
        {
            oForecast.UpdateAnswerExecution(intID, "");
            Start(false);
        }
        protected void btnSchedule_Click(Object Sender, EventArgs e)
        {
            DateTime datSchedule = DateTime.Parse(txtScheduleDate.Text + " " + txtScheduleTime.Text);
            oForecast.UpdateAnswerExecution(intID, datSchedule.ToString());
            Start(true);
        }
        private void Start(bool _schedule)
        {
            oForecast.UpdateAnswerChange(intID, txtChange.Text);
            intRequest = oForecast.GetRequestID(intID, true);
            oForecast.UpdateAnswer(intID, intRequest);
            oForecast.DeleteReset(intID);
            oServiceRequest.Add(intRequest, 1, 1);
            oForecast.UpdateAnswerExecuted(intID, DateTime.Now.ToString(), intProfile);
            int intModel = oForecast.GetModelAsset(intID);
            if (intModel == 0)
                intModel = oForecast.GetModel(intID);
            if (oForecast.CanAutoProvision(intID) == true)
            {
                // Only start the build for auto-provision enabled servers
                oServer.Start(intRequest);
            }
            else
            {
                // Else set the step = 999
                DataSet dsServers = oServer.GetAnswer(intID);
                foreach (DataRow drServer in dsServers.Tables[0].Rows)
                    oServer.UpdateStep(Int32.Parse(drServer["id"].ToString()), 999);
            }
            bool boolNotify = oForecast.NotifyImplementor(intID, intModel, intImplementorDistributed, intWorkstationPlatform, intImplementorMidrange, intEnvironment, intProfile, dsnAsset, dsnIP);
            if (_schedule == true)
                Response.Redirect(Request.Path + "?rid=" + intRequest);
            else if (oForecast.CanAutoProvision(intID) == true)
                Response.Redirect(Request.Path + "?rid=" + intRequest + "&notify=none");
            else
                Response.Redirect(Request.Path + "?rid=" + intRequest + "&notify=" + (boolNotify ? "true" : "false"));
        }
        protected void btnBack_Click(Object Sender, EventArgs e)
        {
            BackToDesign(0);
        }
        protected void btnSoftwareBack_Click(Object Sender, EventArgs e)
        {
            BackToDesign(1);
        }
        private void BackToDesign(int _stopper)
        {
            oForecast.UpdateAnswerExecution(intID, "");
            int intModel = oForecast.GetModel(intID);
            ModelsProperties oModelsProperties = new ModelsProperties(0, dsn);
            int intType = oModelsProperties.GetType(intModel);
            Types oType = new Types(0, dsn);
            string strExecute = oType.Get(intType, "forecast_execution_path");
            OnDemand oOnDemand = new OnDemand(0, dsn);
            int intCount = 0;
            DataSet ds = oOnDemand.GetWizardStepsDoneBack(intID);
            int intStop = ds.Tables[0].Rows.Count - _stopper;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                intCount++;
                if (intCount < intStop)
                    oOnDemand.DeleteWizardStepDone(intID, Int32.Parse(dr["step"].ToString()));
            }
            Response.Redirect(strExecute + "?id=" + intID.ToString());
        }
    }
}