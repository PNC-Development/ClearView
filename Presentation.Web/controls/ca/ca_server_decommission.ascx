<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ca_server_decommission.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.ca_server_decommission" %>
<script type="text/javascript">
    function ExecuteCancel(strName, strRedirect) {
        LoadWait();
        //alert('The service "' + strName + '" has been cancelled!\n\nNOTE: If this was the only service submitted in this request, the following page might be empty.');
        window.navigate(strRedirect);
    }
</script>