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
using Microsoft.ApplicationBlocks.Data;

namespace NCC.ClearView.Presentation.Web
{
    public partial class resource : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
        private string dsnServiceEditor = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["ServiceEditorDSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intViewPage = Int32.Parse(ConfigurationManager.AppSettings["ViewWorkload"]);
        protected int intImplementorDistributed = Int32.Parse(ConfigurationManager.AppSettings["ITEMID_IMPLEMENTOR_DISTRIBUTED"]);
        protected int intImplementorMidrange = Int32.Parse(ConfigurationManager.AppSettings["ITEMID_IMPLEMENTOR_MIDRANGE"]);

        protected int intDataPointAvailableService = Int32.Parse(ConfigurationManager.AppSettings["DATAPOINT_AVAILABLE_SERVICE"]);
        protected DataPoint oDataPoint;
        protected Users oUser;
        protected ResourceRequest oResourceRequest;
        protected Requests oRequest;
        protected Services oService;
        protected ServiceDetails oServiceDetail;
        protected ServiceRequests oServiceRequest;
        protected Applications oApplication;
        protected RequestItems oRequestItem;
        protected Functions oFunction;
        protected StatusLevels oStatusLevel;
        protected ProjectNumber oProjectNumber;
        protected Pages oPage;
        protected Log oLog;
        protected int intProfile = 0;
        protected int intApplication = 0;
        protected int intResource = 0;
        protected string strMenuTab1 = "";
        protected string strOriginal = "";
        protected string strRequestor = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            AuthenticateUser();
            if (Request.Cookies["profileid"] != null && Request.Cookies["profileid"].Value != "")
                intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            if (Request.QueryString["applicationid"] != null && Request.QueryString["applicationid"] != "")
                intApplication = Int32.Parse(Request.QueryString["applicationid"]);
            if (Request.Cookies["application"] != null && Request.Cookies["application"].Value != "")
                intApplication = Int32.Parse(Request.Cookies["application"].Value);
            oDataPoint = new DataPoint(intProfile, dsn);
            oUser = new Users(intProfile, dsn);
            oResourceRequest = new ResourceRequest(intProfile, dsn);
            oRequest = new Requests(intProfile, dsn);
            oService = new Services(intProfile, dsn);
            oServiceDetail = new ServiceDetails(intProfile, dsn);
            oServiceRequest = new ServiceRequests(intProfile, dsn);
            oApplication = new Applications(intProfile, dsn);
            oRequestItem = new RequestItems(intProfile, dsn);
            oFunction = new Functions(intProfile, dsn, intEnvironment);
            oStatusLevel = new StatusLevels();
            oProjectNumber = new ProjectNumber(intProfile, dsn);
            oPage = new Pages(intProfile, dsn);
            oLog = new Log(intProfile, dsn);
            if (oUser.IsAdmin(intProfile) == true || (oDataPoint.GetPagePermission(intApplication, "SERVICE") == true || intDataPointAvailableService == 1))
            {
                panAllow.Visible = true;
                if (Request.QueryString["save"] != null)
                    panSave.Visible = true;
                if (Request.QueryString["clear"] != null)
                    panClear.Visible = true;
                if (Request.QueryString["error"] != null)
                {
                    panError.Visible = true;
                }
                if (Request.QueryString["close"] != null)
                    Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "close", "<script type=\"text/javascript\">window.close();<" + "/" + "script>");
                else if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
                {
                    string strID = oFunction.decryptQueryString(Request.QueryString["id"]);
                    intResource = Int32.Parse(strID);
                    DataSet ds = oDataPoint.GetServiceRequestResource(intResource);
                    if (ds.Tables[0].Rows.Count == 1)
                    {
                        // Load General Information
                       
                        //string strHeader = intResource.ToString();
                        string strHeader = ds.Tables[0].Rows[0]["requestid"].ToString() 
                                +"-" +(ds.Tables[0].Rows[0]["ServiceId"]!=DBNull.Value?ds.Tables[0].Rows[0]["ServiceId"].ToString():"0") 
                                +"-"+(ds.Tables[0].Rows[0]["number"] != DBNull.Value ? ds.Tables[0].Rows[0]["number"].ToString() : "0");

                        lblRequestID.Text =   strHeader;
                        lblRequestID.ToolTip = "Resource Request :" + intResource.ToString();

                        lblHeader.Text = "&quot;" + strHeader + "&quot;";
                        Master.Page.Title = "DataPoint | Request (" + strHeader + ")";
                        lblHeaderSub.Text = "Provides all the information about a resource request...";
                        int intMenuTab = 0;
                        if (Request.QueryString["menu_tab"] != null && Request.QueryString["menu_tab"] != "")
                            intMenuTab = Int32.Parse(Request.QueryString["menu_tab"]);
                        Tab oTab = new Tab(hdnTab.ClientID, intMenuTab, "divMenu1", true, false);

                        oTab.AddTab("Submitted Information", "");
                        oTab.AddTab("Workflow / History", "");
                        oTab.AddTab("Resource(s) Involvement", "");
                        if (oUser.IsAdmin(intProfile) == true)
                        {
                            trAdmin.Visible = true;
                            Variables oVariable = new Variables(intEnvironment);
                            txtUser.Attributes.Add("onkeyup", "return AJAXTextBoxGet(this,'300','195','" + divUser.ClientID + "','" + lstUser.ClientID + "','" + hdnUser.ClientID + "','" + oVariable.URL() + "/frame/users.aspx',2);");
                            lstUser.Attributes.Add("ondblclick", "AJAXClickRow();");
                            oTab.AddTab("Resource Detail", "");
                            divDetail.Visible = true;
                        }
                        
                        if (!IsPostBack)
                        {
                            strOriginal = oResourceRequest.GetSummary(intResource, 0, intEnvironment, dsnServiceEditor, dsnAsset, dsnIP);

                            bool boolDeleted = (ds.Tables[0].Rows[0]["deleted"].ToString() != "0");
                            panDeleted.Visible = boolDeleted;
                            int intRequest = Int32.Parse(ds.Tables[0].Rows[0]["requestid"].ToString());
                            int intRequestor = oRequest.GetUser(intRequest);
                            strRequestor = "<a href=\"javascript:void(0);\" class=\"lookup\" onclick=\"OpenWindow('PROFILE','?userid=" + intRequestor.ToString() + "');\">" + oUser.GetFullName(intRequestor) + " [" + oUser.GetName(intRequestor) + "]</a>";
                            int intItem = Int32.Parse(ds.Tables[0].Rows[0]["itemid"].ToString());
                            int intApp = oRequestItem.GetItemApplication(intItem);
                            int intService = Int32.Parse(ds.Tables[0].Rows[0]["serviceid"].ToString());
                            int intNumber = Int32.Parse(ds.Tables[0].Rows[0]["number"].ToString());
                            // General Information
                            oDataPoint.LoadTextBox(txtRequest, intProfile, null, "/datapoint/service/request.aspx?t=id&q=" + oFunction.encryptQueryString("CVT" + intRequest.ToString()) + "&id=" + oFunction.encryptQueryString(intRequest.ToString()), lblRequest, fldRequest, "RESOURCE_REQUEST", "CVT" + ds.Tables[0].Rows[0]["requestid"].ToString(), "", true, false);
                            oDataPoint.LoadTextBox(txtCreated, intProfile, null, "", lblCreated, fldCreated, "RESOURCE_CREATED", ds.Tables[0].Rows[0]["created"].ToString(), "", true, false);
                            oDataPoint.LoadTextBox(txtDepartment, intProfile, null, "", lblDepartment, fldDepartment, "RESOURCE_DEPARTMENT", ds.Tables[0].Rows[0]["department"].ToString(), "", true, false);
                            oDataPoint.LoadTextBox(txtService, intProfile, null, "", lblService, fldService, "RESOURCE_SERVICE", oService.GetName(intService), "", true, false);
                            string strOwner = "";
                            DataSet dsOwner = oService.GetUser(intService, -1);  // Service Owners
                            foreach (DataRow drOwner in dsOwner.Tables[0].Rows)
                            {
                                if (strOwner != "")
                                    strOwner += ", ";
                                int intOwner = Int32.Parse(drOwner["userid"].ToString());
                                strOwner += "<span class=\"required\">*</span> <a href=\"javascript:void(0);\" class=\"lookup\" onclick=\"OpenWindow('PROFILE','?userid=" + intOwner.ToString() + "');\">" + oUser.GetFullName(intOwner) + " [" + oUser.GetName(intOwner) + "]</a>";
                            }
                            //strOwner = "<b>" + strOwner + "</b>";
                            DataSet dsManager = oService.GetUser(intService, 1);  // Managers
                            foreach (DataRow drManager in dsManager.Tables[0].Rows)
                            {
                                if (strOwner != "")
                                    strOwner += ", ";
                                int intManager = Int32.Parse(drManager["userid"].ToString());
                                strOwner += "<a href=\"javascript:void(0);\" class=\"lookup\" onclick=\"OpenWindow('PROFILE','?userid=" + intManager.ToString() + "');\">" + oUser.GetFullName(intManager) + " [" + oUser.GetName(intManager) + "]</a>";
                            }
                            oDataPoint.LoadTextBox(txtOwner, intProfile, null, "", lblOwner, fldOwner, "RESOURCE_OWNER", strOwner, "", true, false);
                            oDataPoint.LoadTextBox(txtName, intProfile, null, "", lblName, fldName, "RESOURCE_NAME", ds.Tables[0].Rows[0]["name"].ToString(), "", false, true);
                            oDataPoint.LoadTextBox(txtDevices, intProfile, null, "", lblDevices, fldDevices, "RESOURCE_DEVICES", ds.Tables[0].Rows[0]["devices"].ToString(), "", false, true);
                            oDataPoint.LoadTextBox(txtAllocated, intProfile, null, "", lblAllocated, fldAllocated, "RESOURCE_ALLOCATED", ds.Tables[0].Rows[0]["allocated"].ToString(), "", false, true);
                            if (txtDevices.Visible == true && oService.Get(intService, "disable_hours") != "1")
                            {
                                txtAllocated.Enabled = false;
                                txtAllocated.ToolTip = intService.ToString();
                                panDynamic.Visible = true;
                            }
                            int intAccepted = Int32.Parse(ds.Tables[0].Rows[0]["accepted"].ToString());
                            oDataPoint.LoadDropDown(ddlAccepted, intProfile, null, "", lblAccepted, fldAccepted, "REQUEST_ACCEPTED", "name", "id", SqlHelper.ExecuteDataset(dsn, CommandType.Text, "SELECT -1 AS id, 'No' AS name UNION ALL SELECT 0 AS id, 'Pending' AS name UNION ALL SELECT 1 AS id, 'Yes' AS name"), intAccepted, false, false, true);
                            oDataPoint.LoadDropDown(ddlStatus, intProfile, null, "", lblStatus, fldStatus, "REQUEST_STATUS", "name", "id", SqlHelper.ExecuteDataset(dsn, CommandType.Text, oStatusLevel.List()), Int32.Parse(ds.Tables[0].Rows[0]["status"].ToString()), false, false, true);
                            oDataPoint.LoadTextBox(txtReason, intProfile, null, "", lblReason, fldReason, "RESOURCE_REASON", ds.Tables[0].Rows[0]["reason"].ToString(), "", false, false);
                            int intAssigned = Int32.Parse(ds.Tables[0].Rows[0]["assignedby"].ToString());
                            oDataPoint.LoadDropDownAJAX(txtAssignedBy, hdnAssignedBy, divAssignedBy, lstAssignedBy, intEnvironment, intProfile, null, "", lblAssignedBy, fldAssignedBy, "RESOURCE_ASSIGNED", intAssigned, (intAssigned > 0 ? oUser.GetFullName(intAssigned) + " (" + oUser.GetName(intAssigned) + ")" : ""), "/frame/users.aspx", "", false, false);
                            oDataPoint.LoadTextBox(txtAssignedOn, intProfile, null, "", lblAssignedOn, fldAssignedOn, "RESOURCE_ASSIGNED", ds.Tables[0].Rows[0]["assigned"].ToString(), "", true, false);
                            btnAssign.Visible = oUser.IsAdmin(intProfile);
                            btnAssign.Enabled = (intAssigned > 0);

                            ucWorkflow.RequestID = intRequest;
                            ucWorkflow.ServiceID = intService;
                            ucWorkflow.Number = intNumber;

                            ucResourceInvolvement.ResourceRequestId = intResource;
                            //if (ds.Tables[1].Rows.Count > 0)
                            //{
                            //    // Resource Assignment(s)
                            //    if (ds.Tables[2].Rows.Count > 0)
                            //        ds.Relations.Add("relationship", ds.Tables[1].Columns["id"], ds.Tables[2].Columns["id"]);
                            //    rptAssignments.DataSource = ds.Tables[1];
                            //    rptAssignments.DataBind();
                            //    foreach (RepeaterItem ri in rptAssignments.Items)
                            //    {
                            //        Label lblStatusR = (Label)ri.FindControl("lblStatusR");
                            //        int intStatus = Int32.Parse(lblStatusR.Text);
                            //        lblStatusR.Text = oStatusLevel.HTML(intStatus);
                            //        Label lblProgress = (Label)ri.FindControl("lblProgress");
                            //        double dblProgress = double.Parse(lblProgress.Text);
                            //        if (intStatus == 3)
                            //            dblProgress = 100.00;
                            //        if (intStatus < 1 || intStatus > 2)
                            //            lblProgress.Text = "---";
                            //        else
                            //        {
                            //            try
                            //            {
                            //                lblProgress.Text = oServiceRequest.GetStatusBar(dblProgress, "100", "12", true);
                            //            }
                            //            catch
                            //            {
                            //                lblProgress.Text = oServiceRequest.GetStatusBar(0.00, "100", "12", true);
                            //            }
                            //        }

                            //        Panel panDelete = (Panel)ri.FindControl("panDelete");
                            //        if (panDelete.ToolTip == "1")
                            //            panDelete.Visible = true;
                            //        else
                            //        {
                            //            Panel panEdit = (Panel)ri.FindControl("panEdit");
                            //            panEdit.Visible = true;
                            //            Button btnLogin = (Button)ri.FindControl("btnLogin");
                            //            btnLogin.Attributes.Add("onclick", "return OpenNewWindowMenu('/frame/resource_request.aspx?rrid=" + btnLogin.CommandArgument + "', '800', '600');");
                            //            Button btnEdit = (Button)ri.FindControl("btnEdit");
                            //            if (oUser.IsAdmin(intProfile) || oService.IsManager(intService, intProfile))
                            //                btnEdit.Attributes.Add("onclick", "return OpenNewWindowMenu('/datapoint/service/manager.aspx?id=" + btnEdit.CommandArgument + "', '800', '600');");
                            //            else
                            //                btnEdit.Enabled = false;
                            //            LinkButton btnMore = (LinkButton)ri.FindControl("btnMore");
                            //            btnMore.Attributes.Add("onclick", "ShowHideDiv2('div_" + btnMore.CommandArgument + "');return false;");
                            //            if (ds.Tables[2].Rows.Count > 0)
                            //                btnMore.Visible = true;
                            //        }
                            //    }
                            //}
                            //lblAssignments.Visible = (rptAssignments.Items.Count == 0);


                            // This should be the last tab to load
                            DataSet dsResults = oRequest.GetResult(intRequest, intItem, intNumber);
                            if (dsResults.Tables[0].Rows.Count > 0)
                            {
                                // Request Results
                                oTab.AddTab("Request Results", "");
                                divResults.Visible = true;
                                rptResults.DataSource = dsResults;
                                rptResults.DataBind();
                            }
                            lblResults.Visible = (rptResults.Items.Count == 0);

                            strMenuTab1 = oTab.GetTabs();
                        }
                    }
                    else
                        Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "close", "<script type=\"text/javascript\">window.close();<" + "/" + "script>");
                }
                else
                    Response.Redirect("/datapoint/service/datapoint_service_search.aspx");
                btnAssign.Attributes.Add("onclick", "return confirm('Are you sure you want to put this request back in queue for assignment?');");
                btnClose.Attributes.Add("onclick", "window.close();return false;");
                btnPrint.Attributes.Add("onclick", "window.print();return false;");
                btnSave.Attributes.Add("onclick", oDataPoint.LoadValidation());
                btnSaveClose.Attributes.Add("onclick", oDataPoint.LoadValidation());
            }
            else
                panDenied.Visible = true;
        }
        protected void btnNew_Click(Object Sender, EventArgs e)
        {
            Response.Redirect("/datapoint/service/datapoint_service_search.aspx");
        }
        protected void btnAssign_Click(Object Sender, EventArgs e)
        {
            oResourceRequest.UpdateAssignedByReset(intResource);
            Response.Redirect(Request.Path + "?id=" + oFunction.encryptQueryString(intResource.ToString()) + "&clear=true");
        }
        protected void btnSave_Click(Object Sender, EventArgs e)
        {
            int intSave = Save();
            Response.Redirect(Request.Path + "?id=" + oFunction.encryptQueryString(intResource.ToString()) + "&" + (intSave > 0 ? "save" : "error") + "=" + intSave.ToString());
        }
        protected void btnSaveClose_Click(Object Sender, EventArgs e)
        {
            int intSave = Save();
            Response.Redirect(Request.Path + "?id=" + oFunction.encryptQueryString(intResource.ToString()) + "&" + (intSave > 0 ? "close" : "error") + "=" + intSave.ToString());
        }
        protected int Save()
        {
            int intReturn = 1;
            oResourceRequest.UpdateName(intResource, txtName.Text);
            double dblAllocated = 0.00;
            if (panDynamic.Visible == false)
                dblAllocated = double.Parse(txtAllocated.Text);
            else
            {
                double dblQuantity = double.Parse(txtDevices.Text);
                dblAllocated = oServiceDetail.GetHours(Int32.Parse(txtAllocated.ToolTip), dblQuantity);
            }
            oResourceRequest.UpdateDevices(intResource, Int32.Parse(txtDevices.Text), dblAllocated);
            oResourceRequest.UpdateAccepted(intResource, Int32.Parse(ddlAccepted.SelectedItem.Value));
            oResourceRequest.UpdateStatusOverall(intResource, Int32.Parse(ddlStatus.SelectedItem.Value));
            oResourceRequest.UpdateReason(intResource, txtReason.Text);
            if (Request.Form[hdnAssignedBy.UniqueID] != "" && Request.Form[hdnAssignedBy.UniqueID] != "0")
                oResourceRequest.UpdateAssignedBy(intResource, Int32.Parse(Request.Form[hdnAssignedBy.UniqueID]));

            if (trAdmin.Visible == true)
            {
                int intUser = 0;
                if (Int32.TryParse(Request.Form[hdnUser.UniqueID], out intUser) == true)
                {
                    // Assign resource
                    DataSet ds = oResourceRequest.Get(intResource);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        int intRequest = Int32.Parse(ds.Tables[0].Rows[0]["requestid"].ToString());
                        int intItem = Int32.Parse(ds.Tables[0].Rows[0]["itemid"].ToString());
                        int intService = Int32.Parse(ds.Tables[0].Rows[0]["serviceid"].ToString());
                        int intNumber = Int32.Parse(ds.Tables[0].Rows[0]["number"].ToString());
                        string strCVT = "CVT" + intRequest.ToString() + "-" + intService.ToString() + "-" + intNumber.ToString();
                        string strService = oService.GetName(intService);
                        if (intService == 0)
                            strService = oRequestItem.GetItemName(intItem);
                        int intApp = oRequestItem.GetItemApplication(intItem);
                        int intProject = Int32.Parse(oRequest.Get(intRequest, "projectid"));
                        Variables oVariable = new Variables(intEnvironment);

                        DataSet dsWorkflow = oResourceRequest.GetWorkflowsParent(intResource);
                        if (dsWorkflow.Tables[0].Rows.Count == 0)
                        {
                            // New Assignment
                            Projects oProject = new Projects(intProfile, dsn);
                            ProjectsPending oProjectsPending = new ProjectsPending(intProfile, dsn, intEnvironment);
                            int intRequester = Int32.Parse(oRequest.Get(intRequest, "userid"));
                            int intPC = 0;
                            int intIE = 0;
                            if (intProject > 0)
                            {
                                if (oProject.Get(intProject, "lead") != "")
                                    intPC = Int32.Parse(oProject.Get(intProject, "lead"));
                                if (oProject.Get(intProject, "engineer") != "")
                                    intIE = Int32.Parse(oProject.Get(intProject, "engineer"));
                            }
                            else
                            {
                                try
                                {
                                    intPC = Int32.Parse(oProjectsPending.GetRequest(intRequest, "lead"));
                                    intIE = Int32.Parse(oProjectsPending.GetRequest(intRequest, "engineer"));
                                }
                                catch { }
                            }
                            string strCC = "";
                            if (intPC > 0)
                                strCC += oUser.GetName(intPC) + ";";
                            if (intIE > 0)
                                strCC += oUser.GetName(intIE) + ";";

                            bool boolSolo = (oResourceRequest.Get(intResource, "solo") == "1");
                            int intAssigned = 0;
                            bool boolRejected = (oResourceRequest.Get(intResource, "accepted") == "-1");
                            intAssigned = intUser;
                            oResourceRequest.UpdateAccepted(intResource, 1);

                            oProject.Update(intProject, 2);
                            int intResourceWorkflow = oResourceRequest.AddWorkflow(intResource, 0, oResourceRequest.Get(intResource, "name"), intAssigned, 1, 1.00, 2, 0);
                            oLog.AddEvent(intRequest.ToString(), strCVT, "Request assigned by " + oUser.GetFullNameWithLanID(intProfile) + " (ADMIN) to " + oUser.GetFullNameWithLanID(intAssigned), LoggingType.Debug);
                            oResourceRequest.UpdateAssignedBy(intResource, intProfile);
                            ProjectRequest oProjectRequest = new ProjectRequest(intProfile, dsn);
                            string strDefault = oUser.GetApplicationUrl(intAssigned, intViewPage);
                            string strNotify = "";
                            string strSpacerRow = "<tr><td colspan=\"3\"><img src=\"" + oVariable.ImageURL() + "/images/spacer.gif\" border=\"0\" width=\"1\" height=\"7\" /></td></tr>";
                            oResourceRequest.UpdateDevices(intResource, 1, 1.00);
                            if (boolSolo == true)
                            {
                                oResourceRequest.UpdateStatusOverall(intResource, 2);
                                if (oApplication.Get(intApp, "tpm") != "1" && oProject.Get(intProject, "number") == "")
                                    oProject.Update(intProject, oProjectNumber.New());
                                if (chkEmail.Checked == false)
                                {
                                    if (intItem != intImplementorDistributed && intItem != intImplementorMidrange)
                                    {
                                        string strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_REQUEST_ASSIGNMENT");
                                        //if (oProject.Get(intProject, "number").StartsWith("CV") == false)
                                        //    strNotify = "<p><span style=\"color:#0000FF\"><b>PROJECT COORDINATOR:</b> Please allocate the hours listed above for each resource in Clarity.</span></p>";
                                        if (strDefault == "")
                                            oFunction.SendEmail("Request Assignment: " + strService, oUser.GetName(intAssigned), "", strEMailIdsBCC, "Request Assignment: " + strService, "<p><b>The following request has been assigned to you by " + oUser.GetFullName(intProfile) + "</b><p><p>" + oResourceRequest.GetWorkflowSummary(intResourceWorkflow, intEnvironment, dsnServiceEditor, dsnAsset, dsnIP) + "</p>", true, false);
                                        else
                                        {
                                            if (intProject > 0)
                                                oFunction.SendEmail("Request Assignment: " + strService, oUser.GetName(intAssigned), "", strEMailIdsBCC, "Request Assignment: " + strService, "<p><b>The following request has been assigned to you by " + oUser.GetFullName(intProfile) + "</b><p><p>" + oResourceRequest.GetWorkflowSummary(intResourceWorkflow, intEnvironment, dsnServiceEditor, dsnAsset, dsnIP) + "</p><p><a href=\"" + oVariable.URL() + "/redirect.aspx?referrer=/" + strDefault + oPage.GetFullLink(intViewPage) + "?pid=" + intProject.ToString() + "\" target=\"_blank\">Click here to review your new assignment.</a></p>", true, false);
                                            else
                                                oFunction.SendEmail("Request Assignment: " + strService, oUser.GetName(intAssigned), "", strEMailIdsBCC, "Request Assignment: " + strService, "<p><b>The following request has been assigned to you by " + oUser.GetFullName(intProfile) + "</b><p><p>" + oResourceRequest.GetWorkflowSummary(intResourceWorkflow, intEnvironment, dsnServiceEditor, dsnAsset, dsnIP) + "</p><p><a href=\"" + oVariable.URL() + "/redirect.aspx?referrer=/frame/resource_request.aspx?rrid=" + intResourceWorkflow.ToString() + "\" target=\"_blank\">Click here to review your new assignment.</a></p>", true, false);
                                        }
                                        string strActivity = "<tr><td><b>Resource:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>" + oUser.GetFullName(intAssigned) + "</td></tr>";
                                        strActivity += strSpacerRow;
                                        strActivity += "<tr><td><b>Service:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>" + strService + "</td></tr>";
                                        strActivity = "<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"" + oVariable.DefaultFontStyle() + "\">" + strActivity + "</table>";
                                        string strDeliverable = oApplication.Get(intApp, "deliverables_doc");
                                        if (strDeliverable.Trim() != "")
                                            strDeliverable = "<p><a href=\"" + oVariable.URL() + strDeliverable + "\">Click here to view the service deliverables</a></p>";
                                        if (oService.Get(intService, "notify_client") != "0")
                                            oFunction.SendEmail("Request Assignment: " + strService, oUser.GetName(intRequester), strCC, strEMailIdsBCC, "Request Assignment: " + strService, "<p><b>A resource has been assigned to the following request...</b><p><p>" + oResourceRequest.GetWorkflowSummary(intResourceWorkflow, intEnvironment, dsnServiceEditor, dsnAsset, dsnIP) + "</p><p>" + strActivity + "</p>" + strDeliverable + strNotify, true, false);
                                    }
                                }
                            }
                            else
                            {
                                // ADD PM
                                if (chkEmail.Checked == false)
                                {
                                    string strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_REQUEST_ASSIGNMENT");
                                    if (strDefault == "")
                                        oFunction.SendEmail("Request Assignment", oUser.GetName(intAssigned), "", strEMailIdsBCC, "Request Assignment", "<p><b>The following project has been assigned to you by " + oUser.GetFullName(intProfile) + "</b><p><p>" + oProjectRequest.GetBody(intRequest, intEnvironment, true) + "</p>", true, false);
                                    else
                                    {
                                        if (intProject > 0)
                                            oFunction.SendEmail("Request Assignment", oUser.GetName(intAssigned), "", strEMailIdsBCC, "Request Assignment", "<p><b>The following project has been assigned to you by " + oUser.GetFullName(intProfile) + "</b><p><p>" + oProjectRequest.GetBody(intRequest, intEnvironment, true) + "</p><p><a href=\"" + oVariable.URL() + "/redirect.aspx?referrer=/" + strDefault + oPage.GetFullLink(intViewPage) + "?pid=" + intProject.ToString() + "\" target=\"_blank\">Click here to review your new assignment.</a></p>", true, false);
                                        else
                                            oFunction.SendEmail("Request Assignment", oUser.GetName(intAssigned), "", strEMailIdsBCC, "Request Assignment", "<p><b>The following project has been assigned to you by " + oUser.GetFullName(intProfile) + "</b><p><p>" + oProjectRequest.GetBody(intRequest, intEnvironment, true) + "</p><p><a href=\"" + oVariable.URL() + "/redirect.aspx?referrer=/frame/resource_request.aspx?rrid=" + intResourceWorkflow.ToString() + "\" target=\"_blank\">Click here to review your new assignment.</a></p>", true, false);
                                    }
                                    string strExecutive = oProject.Get(intProject, "executive");
                                    string strWorking = oProject.Get(intProject, "working");
                                    string strActivity = "<tr><td><b>Resource:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>" + oUser.GetFullName(intAssigned) + "</td></tr>";
                                    strActivity += strSpacerRow;
                                    strActivity += "<tr><td><b>Service:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>Project Coordinator</td></tr>";
                                    strActivity = "<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"" + oVariable.DefaultFontStyle() + "\">" + strActivity + "</table>";
                                    string strDeliverable = oApplication.Get(intApp, "deliverables_doc");
                                    if (strDeliverable.Trim() != "")
                                        strDeliverable = "<p><a href=\"" + oVariable.URL() + strDeliverable + "\">Click here to view the service deliverables</a></p>";
                                    if (oService.Get(intService, "notify_client") != "0")
                                        oFunction.SendEmail("Request Assignment", oUser.GetName(intRequester), strExecutive + ";" + strWorking + ";" + strCC, strEMailIdsBCC, "Request Assignment", "<p><b>A resource has been assigned to the following project...</b><p><p>" + oProjectRequest.GetBody(intRequest, intEnvironment, true) + "</p><p>" + strActivity + "</p>" + strDeliverable + strNotify, true, false);
                                }
                            }
                        }
                        else
                        {
                            // Update Assignee
                            int intResourceWorkflow = Int32.Parse(dsWorkflow.Tables[0].Rows[0]["id"].ToString());
                            int intOldUser = Int32.Parse(dsWorkflow.Tables[0].Rows[0]["userid"].ToString());
                            if (chkEmail.Checked == false)
                            {
                                string strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_REQUEST_ASSIGNMENT");
                                string strSpacerRow = "<tr><td colspan=\"3\"><img src=\"" + oVariable.ImageURL() + "/images/spacer.gif\" border=\"0\" width=\"1\" height=\"7\" /></td></tr>";
                                string strDefault = oUser.GetApplicationUrl(intUser, intViewPage);
                                // Notify Old User
                                // NOTIFICATION
                                oFunction.SendEmail("Request Assignment", oUser.GetName(intOldUser), "", strEMailIdsBCC, "Request Assignment", "<p><b>The following request has been removed from your workload</b><p><p>" + oResourceRequest.GetWorkflowSummary(intResourceWorkflow, intEnvironment, dsnServiceEditor, dsnAsset, dsnIP) + "</p>", true, false);
                                // Notify New User
                                if (strDefault == "")
                                    oFunction.SendEmail("Request Assignment", oUser.GetName(intUser), "", strEMailIdsBCC, "Request Assignment", "<p><b>The following request has been re-assigned to you by " + oUser.GetFullName(intProfile) + "</b><p><p>" + oResourceRequest.GetWorkflowSummary(intResourceWorkflow, intEnvironment, dsnServiceEditor, dsnAsset, dsnIP) + "</p>", true, false);
                                else
                                {
                                    if (intProject > 0)
                                        oFunction.SendEmail("Request Assignment", oUser.GetName(intUser), "", strEMailIdsBCC, "Request Assignment", "<p><b>The following request has been re-assigned to you by " + oUser.GetFullName(intProfile) + "</b><p><p>" + oResourceRequest.GetWorkflowSummary(intResourceWorkflow, intEnvironment, dsnServiceEditor, dsnAsset, dsnIP) + "</p><p><a href=\"" + oVariable.URL() + "/redirect.aspx?referrer=/" + strDefault + oPage.GetFullLink(intViewPage) + "?pid=" + intProject.ToString() + "\" target=\"_blank\">Click here to review your new assignment.</a></p>", true, false);
                                    else
                                        oFunction.SendEmail("Request Assignment", oUser.GetName(intUser), "", strEMailIdsBCC, "Request Assignment", "<p><b>The following request has been re-assigned to you by " + oUser.GetFullName(intProfile) + "</b><p><p>" + oResourceRequest.GetWorkflowSummary(intResourceWorkflow, intEnvironment, dsnServiceEditor, dsnAsset, dsnIP) + "</p><p><a href=\"" + oVariable.URL() + "/redirect.aspx?referrer=/frame/resource_request.aspx?rrid=" + intResourceWorkflow.ToString() + "\" target=\"_blank\">Click here to review your new assignment.</a></p>", true, false);
                                }
                                string strActivity = "<tr><td><b>Resource:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>" + oUser.GetFullName(intUser) + "</td></tr>";
                                strActivity += strSpacerRow;
                                strActivity += "<tr><td><b>Activity Type:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>" + oService.GetName(intService) + "</td></tr>";
                                strActivity = "<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"" + oVariable.DefaultFontStyle() + "\">" + strActivity + "</table>";
                                string strDeliverable = oApplication.Get(intApp, "deliverables_doc");
                                if (strDeliverable.Trim() != "")
                                    strDeliverable = "<p><a href=\"" + oVariable.URL() + strDeliverable + "\">Click here to view the service deliverables</a></p>";
                            }
                            oResourceRequest.UpdateWorkflowAssigned(intResourceWorkflow, intUser);
                        }
                    }
                }
            }
            return intReturn;
        }
    }
}
