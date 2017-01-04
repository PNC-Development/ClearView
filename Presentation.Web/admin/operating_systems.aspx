<%@ Page Language="C#" AutoEventWireup="True" CodeBehind="operating_systems.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.operating_systems" %>

<script type="text/javascript">
    var oAdd = null;
    var oView = null;
    var oAddButton = null;
    var oDeleteButton = null;
    var oHidden = null;
    var oName = null;
    var oCluster = null;
    var oWorkstation = null;
    var oCode = null;
    var oFactoryCode = null;
    var oCitrixConfig = null;
    var oZEUSOs = null;
    var oZEUSOsVersion = null;
    var oZEUSBuildType = null;
    var oZEUSBuildTypePNC = null;
    var oVMwareOS = null;
    var oBootEnvironment = null;
    var oTaskSequence = null;
    var oAltiris = null;
    var oLinux = null;
    var oAix = null;
    var oSolaris = null;
    var oWindows = null;
    var oWindows2008 = null;
    var oRDPAltiris = null;
    var oRDPMDT = null;
    var oE1000 = null;
    var oManualBuild = null;
    var oDefaultSP = null;
    var oStandard = null;
    var oEnabled = null;
    function Edit(strId, strName, strCluster, strWorkstation, strCode, strFactoryCode, strCitrixConfig, strZEUSOs, strZEUSOsVersion, strZEUSBuildType, strZEUSBuildTypePNC, strVMwareOS, strBootEnvironment, strTaskSequence, strAltiris, strLinux, strAix, strSolaris, strWindows, strWindows2008, strRDPAltiris, strRDPMDT, strE1000, strManualBuild, strDefaultSP, strStandard, strEnabled) {
        oAdd.style.display = "inline";
        oView.style.display = "none";
        oAddButton.value = "Update";
        oDeleteButton.style.display = "inline";
        oHidden.value = strId;
        oName.value = strName;
        oCluster.checked = (strCluster == "1");
        oWorkstation.checked = (strWorkstation == "1");
        oCode.value = strCode;
        oFactoryCode.value = strFactoryCode;
        oCitrixConfig.value = strCitrixConfig;
        oZEUSOs.value = strZEUSOs;
        oZEUSOsVersion.value = strZEUSOsVersion;
        oZEUSBuildType.value = strZEUSBuildType;
        oZEUSBuildTypePNC.value = strZEUSBuildTypePNC;
        oVMwareOS.value = strVMwareOS;
        oBootEnvironment.value = strBootEnvironment;
        oTaskSequence.value = strTaskSequence;
        oAltiris.value = strAltiris;
        oLinux.checked = (strLinux == "1");
        oAix.checked = (strAix == "1");
        oSolaris.checked = (strSolaris == "1");
        oWindows.checked = (strWindows == "1");
        oWindows2008.checked = (strWindows2008 == "1");
        oRDPAltiris.checked = (strRDPAltiris == "1");
        oRDPMDT.checked = (strRDPMDT == "1");
        oE1000.checked = (strE1000 == "1");
        oManualBuild.checked = (strManualBuild == "1");
        oDefaultSP.selectedIndex = 0;
        for (var ii=0; ii<oDefaultSP.length; ii++) {
            if (oDefaultSP.options[ii].value == strDefaultSP)
                oDefaultSP.selectedIndex = ii;
        }
        oStandard.checked = (strStandard == "1");
        oEnabled.checked = (strEnabled == "1");
    }
    function Add() {
        oAdd.style.display = "inline";
        oView.style.display = "none";
        oAddButton.value = "Add";
        oDeleteButton.style.display = "none";
        oHidden.value = "0";
        oName.value = "";
        oCluster.checked = false;
        oWorkstation.checked = false;
        oCode.value = "";
        oFactoryCode.value = "";
        oCitrixConfig.value = "";
        oZEUSOs.value = "";
        oZEUSOsVersion.value = "";
        oZEUSBuildType.value = "";
        oZEUSBuildTypePNC.value = "";
        oVMwareOS.value = "";
        oBootEnvironment.value = "";
        oTaskSequence.value = "";
        oAltiris.value = "";
        oLinux.checked = false;
        oAix.checked = false;
        oSolaris.checked = false;
        oWindows.checked = false;
        oWindows2008.checked = false;
        oRDPAltiris.checked = false;
        oRDPMDT.checked = false;
        oE1000.checked = false;
        oManualBuild.checked = false;
        oDefaultSP.selectedIndex = 0;
        oStandard.checked = true;
        oEnabled.checked = true;
        oName.focus();
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
        oName = document.getElementById('<%=txtName.ClientID%>');
        oCluster = document.getElementById('<%=chkCluster.ClientID%>');
        oWorkstation = document.getElementById('<%=chkWorkstation.ClientID%>');
        oCode = document.getElementById('<%=txtCode.ClientID%>');
        oFactoryCode = document.getElementById('<%=txtFactoryCode.ClientID%>');
        oCitrixConfig = document.getElementById('<%=txtCitrixConfig.ClientID%>');
        oZEUSOs = document.getElementById('<%=txtZEUSOs.ClientID%>');
        oZEUSOsVersion = document.getElementById('<%=txtZEUSOsVersion.ClientID%>');
        oZEUSBuildType = document.getElementById('<%=txtZEUSBuildType.ClientID%>');
        oZEUSBuildTypePNC = document.getElementById('<%=txtZEUSBuildTypePNC.ClientID%>');
        oVMwareOS = document.getElementById('<%=txtVMware.ClientID%>');
        oBootEnvironment = document.getElementById('<%=txtBootEnvironment.ClientID%>');
        oTaskSequence = document.getElementById('<%=txtTaskSequence.ClientID%>');
        oAltiris = document.getElementById('<%=txtAltiris.ClientID%>');
        oLinux = document.getElementById('<%=chkLinux.ClientID%>');
        oAix = document.getElementById('<%=chkAix.ClientID%>');
        oSolaris = document.getElementById('<%=chkSolaris.ClientID%>');
        oWindows = document.getElementById('<%=chkWindows.ClientID%>');
        oWindows2008 = document.getElementById('<%=chkWindows2008.ClientID%>');
        oRDPAltiris = document.getElementById('<%=radRDPAltiris.ClientID%>');
        oRDPMDT = document.getElementById('<%=radRDPMDT.ClientID%>');
        oE1000 = document.getElementById('<%=chkE1000.ClientID%>');
        oManualBuild = document.getElementById('<%=chkManualBuild.ClientID%>');
        oDefaultSP = document.getElementById('<%=ddlDefaultSP.ClientID%>');
        oStandard = document.getElementById('<%=chkStandard.ClientID%>');
        oEnabled = document.getElementById('<%=chkEnabled.ClientID%>');
    }
</script>
<html>
<head>
<title>ClearView Administration</title>
<link rel="stylesheet" type="text/css" href="/css/default.css" />
<script type="text/javascript" src="/javascript/global.js"></script>
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
		    <td><b>Operating Systems</b></td>
		    <td align="right"><a href="javascript:void(0);" onclick="Add();" class="cmlink" title="Click to Add New">Add New</a></td>
		</tr>
		<tr><td colspan="2">&nbsp;</td></tr>
		<tr>
		    <td colspan="2" align="center">
                <div ID="divView" runat="server">
                    <table width="100%" border="0" cellpadding="0" cellspacing="0">
                        <tr>
                            <td>
                                <asp:repeater ID="rptView" runat="server">
                                    <HeaderTemplate>
                                        <table width="100%" cellpadding="3" cellspacing="0" border="0" align="center">
                                            <tr bgcolor="#CCCCCC">
                                                <td class="bold">&nbsp;</td>
                                                <td width="50%" class="bold"><asp:linkbutton ID="lnkName" Text="Name" CommandArgument="name" CssClass="bold" OnClick="OrderView" runat="server"/></td>
                                                <td width="50%" class="bold"><asp:linkbutton ID="lnkModified" Text="Last Modified" CommandArgument="modified" CssClass="bold" OnClick="OrderView" runat="server"/></td>
                                                <td class="bold" align="center"><asp:linkbutton ID="lnkEnabled" Text="Enabled" CommandArgument="enabled" CssClass="bold" OnClick="OrderView" runat="server"/></td>
                                            </tr>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr bgcolor="#EFEFEF" onmouseover="AdminMouseOver(this);" onmouseout="AdminMouseOut(this);" onclick="if(event.srcElement.tagName != 'INPUT')Edit('<%# DataBinder.Eval(Container.DataItem, "id") %>','<%# DataBinder.Eval(Container.DataItem, "name") %>','<%# DataBinder.Eval(Container.DataItem, "cluster_name") %>','<%# DataBinder.Eval(Container.DataItem, "workstation") %>','<%# DataBinder.Eval(Container.DataItem, "code") %>','<%# DataBinder.Eval(Container.DataItem, "factory_code") %>','<%# DataBinder.Eval(Container.DataItem, "citrix_config") %>','<%# DataBinder.Eval(Container.DataItem, "zeus_os") %>','<%# DataBinder.Eval(Container.DataItem, "zeus_os_version") %>','<%# DataBinder.Eval(Container.DataItem, "zeus_build_type") %>','<%# DataBinder.Eval(Container.DataItem, "zeus_build_type_pnc") %>','<%# DataBinder.Eval(Container.DataItem, "vmware_os") %>','<%# DataBinder.Eval(Container.DataItem, "boot_environment") %>','<%# DataBinder.Eval(Container.DataItem, "task_sequence") %>','<%# DataBinder.Eval(Container.DataItem, "altiris") %>','<%# DataBinder.Eval(Container.DataItem, "linux") %>','<%# DataBinder.Eval(Container.DataItem, "aix") %>','<%# DataBinder.Eval(Container.DataItem, "solaris") %>','<%# DataBinder.Eval(Container.DataItem, "windows") %>','<%# DataBinder.Eval(Container.DataItem, "windows2008") %>','<%# DataBinder.Eval(Container.DataItem, "rdp_altiris") %>','<%# DataBinder.Eval(Container.DataItem, "rdp_mdt") %>','<%# DataBinder.Eval(Container.DataItem, "e1000") %>','<%# DataBinder.Eval(Container.DataItem, "manual_build") %>','<%# DataBinder.Eval(Container.DataItem, "default_sp") %>','<%# DataBinder.Eval(Container.DataItem, "standard") %>','<%# DataBinder.Eval(Container.DataItem, "enabled") %>');" class="default">
                                            <td width="5%" align="left">&nbsp;<asp:ImageButton ID="btnDelete" OnClick="btnDeleteLink_Click" runat="server" ToolTip="Delete" ImageAlign="AbsMiddle" ImageUrl="/admin/images/delete.gif" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' /></td>
                                            <td width="50%">&nbsp;<%# DataBinder.Eval(Container.DataItem, "name") %></td>
                                            <td width="50%">&nbsp;<%# DataBinder.Eval(Container.DataItem, "modified") %></td>
                                            <td align="center"><asp:ImageButton ID="btnEnable" runat="server" ImageAlign="AbsMiddle" ImageUrl='<%# (DataBinder.Eval(Container.DataItem, "enabled").ToString() == "1") ? "/admin/images/enabled.gif" : "/admin/images/disabled.gif" %>' CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' OnClick="btnEnable_Click" /></td>
                                        </tr>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </table>
                                    </FooterTemplate>
                                </asp:repeater>
                            </td>
                        </tr>
                    </table>
                </div>

                <div id="divAdd" runat="server" style="display:none">
                    <table width="95%" cellpadding="3" cellspacing="0" border="0" align="center">
                        <tr> 
                            <td class="default">Name:</td>
                            <td><asp:textbox ID="txtName" CssClass="default" runat="server" Width="300" MaxLength="100"/></td>
                        </tr>
                        <tr>
                            <td class="default">Default Service Pack:</td>
                            <td><asp:dropdownlist ID="ddlDefaultSP" CssClass="default" runat="server"/></td>
                        </tr>
                        <tr> 
                            <td class="default">Cluster Name:</td>
                            <td><asp:CheckBox ID="chkCluster" runat="server" /></td>
                        </tr>
                        <tr> 
                            <td class="default">Workstation:</td>
                            <td><asp:CheckBox ID="chkWorkstation" runat="server" /></td>
                        </tr>
                        <tr> 
                            <td class="default">Code:</td>
                            <td><asp:textbox ID="txtCode" CssClass="default" runat="server" Width="30" MaxLength="2"/></td>
                        </tr>
                        <tr> 
                            <td class="default">Factory Code:</td>
                            <td><asp:textbox ID="txtFactoryCode" CssClass="default" runat="server" Width="30" MaxLength="2"/></td>
                        </tr>
                        <tr> 
                            <td class="default">Citrix Config:</td>
                            <td><asp:textbox ID="txtCitrixConfig" CssClass="default" runat="server" Width="50" MaxLength="10"/></td>
                        </tr>
                        <tr> 
                            <td class="default">ZEUS OS:</td>
                            <td><asp:textbox ID="txtZEUSOs" CssClass="default" runat="server" Width="100" MaxLength="20"/></td>
                        </tr>
                        <tr> 
                            <td class="default">ZEUS OS Version:</td>
                            <td><asp:textbox ID="txtZEUSOsVersion" CssClass="default" runat="server" Width="100" MaxLength="20"/></td>
                        </tr>
                        <tr> 
                            <td class="default">ZEUS Build Type:</td>
                            <td><asp:textbox ID="txtZEUSBuildType" CssClass="default" runat="server" Width="150" MaxLength="50"/></td>
                        </tr>
                        <tr> 
                            <td class="default">ZEUS Build Type (PNC):</td>
                            <td><asp:textbox ID="txtZEUSBuildTypePNC" CssClass="default" runat="server" Width="150" MaxLength="50"/></td>
                        </tr>
                        <tr> 
                            <td class="default">VMware OS:</td>
                            <td><asp:textbox ID="txtVMware" CssClass="default" runat="server" Width="200" MaxLength="50"/></td>
                        </tr>
                        <tr> 
                            <td class="default">Boot Environment:</td>
                            <td><asp:textbox ID="txtBootEnvironment" CssClass="default" runat="server" Width="200" MaxLength="50"/></td>
                        </tr>
                        <tr> 
                            <td class="default">Task Sequence:</td>
                            <td><asp:textbox ID="txtTaskSequence" CssClass="default" runat="server" Width="200" MaxLength="50"/></td>
                        </tr>
                        <tr> 
                            <td class="default">Altiris Image:</td>
                            <td><asp:textbox ID="txtAltiris" CssClass="default" runat="server" Width="200" MaxLength="50"/></td>
                        </tr>
                        <tr> 
                            <td class="default">Type:</td>
                            <td>
                                <table cellpadding="2" cellspacing="1" border="0">
                                    <tr>
                                        <td><asp:RadioButton ID="chkLinux" runat="server" CssClass="default" Text="Linux" GroupName="type" /></td>
                                        <td><asp:RadioButton ID="chkAix" runat="server" CssClass="default" Text="AIX" GroupName="type" /></td>
                                        <td><asp:RadioButton ID="chkSolaris" runat="server" CssClass="default" Text="Solaris" GroupName="type" /></td>
                                        <td><asp:RadioButton ID="chkWindows" runat="server" CssClass="default" Text="Windows (not 2008)" GroupName="type" /></td>
                                        <td><asp:RadioButton ID="chkWindows2008" runat="server" CssClass="default" Text="Windows 2008" GroupName="type" /></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr> 
                            <td class="default">RDP:</td>
                            <td>
                                <table cellpadding="2" cellspacing="1" border="0">
                                    <tr>
                                        <td><asp:RadioButton ID="radRDPAltiris" runat="server" CssClass="default" Text="Altiris" GroupName="rdp" /></td>
                                        <td><asp:RadioButton ID="radRDPMDT" runat="server" CssClass="default" Text="MDT" GroupName="rdp" /></td>
                                        <td><asp:RadioButton ID="radRDP" runat="server" CssClass="default" Text="None" GroupName="rdp" /></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr> 
                            <td class="default">Use E1000 VMware NIC:</td>
                            <td><asp:CheckBox ID="chkE1000" runat="server" /></td>
                        </tr>
                        <tr> 
                            <td class="default">Manual Build:</td>
                            <td><asp:CheckBox ID="chkManualBuild" runat="server" /></td>
                        </tr>
                        <tr> 
                            <td class="default">Standard:</td>
                            <td><asp:CheckBox ID="chkStandard" runat="server" Checked="true" /></td>
                        </tr>
                        <tr> 
                            <td class="default">Enabled:</td>
                            <td><asp:CheckBox ID="chkEnabled" runat="server" Checked="true" /></td>
                        </tr>
                        <tr> 
                            <td class="cmdefault">&nbsp;</td>
                            <td><asp:Button ID="btnOrder" runat="server" Text="Change Order" Width="150" CssClass="default" /></td>
                        </tr>
                        <tr> 
                            <td class="cmdefault">&nbsp;</td>
                            <td><asp:Button ID="btnServicePacks" runat="server" Text="Change Service Packs" Width="150" CssClass="default" /></td>
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
    <input type="hidden" id="hdnOrder" runat="server" />
</form>
</body>
</html>
