<%@ Page Language="C#" MasterPageFile="~/frame.Master" AutoEventWireup="true" CodeBehind="resource_request_return.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.resource_request_return" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" runat="server">
    <table width="100%" cellpadding="0" cellspacing="5" border="0">
        <tr>
            <td class="header" style="width:100%;text-align:center;white-space:nowrap;vertical-align:bottom">
                Return Request</td>
        </tr>
    </table>
  
    <table width="100%" cellpadding="3" cellspacing="2" border="0">
        <tr>
            <td valign="top"><asp:Label ID="lblReturnTo" CssClass="bold" Text="Returning To" runat="server" /></td>
        </tr>
        <tr>
            <td valign="top">
                <asp:DropDownList ID="ddlReturnTo" Width="98%" runat="server" />
            </td>
        </tr>
        <tr>
            <td><asp:Label ID="lblComments" CssClass="bold" Text="Comments" runat="server" /></td>
        </tr>
        <tr>
            <td><asp:TextBox ID="txtComments" CssClass="default" Width="98%" runat="server" TextMode ="MultiLine" Rows="9"></asp:TextBox></td>
        </tr>
        <tr>
            <td align="center"><asp:Button ID="btnSave" runat="server" CssClass="default" Width="125" Text="Submit" OnClick="btnSave_Click"  /></td>
        </tr>
     </table>
        <asp:Label ID="lblStorage" runat="server" Visible="false" Text="0" />
</asp:Content>
