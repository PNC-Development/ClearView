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
using Vim25Api;
namespace NCC.ClearView.Presentation.Web
{
    public partial class shared_threshold : System.Web.UI.UserControl
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intProfile;
        protected int intApplication = 0;
        protected int intPage = 0;

        protected Pages oPage;
        protected VMWare oVMWare;
        protected ModelsProperties oModelsProperties;

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
            oModelsProperties = new ModelsProperties(intProfile, dsn);

            if (!IsPostBack)
            {
                if (String.IsNullOrEmpty(Request.QueryString["tid"]) == false)
                {
                    int tid = 0;
                    if (Int32.TryParse(Request.QueryString["tid"], out tid) == true)
                    {
                        if (tid == 70)  // VMWare
                        {

                            if (String.IsNullOrEmpty(Request.QueryString["ds"]) == false)
                            {
                                DataSet rec = oVMWare.GetDatastore(Int32.Parse(Request.QueryString["ds"]));
                                if (rec.Tables[0].Rows.Count > 0)
                                {
                                    lblDSParent.ToolTip = rec.Tables[0].Rows[0]["clusterid"].ToString();
                                    lblDSParent.Text = oVMWare.GetCluster(Int32.Parse(lblDSParent.ToolTip), "name");
                                    lblDSName.Text = rec.Tables[0].Rows[0]["name"].ToString();
                                    ddlType.Text = rec.Tables[0].Rows[0]["storage_type"].ToString();
                                    ddlOperatingSystemGroup.Text = rec.Tables[0].Rows[0]["osgroupid"].ToString();
                                    chkReplicated.Text = rec.Tables[0].Rows[0]["replicated"].ToString();
                                    txtDSMaximum.Text = rec.Tables[0].Rows[0]["maximum"].ToString();
                                    chkServer.Text = rec.Tables[0].Rows[0]["server"].ToString();
                                    chkPagefile.Text = rec.Tables[0].Rows[0]["pagefile"].ToString();
                                    chkOverridePermission.Text = rec.Tables[0].Rows[0]["override_permission"].ToString();
                                    ddlPartner.Text = rec.Tables[0].Rows[0]["partner"].ToString();
                                    chkDSEnabled.Text = rec.Tables[0].Rows[0]["enabled"].ToString();
                                    btnUpdateDS.Text = "Update";
                                    panDS.Visible = true;
                                }
                            }
                            else if (String.IsNullOrEmpty(Request.QueryString["h"]) == false)
                            {
                                DataSet rec = oVMWare.GetHost(Int32.Parse(Request.QueryString["h"]));
                                if (rec.Tables[0].Rows.Count > 0)
                                {
                                    lblHParent.ToolTip = rec.Tables[0].Rows[0]["clusterid"].ToString();
                                    lblHParent.Text = oVMWare.GetCluster(Int32.Parse(lblHParent.ToolTip), "name");
                                    lblHName.Text = rec.Tables[0].Rows[0]["name"].ToString();
                                    txtHMaximum.Text = rec.Tables[0].Rows[0]["maximum"].ToString();
                                    chkHEnabled.Text = rec.Tables[0].Rows[0]["enabled"].ToString();
                                    btnUpdateH.Text = "Update";
                                    panH.Visible = true;
                                }
                            }
                            else if (String.IsNullOrEmpty(Request.QueryString["c"]) == false)
                            {
                                DataSet rec = oVMWare.GetCluster(Int32.Parse(Request.QueryString["c"]));
                                if (rec.Tables[0].Rows.Count > 0)
                                {
                                    int folder = Int32.Parse(rec.Tables[0].Rows[0]["folderid"].ToString());
                                    int datacenter = Int32.Parse(oVMWare.GetFolder(folder, "datacenterid"));
                                    lblCParent.ToolTip = folder.ToString();
                                    lblCParent.Text = oVMWare.GetFolder(folder, "name");
                                    lblCName.Text = rec.Tables[0].Rows[0]["name"].ToString();
                                    ddlVersion.Text = rec.Tables[0].Rows[0]["version"].ToString();
                                    ddlAntiAffinity.Text = rec.Tables[0].Rows[0]["anti_affinity"].ToString();
                                    txtCMaximum.Text = rec.Tables[0].Rows[0]["maximum"].ToString();
                                    txtResourcePool.Text = rec.Tables[0].Rows[0]["resource_pool"].ToString();
                                    txtDatastoreNotify.Text = rec.Tables[0].Rows[0]["datastores_notify"].ToString();
                                    txtDatastoreLeft.Text = rec.Tables[0].Rows[0]["datastores_left"].ToString();
                                    txtDatastoreSize.Text = rec.Tables[0].Rows[0]["datastores_size"].ToString();
                                    chkFull.Text = rec.Tables[0].Rows[0]["at_max"].ToString();
                                    chkAPoff.Text = rec.Tables[0].Rows[0]["auto_provision_off"].ToString();
                                    chkAPoffDR.Text = rec.Tables[0].Rows[0]["auto_provision_dr_off"].ToString();
                                    chkDell.Text = rec.Tables[0].Rows[0]["dell"].ToString();
                                    ddlResiliency.Text = rec.Tables[0].Rows[0]["resiliencyid"].ToString();
                                    chkOracle.Text = rec.Tables[0].Rows[0]["can_oracle"].ToString();
                                    chkOracleCluster.Text = rec.Tables[0].Rows[0]["can_oracle_cluster"].ToString();
                                    chkSQL.Text = rec.Tables[0].Rows[0]["can_sql"].ToString();
                                    chkSQLCluster.Text = rec.Tables[0].Rows[0]["can_sql_cluster"].ToString();
                                    chkCluster.Text = rec.Tables[0].Rows[0]["can_cluster"].ToString();
                                    chkCEnabled.Text = rec.Tables[0].Rows[0]["enabled"].ToString();

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
                                    if (rec.Tables[0].Rows[0]["input_failures"].ToString() != "")
                                        dblA3 = double.Parse(rec.Tables[0].Rows[0]["input_failures"].ToString());
                                    txtFailures.Text = dblA3.ToString();
                                    if (rec.Tables[0].Rows[0]["input_cpu_utilization"].ToString() != "")
                                        dblA4 = double.Parse(rec.Tables[0].Rows[0]["input_cpu_utilization"].ToString());
                                    txtCPUUtilization.Text = dblA4.ToString();
                                    if (rec.Tables[0].Rows[0]["input_ram_utilization"].ToString() != "")
                                        dblA5 = double.Parse(rec.Tables[0].Rows[0]["input_ram_utilization"].ToString());
                                    txtRAMUtilization.Text = dblA5.ToString();
                                    if (rec.Tables[0].Rows[0]["input_max_ram"].ToString() != "")
                                        dblA8 = double.Parse(rec.Tables[0].Rows[0]["input_max_ram"].ToString());
                                    txtMaxRAM.Text = dblA8.ToString();
                                    if (rec.Tables[0].Rows[0]["input_avg_utilization"].ToString() != "")
                                        dblA10 = double.Parse(rec.Tables[0].Rows[0]["input_avg_utilization"].ToString());
                                    txtAvgUtilization.Text = dblA10.ToString();
                                    if (rec.Tables[0].Rows[0]["input_lun_size"].ToString() != "")
                                        dblA20 = double.Parse(rec.Tables[0].Rows[0]["input_lun_size"].ToString());
                                    txtLunSize.Text = dblA20.ToString();
                                    if (rec.Tables[0].Rows[0]["input_lun_utilization"].ToString() != "")
                                        dblA21 = double.Parse(rec.Tables[0].Rows[0]["input_lun_utilization"].ToString());
                                    txtLunUtilization.Text = dblA21.ToString();
                                    if (rec.Tables[0].Rows[0]["input_vms_per_lun"].ToString() != "")
                                        dblA22 = double.Parse(rec.Tables[0].Rows[0]["input_vms_per_lun"].ToString());
                                    txtVMsPerLun.Text = dblA22.ToString();
                                    if (rec.Tables[0].Rows[0]["input_time_lun"].ToString() != "")
                                        dblA25 = double.Parse(rec.Tables[0].Rows[0]["input_time_lun"].ToString());
                                    txtTimeLUN.Text = dblA25.ToString();
                                    if (rec.Tables[0].Rows[0]["input_time_cluster"].ToString() != "")
                                        dblA26 = double.Parse(rec.Tables[0].Rows[0]["input_time_cluster"].ToString());
                                    txtTimeCluster.Text = dblA26.ToString();
                                    if (rec.Tables[0].Rows[0]["input_max_vms_server"].ToString() != "")
                                        dblA29 = double.Parse(rec.Tables[0].Rows[0]["input_max_vms_server"].ToString());
                                    txtMaxVMsServer.Text = dblA29.ToString();
                                    if (rec.Tables[0].Rows[0]["input_max_vms_lun"].ToString() != "")
                                        dblA30 = double.Parse(rec.Tables[0].Rows[0]["input_max_vms_lun"].ToString());
                                    txtMaxVMsLUN.Text = dblA30.ToString();
                                    double dblA13 = 0.00;
                                    double dblA14 = 0.00;
                                    double dblA15 = 0.00;
                                    double dblA16 = 0.00;
                                    int intModel = Int32.Parse(rec.Tables[0].Rows[0]["modelid"].ToString());
                                    DataSet dsModel = oModelsProperties.Get(intModel);
                                    if (dsModel.Tables[0].Rows.Count > 0)
                                    {
                                        //lblModel.Text = dsModel.Tables[0].Rows[0]["name"].ToString();
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
                                    //if (Request.QueryString["query"] != null)
                                    //{
                                        // Get # of hosts from cluster
                                        int intVirtualCenter = Int32.Parse(oVMWare.GetDatacenter(datacenter, "virtualcenterid"));
                                        string strVirtualCenter = oVMWare.GetVirtualCenter(intVirtualCenter, "name");
                                        string strConnect = oVMWare.ConnectDEBUG(oVMWare.GetVirtualCenter(intVirtualCenter, "url"), Int32.Parse(oVMWare.GetVirtualCenter(intVirtualCenter, "environment")), oVMWare.GetDatacenter(datacenter, "name"));
                                        if (strConnect == "")
                                        {
                                            VimService _service = oVMWare.GetService();
                                            ServiceContent _sic = oVMWare.GetSic();
                                            ManagedObjectReference[] hostRefs = oVMWare.GetHosts(lblCName.Text);
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
                                    //}
                                    //else
                                    //    dblA2 = -1.00;
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

                                    btnUpdateC.Text = "Update";
                                    panC.Visible = true;
                                }
                            }
                            else if (String.IsNullOrEmpty(Request.QueryString["f"]) == false)
                            {
                                DataSet rec = oVMWare.GetFolder(Int32.Parse(Request.QueryString["f"]));
                                if (rec.Tables[0].Rows.Count > 0)
                                {
                                    lblFParent.ToolTip = rec.Tables[0].Rows[0]["datacenterid"].ToString();
                                    lblFParent.Text = oVMWare.GetDatacenter(Int32.Parse(lblFParent.ToolTip), "name");
                                    lblFName.Text = rec.Tables[0].Rows[0]["name"].ToString();
                                    txtFNotification.Text = rec.Tables[0].Rows[0]["notification"].ToString();
                                    chkFEnabled.Text = rec.Tables[0].Rows[0]["enabled"].ToString();
                                    btnUpdateF.Text = "Update";
                                    panF.Visible = true;
                                }
                            }
                            else
                                panNone.Visible = true;
                        }
                        else
                            panNone.Visible = true;
                    }
                    else
                        panNone.Visible = true;
                }
                else
                    panNone.Visible = true;
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


        
        protected void btnUpdateF_Click(object sender, EventArgs e)
        {
            int f = 0;
            if (Int32.TryParse(Request.QueryString["f"], out f) == true)
                oVMWare.UpdateFolder(f, Int32.Parse(lblFParent.ToolTip), lblFName.Text, txtFNotification.Text, 0, 0, 0, (chkFEnabled.Text == "1" ? 1 : 0));
            Response.Redirect(FormURL("f=" + f));
        }

        protected void btnUpdateC_Click(object sender, EventArgs e)
        {
            int intMaximum = 0;
            if (Int32.TryParse(txtCMaximum.Text, out intMaximum) == false)
                intMaximum = 99999;
            int c = 0;
            if (Int32.TryParse(Request.QueryString["c"], out c) == true)
                oVMWare.UpdateCluster(c, Int32.Parse(lblCParent.ToolTip), 0, lblCName.Text, Int32.Parse(ddlVersion.Text), Int32.Parse(ddlAntiAffinity.Text), intMaximum, txtResourcePool.Text, txtDatastoreNotify.Text, Int32.Parse(txtDatastoreLeft.Text), Int32.Parse(txtDatastoreSize.Text), 1, (chkFull.Text == "1" ? 1 : 0), (chkAPoff.Text == "1" ? 1 : 0), (chkAPoffDR.Text == "1" ? 1 : 0), (chkDell.Text == "1" ? 1 : 0), Int32.Parse(ddlResiliency.Text), Int32.Parse(chkOracle.Text), Int32.Parse(chkOracleCluster.Text), Int32.Parse(chkSQL.Text), Int32.Parse(chkSQLCluster.Text), Int32.Parse(chkCluster.Text), double.Parse(txtFailures.Text), double.Parse(txtCPUUtilization.Text), double.Parse(txtRAMUtilization.Text), double.Parse(txtMaxRAM.Text), double.Parse(txtAvgUtilization.Text), double.Parse(txtLunSize.Text), double.Parse(txtLunUtilization.Text), double.Parse(txtVMsPerLun.Text), double.Parse(txtTimeLUN.Text), double.Parse(txtTimeCluster.Text), double.Parse(txtMaxVMsServer.Text), double.Parse(txtMaxVMsLUN.Text), (chkCEnabled.Text == "1" ? 1 : 0));
            Response.Redirect(FormURL("c=" + c));
        }

        protected void btnUpdateH_Click(object sender, EventArgs e)
        {
            int intMaximum = 0;
            if (Int32.TryParse(txtCMaximum.Text, out intMaximum) == false)
                intMaximum = 99999;
            int h = 0;
            if (Int32.TryParse(Request.QueryString["h"], out h) == true)
                oVMWare.UpdateHost(h, Int32.Parse(lblHParent.ToolTip), lblHName.Text, intMaximum, (chkHEnabled.Text == "1" ? 1 : 0));
            Response.Redirect(FormURL("h=" + h));
        }

        protected void btnUpdateDS_Click(object sender, EventArgs e)
        {
            int intMaximum = 0;
            if (Int32.TryParse(txtDSMaximum.Text, out intMaximum) == false)
                intMaximum = 99999;
            int ds = 0;
            if (Int32.TryParse(Request.QueryString["ds"], out ds) == true)
                oVMWare.UpdateDatastore(ds, Int32.Parse(lblDSParent.ToolTip), lblDSName.Text, Int32.Parse(ddlType.Text), Int32.Parse(ddlOperatingSystemGroup.Text), (chkReplicated.Text == "1" ? 1 : 0), intMaximum, (chkServer.Text == "1" ? 1 : 0), (chkPagefile.Text == "1" ? 1 : 0), (chkOverridePermission.Text == "1" ? 1 : 0), Int32.Parse(ddlPartner.Text), (chkDSEnabled.Text == "1" ? 1 : 0));
            Response.Redirect(FormURL("ds=" + ds));
        }
    }
}