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
    public partial class ajax_forecast_affects : BasePage
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
                    DataSet dsAffects = oForecast.GetAffectsByQuestion(_questionid);
                    foreach (DataRow drAffect in dsAffects.Tables[0].Rows)
                    {
                        string strDisplay = (drAffect["state"].ToString() == "1" ? "inline" : "none");
                        sb.Append("<value>");
                        sb.Append(drAffect["affectedid"].ToString());
                        sb.Append("</value><text>");
                        sb.Append(strDisplay);
                        sb.Append("</text>");
                    }
                }
                else if (strValues.IndexOf("|") > -1)
                {
                    string strValue = strValues.Substring(0, strValues.IndexOf("|"));
                    if (strValue.IndexOf("_") > -1)
                        strValue = strValue.Substring(0, strValue.IndexOf("_"));
                    int _responseid = Int32.Parse(strValue);
                    int _questionid = Int32.Parse(oForecast.GetResponse(_responseid, "questionid"));
                    DataSet dsResponses = oForecast.GetResponsesNoCustom(_questionid, 1);
                    DataSet dsAffects = oForecast.GetAffectsByQuestion(_questionid);
                    foreach (DataRow drAffect in dsAffects.Tables[0].Rows)
                    {
                        string strDisplay = (drAffect["state"].ToString() == "1" ? "inline" : "none");
                        int intAffected = Int32.Parse(drAffect["affectedid"].ToString());
                        string strTempValues = strValues;
                        while (strTempValues != "")
                        {
                            strValue = strTempValues.Substring(0, strTempValues.IndexOf("|"));
                            if (strValue.IndexOf("_") > -1)
                                strValue = strValue.Substring(0, strValue.IndexOf("_"));
                            _responseid = Int32.Parse(strValue);
                            DataSet dsAffect = oForecast.GetAffected(_questionid, intAffected, _responseid);
                            if (dsAffect.Tables[0].Rows.Count > 0)
                            {
                                strDisplay = (dsAffect.Tables[0].Rows[0]["state"].ToString() == "1" ? "inline" : "none");
                                if (strDisplay == "inline")
                                    break;
                            }
                            strTempValues = strTempValues.Substring(strTempValues.IndexOf("|") + 1);
                        }
                        sb.Append("<value>");
                        sb.Append(drAffect["affectedid"].ToString());
                        sb.Append("</value><text>");
                        sb.Append(strDisplay);
                        sb.Append("</text>");
                    }
                }
                else
                {
                    int _responseid = Int32.Parse(strValues);
                    int _questionid = Int32.Parse(oForecast.GetResponse(_responseid, "questionid"));
                    DataSet dsResponses = oForecast.GetResponsesNoCustom(_questionid, 1);
                    DataSet dsAffects = oForecast.GetAffectsByQuestion(_questionid);
                    foreach (DataRow drAffect in dsAffects.Tables[0].Rows)
                    {
                        string strDisplay = (drAffect["state"].ToString() == "1" ? "inline" : "none");
                        int intAffected = Int32.Parse(drAffect["affectedid"].ToString());
                        foreach (DataRow drResponse in dsResponses.Tables[0].Rows)
                        {
                            int intResponse = Int32.Parse(drResponse["id"].ToString());
                            if (intResponse == _responseid)
                            {
                                DataSet dsAffect = oForecast.GetAffected(_questionid, intAffected, intResponse);
                                if (dsAffect.Tables[0].Rows.Count > 0)
                                    strDisplay = (dsAffect.Tables[0].Rows[0]["state"].ToString() == "1" ? "inline" : "none");
                            }
                        }
                        sb.Append("<value>");
                        sb.Append(drAffect["affectedid"].ToString());
                        sb.Append("</value><text>");
                        sb.Append(strDisplay);
                        sb.Append("</text>");
                    }
                }
                sb.Append("</values>");
                Response.Write(sb.ToString());
                Response.End();
            }
        }
    }
}
