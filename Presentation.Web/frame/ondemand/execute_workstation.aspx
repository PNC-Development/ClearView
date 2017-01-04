<%@ Page Language="C#" MasterPageFile="~/window.Master" AutoEventWireup="true" CodeBehind="execute_workstation.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.execute_workstation" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" runat="server">
    <table width="100%" height="100%" cellpadding="4" cellspacing="2" border="0">
        <tr height="1">
            <td>
                <table width="100%" cellpadding="1" cellspacing="1" border="0">
                    <tr>
                        <td>
                            <table width="100%" cellpadding="0" cellspacing="5" border="0">
                                <tr>
                                    <td rowspan="2"><asp:Image ID="imgStep" runat="Server" ImageAlign="AbsMiddle" /></td>
                                    <td class="header" width="100%" valign="bottom"><asp:Label ID="lblTitle" runat="server" CssClass="header" /></td>
                                </tr>
                                <tr>
                                    <td width="100%" valign="top"><asp:Label ID="lblSubTitle" runat="server" CssClass="default" /></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table width="100%" cellpadding="0" cellspacing="5" border="0" style="border:solid 1px #70BA77; background-color:#EEF7EF">
                                <tr>
                                    <td nowrap class="bigger">Device Count:</td>
                                    <td width="100%" class="default"><b><asp:Label ID="lblCurrentCount" runat="server" CssClass="bigger" /> / <asp:Label ID="lblForecastCount" runat="server" CssClass="bigger" /></b></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <table width="100%" cellpadding="0" cellspacing="5" border="0">
                    <tr>
                        <td class="bigger"><b>Progress:</b></td>
                        <td width="90%"><%=strProgress %></td>
                    </tr>
                </table>
            </td>
        </tr>
        <asp:Panel ID="panPending" runat="server" Visible="false">
        <tr>
            <td>
                <table width="100%" cellpadding="0" cellspacing="5" border="0">
                    <tr>
                        <td rowspan="2"><img src="/images/ico_error.gif" border="0" align="absmiddle" /></td>
                        <td class="header" width="100%" valign="bottom">Pending Approval</td>
                    </tr>
                    <tr>
                        <td width="100%" valign="top">You cannot execute this request because it is still under review.</td>
                    </tr>
                </table>
            </td>
        </tr>
        </asp:Panel>
        <asp:Panel ID="panStep" runat="server" Visible="false">
        <tr>
            <td><asp:PlaceHolder ID="PHStep" runat="server" /></td>
        </tr>
        </asp:Panel>
    </table>
</asp:Content>
