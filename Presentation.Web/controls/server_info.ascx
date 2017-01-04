<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="server_info.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.server_info" %>


<script type="text/javascript">
</script>
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
            <table width="100%" cellpadding="0" cellspacing="5" border="0">
                <tr>
                    <td rowspan="2"><img src="/images/server_search.gif" border="0" align="absmiddle" /></td>
                    <td class="header" width="100%" valign="bottom">Server Name Lookup</td>
                </tr>
                <tr>
                    <td width="100%" valign="top">Use this search page to find information regarding a server.</td>
                </tr>
            </table>
            <table width="100%" cellpadding="4" cellspacing="3" border="0">
                <tr>
                    <td nowrap>Server Name:</td>
                    <td width="100%"><asp:TextBox ID="txtName" runat="server" Width="250" CssClass="default" MaxLength="100" /></td>
                </tr>
                <tr>
                    <td nowrap class="required"></td>
                    <td width="100%"><asp:Button ID="btnSearch" Text="Search" Width="100" CssClass="default" runat="server" OnClick="btnSearch_Click" /></td>
                </tr>
            </table>
            <asp:Panel ID="panSearch" runat="server" Visible="false">
            <table width="100%" cellpadding="4" cellspacing="3" border="0">
                <tr>
                    <td class="hugeheader" colspan="2"><asp:Label ID="lblServer" runat="server" CssClass="hugeheader" /> Details...</td>
                </tr>
                <%=strResults %>
            </table>
            </asp:Panel>
            <asp:Panel ID="panMultiple" runat="server" Visible="false">
            <table width="100%" cellpadding="4" cellspacing="3" border="0">
                <tr>
                    <td class="bold">Please select one of the following servers...</td>
                </tr>
                <tr>
                    <td>
                        <table width="100%" cellpadding="4" cellspacing="0" border="0" style="border:solid 1px #CCCCCC">
                            <tr bgcolor="#EEEEEE">
                                <td nowrap><b>Server Name</b></td>
                                <td nowrap><b>Implementor</b></td>
                                <td nowrap><b>Type</b></td>
                                <td nowrap><b>Project Name</b></td>
                                <td nowrap><b>Project Number</b></td>
                            </tr>
                            <%=strMultiple %>
                        </table>
                    </td>
                </tr>
            </table>
            </asp:Panel>
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
<asp:HiddenField ID="hdnManager" runat="server" />