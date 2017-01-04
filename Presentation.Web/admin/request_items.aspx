<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="request_items.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.request_items" %>

<script type="text/javascript">
    var oAdd = null;
    var oView = null;
    var oAddButton = null;
    var oDeleteButton = null;
    var oHidden = null;
    var oParent = null;
    var oParentId = null;
    var oName = null;
    var oServiceTitle = null;
    var oImage = null;
    var oPlatform = null;
    var oActivity = null;
    var oShow = null;
    var oEnabled = null;
    function Edit(strId, strParentId, strParent, strName, strServiceTitle, strImage, strPlatform, strActivity, strShow, strEnabled) {
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
        oServiceTitle.value = strServiceTitle;
        oImage.value = strImage;
        for (var ii=0; ii<oPlatform.length; ii++) {
            if (oPlatform.options[ii].value == strPlatform)
                oPlatform.selectedIndex = ii;
        }
        oActivity.checked = (strActivity == "1");
        oShow.checked = (strShow == "1");
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
        oServiceTitle.value = "";
        oImage.value = "";
        oPlatform.selectedIndex = 0;
        oActivity.checked = false;
        oShow.checked = false;
        oEnabled.checked = false;
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
        oServiceTitle = document.getElementById('<%=txtServiceTitle.ClientID%>');
        oImage = document.getElementById('<%=txtImage.ClientID%>');
        oPlatform = document.getElementById('<%=ddlPlatform.ClientID%>');
        oActivity = document.getElementById('<%=chkActivity.ClientID%>');
        oShow = document.getElementById('<%=chkShow.ClientID%>');
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
		    <td><b>Request Items</b></td>
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
                            <td class="cmdefault">Application:</td>
                            <td><asp:label ID="lblParent" CssClass="default" runat="server" />&nbsp;&nbsp;<asp:Button ID="btnParent" runat="server" CssClass="default" Width="25" Text="..." /></td>
                        </tr>
                        <tr> 
                            <td class="default" width="100px">Name:</td>
                            <td><asp:textbox ID="txtName" CssClass="default" runat="server" Width="300" MaxLength="100"/></td>
                        </tr>
                        <tr> 
                            <td class="default" width="100px">Service Title:</td>
                            <td><asp:textbox ID="txtServiceTitle" CssClass="default" runat="server" Width="300" MaxLength="100"/></td>
                        </tr>
                        <tr> 
                            <td class="default">Image:</td>
                            <td><asp:textbox ID="txtImage" CssClass="default" runat="server" Width="300" MaxLength="100"/> <asp:Button ID="btnImage" runat="server" CssClass="default" Text="..." /></td>
                        </tr>
                        <tr> 
                            <td class="default">Platform:</td>
                            <td><asp:DropDownList ID="ddlPlatform" runat="server" CssClass="default" /></td>
                        </tr>
                        <tr> 
                            <td class="default">Activity Type:</td>
                            <td><asp:CheckBox ID="chkActivity" runat="server" /></td>
                        </tr>
                        <tr> 
                            <td class="default">Show:</td>
                            <td><asp:CheckBox ID="chkShow" runat="server" Checked="true" /></td>
                        </tr>
                        <tr> 
                            <td class="default">Enabled:</td>
                            <td><asp:CheckBox ID="chkEnabled" runat="server" Checked="true" /></td>
                        </tr>
                        <tr><td height="5" colspan="2"><img src="images/spacer.gif" width="1" height="5" /></td></tr>
                        <tr>
                            <td class="cmdefault">&nbsp;</td>
                            <td>
                                <asp:Button ID="btnTabs" runat="server" Text="Change Tabs" Width="150" CssClass="default" />
                                <asp:Button ID="btnTaskTabs" runat="server" Text="Change Task Tabs" Width="150" CssClass="default" />
                            </td>
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
    <asp:HiddenField ID="hdnUser" runat="server" />
</form>
</body>
</html>

