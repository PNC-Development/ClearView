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
    public partial class rr_account_move : System.Web.UI.UserControl
    {

        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected Pages oPage;
        protected RequestItems oRequestItem;
        protected Applications oApplication;
        protected AccountRequest oAccountRequest;
        protected Requests oRequest;
        protected Domains oDomain;
        protected Functions oFunction;
        private TreeNode oSelected;
        protected int intProfile;
        protected int intPage = 0;
        protected int intApplication = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oPage = new Pages(intProfile, dsn);
            oRequestItem = new RequestItems(intProfile, dsn);
            oApplication = new Applications(intProfile, dsn);
            oAccountRequest = new AccountRequest(intProfile, dsn);
            oRequest = new Requests(intProfile, dsn);
            oFunction = new Functions(intProfile, dsn, intEnvironment);
            oDomain = new Domains(intProfile, dsn);
            if (Request.QueryString["applicationid"] != null && Request.QueryString["applicationid"] != "")
                intApplication = Int32.Parse(Request.QueryString["applicationid"]);
            if (Request.QueryString["pageid"] != null && Request.QueryString["pageid"] != "")
                intPage = Int32.Parse(Request.QueryString["pageid"]);
            if (Request.Cookies["application"] != null && Request.Cookies["application"].Value != "")
                intApplication = Int32.Parse(Request.Cookies["application"].Value);
            if (Request.QueryString["rid"] != "" && Request.QueryString["rid"] != null)
            {
                if (!IsPostBack)
                    LoadLists();
                LoadValues();
                int intItem = Int32.Parse(lblItem.Text);
                int intApp = oRequestItem.GetItemApplication(intItem);
                string strDeliverable = oApplication.Get(intApp, "deliverables_doc");
                if (strDeliverable != "")
                {
                    panDeliverable.Visible = true;
                    btnDeliverable.Attributes.Add("onclick", "return OpenWindow('NEW_WINDOW','" + strDeliverable + "');");
                }
                if (Request.QueryString["u"] != null && Request.QueryString["u"] != "" && Request.QueryString["d"] != null && Request.QueryString["d"] != "")
                {
                    LoadObjects();
                    btnNext.Attributes.Add("onclick", "return ValidateHidden0('" + hdnParent.ClientID + "','" + oTreeview.ClientID + "','Please select a new location for the account');");
                }
                else
                {
                    btnNext.Enabled = false;
                }
                btnContinue.Attributes.Add("onclick", "return ValidateText('" + txtUser.ClientID + "','Please enter a user ID')" +
                    " && ValidateDropDown('" + ddlDomain.ClientID + "','Please select a domain')" +
                    ";");
            }
            btnCancel1.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this service request?');");
        }
        private void LoadLists()
        {
            ddlDomain.DataValueField = "id";
            ddlDomain.DataTextField = "name";
            ddlDomain.DataSource = oDomain.GetsAccountMaintenance(0, 1);
            ddlDomain.DataBind();
            ddlDomain.Items.Insert(0, new ListItem("-- SELECT --", "0"));
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
        private void LoadObjects()
        {
            string strUser = oFunction.decryptQueryString(Request.QueryString["u"]);
            txtUser.Text = strUser;
            int intDomain = Int32.Parse(oFunction.decryptQueryString(Request.QueryString["d"]));
            ddlDomain.SelectedValue = intDomain.ToString();
            intDomain = Int32.Parse(oDomain.Get(intDomain, "environment"));
            AD oAD = new AD(intProfile, dsn, intDomain);
            lblUser.Text = oAD.GetUserFullName(strUser);
            DirectoryEntry oEntry = oAD.UserSearch(strUser);
            if (oEntry != null)
            {
                panTree.Visible = true;
                lblOld.Text = oEntry.Parent.Properties["distinguishedname"].Value.ToString();
                LoadTree(intDomain);
            }
            else
            {
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "saved", "<script type=\"text/javascript\">alert('The user account could not be found.\\n\\nPlease enter a valid account to continue.');<" + "/" + "script>");
                btnNext.Enabled = false;
            }
        }
        private void LoadTree(int _domain)
        {
            PopulateTree(_domain);
            oTreeview.CollapseAll();
            if (oSelected != null)
            {
                while (oSelected != null)
                {
                    oSelected.Expand();
                    oSelected = oSelected.Parent;
                }
            }
            oTreeview.ExpandAll();
        }
        private void PopulateTree(int _domain)
        {
            Variables oVariable = new Variables(_domain);
            DirectoryEntry oEntry = new DirectoryEntry("LDAP://" + oVariable.primaryDCName(dsn) + "/" + oVariable.LDAP(), oVariable.Domain() + "\\" + oVariable.ADUser(), oVariable.ADPassword());
            DirectorySearcher oSearcher = new DirectorySearcher(oEntry);
            oSearcher.Filter = "(objectClass=organizationalUnit)";
            oSearcher.SearchScope = SearchScope.OneLevel;
            SearchResultCollection oCollection = oSearcher.FindAll();
            foreach (SearchResult oResult in oCollection)
            {
                TreeNode oNode = new TreeNode();
                oNode.Text = oResult.GetDirectoryEntry().Properties["name"].Value.ToString();
                if (oResult.GetDirectoryEntry().Properties["name"].Value.ToString() == "OUc_InfoClients")
                {
                    oNode.Text = oResult.GetDirectoryEntry().Properties["name"].Value.ToString();
                    oNode.ToolTip = oNode.Text;
                    oNode.SelectAction = TreeNodeSelectAction.Expand;
                    PopulateTree2(_domain, "OU=" + oResult.GetDirectoryEntry().Properties["name"].Value.ToString() + ",", oNode);
                    oTreeview.Nodes.Add(oNode);
                }
            }
            oTreeview.ExpandDepth = 1;
            oTreeview.Attributes.Add("oncontextmenu", "return false;");
        }
        private void PopulateTree2(int _domain, string _parent, TreeNode oParent)
        {
            Variables oVariable = new Variables(_domain);
            DirectoryEntry oEntry = new DirectoryEntry("LDAP://" + oVariable.primaryDCName(dsn) + "/" + _parent + oVariable.LDAP(), oVariable.Domain() + "\\" + oVariable.ADUser(), oVariable.ADPassword());
            DirectorySearcher oSearcher = new DirectorySearcher(oEntry);
            oSearcher.Filter = "(objectClass=organizationalUnit)";
            oSearcher.SearchScope = SearchScope.OneLevel;
            SearchResultCollection oCollection = oSearcher.FindAll();
            foreach (SearchResult oResult in oCollection)
            {
                TreeNode oNode = new TreeNode();
                string strName = oResult.GetDirectoryEntry().Properties["name"].Value.ToString();
                string strDistinquished = oResult.GetDirectoryEntry().Properties["distinguishedname"].Value.ToString();
                oNode.Text = strName;
                oNode.ToolTip = strName;
                oNode.SelectAction = TreeNodeSelectAction.Expand;
                if (strDistinquished == lblOld.Text)
                {
                    oSelected = oNode;
                    oNode.ImageUrl = "/images/green_arrow.gif";
                    lblOld.Text = strDistinquished;
                }
                else
                    oNode.NavigateUrl = "javascript:SelectNode('" + strDistinquished + "','" + hdnParent.ClientID + "','" + lblNew.ClientID + "');";
                oParent.ChildNodes.Add(oNode);
                PopulateTree2(_domain, "OU=" + strName + "," + _parent, oNode);
            }
        }
        protected void btnContinue_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(oPage.GetFullLink(intPage) + "?rid=" + Request.QueryString["rid"] + "&u=" + oFunction.encryptQueryString(txtUser.Text) + "&d=" + oFunction.encryptQueryString(ddlDomain.SelectedItem.Value));
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
            oAccountRequest.AddMaintenance(intRequest, intItem, intNumber, "MOVE", txtUser.Text, Int32.Parse(ddlDomain.SelectedItem.Value));
            oAccountRequest.DeleteMaintenanceParameters(intRequest, intItem, intNumber);
            oAccountRequest.AddMaintenanceParameter(intRequest, intItem, intNumber, Request.Form[hdnParent.UniqueID]);
            oRequest.UpdateUnNamedRequest(intRequest, "Account Maintenance");
            oRequestItem.UpdateForm(intRequest, true);
            Response.Redirect(oPage.GetFullLink(intPage) + "?rid=" + intRequest.ToString());
        }
        protected void btnCancel_Click(Object Sender, EventArgs e)
        {
            int intRequest = Int32.Parse(Request.QueryString["rid"]);
            oRequest.Cancel(intRequest);
            Response.Redirect(oPage.GetFullLink(intPage));
        }
    }
}