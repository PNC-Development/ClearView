<%@ Page Language="C#" MasterPageFile="~/window.Master" AutoEventWireup="true" CodeBehind="config_csm.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.config_csm" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" runat="server">
<table width="100%" height="100%" cellpadding="4" cellspacing="2" border="0">
    <tr height="1">
        <td colspan="2">
            <table width="100%" cellpadding="0" cellspacing="5" border="0">
                <tr>
                    <td rowspan="2"><img src="/images/cluster.gif" border="0" align="middle" /></td>
                    <td class="header" width="100%" valign="bottom">CSM Config Configuration</td>
                </tr>
                <tr>
                    <td width="100%" valign="top">Using this page, you can modify your CSM configs associated with your server.</td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td colspan="2">
        <asp:Panel ID="panView" runat="server" Visible="false">
            <table width="100%" height="100%" cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td valign="top">
                        <div style="height:100%; overflow:auto">
                        <table width="100%" cellpadding="4" cellspacing="0" border="0">
                            <tr>
                                <td nowrap>Nickname:</td>
                                <td colspan="2"><asp:TextBox ID="txtName" runat="server" CssClass="default" Width="200" MaxLength="50" /></td>
                            </tr>
                            <tr>
                                <td nowrap>Enter the number of servers:</td>
                                <td colspan="2"><asp:TextBox ID="txtServers" runat="server" CssClass="default" Width="100" MaxLength="3" /></td>
                            </tr>
                            <tr>
                                <td nowrap>Number of DR servers:</td>
                                <td colspan="2"><asp:TextBox ID="txtDR" runat="server" CssClass="default" Width="100" MaxLength="3" /></td>
                            </tr>
                        </table>
                        </div>
                    </td>
                </tr>
                <tr height="1">
                    <td>
                        <table width="100%" cellpadding="4" cellspacing="2" border="0">
                            <tr>
                                <td colspan="3"><hr size="1" noshade /></td>
                            </tr>
                            <tr>
                                <td class="required">* = Required Field</td>
                                <td align="center">
                                </td>
                                <td align="right">
                                    <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="default" Width="75" OnClick="btnSave_Click" /> 
                                    <asp:Button ID="btnClose" runat="server" Text="Close" CssClass="default" Width="75" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            </asp:Panel>
            <asp:Panel ID="panDenied" runat="server" Visible="false">
                <table width="100%" cellpadding="2" cellspacing="2" border="0">
                    <tr>
                        <td><img src="/images/spacer.gif" border="0" width="15" height="1" /></td>
                        <td class="header"><img src="/images/bigX.gif" border="0" align="absmiddle" /> Access Denied</td>
                    </tr>
                    <tr><td colspan="2">&nbsp;</td></tr>
                    <tr>
                        <td><img src="/images/spacer.gif" border="0" width="15" height="1" /></td>
                        <td>You do not have rights to view this item.</td>
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
                                    <td align="right"><asp:Button ID="btnDenied" runat="server" CssClass="default" Width="75" Text="Close" /></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </td>
    </tr>
</table>
</asp:Content>
