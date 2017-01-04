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
using System.Net;
using System.IO;
using System.Xml;
namespace NCC.ClearView.Presentation.Web
{
    public partial class rss_hp_server : System.Web.UI.UserControl
    {

        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected Pages oPage;
        protected AppPages oAppPage;
        protected int intProfile;
        protected int intPage = 0;
        protected int intApplication = 0;
        protected string strXml = "http://h10068.www1.hp.com/blogpost/rss_servers.xml";
        //string strXml = "http://www.nytimes.com/services/xml/rss/nyt/HomePage.xml";
        int intCount = 10;
        protected void Page_Load(object sender, EventArgs e)
        {
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oPage = new Pages(intProfile, dsn);
            oAppPage = new AppPages(intProfile, dsn);
            if (Request.QueryString["applicationid"] != null && Request.QueryString["applicationid"] != "")
                intApplication = Int32.Parse(Request.QueryString["applicationid"]);
            if (Request.QueryString["pageid"] != null && Request.QueryString["pageid"] != "")
                intPage = Int32.Parse(Request.QueryString["pageid"]);
            if (Request.Cookies["application"] != null && Request.Cookies["application"].Value != "")
                intApplication = Int32.Parse(Request.Cookies["application"].Value);
            lblTitle.Text = "Hewlett Packard Server Advisories";
            bool boolLogin = false;
            if (encryptUtil.CookieUtil.GetEncryptedCookieValue("proxyuser") != null && encryptUtil.CookieUtil.GetEncryptedCookieValue("proxyuser") != "" && encryptUtil.CookieUtil.GetEncryptedCookieValue("proxypass") != null && encryptUtil.CookieUtil.GetEncryptedCookieValue("proxypass") != "")
            {
                try
                {
                    WebRequest oRequest = WebRequest.Create(strXml);
                    oRequest.Proxy = new WebProxy("http://authproxy.ntl-city.com:8080");
                    oRequest.Proxy.Credentials = new NetworkCredential(encryptUtil.CookieUtil.GetEncryptedCookieValue("proxyuser"), encryptUtil.CookieUtil.GetEncryptedCookieValue("proxypass"));
                    WebResponse oResponse = oRequest.GetResponse();
                    StreamReader oReader = new StreamReader(oResponse.GetResponseStream());
                    XmlTextReader oTextReader = new XmlTextReader(oReader);
                    XmlDocument oDocument = new XmlDocument();
                    oDocument.Load(oTextReader);
                    XmlNodeReader oNode = new XmlNodeReader(oDocument);
                    DataSet ds = new DataSet();
                    ds.ReadXml(oNode);
                    int intDelete = ds.Tables[5].Rows.Count - intCount;
                    for (int ii = intDelete; ii > 0; ii--)
                        ds.Tables[5].Rows[intCount].Delete();
                    DataView dv = ds.Tables[5].DefaultView;
                    rptView.DataSource = dv;
                    rptView.DataBind();
                    lblNone.Visible = (dv.Count == 0);
                }
                catch { boolLogin = true; }
            }
            else
                boolLogin = true;

            if (boolLogin == true)
            {
                panLogin.Visible = true;
                btnLogin.Attributes.Add("onclick", "return ValidateText('" + txtUsername.ClientID + "','Please enter a username') && ValidateText('" + txtPassword.ClientID + "','Please enter a password');");
                txtUsername.Focus();
            }
            else
            {
                panRSS.Visible = true;
                btnLogout.Attributes.Add("onclick", "return confirm('Are you sure you want to clear your credentials?');");
            }
        }
        protected void btnLogin_Click(Object Sender, EventArgs e)
        {
            DateTime _expire;
            if (chkRemember.Checked == true)
                _expire = DateTime.Today.AddMonths(1);
            else
                _expire = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59, 59);
            encryptUtil.CookieUtil.SetEncryptedCookie("proxyuser", txtUsername.Text, _expire);
            encryptUtil.CookieUtil.SetEncryptedCookie("proxypass", txtPassword.Text, _expire);
            Redirect();
        }
        protected void btnLogout_Click(Object Sender, EventArgs e)
        {
            encryptUtil.CookieUtil.SetEncryptedCookie("proxyuser", "");
            encryptUtil.CookieUtil.SetEncryptedCookie("proxypass", "");
            Redirect();
        }
        protected void Redirect()
        {
            if (intPage > 0)
                Response.Redirect(oPage.GetFullLink(intPage));
            else
            {
                Response.Redirect("/interior.aspx");
            }
        }
    }
}