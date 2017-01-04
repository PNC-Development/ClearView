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
    public partial class production : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);

        protected Forecast oForecast;
        protected int intProfile;
        protected int intId;

        protected void Page_Load(object sender, EventArgs e)
        {
            AuthenticateUser();
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oForecast = new Forecast(intProfile, dsn);
            if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
                intId = Int32.Parse(Request.QueryString["id"]);
            if (!IsPostBack)
            {
                string strProduction = oForecast.GetAnswer(intId, "production");
                if (strProduction != "")
                    txtProdDate.Text = DateTime.Parse(strProduction).ToShortDateString();
            }
            imgProdDate.Attributes.Add("onclick", "return OpenCalendar('" + txtProdDate.ClientID + "');");
            btnUpdate.Attributes.Add("onclick", "return ValidateDate('" + txtProdDate.ClientID + "','Please enter or select a valid production date');");
        }

        protected void btnUpdate_Click(Object Sender, EventArgs e)
        {
            oForecast.UpdateAnswerProduction(intId, DateTime.Parse(txtProdDate.Text));
            ClientScript.RegisterClientScriptBlock(typeof(Page), "redirect", "self.close();RefreshOpeningWindow();", true);
        }
    }
}
