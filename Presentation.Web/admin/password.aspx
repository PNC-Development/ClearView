<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="password.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.password" %>


<html>
<head>
<title>Web Content Management Administration</title>
<link rel="stylesheet" type="text/css" href="/css/default.css" />
</head>
<body>
<form id="Form1" runat="server">
<table width="100%" height="100%" border="0" cellspacing="0" cellpadding="0">
	<tr>
		<td align="center" background="/images/admin.gif">
<table width="400" cellpadding="0" cellspacing="0" border="0">
    <tr>
        <td><img src="/images/table_topleft.gif" border="0" width="5" height="26"></td>
        <td nowrap background="/images/table_top.gif" class="greentableheader" width="100%">Change Password</td>
        <td><img src="/images/table_topright.gif" border="0" width="5" height="26"></td>
    </tr>
    <tr>
        <td background="/images/table_left.gif"><img src="/images/table_left.gif" width="5" height="10"></td>
        <td width="100%" bgcolor="#FFFFFF">
            <table width="100%" cellpadding="2" cellspacing="0" border="0">
                <tr><td>&nbsp;</td></tr>
                <tr>
                    <td>Username</td>
                    <td><asp:TextBox ID="txtUsername" runat="server" Width="200" CssClass="default" MaxLength="50" /></td>
                </tr>
                <tr>
                    <td>Password</td>
                    <td><asp:TextBox ID="txtPassword" runat="server" Width="200" CssClass="default" MaxLength="50" TextMode="Password" /></td>
                </tr>
                <tr>
                    <td>New Password</td>
                    <td><asp:TextBox ID="txtPassword1" runat="server" Width="200" CssClass="default" MaxLength="50" TextMode="Password" /></td>
                </tr>
                <tr>
                    <td>Confirm Password</td>
                    <td><asp:TextBox ID="txtPassword2" runat="server" Width="200" CssClass="default" MaxLength="50" TextMode="Password" /></td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td><asp:Button ID="btnChange" runat="server" Text="Change" Width="75" CssClass="default" OnClick="btnChange_Click" /></td>
                </tr>
                <tr><td>&nbsp;</td></tr>
                <tr>
                    <td colspan="2" align="center">
                        <asp:Label ID="lblInvalid" runat="server" CssClass="error" Text="<img src='/images/error.gif' border='0' align='absmiddle' /> Invalid Username or Password" Visible="false" />
                        <asp:Label ID="lblMatch" runat="server" CssClass="default" Text="<img src='/images/error.gif' border='0' align='absmiddle' /> The passwords do not match." Visible="false" />
                        <asp:Label ID="lblChange" runat="server" CssClass="default" Text="<img src='/images/alert.gif' border='0' align='absmiddle' /> The password has been changed." Visible="false" />
                    </td>
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
