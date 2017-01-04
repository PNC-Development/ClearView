<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="sys_application.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.sys_application" %>


<table width="100%" cellpadding="0" cellspacing="0" border="0" class="default">
    <tr>
        <td><img src="/images/table_topleft.gif" border="0" width="5" height="26"></td>
        <td nowrap background="/images/table_top.gif" class="greentableheader" width="100%">Available Applications</td>
        <td><img src="/images/table_topright.gif" border="0" width="5" height="26"></td>
    </tr>
    <tr>
        <td background="/images/table_left.gif"><img src="/images/table_left.gif" width="5" height="10"></td>
        <td width="100%" bgcolor="#FFFFFF">
            <table width="100%" cellpadding="2" cellspacing="0" border="0" class="greentable">
                <%=strMenu %>
                <tr>
                    <td>&nbsp;</td>
                    <td><img src="/images/bigLogout.gif" title="Log Out" border="0" align="absmiddle">&nbsp;<asp:LinkButton ID="btnLogout" runat="server" Text="Log Out of Clearview" OnClick="btnLogout_Click" /></td>
                </tr>
            </table>
        </td>
        <td background="/images/table_right.gif"><img src="/images/table_right.gif" width="5" height="10"></td>
    </tr>
    <tr>
        <td><img src="/images/table_bottomLeft.gif" border="0" width="5" height="9"></td>
        <td width="100%" background="/images/table_bottom.gif"></td>
        <td><img src="/images/table_bottomRight.gif" border="0" width="5" height="9"></td>
    </tr>
</table>