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
    public partial class ajax_forecast_variance : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected void Page_Load(object sender, EventArgs e)
        {
            Forecast oForecast = new Forecast(0, dsn);
            if (Request.QueryString["u"] != null && Request.QueryString["u"] == "GET")
            {
                XmlDocument oDoc = new XmlDocument();
                oDoc.Load(Request.InputStream);
                Response.ContentType = "application/xml";
                StringBuilder sb = new StringBuilder("<values>");
                string strValues = Server.UrlDecode(oDoc.FirstChild.InnerXml);
                int _responseid = 0;
                int _questionid = 0;
                if (strValues.IndexOf("RESET=") == 0)
                {
                    string strValue = strValues.Substring(6);
                    strValue = strValue.Substring(0, strValue.IndexOf("|"));
                    _responseid = Int32.Parse(strValue);
                    _questionid = Int32.Parse(oForecast.GetResponse(_responseid, "questionid"));
                    sb.Append("<value>");
                    sb.Append(_questionid.ToString());
                    sb.Append("</value><text>none</text>");
                }
                else if (strValues.IndexOf("|") > -1)
                {
                    string strDisplay = "none";
                    while (strValues != "")
                    {
                        string strValue = strValues.Substring(0, strValues.IndexOf("|"));
                        if (strValue.IndexOf("_") > -1)
                            strValue = strValue.Substring(0, strValue.IndexOf("_"));
                        strValues = strValues.Substring(strValues.IndexOf("|") + 1);
                        _responseid = Int32.Parse(strValue);
                        _questionid = Int32.Parse(oForecast.GetResponse(_responseid, "questionid"));
                        if (oForecast.GetResponse(_responseid, "variance") == "1")
                        {
                            strDisplay = "inline";
                            break;
                        }
                    }
                    sb.Append("<value>");
                    sb.Append(_questionid.ToString());
                    sb.Append("</value><text>");
                    sb.Append(strDisplay);
                    sb.Append("</text>");
                }
                else if (strValues != "")
                {
                    _responseid = Int32.Parse(strValues);
                    _questionid = Int32.Parse(oForecast.GetResponse(_responseid, "questionid"));
                    sb.Append("<value>");
                    sb.Append(_questionid.ToString());
                    sb.Append("</value><text>");
                    sb.Append(oForecast.GetResponse(_responseid, "variance") == "1" ? "inline" : "none");
                    sb.Append("</text>");
                }
                sb.Append("</values>");
                Response.Write(sb.ToString());
                Response.End();
            }
        }
    }
}
