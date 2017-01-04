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
    public partial class redirect : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;

        protected string strTitle = ConfigurationManager.AppSettings["appTitle"];
        protected void Page_Load(object sender, EventArgs e)
        {
            Control oControl;
            Page.Title = strTitle;
            if (Request.Cookies["profileid"] == null || Request.Cookies["profileid"].Value == "")
            {
                oControl = (Control)LoadControl("/controls/sys/sys_login.ascx");
                PH3.Controls.Add(oControl);
                if (Request.QueryString["referrer"] != null)
                {
                    string strReferrer = "";
                    foreach (string strName in Request.QueryString)
                    {
                        if (strName != "referrer")
                            strReferrer += "&" + strName + "=" + Request.QueryString[strName];
                    }
                    Response.Cookies["userloginreferrer"].Value = Request.QueryString["referrer"] + strReferrer;
                }
            }
            else
            {
                Users oUser = new Users(0, dsn);
                oUser.AddLogin(oUser.GetName(Int32.Parse(Request.Cookies["profileid"].Value)));
                if (Request.QueryString["referrer"] != null)
                {
                    string strReferrer = "";
                    foreach (string strName in Request.QueryString)
                    {
                        if (strName != "referrer")
                            strReferrer += "&" + strName + "=" + Request.QueryString[strName];
                    }
                    Response.Redirect(Request.QueryString["referrer"] + strReferrer);
                }
                else
                    Response.Redirect("/index.aspx");
            }
            oControl = (Control)LoadControl("/controls/sys/sys_rotator_header.ascx");
            PH4.Controls.Add(oControl);
        }
    }
}
