<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="costavoidance_mine.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.costavoidance_mine" %>


<script type="text/javascript">
    function ViewCostAvoidance(strID, strNew) {
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
            <table width="100%" cellpadding="3" cellspacing="0" border="0" align="center" style="border:solid 1px #CCCCCC">
            <asp:repeater ID="rptView" runat="server">
                <HeaderTemplate>
                    <tr bgcolor="#EEEEEE">
                        <td><asp:LinkButton ID="btnOrderCOA" runat="server" CssClass="tableheader" Text="<b>Opportunity</b>" OnClick="btnOrder_Click" CommandArgument="opportunity"/></td>                                                                                                    
                        <td><asp:LinkButton ID="btnOrderDescription" runat="server" CssClass="tableheader" Text="<b>Description</b>" OnClick="btnOrder_Click" CommandArgument="description"/></td>
                        <td><asp:LinkButton ID="btnOrderDate" runat="server" CssClass="tableheader" Text="<b>Date</b>" OnClick="btnOrder_Click" CommandArgument="date"/></td>                         
                        <td><asp:LinkButton ID="btnOrderDateSubmitted" runat="server" CssClass="tableheader" Text="<b>Date Submitted</b>" OnClick="btnOrder_Click" CommandArgument="created"/></td>                        
                        <td><asp:LinkButton ID="btnOrderDateUpdated" runat="server" CssClass="tableheader" Text="<b>Date Updated</b>" OnClick="btnOrder_Click" CommandArgument="modified"/></td>                         
                    </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr onmouseover="CellRowOver(this);" onmouseout="CellRowOut(this);" onclick="ViewCostAvoidance('<%# DataBinder.Eval(Container.DataItem, "id").ToString()%>','1');">
                        <td valign="top" nowrap><%# DataBinder.Eval(Container.DataItem, "opportunity") %></td>
                        <td valign="top" nowrap><%# DataBinder.Eval(Container.DataItem, "description") %></td>       
                        <td valign="top" nowrap><%# DateTime.Parse(DataBinder.Eval(Container.DataItem, "date").ToString()).ToShortDateString() %></td>                                           
                        <td valign="top" nowrap><%# DateTime.Parse(DataBinder.Eval(Container.DataItem, "created").ToString()) %></td>                         
                        <td valign="top" nowrap><%# DateTime.Parse(DataBinder.Eval(Container.DataItem, "modified").ToString())  %></td>                         
                    </tr>
                </ItemTemplate>    
                <AlternatingItemTemplate>
                     <tr bgcolor="#F6F6F6" onmouseover="CellRowOver(this);" onmouseout="CellRowOut(this);" onclick="ViewCostAvoidance('<%# DataBinder.Eval(Container.DataItem, "id").ToString()%>','1');">
                        <td valign="top" nowrap><%# DataBinder.Eval(Container.DataItem, "opportunity") %></td>
                        <td valign="top" nowrap><%# DataBinder.Eval(Container.DataItem, "description") %></td>       
                        <td valign="top" nowrap><%# DateTime.Parse(DataBinder.Eval(Container.DataItem, "date").ToString()).ToShortDateString() %></td>                                           
                        <td valign="top" nowrap><%# DateTime.Parse(DataBinder.Eval(Container.DataItem, "created").ToString()) %></td>                         
                        <td valign="top" nowrap><%# DateTime.Parse(DataBinder.Eval(Container.DataItem, "modified").ToString())  %></td>                         
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