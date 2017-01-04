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
    public partial class resource_request_email_san : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);

        protected ResourceRequest oResourceRequest;
        protected Variables oVariable;
        protected void Page_Load(object sender, EventArgs e)
        {
            oResourceRequest = new ResourceRequest(0, dsn);
            oVariable = new Variables(intEnvironment);
            if (Request.QueryString["rrid"] != null && Request.QueryString["rrid"] != "")
            {
                int intResource = Int32.Parse(Request.QueryString["rrid"]);
                txtEmployee.Attributes.Add("onkeyup", "return AJAXTextBoxGet(this,'300','195','" + divEmployee.ClientID + "','" + lstEmployee.ClientID + "','" + hdnEmployee.ClientID + "','" + oVariable.URL() + "/frame/users.aspx',2);");
                lstEmployee.Attributes.Add("ondblclick", "AJAXClickRow();");
            }
        }
        protected void btnSubmit_Click(Object Sender, EventArgs e)
        {
            Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "reload", "<script type=\"text/javascript\">window.parent.location.reload();window.close();<" + "/" + "script>");
        }
    }
}
