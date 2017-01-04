<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="icons.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.icons" %>

<script type="text/javascript">
    var oAdd = null;
    var oView = null;
    var oAddButton = null;
    var oDeleteButton = null;
    var oHidden = null;
    var oExtension = null;
    var oSmall = null;
    var oLarge = null;
    var oContentType = null;
    var oIframe = null;
    var oPreview = null;
    var oEnabled = null;
    function Edit(strId, strExtension, strSmall, strLarge, strContentType, strIframe, strPreview, strEnabled) {
        oAdd.style.display = "inline";
        oView.style.display = "none";
        oAddButton.value = "Update";
        oDeleteButton.style.display = "inline";
        oHidden.value = strId;
        oExtension.value = strExtension.substring(1);
        oSmall.value = strSmall;
        oLarge.value = strLarge;
        oContentType.value = strContentType;
        oIframe.checked = (strIframe == "1");
        oPreview.checked = (strPreview == "1");
        oEnabled.checked = (strEnabled == "1");
    }
    function Add() {
        oAdd.style.display = "inline";
        oView.style.display = "none";
        oAddButton.value = "Add";
        oDeleteButton.style.display = "none";
        oHidden.value = "0";
        oExtension.value = "";
        oSmall.value = "";
        oLarge.value = "";
        oContentType.value = "";
        oIframe.checked = false;
        oPreview.checked = false;
        oEnabled.checked = false;
        oExtension.focus();
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
        oExtension = document.getElementById('<%=txtExtension.ClientID%>');
        oSmall = document.getElementById('<%=txtSmall.ClientID%>');
        oLarge = document.getElementById('<%=txtLarge.ClientID%>');
        oContentType = document.getElementById('<%=txtContentType.ClientID%>');
        oIframe = document.getElementById('<%=chkIframe.ClientID%>');
        oPreview = document.getElementById('<%=chkPreview.ClientID%>');
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
		    <td><b>Icons</b></td>
		    <td align="right"><a href="javascript:void(0);" onclick="Add();" class="cmlink" title="Click to Add New">Add New</a></td>
		</tr>
		<tr><td colspan="2">&nbsp;</td></tr>
		<tr>
		    <td colspan="2" align="center">
                <div ID="divView" runat="server">
                    <table width="100%" border="0" cellpadding="0" cellspacing="0">
                        <tr>
                            <td>
                                <asp:repeater ID="rptView" runat="server">
                                    <HeaderTemplate>
                                        <table width="100%" cellpadding="3" cellspacing="0" border="0" align="center">
                                            <tr bgcolor="#CCCCCC">
                                                <td class="bold">&nbsp;</td>
                                                <td width="50%" class="bold"><asp:linkbutton ID="lnkName" Text="Extension" CommandArgument="extension" CssClass="bold" OnClick="OrderView" runat="server"/></td>
                                                <td width="50%" class="bold"><asp:linkbutton ID="lnkModified" Text="Last Modified" CommandArgument="modified" CssClass="bold" OnClick="OrderView" runat="server"/></td>
                                                <td class="bold" align="center"><asp:linkbutton ID="lnkEnabled" Text="Enabled" CommandArgument="enabled" CssClass="bold" OnClick="OrderView" runat="server"/></td>
                                            </tr>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr bgcolor="#EFEFEF" onmouseover="AdminMouseOver(this);" onmouseout="AdminMouseOut(this);" onclick="if(event.srcElement.tagName != 'INPUT')Edit('<%# DataBinder.Eval(Container.DataItem, "id") %>','<%# DataBinder.Eval(Container.DataItem, "extension") %>','<%# DataBinder.Eval(Container.DataItem, "small") %>','<%# DataBinder.Eval(Container.DataItem, "large") %>','<%# DataBinder.Eval(Container.DataItem, "content_type") %>','<%# DataBinder.Eval(Container.DataItem, "iframe") %>','<%# DataBinder.Eval(Container.DataItem, "preview") %>','<%# DataBinder.Eval(Container.DataItem, "enabled") %>');" class="default">
                                            <td width="5%" align="left">&nbsp;<asp:ImageButton ID="btnDelete" OnClick="btnDeleteLink_Click" runat="server" ToolTip="Delete" ImageAlign="AbsMiddle" ImageUrl="/admin/images/delete.gif" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' /></td>
                                            <td width="50%">&nbsp;<%# DataBinder.Eval(Container.DataItem, "extension") %></td>
                                            <td width="50%">&nbsp;<%# DataBinder.Eval(Container.DataItem, "modified") %></td>
                                            <td align="center"><asp:ImageButton ID="btnEnable" runat="server" ImageAlign="AbsMiddle" ImageUrl='<%# (DataBinder.Eval(Container.DataItem, "enabled").ToString() == "1") ? "/admin/images/enabled.gif" : "/admin/images/disabled.gif" %>' CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' OnClick="btnEnable_Click" /></td>
                                        </tr>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </table>
                                    </FooterTemplate>
                                </asp:repeater>
                            </td>
                        </tr>
                    </table>
                </div>

                <div id="divAdd" runat="server" style="display:none">
                    <table width="95%" cellpadding="3" cellspacing="0" border="0" align="center">
                        <tr> 
                            <td class="default">Extension:</td>
                            <td>.<asp:textbox ID="txtExtension" CssClass="default" runat="server" Width="50" MaxLength="4"/></td>
                        </tr>
                        <tr> 
                            <td class="default">Small Icon:</td>
                            <td><asp:textbox ID="txtSmall" CssClass="default" runat="server" Width="300" MaxLength="100"/> <asp:Button ID="btnSmall" runat="server" CssClass="default" Text="..." /></td>
                        </tr>
                        <tr> 
                            <td class="default">Large Icon:</td>
                            <td><asp:textbox ID="txtLarge" CssClass="default" runat="server" Width="300" MaxLength="100"/> <asp:Button ID="btnLarge" runat="server" CssClass="default" Text="..." /></td>
                        </tr>
                        <tr> 
                            <td class="default">Content Type:</td>
                            <td><asp:textbox ID="txtContentType" CssClass="default" runat="server" Width="200" MaxLength="50"/></td>
                        </tr>
                        <tr> 
                            <td class="default">IFrame:</td>
                            <td><asp:CheckBox ID="chkIframe" runat="server" /></td>
                        </tr>
                        <tr> 
                            <td class="default">Preview:</td>
                            <td><asp:CheckBox ID="chkPreview" runat="server" /></td>
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
    <input type="hidden" id="hdnOrder" runat="server" />
</form>
</body>
</html>
