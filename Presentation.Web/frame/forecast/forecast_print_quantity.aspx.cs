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
using System.Text;

namespace NCC.ClearView.Presentation.Web
{
    public partial class forecast_print_quantity : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intProfile;
        protected Forecast oForecast;
        protected string strSummary = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            AuthenticateUser();
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oForecast = new Forecast(intProfile, dsn);
            if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
            {
                StringBuilder sb = new StringBuilder(strSummary);
                int intID = Int32.Parse(Request.QueryString["id"]);
                DataSet ds = oForecast.GetAnswer(intID);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    bool boolHA = oForecast.IsHARoom(intID);
                    bool boolRecoveryOne = oForecast.IsDROneToOne(intID);
                    bool boolRecoveryMany = oForecast.IsDRManyToOne(intID);
                    double dblQuantity = double.Parse(ds.Tables[0].Rows[0]["quantity"].ToString());
                    sb.Append("<tr>");
                    sb.Append("<td width=\"100%\"><b>Quantity:</b></td>");
                    sb.Append("<td align=\"right\">&nbsp;</td>");
                    sb.Append("<td align=\"right\">");
                    sb.Append(dblQuantity.ToString());
                    sb.Append("</td>");
                    sb.Append("</tr>");
                    double dblHA = 0.00;
                    string strHA = "No High Availability";
                    if (boolHA == true)
                    {
                        dblHA = double.Parse(ds.Tables[0].Rows[0]["ha"].ToString());
                        strHA = "High Availability";
                    }
                    sb.Append("<tr>");
                    sb.Append("<td width=\"100%\"><b>");
                    sb.Append(strHA);
                    sb.Append(":</b></td>");
                    sb.Append("<td align=\"right\">+</td>");
                    sb.Append("<td align=\"right\">");
                    sb.Append(dblHA.ToString());
                    sb.Append("</td>");
                    sb.Append("</tr>");
                    double dblRecovery = 0.00;
                    string strRecovery = "No Recovery";
                    if (boolRecoveryMany == true)
                    {
                        dblRecovery = double.Parse(ds.Tables[0].Rows[0]["recovery_number"].ToString());
                        strRecovery = "Many-to-One Recovery";
                    }
                    if (boolRecoveryOne == true)
                    {
                        dblRecovery = dblQuantity;
                        strRecovery = "One-to-One Recovery";
                    }
                    sb.Append("<tr>");
                    sb.Append("<td width=\"100%\"><b>");
                    sb.Append(strRecovery);
                    sb.Append(":</b></td>");
                    sb.Append("<td align=\"right\">+</td>");
                    sb.Append("<td align=\"right\">");
                    sb.Append(dblRecovery.ToString());
                    sb.Append("</td>");
                    sb.Append("</tr>");
                    sb.Append("<tr><td colspan=\"3\"><hr size=\"1\" noshade /></td></tr>");
                    dblQuantity = dblQuantity + dblRecovery + dblHA;
                    sb.Append("<tr>");
                    sb.Append("<td width=\"100%\"><b>Total:</b></td>");
                    sb.Append("<td colspan=\"2\" align=\"right\"><b>");
                    sb.Append(dblQuantity.ToString());
                    sb.Append("</b></td>");
                    sb.Append("</tr>");
                }

                sb.Insert(0, "<table width=\"225\" cellpadding=\"3\" cellspacing=\"2\" border=\"0\">");
                sb.Append("</table>");

                strSummary = sb.ToString();
            }
        }
    }
}
