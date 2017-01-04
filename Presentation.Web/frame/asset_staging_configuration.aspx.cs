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
using NCC.ClearView.Presentation.Web.Custom;
using Microsoft.ApplicationBlocks.Data;

namespace NCC.ClearView.Presentation.Web
{
    public partial class asset_staging_configuration : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected Users oUser;
        protected Servers oServer;
        protected Asset oAsset;
        protected AssetOrder oAssetOrder;
        protected Forecast oForecast;
        protected Platforms oPlatform;
        protected Types oType;
        protected Models oModel;
        protected ModelsProperties oModelsProperties;
        protected IPAddresses oIPAddresses;
        protected Functions oFunction;
        protected OperatingSystems oOperatingSystem;
        protected ServicePacks oServicePack;
        protected Classes oClass;
        protected Environments oEnvironment;
        protected Locations oLocation;
        protected RoomsNew oRoom;
        protected RacksNew oRack;
        protected Resiliency oResiliency;
        protected Solaris oSolaris;
        protected int intProfile = 0;
        protected int intApplication = 0;
        protected int intAsset = 0;
        protected string strLocation = "";
        protected string strMenuTab1 = "";
        protected int intRequiredHBACount = 2;
        protected void Page_Load(object sender, EventArgs e)
        {
            AuthenticateUser();

            oUser = new Users(intProfile, dsn);
            oServer = new Servers(intProfile, dsn);
            oAsset = new Asset(intProfile, dsnAsset,dsn);
            oAssetOrder = new AssetOrder(intProfile, dsn, dsnAsset, intEnvironment);
            oForecast = new Forecast(intProfile, dsn);
            oPlatform = new Platforms(intProfile, dsn);
            oType = new Types(intProfile, dsn);
            oModel = new Models(intProfile, dsn);
            oModelsProperties = new ModelsProperties(intProfile, dsn);
            oIPAddresses = new IPAddresses(intProfile, dsnIP, dsn);
            oFunction = new Functions(intProfile, dsn, intEnvironment);
            oOperatingSystem = new OperatingSystems(intProfile, dsn);
            oServicePack = new ServicePacks(intProfile, dsn);
            oClass = new Classes(intProfile, dsn);
            oEnvironment = new Environments(intProfile, dsn);
            oLocation = new Locations(intProfile, dsn);
            oSolaris = new Solaris(intProfile, dsn);
            oRoom = new RoomsNew(intProfile,dsn);
            oRack = new RacksNew(intProfile, dsn);
            oResiliency = new Resiliency(intProfile, dsn);
            if (Request.Cookies["profileid"] != null && Request.Cookies["profileid"].Value != "")
                intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            if (Request.QueryString["applicationid"] != null && Request.QueryString["applicationid"] != "")
                intApplication = Int32.Parse(Request.QueryString["applicationid"]);
            if (Request.Cookies["application"] != null && Request.Cookies["application"].Value != "")
                intApplication = Int32.Parse(Request.Cookies["application"].Value);

            if ((Request.QueryString["assetid"] != null && Request.QueryString["assetid"] != "") &&
               (Request.QueryString["orderid"] != null && Request.QueryString["orderid"] != "") )
            {
                hdnAssetId.Value = oFunction.decryptQueryString(Request.QueryString["assetid"]);
                hdnOrderId.Value = oFunction.decryptQueryString(Request.QueryString["orderid"]);
                //hdnAssetId.Value = "11049";  //Balde"24188";
                if (!IsPostBack)
                {
                    LoadList();
                    LoadAssetInformation();
                    populateLocations();
                }

                ddlClass.Attributes.Add("onchange", "PopulateEnvironments('" + ddlClass.ClientID + "','" + ddlEnvironment.ClientID + "',0);");
                ddlEnvironment.Attributes.Add("onchange", "UpdateDropDownHidden('" + ddlEnvironment.ClientID + "','" + hdnEnvironment.ClientID + "');");


                btnAddWWPortName.Attributes.Add("onclick", "return ValidateText('" + txtWWPortName.ClientID + "','Please enter a World Wide Port name') && ProcessControlButton()" +
                                ";");




            }
            else
            {
                pnlAllow.Visible = false;
                pnlDenied.Visible = true;
            }
        }
        private void LoadList()
        {
            //Class
            ddlClass.DataTextField = "name";
            ddlClass.DataValueField = "id";
            ddlClass.DataSource = oClass.Gets(1);
            ddlClass.DataBind();
            ddlClass.Items.Insert(0, new ListItem("-- SELECT --", "0"));

            //Enclosure
            ddlEnclosure.DataTextField = "name";
            ddlEnclosure.DataValueField = "id";
            ddlEnclosure.DataSource =  oAsset.GetEnclosures((int)AssetStatus.InUse) ;
            ddlEnclosure.DataBind();
            ddlEnclosure.Items.Insert(0, new ListItem("-- SELECT --", "0"));

            //Build Network
            ddlBuildNetwork.DataValueField = "id";
            ddlBuildNetwork.DataTextField = "name";
            ddlBuildNetwork.DataSource = oSolaris.GetBuildNetworks(1);
            ddlBuildNetwork.DataBind();
            ddlBuildNetwork.Items.Insert(0, new ListItem("-- SELECT --", "0"));

            //Resiliency
            ddlResiliency.DataValueField = "id";
            ddlResiliency.DataTextField = "name";
            ddlResiliency.DataSource = oResiliency.Gets(1);
            ddlResiliency.DataBind();
            ddlResiliency.Items.Insert(0, new ListItem("-- SELECT --", "0"));

            //Operating System Group
            ddlOperatingSystemGroup.DataValueField = "id";
            ddlOperatingSystemGroup.DataTextField = "name";
            ddlOperatingSystemGroup.DataSource = oOperatingSystem.GetGroups(1);
            ddlOperatingSystemGroup.DataBind();
            ddlOperatingSystemGroup.Items.Insert(0, new ListItem("-- ANY --", "0"));
        }

        #region Load Asset Details
        
        private void LoadAssetInformation()
        {

            DataSet dsProcurementReq= oAssetOrder.Get(Int32.Parse(hdnOrderId.Value));
            if (dsProcurementReq.Tables[0].Rows.Count > 0)
            {
                DataRow drProcurementReq = dsProcurementReq.Tables[0].Rows[0];
                int intLocation = 0;
                Int32.TryParse(drProcurementReq["LocationId"].ToString(), out intLocation);
                string strLocationCommonName = (intLocation > 0 ? oLocation.GetAddress(intLocation,"commonname") : "");
                if (strLocationCommonName == "")
                {
                    lblIntendedLocation.Text = "Intended Location : " + drProcurementReq["Location"].ToString();
                }
                else
                {
                    lblIntendedLocation.Text = "Intended Location : " + strLocationCommonName;
                }

                lblIntendedRoom.Text = "Intended Room : " + drProcurementReq["Room"].ToString();
                lblIntendedZone.Text = "Intended Zone : " + drProcurementReq["Zone"].ToString();
                lblIntendedRack.Text = "Intended Rack : " + drProcurementReq["Rack"].ToString();
                lblIntendedRackPosition.Text = "Intended Rack Position : " + (drProcurementReq["RackPos"].ToString() == "" ? "N / A" : drProcurementReq["RackPos"].ToString());

                lblIntendedResiliency.Text = "Intended Resiliency : " + (drProcurementReq["Resiliency"].ToString() == "" ? "N / A" : drProcurementReq["Resiliency"].ToString());
                lblIntendedClass.Text = "Intended Class : " + drProcurementReq["Class"].ToString();
                lblIntendedEnvironment.Text = "Intended Environment : " + drProcurementReq["Environment"].ToString();

                lblIntendedEnclosure.Text = "Intended Enclosure : " + (drProcurementReq["Enclosure"].ToString() == "" ? "N / A" : drProcurementReq["Enclosure"].ToString());
                lblIntendedSlot.Text = "Intended Enclosure Slot : " + (drProcurementReq["Enclosure"].ToString() == "" ? "N / A" : drProcurementReq["EnclosureSlot"].ToString());

                lblIntendedOperatingSystemGroup.Text = "Intended Operating System Group : " + (drProcurementReq["OperatingSystemGroup"].ToString() == "" ? "Not Specified" : drProcurementReq["OperatingSystemGroup"].ToString());
            }


            DataSet dsAsset = oAsset.GetAssetsAll(Int32.Parse(hdnAssetId.Value));

            if (dsAsset.Tables[0].Rows.Count == 1)
            {   DataRow  drAsset =dsAsset.Tables[0].Rows[0];
                intAsset = Int32.Parse(drAsset["AssetId"].ToString());
                lblAssetID.Text = "#" + intAsset.ToString();

                string strSerial = drAsset["AssetSerial"].ToString();
                string strAsset = drAsset["AssetTag"].ToString();
                string strHeader = (strSerial.Length > 15 ? strSerial.Substring(0, 15) + "..." : strSerial);

                lblHeader.Text = "&quot;" + strHeader.ToUpper() + "&quot;";
                Master.Page.Title = "Asset Staging and Configuration (" + strHeader + ")";
                lblHeaderSub.Text = "Stage and Configure a asset...";

                int intModel = Int32.Parse(drAsset["AssetModelId"].ToString());
                int intLocation = 0;
                Int32.TryParse(drAsset["LocationId"].ToString(), out intLocation);
                panSwitches.Visible = (oModelsProperties.IsConfigureSwitches(intModel) && oLocation.IsOffsite(intLocation) == false);
                panSwitchesPending.Visible = false;
                int intModelParent = Int32.Parse(oModelsProperties.Get(intModel, "modelid"));
                bool IsDell = oModelsProperties.IsDell(intModel);
                txtAssetSerial.Text = drAsset["AssetSerial"].ToString();
                txtAssetTag.Text = drAsset["AssetTag"].ToString();
                txtModel.Text = oModel.Get(intModelParent, "name");  
                txtModelProperty.Text = drAsset["ModelName"].ToString();
                hdnModelPropertyId.Value = drAsset["AssetModelId"].ToString();
                hdnAssetCategoryId.Value = drAsset["AssetCategoryId"].ToString();
              

                
                SetControlsForAssetCategory(IsDell);

                if (hdnAssetCategoryId.Value=="4") //Rack
                    txtDeviceName.Text = drAsset["Rack"].ToString();
                else
                    txtDeviceName.Text = drAsset["AssetName"].ToString();

               

                ddlClass.SelectedValue = (drAsset["ClassId"].ToString() != "" ? drAsset["ClassId"].ToString() : "0");
                if (drAsset["ClassId"].ToString() != "")
                {
                    ddlEnvironment.DataTextField = "name";
                    ddlEnvironment.DataValueField = "id";
                    ddlEnvironment.DataSource = oClass.GetEnvironment(Int32.Parse(drAsset["ClassId"].ToString()), 0);
                    ddlEnvironment.DataBind();
                    ddlEnvironment.Items.Insert(0, new ListItem("-- SELECT --", "0"));
                }

                hdnEnvironment.Value = (drAsset["EnvironmentId"].ToString() != "" ? drAsset["EnvironmentId"].ToString() : "0");
                ddlEnvironment.SelectedValue = (drAsset["EnvironmentId"].ToString() != "" ? drAsset["EnvironmentId"].ToString() : "0");

                //hdnLocation.Value = (drAsset["LocationId"].ToString() != "" ? drAsset["LocationId"].ToString() : "0");
                hdnZoneId.Value = (drAsset["ZoneId"].ToString() != "" ? drAsset["ZoneId"].ToString() : "0");
                hdnRackId.Value= (drAsset["RackId"].ToString() != "" ? drAsset["RackId"].ToString() : "0");
                txtRackPosition.Text = (drAsset["RackPosition"].ToString() != "" ? drAsset["RackPosition"].ToString() : "0");
                
                populateLocations();

                ddlEnclosure.SelectedValue = (drAsset["EnclosureId"].ToString() != "" ? drAsset["EnclosureId"].ToString() : "0");
                txtSlot.Text = (drAsset["Slot"].ToString() != "" ? drAsset["Slot"].ToString() : "0");
                chkSpare.Checked = (drAsset["Spare"].ToString() == "1" ? true : false);
                txtILO.Text = drAsset["ILO"].ToString();
                txtDummyName.Text = drAsset["DummyName"].ToString();
                txtVLAN.Text = (drAsset["vLAN"].ToString()!=""? drAsset["vLAN"].ToString():"0");
                ddlBuildNetwork.SelectedValue = (drAsset["BuildNetworkId"].ToString() != "" ? drAsset["BuildNetworkId"].ToString() : "0");
                txtOAIP.Text = (drAsset["OAIP"].ToString() != "" ? drAsset["OAIP"].ToString() : "");
                ddlResiliency.SelectedValue = (drAsset["resiliencyid"].ToString() != "" ? drAsset["resiliencyid"].ToString() : "0");
                ddlOperatingSystemGroup.SelectedValue = (drAsset["operatingsystemgroupid"].ToString() != "" ? drAsset["operatingsystemgroupid"].ToString() : "0");


               
                switch (hdnAssetCategoryId.Value)
                {
                    case "1":  //Physical Server
                        if (IsDell == false)
                            LoadWorldWidePortNames(intAsset);
                        break;
                    case "2": //Blade
                        if (IsDell == false)
                            LoadWorldWidePortNames(intAsset);
                        break;
                    case "3": //Enclosure
                        break;
                    case "4": //Rack
                        
                        break;
                    default:
                        break;
                }

                if (!IsPostBack && panSwitches.Visible == true)
                {
                    int intRackID = 0;
                    if (Int32.TryParse(drAsset["rackid"].ToString(), out intRackID) == true)
                    {
                        lblSwitchRack.Text = drAsset["rack"].ToString();
                        lblSwitchRack.ToolTip = drAsset["rackid"].ToString();
                        DataSet dsSwitches = oAsset.GetSwitchsByRack(intRackID, 1);
                        LoadSwitch(ddlNetwork1, dsSwitches, ddlNetwork1Blade, txtNetwork1FexID, ddlNetwork1Port, txtNetwork1Interface, lblNetwork1Interface, hdnNetwork1Blade, hdnNetwork1Port, SwitchPortType.Network, 1, intAsset);
                        LoadSwitch(ddlNetwork2, dsSwitches, ddlNetwork2Blade, txtNetwork2FexID, ddlNetwork2Port, txtNetwork2Interface, lblNetwork2Interface, hdnNetwork2Blade, hdnNetwork2Port, SwitchPortType.Network, 2, intAsset);
                        LoadSwitch(ddlBackup, dsSwitches, ddlBackupBlade, txtBackupFexID, ddlBackupPort, txtBackupInterface, lblBackupInterface, hdnBackupBlade, hdnBackupPort, SwitchPortType.Backup, 1, intAsset);
                        LoadSwitch(ddlRemote, dsSwitches, ddlRemoteBlade, txtRemoteFexID, ddlRemotePort, txtRemoteInterface, lblRemoteInterface, hdnRemoteBlade, hdnRemotePort, SwitchPortType.Remote, 1, intAsset);
                        LoadSwitch(ddlCluster1, dsSwitches, ddlCluster1Blade, txtCluster1FexID, ddlCluster1Port, txtCluster1Interface, lblCluster1Interface, hdnCluster1Blade, hdnCluster1Port, SwitchPortType.Cluster, 1, intAsset);
                        LoadSwitch(ddlCluster2, dsSwitches, ddlCluster2Blade, txtCluster2FexID, ddlCluster2Port, txtCluster2Interface, lblCluster2Interface, hdnCluster2Blade, hdnCluster2Port, SwitchPortType.Cluster, 2, intAsset);
                        LoadSwitch(ddlStorage1, dsSwitches, ddlStorage1Blade, null, ddlStorage1Port, txtStorage1Interface, null, hdnStorage1Blade, hdnStorage1Port, SwitchPortType.Storage, 1, intAsset);
                        LoadSwitch(ddlStorage2, dsSwitches, ddlStorage2Blade, null, ddlStorage2Port, txtStorage2Interface, null, hdnStorage2Blade, hdnStorage2Port, SwitchPortType.Storage, 2, intAsset);
                    }
                    else
                    {
                        panSwitches.Visible = false;
                        panSwitchesPending.Visible = (oModelsProperties.IsConfigureSwitches(intModel) && oLocation.IsOffsite(intLocation) == false);
                        btnSaveAndClose.Enabled = false;
                    }
                }
            }

        
        }

        private void LoadSwitch(DropDownList ddlSwitch, DataSet dsSwitch, DropDownList ddlBlade, TextBox txtFexID, DropDownList ddlPort, TextBox txtInterface, Label lblInterface, HiddenField hdnBlade, HiddenField hdnPort, SwitchPortType _type, int _nic, int _assetid)
        {
            ddlSwitch.DataTextField = "name";
            ddlSwitch.DataValueField = "id";
            ddlSwitch.DataSource = dsSwitch;
            ddlSwitch.DataBind();
            ddlSwitch.Items.Insert(0, new ListItem("-- SELECT --", "0"));
            if (txtFexID != null)
            {
                ddlSwitch.Attributes.Add("onchange", "PopulateSwitchs('" + ddlSwitch.ClientID + "','" + ddlBlade.ClientID + "','" + txtFexID.ClientID + "','" + ddlPort.ClientID + "','" + txtInterface.ClientID + "','" + lblInterface.ClientID + "','" + hdnBlade.ClientID + "');");
                txtFexID.Attributes.Add("onblur", "UpdateTextHidden('" + txtFexID.ClientID + "','" + hdnBlade.ClientID + "');UpdateNetworkInterface('" + txtFexID.ClientID + "','" + ddlPort.ClientID + "','" + lblInterface.ClientID + "');");
                ddlPort.Attributes.Add("onchange", "UpdateDropDownHidden('" + ddlPort.ClientID + "','" + hdnPort.ClientID + "');UpdateNetworkInterface('" + txtFexID.ClientID + "','" + ddlPort.ClientID + "','" + lblInterface.ClientID + "');");
                txtFexID.Style["display"] = "none";
            }
            else
            {
                ddlSwitch.Attributes.Add("onchange", "PopulateSwitchs('" + ddlSwitch.ClientID + "','" + ddlBlade.ClientID + "',null,'" + ddlPort.ClientID + "','" + txtInterface.ClientID + "',null,'" + hdnBlade.ClientID + "');");
                ddlBlade.Attributes.Add("onchange", "UpdateDropDownHidden('" + ddlBlade.ClientID + "','" + hdnBlade.ClientID + "');");
            }
            // Load the current value
            DataSet dsSwitchPort = oAsset.GetSwitchports(_assetid, _type, _nic);
            if (dsSwitchPort.Tables[0].Rows.Count > 0)
            {
                DataRow drSwitchPort = dsSwitchPort.Tables[0].Rows[0];
                ddlSwitch.SelectedValue = drSwitchPort["id"].ToString();
                int intBlades = Int32.Parse(drSwitchPort["blades"].ToString());
                for (int ii = 1; ii <= intBlades; ii++)
                    ddlBlade.Items.Add(new ListItem("Blade # " + ii.ToString(), ii.ToString()));
                ddlBlade.Enabled = (intBlades > 1);
                if (drSwitchPort["nexus"].ToString() == "1" && txtFexID != null)
                {
                    txtFexID.Text = drSwitchPort["blade"].ToString();
                    txtFexID.Style["display"] = "inline";
                    ddlBlade.Style["display"] = "none";
                    hdnBlade.Value = txtFexID.Text;
                }
                else
                {
                    ddlBlade.SelectedValue = drSwitchPort["blade"].ToString();
                    hdnBlade.Value = ddlBlade.SelectedItem.Value;
                }
                int intPorts = Int32.Parse(drSwitchPort["ports"].ToString());
                for (int ii = 1; ii <= intPorts; ii++)
                    ddlPort.Items.Add(new ListItem("Port # " + ii.ToString(), ii.ToString()));
                ddlPort.Enabled = (intPorts > 1);
                ddlPort.SelectedValue = drSwitchPort["port"].ToString();
                txtInterface.Text = drSwitchPort["interface"].ToString();
                if (txtFexID != null && txtFexID.Style["display"] == "inline")
                {
                    txtInterface.Style["display"] = "none";
                    if (lblInterface != null)
                        lblInterface.Text = txtFexID.Text + "/" + ddlPort.SelectedItem.Value;
                }
                hdnPort.Value = ddlPort.SelectedItem.Value;
            }
            else
            {
                ddlBlade.Items.Add("-- SELECT A SWITCH --");
                ddlBlade.Enabled = false;
                hdnBlade.Value = "0";
                ddlPort.Items.Add("-- SELECT A SWITCH --");
                ddlPort.Enabled = false;
                hdnPort.Value = "0";
                if (lblInterface != null)
                    lblInterface.Style["display"] = "none";
            }
        }
        private void LoadWorldWidePortNames(int intAssetId)
        {
            DataSet dsHBA = oAsset.GetHBA(intAssetId);
            dlWWPortNamesList.DataSource = dsHBA;
            dlWWPortNamesList.DataBind();
            lblWWPNamesNoItems.Visible = (dlWWPortNamesList.Items.Count == 0);
            //btnAddWWPortName.Enabled = (dsHBA.Tables[0].Rows.Count < intRequiredHBACount);
        }

        #endregion
        

        #region save Asset details
        
        private void SaveAssetStagingAndConfigDetails()
        {
            switch (hdnAssetCategoryId.Value)
            {
                case "1":  //Physical Server
                    SaveServerInfo();
                    break;
                case "2": //Blade
                    SaveBladeInfo();
                    break;
                case "3": //Enclosure
                    SaveEnclosureInfo();
                    break;
                case "4": //Rack
                    SaveRackInfo();
                    break;
                default:
                    break;
            }
            if (panSwitches.Visible == true)
            {
                AddSwitchPort(ddlNetwork1, Int32.Parse(hdnAssetId.Value), SwitchPortType.Network, 1, hdnNetwork1Blade, hdnNetwork1Port, txtNetwork1Interface.Text);
                AddSwitchPort(ddlNetwork2, Int32.Parse(hdnAssetId.Value), SwitchPortType.Network, 2, hdnNetwork2Blade, hdnNetwork2Port, txtNetwork2Interface.Text);
                AddSwitchPort(ddlBackup, Int32.Parse(hdnAssetId.Value), SwitchPortType.Backup, 1, hdnBackupBlade, hdnBackupPort, txtBackupInterface.Text);
                AddSwitchPort(ddlRemote, Int32.Parse(hdnAssetId.Value), SwitchPortType.Remote, 1, hdnRemoteBlade, hdnRemotePort, txtRemoteInterface.Text);
                AddSwitchPort(ddlCluster1, Int32.Parse(hdnAssetId.Value), SwitchPortType.Cluster, 1, hdnCluster1Blade, hdnCluster1Port, txtCluster1Interface.Text);
                AddSwitchPort(ddlCluster2, Int32.Parse(hdnAssetId.Value), SwitchPortType.Cluster, 2, hdnCluster2Blade, hdnCluster2Port, txtCluster2Interface.Text);
                AddSwitchPort(ddlStorage1, Int32.Parse(hdnAssetId.Value), SwitchPortType.Storage, 1, hdnStorage1Blade, hdnStorage1Port, txtStorage1Interface.Text);
                AddSwitchPort(ddlStorage2, Int32.Parse(hdnAssetId.Value), SwitchPortType.Storage, 2, hdnStorage2Blade, hdnStorage2Port, txtStorage2Interface.Text);
            }
            populateLocations();

        }
        private void AddSwitchPort(DropDownList _switch, int _assetid, SwitchPortType _type, int _nic, HiddenField _blade, HiddenField _port, string _interface)
        {
            int intSwitch = 0;
            int intPort = 0;
            if (Int32.TryParse(_switch.SelectedItem.Value, out intSwitch) == true && Int32.TryParse(Request.Form[_port.UniqueID], out intPort) == true)
            {
                if (intSwitch > 0 && intPort > 0)
                    oAsset.AddSwitchport(intSwitch, _assetid, _type, _nic, Request.Form[_blade.UniqueID], intPort, _interface);
            }
        }
        private void SaveServerInfo()
        {
            DataSet dsAsset = oAsset.GetServerOrBlade(Int32.Parse(hdnAssetId.Value));
            if (dsAsset.Tables[0].Rows.Count > 0)
            {
                oAsset.UpdateServerInfoOnly(Int32.Parse(hdnAssetId.Value), "", (int)AssetStatus.InStock, intProfile, DateTime.Now,
                    Int32.Parse(ddlClass.SelectedValue),
                    Int32.Parse(hdnEnvironment.Value.Trim() == "" ? "0" : hdnEnvironment.Value.Trim()),
                    //Int32.Parse(hdnLocation.Value), Int32.Parse(ddlRoom.SelectedValue),
                    Int32.Parse(hdnRackId.Value), txtRackPosition.Text.Trim(), txtILO.Text.Trim(), 
                    txtDummyName.Text.Trim(), "",
                    Int32.Parse(txtVLAN.Text.Trim() == "" ? "0" : txtVLAN.Text.Trim()), Int32.Parse(ddlBuildNetwork.SelectedItem.Value), 
                    Int32.Parse(ddlResiliency.SelectedItem.Value), Int32.Parse(ddlOperatingSystemGroup.SelectedItem.Value));

                //VerifyIsAssetStagedAndConfiguredAndSetStatusToAvailable(Int32.Parse(hdnOrderId.Value), Int32.Parse(hdnAssetId.Value));

            }
            else
            {
                oAsset.AddServer(Int32.Parse(hdnAssetId.Value), (int)AssetStatus.InStock, intProfile, DateTime.Now,
                     Int32.Parse(ddlClass.SelectedValue),
                     Int32.Parse(hdnEnvironment.Value.Trim() == "" ? "0" : hdnEnvironment.Value.Trim()),
                     //Int32.Parse(hdnLocation.Value), Int32.Parse(ddlRoom.SelectedValue),
                     Int32.Parse(hdnRackId.Value), txtRackPosition.Text.Trim(), txtILO.Text.Trim(),
                     txtDummyName.Text.Trim(), "",
                     Int32.Parse(txtVLAN.Text.Trim() == "" ? "0" : txtVLAN.Text.Trim()), Int32.Parse(ddlBuildNetwork.SelectedItem.Value),
                     Int32.Parse(ddlResiliency.SelectedItem.Value), Int32.Parse(ddlOperatingSystemGroup.SelectedItem.Value));

                //VerifyIsAssetStagedAndConfiguredAndSetStatusToAvailable(Int32.Parse(hdnOrderId.Value), Int32.Parse(hdnAssetId.Value));

            }
        }
        private void SaveBladeInfo()
        {
             DataSet dsAsset = oAsset.GetServerOrBlade(Int32.Parse(hdnAssetId.Value));
             if (dsAsset.Tables[0].Rows.Count > 0)
             {
                 oAsset.UpdateBladeInfoOnly(Int32.Parse(hdnAssetId.Value), "", (int)AssetStatus.Arrived, intProfile, DateTime.Now,
                                Int32.Parse(ddlEnclosure.SelectedValue), Int32.Parse(ddlClass.SelectedValue),
                                Int32.Parse(hdnEnvironment.Value.Trim() == "" ? "0" : hdnEnvironment.Value.Trim()),
                                txtILO.Text.Trim(), txtDummyName.Text.Trim(),
                                "", Int32.Parse(txtVLAN.Text.Trim()),
                                Int32.Parse(ddlBuildNetwork.SelectedItem.Value), Int32.Parse(txtSlot.Text.Trim()), (chkSpare.Checked ? 1 : 0), 
                                Int32.Parse(ddlResiliency.SelectedItem.Value), Int32.Parse(ddlOperatingSystemGroup.SelectedItem.Value));

                 //VerifyIsAssetStagedAndConfiguredAndSetStatusToAvailable(Int32.Parse(hdnOrderId.Value), Int32.Parse(hdnAssetId.Value));
             }
             else 
             {
                 oAsset.AddBlade(Int32.Parse(hdnAssetId.Value), (int)AssetStatus.Arrived, intProfile, DateTime.Now,
                                 Int32.Parse(ddlEnclosure.SelectedValue), Int32.Parse(ddlClass.SelectedValue),
                                 Int32.Parse(hdnEnvironment.Value.Trim() == "" ? "0" : hdnEnvironment.Value.Trim()),
                                 txtILO.Text.Trim(), txtDummyName.Text.Trim(),
                                 "", Int32.Parse(txtVLAN.Text.Trim()),
                                 Int32.Parse(ddlBuildNetwork.SelectedItem.Value), Int32.Parse(txtSlot.Text), (chkSpare.Checked ? 1 : 0), 
                                 Int32.Parse(ddlResiliency.SelectedItem.Value), Int32.Parse(ddlOperatingSystemGroup.SelectedItem.Value));

                 //VerifyIsAssetStagedAndConfiguredAndSetStatusToAvailable(Int32.Parse(hdnOrderId.Value), Int32.Parse(hdnAssetId.Value));

             
             }

        }
        private void SaveEnclosureInfo()
        {
            DataSet dsAsset = oAsset.GetEnclosure(Int32.Parse(hdnAssetId.Value));
             if (dsAsset.Tables[0].Rows.Count > 0)
             {
                 oAsset.UpdateEnclosure(Int32.Parse(hdnAssetId.Value), txtDeviceName.Text.Trim(), (int)AssetStatus.InStock, intProfile, DateTime.Now,
                                   0, 0,
                                    //Int32.Parse(hdnLocation.Value), Int32.Parse(ddlRoom.SelectedItem.Value),
                                    Int32.Parse(hdnRackId.Value), txtRackPosition.Text.Trim(),
                                    Int32.Parse(txtVLAN.Text.Trim() == "" ? "0" : txtVLAN.Text.Trim()),
                                    txtOAIP.Text.Trim(), Int32.Parse(ddlResiliency.SelectedItem.Value));
                 //VerifyIsAssetStagedAndConfiguredAndSetStatusToAvailable(Int32.Parse(hdnAssetId.Value));

             }
             else
             {
                 oAsset.AddEnclosure(Int32.Parse(hdnAssetId.Value), txtDeviceName.Text.Trim(), (int)AssetStatus.InStock, intProfile, DateTime.Now,
                                    0,
                                    0,
                                    //Int32.Parse(hdnLocation.Value), Int32.Parse(ddlRoom.SelectedItem.Value),
                                    Int32.Parse(hdnRackId.Value), txtRackPosition.Text.Trim(),
                                    Int32.Parse(txtVLAN.Text.Trim() == "" ? "0" : txtVLAN.Text.Trim()),
                                    txtOAIP.Text.Trim(), Int32.Parse(ddlResiliency.SelectedItem.Value));

                 //VerifyIsAssetStagedAndConfiguredAndSetStatusToAvailable(Int32.Parse(hdnAssetId.Value));
             
             }

        }
        private void SaveRackInfo()
        {
             DataSet dsAsset = oAsset.GetRackByAsset(Int32.Parse(hdnAssetId.Value));
             if (dsAsset.Tables[0].Rows.Count == 0)
             {
                 oRack.Add(txtDeviceName.Text.Trim(),Int32.Parse(hdnAssetId.Value),Int32.Parse(hdnZoneId.Value),"", 1);
                 oAsset.AddStatus(Int32.Parse(hdnAssetId.Value), txtDeviceName.Text.Trim(),(int)AssetStatus.InStock,intProfile, DateTime.Now);
                 //VerifyIsAssetStagedAndConfiguredAndSetStatusToAvailable(Int32.Parse(hdnAssetId.Value));
             }
            else
             {
                 oRack.Update(Int32.Parse(dsAsset.Tables[0].Rows[0]["rackid"].ToString()), txtDeviceName.Text.Trim(), Int32.Parse(hdnAssetId.Value), Int32.Parse(hdnZoneId.Value), "", 1);
                 oAsset.UpdateStatus(Int32.Parse(hdnAssetId.Value), txtDeviceName.Text.Trim(),(int)AssetStatus.InStock, intProfile, DateTime.Now);
                 //VerifyIsAssetStagedAndConfiguredAndSetStatusToAvailable(Int32.Parse(hdnAssetId.Value));
             }

         }
        #endregion


        private void SetControlsForAssetCategory(bool IsDell)
        {
            trDeviceName.Visible = false;
            trAssetSerial.Visible = true;
            trAssetTag.Visible = true;
            trModel.Visible = false;
            trModelProperty.Visible = true;
            trLocation.Visible = true;
            trRoom.Visible = false;
            trZone.Visible = false;
            trRack.Visible = false;
            trRackPosition.Visible = false;
            trClass.Visible = false;
            trEnvironment.Visible = false;
            trEnclosure.Visible = false;
            trSlot.Visible = false;
            trSpare.Visible = false;
            trILO.Visible = false;
            trDummyName.Visible = false;
            trVLAN.Visible = false;
            trBuildNetwork.Visible = false;
            trOAIP.Visible = false;
            trResiliency.Visible = false;
            trOperatingSystemGroup.Visible = false;
            pnlWWPortNames.Visible = false;

            //Display Build Network for Sun Blade only i.e.  ModelType(15)
            int intModelId = Int32.Parse(oModelsProperties.Get(Int32.Parse(hdnModelPropertyId.Value), "modelid"));
            int intModelTypeId = Int32.Parse(oModel.Get(intModelId, "TypeId"));
            if (intModelTypeId == 15)
                trBuildNetwork.Visible = true;


            switch (hdnAssetCategoryId.Value)
            {
                 

                case "1":  //Physical Server
                    trDeviceName.Visible = false;
                    trLocation.Visible = true;
                    trRoom.Visible = true;
                    trZone.Visible = true;
                    trRack.Visible = true;
                    trRackPosition.Visible = true;
                    trResiliency.Visible = true;
                    trClass.Visible = true;
                    trEnvironment.Visible = true;
                    trEnclosure.Visible = false;
                    trSlot.Visible = false;
                    trILO.Visible = true;
                    trDummyName.Visible = true;
                    trVLAN.Visible = true;
                    trOAIP.Visible = false;
                    trResiliency.Visible = true;
                    trOperatingSystemGroup.Visible = true;
                    pnlWWPortNames.Visible = (IsDell == false);

                    btnSelectLocation.Attributes.Add("onclick", "return LoadLocationRoomRack('" + "rack" + "','" + hdnRackId.ClientID + "', '" + txtLocation.ClientID + "','" + txtRoom.ClientID + "','" + txtZone.ClientID + "','" + txtRack.ClientID + "');");

                    btnSave.Attributes.Add("onclick", "return ValidateInput()" +
                                   " && ValidateHidden0('" + hdnRackId.ClientID + "','<%=btnChangeLocation.ClientID %>','Please select a location details')" +
                                   " && ProcessControlButton()" +
                                   ";");
                    btnSaveAndClose.Attributes.Add("onclick", "return ValidateInput()" +
                                         " && ValidateHidden0('" + hdnRackId.ClientID + "','<%=btnChangeLocation.ClientID %>','Please select a location details')" +
                                         " && ProcessControlButton()" +
                                         ";");

                    break;
                case "2": //Blade
                    trDeviceName.Visible = false;
                    trLocation.Visible = false;
                    trRoom.Visible = false;
                    trZone.Visible = false;
                    trRack.Visible = false;
                    trRackPosition.Visible = false;
                    trResiliency.Visible = true;
                    trClass.Visible = true;
                    trEnvironment.Visible = true;
                    trEnclosure.Visible = true;
                    trSlot.Visible = true;
                    trSpare.Visible = true;
                    trILO.Visible = true;
                    trDummyName.Visible = true;
                    trVLAN.Visible = true;
                    trOAIP.Visible = false;
                    trResiliency.Visible = true;
                    trOperatingSystemGroup.Visible = true;
                    pnlWWPortNames.Visible = (IsDell == false);

                    btnSave.Attributes.Add("onclick", "return ValidateInput()" +
                                  " && ProcessControlButton()" +
                                  ";");
                    btnSaveAndClose.Attributes.Add("onclick", "return ValidateInput()" +
                                         " && ProcessControlButton()" +
                                         ";");

                    break;
                case "3": //Enclosure
                    trDeviceName.Visible = true;
                    trLocation.Visible = true;
                    trRoom.Visible = true;
                    trZone.Visible = true;
                    trRack.Visible = true;
                    trRackPosition.Visible = true;
                    trResiliency.Visible = true;
                    trClass.Visible = false;
                    trEnvironment.Visible = false;
                    trEnclosure.Visible = false;
                    trSlot.Visible = false;
                    trILO.Visible = false;
                    trDummyName.Visible = false;
                    trVLAN.Visible = true;
                    trBuildNetwork.Visible = false;
                    trOAIP.Visible = true;
                    trResiliency.Visible = true;
                    trOperatingSystemGroup.Visible = true;
                    pnlWWPortNames.Visible = false;

                    btnSelectLocation.Attributes.Add("onclick", "return LoadLocationRoomRack('" + "rack" + "','" + hdnRackId.ClientID + "', '" + txtLocation.ClientID + "','" + txtRoom.ClientID + "','" + txtZone.ClientID + "','" + txtRack.ClientID + "');");

                    btnSave.Attributes.Add("onclick", "return ValidateInput()" +
                                  " && ValidateHidden0('" + hdnRackId.ClientID + "','<%=btnChangeLocation.ClientID %>','Please select a location details')" +
                                  " && ProcessControlButton()" +
                                  ";");
                    btnSaveAndClose.Attributes.Add("onclick", "return ValidateInput()" +
                                         " && ValidateHidden0('" + hdnRackId.ClientID + "','<%=btnChangeLocation.ClientID %>','Please select a location details')" +
                                         " && ProcessControlButton()" +
                                         ";");

                    break;
                case "4": //Rack
                    trDeviceName.Visible = true;
                    trLocation.Visible = true;
                    trRoom.Visible = true;
                    trZone.Visible = true;
                    trRack.Visible = false;
                    trRackPosition.Visible = false;
                    trResiliency.Visible = false;
                    trClass.Visible = false;
                    trEnvironment.Visible = false;
                    trEnclosure.Visible = false;
                    trSlot.Visible = false;
                    trILO.Visible = false;
                    trDummyName.Visible = false;
                    trVLAN.Visible = false;
                    trBuildNetwork.Visible = false;
                    trOAIP.Visible = false;
                    trResiliency.Visible = false;
                    trOperatingSystemGroup.Visible = false;
                    pnlWWPortNames.Visible = false;

                    btnSelectLocation.Attributes.Add("onclick", "return LoadLocationRoomRack('" + "zone" + "','" + hdnRackId.ClientID + "', '" + txtLocation.ClientID + "','" + txtRoom.ClientID + "','" + txtZone.ClientID + "','');");

                    btnSave.Attributes.Add("onclick", "return ValidateInput()" +
                                 " && ValidateHidden0('" + hdnRoomId.ClientID + "','<%=btnChangeLocation.ClientID %>','Please select a location details')" +
                                 " && ProcessControlButton()" +
                                 ";");
                    btnSaveAndClose.Attributes.Add("onclick", "return ValidateInput()" +
                                         " && ValidateHidden0('" + hdnRoomId.ClientID + "','<%=btnChangeLocation.ClientID %>','Please select a location details')" +
                                         " && ProcessControlButton()" +
                                         ";");
                    break;
                default:
                    break;
                    
                     
            }
        }

        protected void dlWWPortNamesList_ItemDataBound(object sender, DataListItemEventArgs e)
        {

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataRowView drv = (DataRowView)e.Item.DataItem;
                Label lblWWPortNamesListName = (Label)e.Item.FindControl("lblWWPortNamesListName");
                lblWWPortNamesListName.Text = drv["name"].ToString();

                Label lblWWPortNamesListLastUpdated = (Label)e.Item.FindControl("lblWWPortNamesListLastUpdated");
                lblWWPortNamesListLastUpdated.Text = drv["modified"].ToString();

                LinkButton lnkbtnWWPortNamesListDelete = (LinkButton)e.Item.FindControl("lnkbtnWWPortNamesListDelete");
                lnkbtnWWPortNamesListDelete.Text = "Delete";
                lnkbtnWWPortNamesListDelete.CommandArgument = drv["id"].ToString();
                lnkbtnWWPortNamesListDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this item?')&& ProcessControlButton();");

            }

        }

        protected void btnSave_Click(Object Sender, EventArgs e)
        {
            SaveAssetStagingAndConfigDetails();
            //LoadAssetInformation();
            Response.Redirect(Request.Url.PathAndQuery);
        }

        protected void btnSaveAndClose_Click(Object Sender, EventArgs e)
        {
            SaveAssetStagingAndConfigDetails();
            Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "reload", "<script type=\"text/javascript\">if (window.opener == null) { parent.HidePanel(); } else { window.close(); };window.parent.SaveAndRefreshWindow();<" + "/" + "script>");
        }

        protected void btnClose_Click(Object Sender, EventArgs e)
        {
            Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "reload", "<script type=\"text/javascript\">if (window.opener == null) { parent.HidePanel(); } else { window.close(); };window.parent.SaveAndRefreshWindow();<" + "/" + "script>");

        }


        protected void btnAddWWPortName_Click(Object Sender, EventArgs e)
        {
            oAsset.AddHBA(Int32.Parse(hdnAssetId.Value), txtWWPortName.Text);
            LoadWorldWidePortNames(Int32.Parse(hdnAssetId.Value));
            txtWWPortName.Text = "";
            populateLocations();
            populateSelection();
        }

        protected void lnkbtnWWPortNamesListDelete_Click(Object Sender, EventArgs e)
        {
            LinkButton oButton = (LinkButton)Sender;
            oAsset.DeleteHBA(Int32.Parse(oButton.CommandArgument));
            LoadWorldWidePortNames(Int32.Parse(hdnAssetId.Value));
            populateLocations();
            populateSelection();
        }

        protected void populateLocations()
        {

            //Populate Location Selection
            if (hdnAssetCategoryId.Value == "4")
            {
                if (hdnZoneId.Value == "") hdnZoneId.Value = "0";
                Zones oZone = new Zones(intProfile, dsn);

                DataSet dsLocation = oZone.Gets(Int32.Parse(hdnRackId.Value));
                if (dsLocation.Tables[0].Rows.Count > 0)
                {
                    txtLocation.Text = dsLocation.Tables[0].Rows[0]["Location"].ToString();
                    txtRoom.Text = dsLocation.Tables[0].Rows[0]["Room"].ToString(); ;
                    txtZone.Text = dsLocation.Tables[0].Rows[0]["Zone"].ToString(); ;
                }
            }
            else
            {
                if (hdnRackId.Value == "") hdnRackId.Value = "0";
                RacksNew oRack = new RacksNew(intProfile, dsn);

                DataSet dsLocation = oRack.Gets(Int32.Parse(hdnRackId.Value));
                if (dsLocation.Tables[0].Rows.Count > 0)
                {
                    txtLocation.Text = dsLocation.Tables[0].Rows[0]["Location"].ToString();
                    txtRoom.Text = dsLocation.Tables[0].Rows[0]["Room"].ToString(); ;
                    txtZone.Text = dsLocation.Tables[0].Rows[0]["Zone"].ToString(); ;
                    txtRack.Text = dsLocation.Tables[0].Rows[0]["Rack"].ToString(); ;
                }
            }
        }

        protected void populateSelection()
        {
                ddlEnvironment.DataTextField = "name";
                ddlEnvironment.DataValueField = "id";
                ddlEnvironment.DataSource = oClass.GetEnvironment(Int32.Parse(ddlClass.SelectedValue), 0);
                ddlEnvironment.DataBind();
                ddlEnvironment.Items.Insert(0, new ListItem("-- SELECT --", "0"));
                
                ddlEnvironment.SelectedValue = (hdnEnvironment.Value != "" ? hdnEnvironment.Value : "0");

        
        }
    }
}
