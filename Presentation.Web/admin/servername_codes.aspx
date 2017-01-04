<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="servername_codes.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.servername_codes" %>


<script type="text/javascript">
    var oAdd = null;
    var oView = null;
    var oAddButton = null;
    var oDeleteButton = null;
    var oHidden = null;
    var oCode = null;
    var oClass = null;
    var oEnvironment = null;
    var oEnvironmentH = null;
    var oParent = null;
    var oParentId = null;
    var oEnabled = null;
    function Edit(strId, strCode, strClass, strEnvironment, strParentId, strParent, strEnabled) {
        oAdd.style.display = "inline";
        oView.style.display = "none";
        oAddButton.value = "Update";
        oDeleteButton.style.display = "inline";
        oHidden.value = strId;
        oCode.value = strCode;
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
        oEnabled.checked = (strEnabled == "1");
    }
    function Add(strParentId, strParent) {
        oAdd.style.display = "inline";
        oView.style.display = "none";
        oAddButton.value = "Add";
        oDeleteButton.style.display = "none";
        oHidden.value = "0";
        oCode.value = "";
        oClass.selectedIndex = 0;
        oEnvironment.disabled = true;
		var oOption = document.createElement("OPTION");
		oEnvironment.add(oOption);
		oOption.text = " -- Please select a Class --";
		oOption.value = "0";
        oEnvironment.selectedIndex = 0;
        oEnvironmentH.value = "0";
        oParentId.value = strParentId;
        if (oParentId == "" || strParentId == "0")
            oParent.innerText = "None";
        else
            oParent.innerText = strParent;
        oEnabled.checked = false;
        oCode.focus();
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
        oCode = document.getElementById('<%=txtCode.ClientID%>');
        oClass = document.getElementById('<%=ddlClass.ClientID%>');
        oEnvironment = document.getElementById('<%=ddlEnvironment.ClientID%>');
        oEnvironmentH = document.getElementById('<%=hdnEnvironment.ClientID%>');
        oParent = document.getElementById('<%=lblParent.ClientID%>');
        oParentId = document.getElementById('<%=hdnParent.ClientID%>');
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
		    <td><b>Server Name Codes</b></td>
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
                            <td class="default">SiteCode:</td>
                            <td><asp:textbox ID="txtCode" CssClass="default" runat="server" Width="50" MaxLength="2"/></td>
                        </tr>
                        <tr> 
                            <td class="default">Class:</td>
                            <td><asp:dropdownlist ID="ddlClass" CssClass="default" runat="server"/></td>
                        </tr>
                        <tr> 
                            <td class="default">Environment:</td>
                            <td><asp:dropdownlist ID="ddlEnvironment" CssClass="default" runat="server"/></td>
                        </tr>
                        <tr>
                            <td class="default">Address:</td>
                            <td><asp:label ID="lblParent" CssClass="default" runat="server" />&nbsp;&nbsp;<asp:Button ID="btnParent" runat="server" CssClass="default" Width="25" Text="..." /></td>
                        </tr>
                        <tr> 
                            <td class="default">Enabled:</td>
                            <td><asp:CheckBox ID="chkEnabled" runat="server" Checked="true" /></td>
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
</form>
</body>
</html>
