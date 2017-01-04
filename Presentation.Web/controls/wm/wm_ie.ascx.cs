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
using System.Text;
namespace NCC.ClearView.Presentation.Web
{
    public partial class wm_ie : System.Web.UI.UserControl
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
        protected int intImplementorDistributed = Int32.Parse(ConfigurationManager.AppSettings["ITEMID_IMPLEMENTOR_DISTRIBUTED"]);
        protected int intImplementorMidrange = Int32.Parse(ConfigurationManager.AppSettings["ITEMID_IMPLEMENTOR_MIDRANGE"]);
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
        protected Delegates oDelegate;
        protected Documents oDocument;
        protected StatusLevels oStatus;
        protected Forecast oForecast;
        protected Models oModel;
        protected Types oType;
        protected ModelsProperties oModelsProperties;
        protected Confidence oConfidence;
        protected int intApplication = 0;
        protected int intPage = 0;
        protected int intProject = 0;
        protected int intID = 0;
        protected bool boolDetails = false;
        protected bool boolExecution = false;
        protected bool boolIDC = false;
        protected bool boolDesigns = false;
        protected bool boolDocuments = false;
        protected bool boolMyDocuments = false;
        protected bool boolResource = false;
        protected int intRequest = 0;
        protected int intItem = 0;
        protected int intNumber = 0;
        protected int intService = 0;
        protected string strTabs = "";

        // My variable declarations start - Vijay
        protected int id;
        protected Customized oCustom;
        protected Variables oVariable;
     
        protected int intResourceWorkflow = 0;
        protected int intResourceParent = 0;
        // My variable declarations end - Vijay
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
            oDelegate = new Delegates(intProfile, dsn);
            oDocument = new Documents(intProfile, dsn);
            oStatus = new StatusLevels();
            oForecast = new Forecast(intProfile, dsn);
            oModel = new Models(intProfile, dsn);
            oType = new Types(intProfile, dsn);
            oModelsProperties = new ModelsProperties(intProfile, dsn);
            oConfidence = new Confidence(intProfile, dsn);
            oCustom = new Customized(intProfile, dsn);
            if (Request.QueryString["applicationid"] != null && Request.QueryString["applicationid"] != "")
                intApplication = Int32.Parse(Request.QueryString["applicationid"]);
            if (Request.QueryString["pageid"] != null && Request.QueryString["pageid"] != "")
                intPage = Int32.Parse(Request.QueryString["pageid"]);
            if (Request.Cookies["application"] != null && Request.Cookies["application"].Value != "")
                intApplication = Int32.Parse(Request.Cookies["application"].Value);
            if (Request.QueryString["rrid"] != null && Request.QueryString["rrid"] != "")
                intResourceWorkflow = Int32.Parse(Request.QueryString["rrid"]);
            else if (Request.QueryString["pid"] != null && Request.QueryString["pid"] != "")
            {
                intProject = Int32.Parse(Request.QueryString["pid"]);
                ds = oForecast.GetProject(intProject);
                if (ds.Tables[0].Rows.Count > 0)
                    intID = Int32.Parse(ds.Tables[0].Rows[0]["id"].ToString());
                else
                {
                    intRequest = oRequest.Add(intProject, intProfile);
                    intID = oForecast.Add(intRequest, "", intProfile);
                }
                ds = oResourceRequest.GetProjectUser(intProject, intProfile);
                if (ds.Tables[0].Rows.Count > 0)
                    intResourceWorkflow = Int32.Parse(ds.Tables[0].Rows[0]["id"].ToString());
            }
            if (intResourceWorkflow > 0)
            {
                // Start Workflow Change
                lblResourceWorkflow.Text = intResourceWorkflow.ToString();
                intResourceParent = oResourceRequest.GetWorkflowParent(intResourceWorkflow);
                ds = oResourceRequest.Get(intResourceParent);
                // End Workflow Change
                intRequest = Int32.Parse(ds.Tables[0].Rows[0]["requestid"].ToString());
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
                if (dblAllocated > 0.00)
                {
                    panSlider.Visible = true;
                    dblUsed = (dblUsed / dblAllocated) * 100;
                    sldHours._StartPercent = dblUsed.ToString();
                    sldHours._TotalHours = dblAllocated.ToString();
                }
                if (Request.QueryString["save"] != null && Request.QueryString["save"] != "")
                    Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "saved", "<script type=\"text/javascript\">alert('Information Saved Successfully');<" + "/" + "script>");
                if (Request.QueryString["status"] != null && Request.QueryString["status"] != "")
                    Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "statusd", "<script type=\"text/javascript\">alert('Status Updates has been Added');<" + "/" + "script>");
                intProject = oRequest.GetProjectNumber(intRequest);
                DataSet dsForecast = oForecast.GetProject(intProject);
                if (dsForecast.Tables[0].Rows.Count > 0)
                    intID = Int32.Parse(dsForecast.Tables[0].Rows[0]["id"].ToString());
                else
                {
                    int intTempRequest = oRequest.Add(intProject, intProfile);
                    intID = oForecast.Add(intTempRequest, "", intProfile);
                }
                // Get TPM of project
                int intTPMr = 0;
                int intTPMi = 0;
                int intTPMn = 0;
                int intTPMu = 0;
                DataSet dsTPMItems = oService.GetTPM();
                foreach (DataRow drTPMItem in dsTPMItems.Tables[0].Rows)
                {
                    int _itemid = oService.GetItemId(Int32.Parse(drTPMItem["serviceid"].ToString()));
                    DataSet dsTPMall = oResourceRequest.GetProjectItem(intProject, _itemid);
                    if (dsTPMall.Tables[0].Rows.Count > 0)
                    {
                        intTPMr = Int32.Parse(dsTPMall.Tables[0].Rows[0]["requestid"].ToString());
                        intTPMi = Int32.Parse(dsTPMall.Tables[0].Rows[0]["itemid"].ToString());
                        intTPMn = Int32.Parse(dsTPMall.Tables[0].Rows[0]["number"].ToString());
                        intTPMu = (dsTPMall.Tables[0].Rows[0]["userid"].ToString() == "" ? 0 : Int32.Parse(dsTPMall.Tables[0].Rows[0]["userid"].ToString()));
                    }
                }
                if (intTPMu > 0)
                {
                    lblDetManager.Text = oUser.GetFullName(intTPMu);
                    DataSet dsTPM = oCustom.GetTPM(intTPMr, intTPMi, intTPMn);
                    lblDetPhase.Text = dsTPM.Tables[0].Rows[0]["ppm"].ToString();
                    lblDetDesc.Text = oRequest.Get(intTPMr, "description");
                    if (oRequest.Get(intTPMr, "start_date") != "")
                        lblDetStart.Text = DateTime.Parse(oRequest.Get(intTPMr, "start_date")).ToShortDateString();
                    if (oRequest.Get(intTPMr, "end_date") != "")
                        lblDetEnd.Text = DateTime.Parse(oRequest.Get(intTPMr, "end_date")).ToShortDateString();
                    if (dsTPM.Tables[0].Rows[0]["sharepoint"].ToString() != "")
                        lblDetURL.Text = "<a href=\"" + dsTPM.Tables[0].Rows[0]["sharepoint"].ToString() + "\" target=\"_blank\">" + dsTPM.Tables[0].Rows[0]["sharepoint"].ToString() + "</a>";
                }
                // Details
                lblDetStatus.Text = oStatus.HTML(Int32.Parse(oProject.Get(intProject, "status")));
                //                lblDetFinancials.Text = "??? (Please provide more description of what you want to see here)";
                hdnTab.Value = "D";
                panWorkload.Visible = true;
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
                // Investigation
                if (!IsPostBack)
                {
                    DataSet dsInvestigation = oCustom.GetIDCDetails(intRequest, intItem, intNumber);
                    if (dsInvestigation.Tables[0].Rows.Count > 0)
                    {
                        int intSPOC = 0;
                        if (dsInvestigation.Tables[0].Rows[0]["idc_spoc"].ToString() != "0")
                        {
                            intSPOC = Int32.Parse(dsInvestigation.Tables[0].Rows[0]["idc_spoc"].ToString());
                            txtIDCSPOC.Text = oUser.GetFullName(intSPOC) + " (" + oUser.GetName(intSPOC) + ")";
                        }
                        hdnIDCSPOC.Value = intSPOC.ToString();
                        int intInvestigatedBy = 0;
                        if (dsInvestigation.Tables[0].Rows[0]["investigated_by"].ToString() != "0")
                        {
                            intInvestigatedBy = Int32.Parse(dsInvestigation.Tables[0].Rows[0]["investigated_by"].ToString());
                            txtInvestigatedBy.Text = oUser.GetFullName(intInvestigatedBy) + " (" + oUser.GetName(intInvestigatedBy) + ")";
                        }
                        hdnInvestigatedBy.Value = intInvestigatedBy.ToString();
                        drpInvestigated.SelectedValue = dsInvestigation.Tables[0].Rows[0]["investigated"].ToString();
                        if (dsInvestigation.Tables[0].Rows[0]["followup_date"].ToString() != "")
                            txtFollowupDate.Text = DateTime.Parse(dsInvestigation.Tables[0].Rows[0]["followup_date"].ToString()).ToShortDateString();
                        if (dsInvestigation.Tables[0].Rows[0]["date_engaged"].ToString() != "")
                            txtDateEngaged.Text = DateTime.Parse(dsInvestigation.Tables[0].Rows[0]["date_engaged"].ToString()).ToShortDateString();
                        drpPhase.SelectedValue = dsInvestigation.Tables[0].Rows[0]["phase_engaged"].ToString();
                        drpEffortSize.SelectedValue = dsInvestigation.Tables[0].Rows[0]["effort_size"].ToString();
                        drpInvolvement.SelectedValue = dsInvestigation.Tables[0].Rows[0]["involvement"].ToString();
                        if (drpInvolvement.SelectedItem.Value == "No")
                            divInvolvement.Style["display"] = "inline";
                        drpEIT.SelectedValue = dsInvestigation.Tables[0].Rows[0]["eit_testing"].ToString();
                        drpProjectClass.SelectedValue = dsInvestigation.Tables[0].Rows[0]["project_class"].ToString();
                        drpEnterprise.SelectedValue = dsInvestigation.Tables[0].Rows[0]["enterprise_release"].ToString();
                        drpNoInvolve.SelectedValue = dsInvestigation.Tables[0].Rows[0]["no_involve"].ToString();
                        txtComment.Text = dsInvestigation.Tables[0].Rows[0]["comments"].ToString();
                        double dblSlide = double.Parse(dsInvestigation.Tables[0].Rows[0]["slide_statement"].ToString()) * 100;
                        sldSlide1._StartPercent = dblSlide.ToString();
                        sldSlide1._TotalHours = "1.00";
                        dblSlide = double.Parse(dsInvestigation.Tables[0].Rows[0]["slide_alternatives"].ToString()) * 100;
                        sldSlide2._StartPercent = dblSlide.ToString();
                        sldSlide2._TotalHours = "1.00";
                        dblSlide = double.Parse(dsInvestigation.Tables[0].Rows[0]["slide_recommendations"].ToString()) * 100;
                        sldSlide3._StartPercent = dblSlide.ToString();
                        sldSlide3._TotalHours = "1.00";
                        dblSlide = double.Parse(dsInvestigation.Tables[0].Rows[0]["slide_high_level"].ToString()) * 100;
                        sldSlide4._StartPercent = dblSlide.ToString();
                        sldSlide4._TotalHours = "1.00";
                        dblSlide = double.Parse(dsInvestigation.Tables[0].Rows[0]["slide_detailed"].ToString()) * 100;
                        sldSlide5._StartPercent = dblSlide.ToString();
                        sldSlide5._TotalHours = "1.00";
                    }
                }
                else
                {
                    int intInvestigatedBy = 0;
                    if (Request.Form[hdnInvestigatedBy.UniqueID] != "")
                        intInvestigatedBy = Int32.Parse(Request.Form[hdnInvestigatedBy.UniqueID]);
                    int intIDCSPOC = 0;
                    if (Request.Form[hdnIDCSPOC.UniqueID] != "")
                        intIDCSPOC = Int32.Parse(Request.Form[hdnIDCSPOC.UniqueID]);
                    oCustom.AddIDCDetails(intRequest, intItem, intNumber, drpInvestigated.SelectedItem.Text, intInvestigatedBy, txtFollowupDate.Text, txtDateEngaged.Text, drpPhase.SelectedItem.Text, drpEffortSize.SelectedItem.Text, drpInvolvement.SelectedItem.Text, drpEIT.SelectedItem.Text, drpProjectClass.SelectedItem.Text, drpEnterprise.SelectedItem.Text, drpNoInvolve.SelectedItem.Text, intIDCSPOC, txtComment.Text);
                    boolIDC = true;
                }
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
                    int intStatus = Int32.Parse(_status.Text);
                    int intUser2 = Int32.Parse(_user.Text);
                    _user.Text = oUser.GetFullName(intUser2);
                    Label _item = (Label)ri.FindControl("lblItem");
                    int intItem2 = Int32.Parse(_item.Text);
                    Label _allocated = (Label)ri.FindControl("lblAllocated");
                    Label _used = (Label)ri.FindControl("lblUsed");
                    double dblAllocated2 = oResourceRequest.GetAllocated(intProject, intUser2, intItem2);
                    double dblUsed2 = oResourceRequest.GetUsed(intProject, intUser2, intItem2);
                    Label _percent = (Label)ri.FindControl("lblPercent");
                    _allocated.Text = dblAllocated2.ToString();
                    _used.Text = dblUsed2.ToString();
                    if (dblAllocated2 > 0)
                    {
                        dblUsed2 = dblUsed2 / dblAllocated2;
                        _percent.Text = dblUsed2.ToString("P");
                    }
                    else
                        _percent.Text = dblAllocated2.ToString("P");
                    if (intItem2 == 0)
                        _item.Text = "Project Coordinator";
                    else
                    {
                        if (intItem2 == -1)
                            _item.Text = "Design Implementor";
                        else
                        {
                            int intApp2 = oRequestItem.GetItemApplication(intItem2);
                            _item.Text = oApplication.GetName(intApp2);
                        }
                    }
                    _status.Text = oStatus.Name(intStatus);
                }
                // Load Designs
                ds = oForecast.GetAnswers(intID);
                if ((Request.QueryString["c"] != null && Request.QueryString["c"] != "") || (Request.QueryString["e"] != null && Request.QueryString["e"] != ""))
                {
                    string strC = Request.QueryString["c"];
                    if (strC != "")
                    {
                        Classes oClass = new Classes(intProfile, dsn);
                        btnClear.Enabled = true;
                        if (lblFilter.Text != "")
                            lblFilter.Text += ", ";
                        lblFilter.Text += oClass.Get(Int32.Parse(strC), "name");

                    }
                    string strE = Request.QueryString["e"];
                    if (strE != "")
                    {
                        Environments oEnvironments = new Environments(intProfile, dsn);
                        btnClear.Enabled = true;
                        if (lblFilter.Text != "")
                            lblFilter.Text += ", ";
                        lblFilter.Text += oEnvironments.Get(Int32.Parse(strE), "name");

                    }
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        if (strC != "" && dr["classid"].ToString() != strC)
                            dr.Delete();
                        else if (strE != "" && dr["environmentid"].ToString() != strE)
                            dr.Delete();
                    }
                }
                if (lblFilter.Text == "")
                    lblFilter.Text = "All Environments";
                rptAll.DataSource = ds;
                rptAll.DataBind();
                double dblQT = 0.00;
                double dblAT = 0.00;
                double dblOT = 0.00;
                double dblAmp = 0.00;
                double dblHours = 0.00;
                double dblStorageTotal = 0.00;
                foreach (RepeaterItem ri in rptAll.Items)
                {
                    Label lblId = (Label)ri.FindControl("lblId");
                    Label lblRequestId = (Label)ri.FindControl("lblRequestId");
                    bool boolOverride = (oForecast.GetAnswer(Int32.Parse(lblId.Text), "override") == "1");
                    Label lblPlatformId = (Label)ri.FindControl("lblPlatformId");
                    int intPlatform = Int32.Parse(lblPlatformId.Text);
                    Label lblPlatform = (Label)ri.FindControl("lblPlatform");
                    lblPlatform.Text = "<a href=\"javascript:void(0);\" onclick=\"OpenWindow('PROFILE','?pid=" + intPlatform.ToString() + "');\">" + lblPlatform.Text + "</a>";
                    Label lblCommitment = (Label)ri.FindControl("lblCommitment");
                    // QUANTITY
                    Label lblQuantity = (Label)ri.FindControl("lblQuantity");
                    Label lblRecovery = (Label)ri.FindControl("lblRecovery");
                    double dblQuantity = double.Parse(lblQuantity.Text) + double.Parse(lblRecovery.Text);
                    lblQuantity.Text = "<a href=\"javascript:void(0);\" onclick=\"OpenNewWindow('/frame/forecast/forecast_print_quantity.aspx?id=" + lblId.Text + "',275,200);\">" + dblQuantity.ToString() + "</a>";
                    dblQT += dblQuantity;
                    // AMP and MODEL
                    int intModel = 0;
                    int intType = 0;
                    string strModel = "";
                    double dblReplicate = 0.00;
                    Label lblModel = (Label)ri.FindControl("lblModel");
                    Label lblAmp = (Label)ri.FindControl("lblAmp");
                    // If completed, use the model of the asset.  If not, find the model.
                    // PROGRESS BAR
                    Label lblStep = (Label)ri.FindControl("lblStep");
                    double dblStep = double.Parse(lblStep.Text);
                    double dblSteps = double.Parse(oForecast.GetSteps(intPlatform, 1).Tables[0].Rows.Count.ToString());
                    double dblStepsDone = double.Parse(oForecast.GetStepsDone(Int32.Parse(lblId.Text), 1).Tables[0].Rows.Count.ToString());
                    double dblComplete = (((dblStepsDone) / dblSteps) * 100);
                    lblStep.Text = oServiceRequest.GetStatusBar(dblComplete, "50", "8", false);
                    // LINKS (Delete, Execute, Etc...)
                    Label lblComplete = (Label)ri.FindControl("lblComplete");
                    if (lblComplete.Text != "")
                    {
                        Panel panComplete = (Panel)ri.FindControl("panComplete");
                        lblComplete.Text = "Completed&nbsp;(" + DateTime.Parse(lblComplete.Text).ToShortDateString() + ")";
                        intModel = oForecast.GetModelAsset(Int32.Parse(lblId.Text));
                        if (intModel == 0)
                            intModel = oForecast.GetModel(Int32.Parse(lblId.Text));
                        double dblAmpTemp = (double.Parse(oModelsProperties.Get(intModel, "amp")) * dblQuantity);
                        lblAmp.Text = dblAmpTemp.ToString("N");
                        dblAmp += dblAmpTemp;
                        strModel = oModelsProperties.Get(intModel, "name");
                        double.TryParse(oModelsProperties.Get(intModel, "replicate_times"), out dblReplicate);
                        intModel = Int32.Parse(oModelsProperties.Get(intModel, "modelid"));
                        if (intModel > 0)
                            intType = oModel.GetType(intModel);
                        string strPDF = oModel.Get(intModel, "pdf");
                        if (strPDF != "")
                            lblModel.Text = "<a href=\"javascript:void(0);\" onclick=\"OpenNewWindowMAX('" + strPDF + "');\">" + strModel + "</a>";
                        else
                            lblModel.Text = strModel;
                        panComplete.Visible = true;
                    }
                    else
                    {
                        Panel panExecute = (Panel)ri.FindControl("panExecute");
                        panExecute.Visible = true;
                        LinkButton oDelete = (LinkButton)ri.FindControl("btnDelete");
                        oDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this line item?');");
                        LinkButton oExecute = (LinkButton)ri.FindControl("btnExecute");
                        int intServerModel = oForecast.GetModel(Int32.Parse(lblId.Text));
                        if (intServerModel == 0)
                        {
                            // Get the model selected in the equipment dropdown (if not server)
                            intModel = Int32.Parse(lblModel.Text);
                            if (boolOverride == true && intModel > 0)
                            {
                                intServerModel = intModel;
                                intModel = Int32.Parse(oModelsProperties.Get(intServerModel, "modelid"));
                            }
                        }
                        if (intServerModel > 0)
                        {
                            double dblAmpTemp = (double.Parse(oModelsProperties.Get(intServerModel, "amp")) * dblQuantity);
                            double.TryParse(oModelsProperties.Get(intServerModel, "replicate_times"), out dblReplicate);
                            lblAmp.Text = dblAmpTemp.ToString("N");
                            dblAmp += dblAmpTemp;
                            if (intModel == 0)
                                intModel = Int32.Parse(oModelsProperties.Get(intServerModel, "modelid"));
                            strModel = oModelsProperties.Get(intServerModel, "name");
                        }
                        else if (intModel > 0)
                        {
                            //if (oModelsProperties.Get(intModel).Tables[0].Rows.Count > 0)
                            //{
                            //    double dblAmpTemp = (double.Parse(oModelsProperties.Get(intModel, "amp")) * dblQuantity);
                            //    double.TryParse(oModelsProperties.Get(intModel, "replicate_times"), out dblReplicate);
                            //    lblAmp.Text = dblAmpTemp.ToString("N");
                            //    dblAmp += dblAmpTemp;
                            //}
                            strModel = oModel.Get(intModel, "name");
                        }
                        else
                        {
                            DataSet dsVendor = oForecast.GetAnswer(Int32.Parse(lblId.Text));
                            if (dsVendor.Tables[0].Rows.Count > 0 && dsVendor.Tables[0].Rows[0]["modelname"].ToString() != "")
                            {
                                double dblAmpTemp = (double.Parse(dsVendor.Tables[0].Rows[0]["amp"].ToString()) * dblQuantity);
                                lblAmp.Text = dblAmpTemp.ToString("N");
                                dblAmp += dblAmpTemp;
                                strModel = dsVendor.Tables[0].Rows[0]["modelname"].ToString();
                            }
                        }
                        if (intModel == 0)
                        {
                            lblModel.Text = "Solution Unavailable";
                            lblModel.CssClass = "reddefault";
                        }
                        else
                        {
                            if (intModel > 0)
                                intType = oModel.GetType(intModel);
                            string strPDF = oModel.Get(intModel, "pdf");
                            if (strPDF != "")
                                lblModel.Text = "<a href=\"javascript:void(0);\" onclick=\"OpenNewWindowMAX('" + strPDF + "');\">" + strModel + "</a>";
                            else
                                lblModel.Text = strModel;
                        }
                        if (dblComplete.ToString().Contains("100") == true && oConfidence.Get(Int32.Parse(oForecast.GetAnswer(Int32.Parse(lblId.Text), "confidenceid")), "name").ToUpper().Contains("100") == true)
                        {
                            int intRequestID = 0;
                            if (lblRequestId.Text != "")
                                intRequestID = Int32.Parse(lblRequestId.Text);
                            if (lblCommitment.Text == DateTime.Today.ToShortDateString() || (lblCommitment.Text != "" && DateTime.Parse(lblCommitment.Text) < DateTime.Today) || intRequestID > 0)
                            {
                                if (intModel == 0)
                                    oExecute.Attributes.Add("onclick", "alert('This design has not found a solution based on the requirements you have provided.\\nPlease verify that your information is correct.\\n\\nIf you have validated this information, please contact your Design Implementor to find an alternative solution.');return false;");
                                else
                                {
                                    string strExecute = oType.Get(intType, "forecast_execution_path");
                                    if (strExecute != "")
                                        oExecute.Attributes.Add("onclick", "return OpenWindow('FORECAST_EXECUTE','" + strExecute + "?id=" + lblId.Text + "');");
                                    else
                                        oExecute.Attributes.Add("onclick", "alert('Execution has not been configured for asset type " + oType.Get(intType, "name") + "');return false;");
                                }
                            }
                            else
                                oExecute.Enabled = false;
                        }
                        else
                            oExecute.Enabled = false;
                    }
                    Label lblHours = (Label)ri.FindControl("lblHours");
                    double dblH = 0.00;
                    dblHours += dblH;
                    lblHours.Text = dblH.ToString("N");
                    Label lblAcquisition = (Label)ri.FindControl("lblAcquisition");
                    double dblA = 0.00;
                    DataSet dsA = oForecast.GetAcquisitions(intModel, 1);
                    foreach (DataRow drA in dsA.Tables[0].Rows)
                        dblA += double.Parse(drA["cost"].ToString()) * dblQuantity;
                    dblAT += dblA;
                    lblAcquisition.Text = "<a href=\"javascript:void(0);\" onclick=\"OpenNewWindow('/frame/forecast/forecast_print_acquisition.aspx?id=" + lblId.Text + "',400,300);\">$" + dblA.ToString("N") + "</a>";
                    Label lblOperational = (Label)ri.FindControl("lblOperational");
                    double dblO = 0.00;
                    DataSet dsO = oForecast.GetOperations(intModel, 1);
                    foreach (DataRow drO in dsO.Tables[0].Rows)
                        dblO += double.Parse(drO["cost"].ToString()) * dblQuantity;
                    dblOT += dblO;
                    lblOperational.Text = "<a href=\"javascript:void(0);\" onclick=\"OpenNewWindow('/frame/forecast/forecast_print_operational.aspx?id=" + lblId.Text + "',400,300);\">$" + dblO.ToString("N") + "</a>";
                    // STORAGE
                    DataSet dsStorage = oForecast.GetStorage(Int32.Parse(lblId.Text));
                    double dblStorage = 0.00;
                    if (dsStorage.Tables[0].Rows.Count > 0)
                    {
                        double dblHigh = double.Parse(dsStorage.Tables[0].Rows[0]["high_total"].ToString()) + double.Parse(dsStorage.Tables[0].Rows[0]["high_qa"].ToString()) + double.Parse(dsStorage.Tables[0].Rows[0]["high_test"].ToString()) + (double.Parse(dsStorage.Tables[0].Rows[0]["high_replicated"].ToString()) * dblReplicate) + double.Parse(dsStorage.Tables[0].Rows[0]["high_ha"].ToString());
                        double dblStandard = double.Parse(dsStorage.Tables[0].Rows[0]["standard_total"].ToString()) + double.Parse(dsStorage.Tables[0].Rows[0]["standard_qa"].ToString()) + double.Parse(dsStorage.Tables[0].Rows[0]["standard_test"].ToString()) + (double.Parse(dsStorage.Tables[0].Rows[0]["standard_replicated"].ToString()) * dblReplicate) + double.Parse(dsStorage.Tables[0].Rows[0]["standard_ha"].ToString());
                        double dblLow = double.Parse(dsStorage.Tables[0].Rows[0]["low_total"].ToString()) + double.Parse(dsStorage.Tables[0].Rows[0]["low_qa"].ToString()) + double.Parse(dsStorage.Tables[0].Rows[0]["low_test"].ToString()) + (double.Parse(dsStorage.Tables[0].Rows[0]["low_replicated"].ToString()) * dblReplicate) + double.Parse(dsStorage.Tables[0].Rows[0]["low_ha"].ToString());
                        //if (dsStorage.Tables[0].Rows[0]["high_level"].ToString().ToUpper() == "HIGH")
                        //    dblHigh = dblHigh * 2;
                        //if (dsStorage.Tables[0].Rows[0]["standard_level"].ToString().ToUpper() == "HIGH")
                        //    dblStandard = dblStandard * 2;
                        //if (dsStorage.Tables[0].Rows[0]["low_level"].ToString().ToUpper() == "HIGH")
                        //    dblLow = dblLow * 2;
                        dblStorage = dblHigh + dblStandard + dblLow;
                        dblStorageTotal += dblStorage;
                    }
                    Label lblStorage = (Label)ri.FindControl("lblStorage");
                    lblStorage.Text = "<a href=\"javascript:void(0);\" onclick=\"OpenNewWindow('/frame/forecast/forecast_print_storage.aspx?id=" + lblId.Text + "',650,200);\">" + dblStorage.ToString() + " GB</a>";
                }
                lblQuantityTotal.Text = dblQT.ToString();
                lblStorageTotal.Text = dblStorageTotal.ToString() + " GB";
                lblAmpTotal.Text = dblAmp.ToString("N");
                lblHoursTotal.Text = dblHours.ToString("N");
                lblAcquisitionTotal.Text = "$" + dblAT.ToString("N");
                lblOperationalTotal.Text = "$" + dblOT.ToString("N");
                lblNone.Visible = (rptAll.Items.Count == 0);

                LoadStatus(intResourceWorkflow);
                LoadInformation(intResourceWorkflow);

                btnDenied.Attributes.Add("onclick", "return CloseWindow();");
                oFunction.ConfigureToolButton(btnSave, "/images/tool_save");
                oFunction.ConfigureToolButton(btnPrint, "/images/tool_print");
                btnPrint.Attributes.Add("onclick", "return PrintWindow();");
                oFunction.ConfigureToolButton(btnClose, "/images/tool_close");
                btnClose.Attributes.Add("onclick", "return ExitWindow();");
                btnSave.Attributes.Add("onclick", "return (document.getElementById('divTab4').style.display == \"inline\" && ValidateStatus('" + ddlStatus.ClientID + "','" + txtComments.ClientID + "'))" +
                        "|| ((document.getElementById('divTab2').style.display == \"inline\")" +
                        ");");

                drpInvolvement.Attributes.Add("onchange", "ShowInvolvementReason(this,'" + divInvolvement.ClientID + "');");
                btnFilter.Attributes.Add("onclick", "return OpenWindow('FORECAST_FILTER','');");
                btnNew.Attributes.Add("onclick", "return OpenWindow('FORECAST_EQUIPMENT','?parent=" + intID.ToString() + "&id=0');");
                btnDeleteForecast.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this design?');");


                // My code start - Vijay    

                oVariable = new Variables(intEnvironment);
                btnAddRes.Attributes.Add("onclick", "return EditResource('" + intRequest + "','" + intItem + "','" + intNumber + "','0');");
                btnAddAsset.Attributes.Add("onclick", "return EditAsset('" + intRequest + "','" + intItem + "','" + intNumber + "','0');");
                imgDate.Attributes.Add("onclick", "return ShowCalendar('" + txtDateEngaged.ClientID + "');");
                imgFollowUpDate.Attributes.Add("onclick", "return ShowCalendar('" + txtFollowupDate.ClientID + "');");

                rptAssets.DataSource = oCustom.GetTechAssets(intRequest, intItem, intNumber);
                rptAssets.DataBind();
                rptResource.DataSource = oCustom.GetResourceAssignments(intRequest, intItem, intNumber);
                rptResource.DataBind();

                lblNoAsset.Visible = rptAssets.Items.Count == 0;
                lblNoRes.Visible = rptResource.Items.Count == 0;

                foreach (RepeaterItem ri in rptAssets.Items)
                {
                    Panel panEdit = (Panel)ri.FindControl("panEditable");
                    LinkButton btnDelete = (LinkButton)panEdit.FindControl("btnDelete");

                    btnDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this asset?');");
                    panEdit.Visible = true;
                }



                foreach (RepeaterItem ri in rptResource.Items)
                {
                    id = Int32.Parse(((Label)ri.FindControl("lblResID")).Text);
                    Label lblResourceType = (Label)ri.FindControl("lblResourceType");
                    Label lblRequestedDate = (Label)ri.FindControl("lblRequestedDate");
                    Label lblFulfillDate = (Label)ri.FindControl("lblFulfillDate");
                    lblRequestedDate.Text = Convert.ToDateTime(lblRequestedDate.Text).ToShortDateString();
                    lblFulfillDate.Text = Convert.ToDateTime(lblFulfillDate.Text).ToShortDateString();
                    lblResourceType.Text = oCustom.GetResourceTypeName(id, "name");
                    Panel panEdit = (Panel)ri.FindControl("panEditable");
                    LinkButton btnDelete = (LinkButton)panEdit.FindControl("btnDelete");
                    btnDelete.Attributes.Add("onclick", " return confirm('Are you sure you want to delete this resource?') ;");
                    panEdit.Visible = true;
                }

                txtInvestigatedBy.Attributes.Add("onkeyup", "return AJAXTextBoxGet(this,'300','195','" + divAJAX.ClientID + "','" + lstAJAX.ClientID + "','" + hdnInvestigatedBy.ClientID + "','" + oVariable.URL() + "/frame/users.aspx',2);");
                lstAJAX.Attributes.Add("ondblclick", "AJAXClickRow();");
                txtIDCSPOC.Attributes.Add("onkeyup", "return AJAXTextBoxGet(this,'300','195','" + divIDCSPOC.ClientID + "','" + lstIDCSPOC.ClientID + "','" + hdnIDCSPOC.ClientID + "','" + oVariable.URL() + "/frame/users.aspx',2);");
                lstIDCSPOC.Attributes.Add("ondblclick", "AJAXClickRow();");

                // My code end - Vijay    
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
                Label _comments = (Label)ri.FindControl("lblComments");
                if (_comments.Text.Length > 100)
                    _comments.Text = _comments.Text.Substring(0, 100) + "......";
            }
            lblStatus.Text = oResourceRequest.GetStatus(dblTotalStatus, 50, 15);
        }
        private void LoadInformation(int _request)
        {
            StringBuilder sb = new StringBuilder(strTabs);

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
                    case "I":
                        boolIDC = true;
                        break;
                    case "S":
                        boolDesigns = true;
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
                }
                hdnTab.Value = Request.QueryString["div"];
            }
            if (boolDetails == false && boolExecution == false && boolIDC == false && boolDesigns == false && boolDocuments == false && boolMyDocuments == false && boolResource == false)
            {
                boolDetails = true;
            }

            if (boolDetails == true)
            {
                sb.Append("<td><img src=\"/images/TabOnLeftCap.gif\"></td><td nowrap background=\"/images/TabOnBackground.gif\"><a href=\"javascript:void(0);\" onclick=\"ChangeTab(this,'divTab1','");
                sb.Append(hdnTab.ClientID);
                sb.Append("','XXX',true);\" class=\"tabheader\">Project Details</a></td><td><img src=\"/images/TabOnRightCap.gif\"></td>");
            }
            else
            {
                sb.Append("<td><img src=\"/images/TabOffLeftCap.gif\"></td><td nowrap background=\"/images/TabOffBackground.gif\"><a href=\"javascript:void(0);\" onclick=\"ChangeTab(this,'divTab1','");
                sb.Append(hdnTab.ClientID);
                sb.Append("','XXX',true);\" class=\"tabheader\">Project Details</a></td><td><img src=\"/images/TabOffRightCap.gif\"></td>");
            }

            if (boolIDC == true)
            {
                sb.Append("<td><img src=\"/images/TabOnLeftCap.gif\"></td><td nowrap background=\"/images/TabOnBackground.gif\"><a href=\"javascript:void(0);\" onclick=\"ChangeTab(this,'divTab2','");
                sb.Append(hdnTab.ClientID);
                sb.Append("','I',true);\" class=\"tabheader\">Investigation</a></td><td><img src=\"/images/TabOnRightCap.gif\"></td>");
            }
            else
            {
                sb.Append("<td><img src=\"/images/TabOffLeftCap.gif\"></td><td nowrap background=\"/images/TabOffBackground.gif\"><a href=\"javascript:void(0);\" onclick=\"ChangeTab(this,'divTab2','");
                sb.Append(hdnTab.ClientID);
                sb.Append("','I',true);\" class=\"tabheader\">Investigation</a></td><td><img src=\"/images/TabOffRightCap.gif\"></td>");
            }

            if (boolDesigns == true)
            {
                sb.Append("<td><img src=\"/images/TabOnLeftCap.gif\"></td><td nowrap background=\"/images/TabOnBackground.gif\"><a href=\"javascript:void(0);\" onclick=\"ChangeTab(this,'divTab3','");
                sb.Append(hdnTab.ClientID);
                sb.Append("','S',true);\" class=\"tabheader\">Designs</a></td><td><img src=\"/images/TabOnRightCap.gif\"></td>");
            }
            else
            {
                sb.Append("<td><img src=\"/images/TabOffLeftCap.gif\"></td><td nowrap background=\"/images/TabOffBackground.gif\"><a href=\"javascript:void(0);\" onclick=\"ChangeTab(this,'divTab3','");
                sb.Append(hdnTab.ClientID);
                sb.Append("','S',true);\" class=\"tabheader\">Designs</a></td><td><img src=\"/images/TabOffRightCap.gif\"></td>");
            }

            if (boolExecution == true)
            {
                sb.Append("<td><img src=\"/images/TabOnLeftCap.gif\"></td><td nowrap background=\"/images/TabOnBackground.gif\"><a href=\"javascript:void(0);\" onclick=\"ChangeTab(this,'divTab4','");
                sb.Append(hdnTab.ClientID);
                sb.Append("','E',true);\" class=\"tabheader\">Status Updates</a></td><td><img src=\"/images/TabOnRightCap.gif\"></td>");
            }
            else
            {
                sb.Append("<td><img src=\"/images/TabOffLeftCap.gif\"></td><td nowrap background=\"/images/TabOffBackground.gif\"><a href=\"javascript:void(0);\" onclick=\"ChangeTab(this,'divTab4','");
                sb.Append(hdnTab.ClientID);
                sb.Append("','E',true);\" class=\"tabheader\">Status Updates</a></td><td><img src=\"/images/TabOffRightCap.gif\"></td>");
            }

            if (boolMyDocuments == true)
            {
                sb.Append("<td><img src=\"/images/TabOnLeftCap.gif\"></td><td nowrap background=\"/images/TabOnBackground.gif\"><a href=\"javascript:void(0);\" onclick=\"ChangeTab(this,'divTab6','");
                sb.Append(hdnTab.ClientID);
                sb.Append("','M',true);\" class=\"tabheader\">My Documents</a></td><td><img src=\"/images/TabOnRightCap.gif\"></td>");
            }
            else
            {
                sb.Append("<td><img src=\"/images/TabOffLeftCap.gif\"></td><td nowrap background=\"/images/TabOffBackground.gif\"><a href=\"javascript:void(0);\" onclick=\"ChangeTab(this,'divTab6','");
                sb.Append(hdnTab.ClientID);
                sb.Append("','M',true);\" class=\"tabheader\">My Documents</a></td><td><img src=\"/images/TabOffRightCap.gif\"></td>");
            }

            if (boolDocuments == true)
            {
                sb.Append("<td><img src=\"/images/TabOnLeftCap.gif\"></td><td nowrap background=\"/images/TabOnBackground.gif\"><a href=\"javascript:void(0);\" onclick=\"ChangeTab(this,'divTab7','");
                sb.Append(hdnTab.ClientID);
                sb.Append("','D',true);\" class=\"tabheader\">Project Documents</a></td><td><img src=\"/images/TabOnRightCap.gif\"></td>");
            }
            else
            {
                sb.Append("<td><img src=\"/images/TabOffLeftCap.gif\"></td><td nowrap background=\"/images/TabOffBackground.gif\"><a href=\"javascript:void(0);\" onclick=\"ChangeTab(this,'divTab7','");
                sb.Append(hdnTab.ClientID);
                sb.Append("','D',true);\" class=\"tabheader\">Project Documents</a></td><td><img src=\"/images/TabOffRightCap.gif\"></td>");
            }

            if (boolResource == true)
            {
                sb.Append("<td><img src=\"/images/TabOnLeftCap.gif\"></td><td nowrap background=\"/images/TabOnBackground.gif\"><a href=\"javascript:void(0);\" onclick=\"ChangeTab(this,'divTab8','");
                sb.Append(hdnTab.ClientID);
                sb.Append("','R',true);\" class=\"tabheader\">Resource Involvement</a></td><td><img src=\"/images/TabOnRightCap.gif\"></td>");
            }
            else
            {
                sb.Append("<td><img src=\"/images/TabOffLeftCap.gif\"></td><td nowrap background=\"/images/TabOffBackground.gif\"><a href=\"javascript:void(0);\" onclick=\"ChangeTab(this,'divTab8','");
                sb.Append(hdnTab.ClientID);
                sb.Append("','R',true);\" class=\"tabheader\">Resource Involvement</a></td><td><img src=\"/images/TabOffRightCap.gif\"></td>");
            }

            strTabs = sb.ToString();
        }
        protected void btnSave_Click(Object Sender, EventArgs e)
        {
            int intResourceWorkflow = Int32.Parse(lblResourceWorkflow.Text);
            int intResourceParent = oResourceRequest.GetWorkflowParent(intResourceWorkflow);
            switch (Request.Form[hdnTab.UniqueID])
            {
                case "E":
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
                    oResourceRequest.UpdateWorkflowName(intResourceWorkflow, txtCustom.Text, intProfile);
                    if (dblHours > 0.00)
                        oResourceRequest.UpdateWorkflowHours(intResourceWorkflow, dblHours);
                    // Add Slider Stuff
                    double dblSlide1OLD = 0.00;
                    double dblSlide2OLD = 0.00;
                    double dblSlide3OLD = 0.00;
                    double dblSlide4OLD = 0.00;
                    double dblSlide5OLD = 0.00;
                    DataSet dsOLD = oCustom.GetIDCDetails(intRequest, intItem, intNumber);
                    if (dsOLD.Tables[0].Rows.Count > 0)
                    {
                        dblSlide1OLD = double.Parse(dsOLD.Tables[0].Rows[0]["slide_statement"].ToString());
                        dblSlide2OLD = double.Parse(dsOLD.Tables[0].Rows[0]["slide_alternatives"].ToString());
                        dblSlide3OLD = double.Parse(dsOLD.Tables[0].Rows[0]["slide_recommendations"].ToString());
                        dblSlide4OLD = double.Parse(dsOLD.Tables[0].Rows[0]["slide_high_level"].ToString());
                        dblSlide5OLD = double.Parse(dsOLD.Tables[0].Rows[0]["slide_detailed"].ToString());
                    }
                    double dblSlide1 = 0.00;
                    double dblSlide2 = 0.00;
                    double dblSlide3 = 0.00;
                    double dblSlide4 = 0.00;
                    double dblSlide5 = 0.00;
                    if (Request.Form["hdnSlide1"] != null && Request.Form["hdnSlide1"] != "")
                        dblSlide1 += double.Parse(Request.Form["hdnSlide1"]);
                    if (Request.Form["hdnSlide2"] != null && Request.Form["hdnSlide2"] != "")
                        dblSlide2 += double.Parse(Request.Form["hdnSlide2"]);
                    if (Request.Form["hdnSlide3"] != null && Request.Form["hdnSlide3"] != "")
                        dblSlide3 += double.Parse(Request.Form["hdnSlide3"]);
                    if (Request.Form["hdnSlide4"] != null && Request.Form["hdnSlide4"] != "")
                        dblSlide4 += double.Parse(Request.Form["hdnSlide4"]);
                    if (Request.Form["hdnSlide5"] != null && Request.Form["hdnSlide5"] != "")
                        dblSlide5 += double.Parse(Request.Form["hdnSlide5"]);
                    string strEmail = "";
                    DataSet dsManagers = oService.GetUser(intService, 1);
                    foreach (DataRow drManager in dsManagers.Tables[0].Rows)
                        strEmail += oUser.GetName(Int32.Parse(drManager["userid"].ToString())) + ";";


                    strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_PROJECT");
                    if (dblSlide1 != dblSlide1OLD)
                    {   
                        if (dblSlide1 == 1.00)
                            oFunction.SendEmail("Deliverable Completed", strEmail, "", strEMailIdsBCC, "Deliverable Completed", "<p>This message is sent to inform you that <b>" + oUser.GetFullName(intProfile) + "</b> has successfully completed the &quot;Statement of Work&quot; Deliverable for the following project...</p><p>" + oProject.GetBody(intProject, intEnvironment, false) + "</p>", true, false);
                        oResourceRequest.AddStatus(intResourceWorkflow, Int32.Parse(ddlStatus.SelectedItem.Value), "Statement of Work (%): " + dblSlide1.ToString("P"), intProfile);
                    }
                    if (dblSlide2 != dblSlide2OLD)
                    {
                        if (dblSlide2 == 1.00)
                            oFunction.SendEmail("Deliverable Completed", strEmail, "", strEMailIdsBCC, "Deliverable Completed", "<p>This message is sent to inform you that <b>" + oUser.GetFullName(intProfile) + "</b> has successfully completed the &quot;Solution Alternatives&quot; Deliverable for the following project...</p><p>" + oProject.GetBody(intProject, intEnvironment, false) + "</p>", true, false);
                        oResourceRequest.AddStatus(intResourceWorkflow, Int32.Parse(ddlStatus.SelectedItem.Value), "Solution Alternatives (%): " + dblSlide2.ToString("P"), intProfile);
                    }
                    if (dblSlide3 != dblSlide3OLD)
                    {
                        if (dblSlide3 == 1.00)
                            oFunction.SendEmail("Deliverable Completed", strEmail, "", strEMailIdsBCC, "Deliverable Completed", "<p>This message is sent to inform you that <b>" + oUser.GetFullName(intProfile) + "</b> has successfully completed the &quot;Solution Recommendation&quot; Deliverable for the following project...</p><p>" + oProject.GetBody(intProject, intEnvironment, false) + "</p>", true, false);
                        oResourceRequest.AddStatus(intResourceWorkflow, Int32.Parse(ddlStatus.SelectedItem.Value), "Solution Recommendation (%): " + dblSlide3.ToString("P"), intProfile);
                    }
                    if (dblSlide4 != dblSlide4OLD)
                    {
                        if (dblSlide4 == 1.00)
                            oFunction.SendEmail("Deliverable Completed", strEmail, "", strEMailIdsBCC, "Deliverable Completed", "<p>This message is sent to inform you that <b>" + oUser.GetFullName(intProfile) + "</b> has successfully completed the &quot;High Level Design&quot; Deliverable for the following project...</p><p>" + oProject.GetBody(intProject, intEnvironment, false) + "</p>", true, false);
                        oResourceRequest.AddStatus(intResourceWorkflow, Int32.Parse(ddlStatus.SelectedItem.Value), "High Level Design (%): " + dblSlide4.ToString("P"), intProfile);
                    }
                    if (dblSlide5 != dblSlide5OLD)
                    {
                        if (dblSlide5 == 1.00)
                            oFunction.SendEmail("Deliverable Completed", strEmail, "", strEMailIdsBCC, "Deliverable Completed", "<p>This message is sent to inform you that <b>" + oUser.GetFullName(intProfile) + "</b> has successfully completed the &quot;Detailed Design&quot; Deliverable for the following project...</p><p>" + oProject.GetBody(intProject, intEnvironment, false) + "</p>", true, false);
                        oResourceRequest.AddStatus(intResourceWorkflow, Int32.Parse(ddlStatus.SelectedItem.Value), "Detailed Design (%): " + dblSlide5.ToString("P"), intProfile);
                    }
                    oCustom.UpdateIDCDetails(intRequest, intItem, intNumber, dblSlide1, dblSlide2, dblSlide3, dblSlide4, dblSlide5);
                    Redirect(intProject, intResourceWorkflow, "E");
                    break;
                case "I":
                    int wm_id = 0;
                    int intInvestigatedBy = 0;
                    if (Request.Form[hdnInvestigatedBy.UniqueID] != "")
                        intInvestigatedBy = Int32.Parse(Request.Form[hdnInvestigatedBy.UniqueID]);
                    int intIDCSPOC = 0;
                    if (Request.Form[hdnIDCSPOC.UniqueID] != "")
                        intIDCSPOC = Int32.Parse(Request.Form[hdnIDCSPOC.UniqueID]);

                    wm_id = oCustom.AddIDCDetails(intRequest, intItem, intNumber, drpInvestigated.SelectedItem.Text, intInvestigatedBy, txtFollowupDate.Text, txtDateEngaged.Text, drpPhase.SelectedItem.Text, drpEffortSize.SelectedItem.Text, drpInvolvement.SelectedItem.Text, drpEIT.SelectedItem.Text, drpProjectClass.SelectedItem.Text, drpEnterprise.SelectedItem.Text, drpNoInvolve.SelectedItem.Text, intIDCSPOC, txtComment.Text);
                    Redirect(intProject, intResourceWorkflow, "I");
                    break;
            }
        }
        private void Redirect(int _projectid, int _resourceid, string _div)
        {
            if (intPage == 0)
                Response.Redirect(Request.Path + "?rrid=" + _resourceid.ToString() + "&div=" + _div + "&save=true");
            else
                Response.Redirect(oPage.GetFullLink(intPage) + "?pid=" + _projectid.ToString() + "&div=" + _div + "&save=true");
        }
        protected void btnComplete_Click(Object Sender, EventArgs e)
        {
            int intResourceWorkflow = Int32.Parse(lblResourceWorkflow.Text);
            int intResourceParent = oResourceRequest.GetWorkflowParent(intResourceWorkflow);
            oResourceRequest.UpdateWorkflowStatus(intResourceWorkflow, 3, true);
            oResourceRequest.CloseWorkflow(intResourceWorkflow, intEnvironment, 0, dsnServiceEditor, true, intResourceRequestApprove, intAssignPage, intViewPage, dsnAsset, dsnIP);
            Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "reload", "<script type=\"text/javascript\">RefreshOpeningWindow();window.close();<" + "/" + "script>");
        }

        // Vijay code - Start

        protected void btnDelete_Click(object Sender, EventArgs e)
        {
            LinkButton oButton = (LinkButton)Sender;
            int intId = Int32.Parse(oButton.CommandArgument);
            if (oButton.CommandName == "Asset")
                oCustom.DeleteTechAsset(intId);
            else
                oCustom.DeleteResourceAssignment(intId);

            Redirect(intProject, intResourceWorkflow, "I");
            //string path = "";         
            //path = Request.Url.AbsoluteUri.Contains("&div=I") == true ? Request.Url.AbsoluteUri : Request.Url.AbsoluteUri + "&div=I";
            //Response.Redirect(path);

            //    Response.Redirect(Request.UrlReferrer.AbsoluteUri.Replace("&div=I","")+"&div=I");         
        }

        // Vijay code - End

        protected void chkMyDescription_Change(Object Sender, EventArgs e)
        {
            if (chkMyDescription.Checked == true)
                Response.Redirect(oPage.GetFullLink(intPage) + "?pid=" + Request.QueryString["pid"] + "&mydoc=true&div=M");
            else
                Response.Redirect(oPage.GetFullLink(intPage) + "?pid=" + Request.QueryString["pid"] + "&div=M");
        }
        protected void chkDescription_Change(Object Sender, EventArgs e)
        {
            if (chkDescription.Checked == true)
                Response.Redirect(oPage.GetFullLink(intPage) + "?pid=" + Request.QueryString["pid"] + "&doc=true&div=D");
            else
                Response.Redirect(oPage.GetFullLink(intPage) + "?pid=" + Request.QueryString["pid"] + "&div=D");
        }
        protected void btnMessage_Click(Object Sender, EventArgs e)
        {
            int intUser = Int32.Parse(ddlResource.SelectedItem.Value);
            Functions oFunction = new Functions(intProfile, dsn, intEnvironment);
            strEMailIdsBCC = "";
            if (ddlCommunication.SelectedItem.Value.ToUpper() == "EMAIL")
            {
                string strEmail = oUser.GetName(intUser);
               
                oFunction.SendEmail("", strEmail, oUser.GetName(intProfile), strEMailIdsBCC, "ClearView Communication from " + oUser.GetFullName(intProfile), oProject.GetBody(intProject, intEnvironment, false) + "<table width=\"100%\" border=\"0\" cellpadding=\"2\" cellspacing=\"1\"><tr><td><span style=\"width:100%;border-bottom:1 dotted #999999;\"/></td></tr></table>" + txtMessage.Text, true, false);
            }
            else
            {
                string strPager = oUser.Get(intUser, "pager") + "@archwireless.net";
                oFunction.SendEmail("", strPager, oUser.GetName(intProfile), strEMailIdsBCC, "ClearView Communication from " + oUser.GetFullName(intProfile), txtMessage.Text, false, true);
            }
            Response.Redirect(oPage.GetFullLink(intPage) + "?pid=" + intProject.ToString() + "&comm=sent&div=R");
        }
        protected void btnDeleteForecast_Click(Object Sender, EventArgs e)
        {
            oForecast.Delete(intID);
            DataSet ds = oForecast.GetAnswers(intID);
            OnDemandTasks oOnDemandTask = new OnDemandTasks(intProfile, dsn);
            ResourceRequest oResourceRequest = new ResourceRequest(intProfile, dsn);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                int intAnswer = Int32.Parse(dr["id"].ToString());
                oForecast.DeleteAnswer(intAnswer);
                // Delete any resource requests
                DataSet dsResources = oOnDemandTask.GetPending(intAnswer);
                foreach (DataRow drResource in dsResources.Tables[0].Rows)
                {
                    int intRR = Int32.Parse(drResource["resourceid"].ToString());
                    if (oResourceRequest.GetWorkflow(intRR, "status") != "3")
                        oResourceRequest.DeleteWorkflow(intRR);
                    oOnDemandTask.DeletePending(intAnswer);
                }
            }
            Response.Redirect(oPage.GetFullLink(intPage) + "?pid=" + intProject.ToString());
        }
        protected void btnClear_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(oPage.GetFullLink(intPage) + "?pid=" + intProject.ToString());
        }
    }
}