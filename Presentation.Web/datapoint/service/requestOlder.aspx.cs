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
    public partial class requestOlder : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
        private string dsnServiceEditor = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["ServiceEditorDSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intDataPointAvailableService = Int32.Parse(ConfigurationManager.AppSettings["DATAPOINT_AVAILABLE_SERVICE"]);
        protected int intViewRequest = Int32.Parse(ConfigurationManager.AppSettings["ResourceRequest"]);
        protected DataPoint oDataPoint;
        protected ServiceRequests oServiceRequest;
        protected ResourceRequest oResourceRequest;
        protected RequestItems oRequestItem;
        protected Requests oRequest;
        protected Pages oPage;
        protected Services oService;
        protected Users oUser;
        protected Functions oFunction;
        protected int intProfile = 0;
        protected int intApplication = 0;
        protected int intRequest = 0;
        protected string strMenuTab1 = "";
        private string strEMailIdsBCC = "";
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
            oServiceRequest = new ServiceRequests(intProfile, dsn);
            oResourceRequest = new ResourceRequest(intProfile, dsn);
            oRequestItem = new RequestItems(intProfile, dsn);
            oRequest = new Requests(intProfile, dsn);
            oPage = new Pages(intProfile, dsn);
            oService = new Services(intProfile, dsn);
            oUser = new Users(intProfile, dsn);
            oFunction = new Functions(intProfile, dsn, intEnvironment);
            if (oUser.IsAdmin(intProfile) == true || (oDataPoint.GetPagePermission(intApplication, "SERVICE") == true || intDataPointAvailableService == 1))
            {
                panAllow.Visible = true;
                if (Request.QueryString["save"] != null)
                    panSave.Visible = true;
                if (Request.QueryString["cancel"] != null)
                    panCancel.Visible = true;
                if (Request.QueryString["error"] != null)
                {
                    panError.Visible = true;
                }
                if (Request.QueryString["close"] != null)
                    Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "close", "<script type=\"text/javascript\">window.close();<" + "/" + "script>");
                else if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
                {
                    string strID = oFunction.decryptQueryString(Request.QueryString["id"]);
                    intRequest = Int32.Parse(strID);
                    DataSet ds = oDataPoint.GetServiceRequest(intRequest);
                    if (ds.Tables[0].Rows.Count == 1)
                    {
                        // Load General Information
                        lblRequestID.Text = "#" + intRequest.ToString();
                        string strHeader = "CVT" + intRequest.ToString();
                        lblHeader.Text = "&quot;" + strHeader + "&quot;";
                        Master.Page.Title = "DataPoint | Request (" + strHeader + ")";
                        lblHeaderSub.Text = "Provides all the information about a request...";
                        int intMenuTab = 0;
                        if (Request.QueryString["menu_tab"] != null && Request.QueryString["menu_tab"] != "")
                            intMenuTab = Int32.Parse(Request.QueryString["menu_tab"]);
                        //Tab oTab = new Tab(hdnTab.ClientID, intMenuTab, "divMenu1", true, false);
                        //oTab.AddTab("Request Information", "");
                        panDeleted.Visible = (ds.Tables[0].Rows[0]["deleted"].ToString() == "1");

                        if (!IsPostBack)
                        {
                            // General Information
                            int intRequestor = Int32.Parse(ds.Tables[0].Rows[0]["userid"].ToString());
                            oDataPoint.LoadDropDownAJAX(txtRequestBy, hdnRequestBy, divRequestBy, lstRequestBy, intEnvironment, intProfile, null, "", lblRequestBy, fldRequestBy, "REQUEST_BY", intRequestor, (intRequestor > 0 ? oUser.GetFullName(intRequestor) + " (" + oUser.GetName(intRequestor) + ")" : ""), "/frame/users.aspx", "", false, true);
                            oDataPoint.LoadTextBox(txtRequestOn, intProfile, null, "", lblRequestOn, fldRequestOn, "REQUEST_ON", ds.Tables[0].Rows[0]["created"].ToString(), "", false, true);
                            //oDataPoint.LoadTextBoxDate(txtRequestOn, imgRequestOn, intProfile, null, "", lblRequestOn, fldRequestOn, "REQUEST_ON", DateTime.Parse(ds.Tables[0].Rows[0]["created"].ToString()).ToShortDateString(), "", false, true);
                            oDataPoint.LoadTextBox(txtProjectNumber, intProfile, btnProjectName, "/datapoint/project/project.aspx?t=number&q=" + oFunction.encryptQueryString(ds.Tables[0].Rows[0]["project_number"].ToString()) + "&id=" + oFunction.encryptQueryString(ds.Tables[0].Rows[0]["projectid"].ToString()), lblProjectNumber, fldProjectNumber, "PROJECT_NUMBER", ds.Tables[0].Rows[0]["project_number"].ToString(), "", false, false);
                            oDataPoint.LoadTextBox(txtProjectName, intProfile, null, "", lblProjectName, fldProjectName, "PROJECT_NAME", ds.Tables[0].Rows[0]["project_name"].ToString(), "", false, false);
                            int intCheckout = 100;
                            if (ds.Tables[0].Rows[0]["checkout"].ToString() != "")
                                intCheckout = Int32.Parse(ds.Tables[0].Rows[0]["checkout"].ToString());
                            oDataPoint.LoadDropDown(ddlStatus, intProfile, null, "", lblStatus, fldStatus, "REQUEST_STATUS", "name", "id", SqlHelper.ExecuteDataset(dsn, CommandType.Text, "SELECT -2 AS id, 'Service Browser' AS name UNION ALL SELECT -1 AS id, 'Cart Summary' AS name UNION ALL SELECT 0 AS id, 'Completing Forms' AS name UNION ALL SELECT 1 AS id, 'Submitted' AS name UNION ALL SELECT 100 AS id, 'N / A   (no service request)' AS name"), intCheckout, true, false, true);
                            string strViewRequest = oPage.GetFullLink(intViewRequest);
                            if (strViewRequest != "")
                                lnkView.Attributes.Add("onclick", "return OpenNewWindowMenu('" + strViewRequest + "?rid=" + intRequest.ToString() + "', '800', '600');");
                            else
                                lnkView.Enabled = false;


                            // Service Request(s)
                            //oTab.AddTab("Service Request(s)", "");
                            if (ds.Tables[1].Rows.Count > 0)
                            {
                                if (ds.Tables[2].Rows.Count > 0)
                                    ds.Relations.Add("relationship", ds.Tables[1].Columns["requestid"], ds.Tables[2].Columns["requestid"], false);
                                rptServiceRequests.DataSource = ds.Tables[1];
                                rptServiceRequests.DataBind();
                                LoadRepeater(rptServiceRequests, strViewRequest);
                            }
                            lblServiceRequests.Visible = (rptServiceRequests.Items.Count == 0);

                            // Request Results
                            //oTab.AddTab("Request Results", "");
                            DataSet dsResults = oRequest.GetResult(intRequest);
                            if (dsResults.Tables[0].Rows.Count > 0)
                            {
                                rptResults.DataSource = dsResults;
                                rptResults.DataBind();
                            }
                            lblResults.Visible = (rptResults.Items.Count == 0);

                            //strMenuTab1 = oTab.GetTabs();
                        }
                    }
                    else
                    {
                        if (Request.QueryString["t"] != null && Request.QueryString["q"] != null)
                            Response.Redirect("/datapoint/service/datapoint_service_search.aspx?t=" + Request.QueryString["t"] + "&q=" + Request.QueryString["q"] + "&r=0");
                        else
                            Response.Redirect("/datapoint/service/datapoint_service_search.aspx");
                    }
                }
                else if (Request.QueryString["q"] != null && Request.QueryString["q"] != "")
                {
                    string strQuery = oFunction.decryptQueryString(Request.QueryString["q"]);
                    DataSet ds = oDataPoint.GetServiceRequest(strQuery);
                    if (ds.Tables[0].Rows.Count == 1)
                    {
                        intRequest = Int32.Parse(ds.Tables[0].Rows[0]["requestid"].ToString());
                        Response.Redirect(Request.Path + "?t=" + Request.QueryString["t"] + "&q=" + Request.QueryString["q"] + "&id=" + oFunction.encryptQueryString(intRequest.ToString()));
                    }
                }
                else
                    Response.Redirect("/datapoint/service/datapoint_service_search.aspx");
                btnClose.Attributes.Add("onclick", "window.close();return false;");
                btnPrint.Attributes.Add("onclick", "window.print();return false;");
                btnSave.Attributes.Add("onclick", oDataPoint.LoadValidation());
                btnSaveClose.Attributes.Add("onclick", oDataPoint.LoadValidation());
            }
            else
                panDenied.Visible = true;
        }
        private void LoadRepeater(Repeater rptControl, string strViewRequest)
        {
            foreach (RepeaterItem ri in rptControl.Items)
            {
                Button btnCancel = (Button)ri.FindControl("btnCancel");
                if (btnCancel != null)
                    btnCancel.Attributes.Add("onclick", "return confirm('NOTE: When you cancel a service in ClearView, the following actions will be taken...\\n\\n - Incomplete work will be stopped immediately\\n - Assigned technicians will be notified\\n\\nAre you sure you want to cancel this service?');");
                LinkButton btnNumber = (LinkButton)ri.FindControl("btnNumber");
                int intNumber = 0;
                if (btnNumber != null)
                {
                    if (btnNumber.Text != "")
                        intNumber = Int32.Parse(btnNumber.Text);
                }
                int intService = 0;
                Button btnView = (Button)ri.FindControl("btnView");
                intService = Int32.Parse(btnView.CommandArgument);

                Label lblDetails = (Label)ri.FindControl("lblDetails");
                Label lblProgress = (Label)ri.FindControl("lblProgress");
                Label lblRequestStatus = (Label)ri.FindControl("lblRequestStatus");
                if (lblRequestStatus.Text == "Submitted")
                {
                    if (btnNumber != null)
                    {
                        if (strViewRequest != "" && oService.Get(intService, "disable_customization") == "0")
                        {
                            if (intNumber > 0)
                                btnNumber.Attributes.Add("onclick", "return OpenWindow('PRINTER_FRIENDLY','?page=" + intViewRequest.ToString() + "&rid=" + intRequest.ToString() + "&formid=" + intService.ToString() + "&num=" + intNumber.ToString() + "&view=true', '800', '600');");
                            else
                                btnNumber.Enabled = false;
                        }
                        else
                            btnNumber.Enabled = false;
                    }

                    if (lblProgress.Text == "")
                    {
                        lblProgress.Text = "<i>Unavailable</i>";
                        if (btnNumber != null)
                            btnNumber.Enabled = false;
                    }
                    else
                    {
                        int intResource = Int32.Parse(lblProgress.Text);
                        double dblAllocated = 0.00;
                        double dblUsed = 0.00;
                        bool boolAssigned = false;
                        DataSet dsResource = oDataPoint.GetServiceRequestResource(intResource);
                        foreach (DataRow drResource in dsResource.Tables[1].Rows)
                        {
                            if (drResource["deleted"].ToString() == "0")
                            {
                                boolAssigned = true;
                                dblAllocated += double.Parse(drResource["allocated"].ToString());
                                dblUsed += double.Parse(drResource["used"].ToString());
                            }
                        }
                        if (boolAssigned == false)
                        {
                            Label lblOnDemand = (Label)ri.FindControl("lblOnDemand");
                            if (lblOnDemand == null || lblOnDemand.Text == "0")
                            {
                                Label lblAutomate = (Label)ri.FindControl("lblAutomate");
                                if (lblAutomate == null || lblAutomate.Text == "0")
                                {
                                    string strManager = "<tr><td colspan=\"3\"><img src=\"/images/spacer.gif\" border=\"0\" width=\"15\" height=\"1\"/></td></tr>";
                                    DataSet dsManager = oService.GetUser(intService, 1);  // Managers
                                    foreach (DataRow drManager in dsManager.Tables[0].Rows)
                                    {
                                        int intManager = Int32.Parse(drManager["userid"].ToString());
                                        strManager += "<tr><td><img src=\"/images/spacer.gif\" border=\"0\" width=\"15\" height=\"1\"/></td><td>-</td><td><a href=\"javascript:void(0);\" class=\"lookup\" onclick=\"OpenWindow('PROFILE','?userid=" + intManager.ToString() + "');\">" + oUser.GetFullName(intManager) + " [" + oUser.GetName(intManager) + "]</a></td></tr>";
                                    }
                                    lblProgress.Text = "Pending Assignment [<a href=\"javascript:void(0);\" class=\"lookup\" onclick=\"ShowHideDiv2('divAssign" + intResource.ToString() + "');\">View Service Managers</a>]<div id=\"divAssign" + intResource.ToString() + "\" style=\"display:none\"><table cellpadding=\"2\" cellspacing=\"2\" border=\"0\">" + strManager + "</table></div>";
                                }
                                else
                                {   //For automated 
                                    lblProgress.Text = oServiceRequest.GetStatusBar(100.00, "100", "12", true);
                                    btnView.Visible = false;
                                    btnCancel.Visible = false;
                                }
                            }
                            else
                            {
                                DataSet dsSelected = oService.GetSelected(intRequest, intService, intNumber);
                                for (int ii = intNumber; ii > 0 && dsSelected.Tables[0].Rows.Count == 0; ii--)
                                    dsSelected = oService.GetSelected(intRequest, intService, ii - 1);
                                int intApproved = 0;
                                if (dsSelected.Tables[0].Rows.Count > 0)
                                    intApproved = Int32.Parse(dsSelected.Tables[0].Rows[0]["approved"].ToString());

                                if (intApproved > 0)
                                {
                                    Forecast oForecast = new Forecast(intProfile, dsn);
                                    ModelsProperties oModelsProperties = new ModelsProperties(intProfile, dsn);
                                    Types oType = new Types(intProfile, dsn);
                                    DataSet dsService = oForecast.GetAnswerService(intRequest);
                                    if (dsService.Tables[0].Rows.Count > 0)
                                    {
                                        int intAnswer = Int32.Parse(dsService.Tables[0].Rows[0]["id"].ToString());
                                        int intModel = Int32.Parse(dsService.Tables[0].Rows[0]["modelid"].ToString());
                                        int intType = oModelsProperties.GetType(intModel);
                                        string strExecute = oType.Get(intType, "forecast_execution_path");
                                        if (strExecute != "")
                                            lblProgress.Text = "<a href=\"javascript:void(0);\" class=\"lookup\" title=\"Click here to execute this service\" onclick=\"OpenWindow('FORECAST_EXECUTE','" + strExecute + "?id=" + intAnswer.ToString() + "');\">Execute</a>";
                                        else
                                            lblProgress.Text = "<a href=\"javascript:void(0);\" class=\"lookup\" title=\"Click here to execute this service\" onclick=\"alert('Execution has not been configured for asset type " + oType.Get(intType, "name") + "');\">Execute</a>";
                                    }
                                }
                                else
                                {
                                    lblProgress.Text = "<a title=\"Click here to execute this service\" disabled=\"disabled\">Execute</a>";
                                    if (intApproved < 0)
                                    {
                                        int intDenied = Int32.Parse(dsSelected.Tables[0].Rows[0]["approvedby"].ToString());
                                        string strApprovedOn = dsSelected.Tables[0].Rows[0]["approvedon"].ToString();
                                        string strReason = dsSelected.Tables[0].Rows[0]["reason"].ToString();
                                        lblRequestStatus.Text = "<a href=\"javascript:void(0);\" onclick=\"alert('Denied By: " + oUser.GetFullName(intDenied) + "\\nDenied On: " + strApprovedOn + "\\nReason: " + strReason.Replace(Environment.NewLine, "\\n") + "');\">Request Denied</a>";
                                    }
                                    else
                                    {
                                        lblRequestStatus.Text = "Pending Approval";
                                    }
                                }
                            }
                        }
                        else if (dblAllocated > 0.00)
                            lblProgress.Text = oServiceRequest.GetStatusBar((dblUsed / dblAllocated) * 100.00, "100", "12", true);
                        else
                            lblProgress.Text = "<i>N / A</i>";
                        btnView.Attributes.Add("onclick", "return OpenNewWindowMenu('/datapoint/service/resource.aspx?id=" + oFunction.encryptQueryString(intResource.ToString()) + "', '800', '600');");
                        btnView.ToolTip = "ResourceID: " + intResource.ToString();
                        if (btnNumber != null)
                            lblDetails.Text = "<a href=\"javascript:void(0);\" class=\"lookup\" onclick=\"OpenWindow('SERVICES_DETAIL','?rid=" + intRequest.ToString() + "&sid=" + intService.ToString() + "');\" title=\"Click here to view the details of this service\">" + oService.Get(intService, "name") + "</a>";
                        else
                            lblDetails.Text = "<a href=\"javascript:void(0);\" class=\"lookup\" onclick=\"OpenWindow('SERVICES_DETAIL','?sid=" + intService.ToString() + "');\" title=\"Click here to view the details of this service\">" + oService.Get(intService, "name") + "</a>";
                    }
                }
                else
                {
                    btnView.Enabled = false;
                    if (btnCancel != null)
                        btnCancel.Enabled = false;
                    if (btnNumber != null)
                    {
                        if (strViewRequest != "" && oService.Get(intService, "disable_customization") == "0" && lblProgress.Text.ToUpper().Contains("AWAITING") == true)
                        {
                            if (intNumber > 0)
                                btnNumber.Attributes.Add("onclick", "return OpenNewWindowMenu('" + strViewRequest + "?rid=" + intRequest.ToString() + "&formid=" + intService.ToString() + "&num=" + intNumber.ToString() + "', '800', '600');");
                            else
                                btnNumber.Enabled = false;
                        }
                        else
                            btnNumber.Enabled = false;
                    }
                    lblProgress.Text = "---";
                }

                // Load Workflow Repeaters
                Repeater rptWorkflow = (Repeater)ri.FindControl("rptWorkflow");
                if (rptWorkflow != null)
                    LoadRepeater(rptWorkflow, strViewRequest);
            }
        }
        protected void btnNew_Click(Object Sender, EventArgs e)
        {
            Response.Redirect("/datapoint/service/datapoint_service_search.aspx");
        }
        protected void btnSave_Click(Object Sender, EventArgs e)
        {
            int intSave = Save();
            Response.Redirect(Request.Path + "?t=" + Request.QueryString["t"] + "&q=" + Request.QueryString["q"] + "&id=" + oFunction.encryptQueryString(intRequest.ToString()) + "&" + (intSave > 0 ? "save" : "error") + "=" + intSave.ToString());
        }
        protected void btnSaveClose_Click(Object Sender, EventArgs e)
        {
            int intSave = Save();
            Response.Redirect(Request.Path + "?t=" + Request.QueryString["t"] + "&q=" + Request.QueryString["q"] + "&id=" + oFunction.encryptQueryString(intRequest.ToString()) + "&" + (intSave > 0 ? "close" : "error") + "=" + intSave.ToString());
        }
        protected int Save()
        {
            int intReturn = 1;
            return intReturn;
        }
        protected void btnCancel_Click(Object Sender, EventArgs e)
        {
            Button btnCancel = (Button)Sender;
            int intService = Int32.Parse(btnCancel.CommandArgument);
            // Remove Resource Request
            int intItem = oService.GetItemId(intService);
            DataSet dsForm = oRequestItem.GetForms(intRequest, intService);
            int intCancelNum = Int32.Parse(btnCancel.CommandName);

            strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_REQUEST_ASSIGNMENT,EMAILGRP_REQUEST_STATUS");
            foreach (DataRow drForm in dsForm.Tables[0].Rows)
            {
                int intCancelNum2 = Int32.Parse(drForm["number"].ToString());
                if (intCancelNum > 0)
                    intCancelNum2 = intCancelNum;
                DataSet dsResource = oResourceRequest.Get(intRequest, intItem, intCancelNum2);
                if (dsResource.Tables[0].Rows.Count > 0)
                {
                    int intResourceWorkflow = Int32.Parse(dsResource.Tables[0].Rows[0]["id"].ToString());
                    int intResourceParent = oResourceRequest.GetWorkflowParent(intResourceWorkflow);
                    oResourceRequest.UpdateStatusOverall(intResourceParent, -2);
                    DataSet dsManager = oService.GetUser(intService, 1);
                    DataSet dsResources = oResourceRequest.GetWorkflowsParent(intResourceParent);
                    foreach (DataRow drResources in dsResources.Tables[0].Rows)
                    {
                        int intUser = Int32.Parse(drResources["userid"].ToString());
                        if (intUser == 0)
                        {
                            foreach (DataRow drManager in dsManager.Tables[0].Rows)
                            {
                                int intManager = Int32.Parse(drManager["userid"].ToString());
                                oFunction.SendEmail("ClearView Service Request CANCELLED", oUser.GetName(intManager), "", strEMailIdsBCC, "ClearView Service Request CANCELLED", "<p><b>The following service request has been cancelled by " + oUser.GetFullName(intProfile) + "</b><p><p>" + oResourceRequest.GetWorkflowSummary(Int32.Parse(drResources["id"].ToString()), intEnvironment, dsnServiceEditor, dsnAsset, dsnIP) + "</p>", true, false);
                            }
                        }
                        else
                        {
                            foreach (DataRow drManager in dsManager.Tables[0].Rows)
                            {
                                int intManager = Int32.Parse(drManager["userid"].ToString());
                                oFunction.SendEmail("ClearView Service Request CANCELLED", oUser.GetName(intUser), oUser.GetName(intManager), strEMailIdsBCC, "ClearView Service Request CANCELLED", "<p><b>The following service request has been cancelled by " + oUser.GetFullName(intProfile) + "</b><p><p>" + oResourceRequest.GetWorkflowSummary(Int32.Parse(drResources["id"].ToString()), intEnvironment, dsnServiceEditor, dsnAsset, dsnIP) + "</p>", true, false);
                            }
                        }
                    }
                }
                oService.CancelSelected(intRequest, intService, intCancelNum2);
                if (intCancelNum > 0)
                    break;
            }
            Response.Redirect(Request.Path + "?t=" + Request.QueryString["t"] + "&q=" + Request.QueryString["q"] + "&id=" + oFunction.encryptQueryString(intRequest.ToString()) + "&cancel=true");
        }
    }
}
