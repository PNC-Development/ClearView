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

namespace NCC.ClearView.Presentation.Web
{
    public partial class addhost : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
        private string dsnZeus = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["ZeusDSN"]].ConnectionString;
        protected string strServer = ConfigurationManager.AppSettings["VMWARE_HOST_SERVER"];
        protected string strScript = ConfigurationManager.AppSettings["VMWARE_HOST_SCRIPT"];
        protected int intModel = Int32.Parse(ConfigurationManager.AppSettings["VMWARE_HOST_MODELID"]);
        protected int intLocation = Int32.Parse(ConfigurationManager.AppSettings["OPSLocationID"]);
        protected VMWare oVMWare;
        protected Classes oClass;
        protected Locations oLocation;
        protected Servers oServer;
        protected int intProfile;
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cookies["loginreferrer"].Value = "/admin/vmware/addhost.aspx";
            Response.Cookies["loginreferrer"].Expires = DateTime.Now.AddDays(30);
            if (Request.Cookies["adminid"] != null && Request.Cookies["adminid"].Value != "")
                intProfile = Int32.Parse(Request.Cookies["adminid"].Value);
            else
                Response.Redirect("/admin/login.aspx");
            oVMWare = new VMWare(intProfile, dsn);
            oClass = new Classes(intProfile, dsn);
            oLocation = new Locations(intProfile, dsn);
            oServer = new Servers(intProfile, dsn);
            if (!IsPostBack)
                LoopRepeater();
        }
        private void LoopRepeater()
        {
            DataSet ds = oServer.GetVMwareClustersStatus();
            ds.Relations.Add("relationship", ds.Tables[0].Columns["id"], ds.Tables[1].Columns["parent"], false);
            DataView dv = ds.Tables[0].DefaultView;
            rptView.DataSource = dv;
            rptView.DataBind();
            foreach (RepeaterItem ri in rptView.Items)
            {
                LinkButton btnFix = (LinkButton)ri.FindControl("btnFix");
                btnFix.Attributes.Add("onclick", "return confirm('Are you sure you want to mark this server as fixed and resume the build?');");
                //Repeater rptHistory = (Repeater)ri.FindControl("rptHistory");
                //foreach (RepeaterItem ri2 in rptHistory.Items)
                //{
                //}
            }
        }
        protected void btnFix_Click(Object Sender, EventArgs e)
        {
            LinkButton oButton = (LinkButton)Sender;
            int intServerID = Int32.Parse(oButton.CommandArgument);
            oServer.UpdateError(intServerID, Int32.Parse(oButton.CommandName), 0, 0, true, dsnAsset);
            oVMWare.AddHostNewResult(intServerID, "Last Error marked FIXED!", 0);
            Response.Redirect(Request.Path);
        }
        protected void btnRefresh_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(Request.Path);
        }
    }
}
