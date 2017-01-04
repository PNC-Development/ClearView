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
    public partial class wm_audit_error : System.Web.UI.UserControl
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
        protected Audit oAudit;
        protected OnDemandTasks oOnDemandTasks;
        protected int intApplication = 0;
        protected int intPage = 0;
        protected int intProject = 0;
        protected bool boolDetails = false;
        protected bool boolExecution = false;
        protected int intRequest = 0;
        protected int intItem = 0;
        protected int intService = 0;
        protected int intNumber = 0;
        protected bool boolJoined = false;
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
            oAudit = new Audit(intProfile, dsn);
            oOnDemandTasks = new OnDemandTasks(intProfile, dsn);

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
                boolJoined = (oResourceRequest.GetWorkflow(intResourceWorkflow, "joined") == "1");
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
                    lblUpdated.Text = oResourceRequest.GetUpdated(intResourceParent);
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
                    bool boolSLABreached = false;
                    if (oService.Get(intService, "sla") != "")
                    {
                        oFunction.ConfigureToolButton(btnSLA, "/images/tool_sla");
                        int intDays = oResourceRequest.GetSLA(intResourceParent);
                        if (intDays > -99999)
                        {
                            if (intDays < 1)
                                btnSLA.Style["border"] = "solid 2px #FF0000";
                            else if (intDays < 3)
                                btnSLA.Style["border"] = "solid 2px #FF9999";
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
                    intProject = oRequest.GetProjectNumber(intRequest);
                    hdnTab.Value = "D";
                    panWorkload.Visible = true;
                    bool boolRed = LoadStatus(intResourceWorkflow);
                    string strValidSave = "";
                    string strValidComplete = "";
                    if (boolRed == false && boolSLABreached == true)
                        btnComplete.Attributes.Add("onclick", "alert('NOTE: Your Service Level Agreement (SLA) has been breached!\\n\\nYou must provide a RED STATUS update with an explanation of why your SLA was breached for this request.\\n\\nOnce a RED STATUS update has been provided, you will be able to complete this request.');return false;");
                    else
                    {
                        btnComplete.Attributes.Add("onclick", "return ValidateRadioButtons('" + radActionIgnore.ClientID + "','" + radActionFix.ClientID + "','Please select an action')" +
                            " && (document.getElementById('" + radActionIgnore.ClientID + "').checked == false || (document.getElementById('" + radActionIgnore.ClientID + "').checked == true && ValidateText('" + txtReason.ClientID + "','Please enter the reason for ignoring this message')))" +
                            " && confirm('Are you sure you want to mark this as completed and remove it from your workload?')" +
                            ";");
                    }
                    LoadInformation(intResourceWorkflow);

                    radActionIgnore.Attributes.Add("onclick", "ShowHideDiv('" + trIgnore.ClientID + "','inline');");
                    radActionFix.Attributes.Add("onclick", "ShowHideDiv('" + trIgnore.ClientID + "','none');");
                    btnDenied.Attributes.Add("onclick", "return CloseWindow();");
                    oFunction.ConfigureToolButton(btnSave, "/images/tool_save");
                    oFunction.ConfigureToolButton(btnPrint, "/images/tool_print");
                    btnPrint.Attributes.Add("onclick", "return PrintWindow();");
                    oFunction.ConfigureToolButton(btnClose, "/images/tool_close");
                    btnClose.Attributes.Add("onclick", "return ExitWindow();");
                    btnSave.Attributes.Add("onclick", "return ValidateRadioButtons('" + radActionIgnore.ClientID + "','" + radActionFix.ClientID + "','Please select an action')" +
                        " && (document.getElementById('" + radActionIgnore.ClientID + "').checked == false || (document.getElementById('" + radActionIgnore.ClientID + "').checked == true && ValidateText('" + txtReason.ClientID + "','Please enter the reason for ignoring this message')))" +
                        " && ValidateStatus('" + ddlStatus.ClientID + "','" + txtComments.ClientID + "')" +
                        ";");

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
            DataSet dsAudit = oAudit.GetError(intRequest, intService, intNumber);
            if (dsAudit.Tables[0].Rows.Count > 0)
            {
                radActionFix.Checked = (dsAudit.Tables[0].Rows[0]["fixed"].ToString() == "1");
                radActionIgnore.Checked = (dsAudit.Tables[0].Rows[0]["fixed"].ToString() == "0");
                if (radActionIgnore.Checked)
                {
                    trIgnore.Style["display"] = "inline";
                    txtReason.Text = dsAudit.Tables[0].Rows[0]["reason"].ToString();
                }
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
        protected void btnStatus_Click(Object Sender, EventArgs e)
        {
            int intResourceWorkflow = Int32.Parse(lblResourceWorkflow.Text);
            int intResourceParent = oResourceRequest.GetWorkflowParent(intResourceWorkflow);
            oResourceRequest.AddStatus(intResourceWorkflow, Int32.Parse(ddlStatus.SelectedItem.Value), txtComments.Text, intProfile);
            Response.Redirect(Request.Path + "?rrid=" + intResourceWorkflow.ToString() + "&div=E&status=true");
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
            double dblAllocated = double.Parse(oResourceRequest.GetWorkflow(intResourceWorkflow, "allocated"));
            oResourceRequest.UpdateWorkflowHoursOverwrite(intResourceWorkflow, dblAllocated);
            oAudit.UpdateError(intRequest, intService, intNumber, (radActionFix.Checked ? 1 : 0), (radActionIgnore.Checked ? txtReason.Text : ""));
            oOnDemandTasks.UpdateServerOther(intRequest, intItem, intNumber, (radActionFix.Checked ? 1 : 0));
            oResourceRequest.UpdateWorkflowName(intResourceWorkflow, txtCustom.Text, intProfile);
            Response.Redirect(Request.Path + "?rrid=" + intResourceWorkflow.ToString() + "&div=E&save=true");
        }
        protected void btnComplete_Click(Object Sender, EventArgs e)
        {
            int intResourceWorkflow = Int32.Parse(lblResourceWorkflow.Text);
            int intResourceParent = oResourceRequest.GetWorkflowParent(intResourceWorkflow);

            // Add a green / completed status if there are no updates, OR the last status is not green
            DataSet dsStatus = oResourceRequest.GetStatuss(intResourceWorkflow);
            if (dsStatus.Tables[0].Rows.Count == 0 || dsStatus.Tables[0].Rows[0]["status"].ToString() != "3")
                oResourceRequest.AddStatus(intResourceWorkflow, 3, "Completed", intProfile);

            int intAudit = 0;
            int intStep = 0;
            int intMIS = 0;
            DataSet dsAudit = oAudit.GetError(intRequest, intService, intNumber);
            if (dsAudit.Tables[0].Rows.Count > 0)
            {
                Int32.TryParse(dsAudit.Tables[0].Rows[0]["auditid"].ToString(), out intAudit);
                Int32.TryParse(dsAudit.Tables[0].Rows[0]["step"].ToString(), out intStep);
                Int32.TryParse(dsAudit.Tables[0].Rows[0]["mis"].ToString(), out intMIS);
            }
            if (intAudit > 0) 
            {
                int intServer = 0;
                dsAudit = oAudit.GetServer(intAudit);
                if (dsAudit.Tables[0].Rows.Count > 0)
                    Int32.TryParse(dsAudit.Tables[0].Rows[0]["serverid"].ToString(), out intServer);
                if (intServer > 0)
                {
                    oAudit.UpdateError(intRequest, intService, intNumber, (radActionFix.Checked ? 1 : 0), (radActionIgnore.Checked ? txtReason.Text : ""));
                    oAudit.UpdateErrorCompleted(intRequest, intService, intNumber);
                    // If fixed, clear the errors
                    if (radActionFix.Checked == true)
                    {
                        // Delete all audit scripts and set step to start again
                        oAudit.DeleteServer(intServer, intMIS);
                        oAudit.DeleteServerDetailRemote(intAudit);
                    }
                    if (radActionIgnore.Checked == true)
                    {
                        // Set the ERROR to IGNORE (which will skip the current script and move onto the next one).
                        oAudit.UpdateServer(intAudit, AuditStatus.Ignore, DateTime.Now.ToString());
                    }

                    // Update the error (in cv_servers_errors and cv_ondemand_tasks_done_server)
                    Servers oServer = new Servers(intProfile, dsn);
                    oServer.UpdateError(intServer, intStep, 0, intProfile, false, dsnAsset);
                    if (intMIS == 1)
                        oServer.UpdateMISAudits(intServer, "");

                    // Close Request
                    oResourceRequest.UpdateWorkflowStatus(intResourceWorkflow, 3, true);
                    oResourceRequest.CloseWorkflow(intResourceWorkflow, intEnvironment, 0, dsnServiceEditor, true, intResourceRequestApprove, intAssignPage, intViewPage, dsnAsset, dsnIP);
                    Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "reload", "<script type=\"text/javascript\">RefreshOpeningWindow();window.close();<" + "/" + "script>");
                }
                else
                    Page.ClientScript.RegisterStartupScript(typeof(Page), "missing_server", "<script type=\"text/javascript\">alert('Could not find the server ID ( " + dsAudit.Tables[0].Rows[0]["serverid"].ToString() + ")');<" + "/" + "script>");
            }
            else
                Page.ClientScript.RegisterStartupScript(typeof(Page), "missing_audit", "<script type=\"text/javascript\">alert('Could not find the audit ID ( " + dsAudit.Tables[0].Rows[0]["auditid"].ToString() + ")');<" + "/" + "script>");
        }
    }
}