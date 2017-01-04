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
using System.Xml;
using System.Text;

namespace NCC.ClearView.Presentation.Web
{
    public partial class ajax_decrypt : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected void Page_Load(object sender, EventArgs e)
        {
            Functions oFunction = new Functions(0, dsn, intEnvironment);
            XmlDocument oDoc = new XmlDocument();
            oDoc.Load(Request.InputStream);
            Response.ContentType = "application/xml";
            StringBuilder sb = new StringBuilder("<value>");
            string strValue = Server.UrlDecode(oDoc.FirstChild.InnerXml);
            sb.Append(oFunction.decryptQueryString(strValue));
            sb.Append("</value>");
            Response.Write(sb.ToString());
            Response.End();
        }
    }
}
