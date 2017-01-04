<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="settings.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.settings" %>


<html>
<head>
<title>ClearView Administration</title>
<link rel="stylesheet" type="text/css" href="/css/default.css" />
<script type="text/javascript" src="/javascript/global.js"></script>
</head>
<body topmargin="0" leftmargin="0">
<form id="Form1" runat="server">
        <table width="100%" height="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
        <td height="100%" valign="top">
<table width="98%" cellpadding="0" cellspacing="0" border="0" align="center">
		<tr><td colspan="2">&nbsp;</td></tr>
		<tr>
		    <td colspan="2" align="center">
                <table width="100%" border="0" cellpadding="2" cellspacing="0">
                    <tr>
                        <td><asp:Button ID="btnSave" runat="server" Text="Delete All" Width="75" OnClick="btnSave_Click" CssClass="default" /></td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
        </td>
        </tr>
        </table>
</form>
</body>
</html>
