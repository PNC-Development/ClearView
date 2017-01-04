<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ip_vlans.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.ip_vlans" %>


<script type="text/javascript">
    var oAdd = null;
    var oView = null;
    var oAddButton = null;
    var oDeleteButton = null;
    var oHidden = null;
    var oVlan = null;
    var oPhysicalW = null;
    var oPhysicalU = null;
    var oEcomProd = null;
    var oEcomService = null;
    var oIPX = null;
    var oVirtual = null;
    var oVMworkstationExternal = null;
    var oVMworkstationInternal = null;
    var oVMworkstationDR = null;
    var oVMHost = null;
    var oVMotion = null;
    var oVMWindows = null;
    var oVMLinux = null;
    var oBladesHP = null;
    var oBladesSUN = null;
    var oAPV = null;
    var oMainframe = null;
    var oCSM = null;
    var oCSMSOA = null;
    var oReplicates = null;
    var oPXE = null;
    var oILO = null;
    var oCSMVIP = null;
    var oLTMWeb = null;
    var oLTMApp = null;
    var oLTMMiddle = null;
    var oLTMVIP = null;
    var oWindowsCluster = null;
    var oUnixCluster = null;
    var oAccenture = null;
    var oHA = null;
    var oSunCluster = null;
    var oSunSVE = null;
    var oStorage = null;
    var oDellWeb = null;
    var oDellMiddleware = null;
    var oDellDatabase = null;
    var oDellFile = null;
    var oDellMisc = null;
    var oDellUnder48 = null;
    var oDellAvamar = null;
    var oName = null;
    var oClass = null;
    var oEnvironment = null;
    var oEnvironmentH = null;
    var oParent = null;
    var oParentId = null;
    var oResiliency = null;
    var oEnabled = null;
    function Edit(strId, strVlan, strPhysicalW, strPhysicalU, strEcomProd, strEcomService, strIPX, strVirtual, strVMworkstationExternal, strVMworkstationInternal, strVMworkstationDR, strVMHost, strVMotion, strVMWindows, strVMLinux, strBladesHP, strBladesSUN, strAPV, strMainframe, strCSM, strCSMSOA, strReplicates, strPXE, strILO, strCSMVIP, strLTMWeb, strLTMApp, strLTMMiddle, strLTMVIP, strWindowsCluster, strUnixCluster, strAccenture, strHA, strSunCluster, strSunSVE, strStorage, strDellWeb, strDellMiddleware, strDellDatabase, strDellFile, strDellMisc, strDellUnder48, strDellAvamar, strName, strClass, strEnvironment, strParentId, strParent, strResiliency, strEnabled) {
        oAdd.style.display = "inline";
        oView.style.display = "none";
        oAddButton.value = "Update";
        oDeleteButton.style.display = "inline";
        oHidden.value = strId;
        oVlan.value = strVlan;
        oPhysicalW.checked = (strPhysicalW == "1");
        oPhysicalU.checked = (strPhysicalU == "1");
        oEcomProd.checked = (strEcomProd == "1");
        oEcomService.checked = (strEcomService == "1");
        oIPX.checked = (strIPX == "1");
        oVirtual.checked = (strVirtual == "1");
        oVMworkstationExternal.checked = (strVMworkstationExternal == "1");
        oVMworkstationInternal.checked = (strVMworkstationInternal == "1");
        oVMworkstationDR.checked = (strVMworkstationDR == "1");
        oVMHost.checked = (strVMHost == "1");
        oVMotion.checked = (strVMotion == "1");
        oVMWindows.checked = (strVMWindows == "1");
        oVMLinux.checked = (strVMLinux == "1");
        oBladesHP.checked = (strBladesHP == "1");
        oBladesSUN.checked = (strBladesSUN == "1");
        oAPV.checked = (strAPV == "1");
        oMainframe.checked = (strMainframe == "1");
        oCSM.checked = (strCSM == "1");
        oCSMSOA.checked = (strCSMSOA == "1");
        oReplicates.checked = (strReplicates == "1");
        oPXE.checked = (strPXE == "1");
        oILO.checked = (strILO == "1");
        oCSMVIP.checked = (strCSMVIP == "1");
        oLTMWeb.checked = (strLTMWeb == "1");
        oLTMApp.checked = (strLTMApp == "1");
        oLTMMiddle.checked = (strLTMMiddle == "1");
        oLTMVIP.checked = (strLTMVIP == "1");
        oWindowsCluster.checked = (strWindowsCluster == "1");
        oUnixCluster.checked = (strUnixCluster == "1");
        oAccenture.checked = (strAccenture == "1");
        oHA.checked = (strHA == "1");
        oSunCluster.checked = (strSunCluster == "1");
        oSunSVE.checked = (strSunSVE == "1");
        oStorage.checked = (strStorage == "1");
        oDellWeb.checked = (strDellWeb == "1");
        oDellMiddleware.checked = (strDellMiddleware == "1");
        oDellDatabase.checked = (strDellDatabase == "1");
        oDellFile.checked = (strDellFile == "1");
        oDellMisc.checked = (strDellMisc == "1");
        oDellUnder48.checked = (strDellUnder48 == "1");
        oDellAvamar.checked = (strDellAvamar == "1");
        oName.value = strName;
        for (var ii=0; ii<oClass.length; ii++) {
            if (oClass.options[ii].value == strClass)
                oClass.selectedIndex = ii;
        }
        PopulateEnvironments2(oClass, oEnvironment);
        for (var ii=0; ii<oEnvironment.length; ii++) {
            if (oEnvironment.options[ii].value == strEnvironment)
                oEnvironment.selectedIndex = ii;
        }
        oEnvironmentH.value = strEnvironment;
        oParentId.value = strParentId;
        if (strParentId == "" || strParentId == "0")
            oParent.innerText = "None";
        else
            oParent.innerText = strParent;
        for (var ii=0; ii<oResiliency.length; ii++) {
            if (oResiliency.options[ii].value == strResiliency)
                oResiliency.selectedIndex = ii;
        }
        oEnabled.checked = (strEnabled == "1");
    }
    function Add() {
        oAdd.style.display = "inline";
        oView.style.display = "none";
        oAddButton.value = "Add";
        oDeleteButton.style.display = "none";
        oHidden.value = "0";
        oVlan.value = "";
        oPhysicalW.checked = false;
        oPhysicalU.checked = false;
        oEcomProd.checked = false;
        oEcomService.checked = false;
        oIPX.checked = false;
        oVirtual.checked = false;
        oVMworkstationExternal.checked = false;
        oVMworkstationInternal.checked = false;
        oVMworkstationDR.checked = false;
        oVMHost.checked = false;
        oVMotion.checked = false;
        oVMWindows.checked = false;
        oVMLinux.checked = false;
        oBladesHP.checked = false;
        oBladesSUN.checked = false;
        oAPV.checked = false;
        oMainframe.checked = false;
        oCSM.checked = false;
        oCSMSOA.checked = false;
        oReplicates.checked = false;
        oPXE.checked = false;
        oILO.checked = false;
        oCSMVIP.checked = false;
        oLTMWeb.checked = false;
        oLTMApp.checked = false;
        oLTMMiddle.checked = false;
        oLTMVIP.checked = false;
        oWindowsCluster.checked = false;
        oUnixCluster.checked = false;
        oAccenture.checked = false;
        oHA.checked = false;
        oSunCluster.checked = false;
        oSunSVE.checked = false;
        oStorage.checked = false;
        oDellWeb.checked = false;
        oDellMiddleware.checked = false;
        oDellDatabase.checked = false;
        oDellFile.checked = false;
        oDellMisc.checked = false;
        oDellUnder48.checked = false;
        oDellAvamar.checked = false;
        oName.value = "";
        oClass.selectedIndex = 0;
        oEnvironment.disabled = true;
		var oOption = document.createElement("OPTION");
		oEnvironment.add(oOption);
		oOption.text = " -- Please select a Class --";
		oOption.value = "0";
        oEnvironment.selectedIndex = 0;
        oEnvironmentH.value = "0";
        oParentId.value = '<%=intLocation.ToString() %>';
        oParent.innerText = '<%=oLocation.GetFull(intLocation) %>';
        oResiliency.selectedIndex = 0;
        oEnabled.checked = true;
        oVlan.focus();
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
        oVlan = document.getElementById('<%=txtVlan.ClientID%>');
        oPhysicalW = document.getElementById('<%=chkPhysicalW.ClientID%>');
        oPhysicalU = document.getElementById('<%=chkPhysicalU.ClientID%>');
        oEcomProd = document.getElementById('<%=chkEcomProd.ClientID%>');
        oEcomService = document.getElementById('<%=chkEcomService.ClientID%>');
        oIPX = document.getElementById('<%=chkIPX.ClientID%>');
        oVirtual = document.getElementById('<%=chkVirtual.ClientID%>');
        oVMworkstationExternal = document.getElementById('<%=chkVMworkstationExternal.ClientID%>');
        oVMworkstationInternal = document.getElementById('<%=chkVMworkstationInternal.ClientID%>');
        oVMworkstationDR = document.getElementById('<%=chkVMworkstationDR.ClientID%>');
        oVMHost = document.getElementById('<%=chkVMHost.ClientID%>');
        oVMotion = document.getElementById('<%=chkVMotion.ClientID%>');
        oVMWindows = document.getElementById('<%=chkVMWindows.ClientID%>');
        oVMLinux = document.getElementById('<%=chkVMLinux.ClientID%>');
        oBladesHP = document.getElementById('<%=chkBladesHP.ClientID%>');
        oBladesSUN = document.getElementById('<%=chkBladesSUN.ClientID%>');
        oAPV = document.getElementById('<%=chkAPV.ClientID%>');
        oMainframe = document.getElementById('<%=chkMainframe.ClientID%>');
        oCSM = document.getElementById('<%=chkCSM.ClientID%>');
        oCSMSOA = document.getElementById('<%=chkCSMSOA.ClientID%>');
        oReplicates = document.getElementById('<%=chkReplicates.ClientID%>');
        oPXE = document.getElementById('<%=chkPXE.ClientID%>');
        oILO = document.getElementById('<%=chkILO.ClientID%>');
        oCSMVIP = document.getElementById('<%=chkCSMVIP.ClientID%>');
        oLTMWeb = document.getElementById('<%=chkLTMWeb.ClientID%>');
        oLTMApp = document.getElementById('<%=chkLTMApp.ClientID%>');
        oLTMMiddle = document.getElementById('<%=chkLTMMiddle.ClientID%>');
        oLTMVIP = document.getElementById('<%=chkLTMVIP.ClientID%>');
        oWindowsCluster = document.getElementById('<%=chkWindowsCluster.ClientID%>');
        oUnixCluster = document.getElementById('<%=chkUnixCluster.ClientID%>');
        oAccenture = document.getElementById('<%=chkAccenture.ClientID%>');
        oHA = document.getElementById('<%=chkHA.ClientID%>');
        oSunCluster = document.getElementById('<%=chkSunCluster.ClientID%>');
        oSunSVE = document.getElementById('<%=chkSunSVE.ClientID%>');
        oStorage = document.getElementById('<%=chkStorage.ClientID%>');
        oDellWeb = document.getElementById('<%=chkDellWeb.ClientID%>');
        oDellMiddleware = document.getElementById('<%=chkDellMiddleware.ClientID%>');
        oDellDatabase = document.getElementById('<%=chkDellDatabase.ClientID%>');
        oDellFile = document.getElementById('<%=chkDellFile.ClientID%>');
        oDellMisc = document.getElementById('<%=chkDellMisc.ClientID%>');
        oDellUnder48 = document.getElementById('<%=chkDellUnder48.ClientID%>');
        oDellAvamar = document.getElementById('<%=chkDellAvamar.ClientID%>');
        oName = document.getElementById('<%=txtName.ClientID%>');
        oClass = document.getElementById('<%=ddlClass.ClientID%>');
        oEnvironment = document.getElementById('<%=ddlEnvironment.ClientID%>');
        oEnvironmentH = document.getElementById('<%=hdnEnvironment.ClientID%>');
        oParent = document.getElementById('<%=lblParent.ClientID%>');
        oParentId = document.getElementById('<%=hdnParent.ClientID%>');
        oResiliency = document.getElementById('<%=ddlResiliency.ClientID%>');
        oEnabled = document.getElementById('<%=chkEnabled.ClientID%>');
    }
</script>
<html>
<head>
<title>ClearView Administration</title>
<link rel="stylesheet" type="text/css" href="/css/default.css" />
<script type="text/javascript" src="/javascript/global.js"></script>
<script type="text/javascript" src="/javascript/both.js"></script>
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
		    <td><b>IP Address VLANs</b></td>
		    <td align="right"><a href="javascript:void(0);" onclick="Add();" class="cmlink" title="Click to Add New">Add New</a></td>
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
                        </tr>
                    </table>
                </div>

                <div id="divAdd" runat="server" style="display:none">
                    <table width="95%" cellpadding="3" cellspacing="0" border="0" align="center">
                        <tr> 
                            <td class="default" nowrap>VLAN:</td>
                            <td width="100%"><asp:textbox ID="txtVlan" CssClass="default" runat="server" Width="50" MaxLength="4"/></td>
                        </tr>
                        <tr bgcolor="#F6F6F6"> 
                            <td class="default" nowrap>Available for Physical Windows:</td>
                            <td width="100%"><asp:CheckBox ID="chkPhysicalW" runat="server" Checked="true" /></td>
                        </tr>
                        <tr> 
                            <td class="default" nowrap>Available for Physical UNIX:</td>
                            <td width="100%"><asp:CheckBox ID="chkPhysicalU" runat="server" Checked="true" /></td>
                        </tr>
                        <tr bgcolor="#F6F6F6"> 
                            <td class="default" nowrap>Available for Ecom Production Network:</td>
                            <td width="100%"><asp:CheckBox ID="chkEcomProd" runat="server" Checked="true" /></td>
                        </tr>
                        <tr> 
                            <td class="default" nowrap>Available for Ecom Service Network:</td>
                            <td width="100%"><asp:CheckBox ID="chkEcomService" runat="server" Checked="true" /></td>
                        </tr>
                        <tr bgcolor="#F6F6F6"> 
                            <td class="default" nowrap>Available for IPX:</td>
                            <td width="100%"><asp:CheckBox ID="chkIPX" runat="server" Checked="true" /></td>
                        </tr>
                        <tr> 
                            <td class="default" nowrap>Available for Virtual Workstations:</td>
                            <td width="100%"><asp:CheckBox ID="chkVirtual" runat="server" Checked="true" /></td>
                        </tr>
                        <tr bgcolor="#F6F6F6"> 
                            <td class="default" nowrap>Available for VMWare Workstations (External):</td>
                            <td width="100%"><asp:CheckBox ID="chkVMworkstationExternal" runat="server" Checked="true" /></td>
                        </tr>
                        <tr> 
                            <td class="default" nowrap>Available for VMWare Workstations (Internal):</td>
                            <td width="100%"><asp:CheckBox ID="chkVMworkstationInternal" runat="server" Checked="true" /></td>
                        </tr>
                        <tr bgcolor="#F6F6F6"> 
                            <td class="default" nowrap>Available for VMWare Workstations (DR):</td>
                            <td width="100%"><asp:CheckBox ID="chkVMworkstationDR" runat="server" Checked="true" /></td>
                        </tr>
                        <tr> 
                            <td class="default" nowrap>Available for VMWare Host:</td>
                            <td width="100%"><asp:CheckBox ID="chkVMHost" runat="server" Checked="true" /></td>
                        </tr>
                        <tr bgcolor="#F6F6F6"> 
                            <td class="default" nowrap>Available for VMWare VMotion (VMkernel):</td>
                            <td width="100%"><asp:CheckBox ID="chkVMotion" runat="server" Checked="true" /></td>
                        </tr>
                        <tr> 
                            <td class="default" nowrap>Available for VMWare Windows:</td>
                            <td width="100%"><asp:CheckBox ID="chkVMWindows" runat="server" Checked="true" /></td>
                        </tr>
                        <tr bgcolor="#F6F6F6"> 
                            <td class="default" nowrap>Available for VMWare Linux:</td>
                            <td width="100%"><asp:CheckBox ID="chkVMLinux" runat="server" Checked="true" /></td>
                        </tr>
                        <tr> 
                            <td class="default" nowrap>Available for HP Blades:</td>
                            <td width="100%"><asp:CheckBox ID="chkBladesHP" runat="server" Checked="true" /></td>
                        </tr>
                        <tr> 
                            <td class="default" nowrap>Available for SUN Blades:</td>
                            <td width="100%"><asp:CheckBox ID="chkBladesSUN" runat="server" Checked="true" /></td>
                        </tr>
                        <tr bgcolor="#F6F6F6"> 
                            <td class="default" nowrap>Available for APV:</td>
                            <td width="100%"><asp:CheckBox ID="chkAPV" runat="server" Checked="true" /></td>
                        </tr>
                        <tr> 
                            <td class="default" nowrap>Available for Mainframe:</td>
                            <td width="100%"><asp:CheckBox ID="chkMainframe" runat="server" Checked="true" /></td>
                        </tr>
                        <tr bgcolor="#F6F6F6"> 
                            <td class="default" nowrap>Available for CSM:</td>
                            <td width="100%"><asp:CheckBox ID="chkCSM" runat="server" Checked="true" /></td>
                        </tr>
                        <tr> 
                            <td class="default" nowrap>Available for CSM SOA:</td>
                            <td width="100%"><asp:CheckBox ID="chkCSMSOA" runat="server" Checked="true" /></td>
                        </tr>
                        <tr bgcolor="#F6F6F6"> 
                            <td class="default" nowrap>Available for Dalton DR (Replication):</td>
                            <td width="100%"><asp:CheckBox ID="chkReplicates" runat="server" Checked="true" /></td>
                        </tr>
                        <tr> 
                            <td class="default" nowrap>Available for PXE:</td>
                            <td width="100%"><asp:CheckBox ID="chkPXE" runat="server" Checked="true" /></td>
                        </tr>
                        <tr bgcolor="#F6F6F6"> 
                            <td class="default" nowrap>Available for ILO:</td>
                            <td width="100%"><asp:CheckBox ID="chkILO" runat="server" Checked="true" /></td>
                        </tr>
                        <tr> 
                            <td class="default" nowrap>Available for CSM VIP:</td>
                            <td width="100%"><asp:CheckBox ID="chkCSMVIP" runat="server" Checked="true" /></td>
                        </tr>
                        <tr bgcolor="#F6F6F6"> 
                            <td class="default" nowrap>Available for LTM - Web Tier:</td>
                            <td width="100%"><asp:CheckBox ID="chkLTMWeb" runat="server" Checked="true" /></td>
                        </tr>
                        <tr> 
                            <td class="default" nowrap>Available for LTM - App Tier:</td>
                            <td width="100%"><asp:CheckBox ID="chkLTMApp" runat="server" Checked="true" /></td>
                        </tr>
                        <tr bgcolor="#F6F6F6"> 
                            <td class="default" nowrap>Available for LTM - MiddleWare Tier:</td>
                            <td width="100%"><asp:CheckBox ID="chkLTMMiddle" runat="server" Checked="true" /></td>
                        </tr>
                        <tr> 
                            <td class="default" nowrap>Available for LTM VIP:</td>
                            <td width="100%"><asp:CheckBox ID="chkLTMVIP" runat="server" Checked="true" /></td>
                        </tr>
                        <tr bgcolor="#F6F6F6"> 
                            <td class="default" nowrap>Available for Windows Cluster:</td>
                            <td width="100%"><asp:CheckBox ID="chkWindowsCluster" runat="server" Checked="true" /></td>
                        </tr>
                        <tr> 
                            <td class="default" nowrap>Available for Unix Cluster:</td>
                            <td width="100%"><asp:CheckBox ID="chkUnixCluster" runat="server" Checked="true" /></td>
                        </tr>
                        <tr bgcolor="#F6F6F6"> 
                            <td class="default" nowrap>Available for Accenture:</td>
                            <td width="100%"><asp:CheckBox ID="chkAccenture" runat="server" Checked="true" /></td>
                        </tr>
                        <tr> 
                            <td class="default" nowrap>Available for HA:</td>
                            <td width="100%"><asp:CheckBox ID="chkHA" runat="server" Checked="true" /></td>
                        </tr>
                        <tr bgcolor="#F6F6F6"> 
                            <td class="default" nowrap>Available for Sun Clusters:</td>
                            <td width="100%"><asp:CheckBox ID="chkSunCluster" runat="server" Checked="true" /></td>
                        </tr>
                        <tr> 
                            <td class="default" nowrap>Available for Sun Virtual Environment (SVE):</td>
                            <td width="100%"><asp:CheckBox ID="chkSunSVE" runat="server" Checked="true" /></td>
                        </tr>
                        <tr bgcolor="#F6F6F6"> 
                            <td class="default" nowrap>Available for Storage Device:</td>
                            <td width="100%"><asp:CheckBox ID="chkStorage" runat="server" Checked="true" /></td>
                        </tr>
                        <tr> 
                            <td class="default" nowrap>Available for Dell Web ONLY:</td>
                            <td width="100%"><asp:CheckBox ID="chkDellWeb" runat="server" Checked="true" /></td>
                        </tr>
                        <tr bgcolor="#F6F6F6"> 
                            <td class="default" nowrap>Available for Dell Middleware ONLY:</td>
                            <td width="100%"><asp:CheckBox ID="chkDellMiddleware" runat="server" Checked="true" /></td>
                        </tr>
                        <tr> 
                            <td class="default" nowrap>Available for Dell Database ONLY:</td>
                            <td width="100%"><asp:CheckBox ID="chkDellDatabase" runat="server" Checked="true" /></td>
                        </tr>
                        <tr bgcolor="#F6F6F6"> 
                            <td class="default" nowrap>Available for Dell File ONLY:</td>
                            <td width="100%"><asp:CheckBox ID="chkDellFile" runat="server" Checked="true" /></td>
                        </tr>
                        <tr> 
                            <td class="default" nowrap>Available for Dell Misc ONLY:</td>
                            <td width="100%"><asp:CheckBox ID="chkDellMisc" runat="server" Checked="true" /></td>
                        </tr>
                        <tr bgcolor="#F6F6F6"> 
                            <td class="default" nowrap>Available for Dell Under 48 Hours (unchecked is Over 48 Hours):</td>
                            <td width="100%"><asp:CheckBox ID="chkDellUnder48" runat="server" Checked="true" /></td>
                        </tr>
                        <tr bgcolor="#F6F6F6"> 
                            <td class="default" nowrap>Available for Dell Avamar:</td>
                            <td width="100%"><asp:CheckBox ID="chkDellAvamar" runat="server" Checked="true" /></td>
                        </tr>
                        <tr> 
                            <td class="default" nowrap>Switch Name:</td>
                            <td width="100%"><asp:TextBox ID="txtName" CssClass="default" runat="server" Width="200" MaxLength="30"/></td>
                        </tr>
                        <tr> 
                            <td class="default" nowrap>Class:</td>
                            <td width="100%"><asp:dropdownlist ID="ddlClass" CssClass="default" runat="server" Width="300"/></td>
                        </tr>
                        <tr> 
                            <td class="default" nowrap>Environment:</td>
                            <td width="100%">
                                <asp:dropdownlist ID="ddlEnvironment" CssClass="default" runat="server" Enabled="false" Width="300">
                                    <asp:ListItem Value="-- Please select a Class --" />
                                </asp:dropdownlist>
                            </td>
                        </tr>
                        <tr>
                            <td class="default" nowrap>Address:</td>
                            <td width="100%"><asp:label ID="lblParent" CssClass="default" runat="server" />&nbsp;&nbsp;<asp:Button ID="btnParent" runat="server" CssClass="default" Width="25" Text="..." /></td>
                        </tr>
                        <tr> 
                            <td class="default" nowrap>Resiliency:</td>
                            <td width="100%"><asp:dropdownlist ID="ddlResiliency" CssClass="default" runat="server" Width="300"/></td>
                        </tr>
                        <tr> 
                            <td class="default" nowrap>Enabled:</td>
                            <td width="100%"><asp:CheckBox ID="chkEnabled" runat="server" Checked="true" /></td>
                        </tr>
                        <tr><td height="5" colspan="2">&nbsp;</td></tr>
                        <tr> 
                            <td>&nbsp;</td>
                            <td>
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
    <input type="hidden" id="hdnEnvironment" runat="server" />
    <input type="hidden" id="hdnOrder" runat="server" />
</form>
</body>
</html>
