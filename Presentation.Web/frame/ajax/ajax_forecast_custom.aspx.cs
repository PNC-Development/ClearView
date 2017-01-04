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
    public partial class ajax_forecast_custom : BasePage
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
                if (strValues.IndexOf("RESET=") == 0)
                {
                    string strValue = strValues.Substring(6);
                    if (strValue.IndexOf("|") > -1)
                        strValue = strValue.Substring(0, strValue.IndexOf("|"));
                    if (strValue.IndexOf("_") > -1)
                        strValue = strValue.Substring(0, strValue.IndexOf("_"));
                    int _responseid = Int32.Parse(strValue);
                    int _questionid = Int32.Parse(oForecast.GetResponse(_responseid, "questionid"));
                    DataSet ds = oForecast.GetResponses(_questionid, 1);
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        sb.Append("<value>");
                        sb.Append(dr["id"].ToString());
                        sb.Append("</value><text>none</text>");
                    }
                }
                else if (strValues.IndexOf("|") > -1)
                {
                    string strValue = strValues.Substring(0, strValues.IndexOf("|"));
                    if (strValue.IndexOf("_") > -1)
                        strValue = strValue.Substring(0, strValue.IndexOf("_"));
                    int _responseid = Int32.Parse(strValue);
                    int _questionid = Int32.Parse(oForecast.GetResponse(_responseid, "questionid"));
                    DataSet ds = oForecast.GetResponses(_questionid, 1);
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        sb.Append("<value>");
                        sb.Append(dr["id"].ToString());
                        sb.Append("</value><text>none</text>");
                    }
                    while (strValues != "")
                    {
                        strValue = strValues.Substring(0, strValues.IndexOf("|"));
                        if (strValue.IndexOf("_") > -1)
                            strValue = strValue.Substring(0, strValue.IndexOf("_"));
                        _responseid = Int32.Parse(strValue);
                        int _custom = Int32.Parse(oForecast.GetResponse(_responseid, "custom"));
                        if (_custom > 0)
                        {
                            sb.Append("<value>");
                            sb.Append(_responseid.ToString());
                            sb.Append("</value><text>inline</text>");
                        }
                        strValues = strValues.Substring(strValues.IndexOf("|") + 1);
                    }
                }
                else
                {
                    int _responseid = Int32.Parse(strValues);
                    int _questionid = Int32.Parse(oForecast.GetResponse(_responseid, "questionid"));
                    DataSet ds = oForecast.GetResponses(_questionid, 1);
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        sb.Append("<value>");
                        sb.Append(dr["id"].ToString());
                        sb.Append("</value><text>none</text>");
                    }
                    int _custom = Int32.Parse(oForecast.GetResponse(_responseid, "custom"));
                    if (_custom > 0)
                    {
                        sb.Append("<value>");
                        sb.Append(_responseid.ToString());
                        sb.Append("</value><text>inline</text>");
                    }
                }
                sb.Append("</values>");
                Response.Write(sb.ToString());
                Response.End();
            }
        }
    }
}
