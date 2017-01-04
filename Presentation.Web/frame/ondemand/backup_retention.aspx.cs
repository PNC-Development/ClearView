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
    public partial class backup_retention : BasePage
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
            btnAdd.Attributes.Add("onclick", "return ValidateText('" + txtPath.ClientID + "','Please enter a path')" +
                " && ValidateDate('" + txtFirst.ClientID + "','Please enter a valid date')" +
                " && CheckRetention461('" + ddlRetention.ClientID + "','" + txtRetention.ClientID + "')" +
                " && CheckFrequency461('" + ddlFrequency.ClientID + "','" + txtFrequency.ClientID + "')" +
                ";");
            txtPath.Focus();
            ddlFrequency.Attributes.Add("onchange", "ChangeFrequency461(this,'" + divFrequency.ClientID + "');");
            imgArchiveDate.Attributes.Add("onclick", "return OpenCalendar('" + txtFirst.ClientID + "');");
        }
        protected void btnAdd_Click(Object Sender, EventArgs e)
        {
            oForecast.AddBackupRetention(Int32.Parse(Request.QueryString["id"]), txtPath.Text, DateTime.Parse(txtFirst.Text), (txtRetention.Text == "" ? 0 : Int32.Parse(txtRetention.Text)), ddlRetention.SelectedItem.Value, ddlHour.SelectedItem.Value, ddlSwitch.SelectedItem.Value, ddlFrequency.SelectedItem.Value, txtFrequency.Text);
            Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "reload", "<script type=\"text/javascript\">window.parent.div='3';window.parent.UpdateBackupUrl();<" + "/" + "script>");
        }
    }
}
