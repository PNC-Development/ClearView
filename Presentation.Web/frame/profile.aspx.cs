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
    public partial class profile : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);

        protected Users oUser;
        protected void Page_Load(object sender, EventArgs e)
        {
            oUser = new Users(0, dsn);
            int intUser = 0;
            if (Request.QueryString["userid"] != null && Request.QueryString["userid"] != "")
                intUser = Int32.Parse(Request.QueryString["userid"]);
            else if (Request.QueryString["pid"] != null && Request.QueryString["pid"] != "")
            {
                Platforms oPlatform = new Platforms(0, dsn);
                int intPlatform = Int32.Parse(Request.QueryString["pid"]);
                intUser = oPlatform.GetManager(intPlatform);
            }
            if (intUser > 0)
            {
                DataSet ds = oUser.Get(intUser);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    lblName.Text = ds.Tables[0].Rows[0]["fname"].ToString() + " " + ds.Tables[0].Rows[0]["lname"].ToString();
                    lblPhone.Text = ds.Tables[0].Rows[0]["phone"].ToString();
                    lblPager.Text = ds.Tables[0].Rows[0]["pager"].ToString();
                    string strEmail = oUser.GetEmail(ds.Tables[0].Rows[0]["xid"].ToString(), intEnvironment).ToLower();
                    lblEmail.Text = "<a href=\"mailto:" + strEmail + "\">" + strEmail + "</a>";
                    int intManager = Int32.Parse(ds.Tables[0].Rows[0]["manager"].ToString());
                    lblManager.Text = "<a href=\"" + Request.Path + "?userid=" + intManager.ToString() + "\">" + oUser.GetFullName(intManager) + "</a>";
                    lblSkills.Text = ds.Tables[0].Rows[0]["other"].ToString();
                    imgPicture.ImageUrl = "/frame/picture.aspx?xid=" + ds.Tables[0].Rows[0]["xid"].ToString();
                    imgPicture.Style["border"] = "solid 1px #999999";
                }
            }
        }
    }
}
