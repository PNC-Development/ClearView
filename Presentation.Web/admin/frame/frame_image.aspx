<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frame_image.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.frame_image" %>

<script language="javascript">
var intZoom;
var strFile;
var strType;
function loadPage(strStartFile, strStartType) {
	strFile = strStartFile;
	strType = strStartType;
    intZoom = 100;
    if (strFile == "")
        frmImage.imgPreview.style.display = "none";
    else {
        frmImage.imgPreview.src = strFile;
        frmImage.imgPreview.style.display = "inline";
    }
	if (frmImage.btnInsert.value == "Insert") {
	    frmImage.btnInsert.disabled = true;
	    frmImage.btnDelete.disabled = true;
    }
    else {
	    ModifyImage('alt', frmImage.txtAlternative.value);
	    ModifyImage('border', frmImage.txtBorder.value);
	    ModifyImage('hspace', frmImage.txtHorizontal.value);
	    ModifyImage('vspace', frmImage.txtVertical.value);
    }	
}
function ImageClick(strName) {
    intZoom = 100;
    frmImage.btnInsert.disabled = false;
    frmImage.btnDelete.disabled = false;
    frmImage.imgPreview.style.zoom = intZoom + '%';
    strFile = strName;
    frmImage.imgPreview.src = strFile;
    frmImage.imgPreview.style.display = "inline";
}
function FolderClick(strName) {
}
function btnInsert_Click(strControl) {
    var oFrame = window.top.GetFrame(0);
    if (strControl == "") {
        if (strType == "INSERT")
        {
            if (oFrame != null && window.top.opener == null)
                oFrame.InsertImage("<img alt=\"" + frmImage.txtAlternative.value + "\" hspace=\"" + frmImage.txtHorizontal.value + "\" src=\"" + strFile + "\" align=\"" + frmImage.ddlAlignment.options[frmImage.ddlAlignment.selectedIndex].value + "\" vspace=\"" + frmImage.txtVertical.value + "\" border=\"" + frmImage.txtBorder.value + "\"/>");
            else
                window.top.InsertImage("<img alt=\"" + frmImage.txtAlternative.value + "\" hspace=\"" + frmImage.txtHorizontal.value + "\" src=\"" + strFile + "\" align=\"" + frmImage.ddlAlignment.options[frmImage.ddlAlignment.selectedIndex].value + "\" vspace=\"" + frmImage.txtVertical.value + "\" border=\"" + frmImage.txtBorder.value + "\"/>");
        }
        if (strType == "UPDATE")
        {
            if (oFrame != null && window.top.opener == null)
                oFrame.UpdateImage("<img alt=\"" + frmImage.txtAlternative.value + "\" hspace=\"" + frmImage.txtHorizontal.value + "\" src=\"" + strFile + "\" align=\"" + frmImage.ddlAlignment.options[frmImage.ddlAlignment.selectedIndex].value + "\" vspace=\"" + frmImage.txtVertical.value + "\" border=\"" + frmImage.txtBorder.value + "\"/>");
            else
                window.top.UpdateImage("<img alt=\"" + frmImage.txtAlternative.value + "\" hspace=\"" + frmImage.txtHorizontal.value + "\" src=\"" + strFile + "\" align=\"" + frmImage.ddlAlignment.options[frmImage.ddlAlignment.selectedIndex].value + "\" vspace=\"" + frmImage.txtVertical.value + "\" border=\"" + frmImage.txtBorder.value + "\"/>");
        }
    }
    else
        window.top.UpdateWindow(strFile, strControl, null);
    window.top.HidePanel();
    return false;
}
function btnDelete_Click() {
    if (confirm('Are you sure you want to delete this file?') == true) {
        frmImage.hdnFile.value = strFile;
        frmImage.submit();
    }
}
function btnIn_Click() {
    if (strFile != null) {
	    intZoom = intZoom + (intZoom / 2);
	    frmImage.imgPreview.style.zoom = parseInt(intZoom) + '%';
	    divAlt.style.display = "none";
	    btnShowHideAlt.innerText = "Show";
	}
}
function btnOut_Click() {
    if (strFile != null) {
	    intZoom = intZoom - (intZoom / 3);
	    frmImage.imgPreview.style.zoom = intZoom + '%';
	    divAlt.style.display = "none";
	    btnShowHideAlt.innerText = "Show";
	}
}
function ModifyImage(strAttribute, strValue) {
	if (strAttribute == "alt") {
		if (strValue == "") {
			divAlt.style.display = "none";
			btnShowHideAlt.style.display = "none";
			
		}
		else {
			divAlt.style.posTop = 100;
			divAlt.style.posLeft = 100;
			spnAlt.innerText = strValue;
			divAlt.style.display = "inline";
			btnShowHideAlt.style.display = "inline";
		}
	}
	else {
		frmImage.imgPreview.setAttribute(strAttribute, strValue);
	}
}
function ShowHideAlt() {
	if (btnShowHideAlt.innerText == "Hide") {
		divAlt.style.display = "none";
		btnShowHideAlt.innerText = "Show";
	}
	else {
		divAlt.style.display = "inline";
		btnShowHideAlt.innerText = "Hide";
	}
}
</script>
<html>
<head>
<title>Web Content Management Administration</title>
<link rel="stylesheet" type="text/css" href="/css/default.css" />
<script type="text/javascript" src="/javascript/global.js"></script>
</head>
<body topmargin="0" leftmargin="0" onload="loadPage('<%=Request.QueryString["src"] %>','<%=Request.QueryString["type"] %>');">
<form id="frmImage" runat="server">
<table width="100%" height="100%" border="0" cellSpacing="0" cellPadding="0">
    <tr height="1">
        <td>
            <table width="100%" border="0" cellSpacing="2" cellPadding="2" bgcolor="#e6e9f0">
	            <tr bgcolor="#e6e9f0">
		            <td><b>Image Browser</b></td>
	                <td align="right">
			            <a href="javascript:void(0);" onclick="window.top.HidePanel();"><img src="/admin/images/close.gif" border="0" title="Close"></a>
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
	                    <table width="100%" border="0" cellpadding="2" cellspacing="0">
	                        <tr>
                                <td valign="top">
		                            <div style="width:200px; height:275px; overflow:auto; background-color:#FFFFFF">
                                    <asp:TreeView ID="oTreeview" runat="server" NodeIndent="10" ExpandImageUrl="/admin/images/navfolder.gif" CollapseImageUrl="/admin/images/navfolderopen.gif">
                                        <NodeStyle CssClass="default" />
                                    </asp:TreeView>
                                    </div>
	                            </td>
                                <td width="375" align="center" valign="middle">
            	                    <table width="100%" border="0" cellpadding="2" cellspacing="0">
            	                    <tr>
            	                        <td align="center" class="default">Zoom: <a href="javascript:void(0)" onclick="btnIn_Click()"><img src="/admin/images/zoomin.gif" border="0" align="absmiddle" /></a> <a href="javascript:void(0)" onclick="btnOut_Click()"><img src="/admin/images/zoomout.gif" border="0" align="absmiddle" /></a></td>
            	                    </tr>
            	                    <tr>
            	                        <td align="center">
                                    <div style="width: 100%; height: 250px; overflow:auto;">
		                            <table>
			                            <tr>
				                            <td style="border:dotted 1px #666666" align="center">
					                            <img id="imgPreview" style="display:none">
					                            <div id="divAlt" style="position:absolute;display:none;background-Color:#FFFFDD;border:solid 1px black">
						                            <table cellpadding=1 cellspacing=1><tr><td nowrap><span id="spnAlt" class="default"></span></td></tr></table>
					                            </div>
				                            </td>
			                            </tr>
		                            </table>
		                            </div>
            	                        </td>
            	                    </tr>
            	                    </table>
                                    
	                            </td>
	                        </tr>
	                    </table>	                    
	                    <table width="100%" border="0" cellpadding="2" cellspacing="0">
                            <tr>
                                <td align="center">
            	                    <table width="100%" border="0" cellpadding="2" cellspacing="0">
                                        <tr>
                                            <td colspan="2" class="bold">Layout</td>
                                        </tr>
                                        <tr>
                                            <td class="default">Alignment:</td>
                                            <td>
                                                <asp:DropDownList ID="ddlAlignment" runat="server" CssClass="default">
                                                    <asp:ListItem Value="" Text="--None--"/>
                                                    <asp:ListItem Value="absbottom" Text="absbottom"/>
                                                    <asp:ListItem Value="absmiddle" Text="absmiddle"/>
                                                    <asp:ListItem Value="baseline" Text="baseline"/>
                                                    <asp:ListItem Value="bottom" Text="bottom"/>
                                                    <asp:ListItem Value="left" Text="left"/>
                                                    <asp:ListItem Value="middle" Text="middle"/>
                                                    <asp:ListItem Value="right" Text="right"/>
                                                    <asp:ListItem Value="texttop" Text="texttop"/>
                                                    <asp:ListItem Value="top" Text="top"/>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="default">Border:</td>
                                            <td><input type=text id="txtBorder" class="default" value="<%=(Request.QueryString["border"] == null ? "0" : Request.QueryString["border"].ToString())%>" style="width:50px" onkeyup="ModifyImage('border',this.value);"></td>
                                        </tr>
                                    </table>
                                </td>
                                <td>&nbsp;</td>
                                <td align="center">
            	                    <table width="100%" border="0" cellpadding="2" cellspacing="0">
                                        <tr>
                                            <td colspan="2" class="bold">Spacing</td>
                                        </tr>
                                        <tr>
                                            <td class="default">Horizontal:</td>
                                            <td><input type=text id="txtHorizontal" class="default" value="<%=(Request.QueryString["hspace"] == null ? "0" : Request.QueryString["hspace"].ToString())%>" style="width:50px" onkeyup="ModifyImage('hspace',this.value);"></td>
                                        </tr>
                                        <tr>
                                            <td class="default">Vertical:</td>
                                            <td><input type=text id="txtVertical" class="default" value="<%=(Request.QueryString["vspace"] == null ? "0" : Request.QueryString["vspace"].ToString())%>" style="width:50px" onkeyup="ModifyImage('vspace',this.value);"></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
	                    <table width="100%" border="0" cellpadding="2" cellspacing="0">
	                        <tr>
		                        <td colspan="2" class="bold">Alternative Text:</td>
		                    </tr>
		                    <tr>
		                        <td width="100%"><input type=text id="txtAlternative" class="default" value="<%=(Request.QueryString["alt"] == null ? "" : Request.QueryString["alt"].ToString())%>" style="width:100%" onkeyup="ModifyImage('alt',this.value);"></td>
		                        <td><a href="javascript:void(0);" class="cmlink" onclick="ShowHideAlt()" id="btnShowHideAlt">Hide</a></td>
	                        </tr>
	                    </table>
	                    <hr size="1" noshade />
	                    <table width="100%" border="0" cellpadding="2" cellspacing="0">
	                        <tr>
	                            <td class="bold">Upload Image:</td>
	                        </tr>
	                        <tr>
	                            <td><asp:FileUpload runat="server" ID="oUpload" Width="100%" CssClass="default" /></td>
	                        </tr>
	                        <tr>
	                            <td><asp:Button ID="btnUpload" runat="server" CssClass="default" Text="Upload" Width="75" OnClick="btnUpload_Click" /></td>
	                        </tr>
                        </table>
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
            <asp:Button ID="btnInsert" runat="server" Text="Select" Width="75" CssClass="default" />
            <asp:Button ID="btnDelete" runat="server" Text="Delete" Width="75" CssClass="default" />
            <asp:Button ID="btnClose" runat="server" Text="Close" Width="75" CssClass="default" />
        </td>
    </tr>
    <tr><td height="5" bgcolor="#c8cfdd"><img src="/admin/images/spacer.gif" width="1" height="5" /></td></tr>
</table>
<asp:Label ID="lblType" runat="server" Visible="false" />
<input type=hidden id="hdnFile" runat=server>
</form>
</body>
</html>


