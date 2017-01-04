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
    public partial class wm_backup : System.Web.UI.UserControl
    {

        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
        private string dsnZeus = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["ZeusDSN"]].ConnectionString;
        private string dsnServiceEditor = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["ServiceEditorDSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected string strVirtual = ConfigurationManager.AppSettings["VirtualGatekeeper"];
        protected int intAssignPage = Int32.Parse(ConfigurationManager.AppSettings["AssignWorkload"]);
        protected int intViewPage = Int32.Parse(ConfigurationManager.AppSettings["ViewWorkload"]);
        protected int intResourceRequestApprove = Int32.Parse(ConfigurationManager.AppSettings["ResourceRequestApprove"]);
        protected int intMyWork = Int32.Parse(ConfigurationManager.AppSettings["MyWork"]);
        protected int intImplementorDistributed = Int32.Parse(ConfigurationManager.AppSettings["ITEMID_IMPLEMENTOR_DISTRIBUTED"]);
        protected int intImplementorMidrange = Int32.Parse(ConfigurationManager.AppSettings["ITEMID_IMPLEMENTOR_MIDRANGE"]);
        protected int intWorkstationPlatform = Int32.Parse(ConfigurationManager.AppSettings["WorkstationPlatformID"]);
        protected int intUnder48A = Int32.Parse(ConfigurationManager.AppSettings["DR_HOUR_RESPONSE"]);
        protected int intUnder48Q = Int32.Parse(ConfigurationManager.AppSettings["DR_HOUR_QUESTION"]);
        protected bool boolUsePNCNaming = (ConfigurationManager.AppSettings["USE_PNC_NAMING"] == "1");
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
        protected OnDemandTasks oOnDemandTasks;
        protected Delegates oDelegate;
        protected int intApplication = 0;
        protected int intPage = 0;
        protected int intProject = 0;
        protected bool boolDetails = false;
        protected bool boolExecution = false;
        protected int intRequest = 0;
        protected int intItem = 0;
        protected int intNumber = 0;
        protected ServiceRequests oServiceRequest;
        protected int intWorkloadPage = Int32.Parse(ConfigurationManager.AppSettings["ViewWorkload"]);
        protected bool boolMove = false;
        protected string strValidation = "";
        protected Locations oLocation;
        protected TSM oTSM;
        protected string strBackup = "";
        protected string strBackupHeader = "";
        protected string strPreview = "";
        protected string strHiddenV = "";
        protected Servers oServer;
        protected Variables oVariable;
        protected ServerName oServerName;
        protected Settings oSetting;
        protected Design oDesign;
        protected string strTSMTeam = ConfigurationManager.AppSettings["TSM_TEAM"];
        protected string strTSMTeamTest = ConfigurationManager.AppSettings["TSM_TEAM_TEST"];
        protected string strTSMTeamProd = ConfigurationManager.AppSettings["TSM_TEAM_PROD"];
        protected bool boolProduction = false;



        // Vijay code - start
        protected bool boolBackupInclusion = false;
        protected bool boolBackupExclusion = false;
        protected bool boolArchiveRequirement = false;
        protected bool boolAdditionalConfiguration = false;
        protected bool boolStatusUpdates = false;
        // Vijay code - end
        private string strEMailIdsBCC = "";
        protected StringBuilder strBackupCFI;
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
            oServiceRequest = new ServiceRequests(intProfile, dsn);
            oApplication = new Applications(intProfile, dsn);
            oOnDemandTasks = new OnDemandTasks(intProfile, dsn);
            oDelegate = new Delegates(intProfile, dsn);
            oLocation = new Locations(intProfile, dsn);
            oTSM = new TSM(intProfile, dsn);
            oServer = new Servers(intProfile, dsn);
            oVariable = new Variables(intEnvironment);
            oServerName = new ServerName(intProfile, dsn);
            oSetting = new Settings(intProfile, dsn);
            oDesign = new Design(intProfile, dsn);
            if (Request.QueryString["applicationid"] != null && Request.QueryString["applicationid"] != "")
                intApplication = Int32.Parse(Request.QueryString["applicationid"]);
            if (Request.QueryString["pageid"] != null && Request.QueryString["pageid"] != "")
                intPage = Int32.Parse(Request.QueryString["pageid"]);
            if (Request.Cookies["application"] != null && Request.Cookies["application"].Value != "")
                intApplication = Int32.Parse(Request.Cookies["application"].Value);
            if (Request.QueryString["rrid"] != null && Request.QueryString["rrid"] != "")
            {
                lblResourceWorkflow.Text = Request.QueryString["rrid"];
                int intResourceWorkflow = Int32.Parse(Request.QueryString["rrid"]);
                int intResourceParent = oResourceRequest.GetWorkflowParent(intResourceWorkflow);
                DataSet ds = oResourceRequest.Get(intResourceParent);
                intRequest = Int32.Parse(ds.Tables[0].Rows[0]["requestid"].ToString());
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

                double dblUsed = oResourceRequest.GetWorkflowUsed(intResourceWorkflow);
                lblUpdated.Text = oResourceRequest.GetUpdated(intResourceParent);
                dblUsed = (dblUsed / dblAllocated) * 100;
                if (Request.QueryString["refresh"] != null && Request.QueryString["refresh"] != "")
                    Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "refreshed", "<script type=\"text/javascript\">eval(parent.location = parent.location + '#" + Request.QueryString["refresh"] + "');<" + "/" + "script>");
                if (Request.QueryString["save"] != null && Request.QueryString["save"] != "")
                    Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "saved", "<script type=\"text/javascript\">alert('Information Saved Successfully');<" + "/" + "script>");
                if (Request.QueryString["status"] != null && Request.QueryString["status"] != "")
                    Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "statusd", "<script type=\"text/javascript\">alert('Weekly Status has been Added');<" + "/" + "script>");
                intProject = oRequest.GetProjectNumber(intRequest);
                hdnTab.Value = "D";
                panWorkload.Visible = true;
                bool boolDone = LoadInformation(intResourceWorkflow);
                if (boolDone == true)
                {
                    if (boolComplete == false)
                    {
                        oFunction.ConfigureToolButton(btnComplete, "/images/tool_complete");
                        btnComplete.Attributes.Add("onclick", "return confirm('Are you sure you want to mark this as completed and remove it from your workload?') && ProcessControlButton();");
                    }
                    else
                    {
                        btnSave.ImageUrl = "/images/tool_save_dbl.gif";
                        btnSave.Enabled = false;
                        btnCancel.Enabled = false;
                        btnCancelConfirm.Enabled = false;
                        oFunction.ConfigureToolButton(btnComplete, "/images/tool_complete");
                        btnComplete.Attributes.Add("onclick", "alert('This task has already been marked COMPLETE. You can close this window.');return false;");
                    }
                }
                else
                {
                    btnComplete.ImageUrl = "/images/tool_complete_dbl.gif";
                    btnComplete.Enabled = false;
                }
                btnDenied.Attributes.Add("onclick", "return CloseWindow();");
                oFunction.ConfigureToolButton(btnSave, "/images/tool_save");
                oFunction.ConfigureToolButton(btnPrint, "/images/tool_print");
                btnPrint.Attributes.Add("onclick", "return PrintWindow();");
                oFunction.ConfigureToolButton(btnCancel, "/images/tool_cancel");
                btnCancelConfirm.Attributes.Add("onclick", "return ValidateText('" + txtCancel.ClientID + "','Please enter a reason') && confirm('Are you sure you want to CANCEL this request?');");
                btnCancel.Attributes.Add("onclick", "ShowHideDiv2('" + divCancel.ClientID + "');return false;");
                oFunction.ConfigureToolButton(btnClose, "/images/tool_close");
                btnClose.Attributes.Add("onclick", "return ExitWindow();");
                btnSave.Attributes.Add("onclick", "return EnsureBackupOK('" + chk1.ClientID + "','" + strValidation + "') && ProcessControlButton();");
                btnStatus.Attributes.Add("onclick", "return ValidateStatus('" + ddlStatus.ClientID + "','" + txtComments.ClientID + "')" +
                                        ";");
                btnRefresh.Attributes.Add("onclick", "return ProcessButton(this);");
                bool boolRed = LoadStatus(intResourceWorkflow);
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
        private bool LoadInformation(int _request_workflow)
        {
            if (intProject > 0)
            {
                lblName.Text = oProject.Get(intProject, "name");
                lblNumber.Text = oProject.Get(intProject, "number");
                lblType.Text = "Project";
            }
            else
            {
                lblName.Text = oResourceRequest.GetWorkflow(_request_workflow, "name");
                lblNumber.Text = "CVT" + intRequest.ToString();
                lblType.Text = "Task";
            }
            int intTSMAutomated = 0;
            Int32.TryParse(oSetting.Get("tsm_automated"), out intTSMAutomated);
            int intAvamarAutomated = 0;
            Int32.TryParse(oSetting.Get("avamar_automated"), out intAvamarAutomated);
            int intTSM = 0;
            int intAvamar = 0;
            bool boolDone = false;
            DataSet ds = oOnDemandTasks.GetServerBackup(intRequest, intItem, intNumber);
            if (ds.Tables[0].Rows.Count > 0)
            {
                int intAnswer = Int32.Parse(ds.Tables[0].Rows[0]["answerid"].ToString());
                Forecast oForecast = new Forecast(intProfile, dsn);
                ModelsProperties oModelsProperties = new ModelsProperties(intProfile, dsn);
                int intModel = oForecast.GetModelAsset(intAnswer);
                if (intModel == 0)
                    intModel = oForecast.GetModel(intAnswer);
                if (oModelsProperties.IsVMwareVirtual(intModel) == true)
                    panVirtual.Visible = true;
                btnView.Attributes.Add("onclick", "return OpenWindow('FORECAST_EQUIPMENT','?id=" + intAnswer.ToString() + "');");
                lblView.Text = oOnDemandTasks.GetBody(intAnswer, intImplementorDistributed, intImplementorMidrange);
                lblAnswer.Text = ds.Tables[0].Rows[0]["answerid"].ToString();
                lblModel.Text = ds.Tables[0].Rows[0]["modelid"].ToString();
                chk1.Checked = (ds.Tables[0].Rows[0]["chk1"].ToString() == "1");
                boolDone = (chk1.Checked);
                img1.ImageUrl = (boolDone ? "/images/check.gif" : "/images/green_arrow.gif");
                Servers oServer = new Servers(intProfile, dsn);
                Organizations oOrganization = new Organizations(0, dsn);
                Asset oAsset = new Asset(0, dsnAsset);
                IPAddresses oIPAddresses = new IPAddresses(0, dsnIP, dsn);
                ConsistencyGroups oConsistencyGroups = new ConsistencyGroups(0, dsn);
                Environments oEnvironment = new Environments(0, dsn);
                OperatingSystems oOperatingSystem = new OperatingSystems(0, dsn);
                Classes oClass = new Classes(0, dsn);
                Storage oStorage = new Storage(0, dsn);
                string strName = "";
                StringBuilder sbData = new StringBuilder();
                DataSet dsAnswer = oServer.GetAnswer(intAnswer);
                DataSet dsBackup = oForecast.GetBackup(intAnswer);
                if (dsAnswer.Tables[0].Rows.Count > 0)
                {
                    // FIX
                    bool _prod = false;
                    int intForecast = Int32.Parse(oForecast.GetAnswer(intAnswer, "forecastid"));
                    int intRequest2 = Int32.Parse(oForecast.Get(intForecast, "requestid"));
                    intProject = oRequest.GetProjectNumber(intRequest2);
                    int intClass = Int32.Parse(oForecast.GetAnswer(intAnswer, "classid"));
                    Int32.TryParse(oForecast.GetAnswer(intAnswer, "avamar"), out intAvamar);
                    lblAvamar.Text = intAvamar.ToString();
                    int intRecovery = 0;
                    if (dsBackup.Tables[0].Rows.Count > 0)
                        Int32.TryParse(dsBackup.Tables[0].Rows[0]["recoveryid"].ToString(), out intRecovery);
                    //Int32.TryParse(oLocation.GetAddress(intRecovery, "tsm"), out intTSM);
                    int intAddress = 0;
                    Int32.TryParse(oForecast.GetAnswer(intAnswer, "addressid"), out intAddress);
                    Int32.TryParse(oLocation.GetAddress(intAddress, "tsm"), out intTSM);
                    lblTSM.Text = intTSM.ToString();
                    int intEnv = Int32.Parse(oForecast.GetAnswer(intAnswer, "environmentid"));
                    int intModel2 = Int32.Parse(dsAnswer.Tables[0].Rows[0]["modelid"].ToString()); ;
                   
                    bool boolProd = false;
                    bool boolUnder = false;
                    if (oClass.IsProd(intClass) == false)
                    {
                        boolProduction = true;
                        if (_prod == false)
                        {
                            
                              
                        }
                        else
                            boolProd = true;
                        if (oForecast.GetAnswerPlatform(intAnswer, intUnder48Q, intUnder48A) == true)
                            boolUnder = true;
                    }
                    else
                    {
                       
                            
                    }
                    string strClass = "Test";
                    if (boolProd == true)
                        strClass = "Production";

                    int intImplementor = 0;
                    DataSet dsTasks = oOnDemandTasks.GetPending(intAnswer);
                    if (dsTasks.Tables[0].Rows.Count > 0)
                    {
                        intImplementor = Int32.Parse(dsTasks.Tables[0].Rows[0]["resourceid"].ToString());
                        intImplementor = Int32.Parse(oResourceRequest.GetWorkflow(intImplementor, "userid"));
                    }
                    else
                        intImplementor = -999;

                    sbData.Append("<tr><td colspan=\"2\" class=\"header\">Project Information</tr>");
                    sbData.Append("<tr><td nowrap>Project Name:</td><td width=\"100%\">");
                    sbData.Append(oProject.Get(intProject, "name"));
                    sbData.Append("</td></tr>");
                    sbData.Append("<tr><td nowrap>Project Number:</td><td width=\"100%\">");
                    sbData.Append(oProject.Get(intProject, "number"));
                    sbData.Append("</td></tr>");
                    sbData.Append("<tr><td nowrap>Project Type:</td><td width=\"100%\">");
                    sbData.Append(oProject.Get(intProject, "bd"));
                    sbData.Append("</td></tr>");
                    sbData.Append("<tr><td nowrap>Portfolio:</td><td width=\"100%\">");
                    sbData.Append(oOrganization.GetName(Int32.Parse(oProject.Get(intProject, "organization"))));
                    sbData.Append("</td></tr>");
                    string strLead = oProject.Get(intProject, "lead");
                    string strRequester = oForecast.Get(intForecast, "userid");
                    string strEngineer = oProject.Get(intProject, "engineer");
                    string strTechnical = oProject.Get(intProject, "technical");
                    sbData.Append("<tr><td nowrap>Project Manager:</td><td width=\"100%\">");
                    sbData.Append(strLead != "" ? oUser.GetFullName(Int32.Parse(strLead)) : "N/A");
                    sbData.Append("</td></tr>");
                    sbData.Append("<tr><td nowrap>Requester:</td><td width=\"100%\">");
                    sbData.Append(strRequester != "" ? oUser.GetFullName(Int32.Parse(strRequester)) : "N/A");
                    sbData.Append("</td></tr>");
                    sbData.Append("<tr><td nowrap>Integration Engineer:</td><td width=\"100%\">");
                    sbData.Append(strEngineer != "" ? oUser.GetFullName(Int32.Parse(strEngineer)) : "N/A");
                    sbData.Append("</td></tr>");
                    sbData.Append("<tr><td nowrap>Technical Lead:</td><td width=\"100%\">");
                    sbData.Append(strTechnical != "" ? oUser.GetFullName(Int32.Parse(strTechnical)) : "N/A");
                    sbData.Append("</td></tr>");
                    sbData.Append("<tr><td nowrap>II Resource:</td><td width=\"100%\">");
                    sbData.Append(intImplementor > 0 || intImplementor == -999 ? oUser.GetFullName(intImplementor) : "N/A");
                    sbData.Append("</td></tr>");

                    // Design Information
                    sbData.Append("<tr><td colspan=\"2\">&nbsp;</tr>");
                    sbData.Append("<tr><td colspan=\"2\" class=\"header\">Design Information</tr>");
                    double dblQuantity = double.Parse(oForecast.GetAnswer(intAnswer, "quantity")) + double.Parse(oForecast.GetAnswer(intAnswer, "recovery_number"));
                    sbData.Append("<tr><td nowrap>Commitment Date:</td><td width=\"100%\">");
                    sbData.Append(oForecast.GetAnswer(intAnswer, "implementation") == "" ? "" : DateTime.Parse(oForecast.GetAnswer(intAnswer, "implementation")).ToShortDateString());
                    sbData.Append("</td></tr>");
                    double dblA = 0.00;
                    DataSet dsA = oForecast.GetAcquisitions(intModel, 1);
                    foreach (DataRow drA in dsA.Tables[0].Rows)
                        dblA += double.Parse(drA["cost"].ToString());
                    sbData.Append("<tr><td nowrap>Acquisition Costs:</td><td width=\"100%\">");
                    sbData.Append(dblA.ToString("N"));
                    sbData.Append("</td></tr>");
                    double dblO = 0.00;
                    DataSet dsO = oForecast.GetOperations(intModel, 1);
                    foreach (DataRow drO in dsO.Tables[0].Rows)
                        dblO += double.Parse(drO["cost"].ToString());
                    sbData.Append("<tr><td nowrap>Operational Costs:</td><td width=\"100%\">");
                    sbData.Append(dblO.ToString("N"));
                    sbData.Append("</td></tr>");
                    double dblAmp = (double.Parse(oModelsProperties.Get(intModel, "amp")) * dblQuantity);
                    sbData.Append("<tr><td nowrap>AMPs:</td><td width=\"100%\">");
                    sbData.Append(dblAmp.ToString("N"));
                    sbData.Append(" AMPs");
                    sbData.Append("</td></tr>");
                    sbData.Append("<tr><td nowrap>Application Name:</td><td width=\"100%\">");
                    sbData.Append(oForecast.GetAnswer(intAnswer, "appname"));
                    sbData.Append("</td></tr>");
                    sbData.Append("<tr><td nowrap>Application Code:</td><td width=\"100%\">");
                    sbData.Append(oForecast.GetAnswer(intAnswer, "appcode"));
                    sbData.Append("</td></tr>");
                    string strContact1 = oForecast.GetAnswer(intAnswer, "appcontact");
                    if (strContact1 != "")
                    {
                        sbData.Append("<tr><td nowrap>Departmental Manager:</td><td width=\"100%\">");
                        sbData.Append(oUser.GetFullName(Int32.Parse(strContact1)));
                        sbData.Append(" (");
                        sbData.Append(oUser.GetName(Int32.Parse(strContact1)));
                        sbData.Append(")");
                        sbData.Append("</td></tr>");
                    }
                    string strContact2 = oForecast.GetAnswer(intAnswer, "admin1");
                    if (strContact2 != "")
                    {
                        sbData.Append("<tr><td nowrap>Application Technical Lead:</td><td width=\"100%\">");
                        sbData.Append(oUser.GetFullName(Int32.Parse(strContact2)));
                        sbData.Append(" (");
                        sbData.Append(oUser.GetName(Int32.Parse(strContact2)));
                        sbData.Append(")");
                        sbData.Append("</td></tr>");
                    }
                    string strContact3 = oForecast.GetAnswer(intAnswer, "admin2");
                    if (strContact3 != "")
                    {
                        sbData.Append("<tr><td nowrap>Administrative Contact:</td><td width=\"100%\">");
                        sbData.Append(oUser.GetFullName(Int32.Parse(strContact3)));
                        sbData.Append(" (");
                        sbData.Append(oUser.GetName(Int32.Parse(strContact3)));
                        sbData.Append(")");
                        sbData.Append("</td></tr>");
                    }
                    int intPlatform = Int32.Parse(oForecast.GetAnswer(intAnswer, "platformid"));

                    StringBuilder sbDataInfo = new StringBuilder();
                    int intCount = 0;
                   
                    string strEmail = "";
                    string strShared = "";
                    bool boolOther = false;
                    foreach (DataRow dr in dsAnswer.Tables[0].Rows)
                    {
                        intCount++;
                        boolOther = !boolOther;
                        int intServer = Int32.Parse(dr["id"].ToString());
                        int intUser = Int32.Parse(oForecast.GetAnswer(intAnswer, "userid"));
                        if (intUser > 0)
                            strEmail = oUser.GetName(intUser);
                        int intCSM = Int32.Parse(dr["csmconfigid"].ToString());
                        int intCluster = Int32.Parse(dr["clusterid"].ToString());
                        int intNumber2 = Int32.Parse(dr["number"].ToString());
                        int intName = Int32.Parse(dr["nameid"].ToString());
                        strName = oServer.GetName(intServer, boolUsePNCNaming);
                        if (strShared != "")
                            strShared += ", ";
                        strShared += strName;
                        sbDataInfo.Append("<tr><td nowrap>Design Nickname:</td><td width=\"100%\">");
                        sbDataInfo.Append(oForecast.GetAnswer(intAnswer, "name"));
                        sbDataInfo.Append("</td></tr>");
                        DataSet dsGeneric = oServer.GetGeneric(intServer);
                        string strVIO = "";
                        string strVIODR = "";
                        if (oModelsProperties.IsVIO(intModel) == true)
                        {
                            strVIO = dsGeneric.Tables[0].Rows[0]["vio1"].ToString() + ", " + dsGeneric.Tables[0].Rows[0]["vio2"].ToString();
                            sbDataInfo.Append("<tr><td nowrap>Server Name(s):</td><td width=\"100%\">");
                            sbDataInfo.Append(strVIO);
                            sbDataInfo.Append("</td></tr>");
                        }
                        else
                        {
                            sbDataInfo.Append("<tr><td nowrap>Server Name:</td><td width=\"100%\">");
                            sbDataInfo.Append(strName);
                            sbDataInfo.Append("</td></tr>");
                        }
                        int intAsset = 0;
                        if (dr["assetid"].ToString() != "")
                            intAsset = Int32.Parse(dr["assetid"].ToString());
                        if (dsGeneric.Tables[0].Rows.Count > 0)
                        {
                            if (oModelsProperties.IsStorageDB_BootSANUnix(intModel) == true || oModelsProperties.IsStorageDB_BootSANWindows(intModel) == true)
                            {
                                if (dsGeneric.Tables[0].Rows[0]["dummy_name"].ToString() != "")
                                {
                                    sbDataInfo.Append("<tr><td nowrap>Dummy Name (BFS):</td><td width=\"100%\">");
                                    sbDataInfo.Append(dsGeneric.Tables[0].Rows[0]["dummy_name"].ToString());
                                    sbDataInfo.Append("</td></tr>");
                                }
                            }
                            if (oModelsProperties.IsVIO(intModel) == false)
                            {
                                if (dsGeneric.Tables[0].Rows[0]["ww1"].ToString() != "" || dsGeneric.Tables[0].Rows[0]["ww2"].ToString() != "")
                                {
                                    sbDataInfo.Append("<tr><td nowrap>World Wide Port Name(s):</td><td width=\"100%\">");
                                    sbDataInfo.Append(dsGeneric.Tables[0].Rows[0]["ww1"].ToString());
                                    sbDataInfo.Append(", ");
                                    sbDataInfo.Append(dsGeneric.Tables[0].Rows[0]["ww2"].ToString());
                                    sbDataInfo.Append("</td></tr>");
                                }
                            }
                        }
                        else
                        {
                            if (oModelsProperties.IsStorageDB_BootSANUnix(intModel) == true || oModelsProperties.IsStorageDB_BootSANWindows(intModel) == true)
                            {
                                sbDataInfo.Append("<tr><td nowrap>Dummy Name (BFS):</td><td width=\"100%\">");
                                sbDataInfo.Append(oAsset.GetServerOrBlade(intAsset, "dummy_name"));
                                sbDataInfo.Append("</td></tr>");
                            }
                            DataSet dsHBA = oAsset.GetHBA(intAsset);
                            string strHBA = "";
                            foreach (DataRow drHBA in dsHBA.Tables[0].Rows)
                            {
                                if (strHBA != "")
                                    strHBA += ", ";
                                strHBA += drHBA["name"].ToString();
                            }
                            sbDataInfo.Append("<tr><td nowrap>World Wide Port Names:</td><td width=\"100%\">");
                            sbDataInfo.Append(strHBA);
                            sbDataInfo.Append("</td></tr>");
                        }

                        if (oModelsProperties.IsVIO(intModel) == true)
                        {
                            if (boolUnder == true && dr["dr"].ToString() == "1")
                            {
                                if (dr["dr_name"].ToString() != "")
                                {
                                    sbDataInfo.Append("<tr><td nowrap>DR Server Name:</td><td width=\"100%\">");
                                    sbDataInfo.Append(dr["dr_name"].ToString());
                                    sbDataInfo.Append("</td></tr>");
                                }
                                else
                                {
                                    strVIODR = dsGeneric.Tables[0].Rows[0]["vio1_dr"].ToString() + ", " + dsGeneric.Tables[0].Rows[0]["vio2_dr"].ToString();
                                    sbDataInfo.Append("<tr><td nowrap>DR Server Name(s):</td><td width=\"100%\">");
                                    sbDataInfo.Append(strVIODR);
                                    sbDataInfo.Append("</td></tr>");
                                }
                            }
                        }
                        else
                        {
                            if (boolUnder == true && dr["dr"].ToString() == "1")
                            {
                                if (dr["dr_name"].ToString() != "")
                                {
                                    sbDataInfo.Append("<tr><td nowrap>DR Server Name:</td><td width=\"100%\">");
                                    sbDataInfo.Append(dr["dr_name"].ToString());
                                    sbDataInfo.Append("</td></tr>");
                                }
                                else
                                {
                                    sbDataInfo.Append("<tr><td nowrap>DR Server Name:</td><td width=\"100%\">");
                                    sbDataInfo.Append(strName);
                                    sbDataInfo.Append("-DR");
                                    sbDataInfo.Append("</td></tr>");
                                }
                            }
                        }
                        int intDR = 0;
                        if (dr["drid"].ToString() != "")
                            intDR = Int32.Parse(dr["drid"].ToString());
                        if (boolProd == true)
                        {
                            if (dsGeneric.Tables[0].Rows.Count > 0)
                            {
                                if (oModelsProperties.IsStorageDB_BootSANUnix(intModel) == true || oModelsProperties.IsStorageDB_BootSANWindows(intModel) == true)
                                {
                                    if (dsGeneric.Tables[0].Rows[0]["dummy_name_dr"].ToString() != "")
                                    {
                                        sbDataInfo.Append("<tr><td nowrap>DR Dummy Name (BFS):</td><td width=\"100%\">");
                                        sbDataInfo.Append(dsGeneric.Tables[0].Rows[0]["dummy_name_dr"].ToString());
                                        sbDataInfo.Append("</td></tr>");
                                    }
                                }
                                if (oModelsProperties.IsVIO(intModel) == false)
                                {
                                    if (dsGeneric.Tables[0].Rows[0]["ww1_dr"].ToString() != "" || dsGeneric.Tables[0].Rows[0]["ww2_dr"].ToString() != "")
                                    {
                                        sbDataInfo.Append("<tr><td nowrap>DR World Wide Port Name(s):</td><td width=\"100%\">");
                                        sbDataInfo.Append(dsGeneric.Tables[0].Rows[0]["ww1_dr"].ToString());
                                        sbDataInfo.Append(", ");
                                        sbDataInfo.Append(dsGeneric.Tables[0].Rows[0]["ww2_dr"].ToString());
                                        sbDataInfo.Append("</td></tr>");
                                    }
                                }
                            }
                            else
                            {
                                if (oModelsProperties.IsStorageDB_BootSANUnix(intModel) == true || oModelsProperties.IsStorageDB_BootSANWindows(intModel) == true)
                                {
                                    sbDataInfo.Append("<tr><td nowrap>DR Dummy Name (BFS):</td><td width=\"100%\">");
                                    sbDataInfo.Append(oAsset.GetServerOrBlade(intDR, "dummy_name"));
                                    sbDataInfo.Append("</td></tr>");
                                }
                                DataSet dsHBA = oAsset.GetHBA(intDR);
                                string strHBA = "";
                                foreach (DataRow drHBA in dsHBA.Tables[0].Rows)
                                {
                                    if (strHBA != "")
                                        strHBA += ", ";
                                    strHBA += drHBA["name"].ToString();
                                }
                                sbDataInfo.Append("<tr><td nowrap>DR World Wide Port Names:</td><td width=\"100%\">");
                                sbDataInfo.Append(strHBA);
                                sbDataInfo.Append("</td></tr>");
                            }
                        }
                        sbDataInfo.Append("<tr><td nowrap>Assigned IP Address:</td><td width=\"100%\">");
                        sbDataInfo.Append(oServer.GetIPs(intServer, 1, 0, 0, 0, dsnIP, "", ""));
                        sbDataInfo.Append("</td></tr>");
                        sbDataInfo.Append("<tr><td nowrap>Final IP Address:</td><td width=\"100%\">");
                        sbDataInfo.Append(oServer.GetIPs(intServer, 0, 1, 0, 0, dsnIP, "", ""));
                        sbDataInfo.Append("</td></tr>");
                        sbDataInfo.Append("<tr><td nowrap>Backup IP Address:</td><td width=\"100%\">");
                        sbDataInfo.Append(oServer.GetIPs(intServer, 0, 0, 0, 1, dsnIP, "", ""));
                        sbDataInfo.Append("</td></tr>");
                        int intType = oModelsProperties.GetType(intModel);
                        
                        sbDataInfo.Append("<tr><td nowrap>Is a High Availability Device:</td><td width=\"100%\">");
                        sbDataInfo.Append(oForecast.IsHARoom(intAnswer) ? (dr["ha"].ToString() == "10" ? "Yes" : "No") : "N / A");
                        sbDataInfo.Append("</td></tr>");
                        sbDataInfo.Append("<tr><td nowrap>Model:</td><td width=\"100%\">");
                        sbDataInfo.Append(oModelsProperties.Get(intModel, "name"));
                        sbDataInfo.Append("</td></tr>");
                        sbDataInfo.Append("<tr><td nowrap>Fabric:</td><td width=\"100%\">");
                        sbDataInfo.Append(oModelsProperties.GetFabric(intModel));
                        sbDataInfo.Append("</td></tr>");
                        if (intAsset > 0)
                        {
                            sbDataInfo.Append("<tr><td nowrap>Serial Number:</td><td width=\"100%\">");
                            sbDataInfo.Append(oAsset.Get(intAsset, "serial").ToUpper());
                            sbDataInfo.Append("</td></tr>");
                            sbDataInfo.Append("<tr><td nowrap>Asset Tag:</td><td width=\"100%\">");
                            sbDataInfo.Append(oAsset.Get(intAsset, "asset").ToUpper());
                            sbDataInfo.Append("</td></tr>");
                            sbDataInfo.Append("<tr><td nowrap>Room:</td><td width=\"100%\">");
                            sbDataInfo.Append(oAsset.GetServerOrBlade(intAsset, "room"));
                            sbDataInfo.Append("</td></tr>");
                            sbDataInfo.Append("<tr><td nowrap>Rack:</td><td width=\"100%\">");
                            sbDataInfo.Append(oAsset.GetServerOrBlade(intAsset, "rack"));
                            sbDataInfo.Append("</td></tr>");
                            DataSet dsAssets = oServer.GetAssets(intServer);
                            foreach (DataRow drAsset in dsAssets.Tables[0].Rows)
                            {
                                if (drAsset["latest"].ToString() == "0" && drAsset["dr"].ToString() == "0")
                                {
                                    int intAssetOld = Int32.Parse(drAsset["assetid"].ToString());
                                    sbDataInfo.Append("<tr><td nowrap> - Previous Serial Number:</td><td width=\"100%\">");
                                    sbDataInfo.Append(oAsset.Get(intAssetOld, "serial").ToUpper());
                                    sbDataInfo.Append("</td></tr>");
                                    sbDataInfo.Append("<tr><td nowrap> - Previous Asset Tag:</td><td width=\"100%\">");
                                    sbDataInfo.Append(oAsset.Get(intAssetOld, "asset").ToUpper());
                                    sbDataInfo.Append("</td></tr>");
                                    sbDataInfo.Append("<tr><td nowrap> - Previous Class:</td><td width=\"100%\">");
                                    sbDataInfo.Append(drAsset["class"].ToString());
                                    sbDataInfo.Append("</td></tr>");
                                    sbDataInfo.Append("<tr><td nowrap> - Previous Environment:</td><td width=\"100%\">");
                                    sbDataInfo.Append(drAsset["environment"].ToString());
                                    sbDataInfo.Append("</td></tr>");
                                    sbDataInfo.Append("<tr><td nowrap> - Previous Room:</td><td width=\"100%\">");
                                    sbDataInfo.Append(oAsset.GetServerOrBlade(intAssetOld, "room"));
                                    sbDataInfo.Append("</td></tr>");
                                    sbDataInfo.Append("<tr><td nowrap> - Previous Rack:</td><td width=\"100%\">");
                                    sbDataInfo.Append(oAsset.GetServerOrBlade(intAssetOld, "rack"));
                                    sbDataInfo.Append("</td></tr>");
                                    if (oAsset.GetServerOrBlade(intAssetOld, "enclosureid") != "")
                                    {
                                        int intEnclosureID = Int32.Parse(oAsset.GetServerOrBlade(intAssetOld, "enclosureid"));
                                        if (intEnclosureID > 0)
                                        {
                                            sbDataInfo.Append("<tr><td nowrap> - Previous Enclosure:</td><td width=\"100%\">");
                                            sbDataInfo.Append(oAsset.Get(intEnclosureID, "name"));
                                            sbDataInfo.Append("</td></tr>");
                                            sbDataInfo.Append("<tr><td nowrap> - Previous Slot:</td><td width=\"100%\">");
                                            sbDataInfo.Append(oAsset.GetServerOrBlade(intAssetOld, "slot"));
                                            sbDataInfo.Append("</td></tr>");
                                        }
                                    }
                                }
                            }
                        }
                        if (intDR > 0)
                        {
                            sbDataInfo.Append("<tr><td nowrap>DR Serial Number:</td><td width=\"100%\">");
                            sbDataInfo.Append(oAsset.Get(intDR, "serial").ToUpper());
                            sbDataInfo.Append("</td></tr>");
                            sbDataInfo.Append("<tr><td nowrap>DR Asset Tag:</td><td width=\"100%\">");
                            sbDataInfo.Append(oAsset.Get(intDR, "asset").ToUpper());
                            sbDataInfo.Append("</td></tr>");
                            sbDataInfo.Append("<tr><td nowrap>DR Room:</td><td width=\"100%\">");
                            sbDataInfo.Append(oAsset.GetServerOrBlade(intDR, "room"));
                            sbDataInfo.Append("</td></tr>");
                            sbDataInfo.Append("<tr><td nowrap>DR Rack:</td><td width=\"100%\">");
                            sbDataInfo.Append(oAsset.GetServerOrBlade(intDR, "rack"));
                            sbDataInfo.Append("</td></tr>");
                        }
                        sbDataInfo.Append("<tr><td nowrap>Current Class:</td><td width=\"100%\">");
                        sbDataInfo.Append(strClass);
                        sbDataInfo.Append("</td></tr>");
                        sbDataInfo.Append("<tr><td nowrap>Final Class:</td><td width=\"100%\">");
                        sbDataInfo.Append(oClass.Get(intClass, "name"));
                        sbDataInfo.Append("</td></tr>");
                        sbDataInfo.Append("<tr><td nowrap>Environment:</td><td width=\"100%\">");
                        sbDataInfo.Append(oEnvironment.Get(intEnv, "name"));
                        sbDataInfo.Append("</td></tr>");
                        int intOS = Int32.Parse(dr["osid"].ToString());
                        sbDataInfo.Append("<tr><td nowrap>Operating System:</td><td width=\"100%\">");
                        sbDataInfo.Append(oOperatingSystem.Get(intOS, "name"));
                        sbDataInfo.Append("</td></tr>");
                        sbDataInfo.Append("<tr><td nowrap>Clustered Server Name:</td><td width=\"100%\">");
                        sbDataInfo.Append("N / A");
                        sbDataInfo.Append("</td></tr>");
                        int intConsistency = Int32.Parse(dr["dr_consistencyid"].ToString());
                        if (intConsistency > 0)
                        {
                            sbDataInfo.Append("<tr><td nowrap>Consistency Group Name:</td><td width=\"100%\">");
                            sbDataInfo.Append(oConsistencyGroups.Get(intConsistency, "name"));
                            sbDataInfo.Append("</td></tr>");
                            DataSet dsMembers = oConsistencyGroups.GetMember(intServer);
                            string strMembers = "";
                            foreach (DataRow drMember in dsMembers.Tables[0].Rows)
                            {
                                if (strMembers != "")
                                    strMembers += ", ";
                                strMembers += drMember["name"].ToString();
                            }
                            sbDataInfo.Append("<tr><td nowrap>Consistency Group Members:</td><td width=\"100%\">");
                            sbDataInfo.Append(strMembers);
                            sbDataInfo.Append("</td></tr>");
                        }
                        else
                        {
                            sbDataInfo.Append("<tr><td nowrap>Consistency Group:</td><td width=\"100%\">");
                            sbDataInfo.Append("N / A");
                            sbDataInfo.Append("</td></tr>");
                        }
                        sbData.Append("<tr><td colspan=\"2\">&nbsp;</tr>");
                        sbData.Append("<tr><td colspan=\"2\" class=\"header\">");
                        sbData.Append("ClearView Backup Request Form  for ");
                        sbData.Append(strName);
                        sbData.Append("</td></tr>");
                        sbData.Append(sbDataInfo.ToString());

                        if (intTSM > 0)
                        {
                            // Dynamic Backup
                            string strResult = dr["tsm_output"].ToString();
                            strBackup += "<tr" + (boolOther == false ? " bgcolor=\"#F6F6F6\"" : "") + ">";
                            strBackup += "<td>" + strName + "</td>";
                            strPreview += "<tr>";
                            strPreview += "<td>" + strName + "</td>";
                            int intSchedule = Int32.Parse(dr["tsm_schedule"].ToString());
                            if (intSchedule > 0)
                            {
                                int intDomain = Int32.Parse(oTSM.GetSchedule(intSchedule, "domain"));
                                string strDomain = oTSM.GetDomain(intDomain, "name");
                                int intTSMServer = Int32.Parse(oTSM.GetDomain(intDomain, "tsm"));
                                strPreview += "<td>" + oTSM.Get(intTSMServer, "name") + "</td>";
                                strPreview += "<td>" + oTSM.Get(intTSMServer, "port") + "</td>";
                                strPreview += "<td>" + oTSM.GetSchedule(intSchedule, "name") + "</td>";
                            }
                            // Either TSM, Avamar or Legato

                            strBackup += oTSM.LoadDDL("ddlServer_" + intServer.ToString(), "ddlDomain_" + intServer.ToString(), "ddlSchedule_" + intServer.ToString(), "HDN_" + intServer.ToString() + "_TSM_SCHEDULE", intSchedule, intTSM, intAvamar, strResult);
                            if (intTSM > 0)
                            {
                                strBackup += "<td>";
                                if (strResult == "")
                                {
                                    strBackup += "<select id=\"DDL_" + intServer + "_TSM_CLOPTSET\" class=\"default\" onchange=\"UpdateDDL(this,'HDN_" + intServer + "_TSM_CLOPTSET');\" style=\"width:150px;\">";
                                    strBackup += "<option value=\"0\">-- SELECT --</option>";
                                    DataSet dsTSM = oTSM.GetCloptsets(1);
                                    foreach (DataRow drTSM in dsTSM.Tables[0].Rows)
                                        strBackup += "<option value=\"" + drTSM["id"].ToString() + "\"" + (dr["tsm_cloptset"].ToString() == drTSM["id"].ToString() ? " selected" : "") + ">" + drTSM["name"].ToString() + "</option>";
                                    strBackup += "</select>";
                                }
                                else
                                    strBackup += "<input type=\"text\" style=\"width:200px\" class=\"default\" value=\"" + oTSM.GetCloptset(Int32.Parse(dr["tsm_cloptset"].ToString()), "name") + "\" readonly />";
                                strBackup += "</td>";
                            }
                            if ((intTSM > 0 && intTSMAutomated >= 0) || (intAvamar > 0 && intAvamarAutomated >= 0))
                            {
                                strBackup += "<td><input id=\"chkRedo_" + intServer.ToString() + "\" type=\"checkbox\" name=\"chkRedo_" + intServer.ToString() + "\" onclick=\"UpdateCheckBox(this,'HDN_" + intServer.ToString() + "_TSM_REDO');\" /> Regenerate</td>";
                            }
                            strBackup += "</tr>";
                            strPreview += "</tr>";
                            bool boolBypass = (dr["tsm_bypass"].ToString() == "1");
                            if (intTSM > 0)
                            {
                                if (dr["tsm_register"].ToString() == "")
                                    strBackup += "<tr" + (boolOther == false ? " bgcolor=\"#F6F6F6\"" : "") + "><td>&nbsp;&nbsp;&nbsp;&nbsp;<img src=\"/images/arrow_down_right.gif\" border=\"0\" align=\"absmiddle\"/>REGISTER:</td><td colspan=\"6\"><input type=\"text\" id=\"TXT_" + intServer.ToString() + "_TSM_REGISTER\" style=\"width:850px\" class=\"default\" value=\"Choose Options and Click SAVE to Generate\" disabled maxlength=\"300\" /></td></tr>";
                                else
                                    strBackup += "<tr" + (boolOther == false ? " bgcolor=\"#F6F6F6\"" : "") + "><td>&nbsp;&nbsp;&nbsp;&nbsp;<img src=\"/images/arrow_down_right.gif\" border=\"0\" align=\"absmiddle\"/>REGISTER:</td><td colspan=\"6\"><input type=\"text\" id=\"TXT_" + intServer.ToString() + "_TSM_REGISTER\" style=\"width:850px\" class=\"default\" value=\"" + Server.HtmlEncode(dr["tsm_register"].ToString()) + "\"" + (strResult != "" ? " readonly" : " onblur=\"UpdateTextBox(this,'HDN_" + intServer.ToString() + "_TSM_REGISTER');\"") + " maxlength=\"300\"/></td></tr>";
                                if (dr["tsm_define"].ToString() == "")
                                    strBackup += "<tr" + (boolOther == false ? " bgcolor=\"#F6F6F6\"" : "") + "><td>&nbsp;&nbsp;&nbsp;&nbsp;<img src=\"/images/arrow_down_right.gif\" border=\"0\" align=\"absmiddle\"/>DEFINE:</td><td colspan=\"3\"><input type=\"text\" id=\"TXT_" + intServer.ToString() + "_TSM_DEFINE\" style=\"width:500px\" class=\"default\" value=\"Choose Options and Click SAVE to Generate\" disabled maxlength=\"300\" /></td><td colspan=\"3\"><input id=\"chkRedo_" + intServer.ToString() + "\" type=\"checkbox\" name=\"chkBypass_" + intServer.ToString() + "\" disabled /> Bypass Auto-Registration</td></tr>";
                                else
                                    strBackup += "<tr" + (boolOther == false ? " bgcolor=\"#F6F6F6\"" : "") + "><td>&nbsp;&nbsp;&nbsp;&nbsp;<img src=\"/images/arrow_down_right.gif\" border=\"0\" align=\"absmiddle\"/>DEFINE:</td><td colspan=\"3\"><input type=\"text\" id=\"TXT_" + intServer.ToString() + "_TSM_DEFINE\" style=\"width:500px\" class=\"default\" value=\"" + Server.HtmlEncode(dr["tsm_define"].ToString()) + "\"" + (strResult != "" ? " readonly" : " onblur=\"UpdateTextBox(this,'HDN_" + intServer.ToString() + "_TSM_DEFINE');\"") + " maxlength=\"300\"/></td><td colspan=\"3\"><input id=\"chkRedo_" + intServer.ToString() + "\" type=\"checkbox\" name=\"chkBypass_" + intServer.ToString() + "\"" + (strResult != "" ? " disabled" : " onclick=\"UpdateCheckBox(this,'HDN_" + intServer.ToString() + "_TSM_BYPASS');\"" + (boolBypass ? " checked" : "")) + " /> Bypass Auto-Registration</td></tr>";
                            }
                            else
                            {
                                btnRefresh.Enabled = false;
                            }

                            if (chk1.Checked == true)
                            {
                                if (boolBypass == true || ((intTSM > 0 && intTSMAutomated < 0) || (intAvamar > 0 && intAvamarAutomated < 0)))
                                    strBackup += "<tr" + (boolOther == false ? " bgcolor=\"#F6F6F6\"" : "") + "><td valign=\"top\" align=\"right\" class=\"redlink\">RESULT:</td><td colspan=\"5\">" + (strResult == "" ? "Auto-Registration Skipped (Please register manually)" : oFunction.FormatText(strResult)) + "</td></tr>";
                                else
                                    strBackup += "<tr" + (boolOther == false ? " bgcolor=\"#F6F6F6\"" : "") + "><td valign=\"top\" align=\"right\" class=\"redlink\">RESULT:</td><td colspan=\"5\">" + (strResult == "" ? "Pending Execution..." : oFunction.FormatText(strResult)) + "</td></tr>";
                            }
                            else
                                btnRefresh.Enabled = false;
                            strBackup += "<tr><td colspan=\"6\">&nbsp;</td></tr>";

                            // Backup Header
                            if (intTSM > 0)
                            {
                                strBackupHeader = "<tr bgcolor='#EEEEEE'>";
                                strBackupHeader += "<td><b><u>Server:</u></b></td>";
                                strBackupHeader += "<td><b><u>TSM Server:</u></b></td>";
                                strBackupHeader += "<td><b><u>Domain:</u></b></td>";
                                strBackupHeader += "<td><b><u>Schedule:</u></b></td>";
                                strBackupHeader += "<td><b><u>CLOPTSET:</u></b></td>";
                                strBackupHeader += "<td></td></tr>";
                            }
                            else if (intAvamar > 0)
                            {
                                strBackupHeader = "<tr bgcolor='#EEEEEE'>";
                                strBackupHeader += "<td><b><u>Server:</u></b></td>";
                                strBackupHeader += "<td><b><u>Grid:</u></b></td>";
                                strBackupHeader += "<td><b><u>Domain:</u></b></td>";
                                strBackupHeader += "<td><b><u>Group:</u></b></td>";
                                strBackupHeader += "<td></td></tr>";
                            }


                            strHiddenV += "<input type=\"hidden\" name=\"HDN_" + intServer.ToString() + "_TSM_SCHEDULE\" id=\"HDN_" + intServer.ToString() + "_TSM_SCHEDULE\" value=\"" + dr["tsm_schedule"].ToString() + "\" />";
                            if (intTSM > 0)
                                strHiddenV += "<input type=\"hidden\" name=\"HDN_" + intServer.ToString() + "_TSM_CLOPTSET\" id=\"HDN_" + intServer.ToString() + "_TSM_CLOPTSET\" value=\"" + dr["tsm_cloptset"].ToString() + "\" />";
                            strHiddenV += "<input type=\"hidden\" name=\"HDN_" + intServer.ToString() + "_TSM_REGISTER\" id=\"HDN_" + intServer.ToString() + "_TSM_REGISTER\" value=\"" + Server.HtmlEncode(dr["tsm_register"].ToString()) + "\" />";
                            strHiddenV += "<input type=\"hidden\" name=\"HDN_" + intServer.ToString() + "_TSM_DEFINE\" id=\"HDN_" + intServer.ToString() + "_TSM_DEFINE\" value=\"" + Server.HtmlEncode(dr["tsm_define"].ToString()) + "\" />";
                            strHiddenV += "<input type=\"hidden\" name=\"HDN_" + intServer.ToString() + "_TSM_REDO\" id=\"HDN_" + intServer.ToString() + "_TSM_REDO\" value=\"0\" />";
                            strHiddenV += "<input type=\"hidden\" name=\"HDN_" + intServer.ToString() + "_TSM_BYPASS\" id=\"HDN_" + intServer.ToString() + "_TSM_BYPASS\" value=\"" + dr["tsm_bypass"].ToString() + "\" />";
                            strValidation += intServer.ToString() + ";";
                        }
                        else
                            btnRefresh.Visible = false;
                        sbDataInfo = new StringBuilder();

                        // BACKUP INFORMATION
                        if (dsBackup.Tables[0].Rows.Count > 0)
                        {
                            if (intRecovery > 0)
                            {
                                sbData.Append("<tr><td nowrap>Recovery Location:</td><td width=\"100%\">");
                                sbData.Append(oLocation.GetFull(intRecovery));
                                sbData.Append("</td></tr>");
                            }
                            if (dsBackup.Tables[0].Rows[0]["daily"].ToString() == "1")
                            {
                                sbData.Append("<tr><td nowrap>Timing/Frequency of Backups:</td><td width=\"100%\">");
                                sbData.Append("Daily");
                                sbData.Append("</td></tr>");
                            }
                            else if (dsBackup.Tables[0].Rows[0]["weekly"].ToString() == "1")
                            {
                                sbData.Append("<tr><td nowrap>Timing/Frequency of Backups:</td><td width=\"100%\">");
                                sbData.Append("Weekly");
                                sbData.Append("</td></tr>");
                            }
                            else if (dsBackup.Tables[0].Rows[0]["monthly"].ToString() == "1")
                            {
                                sbData.Append("<tr><td nowrap>Timing/Frequency of Backups:</td><td width=\"100%\">");
                                sbData.Append("Monthly");
                                sbData.Append("</td></tr>");
                            }
                            if (dsBackup.Tables[0].Rows[0]["time"].ToString() == "1")
                            {
                                sbData.Append("<tr><td nowrap>Start Time:</td><td width=\"100%\">");
                                sbData.Append(dsBackup.Tables[0].Rows[0]["time_hour"].ToString());
                                sbData.Append(" ");
                                sbData.Append(dsBackup.Tables[0].Rows[0]["time_switch"].ToString());
                                sbData.Append("</td></tr>");
                            }
                            else
                            {
                                sbData.Append("<tr><td nowrap>Start Time:</td><td width=\"100%\">");
                                sbData.Append("Don't Care");
                                sbData.Append("</td></tr>");
                            }

                            //if (dsBackup.Tables[0].Rows[0]["start_date_prod"].ToString() != "")
                            //    strData += "<tr><td nowrap>Start Date (PROD):</td><td width=\"100%\">" + DateTime.Parse(dsBackup.Tables[0].Rows[0]["start_date_prod"].ToString()).ToShortDateString() + "</td></tr>";
                            //else
                            //    strData += "<tr><td nowrap>Start Date (PROD):</td><td width=\"100%\">" + "N / A" + "</td></tr>";
                            //if (dsBackup.Tables[0].Rows[0]["start_date_test"].ToString() != "")
                            //    strData += "<tr><td nowrap>Start Date (TEST):</td><td width=\"100%\">" + DateTime.Parse(dsBackup.Tables[0].Rows[0]["start_date_test"].ToString()).ToShortDateString() + "</td></tr>";
                            //else
                            //    strData += "<tr><td nowrap>Start Date (TEST):</td><td width=\"100%\">" + "N / A" + "</td></tr>";
                            double dblHighU = 0.00;
                            double dblStandardU = 0.00;
                            double dblLowU = 0.00;
                            double dblHighQAU = 0.00;
                            double dblStandardQAU = 0.00;
                            double dblLowQAU = 0.00;
                            double dblHighTestU = 0.00;
                            double dblStandardTestU = 0.00;
                            double dblLowTestU = 0.00;
                            DataSet dsStorage = oStorage.GetLuns(intAnswer, 0, intCluster, intCSM, intNumber2);
                            foreach (DataRow drStorage in dsStorage.Tables[0].Rows)
                            {
                                if (drStorage["size"].ToString() != "")
                                {
                                    if (drStorage["performance"].ToString() == "High")
                                        dblHighU += double.Parse(drStorage["size"].ToString());
                                    if (drStorage["performance"].ToString() == "Standard")
                                        dblStandardU += double.Parse(drStorage["size"].ToString());
                                    if (drStorage["performance"].ToString() == "Low")
                                        dblLowU += double.Parse(drStorage["size"].ToString());
                                }
                                if (drStorage["size_qa"].ToString() != "")
                                {
                                    if (drStorage["performance"].ToString() == "High")
                                        dblHighQAU += double.Parse(drStorage["size_qa"].ToString());
                                    if (drStorage["performance"].ToString() == "Standard")
                                        dblStandardQAU += double.Parse(drStorage["size_qa"].ToString());
                                    if (drStorage["performance"].ToString() == "Low")
                                        dblLowQAU += double.Parse(drStorage["size_qa"].ToString());
                                }
                                if (drStorage["size_test"].ToString() != "")
                                {
                                    if (drStorage["performance"].ToString() == "High")
                                        dblHighTestU += double.Parse(drStorage["size_test"].ToString());
                                    if (drStorage["performance"].ToString() == "Standard")
                                        dblStandardTestU += double.Parse(drStorage["size_test"].ToString());
                                    if (drStorage["performance"].ToString() == "Low")
                                        dblLowTestU += double.Parse(drStorage["size_test"].ToString());
                                }
                                DataSet dsMount = oStorage.GetMountPoints(Int32.Parse(drStorage["id"].ToString()));
                                foreach (DataRow drMount in dsMount.Tables[0].Rows)
                                {
                                    if (drMount["size"].ToString() != "")
                                    {
                                        if (drMount["performance"].ToString() == "High")
                                            dblHighU += double.Parse(drMount["size"].ToString());
                                        if (drMount["performance"].ToString() == "Standard")
                                            dblStandardU += double.Parse(drMount["size"].ToString());
                                        if (drMount["performance"].ToString() == "Low")
                                            dblLowU += double.Parse(drMount["size"].ToString());
                                    }
                                    if (drMount["size_qa"].ToString() != "")
                                    {
                                        if (drMount["performance"].ToString() == "High")
                                            dblHighQAU += double.Parse(drMount["size_qa"].ToString());
                                        if (drMount["performance"].ToString() == "Standard")
                                            dblStandardQAU += double.Parse(drMount["size_qa"].ToString());
                                        if (drMount["performance"].ToString() == "Low")
                                            dblLowQAU += double.Parse(drMount["size_qa"].ToString());
                                    }
                                    if (drMount["size_test"].ToString() != "")
                                    {
                                        if (drMount["performance"].ToString() == "High")
                                            dblHighTestU += double.Parse(drMount["size_test"].ToString());
                                        if (drMount["performance"].ToString() == "Standard")
                                            dblStandardTestU += double.Parse(drMount["size_test"].ToString());
                                        if (drMount["performance"].ToString() == "Low")
                                            dblLowTestU += double.Parse(drMount["size_test"].ToString());
                                    }
                                }
                            }
                            double dblTotal = dblHighU + dblStandardU + dblLowU + dblHighQAU + dblStandardQAU + dblLowQAU + dblHighTestU + dblStandardTestU + dblLowTestU;
                            sbData.Append("<tr><td nowrap>Total Combined Disk Capacity (GB):</td><td width=\"100%\">");
                            sbData.Append(dblTotal.ToString("0"));
                            sbData.Append(" GB");
                            sbData.Append("</td></tr>");
                            sbData.Append("<tr><td nowrap>Current Combined Disk Utilized (GB):</td><td width=\"100%\">");
                            sbData.Append("5 GB");
                            sbData.Append("</td></tr>");
                            sbData.Append("<tr><td nowrap>Average Size of One Typical Data File:</td><td width=\"100%\">");
                            sbData.Append(dsBackup.Tables[0].Rows[0]["average_one"].ToString());
                            sbData.Append(" GB");
                            sbData.Append("</td></tr>");
                            if (dsBackup.Tables[0].Rows[0]["documentation"].ToString() == "")
                            {
                                sbData.Append("<tr><td nowrap>Production Turnover Documentation:</td><td width=\"100%\">");
                                sbData.Append("Not Specified");
                                sbData.Append("</td></tr>");
                            }
                            else
                            {
                                sbData.Append("<tr><td nowrap>Production Turnover Documentation:</td><td width=\"100%\">");
                                sbData.Append(dsBackup.Tables[0].Rows[0]["documentation"].ToString());
                                sbData.Append("</td></tr>");
                            }

                            rptInclusions.DataSource = oForecast.GetBackupInclusions(intAnswer);
                            rptInclusions.DataBind();
                            lblNoneInclusions.Visible = rptInclusions.Items.Count == 0;

                            rptExclusions.DataSource = oForecast.GetBackupExclusions(intAnswer);
                            rptExclusions.DataBind();
                            lblNoneExclusions.Visible = rptExclusions.Items.Count == 0;

                            rptRetention.DataSource = oForecast.GetBackupRetentions(intAnswer);
                            rptRetention.DataBind();
                            lblNoneRetention.Visible = rptRetention.Items.Count == 0;


                            lblAverage.Text = dsBackup.Tables[0].Rows[0]["average_one"] != DBNull.Value ? dsBackup.Tables[0].Rows[0]["average_one"].ToString() : "0";
                            lblDocumentation.Text = dsBackup.Tables[0].Rows[0]["documentation"] != DBNull.Value ? dsBackup.Tables[0].Rows[0]["documentation"].ToString() : "NA";
                            lblCFPercent.Text = dsBackup.Tables[0].Rows[0]["cf_percent"] != DBNull.Value ? dsBackup.Tables[0].Rows[0]["cf_percent"].ToString() + "%" : "";
                            lblCFCompression.Text = dsBackup.Tables[0].Rows[0]["cf_compression"] != DBNull.Value ? dsBackup.Tables[0].Rows[0]["cf_compression"].ToString() + "%" : "";
                            lblCFAverage.Text = dsBackup.Tables[0].Rows[0]["cf_average"] != DBNull.Value ? dsBackup.Tables[0].Rows[0]["cf_average"].ToString() : "";
                            lblCFBackup.Text = dsBackup.Tables[0].Rows[0]["cf_backup"] != DBNull.Value ? dsBackup.Tables[0].Rows[0]["cf_backup"].ToString() : "";
                            lblCFArchive.Text = dsBackup.Tables[0].Rows[0]["cf_archive"] != DBNull.Value ? dsBackup.Tables[0].Rows[0]["cf_archive"].ToString() : "";
                            lblCFWindow.Text = dsBackup.Tables[0].Rows[0]["cf_window"] != DBNull.Value ? dsBackup.Tables[0].Rows[0]["cf_window"].ToString() + " (Hours)" : "";
                            lblCFSets.Text = dsBackup.Tables[0].Rows[0]["cf_sets"] != DBNull.Value ? dsBackup.Tables[0].Rows[0]["cf_sets"].ToString() : "";
                            lblCDType.Text = dsBackup.Tables[0].Rows[0]["cd_type"] != DBNull.Value ? dsBackup.Tables[0].Rows[0]["cd_type"].ToString() : "";
                            lblCDPercent.Text = dsBackup.Tables[0].Rows[0]["cd_percent"] != DBNull.Value ? dsBackup.Tables[0].Rows[0]["cd_percent"].ToString() + "%" : "";
                            lblCDCompression.Text = dsBackup.Tables[0].Rows[0]["cd_compression"] != DBNull.Value ? dsBackup.Tables[0].Rows[0]["cd_compression"].ToString() : "";
                            lblCDVersions.Text = dsBackup.Tables[0].Rows[0]["cd_versions"] != DBNull.Value ? dsBackup.Tables[0].Rows[0]["cd_versions"].ToString() : "";
                            lblCDWindow.Text = dsBackup.Tables[0].Rows[0]["cd_window"] != DBNull.Value ? dsBackup.Tables[0].Rows[0]["cd_window"].ToString() : "";
                            lblCDGrowth.Text = dsBackup.Tables[0].Rows[0]["cd_growth"] != DBNull.Value ? dsBackup.Tables[0].Rows[0]["cd_growth"].ToString() : "";
                        }
                        else
                        {
                            DataSet dsDesign = oDesign.GetAnswer(intAnswer);
                            if (dsDesign.Tables[0].Rows.Count > 0)
                            {
                                panCFI.Visible = true;
                                int intDesign = Int32.Parse(dsDesign.Tables[0].Rows[0]["id"].ToString());
                                string strFrequency = oDesign.Get(intDesign, "backup_frequency");
                                lblFrequency.Text = (strFrequency == "D" ? "Daily" : (strFrequency == "W" ? "Weekly" : (strFrequency == "M" ? "Monthly" : "N / A")));
                                strBackupCFI = new StringBuilder();
                                DataSet dsBackupCFI = oDesign.GetBackup(intDesign);
                                if (dsBackupCFI.Tables[0].Rows.Count > 0)
                                {
                                    DataRow drBackup = dsBackupCFI.Tables[0].Rows[0];
                                    for (int ii = 0; ii < 7; ii++)
                                    {
                                        strBackupCFI.Append("<tr>");
                                        strBackupCFI.Append("<td>");
                                        string strCheck = "";
                                        if (ii == 0)
                                        {
                                            strBackupCFI.Append("Sunday");
                                            strCheck = drBackup["sun"].ToString();
                                        }
                                        else if (ii == 1)
                                        {
                                            strBackupCFI.Append("Monday");
                                            strCheck = drBackup["mon"].ToString();
                                        }
                                        else if (ii == 2)
                                        {
                                            strBackupCFI.Append("Tuesday");
                                            strCheck = drBackup["tue"].ToString();
                                        }
                                        else if (ii == 3)
                                        {
                                            strBackupCFI.Append("Wednesday");
                                            strCheck = drBackup["wed"].ToString();
                                        }
                                        else if (ii == 4)
                                        {
                                            strBackupCFI.Append("Thursday");
                                            strCheck = drBackup["thu"].ToString();
                                        }
                                        else if (ii == 5)
                                        {
                                            strBackupCFI.Append("Friday");
                                            strCheck = drBackup["fri"].ToString();
                                        }
                                        else
                                        {
                                            strBackupCFI.Append("Saturday");
                                            strCheck = drBackup["sat"].ToString();
                                        }
                                        strBackupCFI.Append("</td>");
                                        for (int jj = 0; jj < 24; jj++)
                                        {
                                            strBackupCFI.Append("<td>");
                                            if (strCheck[jj] == '1')
                                                strBackupCFI.Append("<b>B</b>");
                                            else
                                                strBackupCFI.Append("-");
                                            strBackupCFI.Append("</td>");
                                        }
                                        strBackupCFI.Append("</tr>");
                                    }
                                }

                                rptExclusionsCFI.DataSource = oDesign.GetExclusions(intDesign);
                                rptExclusionsCFI.DataBind();
                                if (rptExclusionsCFI.Items.Count == 0)
                                    lblExclusion.Visible = true;
                            }
                        }
                    }
                    sbData.Insert(0, "<table width=\"100%\" cellpadding=\"2\" cellspacing=\"1\" border=\"0\">");
                    sbData.Append("</table>");
                    lbl1.Text += sbData.ToString();
                }
            }
            if (boolDone == true)
            {
                lblPreview.Text = "<p>Your servers have been successfully registered to a TSM Server. Here are the details...</p>";
                lblPreview.Text += "<p><table cellpadding=\"2\" cellspacing=\"1\" border=\"0\" style=\"" + oVariable.DefaultFontStyle() + "\"><tr><td><b>Server Name:</b></td><td><b>TCPSERVERADDRESS:</b></td><td><b>TCPPort:</b></td><td><b>Schedule:</b></td>" + strPreview + "</table></p>";
                lblPreview.Text += "<p>Please update the TSM client software and recycle the TSM services to pick up the next scheduled backup time (shown above).</p>";
                lblPreview.Text += "<p>Also check that the dsm.opt files have a valid include/exclude list to ensure the machine is only backing up what is required.</p>";
                if (intTSM > 0)
                    panPreview.Visible = true;
            }

            if (Request.QueryString["div"] != null)
            {
                switch (Request.QueryString["div"])
                {
                    case "E":
                        boolExecution = true;
                        break;
                    case "S":
                        boolStatusUpdates = true;
                        break;
                }
            }
            if (boolDetails == false && boolExecution == false && boolStatusUpdates == false)
                boolDetails = true;
            return boolDone;
        }
        protected void btnStatus_Click(Object Sender, EventArgs e)
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

            Response.Redirect(Request.Path + "?rrid=" + intResourceWorkflow.ToString() + "&div=S&status=true");
        }

        protected void btnSave_Click(Object Sender, EventArgs e)
        {
            int intResourceWorkflow = Int32.Parse(lblResourceWorkflow.Text);
            int intResourceParent = oResourceRequest.GetWorkflowParent(intResourceWorkflow);
            oResourceRequest.UpdateWorkflowName(intResourceWorkflow, txtCustom.Text, intProfile);
            int intTSM = Int32.Parse(lblTSM.Text);
            int intAvamar = Int32.Parse(lblAvamar.Text);
            foreach (string strForm in Request.Form)
            {
                if (strForm.StartsWith("HDN_") == true)
                {
                    int intServer = Int32.Parse(strForm.Substring(4, strForm.IndexOf("_", 4) - 4));
                    string strName = oServer.GetName(intServer, boolUsePNCNaming);
                    int intSchedule = Int32.Parse(Request.Form["HDN_" + intServer.ToString() + "_TSM_SCHEDULE"]);
                    int intCLOPTSET = 0;
                    string strRegister = "";
                    string strDefine = "";
                    int intBypass = 0;
                    if (intSchedule > 0)
                    {
                        int intDomain = Int32.Parse(oTSM.GetSchedule(intSchedule, "domain"));
                        string strDomain = oTSM.GetDomain(intDomain, "name");
                        int intTSMServer = Int32.Parse(oTSM.GetDomain(intDomain, "tsm"));
                        if (intTSM > 0)
                        {
                            Int32.TryParse(Request.Form["HDN_" + intServer.ToString() + "_TSM_CLOPTSET"], out intCLOPTSET);
                            if (intCLOPTSET > 0)
                            {
                                intBypass = Int32.Parse(Request.Form["HDN_" + intServer.ToString() + "_TSM_BYPASS"]);
                                if (Request.Form["HDN_" + intServer.ToString() + "_TSM_REGISTER"] == "" || Request.Form["HDN_" + intServer.ToString() + "_TSM_REDO"] == "1")
                                {
                                    // FORMAT: REGISTER NODE OHCLEIIS406P OHCLEIIS406P USERID=none CONTACT="" DOMAIN=TEST  CLOPTSET=WIN32-PROMPTED FORCEPWRESET=NO URL=""
                                    strRegister = "REGISTER NODE " + strName + " " + strName + " USERID=none CONTACT=\"\" DOMAIN=" + strDomain + " CLOPTSET=" + oTSM.GetCloptset(intCLOPTSET, "name") + " FORCEPWRESET=NO URL=\"\"";
                                }
                                else
                                    strRegister = Request.Form["HDN_" + intServer.ToString() + "_TSM_REGISTER"];
                                if (Request.Form["HDN_" + intServer.ToString() + "_TSM_DEFINE"] == "" || Request.Form["HDN_" + intServer.ToString() + "_TSM_REDO"] == "1")
                                {
                                    // FORMAT: DEFINE ASSOCIATION TEST DAILY_INCR_0200_A OHCLEIIS406P
                                    strDefine = "DEFINE ASSOCIATION " + strDomain + " " + oTSM.GetSchedule(intSchedule, "name") + " " + strName;
                                }
                                else
                                    strDefine = Request.Form["HDN_" + intServer.ToString() + "_TSM_DEFINE"];
                            }
                        }
                        if (intAvamar > 0)
                        {
                            // ???
                        }
                    }
                    oServer.UpdateTSM(intServer, intSchedule, intCLOPTSET, strRegister, strDefine, intBypass);
                }
            }
            oOnDemandTasks.UpdateServerBackup(intRequest, intItem, intNumber, (chk1.Checked ? 1 : 0));
            Response.Redirect(Request.Path + "?rrid=" + intResourceWorkflow.ToString() + "&div=E&save=true");
        }
        protected void btnComplete_Click(Object Sender, EventArgs e)
        {
            int intResourceWorkflow = Int32.Parse(lblResourceWorkflow.Text);
            int intResourceParent = oResourceRequest.GetWorkflowParent(intResourceWorkflow);
            oResourceRequest.UpdateWorkflowStatus(intResourceWorkflow, 3, true);
            oResourceRequest.CloseWorkflow(intResourceWorkflow, intEnvironment,  0, dsnServiceEditor, true, intResourceRequestApprove, intAssignPage, intViewPage, dsnAsset, dsnIP);
            oOnDemandTasks.UpdateServerBackupComplete(intRequest, intItem, intNumber);
            // Update II OnDemand Task - N/A
            int intAnswer = Int32.Parse(lblAnswer.Text);
            int intModel = Int32.Parse(lblModel.Text);
            // Update Results if manual
            int intTSMAutomated = 0;
            Int32.TryParse(oSetting.Get("tsm_automated"), out intTSMAutomated);
            int intAvamarAutomated = 0;
            Int32.TryParse(oSetting.Get("avamar_automated"), out intAvamarAutomated);
            int intTSM = Int32.Parse(lblTSM.Text);
            int intAvamar = Int32.Parse(lblAvamar.Text);
            if ((intTSM > 0 && intTSMAutomated < 0) || (intAvamar > 0 && intAvamarAutomated < 0))
            {
                // If manually registered
                DataSet dsAnswer = oServer.GetAnswer(intAnswer);
                foreach (DataRow dr in dsAnswer.Tables[0].Rows)
                {
                    int intServer = Int32.Parse(dr["id"].ToString());
                    oServer.UpdateTSM(intServer, "Manually Registered");
                    oServer.UpdateTSMRegistered(intServer, DateTime.Now.ToString());
                }
            }
            // Initiate PNC Workflow (if applicable)
            PNCTasks oPNCTask = new PNCTasks(0, dsn);
            oPNCTask.InitiateNextStep(intRequest, intAnswer, intModel, intEnvironment, intAssignPage, intViewPage, dsnAsset, dsnIP, dsnServiceEditor);
            // Notify Builder
            Forecast oForecast = new Forecast(0, dsn);
            string strCC = strTSMTeam;
            if (boolProduction == true)
                strCC += strTSMTeamProd;
            else
                strCC += strTSMTeamTest;
            int intBuilder = 0;
            DataSet dsTasks = oOnDemandTasks.GetPending(intAnswer);
            if (dsTasks.Tables[0].Rows.Count > 0)
            {
                intBuilder = Int32.Parse(dsTasks.Tables[0].Rows[0]["resourceid"].ToString());
                intBuilder = Int32.Parse(oResourceRequest.GetWorkflow(intBuilder, "userid"));
            }
            if (intTSM > 0)
            {
                strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DESIGN_BUILDER");
                if (intBuilder > 0)
                    oFunction.SendEmail("TSM Backup Information", oUser.GetName(intBuilder), strCC, strEMailIdsBCC, "TSM Backup Information [" + oForecast.GetAnswer(intAnswer, "name") + "]", "<p>" + lblPreview.Text + "</p>", true, false);
                else
                    oFunction.SendEmail("TSM Backup Information", strEMailIdsBCC, strCC, "", "TSM Backup Information [" + oForecast.GetAnswer(intAnswer, "name") + "]", "<p>There are no implementors assigned to DESIGNID " + intAnswer.ToString() + "</p><p>" + lblPreview.Text + "</p>", true, false);
            }
            Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "reload", "<script type=\"text/javascript\">RefreshOpeningWindow();window.close();<" + "/" + "script>");
        }
        protected void btnRefresh_Click(Object Sender, EventArgs e)
        {
            int intResourceWorkflow = Int32.Parse(lblResourceWorkflow.Text);
            Response.Redirect(Request.Path + "?rrid=" + intResourceWorkflow.ToString() + "&div=E&refresh=refresh");
        }
        protected void btnCancelConfirm_Click(Object Sender, EventArgs e)
        {
            int intResourceWorkflow = Int32.Parse(lblResourceWorkflow.Text);
            int intResourceParent = oResourceRequest.GetWorkflowParent(intResourceWorkflow);
            oResourceRequest.UpdateWorkflowStatus(intResourceWorkflow, -2, true);
            oResourceRequest.CloseWorkflow(intResourceWorkflow, intEnvironment,  0, dsnServiceEditor, true, intResourceRequestApprove, intAssignPage, intViewPage, dsnAsset, dsnIP);
            oOnDemandTasks.UpdateServerBackupComplete(intRequest, intItem, intNumber);
            // Update II OnDemand Task - N/A
            int intAnswer = Int32.Parse(lblAnswer.Text);
            // Notify Builder
            Forecast oForecast = new Forecast(0, dsn);
            string strCC = strTSMTeam;
            if (boolProduction == true)
                strCC += strTSMTeamProd;
            else
                strCC += strTSMTeamTest;
            int intBuilder = 0;
            DataSet dsTasks = oOnDemandTasks.GetPending(intAnswer);
            if (dsTasks.Tables[0].Rows.Count > 0)
            {
                intBuilder = Int32.Parse(dsTasks.Tables[0].Rows[0]["resourceid"].ToString());
                intBuilder = Int32.Parse(oResourceRequest.GetWorkflow(intBuilder, "userid"));
            }
            oResourceRequest.AddStatus(intResourceWorkflow, 3, "Cancelled by Technician: " + txtCancel.Text, intProfile);
            int intTSM = Int32.Parse(lblTSM.Text);
            if (intTSM > 0)
            {
                strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DESIGN_BUILDER");
                if (intBuilder > 0)
                    oFunction.SendEmail("TSM Backup Cancelled", oUser.GetName(intBuilder), strCC, strEMailIdsBCC, "TSM Backup Cancelled [" + oForecast.GetAnswer(intAnswer, "name") + "]", "<p>The TSM Backup request was cancelled by <b>" + oUser.GetFullName(intProfile) + "</b> for the following reason:</p><p>" + oFunction.FormatText(txtCancel.Text) + "</p>", true, false);
                else
                    oFunction.SendEmail("TSM Backup Cancelled", strEMailIdsBCC, strCC, "", "TSM Backup Cancelled [" + oForecast.GetAnswer(intAnswer, "name") + "]", "<p>There are no implementors assigned to DESIGNID " + intAnswer.ToString() + "</p><p>The TSM Backup request was cancelled by <b>" + oUser.GetFullName(intProfile) + "</b> for the following reason:</p><p>" + oFunction.FormatText(txtCancel.Text) + "</p>", true, false);
            }
            Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "reload", "<script type=\"text/javascript\">RefreshOpeningWindow();window.close();<" + "/" + "script>");
        }
    }
}