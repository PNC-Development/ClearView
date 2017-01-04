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
    public partial class host_virtual_configure : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        
        protected Asset oAsset;
        protected int intProfile;
        protected int intHost = 0;
        protected int intAsset = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            AuthenticateUser();
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oAsset = new Asset(intProfile, dsnAsset);
            if (Request.QueryString["save"] != null)
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "save", "<script type=\"text/javascript\">alert('Host successfully configured!');window.close();<" + "/" + "script>");
            if (Request.QueryString["hostid"] != null && Request.QueryString["hostid"] != "")
                intHost = Int32.Parse(Request.QueryString["hostid"]);
            if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
                intAsset = Int32.Parse(Request.QueryString["id"]);
            if (!IsPostBack)
            {
                if (intHost > 0 && intAsset > 0)
                {
                    DataSet ds = oAsset.GetVirtualHost(intAsset);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        btnSave.Text = "Update";
                        txtGuests.Text = ds.Tables[0].Rows[0]["guests"].ToString();
                        txtProcessors.Text = ds.Tables[0].Rows[0]["processors"].ToString();
                    }
                    btnSave.Attributes.Add("onclick", "return ValidateNumber('" + txtGuests.ClientID + "','Please enter a valid number of guests')" +
                        " && ValidateNumber0('" + txtProcessors.ClientID + "','Please enter a valid number of processors')" +
                        ";");
                    btnOS.Attributes.Add("onclick", "return OpenWindow('DEPLOY_HOST_OS','?id=" + intAsset.ToString() + "');");
                    btnEnvironment.Attributes.Add("onclick", "return OpenWindow('DEPLOY_HOST_ENVIRONMENT','?id=" + intAsset.ToString() + "');");
                }
                else
                    btnSave.Enabled = false;
            }
        }
        protected void btnSave_Click(Object Sender, EventArgs e)
        {
            oAsset.AddVirtualHost(intAsset, intHost, Int32.Parse(txtGuests.Text), double.Parse(txtProcessors.Text));
            Response.Redirect(Request.Path + "?hostid=" + intHost.ToString() + "&id=" + intAsset.ToString() + "&save=true");
        }
    }
}
