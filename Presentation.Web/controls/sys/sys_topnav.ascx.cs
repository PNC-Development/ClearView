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
    public partial class sys_topnav : System.Web.UI.UserControl
    {

        private DataSet ds;
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected Applications oApplication;
        protected Pages oPage;
        protected AppPages oAppPage;
        protected int intProfile = 0;
        protected int intApplication = 0;
        protected int intPage = 0;
        protected string strMenu = "";
        protected string strUser;
        protected string strTime;
        protected bool boolWelcome = false;
        protected bool boolBread = true;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (boolWelcome == true)
            {
                panWelcome.Visible = true;
                Users oUser = new Users(0, dsn);
                intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
                strUser = "Welcome, " + oUser.Get(intProfile, "fname") + " " + oUser.Get(intProfile, "lname");
                strTime = DateTime.Now.ToLongDateString();
            }
            if (boolBread == true)
            {
                panBread.Visible = true;
                string strDivider = " | ";
                intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
                oApplication = new Applications(intProfile, dsn);
                oPage = new Pages(intProfile, dsn);
                oAppPage = new AppPages(intProfile, dsn);
                if (Request.QueryString["applicationid"] != null && Request.QueryString["applicationid"] != "")
                    intApplication = Int32.Parse(Request.QueryString["applicationid"]);
                if (Request.QueryString["pageid"] != null && Request.QueryString["pageid"] != "")
                    intPage = Int32.Parse(Request.QueryString["pageid"]);
                if (Request.Cookies["application"] != null && Request.Cookies["application"].Value != "")
                    intApplication = Int32.Parse(Request.Cookies["application"].Value);
                //            if (intApplication > 0)
                //            {
                ds = oApplication.Get(intApplication);
                if (ds.Tables[0].Rows.Count > 0)
                    strMenu += strDivider + "<a class=\"topnav\" href=\"/" + ds.Tables[0].Rows[0]["url"].ToString() + "/default.aspx\">" + ds.Tables[0].Rows[0]["name"].ToString() + "</a>";
                string strSubMenu = "";
                while (intPage > 0)
                {
                    string strTitle = oPage.Get(intPage, "menutitle");
                    strSubMenu = strDivider + "<a class=\"topnav\" " + oPage.GetHref(intPage) + "\" title=\"" + strTitle + "\">" + strTitle + "</a>" + strSubMenu;
                    intPage = oPage.GetParent(intPage);
                }
                if (strSubMenu != "")
                    strMenu += strSubMenu;
                //            }
            }
        }
        protected void btnLogout_Click(Object Sender, EventArgs e)
        {
            Response.Cookies["profileid"].Value = "";
            Response.Redirect(Request.Path);
        }
        protected void btnHome_Click(Object Sender, EventArgs e)
        {
            Response.Cookies["application"].Value = "";
            Response.Redirect("/index.aspx");
        }
    }
}