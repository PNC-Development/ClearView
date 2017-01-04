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
    public partial class servername_mine : System.Web.UI.UserControl
    {

        private DataSet ds;
        protected Pages oPage;
        protected AppPages oAppPage;
        protected Applications oApplication;
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected ServerName oServerName;
        protected string strRedirect = "";
        protected int intProfile;
        protected int intPage = 0;
        protected int intApplication = 0;
        protected int intRecords = 20;
        protected int intRecordStart = 1;
        protected Users oUser;
        protected void Page_Load(object sender, EventArgs e)
        {
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oPage = new Pages(intProfile, dsn);
            oAppPage = new AppPages(intProfile, dsn);
            oApplication = new Applications(intProfile, dsn);
            oUser = new Users(intProfile, dsn);
            oServerName = new ServerName(intProfile, dsn);
            if (Request.QueryString["pageid"] != null && Request.QueryString["pageid"] != "")
                intPage = Int32.Parse(Request.QueryString["pageid"]);
            if (Request.Cookies["application"] != null && Request.Cookies["application"].Value != "")
                intApplication = Int32.Parse(Request.Cookies["application"].Value);
            strRedirect = oPage.GetFullLink(Int32.Parse(oPage.Get(intPage, "related")));
            lblTitle.Text = oPage.Get(intPage, "title");
            oPage.LoadPaging(oServerName.GetMine(intProfile), Request, intPage, lblPage, lblSort, lblTopPaging, lblBottomPaging, txtPage, lblPages, lblRecords, rptView, lblNone);
            foreach (RepeaterItem ri in rptView.Items)
                ((Button)ri.FindControl("btnRelease")).Attributes.Add("onclick", "return confirm('Are you sure you want to release this item?');");
        }
        protected void btnOrder_Click(Object Sender, EventArgs e)
        {
            oPage.btnOrder(Request, Sender, Response, intPage);
        }
        protected void btnPage_Click(Object Sender, ImageClickEventArgs e)
        {
            oPage.btnPage(Request, Response, intPage, txtPage);
        }
        protected void btnRelease_Click(Object Sender, EventArgs e)
        {
            Button oButton = (Button)Sender;
            int intName = Int32.Parse(oButton.CommandArgument);
            switch (oButton.CommandName) 
            {
                case "NCB":
                    oServerName.Update(intName, 1);
                    break;
                case "PNC":
                    oServerName.UpdateFactory(intName, 1);
                    break;
            }
            string strSort = "";
            if (Request.QueryString["sort"] != null)
                strSort = Request.QueryString["sort"];
            string strPage = "1";
            if (Request.QueryString["page"] != null)
                strPage = Request.QueryString["page"];
            Response.Redirect(oPage.GetFullLink(intPage) + "?sort=" + strSort + "&page=" + strPage);
        }
    }
}