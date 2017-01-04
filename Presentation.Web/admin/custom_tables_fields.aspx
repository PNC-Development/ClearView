<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="custom_tables_fields.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.custom_tables_fields" %>

<script type="text/javascript">
    var oAdd = null;
    var oView = null;
    var oAddButton = null;
    var oDeleteButton = null;
    var oHidden = null;
    var oFieldName = null;
    var oName = null;
    var oType = null;
    var oJoinTable = null;
    var oJoinOn = null;
    var oJoinField = null;
    var oParent = null;
    var oParentId = null;
    var oHide = null;
    var oEnabled = null;
    function Edit(strId, strFieldName, strName, strType, strJoinTable, strJoinOn, strJoinField, strParentId, strParent, strHidden, strEnabled) {
        oAdd.style.display = "inline";
        oView.style.display = "none";
        oAddButton.value = "Update";
        oDeleteButton.style.display = "inline";
        oHidden.value = strId;
        oFieldName.value = strFieldName;
        oName.value = strName;
        for (var ii=0; ii<oType.length; ii++) {
            if (oType.options[ii].value == strType)
                oType.selectedIndex = ii;
        }
        oJoinTable.value = strJoinTable;
        oJoinOn.value = strJoinOn;
        oJoinField.value = strJoinField;
        oParentId.value = strParentId;
        if (strParent == "" || strParentId == "0")
            oParent.innerText = "None";
        else
            oParent.innerText = strParent;
        oHide.checked = (strHidden == "1");
        oEnabled.checked = (strEnabled == "1");
    }
    function Add(strParentId, strParent) {
        oAdd.style.display = "inline";
        oView.style.display = "none";
        oAddButton.value = "Add";
        oDeleteButton.style.display = "none";
        oHidden.value = "0";
        oFieldName.value = "";
        oName.value = "";
        oType.selectedIndex = 0;
        oJoinTable.value = "";
        oJoinOn.value = "";
        oJoinField.value = "";
        oParentId.value = strParentId;
        if (strParent == "" || strParentId == "0")
            oParent.innerText = "None";
        else
            oParent.innerText = strParent;
        oHide.checked = false;
        oEnabled.checked = true;
        oFieldName.focus();
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
        oFieldName = document.getElementById('<%=txtFieldName.ClientID%>');
        oName = document.getElementById('<%=txtName.ClientID%>');
        oType = document.getElementById('<%=ddlType.ClientID%>');
        oJoinTable = document.getElementById('<%=txtJoinTable.ClientID%>');
        oJoinOn = document.getElementById('<%=txtJoinOn.ClientID%>');
        oJoinField = document.getElementById('<%=txtJoinField.ClientID%>');
        oParent = document.getElementById('<%=lblParent.ClientID%>');
        oParentId = document.getElementById('<%=hdnParent.ClientID%>');
        oHide = document.getElementById('<%=chkHidden.ClientID%>');
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
		    <td><b>Custom Table Fields</b></td>
		    <td align="right"><a href="javascript:void(0);" onclick="Add(0);" class="cmlink" title="Click to Add New">Add New</a></td>
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
                            <td class="default">Table:</td>
                            <td><asp:label ID="lblParent" CssClass="default" runat="server" />&nbsp;&nbsp;<asp:Button ID="btnParent" runat="server" CssClass="default" Width="25" Text="..." /></td>
                        </tr>
                        <tr> 
                            <td class="default">Field Name:</td>
                            <td><asp:textbox ID="txtFieldName" CssClass="default" runat="server" Width="150" MaxLength="50"/></td>
                        </tr>
                        <tr> 
                            <td class="default">Name:</td>
                            <td><asp:textbox ID="txtName" CssClass="default" runat="server" Width="300" MaxLength="100"/></td>
                        </tr>
                        <tr>
                            <td class="default">Data Type:</td>
                            <td>
                                <asp:dropdownlist ID="ddlType" CssClass="default" runat="server">
                                    <asp:ListItem Text="String" Value="S" />
                                    <asp:ListItem Text="Float" Value="F" />
                                    <asp:ListItem Text="Date" Value="D" />
                                    <asp:ListItem Text="Time" Value="T" />
                                    <asp:ListItem Text="DateTime" Value="DT" />
                                    <asp:ListItem Text="UserID" Value="U" />
                                    <asp:ListItem Text="Priority" Value="P" />
                                    <asp:ListItem Text="True / False" Value="TF" />
                                    <asp:ListItem Text="Yes / No" Value="YN" />
                                    <asp:ListItem Text="Join" Value="J" />
                                </asp:dropdownlist>
                            </td>
                        </tr>
                        <tr> 
                            <td class="default">Join:</td>
                            <td>SELECT <asp:textbox ID="txtJoinField" CssClass="default" runat="server" Width="100" MaxLength="30"/> FROM <asp:textbox ID="txtJoinTable" CssClass="default" runat="server" Width="150" MaxLength="50"/> WHERE <asp:textbox ID="txtJoinOn" CssClass="default" runat="server" Width="100" MaxLength="30"/> = xxx</td>
                        </tr>
                        <tr> 
                            <td class="default">Hidden:</td>
                            <td><asp:CheckBox ID="chkHidden" runat="server" Checked="true" /></td>
                        </tr>
                        <tr> 
                            <td class="default">Enabled:</td>
                            <td><asp:CheckBox ID="chkEnabled" runat="server" Checked="true" /></td>
                        </tr>
                        <tr> 
                            <td class="cmdefault">&nbsp;</td>
                            <td><asp:Button ID="btnOrder" runat="server" Text="Change Order" Width="150" CssClass="default" /></td>
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
