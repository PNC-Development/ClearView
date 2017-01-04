<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="platform_forms.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.platform_forms" %>

<script type="text/javascript">
    var oAdd = null;
    var oView = null;
    var oAddButton = null;
    var oDeleteButton = null;
    var oHidden = null;
    var oName = null;
    var oParent = null;
    var oParentId = null;
    var oPath = null;
    var oMax1 = null;
    var oMax2 = null;
    var oMax3 = null;
    var oMax4 = null;
    var oMax5 = null;
    var oEnabled = null;
    function Edit(strId, strParent, strParentId, strName, strPath, strMax1, strMax2, strMax3, strMax4, strMax5, strEnabled) {
        oAdd.style.display = "inline";
        oView.style.display = "none";
        oAddButton.value = "Update";
        oDeleteButton.style.display = "inline";
        oHidden.value = strId;
        oName.value = strName;
        oParentId.value = strParentId;
        if (strParent == "" || strParentId == "0")
            oParent.innerText = "None";
        else
            oParent.innerText = strParent;
        oPath.value = strPath;
        oMax1.value = strMax1;
        oMax2.value = strMax2;
        oMax3.value = strMax3;
        oMax4.value = strMax4;
        oMax5.value = strMax5;
        oEnabled.checked = (strEnabled == "1");
    }
    function Add(strParentId, strParent) {
        oAdd.style.display = "inline";
        oView.style.display = "none";
        oAddButton.value = "Add";
        oDeleteButton.style.display = "none";
        oHidden.value = "0";
        oName.value = "";
        oParentId.value = strParentId;
        if (strParent == "" || strParentId == "0")
            oParent.innerText = "None";
        else
            oParent.innerText = strParent;
        oPath.value = "";
        oMax1.value = "0";
        oMax2.value = "0";
        oMax3.value = "0";
        oMax4.value = "0";
        oMax5.value = "0";
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
        oParent = document.getElementById('<%=lblParent.ClientID%>');
        oParentId = document.getElementById('<%=hdnParent.ClientID%>');
        oPath = document.getElementById('<%=txtPath.ClientID%>');
        oMax1 = document.getElementById('<%=txtMax1.ClientID%>');
        oMax2 = document.getElementById('<%=txtMax2.ClientID%>');
        oMax3 = document.getElementById('<%=txtMax3.ClientID%>');
        oMax4 = document.getElementById('<%=txtMax4.ClientID%>');
        oMax5 = document.getElementById('<%=txtMax5.ClientID%>');
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
		    <td><b>Platform Forms</b></td>
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
                            <td class="default">Platform:</td>
                            <td><asp:label ID="lblParent" CssClass="default" runat="server" />&nbsp;&nbsp;<asp:Button ID="btnParent" runat="server" CssClass="default" Width="25" Text="..." /></td>
                        </tr>
                        <tr> 
                            <td class="default" width="100px">Name:</td>
                            <td><asp:textbox ID="txtName" CssClass="default" runat="server" Width="300" MaxLength="100"/></td>
                        </tr>
                        <tr> 
                            <td class="default" width="100px" valign="top">Description:</td>
                            <td><asp:textbox ID="txtDescription" CssClass="default" runat="server" Width="300" TextMode="MultiLine" Rows="5" /></td>
                        </tr>
                        <tr> 
                            <td class="default">Image:</td>
                            <td><asp:textbox ID="txtImage" CssClass="default" runat="server" Width="300" MaxLength="100"/> <asp:Button ID="btnImage" runat="server" CssClass="default" Text="..." /></td>
                        </tr>
                        <tr> 
                            <td class="default">Path:</td>
                            <td><asp:textbox ID="txtPath" CssClass="default" runat="server" Width="400" MaxLength="100"/> <asp:button ID="btnPath" runat="server" Text="..." Width="25" CssClass="default"/></td>
                        </tr>
                        <tr> 
                            <td class="default">Max 1:</td>
                            <td><asp:textbox ID="txtMax1" CssClass="default" runat="server" Width="100" MaxLength="5"/></td>
                        </tr>
                        <tr> 
                            <td class="default">Max 2:</td>
                            <td><asp:textbox ID="txtMax2" CssClass="default" runat="server" Width="100" MaxLength="5"/></td>
                        </tr>
                        <tr> 
                            <td class="default">Max 3:</td>
                            <td><asp:textbox ID="txtMax3" CssClass="default" runat="server" Width="100" MaxLength="5"/></td>
                        </tr>
                        <tr> 
                            <td class="default">Max 4:</td>
                            <td><asp:textbox ID="txtMax4" CssClass="default" runat="server" Width="100" MaxLength="5"/></td>
                        </tr>
                        <tr> 
                            <td class="default">Max 5:</td>
                            <td><asp:textbox ID="txtMax5" CssClass="default" runat="server" Width="100" MaxLength="5"/></td>
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

