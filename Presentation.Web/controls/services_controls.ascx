<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="services_controls.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.services_controls" %>

<table width="100%" cellpadding="0" cellspacing="0" border="0" class="default">
    <tr>
        <td><img src="/images/table_topleft.gif" border="0" width="5" height="26"></td>
        <td nowrap background="/images/table_top.gif" class="greentableheader" width="100%"><asp:Label ID="lblTitle" runat="server" CssClass="greentableheader" /></td>
        <td><img src="/images/table_topright.gif" border="0" width="5" height="26"></td>
    </tr>
    <tr>
        <td background="/images/table_left.gif"><img src="/images/table_left.gif" width="5" height="10"></td>
        <td width="100%" bgcolor="#FFFFFF">
            <br />
            <table width="100%" border="0" cellSpacing="2" cellPadding="4" class="default">
                <tr>
		            <td align="center">
		                <table width="100%" cellpadding="0" cellspacing="0" border="0">
                            <tr>
                                <td colspan="2"><%=strFavorites%></td>
                            </tr>
		                    <tr>
		                        <td>
		                            <table cellpadding="4" cellspacing="4" border="0">
		                                <tr>
		                                    <td><asp:Button ID="btnFavorite" runat="server" OnClick="btnFavorite_Click" CssClass="default" Text="Checkout  >>" Height="20" Width="125" /></td>
		                                </tr>
		                            </table>
		                        </td>
		                        <td align="right">
		                        </td>
		                    </tr>
		                </table>
		            </td>
                </tr>
            </table>
            <p>&nbsp;</p>
        </td>
        <td background="/images/table_right.gif"><img src="/images/table_right.gif" width="5" height="10"></td>
    </tr>
    <tr>
        <td><img src="/images/table_bottomLeft.gif" border="0" width="5" height="9"></td>
        <td width="100%" background="/images/table_bottom.gif"></td>
        <td><img src="/images/table_bottomRight.gif" border="0" width="5" height="9"></td>
    </tr>
</table>
<asp:HiddenField ID="hdnService" runat="server" />
