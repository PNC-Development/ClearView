<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="sys_certificate.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.sys_certificate" %>


<asp:Panel ID="panCertificate" runat="server" Visible="false">
<table width="100%" cellpadding="2" cellspacing="0" border="0" class="default">
    <tr><td>&nbsp;</td></tr>
    <tr>
        <td class="box_red" align="center" bgcolor="#FFFFFF">
            <table width="100%" cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td align="center">
                        <table cellpadding="0" cellspacing="5" border="0">
                            <tr>
                                <td rowspan="2"><img src="/images/lock.gif" border="0" align="absmiddle" /></td>
                                <td class="biggestbold" valign="bottom">Site Certificate Issue</td>
                            </tr>
                            <tr>
                                <td valign="top">The URL you are using to access ClearView does not match the certificate. This could cause problems.</td>
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                                <td valign="top"><asp:HyperLink ID="hypCertificate" runat="server" CssClass="default" Text="Click here to switch to the site matching the certificate" /></td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
</asp:Panel>
