<%@ Page Language="C#" MasterPageFile="~/frame.Master" AutoEventWireup="true" CodeBehind="consistency_new.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.consistency_new" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" runat="server">
<table width="100%" cellpadding="0" cellspacing="5" border="0">
    <tr>
        <td rowspan="2"><img src="/images/consistency_add.gif" border="0" align="absmiddle" /></td>
        <td colspan="2" class="header" width="100%" valign="bottom">Add a Consistency Group</td>
    </tr>
    <tr>
        <td colspan="2" width="100%" valign="top">If you cannot find the consistency group, use this page to create it.</td>
    </tr>
    <tr>
        <td></td>
        <td nowrap>Name:</td>
        <td width="100%"><asp:TextBox ID="txtName" runat="server" CssClass="default" Width="300" MaxLength="100" /></td>
    </tr>
    <tr>
        <td></td>
        <td nowrap>&nbsp;</td>
        <td width="100%"><asp:Button ID="btnSubmit" runat="server" CssClass="default" Width="75" Text="Create" OnClick="btnSubmit_Click" /></td>
    </tr>
</table>
</asp:Content>
