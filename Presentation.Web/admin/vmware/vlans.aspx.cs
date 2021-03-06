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
    public partial class vlans : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected VMWare oVMWare;
        protected int intProfile;
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cookies["loginreferrer"].Value = "/admin/vmware/vlans.aspx";
            Response.Cookies["loginreferrer"].Expires = DateTime.Now.AddDays(30);
            if (Request.Cookies["adminid"] != null && Request.Cookies["adminid"].Value != "")
                intProfile = Int32.Parse(Request.Cookies["adminid"].Value);
            else
                Response.Redirect("/admin/login.aspx");
            oVMWare = new VMWare(intProfile, dsn);
            if (!IsPostBack)
            {
                LoadVirtualCenters();
                btnDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this item?');");
                btnCancel.Attributes.Add("onclick", "return Cancel();");
                btnVlan.Attributes.Add("onclick", "return OpenWindow('VMWARE_VLANS','" + hdnId.ClientID + "','',false,'500',500);");
            }
        }
        private void LoadVirtualCenters()
        {
            DataSet ds = oVMWare.GetVirtualCenters(1);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                TreeNode oNode = new TreeNode();
                oNode.Text = dr["name"].ToString();
                oNode.ToolTip = dr["name"].ToString();
                oNode.ImageUrl = "/images/folder.gif";
                oNode.SelectAction = TreeNodeSelectAction.Expand;
                oTreeview.Nodes.Add(oNode);
                LoadDatacenter(Int32.Parse(dr["id"].ToString()), oNode);
            }
            oTreeview.ExpandDepth = 3;
            oTreeview.Attributes.Add("oncontextmenu", "return false;");
        }
        private void LoadDatacenter(int _parent, TreeNode oParent)
        {
            DataSet ds = oVMWare.GetDatacenters(_parent, 1);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                TreeNode oNode = new TreeNode();
                oNode.Text = dr["name"].ToString();
                oNode.ToolTip = dr["name"].ToString();
                oNode.ImageUrl = "/images/folder.gif";
                oNode.SelectAction = TreeNodeSelectAction.Expand;
                oParent.ChildNodes.Add(oNode);
                LoadFolder(Int32.Parse(dr["id"].ToString()), oNode);
            }
        }
        private void LoadFolder(int _parent, TreeNode oParent)
        {
            DataSet ds = oVMWare.GetFolders(_parent, 0);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                TreeNode oNode = new TreeNode();
                oNode.Text = dr["name"].ToString();
                oNode.ToolTip = dr["name"].ToString();
                oNode.ImageUrl = "/images/folder.gif";
                oNode.SelectAction = TreeNodeSelectAction.Expand;
                oParent.ChildNodes.Add(oNode);
                LoadCluster(Int32.Parse(dr["id"].ToString()), oNode);
            }
        }
        private void LoadCluster(int _parent, TreeNode oParent)
        {
            DataSet ds = oVMWare.GetClusters(_parent, 0);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                ddlParent.Items.Insert(0, new ListItem(dr["name"].ToString(), dr["id"].ToString()));
                TreeNode oNode = new TreeNode();
                oNode.Text = dr["name"].ToString();
                oNode.ToolTip = dr["name"].ToString();
                oNode.ImageUrl = "/images/folder.gif";
                oNode.SelectAction = TreeNodeSelectAction.Expand;
                oParent.ChildNodes.Add(oNode);
                LoadVlan(Int32.Parse(dr["id"].ToString()), oNode);
                TreeNode oNew = new TreeNode();
                oNew.Text = "&nbsp;Add Vlan";
                oNew.ToolTip = "Add Vlan";
                oNew.ImageUrl = "/images/green_right.gif";
                oNew.NavigateUrl = "javascript:Add('" + dr["id"].ToString() + "');";
                oNode.ChildNodes.Add(oNew);
            }
        }
        private void LoadVlan(int _parent, TreeNode oParent)
        {
            DataSet ds = oVMWare.GetVlans(_parent, 0);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                TreeNode oNode = new TreeNode();
                oNode.Text = dr["name"].ToString();
                oNode.ToolTip = dr["name"].ToString();
                oNode.SelectAction = TreeNodeSelectAction.Expand;
                oNode.NavigateUrl = "javascript:Edit('" + dr["id"].ToString() + "','" + dr["name"].ToString() + "','" + dr["clusterid"].ToString() + "','" + dr["enabled"].ToString() + "');";
                oParent.ChildNodes.Add(oNode);
            }
        }
        protected void btnAdd_Click(Object Sender, EventArgs e)
        {
            if (Request.Form[hdnId.UniqueID] == "0")
                oVMWare.AddVlan(Int32.Parse(ddlParent.SelectedItem.Value), txtName.Text, (chkEnabled.Checked ? 1 : 0));
            else
                oVMWare.UpdateVlan(Int32.Parse(Request.Form[hdnId.UniqueID]), Int32.Parse(ddlParent.SelectedItem.Value), txtName.Text, (chkEnabled.Checked ? 1 : 0));
            Response.Redirect(Request.Path);
        }
        protected void btnDelete_Click(Object Sender, EventArgs e)
        {
            oVMWare.DeleteVlan(Int32.Parse(Request.Form[hdnId.UniqueID]));
            Response.Redirect(Request.Path);
        }
    }
}
