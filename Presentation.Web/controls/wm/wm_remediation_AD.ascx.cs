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
using System.DirectoryServices;
namespace NCC.ClearView.Presentation.Web
{
    public partial class wm_remediation_AD : System.Web.UI.UserControl
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
        protected ADObject oADObject;
        protected Applications oApplication;
        protected Delegates oDelegate;
        protected int intApplication = 0;
        protected int intPage = 0;
        protected int intProject = 0;
        protected int intRequest = 0;
        protected int intItem = 0;
        protected int intNumber = 0;
        protected int intADCount = 0;
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
            oADObject = new ADObject(intProfile, dsn);
            oApplication = new Applications(intProfile, dsn);
            oDelegate = new Delegates(intProfile, dsn);
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
                int intService = Int32.Parse(ds.Tables[0].Rows[0]["serviceid"].ToString());
                lblService.Text = oService.Get(intService, "name");
                int intApp = oRequestItem.GetItemApplication(intItem);

                if (!IsPostBack)
                {
                    rptView.DataSource = oADObject.GetDomainAccounts(intRequest);
                    rptView.DataBind();
                    intADCount = rptView.Items.Count;
                    lblNoAD.Visible = (intADCount == 0);
                    btnCheck.Enabled = (intADCount > 0);
                }
                double dblUsed = oResourceRequest.GetWorkflowUsed(intResourceWorkflow);
                lblUpdated.Text = oResourceRequest.GetUpdated(intResourceWorkflow);
                if (dblAllocated == dblUsed || intADCount == 0)
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
                lblUsed.Text = dblUsed.ToString("F");
                if (Request.QueryString["save"] != null && Request.QueryString["save"] != "")
                    Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "saved", "<script type=\"text/javascript\">alert('Information Saved Successfully');<" + "/" + "script>");
                if (Request.QueryString["status"] != null && Request.QueryString["status"] != "")
                    Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "statusd", "<script type=\"text/javascript\">alert('Status Updates has been Added');<" + "/" + "script>");
                intProject = oRequest.GetProjectNumber(intRequest);
                panWorkload.Visible = true;
                LoadInformation(intResourceWorkflow);
                panWorkload.Visible = true;
                if (Request.QueryString["check"] != null)
                {
                    if (Request.QueryString["check"] == "yes")
                    {
                        foreach (RepeaterItem ri in rptView.Items)
                        {
                            CheckBox chkDelete = (CheckBox)ri.FindControl("chkDelete");
                            chkDelete.Checked = true;
                        }
                        btnCheck.Text = "Uncheck All";
                    }
                    if (Request.QueryString["check"] == "no")
                    {
                        foreach (RepeaterItem ri in rptView.Items)
                        {
                            CheckBox chkDelete = (CheckBox)ri.FindControl("chkDelete");
                            chkDelete.Checked = false;
                        }
                        btnCheck.Text = "Check All";
                    }
                }
                btnDenied.Attributes.Add("onclick", "return CloseWindow();");
                oFunction.ConfigureToolButton(btnSave, "/images/tool_save");
                oFunction.ConfigureToolButton(btnPrint, "/images/tool_print");
                btnPrint.Attributes.Add("onclick", "return PrintWindow();");
                oFunction.ConfigureToolButton(btnClose, "/images/tool_close");
                btnClose.Attributes.Add("onclick", "return ExitWindow();");
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
        private void LoadInformation(int _request)
        {
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
        }
        protected void btnCheck_Click(Object Sender, EventArgs e)
        {
            int intResourceWorkflow = Int32.Parse(lblResourceWorkflow.Text);
            if (btnCheck.Text == "Check All")
                Response.Redirect(Request.Path + "?rrid=" + intResourceWorkflow.ToString() + "&check=yes");
            else
                Response.Redirect(Request.Path + "?rrid=" + intResourceWorkflow.ToString() + "&check=no");
        }
        protected void btnSave_Click(Object Sender, EventArgs e)
        {
            int intResourceWorkflow = Int32.Parse(lblResourceWorkflow.Text);
            int intResourceParent = oResourceRequest.GetWorkflowParent(intResourceWorkflow);
            oResourceRequest.UpdateWorkflowName(intResourceWorkflow, txtCustom.Text, intProfile);
            ds = oResourceRequest.Get(intResourceParent);
            int intRequest = Int32.Parse(ds.Tables[0].Rows[0]["requestid"].ToString());
            int intItem = Int32.Parse(ds.Tables[0].Rows[0]["itemid"].ToString());
            int intStatus = Int32.Parse(oResourceRequest.GetWorkflow(intResourceWorkflow, "status"));
            double dblHours = double.Parse(lblUsed.Text);
            double dblTime = 5.00 / 60.00;
            foreach (RepeaterItem ri in rptView.Items)
            {
                CheckBox chkDelete = (CheckBox)ri.FindControl("chkDelete");
                if (chkDelete.Checked == true)
                {
                    Label lblId = (Label)ri.FindControl("lblId");
                    Label lblPath = (Label)ri.FindControl("lblPath");
                    Label lblEnvironment = (Label)ri.FindControl("lblEnvironment");
                    Variables oVariable = new Variables(Int32.Parse(lblEnvironment.Text));
                    DirectoryEntry oEntry = new DirectoryEntry(lblPath.Text, oVariable.Domain() + "\\" + oVariable.ADUser(), oVariable.ADPassword());
                    AD oAD = new AD(intProfile, dsn, Int32.Parse(lblEnvironment.Text));
                    if (oAD.Exists(oEntry))
                        oAD.Enable(oEntry, false);
                    dblHours += dblTime;
                    oADObject.UpdateDomainAccount(Int32.Parse(lblId.Text), intProfile);
                }
            }
            // Delete objects over a month old
            ds = oADObject.GetDomainAccountsDisabled(DateTime.Today.AddMonths(-1));
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                int _environment = Int32.Parse(dr["environment"].ToString());
                Variables oVariable = new Variables(_environment);
                DirectoryEntry oEntry = new DirectoryEntry(dr["path"].ToString(), oVariable.Domain() + "\\" + oVariable.ADUser(), oVariable.ADPassword());
                AD oAD = new AD(intProfile, dsn, _environment);
                if (oAD.Exists(oEntry) && oAD.IsDisabled(oEntry) == true)
                {
                    oAD.Delete(oEntry);
                    oADObject.UpdateDomainAccountDisabled(Int32.Parse(dr["adid"].ToString()), -1);
                }
                else
                    oADObject.UpdateDomainAccountDisabled(Int32.Parse(dr["adid"].ToString()), -10);
            }
            double dblTotal = double.Parse(oResourceRequest.GetWorkflow(intResourceWorkflow, "allocated"));
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