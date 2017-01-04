<%@ Page Language="C#" MasterPageFile="~/frame.Master" AutoEventWireup="true" CodeBehind="scheduling.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.scheduling" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" runat="server">
    <table width="100%" cellpadding="2" cellspacing="2" border="0">
        <tr>
            <td colspan="2" align="center"><asp:Label ID="lblResult" runat="server" CssClass="header" /></td>
        </tr>
        <tr>
            <td nowrap><b>Date:</b></td>
            <td width="100%"><asp:Label ID="lblDate" runat="server" CssClass="default" /></td>
        </tr>
        <tr>
            <td nowrap><b>Facilitator:</b></td>
            <td width="100%"><asp:Label ID="lblFacilitator" runat="server" CssClass="default" /></td>
        </tr>
        <tr>
            <td nowrap><b>Event Name:</b></td>
            <td width="100%"><asp:Label ID="lblEventName" runat="server" CssClass="default" /></td>
        </tr>
        <tr>
            <td nowrap><b>Location:</b></td>
            <td width="100%"><asp:Label ID="lblLocation" runat="server" CssClass="default" /></td>
        </tr>
        <tr>
            <td nowrap><b>NetMeeting:</b></td>
            <td width="100%"><asp:Label ID="lblNetMeeting" runat="server" CssClass="default" /></td>
        </tr>
        <tr>
            <td nowrap><b>Conference Line:</b></td>
            <td width="100%"><asp:Label ID="lblConfLine" runat="server" CssClass="default" /></td>
        </tr>
        <tr>
            <td nowrap><b>PassCode:</b></td>
            <td width="100%"><asp:Label ID="lblPassCode" runat="server" CssClass="default" /></td>
        </tr>
        <tr>
            <td nowrap><b>Start Time:</b></td>
            <td width="100%"><asp:Label ID="lblStartTime" runat="server" CssClass="default" /></td>
        </tr>
        <tr>
            <td nowrap><b>End Time:</b></td>
            <td width="100%"><asp:Label ID="lblEndTime" runat="server" CssClass="default" /></td>
        </tr>
        <tr>
            <td nowrap>&nbsp;</td>
            <td width="100%"><asp:Button ID="btnView" runat="server" Text="View Registration(s)" OnClick="btnView_Click" CssClass="default" Width="150" /></td>
        </tr>
    </table>
    <br />
<asp:Panel ID="panEdit" runat="server" Visible="false">
    <table width="100%" cellpadding="2" cellspacing="2" border="0">
        <asp:Panel ID="panRegister" runat="server" Visible="false">
        <tr>
            <td class="bold" colspan="2">Register for this Event</td>
        </tr>
        <tr>
            <td nowrap>Name:</td>
            <td width="100%"><asp:Label ID="lblName" runat="server" CssClass="default" /></td>
        </tr>
        <tr>
            <td nowrap>Phone:</td>
            <td width="100%"><asp:TextBox ID="txtPhone" runat="server" CssClass="default" Width="150" /></td>
        </tr>
        <tr>
            <td nowrap>Department:</td>
            <td width="100%"><asp:DropDownList ID="drpDept" runat="server" CssClass="default" /></td>
        </tr>
        <tr>
            <td nowrap>&nbsp;</td>
            <td width="100%"><asp:Button ID="btnRegister" runat="server" Text="Register" OnClick="btnRegister_Click" CssClass="default" Width="100" /></td>
        </tr>
        </asp:Panel>
        <asp:Panel ID="panUnregister" runat="server" Visible="false">
        <tr>
            <td class="bold" colspan="2"><img src="/images/alert.gif" border="0" align="absmiddle" /> You are registered for this event.</td>
        </tr>
        <tr>
            <td nowrap>&nbsp;</td>
            <td width="100%"><asp:Button ID="btnUnRegister" runat="server" Text="Unregister" OnClick="btnUnRegister_Click" CssClass="default" Width="100" /></td>
        </tr>
        </asp:Panel>
    </table>
</asp:Panel>
<asp:Panel ID="panView" runat="server" Visible="false">
    <table width="100%" cellpadding="2" cellspacing="2" border="0">
        <tr>
            <td class="bold">Current Registrations</td>
        </tr>
    </table>
    <table width="100%" cellpadding="4" cellspacing="0" border="0" style="border:solid 1px #CCCCCC">
        <tr bgcolor="#EEEEEE">
            <td nowrap><b>Name</b></td>
            <td nowrap><b>Phone Number</b></td>
            <td nowrap><b>Department</b></td>
        </tr>
        <asp:repeater ID="rptUsers" runat="server">
            <ItemTemplate>
                <tr>
                    <td><%#DataBinder.Eval(Container.DataItem, "fname") + " " + DataBinder.Eval(Container.DataItem, "lname") %></td>
                    <td><%# DataBinder.Eval(Container.DataItem, "phone") %></td>
                    <td><%# DataBinder.Eval(Container.DataItem, "dept") %></td>
                </tr>
            </ItemTemplate>
        </asp:repeater>
        <tr>
            <td colspan="3">
                <asp:Label ID="lblUsersNone" runat="server" CssClass="default" Visible="false" Text="<img src='/images/alert.gif' border='0' align='absmiddle'> There are no registrations..." />
            </td>
        </tr>
    </table>
    <br />
    <table width="100%" cellpadding="2" cellspacing="2" border="0">
        <tr>
            <td><asp:Button ID="btnBack" runat="server" Text="Back" OnClick="btnBack_Click" CssClass="default" Width="100" /></td>
        </tr>
    </table>
</asp:Panel>
</asp:Content>
