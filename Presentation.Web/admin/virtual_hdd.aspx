<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="virtual_hdd.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.virtual_hdd" %>


<script type="text/javascript">
    var oAdd = null;
    var oView = null;
    var oAddButton = null;
    var oDeleteButton = null;
    var oName = null;
    var oValue = null;
    var oVMware = null;
    var oVirtual = null;
    var oXP = null;
    var oWin7 = null;
    var oDefault = null;
    var oPrompt = null;
    var oEnabled = null;
    function Edit(strId, strName, strValue, strVMware, strVirtual, strXP, strWin7, strDefault, strPrompt, strEnabled) {
        oAdd.style.display = "inline";
        oView.style.display = "none";
        oAddButton.value = "Update";
        oDeleteButton.style.display = "inline";
        oHidden.value = strId;
        oName.value = strName;
        oValue.value = strValue;
        oVMware.checked = (strVMware == "1");
        oVirtual.checked = (strVirtual == "1");
        oXP.checked = (strXP == "1");
        oWin7.checked = (strWin7 == "1");
        oDefault.checked = (strDefault == "1");
        oPrompt.value = strPrompt;
        oEnabled.checked = (strEnabled == "1");
    }
    function Add() {
        oAdd.style.display = "inline";
        oView.style.display = "none";
        oAddButton.value = "Add";
        oDeleteButton.style.display = "none";
        oHidden.value = "0";
        oName.value = "";
        oValue.value = "0";
        oVMware.checked = false;
        oVirtual.checked = false;
        oXP.checked = false;
        oWin7.checked = false;
        oDefault.checked = false;
        oPrompt.value = "";
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
        oValue = document.getElementById('<%=txtValue.ClientID%>');
        oVMware = document.getElementById('<%=chkVMware.ClientID%>');
        oVirtual = document.getElementById('<%=chkVirtual.ClientID%>');
        oXP = document.getElementById('<%=chkXP.ClientID%>');
        oWin7 = document.getElementById('<%=chkWin7.ClientID%>');
        oDefault = document.getElementById('<%=chkDefault.ClientID%>');
        oPrompt = document.getElementById('<%=txtPrompt.ClientID%>');
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
		    <td><b>Virtual Hard Drives</b></td>
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
                                                <td width="50%" class="bold"><asp:linkbutton ID="lnkName" Text="Name" CommandArgument="name" CssClass="bold" OnClick="OrderView" runat="server"/></td>
                                                <td width="50%" class="bold"><asp:linkbutton ID="lnkModified" Text="Last Modified" CommandArgument="modified" CssClass="bold" OnClick="OrderView" runat="server"/></td>
                                                <td class="bold" align="center"><asp:linkbutton ID="lnkEnabled" Text="Enabled" CommandArgument="enabled" CssClass="bold" OnClick="OrderView" runat="server"/></td>
                                            </tr>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr bgcolor="#EFEFEF" onmouseover="AdminMouseOver(this);" onmouseout="AdminMouseOut(this);" onclick="if(event.srcElement.tagName != 'INPUT')Edit('<%# DataBinder.Eval(Container.DataItem, "id") %>','<%# DataBinder.Eval(Container.DataItem, "name") %>','<%# DataBinder.Eval(Container.DataItem, "value") %>','<%# DataBinder.Eval(Container.DataItem, "vmware") %>','<%# DataBinder.Eval(Container.DataItem, "virtual") %>','<%# DataBinder.Eval(Container.DataItem, "xp") %>','<%# DataBinder.Eval(Container.DataItem, "win7") %>','<%# DataBinder.Eval(Container.DataItem, "default") %>','<%# DataBinder.Eval(Container.DataItem, "prompt") %>','<%# DataBinder.Eval(Container.DataItem, "enabled") %>');" class="default">
                                            <td width="5%" align="left">&nbsp;<asp:ImageButton ID="btnDelete" OnClick="btnDeleteLink_Click" runat="server" ToolTip="Delete" ImageAlign="AbsMiddle" ImageUrl="/admin/images/delete.gif" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' /></td>
                                            <td width="50%">&nbsp;<%# DataBinder.Eval(Container.DataItem, "name") %></td>
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
                            <td class="default">Name:</td>
                            <td><asp:textbox ID="txtName" CssClass="default" runat="server" Width="300" MaxLength="50"/></td>
                        </tr>
                        <tr> 
                            <td class="default">Value:</td>
                            <td><asp:textbox ID="txtValue" CssClass="default" runat="server" Width="200" MaxLength="10"/> (in GB)</td>
                        </tr>
                        <tr> 
                            <td class="default">VMware:</td>
                            <td><asp:CheckBox ID="chkVMware" runat="server" Checked="true" /></td>
                        </tr>
                        <tr> 
                            <td class="default">Virtual:</td>
                            <td><asp:CheckBox ID="chkVirtual" runat="server" Checked="true" /></td>
                        </tr>
                        <tr> 
                            <td class="default">Windows XP:</td>
                            <td><asp:CheckBox ID="chkXP" runat="server" Checked="true" /></td>
                        </tr>
                        <tr> 
                            <td class="default">Windows 7:</td>
                            <td><asp:CheckBox ID="chkWin7" runat="server" Checked="true" /></td>
                        </tr>
                        <tr> 
                            <td class="default">Default:</td>
                            <td><asp:CheckBox ID="chkDefault" runat="server" Checked="true" /> (There should only be one)</td>
                        </tr>
                        <tr> 
                            <td class="default">Prompt:</td>
                            <td><asp:textbox ID="txtPrompt" CssClass="default" runat="server" Width="300" TextMode="MultiLine" Rows="5"/></td>
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
    <input type="hidden" id="hdnOrder" runat="server" />
</form>
</body>
</html>
