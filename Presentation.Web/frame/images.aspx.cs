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
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using NCC.ClearView.Application.Core;

namespace NCC.ClearView.Presentation.Web
{
    public partial class images : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        private Reports oReports;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["cid"] != null && Request.QueryString["cid"] != "")
            {
                int intId = Int32.Parse(Request.QueryString["cid"]);
                if (intId > 0)
                {
                    oReports = new Reports(0, dsn);
                    DataSet ds = oReports.GetChart(intId);
                    Response.ContentType = "application/text";
                    Response.Write(ds.Tables[0].Rows[0]["url"]);
                    Response.End();
                }
            }
        }
    }
}
