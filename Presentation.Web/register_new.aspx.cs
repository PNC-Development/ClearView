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
using System.DirectoryServices;

namespace NCC.ClearView.Presentation.Web
{
    public partial class register_new : BasePage
    {
        protected string strTitle = ConfigurationManager.AppSettings["appTitle"];

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title = strTitle;
        }
    }
}
