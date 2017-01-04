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
    public partial class catalog : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);

        protected string strUser = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            strUser = Request.ServerVariables["logon_user"];
            strUser = strUser.Substring(strUser.LastIndexOf("\\") + 1);
            Users oUser = new Users(0, dsn);
            int intUser = oUser.GetId(strUser);
            if (intUser > 0)
            {
                if (Request.QueryString["pageid"] != null)
                {
                    if (Request.QueryString["redirect"] != null)
                    {
                        Pages oPage = new Pages(0, dsn);
                        int intPage = Int32.Parse(Request.QueryString["pageid"]);
                        string strRedirect = oUser.GetApplicationUrl(intUser, intPage);
                        if (strRedirect == "")
                        {
                            panWait.Visible = false;
                            panAccess.Visible = true;
                        }
                        else
                        {
                            strRedirect = "/" + strRedirect + "/" + oPage.GetFullLink(intPage);
                            Response.Cookies["profileid"].Value = intUser.ToString();
                            Response.Redirect(strRedirect);
                        }
                    }
                }
                else
                {
                    panWait.Visible = false;
                    panPage.Visible = true;
                }
            }
            else
            {
                panWait.Visible = false;
                panUser.Visible = true;
            }
        }
    }
}
