<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="wizard_steps.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.wizard_steps" %>

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
    var oShowCluster = null;
    var oShowCSM = null;
    var oSkipCluster = null;
    var oSkipCSM = null;
    var oEnabled = null;
    function Edit(strId, strParentId, strParent, strName, strSubtitle, strPath, strShowCluster, strShowCSM, strSkipCluster, strSkipCSM, strEnabled) {
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
        oShowCluster.checked = (strShowCluster == "1");
        oShowCSM.checked = (strShowCSM == "1");
        oSkipCluster.checked = (strSkipCluster == "1");
        oSkipCSM.checked = (strSkipCSM == "1");
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
        oShowCluster.checked = false;
        oShowCSM.checked = false;
        oSkipCluster.checked = false;
        oSkipCSM.checked = false;
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
        oShowCluster = document.getElementById('<%=chkShowCluster.ClientID%>');
        oShowCSM = document.getElementById('<%=chkShowCSM.ClientID%>');
        oSkipCluster = document.getElementById('<%=chkSkipCluster.ClientID%>');
        oSkipCSM = document.getElementById('<%=chkSkipCSM.ClientID%>');
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
		    <td><b>On Demand Wizard Steps</b></td>
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
                            <td class="default">Show for Cluster:</td>
                            <td><asp:CheckBox ID="chkShowCluster" runat="server" Checked="true" /></td>
                        </tr>
                        <tr> 
                            <td class="default">Show for CSM:</td>
                            <td><asp:CheckBox ID="chkShowCSM" runat="server" Checked="true" /></td>
                        </tr>
                        <tr> 
                            <td class="default">Skip if not Cluster:</td>
                            <td><asp:CheckBox ID="chkSkipCluster" runat="server" Checked="true" /></td>
                        </tr>
                        <tr> 
                            <td class="default">Skip if not CSM:</td>
                            <td><asp:CheckBox ID="chkSkipCSM" runat="server" Checked="true" /></td>
                        </tr>
                        <tr> 
                            <td class="default">Enabled:</td>
                            <td><asp:CheckBox ID="chkEnabled" runat="server" Checked="true" /></td>
                        </tr>
                        <tr><td height="5" colspan="2"><img src="images/spacer.gif" width="1" height="5" /></td></tr>
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

