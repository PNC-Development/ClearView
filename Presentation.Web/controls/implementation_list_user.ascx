<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="implementation_list_user.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.implementation_list_user" %>


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
            <table width="100%" cellpadding="3" cellspacing="0" border="0">
                <tr>
                    <td>
                        <table width="100%" cellpadding="4" cellspacing="0" border="0" align="center">
                            <tr>
                                <td><b><u>Change Number:</u></b></td>
                                <td><b><u>Time:</u></b></td>
                                <td><b><u>Project Name:</u></b></td>
                                <td><b><u>Project Number:</u></b></td>
                            </tr>
                            <asp:repeater ID="rptView" runat="server">
                                <ItemTemplate>
                                    <tr>
                                        <td><a href="javascript:void(0);" onclick="ShowChange('<%# DataBinder.Eval(Container.DataItem, "changeid") %>');"><%# DataBinder.Eval(Container.DataItem, "number") %></a></td>
                                        <td><%# DateTime.Parse(DataBinder.Eval(Container.DataItem, "implementation").ToString()).ToShortTimeString()%></td>
                                        <td><%# DataBinder.Eval(Container.DataItem, "projectname") %></td>
                                        <td><%# DataBinder.Eval(Container.DataItem, "projectnumber") %></td>
                                    </tr>
                                </ItemTemplate>
                            </asp:repeater>
                            <tr>
                                <td colspan="4">
                                    <asp:Label ID="lblNone" runat="server" CssClass="default" Visible="false" Text="<img src='/images/spacer.gif' border='0' height='1' width='25' /><img src='/images/alert.gif' border='0' align='absmiddle'> No scheduled changes" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
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