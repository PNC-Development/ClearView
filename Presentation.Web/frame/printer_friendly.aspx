<%@ Page Language="C#" MasterPageFile="~/window.Master" AutoEventWireup="true" CodeBehind="printer_friendly.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.printer_friendly" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" runat="server">
<asp:PlaceHolder ID="PH3" runat="server" />
<script type="text/javascript">EnablePostBack('<%=strPage%>','<%=Request.Path%>');</script>
</asp:Content>
