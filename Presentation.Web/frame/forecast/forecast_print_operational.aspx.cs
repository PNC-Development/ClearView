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
    public partial class forecast_print_operational : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        
        protected int intProfile;
        protected Forecast oForecast;
        protected Models oModel;
        protected Platforms oPlatform;
        protected ModelsProperties oModelsProperties;
        protected string strSummary = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            AuthenticateUser();
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oForecast = new Forecast(intProfile, dsn);
            oModel = new Models(intProfile, dsn);
            oPlatform = new Platforms(intProfile, dsn);
            oModelsProperties = new ModelsProperties(intProfile, dsn);
            if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
            {
                StringBuilder sb = new StringBuilder(strSummary);
                int intID = Int32.Parse(Request.QueryString["id"]);
                int intModel = 0;
                double dblQuantity = double.Parse(oForecast.GetAnswer(intID, "quantity"));
                double dblRecovery = double.Parse(oForecast.GetAnswer(intID, "recovery_number"));
                int intServerModel = oForecast.GetModelAsset(intID);
                if (intServerModel == 0)
                    intServerModel = oForecast.GetModel(intID);
                if (intServerModel == 0)
                    intModel = Int32.Parse(oForecast.GetAnswer(intID, "modelid"));
                if (intServerModel > 0)
                {
                    if (intModel == 0)
                        intModel = Int32.Parse(oModelsProperties.Get(intServerModel, "modelid"));
                }
                sb.Append("<tr>");
                sb.Append("<td><b>Model:</b></td>");
                sb.Append("<td align=\"right\">");
                sb.Append(oModel.Get(intModel, "name"));
                sb.Append("</td>");
                sb.Append("</tr>");
                double dblTotal = 0.00;
                DataSet ds = oForecast.GetOperations(intModel, 1);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    sb.Append("<tr>");
                    sb.Append("<td><b>");
                    sb.Append(dr["name"].ToString());
                    sb.Append(":</b></td>");
                    double dblCost = double.Parse(dr["cost"].ToString());
                    dblTotal += dblCost;
                    sb.Append("<td align=\"right\">$");
                    sb.Append(dblCost.ToString("N"));
                    sb.Append("</td>");
                    sb.Append("</tr>");
                }
                sb.Append("<tr><td colspan=\"2\"><hr size=\"1\" noshade/></td></tr>");
                sb.Append("<tr><td>Total Amount:</td><td class=\"reddefault\" align=\"right\">$");
                sb.Append(dblTotal.ToString("N"));
                sb.Append("</td></tr>");
                sb.Append("<tr><td colspan=\"2\"><img src=\"/images/spacer.gif\" border=\"0\" width=\"1\" height=\"1\"/></td></tr>");
                sb.Append("<tr><td>Quantity:</td><td align=\"right\">");
                sb.Append(dblQuantity.ToString("N"));
                sb.Append("</td></tr>");
                sb.Append("<tr><td>Recovery:</td><td align=\"right\">+ ");
                sb.Append(dblRecovery.ToString("N"));
                sb.Append("</td></tr>");
                sb.Append("<tr><td colspan=\"2\"><hr size=\"1\" noshade/></td></tr>");
                dblQuantity = dblQuantity + dblRecovery;
                sb.Append("<tr><td>Total Quantity:</td><td align=\"right\">");
                sb.Append(dblQuantity.ToString("N"));
                sb.Append("</td></tr>");
                sb.Append("<tr><td>Total Amount:</td><td class=\"reddefault\" align=\"right\">x $");
                sb.Append(dblTotal.ToString("N"));
                sb.Append("</td></tr>");
                sb.Append("<tr><td colspan=\"2\"><hr size=\"1\" noshade/></td></tr>");
                dblTotal = dblTotal * dblQuantity;
                sb.Append("<tr><td><b>Grand Total:</b></td><td class=\"reddefault\" align=\"right\"><b>$");
                sb.Append(dblTotal.ToString("N"));
                sb.Append("</b></td></tr>");
                sb.Insert(0, "<table width=\"350\" cellpadding=\"3\" cellspacing=\"2\" border=\"0\">");
                sb.Append("</table>");

                strSummary = sb.ToString();
            }
        }
    }
}
