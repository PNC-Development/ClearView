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
    public partial class ajax_datapoint_name : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["u"] != null && Request.QueryString["u"] == "GET")
            {
                XmlDocument oDoc = new XmlDocument();
                oDoc.Load(Request.InputStream);
                DataPoint oDataPoint = new DataPoint(0, dsn);
                string strName = oDoc.ChildNodes[0].ChildNodes[0].InnerText;
                DataSet ds = oDataPoint.GetAssetName(strName, 0, 0, "", "", 0);
                Response.ContentType = "application/xml";
                Response.Write("<values>");
                if (ds.Tables[0].Rows.Count > 0)
                    Response.Write("<value>1</value>");
                else
                    Response.Write("<value>0</value>");
                Response.Write("</values>");
                Response.End();
            }
        }
    }
}
