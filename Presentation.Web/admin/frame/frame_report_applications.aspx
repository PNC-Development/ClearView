<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frame_report_applications.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.frame_report_applications" %>


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
		            <td><b>Report Applications</b></td>
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
            <table width="100%" height="100%" border="0" cellSpacing="2" cellPadding="2" bgcolor="#c8cfdd">
                <tr> 
                    <td>Report:</td>
                    <td width="100%"><asp:label ID="lblName" CssClass="default" runat="server" /></td>
                </tr>
                <tr>
                    <td>Available:</td>
                    <td width="100%"><asp:DropDownList ID="ddlAvailable" runat="server" CssClass="default" />
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td>
                        <asp:Button ID="btnAdd" runat="server" CssClass="default" Width="75" Text="Add" OnClick="btnAdd_Click" /> 
                        <asp:Button ID="btnRemove" runat="server" CssClass="default" Width="75" Text="Remove" OnClick="btnRemove_Click" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2"><asp:ListBox ID="lstCurrent" runat="server" Width="400" CssClass="default" Rows="10" /></td>
                </tr>
	            <tr height="1">
	                <td colspan="2"><hr size="1" noshade /></td>
	            </tr>
            </table>
            </div>
        </td>
    </tr>
    <tr height="1">
        <td bgcolor="#c8cfdd" align="center">
            <asp:Label ID="lblFinish" runat="server" CssClass="default" Text="<img src='/admin/images/enabled.gif' border='0' align='absmiddle' /> Information has been saved" />
        </td>
        <td align="right" bgcolor="#c8cfdd">
            <asp:Button ID="btnClose" runat="server" Text="Close" Width="75" CssClass="default" />
        </td>
    </tr>
    <tr><td colspan="2" height="5" bgcolor="#c8cfdd"><img src="images/spacer.gif" width="1" height="5" /></td></tr>
</table>
<asp:Label ID="lblId" runat="server" Visible="false" />
</form>
</body>
</html>
