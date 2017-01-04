<%@ Page Language="C#" MasterPageFile="~/frame.Master" AutoEventWireup="true" CodeBehind="services_detail.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.services_detail" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" Runat="Server">
<script type="text/javascript">
function ShowDetail(oImg, oDiv) {
    oImg = document.getElementById(oImg);
    oDiv = document.getElementById(oDiv);
    if (oDiv.style.display == 'inline') {
        oDiv.style.display = 'none';
        SwapImage(oImg, '/images/plus.gif');
    }
    else {
        oDiv.style.display = 'inline';
        SwapImage(oImg, '/images/minus.gif');
    }
}
</script>
<%=strView %>
</asp:Content>
