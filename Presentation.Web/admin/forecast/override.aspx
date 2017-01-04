<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="override.aspx.cs" Inherits="NCC.ClearView.Presentation.Web._override" %>
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
		    <td><b>Override Forecast Code</b></td>
		    <td align="right"></td>
		</tr>
		<tr><td colspan="2">&nbsp;</td></tr>
		<tr>
		    <td colspan="2" align="center">
                <div ID="divView" runat="server" style="display:inline">
                    <table width="95%" border="0" cellspacing="2" cellpadding="3" align="center">
                        <tr>
                            <td>Workstation:</td>
                            <td><asp:TextBox ID="txtWorkstation" runat="server" Width="500" CssClass="default" /></td>
                        </tr>
                        <tr>
                            <td>Code:</td>
                            <td><asp:TextBox ID="txtCode" runat="server" Width="500" CssClass="default" /></td>
                        </tr>
                        <tr>
                            <td></td>
                            <td><asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="default" Width="75" OnClick="btnSubmit_Click" /></td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
    </table>
        </td>
        </tr>
        </table>
</form>
</body>
</html>
override" %>

