<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="virtual_host.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.virtual_host" %>


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
		    <td colspan="2"><b>Add a Virtual Workstation Host</b></td>
		</tr>
		<tr>
		    <td colspan="2">&nbsp;</td>
		</tr>
		<tr>
		    <td nowrap>Environment:</td>
		    <td width="100%">
		        <asp:RadioButtonList ID="radEnvironment" runat="server" CssClass="default" RepeatDirection="Horizontal">
		            <asp:ListItem Value="PROD" />
		            <asp:ListItem Value="TEST" />
		        </asp:RadioButtonList>
		    </td>
		</tr>
		<tr>
		    <td nowrap>Server Name:</td>
		    <td width="100%"><asp:TextBox ID="txtName" runat="server" CssClass="default" Width="200" MaxLength="50" /></td>
		</tr>
		<tr>
		    <td nowrap>Serial Number:</td>
		    <td width="100%"><asp:TextBox ID="txtSerial" runat="server" CssClass="default" Width="200" MaxLength="50" /></td>
		</tr>
		<tr>
		    <td nowrap>Asset Tag:</td>
		    <td width="100%"><asp:TextBox ID="txtAsset" runat="server" CssClass="default" Width="200" MaxLength="50" /></td>
		</tr>
		<tr>
		    <td nowrap>Room:</td>
		    <td width="100%"><asp:TextBox ID="txtRoom" runat="server" CssClass="default" Width="200" MaxLength="50" /></td>
		</tr>
		<tr>
		    <td nowrap>Rack:</td>
		    <td width="100%"><asp:TextBox ID="txtRack" runat="server" CssClass="default" Width="200" MaxLength="50" /></td>
		</tr>
		<tr>
		    <td nowrap>Rack Position:</td>
		    <td width="100%"><asp:TextBox ID="txtRackPos" runat="server" CssClass="default" Width="200" MaxLength="50" /></td>
		</tr>
		<tr>
		    <td nowrap>ILO:</td>
		    <td width="100%"><asp:TextBox ID="txtILO" runat="server" CssClass="default" Width="200" MaxLength="50" /></td>
		</tr>
		<tr>
		    <td nowrap>MAC Address:</td>
		    <td width="100%"><asp:TextBox ID="txtMAC" runat="server" CssClass="default" Width="200" MaxLength="50" /></td>
		</tr>
		<tr>
		    <td nowrap>VLAN:</td>
		    <td width="100%"><asp:TextBox ID="txtVlan" runat="server" CssClass="default" Width="200" MaxLength="50" /></td>
		</tr>
        <tr>
            <td nowrap>Model:</td>
            <td width="100%"><asp:label ID="lblParent" CssClass="default" runat="server" />&nbsp;&nbsp;<asp:Button ID="btnParent" runat="server" CssClass="default" Width="25" Text="..." /></td>
        </tr>
		<tr>
		    <td colspan="2">&nbsp;</td>
		</tr>
		<tr>
		    <td colspan="2"><asp:Button ID="btnGo" runat="server" CssClass="default" Width="75" OnClick="btnGo_Click" Text="Go" /></td>
		</tr>
		<tr>
		    <td colspan="2">&nbsp;</td>
		</tr>
		<tr>
		    <td colspan="2"><asp:Label ID="lblDone" runat="server" CssClass="default" /></td>
		</tr>
    </table>
        </td>
        </tr>
        </table>
    <input type="hidden" id="hdnParent" runat="server" />
</form>
</body>
</html>
