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
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;

namespace NCC.ClearView.Presentation.Web
{
    public partial class item_amount : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        
        protected Customized oCustomized;
        protected int intProfile;
        protected void Page_Load(object sender, EventArgs e)
        {
            AuthenticateUser();
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            string strValue = String.Format("{0:C}", "0");
            int intId = 0;
            if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
            {
                oCustomized = new Customized(intProfile, dsn);
                Int32.TryParse(Request.QueryString["id"], out intId);
                DataSet ds = oCustomized.GetItem(intId);
                if (ds.Tables[0].Rows.Count > 0)
                    strValue = String.Format("{0:C}", ds.Tables[0].Rows[0]["amount"]);

            }
            Response.ContentType = "application/text";
            Response.Write(strValue);
            Response.End();
        }
    }
}
