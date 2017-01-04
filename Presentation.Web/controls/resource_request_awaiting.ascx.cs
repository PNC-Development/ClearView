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
    public partial class resource_request_awaiting : System.Web.UI.UserControl
    {

        private DataSet ds;
        protected Pages oPage;
        protected AppPages oAppPage;
        protected Applications oApplication;
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intAssignPage = Int32.Parse(ConfigurationManager.AppSettings["AssignWorkload"]);
        protected ResourceRequest oResourceRequest;
        protected string strRedirect = "";
        protected int intProfile;
        protected int intPage = 0;
        protected int intApplication = 0;
        protected int intRecords = 20;
        protected int intRecordStart = 1;
        protected Users oUser;
        protected Functions oFunction;
        protected void Page_Load(object sender, EventArgs e)
        {
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oPage = new Pages(intProfile, dsn);
            oAppPage = new AppPages(intProfile, dsn);
            oApplication = new Applications(intProfile, dsn);
            oUser = new Users(intProfile, dsn);
            oResourceRequest = new ResourceRequest(intProfile, dsn);
            oFunction = new Functions(intProfile, dsn, intEnvironment);
            if (Request.QueryString["pageid"] != null && Request.QueryString["pageid"] != "")
                intPage = Int32.Parse(Request.QueryString["pageid"]);
            if (Request.Cookies["application"] != null && Request.Cookies["application"].Value != "")
                intApplication = Int32.Parse(Request.Cookies["application"].Value);
            oFunction.Alert(Page, "assigned", "The technician has been assigned and an email notification was sent", "Request Assigned");
            strRedirect = oPage.GetFullLink(intAssignPage);
            lblTitle.Text = oPage.Get(intPage, "title");
            DataSet ds = oResourceRequest.GetAwaiting(intProfile);
            if (Request.QueryString["all"] != null)
            {
                ds = oResourceRequest.GetAwaitingBuddy(intProfile);
                ddlType.SelectedValue = "1";
            }
            oPage.LoadPaging(ds, Request, intPage, lblPage, lblSort, lblTopPaging, lblBottomPaging, txtPage, lblPages, lblRecords, rptView, lblNone);
            ddlType.Attributes.Add("onchange", "LoadWait();");
        }
        protected void btnOrder_Click(Object Sender, EventArgs e)
        {
            oPage.btnOrder(Request, Sender, Response, intPage);
        }
        protected void btnPage_Click(Object Sender, ImageClickEventArgs e)
        {
            oPage.btnPage(Request, Response, intPage, txtPage);
        }
        protected void ddlType_Change(Object Sender, EventArgs e)
        {
            string strSort = "";
            if (Request.QueryString["sort"] != null)
                strSort = Request.QueryString["sort"];
            string strPage = "";
            if (Request.QueryString["page"] != null)
                strPage = Request.QueryString["page"];
            if (strPage == "")
                strPage = "1";
            if (ddlType.SelectedIndex > 0)
                Response.Redirect(oPage.GetFullLink(intPage) + "?page=" + strPage + "&sort=" + strSort + "&all=true");
            else
                Response.Redirect(oPage.GetFullLink(intPage) + "?page=" + strPage + "&sort=" + strSort);
        }
    }
}