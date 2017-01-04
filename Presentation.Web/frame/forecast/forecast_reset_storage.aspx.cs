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
    public partial class forecast_reset_storage : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected Forecast oForecast;
        protected int intID = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            oForecast = new Forecast(0, dsn);
            if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
                intID = Int32.Parse(Request.QueryString["id"]);
        }
        protected void btnYes_Click(Object Sender, EventArgs e)
        {
            oForecast.DeleteStorage(intID);
            oForecast.DeleteStorageOS(intID);
            Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "saved", "<script type=\"text/javascript\">window.parent.opener.navigate(window.parent.opener.location);window.parent.navigate(window.parent.location);<" + "/" + "script>");
        }
        protected void btnNo_Click(Object Sender, EventArgs e)
        {
            Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "saved", "<script type=\"text/javascript\">window.parent.opener.navigate(window.parent.opener.location);window.parent.navigate(window.parent.location);<" + "/" + "script>");
        }
    }
}
