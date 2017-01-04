<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="sofware_components.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.sofware_components" %>
<script type="text/javascript">
    function ViewApproval(strUrl) {
        window.navigate(strUrl);
    }
</script>
<table width="100%" cellpadding="0" cellspacing="0" border="0" class="default">
    <tr>
        <td><img src="/images/table_topleft.gif" border="0" width="5" height="26"></td>
        <td nowrap background="/images/table_top.gif" class="greentableheader" width="100%"><asp:Label ID="lblTitle" runat="server" CssClass="greentableheader" /></td>
        <td nowrap background="/images/table_top.gif" class="greentableheader"></td>
        </td>
        <td><img src="/images/table_topright.gif" border="0" width="5" height="26"></td>
    </tr>
    <tr>
        <td background="/images/table_left.gif"><img src="/images/table_left.gif" width="5" height="10"></td>
        <td width="100%" bgcolor="#FFFFFF" colspan="2">
            <br />
            <table width="100%" cellpadding="3" cellspacing="0" border="0">
                <tr>
                    <td>
                        <b>Page <asp:TextBox ID="txtPage" runat="server" CssClass="default" Width="25" /> of <asp:Label ID="lblPages" runat="server" /> <asp:ImageButton ID="btnPage" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/search.gif" OnClick="btnPage_Click" ToolTip="Go to this page" /></b>
                        <b><asp:Label ID="lblRecords" runat="server" Visible="false" /></b>
                    </td>
                    <td align="right"><asp:Label ID="lblTopPaging" runat="server" /></td>
                </tr>
            </table>
            <br />
            <table width="100%" cellpadding="3" cellspacing="0" border="0" align="center" style="border:solid 1px #CCCCCC">
            <asp:repeater ID="rptView" runat="server">
                <HeaderTemplate>
                    <tr bgcolor="#EEEEEE">
                        <td valign="bottom"><asp:LinkButton ID="btnOrderDesignID" runat="server" CssClass="tableheader" Text="<b>Design ID</b>" OnClick="btnOrder_Click" CommandArgument="answerid" /></td>
                        <td valign="bottom"><asp:LinkButton ID="btnOrderDesignName" runat="server" CssClass="tableheader" Text="<b>Design Nickname</b>" OnClick="btnOrder_Click" CommandArgument="design_name" /></td>
                        <td valign="bottom"><asp:LinkButton ID="btnOrderName" runat="server" CssClass="tableheader" Text="<b>Project Name</b>" OnClick="btnOrder_Click" CommandArgument="project_name" /></td>
                        <td valign="bottom"><asp:LinkButton ID="btnOrderNumber" runat="server" CssClass="tableheader" Text="<b>Project Number</b>" OnClick="btnOrder_Click" CommandArgument="project_number" /></td>
                    </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr class="default" onmouseover="CellRowOver(this);" onmouseout="CellRowOut(this);" onclick="ViewApproval('<%# strRedirect + "?id=" + DataBinder.Eval(Container.DataItem, "answerid") %>');">
                        <td valign="top"><%# DataBinder.Eval(Container.DataItem, "answerid") %></td>
                        <td valign="top"><%# DataBinder.Eval(Container.DataItem, "design_name")%></td>
                        <td valign="top"><%# DataBinder.Eval(Container.DataItem, "project_name") %></td>
                        <td valign="top"><%# DataBinder.Eval(Container.DataItem, "project_number") %></td>
                    </tr>
                </ItemTemplate>
                <AlternatingItemTemplate>
                    <tr class="default" bgcolor="#F6F6F6" onmouseover="CellRowOver(this);" onmouseout="CellRowOut(this);" onclick="ViewApproval('<%# strRedirect + "?id=" + DataBinder.Eval(Container.DataItem, "answerid") %>');">
                        <td valign="top"><%# DataBinder.Eval(Container.DataItem, "answerid") %></td>
                        <td valign="top"><%# DataBinder.Eval(Container.DataItem, "design_name")%></td>
                        <td valign="top"><%# DataBinder.Eval(Container.DataItem, "project_name") %></td>
                        <td valign="top"><%# DataBinder.Eval(Container.DataItem, "project_number") %></td>
                    </tr>
                </AlternatingItemTemplate>
            </asp:repeater>
            <tr>
                <td colspan="4"><asp:Label ID="lblNone" runat="server" CssClass="error" Visible="false" Text="<img src='/images/alert.gif' border='0' align='absmiddle'> There are no items" /></td>
            </tr>
            </table>
            <br />
            <table width="100%" cellpadding="3" cellspacing="0" border="0">
                <tr>
                    <td>
                    </td>
                    <td align="right"><asp:Label ID="lblBottomPaging" runat="server" /></td>
                </tr>
            </table>
            <p>&nbsp;</p>
        </td>
        <td background="/images/table_right.gif"><img src="/images/table_right.gif" width="5" height="10"></td>
    </tr>
    <tr>
        <td><img src="/images/table_bottomLeft.gif" border="0" width="5" height="9"></td>
        <td width="100%" background="/images/table_bottom.gif" colspan="2"></td>
        <td><img src="/images/table_bottomRight.gif" border="0" width="5" height="9"></td>
    </tr>
</table>
<asp:Label ID="lblPage" runat="server" Visible="false" />
<asp:Label ID="lblSort" runat="server" Visible="false" />