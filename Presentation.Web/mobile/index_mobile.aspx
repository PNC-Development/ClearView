<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index_mobile.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.index_mobile" %>

<html>
<head>
<title>ClearView - Scan Gun Import</title>
</head>
<body style="margin-left: 0px; margin-top: 0px;">
<form id="Form1" runat="server" enctype="multipart/form-data">
<table border="0">
    <tr>
        <td nowrap>Path:</td>
        <td width="100%"><asp:FileUpload runat="server" ID="oFile" Width="500" CssClass="default" /></td>
    </tr>
    <tr>
        <td>&nbsp;</td>
        <td><asp:Button ID="btnUpload" runat="server" CssClass="default" Text="Upload" Width="75" OnClick="btnUpload_Click" /></td>
    </tr>
    <tr>
        <td colspan="2">&nbsp;</td>
    </tr>
    <tr>
        <td>&nbsp;</td>
        <td><asp:Label ID="lblResults" runat="server" CssClass="default" /></td>
    </tr>
</table>
</form>
</body>
</html>
