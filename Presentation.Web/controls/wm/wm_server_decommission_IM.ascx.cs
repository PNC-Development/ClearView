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
    public partial class wm_server_decommission_IM : System.Web.UI.UserControl
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
        protected int intApplication = 0;
        protected int intPage = 0;
        protected int intProject = 0;
        protected bool boolDetails = false;
        protected bool boolExecution = false;
        protected bool boolChange = false;
        protected bool boolDocuments = false;
        protected int intRequest = 0;
        protected int intItem = 0;
        protected int intNumber = 0;
        protected bool boolJoined = false;
        protected Servers oServer;
        protected Customized oCustomized;
        protected Classes oClass;
        protected Asset oAsset;
        protected ModelsProperties oModelsProperties;
        protected Models oModel;
        protected Forecast oForecast;
        protected IPAddresses oIPAddress;
        protected bool boolIsServerHasBackup = false;
        protected bool boolIsServerVMWare = false;
        protected Variables oVariables;

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
            oServer = new Servers(intProfile, dsn);
            oCustomized = new Customized(intProfile, dsn);
            oClass = new Classes(intProfile, dsn);
            oAsset = new Asset(intProfile, dsnAsset);
            oModelsProperties = new ModelsProperties(intProfile, dsn);
            oModel = new Models(intProfile, dsn);
            oForecast = new Forecast(intProfile, dsn);
            oIPAddress = new IPAddresses(intProfile, dsnIP, dsn);
            oVariables = new Variables(intEnvironment);
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
                int intService = Int32.Parse(ds.Tables[0].Rows[0]["serviceid"].ToString());
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
                    dblAllocated = dblUsed;

                    //Page Load Script
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "SetControls", "setControls();", true);

                    // Load Decomission Server IM Request Details
                    DataSet dsDecom = oCustomized.GetDecommissionServerIM(intRequest, intItem, intNumber);
                    if (dsDecom.Tables[0].Rows.Count > 0)
                    {
                        int intServer = Int32.Parse(dsDecom.Tables[0].Rows[0]["serverid"].ToString());
                        btnVerifyModel.NavigateUrl = "/datapoint/asset/server.aspx?t=name&q=" + oFunction.encryptQueryString(oServer.GetName(intServer, true));
                        int intModel = Int32.Parse(oServer.Get(intServer, "modelid"));
                        int intParent = Int32.Parse(oModelsProperties.Get(intModel, "modelid"));
                        if (oModel.Get(intParent, "destroy") == "1")
                            radDestroy.CssClass = "bold";
                        else
                            radRedeploy.CssClass = "bold";
                        if (dsDecom.Tables[0].Rows[0]["serverdestroyed"] != DBNull.Value)
                        {
                            //pnlDestroy.Visible = true; 
                            radDestroy.Checked = (Int32.Parse(dsDecom.Tables[0].Rows[0]["serverdestroyed"].ToString()) == 1 ? true : false);
                            if (dsDecom.Tables[0].Rows[0]["DestroyUnRack"] != DBNull.Value)
                                chkUnrack.Checked = (Int32.Parse(dsDecom.Tables[0].Rows[0]["DestroyUnRack"].ToString()) == 1 ? true : false);
                            if (dsDecom.Tables[0].Rows[0]["DestroyWipeDrives"] != DBNull.Value)
                                chkWipeDrives.Checked = (Int32.Parse(dsDecom.Tables[0].Rows[0]["DestroyWipeDrives"].ToString()) == 1 ? true : false);
                            if (dsDecom.Tables[0].Rows[0]["DestroyDispose"] != DBNull.Value)
                                chkDispose.Checked = (Int32.Parse(dsDecom.Tables[0].Rows[0]["DestroyDispose"].ToString()) == 1 ? true : false);
                        }
                        if (dsDecom.Tables[0].Rows[0]["ServerRedeployed"] != DBNull.Value)
                        {
                            //pnlRedeploy.Visible = true; 
                            radRedeploy.Checked = (Int32.Parse(dsDecom.Tables[0].Rows[0]["ServerRedeployed"].ToString()) == 1 ? true : false);
                            if (dsDecom.Tables[0].Rows[0]["RedeployVerifyServerModel"] != DBNull.Value)
                                chkVerifyModel.Checked = (Int32.Parse(dsDecom.Tables[0].Rows[0]["RedeployVerifyServerModel"].ToString()) == 1 ? true : false);
                            if (dsDecom.Tables[0].Rows[0]["RedeployMoveServerToDeploy"] != DBNull.Value)
                                chkMoveServerToDeploy.Checked = (Int32.Parse(dsDecom.Tables[0].Rows[0]["RedeployMoveServerToDeploy"].ToString()) == 1 ? true : false);
                        }

                    }

                    bool boolCheckforCompletion = false;
                    if (radDestroy.Checked == true)
                        if (chkUnrack.Checked == true && chkWipeDrives.Checked == true && chkDispose.Checked == true)
                            boolCheckforCompletion = true;
                    if (radRedeploy.Checked == true)
                        if (chkVerifyModel.Checked == true && chkMoveServerToDeploy.Checked == true)
                            boolCheckforCompletion = true;
                    // End of Load Decomission Server IM Request Details
                    if (boolCheckforCompletion) // (dblAllocated == dblUsed)
                    {
                        if (boolComplete == false)
                        {
                            oFunction.ConfigureToolButton(btnComplete, "/images/tool_complete");

                            btnComplete.Attributes.Add("onclick", "return validationServerOptions()" +
                                " && confirm('Are you sure you want to mark this as completed and remove it from your workload?');");

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
                    if (oService.Get(intService, "sla") != "")
                    {
                        oFunction.ConfigureToolButton(btnSLA, "/images/tool_sla");
                        int intDays = oResourceRequest.GetSLA(intResourceParent);
                        if (intDays < 1)
                            btnSLA.Style["border"] = "solid 2px #FF0000";
                        else if (intDays < 3)
                            btnSLA.Style["border"] = "solid 2px #FF9999";
                        btnSLA.Attributes.Add("onclick", "return OpenWindow('RESOURCE_REQUEST_SLA','?rrid=" + intResourceParent.ToString() + "');");
                    }
                    else
                    {
                        btnSLA.ImageUrl = "/images/tool_sla_dbl.gif";
                        btnSLA.Enabled = false;
                    }
                    oFunction.ConfigureToolButton(btnEmail, "/images/tool_email");
                    btnEmail.Attributes.Add("onclick", "return OpenWindow('RESOURCE_REQUEST_EMAIL','?rrid=" + intResourceWorkflow.ToString() + "&type=GENERIC');");
                    dblUsed = (dblUsed / dblAllocated) * 100;
                    intProject = oRequest.GetProjectNumber(intRequest);
                    hdnTab.Value = "D";
                    panWorkload.Visible = true;
                    LoadStatus(intResourceWorkflow);
                    LoadChange(intResourceWorkflow);
                    LoadInformation(intResourceWorkflow);
                    chkDescription.Checked = (Request.QueryString["doc"] != null);
                    lblDocuments.Text = oDocument.GetDocuments_Service(intRequest, intService, oVariables.DocumentsFolder(), 1, (Request.QueryString["doc"] != null));



                    //setControls
                    radDestroy.Attributes.Add("onclick", "setControls();");
                    radRedeploy.Attributes.Add("onclick", "setControls();");

                    btnDenied.Attributes.Add("onclick", "return CloseWindow();");
                    oFunction.ConfigureToolButton(btnSave, "/images/tool_save");
                    oFunction.ConfigureToolButton(btnPrint, "/images/tool_print");
                    btnPrint.Attributes.Add("onclick", "return PrintWindow();");
                    oFunction.ConfigureToolButton(btnClose, "/images/tool_close");
                    btnClose.Attributes.Add("onclick", "return ExitWindow();");
                    btnSave.Attributes.Add("onclick", "return ValidateStatus('" + ddlStatus.ClientID + "','" + txtComments.ClientID + "')" +
                        ";");
                    //"return ValidateRadioButtons('" + radDestroy.ClientID + "','" + radRedeploy.ClientID + "','Please select the option Destroyed/Redeployed')" +

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
            }
            lblStatus.Text = oResourceRequest.GetStatus(dblTotalStatus, 50, 15);
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
        protected void btnSave_Click(Object Sender, EventArgs e)
        {
            //Save Current Status
            saveRequest();

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

            oResourceRequest.UpdateWorkflowName(intResourceWorkflow, txtCustom.Text, intProfile);
            Response.Redirect(Request.Path + "?rrid=" + intResourceWorkflow.ToString() + "&div=E&save=true");
        }

        private void saveRequest()
        {
            int? intServerDestroyed = null;
            int? intDestroyUnRack = null;
            int? intDestroyWipeDrives = null;
            int? intDestroyDispose = null;
            int? intServerRedeployed = null;
            int? intRedeployVerifyServerModel = null;
            int? intRedeployMoveServerToDeploy = null;

            if (radDestroy.Checked == true)
            {
                intServerDestroyed = 1;
                if (chkUnrack.Checked == true)
                    intDestroyUnRack = 1;
                if (chkWipeDrives.Checked == true)
                    intDestroyWipeDrives = 1;
                if (chkDispose.Checked == true)
                    intDestroyDispose = 1;
            }
            else if (radRedeploy.Checked == true)
            {
                intServerRedeployed = 1;
                if (chkVerifyModel.Checked == true)
                    intRedeployVerifyServerModel = 1;
                if (chkMoveServerToDeploy.Checked == true)
                    intRedeployMoveServerToDeploy = 1;
            }


            DataSet dsDecom = oCustomized.GetDecommissionServerIM(intRequest, intItem, intNumber);
            if (dsDecom.Tables[0].Rows.Count > 0)  //Update Record
            {
                oCustomized.UpdateDecommissionServerIM(intRequest, intItem, intNumber,
                                    intServerDestroyed, intDestroyUnRack, intDestroyWipeDrives, intDestroyDispose,
                                    intServerRedeployed, intRedeployVerifyServerModel, intRedeployMoveServerToDeploy);
            }
        }
        protected void btnComplete_Click(Object Sender, EventArgs e)
        {
            saveRequest();

            DataSet dsDecom = oCustomized.GetDecommissionServerIM(intRequest, intItem, intNumber);
            if (dsDecom.Tables[0].Rows.Count > 0)
            {
                int intServer = Int32.Parse(dsDecom.Tables[0].Rows[0]["serverid"].ToString());
                int intAsset = 0;
                int intAssetDR = 0;
                DataSet dsAsset = oServer.GetAssets(intServer);
                foreach (DataRow drAsset in dsAsset.Tables[0].Rows)
                {
                    if (drAsset["latest"].ToString() == "1")
                        intAsset = Int32.Parse(drAsset["assetid"].ToString());
                    if (drAsset["dr"].ToString() == "1")
                        intAssetDR = Int32.Parse(drAsset["assetid"].ToString());
                }

                if (dsDecom.Tables[0].Rows[0]["serverdestroyed"].ToString() == "1")
                {
                    if (intAsset > 0)
                        oAsset.UpdateStatus(intAsset, "", (int)AssetStatus.Disposed, intProfile, DateTime.Now);
                    if (intAssetDR > 0)
                        oAsset.UpdateStatus(intAssetDR, "", (int)AssetStatus.Disposed, intProfile, DateTime.Now);
                }
                if (dsDecom.Tables[0].Rows[0]["ServerRedeployed"].ToString() == "1")
                {
                    if (intAsset > 0)
                        oAsset.UpdateStatus(intAsset, "", (int)AssetStatus.Arrived, intProfile, DateTime.Now);
                    if (intAssetDR > 0)
                        oAsset.UpdateStatus(intAssetDR, "", (int)AssetStatus.Arrived, intProfile, DateTime.Now);
                }
            }


            int intResourceWorkflow = Int32.Parse(lblResourceWorkflow.Text);
            int intResourceParent = oResourceRequest.GetWorkflowParent(intResourceWorkflow);
            double dblAllocated = double.Parse(oResourceRequest.GetWorkflow(intResourceWorkflow, "allocated"));
            oResourceRequest.UpdateWorkflowHoursOverwrite(intResourceWorkflow, dblAllocated);

            oResourceRequest.UpdateWorkflowStatus(intResourceWorkflow, 3, true);

            oResourceRequest.CloseWorkflow(intResourceWorkflow, intEnvironment,  0, dsnServiceEditor, true, intResourceRequestApprove, intAssignPage, intViewPage, dsnAsset, dsnIP);
            Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "reload", "<script type=\"text/javascript\">RefreshOpeningWindow();window.close();<" + "/" + "script>");
        }
    }
}