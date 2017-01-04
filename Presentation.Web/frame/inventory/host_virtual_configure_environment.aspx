<%@ Page Language="C#" MasterPageFile="~/frame.Master" AutoEventWireup="true" CodeBehind="host_virtual_configure_environment.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.host_virtual_configure_environment" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" Runat="Server">
<script type="text/javascript">
</script>
<table width="100%" cellpadding="4" cellspacing="0" border="0">
    <tr>
        <td class="header">Configure a Virtual Host Environments</td>
    </tr>
    <tr>
        <td width="100%" align="center"><asp:ListBox ID="lstEnvironment" runat="server" CssClass="default" Width="300" Rows="15" SelectionMode="Multiple" /></td>
    </tr>
    <tr>
        <td width="100%" align="center"><asp:Button ID="btnSave" runat="server" CssClass="default" Width="100" Text="Save" OnClick="btnSave_Click" /></td>
    </tr>
</table>
</asp:Content>
