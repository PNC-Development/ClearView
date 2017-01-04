<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="servername_mine.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.servername_mine" %>


<script type="text/javascript">
    function ViewWorkload(strID) {
        window.navigate('<%=strRedirect %>?id=' + strID);
    }
</script>
<table width="100%" cellpadding="0" cellspacing="0" border="0" class="default">
    <tr>
        <td><img src="/images/table_topleft.gif" border="0" width="5" height="26"></td>
        <td nowrap background="/images/table_top.gif" class="greentableheader" width="100%"><asp:Label ID="lblTitle" runat="server" CssClass="greentableheader" /></td>
        <td nowrap background="/images/table_top.gif" class="greentableheader"></td>
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
                        <td><asp:LinkButton ID="btnOrderServer" runat="server" CssClass="tableheader" Text="<b>Server Name</b>" OnClick="btnOrder_Click" CommandArgument="servername" /></td>
                        <td><asp:LinkButton ID="btnOrderName" runat="server" CssClass="tableheader" Text="<b>Nickname</b>" OnClick="btnOrder_Click" CommandArgument="name" /></td>
                        <td><asp:LinkButton ID="btnOrderSubmitted" runat="server" CssClass="tableheader" Text="<b>Requested</b>" OnClick="btnOrder_Click" CommandArgument="modified" /></td>
                        <td></td>
                    </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr class="default" onmouseover="CellRowOver(this);" onmouseout="CellRowOut(this);">
                        <td width="40%"><%# DataBinder.Eval(Container.DataItem, "servername") %></td>
                        <td width="40%"><%# DataBinder.Eval(Container.DataItem, "name") %></td>
                        <td nowrap><%# DateTime.Parse(DataBinder.Eval(Container.DataItem, "modified").ToString()).ToShortDateString() + " " + DateTime.Parse(DataBinder.Eval(Container.DataItem, "modified").ToString()).ToShortTimeString()%></td>
                        <td><asp:Button ID="btnRelease" runat="server" CssClass="default" Width="100" Text="Release" OnClick="btnRelease_Click" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id")%>' CommandName='<%# DataBinder.Eval(Container.DataItem, "type")%>' /></td>
                    </tr>
                </ItemTemplate>
                <AlternatingItemTemplate>
                    <tr class="default" bgcolor="#F6F6F6" onmouseover="CellRowOver(this);" onmouseout="CellRowOut(this);">
                        <td width="40%"><%# DataBinder.Eval(Container.DataItem, "servername") %></td>
                        <td width="40%"><%# DataBinder.Eval(Container.DataItem, "name") %></td>
                        <td nowrap><%# DateTime.Parse(DataBinder.Eval(Container.DataItem, "modified").ToString()).ToShortDateString() + " " + DateTime.Parse(DataBinder.Eval(Container.DataItem, "modified").ToString()).ToShortTimeString()%></td>
                        <td><asp:Button ID="btnRelease" runat="server" CssClass="default" Width="100" Text="Release" OnClick="btnRelease_Click" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id")%>' CommandName='<%# DataBinder.Eval(Container.DataItem, "type")%>' /></td>
                    </tr>
                </AlternatingItemTemplate>
            </asp:repeater>
            <tr>
                <td colspan="3"><asp:Label ID="lblNone" runat="server" CssClass="error" Visible="false" Text="<img src='/images/alert.gif' border='0' align='absmiddle'> There are no items" /></td>
            </tr>
            </table>
            <br />
            <table width="100%" cellpadding="3" cellspacing="0" border="0">
                <tr>
                    <td></td>
                    <td align="right"><asp:Label ID="lblBottomPaging" runat="server" /></td>
                </tr>
                <tr>
                    <td colspan="2">&nbsp;</td>
                </tr>
                <tr>
                    <td colspan="2" class="required"><b>NOTE:</b> Releasing server names will make them available for re-assignment. Only use this feature if you HAVE NOT and ARE NOT going to use the server name. This feature is not designed to assist you in cleaning up your queue. Currently, you cannot clean up your server name queue.</td>
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
