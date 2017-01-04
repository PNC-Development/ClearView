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
    public partial class change_user : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected Users oUser;
        protected int intProfile;
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cookies["loginreferrer"].Value = "/admin/change_user.aspx";
            Response.Cookies["loginreferrer"].Expires = DateTime.Now.AddDays(30);
            if (Request.Cookies["adminid"] != null && Request.Cookies["adminid"].Value != "")
                intProfile = Int32.Parse(Request.Cookies["adminid"].Value);
            else
                Response.Redirect("/admin/login.aspx");
            oUser = new Users(intProfile, dsn);
            if (!IsPostBack)
            {
                if (Request.Cookies["profileid"] != null && Request.Cookies["profileid"].Value != "")
                {
                    int intUser = Int32.Parse(Request.Cookies["profileid"].Value);
                    txtUser.Text = oUser.GetFullName(intUser) + " (" + oUser.GetName(intUser) + ")";
                }
                Variables oVariable = new Variables(intEnvironment);
                txtUser.Attributes.Add("onkeyup", "return AJAXTextBoxGet(this,'300','195','" + divAJAX.ClientID + "','" + lstAJAX.ClientID + "','hdnAJAXValue','" + oVariable.URL() + "/frame/userids.aspx',2);");
                lstAJAX.Attributes.Add("ondblclick", "AJAXClickRow();");
            }
        }
        protected void btnSubmit_Click(Object Sender, EventArgs e)
        {
            if (Request.Form["hdnAJAXValue"] != null && Request.Form["hdnAJAXValue"] != "")
            {
                Response.Cookies["profileid"].Value = Request.Form["hdnAJAXValue"];
                Response.Redirect(Request.Path);
            }
        }
    }
}
