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

namespace NCC.ClearView.Presentation.Web
{
    public partial class print_search : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intProfile;
        protected void Page_Load(object sender, EventArgs e)
        {
            AuthenticateUser();
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            if (Request.QueryString["sid"] != null && Request.QueryString["sid"] != "")
                LoadSearch(Int32.Parse(Request.QueryString["sid"]));
        }
        protected void LoadSearch(int _search)
        {
            Search oSearch = new Search(intProfile, dsn);
            RequestItems oRequestItem = new RequestItems(intProfile, dsn);
            Services oService = new Services(intProfile, dsn);
            Applications oApplication = new Applications(intProfile, dsn);
            StatusLevels oStatusLevel = new StatusLevels();
            Users oUser = new Users(intProfile, dsn);
            Projects oProject = new Projects(intProfile, dsn);
            ResourceRequest oResourceRequest = new ResourceRequest(intProfile, dsn);
            ServiceRequests oServiceRequest = new ServiceRequests(intProfile, dsn);
            DataSet ds = oSearch.GetProject(_search);
            int intApplication = 0;
            if (Request.QueryString["applicationid"] != null && Request.QueryString["applicationid"] != "")
                intApplication = Int32.Parse(Request.QueryString["applicationid"]);
            if (Request.Cookies["application"] != null && Request.Cookies["application"].Value != "")
                intApplication = Int32.Parse(Request.Cookies["application"].Value);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["userid"].ToString() == intProfile.ToString())
                {
                    string strType = ds.Tables[0].Rows[0]["type"].ToString();
                    string strClause = "cv_projects.deleted = 0 AND cv_projects.number NOT LIKE 'CV%' AND cv_projects.number NOT LIKE 'EPS%'";
                    string strOr1 = "";
                    string strOr2 = "";
                    string strSQL = "SELECT DISTINCT '?pid=' + CAST(cv_projects.projectid AS varchar(10)) + '&sid=" + _search.ToString() + "' AS query, cv_projects.name, cv_projects.number, cv_projects.status, cv_projects.modified FROM cv_projects INNER JOIN cv_requests LEFT OUTER JOIN cv_service_requests ON cv_service_requests.requestid = cv_requests.requestid AND cv_service_requests.deleted = 0 LEFT OUTER JOIN cv_project_requests ON cv_project_requests.requestid = cv_requests.requestid AND cv_project_requests.deleted = 0 INNER JOIN cv_resource_requests INNER JOIN cv_request_items INNER JOIN cv_applications ON cv_request_items.applicationid = cv_applications.applicationid AND cv_applications.deleted = 0 ON cv_request_items.itemid = cv_resource_requests.itemid AND cv_request_items.deleted = 0 INNER JOIN cv_users ON cv_resource_requests.userid = cv_users.userid AND cv_users.deleted = 0 AND cv_users.enabled = 1 ON cv_requests.requestid = cv_resource_requests.requestid AND cv_resource_requests.deleted = 0 ON cv_requests.projectid = cv_projects.projectid AND cv_requests.deleted = 0 WHERE ";
                    string strApplication = " AND cv_request_items.applicationid = " + intApplication.ToString();
                    switch (strType)
                    {
                        case "O":
                            strApplication = "";
                            string strOName = ds.Tables[0].Rows[0]["oname"].ToString().Trim();
                            string strONumber = ds.Tables[0].Rows[0]["onumber"].ToString().Trim();
                            if (strONumber != "")
                            {
                                strClause += " AND cv_projects.number LIKE '%" + strONumber + "%'";
                                lblResults.Text = "Project Number LIKE &quot;" + strONumber + "&quot;";
                            }
                            else
                            {
                                strClause += " AND cv_projects.name LIKE '%" + strOName + "%'";
                                lblResults.Text = "Project Name LIKE &quot;" + strOName + "&quot;";
                            }
                            string strOStatus = ds.Tables[0].Rows[0]["ostatus"].ToString();
                            if (strOStatus != "0")
                            {
                                strClause += " AND cv_projects.status = " + strOStatus;
                                if (lblResults.Text != "")
                                    lblResults.Text += " AND ";
                                lblResults.Text += "Status = &quot;" + oStatusLevel.Name(Int32.Parse(strOStatus)) + "&quot;";
                            }
                            string strOStart = ds.Tables[0].Rows[0]["ostart"].ToString();
                            if (strOStart != "")
                            {
                                strOStart = DateTime.Parse(strOStart).ToShortDateString();
                                strClause += " AND cv_projects.modified > " + strOStart;
                                if (lblResults.Text != "")
                                    lblResults.Text += " AND ";
                                lblResults.Text += "Project Date > &quot;" + strOStart + "&quot;";
                            }
                            string strOEnd = ds.Tables[0].Rows[0]["oend"].ToString();
                            if (strOEnd != "")
                            {
                                strOEnd = DateTime.Parse(strOEnd).ToShortDateString();
                                strClause += " AND cv_projects.modified < " + strOEnd;
                                if (lblResults.Text != "")
                                    lblResults.Text += " AND ";
                                lblResults.Text += "Project Date < &quot;" + strOEnd + "&quot;";
                            }
                            break;
                        case "P":
                            string strDepartment = ds.Tables[0].Rows[0]["department"].ToString();
                            if (strDepartment == "0")
                            {
                                strApplication = "";
                                lblResults.Text += "Department = &quot;ALL&quot;";
                            }
                            else
                            {
                                strApplication = " AND cv_request_items.applicationid = " + strDepartment;
                                lblResults.Text += "Department = &quot;" + oApplication.GetName(Int32.Parse(strDepartment)) + "&quot;";
                            }
                            string strDStatus = ds.Tables[0].Rows[0]["dstatus"].ToString();
                            if (strDStatus != "0")
                            {
                                strClause += " AND cv_resource_requests.status = " + strDStatus;
                                if (lblResults.Text != "")
                                    lblResults.Text += " AND ";
                                lblResults.Text += "Status = &quot;" + oStatusLevel.Name(Int32.Parse(strDStatus)) + "&quot;";
                            }
                            string strDStart = ds.Tables[0].Rows[0]["dstart"].ToString();
                            if (strDStart != "")
                            {
                                strDStart = DateTime.Parse(strDStart).ToShortDateString();
                                strClause += " AND cv_requests.modified > " + strDStart;
                                if (lblResults.Text != "")
                                    lblResults.Text += " AND ";
                                lblResults.Text += "Request Date > &quot;" + strDStart + "&quot;";
                            }
                            string strDEnd = ds.Tables[0].Rows[0]["dend"].ToString();
                            if (strDEnd != "")
                            {
                                strDEnd = DateTime.Parse(strDEnd).ToShortDateString();
                                strClause += " AND cv_requests.modified < " + strDEnd;
                                if (lblResults.Text != "")
                                    lblResults.Text += " AND ";
                                lblResults.Text += "Request Date < &quot;" + strDEnd + "&quot;";
                            }
                            break;
                        case "T":
                            string strTechnician = ds.Tables[0].Rows[0]["technician"].ToString();
                            if (strTechnician != "0")
                            {
                                strClause += " AND cv_resource_requests.userid = " + strTechnician;
                                lblResults.Text += "Technician = &quot;" + oUser.GetFullName(Int32.Parse(strTechnician)) + "&quot;";
                            }
                            string strTStatus = ds.Tables[0].Rows[0]["tstatus"].ToString();
                            if (strTStatus != "0")
                            {
                                strClause += " AND cv_resource_requests.status = " + strTStatus;
                                if (lblResults.Text != "")
                                    lblResults.Text += " AND ";
                                lblResults.Text += "Status = &quot;" + oStatusLevel.Name(Int32.Parse(strTStatus)) + "&quot;";
                            }
                            string strTStart = ds.Tables[0].Rows[0]["tstart"].ToString();
                            if (strTStart != "")
                            {
                                strTStart = DateTime.Parse(strTStart).ToShortDateString();
                                strClause += " AND cv_requests.modified > " + strTStart;
                                if (lblResults.Text != "")
                                    lblResults.Text += " AND ";
                                lblResults.Text += "Request Date > &quot;" + strTStart + "&quot;";
                            }
                            string strTEnd = ds.Tables[0].Rows[0]["tend"].ToString();
                            if (strTEnd != "")
                            {
                                strTEnd = DateTime.Parse(strTEnd).ToShortDateString();
                                strClause += " AND cv_requests.modified < " + strTEnd;
                                if (lblResults.Text != "")
                                    lblResults.Text += " AND ";
                                lblResults.Text += "Request Date < &quot;" + strTEnd + "&quot;";
                            }
                            break;
                        case "L":
                            string strLead = ds.Tables[0].Rows[0]["lead"].ToString();
                            if (strLead != "0")
                            {
                                int intLead = Int32.Parse(strLead);
                                strClause += " AND cv_users.manager = " + intLead.ToString();
                                lblResults.Text += "Team Lead = &quot;" + oUser.GetFullName(intLead) + "&quot;";
                            }
                            string strLStatus = ds.Tables[0].Rows[0]["lstatus"].ToString();
                            if (strLStatus != "0")
                            {
                                strClause += " AND cv_resource_requests.status = " + strLStatus;
                                if (lblResults.Text != "")
                                    lblResults.Text += " AND ";
                                lblResults.Text += "Status = &quot;" + oStatusLevel.Name(Int32.Parse(strLStatus)) + "&quot;";
                            }
                            string strLStart = ds.Tables[0].Rows[0]["lstart"].ToString();
                            if (strLStart != "")
                            {
                                strLStart = DateTime.Parse(strLStart).ToShortDateString();
                                strClause += " AND cv_requests.modified > " + strLStart;
                                if (lblResults.Text != "")
                                    lblResults.Text += " AND ";
                                lblResults.Text += "Request Date < &quot;" + strLStart + "&quot;";
                            }
                            string strLEnd = ds.Tables[0].Rows[0]["lend"].ToString();
                            if (strLEnd != "")
                            {
                                strLEnd = DateTime.Parse(strLEnd).ToShortDateString();
                                strClause += " AND cv_requests.modified < " + strLEnd;
                                if (lblResults.Text != "")
                                    lblResults.Text += " AND ";
                                lblResults.Text += "Request Date > &quot;" + strLEnd + "&quot;";
                            }
                            break;
                        case "G":
                            string strItem = ds.Tables[0].Rows[0]["itemid"].ToString();
                            if (strItem != "0")
                            {
                                strClause += " AND cv_resource_requests.itemid = " + strItem;
                                lblResults.Text += "Activity Type = &quot;" + oRequestItem.GetItemName(Int32.Parse(strItem)) + "&quot;";
                            }
                            string strGStatus = ds.Tables[0].Rows[0]["gstatus"].ToString();
                            if (strGStatus != "0")
                            {
                                strClause += " AND cv_resource_requests.status = " + strGStatus;
                                if (lblResults.Text != "")
                                    lblResults.Text += " AND ";
                                lblResults.Text += "Status = &quot;" + oStatusLevel.Name(Int32.Parse(strGStatus)) + "&quot;";
                            }
                            string strGStart = ds.Tables[0].Rows[0]["gstart"].ToString();
                            if (strGStart != "")
                            {
                                strGStart = DateTime.Parse(strGStart).ToShortDateString();
                                strClause += " AND cv_requests.modified > " + strGStart;
                                if (lblResults.Text != "")
                                    lblResults.Text += " AND ";
                                lblResults.Text += "Request Date < &quot;" + strGStart + "&quot;";
                            }
                            string strGEnd = ds.Tables[0].Rows[0]["gend"].ToString();
                            if (strGEnd != "")
                            {
                                strGEnd = DateTime.Parse(strGEnd).ToShortDateString();
                                strClause += " AND cv_requests.modified < " + strGEnd;
                                if (lblResults.Text != "")
                                    lblResults.Text += " AND ";
                                lblResults.Text += "Request Date > &quot;" + strGEnd + "&quot;";
                            }
                            break;
                        case "S":
                            break;
                    }
                    strClause = strClause + strApplication;
                    if (strOr1 != "" && strOr2 != "")
                        strClause = strClause + " AND " + strOr1 + " AND cv_project_requests.id IS NOT NULL OR " + strClause + " AND " + strOr1 + " AND cv_service_requests.id IS NOT NULL OR " + strClause + " AND " + strOr2 + " AND cv_project_requests.id IS NOT NULL OR " + strClause + " AND " + strOr2 + " AND cv_service_requests.id IS NOT NULL";
                    else
                        strClause = strClause + " AND cv_project_requests.id IS NOT NULL OR " + strClause + " AND cv_service_requests.id IS NOT NULL";
                    ds = SqlHelper.ExecuteDataset(dsn, CommandType.Text, strSQL + strClause);
                    DataView dv = ds.Tables[0].DefaultView;
                    if (Request.QueryString["sort"] != null)
                        dv.Sort = Request.QueryString["sort"];
                    rptView.DataSource = dv;
                    rptView.DataBind();
                    foreach (RepeaterItem ri in rptView.Items)
                    {
                        Label _progress = (Label)ri.FindControl("lblProgress");
                        Label _status = (Label)ri.FindControl("lblStatus");
                        _status.Text = oStatusLevel.HTML(Int32.Parse(_status.Text));
                        Label _updated = (Label)ri.FindControl("lblUpdated");
                        Label _number = (Label)ri.FindControl("lblNumber");
                        if (_number.Text == "")
                            _number.Text = "<i>TBD...</i>";
                        double dblUsed = 0.00;
                        double dblAllocated = 0.00;
                        if (_progress.Text.Contains("?pid=") == true)
                        {
                            int intProject = 0;
                            if (Request.QueryString["sid"] != null && Request.QueryString["sid"] != "")
                            {
                                string strProgress = _progress.Text.Substring(0, _progress.Text.IndexOf("&"));
                                intProject = Int32.Parse(strProgress.Substring(5));
                            }
                            else
                                intProject = Int32.Parse(_progress.Text.Substring(5));
                            _updated.Text = oProject.GetLastUpdated(intProject);
                            DataSet dsAll = oResourceRequest.GetWorkflowProject(intProject);
                            foreach (DataRow drAll in dsAll.Tables[0].Rows)
                            {
                                int intResourceWorkflow = Int32.Parse(drAll["id"].ToString());
                                dblAllocated += double.Parse(oResourceRequest.GetWorkflow(intResourceWorkflow, "allocated"));
                                dblUsed += oResourceRequest.GetWorkflowUsed(intResourceWorkflow);
                            }
                        }
                        else if (_progress.Text.Contains("?rid=") == true)
                        {
                            int intRequest = Int32.Parse(_progress.Text.Substring(5));
                            DataSet dsAll = oResourceRequest.GetWorkflowRequestAll(intRequest);
                            foreach (DataRow drAll in dsAll.Tables[0].Rows)
                            {
                                int intResourceWorkflow = Int32.Parse(drAll["id"].ToString());
                                dblAllocated += double.Parse(oResourceRequest.GetWorkflow(intResourceWorkflow, "allocated"));
                                dblUsed += oResourceRequest.GetWorkflowUsed(intResourceWorkflow);
                            }
                        }
                        double dblProgress = 0.00;
                        if (dblAllocated > 0.00)
                            dblProgress = (dblUsed / dblAllocated) * 100.00;
                        _progress.Text = oServiceRequest.GetStatusBar(dblProgress, "100", "6", true);
                    }
                    lblNone.Visible = (rptView.Items.Count == 0);
                }
            }
        }
    }
}
