<%@ Page Language="C#" MasterPageFile="~/window.Master" AutoEventWireup="true" CodeBehind="saved.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.saved" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" runat="server">
<table width="350px" height="200px" cellpadding="0" cellspacing="0" border="0">
    <tr> 
        <td valign="top" width="100%" height="100%" bgcolor="#FFFFFF">
            <table width="100%" height="100%" cellpadding="0" cellspacing="0" border="0">
                <tr height="1">
                    <td><img src="/images/table_topleft.gif" border="0" width="5" height="26"></td>
                    <td nowrap background="/images/table_top.gif" class="greentableheader" width="100%">ClearView Message</td>
                    <td nowrap background="/images/table_top.gif" class="greentableheader"><a href="javascript:void(0);" onclick="parent.HidePanel();"><img src="/images/close.gif" border="0" /></a></td>
                    <td><img src="/images/table_topright.gif" border="0" width="5" height="26"></td>
                </tr>
                <tr>
                    <td background="/images/table_left.gif"><img src="/images/table_left.gif" width="5" height="10"></td>
                    <td  colspan="2"width="100%" bgcolor="#FFFFFF" valign="top">
                        <table width="100%" cellpadding="3" cellspacing="5" border="0">
                            <tr>
                                <td rowspan="3" valign="top"><img src="/images/ico_check.gif" border="0" align="absmiddle" /></td>
                                <td class="mainheader" width="100%" valign="bottom"><asp:Literal ID="litHeader" runat="server" /></td>
                            </tr>
                            <tr>
                                <td width="100%" valign="top"><asp:Literal ID="litMessage" runat="server" /></td>
                            </tr>
                            <tr>
                                <td><img src="/images/spacer.gif" border="0" width="1" height="1" /></td>
                            </tr>
                            <tr>
                                <td colspan="2" class="footer" align="center">This window will automatically close in <asp:Label ID="lblTimer" runat="server" Text="5" /> seconds...</td>
                            </tr>
                        </table>
                    </td>
                    <td background="/images/table_right.gif"><img src="/images/table_right.gif" width="5" height="10"></td>
                </tr>
                <tr height="1">
                    <td><img src="/images/table_bottomLeft.gif" border="0" width="5" height="9"></td>
                    <td colspan="2" width="100%" background="/images/table_bottom.gif"></td>
                    <td><img src="/images/table_bottomRight.gif" border="0" width="5" height="9"></td>
                </tr>
            </table>
        </td>
    </tr>
</table>
</asp:Content>