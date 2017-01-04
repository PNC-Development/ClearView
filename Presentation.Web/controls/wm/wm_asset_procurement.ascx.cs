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
    public partial class wm_asset_procurement : System.Web.UI.UserControl
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

        // For server Workstation Errors
        protected AssetOrder oAssetOrder;
        protected Variables oVariable;


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
            oAssetOrder = new AssetOrder(intProfile, dsn, dsnAsset, intEnvironment);
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


                    LoadLists();
                    oFunction.ConfigureToolButton(btnEmail, "/images/tool_email");
                    oFunction.ConfigureToolButton(btnPrint, "/images/tool_print");
                    oFunction.ConfigureToolButton(btnClose, "/images/tool_close");
                    oFunction.ConfigureToolButton(btnSave, "/images/tool_save");
                    oFunction.ConfigureToolButton(btnComplete, "/images/tool_complete");
                    oFunction.ConfigureToolButton(btnSLA, "/images/tool_sla");

                    btnDenied.Attributes.Add("onclick", "return CloseWindow();");
                    btnPrint.Attributes.Add("onclick", "return PrintWindow();");
                    btnClose.Attributes.Add("onclick", "return ExitWindow();");


                    btnSave.Attributes.Add("onclick", "return ValidateText('" + txtNickName.ClientID + "','Please enter the nick name')" +
                       " && ValidateNumber('" + txtApprovedQuantity.ClientID + "','Please enter the quantity.')" +
                       //" && ValidateNumber('" + txtProcureQuantity.ClientID + "','Please enter the quantity.')" +
                       //" && ValidateNumber('" + txtReDeployQuantity.ClientID + "','Please enter the re-deploy quantity.')" +
                       //" && ValidateNumber('" + txtReturnedQuantity.ClientID + "','Please enter the returned quantity.')" +
                       " && ValidateDropDown('" + ddlPurchaseOrderStatus.ClientID + "','Please select the purchase order status')" +
                       " && ValidatePurchaseOrderStatus()" +
                       ";");
                    //" && ValidateStatus('" + ddlStatus.ClientID + "','" + txtComments.ClientID + "')" +
                    btnStatus.Attributes.Add("onclick", "return ValidateStatus('" + ddlStatus.ClientID + "','" + txtComments.ClientID + "')" +
                      ";");

                    imgBtnPurchaseOrderDate.Attributes.Add("onclick", "return ShowCalendar('" + txtPurchaseOrderDate.ClientID + "');");
                    imgApprovedOn.Attributes.Add("onclick", "return ShowCalendar('" + txtApprovedOn.ClientID + "');");
                    imgQuoteDate.Attributes.Add("onclick", "return ShowCalendar('" + txtQuoteDate.ClientID + "','" + txtWarrantyDate.ClientID + "',1095);");
                    imgBtnVendorOrderDate.Attributes.Add("onclick", "return ShowCalendar('" + txtVendorOrderDate.ClientID + "');");
                    txtAttentionTo.Attributes.Add("onkeyup", "return AJAXTextBoxGet(this,'250','195','" + divAttentionTo.ClientID + "','" + lstAttentionTo.ClientID + "','" + hdnAttentionTo.ClientID + "','" + oVariable.URL() + "/frame/users.aspx',2);");
                    lstAttentionTo.Attributes.Add("ondblclick", "AJAXClickRow();");
                    txtProjectManager.Attributes.Add("onkeyup", "return AJAXTextBoxGet(this,'250','195','" + divProjectManager.ClientID + "','" + lstProjectManager.ClientID + "','" + hdnProjectManager.ClientID + "','" + oVariable.URL() + "/frame/users.aspx',2);");
                    lstProjectManager.Attributes.Add("ondblclick", "AJAXClickRow();");

                    
                    imgDate.Attributes.Add("onclick", "return ShowCalendar('" + txtDate.ClientID + "');");

                }
             
                LoadRequest();
            }
            else
                panDenied.Visible = true;

          

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

                //Change Control and Documents
                LoadChange(intResourceWorkflow);
                lblDocuments.Text = oDocument.GetDocuments_Service(intRequest, intService, oVariable.DocumentsFolder(), 1, (Request.QueryString["doc"] != null));


                // 6/1/2009 - Load ReadOnly View
                if ((oResourceRequest.CanUpdate(intProfile, intResourceWorkflow, false) == false) || boolComplete == true)
                    oFunction.ConfigureToolButtonRO(btnSave, btnComplete);
                
                btnReturn.Visible = false;
                //btnComplete.Visible = false;
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

        private void LoadLists()
        {
            ddlPurchaseOrderStatus.DataValueField = "StatusValue";
            ddlPurchaseOrderStatus.DataTextField = "StatusDescription";
            DataSet ds = oStatusLevel.GetStatusList("PURCHASEORDERSTATUS");
            DataRow drRemove = null;
            //Remove Skip status
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (Int32.Parse(dr["StatusValue"].ToString()) == 4)
                    drRemove = dr;
            }
            if (drRemove != null)
                ds.Tables[0].Rows.Remove(drRemove);

            ddlPurchaseOrderStatus.DataSource = ds.Tables[0];

            ddlPurchaseOrderStatus.DataBind();
            ddlPurchaseOrderStatus.Items.Insert(0, new ListItem("-- Select --", "0"));

            ddlVendorOrderStatus.DataValueField = "StatusValue";
            ddlVendorOrderStatus.DataTextField = "StatusDescription";
            ddlVendorOrderStatus.DataSource = oStatusLevel.GetStatusList("VENDORORDERSTATUS");
            ddlVendorOrderStatus.DataBind();
            ddlVendorOrderStatus.Items.Insert(0, new ListItem("-- Select --", "0"));

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

        private void LoadInformation()
        {
            lblView.Text = oAssetOrder.GetOrderBody(intRequest, intItem, intNumber);
           
            //Get the Execution Tab Details
            DataSet ds= oAssetOrder.Get(intRequest, intItem, intNumber);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];

                hdnOrderId.Value = dr["OrderId"].ToString();
                int intProjectId = 0;
                Int32.TryParse(dr["ProjectId"].ToString(), out intProjectId);
                if (Request.QueryString["projectid"] != null)
                {
                    if (Request.QueryString["projectid"] != "")
                        Int32.TryParse(Request.QueryString["projectid"], out intProjectId);
                    else
                        Page.ClientScript.RegisterStartupScript(typeof(Page), "reload", "<script type=\"text/javascript\">alert('There were " + Request.QueryString["projects"] + " projects found with that project number.  Please try again.');<" + "/" + "script>");
                }
                DataSet dsProject = oProject.Get(intProjectId);
                if (dsProject.Tables[0].Rows.Count == 1)
                {
                    txtProjectNumber.Text = dsProject.Tables[0].Rows[0]["number"].ToString();
                    txtProjectName.Text = dsProject.Tables[0].Rows[0]["name"].ToString();
                    int intLead = 0;
                    Int32.TryParse(dsProject.Tables[0].Rows[0]["lead"].ToString(), out intLead);
                    if (intLead > 0)
                    {
                        txtProjectManager.Text = oUser.GetFullName(intLead) + " (" + oUser.GetName(intLead) + ")";
                        txtProjectManager.Enabled = false;
                    }
                    else
                        txtProjectManager.Enabled = true;

                }
                hdnProjectId.Value = intProjectId.ToString();
                
                txtNickName.Text = dr["NickName"].ToString();

                txtModel.Text = dr["ModelName"].ToString();
                hdnModel.Value = dr["ModelId"].ToString();

                txtQuantity.Text = dr["RequestedQuantity"].ToString();

                txtProcureQuantity.Text = dr["ProcureQuantity"].ToString();
                txtReDeployQuantity.Text = dr["ReDeployQuantity"].ToString();
                txtReturnedQuantity.Text = dr["ReturnedQuantity"].ToString();

                txtPurchaseOrderNumber.Text = dr["PurchaseOrderNumber"].ToString();
                ddlPurchaseOrderStatus.SelectedValue = (dr["PurchaseOrderStatusId"].ToString() != "" ? dr["PurchaseOrderStatusId"].ToString() : "0");
                txtPurchaseOrderDate.Text = (dr["PurchaseOrderDate"].ToString() != "" ? DateTime.Parse(dr["PurchaseOrderDate"].ToString()).ToShortDateString() : "");
                txtApprovedQuantity.Text = dr["ApprovedQuantity"].ToString();
                txtApprovedOn.Text = (dr["ApprovedOn"].ToString() != "" ? DateTime.Parse(dr["ApprovedOn"].ToString()).ToShortDateString() : "");
                if (dr["PurchaseOrderUpload"].ToString() != "")
                {
                    hypPurchaseOrderUpload.Text = "<img src='/images/file.gif' border='0' align='absmiddle'/> Click here to view the file";
                    hypPurchaseOrderUpload.NavigateUrl = dr["PurchaseOrderUpload"].ToString();
                }
                else
                    hypPurchaseOrderUpload.Text = "<img src='/images/alert.gif' border='0' align='absmiddle'/> Please upload a file";

                txtQuoteNumber.Text = dr["QuoteNumber"].ToString();
                txtQuoteDate.Text = (dr["QuoteDate"].ToString() != "" ? DateTime.Parse(dr["QuoteDate"].ToString()).ToShortDateString() : "");
                txtWarrantyDate.Text = (dr["WarrantyDate"].ToString() != "" ? DateTime.Parse(dr["WarrantyDate"].ToString()).ToShortDateString() : "");
                txtSystemPrice.Text = (dr["SystemPrice"].ToString() != "" ? dr["SystemPrice"].ToString() : "0.00");
                txtPurchaseOrderPrice.Text = (dr["PurchaseOrderPrice"].ToString() != "" ? dr["PurchaseOrderPrice"].ToString() : "0.00");
                txtSalesTax.Text = (dr["SalesTax"].ToString() != "" ? dr["SalesTax"].ToString() : "0.00");
                if (dr["ManufacturerQuoteUpload"].ToString() != "")
                {
                    hypManufacturerQuoteUpload.Text = "<img src='/images/file.gif' border='0' align='absmiddle'/> Click here to view the file";
                    hypManufacturerQuoteUpload.NavigateUrl = dr["ManufacturerQuoteUpload"].ToString();
                }
                else
                    hypManufacturerQuoteUpload.Text = "<img src='/images/alert.gif' border='0' align='absmiddle'/> Please upload a file";

                txtVendorTrackingNumber.Text = dr["VendorTrackingNumber"].ToString();
                txtVendorOrderDate.Text = (dr["VendorOrderDate"].ToString() != "" ? DateTime.Parse(dr["VendorOrderDate"].ToString()).ToShortDateString() : "");
                ddlVendorOrderStatus.SelectedValue = (dr["VendorOrderStatusId"].ToString() != "" ? dr["VendorOrderStatusId"].ToString() : "0");
                int intAttentionTo = 0;
                if (Int32.TryParse(dr["AttentionTo"].ToString(), out intAttentionTo) == true)
                {
                    if (intAttentionTo > 0)
                        txtAttentionTo.Text = oUser.GetFullName(intAttentionTo) + " (" + oUser.GetName(intAttentionTo) + ")";
                }
                hdnAttentionTo.Value = intAttentionTo.ToString();

                txtPurchaseOrderComments.Text = dr["PurchaseOrderComments"].ToString();

                //if Purchase Order status= Approved and Vendor Order Status = Shipped OR
                //if Purchase Order status= Rejected Or if Purchase Order status= Skip
                if ((dr["PurchaseOrderStatusId"].ToString() == "2" && dr["VendorOrderStatusId"].ToString() == "1") ||
                    (dr["PurchaseOrderStatusId"].ToString() == "3" || dr["PurchaseOrderStatusId"].ToString() == "4"))
                    btnComplete.Visible = true;
                else
                    btnComplete.Visible = false;

                //Populate Existing comments
                PopulateOrderReqComments();
               
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
            //Add Comments            
            AddOrderReqComments();

            UpdateProcurementDetails();
            oServiceDetail.UpdateCheckboxes(Request, intResourceWorkflow, intRequest, intItem, intNumber);
            double dblAllocated = oResourceRequest.GetDetailsHoursUsed(intRequest, intItem, intNumber, intResourceWorkflow, false);
            oResourceRequest.UpdateWorkflowAllocated(intResourceWorkflow, dblAllocated);

            oResourceRequest.UpdateWorkflowName(intResourceWorkflow, txtCustom.Text, intProfile);
            Response.Redirect(Request.Path + "?rrid=" + intResourceWorkflow.ToString() + "&div=E&save=true");
        }

        protected void btnComplete_Click(Object Sender, EventArgs e)
        {
            CompletedWorkflow();
        }

        protected void btnProject_Click(Object Sender, EventArgs e)
        {
            DataSet dsProject = oProject.Get(txtProjectNumber.Text);
            if (dsProject.Tables[0].Rows.Count == 1)
                Response.Redirect(Request.Path + "?rrid=" + Request.QueryString["rrid"] + "&projectid=" + dsProject.Tables[0].Rows[0]["projectid"].ToString() + "&div=E");
            else
                Response.Redirect(Request.Path + "?rrid=" + Request.QueryString["rrid"] + "&projectid=&projects=" + dsProject.Tables[0].Rows.Count.ToString() + "&div=E");
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

            //update Procurement Details Initiate new Services or Complete Request
            UpdateProcurementDetails();

            //Asset Order => Initiate New Service Or Complete Request
            oAssetOrder.InitiateNextServiceRequestOrCompleteRequest(Int32.Parse(hdnOrderId.Value),intNumber, intService,false,dsnServiceEditor,intAssignPage,intViewPage,dsnAsset,dsnIP);
           
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

       
     
        #region Asset Order

        private void UpdateProcurementDetails()
        {
            DateTime? dtPurchaseOrderDate = null;
            DateTime? dtVendorOrderDate = null;
            DateTime? dtApprovedOn = null;
            DateTime? dtQuoteDate = null;
            DateTime? dtWarrantyDate = null;

            int intOrderID = Int32.Parse(hdnOrderId.Value);

            if (txtPurchaseOrderDate.Text.Trim() != "")
                dtPurchaseOrderDate = DateTime.Parse(txtPurchaseOrderDate.Text.Trim());

            if (txtVendorOrderDate.Text.Trim() != "")
                dtVendorOrderDate = DateTime.Parse(txtVendorOrderDate.Text.Trim());

            if (txtApprovedOn.Text.Trim() != "")
                dtApprovedOn = DateTime.Parse(txtApprovedOn.Text.Trim());

            if (txtQuoteDate.Text.Trim() != "")
            {
                dtQuoteDate = DateTime.Parse(txtQuoteDate.Text.Trim());
                dtWarrantyDate = dtQuoteDate.Value.AddYears(3);
            }

            string strPurchaseOrderUpload = hypPurchaseOrderUpload.NavigateUrl;
            if (filPurchaseOrderUpload.FileName != "" && filPurchaseOrderUpload.PostedFile != null)
            {
                string strDirectory = oVariable.UploadsFolder() + "PROCURE\\";
                if (Directory.Exists(strDirectory) == false)
                    Directory.CreateDirectory(strDirectory);
                string strExtension = filPurchaseOrderUpload.PostedFile.FileName.Trim();
                strExtension = strExtension.Substring(strExtension.LastIndexOf("."));
                strPurchaseOrderUpload = strDirectory + "Purchase_Order_" + intOrderID.ToString() + strExtension;
                filPurchaseOrderUpload.PostedFile.SaveAs(strPurchaseOrderUpload);
            }
            string strManufacturerQuoteUpload = hypManufacturerQuoteUpload.NavigateUrl;
            if (filManufacturerQuoteUpload.FileName != "" && filManufacturerQuoteUpload.PostedFile != null)
            {
                string strDirectory = oVariable.UploadsFolder() + "PROCURE\\";
                if (Directory.Exists(strDirectory) == false)
                    Directory.CreateDirectory(strDirectory);
                string strExtension = filManufacturerQuoteUpload.PostedFile.FileName.Trim();
                strExtension = strExtension.Substring(strExtension.LastIndexOf("."));
                strManufacturerQuoteUpload = strDirectory + "Manufacturer_Quote_" + intOrderID.ToString() + strExtension;
                filManufacturerQuoteUpload.PostedFile.SaveAs(strManufacturerQuoteUpload);
            }
            int intProjectID = 0;
            Int32.TryParse(Request.Form[hdnProjectId.UniqueID], out intProjectID);

            int intProjectManager = 0;
            Int32.TryParse(Request.Form[hdnProjectManager.UniqueID], out intProjectManager);

            if (intProjectManager > 0 && intProjectID > 0)
            {
                DataSet dsProject = oProject.Get(intProjectID);
                if (dsProject.Tables[0].Rows.Count == 1)
                {
                    int intLead = 0;
                    Int32.TryParse(dsProject.Tables[0].Rows[0]["lead"].ToString(), out intLead);
                    if (intLead == 0)
                        oProject.Update(intProjectID, intProjectManager, 0, 0, 0, 0, 0);
                }
            }

            int intAttentionTo = 0;
            Int32.TryParse(Request.Form[hdnAttentionTo.UniqueID], out intAttentionTo);

            float fltPurchaseOrderPrice = 0;
            float.TryParse(txtPurchaseOrderPrice.Text, out fltPurchaseOrderPrice);

            int intApprovedQuantity = 0;
            Int32.TryParse(txtApprovedQuantity.Text, out intApprovedQuantity);

            float fltSystemPrice = 0;
            float.TryParse(txtSystemPrice.Text, out fltSystemPrice);

            float fltSalesTax = 0;
            float.TryParse(txtSalesTax.Text, out fltSalesTax);

            oAssetOrder.UpdateProcurementDetails(
                                Int32.Parse(hdnOrderId.Value),
                                txtNickName.Text.Trim(),
                                Int32.Parse(txtProcureQuantity.Text.Trim()),
                                Int32.Parse(txtReDeployQuantity.Text.Trim()),
                                Int32.Parse(txtReturnedQuantity.Text.Trim()),
                                Int32.Parse(ddlPurchaseOrderStatus.SelectedValue),
                                txtPurchaseOrderComments.Text.Trim(),
                                dtPurchaseOrderDate,
                                fltPurchaseOrderPrice,
                                Int32.Parse(ddlVendorOrderStatus.SelectedValue),
                                dtVendorOrderDate,
                                txtVendorTrackingNumber.Text.Trim(),
                                intProjectID,
                                txtPurchaseOrderNumber.Text,
                                intApprovedQuantity,
                                dtApprovedOn,
                                strPurchaseOrderUpload,
                                txtQuoteNumber.Text,
                                dtQuoteDate,
                                dtWarrantyDate,
                                fltSystemPrice,
                                fltSalesTax,
                                strManufacturerQuoteUpload,
                                intAttentionTo,
                                intProfile);

        }

        protected void PopulateOrderReqComments()
        {
            //Load comments
            if (hdnOrderId.Value != "")
            {

                DataSet dsComments = oAssetOrder.GetAssetOrderComments(Int32.Parse(hdnOrderId.Value));
                dlOrderReqComments.DataSource = dsComments;
                dlOrderReqComments.DataBind();
                lblOrderReqCommentsNoComments.Visible = (dlOrderReqComments.Items.Count == 0);
            }

        }

        private void AddOrderReqComments()
        {
            if (txtOrderReqComments.Text.Trim() != "" && hdnOrderId.Value != "")
            {
                oAssetOrder.AddUpdateAssetOrderComment(
                                    0,
                                    Int32.Parse(hdnOrderId.Value),
                                    txtOrderReqComments.Text.Trim(),
                                    intProfile, 0);
                txtOrderReqComments.Text = "";
            }
        }

        protected void dlOrderReqComments_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            Users oUser = new Users(intProfile, dsn);
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataRowView drv = (DataRowView)e.Item.DataItem;
                Label lblOrderReqComments = (Label)e.Item.FindControl("lblOrderReqComments");
                lblOrderReqComments.Text = drv["Comments"].ToString();

                Label lblOrderReqUpdatedBy = (Label)e.Item.FindControl("lblOrderReqUpdatedBy");
                lblOrderReqUpdatedBy.Text = oUser.GetFullName(Int32.Parse(drv["ModifiedBy"].ToString()));

                Label lblOrderReqLastUpdated = (Label)e.Item.FindControl("lblOrderReqLastUpdated");
                lblOrderReqLastUpdated.Text = drv["modified"].ToString();

                LinkButton lnkbtnOrderReqDelete = (LinkButton)e.Item.FindControl("lnkbtnOrderReqDelete");
                lnkbtnOrderReqDelete.Text = "Delete";
                lnkbtnOrderReqDelete.CommandArgument = drv["Id"].ToString();

                if (intProfile != Int32.Parse(drv["ModifiedBy"].ToString()) || drv["OrderStatusId"].ToString() == "4")
                    lnkbtnOrderReqDelete.Enabled = false;
                else
                    lnkbtnOrderReqDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this comment?')&& ProcessControlButton();");

            }

        }

        protected void btnAddOrderReqComments_Click(Object Sender, EventArgs e)
        {
            int intResourceWorkflow = Int32.Parse(lblResourceWorkflow.Text);
            AddOrderReqComments();
            //PopulateOrderReqComments();
            Response.Redirect(Request.Path + "?rrid=" + intResourceWorkflow.ToString() + "&div=E&save=true");
        }

        protected void lnkbtnOrderReqDelete_Click(Object Sender, EventArgs e)
        {
            int intResourceWorkflow = Int32.Parse(lblResourceWorkflow.Text);
            LinkButton oButton = (LinkButton)Sender;
            oAssetOrder.DeleteAssetOrderComment(Int32.Parse(oButton.CommandArgument), intProfile);
            //PopulateOrderReqComments();
            Response.Redirect(Request.Path + "?rrid=" + intResourceWorkflow.ToString() + "&div=E&save=true");

        }

        #endregion

       
    }
}