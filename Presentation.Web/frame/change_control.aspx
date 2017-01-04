<%@ Page Language="C#" MasterPageFile="~/frame.Master" AutoEventWireup="True" CodeBehind="change_control.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.change_control" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" Runat="Server">
<table width="100%" height="100%" cellpadding="4" cellspacing="2" border="0">
    <tr height="1">
        <td colspan="2" align="center" class="bigCheck">
            <asp:Label ID="lblUpdate" runat="server" CssClass="header" Visible="false" Text="<img src='/images/bigCheck.gif' border='0' align='absmiddle'> Change Control Updated" />
        </td>
    </tr>
    <tr height="1">
        <td nowrap><b>Change Control Number:</b></td>
        <td width="100%">
            <asp:Label ID="lblChange" runat="server" CssClass="default" Visible="false" />
            <asp:TextBox ID="txtChange" runat="server" CssClass="default" Width="100" MaxLength="15" Visible="false" />
        </td>
    </tr>
    <asp:Panel ID="panProject" runat="server" Visible="false">
    <tr height="1">
        <td nowrap><b>Project Name:</b></td>
        <td width="100%"><asp:Label ID="lblName" runat="server" CssClass="default" /></td>
    </tr>
    <tr height="1">
        <td nowrap><b>Project Number:</b></td>
        <td width="100%"><asp:Label ID="lblNumber" runat="server" CssClass="default" /></td>
    </tr>
    </asp:Panel>
    <asp:Panel ID="panTask" runat="server" Visible="false">
    <tr height="1">
        <td nowrap><b>Task Name:</b></td>
        <td width="100%"><asp:Label ID="lblTName" runat="server" CssClass="default" /></td>
    </tr>
    <tr height="1">
        <td nowrap><b>Task Number:</b></td>
        <td width="100%"><asp:Label ID="lblTNumber" runat="server" CssClass="default" /></td>
    </tr>
    </asp:Panel>
    <tr height="1">
        <td nowrap><b>Technician:</b></td>
        <td width="100%"><asp:Label ID="lblImplementor" runat="server" CssClass="default" /></td>
    </tr>
    <tr height="1">
        <td nowrap><b>Implementation Date:</b></td>
        <td width="100%">
            <asp:Label ID="lblDate" runat="server" CssClass="default" Visible="false" />
            <asp:TextBox ID="txtDate" runat="server" CssClass="default" Width="100" MaxLength="10" Visible="false" />
        </td>
    </tr>
    <tr height="1">
        <td nowrap><b>Implementation Time:</b></td>
        <td width="100%">
            <asp:Label ID="lblTime" runat="server" CssClass="default" Visible="false" />
            <asp:TextBox ID="txtTime" runat="server" CssClass="default" Width="100" MaxLength="10" Visible="false" />
        </td>
    </tr>
    <tr height="1">
        <td colspan="2"><b>Comments:</b></td>
    </tr>
    <tr>
        <td colspan="2" valign="top">
        <asp:Panel ID="panComments" runat="server" Visible="false">
        <div style="width:100%; height:100%; overflow:auto">
            <asp:Label ID="lblComments" runat="server" CssClass="default" />
        </div>
        </asp:Panel>
        <asp:TextBox ID="txtComments" runat="server" CssClass="default" Width="100%" Rows="12" TextMode="multiLine" Visible="false" />
        </td>
    </tr>
    <tr height="1">
        <td colspan="2">
            <table width="100%" cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td><asp:LinkButton ID="btnToday" runat="server" OnClick="btnToday_Click" Text="<img src='/images/clock.gif' border='0' align='absmiddle'/><img src='/images/spacer.gif' border='0' width='10' height='20' align='absmiddle'/>All Changes on " /></td>
                    <td align="right">
                        <asp:LinkButton ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" Text="<img src='/images/bigCheckBox.gif' border='0' align='absmiddle'/><img src='/images/spacer.gif' border='0' width='10' height='20' align='absmiddle'/>Update" Visible="false" /> 
                        <asp:LinkButton ID="btnDelete" runat="server" OnClick="btnDelete_Click" Text="<img src='/images/bigError.gif' border='0' align='absmiddle'/><img src='/images/spacer.gif' border='0' width='10' height='20' align='absmiddle'/>Delete" Visible="false" /> 
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
</asp:Content>
