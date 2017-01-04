<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="sync_ad.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.sync_ad" %>


<script type="text/javascript">
    function ShowWait(oShow, oHide) {
        oShow = document.getElementById(oShow);
        oHide = document.getElementById(oHide);
        oShow.style.display = "none";
        oHide.style.display = "inline";
    }
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
        <td height="100%">
        <div style="height:100%; overflow:auto">
<table width="98%" cellpadding="0" cellspacing="0" border="0" align="center">
		<tr><td colspan="2">&nbsp;</td></tr>
		<tr> 
		    <td><b>Sync Active Directory</b></td>
		    <td align="right"></td>
		</tr>
		<tr><td colspan="2">&nbsp;</td></tr>
		<tr>
		    <td colspan="2" align="center">
		        <div id="divShow" runat="server" style="display:inline">
		        <table width="100%" cellpadding="2" cellspacing="0" border="0">
		            <tr>
		                <td>
		                    <asp:RadioButtonList ID="radResults" runat="server" CssClass="default" RepeatDirection="Vertical">
		                        <asp:ListItem Value="Test Mode (Do Not Update Database - Show Results)" />
		                        <asp:ListItem Value="Test Mode - No Errors (Do Not Update Database - Show Results - Bypass Errors)" />
		                        <asp:ListItem Value="<font color=#990000><b>Live Mode (Update Database - Show Results)</b></font>" />
		                        <asp:ListItem Value="Live Mode - No Errors (Update Database - Show Results - Bypass Errors)" />
		                        <asp:ListItem Value="Silent Mode (Update Database - Do Not Show Results)" />
		                        <asp:ListItem Value="Silent Mode - No Errors  (Update Database - Do Not Show Results - Bypass Errors)" />
		                    </asp:RadioButtonList>
		                </td>
		            </tr>
		            <tr>
		                <td>&nbsp;</td>
		            </tr>
		            <tr>
		                <td>Prefix: <asp:Textbox ID="txtSync" runat="server" CssClass="default" Width="100" /></td>
		            </tr>
		            <tr>
		                <td>&nbsp;</td>
		            </tr>
		            <tr>
		                <td><asp:Button ID="btnSync" runat="server" CssClass="default" Width="125" Text="Sync Now" OnClick="btnSync_Click" /></td>
		            </tr>
		            <tr>
		                <td><hr size="1" noshade /></td>
		            </tr>
		            <tr>
		                <td><asp:Label ID="lblResults" runat="server" CssClass="default" /></td>
		            </tr>
		        </table>
		        </div>
		        <div id="divHide" runat="server" style="display:none">
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
	                <tr><td>&nbsp;</td></tr>
	                <tr><td>&nbsp;</td></tr>
	                <tr>
		                <td align="center" class="hugeheader"><p>ClearView is syncing with Active Directory</p></td>
	                </tr>
	                <tr><td>&nbsp;</td></tr>
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
	                <tr>
		                <td align="center" class="default">(This process normally takes between 5 and 10 minutes...)</td>
	                </tr>
	                <tr><td>&nbsp;</td></tr>
	                <tr><td>&nbsp;</td></tr>
                </table>
		        </div>
            </td>
        </tr>
    </table>
        </div>
        </td>
        </tr>
        </table>
</form>
</body>
</html>
