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
    public partial class resource_request_manager : BasePage
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
        protected int intProfile;
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
        private string strEMailIdsBCC = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            AuthenticateUser();
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
            if (Request.QueryString["action"] != null && Request.QueryString["action"] != "")
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "saved", "<script type=\"text/javascript\">alert('Information Saved Successfully');<" + "/" + "script>");
            if (!IsPostBack)
            {
                if (Request.QueryString["rrid"] != null && Request.QueryString["rrid"] != "")
                {
                    lblResourceWorkflow.Text = Request.QueryString["rrid"];
                    int intResourceWorkflow = Int32.Parse(lblResourceWorkflow.Text);
                    int intResourceParent = oResourceRequest.GetWorkflowParent(intResourceWorkflow);
                    ds = oResourceRequest.Get(intResourceParent);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        int intItem = Int32.Parse(ds.Tables[0].Rows[0]["itemid"].ToString());
                        int intService = Int32.Parse(ds.Tables[0].Rows[0]["serviceid"].ToString());
                        int intApp = oRequestItem.GetItemApplication(intItem);
                        int intUser = Int32.Parse(oResourceRequest.GetWorkflow(intResourceWorkflow, "userid"));
                        int intRequest = Int32.Parse(ds.Tables[0].Rows[0]["requestid"].ToString());
                        int intProject = oRequest.GetProjectNumber(intRequest);
                        bool boolManager = false;
                        if (intProject > 0)
                        {
                            panProject.Visible = true;
                            lblName.Text = oProject.Get(intProject, "name");
                            lblNumber.Text = oProject.Get(intProject, "number");
                            lblOrganization.Text = oOrganization.GetName(Int32.Parse(oProject.Get(intProject, "organization")));
                        }
                        else
                        {
                            panTask.Visible = true;
                            lblTaskName.Text = oResourceRequest.GetWorkflow(intResourceWorkflow, "name");
                            lblTaskNumber.Text = "CVT" + intRequest.ToString();
                        }
                        lblSubmitter.Text = oUser.GetFullName(oRequest.GetUser(intRequest));
                        lblStatement.Text = oRequest.Get(intRequest, "description");
                        if (oRequest.Get(intRequest, "start_date") != "")
                            lblStart.Text = DateTime.Parse(oRequest.Get(intRequest, "start_date")).ToShortDateString();
                        else
                            lblStart.Text = "N / A";
                        if (oRequest.Get(intRequest, "end_date") != "")
                            lblEnd.Text = DateTime.Parse(oRequest.Get(intRequest, "end_date")).ToShortDateString();
                        else
                            lblEnd.Text = "N / A";
                        double dblUsed = oResourceRequest.GetWorkflowUsed(intResourceWorkflow);
                        lblUsed.Text = dblUsed.ToString("F");
                        lblAllocated.Text = oResourceRequest.GetWorkflow(intResourceWorkflow, "allocated");
                        txtAllocated.Text = lblAllocated.Text;
                        txtDevices.Text = oResourceRequest.GetWorkflow(intResourceWorkflow, "devices");
                        lblDevices.Text = txtDevices.Text;
                        if (ds.Tables[0].Rows[0]["solo"].ToString() == "0")
                        {
                            btnView.Attributes.Add("onclick", "return OpenWindow('PRINTER_FRIENDLY','?page=" + intViewRequest.ToString() + "&rid=" + intRequest.ToString() + "');");
                            btnView.Text = "View Original Request Details";
                        }
                        else
                        {
                            btnView.Attributes.Add("onclick", "return OpenWindow('PRINTER_FRIENDLY','?page=" + intViewResourceRequest.ToString() + "&rrid=" + intResourceParent.ToString() + "');");
                            if (oApplication.Get(intApp, "tpm") == "1")
                                btnView.Text = "View Original Request Details";
                            else
                                btnView.Text = "View Original Request Details";
                        }
                        string strStatus = ds.Tables[0].Rows[0]["status"].ToString();
                        int intAppManager = oApplication.GetManager(intApp);
                        LoadList(intAppManager, intApp);
                        LoadAvailable(intAppManager, intApp);
                        if (oUser.IsAdmin(intProfile) || oService.IsManager(intService, intProfile) || (oApplication.IsManager(intApp, intProfile) && oApplication.Get(intApp, "disable_manager") != "1") || (oUser.IsManager(intUser, intProfile, true) && oApplication.Get(intApp, "disable_manager") != "1"))
                            boolManager = true;
                        else if (intProfile == intUser)
                        {
                            panVirtual.Visible = true;
                            btnVirtual.Attributes.Add("onclick", "return OpenWindow('RESOURCE_REQUEST','" + intResourceWorkflow.ToString() + "');");
                            btnVirtual.Text = "View This Request";
                        }
                        try
                        {
                            ddlActivity.SelectedValue = intItem.ToString();
                            lblActivity.Text = ddlActivity.SelectedItem.Text;
                            ddlUser.SelectedValue = intUser.ToString();
                            lblUser.Text = ddlUser.SelectedItem.Text;
                        }
                        catch { }
                        if (boolManager == true)
                        {
                            Master.Page.Title = "Resource Request #" + intResourceWorkflow.ToString() + " Management View";
                            panView.Visible = true;
                            panManager.Visible = true;
                            chkOpen.Enabled = (strStatus == "3" || strStatus == "-2" || strStatus == "5");
                            txtAllocated.Visible = true;
                            txtDevices.Visible = true;
                            ddlActivity.Visible = true;
                            ddlUser.Visible = true;
                            lnkAvailable.Visible = true;
                            panVirtual.Visible = true;
                            btnVirtual.Attributes.Add("onclick", "return OpenWindow('RESOURCE_REQUEST','" + intResourceWorkflow.ToString() + "');");
                            btnVirtual.Text = "Virtual Login as " + oUser.Get(intUser, "fname") + " " + oUser.Get(intUser, "lname");
                            btnSave.Attributes.Add("onclick", "return ValidateNumber('" + txtAllocated.ClientID + "','Please enter a valid number for the allocated hours')" +
                                " && ValidateNumber('" + txtDevices.ClientID + "','Please enter a valid number for the device count')" +
                                " && ValidateDropDown('" + ddlActivity.ClientID + "','Please make a selection for the activity type')" +
                                " && ValidateDropDown('" + ddlUser.ClientID + "','Please make a selection for the assigned technician')" +
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
                lnkAvailable.Attributes.Add("onclick", "return ShowHideAvailable('" + divAvailable.ClientID + "');");
                btnFinish.Attributes.Add("onclick", "return CloseWindow();");
                btnClose.Attributes.Add("onclick", "return CloseWindow();");
                btnDenied.Attributes.Add("onclick", "return CloseWindow();");
            }
        }
        private void LoadList(int _appmanager, int _app)
        {
            ds = oUser.GetManagerReports(_appmanager);
            ddlUser.DataValueField = "userid";
            ddlUser.DataTextField = "username";
            ddlUser.DataSource = ds;
            ddlUser.DataBind();
            ddlUser.Items.Insert(0, new ListItem("-- SELECT --", "0"));
            ds = oRequestItem.GetItems(_app, 0, 1);
            ddlActivity.DataValueField = "itemid";
            ddlActivity.DataTextField = "name";
            ddlActivity.DataSource = ds;
            ddlActivity.DataBind();
            ddlActivity.Items.Insert(0, new ListItem("-- SELECT --", "0"));
        }
        private void LoadAvailable(int _appmanager, int _app)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("name", System.Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("hours", System.Type.GetType("System.Double")));
            dt.Columns.Add(new DataColumn("graph", System.Type.GetType("System.Double")));
            ds = oUser.GetManagerReports(_appmanager);
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
            int intResourceWorkflow = Int32.Parse(lblResourceWorkflow.Text);
            int intResourceParent = oResourceRequest.GetWorkflowParent(intResourceWorkflow);
            int intOldUser = Int32.Parse(oResourceRequest.GetWorkflow(intResourceWorkflow, "userid"));
            int intNewUser = Int32.Parse(ddlUser.SelectedItem.Value);
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
            strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_REQUEST_ASSIGNMENT");
            if (intOldUser != intNewUser)
            {
                string strSpacerRow = "<tr><td colspan=\"3\"><img src=\"" + oVariable.ImageURL() + "/images/spacer.gif\" border=\"0\" width=\"1\" height=\"7\" /></td></tr>";
                string strDefault = oUser.GetApplicationUrl(intNewUser, intWorkload);
                // Notify Old User
                // NOTIFICATION
                oFunction.SendEmail("Request Assignment", oUser.GetName(intOldUser), "", strEMailIdsBCC, "Request Assignment", "<p><b>The following request has been removed from your workload</b><p><p>" + oResourceRequest.GetWorkflowSummary(intResourceWorkflow, intEnvironment, dsnServiceEditor, dsnAsset, dsnIP) + "</p>", true, false);
                // Notify New User
                if (strDefault == "")
                    oFunction.SendEmail("Request Assignment", oUser.GetName(intNewUser), "", strEMailIdsBCC, "Request Assignment", "<p><b>The following request has been re-assigned to you by " + oUser.GetFullName(intProfile) + "</b><p><p>" + oResourceRequest.GetWorkflowSummary(intResourceWorkflow, intEnvironment, dsnServiceEditor, dsnAsset, dsnIP) + "</p>", true, false);
                else
                {
                    if (intProject > 0)
                        oFunction.SendEmail("Request Assignment", oUser.GetName(intNewUser), "", strEMailIdsBCC, "Request Assignment", "<p><b>The following request has been re-assigned to you by " + oUser.GetFullName(intProfile) + "</b><p><p>" + oResourceRequest.GetWorkflowSummary(intResourceWorkflow, intEnvironment, dsnServiceEditor, dsnAsset, dsnIP) + "</p><p><a href=\"" + oVariable.URL() + "/redirect.aspx?referrer=/" + strDefault + oPage.GetFullLink(intWorkload) + "?pid=" + intProject.ToString() + "\" target=\"_blank\">Click here to review your new assignment.</a></p>", true, false);
                    else
                        oFunction.SendEmail("Request Assignment", oUser.GetName(intNewUser), "", strEMailIdsBCC, "Request Assignment", "<p><b>The following request has been re-assigned to you by " + oUser.GetFullName(intProfile) + "</b><p><p>" + oResourceRequest.GetWorkflowSummary(intResourceWorkflow, intEnvironment, dsnServiceEditor, dsnAsset, dsnIP) + "</p><p><a href=\"" + oVariable.URL() + "/redirect.aspx?referrer=/frame/resource_request.aspx?rrid=" + intResourceWorkflow.ToString() + "\" target=\"_blank\">Click here to review your new assignment.</a></p>", true, false);
                }
                string strActivity = "<tr><td><b>Resource:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>" + oUser.GetFullName(intNewUser) + "</td></tr>";
                strActivity += strSpacerRow;
                strActivity += "<tr><td><b>Activity Type:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>" + oService.GetName(intService) + "</td></tr>";
                strActivity = "<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"" + oVariable.DefaultFontStyle() + "\">" + strActivity + "</table>";
                string strDeliverable = oApplication.Get(intApp, "deliverables_doc");
                if (strDeliverable.Trim() != "")
                    strDeliverable = "<p><a href=\"" + oVariable.URL() + strDeliverable + "\">Click here to view the service deliverables</a></p>";
                //            if (oService.Get(intService, "notify_client") != "0")
                //            oFunction.SendEmail("Request Assignment", oUser.GetName(intRequester), "", strBCC, "Request Assignment", "<p><b>A resource has been assigned to the following request...</b><p><p>" + oResourceRequest.GetWorkflowSummary(intResourceWorkflow, intEnvironment, dsnServiceEditor) + "</p><p>" + strActivity + "</p>" + strDeliverable, true, false);
            }
            oResourceRequest.UpdateWorkflow(intResourceWorkflow, Int32.Parse(txtDevices.Text), double.Parse(txtAllocated.Text), (chkOpen.Checked ? 2 : intStatus));
            oResourceRequest.UpdateWorkflowAssigned(intResourceWorkflow, intNewUser);
            oResourceRequest.UpdateItemAndService(intResourceParent, intNewItem, intService);
            if (chkOpen.Checked == true && intProject > 0)
                oProject.Update(intProject, 2);
            Response.Redirect(Request.Path + "?rrid=" + intResourceWorkflow.ToString() + "&save=true");
        }
    }
}
