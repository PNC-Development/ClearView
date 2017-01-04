<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="support_all.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.support_all" %>


<script type="text/javascript">
    function ViewSupport(strUrl) {
        window.navigate(strUrl);
    }
</script>
<table width="100%" cellpadding="0" cellspacing="0" border="0" class="default">
    <tr>
        <td><img src="/images/table_topleft.gif" border="0" width="5" height="26"></td>
        <td nowrap background="/images/table_top.gif" class="greentableheader" width="100%"><asp:Label ID="lblTitle" runat="server" CssClass="greentableheader" /></td>
        <td nowrap background="/images/table_top.gif" class="greentableheader"><a href="javascript:window.print();" class="greentableheader">Print this Page</a></td>
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
                        <td><asp:LinkButton ID="btnOrderName" runat="server" CssClass="tableheader" Text="<b>Title</b>" OnClick="btnOrder_Click" CommandArgument="name" /></td>
                        <td><asp:LinkButton ID="btnOrderTitle" runat="server" CssClass="tableheader" Text="<b>Module</b>" OnClick="btnOrder_Click" CommandArgument="menutitle" /></td>
                        <td><asp:LinkButton ID="btnOrderType" runat="server" CssClass="tableheader" Text="<b>Request Type</b>" OnClick="btnOrder_Click" CommandArgument="type" /></td>
                        <td><asp:LinkButton ID="btnOrderRequester" runat="server" CssClass="tableheader" Text="<b>Requested By</b>" OnClick="btnOrder_Click" CommandArgument="userid" /></td>
                        <td><asp:LinkButton ID="btnOrderModified" runat="server" CssClass="tableheader" Text="<b>Requested On</b>" OnClick="btnOrder_Click" CommandArgument="modified" /></td>
                        <td align="center"><asp:LinkButton ID="btnOrderStatus" runat="server" CssClass="tableheader" Text="<b>Status</b>" OnClick="btnOrder_Click" CommandArgument="status" /></td>
                    </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr class="default" onmouseover="CellRowOver(this);" onmouseout="CellRowOut(this);" onclick="ViewSupport('<%# strRedirect + "?id=" + DataBinder.Eval(Container.DataItem, "id") %>');">
                        <td valign="top"><%# DataBinder.Eval(Container.DataItem, "name") %></td>
                        <td valign="top"><%# DataBinder.Eval(Container.DataItem, "menutitle") %></td>
                        <td valign="top"><%# (DataBinder.Eval(Container.DataItem, "type").ToString() == "1" ? "Suggestion" : "Issue") %></td>
                        <td valign="top"><%# oUser.GetFullName(oUser.GetName(Int32.Parse(DataBinder.Eval(Container.DataItem, "userid").ToString()))) %></td>
                        <td valign="top"><%# DateTime.Parse(DataBinder.Eval(Container.DataItem, "modified").ToString()).ToShortDateString() %></td>
                        <td valign="top" align="center"><img src='/images/<%# DataBinder.Eval(Container.DataItem, "statusname") %>.gif' border='0' title='<%# DataBinder.Eval(Container.DataItem, "statusname") %>' /></td>
                    </tr>
                </ItemTemplate>
                <AlternatingItemTemplate>
                    <tr class="default" bgcolor="#F6F6F6" onmouseover="CellRowOver(this);" onmouseout="CellRowOut(this);" onclick="ViewSupport('<%# strRedirect + "?id=" + DataBinder.Eval(Container.DataItem, "id") %>');">
                        <td valign="top"><%# DataBinder.Eval(Container.DataItem, "name") %></td>
                        <td valign="top"><%# DataBinder.Eval(Container.DataItem, "menutitle") %></td>
                        <td valign="top"><%# (DataBinder.Eval(Container.DataItem, "type").ToString() == "1" ? "Suggestion" : "Issue") %></td>
                        <td valign="top"><%# oUser.GetFullName(oUser.GetName(Int32.Parse(DataBinder.Eval(Container.DataItem, "userid").ToString()))) %></td>
                        <td valign="top"><%# DateTime.Parse(DataBinder.Eval(Container.DataItem, "modified").ToString()).ToShortDateString() %></td>
                        <td valign="top" align="center"><img src='/images/<%# DataBinder.Eval(Container.DataItem, "statusname") %>.gif' border='0' title='<%# DataBinder.Eval(Container.DataItem, "statusname") %>' /></td>
                    </tr>
                </AlternatingItemTemplate>
            </asp:repeater>
            <tr>
                <td colspan="6"><asp:Label ID="lblNone" runat="server" CssClass="error" Visible="false" Text="<img src='/images/alert.gif' border='0' align='absmiddle'> There are no items" /></td>
            </tr>
            </table>
            <br />
            <table width="100%" cellpadding="3" cellspacing="0" border="0">
                <tr>
                    <td>
                        <table cellpadding="2" cellspacing="2" border="0">
                            <tr>
                                <%= strFilters %>
                            </tr>
                        </table>
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