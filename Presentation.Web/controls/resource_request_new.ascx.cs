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
using Microsoft.ApplicationBlocks.Data;
using System.Text;
using System.DirectoryServices;
using System.Collections.Generic;
namespace NCC.ClearView.Presentation.Web
{
    public partial class resource_request_new : System.Web.UI.UserControl
    {

        private DataSet ds;
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
        private string dsnServiceEditor = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["ServiceEditorDSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intDataPointAvailableService = Int32.Parse(ConfigurationManager.AppSettings["DATAPOINT_AVAILABLE_SERVICE"]);
        protected int intAssignPage = Int32.Parse(ConfigurationManager.AppSettings["AssignWorkload"]);
        protected int intResourceRequestApprove = Int32.Parse(ConfigurationManager.AppSettings["ResourceRequestApprove"]);
        protected int intServiceDecommission = Int32.Parse(ConfigurationManager.AppSettings["SERVICEID_SERVER_DECOMMISSION"]);
        protected int intServiceRebuildWorkstation = Int32.Parse(ConfigurationManager.AppSettings["SERVICEID_WORKSTATION_REBUILD"]);
        protected int intViewPage = Int32.Parse(ConfigurationManager.AppSettings["ViewWorkload"]);
        protected int intProfile;
        protected int intRequestor;
        protected Applications oApplication;
        protected Pages oPage;
        protected Projects oProject;
        protected ProjectsPending oProjectPending;
        protected ResourceRequest oResourceRequest;
        protected Organizations oOrganization;
        protected Users oUser;
        protected RequestItems oRequestItem;
        protected Requests oRequest;
        protected ServiceRequests oServiceRequest;
        protected RequestFields oRequestField;
        protected Services oService;
        protected ServiceDetails oServiceDetail;
        protected Variables oVariable;
        protected StatusLevels oStatusLevel;
        protected Field oField;
        protected Documents oDocument;
        protected Segment oSegment;
        protected Functions oFunction;
        protected DataPoint oDataPoint;
        protected ServiceEditor oServiceEditor;
        protected Log oLog;
        protected bool boolApproved = false;
        protected int intApplication = 0;
        protected int intPage = 0;
        protected string strServices = "";
        protected string strFavorites = "";
        protected string strSummary = "";
        protected string strCrumbs = "";
        protected int intNumberCount;
        protected bool boolWM = false;
        private string strEMailIdsBCC = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oApplication = new Applications(intProfile, dsn);
            oPage = new Pages(intProfile, dsn);
            oProject = new Projects(intProfile, dsn);
            oProjectPending = new ProjectsPending(intProfile, dsn, intEnvironment);
            oResourceRequest = new ResourceRequest(intProfile, dsn);
            oOrganization = new Organizations(intProfile, dsn);
            oUser = new Users(intProfile, dsn);
            oRequestItem = new RequestItems(intProfile, dsn);
            oRequest = new Requests(intProfile, dsn);
            oServiceRequest = new ServiceRequests(intProfile, dsn);
            oRequestField = new RequestFields(intProfile, dsn);
            oService = new Services(intProfile, dsn);
            oServiceDetail = new ServiceDetails(intProfile, dsn);
            oVariable = new Variables(intEnvironment);
            oStatusLevel = new StatusLevels();
            oField = new Field(intProfile, dsn);
            oDocument = new Documents(intProfile, dsn);
            oSegment = new Segment(intProfile, dsn);
            oFunction = new Functions(intProfile, dsn, intEnvironment);
            oDataPoint = new DataPoint(intProfile, dsn);
            oServiceEditor = new ServiceEditor(intProfile, dsnServiceEditor);
            oLog = new Log(intProfile, dsn);
            if (Request.QueryString["applicationid"] != null && Request.QueryString["applicationid"] != "")
                intApplication = Int32.Parse(Request.QueryString["applicationid"]);
            if (Request.QueryString["pageid"] != null && Request.QueryString["pageid"] != "")
                intPage = Int32.Parse(Request.QueryString["pageid"]);
            if (Request.Cookies["application"] != null && Request.Cookies["application"].Value != "")
                intApplication = Int32.Parse(Request.Cookies["application"].Value);
            lblTitle.Text = oPage.Get(intPage, "title");
            if (Request.QueryString["rid"] != null && Request.QueryString["rid"] != "")
            {
                int intRequest = Int32.Parse(Request.QueryString["rid"]);
                lblRequest.Text = intRequest.ToString();
                if (intRequest == 0)
                {
                    // New Request
                    int intService = 0;
                    if (Request.QueryString["sid"] != null && Request.QueryString["sid"] != "")
                        intService = Int32.Parse(Request.QueryString["sid"]);
                    btnCart.Enabled = false;
                    btnCancel1.Enabled = false;
                    LoadServices(intRequest, intService);
                }
                else if (Request.QueryString["view"] != null)
                {
                    lblTitle.Text = "Request Details";
                    int intFormView = 0;
                    if (Request.QueryString["formid"] != null && Request.QueryString["formid"] != "")
                        intFormView = Int32.Parse(Request.QueryString["formid"]);
                    if (intFormView > 0)
                    {
                        DataSet dsForm = oRequestItem.GetForm(intRequest, intFormView);
                        int intItem = 0;
                        int intNumber = 0;
                        int intService = 0;
                        int intApp = 0;
                        int intProject = 0;
                        if (dsForm.Tables[0].Rows.Count > 0)
                        {
                            intProject = oRequest.GetProjectNumber(intRequest);
                            intItem = Int32.Parse(dsForm.Tables[0].Rows[0]["itemid"].ToString());
                            intApp = oRequestItem.GetItemApplication(intItem);
                            intNumber = Int32.Parse(dsForm.Tables[0].Rows[0]["number"].ToString());
                            intService = Int32.Parse(dsForm.Tables[0].Rows[0]["serviceid"].ToString());
                        }
                        if (Request.QueryString["num"] != null && Request.QueryString["num"] != "")
                            intNumber = Int32.Parse(Request.QueryString["num"]);
                        lblApplication.Text = intApp.ToString();
                        // HEALYFIX - no resourcerequestid
                        lblView.Text = oRequestField.GetBody(intRequest, intItem, intNumber, intService, 0, 0, dsnServiceEditor, intEnvironment, dsnAsset, dsnIP);
                        panView.Visible = true;
                        LoadProject(intProject, intRequest);
                    }
                }
                else
                {
                    if (Request.QueryString["did"] != null && Request.QueryString["did"] != "" && Request.QueryString["didn"] != null && Request.QueryString["didn"] != "") 
                    {
                        oService.DeleteSelected(intRequest, Int32.Parse(Request.QueryString["did"]), Int32.Parse(Request.QueryString["didn"]));
                        oRequestItem.DeleteForms(intRequest, Int32.Parse(Request.QueryString["did"]), Int32.Parse(Request.QueryString["didn"]));
                    }
                    intRequestor = oRequest.GetUser(intRequest);
                    LoadRequest(intRequest);
                }
            }
            else if (Request.QueryString["rrid"] != null && Request.QueryString["rrid"] != "")
            {
                lblTitle.Text = "Request Details";
                int intId = Int32.Parse(Request.QueryString["rrid"]);
                ds = oResourceRequest.Get(intId);
                int intRequest = Int32.Parse(ds.Tables[0].Rows[0]["requestid"].ToString());
                int intProject = oRequest.GetProjectNumber(intRequest);
                int intItem = Int32.Parse(ds.Tables[0].Rows[0]["itemid"].ToString());
                int intService = Int32.Parse(ds.Tables[0].Rows[0]["serviceid"].ToString());
                int intApp = oRequestItem.GetItemApplication(intItem);
                lblApplication.Text = intApp.ToString();
                lblView.Text = oRequestField.GetBodyOverall(intId, 0, dsnServiceEditor, intEnvironment, dsnAsset, dsnIP);
                panView.Visible = true;
                LoadProject(intProject, intRequest);
            }
            else
            {
                if (Request.QueryString["delete"] == null)
                    Response.Redirect(oPage.GetFullLink(intPage) + "?rid=0");
                else
                    Response.Redirect(oPage.GetFullLink(intPage) + "?rid=0&delete=true");
            }

            if (String.IsNullOrEmpty(Request.QueryString["approve"]) == false)
                lblTitle.Text = "Original Request";

            ddlRejectOrganization.Attributes.Add("onchange", "PopulateSegments('" + ddlRejectOrganization.ClientID + "','" + ddlRejectSegment.ClientID + "');");
            ddlRejectSegment.Attributes.Add("onchange", "UpdateDropDownHidden('" + ddlRejectSegment.ClientID + "','" + hdnSegment.ClientID + "');");
            ddlOrganization.Attributes.Add("onchange", "PopulateSegments('" + ddlOrganization.ClientID + "','" + ddlSegment.ClientID + "');");
            ddlSegment.Attributes.Add("onchange", "UpdateDropDownHidden('" + ddlSegment.ClientID + "','" + hdnSegment.ClientID + "');");
            radTaskYes.Attributes.Add("onclick", "ShowHideDiv('" + divTaskYes.ClientID + "','inline');ShowHideDiv('" + divTaskNo.ClientID + "','none');");
            radTaskNo.Attributes.Add("onclick", "ShowHideDiv('" + divTaskNo.ClientID + "','inline');ShowHideDiv('" + divTaskYes.ClientID + "','none');");
            btnDenied.Attributes.Add("onclick", "return CloseWindow();");
            btnCancel1.Attributes.Add("onclick", "return confirm('Are you sure you want to cancel this request?') && ProcessButton(this) && LoadWait();");
            btnCancel2.Attributes.Add("onclick", "return confirm('Are you sure you want to cancel this request?') && ProcessButton(this) && LoadWait();");
            btnCancel3.Attributes.Add("onclick", "return confirm('Are you sure you want to cancel this request?') && ProcessButton(this) && LoadWait();");
            btnCancel4.Attributes.Add("onclick", "return confirm('Are you sure you want to cancel this request?') && ProcessButton(this) && LoadWait();");
            btnAdd.Attributes.Add("onclick", "return ValidateStringItems('" + hdnService.ClientID + "','Please select at least one service to add to this request') && ProcessButton(this) && LoadWait();");
            btnFavorite.Attributes.Add("onclick", "return ValidateStringItems('" + hdnService.ClientID + "','Please select at least one service to add to this request') && ProcessButton(this) && LoadWait();");
            btnPendingProject.Attributes.Add("onclick", "return ValidateNewProject('" + txtNumber.ClientID + "','" + txtName.ClientID + "','" + ddlBaseDisc.ClientID + "','" + ddlOrganization.ClientID + "') && ProcessButton(this) && LoadWait();");
            btnRejectProject.Attributes.Add("onclick", "return ValidateText('" + txtRejectName.ClientID + "','Please enter a project name') && ValidateDropDown('" + ddlRejectType.ClientID + "','Please choose a project type') && ValidateDropDown('" + ddlRejectOrganization.ClientID + "','Please select a sponsoring portfolio') && ProcessButton(this) && LoadWait();");
            btnPendingTask.Attributes.Add("onclick", "return ValidateText('" + txtTaskName.ClientID + "','Please enter a task name') && ProcessButton(this) && LoadWait();");
            btnRejectTask.Attributes.Add("onclick", "return ValidateText('" + txtRejectTaskName.ClientID + "','Please enter a task name') && ProcessButton(this) && LoadWait();");
            btnServices.Attributes.Add("onclick", "return ProcessButton(this) && LoadWait();");
            btnUpdate.Attributes.Add("onclick", "return ProcessButton(this) && LoadWait();");
            btnCheckout.Attributes.Add("onclick", "return ProcessButton(this) && LoadWait();");
            btnSelect.Attributes.Add("onclick", "return ProcessButton(this) && LoadWait();");
            btnRefresh.Attributes.Add("onclick", "return ProcessButton(this) && LoadWait();");
            btnComplete.Attributes.Add("onclick", "return ValidateText('" + txtTitle.ClientID + "','Please enter a title for this request') && ProcessButton(this) && LoadWait();");
            btnResult.Attributes.Add("onclick", "return ProcessButton(this) && LoadWait();");
            btnTitle.Attributes.Add("onclick", "return ValidateText('" + txtTitle.ClientID + "','Please enter a title for this request') && ProcessButton(this) && LoadWait();");
        }
        private void LoadRequest(int intRequest)
        {
            DataSet dsServiceRequest = oServiceRequest.Get(intRequest);
            ds = oRequest.Get(intRequest);
            if (dsServiceRequest.Tables[0].Rows.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                int intProject = Int32.Parse(ds.Tables[0].Rows[0]["projectid"].ToString());
                int intCheckout = Int32.Parse(dsServiceRequest.Tables[0].Rows[0]["checkout"].ToString());
                int intItem = 0;
                int intNumber = 0;
                int intService = 0;
                int intFormID = 0;
                DataSet dsForm;
                LoadProject(intProject, intRequest);
                if (intCheckout > 0 || intProfile == intRequestor)
                {
                    switch (intCheckout)
                    {
                        case -2:
                            // Service Browser
                            btnCart.Enabled = (oService.GetSelected(intRequest).Tables[0].Rows.Count > 0);
                            if (Request.QueryString["sid"] != null && Request.QueryString["sid"] != "")
                                intService = Int32.Parse(Request.QueryString["sid"]);
                            else if (Request.QueryString["add"] != null && Request.Cookies["service_parent"] != null && Request.Cookies["service_parent"].Value != "")
                                Response.Redirect(oPage.GetFullLink(intPage) + "?rid=" + intRequest.ToString() + "&sid=" + Request.Cookies["service_parent"].Value);
                            LoadServices(intRequest, intService);
                            break;
                        case -1:
                            // Cart Summary
                            panCheckout.Visible = true;
                            LoadCart(intRequest);
                            break;
                        case 0:
                            // Checked out - Forms or Finish Summary
                            dsForm = oRequestItem.GetForms(intRequest);
                            int intCount = 0;
                            foreach (DataRow drForm in dsForm.Tables[0].Rows)
                            {
                                intCount++;
                                if (drForm["done"].ToString() == "-1")
                                {
                                    intItem = Int32.Parse(drForm["itemid"].ToString());
                                    intNumber = Int32.Parse(drForm["number"].ToString());
                                    intService = Int32.Parse(drForm["serviceid"].ToString());
                                    break;
                                }
                            }
                          
                            if (Request.QueryString["formid"] != null && Request.QueryString["formid"] != "")
                                intFormID = Int32.Parse(Request.QueryString["formid"]);
                            if (intItem == 0 && intFormID == 0)
                            {
                                // Done with Forms - show finish summary
                                panSummary.Visible = true;
                                LoadFinishSummary(intRequest);
                            }
                            else
                            {
                                if (intFormID > 0)
                                {
                                    dsForm = oRequestItem.GetForm(intRequest, intFormID);
                                    if (dsForm.Tables[0].Rows.Count > 0)
                                    {
                                        intItem = Int32.Parse(dsForm.Tables[0].Rows[0]["itemid"].ToString());
                                        intNumber = Int32.Parse(dsForm.Tables[0].Rows[0]["number"].ToString());
                                        intService = Int32.Parse(dsForm.Tables[0].Rows[0]["serviceid"].ToString());
                                    }
                                    if (Request.QueryString["num"] != null && Request.QueryString["num"] != "")
                                        intNumber = Int32.Parse(Request.QueryString["num"]);
                                }
                                if (oService.Get(intService, "project") == "1" && intProject == -1)
                                {
                                    // Must have project - allow adding
                                    panPending.Visible = true;
                                    if (Request.QueryString["new"] != null)
                                    {
                                        panPendingNew.Visible = true;
                                        DataSet dsOrgs = oOrganization.Gets(1);
                                        ddlOrganization.DataValueField = "organizationid";
                                        ddlOrganization.DataTextField = "name";
                                        ddlOrganization.DataSource = dsOrgs;
                                        ddlOrganization.DataBind();
                                        ddlOrganization.Items.Insert(0, new ListItem("-- SELECT --", "0"));
                                    }
                                    else
                                    {
                                        if (Request.QueryString["select"] != null)
                                            Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "validation", "<script type=\"text/javascript\">alert('Please select a project or choose \"-- PROJECT NOT LISTED --\" to create a new project');<" + "/" + "script>");
                                        string strOrder = "namenumber";
                                        if (Request.QueryString["order"] != null)
                                            strOrder = Request.QueryString["order"];
                                        DataSet dsProjs = oProject.GetActive();
                                        DataView dv = dsProjs.Tables[0].DefaultView;
                                        if (strOrder == "namenumber")
                                            btnOrderName.Enabled = false;
                                        else if (strOrder == "numbername")
                                            btnOrderNumber.Enabled = false;
                                        else
                                            strOrder = "namenumber";
                                        dv.Sort = strOrder;
                                        panPendingChoose.Visible = true;
                                        lstProjects.DataTextField = strOrder;
                                        lstProjects.DataValueField = "projectid";
                                        lstProjects.DataSource = dv;
                                        lstProjects.DataBind();
                                        lstProjects.Items.Insert(0, new ListItem("-- PROJECT NOT LISTED --", "0"));
                                        lstProjects.Items[0].Attributes.Add("style", "color:#DD0000");
                                        lstProjects.Attributes.Add("ondblclick", "ShowProjectInfo(this);");
                                        txtSearchName.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + btnSearch.ClientID + "').click();return false;}} else {return true}; ");
                                        txtSearchNumber.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + btnSearch.ClientID + "').click();return false;}} else {return true}; ");
                                        btnSearch.Attributes.Add("onclick", "SearchTextList('" + txtSearchNumber.ClientID + "','" + txtSearchName.ClientID + "','" + lstProjects.ClientID + "') && LoadWait();return false;");
                                    }
                                }
                                else
                                {
                                    panHeader.Visible = true;
                                    lblService.Text = intService.ToString();
                                    panFavorite.Visible = true;
                                    // Favorites
                                    if (oService.GetFavorite(intProfile, intService).Tables[0].Rows.Count > 0)
                                    {
                                        lblFavorite.Text = "This service is one of your favorites";
                                        btnFavoriteDelete.Visible = true;
                                        btnFavoriteDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to remove this service from your Favorites?') && ProcessButton(this) && LoadWait();");
                                    }
                                    else
                                    {
                                        lblFavorite.Text = "This service is <b>not</b> one of your favorites";
                                        btnFavoriteAdd.Visible = true;
                                        btnFavoriteAdd.Attributes.Add("onclick", "return ProcessButton(this) && LoadWait();");
                                    }
                                    // Show Form
                                    string strPath = oService.Get(intService, "rr_path");
                                    if (strPath.Trim() == "")
                                    {
                                        // For now, bypass
                                        oRequestItem.UpdateFormDone(intRequest, intItem, intNumber, 1);
                                        Response.Redirect(oPage.GetFullLink(intPage) + "?rid=" + intRequest.ToString());
                                    }
                                    else
                                    {
                                        // LOAD CONTROL
                                        string strWorkflowTitle = oService.Get(intService, "workflow_title");
                                        lblHeader.Text = (strWorkflowTitle == "" ? oService.GetName(intService) : strWorkflowTitle);
                                        //lblHeader1.Text = intCount.ToString();
                                        //int intTotal = dsForm.Tables[0].Rows.Count + 1;
                                        //lblHeader2.Text = intTotal.ToString();
                                        panControl.Visible = true;
                                        Control oControl = (Control)LoadControl(strPath);
                                        PHForm.Controls.Add(oControl);
                                    }
                                }
                            }
                            break;
                        case 1:
                            bool boolValidEdit = false;
                            if (Request.QueryString["submitted"] != null)
                            {
                                int intID = 0;
                                Int32.TryParse(Request.QueryString["submitted"], out intID);
                                if (Request.QueryString["formid"] != null && Request.QueryString["formid"] != "")
                                    intFormID = Int32.Parse(Request.QueryString["formid"]);
                                if (intFormID > 0)
                                {
                                    dsForm = oRequestItem.GetForm(intRequest, intFormID);
                                    foreach (DataRow drForm in dsForm.Tables[0].Rows)
                                    {
                                        if (Int32.Parse(drForm["formid"].ToString()) == intID)
                                        {
                                            boolValidEdit = true;
                                            intItem = Int32.Parse(drForm["itemid"].ToString());
                                            intNumber = Int32.Parse(drForm["number"].ToString());
                                            intService = Int32.Parse(drForm["serviceid"].ToString());
                                            break;
                                        }
                                    }
                                }
                            }
                            //for returned request
                            if (Request.QueryString["returned"] == "true" || Request.QueryString["denied"] == "true")
                            {
                                if (Request.QueryString["formid"] != null && Request.QueryString["formid"] != "")
                                    intFormID = Int32.Parse(Request.QueryString["formid"]);
                                if (intFormID > 0)
                                {
                                    dsForm = oRequestItem.GetForm(intRequest, intFormID);
                                    if (dsForm.Tables[0].Rows.Count > 0)
                                    {
                                        intItem = Int32.Parse(dsForm.Tables[0].Rows[0]["itemid"].ToString());
                                        intNumber = Int32.Parse(dsForm.Tables[0].Rows[0]["number"].ToString());
                                        intService = Int32.Parse(dsForm.Tables[0].Rows[0]["serviceid"].ToString());
                                    }
                                    if (Request.QueryString["num"] != null && Request.QueryString["num"] != "")
                                    {
                                        intNumber = Int32.Parse(Request.QueryString["num"]);
                                        intService = intFormID;
                                    }
                                }
                            }
                            if (boolValidEdit == true || Request.QueryString["returned"] == "true" || Request.QueryString["denied"] == "true")
                            {
                                panHeader.Visible = true;
                                lblService.Text = intService.ToString();
                                panFavorite.Visible = true;
                                // Favorites
                                if (oService.GetFavorite(intProfile, intService).Tables[0].Rows.Count > 0)
                                {
                                    lblFavorite.Text = "This service is one of your favorites";
                                    btnFavoriteDelete.Visible = true;
                                    btnFavoriteDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to remove this service from your Favorites?') && ProcessButton(this) && LoadWait();");
                                }
                                else
                                {
                                    lblFavorite.Text = "This service is <b>not</b> one of your favorites";
                                    btnFavoriteAdd.Visible = true;
                                    btnFavoriteAdd.Attributes.Add("onclick", "return ProcessButton(this) && LoadWait();");
                                }
                                // Show Form
                                string strPath = oService.Get(intService, "rr_path");
                                if (strPath.Trim() == "")
                                {
                                    // For now, bypass
                                    oRequestItem.UpdateFormDone(intRequest, intItem, intNumber, 1);
                                    Response.Redirect(oPage.GetFullLink(intPage) + "?rid=" + intRequest.ToString()+"&returned=true");
                                }
                                else
                                {
                                    // LOAD CONTROL
                                    string strWorkflowTitle = oService.Get(intService, "workflow_title");
                                    lblHeader.Text = (strWorkflowTitle == "" ? oService.GetName(intService) : strWorkflowTitle);
                                    //lblHeader1.Text = intCount.ToString();
                                    //int intTotal = dsForm.Tables[0].Rows.Count + 1;
                                    //lblHeader2.Text = intTotal.ToString();
                                    panControl.Visible = true;
                                    Control oControl = (Control)LoadControl(strPath);
                                    PHForm.Controls.Add(oControl);
                                }

                            }
                            else
                            {
                                // Completed Summary
                                intItem = 0;
                                if (intProject > -2)
                                {
                                    dsForm = oRequestItem.GetForms(intRequest);
                                    foreach (DataRow drForm in dsForm.Tables[0].Rows)
                                    {
                                        if (drForm["done"].ToString() == "0")
                                        {
                                            intItem = Int32.Parse(drForm["itemid"].ToString());
                                            intNumber = Int32.Parse(drForm["number"].ToString());
                                            intService = Int32.Parse(drForm["serviceid"].ToString());
                                            // SUBMIT INFORMATION
                                            string strPath = oService.Get(intService, "cp_path");
                                            if (strPath.Trim() == "")
                                            {
                                                // For now, bypass
                                                oRequestItem.UpdateFormDone(intRequest, intItem, intNumber, 1);
                                            }
                                            else
                                            {
                                                Control oControl = (Control)LoadControl(strPath);
                                                PHcp.Controls.Add(oControl);
                                            }
                                        }
                                    }
                                    if (intItem > 0)
                                    {
                                        panHeader.Visible = true;
                                        lblHeader.Text = "Service Request Results";
                                        lblHeaderSub.Text = "Thank you for submitting your service request. Below you can find the results of each service. <b>NOTE:</b> Click &quot;Refresh Request&quot; to view the status of this request..";
                                        panResult.Visible = true;
                                        btnResult.Text = "Next  >>";
                                        btnResult.Width = Unit.Pixel(100);
                                        btnPrinter2.Attributes.Add("onclick", "return OpenWindow('NEW_WINDOW','/frame/printer.aspx?rid=" + intRequest.ToString() + "');");
                                    }
                                    else
                                    {
                                        if (Request.QueryString["cid"] != null && Request.QueryString["cid"] != "")
                                        {
                                            intService = Int32.Parse(Request.QueryString["cid"]);
                                            // Remove Resource Request
                                            intItem = oService.GetItemId(intService);
                                            dsForm = oRequestItem.GetForms(intRequest, intService);
                                            int intCancelNum = 0;
                                            if (Request.QueryString["num"] != null && Request.QueryString["num"] != "")
                                                intCancelNum = Int32.Parse(Request.QueryString["num"]);
                                            if (dsForm.Tables[0].Rows.Count > 0)
                                            {
                                                // Not Workflow
                                                foreach (DataRow drForm in dsForm.Tables[0].Rows)
                                                {
                                                    int intCancelNum2 = Int32.Parse(drForm["number"].ToString());
                                                    if (intCancelNum > 0)
                                                        intCancelNum2 = intCancelNum;
                                                    DataSet dsResource = oResourceRequest.GetResourceRequest(intRequest, intItem, intCancelNum2);
                                                    if (dsResource.Tables[0].Rows.Count > 0)
                                                    {
                                                        int intResourceParent = Int32.Parse(dsResource.Tables[0].Rows[0]["id"].ToString());
                                                        oResourceRequest.UpdateStatusOverall(intResourceParent, -2);
                                                        DataSet dsManager = oService.GetUser(intService, 1);
                                                        DataSet dsResources = oResourceRequest.GetWorkflowsParent(intResourceParent);
                                                        foreach (DataRow drResources in dsResources.Tables[0].Rows)
                                                        {
                                                            int intResourceWorkflow = Int32.Parse(drResources["id"].ToString());
                                                            int intUser = Int32.Parse(drResources["userid"].ToString());
                                                            if (intUser == 0)
                                                            {
                                                                foreach (DataRow drManager in dsManager.Tables[0].Rows)
                                                                {
                                                                    int intManager = Int32.Parse(drManager["userid"].ToString());
                                                                    strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_REQUEST_ASSIGNMENT");
                                                                    oFunction.SendEmail("ClearView Service Request CANCELLED", oUser.GetName(intManager), "", strEMailIdsBCC, "ClearView Service Request CANCELLED", "<p><b>The following service request has been cancelled by " + oUser.GetFullName(intProfile) + "</b><p><p>" + oResourceRequest.GetWorkflowSummary(Int32.Parse(drResources["id"].ToString()), intEnvironment, dsnServiceEditor, dsnAsset, dsnIP) + "</p>", true, false);
                                                                }
                                                            }
                                                            else
                                                            {
                                                                foreach (DataRow drManager in dsManager.Tables[0].Rows)
                                                                {
                                                                    int intManager = Int32.Parse(drManager["userid"].ToString());
                                                                    strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_REQUEST_ASSIGNMENT");
                                                                    oFunction.SendEmail("ClearView Service Request CANCELLED", oUser.GetName(intUser), oUser.GetName(intManager), strEMailIdsBCC, "ClearView Service Request CANCELLED", "<p><b>The following service request has been cancelled by " + oUser.GetFullName(intProfile) + "</b><p><p>" + oResourceRequest.GetWorkflowSummary(Int32.Parse(drResources["id"].ToString()), intEnvironment, dsnServiceEditor, dsnAsset, dsnIP) + "</p>", true, false);
                                                                }
                                                            }
                                                            oResourceRequest.UpdateWorkflowStatus(intResourceWorkflow, -2, true);
                                                        }
                                                    }
                                                    oService.CancelSelected(intRequest, intService, intCancelNum2);
                                                    // Add additional cancel functionality (if exists)
                                                    string strPath = oService.Get(intService, "ca_path");
                                                    if (strPath.Trim() != "")
                                                    {
                                                        panCancel.Visible = true;
                                                        Control oControl = (Control)LoadControl(strPath);
                                                        phCancel.Controls.Add(oControl);
                                                    }
                                                    else
                                                        Response.Redirect(oPage.GetFullLink(intPage) + "?rid=" + intRequest.ToString());
                                                    if (intCancelNum > 0)
                                                        break;
                                                }
                                            }
                                            else
                                            {
                                                // Workflow
                                                int intResourceWorkflow = 0;
                                                // First, check to see if cancelling ALL or just this ONE
                                                bool boolCancelAll = (String.IsNullOrEmpty(Request.QueryString["wfc"]) == false);
                                                DataSet dsResource = oResourceRequest.GetAllService(intRequest, intService, intCancelNum);
                                                foreach (DataRow drResource in dsResource.Tables[0].Rows)
                                                {
                                                    int intResourceParent = Int32.Parse(drResource["RRID"].ToString());
                                                    oResourceRequest.UpdateStatusOverall(intResourceParent, -2);
                                                    DataSet dsManager = oService.GetUser(intService, 1);
                                                    DataSet dsResources = oResourceRequest.GetWorkflowsParent(intResourceParent);
                                                    foreach (DataRow drResources in dsResources.Tables[0].Rows)
                                                    {
                                                        intResourceWorkflow = Int32.Parse(drResources["id"].ToString());
                                                        int intUser = Int32.Parse(drResources["userid"].ToString());
                                                        if (intUser == 0)
                                                        {
                                                            foreach (DataRow drManager in dsManager.Tables[0].Rows)
                                                            {
                                                                int intManager = Int32.Parse(drManager["userid"].ToString());
                                                                strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_REQUEST_ASSIGNMENT");
                                                                oFunction.SendEmail("ClearView Workflow Task CANCELLED", oUser.GetName(intManager), "", strEMailIdsBCC, "ClearView Workflow Task CANCELLED", "<p><b>The following workflow task has been cancelled by " + oUser.GetFullName(intProfile) + "</b><p><p>" + oResourceRequest.GetWorkflowSummary(intResourceWorkflow, intEnvironment, dsnServiceEditor, dsnAsset, dsnIP) + "</p>", true, false);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            foreach (DataRow drManager in dsManager.Tables[0].Rows)
                                                            {
                                                                int intManager = Int32.Parse(drManager["userid"].ToString());
                                                                strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_REQUEST_ASSIGNMENT");
                                                                oFunction.SendEmail("ClearView Workflow Task CANCELLED", oUser.GetName(intUser), oUser.GetName(intManager), strEMailIdsBCC, "ClearView Workflow Task CANCELLED", "<p><b>The following workflow task has been cancelled by " + oUser.GetFullName(intProfile) + "</b><p><p>" + oResourceRequest.GetWorkflowSummary(intResourceWorkflow, intEnvironment, dsnServiceEditor, dsnAsset, dsnIP) + "</p>", true, false);
                                                            }
                                                        }
                                                        oResourceRequest.UpdateWorkflowStatus(intResourceWorkflow, -2, true);

                                                        if (boolCancelAll == false)
                                                        {
                                                            // Generate next workflow.
                                                            string strCVT = "CVT" + intRequest.ToString() + "-" + intService.ToString() + "-" + intCancelNum.ToString();
                                                            oLog.AddEvent(intRequest.ToString(), strCVT, "Client Cancelled ALL", LoggingType.Debug);
                                                            oResourceRequest.CloseWorkflow(intResourceWorkflow, intEnvironment, 0, dsnServiceEditor, true, intResourceRequestApprove, intAssignPage, intViewPage, dsnAsset, dsnIP);
                                                        }
                                                    }
                                                    if (intResourceWorkflow == 0 && boolCancelAll == false)
                                                    {
                                                        // Request has not been assigned, but we want to initiate workflows.
                                                        string strCVT = "CVT" + intRequest.ToString() + "-" + intService.ToString() + "-" + intCancelNum.ToString();
                                                        oLog.AddEvent(intRequest.ToString(), strCVT, "Client Cancelled", LoggingType.Debug);
                                                        oResourceRequest.CloseWorkflowParent(intResourceParent, intEnvironment, 0, dsnServiceEditor, true, intResourceRequestApprove, intAssignPage, intViewPage, dsnAsset, dsnIP);
                                                    }
                                                    // Add additional cancel functionality (if exists)
                                                    string strPath = oService.Get(intService, "ca_path");
                                                    if (strPath.Trim() != "")
                                                    {
                                                        panCancel.Visible = true;
                                                        Control oControl = (Control)LoadControl(strPath);
                                                        phCancel.Controls.Add(oControl);
                                                    }
                                                    else
                                                        Response.Redirect(oPage.GetFullLink(intPage) + "?rid=" + intRequest.ToString());

                                                }
                                            }
                                        }
                                        panSummary.Visible = true;
                                        LoadCompletedSummary(intRequest);
                                    }
                                }
                                else
                                    panPendingSubmission.Visible = true;
                                
                            }
                            break;
                    }
                }
                else
                    panDenied.Visible = true;
            }
            else
                panDenied.Visible = true;
        }
        private void LoadProject(int intProject, int intRequest)
        {
            lblProject.Text = intProject.ToString();
            if (intProject == 0)
            {
                ds = oProjectPending.GetRequest(intRequest);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    lblTaskName.Text = ds.Tables[0].Rows[0]["name"].ToString();
                    lblTaskDescription.Text = ds.Tables[0].Rows[0]["description"].ToString();
                    if (lblTaskDescription.Text == "")
                        lblTaskDescription.Text = "None";
                    lblTaskSubmitted.Text = oUser.GetFullName(Int32.Parse(ds.Tables[0].Rows[0]["userid"].ToString()));
                    panTaskAccepted.Visible = true;
                }
            }
            else
            {
                ds = oProject.Get(intProject);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    lblProjectName.Text = ds.Tables[0].Rows[0]["name"].ToString();
                    lblBaseDiscretionary.Text = ds.Tables[0].Rows[0]["bd"].ToString();
                    lblOrganization.Text = oOrganization.GetName(Int32.Parse(ds.Tables[0].Rows[0]["organization"].ToString()));
                    lblSegment.Text = oSegment.GetName(Int32.Parse(ds.Tables[0].Rows[0]["segmentid"].ToString()));
                    lblProjectNumber.Text = ds.Tables[0].Rows[0]["number"].ToString();
                    if (lblProjectNumber.Text == "")
                        lblProjectNumber.Text = "<i>To Be Determined</i>";
                    boolApproved = oProject.IsApproved(intProject);
                    lblSubmitted.Text = oUser.GetFullName(oRequest.GetUser(intRequest));
                    lblSubmittedOn.Text = DateTime.Parse(oRequest.Get(intRequest, "modified")).ToShortDateString();
                    lblStatus.Text = oStatusLevel.HTML(Int32.Parse(ds.Tables[0].Rows[0]["status"].ToString()));
                    panProjectAccepted.Visible = true;
                }
                else
                {
                    ds = oProjectPending.GetRequest(intRequest);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        lblPending.Text = ds.Tables[0].Rows[0]["id"].ToString();
                        if (ds.Tables[0].Rows[0]["task"].ToString() == "0")
                        {
                            if (ds.Tables[0].Rows[0]["rejected"].ToString() == "")
                            {
                                lblProjectName.Text = ds.Tables[0].Rows[0]["name"].ToString();
                                lblBaseDiscretionary.Text = ds.Tables[0].Rows[0]["bd"].ToString();
                                lblOrganization.Text = oOrganization.GetName(Int32.Parse(ds.Tables[0].Rows[0]["organization"].ToString()));
                                lblSegment.Text = oSegment.GetName(Int32.Parse(ds.Tables[0].Rows[0]["segmentid"].ToString()));
                                lblProjectNumber.Text = ds.Tables[0].Rows[0]["number"].ToString();
                                if (lblProjectNumber.Text == "")
                                    lblProjectNumber.Text = "<i>To Be Determined</i>";
                                boolApproved = false;
                                lblSubmitted.Text = oUser.GetFullName(oRequest.GetUser(intRequest));
                                lblSubmittedOn.Text = DateTime.Parse(oRequest.Get(intRequest, "modified")).ToShortDateString();
                                lblStatus.Text = "PENDING";
                                lblStatus.CssClass = "pending";
                                panProjectAccepted.Visible = true;
                            }
                            else
                            {
                                lblRejectReason.Text = ds.Tables[0].Rows[0]["reason"].ToString();
                                if (lblRejectReason.Text == "")
                                    lblRejectReason.Text = "No reason provided...";
                                txtRejectName.Text = ds.Tables[0].Rows[0]["name"].ToString();
                                ddlRejectType.SelectedValue = ds.Tables[0].Rows[0]["bd"].ToString();
                                DataSet dsOrgs = oOrganization.Gets(1);
                                ddlRejectOrganization.DataValueField = "organizationid";
                                ddlRejectOrganization.DataTextField = "name";
                                ddlRejectOrganization.DataSource = dsOrgs;
                                ddlRejectOrganization.DataBind();
                                ddlRejectOrganization.Items.Insert(0, new ListItem("-- SELECT --", "0"));
                                int intPortfolio = Int32.Parse(ds.Tables[0].Rows[0]["organization"].ToString());
                                ddlRejectOrganization.SelectedValue = intPortfolio.ToString();
                                if (intPortfolio > 0)
                                {
                                    int intSegment = Int32.Parse(ds.Tables[0].Rows[0]["segmentid"].ToString());
                                    hdnSegment.Value = intSegment.ToString();
                                    ddlRejectSegment.Enabled = true;
                                    ddlRejectSegment.DataTextField = "name";
                                    ddlRejectSegment.DataValueField = "id";
                                    ddlRejectSegment.DataSource = oSegment.Gets(intPortfolio, 1);
                                    ddlRejectSegment.DataBind();
                                    ddlRejectSegment.Items.Insert(0, new ListItem("-- SELECT --", "0"));
                                    ddlRejectSegment.SelectedValue = intSegment.ToString();
                                }
                                txtRejectNumber.Text = ds.Tables[0].Rows[0]["number"].ToString();
                                boolApproved = false;
                                lblRejectSubmitted.Text = oUser.GetFullName(oRequest.GetUser(intRequest));
                                lblRejectSubmittedOn.Text = DateTime.Parse(oRequest.Get(intRequest, "modified")).ToShortDateString();
                                lblRejectStatus.Text = "REJECTED";
                                lblRejectStatus.CssClass = "denied";
                                panProjectRejected.Visible = true;
                            }
                        }
                        else
                        {
                            if (ds.Tables[0].Rows[0]["rejected"].ToString() == "")
                            {
                                lblTaskName.Text = ds.Tables[0].Rows[0]["name"].ToString();
                                lblTaskDescription.Text = ds.Tables[0].Rows[0]["description"].ToString();
                                if (lblTaskDescription.Text == "")
                                    lblTaskDescription.Text = "None";
                                lblTaskSubmitted.Text = oUser.GetFullName(Int32.Parse(ds.Tables[0].Rows[0]["userid"].ToString()));
                                panTaskAccepted.Visible = true;
                            }
                            else
                            {
                                lblRejectTaskReason.Text = ds.Tables[0].Rows[0]["reason"].ToString();
                                if (lblRejectTaskReason.Text == "")
                                    lblRejectTaskReason.Text = "No reason provided...";
                                txtRejectTaskName.Text = ds.Tables[0].Rows[0]["name"].ToString();
                                txtRejectDescription.Text = ds.Tables[0].Rows[0]["description"].ToString();
                                lblRejectTaskSubmitted.Text = oUser.GetFullName(Int32.Parse(ds.Tables[0].Rows[0]["userid"].ToString()));
                                panTaskRejected.Visible = true;
                            }
                        }
                    }
                }
            }
        }
        protected void btnRejectProject_Click(Object Sender, EventArgs e)
        {
            int intRequest = Int32.Parse(lblRequest.Text);
            int intSegment = 0;
            if (Request.Form[hdnSegment.UniqueID] != "")
                intSegment = Int32.Parse(Request.Form[hdnSegment.UniqueID]);
            oProjectPending.Update(Int32.Parse(lblPending.Text), txtRejectName.Text, ddlRejectType.SelectedItem.Text, txtRejectNumber.Text, Int32.Parse(ddlRejectOrganization.SelectedItem.Value), intSegment, 0, "");
            Response.Redirect(oPage.GetFullLink(intPage) + "?rid=" + intRequest.ToString());
        }
        protected void btnRejectTask_Click(Object Sender, EventArgs e)
        {
            int intRequest = Int32.Parse(lblRequest.Text);
            oProjectPending.Update(Int32.Parse(lblPending.Text), txtRejectTaskName.Text, "", "", 0, 0, 1, txtRejectDescription.Text);
            Response.Redirect(oPage.GetFullLink(intPage) + "?rid=" + intRequest.ToString());
        }
        protected void btnCancel_Click(Object Sender, EventArgs e)
        {
            int intRequest = Int32.Parse(lblRequest.Text);
            //oRequest.Cancel(intRequest);
            oServiceRequest.DeleteAll(intRequest);
            int intProject = oRequest.GetProjectNumber(intRequest);
            DataSet ds = oRequest.Gets(intProject);
            if (ds.Tables[0].Rows.Count == 0)
                oProject.Delete(intProject);
            Response.Redirect(oPage.GetFullLink(intPage) + "?delete=true");
        }
        protected void btnCart_Click(Object Sender, EventArgs e)
        {
            int intRequest = Int32.Parse(lblRequest.Text);
            oServiceRequest.Update(intRequest, -1);
            Response.Redirect(oPage.GetFullLink(intPage) + "?rid=" + intRequest.ToString());
        }
        protected void btnAdd_Click(Object Sender, EventArgs e)
        {
            int intRequest = Int32.Parse(lblRequest.Text);
            if (intRequest == 0)
            {
                intRequest = oRequest.Add(-1, intProfile);
                oServiceRequest.Add(intRequest, 1, -2);
            }
            string strHidden = Request.Form[hdnService.UniqueID];
            while (strHidden != "")
            {
                string strField = strHidden.Substring(0, strHidden.IndexOf("&"));
                strHidden = strHidden.Substring(strHidden.IndexOf("&") + 1);
                string strFlag = strField.Substring(strField.IndexOf("_") + 1);
                strField = strField.Substring(0, strField.IndexOf("_"));
                if (strFlag == "1")
                    oService.AddSelected(intRequest, Int32.Parse(strField), 1);
            }
            Response.Cookies["service_parent"].Value = Request.QueryString["sid"];
            oServiceRequest.Update(intRequest, -1);
            Response.Redirect(oPage.GetFullLink(intPage) + "?rid=" + intRequest.ToString());
        }
        private void LoadServices(int _requestid, int _parent)
        {
            StringBuilder sb = new StringBuilder();
            panServices.Visible = true;
            panDelete.Visible = (Request.QueryString["delete"] != null);
            strCrumbs = oService.BreadCrumb(_parent, oPage.GetFullLink(intPage) + "?rid=" + _requestid.ToString(), "&nbsp;|&nbsp;");
            int intAction = 0;
            bool boolOther = false;

            string strSearch = Request.QueryString["s"];

            if (String.IsNullOrEmpty(strSearch) == false)
            {
                strCrumbs += "&nbsp;|&nbsp;" + "Search Results";
                txtSearch.Text = oFunction.decryptQueryString(strSearch);
                DataSet dsSearch = oService.GetsSearch(txtSearch.Text, 1, 1);
                foreach (DataRow drSearch in dsSearch.Tables[0].Rows)
                {
                    int intService = Int32.Parse(drSearch["serviceid"].ToString());
                    intAction++;
                    sb.Append("<tr style=\"background-color:");
                    sb.Append(boolOther ? "#F6F6F6" : "#FFFFFF");
                    sb.Append("\" id=\"trDept");
                    sb.Append(drSearch["serviceid"].ToString());
                    sb.Append("\">");
                    if (oService.IsRestricted(intService, intProfile) == true)
                        sb.Append("<td nowrap><img src=\"/images/docshare.gif\" border=\"0\" align=\"absmiddle\"/>&nbsp(<a href=\"javascript:void(0);\" onclick=\"alert('This service has been restricted to certain users. Contact the service owner to request access to this service or to submit the service for you.');\">?</a>)</td>");
                    else
                    {
                        sb.Append("<td nowrap><input type=\"checkbox\" onclick=\"HighlightCheckRow(this, 'trDept");
                        sb.Append(intService.ToString());
                        sb.Append("','");
                        sb.Append(intService.ToString());
                        sb.Append("','");
                        sb.Append(hdnService.ClientID);
                        if (oServiceRequest.GetTasks(intProfile, intService).Tables[0].Rows.Count == 0 || boolWM == false)
                            sb.Append("',true);\"/></td>");
                        else
                            sb.Append("',false);\"/></td>");
                    }
                    sb.Append("<td nowrap width=\"20%\"><a href=\"javascript:void(0);\" onclick=\"OpenWindow('SERVICES_DETAIL','?sid=");
                    sb.Append(intService.ToString());
                    sb.Append("');\">");
                    if (drSearch["workflow_title"].ToString() != "")
                        sb.Append(drSearch["workflow_title"].ToString());
                    else
                        sb.Append(drSearch["name"].ToString());
                    sb.Append("</a></td>");
                    sb.Append("<td width=\"60%\">");
                    sb.Append(drSearch["description"].ToString());
                    sb.Append("</td>");
                    int intItem = oService.GetItemId(intService);
                    string strItem = "";
                    DataSet dsManagers = oService.GetUser(intService, -1);
                    foreach (DataRow drManager in dsManagers.Tables[0].Rows)
                    {
                        if (strItem != "")
                            strItem += "<br/>";
                        strItem += "<a href=\"javascript:void(0);\" onclick=\"OpenWindow('PROFILE','?userid=" + drManager["userid"].ToString() + "');\"><img src=\"/images/user.gif\" border=\"0\" align=\"absmiddle\"/> " + oUser.GetFullName(Int32.Parse(drManager["userid"].ToString())) + "</a>";
                    }
                    if (strItem == "")
                    {
                        // Check the people that get assigned
                        dsManagers = oService.GetUser(intService, 0);
                        foreach (DataRow drManager in dsManagers.Tables[0].Rows)
                        {
                            if (strItem != "")
                                strItem += ", ";
                            strItem += "<a href=\"javascript:void(0);\" onclick=\"OpenWindow('PROFILE','?userid=" + drManager["userid"].ToString() + "');\">" + oUser.GetFullName(Int32.Parse(drManager["userid"].ToString())) + "</a>";
                        }
                    }
                    sb.Append("<td width=\"20%\" nowrap>");
                    sb.Append(strItem);
                    sb.Append("</td>");
                    sb.Append("</tr>");
                    sb.Append("<tr style=\"background-color:");
                    sb.Append(boolOther ? "#F6F6F6" : "#FFFFFF");
                    sb.Append("\">");
                    sb.Append("<td colspan=\"2\" align=\"right\" valign=\"top\"><i>Location(s):</i></td>");
                    sb.Append("<td colspan=\"10\">");
                    sb.Append(drSearch["folders"].ToString().Replace("|", "<br/>"));
                    sb.Append("</td>");
                    sb.Append("</tr>");
                    boolOther = !boolOther;
                    // Need stored procedure to wrap around function
                    //sb.Append("<tr>");
                    //sb.Append("<td colspan=\"2\">");
                    //sb.Append();
                    //sb.Append("</td>");
                    //sb.Append("</tr>");
                }
            }
            else
            {
                if (_parent == 0)
                {
                    bool boolFavorite = false;
                    panFavorites.Visible = true;
                    DataSet dsFavorite = oService.GetFavorites(intProfile);
                    foreach (DataRow drFavorite in dsFavorite.Tables[0].Rows)
                    {
                        int intService = Int32.Parse(drFavorite["serviceid"].ToString());
                        sb.Append("<tr style=\"background-color:");
                        sb.Append(boolFavorite ? "#F6F6F6" : "#FFFFFF");
                        sb.Append("\" id=\"trDept");
                        sb.Append(drFavorite["serviceid"].ToString());
                        sb.Append("\">");
                        boolFavorite = !boolFavorite;
                        if (oService.IsRestricted(intService, intProfile) == true)
                            sb.Append("<td nowrap><img src=\"/images/docshare.gif\" border=\"0\" align=\"absmiddle\"/>&nbsp(<a href=\"javascript:void(0);\" onclick=\"alert('This service has been restricted to certain users. Contact the service owner to request access to this service or to submit the service for you.');\">?</a>)</td>");
                        else
                        {
                            sb.Append("<td nowrap><input type=\"checkbox\" onclick=\"HighlightCheckRow(this, 'trDept");
                            sb.Append(intService.ToString());
                            sb.Append("','");
                            sb.Append(intService.ToString());
                            sb.Append("','");
                            sb.Append(hdnService.ClientID);
                            if (oServiceRequest.GetTasks(intProfile, intService).Tables[0].Rows.Count == 0 || boolWM == false)
                                sb.Append("',true);\"/></td>");
                            else
                                sb.Append("',false);\"/></td>");
                        }
                        sb.Append("<td nowrap width=\"20%\"><a href=\"javascript:void(0);\" onclick=\"OpenWindow('SERVICES_DETAIL','?sid=");
                        sb.Append(intService.ToString());
                        sb.Append("');\">");
                        if (drFavorite["workflow_title"].ToString() != "")
                            sb.Append(drFavorite["workflow_title"].ToString());
                        else
                            sb.Append(drFavorite["name"].ToString());
                        sb.Append("</a></td>");
                        sb.Append("<td width=\"60%\">");
                        sb.Append(drFavorite["description"].ToString());
                        sb.Append("</td>");
                        int intItem = oService.GetItemId(intService);
                        string strItem = "";
                        DataSet dsManagers = oService.GetUser(intService, -1);
                        foreach (DataRow drManager in dsManagers.Tables[0].Rows)
                        {
                            if (strItem != "")
                                strItem += "<br/>";
                            strItem += "<a href=\"javascript:void(0);\" onclick=\"OpenWindow('PROFILE','?userid=" + drManager["userid"].ToString() + "');\"><img src=\"/images/user.gif\" border=\"0\" align=\"absmiddle\"/> " + oUser.GetFullName(Int32.Parse(drManager["userid"].ToString())) + "</a>";
                        }
                        if (strItem == "")
                        {
                            // Check the people that get assigned
                            dsManagers = oService.GetUser(intService, 0);
                            foreach (DataRow drManager in dsManagers.Tables[0].Rows)
                            {
                                if (strItem != "")
                                    strItem += ", ";
                                strItem += "<a href=\"javascript:void(0);\" onclick=\"OpenWindow('PROFILE','?userid=" + drManager["userid"].ToString() + "');\">" + oUser.GetFullName(Int32.Parse(drManager["userid"].ToString())) + "</a>";
                            }
                        }
                        sb.Append("<td width=\"20%\" nowrap>");
                        sb.Append(strItem);
                        sb.Append("</td>");
                        sb.Append("</tr>");
                    }

                    if (sb.ToString() == "")
                    {
                        sb.Append("<tr><td colspan=\"4\"> You have not selected any favorites</td></tr>");
                        btnFavorite.Enabled = false;
                    }

                    strFavorites = sb.ToString();

                    strFavorites = "<tr bgcolor=\"#EEEEEE\"><td></td><td width=\"20%\"><b><u>Service:</u></b></td><td width=\"60%\"><b><u>Description:</u></b></td><td width=\"20%\" nowrap><b><u>Service Owner:</u></b></td></tr>" + strFavorites;
                    strFavorites = "<table width=\"100%\" border=\"0\" cellpadding=\"4\" cellspacing=\"0\" style=\"border:solid 1px #CCCCCC\">" + strFavorites + "</table>";
                }

                sb = new StringBuilder();
                DataSet dsAvailable = oService.GetFolders(_parent, 1);
                // Load Folders
                foreach (DataRow drService in dsAvailable.Tables[0].Rows)
                {
                    int intFolder = Int32.Parse(drService["id"].ToString());
                    sb.Append("<tr style=\"background-color:");
                    sb.Append(boolOther ? "#F6F6F6" : "#FFFFFF");
                    sb.Append("\" id=\"trDept");
                    sb.Append(intFolder.ToString());
                    sb.Append("\">");
                    boolOther = !boolOther;
                    sb.Append("<td nowrap><img src=\"/images/folder24.gif\" border=\"0\" align=\"absmiddle\" />");
                    sb.Append("<td nowrap width=\"20%\"><a href=\"");
                    sb.Append(oPage.GetFullLink(intPage));
                    sb.Append("?rid=");
                    sb.Append(_requestid.ToString());
                    sb.Append("&sid=");
                    sb.Append(intFolder.ToString());
                    sb.Append("\">");
                    sb.Append(drService["name"].ToString());
                    sb.Append("</a> <span class=\"bluenote\"> (");
                    sb.Append(oService.GetFolderCount(intFolder).ToString());
                    sb.Append(")</span></td>");
                    sb.Append("<td width=\"60%\">");
                    sb.Append(drService["description"].ToString());
                    sb.Append("</td>");
                    string strUser = "";
                    int intUser = Int32.Parse(oService.GetFolder(intFolder, "userid"));
                    if (intUser > 0)
                    {
                        strUser += "<a href=\"javascript:void(0);\" onclick=\"OpenWindow('PROFILE','?userid=" + intUser.ToString() + "');\">" + oUser.GetFullName(intUser) + "</a>";
                    }
                    sb.Append("<td width=\"20%\" nowrap>");
                    sb.Append(strUser);
                    sb.Append("</td>");
                    sb.Append("</tr>");
                }
                // Load Services
                DataSet dsChildren = oService.Gets(_parent, 1, 1, 0, 0);
                foreach (DataRow drService in dsChildren.Tables[0].Rows)
                {
                    int intService = Int32.Parse(drService["serviceid"].ToString());
                    intAction++;
                    sb.Append("<tr style=\"background-color:");
                    sb.Append(boolOther ? "#F6F6F6" : "#FFFFFF");
                    sb.Append("\" id=\"trDept");
                    sb.Append(drService["serviceid"].ToString());
                    sb.Append("\">");
                    boolOther = !boolOther;
                    if (oService.IsRestricted(intService, intProfile) == true)
                        sb.Append("<td nowrap><img src=\"/images/docshare.gif\" border=\"0\" align=\"absmiddle\"/>&nbsp(<a href=\"javascript:void(0);\" onclick=\"alert('This service has been restricted to certain users. Contact the service owner to request access to this service or to submit the service for you.');\">?</a>)</td>");
                    else
                    {
                        sb.Append("<td nowrap><input type=\"checkbox\" onclick=\"HighlightCheckRow(this, 'trDept");
                        sb.Append(intService.ToString());
                        sb.Append("','");
                        sb.Append(intService.ToString());
                        sb.Append("','");
                        sb.Append(hdnService.ClientID);
                        if (oServiceRequest.GetTasks(intProfile, intService).Tables[0].Rows.Count == 0 || boolWM == false)
                            sb.Append("',true);\"/></td>");
                        else
                            sb.Append("',false);\"/></td>");
                    }
                    sb.Append("<td nowrap width=\"20%\"><a href=\"javascript:void(0);\" onclick=\"OpenWindow('SERVICES_DETAIL','?sid=");
                    sb.Append(intService.ToString());
                    sb.Append("');\">");
                    string name = drService["name"].ToString();
                    if (drService["workflow_title"].ToString() != "")
                        name = drService["workflow_title"].ToString();
                    sb.Append(name);
                    sb.Append("</a>");
                    sb.Append(" <a href=\"javascript:void(0);\" onclick=\"OpenWindow('SERVICE_LINK','?s=" + oFunction.encryptQueryString(name) + "&f=" + Request.QueryString["sid"] + "');\"><img src=\"/images/link.gif\" border=\"0\" align=\"absmiddle\"/></a>");
                    sb.Append("</td>");
                    sb.Append("<td width=\"60%\">");
                    sb.Append(drService["description"].ToString());
                    sb.Append("</td>");
                    int intItem = oService.GetItemId(intService);
                    string strItem = "";
                    DataSet dsManagers = oService.GetUser(intService, -1);
                    foreach (DataRow drManager in dsManagers.Tables[0].Rows)
                    {
                        if (strItem != "")
                            strItem += "<br/>";
                        strItem += "<a href=\"javascript:void(0);\" onclick=\"OpenWindow('PROFILE','?userid=" + drManager["userid"].ToString() + "');\"><img src=\"/images/user.gif\" border=\"0\" align=\"absmiddle\"/> " + oUser.GetFullName(Int32.Parse(drManager["userid"].ToString())) + "</a>";
                    }
                    if (strItem == "")
                    {
                        // Check the people that get assigned
                        dsManagers = oService.GetUser(intService, 0);
                        foreach (DataRow drManager in dsManagers.Tables[0].Rows)
                        {
                            if (strItem != "")
                                strItem += ", ";
                            strItem += "<a href=\"javascript:void(0);\" onclick=\"OpenWindow('PROFILE','?userid=" + drManager["userid"].ToString() + "');\">" + oUser.GetFullName(Int32.Parse(drManager["userid"].ToString())) + "</a>";
                        }
                    }
                    sb.Append("<td width=\"20%\" nowrap>");
                    sb.Append(strItem);
                    sb.Append("</td>");
                    sb.Append("</tr>");
                }
            }

            strServices = sb.ToString();

            if (strServices != "")
            {
                strServices = "<tr bgcolor=\"#EEEEEE\"><td></td><td width=\"20%\"><b><u>Service:</u></b></td><td width=\"60%\"><b><u>Description:</u></b></td><td width=\"20%\" nowrap><b><u>Service Owner:</u></b></td></tr>" + strServices;
                strServices = "<table width=\"100%\" border=\"0\" cellpadding=\"4\" cellspacing=\"0\" style=\"border:solid 1px #CCCCCC\">" + strServices + "</table>";
            }

            btnAdd.Enabled = (intAction > 0);
            txtSearch.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + imgSearch.ClientID + "').click();return false;}} else {return true}; ");
            imgSearch.Attributes.Add("onclick", "return ValidateText('" + txtSearch.Text + "','Enter some text to search') && LoadWait();");
        }
        private void LoadCart(int _requestid)
        {
            strSummary = "";
            DataSet dsSelected = oService.GetSelected(_requestid);
            int intCount = 0;
            double dblTotal = 0.00;
            bool boolOther = false;
            foreach (DataRow dr in dsSelected.Tables[0].Rows)
            {
                int _serviceid = Int32.Parse(dr["serviceid"].ToString());
                int _number = Int32.Parse(dr["number"].ToString());
                int intItem = oService.GetItemId(_serviceid);
                intCount++;
                if (boolOther == true)
                    strSummary += "<tr bgcolor=\"#F6F6F6\">";
                else
                    strSummary += "<tr>";
                boolOther = !boolOther;
                strSummary += "<td nowrap align=\"right\">" + intCount.ToString() + ".)</td>";
                if (oService.Get(_serviceid, "multiple_quantity") == "1")
                    strSummary += "<td width=\"50\" align=\"center\" nowrap><input type=\"text\" style=\"width:35\" class=\"default\" value=\"" + dr["quantity"].ToString() + "\" maxlength=\"3\" onblur=\"UpdateQuantitySR(this,'" + _serviceid.ToString() + "$" + _number.ToString() + "','" + hdnQuantity.ClientID + "');\"/></td>";
                else
                    strSummary += "<td width=\"50\" align=\"center\">" + dr["quantity"].ToString() + "</td>";
                string strWorkflowTitle = oService.Get(_serviceid, "workflow_title");
                strSummary += "<td width=\"40%\"><a href=\"javascript:void(0);\" onclick=\"OpenWindow('SERVICES_DETAIL','?rid=" + _requestid.ToString() + "&sid=" + _serviceid.ToString() + "&sidn=" + _number.ToString() + "');\" title=\"Click here to view the details of this service\">" + (strWorkflowTitle == "" ? oService.Get(_serviceid, "name") : strWorkflowTitle) + "</a></td>";
                strSummary += "<td width=\"50%\">" + oService.Get(_serviceid, "description") + "</td>";
                double dblQuantity = double.Parse(dr["quantity"].ToString());
                double dblSLA = double.Parse(oService.Get(_serviceid, "sla"));
                string strSLA = "---";
                if (oService.Get(_serviceid, "hide_sla") != "1")
                {
                    if (dblSLA > 0.00)
                    {
                        dblTotal += dblSLA;
                        strSLA = dblSLA.ToString("F") + " HRs";
                    }
                }
                else
                    strSLA = "<i>Hidden</i>";
                strSummary += "<td width=\"10%\" align=\"right\">" + strSLA + "</td>";
                strSummary += "<td nowrap align=\"center\">[<a href=\"" + oPage.GetFullLink(intPage) + "?rid=" + _requestid.ToString() + "&did=" + _serviceid.ToString() + "&didn=" + _number.ToString() + "\" title=\"Click here to remove this service\" onclick=\"return ConfirmDeleteSubItem('remove',null);\">Remove</a>]</td>";
                strSummary += "</tr>";
            }
            lblHoursCheckout.Text = dblTotal.ToString("F") + " HRs";
            if (strSummary == "")
            {
                strSummary = "<tr><td colspan=\"10\"><img src=\"/images/bigAlert.gif\" border=\"0\" align=\"absmiddle\"> There are no services associated with this request</td></tr>";
                btnCheckout.Enabled = false;
            }
            strSummary = "<tr bgcolor=\"#EEEEEE\"><td></td><td width=\"50\" align=\"center\" nowrap><b><u>Qty:</u></b></td><td width=\"40%\"><b><u>Service:</u></b></td><td width=\"50%\"><b><u>Description:</u></b></td><td width=\"10%\" align=\"right\"><b><u>SLA:</u></b></td><td><img src=\"/images/spacer.gif\" border=\"0\" width=\"75\" height=\"1\" /></td></tr>" + strSummary;
            strSummary = "<table width=\"100%\" border=\"0\" cellpadding=\"4\" cellspacing=\"0\" style=\"border:solid 1px #CCCCCC\">" + strSummary + "</table>";
        }
        protected void btnServices_Click(Object Sender, EventArgs e)
        {
            int intRequest = Int32.Parse(lblRequest.Text);
            oServiceRequest.Update(intRequest, -2);
            Response.Redirect(oPage.GetFullLink(intPage) + "?rid=" + intRequest.ToString() + "&add=true");
        }
        protected void btnOrder_Click(Object Sender, EventArgs e)
        {
            LinkButton oButton = (LinkButton)Sender;
            int intRequest = Int32.Parse(lblRequest.Text);
            Response.Redirect(oPage.GetFullLink(intPage) + "?rid=" + intRequest.ToString() + "&order=" + oButton.CommandArgument);
        }
        protected void imgSearch_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(oPage.GetFullLink(intPage) + "?rid=" + Request.QueryString["rid"] + "&s=" + oFunction.encryptQueryString(txtSearch.Text));
        }
        protected void btnSelect_Click(Object Sender, EventArgs e)
        {
            int intRequest = Int32.Parse(lblRequest.Text);
            if (lstProjects.SelectedIndex > -1)
            {
                if (lstProjects.SelectedIndex == 0)
                    Response.Redirect(oPage.GetFullLink(intPage) + "?rid=" + intRequest.ToString() + "&new=true");
                else
                {
                    oRequest.Update(intRequest, Int32.Parse(lstProjects.SelectedItem.Value));
                    Response.Redirect(oPage.GetFullLink(intPage) + "?rid=" + intRequest.ToString());
                }
            }
            else
                Response.Redirect(oPage.GetFullLink(intPage) + "?rid=" + intRequest.ToString() + "&select=false");
        }
        protected void btnPendingProject_Click(Object Sender, EventArgs e)
        {
            int intRequest = Int32.Parse(lblRequest.Text);
            int intProject = 0;
            int intSegment = 0;
            bool boolAdd = false;
            if (txtNumber.Text != "")
            {
                ds = oProject.Get(txtNumber.Text);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    intProject = Int32.Parse(ds.Tables[0].Rows[0]["projectid"].ToString());
                    oRequest.Update(intRequest, intProject);
                    oProject.Update(intProject, 2);
                    Response.Redirect(oPage.GetFullLink(intPage) + "?rid=" + intRequest.ToString() + "&sent=true");
                }
                else
                {
                    if (txtName.Text == "" || ddlBaseDisc.SelectedIndex == 0 || ddlOrganization.SelectedIndex == 0)
                        Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "validation2", "<script type=\"text/javascript\">alert('Invalid Project Number!\\n\\nPlease enter a project name, select a project type, and select a sponsoring portfolio to continue.');<" + "/" + "script>");
                    else
                    {
                        // Add if TPM
                        if (oApplication.Get(intApplication, "tpm") == "1")
                        {
                            if (Request.Form[hdnSegment.UniqueID] != "")
                                intSegment = Int32.Parse(Request.Form[hdnSegment.UniqueID]);
                            intProject = oProject.Add(txtName.Text, ddlBaseDisc.SelectedItem.Text, txtNumber.Text, intProfile, Int32.Parse(ddlOrganization.SelectedItem.Value), intSegment, 2);
                            oRequest.Update(intRequest, intProject);
                            Response.Redirect(oPage.GetFullLink(intPage) + "?rid=" + intRequest.ToString() + "&added=true");
                        }
                        else
                            boolAdd = true;
                    }
                }
            }
            else
                boolAdd = true;
            if (boolAdd == true)
            {
                if (Request.Form[hdnSegment.UniqueID] != "")
                    intSegment = Int32.Parse(Request.Form[hdnSegment.UniqueID]);
                if (oApplication.Get(intApplication, "tpm") == "1")
                {
                    intProject = oProject.Add(txtName.Text, ddlBaseDisc.SelectedItem.Text, txtNumber.Text, intProfile, Int32.Parse(ddlOrganization.SelectedItem.Value), intSegment, 2);
                    oRequest.Update(intRequest, intProject);
                    Response.Redirect(oPage.GetFullLink(intPage) + "?rid=" + intRequest.ToString() + "&added=true");
                }
                else
                {
                    oProjectPending.Add(intRequest, txtName.Text, ddlBaseDisc.SelectedItem.Text, txtNumber.Text, intProfile, Int32.Parse(ddlOrganization.SelectedItem.Value), intSegment, 0, "");
                    oRequest.Update(intRequest, -2);
                    Response.Redirect(oPage.GetFullLink(intPage) + "?rid=" + intRequest.ToString() + "&sent=true");
                }
            }
        }
        protected void btnPendingTask_Click(Object Sender, EventArgs e)
        {
            int intRequest = Int32.Parse(lblRequest.Text);
            // ******** Add for automatic approval of tasks - 2/6/2008
            //oProjectPending.Add(intRequest, txtTaskName.Text, "", "", intProfile, 0, 1, txtDescription.Text, strBCC);
            //oRequest.Update(intRequest, -2);
            oRequest.UpdateDescription(intRequest, txtDescription.Text);
            oRequest.Update(intRequest, 0);
            oDocument.Update(intRequest, 0);
            oServiceRequest.Update(intRequest, txtTaskName.Text);
            Response.Redirect(oPage.GetFullLink(intPage) + "?rid=" + intRequest.ToString() + "&sent=true");
        }
        protected void btnBack_Click(Object Sender, EventArgs e)
        {
            int intRequest = Int32.Parse(lblRequest.Text);
            Response.Redirect(oPage.GetFullLink(intPage) + "?rid=" + intRequest.ToString());
        }
        protected void btnUpdate_Click(Object Sender, EventArgs e)
        {
            int intRequest = Int32.Parse(lblRequest.Text);
            string strHidden = Request.Form[hdnQuantity.UniqueID];
            while (strHidden != "")
            {
                string strField = strHidden.Substring(0, strHidden.IndexOf("&"));
                strHidden = strHidden.Substring(strHidden.IndexOf("&") + 1);
                int intQuantity = 0;

                int.TryParse(strField.Substring(strField.IndexOf("_") + 1), out intQuantity);
                
                strField = strField.Substring(0, strField.IndexOf("_"));
                string strNumber = strField.Substring(strField.IndexOf("$") + 1);
                strField = strField.Substring(0, strField.IndexOf("$"));
                if (intQuantity > 0)
                    oService.UpdateSelected(intRequest, Int32.Parse(strField), Int32.Parse(strNumber), intQuantity);
            }
            Response.Redirect(oPage.GetFullLink(intPage) + "?rid=" + intRequest.ToString());
        }
        protected void btnCheckout_Click(Object Sender, EventArgs e)
        {
            int intRequest = Int32.Parse(lblRequest.Text);
            string strHidden = Request.Form[hdnQuantity.UniqueID];
            while (strHidden != "")
            {
                string strField = strHidden.Substring(0, strHidden.IndexOf("&"));
                strHidden = strHidden.Substring(strHidden.IndexOf("&") + 1);
                int intQuantity = 0;
                try { intQuantity = Int32.Parse(strField.Substring(strField.IndexOf("_") + 1)); }
                catch { }
                strField = strField.Substring(0, strField.IndexOf("_"));
                string strNumber = strField.Substring(strField.IndexOf("$") + 1);
                strField = strField.Substring(0, strField.IndexOf("$"));
                if (intQuantity > 0)
                    oService.UpdateSelected(intRequest, Int32.Parse(strField), Int32.Parse(strNumber), intQuantity);
            }
            intNumberCount = oRequestItem.GetForms(intRequest).Tables[0].Rows.Count;
            // Load Forms        
            DataSet dsSelected = oService.GetSelected(intRequest);
            foreach (DataRow dr in dsSelected.Tables[0].Rows)
            {
                int _serviceid = Int32.Parse(dr["serviceid"].ToString());
                int _itemid = oService.GetItemId(_serviceid);
                //            // Remove Duplicates
                //            if (oRequestItem.GetForm(intRequest, _serviceid).Tables[0].Rows.Count == 0)
                //            {
                int _quantity = Int32.Parse(dr["quantity"].ToString());
                if (oService.Get(_serviceid, "quantity_is_device") == "1")
                    _quantity = 1;
                for (int ii = 1; ii <= _quantity; ii++)
                {
                    intNumberCount++;
                    oRequestItem.AddForm(intRequest, _itemid, _serviceid, intNumberCount);
                }
                //            }
            }
            oServiceRequest.Update(intRequest, 0);
            Response.Redirect(oPage.GetFullLink(intPage) + "?rid=" + intRequest.ToString());
        }
        protected void btnComplete_Click(Object Sender, EventArgs e)
        {
            int intRequest = Int32.Parse(lblRequest.Text);
            oServiceRequest.Update(intRequest, txtTitle.Text);
            oServiceRequest.Update(intRequest, 1);
            Response.Redirect(oPage.GetFullLink(intPage) + "?rid=" + intRequest.ToString());
        }
        protected void btnRefresh_Click(Object Sender, EventArgs e)
        {
            int intRequest = Int32.Parse(lblRequest.Text);
            Response.Redirect(oPage.GetFullLink(intPage) + "?rid=" + intRequest.ToString());
        }
        private void LoadFinishSummary(int _requestid)
        {
            panTitle.Visible = true;
            txtTitle.Text = oServiceRequest.Get(_requestid, "name");
            if (txtTitle.Text == "")
            {
                DataSet dsSelected = oServiceEditor.GetWorkflow(_requestid);
                if (dsSelected.Tables[0].Rows.Count > 0)
                {
                    int _serviceid = Int32.Parse(dsSelected.Tables[0].Rows[0]["serviceid"].ToString());
                    int _number = Int32.Parse(dsSelected.Tables[0].Rows[0]["number"].ToString());
                    //DataSet dsServiceEditor = oServiceEditor.GetRequestData(_requestid, _serviceid, _number, -1);
                    DataSet dsServiceEditor = oServiceEditor.GetRequestData(_requestid, _serviceid, _number, 0, dsn);
                    if (dsServiceEditor.Tables[0].Rows.Count > 0)
                        txtTitle.Text = dsServiceEditor.Tables[0].Rows[0]["title"].ToString();
                }
            }

            btnPrinter.Visible = false;
            btnRefresh.Visible = false;
            bool boolManagerApprovalRequired = LoadSummary(_requestid, false);
            oServiceRequest.UpdateApproval(_requestid, (boolManagerApprovalRequired ? 0 : 1));
            if (boolManagerApprovalRequired)
            {
                panManagerApproval.Visible = true;
                int intManager = 0;
                SearchResultCollection oCollection = oFunction.eDirectory(oUser.GetName(intProfile));
                if (oCollection.Count == 1 && oCollection[0].Properties.Contains("pncmanagerid") == true)
                {
                    string strManager = oFunction.eDirectory(oCollection[0], "pncmanagerid");
                    DataSet dsManager = oUser.Gets(strManager);
                    if (dsManager.Tables[0].Rows.Count == 1)
                        intManager = Int32.Parse(dsManager.Tables[0].Rows[0]["userid"].ToString());
                    if (intManager > 0)
                        oUser.Update(intProfile, intManager);
                }
                else if (oCollection.Count > 0 && oCollection[0].Properties.Contains("pncmanagername") == true && oFunction.eDirectory(oCollection[0], "pncmanagername") != "")
                {
                    // Known issue with lookup, continue using current manager (if applicable)
                    Int32.TryParse(oUser.Get(intProfile, "manager"), out intManager);
                }
                if (intManager == 0)
                {
                    panManagerError.Visible = true;
                    panManagerApproval.Visible = false;
                    btnComplete.Enabled = false;
                }
                else
                    lblManager.Text = oUser.GetFullName(intManager);
            }
        }
        protected void LoadCompletedSummary(int _requestid)
        {
            btnComplete.Visible = false;
            btnTitle.Visible = false;
            btnCancel3.Visible = false;
            btnPrinter.Attributes.Add("onclick", "return OpenWindow('NEW_WINDOW','/frame/printer.aspx?rid=" + _requestid.ToString() + "');");
            LoadSummary(_requestid, true);
            btnPrinter.Visible = String.IsNullOrEmpty(Request.QueryString["approve"]);
            btnRefresh.Visible = String.IsNullOrEmpty(Request.QueryString["approve"]);
        }
        protected bool LoadSummary(int _requestid, bool _submitted)
        {
            bool boolManagerApprovalRequired = false;
            lblHeader.Text = "Service Request Summary";
            lblHeaderSub.Text = "Here is a summary of your services you have requested.";
            panHeader.Visible = true;
            strSummary = "";
            string strSummaryExecute = "";
            //DataSet dsSelected = oService.GetSelectedCart(_requestid);
            DataSet dsSelected = oServiceEditor.GetWorkflow(_requestid);
            double dblTotal = 0.00;
            bool boolOther = false;
            foreach (DataRow dr in dsSelected.Tables[0].Rows)
            {
                bool boolCancelled = (dr["cancelled"].ToString() != "");
                int _status = -100;
                if (Int32.TryParse(dr["Status"].ToString(), out _status) == false)
                    _status = -100;
                else if (_status == (int)ResourceRequestStatus.Cancelled)
                    boolCancelled = true;

                int _formid = 0;
                Int32.TryParse(dr["formid"].ToString(), out _formid);
                int _serviceid = Int32.Parse(dr["serviceid"].ToString());
                if (boolManagerApprovalRequired == false)
                    boolManagerApprovalRequired = (oService.Get(_serviceid, "manager_approval") == "1");
                int _number = Int32.Parse(dr["number"].ToString());
                string strCreated = dr["created"].ToString();
                int intItem = oService.GetItemId(_serviceid);
                if (boolOther == true)
                    strSummary += "<tr bgcolor=\"#F6F6F6\">";
                else
                    strSummary += "<tr>";
                int intResource = 0;
                Int32.TryParse(dr["resourceid"].ToString(), out intResource);
                boolOther = !boolOther;

                string strDecommission = "";
                bool boolDecomAuto = false;
                bool boolDecomManual = false;
                bool boolDecomSubmitted = false;
                string strRebuild = "";
                bool boolRebuild = false;
                bool boolRebuildSubmitted = false;

                // Check Decommission
                if (_serviceid == intServiceDecommission)
                {
                    // Decommission - check the decom date for Edit / Cancel / View options
                    Customized oCustomized = new Customized(0, dsn);
                    Asset oAsset = new Asset(0, dsnAsset, dsn);
                    DateTime datDecom = DateTime.Now.AddDays(1.00);
                    DataSet dsDecommission = oAsset.GetDecommission(_requestid, _number, 2);
                    if (dsDecommission.Tables[0].Rows.Count > 0)
                    {
                        boolDecomAuto = true;
                        if (dsDecommission.Tables[0].Rows[0]["recommissioned"].ToString() != "")
                            boolCancelled = true;
                        else if (DateTime.TryParse(dsDecommission.Tables[0].Rows[0]["decom"].ToString(), out datDecom))
                        {
                            strDecommission = datDecom.ToShortDateString();
                            boolDecomSubmitted = (datDecom < DateTime.Now);
                        }
                    }
                    else
                    {
                        dsDecommission = oCustomized.GetDecommissionServer(_requestid, intItem, _number);
                        if (dsDecommission.Tables[0].Rows.Count > 0)
                        {
                            boolDecomManual = true;
                            if (dsDecommission.Tables[0].Rows[0]["poweroff_new"].ToString() != "")
                            {
                                if (DateTime.TryParse(dsDecommission.Tables[0].Rows[0]["poweroff_new"].ToString(), out datDecom))
                                {
                                    strDecommission = datDecom.ToShortDateString();
                                    boolDecomSubmitted = (datDecom < DateTime.Now);
                                }
                            }
                            else if (DateTime.TryParse(dsDecommission.Tables[0].Rows[0]["poweroff"].ToString(), out datDecom))
                            {
                                strDecommission = datDecom.ToShortDateString();
                                boolDecomSubmitted = (datDecom < DateTime.Now);
                            }
                            DataSet dsDecommissionRR = oResourceRequest.GetRequestService(_requestid, _serviceid, _number);
                            if (dsDecommissionRR.Tables[0].Rows.Count > 0)
                                boolCancelled = (dsDecommissionRR.Tables[0].Rows[0]["status"].ToString() == "-2");
                        }
                    }
                }
                // Check Rebuild
                else if (_serviceid == intServiceRebuildWorkstation)
                {
                    // Rebuild - check the rebuild date for Edit / Cancel / View options
                    Workstations oWorkstation = new Workstations(0, dsn);
                    DateTime datRebuild = DateTime.Now.AddDays(1.00);
                    DataSet dsRebuild = oWorkstation.GetVirtualRebuild(_requestid, _serviceid, _number);
                    foreach (DataRow drRebuild in dsRebuild.Tables[0].Rows)
                    {
                        boolRebuild = true;
                        if (drRebuild["cancelled"].ToString() == "")
                        {
                            if (DateTime.TryParse(drRebuild["scheduled"].ToString(), out datRebuild))
                            {
                                strRebuild = datRebuild.ToShortDateString();
                                boolRebuildSubmitted = (drRebuild["submitted"].ToString() != "" && datRebuild < DateTime.Now);
                            }
                            boolCancelled = false;
                            break;
                        }
                        else
                            boolCancelled = true;
                    }
                }



                // ************ REQUESTID ************
                if (_formid > 0)
                    strSummary += "<td width=\"10%\" nowrap>";
                else
                    strSummary += "<td width=\"10%\" nowrap><img src=\"/images/down_right.gif\" border=\"0\" align=\"absmiddle\"/>&nbsp;";

                if (strCreated != "" || dr["ondemand"].ToString() == "1")
                {
                    if (oUser.IsAdmin(intProfile) == true || (String.IsNullOrEmpty(Request.QueryString["approve"]) && (oDataPoint.GetPagePermission(intApplication, "SERVICE") == true || intDataPointAvailableService == 1)))
                    {
                        if (intResource > 0)
                            strSummary += "<a href=\"javascript:void(0);\" onclick=\"OpenNewWindowMenu('/datapoint/service/resource.aspx?id=" + oFunction.encryptQueryString(intResource.ToString()) + "', '800', '600');\"/>CVT" + _requestid.ToString() + "-" + _serviceid.ToString() + "-" + _number.ToString() + "</a></td>";
                        else if (_formid > 0)
                            strSummary += "<a href=\"javascript:void(0);\" onclick=\"OpenNewWindowMenu('/datapoint/service/form.aspx?id=" + oFunction.encryptQueryString(_formid.ToString()) + "', '800', '600');\"/>CVT" + _requestid.ToString() + "-" + _serviceid.ToString() + "-" + _number.ToString() + "</a></td>";
                        else
                            strSummary += "<a href=\"javascript:void(0);\" onclick=\"OpenNewWindowMenu('/datapoint/service/request.aspx?t=id&q=" + oFunction.encryptQueryString("CVT" + _requestid.ToString()) + "&id=" + oFunction.encryptQueryString(_requestid.ToString()) + "', '800', '600');\"/>CVT" + _requestid.ToString() + "-" + _serviceid.ToString() + "-" + _number.ToString() + "</a></td>";
                    }
                    else
                        strSummary += "CVT" + _requestid.ToString() + "-" + _serviceid.ToString() + "-" + _number.ToString() + "</td>";
                }
                else if (dr["skipped"].ToString() == "1")
                    strSummary += "<i>N / A</i></td>";
                else
                    strSummary += "<i>Pending</i></td>";

                // ************ SERVICE ************
                string strWorkflowTitle = oService.Get(_serviceid, "workflow_title");
                if (_submitted == false)
                    strSummary += "<td width=\"40%\"><a href=\"javascript:void(0);\" onclick=\"OpenWindow('SERVICES_DETAIL','?rid=" + _requestid.ToString() + "&sid=" + _serviceid.ToString() + "');\" title=\"Click here to view the details of this service\">" + (strWorkflowTitle == "" ? oService.Get(_serviceid, "name") : strWorkflowTitle) + "</a></td>";
                else
                {
                    //strSummary += "<td width=\"30%\"><a href=\"javascript:void(0);\" onclick=\"OpenWindow('SERVICE_STATUS','" + dr["path"].ToString() + "?rid=" + _requestid.ToString() + "&sid=" + _serviceid.ToString() + "&sidn=" + _number.ToString() + "');\" title=\"Click here to view the details of this service\">" + oService.Get(_serviceid, "name") + "</a></td>";
                    strSummary += "<td width=\"40%\"><a href=\"javascript:void(0);\" onclick=\"OpenNewWindowMenu('/datapoint/service/request.aspx?t=id&q=" + oFunction.encryptQueryString("CVT" + _requestid.ToString()) + "&id=" + oFunction.encryptQueryString(_requestid.ToString()) + "', '800', '600');\"/>" + (strWorkflowTitle == "" ? oService.Get(_serviceid, "name") : strWorkflowTitle) + "</a></td>";
                }


                // ************ STATUS ************
                string strStatus = "";
                string strStatusService = "";
                StringBuilder strUsers = new StringBuilder();
                List<WorkflowStatus> RR = oResourceRequest.GetStatus(null, intResource, null, null, null, null, false, dsnServiceEditor);
                if (RR.Count == 0)
                    RR = oResourceRequest.GetStatus(null, null, _requestid, _serviceid, null, _number, false, dsnServiceEditor);
                if (RR.Count > 0)
                {
                    foreach (string strUser in RR[0].users)
                    {
                        if (String.IsNullOrEmpty(strUser) == false)
                        {
                            strUsers.Append(strUser);
                            strUsers.AppendLine("<br/>");
                        }
                    }
                    strStatus = RR[0].status;
                    strStatusService = RR[0].service;
                }
                strSummary += "<td width=\"20%\">" + strUsers.ToString() + "</td>";

                int intManagerApproved = 1;
                Int32.TryParse(oServiceRequest.Get(_requestid, "manager_approval"), out intManagerApproved);
                int intApproved = 1;
                DataSet dsSelected2 = oService.GetSelected(_requestid, _serviceid, _number);
                for (int ii = _number; ii > 0 && dsSelected2.Tables[0].Rows.Count == 0; ii--)
                    dsSelected2 = oService.GetSelected(_requestid, _serviceid, ii - 1);
                int intAccepted = 0;
                if (dsSelected2.Tables[0].Rows.Count > 0)
                    Int32.TryParse(dsSelected2.Tables[0].Rows[0]["accepted"].ToString(), out intAccepted);
                if (dsSelected2.Tables[0].Rows.Count > 0)
                    intApproved = Int32.Parse(dsSelected2.Tables[0].Rows[0]["approved"].ToString());
                if (boolCancelled == false && boolDecomAuto == true && strDecommission != "")
                {
                    if (boolDecomSubmitted == true)
                        strSummary += "<td width=\"20%\">Decommissioned on " + strDecommission + "</td>";
                    else
                        strSummary += "<td width=\"20%\">To Be Decommissioned on " + strDecommission + "</td>";
                }
                else if (boolCancelled == false && boolRebuild == true && strRebuild != "")
                {
                    if (boolRebuildSubmitted == true)
                        strSummary += "<td width=\"20%\">Rebuilt on " + strRebuild + "</td>";
                    else
                        strSummary += "<td width=\"20%\">To Be Rebuilt on " + strRebuild + "</td>";
                }
                else if (dr["skipped"].ToString() == "1")
                    strSummary += "<td width=\"20%\" nowrap><a href=\"javascript:void(0);\" onclick=\"alert('This service was skipped due to one or more conditions that were configured in the workflow.\\n\\nFor more information, contact the service owner.');\">Skipped</a></td>";
                else if (boolCancelled == false && strCreated == "" && String.IsNullOrEmpty(strStatusService))
                    strSummary += "<td width=\"20%\" nowrap><span class=\"waiting\">Waiting for previous task</span></td>";
                else
                    strSummary += "<td width=\"20%\" nowrap>" + strStatus + "</td>";


                // ************ SLA ************
                double dblQuantity = 1.00;
                if (oService.Get(_serviceid, "quantity_is_device") == "1")
                {
                    DataSet dsTemp = oService.GetSelected(_requestid, _serviceid);
                    if (dsTemp.Tables[0].Rows.Count > 0)
                        dblQuantity = double.Parse(dsTemp.Tables[0].Rows[0]["quantity"].ToString());
                }
                double dblSLA = double.Parse(oService.Get(_serviceid, "sla"));
                string strSLA = "---";
                if (oService.Get(_serviceid, "hide_sla") != "1")
                {
                    dblTotal += dblSLA;
                    if (dblSLA < 1.00)
                    {
                        // Minutes
                        double dblMinutes = (dblSLA * 60.00);
                        dblMinutes = Math.Ceiling(dblMinutes);
                        strSLA = dblMinutes.ToString("0") + " Mins";
                    }
                    else if (dblSLA > 0.00)
                    {
                        double dblTemp = Math.Ceiling(dblSLA);
                        strSLA = dblTemp.ToString("0") + " HRs";
                    }
                }
                else
                    strSLA = "<i>Hidden</i>";
                strSummary += "<td width=\"10%\" align=\"left\" oncontextmenu=\"alert('" + dblSLA.ToString("F") + "');\">" + strSLA + "</td>";


                //// ************ HOURS ************
                //double dblHours = oServiceDetail.GetHours(_serviceid, dblQuantity);
                //string strHours = "---";
                //if (dblHours == 0.00)
                //{
                //    string strTable = oField.GetTableName2(_serviceid);
                //    if (strTable != "")
                //    {
                //        DataSet dsHours = oField.GetTableServiceRequest(strTable, _requestid.ToString(), intItem.ToString());
                //        if (dsHours.Tables[0].Columns.Contains("hours") == true)
                //        {
                //            foreach (DataRow drHours in dsHours.Tables[0].Rows)
                //                dblHours += double.Parse(drHours["hours"].ToString());
                //        }
                //    }
                //}
                //if (dblHours > 0.00)
                //{
                //    dblTotal += dblHours;
                //    if (dblHours < 1.00)
                //    {
                //        // Minutes
                //        double dblMinutes = (dblHours * 60.00);
                //        dblMinutes = Math.Ceiling(dblMinutes);
                //        strHours = dblMinutes.ToString("0") + " Mins";
                //    }
                //    else
                //    {
                //        double dblTemp = Math.Ceiling(dblHours);
                //        strHours = dblTemp.ToString("0") + " HRs";
                //    }
                //    //strHours = dblHours.ToString("F") + " HRs";
                //}
                //strSummary += "<td width=\"10%\" align=\"left\" oncontextmenu=\"alert('" + dblHours.ToString("F") + "');\"\">" + strHours + "</td>";


                // ************ LINKS ************
                strSummary += "<td nowrap>";

                if (boolCancelled == false)
                {
                    //if (boolDecom == true && _submitted == true)
                    if ((boolDecomAuto == true || boolDecomManual == true) && _submitted == true)
                    {
                        if (boolDecomSubmitted == true)
                        {
                            // Past the start date, only show view link
                            strSummary += "[<a href=\"javascript:void(0);\" onclick=\"OpenWindow('PRINTER_FRIENDLY','?page=" + intPage.ToString() + "&rid=" + _requestid.ToString() + "&formid=" + _serviceid.ToString() + "&num=" + dr["number"].ToString() + "&view=true');\" title=\"Click here to view this service\">View</a>]&nbsp;";
                        }
                        else
                        {
                            // Pending the decommission date...show delete, etc...
                            if (_formid > 0)
                                strSummary += "[<a href=\"" + oPage.GetFullLink(intPage) + "?rid=" + _requestid.ToString() + "&formid=" + _serviceid.ToString() + "&num=" + dr["number"].ToString() + "&submitted=" + _formid.ToString() + "\" title=\"Click here to edit this service\">Edit</a>]&nbsp;";
                        }
                        if (boolDecomSubmitted == false || boolDecomManual == true)
                            strSummary += "[<a href=\"" + oPage.GetFullLink(intPage) + "?rid=" + _requestid.ToString() + "&cid=" + _serviceid.ToString() + "&num=" + dr["number"].ToString() + "\" title=\"Click here to cancel this service\" onclick=\"return ConfirmDeleteSubItem('cancel'," + (_formid == 0 ? "this" : "null") + ");\">Cancel</a>]";
                    }
                    else if (boolRebuild && _submitted == true)
                    {
                        if (boolRebuildSubmitted == true)
                        {
                            // Past the start date, only show view link
                            strSummary += "[<a href=\"javascript:void(0);\" onclick=\"OpenWindow('PRINTER_FRIENDLY','?page=" + intPage.ToString() + "&rid=" + _requestid.ToString() + "&formid=" + _serviceid.ToString() + "&num=" + dr["number"].ToString() + "&view=true');\" title=\"Click here to view this service\">View</a>]&nbsp;";
                        }
                        else
                        {
                            // Pending the rebuild date...show delete, etc...
                            if (_formid > 0)
                                strSummary += "[<a href=\"" + oPage.GetFullLink(intPage) + "?rid=" + _requestid.ToString() + "&formid=" + _serviceid.ToString() + "&num=" + dr["number"].ToString() + "&submitted=" + _formid.ToString() + "\" title=\"Click here to edit this service\">Edit</a>]&nbsp;";
                        }
                        if (boolRebuildSubmitted == false)
                            strSummary += "[<a href=\"" + oPage.GetFullLink(intPage) + "?rid=" + _requestid.ToString() + "&cid=" + _serviceid.ToString() + "&num=" + dr["number"].ToString() + "\" title=\"Click here to cancel this service\" onclick=\"return ConfirmDeleteSubItem('cancel'," + (_formid == 0 ? "this" : "null") + ");\">Cancel</a>]";
                    }
                    else if (dr["ondemand"].ToString() == "1")
                    {
                        strSummary += (oService.Get(_serviceid, "disable_customization") == "0" ? "[<a href=\"javascript:void(0);\" onclick=\"OpenWindow('PRINTER_FRIENDLY','?page=" + intPage.ToString() + "&rid=" + _requestid.ToString() + "&formid=" + _serviceid.ToString() + "&num=" + dr["number"].ToString() + "&view=true');\" title=\"Click here to view this service\">View</a>]" : "");
                        if (_submitted == true && intApproved > 0)
                        {
                            Forecast oForecast = new Forecast(intProfile, dsn);
                            ModelsProperties oModelsProperties = new ModelsProperties(intProfile, dsn);
                            Types oType = new Types(intProfile, dsn);
                            DataSet dsService = oForecast.GetAnswerService(_requestid);
                            if (dsService.Tables[0].Rows.Count > 0)
                            {
                                int intAnswer = Int32.Parse(dsService.Tables[0].Rows[0]["id"].ToString());
                                int intModel = Int32.Parse(dsService.Tables[0].Rows[0]["modelid"].ToString());
                                int intType = oModelsProperties.GetType(intModel);
                                string strExecute = oType.Get(intType, "forecast_execution_path");
                                if (strExecute != "")
                                    strSummary += "[<a href=\"javascript:void(0);\" title=\"Click here to execute this service\" onclick=\"OpenWindow('FORECAST_EXECUTE','" + strExecute + "?id=" + intAnswer.ToString() + "');\">Execute</a>]";
                                else
                                    strSummary += "[<a href=\"javascript:void(0);\" title=\"Click here to execute this service\" onclick=\"alert('Execution has not been configured for asset type " + oType.Get(intType, "name") + "');\">Execute</a>]";
                                if (strSummaryExecute == "")
                                    strSummaryExecute += "<tr><td colspan=\"7\" align=\"right\"><table><tr><td>&nbsp;</td><td rowspan=\"2\"><img src=\"/images/right_up.gif\" border=\"0\" align=\"absmiddle\"/></td><td rowspan=\"2\"><img src=\"/images/spacer.gif\" border=\"0\" width=\"5\" height=\"1\"/></td></tr><tr><td class=\"header\"><img src=\"/images/bigAlert.gif\" border=\"0\" align=\"absmiddle\"/> Don't forget to click the execute button to start your build!</td></tr></table></td><td></td></tr>";
                            }
                            else
                                strSummary += "[<a title=\"Click here to execute this service\" disabled=\"disabled\">Execute</a>]";
                        }
                        else if (_submitted == true && intApproved < 0)
                        {
                            // Rejected
                            strSummary += "[<a href=\"" + oPage.GetFullLink(intPage) + "?rid=" + _requestid.ToString() + "&formid=" + _serviceid.ToString() + "&num=" + dr["number"].ToString() + "&denied=true\" title=\"Click here to update this service\">Change</a>]";
                        }
                        else
                            strSummary += "[<a title=\"Click here to execute this service\" disabled=\"disabled\">Execute</a>]";
                    }
                    else if (GetStatusPercent(_requestid, intItem, _serviceid, _number) == 100.00 || intProfile != intRequestor)
                    {
                        // Has been completed...
                        if (_formid > 0)
                            strSummary += (oService.Get(_serviceid, "disable_customization") == "0" ? "[<a href=\"javascript:void(0);\" onclick=\"OpenWindow('PRINTER_FRIENDLY','?page=" + intPage.ToString() + "&rid=" + _requestid.ToString() + "&formid=" + _serviceid.ToString() + "&num=" + dr["number"].ToString() + "&view=true');\" title=\"Click here to view this service\">View</a>]" : "");
                        else
                            strSummary += "[<a disabled=\"disabled\" oncontextmenu=\"alert('Workflow items cannot be viewed');\">View</a>]&nbsp;";
                        //if (intApproved < 0 || intAccepted < 0)
                        if (intApproved < 0)
                            strSummary += "[<a href=\"" + oPage.GetFullLink(intPage) + "?rid=" + _requestid.ToString() + "&formid=" + _serviceid.ToString() + "&num=" + dr["number"].ToString() + "&denied=true\" title=\"Click here to update this service\">Change</a>]";
                        else if (_status == (int)ResourceRequestStatus.AwaitingResponse) //Awaiting Client Response
                            strSummary += "[<a href=\"" + oPage.GetFullLink(intPage) + "?rid=" + _requestid.ToString() + "&formid=" + _serviceid.ToString() + "&num=" + dr["number"].ToString() + "&returned=true\" title=\"Click here to update this service\">Reply</a>]";
                    }
                    else
                    {
                        // Incomplete.. 
                        if (_submitted == false)
                        {
                            if (_formid > 0)
                            {
                                if (oService.Get(_serviceid, "disable_customization") == "0")
                                    strSummary += "[<a href=\"" + oPage.GetFullLink(intPage) + "?rid=" + _requestid.ToString() + "&formid=" + _serviceid.ToString() + "&num=" + dr["number"].ToString() + "\" title=\"Click here to edit this service\">Edit</a>]&nbsp;";
                                strSummary += "[<a href=\"" + oPage.GetFullLink(intPage) + "?rid=" + _requestid.ToString() + "&did=" + _serviceid.ToString() + "&didn=" + dr["number"].ToString() + "\" title=\"Click here to delete this service\" onclick=\"return ConfirmDeleteSubItem('delete',null);\">Delete</a>]";
                            }
                        }
                        else
                        {
                            //if (intApproved < 0 || intAccepted < 0)
                            if (intApproved < 0)
                                strSummary += "[<a href=\"" + oPage.GetFullLink(intPage) + "?rid=" + _requestid.ToString() + "&formid=" + _serviceid.ToString() + "&num=" + dr["number"].ToString() + "&denied=true\" title=\"Click here to update this service\">Change</a>]";
                            else if (_status == (int)ResourceRequestStatus.AwaitingResponse) //Awaiting Client Response
                                strSummary += "[<a href=\"" + oPage.GetFullLink(intPage) + "?rid=" + _requestid.ToString() + "&formid=" + _serviceid.ToString() + "&num=" + dr["number"].ToString() + "&returned=true\" title=\"Click here to update this service\">Reply</a>]";
                            else if (intAccepted >= 0 && intManagerApproved >= 0)
                            {
                                if (_formid > 0 && oService.Get(_serviceid, "disable_customization") == "0")
                                    strSummary += "[<a href=\"javascript:void(0);\" onclick=\"OpenWindow('PRINTER_FRIENDLY','?page=" + intPage.ToString() + "&rid=" + _requestid.ToString() + "&formid=" + _serviceid.ToString() + "&num=" + dr["number"].ToString() + "&view=true');\" title=\"Click here to view this service\">View</a>]&nbsp;";
                                else
                                    strSummary += "[<a disabled=\"disabled\" oncontextmenu=\"alert('Workflow items cannot be viewed');\">View</a>]&nbsp;";

                                if (String.IsNullOrEmpty(Request.QueryString["approve"]) && strCreated != "")
                                    strSummary += "[<a href=\"" + oPage.GetFullLink(intPage) + "?rid=" + _requestid.ToString() + "&cid=" + _serviceid.ToString() + "&num=" + dr["number"].ToString() + "\" title=\"Click here to cancel this service\" onclick=\"return ConfirmDeleteSubItem('cancel'," + (_formid == 0 ? "this" : "null") + ");\">Cancel</a>]";
                            }
                        }
                    }
                }
                strSummary += "</td>";
                strSummary += "<td>&nbsp;&nbsp;&nbsp;</td>";
                strSummary += "</tr>";
            }
            lblHours.Text = dblTotal.ToString("F") + " HRs";
            if (strSummary != "")
            {
                strSummary = "<tr bgcolor=\"#EEEEEE\"><td width=\"10%\"><b><u>RequestID:</u></b></td><td width=\"40%\"><b><u>Service:</u></b></td><td width=\"20%\"><b><u>Owner(s):</u></b></td><td width=\"20%\"><b><u>Status:</u></b></td><td width=\"10%\" align=\"left\"><b><u>SLA:</u></b></td><td></td><td></td></tr>" + strSummary + strSummaryExecute;
                strSummary = "<table width=\"100%\" border=\"0\" cellpadding=\"4\" cellspacing=\"0\" style=\"border:solid 1px #CCCCCC\">" + strSummary + "</table>";
            }
            else
            {
                strSummary = "<img src=\"/images/circleAlert.gif\" border=\"0\" align=\"absmiddle\"> There are no services associated with this request";
                btnComplete.Enabled = false;
            }
            return boolManagerApprovalRequired;
        }
        protected double GetStatusPercent(int _requestid, int _itemid, int _serviceid, int _number)
        {
            double dblAllocated = 0.00;
            double dblUsed = 0.00;
            int intAssigned = 0;
            bool boolAutomated = (oService.Get(_serviceid, "automate") == "1");
            if (boolAutomated == false)

            {   DataSet dsReqForm=oRequestItem.GetForm(_requestid, _serviceid,_itemid,_number);
                if (dsReqForm.Tables[0].Rows.Count > 0)
                    boolAutomated = (dsReqForm.Tables[0].Rows[0]["automated"].ToString() == "1" ? true : false);
            }
            if (boolAutomated == true)
                return 100.00;
            else
            {
                DataSet dsResource = oResourceRequest.GetRequestService(_requestid, _serviceid, _number);
                if (dsResource.Tables[0].Rows.Count > 0)
                {
                    int intResourceWorkflow = Int32.Parse(dsResource.Tables[0].Rows[0]["id"].ToString());
                    int intStatus = Int32.Parse(dsResource.Tables[0].Rows[0]["status"].ToString());
                    if (intStatus == 3)
                        return 100.00;
                    else
                    {
                        foreach (DataRow drResource in dsResource.Tables[0].Rows)
                        {
                            intResourceWorkflow = Int32.Parse(drResource["id"].ToString());
                            dblAllocated += double.Parse(drResource["allocated"].ToString());
                            double dblHours = oResourceRequest.GetWorkflowUsed(intResourceWorkflow);
                            dblUsed += dblHours;
                            intAssigned += Int32.Parse(drResource["userid"].ToString());
                        }
                    }
                }
                if (intAssigned == 0)
                    return -1.00;
                else
                {
                    double dblProgress = 0.00;
                    if (dblAllocated > 0.00)
                        dblProgress = dblUsed / dblAllocated * 100;
                    return dblProgress;
                }
            }
        }
        protected void btnFavoriteAdd_Click(Object Sender, EventArgs e)
        {
            int intRequest = Int32.Parse(lblRequest.Text);
            int intService = Int32.Parse(lblService.Text);
            oService.AddFavorite(intProfile, intService);
            Response.Redirect(oPage.GetFullLink(intPage) + "?rid=" + intRequest.ToString());
        }
        protected void btnFavoriteDelete_Click(Object Sender, EventArgs e)
        {
            int intRequest = Int32.Parse(lblRequest.Text);
            int intService = Int32.Parse(lblService.Text);
            oService.DeleteFavorite(intProfile, intService);
            Response.Redirect(oPage.GetFullLink(intPage) + "?rid=" + intRequest.ToString());
        }

        protected void btnTitle_Click(object sender, EventArgs e)
        {
            int intRequest = Int32.Parse(lblRequest.Text);
            oServiceRequest.Update(intRequest, txtTitle.Text);
            Response.Redirect(oPage.GetFullLink(intPage) + "?rid=" + intRequest.ToString());
        }
    }
}