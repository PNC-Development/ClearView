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
    public partial class register_controls : System.Web.UI.UserControl
    {

        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected Pages oPage;
        protected Users oUser;
        protected AD oAD;
        protected Variables oVariable;
        protected Functions oFunction;
        protected int intProfile;
        protected int intApplication = 0;
        protected int intPage = 0;
        protected string strMultiple = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oPage = new Pages(intProfile, dsn);
            oUser = new Users(intProfile, dsn);
            oAD = new AD(intProfile, dsn, intEnvironment);
            oVariable = new Variables(intEnvironment);
            oFunction = new Functions(intProfile, dsn, intEnvironment);

            if (Request.QueryString["applicationid"] != null && Request.QueryString["applicationid"] != "")
                intApplication = Int32.Parse(Request.QueryString["applicationid"]);
            if (Request.QueryString["pageid"] != null && Request.QueryString["pageid"] != "")
                intPage = Int32.Parse(Request.QueryString["pageid"]);
            if (Request.Cookies["application"] != null && Request.Cookies["application"].Value != "")
                intApplication = Int32.Parse(Request.Cookies["application"].Value);
            lblTitle.Text = oPage.Get(intPage, "title");
            if (Request.QueryString["add"] != null && Request.QueryString["add"] != "")
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "added", "<script type=\"text/javascript\">alert('User Added Successfully');<" + "/" + "script>");
            if (!IsPostBack)
            {

                if (Request.QueryString["xid"] != null)
                {
                    txtSearchXID.Text = Request.QueryString["xid"];
                    SearchResultCollection oResults = oAD.Search(Request.QueryString["xid"], "cn");
                    if (oResults.Count == 0)
                        Response.Redirect(oPage.GetFullLink(intPage) + "?pid=" + txtSearchXID.Text);
                    else
                        CheckResults(oResults);
                }
                else if (Request.QueryString["pid"] != null)
                {
                    txtSearchXID.Text = Request.QueryString["pid"];
                    SearchResultCollection oCollection = oFunction.eDirectory(txtSearchXID.Text);
                    CheckResults(oCollection);
                }
                else if (Request.QueryString["mail"] != null)
                {
                    txtSearchMail.Text = Request.QueryString["mail"];
                    CheckResults(oAD.Search(Request.QueryString["mail"], "mail"));
                }
                else if (Request.QueryString["fname"] != null)
                {
                    txtSearchFirst.Text = Request.QueryString["fname"];
                    CheckResults(oAD.Search(Request.QueryString["fname"], "givenname"));
                }
                else if (Request.QueryString["lname"] != null)
                {
                    txtSearchLast.Text = Request.QueryString["lname"];
                    CheckResults(oAD.Search(Request.QueryString["lname"], "sn"));
                }
                else
                    panSearch.Visible = true;
                txtSearchXID.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + btnSearch.ClientID + "').click();return false;}} else {return true}; ");
                txtSearchFirst.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + btnSearch.ClientID + "').click();return false;}} else {return true}; ");
                txtSearchLast.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + btnSearch.ClientID + "').click();return false;}} else {return true}; ");
                txtManager.Attributes.Add("onkeyup", "return AJAXTextBoxGet(this,'300','195','" + divManager.ClientID + "','" + lstManager.ClientID + "','" + hdnManager.ClientID + "','" + oVariable.URL() + "/frame/users.aspx',2);");
                lstManager.Attributes.Add("ondblclick", "AJAXClickRow();");
                btnRegister.Attributes.Add("onclick", "return ValidateText('" + txtFirst.ClientID + "','Please enter the first name')" +
                    " && ValidateText('" + txtLast.ClientID + "','Please enter the last name')" +
                    " && ValidateHidden0('" + hdnManager.ClientID + "','" + txtManager.ClientID + "','Please enter the LAN ID of the manager')" +
                    ";");
            }
        }
        protected void CheckResults(SearchResultCollection oResults)
        {
            if (oResults.Count == 0)
            {
                panNone.Visible = true;
                panSearch.Visible = true;
            }
            else if (oResults.Count == 1)
            {
                if (oResults[0].Properties.Contains("extensionattribute10") == true)
                    txtID.Text = oResults[0].GetDirectoryEntry().Properties["extensionattribute10"].Value.ToString();
                if (oResults[0].GetDirectoryEntry().Properties.Contains("businesscategory") == true)
                    txtXID.Text = oResults[0].GetDirectoryEntry().Properties["businesscategory"].Value.ToString();
                else if (oResults[0].Properties.Contains("sAMAccountName") == true)
                    txtXID.Text = oResults[0].GetDirectoryEntry().Properties["sAMAccountName"].Value.ToString();
                else if (oResults[0].Properties.Contains("mailnickname") == true)
                    txtXID.Text = oResults[0].GetDirectoryEntry().Properties["mailnickname"].Value.ToString();

                if (txtID.Text == "" && txtXID.Text != "" && Request.QueryString["xid"] != null)
                {
                    // National City lookup...Try looking up using eDirectory
                    SearchResultCollection oCollection = oFunction.eDirectory("cn", Request.QueryString["xid"]);
                    if (oCollection.Count == 0)
                        oCollection = oFunction.eDirectory("businesscategory", Request.QueryString["xid"]);
                    if (oCollection.Count == 1)
                    {
                        if (oCollection[0].GetDirectoryEntry().Properties.Contains("businesscategory") == true)
                            txtXID.Text = oCollection[0].GetDirectoryEntry().Properties["businesscategory"].Value.ToString();
                        if (oCollection[0].GetDirectoryEntry().Properties.Contains("cn") == true)
                            txtID.Text = oCollection[0].GetDirectoryEntry().Properties["cn"].Value.ToString();
                    }
                }

                //if (txtID.Text == "")
                //    txtID.Text = txtXID.Text;
                //if (txtXID.Text == "")
                //    txtXID.Text = txtID.Text;

                if (txtID.Text != "" && oUser.GetId(txtID.Text) > 0)
                {
                    panRegistered.Visible = true;
                    lblUser.Text = txtID.Text;
                }
                else if (txtXID.Text != "" && oUser.GetId(txtXID.Text) > 0)
                {
                    panRegistered.Visible = true;
                    lblUser.Text = txtXID.Text;
                }
                else
                {
                    panRegister.Visible = true;
                    txtFirst.Text = oResults[0].GetDirectoryEntry().Properties["givenname"].Value.ToString();
                    txtLast.Text = oResults[0].GetDirectoryEntry().Properties["sn"].Value.ToString();
                    if (oResults[0].Properties.Contains("manager") == true)
                    {
                        string strManager = oResults[0].GetDirectoryEntry().Properties["manager"].Value.ToString();
                        DirectoryEntry oManager = new DirectoryEntry("LDAP://" + oVariable.primaryDCName(dsn) + "/" + strManager, oVariable.Domain() + "\\" + oVariable.ADUser(), oVariable.ADPassword());
                        txtManager.Text = oManager.Properties["sAMAccountName"].Value.ToString();
                        int intManager = oUser.GetId(txtManager.Text);
                        if (intManager > 0)
                        {
                            hdnManager.Value = intManager.ToString();
                            txtManager.Text = oUser.GetFullName(intManager) + " (" + oUser.GetName(intManager) + ")";
                        }
                        else
                        {
                            txtManager.Text = "";
                            hdnManager.Value = "0";
                        }
                    }
                    if (oResults[0].Properties.Contains("telephonenumber") == true)
                        txtPhone.Text = oResults[0].GetDirectoryEntry().Properties["telephonenumber"].Value.ToString();
                }
            }
            else
            {
                panMultiple.Visible = true;
                foreach (SearchResult oResult in oResults)
                {
                    if (oResult.Properties.Contains("extensionattribute10") == true)
                    {
                        strMultiple += "<tr onmouseover=\"CellRowOver(this);\" onmouseout=\"CellRowOut(this);\" onclick=\"window.navigate('" + oPage.GetFullLink(intPage) + "?pid=" + oResult.GetDirectoryEntry().Properties["extensionattribute10"].Value.ToString() + "');\">";
                        strMultiple += "<td>" + oResult.GetDirectoryEntry().Properties["extensionattribute10"].Value.ToString() + "</td>";
                    }
                    else if (oResult.Properties.Contains("sAMAccountName") == true)
                    {
                        strMultiple += "<tr onmouseover=\"CellRowOver(this);\" onmouseout=\"CellRowOut(this);\" onclick=\"window.navigate('" + oPage.GetFullLink(intPage) + "?xid=" + oResult.GetDirectoryEntry().Properties["sAMAccountName"].Value.ToString() + "');\">";
                        strMultiple += "<td>" + oResult.GetDirectoryEntry().Properties["sAMAccountName"].Value.ToString() + "</td>";
                    }
                    else if (oResult.Properties.Contains("mail") == true)
                    {
                        strMultiple += "<tr onmouseover=\"CellRowOver(this);\" onmouseout=\"CellRowOut(this);\" onclick=\"window.navigate('" + oPage.GetFullLink(intPage) + "?mail=" + oResult.GetDirectoryEntry().Properties["mail"].Value.ToString() + "');\">";
                        strMultiple += "<td>" + oResult.GetDirectoryEntry().Properties["mailnickname"].Value.ToString() + "</td>";
                    }
                    else
                        strMultiple += "<td></td>";
                    if (oResult.Properties.Contains("givenname") == true)
                        strMultiple += "<td>" + oResult.GetDirectoryEntry().Properties["givenname"].Value.ToString() + "</td>";
                    else
                        strMultiple += "<td></td>";
                    if (oResult.Properties.Contains("sn") == true)
                        strMultiple += "<td>" + oResult.GetDirectoryEntry().Properties["sn"].Value.ToString() + "</td>";
                    else
                        strMultiple += "<td></td>";
                    strMultiple += "</tr>";
                }
            }
        }
        protected void btnSearch_Click(Object Sender, EventArgs e)
        {
            if (txtSearchXID.Text != "")
                Response.Redirect(oPage.GetFullLink(intPage) + "?xid=" + txtSearchXID.Text);
            if (txtSearchMail.Text != "")
                Response.Redirect(oPage.GetFullLink(intPage) + "?mail=" + txtSearchMail.Text);
            if (txtSearchFirst.Text != "")
                Response.Redirect(oPage.GetFullLink(intPage) + "?fname=" + txtSearchFirst.Text);
            if (txtSearchLast.Text != "")
                Response.Redirect(oPage.GetFullLink(intPage) + "?lname=" + txtSearchLast.Text);
        }
        protected void btnRegister_Click(Object Sender, EventArgs e)
        {
            int intGroup = Int32.Parse(ConfigurationManager.AppSettings["CLEARVIEW_USER_GROUPID"]);
            int intManager = Int32.Parse(Request.Form[hdnManager.UniqueID]);
            int intUser = oUser.Register(txtID.Text, txtXID.Text, txtFirst.Text, txtLast.Text, txtPhone.Text, intManager, intGroup, intEnvironment);
            Response.Redirect(oPage.GetFullLink(intPage) + "?add=true");
        }
        protected void btnCancel_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(oPage.GetFullLink(intPage));
        }
    }
}