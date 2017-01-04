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
    public partial class ca_virtual_workstation_rebuild : System.Web.UI.UserControl
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
            Workstations oWorkstation = new Workstations(intProfile, dsn);
            DateTime datOff = DateTime.Today;
            // Get from manaul table
            DataSet dsRebuild = oWorkstation.GetVirtualRebuild(intRequest, intService, intNumber);
            foreach (DataRow drRebuild in dsRebuild.Tables[0].Rows)
            {
                if (drRebuild["cancelled"].ToString() == "")
                {
                    DateTime.TryParse(drRebuild["scheduled"].ToString(), out datOff);
                    break;
                }
            }
            
            if (datOff > DateTime.Now)
            {
                // Cancel automated table
                oWorkstation.UpdateVirtualRebuildCancel(intRequest, intService, intNumber);
            }
            // *****************************************************************************
            // ********* END of Additional Cancel Functionality  ***************************
            // *****************************************************************************
            Page.ClientScript.RegisterStartupScript(typeof(Page), "cancelled", "<script type=\"text/javascript\">ExecuteCancel('" + oService.GetName(intService) + "','" + oPage.GetFullLink(intPage) + "?rid=" + intRequest.ToString() + "');<" + "/" + "script>");
        }
    }
}