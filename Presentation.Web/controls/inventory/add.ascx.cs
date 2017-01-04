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
namespace NCC.ClearView.Presentation.Web
{
    public partial class add : System.Web.UI.UserControl
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intLocation = Int32.Parse(ConfigurationManager.AppSettings["OPSLocationID"]);
        protected Pages oPage;
        protected Asset oAsset;
        protected IPAddresses oIPAddresses;
        protected Orders oOrder;
        protected ModelsProperties oModelsProperties;
        protected Types oType;
        protected int intProfile;
        protected int intApplication = 0;
        protected int intPage = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oPage = new Pages(intProfile, dsn);
            oAsset = new Asset(intProfile, dsnAsset);
            oIPAddresses = new IPAddresses(intProfile, dsnIP, dsn);
            oOrder = new Orders(intProfile, dsnAsset);
            oModelsProperties = new ModelsProperties(intProfile, dsn);
            oType = new Types(intProfile, dsn);
            if (Request.QueryString["applicationid"] != null && Request.QueryString["applicationid"] != "")
                intApplication = Int32.Parse(Request.QueryString["applicationid"]);
            if (Request.QueryString["pageid"] != null && Request.QueryString["pageid"] != "")
                intPage = Int32.Parse(Request.QueryString["pageid"]);
            if (Request.Cookies["application"] != null && Request.Cookies["application"].Value != "")
                intApplication = Int32.Parse(Request.Cookies["application"].Value);
            if (Request.QueryString["add_deploy"] != null)
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "add_deploy", "<script type=\"text/javascript\">alert('The device was successfully deployed');<" + "/" + "script>");
            if (Request.QueryString["add_return"] != null)
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "add_return", "<script type=\"text/javascript\">alert('The device was successfully returned');<" + "/" + "script>");
            if (!IsPostBack)
            {
                int intPlatform = 0;
                if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
                    intPlatform = Int32.Parse(Request.QueryString["id"]);
                if (intPlatform > 0)
                {
                    rptDevices.DataSource = oAsset.Gets(intPlatform, (int)AssetStatus.Arrived);
                    rptDevices.DataBind();
                    foreach (RepeaterItem ri in rptDevices.Items)
                    {
                        Button btnReturn = (Button)ri.FindControl("btnReturn");
                        btnReturn.Attributes.Add("onclick", "return confirm('Are you sure you want to return this device?');");
                        Button btnDeploy = (Button)ri.FindControl("btnDeploy");
                        int intType = oModelsProperties.GetType(Int32.Parse(btnDeploy.CommandName));
                        btnDeploy.Attributes.Add("onclick", "return OpenWindow('ASSET_DEPLOY','" + oType.Get(intType, "asset_deploy_path") + "?id=" + btnDeploy.CommandArgument + "');");
                    }
                    lblNone.Visible = (rptDevices.Items.Count == 0);
                }
            }
        }
        protected void btnReturn_Click(Object Sender, EventArgs e)
        {
            Button oButton = (Button)Sender;
            oAsset.AddStatus(Int32.Parse(oButton.CommandArgument), "", (int)AssetStatus.Returned, intProfile, DateTime.Now);
            Response.Redirect(oPage.GetFullLink(intPage) + "?id=" + Request.QueryString["id"] + "&div=N" + "&add_return=true");
        }
    }
}