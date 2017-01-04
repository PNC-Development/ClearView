<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frame_controls.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.frame_controls" %>

<script type="text/javascript">
function MoveLeft(ddlFrom, ddlTo) {
    ddlFrom = Get(ddlFrom);
    ddlTo = Get(ddlTo);
	if (ddlFrom.selectedIndex > -1) {
		var oOption = document.createElement("OPTION");
		ddlTo.add(oOption);
		oOption.text = ddlFrom.options[ddlFrom.selectedIndex].text;
		oOption.value = ddlFrom.options[ddlFrom.selectedIndex].value;
		ddlFrom.remove(ddlFrom.selectedIndex);
		ddlTo.selectedIndex = ddlTo.length - 1;
		UpdateHiddenField_helper(ddlFrom);
		UpdateHiddenField_helper(ddlTo);
	}
	return false;
}
function MoveUp(ddl) {
    ddl = Get(ddl);
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
			UpdateHiddenField_helper(ddl);
		}
	}
	return false;
}
function MoveOut(ddl) {
    ddl = Get(ddl);
	if (ddl.selectedIndex > -1) {
		var oldIndex = ddl.selectedIndex;
		RemoveHiddenField_helper(ddl, oldIndex);
		ddl.remove(ddl.selectedIndex);
		ddl.selectedIndex = oldIndex + 1;
		UpdateHiddenField_helper(ddl);
	}
	return false;
}
function MoveDown(ddl) {
    ddl = Get(ddl);
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
			UpdateHiddenField_helper(ddl);
		}
	}
	return false;
}
function MoveRight(ddlFrom, ddlTo) {
    ddlFrom = Get(ddlFrom);
    ddlTo = Get(ddlTo);
	if (ddlFrom.selectedIndex > -1) {
		var oOption = document.createElement("OPTION");
		ddlTo.add(oOption);
		oOption.text = ddlFrom.options[ddlFrom.selectedIndex].text;
		oOption.value = ddlFrom.options[ddlFrom.selectedIndex].value;
		ddlFrom.remove(ddlFrom.selectedIndex);
		ddlTo.selectedIndex = ddlTo.length - 1;
		UpdateHiddenField_helper(ddlFrom);
		UpdateHiddenField_helper(ddlTo);
	}
	return false;
}
function addControl(ddlControl, oDDL) {
	if (ddlControl.selectedIndex == 0) {
		alert('Please select a control to add');
		ddlControl.focus();
	}
	else {
        oDDL = Get(oDDL);
		var PHx = oDDL.id;
		PHx = PHx.substring(PHx.indexOf("PH"));
		var oOption = document.createElement("OPTION");
		oDDL.add(oOption);
		oOption.text = ddlControl.options[ddlControl.selectedIndex].text;
		oOption.value = "c" + ddlControl.options[ddlControl.selectedIndex].value;
		oDDL.selectedIndex = oDDL.length - 1;
		UpdateHiddenField(oDDL, "hdn" + PHx);
	}
	return false;
}
function UpdateHiddenField_helper(oDDL) {
	var PHx = oDDL.id;
	PHx = PHx.substring(PHx.indexOf("PH"));
	UpdateHiddenField(oDDL, "hdn" + PHx);
}
function UpdateHiddenField(oControl, strPh) {
	var oHidden = document.getElementById(strPh);
	oHidden.value = "";
	for (var ii=0; ii<oControl.length; ii++) {
		oHidden.value = oHidden.value + oControl.options[ii].value + "_" + ii + "&";
	}
}
function RemoveHiddenField_helper(oDDL, ii) {
	var PHx = oDDL.id;
	PHx = PHx.substring(PHx.indexOf("PH"));
	RemoveHiddenField(oDDL, "hdn" + PHx + "del", ii);
}
function RemoveHiddenField(oControl, strPh, ii) {
	var oHidden = document.getElementById(strPh);
	oHidden.value = oHidden.value + oControl.options[ii].value + "&";
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
		            <td><b>Manage Controls</b></td>
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
	            <tr height="1">
		            <td class="default">Available Controls: <asp:DropDownList ID="ddlControl" runat="server" CssClass="default" Width="300" /> </td>
	            </tr>
	            <tr height="1">
	                <td><hr size="1" noshade /></td>
	            </tr>
	            <tr>
	                <td valign="top">
	                    <asp:Table ID="tblControls" runat="server" CellPadding="4" CellSpacing="2" CssClass="default" />
	                </td>
	            </tr>
            </table>
            </div>
        </td>
    </tr>
    <tr height="1">
        <td align="right" bgcolor="#c8cfdd">
            <asp:Button ID="btnSave" runat="server" Text="Save" Width="75" CssClass="default" OnClick="btnSave_Click" /> 
            <asp:Button ID="btnClose" runat="server" Text="Close" Width="75" CssClass="default" />
        </td>
    </tr>
    <tr><td height="5" bgcolor="#c8cfdd"><img src="images/spacer.gif" width="1" height="5" /></td></tr>
</table>
<asp:Label ID="lblId" runat="server" Visible="false" />
<asp:Label ID="lblType" runat="server" Visible="false" />
<input id="hdnPH1" name="hdnPH1" type="hidden"/>
<input id="hdnPH2" name="hdnPH2" type="hidden"/>
<input id="hdnPH3" name="hdnPH3" type="hidden"/>
<input id="hdnPH4" name="hdnPH4" type="hidden"/>
<input id="hdnPH5" name="hdnPH5" type="hidden"/>
<input id="hdnPH6" name="hdnPH6" type="hidden"/>
<input id="hdnPH7" name="hdnPH7" type="hidden"/>
<input id="hdnPH8" name="hdnPH8" type="hidden"/>
<input id="hdnPH9" name="hdnPH9" type="hidden"/>
<input id="hdnPH10" name="hdnPH10" type="hidden"/>
<input id="hdnPH1del" name="hdnPH1del" type="hidden"/>
<input id="hdnPH2del" name="hdnPH2del" type="hidden"/>
<input id="hdnPH3del" name="hdnPH3del" type="hidden"/>
<input id="hdnPH4del" name="hdnPH4del" type="hidden"/>
<input id="hdnPH5del" name="hdnPH5del" type="hidden"/>
<input id="hdnPH6del" name="hdnPH6del" type="hidden"/>
<input id="hdnPH7del" name="hdnPH7del" type="hidden"/>
<input id="hdnPH8del" name="hdnPH8del" type="hidden"/>
<input id="hdnPH9del" name="hdnPH9del" type="hidden"/>
<input id="hdnPH10del" name="hdnPH10del" type="hidden"/>
</form>
</body>
</html>
