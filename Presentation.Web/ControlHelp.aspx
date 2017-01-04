<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ControlHelp.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.ControlHelp" %>

<html>
<head runat="server">
<title id="Title1" runat="server">National City</title>
<link rel="stylesheet" type="text/css" href="/css/default.css" />
</head>
<body style="margin-top:0; margin-left:0">
<table id="tblHelp" runat="server" width="100%" height="100%" >
    <tr height="1"><td class="bigblue" id="tdHeader"></td></tr>
    <tr height="1"><td><img src="/images/spacer.gif" border="0" height="1" /></td></tr>
    <tr>
        <td>
            <div id="divHelp" runat="server" style="height:100%; overflow:auto"></div>
        </td>
    </tr>
</table>

</body>
</html>