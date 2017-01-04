<%@ Page Language="C#" MasterPageFile="~/frame.Master" AutoEventWireup="true" CodeBehind="document_repository_rename.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.document_repository_rename" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" Runat="Server">       
<table cellpadding="2" cellspacing="2" class="default" width="100%">
    <tr>
        <td nowrap>New Name:<font class="required">&nbsp;*</font></td>
        <td width="100%"><asp:TextBox ID="txtName" runat="server" CssClass="default" Width="200" MaxLength="50" /> <%= strType %></td>
    </tr>
    <tr>
        <td>&nbsp;</td>
       <td><asp:Button ID="btnRename" runat="server" CssClass="default" Text="Rename" Width="85" OnClick="btnRename_Click" /></td>
    </tr>                  
    <tr>
        <td colspan="2" width="100%"><asp:Label Font-Bold="true" ForeColor="#FF0000" ID="lblError" runat="server"></asp:Label></td>
    </tr>               
</table>
</asp:Content>
