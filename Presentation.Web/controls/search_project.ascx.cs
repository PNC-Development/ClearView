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
using System.Text;

namespace NCC.ClearView.Presentation.Web
{
    public partial class search_project : System.Web.UI.UserControl
    {
        private DataSet ds;
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intWorkload = Int32.Parse(ConfigurationManager.AppSettings["ViewWorkload"]);
        protected int intWorkloadManager = Int32.Parse(ConfigurationManager.AppSettings["ViewWorkloadManager"]);
        protected int intProfile;
        protected int intApplication = 0;
        protected int intPage = 0;
        protected Pages oPage;
        protected Organizations oOrganization;
        protected Segment oSegment;
        protected Applications oApplication;
        protected RequestItems oRequestItem;
        protected Services oService;
        protected Search oSearch;
        protected Users oUser;
        protected ServiceRequests oServiceRequest;
        protected ResourceRequest oResourceRequest;
        protected Projects oProject;
        protected StatusLevels oStatusLevel;
        protected ProjectRequest oProjectRequest;
        protected ProjectRequest_Approval oApprove;
        protected Documents oDocument;
        protected StatusLevels oStatus;
        protected Customized oCustomized;
        protected Variables oVariable;
        protected Functions oFunction;
        protected int intRecords = 20;
        protected int intRecordStart = 1;
        protected string strProject = "";
        protected string strDetails = "";
        protected string strRequest = "";
        protected string strPriority = "";
        protected string strDocuments = "";
        protected string strProjectName = "";
        protected string strMenuTab1 = "";
        private string strEMailIdsBCC = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oPage = new Pages(intProfile, dsn);
            oOrganization = new Organizations(intProfile, dsn);
            oSegment = new Segment(intProfile, dsn);
            oApplication = new Applications(intProfile, dsn);
            oRequestItem = new RequestItems(intProfile, dsn);
            oService = new Services(intProfile, dsn);
            oSearch = new Search(intProfile, dsn);
            oUser = new Users(intProfile, dsn);
            oServiceRequest = new ServiceRequests(intProfile, dsn);
            oResourceRequest = new ResourceRequest(intProfile, dsn);
            oProject = new Projects(intProfile, dsn);
            oStatusLevel = new StatusLevels();
            oProjectRequest = new ProjectRequest(intProfile, dsn);
            oDocument = new Documents(intProfile, dsn);
            oStatus = new StatusLevels();
            oApprove = new ProjectRequest_Approval(intProfile, dsn, intEnvironment);
            oCustomized = new Customized(intProfile, dsn);
            oVariable = new Variables(intEnvironment);
            oFunction = new Functions(intProfile, dsn, intEnvironment);
            if (Request.QueryString["applicationid"] != null && Request.QueryString["applicationid"] != "")
                intApplication = Int32.Parse(Request.QueryString["applicationid"]);
            if (Request.QueryString["pageid"] != null && Request.QueryString["pageid"] != "")
                intPage = Int32.Parse(Request.QueryString["pageid"]);
            if (Request.Cookies["application"] != null && Request.Cookies["application"].Value != "")
                intApplication = Int32.Parse(Request.Cookies["application"].Value);
            bool boolUserSearch = false;
            if (Request.QueryString["pid"] != null && Request.QueryString["pid"] != "")
            {
                int intProject = Int32.Parse(Request.QueryString["pid"]);
                strProjectName = oProject.GetName(intProject);
                lblProjectName.Text = "&quot;" + strProjectName + "&quot;";
                // INVOLVEMENT
                DataSet dsInvolvement = oResourceRequest.GetWorkflowProject(intProject);
                int intOldUser = 0;
                foreach (DataRow dr in dsInvolvement.Tables[0].Rows)
                {
                    if (intOldUser == Int32.Parse(dr["userid"].ToString()))
                        dr.Delete();
                    else
                        intOldUser = Int32.Parse(dr["userid"].ToString());
                }
                dsInvolvement = oResourceRequest.GetWorkflowProject(intProject);
                int intOldItem = 0;
                intOldUser = 0;
                foreach (DataRow dr in dsInvolvement.Tables[0].Rows)
                {
                    if (intOldItem == Int32.Parse(dr["itemid"].ToString()) && intOldUser == Int32.Parse(dr["userid"].ToString()))
                        dr.Delete();
                    else
                    {
                        intOldItem = Int32.Parse(dr["itemid"].ToString());
                        intOldUser = Int32.Parse(dr["userid"].ToString());
                    }
                }
                rptInvolvement.DataSource = dsInvolvement;
                rptInvolvement.DataBind();
                lblNoInvolvement.Visible = (rptInvolvement.Items.Count == 0);
                foreach (RepeaterItem ri in rptInvolvement.Items)
                {
                    Label _id = (Label)ri.FindControl("lblId");
                    Label _user = (Label)ri.FindControl("lblUser");
                    Label _status = (Label)ri.FindControl("lblStatus");
                    Label _image = (Label)ri.FindControl("lblImage");
                    int intStatus = Int32.Parse(_status.Text);
                    int intUser = Int32.Parse(_user.Text);
                    _user.Text = oUser.GetFullName(intUser);
                    Label _item = (Label)ri.FindControl("lblItem");
                    int intItem2 = Int32.Parse(_item.Text);
                    Label _allocated = (Label)ri.FindControl("lblAllocated");
                    Label _used = (Label)ri.FindControl("lblUsed");
                    double dblAllocated = oResourceRequest.GetAllocated(intProject, intUser, intItem2);
                    double dblUsed = oResourceRequest.GetUsed(intProject, intUser, intItem2);
                    Label _percent = (Label)ri.FindControl("lblPercent");
                    _allocated.Text = dblAllocated.ToString();
                    _used.Text = dblUsed.ToString();
                    if (dblAllocated > 0)
                    {
                        dblUsed = dblUsed / dblAllocated;
                        _percent.Text = dblUsed.ToString("P");
                    }
                    else
                        _percent.Text = dblAllocated.ToString("P");
                    bool boolTPMDone = false;
                    if (intItem2 == 0)
                        _item.Text = "Project Coordinator";
                    else if (intItem2 == -1)
                        _item.Text = "Design Implementation (Pending Execution)";
                    else
                    {
                        int intApp = oRequestItem.GetItemApplication(intItem2);
                        _item.Text = oApplication.GetName(intApp);
                    }
                    _status.Text = oStatus.Name(intStatus);
                    if (boolTPMDone == true)
                        _status.Text = "Closed";
                }
            }
        }
    }
}