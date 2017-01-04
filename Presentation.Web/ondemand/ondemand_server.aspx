<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ondemand_server.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.ondemand_server" %>

<html>
<head>
<title>Auto-Provisioning</title>
<link rel="stylesheet" type="text/css" href="/css/default.css" />
<script src="/javascript/default.js"type="text/javascript"></script>
<script src="/javascript/both.js"type="text/javascript"></script>
<script type="text/javascript">
	var oRedirectTimer = null;
	var oRedirectInterval = 3000;
	var oRedirectURL = null;
    function ShowHideResult(oDiv) {
        oDiv = document.getElementById(oDiv);
        if (oDiv.style.display == "inline")
            oDiv.style.display = "none";
        else
            oDiv.style.display = "inline";
    }
    
    var intAJAXStep = null;
    var intAJAXServer = null;
    function redirectAJAX(intServer, intStep) {
        intAJAXServer = intServer;
        intAJAXStep = intStep;
		oRedirectURL = "<%=Request.Url.PathAndQuery %>";
		clearTimeout(oRedirectTimer);
		oRedirectTimer = setTimeout("redirectAJAX2()",oRedirectInterval);
		DemoStep();
    }
	function redirectAJAX2() {
		clearTimeout(oRedirectTimer);
        CheckOnDemandStepServer(intAJAXServer, intAJAXStep);
		oRedirectTimer = setTimeout("redirectAJAX2()",oRedirectInterval);
	}
	function redirectAJAXGo() {
		window.location = oRedirectURL;
	}
	function ConfirmPreview(oCheck) {
	    if (oCheck.checked == true)
	        return confirm('WARNING! Depending on the hardware, a preview could take a few minutes to appear.\n\nDuring this time, it is recommended that you allow the loading process to complete.\n\nAre you sure you want to continue?');
	    else
	        return true;
	}
	function LoadError(oError) {
	    if (oError != "") {
	        //OpenWindow("PROVISIONING_ERROR", oError);
	    }
	}
	function DemoStep() {
	    var demo = "USE7355XA4";
	    var _output = document.getElementById("panInventoryYes");
	    if (_output.innerText.indexOf(demo) >= 0) {
	        // find object
	        DemoReplace(_output, "Create ADM Active Directory Group", "Allocate Storage");
	        //alert('done');
	    }
	}
	function DemoReplace(_obj, _find, _replace) {
	    if (_obj.childNodes.length <= 1 && _obj.innerText == _find) {
	        //alert(_obj.childNodes.length);
	        //alert(_obj.innerHTML);
	        //alert(_obj.innerText);
	        _obj.innerText = _obj.innerText.replace(_find, _replace);
	        //alert(_obj.innerText);
	    }
	    else {
	        for (var ii = 0; ii < _obj.childNodes.length; ii++) {
	            DemoReplace(_obj.childNodes[ii], _find, _replace);
	        }
	    }
	}
</script>
</head>
<body leftmargin="0" topmargin="0" onload="DemoStep();">
<form id="Form1" runat="server">
<table width="100%" height="100%" cellpadding="4" cellspacing="2" border="0">
    <tr height="1">
        <td colspan="2" align="center">
            <asp:Panel ID="panDone" runat="server" Visible="false">
            <table cellpadding="2">
                <tr>
                    <td><img src="/images/bigCheck.gif" border="0" align="absmiddle" /></td>
                    <td class="bigger"><b>The auto-provisioning process completed successfully!&nbsp;&nbsp;(<asp:Label ID="lblCompleted" runat="server" CssClass="bigger" />)</b></td>
                </tr>
            </table>
            </asp:Panel>
        </td>
    </tr>
    <tr id="panError" runat="server" visible="false">
        <td colspan="2" align="center" bgcolor="#EEEEEE" style="border: solid 1px #CCCCCC">
            <table cellpadding="3" cellspacing="5" border="0" style="border:solid 1px #999999" bgcolor="#f9f9f9">
                <tr>
                    <td rowspan="5" valign="top"><img src="/images/ico_error.gif" border="0" align="absmiddle" /></td>
                    <td class="header" valign="bottom">Oops!</td>
                </tr>
                <tr>
                    <td valign="top">There is a problem with this design.  Please submit an issue using the <b>Support</b> module referencing Design ID # <asp:Label ID="lblOops" runat="server" />.</td>
                </tr>
            </table>
        </td>
    </tr>
    <tr id="panInventoryNo" runat="server" visible="false">
        <td colspan="2" align="center" bgcolor="#EEEEEE" style="border: solid 1px #CCCCCC">
            <table cellpadding="3" cellspacing="5" border="0" style="border:solid 1px #999999" bgcolor="#f9f9f9">
                <tr>
                    <td rowspan="5" valign="top"><img src="/images/hugeAlert.gif" border="0" align="absmiddle" /></td>
                    <td class="header" valign="bottom">Out of Inventory</td>
                </tr>
                <tr>
                    <td valign="top">This model has been completely depleated.</td>
                </tr>
                <tr>
                    <td valign="top" class="note">Model: <asp:Label ID="lblInventory" runat="server" /></td>
                </tr>
                <tr>
                    <td valign="top">Please contact <u>YOUR</u> manager for more information.</td>
                </tr>
                <tr>
                    <td valign="top"><b>NOTE:</b> This is not a ClearView decision, issue or error.</td>
                </tr>
            </table>
        </td>
    </tr>
    <tr id="panInventoryYes" runat="server" visible="false">
        <td valign="top" width="50%">
            <%=strResult %>
        </td>
        <td valign="top" width="50%">
            <table cellpadding="2" cellspacing="2" border="0">
                <tr>
                    <td class="header">Preview of <%=strName %></td>
                </tr>
                <tr>
                    <td><input type="checkbox" id="chkPreview" runat="server" name="chkPreview" onclick="return ConfirmPreview(this) && document.forms[0].submit();" onserverchange="chkPreview_Change"><label for="chkPreview">Show Preview (Read-Only Mode)</label></td>
                </tr>
                <tr>
                    <td>
                        <table width="400" height="300" cellpadding="0" cellspacing="0" border="0">
                            <tr>
                                <td bgcolor="#000000" align="center" class="whiteheader">Preview Not Available</td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td><asp:LinkButton ID="btnDesign" runat="server" Text="<img src='/images/check.gif' border='0' align='absmiddle' /> Click here to view the original design" Visible="false" /></td>
                </tr>
            </table>
            <asp:Panel ID="panRebuilding" runat="server" Visible="false">
                <br />
                <table cellpadding="0" cellspacing="5" border="0" style="border:solid 1px #EE2C2C; background-color:#F7EFEE">
                    <tr>
                        <td><b>NOTE:</b> This asset is currently being rebuilt...</td>
                    </tr>
                </table>
            </asp:Panel>
        </td>
    </tr>
</table>
<asp:PlaceHolder ID="PH" runat="server" />
<div id="divLine" style="BORDER-RIGHT: gray 1px dashed; DISPLAY: none; WIDTH: 2px; POSITION: absolute; TOP: 58px; HEIGHT: 100%"></div>
<div id="divLineCover" style="DISPLAY: none; FILTER: alpha(opacity=25); WIDTH: 100%; CURSOR: e-resize; POSITION: absolute; TOP: 58px; HEIGHT: 100%; BACKGROUND-COLOR: #666666"></div>
<div id="divLiveCover" style="DISPLAY:none; WIDTH:100%; POSITION:absolute; HEIGHT:100%; FILTER:alpha(opacity=25); BACKGROUND-COLOR:#333333"	onclick="window.top.HidePanel();" oncontextmenu="return false;"></div>
<iframe id="frmLiveCover" frameborder="0" scrolling="no" style="z-index:400; display:none;position:absolute;FILTER:alpha(opacity=0)" src="javascript:''"></iframe>
<iframe id="frmLiveShow" frameborder="0" scrolling="no" style="z-index:401; display:none;position:absolute;" src="javascript:''"></iframe>
</form>
</body>
</html>