<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="sys_down.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.sys_down" %>


<table width="100%" cellpadding="0" cellspacing="0" border="0" class="default">
    <tr>
        <td><img src="/images/table_topleft.gif" border="0" width="5" height="26"></td>
        <td nowrap background="/images/table_top.gif" class="greentableheader" width="100%"><asp:label ID="lblTitle" runat="server" CssClass="greetableheader" /></td>
        <td><img src="/images/table_topright.gif" border="0" width="5" height="26"></td>
    </tr>
    <tr>
        <td background="/images/table_left.gif"><img src="/images/table_left.gif" width="5" height="10"></td>
        <td width="100%" bgcolor="#FFFFFF" align="center">
            <br />
            <p class="header"><img src="/images/hugeAlert.gif" border="0" align="absmiddle" /> ClearView is temporarily down for maintenance.</p>
            <p class="greentableheader">Estimate Time of Completion: <asp:Label ID="lblDate" runat="server" CssClass="greentableheader" /> @ <asp:Label ID="lblTime" runat="server" CssClass="greentableheader" /></p>
            <p>If you have questions or concerns, please contact Josh Ciora at x7639.</p>
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