<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="asset_decommission_wan.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.asset_decommission_wan" %>
<script type="text/javascript">
</script>
<tr>
    <td nowrap>Device Name:</td>
    <td width="100%"><asp:Label ID="lblName" runat="server" CssClass="default" /></td>
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
    <td nowrap>Device Type:</td>
    <td width="100%"><asp:Label ID="lblType" runat="server" CssClass="default" /></td>
</tr>
<tr>
    <td nowrap>Model:</td>
    <td width="100%"><asp:Label ID="lblModel" runat="server" CssClass="default" /></td>
</tr>
<tr>
    <td nowrap>Location:</td>
    <td width="100%"><asp:Label ID="lblLocation" runat="server" CssClass="default" /></td>
</tr>
<tr>
    <td nowrap>Rack:</td>
    <td width="100%"><asp:Label ID="lblRack" runat="server" CssClass="default" /></td>
</tr>
<tr>
    <td nowrap>Environment:</td>
    <td width="100%"><asp:Label ID="lblEnvironment" runat="server" CssClass="default" /></td>
</tr>
<tr>
    <td nowrap>Class:</td>
    <td width="100%"><asp:Label ID="lblClass" runat="server" CssClass="default" /></td>
</tr>
<tr>
    <td nowrap>Commissioned On:</td>
    <td width="100%"><asp:Label ID="lblCommissionedOn" runat="server" CssClass="default" /></td>
</tr>
<tr>
    <td nowrap>Commissioned By:</td>
    <td width="100%"><asp:Label ID="lblCommissionedBy" runat="server" CssClass="default" /></td>
</tr>
<tr>
    <td nowrap>Asset Status:<font class="required">&nbsp;*</font></td>
    <td width="100%">
        <asp:RadioButton ID="radStatusDecom" runat="server" CssClass="default" GroupName="status" Text="DECOMMISSIONED - Asset is immediately available for redeployment" /><br />
        <asp:RadioButton ID="radStatusDispose" runat="server" CssClass="default" GroupName="status" Text="DISPOSED - Asset will never be used again" /><br />
    </td>
</tr>
<tr id="divDecom1" runat="server" style="display:none">
    <td nowrap>Depot Location:<font class="required">&nbsp;*</font></td>
    <td width="100%"><asp:DropDownList ID="ddlDecomLocation" runat="server" CssClass="default" /></td>
</tr>
<tr id="divDecom2" runat="server" style="display:none">
    <td nowrap>Depot Room:<font class="required">&nbsp;*</font></td>
    <td width="100%"><asp:DropDownList ID="ddlRoom" runat="server" CssClass="default" /></td>
</tr>
<tr id="divDecom3" runat="server" style="display:none">
    <td nowrap>Shelf Number:<font class="required">&nbsp;*</font></td>
    <td width="100%"><asp:DropDownList ID="ddlShelf" runat="server" CssClass="default" /></td>
</tr>
<tr>
    <td nowrap>Decommissioned On:<font class="required">&nbsp;*</font></td>
    <td width="100%"><asp:TextBox ID="txtDate" runat="server" CssClass="default" Width="100" MaxLength="10" /> <asp:ImageButton ID="imgDate" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calendar.gif" /></td>
</tr>
<tr>
    <td nowrap>Decommissioned By:</td>
    <td width="100%"><asp:Label ID="lblDecommissionedBy" runat="server" CssClass="default" /></td>
</tr>
<tr>
    <td colspan="2"><hr size="1" noshade /></td>
</tr>
<tr>
    <td nowrap class="required">* = Required Field</td>
    <td align="right">
        <asp:Button ID="btnSubmit" runat="server" CssClass="default" Width="125" Text="Decommission" OnClick="btnSubmit_Click" />
        <asp:Button ID="btnCancel" runat="server" CssClass="default" Width="100" Text="Cancel" OnClick="btnCancel_Click" Visible="false" />
    </td>
</tr>
