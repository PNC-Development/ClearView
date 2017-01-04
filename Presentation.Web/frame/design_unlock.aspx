<%@ Page Language="C#" MasterPageFile="~/frame.Master" AutoEventWireup="true" CodeBehind="design_unlock.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.design_unlock" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" runat="server">
    <table cellpadding="4" cellspacing="0" border="0" align="center">
        <tr>
            <td>
                <table width="100%" cellpadding="0" cellspacing="5" border="0">
                    <tr>
                        <td rowspan="2"><img src="/images/lock.gif" border="0" align="absmiddle" /></td>
                        <td class="header" width="100%" valign="bottom">Unlock Design</td>
                    </tr>
                    <tr>
                        <td width="100%" valign="top">You will need to obtain an unlock code from a ClearView administrator to edit this design.</td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <table cellpadding="4" cellspacing="0" border="0">
                    <tr>
                        <td nowrap>Workstation IP:</td>
                        <td><asp:Label ID="lblConfidenceUnlock" runat="server" CssClass="default" />&nbsp;&nbsp;&nbsp;&nbsp;<span class="footer">(This will be required by the ClearView administrator)</span></td>
                    </tr>
                    <tr>
                        <td nowrap>Unlock Code:</td>
                        <td><asp:TextBox ID="txtConfidenceUnlock" runat="server" CssClass="default" Width="400" /></td>
                    </tr>
                    <tr>
                        <td nowrap valign="top">Reason:</td>
                        <td><asp:TextBox ID="txtConfidenceReason" runat="server" CssClass="default" Width="400" TextMode="MultiLine" Rows="5" /></td>
                    </tr>
                    <tr>
                        <td nowrap>&nbsp;</td>
                        <td>
                            <asp:Button ID="btnConfidenceUnlock" runat="server" CssClass="default" Width="100" Text="Unlock" OnClick="btnConfidenceUnlock_Click" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="2" class="reddefault"><b>NOTE:</b> Unlocking a design will automatically change the confidence level from 100% to 80%.</td>
        </tr>
    </table>
</asp:Content>
