<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="resource_request_view_mine.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.resource_request_view_mine" %>


<script type="text/javascript">
    function ViewResource(strUrl) {
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
                        <td></td>
                        <td><asp:LinkButton ID="btnOrderNumber" runat="server" CssClass="tableheader" Text="<b>Number</b>" OnClick="btnOrder_Click" CommandArgument="number" /></td>
                        <td><asp:LinkButton ID="btnOrderTitle" runat="server" CssClass="tableheader" Text="<b>Title</b>" OnClick="btnOrder_Click" CommandArgument="name" /></td>
                        <td>&nbsp;</td>
                        <td class="tableheader"><b>Selected Services</b></td>
                        <td>&nbsp;</td>
                        <td><asp:LinkButton ID="btnOrderSubmitted" runat="server" CssClass="tableheader" Text="<b>Submitted</b>" OnClick="btnOrder_Click" CommandArgument="modified" /></td>
                        <td>&nbsp;</td>
                        <td><asp:LinkButton ID="btnOrderStatus" runat="server" CssClass="tableheader" Text="<b>Status</b>" OnClick="btnOrder_Click" CommandArgument="checkout" /></td>
                    </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr class="default" onmouseover="CellRowOver(this);" onmouseout="CellRowOut(this);" onclick="ViewResource('<%# strRedirect + "?rid=" + DataBinder.Eval(Container.DataItem, "requestid") %>');">
                        <asp:label ID="lblCompleted" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "completed")%>' Visible="false" />
                        <asp:label ID="lblRejected" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "rejected")%>' Visible="false" />
                        <td><input type="checkbox" onclick="ChangeCheckItems('<%=hdnDelete.ClientID %>', '<%# DataBinder.Eval(Container.DataItem, "requestid")%>', this)" /></td>
                        <td width="20%"><%# (DataBinder.Eval(Container.DataItem, "number").ToString() == "" ? "---" : DataBinder.Eval(Container.DataItem, "number"))%></td>
                        <td width="25%"><%# (DataBinder.Eval(Container.DataItem, "name").ToString() == "" ? "---" : DataBinder.Eval(Container.DataItem, "name"))%></td>
                        <td>&nbsp;</td>
                        <td width="30%">
                            <table cellpadding="3" cellspacing="0" border="0">
                                <asp:repeater id="rptServices" datasource='<%# ((DataRowView)Container.DataItem).Row.GetChildRows("relationship") %>' runat="server">
                                    <ItemTemplate>
                                        <tr>
                                            <td>[<%# DataBinder.Eval(Container.DataItem, "[\"quantity\"]")%>]</td>
                                            <td><%# DataBinder.Eval(Container.DataItem, "[\"name\"]")%></td>
                                            <asp:label ID="lblRRStatus" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "[\"RRStatus\"]")%>' Visible="false" />
                                            <asp:label ID="lblApproved" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "[\"approved\"]")%>' Visible="false" />
                                        </tr>
                                    </ItemTemplate>
                                </asp:repeater>
                            </table>
                        </td>
                        <td>&nbsp;</td>
                        <td width="15%"><%# DateTime.Parse(DataBinder.Eval(Container.DataItem, "modified").ToString()).ToShortDateString() %></td>
                        <td>&nbsp;</td>
                        <td width="10%"><asp:label ID="lblCheckout" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "checkout")%>' /></td>
                    </tr>
                </ItemTemplate>
                <AlternatingItemTemplate>
                    <tr class="default" bgcolor="#F6F6F6" onmouseover="CellRowOver(this);" onmouseout="CellRowOut(this);" onclick="ViewResource('<%# strRedirect + "?rid=" + DataBinder.Eval(Container.DataItem, "requestid") %>');">
                        <asp:label ID="lblCompleted" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "completed")%>' Visible="false" />
                        <asp:label ID="lblRejected" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "rejected")%>' Visible="false" />
                        <td><input type="checkbox" onclick="ChangeCheckItems('<%=hdnDelete.ClientID %>', '<%# DataBinder.Eval(Container.DataItem, "requestid")%>', this)" /></td>
                        <td width="20%"><%# (DataBinder.Eval(Container.DataItem, "number").ToString() == "" ? "---" : DataBinder.Eval(Container.DataItem, "number"))%></td>
                        <td width="25%"><%# (DataBinder.Eval(Container.DataItem, "name").ToString() == "" ? "---" : DataBinder.Eval(Container.DataItem, "name"))%></td>
                        <td>&nbsp;</td>
                        <td width="30%">
                            <table cellpadding="3" cellspacing="0" border="0">
                                <asp:repeater id="rptServices" datasource='<%# ((DataRowView)Container.DataItem).Row.GetChildRows("relationship") %>' runat="server">
                                    <ItemTemplate>
                                        <tr>
                                            <td>[<%# DataBinder.Eval(Container.DataItem, "[\"quantity\"]")%>]</td>
                                            <td><%# DataBinder.Eval(Container.DataItem, "[\"name\"]")%></td>
                                            <asp:label ID="lblRRStatus" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "[\"RRStatus\"]")%>' Visible="false" />
                                            <asp:label ID="lblApproved" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "[\"approved\"]")%>' Visible="false" />
                                        </tr>
                                    </ItemTemplate>
                                </asp:repeater>
                            </table>
                        </td>
                        <td>&nbsp;</td>
                        <td width="15%"><%# DateTime.Parse(DataBinder.Eval(Container.DataItem, "modified").ToString()).ToShortDateString() %></td>
                        <td>&nbsp;</td>
                        <td width="10%"><asp:label ID="lblCheckout" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "checkout")%>' /></td>
                    </tr>
                </AlternatingItemTemplate>
            </asp:repeater>
            <tr>
                <td colspan="10"><asp:Label ID="lblNone" runat="server" CssClass="error" Visible="false" Text="<img src='/images/alert.gif' border='0' align='absmiddle'> There are no items" /></td>
            </tr>
            </table>
            <br />
            <table width="100%" cellpadding="3" cellspacing="0" border="0">
                <tr>
                    <td><asp:Button ID="btnDelete" runat="server" OnClick="btnDelete_Click" Text="Delete Selected" CssClass="default" Width="125" /></td>
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
<asp:HiddenField ID="hdnDelete" runat="server" />