<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="asset_models_servers.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.asset_models_servers" %>


<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
<title>ClearView Administration</title>
<link rel="stylesheet" type="text/css" href="/css/default.css" />
<script type="text/javascript" src="/javascript/global.js"></script>
<script type="text/javascript">
    var oAdd = null;
    var oView = null;
    var oAddButton = null;
    var oDeleteButton = null;
    var oHidden = null;
    var oAvailable = null;
    var oReplicate = null;
    var oAmp = null;
    var oNetworkPorts = null;
    var oStoragePorts = null;
    var oName = null;
    var oName = null;
    var oParent = null;
    var oParentId = null;
    var oRam = null;
    var oCount = null;
    var oSpeed = null;
    var oChipset= null;
    var oStorageThresholdMin = null;
    var oStorageThresholdMax= null;
    
    var oAssetCategory= null;
    
    var high_availability = null;
    var high_performance = null;
    var low_performance = null;
    var enforce_1_1_recovery = null;
    var no_many_1_recovery = null;
    var vmware_virtual = null;
    var ibm_virtual = null;
    var sun_virtual = null;
    var storage_db_boot_local = null;
    var storage_db_boot_san_windows = null;
    var storage_db_boot_san_unix = null;
    var storage_de_fdrive_can_local = null;
    var storage_de_fdrive_must_san = null;
    var storage_de_fdrive_only = null;
    var oManualBuild = null;
    var type_blade = null;
    var type_physical = null;
    var type_vmware = null;
    var type_enclosure = null;
    var config_service_pack = null;
    var config_vmware_template = null;
    var config_maintenance_level = null;
    var vio = null;
    var fabric = null;
    var storage_type = null;
    var inventory = null;
    var dell = null;
    var dellconfig = null;
    var configure_switches = null;
    var enter_name = null;
    var enter_ip = null;
    var associate_project = null;
    var full_height = null;
    var oEnabled = null;
    function Edit(strId, strAvailable, strReplicate, strAmp, strNetworkPorts, strStoragePorts, strName, strParentId, strParent, strRam, strCount,strSpeed,strChipset,strStorageThresholdMin,strStorageThresholdMax, strAssetCategory, _high_availability, _high_performance, _low_performance, _enforce_1_1_recovery, _no_many_1_recovery, _vmware_virtual, _ibm_virtual, _sun_virtual, _storage_db_boot_local, _storage_db_boot_san_windows, _storage_db_boot_san_unix, _storage_de_fdrive_can_local, _storage_de_fdrive_must_san, _storage_de_fdrive_only, strManualBuild, _type_blade, _type_physical, _type_vmware, _type_enclosure, _config_service_pack, _config_vmware_template, _config_maintenance_level, _vio, _fabric, _storage_type, _inventory, _dell, _dellconfig, _configure_switches, _enter_name, _enter_ip, _associate_project, _full_height, strEnabled) {
        oAdd.style.display = "inline";
        oView.style.display = "none";
        oAddButton.value = "Update";
        oDeleteButton.style.display = "inline";
        oHidden.value = strId;
        oAvailable.checked = (strAvailable == "1");
        oReplicate.value = strReplicate;
        oAmp.value = strAmp;
        oNetworkPorts.value = strNetworkPorts;
        oStoragePorts.value = strStoragePorts;
        oName.value = strName;
        oParentId.value = strParentId;
        if (strParent == "" || strParentId == "0")
            oParent.innerText = "None";
        else
            oParent.innerText = strParent;
        oRam.value = strRam;
        oCount.value = strCount;
        oSpeed.value = strSpeed;
        for (var ii=0; ii<oChipset.length; ii++) 
        {
            if (oChipset.options[ii].value == strChipset)
                oChipset.selectedIndex = ii;
        }
        if (strStorageThresholdMin=="")
            strStorageThresholdMin="0";
        oStorageThresholdMin.value =strStorageThresholdMin;
        
        if (strStorageThresholdMax=="")
            strStorageThresholdMax="0";
        oStorageThresholdMax.value=strStorageThresholdMax;
        
        for (var ii=0; ii<oAssetCategory.length; ii++) 
        {
            if (oAssetCategory.options[ii].value == strAssetCategory)
                oAssetCategory.selectedIndex = ii;
        }
        
        
        high_availability.checked = (_high_availability == "1");
        high_performance.checked = (_high_performance == "1");
        low_performance.checked = (_low_performance == "1");
        enforce_1_1_recovery.checked = (_enforce_1_1_recovery == "1");
        no_many_1_recovery.checked = (_no_many_1_recovery == "1");
        vmware_virtual.checked = (_vmware_virtual == "1");
        ibm_virtual.checked = (_ibm_virtual == "1");
        sun_virtual.checked = (_sun_virtual == "1");
        storage_db_boot_local.checked = (_storage_db_boot_local == "1");
        storage_db_boot_san_windows.checked = (_storage_db_boot_san_windows == "1");
        storage_db_boot_san_unix.checked = (_storage_db_boot_san_unix == "1");
        storage_de_fdrive_can_local.checked = (_storage_de_fdrive_can_local == "1");
        storage_de_fdrive_must_san.checked = (_storage_de_fdrive_must_san == "1");
        storage_de_fdrive_only.checked = (_storage_de_fdrive_only == "1");
        oManualBuild.checked = (strManualBuild == "1");
        type_blade.checked = (_type_blade == "1");
        type_physical.checked = (_type_physical == "1");
        type_vmware.checked = (_type_vmware == "1");
        type_enclosure.checked = (_type_enclosure == "1");
        config_service_pack.checked = (_config_service_pack == "1");
        config_vmware_template.checked = (_config_vmware_template == "1");
        config_maintenance_level.checked = (_config_maintenance_level == "1");
        vio.checked = (_vio == "1");
        for (var ii=0; ii<fabric.length; ii++) {
            if (fabric.options[ii].value == _fabric)
                fabric.selectedIndex = ii;
        }
        for (var ii=0; ii<storage_type.length; ii++) {
            if (storage_type.options[ii].value == _storage_type)
                storage_type.selectedIndex = ii;
        }
        inventory.checked = (_inventory == "1");
        dell.checked = (_dell == "1");
        for (var ii=0; ii<dellconfig.length; ii++) {
            if (dellconfig.options[ii].value == _dellconfig)
                dellconfig.selectedIndex = ii;
        }
        configure_switches.checked = (_configure_switches == "1");
        enter_name.checked = (_enter_name == "1");
        enter_ip.checked = (_enter_ip == "1");
        associate_project.checked = (_associate_project == "1");
        full_height.checked = (_full_height == "1");
        oEnabled.checked = (strEnabled == "1");
    }
    function Add(strParentId, strParent) {
        oAdd.style.display = "inline";
        oView.style.display = "none";
        oAddButton.value = "Add";
        oDeleteButton.style.display = "none";
        oHidden.value = "0";
        oAvailable.checked = false;
        oReplicate.value = "0";
        oAmp.value = "0";
        oNetworkPorts.value = "0";
        oStoragePorts.value = "0";
        oName.value = "";
        oParentId.value = strParentId;
        if (strParent == "" || strParentId == "0")
            oParent.innerText = "None";
        else
            oParent.innerText = strParent;
        oRam.value = "0";
        oCount.value = "0";
        oSpeed.value = "0";
        oChipset.selectedIndex = 0;
        oStorageThresholdMin.value="0";
        oStorageThresholdMax.value="0";
        
        oAssetCategory.selectedIndex = 0;
        
        high_availability.checked = false;
        high_performance.checked = false;
        low_performance.checked = false;
        enforce_1_1_recovery.checked = false;
        no_many_1_recovery.checked = false;
        vmware_virtual.checked = false;
        ibm_virtual.checked = false;
        sun_virtual.checked = false;
        storage_db_boot_local.checked = false;
        storage_db_boot_san_windows.checked = false;
        storage_db_boot_san_unix.checked = false;
        storage_de_fdrive_can_local.checked = false;
        storage_de_fdrive_must_san.checked = false;
        storage_de_fdrive_only.checked = false;
        oManualBuild.checked = false;
        type_blade.checked = false;
        type_physical.checked = false;
        type_vmware.checked = false;
        type_enclosure.checked = false;
        config_service_pack.checked = false;
        config_vmware_template.checked = false;
        config_maintenance_level.checked = false;
        vio.checked = false;
        fabric.selectedIndex = 0;
        storage_type.selectedIndex = 0;
        inventory.checked = false;
        dell.checked = false;
        dellconfig.selectedIndex = 0;
        configure_switches.checked = false;
        enter_name.checked = false;
        enter_ip.checked = false;
        associate_project.checked = false;
        full_height.checked = false;
        oAmp.focus();
        oEnabled.checked = true;
    }
    function Cancel() {
        oAdd.style.display = "none";
        oView.style.display = "inline";
        return false;
    }
    function Load() {
        oAdd = document.getElementById('<%=divAdd.ClientID%>');
        oView = document.getElementById('<%=divView.ClientID%>');
        oAddButton = document.getElementById('<%=btnAdd.ClientID%>');
        oDeleteButton = document.getElementById('<%=btnDelete.ClientID%>');
        oHidden = document.getElementById('<%=hdnId.ClientID%>');
        oAvailable = document.getElementById('<%=chkAvailable.ClientID%>');
        oReplicate = document.getElementById('<%=txtReplicate.ClientID%>');
        oAmp = document.getElementById('<%=txtAmp.ClientID%>');
        oNetworkPorts = document.getElementById('<%=txtNetworkPorts.ClientID%>');
        oStoragePorts = document.getElementById('<%=txtStoragePorts.ClientID%>');
        oName = document.getElementById('<%=txtName.ClientID%>');
        oParent = document.getElementById('<%=lblParent.ClientID%>');
        oParentId = document.getElementById('<%=hdnParent.ClientID%>');
        oRam = document.getElementById('<%=txtRam.ClientID%>');
        oCount = document.getElementById('<%=txtCount.ClientID%>');
        oSpeed = document.getElementById('<%=txtSpeed.ClientID%>');
        oChipset = document.getElementById('<%=ddlChipset.ClientID%>');
        oStorageThresholdMin  = document.getElementById('<%=txtStorageThreholdMin.ClientID%>');
        oStorageThresholdMax  = document.getElementById('<%=txtStorageThreholdMax.ClientID%>');
        
        oAssetCategory = document.getElementById('<%=ddlAssetCategory.ClientID%>');


        
        high_availability = document.getElementById('<%=chk_high_availability.ClientID%>');
        high_performance = document.getElementById('<%=chk_high_performance.ClientID%>');
        low_performance = document.getElementById('<%=chk_low_performance.ClientID%>');
        enforce_1_1_recovery = document.getElementById('<%=chk_enforce_1_1_recovery.ClientID%>');
        no_many_1_recovery = document.getElementById('<%=chk_no_many_1_recovery.ClientID%>');
        vmware_virtual = document.getElementById('<%=chk_vmware_virtual.ClientID%>');
        ibm_virtual = document.getElementById('<%=chk_ibm_virtual.ClientID%>');
        sun_virtual = document.getElementById('<%=chk_sun_virtual.ClientID%>');
        storage_db_boot_local = document.getElementById('<%=chk_storage_db_boot_local.ClientID%>');
        storage_db_boot_san_windows = document.getElementById('<%=chk_storage_db_boot_san_windows.ClientID%>');
        storage_db_boot_san_unix = document.getElementById('<%=chk_storage_db_boot_san_unix.ClientID%>');
        storage_de_fdrive_can_local = document.getElementById('<%=chk_storage_de_fdrive_can_local.ClientID%>');
        storage_de_fdrive_must_san = document.getElementById('<%=chk_storage_de_fdrive_must_san.ClientID%>');
        storage_de_fdrive_only = document.getElementById('<%=chk_storage_de_fdrive_only.ClientID%>');
        oManualBuild = document.getElementById('<%=chkManualBuild.ClientID%>');
        type_blade = document.getElementById('<%=chk_type_blade.ClientID%>');
        type_physical = document.getElementById('<%=chk_type_physical.ClientID%>');
        type_vmware = document.getElementById('<%=chk_type_vmware.ClientID%>');
        type_enclosure = document.getElementById('<%=chk_type_enclosure.ClientID%>');
        config_service_pack = document.getElementById('<%=chk_config_service_pack.ClientID%>');
        config_vmware_template = document.getElementById('<%=chk_config_vmware_template.ClientID%>');
        config_maintenance_level = document.getElementById('<%=chk_config_maintenance_level.ClientID%>');
        vio = document.getElementById('<%=chkVio.ClientID%>');
        fabric = document.getElementById('<%=ddlStorageFabric.ClientID%>');
        storage_type = document.getElementById('<%=ddlStorageType.ClientID%>');
        inventory = document.getElementById('<%=chk_inventory.ClientID%>');
        dell = document.getElementById('<%=chkDell.ClientID%>');
        dellconfig = document.getElementById('<%=ddlDell.ClientID%>');
        configure_switches = document.getElementById('<%=chkConfigureSwitches.ClientID%>');
        enter_name = document.getElementById('<%=chkEnterName.ClientID%>');
        enter_ip = document.getElementById('<%=chkEnterIP.ClientID%>');
        associate_project = document.getElementById('<%=chkAssociateProject.ClientID%>');
        full_height = document.getElementById('<%=chkFullHeight.ClientID%>');
        oEnabled = document.getElementById('<%=chkEnabled.ClientID%>');
    }
</script>
</head>
<body topmargin="0" leftmargin="0" onload="Load()">
<form id="Form1" runat="server">
        <table width="100%" height="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
        <td height="100%">
        <div style="height:100%; overflow:auto">
<table width="98%" cellpadding="0" cellspacing="0" border="0" align="center">
		<tr><td colspan="2">&nbsp;</td></tr>
		<tr> 
		    <td><b>Asset Model Properties - Servers</b></td>
		    <td align="right">
		        <a href="javascript:void(0);" onclick="Add(0);" class="cmlink" title="Click to Add New">Add New</a>
		    </td>
		</tr>
		<tr><td colspan="2">&nbsp;</td></tr>
		<tr>
		    <td colspan="2" align="center">
                <div ID="divView" runat="server">
                <table width="100%" border="0" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <asp:TreeView ID="oTreeview" runat="server" ShowLines="true" NodeIndent="35">
                                <NodeStyle CssClass="default" />
                            </asp:TreeView>
                        </td>
                        <td width="150" valign="top">
		                    <img src="/images/check.gif" border="0" align="Absmiddle" /> = Model is enabled (and available for override) <br /><br />
		                    <img src="/images/cancel.gif" border="0" align="Absmiddle" /> = Model is disabled (hidden throughout the system) <br /><br />
		                    Otherwise, Model is enabled but not available for override <br /><br />
                        </td>
                    </tr>
                </table>
               </div>

                <div id="divAdd" runat="server" style="display:none">
                    <table width="95%" cellpadding="3" cellspacing="2" border="0" align="center">
                        <tr>
                            <td nowrap>Model:</td>
                            <td width="100%"><asp:label ID="lblParent" CssClass="default" runat="server" />&nbsp;&nbsp;<asp:Button ID="btnParent" runat="server" CssClass="default" Width="25" Text="..." /></td>
                        </tr>
                        <tr> 
                            <td nowrap>Available:</td>
                            <td width="100%"><asp:CheckBox ID="chkAvailable" runat="server" Checked="true" /></td>
                        </tr>
                        <tr> 
                            <td nowrap>Replicate Times:</td>
                            <td width="100%"><asp:textbox ID="txtReplicate" CssClass="default" runat="server" Width="50" MaxLength="1"/></td>
                        </tr>
                        <tr> 
                            <td nowrap>AMPs:</td>
                            <td width="100%"><asp:textbox ID="txtAmp" CssClass="default" runat="server" Width="100" MaxLength="10"/> A</td>
                        </tr>
                        <tr> 
                            <td nowrap>Network Ports:</td>
                            <td width="100%"><asp:textbox ID="txtNetworkPorts" CssClass="default" runat="server" Width="50" MaxLength="2"/></td>
                        </tr>
                        <tr> 
                            <td nowrap>Storage Ports:</td>
                            <td width="100%"><asp:textbox ID="txtStoragePorts" CssClass="default" runat="server" Width="50" MaxLength="2"/></td>
                        </tr>
                        <tr> 
                            <td nowrap>Name:</td>
                            <td width="100%"><asp:textbox ID="txtName" CssClass="default" runat="server" Width="300" MaxLength="100"/></td>
                        </tr>
                        <tr> 
                            <td nowrap>RAM:</td>
                            <td width="100%"><asp:textbox ID="txtRam" CssClass="default" runat="server" Width="100" MaxLength="10"/> GB</td>
                        </tr>
                        <tr> 
                            <td nowrap>CPU Count:</td>
                            <td width="100%"><asp:textbox ID="txtCount" CssClass="default" runat="server" Width="100" MaxLength="10"/></td>
                        </tr>
                        <tr> 
                            <td nowrap>CPU Speed:</td>
                            <td width="100%"><asp:textbox ID="txtSpeed" CssClass="default" runat="server" Width="100" MaxLength="10"/> GHz</td>
                        </tr>
                        <tr> 
                            <td nowrap>Chipset:</td>
                            <td width="100%"><asp:dropdownlist ID="ddlChipset" CssClass="default" runat="server"/></td>
                        </tr>
                        <tr> 
                            <td nowrap>Model Storage Threshold Min:</td>
                            <td width="100%"><asp:textbox ID="txtStorageThreholdMin" CssClass="default" runat="server" Width="100" MaxLength="10"/> GB</td>
                        </tr>
                          <tr> 
                            <td nowrap>Model Storage Threshold Max:</td>
                            <td width="100%"><asp:textbox ID="txtStorageThreholdMax" CssClass="default" runat="server" Width="100" MaxLength="10"/> GB</td>
                        </tr>
                        <tr> 
                            <td nowrap>Asset Category:</td>
                            <td width="100%"><asp:dropdownlist ID="ddlAssetCategory" CssClass="default" runat="server"/></td>
                        </tr>
                        <tr> 
                            <td nowrap>Enabled:</td>
                            <td width="100%"><asp:CheckBox ID="chkEnabled" runat="server" Checked="true" /></td>
                        </tr>
                        <tr>
                            <td colspan="2">&nbsp;</td>
                        </tr>
                        <tr> 
                            <td colspan="2" bgcolor="#F6F6F6" style="border:solid 1px #CCCCCC">
                                <table width="100%" cellpadding="2" cellspacing="2" border="0">
                                    <tr>
                                        <td colspan="2"><b>Model Configuration</b></td>
                                    </tr>
                                    <tr>
                                        <td nowrap>VIO (Midrange):</td>
                                        <td width="100%"><asp:CheckBox ID="chkVio" runat="server" CssClass="default" /></td>
                                    </tr>
                                    <tr>
                                        <td nowrap>Storage Fabric:</td>
                                        <td width="100%">
                                            <asp:DropDownList ID="ddlStorageFabric" runat="server" CssClass="default">
                                                <asp:ListItem Value="0" Text="Cisco" />
                                                <asp:ListItem Value="1" Text="Brocade" />
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td nowrap>Storage Type:</td>
                                        <td width="100%">
                                            <asp:DropDownList ID="ddlStorageType" runat="server" CssClass="default">
                                                <asp:ListItem Value="0" Text="IBM" />
                                                <asp:ListItem Value="1" Text="EMC" />
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td nowrap>High Availability:</td>
                                        <td width="100%"><asp:CheckBox ID="chk_high_availability" runat="server" CssClass="default" /></td>
                                    </tr>
                                    <tr>
                                        <td nowrap>High Performance:</td>
                                        <td width="100%"><asp:CheckBox ID="chk_high_performance" runat="server" CssClass="default" /></td>
                                    </tr>
                                    <tr>
                                        <td nowrap>Low Performance:</td>
                                        <td width="100%"><asp:CheckBox ID="chk_low_performance" runat="server" CssClass="default" /></td>
                                    </tr>
                                    <tr>
                                        <td nowrap>Enforce 1 to 1 Recovery:</td>
                                        <td width="100%"><asp:CheckBox ID="chk_enforce_1_1_recovery" runat="server" CssClass="default" /></td>
                                    </tr>
                                    <tr>
                                        <td nowrap>No Many to 1 Recovery:</td>
                                        <td width="100%"><asp:CheckBox ID="chk_no_many_1_recovery" runat="server" CssClass="default" /></td>
                                    </tr>
                                    <tr> 
                                        <td nowrap>Manual Build:</td>
                                        <td width="100%"><asp:CheckBox ID="chkManualBuild" runat="server" CssClass="default" /></td>
                                    </tr>
                                    <tr>
                                        <td nowrap>Have Inventory:</td>
                                        <td width="100%"><asp:CheckBox ID="chk_inventory" runat="server" CssClass="default" /> (Unchecking this will cause &quot;Out of Inventory&quot; message to appear in auto-provisioning)</td>
                                    </tr>
                                    <tr>
                                        <td nowrap>Dell:</td>
                                        <td width="100%"><asp:CheckBox ID="chkDell" runat="server" CssClass="default" /></td>
                                    </tr>
                                    <tr>
                                        <td nowrap>Dell Config:</td>
                                        <td width="100%"><asp:DropDownList ID="ddlDell" runat="server" CssClass="default"/></td>
                                    </tr>
                                    <tr>
                                        <td nowrap>Enable Switch Configuration:</td>
                                        <td width="100%"><asp:CheckBox ID="chkConfigureSwitches" runat="server" CssClass="default" /></td>
                                    </tr>
                                    <tr>
                                        <td nowrap>Can Enter NAME in manual build workflow:</td>
                                        <td width="100%"><asp:CheckBox ID="chkEnterName" runat="server" CssClass="default" /></td>
                                    </tr>
                                    <tr>
                                        <td nowrap>Can Enter IP in manual build workflow:</td>
                                        <td width="100%"><asp:CheckBox ID="chkEnterIP" runat="server" CssClass="default" /></td>
                                    </tr>
                                    <tr>
                                        <td nowrap>Associate with Project:</td>
                                        <td width="100%"><asp:CheckBox ID="chkAssociateProject" runat="server" CssClass="default" /></td>
                                    </tr>
                                    <tr>
                                        <td nowrap>Virtual Configuration (if applicable):</td>
                                        <td width="100%">
                                            <table cellpadding="2" cellspacing="1" border="0">
                                                <tr>
                                                    <td><asp:RadioButton ID="chk_vmware_virtual" runat="server" CssClass="default" Text="VMware Virtual" GroupName="virt" /></td>
                                                    <td><asp:RadioButton ID="chk_ibm_virtual" runat="server" CssClass="default" Text="IBM Virtual" GroupName="virt" /></td>
                                                    <td><asp:RadioButton ID="chk_sun_virtual" runat="server" CssClass="default" Text="SUN Virtual" GroupName="virt" /></td>
                                                    <td><asp:RadioButton ID="chk_virtual_none" runat="server" CssClass="default" Text="None" GroupName="virt" /></td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td nowrap>Storage Design Builder Config:</td>
                                        <td width="100%">
                                            <table cellpadding="2" cellspacing="1" border="0">
                                                <tr>
                                                    <td><asp:RadioButton ID="chk_storage_db_boot_local" runat="server" CssClass="default" Text="Boot Local" GroupName="db" /></td>
                                                    <td><asp:RadioButton ID="chk_storage_db_boot_san_windows" runat="server" CssClass="default" Text="Boot SAN Windows" GroupName="db" /></td>
                                                    <td><asp:RadioButton ID="chk_storage_db_boot_san_unix" runat="server" CssClass="default" Text="Boot SAN UNIX" GroupName="db" /></td>
                                                    <td><asp:RadioButton ID="chk_storage_db_none" runat="server" CssClass="default" Text="None" GroupName="db" /></td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td nowrap>Storage Design Execution Config:</td>
                                        <td width="100%">
                                            <table cellpadding="2" cellspacing="1" border="0">
                                                <tr>
                                                    <td><asp:RadioButton ID="chk_storage_de_fdrive_can_local" runat="server" CssClass="default" Text="F:\ drive can be local" GroupName="de" /></td>
                                                    <td><asp:RadioButton ID="chk_storage_de_fdrive_must_san" runat="server" CssClass="default" Text="F:\ drive must be on SAN" GroupName="de" /></td>
                                                    <td><asp:RadioButton ID="chk_storage_de_fdrive_only" runat="server" CssClass="default" Text="Only F:\ drive" GroupName="de" /></td>
                                                    <td><asp:RadioButton ID="chk_storage_de_none" runat="server" CssClass="default" Text="None" GroupName="de" /></td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td nowrap>Hardware Type:</td>
                                        <td width="100%">
                                            <table cellpadding="2" cellspacing="1" border="0">
                                                <tr>
                                                    <td><asp:RadioButton ID="chk_type_blade" runat="server" CssClass="default" Text="Blade" GroupName="hw"  /></td>
                                                    <td><asp:RadioButton ID="chk_type_physical" runat="server" CssClass="default" Text="Physical" GroupName="hw" /></td>
                                                    <td><asp:RadioButton ID="chk_type_vmware" runat="server" CssClass="default" Text="VMware" GroupName="hw" /></td>
                                                    <td><asp:RadioButton ID="chk_type_enclosure" runat="server" CssClass="default" Text="Enclosure" GroupName="hw" /></td>
                                                    <td><asp:RadioButton ID="chk_type_none" runat="server" CssClass="default" Text="None" GroupName="hw" /></td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td nowrap>Full Height:</td>
                                        <td width="100%"><asp:CheckBox ID="chkFullHeight" runat="server" CssClass="default" Text="(Blade Only)" /></td>
                                    </tr>
                                    <tr>
                                        <td nowrap>Device Configuration:</td>
                                        <td width="100%">
                                            <table cellpadding="2" cellspacing="1" border="0">
                                                <tr>
                                                    <td><asp:RadioButton ID="chk_config_service_pack" runat="server" CssClass="default" Text="Service Pack" GroupName="cfg"  /></td>
                                                    <td><asp:RadioButton ID="chk_config_vmware_template" runat="server" CssClass="default" Text="VMware Template" GroupName="cfg" /></td>
                                                    <td><asp:RadioButton ID="chk_config_maintenance_level" runat="server" CssClass="default" Text="Maintenance Level" GroupName="cfg" /></td>
                                                    <td><asp:RadioButton ID="chk_config_none" runat="server" CssClass="default" Text="None" GroupName="cfg" /></td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr><td height="5" colspan="2">&nbsp;</td></tr>
                        <tr> 
                            <td colspan="2">
                                <asp:button ID="btnAdd" CssClass="default" runat="server" Text="Add" Width="75" OnClick="btnAdd_Click" />
                                <asp:button ID="btnDelete" CssClass="default" runat="server" Text="Delete" Width="75" OnClick="btnDelete_Click" />
                                <asp:button ID="btnCancel" CssClass="default" runat="server" Text="Cancel" Width="75" />
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
    </table>
        </div>
        </td>
        </tr>
        </table>
    <input type="hidden" id="hdnId" runat="server" />
    <input type="hidden" id="hdnParent" runat="server" />
</form>
</body>
</html>
