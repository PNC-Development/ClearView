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

namespace NCC.ClearView.Presentation.Web
{
    public partial class SystemError : BaseControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Title = "ClearView encountered an error";
            Page.Title = ConfigurationManager.AppSettings["appTitle"];
            if (!IsPostBack)
            {
                lblError.Text = "ClearView administrators have been notified and are investigating the problem.  You will be contacted shortly.";
                btnClose.Attributes.Add("onclick", "return CloseWindow();");
            }
        }
    }
}