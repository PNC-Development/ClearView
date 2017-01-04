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
    public partial class supply : System.Web.UI.UserControl
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
        protected string strSupply = "";
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
                    DataSet dsTypes = oType.Gets(intPlatform, 1);
                    foreach (DataRow drType in dsTypes.Tables[0].Rows)
                    {
                        bool boolPhysical = false;
                        bool boolBlade = false;
                        bool boolOther = false;
                        int intOldHost = -1;
                        DataSet dsModels = oModelsProperties.GetTypes(1, Int32.Parse(drType["id"].ToString()), 1);
                        foreach (DataRow drModel in dsModels.Tables[0].Rows)
                        {
                            boolOther = !boolOther;
                            int intBlade = Int32.Parse(drModel["blade"].ToString());
                            int intHost = -1;
                            if (drModel["hostid"].ToString() != "")
                                intHost = Int32.Parse(drModel["hostid"].ToString());
                            string strType = "Unknown";
                            if (intBlade == 1)
                            {
                                strType = "Blade";
                                if (boolBlade == false)
                                {
                                    if (sb.ToString() != "")
                                    {
                                        sb.Append("<tr><td colspan=\"4\"><span style=\"width:100%;border-bottom:1 dotted #999999;\"/></td></tr>");
                                    }
                                    sb.Append("<tr><td colspan=\"4\" class=\"bold\">");
                                    sb.Append(strType);
                                    sb.Append("</td></tr>");
                                    boolBlade = true;
                                }
                            }
                            else if (intHost == 0)
                            {
                                strType = "Physical";
                                if (boolPhysical == false)
                                {
                                    if (sb.ToString() != "")
                                    {
                                        sb.Append("<tr><td colspan=\"4\"><p>&nbsp;</p><p>&nbsp;</p></td></tr>");
                                    }
                                    sb.Append("<tr><td colspan=\"4\" class=\"header\">");
                                    sb.Append(drType["name"].ToString());
                                    sb.Append("</td></tr>");
                                    sb.Append("<tr><td colspan=\"4\" class=\"bold\">");
                                    sb.Append(strType);
                                    sb.Append("</td></tr>");
                                    boolPhysical = true;
                                }
                            }
                            else if (intOldHost != intHost)
                            {
                                strType = drModel["host"].ToString();
                                if (intHost > 0)
                                {
                                    if (sb.ToString() != "")
                                    {
                                        sb.Append("<tr><td colspan=\"4\"><span style=\"width:100%;border-bottom:1 dotted #999999;\"/></td></tr>");
                                    }
                                    sb.Append("<tr><td colspan=\"4\" class=\"bold\">");
                                    sb.Append(strType);
                                    sb.Append("</td></tr>");
                                    intOldHost = intHost;
                                }
                            }
                            int intModel = Int32.Parse(drModel["id"].ToString());
                            // Supply
                            DataSet dsModel = oAsset.GetCount(intModel, (int)AssetStatus.Available);
                            int intSupply = dsModel.Tables[0].Rows.Count;
                            sb.Append("<tr");
                            sb.Append(boolOther ? " bgcolor=\"F6F6F6\"" : "");
                            sb.Append("><td nowrap><img src=\"/images/spacer.gif\" border=\"0\" width=\"10\" height=\"1\"/></td><td nowrap>");
                            sb.Append(intSupply == 0 ? drModel["name"].ToString() : "<a href=\"javascript:void(0);\" onclick=\"OpenWindow('INVENTORY_SUPPLY','?model=" + drModel["id"].ToString() + "&status=1');\">" + drModel["name"].ToString() + "</a>");
                            sb.Append(":</td><td width=\"100%\">");
                            sb.Append(oServiceRequest.GetStatusBarFill(((double.Parse(intSupply.ToString()) / double.Parse(intMax.ToString())) * 100.00), "95", false, "#6699CC"));
                            sb.Append("</td><td nowrap>");
                            sb.Append(intSupply.ToString());
                            sb.Append("</td></tr>");
                        }
                    }
                }
            }

            strSupply = sb.ToString();
        }
    }
}