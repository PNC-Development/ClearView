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
    public partial class frame_service_requests : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected string strSummary = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            Services oService = new Services(0, dsn);
            if (Request.QueryString["rid"] != null && Request.QueryString["rid"] != "")
            {
                DataSet dsSelected = oService.GetSelected(Int32.Parse(Request.QueryString["rid"]));
                foreach (DataRow dr in dsSelected.Tables[0].Rows)
                    strSummary += dr["quantity"].ToString() + ": " + oService.Get(Int32.Parse(dr["serviceid"].ToString()), "name");
            }
        }
    }
}
