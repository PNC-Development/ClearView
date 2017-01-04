<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="change_user.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.change_user" %>

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
		    <td><b>Change Live User</b></td>
		    <td align="right"></td>
		</tr>
		<tr><td colspan="2">&nbsp;</td></tr>
		<tr>
		    <td colspan="2" align="center">
                <div ID="divView" runat="server" style="display:inline">
                    <table width="95%" border="0" cellspacing="0" cellpadding="0" align="center">
                        <tr>
                            <td>
                        <table cellpadding="0" cellspacing="0" border="0">
                            <tr>
                                <td><asp:TextBox ID="txtUser" runat="server" Width="300" CssClass="default" /></td>
                            </tr>
                            <tr>
                                <td>
                                    <div id="divAJAX" runat="server" style="overflow:hidden; position:absolute; display:none; background-color:#FFFFFF; border:solid 1px #CCCCCC">
                                        <asp:ListBox ID="lstAJAX" runat="server" CssClass="default" />
                                    </div>
                                </td>
                            </tr>
                        </table>
                        <input type="hidden" id="hdnAJAXValue" name="hdnAJAXValue" />
                            
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="default" Width="75" OnClick="btnSubmit_Click" />
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
    </table>
        </td>
        </tr>
        </table>
    <input type="hidden" id="hdnId" runat="server" />
<input type="hidden" id="hdnUsers" runat="server" />
</form>
</body>
</html>
