<%@ Page Language="C#" MasterPageFile="~/frame.Master" AutoEventWireup="true" CodeBehind="server_deploy.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.server_deploy" Title="Untitled Page" %>
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
        <td width="100%"><asp:DropDownList ID="ddlModels" runat="server" CssClass="default" /></td>
    </tr>
    <tr>
        <td nowrap>Status:</td>
        <td width="100%">
            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="default" Width="200">
                <asp:ListItem Value="0" Text="Arrived" />
                <asp:ListItem Value="1" Text="In Stock (Default)" />
                <asp:ListItem Value="2" Text="Available" />
                <asp:ListItem Value="100" Text="Reserved" />
            </asp:DropDownList>
        </td>
    </tr>
    <asp:Panel ID="panServer" runat="server" Visible="false">
    <tr id="trLocation" runat="server"  >
        <td nowrap><asp:Label ID="lblLocation" runat="server" CssClass="default" Text="Location :<font class='required'>&nbsp;*</font>" /></td>
        <td width="100%" >
            <asp:Label ID="lblLocationValue" runat="server" CssClass="default" BackColor="#f9f9f9" Width="500"/>
            <asp:HiddenField ID="hdnLocationId" runat="server" Value="0" />
            <br /><br />
            <asp:Button ID="btnChangeLocation" runat="server" Text="Change Location Information" CssClass="default"  Width="200" />
        </td>
    </tr>
    <tr id="trRoom" runat="server" >
        <td nowrap><asp:Label ID="lblRoom" runat="server" CssClass="default" Text="Room:<font class='required'>&nbsp;*</font>" /></td>
        <td width="100%"><asp:Label ID="lblRoomValue"  runat="server" CssClass="default" BackColor="#f9f9f9" Width="500"/></td>                            
    </tr>
    <tr id="trRack" runat="server" >
        <td nowrap><asp:Label ID="lblRack" runat="server" CssClass="default" Text="Rack:<font class='required'>&nbsp;*</font>"  /></td>
        <td width="100%"><asp:Label ID="lblRackValue" runat="server" CssClass="default" BackColor="#f9f9f9" Width="150"/>
        <asp:HiddenField ID="hdnRackId" runat="server" /></td>                            
    </tr>
    <tr id="trRackPosition" runat="server" >
        <td nowrap><asp:Label ID="lblRackPosition" runat="server" CssClass="default" Text="Rack Position (U#):" /></td>
        <td width="100%">
            <asp:Label ID="lblRackPositionValue" runat="server" CssClass="default" BackColor="#f9f9f9" Width="50"/>
            <asp:HiddenField ID="hdnRackPosition" runat="server" /> 
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
    </asp:Panel>
    <asp:Panel ID="panBlade" runat="server" Visible="false">
    <tr>
        <td nowrap>Enclosure:</td>
        <td width="100%"><asp:DropDownList ID="ddlEnclosure" runat="server" CssClass="default" Width="300" /></td>
    </tr>
    <tr>
        <td nowrap>Slot:</td>
        <td width="100%"><asp:TextBox ID="txtSlot" runat="server" CssClass="default" Width="30" MaxLength="2" /> <span class="footer">(1 - 16)</span></td>
    </tr>
    <tr>
        <td nowrap>Spare:</td>
        <td width="100%">
            <asp:RadioButton ID="radSpareYes" runat="server" Text="Yes" CssClass="default" GroupName="Spare" />&nbsp;
            <asp:RadioButton ID="radSpareNo" runat="server" Text="No" CssClass="default" GroupName="Spare" />
        </td>
    </tr>
    </asp:Panel>
    <tr>
        <td nowrap>ILO Address:</td>
        <td width="100%"><asp:TextBox ID="txtILO" runat="server" CssClass="default" Width="200" MaxLength="15" /></td>
    </tr>
    <tr>
        <td nowrap>Dummy Name:</td>
        <td width="100%"><asp:TextBox ID="txtDummy" runat="server" CssClass="default" Width="200" MaxLength="50" /></td>
    </tr>
    <tr>
        <td nowrap>MAC Address:</td>
        <td width="100%"><asp:TextBox ID="txtMAC" runat="server" CssClass="default" Width="200" MaxLength="50" /></td>
    </tr>
    <tr>
        <td nowrap>Original VLAN:</td>
        <td width="100%"><asp:TextBox ID="txtVLAN" runat="server" CssClass="default" Width="40" MaxLength="3" /></td>
    </tr>
    <tr>
        <td nowrap>Build Network:</td>
        <td width="100%"><asp:DropDownList ID="ddlBuildNetwork" runat="server" CssClass="default" Width="300" /></td>
    </tr>
    <tr>
        <td nowrap>Resiliency:</td>
        <td width="100%"><asp:DropDownList ID="ddlResiliency" runat="server" CssClass="default" Width="400" /></td>
    </tr>
    <tr>
        <td nowrap>Operating System Group:</td>
        <td width="100%"><asp:DropDownList ID="ddlOperatingSystemGroup" runat="server" CssClass="default" Width="400" /></td>
    </tr>
    <tr>
        <td nowrap></td>
        <td width="100%"><asp:Button ID="btnHBAs" runat="server" CssClass="default" Width="150" Text="Configure HBAs" /> <asp:Label ID="lblHBA" runat="server" CssClass="reddefault" /></td>
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
