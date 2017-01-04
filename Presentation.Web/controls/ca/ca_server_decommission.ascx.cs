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
using Microsoft.ApplicationBlocks.Data;
using System.Text;
namespace NCC.ClearView.Presentation.Web
{
    public partial class ca_server_decommission : System.Web.UI.UserControl
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
        private string dsnServiceEditor = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["ServiceEditorDSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected Pages oPage;
        protected Services oService;
        protected int intApplication = 0;
        protected int intPage = 0;
        protected string strDone = "";
        protected int intProfile;

        protected void Page_Load(object sender, EventArgs e)
        {
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oPage = new Pages(intProfile, dsn);
            oService = new Services(intProfile, dsn);
            if (Request.QueryString["applicationid"] != null && Request.QueryString["applicationid"] != "")
                intApplication = Int32.Parse(Request.QueryString["applicationid"]);
            if (Request.QueryString["pageid"] != null && Request.QueryString["pageid"] != "")
                intPage = Int32.Parse(Request.QueryString["pageid"]);
            if (Request.Cookies["application"] != null && Request.Cookies["application"].Value != "")
                intApplication = Int32.Parse(Request.Cookies["application"].Value);
            int intRequest = 0;
            Int32.TryParse(Request.QueryString["rid"], out intRequest);
            int intService = 0;
            Int32.TryParse(Request.QueryString["cid"], out intService);
            int intNumber = 0;
            Int32.TryParse(Request.QueryString["num"], out intNumber);
            int intItem = oService.GetItemId(intService);
            // *****************************************************************************
            // ********* Additional Cancel Functionality to be performed here **************
            // *****************************************************************************
            int intServer = 0;
            DateTime datNow = DateTime.Now;
            DateTime datOff = datNow;
            Customized oCustomized = new Customized(intProfile, dsn);
            Asset oAsset = new Asset(intProfile, dsnAsset);
            bool boolAutoFound = false;

            // Get from automated table
            DataSet dsDecomServer = oAsset.GetDecommission(intRequest, intNumber, 2);
            if (dsDecomServer.Tables[0].Rows.Count > 0)
            {
                DateTime.TryParse(dsDecomServer.Tables[0].Rows[0]["decom"].ToString(), out datOff);
                boolAutoFound = true;
            }

            // Get from manaul table
            dsDecomServer = oCustomized.GetDecommissionServer(intRequest, intItem, intNumber);
            if (dsDecomServer.Tables[0].Rows.Count > 0)
            {
                Int32.TryParse(dsDecomServer.Tables[0].Rows[0]["serverid"].ToString(), out intServer);
                if (boolAutoFound == false)
                {
                    if (dsDecomServer.Tables[0].Rows[0]["poweroff_new"].ToString() != "")
                        DateTime.TryParse(dsDecomServer.Tables[0].Rows[0]["poweroff_new"].ToString(), out datOff);
                    else
                        DateTime.TryParse(dsDecomServer.Tables[0].Rows[0]["poweroff"].ToString(), out datOff);
                }
            }
            
            if (intServer > 0)
            {
                Servers oServer = new Servers(intProfile, dsn);
                oServer.UpdateDecommissioned(intServer, "");
                DataSet dsAssets = oServer.GetAssets(intServer);
                foreach (DataRow drAsset in dsAssets.Tables[0].Rows)
                {
                    if (drAsset["latest"].ToString() == "1" || drAsset["dr"].ToString() == "1")
                    {
                        int intAsset = Int32.Parse(drAsset["assetid"].ToString());
                        oServer.UpdateAssetDecom(intServer, intAsset, "");
                    }
                }
            }
            if (datOff > datNow)
            {
                // Cancel automated table
                oAsset.UpdateDecommission(intRequest, intItem, intNumber, -2);
            }
            // *****************************************************************************
            // ********* END of Additional Cancel Functionality  ***************************
            // *****************************************************************************
            Page.ClientScript.RegisterStartupScript(typeof(Page), "cancelled", "<script type=\"text/javascript\">ExecuteCancel('" + oService.GetName(intService) + "','" + oPage.GetFullLink(intPage) + "?rid=" + intRequest.ToString() + "');<" + "/" + "script>");
        }
    }
}