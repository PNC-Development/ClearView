<%@ Page Language="C#" MasterPageFile="~/frame.Master" AutoEventWireup="true" CodeBehind="profile.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.profile" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" runat="server">
    <table width="100%" height="100%" cellpadding="2" cellspacing="0" border="0">
        <tr height="1">
            <td valign="top">
                <table width="100%" cellpadding="2" cellspacing="2" border="0">
                    <tr>
                        <td nowrap><b>Name:</b></td>
                        <td width="100%"><asp:Label ID="lblName" runat="server" CssClass="default" /></td>
                    </tr>
                    <tr>
                        <td nowrap><b>Phone:</b></td>
                        <td width="100%"><asp:Label ID="lblPhone" runat="server" CssClass="default" /></td>
                    </tr>
                    <tr>
                        <td nowrap><b>Pager:</b></td>
                        <td width="100%"><asp:Label ID="lblPager" runat="server" CssClass="default" /></td>
                    </tr>
                    <tr>
                        <td nowrap><b>Email:</b></td>
                        <td width="100%"><asp:Label ID="lblEmail" runat="server" CssClass="default" /></td>
                    </tr>
                    <tr>
                        <td nowrap><b>Manager:</b></td>
                        <td width="100%"><asp:Label ID="lblManager" runat="server" CssClass="default" /></td>
                    </tr>
                </table>
            </td>
            <td valign="top"><asp:Image ID="imgPicture" runat="server" Width="90" Height="90" /></td>
        </tr>
        <tr>
            <td colspan="2" valign="top">
                <table width="100%" cellpadding="2" cellspacing="2" border="0">
                    <tr>
                        <td><b>Special Skills:</b></td>
                    </tr>
                    <tr>
                        <td><asp:Label ID="lblSkills" runat="server" CssClass="default" /></td>
                    </tr>
                </table>
            </td>
        </tr>		
    </table>
</asp:Content>
