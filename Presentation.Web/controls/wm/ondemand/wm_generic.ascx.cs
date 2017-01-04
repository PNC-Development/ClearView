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
using System.Net.NetworkInformation;
using System.Threading;
using NCC.ClearView.Application.Core.ClearViewWS;
using System.Collections.Generic;

namespace NCC.ClearView.Presentation.Web
{
    public partial class wm_generic : System.Web.UI.UserControl
    {

        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
        private string dsnZeus = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["ZeusDSN"]].ConnectionString;
        private string dsnServiceEditor = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["ServiceEditorDSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intAssignPage = Int32.Parse(ConfigurationManager.AppSettings["AssignWorkload"]);
        protected int intViewPage = Int32.Parse(ConfigurationManager.AppSettings["ViewWorkload"]);
        protected int intResourceRequestApprove = Int32.Parse(ConfigurationManager.AppSettings["ResourceRequestApprove"]);
        protected int intImplementorDistributed = Int32.Parse(ConfigurationManager.AppSettings["ITEMID_IMPLEMENTOR_DISTRIBUTED"]);
        protected int intImplementorMidrange = Int32.Parse(ConfigurationManager.AppSettings["ITEMID_IMPLEMENTOR_MIDRANGE"]);
        protected int intWorkstationPlatform = Int32.Parse(ConfigurationManager.AppSettings["WorkstationPlatformID"]);
        protected int intTest = Int32.Parse(ConfigurationManager.AppSettings["TestClassID"]);
        protected int intUnder48A = Int32.Parse(ConfigurationManager.AppSettings["DR_HOUR_RESPONSE"]);
        protected int intUnder48Q = Int32.Parse(ConfigurationManager.AppSettings["DR_HOUR_QUESTION"]);
        protected int intStorageItem = Int32.Parse(ConfigurationManager.AppSettings["ITEMID_STORAGE"]);
        protected int intBackupItem = Int32.Parse(ConfigurationManager.AppSettings["ITEMID_BACKUP"]);
        protected int intStorageService = Int32.Parse(ConfigurationManager.AppSettings["SERVICEID_STORAGE"]);
        protected int intBackupService = Int32.Parse(ConfigurationManager.AppSettings["SERVICEID_BACKUP"]);
        protected int intServerPlatformID = Int32.Parse(ConfigurationManager.AppSettings["ServerPlatformID"]);
        protected string strHA = ConfigurationManager.AppSettings["FORECAST_HIGH_AVAILABILITY"];
        protected string strServiceCenterXID = ConfigurationManager.AppSettings["ServiceCenterInputXID"];
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
        protected Forecast oForecast;
        protected Servers oServer;
        protected Storage oStorage;
        protected ModelsProperties oModelsProperties;
        protected Asset oAsset;
        protected ServerName oServerName;
        protected IPAddresses oIPAddresses;
        protected OperatingSystems oOperatingSystem;
        protected Locations oLocation;
        protected Mnemonic oMnemonic;
        protected Resiliency oResiliency;
        protected Documents oDocument;
        protected Settings oSetting;
        protected int intApplication = 0;
        protected int intPage = 0;
        protected int intProject = 0;
        protected bool boolDetails = false;
        protected bool boolExecution = false;
        protected bool boolChange = false;
        protected bool boolDocuments = false;
        protected bool boolStatusUpdates = false;
        protected int intRequest = 0;
        protected int intItem = 0;
        protected int intNumber = 0;
        protected ServiceRequests oServiceRequest;
        protected Classes oClass;
        private double dbl1 = 0.50;
        private double dbl3 = 0.25;
        private double dbl4 = 3.00;
        private double dbl5 = 0.50;
        private double dbl6 = 0.25;
        private double dbl7 = 0.00;
        private double dbl8 = 0.25;
        private double dbl9 = 0.50;
        private double dbl10 = 0.25;
        private double dbl11 = 0.25;

        protected int intDeviceCount = 1;
        protected int intAssetCount = 1;
        protected int intContainerCount = 1;
        protected string strLocation = "";
        protected Environments oEnvironment;
        protected int intLocation = Int32.Parse(ConfigurationManager.AppSettings["OPSLocationID"]);
        protected int intEnvironmentCore = Int32.Parse(ConfigurationManager.AppSettings["CoreEnvironmentID"]);
        protected string ddlNames = "";
        protected string ddlIPs = "";
        protected string ddlSVEs = "";
        protected Variables oVariable;
        protected Log oLog;
        protected PNCTasks oPNCTask;
        protected Solaris oSolaris;
        protected TSM oTSM;
        protected bool boolSVE = false;
        protected bool boolManualIP = false;
        protected bool boolManualName = false;


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
            oClass = new Classes(intProfile, dsn);
            oApplication = new Applications(intProfile, dsn);
            oOnDemandTasks = new OnDemandTasks(intProfile, dsn);
            oDelegate = new Delegates(intProfile, dsn);
            oForecast = new Forecast(intProfile, dsn);
            oServer = new Servers(0, dsn);
            oStorage = new Storage(intProfile, dsn);
            oModelsProperties = new ModelsProperties(intProfile, dsn);
            oAsset = new Asset(intProfile, dsnAsset, dsn);
            oServerName = new ServerName(intProfile, dsn);
            oIPAddresses = new IPAddresses(intProfile, dsnIP, dsn);
            oOperatingSystem = new OperatingSystems(intProfile, dsn);
            oLocation = new Locations(intProfile, dsn);
            oMnemonic = new Mnemonic(intProfile, dsn);
            oResiliency = new Resiliency(intProfile, dsn);
            oDocument = new Documents(intProfile, dsn);
            oSetting = new Settings(intProfile, dsn);

            oEnvironment = new Environments(intProfile, dsn);
            oVariable = new Variables(intEnvironment);
            oLog = new Log(intProfile, dsn);
            oPNCTask = new PNCTasks(intProfile, dsn);
            oSolaris = new Solaris(intProfile, dsn);
            oTSM = new TSM(intProfile, dsn);

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
                // Workflow start
                bool boolComplete = (oResourceRequest.GetWorkflow(intResourceWorkflow, "status") == "3");
                int intUser = Int32.Parse(oResourceRequest.GetWorkflow(intResourceWorkflow, "userid"));
                txtCustom.Text = oResourceRequest.GetWorkflow(intResourceWorkflow, "name");
                double dblAllocated = double.Parse(oResourceRequest.GetWorkflow(intResourceWorkflow, "allocated"));
                // Workflow end
                int intService = Int32.Parse(ds.Tables[0].Rows[0]["serviceid"].ToString());
                lblService.Text = oService.Get(intService, "name");
                int intApp = oRequestItem.GetItemApplication(intItem);

                double dblUsed = oResourceRequest.GetWorkflowUsed(intResourceWorkflow);
                lblUpdated.Text = oResourceRequest.GetUpdated(intResourceParent);
                dblUsed = (dblUsed / dblAllocated) * 100;
                if (Request.QueryString["save"] != null && Request.QueryString["save"] != "")
                    Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "saved", "<script type=\"text/javascript\">alert('Information Saved Successfully');<" + "/" + "script>");
                if (Request.QueryString["error"] != null && Request.QueryString["error"] != "")
                {
                    panError.Visible = true;
                    lblError.Text = oFunction.decryptQueryString(Request.QueryString["error"]);
                    Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "errored", "<script type=\"text/javascript\">alert('*** ERROR **** There was a problem with the current step...\\n\\n" + lblError.Text + "\\n\\nFor more information, please contact your ClearView administrator.');<" + "/" + "script>");
                }
                if (Request.QueryString["warning"] != null && Request.QueryString["warning"] != "")
                {
                    panWarning.Visible = true;
                    lblWarning.Text = oFunction.decryptQueryString(Request.QueryString["warning"]);
                    Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "warned", "<script type=\"text/javascript\">alert('WARNING: " + lblWarning.Text + "\\n\\nNOTE: You can skip this message by clicking the DISREGARD WARNING button.');<" + "/" + "script>");
                }
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
                        btnComplete.Attributes.Add("onclick", "return ProcessControlButton();");
                        //btnComplete.Attributes.Add("onclick", "return ValidateDropDown('" + ddlSuccess.ClientID + "','Please select a status') && confirm('Are you sure you want to mark this as completed and remove it from your workload?');");
                    }
                    else
                    {
                        oFunction.ConfigureToolButton(btnComplete, "/images/tool_complete");
                        btnComplete.Attributes.Add("onclick", "alert('This task has already been marked COMPLETE. You can close this window.');return false;");
                    }
                    //imgSuccess.ImageUrl = "/images/arrow_right.gif";
                    btnSave.ImageUrl = "/images/tool_save_dbl.gif";
                    btnSave.Enabled = false;
                    btnSave.ToolTip = "Complete";
                }
                else
                {
                    btnComplete.ImageUrl = "/images/tool_complete_dbl.gif";
                    btnComplete.Enabled = false;
                    oFunction.ConfigureToolButton(btnSave, "/images/tool_save");
                }
                btnDenied.Attributes.Add("onclick", "return CloseWindow();");
                oFunction.ConfigureToolButton(btnSave, "/images/tool_save");
                oFunction.ConfigureToolButton(btnPrint, "/images/tool_print");
                btnPrint.Attributes.Add("onclick", "return PrintWindow();");
                oFunction.ConfigureToolButton(btnClose, "/images/tool_close");
                btnClose.Attributes.Add("onclick", "return ExitWindow();");
                //btnSave.Attributes.Add("onclick", "return ValidateStatus('" + ddlStatus.ClientID + "','" + txtIssues.ClientID + "');");
                btnStatus.Attributes.Add("onclick", "return ValidateStatus('" + ddlStatus.ClientID + "','" + txtComments.ClientID + "')" +
                                        ";");
                chkDescription.Checked = (Request.QueryString["doc"] != null);

                bool boolRed = LoadStatus(intResourceWorkflow);
                //Change Control and Documents
                LoadChange(intResourceWorkflow);
                lblDocuments.Text = oDocument.GetDocuments_Service(intRequest, intService, oVariable.DocumentsFolder(), 1, (Request.QueryString["doc"] != null));

                // 6/1/2009 - Load ReadOnly View
                if (oResourceRequest.CanUpdate(intProfile, intResourceWorkflow, false) == false)
                {
                    oFunction.ConfigureToolButtonRO(btnSave, btnComplete);
                    //panDenied.Visible = true;
                }
                btnWarning.Attributes.Add("onclick", "return ProcessControlButton() && ProcessButton(this);");
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
            panAdmin.Visible = oUser.IsAdmin(intProfile);
            chkAdmin.Attributes.Add("onclick", "return ConfirmCheckBox(this,'WARNING: This action will skip all subsequent requests / workflows\\n\\nAre you sure you want to complete this request?');");
            bool boolDone = false;
            StringBuilder sbTitleDR = new StringBuilder();
            StringBuilder sbTitleProd = new StringBuilder();
            StringBuilder sbHiddenDR = new StringBuilder();
            StringBuilder sbHiddenProd = new StringBuilder();
            StringBuilder sbGenericDR = new StringBuilder();
            StringBuilder sbGenericProd = new StringBuilder();
            string strStorage = "";
            DataSet ds = oOnDemandTasks.GetGenericII(intRequest, intItem, intNumber);
            if (ds.Tables[0].Rows.Count > 0)
            {
                int intAnswer = Int32.Parse(ds.Tables[0].Rows[0]["answerid"].ToString());
                int intClass = Int32.Parse(oForecast.GetAnswer(intAnswer, "classid"));
                int intEnv = Int32.Parse(oForecast.GetAnswer(intAnswer, "environmentid"));
                int intAddress = Int32.Parse(oForecast.GetAnswer(intAnswer, "addressid"));
                bool boolPNC = (oClass.Get(intClass, "pnc") == "1");
                int intClusterNameOLD = 0;
                DataSet dsClusterName = oServer.GetAnswerClusters(intAnswer);
                if (dsClusterName.Tables[0].Rows.Count > 0)
                {
                    string strClusterNames = "";
                    string strAdditionalNames = "";
                    ServerName oServerName = new ServerName(0, dsn);
                    foreach (DataRow drClusterName in dsClusterName.Tables[0].Rows)
                    {
                        int intClusterNameID = Int32.Parse(drClusterName["clusterid"].ToString());
                        if (intClusterNameOLD != intClusterNameID && intClusterNameID > 0)
                        {
                            panAdditional.Visible = true;
                            if (strClusterNames != "" || strAdditionalNames != "")
                            {
                                if (lblAdditional.Text != "")
                                    lblAdditional.Text += "<br/>";
                                lblAdditional.Text += strAdditionalNames + " (" + strClusterNames + ")";
                            }
                            DataSet dsServerNames = oServer.GetClusters(intClusterNameID);
                            foreach (DataRow drServerName in dsServerNames.Tables[0].Rows)
                            {
                                int intServerName = Int32.Parse(drServerName["id"].ToString());
                                if (strClusterNames != "")
                                    strClusterNames += ", ";
                                strClusterNames += oServer.GetName(intServerName, boolUsePNCNaming);
                            }
                            DataSet dsAdditionalNames = oServerName.GetRelated(intAnswer, intClusterNameID);
                            foreach (DataRow drAdditionalName in dsAdditionalNames.Tables[0].Rows)
                            {
                                int intAdditionalName = Int32.Parse(drAdditionalName["nameid"].ToString());
                                if (strAdditionalNames != "")
                                    strAdditionalNames += ", ";
                                if (boolPNC == true)
                                    strAdditionalNames += oServerName.GetNameFactory(intAdditionalName, 0);
                                else
                                    strAdditionalNames += oServerName.GetName(intAdditionalName, 0);
                            }
                        }
                        intClusterNameOLD = intClusterNameID;
                    }
                    if (strClusterNames != "" || strAdditionalNames != "")
                    {
                        if (lblAdditional.Text != "")
                            lblAdditional.Text += "<br/>";
                        lblAdditional.Text += strAdditionalNames + " (" + strClusterNames + ")";
                    }
                }
                else if (oForecast.IsHARoom(intAnswer) == true)
                {
                    DataSet dsServer = oServer.GetAnswer(intAnswer);
                    foreach (DataRow drServer in dsServer.Tables[0].Rows)
                    {
                        int intHA = Int32.Parse(drServer["id"].ToString());
                        DataSet dsHA = oServer.GetHA(intHA);
                        if (dsHA.Tables[0].Rows.Count > 0)
                        {
                            if (lblHA.Text != "")
                                lblHA.Text += "<br/>";
                            lblHA.Text += oServer.GetName(intHA, true) + " = " + oServer.GetName(Int32.Parse(dsHA.Tables[0].Rows[0]["serverid_ha"].ToString()), true);
                            panHA.Visible = true;
                        }
                    }
                }
                lblAnswer.Text = intAnswer.ToString();
                //btnView.Attributes.Add("onclick", "return OpenWindow('FORECAST_EQUIPMENT','?id=" + intAnswer.ToString() + "');");
                btnView.Attributes.Add("onclick", "OpenNewWindow('/datapoint/service/design.aspx?t=design&q=" + oFunction.encryptQueryString(intAnswer.ToString()) + "',800,600);return false;");
                btnView.ToolTip = intAnswer.ToString();
                //btnBirth.Attributes.Add("onclick", "return OpenWindow('PDF_BIRTH','?id=" + intAnswer.ToString() + "');");
                //btnSC.Attributes.Add("onclick", "return OpenWindow('PDF_BIRTH','?id=" + intAnswer.ToString() + "');");
                //lblView.Text = oOnDemandTasks.GetBody(intAnswer, intImplementorDistributed, intImplementorMidrange);
                lblExecutedOn.Text = oForecast.GetAnswer(intAnswer, "executed");
                int intExecutedBy = 0;
                Int32.TryParse(oForecast.GetAnswer(intAnswer, "executed_by"), out intExecutedBy);
                lblExecutedBy.Text = oUser.GetFullName(intExecutedBy);
                int intModel = Int32.Parse(ds.Tables[0].Rows[0]["modelid"].ToString());
                boolSVE = oModelsProperties.IsSUNVirtual(intModel);
                boolManualName = oModelsProperties.IsEnterName(intModel);
                boolManualIP = oModelsProperties.IsEnterIP(intModel);
                lblModel.Text = oModelsProperties.Get(intModel, "name");
                lblModel.ToolTip = intModel.ToString();
                lblModelID.Text = intModel.ToString();
                int intParent = Int32.Parse(oModelsProperties.Get(intModel, "modelid"));
                Models oModel = new Models(intProfile, dsn);
                int intType = Int32.Parse(oModel.Get(intParent, "typeid"));
                Types oType = new Types(intProfile, dsn);
                string strExecute = oType.Get(intType, "forecast_execution_path");
                chk1.Checked = (ds.Tables[0].Rows[0]["chk1"].ToString() != "");
                chk1.ToolTip = ds.Tables[0].Rows[0]["chk1"].ToString();
                chk3.Checked = (ds.Tables[0].Rows[0]["chk3"].ToString() != "");
                chk3.ToolTip = ds.Tables[0].Rows[0]["chk3"].ToString();
                chk4.Checked = (ds.Tables[0].Rows[0]["chk4"].ToString() != "");
                chk4.ToolTip = ds.Tables[0].Rows[0]["chk4"].ToString();
                chk5.Checked = (ds.Tables[0].Rows[0]["chk5"].ToString() != "");
                chk5.ToolTip = ds.Tables[0].Rows[0]["chk5"].ToString();
                chk6.Checked = (ds.Tables[0].Rows[0]["chk6"].ToString() != "");
                chk6.ToolTip = ds.Tables[0].Rows[0]["chk6"].ToString();
                // Last 3 are for moved assets only
                chk7.Checked = (ds.Tables[0].Rows[0]["chk7"].ToString() != "");
                chk7.ToolTip = ds.Tables[0].Rows[0]["chk7"].ToString();
                chk8.Checked = (ds.Tables[0].Rows[0]["chk8"].ToString() != "");
                chk8.ToolTip = ds.Tables[0].Rows[0]["chk8"].ToString();
                chk9.Checked = (ds.Tables[0].Rows[0]["chk9"].ToString() != "");
                chk9.ToolTip = ds.Tables[0].Rows[0]["chk9"].ToString();

                int intNotificationsTest = Int32.Parse(ds.Tables[0].Rows[0]["notifications_test"].ToString());
                //btnGenerateTest.Enabled = false;
                int intNotificationsProd = Int32.Parse(ds.Tables[0].Rows[0]["notifications_prod"].ToString());
                //btnGenerateProd.Enabled = false;

                if (!IsPostBack)
                {
                    img1.ImageUrl = (panError.Visible ? "/images/bigError.gif" : (panWarning.Visible ? "/images/bigAlert.gif" : "/images/arrow_right.gif"));
                    chk1.Enabled = true;
                    tr1.Visible = true;

                    string strAdditional = "";
                    DataSet dsAnswer = oServer.GetAnswer(intAnswer);
                    string strServers = "";
                    foreach (DataRow drAnswer in dsAnswer.Tables[0].Rows)
                    {
                        int intServer = Int32.Parse(drAnswer["id"].ToString());
                        strServers += intServer.ToString() + ";";
                        string strName = oServer.GetName(intServer, boolUsePNCNaming);
                        int intClusterID = Int32.Parse(drAnswer["clusterid"].ToString());
                        DataSet dsAdditional = oServerName.GetRelated(intAnswer, intClusterID);
                        foreach (DataRow drAdditional in dsAdditional.Tables[0].Rows)
                            strAdditional += oServerName.GetName(Int32.Parse(drAdditional["nameid"].ToString()), 0) + ", ";
                    }

                    litStorage.Text = GetStorageShared(intAnswer, intClass, intModel);

                    bool boolMover = false;
                    //if (boolPNC == false && (intEnv != intEnvironmentCore || oClass.IsProd(intClass)))
                    if (intEnv != intEnvironmentCore || (boolPNC == false && oClass.IsProd(intClass)))
                        boolMover = true;

                    if (oModelsProperties.IsVMwareVirtual(intModel) == true || oModelsProperties.IsIBMVirtual(intModel) == true || oModelsProperties.IsSUNVirtual(intModel) == true)
                    {
                        radAutoVirtual.Checked = true;
                        radAutoVirtual.ToolTip = "Virtual Models must be auto-generated by ClearView";
                        radAutoYes.Enabled = false;
                        radAutoNo.Enabled = false;
                    }
                    else
                    {
                        radAutoVirtual.Enabled = false;
                    }

                    if (chk1.Checked == true)
                    {
                        img1.ImageUrl = "/images/bigCheckBox.gif";
                        chk1.Enabled = false;
                        tr1.Visible = false;
                        tr1Done.Visible = true;
                        DateTime dat1 = DateTime.Now;
                        DateTime.TryParse(ds.Tables[0].Rows[0]["chk1"].ToString(), out dat1);
                        lbl1.Text = dat1.ToLongDateString() + " @ " + dat1.ToLongTimeString();
                        img3.ImageUrl = (panError.Visible ? "/images/bigError.gif" : (panWarning.Visible ? "/images/bigAlert.gif" : "/images/arrow_right.gif"));
                        chk3.Enabled = true;
                        tr3.Visible = true;
                    }
                    if (chk3.Checked == true)
                    {
                        img3.ImageUrl = "/images/bigCheckBox.gif";
                        chk3.Enabled = false;
                        tr3.Visible = false;
                        tr3Done.Visible = true;
                        DateTime dat3 = DateTime.Now;
                        DateTime.TryParse(ds.Tables[0].Rows[0]["chk3"].ToString(), out dat3);
                        lbl3.Text = dat3.ToLongDateString() + " @ " + dat3.ToLongTimeString();
                        img4.ImageUrl = (panError.Visible ? "/images/bigError.gif" : (panWarning.Visible ? "/images/bigAlert.gif" : "/images/arrow_right.gif"));
                        chk4.Enabled = true;
                        tr4.Visible = true;
                    }
                    if (chk4.Checked == true)
                    {
                        img4.ImageUrl = "/images/bigCheckBox.gif";
                        chk4.Enabled = false;
                        tr4.Visible = false;
                        tr4Done.Visible = true;
                        DateTime dat4 = DateTime.Now;
                        DateTime.TryParse(ds.Tables[0].Rows[0]["chk4"].ToString(), out dat4);
                        lbl4.Text = dat4.ToLongDateString() + " @ " + dat4.ToLongTimeString();
                        img5.ImageUrl = (panError.Visible ? "/images/bigError.gif" : (panWarning.Visible ? "/images/bigAlert.gif" : "/images/arrow_right.gif"));
                        chk5.Enabled = true;
                        tr5.Visible = true;
                    }
                    if (chk5.Checked == true)
                    {
                        img5.ImageUrl = "/images/bigCheckBox.gif";
                        chk5.Enabled = false;
                        tr5.Visible = false;
                        tr5Done.Visible = true;
                        DateTime dat5 = DateTime.Now;
                        DateTime.TryParse(ds.Tables[0].Rows[0]["chk5"].ToString(), out dat5);
                        lbl5.Text = dat5.ToLongDateString() + " @ " + dat5.ToLongTimeString();
                        img6.ImageUrl = (panError.Visible ? "/images/bigError.gif" : (panWarning.Visible ? "/images/bigAlert.gif" : "/images/arrow_right.gif"));
                        chk6.Enabled = true;
                        tr6.Visible = true;
                    }

                    if (chk6.Checked == true)
                    {
                        img6.ImageUrl = "/images/bigCheckBox.gif";
                        chk6.Enabled = false;
                        tr6.Visible = false;
                        tr6Done.Visible = true;
                        DateTime dat6 = DateTime.Now;
                        DateTime.TryParse(ds.Tables[0].Rows[0]["chk6"].ToString(), out dat6);
                        lbl6.Text = dat6.ToLongDateString() + " @ " + dat6.ToLongTimeString();
                        img7.ImageUrl = (panError.Visible ? "/images/bigError.gif" : (panWarning.Visible ? "/images/bigAlert.gif" : "/images/arrow_right.gif"));
                        chk7.Enabled = true;
                        tr7.Visible = true;
                    }

                    if (boolMover == true)
                    {
                        panMove.Visible = true;
                        if (chk7.Checked == true)
                        {
                            img7.ImageUrl = "/images/bigCheckBox.gif";
                            chk7.Enabled = false;
                            tr7.Visible = false;
                            tr7Done.Visible = true;
                            DateTime dat7 = DateTime.Now;
                            DateTime.TryParse(ds.Tables[0].Rows[0]["chk7"].ToString(), out dat7);
                            lbl7.Text = dat7.ToLongDateString() + " @ " + dat7.ToLongTimeString();
                            img8.ImageUrl = (panError.Visible ? "/images/bigError.gif" : (panWarning.Visible ? "/images/bigAlert.gif" : "/images/arrow_right.gif"));
                            chk8.Enabled = true;
                            tr8.Visible = true;
                        }
                        if (chk8.Checked == true)
                        {
                            img8.ImageUrl = "/images/bigCheckBox.gif";
                            chk8.Enabled = false;
                            tr8.Visible = false;
                            tr8Done.Visible = true;
                            DateTime dat8 = DateTime.Now;
                            DateTime.TryParse(ds.Tables[0].Rows[0]["chk8"].ToString(), out dat8);
                            lbl8.Text = dat8.ToLongDateString() + " @ " + dat8.ToLongTimeString();
                            img9.ImageUrl = (panError.Visible ? "/images/bigError.gif" : (panWarning.Visible ? "/images/bigAlert.gif" : "/images/arrow_right.gif"));
                            chk9.Enabled = true;
                            tr9.Visible = true;
                        }
                        if (chk9.Checked == true)
                        {
                            img9.ImageUrl = "/images/bigCheckBox.gif";
                            chk9.Enabled = false;
                            tr9.Visible = false;
                            tr9Done.Visible = true;
                            DateTime dat9 = DateTime.Now;
                            DateTime.TryParse(ds.Tables[0].Rows[0]["chk9"].ToString(), out dat9);
                            lbl9.Text = dat9.ToLongDateString() + " @ " + dat9.ToLongTimeString();
                        }
                    }
                    else
                    {
                        panMove.Visible = false;
                        chk7.Checked = true;
                        chk7.Enabled = false;
                        img7.ImageUrl = "/images/cancel.gif";
                        chk8.Checked = true;
                        chk8.Enabled = false;
                        img8.ImageUrl = "/images/cancel.gif";
                        chk9.Checked = true;
                        chk9.Enabled = false;
                        img9.ImageUrl = "/images/cancel.gif";
                    }

                    boolDone = (chk1.Checked && chk3.Checked && chk4.Checked && chk5.Checked && chk6.Checked && (chk7.Checked || panMove.Visible == false) && (chk8.Checked || panMove.Visible == false) && (chk9.Checked || panMove.Visible == false));
                    
                    tr3Wait.Visible = (tr3.Visible == false && tr3Done.Visible == false);
                    tr4Wait.Visible = (tr4.Visible == false && tr4Done.Visible == false);
                    tr5Wait.Visible = (tr5.Visible == false && tr5Done.Visible == false);
                    tr6Wait.Visible = (tr6.Visible == false && tr6Done.Visible == false);
                    tr7Wait.Visible = (tr7.Visible == false && tr7Done.Visible == false);
                    tr8Wait.Visible = (tr8.Visible == false && tr8Done.Visible == false);
                    tr9Wait.Visible = (tr9.Visible == false && tr9Done.Visible == false);

                    int intSVEClusterID = 0;
                    DataSet dsManual = oServer.GetManual(intAnswer, false);
                    rptServers.DataSource = dsManual;
                    rptServers.DataBind();
                    foreach (RepeaterItem ri in rptServers.Items)
                    {
                        Label lblName = (Label)ri.FindControl("lblName");
                        if (lblName.Text != "--- Pending ---")
                            lblName.Text = "<a href=\"javascript:void(0);\" onclick=\"OpenNewWindow('/datapoint/asset/datapoint_asset_search.aspx?t=name&q=" + oFunction.encryptQueryString(lblName.Text) + "&id=" + oFunction.encryptQueryString(lblName.ToolTip) + "',800,600);\">" + lblName.Text + "</a>";
                        else
                            lblName.Text = "<i>" + lblName.Text + "</i>";

                        Label lblStorage = (Label)ri.FindControl("lblStorage");
                        Literal litStorageConfig = (Literal)ri.FindControl("litStorageConfig");
                        int intServerCluster = Int32.Parse(lblStorage.Text);
                        int intServerNumber = Int32.Parse(lblStorage.ToolTip);
                        lblStorage.Text = "[<a href=\"javascript:void(0);\" onclick=\"ShowHideDiv2('trStorage" + lblStorage.Text + "_" + lblStorage.ToolTip + "');\">Show</a>]";
                        litStorageConfig.Text = GetStorage(intAnswer, intClass, intServerCluster, 0, intServerNumber, intModel);
                        if (litStorageConfig.Text == "")
                            lblStorage.Text = "<span disabled=\"disabled\">[None]</span>";

                        Label lblAsset = (Label)ri.FindControl("lblAsset");
                        if (lblAsset.Text != "--- Pending ---")
                            lblAsset.Text = "<a href=\"javascript:void(0);\" onclick=\"OpenNewWindow('/datapoint/asset/datapoint_asset_search.aspx?t=serial&q=" + oFunction.encryptQueryString(lblAsset.Text) + "',800,600);\">" + lblAsset.Text + "</a>";
                        else
                            lblAsset.Text = "<i>" + lblAsset.Text + "</i>";
                        Label lblAssetDR = (Label)ri.FindControl("lblAssetDR");
                        if (lblAssetDR.Text != "--- None ---")
                        {
                            if (lblAssetDR.Text != "--- Pending ---")
                            {
                                if (lblAssetDR.Text != "Missing!!!")
                                    lblAssetDR.Text = "<a href=\"javascript:void(0);\" onclick=\"OpenNewWindow('/datapoint/asset/datapoint_asset_search.aspx?t=serial&q=" + oFunction.encryptQueryString(lblAssetDR.Text) + "',800,600);\">" + lblAssetDR.Text + "</a>";
                                else
                                    lblAssetDR.Text = "<span class=\"highlight\">" + lblAssetDR.Text + "</span>";
                            }
                            else
                                lblAssetDR.Text = "<i>" + lblAssetDR.Text + "</i>";
                        }
                        else
                            lblAssetDR.Text = "<i>Not Required</i>";

                        Label lblIP1 = (Label)ri.FindControl("lblIP1");
                        if (lblIP1.Text == "--- Pending ---")
                            lblIP1.Text = "<i>" + lblIP1.Text + "</i>";
                        Label lblIP2 = (Label)ri.FindControl("lblIP2");
                        if (lblIP2.Text == "--- Pending ---")
                            lblIP2.Text = "<i>" + lblIP2.Text + "</i>";
                        Label lblIP3 = (Label)ri.FindControl("lblIP3");
                        if (lblIP3.Text == "--- Pending ---")
                            lblIP3.Text = "<i>" + lblIP3.Text + "</i>";

                        Label lblServiceCenter = (Label)ri.FindControl("lblServiceCenter");
                        string strServiceCenter = oVariable.UploadsFolder() + "SC\\" + "SC_" + lblServiceCenter.Text + "_" + oProject.Get(intProject, "number") + ".HTM";
                        System.IO.FileInfo oFile = new System.IO.FileInfo(strServiceCenter);
                        if (oFile.Exists == true)
                            lblServiceCenter.Text = "<a href=\"" + strServiceCenter + "\" target=\"_blank\" title=\"Click here for the service center form\"><img src=\"/images/icons/html.gif\" border=\"0\" align=\"absmiddle\"/></a>";
                        else
                            lblServiceCenter.Text = "<input type=\"image\" src=\"/images/icons/html.gif\" class=\"disabledImage\" disabled=\"disabled\" title=\"No Service Center Form\" />";

                        Label lblDNS = (Label)ri.FindControl("lblDNS");
                        int intServerDNS = Int32.Parse(lblDNS.Text);
                        string strDNS = oServer.Get(intServerDNS, "dns_auto");
                        string strDNSComments = oServer.Get(intServerDNS, "dns_output");
                        if (strDNS == "-100" || strDNSComments == "")
                            lblDNS.Text = "<img src=\"/images/pending.gif\" border=\"0\" align=\"absmiddle\" title=\"Waiting for the server to be built\"/>";
                        else if (strDNS == "1")
                            lblDNS.Text = "<img src=\"/images/check.gif\" border=\"0\" align=\"absmiddle\" title=\"" + strDNSComments + "\"/>";
                        else
                            lblDNS.Text = "<img src=\"/images/error.gif\" border=\"0\" align=\"absmiddle\" title=\"" + strDNSComments + "\"/>";

                        if (boolSVE == true)
                        {
                            Label lblSVECluster = (Label)ri.FindControl("lblSVECluster");
                            Int32.TryParse(lblSVECluster.ToolTip, out intSVEClusterID);
                            lblSVECluster.Text += (oSolaris.GetSVECluster(intSVEClusterID, "trunking") == "1" ? " (Trunked)" : " (Not Trunked)");
                        }
                    }

                    // Load TDPO Information
                    bool boolTDPO = true;
                    bool boolSVEClustered = true;
                    string strSVEType = "";
                    if (boolSVE == true)
                    {
                        int intSubApplication = 0;
                        if (oForecast.GetAnswer(intAnswer, "subapplicationid") != "")
                            intSubApplication = Int32.Parse(oForecast.GetAnswer(intAnswer, "subapplicationid"));
                        if (intSubApplication > 0)
                        {
                            lblSVEName.Text = oServerName.GetSubApplication(intSubApplication, "name");
                            boolSVEClustered = (oServerName.GetSubApplication(intSubApplication, "code") == "Z");
                            strSVEType = oServerName.GetSubApplication(intSubApplication, "factory_code");
                        }

                        bool boolContainer = false;
                        foreach (DataRow drManual in dsManual.Tables[0].Rows)
                        {
                            int intServer = Int32.Parse(drManual["id"].ToString());
                            if (oServer.Get(intServer, "tsm_bypass") == "0")
                            {
                                // Since the TSM was not bypassed, show the TDPO information
                                panBackup.Visible = true;
                                // Get Container Name
                                string strContainer = oServer.GetName(intServer, true);
                                if (strSVEType == "DB" && strContainer.EndsWith("A") == true)
                                {
                                    strContainer = strContainer.Substring(0, strContainer.Length - 1);
                                    boolContainer = true;
                                }

                                int intSchedule = 0;
                                Int32.TryParse(oServer.Get(intServer, "tsm_schedule"), out intSchedule);
                                string strOutput = oServer.Get(intServer, "tsm_output");

                                if (oLocation.GetAddress(intAddress, "tsm") == "1")
                                {
                                    if (oForecast.GetAnswer(intAnswer, "backup") == "1")
                                    {
                                        DataSet dsBackupTask = oOnDemandTasks.GetServerBackup(intAnswer);
                                        if (dsBackupTask.Tables[0].Rows.Count > 0 && dsBackupTask.Tables[0].Rows[0]["completed"] == "")
                                        {
                                            lblBackup.Text = "<img src=\"/images/active.gif\" align=\"absmiddle\" border=\"0\" title=\"CVT" + dsBackupTask.Tables[0].Rows[0]["requestid"].ToString() + "\"/> Backup is being performed manually";
                                            boolTDPO = false;
                                        }
                                        else if (strOutput == "PENDING")
                                        {
                                            lblBackup.Text = "<img src=\"/images/active.gif\" align=\"absmiddle\" border=\"0\" title=\"" + strOutput + "\"/> Backup Automation in process....continue to refresh the page....";
                                            boolTDPO = false;
                                            break;
                                        }
                                        else if (intSchedule == 0)
                                        {
                                            lblBackup.Text = "<i>Not available at this time...</i>";
                                            boolTDPO = false;
                                            break;
                                        }
                                        else
                                        {
                                            string strSchedule = oTSM.GetSchedule(intSchedule, "name");
                                            int intDomain = Int32.Parse(oTSM.GetSchedule(intSchedule, "domain"));
                                            string strDomain = oTSM.GetDomain(intDomain, "name");
                                            int intTSMServer = Int32.Parse(oTSM.GetDomain(intDomain, "tsm"));
                                            string strServer = oTSM.Get(intTSMServer, "name");
                                            string strPort = oTSM.Get(intTSMServer, "port");
                                            lblBackup.Text += "<p><img src=\"/images/check.gif\" align=\"absmiddle\" border=\"0\"/> <b><u>" + strContainer + "</u></b> has been registered on " + strServer + ", TCPPort " + strPort + "</p>";
                                            if (boolContainer == true)
                                            {
                                                // No other servers to do since this is a multi-node container.
                                                break;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        lblBackup.Text = "<p><img src=\"/images/check.gif\" align=\"absmiddle\" border=\"0\"/> Backup was not requested for <b><u>" + strContainer + "</u></b></p>";
                                        break;
                                    }
                                }
                                else
                                {
                                    lblBackup.Text = "<p><img src=\"/images/check.gif\" align=\"absmiddle\" border=\"0\"/> Backup was not available for <b><u>" + strContainer + "</u></b></p>";
                                    break;
                                }
                            }
                        }
                    }

                    if (chk1.Enabled == true)
                    {
                        ddlClass.DataTextField = "name";
                        ddlClass.DataValueField = "id";
                        ddlClass.DataSource = oClass.GetForecasts(1);
                        ddlClass.DataBind();
                        ddlClass.Items.Insert(0, new ListItem("-- SELECT --", "0"));
                        try
                        {
                            ddlClass.SelectedValue = intClass.ToString();
                        }
                        catch { }
                        if (ddlClass.SelectedIndex == 0)
                        {
                            // Class is not available, add it.
                            ddlClass.Items.Add(new ListItem(oClass.Get(intClass, "name") + " *", intClass.ToString()));
                            ddlClass.SelectedValue = intClass.ToString();
                        }
                        hdnEnvironment.Value = intEnv.ToString();
                        ddlEnvironment.SelectedValue = intEnv.ToString();
                        ddlEnvironment.Enabled = true;
                        ddlEnvironment.DataTextField = "name";
                        ddlEnvironment.DataValueField = "id";
                        ddlEnvironment.DataSource = oClass.GetEnvironment(intClass, 1);
                        ddlEnvironment.DataBind();
                        ddlEnvironment.Items.Insert(0, new ListItem("-- SELECT --", "0"));
                        ddlClass.Attributes.Add("onchange", "PopulateEnvironments('" + ddlClass.ClientID + "','" + ddlEnvironment.ClientID + "',1);");
                        ddlEnvironment.Attributes.Add("onchange", "UpdateDropDownHidden('" + ddlEnvironment.ClientID + "','" + hdnEnvironment.ClientID + "');");
                        strLocation = oLocation.LoadDDL("ddlState", "ddlCity", "ddlAddress", hdnLocation.ClientID, intAddress, true, "ddlCommon");
                        hdnLocation.Value = intAddress.ToString();
                        radAutoYes.Attributes.Add("onclick", "ShowHideDiv('" + trAutoYes.ClientID + "','inline');ShowHideDiv('" + trAutoNo.ClientID + "','none');");
                        radAutoNo.Attributes.Add("onclick", "ShowHideDiv('" + trAutoNo.ClientID + "','inline');ShowHideDiv('" + trAutoYes.ClientID + "','none');");
                        rptAssets.DataSource = dsManual;
                        rptAssets.DataBind();

                        if (boolSVE == true)
                        {
                            //trSVE.Visible = true;
                            //Load SVEs
                            /*
                            StringBuilder strSVEs = new StringBuilder();
                            DataSet dsSVEs = oSolaris.GetSVEClusters(1, 1);
                            DataView dvSVEs = dsSVEs.Tables[0].DefaultView;
                            dvSVEs.Sort = "name ASC";
                            foreach (DataRowView drSVE in dvSVEs)
                            {
                                strSVEs.Append("<option value=\"");
                                int intSVE = Int32.Parse(drSVE["id"].ToString());
                                string strSVEHosts = "";
                                DataSet dsSVEHosts = oServer.GetSVEClusters(intSVE);
                                foreach (DataRow drSVEHost in dsSVEHosts.Tables[0].Rows)
                                {
                                    if (strSVEHosts != "")
                                        strSVEHosts += ", ";
                                    strSVEHosts += drSVEHost["servername"].ToString();
                                }
                                strSVEs.Append(drSVE["id"].ToString());
                                strSVEs.Append("\">");
                                strSVEs.Append(drSVE["name"].ToString());
                                strSVEs.Append(" (");
                                strSVEs.Append(strSVEHosts);
                                strSVEs.Append(")");
                                strSVEs.Append("</option>");
                            }
                            ddlSVEs = strSVEs.ToString();
                            rptSVE.DataSource = dsManual;
                            rptSVE.DataBind();
                            */
                        }

                        btnSave.Attributes.Add("onclick", "return EnsureManualStep1('" + radAutoYes.ClientID + "','" + radAutoNo.ClientID + "','" + radAutoVirtual.ClientID + "','" + ddlClass.ClientID + "','" + ddlEnvironment.ClientID + "','" + hdnLocation.ClientID + "','" + strServers + "') && ProcessControlButton();");
                        if (strAdditional != "")
                            lblAdditional.Text += "<b>Additional Names:</b>" + strAdditional;

                    }
                    if (chk3.Enabled == true)
                    {
                        //Load IPs
                        StringBuilder strIPs = new StringBuilder();
                        DataSet dsIPs = oIPAddresses.GetMineUnused(intProfile);
                        DataView dvIPs = dsIPs.Tables[0].DefaultView;
                        if (Request.QueryString["sortip"] != null)
                        {
                            radNamesNo.Checked = true;
                            trNamesNo.Style["display"] = "inline";
                            ddlOrderIP.SelectedValue = Request.QueryString["sortip"];
                            dvIPs.Sort = Request.QueryString["sortip"].ToString();
                        }
                        else
                            dvIPs.Sort = "modified DESC";
                        foreach (DataRowView drIP in dvIPs)
                        {
                            strIPs.Append("<option value=\"");
                            strIPs.Append(drIP["id"].ToString());
                            strIPs.Append("\">");
                            strIPs.Append(drIP["name"].ToString());
                            strIPs.Append(" (");
                            strIPs.Append(drIP["custom"].ToString());
                            strIPs.Append(")");
                            strIPs.Append("</option>");
                        }
                        ddlIPs = strIPs.ToString();

                        //Load Names
                        StringBuilder strNames = new StringBuilder();
                        DataSet dsNames = oServerName.GetMineUnused(intProfile, (boolPNC ? 1 : 0));
                        DataView dvNames = dsNames.Tables[0].DefaultView;
                        if (Request.QueryString["sortname"] != null)
                        {
                            radNamesNo.Checked = true;
                            trNamesNo.Style["display"] = "inline";
                            ddlOrderName.SelectedValue = Request.QueryString["sortname"];
                            dvNames.Sort = Request.QueryString["sortname"].ToString();
                        }
                        else
                            dvNames.Sort = "modified DESC";
                        foreach (DataRowView drName in dvNames)
                        {
                            strNames.Append("<option value=\"");
                            strNames.Append(drName["id"].ToString());
                            strNames.Append("\">");
                            strNames.Append(drName["servername"].ToString());
                            strIPs.Append(" (");
                            strIPs.Append(drName["name"].ToString());
                            strIPs.Append(")");
                            strNames.Append("</option>");
                        }
                        ddlNames = strNames.ToString();

                        radNamesYes.Attributes.Add("onclick", "ShowHideDiv('" + trNamesNo.ClientID + "','none');ShowHideDiv('" + panAdditionalName.ClientID + "','none');ShowHideDiv('" + panAdditionalIP.ClientID + "','none');");
                        radNamesNo.Attributes.Add("onclick", "ShowHideDiv('" + trNamesNo.ClientID + "','inline');ShowHideDiv('" + panAdditionalName.ClientID + "','" + (boolManualName ? "inline" : "none") + "');ShowHideDiv('" + panAdditionalIP.ClientID + "','" + (boolManualIP ? "inline" : "none") + "');");
                        rptNames.DataSource = dsManual;
                        rptNames.DataBind();
                        imgSearchName.Attributes.Add("onclick", "return FilterDropDown('" + txtSearchName.ClientID + "','SEARCH_NAME');");
                        imgSearchIP1.Attributes.Add("onclick", "return FilterDropDown('" + txtSearchIP1.ClientID + "','SEARCH_IP1');");
                        imgSearchIP2.Attributes.Add("onclick", "return FilterDropDown('" + txtSearchIP2.ClientID + "','SEARCH_IP2');");
                        imgSearchIP3.Attributes.Add("onclick", "return FilterDropDown('" + txtSearchIP3.ClientID + "','SEARCH_IP3');");
                        txtSearchName.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + imgSearchName.ClientID + "').click();return false;}} else {return true}; ");
                        txtSearchIP1.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + imgSearchIP1.ClientID + "').click();return false;}} else {return true}; ");
                        txtSearchIP2.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + imgSearchIP2.ClientID + "').click();return false;}} else {return true}; ");
                        txtSearchIP3.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + imgSearchIP3.ClientID + "').click();return false;}} else {return true}; ");
                        btnSave.Attributes.Add("onclick", "return EnsureManualStep2('" + radNamesYes.ClientID + "','" + radNamesNo.ClientID + "','" + strServers + "') && ProcessControlButton();");
                    }
                    if (chk4.Enabled == true)
                    {
                        if (oModelsProperties.IsSUNVirtual(intModel))
                        {
                            // SAN is required prior to building.
                            panPreBuildFormsYes.Visible = true;
                            bool boolCompleted = true;
                            int intProd = (oClass.IsProd(intClass) ? 1 : 0);
                            DataSet dsSAN = oOnDemandTasks.GetServerStorage(intAnswer, intProd);
                            if (dsSAN.Tables[0].Rows.Count == 0)
                            {
                                if (oSolaris.GetSVECluster(intSVEClusterID, "storage_allocated") != "1")
                                {
                                    // Generate Request and Reload
                                    int intQuantity = Int32.Parse(oResourceRequest.GetWorkflow(_request_workflow, "devices"));
                                    int intStorageNumber = oResourceRequest.GetNumber(intRequest, intStorageItem);
                                    oOnDemandTasks.AddServerStorage(intRequest, intStorageItem, intStorageNumber, intAnswer, intProd, intModel);
                                    int intStorage = oResourceRequest.Add(intRequest, intStorageItem, intStorageService, intStorageNumber, "Auto-Provisioning Task (Storage)", intQuantity, 0.00, 2, 1, 1, 1);
                                    if (oServiceRequest.NotifyApproval(intStorage, intResourceRequestApprove, intEnvironment, "", dsnServiceEditor) == false)
                                        oServiceRequest.NotifyTeamLead(intStorageItem, intStorage, intAssignPage, intViewPage, intEnvironment, "", dsnServiceEditor, dsnAsset, dsnIP, 0);

                                    if (intProd == 1)
                                        oOnDemandTasks.UpdateGenericIINotificationsProd(intRequest, intItem, intNumber);
                                    else
                                        oOnDemandTasks.UpdateGenericIINotificationsTest(intRequest, intItem, intNumber);
                                    // Reload
                                    Response.Redirect(Request.Url.PathAndQuery);
                                }
                                else
                                    panPreBuildFormsNo.Visible = true;
                            }
                            else
                            {
                                int intRequestSAN = Int32.Parse(dsSAN.Tables[0].Rows[0]["requestid"].ToString());
                                int intNumberSAN = Int32.Parse(dsSAN.Tables[0].Rows[0]["number"].ToString());
                                rptPreBuildForms.DataSource = oResourceRequest.GetAllService(intRequestSAN, intStorageService, intNumberSAN);
                                rptPreBuildForms.DataBind();
                                foreach (RepeaterItem ri in rptPreBuildForms.Items)
                                {
                                    Label lblImage = (Label)ri.FindControl("lblImage");
                                    Label lblRequestID = (Label)ri.FindControl("lblRequestID");
                                    Label lblService = (Label)ri.FindControl("lblService");
                                    Label lblCreated = (Label)ri.FindControl("lblCreated");
                                    Label lblAssignedTo = (Label)ri.FindControl("lblAssignedTo");
                                    Label lblAssignedOn = (Label)ri.FindControl("lblAssignedOn");
                                    Label lblModified = (Label)ri.FindControl("lblModified");
                                    Label lblStatus = (Label)ri.FindControl("lblStatus");

                                    if (lblStatus.ToolTip == "1")
                                        ri.Visible = false;
                                    else
                                    {
                                        int intRR = Int32.Parse(lblRequestID.Text);
                                        int intRRW = Int32.Parse(lblAssignedTo.Text);

                                        DataSet dsRR = oResourceRequest.Get(intRR);
                                        if (dsRR.Tables[0].Rows.Count > 0)
                                        {
                                            int intRequestRR = Int32.Parse(dsRR.Tables[0].Rows[0]["requestid"].ToString());
                                            int intServiceRR = Int32.Parse(dsRR.Tables[0].Rows[0]["serviceid"].ToString());
                                            int intNumberRR = Int32.Parse(dsRR.Tables[0].Rows[0]["number"].ToString());
                                            lblService.Text = oService.GetName(intServiceRR);
                                            lblCreated.Text = dsRR.Tables[0].Rows[0]["created"].ToString();
                                            lblModified.Text = dsRR.Tables[0].Rows[0]["modified"].ToString();
                                            if (intRRW == 0)
                                            {
                                                lblAssignedTo.Text = "Pending Assignment";
                                                lblAssignedOn.Text = "---";
                                            }
                                            else
                                            {
                                                DataSet dsRRW = oResourceRequest.GetWorkflow(intRRW);
                                                if (dsRRW.Tables[0].Rows.Count > 0)
                                                {
                                                    int intAssignedTo = Int32.Parse(dsRRW.Tables[0].Rows[0]["userid"].ToString());
                                                    lblAssignedTo.Text = oUser.GetFullName(intAssignedTo);
                                                    lblAssignedOn.Text = dsRR.Tables[0].Rows[0]["assigned"].ToString();
                                                    lblModified.Text = dsRRW.Tables[0].Rows[0]["modified"].ToString();
                                                }
                                            }
                                            lblRequestID.Text = "<a href=\"javascript:void(0);\" class=\"lookup\" onclick=\"OpenNewWindowMenu('/datapoint/service/resource.aspx?id=" + oFunction.encryptQueryString(intRR.ToString()) + "', '800', '600');\">" + intRequestRR.ToString() + "-" + intServiceRR.ToString() + "-" + intNumberRR.ToString() + "</a>";
                                        }
                                        if (lblStatus.Text == "-2")
                                        {
                                            lblStatus.Text = "Cancelled";
                                            lblImage.Text = "<img src=\"/images/cancel.gif\" border=\"0\" align=\"absmiddle\"/>";
                                        }
                                        else if (lblStatus.Text != "3")
                                        {
                                            boolCompleted = false;
                                            lblStatus.Text = "Active";
                                            lblImage.Text = "<img src=\"/images/active.gif\" border=\"0\" align=\"absmiddle\"/>";
                                        }
                                        else
                                        {
                                            lblStatus.Text = "Completed";
                                            lblImage.Text = "<img src=\"/images/check.gif\" border=\"0\" align=\"absmiddle\"/>";
                                        }
                                    }
                                }
                            }

                            if (boolCompleted == false)
                            {
                                chk4.Enabled = false;
                                img4.ImageUrl = "/images/bigAlert.gif";
                                btnSave.ImageUrl = "/images/tool_save_dbl.gif";
                                btnSave.Enabled = false;
                                btnSave.ToolTip = "Open Tasks";
                            }
                        }
                        else
                            panPreBuildFormsNo.Visible = true;
                        btnSave.Attributes.Add("onclick", "return ProcessControlButton();");
                    }
                    if (chk5.Enabled == true)
                    {
                        if (boolTDPO == true)
                            btnSave.Attributes.Add("onclick", "return confirm('Please confirm that the server is built and the assigned IP address is pingable') && ProcessControlButton();");
                        else 
                        {
                            btnSave.ImageUrl = "/images/tool_save_dbl.gif";
                            btnSave.Enabled = false;
                            btnSave.ToolTip = "Backup Pending...";
                        }
                    }
                    if (chk6.Enabled == true)
                    {
                        // Locate Birth Certificate
                        string strBirthCertificate = oVariable.UploadsFolder() + "birth\\" + "BIRTH_" + intAnswer.ToString() + "_" + oProject.Get(intProject, "number") + ".PDF";
                        System.IO.FileInfo oFile = new System.IO.FileInfo(strBirthCertificate);
                        if (oFile.Exists == true)
                            lblForms.Text += "<img src=\"/images/icons/pdf.gif\" border=\"0\" align=\"absmiddle\"/> <a href=\"javascript:void(0);\" onclick=\"OpenNewWindowNoLoad('" + strBirthCertificate + "',800,600);\">Click here to view the Original Birth Certificate</a>";
                        else
                        {
                            //PDFs oPDF = new PDFs(dsn, dsnAsset, dsnIP, intEnvironment);
                            //// Initiate Service Center Request
                            //foreach (DataRow drManual in dsManual.Tables[0].Rows)
                            //{
                            //    int intServer = Int32.Parse(drManual["id"].ToString());
                            //    oPDF.CreateSCRequest(intServer, boolUsePNCNaming);
                            //}
                            // Generate Birth Certificate
                            //oPDF.CreateDocuments(intAnswer, false, false, null, true, true, true, false, boolUsePNCNaming, true);
                            //oFile = new System.IO.FileInfo(strBirthCertificate);
                            //if (oFile.Exists == true)
                            //    lblForms.Text += "<img src=\"/images/icons/pdf.gif\" border=\"0\" align=\"absmiddle\"/> <a href=\"javascript:void(0);\" onclick=\"OpenNewWindowNoLoad('" + strBirthCertificate + "',800,600);\">Click here to view the Original Birth Certificate</a>";
                            //else
                                lblForms.Text = "<img src=\"/images/icons/pdf.gif\" border=\"0\" align=\"absmiddle\"/> Birth Certificate Not Found";
                        }

                        // Load Tasks
                        DataSet dsPNC = oPNCTask.GetStepsDesign(intAnswer, 0, 0);
                        if (dsPNC.Tables[0].Rows.Count == 0)
                            panPostBuildFormsNo.Visible = true;
                        else
                        {
                            int intCurrentStep = 0;
                            DataSet dsPNCCurrent = oPNCTask.GetStepsDesign(intAnswer, 1, 0);
                            if (dsPNCCurrent.Tables[0].Rows.Count > 0)
                                Int32.TryParse(dsPNCCurrent.Tables[0].Rows[0]["step"].ToString(), out intCurrentStep);

                            panPostBuildFormsYes.Visible = true;
                            bool boolPostBuildForms = true;
                            rptPostBuildForms.DataSource = dsPNC;
                            rptPostBuildForms.DataBind();
                            foreach (RepeaterItem ri in rptPostBuildForms.Items)
                            {
                                Label lblName = (Label)ri.FindControl("lblName");
                                Label lblImage = (Label)ri.FindControl("lblImage");
                                Label lblStatus = (Label)ri.FindControl("lblStatus");
                                Label lblCreated = (Label)ri.FindControl("lblCreated");

                                if (lblStatus.ToolTip == "1")
                                    ri.Visible = false;
                                else
                                {
                                    int intThisStep = 0;
                                    Int32.TryParse(lblImage.ToolTip, out intThisStep);

                                    if (lblStatus.Text == "")
                                    {
                                        if (lblName.ToolTip == "0")
                                        {
                                            if (lblCreated.Text != "")
                                            {
                                                lblImage.Text = "<img src=\"/images/active.gif\" border=\"0\" align=\"absmiddle\" />";
                                                lblStatus.Text = "Processing...";
                                            }
                                            else if (intCurrentStep > intThisStep)
                                            {
                                                lblImage.Text = "<img src=\"/images/check.gif\" border=\"0\" align=\"absmiddle\" />";
                                                lblStatus.Text = "Skipped";
                                            }
                                            else
                                            {
                                                lblImage.Text = "";
                                                lblStatus.Text = "Waiting";
                                                lblStatus.ToolTip = intCurrentStep.ToString() + "," + intThisStep.ToString();
                                            }
                                        }
                                        else
                                        {
                                            int intResourceID = 0;
                                            if (Int32.TryParse(lblName.ToolTip, out intResourceID) == true)
                                            {
                                                if (oResourceRequest.Get(intResourceID, "status") == "-2")
                                                {
                                                    lblImage.Text = "<img src=\"/images/cancel.gif\" border=\"0\" align=\"absmiddle\"/>";
                                                    lblStatus.Text = "Cancelled";
                                                }
                                                else
                                                {
                                                    intCurrentStep = intThisStep;
                                                    boolPostBuildForms = false;
                                                    Label lblAssignedTo = (Label)ri.FindControl("lblAssignedTo");
                                                    if (lblAssignedTo.Text == "")
                                                    {
                                                        lblImage.Text = "<img src=\"/images/pending.gif\" border=\"0\" align=\"absmiddle\" />";
                                                        lblStatus.Text = "Pending Assignment";
                                                    }
                                                    else
                                                    {
                                                        if (lblImage.Text == "1")
                                                        {
                                                            lblImage.Text = "<img src=\"/images/error.gif\" border=\"0\" align=\"absmiddle\" />";
                                                            lblStatus.Text = "Error";
                                                        }
                                                        else if (lblImage.Text == "2")
                                                        {
                                                            lblImage.Text = "<img src=\"/images/alert.gif\" border=\"0\" align=\"absmiddle\" />";
                                                            lblStatus.Text = "Alert";
                                                        }
                                                        else
                                                        {
                                                            lblImage.Text = "<img src=\"/images/active.gif\" border=\"0\" align=\"absmiddle\" />";
                                                            lblStatus.Text = "Active";
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        lblImage.Text = "<img src=\"/images/check.gif\" border=\"0\" align=\"absmiddle\" />";
                                        lblStatus.Text = "Completed on " + lblStatus.Text;
                                    }
                                }
                            }

                            if (boolPostBuildForms == false)
                            {
                                chk6.Enabled = false;
                                img6.ImageUrl = "/images/bigAlert.gif";
                                btnSave.ImageUrl = "/images/tool_save_dbl.gif";
                                btnSave.Enabled = false;
                                btnSave.ToolTip = "Open Tasks";
                            }
                        }
                        btnSave.Attributes.Add("onclick", "return ProcessControlButton();");
                    }
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

        protected void btnChange_Click(Object Sender, EventArgs e)
        {
            int intResourceWorkflow = Int32.Parse(lblResourceWorkflow.Text);
            int intResourceParent = oResourceRequest.GetWorkflowParent(intResourceWorkflow);
            oResourceRequest.AddChangeControl(intResourceWorkflow, txtNumber.Text, DateTime.Parse(txtDate.Text + " " + txtTime.Text), txtChange.Text);
            Response.Redirect(Request.Path + "?rrid=" + intResourceWorkflow.ToString() + "&div=C&save=true");
        }

        protected void btnDeleteChange_Click(Object Sender, EventArgs e)
        {
            LinkButton oButton = (LinkButton)Sender;
            oResourceRequest.DeleteChangeControl(Int32.Parse(oButton.CommandArgument));
            Response.Redirect(Request.Path + "?rrid=" + lblResourceWorkflow.Text + "&div=C");
        }

        protected void btnReturn_Click(Object Sender, EventArgs e)
        {
        }

        protected void chkDescription_Change(Object Sender, EventArgs e)
        {
            if (chkDescription.Checked == true)
                Response.Redirect(Request.Path + "?rrid=" + Request.QueryString["rrid"] + "&doc=true&div=D");
            else
                Response.Redirect(Request.Path + "?rrid=" + Request.QueryString["rrid"] + "&div=D");
        }
        protected void btnSave_Click(Object Sender, EventArgs e)
        {
            Save(false);
        }
        protected void btnWarning_Click(Object Sender, EventArgs e)
        {
            Save(true);
        }
        private void Save(bool _bypass_warning)
        {
            Servers oServer = new Servers(0, dsn);
            int intResourceWorkflow = Int32.Parse(lblResourceWorkflow.Text);
            int intResourceParent = oResourceRequest.GetWorkflowParent(intResourceWorkflow);
            int intModel = Int32.Parse(lblModelID.Text);
            string strModel = oModelsProperties.Get(intModel, "name");
            bool boolBlade = oModelsProperties.IsTypeBlade(intModel);
            bool boolDell = oModelsProperties.IsDell(intModel);
            int intAnswer = Int32.Parse(lblAnswer.Text);
            int intClass = Int32.Parse(oForecast.GetAnswer(intAnswer, "classid"));
            int intEnv = Int32.Parse(oForecast.GetAnswer(intAnswer, "environmentid"));
            bool boolPNC = (oClass.Get(intClass, "pnc") == "1");
            int intAddress = Int32.Parse(oForecast.GetAnswer(intAnswer, "addressid"));
            int intMnemonic = 0;
            if (oForecast.GetAnswer(intAnswer, "mnemonicid") != "")
                intMnemonic = Int32.Parse(oForecast.GetAnswer(intAnswer, "mnemonicid"));
            bool boolBIR = (oForecast.GetAnswer(intAnswer, "resiliency") == "1");    // Related to the Business Resiliency Effort (use old data center strategy)
            int intResiliency = oResiliency.GetIDFromMnemonic(intMnemonic, boolBIR, intAnswer);
            DataSet dsAnswer = oServer.GetAnswer(intAnswer);
            string strError = "";
            string strWarning = "";

            bool boolSVEClustered = false;
            int intSubApplication = 0;
            string strSVEType = "";

            if (oForecast.GetAnswer(intAnswer, "subapplicationid") != "")
                intSubApplication = Int32.Parse(oForecast.GetAnswer(intAnswer, "subapplicationid"));

            /*
            if (oClass.IsProd(intClass))
                boolSVEClustered = true;
            if (oClass.IsQA(intClass))
                boolSVEClustered = true;
            if (oClass.IsTest(intClass))
                boolSVEClustered = false;
            */

            if (intSubApplication > 0)
            {
                boolSVEClustered = (oServerName.GetSubApplication(intSubApplication, "code") == "Z");
                strSVEType = oServerName.GetSubApplication(intSubApplication, "factory_code");
            }

            if (strSVEType == "DB")
            {
                // Database SVE
            }
            else if (strSVEType == "Z")
            {
                // Non-Database SVE
            }

            if (chkAdmin.Checked == true)
            {
                oForecast.UpdateAnswerCompleted(intAnswer);
                foreach (DataRow drAnswer in dsAnswer.Tables[0].Rows)
                {
                    int intServer = Int32.Parse(drAnswer["id"].ToString());
                    oServer.UpdateDNS(intServer, 1, "Skipped (Admin)");
                    // Set the build dates
                    if (oServer.Get(intServer, "build_started") == "")
                        oServer.UpdateBuildStarted(intServer, DateTime.Now.ToString(), true);
                    if (oServer.Get(intServer, "build_completed") == "")
                        oServer.UpdateBuildCompleted(intServer, DateTime.Now.ToString());
                    if (oServer.Get(intServer, "build_ready") == "")
                        oServer.UpdateBuildReady(intServer, DateTime.Now.ToString(), true);
                }
            }
            else
            {
                int intServerCount = dsAnswer.Tables[0].Rows.Count;
                if (oModelsProperties.IsSUNVirtual(intModel) == true)
                {
                    if (boolSVEClustered == true)
                    {
                        // Make sure the quantity is >= 2 and <= 6
                        if (intServerCount < 2 || intServerCount > 6)
                            strError = "The quantity for an SVE Production or QA design must be between 2 and 6. (Current quantity = " + intServerCount.ToString() + ")";
                    }
                }

                if (strError == "")
                {
                    if (chk1.Enabled == true)
                    {
                        // Save SVE Setting (if applicable)
                        if (oModelsProperties.IsSUNVirtual(intModel) == true)
                        {
                            int intSVEClusterID = 0;
                            foreach (DataRow drAnswer in dsAnswer.Tables[0].Rows)
                            {
                                int intServer = Int32.Parse(drAnswer["id"].ToString());
                                if (intSVEClusterID == 0)
                                {
                                    DataSet dsSVEClusters = oSolaris.GetSVEClustersAssign((strSVEType == "DB" ? 1 : 0), intClass, intAddress, intResiliency, intServer);
                                    oLog.AddEvent(intAnswer, "", "", "There are " + dsSVEClusters.Tables[0].Rows.Count.ToString() + " sun virtual environments available....pr_getSVEClustersAssign(" + strSVEType + "," + intClass.ToString() + "," + intAddress.ToString() + "," + intResiliency.ToString() + "," + intServer.ToString() + ")", LoggingType.Debug);
                                    if (dsSVEClusters.Tables[0].Rows.Count > 0)
                                        intSVEClusterID = Int32.Parse(dsSVEClusters.Tables[0].Rows[0]["id"].ToString());
                                }
                                if (intSVEClusterID > 0)
                                {
                                    int intClusterID = Int32.Parse(drAnswer["clusterid"].ToString());
                                    double dblStorage = 0.00;
                                    DataSet dsStorage = oStorage.GetLunsClusterShared(intAnswer, intClusterID);
                                    foreach (DataRow drStorage in dsStorage.Tables[0].Rows)
                                    {
                                        double dblTemp = 0.00;
                                        double.TryParse(drStorage["size"].ToString(), out dblTemp);
                                        dblStorage += dblTemp;
                                        double.TryParse(drStorage["size_qa"].ToString(), out dblTemp);
                                        dblStorage += dblTemp;
                                        double.TryParse(drStorage["size_test"].ToString(), out dblTemp);
                                        dblStorage += dblTemp;
                                    }
                                    oSolaris.AddSVEGuest(intSVEClusterID, intServer, 0, dblStorage, oForecast.GetCPU(intAnswer), oForecast.GetRAM(intAnswer));
                                }
                                else
                                {
                                    strError = "A SUN Virtual Environment " + (strSVEType == "DB" ? "Database" : "Application") + " Cluster could not be found in " + oClass.Get(intClass, "name").ToUpper();
                                    break;
                                }
                            }
                        }

                        if (strError == "")
                        {
                            bool boolDR = (oClass.IsProd(intClass) && oModelsProperties.IsEnforce1to1Recovery(intModel));
                            // Step # 1 : Reserve Asset(s)
                            if (radAutoVirtual.Checked == true)
                            {
                                // Automatically assign the virtual asset(s)
                                foreach (DataRow drAnswer in dsAnswer.Tables[0].Rows)
                                {
                                    int intServer = Int32.Parse(drAnswer["id"].ToString());
                                    int intAssetLatest = 0;
                                    int intAssetDR = 0;
                                    DataSet dsAssets = oServer.GetAssets(intServer);
                                    foreach (DataRow drAsset in dsAssets.Tables[0].Rows)
                                    {
                                        if (drAsset["latest"].ToString() == "1")
                                            intAssetLatest = Int32.Parse(drAsset["assetid"].ToString());
                                        else if (drAsset["dr"].ToString() == "1")
                                            intAssetDR = Int32.Parse(drAsset["assetid"].ToString());
                                    }

                                    if (intAssetLatest == 0)
                                    {
                                        string strName = "Design # " + intAnswer.ToString() + " (" + intServer.ToString() + ")";
                                        string strDR = "";
                                        if (boolDR == false && drAnswer["dr"].ToString() == "1")
                                            boolDR = true;
                                        if (boolDR == true)
                                        {
                                            if (drAnswer["dr_exist"].ToString() == "1")
                                                strDR = drAnswer["dr_name"].ToString();
                                            else
                                                strDR = strName + "-DR";
                                        }
                                        else
                                            strDR = "N / A";

                                        // Virtual Asset Generation
                                        string strSerial = oAsset.GetVSG("SERVER");
                                        string strAsset = strSerial;
                                        oAsset.UpdateVSG(strSerial, strName, "SERVER");
                                        intAssetLatest = oAsset.AddGuest(strName, intModel, strSerial, strAsset, (int)AssetStatus.InUse, intProfile, DateTime.Now, 0, 0.00, 0.00, 0.00, intClass, intEnv, intAddress, 0, 0);
                                        oServer.AddAsset(intServer, intAssetLatest, intClass, intEnv, (panMove.Visible ? 1 : 0), 0);
                                        if (boolDR == true)
                                        {
                                            if (intAssetDR == 0)
                                            {
                                                // Virtual Asset Generation (DR)
                                                string strSerialDR = oAsset.GetVSG("SERVER");
                                                string strAssetDR = strSerial;
                                                oAsset.UpdateVSG(strSerialDR, strName, "SERVER");
                                                intAssetDR = oAsset.AddGuest(strDR, intModel, strSerialDR, strAssetDR, (int)AssetStatus.InUse, intProfile, DateTime.Now, 0, 0.00, 0.00, 0.00, intClass, intEnv, intAddress, 0, 0);
                                                oServer.AddAsset(intServer, intAssetDR, intClass, intEnv, (panMove.Visible ? 1 : 0), 1);
                                            }
                                        }
                                    }
                                }
                            }
                            if (radAutoYes.Checked == true)
                            {
                                // Automatic Query for Asset
                                int intAssetClass = Int32.Parse(ddlClass.SelectedItem.Value);
                                int intAssetEnvironment = Int32.Parse(Request.Form[hdnEnvironment.UniqueID]);
                                int intAssetAddress = Int32.Parse(Request.Form[hdnLocation.UniqueID]);
                                foreach (DataRow drAnswer in dsAnswer.Tables[0].Rows)
                                {
                                    int intServer = Int32.Parse(drAnswer["id"].ToString());
                                    int intOS = 0;
                                    if (drAnswer["osid"].ToString() != "")
                                        intOS = Int32.Parse(drAnswer["osid"].ToString());
                                    int intAssetLatest = 0;
                                    int intAssetDR = 0;
                                    DataSet dsAssets = oServer.GetAssets(intServer);
                                    foreach (DataRow drAsset in dsAssets.Tables[0].Rows)
                                    {
                                        if (drAsset["latest"].ToString() == "1")
                                            intAssetLatest = Int32.Parse(drAsset["assetid"].ToString());
                                        else if (drAsset["dr"].ToString() == "1")
                                            intAssetDR = Int32.Parse(drAsset["assetid"].ToString());
                                    }

                                    if (boolDR == false && drAnswer["dr"].ToString() == "1")
                                        boolDR = true;
                                    if (intAssetLatest == 0)
                                    {
                                        List<int> lstAsset = oAsset.GetServerOrBladeAvailable(intAssetClass, intAssetEnvironment, intAssetAddress, intModel, intAnswer, dsn, "", false, intProject, intResiliency, intOS, "", boolDR, oModelsProperties.IsDell(intModel), dsnServiceEditor);
                                        intAssetLatest = lstAsset[0];
                                        if (intAssetLatest > 0)
                                        {
                                            if (boolDR == true)
                                            {
                                                intAssetDR = lstAsset[1];
                                                if (intAssetDR > 0)
                                                {
                                                    oServer.AddAsset(intServer, intAssetLatest, intAssetClass, intAssetEnvironment, (panMove.Visible ? 1 : 0), 0);
                                                    oServer.AddAsset(intServer, intAssetDR, intAssetClass, intAssetEnvironment, (panMove.Visible ? 1 : 0), 1);
                                                }
                                                else
                                                    strError = "A Disaster Recovery Asset was not assigned (ServerID = " + intServer.ToString() + ", DesignID = " + intAnswer.ToString() + ", EnvironmentID = " + intAssetEnvironment.ToString() + ", intModel = " + intModel.ToString() + ", BladeAssetID = " + (oModelsProperties.IsTypeBlade(intModel) ? intAssetLatest.ToString() : "0") + ")";
                                            }
                                            else
                                                oServer.AddAsset(intServer, intAssetLatest, intAssetClass, intAssetEnvironment, (panMove.Visible ? 1 : 0), 0);
                                        }
                                        else
                                            strError = "The inventory has been depleted (ServerID = " + intServer.ToString() + ", DesignID = " + intAnswer.ToString() + ", ClassID = " + intAssetClass.ToString() + ", EnvironmentID = " + intAssetEnvironment.ToString() + ", AddressID = " + intAssetAddress.ToString() + ", ModelID = " + intModel.ToString() + ")";
                                    }

                                    if (strError == "")
                                    {
                                        if (intAssetLatest > 0)
                                            oAsset.AddStatus(intAssetLatest, "Design # " + intAnswer.ToString(), (int)AssetStatus.Reserved, intProfile, DateTime.Now);
                                        if (intAssetDR > 0)
                                            oAsset.AddStatus(intAssetDR, "Design # " + intAnswer.ToString() + " (DR)", (int)AssetStatus.Reserved, intProfile, DateTime.Now);
                                    }
                                }
                            }
                            if (radAutoNo.Checked == true)
                            {
                                // Use the serial numbers entered to query for the asset(s)
                                foreach (DataRow drAnswer in dsAnswer.Tables[0].Rows)
                                {
                                    int intServer = Int32.Parse(drAnswer["id"].ToString());
                                    int intAssetLatest = 0;
                                    int intAssetDR = 0;
                                    DataSet dsAssets = oServer.GetAssets(intServer);
                                    foreach (DataRow drAsset in dsAssets.Tables[0].Rows)
                                    {
                                        if (drAsset["latest"].ToString() == "1")
                                            intAssetLatest = Int32.Parse(drAsset["assetid"].ToString());
                                        else if (drAsset["dr"].ToString() == "1")
                                            intAssetDR = Int32.Parse(drAsset["assetid"].ToString());
                                    }

                                    if (boolDR == false && drAnswer["dr"].ToString() == "1")
                                        boolDR = true;
                                    if (intAssetLatest == 0)
                                    {
                                        string strSerial = Request.Form["HDN_" + intServer.ToString() + "_serial"];
                                        DataSet dsAsset = oAsset.Get(strSerial, intModel);
                                        if (dsAsset.Tables[0].Rows.Count > 0)
                                        {
                                            if (dsAsset.Tables[0].Rows[0]["asset_attribute"].ToString() == "0")
                                            {
                                                if (dsAsset.Tables[0].Rows[0]["status"].ToString() == "2")
                                                {
                                                    intAssetLatest = Int32.Parse(dsAsset.Tables[0].Rows[0]["id"].ToString());
                                                    string strSerialDR = Request.Form["HDN_" + intServer.ToString() + "_serial_dr"];
                                                    if (boolDR == true && strSerialDR != "--- None ---")
                                                    {
                                                        DataSet dsAssetDR = oAsset.Get(strSerialDR, intModel);
                                                        if (dsAssetDR.Tables[0].Rows.Count > 0)
                                                        {
                                                            if (dsAssetDR.Tables[0].Rows[0]["asset_attribute"].ToString() == "0")
                                                            {
                                                                if (dsAssetDR.Tables[0].Rows[0]["status"].ToString() == "2")
                                                                {
                                                                    intAssetDR = Int32.Parse(dsAssetDR.Tables[0].Rows[0]["id"].ToString());
                                                                    oServer.AddAsset(intServer, intAssetLatest, intClass, intEnv, (panMove.Visible ? 1 : 0), 0);
                                                                    oServer.AddAsset(intServer, intAssetDR, intClass, intEnv, (panMove.Visible ? 1 : 0), 1);
                                                                }
                                                                else
                                                                    strError = strSerialDR.ToUpper() + " is not set to AVAILABLE and therefore, cannot be assigned at this time";
                                                            }
                                                            else
                                                                strError = strSerialDR.ToUpper() + " is not set to OK and therefore, cannot be assigned at this time";
                                                        }
                                                        else
                                                            strError = strSerialDR.ToUpper() + " was either not found, or the model is not equal to " + lblModel.Text;
                                                    }
                                                    else
                                                        oServer.AddAsset(intServer, intAssetLatest, intClass, intEnv, (panMove.Visible ? 1 : 0), 0);
                                                }
                                                else
                                                    strError = strSerial.ToUpper() + " is not set to AVAILABLE and therefore, cannot be assigned at this time";
                                            }
                                            else
                                                strError = strSerial.ToUpper() + " is not set to OK and therefore, cannot be assigned at this time";
                                        }
                                        else
                                            strError = strSerial.ToUpper() + " was either not found, or the model is not equal to " + lblModel.Text;
                                    }

                                    if (strError == "")
                                    {
                                        if (intAssetLatest > 0)
                                            oAsset.AddStatus(intAssetLatest, "Design # " + intAnswer.ToString(), (int)AssetStatus.Reserved, intProfile, DateTime.Now);
                                        if (intAssetDR > 0)
                                            oAsset.AddStatus(intAssetDR, "Design # " + intAnswer.ToString() + " (DR)", (int)AssetStatus.Reserved, intProfile, DateTime.Now);
                                    }
                                }
                            }
                        }
                    }

                    if (chk3.Enabled == true)
                    {
                        // Step # 2 : Assign Name(s) / IP Address(es)
                        int intHost = Int32.Parse(oForecast.GetAnswer(intAnswer, "hostid"));
                        int intApplication = 0;
                        if (oForecast.GetAnswer(intAnswer, "applicationid") != "")
                            intApplication = Int32.Parse(oForecast.GetAnswer(intAnswer, "applicationid"));
                        Host oHost = new Host(intProfile, dsn);
                        bool boolDR = (oClass.IsProd(intClass) && oModelsProperties.IsEnforce1to1Recovery(intModel));
                        int intNameSVE1 = 0;
                        int intNameSVE2 = 0;
                        int intNameSVE3 = 0;
                        int intNameSVE4 = 0;
                        int intNameSVE5 = 0;
                        int intNameSVE6 = 0;
                        bool boolSVENaming = false;
                        string strAdditionalName = "";
                        foreach (DataRow drAnswer in dsAnswer.Tables[0].Rows)
                        {
                            int intServer = Int32.Parse(drAnswer["id"].ToString());
                            int intOS = 0;
                            if (drAnswer["osid"].ToString() != "")
                                intOS = Int32.Parse(drAnswer["osid"].ToString());
                            int intNameAssigned = Int32.Parse(drAnswer["nameid"].ToString());
                            int intName = 0;
                            if (Request.Form["HDN_" + intServer.ToString() + "_name"] != "")
                                intName = Int32.Parse(Request.Form["HDN_" + intServer.ToString() + "_name"]);

                            // There are numerous spots where we need the assets assigned, so let's get them now...
                            int intAssetLatest = 0;
                            int intAssetDR = 0;
                            DataSet dsAssets = oServer.GetAssets(intServer);
                            foreach (DataRow drAsset in dsAssets.Tables[0].Rows)
                            {
                                if (drAsset["latest"].ToString() == "1")
                                    intAssetLatest = Int32.Parse(drAsset["assetid"].ToString());
                                else if (drAsset["dr"].ToString() == "1")
                                    intAssetDR = Int32.Parse(drAsset["assetid"].ToString());
                            }

                            // First, let's do the servername
                            if (intName == -999)
                            {
                                // Manual
                                string strManualName = Request.Form["HDN_" + intServer.ToString() + "_manual_name"].Trim().ToUpper();
                                strAdditionalName = strManualName;
                                if (strManualName != "" && oServer.GetName(intServer, true).Trim().ToUpper() != strManualName)
                                {
                                    if (oModelsProperties.IsSUNVirtual(intModel) == true)
                                    {
                                        if (strSVEType == "DB")
                                        {
                                            // Database naming = Mnemonic + Environment (DBQ) + SEQUENCE (00) + (A,B,C,etc...)
                                            if (strManualName.Length == 8 || strManualName.Length == 9)
                                            {
                                                string strMnemonic = strManualName.Substring(0, 3);
                                                string strEnvironment = strManualName.Substring(3, 3);
                                                string strName1 = strManualName.Substring(6, 1);
                                                string strName2 = strManualName.Substring(7, 1);
                                                string strFunc = "";
                                                if (strManualName.Length == 9)
                                                    strFunc = strManualName.Substring(8, 1);
                                                intName = oServerName.AddFactory("", "", strMnemonic, strEnvironment, strName1, strName2, strFunc, "", intProfile, "GENERIC" + intServer.ToString(), 0);
                                                oServer.UpdateServerNamed(intServer, intName);
                                                // Update Asset
                                                if (intAssetLatest > 0)
                                                    oAsset.AddStatus(intAssetLatest, strManualName, (int)AssetStatus.InUse, intProfile, DateTime.Now);
                                                if (intAssetDR > 0)
                                                    oAsset.AddStatus(intAssetDR, strManualName + "-DR", (int)AssetStatus.InUse, intProfile, DateTime.Now);
                                            }
                                            else
                                                strError = "The name " + strManualName + " is not 8 or 9 characters long and therefore, is invalid.";
                                        }
                                        else if (strSVEType == "Z")
                                        {
                                            // Non-DB naming = SCXXX104Z = SCXXX104ZA + SCXXX104ZB
                                            if (strManualName.Length == 9 || strManualName.Length == 10)
                                            {
                                                string strOS = strManualName.Substring(0, 1);
                                                string strLocation = strManualName.Substring(1, 1);
                                                string strMnemonic = strManualName.Substring(2, 3);
                                                string strEnvironment = strManualName.Substring(5, 1);
                                                string strName1 = strManualName.Substring(6, 1);
                                                string strName2 = strManualName.Substring(7, 1);
                                                string strFunc = strManualName.Substring(8, 1);
                                                string strSpecific = "";
                                                if (strManualName.Length == 10)
                                                    strSpecific = strManualName.Substring(9, 1);
                                                intName = oServerName.AddFactory(strOS, strLocation, strMnemonic, strEnvironment, strName1, strName2, strFunc, strSpecific, intProfile, "GENERIC" + intServer.ToString(), 0);
                                                oServer.UpdateServerNamed(intServer, intName);
                                                // Update Asset
                                                if (intAssetLatest > 0)
                                                    oAsset.AddStatus(intAssetLatest, strManualName, (int)AssetStatus.InUse, intProfile, DateTime.Now);
                                                if (intAssetDR > 0)
                                                    oAsset.AddStatus(intAssetDR, strManualName + "-DR", (int)AssetStatus.InUse, intProfile, DateTime.Now);
                                            }
                                            else
                                                strError = "The name " + strManualName + " is not 9 or 10 characters long and therefore, is invalid.";
                                        }
                                        else
                                        {
                                            strError = "The SUN SVE type (" + strSVEType + ") is not configured for naming";
                                        }
                                    }
                                    else
                                    {
                                        if (boolPNC == true)
                                        {
                                            // Example = WCXXX104AZ
                                            if (strManualName.Length == 9 || strManualName.Length == 10)
                                            {
                                                string strOS = strManualName.Substring(0, 1);
                                                string strLocation = strManualName.Substring(1, 1);
                                                string strMnemonic = strManualName.Substring(2, 3);
                                                string strEnvironment = strManualName.Substring(5, 1);
                                                string strName1 = strManualName.Substring(6, 1);
                                                string strName2 = strManualName.Substring(7, 1);
                                                string strFunc = strManualName.Substring(8, 1);
                                                string strSpecific = "";
                                                if (strManualName.Length == 10)
                                                    strSpecific = strManualName.Substring(9, 1);
                                                intName = oServerName.AddFactory(strOS, strLocation, strMnemonic, strEnvironment, strName1, strName2, strFunc, strSpecific, intProfile, "GENERIC" + intServer.ToString(), 0);
                                                oServer.UpdateServerNamed(intServer, intName);
                                                // Update Asset
                                                if (intAssetLatest > 0)
                                                    oAsset.AddStatus(intAssetLatest, strManualName, (int)AssetStatus.InUse, intProfile, DateTime.Now);
                                                if (intAssetDR > 0)
                                                    oAsset.AddStatus(intAssetDR, strManualName + "-DR", (int)AssetStatus.InUse, intProfile, DateTime.Now);
                                            }
                                            else
                                                strError = "The name " + strManualName + " is not 9 or 10 characters long and therefore, is invalid.";
                                        }
                                        else
                                        {
                                            if (strManualName.Length == 12)
                                            {
                                                string strPrefix1 = strManualName.Substring(0, 5);
                                                string strPrefix2 = strManualName.Substring(5, 3);
                                                string strCode = strManualName.Substring(8, 2);
                                                string strName1 = strManualName.Substring(10, 1);
                                                string strName2 = strManualName.Substring(11, 1);
                                                intName = oServerName.Add(0, strPrefix1, strPrefix2, strCode, strName1, strName2, intProfile, "GENERIC" + intServer.ToString(), 0);
                                                oServer.UpdateServerNamed(intServer, intName);
                                                // Update Asset
                                                if (intAssetLatest > 0)
                                                    oAsset.AddStatus(intAssetLatest, strManualName, (int)AssetStatus.InUse, intProfile, DateTime.Now);
                                                if (intAssetDR > 0)
                                                    oAsset.AddStatus(intAssetDR, strManualName + "-DR", (int)AssetStatus.InUse, intProfile, DateTime.Now);
                                            }
                                            else
                                                strError = "The name " + strManualName + " is not 12 characters long and therefore, is invalid.";
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (intNameAssigned == 0 && boolSVENaming == false)
                                {
                                    if (radNamesYes.Checked == true)
                                        intName = 0;
                                    if (intName == 0)
                                    {
                                        // Name was not selected, so Auto-generate it
                                        int intClusterID = Int32.Parse(drAnswer["clusterid"].ToString());
                                        int intInfrastructure = (drAnswer["infrastructure"].ToString() != "1" ? 0 : 1);
                                        DataSet dsComponents = oServerName.GetComponentDetailSelected(intServer, 1);
                                        if (boolPNC == true)
                                        {
                                            string _os = oOperatingSystem.Get(intOS, "factory_code");
                                            string _location = oLocation.GetAddress(intAddress, "factory_code");
                                            string _mnemonic = oMnemonic.Get(intMnemonic, "factory_code");
                                            string _environment = oClass.Get(intClass, "factory_code");
                                            // Get Server Function
                                            string _function = "";
                                            if (intSubApplication > 0)
                                                _function = oServerName.GetSubApplication(intSubApplication, "factory_code");
                                            if (_function == "" && intApplication > 0)
                                                _function = oServerName.GetApplication(intApplication, "factory_code");
                                            if (_function == "")
                                            {
                                                foreach (DataRow drComponent in dsComponents.Tables[0].Rows)
                                                {
                                                    _function = drComponent["factory_code"].ToString();
                                                    break;
                                                }
                                            }
                                            if (_function == "")
                                                _function = "A";
                                            // Get Specifics
                                            string _specific = "";
                                            if (intSubApplication > 0)
                                                _specific = oServerName.GetSubApplication(intSubApplication, "factory_code_specific");
                                            if (_specific == "" && intApplication > 0)
                                                _specific = oServerName.GetApplication(intApplication, "factory_code_specific");
                                            if (_specific == "")
                                            {
                                                foreach (DataRow drComponent in dsComponents.Tables[0].Rows)
                                                {
                                                    _specific = drComponent["factory_code_specific"].ToString();
                                                    break;
                                                }
                                            }
                                            if (_specific == "" && oForecast.IsHACluster(intAnswer) == true)
                                                _specific = "Z";
                                            if (intNameSVE1 == 0 && intNameSVE2 == 0 && intNameSVE3 == 0 && intNameSVE4 == 0 && intNameSVE5 == 0 && intNameSVE6 == 0)
                                            {
                                                if (oModelsProperties.IsSUNVirtual(intModel) == true)
                                                {
                                                    // The "factory_code" of the subapplication either contains "DB" for SVE DB, or "Z" for SVE APP
                                                    if (strSVEType == "DB")
                                                    {
                                                        // SVE naming
                                                        _os = "";
                                                        _location = "";
                                                        if (oClass.IsProd(intClass))
                                                        {
                                                            _environment = "DBP";
                                                        }
                                                        if (oClass.IsQA(intClass))
                                                        {
                                                            _environment = "DBQ";
                                                        }
                                                        if (oClass.IsTestDev(intClass))
                                                        {
                                                            _environment = "DBT";
                                                        }
                                                        _function = "";
                                                        _specific = "";
                                                    }
                                                    else if (strSVEType == "Z")
                                                    {
                                                        // Only need to increase the specific since the _function will be "Z" which is correct.
                                                        if (_specific == "Z")
                                                            _specific = "";
                                                        _function = strSVEType;
                                                    }
                                                    else
                                                    {
                                                        strError = "The SUN SVE type (" + strSVEType + ") is not configured for naming";
                                                    }
                                                }
                                                if (strError == "")
                                                {
                                                    intName = oServerName.AddFactory(_os, _location, _mnemonic, _environment, intClass, intEnv, _function, _specific, intProfile, "GENERIC" + intServer.ToString(), dsnServiceEditor);
                                                    if (oModelsProperties.IsSUNVirtual(intModel) == true && boolSVEClustered == true && intName > 0)
                                                    {
                                                        // Assign the previous one as an additional one
                                                        oServerName.AddRelated(intAnswer, intClusterID, intName);
                                                        // Create the next one (will only be applied if count > 1)
                                                        string strNameSVE = oServerName.GetNameFactory(intName, 0);
                                                        strAdditionalName = strNameSVE;
                                                        // Get the last three characters
                                                        strNameSVE = strAdditionalName.Substring(strAdditionalName.Length - 2);
                                                        if (strNameSVE.EndsWith(strSVEType) == true)
                                                            strNameSVE = strAdditionalName.Substring(strAdditionalName.Length - 3, 2);
                                                        if (intServerCount >= 1 && intNameSVE1 == 0)
                                                            intNameSVE1 = oServerName.AddFactory(_os, _location, _mnemonic, _environment, strNameSVE[0].ToString(), strNameSVE[1].ToString(), _function, "A", intProfile, "SVE_ServerID_" + intServer.ToString() + "_A", 0);
                                                        if (intServerCount >= 2 && intNameSVE2 == 0)
                                                            intNameSVE2 = oServerName.AddFactory(_os, _location, _mnemonic, _environment, strNameSVE[0].ToString(), strNameSVE[1].ToString(), _function, "B", intProfile, "SVE_ServerID_" + intServer.ToString() + "_B", 0);
                                                        if (intServerCount >= 3 && intNameSVE3 == 0)
                                                            intNameSVE3 = oServerName.AddFactory(_os, _location, _mnemonic, _environment, strNameSVE[0].ToString(), strNameSVE[1].ToString(), _function, "C", intProfile, "SVE_ServerID_" + intServer.ToString() + "_C", 0);
                                                        if (intServerCount >= 4 && intNameSVE4 == 0)
                                                            intNameSVE4 = oServerName.AddFactory(_os, _location, _mnemonic, _environment, strNameSVE[0].ToString(), strNameSVE[1].ToString(), _function, "D", intProfile, "SVE_ServerID_" + intServer.ToString() + "_D", 0);
                                                        if (intServerCount >= 5 && intNameSVE5 == 0)
                                                            intNameSVE5 = oServerName.AddFactory(_os, _location, _mnemonic, _environment, strNameSVE[0].ToString(), strNameSVE[1].ToString(), _function, "E", intProfile, "SVE_ServerID_" + intServer.ToString() + "_E", 0);
                                                        if (intServerCount >= 6 && intNameSVE6 == 0)
                                                            intNameSVE6 = oServerName.AddFactory(_os, _location, _mnemonic, _environment, strNameSVE[0].ToString(), strNameSVE[1].ToString(), _function, "F", intProfile, "SVE_ServerID_" + intServer.ToString() + "_F", 0);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                // Set the name to the previously generated name (the "B" name for SVE)
                                                if (intNameSVE2 > 0)
                                                {
                                                    intName = intNameSVE2;
                                                    intNameSVE2 = 0;
                                                }
                                                else if (intNameSVE3 > 0)
                                                {
                                                    intName = intNameSVE3;
                                                    intNameSVE3 = 0;
                                                }
                                                else if (intNameSVE4 > 0)
                                                {
                                                    intName = intNameSVE4;
                                                    intNameSVE4 = 0;
                                                }
                                                else if (intNameSVE5 > 0)
                                                {
                                                    intName = intNameSVE5;
                                                    intNameSVE5 = 0;
                                                }
                                                else if (intNameSVE6 > 0)
                                                {
                                                    intName = intNameSVE6;
                                                    intNameSVE6 = 0;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            string strPrefix = "APP";
                                            if (intHost > 0)
                                                strPrefix = oHost.Get(intHost, "prefix");
                                            else if (intApplication > 0)
                                                strPrefix = oServerName.GetApplication(intApplication, "code");
                                            if (strPrefix == "APP")
                                            {
                                                foreach (DataRow drComponent in dsComponents.Tables[0].Rows)
                                                {
                                                    strPrefix = drComponent["code"].ToString();
                                                    break;
                                                }
                                            }
                                            if (strPrefix == "APP" && intInfrastructure > 0)
                                                strPrefix = "UTL";
                                            if (intClusterID > 0)
                                            {
                                                DataSet dsNames = oServerName.GetRelated(intAnswer, intClusterID);
                                                if (dsNames.Tables[0].Rows.Count == 0)
                                                {
                                                    Cluster oCluster = new Cluster(0, dsn);
                                                    DataSet dsCluster = oServer.GetAnswerClusters(intAnswer);
                                                    foreach (DataRow drCluster in dsCluster.Tables[0].Rows)
                                                    {
                                                        int intCluster2 = Int32.Parse(drCluster["clusterid"].ToString());
                                                        if (intCluster2 == intClusterID)
                                                        {
                                                            bool boolSQL = false;
                                                            DataSet dsInstance = oCluster.GetInstances(intCluster2);
                                                            foreach (DataRow drInstance in dsInstance.Tables[0].Rows)
                                                            {
                                                                string strPrefix2 = strPrefix;
                                                                if (drInstance["sql"].ToString() == "1")
                                                                {
                                                                    boolSQL = true;
                                                                    strPrefix2 = "SQL";
                                                                }
                                                                strPrefix2 = "C" + strPrefix2.Substring(0, 2);
                                                                if (oForecast.IsOSMidrange(intAnswer) == true)
                                                                    strPrefix2 = "X" + strPrefix2.Substring(0, 2);
                                                                strPrefix2 = strPrefix2.ToUpper().Trim();
                                                                int intClusterName = oServerName.Add(intClass, intEnv, intAddress, strPrefix2, 0, oCluster.Get(intCluster2, "name"), 1, dsnServiceEditor);
                                                                oServerName.AddRelated(intAnswer, intCluster2, intClusterName);
                                                            }
                                                            int intClusterNameCLU = oServerName.Add(intClass, intEnv, intAddress, "CLU", 0, oCluster.Get(intCluster2, "name"), 1, dsnServiceEditor);
                                                            oServerName.AddRelated(intAnswer, intCluster2, intClusterNameCLU);
                                                            if (boolSQL == true)
                                                            {
                                                                int intDTCName = oServerName.Add(intClass, intEnv, intAddress, "DTC", 0, oCluster.Get(intCluster2, "name"), 1, dsnServiceEditor);
                                                                oServerName.AddRelated(intAnswer, intCluster2, intDTCName);
                                                            }
                                                            break;
                                                        }
                                                    }
                                                }
                                                strPrefix = "N" + strPrefix.Substring(0, 2);
                                                if (oForecast.IsOSMidrange(intAnswer) == true)
                                                    strPrefix = "X" + strPrefix.Substring(0, 2);
                                            }
                                            else if (oForecast.IsOSMidrange(intAnswer) == true)
                                                strPrefix = "X" + strPrefix.Substring(0, 2);
                                            strPrefix = strPrefix.ToUpper().Trim();
                                            intName = oServerName.Add(intClass, intEnv, intAddress, strPrefix, intProfile, "WM_GENERIC", 1, dsnServiceEditor);
                                        }
                                        // If SUN SVE and name has been assigned, use that name.
                                        if (intNameSVE1 > 0)
                                        {
                                            intName = intNameSVE1;
                                            intNameSVE1 = 0;
                                        }
                                    }
                                }
                            }
                            // Assign name
                            string strName = "";
                            string strDR = "";
                            if (boolDR == false && drAnswer["dr"].ToString() == "1")
                                boolDR = true;
                            if (boolDR)
                            {
                                if (drAnswer["dr_exist"].ToString() == "1")
                                    strDR = drAnswer["dr_name"].ToString();
                                else
                                    strDR = strName + "-DR";
                            }
                            else
                                strDR = "N / A";

                            if (intNameAssigned > 0)
                                strName = oServer.GetName(intServer, true);
                            else if (intName > 0)
                            {
                                oServer.UpdateServerNamed(intServer, intName);
                                // Update Asset
                                strName = oServer.GetName(intServer, true);
                                if (intAssetLatest > 0)
                                    oAsset.AddStatus(intAssetLatest, strName, (int)AssetStatus.InUse, intProfile, DateTime.Now);
                                if (intAssetDR > 0)
                                    oAsset.AddStatus(intAssetDR, strDR, (int)AssetStatus.InUse, intProfile, DateTime.Now);
                            }
                            else if (strError == "")
                            {
                                strError = "All available server names are in use for the criteria specified for serverID " + intServer.ToString();
                                break;
                            }
                            else
                                break;
                        }

                        if (strError == "")
                        {
                            int intAdditionalNetwork = 0;
                            int intAdditionalClusterID = 0;
                            foreach (DataRow drAnswer in dsAnswer.Tables[0].Rows)
                            {
                                int intServer = Int32.Parse(drAnswer["id"].ToString());
                                string strName = oServer.GetName(intServer, true);
                                intAdditionalClusterID = Int32.Parse(drAnswer["clusterid"].ToString());
                                bool boolTrunkedSVE = false;
                                if (Request.Form["HDN_" + intServer.ToString() + "_trunking"] != "")
                                    if (Int32.Parse(Request.Form["HDN_" + intServer.ToString() + "_trunking"]) == 1)
                                        boolTrunkedSVE = true;
                                int intOS = 0;
                                if (drAnswer["osid"].ToString() != "")
                                    intOS = Int32.Parse(drAnswer["osid"].ToString());
                                int intIP1 = 0;
                                int intIP2 = 0;
                                int intIP3 = 0;
                                int intIP1Assigned = 0;
                                int intIP2Assigned = 0;
                                int intIP3Assigned = 0;
                                int intIP1Backup = 0;
                                int intIP2Backup = 0;
                                int intIP3Backup = 0;
                                int intIP1Network = 0;
                                int intIP2Network = 0;
                                int intIP3Network = 0;
                                if (Request.Form["HDN_" + intServer.ToString() + "_ip1"] != "")
                                    intIP1 = Int32.Parse(Request.Form["HDN_" + intServer.ToString() + "_ip1"]);
                                if (Request.Form["HDN_" + intServer.ToString() + "_ip2"] != "")
                                    intIP2 = Int32.Parse(Request.Form["HDN_" + intServer.ToString() + "_ip2"]);
                                if (Request.Form["HDN_" + intServer.ToString() + "_ip3"] != "")
                                    intIP3 = Int32.Parse(Request.Form["HDN_" + intServer.ToString() + "_ip3"]);

                                if (oOperatingSystem.IsSolaris(intOS) == true && oModelsProperties.IsSUNVirtual(intModel) == false)
                                {
                                    if (intIP2 < 0)
                                        intIP2 = 0;
                                    if (intIP3 < 0)
                                        intIP3 = 0;
                                }

                                // There are numerous spots where we need the assets assigned, so let's get them now...
                                int intAssetLatest = 0;
                                int intAssetDR = 0;
                                DataSet dsAssets = oServer.GetAssets(intServer);
                                foreach (DataRow drAsset in dsAssets.Tables[0].Rows)
                                {
                                    if (drAsset["latest"].ToString() == "1")
                                        intAssetLatest = Int32.Parse(drAsset["assetid"].ToString());
                                    else if (drAsset["dr"].ToString() == "1")
                                        intAssetDR = Int32.Parse(drAsset["assetid"].ToString());
                                }

                                // Next, let's get the assigned IP address(es)...
                                DataSet dsIP = oServer.GetIP(intServer, 0, 1, 0, 0);
                                foreach (DataRow drIP in dsIP.Tables[0].Rows)
                                {
                                    if (intIP1Assigned == 0)
                                        intIP1Assigned = Int32.Parse(drIP["ipaddressid"].ToString());
                                    else if (intIP2Assigned == 0)
                                        intIP2Assigned = Int32.Parse(drIP["ipaddressid"].ToString());
                                    else if (intIP3Assigned == 0)
                                        intIP3Assigned = Int32.Parse(drIP["ipaddressid"].ToString());
                                }

                                DataSet dsIPBackup = oServer.GetIP(intServer, 0, 0, 0, 1);
                                foreach (DataRow drIPBackup in dsIPBackup.Tables[0].Rows)
                                {
                                    if (intIP1Backup == 0)
                                        intIP1Backup = Int32.Parse(drIPBackup["ipaddressid"].ToString());
                                    else if (intIP2Backup == 0)
                                        intIP2Backup = Int32.Parse(drIPBackup["ipaddressid"].ToString());
                                    else if (intIP3Backup == 0)
                                        intIP3Backup = Int32.Parse(drIPBackup["ipaddressid"].ToString());
                                }

                                // Once switches can be automated (to change VLAN) modify this...
                                bool boolOKtoAssignIP = false;
                                bool boolIsNexusSwitch = false;
                                DataSet dsSwitch = oAsset.GetSwitchports(intAssetLatest, SwitchPortType.Network);
                                if (boolBlade == true)
                                {
                                    if (boolDell == true)
                                    {
                                        boolIsNexusSwitch = true;
                                        int intEnclosure = Int32.Parse(oAsset.GetServerOrBlade(intAssetLatest, "enclosureid"));
                                        int intSlot = Int32.Parse(oAsset.GetServerOrBlade(intAssetLatest, "slot"));
                                        string strEnclosure = oAsset.GetStatus(intEnclosure, "name");
                                        DataSet dsDellBlade = oAsset.GetDellBladeSwitchports(strEnclosure, intSlot);
                                        if (dsDellBlade.Tables[0].Rows.Count == 1)
                                            boolOKtoAssignIP = true;
                                        else
                                            boolOKtoAssignIP = false;
                                    }
                                    else
                                        boolOKtoAssignIP = true;
                                }
                                else
                                {
                                    if (dsSwitch.Tables[0].Rows.Count > 0)
                                    {
                                        boolOKtoAssignIP = true;
                                        // Check to see if switch is nexus, catalyst, etc...
                                        boolIsNexusSwitch = (dsSwitch.Tables[0].Rows[0]["nexus"].ToString() == "1");
                                    }
                                }

                                // Get Components
                                DataSet dsComponents = oServerName.GetComponentDetailSelected(intServer, 1);
                                bool boolWeb = false;
                                int intDellWeb = 0;
                                int intDellMiddleware = 0;
                                int intDellDatabase = 0;
                                int intDellFile = 0;
                                int intDellMisc = 0;
                                int intDellUnder48 = 0;
                                if (oOperatingSystem.IsSolaris(intOS) == true && (boolIsNexusSwitch == true || boolTrunkedSVE)) // assuming all trunked SVE hosts are on Nexus
                                {
                                    // Might need to decide at some point which one takes precedence (if both database and web are selected)
                                    foreach (DataRow drComponent in dsComponents.Tables[0].Rows)
                                    {
                                        if (drComponent["sql"].ToString() == "1" || drComponent["dbase"].ToString() == "1")
                                            intDellDatabase = 1;
                                        if (drComponent["iis"].ToString() == "1" || drComponent["web"].ToString() == "1")
                                        {
                                            intDellWeb = 1;
                                            boolWeb = true;
                                        }
                                    }
                                    intDellUnder48 = (oForecast.IsDRUnder48(intAnswer, false) ? 1 : 0);
                                    if (intDellDatabase == 0 && intDellWeb == 0)
                                        intDellMisc = 1;
                                }
                                else
                                {
                                    foreach (DataRow drComponent in dsComponents.Tables[0].Rows)
                                    {
                                        if (drComponent["iis"].ToString() == "1" || drComponent["web"].ToString() == "1")
                                            boolWeb = true;
                                    }
                                }
                                // Check Load Balancing
                                int intLTM_Web = 0;
                                int intLTM_App = 0;
                                int intLTM_Middleware = 0;
                                if (oForecast.IsHACSM(intAnswer) == true)
                                {
                                    intLTM_Middleware = (oForecast.IsHACSMMiddleware(intAnswer) ? 1 : 0);
                                    // If Yes, then we know it's a Middleware assignment.
                                    if (intLTM_Middleware == 0)
                                    {
                                        // If No, we need to continue....
                                        if (boolWeb == false && oForecast.IsHACSMWeb(intAnswer) == false)
                                        {
                                            // If a web component is NOT selected, we know its an App Tier assignment.
                                            intLTM_App = 1;
                                        }
                                        else
                                        {
                                            // If a web component is selected (IIS, Apache, etc...), prompt the user...
                                            if (oForecast.IsHACSMApp(intAnswer) == true)
                                            {
                                                // If Yes, then we know it's a Web + App Tier assignment
                                                intLTM_App = 1;
                                                intLTM_Web = 1;
                                            }
                                            else
                                            {
                                                // If No, then we know it's a Web Tier assignment.
                                                intLTM_Web = 1;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        intDellDatabase = 0;
                                        intDellWeb = 0;
                                        intDellMisc = 0;
                                    }
                                }


                                if (strError == "")
                                {
                                    // Time for the IP Address (#1)
                                    if (intIP1Assigned == 0)
                                    {
                                        if (intIP1 == -999)
                                        {
                                            // Manual
                                            strError = AddIPs(intServer, 0, 0, "HDN_" + intServer.ToString() + "_manual_ip1_", intModel, oServer.GetName(intServer, true));
                                        }
                                        else if (intIP1 > -1)
                                        {
                                            if (radNamesYes.Checked == true)
                                                intIP1 = 0;
                                            bool boolDHCP = false;
                                            if (intIP1 == 0)
                                            {
                                                if (oModelsProperties.IsSUNVirtual(intModel) == true || strModel.ToUpper().StartsWith("MICROLPAR") == true)
                                                {
                                                    if (oModelsProperties.IsSUNVirtual(intModel) == true)
                                                    {
                                                        if (boolTrunkedSVE)
                                                        {
                                                            oLog.AddEvent(intAnswer, strName, "", "Getting IP Address #1 for SVE Trunked (Class = " + intClass.ToString() + ", Env = " + intEnv.ToString() + ", Address = " + intAddress.ToString() + ", Web = " + intDellWeb.ToString() + ", Middleware = " + intDellMiddleware.ToString() + ", Database = " + intDellDatabase.ToString() + ", File = " + intDellFile.ToString() + ", Misc = " + intDellMisc.ToString() + ", Under48 = " + intDellUnder48.ToString() + ", Resiliency: " + oResiliency.Get(intResiliency, "name") + ", Avamar = 0)", LoggingType.Information);
                                                            intIP1 = oIPAddresses.Get_Dell(intClass, intEnv, intAddress, 0, 0, 0, 0, 0, 0, 0, intDellWeb, intDellMiddleware, intDellDatabase, intDellFile, intDellMisc, intDellUnder48, 0, true, intServer, 0, intResiliency, intEnvironment, dsnServiceEditor);
                                                        }
                                                        else
                                                        {
                                                            oLog.AddEvent(intAnswer, strName, "", "Getting IP Address #1 for NON-SVE Trunked (Class = " + intClass.ToString() + ", Env = " + intEnv.ToString() + ", Address = " + intAddress.ToString() + ", sun_sve: 1, ServerID = " + intServer.ToString() + ")", LoggingType.Information);
                                                            intIP1 = oIPAddresses.Get_(intClass, intEnv, intAddress, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, true, intServer, intEnvironment, dsnServiceEditor);
                                                        }
                                                    }
                                                    if (strModel.ToUpper().StartsWith("MICROLPAR") == true)
                                                        intIP1 = oIPAddresses.Get_(intClass, intEnv, intAddress, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, true, intServer, intEnvironment, dsnServiceEditor);
                                                    // Log IP Address Query
                                                    oServer.AddOutput(intServer, "IP_ASSIGN_1", oIPAddresses.Results());
                                                    oIPAddresses.ClearResults();
                                                }
                                                else if (oModelsProperties.IsVMwareVirtual(intModel) == true)
                                                {
                                                    // There shouldn't be any vmware going through the manual process
                                                }
                                                else if (oModelsProperties.IsTypeBlade(intModel) || oModelsProperties.IsTypePhysical(intModel))
                                                {
                                                    int intVLAN = 0;
                                                    if (oAsset.GetServerOrBlade(intAssetLatest, "vlan") != "")
                                                        intVLAN = Int32.Parse(oAsset.GetServerOrBlade(intAssetLatest, "vlan"));
                                                    if (intVLAN == -999)    // DHCP
                                                        boolDHCP = true;
                                                    else if (intVLAN == 999)     // Nexus (Dell)
                                                    {
                                                        oLog.AddEvent(intAnswer, strName, "", "Getting IP Address #1 for Dell (Class = " + intClass.ToString() + ", Env = " + intEnv.ToString() + ", Address = " + intAddress.ToString() + ", LTM (Web) = " + intLTM_Web.ToString() + ", LTM (App) = " + intLTM_App.ToString() + ", LTM (Middleware) = " + intLTM_Middleware.ToString() + ", Web = " + intDellWeb.ToString() + ", Middleware = " + intDellMiddleware.ToString() + ", Database = " + intDellDatabase.ToString() + ", File = " + intDellFile.ToString() + ", Misc = " + intDellMisc.ToString() + ", Under48 = " + intDellUnder48.ToString() + ", Resiliency: " + oResiliency.Get(intResiliency, "name") + ", Avamar = 0)", LoggingType.Information);
                                                        intIP1 = oIPAddresses.Get_Dell(intClass, intEnv, intAddress, 0, 0, intLTM_Web, intLTM_App, intLTM_Middleware, 0, 0, intDellWeb, intDellMiddleware, intDellDatabase, intDellFile, intDellMisc, intDellUnder48, 0, true, intServer, 0, intResiliency, intEnvironment, dsnServiceEditor);
                                                    }
                                                    else
                                                    {
                                                        if (oModelsProperties.IsTypeBlade(intModel) == true)
                                                        {
                                                            oLog.AddEvent(intAnswer, strName, "", "Getting IP Address #1 for HP BLADE (Class = " + intClass.ToString() + ", Env = " + intEnv.ToString() + ", Address = " + intAddress.ToString() + ", VLAN = " + intVLAN.ToString() + ", HA = No)", LoggingType.Information);
                                                            intIP1 = oIPAddresses.Get_Blade_HP(intClass, intEnv, intAddress, 0, 0, intVLAN, 0, 0, true, intServer, intResiliency, intEnvironment, dsnServiceEditor);
                                                        }
                                                        else
                                                        {
                                                            oLog.AddEvent(intAnswer, strName, "", "Getting IP Address #1 for PHYSICAL (Class = " + intClass.ToString() + ", Env = " + intEnv.ToString() + ", Address = " + intAddress.ToString() + ", VLAN = " + intVLAN.ToString() + ", HA = No)", LoggingType.Information);
                                                            intIP1 = oIPAddresses.Get_VLAN(intClass, intEnv, intAddress, 0, 0, intVLAN, 0, 0, 0, 0, 0, 0, 0, true, intServer, 0, 0, intResiliency, intEnvironment, dsnServiceEditor);
                                                        }
                                                    }
                                                    // Log IP Address Query
                                                    oServer.AddOutput(intServer, "IP_ASSIGN_1", oIPAddresses.Results());
                                                    oIPAddresses.ClearResults();
                                                }
                                            }
                                            if (intIP1 > 0)
                                            {
                                                bool boolBackupIP = false;
                                                //if (oLocation.GetAddress(intAddress, "tsm") != "1")
                                                //{
                                                //    if (intIP1Backup == 0)
                                                //    {
                                                //        oLog.AddEvent(strName, "", "Getting Backup IP Address #1", LoggingType.Information);
                                                //        if (oModelsProperties.IsSUNVirtual(intModel) == true)
                                                //            intIP1Backup = oIPAddresses.Get_Dell(intClass, intEnv, intAddress, 0, 0, 0, 0, 0, 0, (boolTrunkedSVE ? 0 : 1), 0, 0, 0, 0, 0, 0, 1, true, 0, 0, intResiliency, intEnvironment, dsnServiceEditor);
                                                //        else
                                                //            intIP1Backup = oIPAddresses.Get_Dell(intClass, intEnv, intAddress, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, true, 0, 0, intResiliency, intEnvironment, dsnServiceEditor);
                                                //        // Log IP Address Query
                                                //        oServer.AddOutput(intServer, "IP_BACKUP_1", oIPAddresses.Results());
                                                //        oIPAddresses.ClearResults();
                                                //    }
                                                //    if (intIP1Backup == 0)
                                                //    {
                                                //        oIPAddresses.Delete(intIP1);
                                                //        strError = "Error generating a Backup IP Address (#1) for serverID " + intServer.ToString();
                                                //        break;
                                                //    }
                                                //    else
                                                //        boolBackupIP = true;
                                                //}
                                                //else
                                                    boolBackupIP = true;

                                                if (boolBackupIP == true)
                                                {
                                                    oServer.AddIP(intServer, intIP1, 0, 1, 0, 0);
                                                    //if (intIP1Backup > 0)
                                                    //    oServer.AddIP(intServer, intIP1Backup, 0, 0, 0, 1);
                                                }
                                            }
                                            if (boolDHCP)
                                                oLog.AddEvent(strName, "", "Asset VLAN = -999 (DHCP)...skipping IP Address (#1)", LoggingType.Information);
                                            else if (intIP1 == 0)
                                            {
                                                strError = "Error generating an IP Address (#1) for serverID " + intServer.ToString();
                                                break;
                                            }
                                            else
                                            {
                                                intIP1Network = oIPAddresses.GetAddressNetwork(intIP1);
                                                intAdditionalNetwork = intIP1Network;
                                            }
                                        }
                                    }
                                }

                                if (strError == "")
                                {
                                    // Time for the IP Address (#2)
                                    if (intIP2Assigned == 0 && intIP2 != intIP1Backup)  // Single server with backup ip address
                                    {
                                        if (intIP2 == -999)
                                        {
                                            // Manual
                                            strError = AddIPs(intServer, 0, 0, "HDN_" + intServer.ToString() + "_manual_ip2_", intModel, oServer.GetName(intServer, true));
                                        }
                                        else if (intIP2 > -1)
                                        {
                                            if (radNamesYes.Checked == true)
                                                intIP2 = 0;
                                            bool boolDHCP = false;
                                            if (intIP2 == 0)
                                            {
                                                if (oModelsProperties.IsSUNVirtual(intModel) == true || strModel.ToUpper().StartsWith("MICROLPAR") == true)
                                                {
                                                    if (oModelsProperties.IsSUNVirtual(intModel) == true)
                                                    {
                                                        if (boolTrunkedSVE)
                                                        {
                                                            oLog.AddEvent(intAnswer, strName, "", "Getting IP Address #2 for SVE Trunked (Class = " + intClass.ToString() + ", Env = " + intEnv.ToString() + ", Address = " + intAddress.ToString() + ", Web = " + intDellWeb.ToString() + ", Middleware = " + intDellMiddleware.ToString() + ", Database = " + intDellDatabase.ToString() + ", File = " + intDellFile.ToString() + ", Misc = " + intDellMisc.ToString() + ", Under48 = " + intDellUnder48.ToString() + ", Resiliency: " + oResiliency.Get(intResiliency, "name") + ", Avamar = 0)", LoggingType.Information);
                                                            intIP2 = oIPAddresses.Get_Dell(intClass, intEnv, intAddress, 0, 0, 0, 0, 0, 0, 0, intDellWeb, intDellMiddleware, intDellDatabase, intDellFile, intDellMisc, intDellUnder48, 0, true, intServer, 0, intResiliency, intEnvironment, dsnServiceEditor);
                                                        }
                                                        else
                                                            intIP2 = oIPAddresses.Get_(intClass, intEnv, intAddress, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, true, intServer, intEnvironment, dsnServiceEditor);
                                                    }
                                                    if (strModel.ToUpper().StartsWith("MICROLPAR") == true)
                                                        intIP2 = oIPAddresses.Get_(intClass, intEnv, intAddress, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, true, intServer, intEnvironment, dsnServiceEditor);
                                                    // Log IP Address Query
                                                    oServer.AddOutput(intServer, "IP_ASSIGN_2", oIPAddresses.Results());
                                                    oIPAddresses.ClearResults();
                                                }
                                                else if (oModelsProperties.IsVMwareVirtual(intModel) == true)
                                                {
                                                    // There shouldn't be any vmware going through the manual process
                                                }
                                                else if (oModelsProperties.IsTypeBlade(intModel) || oModelsProperties.IsTypePhysical(intModel))
                                                {
                                                    int intVLAN = 0;
                                                    if (oAsset.GetServerOrBlade(intAssetLatest, "vlan") != "")
                                                        intVLAN = Int32.Parse(oAsset.GetServerOrBlade(intAssetLatest, "vlan"));
                                                    if (intVLAN == -999)    // DHCP
                                                        boolDHCP = true;
                                                    else if (intVLAN == 999)     // Nexus (Dell)
                                                    {
                                                        oLog.AddEvent(intAnswer, strName, "", "Getting IP Address #2 for Dell (Class = " + intClass.ToString() + ", Env = " + intEnv.ToString() + ", Address = " + intAddress.ToString() + ", LTM (Web) = " + intLTM_Web.ToString() + ", LTM (App) = " + intLTM_App.ToString() + ", LTM (Middleware) = " + intLTM_Middleware.ToString() + ", Web = " + intDellWeb.ToString() + ", Middleware = " + intDellMiddleware.ToString() + ", Database = " + intDellDatabase.ToString() + ", File = " + intDellFile.ToString() + ", Misc = " + intDellMisc.ToString() + ", Under48 = " + intDellUnder48.ToString() + ", Resiliency: " + oResiliency.Get(intResiliency, "name") + ", Avamar = 0)", LoggingType.Information);
                                                        intIP2 = oIPAddresses.Get_Dell(intClass, intEnv, intAddress, 0, 0, intLTM_Web, intLTM_App, intLTM_Middleware, 0, 0, intDellWeb, intDellMiddleware, intDellDatabase, intDellFile, intDellMisc, intDellUnder48, 0, true, intServer, 0, intResiliency, intEnvironment, dsnServiceEditor);
                                                    }
                                                    else
                                                    {
                                                        if (oModelsProperties.IsTypeBlade(intModel) == true)
                                                        {
                                                            oLog.AddEvent(intAnswer, strName, "", "Getting IP Address #2 for HP BLADE (Class = " + intClass.ToString() + ", Env = " + intEnv.ToString() + ", Address = " + intAddress.ToString() + ", VLAN = " + intVLAN.ToString() + ", HA = No)", LoggingType.Information);
                                                            intIP2 = oIPAddresses.Get_Blade_HP(intClass, intEnv, intAddress, 0, 0, intVLAN, 0, 0, true, intServer, intResiliency, intEnvironment, dsnServiceEditor);
                                                        }
                                                        else
                                                        {
                                                            oLog.AddEvent(intAnswer, strName, "", "Getting IP Address #2 for PHYSICAL (Class = " + intClass.ToString() + ", Env = " + intEnv.ToString() + ", Address = " + intAddress.ToString() + ", VLAN = " + intVLAN.ToString() + ", HA = No)", LoggingType.Information);
                                                            intIP2 = oIPAddresses.Get_VLAN(intClass, intEnv, intAddress, 0, 0, intVLAN, 0, 0, 0, 0, 0, 0, 0, true, intServer, 0, 0, intResiliency, intEnvironment, dsnServiceEditor);
                                                        }
                                                    }
                                                    // Log IP Address Query
                                                    oServer.AddOutput(intServer, "IP_ASSIGN_2", oIPAddresses.Results());
                                                    oIPAddresses.ClearResults();
                                                }
                                            }
                                            if (intIP2 > 0)
                                            {
                                                bool boolBackupIP = false;
                                                //if (oLocation.GetAddress(intAddress, "tsm") != "1")
                                                //{
                                                //    if (intIP2Backup == 0)
                                                //    {
                                                //        oLog.AddEvent(strName, "", "Getting Backup IP Address #2", LoggingType.Information);
                                                //        if (oModelsProperties.IsSUNVirtual(intModel) == true)
                                                //            intIP2Backup = oIPAddresses.Get_Dell(intClass, intEnv, intAddress, 0, 0, 0, 0, 0, 0, (boolTrunkedSVE ? 0 : 1), 0, 0, 0, 0, 0, 0, 1, true, 0, 0, intResiliency, intEnvironment, dsnServiceEditor);
                                                //        else
                                                //            intIP2Backup = oIPAddresses.Get_Dell(intClass, intEnv, intAddress, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, true, 0, 0, intResiliency, intEnvironment, dsnServiceEditor);
                                                //        // Log IP Address Query
                                                //        oServer.AddOutput(intServer, "IP_BACKUP_2", oIPAddresses.Results());
                                                //        oIPAddresses.ClearResults();
                                                //    }
                                                //    if (intIP2Backup == 0)
                                                //    {
                                                //        oIPAddresses.Delete(intIP2);
                                                //        strError = "Error generating a Backup IP Address (#2) for serverID " + intServer.ToString();
                                                //        break;
                                                //    }
                                                //    else
                                                //        boolBackupIP = true;
                                                //}
                                                //else
                                                    boolBackupIP = true;

                                                if (boolBackupIP == true)
                                                {
                                                    oServer.AddIP(intServer, intIP2, 0, 1, 0, 0);
                                                    //if (intIP1Backup > 0)
                                                    //    oServer.AddIP(intServer, intIP2Backup, 0, 0, 0, 1);
                                                }
                                            }
                                            if (boolDHCP)
                                                oLog.AddEvent(strName, "", "Asset VLAN = -999 (DHCP)...skipping IP Address (#2)", LoggingType.Information);
                                            else if (intIP2 == 0)
                                            {
                                                strError = "Error generating an IP Address (#2) for serverID " + intServer.ToString();
                                                break;
                                            }
                                            else
                                                intIP2Network = oIPAddresses.GetAddressNetwork(intIP2);
                                        }
                                    }
                                }

                                if (strError == "")
                                {
                                    // Time for the IP Address (#3)
                                    if (intIP3Assigned == 0)
                                    {
                                        if (intIP3 == -999)
                                        {
                                            // Manual
                                            strError = AddIPs(intServer, 0, 0, "HDN_" + intServer.ToString() + "_manual_ip3_", intModel, oServer.GetName(intServer, true));
                                        }
                                        else if (intIP3 > -1)
                                        {
                                            if (radNamesYes.Checked == true)
                                                intIP3 = 0;
                                            bool boolDHCP = false;
                                            if (intIP3 == 0)
                                            {
                                                if (oModelsProperties.IsSUNVirtual(intModel) == true || strModel.ToUpper().StartsWith("MICROLPAR") == true)
                                                {
                                                    if (oModelsProperties.IsSUNVirtual(intModel) == true)
                                                    {
                                                        if (boolTrunkedSVE)
                                                        {
                                                            oLog.AddEvent(intAnswer, strName, "", "Getting IP Address #3 for SVE Trunked (Class = " + intClass.ToString() + ", Env = " + intEnv.ToString() + ", Address = " + intAddress.ToString() + ", Web = " + intDellWeb.ToString() + ", Middleware = " + intDellMiddleware.ToString() + ", Database = " + intDellDatabase.ToString() + ", File = " + intDellFile.ToString() + ", Misc = " + intDellMisc.ToString() + ", Under48 = " + intDellUnder48.ToString() + ", Resiliency: " + oResiliency.Get(intResiliency, "name") + ", Avamar = 0)", LoggingType.Information);
                                                            intIP3 = oIPAddresses.Get_Dell(intClass, intEnv, intAddress, 0, 0, 0, 0, 0, 0, 0, intDellWeb, intDellMiddleware, intDellDatabase, intDellFile, intDellMisc, intDellUnder48, 0, true, intServer, 0, intResiliency, intEnvironment, dsnServiceEditor);
                                                        }
                                                        else
                                                            intIP3 = oIPAddresses.Get_(intClass, intEnv, intAddress, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, true, intServer, intEnvironment, dsnServiceEditor);
                                                    }
                                                    if (strModel.ToUpper().StartsWith("MICROLPAR") == true)
                                                        intIP3 = oIPAddresses.Get_(intClass, intEnv, intAddress, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, true, intServer, intEnvironment, dsnServiceEditor);
                                                    // Log IP Address Query
                                                    oServer.AddOutput(intServer, "IP_ASSIGN_3", oIPAddresses.Results());
                                                    oIPAddresses.ClearResults();
                                                }
                                                else if (oModelsProperties.IsVMwareVirtual(intModel) == true)
                                                {
                                                    // There shouldn't be any vmware going through the manual process
                                                }
                                                else if (oModelsProperties.IsTypeBlade(intModel) || oModelsProperties.IsTypePhysical(intModel))
                                                {
                                                    int intVLAN = 0;
                                                    if (oAsset.GetServerOrBlade(intAssetLatest, "vlan") != "")
                                                        intVLAN = Int32.Parse(oAsset.GetServerOrBlade(intAssetLatest, "vlan"));
                                                    if (intVLAN == -999)    // DHCP
                                                        boolDHCP = true;
                                                    else if (intVLAN == 999)     // Nexus (Dell)
                                                    {
                                                        oLog.AddEvent(intAnswer, strName, "", "Getting IP Address #3 for Dell (Class = " + intClass.ToString() + ", Env = " + intEnv.ToString() + ", Address = " + intAddress.ToString() + ", LTM (Web) = " + intLTM_Web.ToString() + ", LTM (App) = " + intLTM_App.ToString() + ", LTM (Middleware) = " + intLTM_Middleware.ToString() + ", Web = " + intDellWeb.ToString() + ", Middleware = " + intDellMiddleware.ToString() + ", Database = " + intDellDatabase.ToString() + ", File = " + intDellFile.ToString() + ", Misc = " + intDellMisc.ToString() + ", Under48 = " + intDellUnder48.ToString() + ", Resiliency: " + oResiliency.Get(intResiliency, "name") + ", Avamar = 0)", LoggingType.Information);
                                                        intIP3 = oIPAddresses.Get_Dell(intClass, intEnv, intAddress, 0, 0, intLTM_Web, intLTM_App, intLTM_Middleware, 0, 0, intDellWeb, intDellMiddleware, intDellDatabase, intDellFile, intDellMisc, intDellUnder48, 0, true, intServer, 0, intResiliency, intEnvironment, dsnServiceEditor);
                                                    }
                                                    else
                                                    {
                                                        if (oModelsProperties.IsTypeBlade(intModel) == true)
                                                        {
                                                            oLog.AddEvent(intAnswer, strName, "", "Getting IP Address #3 for HP BLADE (Class = " + intClass.ToString() + ", Env = " + intEnv.ToString() + ", Address = " + intAddress.ToString() + ", VLAN = " + intVLAN.ToString() + ", HA = No)", LoggingType.Information);
                                                            intIP3 = oIPAddresses.Get_Blade_HP(intClass, intEnv, intAddress, 0, 0, intVLAN, 0, 0, true, intServer, intResiliency, intEnvironment, dsnServiceEditor);
                                                        }
                                                        else
                                                        {
                                                            oLog.AddEvent(intAnswer, strName, "", "Getting IP Address #3 for PHYSICAL (Class = " + intClass.ToString() + ", Env = " + intEnv.ToString() + ", Address = " + intAddress.ToString() + ", VLAN = " + intVLAN.ToString() + ", HA = No)", LoggingType.Information);
                                                            intIP3 = oIPAddresses.Get_VLAN(intClass, intEnv, intAddress, 0, 0, intVLAN, 0, 0, 0, 0, 0, 0, 0, true, intServer, 0, 0, intResiliency, intEnvironment, dsnServiceEditor);
                                                        }
                                                    }
                                                    // Log IP Address Query
                                                    oServer.AddOutput(intServer, "IP_ASSIGN_3", oIPAddresses.Results());
                                                    oIPAddresses.ClearResults();
                                                }
                                            }
                                            if (intIP3 > 0)
                                            {
                                                bool boolBackupIP = false;
                                                //if (oLocation.GetAddress(intAddress, "tsm") != "1")
                                                //{
                                                //    if (intIP3Backup == 0)
                                                //    {
                                                //        oLog.AddEvent(strName, "", "Getting Backup IP Address #3", LoggingType.Information);
                                                //        if (oModelsProperties.IsSUNVirtual(intModel) == true)
                                                //            intIP3Backup = oIPAddresses.Get_Dell(intClass, intEnv, intAddress, 0, 0, 0, 0, 0, 0, (boolTrunkedSVE ? 0 : 1), 0, 0, 0, 0, 0, 0, 1, true, 0, 0, intResiliency, intEnvironment, dsnServiceEditor);
                                                //        else
                                                //            intIP3Backup = oIPAddresses.Get_Dell(intClass, intEnv, intAddress, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, true, 0, 0, intResiliency, intEnvironment, dsnServiceEditor);
                                                //        // Log IP Address Query
                                                //        oServer.AddOutput(intServer, "IP_BACKUP_3", oIPAddresses.Results());
                                                //        oIPAddresses.ClearResults();
                                                //    }
                                                //    if (intIP3Backup == 0)
                                                //    {
                                                //        oIPAddresses.Delete(intIP3);
                                                //        strError = "Error generating a Backup IP Address (#3) for serverID " + intServer.ToString();
                                                //        break;
                                                //    }
                                                //    else
                                                //        boolBackupIP = true;
                                                //}
                                                //else
                                                    boolBackupIP = true;

                                                if (boolBackupIP == true)
                                                {
                                                    oServer.AddIP(intServer, intIP3, 0, 1, 0, 0);
                                                    //if (intIP1Backup > 0)
                                                    //    oServer.AddIP(intServer, intIP3Backup, 0, 0, 0, 1);
                                                }
                                            }
                                            if (boolDHCP)
                                                oLog.AddEvent(strName, "", "Asset VLAN = -999 (DHCP)...skipping IP Address (#3)", LoggingType.Information);
                                            else if (intIP3 == 0)
                                            {
                                                strError = "Error generating an IP Address (#3) for serverID " + intServer.ToString();
                                                break;
                                            }
                                            else
                                                intIP3Network = oIPAddresses.GetAddressNetwork(intIP3);
                                        }
                                    }
                                }
                            }

                            string strAdditionalManual = "";
                            if (strError == "")
                            {
                                // Additional Name
                                if (boolManualName == true && txtAdditionalName.Text.Trim() != "")
                                {
                                    string strName = txtAdditionalName.Text.Trim().ToUpper();
                                    // Additional Names
                                    if (oModelsProperties.IsSUNVirtual(intModel) == true)
                                    {
                                        if (strSVEType == "DB")
                                        {
                                            // Database naming = Mnemonic + Environment (DBQ) + SEQUENCE (00) + (A,B,C,etc...)
                                            if (strName.Length == 8 || strName.Length == 9)
                                            {
                                                string strMnemonic = strName.Substring(0, 3);
                                                string strEnvironment = strName.Substring(3, 3);
                                                string strName1 = strName.Substring(6, 1);
                                                string strName2 = strName.Substring(7, 1);
                                                string strFunc = "";
                                                if (strName.Length == 9)
                                                    strFunc = strName.Substring(8, 1);
                                                int intName = oServerName.AddFactory("", "", strMnemonic, strEnvironment, strName1, strName2, strFunc, "", intProfile, "GENERIC" + intAnswer.ToString() + "_ADDITIONAL", 0);
                                                oServerName.AddRelated(intAnswer, intAdditionalClusterID, intName);
                                                strAdditionalManual = oServerName.GetNameFactory(intName, 0);
                                            }
                                            else
                                                strError = "The name " + strName + " is not 8 or 9 characters long and therefore, is invalid.";
                                        }
                                        else if (strSVEType == "Z")
                                        {
                                            // Non-DB naming = SCXXX104Z = SCXXX104ZA + SCXXX104ZB
                                            if (strName.Length == 9 || strName.Length == 10)
                                            {
                                                string strOS = strName.Substring(0, 1);
                                                string strLocation = strName.Substring(1, 1);
                                                string strMnemonic = strName.Substring(2, 3);
                                                string strEnvironment = strName.Substring(5, 1);
                                                string strName1 = strName.Substring(6, 1);
                                                string strName2 = strName.Substring(7, 1);
                                                string strFunc = strName.Substring(8, 1);
                                                string strSpecific = "";
                                                if (strName.Length == 10)
                                                    strSpecific = strName.Substring(9, 1);
                                                int intName = oServerName.AddFactory(strOS, strLocation, strMnemonic, strEnvironment, strName1, strName2, strFunc, strSpecific, intProfile, "GENERIC" + intAnswer.ToString() + "_ADDITIONAL", 0);
                                                oServerName.AddRelated(intAnswer, intAdditionalClusterID, intName);
                                                strAdditionalManual = oServerName.GetNameFactory(intName, 0);
                                            }
                                            else
                                                strError = "The name " + strName + " is not 9 or 10 characters long and therefore, is invalid.";
                                        }
                                        else
                                        {
                                            strError = "The SUN SVE type (" + strSVEType + ") is not configured for naming";
                                        }
                                    }
                                    else
                                    {
                                        if (boolPNC == true)
                                        {
                                            // Example = WCXXX104AZ
                                            if (strName.Length == 9 || strName.Length == 10)
                                            {
                                                string strOS = strName.Substring(0, 1);
                                                string strLocation = strName.Substring(1, 1);
                                                string strMnemonic = strName.Substring(2, 3);
                                                string strEnvironment = strName.Substring(5, 1);
                                                string strName1 = strName.Substring(6, 1);
                                                string strName2 = strName.Substring(7, 1);
                                                string strFunc = strName.Substring(8, 1);
                                                string strSpecific = "";
                                                if (strName.Length == 10)
                                                    strSpecific = strName.Substring(9, 1);
                                                int intName = oServerName.AddFactory(strOS, strLocation, strMnemonic, strEnvironment, strName1, strName2, strFunc, strSpecific, intProfile, "GENERIC" + intAnswer.ToString() + "_ADDITIONAL", 0);
                                                oServerName.AddRelated(intAnswer, intAdditionalClusterID, intName);
                                                strAdditionalManual = oServerName.GetNameFactory(intName, 0);
                                            }
                                            else
                                                strError = "The name " + strName + " is not 9 or 10 characters long and therefore, is invalid.";
                                        }
                                        else
                                        {
                                            if (strName.Length == 12)
                                            {
                                                string strPrefix1 = strName.Substring(0, 5);
                                                string strPrefix2 = strName.Substring(5, 3);
                                                string strCode = strName.Substring(8, 2);
                                                string strName1 = strName.Substring(10, 1);
                                                string strName2 = strName.Substring(11, 1);
                                                int intName = oServerName.Add(0, strPrefix1, strPrefix2, strCode, strName1, strName2, intProfile, "GENERIC" + intAnswer.ToString() + "_ADDITIONAL", 0);
                                                oServerName.AddRelated(intAnswer, intAdditionalClusterID, intName);
                                                strAdditionalManual = oServerName.GetName(intName, 0);
                                            }
                                            else
                                                strError = "The name " + strName + " is not 12 characters long and therefore, is invalid.";
                                        }
                                    }
                                }
                            }

                            if (strError == "")
                            {
                                // Time for any additional IP addresses
                                if (boolManualIP == true)
                                {
                                    strError = AddIPs(0, intAnswer, intAdditionalClusterID, txtAdditionalIP1.Text, txtAdditionalIP2.Text, txtAdditionalIP3.Text, txtAdditionalIP4.Text, intModel, strAdditionalManual);
                                }
                                else
                                {
                                    bool boolAdditionalIP = false;
                                    if (oModelsProperties.IsSUNVirtual(intModel) == true && boolSVEClustered == true)
                                        boolAdditionalIP = true;
                                    if (boolAdditionalIP == true)
                                    {
                                        int intAdditionalIP = 0;
                                        DataSet dsAdditionalIP = oIPAddresses.GetRelated(intAnswer, intAdditionalClusterID);
                                        if (dsAdditionalIP.Tables[0].Rows.Count > 0)
                                            Int32.TryParse(dsAdditionalIP.Tables[0].Rows[0]["ipaddressid"].ToString(), out intAdditionalIP);

                                        if (intAdditionalIP == 0)
                                        {
                                            // Must get one.
                                            if (oModelsProperties.IsSUNVirtual(intModel) == true)
                                            {
                                                intAdditionalIP = oIPAddresses.Get_Network(intClass, intEnv, intAddress, 0, 0, intAdditionalNetwork, true, intEnvironment, dsnServiceEditor);
                                            }

                                            if (intAdditionalIP > 0)
                                            {
                                                oIPAddresses.AddRelated(intAnswer, intAdditionalClusterID, intAdditionalIP);
                                            }
                                        }
                                        if (intAdditionalIP == 0)
                                            strError = "Error generating an Additional IP Address for designID " + intAnswer.ToString();
                                    }
                                }
                            }
                        }

                            
                            
                            
                            
                            

                        if (strError == "" && oModelsProperties.IsSUNVirtual(intModel) == true)
                        {
                            // DNS should be registered prior to building the server for SVE
                            DataSet dsManual = oServer.GetManual(intAnswer, false);
                            if (intEnvironment == (int)CurrentEnvironment.CORPDMN || intEnvironment == (int)CurrentEnvironment.PNCNT_PROD)
                            {
                                System.Net.NetworkCredential oCredentialsDNS = new System.Net.NetworkCredential(oVariable.ADUser(), oVariable.ADPassword(), oVariable.Domain());
                                ClearViewWebServices oWebService = new ClearViewWebServices();
                                oWebService.Timeout = Timeout.Infinite;
                                oWebService.Credentials = oCredentialsDNS;
                                oWebService.Url = oVariable.WebServiceURL();
                                bool boolDNS_QIP = oSetting.IsDNS_QIP();
                                bool boolDNS_Bluecat = oSetting.IsDNS_Bluecat();
                                foreach (DataRow drManual in dsManual.Tables[0].Rows)
                                {
                                    int intServer = Int32.Parse(drManual["id"].ToString());
                                    string strName = drManual["servername"].ToString();
                                    string strSerial = drManual["serial"].ToString();
                                    string strIP = "";
                                    string strBackup = "";

                                    if (drManual["finalid1"].ToString() == "1")
                                        strIP = drManual["ipaddress1"].ToString();
                                    if (drManual["finalid1"].ToString() == "0")
                                        strBackup = drManual["ipaddress1"].ToString();

                                    if (drManual["finalid2"].ToString() == "1" && strIP == "")
                                        strIP = drManual["ipaddress2"].ToString();
                                    if (drManual["finalid2"].ToString() == "0" && strBackup == "")
                                        strBackup = drManual["ipaddress2"].ToString();

                                    if (drManual["finalid3"].ToString() == "1" && strIP == "")
                                        strIP = drManual["ipaddress3"].ToString();
                                    if (drManual["finalid3"].ToString() == "0" && strBackup == "")
                                        strBackup = drManual["ipaddress3"].ToString();

                                    string strDNSAuto = oServer.Get(intServer, "dns_auto");

                                    if (strIP != "" && strDNSAuto != "1")
                                    {
                                        // Auto-Register in DNS
                                        if (boolDNS_QIP == true)
                                        {
                                            oLog.AddEvent(strName, strSerial, "Calling Web Service Function CreateDNSforPNC(" + strIP + ", " + strName + ", Server, , " + oVariable.DNS_Domain() + ", " + oVariable.DNS_NameService() + ", " + oVariable.DNS_DynamicDNSUpdate() + ", " + intProfile.ToString() + ", true) on " + oVariable.WebServiceURL(), LoggingType.Information);
                                            string strDNS = oWebService.CreateDNSforPNC(strIP, strName, "Server", "", oVariable.DNS_Domain(), oVariable.DNS_NameService(), oVariable.DNS_DynamicDNSUpdate(), intProfile, intAnswer, true);
                                            if (strDNS == "SUCCESS" || strDNS.StartsWith("***DUPLICATE") == true)
                                            {
                                                oLog.AddEvent(strName, strSerial, "QIP DNS Record = SUCCESS", LoggingType.Information);
                                                // The script ran successfully
                                                if (boolDNS_Bluecat == false)
                                                    oServer.UpdateDNS(intServer, 1, "Completed");
                                            }
                                            else
                                            {
                                                if (strDNS.StartsWith("***CONFLICT") == true)
                                                {
                                                    oLog.AddEvent(strName, strSerial, "QIP DNS Record = CONFLICT", LoggingType.Warning);
                                                    strWarning = "The IP Address " + strIP + " for device " + strName + " was not successfully registered in QIP DNS (Conflict)";
                                                    // A conflict occurred...awaiting the service technician to fix
                                                    if (boolDNS_Bluecat == false)
                                                        oServer.UpdateDNS(intServer, 0, "Conflict");
                                                    string strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DEVELOPER_WARNING");
                                                    oFunction.SendEmail("QIP DNS Automation Conflict", strEMailIdsBCC, "", "", "QIP DNS Automation Conflict", "<p>There was a CONFLICT creating the QIP DNS Record for " + strName + " (SERVERID: " + intServer.ToString() + ") (ANSWERID: " + intAnswer.ToString() + ")</p><p>Error: " + strDNS + "</p><p>NOTE: A request should have been generated and assigned to a DNS technician.</p>", true, false);
                                                }
                                                else if (strDNS.ToUpper().Contains("SUBNET FOR") == true)
                                                {
                                                    oLog.AddEvent(strName, strSerial, "QIP DNS Record = SUBNET ERROR: " + strDNS, LoggingType.Error);
                                                    strWarning = "The IP Address " + strIP + " for device " + strName + " was not successfully registered in QIP DNS (Subnet Does Not Exist)";
                                                    // This occurs if a subnet does not exist.  Will need to be routed for assignment and continue.
                                                    if (boolDNS_Bluecat == false)
                                                        oServer.UpdateDNS(intServer, -10, strDNS);
                                                    string strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DEVELOPER_WARNING");
                                                    oFunction.SendEmail("QIP DNS Automation Subnet Error", strEMailIdsBCC, "", "", "QIP DNS Automation Subnet Error", "<p>There was a SUBNET ERROR creating the QIP DNS Record for " + strName + " (SERVERID: " + intServer.ToString() + ") (ANSWERID: " + intAnswer.ToString() + ")</p><p>Error: " + strDNS + "</p><p>NOTE: A request should have been generated and assigned to a DNS technician.</p>", true, false);
                                                }
                                                else
                                                {
                                                    oLog.AddEvent(strName, strSerial, "QIP DNS Record = ERROR: " + strDNS, LoggingType.Error);
                                                    strWarning = "The IP Address " + strIP + " for device " + strName + " was not successfully registered in QIP DNS (" + strDNS + ")";
                                                    // An error was encountered...log the error
                                                    if (boolDNS_Bluecat == false)
                                                        oServer.UpdateDNS(intServer, -1, strDNS);
                                                    string strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DEVELOPER_ERROR,EMAILGRP_PROVISIONING_SUPPORT");
                                                    oFunction.SendEmail("QIP DNS Automation Error", strEMailIdsBCC, "", "", "QIP DNS Automation Error", "<p>There was an ERROR creating the QIP DNS Record for " + strName + " (SERVERID: " + intServer.ToString() + ") (ANSWERID: " + intAnswer.ToString() + ")</p><p>Error: " + strDNS + "</p><p>NOTE: Since this is an unexpected error, there was no task initiated to fix this problem. You must correct it yourself.</p>", true, false);
                                                }
                                                if (strWarning != "" && _bypass_warning == false)
                                                    break;
                                            }
                                        }
                                        if (strError == "" && (strWarning == "" || _bypass_warning == true))
                                        {
                                            if (boolDNS_Bluecat == true)
                                            {
                                                oLog.AddEvent(strName, strSerial, "Calling Web Service Function CreateBluecatDNS(" + strIP + ", " + strName + ", ) on " + oVariable.WebServiceURL(), LoggingType.Information);
                                                string strDNS = oWebService.CreateBluecatDNS(strIP, strName, strName, "");
                                                if (strDNS == "SUCCESS" || strDNS.StartsWith("***DUPLICATE") == true)
                                                {
                                                    oLog.AddEvent(strName, strSerial, "BlueCat DNS Record = SUCCESS", LoggingType.Information);
                                                    // The script ran successfully
                                                    oServer.UpdateDNS(intServer, 1, "Completed");
                                                }
                                                else
                                                {
                                                    if (strDNS.StartsWith("***CONFLICT") == true)
                                                    {
                                                        oLog.AddEvent(strName, strSerial, "BlueCat DNS Record = CONFLICT", LoggingType.Warning);
                                                        strError = "The IP Address " + strIP + " for device " + strName + " was not successfully registered in BlueCat DNS (Conflict)";
                                                        // A conflict occurred...awaiting the service technician to fix
                                                        oServer.UpdateDNS(intServer, 0, "Conflict");
                                                        string strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DEVELOPER_WARNING");
                                                        oFunction.SendEmail("BlueCat DNS Automation Conflict", strEMailIdsBCC, "", "", "BlueCat DNS Automation Conflict", "<p>There was a CONFLICT creating the BlueCat DNS Record for " + strName + " (SERVERID: " + intServer.ToString() + ") (ANSWERID: " + intAnswer.ToString() + ")</p><p>Error: " + strDNS + "</p><p>NOTE: A request should have been generated and assigned to a DNS technician.</p>", true, false);
                                                    }
                                                    else if (strDNS.ToUpper().Contains("SUBNET FOR") == true)
                                                    {
                                                        oLog.AddEvent(strName, strSerial, "BlueCat DNS Record = SUBNET ERROR: " + strDNS, LoggingType.Error);
                                                        strError = "The IP Address " + strIP + " for device " + strName + " was not successfully registered in BlueCat DNS (Subnet Does Not Exist)";
                                                        // This occurs if a subnet does not exist.  Will need to be routed for assignment and continue.
                                                        oServer.UpdateDNS(intServer, -10, strDNS);
                                                        string strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DEVELOPER_WARNING");
                                                        oFunction.SendEmail("BlueCat DNS Automation Subnet Error", strEMailIdsBCC, "", "", "BlueCat DNS Automation Subnet Error", "<p>There was a SUBNET ERROR creating the BlueCat DNS Record for " + strName + " (SERVERID: " + intServer.ToString() + ") (ANSWERID: " + intAnswer.ToString() + ")</p><p>Error: " + strDNS + "</p><p>NOTE: A request should have been generated and assigned to a DNS technician.</p>", true, false);
                                                    }
                                                    else
                                                    {
                                                        oLog.AddEvent(strName, strSerial, "BlueCat DNS Record = ERROR: " + strDNS, LoggingType.Error);
                                                        strError = "The IP Address " + strIP + " for device " + strName + " was not successfully registered in BlueCat DNS (" + strDNS + ")";
                                                        // An error was encountered...log the error
                                                        oServer.UpdateDNS(intServer, -1, strDNS);
                                                        string strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DEVELOPER_ERROR,EMAILGRP_PROVISIONING_SUPPORT");
                                                        oFunction.SendEmail("BlueCat DNS Automation Error", strEMailIdsBCC, "", "", "BlueCat DNS Automation Error", "<p>There was an ERROR creating the BlueCat DNS Record for " + strName + " (SERVERID: " + intServer.ToString() + ") (ANSWERID: " + intAnswer.ToString() + ")</p><p>Error: " + strDNS + "</p><p>NOTE: Since this is an unexpected error, there was no task initiated to fix this problem. You must correct it yourself.</p>", true, false);
                                                    }
                                                    break;
                                                }
                                            }
                                        }
                                    }


                                    if (strBackup != "" && strBackup != "N / A" && strDNSAuto != "1")
                                    {
                                        // Auto-Register in DNS
                                        if (boolDNS_QIP == true)
                                        {
                                            oLog.AddEvent(strName, strSerial, "Calling Web Service Function CreateDNSforPNC(" + strBackup + ", " + strName + "-backup, Server, , " + oVariable.DNS_Domain() + ", " + oVariable.DNS_NameService() + ", " + oVariable.DNS_DynamicDNSUpdate() + ", " + intProfile.ToString() + ", true) on " + oVariable.WebServiceURL(), LoggingType.Information);
                                            string strDNS = oWebService.CreateDNSforPNC(strBackup, strName + "-backup", "Server", "", oVariable.DNS_Domain(), oVariable.DNS_NameService(), oVariable.DNS_DynamicDNSUpdate(), intProfile, intAnswer, true);
                                            if (strDNS == "SUCCESS" || strDNS.StartsWith("***DUPLICATE") == true)
                                            {
                                                oLog.AddEvent(strName, strSerial, "QIP DNS Record = SUCCESS", LoggingType.Information);
                                                // The script ran successfully
                                                if (boolDNS_Bluecat == false)
                                                    oServer.UpdateDNS(intServer, 1, "Completed");
                                            }
                                            else
                                            {
                                                if (strDNS.StartsWith("***CONFLICT") == true)
                                                {
                                                    oLog.AddEvent(strName, strSerial, "QIP DNS Record = CONFLICT", LoggingType.Warning);
                                                    strWarning = "The IP Address " + strBackup + " for device " + strName + "-BACKUP was not successfully registered in QIP DNS (Conflict)";
                                                    // A conflict occurred...awaiting the service technician to fix
                                                    if (boolDNS_Bluecat == false)
                                                        oServer.UpdateDNS(intServer, 0, "Conflict");
                                                    string strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DEVELOPER_WARNING");
                                                    oFunction.SendEmail("QIP DNS Automation Conflict", strEMailIdsBCC, "", "", "QIP DNS Automation Conflict", "<p>There was a CONFLICT creating the QIP DNS Record for " + strName + "-BACKUP (SERVERID: " + intServer.ToString() + ") (ANSWERID: " + intAnswer.ToString() + ")</p><p>Error: " + strDNS + "</p><p>NOTE: A request should have been generated and assigned to a DNS technician.</p>", true, false);
                                                }
                                                else if (strDNS.ToUpper().Contains("SUBNET FOR") == true)
                                                {
                                                    oLog.AddEvent(strName, strSerial, "QIP DNS Record = SUBNET ERROR: " + strDNS, LoggingType.Error);
                                                    strWarning = "The IP Address " + strBackup + " for device " + strName + "-BACKUP was not successfully registered in QIP DNS (Subnet Does Not Exist)";
                                                    // This occurs if a subnet does not exist.  Will need to be routed for assignment and continue.
                                                    if (boolDNS_Bluecat == false)
                                                        oServer.UpdateDNS(intServer, -10, strDNS);
                                                    string strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DEVELOPER_WARNING");
                                                    oFunction.SendEmail("QIP DNS Automation Subnet Error", strEMailIdsBCC, "", "", "QIP DNS Automation Subnet Error", "<p>There was a SUBNET ERROR creating the QIP DNS Record for " + strName + "-BACKUP (SERVERID: " + intServer.ToString() + ") (ANSWERID: " + intAnswer.ToString() + ")</p><p>Error: " + strDNS + "</p><p>NOTE: A request should have been generated and assigned to a DNS technician.</p>", true, false);
                                                }
                                                else
                                                {
                                                    oLog.AddEvent(strName, strSerial, "QIP DNS Record = ERROR: " + strDNS, LoggingType.Error);
                                                    strWarning = "The IP Address " + strBackup + " for device " + strName + "-BACKUP was not successfully registered in QIP DNS (" + strDNS + ")";
                                                    // An error was encountered...log the error
                                                    if (boolDNS_Bluecat == false)
                                                        oServer.UpdateDNS(intServer, -1, strDNS);
                                                    string strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DEVELOPER_ERROR,EMAILGRP_PROVISIONING_SUPPORT");
                                                    oFunction.SendEmail("QIP DNS Automation Error", strEMailIdsBCC, "", "", "QIP DNS Automation Error", "<p>There was an ERROR creating the QIP DNS Record for " + strName + "-BACKUP (SERVERID: " + intServer.ToString() + ") (ANSWERID: " + intAnswer.ToString() + ")</p><p>Error: " + strDNS + "</p><p>NOTE: Since this is an unexpected error, there was no task initiated to fix this problem. You must correct it yourself.</p>", true, false);
                                                }
                                                if (strWarning != "" && _bypass_warning == false)
                                                    break;
                                            }
                                        }
                                        if (strError == "" && (strWarning == "" || _bypass_warning == true))
                                        {
                                            if (boolDNS_Bluecat == true)
                                            {
                                                oLog.AddEvent(strName, strSerial, "Calling Web Service Function CreateBluecatDNS(" + strBackup + ", " + strName + "-backup, ) on " + oVariable.WebServiceURL(), LoggingType.Information);
                                                string strDNS = oWebService.CreateBluecatDNS(strBackup, strName + "-backup", strName + "-backup", "");
                                                if (strDNS == "SUCCESS" || strDNS.StartsWith("***DUPLICATE") == true)
                                                {
                                                    oLog.AddEvent(strName, strSerial, "BlueCat DNS Record = SUCCESS", LoggingType.Information);
                                                    // The script ran successfully
                                                    oServer.UpdateDNS(intServer, 1, "Completed");
                                                }
                                                else
                                                {
                                                    if (strDNS.StartsWith("***CONFLICT") == true)
                                                    {
                                                        oLog.AddEvent(strName, strSerial, "BlueCat DNS Record = CONFLICT", LoggingType.Warning);
                                                        strError = "The IP Address " + strBackup + " for device " + strName + "-BACKUP was not successfully registered in BlueCat DNS (Conflict)";
                                                        // A conflict occurred...awaiting the service technician to fix
                                                        oServer.UpdateDNS(intServer, 0, "Conflict");
                                                        string strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DEVELOPER_WARNING");
                                                        oFunction.SendEmail("BlueCat DNS Automation Conflict", strEMailIdsBCC, "", "", "BlueCat DNS Automation Conflict", "<p>There was a CONFLICT creating the BlueCat DNS Record for " + strName + "-BACKUP (SERVERID: " + intServer.ToString() + ") (ANSWERID: " + intAnswer.ToString() + ")</p><p>Error: " + strDNS + "</p><p>NOTE: A request should have been generated and assigned to a DNS technician.</p>", true, false);
                                                    }
                                                    else if (strDNS.ToUpper().Contains("SUBNET FOR") == true)
                                                    {
                                                        oLog.AddEvent(strName, strSerial, "BlueCat DNS Record = SUBNET ERROR: " + strDNS, LoggingType.Error);
                                                        strError = "The IP Address " + strBackup + " for device " + strName + "-BACKUP was not successfully registered in BlueCat DNS (Subnet Does Not Exist)";
                                                        // This occurs if a subnet does not exist.  Will need to be routed for assignment and continue.
                                                        oServer.UpdateDNS(intServer, -10, strDNS);
                                                        string strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DEVELOPER_WARNING");
                                                        oFunction.SendEmail("BlueCat DNS Automation Subnet Error", strEMailIdsBCC, "", "", "BlueCat DNS Automation Subnet Error", "<p>There was a SUBNET ERROR creating the BlueCat DNS Record for " + strName + "-BACKUP (SERVERID: " + intServer.ToString() + ") (ANSWERID: " + intAnswer.ToString() + ")</p><p>Error: " + strDNS + "</p><p>NOTE: A request should have been generated and assigned to a DNS technician.</p>", true, false);
                                                    }
                                                    else
                                                    {
                                                        oLog.AddEvent(strName, strSerial, "BlueCat DNS Record = ERROR: " + strDNS, LoggingType.Error);
                                                        strError = "The IP Address " + strBackup + " for device " + strName + "-BACKUP was not successfully registered in BlueCat DNS (" + strDNS + ")";
                                                        // An error was encountered...log the error
                                                        oServer.UpdateDNS(intServer, -1, strDNS);
                                                        string strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DEVELOPER_ERROR,EMAILGRP_PROVISIONING_SUPPORT");
                                                        oFunction.SendEmail("BlueCat DNS Automation Error", strEMailIdsBCC, "", "", "BlueCat DNS Automation Error", "<p>There was an ERROR creating the BlueCat DNS Record for " + strName + "-BACKUP (SERVERID: " + intServer.ToString() + ") (ANSWERID: " + intAnswer.ToString() + ")</p><p>Error: " + strDNS + "</p><p>NOTE: Since this is an unexpected error, there was no task initiated to fix this problem. You must correct it yourself.</p>", true, false);
                                                    }
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                foreach (DataRow drManual in dsManual.Tables[0].Rows)
                                {
                                    int intServer = Int32.Parse(drManual["id"].ToString());
                                    string strName = drManual["servername"].ToString();
                                    string strSerial = drManual["serial"].ToString();
                                    oLog.AddEvent(strName, strSerial, "PNC DNS Record = SKIPPED", LoggingType.Information);
                                    // The script ran successfully
                                    oServer.UpdateDNS(intServer, 1, "Skipped");
                                }
                            }
                        }

                    }

                    bool boolRegisterBackup = false;

                    if (chk4.Enabled == true)
                    {
                        // Step # 3 : Generate Pre-Build Forms
                        if (oModelsProperties.IsSUNVirtual(intModel) == true)
                            boolRegisterBackup = true;

                        DataSet dsManual = oServer.GetManual(intAnswer, false);
                        foreach (DataRow drManual in dsManual.Tables[0].Rows)
                        {
                            int intServer = Int32.Parse(drManual["id"].ToString());
                            oServer.UpdateBuildStarted(intServer, DateTime.Now.ToString(), true);
                        }
                    }

                    if (chk5.Enabled == true)
                    {
                        // Step # 4 : Build the Server(s)
                        if (oModelsProperties.IsSUNVirtual(intModel) == false)
                            boolRegisterBackup = true;

                        DataSet dsManual = oServer.GetManual(intAnswer, false);

                        if (panWarning.Visible == false)
                        {
                            // Validate the assigned IPs are pingable
                            foreach (DataRow drManual in dsManual.Tables[0].Rows)
                            {
                                int intServer = Int32.Parse(drManual["id"].ToString());
                                string strName = drManual["servername"].ToString();
                                string strSerial = drManual["serial"].ToString();
                                string strIP = drManual["ipaddress1"].ToString();

                                if (strIP != "")
                                {
                                    // Ping the IP address
                                    Ping oPing = new Ping();
                                    string strPingReply = "";
                                    try
                                    {
                                        PingReply oReply = oPing.Send(strIP);
                                        strPingReply = oReply.Status.ToString().ToUpper();
                                    }
                                    catch { }
                                    if (strPingReply != "SUCCESS")
                                    {
                                        strWarning = "The IP Address " + strIP + " for device " + strName + " did not respond successfully to a ping request (" + strPingReply + ")";
                                        break;
                                    }
                                }
                            }
                        }

                        if (strError == "" && (strWarning == "" || _bypass_warning == true))
                        {
                            // Register in DNS and Finish Servers
                            if (intEnvironment == (int)CurrentEnvironment.CORPDMN || intEnvironment == (int)CurrentEnvironment.PNCNT_PROD)
                            {
                                System.Net.NetworkCredential oCredentialsDNS = new System.Net.NetworkCredential(oVariable.ADUser(), oVariable.ADPassword(), oVariable.Domain());
                                ClearViewWebServices oWebService = new ClearViewWebServices();
                                oWebService.Timeout = Timeout.Infinite;
                                oWebService.Credentials = oCredentialsDNS;
                                oWebService.Url = oVariable.WebServiceURL();
                                bool boolDNS_QIP = oSetting.IsDNS_QIP();
                                bool boolDNS_Bluecat = oSetting.IsDNS_Bluecat();
                                foreach (DataRow drManual in dsManual.Tables[0].Rows)
                                {
                                    int intServer = Int32.Parse(drManual["id"].ToString());
                                    string strName = drManual["servername"].ToString();
                                    string strSerial = drManual["serial"].ToString();
                                    string strIP = "";
                                    string strBackup = "";

                                    if (drManual["finalid1"].ToString() == "1")
                                        strIP = drManual["ipaddress1"].ToString();
                                    if (drManual["finalid1"].ToString() == "0")
                                        strBackup = drManual["ipaddress1"].ToString();

                                    if (drManual["finalid2"].ToString() == "1" && strIP == "")
                                        strIP = drManual["ipaddress2"].ToString();
                                    if (drManual["finalid2"].ToString() == "0" && strBackup == "")
                                        strBackup = drManual["ipaddress2"].ToString();

                                    if (drManual["finalid3"].ToString() == "1" && strIP == "")
                                        strIP = drManual["ipaddress3"].ToString();
                                    if (drManual["finalid3"].ToString() == "0" && strBackup == "")
                                        strBackup = drManual["ipaddress3"].ToString();

                                    string strDNSAuto = oServer.Get(intServer, "dns_auto");

                                    if (strIP != "" && strDNSAuto != "1")
                                    {
                                        // Auto-Register in DNS
                                        if (boolDNS_QIP == true)
                                        {
                                            oLog.AddEvent(strName, strSerial, "Calling Web Service Function CreateDNSforPNC(" + strIP + ", " + strName + ", Server, , " + oVariable.DNS_Domain() + ", " + oVariable.DNS_NameService() + ", " + oVariable.DNS_DynamicDNSUpdate() + ", " + intProfile.ToString() + ", true) on " + oVariable.WebServiceURL(), LoggingType.Information);
                                            string strDNS = oWebService.CreateDNSforPNC(strIP, strName, "Server", "", oVariable.DNS_Domain(), oVariable.DNS_NameService(), oVariable.DNS_DynamicDNSUpdate(), intProfile, intAnswer, true);
                                            if (strDNS == "SUCCESS" || strDNS.StartsWith("***DUPLICATE") == true)
                                            {
                                                oLog.AddEvent(strName, strSerial, "QIP DNS Record = SUCCESS", LoggingType.Information);
                                                // The script ran successfully
                                                if (boolDNS_Bluecat == false)
                                                {
                                                    oServer.UpdateDNS(intServer, 1, "Completed");
                                                    // Set the build dates
                                                    oServer.UpdateBuildCompleted(intServer, DateTime.Now.ToString());
                                                }
                                            }
                                            else
                                            {
                                                if (strDNS.StartsWith("***CONFLICT") == true)
                                                {
                                                    oLog.AddEvent(strName, strSerial, "QIP DNS Record = CONFLICT", LoggingType.Warning);
                                                    strWarning = "The IP Address " + strIP + " for device " + strName + " was not successfully registered in QIP (Conflict)";
                                                    // A conflict occurred...awaiting the service technician to fix
                                                    if (boolDNS_Bluecat == false)
                                                        oServer.UpdateDNS(intServer, 0, "Conflict");
                                                    string strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DEVELOPER_WARNING");
                                                    oFunction.SendEmail("PNC DNS Automation Conflict", strEMailIdsBCC, "", "", "QIP DNS Automation Conflict", "<p>There was a CONFLICT creating the QIP DNS Record for " + strName + " (SERVERID: " + intServer.ToString() + ") (ANSWERID: " + intAnswer.ToString() + ")</p><p>Error: " + strDNS + "</p><p>NOTE: A request should have been generated and assigned to a DNS technician.</p>", true, false);
                                                }
                                                else if (strDNS.ToUpper().StartsWith("***ERROR: SUBNET FOR") == true)
                                                {
                                                    oLog.AddEvent(strName, strSerial, "QIP DNS Record = SUBNET ERROR: " + strDNS, LoggingType.Error);
                                                    strWarning = "The IP Address " + strIP + " for device " + strName + " was not successfully registered in QIP (Subnet Does Not Exist)";
                                                    // This occurs if a subnet does not exist.  Will need to be routed for assignment and continue.
                                                    if (boolDNS_Bluecat == false)
                                                        oServer.UpdateDNS(intServer, -10, strDNS);
                                                    string strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DEVELOPER_WARNING");
                                                    oFunction.SendEmail("PNC DNS Automation Subnet Error", strEMailIdsBCC, "", "", "QIP DNS Automation Subnet Error", "<p>There was a SUBNET ERROR creating the QIP DNS Record for " + strName + " (SERVERID: " + intServer.ToString() + ") (ANSWERID: " + intAnswer.ToString() + ")</p><p>Error: " + strDNS + "</p><p>NOTE: A request should have been generated and assigned to a DNS technician.</p>", true, false);
                                                }
                                                else
                                                {
                                                    oLog.AddEvent(strName, strSerial, "QIP DNS Record = ERROR: " + strDNS, LoggingType.Error);
                                                    strWarning = "The IP Address " + strIP + " for device " + strName + " was not successfully registered in QIP (" + strDNS + ")";
                                                    // An error was encountered...log the error
                                                    if (boolDNS_Bluecat == false)
                                                        oServer.UpdateDNS(intServer, -1, strDNS);
                                                    string strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DEVELOPER_ERROR,EMAILGRP_PROVISIONING_SUPPORT");
                                                    oFunction.SendEmail("PNC DNS Automation Error", strEMailIdsBCC, "", "", "QIP DNS Automation Error", "<p>There was an ERROR creating the QIP DNS Record for " + strName + " (SERVERID: " + intServer.ToString() + ") (ANSWERID: " + intAnswer.ToString() + ")</p><p>Error: " + strDNS + "</p><p>NOTE: Since this is an unexpected error, there was no task initiated to fix this problem. You must correct it yourself.</p>", true, false);
                                                }
                                                if (strWarning != "" && _bypass_warning == false)
                                                    break;
                                            }
                                        }
                                        if (strError == "" && (strWarning == "" || _bypass_warning == true))
                                        {
                                            if (boolDNS_Bluecat == true)
                                            {
                                                oLog.AddEvent(strName, strSerial, "Calling Web Service Function CreateBluecatDNS(" + strIP + ", " + strName + ", ) on " + oVariable.WebServiceURL(), LoggingType.Information);
                                                string strDNS = oWebService.CreateBluecatDNS(strIP, strName, strName, "");
                                                if (strDNS == "SUCCESS" || strDNS.StartsWith("***DUPLICATE") == true)
                                                {
                                                    oLog.AddEvent(strName, strSerial, "BlueCat DNS Record = SUCCESS", LoggingType.Information);
                                                    // The script ran successfully
                                                    oServer.UpdateDNS(intServer, 1, "Completed");
                                                    // Set the build dates
                                                    oServer.UpdateBuildCompleted(intServer, DateTime.Now.ToString());
                                                }
                                                else
                                                {
                                                    if (strDNS.StartsWith("***CONFLICT") == true)
                                                    {
                                                        oLog.AddEvent(strName, strSerial, "BlueCat DNS Record = CONFLICT", LoggingType.Warning);
                                                        strError = "The IP Address " + strIP + " for device " + strName + " was not successfully registered in BlueCat DNS (Conflict)";
                                                        // A conflict occurred...awaiting the service technician to fix
                                                        oServer.UpdateDNS(intServer, 0, "Conflict");
                                                        string strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DEVELOPER_WARNING");
                                                        oFunction.SendEmail("BlueCat DNS Automation Conflict", strEMailIdsBCC, "", "", "BlueCat DNS Automation Conflict", "<p>There was a CONFLICT creating the BlueCat DNS Record for " + strName + " (SERVERID: " + intServer.ToString() + ") (ANSWERID: " + intAnswer.ToString() + ")</p><p>Error: " + strDNS + "</p><p>NOTE: A request should have been generated and assigned to a DNS technician.</p>", true, false);
                                                    }
                                                    else if (strDNS.ToUpper().Contains("SUBNET FOR") == true)
                                                    {
                                                        oLog.AddEvent(strName, strSerial, "BlueCat DNS Record = SUBNET ERROR: " + strDNS, LoggingType.Error);
                                                        strError = "The IP Address " + strIP + " for device " + strName + " was not successfully registered in BlueCat DNS (Subnet Does Not Exist)";
                                                        // This occurs if a subnet does not exist.  Will need to be routed for assignment and continue.
                                                        oServer.UpdateDNS(intServer, -10, strDNS);
                                                        string strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DEVELOPER_WARNING");
                                                        oFunction.SendEmail("BlueCat DNS Automation Subnet Error", strEMailIdsBCC, "", "", "BlueCat DNS Automation Subnet Error", "<p>There was a SUBNET ERROR creating the BlueCat DNS Record for " + strName + " (SERVERID: " + intServer.ToString() + ") (ANSWERID: " + intAnswer.ToString() + ")</p><p>Error: " + strDNS + "</p><p>NOTE: A request should have been generated and assigned to a DNS technician.</p>", true, false);
                                                    }
                                                    else
                                                    {
                                                        oLog.AddEvent(strName, strSerial, "BlueCat DNS Record = ERROR: " + strDNS, LoggingType.Error);
                                                        strError = "The IP Address " + strIP + " for device " + strName + " was not successfully registered in BlueCat DNS (" + strDNS + ")";
                                                        // An error was encountered...log the error
                                                        oServer.UpdateDNS(intServer, -1, strDNS);
                                                        string strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DEVELOPER_ERROR,EMAILGRP_PROVISIONING_SUPPORT");
                                                        oFunction.SendEmail("BlueCat DNS Automation Error", strEMailIdsBCC, "", "", "BlueCat DNS Automation Error", "<p>There was an ERROR creating the BlueCat DNS Record for " + strName + " (SERVERID: " + intServer.ToString() + ") (ANSWERID: " + intAnswer.ToString() + ")</p><p>Error: " + strDNS + "</p><p>NOTE: Since this is an unexpected error, there was no task initiated to fix this problem. You must correct it yourself.</p>", true, false);
                                                    }
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                    else if (oServer.Get(intServer, "build_completed") == "")
                                        oServer.UpdateBuildCompleted(intServer, DateTime.Now.ToString());



                                    if (strBackup != "" && strBackup != "N / A" && strDNSAuto != "1")
                                    {
                                        // Auto-Register in DNS
                                        if (boolDNS_QIP == true)
                                        {
                                            oLog.AddEvent(strName, strSerial, "Calling Web Service Function CreateDNSforPNC(" + strBackup + ", " + strName + "-backup, Server, , " + oVariable.DNS_Domain() + ", " + oVariable.DNS_NameService() + ", " + oVariable.DNS_DynamicDNSUpdate() + ", " + intProfile.ToString() + ", true) on " + oVariable.WebServiceURL(), LoggingType.Information);
                                            string strDNS = oWebService.CreateDNSforPNC(strBackup, strName + "-backup", "Server", "", oVariable.DNS_Domain(), oVariable.DNS_NameService(), oVariable.DNS_DynamicDNSUpdate(), intProfile, intAnswer, true);
                                            if (strDNS == "SUCCESS" || strDNS.StartsWith("***DUPLICATE") == true)
                                            {
                                                oLog.AddEvent(strName, strSerial, "QIP DNS Record = SUCCESS", LoggingType.Information);
                                                // The script ran successfully
                                                if (boolDNS_Bluecat == false)
                                                    oServer.UpdateDNS(intServer, 1, "Completed");
                                            }
                                            else
                                            {
                                                if (strDNS.StartsWith("***CONFLICT") == true)
                                                {
                                                    oLog.AddEvent(strName, strSerial, "QIP DNS Record = CONFLICT", LoggingType.Warning);
                                                    strWarning = "The IP Address " + strBackup + " for device " + strName + "-BACKUP was not successfully registered in QIP DNS (Conflict)";
                                                    // A conflict occurred...awaiting the service technician to fix
                                                    if (boolDNS_Bluecat == false)
                                                        oServer.UpdateDNS(intServer, 0, "Conflict");
                                                    string strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DEVELOPER_WARNING");
                                                    oFunction.SendEmail("QIP DNS Automation Conflict", strEMailIdsBCC, "", "", "QIP DNS Automation Conflict", "<p>There was a CONFLICT creating the QIP DNS Record for " + strName + "-BACKUP (SERVERID: " + intServer.ToString() + ") (ANSWERID: " + intAnswer.ToString() + ")</p><p>Error: " + strDNS + "</p><p>NOTE: A request should have been generated and assigned to a DNS technician.</p>", true, false);
                                                }
                                                else if (strDNS.ToUpper().Contains("SUBNET FOR") == true)
                                                {
                                                    oLog.AddEvent(strName, strSerial, "QIP DNS Record = SUBNET ERROR: " + strDNS, LoggingType.Error);
                                                    strWarning = "The IP Address " + strBackup + " for device " + strName + "-BACKUP was not successfully registered in QIP DNS (Subnet Does Not Exist)";
                                                    // This occurs if a subnet does not exist.  Will need to be routed for assignment and continue.
                                                    if (boolDNS_Bluecat == false)
                                                        oServer.UpdateDNS(intServer, -10, strDNS);
                                                    string strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DEVELOPER_WARNING");
                                                    oFunction.SendEmail("QIP DNS Automation Subnet Error", strEMailIdsBCC, "", "", "QIP DNS Automation Subnet Error", "<p>There was a SUBNET ERROR creating the QIP DNS Record for " + strName + "-BACKUP (SERVERID: " + intServer.ToString() + ") (ANSWERID: " + intAnswer.ToString() + ")</p><p>Error: " + strDNS + "</p><p>NOTE: A request should have been generated and assigned to a DNS technician.</p>", true, false);
                                                }
                                                else
                                                {
                                                    oLog.AddEvent(strName, strSerial, "QIP DNS Record = ERROR: " + strDNS, LoggingType.Error);
                                                    strWarning = "The IP Address " + strBackup + " for device " + strName + "-BACKUP was not successfully registered in QIP DNS (" + strDNS + ")";
                                                    // An error was encountered...log the error
                                                    if (boolDNS_Bluecat == false)
                                                        oServer.UpdateDNS(intServer, -1, strDNS);
                                                    string strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DEVELOPER_ERROR,EMAILGRP_PROVISIONING_SUPPORT");
                                                    oFunction.SendEmail("QIP DNS Automation Error", strEMailIdsBCC, "", "", "QIP DNS Automation Error", "<p>There was an ERROR creating the QIP DNS Record for " + strName + "-BACKUP (SERVERID: " + intServer.ToString() + ") (ANSWERID: " + intAnswer.ToString() + ")</p><p>Error: " + strDNS + "</p><p>NOTE: Since this is an unexpected error, there was no task initiated to fix this problem. You must correct it yourself.</p>", true, false);
                                                }
                                                if (strWarning != "" && _bypass_warning == false)
                                                    break;
                                            }
                                        }
                                        if (strError == "" && (strWarning == "" || _bypass_warning == true))
                                        {
                                            if (boolDNS_Bluecat == true)
                                            {
                                                oLog.AddEvent(strName, strSerial, "Calling Web Service Function CreateBluecatDNS(" + strBackup + ", " + strName + "-backup, ) on " + oVariable.WebServiceURL(), LoggingType.Information);
                                                string strDNS = oWebService.CreateBluecatDNS(strBackup, strName + "-backup", strName + "-backup", "");
                                                if (strDNS == "SUCCESS" || strDNS.StartsWith("***DUPLICATE") == true)
                                                {
                                                    oLog.AddEvent(strName, strSerial, "BlueCat DNS Record = SUCCESS", LoggingType.Information);
                                                    // The script ran successfully
                                                    oServer.UpdateDNS(intServer, 1, "Completed");
                                                }
                                                else
                                                {
                                                    if (strDNS.StartsWith("***CONFLICT") == true)
                                                    {
                                                        oLog.AddEvent(strName, strSerial, "BlueCat DNS Record = CONFLICT", LoggingType.Warning);
                                                        strError = "The IP Address " + strBackup + " for device " + strName + "-BACKUP was not successfully registered in BlueCat DNS (Conflict)";
                                                        // A conflict occurred...awaiting the service technician to fix
                                                        oServer.UpdateDNS(intServer, 0, "Conflict");
                                                        string strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DEVELOPER_WARNING");
                                                        oFunction.SendEmail("BlueCat DNS Automation Conflict", strEMailIdsBCC, "", "", "BlueCat DNS Automation Conflict", "<p>There was a CONFLICT creating the BlueCat DNS Record for " + strName + "-BACKUP (SERVERID: " + intServer.ToString() + ") (ANSWERID: " + intAnswer.ToString() + ")</p><p>Error: " + strDNS + "</p><p>NOTE: A request should have been generated and assigned to a DNS technician.</p>", true, false);
                                                    }
                                                    else if (strDNS.ToUpper().Contains("SUBNET FOR") == true)
                                                    {
                                                        oLog.AddEvent(strName, strSerial, "BlueCat DNS Record = SUBNET ERROR: " + strDNS, LoggingType.Error);
                                                        strError = "The IP Address " + strBackup + " for device " + strName + "-BACKUP was not successfully registered in BlueCat DNS (Subnet Does Not Exist)";
                                                        // This occurs if a subnet does not exist.  Will need to be routed for assignment and continue.
                                                        oServer.UpdateDNS(intServer, -10, strDNS);
                                                        string strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DEVELOPER_WARNING");
                                                        oFunction.SendEmail("BlueCat DNS Automation Subnet Error", strEMailIdsBCC, "", "", "BlueCat DNS Automation Subnet Error", "<p>There was a SUBNET ERROR creating the BlueCat DNS Record for " + strName + "-BACKUP (SERVERID: " + intServer.ToString() + ") (ANSWERID: " + intAnswer.ToString() + ")</p><p>Error: " + strDNS + "</p><p>NOTE: A request should have been generated and assigned to a DNS technician.</p>", true, false);
                                                    }
                                                    else
                                                    {
                                                        oLog.AddEvent(strName, strSerial, "BlueCat DNS Record = ERROR: " + strDNS, LoggingType.Error);
                                                        strError = "The IP Address " + strBackup + " for device " + strName + "-BACKUP was not successfully registered in BlueCat DNS (" + strDNS + ")";
                                                        // An error was encountered...log the error
                                                        oServer.UpdateDNS(intServer, -1, strDNS);
                                                        string strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DEVELOPER_ERROR,EMAILGRP_PROVISIONING_SUPPORT");
                                                        oFunction.SendEmail("BlueCat DNS Automation Error", strEMailIdsBCC, "", "", "BlueCat DNS Automation Error", "<p>There was an ERROR creating the BlueCat DNS Record for " + strName + "-BACKUP (SERVERID: " + intServer.ToString() + ") (ANSWERID: " + intAnswer.ToString() + ")</p><p>Error: " + strDNS + "</p><p>NOTE: Since this is an unexpected error, there was no task initiated to fix this problem. You must correct it yourself.</p>", true, false);
                                                    }
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                foreach (DataRow drManual in dsManual.Tables[0].Rows)
                                {
                                    int intServer = Int32.Parse(drManual["id"].ToString());
                                    string strName = drManual["servername"].ToString();
                                    string strSerial = drManual["serial"].ToString();
                                    oLog.AddEvent(strName, strSerial, "PNC DNS Record = SKIPPED", LoggingType.Information);
                                    // The script ran successfully
                                    oServer.UpdateDNS(intServer, 1, "Skipped");
                                    // Set the build dates
                                    oServer.UpdateBuildCompleted(intServer, DateTime.Now.ToString());
                                }
                            }
                        }

                        if (strError == "" && (strWarning == "" || _bypass_warning == true))
                        {
                            // Update Forcast
                            oForecast.UpdateAnswerCompleted(intAnswer);
                            // Initiate Pre-Prod Tasks
                            oPNCTask.InitiateNextStep(intRequest, intAnswer, intModel, intEnvironment, intAssignPage, intViewPage, dsnAsset, dsnIP, dsnServiceEditor);
                        }
                        else
                            oLog.AddEvent(intAnswer, "", "", "Design has been manually built (Error: " + strError + ", Warning: " + strWarning + ")", LoggingType.Debug);
                    }

                    if (boolRegisterBackup == true)
                    {
                        int intTSMAutomated = 0;
                        Int32.TryParse(oSetting.Get("tsm_automated"), out intTSMAutomated);
                        int intAvamarAutomated = 0;
                        Int32.TryParse(oSetting.Get("avamar_automated"), out intAvamarAutomated);
                        int intRecovery = 0;
                        DataSet dsBackup = oForecast.GetBackup(intAnswer);
                        if (dsBackup.Tables[0].Rows.Count > 0)
                            Int32.TryParse(dsBackup.Tables[0].Rows[0]["recoveryid"].ToString(), out intRecovery);
                        int intTSM = 0;
                        //Int32.TryParse(oLocation.GetAddress(intRecovery, "tsm"), out intTSM);
                        Int32.TryParse(oLocation.GetAddress(intAddress, "tsm"), out intTSM);
                        int intAvamar = 0;
                        Int32.TryParse(oForecast.GetAnswer(intAnswer, "avamar"), out intAvamar);

                        DataSet dsManual = oServer.GetManual(intAnswer, false);
                        bool boolContainerDone = false;
                        foreach (DataRow drManual in dsManual.Tables[0].Rows)
                        {
                            int intServer = Int32.Parse(drManual["id"].ToString());
                            string strName = drManual["servername"].ToString();
                            string strSerial = drManual["serial"].ToString();

                            if ((intTSM == 1 && intTSMAutomated == 1) || (intAvamar == 1 && intAvamarAutomated == 1))
                            {
                                if (boolContainerDone == false)
                                {
                                    oLog.AddEvent(strName, strSerial, "Setting TSM output PENDING status and Date " + ((intTSM == 1 && intTSMAutomated == 1) ? "TSM" : ((intAvamar == 1 && intAvamarAutomated == 1) ? "AVAMAR" : "Unknown")) + " ~ Generic", LoggingType.Debug);
                                    // Update the TSM table to enable automated TSM of TDPO
                                    oServer.UpdateTSM(intServer, "PENDING");
                                    // Check to see if this is the "A" side of the container.
                                    if (oModelsProperties.IsSUNVirtual(intModel) == true && strSVEType == "DB" && strName.EndsWith("A") == true)
                                        boolContainerDone = true;

                                }
                                else
                                    oServer.UpdateTSM(intServer, "Completed");
                                oServer.UpdateTSMRegistered(intServer, DateTime.Now.ToString());
                            }
                            else
                                oServer.UpdateTSM(intServer, "");
                        }
                    }

                    if (chk6.Enabled == true)
                    {
                        // Step # 5 : Generate Post-Build Forms
                        if (panMove.Visible == false)
                        {
                            // Complete and Close
                        }
                    }

                    if (panMove.Visible == true)
                    {
                        if (chk7.Enabled == true)
                        {
                            // Step # 6 : Reserve Assets (for Final Location)
                            oOnDemandTasks.UpdateGenericIIProd(intRequest, intItem, intNumber);
                        }
                        if (chk8.Enabled == true)
                        {
                            // Step # 7 : Generate Forms (for Final Location)
                        }
                        if (chk9.Enabled == true)
                        {
                            // Step # 8 : Release Build Assets (for Final Location)
                        }
                    }
                }
            }

            // Save Information
            oResourceRequest.UpdateWorkflowName(intResourceWorkflow, txtCustom.Text, intProfile);
            if (strError == "" && (strWarning == "" || _bypass_warning == true))
            {
                double dblHours = 0.00;
                dblHours += (chk1.Checked || chkAdmin.Checked ? dbl1 : 0.00);
                dblHours += (chk3.Checked || chkAdmin.Checked ? dbl3 : 0.00);
                dblHours += (chk4.Checked || chkAdmin.Checked ? dbl4 : 0.00);
                dblHours += (chk5.Checked || chkAdmin.Checked ? dbl5 : 0.00);
                dblHours += (chk6.Checked || chkAdmin.Checked ? dbl6 : 0.00);
                dblHours += (chk7.Checked || chkAdmin.Checked || panMove.Visible == false ? dbl7 : 0.00);
                dblHours += (chk8.Checked || chkAdmin.Checked || panMove.Visible == false ? dbl8 : 0.00);
                dblHours += (chk9.Checked || chkAdmin.Checked || panMove.Visible == false ? dbl9 : 0.00); ;
                dblHours += dbl10;
                dblHours += dbl11;
                double dblUsed = oResourceRequest.GetWorkflowUsed(intResourceWorkflow);
                dblHours = (dblHours - dblUsed);
                if (dblHours > 0.00)
                    oResourceRequest.UpdateWorkflowHours(intResourceWorkflow, dblHours);
                oOnDemandTasks.UpdateGenericII(intRequest, intItem, intNumber, SaveCheck(chk1, chkAdmin, false), SaveCheck(chk3, chkAdmin, false), SaveCheck(chk4, chkAdmin, false), SaveCheck(chk5, chkAdmin, false), SaveCheck(chk6, chkAdmin, false), SaveCheck(chk7, chkAdmin, (panMove.Visible == false)), SaveCheck(chk8, chkAdmin, (panMove.Visible == false)), SaveCheck(chk9, chkAdmin, (panMove.Visible == false)), DateTime.Now.ToString(), DateTime.Now.ToString(), DateTime.Now.ToString(), DateTime.Now.ToString(), DateTime.Now.ToString(), DateTime.Now.ToString());
                Response.Redirect(Request.Path + "?rrid=" + intResourceWorkflow.ToString() + "&div=E&save=true");
            }
            else if (strError == "")
            {
                Response.Redirect(Request.Path + "?rrid=" + intResourceWorkflow.ToString() + "&div=E&warning=" + oFunction.encryptQueryString(strWarning));
            }
            else
            {
                Response.Redirect(Request.Path + "?rrid=" + intResourceWorkflow.ToString() + "&div=E&error=" + oFunction.encryptQueryString(strError));
            }
        }
        private string SaveCheck(CheckBox chkStep, CheckBox chkA, bool boolMove)
        {
            if (chkStep.Checked || chkStep.Enabled || chkA.Checked || boolMove)
            {
                if (chkStep.ToolTip == "")
                    return DateTime.Now.ToString();
                else
                    return chkStep.ToolTip;
            }
            else
                return "";
        }

        protected void btnComplete_Click(Object Sender, EventArgs e)
        {
            int intResourceWorkflow = Int32.Parse(lblResourceWorkflow.Text);
            int intResourceParent = oResourceRequest.GetWorkflowParent(intResourceWorkflow);
            oResourceRequest.UpdateWorkflowStatus(intResourceWorkflow, 3, true);
            oOnDemandTasks.UpdateGenericIIComplete(intRequest, intItem, intNumber);
            //Forecast oForecast = new Forecast(intProfile, dsn);
            //oForecast.UpdateAnswerFinished(Int32.Parse(lblAnswer.Text));
            oResourceRequest.CloseWorkflow(intResourceWorkflow, intEnvironment, 0, dsnServiceEditor, true, intResourceRequestApprove, intAssignPage, intViewPage, dsnAsset, dsnIP);
            Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "reload", "<script type=\"text/javascript\">RefreshOpeningWindow();window.close();<" + "/" + "script>");
        }


        private string AddIPs(int _serverid, int _answerid, int _additionalClusterID, string _form, int _modelid, string _name)
        {
            string strIP1 = Request.Form[_form + "1"].Trim().ToUpper();
            string strIP2 = Request.Form[_form + "2"].Trim().ToUpper();
            string strIP3 = Request.Form[_form + "3"].Trim().ToUpper();
            string strIP4 = Request.Form[_form + "4"].Trim().ToUpper();
            return AddIPs(_serverid, _answerid, _additionalClusterID, strIP1, strIP2, strIP3, strIP4, _modelid, _name);
        }
        private string AddIPs(int _serverid, int _answerid, int _additionalClusterID, string strIP1, string strIP2, string strIP3, string strIP4, int _modelid, string _name)
        {
            string strIP = strIP1 + "." + strIP2 + "." + strIP3 + "." + strIP4;
            if (strIP1 != "" && strIP2 != "" && strIP3 != "" && strIP4 != "")
            {
                int intIP1 = 0;
                int intIP2 = 0;
                int intIP3 = 0;
                int intIP4 = 0;
                Int32.TryParse(strIP1, out intIP1);
                Int32.TryParse(strIP2, out intIP2);
                Int32.TryParse(strIP3, out intIP3);
                Int32.TryParse(strIP4, out intIP4);

                if (intIP1 > 0 && intIP1 < 255 && intIP2 > 0 && intIP2 < 255 && intIP3 > 0 && intIP3 < 255 && intIP4 > 0 && intIP4 < 255)
                {
                    bool boolOK = true;
                    DataSet dsIP = oIPAddresses.Get(intIP1, intIP2, intIP3, intIP4);
                    foreach (DataRow drIP in dsIP.Tables[0].Rows)
                    {
                        if (drIP["available"].ToString() == "0")
                        {
                            boolOK = false;
                            break;
                        }
                    }
                    if (boolOK == true)
                    {
                        int intIP = oIPAddresses.Add(0, intIP1, intIP2, intIP3, intIP4, intProfile);
                        if (_serverid > 0)
                            oServer.AddIP(_serverid, intIP, 0, 1, 0, 0);
                        else
                            oIPAddresses.AddRelated(_answerid, _additionalClusterID, intIP);
                        return "";
                    }
                    else
                        return "The IP Address " + strIP + " is already reserved or is currently in use";
                }
                else
                    return "Invalid IP Address";
            }
            else
                return "";
        }
        
        private string GetStorage(int intAnswer, int intClass, int intCluster2, int intCSMConfig2, int intNumber2, int intModel)
        {
            DataSet dsLuns = new DataSet();
            if (intCluster2 == 0)
                dsLuns = oStorage.GetLuns(intAnswer, 0, intCluster2, intCSMConfig2, intNumber2);
            else
                dsLuns = oStorage.GetLunsClusterNonShared(intAnswer, intCluster2, intNumber2);
            bool boolOverride = (oForecast.GetAnswer(intAnswer, "storage_override") == "1");
            StringBuilder sbStorage = new StringBuilder(AddStorage(dsLuns, intModel, intClass, boolOverride));
            if (sbStorage.ToString() != "")
            {
                // Header
                sbStorage.Insert(0, "</tr>");
                if (oClass.IsProd(intClass))
                    sbStorage.Insert(0, "<td>Requested</td><td>Actual</td><td>Requested</td><td>Actual</td>");
                sbStorage.Insert(0, "<td>Requested</td><td>Actual</td>");
                sbStorage.Insert(0, "<tr class=\"bold\"><td></td><td>Path</td><td>Performance</td><td>Serial</td>");
                // Background
                sbStorage.Insert(0, "</tr>");
                if (oClass.IsProd(intClass))
                    sbStorage.Insert(0, "<td colspan=\"2\" class=\"framegreen\">Replication</td><td colspan=\"2\" class=\"framegreen\">High Availability</td>");
                sbStorage.Insert(0, "<td colspan=\"2\" class=\"framegreen\">Size</td>");
                sbStorage.Insert(0, "<tr><td colspan=\"4\">&nbsp;</td>");
                // Actual Storage
                //sbStorage.Insert(0, "<table id=\"tblStorage" + intCluster2.ToString() + "_" + intNumber2.ToString() + "\" style=\"display:none\" width=\"100%\" cellpadding=\"2\" cellspacing=\"1\" border=\"0\">");
                sbStorage.Insert(0, "<table id=\"tblStorage" + intCluster2.ToString() + "_" + intNumber2.ToString() + "\" style=\"display:inline\" width=\"100%\" cellpadding=\"2\" cellspacing=\"1\" border=\"0\">");
                sbStorage.Append("</table>");
                //sbStorage.Insert(0, "<a href=\"javascript:void(0);\" onclick=\"ShowHideDiv2('tblStorage" + intCluster2.ToString() + "_" + intNumber2.ToString() + "');\">Show Storage Settings for " + strName + "</a><br/>");
            }
            return sbStorage.ToString();
        }
        private string GetStorageShared(int intAnswer, int intClass, int intModel)
        {
            string strClusterNames = "";
            StringBuilder sbStorage = new StringBuilder();
            int intClusterOLD = 0;
            bool boolOverride = (oForecast.GetAnswer(intAnswer, "storage_override") == "1");
            DataSet dsCluster = oServer.GetAnswerClusters(intAnswer);
            foreach (DataRow drCluster in dsCluster.Tables[0].Rows)
            {
                int intClusterID = Int32.Parse(drCluster["clusterid"].ToString());
                if (intClusterOLD != intClusterID)
                {
                    if (intClusterID > 0)
                    {
                        DataSet dsServers = oServer.GetClusters(intClusterID);
                        foreach (DataRow drServer in dsServers.Tables[0].Rows)
                        {
                            int intServer = Int32.Parse(drServer["id"].ToString());
                            if (strClusterNames != "")
                                strClusterNames += ", ";
                            strClusterNames += oServer.GetName(intServer, true);
                        }
                        DataSet dsLuns = oStorage.GetLunsClusterShared(intAnswer, intClusterID);
                        sbStorage.Append(AddStorage(dsLuns, intModel, intClass, boolOverride));
                    }
                }
                intClusterOLD = intClusterID;
            }
            if (sbStorage.ToString() != "")
            {
                // Header
                sbStorage.Insert(0, "</tr>");
                if (oClass.IsProd(intClass))
                    sbStorage.Insert(0, "<td>Requested</td><td>Actual</td><td>Requested</td><td>Actual</td>");
                sbStorage.Insert(0, "<td>Requested</td><td>Actual</td>");
                sbStorage.Insert(0, "<tr class=\"bold\"><td></td><td>Path</td><td>Performance</td><td>Serial</td>");
                // Background
                sbStorage.Insert(0, "</tr>");
                if (oClass.IsProd(intClass))
                    sbStorage.Insert(0, "<td colspan=\"2\" class=\"framegreen\">Replication</td><td colspan=\"2\" class=\"framegreen\">High Availability</td>");
                sbStorage.Insert(0, "<td colspan=\"2\" class=\"framegreen\">Size</td>");
                sbStorage.Insert(0, "<tr><td colspan=\"4\">&nbsp;</td>");
                // Actual Storage
                sbStorage.Insert(0, "<table id=\"tblStorage" + intClusterOLD.ToString() + "\" style=\"display:none\" width=\"100%\" cellpadding=\"2\" cellspacing=\"1\" border=\"0\">");
                sbStorage.Append("</table>");
                sbStorage.Insert(0, "<br/><a href=\"javascript:void(0);\" onclick=\"ShowHideDiv2('tblStorage" + intClusterOLD.ToString() + "');\" class=\"redlink\">Show Shared Storage for " + strClusterNames + "</a><br/>");
            }
            return sbStorage.ToString();
        }
        private string AddStorage(DataSet dsLuns, int intModel, int intClass, bool boolOverride)
        {
            StringBuilder sbStorage = new StringBuilder();
            int intRow = 0;
            int intAnswer = Int32.Parse(lblAnswer.Text);
            foreach (DataRow drLun in dsLuns.Tables[0].Rows)
            {
                intRow++;
                sbStorage.Append("<tr>");
                sbStorage.Append("<td>");
                sbStorage.Append(intRow.ToString());
                sbStorage.Append("</td>");
                string strLetter = drLun["letter"].ToString();
                if (strLetter == "")
                {
                    if (drLun["driveid"].ToString() == "-1000")
                        strLetter = "E";
                    else if (drLun["driveid"].ToString() == "-100")
                        strLetter = "F";
                    else if (drLun["driveid"].ToString() == "-10")
                        strLetter = "P";
                    else if (drLun["driveid"].ToString() == "-1")
                        strLetter = "Q";
                }
                if ((boolOverride == true && drLun["driveid"].ToString() == "0") || oForecast.IsOSMidrange(intAnswer) == true)
                {
                    sbStorage.Append("<td>");
                    sbStorage.Append(drLun["path"].ToString());
                    sbStorage.Append("</td>");
                }
                else
                {
                    sbStorage.Append("<td>");
                    sbStorage.Append(strLetter);
                    sbStorage.Append(":");
                    sbStorage.Append(drLun["path"].ToString());
                    sbStorage.Append("</td>");
                }
                sbStorage.Append("<td>");
                sbStorage.Append(drLun["performance"].ToString());
                sbStorage.Append("</td>");
                sbStorage.Append("<td>");
                sbStorage.Append(drLun["serialno"].ToString());
                sbStorage.Append("</td>");
                if (oClass.IsProd(intClass))
                {
                    sbStorage.Append("<td>");
                    sbStorage.Append(drLun["size"].ToString());
                    sbStorage.Append(" GB</td>");
                    sbStorage.Append("<td class=\"required\">");
                    sbStorage.Append(drLun["actual_size"].ToString() == "-1" ? "Pending" : drLun["actual_size"].ToString() + " GB");
                    sbStorage.Append("</td>");
                }
                if (oClass.IsQA(intClass))
                {
                    sbStorage.Append("<td>");
                    sbStorage.Append(drLun["size_qa"].ToString());
                    sbStorage.Append(" GB</td>");
                    sbStorage.Append("<td class=\"required\">");
                    sbStorage.Append(drLun["actual_size_qa"].ToString() == "-1" ? "Pending" : drLun["actual_size_qa"].ToString() + " GB");
                    sbStorage.Append("</td>");
                }
                if (oClass.IsTestDev(intClass))
                {
                    sbStorage.Append("<td>");
                    sbStorage.Append(drLun["size_test"].ToString());
                    sbStorage.Append(" GB</td>");
                    sbStorage.Append("<td class=\"required\">");
                    sbStorage.Append(drLun["actual_size_test"].ToString() == "-1" ? "Pending" : drLun["actual_size_test"].ToString() + " GB");
                    sbStorage.Append("</td>");
                }
                if (oClass.IsProd(intClass))
                {
                    sbStorage.Append("<td>");
                    sbStorage.Append(drLun["replicated"].ToString() == "0" ? "No" : "Yes");
                    sbStorage.Append("</td>");
                    sbStorage.Append("<td class=\"required\">");
                    sbStorage.Append(drLun["actual_replicated"].ToString() == "-1" ? "Pending" : (drLun["actual_replicated"].ToString() == "0" ? "No" : "Yes"));
                    sbStorage.Append("</td>");
                }
                if (oClass.IsProd(intClass))
                {
                    sbStorage.Append("<td>");
                    sbStorage.Append(drLun["high_availability"].ToString() == "0" ? "No" : "Yes (" + drLun["size"].ToString() + " GB)");
                    sbStorage.Append("</td>");
                    sbStorage.Append("<td class=\"required\">");
                    sbStorage.Append(drLun["actual_high_availability"].ToString() == "-1" ? "Pending" : (drLun["actual_high_availability"].ToString() == "0" ? "No" : "Yes (" + drLun["actual_size"].ToString() + " GB)"));
                    sbStorage.Append("</td>");
                }
                sbStorage.Append("</tr>");
                DataSet dsPoints = oStorage.GetMountPoints(Int32.Parse(drLun["id"].ToString()));
                int intPoint = 0;
                foreach (DataRow drPoint in dsPoints.Tables[0].Rows)
                {
                    intRow++;
                    intPoint++;
                    sbStorage.Append("<tr>");
                    sbStorage.Append("<td>");
                    sbStorage.Append(intRow.ToString());
                    sbStorage.Append("</td>");
                    if (oForecast.IsOSMidrange(intAnswer) == true)
                    {
                        sbStorage.Append("<td>");
                        sbStorage.Append(drPoint["path"].ToString());
                        sbStorage.Append("</td>");
                    }
                    else
                    {
                        sbStorage.Append("<td>");
                        sbStorage.Append(strLetter);
                        sbStorage.Append(":\\SH");
                        sbStorage.Append(drLun["driveid"].ToString());
                        sbStorage.Append("VOL");
                        sbStorage.Append(intPoint < 10 ? "0" : "");
                        sbStorage.Append(intPoint.ToString());
                        sbStorage.Append("</td>");
                    }
                    sbStorage.Append("<td>");
                    sbStorage.Append(drPoint["performance"].ToString());
                    sbStorage.Append("</td>");
                    sbStorage.Append("<td>");
                    sbStorage.Append(drPoint["serialno"].ToString());
                    sbStorage.Append("</td>");
                    if (oClass.IsProd(intClass))
                    {
                        sbStorage.Append("<td>");
                        sbStorage.Append(drPoint["size"].ToString());
                        sbStorage.Append(" GB</td>");
                        sbStorage.Append("<td class=\"required\">");
                        sbStorage.Append(drPoint["actual_size"].ToString() == "-1" ? "Pending" : drPoint["actual_size"].ToString() + " GB");
                        sbStorage.Append("</td>");
                    }
                    if (oClass.IsQA(intClass))
                    {
                        sbStorage.Append("<td>");
                        sbStorage.Append(drPoint["size_qa"].ToString());
                        sbStorage.Append(" GB</td>");
                        sbStorage.Append("<td class=\"required\">");
                        sbStorage.Append(drPoint["actual_size_qa"].ToString() == "-1" ? "Pending" : drPoint["actual_size_qa"].ToString() + " GB");
                        sbStorage.Append("</td>");
                    }
                    if (oClass.IsTestDev(intClass))
                    {
                        sbStorage.Append("<td>");
                        sbStorage.Append(drPoint["size_test"].ToString());
                        sbStorage.Append(" GB</td>");
                        sbStorage.Append("<td class=\"required\">");
                        sbStorage.Append(drPoint["actual_size_test"].ToString() == "-1" ? "Pending" : drPoint["actual_size_test"].ToString() + " GB");
                        sbStorage.Append("</td>");
                    }
                    if (oClass.IsProd(intClass))
                    {
                        sbStorage.Append("<td>");
                        sbStorage.Append(drPoint["replicated"].ToString() == "0" ? "No" : "Yes");
                        sbStorage.Append("</td>");
                        sbStorage.Append("<td class=\"required\">");
                        sbStorage.Append(drPoint["actual_replicated"].ToString() == "-1" ? "Pending" : (drPoint["actual_replicated"].ToString() == "0" ? "No" : "Yes"));
                        sbStorage.Append("</td>");
                    }
                    if (oClass.IsProd(intClass))
                    {
                        sbStorage.Append("<td>");
                        sbStorage.Append(drPoint["high_availability"].ToString() == "0" ? "No" : "Yes (" + drPoint["size"].ToString() + " GB)");
                        sbStorage.Append("</td>");
                        sbStorage.Append("<td class=\"required\">");
                        sbStorage.Append(drPoint["actual_high_availability"].ToString() == "-1" ? "Pending" : (drPoint["actual_high_availability"].ToString() == "0" ? "No" : "Yes (" + drPoint["actual_size"].ToString() + " GB)"));
                        sbStorage.Append("</td>");
                    }
                    sbStorage.Append("</tr>");
                }
            }
            return sbStorage.ToString();
        }
        protected void btnOrder_Click(Object Sender, EventArgs e)
        {
            int intResourceWorkflow = Int32.Parse(lblResourceWorkflow.Text);
            Response.Redirect(Request.Path + "?rrid=" + intResourceWorkflow.ToString() + "&div=E&sortname=" + ddlOrderName.SelectedItem.Value + "&sortip=" + ddlOrderIP.SelectedItem.Value);
        }
        protected void btnGenerateTest_Click(Object Sender, EventArgs e)
        {
            int intResourceWorkflow = Int32.Parse(lblResourceWorkflow.Text);
            int intResourceParent = oResourceRequest.GetWorkflowParent(intResourceWorkflow);
            int intAnswer = Int32.Parse(lblAnswer.Text);
            int intModel = Int32.Parse(lblModelID.Text);
            PDFs oPDF = new PDFs(dsn, dsnAsset, dsnIP, intEnvironment);
            // Complete Designs (and set step = 999 for servers)
            DataSet dsServers = oServer.GetAnswer(intAnswer);
            foreach (DataRow drServer in dsServers.Tables[0].Rows)
            {
                // Send to Service Center Request
                int intServer = Int32.Parse(drServer["id"].ToString());
                //oPDF.CreateSCRequest(intServer, boolUsePNCNaming);
                oServer.UpdateStep(intServer, 999);
            }
            oForecast.UpdateAnswerCompleted(intAnswer);

            int intQuantity = Int32.Parse(oResourceRequest.GetWorkflow(intResourceWorkflow, "devices"));
            if (oForecast.IsStorage(intAnswer) == true && oForecast.GetAnswer(intAnswer, "storage") == "1")
            {
                if (oModelsProperties.IsVMwareVirtual(intModel) == false)
                {
                    // Send to Storage
                    bool boolAssignNew = true;
                    if (oOnDemandTasks.GetServerStorage(intAnswer, 0).Tables[0].Rows.Count > 0)
                        boolAssignNew = false;
                    if (boolAssignNew == true)
                    {
                        int intStorageNumber = oResourceRequest.GetNumber(intRequest, intStorageItem);
                        oOnDemandTasks.AddServerStorage(intRequest, intStorageItem, intStorageNumber, intAnswer, 0, intModel);
                        int intStorage = oResourceRequest.Add(intRequest, intStorageItem, intStorageService, intStorageNumber, "Auto-Provisioning Task (Storage)", intQuantity, 0.00, 2, 1, 1, 1);
                        if (oServiceRequest.NotifyApproval(intStorage, intResourceRequestApprove, intEnvironment, "", dsnServiceEditor) == false)
                            oServiceRequest.NotifyTeamLead(intStorageItem, intStorage, intAssignPage, intViewPage, intEnvironment,  "", dsnServiceEditor, dsnAsset, dsnIP, 0);
                        // Generate Storage Form
                        //oPDF.CreateDocuments(intAnswer, true, false, null, true, true, true, false);
                    }
                }
            }
            else
            {
                oOnDemandTasks.UpdateStorage(intAnswer, intModel);
                chk7.Checked = true;
            }
            if (oForecast.GetAnswer(intAnswer, "backup") == "1")
            {
                // Send to Backup
                bool boolAssignNew = true;
                if (oOnDemandTasks.GetServerBackup(intAnswer).Tables[0].Rows.Count > 0)
                    boolAssignNew = false;
                if (boolAssignNew == true)
                {
                    int intBackupNumber = oResourceRequest.GetNumber(intRequest, intBackupItem);
                    oOnDemandTasks.AddServerBackup(intRequest, intBackupItem, intBackupNumber, intAnswer, intModel);
                    int intBackup = oResourceRequest.Add(intRequest, intBackupItem, intBackupService, intBackupNumber, "Auto-Provisioning Task (Backup)", intQuantity, 0.00, 2, 1, 1, 1);
                    if (oServiceRequest.NotifyApproval(intBackup, intResourceRequestApprove, intEnvironment, "", dsnServiceEditor) == false)
                        oServiceRequest.NotifyTeamLead(intBackupItem, intBackup, intAssignPage, intViewPage, intEnvironment,  "", dsnServiceEditor, dsnAsset, dsnIP, 0);
                    // Generate Backup Form
                    //oPDF.CreateDocuments(intAnswer, false, true, null, true, true, true, false);
                }
            }
            oOnDemandTasks.UpdateGenericIINotificationsTest(intRequest, intItem, intNumber);
            // Generate Birth Certificate
            oPDF.CreateDocuments(intAnswer, false, false, null, true, true, true, false, boolUsePNCNaming, false);
            Response.Redirect(Request.Path + "?rrid=" + intResourceWorkflow.ToString() + "&div=E&save=true");
        }
        protected void btnGenerateProd_Click(Object Sender, EventArgs e)
        {
            int intResourceWorkflow = Int32.Parse(lblResourceWorkflow.Text);
            int intResourceParent = oResourceRequest.GetWorkflowParent(intResourceWorkflow);
            int intAnswer = Int32.Parse(lblAnswer.Text);
            int intModel = Int32.Parse(lblModelID.Text);
            PDFs oPDF = new PDFs(dsn, dsnAsset, dsnIP, intEnvironment);
            int intQuantity = Int32.Parse(oResourceRequest.GetWorkflow(intResourceWorkflow, "devices"));
            if (oForecast.IsStorage(intAnswer) == true && oForecast.GetAnswer(intAnswer, "storage") == "1")
            {
                if (oModelsProperties.IsVMwareVirtual(intModel) == false)
                {
                    // Send to Storage
                    bool boolAssignNew = true;
                    if (oOnDemandTasks.GetServerStorage(intAnswer, 1).Tables[0].Rows.Count > 0)
                        boolAssignNew = false;
                    if (boolAssignNew == true)
                    {
                        int intStorageNumber = oResourceRequest.GetNumber(intRequest, intStorageItem);
                        oOnDemandTasks.AddServerStorage(intRequest, intStorageItem, intStorageNumber, intAnswer, 1, intModel);
                        int intStorage = oResourceRequest.Add(intRequest, intStorageItem, intStorageService, intStorageNumber, "Auto-Provisioning Task (Storage)", intQuantity, 0.00, 2, 1, 1, 1);
                        if (oServiceRequest.NotifyApproval(intStorage, intResourceRequestApprove, intEnvironment, "", dsnServiceEditor) == false)
                            oServiceRequest.NotifyTeamLead(intStorageItem, intStorage, intAssignPage, intViewPage, intEnvironment,  "", dsnServiceEditor, dsnAsset, dsnIP, 0);
                    }
                }
            }
            oOnDemandTasks.UpdateGenericIINotificationsProd(intRequest, intItem, intNumber);
            Response.Redirect(Request.Path + "?rrid=" + intResourceWorkflow.ToString() + "&div=E&save=true");
        }
    }
}
