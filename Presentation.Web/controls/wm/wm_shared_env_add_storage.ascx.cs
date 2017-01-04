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
using NCC.ClearView.Presentation.Web.Custom;
using System.IO;
namespace NCC.ClearView.Presentation.Web
{
    public partial class wm_shared_env_add_storage : System.Web.UI.UserControl
    {

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
        protected StatusLevels oStatusLevel;

        protected Asset oAsset;
        protected AssetSharedEnvOrder oAssetSharedEnvOrder;
        protected WMServiceTasks oWMServiceTasks;

        protected Variables oVariable;
        protected DataPoint oDataPoint;

        protected int intApplication = 0;
        protected int intPage = 0;
        protected int intProject = 0;
        protected bool boolDetails = false;
        protected bool boolExecution = false;
        protected bool boolChange = false;
        protected bool boolDocuments = false;
        protected bool boolStatusUpdates = false;
        protected int intRequest = 0;
        protected int intService = 0;
        protected int intItem = 0;
        protected int intNumber = 0;
    
        protected bool boolCheckboxes = false;
        protected string strCheckboxes = "";
        protected bool boolServiceReturned = false;
           
       

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
            oStatusLevel = new StatusLevels(intProfile, dsn);
            oAssetSharedEnvOrder = new AssetSharedEnvOrder(intProfile, dsn, dsnAsset, intEnvironment);
            oWMServiceTasks = new WMServiceTasks(intProfile, dsn);

            oAsset = new Asset(0, dsnAsset, dsn);
            oDataPoint = new DataPoint(intProfile, dsn);
            oVariable = new Variables(intEnvironment);
          
            if (Request.QueryString["applicationid"] != null && Request.QueryString["applicationid"] != "")
                intApplication = Int32.Parse(Request.QueryString["applicationid"]);
            if (Request.QueryString["pageid"] != null && Request.QueryString["pageid"] != "")
                intPage = Int32.Parse(Request.QueryString["pageid"]);
            if (Request.Cookies["application"] != null && Request.Cookies["application"].Value != "")
                intApplication = Int32.Parse(Request.Cookies["application"].Value);
            if (Request.QueryString["rrid"] != null && Request.QueryString["rrid"] != "")
            {
                //Load Attributes
                if (!IsPostBack)
                {
                if (Request.QueryString["save"] != null && Request.QueryString["save"] != "")
                    Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "saved", "<script type=\"text/javascript\">alert('Information Saved Successfully');<" + "/" + "script>");
                if (Request.QueryString["status"] != null && Request.QueryString["status"] != "")
                    Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "statusd", "<script type=\"text/javascript\">alert('Status Updates Added Successfully');<" + "/" + "script>");


                   
                    oFunction.ConfigureToolButton(btnEmail, "/images/tool_email");
                    oFunction.ConfigureToolButton(btnPrint, "/images/tool_print");
                    oFunction.ConfigureToolButton(btnClose, "/images/tool_close");
                    oFunction.ConfigureToolButton(btnSave, "/images/tool_save");
                    oFunction.ConfigureToolButton(btnComplete, "/images/tool_complete");
                    oFunction.ConfigureToolButton(btnSLA, "/images/tool_sla");

                    btnDenied.Attributes.Add("onclick", "return CloseWindow();");
                    btnPrint.Attributes.Add("onclick", "return PrintWindow();");
                    btnClose.Attributes.Add("onclick", "return ExitWindow();");


                    //btnSave.Attributes.Add("onclick", "return ValidateStatus('" + ddlStatus.ClientID + "','" + txtComments.ClientID + "')" +
                    //   ";");
                    btnAddDataStoreSelection.Attributes.Add("onclick", "return ValidateText('" + txtDataStoreName.ClientID + "','Please enter datastore')" +
                                       ";");
                    btnStatus.Attributes.Add("onclick", "return ValidateStatus('" + ddlStatus.ClientID + "','" + txtComments.ClientID + "')" +
                      ";");

                    imgDate.Attributes.Add("onclick", "return ShowCalendar('" + txtDate.ClientID + "');");

                }
             
                LoadRequest();
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

        private void LoadRequest()
        {

            // Get Workflow Details
            lblResourceWorkflow.Text = Request.QueryString["rrid"];
            int intResourceWorkflow = Int32.Parse(Request.QueryString["rrid"]);
            int intResourceParent = oResourceRequest.GetWorkflowParent(intResourceWorkflow);
            int intUser = Int32.Parse(oResourceRequest.GetWorkflow(intResourceWorkflow, "userid"));
            bool boolComplete = (oResourceRequest.GetWorkflow(intResourceWorkflow, "status") == "3");
            double dblAllocated = double.Parse(oResourceRequest.GetWorkflow(intResourceWorkflow, "allocated"));
            bool boolJoined = (oResourceRequest.GetWorkflow(intResourceWorkflow, "joined") == "1");

            DataSet ds = oResourceRequest.Get(intResourceParent);

            //Project or Task Details
            intRequest = Int32.Parse(ds.Tables[0].Rows[0]["requestid"].ToString());
            intProject = oRequest.GetProjectNumber(intRequest);
            if (intProject > 0)
            {
                lblName.Text = oProject.Get(intProject, "name");
                lblNumber.Text = oProject.Get(intProject, "number");
                lblType.Text = "Project";
            }
            else
            {
                lblName.Text = oResourceRequest.GetWorkflow(intRequest, "name");
                lblNumber.Text = "CVT" + intRequest.ToString();
                lblType.Text = "Task";
            }

            intService = Int32.Parse(ds.Tables[0].Rows[0]["serviceid"].ToString());
            intItem = Int32.Parse(ds.Tables[0].Rows[0]["itemid"].ToString());
            intNumber = Int32.Parse(ds.Tables[0].Rows[0]["number"].ToString());
            int intApp = oRequestItem.GetItemApplication(intItem);

            lblService.Text = oService.Get(intService, "name");

            lblRequestedBy.Text = oUser.GetFullName(oRequest.GetUser(intRequest));
            lblRequestedOn.Text = DateTime.Parse(oResourceRequest.Get(intResourceParent, "created")).ToString();
            lblDescription.Text = oRequest.Get(intRequest, "description");
            if (lblDescription.Text == "")
                lblDescription.Text = "<i>No information</i>";

            txtCustom.Text = oResourceRequest.GetWorkflow(intResourceWorkflow, "name");
            lblUpdated.Text = oResourceRequest.GetUpdated(intResourceParent);


            if (!IsPostBack)
            {
                double dblUsed = oResourceRequest.GetWorkflowUsed(intResourceWorkflow);
                if (dblAllocated == dblUsed)
                {
                    if (boolComplete == false)
                    {

                        btnComplete.Attributes.Add("onclick", "return confirm('Are you sure you want to mark this as completed and remove it from your workload?');");
                    }
                    else
                    {

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
                btnEmail.Attributes.Add("onclick", "return OpenWindow('RESOURCE_REQUEST_EMAIL','?rrid=" + intResourceWorkflow.ToString() + "&type=GENERIC');");
                dblUsed = (dblUsed / dblAllocated) * 100;

                panWorkload.Visible = true;
                bool boolRed = LoadStatus(intResourceWorkflow);
                if (boolRed == false && boolSLABreached == true)
                    btnComplete.Attributes.Add("onclick", "alert('NOTE: Your Service Level Agreement (SLA) has been breached!\\n\\nYou must provide a RED STATUS update with an explanation of why your SLA was breached for this request.\\n\\nOnce a RED STATUS update has been provided, you will be able to complete this request.');return false;");

                LoadInformation();
                chkDescription.Checked = (Request.QueryString["doc"] != null);

                LoadChange(intResourceWorkflow);
                lblDocuments.Text = oDocument.GetDocuments_Service(intRequest, intService, oVariable.DocumentsFolder(), 1, (Request.QueryString["doc"] != null));


                if ((oResourceRequest.CanUpdate(intProfile, intResourceWorkflow, false) == false) || boolComplete == true)
                    oFunction.ConfigureToolButtonRO(btnSave, btnComplete);

                btnReturn.Visible = false;

            }

            //div selection
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
                    case "S":
                        boolStatusUpdates = true;
                        break;
                }
            }
            if (boolDetails == false && boolExecution == false && boolStatusUpdates == false && boolChange == false && boolDocuments == false)
                boolDetails = true;


        }

        private void LoadInformation()
        {
            lblView.Text = oAssetSharedEnvOrder.GetOrderBody(intRequest,intItem,intNumber);

            //Get the Execution Tab Details
            DataSet ds = oAssetSharedEnvOrder.Get(intRequest,intItem,intNumber);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];

                hdnOrderId.Value = dr["OrderId"].ToString();
                VMWare oVMWare = new VMWare(intProfile, dsn);
                string strHeader = "";

                if (dr["OrderType"].ToString() == "1")//Add Cluster -Get folder
                    strHeader = oVMWare.GetFolder(Int32.Parse(dr["ParentId"].ToString()), "name");
                else if (dr["OrderType"].ToString() == "2")//Add Host -Get Cluster
                    strHeader = oVMWare.GetCluster(Int32.Parse(dr["ParentId"].ToString()), "name");
                else if (dr["OrderType"].ToString() == "3")//Add Storage -Get Host
                    strHeader = oVMWare.GetHost(Int32.Parse(dr["ParentId"].ToString()), "name");

                

                LoadWMServiceTasks();
                LoadDataStores();
               
                DataSet dsDataStore = oAssetSharedEnvOrder.GetDataStoreSelection(Int32.Parse(hdnOrderId.Value));
                if ((oWMServiceTasks.IsWMServiceTaskCompleted(intRequest, intService, intItem, intNumber) == false)
                    || dsDataStore.Tables[0].Rows.Count < 1)
                {
                    btnComplete.Visible = false;
                }
                else
                {
                    btnComplete.Enabled = true;
                    btnComplete.Visible = true;
                }

                
              }
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
                int intResourceWorkflow = Int32.Parse(lblResourceWorkflow.Text);
                int intResourceParent = oResourceRequest.GetWorkflowParent(intResourceWorkflow);


                SaveWMServiceTaskStatus();

                oServiceDetail.UpdateCheckboxes(Request, intResourceWorkflow, intRequest, intItem, intNumber);
                double dblAllocated = oResourceRequest.GetDetailsHoursUsed(intRequest, intItem, intNumber, intResourceWorkflow, false);
                oResourceRequest.UpdateWorkflowAllocated(intResourceWorkflow, dblAllocated);

                oResourceRequest.UpdateWorkflowName(intResourceWorkflow, txtCustom.Text, intProfile);
                Response.Redirect(Request.Path + "?rrid=" + intResourceWorkflow.ToString() + "&div=E&save=true" );
        }

        protected void btnComplete_Click(Object Sender, EventArgs e)
        {
            CompletedWorkflow();
        }

        private void CompletedWorkflow()
        {

            int intResourceWorkflow = Int32.Parse(lblResourceWorkflow.Text);
            int intResourceParent = oResourceRequest.GetWorkflowParent(intResourceWorkflow);

            double dblAllocated = double.Parse(oResourceRequest.GetWorkflow(intResourceWorkflow, "allocated"));
            oResourceRequest.UpdateWorkflowHoursOverwrite(intResourceWorkflow, dblAllocated);

            // Add a green / completed status if there are no updates, OR the last status is not green
            DataSet dsStatus = oResourceRequest.GetStatuss(intResourceWorkflow);
            if (dsStatus.Tables[0].Rows.Count == 0 || dsStatus.Tables[0].Rows[0]["status"].ToString() != "3")
                oResourceRequest.AddStatus(intResourceWorkflow, 3, "Completed", intProfile);
            oResourceRequest.UpdateWorkflowStatus(intResourceWorkflow, 3, true);
            oResourceRequest.CloseWorkflow(intResourceWorkflow, intEnvironment, 0, dsnServiceEditor, true, intResourceRequestApprove, intAssignPage, intViewPage, dsnAsset, dsnIP);

            AddDataStores();
            oAssetSharedEnvOrder.UpdateOrderStatus(Int32.Parse(hdnOrderId.Value), (int)AssestOrderReqStatus.Completed, intProfile);

            //If this service was returned then update the status of next service 
            if (boolServiceReturned == true)
            {
                DataSet dsRR = oResourceRequest.GetRequestService(intRequest, intService, intNumber);
                if (dsRR.Tables[0].Rows.Count > 0)
                {
                    int intRRId = Int32.Parse(dsRR.Tables[0].Rows[0]["parent"].ToString());

                    DataSet dsRRReturn = oResourceRequest.getResourceRequestReturn(intRRId, intService, intNumber, 1, 0);
                    if (dsRRReturn.Tables[0].Rows.Count > 0)
                    {
                        int intNextRRId = Int32.Parse(dsRRReturn.Tables[0].Rows[0]["NextRRId"].ToString());
                        int intNextServiceId = Int32.Parse(dsRRReturn.Tables[0].Rows[0]["NextServiceId"].ToString());
                        int intNextNumber = Int32.Parse(dsRRReturn.Tables[0].Rows[0]["NextNumber"].ToString()); ;


                        oResourceRequest.UpdateStatusRequest(intNextRRId, 2);
                        DataSet dsRRWF = oResourceRequest.GetWorkflowsParent(intNextRRId);
                        foreach (DataRow dr in dsRRWF.Tables[0].Rows)
                        {
                            int intRRWFId = Int32.Parse(dr["id"].ToString());
                            oResourceRequest.UpdateWorkflowStatus(intRRWFId, 2, true);
                        }
                    }
                }
                oResourceRequest.updateResourceRequestReturnCompleted(Int32.Parse(lblReqReturnedId.Text));

            }
            Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "reload", "<script type=\"text/javascript\">RefreshOpeningWindow();window.close();<" + "/" + "script>");

        }

     
        #region WM Tasks - Populate/ Add/Update

        #region Load WM Service Tasks

        private void LoadWMServiceTasks()
        {

            DataSet ds = oWMServiceTasks.getWMServiceTasksStatus(
                                                            intRequest,
                                                            intService,
                                                            intItem,
                                                            intNumber);

            dlWMServiceTaskList.DataSource = ds;
            dlWMServiceTaskList.DataBind();
            string strService = oService.Get(intService, "name");

        }

        protected void dlWMServiceTaskList_ItemDataBound(object sender, DataListItemEventArgs e)
        {

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataRowView drv = (DataRowView)e.Item.DataItem;

                HiddenField hdnTaskStatusId = (HiddenField)e.Item.FindControl("hdnTaskStatusId");
                hdnTaskStatusId.Value = drv["TaskStatusId"].ToString();


                HiddenField hdnTaskId = (HiddenField)e.Item.FindControl("hdnTaskId");
                hdnTaskId.Value = drv["TaskId"].ToString();


                Label lblWMServiceTaskListTask = (Label)e.Item.FindControl("lblWMServiceTaskListTask");
                lblWMServiceTaskListTask.Text = drv["TaskName"].ToString() + (drv["Completed"].ToString() != "" ? " [Completed : " + drv["Completed"].ToString() + "]" : "");

                CheckBox chkSelectTask = (CheckBox)e.Item.FindControl("chkSelectTask");

                Label lblWMServiceTaskListRequestAdd = (Label)e.Item.FindControl("lblWMServiceTaskListRequestAdd");


                TextBox txtRequestNo = (TextBox)e.Item.FindControl("txtRequestNo");
                Button btnWMServiceTaskReqAdd = (Button)e.Item.FindControl("btnWMServiceTaskReqAdd");

                btnWMServiceTaskReqAdd.Attributes.Add("onclick", "return ValidateText('" + txtRequestNo.ClientID + "','Please enter valid active request number')" +
                                                               " && ProcessControlButton()" +
                                                               ";");

                if (drv["Completed"].ToString() == "")
                {
                    chkSelectTask.Enabled = true;
                    chkSelectTask.Checked = false;
                }
                else
                {
                    chkSelectTask.Checked = true;
                    chkSelectTask.Enabled = false;

                }
                lblWMServiceTaskListTask.Enabled = chkSelectTask.Enabled;
                lblWMServiceTaskListRequestAdd.Enabled = chkSelectTask.Enabled;

                txtRequestNo.Enabled = chkSelectTask.Enabled;
                btnWMServiceTaskReqAdd.Enabled = chkSelectTask.Enabled;

                //Load the associated service request
                if (hdnTaskStatusId.Value != "")
                {
                    DataList dlWMServiceTaskReqList = (DataList)e.Item.FindControl("dlWMServiceTaskReqList");

                    DataTable dtWMServiceTaskReqs = getTaskReqDetails(Int32.Parse(hdnTaskStatusId.Value));
                    if (dtWMServiceTaskReqs != null)
                    {
                        foreach (DataRow drTmp in dtWMServiceTaskReqs.Rows)
                        {
                            if (drTmp["ServiceStatus"].ToString() != "3")
                                chkSelectTask.Enabled = false;
                        }
                        dlWMServiceTaskReqList.DataSource = dtWMServiceTaskReqs;
                        dlWMServiceTaskReqList.DataBind();
                        dlWMServiceTaskReqList.ItemDataBound +=
                         new DataListItemEventHandler(this.dlWMServiceTaskReqList_ItemDataBound);
                    }
                }

            }

        }

        protected void dlWMServiceTaskReqList_ItemDataBound(object sender, DataListItemEventArgs e)
        {

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataRowView drv = (DataRowView)e.Item.DataItem;

                HiddenField hdnTaskReqStatusId = (HiddenField)e.Item.FindControl("hdnTaskReqStatusId");
                hdnTaskReqStatusId.Value = drv["TaskStatusId"].ToString();

                HiddenField hdnTaskRequestId = (HiddenField)e.Item.FindControl("hdnTaskRequestId");
                hdnTaskRequestId.Value = drv["RequestId"].ToString();

                LinkButton lnkbtnAssetBnDTaskReqNo = (LinkButton)e.Item.FindControl("lnkbtnAssetBnDTaskReqNo");
                lnkbtnAssetBnDTaskReqNo.Text = drv["ReqServiceNumber"].ToString();
                lnkbtnAssetBnDTaskReqNo.Attributes.Add("onclick", "return OpenNewWindowMenu('/datapoint/service/resource.aspx?id=" + oFunction.encryptQueryString(drv["ResourceRequestID"].ToString()) + "', '800', '600');");

                Label lblWMServiceTaskReqListReqName = (Label)e.Item.FindControl("lblWMServiceTaskReqListReqName");
                lblWMServiceTaskReqListReqName.Text = drv["ServiceName"].ToString();

                Label lblWMServiceTaskReqListResource = (Label)e.Item.FindControl("lblWMServiceTaskReqListResource");
                lblWMServiceTaskReqListResource.Text = drv["UserAssignedToName"].ToString();

                Label lblWMServiceTaskReqListReqStatus = (Label)e.Item.FindControl("lblWMServiceTaskReqListReqStatus");
                lblWMServiceTaskReqListReqStatus.Text = drv["ServiceStatusName"].ToString();

                Label lblWMServiceTaskReqListLastUpdated = (Label)e.Item.FindControl("lblWMServiceTaskReqListLastUpdated");
                lblWMServiceTaskReqListLastUpdated.Text = (drv["RequestModified"].ToString() != "" ? DateTime.Parse(drv["RequestModified"].ToString()).ToShortDateString() : "");

                LinkButton lnkbtnWMServiceTaskReqListRemove = (LinkButton)e.Item.FindControl("lnkbtnWMServiceTaskReqListRemove");
                lnkbtnWMServiceTaskReqListRemove.Text = "Remove";
                lnkbtnWMServiceTaskReqListRemove.CommandArgument = drv["RequestId"].ToString();
                lnkbtnWMServiceTaskReqListRemove.CommandName = "REMOVEREQUEST";
                lnkbtnWMServiceTaskReqListRemove.Attributes.Add("onclick", "return confirm('Are you sure you want to remove all requests with request# " + drv["RequestNumber"].ToString() + " ?')&& ProcessControlButton();");

            }

        }

        protected void dlWMServiceTaskList_Command(object sender, DataListCommandEventArgs e)
        {

            if (e.CommandName.ToUpper() == "ADDREQUEST")
            {
                //Validate if its valid request number
                TextBox txtRequestNo = (TextBox)e.Item.FindControl("txtRequestNo");
                int intAssociatedRequest = 0;
                if (validateRequest(txtRequestNo.Text.Trim(), ref intAssociatedRequest) == false)
                {
                    Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "InvalidRequest", "<script type=\"text/javascript\">alert('Please enter valid request number.');<" + "/" + "script>");

                }
                else
                {
                    HiddenField hdnTaskId = (HiddenField)e.Item.FindControl("hdnTaskId");
                    HiddenField hdnTaskStatusId = (HiddenField)e.Item.FindControl("hdnTaskStatusId");

                    if (hdnTaskStatusId.Value == "0" || hdnTaskStatusId.Value == "")
                    {
                        int intTaskStatusId = 0;
                        intTaskStatusId = oWMServiceTasks.addWMServiceTaskStatus(intTaskStatusId,
                                    intRequest,
                                    intService,
                                    intItem,
                                    intNumber,
                                    Int32.Parse(hdnTaskId.Value),
                                    0,
                                    double.Parse("0"),
                                    intProfile, 0);
                        hdnTaskStatusId.Value = intTaskStatusId.ToString();
                    }



                    oWMServiceTasks.addRemoveWMServiceTasksStatusRequest(Int32.Parse(hdnTaskStatusId.Value),
                                                                        intAssociatedRequest, intProfile, 1, 0);
                    
                    int intResourceWorkflow = Int32.Parse(lblResourceWorkflow.Text);
                    Response.Redirect(Request.Path + "?rrid=" + intResourceWorkflow.ToString() + "&div=E&save=true");
                }
            }


        }

        protected void dlWMServiceTaskReqList_Command(object sender, DataListCommandEventArgs e)
        {

            if (e.CommandName.ToUpper() == "REMOVEREQUEST")
            {

                HiddenField hdnTaskReqStatusId = (HiddenField)e.Item.FindControl("hdnTaskReqStatusId");

                HiddenField hdnTaskRequestId = (HiddenField)e.Item.FindControl("hdnTaskRequestId");

                oWMServiceTasks.addRemoveWMServiceTasksStatusRequest(Int32.Parse(hdnTaskReqStatusId.Value),
                                                                    Int32.Parse(hdnTaskRequestId.Value), intProfile, 1, 1);

                int intResourceWorkflow = Int32.Parse(lblResourceWorkflow.Text);
                Response.Redirect(Request.Path + "?rrid=" + intResourceWorkflow.ToString() + "&div=E&save=true");
            }


        }

        protected bool validateRequest(string _requestnumber, ref int _intRequestId)
        {
            DataTable dtRequestsActive = null;
            //Get the Active Request 
            DataSet dsRequests = oDataPoint.GetServiceRequestSearchResults(
                                       _requestnumber,
                                       "",
                                       2, null, null, null,
                                       null,
                                       null,
                                       null, null, null, null, null, null,
                                       "SubmittedOn", 1, 1, 0);

            string[] strColumnNames ={ "RequestNumber", "RequestId" };
            dtRequestsActive = dsRequests.Tables[0].DefaultView.ToTable(true, strColumnNames);

            DataRow[] drReqs = null;
            string strFilterCriteria = ("RequestNumber ='" + _requestnumber + "'");
            drReqs = dtRequestsActive.Select(strFilterCriteria);

            if (drReqs.Length > 0)
            {
                _intRequestId = Int32.Parse(drReqs[0]["requestid"].ToString());
                return true;
            }
            else
                return false;
        }

        private DataTable getTaskReqDetails(int _taskStatusId)
        {
            DataTable dtTaskRequests = null;

            DataSet dsTaskReqs = oWMServiceTasks.getWMServiceTasksStatusRequest(_taskStatusId);
            foreach (DataRow dr in dsTaskReqs.Tables[0].Rows)
            {
                DataSet dsReqsDetails = oDataPoint.GetServiceRequestSearchResults(
                                        dr["RequestId"].ToString(),
                                        "",
                                        null, null, null, null,
                                        null,
                                        null,
                                        null, null, null, null, null, null,
                                        "SubmittedOn", 1, 1, 0);
                if (dtTaskRequests == null)
                    dtTaskRequests = dsReqsDetails.Tables[0];
                else
                {
                    foreach (DataRow drTmp in dsReqsDetails.Tables[0].Rows)
                    {
                        dtTaskRequests.ImportRow(drTmp);
                    }
                }

            }
            if (dtTaskRequests != null)
            {
                dtTaskRequests.Columns.Add("TaskStatusId", Type.GetType("System.Int32"));

                foreach (DataRow dr in dtTaskRequests.Rows)
                {
                    dr["TaskStatusId"] = _taskStatusId.ToString();
                }
            }
            return dtTaskRequests;
        }

        #endregion

        private void SaveWMServiceTaskStatus()
        {
            foreach (DataListItem dlItem in dlWMServiceTaskList.Items)
            {
                HiddenField hdnTaskId = (HiddenField)dlItem.FindControl("hdnTaskId");
                HiddenField hdnTaskStatusId = (HiddenField)dlItem.FindControl("hdnTaskStatusId");
                CheckBox chkSelectTask = (CheckBox)dlItem.FindControl("chkSelectTask");

                if (chkSelectTask.Checked == true && chkSelectTask.Enabled == true)
                {

                    int intTaskStatusId = 0;

                    if (hdnTaskStatusId.Value != "")
                        intTaskStatusId = Int32.Parse(hdnTaskStatusId.Value);

                    intTaskStatusId = oWMServiceTasks.addWMServiceTaskStatus(intTaskStatusId,
                                    intRequest,
                                    intService,
                                    intItem,
                                    intNumber,
                                    Int32.Parse(hdnTaskId.Value),
                                    0,
                                    double.Parse("0"),
                                    intProfile, 1);
                }
            }

        }

        #endregion

        #region Datastore

        private void LoadDataStores()
        {

            DataSet ds = oAssetSharedEnvOrder.GetDataStoreSelection(Int32.Parse(hdnOrderId.Value));
            dlDataStoreSelection.DataSource = ds;
            dlDataStoreSelection.DataBind();
            lblDataStoreNoItems.Visible = (dlDataStoreSelection.Items.Count > 0 ? false : true);
        }

        protected void dlDataStoreSelection_ItemDataBound(object sender, DataListItemEventArgs e)
        {

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataRowView drv = (DataRowView)e.Item.DataItem;

                Label lblDataStoreName = (Label)e.Item.FindControl("lblDataStoreName");
                lblDataStoreName.Text = drv["Name"].ToString() ;

                Label lblDataStoreStorageType = (Label)e.Item.FindControl("lblDataStoreStorageType");
                lblDataStoreStorageType.Text = drv["Storage_Type"].ToString();

                Label lblDataStoreReplicated = (Label)e.Item.FindControl("lblDataStoreReplicated");
                lblDataStoreReplicated.Text = drv["Replicated"].ToString();

                LinkButton lnkbtnDataStoreDelete = (LinkButton)e.Item.FindControl("lnkbtnDataStoreDelete");
                lnkbtnDataStoreDelete.Text = "DELETE";
                lnkbtnDataStoreDelete.CommandArgument = drv["Id"].ToString();
                lnkbtnDataStoreDelete.CommandName = "DELETEDATASTORE";

                int intResourceWorkflow = Int32.Parse(lblResourceWorkflow.Text);
                bool boolComplete = (oResourceRequest.GetWorkflow(intResourceWorkflow, "status") == "3");
                if ((oResourceRequest.CanUpdate(intProfile, intResourceWorkflow, false) == false) || boolComplete == true)
                    lnkbtnDataStoreDelete.Enabled = false;
                else
                    lnkbtnDataStoreDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to remove datastore " + drv["Name"].ToString() + " ?')&& ProcessControlButton();");
               

             }

            }

        protected void dlDataStoreSelection_Command(object sender, DataListCommandEventArgs e)
        {

            if (e.CommandName.ToUpper() == "DELETEDATASTORE")
            {
                oAssetSharedEnvOrder.DeleteDataStoreSelection(Int32.Parse(e.CommandArgument.ToString()),intProfile);

                int intResourceWorkflow = Int32.Parse(lblResourceWorkflow.Text);
                Response.Redirect(Request.Path + "?rrid=" + intResourceWorkflow.ToString() + "&div=E&save=true");
            }


        }

        protected void btnAddDataStoreSelection_Click(Object Sender, EventArgs e)
        {
            //Get Parent
            DataSet ds = oAssetSharedEnvOrder.Get(intRequest, intItem, intNumber);
            int intOrderId = 0;
            int intOrderType = 0;
            int intClusterId = 0;
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];

                intOrderId = Int32.Parse(dr["OrderId"].ToString());
                intOrderType = Int32.Parse(dr["OrderType"].ToString());

                VMWare oVMWare = new VMWare(intProfile, dsn);
                Servers oServer = new Servers(intProfile, dsn);

                if (intOrderType == (int)AssetSharedEnvOrderType.AddCluster)//Add Cluster -Get folder
                {
                    DataSet dsServer = oServer.GetRequests(intRequest, 0);
                    if (dsServer.Tables[0].Rows.Count > 0)
                        intClusterId = Int32.Parse(dsServer.Tables[0].Rows[0]["vmware_clusterid"].ToString());
                }
                if (intOrderType == (int)AssetSharedEnvOrderType.AddHost || intOrderType == (int)AssetSharedEnvOrderType.AddStorage)//Add Host -Get Cluster
                    intClusterId = Int32.Parse(dr["ParentId"].ToString());


                oAssetSharedEnvOrder.AddDataStoreSelection(
                    Int32.Parse(hdnOrderId.Value),
                    intClusterId,
                    txtDataStoreName.Text.Trim(),
                    Int32.Parse(ddlDataStoreSelectionStorageType.SelectedValue),
                    (chkDataStoreReplicated.Checked ? 1 : 0), intProfile);

                int intResourceWorkflow = Int32.Parse(lblResourceWorkflow.Text);
                Response.Redirect(Request.Path + "?rrid=" + intResourceWorkflow.ToString() + "&div=E&save=true");
            }
        }

        private void AddDataStores()
        {
            VMWare oVMWare = new VMWare(intProfile, dsn);
            DataSet ds = oAssetSharedEnvOrder.GetDataStoreSelection(Int32.Parse(hdnOrderId.Value));
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                     oVMWare.AddDatastore(Int32.Parse(dr["ClusterId"].ToString()),
                     dr["Name"].ToString(),
                     Int32.Parse(dr["Storage_Type"].ToString()), 0,
                     Int32.Parse(dr["Replicated"].ToString()), 99999, 1, 0, 0, 0, 1);
            }


        }

        #endregion

    }
}