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
    public partial class wm_report : System.Web.UI.UserControl
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
        protected int intApplication = 0;
        protected int intPage = 0;
        protected int intProject = 0;
        protected bool boolDetails = false;
        protected bool boolExecution = false;
        protected bool boolChange = false;
        protected bool boolExtension = false;
        protected int intRequest = 0;
        protected int intItem = 0;
        protected int intNumber = 0;
        protected bool boolCheckboxes = false;
        protected string strCheckboxes = "";
        protected string strView = "";
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
                    lblUpdated.Text = oResourceRequest.GetUpdated(intResourceWorkflow);
                    if (dblAllocated == dblUsed)
                    {
                        if (boolComplete == false)
                        {
                            oFunction.ConfigureToolButton(btnComplete, "/images/tool_complete");
                            btnComplete.Attributes.Add("onclick", "return confirm('Are you sure you want to mark this as completed and remove it from your workload?');");
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
                    dblUsed = (dblUsed / dblAllocated) * 100;
                    strCheckboxes = oServiceDetail.LoadCheckboxes(intRequest, intItem, intNumber, intResourceWorkflow, intService);
                    if (oService.Get(intService, "tasks") != "1" || strCheckboxes == "")
                    {
                        panSlider.Visible = true;
                        sldHours._StartPercent = dblUsed.ToString();
                        sldHours._TotalHours = dblAllocated.ToString();
                    }
                    else
                        panCheckboxes.Visible = true;
                    intProject = oRequest.GetProjectNumber(intRequest);
                    hdnTab.Value = "D";
                    panWorkload.Visible = true;
                    LoadStatus(intResourceWorkflow);
                    LoadInformation(intResourceWorkflow);

                    btnDenied.Attributes.Add("onclick", "return CloseWindow();");
                    oFunction.ConfigureToolButton(btnSave, "/images/tool_save");
                    oFunction.ConfigureToolButton(btnPrint, "/images/tool_print");
                    btnPrint.Attributes.Add("onclick", "return PrintWindow();");
                    oFunction.ConfigureToolButton(btnClose, "/images/tool_close");
                    btnClose.Attributes.Add("onclick", "return ExitWindow();");
                    btnSave.Attributes.Add("onclick", "return ValidateStatus('" + ddlStatus.ClientID + "','" + txtComments.ClientID + "');");
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

        private void LoadInformation(int _request)
        {
            StringBuilder sb = new StringBuilder();
            Reports oReport = new Reports(intProfile, dsn);
            Variables oVariable = new Variables(intEnvironment);
            ds = oReport.GetOrderReport(intRequest, intItem, intNumber);

            sb.Append("<table width=\"100%\" cellpadding=\"2\" cellspacing=\"3\" border=\"0\">");

            if (ds.Tables[0].Rows.Count > 0)
            {
                sb.Append("<tr><td valign=\"top\"><b>Title: </b></td>");
                sb.Append("<td>");
                sb.Append(ds.Tables[0].Rows[0]["title"]);
                sb.Append("</td></tr>");

                sb.Append("<tr><td valign=\"top\"><b>Data Source: </b></td>");
                sb.Append("<td>");
                sb.Append(ds.Tables[0].Rows[0]["data_source"]);
                sb.Append("</td></tr>");

                sb.Append("<tr><td valign=\"top\"><b>Chart Type: </b></td>");
                sb.Append("<td>");
                sb.Append(ds.Tables[0].Rows[0]["chart_type"]);
                sb.Append("</td></tr>");

                sb.Append("<tr><td valign=\"top\"><b>Report Upload: </b></td>");

                if ((string)ds.Tables[0].Rows[0]["report_upload"] != DBNull.Value.ToString())
                {
                    sb.Append("<td><a href=\"");
                    sb.Append(oVariable.URL());
                    sb.Append("/");
                    sb.Append(ds.Tables[0].Rows[0]["report_upload"]);
                    sb.Append("\" target=\"_blank\">Click here to view the document</a></td></tr>");
                }

                sb.Append("<tr><td valign=\"top\"><b>Instructions: </b></td>");
                sb.Append("<td>");
                sb.Append(ds.Tables[0].Rows[0]["instructions"]);
                sb.Append("</td></tr>");

                sb.Append("<tr><td valign=\"top\"><b>Data Exclusion: </b></td>");
                sb.Append("<td>");
                sb.Append(ds.Tables[0].Rows[0]["data_exclusion"]);
                sb.Append("</td></tr>");

                int intId = Int32.Parse(ds.Tables[0].Rows[0]["id"].ToString());
                DataSet ds2 = oReport.GetOrderReportDataFields(intId);
                DataSet ds3 = oReport.GetOrderReportCalculations(intId);
                DataSet ds4 = oReport.GetOrderReportApplications(intId);

                sb.Append("<tr><td valign=\"top\"><b>Data Fields:</b></td>");
                sb.Append("<td colspan=\"2\"><table width=\"50%\" cellpadding=\"2\" cellspacing=\"0\">");
                sb.Append("<tr bgcolor=\"#EEEEEE\"><td><b>Field Name</b></td><td><b>Field Type</b></td></tr>");

                string strRep1 = "";
                foreach (DataRow dr in ds2.Tables[0].Rows)
                {
                    strRep1 += "<tr><td>" + dr["name"] + "</td><td>" + dr["type"] + "</td></tr>";
                }
                if (strRep1 == "")
                {
                    sb.Append("<tr><td colspan=\"2\"><img src=\"/images/alert.gif\" border=\"0\" align=\"absmiddle\"> There are no items </td></tr></table></td></tr>");
                }
                else
                {
                    sb.Append(strRep1);
                    sb.Append("</table></td></tr>");
                }

                sb.Append("<tr><td valign=\"top\"><b>Calculations:</b></td>");
                sb.Append("<td colspan=\"2\"><table width=\"50%\" cellpadding=\"2\" cellspacing=\"0\">");
                sb.Append("<tr bgcolor=\"#EEEEEE\"><td><b>Field Name</b></td><td><b>Formula</b></td></tr>");

                string strRep2 = "";
                foreach (DataRow dr in ds3.Tables[0].Rows)
                {
                    strRep2 += "<tr><td>" + dr["name"] + "</td><td>" + dr["formula"] + "</td></tr>";
                }
                if (strRep2 == "")
                {
                    sb.Append("<tr><td colspan=\"2\"><img src=\"/images/alert.gif\" border=\"0\" align=\"absmiddle\"> There are no items </td></tr></table></td></tr>");
                }
                else
                {
                    sb.Append(strRep2);
                    sb.Append("</table></td></tr>");
                }

                sb.Append("<tr><td valign=\"top\"><b>Who can view this report?:</b></td><td><span>");

                foreach (DataRow dr in ds4.Tables[0].Rows)
                {
                    sb.Append(dr["appname"]);
                    sb.Append("; <br>");
                }

                sb.Append("</span></td></tr>");
            }

            sb.Append("</table>");

            strView = sb.ToString();

            if (intProject > 0)
            {
                lblName.Text = oProject.Get(intProject, "name");
                lblNumber.Text = oProject.Get(intProject, "number");
                lblType.Text = "Project";
            }
            else
            {
                lblName.Text = oResourceRequest.GetWorkflow(_request, "name");
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
                    case "X":
                        boolExtension = true;
                        break;
                }
            }
            if (boolDetails == false && boolExecution == false && boolChange == false && boolExtension == false)
                boolDetails = true;
        }
        protected void btnStatus_Click(Object Sender, EventArgs e)
        {
            int intResourceWorkflow = Int32.Parse(lblResourceWorkflow.Text);
            int intResourceParent = oResourceRequest.GetWorkflowParent(intResourceWorkflow);
            oResourceRequest.AddStatus(intResourceWorkflow, Int32.Parse(ddlStatus.SelectedItem.Value), txtComments.Text, intProfile);
            Response.Redirect(Request.Path + "?rrid=" + intResourceWorkflow.ToString() + "&div=E&status=true");
        }

        protected void btnDeleteChange_Click(Object Sender, EventArgs e)
        {
            int intResourceWorkflow = Int32.Parse(lblResourceWorkflow.Text);
            LinkButton oButton = (LinkButton)Sender;
            oResourceRequest.DeleteChangeControl(Int32.Parse(oButton.CommandArgument));
            Response.Redirect(Request.Path + "?rrid=" + intResourceWorkflow.ToString() + "&div=C");
        }
        protected void btnReason_Click(Object Sender, EventArgs e)
        {
            int intResourceWorkflow = Int32.Parse(lblResourceWorkflow.Text);
            Response.Redirect(Request.Path + "?rrid=" + intResourceWorkflow.ToString() + "&div=X&reason=true");
        }
        protected void btnSave_Click(Object Sender, EventArgs e)
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
            double dblHours = 0.00;
            if (panSlider.Visible == true)
            {
                if (Request.Form["hdnHours"] != null && Request.Form["hdnHours"] != "")
                    dblHours = double.Parse(Request.Form["hdnHours"]);
                double dblUsed = oResourceRequest.GetWorkflowUsed(intResourceWorkflow);
                dblHours = (dblHours - dblUsed);
                if (dblHours > 0.00)
                    oResourceRequest.UpdateWorkflowHours(intResourceWorkflow, dblHours);
            }
            else
            {
                oServiceDetail.UpdateCheckboxes(Request, intResourceWorkflow, intRequest, intItem, intNumber);
                double dblAllocated = oResourceRequest.GetDetailsHoursUsed(intRequest, intItem, intNumber, intResourceWorkflow, false);
                oResourceRequest.UpdateWorkflowAllocated(intResourceWorkflow, dblAllocated);
            }
            oResourceRequest.UpdateWorkflowName(intResourceWorkflow, txtCustom.Text, intProfile);
            Response.Redirect(Request.Path + "?rrid=" + intResourceWorkflow.ToString() + "&div=E&save=true");
        }
        protected void btnComplete_Click(Object Sender, EventArgs e)
        {
            int intResourceWorkflow = Int32.Parse(lblResourceWorkflow.Text);
            int intResourceParent = oResourceRequest.GetWorkflowParent(intResourceWorkflow);
            oResourceRequest.UpdateWorkflowStatus(intResourceWorkflow, 3, true);
            oResourceRequest.CloseWorkflow(intResourceWorkflow, intEnvironment,  0, dsnServiceEditor, true, intResourceRequestApprove, intAssignPage, intViewPage, dsnAsset, dsnIP);
            Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "reload", "<script type=\"text/javascript\">RefreshOpeningWindow();window.close();<" + "/" + "script>");
        }
    }
}