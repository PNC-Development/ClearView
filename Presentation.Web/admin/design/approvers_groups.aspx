<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="approvers_groups.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.design_approvers_groups" %>

<script type="text/javascript">
</script>
<html>
<head>
<title>ClearView Administration</title>
<link rel="stylesheet" type="text/css" href="/css/default.css" />
<script type="text/javascript" src="/javascript/global.js"></script>
</head>
<body topmargin="0" leftmargin="0">
<form id="Form1" runat="server">
        <table width="100%" height="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
        <td height="100%">
        <div style="height:100%; overflow:auto">
<table width="98%" cellpadding="0" cellspacing="0" border="0" align="center">
		<tr><td colspan="2">&nbsp;</td></tr>
		<tr> 
		    <td><b>Design Approvers (Overall)</b></td>
		    <td align="right"><asp:LinkButton id="btnNew" runat="server" Text="Add New" OnClick="btnNew_Click" /></td>
		</tr>
		<tr><td colspan="2">&nbsp;</td></tr>
		<tr>
		    <td colspan="2" align="center">
                <asp:Panel ID="panView" runat="server" Visible="false">
                    <table width="100%" border="0" cellpadding="0" cellspacing="0">
                        <tr>
                            <td>
                                <asp:repeater ID="rptView" runat="server">
                                    <HeaderTemplate>
                                        <table width="100%" cellpadding="3" cellspacing="0" border="0" align="center">
                                            <tr bgcolor="#CCCCCC">
                                                <td class="bold">&nbsp;</td>
                                                <td width="33%" class="bold"><asp:linkbutton ID="lnkGroup" Text="Group" CommandArgument="group" CssClass="bold" OnClick="OrderView" runat="server"/></td>
                                                <td width="33%" class="bold"><asp:linkbutton ID="lnkException" Text="Only Exception" CommandArgument="only_exceptions" CssClass="bold" OnClick="OrderView" runat="server"/></td>
                                                <td width="33%" class="bold"><asp:linkbutton ID="lnkModified" Text="Last Modified" CommandArgument="modified" CssClass="bold" OnClick="OrderView" runat="server"/></td>
                                                <td class="bold" align="center"><asp:linkbutton ID="lnkEnabled" Text="Enabled" CommandArgument="enabled" CssClass="bold" OnClick="OrderView" runat="server"/></td>
                                            </tr>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr bgcolor="#EFEFEF" onmouseover="AdminMouseOver(this);" onmouseout="AdminMouseOut(this);" onclick="if(event.srcElement.tagName != 'INPUT')window.navigate('<%# Request.Path + "?id=" + DataBinder.Eval(Container.DataItem, "id")%>');" class="default">
                                            <td width="5%" align="left">&nbsp;<asp:ImageButton ID="btnDelete" OnClick="btnDeleteLink_Click" runat="server" ToolTip="Delete" ImageAlign="AbsMiddle" ImageUrl="/admin/images/delete.gif" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' /></td>
                                            <td width="33%">&nbsp;<%# DataBinder.Eval(Container.DataItem, "group") %></td>
                                            <td width="33%">&nbsp;<img src='<%# (DataBinder.Eval(Container.DataItem, "only_exceptions").ToString() == "1") ? "/admin/images/enabled.gif" : "/admin/images/disabled.gif" %>' border='0' /></td>
                                            <td width="33%">&nbsp;<%# DataBinder.Eval(Container.DataItem, "modified") %></td>
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
               </asp:Panel>

                <asp:Panel id="panAdd" runat="server" Visible="false">
                    <table width="95%" cellpadding="3" cellspacing="0" border="0" align="center">
                        <tr> 
                            <td class="default" width="100px">Group:</td>
                            <td><asp:DropDownList ID="ddlGroup" CssClass="default" runat="server" /></td>
                        </tr>
                        <tr> 
                            <td class="default">Only Exceptions:</td>
                            <td><asp:CheckBox ID="chkExceptions" runat="server" /></td>
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
                                <asp:button ID="btnCancel" CssClass="default" runat="server" Text="Cancel" Width="75" OnClick="btnCancel_Click" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
    </table>
        </div>
        </td>
        </tr>
        </table>
    <input type="hidden" id="hdnId" runat="server" />
</form>
</body>
</html>