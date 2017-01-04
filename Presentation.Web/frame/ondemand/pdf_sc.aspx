<%@ Page Language="C#" MasterPageFile="~/frame.Master" AutoEventWireup="true" CodeBehind="pdf_sc.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.pdf_sc" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" runat="server">
    <table width="100%" cellpadding="0" cellspacing="5" border="0">
        <tr>
            <td rowspan="2"><img src="/images/bigCheck.gif" border="0" align="absmiddle" /></td>
            <td class="header" width="100%" valign="bottom">Form Generated Successfully</td>
        </tr>
        <tr>
            <td width="100%" valign="top">The form was re-generated and sent via Email</td>
        </tr>
        <tr>
            <td colspan="2" align="center">&nbsp;</td>
        </tr>
        <tr>
            <td colspan="2" align="center"><input type="button" class="default" style="width:100px" onclick="javascript:history.go(-1)" value="Back" /></td>
        </tr>
    </table>
</asp:Content>
