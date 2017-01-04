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
    public partial class sys_link : System.Web.UI.UserControl
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected Pages oPage;
        protected Variables oVariable;
        protected int intProfile;
        protected int intPage = 0;
        protected int intApplication = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oPage = new Pages(intProfile, dsn);
            oVariable = new Variables(intEnvironment);
            if (Request.QueryString["applicationid"] != null && Request.QueryString["applicationid"] != "")
                intApplication = Int32.Parse(Request.QueryString["applicationid"]);
            if (Request.QueryString["pageid"] != null && Request.QueryString["pageid"] != "")
                intPage = Int32.Parse(Request.QueryString["pageid"]);
            if (Request.Cookies["application"] != null && Request.Cookies["application"].Value != "")
                intApplication = Int32.Parse(Request.Cookies["application"].Value);
            lblTitle.Text = oPage.Get(intPage, "title");
            txtUser.Attributes.Add("onkeyup", "return AJAXTextBoxGet(this,'300','195','" + divUser.ClientID + "','" + lstUser.ClientID + "','" + hdnUser.ClientID + "','" + oVariable.URL() + "/frame/users.aspx',2);");
            lstUser.Attributes.Add("ondblclick", "AJAXClickRow();");
            btnSubmit.Attributes.Add("onclick", "return ValidateHidden('" + hdnUser.ClientID + "','" + txtUser.ClientID + "','Please enter a valid LAN ID') &&  ValidateDropDown('" + ddlPages.ClientID + "','Please select a page to be linked');");
            if (Request.QueryString["link"] != null)
                lblCommunication.Visible = true;
            if (!IsPostBack)
            {
                int intParent = Int32.Parse(oPage.Get(intPage, "parent"));
                ddlPages.DataValueField = "pageid";
                ddlPages.DataTextField = "title";
                ddlPages.DataSource = oPage.Gets(intApplication, intProfile, intParent, 1, 1);
                ddlPages.DataBind();
                ddlPages.Items.Insert(0, new ListItem("-- SELECT --", "0"));
            }
        }
        protected void btnSubmit_Click(Object Sender, EventArgs e)
        {
            int intUser = Int32.Parse(Request.Form[hdnUser.UniqueID]);
            Functions oFunction = new Functions(intProfile, dsn, intEnvironment);
            Users oUser = new Users(intProfile, dsn);
            int intLinked = Int32.Parse(ddlPages.SelectedItem.Value);
            string strDefault = oUser.GetApplicationUrl(intUser, intLinked);
            oFunction.SendEmail("ClearView | Send a Link", oUser.GetName(intUser), "", "", "ClearView - Send a Link", "<p>" + oUser.GetFullName(intProfile) + " has sent you a link in ClearView.</p><p><a href=\"" + oVariable.URL() + "/redirect.aspx?referrer=/" + strDefault + oPage.GetFullLink(intLinked) + "\" target=\"_blank\">Click here to go to this link.</a></p>", true, false);
            Response.Redirect(oPage.GetFullLink(intPage) + "?link=sent");
        }
    }
}