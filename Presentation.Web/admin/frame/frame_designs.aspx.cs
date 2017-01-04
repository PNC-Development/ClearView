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

namespace NCC.ClearView.Presentation.Web
{
    public partial class frame_designs : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected  string strSteps = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            Forecast oForecast = new Forecast(0, dsn);
            if (Request.QueryString["rid"] != null && Request.QueryString["rid"] != "")
            {
                int intRequest = Int32.Parse(Request.QueryString["rid"]);
                DataSet dsForecast = oForecast.GetRequest(intRequest);
                if (dsForecast.Tables[0].Rows.Count > 0)
                {
                    int intForecast = Int32.Parse(dsForecast.Tables[0].Rows[0]["id"].ToString());
                    DataSet dsDesigns = oForecast.GetAnswers(intForecast);
                    foreach (DataRow drDesign in dsDesigns.Tables[0].Rows)
                    {
                        int intDesign = Int32.Parse(drDesign["id"].ToString());
                        int intPlatform = Int32.Parse(drDesign["platformid"].ToString());
                        DataSet ds = oForecast.GetSteps(intPlatform, 1);
                        DataSet dsStepsDone = oForecast.GetStepsDone(intDesign, 0);
                        int intCount = 0;
                        int intStepCount = 0;
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            intStepCount++;
                            intCount++;
                            strSteps += "<table width=\"100%\" cellpadding=\"0\" cellspacing=\"5\" border=\"0\">";
                            strSteps += "<tr>";
                            string strURL = "<a href=\"/frame/forecast_equipment.aspx?id=" + intDesign.ToString() + "&step=" + intStepCount.ToString() + "\">";
                            string strImage = "<img src=\"/images/ico_hourglass40.gif\" border=\"0\" align=\"absmiddle\" />";
                            if (dsStepsDone.Tables[0].Rows[intCount - 1]["done"].ToString() == "1")
                                strImage = "<img src=\"/images/ico_check40.gif\" border=\"0\" align=\"absmiddle\" />";
                            strSteps += "<td rowspan=\"2\">" + strURL + strImage + "</a></td>";
                            strSteps += "<td class=\"bold\" width=\"100%\" valign=\"bottom\">" + strURL + "Step " + intCount.ToString() + ": " + dr["name"].ToString() + "</a></td>";
                            strSteps += "</tr>";
                            strSteps += "<tr>";
                            strSteps += "<td width=\"100%\" valign=\"top\">" + dr["subtitle"].ToString() + "</td>";
                            strSteps += "</tr>";
                            strSteps += "</table>";
                        }
                        strSteps += "<hr size=\"1\" noshade/>";
                    }
                }
            }
        }
    }
}
