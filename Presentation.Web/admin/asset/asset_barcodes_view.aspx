<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="asset_barcodes_view.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.asset_barcodes_view" %>

<html>
<head>
<title>ClearView | Barcodes</title>
<link rel="stylesheet" type="text/css" href="/css/default.css" />
<script src="/javascript/default.js"type="text/javascript"></script>
<script src="/javascript/both.js"type="text/javascript"></script>
</head>
<body leftmargin="0" topmargin="0">
<form id="Form1" runat="server">
<table>
    <tr>
        <td valign="top"><%=strCodes1 %></td>
        <td valign="top"><%=strCodes2 %></td>
        <!--<td valign="top"><%=strCodes3 %></td>-->
    </tr>
</table>
</form>
</body>
</html>
