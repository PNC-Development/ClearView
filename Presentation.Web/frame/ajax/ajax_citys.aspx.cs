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

namespace NCC.ClearView.Presentation.Web
{
    public partial class ajax_citys : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected void Page_Load(object sender, EventArgs e)
        {
            Locations oLocation = new Locations(0, dsn);
            if (Request.QueryString["u"] != null && Request.QueryString["u"] == "GET")
            {
                XmlDocument oDoc = new XmlDocument();
                oDoc.Load(Request.InputStream);
                Response.ContentType = "application/xml";
                string strResponse = "<values>";
                string strValue = Server.UrlDecode(oDoc.FirstChild.InnerXml);
                int intState = Int32.Parse(strValue);
                DataSet ds = oLocation.GetCitys(intState, 1);
                foreach (DataRow dr in ds.Tables[0].Rows)
                    strResponse += "<value><![CDATA[" + dr["id"].ToString() + "]]></value><text><![CDATA[" + dr["name"].ToString() + "]]></text>";
                strResponse += "</values>";
                Response.Write(strResponse);
                Response.End();
            }
        }
    }
}
