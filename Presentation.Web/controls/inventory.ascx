<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="inventory.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.inventory" %>


<script type="text/javascript">
</script>
<table width="100%" cellpadding="0" cellspacing="0" border="0" class="default">
    <tr height="1">
        <td><img src="/images/table_topleft.gif" border="0" width="5" height="26"></td>
        <td nowrap background="/images/table_top.gif" class="greentableheader" width="100%"><asp:Label ID="lblTitle" runat="server" CssClass="greentableheader" /></td>
        <td><img src="/images/table_topright.gif" border="0" width="5" height="26"></td>
    </tr>
    <tr>
        <td background="/images/table_left.gif"><img src="/images/table_left.gif" width="5" height="10"></td>
        <td width="100%" bgcolor="#FFFFFF">
            <br />
            <table width="100%" cellpadding="0" cellspacing="5" border="0">
                <tr>
                    <td rowspan="2"><img src="/images/inventory.gif" border="0" align="absmiddle" /></td>
                    <td class="header" width="100%" valign="bottom">Inventory Manager</td>
                </tr>
                <tr>
                    <td width="100%" valign="top">Manage the inventory related to on-demand activities.</td>
                </tr>
            </table>
            <table width="100%" cellpadding="4" cellspacing="3" border="0">
                <tr>
                    <td>
                        <table width="100%" cellpadding="0" cellspacing="0" border="0" class="default">
                            <tr>
                                <td width="6"><img src="/images/box_top_left.gif" border="0" width="6" height="6"></td>
                                <td width="100%" nowrap background="/images/box_top.gif"></td>
                                <td width="6"><img src="/images/box_top_right.gif" border="0" width="6" height="6"></td>
                            </tr>
                            <tr>
                                <td width="6" background="/images/box_left.gif"><img src="/images/box_left.gif" width="6" height="6"></td>
                                <td width="100%" bgcolor="#FFFFFF" valign="top">
                                    <%=strPlatforms %>
                                </td>
                                <td width="6" background="/images/box_right.gif"><img src="/images/box_right.gif" width="6" height="6"></td>
                            </tr>
                            <tr>
                                <td width="6"><img src="/images/box_bottom_left.gif" border="0" width="6" height="6"></td>
                                <td width="100%" background="/images/box_bottom.gif"></td>
                                <td width="6"><img src="/images/box_bottom_right.gif" border="0" width="6" height="6"></td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <table width="100%" cellpadding="4" cellspacing="3" border="0">
                <%=strTabs %>
            </table>
            <p>&nbsp;</p>
        </td>
        <td background="/images/table_right.gif"><img src="/images/table_right.gif" width="5" height="10"></td>
    </tr>
    <tr height="1">
        <td><img src="/images/table_bottomLeft.gif" border="0" width="5" height="9"></td>
        <td width="100%" background="/images/table_bottom.gif"></td>
        <td><img src="/images/table_bottomRight.gif" border="0" width="5" height="9"></td>
    </tr>
</table>
<asp:HiddenField ID="hdnTab" runat="server" />
