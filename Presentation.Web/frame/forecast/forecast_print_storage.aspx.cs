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
    public partial class forecast_print_storage : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        
        protected int intProfile;
        protected Forecast oForecast;
        protected Platforms oPlatform;
        protected Models oModel;
        protected ModelsProperties oModelsProperties;
        protected string strSummary = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            AuthenticateUser();
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oForecast = new Forecast(intProfile, dsn);
            oPlatform = new Platforms(intProfile, dsn);
            oModel = new Models(intProfile, dsn);
            oModelsProperties = new ModelsProperties(intProfile, dsn);
            if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
            {
                StringBuilder sb = new StringBuilder(strSummary);
                int intID = Int32.Parse(Request.QueryString["id"]);
                int intPlatform = Int32.Parse(oForecast.GetAnswer(intID, "platformid"));
                int intServerModel = oForecast.GetModelAsset(intID);
                if (intServerModel == 0)
                    intServerModel = oForecast.GetModel(intID);
                int intModel = intServerModel;
                double dblReplicate = 0.00;
                if (oModelsProperties.Get(intServerModel).Tables[0].Rows.Count > 0)
                {
                    intModel = Int32.Parse(oModelsProperties.Get(intServerModel, "modelid"));
                    double.TryParse(oModelsProperties.Get(intModel, "replicate_times"), out dblReplicate);
                }
                DataSet ds = oForecast.GetStorage(intID);
               
                string strStorage = "";
                if (ds.Tables[0].Rows.Count > 0)
                {
                    sb.Append("<tr>");
                    sb.Append("<td>&nbsp;</td>");
                    sb.Append("<td>&nbsp;</td>");
                    sb.Append("</tr>");
                    double dblOverall = 0.00;
                    if (ds.Tables[0].Rows[0]["high"].ToString() == "1")
                    {
                        sb.Append("<tr>");
                        sb.Append("<td><b>High Performance</b></td>");
                        double dblTotal = double.Parse(ds.Tables[0].Rows[0]["high_total"].ToString());
                        sb.Append("<td nowrap>");
                        sb.Append(dblTotal.ToString("N"));
                        sb.Append(" GB");
                        double dblReplicated = double.Parse(ds.Tables[0].Rows[0]["high_replicated"].ToString());
                        dblReplicated = dblReplicated * dblReplicate;
                        if (dblReplicated > 0.00)
                        {
                            sb.Append(" + ");
                            sb.Append(dblReplicated.ToString("N"));
                            sb.Append(" GB (<i>Replication</i>)");
                        }
                        double dblHA = double.Parse(ds.Tables[0].Rows[0]["high_ha"].ToString());
                        if (dblHA > 0.00)
                        {
                            sb.Append(" + ");
                            sb.Append(dblHA.ToString("N"));
                            sb.Append(" GB (<i>HA</i>)");
                        }
                        dblTotal = dblTotal + dblReplicated + dblHA;
                       
                        double dblQA = double.Parse(ds.Tables[0].Rows[0]["high_qa"].ToString());
                        dblTotal = dblTotal + dblQA;
                        if (dblQA > 0.00)
                        {
                            sb.Append(" + ");
                            sb.Append(dblQA.ToString("N"));
                            sb.Append(" GB (<i>QA</i>)");
                        }
                        double dblTest = double.Parse(ds.Tables[0].Rows[0]["high_test"].ToString());
                        dblTotal = dblTotal + dblTest;
                        if (dblTest > 0.00)
                        {
                            sb.Append(" + ");
                            sb.Append(dblTest.ToString("N"));
                            sb.Append(" GB (<i>Test</i>)");
                        }
                        sb.Append(" = ");
                        sb.Append(dblTotal.ToString("N"));
                        sb.Append(" GB");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                        if (strStorage != "")
                        {
                            strStorage += " + ";
                        }
                        strStorage += dblTotal.ToString("N") + " GB";
                        dblOverall += dblTotal;
                    }
                    if (ds.Tables[0].Rows[0]["standard"].ToString() == "1")
                    {
                        sb.Append("<tr>");
                        sb.Append("<td><b>Standard Performance</b></td>");
                        double dblTotal = double.Parse(ds.Tables[0].Rows[0]["standard_total"].ToString());
                        sb.Append("<td nowrap>");
                        sb.Append(dblTotal.ToString("N"));
                        sb.Append(" GB");
                        double dblReplicated = double.Parse(ds.Tables[0].Rows[0]["standard_replicated"].ToString());
                        dblReplicated = dblReplicated * dblReplicate;
                        if (dblReplicated > 0.00)
                        {
                            sb.Append(" + ");
                            sb.Append(dblReplicated.ToString("N"));
                            sb.Append(" GB (<i>Replication</i>)");
                        }
                        double dblHA = double.Parse(ds.Tables[0].Rows[0]["standard_ha"].ToString());
                        if (dblHA > 0.00)
                        {
                            sb.Append(" + ");
                            sb.Append(dblHA.ToString("N"));
                            sb.Append(" GB (<i>HA</i>)");
                        }
                        dblTotal = dblTotal + dblReplicated + dblHA;
                       
                        double dblQA = double.Parse(ds.Tables[0].Rows[0]["standard_qa"].ToString());
                        dblTotal = dblTotal + dblQA;
                        if (dblQA > 0.00)
                        {
                            sb.Append(" + ");
                            sb.Append(dblQA.ToString("N"));
                            sb.Append(" GB (<i>QA</i>)");
                        }
                        double dblTest = double.Parse(ds.Tables[0].Rows[0]["standard_test"].ToString());
                        dblTotal = dblTotal + dblTest;
                        if (dblTest > 0.00)
                        {
                            sb.Append(" + ");
                            sb.Append(dblTest.ToString("N"));
                            sb.Append(" GB (<i>Test</i>)");
                        }
                        sb.Append(" = ");
                        sb.Append(dblTotal.ToString("N"));
                        sb.Append(" GB");
                        sb.Append("</td>");
                        sb.Append("</tr>");

                        if (strStorage != "")
                        {
                            strStorage += " + ";
                        }
                        strStorage += dblTotal.ToString("N") + " GB";
                        dblOverall += dblTotal;
                    }
                    if (ds.Tables[0].Rows[0]["low"].ToString() == "1")
                    {
                        sb.Append("<tr>");
                        sb.Append("<td><b>Low Performance</b></td>");
                        double dblTotal = double.Parse(ds.Tables[0].Rows[0]["low_total"].ToString());
                        sb.Append("<td nowrap>");
                        sb.Append(dblTotal.ToString("N"));
                        sb.Append(" GB");
                        double dblReplicated = double.Parse(ds.Tables[0].Rows[0]["low_replicated"].ToString());
                        dblReplicated = dblReplicated * dblReplicate;
                        if (dblReplicated > 0.00)
                        {
                            sb.Append(" + ");
                            sb.Append(dblReplicated.ToString("N"));
                            sb.Append(" GB (<i>Replication</i>)");
                        }
                        double dblHA = double.Parse(ds.Tables[0].Rows[0]["low_ha"].ToString());
                        if (dblHA > 0.00)
                        {
                            sb.Append(" + ");
                            sb.Append(dblHA.ToString("N"));
                            sb.Append(" GB (<i>HA</i>)");
                        }
                        dblTotal = dblTotal + dblReplicated + dblHA;
                        if (ds.Tables[0].Rows[0]["low_level"].ToString().ToUpper() == "HIGH")
                        {
                            dblTotal = dblTotal + dblTotal;
                            sb.Append(" x 2 (<i>High Availability</i>)");
                        }
                        double dblQA = double.Parse(ds.Tables[0].Rows[0]["low_qa"].ToString());
                        dblTotal = dblTotal + dblQA;
                        if (dblQA > 0.00)
                        {
                            sb.Append(" + ");
                            sb.Append(dblQA.ToString("N"));
                            sb.Append(" GB (<i>QA</i>)");
                        }
                        double dblTest = double.Parse(ds.Tables[0].Rows[0]["low_test"].ToString());
                        dblTotal = dblTotal + dblTest;
                        if (dblTest > 0.00)
                        {
                            sb.Append(" + ");
                            sb.Append(dblTest.ToString("N"));
                            sb.Append(" GB (<i>Test</i>)");
                        }
                        sb.Append(" = ");
                        sb.Append(dblTotal.ToString("N"));
                        sb.Append(" GB");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                        if (strStorage != "")
                            strStorage += " + ";
                        strStorage += dblTotal.ToString("N") + " GB";
                        dblOverall += dblTotal;
                    }
                    sb.Append("<tr><td colspan=\"2\"><hr size=\"1\" noshade /></td></tr>");
                    sb.Append("<tr>");
                    sb.Append("<td><b>Total</b></td>");
                    sb.Append("<td>");
                    sb.Append(strStorage);
                    sb.Append(" = <span class=\"error\">");
                    sb.Append(dblOverall.ToString("N"));
                    sb.Append(" GB</span></td>");
                    sb.Append("</tr>");
                }
                sb.Insert(0, "<table cellpadding=\"3\" cellspacing=\"2\" border=\"0\">");
                sb.Append("</table>");

                strSummary = sb.ToString();
            }
        }
    }
}
