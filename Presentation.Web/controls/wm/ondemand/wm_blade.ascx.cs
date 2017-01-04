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
    public partial class wm_blade : System.Web.UI.UserControl
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
        protected int intStorageItem = Int32.Parse(ConfigurationManager.AppSettings["ITEMID_STORAGE"]);
        protected int intBackupItem = Int32.Parse(ConfigurationManager.AppSettings["ITEMID_BACKUP"]);
        protected int intStorageService = Int32.Parse(ConfigurationManager.AppSettings["SERVICEID_STORAGE"]);
        protected int intBackupService = Int32.Parse(ConfigurationManager.AppSettings["SERVICEID_BACKUP"]);
        protected int intCore = Int32.Parse(ConfigurationManager.AppSettings["CoreEnvironmentID"]);
        protected string strHA = ConfigurationManager.AppSettings["FORECAST_HIGH_AVAILABILITY"];
        protected string strTSM = ConfigurationManager.AppSettings["TSM_MAILBOX"];
        protected string strSAN = ConfigurationManager.AppSettings["SAN_MAILBOX"];
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
        protected ServerName oServerName;
        protected Classes oClass;
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
        private double dbl1 = 0.50;
       
        private double dbl3 = 0.25;
        private double dbl4 = 3.00;
        private double dbl5 = 0.50;
        private double dbl6 = 0.00;
        private double dbl7 = 0.50;
        private double dbl8 = 1.00;
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
            oRequestField = new RequestFields(intProfile, dsn);
            oServiceRequest = new ServiceRequests(intProfile, dsn);
            oApplication = new Applications(intProfile, dsn);
            oOnDemandTasks = new OnDemandTasks(intProfile, dsn);
            oDelegate = new Delegates(intProfile, dsn);
            oForecast = new Forecast(intProfile, dsn);
            oServer = new Servers(0, dsn);
            oStorage = new Storage(intProfile, dsn);
            oModelsProperties = new ModelsProperties(intProfile, dsn);
            oServerName = new ServerName(intProfile, dsn);
            oClass = new Classes(intProfile, dsn);
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
                if (Request.QueryString["status"] != null && Request.QueryString["status"] != "")
                    Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "statusd", "<script type=\"text/javascript\">alert('Weekly Status has been Added');<" + "/" + "script>");
                if (Request.QueryString["reserve"] != null && Request.QueryString["reserve"] != "")
                {
                    string strReserve = "";
                    if (Request.QueryString["prod"] != null && Request.QueryString["prod"] != "")
                        strReserve += "There was a problem reserving one or more PRODUCTION assets.\\n\\nYour ClearView administrator has been notified.";
                    if (Request.QueryString["dr"] != null && Request.QueryString["dr"] != "")
                        strReserve += "There was a problem reserving one or more DISASTER RECOVERY assets.\\n\\nYour ClearView administrator has been notified.";
                    Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "reserved", "<script type=\"text/javascript\">alert('" + strReserve + "');<" + "/" + "script>");
                }
                intProject = oRequest.GetProjectNumber(intRequest);
                hdnTab.Value = "D";
                panWorkload.Visible = true;
                LoadStatus(intResourceWorkflow);
                bool boolDone = LoadInformation(intResourceWorkflow);
                if (boolDone == true)
                {
                    if (boolComplete == false)
                    {
                        oFunction.ConfigureToolButton(btnComplete, "/images/tool_complete");
                        btnComplete.Attributes.Add("onclick", "return ValidateDropDown('" + ddlSuccess.ClientID + "','Please select a status') && confirm('Are you sure you want to mark this as completed and remove it from your workload?');");
                    }
                    else
                    {
                        oFunction.ConfigureToolButton(btnComplete, "/images/tool_complete");
                        btnComplete.Attributes.Add("onclick", "alert('This task has already been marked COMPLETE. You can close this window.');return false;");
                    }
                    imgSuccess.ImageUrl = "/images/green_arrow.gif";
                    btnSave.ImageUrl = "/images/tool_save.gif";
                    btnSave.Enabled = false;
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
                btnSave.Attributes.Add("onclick", "return ValidateStatus('" + ddlStatus.ClientID + "','" + txtIssues.ClientID + "');");
                btnProduction.Attributes.Add("onclick", "return ProcessButton(this);");
                btnRelease.Attributes.Add("onclick", "return ProcessButton(this);");
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
            bool boolDone = false;
            DataSet ds = oOnDemandTasks.GetBladeII(intRequest, intItem, intNumber);
            if (ds.Tables[0].Rows.Count > 0)
            {
                int intAnswer = Int32.Parse(ds.Tables[0].Rows[0]["answerid"].ToString());
                int intClass = Int32.Parse(oForecast.GetAnswer(intAnswer, "classid"));
                int intEnv = Int32.Parse(oForecast.GetAnswer(intAnswer, "environmentid"));
                Classes oClass = new Classes(0, dsn);
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
                btnProduction.Enabled = false;
                btnProduction.CommandArgument = intAnswer.ToString();
                btnRelease.Enabled = false;
                btnRelease.CommandArgument = intAnswer.ToString();
                btnView.Attributes.Add("onclick", "return OpenWindow('FORECAST_EQUIPMENT','?id=" + intAnswer.ToString() + "');");
                btnBirth.Attributes.Add("onclick", "return OpenWindow('PDF_BIRTH','?id=" + intAnswer.ToString() + "');");
                btnSC.Attributes.Add("onclick", "return OpenWindow('PDF_BIRTH','?id=" + intAnswer.ToString() + "');");
                lblView.Text = oOnDemandTasks.GetBody(intAnswer, intImplementorDistributed, intImplementorMidrange);
                int intModel = Int32.Parse(ds.Tables[0].Rows[0]["modelid"].ToString());
                intModel = Int32.Parse(oModelsProperties.Get(intModel, "modelid"));
                Models oModel = new Models(intProfile, dsn);
                int intType = Int32.Parse(oModel.Get(intModel, "typeid"));
                Types oType = new Types(intProfile, dsn);
                string strExecute = oType.Get(intType, "forecast_execution_path");
                if (strExecute != "")
                    btnExecute.Attributes.Add("onclick", "return OpenWindow('FORECAST_EXECUTE','" + strExecute + "?id=" + intAnswer.ToString() + "');");
                chk1.Checked = (ds.Tables[0].Rows[0]["chk1"].ToString() == "1");
                chk3.Checked = (ds.Tables[0].Rows[0]["chk3"].ToString() == "1");
                chk4.Checked = (ds.Tables[0].Rows[0]["chk4"].ToString() == "1");
                chk5.Checked = (ds.Tables[0].Rows[0]["chk5"].ToString() == "1");
                chk6.Checked = (ds.Tables[0].Rows[0]["chk6"].ToString() == "1");
                chk7.Checked = (ds.Tables[0].Rows[0]["chk7"].ToString() == "1");
                chk8.Checked = (ds.Tables[0].Rows[0]["chk8"].ToString() == "1");
                chk9.Checked = (ds.Tables[0].Rows[0]["chk9"].ToString() == "1");
                chk10.Checked = (ds.Tables[0].Rows[0]["chk10"].ToString() == "1");
                chk11.Checked = (ds.Tables[0].Rows[0]["chk11"].ToString() == "1");
                chk12.Checked = (ds.Tables[0].Rows[0]["chk12"].ToString() == "1");
                int intNotifications = Int32.Parse(ds.Tables[0].Rows[0]["notifications"].ToString());
                bool boolProduction = true;
                if (oClass.Get(intClass, "prod") != "1" || (oClass.Get(intClass, "prod") == "1" && oClass.Get(intClass, "pnc") == "1"))
                {
                    boolProduction = false;
                    chk9.Checked = true;
                    img9.ImageUrl = "/images/cancel.gif";
                    chk10.Checked = true;
                    img10.ImageUrl = "/images/cancel.gif";
                    chk11.Checked = true;
                    img11.ImageUrl = "/images/cancel.gif";
                    chk12.Checked = true;
                    img12.ImageUrl = "/images/cancel.gif";
                }
                boolDone = (chk1.Checked && chk3.Checked && chk4.Checked && chk5.Checked && chk6.Checked && chk7.Checked && chk8.Checked && chk9.Checked && chk10.Checked && chk11.Checked && chk12.Checked);
               
                img1.ImageUrl = "/images/green_arrow.gif";
                if (chk1.Checked == true)
                {
                    btnExecute.Enabled = false;
                    img1.ImageUrl = "/images/check.gif";
                    img3.ImageUrl = "/images/green_arrow.gif";
                    chk3.Enabled = true;
                    DataSet dsAnswer = oServer.GetAnswer(intAnswer);
                    foreach (DataRow drAnswer in dsAnswer.Tables[0].Rows)
                    {
                        int intServer = Int32.Parse(drAnswer["id"].ToString());
                        int intAsset = 0;
                        int intClass2 = 0;
                        int intEnv2 = 0;
                        if (drAnswer["assetid"].ToString() != "")
                        {
                            intAsset = Int32.Parse(drAnswer["assetid"].ToString());
                            intClass2 = Int32.Parse(drAnswer["classid"].ToString());
                            intEnv2 = Int32.Parse(drAnswer["environmentid"].ToString());
                        }
                        if (intClass2 == 0 || intEnv2 == 0 || intClass2 != intClass || boolProduction == false || intEnv2 != intEnv)
                        {
                            if (lbl1.Text != "")
                                lbl1.Text += "<br/>";
                            int intServerName = Int32.Parse(drAnswer["nameid"].ToString());
                            string strName = oServer.GetName(intServer, boolUsePNCNaming);
                            lbl1.Text += "Server Name: " + strName + "<br/>";
                            int intDomain = Int32.Parse(drAnswer["domainid"].ToString());
                            Domains oDomain = new Domains(intProfile, dsn);
                            boolMove = (oDomain.Get(intDomain, "move") == "1");
                            if (boolMove == true)
                                lbl1.Text += "DHCP Address: " + drAnswer["dhcp"].ToString() + "<br/>";
                            int intIPAddress = 0;
                            IPAddresses oIPAddresses = new IPAddresses(0, dsnIP, dsn);
                            lbl1.Text += "Assigned IP Address: " + oServer.GetIPs(intServer, 1, 0, 0, dsnIP, "", "") + "<br/>";
                            lbl1.Text += "Final IP Address: " + oServer.GetIPs(intServer, 0, 1, 0, dsnIP, "", "") + "<br/>";
                            Asset oAsset = new Asset(intProfile, dsnAsset);
                            if (intAsset > 0)
                            {
                                DataSet dsAsset = oAsset.GetServerOrBlade(intAsset);
                                if (dsAsset.Tables[0].Rows.Count > 0)
                                {
                                    lbl1.Text += "Serial Number: " + dsAsset.Tables[0].Rows[0]["serial"].ToString() + "<br/>";
                                    int intEnclosure = Int32.Parse(dsAsset.Tables[0].Rows[0]["enclosureid"].ToString());
                                    // Enclosure name
                                    lbl1.Text += "Enclosure: " + oAsset.GetEnclosure(intEnclosure, "name") + "<br/>";
                                    lbl1.Text += "Bay #: " + dsAsset.Tables[0].Rows[0]["slot"].ToString() + "<br/>";
                                    lbl1.Text += "Room: " + oAsset.GetEnclosure(intEnclosure, "room") + "<br/>";
                                    lbl1.Text += "Rack: " + oAsset.GetEnclosure(intEnclosure, "rack") + "<br/>";
                                    lbl1.Text += "ILO: " + "<a href=\"https://" + dsAsset.Tables[0].Rows[0]["ilo"].ToString() + "\" target=\"_blank\">" + dsAsset.Tables[0].Rows[0]["ilo"].ToString() + "</a><br/>";
                                }
                            }
                            int intCluster2 = Int32.Parse(drAnswer["clusterid"].ToString());
                            int intCSMConfig2 = Int32.Parse(drAnswer["csmconfigid"].ToString());
                            int intNumber2 = Int32.Parse(drAnswer["number"].ToString());
                            lbl1.Text += GetStorage(intAnswer, intCluster2, intCSMConfig2, intNumber2, intModel, strName);
                        }
                        lbl1.Text += GetStorageShared(intAnswer, intModel);
                    }
                    
                }
                if (chk3.Checked == true)
                {
                    chk1.Enabled = false;
                    img3.ImageUrl = "/images/check.gif";
                    img4.ImageUrl = "/images/green_arrow.gif";
                    chk4.Enabled = true;
                }
                if (chk4.Checked == true)
                {
                    chk3.Enabled = false;
                    img4.ImageUrl = "/images/check.gif";
                    img5.ImageUrl = "/images/active.gif";
                    // Open checkbox for editing by II team
                    chk5.Enabled = true;
                }
                if (chk5.Checked == true)
                {
                    // Open checkbox for editing by II team
                    //chk4.Enabled = false;
                    img5.ImageUrl = "/images/check.gif";
                    img6.ImageUrl = "/images/green_arrow.gif";
                    chk6.Enabled = true;
                }
                if (chk6.Checked == true)
                {
                    chk5.Enabled = false;
                    img6.ImageUrl = "/images/check.gif";
                    img7.ImageUrl = "/images/green_arrow.gif";
                    chk7.Enabled = true;
                }
                if (chk7.Checked == true)
                {
                    chk6.Enabled = false;
                    img7.ImageUrl = "/images/check.gif";
                    img8.ImageUrl = "/images/green_arrow.gif";
                    chk8.Enabled = true;
                }
                if (chk8.Checked == true)
                {
                    btnProduction.Enabled = true;
                    chk7.Enabled = false;
                    img8.ImageUrl = "/images/check.gif";
                    img9.ImageUrl = "/images/green_arrow.gif";
                    chk9.Enabled = true;
                }
                if (chk9.Checked == true)
                {
                    chk9.Enabled = false;
                    btnProduction.Enabled = false;
                    if (boolProduction == true)
                    {
                        DataSet dsAssets = oServer.GetAssets(intAnswer, intClass, intEnv);
                        foreach (DataRow drAsset in dsAssets.Tables[0].Rows)
                        {
                            if (lbl9.Text != "")
                                lbl9.Text += "<br/>";
                            int intServer = Int32.Parse(drAsset["id"].ToString());
                            int intServerName = Int32.Parse(drAsset["nameid"].ToString());
                            string strName = oServer.GetName(intServer, boolUsePNCNaming);
                            Asset oAsset = new Asset(intProfile, dsnAsset);
                            int intAsset = 0;
                            if (drAsset["assetid"].ToString() != "")
                                intAsset = Int32.Parse(drAsset["assetid"].ToString());
                            if (intAsset > 0)
                            {
                                DataSet dsAsset = oAsset.GetServerOrBlade(intAsset);
                                if (dsAsset.Tables[0].Rows.Count > 0)
                                {
                                    lbl9.Text += "Server Name: " + dsAsset.Tables[0].Rows[0]["name"].ToString() + "<br/>";
                                    lbl9.Text += "Serial Number: " + dsAsset.Tables[0].Rows[0]["serial"].ToString() + "<br/>";
                                    int intEnclosure = Int32.Parse(dsAsset.Tables[0].Rows[0]["enclosureid"].ToString());
                                    // Enclosure name
                                    lbl9.Text += "Enclosure: " + oAsset.GetEnclosure(intEnclosure, "name") + "<br/>";
                                    lbl9.Text += "Bay #: " + dsAsset.Tables[0].Rows[0]["slot"].ToString() + "<br/>";
                                    lbl9.Text += "Room: " + oAsset.GetEnclosure(intEnclosure, "room") + "<br/>";
                                    lbl9.Text += "Rack: " + oAsset.GetEnclosure(intEnclosure, "rack") + "<br/>";
                                    lbl9.Text += "ILO: " + "<a href=\"https://" + dsAsset.Tables[0].Rows[0]["ilo"].ToString() + "\" target=\"_blank\">" + dsAsset.Tables[0].Rows[0]["ilo"].ToString() + "</a><br/>";
                                }
                            }
                            int intCluster2 = Int32.Parse(drAsset["clusterid"].ToString());
                            int intCSMConfig2 = Int32.Parse(drAsset["csmconfigid"].ToString());
                            int intNumber2 = Int32.Parse(drAsset["number"].ToString());
                            lbl9.Text += GetStorage(intAnswer, intCluster2, intCSMConfig2, intNumber2, intModel, strName);
                        }
                        if (lbl9.Text == "")
                            lbl9.Text = "<img src=\"/images/alert.gif\" border=\"0\" align=\"absmiddle\"/> There are no assets assigned to this design";
                        else
                            lbl9.Text += GetStorageShared(intAnswer, intModel);
                        img9.ImageUrl = "/images/check.gif";
                        chk8.Enabled = false;
                    }
                    else
                        img9.ImageUrl = "/images/cancel.gif";
                    img10.ImageUrl = "/images/green_arrow.gif";
                    chk10.Enabled = true;
                }
                if (chk10.Checked == true)
                {
                    chk9.Enabled = false;
                    if (boolProduction == true)
                        img10.ImageUrl = "/images/check.gif";
                    else
                        img10.ImageUrl = "/images/cancel.gif";
                    img11.ImageUrl = "/images/green_arrow.gif";
                    chk11.Enabled = true;
                }
                if (chk11.Checked == true)
                {
                    chk10.Enabled = false;
                    if (boolProduction == true)
                    {
                        img11.ImageUrl = "/images/check.gif";
                        btnRelease.Enabled = true;
                    }
                    else
                        img11.ImageUrl = "/images/cancel.gif";
                    img12.ImageUrl = "/images/green_arrow.gif";
                    chk12.Enabled = true;
                }
                if (chk12.Checked == true)
                {
                    chk11.Enabled = false;
                    btnRelease.Enabled = false;
                    if (boolProduction == true)
                        img12.ImageUrl = "/images/check.gif";
                    else
                    {
                        img12.ImageUrl = "/images/cancel.gif";
                        chk12.Enabled = false;
                    }
                }
                // *** END CODE (COMMENT / UNCOMMENT)
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
        private string GetStorage(int intAnswer, int intCluster2, int intCSMConfig2, int intNumber2, int intModel, string strName)
        {
            DataSet dsLuns = new DataSet();
            if (intCluster2 == 0)
                dsLuns = oStorage.GetLuns(intAnswer, 0, intCluster2, intCSMConfig2, intNumber2);
            else
                dsLuns = oStorage.GetLunsClusterNonShared(intAnswer, intCluster2);
            bool boolOverride = (oForecast.GetAnswer(intAnswer, "storage_override") == "1");
            string strStorage = AddStorage(dsLuns, intModel, boolOverride);
            if (strStorage != "")
            {
                strStorage = "<tr class=\"bold\"><td></td><td>Path</td><td>Performance</td><td>Requested<br/>Size in Prod</td><td>Actual<br/>Size in Prod</td><td>Requested<br/>Size in QA</td><td>Actual<br/>Size in QA</td><td>Requested<br/>Size in Test</td><td>Actual<br/>Size in Test</td><td>Requested<br/>Replication</td><td>Actual<br/>Replication</td><td>Requested<br/>High Availability</td><td>Actual<br/>High Availability</td></tr>" + strStorage;
                strStorage = "<table id=\"tblStorage" + intCluster2.ToString() + "_" + intNumber2.ToString() + "\" style=\"display:none\" width=\"100%\" cellpadding=\"2\" cellspacing=\"1\" border=\"0\">" + strStorage + "</table>";
                strStorage = "<a href=\"javascript:void(0);\" onclick=\"ShowHideDiv2('tblStorage" + intCluster2.ToString() + "_" + intNumber2.ToString() + "');\">Show Storage Settings for " + strName + "</a><br/>" + strStorage;
            }
            return strStorage;
        }
        private string GetStorageShared(int intAnswer, int intModel)
        {
            bool boolOverride = (oForecast.GetAnswer(intAnswer, "storage_override") == "1");
            string strClusterNames = "";
            string strStorage = "";
            int intClusterOLD = 0;
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
                            strClusterNames += oServer.GetName(intServer, boolUsePNCNaming);
                        }
                        DataSet dsLuns = oStorage.GetLunsClusterShared(intAnswer, intClusterID);
                        strStorage += AddStorage(dsLuns, intModel, boolOverride);
                    }
                }
                intClusterOLD = intClusterID;
            }
            if (strStorage != "")
            {
                strStorage = "<tr class=\"bold\"><td></td><td>Path</td><td>Performance</td><td>Requested<br/>Size in Prod</td><td>Actual<br/>Size in Prod</td><td>Requested<br/>Size in QA</td><td>Actual<br/>Size in QA</td><td>Requested<br/>Size in Test</td><td>Actual<br/>Size in Test</td><td>Requested<br/>Replication</td><td>Actual<br/>Replication</td><td>Requested<br/>High Availability</td><td>Actual<br/>High Availability</td></tr>" + strStorage;
                strStorage = "<table id=\"tblStorage" + intClusterOLD.ToString() + "\" style=\"display:none\" width=\"100%\" cellpadding=\"2\" cellspacing=\"1\" border=\"0\">" + strStorage + "</table>";
                strStorage = "<br/><a href=\"javascript:void(0);\" onclick=\"ShowHideDiv2('tblStorage" + intClusterOLD.ToString() + "');\" class=\"redlink\">Show Shared Storage for " + strClusterNames + "</a><br/>" + strStorage;
            }
            return strStorage;
        }
        private string AddStorage(DataSet dsLuns, int intModel, bool boolOverride)
        {
            int intAnswer = Int32.Parse(lblAnswer.Text);
            StringBuilder sbStorage = new StringBuilder();
            int intRow = 0;
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
                sbStorage.Append(drLun["size"].ToString());
                sbStorage.Append(" GB</td>");
                sbStorage.Append("<td class=\"required\">");
                sbStorage.Append(drLun["actual_size"].ToString() == "-1" ? "Pending" : drLun["actual_size"].ToString() + " GB");
                sbStorage.Append("</td>");
                sbStorage.Append("<td>");
                sbStorage.Append(drLun["size_qa"].ToString());
                sbStorage.Append(" GB</td>");
                sbStorage.Append("<td class=\"required\">");
                sbStorage.Append(drLun["actual_size_qa"].ToString() == "-1" ? "Pending" : drLun["actual_size_qa"].ToString() + " GB");
                sbStorage.Append("</td>");
                sbStorage.Append("<td>");
                sbStorage.Append(drLun["size_test"].ToString());
                sbStorage.Append(" GB</td>");
                sbStorage.Append("<td class=\"required\">");
                sbStorage.Append(drLun["actual_size_test"].ToString() == "-1" ? "Pending" : drLun["actual_size_test"].ToString() + " GB");
                sbStorage.Append("</td>");
                sbStorage.Append("<td>");
                sbStorage.Append(drLun["replicated"].ToString() == "0" ? "No" : "Yes");
                sbStorage.Append("</td>");
                sbStorage.Append("<td class=\"required\">");
                sbStorage.Append(drLun["actual_replicated"].ToString() == "-1" ? "Pending" : (drLun["actual_replicated"].ToString() == "0" ? "No" : "Yes"));
                sbStorage.Append("</td>");
                sbStorage.Append("<td>");
                sbStorage.Append(drLun["high_availability"].ToString() == "0" ? "No" : "Yes (" + drLun["size"].ToString() + " GB)");
                sbStorage.Append("</td>");
                sbStorage.Append("<td class=\"required\">");
                sbStorage.Append(drLun["actual_high_availability"].ToString() == "-1" ? "Pending" : (drLun["actual_high_availability"].ToString() == "0" ? "No" : "Yes (" + drLun["actual_size"].ToString() + " GB)"));
                sbStorage.Append("</td>");
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
                    sbStorage.Append(drPoint["size"].ToString());
                    sbStorage.Append(" GB</td>");
                    sbStorage.Append("<td class=\"required\">");
                    sbStorage.Append(drPoint["actual_size"].ToString() == "-1" ? "Pending" : drPoint["actual_size"].ToString() + " GB");
                    sbStorage.Append("</td>");
                    sbStorage.Append("<td>");
                    sbStorage.Append(drPoint["size_qa"].ToString());
                    sbStorage.Append(" GB</td>");
                    sbStorage.Append("<td class=\"required\">");
                    sbStorage.Append(drPoint["actual_size_qa"].ToString() == "-1" ? "Pending" : drPoint["actual_size_qa"].ToString() + " GB");
                    sbStorage.Append("</td>");
                    sbStorage.Append("<td>");
                    sbStorage.Append(drPoint["size_test"].ToString());
                    sbStorage.Append(" GB</td>");
                    sbStorage.Append("<td class=\"required\">");
                    sbStorage.Append(drPoint["actual_size_test"].ToString() == "-1" ? "Pending" : drPoint["actual_size_test"].ToString() + " GB");
                    sbStorage.Append("</td>");
                    sbStorage.Append("<td>");
                    sbStorage.Append(drPoint["replicated"].ToString() == "0" ? "No" : "Yes");
                    sbStorage.Append("</td>");
                    sbStorage.Append("<td class=\"required\">");
                    sbStorage.Append(drPoint["actual_replicated"].ToString() == "-1" ? "Pending" : (drPoint["actual_replicated"].ToString() == "0" ? "No" : "Yes"));
                    sbStorage.Append("</td>");
                    sbStorage.Append("<td>");
                    sbStorage.Append(drPoint["high_availability"].ToString() == "0" ? "No" : "Yes (" + drPoint["size"].ToString() + " GB)");
                    sbStorage.Append("</td>");
                    sbStorage.Append("<td class=\"required\">");
                    sbStorage.Append(drPoint["actual_high_availability"].ToString() == "-1" ? "Pending" : (drPoint["actual_high_availability"].ToString() == "0" ? "No" : "Yes (" + drPoint["actual_size"].ToString() + " GB)"));
                    sbStorage.Append("</td>");
                    sbStorage.Append("</tr>");
                }
            }
            return sbStorage.ToString();
        }
        protected void btnProduction_Click(Object Sender, EventArgs e)
        {
            int intResourceWorkflow = Int32.Parse(lblResourceWorkflow.Text);
            int intResourceParent = oResourceRequest.GetWorkflowParent(intResourceWorkflow);
            int intAnswer = Int32.Parse(btnProduction.CommandArgument);
            int intRecovery = 0;
            if (oForecast.GetAnswer(intAnswer, "recovery_number") != "")
                intRecovery = Int32.Parse(oForecast.GetAnswer(intAnswer, "recovery_number"));
            Variables oVariable = new Variables(intEnvironment);
            int intClass = Int32.Parse(oForecast.GetAnswer(intAnswer, "classid"));
            int intEnv = Int32.Parse(oForecast.GetAnswer(intAnswer, "environmentid"));
            int intAddress = Int32.Parse(oForecast.GetAnswer(intAnswer, "addressid"));
            Asset oAsset = new Asset(intProfile, dsnAsset);
            Servers oServer = new Servers(intProfile, dsn);
            DataSet dsAnswer = oServer.GetAnswer(intAnswer);
            foreach (DataRow drAnswer in dsAnswer.Tables[0].Rows)
            {
                int intServer = Int32.Parse(drAnswer["id"].ToString());
                int intName = Int32.Parse(drAnswer["nameid"].ToString());
                string strName = oServer.GetName(intServer, boolUsePNCNaming);
                int intModel = Int32.Parse(drAnswer["modelid"].ToString());
                int intAsset = oAsset.GetServerOrBladeAvailable(intClass, intEnv, intAddress, intModel, intAnswer, dsn, strHA, false);
                int intAssetDR = 0;
                if (intAsset > 0)
                {
                    if (intRecovery > 0)
                    {
                        DataSet dsAssets = oAsset.GetServerOrBladeAvailableDR(intEnv, intModel, intAsset);
                        if (dsAssets.Tables[0].Rows.Count > 0)
                            intAssetDR = Int32.Parse(dsAssets.Tables[0].Rows[0]["id"].ToString());
                    }
                    else
                        intAssetDR = -1;
                }
                if (intAsset > 0 && intAssetDR != 0)
                {
                    oAsset.AddStatus(intAsset, strName, (int)AssetStatus.InUse, intProfile, DateTime.Now);
                    oServer.AddAsset(intServer, intAsset, intClass, intEnv, 0, 0);
                    if (intAssetDR > 0)
                    {
                        oAsset.AddStatus(intAssetDR, strName + "-DR", (int)AssetStatus.InUse, intProfile, DateTime.Now);
                        oServer.AddAsset(intServer, intAssetDR, intClass, intEnv, 0, 1);
                        string strBody = oAsset.GetDRBody(intAsset, intAssetDR, "Production Asset", "Disaster Recovery Asset", intEnvironment);
                        strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DEVELOPER_ALERT");
                        oFunction.SendEmail("Disaster Recovery Asset Notification", oVariable.DRNotifications(), "", strEMailIdsBCC, "Disaster Recovery Asset Notification", "<p>This message is to inform you that the following assets have been allocated in ClearView...<p><p>" + strBody + "</p>" + oVariable.EmailFooter(), true, false);
                    }
                    oOnDemandTasks.UpdateBladeII(intRequest, intItem, intNumber, (chk1.Checked ? 1 : 0), (chk3.Checked ? 1 : 0), (chk4.Checked ? 1 : 0), (chk5.Checked ? 1 : 0), (chk6.Checked ? 1 : 0), (chk7.Checked ? 1 : 0), (chk8.Checked ? 1 : 0), 1, (chk10.Checked ? 1 : 0), (chk11.Checked ? 1 : 0), (chk12.Checked ? 1 : 0));
                    oOnDemandTasks.UpdateBladeIIProd(intRequest, intItem, intNumber);
                }
                else if (intAsset == 0)
                {
                    strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DEVELOPER_ERROR");
                    oFunction.SendEmail("!!! ERROR: Reserving Production Assets !!!", strEMailIdsBCC, "", "", "!!! ERROR: Reserving Production Assets !!!", "<p>The production assets for SERVERID " + intServer.ToString() + " (BLADE) were not reserved - Class: " + intClass.ToString() + ", Env: " + intEnv.ToString() + ", Address: " + intAddress.ToString() + ", Model: " + intModel.ToString() + "<p>", true, false);
                    Response.Redirect(Request.Path + "?rrid=" + intResourceWorkflow.ToString() + "&div=E&reserve=true&prod=true");
                }
                else if (intAssetDR == 0)
                {
                    strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DEVELOPER_ERROR");
                    oFunction.SendEmail("!!! ERROR: Reserving DR Assets !!!", strEMailIdsBCC, "", "", "!!! ERROR: Reserving DR Assets !!!", "<p>The DR assets for SERVERID " + intServer.ToString() + " (BLADE) were not reserved - PROD ASSETID: " + intAsset.ToString() + "<p>", true, false);
                    Response.Redirect(Request.Path + "?rrid=" + intResourceWorkflow.ToString() + "&div=E&reserve=true&dr=true");
                }
            }
            Response.Redirect(Request.Path + "?rrid=" + intResourceWorkflow.ToString() + "&div=E&save=true");
        }
        protected void btnRelease_Click(Object Sender, EventArgs e)
        {
            int intAnswer = Int32.Parse(btnRelease.CommandArgument);
            int intClass = Int32.Parse(oForecast.GetAnswer(intAnswer, "classid"));
            int intEnv = Int32.Parse(oForecast.GetAnswer(intAnswer, "environmentid"));
            Variables oVariable = new Variables(intEnvironment);
            DataSet dsAssets = oServer.GetAssetsNot(intAnswer, intClass, intEnv);
            Asset oAsset = new Asset(0, dsnAsset);
            foreach (DataRow drAsset in dsAssets.Tables[0].Rows)
            {
                int intServer = Int32.Parse(drAsset["id"].ToString());
                int intAsset = Int32.Parse(drAsset["assetid"].ToString());
                DataSet dsStatus = oAsset.GetStatus(intAsset);
                bool boolReleased = true;
                if (dsStatus.Tables[0].Rows.Count > 0)
                {
                    if (Int32.Parse(dsStatus.Tables[0].Rows[0]["status"].ToString()) == (int)AssetStatus.InUse && dsStatus.Tables[0].Rows[0]["name"].ToString() == oServer.GetName(intServer, boolUsePNCNaming))
                    {
                        oAsset.AddStatus(intAsset, "", (int)AssetStatus.Available, intProfile, DateTime.Now);
                        //oServer.DeleteAsset(intServer, intAsset);
                        oServer.DeleteIP(intServer, 1, 0, 0, dsnIP);
                    }
                    else
                        boolReleased = false;
                }
                else
                    boolReleased = false;
                strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DEVELOPER_ALERT");
                if (boolReleased == true)
                    oFunction.SendEmail("!!! Releasing Build Assets !!!", strEMailIdsBCC, "", "", "!!! Releasing Build Assets !!!", "The build assets for ANSWERID " + intAnswer.ToString() + " (BLADE) were released", true, false);
                else
                    oFunction.SendEmail("!!! Releasing Build Assets !!!", strEMailIdsBCC, "", "", "!!! Releasing Build Assets !!!", "The build assets for ANSWERID " + intAnswer.ToString() + " (BLADE) were NOT released", true, false);
            }
        }
        protected void btnSave_Click(Object Sender, EventArgs e)
        {
            int intResourceWorkflow = Int32.Parse(lblResourceWorkflow.Text);
            int intResourceParent = oResourceRequest.GetWorkflowParent(intResourceWorkflow);
            if (ddlStatus.SelectedIndex > -1 && txtIssues.Text.Trim() != "")
                oResourceRequest.AddStatus(intResourceWorkflow, Int32.Parse(ddlStatus.SelectedItem.Value), txtIssues.Text);
            oResourceRequest.UpdateWorkflowName(intResourceWorkflow, txtCustom.Text, intProfile);
            double dblHours = 0.00;
            dblHours += (chk1.Checked ? dbl1 : 0.00);
            dblHours += (chk3.Checked ? dbl3 : 0.00);
            dblHours += (chk4.Checked ? dbl4 : 0.00);
            dblHours += (chk5.Checked ? dbl5 : 0.00);
            dblHours += (chk6.Checked ? dbl6 : 0.00);
            dblHours += (chk7.Checked ? dbl7 : 0.00);
            dblHours += (chk8.Checked ? dbl8 : 0.00);
            double dblUsed = oResourceRequest.GetWorkflowUsed(intResourceWorkflow);
            dblHours = (dblHours - dblUsed);
            if (dblHours > 0.00)
                oResourceRequest.UpdateWorkflowHours(intResourceWorkflow, dblHours);
            oOnDemandTasks.UpdateBladeII(intRequest, intItem, intNumber, (chk1.Checked ? 1 : 0), (chk3.Checked ? 1 : 0), (chk4.Checked ? 1 : 0), (chk5.Checked ? 1 : 0), (chk6.Checked ? 1 : 0), (chk7.Checked ? 1 : 0), (chk8.Checked ? 1 : 0), (chk9.Checked ? 1 : 0), (chk10.Checked ? 1 : 0), (chk11.Checked ? 1 : 0), (chk12.Checked ? 1 : 0));
            Response.Redirect(Request.Path + "?rrid=" + intResourceWorkflow.ToString() + "&div=E&save=true");
        }
        protected void btnComplete_Click(Object Sender, EventArgs e)
        {
            int intResourceWorkflow = Int32.Parse(lblResourceWorkflow.Text);
            int intResourceParent = oResourceRequest.GetWorkflowParent(intResourceWorkflow);
            oResourceRequest.UpdateWorkflowStatus(intResourceWorkflow, 3, true);
            oOnDemandTasks.AddSuccess(intResourceParent, "Blade", Int32.Parse(ddlSuccess.SelectedItem.Value), txtComments.Text);
            oOnDemandTasks.UpdateBladeIIComplete(intRequest, intItem, intNumber);
            Forecast oForecast = new Forecast(intProfile, dsn);
            oForecast.UpdateAnswerFinished(Int32.Parse(lblAnswer.Text));
            oResourceRequest.CloseWorkflow(intResourceWorkflow, intEnvironment, 0, dsnServiceEditor, true, intResourceRequestApprove, intAssignPage, intViewPage, dsnAsset, dsnIP);
            Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "reload", "<script type=\"text/javascript\">RefreshOpeningWindow();window.close();<" + "/" + "script>");
        }
    }
}