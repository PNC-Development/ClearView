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
    public partial class ajax_environments_multiple : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected void Page_Load(object sender, EventArgs e)
        {
            Classes oClass = new Classes(0, dsn);
            Functions oFunction = new Functions(0, dsn, 0);
            if (Request.QueryString["u"] != null && Request.QueryString["u"] == "GET")
            {
                XmlDocument oDoc = new XmlDocument();
                oDoc.Load(Request.InputStream);
                Response.ContentType = "application/xml";
                StringBuilder sb = new StringBuilder("<values>");
                string strValue = Server.UrlDecode(oDoc.ChildNodes[0].ChildNodes[0].InnerText);
                int intForecast = Int32.Parse(oDoc.ChildNodes[0].ChildNodes[1].InnerText);

                if (strValue == "0,")
                {
                    strValue = "";
                    DataSet dsClass = oClass.Gets(1);
                    foreach (DataRow drClass in dsClass.Tables[0].Rows)
                    {
                        strValue += drClass["id"] + ",";
                    }
                }

                string[] strValues;
                char[] strSplit = { ',' };
                strValues = strValue.Split(strSplit);
                strValue = oFunction.BuildXmlString("data", strValues);
                DataSet ds = oClass.GetEnvironments(strValue, intForecast);

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    sb.Append("<value>");
                    sb.Append(dr["id"].ToString());
                    sb.Append("</value><text>");
                    sb.Append(dr["name"].ToString());
                    sb.Append("</text>");
                }

                sb.Append("</values>");
                Response.Write(sb.ToString());
                Response.End();
            }
        }
    }
}
