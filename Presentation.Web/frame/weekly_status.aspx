<%@ Page Language="C#" MasterPageFile="~/frame.Master" AutoEventWireup="true" CodeBehind="weekly_status.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.weekly_status" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" runat="server">
    <asp:Panel ID="panResource" runat="server" Visible="false">
    <table width="100%" height="100%" cellpadding="2" cellspacing="0" border="0">
        <tr height="1">
            <td><asp:Label ID="lblStatus" runat="server" CssClass="default" /></td>
        </tr>
        <tr height="1">
            <td>&nbsp;</td>
        </tr>
        <tr height="1">
            <td><b>Comments / Issues:</b></td>
        </tr>
        <tr>
            <td valign="top"><asp:Label ID="lblComments2" runat="server" CssClass="default" /></td>
        </tr>
    </table>
    </asp:Panel>
    <asp:Panel ID="panTPMPC" runat="server" Visible="false">
    <table width="100%" height="100%" cellpadding="2" cellspacing="0" border="0">
        <asp:Panel ID="panTPM" runat="server" Visible="false">
        <tr height="1">
            <td nowrap align="center"><b>Week Of:</b></td>
            <td nowrap align="center"><b>Scope</b></td>
            <td nowrap align="center"><b>Timeline</b></td>
            <td nowrap align="center"><b>Budget</b></td>
        </tr>
        <tr height="1">
            <td align="center"><asp:Label ID="lblScopeD" runat="server" CssClass="default" /></td>
            <td align="center"><asp:Label ID="lblScope" runat="server" CssClass="default" /></td>
            <td align="center"><asp:Label ID="lblTimeline" runat="server" CssClass="default" /></td>
            <td align="center"><asp:Label ID="lblBudget" runat="server" CssClass="default" /></td>
        </tr>
        </asp:Panel>
        <asp:Panel ID="panPC" runat="server" Visible="false">
        <tr height="1">
            <td nowrap align="center"><b>Week Of:</b></td>
            <td nowrap align="center"><b>Variance</b></td>
        </tr>
        <tr height="1">
            <td align="center"><asp:Label ID="lblVarianceD" runat="server" CssClass="default" /></td>
            <td align="center"><asp:Label ID="lblVariance" runat="server" CssClass="default" /></td>
        </tr>
        </asp:Panel>
        <tr height="1">
            <td colspan="4"><img src="/images/spacer.gif" border="0" width="1" height="5" /></td>
        </tr>
        <tr height="1">
            <td colspan="4"><b>This Week's Accomplishments:</b></td>
        </tr>
        <tr height="1">
            <td colspan="4"><asp:TextBox ID="txtThis" runat="server" CssClass="default" Width="100%" Rows="5" TextMode="multiLine" /></td>
        </tr>
        <tr height="1">
            <td colspan="4"><b>Next Week's Accomplishments:</b></td>
        </tr>
        <tr height="1">
            <td colspan="4"><asp:TextBox ID="txtNext" runat="server" CssClass="default" Width="100%" Rows="5" TextMode="multiLine" /></td>
        </tr>
        <tr height="1">
            <td colspan="4"><b>Comments / Issues:</b></td>
        </tr>
        <tr height="1">
            <td colspan="4"><asp:TextBox ID="txtComments" runat="server" CssClass="default" Width="100%" Rows="5" TextMode="multiLine" /></td>
        </tr>
        <tr>
            <td colspan="4">
                <table width="100%" cellpadding="0" cellspacing="0" border="0">
                    <tr>
                        <td><asp:LinkButton ID="btnDelete" runat="server" OnClick="btnDelete_Click" Text="<img src='/images/bigError.gif' border='0' align='absmiddle'/><img src='/images/spacer.gif' border='0' width='10' height='20' align='absmiddle'/>Delete" /></td>
                        <td align="right"><asp:LinkButton ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" Text="<img src='/images/bigCheckBox.gif' border='0' align='absmiddle'/><img src='/images/spacer.gif' border='0' width='10' height='20' align='absmiddle'/>Update" /></td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </asp:Panel>
<asp:Label ID="lblType" runat="server" Visible="false" />
<asp:Label ID="lblProject" runat="server" Visible="false" />
</asp:Content>
