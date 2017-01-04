<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="forecast_steps.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.forecast_steps" %>

<script type="text/javascript">
    var oAdd = null;
    var oView = null;
    var oAddButton = null;
    var oDeleteButton = null;
    var oHidden = null;
    var oParent = null;
    var oParentId = null;
    var oName = null;
    var oSubtitle = null;
    var oPath = null;
    var oOverride = null;
    var oImage = null;
    var oAdditional = null;
    var oEnabled = null;
    function Edit(strId, strParentId, strParent, strName, strSubtitle, strPath, strOverride, strImage, strAdditional, strEnabled) {
        oAdd.style.display = "inline";
        oView.style.display = "none";
        oAddButton.value = "Update";
        oDeleteButton.style.display = "inline";
        oHidden.value = strId;
        oParentId.value = strParentId;
        if (strParent == "" || strParent == "0")
            oParent.innerText = "No Parent";
        else
            oParent.innerText = strParent;
        oName.value = strName;
        oSubtitle.value = strSubtitle;
        oPath.value = strPath;
        oOverride.value = strOverride;
        oImage.value = strImage;
        oAdditional.checked = (strAdditional == "1");
        oEnabled.checked = (strEnabled == "1");
    }
    function Add(strParentId, strParent) {
        oAdd.style.display = "inline";
        oView.style.display = "none";
        oAddButton.value = "Add";
        oDeleteButton.style.display = "none";
        oHidden.value = "0";
        oParentId.value = strParentId;
        if (strParent == "" || strParent == "0")
            oParent.innerText = "No Parent";
        else
            oParent.innerText = strParent;
        oName.value = "";
        oSubtitle.value = "";
        oPath.value = "";
        oOverride.value = "";
        oImage.value = "";
        oAdditional.checked = false;
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
        oParent = document.getElementById('<%=lblParent.ClientID%>');
        oParentId = document.getElementById('<%=hdnParent.ClientID%>');
        oName = document.getElementById('<%=txtName.ClientID%>');
        oSubtitle = document.getElementById('<%=txtSubtitle.ClientID%>');
        oPath = document.getElementById('<%=txtPath.ClientID%>');
        oOverride = document.getElementById('<%=txtOverride.ClientID%>');
        oImage = document.getElementById('<%=txtImage.ClientID%>');
        oAdditional = document.getElementById('<%=chkAdditional.ClientID%>');
        oEnabled = document.getElementById('<%=chkEnabled.ClientID%>');
    }
</script>
<html>
<head>
<title>ClearView Administration</title>
<link rel="stylesheet" type="text/css" href="/css/default.css" />
<script type="text/javascript" src="/javascript/global.js"></script>
<script type="text/javascript" src="/javascript/ajax.js"></script>
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
		    <td><b>Forecast Steps</b></td>
		    <td align="right">&nbsp;</td>
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
                            <td class="default">Name:</td>
                            <td><asp:textbox ID="txtName" CssClass="default" runat="server" Width="200" MaxLength="50"/></td>
                        </tr>
                        <tr> 
                            <td class="default">Subtitle:</td>
                            <td><asp:textbox ID="txtSubtitle" CssClass="default" runat="server" Width="500" /></td>
                        </tr>
                        <tr> 
                            <td class="default">Path:</td>
                            <td><asp:textbox ID="txtPath" CssClass="default" runat="server" Width="400" MaxLength="100"/> <asp:button ID="btnPath" runat="server" Text="..." Width="25" CssClass="default"/></td>
                        </tr>
                        <tr> 
                            <td class="default">Override Path:</td>
                            <td><asp:textbox ID="txtOverride" CssClass="default" runat="server" Width="400" MaxLength="100"/> <asp:button ID="btnOverride" runat="server" Text="..." Width="25" CssClass="default"/></td>
                        </tr>
                        <tr> 
                            <td class="default">Image Path:</td>
                            <td><asp:textbox ID="txtImage" CssClass="default" runat="server" Width="400" MaxLength="100"/> <asp:button ID="btnImage" runat="server" Text="..." Width="25" CssClass="default"/></td>
                        </tr>
                        <tr> 
                            <td class="default">Additional Config Step:</td>
                            <td><asp:CheckBox ID="chkAdditional" runat="server" Checked="true" /></td>
                        </tr>
                        <tr> 
                            <td class="default">Enabled:</td>
                            <td><asp:CheckBox ID="chkEnabled" runat="server" Checked="true" /></td>
                        </tr>
                        <tr><td height="5" colspan="2"><img src="images/spacer.gif" width="1" height="5" /></td></tr>
                        <tr> 
                            <td class="cmdefault">&nbsp;</td>
                            <td><asp:Button ID="btnReset" runat="server" Text="Step Resets" Width="150" CssClass="default" /></td>
                        </tr>
                        <tr> 
                            <td class="default">&nbsp;</td>
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
