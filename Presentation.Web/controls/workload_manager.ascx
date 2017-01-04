<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="workload_manager.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.workload_manager" %>


<script type="text/javascript">
    function ViewWorkload(strID, strNew) {
        if (strNew == "0")
            window.navigate('<%=strRedirect %>?pid=' + strID);
        else
            OpenWindow("RESOURCE_REQUEST", strID);
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
                        <table cellpadding="0" cellspacing="0" border="0">
                            <tr>
                                <td>
                                    <b>Page <asp:TextBox ID="txtPage" runat="server" CssClass="default" Width="25" /> of <asp:Label ID="lblPages" runat="server" /> <asp:ImageButton ID="btnPage" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/search.gif" OnClick="btnPage_Click" ToolTip="Go to this page" /></b>
                                    <b><asp:Label ID="lblRecords" runat="server" Visible="false" /></b>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td align="center">
                        <table cellpadding="0" cellspacing="0" border="0">
                            <tr>
                                <td>Request #:</td>
                                <td><img src="/images/spacer.gif" border="0" width="10" height="1" /></td>
                                <td><asp:TextBox ID="txtRequest" runat="server" CssClass="default" Width="75" MaxLength="10" /></td>
                                <td><img src="/images/spacer.gif" border="0" width="10" height="1" /></td>
                                <td><asp:Button ID="btnGo" runat="server" CssClass="default" Width="50" Text="Go" OnClick="btnGo_Click" /></td>
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
                        <td><asp:LinkButton ID="btnOrderNumber" runat="server" CssClass="tableheader" Text="<b>Number</b>" OnClick="btnOrder_Click" CommandArgument="number" /></td>
                        <td><asp:LinkButton ID="btnOrderName" runat="server" CssClass="tableheader" Text="<b>Name</b>" OnClick="btnOrder_Click" CommandArgument="name" /></td>
                        <td><asp:LinkButton ID="btnOrderSLA" runat="server" CssClass="tableheader" Text="<b>SLA</b>" OnClick="btnOrder_Click" CommandArgument="modified" /></td>
                        <td><asp:LinkButton ID="btnOrderElapsed" runat="server" CssClass="tableheader" Text="<b>Elapsed Days</b>" OnClick="btnOrder_Click" CommandArgument="modified" /></td>
                        <td><asp:LinkButton ID="btnOrderUserName" runat="server" CssClass="tableheader" Text="<b>Assigned To</b>" OnClick="btnOrder_Click" CommandArgument="username" /></td>
                        <td><asp:LinkButton ID="btnOrderSubmitted" runat="server" CssClass="tableheader" Text="<b>Submitted</b>" OnClick="btnOrder_Click" CommandArgument="modified" /></td>
                    </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr class='<%# (DataBinder.Eval(Container.DataItem, "new").ToString() == "1" ? "bold" : "default") %>' onmouseover="CellRowOver(this);" onmouseout="CellRowOut(this);" onclick="ViewWorkload('<%# DataBinder.Eval(Container.DataItem, "id") %>','<%# DataBinder.Eval(Container.DataItem, "newwindow") %>');">
                        <td width="1" valign="top" align="right"><asp:Label ID="lblColor" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "green").ToString() + "_" + DataBinder.Eval(Container.DataItem, "yellow").ToString() + "_" + DataBinder.Eval(Container.DataItem, "red").ToString() %>' ToolTip='<%# DataBinder.Eval(Container.DataItem, "comments").ToString() %>' /></td>
                        <td valign="top"><asp:label ID="lblNumber" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "number") %>' /></td>
                        <td valign="top"><%# DataBinder.Eval(Container.DataItem, "name") %></td>
                        <td valign="top"><%# DataBinder.Eval(Container.DataItem, "SLA") %> days</td>
                        <td valign="top"><%# DataBinder.Eval(Container.DataItem, "ElapsedDays") %> days</td>
                        <td valign="top"><%# DataBinder.Eval(Container.DataItem, "username") %></td>
                        <td valign="top"><asp:label ID="lblSubmitted" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "id") %>' /></td>
                    </tr>
                </ItemTemplate>
                <AlternatingItemTemplate>
                    <tr class='<%# (DataBinder.Eval(Container.DataItem, "new").ToString() == "1" ? "bold" : "default") %>' bgcolor="#F6F6F6" onmouseover="CellRowOver(this);" onmouseout="CellRowOut(this);" onclick="ViewWorkload('<%# DataBinder.Eval(Container.DataItem, "id") %>','<%# DataBinder.Eval(Container.DataItem, "newwindow") %>');">
                        <td width="1" valign="top" align="right"><asp:Label ID="lblColor" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "green").ToString() + "_" + DataBinder.Eval(Container.DataItem, "yellow").ToString() + "_" + DataBinder.Eval(Container.DataItem, "red").ToString() %>' ToolTip='<%# DataBinder.Eval(Container.DataItem, "comments").ToString() %>' /></td>
                        <td valign="top"><asp:label ID="lblNumber" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "number") %>' /></td>
                        <td valign="top"><%# DataBinder.Eval(Container.DataItem, "name") %></td>
                        <td valign="top"><%# DataBinder.Eval(Container.DataItem, "SLA") %> days</td>
                        <td valign="top"><%# DataBinder.Eval(Container.DataItem, "ElapsedDays") %> days</td>
                        <td valign="top"><%# DataBinder.Eval(Container.DataItem, "username") %></td>
                        <td valign="top"><asp:label ID="lblSubmitted" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "id") %>' /></td>
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
                    <td></td>
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