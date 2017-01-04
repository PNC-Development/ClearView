<%@ Page Language="C#" MasterPageFile="~/frame.Master" AutoEventWireup="true" CodeBehind="resource_request_sla.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.resource_request_sla" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" runat="server">
    <table width="100%" cellpadding="0" cellspacing="5" border="0">
        <tr>
            <td rowspan="2"><img src="/images/clock.gif" border="0" align="middle" /></td>
            <td class="header" width="100%" valign="bottom">Service Level Agreement</td>
        </tr>
        <tr>
            <td width="100%" valign="top">How long you have to do the work...</td>
        </tr>
    </table>
    <asp:Panel ID="panShow" runat="server" Visible="false">
    <table width="100%" cellpadding="3" cellspacing="2" border="0">
        <tr>
            <td colspan="2">Your Service Level Agreement (SLA) is <b><asp:Label ID="lblDays" runat="server" CssClass="default" /></b> business days.</p></td>
        </tr>
        <tr>
            <td nowrap>Assigned:</td>
            <td width="100%"><asp:Label ID="lblAssigned" runat="server" CssClass="default" /></td>
        </tr>
        <tr>
            <td nowrap>Deadline:</td>
            <td width="100%"><asp:Label ID="lblDeadline" runat="server" CssClass="default" /></td>
        </tr>
        <tr>
            <td colspan="2">&nbsp;</td>
        </tr>
        <tr>
            <td colspan="2" align="center"><asp:Button ID="btnClose" runat="server" CssClass="default" Width="125" Text="Close Window" /></td>
        </tr>
    </table>
    </asp:Panel>
    <asp:Panel ID="panHide" runat="server" Visible="false">
    <table width="100%" cellpadding="3" cellspacing="2" border="0">
        <tr>
            <td class="default">
                <p><img src="/images/bigAlert.gif" border="0" align="absmiddle" /> <b>SLA Not Configured</b></p>
                <p>The following service managers have not configured an SLA for this service:</p>
                <p><asp:Label ID="lblManager" runat="server" CssClass="default" /></p>
            </td>
        </tr>
        <tr>
            <td>&nbsp;</td>
        </tr>
    </table>
    </asp:Panel>
</asp:Content>
