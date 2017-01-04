<%@ Page Language="C#" MasterPageFile="~/frame.Master" AutoEventWireup="true" CodeBehind="milestone.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.milestone" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" runat="server">
    <table width="100" height="100%" cellpadding="2" cellspacing="2" border="0">
        <tr height="1">
            <td nowrap><b>Approved Date:</b></td>
            <td width="100%"><asp:TextBox ID="txtApproved" runat="server" CssClass="default" Width="100" MaxLength="10" /> <asp:ImageButton ID="imgApproved" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calendar.gif" /></td>
        </tr>
        <tr height="1">
            <td nowrap><b>Forecasted Date:</b></td>
            <td width="100%"><asp:TextBox ID="txtForecasted" runat="server" CssClass="default" Width="100" MaxLength="10" /> <asp:ImageButton ID="imgForecasted" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calendar.gif" /></td>
        </tr>
        <tr height="1">
            <td nowrap><b>Completed:</b></td>
            <td width="100%"><asp:CheckBox ID="chkComplete" runat="server" CssClass="default" /></td>
        </tr>
        <tr height="1">
            <td nowrap><b>Milestone:</b></td>
            <td width="100%"><asp:TextBox ID="txtMilestone" runat="server" CssClass="default" Width="300" /></td>
        </tr>
        <tr height="1">
            <td colspan="2"><img src="/images/spacer.gif" border="0" width="1" height="5" /></td>
        </tr>
        <tr height="1">
            <td colspan="2"><b>Description:</b></td>
        </tr>
        <tr height="1">
            <td colspan="2"><asp:TextBox ID="txtDescription" runat="server" CssClass="default" Width="400" Rows="10" TextMode="multiLine" /></td>
        </tr>
        <tr>
            <td colspan="2">
                <table width="100%" cellpadding="0" cellspacing="0" border="0">
                    <tr>
                        <td><asp:LinkButton ID="btnDelete" runat="server" OnClick="btnDelete_Click" Text="<img src='/images/bigError.gif' border='0' align='absmiddle'/><img src='/images/spacer.gif' border='0' width='10' height='20' align='absmiddle'/>Delete" /></td>
                        <td align="right"><asp:LinkButton ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" Text="<img src='/images/bigCheckBox.gif' border='0' align='absmiddle'/><img src='/images/spacer.gif' border='0' width='10' height='20' align='absmiddle'/>Update" /></td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
