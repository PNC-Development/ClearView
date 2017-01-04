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
    public partial class wm_GENERIC : System.Web.UI.UserControl
    {
        private DataSet ds;
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
        private string dsnServiceEditor = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["ServiceEditorDSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected string strVirtual = ConfigurationManager.AppSettings["VirtualGatekeeper"];
        protected int intAssignPage = Int32.Parse(ConfigurationManager.AppSettings["AssignWorkload"]);
        protected int intResourceRequestApprove = Int32.Parse(ConfigurationManager.AppSettings["ResourceRequestApprove"]);
        protected int intViewPage = Int32.Parse(ConfigurationManager.AppSettings["ViewWorkload"]);
        protected int intMyWork = Int32.Parse(ConfigurationManager.AppSettings["MyWork"]);
        protected int intProfile;
        protected Projects oProject;
        protected Functions oFunction;
        protected Users oUser;
        protected Pages oPage;
        protected ResourceRequest oResourceRequest;
        protected RequestItems oRequestItem;
        protected Requests oRequest;
        protected Services oService;
        protected ServiceRequests oServiceRequest;
        protected RequestFields oRequestField;
        protected Applications oApplication;
        protected ServiceDetails oServiceDetail;
        protected Delegates oDelegate;
        protected Documents oDocument;
        protected Log oLog;
        protected int intApplication = 0;
        protected int intPage = 0;
        protected int intProject = 0;
        protected bool boolDetails = false;
        protected bool boolExecution = false;
        protected bool boolChange = false;
        protected bool boolDocuments = false;
        protected bool boolStatusUpdates = false;
        protected int intRequest = 0;
        protected int intService = 0;
        protected int intItem = 0;
        protected int intNumber = 0;
        protected bool boolCheckboxes = false;
        protected string strCheckboxes = "";
        protected bool boolJoined = false;
        protected bool boolServiceReturned = false;
        protected ServiceEditor oServiceEditor;
        protected Variables oVariable;
        protected string strForm = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oProject = new Projects(intProfile, dsn);
            oFunction = new Functions(intProfile, dsn, intEnvironment);
            oUser = new Users(intProfile, dsn);
            oPage = new Pages(intProfile, dsn);
            oResourceRequest = new ResourceRequest(intProfile, dsn);
            oRequestItem = new RequestItems(intProfile, dsn);
            oRequest = new Requests(intProfile, dsn);
            oService = new Services(intProfile, dsn);
            oServiceRequest = new ServiceRequests(intProfile, dsn);
            oRequestField = new RequestFields(intProfile, dsn);
            oApplication = new Applications(intProfile, dsn);
            oServiceDetail = new ServiceDetails(intProfile, dsn);
            oDelegate = new Delegates(intProfile, dsn);
            oDocument = new Documents(intProfile, dsn);
            oLog = new Log(intProfile, dsn);
            oServiceEditor = new ServiceEditor(intProfile, dsnServiceEditor);
            oVariable = new Variables(intEnvironment);
            if (Request.QueryString["applicationid"] != null && Request.QueryString["applicationid"] != "")
                intApplication = Int32.Parse(Request.QueryString["applicationid"]);
            if (Request.QueryString["pageid"] != null && Request.QueryString["pageid"] != "")
                intPage = Int32.Parse(Request.QueryString["pageid"]);
            if (Request.Cookies["application"] != null && Request.Cookies["application"].Value != "")
                intApplication = Int32.Parse(Request.Cookies["application"].Value);
            if (Request.QueryString["rrid"] != null && Request.QueryString["rrid"] != "")
            {
                // Start Workflow Change
                lblResourceWorkflow.Text = Request.QueryString["rrid"];
                int intResourceWorkflow = Int32.Parse(Request.QueryString["rrid"]);
                int intResourceParent = oResourceRequest.GetWorkflowParent(intResourceWorkflow);
                ds = oResourceRequest.Get(intResourceParent);
                // End Workflow Change
                intRequest = Int32.Parse(ds.Tables[0].Rows[0]["requestid"].ToString());
                lblRequestedBy.Text = oUser.GetFullName(oRequest.GetUser(intRequest));
                lblRequestedOn.Text = DateTime.Parse(oResourceRequest.Get(intResourceParent, "created")).ToString();
                lblDescription.Text = oRequest.Get(intRequest, "description");
                if (lblDescription.Text == "")
                    lblDescription.Text = "<i>No information</i>";
                intItem = Int32.Parse(ds.Tables[0].Rows[0]["itemid"].ToString());
                intNumber = Int32.Parse(ds.Tables[0].Rows[0]["number"].ToString());
                int intStatus = 0;
                Int32.TryParse(oResourceRequest.GetWorkflow(intResourceWorkflow, "status"), out intStatus);
                // Start Workflow Change
                bool boolComplete = (oResourceRequest.GetWorkflow(intResourceWorkflow, "status") == "3");
                int intUser = Int32.Parse(oResourceRequest.GetWorkflow(intResourceWorkflow, "userid"));
                txtCustom.Text = oResourceRequest.GetWorkflow(intResourceWorkflow, "name");
                double dblAllocated = double.Parse(oResourceRequest.GetWorkflow(intResourceWorkflow, "allocated"));
                boolJoined = (oResourceRequest.GetWorkflow(intResourceWorkflow, "joined") == "1");
                // End Workflow Change
                intService = Int32.Parse(ds.Tables[0].Rows[0]["serviceid"].ToString());
                lblService.Text = oService.Get(intService, "name");
                int intApp = oRequestItem.GetItemApplication(intItem);

                //Check if this service returned
                bool boolReturned = false;
                DataSet dsRR = oResourceRequest.GetRequestService(intRequest, intService, intNumber);
                if (dsRR.Tables[0].Rows.Count > 0)
                {
                    int intRRId = Int32.Parse(dsRR.Tables[0].Rows[0]["parent"].ToString());
                    DataSet dsRRReturn = oResourceRequest.getResourceRequestReturn(intRRId, intService, intNumber, 1, 0);
                    if (dsRRReturn.Tables[0].Rows.Count > 0)
                    {
                        boolReturned = true;
                        lblReqReturnedId.Text = dsRRReturn.Tables[0].Rows[0]["Id"].ToString();
                        lblReqReturnCommentValue.Text = oFunction.FormatText(dsRRReturn.Tables[0].Rows[0]["Comments"].ToString());
                        lblReqReturnedByValue.Text = oUser.GetName(Int32.Parse(dsRRReturn.Tables[0].Rows[0]["ReturnedByUser"].ToString()));
                        lblReqReturnedByValue.Text = oUser.GetFullName(Int32.Parse(dsRRReturn.Tables[0].Rows[0]["ReturnedByUser"].ToString()));
                        pnlReqReturn.Visible = true;
                        boolServiceReturned = true;
                    }
                    else
                        pnlReqReturn.Visible = false;
                }
             
                if (!IsPostBack)
                {
                    if (Request.QueryString["save"] != null && Request.QueryString["save"] != "")
                        Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "saved", "<script type=\"text/javascript\">alert('Information Saved Successfully');<" + "/" + "script>");
                    if (Request.QueryString["require"] != null && Request.QueryString["require"] != "")
                        Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "required", "<script type=\"text/javascript\">alert('Information Saved Successfully\\n\\nHowever, one or more required fields were skipped.\\nUpdate these fields and you will be able to complete this request." + (string.IsNullOrEmpty(Request.QueryString["required"]) ? "" : "\\n\\n - " + oFunction.decryptQueryString(Request.QueryString["required"])) + "');<" + "/" + "script>");
                    if (Request.QueryString["status"] != null && Request.QueryString["status"] != "")
                        Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "statusd", "<script type=\"text/javascript\">alert('Status Update has been Added');<" + "/" + "script>");

                    DataSet dsServiceEditor = oServiceEditor.GetRequestData(intRequest, intService, intNumber, 1, dsn);
                    strForm = oServiceEditor.LoadForm(intRequest, intService, intNumber, true, false, "", intEnvironment, dsServiceEditor, dsn);
                    
                    string strRequired = oServiceEditor.GetRequired();
                    double dblUsed = oResourceRequest.GetWorkflowUsed(intResourceWorkflow);
                    lblUpdated.Text = oResourceRequest.GetUpdated(intResourceParent);
                    if (dblAllocated == dblUsed)
                    {
                        if (boolComplete == false)
                        {
                            oFunction.ConfigureToolButton(btnComplete, "/images/tool_complete");
                            btnComplete.Attributes.Add("onclick", "return true " + strRequired + " && confirm('Are you sure you want to mark this as completed and remove it from your workload?') && ProcessControlButton();");
                        }
                        else
                        {
                            btnSave.ImageUrl = "/images/tool_save_dbl.gif";
                            btnSave.Enabled = false;
                            btnReturn.ImageUrl = "/images/tool_return_dbl.gif";
                            btnReturn.Enabled = false;
                            oFunction.ConfigureToolButton(btnComplete, "/images/tool_complete");
                            btnComplete.Attributes.Add("onclick", "alert('This task has already been marked COMPLETE. You can close this window.');return false;");
                        }
                    }
                    else
                    {
                        btnComplete.ImageUrl = "/images/tool_complete_dbl.gif";
                        btnComplete.Enabled = false;
                    }
                    bool boolSLABreached = false;
                    if (oService.Get(intService, "sla") != "")
                    {
                        oFunction.ConfigureToolButton(btnSLA, "/images/tool_sla");
                        int intDays = oResourceRequest.GetSLA(intResourceParent);
                        if (intDays > -99999)
                        {
                            if (boolReturned == false)
                            {
                                if (intDays < 1)
                                    btnSLA.Style["border"] = "solid 2px #FF0000";
                                else if (intDays < 3)
                                    btnSLA.Style["border"] = "solid 2px #FF9999";
                            }
                            boolSLABreached = (intDays < 0);
                            btnSLA.Attributes.Add("onclick", "return OpenWindow('RESOURCE_REQUEST_SLA','?rrid=" + intResourceParent.ToString() + "');");
                        }
                        else
                        {
                            btnSLA.ImageUrl = "/images/tool_sla_dbl.gif";
                            btnSLA.Enabled = false;
                        }
                    }
                    else
                    {
                        btnSLA.ImageUrl = "/images/tool_sla_dbl.gif";
                        btnSLA.Enabled = false;
                    }
                    oFunction.ConfigureToolButton(btnEmail, "/images/tool_email");
                    btnEmail.Attributes.Add("onclick", "return OpenWindow('RESOURCE_REQUEST_EMAIL','?rrid=" + intResourceWorkflow.ToString() + "&type=GENERIC');");
                    dblUsed = (dblUsed / dblAllocated) * 100;
                    strCheckboxes = oServiceDetail.LoadCheckboxes(intRequest, intItem, intNumber, intResourceWorkflow, intService);
                    if (strForm != "")
                        panForm.Visible = true;
                    if (oService.Get(intService, "tasks") != "1" || strCheckboxes == "")
                    {
                        if (oService.Get(intService, "no_slider") == "1")
                        {
                            panNoSlider.Visible = true;
                            btnComplete.ImageUrl = "/images/tool_complete.gif";
                            btnComplete.Enabled = true;
                            btnComplete.Attributes.Add("onclick", "return true " + strRequired + " && confirm('Are you sure you want to mark this as completed and remove it from your workload?') && ProcessControlButton();");
                            btnComplete.CommandArgument = "FAST";
                        }
                        else
                        {
                            panSlider.Visible = true;
                            sldHours._StartPercent = dblUsed.ToString();
                            sldHours._TotalHours = dblAllocated.ToString();
                        }
                    }
                    else
                        panCheckboxes.Visible = true;
                    intProject = oRequest.GetProjectNumber(intRequest);
                    hdnTab.Value = "D";
                    panWorkload.Visible = true;
                    bool boolRed = LoadStatus(intResourceWorkflow);
                    if (boolRed == false && boolSLABreached == true && boolReturned == false)
                        btnComplete.Attributes.Add("onclick", "alert('NOTE: Your Service Level Agreement (SLA) has been breached!\\n\\nYou must provide a RED STATUS update with an explanation of why your SLA was breached for this request.\\n\\nOnce a RED STATUS update has been provided, you will be able to complete this request.');return false;");
                    LoadChange(intResourceWorkflow);
                    LoadInformation(intResourceWorkflow);
                    chkDescription.Checked = (Request.QueryString["doc"] != null);
                    lblDocuments.Text = oDocument.GetDocuments_Service(intRequest, intService, oVariable.DocumentsFolder(), 1, (Request.QueryString["doc"] != null));
                    // GetDocuments(string _physical, int _projectid, int _requestid, int _userid, int _security, bool _show_description, bool _mine)
                    //lblDocuments.Text = oDocument.GetDocuments(Request.PhysicalApplicationPath, 0, intRequest, 0, 1, (Request.QueryString["doc"] != null), false);

                    btnDenied.Attributes.Add("onclick", "return CloseWindow();");
                    oFunction.ConfigureToolButton(btnSave, "/images/tool_save");
                    oFunction.ConfigureToolButton(btnPrint, "/images/tool_print");
                    btnPrint.Attributes.Add("onclick", "return PrintWindow();");
                    oFunction.ConfigureToolButton(btnClose, "/images/tool_close");
                    btnClose.Attributes.Add("onclick", "return ExitWindow();");
                    //btnSave.Attributes.Add("onclick", "return ValidateStatus('" + ddlStatus.ClientID + "','" + txtComments.ClientID + "') && ProcessControlButton();");
                    btnSave.Attributes.Add("onclick", "return ProcessControlButton();");
                    btnStatus.Attributes.Add("onclick", "return ValidateStatus('" + ddlStatus.ClientID + "','" + txtComments.ClientID + "')" +
                        " && ProcessControlButton()" +
                        ";");
                    btnChange.Attributes.Add("onclick", "return ValidateText('" + txtNumber.ClientID + "','Please enter a change control number')" +
                        " && ValidateDate('" + txtDate.ClientID + "','Please enter a valid implementation date')" +
                        " && ValidateTime('" + txtTime.ClientID + "','Please enter a valid implementation time')" +
                        " && ProcessControlButton()" +
                        ";");
                    imgDate.Attributes.Add("onclick", "return ShowCalendar('" + txtDate.ClientID + "');");
                    // 6/1/2009 - Load ReadOnly View
                    if (oResourceRequest.CanUpdate(intProfile, intResourceWorkflow, false) == false)
                    {
                        oFunction.ConfigureToolButtonRO(btnSave, btnComplete);
                        //panDenied.Visible = true;
                    }

                    if (oService.Get(intService, "rr_path") == "/controls/rr/rr_service_editor.ascx" && oService.Get(intService, "disable_customization") == "0")
                    {
                        if (intStatus != (int)ResourceRequestStatus.AwaitingResponse) //Awaiting Client Response
                        {
                            btnReturn.Visible = true;
                            oFunction.ConfigureToolButton(btnReturn, "/images/tool_return");
                            btnReturn.Attributes.Add("onclick", "return OpenWindow('RESOURCE_REQUEST_RETURN','?rrid=" + intResourceParent.ToString() + "&rrwfid=" + intResourceWorkflow.ToString() + "');");
                        }
                        else
                        {
                            btnReturn.Visible = true;
                            oFunction.ConfigureToolButton(btnReturn, "/images/tool_return");
                            btnReturn.Attributes.Add("onclick", "alert('This request has already been returned');return false;");
                        }
                    }
                    else
                        btnReturn.Visible = false;
                }
            }
            else
                panDenied.Visible = true;
        }
        private bool LoadStatus(int _resourceid)
        {
            bool boolRed = false;
            DataSet dsStatus = oResourceRequest.GetStatuss(_resourceid);
            rptStatus.DataSource = dsStatus;
            rptStatus.DataBind();
            lblNoStatus.Visible = (rptStatus.Items.Count == 0);
            double dblTotalStatus = 0.00;
            foreach (RepeaterItem ri in rptStatus.Items)
            {
                Label _status = (Label)ri.FindControl("lblStatus");
                if (boolRed == false && _status.Text == "1")
                    boolRed = true;
                double dblStatus = double.Parse(_status.Text);
                if (dblTotalStatus == 0.00)
                    dblTotalStatus = dblStatus;
                _status.Text = oResourceRequest.GetStatus(dblStatus, 50, 15);
            }
            lblStatus.Text = oResourceRequest.GetStatus(dblTotalStatus, 50, 15);
            return boolRed;
        }
        private void LoadChange(int _resourceid)
        {
            DataSet dsChange = oResourceRequest.GetChangeControls(_resourceid);
            rptChange.DataSource = dsChange;
            rptChange.DataBind();
            lblNoChange.Visible = (rptChange.Items.Count == 0);
            foreach (RepeaterItem ri in rptChange.Items)
            {
                LinkButton _delete = (LinkButton)ri.FindControl("btnDeleteChange");
                _delete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this change control?');");
            }
        }
        private void LoadInformation(int _request)
        {
            lblView.Text = oRequestField.GetBodyWorkflow(_request, dsnServiceEditor, intEnvironment, dsnAsset, dsnIP);
            if (intProject > 0)
            {
                lblName.Text = oProject.Get(intProject, "name");
                lblNumber.Text = oProject.Get(intProject, "number");
                lblType.Text = "Project";
            }
            else
            {
                lblName.Text = oResourceRequest.GetWorkflow(_request, "name");
                //lblName.Text = oResourceRequest.GetWorkflow(_request, "name");
                lblNumber.Text = "CVT" + intRequest.ToString();
                lblType.Text = "Task";
            }
            if (Request.QueryString["div"] != null)
            {
                switch (Request.QueryString["div"])
                {
                    case "E":
                        boolExecution = true;
                        break;
                    case "C":
                        boolChange = true;
                        break;
                    case "D":
                        boolDocuments = true;
                        break;
                    case "S":
                        boolStatusUpdates = true;
                        break;
                }
            }
            if (boolDetails == false && boolExecution == false && boolStatusUpdates == false && boolChange == false && boolDocuments == false)
                boolDetails = true;

        }
        protected void btnStatus_Click(Object Sender, EventArgs e)
        {
            int intResourceWorkflow = Int32.Parse(lblResourceWorkflow.Text);
            int intStatusLatest = 0;
            DataSet dsLatest = oResourceRequest.GetStatusLatest(intResourceWorkflow);
            if (dsLatest.Tables[0].Rows.Count > 0)
                Int32.TryParse(dsLatest.Tables[0].Rows[0]["status"].ToString(), out intStatusLatest);
            int intResourceParent = oResourceRequest.GetWorkflowParent(intResourceWorkflow);
            if (ddlStatus.SelectedIndex > -1 && txtComments.Text.Trim() != "")
            {
                oResourceRequest.AddStatus(intResourceWorkflow, Int32.Parse(ddlStatus.SelectedItem.Value), txtComments.Text, intProfile);
                string strNotifyClient = "";
                //CVT62149 Workload Manager Red Light Status =Hold
                if (ddlStatus.SelectedValue == "1") // Red
                {
                    oResourceRequest.UpdateWorkflowStatus(intResourceWorkflow, 5, true);
                    strNotifyClient = (oService.Get(intService, "notify_red") == "1" ? "On Hold" : "");
                }
                else
                {
                    oResourceRequest.UpdateWorkflowStatus(intResourceWorkflow, 2, true);
                    if (ddlStatus.SelectedValue == "2") // Yellow
                        strNotifyClient = (oService.Get(intService, "notify_yellow") == "1" ? "Alert" : "");
                    else
                        strNotifyClient = ((intStatusLatest == 1 || intStatusLatest == 2) && oService.Get(intService, "notify_green") == "1" ? "Resume" : "");
                }

                if (strNotifyClient != "")
                {
                    // Notify the client of the status change.
                    int intRequestor = oRequest.GetUser(intRequest);
                    string strComment = txtComments.Text.Trim();
                    if (strComment != "")
                    {
                        string strBodyComment = "<table width=\"100%\" cellpadding=\"4\" cellspacing=\"3\" border=\"0\" align=\"center\" style=\"border:solid 1px #CCCCCC;" + oVariable.DefaultFontStyle() + "\">";
                        strBodyComment += "<tr bgcolor=\"#EEEEEE\"><td><span style=\"" + oVariable.DefaultFontStyleHeader() + "\">" + oUser.GetFullName(intProfile) + "</span>&nbsp;&nbsp;[" + DateTime.Now.ToString() + "]:</td></tr>";
                        strBodyComment += "<tr><td>" + oFunction.FormatText(strComment) + "</td></tr>";
                        strBodyComment += "</table>";
                        strComment = strBodyComment;
                    }
                    string strLink = "<a href=\"" + oVariable.URL() + "/redirect.aspx?referrer=/datapoint/service/resource.aspx?id=" + oFunction.encryptQueryString(intResourceParent.ToString()) + "\" target=\"_blank\">Click here to view this request.</a>";
                    string strHeading = "Great news! All previous issues have been resolved and the progression of this task has resumed.";
                    if (strNotifyClient == "On Hold")
                        strHeading = "An issue has been encountered that is preventing this task from being completed. The request has been placed <b>ON HOLD</b>.";
                    else if (strNotifyClient == "Alert")
                        strHeading = "Minor issues have been encountered that are slowing down the progression of this task.  Although still being processed, the service level agreement (SLA) may be exceeded.";
                    oFunction.SendEmail("Service Request #CVT" + intRequest.ToString() + "-" + intService.ToString() + "-" + intNumber.ToString() + " Notification", oUser.GetName(intRequestor), "", oUser.GetName(intProfile), "Service Request #CVT" + intRequest.ToString() + "-" + intService.ToString() + "-" + intNumber.ToString() + " Notification *" + strNotifyClient + "*", "<p>" + strHeading + "</p><p>" + strComment + "</p><p>" + oResourceRequest.GetWorkflowSummary(intResourceWorkflow, intEnvironment, dsnServiceEditor, dsnAsset, dsnIP) + "</p><p>" + strLink + "</p>", true, false);
                }

                //Send notification to users of associated tasks
                WMServiceTasks oWMServiceTasks = new WMServiceTasks(intProfile, dsn);
                DataSet DsWMAssociatedTasks = oWMServiceTasks.getWMServiceTasksStatusRequestWithRequest(intRequest);
                Customized oCustomized = new Customized(intProfile, dsn);
                
                if (DsWMAssociatedTasks.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow drWMAssoicatedTasks in DsWMAssociatedTasks.Tables[0].Rows)
                    {
                        string strWMAssoicatedTasksUser = oUser.GetName(Int32.Parse(drWMAssoicatedTasks["createdBy"].ToString()));
                        string strComment = txtComments.Text.Trim();
                        string strLink = "<p><a href=\"" + oVariable.URL() + "/redirect.aspx?referrer=/datapoint/service/resource.aspx?id=" + oFunction.encryptQueryString(intResourceParent.ToString()) + "\" target=\"_blank\">Click here to view this request.</a></p>";
                        oFunction.SendEmail("Associated Request Updated CVT" + intRequest.ToString() + "-" + intService.ToString() + "-" + intNumber.ToString(), strWMAssoicatedTasksUser, "", "", "Associated Request Updated CVT" + intRequest.ToString() + "-" + intService.ToString() + "-" + intNumber.ToString(), "<p><b>The following associated service request has been updated with following comments...</b></br>" + strComment + "</p>" + strLink + "<p>" + oResourceRequest.GetWorkflowSummary(intResourceWorkflow, intEnvironment, dsnServiceEditor, dsnAsset, dsnIP) + "</p>", true, false);
                    }
                }
                //End of Send notification to users of associated tasks

            }

            Response.Redirect(Request.Path + "?rrid=" + intResourceWorkflow.ToString() + "&div=S&status=true");

        }
        protected void btnChange_Click(Object Sender, EventArgs e)
        {
            int intResourceWorkflow = Int32.Parse(lblResourceWorkflow.Text);
            int intResourceParent = oResourceRequest.GetWorkflowParent(intResourceWorkflow);
            oResourceRequest.AddChangeControl(intResourceWorkflow, txtNumber.Text, DateTime.Parse(txtDate.Text + " " + txtTime.Text), txtChange.Text);
            Response.Redirect(Request.Path + "?rrid=" + intResourceWorkflow.ToString() + "&div=C&save=true");
        }
        protected void btnDeleteChange_Click(Object Sender, EventArgs e)
        {
            LinkButton oButton = (LinkButton)Sender;
            oResourceRequest.DeleteChangeControl(Int32.Parse(oButton.CommandArgument));
            Response.Redirect(Request.Path + "?rrid=" + lblResourceWorkflow.Text + "&div=C");
        }
        protected void btnReturn_Click(Object Sender, EventArgs e)
        {
        }
        protected void chkDescription_Change(Object Sender, EventArgs e)
        {
            if (chkDescription.Checked == true)
                Response.Redirect(Request.Path + "?rrid=" + Request.QueryString["rrid"] + "&doc=true&div=D");
            else
                Response.Redirect(Request.Path + "?rrid=" + Request.QueryString["rrid"] + "&div=D");
        }
        protected void btnSave_Click(Object Sender, EventArgs e)
        {
            int intResourceWorkflow = Int32.Parse(lblResourceWorkflow.Text);
            int intResourceParent = oResourceRequest.GetWorkflowParent(intResourceWorkflow);
            //if (ddlStatus.SelectedIndex > -1 && txtComments.Text.Trim() != "")
            //{
            //    oResourceRequest.AddStatus(intResourceWorkflow, Int32.Parse(ddlStatus.SelectedItem.Value), txtComments.Text);

            //    //CVT62149 Workload Manager Red Light Status =Hold
            //    if (ddlStatus.SelectedValue == "1") //Red
            //        oResourceRequest.UpdateWorkflowStatus(intResourceWorkflow, 5, true);
            //    else
            //        oResourceRequest.UpdateWorkflowStatus(intResourceWorkflow, 2, true); 
            //}

            if (oResourceRequest.Get(intResourceParent, "status") == ((int)ResourceRequestStatus.Closed).ToString()
                || oResourceRequest.GetWorkflow(intResourceWorkflow, "status") == ((int)ResourceRequestStatus.Closed).ToString())
            {
                // already completed - nothing to do except close the window.
            }
            else
            {
                if (panNoSlider.Visible == false)
                {
                    double dblHours = 0.00;
                    if (panSlider.Visible == true)
                    {
                        if (Request.Form["hdnHours"] != null && Request.Form["hdnHours"] != "")
                            dblHours = double.Parse(Request.Form["hdnHours"]);
                        double dblUsed = oResourceRequest.GetWorkflowUsed(intResourceWorkflow);
                        dblHours = (dblHours - dblUsed);
                        if (dblHours > 0.00)
                            oResourceRequest.UpdateWorkflowHours(intResourceWorkflow, dblHours);
                    }
                    else
                    {
                        oServiceDetail.UpdateCheckboxes(Request, intResourceWorkflow, intRequest, intItem, intNumber);
                        double dblAllocated = oResourceRequest.GetDetailsHoursUsed(intRequest, intItem, intNumber, intResourceWorkflow, false);
                        oResourceRequest.UpdateWorkflowAllocated(intResourceWorkflow, dblAllocated);
                    }
                }
                if (panForm.Visible == true)
                    oServiceEditor.SaveForm(Request, intRequest, intService, intNumber, true, intEnvironment, dsn);
                oResourceRequest.UpdateWorkflowName(intResourceWorkflow, txtCustom.Text, intProfile);
            }
            Response.Redirect(Request.Path + "?rrid=" + intResourceWorkflow.ToString() + "&div=E&save=true");
        }
        protected void btnComplete_Click(Object Sender, EventArgs e)
        {
            int intResourceWorkflow = Int32.Parse(lblResourceWorkflow.Text);
            string strInvalid = "";
            if (panForm.Visible == true)
                strInvalid = oServiceEditor.SaveForm(Request, intRequest, intService, intNumber, true, intEnvironment, dsn);

            if (strInvalid == "")
            {
                // All Required Fields have been completed
                int intResourceParent = oResourceRequest.GetWorkflowParent(intResourceWorkflow);
                if (oResourceRequest.Get(intResourceParent, "status") == ((int)ResourceRequestStatus.Closed).ToString()
                    || oResourceRequest.GetWorkflow(intResourceWorkflow, "status") == ((int)ResourceRequestStatus.Closed).ToString())
                {
                    // already completed - nothing to do except close the window.
                }
                else
                {
                    if (panNoSlider.Visible == true)
                    {
                        double dblAllocated = double.Parse(oResourceRequest.GetWorkflow(intResourceWorkflow, "allocated"));
                        oResourceRequest.UpdateWorkflowHoursOverwrite(intResourceWorkflow, dblAllocated);
                    }
                    // Add a green / completed status if there are no updates, OR the last status is not green
                    DataSet dsStatus = oResourceRequest.GetStatuss(intResourceWorkflow);
                    if (dsStatus.Tables[0].Rows.Count == 0 || dsStatus.Tables[0].Rows[0]["status"].ToString() != "3")
                        oResourceRequest.AddStatus(intResourceWorkflow, (int)ResourceRequestStatus.Closed, "Completed", intProfile);
                    oResourceRequest.UpdateWorkflowStatus(intResourceWorkflow, (int)ResourceRequestStatus.Closed, true);
                    string strCVT = "CVT" + intRequest.ToString() + "-" + intService.ToString() + "-" + intNumber.ToString();
                    oLog.AddEvent(intRequest.ToString(), strCVT, "Closed WM_Generic by " + oUser.GetFullNameWithLanID(intProfile), LoggingType.Debug);
                    oResourceRequest.CloseWorkflow(intResourceWorkflow, intEnvironment, 0, dsnServiceEditor, true, intResourceRequestApprove, intAssignPage, intViewPage, dsnAsset, dsnIP, boolServiceReturned);

                    // Send notification to list of completion users
                    string strCompletion = oService.Get(intService, "notify_complete");
                    if (strCompletion != "")
                    {
                        string strLink = "<a href=\"" + oVariable.URL() + "/redirect.aspx?referrer=/datapoint/service/resource.aspx?id=" + oFunction.encryptQueryString(intResourceParent.ToString()) + "\" target=\"_blank\">Click here to view this request.</a>";
                        oFunction.SendEmail("Service Request #CVT" + intRequest.ToString() + "-" + intService.ToString() + "-" + intNumber.ToString() + " Complete", strCompletion, "", "", "Service Request #CVT" + intRequest.ToString() + "-" + intService.ToString() + "-" + intNumber.ToString() + " Complete", "<p>This message is to notify you that the following service request has been completed...</p><p>" + oResourceRequest.GetWorkflowSummary(intResourceWorkflow, intEnvironment, dsnServiceEditor, dsnAsset, dsnIP) + "</p><p>You are receiving this message because you have been configured to receive a notification for all completed requests for the service <b>" + oService.GetName(intService) + "</b><p>" + strLink + "</p>", true, false);
                    }

                    //Send notification to users of associated tasks
                    WMServiceTasks oWMServiceTasks = new WMServiceTasks(intProfile, dsn);
                    DataSet DsWMAssociatedTasks = oWMServiceTasks.getWMServiceTasksStatusRequestWithRequest(intRequest);
                    Customized oCustomized = new Customized(intProfile, dsn);

                    if (DsWMAssociatedTasks.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow drWMAssoicatedTasks in DsWMAssociatedTasks.Tables[0].Rows)
                        {
                            string strWMAssoicatedTasksUser = oUser.GetName(Int32.Parse(drWMAssoicatedTasks["createdBy"].ToString()));
                            string strComment = "";
                            string strLink = "<p><a href=\"" + oVariable.URL() + "/redirect.aspx?referrer=/datapoint/service/resource.aspx?id=" + oFunction.encryptQueryString(intResourceParent.ToString()) + "\" target=\"_blank\">Click here to view this request.</a></p>";
                            oFunction.SendEmail("Associated Request Completed CVT" + intRequest.ToString() + "-" + intService.ToString() + "-" + intNumber.ToString(), strWMAssoicatedTasksUser, "", "", "Associated Request Completed CVT" + intRequest.ToString() + "-" + intService.ToString() + "-" + intNumber.ToString(), "<p><b>The following associated service request has been completed!</b></br>" + strComment + "</p>" + strLink + "<p>" + oResourceRequest.GetWorkflowSummary(intResourceWorkflow, intEnvironment, dsnServiceEditor, dsnAsset, dsnIP) + "</p>", true, false);
                        }
                    }
                    //End of Send notification to users of associated tasks

                    if (intService == Int32.Parse(ConfigurationManager.AppSettings["SERVICEID_DECOMMISSION_SUPPORT"])
                        || intService == Int32.Parse(ConfigurationManager.AppSettings["SERVICEID_DESTROY_SUPPORT"])
                        || intService == Int32.Parse(ConfigurationManager.AppSettings["SERVICEID_DECOMMISSION_SUPPORT_WORKSTATION"])
                        || intService == Int32.Parse(ConfigurationManager.AppSettings["SERVICEID_DESTROY_SUPPORT_WORKSTATION"]))
                    {
                        Asset oAsset = new Asset(0, dsnAsset);
                        oAsset.UpdateDecommissionFixed(intRequest, intNumber);
                    }

                    //If this service was returned then update the status of next service 
                    if (boolServiceReturned == true)
                    {
                        DataSet dsRR = oResourceRequest.GetRequestService(intRequest, intService, intNumber);
                        if (dsRR.Tables[0].Rows.Count > 0)
                        {
                            int intRRId = Int32.Parse(dsRR.Tables[0].Rows[0]["parent"].ToString());

                            DataSet dsRRReturn = oResourceRequest.getResourceRequestReturn(intRRId, intService, intNumber, 1, 0);
                            if (dsRRReturn.Tables[0].Rows.Count > 0)
                            {
                                int intNextRRId = Int32.Parse(dsRRReturn.Tables[0].Rows[0]["NextRRId"].ToString());
                                int intNextServiceId = Int32.Parse(dsRRReturn.Tables[0].Rows[0]["NextServiceId"].ToString());
                                int intNextNumber = Int32.Parse(dsRRReturn.Tables[0].Rows[0]["NextNumber"].ToString()); ;

                                // Get all at that level to resume
                                DataSet dsWorkflow = oServiceEditor.GetWorkflow(intRequest);
                                // Get leveling
                                int intLevel = 0;
                                foreach (DataRow drWorkflow in dsWorkflow.Tables[0].Rows)
                                {
                                    if (Int32.Parse(drWorkflow["serviceid"].ToString()) == intNextServiceId)
                                    {
                                        intLevel = Int32.Parse(drWorkflow["leveling"].ToString());
                                        break;
                                    }
                                }
                                foreach (DataRow drWorkflow in dsWorkflow.Tables[0].Rows)
                                {
                                    if (Int32.Parse(drWorkflow["leveling"].ToString()) == intLevel)
                                    {
                                        string strService = oService.GetName(Int32.Parse(drWorkflow["ServiceID"].ToString()));
                                        int intResourceRequestID = 0;
                                        if (Int32.TryParse(drWorkflow["ResourceID"].ToString(), out intResourceRequestID))
                                        {
                                            oResourceRequest.UpdateStatusRequest(intResourceRequestID, 2);
                                            DataSet dsRRWF = oResourceRequest.GetWorkflowsParent(intResourceRequestID);
                                            foreach (DataRow dr in dsRRWF.Tables[0].Rows)
                                            {
                                                int intRRWFId = Int32.Parse(dr["id"].ToString());
                                                int intRRWFUserId = Int32.Parse(dr["userid"].ToString());
                                                oResourceRequest.UpdateWorkflowStatus(intRRWFId, 2, true);
                                                // Notify
                                                string strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_REQUEST_ASSIGNMENT");
                                                string strDefault = oUser.GetApplicationUrl(intRRWFUserId, intViewPage);
                                                if (strDefault == "")
                                                    oFunction.SendEmail("Returned Request Completed: " + strService, oUser.GetName(intRRWFUserId), "", strEMailIdsBCC, "Returned Request Completed: " + strService, "<p><b>The following returned request has been completed by " + oUser.GetFullName(intProfile) + " and is now in your queue.</b></p><p>" + oResourceRequest.GetWorkflowSummary(intResourceWorkflow, intEnvironment, dsnServiceEditor, dsnAsset, dsnIP) + "</p>", true, false);
                                                else
                                                {
                                                    if (intProject > 0)
                                                        oFunction.SendEmail("Returned Request Completed: " + strService, oUser.GetName(intRRWFUserId), "", strEMailIdsBCC, "Returned Request Completed: " + strService, "<p><b>The following returned request has been completed by " + oUser.GetFullName(intProfile) + " and is now in your queue.</b></p><p>" + oResourceRequest.GetWorkflowSummary(intResourceWorkflow, intEnvironment, dsnServiceEditor, dsnAsset, dsnIP) + "</p><p><a href=\"" + oVariable.URL() + "/redirect.aspx?referrer=/" + strDefault + oPage.GetFullLink(intViewPage) + "?pid=" + intProject.ToString() + "\" target=\"_blank\">Click here to review your assignment.</a></p>", true, false);
                                                    else
                                                        oFunction.SendEmail("Returned Request Completed: " + strService, oUser.GetName(intRRWFUserId), "", strEMailIdsBCC, "Returned Request Completed: " + strService, "<p><b>The following returned request has been completed by " + oUser.GetFullName(intProfile) + " and is now in your queue.</b></p><p>" + oResourceRequest.GetWorkflowSummary(intResourceWorkflow, intEnvironment, dsnServiceEditor, dsnAsset, dsnIP) + "</p><p><a href=\"" + oVariable.URL() + "/redirect.aspx?referrer=/frame/resource_request.aspx?rrid=" + intResourceWorkflow.ToString() + "\" target=\"_blank\">Click here to review your assignment.</a></p>", true, false);
                                                }
                                            }
                                        }
                                    }
                                }


                                //oResourceRequest.UpdateStatusRequest(intNextRRId, 2);
                                //DataSet dsRRWF = oResourceRequest.GetWorkflowsParent(intNextRRId);
                                //foreach (DataRow dr in dsRRWF.Tables[0].Rows)
                                //{
                                //    int intRRWFId = Int32.Parse(dr["id"].ToString());
                                //    oResourceRequest.UpdateWorkflowStatus(intRRWFId, 2, true);
                                //}
                            }
                        }
                        oResourceRequest.updateResourceRequestReturnCompleted(Int32.Parse(lblReqReturnedId.Text));

                    }
                }
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "reload", "<script type=\"text/javascript\">RefreshOpeningWindow();window.close();<" + "/" + "script>");
            }
            else
                Response.Redirect(Request.Path + "?rrid=" + intResourceWorkflow.ToString() + "&div=E&require=true&required=" + oFunction.encryptQueryString(strInvalid));
        }
    }
}