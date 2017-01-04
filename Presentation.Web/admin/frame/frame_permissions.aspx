<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frame_permissions.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.frame_permissions" %>
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
<table width="100%" height="100%" border="0" cellSpacing="0" cellPadding="0">
    <tr height="1">
        <td colspan="2">
            <table width="100%" border="0" cellSpacing="2" cellPadding="2" bgcolor="#e6e9f0">
	            <tr bgcolor="#e6e9f0">
		            <td><b>Manage Permission</b></td>
	                <td align="right">
			            <a href="javascript:void(0);" onclick="parent.HidePanel();"><img src="/admin/images/close.gif" border="0" title="Close"></a>
	                </td>
	            </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <div style="height:100%; width:100%; overflow:auto;">
            <table width="100%" height="100%" border="0" cellSpacing="2" cellPadding="2" bgcolor="#c8cfdd" class="default">
                <tr> 
                    <td>Application:</td>
                    <td><asp:label ID="lblApplication" CssClass="default" runat="server" /></td>
                </tr>
                <tr> 
                    <td>Group:</td>
                    <td><asp:DropDownList ID="ddlGroup" CssClass="default" runat="server" /></td>
                </tr>
                <tr> 
                    <td>Permission:</td>
                    <td>
                        <asp:DropDownList ID="ddlPermission" CssClass="default" runat="server">
                            <asp:ListItem Value="0" Text="-- SELECT --" />
                            <asp:ListItem Value="1" Text="Green" />
                            <asp:ListItem Value="2" Text="Bronze" />
                            <asp:ListItem Value="3" Text="Silver" />
                            <asp:ListItem Value="4" Text="Gold" />
                        </asp:DropDownList>
                    </td>
                </tr>
	            <tr height="1">
	                <td colspan="2"><hr size="1" noshade /></td>
	            </tr>
            </table>
            </div>
        </td>
    </tr>
    <tr height="1">
        <td bgcolor="#c8cfdd"></td>
        <td align="right" bgcolor="#c8cfdd">
            <asp:Button ID="btnSave" runat="server" Text="Save" Width="75" CssClass="default" OnClick="btnSave_Click" /> 
            <asp:Button ID="btnDelete" runat="server" Text="Delete" Width="75" CssClass="default" OnClick="btnDelete_Click" /> 
            <asp:Button ID="btnClose" runat="server" Text="Close" Width="75" CssClass="default" />
        </td>
    </tr>
    <tr><td colspan="2" height="5" bgcolor="#c8cfdd"><img src="images/spacer.gif" width="1" height="5" /></td></tr>
</table>
<asp:Label ID="lblId" runat="server" Visible="false" />
<input type="hidden" runat="server" id="hdnId" />
<input type="hidden" id="hdnApplication" runat="server" />
</form>
</body>
</html>
