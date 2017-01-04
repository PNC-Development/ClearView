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
    public partial class asset_models_other : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;

        protected int intPlatformS = Int32.Parse(ConfigurationManager.AppSettings["ServerPlatformID"]);
        protected int intPlatformN = Int32.Parse(ConfigurationManager.AppSettings["NetworkPlatformID"]);
        protected Platforms oPlatform;
        protected Types oType;
        protected Models oModel;
        protected ModelsProperties oModelsProperties;
        protected AssetCategory oAssetCategory;
        protected Dells oDell;
        protected int intProfile;
        private bool boolAvailable = false;
        private bool boolEnabled = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cookies["loginreferrer"].Value = "/admin/asset/asset_models_other.aspx";
            Response.Cookies["loginreferrer"].Expires = DateTime.Now.AddDays(30);
            if (Request.Cookies["adminid"] != null && Request.Cookies["adminid"].Value != "")
                intProfile = Int32.Parse(Request.Cookies["adminid"].Value);
            else
                Response.Redirect("/admin/login.aspx");
            oPlatform = new Platforms(intProfile, dsn);
            oType = new Types(intProfile, dsn);
            oModel = new Models(intProfile, dsn);
            oModelsProperties = new ModelsProperties(intProfile, dsn);
            oAssetCategory = new AssetCategory(intProfile, dsn);
            oDell = new Dells(intProfile, dsn);

            if (!IsPostBack)
            {
                LoadList();
                LoadPlatforms();
                btnParent.Attributes.Add("onclick", "return OpenWindow('MODELBROWSER','" + hdnParent.ClientID + "','&control=" + hdnParent.ClientID + "&controltext=" + lblParent.ClientID + "',false,400,600);");
                btnDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this item?');");
                btnCancel.Attributes.Add("onclick", "return Cancel();");
            }
        }
        private void LoadList()
        {
            ddlAssetCategory.DataTextField = "AssetCategoryName";
            ddlAssetCategory.DataValueField = "id";
            ddlAssetCategory.DataSource = oAssetCategory.Gets(1);
            ddlAssetCategory.DataBind();
            ddlAssetCategory.Items.Insert(0, new ListItem("-- Select --", "0"));
            ddlChipset.DataTextField = "name";
            ddlChipset.DataValueField = "id";
            ddlChipset.DataSource = oModel.GetChipsets(1);
            ddlChipset.DataBind();
            ddlChipset.Items.Insert(0, new ListItem("-- Select --", "0"));
            ddlDell.DataTextField = "name";
            ddlDell.DataValueField = "id";
            ddlDell.DataSource = oDell.Gets(1);
            ddlDell.DataBind();
            ddlDell.Items.Insert(0, new ListItem("-- Select --", "0"));
        }
        private void LoadPlatforms()
        {
            DataSet ds = oPlatform.Gets(1);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (Int32.Parse(dr["platformid"].ToString()) != intPlatformN && Int32.Parse(dr["platformid"].ToString()) != intPlatformS)
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
        }
        private void LoadTypes(int _platformid, TreeNode oParent)
        {
            DataSet ds = oType.Gets(_platformid, 1);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                TreeNode oNode = new TreeNode();
                oNode.Text = dr["name"].ToString();
                oNode.ToolTip = dr["name"].ToString();
                oNode.ImageUrl = "/images/folder.gif";
                oNode.SelectAction = TreeNodeSelectAction.Expand;
                oParent.ChildNodes.Add(oNode);
                LoadModels(Int32.Parse(dr["id"].ToString()), oNode);
            }
        }
        private void LoadModels(int _typeid, TreeNode oParent)
        {
            DataSet ds = oModel.Gets(_typeid, 1);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                TreeNode oNode = new TreeNode();
                oNode.Text = dr["name"].ToString();
                oNode.ToolTip = dr["name"].ToString();
                oNode.SelectAction = TreeNodeSelectAction.Expand;
                oParent.ChildNodes.Add(oNode);
                LoadModelServers(Int32.Parse(dr["id"].ToString()), oNode);
                if (boolAvailable == true)
                    oNode.ImageUrl = "/images/check.gif";
                else if (boolEnabled == false)
                    oNode.ImageUrl = "/images/cancel.gif";
                TreeNode oNew = new TreeNode();
                oNew.Text = "&nbsp;Add Model";
                oNew.ToolTip = "Add Model";
                oNew.ImageUrl = "/images/green_right.gif";
                oNew.NavigateUrl = "javascript:Add('" + dr["id"].ToString() + "','" + oModel.Get(Int32.Parse(dr["id"].ToString()), "name") + "');";
                oNode.ChildNodes.Add(oNew);
            }
        }
        private void LoadModelServers(int _modelid, TreeNode oParent)
        {
            boolAvailable = false;
            boolEnabled = true;
            DataSet ds = oModelsProperties.GetModels(0, _modelid, 0);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                TreeNode oNode = new TreeNode();
                if (dr["available"].ToString() == "1")
                {
                    if (boolAvailable == false)
                        boolAvailable = true;
                    oNode.ImageUrl = "/images/check.gif";
                }
                if (dr["enabled"].ToString() != "1")
                {
                    if (boolEnabled == true)
                        boolEnabled = false;
                    oNode.ImageUrl = "/images/cancel.gif";
                }
                string strName = dr["name"].ToString();
                if (strName == "")
                    strName = oModel.Get(_modelid, "name");
                oNode.Text = strName;
                oNode.ToolTip = strName;
                oNode.SelectAction = TreeNodeSelectAction.Expand;
                oNode.NavigateUrl = "javascript:Edit('" + dr["id"].ToString() + "','" + dr["available"].ToString() + "','" + dr["replicate_times"].ToString() + "','" + dr["amp"].ToString() + "','" + dr["network_ports"].ToString() + "','" + dr["storage_ports"].ToString() + "','" + dr["name"].ToString() + "','" + dr["modelid"].ToString() + "','" + oModel.Get(Int32.Parse(dr["modelid"].ToString()), "name") + "','" + dr["ram"].ToString() + "','" + dr["cpu_count"].ToString() + "','" + dr["cpu_speed"].ToString() + "','" + dr["chipset"].ToString() + "','" + dr["StorageThresholdMin"].ToString() + "','" + dr["StorageThresholdMax"].ToString() + "','" + dr["asset_category"].ToString() + "','" + dr["high_availability"].ToString() + "','" + dr["high_performance"].ToString() + "','" + dr["low_performance"].ToString() + "','" + dr["enforce_1_1_recovery"].ToString() + "','" + dr["no_many_1_recovery"].ToString() + "','" + dr["vmware_virtual"].ToString() + "','" + dr["ibm_virtual"].ToString() + "','" + dr["sun_virtual"].ToString() + "','" + dr["storage_db_boot_local"].ToString() + "','" + dr["storage_db_boot_san_windows"].ToString() + "','" + dr["storage_db_boot_san_unix"].ToString() + "','" + dr["storage_de_fdrive_can_local"].ToString() + "','" + dr["storage_de_fdrive_must_san"].ToString() + "','" + dr["storage_de_fdrive_only"].ToString() + "','" + dr["manual_build"].ToString() + "','" + dr["type_blade"].ToString() + "','" + dr["type_physical"].ToString() + "','" + dr["type_vmware"].ToString() + "','" + dr["type_enclosure"].ToString() + "','" + dr["config_service_pack"].ToString() + "','" + dr["config_vmware_template"].ToString() + "','" + dr["config_maintenance_level"].ToString() + "','" + dr["vio"].ToString() + "','" + dr["fabric"].ToString() + "','" + dr["storage_type"].ToString() + "','" + dr["inventory"].ToString() + "','" + dr["dell"].ToString() + "','" + dr["dellconfigid"].ToString() + "','" + dr["configure_switches"].ToString() + "','" + dr["enter_name"].ToString() + "','" + dr["enter_ip"].ToString() + "','" + dr["associate_project"].ToString() + "','" + dr["full_height"].ToString() + "','" + dr["enabled"].ToString() + "');";
                oParent.ChildNodes.Add(oNode);
            }
            oTreeview.ExpandDepth = 0;
            oTreeview.Attributes.Add("oncontextmenu", "return false;");
        }
        protected void btnAdd_Click(Object Sender, EventArgs e)
        {
            int intParent = Int32.Parse(Request.Form[hdnParent.UniqueID]);

            if (txtStorageThreholdMin.Text == "")
                txtStorageThreholdMin.Text = "0";
            if (txtStorageThreholdMax.Text == "")
                txtStorageThreholdMax.Text = "0";

            if (Request.Form[hdnId.UniqueID] == "0")
                oModelsProperties.AddOther(intParent, (chkAvailable.Checked ? 1 : 0), Int32.Parse(txtReplicate.Text), double.Parse(txtAmp.Text), Int32.Parse(txtNetworkPorts.Text), Int32.Parse(txtStoragePorts.Text), txtName.Text, Int32.Parse(txtRam.Text), Int32.Parse(txtCount.Text), double.Parse(txtSpeed.Text), Int32.Parse(ddlChipset.SelectedItem.Value), double.Parse(txtStorageThreholdMin.Text), double.Parse(txtStorageThreholdMax.Text), Int32.Parse(ddlAssetCategory.SelectedItem.Value), (chk_high_availability.Checked ? 1 : 0), (chk_high_performance.Checked ? 1 : 0), (chk_low_performance.Checked ? 1 : 0), (chk_enforce_1_1_recovery.Checked ? 1 : 0), (chk_no_many_1_recovery.Checked ? 1 : 0), (chk_vmware_virtual.Checked ? 1 : 0), (chk_ibm_virtual.Checked ? 1 : 0), (chk_sun_virtual.Checked ? 1 : 0), (chk_storage_db_boot_local.Checked ? 1 : 0), (chk_storage_db_boot_san_windows.Checked ? 1 : 0), (chk_storage_db_boot_san_unix.Checked ? 1 : 0), (chk_storage_de_fdrive_can_local.Checked ? 1 : 0), (chk_storage_de_fdrive_must_san.Checked ? 1 : 0), (chk_storage_de_fdrive_only.Checked ? 1 : 0), (chkManualBuild.Checked ? 1 : 0), (chk_type_blade.Checked ? 1 : 0), (chk_type_physical.Checked ? 1 : 0), (chk_type_vmware.Checked ? 1 : 0), (chk_type_enclosure.Checked ? 1 : 0), (chk_config_service_pack.Checked ? 1 : 0), (chk_config_vmware_template.Checked ? 1 : 0), (chk_config_maintenance_level.Checked ? 1 : 0), (chkVio.Checked ? 1 : 0), Int32.Parse(ddlStorageFabric.SelectedItem.Value), Int32.Parse(ddlStorageType.SelectedItem.Value), (chk_inventory.Checked ? 1 : 0), (chkDell.Checked ? 1 : 0), Int32.Parse(ddlDell.SelectedItem.Value), (chkConfigureSwitches.Checked ? 1 : 0), (chkEnterName.Checked ? 1 : 0), (chkEnterIP.Checked ? 1 : 0), (chkAssociateProject.Checked ? 1 : 0), (chkFullHeight.Checked ? 1 : 0), (chkEnabled.Checked ? 1 : 0));
            else
                oModelsProperties.UpdateOther(Int32.Parse(Request.Form[hdnId.UniqueID]), intParent, (chkAvailable.Checked ? 1 : 0), Int32.Parse(txtReplicate.Text), double.Parse(txtAmp.Text), Int32.Parse(txtNetworkPorts.Text), Int32.Parse(txtStoragePorts.Text), txtName.Text, Int32.Parse(txtRam.Text), Int32.Parse(txtCount.Text), double.Parse(txtSpeed.Text), Int32.Parse(ddlChipset.SelectedItem.Value), double.Parse(txtStorageThreholdMin.Text), double.Parse(txtStorageThreholdMax.Text), Int32.Parse(ddlAssetCategory.SelectedItem.Value), (chk_high_availability.Checked ? 1 : 0), (chk_high_performance.Checked ? 1 : 0), (chk_low_performance.Checked ? 1 : 0), (chk_enforce_1_1_recovery.Checked ? 1 : 0), (chk_no_many_1_recovery.Checked ? 1 : 0), (chk_vmware_virtual.Checked ? 1 : 0), (chk_ibm_virtual.Checked ? 1 : 0), (chk_sun_virtual.Checked ? 1 : 0), (chk_storage_db_boot_local.Checked ? 1 : 0), (chk_storage_db_boot_san_windows.Checked ? 1 : 0), (chk_storage_db_boot_san_unix.Checked ? 1 : 0), (chk_storage_de_fdrive_can_local.Checked ? 1 : 0), (chk_storage_de_fdrive_must_san.Checked ? 1 : 0), (chk_storage_de_fdrive_only.Checked ? 1 : 0), (chkManualBuild.Checked ? 1 : 0), (chk_type_blade.Checked ? 1 : 0), (chk_type_physical.Checked ? 1 : 0), (chk_type_vmware.Checked ? 1 : 0), (chk_type_enclosure.Checked ? 1 : 0), (chk_config_service_pack.Checked ? 1 : 0), (chk_config_vmware_template.Checked ? 1 : 0), (chk_config_maintenance_level.Checked ? 1 : 0), (chkVio.Checked ? 1 : 0), Int32.Parse(ddlStorageFabric.SelectedItem.Value), Int32.Parse(ddlStorageType.SelectedItem.Value), (chk_inventory.Checked ? 1 : 0), (chkDell.Checked ? 1 : 0), Int32.Parse(ddlDell.SelectedItem.Value), (chkConfigureSwitches.Checked ? 1 : 0), (chkEnterName.Checked ? 1 : 0), (chkEnterIP.Checked ? 1 : 0), (chkAssociateProject.Checked ? 1 : 0), (chkFullHeight.Checked ? 1 : 0), (chkEnabled.Checked ? 1 : 0));
            Response.Redirect(Request.Path);
        }
        protected void btnDelete_Click(Object Sender, EventArgs e)
        {
            oModelsProperties.Delete(Int32.Parse(Request.Form[hdnId.UniqueID]));
            Response.Redirect(Request.Path);
        }
    }
}
