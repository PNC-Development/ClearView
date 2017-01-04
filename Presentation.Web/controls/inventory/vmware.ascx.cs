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
using System.Text;
using NCC.ClearView.Presentation.Web.Custom;
namespace NCC.ClearView.Presentation.Web
{
    public partial class vmware : System.Web.UI.UserControl
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intProfile;
        protected int intApplication = 0;
        protected int intPage = 0;
        protected string strMenuTab1 = "";

        protected Pages oPage;
        protected VMWare oVMWare;
        protected Resiliency oResiliency;
        protected OperatingSystems oOperatingSystems;

        protected void Page_Load(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            if (Request.QueryString["applicationid"] != null && Request.QueryString["applicationid"] != "")
                intApplication = Int32.Parse(Request.QueryString["applicationid"]);
            if (Request.QueryString["pageid"] != null && Request.QueryString["pageid"] != "")
                intPage = Int32.Parse(Request.QueryString["pageid"]);
            if (Request.Cookies["application"] != null && Request.Cookies["application"].Value != "")
                intApplication = Int32.Parse(Request.Cookies["application"].Value);

            oPage = new Pages(intProfile, dsn);
            oVMWare = new VMWare(intProfile, dsn);
            oResiliency = new Resiliency(intProfile, dsn);
            oOperatingSystems = new OperatingSystems(intProfile, dsn);

            if (!IsPostBack)
            {
                if (String.IsNullOrEmpty(Request.QueryString["v"]) == false)
                {
                    int v = Int32.Parse(Request.QueryString["v"]);
                    DataSet rec = oVMWare.GetVlan(v);
                    if (rec.Tables[0].Rows.Count > 0)
                    {
                        lblVParent.ToolTip = rec.Tables[0].Rows[0]["clusterid"].ToString();
                        lblVParent.Text = oVMWare.GetCluster(Int32.Parse(lblVParent.ToolTip), "name");
                        txtVName.Text = rec.Tables[0].Rows[0]["name"].ToString();
                        chkVEnabled.Checked = (rec.Tables[0].Rows[0]["enabled"].ToString() == "1");
                        btnUpdateV.Text = "Update";
                    }
                    else if (String.IsNullOrEmpty(Request.QueryString["c"]) == false)
                    {
                        lblVParent.ToolTip = Request.QueryString["c"];
                        lblVParent.Text = oVMWare.GetCluster(Int32.Parse(lblVParent.ToolTip), "name");
                    }
                    panVLAN.Visible = true;
                }
                else if (String.IsNullOrEmpty(Request.QueryString["ds"]) == false)
                {
                    ddlOperatingSystemGroup.DataTextField = "name";
                    ddlOperatingSystemGroup.DataValueField = "id";
                    ddlOperatingSystemGroup.DataSource = oOperatingSystems.GetGroups(1);
                    ddlOperatingSystemGroup.DataBind();
                    ddlOperatingSystemGroup.Items.Insert(0, new ListItem("-- ALL OS's --", "0"));

                    ddlPartner.DataTextField = "name";
                    ddlPartner.DataValueField = "id";

                    int c = 0;
                    if (String.IsNullOrEmpty(Request.QueryString["c"]) == false)
                    {
                        Int32.TryParse(Request.QueryString["c"], out c);
                        ddlPartner.DataSource = oVMWare.GetDatastores(c, 1);
                        ddlPartner.DataBind();
                        ddlPartner.Items.Insert(0, new ListItem("-- NONE --", "0"));
                    }

                    int ds = Int32.Parse(Request.QueryString["ds"]);
                    DataSet rec = oVMWare.GetDatastore(ds);
                    if (rec.Tables[0].Rows.Count > 0)
                    {
                        c = Int32.Parse(rec.Tables[0].Rows[0]["clusterid"].ToString());
                        ddlPartner.DataSource = oVMWare.GetDatastores(c, 1);
                        ddlPartner.DataBind();
                        ddlPartner.Items.Insert(0, new ListItem("-- NONE --", "0"));

                        txtDSName.Text = rec.Tables[0].Rows[0]["name"].ToString();
                        ddlType.SelectedValue = rec.Tables[0].Rows[0]["storage_type"].ToString();
                        ddlOperatingSystemGroup.SelectedValue = rec.Tables[0].Rows[0]["osgroupid"].ToString();
                        chkReplicated.Checked = (rec.Tables[0].Rows[0]["replicated"].ToString() == "1");
                        txtDSMaximum.Text = rec.Tables[0].Rows[0]["maximum"].ToString();
                        chkServer.Checked = (rec.Tables[0].Rows[0]["server"].ToString() == "1");
                        chkPagefile.Checked = (rec.Tables[0].Rows[0]["pagefile"].ToString() == "1");
                        chkOverridePermission.Checked = (rec.Tables[0].Rows[0]["override_permission"].ToString() == "1");
                        ddlPartner.SelectedValue = rec.Tables[0].Rows[0]["partner"].ToString();
                        chkDSEnabled.Checked = (rec.Tables[0].Rows[0]["enabled"].ToString() == "1");
                        btnUpdateDS.Text = "Update";
                    }

                    lblDSParent.ToolTip = c.ToString();
                    lblDSParent.Text = oVMWare.GetCluster(c, "name");
                    panDatastore.Visible = true;
                }
                else if (String.IsNullOrEmpty(Request.QueryString["h"]) == false)
                {
                    int h = Int32.Parse(Request.QueryString["h"]);
                    DataSet rec = oVMWare.GetHost(h);
                    if (rec.Tables[0].Rows.Count > 0)
                    {
                        lblHParent.ToolTip = rec.Tables[0].Rows[0]["clusterid"].ToString();
                        lblHParent.Text = oVMWare.GetCluster(Int32.Parse(lblHParent.ToolTip), "name");
                        txtHName.Text = rec.Tables[0].Rows[0]["name"].ToString();
                        txtHMaximum.Text = rec.Tables[0].Rows[0]["maximum"].ToString();
                        chkHEnabled.Checked = (rec.Tables[0].Rows[0]["enabled"].ToString() == "1");
                        btnUpdateH.Text = "Update";
                    }
                    else if (String.IsNullOrEmpty(Request.QueryString["c"]) == false)
                    {
                        lblHParent.ToolTip = Request.QueryString["c"];
                        lblHParent.Text = oVMWare.GetCluster(Int32.Parse(lblHParent.ToolTip), "name");
                    }
                    panHost.Visible = true;
                }
                else if (String.IsNullOrEmpty(Request.QueryString["c"]) == false)
                {
                    ddlResiliency.DataTextField = "name";
                    ddlResiliency.DataValueField = "id";
                    ddlResiliency.DataSource = oResiliency.Gets(1);
                    ddlResiliency.DataBind();

                    int c = Int32.Parse(Request.QueryString["c"]);
                    DataSet rec = oVMWare.GetCluster(c);
                    if (rec.Tables[0].Rows.Count > 0)
                    {
                        lblCParent.ToolTip = rec.Tables[0].Rows[0]["folderid"].ToString();
                        lblCParent.Text = oVMWare.GetFolder(Int32.Parse(lblCParent.ToolTip), "name");
                        txtCName.Text = rec.Tables[0].Rows[0]["name"].ToString();
                        ddlVersion.SelectedValue = rec.Tables[0].Rows[0]["version"].ToString();
                        ddlAntiAffinity.SelectedValue = rec.Tables[0].Rows[0]["anti_affinity"].ToString();
                        txtCMaximum.Text = rec.Tables[0].Rows[0]["maximum"].ToString();
                        txtResourcePool.Text = rec.Tables[0].Rows[0]["resource_pool"].ToString();
                        txtDatastoreNotify.Text = rec.Tables[0].Rows[0]["datastores_notify"].ToString();
                        txtDatastoreLeft.Text = rec.Tables[0].Rows[0]["datastores_left"].ToString();
                        txtDatastoreSize.Text = rec.Tables[0].Rows[0]["datastores_size"].ToString();
                        chkFull.Checked = (rec.Tables[0].Rows[0]["at_max"].ToString() == "1");
                        chkAPoff.Checked = (rec.Tables[0].Rows[0]["auto_provision_off"].ToString() == "1");
                        chkAPoffDR.Checked = (rec.Tables[0].Rows[0]["auto_provision_dr_off"].ToString() == "1");
                        chkDell.Checked = (rec.Tables[0].Rows[0]["dell"].ToString() == "1");
                        ddlResiliency.SelectedValue = rec.Tables[0].Rows[0]["resiliencyid"].ToString();
                        chkOracle.Checked = (rec.Tables[0].Rows[0]["can_oracle"].ToString() == "1");
                        chkOracleCluster.Checked = (rec.Tables[0].Rows[0]["can_oracle_cluster"].ToString() == "1");
                        chkSQL.Checked = (rec.Tables[0].Rows[0]["can_sql"].ToString() == "1");
                        chkSQLCluster.Checked = (rec.Tables[0].Rows[0]["can_sql_cluster"].ToString() == "1");
                        chkCluster.Checked = (rec.Tables[0].Rows[0]["can_cluster"].ToString() == "1");
                        chkCEnabled.Checked = (rec.Tables[0].Rows[0]["enabled"].ToString() == "1");

                        txtFailures.Text = rec.Tables[0].Rows[0]["input_failures"].ToString();
                        txtCPUUtilization.Text = rec.Tables[0].Rows[0]["input_cpu_utilization"].ToString();
                        txtRAMUtilization.Text = rec.Tables[0].Rows[0]["input_ram_utilization"].ToString();
                        txtMaxRAM.Text = rec.Tables[0].Rows[0]["input_max_ram"].ToString();
                        txtAvgUtilization.Text = rec.Tables[0].Rows[0]["input_avg_utilization"].ToString();
                        txtLunSize.Text = rec.Tables[0].Rows[0]["input_lun_size"].ToString();
                        txtLunUtilization.Text = rec.Tables[0].Rows[0]["input_lun_utilization"].ToString();
                        txtVMsPerLun.Text = rec.Tables[0].Rows[0]["input_vms_per_lun"].ToString();
                        txtTimeLUN.Text = rec.Tables[0].Rows[0]["input_time_lun"].ToString();
                        txtTimeCluster.Text = rec.Tables[0].Rows[0]["input_time_cluster"].ToString();
                        txtMaxVMsServer.Text = rec.Tables[0].Rows[0]["input_max_vms_server"].ToString();
                        txtMaxVMsLUN.Text = rec.Tables[0].Rows[0]["input_max_vms_lun"].ToString();

                        btnUpdateC.Text = "Update";

                        rptHosts.DataSource = oVMWare.GetHosts(c, 0);
                        rptHosts.DataBind();
                        rptDatastores.DataSource = oVMWare.GetDatastores(c, 0);
                        rptDatastores.DataBind();
                        rptVLANs.DataSource = oVMWare.GetVlans(c, 0);
                        rptVLANs.DataBind();

                        int intMenuTab = 0;
                        if (Request.QueryString["menu_tab"] != null && Request.QueryString["menu_tab"] != "")
                            intMenuTab = Int32.Parse(Request.QueryString["menu_tab"]);
                        Tab oTab = new Tab(hdnTab.ClientID, intMenuTab, "divMenu1", true, false);
                        oTab.AddTab("Hosts", "");
                        oTab.AddTab("Datastores", "");
                        oTab.AddTab("VLANs", "");
                        strMenuTab1 = oTab.GetTabs();
                    }
                    else if (String.IsNullOrEmpty(Request.QueryString["f"]) == false)
                    {
                        lblCParent.ToolTip = Request.QueryString["f"];
                        lblCParent.Text = oVMWare.GetFolder(Int32.Parse(lblCParent.ToolTip), "name");
                    }
                    panCluster.Visible = true;
                }
                else if (String.IsNullOrEmpty(Request.QueryString["f"]) == false)
                {
                    int f = Int32.Parse(Request.QueryString["f"]);
                    DataSet rec = oVMWare.GetFolder(f);
                    if (rec.Tables[0].Rows.Count > 0)
                    {
                        lblFParent.ToolTip = rec.Tables[0].Rows[0]["datacenterid"].ToString();
                        lblFParent.Text = oVMWare.GetDatacenter(Int32.Parse(lblFParent.ToolTip), "name");
                        txtFName.Text = rec.Tables[0].Rows[0]["name"].ToString();
                        txtFNotification.Text = rec.Tables[0].Rows[0]["notification"].ToString();
                        chkFEnabled.Checked = (rec.Tables[0].Rows[0]["enabled"].ToString() == "1");
                        btnUpdateF.Text = "Update";

                        rptClusters.DataSource = oVMWare.GetClusters(f, 0);
                        rptClusters.DataBind();
                    }
                    else if (String.IsNullOrEmpty(Request.QueryString["dc"]) == false)
                    {
                        lblFParent.ToolTip = Request.QueryString["dc"];
                        lblFParent.Text = oVMWare.GetDatacenter(Int32.Parse(lblFParent.ToolTip), "name");
                    }
                    panFolder.Visible = true;
                }
                else if (String.IsNullOrEmpty(Request.QueryString["dc"]) == false)
                {
                    ddlDCParent.DataTextField = "name";
                    ddlDCParent.DataValueField = "id";
                    ddlDCParent.DataSource = oVMWare.GetVirtualCenters(1);
                    ddlDCParent.DataBind();

                    int dc = Int32.Parse(Request.QueryString["dc"]);
                    DataSet rec = oVMWare.GetDatacenter(dc);
                    if (rec.Tables[0].Rows.Count > 0)
                    {
                        ddlDCParent.SelectedValue = rec.Tables[0].Rows[0]["virtualcenterid"].ToString();
                        txtDCName.Text = rec.Tables[0].Rows[0]["name"].ToString();
                        txtDCBuildFolder.Text = rec.Tables[0].Rows[0]["build_folder"].ToString();
                        txtDCDesktopNetwork.Text = rec.Tables[0].Rows[0]["desktop_network"].ToString();
                        txtDCWorkstationDecomVLAN.Text = rec.Tables[0].Rows[0]["workstation_decom_vlan"].ToString();
                        chkDCEnabled.Checked = (rec.Tables[0].Rows[0]["enabled"].ToString() == "1");
                        btnUpdateDC.Text = "Update";

                        rptFolders.DataSource = oVMWare.GetFolders(dc, 0);
                        rptFolders.DataBind();
                    }
                    else if (String.IsNullOrEmpty(Request.QueryString["vc"]) == false)
                    {
                        ddlDCParent.SelectedValue = Request.QueryString["vc"];
                    }
                    panDC.Visible = true;
                }
                else if (String.IsNullOrEmpty(Request.QueryString["vc"]) == false)
                {
                    int vc = Int32.Parse(Request.QueryString["vc"]);
                    DataSet rec = oVMWare.GetVirtualCenter(vc);
                    if (rec.Tables[0].Rows.Count > 0)
                    {
                        txtName.Text = rec.Tables[0].Rows[0]["name"].ToString();
                        txtUrl.Text = rec.Tables[0].Rows[0]["url"].ToString();
                        ddlEnvironment.SelectedValue = rec.Tables[0].Rows[0]["environment"].ToString();
                        chkEnabled.Checked = (rec.Tables[0].Rows[0]["enabled"].ToString() == "1");
                        btnUpdateVC.Text = "Update";

                        rptDCs.DataSource = oVMWare.GetDatacenters(vc, 0);
                        rptDCs.DataBind();
                    }
                    panVC.Visible = true;
                }
                else
                {
                    panVCs.Visible = true;
                    rptVCs.DataSource = oVMWare.GetVirtualCenters(0);
                    rptVCs.DataBind();
                }
            }
        }
        protected string FormURL(string _additional_querystring)
        {
            string strRedirect = "";
            strRedirect += BuildURL("id", strRedirect);
            strRedirect += BuildURL("tid", strRedirect);
            if (_additional_querystring != "")
            {
                if (strRedirect == "")
                    _additional_querystring = "?" + _additional_querystring;
                else
                    _additional_querystring = "&" + _additional_querystring;
            }
            return oPage.GetFullLink(intPage) + strRedirect + _additional_querystring;
        }
        protected string BuildURL(string _value, string _url)
        {
            string strReturn = "";
            if (Request.QueryString[_value] != null)
            {
                if (_url == "")
                    strReturn = "?" + _value + "=" + Request.QueryString[_value];
                else
                    strReturn = "&" + _value + "=" + Request.QueryString[_value];
            }
            return strReturn;
        }





        protected void btnAddVirtualCenter_Click(object sender, EventArgs e)
        {
            Response.Redirect(FormURL("vc=0"));
        }

        protected void btnAddDataCenter_Click(object sender, EventArgs e)
        {
            LinkButton button = (LinkButton)sender;
            Response.Redirect(FormURL("dc=0&vc=" + button.CommandArgument));
        }

        protected void btnAddFolder_Click(object sender, EventArgs e)
        {
            LinkButton button = (LinkButton)sender;
            Response.Redirect(FormURL("f=0&dc=" + button.CommandArgument));
        }

        protected void btnAddCluster_Click(object sender, EventArgs e)
        {
            LinkButton button = (LinkButton)sender;
            Response.Redirect(FormURL("c=0&f=" + button.CommandArgument));
        }

        protected void btnAddHost_Click(object sender, EventArgs e)
        {
            LinkButton button = (LinkButton)sender;
            Response.Redirect(FormURL("h=0&c=" + button.CommandArgument));
        }

        protected void btnAddDatastore_Click(object sender, EventArgs e)
        {
            LinkButton button = (LinkButton)sender;
            Response.Redirect(FormURL("ds=0&c=" + button.CommandArgument));
        }

        protected void btnAddVLAN_Click(object sender, EventArgs e)
        {
            LinkButton button = (LinkButton)sender;
            Response.Redirect(FormURL("v=0&c=" + button.CommandArgument));
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect(FormURL(""));
        }


        
        protected void btnUpdateVC_Click(object sender, EventArgs e)
        {
            int vc = 0;
            if (Int32.TryParse(Request.QueryString["vc"], out vc) == true)
                oVMWare.UpdateVirtualCenter(vc, txtName.Text, txtUrl.Text, Int32.Parse(ddlEnvironment.SelectedItem.Value), (chkEnabled.Checked ? 1 : 0));
            else
                oVMWare.AddVirtualCenter(txtName.Text, txtUrl.Text, Int32.Parse(ddlEnvironment.SelectedItem.Value), (chkEnabled.Checked ? 1 : 0));
            Response.Redirect(FormURL(""));
        }

        protected void btnUpdateDC_Click(object sender, EventArgs e)
        {
            int dc = 0;
            if (Int32.TryParse(Request.QueryString["dc"], out dc) == true)
                oVMWare.UpdateDatacenter(dc, Int32.Parse(ddlDCParent.SelectedItem.Value), txtDCName.Text, txtDCBuildFolder.Text, txtDCDesktopNetwork.Text, txtDCWorkstationDecomVLAN.Text, (chkDCEnabled.Checked ? 1 : 0));
            else
                oVMWare.AddDatacenter(Int32.Parse(ddlDCParent.SelectedItem.Value), txtDCName.Text, txtDCBuildFolder.Text, txtDCDesktopNetwork.Text, txtDCWorkstationDecomVLAN.Text, (chkDCEnabled.Checked ? 1 : 0));
            Response.Redirect(FormURL(""));
        }

        protected void btnUpdateF_Click(object sender, EventArgs e)
        {
            int f = 0;
            if (Int32.TryParse(Request.QueryString["f"], out f) == true)
                oVMWare.UpdateFolder(f, Int32.Parse(lblFParent.ToolTip), txtFName.Text, txtFNotification.Text, 0, 0, 0, (chkFEnabled.Checked ? 1 : 0));
            else
                oVMWare.AddFolder(Int32.Parse(lblFParent.ToolTip), txtFName.Text, txtFNotification.Text, 0, 0, 0, (chkFEnabled.Checked ? 1 : 0));
            Response.Redirect(FormURL(""));
        }

        protected void btnUpdateC_Click(object sender, EventArgs e)
        {
            int intMaximum = 0;
            if (Int32.TryParse(txtCMaximum.Text, out intMaximum) == false)
                intMaximum = 99999;
            int c = 0;
            if (Int32.TryParse(Request.QueryString["c"], out c) == true)
                oVMWare.UpdateCluster(c, Int32.Parse(lblCParent.ToolTip), 0, txtCName.Text, Int32.Parse(ddlVersion.SelectedItem.Value), Int32.Parse(ddlAntiAffinity.SelectedItem.Value), intMaximum, txtResourcePool.Text, txtDatastoreNotify.Text, Int32.Parse(txtDatastoreLeft.Text), Int32.Parse(txtDatastoreSize.Text), 1, (chkFull.Checked ? 1 : 0), (chkAPoff.Checked ? 1 : 0), (chkAPoffDR.Checked ? 1 : 0), (chkDell.Checked ? 1 : 0), Int32.Parse(ddlResiliency.SelectedItem.Value), (chkOracle.Checked ? 1 : 0), (chkOracleCluster.Checked ? 1 : 0), (chkSQL.Checked ? 1 : 0), (chkSQLCluster.Checked ? 1 : 0), (chkCluster.Checked ? 1 : 0), double.Parse(txtFailures.Text), double.Parse(txtCPUUtilization.Text), double.Parse(txtRAMUtilization.Text), double.Parse(txtMaxRAM.Text), double.Parse(txtAvgUtilization.Text), double.Parse(txtLunSize.Text), double.Parse(txtLunUtilization.Text), double.Parse(txtVMsPerLun.Text), double.Parse(txtTimeLUN.Text), double.Parse(txtTimeCluster.Text), double.Parse(txtMaxVMsServer.Text), double.Parse(txtMaxVMsLUN.Text), (chkCEnabled.Checked ? 1 : 0));
            else
                oVMWare.AddCluster(Int32.Parse(lblCParent.ToolTip), 0, txtCName.Text, Int32.Parse(ddlVersion.SelectedItem.Value), Int32.Parse(ddlAntiAffinity.SelectedItem.Value), intMaximum, txtResourcePool.Text, txtDatastoreNotify.Text, Int32.Parse(txtDatastoreLeft.Text), Int32.Parse(txtDatastoreSize.Text), 1, (chkFull.Checked ? 1 : 0), (chkAPoff.Checked ? 1 : 0), (chkAPoffDR.Checked ? 1 : 0), (chkDell.Checked ? 1 : 0), Int32.Parse(ddlResiliency.SelectedItem.Value), (chkOracle.Checked ? 1 : 0), (chkOracleCluster.Checked ? 1 : 0), (chkSQL.Checked ? 1 : 0), (chkSQLCluster.Checked ? 1 : 0), (chkCluster.Checked ? 1 : 0), 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, (chkCEnabled.Checked ? 1 : 0));
            Response.Redirect(FormURL(""));
        }

        protected void btnUpdateH_Click(object sender, EventArgs e)
        {
            int intMaximum = 0;
            if (Int32.TryParse(txtCMaximum.Text, out intMaximum) == false)
                intMaximum = 99999;
            int h = 0;
            if (Int32.TryParse(Request.QueryString["h"], out h) == true)
                oVMWare.UpdateHost(h, Int32.Parse(lblHParent.ToolTip), txtHName.Text, intMaximum, (chkHEnabled.Checked ? 1 : 0));
            else
                oVMWare.AddHost(Int32.Parse(lblHParent.ToolTip), txtHName.Text, intMaximum, (chkHEnabled.Checked ? 1 : 0));
            Response.Redirect(FormURL(""));
        }

        protected void btnUpdateDS_Click(object sender, EventArgs e)
        {
            int intMaximum = 0;
            if (Int32.TryParse(txtDSMaximum.Text, out intMaximum) == false)
                intMaximum = 99999;
            int ds = 0;
            if (Int32.TryParse(Request.QueryString["ds"], out ds) == true)
                oVMWare.UpdateDatastore(ds, Int32.Parse(lblDSParent.ToolTip), txtDSName.Text, Int32.Parse(ddlType.SelectedItem.Value), Int32.Parse(ddlOperatingSystemGroup.SelectedItem.Value), (chkReplicated.Checked ? 1 : 0), intMaximum, (chkServer.Checked ? 1 : 0), (chkPagefile.Checked ? 1 : 0), (chkOverridePermission.Checked ? 1 : 0), Int32.Parse(ddlPartner.SelectedItem.Value), (chkDSEnabled.Checked ? 1 : 0));
            else
                oVMWare.AddDatastore(Int32.Parse(lblDSParent.ToolTip), txtDSName.Text, Int32.Parse(ddlType.SelectedItem.Value), Int32.Parse(ddlOperatingSystemGroup.SelectedItem.Value), (chkReplicated.Checked ? 1 : 0), intMaximum, (chkServer.Checked ? 1 : 0), (chkPagefile.Checked ? 1 : 0), (chkOverridePermission.Checked ? 1 : 0), Int32.Parse(ddlPartner.SelectedItem.Value), (chkDSEnabled.Checked ? 1 : 0));
            Response.Redirect(FormURL(""));
        }

        protected void btnUpdateV_Click(object sender, EventArgs e)
        {
            int v = 0;
            if (Int32.TryParse(Request.QueryString["v"], out v) == true)
                oVMWare.UpdateVlan(v, Int32.Parse(lblVParent.ToolTip), txtVName.Text, (chkVEnabled.Checked ? 1 : 0));
            else
                oVMWare.AddVlan(Int32.Parse(lblVParent.ToolTip), txtVName.Text, (chkVEnabled.Checked ? 1 : 0));
            Response.Redirect(FormURL(""));
        }
    }
}