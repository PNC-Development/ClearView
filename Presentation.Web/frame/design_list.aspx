<%@ Page Language="C#" MasterPageFile="~/frame.Master" AutoEventWireup="true" CodeBehind="design_list.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.design_list" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" runat="server">

    <asp:Panel ID="panHelp" runat="server" Visible="false">
        <table cellpadding="5" cellspacing="2" border="0" align="center">
            <tr>
                <td class="bigger"><b>&quot;<asp:Literal ID="litHelpHeader" runat="server" />&quot; Help</b></td>
            </tr>
            <tr>
                <td><asp:Literal ID="litHelp" runat="server" /></td>
            </tr>
        </table>
    </asp:Panel>
    
    <asp:Panel ID="panStorage" runat="server" Visible="false">
        <table width="100%" cellpadding="5" cellspacing="2" border="0">
            <tr>
                <td class="bigger" colspan="2"><b>Storage Configuration</b></td>
            </tr>
            <tr>
                <td colspan="2">
                    <table width="100%" cellpadding="4" cellspacing="0" border="0" align="center" style="border:solid 1px #CCCCCC">
                        <tr bgcolor="#EEEEEE">
                            <td style='display:<%=boolWindows ? "inline" : "none" %>' nowrap><b><u>Drive:</u></b></td>
                            <td style='display:<%=boolWindows ? "inline" : "none" %>' width="100%"><b><u>Mount Point:</u></b></td>
                            <td style='display:<%=boolWindows == false ? "inline" : "none" %>' width="100%"><b><u>Filesystem:</u></b></td>
                            <td nowrap><b><u>Shared:</u></b></td>
                            <td nowrap><b><u>Size:</u></b></td>
                        </tr>
                        <tr id="trStorageApp" runat="server" visible="false">
                            <td nowrap>E:</td>
                            <td width="100%">&nbsp;&nbsp;<span class="footer">(Reserved: Application Drive)</span></td>
                            <td nowrap><asp:CheckBox ID="chkStorageSizeE" runat="server" Enabled="false" Checked="false" /></td>
                            <td nowrap><asp:Label ID="txtStorageSizeE" runat="server" /> GB</td>
                        </tr>
                        <asp:repeater ID="rptStorage" runat="server">
                            <ItemTemplate>
                                <tr>
                                    <td style='display:<%=boolWindows ? "inline" : "none" %>' valign="top" nowrap><%# DataBinder.Eval(Container.DataItem, "letter") %></td>
                                    <td valign="top" width="100%"><%# DataBinder.Eval(Container.DataItem, "path") %></td>
                                    <td valign="top" nowrap><asp:CheckBox ID="chkStorageSize" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "shared") %>' Enabled="false" /></td>
                                    <td valign="top" nowrap><%# DataBinder.Eval(Container.DataItem, "size") %> GB</td>
                                </tr>
                            </ItemTemplate>
                        </asp:repeater>
                    </table>
                </td>
            </tr>
        </table>
    </asp:Panel>
    
    <asp:Panel ID="panAccounts" runat="server" Visible="false">
        <table width="100%" cellpadding="5" cellspacing="2" border="0">
            <tr>
                <td class="bigger" colspan="2"><b>Account Configuration</b></td>
            </tr>
            <tr>
                <td colspan="2">
                    <table width="100%" cellpadding="4" cellspacing="0" border="0" style="border:solid 1px #CCCCCC">
                        <tr bgcolor="#EEEEEE">
                            <td><b><u>User:</u></b></td>
                            <td><b><u>Permission:</u></b></td>
                        </tr>
                        <asp:repeater ID="rptAccounts" runat="server">
                            <ItemTemplate>
                                <tr>
                                    <td valign="top"><%# oUser.GetFullName(Int32.Parse(DataBinder.Eval(Container.DataItem, "userid").ToString())) %> (<%# oUser.GetName(Int32.Parse(DataBinder.Eval(Container.DataItem, "userid").ToString()))%>)</td>
                                    <td valign="top"><asp:Label ID="lblPermissions" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "access") %>' ToolTip='<%# DataBinder.Eval(Container.DataItem, "remote") %>' /></td>
                                </tr>
                            </ItemTemplate>
                            <AlternatingItemTemplate>
                                <tr bgcolor="F6F6F6">
                                    <td valign="top"><%# oUser.GetFullName(Int32.Parse(DataBinder.Eval(Container.DataItem, "userid").ToString()))%> (<%# oUser.GetName(Int32.Parse(DataBinder.Eval(Container.DataItem, "userid").ToString()))%>)</td>
                                    <td valign="top"><asp:Label ID="lblPermissions" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "access") %>' ToolTip='<%# DataBinder.Eval(Container.DataItem, "remote") %>' /></td>
                                </tr>
                            </AlternatingItemTemplate>
                        </asp:repeater>
                        <tr>
                            <td colspan="2">
                                <asp:Label ID="lblNone" runat="server" CssClass="default" Visible="false" Text="<img src='/images/spacer.gif' border='0' height='1' width='25' /><img src='/images/alert.gif' border='0' align='absmiddle'> You have not added any accounts to this request" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </asp:Panel>
    
    <asp:Panel ID="panExclusions" runat="server" Visible="false">
        <table width="100%" cellpadding="5" cellspacing="2" border="0">
            <tr>
                <td class="bigger" colspan="2"><b>Backup Exclusion Configuration</b></td>
            </tr>
            <tr>
                <td colspan="2">
                    <table width="100%" cellpadding="4" cellspacing="0" border="0" style="border:solid 1px #CCCCCC">
                        <tr bgcolor="#EEEEEE">
                            <td><b><u>Path:</u></b></td>
                        </tr>
                        <asp:repeater ID="rptExclusions" runat="server">
                            <ItemTemplate>
                                <tr>
                                    <td valign="top" width="100%"><%# DataBinder.Eval(Container.DataItem, "path") %></td>
                                </tr>
                            </ItemTemplate>
                            <AlternatingItemTemplate>
                                <tr bgcolor="F6F6F6">
                                    <td valign="top" width="100%"><%# DataBinder.Eval(Container.DataItem, "path") %></td>
                                </tr>
                            </AlternatingItemTemplate>
                        </asp:repeater>
                        <tr>
                            <td colspan="2">
                                <asp:Label ID="lblExclusion" runat="server" CssClass="default" Visible="false" Text="<img src='/images/spacer.gif' border='0' height='1' width='25' /><img src='/images/alert.gif' border='0' align='absmiddle'> There are no backup exclusions" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </asp:Panel>
    
    <asp:Panel ID="panQuantity" runat="server" Visible="false">
        <table cellpadding="5" cellspacing="2" border="0" align="center">
            <tr>
                <td class="bigger" colspan="4"><b>Server Count Configuration</b></td>
            </tr>
            <tr>
                <td></td>
                <td>Server Count</td>
                <td>:</td>
                <td><asp:Label ID="lblQuantity" runat="server" /></td>
            </tr>
            <tr id="trQuantityDR" runat="server" visible="false">
                <td>+</td>
                <td>One to One Recovery (DR)</td>
                <td>:</td>
                <td><asp:Label ID="lblQuantityDR" runat="server" /></td>
            </tr>
            <tr>
                <td colspan="4"><hr size="1" noshade /></td>
            </tr>
            <tr>
                <td></td>
                <td><b>Total Count</b></td>
                <td>=</td>
                <td><asp:Label ID="lblQuantityTotal" runat="server" /></td>
            </tr>
        </table>
    </asp:Panel>
    
    <asp:Panel ID="panStorageAmount" runat="server" Visible="false">
        <table width="100%" cellpadding="5" cellspacing="2" border="0">
            <tr>
                <td class="bigger" colspan="4"><b>Storage Amount Configuration</b></td>
            </tr>
            <tr>
                <td></td>
                <td>Storage Amount</td>
                <td>:</td>
                <td><asp:Label ID="lblStorage" runat="server" /> GB(s)</td>
            </tr>
            <tr id="trStorageDR" runat="server" visible="false">
                <td>+</td>
                <td>Replicated Storage (DR)</td>
                <td>:</td>
                <td><asp:Label ID="lblStorageDR" runat="server" /> GB(s)</td>
            </tr>
            <tr>
                <td colspan="4"><hr size="1" noshade /></td>
            </tr>
            <tr>
                <td></td>
                <td><b>Total Storage</b></td>
                <td>=</td>
                <td><asp:Label ID="lblStorageTotal" runat="server" /> GB(s)</td>
            </tr>
        </table>
    </asp:Panel>
    
    <asp:Panel ID="panLiteral" runat="server" Visible="false">
        <asp:Literal ID="litLiteral" runat="server" />
    </asp:Panel>

</asp:Content>
