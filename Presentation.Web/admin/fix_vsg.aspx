<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="fix_vsg.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.fix_vsg" %>

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
		    <td colspan="2"><b>Import VSG Numbers</b></td>
		</tr>
		<tr><td colspan="2">&nbsp;</td></tr>
        <tr>
            <td nowrap>Type:</td>
            <td width="100%">
                <asp:DropDownList runat="server" ID="ddlType" Width="300" CssClass="default">
                    <asp:ListItem Value="SERVER" />
                    <asp:ListItem Value="WORKSTATION" />
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td nowrap>Path:</td>
            <td width="100%"><asp:FileUpload runat="server" ID="oFile" Width="500" CssClass="default" /></td>
        </tr>
        <tr>
            <td>&nbsp;</td>
            <td><asp:Button ID="btnGo" runat="server" CssClass="default" Width="75" OnClick="btnGo_Click" Text="Go" /></td>
        </tr>
		<tr><td colspan="2">&nbsp;</td></tr>
		<tr><td colspan="2"><asp:Label ID="lblDone" runat="server" CssClass="default" /></td></tr>
    </table>
        </td>
        </tr>
        </table>
</form>
</body>
</html>

