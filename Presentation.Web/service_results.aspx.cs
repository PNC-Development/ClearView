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
    public partial class service_results : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected string strTitle = ConfigurationManager.AppSettings["appTitle"];
        protected string strResults = "";
        private Requests oRequests;
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title = strTitle;
            lblTitle.Text = "ClearView Service Results";
            if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
            {
                oRequests = new Requests(0, dsn);
                DataSet ds = oRequests.GetRequestResultsApplication(Request.QueryString["id"]);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    strResults += "<span title=\"" + dr["modified"].ToString() + "\">" + dr["result"].ToString() + "</span>";
                }
            }
        }
    }
}
