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
    public partial class wm_storage : System.Web.UI.UserControl
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
        protected int intApplicationCitrix = Int32.Parse(ConfigurationManager.AppSettings["APPLICATIONID_CITRIX"]);
        protected bool boolUsePNCNaming = (ConfigurationManager.AppSettings["USE_PNC_NAMING"] == "1");
        protected int intUseLunSerial = Int32.Parse(ConfigurationManager.AppSettings["USE_SAN_LUN_SERIAL"]);
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
        protected bool boolStatus = false;
        protected int intRequest = 0;
        protected int intItem = 0;
        protected int intService = 0;
        protected int intNumber = 0;
        protected ServiceRequests oServiceRequest;
        protected Storage oStorage;
        protected Mnemonic oMnemonic;
        protected int intWorkloadPage = Int32.Parse(ConfigurationManager.AppSettings["ViewWorkload"]);
        protected bool boolMove = false;
        private string strEMailIdsBCC = "";
        protected Solaris oSolaris;
        private string strBootVolumeBackground = "BootVolume";

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
            oStorage = new Storage(intProfile, dsn);
            oApplication = new Applications(intProfile, dsn);
            oOnDemandTasks = new OnDemandTasks(intProfile, dsn);
            oDelegate = new Delegates(intProfile, dsn);
            oSolaris = new Solaris(intProfile, dsn);
            oMnemonic = new Mnemonic(intProfile, dsn);

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
                int intStatus = 0;
                Int32.TryParse(oResourceRequest.GetWorkflow(intResourceWorkflow, "status"), out intStatus);
                // Workflow start
                bool boolComplete = (oResourceRequest.GetWorkflow(intResourceWorkflow, "status") == "3");
                int intUser = Int32.Parse(oResourceRequest.GetWorkflow(intResourceWorkflow, "userid"));
                txtCustom.Text = oResourceRequest.GetWorkflow(intResourceWorkflow, "name");
                double dblAllocated = double.Parse(oResourceRequest.GetWorkflow(intResourceWorkflow, "allocated"));
                // Workflow end
                intService = Int32.Parse(ds.Tables[0].Rows[0]["serviceid"].ToString());
                lblService.Text = oService.Get(intService, "name");
                int intApp = oRequestItem.GetItemApplication(intItem);

                double dblUsed = oResourceRequest.GetWorkflowUsed(intResourceWorkflow);
                lblUpdated.Text = oResourceRequest.GetUpdated(intResourceParent);
                txtHours.Text = dblUsed.ToString("F");
                dblUsed = (dblUsed / dblAllocated) * 100;
                if (Request.QueryString["save"] != null && Request.QueryString["save"] != "")
                    Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "saved", "<script type=\"text/javascript\">alert('Information Saved Successfully');<" + "/" + "script>");
                if (Request.QueryString["status"] != null && Request.QueryString["status"] != "")
                    Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "statusd", "<script type=\"text/javascript\">alert('Weekly Status has been Added');<" + "/" + "script>");
                intProject = oRequest.GetProjectNumber(intRequest);
                hdnTab.Value = "D";
                panWorkload.Visible = true;
                bool boolRed = LoadStatus(intResourceWorkflow);
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
                        btnReturn.ImageUrl = "/images/tool_return_dbl.gif";
                        btnReturn.Enabled = false;
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
                if (oService.Get(intService, "sla") != "" && oService.Get(intService, "sla") != "0")
                {
                    oFunction.ConfigureToolButton(btnSLA, "/images/tool_sla");
                    int intDays = oResourceRequest.GetSLA(intResourceParent);
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
                if (boolRed == false && boolSLABreached == true)
                    btnComplete.Attributes.Add("onclick", "alert('NOTE: Your Service Level Agreement (SLA) has been breached!\\n\\nYou must provide a RED STATUS update with an explanation of why your SLA was breached for this request.\\n\\nOnce a RED STATUS update has been provided, you will be able to complete this request.');return false;");
                oFunction.ConfigureToolButton(btnEmail, "/images/tool_email");
                btnEmail.Attributes.Add("onclick", "return OpenWindow('RESOURCE_REQUEST_EMAIL','?rrid=" + intResourceWorkflow.ToString() + "&type=STORAGE');");
                btnDenied.Attributes.Add("onclick", "return CloseWindow();");
                oFunction.ConfigureToolButton(btnSave, "/images/tool_save");
                btnSave.Attributes.Add("onclick", "return ValidateNumber('" + txtHours.ClientID + "','Please enter a valid number of hours') && EnsureTextbox() && EnsureTextbox0() && EnsureStatus('" + hdnTab.ClientID + "','" + ddlStatus.ClientID + "','" + txtComments.ClientID + "') && ProcessControlButton();");
                oFunction.ConfigureToolButton(btnPrint, "/images/tool_print");
                btnPrint.Attributes.Add("onclick", "return PrintWindow();");
                oFunction.ConfigureToolButton(btnClose, "/images/tool_close");
                btnClose.Attributes.Add("onclick", "return ExitWindow();");
                // 6/1/2009 - Load ReadOnly View
                if (oResourceRequest.CanUpdate(intProfile, intResourceWorkflow, false) == false)
                {
                    oFunction.ConfigureToolButtonRO(btnSave, btnComplete);
                    //panDenied.Visible = true;
                }
                if (intStatus != (int)ResourceRequestStatus.AwaitingResponse) //Awaiting Client Response
                {
                    oFunction.ConfigureToolButton(btnReturn, "/images/tool_return");
                    btnReturn.Attributes.Add("onclick", "return OpenWindow('RESOURCE_REQUEST_RETURN','?rrid=" + intResourceParent.ToString() + "&rrwfid=" + intResourceWorkflow.ToString() + "');");
                }
            }
            else
                panDenied.Visible = true;
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
            DataSet ds = oOnDemandTasks.GetServerStorage(intRequest, intItem, intNumber);
            if (ds.Tables[0].Rows.Count > 0)
            {
                int intAnswer = Int32.Parse(ds.Tables[0].Rows[0]["answerid"].ToString());
                Forecast oForecast = new Forecast(intProfile, dsn);
                if (oForecast.GetAnswer(intAnswer, "forecastid") != "")
                {
                    int intForecast = Int32.Parse(oForecast.GetAnswer(intAnswer, "forecastid"));
                    if (oForecast.Get(intForecast, "requestid") != "")
                    {
                        ModelsProperties oModelsProperties = new ModelsProperties(intProfile, dsn);
                        int intModel = oForecast.GetModelAsset(intAnswer);
                        if (intModel == 0)
                            intModel = oForecast.GetModel(intAnswer);
                        //btnView.Attributes.Add("onclick", "return OpenWindow('FORECAST_EQUIPMENT','?id=" + intAnswer.ToString() + "');");
                        btnView.Attributes.Add("onclick", "OpenNewWindow('/datapoint/service/design.aspx?t=design&q=" + oFunction.encryptQueryString(intAnswer.ToString()) + "',800,600);return false;");
                        btnView.ToolTip = intAnswer.ToString();
                        lblView.Text = oOnDemandTasks.GetBody(intAnswer, intImplementorDistributed, intImplementorMidrange);
                        chk1.Checked = (ds.Tables[0].Rows[0]["chk1"].ToString() == "1");
                        lblAnswer.Text = ds.Tables[0].Rows[0]["answerid"].ToString();
                        lblProd.Text = ds.Tables[0].Rows[0]["prod"].ToString();
                        bool _prod = (lblProd.Text == "1");
                        lblModel.Text = ds.Tables[0].Rows[0]["modelid"].ToString();
                        boolDone = (chk1.Checked);
                        img1.ImageUrl = (boolDone ? "/images/check.gif" : "/images/green_arrow.gif");
                        Servers oServer = new Servers(intProfile, dsn);
                        Organizations oOrganization = new Organizations(0, dsn);
                        ServerName oServerName = new ServerName(0, dsn);
                        Asset oAsset = new Asset(0, dsnAsset);
                        IPAddresses oIPAddresses = new IPAddresses(0, dsnIP, dsn);
                        ConsistencyGroups oConsistencyGroups = new ConsistencyGroups(0, dsn);
                        Environments oEnvironment = new Environments(0, dsn);
                        OperatingSystems oOperatingSystem = new OperatingSystems(0, dsn);
                        Classes oClass = new Classes(0, dsn);
                        string strName = "";
                        lbl1.Text = oStorage.GetBody(intRequest, intItem, intNumber, dsnAsset, dsnIP, intEnvironment, true);
                        if (lbl1.Text == "")
                            lbl1.Text = "Design Not Found";
                    }
                    else
                        panDeleted.Visible = true;
                }
                else
                    panDeleted.Visible = true;
            }
            if (Request.QueryString["div"] != null)
            {
                switch (Request.QueryString["div"])
                {
                    case "E":
                        boolExecution = true;
                        hdnTab.Value = "E";
                        break;
                    case "S":
                        boolStatus = true;
                        hdnTab.Value = "S";
                        break;
                }
            }
            if (boolDetails == false && boolExecution == false && boolStatus == false)
                boolDetails = true;
            return boolDone;
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
        protected void btnSave_Click(Object Sender, EventArgs e)
        {
            int intResourceWorkflow = Int32.Parse(lblResourceWorkflow.Text);
            int intResourceParent = oResourceRequest.GetWorkflowParent(intResourceWorkflow);
            oResourceRequest.UpdateWorkflowName(intResourceWorkflow, txtCustom.Text, intProfile);
            switch (Request.Form[hdnTab.UniqueID])
            {
                case "S":
                    oResourceRequest.AddStatus(intResourceWorkflow, Int32.Parse(ddlStatus.SelectedItem.Value), txtComments.Text, intProfile);
                    Response.Redirect(Request.Path + "?rrid=" + intResourceWorkflow.ToString() + "&div=S&save=true");
                    break;
                case "E":
                    //oResourceRequest.UpdateAllocated(intResource, double.Parse(txtHours.Text));
                    oResourceRequest.UpdateWorkflowHoursOverwrite(intResourceWorkflow, double.Parse(txtHours.Text));
                    foreach (string strForm in Request.Form)
                    {
                        if (strForm.StartsWith("HDN_") == true)
                        {
                            string strValue = strForm.Substring(4);
                            int intID = Int32.Parse(strValue.Substring(0, strValue.IndexOf("_")));
                            strValue = strValue.Substring(strValue.IndexOf("_") + 1);
                            string strType = strValue.Substring(0, strValue.IndexOf("_"));
                            strValue = strValue.Substring(strValue.IndexOf("_") + 1);
                            try
                            {
                                if (intUseLunSerial > 0)
                                {
                                    if (strType == "L")
                                        oStorage.UpdateLunActual(intID, (Request.Form["HDN_" + intID.ToString() + "_L_SERIALNO"]), double.Parse(Request.Form["HDN_" + intID.ToString() + "_L_SIZE"]), double.Parse(Request.Form["HDN_" + intID.ToString() + "_L_SIZE_QA"]), double.Parse(Request.Form["HDN_" + intID.ToString() + "_L_SIZE_TEST"]), (Request.Form["HDN_" + intID.ToString() + "_L_REPLICATED"] == "Yes" ? 1 : 0), (Request.Form["HDN_" + intID.ToString() + "_L_HIGH_AVAILABILITY"] == "Yes" ? 1 : 0));
                                    else
                                        oStorage.UpdateMountPointActual(intID, (Request.Form["HDN_" + intID.ToString() + "_M_SERIALNO"]), double.Parse(Request.Form["HDN_" + intID.ToString() + "_M_SIZE"]), double.Parse(Request.Form["HDN_" + intID.ToString() + "_M_SIZE_QA"]), double.Parse(Request.Form["HDN_" + intID.ToString() + "_M_SIZE_TEST"]), (Request.Form["HDN_" + intID.ToString() + "_M_REPLICATED"] == "Yes" ? 1 : 0), (Request.Form["HDN_" + intID.ToString() + "_M_HIGH_AVAILABILITY"] == "Yes" ? 1 : 0));
                                }
                                else
                                {
                                    if (strType == "L")
                                        oStorage.UpdateLunActual(intID, "", double.Parse(Request.Form["HDN_" + intID.ToString() + "_L_SIZE"]), double.Parse(Request.Form["HDN_" + intID.ToString() + "_L_SIZE_QA"]), double.Parse(Request.Form["HDN_" + intID.ToString() + "_L_SIZE_TEST"]), (Request.Form["HDN_" + intID.ToString() + "_L_REPLICATED"] == "Yes" ? 1 : 0), (Request.Form["HDN_" + intID.ToString() + "_L_HIGH_AVAILABILITY"] == "Yes" ? 1 : 0));
                                    else
                                        oStorage.UpdateMountPointActual(intID, "", double.Parse(Request.Form["HDN_" + intID.ToString() + "_M_SIZE"]), double.Parse(Request.Form["HDN_" + intID.ToString() + "_M_SIZE_QA"]), double.Parse(Request.Form["HDN_" + intID.ToString() + "_M_SIZE_TEST"]), (Request.Form["HDN_" + intID.ToString() + "_M_REPLICATED"] == "Yes" ? 1 : 0), (Request.Form["HDN_" + intID.ToString() + "_M_HIGH_AVAILABILITY"] == "Yes" ? 1 : 0));
                                }
                            }
                            catch { }
                        }
                    }
                    oOnDemandTasks.UpdateServerStorage(intRequest, intItem, intNumber, (chk1.Checked ? 1 : 0));
                    Response.Redirect(Request.Path + "?rrid=" + intResourceWorkflow.ToString() + "&div=E&save=true");
                    break;
            }
        }
        protected void btnComplete_Click(Object Sender, EventArgs e)
        {
            int intResourceWorkflow = Int32.Parse(lblResourceWorkflow.Text);
            int intResourceParent = oResourceRequest.GetWorkflowParent(intResourceWorkflow);
            oResourceRequest.UpdateWorkflowStatus(intResourceWorkflow, 3, true);
            oResourceRequest.CloseWorkflow(intResourceWorkflow, intEnvironment,  0, dsnServiceEditor, true, intResourceRequestApprove, intAssignPage, intViewPage, dsnAsset, dsnIP);
            oOnDemandTasks.UpdateServerStorageComplete(intRequest, intItem, intNumber);
            // Add a green / completed status if there are no updates, OR the last status is not green
            DataSet dsStatus = oResourceRequest.GetStatuss(intResourceWorkflow);
            if (dsStatus.Tables[0].Rows.Count == 0 || dsStatus.Tables[0].Rows[0]["status"].ToString() != "3")
                oResourceRequest.AddStatus(intResourceWorkflow, 3, "Completed", intProfile);
            // Update II OnDemand Task
            int intAnswer = Int32.Parse(lblAnswer.Text);
            int intModel = Int32.Parse(lblModel.Text);
            oOnDemandTasks.UpdateStorage(intAnswer, intModel);
            // Initiate PNC Workflow (if applicable)
            PNCTasks oPNCTask = new PNCTasks(0, dsn);
            strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DESIGN_BUILDER");
            //oPNCTask.InitiateNextStep(intRequest, intService, intNumber, intAnswer, intModel, intEnvironment, intApplicationCitrix, intAssignPage, intViewPage, dsnAsset, dsnIP, dsnServiceEditor, false, 0);
            oPNCTask.InitiateNextStep(intRequest, intAnswer, intModel, intEnvironment, intAssignPage, intViewPage, dsnAsset, dsnIP, dsnServiceEditor);
            // Notify Builder
            bool boolProd = (lblProd.Text == "1");
            Forecast oForecast = new Forecast(0, dsn);
            int intBuilder = 0;
            DataSet dsTasks = oOnDemandTasks.GetPending(intAnswer);
            if (dsTasks.Tables[0].Rows.Count > 0)
            {
                intBuilder = Int32.Parse(dsTasks.Tables[0].Rows[0]["resourceid"].ToString());
                intBuilder = Int32.Parse(oResourceRequest.GetWorkflow(intBuilder, "userid"));
            }

            if (intBuilder > 0)
                oFunction.SendEmail("Storage Completed!", oUser.GetName(intBuilder), "", strEMailIdsBCC, "Storage Completed!", "<p><b>This message is to notify you that " + (boolProd ? "PRODUCTION" : "TEST") + " storage has been configured for the following design...</b></p><p>" + oForecast.GetAnswerBody(intAnswer, intEnvironment, dsnAsset, dsnIP) + "</p>", true, false);
            else
                oFunction.SendEmail("Storage Completed!", strEMailIdsBCC, "", "", "Storage Completed!", "<p><b>This message is to notify you that " + (boolProd ? "PRODUCTION" : "QA / TEST") + " storage has been configured for the following design...</b> (No Builder for ANSWERID " + intAnswer.ToString() + ")</p><p>" + oForecast.GetAnswerBody(intAnswer, intEnvironment, dsnAsset, dsnIP) + "</p>", true, false);
            Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "reload", "<script type=\"text/javascript\">RefreshOpeningWindow();window.close();<" + "/" + "script>");
        }
        protected void btnDelete_Click(Object Sender, EventArgs e)
        {
            int intResourceWorkflow = Int32.Parse(lblResourceWorkflow.Text);
            int intResourceParent = oResourceRequest.GetWorkflowParent(intResourceWorkflow);
            oResourceRequest.UpdateWorkflowStatus(intResourceWorkflow, -2, true);
            oResourceRequest.CloseWorkflow(intResourceWorkflow, intEnvironment,  0, dsnServiceEditor, true, intResourceRequestApprove, intAssignPage, intViewPage, dsnAsset, dsnIP);
            oOnDemandTasks.UpdateServerStorageComplete(intRequest, intItem, intNumber);
            Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "reload", "<script type=\"text/javascript\">RefreshOpeningWindow();window.close();<" + "/" + "script>");
        }
    }
}
