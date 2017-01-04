<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ip_address.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.ip_address" %>

<script type="text/javascript">
    var oAdd = null;
    var oView = null;
    var oAddButton = null;
    var oDeleteButton = null;
    var oHidden = null;
    var oParent = null;
    var oParentId = null;
    var oAdd1 = null;
    var oAdd2 = null;
    var oAdd3 = null;
    var oAdd4 = null;
    var oDHCP = null;
    var oAvailable = null;
    function Edit(strId, strParentId, strParent, strAdd1, strAdd2, strAdd3, strAdd4, strDHCP, strAvailable) {
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
        oAdd1.value = strAdd1;
        oAdd2.value = strAdd2;
        oAdd3.value = strAdd3;
        oAdd4.value = strAdd4;
        oDHCP.checked = (strDHCP == "1");
        oAvailable.checked = (strAvailable == "1");
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
        oAdd1 = document.getElementById('<%=txtAdd1.ClientID%>');
        oAdd2 = document.getElementById('<%=txtAdd2.ClientID%>');
        oAdd3 = document.getElementById('<%=txtAdd3.ClientID%>');
        oAdd4 = document.getElementById('<%=txtAdd4.ClientID%>');
        oDHCP = document.getElementById('<%=chkDHCP.ClientID%>');
        oAvailable = document.getElementById('<%=chkAvailable.ClientID%>');
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
		    <td><b>IP Addresses</b></td>
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
                            <td class="default">Network:</td>
                            <td><asp:label ID="lblParent" CssClass="default" runat="server" />&nbsp;&nbsp;<asp:Button ID="btnParent" runat="server" CssClass="default" Width="25" Text="..." /></td>
                        </tr>
                        <tr> 
                            <td class="default">Address:</td>
                            <td><asp:textbox ID="txtAdd1" CssClass="default" runat="server" Width="50" MaxLength="3"/> <asp:textbox ID="txtAdd2" CssClass="default" runat="server" Width="50" MaxLength="3"/> <asp:textbox ID="txtAdd3" CssClass="default" runat="server" Width="50" MaxLength="3"/> <asp:textbox ID="txtAdd4" CssClass="default" runat="server" Width="50" MaxLength="3"/></td>
                        </tr>
                        <tr> 
                            <td class="default">DHCP:</td>
                            <td><asp:CheckBox ID="chkDHCP" runat="server" Checked="true" /></td>
                        </tr>
                        <tr> 
                            <td class="default">Available:</td>
                            <td><asp:CheckBox ID="chkAvailable" runat="server" Checked="true" /></td>
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
