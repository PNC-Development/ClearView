<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frame_file_browser.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.frame_file_browser" %>

<script type="text/javascript">
    function Select(strValue, oHidden, oName) {
        oHidden = document.getElementById(oHidden);
        oName = document.getElementById(oName);
        oHidden.value = strValue;
        oName.innerText = strValue;
    }
    function Update(oHidden, strControl) {
        oHidden = document.getElementById(oHidden);
        window.top.UpdateWindow(oHidden.value, strControl, null);
        window.top.HidePanel();
        return false;
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
<table width="100%" height="100%" border="0" cellSpacing="0" cellPadding="0">
    <tr height="1">
        <td>
            <table width="100%" border="0" cellSpacing="2" cellPadding="2" bgcolor="#e6e9f0">
	            <tr bgcolor="#e6e9f0">
		            <td><b>File Browser</b></td>
	                <td align="right">
			            <a href="javascript:void(0);" onclick="parent.HidePanel();"><img src="/admin/images/close.gif" border="0" title="Close"></a>
	                </td>
	            </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td>
            <table width="100%" height="100%" border="0" cellSpacing="2" cellPadding="2" bgcolor="#c8cfdd">
	            <tr height="1">
	                <td class="bold">File:</td>
	                <td width="100%"><asp:TextBox ID="txtName" runat="server" CssClass="default" Width="100%" /></td>
                </tr>
	            <tr>
	                <td colspan="2" valign="top" class="default">
	                    <div style="height:100%; width:100%; overflow:auto; background-color:#FFFFFF">
                        <asp:TreeView ID="oTreeview" runat="server" ShowLines="true" NodeIndent="10">
                            <NodeStyle CssClass="default" />
                        </asp:TreeView>
                        </div>
	                </td>
	            </tr>
	            <tr height="1">
	                <td colspan="2"><hr size="1" noshade /></td>
	            </tr>
                <tr height="1">
                    <td colspan="2" class="default" align="right"><asp:Label ID="lblPage" runat="server" CssClass="default" /></td>
                </tr>
            </table>
        </td>
    </tr>
    <tr height="1">
        <td align="right" bgcolor="#c8cfdd">
            <asp:Button ID="btnSave" runat="server" Text="Save" Width="75" CssClass="default" /> 
            <asp:Button ID="btnClose" runat="server" Text="Close" Width="75" CssClass="default" />
        </td>
    </tr>
    <tr><td height="5" bgcolor="#c8cfdd"><img src="images/spacer.gif" width="1" height="5" /></td></tr>
</table>
<input type="hidden" runat="server" id="hdnId" />
</form>
</body>
</html>


