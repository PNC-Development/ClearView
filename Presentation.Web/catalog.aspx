<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="catalog.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.catalog" %>

<html>
<head>
<title>ClearView | National City</title>
<link rel="stylesheet" type="text/css" href="/css/default.css" />
<script type="text/javascript">
    function Redirect() {
        var strURL = window.location.href;
        if (strURL.indexOf("redirect") == -1)
            setTimeout("RedirectGo()",1500);
    }
    function RedirectGo() {
        window.navigate(window.location + "&redirect=go");
    }
</script>
</head>
<body leftmargin="0" topmargin="0" onload="Redirect()">
<table width="100%" height="100%" cellpadding="4" cellspacing="3" border="0">
    <tr>
        <td valign="middle" align="center" class="header">
            <asp:Panel ID="panWait" runat="server" Visible="true">
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
	                <tr>
		                <td align="center" class="hugeheader"><p>Loading ClearView</p></td>
	                </tr>
	                <tr><td>&nbsp;</td></tr>
	                <tr><td>&nbsp;</td></tr>
	                <tr><td align="center">(Using profile: <%=strUser %>)</td></tr>
	                <tr><td>&nbsp;</td></tr>
	                <tr>
		                <td align="center" class="header"><img src="/images/wait.gif" border="0"/></td>
	                </tr>
	                <tr><td>&nbsp;</td></tr>
	                <tr><td>&nbsp;</td></tr>
	                <tr>
		                <td align="center" class="header">Please wait...</td>
	                </tr>
	                <tr><td>&nbsp;</td></tr>
	                <tr><td>&nbsp;</td></tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="panAccess" runat="server" Visible="false">
                <img src="/images/bigError.gif" border="0" align="absmiddle" /> Access Denied for user <%=strUser %>...
            </asp:Panel>
            <asp:Panel ID="panUser" runat="server" Visible="false">
                <img src="/images/bigError.gif" border="0" align="absmiddle" /> The user <%=strUser %> is not registered to use ClearView...
            </asp:Panel>
            <asp:Panel ID="panPage" runat="server" Visible="false">
                <img src="/images/bigError.gif" border="0" align="absmiddle" /> Missing a &quot;PAGEID&quot; querystring value...
            </asp:Panel>
        </td>
    </tr>
</table>
</body>
</html>