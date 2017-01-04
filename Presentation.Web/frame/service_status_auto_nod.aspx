<%@ Page Language="C#" MasterPageFile="~/frame.Master" AutoEventWireup="true" CodeBehind="service_status_auto_nod.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.service_status_auto_nod" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" Runat="Server">
 <script type="text/javascript">
function ShowAccountDetail(oDiv) {
    oDiv = document.getElementById(oDiv);
    if (oDiv.style.display == 'inline')
        oDiv.style.display = 'none';
    else
        oDiv.style.display = 'inline';
}
</script>
    <table width="100%" height="100%" cellpadding="3" cellspacing="2" border="0">
        <tr height="1">
            <td class="header">&quot;<%=strTitle %>&quot; Results...</td>
        </tr>
        <tr height="1">
            <td><span style="width:100%;border-bottom:1 dotted #CCCCCC;"/></td>
        </tr>
        <tr>
            <td>
                <div style="height:100%; overflow:auto"><%=strView%></div>
            </td>
        </tr>
    </table>
</asp:Content>
