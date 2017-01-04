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
using Microsoft.ApplicationBlocks.Data;
using NCC.ClearView.Application.Core;

namespace NCC.ClearView.Presentation.Web
{
    public partial class search_archived : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private Customized oCustomized;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["s"] != null)
            {
                oCustomized = new Customized(0, dsn);
                DataSet ds = oCustomized.GetWMServerArchiveServerNames();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    string strValue = dr["servername"].ToString().ToUpper();
                    if (strValue.StartsWith(Request.QueryString["s"].ToUpper()) == false)
                        dr.Delete();
                }
                rptView.DataSource = ds;
                rptView.DataBind();
                lblNone.Visible = (rptView.Items.Count == 0);
            }
        }
    }
}
