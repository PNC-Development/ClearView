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
using System.Net.NetworkInformation;
using NCC.ClearView.Application.Core.ClearViewWS;
namespace NCC.ClearView.Presentation.Web
{
    public partial class wm_ltm : System.Web.UI.UserControl
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
        protected string strBCC = ConfigurationManager.AppSettings["WorkflowBCC"];
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
        protected string strValidation = "";
        protected string strConfig = "";
        protected string strHiddenV = "";
        protected LTM oLTM;
        protected Servers oServer;
        private bool boolValidatePing = false;
        private bool boolValidateDNS = false;
        private bool boolValidateDB = false;
        private bool boolValidateFormat = false;

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

                if (!IsPostBack)
                {
                    if (Request.QueryString["save"] != null && Request.QueryString["save"] != "")
                        Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "saved", "<script type=\"text/javascript\">alert('Information Saved Successfully');<" + "/" + "script>");
                    if (Request.QueryString["status"] != null && Request.QueryString["status"] != "")
                        Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "statusd", "<script type=\"text/javascript\">alert('Weekly Status has been Added');<" + "/" + "script>");
                    double dblUsed = oResourceRequest.GetWorkflowUsed(intResourceWorkflow);
                    lblUpdated.Text = oResourceRequest.GetUpdated(intResourceParent);
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
                    LoadLTM();
                    btnComplete.ImageUrl = "/images/tool_complete.gif";
                    btnComplete.Enabled = true;
                    btnComplete.Attributes.Add("onclick", "return EnsureLTMok('" + strValidation + "','" + txtVipName.ClientID + "','" + txtVipIP1.ClientID + "','" + txtVipIP2.ClientID + "','" + txtVipIP3.ClientID + "','" + txtVipIP4.ClientID + "') && confirm('Are you sure you want to mark this as completed and remove it from your workload?') && AlertMessage('This action requires validation that might take a few minutes to complete...\\n\\nPlease be patient and wait for the window to finish processing...') && ProcessControlButton();;");
                    btnComplete.CommandArgument = "FAST";
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
                    btnSave.Attributes.Add("onclick", "return ValidateStatus('" + ddlStatus.ClientID + "','" + txtComments.ClientID + "') && ProcessControlButton();");
                    btnChange.Attributes.Add("onclick", "return ValidateText('" + txtNumber.ClientID + "','Please enter a change control number')" +
                        " && ValidateDate('" + txtDate.ClientID + "','Please enter a valid implementation date')" +
                        " && ValidateTime('" + txtTime.ClientID + "','Please enter a valid implementation time')" +
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
        private void LoadLTM()
        {
            int intAnswer = 0;
            int intModel = 0;
            DataSet ds = oOnDemandTasks.GetServerOther(intRequest, intService, intNumber);
            if (ds.Tables[0].Rows.Count > 0)
            {
                intAnswer = Int32.Parse(ds.Tables[0].Rows[0]["answerid"].ToString());
                intModel = Int32.Parse(ds.Tables[0].Rows[0]["modelid"].ToString());
            }
            if (intAnswer > 0)
            {
                DataSet dsConfig = oLTM.GetConfig(intAnswer);
                if (dsConfig.Tables[0].Rows.Count > 0)
                {
                    DataRow drConfig = dsConfig.Tables[0].Rows[0];
                    txtVipName.Text = drConfig["name"].ToString();
                    txtVipIP1.Text = drConfig["ip1"].ToString();
                    txtVipIP2.Text = drConfig["ip2"].ToString();
                    txtVipIP3.Text = drConfig["ip3"].ToString();
                    txtVipIP4.Text = drConfig["ip4"].ToString();
                    string strPath = drConfig["path"].ToString();
                    if (strPath != "")
                    {
                        trConfig.Visible = true;
                        btnConfig.NavigateUrl = strPath;
                        btnConfig.ToolTip = strPath;
                    }
                }
                bool boolOther = false;
                DataSet dsAnswer = oServer.GetAnswer(intAnswer);
                foreach (DataRow dr in dsAnswer.Tables[0].Rows)
                {
                    boolOther = !boolOther;
                    int intServer = Int32.Parse(dr["id"].ToString());
                    string strName = oServer.GetName(intServer, true);
                    strConfig += "<tr id=\"TR_" + intServer.ToString() + "\"" + (boolOther == false ? " bgcolor=\"#F6F6F6\"" : "") + ">";
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
                    }
                    strConfig += "<td><input type=\"text\" id=\"TXT_" + intServer.ToString() + "_IP1\" style=\"width:35px\" class=\"default\" value=\"" + strIP1 + "\" onblur=\"UpdateTextBox(this,'HDN_" + intServer.ToString() + "_IP1');\" maxlength=\"3\"/></td>";
                    strConfig += "<td>.</td>";
                    strConfig += "<td><input type=\"text\" id=\"TXT_" + intServer.ToString() + "_IP2\" style=\"width:35px\" class=\"default\" value=\"" + strIP2 + "\" onblur=\"UpdateTextBox(this,'HDN_" + intServer.ToString() + "_IP2');\" maxlength=\"3\"/></td>";
                    strConfig += "<td>.</td>";
                    strConfig += "<td><input type=\"text\" id=\"TXT_" + intServer.ToString() + "_IP3\" style=\"width:35px\" class=\"default\" value=\"" + strIP3 + "\" onblur=\"UpdateTextBox(this,'HDN_" + intServer.ToString() + "_IP3');\" maxlength=\"3\"/></td>";
                    strConfig += "<td>.</td>";
                    strConfig += "<td><input type=\"text\" id=\"TXT_" + intServer.ToString() + "_IP4\" style=\"width:35px\" class=\"default\" value=\"" + strIP4 + "\" onblur=\"UpdateTextBox(this,'HDN_" + intServer.ToString() + "_IP4');\" maxlength=\"3\"/></td>";
                    strConfig += "<td><input type=\"text\" id=\"TXT_" + intServer.ToString() + "_VLAN\" style=\"width:70px\" class=\"default\" value=\"" + strVlan + "\" onblur=\"UpdateTextBox(this,'HDN_" + intServer.ToString() + "_VLAN');\" maxlength=\"4\"/></td>";
                    strConfig += "</tr>";
                    strHiddenV += "<input type=\"hidden\" name=\"HDN__" + intServer.ToString() + "\" id=\"HDN__" + intServer.ToString() + "\" value=\"" + intServer.ToString() + "\" />";
                    strHiddenV += "<input type=\"hidden\" name=\"HDN_" + intServer.ToString() + "_IP1\" id=\"HDN_" + intServer.ToString() + "_IP1\" value=\"" + strIP1 + "\" />";
                    strHiddenV += "<input type=\"hidden\" name=\"HDN_" + intServer.ToString() + "_IP2\" id=\"HDN_" + intServer.ToString() + "_IP2\" value=\"" + strIP2 + "\" />";
                    strHiddenV += "<input type=\"hidden\" name=\"HDN_" + intServer.ToString() + "_IP3\" id=\"HDN_" + intServer.ToString() + "_IP3\" value=\"" + strIP3 + "\" />";
                    strHiddenV += "<input type=\"hidden\" name=\"HDN_" + intServer.ToString() + "_IP4\" id=\"HDN_" + intServer.ToString() + "_IP4\" value=\"" + strIP4 + "\" />";
                    strHiddenV += "<input type=\"hidden\" name=\"HDN_" + intServer.ToString() + "_VLAN\" id=\"HDN_" + intServer.ToString() + "_VLAN\" value=\"" + strVlan + "\" />";
                    strValidation += intServer.ToString() + ";";
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
        protected void btnSave_Click(Object Sender, EventArgs e)
        {
            int intResourceWorkflow = Int32.Parse(lblResourceWorkflow.Text);
            int intResourceParent = oResourceRequest.GetWorkflowParent(intResourceWorkflow);
            if (ddlStatus.SelectedIndex > -1 && txtComments.Text.Trim() != "")
                oResourceRequest.AddStatus(intResourceWorkflow, Int32.Parse(ddlStatus.SelectedItem.Value), txtComments.Text, intProfile);
            double dblAllocated = double.Parse(oResourceRequest.GetWorkflow(intResourceWorkflow, "allocated"));
            oResourceRequest.UpdateWorkflowHoursOverwrite(intResourceWorkflow, dblAllocated);
            oResourceRequest.UpdateWorkflowName(intResourceWorkflow, txtCustom.Text, intProfile);
            int intAnswer = 0;
            DataSet ds = oOnDemandTasks.GetServerOther(intRequest, intService, intNumber);
            if (ds.Tables[0].Rows.Count > 0)
                intAnswer = Int32.Parse(ds.Tables[0].Rows[0]["answerid"].ToString());
            string strSaveError = SaveConfigs(intAnswer, chkValidate.Checked);
            if (strSaveError == "")
                Response.Redirect(Request.Path + "?rrid=" + intResourceWorkflow.ToString() + "&div=E&save=true");
            else
                GetError(strSaveError);
        }
        protected void btnComplete_Click(Object Sender, EventArgs e)
        {
            int intResourceWorkflow = Int32.Parse(lblResourceWorkflow.Text);
            int intResourceParent = oResourceRequest.GetWorkflowParent(intResourceWorkflow);
            int intAnswer = 0;
            int intModel = 0;
            DataSet ds = oOnDemandTasks.GetServerOther(intRequest, intService, intNumber);
            if (ds.Tables[0].Rows.Count > 0)
            {
                intAnswer = Int32.Parse(ds.Tables[0].Rows[0]["answerid"].ToString());
                intModel = Int32.Parse(ds.Tables[0].Rows[0]["modelid"].ToString());
            }
            string strSaveError = SaveConfigs(intAnswer, true);
            if (strSaveError == "")
            {
                // Add a green / completed status if there are no updates, OR the last status is not green
                DataSet dsStatus = oResourceRequest.GetStatuss(intResourceWorkflow);
                if (dsStatus.Tables[0].Rows.Count == 0 || dsStatus.Tables[0].Rows[0]["status"].ToString() != "3")
                    oResourceRequest.AddStatus(intResourceWorkflow, 3, "Completed", intProfile);

                oOnDemandTasks.UpdateServerOtherComplete(intRequest, intService, intNumber);
                oResourceRequest.UpdateWorkflowStatus(intResourceWorkflow, 3, true);

                PNCTasks oPNCTask = new PNCTasks(0, dsn);
                //oPNCTask.InitiateNextStep(intRequest, intService, intNumber, intAnswer, intModel, intEnvironment, intApplicationCitrix, intAssignPage, intViewPage, dsnAsset, dsnIP, dsnServiceEditor, false, 50);
                oPNCTask.InitiateNextStep(intRequest, intAnswer, intModel, intEnvironment, intAssignPage, intViewPage, dsnAsset, dsnIP, dsnServiceEditor);

                oResourceRequest.CloseWorkflow(intResourceWorkflow, intEnvironment, 0, dsnServiceEditor, true, intResourceRequestApprove, intAssignPage, intViewPage, dsnAsset, dsnIP);
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "reload", "<script type=\"text/javascript\">RefreshOpeningWindow();window.close();<" + "/" + "script>");
            }
            else
                GetError(strSaveError);
        }
        private string SaveConfigs(int _answerid, bool _complete)
        {
            string strServerError = "";
            if (_answerid > 0)
            {
                string strPath = btnConfig.ToolTip;
                if (txtFile.FileName != "" && txtFile.PostedFile != null)
                {
                    string strDirectory = oVariable.DocumentsFolder() + "ltm";
                    if (Directory.Exists(strDirectory) == false)
                        Directory.CreateDirectory(strDirectory);
                    string strFile = txtFile.PostedFile.FileName.Trim();
                    string strFileName = strFile.Substring(strFile.LastIndexOf("\\") + 1);
                    string strExtension = txtFile.FileName;
                    strExtension = strExtension.Substring(strExtension.LastIndexOf("."));
                    strPath = strDirectory + "\\" + strFileName;
                    txtFile.PostedFile.SaveAs(strPath);
                }

                // Validation
                if (_complete == true)
                {
                    int intIP1 = 0;
                    Int32.TryParse(txtVipIP1.Text, out intIP1);
                    int intIP2 = 0;
                    Int32.TryParse(txtVipIP2.Text, out intIP2);
                    int intIP3 = 0;
                    Int32.TryParse(txtVipIP3.Text, out intIP3);
                    int intIP4 = 0;
                    Int32.TryParse(txtVipIP4.Text, out intIP4);

                    bool boolVip = CheckIP(_answerid, intIP1, intIP2, intIP3, intIP4);
                    if (boolVip == true)
                    {
                        foreach (string strForm in Request.Form)
                        {
                            if (strForm.StartsWith("HDN__") == true)
                            {
                                int intServer = Int32.Parse(strForm.Substring(5));
                                intIP1 = 0;
                                Int32.TryParse(Request.Form["HDN_" + intServer.ToString() + "_IP1"], out intIP1);
                                intIP2 = 0;
                                Int32.TryParse(Request.Form["HDN_" + intServer.ToString() + "_IP2"], out intIP2);
                                intIP3 = 0;
                                Int32.TryParse(Request.Form["HDN_" + intServer.ToString() + "_IP3"], out intIP3);
                                intIP4 = 0;
                                Int32.TryParse(Request.Form["HDN_" + intServer.ToString() + "_IP4"], out intIP4);
                                bool boolIP = CheckIP(_answerid, intIP1, intIP2, intIP3, intIP4);
                                if (boolIP == false)
                                    strServerError = intServer.ToString();
                            }
                        }
                    }
                    else
                        strServerError = "0";
                }

                if (_complete == false || strServerError == "")
                {
                    oLTM.DeleteConfig(_answerid);
                    int intIP1 = 0;
                    Int32.TryParse(txtVipIP1.Text, out intIP1);
                    int intIP2 = 0;
                    Int32.TryParse(txtVipIP2.Text, out intIP2);
                    int intIP3 = 0;
                    Int32.TryParse(txtVipIP3.Text, out intIP3);
                    int intIP4 = 0;
                    Int32.TryParse(txtVipIP4.Text, out intIP4);
                    oLTM.AddConfig(_answerid, txtVipName.Text, intIP1, intIP2, intIP3, intIP4, strPath, intProfile);

                    foreach (string strForm in Request.Form)
                    {
                        if (strForm.StartsWith("HDN__") == true)
                        {
                            int intServer = Int32.Parse(strForm.Substring(5));
                            oLTM.DeleteConfigs(intServer);
                            intIP1 = 0;
                            Int32.TryParse(Request.Form["HDN_" + intServer.ToString() + "_IP1"], out intIP1);
                            intIP2 = 0;
                            Int32.TryParse(Request.Form["HDN_" + intServer.ToString() + "_IP2"], out intIP2);
                            intIP3 = 0;
                            Int32.TryParse(Request.Form["HDN_" + intServer.ToString() + "_IP3"], out intIP3);
                            intIP4 = 0;
                            Int32.TryParse(Request.Form["HDN_" + intServer.ToString() + "_IP4"], out intIP4);
                            int intVLAN = 0;
                            Int32.TryParse(Request.Form["HDN_" + intServer.ToString() + "_VLAN"], out intVLAN);
                            oLTM.AddConfigs(intServer, intIP1, intIP2, intIP3, intIP4, intVLAN);
                        }
                    }
                }
            }
            return strServerError;
        }
        private bool CheckIP(int _answerid, int _ip1, int _ip2, int _ip3, int _ip4)
        {
            if (_ip1 > 0 && _ip1 < 256 && _ip2 > 0 && _ip2 < 256 && _ip3 > 0 && _ip3 < 256 && _ip4 > 0 && _ip4 < 256)
            {
                string strIP = _ip1.ToString() + "." + _ip2.ToString() + "." + _ip3.ToString() + "." + _ip4.ToString();
                //  Make sure PING times out (that it is not in use)
                Ping oPing = new Ping();
                string strStatus = "";
                try
                {
                    PingReply oReply = oPing.Send(strIP);
                    strStatus = oReply.Status.ToString().ToUpper();
                }
                catch { }
                if (strStatus == "SUCCESS")
                {
                    boolValidatePing = true;
                    return false;
                }


                // Make sure that this IP address does not exist in DNS (only if PNC)
                Classes oClass = new Classes(intProfile, dsn);
                int intClass = Int32.Parse(oForecast.GetAnswer(_answerid, "classid"));
                if (oClass.Get(intClass, "pnc") == "1")
                {
                    Variables oVariableWS = new Variables(intEnvironment);
                    System.Net.NetworkCredential oCredentials = new System.Net.NetworkCredential(oVariableWS.ADUser(), oVariableWS.ADPassword(), oVariableWS.Domain());
                    ClearViewWebServices oWS = new ClearViewWebServices();
                    oWS.Timeout = Int32.Parse(ConfigurationManager.AppSettings["WS_TIMEOUT"]);
                    oWS.Credentials = oCredentials;
                    oWS.Url = oVariableWS.WebServiceURL();
                    Settings oSetting = new Settings(0, dsn);
                    bool boolDNS_QIP = oSetting.IsDNS_QIP();
                    bool boolDNS_Bluecat = oSetting.IsDNS_Bluecat();
                    if (boolDNS_QIP == true)
                    {
                        string strSearchName = oWS.SearchDNSforPNC(strIP, "", false, true);
                        if (strSearchName.Trim().ToUpper() != "***NOTFOUND")
                        {
                            boolValidateDNS = true;
                            return false;
                        }
                    }
                    if (boolDNS_Bluecat == true)
                    {
                        string strSearchName = oWS.SearchBluecatDNS(strIP, "");
                        if (strSearchName.Trim().ToUpper() != "***NOTFOUND")
                        {
                            boolValidateDNS = true;
                            return false;
                        }
                    }
                }


                // Make sure it is not already assigned in database
                IPAddresses oIPAddresses = new IPAddresses(intProfile, dsnIP, dsn);
                DataSet dsIP = oIPAddresses.Get(_ip1, _ip2, _ip3, _ip4);
                foreach (DataRow drIP in dsIP.Tables[0].Rows)
                {
                    if (drIP["available"].ToString() == "0")
                    {
                        boolValidateDB = true;
                        return false;
                        break;
                    }
                }

                return true;
            }
            else
            {
                boolValidateFormat = true;
                return false;
            }
        }
        private void GetError(string _error)
        {
            boolExecution = true;
            if (_error == "0")
                trVip.Attributes.Add("bgcolor", "#FFF68F");
            else
                trVip.Attributes.Remove("bgcolor");
            if (boolValidatePing)
                Page.ClientScript.RegisterStartupScript(typeof(Page), "validation", "<script type=\"text/javascript\">alert('The highlighted IP Address is already in use and responds on the network');<" + "/" + "script>");
            else if (boolValidateDNS)
                Page.ClientScript.RegisterStartupScript(typeof(Page), "validation", "<script type=\"text/javascript\">alert('The highlighted IP Address already exists in PNC DNS');<" + "/" + "script>");
            else if (boolValidateDB)
                Page.ClientScript.RegisterStartupScript(typeof(Page), "validation", "<script type=\"text/javascript\">alert('The highlighted IP Address already exists in the IP Address database and cannot be used again');<" + "/" + "script>");
            else if (boolValidateFormat)
                Page.ClientScript.RegisterStartupScript(typeof(Page), "validation", "<script type=\"text/javascript\">alert('The highlighted IP Address is in an invalid format');<" + "/" + "script>");
            else
                Page.ClientScript.RegisterStartupScript(typeof(Page), "validation", "<script type=\"text/javascript\">alert('The highlighted IP Address caused an unknown validation error');<" + "/" + "script>");

            // Reload TextBoxes
            bool boolOther = false;
            foreach (string strForm in Request.Form)
            {
                if (strForm.StartsWith("HDN__") == true)
                {
                    boolOther = !boolOther;
                    int intServer = Int32.Parse(strForm.Substring(5));
                    string strName = oServer.GetName(intServer, true);
                    if (_error == intServer.ToString())
                        strConfig += "<tr bgcolor=\"#FFF68F\">";
                    else
                        strConfig += "<tr" + (boolOther == false ? " bgcolor=\"#F6F6F6\"" : "") + ">";
                    strConfig += "<td>" + strName + "</td>";
                    string strIP1 = Request.Form["HDN_" + intServer.ToString() + "_IP1"];
                    string strIP2 = Request.Form["HDN_" + intServer.ToString() + "_IP2"];
                    string strIP3 = Request.Form["HDN_" + intServer.ToString() + "_IP3"];
                    string strIP4 = Request.Form["HDN_" + intServer.ToString() + "_IP4"];
                    string strVlan = Request.Form["HDN_" + intServer.ToString() + "_VLAN"];
                    strConfig += "<td><input type=\"text\" id=\"TXT_" + intServer.ToString() + "_IP1\" style=\"width:35px\" class=\"default\" value=\"" + strIP1 + "\" onblur=\"UpdateTextBox(this,'HDN_" + intServer.ToString() + "_IP1');\" maxlength=\"3\"/></td>";
                    strConfig += "<td>.</td>";
                    strConfig += "<td><input type=\"text\" id=\"TXT_" + intServer.ToString() + "_IP2\" style=\"width:35px\" class=\"default\" value=\"" + strIP2 + "\" onblur=\"UpdateTextBox(this,'HDN_" + intServer.ToString() + "_IP2');\" maxlength=\"3\"/></td>";
                    strConfig += "<td>.</td>";
                    strConfig += "<td><input type=\"text\" id=\"TXT_" + intServer.ToString() + "_IP3\" style=\"width:35px\" class=\"default\" value=\"" + strIP3 + "\" onblur=\"UpdateTextBox(this,'HDN_" + intServer.ToString() + "_IP3');\" maxlength=\"3\"/></td>";
                    strConfig += "<td>.</td>";
                    strConfig += "<td><input type=\"text\" id=\"TXT_" + intServer.ToString() + "_IP4\" style=\"width:35px\" class=\"default\" value=\"" + strIP4 + "\" onblur=\"UpdateTextBox(this,'HDN_" + intServer.ToString() + "_IP4');\" maxlength=\"3\"/></td>";
                    strConfig += "<td><input type=\"text\" id=\"TXT_" + intServer.ToString() + "_VLAN\" style=\"width:70px\" class=\"default\" value=\"" + strVlan + "\" onblur=\"UpdateTextBox(this,'HDN_" + intServer.ToString() + "_VLAN');\" maxlength=\"4\"/></td>";
                    strConfig += "</tr>";
                    strHiddenV += "<input type=\"hidden\" name=\"HDN__" + intServer.ToString() + "\" id=\"HDN__" + intServer.ToString() + "\" value=\"" + intServer.ToString() + "\" />";
                    strHiddenV += "<input type=\"hidden\" name=\"HDN_" + intServer.ToString() + "_IP1\" id=\"HDN_" + intServer.ToString() + "_IP1\" value=\"" + strIP1 + "\" />";
                    strHiddenV += "<input type=\"hidden\" name=\"HDN_" + intServer.ToString() + "_IP2\" id=\"HDN_" + intServer.ToString() + "_IP2\" value=\"" + strIP2 + "\" />";
                    strHiddenV += "<input type=\"hidden\" name=\"HDN_" + intServer.ToString() + "_IP3\" id=\"HDN_" + intServer.ToString() + "_IP3\" value=\"" + strIP3 + "\" />";
                    strHiddenV += "<input type=\"hidden\" name=\"HDN_" + intServer.ToString() + "_IP4\" id=\"HDN_" + intServer.ToString() + "_IP4\" value=\"" + strIP4 + "\" />";
                    strHiddenV += "<input type=\"hidden\" name=\"HDN_" + intServer.ToString() + "_VLAN\" id=\"HDN_" + intServer.ToString() + "_VLAN\" value=\"" + strVlan + "\" />";
                    strValidation += intServer.ToString() + ";";
                    btnComplete.Attributes.Add("onclick", "return EnsureLTMok('" + strValidation + "','" + txtVipName.ClientID + "','" + txtVipIP1.ClientID + "','" + txtVipIP2.ClientID + "','" + txtVipIP3.ClientID + "','" + txtVipIP4.ClientID + "') && confirm('Are you sure you want to mark this as completed and remove it from your workload?') && AlertMessage('This action requires validation that might take a few minutes to complete...\\n\\nPlease be patient and wait for the window to finish processing...') && ProcessControlButton();;");
                }
            }
        }
    }
}