<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SystemError.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.SystemError" %>
<table width="100%" cellpadding="2" cellspacing="2" border="0">
    <tr>
        <td>
            <img src="/images/spacer.gif" border="0" width="15" height="1" /></td>
        <td class="header">
            <img src="/images/bigX.gif" border="0" align="absmiddle" />
            An Error Occurred</td>
    </tr>
    <tr>
        <td colspan="2">
            &nbsp;</td>
    </tr>
    <tr>
        <td>
            <img src="/images/spacer.gif" border="0" width="15" height="1" /></td>
        <td>
            <asp:Label ID="lblError" runat="server" CssClass="default" /></td>
    </tr>
    <tr>
        <td colspan="2">
            &nbsp;</td>
    </tr>
    <tr>
        <td colspan="2">
            <hr size="1" noshade />
        </td>
    </tr>
    <tr>
        <td colspan="2" align="right">
            <table width="100%" cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td class="footer">
                    </td>
                    <td align="right">
                        <asp:Button ID="btnClose" runat="server" CssClass="default" Width="75" Text="Close" /></td>
                </tr>
            </table>
        </td>
    </tr>
</table>