<%@ Page Language="C#" MasterPageFile="~/frame.Master" AutoEventWireup="true" CodeBehind="vacation.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.vacation" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" runat="server">
    <table width="100%" height="100%" cellpadding="2" cellspacing="0" border="0">
        <tr height="1">
            <td align="center"><b><asp:Label ID="lblDate" runat="server" CssClass="header" /></b></td>
        </tr>
        <tr>
            <td valign="top">
                <asp:repeater ID="rptView" runat="server">
                    <HeaderTemplate>
                        <table width="100%" cellpadding="3" cellspacing="2" border="0" align="center">
                            <tr>
                                <td background="/images/tableheader.gif"><b>Employee</b></td>
                                <td background="/images/tableheader.gif"><b>Type</b></td>
                                <td background="/images/tableheader.gif"><b>Reason</b></td>
                                <td background="/images/tableheader.gif"><b>Status</b></td>
                            </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td><%# DataBinder.Eval(Container.DataItem, "username") %></td>
                            <td><%# DataBinder.Eval(Container.DataItem, "duration") %></td>
                            <td><%# DataBinder.Eval(Container.DataItem, "reason") %></td>
                            <td><%# (DataBinder.Eval(Container.DataItem, "approved").ToString() == "1" ? "Approved" : "Pending") %></td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        </table>
                    </FooterTemplate>
                </asp:repeater>
            </td>
        </tr>
    </table>
</asp:Content>
