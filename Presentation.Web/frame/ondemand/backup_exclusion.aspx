<%@ Page Language="C#" MasterPageFile="~/frame.Master" AutoEventWireup="true" CodeBehind="backup_exclusion.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.backup_exclusion" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" runat="server">
<script type="text/javascript">
    function Reload() {
        var strParent = parent.location.toString();
        if (parent.location.toString().indexOf("&child") == -1)
            strParent += "&child=true";
        parent.navigate(strParent);
        parent.HidePanel();
    }
</script>
<table cellpadding="3" cellspacing="2" border="0">
    <tr>
        <td class="bold">File, Folder, File Space Path:</td>
    </tr>
    <tr>
        <td><asp:TextBox ID="txtPath" runat="server" CssClass="default" Width="450" MaxLength="200" /></td>
    </tr>
    <tr>
        <td class="footer">Example: C:\WINNT</td>
    </tr>
    <tr>
        <td><asp:Button ID="btnAdd" runat="server" CssClass="default" Width="125" Text="Add Exclusion" OnClick="btnAdd_Click" /></td>
    </tr>
</table>
</asp:Content>
