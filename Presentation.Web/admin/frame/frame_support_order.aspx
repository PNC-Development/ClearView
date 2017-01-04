<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frame_support_order.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.frame_support_order" %>

<script type="text/javascript">
    function MoveOrderUp(ddl) {
	    if (ddl.selectedIndex > -1) {
		    var oldIndex = ddl.selectedIndex - 1;
		    if (oldIndex > -1) {

			    var oldValue = ddl.options[ddl.selectedIndex].value;
			    var oldText = ddl.options[ddl.selectedIndex].text;
			    ddl.options[ddl.selectedIndex].value = ddl.options[ddl.selectedIndex - 1].value;
			    ddl.options[ddl.selectedIndex].text = ddl.options[ddl.selectedIndex - 1].text;
			    ddl.options[ddl.selectedIndex - 1].value = oldValue;
			    ddl.options[ddl.selectedIndex - 1].text = oldText;
			    ddl.selectedIndex = oldIndex;
			    UpdateOrder(ddl);
		    }
		    else
			    alert('You cannot move this item');
	    }
	    else
		    alert('Please select an item to move');
	    return false;
    }
    function MoveOrderDown(ddl) {
	    if (ddl.selectedIndex > -1) {
		    var oldIndex = ddl.selectedIndex + 1;
		    if (oldIndex < ddl.length) {
			    var oldValue = ddl.options[ddl.selectedIndex].value;
			    var oldText = ddl.options[ddl.selectedIndex].text;
			    ddl.options[ddl.selectedIndex].value = ddl.options[ddl.selectedIndex + 1].value;
			    ddl.options[ddl.selectedIndex].text = ddl.options[ddl.selectedIndex + 1].text;
			    ddl.options[ddl.selectedIndex + 1].value = oldValue;
			    ddl.options[ddl.selectedIndex + 1].text = oldText;
			    ddl.selectedIndex = oldIndex;
			    UpdateOrder(ddl);
		    }
		    else
			    alert('You cannot move this item');
	    }
	    else
		    alert('Please select an item to move');
	    return false;
    }
    function UpdateOrder(oDDL) {
	    var oHidden = document.getElementById("hdnUpdateOrder");
	    oHidden.value = "";
	    for (var ii=0; ii<oDDL.length; ii++) {
		    oHidden.value = oHidden.value + oDDL.options[ii].value + "&";
	    }
    }
    function Update(oHidden, strControl) {
        oHidden = document.getElementById(oHidden);
        window.top.UpdateWindow(oHidden.value, strControl, null, null);
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
		            <td><b>Order</b></td>
	                <td align="right">
			            <a href="javascript:void(0);" onclick="parent.HidePanel();"><img src="/admin/images/close.gif" border="0" title="Close"></a>
	                </td>
	            </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td>
            <div style="height:100%; width:100%; overflow:auto;">
            <table width="100%" height="100%" border="0" cellSpacing="2" cellPadding="2" bgcolor="#c8cfdd">
	            <tr>
	                <td valign="top" class="default">
	                    <asp:ListBox ID="lstOrder" runat="server" Width="370" CssClass="default" Rows="20"/>
			            <div align="center" style="width:370px">
			            <asp:ImageButton ID="imgOrderUp" runat="server" ImageUrl="/admin/images/up.gif" ToolTip="Move Page Up"/>
			            &nbsp;&nbsp;&nbsp;&nbsp;
			            <asp:ImageButton ID="imgOrderDown" runat="server" ImageUrl="/admin/images/dn.gif" ToolTip="Move Page Down"/>
			            </div>
	                </td>
	            </tr>
	            <tr height="1">
	                <td><hr size="1" noshade /></td>
	            </tr>
            </table>
            </div>
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
<asp:Label ID="lblId" runat="server" Visible="false" />
<asp:Label ID="lblType" runat="server" Visible="false" />
<input type="hidden" runat="server" id="hdnId" />
<input id="hdnUpdateOrder" name="hdnUpdateOrder" type="hidden"/>
</form>
</body>
</html>
