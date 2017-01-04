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
using System.IO;
using System.Net.Mail;
using System.Text;
namespace NCC.ClearView.Presentation.Web
{
    public partial class wm_pc_new : System.Web.UI.UserControl
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
        protected int intViewRequest = Int32.Parse(ConfigurationManager.AppSettings["ViewRequest"]);
        protected int intViewResourceRequest = Int32.Parse(ConfigurationManager.AppSettings["ResourceRequest"]);
        protected int intWorkloadManager = Int32.Parse(ConfigurationManager.AppSettings["ViewWorkload"]);
        protected string strClarity = ConfigurationManager.AppSettings["CLARITY_MAILBOX"];
        protected int intProfile;
        protected Projects oProject;
        protected Functions oFunction;
        protected Users oUser;
        protected Pages oPage;
        protected ResourceRequest oResourceRequest;
        protected RequestItems oRequestItem;
        protected Requests oRequest;
        protected StatusLevels oStatusLevel;
        protected ServiceRequests oServiceRequest;
        protected Applications oApplication;
        protected Documents oDocument;
        protected RequestFields oRequestField;
        protected ProjectRequest oProjectRequest;
        protected Variables oVariable;
        protected TPM oTPM;
        protected Services oService;
        protected Segment oSegment;
        protected Delegates oDelegate;
        protected int intImplementorDistributed = Int32.Parse(ConfigurationManager.AppSettings["ITEMID_IMPLEMENTOR_DISTRIBUTED"]);
        protected int intImplementorMidrange = Int32.Parse(ConfigurationManager.AppSettings["ITEMID_IMPLEMENTOR_MIDRANGE"]);
        protected int intApplication = 0;
        protected int intPage = 0;
        protected int intProject = 0;
        protected bool boolDetails = false;
        protected bool boolStatus = false;
        protected bool boolMilestones = false;
        protected bool boolMyDocuments = false;
        protected bool boolDocuments = false;
        protected bool boolResource = false;
        protected int intRequest = 0;
        protected int intItem = 0;
        protected int intNumber = 0;
        protected int intLead = 0;
        protected int intWorking = 0;
        protected int intExecutive = 0;
        protected bool boolNotifyChange = false;
        protected string strForm = "C:\\inetpub\\wwwroot\\clearview\\help\\clarity.xls";
        protected string strClarityBody = "";
        private StreamWriter oClarityFile;
        protected string strTabs = "";
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
            oStatusLevel = new StatusLevels();
            oServiceRequest = new ServiceRequests(intProfile, dsn);
            oApplication = new Applications(intProfile, dsn);
            oDocument = new Documents(intProfile, dsn);
            oRequestField = new RequestFields(intProfile, dsn);
            oProjectRequest = new ProjectRequest(intProfile, dsn);
            oVariable = new Variables(intEnvironment);
            oTPM = new TPM(intProfile, dsn, intEnvironment);
            oService = new Services(intProfile, dsn);
            oSegment = new Segment(intProfile, dsn);
            oDelegate = new Delegates(intProfile, dsn);
            if (Request.QueryString["applicationid"] != null && Request.QueryString["applicationid"] != "")
                intApplication = Int32.Parse(Request.QueryString["applicationid"]);
            if (Request.QueryString["pageid"] != null && Request.QueryString["pageid"] != "")
                intPage = Int32.Parse(Request.QueryString["pageid"]);
            if (Request.Cookies["application"] != null && Request.Cookies["application"].Value != "")
                intApplication = Int32.Parse(Request.Cookies["application"].Value);
            int intResourceWorkflow = 0;
            int intResourceParent = 0;
            int intUser = 0;
            int intSolo = 0;
            if (Request.QueryString["rrid"] != null && Request.QueryString["rrid"] != "")
            {
                intResourceWorkflow = Int32.Parse(Request.QueryString["rrid"]);
                intResourceParent = oResourceRequest.GetWorkflowParent(intResourceWorkflow);
                ds = oResourceRequest.Get(intResourceParent);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    intRequest = Int32.Parse(ds.Tables[0].Rows[0]["requestid"].ToString());
                    intProject = oRequest.GetProjectNumber(intRequest);
                    intItem = Int32.Parse(ds.Tables[0].Rows[0]["itemid"].ToString());
                    intNumber = Int32.Parse(ds.Tables[0].Rows[0]["number"].ToString());
                    intUser = Int32.Parse(ds.Tables[0].Rows[0]["userid"].ToString());
                    intSolo = Int32.Parse(ds.Tables[0].Rows[0]["solo"].ToString());
                    intLead = oUser.GetManager(intProfile, true);
                }
                btnClose.Visible = true;
                oFunction.ConfigureToolButton(btnClose, "/images/tool_close");
                btnClose.Attributes.Add("onclick", "return ExitWindow();");
            }
            if (Request.QueryString["pid"] != null && Request.QueryString["pid"] != "")
            {
                intProject = Int32.Parse(Request.QueryString["pid"]);
                ds = oProject.GetCoordinator(intProject, 0);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (Int32.Parse(dr["userid"].ToString()) == intProfile || oDelegate.Get(Int32.Parse(dr["userid"].ToString()), intProfile) > 0)
                    {
                        intResourceWorkflow = Int32.Parse(dr["id"].ToString());
                        intResourceParent = oResourceRequest.GetWorkflowParent(intResourceWorkflow);
                        intRequest = Int32.Parse(dr["requestid"].ToString());
                        intItem = Int32.Parse(dr["itemid"].ToString());
                        intNumber = Int32.Parse(dr["number"].ToString());
                        intUser = Int32.Parse(dr["userid"].ToString());
                        intSolo = Int32.Parse(dr["solo"].ToString());
                        intLead = oUser.GetManager(intProfile, true);
                    }
                }
            }
            if (intResourceWorkflow > 0)
            {
                lblResourceWorkflow.Text = intResourceWorkflow.ToString();
                intProject = oRequest.GetProjectNumber(intRequest);
                // Documents
                btnDocuments.Attributes.Add("onclick", "return OpenWindow('DOCUMENTS_SECURE','?pid=" + intProject.ToString() + "');");
                chkMyDescription.Checked = (Request.QueryString["mydoc"] != null);
                lblMyDocuments.Text = oDocument.GetDocuments_Mine(intProject, intProfile, oVariable.DocumentsFolder(), -1, (Request.QueryString["mydoc"] != null));
                // GetDocuments(string _physical, int _projectid, int _requestid, int _userid, int _security, bool _show_description, bool _mine)
                //lblMyDocuments.Text = oDocument.GetDocuments(Request.PhysicalApplicationPath, intProject, 0, intProfile, -1, (Request.QueryString["mydoc"] != null), true);
                chkDescription.Checked = (Request.QueryString["doc"] != null);
                lblDocuments.Text = oDocument.GetDocuments_Project(intProject, intProfile, oVariable.DocumentsFolder(), 1, (Request.QueryString["doc"] != null));
                // GetDocuments(string _physical, int _projectid, int _requestid, int _userid, int _security, bool _show_description, bool _mine)
                //lblDocuments.Text = oDocument.GetDocuments(Request.PhysicalApplicationPath, intProject, 0, intProfile, 1, (Request.QueryString["doc"] != null), false);
                intWorking = Int32.Parse(oProject.Get(intProject, "working"));
                intExecutive = Int32.Parse(oProject.Get(intProject, "executive"));
                if (intSolo.ToString() == "0")
                {
                    btnView.Text = "View Original Project Request";
                    btnView.Attributes.Add("onclick", "return OpenWindow('PRINTER_FRIENDLY','?page=" + intViewRequest.ToString() + "&rid=" + intRequest.ToString() + "');");
                }
                else
                {
                    btnView.Text = "View Original Request Details";
                    btnView.Attributes.Add("onclick", "return OpenWindow('PRINTER_FRIENDLY','?page=" + intViewResourceRequest.ToString() + "&rrid=" + intResourceParent.ToString() + "');");
                }
                txtWorking.Text = oUser.GetFullName(intWorking) + " (" + oUser.GetName(intWorking) + ")";
                txtExecutive.Text = oUser.GetFullName(intExecutive) + " (" + oUser.GetName(intExecutive) + ")";
                oFunction.ConfigureToolButton(btnComplete, "/images/tool_complete");
                btnComplete.Attributes.Add("onclick", "return confirm('Are you sure you want to mark this as completed and remove it from your workload?');");
                oFunction.ConfigureToolButton(btnSave, "/images/tool_save");
                oFunction.ConfigureToolButton(btnPrint, "/images/tool_print");
                btnPrint.Attributes.Add("onclick", "return PrintWindow();");
                oFunction.ConfigureToolButton(btnHold, "/images/tool_hold");
                btnHold.Attributes.Add("onclick", "return confirm('NOTE: By cancelling this project, all open requests will automatically be put on hold and the technicians will be notified.\\n\\nAre you sure you want to put this project on HOLD?');");
                oFunction.ConfigureToolButton(btnCancel, "/images/tool_cancel");
                btnCancel.Attributes.Add("onclick", "return confirm('NOTE: By cancelling this project, all open requests will automatically be cancelled and the technicians will be notified.\\n\\nAre you sure you want to CANCEL this project?');");
                oFunction.ConfigureToolButton(btnPCRCSRC, "/images/tool_pcr_csrc");
                btnPCRCSRC.Attributes.Add("onclick", "return OpenWindow('PCR_CSRC','?pid=" + intProject.ToString() + "');");

                if (Request.QueryString["form"] != null && Request.QueryString["form"] != "")
                    Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "formsent", "<script type=\"text/javascript\">alert('Your clarity form was generated and sent to clarity processing team\\n\\nNOTE: You can view this form by clicking on the PROJECT DOCUMENTS tab.');<" + "/" + "script>");
                if (Request.QueryString["status"] != null && Request.QueryString["status"] != "")
                    Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "statussaved", "<script type=\"text/javascript\">alert('Status updates saved successfully');<" + "/" + "script>");
                if (Request.QueryString["milestone"] != null && Request.QueryString["milestone"] != "")
                    Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "milestonesaved", "<script type=\"text/javascript\">alert('Milestone Saved Successfully');<" + "/" + "script>");
                if (Request.QueryString["save"] != null && Request.QueryString["save"] != "")
                    Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "saved", "<script type=\"text/javascript\">alert('Information Saved Successfully');<" + "/" + "script>");
                if (Request.QueryString["hold"] != null && Request.QueryString["hold"] != "")
                    Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "held", "<script type=\"text/javascript\">alert('The project has been put on hold');<" + "/" + "script>");
                if (Request.QueryString["cancel"] != null && Request.QueryString["cancel"] != "")
                    Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "cancelled", "<script type=\"text/javascript\">alert('The project has been cancelled');<" + "/" + "script>");
                panWorkload.Visible = true;
                ds = oResourceRequest.GetWorkflowProject(intProject);
                int intOldUser = 0;
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (intOldUser == Int32.Parse(dr["userid"].ToString()))
                        dr.Delete();
                    else
                        intOldUser = Int32.Parse(dr["userid"].ToString());
                }
                ddlResource.DataValueField = "userid";
                ddlResource.DataTextField = "userid";
                ddlResource.DataSource = ds;
                ddlResource.DataBind();
                foreach (ListItem oItem in ddlResource.Items)
                    oItem.Text = oUser.GetFullName(Int32.Parse(oItem.Value));
                ddlResource.Items.Insert(0, new ListItem("-- SELECT --", "0"));
                // Load Involvement
                DataSet dsInvolvement = oResourceRequest.GetWorkflowProject(intProject);
                int intOldItem = 0;
                intOldUser = 0;
                foreach (DataRow dr in dsInvolvement.Tables[0].Rows)
                {
                    if (intImplementorDistributed == Int32.Parse(dr["itemid"].ToString()))
                        dr.Delete();
                    else if (intImplementorMidrange == Int32.Parse(dr["itemid"].ToString()))
                        dr.Delete();
                    else if (intOldItem == Int32.Parse(dr["itemid"].ToString()) && intOldUser == Int32.Parse(dr["userid"].ToString()))
                        dr.Delete();
                    else
                    {
                        intOldItem = Int32.Parse(dr["itemid"].ToString());
                        intOldUser = Int32.Parse(dr["userid"].ToString());
                    }
                }
                rptInvolvement.DataSource = dsInvolvement;
                rptInvolvement.DataBind();
                lblNoInvolvement.Visible = (rptInvolvement.Items.Count == 0);
                foreach (RepeaterItem ri in rptInvolvement.Items)
                {
                    Label _id = (Label)ri.FindControl("lblId");
                    Label _user = (Label)ri.FindControl("lblUser");
                    Label _status = (Label)ri.FindControl("lblStatus");
                    Label _image = (Label)ri.FindControl("lblImage");
                    int intStatus = Int32.Parse(_status.Text);
                    intUser = Int32.Parse(_user.Text);
                    _user.Text = oUser.GetFullName(intUser);
                    Label _item = (Label)ri.FindControl("lblItem");
                    int intItem2 = Int32.Parse(_item.Text);
                    Label _allocated = (Label)ri.FindControl("lblAllocated");
                    Label _used = (Label)ri.FindControl("lblUsed");
                    double dblAllocated = oResourceRequest.GetAllocated(intProject, intUser, intItem2);
                    double dblUsed = oResourceRequest.GetUsed(intProject, intUser, intItem2);
                    Label _percent = (Label)ri.FindControl("lblPercent");
                    if (intItem2 == 0)
                    {
                        _item.Text = "Project Coordinator";
                        dblUsed = oTPM.GetHoursTotal(intRequest, intItem2, 0);
                        dblAllocated = oTPM.GetHoursAllocated(intRequest, intItem2, 0);
                    }
                    else
                    {
                        int intApp = oRequestItem.GetItemApplication(intItem2);
                        _item.Text = oApplication.GetName(intApp);
                    }

                    _allocated.Text = dblAllocated.ToString();
                    _used.Text = dblUsed.ToString();
                    if (dblAllocated > 0)
                    {
                        dblUsed = dblUsed / dblAllocated;
                        _percent.Text = dblUsed.ToString("P");
                    }
                    else
                        _percent.Text = dblAllocated.ToString("P");
                    _status.Text = oStatusLevel.Name(intStatus);
                }
                // MY Involvement
                DataSet dsMine = oResourceRequest.GetWorkflowProject(intProject);
                foreach (DataRow dr in dsMine.Tables[0].Rows)
                {
                    if (intProfile != Int32.Parse(dr["userid"].ToString()) || Int32.Parse(dr["itemid"].ToString()) == 0 || oApplication.Get(oRequestItem.GetItemApplication(Int32.Parse(dr["itemid"].ToString())), "tpm") == "1")
                        dr.Delete();
                }
                rptMine.DataSource = dsMine;
                rptMine.DataBind();
                lblNoMine.Visible = (rptMine.Items.Count == 0);
                foreach (RepeaterItem ri in rptMine.Items)
                {
                    Label _id = (Label)ri.FindControl("lblId");
                    Label _user = (Label)ri.FindControl("lblUser");
                    Label _status = (Label)ri.FindControl("lblStatus");
                    Label _service = (Label)ri.FindControl("lblServiceId");
                    int _serviceid = Int32.Parse(_service.Text);
                    Label _name = (Label)ri.FindControl("lblName");
                    int intStatus = Int32.Parse(_status.Text);
                    intUser = Int32.Parse(_user.Text);
                    if (intUser != intProfile || intStatus < 1 || intStatus > 2)
                        ri.Visible = false;
                    else
                    {
                        if (_name.Text == "")
                            _name.Text = oService.GetName(_serviceid);
                        Label _item = (Label)ri.FindControl("lblItem");
                        Label _allocated = (Label)ri.FindControl("lblAllocated");
                        double dblAllocated = double.Parse(_allocated.Text);
                        Label _used = (Label)ri.FindControl("lblUsed");
                        double dblUsed = oResourceRequest.GetWorkflowUsed(Int32.Parse(_id.Text));
                        Label _percent = (Label)ri.FindControl("lblPercent");
                        int intItem2 = Int32.Parse(_item.Text);
                        if (intItem2 == 0)
                        {
                            _item.Text = "Project Coordinator";
                            dblUsed = oTPM.GetHoursTotal(intRequest, intItem2, 0);
                            dblAllocated = oTPM.GetHoursAllocated(intRequest, intItem2, 0);
                        }
                        else
                        {
                            int intApp = oRequestItem.GetItemApplication(intItem2);
                            _item.Text = oApplication.GetName(intApp);
                        }
                        _allocated.Text = dblAllocated.ToString();
                        _used.Text = dblUsed.ToString();
                        if (dblAllocated > 0)
                        {
                            dblUsed = dblUsed / dblAllocated;
                            _percent.Text = dblUsed.ToString("P");
                        }
                        else
                            _percent.Text = dblAllocated.ToString("P");
                        _status.Text = oStatusLevel.Name(intStatus);
                    }
                }
                // Milestones
                rptMilestones.DataSource = oResourceRequest.GetMilestones(intResourceWorkflow);
                rptMilestones.DataBind();
                lblNoMilestone.Visible = (rptMilestones.Items.Count == 0);
                LoadStatus(intResourceWorkflow);
                if (!IsPostBack)
                {
                    LoadLists();
                    LoadInformation(intResourceWorkflow);
                    lblPriority.Text = oProjectRequest.GetPriority(intRequest);
                    lblProjectStatus.Text = oStatusLevel.HTML(Int32.Parse(oProject.Get(intProject, "status")));
                }
                LoadTabs();
                ddlPortfolio.Attributes.Add("onchange", "PopulateSegments('" + ddlPortfolio.ClientID + "','" + ddlSegment.ClientID + "');");
                ddlSegment.Attributes.Add("onchange", "UpdateDropDownHidden('" + ddlSegment.ClientID + "','" + hdnSegment.ClientID + "');");
                btnSave.Attributes.Add("onclick", "return ValidateStatusPC('" + hdnType.ClientID + "','" + ddlVariance.ClientID + "','" + txtStatusDate.ClientID + "','" + txtComments.ClientID + "','" + txtThisWeek.ClientID + "','" + txtNextWeek.ClientID + "') && ValidateMilestone('" + hdnType.ClientID + "','" + txtMilestoneApproved.ClientID + "','" + txtMilestoneForecasted.ClientID + "','" + txtMilestone.ClientID + "','" + txtDetail.ClientID + "');");
                btnMessage.Attributes.Add("onclick", "return ValidateDropDown('" + ddlResource.ClientID + "','Please make a selection for the recipient of this communication')" +
                    " && ValidateDropDown('" + ddlCommunication.ClientID + "','Please make a selection for the form of communication')" +
                    " && ValidateText('" + txtMessage.ClientID + "','Please enter some text for the communication')" +
                    ";");
                btnCostAdd.Attributes.Add("onclick", "return MoveList('" + lstCostsAvailable.ClientID + "','" + lstCostsCurrent.ClientID + "','" + hdnCosts.ClientID + "','" + lstCostsCurrent.ClientID + "');");
                lstCostsAvailable.Attributes.Add("ondblclick", "return MoveList('" + lstCostsAvailable.ClientID + "','" + lstCostsCurrent.ClientID + "','" + hdnCosts.ClientID + "','" + lstCostsCurrent.ClientID + "');");
                btnCostRemove.Attributes.Add("onclick", "return MoveList('" + lstCostsCurrent.ClientID + "','" + lstCostsAvailable.ClientID + "','" + hdnCosts.ClientID + "','" + lstCostsCurrent.ClientID + "');");
                lstCostsCurrent.Attributes.Add("ondblclick", "return MoveList('" + lstCostsCurrent.ClientID + "','" + lstCostsAvailable.ClientID + "','" + hdnCosts.ClientID + "','" + lstCostsCurrent.ClientID + "');");
                imgStatusDate.Attributes.Add("onclick", "return ShowCalendar('" + txtStatusDate.ClientID + "');");
                imgMilestoneApproved.Attributes.Add("onclick", "return ShowCalendar('" + txtMilestoneApproved.ClientID + "');");
                imgMilestoneForecasted.Attributes.Add("onclick", "return ShowCalendar('" + txtMilestoneForecasted.ClientID + "');");
                txtWorking.Attributes.Add("onkeyup", "return AJAXTextBoxGet(this,'250','195','" + divWorking.ClientID + "','" + lstWorking.ClientID + "','" + hdnWorking.ClientID + "','" + oVariable.URL() + "/frame/users.aspx',2);");
                lstWorking.Attributes.Add("ondblclick", "AJAXClickRow();");
                txtExecutive.Attributes.Add("onkeyup", "return AJAXTextBoxGet(this,'250','195','" + divExecutive.ClientID + "','" + lstExecutive.ClientID + "','" + hdnExecutive.ClientID + "','" + oVariable.URL() + "/frame/users.aspx',2);");
                lstExecutive.Attributes.Add("ondblclick", "AJAXClickRow();");
                imgStartA.Attributes.Add("onclick", "return ShowCalendar('" + txtStartA.ClientID + "');");
                imgEndA.Attributes.Add("onclick", "return ShowCalendar('" + txtEndA.ClientID + "');");
                imgActualE.Attributes.Add("onclick", "return ShowCalculator('" + txtActualE.ClientID + "');");
                imgActualI.Attributes.Add("onclick", "return ShowCalculator('" + txtActualI.ClientID + "');");
                imgActualO.Attributes.Add("onclick", "return ShowCalculator('" + txtActualO.ClientID + "');");
            }
            else
                panDenied.Visible = true;
            btnDenied.Attributes.Add("onclick", "return CloseWindow();");
        }
        private void LoadLists()
        {
            Costs oCost = new Costs(intProfile, dsn);
            ds = oCost.Gets(1);
            lstCostsAvailable.DataValueField = "costid";
            lstCostsAvailable.DataTextField = "name";
            lstCostsAvailable.DataSource = ds;
            lstCostsAvailable.DataBind();
            Organizations oOrganization = new Organizations(intProfile, dsn);
            ds = oOrganization.Gets(1);
            ddlPortfolio.DataValueField = "organizationid";
            ddlPortfolio.DataTextField = "name";
            ddlPortfolio.DataSource = ds;
            ddlPortfolio.DataBind();
            ddlPortfolio.Items.Insert(0, new ListItem("-- SELECT --", "0"));
        }
        private void LoadStatus(int _resourceid)
        {
            DataSet dsStatus = oResourceRequest.GetStatussTPM(_resourceid);
            rptStatus.DataSource = dsStatus;
            rptStatus.DataBind();
            lblNoStatus.Visible = (rptStatus.Items.Count == 0);
            double dblTotalScope = 0.00;
            double dblTotalTimeline = 0.00;
            double dblTotalBudget = 0.00;
            foreach (RepeaterItem ri in rptStatus.Items)
            {
                Label _scope = (Label)ri.FindControl("lblScope");
                //Label _timeline = (Label)ri.FindControl("lblTimeline");
                //Label _budget = (Label)ri.FindControl("lblBudget");
                Label _status = (Label)ri.FindControl("lblStatus");
                //Label _status_scope = (Label)ri.FindControl("lblStatusScope");
                //Label _status_timeline = (Label)ri.FindControl("lblStatusTimeline");
                //Label _status_budget = (Label)ri.FindControl("lblStatusBudget");
                double dblScope = double.Parse(_scope.Text);
                double dblTimeline = 3.00;
                double dblBudget = 3.00;
                //double dblTimeline = double.Parse(_timeline.Text);
                //double dblBudget = double.Parse(_budget.Text);
                //_status_scope.Text = oResourceRequest.GetStatus(dblScope, 60, 10);
                //_status_timeline.Text = oResourceRequest.GetStatus(dblTimeline, 60, 10);
                //_status_budget.Text = oResourceRequest.GetStatus(dblBudget, 60, 10);
                _status.Text = oResourceRequest.GetStatus(dblScope, dblTimeline, dblBudget, 83, 15);
                if (dblTotalScope == 0.00)
                {
                    dblTotalScope = dblScope;
                    dblTotalTimeline = dblTimeline;
                    dblTotalBudget = dblBudget;
                }
            }
            lblStatus.Text = oResourceRequest.GetStatus(dblTotalScope, dblTotalTimeline, dblTotalBudget, 83, 15);
        }
        private void LoadInformation(int _request)
        {
            Organizations oOrganization = new Organizations(intProfile, dsn);
            DataSet dsProject = oProject.Get(intProject);
            txtName.Text = oProject.Get(intProject, "name");
            txtCustom.Text = oResourceRequest.Get(_request, "name");
            lblNumber.Text = oProject.Get(intProject, "number");
            ddlBaseDisc.SelectedValue = oProject.Get(intProject, "bd");
            int intPortfolio = Int32.Parse(oProject.Get(intProject, "organization"));
            ddlPortfolio.SelectedValue = intPortfolio.ToString();
            if (intPortfolio > 0)
            {
                int intSegment = Int32.Parse(oProject.Get(intProject, "segmentid"));
                hdnSegment.Value = intSegment.ToString();
                ddlSegment.Enabled = true;
                ddlSegment.DataTextField = "name";
                ddlSegment.DataValueField = "id";
                ddlSegment.DataSource = oSegment.Gets(intPortfolio, 1);
                ddlSegment.DataBind();
                ddlSegment.Items.Insert(0, new ListItem("-- SELECT --", "0"));
                ddlSegment.SelectedValue = intSegment.ToString();
            }
            txtDescription.Text = oRequest.Get(intRequest, "description");
            //        txtDescription.Attributes.Add("onkeydown", "return DescriptionDown(this);");
            //        txtDescription.Attributes.Add("onkeyup", "return DescriptionUp(this);");
            DataSet dsPR = oProjectRequest.Get(intRequest);
            if (dsPR.Tables[0].Rows.Count > 0)
            {
                if (oRequest.Get(intRequest, "start_date") != "")
                    lblStartE.Text = DateTime.Parse(oRequest.Get(intRequest, "start_date")).ToShortDateString();
                else
                    lblStartE.Text = "N / A";
                if (oRequest.Get(intRequest, "end_date") != "")
                    lblEndE.Text = DateTime.Parse(oRequest.Get(intRequest, "end_date")).ToShortDateString();
                else
                    lblEndE.Text = "N / A";
                lblEstimateI.Text = dsPR.Tables[0].Rows[0]["internal_labor"].ToString();
                lblEstimateE.Text = dsPR.Tables[0].Rows[0]["external_labor"].ToString();
                lblEstimateO.Text = dsPR.Tables[0].Rows[0]["expected_capital"].ToString();
            }
            // Project Schedule
            DataSet dsRequest = oTPM.Get(intRequest, intItem, intNumber);
            if (dsRequest.Tables[0].Rows.Count == 0)
            {
                oTPM.Add(intRequest, intItem, intNumber, intProfile);
                dsRequest = oTPM.Get(intRequest, intItem, intNumber);
            }
            if (dsRequest.Tables[0].Rows[0]["start_e"].ToString() != "")
                txtStartA.Text = DateTime.Parse(dsRequest.Tables[0].Rows[0]["start_e"].ToString()).ToShortDateString();
            lblStartDate.Text = txtStartA.Text;
            if (lblStartDate.Text == "")
                lblStartDate.Text = "<i>Pending</i>";
            if (dsRequest.Tables[0].Rows[0]["end_e"].ToString() != "")
                txtEndA.Text = DateTime.Parse(dsRequest.Tables[0].Rows[0]["end_e"].ToString()).ToShortDateString();
            lblEndDate.Text = txtEndA.Text;
            if (lblEndDate.Text == "")
                lblEndDate.Text = "<i>Pending</i>";
            double dblActIE = 0.00;
            if (dsRequest.Tables[0].Rows[0]["actie"].ToString() != "")
                dblActIE = double.Parse(dsRequest.Tables[0].Rows[0]["actie"].ToString());
            txtActualI.Text = dblActIE.ToString("F");
            double dblActEE = 0.00;
            if (dsRequest.Tables[0].Rows[0]["actee"].ToString() != "")
                dblActEE = double.Parse(dsRequest.Tables[0].Rows[0]["actee"].ToString());
            txtActualE.Text = dblActEE.ToString("F");
            double dblActHE = 0.00;
            if (dsRequest.Tables[0].Rows[0]["acthe"].ToString() != "")
                dblActHE = double.Parse(dsRequest.Tables[0].Rows[0]["acthe"].ToString());
            txtActualO.Text = dblActHE.ToString("F");
            double dblActual = dblActIE + dblActEE + dblActHE;
            lblTotalActual.Text = dblActual.ToString("F");
            lblActual.Text = dblActual.ToString("F");
            string strCosts = dsRequest.Tables[0].Rows[0]["costs"].ToString();
            hdnCosts.Value = strCosts;
            string[] strCost;
            char[] strSplit = { '&' };
            strCost = strCosts.Split(strSplit);
            for (int ii = 0; ii < lstCostsAvailable.Items.Count; ii++)
            {
                for (int jj = 0; jj < strCost.Length; jj++)
                {
                    if (strCost[jj].Trim() != "" && strCost[jj] == lstCostsAvailable.Items[ii].Value)
                    {
                        lstCostsCurrent.Items.Add(lstCostsAvailable.Items[ii]);
                        lstCostsAvailable.Items.Remove(lstCostsAvailable.Items[ii]);
                        ii--;
                        break;
                    }
                }
            }
            //if (dsRequest.Tables[0].Rows[0]["project_start_date"].ToString() != "")
            //    txtStartDate.Text = DateTime.Parse(dsRequest.Tables[0].Rows[0]["project_start_date"].ToString()).ToShortDateString();
            //if (lblEndDate.Text == "")
            //    lblEndDate.Text = "<i>Pending</i>";

            // Load Phases according to what is available
            DataSet dsClose = oResourceRequest.GetWorkflowProjectAll(intProject);
            bool boolClose = true;
            foreach (DataRow dr in dsClose.Tables[0].Rows)
            {
                if (Int32.Parse(dr["id"].ToString()) != _request && Int32.Parse(dr["status"].ToString()) < 3 && Int32.Parse(dr["status"].ToString()) > 0 && intImplementorDistributed != Int32.Parse(dr["itemid"].ToString()) && intImplementorMidrange != Int32.Parse(dr["itemid"].ToString()))
                {
                    boolClose = false;
                    break;
                }
            }
            if (boolClose == false)
            {
                btnComplete.ImageUrl = "/images/tool_complete_dbl.gif";
                btnComplete.Enabled = false;
                //            lblComplete.Text = oFunction.CreateBox("box_", "<img src=\"/images/alert.gif\" border=\"0\" align=\"absmiddle\" /> Resources have not completed this project", "error");
                lblComplete.Text = "<table width=\"400\" style=\"border: outset 2px #FFFFFF\" border=\"0\"><tr><td align=\"center\"><img src=\"/images/alert.gif\" border=\"0\" align=\"absmiddle\" /> Resources still have active tasks associated with this project</td></tr></table>";
            }
        }
        private void LoadTabs()
        {
            StringBuilder sb = new StringBuilder(strTabs);

            if (Request.QueryString["div"] != null)
            {
                hdnType.Value = Request.QueryString["div"];
                switch (Request.QueryString["div"])
                {
                    case "E":
                        boolDetails = true;
                        break;
                    case "S":
                        boolStatus = true;
                        break;
                    case "D":
                        boolDocuments = true;
                        break;
                    case "M":
                        boolMyDocuments = true;
                        break;
                    case "R":
                        boolResource = true;
                        break;
                    case "L":
                        boolMilestones = true;
                        break;
                }
            }
            else
            {
                hdnType.Value = "E";
                boolDetails = true;
            }

            if (boolDetails == true)
            {
                sb.Append("<td><img src=\"/images/TabOnLeftCap.gif\"></td><td nowrap background=\"/images/TabOnBackground.gif\"><a href=\"javascript:void(0);\" onclick=\"ChangeTab(this,'divTab7','");
                sb.Append(hdnType.ClientID);
                sb.Append("','E',true);\" class=\"tabheader\">Project Details</a></td><td><img src=\"/images/TabOnRightCap.gif\"></td>");
            }
            else
            {
                sb.Append("<td><img src=\"/images/TabOffLeftCap.gif\"></td><td nowrap background=\"/images/TabOffBackground.gif\"><a href=\"javascript:void(0);\" onclick=\"ChangeTab(this,'divTab7','");
                sb.Append(hdnType.ClientID);
                sb.Append("','E',true);\" class=\"tabheader\">Project Details</a></td><td><img src=\"/images/TabOffRightCap.gif\"></td>");
            }

            if (boolMyDocuments == true)
            {
                sb.Append("<td><img src=\"/images/TabOnLeftCap.gif\"></td><td nowrap background=\"/images/TabOnBackground.gif\"><a href=\"javascript:void(0);\" onclick=\"ChangeTab(this,'divTab3','");
                sb.Append(hdnType.ClientID);
                sb.Append("','M',true);\" class=\"tabheader\">My Documents</a></td><td><img src=\"/images/TabOnRightCap.gif\"></td>");
            }
            else
            {
                sb.Append("<td><img src=\"/images/TabOffLeftCap.gif\"></td><td nowrap background=\"/images/TabOffBackground.gif\"><a href=\"javascript:void(0);\" onclick=\"ChangeTab(this,'divTab3','");
                sb.Append(hdnType.ClientID);
                sb.Append("','M',true);\" class=\"tabheader\">My Documents</a></td><td><img src=\"/images/TabOffRightCap.gif\"></td>");
            }

            if (boolDocuments == true)
            {
                sb.Append("<td><img src=\"/images/TabOnLeftCap.gif\"></td><td nowrap background=\"/images/TabOnBackground.gif\"><a href=\"javascript:void(0);\" onclick=\"ChangeTab(this,'divTab4','");
                sb.Append(hdnType.ClientID);
                sb.Append("','D',true);\" class=\"tabheader\">Project Documents</a></td><td><img src=\"/images/TabOnRightCap.gif\"></td>");
            }
            else
            {
                sb.Append("<td><img src=\"/images/TabOffLeftCap.gif\"></td><td nowrap background=\"/images/TabOffBackground.gif\"><a href=\"javascript:void(0);\" onclick=\"ChangeTab(this,'divTab4','");
                sb.Append(hdnType.ClientID);
                sb.Append("','D',true);\" class=\"tabheader\">Project Documents</a></td><td><img src=\"/images/TabOffRightCap.gif\"></td>");
            }

            if (boolMilestones == true)
            {
                sb.Append("<td><img src=\"/images/TabOnLeftCap.gif\"></td><td nowrap background=\"/images/TabOnBackground.gif\"><a href=\"javascript:void(0);\" onclick=\"ChangeTab(this,'divTab2','");
                sb.Append(hdnType.ClientID);
                sb.Append("','L',true);\" class=\"tabheader\">Milestones</a></td><td><img src=\"/images/TabOnRightCap.gif\"></td>");
            }
            else
            {
                sb.Append("<td><img src=\"/images/TabOffLeftCap.gif\"></td><td nowrap background=\"/images/TabOffBackground.gif\"><a href=\"javascript:void(0);\" onclick=\"ChangeTab(this,'divTab2','");
                sb.Append(hdnType.ClientID);
                sb.Append("','L',true);\" class=\"tabheader\">Milestones</a></td><td><img src=\"/images/TabOffRightCap.gif\"></td>");
            }

            if (boolResource == true)
            {
                sb.Append("<td><img src=\"/images/TabOnLeftCap.gif\"></td><td nowrap background=\"/images/TabOnBackground.gif\"><a href=\"javascript:void(0);\" onclick=\"ChangeTab(this,'divTab1','");
                sb.Append(hdnType.ClientID);
                sb.Append("','R',true);\" class=\"tabheader\">Resource Involvement</a></td><td><img src=\"/images/TabOnRightCap.gif\"></td>");
            }
            else
            {
                sb.Append("<td><img src=\"/images/TabOffLeftCap.gif\"></td><td nowrap background=\"/images/TabOffBackground.gif\"><a href=\"javascript:void(0);\" onclick=\"ChangeTab(this,'divTab1','");
                sb.Append(hdnType.ClientID);
                sb.Append("','R',true);\" class=\"tabheader\">Resource Involvement</a></td><td><img src=\"/images/TabOffRightCap.gif\"></td>");
            }

            if (boolStatus == true)
            {
                sb.Append("<td><img src=\"/images/TabOnLeftCap.gif\"></td><td nowrap background=\"/images/TabOnBackground.gif\"><a href=\"javascript:void(0);\" onclick=\"ChangeTab(this,'divTab5','");
                sb.Append(hdnType.ClientID);
                sb.Append("','S',true);\" class=\"tabheader\">Status Updates</a></td><td><img src=\"/images/TabOnRightCap.gif\"></td>");
            }
            else
            {
                sb.Append("<td><img src=\"/images/TabOffLeftCap.gif\"></td><td nowrap background=\"/images/TabOffBackground.gif\"><a href=\"javascript:void(0);\" onclick=\"ChangeTab(this,'divTab5','");
                sb.Append(hdnType.ClientID);
                sb.Append("','S',true);\" class=\"tabheader\">Status Updates</a></td><td><img src=\"/images/TabOffRightCap.gif\"></td>");
            }

            sb.Append("<td><img src=\"/images/TabOffLeftCap.gif\"></td><td nowrap background=\"/images/TabOffBackground.gif\"><a href=\"javascript:void(0);\" onclick=\"ChangeTab(this,'divTab6','");
            sb.Append(hdnType.ClientID);
            sb.Append("','T',true);\" class=\"tabheader\">Additional Tasks</a></td><td><img src=\"/images/TabOffRightCap.gif\"></td>");

            strTabs = sb.ToString();
        }
        protected void btnSave_Click(Object Sender, EventArgs e)
        {
            int intResourceWorkflow = Int32.Parse(lblResourceWorkflow.Text);
            int intResourceParent = oResourceRequest.GetWorkflowParent(intResourceWorkflow);
           
           
           
            oResourceRequest.UpdateWorkflowName(intResourceWorkflow, txtCustom.Text, intProfile);
            UpdateProject();
            switch (Request.Form[hdnType.UniqueID])
            {
                case "E":
                    oTPM.Execution(intRequest, intItem, intNumber, "", "", "", txtActualI.Text, txtActualE.Text, txtActualO.Text, "", "", "", "", "", txtStartA.Text, txtEndA.Text, "", "", 0);
                    Response.Redirect(oPage.GetFullLink(intPage) + "?pid=" + intProject.ToString() + "&div=E");
                    break;
                case "S":
                    oResourceRequest.AddStatusTPM(intResourceWorkflow, Int32.Parse(ddlVariance.SelectedItem.Value), 0, 0, DateTime.Parse(txtStatusDate.Text), txtComments.Text, txtThisWeek.Text, txtNextWeek.Text);
                    Response.Redirect(oPage.GetFullLink(intPage) + "?pid=" + intProject.ToString() + "&div=S&status=true");
                    break;
                case "L":
                    oResourceRequest.AddMilestone(intResourceWorkflow, DateTime.Parse(txtMilestoneApproved.Text), DateTime.Parse(txtMilestoneForecasted.Text), (chkComplete.Checked ? 1 : 0), txtMilestone.Text, txtDetail.Text);
                    Response.Redirect(oPage.GetFullLink(intPage) + "?pid=" + intProject.ToString() + "&div=L&milestone=true");
                    break;
            }
        }
        private void UpdateProject()
        {
            string strName = oProject.Get(intProject, "name");
            string strBody = "";
            string strTo = "";
            if (strName != txtName.Text)
                strBody += "<li>The project name was changed<br/><br/></li>";
            int intPortfolio = Int32.Parse(oProject.Get(intProject, "organization"));
            int intNewPortfolio = Int32.Parse(ddlPortfolio.SelectedItem.Value);
            if (intPortfolio != intNewPortfolio)
                strBody += "<li>The sponsoring portfolio was changed<br/><br/></li>";
            int intSegment = Int32.Parse(oProject.Get(intProject, "segmentid"));
            int intNewSegment = 0;
            if (Request.Form[hdnSegment.UniqueID] != "")
                intNewSegment = Int32.Parse(Request.Form[hdnSegment.UniqueID]);
            if (intSegment != intNewSegment)
                strBody += "<li>The segment was changed<br/><br/></li>";
            int intStatus = Int32.Parse(oProject.Get(intProject, "status"));
            //            int intNewStatus = Int32.Parse(ddlStatus.SelectedItem.Value);
            //          if (intStatus != intNewStatus)
            //            strBody += "<li>The project status was changed to " + ddlStatus.SelectedItem.Text.ToUpper() + "<br/><br/></li>";
            int intNewStatus = 2;
            int intUser = Int32.Parse(oProject.Get(intProject, "userid"));
            oProject.Update(intProject, txtName.Text, ddlBaseDisc.SelectedItem.Value, lblNumber.Text, intUser, intNewPortfolio, intNewSegment, intNewStatus);
            string strDescription = oRequest.Get(intRequest, "description");
            if (strDescription != txtDescription.Text)
            {
                oRequest.UpdateDescription(intRequest, txtDescription.Text);
                strBody += "<li>The project description was changed<br/><br/></li>";
            }
            if (Request.Form[hdnWorking.UniqueID] != "")
            {
                int intNewWorking = Int32.Parse(Request.Form[hdnWorking.UniqueID]);
                if (intWorking != intNewWorking)
                {
                    strBody += "<li>The working sponsor was changed from " + oUser.GetFullName(intWorking) + " to " + oUser.GetFullName(intNewWorking) + "<br/><br/></li>";
                    strTo = oUser.GetName(intWorking) + ";" + oUser.GetName(intNewWorking) + ";";
                    intWorking = intNewWorking;
                }
                else
                    strTo = oUser.GetName(intWorking) + ";";
            }
            else
                strTo = oUser.GetName(intWorking) + ";";
            if (Request.Form[hdnExecutive.UniqueID] != "")
            {
                int intNewExecutive = Int32.Parse(Request.Form[hdnExecutive.UniqueID]);
                if (intExecutive != intNewExecutive)
                {
                    strBody += "<li>The executive sponsor was changed from " + oUser.GetFullName(intExecutive) + " to " + oUser.GetFullName(intNewExecutive) + "<br/><br/></li>";
                    strTo += oUser.GetName(intExecutive) + ";" + oUser.GetName(intNewExecutive) + ";";
                    intExecutive = intNewExecutive;
                }
                else
                    strTo += oUser.GetName(intExecutive) + ";";
            }
            else
                strTo += oUser.GetName(intExecutive) + ";";
            oProject.Update(intProject, 0, intExecutive, intWorking, 0, 0, 0);
            if (strBody != "" && boolNotifyChange == true)
                Notify(strTo, strBody, (intStatus != intNewStatus));
        }
        protected void chkMyDescription_Change(Object Sender, EventArgs e)
        {
            if (chkMyDescription.Checked == true)
                Response.Redirect(oPage.GetFullLink(intPage) + "?pid=" + intProject.ToString() + "&mydoc=true&div=M");
            else
                Response.Redirect(oPage.GetFullLink(intPage) + "?pid=" + intProject.ToString() + "&div=M");
        }
        protected void chkDescription_Change(Object Sender, EventArgs e)
        {
            if (chkDescription.Checked == true)
                Response.Redirect(oPage.GetFullLink(intPage) + "?pid=" + intProject.ToString() + "&doc=true&div=D");
            else
                Response.Redirect(oPage.GetFullLink(intPage) + "?pid=" + intProject.ToString() + "&div=D");
        }
        protected void btnComplete_Click(Object Sender, EventArgs e)
        {
            CloseProject(true);
            Response.Redirect(oPage.GetFullLink(intPage) + "?pid=" + intProject.ToString() + "&action=finish");
        }
        protected void btnHold_Click(Object Sender, EventArgs e)
        {
            UpdateRequest(5);
            Response.Redirect(oPage.GetFullLink(intPage) + "?pid=" + intProject.ToString() + "&hold=true");
        }
        protected void btnCancel_Click(Object Sender, EventArgs e)
        {
            UpdateRequest(-2);
            Response.Redirect(oPage.GetFullLink(intPage) + "?pid=" + intProject.ToString() + "&cancel=true");
        }
        private void UpdateRequest(int _status)
        {
            string strStatus = "PUT ON HOLD";
            if (_status == -2)
                strStatus = "CANCELLED";
            ds = oProjectRequest.Get(intRequest);
            DataSet dsRequest = oRequest.Get(intRequest);
            int intProject = Int32.Parse(dsRequest.Tables[0].Rows[0]["projectid"].ToString());
            DataSet dsProject = oProject.Get(intProject);
            bool boolApproved = oProject.IsApproved(intProject);
            string strTo = "";
            string strUser = oUser.GetName(intProfile);
            if (strUser != "")
                strTo += strUser + ";";
            string strExecutive = oUser.GetName(Int32.Parse(dsProject.Tables[0].Rows[0]["executive"].ToString()));
            if (strExecutive != "")
                strTo += strExecutive + ";";
            string strWorking = oUser.GetName(Int32.Parse(dsProject.Tables[0].Rows[0]["working"].ToString()));
            if (strWorking != "")
                strTo += strWorking + ";";
            ds = oResourceRequest.GetWorkflowProjectAll(intProject);
            string strCC = "";
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (Int32.Parse(dr["userid"].ToString()) > 0 && Int32.Parse(dr["accepted"].ToString()) > -1 && Int32.Parse(dr["status"].ToString()) > 0 && Int32.Parse(dr["status"].ToString()) < 3)
                {
                    strUser = oUser.GetName(Int32.Parse(dr["userid"].ToString()));
                    if (strUser != "")
                        strCC += strUser + ";";
                    oResourceRequest.UpdateWorkflowStatus(Int32.Parse(dr["id"].ToString()), _status, true);
                }
            }
            oProject.Update(intProject, _status);
            // NOTIFICATION
            //oFunction.SendEmail("Project Status Update", strTo, strCC, strBCC, "Project Status Update", "<p><b>The following project has been " + strStatus + " by " + oUser.GetFullName(intProfile) + "...</b></p><p>" + oProject.GetBody(intProject, intEnvironment, true) + "</p>", true, false);
            strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_PROJECT");

            oFunction.SendEmail("Project Status Update", "", "", strEMailIdsBCC, "Project Status Update", "<p>TO: " + strTo + "<br/>CC: " + strCC + "<b>The following project has been " + strStatus + " by " + oUser.GetFullName(intProfile) + "...</b></p><p>" + oProject.GetBody(intProject, intEnvironment, true) + "</p>", true, false);
        }
        private void CloseProject(bool _done)
        {
            int intResourceWorkflow = Int32.Parse(lblResourceWorkflow.Text);
            int intResourceParent = oResourceRequest.GetWorkflowParent(intResourceWorkflow);
            if (_done == true)
            {
                oResourceRequest.UpdateWorkflowStatus(intResourceWorkflow, 3, true);
                oResourceRequest.CloseWorkflow(intResourceWorkflow, intEnvironment, 0, dsnServiceEditor, true, intResourceRequestApprove, intAssignPage, intViewPage, dsnAsset, dsnIP);
                Response.Redirect(oPage.GetFullLink(intPage) + "?action=finish");
            }
        }
        protected void btnMessage_Click(Object Sender, EventArgs e)
        {
            int intUser = Int32.Parse(ddlResource.SelectedItem.Value);
            Functions oFunction = new Functions(intProfile, dsn, intEnvironment);
            strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_PROJECT");
            if (ddlCommunication.SelectedItem.Value.ToUpper() == "EMAIL")
            {
                string strEmail = oUser.GetName(intUser);
                oFunction.SendEmail("", strEmail, oUser.GetName(intProfile), strEMailIdsBCC, "Communication from " + oUser.GetFullName(intProfile), oProject.GetBody(intProject, intEnvironment, false) + "<table width=\"100%\" border=\"0\" cellpadding=\"2\" cellspacing=\"1\"><tr><td><span style=\"width:100%;border-bottom:1 dotted #999999;\"/></td></tr></table>" + txtMessage.Text, true, false);
            }
            else
            {
                string strPager = oUser.Get(intUser, "pager") + "@archwireless.net";
                oFunction.SendEmail("", strPager, oUser.GetName(intProfile), strEMailIdsBCC, "Communication from " + oUser.GetFullName(intProfile), txtMessage.Text, false, true);
            }
            Response.Redirect(oPage.GetFullLink(intPage) + "?pid=" + intProject.ToString() + "&comm=sent&div=R");
        }
        private void Notify(string _to, string _text, bool _resources)
        {
            string strCC = "";
            strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_PROJECT");
            if (_resources == true)
            {
                DataSet dsNotify = oResourceRequest.GetWorkflowProjectAll(intProject);
                foreach (DataRow dr in dsNotify.Tables[0].Rows)
                    strCC += oUser.GetName(Int32.Parse(dr["userid"].ToString())) + ";";
            }
            string strEmail = oUser.GetName(Int32.Parse(oProject.Get(intProject, "userid"))) + ";";
            strEmail += oUser.GetName(oRequest.GetUser(intRequest)) + ";";
            // NOTIFICATION
            //oFunction.SendEmail("Project Details Update", strEmail, strCC + _to, strBCC, "Project Details Update", "<p><b>This message is to notify you that the following project has been modified...</b></p><p>" + oProject.GetBody(intProject, intEnvironment, true) + "</p><p><br/><span style=\"color:#0000FF\"><b>Description of Change(s):</b></span></p><p><ul>" + _text + "</ul></p>", true, false);
            oFunction.SendEmail("Project Details Update", "", "", strEMailIdsBCC, "Project Details Update", "<p>TO: " + strEmail + "<br/>CC: " + strCC + _to + "<b>This message is to notify you that the following project has been modified...</b></p><p>" + oProject.GetBody(intProject, intEnvironment, true) + "</p><p><br/><span style=\"color:#0000FF\"><b>Description of Change(s):</b></span></p><p><ul>" + _text + "</ul></p>", true, false);
        }
        protected void btnGenerate_Click(Object Sender, EventArgs e)
        {
            oVariable = new Variables(intEnvironment);
            strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_PROJECT");
            string strAttachment = "";
            string strPath = oVariable.UploadsFolder();
            DateTime _now = DateTime.Now;
            if (Directory.Exists(strPath) == false)
                Directory.CreateDirectory(strPath);
            strPath += "\\PROJECTS";
            if (Directory.Exists(strPath) == false)
                Directory.CreateDirectory(strPath);
            strPath += "\\" + intProject.ToString();
            if (Directory.Exists(strPath) == false)
                Directory.CreateDirectory(strPath);
            //        strPath += "\\Clarity";
            //        if (Directory.Exists(strPath) == false)
            //            Directory.CreateDirectory(strPath);
            //        strPath += "\\" + _now.Month.ToString() + _now.Day.ToString() + _now.Year.ToString() + _now.Hour.ToString() + _now.Minute.ToString() + _now.Second.ToString() + _now.Millisecond.ToString();
            strPath += "\\ClaritySetup";
            MailMessage oMessage = new MailMessage();
            MailAddress oFrom = new MailAddress(oUser.GetEmail(oUser.GetName(intProfile), intEnvironment));
            oMessage.From = oFrom;
            oMessage.CC.Add(oFrom);
            MailAddress oTo = new MailAddress(strClarity);
            oMessage.To.Add(oTo);
            // BCC
            char[] strSplit = { ';' };
            string[] strEmail = strEMailIdsBCC.Split(strSplit);
            for (int ii = 0; ii < strEmail.Length; ii++)
            {
                if (strEmail[ii].Trim() != "")
                {
                    string strAddress = oUser.GetEmail(strEmail[ii], intEnvironment);
                    if (strAddress != "")
                    {
                        MailAddress oBcc = new MailAddress(strAddress);
                        oMessage.Bcc.Add(oBcc);
                    }
                }
            }
            //strAttachment = GenerateExcel(strPath, oMessage);
            //strAttachment = GenerateText(strPath, oMessage);
            strAttachment = GenerateHTML(strPath, oMessage);
            oMessage.Subject = "Clarity Project Configuration";
            oMessage.Body = strClarityBody;
            if (strAttachment != "")
            {
                oMessage.Attachments.Add(new Attachment(strAttachment));
                SmtpClient oClient = new SmtpClient(oVariable.SmtpServer());
                oClient.Send(oMessage);
                //            oDocument.Add(intProject, intProfile, "Clarity Processing Form - " + DateTime.Now.ToShortDateString(), strAttachment, "Clarity Processing Form - " + DateTime.Now.ToString(), 1);
            }
            Response.Redirect(oPage.GetFullLink(intPage) + "?pid=" + intProject.ToString() + "&form=sent&div=1");
        }
        private void GenerateExcel(string strPath, MailMessage _message)
        {
            //_message.IsBodyHtml = false;
            //Microsoft.Office.Interop.Excel.Application oExcel = new Microsoft.Office.Interop.Excel.ApplicationClass();
            //oExcel.Visible = true;
            //Microsoft.Office.Interop.Excel.Workbook oWorkbook = oExcel.Workbooks.Open(strForm, 0, false, 5, "", "", false, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "", true, false, 0, true, false, false);
            //Microsoft.Office.Interop.Excel.Sheets oSheets = oWorkbook.Worksheets;
            //Microsoft.Office.Interop.Excel.Worksheet oWorksheet = (Microsoft.Office.Interop.Excel.Worksheet)oSheets.get_Item("Proj Attributes");
            //Microsoft.Office.Interop.Excel.Range oRange = (Microsoft.Office.Interop.Excel.Range)oWorksheet.get_Range("A1", "A1");
            //Response.Write(oRange.Value2.ToString());
            //oWorkbook.SaveAs("C:\\saved.xls", null, null, null, false, false, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange, null, null, null, null, null);
            //oWorkbook.Close(null, null, null);
            //oExcel.Workbooks.Close();
            //oExcel.Quit();
        }
        private string GenerateHTML(string strPath, MailMessage _message)
        {
            _message.IsBodyHtml = true;
            strClarityBody = "<html><body><p>Clarity Processing,<br/><br/>Attached is my project information required to generate my clarity number. Please let me know if you have any questions or require additional assistance.<br/><br/>Sincerely,<br/>" + oUser.Get(intProfile, "fname") + " " + oUser.Get(intProfile, "lname") + "</p></body></html>";
            try
            {
                string strNewFile = strPath + ".htm";
                if (File.Exists(strNewFile) == true)
                    File.Delete(strNewFile);
                oClarityFile = File.CreateText(strNewFile);
                oClarityFile.WriteLine("<html>");
                oClarityFile.WriteLine("<body>");
                oClarityFile.WriteLine("<p><a style=\"font-family:'openSans', Verdana, Arial, Helvetica, sans-serif; font-size:10px; color:#003399\" href=\"javascript:print();\">Print Page</a>&nbsp;&nbsp;&nbsp;&nbsp;<a style=\"font-family:'openSans', Verdana, Arial, Helvetica, sans-serif; font-size:10px; color:#003399\" href=\"javascript:window.close();\">Close Window</a></p>");
                oClarityFile.WriteLine("<table cellpadding=\"5\" cellspacing=\"0\" border=\"1\" style=\"font-family:'openSans', Verdana, Arial, Helvetica, sans-serif; font-size:10px; color:#404040\">");
                oClarityFile.WriteLine("<tr><td colspan=\"3\" style=\"font-size:12px\"><b>Work Detail Screen</b></td></tr>");
                oClarityFile.WriteLine("<tr><td><b>Field Name</b></td><td><b>Field Value</b></td><td><b>Clarity Use</b></td></tr>");
                oClarityFile.WriteLine("<tr><td>Project Name</td><td>" + txtName.Text + "</td><td>Properties - General tab</td></tr>");
                oClarityFile.WriteLine("<tr><td>Brief Description of the Project</td><td>" + txtDescription.Text + "</td><td>Properties - General tab</td></tr>");
                oClarityFile.WriteLine("<tr><td>Project Start Date</td><td>" + lblStartDate.Text + "</td><td>Properties - General tab</td></tr>");
                oClarityFile.WriteLine("<tr><td>Project Finish Date</td><td>Pending</td><td>Properties - General tab</td></tr>");
                oClarityFile.WriteLine("<tr><td>Project Manager</td><td>" + oUser.GetFullName(intProfile) + "</td><td>Properties - General tab</td></tr>");
                oClarityFile.WriteLine("<tr><td>Priority</td><td>30</td><td>Properties - General tab</td></tr>");
                oClarityFile.WriteLine("<tr><td>Project Category</td><td>New</td><td>Properties - General tab</td></tr>");
                oClarityFile.WriteLine("<tr><td>NCC Status</td><td>Approved</td><td>Properties - General tab</td></tr>");
                oClarityFile.WriteLine("<tr><td>Status</td><td>Approved</td><td>Properties - General tab</td></tr>");
                oClarityFile.WriteLine("<tr><td>Business Outcome</td><td>Pending</td><td>Properties - General tab</td></tr>");
                oClarityFile.WriteLine("<tr><td>NCC Application Code</td><td>ISD</td><td>Properties - General tab</td></tr>");
                string strPlatform = GetPlatform();
                oClarityFile.WriteLine("<tr><td>NCC Platform</td><td>" + strPlatform + "</td><td>Properties - General tab</td></tr>");
                oClarityFile.WriteLine("<tr><td>National City OBS</td><td>" + ddlPortfolio.SelectedItem.Text + " - " + oApplication.GetName(intApplication) + "</td><td>Properties - General tab</td></tr>");

                oClarityFile.WriteLine("<tr><td colspan=\"3\">&nbsp;</td></tr>");
                oClarityFile.WriteLine("<tr><td colspan=\"3\" style=\"font-size:12px\"><b>Schedule Screen</b></td></tr>");
                oClarityFile.WriteLine("<tr><td><b>Field Name</b></td><td><b>Field Value</b></td><td><b>Clarity Use</b></td></tr>");
                oClarityFile.WriteLine("<tr><td>Charge Code</td><td>" + GetCode() + "</td><td>Properties - Schedule tab</td></tr>");

                oClarityFile.WriteLine("<tr><td colspan=\"3\">&nbsp;</td></tr>");
                oClarityFile.WriteLine("<tr><td colspan=\"3\" style=\"font-size:12px\"><b>Key Persons Screen</b></td></tr>");
                oClarityFile.WriteLine("<tr><td><b>Field Name</b></td><td><b>Field Value</b></td><td><b>Clarity Use</b></td></tr>");
                oClarityFile.WriteLine("<tr><td>NCC BIO</td><td>" + oUser.GetFullName(intExecutive) + "</td><td>Properties - Key Person tab</td></tr>");
                oClarityFile.WriteLine("<tr><td>NCC Financial Manager</td><td>" + oUser.GetFullName(intWorking) + "</td><td>Properties - Key Person tab & General tab</td></tr>");
                oClarityFile.WriteLine("<tr><td>NCC SIO</td><td>" + "GENERAL MANAGER" + "</td><td>Properties - Key Person tab</td></tr>");
                oClarityFile.WriteLine("<tr><td>NCC Group Manager</td><td>" + oUser.GetFullName(oUser.GetManager(intProfile, true)) + "</td><td>Properties - Key Person tab</td></tr>");

                oClarityFile.WriteLine("<tr><td colspan=\"3\">&nbsp;</td></tr>");
                oClarityFile.WriteLine("<tr><td colspan=\"3\" style=\"font-size:12px\"><b>Financial Screen</b></td></tr>");
                oClarityFile.WriteLine("<tr><td><b>Field Name</b></td><td><b>Field Value</b></td><td><b>Clarity Use</b></td></tr>");
                string strType = (ddlBaseDisc.SelectedItem.Value == "Base" ? "BASE (Base Project)" : "DISCRET (Discretionary Project)");
                oClarityFile.WriteLine("<tr><td>Project Class</td><td>" + strType + "</td><td>Properties - Financial tab</td></tr>");
                oClarityFile.WriteLine("<tr><td>Location/Department</td><td>" + strType + "</td><td>Properties - Financial tab</td></tr>");

                oClarityFile.WriteLine("<tr><td colspan=\"3\">&nbsp;</td></tr>");
                oClarityFile.WriteLine("<tr><td colspan=\"3\" style=\"font-size:12px\"><b>Line of Business Funding Allocations</b></td></tr>");
                oClarityFile.WriteLine("<tr><td><b>Field Name</b></td><td><b>Field Value</b></td><td><b>Clarity Use</b></td></tr>");
                oClarityFile.WriteLine("<tr><td>1) Sub-Line of Credit</td><td>Base Project - Base Project</td><td>Properties - Financial tab</td></tr>");
                oClarityFile.WriteLine("<tr><td>Percentage (%)</td><td>100%</td><td>Properties - Financial tab</td></tr>");

                oClarityFile.WriteLine("<tr><td colspan=\"3\">&nbsp;</td></tr>");
                oClarityFile.WriteLine("<tr><td colspan=\"3\" style=\"font-size:12px\"><b>Project Template</b></td></tr>");
                oClarityFile.WriteLine("<tr><td><b>Field Name</b></td><td><b>Field Value</b></td><td><b>Clarity Use</b></td></tr>");
                oClarityFile.WriteLine("<tr><td>Clarity Project Template</td><td>EPS Renewal " + strPlatform + "</td><td>Properties</td></tr>");

                oClarityFile.WriteLine("</table>");
                oClarityFile.WriteLine("</body>");
                oClarityFile.WriteLine("</html>");
                oClarityFile.Flush();
                oClarityFile.Close();
                return strNewFile;
            }
            catch
            {
                return "";
            }
        }
        private string GenerateText(string strPath, MailMessage _message)
        {
            _message.IsBodyHtml = false;
            strClarityBody = "Clarity Processing,\n\nAttached is my project information required to generate my clarity number. Please let me know if you have any questions or require addtional assistance.\n\nSincerely,\n" + oUser.Get(intProfile, "fname") + " " + oUser.Get(intProfile, "lname") + "\n\n\n\n\n";
            try
            {
                string strNewFile = strPath + ".txt";
                if (File.Exists(strNewFile) == true)
                    File.Delete(strNewFile);
                oClarityFile = File.CreateText(strNewFile);
                oClarityFile.WriteLine("************************************************");
                oClarityFile.WriteLine("************  Work Detail Screen  **************");
                oClarityFile.WriteLine("************************************************");
                oClarityFile.WriteLine("Project Name ...................... " + txtName.Text);
                oClarityFile.WriteLine("Brief Description of the Project .. " + txtDescription.Text);
                oClarityFile.WriteLine("Project Start Date ................ " + lblStartDate.Text);
                oClarityFile.WriteLine("Project Finish Date ............... Pending");
                oClarityFile.WriteLine("Project Manager ................... " + oUser.GetFullName(intProfile));
                oClarityFile.WriteLine("Priority .......................... 30");
                oClarityFile.WriteLine("Project Category .................. New");
                oClarityFile.WriteLine("NCC Status ........................ Approved");
                oClarityFile.WriteLine("Status ............................ Approved");
                oClarityFile.WriteLine("Business Outcome .................. Pending");
                oClarityFile.WriteLine("NCC Application Code .............. ISD");
                string strPlatform = GetPlatform();
                oClarityFile.WriteLine("NCC Platform ...................... " + strPlatform);
                oClarityFile.WriteLine("National City OBS ................. " + ddlPortfolio.SelectedItem.Text + " - " + oApplication.GetName(intApplication));
                oClarityFile.WriteLine("************************************************");
                oClarityFile.WriteLine("*************  Schedule Screen  ****************");
                oClarityFile.WriteLine("************************************************");
                oClarityFile.WriteLine("Charge Code ....................... " + GetCode());
                oClarityFile.WriteLine("************************************************");
                oClarityFile.WriteLine("***********  Key Persons Screen  ***************");
                oClarityFile.WriteLine("************************************************");
                oClarityFile.WriteLine("NCC BIO ........................... " + oUser.GetFullName(intExecutive));
                oClarityFile.WriteLine("NCC Financial Manager ............. " + oUser.GetFullName(intWorking));
                oClarityFile.WriteLine("NCC SIO ........................... " + "GENERAL MANAGER");
                oClarityFile.WriteLine("NCC Group Manager ................. " + oUser.GetFullName(oUser.GetManager(intProfile, true)));
                oClarityFile.WriteLine("************************************************");
                oClarityFile.WriteLine("************  Financial Screen  ****************");
                oClarityFile.WriteLine("************************************************");
                string strType = (ddlBaseDisc.SelectedItem.Value == "Base" ? "BASE (Base Project)" : "DISCRET (Discretionary Project)");
                oClarityFile.WriteLine("Project Class ..................... " + strType);
                oClarityFile.WriteLine("Location/Department ............... " + strType);
                oClarityFile.WriteLine("************************************************");
                oClarityFile.WriteLine("****  Line of Business Funding Allocations  ****");
                oClarityFile.WriteLine("************************************************");
                oClarityFile.WriteLine("1) Sub-Line of Credit ............. Base Project - Base Project");
                oClarityFile.WriteLine("Percentage (%) .................... 100%");
                oClarityFile.WriteLine("************************************************");
                oClarityFile.WriteLine("************  Project Template  ****************");
                oClarityFile.WriteLine("************************************************");
                oClarityFile.WriteLine("Clarity Project Template .......... EPS Renewal " + strPlatform);
                oClarityFile.Flush();
                oClarityFile.Close();
                return strNewFile;
            }
            catch
            {
                return "";
            }
        }
        private string GetPlatform()
        {
            string strPlatform = "";
            DataSet dsPlatforms = oProjectRequest.GetPlatforms(intRequest);
            bool boolIntel = false;
            bool boolUnix = false;
            bool boolMainframe = false;
            foreach (DataRow drPlatform in dsPlatforms.Tables[0].Rows)
            {
                if (drPlatform["name"].ToString() == "Distributive")
                    boolIntel = true;
                if (drPlatform["name"].ToString() == "Mainframe")
                    boolMainframe = true;
                if (drPlatform["name"].ToString() == "Midrange")
                    boolUnix = true;
            }
            if (boolMainframe == true)
                strPlatform = "Mainframe";
            else if (boolUnix == true)
                strPlatform = "Unix";
            else if (boolIntel == true)
                strPlatform = "Intel";
            else
                strPlatform = "Not Listed";
            return strPlatform;
        }
        private string GetCode()
        {
            string strCode = "Expensed = Labor is not SOP 98-1 eligible";
            if (oProjectRequest.Get(intRequest, "internal_labor") == "Greater than $1,000,000" || oProjectRequest.Get(intRequest, "external_labor") == "Greater than $1,000,000")
                strCode = "Capitalized = software development labor is eligible for capitalization under AICPA SOP 98-1";
            return strCode;
        }
    }
}