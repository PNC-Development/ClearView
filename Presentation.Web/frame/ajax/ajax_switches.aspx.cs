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
    public partial class ajax_switches : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected void Page_Load(object sender, EventArgs e)
        {
            Asset oAsset = new Asset(0, dsnAsset, dsn);
            if (Request.QueryString["u"] != null && Request.QueryString["u"] == "GET")
            {
                XmlDocument oDoc = new XmlDocument();
                oDoc.Load(Request.InputStream);
                Response.ContentType = "application/xml";
                string strResponse = "<values>";
                string strValue = Server.UrlDecode(oDoc.FirstChild.InnerXml);
                int intSwitch = Int32.Parse(strValue);
                DataSet ds = oAsset.GetSwitch(intSwitch);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    strResponse += "<value>" + dr["blades"].ToString() + "</value><value>" + dr["ports"].ToString() + "</value><value>" + dr["nexus"].ToString() + "</value>";
                }
                strResponse += "</values>";
                Response.Write(strResponse);
                Response.End();
            }
        }
    }
}
