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
using Microsoft.ApplicationBlocks.Data;
using NCC.ClearView.Presentation.Web.Custom;
namespace NCC.ClearView.Presentation.Web
{
    public partial class wm_enhancement : System.Web.UI.UserControl
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
        protected int intEnhancementPage = Int32.Parse(ConfigurationManager.AppSettings["HELP_ENHANCEMENT_PAGEID"]);
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
        protected Enhancements oEnhancement;
        protected Variables oVariable;
        protected int intApplication = 0;
        protected int intPage = 0;
        protected int intProject = 0;
        protected bool boolDetails = false;
        protected bool boolExecution = false;
        protected int intRequest = 0;
        protected int intItem = 0;
        protected int intNumber = 0;
        protected int intID = 0;
        protected string strMenuTab1 = "";

        // Vijay Code
        protected int intService;
        protected string strMessages = "";

        private string strEMailIdsBCC = "";

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
            oEnhancement = new Enhancements(intProfile, dsn);
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
                DataSet dsEnhancement = oEnhancement.GetRequest(intRequest);
                if (dsEnhancement.Tables[0].Rows.Count == 1)
                {
                    intID = Int32.Parse(dsEnhancement.Tables[0].Rows[0]["id"].ToString());
                    lblStatus.Text = oEnhancement.Status(intID);
                }
                else
                    lblStatus.Text = "Not Found (" + dsEnhancement.Tables[0].Rows.Count.ToString() + ")";
                lblRequestedBy.Text = oUser.GetFullName(oRequest.GetUser(intRequest));
                lblRequestedOn.Text = DateTime.Parse(oResourceRequest.Get(intResourceParent, "created")).ToString();
                lblDescription.Text = oRequest.Get(intRequest, "description");
                if (lblDescription.Text == "")
                    lblDescription.Text = "<i>No information</i>";
                intItem = Int32.Parse(ds.Tables[0].Rows[0]["itemid"].ToString());
                intNumber = Int32.Parse(ds.Tables[0].Rows[0]["number"].ToString());
                // Start Workflow Change
                bool boolComplete = (oResourceRequest.GetWorkflow(intResourceWorkflow, "status") == "3");
                int intUser = Int32.Parse(oResourceRequest.GetWorkflow(intResourceWorkflow, "userid"));
                txtCustom.Text = oResourceRequest.GetWorkflow(intResourceWorkflow, "name");
                double dblAllocated = double.Parse(oResourceRequest.GetWorkflow(intResourceWorkflow, "allocated"));
                // End Workflow Change
                intService = Int32.Parse(ds.Tables[0].Rows[0]["serviceid"].ToString());
                lblService.Text = oService.Get(intService, "name");
                int intApp = oRequestItem.GetItemApplication(intItem);

                if (Request.QueryString["save"] != null && Request.QueryString["save"] != "")
                    Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "saved", "<script type=\"text/javascript\">alert('Information Saved Successfully');<" + "/" + "script>");
                if (Request.QueryString["status"] != null && Request.QueryString["status"] != "")
                    Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "statusd", "<script type=\"text/javascript\">alert('Status Updates has been Added');<" + "/" + "script>");
                if (!IsPostBack)
                {
                    double dblUsed = oResourceRequest.GetWorkflowUsed(intResourceWorkflow);
                    lblUpdated.Text = oResourceRequest.GetUpdated(intResourceWorkflow);
                    if (dblAllocated == dblUsed)
                    {
                        if (boolComplete == false)
                        {
                            oFunction.ConfigureToolButton(btnComplete, "/images/tool_complete");
                            btnComplete.Attributes.Add("onclick", "return confirm('Are you sure you want to mark this as completed and remove it from your workload?');");
                        }
                        else
                        {
                            oFunction.ConfigureToolButton(btnComplete, "/images/tool_complete");
                            btnComplete.Attributes.Add("onclick", "alert('This task has already been marked COMPLETE. You can close this window.');return false;");
                        }
                    }
                    else
                    {
                        btnComplete.ImageUrl = "/images/tool_complete_dbl.gif";
                        btnComplete.Enabled = false;
                    }
                    dblUsed = (dblUsed / dblAllocated) * 100;
                    intProject = oRequest.GetProjectNumber(intRequest);
                    hdnTab.Value = "D";
                    panWorkload.Visible = true;
                    LoadLists();
                    LoadStatus(intResourceWorkflow);
                    LoadInformation(intResourceWorkflow);

                    btnDenied.Attributes.Add("onclick", "return CloseWindow();");
                    oFunction.ConfigureToolButton(btnSave, "/images/tool_save");
                    oFunction.ConfigureToolButton(btnPrint, "/images/tool_print");
                    btnPrint.Attributes.Add("onclick", "return PrintWindow();");
                    oFunction.ConfigureToolButton(btnClose, "/images/tool_close");
                    btnClose.Attributes.Add("onclick", "return ExitWindow();");
                    btnMessage.Attributes.Add("onclick", "ShowHideDiv2('" + divMessage.ClientID + "');return false;");

                    // 6/1/2009 - Load ReadOnly View
                    if (oResourceRequest.CanUpdate(intProfile, intResourceWorkflow, false) == false)
                    {
                        oFunction.ConfigureToolButtonRO(btnSave, btnComplete);
                        //panDenied.Visible = true;
                    }
                }
            }
            else
                panDenied.Visible = true;
        }
        private void LoadLists()
        {
            oEnhancement.AddVersions(radEstimate);
            oEnhancement.AddVersions(radRelease);
        }
        private void LoadStatus(int _resourceid)
        {
            int intResourceWorkflow = Int32.Parse(lblResourceWorkflow.Text);
            int intResourceParent = oResourceRequest.GetWorkflowParent(intResourceWorkflow);
        }
        private void LoadInformation(int _request)
        {
            lblView.Text = oEnhancement.GetBody(intID, intEnvironment);
            if (intProject > 0)
            {
                lblName.Text = oProject.Get(intProject, "name");
                lblNumber.Text = oProject.Get(intProject, "number");
                lblType.Text = "Project";
            }
            else
            {
                lblName.Text = oResourceRequest.GetWorkflow(_request, "name");
                lblNumber.Text = "CVT" + intRequest.ToString();
                lblType.Text = "Task";
            }
            int intMenuTab = 0;
            if (Request.QueryString["menu_tab"] != null && Request.QueryString["menu_tab"] != "")
                intMenuTab = Int32.Parse(Request.QueryString["menu_tab"]);
            Tab oTab = new Tab(hdnTab.ClientID, intMenuTab, "divMenu1", true, false);
            oTab.AddTab("Enhancement Details", "");
            oTab.AddTab("Execution", "");


            int intStep = 1;
            DataSet dsSteps = oEnhancement.GetSteps(intID, 0, 0);
            if (dsSteps.Tables[0].Rows.Count > 0)
                intStep = Int32.Parse(dsSteps.Tables[0].Rows[0]["step"].ToString());

            intStep = LoadStep(1, intStep, img1, null, tr1, tr1Done, lbl1, dsSteps);
            intStep = LoadStep(2, intStep, img2, tr2Wait, tr2, tr2Done, lbl2, dsSteps);
            intStep = LoadStep(3, intStep, img3, tr3Wait, tr3, tr3Done, lbl3, dsSteps);
            intStep = LoadStep(4, intStep, img4, tr4Wait, tr4, tr4Done, lbl4, dsSteps);
            intStep = LoadStep(5, intStep, img5, tr5Wait, tr5, tr5Done, lbl5, dsSteps);
            intStep = LoadStep(6, intStep, img6, tr6Wait, tr6, tr6Done, lbl6, dsSteps);

            DataSet dsDocuments = oEnhancement.GetDocuments(intID);

            // Step # 1
            switch (intStep)
            {
                case 1:
                    btnSave.Attributes.Add("onclick", "return (document.getElementById('" + chk1.ClientID + "').checked == true)" +
                                        " || (document.getElementById('" + chk1.ClientID + "').checked == false" +
                                        " && ValidateText('" + filFunctional.ClientID + "','Select a functional requirement document')" +
                                        " && ValidateNumber0('" + txtDays.ClientID + "','Enter a valid number for the estimated date range')" +
                                        " && ValidateRadioList('" + radEstimate.ClientID + "','Make a selection for your estimated release date')" +
                                        ") && ProcessControlButton() && LoadWait()" +
                                        ";");
                    break;
                case 2:
                    rptApprovers.DataSource = oEnhancement.GetApprovalResults(intID, intStep);
                    rptApprovers.DataBind();
                    lblApprovers.Visible = (rptApprovers.Items.Count == 0);
                    btnApprovalGroup.Attributes.Add("onclick", "return OpenWindow('ENHANCEMENT_APPROVAL_GROUPS','?enhancementid=" + intID.ToString() + "&step=" + intStep.ToString() + "');");
                    break;
                case 3:
                    if (dsDocuments.Tables[0].Rows.Count > 0)
                        lblEstimate.Text = dsDocuments.Tables[0].Rows[0]["release"].ToString(); ;
                    radEstimateYes.Attributes.Add("onclick", "ShowHideDiv('" + divEstimate.ClientID + "','none');");
                    radEstimateNo.Attributes.Add("onclick", "ShowHideDiv('" + divEstimate.ClientID + "','inline');");
                    btnSave.Attributes.Add("onclick", "return ValidateRadioButtons('" + radEstimateYes.ClientID + "','" + radEstimateNo.ClientID + "','Select whether or not the estimated release date is OK')" +
                                        " && (document.getElementById('" + radEstimateYes.ClientID + "').checked == true || (document.getElementById('" + radEstimateNo.ClientID + "').checked == true && ValidateRadioList('" + radRelease.ClientID + "','Make a selection for your estimated release date')))" +
                                        " && ProcessControlButton() && LoadWait()" +
                                        ";");
                    break;
            }

            strMessages = oEnhancement.GetMessages(intID, false, "#E1FFE1");
            oTab.AddTab("Message Thread (" + oEnhancement.GetMessages(intID).Tables[0].Rows.Count.ToString() + ")", "");

            rptDocuments.DataSource = dsDocuments;
            rptDocuments.DataBind();
            lblDocuments.Visible = (rptDocuments.Items.Count == 0);
            foreach (RepeaterItem ri in rptDocuments.Items)
            {
                LinkButton _delete = (LinkButton)ri.FindControl("btnDeleteDocument");
                _delete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this document?');");
            }
            oTab.AddTab("Functional Requirements Documentation (" + rptDocuments.Items.Count.ToString() + ")", "");
            oTab.AddTab("Log (" + oEnhancement.LoadLog(intID, rptLog, lblLog) + ")", "");
            oTab.AddTab("Release Notes", "");
            strMenuTab1 = oTab.GetTabs();
        }
        private int LoadStep(int _step, int _current, Image _img, HtmlTableRow _wait, HtmlTableRow _row, HtmlTableRow _done, Label _lbl, DataSet _steps)
        {
            if (_current < _step)
            {
                _img.ImageUrl = "/images/bigHourGlass.gif";
                if (_wait != null)
                    _wait.Visible = true;
            }
            else
            {
                bool boolCompleted = false;
                bool boolApproved = false;
                foreach (DataRow dr in _steps.Tables[0].Rows)
                {
                    if (Int32.Parse(dr["step"].ToString()) == _step)
                    {
                        if (dr["completed"].ToString() != "")
                        {
                            boolCompleted = true;
                            _lbl.Text = "Completed on " + dr["completed"].ToString();
                        }
                        if (dr["approved"].ToString() != "")
                        {
                            boolApproved = true;
                            _lbl.Text += "Approved on " + dr["approved"].ToString();
                        }
                        break;
                    }
                }
                if (_current == _step && boolApproved == false)
                {
                    if (boolCompleted == true)
                    {
                        _img.ImageUrl = "/images/please_wait.gif";
                        btnSave.Enabled = false;
                        btnSave.ImageUrl = "/images/tool_save_dbl.gif";
                        _done.Visible = true;
                        _lbl.Text += "&nbsp;...awaiting on approvals (see Log tab)";
                    }
                    else
                    {
                        _img.ImageUrl = "/images/arrow_right.gif";
                        _row.Visible = true;
                    }
                    lblStep.Text = _current.ToString();
                }
                else
                {
                    if (_current == _step)
                        _current++;
                    _img.ImageUrl = "/images/bigCheckBox.gif";
                    _done.Visible = true;
                }
            }
            return _current;
        }
        protected void btnSave_Click(Object Sender, EventArgs e)
        {
            int intResourceWorkflow = Int32.Parse(lblResourceWorkflow.Text);
            int intResourceParent = oResourceRequest.GetWorkflowParent(intResourceWorkflow);
            oResourceRequest.UpdateWorkflowName(intResourceWorkflow, txtCustom.Text, intProfile);

            int intClient = oRequest.GetUser(intRequest);
            switch (Request.Form[hdnTab.UniqueID])
            {
                case "2":   // Execution
                    int intStep = Int32.Parse(lblStep.Text);
                    string strNow = DateTime.Now.ToString();
                    switch (intStep)
                    {
                        case 1:
                            // Functional Requirements
                            int intDays = 0;
                            int intVersion = 0;
                            if (filFunctional.FileName != "" && filFunctional.PostedFile != null && Int32.TryParse(txtDays.Text, out intDays) && Int32.TryParse(radEstimate.SelectedItem.Value, out intVersion))
                            {
                                string strExtension = filFunctional.FileName;
                                string strType = strExtension.Substring(0, 3);
                                strExtension = strExtension.Substring(strExtension.LastIndexOf("."));
                                string strFile = DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString() + "_" + intProfile.ToString() + strExtension;
                                string strPath = oVariable.UploadsFolder() + strFile;
                                oFile.PostedFile.SaveAs(strPath);

                                oEnhancement.AddDocument(intID, strPath, intDays, intVersion);
                            }
                            if (chk1.Checked)
                            {
                                oEnhancement.AddStep(intID, intStep, strNow, "");
                                oEnhancement.AddLog(intID, intStep, "Functional Requirements Approval", intProfile, "");
                                oResourceRequest.UpdateStatusOverall(intResourceParent, (int)EnhancementStatus.AwaitingApproval);
                            }
                            break;
                        case 2:
                            // Approvers
                            if (chk2.Checked)
                            {
                                oEnhancement.AddStep(intID, intStep, strNow, strNow);
                                oEnhancement.AddLog(intID, intStep, "Approvals Skipped", intProfile, strNow);
                                oResourceRequest.UpdateStatusOverall(intResourceParent, (int)EnhancementStatus.InDevelopment);
                            }
                            break;
                    }
                    Response.Redirect(Request.Path + "?rrid=" + intResourceWorkflow.ToString() + "&menu_tab=2&save=true");
                    break;
                case "3":   // Message Thread
                    if (txtText.Text != "")
                    {
                        string strXid = oUser.GetName(intClient);
                        string strVirtualPath = "";
                        string strFile = "";
                        if (oFile.FileName != "" && oFile.PostedFile != null)
                        {
                            string strExtension = oFile.FileName;
                            string strType = strExtension.Substring(0, 3);
                            strExtension = strExtension.Substring(strExtension.LastIndexOf("."));
                            strFile = DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString() + "_" + intProfile.ToString() + strExtension;
                            strVirtualPath = oVariable.UploadsFolder() + strFile;
                            string strPath = oVariable.UploadsFolder() + strFile;
                            oFile.PostedFile.SaveAs(strPath);
                        }
                        oEnhancement.AddMessage(intID, txtText.Text, strVirtualPath, intProfile, 1);
                        string strDefault = oUser.GetApplicationUrl(intClient, intEnhancementPage);
                        string strBody = "";
                        if (strDefault != "")
                            strBody += "<p><a href=\"" + oVariable.URL() + "/redirect.aspx?referrer=/" + strDefault + oPage.GetFullLink(intEnhancementPage) + "?id=" + intID.ToString() + "\" target=\"_blank\">Click here to view this ticket or submit a response</a></p>";
                        strBody += "<table width=\"100%\" cellpadding=\"4\" cellspacing=\"3\" border=\"0\" align=\"center\" style=\"border:solid 1px #CCCCCC;" + oVariable.DefaultFontStyle() + "\">";
                        //DataSet dsMessages = oCustomized.GetMessages(intRequest, 0);
                        //foreach (DataRow drMessage in dsMessages.Tables[0].Rows)
                        //{
                        //    strBody += "<tr bgcolor=\"#EEEEEE\"><td><span style=\"" + oVariable.DefaultFontStyleHeader() + "\">" + (drMessage["admin"].ToString() == "1" ? "ClearView Administrator" : oUser.GetFullName(intProfile)) + "</span>&nbsp;&nbsp;[" + DateTime.Parse(drMessage["created"].ToString()).ToString() + "]:</td></tr>";
                        //    strBody += "<tr><td>" + drMessage["message"].ToString() + "</td></tr>";
                        //    strBody += "<tr><td>&nbsp;</td></tr>";
                        //}
                        strBody += "<tr bgcolor=\"#EEEEEE\"><td><span style=\"" + oVariable.DefaultFontStyleHeader() + "\">ClearView Administrator</span>&nbsp;&nbsp;[" + DateTime.Now.ToString() + "]:</td></tr>";
                        strBody += "<tr><td>" + oFunction.FormatText(txtText.Text) + "</td></tr>";
                        if (strVirtualPath != "")
                        {
                            strBody += "<tr><td style=\"border-bottom:dashed 1px #CCCCCC\">&nbsp;</td></tr>";
                            //strBody += "<tr><td><img src=\"" + oVariable.ImageURL() + "/images/file.gif\" align=\"absmiddle\" border=\"0\"/> <a href=\"" + oVariable.URL() + strVirtualPath + "\" target=\"_blank\">" + strFile + "</a></td></tr>";
                            strBody += "<tr><td><img src=\"" + oVariable.ImageURL() + "/images/file.gif\" align=\"absmiddle\" border=\"0\"/> <a href=\"" + strVirtualPath + "\" target=\"_blank\">" + strFile + "</a></td></tr>";
                        }
                        strBody += "</table>";

                        strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DEVELOPER_ALERT");
                        oFunction.SendEmail("Enhancement Response [CVT" + intRequest.ToString() + "]", strXid, "", strEMailIdsBCC, "Enhancement Response [#CVT" + intRequest.ToString() + "]", "<p>" + oEnhancement.GetBody(intID, intEnvironment) + "</p><p>" + strBody + "</p>", true, false);
                        Response.Redirect(Request.Path + "?rrid=" + intResourceWorkflow.ToString() + "&menu_tab=3&save=true");
                    }
                    break;
            }
        }
        protected void btnComplete_Click(Object Sender, EventArgs e)
        {
            int intResourceWorkflow = Int32.Parse(lblResourceWorkflow.Text);
            int intResourceParent = oResourceRequest.GetWorkflowParent(intResourceWorkflow);
            oResourceRequest.UpdateWorkflowStatus(intResourceWorkflow, 3, true);
            //oCustomized.UpdateEnhancementStatus(intRequest, 3);
            oResourceRequest.CloseWorkflow(intResourceWorkflow, intEnvironment, 0, dsnServiceEditor, true, intResourceRequestApprove, intAssignPage, intViewPage, dsnAsset, dsnIP);
            Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "reload", "<script type=\"text/javascript\">RefreshOpeningWindow();window.close();<" + "/" + "script>");
        }
        protected void btnDeleteDocument_Click(Object Sender, EventArgs e)
        {
            int intResourceWorkflow = Int32.Parse(lblResourceWorkflow.Text);
            LinkButton oDelete = (LinkButton)Sender;
            oEnhancement.DeleteDocument(Int32.Parse(oDelete.CommandArgument));
            Response.Redirect(Request.Path + "?rrid=" + intResourceWorkflow.ToString() + "&menu_tab=4&save=true");
        }
        
    }
}