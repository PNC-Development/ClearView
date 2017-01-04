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
    public partial class sys_personal : System.Web.UI.UserControl
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intProfile;
        protected Users oUser;
        protected void Page_Load(object sender, EventArgs e)
        {
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oUser = new Users(intProfile, dsn);
            DataSet ds = oUser.Get(intProfile);
            if (ds.Tables[0].Rows.Count > 0)
            {
                // Redirect to update page if older than 3 months
                DataRow dr = ds.Tables[0].Rows[0];
                DateTime dtLastModified = DateTime.Parse(dr["modified"].ToString());
                DateTime dtNextUpdates = dtLastModified.AddMonths(3);

                int intSettingsPage = Int32.Parse(ConfigurationManager.AppSettings["SETTINGS_PAGEID"]);
                string strDefault = oUser.GetApplicationUrl(intProfile, intSettingsPage);

                if (dtNextUpdates <= DateTime.Now && strDefault != "")  //Check for 3 Months
                {
                    //string strRedirect = "/updateUserProfile.aspx";
                    Pages oPage = new Pages(0, dsn);
                    string strRedirect = "/" + strDefault + oPage.GetFullLink(intSettingsPage);
                    Response.Redirect(strRedirect);
                }
            }
            if (Request.Path == "/index.aspx")
                Response.Redirect("/interior.aspx");
            lblName.Text = oUser.GetFullName(intProfile) + "&nbsp;&nbsp;(" + oUser.GetName(intProfile).ToUpper() + ")";
            btnDesign.Attributes.Add("onclick", "return OpenWindow('DESIGNER', '');");
        }
    }
}