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
    public partial class password : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected Users oUser;
        protected Settings oSetting;
        protected void Page_Load(object sender, EventArgs e)
        {
            oSetting = new Settings(0, dsn);
            oUser = new Users(0, dsn);
            lblChange.Visible = false;
            lblMatch.Visible = false;
        }
        protected void btnChange_Click(Object Sender, EventArgs e)
        {
            int intProfile = oUser.Login(txtUsername.Text, txtPassword.Text, intEnvironment, true, true);
            if (intProfile == -2)
            {
                if (txtPassword1.Text == txtPassword2.Text)
                {
                    oSetting.Update(txtUsername.Text, txtPassword1.Text, intEnvironment);
                    lblChange.Visible = true;
                }
                else
                    lblMatch.Visible = true;
            }
            else
                lblInvalid.Visible = true;
        }
    }
}
