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
using System.DirectoryServices;

namespace NCC.ClearView.Presentation.Web
{
    public partial class Login : BasePage
    {
        protected string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected Users oUser;

        protected void Page_Load()
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
            //int intUserId = oUser.Login(txtUsername.Text, txtPassword.Text, intEnvironment, true, false);
            int intUserId = LoginUser(txtUsername.Text, txtPassword.Text, intEnvironment, true, false);
            if (intUserId == -2)
            {
                Response.Cookies["adminid"].Value = intUserId.ToString();
                Response.Redirect("/admin/admin_index.aspx");
            }
            else
            {
                if (intUserId < 1)
                    Response.Redirect(Request.Path + "?error=" + intUserId.ToString());
                else
                {
                    Response.Cookies["profileid"].Value = intUserId.ToString();
                    if (chkRemember.Checked == true)
                        Response.Cookies["profileid"].Expires = DateTime.Now.AddDays(30);
                    Redirect();
                }
            }
        }
        public int LoginUser(string _username, string _password, int _environment, bool _log, bool _admin)
        {
            Variables oNCB = new Variables(_environment);
            int _id = oUser.GetId(_username);
            if (_id != 0)
            {
                DirectoryEntry oEntry = new DirectoryEntry(oNCB.primaryDC(dsn), oNCB.Domain() + "\\" + _username, _password);
                DirectorySearcher oSearcher = new DirectorySearcher(oEntry);
                oSearcher.Filter = "(objectCategory=user)";
                try
                {
                    SearchResult oResult = oSearcher.FindOne();
                    oUser.AddLogin(_username);
                    return _id;
                }
                catch
                {
                    // ADD PNC Authentication
                    if (_environment == (int)CurrentEnvironment.CORPDMN)
                    {
                        Variables oPNC = new Variables((int)CurrentEnvironment.PNCNT_PROD);
                        DirectoryEntry oPNCEntry = new DirectoryEntry(oPNC.primaryDC(dsn), oPNC.Domain() + "\\" + _username, _password);
                        DirectorySearcher oPNCSearcher = new DirectorySearcher(oPNCEntry);
                        oSearcher.Filter = "(objectCategory=user)";
                        try
                        {
                            SearchResult oPNCResult = oPNCSearcher.FindOne();
                            oUser.AddLogin(_username);
                            return _id;
                        }
                        catch
                        {
                            // ADD PNC Authentication
                            return -10;
                        }
                    }
                    else if (_environment == (int)CurrentEnvironment.PNCNT_PROD)
                    {
                        oNCB = new Variables((int)CurrentEnvironment.CORPDMN);
                        DirectoryEntry oNCBEntry = new DirectoryEntry(oNCB.primaryDC(dsn), oNCB.Domain() + "\\" + _username, _password);
                        DirectorySearcher oNCBSearcher = new DirectorySearcher(oNCBEntry);
                        oNCBSearcher.Filter = "(objectCategory=user)";
                        try
                        {
                            SearchResult oNCBResult = oNCBSearcher.FindOne();
                            oUser.AddLogin(_username);
                            return _id;
                        }
                        catch
                        {
                            // ADD PNC Authentication
                            return -10;
                        }
                    }
                    else
                        return -1;
                }
            }
            else
            {
                return 0;
            }
        }
        private void Redirect()
        {
            // Checking for modifications

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
