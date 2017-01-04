<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="datacenters.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.datacenters" %>

<script type="text/javascript">
    var oAdd = null;
    var oView = null;
    var oAddButton = null;
    var oDeleteButton = null;
    var oHidden = null;
    var oName = null;
    var oParent = null;
    var oBuildFolder = null;
    var oDesktopNetwork = null;
    var oWorkstationDecomVLAN = null;
    var oEnabled = null;
    function Edit(strId, strName, strParent, strBuildFolder, strDesktopNetwork, strWorkstationDecomVLAN, strEnabled) {
        oAdd.style.display = "inline";
        oView.style.display = "none";
        oAddButton.value = "Update";
        oDeleteButton.style.display = "inline";
        oHidden.value = strId;
        oName.value = strName;
        for (var ii=0; ii<oParent.length; ii++) {
            if (oParent.options[ii].value == strParent)
                oParent.selectedIndex = ii;
        }
        oBuildFolder.value = strBuildFolder;
        oDesktopNetwork.value = strDesktopNetwork;
        oWorkstationDecomVLAN.value = strWorkstationDecomVLAN;
        oEnabled.checked = (strEnabled == "1");
    }
    function Add(strParent) {
        oAdd.style.display = "inline";
        oView.style.display = "none";
        oAddButton.value = "Add";
        oDeleteButton.style.display = "none";
        oHidden.value = "0";
        oName.value = "";
        for (var ii=0; ii<oParent.length; ii++) {
            if (oParent.options[ii].value == strParent)
                oParent.selectedIndex = ii;
        }
        oBuildFolder.value = "";
        oDesktopNetwork.value = "";
        oWorkstationDecomVLAN.value = "";
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
        oParent = document.getElementById('<%=ddlParent.ClientID%>');
        oBuildFolder = document.getElementById('<%=txtBuildFolder.ClientID%>');
        oDesktopNetwork = document.getElementById('<%=txtDesktopNetwork.ClientID%>');
        oWorkstationDecomVLAN = document.getElementById('<%=txtWorkstationDecomVLAN.ClientID%>');
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
		    <td><b>Datacenters</b></td>
		    <td align="right"></td>
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
                            <td class="default" width="100px">Virtual Center:</td>
                            <td><asp:DropDownList ID="ddlParent" CssClass="default" runat="server"/></td>
                        </tr>
                        <tr> 
                            <td class="default" width="100px">Name:</td>
                            <td><asp:textbox ID="txtName" CssClass="default" runat="server" Width="200" MaxLength="50"/>&nbsp;&nbsp;&nbsp;<span class="reddefault"><b>NOTE:</b> This field IS CASE SENSITIVE.</span></td>
                        </tr>
                        <tr> 
                            <td class="default" width="100px">Build Folder:</td>
                            <td><asp:textbox ID="txtBuildFolder" CssClass="default" runat="server" Width="200" MaxLength="100"/>&nbsp;&nbsp;&nbsp;<span class="reddefault"><b>NOTE:</b> This field IS CASE SENSITIVE.</span></td>
                        </tr>
                        <tr> 
                            <td class="default" width="100px">Desktop Network:</td>
                            <td><asp:textbox ID="txtDesktopNetwork" CssClass="default" runat="server" Width="200" MaxLength="100"/>&nbsp;&nbsp;&nbsp;<span class="reddefault"><b>NOTE:</b> This field IS CASE SENSITIVE.</span></td>
                        </tr>
                        <tr> 
                            <td class="default" width="100px">Workstation Decom VLAN:</td>
                            <td><asp:textbox ID="txtWorkstationDecomVLAN" CssClass="default" runat="server" Width="200" MaxLength="100"/>&nbsp;&nbsp;&nbsp;<span class="reddefault"><b>NOTE:</b> This field IS CASE SENSITIVE.</span></td>
                        </tr>
                        <tr> 
                            <td width="100px"></td>
                            <td class="footer">Example: QA_Build</td>
                        </tr>
                        <tr> 
                            <td class="default">Enabled:</td>
                            <td><asp:CheckBox ID="chkEnabled" runat="server" Checked="true" /></td>
                        </tr>
                        <tr> 
                            <td class="cmdefault">&nbsp;</td>
                            <td><asp:Button ID="btnLocations" runat="server" Text="Configure Locations" Width="150" CssClass="default" /></td>
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
</form>
</body>
</html>