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
    public partial class zeus : BasePage
    {
        private string dsnZeus = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["ZeusDSN"]].ConnectionString;
        
        protected int intProfile;
        protected Zeus oZeus;
        protected void Page_Load(object sender, EventArgs e)
        {
            AuthenticateUser();
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oZeus = new Zeus(intProfile, dsnZeus);
            if (Request.QueryString["s"] != null && Request.QueryString["s"] != "")
            {
                rptZeus.DataSource = oZeus.GetResults(Request.QueryString["s"]);
                rptZeus.DataBind();
                lblZeus.Visible = (rptZeus.Items.Count == 0);
            }
        }
    }
}
