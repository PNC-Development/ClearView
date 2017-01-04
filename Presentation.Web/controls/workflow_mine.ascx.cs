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
    public partial class workflow_mine : System.Web.UI.UserControl
    {

        private DataSet ds;
        protected Pages oPage;
        protected AppPages oAppPage;
        protected Applications oApplication;
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intWorkflowPage = Int32.Parse(ConfigurationManager.AppSettings["WorkflowSuffix"]);
        protected string strWorkflowBCC = ConfigurationManager.AppSettings["WorkflowBCC"];
        protected string strRedirect = "";
        protected int intProfile;
        protected int intPage = 0;
        protected int intApplication = 0;
        protected int intRecords = 20;
        protected int intRecordStart = 1;
        protected ProjectRequest oProjectRequest;
        protected ProjectRequest_Approval oApprove;
        protected Users oUser;
        protected void Page_Load(object sender, EventArgs e)
        {
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oProjectRequest = new ProjectRequest(intProfile, dsn);
            oApprove = new ProjectRequest_Approval(intProfile, dsn, intEnvironment);
            oPage = new Pages(intProfile, dsn);
            oAppPage = new AppPages(intProfile, dsn);
            oApplication = new Applications(intProfile, dsn);
            oUser = new Users(intProfile, dsn);
            if (Request.QueryString["pageid"] != null && Request.QueryString["pageid"] != "")
                intPage = Int32.Parse(Request.QueryString["pageid"]);
            if (Request.Cookies["application"] != null && Request.Cookies["application"].Value != "")
                intApplication = Int32.Parse(Request.Cookies["application"].Value);
            strRedirect = oPage.GetFullLink(intWorkflowPage);
            lblTitle.Text = oPage.Get(intPage, "title");
            oPage.LoadPaging(oApprove.GetAwaiting(intProfile), Request, intPage, lblPage, lblSort, lblTopPaging, lblBottomPaging, txtPage, lblPages, lblRecords, rptView, lblNone);
        }
        protected void btnOrder_Click(Object Sender, EventArgs e)
        {
            oPage.btnOrder(Request, Sender, Response, intPage);
        }
        protected void btnPage_Click(Object Sender, ImageClickEventArgs e)
        {
            oPage.btnPage(Request, Response, intPage, txtPage);
        }
    }
}
