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
using NCC.ClearView.Presentation.Web.Custom;
using System.DirectoryServices;
namespace NCC.ClearView.Presentation.Web
{
    public partial class rr_account_modify : System.Web.UI.UserControl
    {

        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected string strVirtual = ConfigurationManager.AppSettings["VirtualGatekeeper"];
        protected int intAssignPage = Int32.Parse(ConfigurationManager.AppSettings["AssignWorkload"]);
        protected int intResourceRequestApprove = Int32.Parse(ConfigurationManager.AppSettings["ResourceRequestApprove"]);
        protected int intMaximum = Int32.Parse(ConfigurationManager.AppSettings["MAX_AD_RESULTS"]);
        protected Pages oPage;
        protected RequestItems oRequestItem;
        protected Requests oRequest;
        protected RequestFields oRequestField;
        protected Applications oApplication;
        protected AccountRequest oAccountRequest;
        protected Users oUser;
        protected int intProfile;
        protected int intPage = 0;
        protected int intApplication = 0;
        protected Domains oDomain;
        protected Functions oFunction;
        protected string strMemberships = "";
        protected string strMenuTab1 = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oPage = new Pages(intProfile, dsn);
            oRequestItem = new RequestItems(intProfile, dsn);
            oRequest = new Requests(intProfile, dsn);
            oRequestField = new RequestFields(intProfile, dsn);
            oApplication = new Applications(intProfile, dsn);
            oAccountRequest = new AccountRequest(intProfile, dsn);
            oUser = new Users(intProfile, dsn);
            oDomain = new Domains(intProfile, dsn);
            oFunction = new Functions(intProfile, dsn, intEnvironment);


            //Menus
            int intMenuTab = 0;
            if (Request.QueryString["menu_tab"] != null && Request.QueryString["menu_tab"] != "")
                intMenuTab = Int32.Parse(Request.QueryString["menu_tab"]);
            Tab oTab = new Tab("", intMenuTab, "divMenu1", true, false);
            //Tab oTab = new Tab(hdnType.ClientID, intMenuTab, "divMenu1", true, false);

            oTab.AddTab("Account Properties", "");
            oTab.AddTab("Group Memberships", "");
            strMenuTab1 = oTab.GetTabs();

            //End Menus

            if (Request.QueryString["applicationid"] != null && Request.QueryString["applicationid"] != "")
                intApplication = Int32.Parse(Request.QueryString["applicationid"]);
            if (Request.QueryString["pageid"] != null && Request.QueryString["pageid"] != "")
                intPage = Int32.Parse(Request.QueryString["pageid"]);
            if (Request.Cookies["application"] != null && Request.Cookies["application"].Value != "")
                intApplication = Int32.Parse(Request.Cookies["application"].Value);
            if (Request.QueryString["rid"] != "" && Request.QueryString["rid"] != null)
            {
                LoadValues();
                int intItem = Int32.Parse(lblItem.Text);
                int intApp = oRequestItem.GetItemApplication(intItem);
                string strDeliverable = oApplication.Get(intApp, "deliverables_doc");
                if (strDeliverable != "")
                {
                    panDeliverable.Visible = true;
                    btnDeliverable.Attributes.Add("onclick", "return OpenWindow('NEW_WINDOW','" + strDeliverable + "');");
                }
                if (!IsPostBack)
                {
                    ddlDomain.DataValueField = "id";
                    ddlDomain.DataTextField = "name";
                    ddlDomain.DataSource = oDomain.GetsAccountMaintenance(0, 1);
                    ddlDomain.DataBind();
                    ddlDomain.Items.Insert(0, new ListItem("-- SELECT --", "0"));
                }
                if (Request.QueryString["u"] != null && Request.QueryString["u"] != "" && Request.QueryString["d"] != null && Request.QueryString["d"] != "")
                {
                    if (!IsPostBack)
                        LoadObjects();
                    btnNext.Attributes.Add("onclick", "return ValidateText('" + txtFirst.ClientID + "','Please enter a first name')" +
                        " && ValidateText('" + txtLast.ClientID + "','Please enter a last name')" +
                        ";");
                }
                else
                {
                    btnNext.Enabled = false;
                }
                btnContinue.Attributes.Add("onclick", "return ValidateText('" + txtID.ClientID + "','Please enter a user ID')" +
                    " && ValidateDropDown('" + ddlDomain.ClientID + "','Please select a domain')" +
                    ";");
            }
            btnCancel1.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this service request?');");
        }
        private void LoadValues()
        {
            int intRequest = Int32.Parse(Request.QueryString["rid"]);
            DataSet dsItems = oRequestItem.GetForms(intRequest);
            if (dsItems.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow drItem in dsItems.Tables[0].Rows)
                {
                    if (drItem["done"].ToString() == "-1")
                    {
                        lblItem.Text = drItem["itemid"].ToString();
                        lblNumber.Text = drItem["number"].ToString();
                        break;
                    }
                }
            }
        }
        protected void btnContinue_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(oPage.GetFullLink(intPage) + "?rid=" + Request.QueryString["rid"] + "&u=" + oFunction.encryptQueryString(txtID.Text) + "&d=" + oFunction.encryptQueryString(ddlDomain.SelectedItem.Value));
        }
        private void LoadObjects()
        {
            string strUser = oFunction.decryptQueryString(Request.QueryString["u"]);
            txtID.Text = strUser;
            int intDomain = Int32.Parse(oFunction.decryptQueryString(Request.QueryString["d"]));
            ddlDomain.SelectedValue = intDomain.ToString();
            intDomain = Int32.Parse(oDomain.Get(intDomain, "environment"));
            AD oAD = new AD(intProfile, dsn, intDomain);
            DirectoryEntry oEntry = oAD.UserSearch(strUser);
            Variables oVariable = new Variables(intEnvironment);
            if (oEntry != null)
            {
                if (oEntry.Properties.Contains("distinguishedName") == true)
                    lblLocation.Text = oEntry.Properties["distinguishedName"].Value.ToString();
                if (oEntry.Properties.Contains("givenname") == true)
                    txtFirst.Text = oEntry.Properties["givenname"].Value.ToString();
                if (oEntry.Properties.Contains("sn") == true)
                    txtLast.Text = oEntry.Properties["sn"].Value.ToString();
                if (oEntry.Properties.Contains("whencreated") == true)
                    lblCreated.Text = oEntry.Properties["whencreated"].Value.ToString();
                if (oEntry.Properties.Contains("whenchanged") == true)
                    lblModified.Text = oEntry.Properties["whenchanged"].Value.ToString();
                if (oAD.IsDisabled(oEntry) == true)
                {
                    lblDisabled.Text = "Yes <img src='/images/required.gif' border='0' align='absmiddle'/>";
                    chkEnable.Visible = true;
                }
                else
                    lblDisabled.Text = "No";
                if (oAD.IsLocked(oEntry) == true)
                {
                    lblLocked.Text = "Yes <img src='/images/required.gif' border='0' align='absmiddle'/>";
                    chkUnlock.Visible = true;
                }
                else
                    lblLocked.Text = "No";
                string strGroups = oAD.GetGroups(oEntry);
                string[] strGroup;
                char[] strSplit = { ';' };
                strGroup = strGroups.Split(strSplit);
                // Add Groups
                for (int ii = 0; ii < strGroup.Length; ii++)
                {
                    if (strGroup[ii].Trim() != "")
                    {
                        strMemberships += "<tr><td>" + strGroup[ii].Trim() + "</td><td>[<a href=\"javascript:void(0);\" onclick=\"DeleteList(this,'" + strGroup[ii].Trim() + "','" + hdnGroups.ClientID + "');\">Delete</a>]</td></tr>";
                        hdnGroups.Value += strGroup[ii].Trim() + ";";
                    }
                }
                strMemberships = "<table id=\"tblMemberships\" cellpadding=\"2\" cellspacing=\"1\" border=\"0\">" + strMemberships + "</table>";
                panContinue.Visible = true;
                btnGroup.Attributes.Add("onclick", "return AJAXButtonGet(this,'" + txtGroup.ClientID + "','300','195','" + divGroup.ClientID + "','" + lstGroup.ClientID + "','" + hdnGroup.ClientID + "','" + oVariable.URL() + "/frame/ajax/ajax_ad_groups.aspx',2,'" + divGroupMultiple.ClientID + "','" + intDomain.ToString() + "',null,'" + hdnGroups.ClientID + "','tblMemberships');");
                lstGroup.Attributes.Add("ondblclick", "AJAXClickRow();");
            }
            else
            {
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "saved", "<script type=\"text/javascript\">alert('The user account could not be found.\\n\\nPlease enter a valid account to continue.');<" + "/" + "script>");
                btnNext.Enabled = false;
            }
        }
        protected void btnBack_Click(Object Sender, EventArgs e)
        {
            int intRequest = Int32.Parse(Request.QueryString["rid"]);
            oRequestItem.UpdateForm(intRequest, false);
            Response.Redirect(oPage.GetFullLink(intPage) + "?rid=" + intRequest.ToString());
        }
        protected void btnNext_Click(Object Sender, EventArgs e)
        {
            int intRequest = Int32.Parse(Request.QueryString["rid"]);
            int intItem = Int32.Parse(lblItem.Text);
            int intNumber = Int32.Parse(lblNumber.Text);
            oAccountRequest.AddMaintenance(intRequest, intItem, intNumber, "MODIFY", txtID.Text, Int32.Parse(ddlDomain.SelectedItem.Value));
            oAccountRequest.DeleteMaintenanceParameters(intRequest, intItem, intNumber);
            oAccountRequest.AddMaintenanceParameter(intRequest, intItem, intNumber, txtFirst.Text);
            oAccountRequest.AddMaintenanceParameter(intRequest, intItem, intNumber, txtLast.Text);
            oAccountRequest.AddMaintenanceParameter(intRequest, intItem, intNumber, (chkEnable.Checked ? "1" : "0"));
            oAccountRequest.AddMaintenanceParameter(intRequest, intItem, intNumber, (chkUnlock.Checked ? "1" : "0"));
            oAccountRequest.AddMaintenanceParameter(intRequest, intItem, intNumber, Request.Form[hdnGroups.UniqueID]);
            oRequestItem.UpdateForm(intRequest, true);
            Response.Redirect(oPage.GetFullLink(intPage) + "?rid=" + Request.QueryString["rid"] + "&did=" + ddlDomain.SelectedItem.Value);
        }
        protected void btnCancel_Click(Object Sender, EventArgs e)
        {
            int intRequest = Int32.Parse(Request.QueryString["rid"]);
            oRequest.Cancel(intRequest);
            Response.Redirect(oPage.GetFullLink(intPage));
        }
    }
}