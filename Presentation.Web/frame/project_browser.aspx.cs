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
    public partial class project_browser : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;

        protected Projects oProject;
        protected Requests oRequest;
        protected Services oService;
        protected string strView = "";
        protected string strTitle = "Unavailable";
        protected int intProfile;
        protected void Page_Load(object sender, EventArgs e)
        {
            AuthenticateUser();
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oProject = new Projects(intProfile, dsn);
            oRequest = new Requests(intProfile, dsn);
            oService = new Services(intProfile, dsn);
            if (Request.QueryString["rid"] != null && Request.QueryString["rid"] != "" && Request.QueryString["sid"] != null && Request.QueryString["sid"] != "")
            {
                int intRequest = Int32.Parse(Request.QueryString["rid"]);
                int intService = Int32.Parse(Request.QueryString["sid"]);
                if (oRequest.GetUser(intRequest) == intProfile)
                {
                    DataSet ds = oRequest.GetResult(intRequest);
                    foreach (DataRow dr in ds.Tables[0].Rows)
                        strView += dr["result"].ToString();
                }
                strTitle = oService.GetName(intService);
            }
        }
    }
}
