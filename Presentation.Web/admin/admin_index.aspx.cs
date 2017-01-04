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

namespace NCC.ClearView.Presentation.Web
{
    public partial class admin_index : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;

        protected int intProfile;
        protected string strRedirect = "pages.aspx";
        protected string strPush = "";
        protected string strMenuTab1 = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Cookies["adminid"] != null && Request.Cookies["adminid"].Value != "")
                intProfile = Int32.Parse(Request.Cookies["adminid"].Value);
            else
                Response.Redirect("/admin/login.aspx");
            if (Request.QueryString["referrer"] != null && Request.QueryString["referrer"] != "")
                strRedirect = Request.QueryString["referrer"];
            else if (Request.Cookies["loginreferrer"] != null && Request.Cookies["loginreferrer"].Value != "")
                strRedirect = Request.Cookies["loginreferrer"].Value;
            Enhancements oEnhancement = new Enhancements(0, dsn);
            DataSet ds = oEnhancement.GetVersions(1);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (dr["compiled"].ToString() != "")
                    strPush += "<tr><td>" + dr["name"].ToString() + "</td><td>" + DateTime.Parse(dr["compiled"].ToString()).ToShortDateString() + "</td><td class=\"approved\">Success</td></tr>";
            }
            Tab oTab = new Tab("", 0, "divMenu1", false, true);
            oTab.AddTab("Functions", "");
            oTab.AddTab("VMware", "");
            oTab.AddTab("General", "");
            oTab.AddTab("Asset", "");
            oTab.AddTab("Proj / Req", "");
            oTab.AddTab("Design", "");
            oTab.AddTab("Forecast", "");
            strMenuTab1 = oTab.GetTabs();
        }
    }
}
