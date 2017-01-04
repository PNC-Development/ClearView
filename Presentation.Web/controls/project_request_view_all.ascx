<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="project_request_view_all.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.project_request_view_all" %>


<script type="text/javascript">
    function ViewProject(strUrl) {
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
                        <td><asp:LinkButton ID="btnOrderName" runat="server" CssClass="tableheader" Text="<b>Name</b>" OnClick="btnOrder_Click" CommandArgument="name" /></td>
                        <td><asp:LinkButton ID="btnOrderSubmitted" runat="server" CssClass="tableheader" Text="<b>Submitted</b>" OnClick="btnOrder_Click" CommandArgument="modified" /></td>
                        <td>&nbsp;</td>
                        <td><asp:LinkButton ID="btnOrderRequester" runat="server" CssClass="tableheader" Text="<b>Requester</b>" OnClick="btnOrder_Click" CommandArgument="userid" /></td>
                        <td>&nbsp;</td>
                        <td align="center"><asp:LinkButton ID="btnOrderReqType" runat="server" CssClass="tableheader" Text="<b>Audit</b>" OnClick="btnOrder_Click" CommandArgument="req_type" /></td>
                        <td>&nbsp;</td>
                        <td align="center"><asp:LinkButton ID="btnOrderC1" runat="server" CssClass="tableheader" Text="<b>C1</b>" OnClick="btnOrder_Click" CommandArgument="c1" /></td>
                        <td>&nbsp;</td>
                        <td align="center"><asp:LinkButton ID="btnOrderEnd" runat="server" CssClass="tableheader" Text="<b>End<br/>Life</b>" OnClick="btnOrder_Click" CommandArgument="endlife" /></td>
                        <td>&nbsp;</td>
                        <td align="center" width="60"><asp:LinkButton ID="btnOrderExpected" runat="server" CssClass="tableheader" Text="<b>Expected<br/>Cost</b>" OnClick="btnOrder_Click" CommandArgument="expected_cost" /></td>
                        <td>&nbsp;</td>
                        <td align="center" width="60"><asp:LinkButton ID="btnOrderAvoidance" runat="server" CssClass="tableheader" Text="<b>Cost<br/>Avoidance</b>" OnClick="btnOrder_Click" CommandArgument="cost_avoidance" /></td>
                        <td>&nbsp;</td>
                        <td align="center" width="60"><asp:LinkButton ID="btnOrderImpact" runat="server" CssClass="tableheader" Text="<b>Impact</b>" OnClick="btnOrder_Click" CommandArgument="impact_analysis" /></td>
                        <td>&nbsp;</td>
                        <td align="center" width="60"><asp:LinkButton ID="btnOrderOverall" runat="server" CssClass="tableheader" Text="<b>Overall</b>" OnClick="btnOrder_Click" CommandArgument="overall_priority" /></td>
                        <td>&nbsp;</td>
                        <td align="center"><asp:LinkButton ID="btnOrderStatus" runat="server" CssClass="tableheader" Text="<b>Status</b>" OnClick="btnOrder_Click" CommandArgument="status" /></td>
                    </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr class="default" onmouseover="CellRowOver(this);" onmouseout="CellRowOut(this);" onclick="ViewProject('<%# strRedirect + "?rid=" + DataBinder.Eval(Container.DataItem, "requestid") %>');">
                        <td valign="top"><%# DataBinder.Eval(Container.DataItem, "name") %></td>
                        <td valign="top"><%# DateTime.Parse(DataBinder.Eval(Container.DataItem, "modified").ToString()).ToShortDateString() %></td>
                        <td>&nbsp;</td>
                        <td valign="top"><%# oUser.GetFullName(oUser.GetName(Int32.Parse(DataBinder.Eval(Container.DataItem, "userid").ToString()))) %></td>
                        <td>&nbsp;</td>
                        <td valign="top" align="center"><%#(DataBinder.Eval(Container.DataItem, "req_type").ToString() == "1" ? "<img src=\"/images/siren.gif\" border=\"0\" align=\"absmiddle\">" : "")%></td>
                        <td>&nbsp;</td>
                        <td valign="top" align="center"><%#(DataBinder.Eval(Container.DataItem, "c1").ToString() == "1" ? "<img src=\"/images/postit.gif\" border=\"0\" align=\"absmiddle\">" : "")%></td>
                        <td>&nbsp;</td>
                        <td valign="top" align="center"><%#(DataBinder.Eval(Container.DataItem, "endlife").ToString() == "1" ? "<img src=\"/images/xx.gif\" border=\"0\" align=\"absmiddle\">" : "")%></td>
                        <td>&nbsp;</td>
                        <td valign="top" align="center" width="60" <%# oProjectRequest.GetPriorityColor(Double.Parse(DataBinder.Eval(Container.DataItem, "expected_cost").ToString())) %>><%# Double.Parse(DataBinder.Eval(Container.DataItem, "expected_cost").ToString()).ToString("P").TrimEnd(charsToTrim)%></td>
                        <td>&nbsp;</td>
                        <td valign="top" align="center" width="60" <%# oProjectRequest.GetPriorityColor(Double.Parse(DataBinder.Eval(Container.DataItem, "cost_avoidance").ToString())) %>><%# Double.Parse(DataBinder.Eval(Container.DataItem, "cost_avoidance").ToString()).ToString("P").TrimEnd(charsToTrim)%></td>
                        <td>&nbsp;</td>
                        <td valign="top" align="center" width="60" <%# oProjectRequest.GetPriorityColor(Double.Parse(DataBinder.Eval(Container.DataItem, "impact_analysis").ToString())) %>><%# Double.Parse(DataBinder.Eval(Container.DataItem, "impact_analysis").ToString()).ToString("P").TrimEnd(charsToTrim)%></td>
                        <td>&nbsp;</td>
                        <td valign="top" align="center" width="60" <%# oProjectRequest.GetPriorityColor(Double.Parse(DataBinder.Eval(Container.DataItem, "overall_priority").ToString())) %>><%# Double.Parse(DataBinder.Eval(Container.DataItem, "overall_priority").ToString()).ToString("P").TrimEnd(charsToTrim)%></td>
                        <td>&nbsp;</td>
                        <td valign="top" align="center"><img src='/images/<%# DataBinder.Eval(Container.DataItem, "statusname") %>.gif' border='0' title='<%# DataBinder.Eval(Container.DataItem, "statusname") %>' /></td>
                    </tr>
                </ItemTemplate>
                <AlternatingItemTemplate>
                    <tr class="default" bgcolor="#F6F6F6" onmouseover="CellRowOver(this);" onmouseout="CellRowOut(this);" onclick="ViewProject('<%# strRedirect + "?rid=" + DataBinder.Eval(Container.DataItem, "requestid") %>');">
                        <td valign="top"><%# DataBinder.Eval(Container.DataItem, "name") %></td>
                        <td valign="top"><%# DateTime.Parse(DataBinder.Eval(Container.DataItem, "modified").ToString()).ToShortDateString() %></td>
                        <td>&nbsp;</td>
                        <td valign="top"><%# oUser.GetFullName(oUser.GetName(Int32.Parse(DataBinder.Eval(Container.DataItem, "userid").ToString()))) %></td>
                        <td>&nbsp;</td>
                        <td valign="top" align="center"><%#(DataBinder.Eval(Container.DataItem, "req_type").ToString() == "1" ? "<img src=\"/images/siren.gif\" border=\"0\" align=\"absmiddle\">" : "")%></td>
                        <td>&nbsp;</td>
                        <td valign="top" align="center"><%#(DataBinder.Eval(Container.DataItem, "c1").ToString() == "1" ? "<img src=\"/images/postit.gif\" border=\"0\" align=\"absmiddle\">" : "")%></td>
                        <td>&nbsp;</td>
                        <td valign="top" align="center"><%#(DataBinder.Eval(Container.DataItem, "endlife").ToString() == "1" ? "<img src=\"/images/xx.gif\" border=\"0\" align=\"absmiddle\">" : "")%></td>
                        <td>&nbsp;</td>
                        <td valign="top" align="center" width="60" <%# oProjectRequest.GetPriorityColor(Double.Parse(DataBinder.Eval(Container.DataItem, "expected_cost").ToString())) %>><%# Double.Parse(DataBinder.Eval(Container.DataItem, "expected_cost").ToString()).ToString("P").TrimEnd(charsToTrim)%></td>
                        <td>&nbsp;</td>
                        <td valign="top" align="center" width="60" <%# oProjectRequest.GetPriorityColor(Double.Parse(DataBinder.Eval(Container.DataItem, "cost_avoidance").ToString())) %>><%# Double.Parse(DataBinder.Eval(Container.DataItem, "cost_avoidance").ToString()).ToString("P").TrimEnd(charsToTrim)%></td>
                        <td>&nbsp;</td>
                        <td valign="top" align="center" width="60" <%# oProjectRequest.GetPriorityColor(Double.Parse(DataBinder.Eval(Container.DataItem, "impact_analysis").ToString())) %>><%# Double.Parse(DataBinder.Eval(Container.DataItem, "impact_analysis").ToString()).ToString("P").TrimEnd(charsToTrim)%></td>
                        <td>&nbsp;</td>
                        <td valign="top" align="center" width="60" <%# oProjectRequest.GetPriorityColor(Double.Parse(DataBinder.Eval(Container.DataItem, "overall_priority").ToString())) %>><%# Double.Parse(DataBinder.Eval(Container.DataItem, "overall_priority").ToString()).ToString("P").TrimEnd(charsToTrim)%></td>
                        <td>&nbsp;</td>
                        <td valign="top" align="center"><img src='/images/<%# DataBinder.Eval(Container.DataItem, "statusname") %>.gif' border='0' title='<%# DataBinder.Eval(Container.DataItem, "statusname") %>' /></td>
                    </tr>
                </AlternatingItemTemplate>
                <SeparatorTemplate>
                    <tr>
                        <td colspan="16" height="1"><img src="/images/spacer.gif" border="0" width="1" height="1" /></td>
                    </tr>
                </SeparatorTemplate>
            </asp:repeater>
            <tr>
                <td colspan="16"><asp:Label ID="lblNone" runat="server" CssClass="error" Visible="false" Text="<img src='/images/alert.gif' border='0' align='absmiddle'> There are no items" /></td>
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