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
    public partial class storage3rd : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intDataPointAvailableService = Int32.Parse(ConfigurationManager.AppSettings["DATAPOINT_AVAILABLE_SERVICE"]);
        protected Customized oCustomized;
        protected Users oUser;
        protected Functions oFunction;
        protected DataPoint oDataPoint;
        protected int intProfile;
        protected string strResults = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            AuthenticateUser();
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oCustomized = new Customized(intProfile, dsn);
            oUser = new Users(intProfile, dsn);
            oFunction = new Functions(intProfile, dsn, intEnvironment);
            oDataPoint = new DataPoint(intProfile, dsn);
            int intProd = Int32.Parse(Request.QueryString["prod"]);
            int intQA = Int32.Parse(Request.QueryString["qa"]);
            int intTest = Int32.Parse(Request.QueryString["test"]);
            int intAddress = Int32.Parse(Request.QueryString["add"]);
            int intHigh = Int32.Parse(Request.QueryString["high"]);
            int intStandard = Int32.Parse(Request.QueryString["stand"]);
            int intLow = Int32.Parse(Request.QueryString["low"]);
            int intReplication = Int32.Parse(Request.QueryString["rep"]);

            int intApplication = 0;
            if (Request.QueryString["applicationid"] != null && Request.QueryString["applicationid"] != "")
                intApplication = Int32.Parse(Request.QueryString["applicationid"]);
            if (Request.Cookies["application"] != null && Request.Cookies["application"].Value != "")
                intApplication = Int32.Parse(Request.Cookies["application"].Value);

            bool boolOther = false;
            double dblAmountTotal = 0.00;
            double dblAmountReplicatedTotal = 0.00;
            double dblAmountHATotal = 0.00;
            DataSet ds = oCustomized.GetStorage3rdForecast(intProd, intQA, intTest, intAddress);
            StringBuilder sb = new StringBuilder(strResults);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                string strPerformance = dr["performance"].ToString().ToUpper();
                double dblAmount = double.Parse(dr["amount"].ToString());
                if (dblAmount > 0.00)
                {
                    if ((intHigh == 1 && strPerformance.Contains("HIGH") == true) || (intStandard == 1 && strPerformance.Contains("STANDARD") == true) || (intLow == 1 && strPerformance.Contains("LOW") == true))
                    {
                        if (intReplication == 0 || (intReplication == 1 && dr["replicated"].ToString().ToUpper() == "YES"))
                        {
                            double dblReplicateTimes = (dr["replicated"].ToString().ToUpper() == "YES" ? (dr["fabric"].ToString().ToUpper() == "CISCO" ? 2.00 : 3.00) : 0.00);
                            double dblHA = (dr["ha"].ToString().ToUpper() == "1" ? (dr["fabric"].ToString().ToUpper() == "CISCO" ? 2.00 : 0.00) : 0.00);
                            double dblReplicateAmount = dblAmount * dblReplicateTimes;
                            double dblHAAmount = dblAmount * dblHA;
                            dblAmountTotal += dblAmount;
                            dblAmountHATotal += dblHAAmount;
                            dblAmountReplicatedTotal += dblReplicateAmount;
                            boolOther = !boolOther;
                            sb.Append("<tr");
                            sb.Append(boolOther ? " bgcolor=\"F6F6F6\"" : "");
                            sb.Append(">");
                            int _requestid = Int32.Parse(dr["requestid"].ToString());
                            
                            if (oUser.IsAdmin(intProfile) == true || (oDataPoint.GetPagePermission(intApplication, "SERVICE") == true || intDataPointAvailableService == 1))
                            {
                                sb.Append("<td><a href=\"javascript:void(0);\" onclick=\"OpenNewWindowMenu('/datapoint/service/request.aspx?t=id&q=");
                                sb.Append(oFunction.encryptQueryString("CVT" + _requestid.ToString()));
                                sb.Append("&id=");
                                sb.Append(oFunction.encryptQueryString(_requestid.ToString()));
                                sb.Append("', '800', '600');\"/>CVT");
                                sb.Append(_requestid.ToString());
                                sb.Append("</a></td>");
                            }
                            else
                            {
                                sb.Append("<td>");
                                sb.Append(_requestid.ToString());
                                sb.Append("</td>");
                            }

                            sb.Append("<td>");
                            sb.Append(dr["class"].ToString());
                            sb.Append("</td>");
                            sb.Append("<td>");
                            sb.Append(dr["environment"].ToString());
                            sb.Append("</td>");
                            sb.Append("<td>");
                            sb.Append(dr["address"].ToString());
                            sb.Append("</td>");
                            sb.Append("<td>");
                            sb.Append(dr["performance"].ToString());
                            sb.Append("</td>");
                            sb.Append("<td>");
                            sb.Append(dr["fabric"].ToString());
                            sb.Append("</td>");
                            sb.Append("<td>");
                            sb.Append(dr["replicated"].ToString());
                            sb.Append("</td>");
                            sb.Append("<td>");
                            sb.Append(dr["ha"].ToString() == "1" ? "Yes" : "No");
                            sb.Append("</td>");
                            sb.Append("<td>");
                            sb.Append(dr["end_date"].ToString() == "" ? "" : DateTime.Parse(dr["end_date"].ToString()).ToShortDateString());
                            sb.Append("</td>");
                            sb.Append("<td align=\"right\">");
                            sb.Append(dblAmount.ToString("F"));
                            sb.Append(" GB</td>");
                            sb.Append("<td align=\"right\">");
                            sb.Append(dblHAAmount.ToString("F"));
                            sb.Append(" GB</td>");
                            sb.Append("<td align=\"right\">");
                            sb.Append(dblReplicateAmount.ToString("F"));
                            sb.Append(" GB</td>");
                            sb.Append("</tr>");
                        }
                    }
                }
            }

            strResults = sb.ToString();

            lblAmount.Text = dblAmountTotal.ToString("F");
            lblAmountHA.Text = dblAmountHATotal.ToString("F");
            lblAmountReplicated.Text = dblAmountReplicatedTotal.ToString("F");
        }
    }
}
