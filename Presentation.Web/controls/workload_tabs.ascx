<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="workload_tabs.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.workload_tabs" %>


<script type="text/javascript">
    function ShowRR(strID) {
        OpenWindow("RESOURCE_REQUEST", strID);
        return false;
    }
</script>
<table width="100%" cellpadding="0" cellspacing="0" border="0" class="default">
    <tr>
        <td><img src="/images/table_topleft.gif" border="0" width="5" height="26"></td>
        <td nowrap background="/images/table_top.gif" class="greentableheader" width="100%"><asp:label ID="lblTitle" runat="server" CssClass="greetableheader" />Workload Manager</td>
        <td><img src="/images/table_topright.gif" border="0" width="5" height="26"></td>
    </tr>
    <tr>
        <td background="/images/table_left.gif"><img src="/images/table_left.gif" width="5" height="10"></td>
        <td width="100%" bgcolor="#FFFFFF">
            <br />
            <table width="100%" cellpadding="4" cellspacing="2" border="0">
                <tr>
                    <td valign="top">
                        <%=strTabs %>
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <asp:PlaceHolder ID="PHDiv" runat="server" />
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
<asp:Label ID="lblRequest" runat="server" Visible="false" />
<asp:Label ID="lblProject" runat="server" Visible="false" />