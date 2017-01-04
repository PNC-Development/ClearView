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
using System.Text;

namespace NCC.ClearView.Presentation.Web
{
    public partial class service_link : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected Users oUser;
        protected Services oService;
        protected Variables oVariable;
        protected Functions oFunctions;
        protected int intProfile;
        protected void Page_Load(object sender, EventArgs e)
        {
            AuthenticateUser();
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);

            oUser = new Users(intProfile, dsn);
            oService = new Services(intProfile, dsn);
            oVariable = new Variables(intEnvironment);
            oFunctions = new Functions(intProfile, dsn, intEnvironment);

            if (!IsPostBack)
            {
                if (String.IsNullOrEmpty(Request.QueryString["s"]) == false && String.IsNullOrEmpty(Request.QueryString["f"]) == false)
                {
                    lblService.Text = oFunctions.decryptQueryString(Request.QueryString["s"]);
                    lblLocation.Text = oService.GetFolderLocation(Int32.Parse(Request.QueryString["f"]));
                }
                txtUser.Attributes.Add("onkeyup", "return AJAXTextBoxGet(this,'300','195','" + divUser.ClientID + "','" + lstUser.ClientID + "','" + hdnUser.ClientID + "','" + oVariable.URL() + "/frame/users.aspx',2);");
                lstUser.Attributes.Add("ondblclick", "AJAXClickRow();");
            }
        }
        protected void btnSubmit_Click(Object Sender, EventArgs e)
        {
            bool boolSent = false;

            int User = 0;
            if (Int32.TryParse(Request.Form[hdnUser.UniqueID], out User))
            {
                StringBuilder body = new StringBuilder();
                body.Append(oUser.GetFullName(User));
                body.Append(", per your recent inquiry regarding a service, you can find the &quot;");
                body.Append(lblService.Text);
                body.Append("&quot; service within the Service Request module in ClearView. The service is located in the ");
                body.Append(lblLocation.Text);
                body.Append(" directory.");
                oFunctions.SendEmail("Service Request Link", oUser.GetName(User), "", oUser.GetName(intProfile), "Service Request Link", "<p>" + body.ToString() + "</p>", true, false);
                boolSent = true;
            }

            if (boolSent)
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "reload", "<script type=\"text/javascript\">alert('An email was successfully sent.');window.parent.location.reload();window.close();<" + "/" + "script>");
            else
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "reload", "<script type=\"text/javascript\">alert('There was a problem sending the email.');window.parent.location.reload();window.close();<" + "/" + "script>");
        }
    }
}
