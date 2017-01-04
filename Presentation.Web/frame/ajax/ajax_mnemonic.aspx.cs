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
using System.DirectoryServices;

namespace NCC.ClearView.Presentation.Web
{
    public partial class ajax_mnemonic : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intCount = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            Mnemonic oMnemonic = new Mnemonic(0, dsn);
            if (Request.QueryString["u"] != null && Request.QueryString["u"] == "GET")
            {
                XmlDocument oDoc = new XmlDocument();
                oDoc.Load(Request.InputStream);
                int intMnemonic = Int32.Parse(Server.UrlDecode(oDoc.ChildNodes[0].ChildNodes[0].InnerText));
                string strField = Server.UrlDecode(oDoc.ChildNodes[0].ChildNodes[1].InnerText);
                Response.ContentType = "text/xml";
                Response.Write("<values><value>" + oMnemonic.Get(intMnemonic, strField) + "</value></values>");
                Response.End();
            }
        }
    }
}
