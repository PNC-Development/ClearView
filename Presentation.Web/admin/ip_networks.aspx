<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ip_networks.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.ip_networks" %>

<script type="text/javascript">
    var oAdd = null;
    var oView = null;
    var oAddButton = null;
    var oDeleteButton = null;
    var oHidden = null;
    var oParent = null;
    var oParentId = null;
    var oMin1 = null;
    var oMin2 = null;
    var oMin3 = null;
    var oMin4 = null;
    var oMax1 = null;
    var oMax2 = null;
    var oMax3 = null;
    var oMax4 = null;
    var oMask = null;
    var oGateway = null;
    var oStarting = null;
    var oMaximum = null;
    var oReverse = null;
    var oRoutable = null;
    var oNotify = null;
    var oOnlyApps = null;
    var oOnlyComponennts = null;
    var oDescription = null;
    var oEnabled = null;
    function Edit(strId, strParentId, strParent, strAdd1, strAdd2, strAdd3, strMin4, strMax4, strMask, strGateway, strStarting, strMaximum, strReverse, strRoutable, strNotify, strOnlyApps, strOnlyComponents, strDescription, strEnabled) {
        oAdd.style.display = "inline";
        oView.style.display = "none";
        oAddButton.value = "Update";
        oDeleteButton.style.display = "inline";
        oHidden.value = strId;
        oParentId.value = strParentId;
        if (strParentId == "" || strParentId == "0")
            oParent.innerText = "None";
        else
            oParent.innerText = strParent;
        oMin1.value = strAdd1;
        oMin2.value = strAdd2;
        oMin3.value = strAdd3;
        oMin4.value = strMin4;
        oMax1.value = strAdd1;
        oMax2.value = strAdd2;
        oMax3.value = strAdd3;
        oMax4.value = strMax4;
        oMask.value = strMask;
        oGateway.value = strGateway;
        oStarting.value = strStarting;
        oMaximum.value = strMaximum;
        oReverse.checked = (strReverse == "1");
        oRoutable.checked = (strRoutable == "1");
        oNotify.value = strNotify;
        oOnlyApps.checked = (strOnlyApps == "1");
        oOnlyComponennts.checked = (strOnlyComponents == "1");
        oDescription.value = strDescription;
        oEnabled.checked = (strEnabled == "1");
    }
    function Add(strParentId, strParent) {
        oAdd.style.display = "inline";
        oView.style.display = "none";
        oAddButton.value = "Add";
        oDeleteButton.style.display = "none";
        oHidden.value = "0";
        oParentId.value = strParentId;
        if (oParentId == "" || strParentId == "0")
            oParent.innerText = "None";
        else
            oParent.innerText = strParent;
        oMin1.value = "";
        oMin2.value = "";
        oMin3.value = "";
        oMin4.value = "";
        oMax1.value = "";
        oMax2.value = "";
        oMax3.value = "";
        oMax4.value = "";
        oMask.value = "255.255.255.0";
        oGateway.value = "";
        oStarting.value = "0";
        oMaximum.value = "0";
        oReverse.checked = false;
        oRoutable.checked = false;
        oNotify.value = "";
        oOnlyApps.checked = false;
        oOnlyComponennts.checked = false;
        oDescription.value = "";
        oEnabled.checked = true;
        oMin1.focus();
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
        oParent = document.getElementById('<%=lblParent.ClientID%>');
        oParentId = document.getElementById('<%=hdnParent.ClientID%>');
        oMin1 = document.getElementById('<%=txtMin1.ClientID%>');
        oMin2 = document.getElementById('<%=txtMin2.ClientID%>');
        oMin3 = document.getElementById('<%=txtMin3.ClientID%>');
        oMin4 = document.getElementById('<%=txtMin4.ClientID%>');
        oMax1 = document.getElementById('<%=txtMax1.ClientID%>');
        oMax2 = document.getElementById('<%=txtMax2.ClientID%>');
        oMax3 = document.getElementById('<%=txtMax3.ClientID%>');
        oMax4 = document.getElementById('<%=txtMax4.ClientID%>');
        oMask = document.getElementById('<%=txtMask.ClientID%>');
        oGateway = document.getElementById('<%=txtGateway.ClientID%>');
        oStarting = document.getElementById('<%=txtStart.ClientID%>');
        oMaximum = document.getElementById('<%=txtMaximum.ClientID%>');
        oReverse = document.getElementById('<%=chkReverse.ClientID%>');
        oRoutable = document.getElementById('<%=chkRoutable.ClientID%>');
        oNotify = document.getElementById('<%=txtNotify.ClientID%>');
        oOnlyApps = document.getElementById('<%=chkOnlyApps.ClientID%>');
        oOnlyComponennts = document.getElementById('<%=chkOnlyComponennts.ClientID%>');
        oDescription = document.getElementById('<%=txtDescription.ClientID%>');
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
		    <td><b>IP Address Networks</b></td>
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
                            <td class="default">VLAN:</td>
                            <td><asp:label ID="lblParent" CssClass="default" runat="server" />&nbsp;&nbsp;<asp:Button ID="btnParent" runat="server" CssClass="default" Width="25" Text="..." /></td>
                        </tr>
                        <tr> 
                            <td class="default">Minimum Range:</td>
                            <td><asp:textbox ID="txtMin1" CssClass="default" runat="server" Width="50" MaxLength="3"/> <asp:textbox ID="txtMin2" CssClass="default" runat="server" Width="50" MaxLength="3"/> <asp:textbox ID="txtMin3" CssClass="default" runat="server" Width="50" MaxLength="3"/> <asp:textbox ID="txtMin4" CssClass="default" runat="server" Width="50" MaxLength="3"/></td>
                        </tr>
                        <tr> 
                            <td class="default">Maximum Range:</td>
                            <td><asp:textbox ID="txtMax1" CssClass="default" runat="server" Width="50" MaxLength="3" Enabled="false"/> <asp:textbox ID="txtMax2" CssClass="default" runat="server" Width="50" MaxLength="3" Enabled="false"/> <asp:textbox ID="txtMax3" CssClass="default" runat="server" Width="50" MaxLength="3" Enabled="false"/> <asp:textbox ID="txtMax4" CssClass="default" runat="server" Width="50" MaxLength="3"/></td>
                        </tr>
                        <tr> 
                            <td class="default">Subnet Mask:</td>
                            <td><asp:textbox ID="txtMask" CssClass="default" runat="server" Width="150" MaxLength="15"/></td>
                        </tr>
                        <tr> 
                            <td class="default">Gateway:</td>
                            <td><asp:textbox ID="txtGateway" CssClass="default" runat="server" Width="150" MaxLength="15"/></td>
                        </tr>
                        <tr> 
                            <td class="default">Starting Address:</td>
                            <td><asp:textbox ID="txtStart" CssClass="default" runat="server" Width="50" MaxLength="3" /></td>
                        </tr>
                        <tr> 
                            <td class="default">Maximum IPs:</td>
                            <td><asp:textbox ID="txtMaximum" CssClass="default" runat="server" Width="50" MaxLength="3" /></td>
                        </tr>
                        <tr> 
                            <td class="default">Reverse:</td>
                            <td><asp:CheckBox ID="chkReverse" runat="server" Checked="true" /> (check to assign addresses from 255 -> 1)</td>
                        </tr>
                        <tr> 
                            <td class="default">Routable:</td>
                            <td><asp:CheckBox ID="chkRoutable" runat="server" Checked="true" /></td>
                        </tr>
                        <tr> 
                            <td class="default">Notify when Assign:</td>
                            <td><asp:TextBox ID="txtNotify" runat="server" CssClass="default" Width="400" MaxLength="100" /> [full email address...separate with ;]</td>
                        </tr>
                        <tr> 
                            <td class="default">Applications ONLY:</td>
                            <td><asp:CheckBox ID="chkOnlyApps" runat="server" /> (check if this network should only be assigned for configured / selected server applications)</td>
                        </tr>
                        <tr> 
                            <td class="default">Components ONLY:</td>
                            <td><asp:CheckBox ID="chkOnlyComponennts" runat="server" /> (check if this network should only be assigned for configured / selected server components)</td>
                        </tr>
                        <tr> 
                            <td class="default" valign="top">Description:</td>
                            <td><asp:TextBox ID="txtDescription" runat="server" CssClass="default" TextMode="multiLine" Width="400" Rows="5" /></td>
                        </tr>
                        <tr> 
                            <td class="default">Enabled:</td>
                            <td><asp:CheckBox ID="chkEnabled" runat="server" Checked="true" /></td>
                        </tr>
                        <tr><td height="5" colspan="2"><img src="images/spacer.gif" width="1" height="5" /></td></tr>
                        <tr> 
                            <td class="cmdefault">&nbsp;</td>
                            <td><asp:Button ID="btnRelations" runat="server" Text="Network Relations" Width="175" CssClass="default" /></td>
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
    <input type="hidden" id="hdnOrder" runat="server" />
</form>
</body>
</html>
