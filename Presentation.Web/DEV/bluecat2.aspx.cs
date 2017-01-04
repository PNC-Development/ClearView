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
using NCC.ClearView.Application.Core.Proteus;
using NCC.ClearView.Application.Core;
using System.Net;

namespace NCC.ClearView.Presentation.Web.DEV
{
    public partial class bluecat2 : System.Web.UI.Page
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
        private string dsnServiceEditor = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["ServiceEditorDSN"]].ConnectionString;
        private int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        private string strBluecatLogin = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            Variables oVariable = new Variables(intEnvironment);
            //System.Net.NetworkCredential oCredentialsProteus = new System.Net.NetworkCredential(oVariable.ADUser(), oVariable.ADPassword(), oVariable.Domain());
            ProteusAPI oProteusAPI = new ProteusAPI();
            //oProteusAPI.Credentials = oCredentialsProteus;
            oProteusAPI.CookieContainer = new CookieContainer();
            if (string.IsNullOrEmpty(Request.QueryString["proxy"]))
                oProteusAPI.Proxy = oVariable.GetProxy(dsn);
            oProteusAPI.Url = oVariable.BlueCatWebService(dsn);

            // Login
            oProteusAPI = LoginBluecat(oProteusAPI, oVariable);
            if (oProteusAPI == null)
                Response.Write("ERROR: " + "Unable to login (" + strBluecatLogin + ")");
            else
            {
                string strOutput = oProteusAPI.getSystemInfo();
                // Log out
                oProteusAPI.logout();
            }
        }
        private ProteusAPI LoginBluecat(ProteusAPI oProteusAPI, Variables oVariable)
        {
            strBluecatLogin = "";
            try
            {
                oProteusAPI.login(oVariable.BlueCatUsername(), oVariable.BlueCatPassword());
                APIEntity oTest = oProteusAPI.getEntityByName(0, oVariable.BlueCatConfiguration(), "Configuration");
            }
            catch (Exception exLogin)
            {
                //if (exLogin.Message.ToUpper().Contains("HTTP STATUS 503: SERVICE UNAVAILABLE") == true)
                //{
                //    oProteusAPI.Url = oVariable.BlueCatWebService2();
                //    try
                //    {
                //        oProteusAPI.login(oVariable.BlueCatUsername(), oVariable.BlueCatPassword());
                //        APIEntity oTest2 = oProteusAPI.getEntityByName(0, oVariable.BlueCatConfiguration(), "Configuration");
                //    }
                //    catch (Exception exLogin2)
                //    {
                //        strBluecatLogin = exLogin2.Message;
                //        return null;
                //    }
                //}
                //else
                //{
                strBluecatLogin = exLogin.Message;
                return null;
                //}
            }
            return oProteusAPI;
        }
    }
}
