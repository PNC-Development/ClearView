<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="sys_topnav.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.sys_topnav" %>


<asp:Panel ID="panWelcome" runat="server" Visible="false">
<table width="100%" cellpadding="2" cellspacing="0" border="0" class="default">
    <tr>
        <td><%=strUser%> - <asp:LinkButton ID="btnLogout" runat="server" Text="Log Out" OnClick="btnLogout_Click" /></td>
        <td align="right"><%=strTime%></td>
    </tr>
</table>
</asp:Panel>
<asp:Panel ID="panBread" runat="server" Visible="false">
&nbsp;<asp:LinkButton ID="btnHome" runat="server" Text="Clearview Home" CssClass="topnav" OnClick="btnHome_Click" /><%=strMenu%>
</asp:Panel>