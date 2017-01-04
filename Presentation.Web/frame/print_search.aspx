<%@ Page Language="C#" MasterPageFile="~/frame.Master" AutoEventWireup="true" CodeBehind="print_search.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.print_search" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" runat="server">
    <table width="100%" cellpadding="0" cellspacing="0" border="0">
        <tr>
            <td>
                <table width="100%" cellpadding="3" cellspacing="0" border="0" align="center">
                    <asp:repeater ID="rptView" runat="server">
                        <HeaderTemplate>
                            <tr bgcolor="#EEEEEE">
                                <td class="tableheader"><b>Name:</b></td>
                                <td class="tableheader"><b>Number:</b></td>
                                <td class="tableheader"><b>Status:</b></td>
                                <td class="tableheader"><b>Color:</b></td>
                                <td class="tableheader"><b>Last Updated:</b></td>
                                <td class="tableheader"><b>Progress:</b></td>
                            </tr>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr class="default">
                                <td nowrap valign="top"><%# DataBinder.Eval(Container.DataItem, "name")%></td>
                                <td nowrap valign="top"><asp:label ID="lblNumber" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "number") %>' /></td>
                                <td nowrap valign="top"><asp:label ID="lblStatus" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "status") %>' /></td>
                                <td nowrap valign="top"><asp:label ID="lblColor" runat="server" CssClass="default" Text='' /></td>
                                <td nowrap valign="top"><asp:label ID="lblUpdated" runat="server" CssClass="default" Text='' /></td>
                                <td nowrap valign="top"><asp:label ID="lblProgress" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "query") %>' /></td>
                            </tr>
                        </ItemTemplate>
                        <AlternatingItemTemplate>
                            <tr class="default" bgcolor="#F6F6F6">
                                <td nowrap valign="top"><%# DataBinder.Eval(Container.DataItem, "name")%></td>
                                <td nowrap valign="top"><asp:label ID="lblNumber" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "number") %>' /></td>
                                <td nowrap valign="top"><asp:label ID="lblStatus" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "status") %>' /></td>
                                <td nowrap valign="top"><asp:label ID="lblColor" runat="server" CssClass="default" Text='' /></td>
                                <td nowrap valign="top"><asp:label ID="lblUpdated" runat="server" CssClass="default" Text='' /></td>
                                <td nowrap valign="top"><asp:label ID="lblProgress" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "query") %>' /></td>
                            </tr>
                        </AlternatingItemTemplate>
                    </asp:repeater>
                    <tr>
                        <td colspan="5"><asp:Label ID="lblNone" runat="server" CssClass="error" Visible="false" Text="<img src='/images/alert.gif' border='0' align='absmiddle'> There are no items" /></td>
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
