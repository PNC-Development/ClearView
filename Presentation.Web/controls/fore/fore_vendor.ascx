<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="fore_vendor.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.fore_vendor" %>
<script type="text/javascript">
</script>
<table width="100%" cellpadding="4" cellspacing="2" border="0">
    <tr>
        <td nowrap><b>Classification:</b><font class="required">&nbsp;*</font></td>
        <td width="100%"><asp:DropDownList ID="ddlPlatforms" runat="server" CssClass="default" AutoPostBack="true" OnSelectedIndexChanged="ddlPlatforms_Change" /></td>
    </tr>
    <asp:Panel ID="panTypes" runat="server" Visible="false">
    <tr>
        <td nowrap><b>Asset Type:</b><font class="required">&nbsp;*</font></td>
        <td width="100%"><asp:DropDownList ID="ddlTypes" runat="server" CssClass="default" /></td>
    </tr>
    <tr>
        <td nowrap><b>Make:</b><font class="required">&nbsp;*</font></td>
        <td width="100%"><asp:TextBox ID="txtMake" runat="server" CssClass="default" MaxLength="50" Width="200" /></td>
    </tr>
    <tr>
        <td nowrap><b>Model:</b><font class="required">&nbsp;*</font></td>
        <td width="100%"><asp:TextBox ID="txtModel" runat="server" CssClass="default" MaxLength="50" Width="200" /></td>
    </tr>
    <tr>
        <td nowrap><b>Size (W):</b><font class="required">&nbsp;*</font></td>
        <td width="100%"><asp:TextBox ID="txtWidth" runat="server" CssClass="default" MaxLength="8" Width="75" /> (inches)</td>
    </tr>
    <tr>
        <td nowrap><b>Size (H):</b><font class="required">&nbsp;*</font></td>
        <td width="100%"><asp:TextBox ID="txtHeight" runat="server" CssClass="default" MaxLength="8" Width="75" /> (inches)</td>
    </tr>
    <tr>
        <td nowrap><b>AMP Rating:</b><font class="required">&nbsp;*</font></td>
        <td width="100%"><asp:TextBox ID="txtAmp" runat="server" CssClass="default" MaxLength="8" Width="75" /> (amps)</td>
    </tr>
    <tr>
        <td></td>
        <td>
            Please provide any additional specifications:<br />
            <asp:TextBox ID="txtOther" runat="server" CssClass="default" Width="500" Rows="5" TextMode="MultiLine" />
        </td>
    </tr>
</table>
<table width="100%" cellpadding="4" cellspacing="2" border="0">
    <tr>
        <td colspan="3"><hr size="1" noshade /></td>
    </tr>
    <tr>
        <td class="required">* = Required Field</td>
        <td align="center">
            <asp:Panel ID="panNavigation" runat="server" Visible="false">
                <asp:Button ID="btnBack" runat="server" OnClick="btnBack_Click" Text="Back" CssClass="default" Width="75" /> <asp:Button ID="btnNext" runat="server" OnClick="btnNext_Click" Text="Next" CssClass="default" Width="75" />
            </asp:Panel>
            <asp:Panel ID="panUpdate" runat="server" Visible="false">
                <asp:Button ID="btnUpdate" runat="server" OnClick="btnUpdate_Click" Text="Update" CssClass="default" Width="75" /> <asp:Button ID="btnCancel" runat="server" OnClick="btnCancel_Click" Text="Cancel" CssClass="default" Width="75" />
            </asp:Panel>
        </td>
        <td align="right">
            <asp:Button ID="btnHundred" runat="server" OnClick="btnCancel_Click" Text="Back" CssClass="default" Width="75" Visible="false" /> 
            <asp:Button ID="btnClose" runat="server" Text="Close" CssClass="default" Width="75" />
        </td>
    </tr>
    </asp:Panel>
<asp:Label ID="lblVendor" runat="server" Visible="false" />
