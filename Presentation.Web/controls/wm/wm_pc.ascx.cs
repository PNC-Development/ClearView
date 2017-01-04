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
namespace NCC.ClearView.Presentation.Web
{
    public partial class wm_pc : System.Web.UI.UserControl
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
        protected bool boolStep1 = false;
        protected bool boolStep2 = false;
        protected bool boolStep3 = false;
        protected bool boolStep4 = false;
        protected bool boolStep5 = false;
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
        protected string strHelpText = "START HERE!";
        protected string strForm = "C:\\inetpub\\wwwroot\\clearview\\help\\clarity.xls";
        protected string strClarityBody = "";
        private StreamWriter oClarityFile;
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
                panStart.Visible = (strHelpText != "");
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
                hdnTab.Value = "1";
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
                ddlPortfolio.Attributes.Add("onchange", "PopulateSegments('" + ddlPortfolio.ClientID + "','" + ddlSegment.ClientID + "');");
                ddlSegment.Attributes.Add("onchange", "UpdateDropDownHidden('" + ddlSegment.ClientID + "','" + hdnSegment.ClientID + "');");
                btnSave.Attributes.Add("onclick", "return ValidatePhase('" + hdnTab.ClientID + "','" + hdnPhase.ClientID + "') && ValidateStatusTPM('" + hdnTab.ClientID + "','" + ddlScope.ClientID + "','" + ddlTimeline.ClientID + "','" + ddlBudget.ClientID + "','" + txtStatusDate.ClientID + "','" + txtComments.ClientID + "','" + txtThisWeek.ClientID + "','" + txtNextWeek.ClientID + "') && ValidateMilestone('" + hdnTab.ClientID + "','" + txtMilestoneApproved.ClientID + "','" + txtMilestoneForecasted.ClientID + "','" + txtMilestone.ClientID + "','" + txtDetail.ClientID + "');");
                btnMessage.Attributes.Add("onclick", "return ValidateDropDown('" + ddlResource.ClientID + "','Please make a selection for the recipient of this communication')" +
                    " && ValidateDropDown('" + ddlCommunication.ClientID + "','Please make a selection for the form of communication')" +
                    " && ValidateText('" + txtMessage.ClientID + "','Please enter some text for the communication')" +
                    ";");
                btnCostAdd.Attributes.Add("onclick", "return MoveList('" + lstCostsAvailable.ClientID + "','" + lstCostsCurrent.ClientID + "','" + hdnCosts.ClientID + "','" + lstCostsCurrent.ClientID + "');");
                lstCostsAvailable.Attributes.Add("ondblclick", "return MoveList('" + lstCostsAvailable.ClientID + "','" + lstCostsCurrent.ClientID + "','" + hdnCosts.ClientID + "','" + lstCostsCurrent.ClientID + "');");
                btnCostRemove.Attributes.Add("onclick", "return MoveList('" + lstCostsCurrent.ClientID + "','" + lstCostsAvailable.ClientID + "','" + hdnCosts.ClientID + "','" + lstCostsCurrent.ClientID + "');");
                lstCostsCurrent.Attributes.Add("ondblclick", "return MoveList('" + lstCostsCurrent.ClientID + "','" + lstCostsAvailable.ClientID + "','" + hdnCosts.ClientID + "','" + lstCostsCurrent.ClientID + "');");
                // Dates
                imgtxtActDS.Attributes.Add("onclick", "return ShowCalendar('" + txtActDS.ClientID + "');");
                imgtxtActDF.Attributes.Add("onclick", "return ShowCalendar('" + txtActDF.ClientID + "');");
                imgtxtActPS.Attributes.Add("onclick", "return ShowCalendar('" + txtActPS.ClientID + "');");
                imgtxtActPF.Attributes.Add("onclick", "return ShowCalendar('" + txtActPF.ClientID + "');");
                imgtxtActES.Attributes.Add("onclick", "return ShowCalendar('" + txtActES.ClientID + "');");
                imgtxtActEF.Attributes.Add("onclick", "return ShowCalendar('" + txtActEF.ClientID + "');");
                imgtxtAppPS.Attributes.Add("onclick", "return ShowCalendar('" + txtAppPS.ClientID + "');");
                imgtxtAppPF.Attributes.Add("onclick", "return ShowCalendar('" + txtAppPF.ClientID + "');");
                imgtxtAppES.Attributes.Add("onclick", "return ShowCalendar('" + txtAppES.ClientID + "');");
                imgtxtAppEF.Attributes.Add("onclick", "return ShowCalendar('" + txtAppEF.ClientID + "');");
                // Calculators
                imgtxtActDI.Attributes.Add("onclick", "return ShowCalculator('" + txtActDI.ClientID + "');");
                imgtxtActDE.Attributes.Add("onclick", "return ShowCalculator('" + txtActDE.ClientID + "');");
                imgtxtActDH.Attributes.Add("onclick", "return ShowCalculator('" + txtActDH.ClientID + "');");
                imgtxtAppPI.Attributes.Add("onclick", "return ShowCalculator('" + txtAppPI.ClientID + "');");
                imgtxtAppPE.Attributes.Add("onclick", "return ShowCalculator('" + txtAppPE.ClientID + "');");
                imgtxtAppPH.Attributes.Add("onclick", "return ShowCalculator('" + txtAppPH.ClientID + "');");
                imgtxtActPI.Attributes.Add("onclick", "return ShowCalculator('" + txtActPI.ClientID + "');");
                imgtxtActPE.Attributes.Add("onclick", "return ShowCalculator('" + txtActPE.ClientID + "');");
                imgtxtActPH.Attributes.Add("onclick", "return ShowCalculator('" + txtActPH.ClientID + "');");
                imgtxtAppEI.Attributes.Add("onclick", "return ShowCalculator('" + txtAppEI.ClientID + "');");
                imgtxtAppEE.Attributes.Add("onclick", "return ShowCalculator('" + txtAppEE.ClientID + "');");
                imgtxtAppEH.Attributes.Add("onclick", "return ShowCalculator('" + txtAppEH.ClientID + "');");
                imgtxtActEI.Attributes.Add("onclick", "return ShowCalculator('" + txtActEI.ClientID + "');");
                imgtxtActEE.Attributes.Add("onclick", "return ShowCalculator('" + txtActEE.ClientID + "');");
                imgtxtActEH.Attributes.Add("onclick", "return ShowCalculator('" + txtActEH.ClientID + "');");
                imgStart.Attributes.Add("onclick", "return ShowCalendar('" + txtStartDate.ClientID + "');");
                imgStatusDate.Attributes.Add("onclick", "return ShowCalendar('" + txtStatusDate.ClientID + "');");
                imgMilestoneApproved.Attributes.Add("onclick", "return ShowCalendar('" + txtMilestoneApproved.ClientID + "');");
                imgMilestoneForecasted.Attributes.Add("onclick", "return ShowCalendar('" + txtMilestoneForecasted.ClientID + "');");
                txtWorking.Attributes.Add("onkeyup", "return AJAXTextBoxGet(this,'250','195','" + divWorking.ClientID + "','" + lstWorking.ClientID + "','" + hdnWorking.ClientID + "','" + oVariable.URL() + "/frame/users.aspx',2);");
                lstWorking.Attributes.Add("ondblclick", "AJAXClickRow();");
                txtExecutive.Attributes.Add("onkeyup", "return AJAXTextBoxGet(this,'250','195','" + divExecutive.ClientID + "','" + lstExecutive.ClientID + "','" + hdnExecutive.ClientID + "','" + oVariable.URL() + "/frame/users.aspx',2);");
                lstExecutive.Attributes.Add("ondblclick", "AJAXClickRow();");
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
                Label _timeline = (Label)ri.FindControl("lblTimeline");
                Label _budget = (Label)ri.FindControl("lblBudget");
                Label _status = (Label)ri.FindControl("lblStatus");
                Label _status_scope = (Label)ri.FindControl("lblStatusScope");
                Label _status_timeline = (Label)ri.FindControl("lblStatusTimeline");
                Label _status_budget = (Label)ri.FindControl("lblStatusBudget");
                double dblScope = double.Parse(_scope.Text);
                double dblTimeline = double.Parse(_timeline.Text);
                double dblBudget = double.Parse(_budget.Text);
                _status_scope.Text = oResourceRequest.GetStatus(dblScope, 60, 10);
                _status_timeline.Text = oResourceRequest.GetStatus(dblTimeline, 60, 10);
                _status_budget.Text = oResourceRequest.GetStatus(dblBudget, 60, 10);
                _status.Text = oResourceRequest.GetStatus(dblScope, dblTimeline, dblBudget, 83, 15);
                dblTotalScope = dblScope;
                dblTotalTimeline = dblTimeline;
                dblTotalBudget = dblBudget;
            }
            lblStatus.Text = oResourceRequest.GetStatus(dblTotalScope, dblTotalTimeline, dblTotalBudget, 83, 15);
        }
        private void LoadInformation(int _request)
        {
            Organizations oOrganization = new Organizations(intProfile, dsn);
            DataSet dsProject = oProject.Get(intProject);
            txtName.Text = oProject.Get(intProject, "name");
            txtCustom.Text = oResourceRequest.GetWorkflow(_request, "name");
            txtNumber.Text = oProject.Get(intProject, "number");
            lblNumber.Text = txtNumber.Text;
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
            // Project Schedule
            DataSet dsRequest = oTPM.Get(intRequest, intItem, intNumber);
            if (dsRequest.Tables[0].Rows.Count == 0)
            {
                oTPM.Add(intRequest, intItem, intNumber, intProfile);
                dsRequest = oTPM.Get(intRequest, intItem, intNumber);
            }
            if (dsRequest.Tables[0].Rows[0]["appsp"].ToString() != "")
                txtAppPS.Text = DateTime.Parse(dsRequest.Tables[0].Rows[0]["appsp"].ToString()).ToShortDateString();
            if (dsRequest.Tables[0].Rows[0]["appep"].ToString() != "")
                txtAppPF.Text = DateTime.Parse(dsRequest.Tables[0].Rows[0]["appep"].ToString()).ToShortDateString();
            if (dsRequest.Tables[0].Rows[0]["appse"].ToString() != "")
                txtAppES.Text = DateTime.Parse(dsRequest.Tables[0].Rows[0]["appse"].ToString()).ToShortDateString();
            if (dsRequest.Tables[0].Rows[0]["appee"].ToString() != "")
                txtAppEF.Text = DateTime.Parse(dsRequest.Tables[0].Rows[0]["appee"].ToString()).ToShortDateString();
            lblApprovedF.Text = (txtAppEF.Text == "" ? "---" : txtAppEF.Text);
            if (dsRequest.Tables[0].Rows[0]["start_d"].ToString() != "")
                txtActDS.Text = DateTime.Parse(dsRequest.Tables[0].Rows[0]["start_d"].ToString()).ToShortDateString();
            lblActualS.Text = (txtActDS.Text == "" ? "---" : txtActDS.Text);
            if (dsRequest.Tables[0].Rows[0]["end_d"].ToString() != "")
                txtActDF.Text = DateTime.Parse(dsRequest.Tables[0].Rows[0]["end_d"].ToString()).ToShortDateString();
            if (dsRequest.Tables[0].Rows[0]["start_p"].ToString() != "")
                txtActPS.Text = DateTime.Parse(dsRequest.Tables[0].Rows[0]["start_p"].ToString()).ToShortDateString();
            if (dsRequest.Tables[0].Rows[0]["end_p"].ToString() != "")
                txtActPF.Text = DateTime.Parse(dsRequest.Tables[0].Rows[0]["end_p"].ToString()).ToShortDateString();
            if (dsRequest.Tables[0].Rows[0]["start_e"].ToString() != "")
                txtActES.Text = DateTime.Parse(dsRequest.Tables[0].Rows[0]["start_e"].ToString()).ToShortDateString();
            if (dsRequest.Tables[0].Rows[0]["end_e"].ToString() != "")
                txtActEF.Text = DateTime.Parse(dsRequest.Tables[0].Rows[0]["end_e"].ToString()).ToShortDateString();
            lblActualF.Text = (txtActEF.Text == "" ? "---" : txtActEF.Text);
            if (txtAppPF.Text == "" || txtActPF.Text == "")
                lblVarianceDaysP.Text = "---";
            else
            {
                TimeSpan oSpan = DateTime.Parse(txtActPF.Text).Subtract(DateTime.Parse(txtAppPF.Text));
                lblVarianceDaysP.Text = oSpan.Days.ToString();
            }
            if (txtAppPS.Text == "" || txtAppPF.Text == "" || txtActPF.Text == "")
                lblVariancePercentP.Text = "---";
            else
            {
                TimeSpan oSpan1 = DateTime.Parse(txtActPF.Text).Subtract(DateTime.Parse(txtAppPS.Text));
                TimeSpan oSpan2 = DateTime.Parse(txtAppPF.Text).Subtract(DateTime.Parse(txtAppPS.Text));
                double dblSpan = (double.Parse(oSpan1.Days.ToString()) / double.Parse(oSpan2.Days.ToString()) - 1.00) * 100;
                lblVariancePercentP.Text = dblSpan.ToString("F") + "%";
            }
            if (txtAppEF.Text == "" || txtActEF.Text == "")
                lblVarianceDaysE.Text = "---";
            else
            {
                TimeSpan oSpan = DateTime.Parse(txtActEF.Text).Subtract(DateTime.Parse(txtAppEF.Text));
                lblVarianceDaysE.Text = oSpan.Days.ToString();
            }
            if (txtAppES.Text == "" || txtAppEF.Text == "" || txtActEF.Text == "")
                lblVariancePercentE.Text = "---";
            else
            {
                TimeSpan oSpan1 = DateTime.Parse(txtActEF.Text).Subtract(DateTime.Parse(txtAppES.Text));
                TimeSpan oSpan2 = DateTime.Parse(txtAppEF.Text).Subtract(DateTime.Parse(txtAppES.Text));
                double dblSpan = (double.Parse(oSpan1.Days.ToString()) / double.Parse(oSpan2.Days.ToString()) - 1.00) * 100;
                lblVariancePercentE.Text = dblSpan.ToString("F") + "%";
            }

            // ****************************************************
            // ***************** Project Financials ***************
            // ****************************************************
            // ..... DISCOVERY .....
            double dblActDI = 0.00;
            if (dsRequest.Tables[0].Rows[0]["actid"].ToString() != "")
                dblActDI = double.Parse(dsRequest.Tables[0].Rows[0]["actid"].ToString());
            txtActDI.Text = dblActDI.ToString("F");
            double dblActDE = 0.00;
            if (dsRequest.Tables[0].Rows[0]["acted"].ToString() != "")
                dblActDE = double.Parse(dsRequest.Tables[0].Rows[0]["acted"].ToString());
            txtActDE.Text = dblActDE.ToString("F");
            double dblActDH = 0.00;
            if (dsRequest.Tables[0].Rows[0]["acthd"].ToString() != "")
                dblActDH = double.Parse(dsRequest.Tables[0].Rows[0]["acthd"].ToString());
            txtActDH.Text = dblActDH.ToString("F");
            double dblActD = dblActDI + dblActDE + dblActDH;
            lblActD.Text = dblActD.ToString("N");
            // ..... PLANNING .....
            double dblAppPI = 0.00;
            if (dsRequest.Tables[0].Rows[0]["appip"].ToString() != "")
                dblAppPI = double.Parse(dsRequest.Tables[0].Rows[0]["appip"].ToString());
            txtAppPI.Text = dblAppPI.ToString("F");
            double dblAppPE = 0.00;
            if (dsRequest.Tables[0].Rows[0]["appexp"].ToString() != "")
                dblAppPE = double.Parse(dsRequest.Tables[0].Rows[0]["appexp"].ToString());
            txtAppPE.Text = dblAppPE.ToString("F");
            double dblAppPH = 0.00;
            if (dsRequest.Tables[0].Rows[0]["apphp"].ToString() != "")
                dblAppPH = double.Parse(dsRequest.Tables[0].Rows[0]["apphp"].ToString());
            txtAppPH.Text = dblAppPH.ToString("F");
            double dblActPI = 0.00;
            if (dsRequest.Tables[0].Rows[0]["actip"].ToString() != "")
                dblActPI = double.Parse(dsRequest.Tables[0].Rows[0]["actip"].ToString());
            txtActPI.Text = dblActPI.ToString("F");
            double dblActPE = 0.00;
            if (dsRequest.Tables[0].Rows[0]["actep"].ToString() != "")
                dblActPE = double.Parse(dsRequest.Tables[0].Rows[0]["actep"].ToString());
            txtActPE.Text = dblActPE.ToString("F");
            double dblActPH = 0.00;
            if (dsRequest.Tables[0].Rows[0]["acthp"].ToString() != "")
                dblActPH = double.Parse(dsRequest.Tables[0].Rows[0]["acthp"].ToString());
            txtActPH.Text = dblActPH.ToString("F");
            double dblEstPI = 0.00;
            if (dsRequest.Tables[0].Rows[0]["estip"].ToString() != "")
                dblEstPI = double.Parse(dsRequest.Tables[0].Rows[0]["estip"].ToString());
            double dblVarDPI = dblActPI - dblAppPI;
            lblVarDPI.Text = dblVarDPI.ToString("N");
            double dblVarDPE = dblActPE - dblAppPE;
            lblVarDPE.Text = dblVarDPE.ToString("N");
            double dblVarDPH = dblActPH - dblAppPH;
            lblVarDPH.Text = dblVarDPH.ToString("N");
            double dblVarPPI = dblVarDPI * 100 / dblAppPI;
            lblVarPPI.Text = dblVarPPI.ToString("F") + "%";
            double dblVarPPE = dblVarDPE * 100 / dblAppPE;
            lblVarPPE.Text = dblVarPPE.ToString("F") + "%";
            double dblVarPPH = dblVarDPH * 100 / dblAppPH;
            lblVarPPH.Text = dblVarPPH.ToString("F") + "%";
            double dblAppP = dblAppPI + dblAppPE + dblAppPH;
            lblAppP.Text = dblAppP.ToString("N");
            double dblActP = dblActPI + dblActPE + dblActPH;
            lblActP.Text = dblActP.ToString("N");
            double dblVarDP = dblVarDPI + dblVarDPE + dblVarDPH;
            lblVarDP.Text = dblVarDP.ToString("N");
            double dblVarPP = dblVarDP * 100 / dblAppP;
            lblVarPP.Text = dblVarPP.ToString("F") + "%";
            // ..... EXECUTION  .....
            double dblAppEI = 0.00;
            if (dsRequest.Tables[0].Rows[0]["appie"].ToString() != "")
                dblAppEI = double.Parse(dsRequest.Tables[0].Rows[0]["appie"].ToString());
            txtAppEI.Text = dblAppEI.ToString("F");
            double dblAppEE = 0.00;
            if (dsRequest.Tables[0].Rows[0]["appexe"].ToString() != "")
                dblAppEE = double.Parse(dsRequest.Tables[0].Rows[0]["appexe"].ToString());
            txtAppEE.Text = dblAppEE.ToString("F");
            double dblAppEH = 0.00;
            if (dsRequest.Tables[0].Rows[0]["apphe"].ToString() != "")
                dblAppEH = double.Parse(dsRequest.Tables[0].Rows[0]["apphe"].ToString());
            txtAppEH.Text = dblAppEH.ToString("F");
            double dblActEI = 0.00;
            if (dsRequest.Tables[0].Rows[0]["actie"].ToString() != "")
                dblActEI = double.Parse(dsRequest.Tables[0].Rows[0]["actie"].ToString());
            txtActEI.Text = dblActEI.ToString("F");
            double dblActEE = 0.00;
            if (dsRequest.Tables[0].Rows[0]["actee"].ToString() != "")
                dblActEE = double.Parse(dsRequest.Tables[0].Rows[0]["actee"].ToString());
            txtActEE.Text = dblActEE.ToString("F");
            double dblActEH = 0.00;
            if (dsRequest.Tables[0].Rows[0]["acthe"].ToString() != "")
                dblActEH = double.Parse(dsRequest.Tables[0].Rows[0]["acthe"].ToString());
            txtActEH.Text = dblActEH.ToString("F");
            double dblVarDEI = dblActEI - dblAppEI;
            lblVarDEI.Text = dblVarDEI.ToString("N");
            double dblVarDEE = dblActEE - dblAppEE;
            lblVarDEE.Text = dblVarDEE.ToString("N");
            double dblVarDEH = dblActEH - dblAppEH;
            lblVarDEH.Text = dblVarDEH.ToString("N");
            double dblVarPEI = dblVarDEI * 100 / dblAppEI;
            lblVarPEI.Text = dblVarPEI.ToString("F") + "%";
            double dblVarPEE = dblVarDEE * 100 / dblAppEE;
            lblVarPEE.Text = dblVarPEE.ToString("F") + "%";
            double dblVarPEH = dblVarDEH * 100 / dblAppEH;
            lblVarPEH.Text = dblVarPEH.ToString("F") + "%";
            double dblAppE = dblAppEI + dblAppEE + dblAppEH;
            lblAppE.Text = dblAppE.ToString("N");
            double dblActE = dblActEI + dblActEE + dblActEH;
            lblActE.Text = dblActE.ToString("N");
            double dblVarDE = dblVarDEI + dblVarDEE + dblVarDEH;
            lblVarDE.Text = dblVarDE.ToString("N");
            double dblVarPE = dblVarDE * 100 / dblAppE;
            lblVarPE.Text = dblVarPE.ToString("F") + "%";
            // Totals
            double dblApp = dblActD + dblAppP + dblAppE;
            double dblAct = dblActD + dblActP + dblActE;
            double dblVarD = dblVarDP + dblVarDE;
            double dblVarP = dblVarD * 100 / dblApp;
            double dblApprovedI = dblActDI + dblAppPI + dblAppEI;
            double dblApprovedE = dblActDE + dblAppPE + dblAppEE;
            double dblApprovedC = dblActDH + dblAppPH + dblAppEH;
            double dblActualI = dblActDI + dblActPI + dblActEI;
            double dblActualE = dblActDE + dblActPE + dblActEE;
            double dblActualC = dblActDH + dblActPH + dblActEH;
            double dblAllocated = 0.00;
            double dblUsed = 0.00;
            DataSet dsHours;
            // Discovery hours
            lblDiscoveryHRs.Text = dsRequest.Tables[0].Rows[0]["d_hrs"].ToString();
            txtDiscovery.Text = lblDiscoveryHRs.Text;
            dblAllocated = double.Parse(lblDiscoveryHRs.Text);
            dblUsed = 0.00;
            dsHours = oTPM.GetHours(intRequest, intItem, intNumber, "D");
            if (dsHours.Tables[0].Rows.Count > 0)
                dblUsed = double.Parse(dsHours.Tables[0].Rows[0]["used"].ToString());
            dblUsed = (dblUsed / dblAllocated) * 100;
            sldDiscovery._StartPercent = dblUsed.ToString();
            sldDiscovery._TotalHours = dblAllocated.ToString();
            // Planning Hours
            lblPlanningHRs.Text = dsRequest.Tables[0].Rows[0]["p_hrs"].ToString();
            txtPlanning.Text = lblPlanningHRs.Text;
            dblAllocated = double.Parse(lblPlanningHRs.Text);
            dblUsed = 0.00;
            dsHours = oTPM.GetHours(intRequest, intItem, intNumber, "P");
            if (dsHours.Tables[0].Rows.Count > 0)
                dblUsed = double.Parse(dsHours.Tables[0].Rows[0]["used"].ToString());
            dblUsed = (dblUsed / dblAllocated) * 100;
            sldPlanning._StartPercent = dblUsed.ToString();
            sldPlanning._TotalHours = dblAllocated.ToString();
            // Execution Hours
            lblExecutionHRs.Text = dsRequest.Tables[0].Rows[0]["e_hrs"].ToString();
            txtExecution.Text = lblExecutionHRs.Text;
            dblAllocated = double.Parse(lblExecutionHRs.Text);
            dblUsed = 0.00;
            dsHours = oTPM.GetHours(intRequest, intItem, intNumber, "E");
            if (dsHours.Tables[0].Rows.Count > 0)
                dblUsed = double.Parse(dsHours.Tables[0].Rows[0]["used"].ToString());
            dblUsed = (dblUsed / dblAllocated) * 100;
            sldExecution._StartPercent = dblUsed.ToString();
            sldExecution._TotalHours = dblAllocated.ToString();
            // Closing Hours
            lblClosingHRs.Text = dsRequest.Tables[0].Rows[0]["c_hrs"].ToString();
            txtClosing.Text = lblClosingHRs.Text;
            dblAllocated = double.Parse(lblClosingHRs.Text);
            dblUsed = 0.00;
            dsHours = oTPM.GetHours(intRequest, intItem, intNumber, "C");
            if (dsHours.Tables[0].Rows.Count > 0)
                dblUsed = double.Parse(dsHours.Tables[0].Rows[0]["used"].ToString());
            dblUsed = (dblUsed / dblAllocated) * 100;
            sldClosing._StartPercent = dblUsed.ToString();
            sldClosing._TotalHours = dblAllocated.ToString();
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
            if (dsRequest.Tables[0].Rows[0]["project_start_date"].ToString() != "")
                txtStartDate.Text = DateTime.Parse(dsRequest.Tables[0].Rows[0]["project_start_date"].ToString()).ToShortDateString();
            lblApprovedS.Text = (txtStartDate.Text == "" ? "---" : txtStartDate.Text);
            txtBetter.Text = dsRequest.Tables[0].Rows[0]["better"].ToString();
            txtWorse.Text = dsRequest.Tables[0].Rows[0]["worse"].ToString();
            txtLessons.Text = dsRequest.Tables[0].Rows[0]["lessons"].ToString();
            lblApprovedI.Text = dblApprovedI.ToString("N");
            lblApprovedE.Text = dblApprovedE.ToString("N");
            lblApprovedC.Text = dblApprovedC.ToString("N");
            double dblApproved = dblApprovedC + dblApprovedE + dblApprovedI;
            lblApproved.Text = dblApproved.ToString("N");
            lblActualI.Text = dblActualI.ToString("N");
            lblActualE.Text = dblActualE.ToString("N");
            lblActualC.Text = dblActualC.ToString("N");
            double dblActual = dblActualI + dblActualE + dblActualC;
            lblActual.Text = dblActual.ToString("N");
            double dblVarianceI = dblApprovedI - dblActualI;
            double dblVarianceE = dblApprovedE - dblActualE;
            double dblVarianceC = dblApprovedC - dblActualC;
            lblVarianceI.Text = dblVarianceI.ToString("N");
            lblVarianceE.Text = dblVarianceE.ToString("N");
            lblVarianceC.Text = dblVarianceC.ToString("N");
            double dblVariance = dblVarianceI + dblVarianceE + dblVarianceC;
            lblVariance.Text = dblVariance.ToString("N");
            lblTotalApproved.Text = lblApproved.Text;
            lblTotalActual.Text = lblActual.Text;
            lblTotalVariance.Text = lblVariance.Text;
            if (lblApprovedS.Text != "---" && lblActualS.Text != "---")
            {
                TimeSpan oSpan = DateTime.Parse(lblApprovedS.Text).Subtract(DateTime.Parse(lblActualS.Text));
                lblVarianceS.Text = oSpan.Days.ToString();
            }
            else
                lblVarianceS.Text = "---";
            if (lblApprovedF.Text != "---" && lblActualF.Text != "---")
            {
                TimeSpan oSpan = DateTime.Parse(lblApprovedF.Text).Subtract(DateTime.Parse(lblActualF.Text));
                lblVarianceF.Text = oSpan.Days.ToString();
            }
            else
                lblVarianceF.Text = "---";
            if (lblEndDate.Text == "")
                lblEndDate.Text = "<i>Pending</i>";
            bool boolDiv = false;
            if (Request.QueryString["div"] != null)
            {
                hdnTab.Value = Request.QueryString["div"];
                switch (Request.QueryString["div"])
                {
                    case "1":
                        boolStep1 = true;
                        boolDiv = true;
                        break;
                    case "2":
                        boolStep2 = true;
                        boolDiv = true;
                        break;
                    case "3":
                        boolStep3 = true;
                        boolDiv = true;
                        break;
                    case "4":
                        boolStep4 = true;
                        boolDiv = true;
                        break;
                    case "5":
                        boolStep5 = true;
                        boolDiv = true;
                        break;
                    case "S":
                        boolStatus = true;
                        boolDiv = true;
                        break;
                    case "D":
                        boolDocuments = true;
                        boolDiv = true;
                        break;
                    case "M":
                        boolMyDocuments = true;
                        boolDiv = true;
                        break;
                    case "R":
                        boolResource = true;
                        boolDiv = true;
                        break;
                    case "L":
                        boolMilestones = true;
                        boolDiv = true;
                        break;
                }
            }

            // Load Phases according to what is available
            bool boolUnlock = (oProject.Get(intProject, "number") != "");
            bool boolDiscovery = (boolUnlock == true && (dsRequest.Tables[0].Rows[0]["d_done"].ToString() == ""));
            bool boolPlanning = (boolUnlock == true && boolDiscovery == false && (dsRequest.Tables[0].Rows[0]["p_done"].ToString() == ""));
            bool boolExecution = (boolUnlock == true && boolPlanning == false && (dsRequest.Tables[0].Rows[0]["c_done"].ToString() == ""));
            bool boolClosing = (boolUnlock == true && boolExecution == false && (dsRequest.Tables[0].Rows[0]["c_done"].ToString() == ""));
            if (boolUnlock == false)
            {
                if (boolDiv == false)
                    boolStep1 = true;
                lblNumber.Text = "<img src=\"/images/alert.gif\" border=\"0\" align=\"absmiddle\" /> <b>Please enter a clarity number!</b>";
                lblNumber.CssClass = "reddefault";
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "unlock", "<script type=\"text/javascript\">alert('This project is currently locked!\\n\\nTo unlock this project, please enter the number of hours required for the Discovery phase, and a valid clarity / project number.');<" + "/" + "script>");
                txtName.Enabled = false;
                ddlBaseDisc.Enabled = false;
                txtWorking.Enabled = false;
                txtExecutive.Enabled = false;
                ddlPortfolio.Enabled = false;
                txtDescription.Enabled = false;
                txtStartDate.Enabled = false;
                imgStart.Enabled = false;
                lstCostsAvailable.Enabled = false;
                lstCostsCurrent.Enabled = false;
                btnCostAdd.Enabled = false;
                btnCostRemove.Enabled = false;
                btnDocuments.Enabled = false;
                btnComplete.ImageUrl = "/images/tool_complete_dbl.gif";
                btnComplete.Enabled = false;
                btnHold.Enabled = false;
                btnPCRCSRC.Enabled = false;
                hdnPhase.Value = "1";
            }
            else if (boolDiscovery == true)
            {
                if (boolDiv == false)
                    boolStep2 = true;
                hdnPhase.Value = "2";
                sldDiscovery._Enabled = true;
                txtPlanning.Enabled = true;
                txtActDS.Enabled = true;
                txtActDF.Enabled = true;
                txtActDI.Enabled = true;
                txtActDE.Enabled = true;
                txtActDH.Enabled = true;
                imgtxtActDS.Enabled = true;
                imgtxtActDF.Enabled = true;
                imgtxtActDI.Enabled = true;
                imgtxtActDE.Enabled = true;
                imgtxtActDH.Enabled = true;
                btnComplete.ImageUrl = "/images/tool_complete_dbl.gif";
                btnComplete.Enabled = false;
            }
            else if (boolPlanning == true)
            {
                if (boolDiv == false)
                    boolStep3 = true;
                hdnPhase.Value = "3";
                sldPlanning._Enabled = true;
                txtExecution.Enabled = true;
                txtAppPS.Enabled = true;
                txtAppPF.Enabled = true;
                txtActPS.Enabled = true;
                txtActPF.Enabled = true;
                txtAppPI.Enabled = true;
                txtAppPE.Enabled = true;
                txtAppPH.Enabled = true;
                txtActPI.Enabled = true;
                txtActPE.Enabled = true;
                txtActPH.Enabled = true;
                imgtxtAppPS.Enabled = true;
                imgtxtAppPF.Enabled = true;
                imgtxtActPS.Enabled = true;
                imgtxtActPF.Enabled = true;
                imgtxtAppPI.Enabled = true;
                imgtxtAppPE.Enabled = true;
                imgtxtAppPH.Enabled = true;
                imgtxtActPI.Enabled = true;
                imgtxtActPE.Enabled = true;
                imgtxtActPH.Enabled = true;
                btnComplete.ImageUrl = "/images/tool_complete_dbl.gif";
                btnComplete.Enabled = false;
            }
            else if (boolExecution == true)
            {
                if (boolDiv == false)
                    boolStep4 = true;
                hdnPhase.Value = "4";
                sldExecution._Enabled = true;
                txtClosing.Enabled = true;
                txtAppES.Enabled = true;
                txtAppEF.Enabled = true;
                txtActES.Enabled = true;
                txtActEF.Enabled = true;
                txtAppEI.Enabled = true;
                txtAppEE.Enabled = true;
                txtAppEH.Enabled = true;
                txtActEI.Enabled = true;
                txtActEE.Enabled = true;
                txtActEH.Enabled = true;
                imgtxtAppES.Enabled = true;
                imgtxtAppEF.Enabled = true;
                imgtxtActES.Enabled = true;
                imgtxtActEF.Enabled = true;
                imgtxtAppEI.Enabled = true;
                imgtxtAppEE.Enabled = true;
                imgtxtAppEH.Enabled = true;
                imgtxtActEI.Enabled = true;
                imgtxtActEE.Enabled = true;
                imgtxtActEH.Enabled = true;
                btnComplete.ImageUrl = "/images/tool_complete_dbl.gif";
                btnComplete.Enabled = false;
            }
            else if (boolClosing == true)
            {
                if (boolDiv == false)
                    boolStep5 = true;
                hdnPhase.Value = "5";
                sldClosing._Enabled = true;
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
                    lblComplete.Text = oFunction.CreateBox("box_", "<img src=\"/images/alert.gif\" border=\"0\" align=\"absmiddle\" /> Resources have not completed this project", "error");
                }
            }
        }
        protected void btnSave_Click(Object Sender, EventArgs e)
        {
            int intResourceWorkflow = Int32.Parse(lblResourceWorkflow.Text);
            int intResourceParent = oResourceRequest.GetWorkflowParent(intResourceWorkflow);
            double dblUsed = 0.00;
            double dblAllocated = 0.00;
            string strCompleted = "";
            oResourceRequest.UpdateWorkflowName(intResourceWorkflow, txtCustom.Text, intProfile);
            UpdateProject();
            switch (Request.Form[hdnTab.UniqueID])
            {
                case "1":
                    oTPM.Unlock(intRequest, intItem, intNumber, txtDiscovery.Text, Request.Form[hdnCosts.UniqueID], txtStartDate.Text, intProfile);
                    Response.Redirect(oPage.GetFullLink(intPage) + "?pid=" + intProject.ToString() + "&div=2&save=true");
                    break;
                case "2":
                    dblUsed = 0.00;
                    dblAllocated = double.Parse(lblDiscoveryHRs.Text);
                    if (Request.Form["hdnDiscovery"] != null && Request.Form["hdnDiscovery"] != "")
                        dblUsed = double.Parse(Request.Form["hdnDiscovery"]);
                    strCompleted = (dblAllocated == dblUsed ? DateTime.Now.ToString() : "");
                    oTPM.UpdateHours(intRequest, intItem, intNumber, "D", dblUsed);
                    oTPM.Discovery(intRequest, intItem, intNumber, "", "", "", txtActDI.Text, txtActDE.Text, txtActDH.Text, "", "", "", "", "", txtActDS.Text, txtActDF.Text, strCompleted, txtPlanning.Text, intProfile);
                    if (strCompleted == "")
                        Response.Redirect(oPage.GetFullLink(intPage) + "?pid=" + intProject.ToString() + "&div=2&save=true");
                    else
                        Response.Redirect(oPage.GetFullLink(intPage) + "?pid=" + intProject.ToString() + "&div=3&save=true");
                    break;
                case "3":
                    dblUsed = 0.00;
                    dblAllocated = double.Parse(lblPlanningHRs.Text);
                    if (Request.Form["hdnPlanning"] != null && Request.Form["hdnPlanning"] != "")
                        dblUsed = double.Parse(Request.Form["hdnPlanning"]);
                    strCompleted = (dblAllocated == dblUsed ? DateTime.Now.ToString() : "");
                    oTPM.UpdateHours(intRequest, intItem, intNumber, "P", dblUsed);
                    oTPM.Planning(intRequest, intItem, intNumber, txtAppPI.Text, txtAppPE.Text, txtAppPH.Text, txtActPI.Text, txtActPE.Text, txtActPH.Text, "", "", "", txtAppPS.Text, txtAppPF.Text, txtActPS.Text, txtActPF.Text, strCompleted, txtExecution.Text, intProfile);
                    if (strCompleted == "")
                        Response.Redirect(oPage.GetFullLink(intPage) + "?pid=" + intProject.ToString() + "&div=3&save=true");
                    else
                        Response.Redirect(oPage.GetFullLink(intPage) + "?pid=" + intProject.ToString() + "&div=4&save=true");
                    break;
                case "4":
                    dblUsed = 0.00;
                    dblAllocated = double.Parse(lblExecutionHRs.Text);
                    if (Request.Form["hdnExecution"] != null && Request.Form["hdnExecution"] != "")
                        dblUsed = double.Parse(Request.Form["hdnExecution"]);
                    strCompleted = (dblAllocated == dblUsed ? DateTime.Now.ToString() : "");
                    oTPM.UpdateHours(intRequest, intItem, intNumber, "E", dblUsed);
                    oTPM.Execution(intRequest, intItem, intNumber, txtAppEI.Text, txtAppEE.Text, txtAppEH.Text, txtActEI.Text, txtActEE.Text, txtActEH.Text, "", "", "", txtAppES.Text, txtAppEF.Text, txtActES.Text, txtActEF.Text, strCompleted, txtClosing.Text, intProfile);
                    if (strCompleted == "")
                        Response.Redirect(oPage.GetFullLink(intPage) + "?pid=" + intProject.ToString() + "&div=4&save=true");
                    else
                        Response.Redirect(oPage.GetFullLink(intPage) + "?pid=" + intProject.ToString() + "&div=5&save=true");
                    break;
                case "5":
                    dblUsed = 0.00;
                    dblAllocated = double.Parse(lblClosingHRs.Text);
                    if (Request.Form["hdnClosing"] != null && Request.Form["hdnClosing"] != "")
                        dblUsed = double.Parse(Request.Form["hdnClosing"]);
                    strCompleted = (dblAllocated == dblUsed ? DateTime.Now.ToString() : "");
                    oTPM.UpdateHours(intRequest, intItem, intNumber, "C", dblUsed);
                    oTPM.Closing(intRequest, intItem, intNumber, "", "", "", "", "", "", "", "", "", "", "", "", "", strCompleted, txtBetter.Text, txtWorse.Text, txtLessons.Text, intProfile);
                    Response.Redirect(oPage.GetFullLink(intPage) + "?pid=" + intProject.ToString() + "&div=5&save=true");
                    break;
                case "S":
                    oResourceRequest.AddStatusTPM(intResourceWorkflow, Int32.Parse(ddlScope.SelectedItem.Value), Int32.Parse(ddlTimeline.SelectedItem.Value), Int32.Parse(ddlBudget.SelectedItem.Value), DateTime.Parse(txtStatusDate.Text), txtComments.Text, txtThisWeek.Text, txtNextWeek.Text);
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
            oProject.Update(intProject, txtName.Text, ddlBaseDisc.SelectedItem.Value, txtNumber.Text, intUser, intNewPortfolio, intNewSegment, intNewStatus);
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
        protected void CloseProject(bool _done)
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
                oClarityFile.WriteLine("<tr><td>Project Start Date</td><td>" + txtStartDate.Text + "</td><td>Properties - General tab</td></tr>");
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
                oClarityFile.WriteLine("Project Start Date ................ " + txtStartDate.Text);
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