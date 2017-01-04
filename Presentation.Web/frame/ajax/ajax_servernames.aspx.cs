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
using System.Xml;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using NCC.ClearView.Application.Core;

namespace NCC.ClearView.Presentation.Web
{
    public partial class ajax_servernames : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        private Servers oServer;  
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["u"] != null && Request.QueryString["u"] == "GET")
            {
                XmlDocument oDoc = new XmlDocument();
                oDoc.Load(Request.InputStream);
                oServer = new Servers(0, dsn);
                DataSet ds = oServer.Get(Server.UrlDecode(oDoc.FirstChild.InnerXml), false);
                Response.ContentType = "application/xml";
                Response.Write("<values>");
                foreach (DataRow dr in ds.Tables[0].Rows)
                    Response.Write("<value><![CDATA[" + dr["id"].ToString() + "]]></value><text><![CDATA[" + dr["servername"].ToString() + "]]></text>");
                Response.Write("</values>");
                Response.End();
            }
        }
    }
}
