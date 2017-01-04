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
    public partial class datastores : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected VMWare oVMWare;
        protected OperatingSystems oOperatingSystems;
        protected int intProfile;
        protected int intID = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cookies["loginreferrer"].Value = "/admin/vmware/datastores.aspx";
            Response.Cookies["loginreferrer"].Expires = DateTime.Now.AddDays(30);
            if (Request.Cookies["adminid"] != null && Request.Cookies["adminid"].Value != "")
                intProfile = Int32.Parse(Request.Cookies["adminid"].Value);
            else
                Response.Redirect("/admin/login.aspx");
            oVMWare = new VMWare(intProfile, dsn);
            oOperatingSystems = new OperatingSystems(intProfile, dsn);

            if (!IsPostBack)
                LoadLists();
            if (Request.QueryString["id"] == null)
            {
                if (Request.QueryString["add"] == null)
                    LoadVirtualCenters();
                else
                {
                    int intClusterID = Int32.Parse(Request.QueryString["add"]);
                    LoadClusters(intClusterID);
                    LoadPartners(intClusterID);
                    ddlParent.SelectedValue = intClusterID.ToString();
                    txtMaximum.Text = "99999";
                    panAdd.Visible = true;
                    btnDelete.Enabled = false;
                }
            }
            else
            {
                panAdd.Visible = true;
                intID = Int32.Parse(Request.QueryString["id"]);
                if (intID > 0 && !IsPostBack)
                {
                    DataSet ds = oVMWare.GetDatastore(intID);
                    hdnId.Value = intID.ToString();
                    txtName.Text = ds.Tables[0].Rows[0]["name"].ToString();
                    int intClusterID = Int32.Parse(ds.Tables[0].Rows[0]["clusterid"].ToString());
                    LoadClusters(intClusterID);
                    LoadPartners(intClusterID);
                    ddlParent.SelectedValue = intClusterID.ToString();
                    ddlType.SelectedValue = ds.Tables[0].Rows[0]["storage_type"].ToString();
                    ddlOperatingSystemGroup.SelectedValue = ds.Tables[0].Rows[0]["osgroupid"].ToString();
                    chkReplicated.Checked = (ds.Tables[0].Rows[0]["replicated"].ToString() == "1");
                    txtMaximum.Text = ds.Tables[0].Rows[0]["maximum"].ToString();
                    chkServer.Checked = (ds.Tables[0].Rows[0]["server"].ToString() == "1");
                    chkPagefile.Checked = (ds.Tables[0].Rows[0]["pagefile"].ToString() == "1");
                    chkOverridePermission.Checked = (ds.Tables[0].Rows[0]["override_permission"].ToString() == "1");
                    ddlPartner.SelectedValue = ds.Tables[0].Rows[0]["partner"].ToString();
                    chkEnabled.Checked = (ds.Tables[0].Rows[0]["enabled"].ToString() == "1");
                    btnAdd.Text = "Update";
                }
            }
            btnDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this item?');");
            btnCancel.Attributes.Add("onclick", "return Cancel();");
        }
        private void LoadLists()
        {
            ddlOperatingSystemGroup.DataTextField = "name";
            ddlOperatingSystemGroup.DataValueField = "id";
            ddlOperatingSystemGroup.DataSource = oOperatingSystems.GetGroups(1);
            ddlOperatingSystemGroup.DataBind();
            ddlOperatingSystemGroup.Items.Insert(0, new ListItem("-- ALL OS's --", "0"));
        }
        private void LoadClusters(int _clusterid)
        {
            int intFolderID = Int32.Parse(oVMWare.GetCluster(_clusterid, "folderid"));
            ddlParent.DataTextField = "name";
            ddlParent.DataValueField = "id";
            ddlParent.DataSource = oVMWare.GetClusters(intFolderID, 1);
            ddlParent.DataBind();
            ddlParent.Items.Insert(0, new ListItem("-- SELECT --", "0"));
        }
        private void LoadPartners(int _clusterid)
        {
            ddlPartner.DataTextField = "name";
            ddlPartner.DataValueField = "id";
            ddlPartner.DataSource = oVMWare.GetDatastores(_clusterid, 1);
            ddlPartner.DataBind();
            ddlPartner.Items.Insert(0, new ListItem("-- NONE --", "0"));
        }
        private void LoadVirtualCenters()
        {
            panView.Visible = true;
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
                TreeNode oNode = new TreeNode();
                oNode.Text = dr["name"].ToString();
                oNode.ToolTip = dr["name"].ToString();
                oNode.ImageUrl = "/images/folder.gif";
                oNode.SelectAction = TreeNodeSelectAction.Expand;
                oParent.ChildNodes.Add(oNode);
                LoadDatastore(Int32.Parse(dr["id"].ToString()), oNode);
                TreeNode oNew = new TreeNode();
                oNew.Text = "&nbsp;Add Datastore";
                oNew.ToolTip = "Add Datastore";
                oNew.ImageUrl = "/images/green_right.gif";
                oNew.NavigateUrl = Request.Path + "?add=" + dr["id"].ToString();
                oNode.ChildNodes.Add(oNew);
            }
        }
        private void LoadDatastore(int _parent, TreeNode oParent)
        {
            DataSet ds = oVMWare.GetDatastores(_parent, 0);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                TreeNode oNode = new TreeNode();
                oNode.Text = dr["name"].ToString();
                oNode.ToolTip = dr["name"].ToString();
                if (dr["enabled"].ToString() == "1")
                    oNode.ImageUrl = "/images/check.gif";
                else
                    oNode.ImageUrl = "/images/cancel.gif";
                oNode.SelectAction = TreeNodeSelectAction.Expand;
                oNode.NavigateUrl = Request.Path + "?id=" + dr["id"].ToString();
                oParent.ChildNodes.Add(oNode);
            }
        }
        protected void btnAdd_Click(Object Sender, EventArgs e)
        {
            int intMaximum = 0;
            if (Int32.TryParse(txtMaximum.Text, out intMaximum) == false)
                intMaximum = 99999;
            if (intID == 0)
                oVMWare.AddDatastore(Int32.Parse(ddlParent.SelectedItem.Value), txtName.Text, Int32.Parse(ddlType.SelectedItem.Value), Int32.Parse(ddlOperatingSystemGroup.SelectedItem.Value), (chkReplicated.Checked ? 1 : 0), intMaximum, (chkServer.Checked ? 1 : 0), (chkPagefile.Checked ? 1 : 0), (chkOverridePermission.Checked ? 1 : 0), Int32.Parse(ddlPartner.SelectedItem.Value), (chkEnabled.Checked ? 1 : 0));
            else
                oVMWare.UpdateDatastore(Int32.Parse(Request.Form[hdnId.UniqueID]), Int32.Parse(ddlParent.SelectedItem.Value), txtName.Text, Int32.Parse(ddlType.SelectedItem.Value), Int32.Parse(ddlOperatingSystemGroup.SelectedItem.Value), (chkReplicated.Checked ? 1 : 0), intMaximum, (chkServer.Checked ? 1 : 0), (chkPagefile.Checked ? 1 : 0), (chkOverridePermission.Checked ? 1 : 0), Int32.Parse(ddlPartner.SelectedItem.Value), (chkEnabled.Checked ? 1 : 0));
            Response.Redirect(Request.Path);
        }
        protected void btnCancel_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(Request.Path);
        }
        protected void btnDelete_Click(Object Sender, EventArgs e)
        {
            oVMWare.DeleteDatastore(Int32.Parse(Request.Form[hdnId.UniqueID]));
            Response.Redirect(Request.Path);
        }
    }
}
