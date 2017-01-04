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
    public partial class action : System.Web.UI.UserControl
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected Platforms oPlatform;
        protected Types oType;
        protected ModelsProperties oModelsProperties;
        protected Asset oAsset;
        protected ServiceRequests oServiceRequest;
        protected Forecast oForecast;
        protected int intProfile;
        protected int intApplication = 0;
        protected int intPage = 0;
        protected string strSummary = "";
        protected int intMax = 50;

        protected void Page_Load(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oPlatform = new Platforms(intProfile, dsn);
            oType = new Types(intProfile, dsn);
            oModelsProperties = new ModelsProperties(intProfile, dsn);
            oAsset = new Asset(intProfile, dsnAsset);
            oServiceRequest = new ServiceRequests(intProfile, dsn);
            oForecast = new Forecast(intProfile, dsn);
            if (Request.QueryString["applicationid"] != null && Request.QueryString["applicationid"] != "")
                intApplication = Int32.Parse(Request.QueryString["applicationid"]);
            if (Request.QueryString["pageid"] != null && Request.QueryString["pageid"] != "")
                intPage = Int32.Parse(Request.QueryString["pageid"]);
            if (Request.Cookies["application"] != null && Request.Cookies["application"].Value != "")
                intApplication = Int32.Parse(Request.Cookies["application"].Value);
            int intPlatform = 0;
            if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
                intPlatform = Int32.Parse(Request.QueryString["id"]);
            if (intPlatform > 0)
            {
                intMax = Int32.Parse(oPlatform.Get(intPlatform, "max_inventory1"));
                if (!IsPostBack)
                {
                    DataSet dsDemand = oForecast.GetAnswersModel(intPlatform);
                    DataTable dtDemand = dsDemand.Tables[0];
                    DataSet dsTypes = oType.Gets(intPlatform, 1);
                    foreach (DataRow drType in dsTypes.Tables[0].Rows)
                    {
                        int intWarning = Int32.Parse(drType["inventory_warning"].ToString());
                        int intCritical = Int32.Parse(drType["inventory_critical"].ToString());
                        bool boolOther = false;
                        DataSet dsModels = oModelsProperties.GetTypes(1, Int32.Parse(drType["id"].ToString()), 1);
                        foreach (DataRow drModel in dsModels.Tables[0].Rows)
                        {
                            int intBlade = Int32.Parse(drModel["blade"].ToString());
                            int intHost = -1;
                            if (drModel["hostid"].ToString() != "")
                                intHost = Int32.Parse(drModel["hostid"].ToString());
                            int intModel = Int32.Parse(drModel["id"].ToString());
                            // Supply
                            DataSet dsModel = oAsset.GetCount(intModel, (int)AssetStatus.Available);
                            int intSupply = dsModel.Tables[0].Rows.Count;
                            // Demand
                            DataRow[] drModels = dtDemand.Select("model = " + intModel.ToString());
                            int intDemand = 0;
                            foreach (DataRow dr in drModels)
                            {
                                intDemand += Int32.Parse(dr["quantity"].ToString());
                                intDemand += Int32.Parse(dr["recovery_number"].ToString());
                            }
                            bool boolWarning = (intDemand > 0 && (intDemand + intWarning) >= intSupply);
                            bool boolCritical = (intDemand > 0 && (intDemand + intCritical) >= intSupply);
                            if (boolWarning || boolCritical)
                            {
                                boolOther = !boolOther;
                                sb = new StringBuilder(strSummary);

                                sb.Append("<tr");
                                sb.Append(boolOther ? " bgcolor=\"F6F6F6\"" : "");
                                sb.Append(" class=\"header\"><td nowrap><img src=\"/images/spacer.gif\" border=\"0\" width=\"10\" height=\"1\"/></td><td colspan=\"3\"><img src=\"/images/");
                                sb.Append(boolCritical ? "bigError.gif" : "bigAlert.gif");
                                sb.Append("\" border=\"0\" align=\"absmiddle\"/> ");
                                sb.Append(boolCritical ? "CRITICAL" : "WARNING");
                                sb.Append(": ");
                                sb.Append(drModel["name"].ToString());
                                sb.Append("</td></tr>");
                                sb.Append("<tr");
                                sb.Append(boolOther ? " bgcolor=\"F6F6F6\"" : "");
                                sb.Append("><td nowrap><img src=\"/images/spacer.gif\" border=\"0\" width=\"10\" height=\"1\"/></td><td nowrap>Supply:</td><td width=\"100%\">");
                                sb.Append(oServiceRequest.GetStatusBarFill(((double.Parse(intSupply.ToString()) / double.Parse(intMax.ToString())) * 100.00), "95", false, "#6699CC"));
                                sb.Append("</td><td nowrap>");
                                sb.Append(intSupply.ToString());
                                sb.Append("</td></tr>");
                                sb.Append("<tr");
                                sb.Append(boolOther ? " bgcolor=\"F6F6F6\"" : "");
                                sb.Append("><td nowrap><img src=\"/images/spacer.gif\" border=\"0\" width=\"10\" height=\"1\"/></td><td nowrap>Demand:</td><td width=\"100%\">");
                                sb.Append(oServiceRequest.GetStatusBarFill(((double.Parse(intDemand.ToString()) / double.Parse(intMax.ToString())) * 100.00), "95", false, "#CC3300"));
                                sb.Append("</td><td nowrap>");
                                sb.Append(intDemand.ToString());
                                sb.Append("</td></tr>");
                                
                                strSummary = sb.ToString();
                            }
                        }
                    }

                    if (strSummary == "")
                    {
                        strSummary = "<tr><td class=\"header\"><img src=\"/images/bigCheck.gif\" border=\"0\" align=\"absmiddle\"/> No Action Required</td></tr>";
                    }
                }
            }
        }
    }
}