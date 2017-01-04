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
using NCC.ClearView.Presentation.Web.Custom;
namespace NCC.ClearView.Presentation.Web
{
    public partial class provisioning_status : System.Web.UI.Page
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
        private string dsnServiceEditor = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["ServiceEditorDSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intAssignPage = Int32.Parse(ConfigurationManager.AppSettings["AssignWorkload"]);
        protected int intResourceRequestApprove = Int32.Parse(ConfigurationManager.AppSettings["ResourceRequestApprove"]);
        protected int intViewPage = Int32.Parse(ConfigurationManager.AppSettings["ViewWorkload"]);

        protected int intStorageService = Int32.Parse(ConfigurationManager.AppSettings["SERVICEID_STORAGE"]);
        protected int intBackupService = Int32.Parse(ConfigurationManager.AppSettings["SERVICEID_BACKUP"]);
        protected int intServiceCSM = Int32.Parse(ConfigurationManager.AppSettings["SERVICEID_CSM"]);
        protected int intApplicationCitrix = Int32.Parse(ConfigurationManager.AppSettings["APPLICATIONID_CITRIX"]);
        private int intProfile = 0;
        private Servers oServer;

        protected string strProvisioningStatus = "";
       
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Cookies["profileid"] != null && Request.Cookies["profileid"].Value != "")
                intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
          
            oServer = new Servers(intProfile, dsn);

            Functions oFunction = new Functions(0, dsn, intEnvironment);
            if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
            {

                int intServer = Int32.Parse(oFunction.decryptQueryString(Request.QueryString["id"]));

                strProvisioningStatus = oServer.GetExecution(intServer, intEnvironment, dsnAsset, dsnIP, dsnServiceEditor, intAssignPage, intViewPage);

            }

        }
    }
}
