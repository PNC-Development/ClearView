<%@ Page Language="C#" MasterPageFile="~/frame.Master" AutoEventWireup="true" CodeBehind="project_browser.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.project_browser" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" runat="server">
<script type="text/javascript">
function ShowAccountDetail(oDiv) {
    oDiv = document.getElementById(oDiv);
    if (oDiv.style.display == 'inline')
        oDiv.style.display = 'none';
    else
        oDiv.style.display = 'inline';
}
</script>
<table width="100%" cellpadding="4" cellspacing="2" border="0">
    <tr>
        <td class="header" nowrap>Project Name:</td>
        <td class="header" width="100%"><%=strTitle %></td>
    </tr>
    <tr>
        <td valign="top" colspan="2"><br /><%=strView%></td>
    </tr>
</table>
</asp:Content>
