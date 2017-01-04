<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="mnemonics_control.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.mnemonics_control" %>


<table width="100%" cellpadding="0" cellspacing="0" border="0" class="default">
    <tr>
        <td><img src="/images/table_topleft.gif" border="0" width="5" height="26"></td>
        <td nowrap background="/images/table_top.gif" class="greentableheader" width="100%"><asp:Label ID="lblTitle" runat="server" CssClass="greentableheader" /></td>
        <td><img src="/images/table_topright.gif" border="0" width="5" height="26"></td>
    </tr>
    <tr>
        <td background="/images/table_left.gif"><img src="/images/table_left.gif" width="5" height="10"></td>
        <td width="100%" bgcolor="#FFFFFF">
            <br />
            <div style="height:300px; overflow:auto">
                <table width="100%" cellpadding="3" cellspacing="0" border="0">
                    <tr>
                        <td>
                            <table width="100%" cellpadding="4" cellspacing="0" border="0" align="center">
                                <tr>
                                    <td class="header">Recently Added (within the last 7 days)</td>
                                    <td align="right">
                                        <table cellpadding="4" cellspacing="0" border="0">
                                            <tr>
                                                <td>Order By:</td>
                                                <td>
                                                    <asp:DropDownList ID="ddlOrder" runat="server" CssClass="default" OnSelectedIndexChanged="ddlOrder_Change" AutoPostBack="true">
                                                        <asp:ListItem Text="Code" Value="factory_code" />
                                                        <asp:ListItem Text="Name" Value="name" />
                                                        <asp:ListItem Text="Created" Value="created DESC" />
                                                    </asp:DropDownList>
                                                </td>
                                                <td><img src="/images/spacer.gif" border="0" width="15" height="1" /></td>
                                                <td>Filter:</td>
                                                <td><asp:TextBox ID="txtFilter" runat="server" CssClass="default" Width="100" MaxLength="50" /></td>
                                                <td><asp:ImageButton ID="btnFilter" runat="server" CssClass="default" ImageUrl="/images/go.gif" ImageAlign="AbsMiddle" OnClick="btnFilter_Click" ToolTip="Apply Filter" /></td>
                                                <td><asp:ImageButton ID="btnFilterRemove" runat="server" CssClass="default" ImageUrl="/images/cancel.gif" ImageAlign="AbsMiddle" OnClick="btnFilterRemove_Click" ToolTip="Clear Filter" /></td>
                                            </tr>
                                            <tr id="trWait" runat="server" style="display:none">
                                                <td colspan="7" align="center" class="bold">
                                                    <img src="/images/spacer.gif" border="0" width="20" height="1" /><img src="/images/please_wait.gif" border="0" align="absmiddle" /> Please Wait...
                                                </td>
                                            </tr>
                                        </table>
                                </td>
                                    </td>
                                </tr>
                                <asp:repeater ID="rptNew" runat="server">
                                    <ItemTemplate>
                                        <tr>
                                            <td colspan="2">
                                                <table cellpadding="4" cellspacing="0" border="0">
                                                    <tr>
                                                        <td rowspan="3" valign="top"><img src="/images/new.gif" border="0" align="absmiddle" /></td>
                                                        <td class="header" valign="bottom" colspan="2"><%# DataBinder.Eval(Container.DataItem, "factory_code") %></td>
                                                    </tr>
                                                    <tr>
                                                        <td valign="bottom">Name:</td>
                                                        <td valign="bottom"><%# DataBinder.Eval(Container.DataItem, "name") %></td>
                                                    </tr>
                                                    <tr>
                                                        <td valign="top">Created:</td>
                                                        <td valign="top"><%# DateTime.Parse(DataBinder.Eval(Container.DataItem, "created").ToString()).ToString() %></td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:repeater>
                                <tr>
                                    <td colspan="2">
                                        <asp:Label ID="lblNew" runat="server" CssClass="default" Visible="false" Text="<img src='/images/spacer.gif' border='0' height='1' width='25' /><img src='/images/alert.gif' border='0' align='absmiddle'> There are no new mnemonics" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2"><span style="width:100%;border-bottom:1 dotted #999999;"/></td>
                                </tr>
                            </table>
                            <br />
                            <table width="100%" cellpadding="4" cellspacing="0" border="0" align="center">
                                <tr id="trFilter" runat="server" visible="false">
                                    <td colspan="4" class="biggerreddefault"><asp:Literal ID="litFilter" runat="server" /></td>
                                </tr>
                                <tr>
                                    <td><b><u></u></b></td>
                                    <td><b><u>Code:</u></b></td>
                                    <td><b><u>Name:</u></b></td>
                                    <td><b><u>Created:</u></b></td>
                                </tr>
                                <asp:repeater ID="rptOld" runat="server">
                                    <ItemTemplate>
                                        <tr>
                                            <td><img src="/images/spacer.gif" border="0" width="20" height="1" /></td>
                                            <td><%# DataBinder.Eval(Container.DataItem, "factory_code")%></td>
                                            <td><%# DataBinder.Eval(Container.DataItem, "name") %></td>
                                            <td><%# DateTime.Parse(DataBinder.Eval(Container.DataItem, "created").ToString()).ToString()%></td>
                                        </tr>
                                    </ItemTemplate>
                                    <AlternatingItemTemplate>
                                        <tr bgcolor="#EEEEEE">
                                            <td><img src="/images/spacer.gif" border="0" width="20" height="1" /></td>
                                            <td><%# DataBinder.Eval(Container.DataItem, "factory_code")%></td>
                                            <td><%# DataBinder.Eval(Container.DataItem, "name") %></td>
                                            <td><%# DateTime.Parse(DataBinder.Eval(Container.DataItem, "created").ToString()).ToString()%></td>
                                        </tr>
                                    </AlternatingItemTemplate>
                                </asp:repeater>
                                <tr>
                                    <td colspan="4">
                                        <asp:Label ID="lblOld" runat="server" CssClass="default" Visible="false" Text="<img src='/images/spacer.gif' border='0' height='1' width='25' /><img src='/images/alert.gif' border='0' align='absmiddle'> There are no old mnemonics" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </div>
            <p>&nbsp;</p>
        </td>
        <td background="/images/table_right.gif"><img src="/images/table_right.gif" width="5" height="10"></td>
    </tr>
    <tr>
        <td><img src="/images/table_bottomLeft.gif" border="0" width="5" height="9"></td>
        <td width="100%" background="/images/table_bottom.gif"></td>
        <td><img src="/images/table_bottomRight.gif" border="0" width="5" height="9"></td>
    </tr>
</table>