<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="asset_checkin_wan.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.asset_checkin_wan" %>
<script type="text/javascript">
</script>
<tr>
    <td nowrap>Model:<font class="required">&nbsp;*</font></td>
    <td width="100%"><asp:DropDownList ID="ddlModel" runat="server" CssClass="default" Width="300" /></td>
</tr>
<tr>
    <td nowrap>Serial Number:<font class="required">&nbsp;*</font></td>
    <td width="100%"><asp:TextBox ID="txtSerial" runat="server" CssClass="default" Width="150" MaxLength="50" /></td>
</tr>
<tr>
    <td nowrap>Asset Tag:</td>
    <td width="100%"><asp:TextBox ID="txtAsset" runat="server" CssClass="default" Width="150" MaxLength="20" /></td>
</tr>
<tr>
    <td nowrap>Depot Location:<font class="required">&nbsp;*</font></td>
    <td width="100%"><asp:DropDownList ID="ddlDepot" runat="server" CssClass="default" /></td>
</tr>
<tr>
    <td nowrap>Depot Room:<font class="required">&nbsp;*</font></td>
    <td width="100%"><asp:TextBox ID="txtDepotRoom" runat="server" CssClass="default" Width="150" MaxLength="100" /></td>
</tr>
<tr>
    <td nowrap>Shelf Number:<font class="required">&nbsp;*</font></td>
    <td width="100%"><asp:TextBox ID="txtShelf" runat="server" CssClass="default" Width="150" MaxLength="100" /></td>
</tr>
<tr>
    <td nowrap>P.O. Number:</td>
    <td width="100%"><asp:TextBox ID="txtPo" runat="server" CssClass="default" Width="150" MaxLength="20" /></td>
</tr>
<tr>
    <td nowrap>Received On:<font class="required">&nbsp;*</font></td>
    <td width="100%"><asp:TextBox ID="txtDate" runat="server" CssClass="default" Width="100" MaxLength="10" /> <asp:ImageButton ID="imgDate" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calendar.gif" /></td>
</tr>
<tr>
    <td nowrap>Available Ports:<font class="required">&nbsp;*</font></td>
    <td width="100%"><asp:TextBox ID="txtPorts" runat="server" CssClass="default" Width="75" MaxLength="10" /></td>
</tr>
<tr>
    <td colspan="2"><hr size="1" noshade /></td>
</tr>
<tr>
    <td nowrap class="required">* = Required Field</td>
    <td align="right"><asp:Button ID="btnSubmit" runat="server" CssClass="default" Width="75" Text="Check-In" OnClick="btnSubmit_Click" /></td>
</tr>
