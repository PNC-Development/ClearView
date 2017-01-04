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
    public partial class calculator : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["value"] != null && Request.QueryString["value"] != "")
                Page.ClientScript.RegisterStartupScript(this.GetType(), "startup", "LoadCalc(" + Request.QueryString["value"] + ");", true);
            else
                Page.ClientScript.RegisterStartupScript(this.GetType(), "startup", "LoadCalc(0.00);", true);
        }
    }
}
