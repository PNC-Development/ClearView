<%@ Page Language="C#" MasterPageFile="~/clearview.Master" AutoEventWireup="true" CodeBehind="email.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.email" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" Runat="Server">
<script type="text/javascript">
</script>
<table width="100%" height="100%" border="0" cellspacing="0" cellpadding="0">
    <tr height="1" style="display:none">
        <td bgcolor="#007253"><img src="/images/header_wide.aspx" border="0" /></td>
        <td bgcolor="#007253" align="right"></td>
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
        <td colspan="2" height="26" background="/images/button_back.gif" colspan="5"><asp:PlaceHolder ID="PH1" runat="server" /></td>
    </tr>
    <tr> 
        <td colspan="2" bgcolor="E9E9E9">
            <table width="98%" height="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                <tr height="1">
                    <td align="left" valign="top">&nbsp;</td>
                </tr>
                <tr> 
                    <td align="left" valign="top">
<table width="100%" cellpadding="0" cellspacing="0" border="0">
    <tr>
        <td align="center">
<table width="600" cellpadding="0" cellspacing="0" border="0">
    <tr>
        <td><img src="/images/table_topleft.gif" border="0" width="5" height="26"></td>
        <td nowrap background="/images/table_top.gif" class="greentableheader" width="100%"><asp:Label ID="lblTitle" runat="server" CssClass="greentableheader" /></td>
        <td><img src="/images/table_topright.gif" border="0" width="5" height="26"></td>
    </tr>
    <tr>
        <td background="/images/table_left.gif"><img src="/images/table_left.gif" width="5" height="10"></td>
        <td width="100%" bgcolor="#FFFFFF">
            <asp:Panel ID="panForm" runat="server" Visible="false">
            <table width="100%" cellpadding="2" cellspacing="2" border="0">
                <tr>
                    <td colspan="2">&nbsp;</td>
                </tr>
                <tr>
                    <td>From:</td>
                    <td><asp:TextBox ID="txtFrom" runat="server" Width="250" CssClass="default" /> <asp:CheckBox ID="chkFrom" runat="server" Text="Resolve AD" /></td>
                </tr>
                <tr>
                    <td>To:</td>
                    <td><asp:TextBox ID="txtTo" runat="server" Width="250" CssClass="default" /> <asp:CheckBox ID="chkTo" runat="server" Text="Resolve AD" /></td>
                </tr>
                <tr>
                    <td>Subject:</td>
                    <td><asp:TextBox ID="txtSubject" runat="server" Width="450" CssClass="default" /></td>
                </tr>
                <tr>
                    <td>HTML:</td>
                    <td><asp:CheckBox ID="chkHTML" runat="server" /></td>
                </tr>
                <tr>
                    <td valign="top">Message:</td>
                    <td><asp:TextBox ID="txtMessage" runat="server" Width="450" CssClass="default" TextMode="multiLine" Rows="10" /></td>
                </tr>
                <tr>
                    <td></td>
                    <td><asp:CheckBox ID="chkSettings" runat="server" Text="Use Default Mail Settings" Checked="true" OnCheckedChanged="chkSettings_CheckedChanged" AutoPostBack="true" /></td>
                </tr>
                <tr id="trSettings" runat="server" visible="false">
                    <td valign="top">SMTP Server:</td>
                    <td><asp:TextBox ID="txtSMTP" runat="server" Width="200" CssClass="default" /></td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td><asp:Button ID="btnSend" Text="Send" Width="75" CssClass="default" runat="server" OnClick="btnSend_Click" /></td>
                </tr>
                <tr>
                    <td colspan="2" align="center" class="header">
                        <asp:Label ID="lblError" runat="server" Text="<img src='/images/bigX.gif' border='0' align='absmiddle' /> You do not have rights to send emails" Visible="false" />
                    </td>
                </tr>
            </table>
            </asp:Panel>
            <asp:Panel ID="panFinish" runat="server" Visible="false">
                <table width="100%" cellpadding="2" cellspacing="2" border="0">
                    <tr>
                        <td><img src="/images/spacer.gif" border="0" width="15" height="1" /></td>
                        <td class="header"><img src="/images/bigCheck.gif" border="0" align="absmiddle" /> Message Sent</td>
                    </tr>
                    <tr><td colspan="2">&nbsp;</td></tr>
                    <tr>
                        <td><img src="/images/spacer.gif" border="0" width="15" height="1" /></td>
                        <td>Your message has been sent.</td>
                    </tr>
                    <tr><td colspan="2">&nbsp;</td></tr>
                    <tr>
                        <td colspan="2"><hr size="1" noshade /></td>
                    </tr>
                    <tr>
                        <td colspan="2" align="right">
                            <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                <tr>
                                    <td class="footer"></td>
                                    <td align="right"><asp:Button ID="btnFinish" runat="server" CssClass="default" Width="75" Text="Finish" /></td>
                                </tr>
                            </table>
                        </td>
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
        </td>
    </tr>
</table>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
</asp:Content>