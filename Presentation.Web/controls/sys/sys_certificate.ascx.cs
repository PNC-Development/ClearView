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
    public partial class sys_certificate : System.Web.UI.UserControl
    {
        private string certificate = ConfigurationManager.AppSettings["CERTIFICATE"];
        protected void Page_Load(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(certificate))
                panCertificate.Visible = false;
            else
            {
                string url = Request.Url.Scheme + "://" + Request.Url.Host;
                if (url.ToUpper() != certificate.ToUpper())
                {
                    panCertificate.Visible = true;
                    hypCertificate.NavigateUrl = Request.Url.ToString().Replace(url, certificate);
                    hypCertificate.Attributes.Add("onclick", "alert('You are about to be redirected to:\\n" + certificate + "\\n\\nOnce there, be sure to update your favorite/bookmark.');");
                }
            }
        }
    }
}