<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="_deleted_sys_new.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.sys_new" %>


<table width="100%" cellpadding="0" cellspacing="0" border="0" class="default">
    <tr>
        <td><img src="/images/table_topleft.gif" border="0" width="5" height="26"></td>
        <td nowrap background="/images/table_top.gif" class="greentableheader" width="100%">What's New in Clearview</td>
        <td><img src="/images/table_topright.gif" border="0" width="5" height="26"></td>
    </tr>
    <tr>
        <td background="/images/table_left.gif"><img src="/images/table_left.gif" width="5" height="10"></td>
        <td width="100%" bgcolor="#FFFFFF">
            <br />
            <asp:repeater ID="rptNew" runat="server">
                <ItemTemplate>
                    <table cellpadding="4" cellspacing="0" border="0">
                        <tr>
                            <td rowspan="2" valign="top"><%# (DateTime.Parse(DataBinder.Eval(Container.DataItem, "created").ToString()).AddDays(14) >= DateTime.Now ? "<img src='/images/new.gif' border='0' align='absmiddle' />" : "<img src='/images/spacer.gif' border='0' width='40' height='38' />")%></td>
                            <td class="header" valign="bottom"><%# DataBinder.Eval(Container.DataItem, "title") %></td>
                        </tr>
                        <tr>
                            <td valign="top" colspan="2"><%# oFunction.FormatText(DataBinder.Eval(Container.DataItem, "description").ToString()) %></td>
                        </tr>
                        <asp:Panel ID="panAttachment" runat="server" Visible="false">
                        <tr>
                            <td></td>
                            <td class="default" colspan="2"><asp:Label ID="lblAttachment" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "attachment") %>' /></td>
                        </tr>
                        </asp:Panel>
                        <tr>
                            <td></td>
                            <td class="help"> - Modified On <%# DateTime.Parse(DataBinder.Eval(Container.DataItem, "modified").ToString()).ToString() %></td>
                        </tr>
                    </table>
                </ItemTemplate>
                <SeparatorTemplate>
                    <table width="100%" cellpadding="4" cellspacing="0" border="0">
                        <tr>
                            <td><span style="width:100%;border-bottom:1 dotted #999999;"/></td>
                        </tr>
                    </table>
                </SeparatorTemplate>
            </asp:repeater>
            <asp:Label ID="lblNew" runat="server" CssClass="default" Visible="false" Text="<img src='/images/alert.gif' border='0' align='absmiddle'> There are no updates" />
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