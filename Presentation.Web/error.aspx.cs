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
    public partial class error : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected string strTitle = ConfigurationManager.AppSettings["appTitle"];

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title = strTitle;
            lblTitle.Text = "ClearView encountered an error";

            if (!IsPostBack)
            {
                lblError.Text = "ClearView administrators have been notified and are investigating the problem.  You will be contacted shortly.";
                btnClose.Attributes.Add("onclick", "return CloseWindow();");
                Variables oVariable = new Variables(intEnvironment);
                hypCommunity.NavigateUrl = oVariable.Community();
            }
        }
    }
}
