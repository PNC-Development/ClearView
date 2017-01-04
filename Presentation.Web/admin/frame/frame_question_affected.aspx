<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frame_question_affected.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.frame_question_affected" %>

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
		            <td><b>Response Affected</b></td>
	                <td align="right">
			            <a href="javascript:void(0);" onclick="parent.HidePanel();"><img src="/admin/images/close.gif" border="0" title="Close"></a>
	                </td>
	            </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <table width="100%" height="100%" border="0" cellSpacing="2" cellPadding="2" bgcolor="#c8cfdd" class="default">
                <tr>
                    <td nowrap><b>Question:</b></td>
                    <td width="100%"><asp:Label ID="lblQuestion" runat="server" CssClass="default" /></td>
                </tr>
                <tr>
                    <td nowrap><b>Response:</b></td>
                    <td width="100%"><asp:Label ID="lblResponse" runat="server" CssClass="default" /></td>
                </tr>
                <tr>
                    <td nowrap><b>Affected:</b></td>
                    <td width="100%"><asp:Label ID="lblAffected" runat="server" CssClass="default" /></td>
                </tr>
                <tr>
                    <td nowrap><b>State:</b></td>
                    <td width="100%">
                        <asp:DropDownList ID="ddlState" runat="server" CssClass="default">
                            <asp:ListItem Text="None" Value="0" />
                            <asp:ListItem Text="Inline" Value="1" />
                        </asp:DropDownList>
                    </td>
                </tr>
	            <tr height="1">
	                <td colspan="2"><hr size="1" noshade /></td>
	            </tr>
            </table>
        </td>
    </tr>
    <tr height="1">
        <td bgcolor="#c8cfdd"><asp:Button ID="btnDelete" runat="server" Text="Delete" Width="75" CssClass="default" OnClick="btnDelete_Click" /></td>
        <td align="right" bgcolor="#c8cfdd">
            <asp:Button ID="btnSave" runat="server" Text="Save" Width="75" CssClass="default" OnClick="btnSave_Click" /> 
            <asp:Button ID="btnClose" runat="server" Text="Close" Width="75" CssClass="default" />
        </td>
    </tr>
    <tr><td colspan="2" height="5" bgcolor="#c8cfdd"><img src="images/spacer.gif" width="1" height="5" /></td></tr>
</table>
<asp:Label ID="lblId" runat="server" Visible="false" />
<input type="hidden" runat="server" id="hdnId" />
</form>
</body>
