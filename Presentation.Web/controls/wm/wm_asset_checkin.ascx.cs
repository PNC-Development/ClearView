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
    public partial class wm_asset_checkin : System.Web.UI.UserControl
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

        
        protected AssetOrder oAssetOrder;
        protected Asset oAsset;
        protected Variables oVariable;
        protected Locations oLocation;
        protected Depot oDepot;

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

        DataTable dtAsset= new DataTable();
       
        protected string strMenuTab1 = "";
       
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
            oAsset = new Asset(0, dsnAsset);
            oVariable = new Variables(intEnvironment);
            oLocation = new Locations(intProfile, dsn);
            oDepot = new Depot(intProfile, dsn);

            if (Request.QueryString["applicationid"] != null && Request.QueryString["applicationid"] != "")
                intApplication = Int32.Parse(Request.QueryString["applicationid"]);
            if (Request.QueryString["pageid"] != null && Request.QueryString["pageid"] != "")
                intPage = Int32.Parse(Request.QueryString["pageid"]);
            if (Request.Cookies["application"] != null && Request.Cookies["application"].Value != "")
                intApplication = Int32.Parse(Request.Cookies["application"].Value);
            if (Request.QueryString["rrid"] != null && Request.QueryString["rrid"] != "")
            {
                LoadAssetTable();
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

                    
                    btnSave.Attributes.Add("onclick", "return ValidateDropDown('" + ddlDepot.ClientID + "','Please select a location received')" +
                       " && ValidateDate('" + txtDateReceived.ClientID + "','Please enter date received')" +
                       " && ValidateSelectedAssets()" +
                       ";");
                    
                    //" && ValidateStatus('" + ddlStatus.ClientID + "','" + txtComments.ClientID + "')" +

                    btnStatus.Attributes.Add("onclick", "return ValidateStatus('" + ddlStatus.ClientID + "','" + txtComments.ClientID + "')" +
                                            ";");

                    imgbtnDateReceived.Attributes.Add("onclick", "return ShowCalendar('" + txtDateReceived.ClientID + "');");

                    imgDate.Attributes.Add("onclick", "return ShowCalendar('" + txtDate.ClientID + "');");

                    LoadList();

                }
             
                LoadRequest();
            }
            else
                panDenied.Visible = true;
  
        }

        private void LoadList()
        {
            ddlDepot.DataValueField = "id";
            ddlDepot.DataTextField = "name";
            ddlDepot.DataSource = oDepot.Gets(1);
            ddlDepot.DataBind();
            ddlDepot.Items.Insert(0, new ListItem("-- Select --", "0"));
        }

        private void LoadAssetTable()
        {
            dtAsset.Columns.Add("AssetID", System.Type.GetType("System.Int32"));
            dtAsset.Columns.Add("Serial", System.Type.GetType("System.String"));
            dtAsset.Columns.Add("AssetTag", System.Type.GetType("System.String"));
            dtAsset.Columns.Add("ValidAsset", System.Type.GetType("System.Int32"));
            dtAsset.Columns.Add("Returned", System.Type.GetType("System.Int32"));
            dtAsset.Columns.Add("Comments", System.Type.GetType("System.String"));
            dtAsset.Columns.Add("ScanDateTime", System.Type.GetType("System.DateTime"));
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
                if (Request.QueryString["save"] != null && Request.QueryString["save"] != "")
                    Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "saved", "<script type=\"text/javascript\">alert('Information Saved Successfully');<" + "/" + "script>");
                if (Request.QueryString["status"] != null && Request.QueryString["status"] != "")
                    Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "statusd", "<script type=\"text/javascript\">alert('Status Updates has been Added');<" + "/" + "script>");


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
                if ((oResourceRequest.CanUpdate(intProfile, intResourceWorkflow, false) == false) || boolComplete ==true)
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

        private void LoadInformation()
        {
            lblView.Text = oAssetOrder.GetOrderBody(intRequest, intItem, intNumber);

            //Get the Execution Tab Details
            DataSet ds = oAssetOrder.Get(intRequest, intItem, intNumber);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];

                hdnOrderId.Value = dr["OrderId"].ToString();
                hdnProcureQuantity.Value = dr["ProcureQuantity"].ToString();
                hdnModel.Value = dr["ModelId"].ToString();
                ddlDepot.SelectedValue = (dr["ReceivedDepotId"].ToString() == "" ? "0" : dr["ReceivedDepotId"].ToString());
                txtDateReceived.Text = (dr["ReceivedDate"].ToString() != "" ? DateTime.Parse(dr["ReceivedDate"].ToString()).ToShortDateString() : "");

                //Populate Existing comments
                PopulateOrderReqComments();

                LoadAssetsForOrder();
               }
        }

        private void LoadAssetsForOrder()
        {
            DataSet dsAsset = oAsset.GetAssetsByOrder(Int32.Parse(hdnOrderId.Value));
            foreach (DataRow drAssetTemp in dsAsset.Tables[0].Rows)
            {
                DataRow drAsset = dtAsset.NewRow();
                drAsset["AssetID"] = Int32.Parse(drAssetTemp["id"].ToString());
                drAsset["Serial"] = drAssetTemp["serial"].ToString();
                drAsset["AssetTag"] = drAssetTemp["asset"].ToString();
                drAsset["ValidAsset"] = 1;
                drAsset["Returned"] = Int32.Parse(drAssetTemp["returned"].ToString() != "" ? drAssetTemp["returned"].ToString() : "0");
                drAsset["Comments"] = drAssetTemp["comments"].ToString();
                if (drAssetTemp["datestamp"].ToString() != "")
                    drAsset["ScanDateTime"] = DateTime.Parse(drAssetTemp["datestamp"].ToString());

                dtAsset.Rows.Add(drAsset);
            }

            if (dsAsset.Tables[0].Rows.Count.ToString() == hdnProcureQuantity.Value)
                btnComplete.Visible = true;
            else
                btnComplete.Visible = false;

            dlAssetList.DataSource = dtAsset;
            dlAssetList.DataBind();
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
            if (IsValid() == true)
            {
                int intResourceWorkflow = Int32.Parse(lblResourceWorkflow.Text);
                int intResourceParent = oResourceRequest.GetWorkflowParent(intResourceWorkflow);

                //Add Comments            
                AddOrderReqComments();
                //Add update selected assets
                AddUpdateAssets();

                oServiceDetail.UpdateCheckboxes(Request, intResourceWorkflow, intRequest, intItem, intNumber);
                double dblAllocated = oResourceRequest.GetDetailsHoursUsed(intRequest, intItem, intNumber, intResourceWorkflow, false);
                oResourceRequest.UpdateWorkflowAllocated(intResourceWorkflow, dblAllocated);

                oResourceRequest.UpdateWorkflowName(intResourceWorkflow, txtCustom.Text, intProfile);
                Response.Redirect(Request.Path + "?rrid=" + intResourceWorkflow.ToString() + "&div=E&save=true" );
            }
        }

        protected void btnComplete_Click(Object Sender, EventArgs e)
        {
            CompletedWorkflow();
        }

        private bool IsValid()
        {
            foreach (DataListItem dlItem in dlAssetList.Items)
            {
             CheckBox chkAssetReturn = (CheckBox)dlItem.FindControl("chkAssetReturn");
                if (chkAssetReturn.Checked == true)
                {
                   
                    TextBox txtAssetListComments = (TextBox)dlItem.FindControl("txtAssetListComments");
                    if (txtAssetListComments.Text == "")
                    {
                        Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "save", "<script type=\"text/javascript\">alert('Please enter comments for returning asset(s) ');<" + "/" + "script>");
                        return false;
                    }
                }
            }

            return true;
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

            //Asset Order => Initiate New Service Or Complete Request
            oAssetOrder.InitiateNextServiceRequestOrCompleteRequest(Int32.Parse(hdnOrderId.Value), intNumber, intService, false, dsnServiceEditor, intAssignPage, intViewPage, dsnAsset, dsnIP);
            
           
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

        protected void dlAssetList_ItemDataBound(object sender, DataListItemEventArgs e)
        {

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataRowView drv = (DataRowView)e.Item.DataItem;

                HiddenField hdnAssetId = (HiddenField)e.Item.FindControl("hdnAssetId");
                hdnAssetId.Value = drv["AssetId"].ToString();

                Label lblAssetListSerialNo = (Label)e.Item.FindControl("lblAssetListSerialNo");
                lblAssetListSerialNo.Text = drv["Serial"].ToString();

                Label lblAssetListAssetTag = (Label)e.Item.FindControl("lblAssetListAssetTag");
                lblAssetListAssetTag.Text = drv["AssetTag"].ToString();


                Label lblAssetListAssetScanDateTime = (Label)e.Item.FindControl("lblAssetListAssetScanDateTime");
                lblAssetListAssetScanDateTime.Text = drv["ScanDateTime"].ToString();

                CheckBox chkSelectAsset = (CheckBox)e.Item.FindControl("chkSelectAsset");
                if (drv["AssetId"].ToString() != "")
                {
                    chkSelectAsset.Checked = true;
                    chkSelectAsset.Enabled = false;
                }


                CheckBox chkAssetReturn = (CheckBox)e.Item.FindControl("chkAssetReturn");
                if (drv["returned"].ToString() == "1")
                    chkAssetReturn.Checked = true;

                if (drv["ValidAsset"].ToString() == "0")
                {
                    chkSelectAsset.Enabled = false;
                    chkSelectAsset.Checked = false;
                    chkAssetReturn.Enabled = false;
                    chkAssetReturn.Checked = false;
                }

                TextBox txtAssetListComments = (TextBox)e.Item.FindControl("txtAssetListComments");
                txtAssetListComments.Text = drv["Comments"].ToString();


            }

        }

        #region Update Order Request & Assets

        protected void AddUpdateAssets()
        {
            int intOrderId = 0;
            intOrderId = Int32.Parse(hdnOrderId.Value);

            oAssetOrder.UpdateAssetOrderDepotDateReceived(intOrderId,
                                                                             Int32.Parse(ddlDepot.SelectedValue),
                                                                             DateTime.Parse(txtDateReceived.Text.Trim()),
                                                                             intProfile);
                                                                             
            foreach (DataListItem dlItem in dlAssetList.Items)
            {
                HiddenField hdnAssetId = (HiddenField)dlItem.FindControl("hdnAssetId");
                CheckBox chkSelectAsset = (CheckBox)dlItem.FindControl("chkSelectAsset");
                CheckBox chkAssetReturn = (CheckBox)dlItem.FindControl("chkAssetReturn");
                Label lblAssetListSerialNo = (Label)dlItem.FindControl("lblAssetListSerialNo");
                Label lblAssetListAssetTag = (Label)dlItem.FindControl("lblAssetListAssetTag");
                Label lblAssetListAssetScanDateTime = (Label)dlItem.FindControl("lblAssetListAssetScanDateTime");

                TextBox txtAssetListComments = (TextBox)dlItem.FindControl("txtAssetListComments");

                if (chkSelectAsset.Checked == true)
                {
                    if (hdnAssetId.Value != "")//Update Asset 
                    {
                        oAsset.UpdateReturned(Int32.Parse(hdnAssetId.Value),
                                              (chkAssetReturn.Checked == true ? 1 : 0), 
                                              txtAssetListComments.Text);
                    }
                    else //Insert Asset 
                    {
                        int intAsset = oAsset.Add(Int32.Parse(hdnOrderId.Value), Int32.Parse(ddlDepot.SelectedValue),
                                                    Int32.Parse(hdnModel.Value), lblAssetListSerialNo.Text.Trim().ToUpper(), 
                                                    lblAssetListAssetTag.Text.Trim().ToUpper(),
                                                    (int)AssetStatus.Arrived, intProfile, 
                                                    DateTime.Parse(lblAssetListAssetScanDateTime.Text), 0, 1,
                                                    (chkAssetReturn.Checked==true?1:0),txtAssetListComments.Text);
                        hdnAssetId.Value = intAsset.ToString();

                    }
                }
            }
        }

        protected void btnUpload_Click(Object Sender, EventArgs e)
        {
            //Populate already saved assets against this order
            LoadAssetsForOrder();
            boolDetails = false;
            boolExecution = true;
            boolStatusUpdates = false;
            boolChange = false;
            boolDocuments = false;

            int intSuccess = 0;
            int intDuplicate = 0;
            int intError = 0;
            ModelsProperties oModelsProperties = new ModelsProperties(0, dsn);
            if (oFile.PostedFile != null && oFile.FileName != "")
            {
                string strPhysical = oVariable.DocumentsFolder() + "xml\\";
                if (Directory.Exists(strPhysical) == false)
                {
                    lblResults.Text = "<b>Created XML directory &quot;" + strPhysical + "&quot;...SUCCESS!</b><br/><br/>";
                    Directory.CreateDirectory(strPhysical);
                }
                else
                    lblResults.Text = "<b>XML directory &quot;" + strPhysical + "&quot; already exists...</b><br/><br/>";
                lblResults.Text += "<b>Results of File &quot;" + oFile.FileName + "&quot;</b><br/>";
                string strFile = strPhysical + oFile.FileName;
                oFile.PostedFile.SaveAs(strFile);
                DataSet ds = new DataSet();
                ds.ReadXml(strFile);

                DataTable dtAssetImport = ds.Tables[0];
                dtAssetImport.Columns.Add("ValidAsset", System.Type.GetType("System.Int32"));
                dtAssetImport.Columns.Add("Comments", System.Type.GetType("System.String"));
               
                DataRow[] drSelect = null;
                drSelect = dtAssetImport.Select("svrModel = " + Int32.Parse(hdnModel.Value));


                switch (oFile.FileName[0])
                {
                    case 'E':
                        // Existing
                        foreach (DataRow dr in drSelect)
                        {
                            int intModel = 0;
                            try
                            {
                                intModel = Int32.Parse(dr["svrModel"].ToString());
                            }
                            catch
                            {
                                intModel = oModelsProperties.Get(dr["svrModel"].ToString());
                            }
                            if (intModel > 0)
                            {
                                if (oAsset.Get(dr["svrSerial"].ToString().Trim().ToUpper(), intModel).Tables[0].Rows.Count > 0)
                                {
                                    dr["ValidAsset"] = 0;
                                    dr["Comments"] = "DUPLICATE: " + dr["svrSerial"].ToString().Trim().ToUpper() + " [" + dr["svrAsset"].ToString().Trim().ToUpper() + "]";
                                    intDuplicate++;
                                }
                                else
                                {
                                    dr["ValidAsset"] = 1;
                                    intSuccess++;
                                }
                            }
                            else
                            {
                                dr["ValidAsset"] = 0;
                                dr["Comments"] = "### ERROR ###: " + dr["svrModel"].ToString().Trim().ToUpper() + " does not exist";
                                intError++;
                            }
                        }
                        lblResults.Text += "<p><hr size=1 noshade/></p>";
                        lblResults.Text += "Duplicates: " + intDuplicate.ToString() + " " + " Errors: " + intError.ToString() + "<br/>";

                        foreach (DataRow dr in drSelect)
                        {
                            DataRow drAsset = dtAsset.NewRow();

                            drAsset["Serial"] = dr["svrSerial"].ToString();
                            drAsset["AssetTag"] = dr["svrAsset"].ToString();
                            drAsset["ValidAsset"] = dr["ValidAsset"];
                            drAsset["Comments"] = dr["Comments"];
                            drAsset["ScanDateTime"] = DateTime.Parse(dr["timeStamp"].ToString());
                            dtAsset.Rows.Add(drAsset);
                           
                        }

                        dlAssetList.DataSource = dtAsset;
                        dlAssetList.DataBind();

                        break;

                    case 'S':
                        // New
                        foreach (DataRow dr in drSelect)
                        {
                            int intModel = 0;
                            try
                            {
                                intModel = Int32.Parse(dr["svrModel"].ToString());
                            }
                            catch
                            {
                                intModel = oModelsProperties.Get(dr["svrModel"].ToString());
                            }
                            if (intModel > 0)
                            {
                                if (oAsset.Get(dr["svrSerial"].ToString().Trim().ToUpper(), intModel).Tables[0].Rows.Count > 0)
                                {
                                    dr["ValidAsset"] = 0;
                                    dr["Comments"] = "DUPLICATE: " + dr["svrSerial"].ToString().Trim().ToUpper() + " [" + dr["svrAsset"].ToString().Trim().ToUpper() + "]";
                                    intDuplicate++;
                                }
                                else
                                {
                                    dr["ValidAsset"] = 1;
                                    intSuccess++;
                                }
                            }
                            else
                            {
                                dr["ValidAsset"] = 0;
                                dr["Comments"] = "### ERROR ###: " + dr["svrModel"].ToString().Trim().ToUpper() + " does not exist";
                                intError++;
                            }
                        }
                        lblResults.Text += "<p><hr size=1 noshade/></p>";
                        lblResults.Text += "Duplicates: " + intDuplicate.ToString() + " " + " Errors: " + intError.ToString() + "<br/>";



                        foreach (DataRow dr in drSelect)
                        {
                            DataRow drAsset = dtAsset.NewRow();

                            drAsset["Serial"] = dr["svrSerial"].ToString();
                            drAsset["AssetTag"] = dr["svrAsset"].ToString();
                            drAsset["ValidAsset"] = dr["ValidAsset"];
                            drAsset["Comments"] = dr["Comments"];
                            drAsset["ScanDateTime"] = DateTime.Parse(dr["timeStamp"].ToString());
                            dtAsset.Rows.Add(drAsset);
                        }

                        dlAssetList.DataSource = dtAsset;
                        dlAssetList.DataBind();

                        break;

                    default:
                        lblResults.Text = "Invalid XML File";
                        break;
                }
            }
        }

        protected int GetParent(DataSet dsParent, int intParent)
        {
            int intReturn = 0;
            foreach (DataRow drParent in dsParent.Tables[0].Rows)
            {
                if (Int32.Parse(drParent["svrIndex"].ToString()) == intParent)
                {
                    DataSet dsSerial = oAsset.Get(drParent["svrSerial"].ToString());
                    if (dsSerial.Tables[0].Rows.Count > 0)
                    {
                        intReturn = Int32.Parse(dsSerial.Tables[0].Rows[0]["id"].ToString());
                        break;
                    }
                }
            }
            return intReturn;
        }

        protected void ErrorMessage(string _model)
        {
            string strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DEVELOPER_WARNING");
            oFunction.SendEmail("ClearView Action Required", strEMailIdsBCC, "", "", "ClearView Action Required", "<p><b>When importing models from the mobile application, there was a problem importing the following model...</b></p><p>" + _model + "</p>", false, false);
        }

       #endregion

        #region Asset Order

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

        protected void AddOrderReqComments()
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
            Response.Redirect(Request.Path + "?rrid=" + intResourceWorkflow.ToString() + "&div=E&save=true");
        }

        protected void lnkbtnOrderReqDelete_Click(Object Sender, EventArgs e)
        {
            int intResourceWorkflow = Int32.Parse(lblResourceWorkflow.Text);
            LinkButton oButton = (LinkButton)Sender;
            oAssetOrder.DeleteAssetOrderComment(Int32.Parse(oButton.CommandArgument), intProfile);
            Response.Redirect(Request.Path + "?rrid=" + intResourceWorkflow.ToString() + "&div=E&save=true");

        }

        #endregion
    }
}