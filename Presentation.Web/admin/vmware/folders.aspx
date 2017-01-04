<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="folders.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.folders" %>

<script type="text/javascript">
    var oAdd = null;
    var oView = null;
    var oAddButton = null;
    var oDeleteButton = null;
    var oHidden = null;
    var oName = null;
    var oNotification = null;
    var oParent = null;
    var oClass = null;
    var oEnvironment = null;
    var oEnvironmentH = null;
    var oLocation = null;
    var oLocationH = null;
    var oEnabled = null;
    function Edit(strId, strParent, strName, strNotification, strClass, strEnvironment, strState, strCity, strAddress, strEnabled) {
        oAdd.style.display = "inline";
        oView.style.display = "none";
        oAddButton.value = "Update";
        oDeleteButton.style.display = "inline";
        oHidden.value = strId;
        for (var ii=0; ii<oParent.length; ii++) {
            if (oParent.options[ii].value == strParent)
                oParent.selectedIndex = ii;
        }
        oName.value = strName;
        oNotification.value = strNotification;
        var boolClassFound = false;
        for (var ii=0; ii<oClass.length; ii++) {
            if (oClass.options[ii].value == strClass) {
                oClass.selectedIndex = ii;
                boolClassFound = true;
                break;
            }
        }
        if (boolClassFound == false)
            oClass.selectedIndex = 0;
        PopulateEnvironments2(oClass, oEnvironment);
        for (var ii=0; ii<oEnvironment.length; ii++) {
            if (oEnvironment.options[ii].value == strEnvironment) {
                oEnvironment.selectedIndex = ii;
                break;
            }
        }
        oEnvironmentH.value = strEnvironment;
        LoadAddresses("ddlState", "ddlCity", "ddlAddress", "ddlCommon", oLocationH, strState, strCity, strAddress);
        oEnabled.checked = (strEnabled == "1");
    }
    function Add(strParent) {
        oAdd.style.display = "inline";
        oView.style.display = "none";
        oAddButton.value = "Add";
        oDeleteButton.style.display = "none";
        oHidden.value = "0";
        for (var ii=0; ii<oParent.length; ii++) {
            if (oParent.options[ii].value == strParent)
                oParent.selectedIndex = ii;
        }
        oName.value = "";
        oNotification.value = "";
        oClass.selectedIndex = 0;
        oEnvironment.disabled = true;
		var oOption = document.createElement("OPTION");
		oEnvironment.add(oOption);
		oOption.text = " -- Please select a Class --";
		oOption.value = "0";
        oEnvironment.selectedIndex = 0;
        oEnvironmentH.value = "0";
        oEnabled.checked = true;
        oLocation.selectedIndex = 0;
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
        oParent = document.getElementById('<%=ddlParent.ClientID%>');
        oName = document.getElementById('<%=txtName.ClientID%>');
        oNotification = document.getElementById('<%=txtNotification.ClientID%>');
        oClass = document.getElementById('<%=ddlClass.ClientID%>');
        oEnvironment = document.getElementById('<%=ddlEnvironment.ClientID%>');
        oEnvironmentH = document.getElementById('<%=hdnEnvironment.ClientID%>');
        oLocation = document.getElementById('ddlCommon');
        oLocationH = document.getElementById('<%=hdnLocation.ClientID%>');
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
		    <td><b>Folders</b></td>
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
                            <td class="default" width="100px">Datacenter:</td>
                            <td><asp:DropDownList ID="ddlParent" CssClass="default" runat="server"/></td>
                        </tr>
                        <tr> 
                            <td class="default" width="100px">Name:</td>
                            <td><asp:textbox ID="txtName" CssClass="default" runat="server" Width="200" MaxLength="50"/>&nbsp;&nbsp;&nbsp;<span class="reddefault"><b>NOTE:</b> This field IS CASE SENSITIVE.</span></td>
                        </tr>
                        <tr> 
                            <td class="default" width="100px">Notification:</td>
                            <td>Notify <asp:textbox ID="txtNotification" CssClass="default" runat="server" Width="300" MaxLength="100"/> when a cluster has reached 8 hosts and a new cluster should be provisioned</td>
                        </tr>
                        <tr> 
                            <td class="default" width="100px"></td>
                            <td class="reddefault"><img src="/images/spacer.gif" border="0" width="50" height="1" /><img src="/images/down_right.gif" border="0" align="absmiddle" /><b>NOTE:</b> For the notifications, please use LAN ID with a semi-colon separating the users</td>
                        </tr>
                        <tr>
                            <td class="default">Build Location:</td>
                            <td><%=strLocation %></td>
                        </tr>
                        <tr>
                            <td class="default">Class:</td>
                            <td><asp:DropDownList ID="ddlClass" CssClass="default" runat="server" Width="200" /></td>
                        </tr>
                        <tr>
                            <td class="default">Environment:</td>
                            <td>
                                <asp:DropDownList ID="ddlEnvironment" CssClass="default" runat="server" Width="200" Enabled="false" >
                                    <asp:ListItem Value="-- Please select a Class --" />
                                </asp:DropDownList>
                            </td>
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
    <input type="hidden" id="hdnLocation" runat="server" />
    <input type="hidden" id="hdnEnvironment" runat="server" />
</form>
</body>
</html>

