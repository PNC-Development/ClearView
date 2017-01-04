<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="permissions.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.permissions" %>

<script type="text/javascript">
    function Add(strId) {
        OpenWindow('PERMISSIONS','','0&applicationid=' + strId,false,450,200);
    }
    function Edit(strId) {
        OpenWindow('PERMISSIONS','',strId,false,450,200);
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
        <td height="100%" valign="top">
<table width="98%" cellpadding="0" cellspacing="0" border="0" align="center">
		<tr><td colspan="2">&nbsp;</td></tr>
		<tr> 
		    <td><b>Permissions</b></td>
		    <td align="right"><asp:LinkButton ID="btnExpand" runat="server" OnClick="btnExpand_Click" /></td>
		</tr>
		<tr><td colspan="2">&nbsp;</td></tr>
		<tr>
		    <td colspan="2" align="center">
                <table width="100%" border="0" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <asp:TreeView ID="oTreeview" runat="server" ShowLines="true" NodeIndent="35">
                                <NodeStyle CssClass="default" />
                            </asp:TreeView>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
        </td>
        </tr>
        </table>
</form>
</body>
</html>
