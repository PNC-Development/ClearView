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
    public partial class report_about : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;

        protected string strTitle = "About";
        protected string strAbout = "There is no information about this report";
        protected void Page_Load(object sender, EventArgs e)
        {
            Reports oReport = new Reports(0, dsn);
            if (Request.QueryString["id"] != null)
            {
                strTitle = oReport.Get(Int32.Parse(Request.QueryString["id"]), "title");
                strAbout = oReport.Get(Int32.Parse(Request.QueryString["id"]), "about");
            }
        }
    }
}
