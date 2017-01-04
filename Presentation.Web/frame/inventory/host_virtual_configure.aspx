<%@ Page Language="C#" MasterPageFile="~/frame.Master" AutoEventWireup="true" CodeBehind="host_virtual_configure.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.host_virtual_configure" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" Runat="Server">
<script type="text/javascript">
</script>
<table width="100%" cellpadding="4" cellspacing="0" border="0">
    <tr>
        <td colspan="2" class="header">Configure a Virtual Host</td>
    </tr>
    <tr>
        <td nowrap>Maximum Guests:</td>
        <td width="100%"><asp:TextBox ID="txtGuests" runat="server" CssClass="default" Width="100" MaxLength="5" /></td>
    </tr>
    <tr>
        <td nowrap>Processors:</td>
        <td width="100%"><asp:TextBox ID="txtProcessors" runat="server" CssClass="default" Width="100" MaxLength="5" /></td>
    </tr>
    <tr>
        <td colspan="2">&nbsp;</td>
    </tr>
    <tr>
        <td nowrap>&nbsp;</td>
        <td width="100%"><asp:Button ID="btnEnvironment" runat="server" CssClass="default" Width="175" Text="Configure Environments" /> <asp:Button ID="btnOS" runat="server" CssClass="default" Width="175" Text="Configure Operating Systems" /></td>
    </tr>
    <tr>
        <td colspan="2">&nbsp;</td>
    </tr>
    <tr>
        <td nowrap>&nbsp;</td>
        <td width="100%"><asp:Button ID="btnSave" runat="server" CssClass="default" Width="100" Text="Save" OnClick="btnSave_Click" /></td>
    </tr>
</table>
</asp:Content>
