<%@ Page Language="C#" AutoEventWireup="true" Codebehind="login.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.Login" MasterPageFile="~/clearview.master" %>
<asp:content id="Content1" contentplaceholderid="AllContent" runat="Server">
<script type="text/javascript">
</script>
<table width="100%" height="100%" border="0" cellspacing="0" cellpadding="0">
    <tr height="1" style="display:none"> 
        <td bgcolor="#007253"><img src="/images/header_wide.aspx" border="0" /></td>
        <td bgcolor="#007253" align="right"><asp:PlaceHolder ID="PH4" runat="server" /></td>
    </tr>
    <tr height="1"> 
        <td colspan="2">
            <table width="100%" border="0" cellspacing="0" cellpadding="5">
                <tr style="background-color:#FFFFFF; display:inline">
                    <td><img src="/images/PNCHeaderLogo.gif" border="0" /></td>
                    <td align="right"><img src="/images/HeaderGradient.gif" border="0" /></td>
                </tr>
                <tr style="background-color:#FFFFFF; display:none">
                    <td background="/images/PNCLogoBack.gif" width="100%"><img src="/images/PNCLogo.gif" border="0" /></td>
                    <td align="right"><img src="/images/HeaderGradient.gif" border="0" /></td>
                </tr>
                <tr>
                    <td colspan="2" align="right" class="whitedefault"><DIV id=thinOrangeBar><asp:label ID="lblName" runat="server" CssClass="whitedefault" />&nbsp;&nbsp;</DIV></td>
                </tr>
            </table>
        </td>
    </tr>
    <tr bgcolor="#000000" height="1"> 
        <td colspan="2" height="26" background="/images/button_back.gif">
            <table width="100%" cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td><asp:PlaceHolder ID="PH1" runat="server" /></td>
                    <td align="right"></td>
                </tr>
            </table>
        </td>
    </tr>
    <tr> 
        <td colspan="2" bgcolor="#E9E9E9">
            <table width="98%" height="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                <tr height="1">
                    <td align="left" valign="top">&nbsp;</td>
                </tr>
                <tr> 
                    <td align="left" valign="top">
<table width="100%" cellpadding="0" cellspacing="0" border="0" class="default">
    <tr>
        <td align="center">
            <table width="400" cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td><img src="/images/table_topleft.gif" border="0" width="5" height="26"></td>
                    <td nowrap background="/images/table_top.gif" class="greentableheader" width="100%">Login</td>
                    <td><img src="/images/table_topright.gif" border="0" width="5" height="26"></td>
                </tr>
                <tr>
                    <td background="/images/table_left.gif"><img src="/images/table_left.gif" width="5" height="10"></td>
                    <td width="100%" bgcolor="#FFFFFF">
                        <table width="100%" cellpadding="2" cellspacing="0" border="0" class="greentable">
                            <tr height="5">
                                <td colspan="2"><img src="/images/spacer.gif" border="0" height="5" width="1" /></td>
                            </tr>
                            <tr>
                                <td><asp:Label ID="lblUsername" runat="server" CssClass="default" Text="Username" /></td>
                                <td><asp:TextBox ID="txtUsername" runat="server" Width="200" CssClass="default" MaxLength="50" /></td>
                            </tr>
                            <tr>
                                <td><asp:Label ID="lblPassword" runat="server" CssClass="default" Text="Password" /></td>
                                <td><asp:TextBox ID="txtPassword" runat="server" Width="200" CssClass="default" MaxLength="50" TextMode="Password" /></td>
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                                <td><asp:CheckBox ID="chkRemember" runat="server" CssClass="footer" Text="Remember my credentials on this computer" /></td>
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
                                <td>&nbsp;</td>
                                <td><a href="/register.aspx">New to ClearView? Click Here to Register</a></td>
                            </tr>
                            <tr height="5">
                                <td colspan="2"><img src="/images/spacer.gif" border="0" height="5" width="1" /></td>
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                                <td><a href="/support.aspx">Trouble Logging In? Click Here to Contact Us</a></td>
                            </tr>
                            <tr height="5">
                                <td colspan="2"><img src="/images/spacer.gif" border="0" height="5" width="1" /></td>
                            </tr>
                            <tr>
                                <td colspan="2" align="center" class="header">
                                    <asp:Label ID="lblLogout" runat="server" Text="<img src='/images/bigCheck.gif' border='0' align='absmiddle' /> You have been logged out" Visible="false" />
                                    <asp:Label ID="lblInvalid" runat="server" Text="<img src='/images/bigError.gif' border='0' align='absmiddle' /> Invalid Username or Password" Visible="false" />
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td background="/images/table_right.gif"><img src="/images/table_right.gif" width="5" height="10"></td>
                </tr>
                <tr>
                    <td><img src="/images/table_bottomLeft.gif" border="0" width="5" height="9"></td>
                    <td width="100%" background="/images/table_bottom.gif"></td>
                    <td><img src="/images/table_bottomRight.gif" border="0" width="5" height="9"></td>
                </tr>
            </table>
        </td>
    </tr>
</table>
                        <p>&nbsp;</p>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
</asp:content>
