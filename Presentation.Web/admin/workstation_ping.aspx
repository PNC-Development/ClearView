<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="workstation_ping.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.workstation_ping" %>


<html>
<head>
<title>ClearView Administration</title>
<link rel="stylesheet" type="text/css" href="/css/default.css" />
<script type="text/javascript" src="/javascript/global.js"></script>
<script type="text/javascript" src="/javascript/ajax.js"></script>
</head>
<body topmargin="0" leftmargin="0">
<form id="Form1" runat="server">
        <table width="100%" height="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
        <td height="100%" valign="top">
<table width="98%" cellpadding="0" cellspacing="0" border="0" align="center">
		<tr><td colspan="2">&nbsp;</td></tr>
		<tr> 
		    <td><b>Workstation Ping</b> <asp:Label ID="lblCount" runat="server" CssClass="default" /></td>
		    <td align="right">Delete Records Before: <asp:TextBox ID="txtDate" runat="server" CssClass="default" Width="100" MaxLength="10" /> <asp:Button ID="btnDelete" runat="server" CssClass="default" Width="75" Text="Delete" OnClick="btnDelete_Click" /></td>
		</tr>
		<tr><td colspan="2">&nbsp;</td></tr>
		<tr>
		    <td colspan="2">
                <asp:TreeView ID="oTreeview" runat="server" ShowLines="true" NodeIndent="35">
                    <NodeStyle CssClass="default" />
                </asp:TreeView>
            </td>
        </tr>
    </table>
        </td>
        </tr>
        </table>
</form>
</body>
</html>
