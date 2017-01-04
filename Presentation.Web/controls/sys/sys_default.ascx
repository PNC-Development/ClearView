<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="sys_default.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.sys_default" %>

<script runat="server">
    private void Page_Load()
    {
    }
</script>
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
            <p class="header"><img src="/images/bigError.gif" border="0" align="absmiddle" /> Invalid ClearView Configuration.</p>
            <p class="greentableheader">ClearView has encountered a configuration problem and has found nothing to load.</p>
            <p>Please contact your ClearView administrator immediately. Please reference the full URL of this page in your message.</p>
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