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
    public partial class frame_networks_related : BasePage
    {
        private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intProfile;
        protected IPAddresses oIPAddresses;
        protected Environments oEnvironment;
        protected Classes oClass;
        protected int intNetwork;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Cookies["adminid"] != null && Request.Cookies["adminid"].Value != "")
                intProfile = Int32.Parse(Request.Cookies["adminid"].Value);
            else
                Reload();
            oIPAddresses = new IPAddresses(intProfile, dsnIP, dsn);
            oEnvironment = new Environments(intProfile, dsn);
            oClass = new Classes(intProfile, dsn);
            btnClose.Attributes.Add("onclick", "return HidePanel();");
            if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
                intNetwork = Int32.Parse(Request.QueryString["id"]);
            if (!IsPostBack)
            {
                LoadClasses();
                if (intNetwork > 0)
                    lblName.Text = oIPAddresses.GetNetworkName(intNetwork);
                // Expand Nodes
                foreach (TreeNode oChild in oTree.Nodes)
                {
                    if (Expand(oChild) == true)
                        oChild.Expand();
                }
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
        private void LoadClasses()
        {
            DataSet ds = oClass.Gets(1);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                TreeNode oNode = new TreeNode();
                oNode.Text = dr["name"].ToString();
                oNode.ToolTip = dr["name"].ToString();
                oNode.ImageUrl = "/images/folder.gif";
                oNode.SelectAction = TreeNodeSelectAction.Expand;
                oTree.Nodes.Add(oNode);
                LoadEnvironments(Int32.Parse(dr["id"].ToString()), oNode);
            }
            oTree.ExpandDepth = 0;
            oTree.Attributes.Add("oncontextmenu", "return false;");
        }
        private void LoadEnvironments(int _parent, TreeNode oParent)
        {
            DataSet ds = oClass.GetEnvironment(_parent, 0);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                TreeNode oNode = new TreeNode();
                oNode.Text = dr["name"].ToString();
                oNode.ToolTip = dr["name"].ToString();
                oNode.ImageUrl = "/images/folder.gif";
                oNode.SelectAction = TreeNodeSelectAction.Expand;
                oParent.ChildNodes.Add(oNode);
                LoadAddress(_parent, Int32.Parse(dr["id"].ToString()), oNode);
            }
        }
        private void LoadAddress(int _classid, int _environmentid, TreeNode oParent)
        {
            DataSet ds = oIPAddresses.GetVlansAddress(_classid, _environmentid, 0);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                TreeNode oNode = new TreeNode();
                oNode.Text = dr["name"].ToString();
                oNode.ToolTip = dr["name"].ToString();
                oNode.ImageUrl = "/images/folder.gif";
                oNode.SelectAction = TreeNodeSelectAction.Expand;
                oParent.ChildNodes.Add(oNode);
                LoadVlans(_classid, _environmentid, Int32.Parse(dr["id"].ToString()), oNode);
            }
        }
        private void LoadVlans(int _classid, int _environmentid, int _addressid, TreeNode oParent)
        {
            DataSet ds = oIPAddresses.GetVlansAddress(_classid, _environmentid, _addressid, 0);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                TreeNode oNode = new TreeNode();
                oNode.Text = dr["vlan"].ToString();
                oNode.ToolTip = dr["vlan"].ToString();
                oNode.ImageUrl = "/images/folder.gif";
                oNode.SelectAction = TreeNodeSelectAction.Expand;
                oParent.ChildNodes.Add(oNode);
                Load(Int32.Parse(dr["id"].ToString()), oNode);
            }
        }
        private void Load(int _parent, TreeNode oParent)
        {
            DataSet dsOther = oIPAddresses.GetNetworkRelations(intNetwork);
            DataSet ds = oIPAddresses.GetNetworks(_parent, 0);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                TreeNode oNode = new TreeNode();
                oNode.Text = oIPAddresses.GetNetworkName(Int32.Parse(dr["id"].ToString()));
                oNode.Value = dr["id"].ToString();
                oNode.ToolTip = oIPAddresses.GetNetworkName(Int32.Parse(dr["id"].ToString()));
                oNode.SelectAction = TreeNodeSelectAction.None;
                oNode.Checked = false;
                foreach (DataRow drOther in dsOther.Tables[0].Rows)
                {
                    if (dr["id"].ToString() == drOther["id"].ToString())
                        oNode.Checked = true;
                }
                oParent.ChildNodes.Add(oNode);
            }
        }
        protected void btnSave_Click(Object Sender, EventArgs e)
        {
            oIPAddresses.DeleteNetworkRelation(intNetwork);
            foreach (TreeNode oNode in oTree.Nodes)
                SaveTree(oNode);
            Reload();
        }
        private void SaveTree(TreeNode oParent)
        {
            foreach (TreeNode oNode in oParent.ChildNodes)
            {
                if (oNode.Checked == true)
                    oIPAddresses.AddNetworkRelation(intNetwork, Int32.Parse(oNode.Value));
                SaveTree(oNode);
            }
        }
        private void Reload()
        {
            Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "reload", "<script type=\"text/javascript\">window.top.location.reload();<" + "/" + "script>");
        }
    }
}
