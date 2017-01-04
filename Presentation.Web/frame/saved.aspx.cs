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
    public partial class saved : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["alert"] != null)
            {
                Functions oFunction = new Functions(0, "", 0);
                if (Request.QueryString["header"] != null && Request.QueryString["header"] != "")
                    litHeader.Text = oFunction.decryptQueryString(Request.QueryString["header"]);
                else
                    litHeader.Text = "Complete";
                if (Request.QueryString["alert"] != null && Request.QueryString["alert"] != "")
                    litMessage.Text = oFunction.decryptQueryString(Request.QueryString["alert"]);
                else
                    litHeader.Text = "Information Saved Successfully";
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "countdown", "<script type=\"text/javascript\">StartCountdown('" + lblTimer.ClientID + "');<" + "/" + "script>");
                //btnClose.Attributes.Add("onclick", "return parent.HidePanel();");
            }
        }
    }
}
