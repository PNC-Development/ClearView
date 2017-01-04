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
using NCC.ClearView.Application.Core;

namespace NCC.ClearView.Presentation.Web
{
    public partial class ajax_mnemonics_pending : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["u"] != null && Request.QueryString["u"] == "GET")
            {
                XmlDocument oDoc = new XmlDocument();
                oDoc.Load(Request.InputStream);
                Mnemonic oMnemonic = new Mnemonic(0, dsn);
                DataSet ds = oMnemonic.GetPending(Server.UrlDecode(oDoc.FirstChild.InnerXml));
                Response.ContentType = "application/xml";
                Response.Write("<values>");
                foreach (DataRow dr in ds.Tables[0].Rows)
                    Response.Write("<value>" + dr["id"].ToString() + "</value><text><![CDATA[" + dr["factory_code"].ToString() + " - " + dr["name"].ToString() + "]]></text>");
                Response.Write("</values>");
                Response.End();
            }
        }
    }
}
