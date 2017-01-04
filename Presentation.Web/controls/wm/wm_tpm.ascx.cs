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
using iTextSharp.text;
using System.IO;
using iTextSharp.text.pdf;
namespace NCC.ClearView.Presentation.Web
{
    public partial class wm_tpm : System.Web.UI.UserControl, ICallbackEventHandler
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
        protected int intCSRCPage = Int32.Parse(ConfigurationManager.AppSettings["CSRC_WORKFLOW"]);
        protected int intPCRPage = Int32.Parse(ConfigurationManager.AppSettings["PCR_WORKFLOW"]);
        protected int intProfile;
        protected Projects oProject;
        protected Functions oFunction;
        protected Users oUser;
        protected Pages oPage;
        protected ResourceRequest oResourceRequest;
        protected RequestItems oRequestItem;
        protected Requests oRequest;
        protected StatusLevels oStatus;
        protected ServiceRequests oServiceRequest;
        protected Applications oApplication;
        protected Documents oDocument;
        protected RequestFields oRequestField;
        protected ProjectRequest oProjectRequest;
        protected Variables oVariable;
        protected TPM oTPM;
        protected Customized oCustomized;
        protected Delegates oDelegate;
        protected Segment oSegment;
        protected Services oService;
        protected Costs oCost;
        protected int intApplication = 0;
        protected int intPage = 0;
        protected int intProject = 0;
        protected bool boolDetails = false;
        protected bool boolFinancials = false;
        protected bool boolStatus = false;
        protected bool boolInformation = false;
        protected bool boolClose = false;
        protected bool boolResource = false;
        protected bool boolMilestones = false;
        protected int intRequest = 0;
        protected int intItem = 0;
        protected int intNumber = 0;
        protected int intService = 0;
        protected int intLead = 0;
        protected string strCSRC = "";
        protected string strPCR = "";
        protected int intWorking = 0;
        protected int intExecutive = 0;
        protected bool boolNotifyChange = false;
        protected int intImplementorDistributed = Int32.Parse(ConfigurationManager.AppSettings["ITEMID_IMPLEMENTOR_DISTRIBUTED"]);
        protected int intImplementorMidrange = Int32.Parse(ConfigurationManager.AppSettings["ITEMID_IMPLEMENTOR_MIDRANGE"]);
        private Variables oVariables;

        protected string strResult = "";
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
            oStatus = new StatusLevels();
            oServiceRequest = new ServiceRequests(intProfile, dsn);
            oApplication = new Applications(intProfile, dsn);
            oDocument = new Documents(intProfile, dsn);
            oRequestField = new RequestFields(intProfile, dsn);
            oProjectRequest = new ProjectRequest(intProfile, dsn);
            oVariable = new Variables(intEnvironment);
            oTPM = new TPM(intProfile, dsn, intEnvironment);
            oCustomized = new Customized(intProfile, dsn);
            oDelegate = new Delegates(intProfile, dsn);
            oSegment = new Segment(intProfile, dsn);
            oService = new Services(intProfile, dsn);
            oCost = new Costs(intProfile, dsn);
            oVariables = new Variables(intEnvironment);
            bool boolDebug = false;

            if (Request.QueryString["applicationid"] != null && Request.QueryString["applicationid"] != "")
                intApplication = Int32.Parse(Request.QueryString["applicationid"]);
            if (Request.QueryString["pageid"] != null && Request.QueryString["pageid"] != "")
                intPage = Int32.Parse(Request.QueryString["pageid"]);
            if (Request.Cookies["application"] != null && Request.Cookies["application"].Value != "")
                intApplication = Int32.Parse(Request.Cookies["application"].Value);
            if (Request.QueryString["action"] != null)
                panFinish.Visible = true;
            else
            {
                int intResourceWorkflow = 0;
                int intResourceParent = 0;
                int intUser = 0;
                int intSolo = 0;
                int intStatus2 = 0;
                if (Request.QueryString["rrid"] != null && Request.QueryString["rrid"] != "")
                {
                    intResourceWorkflow = Int32.Parse(Request.QueryString["rrid"]);
                    intResourceParent = oResourceRequest.GetWorkflowParent(intResourceWorkflow);
                    ds = oResourceRequest.Get(intResourceParent);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        intRequest = Int32.Parse(ds.Tables[0].Rows[0]["requestid"].ToString());
                        intService = Int32.Parse(ds.Tables[0].Rows[0]["serviceid"].ToString());
                        intNumber = Int32.Parse(ds.Tables[0].Rows[0]["number"].ToString());
                        intProject = oRequest.GetProjectNumber(intRequest);
                        intUser = Int32.Parse(oResourceRequest.GetWorkflow(intResourceWorkflow, "userid"));
                        intItem = Int32.Parse(ds.Tables[0].Rows[0]["itemid"].ToString());
                        int intApp = oRequestItem.GetItemApplication(intItem);
                        if (boolDebug == true)
                        {
                            Response.Write((intProfile == intUser) + "<br/>");
                            Response.Write("Service: " + intService.ToString() + "<br/>");
                            Response.Write("Profile: " + intProfile.ToString() + "<br/>");
                            Response.Write(oService.IsManager(intService, intProfile) + "<br/>");
                            Response.Write((oDelegate.Get(intUser, intProfile) > 0) + "<br/>");
                            Response.Write((oApplication.IsManager(intApp, intProfile) && oApplication.Get(intApp, "disable_manager") != "1") + "<br/>");
                        }
                        if (intProfile == intUser || oService.IsManager(intService, intProfile) || oDelegate.Get(intUser, intProfile) > 0 || (oApplication.IsManager(intApp, intProfile) && oApplication.Get(intApp, "disable_manager") != "1") || (intUser > 0 && oUser.IsManager(intUser, intProfile, true) && oApplication.Get(intApp, "disable_manager") != "1") || (oApplication.Get(intApp, "tpm") == "1" && oApplication.IsManager(intApp, oUser.GetManager(intProfile, true))))
                        {
                            intRequest = Int32.Parse(ds.Tables[0].Rows[0]["requestid"].ToString());
                            intSolo = Int32.Parse(ds.Tables[0].Rows[0]["solo"].ToString());
                            intStatus2 = Int32.Parse(ds.Tables[0].Rows[0]["status"].ToString());
                            intLead = oUser.GetManager(intProfile, true);
                        }
                        else
                            intResourceWorkflow = 0;
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
                        intUser = Int32.Parse(dr["userid"].ToString());
                        intItem = Int32.Parse(dr["itemid"].ToString());
                        int intApp = oRequestItem.GetItemApplication(intItem);
                        if (boolDebug == true)
                        {
                            Response.Write((intProfile == intUser) + "<br/>");
                            Response.Write(oService.IsManager(intService, intProfile) + "<br/>");
                            Response.Write((oDelegate.Get(intUser, intProfile) > 0) + "<br/>");
                            Response.Write((oApplication.IsManager(intApp, intProfile) && oApplication.Get(intApp, "disable_manager") != "1") + "<br/>");
                            Response.Write((intUser > 0 && oUser.IsManager(intUser, intProfile, true) && oApplication.Get(intApp, "disable_manager") != "1") + "<br/>");
                        }
                        if (intProfile == intUser || oService.IsManager(intService, intProfile) || oDelegate.Get(intUser, intProfile) > 0 || (oApplication.IsManager(intApp, intProfile) && oApplication.Get(intApp, "disable_manager") != "1") || (intUser > 0 && oUser.IsManager(intUser, intProfile, true) && oApplication.Get(intApp, "disable_manager") != "1"))
                        {
                            hdnID.Value = dr["id"].ToString();
                            intResourceWorkflow = Int32.Parse(dr["id"].ToString());
                            intResourceParent = oResourceRequest.GetWorkflowParent(intResourceWorkflow);
                            intRequest = Int32.Parse(dr["requestid"].ToString());
                            intNumber = Int32.Parse(dr["number"].ToString());
                            intService = Int32.Parse(dr["serviceid"].ToString());
                            intUser = Int32.Parse(dr["userid"].ToString());
                            intSolo = Int32.Parse(dr["solo"].ToString());
                            intStatus2 = Int32.Parse(dr["status"].ToString());
                            intLead = oUser.GetManager(intProfile, true);
                            //                    Response.Write("intProfile: " + intProfile.ToString() + "<br/>");
                            //                    Response.Write("intUser: " + intUser.ToString() + "<br/>");
                            //                    Response.Write("intProfile == intUser: " + (intProfile == intUser) + "<br/>");
                            //                    Response.Write("intItemManager: " + intItemManager.ToString() + "<br/>");
                            //                    Response.Write("intProfile == intItemManager: " + (intProfile == intItemManager) + "<br/>");
                            //                    Response.Write("intAppManager: " + intAppManager.ToString() + "<br/>");
                            //                    Response.Write("intProfile == intAppManager && oApplication.Get(intApp, \"disable_manager\") != \"1\": " + (intProfile == intAppManager && oApplication.Get(intApp, "disable_manager") != "1") + "<br/>");
                            //                    Response.Write("intUser > 0 && oUser.IsManager(intUser, intProfile, true) && oApplication.Get(intApp, \"disable_manager\") != \"1\": " + (intUser > 0 && oUser.IsManager(intUser, intProfile, true) && oApplication.Get(intApp, "disable_manager") != "1") + "<br/>");
                            //                    Response.Write("oApplication.Get(intApp, \"tpm\") == \"1\" && oUser.GetManager(intProfile, true) == oApplication.GetManager(intApp): " + (oApplication.Get(intApp, "tpm") == "1" && oUser.GetManager(intProfile, true) == oApplication.GetManager(intApp)) + "<br/>");
                            //                    Response.Write("Resource: " + intResource.ToString() + "<br/>");
                            break;
                        }
                    }
                }
                if (intResourceWorkflow > 0)
                {
                    lblResourceWorkflow.Text = intResourceWorkflow.ToString();
                    // Retrieve Previous Week
                    if (Request.QueryString["retrieve"] != null)
                    {
                        DataSet dsRetrieve = oResourceRequest.GetStatussTPM(intResourceWorkflow);
                        if (dsRetrieve.Tables[0].Rows.Count > 0)
                        {
                            ddlScope.SelectedValue = dsRetrieve.Tables[0].Rows[0]["scope"].ToString();
                            ddlTimeline.SelectedValue = dsRetrieve.Tables[0].Rows[0]["timeline"].ToString();
                            ddlBudget.SelectedValue = dsRetrieve.Tables[0].Rows[0]["budget"].ToString();
                            txtStatusDate.Text = DateTime.Parse(dsRetrieve.Tables[0].Rows[0]["datestamp"].ToString()).ToShortDateString();
                            txtComments.Text = dsRetrieve.Tables[0].Rows[0]["comments"].ToString();
                            txtThisWeek.Text = dsRetrieve.Tables[0].Rows[0]["thisweek"].ToString();
                            txtNextWeek.Text = dsRetrieve.Tables[0].Rows[0]["nextweek"].ToString();
                        }
                    }
                    // Documents
                    btnDocuments.Attributes.Add("onclick", "return OpenWindow('DOCUMENTS_SECURE','?pid=" + intProject.ToString() + "');");
                    chkMyDescription.Checked = (Request.QueryString["mydoc"] != null);
                    lblMyDocuments.Text = oDocument.GetDocuments_Mine(intProject, intProfile, oVariables.DocumentsFolder(), -1, (Request.QueryString["mydoc"] != null));
                    // GetDocuments(string _physical, int _projectid, int _requestid, int _userid, int _security, bool _show_description, bool _mine)
                    //lblMyDocuments.Text = oDocument.GetDocuments(Request.PhysicalApplicationPath, (intProject > 0 ? intProject : 0), (intProject > 0 ? 0 : intRequest), intProfile, -1, (Request.QueryString["mydoc"] != null), true);
                    chkDescription.Checked = (Request.QueryString["doc"] != null);
                    lblDocuments.Text = oDocument.GetDocuments_Project(intProject, intProfile, oVariables.DocumentsFolder(), 1, (Request.QueryString["doc"] != null));
                    // GetDocuments(string _physical, int _projectid, int _requestid, int _userid, int _security, bool _show_description, bool _mine)
                    //lblDocuments.Text = oDocument.GetDocuments(Request.PhysicalApplicationPath, (intProject > 0 ? intProject : 0), (intProject > 0 ? 0 : intRequest), intProfile, 1, (Request.QueryString["doc"] != null), false);
                    if (intProject > 0)
                    {
                        intWorking = Int32.Parse(oProject.Get(intProject, "working"));
                        intExecutive = Int32.Parse(oProject.Get(intProject, "executive"));
                    }
                    if (intSolo.ToString() == "0")
                    {
                        lblSubmitted.Text = DateTime.Parse(oProjectRequest.Get(intRequest, "created")).ToShortDateString();
                        btnView.Text = "View Original Project Request";
                        btnView.Attributes.Add("onclick", "return OpenWindow('PRINTER_FRIENDLY','?page=" + intViewRequest.ToString() + "&rid=" + intRequest.ToString() + "');");
                    }
                    else
                    {
                        lblSubmitted.Text = DateTime.Parse(oRequest.Get(intRequest, "modified")).ToShortDateString();
                        btnView.Text = "View Original Request Details";
                        btnView.Attributes.Add("onclick", "return OpenWindow('PRINTER_FRIENDLY','?page=" + intViewResourceRequest.ToString() + "&rrid=" + intResourceParent.ToString() + "');");
                    }
                    if (intWorking > 0)
                    {
                        txtWorking.Text = oUser.GetFullName(intWorking) + " (" + oUser.GetName(intWorking) + ")";
                        //lblCSRCW.Text = oUser.GetFullName(intWorking);
                        //lblPCRW.Text = oUser.GetFullName(intWorking);                    
                        lblWorking.Text = oUser.GetFullName(intWorking);
                    }
                    if (intExecutive > 0)
                    {
                        txtExecutive.Text = oUser.GetFullName(intExecutive) + " (" + oUser.GetName(intExecutive) + ")";
                        //lblCSRCE.Text = oUser.GetFullName(intExecutive);
                        //lblPCRE.Text = oUser.GetFullName(intExecutive);                 
                        lblExecutive.Text = oUser.GetFullName(intExecutive);
                    }
                    if (intStatus2 > -1 && intStatus2 < 3)
                    {
                        DataSet dsResources = oResourceRequest.GetWorkflowProjectAll(intProject);
                        bool boolClose = true;
                        int intOpen = 0;
                        foreach (DataRow drResource in dsResources.Tables[0].Rows)
                        {
                            
                            if (Int32.Parse(drResource["id"].ToString()) != intResourceWorkflow && Int32.Parse(drResource["userid"].ToString()) > 0 && Int32.Parse(drResource["status"].ToString()) < 3 && Int32.Parse(drResource["status"].ToString()) > 0 && intImplementorDistributed != Int32.Parse(drResource["itemid"].ToString()) && intImplementorMidrange != Int32.Parse(drResource["itemid"].ToString()))
                            {
                                intOpen = Int32.Parse(drResource["id"].ToString());
                                
                                boolClose = false;
                                break;
                            }
                        }
                        if (boolClose == true)
                        {
                            oFunction.ConfigureToolButton(btnComplete, "/images/tool_complete");
                            btnComplete.Attributes.Add("onclick", "return confirm('Are you sure you want to mark this as completed and remove it from your workload?');");
                        }
                        else
                        {
                            oFunction.ConfigureToolButton(btnComplete, "/images/tool_complete");
                            btnComplete.Attributes.Add("onclick", "alert('You cannot complete this project until all assigned resources are completed.\\n\\nFor a list of assigned resources, click on the Resource Involvement tab\\n\\nRESOURCEID: " + intOpen.ToString() + "');return false;");
                        }
                    }
                    else
                    {
                        btnComplete.ImageUrl = "/images/tool_complete_dbl.gif";
                        btnComplete.Enabled = false;
                    }
                    oFunction.ConfigureToolButton(btnSave, "/images/tool_save");
                    oFunction.ConfigureToolButton(btnPrint, "/images/tool_print");
                    btnPrint.Attributes.Add("onclick", "return PrintWindow();");
                    oFunction.ConfigureToolButton(btnHold, "/images/tool_hold");
                    btnHold.Attributes.Add("onclick", "return confirm('NOTE: By cancelling this project, all open requests will automatically be put on hold and the technicians will be notified.\\n\\nAre you sure you want to put this project on HOLD?');");
                    oFunction.ConfigureToolButton(btnCancel, "/images/tool_cancel");
                    btnCancel.Attributes.Add("onclick", "return confirm('NOTE: By cancelling this project, all open requests will automatically be cancelled and the technicians will be notified.\\n\\nAre you sure you want to CANCEL this project?');");
                    if (Request.QueryString["save"] != null && Request.QueryString["save"] != "")
                        Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "saved", "<script type=\"text/javascript\">alert('Information Saved Successfully');<" + "/" + "script>");
                    if (Request.QueryString["hold"] != null && Request.QueryString["hold"] != "")
                        Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "held", "<script type=\"text/javascript\">alert('The project has been put on hold');<" + "/" + "script>");
                    if (Request.QueryString["cancel"] != null && Request.QueryString["cancel"] != "")
                        Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "cancelled", "<script type=\"text/javascript\">alert('The project has been cancelled');<" + "/" + "script>");

                    if (Request.QueryString["mail"] != null && Request.QueryString["mail"] != "")
                        Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "closure_form sent", "<script type=\"text/javascript\">alert('An email has been to the approvers notifying the project closure status');<" + "/" + "script>");
                    hdnTab.Value = "D";
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
                    foreach (System.Web.UI.WebControls.ListItem oItem in ddlResource.Items)
                        oItem.Text = oUser.GetFullName(Int32.Parse(oItem.Value));
                    ddlResource.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- SELECT --", "0"));
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
                        //                    else if (intOldItem == Int32.Parse(dr["itemid"].ToString()) && intOldUser == Int32.Parse(dr["userid"].ToString()))
                        //                        dr.Delete();
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
                        _allocated.Text = dblAllocated.ToString();
                        _used.Text = dblUsed.ToString();
                        if (dblAllocated > 0)
                        {
                            dblUsed = dblUsed / dblAllocated;
                            _percent.Text = dblUsed.ToString("P");
                        }
                        else
                            _percent.Text = dblAllocated.ToString("P");
                        bool boolTPMDone = false;
                        if (intItem2 == 0)
                            _item.Text = "Project Coordinator";
                        else
                        {
                            int intApp = oRequestItem.GetItemApplication(intItem2);
                            _item.Text = oApplication.GetName(intApp);
                        }
                        _status.Text = oStatus.Name(intStatus);
                        if (boolTPMDone == true)
                            _status.Text = "Closed";
                    }
                    // Milestones
                    rptMilestones.DataSource = oResourceRequest.GetMilestones(intResourceWorkflow);
                    rptMilestones.DataBind();
                    lblNoMilestone.Visible = (rptMilestones.Items.Count == 0);
                    LoadStatus(intResourceWorkflow);
                    if (!IsPostBack)
                    {
                        LoadLists();
                        LoadProject(intResourceWorkflow);
                        LoadCSRC();
                        LoadPCR();
                        lblPriority.Text = oProjectRequest.GetPriority(intRequest);
                        switch (oProject.Get(intProject, "status"))
                        {
                            case "-2":
                                lblProjectStatus.Text = "Cancelled";
                                break;
                            case "1":
                                lblProjectStatus.Text = "Approved / Active";
                                break;
                            case "2":
                                lblProjectStatus.Text = "Active";
                                break;
                            case "3":
                                lblProjectStatus.Text = "Completed";
                                break;
                            case "5":
                                lblProjectStatus.Text = "On Hold";
                                break;
                            default:
                                lblProjectStatus.Text = oProject.Get(intProject, "status");
                                break;
                        }
                    }
                    //lblTeamLeadCSRC.Text = oUser.GetFullName(intLead);                 
                    ddlPortfolio.Attributes.Add("onchange", "PopulateSegments('" + ddlPortfolio.ClientID + "','" + ddlSegment.ClientID + "');");
                    ddlSegment.Attributes.Add("onchange", "UpdateDropDownHidden('" + ddlSegment.ClientID + "','" + hdnSegment.ClientID + "');");
                    btnSave.Attributes.Add("onclick", "return ValidateStatusTPM('" + hdnTab.ClientID + "','" + ddlScope.ClientID + "','" + ddlTimeline.ClientID + "','" + ddlBudget.ClientID + "','" + txtStatusDate.ClientID + "','" + txtComments.ClientID + "','" + txtThisWeek.ClientID + "','" + txtNextWeek.ClientID + "') && ValidateMilestone('" + hdnTab.ClientID + "','" + txtMilestoneApproved.ClientID + "','" + txtMilestoneForecasted.ClientID + "','" + txtMilestone.ClientID + "','" + txtDetail.ClientID + "');");
                    //            btnSave.Attributes.Add("onclick", "return ValidateStatusTPM2('" + hdnTab.ClientID + "','" + ddlScope.ClientID + "','" + ddlTimeline.ClientID + "','" + ddlBudget.ClientID + "','" + txtStatusDate.ClientID + "','" + txtComments.ClientID + "','" + txtThisWeek.ClientID + "','" + txtNextWeek.ClientID + "');");
                    btnCostAdd.Attributes.Add("onclick", "return MoveList('" + lstCostsAvailable.ClientID + "','" + lstCostsCurrent.ClientID + "','" + hdnCosts.ClientID + "','" + lstCostsCurrent.ClientID + "');");
                    lstCostsAvailable.Attributes.Add("ondblclick", "return MoveList('" + lstCostsAvailable.ClientID + "','" + lstCostsCurrent.ClientID + "','" + hdnCosts.ClientID + "','" + lstCostsCurrent.ClientID + "');");
                    btnCostRemove.Attributes.Add("onclick", "return MoveList('" + lstCostsCurrent.ClientID + "','" + lstCostsAvailable.ClientID + "','" + hdnCosts.ClientID + "','" + lstCostsCurrent.ClientID + "');");
                    lstCostsCurrent.Attributes.Add("ondblclick", "return MoveList('" + lstCostsCurrent.ClientID + "','" + lstCostsAvailable.ClientID + "','" + hdnCosts.ClientID + "','" + lstCostsCurrent.ClientID + "');");
                    // Dates
                    imgtxtStartD.Attributes.Add("onclick", "return ShowCalendar('" + txtStartD.ClientID + "');");
                    imgtxtEndD.Attributes.Add("onclick", "return ShowCalendar('" + txtEndD.ClientID + "');");
                    imgtxtStartP.Attributes.Add("onclick", "return ShowCalendar('" + txtStartP.ClientID + "');");
                    imgtxtEndP.Attributes.Add("onclick", "return ShowCalendar('" + txtEndP.ClientID + "');");
                    imgtxtStartE.Attributes.Add("onclick", "return ShowCalendar('" + txtStartE.ClientID + "');");
                    imgtxtEndE.Attributes.Add("onclick", "return ShowCalendar('" + txtEndE.ClientID + "');");
                    imgtxtStartC.Attributes.Add("onclick", "return ShowCalendar('" + txtStartC.ClientID + "');");
                    imgtxtEndC.Attributes.Add("onclick", "return ShowCalendar('" + txtEndC.ClientID + "');");
                    imgtxtAppSD.Attributes.Add("onclick", "return ShowCalendar('" + txtAppSD.ClientID + "');");
                    imgtxtAppED.Attributes.Add("onclick", "return ShowCalendar('" + txtAppED.ClientID + "');");
                    imgtxtAppSP.Attributes.Add("onclick", "return ShowCalendar('" + txtAppSP.ClientID + "');");
                    imgtxtAppEP.Attributes.Add("onclick", "return ShowCalendar('" + txtAppEP.ClientID + "');");
                    imgtxtAppSE.Attributes.Add("onclick", "return ShowCalendar('" + txtAppSE.ClientID + "');");
                    imgtxtAppEE.Attributes.Add("onclick", "return ShowCalendar('" + txtAppEE.ClientID + "');");
                    imgtxtAppSC.Attributes.Add("onclick", "return ShowCalendar('" + txtAppSC.ClientID + "');");
                    imgtxtAppEC.Attributes.Add("onclick", "return ShowCalendar('" + txtAppEC.ClientID + "');");
                    // Calculators
                    imgtxtAppID.Attributes.Add("onclick", "return ShowCalculator('" + txtAppID.ClientID + "');");
                    imgtxtAppExD.Attributes.Add("onclick", "return ShowCalculator('" + txtAppExD.ClientID + "');");
                    imgtxtAppHD.Attributes.Add("onclick", "return ShowCalculator('" + txtAppHD.ClientID + "');");
                    imgtxtActID.Attributes.Add("onclick", "return ShowCalculator('" + txtActID.ClientID + "');");
                    imgtxtActED.Attributes.Add("onclick", "return ShowCalculator('" + txtActED.ClientID + "');");
                    imgtxtActHD.Attributes.Add("onclick", "return ShowCalculator('" + txtActHD.ClientID + "');");
                    imgtxtEstID.Attributes.Add("onclick", "return ShowCalculator('" + txtEstID.ClientID + "');");
                    imgtxtEstED.Attributes.Add("onclick", "return ShowCalculator('" + txtEstED.ClientID + "');");
                    imgtxtEstHD.Attributes.Add("onclick", "return ShowCalculator('" + txtEstHD.ClientID + "');");
                    imgtxtAppIP.Attributes.Add("onclick", "return ShowCalculator('" + txtAppIP.ClientID + "');");
                    imgtxtAppExp.Attributes.Add("onclick", "return ShowCalculator('" + txtAppExp.ClientID + "');");
                    imgtxtAppHP.Attributes.Add("onclick", "return ShowCalculator('" + txtAppHP.ClientID + "');");
                    imgtxtActIP.Attributes.Add("onclick", "return ShowCalculator('" + txtActIP.ClientID + "');");
                    imgtxtActEP.Attributes.Add("onclick", "return ShowCalculator('" + txtActEP.ClientID + "');");
                    imgtxtActHP.Attributes.Add("onclick", "return ShowCalculator('" + txtActHP.ClientID + "');");
                    imgtxtEstIP.Attributes.Add("onclick", "return ShowCalculator('" + txtEstIP.ClientID + "');");
                    imgtxtEstEP.Attributes.Add("onclick", "return ShowCalculator('" + txtEstEP.ClientID + "');");
                    imgtxtEstHP.Attributes.Add("onclick", "return ShowCalculator('" + txtEstHP.ClientID + "');");
                    imgtxtAppIE.Attributes.Add("onclick", "return ShowCalculator('" + txtAppIE.ClientID + "');");
                    imgtxtAppExE.Attributes.Add("onclick", "return ShowCalculator('" + txtAppExE.ClientID + "');");
                    imgtxtAppHE.Attributes.Add("onclick", "return ShowCalculator('" + txtAppHE.ClientID + "');");
                    imgtxtActIE.Attributes.Add("onclick", "return ShowCalculator('" + txtActIE.ClientID + "');");
                    imgtxtActEE.Attributes.Add("onclick", "return ShowCalculator('" + txtActEE.ClientID + "');");
                    imgtxtActHE.Attributes.Add("onclick", "return ShowCalculator('" + txtActHE.ClientID + "');");
                    imgtxtEstIE.Attributes.Add("onclick", "return ShowCalculator('" + txtEstIE.ClientID + "');");
                    imgtxtEstEE.Attributes.Add("onclick", "return ShowCalculator('" + txtEstEE.ClientID + "');");
                    imgtxtEstHE.Attributes.Add("onclick", "return ShowCalculator('" + txtEstHE.ClientID + "');");
                    imgtxtAppIC.Attributes.Add("onclick", "return ShowCalculator('" + txtAppIC.ClientID + "');");
                    imgtxtAppExC.Attributes.Add("onclick", "return ShowCalculator('" + txtAppExC.ClientID + "');");
                    imgtxtAppHC.Attributes.Add("onclick", "return ShowCalculator('" + txtAppHC.ClientID + "');");
                    imgtxtActIC.Attributes.Add("onclick", "return ShowCalculator('" + txtActIC.ClientID + "');");
                    imgtxtActEC.Attributes.Add("onclick", "return ShowCalculator('" + txtActEC.ClientID + "');");
                    imgtxtActHC.Attributes.Add("onclick", "return ShowCalculator('" + txtActHC.ClientID + "');");
                    imgtxtEstIC.Attributes.Add("onclick", "return ShowCalculator('" + txtEstIC.ClientID + "');");
                    imgtxtEstEC.Attributes.Add("onclick", "return ShowCalculator('" + txtEstEC.ClientID + "');");
                    imgtxtEstHC.Attributes.Add("onclick", "return ShowCalculator('" + txtEstHC.ClientID + "');");
                    imgCSRCEC.Attributes.Add("onclick", "return ShowCalendar('" + txtCSRCEC.ClientID + "');");
                    imgCSRCED.Attributes.Add("onclick", "return ShowCalendar('" + txtCSRCED.ClientID + "');");
                    imgCSRCEE.Attributes.Add("onclick", "return ShowCalendar('" + txtCSRCEE.ClientID + "');");
                    imgCSRCEP.Attributes.Add("onclick", "return ShowCalendar('" + txtCSRCEP.ClientID + "');");
                    imgCSRCExC.Attributes.Add("onclick", "return ShowCalculator('" + txtCSRCExC.ClientID + "');");
                    imgCSRCExD.Attributes.Add("onclick", "return ShowCalculator('" + txtCSRCExD.ClientID + "');");
                    imgCSRCExE.Attributes.Add("onclick", "return ShowCalculator('" + txtCSRCExE.ClientID + "');");
                    imgCSRCExP.Attributes.Add("onclick", "return ShowCalculator('" + txtCSRCExP.ClientID + "');");
                    imgCSRCHC.Attributes.Add("onclick", "return ShowCalculator('" + txtCSRCHC.ClientID + "');");
                    imgCSRCHD.Attributes.Add("onclick", "return ShowCalculator('" + txtCSRCHD.ClientID + "');");
                    imgCSRCHE.Attributes.Add("onclick", "return ShowCalculator('" + txtCSRCHE.ClientID + "');");
                    imgCSRCHP.Attributes.Add("onclick", "return ShowCalculator('" + txtCSRCHP.ClientID + "');");
                    imgCSRCIC.Attributes.Add("onclick", "return ShowCalculator('" + txtCSRCIC.ClientID + "');");
                    imgCSRCID.Attributes.Add("onclick", "return ShowCalculator('" + txtCSRCID.ClientID + "');");
                    imgCSRCIE.Attributes.Add("onclick", "return ShowCalculator('" + txtCSRCIE.ClientID + "');");
                    imgCSRCIP.Attributes.Add("onclick", "return ShowCalculator('" + txtCSRCIP.ClientID + "');");
                    imgCSRCSC.Attributes.Add("onclick", "return ShowCalendar('" + txtCSRCSC.ClientID + "');");
                    imgCSRCSD.Attributes.Add("onclick", "return ShowCalendar('" + txtCSRCSD.ClientID + "');");
                    imgCSRCSE.Attributes.Add("onclick", "return ShowCalendar('" + txtCSRCSE.ClientID + "');");
                    imgCSRCSP.Attributes.Add("onclick", "return ShowCalendar('" + txtCSRCSP.ClientID + "');");
                    imgPCRScheduleCE.Attributes.Add("onclick", "return ShowCalendar('" + txtPCRScheduleCE.ClientID + "');");
                    imgPCRScheduleCS.Attributes.Add("onclick", "return ShowCalendar('" + txtPCRScheduleCS.ClientID + "');");
                    imgPCRScheduleDE.Attributes.Add("onclick", "return ShowCalendar('" + txtPCRScheduleDE.ClientID + "');");
                    imgPCRScheduleDS.Attributes.Add("onclick", "return ShowCalendar('" + txtPCRScheduleDS.ClientID + "');");
                    imgPCRScheduleEE.Attributes.Add("onclick", "return ShowCalendar('" + txtPCRScheduleEE.ClientID + "');");
                    imgPCRScheduleES.Attributes.Add("onclick", "return ShowCalendar('" + txtPCRScheduleES.ClientID + "');");
                    imgPCRSchedulePE.Attributes.Add("onclick", "return ShowCalendar('" + txtPCRSchedulePE.ClientID + "');");
                    imgPCRSchedulePS.Attributes.Add("onclick", "return ShowCalendar('" + txtPCRSchedulePS.ClientID + "');");
                    imgPCRFinancialC.Attributes.Add("onclick", "return ShowCalculator('" + txtPCRFinancialC.ClientID + "');");
                    imgPCRFinancialD.Attributes.Add("onclick", "return ShowCalculator('" + txtPCRFinancialD.ClientID + "');");
                    imgPCRFinancialE.Attributes.Add("onclick", "return ShowCalculator('" + txtPCRFinancialE.ClientID + "');");
                    imgPCRFinancialP.Attributes.Add("onclick", "return ShowCalculator('" + txtPCRFinancialP.ClientID + "');");
                    imgStatusDate.Attributes.Add("onclick", "return ShowCalendar('" + txtStatusDate.ClientID + "');");
                    imgMilestoneApproved.Attributes.Add("onclick", "return ShowCalendar('" + txtMilestoneApproved.ClientID + "');");
                    imgMilestoneForecasted.Attributes.Add("onclick", "return ShowCalendar('" + txtMilestoneForecasted.ClientID + "');");
                    txtWorking.Attributes.Add("onkeyup", "return AJAXTextBoxGet(this,'300','195','" + divWorking.ClientID + "','" + lstWorking.ClientID + "','" + hdnWorking.ClientID + "','" + oVariable.URL() + "/frame/users.aspx',2);");
                    lstWorking.Attributes.Add("ondblclick", "AJAXClickRow();");
                    txtExecutive.Attributes.Add("onkeyup", "return AJAXTextBoxGet(this,'300','195','" + divExecutive.ClientID + "','" + lstExecutive.ClientID + "','" + hdnExecutive.ClientID + "','" + oVariable.URL() + "/frame/users.aspx',2);");
                    lstExecutive.Attributes.Add("ondblclick", "AJAXClickRow();");

                    // Vijay Code - Start
                    txtLead.Attributes.Add("onkeyup", "return AJAXTextBoxGet(this,'300','195','" + divLead.ClientID + "','" + lstLead.ClientID + "','" + hdnLead.ClientID + "','" + oVariable.URL() + "/frame/users.aspx',2);");
                    lstLead.Attributes.Add("ondblclick", "return AJAXTextBoxGet(this,'300','195','" + divLead.ClientID + "','" + lstLead.ClientID + "','" + hdnLead.ClientID + "','" + oVariable.URL() + "/frame/users.aspx',2);");
                    // Vijay Code - End

                    //txtCSRCD.Attributes.Add("onkeyup", "return AJAXTextBoxGet(this,'300','195','" + divCSRCD.ClientID + "','" + lstCSRCD.ClientID + "','" + hdnCSRCD.ClientID + "','" + oVariable.URL() + "/frame/users.aspx',2);");
                    //lstCSRCD.Attributes.Add("ondblclick", "AJAXClickRow();");
                    //txtCSRCC.Attributes.Add("onkeyup", "return AJAXTextBoxGet(this,'300','195','" + divCSRCC.ClientID + "','" + lstCSRCC.ClientID + "','" + hdnCSRCC.ClientID + "','" + oVariable.URL() + "/frame/users.aspx',2);");
                    //lstCSRCC.Attributes.Add("ondblclick", "AJAXClickRow();");          

                    chkDiscovery.Attributes.Add("onclick", "CheckCSRC('" + chkDiscovery.ClientID + "','" + chkPlanning.ClientID + "','" + chkExecution.ClientID + "','" + chkClosing.ClientID + "','" + divCSRC.ClientID + "');ShowHideDivCheck('" + divDiscovery.ClientID + "',this);");
                    chkPlanning.Attributes.Add("onclick", "CheckCSRC('" + chkDiscovery.ClientID + "','" + chkPlanning.ClientID + "','" + chkExecution.ClientID + "','" + chkClosing.ClientID + "','" + divCSRC.ClientID + "');ShowHideDivCheck('" + divPlanning.ClientID + "',this);");
                    chkExecution.Attributes.Add("onclick", "CheckCSRC('" + chkDiscovery.ClientID + "','" + chkPlanning.ClientID + "','" + chkExecution.ClientID + "','" + chkClosing.ClientID + "','" + divCSRC.ClientID + "');ShowHideDivCheck('" + divExecution.ClientID + "',this);");
                    chkClosing.Attributes.Add("onclick", "CheckCSRC('" + chkDiscovery.ClientID + "','" + chkPlanning.ClientID + "','" + chkExecution.ClientID + "','" + chkClosing.ClientID + "','" + divCSRC.ClientID + "');ShowHideDivCheck('" + divClosing.ClientID + "',this);");
                    chkPCR.Attributes.Add("onclick", "ShowHideDivCheck('" + divPCR.ClientID + "',this);");
                    chkSchedule.Attributes.Add("onclick", "ShowHideDivCheck('" + divSchedule.ClientID + "',this);");
                    chkFinancial.Attributes.Add("onclick", "ShowHideDivCheck('" + divFinancial.ClientID + "',this);");
                    chkReason.Attributes.Add("onclick", "ShowHideDivCheck('" + divReason.ClientID + "',this);");

                    // Vijay Code - Start

                    txtSupport.Text = "0";
                    txtEPSChargeback.Text = "0";
                    txtSwMaintenance.Text = "0";
                    txtTransCosts.Text = "0";
                    txtFinancialTotal.Text = "0";

                    ClientScriptManager cm = Page.ClientScript;
                    String cbReference = cm.GetCallbackEventReference(this, "arg", "ReceiveServerData", "");
                    String callbackScript = "function CallServer(arg, context) {" + cbReference + "; }";
                    cm.RegisterClientScriptBlock(this.GetType(), "CallServer", callbackScript, true);


                    imgtxtSupport.Attributes.Add("onclick", "return ShowCalculator('" + txtSupport.ClientID + "');");
                    imgtxtEPSChargeback.Attributes.Add("onclick", "return ShowCalculator('" + txtEPSChargeback.ClientID + "');");
                    imgtxtSwMaintenance.Attributes.Add("onclick", "return ShowCalculator('" + txtSwMaintenance.ClientID + "');");
                    imgtxtTransCosts.Attributes.Add("onclick", "return ShowCalculator('" + txtTransCosts.ClientID + "');");

                    btnCalcTotal.Attributes.Add("onclick", "return CalcTotal('" + txtSupport.ClientID + "','" + txtEPSChargeback.ClientID + "','" + txtSwMaintenance.ClientID + "','" + txtTransCosts.ClientID + "','" + txtFinancialTotal.ClientID + "');");
                    btnUpdatePCR.Attributes.Add("onclick", "return ValidateChecks3('" + this.ClientID + "','');");
                    chkScope.Attributes.Add("onclick", "ShowHideDivCheck('" + divScope.ClientID + "',this);");
                    btnSubmitCSRC.Attributes.Add("onclick", "return ValidateText('" + txtCSRCName.ClientID + "','Please enter a Nickname for this CSRC') " +
                       " && ValidateCSRC('" + chkDiscovery.ClientID + "'" +
                        ",'" + chkPlanning.ClientID + "'" +
                        ",'" + chkExecution.ClientID + "'" +
                        ",'" + chkClosing.ClientID + "'" +
                        ",'" + txtCSRCSD.ClientID + "'" +
                        ",'" + txtCSRCED.ClientID + "'" +
                        ",'" + txtCSRCID.ClientID + "'" +
                        ",'" + txtCSRCExD.ClientID + "'" +
                        ",'" + txtCSRCHD.ClientID + "'" +
                        ",'" + txtCSRCSP.ClientID + "'" +
                        ",'" + txtCSRCEP.ClientID + "'" +
                        ",'" + txtCSRCIP.ClientID + "'" +
                        ",'" + txtCSRCExP.ClientID + "'" +
                        ",'" + txtCSRCHP.ClientID + "'" +
                        ",'" + txtCSRCSE.ClientID + "'" +
                        ",'" + txtCSRCEE.ClientID + "'" +
                        ",'" + txtCSRCIE.ClientID + "'" +
                        ",'" + txtCSRCExE.ClientID + "'" +
                        ",'" + txtCSRCHE.ClientID + "'" +
                        ",'" + txtCSRCSC.ClientID + "'" +
                        ",'" + txtCSRCEC.ClientID + "'" +
                        ",'" + txtCSRCIC.ClientID + "'" +
                        ",'" + txtCSRCExC.ClientID + "'" +
                        ",'" + txtCSRCHC.ClientID + "'" +
                        ");");
                    btnSubmitPCR.Attributes.Add("onclick", "return ValidateText('" + txtPCRName.ClientID + "','Please enter a Nickname for this PCR') " +
                      " && ValidatePCR('" + chkScope.ClientID + "'" +
                        ",'" + chkSchedule.ClientID + "'" +
                        ",'" + chkPCRScheduleD.ClientID + "'" +
                        ",'" + txtPCRScheduleDS.ClientID + "'" +
                        ",'" + txtPCRScheduleDE.ClientID + "'" +
                        ",'" + chkPCRScheduleP.ClientID + "'" +
                        ",'" + txtPCRSchedulePS.ClientID + "'" +
                        ",'" + txtPCRSchedulePE.ClientID + "'" +
                        ",'" + chkPCRScheduleE.ClientID + "'" +
                        ",'" + txtPCRScheduleES.ClientID + "'" +
                        ",'" + txtPCRScheduleEE.ClientID + "'" +
                        ",'" + chkPCRScheduleC.ClientID + "'" +
                        ",'" + txtPCRScheduleCS.ClientID + "'" +
                        ",'" + txtPCRScheduleCE.ClientID + "'" +
                        ",'" + chkFinancial.ClientID + "'" +
                        ",'" + chkPCRFinancialD.ClientID + "'" +
                        ",'" + txtPCRFinancialD.ClientID + "'" +
                        ",'" + chkPCRFinancialP.ClientID + "'" +
                        ",'" + txtPCRFinancialP.ClientID + "'" +
                        ",'" + chkPCRFinancialE.ClientID + "'" +
                        ",'" + txtPCRFinancialE.ClientID + "'" +
                        ",'" + chkPCRFinancialC.ClientID + "'" +
                        ",'" + txtPCRFinancialC.ClientID + "'" +
                        ");");

                    rptClosurePDF.DataSource = oTPM.GetProjectClosurePDFs(intRequest, intItem, intNumber);
                    rptClosurePDF.DataBind();
                    lblNoClosurePDF.Visible = rptClosurePDF.Items.Count == 0;

                    img1Week.Attributes.Add("onclick", "return ShowCalendar('" + txtOneWeek.ClientID + "');");
                    imgSurvey.Attributes.Add("onclick", "return ShowCalendar('" + txtSurvey.ClientID + "');"); txtPCRName.Attributes.Add("onchange", "AjaxCheckNickName('PCR','" + txtPCRName.ClientID + "','" + intRequest + "','" + intItem + "','" + intNumber + "');");
                    txtCSRCName.Attributes.Add("onchange", "AjaxCheckNickName('CSRC','" + txtCSRCName.ClientID + "','" + intRequest + "','" + intItem + "','" + intNumber + "');");
                    btnGenerateCloseForm.Attributes.Add("onclick", "return ValidateDate('" + txtOneWeek.ClientID + "','Please select a one week date')");
                    btnAddAcc.Attributes.Add("onclick", "return addAccomplishment('" + txtAccomplishment.ClientID + "');");
                    btnUpdateAcc.Attributes.Add("onclick", "return updateAccomplishment('" + txtAccomplishment.ClientID + "','hdnAccomplishment');");
                    btnRouteClosurePDF.Attributes.Add("onclick", "return ValidateChecks2('chkPDF[]','" + intWorking + "','" + intExecutive + "','hdnClosurePDFCheck');");
                    //  btnPCR.Attributes.Add("onclick", "return OpenWindow('PCR/CSRC_APPROVALS','?test="+hdnCheck.Value+"');");

                    // Vijay Code - End

                }
                else
                    panDenied.Visible = true;
            }
            btnDenied.Attributes.Add("onclick", "return CloseWindow();");
            btnFinish.Attributes.Add("onclick", "return CloseWindow();");
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
            ddlPortfolio.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- SELECT --", "0"));
        }
        private void LoadStatus(int _resourceid)
        {
            DataSet dsStatus = oResourceRequest.GetStatussTPM(_resourceid);
            rptStatus.DataSource = dsStatus;
            rptStatus.DataBind();
            lblNoStatus.Visible = (rptStatus.Items.Count == 0);
            btnRetrieve.Enabled = (rptStatus.Items.Count > 0);
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
                if (dblTotalScope == 0.00)
                {
                    dblTotalScope = dblScope;
                    dblTotalTimeline = dblTimeline;
                    dblTotalBudget = dblBudget;
                }
            }
            lblStatus.Text = oResourceRequest.GetStatus(dblTotalScope, dblTotalTimeline, dblTotalBudget, 83, 15);
        }
        private void LoadCSRC()
        {
            DataSet dsNew = oTPM.GetCSRCs(intRequest, intItem, intNumber);
            int intCount = 0;
            foreach (DataRow dr in dsNew.Tables[0].Rows)
            {
                intCount++;
                string strStatus = "Pending";
                if (dr["status"].ToString() == "1")
                    strStatus = "Approved";
                if (dr["status"].ToString() == "-1")
                    strStatus = "Denied";
                strCSRC += "<tr><td><a href=\"javascript:void(0);\" onclick=\"return OpenWindow('CSRC_FORM','?id=" + dr["id"] + "');\">" + dr["name"] + "</a></td><td><a href=\"" + oVariable.URL() + "/" + dr["path"].ToString() + "\" target=\"_blank\"><img src=\"/images/icons/pdf.gif \" align=\"absmiddle\" border=\"0\"/></a></td><td><a href=\"javascript:void(0);\" onclick=\"return OpenWindow('PCR/CSRC_APPROVALS','?id=" + dr["id"] + "&view=CSRC');\">" + strStatus + "</a></td><td>" + (dr["completed"].ToString() == "" ? "N / A" : DateTime.Parse(dr["completed"].ToString()).ToShortDateString()) + "</td><td><input type=\"checkbox\" name=\"routeCSRC[]\" id=\"chk" + intCount + "\" value=\"" + dr["id"] + "\" onclick=\"UpdateHidden2('routeCSRC[]','hdnCSRCCheck');\"/></td></tr>";
            }
            if (strCSRC == "")
                strCSRC = "<tr><td colspan=\"4\">&nbsp;<img src='/images/alert.gif' border='0' align='absmiddle'> There are no CSRC(s) associated with this project...</td></tr>";

            strCSRC += "<tr><td colspan=\"5\" align=\"right\"><input type=\"button\" class=\"default\" value=\"Route for Approval\" width=\"150\" onclick=\"return ValidateChecks('routeCSRC[]'," + intWorking + "," + intExecutive + ",'hdnCSRCCheck','CSRC'); \"/></td></tr>";
        }
        private void LoadPCR()
        {
            DataSet dsNew = oTPM.GetPCRs(intRequest, intItem, intNumber);
            int intCount = 0;
            foreach (DataRow dr in dsNew.Tables[0].Rows)
            {
                intCount++;
                string strStatus = "Pending";
                if (dr["status"].ToString() == "1")
                    strStatus = "Approved";
                if (dr["status"].ToString() == "-1")
                    strStatus = "Denied";
                string strList = "<select id=\"status" + intCount + "\" class=\"default\" onchange=\"UpdateHidden3(this.id,'hdnStatus','" + dr["id"] + "');\"><option value=\"0\"" + (strStatus == "Pending" ? "selected=\"selected\"" : "") + ">Pending</option><option value=\"1\" " + (strStatus == "Approved" ? "selected=\"selected\"" : "") + ">Approved</option><option value=\"-1\"" + (strStatus == "Denied" ? "selected=\"selected\"" : "") + ">Denied</option></select>";

                strPCR += "<tr><td><a href=\"javascript:void(0);\" onclick=\"return OpenWindow('PCR_FORM','?id=" + dr["id"] + "');\">" + dr["name"] + "</a></td><td><a href=\"" + oVariable.URL() + "/" + dr["path"].ToString() + "\" target=\"_blank\"><img src=\"/images/icons/pdf.gif \" align=\"absmiddle\" border=\"0\"/></a></td><td>" + DateTime.Parse(dr["created"].ToString()) + "</td><td>" + strList + "</td><td>" + (dr["completed"].ToString() == "" ? "N / A" : DateTime.Parse(dr["completed"].ToString()).ToString("MM/dd/yyyy hh:mm:ss tt")) + "</td><td align=\"center\">" + (dr["scope"].ToString() == "0" ? "No" : "Yes") + "</td><td align=\"center\">" + (dr["s"].ToString() == "0" ? "No" : "Yes") + "</td><td align=\"center\">" + (dr["f"].ToString() == "0" ? "No" : "Yes") + "</td><td><input type=\"checkbox\" name=\"routePCR[]\" id=\"chk" + intCount + "\" value=\"" + dr["id"] + "\" onclick=\"UpdateHidden2('routePCR[]','hdnPCRCheck');\" /></td></tr>";
                //strPCR += "<tr><td><a href=\"javascript:void(0);\" onclick=\"return OpenWindow('PCR_FORM','?id=" + dr["id"] + "');\">" + dr["name"] + "</a></td><td><a href=\"" + oVariable.URL() + "/"+ dr["path"].ToString() + "\" target=\"_blank\"><img src=\"/images/icons/pdf.gif \" align=\"absmiddle\" border=\"0\"/></a></td><td><a href=\"javascript:void(0);\" onclick=\"return OpenWindow('PCR/CSRC_APPROVALS','?id="+ dr["id"] +"&view=PCR') ;\">" + strStatus + "</a></td><td>" + (dr["completed"].ToString() == "" ? "N / A" : DateTime.Parse(dr["completed"].ToString()).ToShortDateString()) + "</td><td align=\"center\">" + (dr["scope"].ToString() == "0" ? "No" : "Yes") + "</td><td align=\"center\">" + (dr["s"].ToString() == "0" ? "No" : "Yes") + "</td><td align=\"center\">" + (dr["f"].ToString() == "0" ? "No" : "Yes") + "</td><td><input type=\"checkbox\" name=\"routePCR[]\" id=\"chk" + intCount + "\" value=\"" + dr["id"] + "\" onclick=\"UpdateHidden2('routePCR[]','hdnPCRCheck');\" /></td></tr>";         
            }
            if (strPCR == "")
                strPCR = "<tr><td colspan=\"7\">&nbsp;<img src='/images/alert.gif' border='0' align='absmiddle'>  There are no PCR(s) associated with this project... </td></tr>";

            //   strPCR += "<tr><td colspan=\"8\" align=\"right\"><input type=\"button\" class=\"default\" value=\"Update PCR Status\" width=\"150\" onclick=\"return ValidateChecks3('routePCR[]','hdnPCRCheck'); \"/></td></tr>";        
            //     strPCR += "<tr><td colspan=\"8\" align=\"right\"><input type=\"button\" class=\"default\" value=\"Route for Approval\" width=\"150\" onclick=\"return ValidateChecks('routePCR[]',"+ intWorking +","+ intExecutive +",'hdnPCRCheck','PCR'); \"/></td></tr>";        
        }
        private void LoadProject(int _resource)
        {
            Organizations oOrganization = new Organizations(intProfile, dsn);
            DataSet dsProject = oProject.Get(intProject);
            lblName.Text = oProject.Get(intProject, "name");
            txtProjectName1.Text = lblName.Text;
            txtProjectName2.Text = lblName.Text;
            txtCustomName.Text = oResourceRequest.GetWorkflow(_resource, "name");
            lblNumber.Text = oProject.Get(intProject, "number");
            txtProjectNumber.Text = lblNumber.Text;
            if (intProject > 0)
            {
                int intPortfolio = Int32.Parse(oProject.Get(intProject, "organization"));
                lblPortfolio.Text = oOrganization.GetName(intPortfolio);
                ddlPortfolio.SelectedValue = intPortfolio.ToString();
                if (intPortfolio > 0)
                {
                    int intSegment = Int32.Parse(oProject.Get(intProject, "segmentid"));
                    lblSegment.Text = oSegment.GetName(intSegment);
                    hdnSegment.Value = intSegment.ToString();
                    ddlSegment.Enabled = true;
                    ddlSegment.DataTextField = "name";
                    ddlSegment.DataValueField = "id";
                    ddlSegment.DataSource = oSegment.Gets(intPortfolio, 1);
                    ddlSegment.DataBind();
                    ddlSegment.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- SELECT --", "0"));
                    ddlSegment.SelectedValue = intSegment.ToString();
                }
            }
            else
            {
                lblPortfolio.Text = "<i>None</i>";
                lblSegment.Text = "<i>None</i>";
            }
            lblSubmittedBy.Text = oUser.GetFullName(oRequest.GetUser(intRequest));
            txtDescription.Text = oRequest.Get(intRequest, "description");
            if (oRequest.Get(intRequest, "start_date") != "")
                lblStart.Text = DateTime.Parse(oRequest.Get(intRequest, "start_date")).ToLongDateString();
            else
                lblStart.Text = "N / A";
            if (oRequest.Get(intRequest, "end_date") != "")
                lblEnd.Text = DateTime.Parse(oRequest.Get(intRequest, "end_date")).ToLongDateString();
            else
                lblEnd.Text = "N / A";
            if (oResourceRequest.GetWorkflow(_resource, "modified") != "")
                lblAssigned.Text = DateTime.Parse(oResourceRequest.GetWorkflow(_resource, "modified")).ToLongDateString();
            else
                lblAssigned.Text = "Invalid Modifed for " + _resource.ToString();
            //        txtDescription.Attributes.Add("onkeydown", "return DescriptionDown(this);");
            //        txtDescription.Attributes.Add("onkeyup", "return DescriptionUp(this);");
            ddlStatus.SelectedValue = oProject.Get(intProject, "status"); ;
            DataSet dsResource = oCustomized.GetTPM(intRequest, intItem, intNumber);
            if (dsResource.Tables[0].Rows.Count > 0)
            {
                chkFinancials.Checked = (dsResource.Tables[0].Rows[0]["financials_exclude"].ToString() == "1");
                txtStartD.Text = GetDate(dsResource.Tables[0].Rows[0]["start_d"].ToString());
                lblProjectStart.Text = (txtStartD.Text == "" ? "---" : txtStartD.Text);
                txtEndD.Text = GetDate(dsResource.Tables[0].Rows[0]["end_d"].ToString());
                txtStartP.Text = GetDate(dsResource.Tables[0].Rows[0]["start_p"].ToString());
                txtEndP.Text = GetDate(dsResource.Tables[0].Rows[0]["end_p"].ToString());
                txtStartE.Text = GetDate(dsResource.Tables[0].Rows[0]["start_e"].ToString());
                txtEndE.Text = GetDate(dsResource.Tables[0].Rows[0]["end_e"].ToString());
                txtStartC.Text = GetDate(dsResource.Tables[0].Rows[0]["start_c"].ToString());
                txtEndC.Text = GetDate(dsResource.Tables[0].Rows[0]["end_c"].ToString());
                string strCosts = dsResource.Tables[0].Rows[0]["costs"].ToString();
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
                ddlPPM.SelectedValue = dsResource.Tables[0].Rows[0]["ppm"].ToString();
                txtBetter.Text = dsResource.Tables[0].Rows[0]["better"].ToString();
                txtWorse.Text = dsResource.Tables[0].Rows[0]["worse"].ToString();
                txtLessons.Text = dsResource.Tables[0].Rows[0]["lessons"].ToString();
                // Project Schedule
                txtAppSD.Text = GetDate(dsResource.Tables[0].Rows[0]["appsd"].ToString());
                lblApprovedStart.Text = (txtAppSD.Text == "" ? "---" : txtAppSD.Text);
                txtAppED.Text = GetDate(dsResource.Tables[0].Rows[0]["apped"].ToString());
                txtAppSP.Text = GetDate(dsResource.Tables[0].Rows[0]["appsp"].ToString());
                txtAppEP.Text = GetDate(dsResource.Tables[0].Rows[0]["appep"].ToString());
                txtAppSE.Text = GetDate(dsResource.Tables[0].Rows[0]["appse"].ToString());
                txtAppEE.Text = GetDate(dsResource.Tables[0].Rows[0]["appee"].ToString());
                txtAppSC.Text = GetDate(dsResource.Tables[0].Rows[0]["appsc"].ToString());
                txtAppEC.Text = GetDate(dsResource.Tables[0].Rows[0]["appec"].ToString());
                lblApprovedEnd.Text = (txtAppEC.Text == "" ? "---" : txtAppEC.Text);
                lblEndDate.Text = (txtAppEC.Text == "" ? "<i>Pending</i>" : txtAppEC.Text);
                lblProjectEnd.Text = (txtEndC.Text == "" ? "---" : txtEndC.Text);
                if (txtAppED.Text == "" || txtEndD.Text == "")
                    lblVarianceDaysD.Text = "---";
                else
                {
                    TimeSpan oSpan = DateTime.Parse(txtEndD.Text).Subtract(DateTime.Parse(txtAppED.Text));
                    lblVarianceDaysD.Text = oSpan.Days.ToString();
                }
                if (txtAppSD.Text == "" || txtAppED.Text == "" || txtEndD.Text == "")
                    lblVariancePercentD.Text = "---";
                else
                {
                    TimeSpan oSpan1 = DateTime.Parse(txtEndD.Text).Subtract(DateTime.Parse(txtAppSD.Text));
                    TimeSpan oSpan2 = DateTime.Parse(txtAppED.Text).Subtract(DateTime.Parse(txtAppSD.Text));
                    double dblSpan1 = double.Parse(oSpan1.Days.ToString());
                    double dblSpan2 = double.Parse(oSpan2.Days.ToString());
                    double dblSpan = ((dblSpan1 / dblSpan2) - 1.00) * 100;
                    lblVariancePercentD.Text = dblSpan.ToString("F") + "%";
                }
                if (txtAppEP.Text == "" || txtEndP.Text == "")
                    lblVarianceDaysP.Text = "---";
                else
                {
                    TimeSpan oSpan = DateTime.Parse(txtEndP.Text).Subtract(DateTime.Parse(txtAppEP.Text));
                    lblVarianceDaysP.Text = oSpan.Days.ToString();
                }
                if (txtAppSP.Text == "" || txtAppEP.Text == "" || txtEndP.Text == "")
                    lblVariancePercentP.Text = "---";
                else
                {
                    TimeSpan oSpan1 = DateTime.Parse(txtEndP.Text).Subtract(DateTime.Parse(txtAppSP.Text));
                    TimeSpan oSpan2 = DateTime.Parse(txtAppEP.Text).Subtract(DateTime.Parse(txtAppSP.Text));
                    double dblSpan1 = double.Parse(oSpan1.Days.ToString());
                    double dblSpan2 = double.Parse(oSpan2.Days.ToString());
                    double dblSpan = ((dblSpan1 / dblSpan2) - 1.00) * 100;
                    lblVariancePercentP.Text = dblSpan.ToString("F") + "%";
                }
                if (txtAppEE.Text == "" || txtEndE.Text == "")
                    lblVarianceDaysE.Text = "---";
                else
                {
                    TimeSpan oSpan = DateTime.Parse(txtEndE.Text).Subtract(DateTime.Parse(txtAppEE.Text));
                    lblVarianceDaysE.Text = oSpan.Days.ToString();
                }
                if (txtAppSE.Text == "" || txtAppEE.Text == "" || txtEndE.Text == "")
                    lblVariancePercentE.Text = "---";
                else
                {
                    TimeSpan oSpan1 = DateTime.Parse(txtEndE.Text).Subtract(DateTime.Parse(txtAppSE.Text));
                    TimeSpan oSpan2 = DateTime.Parse(txtAppEE.Text).Subtract(DateTime.Parse(txtAppSE.Text));
                    double dblSpan1 = double.Parse(oSpan1.Days.ToString());
                    double dblSpan2 = double.Parse(oSpan2.Days.ToString());
                    double dblSpan = ((dblSpan1 / dblSpan2) - 1.00) * 100;
                    lblVariancePercentE.Text = dblSpan.ToString("F") + "%";
                }
                if (txtAppEC.Text == "" || txtEndC.Text == "")
                    lblVarianceDaysC.Text = "---";
                else
                {
                    TimeSpan oSpan = DateTime.Parse(txtEndC.Text).Subtract(DateTime.Parse(txtAppEC.Text));
                    lblVarianceDaysC.Text = oSpan.Days.ToString();
                }
                if (txtAppSC.Text == "" || txtAppEC.Text == "" || txtEndC.Text == "")
                    lblVariancePercentC.Text = "---";
                else
                {
                    TimeSpan oSpan1 = DateTime.Parse(txtEndC.Text).Subtract(DateTime.Parse(txtAppSC.Text));
                    TimeSpan oSpan2 = DateTime.Parse(txtAppEC.Text).Subtract(DateTime.Parse(txtAppSC.Text));
                    double dblSpan1 = double.Parse(oSpan1.Days.ToString());
                    double dblSpan2 = double.Parse(oSpan2.Days.ToString());
                    double dblSpan = ((dblSpan1 / dblSpan2) - 1.00) * 100;
                    lblVariancePercentC.Text = dblSpan.ToString("F") + "%";
                }
                if (lblProjectEnd.Text == "---" || lblApprovedEnd.Text == "---")
                    lblVarianceDays.Text = "---";
                else
                {
                    TimeSpan oSpan = DateTime.Parse(lblProjectEnd.Text).Subtract(DateTime.Parse(lblApprovedEnd.Text));
                    lblVarianceDays.Text = oSpan.Days.ToString();
                }
                if (lblApprovedStart.Text == "---" || lblApprovedEnd.Text == "---" || lblProjectEnd.Text == "---")
                    lblVariancePercent.Text = "---";
                else
                {
                    TimeSpan oSpan1 = DateTime.Parse(lblProjectEnd.Text).Subtract(DateTime.Parse(lblApprovedStart.Text));
                    TimeSpan oSpan2 = DateTime.Parse(lblApprovedEnd.Text).Subtract(DateTime.Parse(lblApprovedStart.Text));
                    double dblSpan1 = double.Parse(oSpan1.Days.ToString());
                    double dblSpan2 = double.Parse(oSpan2.Days.ToString());
                    double dblSpan = ((dblSpan1 / dblSpan2) - 1.00) * 100;
                    lblVariancePercent.Text = dblSpan.ToString("F") + "%";
                }
                if (txtStartD.Text == "" || txtEndD.Text == "")
                {
                    chkPCRScheduleD.Enabled = false;
                    lblPCRScheduleD.Text = "N / A";
                }
                else
                    lblPCRScheduleD.Text = txtStartD.Text + " - " + txtEndD.Text;
                if (txtStartP.Text == "" || txtEndP.Text == "")
                {
                    chkPCRScheduleP.Enabled = false;
                    lblPCRScheduleP.Text = "N / A";
                }
                else
                    lblPCRScheduleP.Text = txtStartP.Text + " - " + txtEndP.Text;
                if (txtStartE.Text == "" || txtEndE.Text == "")
                {
                    chkPCRScheduleE.Enabled = false;
                    lblPCRScheduleE.Text = "N / A";
                }
                else
                    lblPCRScheduleE.Text = txtStartE.Text + " - " + txtEndE.Text;
                if (txtStartC.Text == "" || txtEndC.Text == "")
                {
                    chkPCRScheduleC.Enabled = false;
                    lblPCRScheduleC.Text = "N / A";
                }
                else
                    lblPCRScheduleC.Text = txtStartC.Text + " - " + txtEndC.Text;

                // ****************************************************
                // ***************** Project Financials ***************
                // ****************************************************
                // ..... DISCOVERY .....
                double dblAppDI = GetFloat(dsResource.Tables[0].Rows[0]["appid"].ToString());
                txtAppID.Text = dblAppDI.ToString("F");
                double dblAppDE = GetFloat(dsResource.Tables[0].Rows[0]["appexd"].ToString());
                txtAppExD.Text = dblAppDE.ToString("F");
                double dblAppDH = GetFloat(dsResource.Tables[0].Rows[0]["apphd"].ToString());
                txtAppHD.Text = dblAppDH.ToString("F");
                double dblActDI = GetFloat(dsResource.Tables[0].Rows[0]["actid"].ToString());
                txtActID.Text = dblActDI.ToString("F");
                double dblActDE = GetFloat(dsResource.Tables[0].Rows[0]["acted"].ToString());
                txtActED.Text = dblActDE.ToString("F");
                double dblActDH = GetFloat(dsResource.Tables[0].Rows[0]["acthd"].ToString());
                txtActHD.Text = dblActDH.ToString("F");
                double dblEstDI = GetFloat(dsResource.Tables[0].Rows[0]["estid"].ToString());
                txtEstID.Text = dblEstDI.ToString("F");
                double dblEstDE = GetFloat(dsResource.Tables[0].Rows[0]["ested"].ToString());
                txtEstED.Text = dblEstDE.ToString("F");
                double dblEstDH = GetFloat(dsResource.Tables[0].Rows[0]["esthd"].ToString());
                txtEstHD.Text = dblEstDH.ToString("F");
                double dblForeDI = dblActDI + dblEstDI;
                lblForeDI.Text = dblForeDI.ToString("N");
                double dblForeDE = dblActDE + dblEstDE;
                lblForeDE.Text = dblForeDE.ToString("N");
                double dblForeDH = dblActDH + dblEstDH;
                lblForeDH.Text = dblForeDH.ToString("N");
                double dblVarDDI = dblForeDI - dblAppDI;
                lblVarDDI.Text = dblVarDDI.ToString("N");
                double dblVarDDE = dblForeDE - dblAppDE;
                lblVarDDE.Text = dblVarDDE.ToString("N");
                double dblVarDDH = dblForeDH - dblAppDH;
                lblVarDDH.Text = dblVarDDH.ToString("N");
                double dblVarPDI = dblVarDDI * 100 / dblAppDI;
                lblVarPDI.Text = dblVarPDI.ToString("F") + "%";
                double dblVarPDE = dblVarDDE * 100 / dblAppDE;
                lblVarPDE.Text = dblVarPDE.ToString("F") + "%";
                double dblVarPDH = dblVarDDH * 100 / dblAppDH;
                lblVarPDH.Text = dblVarPDH.ToString("F") + "%";
                double dblAppD = dblAppDI + dblAppDE + dblAppDH;
                lblAppD.Text = dblAppD.ToString("N");
                double dblActD = dblActDI + dblActDE + dblActDH;
                lblActD.Text = dblActD.ToString("N");
                double dblEstD = dblEstDI + dblEstDE + dblEstDH;
                lblEstimateD.Text = dblEstD.ToString("N");
                double dblForeD = dblForeDI + dblForeDE + dblForeDH;
                lblForeD.Text = dblForeD.ToString("N");
                double dblVarDD = dblVarDDI + dblVarDDE + dblVarDDH;
                lblVarDD.Text = dblVarDD.ToString("N");
                double dblVarPD = dblVarDD * 100 / dblAppD;
                lblVarPD.Text = dblVarPD.ToString("F") + "%";
                // ..... PLANNING .....
                double dblAppPI = GetFloat(dsResource.Tables[0].Rows[0]["appip"].ToString());
                txtAppIP.Text = dblAppPI.ToString("F");
                double dblAppPE = GetFloat(dsResource.Tables[0].Rows[0]["appexp"].ToString());
                txtAppExp.Text = dblAppPE.ToString("F");
                double dblAppPH = GetFloat(dsResource.Tables[0].Rows[0]["apphp"].ToString());
                txtAppHP.Text = dblAppPH.ToString("F");
                double dblActPI = GetFloat(dsResource.Tables[0].Rows[0]["actip"].ToString());
                txtActIP.Text = dblActPI.ToString("F");
                double dblActPE = GetFloat(dsResource.Tables[0].Rows[0]["actep"].ToString());
                txtActEP.Text = dblActPE.ToString("F");
                double dblActPH = GetFloat(dsResource.Tables[0].Rows[0]["acthp"].ToString());
                txtActHP.Text = dblActPH.ToString("F");
                double dblEstPI = GetFloat(dsResource.Tables[0].Rows[0]["estip"].ToString());
                txtEstIP.Text = dblEstPI.ToString("F");
                double dblEstPE = GetFloat(dsResource.Tables[0].Rows[0]["estep"].ToString());
                txtEstEP.Text = dblEstPE.ToString("F");
                double dblEstPH = GetFloat(dsResource.Tables[0].Rows[0]["esthp"].ToString());
                txtEstHP.Text = dblEstPH.ToString("F");
                double dblForePI = dblActPI + dblEstPI;
                lblForePI.Text = dblForePI.ToString("N");
                double dblForePE = dblActPE + dblEstPE;
                lblForePE.Text = dblForePE.ToString("N");
                double dblForePH = dblActPH + dblEstPH;
                lblForePH.Text = dblForePH.ToString("N");
                double dblVarDPI = dblForePI - dblAppPI;
                lblVarDPI.Text = dblVarDPI.ToString("N");
                double dblVarDPE = dblForePE - dblAppPE;
                lblVarDPE.Text = dblVarDPE.ToString("N");
                double dblVarDPH = dblForePH - dblAppPH;
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
                double dblEstP = dblEstPI + dblEstPE + dblEstPH;
                lblEstimateP.Text = dblEstP.ToString("N");
                double dblForeP = dblForePI + dblForePE + dblForePH;
                lblForeP.Text = dblForeP.ToString("N");
                double dblVarDP = dblVarDPI + dblVarDPE + dblVarDPH;
                lblVarDP.Text = dblVarDP.ToString("N");
                double dblVarPP = dblVarDP * 100 / dblAppP;
                lblVarPP.Text = dblVarPP.ToString("F") + "%";
                // ..... EXECUTION  .....
                double dblAppEI = GetFloat(dsResource.Tables[0].Rows[0]["appie"].ToString());
                txtAppIE.Text = dblAppEI.ToString("F");
                double dblAppEE = GetFloat(dsResource.Tables[0].Rows[0]["appexe"].ToString());
                txtAppExE.Text = dblAppEE.ToString("F");
                double dblAppEH = GetFloat(dsResource.Tables[0].Rows[0]["apphe"].ToString());
                txtAppHE.Text = dblAppEH.ToString("F");
                double dblActEI = GetFloat(dsResource.Tables[0].Rows[0]["actie"].ToString());
                txtActIE.Text = dblActEI.ToString("F");
                double dblActEE = GetFloat(dsResource.Tables[0].Rows[0]["actee"].ToString());
                txtActEE.Text = dblActEE.ToString("F");
                double dblActEH = GetFloat(dsResource.Tables[0].Rows[0]["acthe"].ToString());
                txtActHE.Text = dblActEH.ToString("F");
                double dblEstEI = GetFloat(dsResource.Tables[0].Rows[0]["estie"].ToString());
                txtEstIE.Text = dblEstEI.ToString("F");
                double dblEstEE = GetFloat(dsResource.Tables[0].Rows[0]["estee"].ToString());
                txtEstEE.Text = dblEstEE.ToString("F");
                double dblEstEH = GetFloat(dsResource.Tables[0].Rows[0]["esthe"].ToString());
                txtEstHE.Text = dblEstEH.ToString("F");
                double dblForeEI = dblActEI + dblEstEI;
                lblForeEI.Text = dblForeEI.ToString("N");
                double dblForeEE = dblActEE + dblEstEE;
                lblForeEE.Text = dblForeEE.ToString("N");
                double dblForeEH = dblActEH + dblEstEH;
                lblForeEH.Text = dblForeEH.ToString("N");
                double dblVarDEI = dblForeEI - dblAppEI;
                lblVarDEI.Text = dblVarDEI.ToString("N");
                double dblVarDEE = dblForeEE - dblAppEE;
                lblVarDEE.Text = dblVarDEE.ToString("N");
                double dblVarDEH = dblForeEH - dblAppEH;
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
                double dblEstE = dblEstEI + dblEstEE + dblEstEH;
                lblEstimateE.Text = dblEstE.ToString("N");
                double dblForeE = dblForeEI + dblForeEE + dblForeEH;
                lblForeE.Text = dblForeE.ToString("N");
                double dblVarDE = dblVarDEI + dblVarDEE + dblVarDEH;
                lblVarDE.Text = dblVarDE.ToString("N");
                double dblVarPE = dblVarDE * 100 / dblAppE;
                lblVarPE.Text = dblVarPE.ToString("F") + "%";
                // ..... CLOSING  .....
                double dblAppCI = GetFloat(dsResource.Tables[0].Rows[0]["appic"].ToString());
                txtAppIC.Text = dblAppCI.ToString("F");
                double dblAppCE = GetFloat(dsResource.Tables[0].Rows[0]["appexc"].ToString());
                txtAppExC.Text = dblAppCE.ToString("F");
                double dblAppCH = GetFloat(dsResource.Tables[0].Rows[0]["apphc"].ToString());
                txtAppHC.Text = dblAppCH.ToString("F");
                double dblActCI = GetFloat(dsResource.Tables[0].Rows[0]["actic"].ToString());
                txtActIC.Text = dblActCI.ToString("F");
                double dblActCE = GetFloat(dsResource.Tables[0].Rows[0]["actec"].ToString());
                txtActEC.Text = dblActCE.ToString("F");
                double dblActCH = GetFloat(dsResource.Tables[0].Rows[0]["acthc"].ToString());
                txtActHC.Text = dblActCH.ToString("F");
                double dblEstCI = GetFloat(dsResource.Tables[0].Rows[0]["estic"].ToString());
                txtEstIC.Text = dblEstCI.ToString("F");
                double dblEstCE = GetFloat(dsResource.Tables[0].Rows[0]["estec"].ToString());
                txtEstEC.Text = dblEstCE.ToString("F");
                double dblEstCH = GetFloat(dsResource.Tables[0].Rows[0]["esthc"].ToString());
                txtEstHC.Text = dblEstCH.ToString("F");
                double dblForeCI = dblActCI + dblEstCI;
                lblForeCI.Text = dblForeCI.ToString("N");
                double dblForeCE = dblActCE + dblEstCE;
                lblForeCE.Text = dblForeCE.ToString("N");
                double dblForeCH = dblActCH + dblEstCH;
                lblForeCH.Text = dblForeCH.ToString("N");
                double dblVarDCI = dblForeCI - dblAppCI;
                lblVarDCI.Text = dblVarDCI.ToString("N");
                double dblVarDCE = dblForeCE - dblAppCE;
                lblVarDCE.Text = dblVarDCE.ToString("N");
                double dblVarDCH = dblForeCH - dblAppCH;
                lblVarDCH.Text = dblVarDCH.ToString("N");
                double dblVarPCI = dblVarDCI * 100 / dblAppCI;
                lblVarPCI.Text = dblVarPCI.ToString("F") + "%";
                double dblVarPCE = dblVarDCE * 100 / dblAppCE;
                lblVarPCE.Text = dblVarPCE.ToString("F") + "%";
                double dblVarPCH = dblVarDCH * 100 / dblAppCH;
                lblVarPCH.Text = dblVarPCH.ToString("F") + "%";
                double dblAppC = dblAppCI + dblAppCE + dblAppCH;
                lblAppC.Text = dblAppC.ToString("N");
                double dblActC = dblActCI + dblActCE + dblActCH;
                lblActC.Text = dblActC.ToString("N");
                double dblEstC = dblEstCI + dblEstCE + dblEstCH;
                lblEstimateC.Text = dblEstC.ToString("N");
                double dblForeC = dblForeCI + dblForeCE + dblForeCH;
                lblForeC.Text = dblForeC.ToString("N");
                double dblVarDC = dblVarDCI + dblVarDCE + dblVarDCH;
                lblVarDC.Text = dblVarDC.ToString("N");
                double dblVarPC = dblVarDC * 100 / dblAppC;
                lblVarPC.Text = dblVarPC.ToString("F") + "%";
                // Totals
                double dblForeIn = dblForeDI + dblForePI + dblForeEI + dblForeCI;
                lblForeIn.Text = dblForeIn.ToString("N");
                double dblForeEx = dblForeDE + dblForePE + dblForeEE + dblForeCE;
                lblForeEx.Text = dblForeEx.ToString("N");
                double dblForeCl = dblForeDH + dblForePH + dblForeEH + dblForeCH;
                lblForeCl.Text = dblForeCl.ToString("N");
                double dblAppIn = dblAppDI + dblAppPI + dblAppEI + dblAppCI;
                double dblAppEx = dblAppDE + dblAppPE + dblAppEE + dblAppCE;
                double dblAppCl = dblAppDH + dblAppPH + dblAppEH + dblAppCH;
                double dblVarDIn = dblForeIn - dblAppIn;
                lblVarDIn.Text = dblVarDIn.ToString("N");
                double dblVarDEx = dblForeEx - dblAppEx;
                lblVarDEx.Text = dblVarDEx.ToString("N");
                double dblVarDCl = dblForeCl - dblAppCl;
                lblVarDCl.Text = dblVarDCl.ToString("N");

                double dblVarPIn = dblVarDIn * 100 / dblAppIn;
                lblVarPIn.Text = dblVarPIn.ToString("F") + "%";
                double dblVarPEx = dblVarDEx * 100 / dblAppEx;
                lblVarPEx.Text = dblVarPEx.ToString("F") + "%";
                double dblVarPCl = dblVarDCl * 100 / dblAppCl;
                lblVarPCl.Text = dblVarPCl.ToString("F") + "%";

                double dblApp = dblAppD + dblAppP + dblAppE + dblAppC;
                lblApp.Text = dblApp.ToString("N");
                double dblAct = dblActD + dblActP + dblActE + dblActC;
                lblAct.Text = dblAct.ToString("N");
                double dblEst = dblEstD + dblEstP + dblEstE + dblEstC;
                lblEstimate.Text = dblEst.ToString("N");
                double dblFore = dblForeD + dblForeP + dblForeE + dblForeC;
                lblFore.Text = dblFore.ToString("N");
                double dblVarD = dblVarDD + dblVarDP + dblVarDE + dblVarDC;
                lblVarD.Text = dblVarD.ToString("N");
                double dblVarP = dblVarD * 100 / dblApp;
                lblVarP.Text = dblVarP.ToString("F") + "%";

                lblPCRFinancialD.Text = "$" + dblAppD.ToString("N");
                lblPCRFinancialP.Text = "$" + dblAppP.ToString("N");
                lblPCRFinancialE.Text = "$" + dblAppE.ToString("N");
                lblPCRFinancialC.Text = "$" + dblAppC.ToString("N");

                double dblApprovedI = dblAppDI + dblAppPI + dblAppEI + dblAppCI;
                double dblApprovedE = dblAppDE + dblAppPE + dblAppEE + dblAppCE;
                double dblApprovedC = dblAppDH + dblAppPH + dblAppEH + dblAppCH;
                double dblActualI = dblActDI + dblActPI + dblActEI + dblActCI;
                double dblActualE = dblActDE + dblActPE + dblActEE + dblActCE;
                double dblActualC = dblActDH + dblActPH + dblActEH + dblActCH;
                double dblEstimateI = dblEstDI + dblEstPI + dblEstEI + dblEstCI;
                double dblEstimateE = dblEstDE + dblEstPE + dblEstEE + dblEstCE;
                double dblEstimateC = dblEstDH + dblEstPH + dblEstEH + dblEstCH;
                txtStatusDate.Text = DateTime.Now.ToShortDateString();
                lblApprovedI.Text = dblApprovedI.ToString("N");
                lblAppIn.Text = lblApprovedI.Text;
                lblApprovedE.Text = dblApprovedE.ToString("N");
                lblAppEx.Text = lblApprovedE.Text;
                lblApprovedC.Text = dblApprovedC.ToString("N");
                lblAppCl.Text = lblApprovedC.Text;
                double dblApproved = dblApprovedC + dblApprovedE + dblApprovedI;
                lblApproved.Text = dblApproved.ToString("N");
                lblActualI.Text = dblActualI.ToString("N");
                lblActIn.Text = lblActualI.Text;
                lblActualE.Text = dblActualE.ToString("N");
                lblActEx.Text = lblActualE.Text;
                lblActualC.Text = dblActualC.ToString("N");
                lblActCl.Text = lblActualC.Text;
                double dblActual = dblActualI + dblActualE + dblActualC;
                lblActual.Text = dblActual.ToString("N");
                lblEstIn.Text = dblEstimateI.ToString("N");
                lblEstEx.Text = dblEstimateE.ToString("N");
                lblEstCl.Text = dblEstimateC.ToString("N");
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
                lblApprovedS.Text = lblApprovedStart.Text;
                lblApprovedF.Text = lblApprovedEnd.Text;
                lblActualS.Text = lblProjectStart.Text;
                lblActualF.Text = lblProjectEnd.Text;
                if (lblApprovedS.Text != "---" && lblActualS.Text != "---")
                {
                    TimeSpan oSpan = DateTime.Parse(lblApprovedS.Text).Subtract(DateTime.Parse(lblApprovedStart.Text));
                    lblVarianceS.Text = oSpan.Days.ToString();
                }
                else
                    lblVarianceS.Text = "---";
                if (lblApprovedF.Text != "---" && lblActualF.Text != "---")
                {
                    TimeSpan oSpan = DateTime.Parse(lblApprovedF.Text).Subtract(DateTime.Parse(lblApprovedEnd.Text));
                    lblVarianceF.Text = oSpan.Days.ToString();
                }
                else
                    lblVarianceF.Text = "---";


                txtSharepoint.Text = dsResource.Tables[0].Rows[0]["sharepoint"].ToString();
                hypSharepoint.NavigateUrl = txtSharepoint.Text;
            }

            if (Request.QueryString["div"] != null)
            {
                switch (Request.QueryString["div"])
                {
                    case "D":
                        boolDetails = true;
                        hdnTab.Value = "D";
                        break;
                    case "F":
                        boolFinancials = true;
                        hdnTab.Value = "F";
                        break;
                    case "S":
                        boolStatus = true;
                        hdnTab.Value = "S";
                        break;
                    case "I":
                        boolInformation = true;
                        hdnTab.Value = "I";
                        break;
                    case "C":
                        boolClose = true;
                        hdnTab.Value = "C";
                        break;
                    case "R":
                        boolResource = true;
                        hdnTab.Value = "R";
                        break;
                    case "L":
                        boolMilestones = true;
                        hdnTab.Value = "L";
                        break;
                }
            }
            else
            {
                boolDetails = false;
                boolFinancials = false;
                boolStatus = false;
                boolInformation = false;
                boolClose = false;
                boolResource = false;
                boolMilestones = false;
            }
            if (boolDetails == false && boolFinancials == false && boolStatus == false && boolInformation == false && boolClose == false && boolResource == false && boolMilestones == false)
                boolDetails = true;
        }
        private string GetDate(string _date)
        {
            if (_date == "")
                return "";
            else
                return DateTime.Parse(_date).ToShortDateString();
        }
        private double GetFloat(string _float)
        {
            if (_float == "")
                return 0.00;
            else
                return double.Parse(_float);
        }
        protected void btnSave_Click(Object Sender, EventArgs e)
        {
            int intResourceWorkflow = Int32.Parse(lblResourceWorkflow.Text);
            int intResourceParent = oResourceRequest.GetWorkflowParent(intResourceWorkflow);
            switch (Request.Form[hdnTab.UniqueID])
            {
                case "D":
                    UpdateProject();
                    oResourceRequest.UpdateWorkflowName(intResourceWorkflow, txtCustomName.Text, intProfile);
                    Redirect(intProject, intResourceWorkflow, "D");
                    break;
                case "F":
                    UpdateProject();
                    Redirect(intProject, intResourceWorkflow, "F");
                    break;
                case "S":
                    oResourceRequest.AddStatusTPM(intResourceWorkflow, Int32.Parse(ddlScope.SelectedItem.Value), Int32.Parse(ddlTimeline.SelectedItem.Value), Int32.Parse(ddlBudget.SelectedItem.Value), DateTime.Parse(txtStatusDate.Text), txtComments.Text, txtThisWeek.Text, txtNextWeek.Text);
                    Redirect(intProject, intResourceWorkflow, "S");
                    break;
                case "L":
                    oResourceRequest.AddMilestone(intResourceWorkflow, DateTime.Parse(txtMilestoneApproved.Text), DateTime.Parse(txtMilestoneForecasted.Text), (chkComplete.Checked ? 1 : 0), txtMilestone.Text, txtDetail.Text);
                    Redirect(intProject, intResourceWorkflow, "L");
                    break;
                case "I":
                    Redirect(intProject, intResourceWorkflow, "S");
                    break;
                case "C":
                    UpdateProject();
                    CloseProject(false);
                    Redirect(intProject, intResourceWorkflow, "C");
                    break;
                case "R":
                    Redirect(intProject, intResourceWorkflow, "R");
                    break;
            }
        }
        private void UpdateProject()
        {
            int intResourceWorkflow = Int32.Parse(lblResourceWorkflow.Text);
            int intResourceParent = oResourceRequest.GetWorkflowParent(intResourceWorkflow);
            if (oCustomized.GetTPM(intRequest, intItem, intNumber).Tables[0].Rows.Count == 0)
                oCustomized.AddTPM(intRequest, intItem, intNumber, 3, oRequest.Get(intRequest, "description"), DateTime.Parse(oRequest.Get(intRequest, "start_date")), DateTime.Parse(oRequest.Get(intRequest, "end_date")));
            oCustomized.UpdateTPM(intRequest, intItem, intNumber, (chkFinancials.Checked ? 1 : 0), txtStartD.Text, txtEndD.Text, txtStartP.Text, txtEndP.Text, txtStartE.Text, txtEndE.Text, txtStartC.Text, txtEndC.Text, Request.Form[hdnCosts.UniqueID], ddlPPM.SelectedItem.Value, txtAppSD.Text, txtAppED.Text, txtAppSP.Text, txtAppEP.Text, txtAppSE.Text, txtAppEE.Text, txtAppSC.Text, txtAppEC.Text, txtAppID.Text, txtAppExD.Text, txtAppHD.Text, txtActID.Text, txtActED.Text, txtActHD.Text, txtEstID.Text, txtEstED.Text, txtEstHD.Text, txtAppIP.Text, txtAppExp.Text, txtAppHP.Text, txtActIP.Text, txtActEP.Text, txtActHP.Text, txtEstIP.Text, txtEstEP.Text, txtEstHP.Text, txtAppIE.Text, txtAppExE.Text, txtAppHE.Text, txtActIE.Text, txtActEE.Text, txtActHE.Text, txtEstIE.Text, txtEstEE.Text, txtEstHE.Text, txtAppIC.Text, txtAppExC.Text, txtAppHC.Text, txtActIC.Text, txtActEC.Text, txtActHC.Text, txtEstIC.Text, txtEstEC.Text, txtEstHC.Text, txtSharepoint.Text, txtBetter.Text, txtWorse.Text, txtLessons.Text);
            string strName = "";
            if (txtProjectName1.Text != lblName.Text)
                strName = txtProjectName1.Text;
            if (txtProjectName2.Text != lblName.Text)
                strName = txtProjectName2.Text;
            if (strName == "")
                strName = lblName.Text;
            if (strName != "")
            {
                string strBody = "";
                string strTo = "";
                if (strName != lblName.Text)
                    strBody += "<li>The project name was changed<br/><br/></li>";
                int intNewPortfolio = Int32.Parse(ddlPortfolio.SelectedItem.Value);
                int intNewStatus = Int32.Parse(ddlStatus.SelectedItem.Value);
                int intNewSegment = 0;
                if (Request.Form[hdnSegment.UniqueID] != "")
                    intNewSegment = Int32.Parse(Request.Form[hdnSegment.UniqueID]);
                int intStatus = -100;
                int intUser = intProfile;
                if (intProject > 0)
                {
                    int intPortfolio = Int32.Parse(oProject.Get(intProject, "organization"));
                    if (intPortfolio != intNewPortfolio)
                        strBody += "<li>The sponsoring portfolio was changed<br/><br/></li>";
                    int intSegment = Int32.Parse(oProject.Get(intProject, "segmentid"));
                    if (intSegment != intNewSegment)
                        strBody += "<li>The segment was changed<br/><br/></li>";
                    intStatus = Int32.Parse(oProject.Get(intProject, "status"));
                    if (intStatus != intNewStatus)
                        strBody += "<li>The project status was changed to " + ddlStatus.SelectedItem.Text.ToUpper() + "<br/><br/></li>";
                    intUser = Int32.Parse(oProject.Get(intProject, "userid"));
                }
                oProject.Update(intProject, strName, oProject.Get(intProject, "bd"), txtProjectNumber.Text, intUser, intNewPortfolio, intNewSegment, intNewStatus);
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
                if (intProject > 0)
                    oProject.Update(intProject, 0, intExecutive, intWorking, 0, 0, 0);
                if (strBody != "" && boolNotifyChange == true)
                    Notify(strTo, strBody, (intStatus != intNewStatus));
            }
        }
        private void Redirect(int _projectid, int _resourceid, string _div)
        {
            if (intPage == 0)
                Response.Redirect(Request.Path + "?rrid=" + _resourceid.ToString() + "&div=" + _div + "&save=true");
            else
                Response.Redirect(oPage.GetFullLink(intPage) + "?pid=" + _projectid.ToString() + "&div=" + _div + "&save=true");
        }
        protected void chkMyDescription_Change(Object Sender, EventArgs e)
        {
            if (chkMyDescription.Checked == true)
                Response.Redirect(oPage.GetFullLink(intPage) + "?pid=" + intProject.ToString() + "&mydoc=true&div=D");
            else
                Response.Redirect(oPage.GetFullLink(intPage) + "?pid=" + intProject.ToString() + "&div=D");
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
            int intResourceWorkflow = Int32.Parse(lblResourceWorkflow.Text);
            int intResourceParent = oResourceRequest.GetWorkflowParent(intResourceWorkflow);
            int intRequest = Int32.Parse(oResourceRequest.Get(intResourceParent, "requestid"));
            int intProject = oRequest.GetProjectNumber(intRequest);
            DataSet dsProject = oProject.Get(intProject);
            bool boolApproved = oProject.IsApproved(intProject);
            string strTo = "";
            string strUser = oUser.GetName(intProfile);
            if (strUser != "")
                strTo += strUser + ";";
            if (intExecutive > 0)
                strTo += oUser.GetName(intExecutive) + ";";
            if (intWorking > 0)
                strTo += oUser.GetName(intWorking) + ";";
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
            oResourceRequest.UpdateWorkflowStatus(intResourceWorkflow, _status, true);
            // NOTIFICATION
            //oFunction.SendEmail("Project Status Update", strTo, strCC, strBCC, "Project Status Update", "<p><b>The following project request has been " + strStatus + " by " + oUser.GetFullName(intProfile) + "...</b></p><p>" + oProject.GetBody(intProject, intEnvironment, true) + "</p>", true, false);

            strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_PROJECT");
            oFunction.SendEmail("Project Status Update", "", "", strEMailIdsBCC, "Project Status Update", "<p>TO: " + strTo + "<br/>CC: " + strCC + "<b>The following project request has been " + strStatus + " by " + oUser.GetFullName(intProfile) + "...</b></p><p>" + oProject.GetBody(intProject, intEnvironment, true) + "</p>", true, false);
        }
        private void CloseProject(bool _done)
        {
            int intResourceWorkflow = Int32.Parse(lblResourceWorkflow.Text);
            int intResourceParent = oResourceRequest.GetWorkflowParent(intResourceWorkflow);
            if (_done == true)
            {
                oResourceRequest.UpdateWorkflowStatus(intResourceWorkflow, 3, true);
                oResourceRequest.CloseWorkflow(intResourceWorkflow, intEnvironment, 0, dsnServiceEditor, true, intResourceRequestApprove, intAssignPage, intViewPage, dsnAsset, dsnIP);
                oProject.Close(intProject, intEnvironment);
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
            strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_PROJECT");
            oFunction.SendEmail("Project Details Update", "", "", strEMailIdsBCC, "Project Details Update", "<p>TO: " + strEmail + "<br/>CC: " + strCC + _to + "<b>This message is to notify you that the following project has been modified...</b></p><p>" + oProject.GetBody(intProject, intEnvironment, true) + "</p><p><br/><span style=\"color:#0000FF\"><b>Description of Change(s):</b></span></p><p><ul>" + _text + "</ul></p>", true, false);
        }
        protected void btnRetrieve_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(oPage.GetFullLink(intPage) + "?pid=" + intProject.ToString() + "&div=S&retrieve=true");
        }

        // Vijay Code
        protected void btnSubmitPCR_Click(Object Sender, EventArgs e)
        {
            // string strPath = ExportPCRToPDF();
            string strPath = ExportToPDF("PCR");
            string strReason = "";
            for (int ii = 0; ii < chkPCRReason.Items.Count; ii++)
            {
                if (chkPCRReason.Items[ii].Selected == true)
                    strReason += chkPCRReason.Items[ii].Value + ";";
            }
            int intId = oTPM.AddPCR(intRequest, intItem, intNumber, txtPCRName.Text, (chkScope.Checked ? 1 : 0), (chkSchedule.Checked ? 1 : 0), txtPCRScheduleDS.Text, txtPCRScheduleDE.Text, txtPCRSchedulePS.Text, txtPCRSchedulePE.Text, txtPCRScheduleES.Text, txtPCRScheduleEE.Text, txtPCRScheduleCS.Text, txtPCRScheduleCE.Text, (chkFinancial.Checked ? 1 : 0), txtPCRFinancialD.Text, txtPCRFinancialP.Text, txtPCRFinancialE.Text, txtPCRFinancialC.Text, strReason, strPath, txtScopeComments.Text, txtScheduleComments.Text, txtFinancialComments.Text);
            Response.Redirect(oPage.GetFullLink(intPage) + "?pid=" + intProject.ToString() + "&div=I&save=true");
        }

        protected void btnSubmitCSRC_Click(Object Sender, EventArgs e)
        {
            // string strPath = ExportCSRCtoPDF();
            string strPath = ExportToPDF("CSRC");
            int intId = oTPM.AddCSRC(intRequest, intItem, intNumber, txtCSRCName.Text, (chkDiscovery.Checked ? 1 : 0), (chkPlanning.Checked ? 1 : 0), (chkExecution.Checked ? 1 : 0), (chkClosing.Checked ? 1 : 0), txtCSRCSD.Text, txtCSRCED.Text, txtCSRCID.Text, txtCSRCExD.Text, txtCSRCHD.Text, txtCSRCSP.Text, txtCSRCEP.Text, txtCSRCIP.Text, txtCSRCExP.Text, txtCSRCHP.Text, txtCSRCSE.Text, txtCSRCEE.Text, txtCSRCIE.Text, txtCSRCExE.Text, txtCSRCHE.Text, txtCSRCSC.Text, txtCSRCEC.Text, txtCSRCIC.Text, txtCSRCExC.Text, txtCSRCHC.Text, strPath);
            Response.Redirect(oPage.GetFullLink(intPage) + "?pid=" + intProject.ToString() + "&div=I&save=true");
        }

        protected string ExportToPDF(string strExportType)
        {
            Document doc = new Document();
            Cell cell;
            FileStream fs = null;
            string strFile = "";
            string strVirtualPath = "";
            string strPhysicalPath = "";

            try
            {

                iTextSharp.text.Table oTable = new iTextSharp.text.Table(2);
                oTable.BorderWidth = 0;
                oTable.BorderColor = new iTextSharp.text.Color(255, 255, 255);
                oTable.Padding = 2;
                oTable.Width = 100;

                iTextSharp.text.Font oFontHeader = new iTextSharp.text.Font(iTextSharp.text.Font.HELVETICA, 10, 1);
                iTextSharp.text.Font oFontBold = new iTextSharp.text.Font(iTextSharp.text.Font.HELVETICA, 8, 1);
                iTextSharp.text.Font oFont = new iTextSharp.text.Font(iTextSharp.text.Font.HELVETICA, 8, 0);


                strFile = lblName.Text + "_" + DateTime.Now.ToShortTimeString().Replace(":", "") + ".pdf";
                strVirtualPath = oVariables.UploadsFolder() + strFile;
                strPhysicalPath = strVirtualPath;

                fs = new FileStream(strPhysicalPath, FileMode.Create);
                PdfWriter.GetInstance(doc, fs);
                // PdfWriter.GetInstance(doc, Response.OutputStream);

                if (strExportType == "CSRC")
                {
                    string strHeader = "ClearView CSRC Information";
                    HeaderFooter header = new HeaderFooter(new Phrase(strHeader, new iTextSharp.text.Font(iTextSharp.text.Font.HELVETICA, 6)), false);
                    header.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    header.Alignment = 2;
                    doc.Header = header;
                    string strFooter = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString();
                    HeaderFooter footer = new HeaderFooter(new Phrase(strFooter, new iTextSharp.text.Font(iTextSharp.text.Font.HELVETICA, 6)), false);
                    footer.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    footer.Alignment = 2;
                    doc.Footer = footer;
                    doc.Open();

                    cell = new Cell(new Phrase("Project Capital Service Review Committee Report", oFontHeader));
                    cell.Colspan = 2;
                    cell.BackgroundColor = new iTextSharp.text.Color(169, 162, 141);
                    oTable.AddCell(cell);

                    cell = new Cell(new Phrase("PMM Phase", oFontBold));
                    cell.Colspan = 2;
                    oTable.AddCell(cell);
                    oTable.AddCell(new Cell(new Phrase("Discovery", oFont)));
                    oTable.AddCell(new Cell(new Phrase((chkDiscovery.Checked ? "Yes" : "No"), oFont)));
                    oTable.AddCell(new Cell(new Phrase("Planning", oFont)));
                    oTable.AddCell(new Cell(new Phrase((chkPlanning.Checked ? "Yes" : "No"), oFont)));
                    oTable.AddCell(new Cell(new Phrase("Execution", oFont)));
                    oTable.AddCell(new Cell(new Phrase((chkExecution.Checked ? "Yes" : "No"), oFont)));
                    oTable.AddCell(new Cell(new Phrase("Closing", oFont)));
                    oTable.AddCell(new Cell(new Phrase((chkClosing.Checked ? "Yes" : "No"), oFont)));


                    cell = new Cell(new Phrase("Discovery", oFontBold));
                    cell.Colspan = 2;
                    oTable.AddCell(cell);

                    oTable.AddCell(new Cell(new Phrase("Phase start date", oFont)));
                    if (txtCSRCSD.Text == "")
                        oTable.AddCell(new Cell(new Phrase("N / A", oFont)));
                    else
                        oTable.AddCell(new Cell(new Phrase(GetDate(txtCSRCSD.Text), oFont)));

                    oTable.AddCell(new Cell(new Phrase("Phase end date", oFont)));
                    if (txtCSRCED.Text == "")
                        oTable.AddCell(new Cell(new Phrase("N / A", oFont)));
                    else
                        oTable.AddCell(new Cell(new Phrase(GetDate(txtCSRCED.Text), oFont)));

                    oTable.AddCell(new Cell(new Phrase("Internal Labor", oFont)));
                    if (txtCSRCID.Text == "")
                        oTable.AddCell(new Cell(new Phrase("$0.0", oFont)));
                    else
                        oTable.AddCell(new Cell(new Phrase("$" + GetFloat(txtCSRCID.Text).ToString("F"), oFont)));

                    oTable.AddCell(new Cell(new Phrase("External Labor", oFont)));
                    if (txtCSRCExD.Text == "")
                        oTable.AddCell(new Cell(new Phrase("$0.0", oFont)));
                    else
                        oTable.AddCell(new Cell(new Phrase("$" + GetFloat(txtCSRCExD.Text).ToString("F"), oFont)));

                    oTable.AddCell(new Cell(new Phrase("HW/SW/One Time Cost", oFont)));
                    if (txtCSRCHD.Text == "")
                        oTable.AddCell(new Cell(new Phrase("$0.0", oFont)));
                    else
                        oTable.AddCell(new Cell(new Phrase("$" + GetFloat(txtCSRCHD.Text).ToString("F"), oFont)));

                    cell = new Cell(new Phrase("Planning", oFontBold));
                    cell.Colspan = 2;
                    oTable.AddCell(cell);

                    oTable.AddCell(new Cell(new Phrase("Phase start date", oFont)));
                    if (txtCSRCSP.Text == "")
                        oTable.AddCell(new Cell(new Phrase("N / A", oFont)));
                    else
                        oTable.AddCell(new Cell(new Phrase(GetDate(txtCSRCSP.Text), oFont)));

                    oTable.AddCell(new Cell(new Phrase("Phase end date", oFont)));
                    if (txtCSRCEP.Text == "")
                        oTable.AddCell(new Cell(new Phrase("N / A", oFont)));
                    else
                        oTable.AddCell(new Cell(new Phrase(GetDate(txtCSRCEP.Text), oFont)));

                    oTable.AddCell(new Cell(new Phrase("Internal Labor", oFont)));
                    if (txtCSRCIP.Text == "")
                        oTable.AddCell(new Cell(new Phrase("$0.0", oFont)));
                    else
                        oTable.AddCell(new Cell(new Phrase("$" + GetFloat(txtCSRCIP.Text).ToString("F"), oFont)));

                    oTable.AddCell(new Cell(new Phrase("External Labor", oFont)));
                    if (txtCSRCExP.Text == "")
                        oTable.AddCell(new Cell(new Phrase("$0.0", oFont)));
                    else
                        oTable.AddCell(new Cell(new Phrase("$" + GetFloat(txtCSRCExP.Text).ToString("F"), oFont)));

                    oTable.AddCell(new Cell(new Phrase("HW/SW/One Time Cost", oFont)));
                    if (txtCSRCHP.Text == "")
                        oTable.AddCell(new Cell(new Phrase("$0.0", oFont)));
                    else
                        oTable.AddCell(new Cell(new Phrase("$" + GetFloat(txtCSRCHP.Text).ToString("F"), oFont)));

                    cell = new Cell(new Phrase("Execution", oFontBold));
                    cell.Colspan = 2;
                    oTable.AddCell(cell);

                    oTable.AddCell(new Cell(new Phrase("Phase start date", oFont)));
                    if (txtCSRCSE.Text == "")
                        oTable.AddCell(new Cell(new Phrase("N / A", oFont)));
                    else
                        oTable.AddCell(new Cell(new Phrase(GetDate(txtCSRCSE.Text), oFont)));

                    oTable.AddCell(new Cell(new Phrase("Phase end date", oFont)));
                    if (txtCSRCEE.Text == "")
                        oTable.AddCell(new Cell(new Phrase("N / A", oFont)));
                    else
                        oTable.AddCell(new Cell(new Phrase(GetDate(txtCSRCEE.Text), oFont)));

                    oTable.AddCell(new Cell(new Phrase("Internal Labor", oFont)));
                    if (txtCSRCIE.Text == "")
                        oTable.AddCell(new Cell(new Phrase("$0.0", oFont)));
                    else
                        oTable.AddCell(new Cell(new Phrase("$" + GetFloat(txtCSRCIE.Text).ToString("F"), oFont)));

                    oTable.AddCell(new Cell(new Phrase("External Labor", oFont)));
                    if (txtCSRCExE.Text == "")
                        oTable.AddCell(new Cell(new Phrase("$0.0", oFont)));
                    else
                        oTable.AddCell(new Cell(new Phrase("$" + GetFloat(txtCSRCExE.Text).ToString("F"), oFont)));

                    oTable.AddCell(new Cell(new Phrase("HW/SW/One Time Cost", oFont)));
                    if (txtCSRCHE.Text == "")
                        oTable.AddCell(new Cell(new Phrase("$0.0", oFont)));
                    else
                        oTable.AddCell(new Cell(new Phrase("$" + GetFloat(txtCSRCHE.Text).ToString("F"), oFont)));

                    cell = new Cell(new Phrase("Closing", oFontBold));
                    cell.Colspan = 2;
                    oTable.AddCell(cell);

                    oTable.AddCell(new Cell(new Phrase("Phase start date", oFont)));
                    if (txtCSRCSC.Text == "")
                        oTable.AddCell(new Cell(new Phrase("N / A", oFont)));
                    else
                        oTable.AddCell(new Cell(new Phrase(GetDate(txtCSRCSC.Text), oFont)));

                    oTable.AddCell(new Cell(new Phrase("Phase end date", oFont)));
                    if (txtCSRCEC.Text == "")
                        oTable.AddCell(new Cell(new Phrase("N / A", oFont)));
                    else
                        oTable.AddCell(new Cell(new Phrase(GetDate(txtCSRCEC.Text), oFont)));

                    oTable.AddCell(new Cell(new Phrase("Internal Labor", oFont)));
                    if (txtCSRCIC.Text == "")
                        oTable.AddCell(new Cell(new Phrase("$0.0", oFont)));
                    else
                        oTable.AddCell(new Cell(new Phrase("$" + GetFloat(txtCSRCIC.Text).ToString("F"), oFont)));

                    oTable.AddCell(new Cell(new Phrase("External Labor", oFont)));
                    if (txtCSRCExC.Text == "")
                        oTable.AddCell(new Cell(new Phrase("$0.0", oFont)));
                    else
                        oTable.AddCell(new Cell(new Phrase("$" + GetFloat(txtCSRCExC.Text).ToString("F"), oFont)));

                    oTable.AddCell(new Cell(new Phrase("HW/SW/One Time Cost", oFont)));
                    if (txtCSRCHC.Text == "")
                        oTable.AddCell(new Cell(new Phrase("$0.0", oFont)));
                    else
                        oTable.AddCell(new Cell(new Phrase("$" + GetFloat(txtCSRCHC.Text).ToString("F"), oFont)));
                    doc.Add(oTable);
                }
                else
                {

                    string strHeader = "ClearView PCR Information";
                    HeaderFooter header = new HeaderFooter(new Phrase(strHeader, new iTextSharp.text.Font(iTextSharp.text.Font.HELVETICA, 6)), false);
                    header.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    header.Alignment = 2;
                    doc.Header = header;
                    string strFooter = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString();
                    HeaderFooter footer = new HeaderFooter(new Phrase(strFooter, new iTextSharp.text.Font(iTextSharp.text.Font.HELVETICA, 6)), false);
                    footer.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    footer.Alignment = 2;
                    doc.Footer = footer;
                    doc.Open();


                    cell = new Cell(new Phrase("Project Change Request Report", oFontHeader));
                    cell.Colspan = 2;
                    cell.BackgroundColor = new iTextSharp.text.Color(169, 162, 141);
                    oTable.AddCell(cell);


                    //oTable.AddCell(new Cell(new Phrase("Project Name:", oFontBold)));
                    //oTable.AddCell(new Cell(new Phrase(lblName.Text, oFont)));
                    //oTable.AddCell(new Cell(new Phrase("Date Submitted:", oFontBold)));
                    //oTable.AddCell(new Cell(new Phrase(lblSubmitted.Text, oFont)));
                    //oTable.AddCell(new Cell(new Phrase("Change Request Name:", oFontBold)));
                    //oTable.AddCell(new Cell(new Phrase(txtPCRName.Text, oFont)));
                    //oTable.AddCell(new Cell(new Phrase("Portfolio:", oFontBold)));
                    //oTable.AddCell(new Cell(new Phrase(lblPortfolio.Text, oFont)));
                    //oTable.AddCell(new Cell(new Phrase("Technical Project Manager:", oFontBold)));

                    //string strTPM = oUser.GetFullName(oProject.Get(intProject, "technical"));

                    //DataSet dsResource = oCustomized.GetTPM(intRequest, intItem, intNumber);
                    //string[] strCosts = dsResource.Tables[0].Rows[0]["costs"].ToString().Split('&');
                    //string strCost = "";


                    //foreach (string str in strCosts)
                    //{
                    //    if (str != "")
                    //    {
                    //        strCost += oCost.GetName(Int32.Parse(str));
                    //    }
                    //}


                    //oTable.AddCell(new Cell(new Phrase(strTPM == "" ? "Not Assigned" : strTPM, oFont)));
                    //oTable.AddCell(new Cell(new Phrase("Cost Center:", oFontBold)));
                    //oTable.AddCell(new Cell(new Phrase(strCost, oFont)));



                    oTable.AddCell(new Cell(new Phrase("Scope:", oFontBold)));
                    oTable.AddCell(new Cell(new Phrase((chkScope.Checked ? "Yes" : "No"), oFont)));
                    oTable.AddCell(new Cell(new Phrase("Schedule:", oFontBold)));
                    oTable.AddCell(new Cell(new Phrase((chkSchedule.Checked ? "Yes" : "No"), oFont)));
                    oTable.AddCell(new Cell(new Phrase("Financial:", oFontBold)));
                    oTable.AddCell(new Cell(new Phrase((chkFinancial.Checked ? "Yes" : "No"), oFont)));


                    cell = new Cell(new Phrase("Detailed Description of Proposed Change", oFontBold));
                    cell.Colspan = 2;
                    oTable.AddCell(cell);

                    oTable.AddCell(new Cell(new Phrase("Scope:", oFontBold)));
                    oTable.AddCell(new Cell(new Paragraph(txtScopeComments.Text == "" ? "N/A" : txtScopeComments.Text, oFont)));

                    oTable.AddCell(new Cell(new Phrase("Schedule:", oFontBold)));
                    oTable.AddCell(new Cell(new Paragraph(txtScheduleComments.Text == "" ? "N/A" : txtScheduleComments.Text, oFont)));

                    oTable.AddCell(new Cell(new Phrase("Financial:", oFontBold)));
                    oTable.AddCell(new Cell(new Paragraph(txtFinancialComments.Text == "" ? "N/A" : txtFinancialComments.Text, oFont)));

                    doc.Add(oTable);


                    iTextSharp.text.Table oTable2 = new iTextSharp.text.Table(3);
                    oTable2.BorderWidth = 0;
                    oTable2.BorderColor = new iTextSharp.text.Color(255, 255, 255);
                    oTable2.Padding = 2;
                    oTable2.Width = 100;

                    cell = new Cell(new Phrase("Schedule Change Details", oFontBold));
                    cell.Colspan = 3;
                    oTable2.AddCell(cell);

                    cell = new Cell(new Phrase("Phase", oFontBold));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    oTable2.AddCell(cell);
                    cell = new Cell(new Phrase("Approved Dates", oFontBold));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    oTable2.AddCell(cell);
                    cell = new Cell(new Phrase("Modified Dates", oFontBold));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    oTable2.AddCell(cell);

                    oTable2.AddCell(new Cell(new Phrase("Discovery", oFont)));
                    string strPCRScheduleAppD = chkPCRScheduleD.Enabled ? txtAppSD.Text + "-" + txtAppED.Text : lblPCRScheduleD.Text;
                    string strPCRScheduleModD = chkPCRScheduleD.Enabled ? txtPCRScheduleDS.Text + "-" + txtPCRScheduleDE.Text : lblPCRScheduleD.Text;
                    cell = new Cell(new Phrase(strPCRScheduleAppD, oFont));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    oTable2.AddCell(cell);
                    cell = new Cell(new Phrase(strPCRScheduleModD, oFont));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    oTable2.AddCell(cell);

                    oTable2.AddCell(new Cell(new Phrase("Planning", oFont)));
                    string strPCRScheduleAppP = chkPCRScheduleP.Enabled ? txtAppSP.Text + "-" + txtAppEP.Text : lblPCRScheduleP.Text;
                    string strPCRScheduleModP = chkPCRScheduleP.Enabled ? txtPCRSchedulePS.Text + "-" + txtPCRSchedulePE.Text : lblPCRScheduleP.Text;
                    cell = new Cell(new Phrase(strPCRScheduleAppP, oFont));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    oTable2.AddCell(cell);
                    cell = new Cell(new Phrase(strPCRScheduleModP, oFont));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    oTable2.AddCell(cell);

                    oTable2.AddCell(new Cell(new Phrase("Execution", oFont)));
                    string strPCRScheduleAppE = chkPCRScheduleE.Enabled ? txtAppSE.Text + "-" + txtAppEE.Text : lblPCRScheduleE.Text;
                    string strPCRScheduleModE = chkPCRScheduleE.Enabled ? txtPCRScheduleES.Text + "-" + txtPCRScheduleEE.Text : lblPCRScheduleE.Text;
                    cell = new Cell(new Phrase(strPCRScheduleAppE, oFont));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    oTable2.AddCell(cell);
                    cell = new Cell(new Phrase(strPCRScheduleModE, oFont));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    oTable2.AddCell(cell);

                    oTable2.AddCell(new Cell(new Phrase("Closing", oFont)));
                    string strPCRScheduleAppC = chkPCRScheduleC.Enabled ? txtAppSC.Text + "-" + txtAppEC.Text : lblPCRScheduleC.Text;
                    string strPCRScheduleModC = chkPCRScheduleC.Enabled ? txtPCRScheduleCS.Text + "-" + txtPCRScheduleCE.Text : lblPCRScheduleC.Text;
                    cell = new Cell(new Phrase(strPCRScheduleAppC, oFont));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    oTable2.AddCell(cell);
                    cell = new Cell(new Phrase(strPCRScheduleModC, oFont));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    oTable2.AddCell(cell);

                    doc.Add(oTable2);


                    iTextSharp.text.Table oTable3 = new iTextSharp.text.Table(3);
                    oTable3.BorderWidth = 0;
                    oTable3.BorderColor = new iTextSharp.text.Color(255, 255, 255);
                    oTable3.Padding = 2;
                    oTable3.Alignment = Element.ALIGN_LEFT;
                    oTable3.Width = 100;

                    cell = new Cell(new Phrase("Financial Change Details", oFontBold));
                    cell.Colspan = 3;
                    oTable3.AddCell(cell);


                    cell = new Cell(new Phrase("Phase", oFontBold));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    oTable3.AddCell(cell);
                    cell = new Cell(new Phrase("Approved Financials", oFontBold));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    oTable3.AddCell(cell);
                    cell = new Cell(new Phrase("Modified Financials", oFontBold));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    oTable3.AddCell(cell);


                    oTable3.AddCell(new Cell(new Phrase("Discovery", oFont)));
                    cell = new Cell(new Phrase(lblPCRFinancialD.Text, oFont));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    oTable3.AddCell(cell);
                    cell = new Cell(new Phrase("$" + GetFloat(txtPCRFinancialD.Text).ToString("F"), oFont));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    oTable3.AddCell(cell);

                    oTable3.AddCell(new Cell(new Phrase("Planning", oFont)));
                    cell = new Cell(new Phrase(lblPCRFinancialP.Text, oFont));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    oTable3.AddCell(cell);
                    cell = new Cell(new Phrase("$" + GetFloat(txtPCRFinancialP.Text).ToString("F"), oFont));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    oTable3.AddCell(cell);

                    oTable3.AddCell(new Cell(new Phrase("Execution", oFont)));
                    cell = new Cell(new Phrase(lblPCRFinancialE.Text, oFont));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    oTable3.AddCell(cell);
                    cell = new Cell(new Phrase("$" + GetFloat(txtPCRFinancialE.Text).ToString("F"), oFont));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    oTable3.AddCell(cell);

                    oTable3.AddCell(new Cell(new Phrase("Closing", oFont)));
                    cell = new Cell(new Phrase(lblPCRFinancialC.Text, oFont));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    oTable3.AddCell(cell);
                    cell = new Cell(new Phrase("$" + GetFloat(txtPCRFinancialC.Text).ToString("F"), oFont));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    oTable3.AddCell(cell);
                    doc.Add(oTable3);



                    iTextSharp.text.Table oTable5 = new iTextSharp.text.Table(4);
                    oTable5.BorderWidth = 0;
                    oTable5.BorderColor = new iTextSharp.text.Color(255, 255, 255);
                    oTable5.Padding = 2;
                    oTable5.Width = 100;



                    cell = new Cell(new Phrase("Detail Of Financial Impact Of Proposed Change", oFontBold));
                    cell.Colspan = 4;
                    oTable5.AddCell(cell);



                    cell = new Cell(new Phrase("Discovery", oFontBold));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    oTable5.AddCell(cell);
                    cell = new Cell(new Phrase("Current Approved Budget", oFontBold));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    oTable5.AddCell(cell);
                    cell = new Cell(new Phrase("Change in Budget", oFontBold));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    oTable5.AddCell(cell);
                    cell = new Cell(new Phrase("New Budget Total", oFontBold));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    oTable5.AddCell(cell);



                    oTable5.AddCell(new Cell(new Phrase("Internal Labor", oFont)));
                    cell = new Cell(new Phrase("$" + GetFloat(txtAppID.Text).ToString("N"), oFont));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    oTable5.AddCell(cell);
                    cell = new Cell(new Phrase("$" + GetFloat(txtActID.Text).ToString("N"), oFont));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    oTable5.AddCell(cell);
                    cell = new Cell(new Phrase("$" + GetFloat(lblForeDI.Text).ToString("N"), oFont));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    oTable5.AddCell(cell);

                    oTable5.AddCell(new Phrase("External Labor", oFont));
                    cell = new Cell(new Phrase("$" + GetFloat(txtAppExD.Text).ToString("N"), oFont));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    oTable5.AddCell(cell);
                    cell = new Cell(new Phrase("$" + GetFloat(txtActED.Text).ToString("N"), oFont));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    oTable5.AddCell(cell);
                    cell = new Cell(new Phrase("$" + GetFloat(lblForeDE.Text).ToString("N"), oFont));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    oTable5.AddCell(cell);


                    oTable5.AddCell(new Phrase("HW/SW/One Time Cost", oFont));
                    cell = new Cell(new Phrase("$" + GetFloat(txtAppHD.Text).ToString("N"), oFont));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    oTable5.AddCell(cell);
                    cell = new Cell(new Phrase("$" + GetFloat(txtActHD.Text).ToString("N"), oFont));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    oTable5.AddCell(cell);
                    cell = new Cell(new Phrase("$" + GetFloat(lblForeDH.Text).ToString("N"), oFont));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    oTable5.AddCell(cell);




                    oTable5.AddCell(new Phrase("Total", oFont));
                    cell = new Cell(new Phrase("$" + GetFloat(lblAppD.Text).ToString("N"), oFont));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    oTable5.AddCell(cell);
                    cell = new Cell(new Phrase("$" + GetFloat(lblActD.Text).ToString("N"), oFont));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    oTable5.AddCell(cell);
                    cell = new Cell(new Phrase("$" + GetFloat(lblForeD.Text).ToString("N"), oFont));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    oTable5.AddCell(cell);



                    cell = new Cell();
                    cell.Colspan = 4;
                    oTable5.AddCell(cell);


                    cell = new Cell(new Phrase("Planning", oFontBold));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    oTable5.AddCell(cell);
                    cell = new Cell(new Phrase("Current Approved Budget", oFontBold));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    oTable5.AddCell(cell);
                    cell = new Cell(new Phrase("Change in Budget", oFontBold));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    oTable5.AddCell(cell);
                    cell = new Cell(new Phrase("New Budget Total", oFontBold));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    oTable5.AddCell(cell);


                    oTable5.AddCell(new Cell(new Phrase("Internal Labor", oFont)));
                    cell = new Cell(new Phrase("$" + GetFloat(txtAppIP.Text).ToString("N"), oFont));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    oTable5.AddCell(cell);
                    cell = new Cell(new Phrase("$" + GetFloat(txtActIP.Text).ToString("N"), oFont));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    oTable5.AddCell(cell);
                    cell = new Cell(new Phrase("$" + GetFloat(lblForePI.Text).ToString("N"), oFont));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    oTable5.AddCell(cell);

                    oTable5.AddCell(new Phrase("External Labor", oFont));
                    cell = new Cell(new Phrase("$" + GetFloat(txtAppExp.Text).ToString("N"), oFont));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    oTable5.AddCell(cell);
                    cell = new Cell(new Phrase("$" + GetFloat(txtActEP.Text).ToString("N"), oFont));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    oTable5.AddCell(cell);
                    cell = new Cell(new Phrase("$" + GetFloat(lblForePE.Text).ToString("N"), oFont));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    oTable5.AddCell(cell);


                    oTable5.AddCell(new Phrase("HW/SW/One Time Cost", oFont));
                    cell = new Cell(new Phrase("$" + GetFloat(txtAppHP.Text).ToString("N"), oFont));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    oTable5.AddCell(cell);
                    cell = new Cell(new Phrase("$" + GetFloat(txtActHP.Text).ToString("N"), oFont));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    oTable5.AddCell(cell);
                    cell = new Cell(new Phrase("$" + GetFloat(lblForePH.Text).ToString("N"), oFont));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    oTable5.AddCell(cell);



                    oTable5.AddCell(new Phrase("Total", oFont));
                    cell = new Cell(new Phrase("$" + GetFloat(lblAppP.Text).ToString("N"), oFont));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    oTable5.AddCell(cell);
                    cell = new Cell(new Phrase("$" + GetFloat(lblActP.Text).ToString("N"), oFont));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    oTable5.AddCell(cell);
                    cell = new Cell(new Phrase("$" + GetFloat(lblForeP.Text).ToString("N"), oFont));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    oTable5.AddCell(cell);


                    cell = new Cell();
                    cell.Colspan = 4;
                    oTable5.AddCell(cell);


                    cell = new Cell(new Phrase("Execution", oFontBold));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    oTable5.AddCell(cell);
                    cell = new Cell(new Phrase("Current Approved Budget", oFontBold));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    oTable5.AddCell(cell);
                    cell = new Cell(new Phrase("Change in Budget", oFontBold));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    oTable5.AddCell(cell);
                    cell = new Cell(new Phrase("New Budget Total", oFontBold));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    oTable5.AddCell(cell);

                    oTable5.AddCell(new Cell(new Phrase("Internal Labor", oFont)));
                    cell = new Cell(new Phrase("$" + GetFloat(txtAppIE.Text).ToString("N"), oFont));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    oTable5.AddCell(cell);
                    cell = new Cell(new Phrase("$" + GetFloat(txtActIE.Text).ToString("N"), oFont));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    oTable5.AddCell(cell);
                    cell = new Cell(new Phrase("$" + GetFloat(lblForeEI.Text).ToString("N"), oFont));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    oTable5.AddCell(cell);


                    oTable5.AddCell(new Phrase("External Labor", oFont));
                    cell = new Cell(new Phrase("$" + GetFloat(txtAppExE.Text).ToString("N"), oFont));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    oTable5.AddCell(cell);
                    cell = new Cell(new Phrase("$" + GetFloat(txtActEE.Text).ToString("N"), oFont));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    oTable5.AddCell(cell);
                    cell = new Cell(new Phrase("$" + GetFloat(lblForeEE.Text).ToString("N"), oFont));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    oTable5.AddCell(cell);


                    oTable5.AddCell(new Phrase("HW/SW/One Time Cost", oFont));
                    cell = new Cell(new Phrase("$" + GetFloat(txtAppHE.Text).ToString("N"), oFont));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    oTable5.AddCell(cell);
                    cell = new Cell(new Phrase("$" + GetFloat(txtActHE.Text).ToString("N"), oFont));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    oTable5.AddCell(cell);
                    cell = new Cell(new Phrase("$" + GetFloat(lblForeEH.Text).ToString("N"), oFont));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    oTable5.AddCell(cell);



                    oTable5.AddCell(new Phrase("Total", oFont));
                    cell = new Cell(new Phrase("$" + GetFloat(lblAppE.Text).ToString("N"), oFont));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    oTable5.AddCell(cell);
                    cell = new Cell(new Phrase("$" + GetFloat(lblActE.Text).ToString("N"), oFont));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    oTable5.AddCell(cell);
                    cell = new Cell(new Phrase("$" + GetFloat(lblForeE.Text).ToString("N"), oFont));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    oTable5.AddCell(cell);

                    cell = new Cell();
                    cell.Colspan = 4;
                    oTable5.AddCell(cell);



                    cell = new Cell(new Phrase("Closing", oFontBold));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    oTable5.AddCell(cell);
                    cell = new Cell(new Phrase("Current Approved Budget", oFontBold));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    oTable5.AddCell(cell);
                    cell = new Cell(new Phrase("Change in Budget", oFontBold));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    oTable5.AddCell(cell);
                    cell = new Cell(new Phrase("New Budget Total", oFontBold));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    oTable5.AddCell(cell);

                    oTable5.AddCell(new Cell(new Phrase("Internal Labor", oFont)));
                    cell = new Cell(new Phrase("$" + GetFloat(txtAppIC.Text).ToString("N"), oFont));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    oTable5.AddCell(cell);
                    cell = new Cell(new Phrase("$" + GetFloat(txtActIC.Text).ToString("N"), oFont));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    oTable5.AddCell(cell);
                    cell = new Cell(new Phrase("$" + GetFloat(lblForeCI.Text).ToString("N"), oFont));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    oTable5.AddCell(cell);

                    oTable5.AddCell(new Phrase("External Labor", oFont));
                    cell = new Cell(new Phrase("$" + GetFloat(txtAppExC.Text).ToString("N"), oFont));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    oTable5.AddCell(cell);
                    cell = new Cell(new Phrase("$" + GetFloat(txtActEC.Text).ToString("N"), oFont));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    oTable5.AddCell(cell);
                    cell = new Cell(new Phrase("$" + GetFloat(lblForeCE.Text).ToString("N"), oFont));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    oTable5.AddCell(cell);


                    oTable5.AddCell(new Phrase("HW/SW/One Time Cost", oFont));
                    cell = new Cell(new Phrase("$" + GetFloat(txtAppHC.Text).ToString("N"), oFont));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    oTable5.AddCell(cell);
                    cell = new Cell(new Phrase("$" + GetFloat(txtActHC.Text).ToString("N"), oFont));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    oTable5.AddCell(cell);
                    cell = new Cell(new Phrase("$" + GetFloat(lblForeCH.Text).ToString("N"), oFont));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    oTable5.AddCell(cell);


                    oTable5.AddCell(new Phrase("Total", oFont));
                    cell = new Cell(new Phrase("$" + GetFloat(lblAppC.Text).ToString("N"), oFont));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    oTable5.AddCell(cell);
                    cell = new Cell(new Phrase("$" + GetFloat(lblActC.Text).ToString("N"), oFont));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    oTable5.AddCell(cell);
                    cell = new Cell(new Phrase("$" + GetFloat(lblForeC.Text).ToString("N"), oFont));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    oTable5.AddCell(cell);



                    doc.Add(oTable5);



                    iTextSharp.text.Table oTable4 = new iTextSharp.text.Table(3);
                    oTable4.BorderWidth = 0;
                    oTable4.BorderColor = new iTextSharp.text.Color(255, 255, 255);
                    oTable4.Padding = 2;
                    oTable4.Width = 100;
                    cell = new Cell(new Phrase("Reason for PCR ", oFontBold));
                    cell.Colspan = 3;
                    oTable4.AddCell(cell);


                    iTextSharp.text.List list = new List(false, 10);
                    list.IndentationRight = 2.5F;
                    list.ListSymbol = new Chunk("\u2022", FontFactory.GetFont(FontFactory.HELVETICA, 10));

                    for (int ii = 0; ii < chkPCRReason.Items.Count; ii++)
                    {
                        if (chkPCRReason.Items[ii].Selected == true)
                            list.Add(new iTextSharp.text.ListItem(chkPCRReason.Items[ii].Value, oFont));
                    }
                    cell = new Cell(list);
                    cell.Colspan = 3;
                    oTable4.AddCell(cell);
                    doc.Add(oTable4);
                }


                //Response.ContentType = "application/pdf";
                //Response.AddHeader("Content-Disposition", "attachment; filename=export.pdf");
                //Response.End();
                //Response.Flush();
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "msg", "alert('" + ex.Message.Replace("'", "") + "');", true);

            }
            finally
            {
                doc.Close();
                fs.Close();
            }
            return strVirtualPath;

        }

        protected void Generate_PDF(Object Sender, EventArgs e)
        {
            Document doc = new Document();
            FileStream fs = null;
            string strPath = "";
            string strFile = "";
            try
            {

                iTextSharp.text.Table oTable = new iTextSharp.text.Table(2);
                oTable.BorderWidth = 0;
                oTable.BorderColor = new iTextSharp.text.Color(255, 255, 255);
                oTable.Padding = 2;
                oTable.Width = 100;

                iTextSharp.text.Font oFontHeader = new iTextSharp.text.Font(iTextSharp.text.Font.HELVETICA, 10, 1);
                iTextSharp.text.Font oFontBold = new iTextSharp.text.Font(iTextSharp.text.Font.HELVETICA, 8, 1);
                iTextSharp.text.Font oFont = new iTextSharp.text.Font(iTextSharp.text.Font.HELVETICA, 8, 0);


                //string strFile = DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString() + "_" + intProfile.ToString() + ".pdf";
                strFile = lblName.Text + "_ClosureReport" + DateTime.Now.ToLongTimeString().Replace(":", "") + DateTime.Now.Millisecond.ToString() + ".pdf";
                strPath = oVariables.UploadsFolder() + strFile;
                fs = new FileStream(strPath, FileMode.Create);

                PdfWriter.GetInstance(doc, fs);
                string strHeader = "ClearView Close Tab Summary";
                HeaderFooter header = new HeaderFooter(new Phrase(strHeader, new iTextSharp.text.Font(iTextSharp.text.Font.HELVETICA, 6)), false);
                header.Border = iTextSharp.text.Rectangle.NO_BORDER;
                header.Alignment = 2;
                doc.Header = header;
                string strFooter = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString();
                HeaderFooter footer = new HeaderFooter(new Phrase(strFooter, new iTextSharp.text.Font(iTextSharp.text.Font.HELVETICA, 6)), false);
                footer.Border = iTextSharp.text.Rectangle.NO_BORDER;
                footer.Alignment = 2;
                doc.Footer = footer;
                doc.Open();

                Cell cell = new Cell(new Phrase("Project Closure Form Report", oFontHeader));
                cell.Colspan = 2;
                cell.BackgroundColor = new iTextSharp.text.Color(169, 162, 141);
                oTable.AddCell(cell);



                int intTPM = Int32.Parse(oProject.Get(intProject, "technical"));
                string strTPM = oUser.GetFullName(intTPM);

                oTable.AddCell(new Cell(new Phrase("Project Name:", oFontBold)));
                oTable.AddCell(new Cell(new Phrase(lblName.Text, oFont)));
                oTable.AddCell(new Cell(new Phrase("Clarity #:", oFontBold)));
                oTable.AddCell(new Cell(new Phrase(txtProjectNumber.Text, oFont)));
                oTable.AddCell(new Cell(new Phrase("Technical Project Manager:", oFontBold)));
                oTable.AddCell(new Cell(new Phrase(strTPM, oFont)));
                oTable.AddCell(new Cell(new Phrase("Executive Sponsor:", oFontBold)));
                oTable.AddCell(new Cell(new Phrase(lblExecutive.Text, oFont)));
                oTable.AddCell(new Cell(new Phrase("Portfolio:", oFontBold)));
                oTable.AddCell(new Cell(new Phrase(lblPortfolio.Text, oFont)));
                oTable.AddCell(new Cell(new Phrase("Date Submitted:", oFontBold)));
                oTable.AddCell(new Cell(new Phrase(DateTime.Now.ToShortDateString(), oFont)));



                iTextSharp.text.List list = new List(false, 10);
                list.IndentationRight = 2.5F;
                list.ListSymbol = new Chunk("\u2022", FontFactory.GetFont(FontFactory.HELVETICA, 10));
                string[] strArr = hdnAcc.Value.Split('\n');
                foreach (string str in strArr)
                {
                    if (str != "")
                    {
                        string str2 = str.Substring(str.IndexOf("(edit)") + 6).Replace('\r', ' ');
                        list.Add(new iTextSharp.text.ListItem(str2, oFont));
                    }
                }

                cell = new Cell();
                cell.Colspan = 2;
                cell.Rowspan = 2;

                string strTxt = "This notification is to confirm the closure of Project \"" + lblName.Text + "\". As of "
                            + lblProjectEnd.Text + " Project \"" + lblName.Text + "\" will be considered formally closed. The following has been accomplished:";

                cell.Add(new Phrase(strTxt, oFont));
                cell.Add(list);


                string strText = "Please contact " + txtLead.Text + " by " + txtOneWeek.Text + " if you identify any "
                                + " issues that could require the project to be re-opened.";


                cell.Add(new Phrase(strText, oFont));
                //  cell.Colspan = 2;
                oTable.AddCell(cell);



                iTextSharp.text.Table oTable2 = new iTextSharp.text.Table(3);
                oTable2.BorderWidth = 0;
                oTable2.BorderColor = new iTextSharp.text.Color(255, 255, 255);
                oTable2.Padding = 2;
                oTable2.Width = 100;


                cell = new Cell(new Phrase("Project Budget Summary", oFontBold));
                cell.Header = true;
                cell.Colspan = 3;


                oTable2.AddCell(cell);
                oTable2.AddCell(new Cell());
                oTable2.AddCell(new Cell(new Phrase("Execution C1 + Approved Project Change Requests", oFont)));
                oTable2.AddCell(new Cell(new Phrase("Actuals", oFont)));
                oTable2.AddCell(new Cell(new Phrase("Project Start Date:", oFont)));
                oTable2.AddCell(new Cell(new Phrase(txtAppSE.Text, oFont)));
                oTable2.AddCell(new Cell(new Phrase(txtStartE.Text, oFont)))
                    ;
                oTable2.AddCell(new Cell(new Phrase("Project End Date:", oFont)));
                oTable2.AddCell(new Cell(new Phrase(txtAppEE.Text, oFont)));
                oTable2.AddCell(new Cell(new Phrase(txtEndE.Text, oFont)));

                oTable2.AddCell(new Cell(new Phrase("Internal Labor Costs:", oFont)));
                oTable2.AddCell(new Cell(new Phrase("$" + txtAppIE.Text, oFont)));
                double dblIEActual = Double.Parse(txtActIE.Text == String.Empty ? "0" : txtActIE.Text) + Double.Parse(txtEstIE.Text == String.Empty ? "0" : txtEstIE.Text);
                oTable2.AddCell(new Cell(new Phrase("$" + dblIEActual.ToString("F"), oFont)));

                oTable2.AddCell(new Cell(new Phrase("External Labor Costs:", oFont)));
                oTable2.AddCell(new Cell(new Phrase("$" + txtAppExE.Text, oFont)));
                double dblExActual = Double.Parse(txtActEE.Text == String.Empty ? "0" : txtActEE.Text) + Double.Parse(txtEstEE.Text == String.Empty ? "0" : txtEstEE.Text);
                oTable2.AddCell(new Cell(new Phrase("$" + dblExActual.ToString("F"), oFont)));

                oTable2.AddCell(new Cell(new Phrase("Capital Costs:", oFont)));
                oTable2.AddCell(new Cell(new Phrase("$" + txtAppHE.Text, oFont)));
                double dblHActual = Double.Parse(txtActHE.Text == String.Empty ? "0" : txtActHE.Text) + Double.Parse(txtEstHE.Text == String.Empty ? "0" : txtEstHE.Text);
                oTable2.AddCell(new Cell(new Phrase("$" + dblHActual.ToString("F"), oFont)));

                cell = new Cell(new Phrase("Any Actual Values that are higher/later than the Execution C1+Approved Project change Requests values, are explained below:", oFontBold));
                cell.Colspan = 3;
                oTable2.AddCell(cell);

                cell = new Cell(new Paragraph(txtVariance.Text == "" ? "N/A" : txtVariance.Text, oFont));
                cell.Colspan = 3;
                oTable2.AddCell(cell);


                iTextSharp.text.Table oTable3 = new iTextSharp.text.Table(2);
                oTable3.BorderWidth = 0;
                oTable3.BorderColor = new iTextSharp.text.Color(255, 255, 255);
                oTable3.Padding = 2;
                oTable3.Width = 100;
                oTable3.AddCell(new Cell(new Phrase("Deliverable/Milestone", oFontBold)));
                oTable3.AddCell(new Cell(new Phrase("Completion Date", oFontBold)));
                DataSet ds2 = (DataSet)rptMilestones.DataSource;
                foreach (DataRow dr in ds2.Tables[0].Rows)
                {
                    oTable3.AddCell(new Cell(new Phrase(dr["Milestone"].ToString(), oFont)));
                    oTable3.AddCell(new Cell(new Phrase(DateTime.Parse(dr["Approved"].ToString()).ToShortDateString(), oFont)));
                }

                cell = new Cell(new Phrase("Project Repository is:", oFontBold));
                cell.Colspan = 3;
                cell.Add(new Phrase(txtSharepoint.Text, oFont));
                oTable2.AddCell(cell);

                cell = new Cell(new Phrase("Lessons Learned (What best practices can be taken from the project?):", oFontBold));
                cell.Colspan = 3;
                cell.Rowspan = 3;
                cell.Add(new Phrase(txtLessons.Text, oFont));
                oTable2.AddCell(cell);

                doc.Add(oTable);
                doc.Add(oTable2);
                doc.Add(oTable3);

                oTPM.AddProjectClosurePDF(intRequest, intItem, intNumber, oVariables.UploadsFolder() + strFile);
                Response.Redirect(oPage.GetFullLink(intPage) + "?pid=" + intProject.ToString() + "&div=C");
            }
            catch { }
            finally
            {
                doc.Close();
                fs.Close();
            }
        }

        public void RaiseCallbackEvent(String eventArgument)
        {
            int Intid = Int32.Parse(eventArgument.Substring(0, eventArgument.IndexOf(":")));
            int IntStatus = Int32.Parse(eventArgument.Substring(eventArgument.IndexOf(":") + 1).Replace("+", ""));
            oTPM.UpdatePCRStatus(Intid, IntStatus);

        }

        public string GetCallbackResult()
        {
            return strResult;
        }

    
    }
}