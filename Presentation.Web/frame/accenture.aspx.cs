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
    public partial class accenture : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intForecastPage = Int32.Parse(ConfigurationManager.AppSettings["ForecastEdit"]);
        protected int intViewWorkload = Int32.Parse(ConfigurationManager.AppSettings["ViewWorkload"]);
        protected int intForceOverride = Int32.Parse(ConfigurationManager.AppSettings["ForceForecastOverride"]);
        protected int intConfidenceUnlock = Int32.Parse(ConfigurationManager.AppSettings["CONFIDENCE_UNLOCK"]);
        protected int intPlatformS = Int32.Parse(ConfigurationManager.AppSettings["ServerPlatformID"]);
        protected int intViewPage = Int32.Parse(ConfigurationManager.AppSettings["ViewWorkload"]);
        protected string strIARB = ConfigurationManager.AppSettings["IARB"];
        protected int intIARB = Int32.Parse(ConfigurationManager.AppSettings["IARB_PAGEID"]);
        protected int intProfile;
        protected Forecast oForecast;
        protected Solution oSolution;
        protected Platforms oPlatform;
        protected Pages oPage;
        protected Users oUser;
        protected Functions oFunction;

        protected int intForecast;
        protected int intID = 0;
        protected string strSteps = "";
        private string strEMailIdsBCC = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            AuthenticateUser();
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oForecast = new Forecast(intProfile, dsn);
            oSolution = new Solution(intProfile, dsn);
            oPlatform = new Platforms(intProfile, dsn);
            oPage = new Pages(intProfile, dsn);
            oUser = new Users(intProfile, dsn);
            oFunction = new Functions(intProfile, dsn, intEnvironment);
        }
    }
}
