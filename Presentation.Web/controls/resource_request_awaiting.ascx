<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="resource_request_awaiting.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.resource_request_awaiting" %>


<script type="text/javascript">
    function AssignWork(strUrl) {
        if (event.srcElement.tagName != "INPUT")
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
                        <table cellpadding="0" cellspacing="0" border="0">
                            <tr>
                                <td>
                                    <b>Page <asp:TextBox ID="txtPage" runat="server" CssClass="default" Width="25" /> of <asp:Label ID="lblPages" runat="server" /> <asp:ImageButton ID="btnPage" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/search.gif" OnClick="btnPage_Click" ToolTip="Go to this page" /></b>
                                    <b><asp:Label ID="lblRecords" runat="server" Visible="false" /></b>
                                </td>
                                <td><img src="/images/spacer.gif" border="0" width="50" height="1" /></td>
                                <td>Filter:</td>
                                <td><img src="/images/spacer.gif" border="0" width="10" height="1" /></td>
                                <td>
                                    <asp:DropDownList ID="ddlType" runat="server" CssClass="default" AutoPostBack="true" OnSelectedIndexChanged="ddlType_Change">
                                        <asp:ListItem Value="0" Text="Show Requests Awaiting My Assignment ONLY" />
                                        <asp:ListItem Value="1" Text="Show Requests Awaiting My Assignment and Out of Office Buddies" />
                                    </asp:DropDownList>
                                </td>
                                <td class="bold">
                                    <div id="divType" runat="server" style="display:none">
                                        <img src="/images/spacer.gif" border="0" width="20" height="1" /><img src="/images/please_wait.gif" border="0" align="absmiddle" /> Please Wait...
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td align="right"><asp:Label ID="lblTopPaging" runat="server" /></td>
                </tr>
            </table>
            <br />
            <table width="100%" cellpadding="3" cellspacing="0" border="0" align="center" style="border:solid 1px #CCCCCC">
            <asp:repeater ID="rptView" runat="server">
                <HeaderTemplate>
                    <tr bgcolor="#EEEEEE">
                        <td width="1"></td>
                        <td valign="bottom"><asp:LinkButton ID="btnOrderID" runat="server" CssClass="tableheader" Text="<b>RequestID</b>" OnClick="btnOrder_Click" CommandArgument="requestid" /></td>
                        <td valign="bottom"><asp:LinkButton ID="btnOrderName" runat="server" CssClass="tableheader" Text="<b>Name</b>" OnClick="btnOrder_Click" CommandArgument="name" /></td>
                        <td valign="bottom"><asp:LinkButton ID="btnOrderTitle" runat="server" CssClass="tableheader" Text="<b>Service Type</b>" OnClick="btnOrder_Click" CommandArgument="title" /></td>
                        <td valign="bottom"><asp:LinkButton ID="btnOrderType" runat="server" CssClass="tableheader" Text="<b>Submitter</b>" OnClick="btnOrder_Click" CommandArgument="requestor" /></td>
                        <td valign="bottom"><asp:LinkButton ID="btnOrderSubmitted" runat="server" CssClass="tableheader" Text="<b>Submitted</b>" OnClick="btnOrder_Click" CommandArgument="modified" /></td>
                        <td valign="bottom" nowrap align="center"><asp:LinkButton ID="btnOrderStart" runat="server" CssClass="tableheader" Text="<b>Proposed<br/>Start Date</b>" OnClick="btnOrder_Click" CommandArgument="start_date" /></td>
                    </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr class="default" onmouseover="CellRowOver(this);" onmouseout="CellRowOut(this);" onclick="AssignWork('<%# strRedirect + "?rrid=" + DataBinder.Eval(Container.DataItem, "id") %>');">
                        <td width="1"><%# (DataBinder.Eval(Container.DataItem, "delegate").ToString() == "1" ? "<img src='/images/delegate.gif' border='0' align='absmiddle'/>&nbsp;" : "")%><%# (DataBinder.Eval(Container.DataItem, "status").ToString() == "5" ? "<img src='/images/onhold.gif' border='0' align='absmiddle'/>&nbsp;" : "")%></td>
                        <td valign="top">CVT<%# DataBinder.Eval(Container.DataItem, "requestid") %></td>
                        <td valign="top"><%# DataBinder.Eval(Container.DataItem, "name") %></td>
                        <td valign="top"><%# DataBinder.Eval(Container.DataItem, "title") %></td>
                        <td valign="top"><%# DataBinder.Eval(Container.DataItem, "requestor")%></td>
                        <td valign="top"><%# DateTime.Parse(DataBinder.Eval(Container.DataItem, "modified").ToString()).ToShortDateString() %></td>
                        <td valign="top" align="center"><%# (DataBinder.Eval(Container.DataItem, "start_date").ToString() == "" ? "N/A" : DateTime.Parse(DataBinder.Eval(Container.DataItem, "start_date").ToString()).ToShortDateString())%></td>
                    </tr>
                </ItemTemplate>
                <AlternatingItemTemplate>
                    <tr class="default" bgcolor="#F6F6F6" onmouseover="CellRowOver(this);" onmouseout="CellRowOut(this);" onclick="AssignWork('<%# strRedirect + "?rrid=" + DataBinder.Eval(Container.DataItem, "id") %>');">
                        <td width="1"><%# (DataBinder.Eval(Container.DataItem, "delegate").ToString() == "1" ? "<img src='/images/delegate.gif' border='0' align='absmiddle'/>&nbsp;" : "")%><%# (DataBinder.Eval(Container.DataItem, "status").ToString() == "5" ? "<img src='/images/onhold.gif' border='0' align='absmiddle'/>&nbsp;" : "")%></td>
                        <td valign="top">CVT<%# DataBinder.Eval(Container.DataItem, "requestid") %></td>
                        <td valign="top"><%# DataBinder.Eval(Container.DataItem, "name") %></td>
                        <td valign="top"><%# DataBinder.Eval(Container.DataItem, "title") %></td>
                        <td valign="top"><%# DataBinder.Eval(Container.DataItem, "requestor")%></td>
                        <td valign="top"><%# DateTime.Parse(DataBinder.Eval(Container.DataItem, "modified").ToString()).ToShortDateString() %></td>
                        <td valign="top" align="center"><%# (DataBinder.Eval(Container.DataItem, "start_date").ToString() == "" ? "N/A" : DateTime.Parse(DataBinder.Eval(Container.DataItem, "start_date").ToString()).ToShortDateString())%></td>
                    </tr>
                </AlternatingItemTemplate>
            </asp:repeater>
            <tr>
                <td colspan="2"><asp:Label ID="lblNone" runat="server" CssClass="error" Visible="false" Text="<img src='/images/alert.gif' border='0' align='absmiddle'> There are no items" /></td>
            </tr>
            </table>
            <br />
            <table width="100%" cellpadding="3" cellspacing="0" border="0">
                <tr>
                    <td><img src="/images/delegate.gif" border="0" align="absmiddle" /> = Covering for an Out of Office Buddy&nbsp;&nbsp;&nbsp;&nbsp;<img src="/images/onhold.gif" border="0" align="absmiddle" /> = Request On Hold</td>
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