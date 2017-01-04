<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="fore_mine.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.fore_mine" %>
<script type="text/javascript">
    function ViewItem(strUrl) {
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
                        <td><asp:LinkButton ID="btnOrderName" runat="server" CssClass="tableheader" Text="<b>Project Name:</b>" OnClick="btnOrder_Click" CommandArgument="name" /></td>
                        <td><asp:LinkButton ID="btnOrderNumber" runat="server" CssClass="tableheader" Text="<b>Project Number:</b>" OnClick="btnOrder_Click" CommandArgument="number" /></td>
                        <td><asp:LinkButton ID="btnOrderLead" runat="server" CssClass="tableheader" Text="<b>Requestor:</b>" OnClick="btnOrder_Click" CommandArgument="manager" /></td>
                        <td><asp:LinkButton ID="btnOrderPortfolio" runat="server" CssClass="tableheader" Text="<b>Portfolio:</b>" OnClick="btnOrder_Click" CommandArgument="portfolio" /></td>
                        <td><asp:LinkButton ID="btnOrderCount" runat="server" CssClass="tableheader" Text="<b>Devices:</b>" OnClick="btnOrder_Click" CommandArgument="quantity" /></td>
                        <!--
                        <td><asp:LinkButton ID="btnOrderAmp" runat="server" CssClass="tableheader" Text="<b>AMP:</b>" OnClick="btnOrder_Click" CommandArgument="amp" /></td>
                        -->
                        <td><asp:LinkButton ID="btnOrderDate" runat="server" CssClass="tableheader" Text="<b>Commitment Date:</b>" OnClick="btnOrder_Click" CommandArgument="implementation" /></td>
                    </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr class="default" onmouseover="CellRowOver(this);" onmouseout="CellRowOut(this);" onclick="ViewItem('<%# strRedirect + "?id=" + DataBinder.Eval(Container.DataItem, "id") %>');">
                        <asp:Label ID="lblProject" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "projectid")%>' />
                        <asp:Label ID="lblRequest" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "requestid")%>' />
                        <td><%# (DataBinder.Eval(Container.DataItem, "name").ToString() == "" ? "---" : DataBinder.Eval(Container.DataItem, "name"))%></td>
                        <td><%# (DataBinder.Eval(Container.DataItem, "number").ToString() == "" ? "---" : DataBinder.Eval(Container.DataItem, "number"))%></td>
                        <td><asp:Label ID="lblLead" runat="server" CssClass="default" /></td>
                        <td><%# DataBinder.Eval(Container.DataItem, "portfolio")%></td>
                        <td><%# DataBinder.Eval(Container.DataItem, "quantity")%></td>
                        <!--
                        <td><asp:Label ID="lblAmp" runat="server" CssClass="default" /></td>
                        -->
                        <td><%# DateTime.Parse(DataBinder.Eval(Container.DataItem, "implementation").ToString()).ToShortDateString()%></td>
                    </tr>
                </ItemTemplate>
                <AlternatingItemTemplate>
                    <tr class="default" bgcolor="#F6F6F6" onmouseover="CellRowOver(this);" onmouseout="CellRowOut(this);" onclick="ViewItem('<%# strRedirect + "?id=" + DataBinder.Eval(Container.DataItem, "id") %>');">
                        <asp:Label ID="lblProject" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "projectid")%>' />
                        <asp:Label ID="lblRequest" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "requestid")%>' />
                        <td><%# (DataBinder.Eval(Container.DataItem, "name").ToString() == "" ? "---" : DataBinder.Eval(Container.DataItem, "name"))%></td>
                        <td><%# (DataBinder.Eval(Container.DataItem, "number").ToString() == "" ? "---" : DataBinder.Eval(Container.DataItem, "number"))%></td>
                        <td><asp:Label ID="lblLead" runat="server" CssClass="default" /></td>
                        <td><%# DataBinder.Eval(Container.DataItem, "portfolio")%></td>
                        <td><%# DataBinder.Eval(Container.DataItem, "quantity")%></td>
                        <!--
                        <td><asp:Label ID="lblAmp" runat="server" CssClass="default" /></td>
                        -->
                        <td><%# DateTime.Parse(DataBinder.Eval(Container.DataItem, "implementation").ToString()).ToShortDateString()%></td>
                    </tr>
                </AlternatingItemTemplate>
            </asp:repeater>
            <tr>
                <td colspan="8"><asp:Label ID="lblNone" runat="server" CssClass="error" Visible="false" Text="<img src='/images/alert.gif' border='0' align='absmiddle'> There are no items" /></td>
            </tr>
            </table>
            <br />
            <table width="100%" cellpadding="3" cellspacing="0" border="0">
                <tr>
                    <td><%=strTotals %></td>
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