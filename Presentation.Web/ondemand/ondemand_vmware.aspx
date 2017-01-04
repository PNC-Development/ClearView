<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ondemand_vmware.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.ondemand_vmware" %>

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
        if (<%=intShowBuild %> == 1) {
            frmPreview = document.getElementById("frmPreview");
            frmPreview.navigate("/frame/loading_preview.htm?referrer=/ondemand/ondemand_vmware_preview.aspx?name=<%=strPreviewName %>&token=<%=strPreviewToken %>");
        }
        else
            VMWareNoPreview();

        intAJAXServer = intServer;
        intAJAXStep = intStep;
		oRedirectURL = "<%=Request.Url.PathAndQuery %>";
		clearTimeout(oRedirectTimer);
		oRedirectTimer = setTimeout("redirectAJAX2()",oRedirectInterval);
	}
function DHTMLHelp2(oMainTool, oMainFrame) {
    oMainTool.style.zIndex = 100;
    oMainFrame.style.width = oMainTool.offsetWidth;
    oMainFrame.style.height = parseInt(oMainTool.offsetHeight);
    oMainFrame.style.top = oMainTool.style.top;
    oMainFrame.style.left = oMainTool.style.left;
    oMainFrame.style.zIndex = oMainTool.style.zIndex - 1;
    oMainFrame.style.display = "inline";
}
	function redirectAJAX2() {
		clearTimeout(oRedirectTimer);
        CheckOnDemandStepServer(intAJAXServer, intAJAXStep);
		oRedirectTimer = setTimeout("redirectAJAX2()",oRedirectInterval);
	}
	function redirectAJAXGo() {
		window.location = oRedirectURL;
	}
	function VMWarePreviewResize(strWidth, strHeight) {
        var frmResize = document.getElementById("frmPreview");
        frmResize.width = strWidth;
        frmResize.height = strHeight;
	}
	function VMWareNoPreview() {
        var divYes = document.getElementById("divPreviewYes");
        divYes.style.display = "none";
        var divNo = document.getElementById("divPreviewNo");
        divNo.style.display = "inline";
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
</script>
</head>
<body leftmargin="0" topmargin="0">
<form id="Form1" runat="server">
<table width="100%" cellpadding="4" cellspacing="2" border="0">
    <tr>
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
                        <div id="divPreviewYes" style="display:inline">
                        <table cellpadding="0" cellspacing="0" border="0">
                            <tr>
                                <td>
                                    <iframe style="z-index:600" id="frmPreview" name="frmPreview" frameborder="0" scrolling="auto" width="400" height="300"></iframe>
                                </td>
                            </tr>
                        </table>
                        </div>
                        <div id="divPreviewNo" style="display:none">
                            <table width="400" height="300" cellpadding="0" cellspacing="0" border="0">
                                <tr>
                                    <td bgcolor="#000000" align="center" class="whiteheader">Preview Not Available</td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td><asp:LinkButton ID="btnDesign" runat="server" Text="<img src='/images/check.gif' border='0' align='absmiddle' /> Click here to view the original design" Visible="false" /></td>
                </tr>
            </table>
        </td>
    </tr>
</table>
<asp:PlaceHolder ID="PH" runat="server" />
    <div id="divCover" style="DISPLAY:none; WIDTH:100%; POSITION:absolute; HEIGHT:100%; FILTER:alpha(opacity=25); BACKGROUND-COLOR:#333333"	onclick="alert('click');return false;" oncontextmenu="alert('click');return false;">
    <iframe id="frmCover" frameborder="0" scrolling="no" style="z-index:400; display:none;position:absolute;FILTER:progid:DXImageTransform.Microsoft.Alpha(style=0,opacity=0)" allowtransparency="true" src="javascript:''"></iframe>
    </div>
</form>
</body>
</html>