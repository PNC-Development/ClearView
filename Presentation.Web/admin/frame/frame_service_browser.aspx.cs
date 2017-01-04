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
    public partial class frame_service_browser : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intProfile;
        protected Services oService;
        private TreeNode oSelected;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Cookies["adminid"] != null && Request.Cookies["adminid"].Value != "")
                intProfile = Int32.Parse(Request.Cookies["adminid"].Value);
            else
                Reload();
            oService = new Services(intProfile, dsn);
            if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
                lblId.Text = Request.QueryString["id"];
            string strControl = "";
            string strControlText = "";
            if (Request.QueryString["control"] != null)
                strControl = Request.QueryString["control"];
            if (Request.QueryString["controltext"] != null)
                strControlText = Request.QueryString["controltext"];
            btnNone.Attributes.Add("onclick", "return Reset(0,'" + hdnId.ClientID + "','No Item','" + txtName.ClientID + "');");
            btnSave.Attributes.Add("onclick", "return Update('" + hdnId.ClientID + "','" + strControl + "','" + txtName.ClientID + "','" + strControlText + "');");
            btnClose.Attributes.Add("onclick", "return HidePanel();");
            LoadTree();
        }
        private void LoadTree()
        {
            PopulateTree(0, oTreeview, null);
            oTreeview.CollapseAll();
            if (oSelected == null)
                txtName.Text = "No Parent";
            else
            {
                while (oSelected != null)
                {
                    oSelected.Expand();
                    oSelected = oSelected.Parent;
                }
            }
        }
        private void PopulateTree(int _parent, TreeView oTree, TreeNode oParent)
        {
            DataSet ds = oService.GetFolders(_parent, 0);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                TreeNode oNode = new TreeNode();
                oNode.Text = dr["name"].ToString();
                oNode.ToolTip = dr["name"].ToString();
                oNode.SelectAction = TreeNodeSelectAction.Expand;
                if (oParent == null)
                    oTree.Nodes.Add(oNode);
                else
                    oParent.ChildNodes.Add(oNode);
                PopulateTree(Int32.Parse(dr["id"].ToString()), oTree, oNode);
                LoadServices(Int32.Parse(dr["id"].ToString()), oNode);
            }
        }
        private void LoadServices(int _parent, TreeNode oParent)
        {
            DataSet ds = oService.Gets(_parent, 0, 0, 1, 0);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                TreeNode oNode = new TreeNode();
                oNode.Text = dr["name"].ToString();
                oNode.ToolTip = dr["name"].ToString();
                oNode.NavigateUrl = "javascript:Select(" + dr["serviceid"].ToString() + ",'" + hdnId.ClientID + "','" + dr["name"].ToString() + "','" + txtName.ClientID + "');";
                oParent.ChildNodes.Add(oNode);
                if (dr["serviceid"].ToString() == lblId.Text)
                {
                    oSelected = oNode;
                    txtName.Text = dr["name"].ToString();
                }
            }
            oTreeview.ExpandDepth = 1;
            oTreeview.Attributes.Add("oncontextmenu", "return false;");
        }
        private void Reload()
        {
            Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "reload", "<script type=\"text/javascript\">window.top.location.reload();<" + "/" + "script>");
        }
    }
}
