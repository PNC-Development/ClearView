<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.AdminLogin" %>

<script type="text/javascript">
    function Load() {
        if (window.parent != null && window.parent != window)
            window.parent.navigate(window.location);
    }
</script>
<html>
<head>
<title>Web Content Management Administration</title>
<link rel="stylesheet" type="text/css" href="/css/default.css" />
</head>
<body onload="Load()">
<form id="Form1" runat="server">
<table width="100%" height="100%" border="0" cellspacing="0" cellpadding="0">
	<tr>
		<td align="center" background="/images/admin.gif">
<table width="400" cellpadding="0" cellspacing="0" border="0">
    <tr>
        <td><img src="/images/table_topleft.gif" border="0" width="5" height="26"></td>
        <td nowrap background="/images/table_top.gif" class="greentableheader" width="100%">Login</td>
        <td><img src="/images/table_topright.gif" border="0" width="5" height="26"></td>
    </tr>
    <tr>
        <td background="/images/table_left.gif"><img src="/images/table_left.gif" width="5" height="10"></td>
        <td width="100%" bgcolor="#FFFFFF">
            <table width="100%" cellpadding="2" cellspacing="0" border="0" class="greentable">
                <tr height="5">
                    <td colspan="2"><img src="/images/spacer.gif" border="0" height="5" width="1" /></td>
                </tr>
                <tr>
                    <td>Username</td>
                    <td><asp:TextBox ID="txtUsername" runat="server" Width="200" CssClass="default" MaxLength="50" /></td>
                </tr>
                <tr>
                    <td>Password</td>
                    <td><asp:TextBox ID="txtPassword" runat="server" Width="200" CssClass="default" MaxLength="50" TextMode="Password" /></td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td><asp:Button ID="btnLogin" runat="server" Text="Login" Width="75" CssClass="default" OnClick="btnLogin_Click" /></td>
                </tr>
                <tr>
                    <td colspan="2" align="center"><asp:Label ID="lblInvalid" runat="server" CssClass="error" Text="<img src='/images/error.gif' border='0' align='absmiddle' /> Invalid Username or Password" Visible="false" /></td>
                </tr>
                <tr>
                    <td colspan="2" align="center">&nbsp;</td>
                </tr>
                <tr>
                    <td colspan="2" align="right"><img src="/images/check.gif" border="0" align="absmiddle" /> <a href="/index.aspx">Click here to return to the public system</a></td>
                </tr>
            </table>
        </td>
        <td background="/images/table_right.gif"><img src="/images/table_right.gif" width="5" height="10"></td>
    </tr>
    <tr>
        <td><img src="/images/table_bottomLeft.gif" border="0" width="5" height="9"></td>
        <td width="100%" background="/images/table_bottom.gif"></td>
        <td><img src="/images/table_bottomRight.gif" border="0" width="5" height="9"></td>
    </tr>
</table>
		</td>
	</tr>
</table>
</form>
</body>
</html>
