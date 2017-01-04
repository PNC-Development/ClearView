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
    public partial class server_error : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
        private string dsnZeus = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["ZeusDSN"]].ConnectionString;
        protected int intProfile;
        protected void Page_Load(object sender, EventArgs e)
        {
            AuthenticateUser();
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            Servers oServer = new Servers(intProfile, dsn);
            Asset oAsset = new Asset(intProfile, dsnAsset);
            Zeus oZeus = new Zeus(intProfile, dsnZeus);
            if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
            {
                int intID = Int32.Parse(Request.QueryString["id"]);
                if (Request.QueryString["step"] != null && Request.QueryString["step"] != "")
                {
                    int intAsset = 0;
                    DataSet ds = oServer.GetErrors(intID);
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        if (dr["assetid"].ToString() != "")
                            intAsset = Int32.Parse(dr["assetid"].ToString());
                    }
                    oServer.UpdateError(intID, Int32.Parse(Request.QueryString["step"]), 0, 0, true, dsnAsset);
                    if (intAsset > 0)
                    {
                        string strSerial = oAsset.Get(intAsset, "serial");
                        oZeus.UpdateResults(strSerial);
                    }
                    Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "code", "<script type=\"text/javascript\">parent.navigate(parent.location);<" + "/" + "script>");
                }
            }
        }
    }
}
