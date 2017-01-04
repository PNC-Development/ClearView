<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="service_folders.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.service_folders" %>


<script type="text/javascript">
    var oAdd = null;
    var oView = null;
    var oAddButton = null;
    var oDeleteButton = null;
    var oHidden = null;
    var oName = null;
    var oDesc = null;
    var oImage = null;
    var oParentId = null;
    var oParent = null;
    var oUser = null;
    var oUserId = null;
    var oEnabled = null;
    function Edit(strId, strName, strDesc, strImage, strParentId, strParent, strUserId, strUser, strEnabled) {
        oAdd.style.display = "inline";
        oView.style.display = "none";
        oAddButton.value = "Update";
        oDeleteButton.style.display = "inline";
        oHidden.value = strId;
        oName.value = strName;
        oDesc.value = strDesc;
        oImage.value = strImage;
        oParentId.value = strParentId;
        if (strParent == "")
            oParent.innerText = "No Parent";
        else
            oParent.innerText = strParent;
        oUser.value = strUser;
        oUserId.value = strUserId;
        oEnabled.checked = (strEnabled == "1");
    }
    function Add(strParentId, strParent) {
        oAdd.style.display = "inline";
        oView.style.display = "none";
        oAddButton.value = "Add";
        oDeleteButton.style.display = "none";
        oHidden.value = "0";
        oName.value = "";
        oDesc.value = "";
        oImage.value = "";
        oParentId.value = strParentId;
        if (strParent == "")
            oParent.innerText = "No Parent";
        else
            oParent.innerText = strParent;
        oUser.value = "";
        oUserId.value = "0";
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
        oName = document.getElementById('<%=txtName.ClientID%>');
        oDesc = document.getElementById('<%=txtDescription.ClientID%>');
        oImage = document.getElementById('<%=txtImage.ClientID%>');
        oParent = document.getElementById('<%=lblParent.ClientID%>');
        oParentId = document.getElementById('<%=hdnParent.ClientID%>');
        oUserId = document.getElementById('<%=hdnUser.ClientID%>');
        oUser = document.getElementById('<%=txtUser.ClientID%>');
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
		    <td><b>Service Folders</b></td>
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
                            <td class="default">Parent:</td>
                            <td><asp:label ID="lblParent" CssClass="default" runat="server" />&nbsp;&nbsp;<asp:Button ID="btnParent" runat="server" CssClass="default" Width="25" Text="..." /></td>
                        </tr>
                        <tr> 
                            <td class="default">User:</td>
                            <td>
                                <table cellpadding="0" cellspacing="0" border="0">
                                    <tr>
                                        <td><asp:TextBox ID="txtUser" runat="server" Width="300" CssClass="default" /></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div id="divAJAX" runat="server" style="overflow:hidden; position:absolute; display:none; background-color:#FFFFFF; border:solid 1px #CCCCCC">
                                                <asp:ListBox ID="lstAJAX" runat="server" CssClass="default" />
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                                <input type="hidden" id="hdnAJAXValue" name="hdnAJAXValue" />
                            </td>
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
    <input type="hidden" id="hdnUser" runat="server" />
    <input type="hidden" id="hdnOrder" runat="server" />
</form>
</body>
</html>
