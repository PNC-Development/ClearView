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
    public partial class frame_page_browser : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intProfile;
        protected Pages oPage;
        private TreeNode oSelected;
        private DataSet ds;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Cookies["adminid"] != null && Request.Cookies["adminid"].Value != "")
                intProfile = Int32.Parse(Request.Cookies["adminid"].Value);
            else
                Reload();
            oPage = new Pages(intProfile, dsn);
            if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
                lblId.Text = Request.QueryString["id"];
            string strControl = "";
            string strControlText = "";
            if (Request.QueryString["control"] != null)
                strControl = Request.QueryString["control"];
            if (Request.QueryString["controltext"] != null)
                strControlText = Request.QueryString["controltext"];
            btnNone.Attributes.Add("onclick", "return Reset(0,'" + hdnId.ClientID + "','No Parent Page','" + txtName.ClientID + "');");
            btnSave.Attributes.Add("onclick", "return Update('" + hdnId.ClientID + "','" + strControl + "','" + txtName.ClientID + "','" + strControlText + "');");
            btnClose.Attributes.Add("onclick", "return HidePanel();");
            LoadTree();
        }
        private void LoadTree()
        {
            PopulateTree(oTreeview, null, 0);
            oTreeview.CollapseAll();
            if (oSelected == null)
                txtName.Text = "No Parent Page";
            else
            {
                while (oSelected != null)
                {
                    oSelected.Expand();
                    oSelected = oSelected.Parent;
                }
            }
        }
        private void PopulateTree(TreeView oTree, TreeNode oParent, int _parent)
        {
            ds = oPage.Gets(_parent, 1);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                TreeNode oNode = new TreeNode();
                oNode.Text = dr["title"].ToString();
                oNode.ToolTip = dr["title"].ToString();
                oNode.NavigateUrl = "javascript:Select(" + dr["pageid"].ToString() + ",'" + hdnId.ClientID + "','" + dr["title"].ToString() + "','" + txtName.ClientID + "');";
                if (oParent == null)
                    oTree.Nodes.Add(oNode);
                else
                    oParent.ChildNodes.Add(oNode);
                if (dr["pageid"].ToString() == lblId.Text)
                {
                    oSelected = oNode;
                    txtName.Text = dr["title"].ToString();
                }
                PopulateTree(null, oNode, Int32.Parse(dr["pageid"].ToString()));
            }
        }
        private void Reload()
        {
            Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "reload", "<script type=\"text/javascript\">window.top.location.reload();<" + "/" + "script>");
        }
    }
}
