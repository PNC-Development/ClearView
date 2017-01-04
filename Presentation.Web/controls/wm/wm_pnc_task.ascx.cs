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
    public partial class wm_pnc_task : System.Web.UI.UserControl
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
        protected int intApplicationCitrix = Int32.Parse(ConfigurationManager.AppSettings["APPLICATIONID_CITRIX"]);
        protected int intSecurityServiceID = Int32.Parse(ConfigurationManager.AppSettings["SERVICEID_SECURITY"]);
        protected int intServiceCSM = Int32.Parse(ConfigurationManager.AppSettings["SERVICEID_CSM"]);
        protected int intServiceDNS = Int32.Parse(ConfigurationManager.AppSettings["SERVICEID_PNC_DNS"]);
        protected int intServiceCluster = Int32.Parse(ConfigurationManager.AppSettings["SERVICEID_CLUSTER"]);
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
        protected Documents oDocument;
        protected OnDemandTasks oOnDemandTasks;
        protected Forecast oForecast;
        protected Variables oVariable;
        protected PNCTasks oPNCTask;
        protected Design oDesign;
        protected int intApplication = 0;
        protected int intPage = 0;
        protected int intProject = 0;
        protected bool boolDetails = false;
        protected bool boolExecution = false;
        protected bool boolChange = false;
        protected bool boolDocuments = false;
        protected int intRequest = 0;
        protected int intItem = 0;
        protected int intService = 0;
        protected int intNumber = 0;
        protected bool boolCheckboxes = false;
        protected string strCheckboxes = "";
        protected bool boolJoined = false;
        protected string strConfig = "";
        protected LTM oLTM;
        protected Servers oServer;
        protected Cluster oCluster;
        protected string strCluster = "";

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
            oDocument = new Documents(intProfile, dsn);
            oOnDemandTasks = new OnDemandTasks(intProfile, dsn);
            oForecast = new Forecast(intProfile, dsn);
            oVariable = new Variables(intEnvironment);
            oLTM = new LTM(intProfile, dsn);
            oServer = new Servers(intProfile, dsn);
            oCluster = new Cluster(intProfile, dsn);
            oPNCTask = new PNCTasks(intProfile, dsn);
            oDesign = new Design(intProfile, dsn);

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
                    bool boolSaved = false;
                    if (dblAllocated == dblUsed)
                    {
                        boolSaved = true;
                        if (boolComplete == false)
                        {
                            oFunction.ConfigureToolButton(btnComplete, "/images/tool_complete");
                            btnComplete.Attributes.Add("onclick", "return confirm('Are you sure you want to mark this as completed and remove it from your workload?') && ProcessControlButton();");
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
                    strCheckboxes = oServiceDetail.LoadCheckboxes(intRequest, intItem, intNumber, intResourceWorkflow, intService);
                    if (oService.Get(intService, "tasks") != "1" || strCheckboxes == "")
                    {
                        if (oService.Get(intService, "no_slider") == "1")
                        {
                            panNoSlider.Visible = true;
                            btnComplete.ImageUrl = "/images/tool_complete.gif";
                            btnComplete.Enabled = true;
                            btnComplete.Attributes.Add("onclick", "return confirm('Are you sure you want to mark this as completed and remove it from your workload?') && ProcessControlButton();");
                            btnComplete.CommandArgument = "FAST";
                        }
                        else
                        {
                            panSlider.Visible = true;
                            sldHours._StartPercent = dblUsed.ToString();
                            sldHours._TotalHours = dblAllocated.ToString();
                        }
                    }
                    else
                        panCheckboxes.Visible = true;
                    if (intService == intSecurityServiceID)
                        LoadSecurity();
                    if (intService == intServiceCSM)
                        LoadLTM(boolSaved);
                    if (intService == intServiceCluster)
                        LoadCluster();
                    LoadMIS(boolComplete, intResourceWorkflow);
                    intProject = oRequest.GetProjectNumber(intRequest);
                    hdnTab.Value = "D";
                    panWorkload.Visible = true;
                    bool boolRed = LoadStatus(intResourceWorkflow);
                    if (boolRed == false && boolSLABreached == true)
                        btnComplete.Attributes.Add("onclick", "alert('NOTE: Your Service Level Agreement (SLA) has been breached!\\n\\nYou must provide a RED STATUS update with an explanation of why your SLA was breached for this request.\\n\\nOnce a RED STATUS update has been provided, you will be able to complete this request.');return false;");
                    LoadChange(intResourceWorkflow);
                    LoadInformation(intResourceWorkflow);
                    chkDescription.Checked = (Request.QueryString["doc"] != null);
                    lblDocuments.Text = oDocument.GetDocuments_Service(intRequest, intService, oVariable.DocumentsFolder(), 1, (Request.QueryString["doc"] != null));
                    btnDenied.Attributes.Add("onclick", "return CloseWindow();");
                    oFunction.ConfigureToolButton(btnSave, "/images/tool_save");
                    oFunction.ConfigureToolButton(btnPrint, "/images/tool_print");
                    btnPrint.Attributes.Add("onclick", "return PrintWindow();");
                    oFunction.ConfigureToolButton(btnClose, "/images/tool_close");
                    btnClose.Attributes.Add("onclick", "return ExitWindow();");
                    //btnSave.Attributes.Add("onclick", "return ValidateStatus('" + ddlStatus.ClientID + "','" + txtComments.ClientID + "');");
                    btnSave.Attributes.Add("onclick", "return ProcessControlButton();");
                    btnChange.Attributes.Add("onclick", "return ValidateText('" + txtNumber.ClientID + "','Please enter a change control number')" +
                        " && ValidateDate('" + txtDate.ClientID + "','Please enter a valid implementation date')" +
                        " && ValidateTime('" + txtTime.ClientID + "','Please enter a valid implementation time')" +
                        " && ProcessControlButton()" +
                        ";");
                    imgDate.Attributes.Add("onclick", "return ShowCalendar('" + txtDate.ClientID + "');");
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
        private void LoadSecurity()
        {
            ModelsProperties oModelsProperties = new ModelsProperties(intProfile, dsn);
            Servers oServer = new Servers(intProfile, dsn);
            OnDemand oOnDemand = new OnDemand(0, dsn);

            int intAnswer = 0;
            int intModel = 0;
            DataSet dsSecurity = oOnDemandTasks.GetServerOther(intRequest, intService, intNumber);
            if (dsSecurity.Tables[0].Rows.Count > 0)
            {
                intAnswer = Int32.Parse(dsSecurity.Tables[0].Rows[0]["answerid"].ToString());
                intModel = Int32.Parse(dsSecurity.Tables[0].Rows[0]["modelid"].ToString());
            }
            //intAnswer = 2681;
            //intModel = 395;
            int intStep = (oModelsProperties.IsTypeVMware(intModel) ? 6 : 4);
            if (intAnswer > 0)
            {
                panSecurity.Visible = true;
                string strRequestResult = "";

                // PRE (creation of groups)
                DataSet dsServers = oServer.GetAnswer(intAnswer);
                foreach (DataRow drServer in dsServers.Tables[0].Rows)
                {
                    strRequestResult += "<tr>";
                    int intServer = Int32.Parse(drServer["id"].ToString());
                    strRequestResult += "<td valign=\"top\"><b>" + oServer.GetName(intServer, true) + ":</b></td>";
                    DataSet dsStep = oOnDemand.GetStepDoneServer(intServer, intStep);
                    if (dsStep.Tables[0].Rows.Count > 0)
                        strRequestResult += "<td valign=\"top\">" + dsStep.Tables[0].Rows[0]["result"].ToString() + "</td>";
                    strRequestResult += "</tr>";
                }
                if (strRequestResult != "")
                    strRequestResult = "<table cellpadding=\"3\" cellspacing=\"2\" border=\"0\">" + strRequestResult + "</table>";
                lblSecurityPre.Text = strRequestResult;

                // POST (account generation and ties to groups)
                strRequestResult = "";
                int intRequestAD = oForecast.GetRequestID(intAnswer, true);
                DataSet dsRequestResult = oRequest.GetResult(intRequestAD);
                foreach (DataRow drRequestResult in dsRequestResult.Tables[0].Rows)
                    strRequestResult += drRequestResult["result"].ToString();
                lblSecurityPost.Text = strRequestResult;
            }
        }
        private void LoadLTM(bool _ready_to_complete)
        {
            int intAnswer = 0;
            int intModel = 0;
            DataSet ds = oOnDemandTasks.GetServerOther(intRequest, intService, intNumber);
            if (ds.Tables[0].Rows.Count > 0)
            {
                intAnswer = Int32.Parse(ds.Tables[0].Rows[0]["answerid"].ToString());
                intModel = Int32.Parse(ds.Tables[0].Rows[0]["modelid"].ToString());
            }
            if (_ready_to_complete == true)
            {
                panLTMStatusYes.Visible = true;
                btnLTM.Attributes.Add("onclick", "return ProcessButton(this);");
            }
            else
            {
                panLTMStatusNo.Visible = true;
                btnLTM.Enabled = false;
            }
            bool boolCompleted = true;
            bool boolError = false;
            bool boolFoundConfig = false;
            if (intAnswer > 0)
            {
                panLTM.Visible = true;
                DataSet dsConfig = oLTM.GetConfig(intAnswer);
                if (dsConfig.Tables[0].Rows.Count > 0)
                {
                    boolFoundConfig = true;
                    DataRow drConfig = dsConfig.Tables[0].Rows[0];
                    lblVipName.Text = drConfig["name"].ToString();
                    lblVip.Text = drConfig["ip1"].ToString() + "." + drConfig["ip2"].ToString() + "." + drConfig["ip3"].ToString() + "." + drConfig["ip4"].ToString();
                    string strPath = drConfig["path"].ToString();
                    if (strPath != "")
                    {
                        trConfig.Visible = true;
                        btnConfig.NavigateUrl = strPath;
                        btnConfig.ToolTip = strPath;
                    }
                    if (drConfig["completed"].ToString() == "")
                        boolCompleted = false;
                    if (drConfig["installed"].ToString() == "" && _ready_to_complete == true)
                        oLTM.UpdateConfigInstalled(intAnswer, DateTime.Now);
                    string strResult = drConfig["result"].ToString();
                    if (strResult == "")
                        lblLTM.Text = "<i>Pending...</i>";
                    else 
                    {
                        if (strResult.ToUpper().Contains("ERROR") == true)
                            boolError = true;
                        lblLTM.Text = strResult;
                    }
                }
                else
                    boolCompleted = false;
                DataSet dsAnswer = oServer.GetAnswer(intAnswer);
                foreach (DataRow dr in dsAnswer.Tables[0].Rows)
                {
                    int intServer = Int32.Parse(dr["id"].ToString());
                    string strName = oServer.GetName(intServer, true);
                    strConfig += "<tr>";
                    strConfig += "<td>" + strName + "</td>";
                    string strIP1 = "";
                    string strIP2 = "";
                    string strIP3 = "";
                    string strIP4 = "";
                    string strVlan = "";
                    DataSet dsConfigs = oLTM.GetConfigs(intServer);
                    if (dsConfigs.Tables[0].Rows.Count > 0)
                    {
                        DataRow drConfigs = dsConfigs.Tables[0].Rows[0];
                        strIP1 = drConfigs["ip1"].ToString();
                        strIP2 = drConfigs["ip2"].ToString();
                        strIP3 = drConfigs["ip3"].ToString();
                        strIP4 = drConfigs["ip4"].ToString();
                        strVlan = drConfigs["vlan"].ToString();
                        if (drConfigs["completed"].ToString() == "")
                            boolCompleted = false;
                        if (drConfigs["installed"].ToString() == "" && _ready_to_complete == true)
                            oLTM.UpdateConfigsInstalled(intServer, DateTime.Now);
                    }
                    strConfig += "<td>" + strIP1 + "." + strIP2 + "." + strIP3 + "." + strIP4 + "</td>";
                    strConfig += "<td>" + strVlan + "</td>";
                    strConfig += "</tr>";

                    if (_ready_to_complete == true)
                    {
                        DataSet dsLTM = oLTM.GetConfigsResult(intAnswer);
                        rptLTM.DataSource = dsLTM;
                        rptLTM.DataBind();
                        foreach (DataRow drLTM in dsLTM.Tables[0].Rows)
                        {
                            if (drLTM["error"].ToString() == "1")
                            {
                                boolError = true;
                                break;
                            }
                        }
                    }
                }
            }
            else
                boolCompleted = false;

            if (_ready_to_complete == true)
            {
                if (boolError == true)
                    panLTMStatusError.Visible = true;
                else
                {
                    if (boolFoundConfig == true)
                    {
                        if (boolCompleted == false)
                        {
                            panLTMStatusPending.Visible = true;
                            btnComplete.ImageUrl = "/images/tool_complete_dbl.gif";
                            btnComplete.Enabled = false;
                        }
                        else
                            panLTMStatusDone.Visible = true;
                    }
                    else
                        panLTM.Visible = false;
                }
            }
        }
        private void LoadCluster()
        {
            panCluster.Visible = true;
            int intAnswer = 0;
            int intModel = 0;
            DataSet dsSecurity = oOnDemandTasks.GetServerOther(intRequest, intService, intNumber);
            if (dsSecurity.Tables[0].Rows.Count > 0)
            {
                intAnswer = Int32.Parse(dsSecurity.Tables[0].Rows[0]["answerid"].ToString());
                intModel = Int32.Parse(dsSecurity.Tables[0].Rows[0]["modelid"].ToString());
            }
            //intAnswer = 12227;
            DataSet dsCluster = oServer.GetAnswerClusters(intAnswer);
            foreach (DataRow drCluster in dsCluster.Tables[0].Rows)
            {
                int intCluster = Int32.Parse(drCluster["clusterid"].ToString());
                strCluster += "<tr><td>Cluster Nickname:</td><td>" + oCluster.Get(intCluster, "nickname") + "</td></tr>";
                strCluster += "<tr><td>Cluster Name:</td><td><input type=\"text\" class=\"default\" style=\"width:300px\" onblur=\"UpdateTextValue(this,'HDN_" + intCluster.ToString() + "');\" value=\"" + oCluster.Get(intCluster, "name") + "\"></td></tr>";
                strCluster += "<input type=\"hidden\" name=\"HDN_" + intCluster.ToString() + "\" id=\"HDN_" + intCluster.ToString() + "\" value=\"" + oCluster.Get(intCluster, "name") + "\" />";
                string strNames = "";
                if (intCluster > 0)
                {
                    DataSet dsServers = oServer.GetClusters(intCluster);
                    foreach (DataRow drServer in dsServers.Tables[0].Rows)
                    {
                        int intServer = Int32.Parse(drServer["id"].ToString());
                        if (strNames != "")
                            strNames += ", ";
                        strNames += oServer.GetName(intServer, true);
                    }
                }
                strCluster += "<tr><td>Server Name(s):</td><td>" + strNames + "</td></tr>";
                strCluster += "<tr><td colspan=\"2\">&nbsp;</td></tr>";
            }
        }
        private void LoadMIS(bool _complete, int _rrwid)
        {
            int intAnswer = 0;
            int intModel = 0;
            DataSet dsTasks = oOnDemandTasks.GetServerOther(intRequest, intService, intNumber);
            if (dsTasks.Tables[0].Rows.Count > 0)
            {
                intAnswer = Int32.Parse(dsTasks.Tables[0].Rows[0]["answerid"].ToString());
                intModel = Int32.Parse(dsTasks.Tables[0].Rows[0]["modelid"].ToString());
            }
            if (intAnswer > 0)
            {
                DataSet ds = oPNCTask.GetAnswer(intAnswer);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (_rrwid == Int32.Parse(dr["rrwid"].ToString()))
                    {
                        int intTask = Int32.Parse(dr["taskid"].ToString());
                        if (intTask > 0)
                        {
                            if (oPNCTask.Get(intTask, "client") == "1")
                            {
                                // MIS - show / hide accept build.
                                panMIS.Visible = true;
                                DataSet dsDesign = oDesign.GetAnswer(intAnswer);
                                if (dsDesign.Tables[0].Rows.Count > 0)
                                {
                                    radAccept.Checked = (dsDesign.Tables[0].Rows[0]["mis_approved"].ToString() != "");
                                    radReject.Checked = (dsDesign.Tables[0].Rows[0]["mis_rejected"].ToString() != "");
                                }

                                if (_complete == false)
                                {
                                    oFunction.ConfigureToolButton(btnComplete, "/images/tool_complete");
                                    btnComplete.Attributes.Add("onclick", "return ValidateCheckListsNoFocus('chkTaskList" + _rrwid.ToString() + "','You must check off on all the required tasks before you can complete this request') && ValidateRadioButtons('" + radAccept.ClientID + "','" + radReject.ClientID + "','Please select whether or not you would like to accept or reject the design in its currently provisioned state') && confirm('Are you sure you want to mark this as completed and remove it from your workload?') && ProcessControlButton();");
                                }
                                else
                                    btnComplete.Attributes.Add("onclick", "alert('This task has already been marked COMPLETE. You can close this window.');return false;");
                            }
                        }
                    }
                }
            }

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
            if (Request.QueryString["div"] != null)
            {
                switch (Request.QueryString["div"])
                {
                    case "E":
                        boolExecution = true;
                        break;
                    case "C":
                        boolChange = true;
                        break;
                    case "D":
                        boolDocuments = true;
                        break;
                }
            }
            if (boolDetails == false && boolExecution == false && boolChange == false && boolDocuments == false)
                boolDetails = true;
        }
        protected void btnStatus_Click(Object Sender, EventArgs e)
        {
            int intResourceWorkflow = Int32.Parse(lblResourceWorkflow.Text);
            int intResourceParent = oResourceRequest.GetWorkflowParent(intResourceWorkflow);
            oResourceRequest.AddStatus(intResourceWorkflow, Int32.Parse(ddlStatus.SelectedItem.Value), txtComments.Text, intProfile);
            Response.Redirect(Request.Path + "?rrid=" + intResourceWorkflow.ToString() + "&div=E&status=true");
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
        protected void chkDescription_Change(Object Sender, EventArgs e)
        {
            if (chkDescription.Checked == true)
                Response.Redirect(Request.Path + "?rrid=" + Request.QueryString["rrid"] + "&doc=true&div=D");
            else
                Response.Redirect(Request.Path + "?rrid=" + Request.QueryString["rrid"] + "&div=D");
        }
        protected void btnLTM_Click(Object Sender, EventArgs e)
        {
            int intResourceWorkflow = Int32.Parse(lblResourceWorkflow.Text);
            Response.Redirect(Request.Path + "?rrid=" + intResourceWorkflow.ToString() + "&div=E");
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
            if (panCluster.Visible == true)
            {
                foreach (string strForm in Request.Form)
                {
                    if (strForm.StartsWith("HDN_") == true)
                    {
                        string strValue = strForm.Substring(4);
                        int intCluster = Int32.Parse(strValue);
                        oCluster.UpdateName(intCluster, Request.Form["HDN_" + intCluster.ToString()]);
                    }
                }
            }
            SaveMIS();
            if (panNoSlider.Visible == false)
            {
                double dblHours = 0.00;
                if (panSlider.Visible == true)
                {
                    if (Request.Form["hdnHours"] != null && Request.Form["hdnHours"] != "")
                        dblHours = double.Parse(Request.Form["hdnHours"]);
                    double dblUsed = oResourceRequest.GetWorkflowUsed(intResourceWorkflow);
                    dblHours = (dblHours - dblUsed);
                    if (dblHours > 0.00)
                        oResourceRequest.UpdateWorkflowHours(intResourceWorkflow, dblHours);
                }
                else
                {
                    oServiceDetail.UpdateCheckboxes(Request, intResourceWorkflow, intRequest, intItem, intNumber);
                    double dblAllocated = oResourceRequest.GetDetailsHoursUsed(intRequest, intItem, intNumber, intResourceWorkflow, false);
                    oResourceRequest.UpdateWorkflowAllocated(intResourceWorkflow, dblAllocated);
                }
            }
            oResourceRequest.UpdateWorkflowName(intResourceWorkflow, txtCustom.Text, intProfile);
            Response.Redirect(Request.Path + "?rrid=" + intResourceWorkflow.ToString() + "&div=E&save=true");
        }
        protected void btnComplete_Click(Object Sender, EventArgs e)
        {
            int intResourceWorkflow = Int32.Parse(lblResourceWorkflow.Text);
            int intResourceParent = oResourceRequest.GetWorkflowParent(intResourceWorkflow);
            if (panNoSlider.Visible == true)
            {
                double dblAllocated = double.Parse(oResourceRequest.GetWorkflow(intResourceWorkflow, "allocated"));
                oResourceRequest.UpdateWorkflowHoursOverwrite(intResourceWorkflow, dblAllocated);
            }
            // Add a green / completed status if there are no updates, OR the last status is not green
            DataSet dsStatus = oResourceRequest.GetStatuss(intResourceWorkflow);
            if (dsStatus.Tables[0].Rows.Count == 0 || dsStatus.Tables[0].Rows[0]["status"].ToString() != "3")
                oResourceRequest.AddStatus(intResourceWorkflow, 3, "Completed", intProfile);

            int intAnswer = 0;
            int intModel = 0;
            DataSet ds = oOnDemandTasks.GetServerOther(intRequest, intService, intNumber);
            if (ds.Tables[0].Rows.Count > 0)
            {
                intAnswer = Int32.Parse(ds.Tables[0].Rows[0]["answerid"].ToString());
                intModel = Int32.Parse(ds.Tables[0].Rows[0]["modelid"].ToString());
            }
            else if (intService == intServiceDNS)
            {
                Customized oCustomized = new Customized(0, dsn);
                ds = oCustomized.GetPNCDNSConflict(intRequest, intItem, intNumber);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    intAnswer = Int32.Parse(ds.Tables[0].Rows[0]["answerid"].ToString());
                    intModel = oForecast.GetModelAsset(intAnswer);
                    if (intModel == 0)
                        intModel = oForecast.GetModel(intAnswer);
                    oCustomized.UpdatePNCDNSConflict(intRequest, intItem, intNumber);
                }
            }
            SaveMIS();
            oOnDemandTasks.UpdateServerOtherComplete(intRequest, intService, intNumber);
            oResourceRequest.UpdateWorkflowStatus(intResourceWorkflow, 3, true);

            //oPNCTask.InitiateNextStep(intRequest, intService, intNumber, intAnswer, intModel, intEnvironment, intApplicationCitrix, intAssignPage, intViewPage, dsnAsset, dsnIP, dsnServiceEditor, false, 0);
            oPNCTask.InitiateNextStep(intRequest, intAnswer, intModel, intEnvironment, intAssignPage, intViewPage, dsnAsset, dsnIP, dsnServiceEditor);


            oResourceRequest.CloseWorkflow(intResourceWorkflow, intEnvironment, 0, dsnServiceEditor, true, intResourceRequestApprove, intAssignPage, intViewPage, dsnAsset, dsnIP);
            Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "reload", "<script type=\"text/javascript\">RefreshOpeningWindow();window.close();<" + "/" + "script>");
        }
        private void SaveMIS()
        {
            if (panMIS.Visible == true)
            {
                int intAnswer = 0;
                DataSet ds = oOnDemandTasks.GetServerOther(intRequest, intService, intNumber);
                if (ds.Tables[0].Rows.Count > 0)
                    intAnswer = Int32.Parse(ds.Tables[0].Rows[0]["answerid"].ToString());
                DataSet dsDesign = oDesign.GetAnswer(intAnswer);
                if (dsDesign.Tables[0].Rows.Count > 0)
                {
                    int intDesign = Int32.Parse(dsDesign.Tables[0].Rows[0]["id"].ToString());
                    if (radAccept.Checked)
                        oDesign.UpdateMISApproved(intDesign, DateTime.Now.ToString());
                    if (radReject.Checked)
                        oDesign.UpdateMISRejected(intDesign, DateTime.Now.ToString());
                }
            }
        }
    }
}
