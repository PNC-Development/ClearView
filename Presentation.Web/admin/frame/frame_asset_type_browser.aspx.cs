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
    public partial class frame_asset_type_browser : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intProfile;
        protected Types oType;
        protected Platforms oPlatform;
        private TreeNode oSelected;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Cookies["adminid"] != null && Request.Cookies["adminid"].Value != "")
                intProfile = Int32.Parse(Request.Cookies["adminid"].Value);
            else
                Reload();
            oType = new Types(intProfile, dsn);
            oPlatform = new Platforms(intProfile, dsn);
            if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
                lblId.Text = Request.QueryString["id"];
            string strControl = "";
            string strControlText = "";
            if (Request.QueryString["control"] != null)
                strControl = Request.QueryString["control"];
            if (Request.QueryString["controltext"] != null)
                strControlText = Request.QueryString["controltext"];
            btnNone.Attributes.Add("onclick", "return Reset(0,'" + hdnId.ClientID + "','None','" + txtName.ClientID + "');");
            btnSave.Attributes.Add("onclick", "return Update('" + hdnId.ClientID + "','" + strControl + "','" + txtName.ClientID + "','" + strControlText + "');");
            btnClose.Attributes.Add("onclick", "return HidePanel();");
            LoadTree();
        }
        private void LoadTree()
        {
            PopulateTree();
            oTreeview.CollapseAll();
            if (oSelected == null)
                txtName.Text = "None";
            else
            {
                while (oSelected != null)
                {
                    oSelected.Expand();
                    oSelected = oSelected.Parent;
                }
            }
        }
        private void PopulateTree()
        {
            DataSet ds = oPlatform.Gets(1);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                TreeNode oNode = new TreeNode();
                oNode.Text = dr["name"].ToString();
                oNode.ToolTip = dr["name"].ToString();
                oNode.ImageUrl = "/images/folder.gif";
                oNode.SelectAction = TreeNodeSelectAction.Expand;
                oTreeview.Nodes.Add(oNode);
                LoadTypes(Int32.Parse(dr["platformid"].ToString()), oNode);
            }
        }
        private void LoadTypes(int _platformid, TreeNode oParent)
        {
            DataSet ds = oType.Gets(_platformid, 0);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                TreeNode oNode = new TreeNode();
                oNode.Text = dr["name"].ToString();
                oNode.ToolTip = dr["name"].ToString();
                oNode.NavigateUrl = "javascript:Select(" + dr["id"].ToString() + ",'" + hdnId.ClientID + "','" + dr["name"].ToString() + "','" + txtName.ClientID + "');";
                oParent.ChildNodes.Add(oNode);
                if (dr["id"].ToString() == lblId.Text)
                {
                    oSelected = oNode;
                    txtName.Text = dr["name"].ToString();
                }
            }
            oTreeview.ExpandDepth = 2;
            oTreeview.Attributes.Add("oncontextmenu", "return false;");
        }
        private void Reload()
        {
            Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "reload", "<script type=\"text/javascript\">window.top.location.reload();<" + "/" + "script>");
        }
    }
}
