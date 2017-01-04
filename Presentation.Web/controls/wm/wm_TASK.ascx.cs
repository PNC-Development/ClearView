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
    public partial class wm_TASK : System.Web.UI.UserControl
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
        protected RequestFields oRequestField;
        protected Applications oApplication;
        protected Documents oDocument;
        protected Delegates oDelegate;
        protected Variables oVariables;
        protected int intApplication = 0;
        protected int intPage = 0;
        protected int intProject = 0;
        protected bool boolExecution = false;
        protected bool boolChange = false;
        protected bool boolMyDocuments = false;
        protected int intRequest = 0;
        protected int intItem = 0;
        protected int intNumber = 0;

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
            oRequestField = new RequestFields(intProfile, dsn);
            oApplication = new Applications(intProfile, dsn);
            oDocument = new Documents(intProfile, dsn);
            oDelegate = new Delegates(intProfile, dsn);
            oVariables = new Variables(intEnvironment);
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
                double dblAllocated = double.Parse(oResourceRequest.GetWorkflow(intResourceWorkflow, "allocated"));
                // End Workflow Change
                int intService = Int32.Parse(ds.Tables[0].Rows[0]["serviceid"].ToString());
                int intApp = oRequestItem.GetItemApplication(intItem);

                if (Request.QueryString["save"] != null && Request.QueryString["save"] != "")
                    Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "saved", "<script type=\"text/javascript\">alert('Information Saved Successfully');<" + "/" + "script>");
                if (Request.QueryString["status"] != null && Request.QueryString["status"] != "")
                    Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "statusd", "<script type=\"text/javascript\">alert('Status Updates has been Added');<" + "/" + "script>");
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
                if (oService.Get(intService, "sla") != "")
                {
                    oFunction.ConfigureToolButton(btnSLA, "/images/tool_sla");
                    int intDays = oResourceRequest.GetSLA(intResourceParent);
                    if (intDays < 1)
                        btnSLA.Style["border"] = "solid 2px #FF0000";
                    else if (intDays < 3)
                        btnSLA.Style["border"] = "solid 2px #FF9999";
                    btnSLA.Attributes.Add("onclick", "return OpenWindow('RESOURCE_REQUEST_SLA','?rrid=" + intResourceParent.ToString() + "');");
                }
                else
                {
                    btnSLA.ImageUrl = "/images/tool_sla_dbl.gif";
                    btnSLA.Enabled = false;
                }
                oFunction.ConfigureToolButton(btnEmail, "/images/tool_email");
                btnEmail.Attributes.Add("onclick", "return OpenWindow('RESOURCE_REQUEST_EMAIL','?rrid=" + intResourceWorkflow.ToString() + "&type=TASK');");
                dblUsed = (dblUsed / dblAllocated) * 100;
                sldHours._StartPercent = dblUsed.ToString();
                sldHours._TotalHours = dblAllocated.ToString();
                if (Request.QueryString["save"] != null && Request.QueryString["save"] != "")
                    Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "saved", "<script type=\"text/javascript\">alert('Information Saved Successfully');<" + "/" + "script>");
                if (Request.QueryString["status"] != null && Request.QueryString["status"] != "")
                    Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "statusd", "<script type=\"text/javascript\">alert('Status Updates has been Added');<" + "/" + "script>");
                intProject = oRequest.GetProjectNumber(intRequest);
                if (intProject == 0)
                {
                    // Documents
                    btnDocuments.Attributes.Add("onclick", "return OpenWindow('DOCUMENTS_SECURE','?rid=" + intRequest.ToString() + "');");
                    chkMyDescription.Checked = (Request.QueryString["mydoc"] != null);
                    if (intProject > 0)
                        lblMyDocuments.Text = oDocument.GetDocuments_Mine(intProject, intProfile, oVariables.DocumentsFolder(), -1, (Request.QueryString["mydoc"] != null));
                    else
                        lblMyDocuments.Text = oDocument.GetDocuments_Request(intRequest, intProfile, oVariables.DocumentsFolder(), -1, (Request.QueryString["mydoc"] != null));
                    // GetDocuments(string _physical, int _projectid, int _requestid, int _userid, int _security, bool _show_description, bool _mine)
                    //lblMyDocuments.Text = oDocument.GetDocuments(Request.PhysicalApplicationPath, intProject, intRequest, intProfile, -1, (Request.QueryString["mydoc"] != null), true);
                    panWorkload.Visible = true;
                    LoadStatus(intResourceWorkflow);
                    LoadChange(intResourceWorkflow);
                    LoadInformation(intResourceWorkflow);

                    btnDenied.Attributes.Add("onclick", "return CloseWindow();");
                    oFunction.ConfigureToolButton(btnSave, "/images/tool_save");
                    oFunction.ConfigureToolButton(btnPrint, "/images/tool_print");
                    btnPrint.Attributes.Add("onclick", "return PrintWindow();");
                    oFunction.ConfigureToolButton(btnClose, "/images/tool_close");
                    btnClose.Attributes.Add("onclick", "return ExitWindow();");
                    btnSave.Attributes.Add("onclick", "return ValidateStatus('" + ddlStatus.ClientID + "','" + txtComments.ClientID + "');");
                    btnChange.Attributes.Add("onclick", "return ValidateText('" + txtNumber.ClientID + "','Please enter a change control number')" +
                        " && ValidateDate('" + txtDate.ClientID + "','Please enter a valid implementation date')" +
                        " && ValidateTime('" + txtTime.ClientID + "','Please enter a valid implementation time')" +
                        ";");
                    imgDate.Attributes.Add("onclick", "return ShowCalendar('" + txtDate.ClientID + "');");

                    // 6/1/2009 - Load ReadOnly View
                    if (oResourceRequest.CanUpdate(intProfile, intResourceWorkflow, false) == false)
                    {
                        oFunction.ConfigureToolButtonRO(btnSave, btnComplete);
                        //panDenied.Visible = true;
                    }
                }
                else
                    panDenied.Visible = true;
            }
            else
                panDenied.Visible = true;
        }
        private void LoadStatus(int _resourceid)
        {
            DataSet dsStatus = oResourceRequest.GetStatuss(_resourceid);
            rptStatus.DataSource = dsStatus;
            rptStatus.DataBind();
            lblNoStatus.Visible = (rptStatus.Items.Count == 0);
            double dblTotalStatus = 0.00;
            foreach (RepeaterItem ri in rptStatus.Items)
            {
                Label _status = (Label)ri.FindControl("lblStatus");
                double dblStatus = double.Parse(_status.Text);
                if (dblTotalStatus == 0.00)
                    dblTotalStatus = dblStatus;
                _status.Text = oResourceRequest.GetStatus(dblStatus, 50, 15);
            }
            lblStatus.Text = oResourceRequest.GetStatus(dblTotalStatus, 50, 15);
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
        private void LoadInformation(int intResourceWorkflow)
        {
            int intResourceParent = oResourceRequest.GetWorkflowParent(intResourceWorkflow);
            int intRequest = Int32.Parse(oResourceRequest.Get(intResourceParent, "requestid"));
            DataSet ds = oRequest.Get(intRequest);
            lblName.Text = oResourceRequest.GetWorkflow(intResourceWorkflow, "name");
            lblStart.Text = DateTime.Parse(ds.Tables[0].Rows[0]["start_date"].ToString()).ToLongDateString();
            lblEnd.Text = DateTime.Parse(ds.Tables[0].Rows[0]["end_date"].ToString()).ToLongDateString();
            lblDescription.Text = ds.Tables[0].Rows[0]["description"].ToString();
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
                    case "M":
                        boolMyDocuments = true;
                        break;
                }
            }
            if (boolExecution == false && boolChange == false && boolMyDocuments == false)
                boolExecution = true;
        }
        protected void chkMyDescription_Change(Object Sender, EventArgs e)
        {
            if (chkMyDescription.Checked == true)
                Response.Redirect(oPage.GetFullLink(intPage) + "?pid=" + Request.QueryString["pid"] + "&mydoc=true&div=M");
            else
                Response.Redirect(oPage.GetFullLink(intPage) + "?pid=" + Request.QueryString["pid"] + "&div=M");
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
            int intResourceWorkflow = Int32.Parse(lblResourceWorkflow.Text);
            int intResourceParent = oResourceRequest.GetWorkflowParent(intResourceWorkflow);
            LinkButton oButton = (LinkButton)Sender;
            oResourceRequest.DeleteChangeControl(Int32.Parse(oButton.CommandArgument));
            Response.Redirect(Request.Path + "?rrid=" + intResourceWorkflow.ToString() + "&div=C");
        }
        protected void btnSave_Click(Object Sender, EventArgs e)
        {
            int intResourceWorkflow = Int32.Parse(lblResourceWorkflow.Text);
            int intResourceParent = oResourceRequest.GetWorkflowParent(intResourceWorkflow);
            if (ddlStatus.SelectedIndex > -1 && txtComments.Text.Trim() != "")
            {
                oResourceRequest.AddStatus(intResourceWorkflow, Int32.Parse(ddlStatus.SelectedItem.Value), txtComments.Text, intProfile);
                //CVT62149 Workload Manager Red Light Status =Hold
                if (ddlStatus.SelectedValue == "1") //Red
                    oResourceRequest.UpdateWorkflowStatus(intResourceWorkflow, 5, true);
                else
                    oResourceRequest.UpdateWorkflowStatus(intResourceWorkflow, 2, true); 
            }
            double dblHours = 0.00;
            if (Request.Form["hdnHours"] != null && Request.Form["hdnHours"] != "")
                dblHours = double.Parse(Request.Form["hdnHours"]);
            double dblUsed = oResourceRequest.GetWorkflowUsed(intResourceWorkflow);
            dblHours = (dblHours - dblUsed);
            if (dblHours > 0.00)
                oResourceRequest.UpdateWorkflowHours(intResourceWorkflow, dblHours);
            Response.Redirect(Request.Path + "?rrid=" + intResourceWorkflow.ToString() + "&save=true");
        }
        protected void btnComplete_Click(Object Sender, EventArgs e)
        {
            int intResourceWorkflow = Int32.Parse(lblResourceWorkflow.Text);
            int intResourceParent = oResourceRequest.GetWorkflowParent(intResourceWorkflow);
            oResourceRequest.UpdateWorkflowStatus(intResourceWorkflow, 3, true);
            oResourceRequest.CloseWorkflow(intResourceWorkflow, intEnvironment, 0, dsnServiceEditor, true, intResourceRequestApprove, intAssignPage, intViewPage, dsnAsset, dsnIP);
            Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "reload", "<script type=\"text/javascript\">RefreshOpeningWindow();window.close();<" + "/" + "script>");
        }
    }
}