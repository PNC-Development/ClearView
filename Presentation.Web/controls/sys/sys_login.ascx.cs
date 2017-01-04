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
    public partial class sys_login : System.Web.UI.UserControl
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected Users oUser;
        protected int intProfile;
        protected void Page_Load(object sender, EventArgs e)
        {
            lblInvalid.Visible = false;
            oUser = new Users(0, dsn);
            if (Request.Cookies["profileid"] != null && Request.Cookies["profileid"].Value != "")
            {
                oUser.AddLogin(oUser.GetName(Int32.Parse(Request.Cookies["profileid"].Value)));
                Redirect();
            }
            else
            {
                if (Request.QueryString["logout"] != null)
                    lblLogout.Visible = true;
                if (Request.QueryString["error"] != null)
                    lblInvalid.Visible = true;
                txtUsername.Focus();
                btnLogin.Attributes.Add("onclick", "return ValidateText('" + txtUsername.ClientID + "','Please enter a username') && ValidateText('" + txtPassword.ClientID + "','Please enter a password');");
            }
        }
        protected void btnLogin_Click(Object Sender, EventArgs e)
        {
            int intUserId = oUser.Login(txtUsername.Text, txtPassword.Text, intEnvironment, true, false);
            if (intUserId == -2)
            {
                Response.Cookies["adminid"].Value = intUserId.ToString();
                Response.Redirect("/admin/admin_index.aspx");
            }
            else
            {
                if (intUserId < 1)
                    Response.Redirect(Request.Path + "?error=true");
                else
                {
                    Response.Cookies["profileid"].Value = intUserId.ToString();
                    intProfile = intUserId;
                    if (chkRemember.Checked == true)
                        Response.Cookies["profileid"].Expires = DateTime.Now.AddDays(30);
                    Redirect();
                }
            }
        }
        private void Redirect()
        {
            //Check if user record is updated in last 3 months
            Response.Cookies["updateuserprofile"].Value = "false";
            DataSet ds = new DataSet();
            ds = oUser.Get(intProfile);
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
            else
            {
                string strRedirect = "/index.aspx";
                if (Request.QueryString["referrer"] != null)
                {
                    strRedirect = Request.QueryString["referrer"];
                    for (int ii = 1; ii < Request.QueryString.Count; ii++)
                        strRedirect += "&" + Request.QueryString.AllKeys[ii] + "=" + Request.QueryString[ii];
                }
                if (Request.Cookies["userloginreferrer"] != null && Request.Cookies["userloginreferrer"].Value != "")
                {
                    strRedirect = Request.Cookies["userloginreferrer"].Value;
                    Response.Cookies["userloginreferrer"].Value = "";
                }

                Response.Redirect(strRedirect);
            }

        }
    }
}