<%@ Page Language="C#" MasterPageFile="~/frame.Master" AutoEventWireup="true" CodeBehind="service_status_assign_storage_3rd.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.service_status_assign_storage_3rd" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" Runat="Server">
<script type="text/javascript">
	function ShowHide(oDiv) {
		oDiv = document.getElementById(oDiv);
		if (oDiv.style.display == "none")
			oDiv.style.display = "inline";
		else
			oDiv.style.display = "none";
	}
</script>
<%=strView%>
</asp:Content>
