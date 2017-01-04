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
    public partial class host_virtual_configure_environment : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);

        protected Asset oAsset;
        protected int intProfile;
        protected int intAsset = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            AuthenticateUser();
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oAsset = new Asset(intProfile, dsnAsset);
            if (Request.QueryString["save"] != null)
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "save", "<script type=\"text/javascript\">alert('Host environments successfully configured!');window.close();<" + "/" + "script>");
            if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
                intAsset = Int32.Parse(Request.QueryString["id"]);
            if (!IsPostBack)
            {
                if (intAsset > 0)
                {
                    LoadLists();
                    DataSet ds = oAsset.GetVirtualHostEnvironment(intAsset);
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        foreach (ListItem oItem in lstEnvironment.Items)
                        {
                            if (oItem.Value == dr["environment"].ToString())
                            {
                                oItem.Selected = true;
                                break;
                            }
                        }
                    }
                }
                else
                    btnSave.Enabled = false;
            }
        }
        public void LoadLists()
        {
            lstEnvironment.Items.Insert(0, new ListItem("CORPDMN", "4"));
            lstEnvironment.Items.Insert(0, new ListItem("CORPTEST", "3"));
            lstEnvironment.Items.Insert(0, new ListItem("CORPDEV", "2"));
        }
        protected void btnSave_Click(Object Sender, EventArgs e)
        {
            oAsset.DeleteVirtualHostEnvironment(intAsset);
            foreach (ListItem oItem in lstEnvironment.Items)
            {
                if (oItem.Selected == true)
                    oAsset.AddVirtualHostEnvironment(intAsset, Int32.Parse(oItem.Value));
            }
            Response.Redirect(Request.Path + "?id=" + intAsset.ToString() + "&save=true");
        }
    }
}
