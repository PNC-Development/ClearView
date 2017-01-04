<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="asset_commission_wan.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.asset_commission_wan" %>
<script type="text/javascript">
</script>
<tr>
    <td nowrap>Serial Number:</td>
    <td width="100%"><asp:Label ID="lblSerial" runat="server" CssClass="default" /></td>
</tr>
<tr>
    <td nowrap>Asset Tag:<font class="required">&nbsp;*</font></td>
    <td width="100%"><asp:TextBox ID="txtAsset" runat="server" CssClass="default" Width="150" MaxLength="20" /></td>
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
    <td nowrap>Available Ports:<font class="required">&nbsp;*</font></td>
    <td width="100%"><asp:TextBox ID="txtPorts" runat="server" CssClass="default" Width="75" MaxLength="10" /></td>
</tr>
<asp:Panel ID="panReceived" runat="server" Visible="false">
<tr>
    <td nowrap>Received On:</td>
    <td width="100%"><asp:Label ID="lblReceivedOn" runat="server" CssClass="default" /></td>
</tr>
<tr>
    <td nowrap>Received By:</td>
    <td width="100%"><asp:Label ID="lblReceivedBy" runat="server" CssClass="default" /></td>
</tr>
</asp:Panel>
<tr>
    <td colspan="2"><span style="width:100%;border-bottom:1 dotted #CCCCCC;"/></td>
</tr>
<tr>
    <td nowrap>Location:<font class="required">&nbsp;*</font></td>
    <td width="100%"><%=strLocation %></td>
</tr>
<tr>
    <td nowrap>Room:<font class="required">&nbsp;*</font></td>
    <td width="100%"><asp:TextBox ID="txtRoom" runat="server" CssClass="default" Width="200" MaxLength="100" /></td>
</tr>
<tr>
    <td nowrap>Rack:<font class="required">&nbsp;*</font></td>
    <td width="100%"><asp:TextBox ID="txtRack" runat="server" CssClass="default" Width="200" MaxLength="100" /></td>
</tr>
<tr>
    <td nowrap>Rack Position:<font class="required">&nbsp;*</font></td>
    <td width="100%"><asp:TextBox ID="txtRackPosition" runat="server" CssClass="default" Width="200" MaxLength="100" /></td>
</tr>
<tr>
    <td nowrap>Current Class:<font class="required">&nbsp;*</font></td>
    <td width="100%"><asp:DropDownList ID="ddlClass" runat="server" CssClass="default" Width="200" /></td>
</tr>
<tr>
    <td nowrap>Environment:<font class="required">&nbsp;*</font></td>
    <td width="100%">
        <asp:DropDownList ID="ddlEnvironment" runat="server" CssClass="default" Width="200" Enabled="false" >
            <asp:ListItem Value="-- Please select a Class --" />
        </asp:DropDownList>
    </td>
</tr>
<tr>
    <td nowrap>Device Name:<font class="required">&nbsp;*</font></td>
    <td width="100%"><asp:TextBox ID="txtName" runat="server" CssClass="default" Width="200" MaxLength="100" /></td>
</tr>
<tr>
    <td nowrap>IP Address:<font class="required">&nbsp;*</font></td>
    <td width="100%"><asp:TextBox ID="txtIP1" runat="server" CssClass="default" Width="30" MaxLength="3" /> . <asp:TextBox ID="txtIP2" runat="server" CssClass="default" Width="30" MaxLength="3" /> . <asp:TextBox ID="txtIP3" runat="server" CssClass="default" Width="30" MaxLength="3" /> . <asp:TextBox ID="txtIP4" runat="server" CssClass="default" Width="30" MaxLength="3" /></td>
</tr>
<tr>
    <td nowrap>Commissioned By:</td>
    <td width="100%"><asp:Label ID="lblCommissionedBy" runat="server" CssClass="default" /></td>
</tr>
<asp:Panel ID="panCommission" runat="server" Visible="false">
<tr>
    <td nowrap>Commissioned On:<font class="required">&nbsp;*</font></td>
    <td width="100%"><asp:TextBox ID="txtDate" runat="server" CssClass="default" Width="100" MaxLength="10" /> <asp:ImageButton ID="imgDate" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calendar.gif" /></td>
</tr>
</asp:Panel>
<asp:Panel ID="panCommissioned" runat="server" Visible="false">
<tr>
    <td nowrap>Commissioned On:</td>
    <td width="100%"><asp:Label ID="lblCommissionedOn" runat="server" CssClass="default" /></td>
</tr>
</asp:Panel>
<tr>
    <td colspan="2"><hr size="1" noshade /></td>
</tr>
<tr>
    <td nowrap class="required">* = Required Field</td>
    <td align="right"><asp:Button ID="btnSubmit" runat="server" CssClass="default" Width="75" Text="Save" OnClick="btnSubmit_Click" /></td>
</tr>
<input type="hidden" id="hdnEnvironment" runat="server" />
<input type="hidden" id="hdnLocation" runat="server" />
