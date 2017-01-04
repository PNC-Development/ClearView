<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="rss_hp_server.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.rss_hp_server" %>


<script type="text/javascript">
    function OpenRss(strUrl) {
        window.open(strUrl);
    }
</script>
<br />
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
            <asp:Panel ID="panRSS" runat="server" Visible="false">
            <table width="100%" cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td>
                        <table width="100%" cellpadding="4" cellspacing="0" border="0" align="center">
                            <tr>
                                <td><b>Title</b></td>
                                <td><b>Description</b></td>
                                <td><b>Date</b></td>
                            </tr>
                            <asp:repeater ID="rptView" runat="server">
                                <ItemTemplate>
                                    <tr>
                                        <td valign="top"><a href='<%# DataBinder.Eval(Container.DataItem, "link") %>' target="_blank"><%# DataBinder.Eval(Container.DataItem, "title") %></a></td>
                                        <td valign="top"><%# DataBinder.Eval(Container.DataItem, "description") %></td>
                                        <td valign="top"><%# DateTime.Parse(DataBinder.Eval(Container.DataItem, "pubdate").ToString()).ToString() %></td>
                                    </tr>
                                </ItemTemplate>
                            </asp:repeater>
                            <tr>
                                <td colspan="3" class="default">
                                    <asp:Label ID="lblNone" runat="server" Visible="false" Text="<img src='/images/alert.gif' border='0' align='absmiddle'> There are no items" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td align="center">
                        <table cellpadding="3" cellspacing="2" border="0" class="offsettable">
                            <tr>
                                <td class="bigred">Proxy Authenticated!!</td>
                            </tr>
                            <tr>
                                <td class="error">You are accessing an external website using cached proxy credentials. To clear these credentials, click on the following button.</td>
                            </tr>
                            <tr>
                                <td align="center"><asp:Button ID="btnLogout" runat="server" CssClass="default" Text="Clear Credentials" Width="150" OnClick="btnLogout_Click" /></td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            </asp:Panel>
            <asp:Panel ID="panLogin" runat="server" Visible="false">
                <table cellpadding="2" cellspacing="0" border="0">
                    <tr>
                        <td colspan="2" class="header"><img src="/images/bigalert.gif" border="0" align="absmiddle" /> Proxy Authentication Required</td>
                    </tr>
                    <tr>
                        <td>Username</td>
                        <td><asp:TextBox ID="txtUsername" runat="server" Width="200" CssClass="default" MaxLength="50" /></td>
                    </tr>
                    <tr>
                        <td>Password</td>
                        <td><asp:TextBox ID="txtPassword" runat="server" Width="200" CssClass="default" MaxLength="50" TextMode="Password" /></td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                        <td><asp:CheckBox ID="chkRemember" runat="server" CssClass="footer" Text="Remember my proxy credentials" /></td>
                    </tr>
                    <tr height="5">
                        <td colspan="2"><img src="/images/spacer.gif" border="0" height="5" width="1" /></td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                        <td><asp:Button ID="btnLogin" runat="server" Text="Login" Width="75" CssClass="default" OnClick="btnLogin_Click" /></td>
                    </tr>
                    <tr height="5">
                        <td colspan="2"><img src="/images/spacer.gif" border="0" height="5" width="1" /></td>
                    </tr>
                    <tr>
                        <td colspan="2" class="error"><b>NOTE:</b> You are attempting to access an external website. Please enter your proxy credentials.</td>
                    </tr>
                </table>
            </asp:Panel>
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