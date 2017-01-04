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
using Vim25Api;

namespace NCC.ClearView.Presentation.Web
{
    public partial class clusters : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected VMWare oVMWare;
        protected ModelsProperties oModelsProperties;
        protected Resiliency oResiliency;
        protected int intProfile;
        protected int intID = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cookies["loginreferrer"].Value = "/admin/vmware/clusters.aspx";
            Response.Cookies["loginreferrer"].Expires = DateTime.Now.AddDays(30);
            if (Request.Cookies["adminid"] != null && Request.Cookies["adminid"].Value != "")
                intProfile = Int32.Parse(Request.Cookies["adminid"].Value);
            else
                Response.Redirect("/admin/login.aspx");
            oVMWare = new VMWare(intProfile, dsn);
            oModelsProperties = new ModelsProperties(intProfile, dsn);
            oResiliency = new Resiliency(intProfile, dsn);
            if (Request.QueryString["id"] != null)
            {
                panAdd.Visible = true;
                intID = Int32.Parse(Request.QueryString["id"]);
                if (intID > 0 && !IsPostBack)
                {
                    DataSet ds = oVMWare.GetCluster(intID);
                    int intFolder = Int32.Parse(ds.Tables[0].Rows[0]["folderid"].ToString());
                    int intDatacenter = Int32.Parse(oVMWare.GetFolder(intFolder, "datacenterid"));
                    LoadFolders(intDatacenter);
                    ddlParent.SelectedValue = intFolder.ToString();
                    int intModel = Int32.Parse(ds.Tables[0].Rows[0]["modelid"].ToString());
                    hdnModel.Value = intModel.ToString();
                    txtName.Text = ds.Tables[0].Rows[0]["name"].ToString();
                    ddlVersion.SelectedValue = ds.Tables[0].Rows[0]["version"].ToString();
                    ddlAntiAffinity.SelectedValue = ds.Tables[0].Rows[0]["anti_affinity"].ToString();
                    txtMaximum.Text = ds.Tables[0].Rows[0]["maximum"].ToString();
                    txtResourcePool.Text = ds.Tables[0].Rows[0]["resource_pool"].ToString();
                    txtDatastoreNotify.Text = ds.Tables[0].Rows[0]["datastores_notify"].ToString();
                    txtDatastoreLeft.Text = ds.Tables[0].Rows[0]["datastores_left"].ToString();
                    txtDatastoreSize.Text = ds.Tables[0].Rows[0]["datastores_size"].ToString();
                    chkPNC.Checked = (ds.Tables[0].Rows[0]["pnc"].ToString() == "1");
                    chkFull.Checked = (ds.Tables[0].Rows[0]["at_max"].ToString() == "1");
                    chkAPoff.Checked = (ds.Tables[0].Rows[0]["auto_provision_off"].ToString() == "1");
                    chkAPoffDR.Checked = (ds.Tables[0].Rows[0]["auto_provision_dr_off"].ToString() == "1");
                    chkDell.Checked = (ds.Tables[0].Rows[0]["dell"].ToString() == "1");
                    ddlResiliency.SelectedValue = ds.Tables[0].Rows[0]["resiliencyid"].ToString();
                    chkOracle.Checked = (ds.Tables[0].Rows[0]["can_oracle"].ToString() == "1");
                    chkOracleCluster.Checked = (ds.Tables[0].Rows[0]["can_oracle_cluster"].ToString() == "1");
                    chkSQL.Checked = (ds.Tables[0].Rows[0]["can_sql"].ToString() == "1");
                    chkSQLCluster.Checked = (ds.Tables[0].Rows[0]["can_sql_cluster"].ToString() == "1");
                    chkCluster.Checked = (ds.Tables[0].Rows[0]["can_cluster"].ToString() == "1");
                    chkEnabled.Checked = (ds.Tables[0].Rows[0]["enabled"].ToString() == "1");
                    // Calculations
                    double dblA3 = 0.00;
                    double dblA4 = 0.00;
                    double dblA5 = 0.00;
                    double dblA8 = 0.00;
                    double dblA10 = 0.00;
                    double dblA20 = 0.00;
                    double dblA21 = 0.00;
                    double dblA22 = 0.00;
                    double dblA25 = 0.00;
                    double dblA26 = 0.00;
                    double dblA29 = 0.00;
                    double dblA30 = 0.00;
                    if (ds.Tables[0].Rows[0]["input_failures"].ToString() != "")
                        dblA3 = double.Parse(ds.Tables[0].Rows[0]["input_failures"].ToString());
                    txtFailures.Text = dblA3.ToString();
                    if (ds.Tables[0].Rows[0]["input_cpu_utilization"].ToString() != "")
                        dblA4 = double.Parse(ds.Tables[0].Rows[0]["input_cpu_utilization"].ToString());
                    txtCPUUtilization.Text = dblA4.ToString();
                    if (ds.Tables[0].Rows[0]["input_ram_utilization"].ToString() != "")
                        dblA5 = double.Parse(ds.Tables[0].Rows[0]["input_ram_utilization"].ToString());
                    txtRAMUtilization.Text = dblA5.ToString();
                    if (ds.Tables[0].Rows[0]["input_max_ram"].ToString() != "")
                        dblA8 = double.Parse(ds.Tables[0].Rows[0]["input_max_ram"].ToString());
                    txtMaxRAM.Text = dblA8.ToString();
                    if (ds.Tables[0].Rows[0]["input_avg_utilization"].ToString() != "")
                        dblA10 = double.Parse(ds.Tables[0].Rows[0]["input_avg_utilization"].ToString());
                    txtAvgUtilization.Text = dblA10.ToString();
                    if (ds.Tables[0].Rows[0]["input_lun_size"].ToString() != "")
                        dblA20 = double.Parse(ds.Tables[0].Rows[0]["input_lun_size"].ToString());
                    txtLunSize.Text = dblA20.ToString();
                    if (ds.Tables[0].Rows[0]["input_lun_utilization"].ToString() != "")
                        dblA21 = double.Parse(ds.Tables[0].Rows[0]["input_lun_utilization"].ToString());
                    txtLunUtilization.Text = dblA21.ToString();
                    if (ds.Tables[0].Rows[0]["input_vms_per_lun"].ToString() != "")
                        dblA22 = double.Parse(ds.Tables[0].Rows[0]["input_vms_per_lun"].ToString());
                    txtVMsPerLun.Text = dblA22.ToString();
                    if (ds.Tables[0].Rows[0]["input_time_lun"].ToString() != "")
                        dblA25 = double.Parse(ds.Tables[0].Rows[0]["input_time_lun"].ToString());
                    txtTimeLUN.Text = dblA25.ToString();
                    if (ds.Tables[0].Rows[0]["input_time_cluster"].ToString() != "")
                        dblA26 = double.Parse(ds.Tables[0].Rows[0]["input_time_cluster"].ToString());
                    txtTimeCluster.Text = dblA26.ToString();
                    if (ds.Tables[0].Rows[0]["input_max_vms_server"].ToString() != "")
                        dblA29 = double.Parse(ds.Tables[0].Rows[0]["input_max_vms_server"].ToString());
                    txtMaxVMsServer.Text = dblA29.ToString();
                    if (ds.Tables[0].Rows[0]["input_max_vms_lun"].ToString() != "")
                        dblA30 = double.Parse(ds.Tables[0].Rows[0]["input_max_vms_lun"].ToString());
                    txtMaxVMsLUN.Text = dblA30.ToString();
                    double dblA13 = 0.00;
                    double dblA14 = 0.00;
                    double dblA15 = 0.00;
                    double dblA16 = 0.00;
                    DataSet dsModel = oModelsProperties.Get(intModel);
                    if (dsModel.Tables[0].Rows.Count > 0)
                    {
                        lblModel.Text = dsModel.Tables[0].Rows[0]["name"].ToString();
                        dblA13 = double.Parse(dsModel.Tables[0].Rows[0]["ram"].ToString());
                        dblA14 = double.Parse(dsModel.Tables[0].Rows[0]["cpu_count"].ToString());
                        dblA15 = 1.00;
                        dblA16 = double.Parse(dsModel.Tables[0].Rows[0]["cpu_speed"].ToString());
                    }
                    txtRAM.Text = dblA13.ToString();
                    txtCores.Text = dblA14.ToString();
                    txtCPUs.Text = dblA15.ToString();
                    txtProcs.Text = dblA16.ToString();
                    double dblA17 = dblA14 * dblA15 * dblA16;   // =A14*A15*A16
                    txtTotal.Text = dblA17.ToString();

                    double dblA2 = 0.00;
                    if (Request.QueryString["query"] != null)
                    {
                        trCapacity.Visible = true;
                        chkQuery.Checked = true;
                        // Get # of hosts from cluster
                        int intVirtualCenter = Int32.Parse(oVMWare.GetDatacenter(intDatacenter, "virtualcenterid"));
                        string strVirtualCenter = oVMWare.GetVirtualCenter(intVirtualCenter, "name");
                        string strConnect = oVMWare.ConnectDEBUG(oVMWare.GetVirtualCenter(intVirtualCenter, "url"), Int32.Parse(oVMWare.GetVirtualCenter(intVirtualCenter, "environment")), oVMWare.GetDatacenter(intDatacenter, "name"));
                        if (strConnect == "")
                        {
                            VimService _service = oVMWare.GetService();
                            ServiceContent _sic = oVMWare.GetSic();
                            ManagedObjectReference[] hostRefs = oVMWare.GetHosts(txtName.Text);
                            dblA2 = double.Parse(hostRefs.Length.ToString());
                            if (_service != null)
                            {
                                _service.Abort();
                                if (_service.Container != null)
                                    _service.Container.Dispose();
                                try
                                {
                                    _service.Logout(_sic.sessionManager);
                                }
                                catch { }
                                _service.Dispose();
                                _service = null;
                                _sic = null;
                            }
                        }
                        else
                            dblA2 = -1.00;
                    }
                    else
                        dblA2 = -1.00;
                    txtServers.Text = dblA2.ToString();
                    // Maximum CPU allocated
                    double dblA9 = dblA13 * dblA8;
                    dblA9 = dblA17 / dblA9;
                    txtMaxCPU.Text = dblA9.ToString();          // =A17/A13*A8
                    // Total CPU & RAM
                    double dblD2 = dblA2 * dblA17;
                    txtTotalCPU.Text = dblD2.ToString();        // =A2*A17
                    double dblD3 = dblA2 * dblA13;
                    txtTotalRAM.Text = dblD3.ToString();        // =A2*A13
                    // Reserves
                    double dblD7 = dblA29 * dblA26 * dblA9;
                    txtReserveCPU.Text = dblD7.ToString();      // =A29*A26*A9
                    double dblD8 = dblA29 * dblA26 * dblA8;
                    txtReserveRAM.Text = dblD8.ToString();      // =A29*A26*A8
                    double dblD9 = dblA10 + dblA8;
                    dblD9 = dblA30 * dblA25 * dblD9;
                    txtReserveDisk.Text = dblD9.ToString();     // =A30*A25*(A10+A8)
                    // Expansion
                    double dblD12 = dblA3 * 17.00;
                    dblD12 = dblD2 - dblD7 - dblD12;
                    dblD12 = dblD12 / dblD2;
                    dblD12 = dblD12 * dblA4;
                    dblD12 = Math.Floor(dblD12);
                    txtExpandCPU.Text = dblD12.ToString();      // =INT((D2-D7-(A3*17))/D2*A4)
                    double dblD13 = dblA3 * 13.00;
                    dblD13 = dblD3 - dblD8 - dblD13;
                    dblD13 = dblD13 / dblD3;
                    dblD13 = dblD13 * dblA5;
                    dblD13 = Math.Floor(dblD13);
                    txtExpandRAM.Text = dblD13.ToString();      // =INT((D3-D8-(A3*13))/D3*A5)
                    double dblD14 = dblA20 * dblA21;
                    dblD14 = dblD14 / 100.00;
                    dblD14 = dblD14 - dblD9;
                    dblD14 = dblD14 / dblA20;
                    dblD14 = dblD14 * 100.00;
                    dblD14 = Math.Floor(dblD14);
                    txtExpandDisk.Text = dblD14.ToString();      // =INT(((A20*A21/100)-D9)/A20*100)


                    btnDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this item?');");
                    btnModel.Attributes.Add("onclick", "return OpenWindow('SUBMODELBROWSER','" + hdnModel.ClientID + "','&control=" + hdnModel.ClientID + "&controltext=" + lblModel.ClientID + "',false,400,600);");
                    btnOS.Attributes.Add("onclick", "return OpenWindow('VMWARE_OS','','" + intID.ToString() + "',false,'650',500);");
                }
                else if (intID == 0)
                {
                    trCapacity.Visible = true;
                    chkQuery.Checked = true;
                    chkQuery.Enabled = false;
                    btnOS.Enabled = false;
                }
            }
            else if (Request.QueryString["folderid"] != null)
            {
                int intFolder = Int32.Parse(Request.QueryString["folderid"]);
                int intDatacenter = Int32.Parse(oVMWare.GetFolder(intFolder, "datacenterid"));
                LoadFolders(intDatacenter);
                ddlParent.SelectedValue = intFolder.ToString();
                panAdd.Visible = true;

                trCapacity.Visible = false;
                chkQuery.Checked = true;
                chkQuery.Enabled = false;
                btnOS.Enabled = false;
            }
            else
            {
                LoadVirtualCenters();
            }
        }
        protected void chkQuery_Change(Object Sender, EventArgs e)
        {
            Response.Redirect(Request.Path + "?id=" + Request.QueryString["id"] + (chkQuery.Checked ? "&query=true" : ""));
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
        private void LoadFolders(int _parent)
        {
            DataSet ds = oVMWare.GetFolders(_parent, 0);
            ddlParent.DataTextField = "name";
            ddlParent.DataValueField = "id";
            ddlParent.DataSource = ds;
            ddlParent.DataBind();
            ddlParent.Items.Insert(0, new ListItem("-- NONE --", "0"));
            ddlResiliency.DataValueField = "id";
            ddlResiliency.DataTextField = "name";
            ddlResiliency.DataSource = oResiliency.Gets(1);
            ddlResiliency.DataBind();
            ddlResiliency.Items.Insert(0, new ListItem("-- SELECT --", "0"));
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
                TreeNode oNew = new TreeNode();
                oNew.Text = "&nbsp;Add Cluster";
                oNew.ToolTip = "Add Cluster";
                oNew.ImageUrl = "/images/green_right.gif";
                oNew.NavigateUrl = Request.Path + "?folderid=" + dr["id"].ToString();
                oNode.ChildNodes.Add(oNew);
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
                if (dr["at_max"].ToString() != "1")
                    oNode.ImageUrl = "/images/check.gif";
                if (dr["enabled"].ToString() != "1")
                    oNode.ImageUrl = "/images/cancel.gif";
                oNode.SelectAction = TreeNodeSelectAction.Expand;
                oNode.NavigateUrl = Request.Path + "?id=" + dr["id"].ToString();
                oParent.ChildNodes.Add(oNode);
            }
        }
        protected void btnAdd_Click(Object Sender, EventArgs e)
        {
            int intModel = 0;
            if (Request.Form[hdnModel.UniqueID] != "")
                intModel = Int32.Parse(Request.Form[hdnModel.UniqueID]);
            int intMaximum = 0;
            if (Int32.TryParse(txtMaximum.Text, out intMaximum) == false)
                intMaximum = 99999;
            int intDatastoreLeft = 0;
            Int32.TryParse(txtDatastoreLeft.Text, out intDatastoreLeft);
            int intDatastoreSize = 0;
            Int32.TryParse(txtDatastoreSize.Text, out intDatastoreSize);
            if (intID == 0)
                oVMWare.AddCluster(Int32.Parse(ddlParent.SelectedItem.Value), intModel, txtName.Text, Int32.Parse(ddlVersion.SelectedItem.Value), Int32.Parse(ddlAntiAffinity.SelectedItem.Value), intMaximum, txtResourcePool.Text, txtDatastoreNotify.Text, intDatastoreLeft, intDatastoreSize, (chkPNC.Checked ? 1 : 0), (chkFull.Checked ? 1 : 0), (chkAPoff.Checked ? 1 : 0), (chkAPoffDR.Checked ? 1 : 0), (chkDell.Checked ? 1 : 0), Int32.Parse(ddlResiliency.SelectedItem.Value), (chkOracle.Checked ? 1 : 0), (chkOracleCluster.Checked ? 1 : 0), (chkSQL.Checked ? 1 : 0), (chkSQLCluster.Checked ? 1 : 0), (chkCluster.Checked ? 1 : 0), 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, (chkEnabled.Checked ? 1 : 0));
            else
                oVMWare.UpdateCluster(intID, Int32.Parse(ddlParent.SelectedItem.Value), intModel, txtName.Text, Int32.Parse(ddlVersion.SelectedItem.Value), Int32.Parse(ddlAntiAffinity.SelectedItem.Value), intMaximum, txtResourcePool.Text, txtDatastoreNotify.Text, intDatastoreLeft, intDatastoreSize, (chkPNC.Checked ? 1 : 0), (chkFull.Checked ? 1 : 0), (chkAPoff.Checked ? 1 : 0), (chkAPoffDR.Checked ? 1 : 0), (chkDell.Checked ? 1 : 0), Int32.Parse(ddlResiliency.SelectedItem.Value), (chkOracle.Checked ? 1 : 0), (chkOracleCluster.Checked ? 1 : 0), (chkSQL.Checked ? 1 : 0), (chkSQLCluster.Checked ? 1 : 0), (chkCluster.Checked ? 1 : 0), double.Parse(txtFailures.Text), double.Parse(txtCPUUtilization.Text), double.Parse(txtRAMUtilization.Text), double.Parse(txtMaxRAM.Text), double.Parse(txtAvgUtilization.Text), double.Parse(txtLunSize.Text), double.Parse(txtLunUtilization.Text), double.Parse(txtVMsPerLun.Text), double.Parse(txtTimeLUN.Text), double.Parse(txtTimeCluster.Text), double.Parse(txtMaxVMsServer.Text), double.Parse(txtMaxVMsLUN.Text), (chkEnabled.Checked ? 1 : 0));
            Response.Redirect(Request.Path);
        }
        protected void btnDelete_Click(Object Sender, EventArgs e)
        {
            oVMWare.DeleteCluster(intID);
            Response.Redirect(Request.Path);
        }
        protected void btnCancel_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(Request.Path);
        }
    }
}
