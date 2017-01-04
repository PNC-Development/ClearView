<%@ Page Language="C#" MasterPageFile="~/frame.Master" AutoEventWireup="true" CodeBehind="network_deploy.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.network_deploy" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" runat="server">
<table width="100%" cellpadding="4" cellspacing="3" border="0">
    <tr>
        <td nowrap>Tracking Number:</td>
        <td width="100%"><asp:Label ID="lblTracking" runat="server" CssClass="default" /></td>
    </tr>
    <tr>
        <td nowrap>Serial Number:</td>
        <td width="100%"><asp:Label ID="lblSerial" runat="server" CssClass="default" /></td>
    </tr>
    <tr>
        <td nowrap>Asset Tag:</td>
        <td width="100%"><asp:Label ID="lblAsset" runat="server" CssClass="default" /></td>
    </tr>
    <tr>
        <td nowrap>Model:</td>
        <td width="100%"><asp:Label ID="lblModel" runat="server" CssClass="default" /></td>
    </tr>
    <tr>
        <td nowrap>Status:</td>
        <td width="100%">
            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="default" Width="200">
                <asp:ListItem Value="0" Text="Arrived" />
                <asp:ListItem Value="1" Text="In Stock" />
                <asp:ListItem Value="2" Text="Available" />
                <asp:ListItem Value="10" Text="In Use" />
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td nowrap>Depot Location:</td>
        <td width="100%"><asp:DropDownList ID="ddlDepot" runat="server" CssClass="default" /></td>
    </tr>
    <tr>
        <td nowrap>Depot Room:</td>
        <td width="100%"><asp:DropDownList ID="ddlDepotRoom" runat="server" CssClass="default" /></td>
    </tr>
    <tr>
        <td nowrap>Shelf Number:</td>
        <td width="100%"><asp:DropDownList ID="ddlShelf" runat="server" CssClass="default" /></td>
    </tr>
    <tr>
        <td nowrap>Available Ports:</td>
        <td width="100%"><asp:TextBox ID="txtPorts" runat="server" CssClass="default" Width="75" MaxLength="10" /></td>
    </tr>
    <tr>
        <td nowrap>Device Name:</td>
        <td width="100%"><asp:TextBox ID="txtName" runat="server" CssClass="default" Width="200" MaxLength="30" /></td>
    </tr>
    <tr>
        <td nowrap>Location:</td>
        <td colspan="2">
            <table cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td><asp:DropDownList ID="ddlLocation" runat="server" CssClass="default" Width="300" /></td>
                    <td><asp:CheckBox ID="chkLocation" runat="server" CssClass="default" Text="Show All Locations" AutoPostBack="true" OnCheckedChanged="chkLocation_Change" /></td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td nowrap>Class:</td>
        <td width="100%"><asp:DropDownList ID="ddlClass" CssClass="default" runat="server" Width="300" /></td>
    </tr>
    <tr>
        <td nowrap>Environment:</td>
        <td width="100%">
            <asp:DropDownList ID="ddlEnvironment" CssClass="default" runat="server" Width="300" Enabled="false" >
                <asp:ListItem Value="-- Please select a Class --" />
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td nowrap>Room:</td>
        <td width="100%"><asp:DropDownList ID="ddlRoom" runat="server" CssClass="default" Width="300" /></td>
    </tr>
    <tr>
        <td nowrap>Rack:</td>
        <td width="100%"><asp:DropDownList ID="ddlRack" runat="server" CssClass="default" Width="300" /></td>
    </tr>
    <tr>
        <td nowrap>Rack Position (U#):</td>
        <td width="100%"><asp:TextBox ID="txtRackPosition" runat="server" CssClass="default" Width="100" MaxLength="10" /> <span class="footer">[Format: 2-4]</span></td>
    </tr>
    <tr>
        <td nowrap></td>
        <td width="100%">
            <asp:Button ID="btnSubmit" runat="server" CssClass="default" Width="100" Text="Save" OnClick="btnSubmit_Click" /> 
            <asp:Button ID="btnClose" runat="server" CssClass="default" Width="100" Text="Close" /> 
        </td>
    </tr>
</table>
<input type="hidden" id="hdnEnvironment" runat="server" />
</asp:Content>
