<%@ Page Language="C#" MasterPageFile="~/clearview.Master" AutoEventWireup="true" CodeBehind="service_results.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.service_results" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" Runat="Server">
<table width="100%" height="100%" border="0" cellspacing="0" cellpadding="0">
    <tr height="1"> 
        <td bgcolor="#007253"><img src="/images/header_wide.aspx?t=Service Editor" border="0" /></td>
        <td bgcolor="#007253" align="right"><asp:PlaceHolder ID="PH4" runat="server" /></td>
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
                                    <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                        <tr>
                                            <td><img src="/images/table_topleft.gif" border="0" width="5" height="26"></td>
                                            <td nowrap background="/images/table_top.gif" class="greentableheader" width="100%"><asp:Label ID="lblTitle" runat="server" CssClass="greentableheader" /></td>
                                            <td><img src="/images/table_topright.gif" border="0" width="5" height="26"></td>
                                        </tr>
                                        <tr>
                                            <td background="/images/table_left.gif"><img src="/images/table_left.gif" width="5" height="10"></td>
                                            <td width="100%" bgcolor="#FFFFFF">
                                                <br />
                                                <%=strResults %>
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