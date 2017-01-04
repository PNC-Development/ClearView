<%@ Page Language="C#" MasterPageFile="~/clearview.Master" AutoEventWireup="true" CodeBehind="NotFound.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.NotFound" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" Runat="Server">
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
        <td colspan="2" bgcolor="E9E9E9">
            <table width="98%" height="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                <tr height="1">
                    <td align="left" valign="top">&nbsp;</td>
                    <td align="left" valign="top"><img src="/images/spacer.gif" width="15" height="15" /></td>
                    <td align="left" valign="top">&nbsp;</td>
                </tr>
                <tr> 
                    <td width="180" align="left" valign="top">
                        <asp:PlaceHolder ID="PH2" runat="server" />
                    </td>
                    <td width="12" align="left" valign="top">&nbsp;</td>
                    <td width="100%" align="left" valign="top">
<table width="100%" cellpadding="0" cellspacing="0" border="0">
    <tr>
        <td><img src="/images/table_topleft.gif" border="0" width="5" height="26"></td>
        <td nowrap background="/images/table_top.gif" class="greentableheader" width="100%">Insufficient Permission</td>
        <td><img src="/images/table_topright.gif" border="0" width="5" height="26"></td>
    </tr>
    <tr>
        <td background="/images/table_left.gif"><img src="/images/table_left.gif" width="5" height="10"></td>
        <td width="100%" bgcolor="#FFFFFF">
            <br />
                <table width="100%" cellpadding="0" cellspacing="5" border="0">
                    <tr>
                        <td rowspan="2"><img src="/images/ico_error.gif" border="0" align="absmiddle" /></td>
                        <td class="header" width="100%" valign="bottom">Insufficient Permission</td>
                    </tr>
                    <tr>
                        <td width="100%" valign="top">You do not have appropriate permission to view this page. You are seeing this message for one of two reasons...</td>
                    </tr>
                    <tr>
                        <td></td>
                        <td width="100%">1.) This page exists but you do not have permission to view it.</td>
                    </tr>
                    <tr>
                        <td></td>
                        <td width="100%">2.) This page does not exist.</td>
                    </tr>
                    <tr>
                        <td colspan="2">&nbsp;</td>
                    </tr>
                    <tr>
                        <td></td>
                        <td width="100%">If you think this page does exist and you should have rights to view it, please contact your ClearView administrator by clicking the <span class="help_box"><img src="/images/help_box.gif" border="0" align="absmiddle" /> SUPPORT</span> button link from the top navigation.</td>
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
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
</asp:Content>