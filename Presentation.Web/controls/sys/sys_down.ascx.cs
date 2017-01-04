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
    public partial class sys_down : System.Web.UI.UserControl
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected Pages oPage;
        protected Settings oSetting;
        protected int intPage = 0;
        protected int intApplication = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            oPage = new Pages(0, dsn);
            oSetting = new Settings(0, dsn);
            if (Request.QueryString["applicationid"] != null && Request.QueryString["applicationid"] != "")
                intApplication = Int32.Parse(Request.QueryString["applicationid"]);
            if (Request.QueryString["pageid"] != null && Request.QueryString["pageid"] != "")
                intPage = Int32.Parse(Request.QueryString["pageid"]);
            if (Request.Cookies["application"] != null && Request.Cookies["application"].Value != "")
                intApplication = Int32.Parse(Request.Cookies["application"].Value);
            lblTitle.Text = "ClearView Currently Unavailable";
            string strDate = oSetting.Get("maintenance");
            if (strDate == "")
                Response.Redirect("/index.aspx");
            else
            {
                try
                {
                    DateTime _date = DateTime.Parse(strDate);
                    lblDate.Text = _date.ToLongDateString();
                    lblTime.Text = _date.ToLongTimeString();
                }
                catch { }
            }
        }
    }
}