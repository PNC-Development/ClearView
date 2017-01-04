<%@ Page Language="C#" MasterPageFile="~/frame.Master" AutoEventWireup="true" CodeBehind="report_about.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.report_about" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" runat="server">
    <table width="100%" height="100%" cellpadding="2" cellspacing="0" border="0">
        <tr height="1">
            <td class="header"><%=strTitle%></td>
        </tr>
        <tr>
            <td valign="top"><%=strAbout%></td>
        </tr>
    </table>
</asp:Content>
