<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frame_service_details.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.frame_service_details" %>

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
		            <td><b><asp:Label ID="lblName" runat="server" CssClass="default" /> Details</b></td>
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
                <asp:Panel ID="panAll" runat="server" Visible="false">
                <asp:TreeView ID="oTree" runat="server" CssClass="default" ShowLines="true" NodeIndent="30">
                </asp:TreeView>
                </asp:Panel>
                <asp:Panel ID="panEdit" runat="server" Visible="false">
                    <table cellpadding="4" cellspacing="3" border="0">
                        <tr>
                            <td>Name:</td>
                            <td><asp:TextBox ID="txtName" runat="server" CssClass="default" Width="200" MaxLength="300" /></td>
                        </tr>
                        <tr>
                            <td>Hours:</td>
                            <td><asp:TextBox ID="txtHours" runat="server" CssClass="default" Width="50" MaxLength="8" /> HRs</td>
                        </tr>
                        <tr>
                            <td>Additonal:</td>
                            <td><asp:TextBox ID="txtAdditional" runat="server" CssClass="default" Width="50" MaxLength="8" /> HRs</td>
                        </tr>
                        <tr>
                            <td>WM Checkbox:</td>
                            <td><asp:CheckBox ID="chkCheck" runat="server" CssClass="default" /></td>
                        </tr>
                        <tr>
                            <td>Enabled:</td>
                            <td><asp:CheckBox ID="chkEnabled" runat="server" CssClass="default" /></td>
                        </tr>
                        <tr>
                            <td></td>
                            <td>
                                <asp:Button ID="btnSave" runat="server" CssClass="default" Width="75" Text="Save" OnClick="btnSave_Click" /> 
                                <asp:Button ID="btnCancel" runat="server" CssClass="default" Width="75" Text="Cancel" OnClick="btnCancel_Click" /> 
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </div>
        </td>
    </tr>
    <tr><td colspan="2" height="5" bgcolor="#c8cfdd"><img src="images/spacer.gif" width="1" height="5" /></td></tr>
</table>
<asp:Label ID="lblApplication" runat="server" Visible="false" />
<input type="hidden" runat="server" id="hdnId" />
</form>
</body>
</html>
