<%@ Page Language="C#" MasterPageFile="~/frame.Master" AutoEventWireup="true" CodeBehind="change_controls.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.change_controls" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" Runat="Server">
<table width="100%" cellpadding="4" cellspacing="0" border="0" align="center" style="border:solid 1px #CCCCCC">
    <tr bgcolor="#EEEEEE">
        <td><b>Change Number</b></td>
        <td><b>Time</b></td>
        <td><b>Project Name</b></td>
        <td><b>Technician</b></td>
    </tr>
    <asp:repeater ID="rptView" runat="server">
        <ItemTemplate>
            <tr>
                <td valign="top"><asp:LinkButton ID="btnView" runat="server" OnClick="btnView_Click" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "changeid") %>' Text='<%# DataBinder.Eval(Container.DataItem, "number") %>' /></td>
                <td valign="top"><%# DateTime.Parse(DataBinder.Eval(Container.DataItem, "implementation").ToString()).ToShortTimeString()%></td>
                <td valign="top"><%# DataBinder.Eval(Container.DataItem, "projectname") %></td>
                <td valign="top"><%# oUser.GetFullName(Int32.Parse(DataBinder.Eval(Container.DataItem, "userid").ToString())) %></td>
            </tr>
        </ItemTemplate>
    </asp:repeater>
    <tr>
        <td colspan="4">
            <asp:Label ID="lblNone" runat="server" CssClass="default" Visible="false" Text="<img src='/images/spacer.gif' border='0' height='1' width='25' /><img src='/images/alert.gif' border='0' align='absmiddle'> No scheduled change controls" />
        </td>
    </tr>
</table>
</asp:Content>
