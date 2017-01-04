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
    public partial class AdminLogin : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected Users oUser;
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cookies["adminid"].Value = "";
            oUser = new Users(0, dsn);
            txtUsername.Focus();
            lblInvalid.Visible = false;
        }
        protected void btnLogin_Click(Object Sender, EventArgs e)
        {
            int intProfile = oUser.Login(txtUsername.Text, txtPassword.Text, intEnvironment, true, true);
            if (intProfile != -2)
                lblInvalid.Visible = true;
            else
            {
                Response.Cookies["adminid"].Value = intProfile.ToString();
                Redirect();
            }
        }
        private void Redirect()
        {
            Response.Redirect("/admin/admin_index.aspx");
        }
    }
}
