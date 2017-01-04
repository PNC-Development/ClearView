<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="enhancement_mine.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.enhancement_mine" %>



<script type="text/javascript">
    function ViewSupport(strID, strNew) {
        if (strNew == "")
            window.navigate('<%=strRedirect %>?id=' + strID);
        else
            window.navigate('<%=strRedirect %>?old=' + strID);
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
            <fieldset>
                <legend class="tableheader"><b>Filtering Otions</b></legend>
                <table width="100%" cellpadding="4" cellspacing="0" border="0">
                    <tr>
                        <td><asp:Label ID="lblStatus" runat="server" CssClass="default" Text="Status:" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:CheckBox ID="chkAll" runat="server" CssClass="default" Text="Select All" /></td>
                        <td><asp:Label ID="lblSubmittedDate" runat="server" CssClass="default" Text="Submitted Date Range:" /></td>
                    </tr>
                    <tr>
                        <td rowspan="2" width="60%"><asp:CheckBoxList ID="lstStatus" runat="server" CssClass="smalldefault" RepeatColumns="4" RepeatDirection="Vertical" /> </td>
                        <td width="40%" valign="top"><asp:TextBox ID="txtStart" runat="server" CssClass="smalldefault" Width="70" MaxLength="10" /> <asp:ImageButton ID="imgStart" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calendar.gif" /> - <asp:TextBox ID="txtEnd" runat="server" CssClass="smalldefault" Width="70" MaxLength="10" /> <asp:ImageButton ID="imgEnd" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calendar.gif" /></td>
                    </tr>
                    <tr>
                        <td width="40%" valign="bottom"><asp:Button ID="btnFilter" runat="server" CssClass="default" Width="100" Text="Apply Filter" OnClick="btnFilter_Click" /></td>
                    </tr>
                    
                </table>
            </fieldset>
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
                        <td><asp:LinkButton ID="btnOrderRequest" runat="server" CssClass="tableheader" Text="<b>RequestID</b>" OnClick="btnOrder_Click" CommandArgument="requestid"/></td>                                                                                                    
                        <td><asp:LinkButton ID="btnOrderTitle" runat="server" CssClass="tableheader" Text="<b>Title</b>" OnClick="btnOrder_Click" CommandArgument="title"/></td>                                                                                                    
                        <td><asp:LinkButton ID="btnOrderModule" runat="server" CssClass="tableheader" Text="<b>Module</b>" OnClick="btnOrder_Click" CommandArgument="module"/></td>                         
                        <td><asp:LinkButton ID="btnOrderDateSubmitted" runat="server" CssClass="tableheader" Text="<b>Submitted</b>" OnClick="btnOrder_Click" CommandArgument="created"/></td>                        
                        <td><asp:LinkButton ID="btnOrderDateUpdated" runat="server" CssClass="tableheader" Text="<b>Updated</b>" OnClick="btnOrder_Click" CommandArgument="modified"/></td>
                        <td><asp:LinkButton ID="btnOrderDateCompleted" runat="server" CssClass="tableheader" Text="<b>Completed</b>" OnClick="btnOrder_Click" CommandArgument="completed"/></td>
                        <td><asp:LinkButton ID="btnOrderStatus" runat="server" CssClass="tableheader" Text="<b>Status</b>" OnClick="btnOrder_Click" CommandArgument="status"/></td> 
                    </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr class='<%# (DataBinder.Eval(Container.DataItem, "new").ToString() == "1" ? "bold" : "default") %>' onmouseover="CellRowOver(this);" onmouseout="CellRowOut(this);" onclick="ViewSupport('<%# DataBinder.Eval(Container.DataItem, "id").ToString()%>','<%# DataBinder.Eval(Container.DataItem, "new").ToString()%>');">
                        <td valign="top" nowrap>CVT<%# DataBinder.Eval(Container.DataItem, "requestid") %></td>
                        <td valign="top" nowrap><%# DataBinder.Eval(Container.DataItem, "title") %></td>
                        <td valign="top" nowrap><%# DataBinder.Eval(Container.DataItem, "module") %></td>                         
                        <td valign="top" nowrap><%# DateTime.Parse(DataBinder.Eval(Container.DataItem, "created").ToString()).ToShortDateString()%></td>                         
                        <td valign="top" nowrap><%# DataBinder.Eval(Container.DataItem, "modified") == DBNull.Value ? "N / A": DateTime.Parse(DataBinder.Eval(Container.DataItem, "modified").ToString()).ToShortDateString() %></td>                        
                        <td valign="top" nowrap><%# DataBinder.Eval(Container.DataItem, "completed") == DBNull.Value ? "N / A": DateTime.Parse(DataBinder.Eval(Container.DataItem, "completed").ToString()).ToShortDateString() %></td>                        
                        <td valign="top" nowrap><%# DataBinder.Eval(Container.DataItem, "new").ToString() == "" ? oEnhancement.Status(Int32.Parse(DataBinder.Eval(Container.DataItem, "id").ToString())) : oStatusLevel.HTMLSupport(Int32.Parse(DataBinder.Eval(Container.DataItem, "status").ToString()))%></td>
                    </tr>
                </ItemTemplate>    
                <AlternatingItemTemplate>
                    <tr class='<%# (DataBinder.Eval(Container.DataItem, "new").ToString() == "1" ? "bold" : "default") %>' bgcolor="#F6F6F6" onmouseover="CellRowOver(this);" onmouseout="CellRowOut(this);" onclick="ViewSupport('<%# DataBinder.Eval(Container.DataItem, "id").ToString()%>','<%# DataBinder.Eval(Container.DataItem, "new").ToString()%>');">
                        <td valign="top" nowrap>CVT<%# DataBinder.Eval(Container.DataItem, "requestid") %></td>
                        <td valign="top" nowrap><%# DataBinder.Eval(Container.DataItem, "title") %></td>
                        <td valign="top" nowrap><%# DataBinder.Eval(Container.DataItem, "module") %></td>                         
                        <td valign="top" nowrap><%# DateTime.Parse(DataBinder.Eval(Container.DataItem, "created").ToString()).ToShortDateString()%></td>                         
                        <td valign="top" nowrap><%# DataBinder.Eval(Container.DataItem, "modified") == DBNull.Value ? "N / A":DateTime.Parse(DataBinder.Eval(Container.DataItem, "modified").ToString()).ToShortDateString() %></td>                        
                        <td valign="top" nowrap><%# DataBinder.Eval(Container.DataItem, "completed") == DBNull.Value ? "N / A":DateTime.Parse(DataBinder.Eval(Container.DataItem, "completed").ToString()).ToShortDateString() %></td>                        
                        <td valign="top" nowrap><%# DataBinder.Eval(Container.DataItem, "new").ToString() == "" ? oEnhancement.Status(Int32.Parse(DataBinder.Eval(Container.DataItem, "id").ToString())) : oStatusLevel.HTMLSupport(Int32.Parse(DataBinder.Eval(Container.DataItem, "status").ToString()))%></td>
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