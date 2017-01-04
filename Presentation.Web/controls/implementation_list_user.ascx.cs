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
    public partial class implementation_list_user : System.Web.UI.UserControl
    {

        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected Pages oPage;
        protected AppPages oAppPage;
        protected int intProfile;
        protected int intPage = 0;
        protected int intApplication = 0;
        protected ResourceRequest oResourceRequest;
        protected void Page_Load(object sender, EventArgs e)
        {
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oPage = new Pages(intProfile, dsn);
            oAppPage = new AppPages(intProfile, dsn);
            oResourceRequest = new ResourceRequest(intProfile, dsn);
            if (Request.QueryString["applicationid"] != null && Request.QueryString["applicationid"] != "")
                intApplication = Int32.Parse(Request.QueryString["applicationid"]);
            if (Request.QueryString["pageid"] != null && Request.QueryString["pageid"] != "")
                intPage = Int32.Parse(Request.QueryString["pageid"]);
            if (Request.Cookies["application"] != null && Request.Cookies["application"].Value != "")
                intApplication = Int32.Parse(Request.Cookies["application"].Value);
            DateTime _date = DateTime.Today;
            lblTitle.Text = "My Upcoming Changes for " + _date.ToLongDateString();
            DataSet ds = oResourceRequest.GetChangeControlsUser(intProfile);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (DateTime.Parse(dr["implementation"].ToString()).ToShortDateString() != _date.ToShortDateString())
                    dr.Delete();
            }
            rptView.DataSource = ds;
            rptView.DataBind();
            lblNone.Visible = (rptView.Items.Count == 0);
        }
    }
}