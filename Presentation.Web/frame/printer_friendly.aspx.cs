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
    public partial class printer_friendly : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        
        protected int intProfile;
        protected Pages oPage;
        protected PageControls oPageControl;
        protected string strPage = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            AuthenticateUser();
            Control oControl;
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            int intPage = 0;
            if (Request.QueryString["page"] != null && Request.QueryString["page"] != "")
                intPage = Int32.Parse(Request.QueryString["page"]);
            oPage = new Pages(intProfile, dsn);
            oPageControl = new PageControls(intProfile, dsn);
            if (intPage > 0)
            {
                this.Page.Title = "ClearView | " + oPage.Get(intPage, "browsertitle");
                // Load Page Controls
                DataSet ds = oPageControl.GetPage(intPage, 1);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    oControl = (Control)LoadControl(Request.ApplicationPath + dr["path"].ToString());
                    PH3.Controls.Add(oControl);
                }
            }
        }
    }
}
