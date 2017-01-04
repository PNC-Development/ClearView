<%@ Page Language="C#" MasterPageFile="~/frame.Master" AutoEventWireup="true" CodeBehind="print_asset_search.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.print_asset_search" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" runat="server">
    <table width="100%" cellpadding="0" cellspacing="0" border="0">
        <tr>
            <td>
                <table width="100%" cellpadding="3" cellspacing="0" border="0" align="center">
                    <asp:repeater ID="rptView" runat="server">
                        <HeaderTemplate>
                            <tr bgcolor="#EEEEEE">
                                <td class="tableheader"><b>ID:</b></td>
                                <td class="tableheader"><b>Device Name:</b></td>
                                <td class="tableheader"><b>Serial Number:</b></td>
                                <td class="tableheader"><b>Asset Tag:</b></td>
                                <td class="tableheader"><b>Platform:</b></td>
                                <td class="tableheader"><b>Type:</b></td>
                                <td class="tableheader"><b>Model:</b></td>
                                <td class="tableheader"><b>Status:</b></td>
                            </tr>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr class="default">
                                    <td valign="top"><%# DataBinder.Eval(Container.DataItem, "id")%></td>
                                    <td valign="top"><%# DataBinder.Eval(Container.DataItem, "name") %></td>
                                    <td valign="top"><%# DataBinder.Eval(Container.DataItem, "serial") %></td>
                                    <td valign="top"><%# DataBinder.Eval(Container.DataItem, "asset") %></td>
                                    <td valign="top"><%# DataBinder.Eval(Container.DataItem, "platform") %></td>
                                    <td valign="top"><%# DataBinder.Eval(Container.DataItem, "type") %></td>
                                    <td valign="top"><%# DataBinder.Eval(Container.DataItem, "model") %></td>
                                    <td valign="top"><%# DataBinder.Eval(Container.DataItem, "statusname")%></td>
                            </tr>
                        </ItemTemplate>
                        <AlternatingItemTemplate>
                            <tr class="default">
                                    <td valign="top"><%# DataBinder.Eval(Container.DataItem, "id")%></td>
                                    <td valign="top"><%# DataBinder.Eval(Container.DataItem, "name") %></td>
                                    <td valign="top"><%# DataBinder.Eval(Container.DataItem, "serial") %></td>
                                    <td valign="top"><%# DataBinder.Eval(Container.DataItem, "asset") %></td>
                                    <td valign="top"><%# DataBinder.Eval(Container.DataItem, "platform") %></td>
                                    <td valign="top"><%# DataBinder.Eval(Container.DataItem, "type") %></td>
                                    <td valign="top"><%# DataBinder.Eval(Container.DataItem, "model") %></td>
                                    <td valign="top"><%# DataBinder.Eval(Container.DataItem, "statusname")%></td>
                            </tr>
                        </AlternatingItemTemplate>
                    </asp:repeater>
                    <tr>
                        <td colspan="8"><asp:Label ID="lblNone" runat="server" CssClass="error" Visible="false" Text="<img src='/images/alert.gif' border='0' align='absmiddle'> There are no items" /></td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td><b>Search Criteria:</b></td>
        </tr>
        <tr>
            <td><asp:Label ID="lblResults" runat="server" CssClass="default" /></td>
        </tr>
    </table>
</asp:Content>
