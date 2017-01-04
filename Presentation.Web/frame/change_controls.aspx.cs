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
    public partial class change_controls : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intProfile;
        protected ResourceRequest oResourceRequest;
        protected Users oUser;
        protected string strDay;
        protected void Page_Load(object sender, EventArgs e)
        {
            AuthenticateUser();
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oResourceRequest = new ResourceRequest(intProfile, dsn);
            oUser = new Users(intProfile, dsn);
            if (Request.QueryString["d"] != null && Request.QueryString["d"] != "")
            {
                DateTime _date = DateTime.Parse(Request.QueryString["d"]);
                strDay = _date.ToLongDateString();
                DataSet ds = oResourceRequest.GetChangeControlsDate(_date);
                rptView.DataSource = ds;
                rptView.DataBind();
                lblNone.Visible = (rptView.Items.Count == 0);
            }
        }
        protected void btnView_Click(Object Sender, EventArgs e)
        {
            LinkButton oLink = (LinkButton)Sender;
            Response.Redirect("/frame/change_control.aspx?id=" + oLink.CommandArgument);
        }
    }
}
