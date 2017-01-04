<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frame_user_roles.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.frame_user_roles" %>


<script type="text/javascript">
    function UpdateOrder(oList, oHidden) {
	    oHidden = document.getElementById(oHidden);
	    oHidden.value = "";
	    for (var ii=0; ii<oList.length; ii++) {
		    oHidden.value = oHidden.value + oList.options[ii].value + "&";
	    }
    }
    function MoveIn(ddlF, ddl, oHidden) {
	    if (ddlF.selectedIndex > 0) {
		    var oldIndex = ddlF.selectedIndex;
		    var oOption = document.createElement("OPTION");
		    ddl.add(oOption);
		    oOption.text = ddlF.options[ddlF.selectedIndex].text;
		    oOption.value = ddlF.options[ddlF.selectedIndex].value;
		    ddlF.remove(ddlF.selectedIndex);
		    ddl.selectedIndex = ddl.length - 1;
		    ddlF.selectedIndex = oldIndex - 1;
		    UpdateOrder(ddl, oHidden);
	    }
	    return false;
    }
    function MoveOut(ddlF, ddl, oHidden) {
	    if (ddlF.selectedIndex > -1) {
		    var oldIndex = ddlF.selectedIndex;
		    var oOption = document.createElement("OPTION");
		    ddl.add(oOption);
		    oOption.text = ddlF.options[ddlF.selectedIndex].text;
		    oOption.value = ddlF.options[ddlF.selectedIndex].value;
		    ddlF.remove(ddlF.selectedIndex);
		    ddl.selectedIndex = ddl.length - 1;
		    ddlF.selectedIndex = oldIndex - 1;
		    UpdateOrder(ddlF, oHidden);
	    }
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
        <td colspan="2">
            <table width="100%" border="0" cellSpacing="2" cellPadding="2" bgcolor="#e6e9f0">
	            <tr bgcolor="#e6e9f0">
		            <td><b>User Groups</b></td>
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
                    <td>User:</td>
                    <td width="100%"><asp:label ID="lblUser" CssClass="default" runat="server" /></td>
                </tr>
                <tr>
                    <td>Available:</td>
                    <td width="100%"><asp:DropDownList ID="ddlAvailable" runat="server" CssClass="default" /> <asp:Button ID="btnAdd" runat="server" CssClass="default" Width="75" Text="Add" /></td>
                </tr>
                <tr>
                    <td colspan="2"><asp:ListBox ID="lstCurrent" runat="server" Width="400" CssClass="default" Rows="10" /></td>
                </tr>
                <tr>
                    <td colspan="2" align="center"><asp:Button ID="btnRemove" runat="server" CssClass="default" Width="75" Text="Remove" /></td>
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
            <asp:Button ID="btnSave" runat="server" Text="Save" Width="75" CssClass="default" OnClick="btnSave_Click" /> 
            <asp:Button ID="btnClose" runat="server" Text="Close" Width="75" CssClass="default" />
        </td>
    </tr>
    <tr><td colspan="2" height="5" bgcolor="#c8cfdd"><img src="/admin/images/spacer.gif" width="1" height="5" /></td></tr>
</table>
<asp:Label ID="lblId" runat="server" Visible="false" />
<asp:Label ID="lblUserId" runat="server" Visible="false" />
<input type="hidden" runat="server" id="hdnRoles" />
</form>
</body>
</html>
