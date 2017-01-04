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
    public partial class workflow_control : System.Web.UI.UserControl
    {

        private DataSet ds;
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected bool boolDirector = (ConfigurationManager.AppSettings["DirectorApproval"] == "1");
        protected int intResourcePage = Int32.Parse(ConfigurationManager.AppSettings["AssignWorkload"]);
        protected int intResourceRequestPage = Int32.Parse(ConfigurationManager.AppSettings["AssignWorkload"]);
        protected int intWorkflowPage = Int32.Parse(ConfigurationManager.AppSettings["WorkflowSuffix"]);
        protected int intWorkloadPage = Int32.Parse(ConfigurationManager.AppSettings["ViewWorkload"]);
        protected int intViewRequest = Int32.Parse(ConfigurationManager.AppSettings["ViewRequest"]);
        protected string strWorkflowBCC = ConfigurationManager.AppSettings["WorkflowBCC"];
        protected int intProfile;
        protected ProjectRequest oProjectRequest;
        protected ProjectRequest_Approval oApprove;
        protected Projects oProject;
        protected Functions oFunction;
        protected AD oAD;
        protected Users oUser;
        protected Applications oApplication;
        protected Pages oPage;
        protected AppPages oAppPage;
        protected Variables oVariable;
        protected Organizations oOrganization;
        protected RequestItems oRequestItem;
        protected Services oService;
        protected ResourceRequest oResourceRequest;
        protected Requests oRequest;
        protected Documents oDocument;
        protected int intApplication = 0;
        protected int intPage = 0;
        protected string strPriority;
        protected bool boolDetails = false;
        protected bool boolDocuments = false;
        protected bool boolDiscussion = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oProjectRequest = new ProjectRequest(intProfile, dsn);
            oApprove = new ProjectRequest_Approval(intProfile, dsn, intEnvironment);
            oProject = new Projects(intProfile, dsn);
            oAD = new AD(intProfile, dsn, intEnvironment);
            oFunction = new Functions(intProfile, dsn, intEnvironment);
            oUser = new Users(intProfile, dsn);
            int intRequest = 0;
            oApplication = new Applications(intProfile, dsn);
            oPage = new Pages(intProfile, dsn);
            oAppPage = new AppPages(intProfile, dsn);
            oVariable = new Variables(intEnvironment);
            oOrganization = new Organizations(intProfile, dsn);
            oRequestItem = new RequestItems(intProfile, dsn);
            oService = new Services(intProfile, dsn);
            oResourceRequest = new ResourceRequest(intProfile, dsn);
            oRequest = new Requests(intProfile, dsn);
            oDocument = new Documents(intProfile, dsn);
            bool boolWorkflow = false;
            if (Request.QueryString["action"] != null && Request.QueryString["action"] != "")
            {
                panFinish.Visible = true;
                if (Request.QueryString["rid"] != null && Request.QueryString["rid"] != "")
                    lblRequest.Text = Request.QueryString["rid"];
            }
            else
            {
                if (Request.QueryString["applicationid"] != null && Request.QueryString["applicationid"] != "")
                    intApplication = Int32.Parse(Request.QueryString["applicationid"]);
                if (Request.QueryString["pageid"] != null && Request.QueryString["pageid"] != "")
                    intPage = Int32.Parse(Request.QueryString["pageid"]);
                if (Request.Cookies["application"] != null && Request.Cookies["application"].Value != "")
                    intApplication = Int32.Parse(Request.Cookies["application"].Value);
                if (!IsPostBack)
                {
                    if (Request.QueryString["rid"] != null && Request.QueryString["rid"] != "")
                    {
                        intRequest = Int32.Parse(Request.QueryString["rid"]);
                        lblRequest.Text = Request.QueryString["rid"];
                    }
                    if (intRequest > 0)
                    {
                        ds = oApprove.Get(intRequest, intProfile);
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            bool boolButtons = false;
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                if (dr["approval"].ToString() == "0")
                                {
                                    boolButtons = true;
                                    lblStep.Text = dr["step"].ToString();
                                }
                                boolWorkflow = true;
                                panDiscussion.Visible = true;
                                rptComments.DataSource = oProjectRequest.GetComments(intRequest);
                                rptComments.DataBind();
                                foreach (RepeaterItem ri in rptComments.Items)
                                {
                                    LinkButton oDelete = (LinkButton)ri.FindControl("btnDelete");
                                    oDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this comment?');");
                                }
                                lblNoComments.Visible = (rptComments.Items.Count == 0);
                                lblUser.Text = oUser.GetFullName(oUser.GetName(intProfile));
                            }
                            btnApprove.Enabled = boolButtons;
                            btnDeny.Enabled = boolButtons;
                            btnShelf.Enabled = boolButtons;
                        }
                    }
                    else if (Int32.Parse(oProjectRequest.Get(intRequest, "userid")) == intProfile)
                    {
                        boolWorkflow = true;
                        panDiscussionX.Visible = true;
                    }
                    if (boolWorkflow == true)
                    {
                        panWorkflow.Visible = true;
                        string strApplication = "";
                        if (intApplication > 0)
                        {
                            ds = oApplication.Get(intApplication);
                            strApplication = ds.Tables[0].Rows[0]["url"].ToString();
                        }
                        ds = oProjectRequest.Get(intRequest);
                        DataSet dsRequest = oRequest.Get(intRequest);
                        int intProject = Int32.Parse(dsRequest.Tables[0].Rows[0]["projectid"].ToString());
                        DataSet dsProject = oProject.Get(intProject);
                        // PRIORITY
                        strPriority = "<div align=\"center\">" + oProjectRequest.GetPriority(intRequest, intEnvironment) + "</div>";
                        // DOCUMENTS
                        chkDescription.Checked = (Request.QueryString["doc"] != null);
                        lblDocuments.Text = oDocument.GetDocuments_Project(intProject, intProfile, oVariable.DocumentsFolder(), 1, (Request.QueryString["doc"] != null));
                        // GetDocuments(string _physical, int _projectid, int _requestid, int _userid, int _security, bool _show_description, bool _mine)
                        //lblDocuments.Text = oDocument.GetDocuments(Request.PhysicalApplicationPath, intProject, 0, intProfile, 1, (Request.QueryString["doc"] != null), false);
                        // DETAILS
                        int intRequestor = Int32.Parse(dsRequest.Tables[0].Rows[0]["userid"].ToString());
                        lblName2.Text = oUser.GetFullName(intRequestor);
                        lblDate2.Text = ds.Tables[0].Rows[0]["modified"].ToString();
                        lblProjectTask.Text = dsProject.Tables[0].Rows[0]["name"].ToString();
                        lblOrganization.Text = oOrganization.GetName(Int32.Parse(dsProject.Tables[0].Rows[0]["organization"].ToString()));
                        lblBaseDisc.Text = dsProject.Tables[0].Rows[0]["bd"].ToString();
                        lblClarity2.Text = dsProject.Tables[0].Rows[0]["number"].ToString();
                        if (lblClarity2.Text == "")
                            lblClarity2.Text = "<i>To Be Determined</i>";
                        lblExecutive2.Text = oUser.GetFullName(Int32.Parse(oProject.Get(intProject, "executive")));
                        lblWorking2.Text = oUser.GetFullName(Int32.Parse(oProject.Get(intProject, "working")));
                        lblC1.Text = (ds.Tables[0].Rows[0]["c1"].ToString() == "1" ? "Yes" : "No");
                        lblEndLife.Text = (ds.Tables[0].Rows[0]["endlife"].ToString() == "1" ? "Yes" : "No");
                        if (ds.Tables[0].Rows[0]["endlife_date"].ToString() == "")
                            lblEndLifeDate.Text = "<i>Not Applicable</i>";
                        else
                            lblEndLifeDate.Text = DateTime.Parse(ds.Tables[0].Rows[0]["endlife_date"].ToString()).ToShortDateString();
                        lblInitiative.Text = dsRequest.Tables[0].Rows[0]["description"].ToString();
                        lblRequirement.Text = (ds.Tables[0].Rows[0]["req_type"].ToString() == "1" ? "Yes" : "No");
                        if (ds.Tables[0].Rows[0]["req_date"].ToString() == "")
                            lblRequirementDate.Text = "<i>Not Applicable</i>";
                        else
                            lblRequirementDate.Text = DateTime.Parse(ds.Tables[0].Rows[0]["req_date"].ToString()).ToShortDateString();
                        lblInterdependency.Text = ds.Tables[0].Rows[0]["interdependency"].ToString();
                        lblProjects.Text = ds.Tables[0].Rows[0]["projects"].ToString();
                        lblCapability.Text = ds.Tables[0].Rows[0]["capability"].ToString();
                        lblHours.Text = ds.Tables[0].Rows[0]["man_hours"].ToString();
                        lblStart.Text = DateTime.Parse(dsRequest.Tables[0].Rows[0]["start_date"].ToString()).ToShortDateString();
                        lblCompletion.Text = DateTime.Parse(dsRequest.Tables[0].Rows[0]["end_date"].ToString()).ToShortDateString();
                        lblCapital.Text = ds.Tables[0].Rows[0]["expected_capital"].ToString();
                        lblInternal.Text = ds.Tables[0].Rows[0]["internal_labor"].ToString();
                        lblExternal.Text = ds.Tables[0].Rows[0]["external_labor"].ToString();
                        lblMaintenance.Text = ds.Tables[0].Rows[0]["maintenance_increase"].ToString();
                        lblExpenses.Text = ds.Tables[0].Rows[0]["project_expenses"].ToString();
                        lblCostAvoidance.Text = ds.Tables[0].Rows[0]["estimated_avoidance"].ToString();
                        lblSavings.Text = ds.Tables[0].Rows[0]["estimated_savings"].ToString();
                        lblRealized.Text = ds.Tables[0].Rows[0]["realized_savings"].ToString();
                        lblBusinessAvoidance.Text = ds.Tables[0].Rows[0]["business_avoidance"].ToString();
                        lblMaintenanceAvoidance.Text = ds.Tables[0].Rows[0]["maintenance_avoidance"].ToString();
                        lblReusability.Text = ds.Tables[0].Rows[0]["asset_reusability"].ToString();
                        lblInternalImpact.Text = ds.Tables[0].Rows[0]["internal_impact"].ToString();
                        lblExternalImpact.Text = ds.Tables[0].Rows[0]["external_impact"].ToString();
                        lblImpact.Text = ds.Tables[0].Rows[0]["business_impact"].ToString();
                        lblStrategic.Text = ds.Tables[0].Rows[0]["strategic_opportunity"].ToString();
                        lblAcquisition.Text = ds.Tables[0].Rows[0]["acquisition"].ToString();
                        lblCapabilities.Text = ds.Tables[0].Rows[0]["technology_capabilities"].ToString();
                        lblTPM2.Text = (ds.Tables[0].Rows[0]["tpm"].ToString() == "1" ? "Yes" : "No");
                        if (ds.Tables[0].Rows[0]["modified"].ToString() != "")
                        {
                            lblUpdated.Visible = true;
                            DateTime _updated = DateTime.Parse(ds.Tables[0].Rows[0]["modified"].ToString());
                            lblUpdated.Text = "<img src='/images/bigAlert.gif' border='0' align='absmiddle' /> This project request was updated on " + _updated.ToShortDateString() + " at " + _updated.ToShortTimeString();
                        }
                        ds = oProjectRequest.GetPlatforms(intRequest);
                        foreach (DataRow dr in ds.Tables[0].Rows)
                            lblPlatforms.Text += dr["name"].ToString() + "<br/>";
                        dsProject = oProjectRequest.GetResources(intRequest);
                        if (dsProject.Tables[0].Rows.Count > 0)
                        {
                            int intItem = Int32.Parse(dsProject.Tables[0].Rows[0]["itemid"].ToString());
                            int intService = Int32.Parse(dsProject.Tables[0].Rows[0]["serviceid"].ToString());
                            int intAccepted = Int32.Parse(dsProject.Tables[0].Rows[0]["accepted"].ToString());
                            int intManager = Int32.Parse(dsProject.Tables[0].Rows[0]["userid"].ToString());
                            if (intItem > 0)
                            {
                                lblTPMService.Text = oService.GetName(intService);
                                panTPM.Visible = true;
                            }
                            panPM.Visible = true;
                            if (intAccepted == -1)
                                lblPM.Text = "Pending Assignment (" + oUser.GetFullName(intRequestor) + ")";
                            else if (intManager > 0)
                                lblPM.Text = oUser.GetFullName(intManager);
                            else if (intItem > 0)
                                lblPM.Text = "Pending Assignment";
                        }
                        // HISTORY
                        rptHistory.DataSource = oApprove.Get(intRequest);
                        rptHistory.DataBind();
                        int intStep = oApprove.GetStep(intRequest);
                        foreach (RepeaterItem ri in rptHistory.Items)
                        {
                            Label oStep = (Label)ri.FindControl("lblStep");
                            Label oUserId = (Label)ri.FindControl("lblUserId");
                            int intUser = Int32.Parse(oUserId.Text);
                            if (intUser > 0)
                                oUserId.Text = oUser.GetFullName(intUser);
                            else
                                oUserId.Text = "---";
                            Label oImage = (Label)ri.FindControl("lblImage");
                            Label oModified = (Label)ri.FindControl("lblModified");
                            Label oApproval = (Label)ri.FindControl("lblApproval");
                            switch (oApproval.Text)
                            {
                                case "-100":
                                    oApproval.Text = "<span class=\"expedited\">EXPEDITED</span>";
                                    oModified.Text = "Not Available";
                                    oModified.CssClass = "pending";
                                    break;
                                case "-10":
                                    oApproval.Text = "<span class=\"waiting\">Waiting</span>";
                                    break;
                                case "-1":
                                    oApproval.Text = "<span class=\"denied\">Denied</span>";
                                    break;
                                case "0":
                                    oApproval.Text = "<span class=\"pending\">Pending</span>";
                                    if (Int32.Parse(oStep.Text) == intStep)
                                        oImage.Text = "<img src=\"/images/green_right.gif\" border=\"0\" align=\"absmiddle\">&nbsp;";
                                    break;
                                case "1":
                                    oApproval.Text = "<span class=\"approved\">Approved</span>";
                                    break;
                                case "10":
                                    oApproval.Text = "<span class=\"shelved\">Shelved</span>";
                                    break;
                                case "100":
                                    if (oStep.Text == "2")
                                        oApproval.Text = "<span class=\"pending\">Already Approved</span>";
                                    else
                                        oApproval.Text = "<span class=\"pending\">Majority Voted</span>";
                                    oModified.Text = "Not Available";
                                    oModified.CssClass = "pending";
                                    break;
                            }
                            switch (oStep.Text)
                            {
                                case "1":
                                    oStep.Text = "Manager";
                                    break;
                                case "2":
                                    oStep.Text = "Platform";
                                    break;
                                case "3":
                                    oStep.Text = "Board";
                                    break;
                                case "4":
                                    oStep.Text = "Director";
                                    break;
                            }
                        }
                    }
                    else
                        panDenied.Visible = true;
                }
            }
            if (Request.QueryString["div"] != null)
            {
                switch (Request.QueryString["div"])
                {
                    case "D":
                        boolDocuments = true;
                        break;
                    case "C":
                        boolDiscussion = true;
                        break;
                }
            }
            if (boolDetails == false && boolDocuments == false && boolDiscussion == false)
                boolDetails = true;
            btnClose.Attributes.Add("onclick", "return CloseWindow();");
            btnFinish.Attributes.Add("onclick", "return CloseWindow();");
            btnApprove.Attributes.Add("onclick", "return confirm('Are you sure you want to APPROVE this request?');");
            btnDeny.Attributes.Add("onclick", "return confirm('Are you sure you want to DENY this request?');");
            btnShelf.Attributes.Add("onclick", "return confirm('Are you sure you want to SHELF this request?');");
        }
        protected void btnInternal_Click(Object Sender, EventArgs e)
        {
            oProjectRequest.AddComment(Int32.Parse(lblRequest.Text), intProfile, txtInternal.Text, intEnvironment,  intViewRequest);
            Response.Redirect(oPage.GetFullLink(intPage) + "?rid=" + lblRequest.Text + "&div=C");
        }
        protected void btnDelete_Click(Object Sender, EventArgs e)
        {
            LinkButton oDelete = (LinkButton)Sender;
            oProjectRequest.DeleteComment(Int32.Parse(oDelete.CommandArgument));
            Response.Redirect(oPage.GetFullLink(intPage) + "?rid=" + lblRequest.Text + "&div=C");
        }
        protected void chkDescription_Change(Object Sender, EventArgs e)
        {
            if (chkDescription.Checked == true)
                Response.Redirect(oPage.GetFullLink(intPage) + "?rid=" + lblRequest.Text + "&doc=true&div=D");
            else
                Response.Redirect(oPage.GetFullLink(intPage) + "?rid=" + lblRequest.Text + "&div=D");
        }
        protected void btnSubmit_Click(Object Sender, EventArgs e)
        {
            Button oButton = (Button)Sender;
            int intStep = Int32.Parse(lblStep.Text);
            switch (intStep)
            {
                case 1:
                    oApprove.ManagerApproval(Int32.Parse(lblRequest.Text), intProfile, Int32.Parse(oButton.CommandArgument), intWorkflowPage, "", boolDirector);
                    break;
                case 2:
                    oApprove.PlatformApproval(Int32.Parse(lblRequest.Text), intProfile, Int32.Parse(oButton.CommandArgument), intWorkflowPage, "", boolDirector);
                    break;
                case 3:
                    oApprove.BoardApproval(Int32.Parse(lblRequest.Text), intProfile, Int32.Parse(oButton.CommandArgument), intWorkflowPage, intWorkloadPage, "", intResourceRequestPage, boolDirector);
                    break;
                case 4:
                    oApprove.DirectorApproval(Int32.Parse(lblRequest.Text), intProfile, Int32.Parse(oButton.CommandArgument), intWorkflowPage, intWorkloadPage, "", intResourceRequestPage);
                    break;
            }
            Response.Redirect(oPage.GetFullLink(intPage) + "?rid=" + lblRequest.Text + "&action=done");
        }
    }
}