<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="sys_personal.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.sys_personal" %>


<table width="100%" cellpadding="0" cellspacing="0" border="0" class="default">
    <tr>
        <td><img src="/images/table_topleft.gif" border="0" width="5" height="26"></td>
        <td nowrap background="/images/table_top.gif" class="greentableheader" width="100%">Personal Page</td>
        <td><img src="/images/table_topright.gif" border="0" width="5" height="26"></td>
    </tr>
    <tr>
        <td background="/images/table_left.gif"><img src="/images/table_left.gif" width="5" height="10"></td>
        <td width="100%" bgcolor="#FFFFFF">
            <br />
            <p>Logged In: <b><asp:label ID="lblName" runat="server" /></b></p>
            <p>Your personal page can be configured to show customized controls that you want to see.</p>
            <p><asp:LinkButton ID="btnDesign" runat="server" Text="Click here to design your page." /></p>
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