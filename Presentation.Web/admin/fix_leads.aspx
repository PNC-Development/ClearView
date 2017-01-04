<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="fix_leads.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.fix_leads" %>


<script type="text/javascript">
</script>
<html>
<head>
<title>Web Content Management Administration</title>
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
		    <td><b>Fix Project Leads</b></td>
		    <td align="right"></td>
		</tr>
		<tr><td colspan="2">&nbsp;</td></tr>
		<tr><td colspan="2">Click this button to update the project coordinator / technical project mangagers for projects...</td></tr>
		<tr><td colspan="2">&nbsp;</td></tr>
		<tr><td colspan="2"><asp:Button ID="btnGo" runat="server" CssClass="default" Width="75" OnClick="btnGo_Click" Text="Go" /></td></tr>
		<tr><td colspan="2">&nbsp;</td></tr>
		<tr><td colspan="2"><asp:Label ID="lblDone" runat="server" CssClass="default" /></td></tr>
    </table>
        </td>
        </tr>
        </table>
</form>
</body>
</html>
