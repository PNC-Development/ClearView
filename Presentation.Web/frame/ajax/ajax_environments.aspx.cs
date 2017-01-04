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
    public partial class ajax_environments : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected void Page_Load(object sender, EventArgs e)
        {
            Classes oClass = new Classes(0, dsn);
            if (Request.QueryString["u"] != null && Request.QueryString["u"] == "GET")
            {
                XmlDocument oDoc = new XmlDocument();
                oDoc.Load(Request.InputStream);
                Response.ContentType = "application/xml";
                string strResponse = "<values>";
                string strValue = Server.UrlDecode(oDoc.ChildNodes[0].ChildNodes[0].InnerText);
                int intForecast = Int32.Parse(oDoc.ChildNodes[0].ChildNodes[1].InnerText);
                int intClass = Int32.Parse(strValue);
                DataSet ds = oClass.GetEnvironment(intClass, intForecast);
                foreach (DataRow dr in ds.Tables[0].Rows)
                    strResponse += "<value>" + dr["id"].ToString() + "</value><text>" + dr["name"].ToString() + "</text>";
                strResponse += "</values>";
                Response.Write(strResponse);
                Response.End();
            }
        }
    }
}
