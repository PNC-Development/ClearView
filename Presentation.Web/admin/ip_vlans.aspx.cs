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
    public partial class ip_vlans : BasePage
    {
        private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intLocation = Int32.Parse(ConfigurationManager.AppSettings["OPSLocationID"]);
        protected IPAddresses oIPAddresses;
        protected Environments oEnvironment;
        protected Classes oClass;
        protected Locations oLocation;
        protected Resiliency oResiliency;
        protected int intProfile;
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cookies["loginreferrer"].Value = "/admin/ip_vlans.aspx";
            Response.Cookies["loginreferrer"].Expires = DateTime.Now.AddDays(30);
            if (Request.Cookies["adminid"] != null && Request.Cookies["adminid"].Value != "")
                intProfile = Int32.Parse(Request.Cookies["adminid"].Value);
            else
                Response.Redirect("/admin/login.aspx");
            oIPAddresses = new IPAddresses(intProfile, dsnIP, dsn);
            oEnvironment = new Environments(intProfile, dsn);
            oClass = new Classes(intProfile, dsn);
            oLocation = new Locations(intProfile, dsn);
            oResiliency = new Resiliency(intProfile, dsn);
            if (!IsPostBack)
            {
                LoadLists();
                LoadClasses();
                oTreeview.ExpandDepth = 0;
                btnParent.Attributes.Add("onclick", "return OpenWindow('LOCATION_BROWSER','" + hdnParent.ClientID + "','&control=" + hdnParent.ClientID + "&controltext=" + lblParent.ClientID + "',false,400,600);");
                btnDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this item?');");
                btnCancel.Attributes.Add("onclick", "return Cancel();");
                ddlClass.Attributes.Add("onchange", "PopulateEnvironments('" + ddlClass.ClientID + "','" + ddlEnvironment.ClientID + "',0);");
                ddlEnvironment.Attributes.Add("onchange", "UpdateDropDownHidden('" + ddlEnvironment.ClientID + "','" + hdnEnvironment.ClientID + "');");
            }
        }
        private void LoadLists()
        {
            ddlClass.DataTextField = "name";
            ddlClass.DataValueField = "id";
            ddlClass.DataSource = oClass.Gets(1);
            ddlClass.DataBind();
            ddlClass.Items.Insert(0, new ListItem("-- SELECT --", "0"));
            ddlResiliency.DataValueField = "id";
            ddlResiliency.DataTextField = "name";
            ddlResiliency.DataSource = oResiliency.Gets(1);
            ddlResiliency.DataBind();
            ddlResiliency.Items.Insert(0, new ListItem("-- SELECT --", "0"));
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
                oTreeview.Nodes.Add(oNode);
                LoadEnvironments(Int32.Parse(dr["id"].ToString()), oNode);
            }
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
                oNode.SelectAction = TreeNodeSelectAction.Expand;
                oNode.NavigateUrl = "javascript:Edit('" + dr["id"].ToString() + "','" + dr["vlan"].ToString() + "','" + dr["physical_windows"].ToString() + "','" + dr["physical_unix"].ToString() + "','" + dr["ecom_production"].ToString() + "','" + dr["ecom_service"].ToString() + "','" + dr["ipx"].ToString() + "','" + dr["virtual_workstation"].ToString() + "','" + dr["vmware_workstation_external"].ToString() + "','" + dr["vmware_workstation_internal"].ToString() + "','" + dr["vmware_workstation_dr"].ToString() + "','" + dr["vmware_host"].ToString() + "','" + dr["vmware_vmotion"].ToString() + "','" + dr["vmware_windows"].ToString() + "','" + dr["vmware_linux"].ToString() + "','" + dr["blades_hp"].ToString() + "','" + dr["blades_sun"].ToString() + "','" + dr["apv"].ToString() + "','" + dr["mainframe"].ToString() + "','" + dr["csm"].ToString() + "','" + dr["csm_soa"].ToString() + "','" + dr["replicates"].ToString() + "','" + dr["pxe"].ToString() + "','" + dr["ilo"].ToString() + "','" + dr["csm_vip"].ToString() + "','" + dr["ltm_web"].ToString() + "','" + dr["ltm_app"].ToString() + "','" + dr["ltm_middle"].ToString() + "','" + dr["ltm_vip"].ToString() + "','" + dr["windows_cluster"].ToString() + "','" + dr["unix_cluster"].ToString() + "','" + dr["accenture"].ToString() + "','" + dr["ha"].ToString() + "','" + dr["sun_cluster"].ToString() + "','" + dr["sun_sve"].ToString() + "','" + dr["storage"].ToString() + "','" + dr["dell_web"].ToString() + "','" + dr["dell_middleware"].ToString() + "','" + dr["dell_database"].ToString() + "','" + dr["dell_file"].ToString() + "','" + dr["dell_misc"].ToString() + "','" + dr["dell_under48"].ToString() + "','" + dr["dell_avamar"].ToString() + "','" + dr["switchname"].ToString() + "','" + dr["classid"].ToString() + "','" + dr["environmentid"].ToString() + "','" + dr["addressid"].ToString() + "','" + oLocation.GetFull(Int32.Parse(dr["addressid"].ToString())) + "','" + dr["resiliencyid"].ToString() + "','" + dr["enabled"].ToString() + "');";
                oParent.ChildNodes.Add(oNode);
            }
        }
        protected void btnAdd_Click(Object Sender, EventArgs e)
        {
            if (Request.Form[hdnId.UniqueID] == "0")
                oIPAddresses.AddVlan(Int32.Parse(txtVlan.Text), (chkPhysicalW.Checked ? 1 : 0), (chkPhysicalU.Checked ? 1 : 0), (chkEcomProd.Checked ? 1 : 0), (chkEcomService.Checked ? 1 : 0), (chkIPX.Checked ? 1 : 0), (chkVirtual.Checked ? 1 : 0), (chkVMworkstationExternal.Checked ? 1 : 0), (chkVMworkstationInternal.Checked ? 1 : 0), (chkVMworkstationDR.Checked ? 1 : 0), (chkVMHost.Checked ? 1 : 0), (chkVMotion.Checked ? 1 : 0), (chkVMWindows.Checked ? 1 : 0), (chkVMLinux.Checked ? 1 : 0), (chkBladesHP.Checked ? 1 : 0), (chkBladesSUN.Checked ? 1 : 0), (chkAPV.Checked ? 1 : 0), (chkMainframe.Checked ? 1 : 0), (chkCSM.Checked ? 1 : 0), (chkCSMSOA.Checked ? 1 : 0), (chkReplicates.Checked ? 1 : 0), (chkPXE.Checked ? 1 : 0), (chkILO.Checked ? 1 : 0), (chkCSMVIP.Checked ? 1 : 0), (chkLTMWeb.Checked ? 1 : 0), (chkLTMApp.Checked ? 1 : 0), (chkLTMMiddle.Checked ? 1 : 0), (chkLTMVIP.Checked ? 1 : 0), (chkWindowsCluster.Checked ? 1 : 0), (chkUnixCluster.Checked ? 1 : 0), (chkAccenture.Checked ? 1 : 0), (chkHA.Checked ? 1 : 0), (chkSunCluster.Checked ? 1 : 0), (chkSunSVE.Checked ? 1 : 0), (chkStorage.Checked ? 1 : 0), (chkDellWeb.Checked ? 1 : 0), (chkDellMiddleware.Checked ? 1 : 0), (chkDellDatabase.Checked ? 1 : 0), (chkDellFile.Checked ? 1 : 0), (chkDellMisc.Checked ? 1 : 0), (chkDellUnder48.Checked ? 1 : 0), (chkDellAvamar.Checked ? 1 : 0), txtName.Text, "", Int32.Parse(ddlClass.SelectedItem.Value), Int32.Parse(Request.Form[hdnEnvironment.UniqueID]), Int32.Parse(Request.Form[hdnParent.UniqueID]), Int32.Parse(ddlResiliency.SelectedItem.Value), (chkEnabled.Checked ? 1 : 0));
            else
                oIPAddresses.UpdateVlan(Int32.Parse(Request.Form[hdnId.UniqueID]), Int32.Parse(txtVlan.Text), (chkPhysicalW.Checked ? 1 : 0), (chkPhysicalU.Checked ? 1 : 0), (chkEcomProd.Checked ? 1 : 0), (chkEcomService.Checked ? 1 : 0), (chkIPX.Checked ? 1 : 0), (chkVirtual.Checked ? 1 : 0), (chkVMworkstationExternal.Checked ? 1 : 0), (chkVMworkstationInternal.Checked ? 1 : 0), (chkVMworkstationDR.Checked ? 1 : 0), (chkVMHost.Checked ? 1 : 0), (chkVMotion.Checked ? 1 : 0), (chkVMWindows.Checked ? 1 : 0), (chkVMLinux.Checked ? 1 : 0), (chkBladesHP.Checked ? 1 : 0), (chkBladesSUN.Checked ? 1 : 0), (chkAPV.Checked ? 1 : 0), (chkMainframe.Checked ? 1 : 0), (chkCSM.Checked ? 1 : 0), (chkCSMSOA.Checked ? 1 : 0), (chkReplicates.Checked ? 1 : 0), (chkPXE.Checked ? 1 : 0), (chkILO.Checked ? 1 : 0), (chkCSMVIP.Checked ? 1 : 0), (chkLTMWeb.Checked ? 1 : 0), (chkLTMApp.Checked ? 1 : 0), (chkLTMMiddle.Checked ? 1 : 0), (chkLTMVIP.Checked ? 1 : 0), (chkWindowsCluster.Checked ? 1 : 0), (chkUnixCluster.Checked ? 1 : 0), (chkAccenture.Checked ? 1 : 0), (chkHA.Checked ? 1 : 0), (chkSunCluster.Checked ? 1 : 0), (chkSunSVE.Checked ? 1 : 0), (chkStorage.Checked ? 1 : 0), (chkDellWeb.Checked ? 1 : 0), (chkDellMiddleware.Checked ? 1 : 0), (chkDellDatabase.Checked ? 1 : 0), (chkDellFile.Checked ? 1 : 0), (chkDellMisc.Checked ? 1 : 0), (chkDellUnder48.Checked ? 1 : 0), (chkDellAvamar.Checked ? 1 : 0), txtName.Text, "", Int32.Parse(ddlClass.SelectedItem.Value), Int32.Parse(Request.Form[hdnEnvironment.UniqueID]), Int32.Parse(Request.Form[hdnParent.UniqueID]), Int32.Parse(ddlResiliency.SelectedItem.Value), (chkEnabled.Checked ? 1 : 0));
            Response.Redirect(Request.Path);
        }
        protected void btnDelete_Click(Object Sender, EventArgs e)
        {
            oIPAddresses.DeleteVlan(Int32.Parse(Request.Form[hdnId.UniqueID]));
            Response.Redirect(Request.Path);
        }
    }
}
