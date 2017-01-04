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
    public partial class manager : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
        private string dsnServiceEditor = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["ServiceEditorDSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected DataSet ds;
        protected int intWorkload = Int32.Parse(ConfigurationManager.AppSettings["ViewWorkload"]);
        protected int intViewRequest = Int32.Parse(ConfigurationManager.AppSettings["ViewRequest"]);
        protected int intViewResourceRequest = Int32.Parse(ConfigurationManager.AppSettings["ResourceRequest"]);
        protected int intProfile = 0;
        protected Projects oProject;
        protected ServiceRequests oServiceRequest;
        protected Organizations oOrganization;
        protected ResourceRequest oResourceRequest;
        protected RequestItems oRequestItem;
        protected Services oService;
        protected Pages oPage;
        protected Variables oVariable;
        protected Applications oApplication;
        protected Requests oRequest;
        protected Users oUser;
        protected ServiceDetails oServiceDetail;
        protected StatusLevels oStatusLevel;
        protected Log oLog;
        private string strEMailIdsBCC = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            AuthenticateUser();
            if (Request.Cookies["profileid"] != null && Request.Cookies["profileid"].Value != "")
                intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oProject = new Projects(intProfile, dsn);
            oServiceRequest = new ServiceRequests(intProfile, dsn);
            oOrganization = new Organizations(intProfile, dsn);
            oResourceRequest = new ResourceRequest(intProfile, dsn);
            oRequestItem = new RequestItems(intProfile, dsn);
            oService = new Services(intProfile, dsn);
            oPage = new Pages(intProfile, dsn);
            oVariable = new Variables(intEnvironment);
            oApplication = new Applications(intProfile, dsn);
            oRequest = new Requests(intProfile, dsn);
            oUser = new Users(intProfile, dsn);
            oServiceDetail = new ServiceDetails(intProfile, dsn);
            oStatusLevel = new StatusLevels(intProfile, dsn);
            oLog = new Log(intProfile, dsn);

            if (Request.QueryString["save"] != null)
                panSave.Visible = true;
            if (Request.QueryString["close"] != null)
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "close", "<script type=\"text/javascript\">window.close();<" + "/" + "script>");
            if (!IsPostBack)
            {
                if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
                {
                    lblResourceWorkflow.Text = Request.QueryString["id"];
                    int intResourceWorkflow = Int32.Parse(lblResourceWorkflow.Text);
                    int intResourceParent = oResourceRequest.GetWorkflowParent(intResourceWorkflow);

                    string strHeader = intResourceWorkflow.ToString();
                    lblHeader.Text = "&quot;" + strHeader + "&quot;";
                    Master.Page.Title = "DataPoint | Resource (" + strHeader + ") Administration";
                    lblHeaderSub.Text = "Provides administration of a resource request...";

                    ds = oResourceRequest.Get(intResourceParent);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        int intItem = Int32.Parse(ds.Tables[0].Rows[0]["itemid"].ToString());
                        int intService = Int32.Parse(ds.Tables[0].Rows[0]["serviceid"].ToString());
                        int intApp = oRequestItem.GetItemApplication(intItem);
                        int intUser = Int32.Parse(oResourceRequest.GetWorkflow(intResourceWorkflow, "userid"));
                        int intRequest = Int32.Parse(ds.Tables[0].Rows[0]["requestid"].ToString());
                        int intNumber = Int32.Parse(ds.Tables[0].Rows[0]["number"].ToString());
                        int intProject = oRequest.GetProjectNumber(intRequest);
                        if (oService.Get(intService, "disable_hours") != "1")
                        {
                            txtAllocated.Enabled = false;
                            txtAllocated.ToolTip = intService.ToString();
                            panDynamic.Visible = true;
                        }
                        bool boolManager = false;
                        double dblUsed = oResourceRequest.GetWorkflowUsed(intResourceWorkflow);
                        lblUsed.Text = dblUsed.ToString("F");
                        lblAllocated.Text = oResourceRequest.GetWorkflow(intResourceWorkflow, "allocated");
                        txtAllocated.Text = lblAllocated.Text;
                        txtDevices.Text = oResourceRequest.GetWorkflow(intResourceWorkflow, "devices");
                        lblDevices.Text = txtDevices.Text;
                        string strStatus = ds.Tables[0].Rows[0]["status"].ToString();
                        ddlStatus.DataValueField = "StatusValue";
                        ddlStatus.DataTextField = "StatusDescription";
                        ddlStatus.DataSource = oStatusLevel.GetStatusList("RRWFSTATUS");
                        ddlStatus.DataBind();
                        ddlStatus.SelectedValue = strStatus;
                        int intAppManager = oApplication.GetManager(intApp);
                        LoadList(intAppManager, intApp);
                        LoadAvailable(intAppManager, intApp, intRequest, intService, intNumber);
                        if (oUser.IsAdmin(intProfile) || oService.IsManager(intService, intProfile) || (oApplication.IsManager(intApp, intProfile) && oApplication.Get(intApp, "disable_manager") != "1") || (oUser.IsManager(intUser, intProfile, true) && oApplication.Get(intApp, "disable_manager") != "1"))
                            boolManager = true;
                        try
                        {
                            ddlActivity.SelectedValue = intItem.ToString();
                            lblActivity.Text = ddlActivity.SelectedItem.Text;
                            hdnUser.Value = intUser.ToString();
                            txtUser.Text = oUser.GetFullName(intUser) + " (" + oUser.GetName(intUser) + ")";
                            lblUser.Text = txtUser.Text;
                        }
                        catch { }
                        if (boolManager == true)
                        {
                            Master.Page.Title = "Resource Request #" + intResourceWorkflow.ToString() + " Management View";
                            panView.Visible = true;
                            panManager.Visible = true;
                            txtAllocated.Visible = true;
                            txtDevices.Visible = true;
                            ddlActivity.Visible = true;
                            panUser.Visible = true;
                            ddlStatus.Enabled = true;
                            btnSave.Attributes.Add("onclick", "return ValidateNumber('" + txtAllocated.ClientID + "','Please enter a valid number for the allocated hours')" +
                                " && ValidateNumber('" + txtDevices.ClientID + "','Please enter a valid number for the device count')" +
                                " && ValidateDropDown('" + ddlActivity.ClientID + "','Please make a selection for the activity type')" +
                                " && ValidateHidden0('" + hdnUser.ClientID + "','" + txtUser.ClientID + "','Please select an assigned technician')" +
                                " && ProcessControlButton()" +
                                ";");
                            btnSaveClose.Attributes.Add("onclick", "return ValidateNumber('" + txtAllocated.ClientID + "','Please enter a valid number for the allocated hours')" +
                                " && ValidateNumber('" + txtDevices.ClientID + "','Please enter a valid number for the device count')" +
                                " && ValidateDropDown('" + ddlActivity.ClientID + "','Please make a selection for the activity type')" +
                                " && ValidateHidden0('" + hdnUser.ClientID + "','" + txtUser.ClientID + "','Please select an assigned technician')" +
                                " && ProcessControlButton()" +
                                ";");
                        }
                        else
                        {
                            Master.Page.Title = "Resource Request #" + intResourceWorkflow.ToString() + " Technician View";
                            panView.Visible = true;
                            lblAllocated.Visible = true;
                            lblDevices.Visible = true;
                            lblActivity.Visible = true;
                            lblUser.Visible = true;
                        }
                    }
                    else
                        panDenied.Visible = true;
                }
                else
                    panDenied.Visible = true;

                txtUser.Attributes.Add("onkeyup", "return AJAXTextBoxGet(this,'300','195','" + divUser.ClientID + "','" + lstUser.ClientID + "','" + hdnUser.ClientID + "','" + oVariable.URL() + "/frame/users.aspx',2);");
                lstUser.Attributes.Add("ondblclick", "AJAXClickRow();");
                lnkAvailable.Attributes.Add("onclick", "return ShowHideAvailable('" + divAvailable.ClientID + "');");
                btnFinish.Attributes.Add("onclick", "return CloseWindow();");
                btnClose.Attributes.Add("onclick", "return CloseWindow();");
                btnDenied.Attributes.Add("onclick", "return CloseWindow();");
            }
        }
        private void LoadList(int _appmanager, int _app)
        {
            ds = oRequestItem.GetItems(_app, 0, 1);
            ddlActivity.DataValueField = "itemid";
            ddlActivity.DataTextField = "name";
            ddlActivity.DataSource = ds;
            ddlActivity.DataBind();
            ddlActivity.Items.Insert(0, new ListItem("-- SELECT --", "0"));
        }
        private void LoadAvailable(int _appmanager, int _app, int _requestid, int _serviceid, int _number)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("name", System.Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("hours", System.Type.GetType("System.Double")));
            dt.Columns.Add(new DataColumn("graph", System.Type.GetType("System.Double")));
            ds = oUser.GetManagerReports(_appmanager, _requestid, _serviceid, _number);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                int intUser = Int32.Parse(dr["userid"].ToString());
                DataSet dsAss = oResourceRequest.GetWorkflowAssigned(intUser, 2);
                double dblTotal = 0;
                foreach (DataRow drAss in dsAss.Tables[0].Rows)
                {
                    int intId = Int32.Parse(drAss["id"].ToString());
                    double dblAllocated = double.Parse(drAss["allocated"].ToString());
                    double dblUsed = oResourceRequest.GetWorkflowUsed(intId);
                    dblTotal += dblAllocated - dblUsed;
                }
                DataRow drRow = dt.NewRow();
                drRow["name"] = oUser.GetFullName(intUser);
                drRow["hours"] = dblTotal;
                dt.Rows.Add(drRow);
            }
            ds = new DataSet();
            ds.Tables.Add(dt);
            double dblMax = 0;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                double dblHours = double.Parse(dr["hours"].ToString());
                if (dblMax < dblHours)
                    dblMax = dblHours;
            }
            if (dblMax > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    double dblHours = double.Parse(dr["hours"].ToString());
                    dblHours = dblHours / dblMax;
                    dr["graph"] = dblHours * 400;
                }
            }
            rptAvailable.DataSource = ds;
            rptAvailable.DataBind();
        }
        protected void btnSave_Click(Object Sender, EventArgs e)
        {
            Save();
            Response.Redirect(Request.Path + "?id=" + lblResourceWorkflow.Text + "&save=true");
        }
        protected void btnSaveClose_Click(Object Sender, EventArgs e)
        {
            Save();
            Response.Redirect(Request.Path + "?id=" + lblResourceWorkflow.Text + "&close=true");
        }
        private void Save()
        {
            int intResourceWorkflow = Int32.Parse(lblResourceWorkflow.Text);
            int intResourceParent = oResourceRequest.GetWorkflowParent(intResourceWorkflow);
            int intOldUser = Int32.Parse(oResourceRequest.GetWorkflow(intResourceWorkflow, "userid"));
            int intNewUser = 0;
            Int32.TryParse(Request.Form[hdnUser.UniqueID], out intNewUser);
            int intOldItem = Int32.Parse(oResourceRequest.Get(intResourceParent, "itemid"));
            int intNewItem = Int32.Parse(ddlActivity.SelectedItem.Value);
            int intNumber = Int32.Parse(oResourceRequest.Get(intResourceParent, "number"));
            int intService = Int32.Parse(oResourceRequest.Get(intResourceParent, "serviceid"));
            int intApp = oRequestItem.GetItemApplication(intNewItem);
            int intRequest = Int32.Parse(oResourceRequest.Get(intResourceParent, "requestid"));
            int intProject = oRequest.GetProjectNumber(intRequest);
            int intRequester = oRequest.GetUser(intRequest);
            int intStatus = Int32.Parse(oResourceRequest.GetWorkflow(intResourceWorkflow, "status"));
            Functions oFunction = new Functions(intProfile, dsn, intEnvironment);
            string strCVT = "CVT" + intRequest.ToString() + "-" + intService.ToString() + "-" + intNumber.ToString();
            if (intOldUser != intNewUser)
            {
                oLog.AddEvent(intRequest.ToString(), strCVT, "Re-assigned from " + oUser.GetFullNameWithLanID(intOldUser) + " to " + oUser.GetFullNameWithLanID(intNewUser) + " by " + oUser.GetFullNameWithLanID(intProfile), LoggingType.Debug);
                strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_REQUEST_ASSIGNMENT");
                string strSpacerRow = "<tr><td colspan=\"3\"><img src=\"" + oVariable.ImageURL() + "/images/spacer.gif\" border=\"0\" width=\"1\" height=\"7\" /></td></tr>";
                string strDefault = oUser.GetApplicationUrl(intNewUser, intWorkload);
                // Notify Old User
                // NOTIFICATION
                oFunction.SendEmail("Service Request Assignment", oUser.GetName(intOldUser), "", strEMailIdsBCC, "Service Request Removed [" + strCVT + "]", "<p><b>The following request has been removed from your workload</b><p><p>" + oResourceRequest.GetWorkflowSummary(intResourceWorkflow, intEnvironment, dsnServiceEditor, dsnAsset, dsnIP) + "</p>", true, false);
                // Notify New User
                if (strDefault == "")
                    oFunction.SendEmail("Service Request Assignment", oUser.GetName(intNewUser), "", strEMailIdsBCC, "Service Request Assignment [" + strCVT + "]", "<p><b>The following request has been re-assigned to you by " + oUser.GetFullName(intProfile) + "</b><p><p>" + oResourceRequest.GetWorkflowSummary(intResourceWorkflow, intEnvironment, dsnServiceEditor, dsnAsset, dsnIP) + "</p>", true, false);
                else
                {
                    if (intProject > 0)
                        oFunction.SendEmail("Service Request Assignment", oUser.GetName(intNewUser), "", strEMailIdsBCC, "Service Request Assignment [" + strCVT + "]", "<p><b>The following request has been re-assigned to you by " + oUser.GetFullName(intProfile) + "</b><p><p>" + oResourceRequest.GetWorkflowSummary(intResourceWorkflow, intEnvironment, dsnServiceEditor, dsnAsset, dsnIP) + "</p><p><a href=\"" + oVariable.URL() + "/redirect.aspx?referrer=/" + strDefault + oPage.GetFullLink(intWorkload) + "?pid=" + intProject.ToString() + "\" target=\"_blank\">Click here to review your new assignment.</a></p>", true, false);
                    else
                        oFunction.SendEmail("Service Request Assignment", oUser.GetName(intNewUser), "", strEMailIdsBCC, "Service Request Assignment [" + strCVT + "]", "<p><b>The following request has been re-assigned to you by " + oUser.GetFullName(intProfile) + "</b><p><p>" + oResourceRequest.GetWorkflowSummary(intResourceWorkflow, intEnvironment, dsnServiceEditor, dsnAsset, dsnIP) + "</p><p><a href=\"" + oVariable.URL() + "/redirect.aspx?referrer=/frame/resource_request.aspx?rrid=" + intResourceWorkflow.ToString() + "\" target=\"_blank\">Click here to review your new assignment.</a></p>", true, false);
                }
                string strActivity = "<tr><td><b>Resource:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>" + oUser.GetFullName(intNewUser) + "</td></tr>";
                strActivity += strSpacerRow;
                strActivity += "<tr><td><b>Activity Type:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>" + oService.GetName(intService) + "</td></tr>";
                strActivity = "<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"" + oVariable.DefaultFontStyle() + "\">" + strActivity + "</table>";
                string strDeliverable = oApplication.Get(intApp, "deliverables_doc");
                if (strDeliverable.Trim() != "")
                    strDeliverable = "<p><a href=\"" + oVariable.URL() + strDeliverable + "\">Click here to view the service deliverables</a></p>";
            }
            double dblAllocated = 0.00;
            if (panDynamic.Visible == false)
                dblAllocated = double.Parse(txtAllocated.Text);
            else
            {
                double dblQuantity = double.Parse(txtDevices.Text);
                dblAllocated = oServiceDetail.GetHours(Int32.Parse(txtAllocated.ToolTip), dblQuantity);
            }
            int intStatusNew = Int32.Parse(ddlStatus.SelectedItem.Value);
            oResourceRequest.UpdateWorkflow(intResourceWorkflow, Int32.Parse(txtDevices.Text), dblAllocated, intStatusNew);
            oResourceRequest.UpdateStatusOverall(intResourceParent, intStatusNew);
            oResourceRequest.UpdateWorkflowAssigned(intResourceWorkflow, intNewUser);
            oResourceRequest.UpdateItemAndService(intResourceParent, intNewItem, intService);
            if (intStatusNew == (int)ResourceRequestStatus.Active && intProject > 0)
                oProject.Update(intProject, 2);
            if (intStatus == (int)ResourceRequestStatus.Closed && intStatusNew != intStatus)
            {
                // Re-opened
                oLog.AddEvent(intRequest.ToString(), strCVT, "Re-opened by " + oUser.GetFullNameWithLanID(intProfile), LoggingType.Debug);
                oResourceRequest.UpdateCompleted(intRequest, oService.GetItemId(intService), intService, intNumber, "");
            }
            else if (intStatusNew == (int)ResourceRequestStatus.Closed && intStatusNew != intStatus)
            {
                // Close
                oLog.AddEvent(intRequest.ToString(), strCVT, "Closed by " + oUser.GetFullNameWithLanID(intProfile), LoggingType.Debug);
                oResourceRequest.UpdateCompleted(intRequest, oService.GetItemId(intService), intService, intNumber, DateTime.Now.ToString());
            }
        }
    }
}
