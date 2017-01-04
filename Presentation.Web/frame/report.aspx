<%@ Page Language="C#" MasterPageFile="~/none.Master" AutoEventWireup="true" CodeBehind="report.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.report" Title="Untitled Page" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" runat="server">
<rsweb:ReportViewer ID="ReportViewer1" runat="server" width="100%" height="100%" ProcessingMode="Remote" ShowParameterPrompts="True" >
    <ServerReport />
</rsweb:ReportViewer>
</asp:Content>
