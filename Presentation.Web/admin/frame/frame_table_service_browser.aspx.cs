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
    public partial class frame_table_service_browser : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intProfile;
        protected Field oField;
        protected Services oService;
        protected int intTable;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Cookies["adminid"] != null && Request.Cookies["adminid"].Value != "")
                intProfile = Int32.Parse(Request.Cookies["adminid"].Value);
            else
                Reload();
            oField = new Field(intProfile, dsn);
            oService = new Services(intProfile, dsn);
            btnClose.Attributes.Add("onclick", "return HidePanel();");
            if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
                intTable = Int32.Parse(Request.QueryString["id"]);
            if (!IsPostBack)
            {
                LoadTree();
                if (intTable > 0)
                    lblName.Text = oField.GetTable(intTable, "tablename");
            }
        }
        private void LoadTree()
        {
            PopulateTree(0, oTree, null);
            oTree.CollapseAll();
            // Expand Nodes
            foreach (TreeNode oChild in oTree.Nodes)
            {
                if (Expand(oChild) == true)
                    oChild.Expand();
            }
        }
        private bool Expand(TreeNode oNode)
        {
            bool boolExpand = false;
            if (oNode.Checked == true)
                boolExpand = true;
            else
            {
                if (oNode.ChildNodes.Count > 0)
                {
                    foreach (TreeNode oChild in oNode.ChildNodes)
                    {
                        if (Expand(oChild) == true)
                        {
                            oChild.Expand();
                            boolExpand = true;
                        }
                    }
                }
                else
                    boolExpand = false;
            }
            return boolExpand;
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
            DataSet dsOther = oField.GetPermissions(intTable);
            DataSet ds = oService.Gets(_parent, 0, 0, 1, 0);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                TreeNode oNode = new TreeNode();
                oNode.Text = dr["name"].ToString();
                oNode.ToolTip = dr["name"].ToString();
                oNode.Value = dr["serviceid"].ToString();
                oNode.SelectAction = TreeNodeSelectAction.None;
                oNode.Checked = false;
                foreach (DataRow drOther in dsOther.Tables[0].Rows)
                {
                    if (dr["serviceid"].ToString() == drOther["serviceid"].ToString())
                        oNode.Checked = true;
                }
                oParent.ChildNodes.Add(oNode);
            }
            oTree.ExpandDepth = 1;
            oTree.Attributes.Add("oncontextmenu", "return false;");
        }
        protected void btnSave_Click(Object Sender, EventArgs e)
        {
            oField.DeletePermission(intTable);
            foreach (TreeNode oNode in oTree.Nodes)
                SaveTree(oNode);
            Reload();
        }
        private void SaveTree(TreeNode oParent)
        {
            foreach (TreeNode oNode in oParent.ChildNodes)
            {
                if (oNode.Checked == true)
                    oField.AddPermission2(Int32.Parse(oNode.Value), intTable);
                SaveTree(oNode);
            }
        }
        private void Reload()
        {
            Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "reload", "<script type=\"text/javascript\">window.top.location.reload();<" + "/" + "script>");
        }
    }
}
