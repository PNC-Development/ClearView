<%@ Page Language="C#" MasterPageFile="~/frame.Master" AutoEventWireup="true" CodeBehind="forecast_reset_storage.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.forecast_reset_storage" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" runat="server">
    <table width="100%" cellpadding="2" cellspacing="2" border="0">
        <tr>
            <td class="bold" align="center">Are you sure you want to reset the storage?</td>
        </tr>
        <tr>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td align="center">
                <asp:Button ID="btnYes" runat="server" CssClass="default" Width="75" Text="Yes" OnClick="btnYes_Click" />
                <asp:Button ID="btnNo" runat="server" CssClass="default" Width="75" Text="No" OnClick="btnNo_Click" />
            </td>
        </tr>
    </table>
</asp:Content>
