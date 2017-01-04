<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="audit_script_set_details.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.audit_script_set_details" %>


<html>
<head>
<title>ClearView Administration</title>
<link rel="stylesheet" type="text/css" href="/css/default.css" />
<script type="text/javascript" src="/javascript/global.js"></script>
<script type="text/javascript" src="/javascript/ajax.js"></script>
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
		    if (oOption.value != "0")
		    {
		        ddlF.remove(ddlF.selectedIndex);
		    }
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
		    <td colspan="2"><b>Available Scripts</b></td>
		</tr>
		<tr><td colspan="2">&nbsp;</td></tr>
        <tr>
            <td class="default"><img src="/images/spacer.gif" border="0" width="15" height="1" /></td>
            <td class="default" width="100%"><asp:DropDownList ID="ddlAvailable" runat="server" CssClass="default" /> <asp:Button ID="btnAdd" runat="server" CssClass="default" Width="75" Text="Add" /></td>
        </tr>
        <tr>
            <td colspan="2" class="default">&nbsp;</td>
        </tr>
        <tr>
            <td colspan="2" class="default"><hr size="1" noshade /></td>
        </tr>
        <tr>
            <td colspan="2" class="default">&nbsp;</td>
        </tr>
        <tr>
            <td colspan="2" class="default"><b>Current Scripts:</b></td>
        </tr>
        <tr>
            <td colspan="2" valign="top">
                <table cellpadding="0" cellspacing="0" border="0">
                    <tr>
                        <td class="default"><img src="/images/spacer.gif" border="0" width="15" height="1" /></td>
                        <td colspan="3"><asp:ListBox ID="lstCurrent" runat="server" Width="350" CssClass="default" Rows="10" /></td>
                    </tr>
                    <tr>
                        <td class="default"><img src="/images/spacer.gif" border="0" width="15" height="1" /></td>
                        <td class="default" align="right"><asp:ImageButton ID="imgOrderUp" runat="server" ImageUrl="/images/up.gif" ToolTip="Move Up"/></td>
                        <td class="default" align="center"><asp:ImageButton ID="imgRemove" runat="server" ImageUrl="/images/dl.gif" ToolTip="Remove" /></td>
                        <td class="default"><asp:ImageButton ID="imgOrderDown" runat="server" ImageUrl="/images/dn.gif" ToolTip="Move Down"/></td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr> 
            <td>&nbsp;</td>
            <td>
                <asp:button ID="btnSave" CssClass="default" runat="server" Text="Save" Width="75" OnClick="btnSave_Click" />
            </td>
        </tr>
    </table>
        </div>
        </td>
        </tr>
        </table>
    <input type="hidden" id="hdnParent" runat="server" />
    <input type="hidden" id="hdnOrder" runat="server" />
</form>
</body>
</html>
