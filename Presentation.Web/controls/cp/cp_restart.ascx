<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cp_restart.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.cp_restart" %>
<%=strDone%>
<script type="text/javascript">
function ShowAccountDetail(oDiv) {
    oDiv = document.getElementById(oDiv);
    if (oDiv.style.display == 'inline')
        oDiv.style.display = 'none';
    else
        oDiv.style.display = 'inline';
}
</script>
