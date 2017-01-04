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
using System.IO;
using NCC.ClearView.Presentation.Web.Custom;
namespace NCC.ClearView.Presentation.Web
{
    public partial class sve : System.Web.UI.UserControl
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected Pages oPage;
        protected Servers oServer;
        protected Solaris oSolaris;
        protected Classes oClass;
        protected IPAddresses oIPAddresses;
        protected Asset oAsset;
        protected Functions oFunction;
        protected ServiceRequests oServiceRequest;
        protected Mnemonic oMnemonic;
        protected Forecast oForecast;
        protected Storage oStorage;
        protected TSM oTSM;
        protected Resiliency oResiliency;
        protected Locations oLocation;
        protected int intProfile;
        protected int intApplication = 0;
        protected int intPage = 0;

        protected int intCluster = 0;
        protected int intHost = 0;
        protected int intGuest = 0;
        protected string strGuests = "";
        protected int intProcessor = 0;
        protected int intSubnet = 0;
        protected int intMnemonic = 0;
        protected int intGuestCount = 1;
        protected string strMenuTab1 = "";
        protected string strLocation = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oPage = new Pages(intProfile, dsn);
            oServer = new Servers(intProfile, dsn);
            oSolaris = new Solaris(intProfile, dsn);
            oClass = new Classes(intProfile, dsn);
            oIPAddresses = new IPAddresses(intProfile, dsnIP, dsn);
            oAsset = new Asset(intProfile, dsnAsset, dsn);
            oFunction = new Functions(intProfile, dsn, intEnvironment);
            oServiceRequest = new ServiceRequests(intProfile, dsn);
            oMnemonic = new Mnemonic(intProfile, dsn);
            oForecast = new Forecast(intProfile, dsn);
            oStorage = new Storage(intProfile, dsn);
            oTSM = new TSM(intProfile, dsn);
            oResiliency = new Resiliency(intProfile, dsn);
            oLocation = new Locations(intProfile, dsn);
            if (Request.QueryString["applicationid"] != null && Request.QueryString["applicationid"] != "")
                intApplication = Int32.Parse(Request.QueryString["applicationid"]);
            if (Request.QueryString["pageid"] != null && Request.QueryString["pageid"] != "")
                intPage = Int32.Parse(Request.QueryString["pageid"]);
            if (Request.Cookies["application"] != null && Request.Cookies["application"].Value != "")
                intApplication = Int32.Parse(Request.Cookies["application"].Value);
            
            if (Request.QueryString["cluster"] != null && Request.QueryString["cluster"] != "")
                Int32.TryParse(Request.QueryString["cluster"], out intCluster);

            if (Request.QueryString["save"] != null && Request.QueryString["save"] != "")
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "saved", "<script type=\"text/javascript\">alert('Information Saved Successfully');<" + "/" + "script>");
            if (Request.QueryString["delete"] != null && Request.QueryString["delete"] != "")
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "deleted", "<script type=\"text/javascript\">alert('Item Deleted Successfully');<" + "/" + "script>");

            if (Request.QueryString["host"] != null && Request.QueryString["host"] != "")
            {
                panHost.Visible = true;
                Int32.TryParse(Request.QueryString["host"], out intHost);
                lblHostCluster.Text = oSolaris.GetSVECluster(intCluster, "name");
                if (!IsPostBack)
                {
                    int intBackupServer = 0;
                    int intBackupDomain = 0;
                    int intBackupSchedule = 0;

                    DataSet ds = oServer.Get(intHost);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        txtHost.Text = ds.Tables[0].Rows[0]["servername"].ToString();
                        hdnHost.Value = intHost.ToString();
                        int intAsset = 0;
                        Int32.TryParse(ds.Tables[0].Rows[0]["assetid"].ToString(), out intAsset);
                        lblSerial.Text = oAsset.Get(intAsset, "serial");
                        lblSerial.Text = "<a href=\"javascript:void(0);\" onclick=\"OpenNewWindow('/datapoint/asset/datapoint_asset_search.aspx?t=serial&q=" + oFunction.encryptQueryString(lblSerial.Text) + "',800,600);\">" + lblSerial.Text + "</a>";
                        trHost1.Visible = trHost2.Visible = trHost3.Visible = trHost4.Visible = true;
                        Int32.TryParse(ds.Tables[0].Rows[0]["tsm_schedule"].ToString(), out intBackupSchedule);
                        Int32.TryParse(oTSM.GetSchedule(intBackupSchedule, "domain"), out intBackupDomain);
                        Int32.TryParse(oTSM.GetDomain(intBackupDomain, "tsm"), out intBackupServer);
                        btnHostUpdate.Visible = true;
                        btnHostUpdate.Attributes.Add("onclick", "return ValidateHidden0('" + hdnHost.ClientID + "','" + txtHost.ClientID + "','Please enter a hostname\\n\\n(Start typing and a list will be presented...)')" +
                            " && ProcessButton(this) && LoadWait()" +
                            ";");
                        btnHostDelete.Visible = true;
                        btnHostDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this item') && ProcessButton(this) && LoadWait();");
                    }
                    else
                    {
                        btnHostAdd.Visible = true;
                        btnHostAdd.Attributes.Add("onclick", "return ValidateHidden0('" + hdnHost.ClientID + "','" + txtHost.ClientID + "','Please enter a hostname\\n\\n(Start typing and a list will be presented...)')" +
                            " && ProcessButton(this) && LoadWait()" +
                            ";");
                        btnHostAdd.Enabled = (oServer.GetSVEClusters(intCluster).Tables[0].Rows.Count < 8);
                    }
                    btnHostCancel.Attributes.Add("onclick", "return ProcessButton(this) && LoadWait();");


                    ddlBackupServer.DataTextField = "name";
                    ddlBackupServer.DataValueField = "id";
                    ddlBackupServer.DataSource = oTSM.Gets(1, 0, 1);
                    ddlBackupServer.DataBind();
                    ddlBackupServer.Items.Insert(0, new ListItem("-- SELECT --", "0"));
                    ddlBackupServer.SelectedValue = intBackupServer.ToString();

                    if (intBackupServer > 0)
                    {
                        ddlBackupDomain.DataTextField = "name";
                        ddlBackupDomain.DataValueField = "id";
                        ddlBackupDomain.DataSource = oTSM.GetDomains(intBackupServer, 1);
                        ddlBackupDomain.DataBind();
                        ddlBackupDomain.Items.Insert(0, new ListItem("-- SELECT --", "0"));
                        ddlBackupDomain.SelectedValue = intBackupDomain.ToString();
                    }
                    else
                    {
                        ddlBackupDomain.Items.Insert(0, new ListItem("-- SELECT A SERVER --", "0"));
                        ddlBackupDomain.Enabled = false;
                        ddlBackupSchedule.Items.Insert(0, new ListItem("-- SELECT A SERVER --", "0"));
                        ddlBackupSchedule.Enabled = false;
                    }

                    if (intBackupDomain > 0)
                    {
                        ddlBackupSchedule.DataTextField = "name";
                        ddlBackupSchedule.DataValueField = "id";
                        ddlBackupSchedule.DataSource = oTSM.GetSchedules(intBackupDomain, 1);
                        ddlBackupSchedule.DataBind();
                        ddlBackupSchedule.Items.Insert(0, new ListItem("-- SELECT --", "0"));
                        ddlBackupSchedule.SelectedValue = intBackupSchedule.ToString();
                    }
                    else
                    {
                        ddlBackupSchedule.Items.Insert(0, new ListItem("-- SELECT A DOMAIN --", "0"));
                        ddlBackupSchedule.Enabled = false;
                    }
                    ddlBackupServer.Attributes.Add("onchange", "PopulateTSMDomains('" + ddlBackupServer.ClientID + "','" + ddlBackupDomain.ClientID + "','" + ddlBackupSchedule.ClientID + "');ResetDropDownHidden('" + hdnSchedule.ClientID + "');");
                    ddlBackupDomain.Attributes.Add("onchange", "PopulateTSMSchedules('" + ddlBackupDomain.ClientID + "','" + ddlBackupSchedule.ClientID + "');ResetDropDownHidden('" + hdnSchedule.ClientID + "');");
                    ddlBackupSchedule.Attributes.Add("onchange", "UpdateDropDownHidden('" + ddlBackupSchedule.ClientID + "','" + hdnSchedule.ClientID + "');");
                }
            }
            else if (Request.QueryString["guest"] != null && Request.QueryString["guest"] != "")
            {
                panGuest.Visible = true;
                Int32.TryParse(Request.QueryString["guest"], out intGuest);
                lblGuestCluster.Text = oSolaris.GetSVECluster(intCluster, "name");
                if (!IsPostBack)
                {
                    ddlProcessorPool.DataTextField = "name";
                    ddlProcessorPool.DataValueField = "id";
                    ddlProcessorPool.DataSource = oSolaris.GetProcessorPools(intCluster, 1);
                    ddlProcessorPool.DataBind();
                    ddlProcessorPool.Items.Insert(0, new ListItem("-- SELECT --", "0"));

                    DataSet ds = oSolaris.GetSVEGuest(intGuest);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        string strName = oServer.GetName(intGuest, true);
                        if (strName.EndsWith("A") || strName.EndsWith("B"))
                            strName = strName.Substring(0, strName.Length - 1);
                        lblGuest.Text = strName;
                        ddlProcessorPool.SelectedValue = ds.Tables[0].Rows[0]["poolid"].ToString();
                        int intAnswer = 0;
                        Int32.TryParse(ds.Tables[0].Rows[0]["answerid"].ToString(), out intAnswer);
                        int intClusterID = 0;
                        Int32.TryParse(oServer.Get(intGuest, "clusterid"), out intClusterID);
                        txtGuestCPU.Text = ds.Tables[0].Rows[0]["cpu"].ToString();
                        lblGuestCPU.Text = oForecast.GetCPU(intAnswer).ToString();
                        txtGuestRAM.Text = ds.Tables[0].Rows[0]["ram"].ToString();
                        lblGuestRAM.Text = oForecast.GetRAM(intAnswer).ToString();
                        txtGuestAllocated.Text = ds.Tables[0].Rows[0]["allocated"].ToString();
                        double dblStorage = 0.00;
                        DataSet dsStorage = oStorage.GetLunsClusterShared(intAnswer, intClusterID);
                        foreach (DataRow drStorage in dsStorage.Tables[0].Rows)
                        {
                            double dblTemp = 0.00;
                            double.TryParse(drStorage["size"].ToString(), out dblTemp);
                            dblStorage += dblTemp;
                            double.TryParse(drStorage["size_qa"].ToString(), out dblTemp);
                            dblStorage += dblTemp;
                            double.TryParse(drStorage["size_test"].ToString(), out dblTemp);
                            dblStorage += dblTemp;
                        }
                        lblGuestAllocated.Text = dblStorage.ToString();
                        btnGuestUpdate.Visible = true;
                        btnGuestUpdate.Attributes.Add("onclick", "return ValidateDropDown('" + ddlProcessorPool.ClientID + "','Please select a processor pool')" +
                            " && ValidateNumber('" + txtGuestAllocated.ClientID + "','Please enter a valid number for the storage allocation')" +
                            " && ValidateNumber('" + txtGuestCPU.ClientID + "','Please enter a valid number for the number of CPUs')" +
                            " && ValidateNumber('" + txtGuestRAM.ClientID + "','Please enter a valid number for the amount of RAM')" +
                            " && ProcessButton(this) && LoadWait()" +
                            ";");
                    }
                }
                btnGuestCancel2.Attributes.Add("onclick", "return ProcessButton(this) && LoadWait();");
            }
            else if (Request.QueryString["guests"] != null && Request.QueryString["guests"] != "")
            {
                panGuests.Visible = true;
                strGuests = oFunction.decryptQueryString(Request.QueryString["guests"]);
                lblGuestsCluster.Text = oSolaris.GetSVECluster(intCluster, "name");
                if (!IsPostBack)
                {
                    ddlCluster.DataTextField = "name";
                    ddlCluster.DataValueField = "id";
                    ddlCluster.DataSource = oSolaris.GetSVEClusters(1, 0, -1, 1);
                    ddlCluster.DataBind();
                    ddlCluster.Items.Insert(0, new ListItem("-- SELECT --", "0"));

                    //90_1&77_1&
                    while (strGuests != "")
                    {
                        string strField = strGuests.Substring(0, strGuests.IndexOf("&"));
                        strGuests = strGuests.Substring(strGuests.IndexOf("&") + 1);
                        string strFlag = strField.Substring(strField.IndexOf("_") + 1);
                        strField = strField.Substring(0, strField.IndexOf("_"));
                        if (strFlag == "1")
                        {
                            int intServerID = Int32.Parse(strField);
                            TableRow oRow = new TableRow();
                            TableCell oCell = new TableCell();
                            string strName = oServer.GetName(intServerID, true);
                            if (strName.EndsWith("A") || strName.EndsWith("B"))
                                strName = strName.Substring(0, strName.Length - 1);
                            oCell.Text = strName;
                            oRow.Cells.Add(oCell);
                            tblGuests.Rows.Add(oRow);
                        }
                    }
                    tblGuests.Style["border"] = "solid 1px #CCCCCC";
                    btnGuestMove.Attributes.Add("onclick", "return ValidateDropDown('" + ddlCluster.ClientID + "','Please select a cluster')" +
                        " && ProcessButton(this) && LoadWait()" +
                        ";");
                    btnGuestCancel.Attributes.Add("onclick", "return ProcessButton(this) && LoadWait();");
                }
            }
            else if (Request.QueryString["processor"] != null && Request.QueryString["processor"] != "")
            {
                panProcessor.Visible = true;
                Int32.TryParse(Request.QueryString["processor"], out intProcessor);
                lblProcessorCluster.Text = oSolaris.GetSVECluster(intCluster, "name");
                if (!IsPostBack)
                {
                    DataSet ds = oSolaris.GetProcessorPool(intProcessor);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        txtProcessorName.Text = ds.Tables[0].Rows[0]["name"].ToString();
                        txtProcessorDescription.Text = ds.Tables[0].Rows[0]["description"].ToString();
                        txtProcessorLow.Text = ds.Tables[0].Rows[0]["low"].ToString();
                        txtProcessorHigh.Text = ds.Tables[0].Rows[0]["high"].ToString();
                        txtProcessorWarning.Text = ds.Tables[0].Rows[0]["warning"].ToString();
                        txtProcessorCritical.Text = ds.Tables[0].Rows[0]["critical"].ToString();
                        txtProcessorError.Text = ds.Tables[0].Rows[0]["error"].ToString();
                        chkProcessorEnabled.Checked = (ds.Tables[0].Rows[0]["enabled"].ToString() == "1");
                        btnProcessorUpdate.Visible = true;
                        btnProcessorUpdate.Attributes.Add("onclick", "return ValidateText('" + txtProcessorName.ClientID + "','Please enter a name')" +
                            " && ValidateNumber('" + txtProcessorLow.ClientID + "','Please enter a valid number for the low availability')" +
                            " && ValidateNumber('" + txtProcessorHigh.ClientID + "','Please enter a valid number for the high availability')" +
                            " && ValidateNumber('" + txtProcessorWarning.ClientID + "','Please enter a valid number for the warning threshold')" +
                            " && ValidateNumber('" + txtProcessorCritical.ClientID + "','Please enter a valid number for the critical threshold')" +
                            " && ValidateNumber('" + txtProcessorError.ClientID + "','Please enter a valid number for the error threshold')" +
                            " && ProcessButton(this) && LoadWait()" +
                            ";");
                        btnProcessorDelete.Visible = true;
                        btnProcessorDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this item') && ProcessButton(this) && LoadWait();");
                    }
                    else
                    {
                        btnProcessorAdd.Visible = true;
                        btnProcessorAdd.Attributes.Add("onclick", "return ValidateText('" + txtProcessorName.ClientID + "','Please enter a name')" +
                            " && ValidateNumber('" + txtProcessorLow.ClientID + "','Please enter a valid number for the low availability')" +
                            " && ValidateNumber('" + txtProcessorHigh.ClientID + "','Please enter a valid number for the high availability')" +
                            " && ValidateNumber('" + txtProcessorWarning.ClientID + "','Please enter a valid number for the warning threshold')" +
                            " && ValidateNumber('" + txtProcessorCritical.ClientID + "','Please enter a valid number for the critical threshold')" +
                            " && ValidateNumber('" + txtProcessorError.ClientID + "','Please enter a valid number for the error threshold')" +
                            " && ProcessButton(this) && LoadWait()" +
                            ";");
                    }
                }
                btnProcessorCancel.Attributes.Add("onclick", "return ProcessButton(this) && LoadWait();");
            }
            else if (Request.QueryString["subnet"] != null && Request.QueryString["subnet"] != "")
            {
                Int32.TryParse(Request.QueryString["subnet"], out intSubnet);
            }
            else if (Request.QueryString["mnemonic"] != null && Request.QueryString["mnemonic"] != "")
            {
                panMnemonic.Visible = true;
                Int32.TryParse(Request.QueryString["mnemonic"], out intMnemonic);
                lblMnemonicCluster.Text = oSolaris.GetSVECluster(intCluster, "name");
                if (!IsPostBack)
                {
                    DataSet ds = oSolaris.GetSVEMnemonic(intMnemonic);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        int intMnemonicID = Int32.Parse(ds.Tables[0].Rows[0]["mnemonicid"].ToString());
                        hdnMnemonic.Value = intMnemonicID.ToString();
                        txtMnemonic.Text = oMnemonic.Get(intMnemonicID, "factory_code") + " - " + oMnemonic.Get(intMnemonicID, "name");
                        chkMnemonicEnabled.Checked = (ds.Tables[0].Rows[0]["enabled"].ToString() == "1");
                        btnMnemonicUpdate.Visible = true;
                        btnMnemonicUpdate.Attributes.Add("onclick", "return ValidateHidden0('" + hdnMnemonic.ClientID + "','" + txtMnemonic.ClientID + "','Please enter a mnemonic\\n\\n(Start typing and a list will be presented...)')" +
                            " && ProcessButton(this) && LoadWait()" +
                            ";");
                        btnMnemonicDelete.Visible = true;
                        btnMnemonicDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this item') && ProcessButton(this) && LoadWait();");
                    }
                    else
                    {
                        btnMnemonicAdd.Visible = true;
                        btnMnemonicAdd.Attributes.Add("onclick", "return ValidateHidden0('" + hdnMnemonic.ClientID + "','" + txtMnemonic.ClientID + "','Please enter a mnemonic\\n\\n(Start typing and a list will be presented...)')" +
                            " && ProcessButton(this) && LoadWait()" +
                            ";");
                    }
                    btnMnemonicCancel.Attributes.Add("onclick", "return ProcessButton(this) && LoadWait();");
                }
            }
            else
            {
                if (Request.QueryString["cluster"] == null)
                {
                    panClusters.Visible = true;
                    if (!IsPostBack)
                    {
                        int intAvailable = -1;
                        if (Request.Cookies["sve_available"] != null && String.IsNullOrEmpty(Request.Cookies["sve_available"].Value) == false)
                            intAvailable = Int32.Parse(Request.Cookies["sve_available"].Value);
                        radAvailable.SelectedValue = intAvailable.ToString();
                        int intMnemonics = 0;
                        if (Request.Cookies["sve_mnemonics"] != null && String.IsNullOrEmpty(Request.Cookies["sve_mnemonics"].Value) == false)
                            intMnemonics = Int32.Parse(Request.Cookies["sve_mnemonics"].Value);
                        chkMnemonics.Checked = (intMnemonics == 1);
                        int intTrunking = -1;
                        if (Request.Cookies["sve_trunk"] != null && String.IsNullOrEmpty(Request.Cookies["sve_trunk"].Value) == false)
                            intTrunking = Int32.Parse(Request.Cookies["sve_trunk"].Value);
                        radTrunk.SelectedValue = intTrunking.ToString();
                        rptClusters.DataSource = oSolaris.GetSVEClusters(intAvailable, intMnemonics, intTrunking, 0);
                        rptClusters.DataBind();
                        lblClusters.Visible = (rptClusters.Items.Count == 0);
                        foreach (RepeaterItem ri in rptClusters.Items)
                        {
                            int intID = 0;
                            Label lblGuests = (Label)ri.FindControl("lblGuests");
                            Label lblHosts = (Label)ri.FindControl("lblHosts");
                            Label lblIPs = (Label)ri.FindControl("lblIPs");
                            Label lblPools = (Label)ri.FindControl("lblPools");
                            Label lblStatus = (Label)ri.FindControl("lblStatus");

                            if (Int32.TryParse(lblGuests.Text, out intID) == true)
                            {
                                lblGuests.Text = oSolaris.GetSVEGuests(intID).Tables[0].Rows.Count.ToString();
                                lblHosts.Text = oServer.GetSVEClusters(intID).Tables[0].Rows.Count.ToString();
                                CheckThresholds(intID, lblStatus, lblIPs, lblPools);
                            }
                        }
                        btnFilter.Attributes.Add("onclick", "return ProcessButton(this) && LoadWait();");
                        btnFilterClear.Attributes.Add("onclick", "return ProcessButton(this) && LoadWait();");
                    }
                }
                else
                {
                    panCluster.Visible = true;
                    if (!IsPostBack)
                    {
                        ddlClusterClass.DataValueField = "id";
                        ddlClusterClass.DataTextField = "name";
                        ddlClusterClass.DataSource = oClass.GetForecasts(1);
                        ddlClusterClass.DataBind();
                        ddlClusterClass.Items.Insert(0, new ListItem("-- SELECT --", "0"));
                        ddlClusterResiliency.DataValueField = "id";
                        ddlClusterResiliency.DataTextField = "name";
                        ddlClusterResiliency.DataSource = oResiliency.Gets(1);
                        ddlClusterResiliency.DataBind();
                        ddlClusterResiliency.Items.Insert(0, new ListItem("-- SELECT --", "0"));
                        DataSet ds = oSolaris.GetSVECluster(intCluster);
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            txtClusterName.Text = ds.Tables[0].Rows[0]["name"].ToString();
                            radClusterDB.Checked = (ds.Tables[0].Rows[0]["db"].ToString() == "1");
                            radClusterApp.Checked = (radClusterDB.Checked == false);
                            int intClass = 0;
                            Int32.TryParse(ds.Tables[0].Rows[0]["classid"].ToString(), out intClass);
                            ddlClusterClass.SelectedValue = intClass.ToString();
                            ddlClusterResiliency.SelectedValue = ds.Tables[0].Rows[0]["resiliencyid"].ToString();
                            txtClusterComments.Text = ds.Tables[0].Rows[0]["comments"].ToString();
                            chkClusterStorage.Checked = (ds.Tables[0].Rows[0]["storage_allocated"].ToString() == "1");
                            chkClusterTrunking.Checked = (ds.Tables[0].Rows[0]["trunking"].ToString() == "1");
                            divIP.Visible = (chkClusterTrunking.Checked == false);
                            chkClusterAvailable.Checked = (ds.Tables[0].Rows[0]["available"].ToString() == "1");
                            btnClusterUpdate.Visible = true;
                            btnClusterUpdate.Attributes.Add("onclick", "return ValidateText('" + txtClusterName.ClientID + "','Please enter a name for this cluster')" +
                                " && ValidateDropDown('" + ddlClusterClass.ClientID + "','Please select a class')" +
                                " && ValidateDropDown('" + ddlClusterResiliency.ClientID + "','Please select a resiliency')" +
                                " && ValidateRadioButtons('" + radClusterDB.ClientID + "','" + radClusterApp.ClientID + "','Please select the type of cluster')" +
                                " && ProcessButton(this) && LoadWait()" +
                                ";");
                            btnClusterDelete.Visible = true;
                            btnClusterDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this server') && ProcessButton(this) && LoadWait();");

                            // Thresholds
                            CheckThresholds(intCluster, lblThreshold, null, null);

                            // Locations
                            rptLocations.DataSource = oSolaris.GetSVELocations(intCluster);
                            rptLocations.DataBind();
                            lblLocations.Visible = (rptLocations.Items.Count == 0);
                            foreach (RepeaterItem ri in rptLocations.Items)
                                ((ImageButton)ri.FindControl("btnDeleteLocation")).Attributes.Add("onclick", "return confirm('Are you sure you want to delete this location?');");

                            // Hosts
                            rptHosts.DataSource = oServer.GetSVEClusters(intCluster);
                            rptHosts.DataBind();
                            lblHosts.Visible = (rptHosts.Items.Count == 0);
                            btnAddHost.Enabled = (rptHosts.Items.Count < 8);
                            // Processor Pools
                            rptProcessors.DataSource = oSolaris.GetProcessorPools(intCluster, 0);
                            rptProcessors.DataBind();
                            lblProcessors.Visible = (rptProcessors.Items.Count == 0);
                            foreach (RepeaterItem ri in rptProcessors.Items)
                            {
                                Label lblStatus = (Label)ri.FindControl("lblStatus");
                                int intID = Int32.Parse(lblStatus.Text);
                                // Calculate Status
                                double dblUse = 0.00;
                                DataSet dsGuest = oSolaris.GetSVEGuestsByPool(intID);
                                foreach (DataRow drGuest in dsGuest.Tables[0].Rows)
                                {
                                    dblUse += double.Parse(drGuest["cpu"].ToString());
                                }
                                double dblTotal = double.Parse(oSolaris.GetProcessorPool(intID, "critical"));
                                double dblAvailable = dblTotal - dblUse;
                                double dblProgress = (dblAvailable / dblTotal) * 100.00;
                                lblStatus.Text = dblProgress.ToString("0") + "% available" + oServiceRequest.GetStatusBar(dblProgress, "100", "6", false);
                            }

                            if (chkClusterTrunking.Checked == false)
                            {
                                // IP Subnets
                                rptSubnets.DataSource = oIPAddresses.GetNetworksSVE(intCluster, intClass);
                                rptSubnets.DataBind();
                                lblSubnets.Visible = (rptSubnets.Items.Count == 0);
                                foreach (RepeaterItem ri in rptSubnets.Items)
                                {
                                    CheckBox chkSelected = (CheckBox)ri.FindControl("chkSelected");
                                    Label lblMin4 = (Label)ri.FindControl("lblMin4");
                                    Label lblMax4 = (Label)ri.FindControl("lblMax4");
                                    Label lblInUse = (Label)ri.FindControl("lblInUse");
                                    // Calculate Status
                                    double dblMin = double.Parse(lblMin4.Text);
                                    double dblMax = double.Parse(lblMax4.Text);
                                    double dblUse = double.Parse(lblInUse.Text);
                                    double dblTotal = (dblMax - dblMin + 1.00);
                                    double dblAvailable = dblTotal - dblUse;
                                    double dblProgress = 0.00;
                                    if (dblAvailable > 0.00)
                                        dblProgress = (dblAvailable / dblTotal) * 100.00;
                                    lblInUse.Text = dblAvailable.ToString("0") + " of " + dblTotal.ToString("0") + " are available " + oServiceRequest.GetStatusBar(dblProgress, "100", "6", false);
                                }
                            }
                            // Mnemonics
                            rptMnemonics.DataSource = oSolaris.GetSVEMnemonics(intCluster, 0);
                            rptMnemonics.DataBind();
                            lblMnemonics.Visible = (rptMnemonics.Items.Count == 0);
                            // Guests
                            rptGuests.DataSource = oSolaris.GetSVEGuests(intCluster);
                            rptGuests.DataBind();
                            lblGuests.Visible = (rptGuests.Items.Count == 0);
                            //foreach (RepeaterItem ri in rptGuests.Items)
                            //{
                            //    Label lblIP = (Label)ri.FindControl("lblIP");
                            //    int intServerID = Int32.Parse(lblIP.Text);
                            //    int intClusterID = Int32.Parse(oServer.Get(intServerID, "clusterid"));
                            //    lblIP.Text = oServer.GetIPs(Int32.Parse(lblIP.Text), intClusterID, 0, 0, 0, dsnIP, ", ", "N / A");
                            //}
                        }
                        else
                        {
                            btnClusterAdd.Visible = true;
                            btnClusterAdd.Attributes.Add("onclick", "return ValidateText('" + txtClusterName.ClientID + "','Please enter a name for this cluster')" +
                                " && ValidateDropDown('" + ddlClusterClass.ClientID + "','Please select a class')" +
                                " && ValidateDropDown('" + ddlClusterResiliency.ClientID + "','Please select a resiliency')" +
                                " && ValidateRadioButtons('" + radClusterDB.ClientID + "','" + radClusterApp.ClientID + "','Please select the type of cluster')" +
                                " && ProcessButton(this) && LoadWait()" +
                                ";");
                            btnAddCluster.Enabled = false;
                        }
                        btnClusterCancel.Attributes.Add("onclick", "return ProcessButton(this) && LoadWait();");
                        strLocation = oLocation.LoadDDL("ddlState", "ddlCity", "ddlAddress", hdnLocation.ClientID, 0, true, "ddlCommon");
                        int intMenuTab = 0;
                        if (Request.QueryString["menu_tab"] != null && Request.QueryString["menu_tab"] != "")
                            intMenuTab = Int32.Parse(Request.QueryString["menu_tab"]);
                        Tab oTab = new Tab(hdnTab.ClientID, intMenuTab, "divMenu1", true, false);
                        oTab.AddTab("Clustered Hosts", "");
                        oTab.AddTab("Locations", "");
                        oTab.AddTab("Processor Pools", "");
                        if (chkClusterTrunking.Checked == false)
                            oTab.AddTab("IP Subnets", "");
                        oTab.AddTab("Associated Mnemonics", "");
                        oTab.AddTab("Guests", "");
                        strMenuTab1 = oTab.GetTabs();
                    }
                }
            }
            btnAddCluster.Attributes.Add("onclick", "return ProcessButton(this) && LoadWait();");
            btnAddHost.Attributes.Add("onclick", "return ProcessButton(this) && LoadWait();");
            btnAddMnemonic.Attributes.Add("onclick", "return ProcessButton(this) && LoadWait();");
            btnAddProcessor.Attributes.Add("onclick", "return ProcessButton(this) && LoadWait();");
            btnSaveSubnet.Attributes.Add("onclick", "return ProcessButton(this) && LoadWait();");
            Variables oVariable = new Variables(intEnvironment);
            txtHost.Attributes.Add("onkeyup", "return AJAXTextBoxGet(this,'200','195','" + divHost.ClientID + "','" + lstHost.ClientID + "','" + hdnHost.ClientID + "','" + oVariable.URL() + "/frame/ajax/ajax_servernames.aspx',2);");
            lstHost.Attributes.Add("ondblclick", "AJAXClickRow();");
            txtMnemonic.Attributes.Add("onkeyup", "return AJAXTextBoxGet(this,'500','195','" + divMnemonic.ClientID + "','" + lstMnemonic.ClientID + "','" + hdnMnemonic.ClientID + "','" + oVariable.URL() + "/frame/ajax/ajax_mnemonics.aspx',2);");
            lstMnemonic.Attributes.Add("ondblclick", "AJAXClickRow();");
            btnMoveGuest.Attributes.Add("onclick", "return ValidateStringItems('" + hdnMove.ClientID + "','Please select at least one guest to move') && ProcessButton(this) && LoadWait();");
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

        
        protected void CheckThresholds(int _clusterid, Label _status, Label _lblIPs, Label _lblPools)
        {
            string strError = "";
            string strCritical = "";
            string strWarning = "";
            string strMessage = "";
            string strAction = "";

            // **************************************************************
            // Threshold - Processor Pools
            // **************************************************************
            strAction = "<p><a href=\"javascript:void(0);\" onclick=\"ShowHideDiv2('divProcessor_" + _clusterid.ToString() + "');\">Click Here for Recommended Action(s)</a><div id=\"divProcessor_" + _clusterid.ToString() + "\" style=\"display:none\"><br/>";
            strAction += "<br/><b><u>ACTION # 1:</u> Increase the threshold settings for the processor pool</b><br/>";
            strAction += "<br/><b><u>ACTION # 2:</u> Add more proceesors to the host(s) and update the thresholds to reflect the change</b><br/>";
            strAction += "<br/><b><u>ACTION # 3:</u> Add an additional host to the cluster (if possible) and update the thresholds to reflect the change</b><br/>";
            strAction += "</div></p>";
            double dblPoolTotal = 0.00;
            double dblPoolAvailable = 0.00;
            DataSet dsPool = oSolaris.GetProcessorPools(_clusterid, 0);
            foreach (DataRow drPool in dsPool.Tables[0].Rows)
            {
                int intProcessorID = Int32.Parse(drPool["id"].ToString());
                // Calculate Status
                double dblUse = 0.00;
                DataSet dsGuest = oSolaris.GetSVEGuestsByPool(intProcessorID);
                foreach (DataRow drGuest in dsGuest.Tables[0].Rows)
                    dblUse += double.Parse(drGuest["cpu"].ToString());
                double dblTotal = double.Parse(oSolaris.GetProcessorPool(intProcessorID, "critical"));
                double dblWarning = double.Parse(oSolaris.GetProcessorPool(intProcessorID, "warning"));
                double dblError = double.Parse(oSolaris.GetProcessorPool(intProcessorID, "error"));
                dblPoolAvailable += dblTotal - dblUse;
                dblPoolTotal += dblTotal;

                if (dblUse >= dblError)
                {
                    strMessage = "ERROR Threshold: The current number of CPUs (" + dblUse.ToString("0") + ") has exceeded the ERROR threshold value of " + dblError.ToString("0") + " CPUs";
                    if (_lblIPs != null)
                        strError += "- " + strMessage + Environment.NewLine;
                    else
                        _status.Text += oFunction.BuildBox("/images/ico_error.gif", "Processor Pool: " + oSolaris.GetProcessorPool(intProcessorID, "name"), strMessage + strAction, "box_red header");
                }
                else if (dblUse < dblError && dblUse >= dblTotal)
                {
                    strMessage = "CRITICAL Threshold: The current number of CPUs (" + dblUse.ToString("0") + ") has exceeded its CRITICAL threshold and is now approaching the ERROR threshold value of " + dblError.ToString("0") + " CPUs";
                    if (_lblIPs != null)
                        strCritical += "- " + strMessage + Environment.NewLine;
                    else
                        _status.Text += oFunction.BuildBox("/images/bigWarning.gif", "Processor Pool: " + oSolaris.GetProcessorPool(intProcessorID, "name"), strMessage + strAction, "box_red header");
                }
                else if (dblUse < dblTotal && dblUse >= dblWarning)
                {
                    strMessage = "Warning Threshold: The current number of CPUs (" + dblUse.ToString("0") + ") is approaching the CRITICAL threshold value of " + dblTotal.ToString("0") + " CPUs";
                    if (_lblIPs != null)
                        strWarning += "- " + strMessage + Environment.NewLine;
                    else
                        _status.Text += oFunction.BuildBox("/images/hugeAlert.gif", "Processor Pool: " + oSolaris.GetProcessorPool(intProcessorID, "name"), strMessage + strAction, "box_yellow header");
                }
            }
            if (_lblPools != null)
            {
                double dblPoolProgress = 0.00;
                if (dblPoolTotal > 0.00)
                    dblPoolProgress = (dblPoolAvailable / dblPoolTotal) * 100.00;
                _lblPools.Text = dblPoolProgress.ToString("0") + "% available" + oServiceRequest.GetStatusBar(dblPoolProgress, "100", "6", false);
            }


            if (oSolaris.GetSVECluster(_clusterid, "trunking") != "1")
            {
                // **************************************************************
                // Threshold - IP Ranges
                // **************************************************************
                strAction = "<p><a href=\"javascript:void(0);\" onclick=\"ShowHideDiv2('divIPRange_" + _clusterid.ToString() + "');\">Click Here for Recommended Action(s)</a><div id=\"divIPRange_" + _clusterid.ToString() + "\" style=\"display:none\"><br/>";
                strAction += "<br/><b><u>ACTION # 1:</u> Request a new IP RANGE be given from network engineering</b><br/>";
                strAction += "<br/> - Send an email to IPAddress@pnc.com (and copy a ClearView administrator)";
                strAction += "<br/> - Include in the email all of the IP ranges you have been given for the subnet";
                strAction += "<br/> - Request that either an additional range be given, or a new subnet be created";
                strAction += "<br/> - Contact the ClearView administrator after you receive a response from the IPAddress mailbox";
                strAction += "</div></p>";
                double dblIPTotal = 0.00;
                double dblIPAvailable = 0.00;
                int intIPClass = 0;
                if (Int32.TryParse(oSolaris.GetSVECluster(_clusterid, "classid"), out intIPClass) == true)
                {
                    DataSet dsIP = oIPAddresses.GetNetworksSVE(_clusterid, intIPClass);
                    foreach (DataRow drIP in dsIP.Tables[0].Rows)
                    {
                        if (drIP["selected"].ToString() == "1")
                        {
                            double dblMin = double.Parse(drIP["min4"].ToString());
                            double dblMax = double.Parse(drIP["max4"].ToString());
                            double dblUse = double.Parse(drIP["inuse"].ToString());
                            // Calculate Status
                            double dblTotal = (dblMax - dblMin + 1.00);
                            dblIPAvailable += dblTotal - dblUse;
                            dblIPTotal += dblTotal;
                        }
                    }
                    if (_lblIPs != null)
                    {
                        double dblIPProgress = 0.00;
                        if (dblIPTotal > 0.00)
                            dblIPProgress = (dblIPAvailable / dblIPTotal) * 100.00;
                        _lblIPs.Text = dblIPProgress.ToString("0") + "% available" + oServiceRequest.GetStatusBar(dblIPProgress, "100", "6", false);
                    }
                }

                if (dblIPAvailable <= 0.00)
                {
                    strMessage = "ERROR Threshold: There are no IP Addresses available in any of the selected ranges";
                    if (_lblIPs != null)
                        strError += "- " + strMessage + Environment.NewLine;
                    else
                        _status.Text += oFunction.BuildBox("/images/ico_error.gif", "IP Address Ranges", strMessage + strAction, "box_red header");
                }
                else if (dblIPAvailable <= 10.00)
                {
                    strMessage = "CRITICAL Threshold: Less than 10 IP Addresses are available for the selected ranges (only " + dblIPAvailable.ToString("0") + " addresses available)";
                    if (_lblIPs != null)
                        strCritical += "- " + strMessage + Environment.NewLine;
                    else
                        _status.Text += oFunction.BuildBox("/images/bigWarning.gif", "IP Address Ranges", strMessage + strAction, "box_red header");
                }
                else if (dblIPAvailable <= 20.00)
                {
                    strMessage = "Warning Threshold: Less than 20 IP Addresses are available for the selected ranges  (only " + dblIPAvailable.ToString("0") + " addresses available)";
                    if (_lblIPs != null)
                        strWarning += "- " + strMessage + Environment.NewLine;
                    else
                        _status.Text += oFunction.BuildBox("/images/hugeAlert.gif", "IP Address Ranges", strMessage + strAction, "box_yellow header");
                }


                // **************************************************************
                // Threshold - IP Subnets
                // **************************************************************
                strAction = "<p><a href=\"javascript:void(0);\" onclick=\"ShowHideDiv2('divIPSubnet_" + _clusterid.ToString() + "');\">Click Here for Recommended Action(s)</a><div id=\"divIPSubnet_" + _clusterid.ToString() + "\" style=\"display:none\"><br/>";
                strAction += "<br/><b><u>ACTION # 1:</u>Request a new SUBNET be created by network engineering</b><br/>";
                strAction += "<br/> - Send an email to IPAddress@pnc.com (and copy a ClearView administrator)";
                strAction += "<br/> - Include in the email at least one of the existing IP ranges you have already been given";
                strAction += "<br/> - Request that a new subnet be created and that it mirror the configuration of your existing range(s)";
                strAction += "</div></p>";
                int intIPClass2 = 0;
                if (Int32.TryParse(oSolaris.GetSVECluster(_clusterid, "classid"), out intIPClass2) == true)
                {
                    double dblSubnetTotal = 0.00;
                    double dblSubnetAvailable = 0.00;
                    int intTotal = 0;
                    string strSubnet = "";
                    DataSet dsIP = oIPAddresses.GetNetworksSVEs(_clusterid, intIPClass2);
                    foreach (DataRow drIP in dsIP.Tables[0].Rows)
                    {
                        double dblInUse = double.Parse(drIP["inuse"].ToString());
                        dblSubnetTotal += 235.00;
                        int intAdd1 = Int32.Parse(drIP["add1"].ToString());
                        int intAdd2 = Int32.Parse(drIP["add2"].ToString());
                        int intAdd3 = Int32.Parse(drIP["add3"].ToString());
                        double dblTotal = 0.00;
                        DataSet dsRange = oIPAddresses.GetNetworksRanges(intAdd1, intAdd2, intAdd3);
                        foreach (DataRow drRange in dsRange.Tables[0].Rows)
                        {
                            double dblMin = double.Parse(drRange["min4"].ToString());
                            double dblMax = double.Parse(drRange["max4"].ToString());
                            dblSubnetAvailable += (dblMax - dblMin + 1.00);
                        }
                        if (dblSubnetAvailable <= 60.00)
                            strSubnet = intAdd1.ToString() + "." + intAdd2.ToString() + "." + intAdd3.ToString() + ".x";
                    }

                    if (dblSubnetAvailable <= 0.00)
                    {
                        strMessage = "ERROR Threshold: There are no available IP Addresses in the selected subnet(s)";
                        if (_lblIPs != null)
                            strError += "- " + strMessage + Environment.NewLine;
                        else
                            _status.Text += oFunction.BuildBox("/images/ico_error.gif", "IP Subnet: " + strSubnet, strMessage + strAction, "box_red header");
                    }
                    else if (dblSubnetAvailable <= 30.00)
                    {
                        strMessage = "CRITICAL Threshold: There are less than 30 available IP Addresses in the selected subnet(s) (only " + dblSubnetAvailable.ToString("0") + " addresses available)";
                        if (_lblIPs != null)
                            strCritical += "- " + strMessage + Environment.NewLine;
                        else
                            _status.Text += oFunction.BuildBox("/images/bigWarning.gif", "IP Subnet: " + strSubnet, strMessage + strAction, "box_red header");
                    }
                    else if (dblSubnetAvailable <= 60.00)
                    {
                        strMessage = "Warning Threshold: There are less than 60 available IP Addresses in the selected subnet(s) (only " + dblSubnetAvailable.ToString("0") + " addresses available)";
                        if (_lblIPs != null)
                            strWarning += "- " + strMessage + Environment.NewLine;
                        else
                            _status.Text += oFunction.BuildBox("/images/hugeAlert.gif", "IP Subnet: " + strSubnet, strMessage + strAction, "box_yellow header");
                    }
                    //if (_lblIPs != null)
                    //{
                    //    double dblIPProgress = 0.00;
                    //    if (dblIPTotal > 0.00)
                    //        dblIPProgress = (dblIPAvailable / dblIPTotal) * 100.00;
                    //    _lblIPs.Text = dblIPProgress.ToString("0") + "% available" + oServiceRequest.GetStatusBar(dblIPProgress, "100", "6", false);
                    //}
                }

                if (_lblIPs != null)
                {
                    if (strError != "")
                        _status.Text += "<img src=\"/images/error.gif\" border=\"0\" align=\"absmiddle\" title=\"" + strError + "\"/>";
                    else if (strCritical != "")
                        _status.Text += "<img src=\"/images/red_alert.gif\" border=\"0\" align=\"absmiddle\" title=\"" + strCritical + "\"/>";
                    else if (strWarning != "")
                        _status.Text += "<img src=\"/images/alert.gif\" border=\"0\" align=\"absmiddle\" title=\"" + strWarning + "\"/>";
                }
            }
            else
                if (_lblIPs != null)
                    _lblIPs.Text = "N / A";
        }
        protected void btnFilter_Click(Object Sender, EventArgs e)
        {
            int intAvailable = Int32.Parse(radAvailable.SelectedItem.Value);
            Response.Cookies["sve_available"].Value = intAvailable.ToString();
            int intMnemonics = (chkMnemonics.Checked ? 1 : 0);
            Response.Cookies["sve_mnemonics"].Value = intMnemonics.ToString();
            int intTrunking = Int32.Parse(radTrunk.SelectedItem.Value);
            Response.Cookies["sve_trunk"].Value = intTrunking.ToString();
            rptClusters.DataSource = oSolaris.GetSVEClusters(intAvailable, intMnemonics, intTrunking, 0);
            rptClusters.DataBind();
            lblClusters.Visible = (rptClusters.Items.Count == 0);
            foreach (RepeaterItem ri in rptClusters.Items)
            {
                int intID = 0;
                Label lblGuests = (Label)ri.FindControl("lblGuests");
                Label lblHosts = (Label)ri.FindControl("lblHosts");
                Label lblIPs = (Label)ri.FindControl("lblIPs");
                Label lblPools = (Label)ri.FindControl("lblPools");
                Label lblStatus = (Label)ri.FindControl("lblStatus");

                if (Int32.TryParse(lblGuests.Text, out intID) == true)
                {
                    lblGuests.Text = oSolaris.GetSVEGuests(intID).Tables[0].Rows.Count.ToString();
                    lblHosts.Text = oServer.GetSVEClusters(intID).Tables[0].Rows.Count.ToString();
                    CheckThresholds(intID, lblStatus, lblIPs, lblPools);
                }
            }
            btnFilterClear.Visible = true;
        }
        protected void btnFilterClear_Click(Object Sender, EventArgs e)
        {
            Response.Cookies["sve_available"].Value = "-1";
            Response.Cookies["sve_mnemonics"].Value = "0";
            Response.Cookies["sve_trunk"].Value = "-1";
            Response.Redirect(FormURL(""));
        }
        protected void btnAddCluster_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(FormURL("cluster=0"));
        }
        protected void btnClusterAdd_Click(Object Sender, EventArgs e)
        {
            oSolaris.AddSVECluster(txtClusterName.Text, (radClusterDB.Checked ? 1 : 0), Int32.Parse(ddlClusterClass.SelectedItem.Value), Int32.Parse(ddlClusterResiliency.SelectedItem.Value), 1, (chkClusterAvailable.Checked ? 1 : 0), txtClusterComments.Text, (chkClusterStorage.Checked ? 1 : 0), (chkClusterTrunking.Checked ? 1 : 0), 1);
            Response.Redirect(FormURL("save=true"));
        }
        protected void btnClusterUpdate_Click(Object Sender, EventArgs e)
        {
            oSolaris.UpdateSVECluster(intCluster, txtClusterName.Text, (radClusterDB.Checked ? 1 : 0), Int32.Parse(ddlClusterClass.SelectedItem.Value), Int32.Parse(ddlClusterResiliency.SelectedItem.Value), 1, (chkClusterAvailable.Checked ? 1 : 0), txtClusterComments.Text, (chkClusterStorage.Checked ? 1 : 0), (chkClusterTrunking.Checked ? 1 : 0), 1);
            Response.Redirect(FormURL("cluster=" + intCluster.ToString() + "&save=true"));
        }
        protected void btnClusterDelete_Click(Object Sender, EventArgs e)
        {
            oSolaris.DeleteSVECluster(intCluster);
            Response.Redirect(FormURL("save=true"));
        }
        protected void btnClusterCancel_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(FormURL(""));
        }
        protected void btnAddHost_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(FormURL("cluster=" + intCluster.ToString() + "&host=0"));
        }
        protected void btnHostAdd_Click(Object Sender, EventArgs e)
        {
            int intServerID = 0;
            Int32.TryParse(Request.Form[hdnHost.UniqueID], out intServerID);
            if (intServerID > 0)
                oServer.UpdateSVECluster(intServerID, intCluster);
            Response.Redirect(FormURL("cluster=" + intCluster.ToString() + "&menu_tab=1" + "&save=true"));
        }
        protected void btnHostUpdate_Click(Object Sender, EventArgs e)
        {
            int intServerID = 0;
            Int32.TryParse(Request.Form[hdnHost.UniqueID], out intServerID);
            if (intServerID > 0 && intHost != intServerID) 
            {
                oServer.UpdateSVECluster(intHost, 0);
                oServer.UpdateSVECluster(intServerID, intCluster);
            }
            int intSchedule = 0;
            Int32.TryParse(Request.Form[hdnSchedule.UniqueID], out intSchedule);
            if (intSchedule > 0)
                oServer.UpdateTSMSchedule(intServerID, intSchedule);
            Response.Redirect(FormURL("cluster=" + intCluster.ToString() + "&host=" + intHost.ToString() + "&save=true"));
        }
        protected void btnHostDelete_Click(Object Sender, EventArgs e)
        {
            oServer.UpdateSVECluster(intHost, 0);
            Response.Redirect(FormURL("cluster=" + intCluster.ToString() + "&menu_tab=1" + "&delete=true"));
        }
        protected void btnHostCancel_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(FormURL("cluster=" + intCluster.ToString() + "&menu_tab=1"));
        }
        protected void btnMoveGuest_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(FormURL("cluster=" + intCluster.ToString() + "&guests=" + oFunction.encryptQueryString(Request.Form[hdnMove.UniqueID])));
        }
        protected void btnLocation_Click(Object Sender, EventArgs e)
        {
            oSolaris.AddSVELocation(intCluster, Int32.Parse(Request.Form[hdnLocation.UniqueID]));
            Response.Redirect(FormURL("cluster=" + intCluster.ToString() + "&menu_tab=2"));
        }
        protected void btnDeleteLocation_Click(Object Sender, ImageClickEventArgs e)
        {
            ImageButton oButton = (ImageButton)Sender;
            oSolaris.DeleteSVELocation(Int32.Parse(oButton.CommandArgument));
            Response.Redirect(FormURL("cluster=" + intCluster.ToString() + "&menu_tab=2"));
        }
        protected void btnAddProcessor_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(FormURL("cluster=" + intCluster.ToString() + "&processor=0"));
        }
        protected void btnProcessorAdd_Click(Object Sender, EventArgs e)
        {
            oSolaris.AddProcessorPool(intCluster, txtProcessorName.Text, txtProcessorDescription.Text, Int32.Parse(txtProcessorLow.Text), Int32.Parse(txtProcessorHigh.Text), Int32.Parse(txtProcessorWarning.Text), Int32.Parse(txtProcessorCritical.Text), Int32.Parse(txtProcessorError.Text), (chkProcessorEnabled.Checked ? 1 : 0));
            Response.Redirect(FormURL("cluster=" + intCluster.ToString() + "&menu_tab=3" + "&save=true"));
        }
        protected void btnProcessorUpdate_Click(Object Sender, EventArgs e)
        {
            oSolaris.UpdateProcessorPool(intProcessor, intCluster, txtProcessorName.Text, txtProcessorDescription.Text, Int32.Parse(txtProcessorLow.Text), Int32.Parse(txtProcessorHigh.Text), Int32.Parse(txtProcessorWarning.Text), Int32.Parse(txtProcessorCritical.Text), Int32.Parse(txtProcessorError.Text), (chkProcessorEnabled.Checked ? 1 : 0));
            Response.Redirect(FormURL("cluster=" + intCluster.ToString() + "&processor=" + intProcessor.ToString() + "&save=true"));
        }
        protected void btnProcessorDelete_Click(Object Sender, EventArgs e)
        {
            oSolaris.DeleteProcessorPool(intProcessor);
            Response.Redirect(FormURL("cluster=" + intCluster.ToString() + "&menu_tab=3" + "&delete=true"));
        }
        protected void btnProcessorCancel_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(FormURL("cluster=" + intCluster.ToString() + "&menu_tab=3"));
        }
        protected void btnSaveSubnet_Click(Object Sender, EventArgs e)
        {
            oSolaris.DeleteSVENetwork(intCluster);
            foreach (RepeaterItem ri in rptSubnets.Items)
            {
                CheckBox chkSelected = (CheckBox)ri.FindControl("chkSelected");
                if (chkSelected.Checked == true)
                {
                    Label lblAdd1 = (Label)ri.FindControl("lblAdd1");
                    Label lblAdd2 = (Label)ri.FindControl("lblAdd2");
                    Label lblAdd3 = (Label)ri.FindControl("lblAdd3");
                    Label lblMin4 = (Label)ri.FindControl("lblMin4");
                    Label lblMax4 = (Label)ri.FindControl("lblMax4");
                    DataSet ds = oIPAddresses.GetNetworksRange(Int32.Parse(lblAdd1.Text), Int32.Parse(lblAdd2.Text), Int32.Parse(lblAdd3.Text), Int32.Parse(lblMin4.Text), Int32.Parse(lblMax4.Text));
                    foreach (DataRow dr in ds.Tables[0].Rows)
                        oSolaris.AddSVENetwork(intCluster, Int32.Parse(dr["id"].ToString()));
                }
            }
            Response.Redirect(FormURL("cluster=" + intCluster.ToString() + "&menu_tab=4" + "&save=true"));
        }
        protected void btnAddMnemonic_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(FormURL("cluster=" + intCluster.ToString() + "&mnemonic=0"));
        }
        protected void btnMnemonicAdd_Click(Object Sender, EventArgs e)
        {
            int intMnemonicID = 0;
            Int32.TryParse(Request.Form[hdnMnemonic.UniqueID], out intMnemonicID);
            if (intMnemonicID > 0)
                oSolaris.AddSVEMnemonic(intCluster, intMnemonicID, (chkMnemonicEnabled.Checked ? 1 : 0));
            Response.Redirect(FormURL("cluster=" + intCluster.ToString() + "&menu_tab=5" + "&save=true"));
        }
        protected void btnMnemonicUpdate_Click(Object Sender, EventArgs e)
        {
            int intMnemonicID = 0;
            Int32.TryParse(Request.Form[hdnMnemonic.UniqueID], out intMnemonicID);
            oSolaris.UpdateSVEMnemonic(intMnemonic, intCluster, intMnemonicID, (chkMnemonicEnabled.Checked ? 1 : 0));
            Response.Redirect(FormURL("cluster=" + intCluster.ToString() + "&mnemonic=" + intMnemonic.ToString() + "&save=true"));
        }
        protected void btnMnemonicDelete_Click(Object Sender, EventArgs e)
        {
            oSolaris.DeleteSVEMnemonic(intMnemonic);
            Response.Redirect(FormURL("cluster=" + intCluster.ToString() + "&menu_tab=5" + "&delete=true"));
        }
        protected void btnMnemonicCancel_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(FormURL("cluster=" + intCluster.ToString() + "&menu_tab=5"));
        }
        protected void btnGuestMove_Click(Object Sender, EventArgs e)
        {
            strGuests = oFunction.decryptQueryString(Request.QueryString["guests"]);
            while (strGuests != "")
            {
                string strField = strGuests.Substring(0, strGuests.IndexOf("&"));
                strGuests = strGuests.Substring(strGuests.IndexOf("&") + 1);
                string strFlag = strField.Substring(strField.IndexOf("_") + 1);
                strField = strField.Substring(0, strField.IndexOf("_"));
                if (strFlag == "1")
                {
                    int intServerID = Int32.Parse(strField);
                    int intClusterID = 0;
                    Int32.TryParse(oServer.Get(intServerID, "clusterid"), out intClusterID);
                    if (intClusterID > 0)
                    {
                        DataSet dsServer = oServer.GetClusters(intClusterID);
                        foreach (DataRow drServer in dsServer.Tables[0].Rows)
                        {
                            if (Int32.TryParse(drServer["id"].ToString(), out intServerID))
                                oSolaris.UpdateSVEGuestCluster(intServerID, intCluster);
                        }
                    }
                    else
                        oSolaris.UpdateSVEGuestCluster(intServerID, intCluster);
                }
            }
            Response.Redirect(FormURL("cluster=" + intCluster.ToString() + "&menu_tab=6" + "&save=true"));
        }
        protected void btnGuestUpdate_Click(Object Sender, EventArgs e)
        {
            oSolaris.UpdateSVEGuest(intGuest, Int32.Parse(ddlProcessorPool.SelectedItem.Value), double.Parse(txtGuestAllocated.Text), Int32.Parse(txtGuestCPU.Text), Int32.Parse(txtGuestRAM.Text));
            Response.Redirect(FormURL("cluster=" + intCluster.ToString() + "&menu_tab=6"));
        }
        protected void btnGuestCancel_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(FormURL("cluster=" + intCluster.ToString() + "&menu_tab=6"));
        }
    }
}