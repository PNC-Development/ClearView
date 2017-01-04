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
using System.Net.Mail;

namespace NCC.ClearView.Presentation.Web
{
    public partial class error_new : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            PH1.Controls.Add(this.LoadControl("/controls/sys/sys_topnav.ascx"));
        }
    }
}
