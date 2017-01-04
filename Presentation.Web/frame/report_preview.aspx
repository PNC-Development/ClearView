<%@ Page Language="C#" MasterPageFile="~/none.Master" AutoEventWireup="true" CodeBehind="report_preview.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.report_preview" Title="Untitled Page" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" runat="server">
    <rsweb:ReportViewer ID="ReportViewer1" runat="server" Visible="false" width="100%" height="100%" ProcessingMode="Remote" ShowParameterPrompts="False" >
        <ServerReport />
    </rsweb:ReportViewer>
    <div id="divNA" runat="server" visible="false" class="default">
        <table width="100%" height="100%">
	        <tr><td>&nbsp;</td></tr>
	        <tr height="1"><td align="center"><img src="/images/bigError.gif" border="0"/></td></tr>
	        <tr height="1"><td>&nbsp;</td></tr>
	        <tr height="1"><td align="center"><b>Preview Not Available</b></td></tr>
	        <tr><td>&nbsp;</td></tr>
        </table>
    </div>
</asp:Content>
