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
    public partial class ad_sync : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected AD oAD;
        protected Settings oSetting;
        protected int intProfile;
        private int intID = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cookies["loginreferrer"].Value = "/admin/ad_sync.aspx";
            Response.Cookies["loginreferrer"].Expires = DateTime.Now.AddDays(30);
            if (Request.Cookies["adminid"] != null && Request.Cookies["adminid"].Value != "")
                intProfile = Int32.Parse(Request.Cookies["adminid"].Value);
            else
                Response.Redirect("/admin/login.aspx");
            oAD = new AD(intProfile, dsn, intEnvironment);
            oSetting = new Settings(intProfile, dsn);
            Int32.TryParse(Request.QueryString["id"], out intID);
            if (intID > 0)
                txtImport.Text = oAD.GetSync(intID, "results");
            rptImport.DataSource = oAD.GetSync();
            rptImport.DataBind();
            if (!IsPostBack)
                txtSync.Text = oSetting.Get("ad_sync");
            btnSyncNow.Attributes.Add("onclick", "return confirm('WARNING: This import will modify production data and CANNOT be undone!\\n\\nAre you sure you want to continue?')&& ProcessButton(this) && WaitDDL('" + divWait.ClientID + "');");
        }
        protected void btnSyncNow_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(Request.Path + "?id=" + oAD.Sync(0.00, chkSyncNow.Checked).ToString());
        }
        protected void btnSync_Click(Object Sender, EventArgs e)
        {
            oSetting.UpdateADSych(txtSync.Text);
            Response.Redirect(Request.Path);
        }
    }
}
