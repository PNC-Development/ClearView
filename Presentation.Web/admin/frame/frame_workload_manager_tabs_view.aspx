<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frame_workload_manager_tabs_view.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.frame_workload_manager_tabs_view" %>

<script type="text/javascript">
    function MoveOrderUp(oList, oHidden) {
	    if (oList.selectedIndex > -1) {
		    var oldIndex = oList.selectedIndex - 1;
		    if (oldIndex > -1) {

			    var oldValue = oList.options[oList.selectedIndex].value;
			    var oldText = oList.options[oList.selectedIndex].text;
			    oList.options[oList.selectedIndex].value = oList.options[oList.selectedIndex - 1].value;
			    oList.options[oList.selectedIndex].text = oList.options[oList.selectedIndex - 1].text;
			    oList.options[oList.selectedIndex - 1].value = oldValue;
			    oList.options[oList.selectedIndex - 1].text = oldText;
			    oList.selectedIndex = oldIndex;
			    UpdateOrder(oList, oHidden);
		    }
	    }
	    return false;
    }
    function MoveOrderDown(oList, oHidden) {
	    if (oList.selectedIndex > -1) {
		    var oldIndex = oList.selectedIndex + 1;
		    if (oldIndex < oList.length) {
			    var oldValue = oList.options[oList.selectedIndex].value;
			    var oldText = oList.options[oList.selectedIndex].text;
			    oList.options[oList.selectedIndex].value = oList.options[oList.selectedIndex + 1].value;
			    oList.options[oList.selectedIndex].text = oList.options[oList.selectedIndex + 1].text;
			    oList.options[oList.selectedIndex + 1].value = oldValue;
			    oList.options[oList.selectedIndex + 1].text = oldText;
			    oList.selectedIndex = oldIndex;
			    UpdateOrder(oList, oHidden);
		    }
	    }
	    return false;
    }
    function UpdateOrder(oList, oHidden) {
	    oHidden = document.getElementById(oHidden);
	    oHidden.value = "";
	    for (var ii=0; ii<oList.length; ii++)
		    oHidden.value = oHidden.value + oList.options[ii].value + "&";
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
    function Update(oHidden, strControl) {
        oHidden = document.getElementById(oHidden);
        window.top.UpdateWindow(oHidden.value, strControl, null, null);
        window.top.HidePanel();
        return false;
    }
</script>
<html>
<head>
<title>ClearView Administration</title>
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
		            <td><b>Request Item Tabs</b></td>
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
                    <td nowrap class="default class="bold">Tabs:</td>
                    <td width="100%"><asp:DropDownList ID="ddlAvailable" runat="server" CssClass="default" Width="200" /> <asp:Button ID="btnAdd" runat="server" CssClass="default" Width="75" Text="Add" /></td>
                </tr>
                <tr>
                    <td colspan="2"><b>Current Tabs:</b></td>
                </tr>
                <tr>
                    <td colspan="2" valign="top" class="default">
                        <asp:ListBox ID="lstCurrent" runat="server" Width="320" Rows="20" CssClass="default" />
                        <div align="center" style="width:320px">
                            <asp:ImageButton ID="imgOrderUp" runat="server" ImageUrl="/images/up.gif" ToolTip="Move Up"/>
                            &nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:ImageButton ID="imgRemove" runat="server" ImageUrl="/images/dl.gif" ToolTip="Remove" />
                            &nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:ImageButton ID="imgOrderDown" runat="server" ImageUrl="/images/dn.gif" ToolTip="Move Down"/>
                        </div>
                    </td>
                </tr>
                <tr height="1">
                    <td colspan="2"><hr size="1" noshade /></td>
                </tr>
                <tr height="1">
                    <td colspan="2" align="right" bgcolor="#c8cfdd">
                        <asp:Button ID="btnSave" runat="server" Text="Save" Width="75" CssClass="default" OnClick="btnSave_Click" /> 
                        <asp:Button ID="btnClose" runat="server" Text="Close" Width="75" CssClass="default" />
                    </td>
                </tr>
                <tr><td colspan="2" height="5" bgcolor="#c8cfdd"><img src="images/spacer.gif" width="1" height="5" /></td></tr>
            </table>
        </td>
    </tr>
</table>    
<input type="hidden" runat="server" id="hdnOrder" />
</form>
</body>
</html>
