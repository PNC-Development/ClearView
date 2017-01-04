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
    public partial class pages : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected Pages oPage;
        protected Applications oApplication;
        protected Templates oTemplate;
        protected int intProfile;
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cookies["loginreferrer"].Value = "/admin/pages.aspx";
            Response.Cookies["loginreferrer"].Expires = DateTime.Now.AddDays(30);
            if (Request.Cookies["adminid"] != null && Request.Cookies["adminid"].Value != "")
                intProfile = Int32.Parse(Request.Cookies["adminid"].Value);
            else
                Response.Redirect("/admin/login.aspx");
            oPage = new Pages(intProfile, dsn);
            oApplication = new Applications(intProfile, dsn);
            oTemplate = new Templates(intProfile, dsn);
            if (!IsPostBack)
            {
                LoadLists();
                LoadPages(oTreeview, null, 0);
                oTreeview.ExpandDepth = 0;
                btnParent.Attributes.Add("onclick", "return OpenWindow('PAGEBROWSER','" + hdnParent.ClientID + "','&control=" + hdnParent.ClientID + "&controltext=" + lblParent.ClientID + "',false,400,600);");
                btnRelated.Attributes.Add("onclick", "return OpenWindow('PAGEBROWSER','" + hdnRelated.ClientID + "','&control=" + hdnRelated.ClientID + "&controltext=" + lblRelated.ClientID + "',false,400,600);");
                btnOrder.Attributes.Add("onclick", "return OpenWindow('PAGEORDER','" + hdnId.ClientID + "','" + hdnOrder.ClientID + "',false,400,400);");
                btnDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this page?');");
                btnCancel.Attributes.Add("onclick", "return Cancel();");
                btnFill.Attributes.Add("onclick", "return FillTitle('" + txtTitle.ClientID + "','" + txtUrlTitle.ClientID + "','" + txtMenuTitle.ClientID + "','" + txtBrowser.ClientID + "');");
                btnBrowseNav.Attributes.Add("onclick", "return OpenWindow('IMAGEPATH','','" + txtNav.ClientID + "',false,500,550);");
                btnBrowseOver.Attributes.Add("onclick", "return OpenWindow('IMAGEPATH','','" + txtNavOver.ClientID + "',false,500,550);");
                btnControls.Attributes.Add("onclick", "return OpenWindow('CONTROLS','" + hdnId.ClientID + "','1',false,'1000',300);");
            }
        }
        private void LoadLists()
        {
            DataSet ds = oTemplate.Gets(1);
            ddlTemplate.DataTextField = "name";
            ddlTemplate.DataValueField = "templateid";
            ddlTemplate.DataSource = ds;
            ddlTemplate.DataBind();
            ddlTemplate.Items.Insert(0, new ListItem("-- SELECT --", "0"));
        }
        private void LoadPages(TreeView oTree, TreeNode oParent, int _parent)
        {
            DataSet ds = oPage.Gets(_parent, 0);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                TreeNode oNode = new TreeNode();
                oNode.Text = dr["title"].ToString();
                oNode.ToolTip = dr["title"].ToString();
                string strRelated = "No Related Page";
                if (dr["related"].ToString() != "0")
                    strRelated = oPage.Get(Int32.Parse(dr["related"].ToString()), "title");
                string strParent = "No Parent Page";
                if (dr["parent"].ToString() != "0")
                    strParent = oPage.Get(_parent, "title");
                oNode.NavigateUrl = "javascript:Edit('" + dr["pageid"].ToString() + "','" + strParent + "','" + dr["parent"].ToString() + "','" + dr["title"].ToString() + "','" + dr["urltitle"].ToString() + "','" + dr["menutitle"].ToString() + "','" + dr["browsertitle"].ToString() + "','" + dr["templateid"].ToString() + "','" + strRelated + "','" + dr["related"].ToString() + "','" + dr["navimage"].ToString() + "','" + dr["navoverimage"].ToString() + "','" + dr["description"].ToString() + "','" + dr["tooltip"].ToString() + "','" + dr["sproc"].ToString() + "','" + dr["window"].ToString() + "','" + dr["url"].ToString() + "','" + dr["target"].ToString() + "','" + dr["navigation"].ToString() + "','" + dr["enabled"].ToString() + "');";
                if (oParent == null)
                    oTree.Nodes.Add(oNode);
                else
                    oParent.ChildNodes.Add(oNode);
                LoadPages(null, oNode, Int32.Parse(dr["pageid"].ToString()));
            }
        }
        protected void btnAdd_Click(Object Sender, EventArgs e)
        {
            if (Request.Form[hdnId.UniqueID] == "0")
                oPage.Add(Int32.Parse(Request.Form[hdnParent.UniqueID]), txtTitle.Text, txtUrlTitle.Text, txtMenuTitle.Text, txtBrowser.Text, Int32.Parse(ddlTemplate.SelectedItem.Value), Int32.Parse(Request.Form[hdnRelated.UniqueID]), txtNav.Text, txtNavOver.Text, txtDescription.Text, txtToolTip.Text, txtSProc.Text, (chkWindow.Checked ? 1 : 0), txtUrl.Text, ddlTarget.SelectedItem.Value, (chkNavigation.Checked == true ? 1 : 0), (chkEnabled.Checked == true ? 1 : 0));
            else
                oPage.Update(Int32.Parse(Request.Form[hdnId.UniqueID]), Int32.Parse(Request.Form[hdnParent.UniqueID]), txtTitle.Text, txtUrlTitle.Text, txtMenuTitle.Text, txtBrowser.Text, Int32.Parse(ddlTemplate.SelectedItem.Value), Int32.Parse(Request.Form[hdnRelated.UniqueID]), txtNav.Text, txtNavOver.Text, txtDescription.Text, txtToolTip.Text, txtSProc.Text, (chkWindow.Checked ? 1 : 0), txtUrl.Text, ddlTarget.SelectedItem.Value, (chkNavigation.Checked == true ? 1 : 0), (chkEnabled.Checked == true ? 1 : 0));
            if (Request.Form[hdnOrder.UniqueID] != "")
            {
                string strOrder = Request.Form[hdnOrder.UniqueID];
                int intCount = 0;
                while (strOrder != "")
                {
                    intCount++;
                    int intPage = Int32.Parse(strOrder.Substring(0, strOrder.IndexOf("&")));
                    strOrder = strOrder.Substring(strOrder.IndexOf("&") + 1);
                    oPage.UpdateOrder(intPage, intCount);
                }
            }
            Response.Redirect(Request.Path);
        }
        protected void btnEnable_Click(Object Sender, ImageClickEventArgs e)
        {
            ImageButton oEnable = (ImageButton)Sender;
            oPage.Enable(Int32.Parse(oEnable.CommandArgument), (oEnable.ImageUrl == "images/enabled.gif" ? 0 : 1));
            Response.Redirect(Request.Path);
        }
        protected void btnDeleteLink_Click(Object Sender, EventArgs e)
        {
            ImageButton oDelete = (ImageButton)Sender;
            oPage.Delete(Int32.Parse(oDelete.CommandArgument));
            Response.Redirect(Request.Path);
        }
        protected void btnDelete_Click(Object Sender, EventArgs e)
        {
            oPage.Delete(Int32.Parse(Request.Form[hdnId.UniqueID]));
            Response.Redirect(Request.Path);
        }
    }
}
