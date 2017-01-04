<%@ Page Language="C#" MasterPageFile="~/clearview.Master" AutoEventWireup="true" Codebehind="error.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.error" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" runat="Server">
<script type="text/javascript">
</script>
    <table width="100%" height="100%" border="0" cellspacing="0" cellpadding="0">
        <tr height="1" style="display:none">
            <td bgcolor="#007253">
                <img src="/images/header_wide.aspx" border="0" /></td>
            <td bgcolor="#007253" align="right">
            </td>
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
            <td colspan="2" height="26" background="/images/button_back.gif" colspan="5">
                <asp:PlaceHolder ID="PH1" runat="server" />
            </td>
        </tr>
        <tr>
            <td colspan="2" bgcolor="E9E9E9">
                <table width="98%" height="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                    <tr height="1">
                        <td align="left" valign="top">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td align="left" valign="top">
                            <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                <tr>
                                    <td align="center">
                                        <table width="600" cellpadding="0" cellspacing="0" border="0">
                                            <tr>
                                                <td>
                                                    <img src="/images/table_topleft.gif" border="0" width="5" height="26"></td>
                                                <td nowrap background="/images/table_top.gif" class="greentableheader" width="100%">
                                                    <asp:Label ID="lblTitle" runat="server" CssClass="greentableheader" /></td>
                                                <td>
                                                    <img src="/images/table_topright.gif" border="0" width="5" height="26"></td>
                                            </tr>
                                            <tr>
                                                <td background="/images/table_left.gif">
                                                    <img src="/images/table_left.gif" width="5" height="10"></td>
                                                <td width="100%" bgcolor="#FFFFFF">
                                                    <br />
                                                    <table width="100%" cellpadding="2" cellspacing="2" border="0">
                                                        <tr>
                                                            <td>
                                                                <img src="/images/spacer.gif" border="0" width="15" height="1" /></td>
                                                            <td class="header">
                                                                <img src="/images/bigError2.gif" border="0" align="absmiddle" />
                                                                An Error Occurred</td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2">
                                                                &nbsp;</td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <img src="/images/spacer.gif" border="0" width="15" height="1" /></td>
                                                            <td>
                                                                <asp:Label ID="lblError" runat="server" CssClass="default" /></td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2">
                                                                &nbsp;</td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <img src="/images/spacer.gif" border="0" width="15" height="1" /></td>
                                                            <td>
                                                                <asp:HyperLink ID="hypCommunity" runat="server" Text="Visit the ClearView Community" Target="_blank" /> to see if there are any system-wide problems that may contribute to your error.
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2">
                                                                &nbsp;</td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2">
                                                                <hr size="1" noshade />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2" align="right">
                                                                <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                                                    <tr>
                                                                        <td class="footer">
                                                                        </td>
                                                                        <td align="right">
                                                                            <asp:Button ID="btnClose" runat="server" CssClass="default" Width="75" Text="Close" /></td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <p>
                                                        &nbsp;</p>
                                                </td>
                                                <td background="/images/table_right.gif">
                                                    <img src="/images/table_right.gif" width="5" height="10"></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <img src="/images/table_bottomLeft.gif" border="0" width="5" height="9"></td>
                                                <td width="100%" background="/images/table_bottom.gif">
                                                </td>
                                                <td>
                                                    <img src="/images/table_bottomRight.gif" border="0" width="5" height="9"></td>
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
