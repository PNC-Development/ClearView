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
namespace NCC.ClearView.Presentation.Web
{
    public partial class wm_support : System.Web.UI.UserControl
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
        protected int intIssuePage = Int32.Parse(ConfigurationManager.AppSettings["HELP_ISSUE_PAGEID"]);
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
        protected Customized oCustomized;
        protected Variables oVariable;
        protected StatusLevels oStatusLevel;
        protected Enhancements oEnhancement;
        protected int intApplication = 0;
        protected int intPage = 0;
        protected int intProject = 0;
        protected bool boolDetails = false;
        protected bool boolExecution = false;
        protected int intRequest = 0;
        protected int intItem = 0;
        protected int intNumber = 0;
        private Variables oVariables;

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
            oCustomized = new Customized(intProfile, dsn);
            oVariable = new Variables(intEnvironment);
            oStatusLevel = new StatusLevels();
            oEnhancement = new Enhancements(intProfile, dsn);
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
                // Start Workflow Change
                bool boolComplete = (oResourceRequest.GetWorkflow(intResourceWorkflow, "status") == "3");
                int intUser = Int32.Parse(oResourceRequest.GetWorkflow(intResourceWorkflow, "userid"));
                txtCustom.Text = oResourceRequest.GetWorkflow(intResourceWorkflow, "name");
                double dblAllocated = double.Parse(oResourceRequest.GetWorkflow(intResourceWorkflow, "allocated"));
                // End Workflow Change
                intService = Int32.Parse(ds.Tables[0].Rows[0]["serviceid"].ToString());
                lblService.Text = oService.Get(intService, "name");
                int intApp = oRequestItem.GetItemApplication(intItem);

                strMessages = oCustomized.GetMessages(intRequest, false, "#E1FFE1");

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
                            btnComplete.Attributes.Add("onclick", "return confirm('Are you sure you want to mark this as completed and remove it from your workload?') && ProcessControlButton();");
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
                    sldHours._StartPercent = dblUsed.ToString();
                    sldHours._TotalHours = dblAllocated.ToString();
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
                    btnSave.Attributes.Add("onclick", "return ProcessControlButton();");
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
            ddlCurrentStatus.DataTextField = "name";
            ddlCurrentStatus.DataValueField = "id";
            ddlCurrentStatus.DataSource = SqlHelper.ExecuteDataset(dsn, CommandType.Text, oStatusLevel.SupportStatusList());
            ddlCurrentStatus.DataBind();
        }
        private void LoadStatus(int _resourceid)
        {
            int intResourceWorkflow = Int32.Parse(lblResourceWorkflow.Text);
            int intResourceParent = oResourceRequest.GetWorkflowParent(intResourceWorkflow);
            ddlCurrentStatus.SelectedValue = oResourceRequest.GetWorkflow(intResourceWorkflow, "status");
        }
        private void LoadInformation(int _request)
        {
            if (intNumber == 1) // number = 1 (Enhancement) ; number = 2 (Issue)  -- Vijay
            {
                lblView.Text = oCustomized.GetEnhancementBody(oCustomized.GetEnhancementID(intRequest), intEnvironment, true);
                DataSet dsReleases = oEnhancement.GetVersions();
                foreach (DataRow drRelease in dsReleases.Tables[0].Rows)
                {
                    string strName = DateTime.Parse(drRelease["release"].ToString()).ToShortDateString();
                    if (DateTime.Parse(drRelease["cutoff"].ToString()) < DateTime.Now)
                        strName += " (Closed)";
                    else if (drRelease["available"].ToString() != "1")
                        strName += " (Full)";
                    ddlRelease.Items.Add(new ListItem(strName, drRelease["id"].ToString()));
                }
                ddlRelease.Items.Insert(0, new ListItem("-- To Be Determined --", "0"));
                // Load data
                DataSet dsInfo = oCustomized.GetEnhancement(intRequest);
                if (dsInfo.Tables[0].Rows.Count > 0)
                {
                    ddlRelease.SelectedValue = dsInfo.Tables[0].Rows[0]["release"].ToString();
                    radPriority.SelectedValue = dsInfo.Tables[0].Rows[0]["priority"].ToString();
                }
                trPriority.Visible = true;
                trRelease.Visible = true;
            }
            else
                lblView.Text = oCustomized.GetIssueBody(oCustomized.GetIssueID(intRequest), intEnvironment, true);
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
            if (Request.QueryString["div"] != null)
            {
                switch (Request.QueryString["div"])
                {
                    case "E":
                        boolExecution = true;
                        break;
                }
            }
            if (boolDetails == false && boolExecution == false)
                boolDetails = true;
        }
        protected void btnSave_Click(Object Sender, EventArgs e)
        {
            oVariables = new Variables(intEnvironment);
            int intResourceWorkflow = Int32.Parse(lblResourceWorkflow.Text);
            int intResourceParent = oResourceRequest.GetWorkflowParent(intResourceWorkflow);
            double dblHours = 0.00;
            if (Request.Form["hdnHours"] != null && Request.Form["hdnHours"] != "")
                dblHours = double.Parse(Request.Form["hdnHours"]);
            double dblUsed = oResourceRequest.GetWorkflowUsed(intResourceWorkflow);
            dblHours = (dblHours - dblUsed);
            if (dblHours > 0.00)
                oResourceRequest.UpdateWorkflowHours(intResourceWorkflow, dblHours);
            oResourceRequest.UpdateWorkflowName(intResourceWorkflow, txtCustom.Text, intProfile);

            int intUser = oRequest.GetUser(intRequest);
            oResourceRequest.UpdateWorkflowStatus(intResourceWorkflow, Int32.Parse(ddlCurrentStatus.SelectedValue), true);
            if (intNumber == 1)
            {
                oCustomized.UpdateEnhancementStatus(intRequest, Int32.Parse(ddlCurrentStatus.SelectedValue), Int32.Parse(ddlRelease.SelectedItem.Value), radPriority.SelectedItem.Value);
                oCustomized.UpdateEnhancementNew(intRequest, 1);
            }
            else
            {
                oCustomized.UpdateIssueStatus(intRequest, Int32.Parse(ddlCurrentStatus.SelectedValue));
                oCustomized.UpdateIssueNew(intRequest, 1);
            }
            if (txtText.Text != "")
            {
                string strXid = oUser.GetName(intUser);
                char chType = (intNumber == 1 ? 'E' : 'S');
                string strVirtualPath = "";
                string strFile = "";
                if (oFile.FileName != "" && oFile.PostedFile != null)
                {
                    string strExtension = oFile.FileName;
                    string strType = strExtension.Substring(0, 3);
                    strExtension = strExtension.Substring(strExtension.LastIndexOf("."));
                    strFile = DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString() + "_" + intProfile.ToString() + strExtension;
                    strVirtualPath = oVariables.UploadsFolder() + strFile;
                    string strPath = oVariables.UploadsFolder() + strFile;
                    oFile.PostedFile.SaveAs(strPath);
                }
                oCustomized.AddMessage(intRequest, chType, txtText.Text, strVirtualPath, intApplication, intProfile, 1, 1);
                string strDefault = oUser.GetApplicationUrl(intUser, (intNumber == 1 ? intEnhancementPage : intIssuePage));
                string strBody = oFunction.EmailComments("ClearView Administrator", txtText.Text, strVirtualPath, strFile, false);
                //strBody += "<table width=\"100%\" cellpadding=\"4\" cellspacing=\"3\" border=\"0\" align=\"center\" style=\"border:solid 1px #779ccc;" + oVariable.DefaultFontStyle() + "\">";
                ////DataSet dsMessages = oCustomized.GetMessages(intRequest, 0);
                ////foreach (DataRow drMessage in dsMessages.Tables[0].Rows)
                ////{
                ////    strBody += "<tr bgcolor=\"#EEEEEE\"><td><span style=\"" + oVariable.DefaultFontStyleHeader() + "\">" + (drMessage["admin"].ToString() == "1" ? "ClearView Administrator" : oUser.GetFullName(intProfile)) + "</span>&nbsp;&nbsp;[" + DateTime.Parse(drMessage["created"].ToString()).ToString() + "]:</td></tr>";
                ////    strBody += "<tr><td>" + drMessage["message"].ToString() + "</td></tr>";
                ////    strBody += "<tr><td>&nbsp;</td></tr>";
                ////}
                //strBody += "<tr bgcolor=\"#f0f7ff\"><td><span style=\"" + oVariable.DefaultFontStyleHeader() + "\">ClearView Administrator</span>&nbsp;&nbsp;[" + DateTime.Now.ToString() + "]:</td></tr>";
                //strBody += "<tr><td>" + oFunction.FormatText(txtText.Text) + "</td></tr>";
                //if (strVirtualPath != "")
                //{
                //    strBody += "<tr><td style=\"border-bottom:dashed 1px #CCCCCC\">&nbsp;</td></tr>";
                //    strBody += "<tr><td><img src=\"" + oVariable.ImageURL() + "/images/file.gif\" align=\"absmiddle\" border=\"0\"/> <a href=\"" + strVirtualPath + "\" target=\"_blank\">" + strFile + "</a></td></tr>";
                //}
                //strBody += "</table>";
                if (strDefault != "")
                    strBody += "<p><a href=\"" + oVariable.URL() + "/redirect.aspx?referrer=/" + strDefault + oPage.GetFullLink((intNumber == 1 ? intEnhancementPage : intIssuePage)) + "?id=" + (intNumber == 1 ? oCustomized.GetEnhancementID(intRequest) : oCustomized.GetIssueID(intRequest)) + "\" target=\"_blank\">Click here to view this ticket or submit a response</a></p>";

                //strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DEVELOPER_ALERT");
                oFunction.SendEmail((intNumber == 1 ? "Enhancement" : "Support") + " Response [CVT" + intRequest.ToString() + "]", strXid, "", oUser.GetName(intProfile), (intNumber == 1 ? "Enhancement" : "Support") + " Response [#CVT" + intRequest.ToString() + "]", "<p>" + strBody + "</p><p>" + (intNumber == 1 ? oCustomized.GetEnhancementBody(oCustomized.GetEnhancementID(intRequest), intEnvironment, false) : oCustomized.GetIssueBody(oCustomized.GetIssueID(intRequest), intEnvironment, false)) + "</p>", true, false);
            }
            // Vijay Code - End

            Response.Redirect(Request.Path + "?rrid=" + intResourceWorkflow.ToString() + "&div=E&save=true");
        }
        protected void btnComplete_Click(Object Sender, EventArgs e)
        {
            int intResourceWorkflow = Int32.Parse(lblResourceWorkflow.Text);
            int intResourceParent = oResourceRequest.GetWorkflowParent(intResourceWorkflow);
            oResourceRequest.UpdateWorkflowStatus(intResourceWorkflow, 3, true);
            if (intNumber == 1)
            {
                oCustomized.UpdateEnhancementStatus(intRequest, 3, Int32.Parse(ddlRelease.SelectedItem.Value), radPriority.SelectedItem.Value);
                oCustomized.UpdateEnhancementNew(intRequest, 1);
            }
            else
            {
                oCustomized.UpdateIssueStatus(intRequest, 3);
                oCustomized.UpdateIssueNew(intRequest, 1);
            }
            oResourceRequest.CloseWorkflow(intResourceWorkflow, intEnvironment, 0, dsnServiceEditor, true, intResourceRequestApprove, intAssignPage, intViewPage, dsnAsset, dsnIP);
            Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "reload", "<script type=\"text/javascript\">RefreshOpeningWindow();window.close();<" + "/" + "script>");
        }
    }
}