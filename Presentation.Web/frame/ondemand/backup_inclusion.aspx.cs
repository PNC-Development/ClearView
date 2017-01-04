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
    public partial class backup_inclusion : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intProfile;
        protected Forecast oForecast;
        protected void Page_Load(object sender, EventArgs e)
        {
            AuthenticateUser();
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oForecast = new Forecast(intProfile, dsn);
            btnAdd.Attributes.Add("onclick", "return ValidateText('" + txtPath.ClientID + "','Please enter a path');");
            txtPath.Focus();
            txtPath.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + btnAdd.ClientID + "').click();return false;}} else {return true}; ");
        }
        protected void btnAdd_Click(Object Sender, EventArgs e)
        {
            oForecast.AddBackupInclusion(Int32.Parse(Request.QueryString["id"]), txtPath.Text);
            Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "reload", "<script type=\"text/javascript\">window.parent.div='1';window.parent.UpdateBackupUrl();<" + "/" + "script>");
        }
    }
}
