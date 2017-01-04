<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="pages.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.pages" %>


<script type="text/javascript">
    var oAdd = null;
    var oView = null;
    var oAddButton = null;
    var oDeleteButton = null;
    var oControls = null;
    var oHidden = null;
    var oParent = null;
    var oParentH = null;
    var oTitle = null;
    var oUrlTitle = null;
    var oMenuTitle = null;
    var oBrowserTitle = null;
    var oTemplate = null;
    var oRelated = null;
    var oNavImage = null;
    var oNavOverImage = null;
    var oDescription = null;
    var oToolTip = null;
    var oSProc = null;
    var oWindow = null;
    var oUrl = null;
    var oTarget = null;
    var oNavigation = null;
    var oDisplay = null;
    var oOrder = null;
    var oEnabled = null;
    function Edit(strId, strParent, strParentH, strTitle, strUrlTitle, strMenuTitle, strBrowserTitle, strTemplate, strRelated, strRelatedH, strNavImage, strNavOverImage, strDescription, strToolTip, strSProc, strWindow, strUrl, strTarget, strNavigation, strEnabled) {
        oAdd.style.display = "inline";
        oView.style.display = "none";
        oAddButton.value = "Update";
        oDeleteButton.style.display = "inline";
        oControls.disabled = false;
        oHidden.value = strId;
        if (strParent == "")
            oParent.innerText = "No Parent Page";
        else
            oParent.innerText = strParent;
        oParentH.value = strParentH;
        oTitle.value = strTitle;
        oUrlTitle.value = strUrlTitle;
        oMenuTitle.value = strMenuTitle;
        oBrowserTitle.value = strBrowserTitle;
        for (var ii=0; ii<oTemplate.length; ii++) {
            if (oTemplate.options[ii].value == strTemplate)
                oTemplate.selectedIndex = ii;
        }
        if (strRelated == "")
            oRelated.innerText = "No Related Page";
        else
            oRelated.innerText = strRelated;
        oRelatedH.value = strRelatedH;
        oNavImage.value = strNavImage;
        oNavOverImage.value = strNavOverImage;
        oDescription.value = strDescription;
        oToolTip.value = strToolTip;
        oSProc.value = strSProc;
        oWindow.checked = (strWindow == "1");
        oUrl.value = strUrl;
        for (var ii=0; ii<oTarget.length; ii++) {
            if (oTarget.options[ii].value == strTarget)
                oTarget.selectedIndex = ii;
        }
        oNavigation.checked = (strNavigation == "1");
        oDisplay.value = "";
        oOrder.disabled = false;
        oEnabled.checked = (strEnabled == "1");
    }
    function Add() {
        oAdd.style.display = "inline";
        oView.style.display = "none";
        oAddButton.value = "Add";
        oDeleteButton.style.display = "none";
        oControls.disabled = true;
        oHidden.value = "0";
        oParent.innerText = "No Parent Page";
        oParentH.value = "0";
        oTitle.value = "";
        oUrlTitle.value = "";
        oMenuTitle.value = "";
        oBrowserTitle.value = "";
        oTemplate.selectedIndex = 0;
        oRelated.innerText = "No Related Page";
        oRelatedH.value = "0";
        oNavImage.value = "";
        oNavOverImage.value = "";
        oToolTip.value = "";
        oDescription.value = "";
        oSProc.value = "";
        oWindow.checked = false;
        oUrl.value = "";
        oTarget.selectedIndex = 0;
        oNavigation.checked = false;
        oDisplay.value = "";
        oEnabled.checked = true;
        oOrder.disabled = true;
        oTitle.focus();
    }
    function Cancel() {
        oAdd.style.display = "none";
        oView.style.display = "inline";
        return false;
    }
    function Load() {
        oAdd = document.getElementById('<%=divAdd.ClientID%>');
        oView = document.getElementById('<%=divView.ClientID%>');
        oAddButton = document.getElementById('<%=btnAdd.ClientID%>');
        oDeleteButton = document.getElementById('<%=btnDelete.ClientID%>');
        oControls = document.getElementById('<%=btnControls.ClientID%>');
        oHidden = document.getElementById('<%=hdnId.ClientID%>');
        oParent = document.getElementById('<%=lblParent.ClientID%>');
        oParentH = document.getElementById('<%=hdnParent.ClientID%>');
        oTitle = document.getElementById('<%=txtTitle.ClientID%>');
        oUrlTitle = document.getElementById('<%=txtUrlTitle.ClientID%>');
        oMenuTitle = document.getElementById('<%=txtMenuTitle.ClientID%>');
        oBrowserTitle = document.getElementById('<%=txtBrowser.ClientID%>');
        oTemplate = document.getElementById('<%=ddlTemplate.ClientID%>');
        oRelated = document.getElementById('<%=lblRelated.ClientID%>');
        oRelatedH = document.getElementById('<%=hdnRelated.ClientID%>');
        oNavImage = document.getElementById('<%=txtNav.ClientID%>');
        oNavOverImage = document.getElementById('<%=txtNavOver.ClientID%>');
        oDescription = document.getElementById('<%=txtDescription.ClientID%>');
        oToolTip = document.getElementById('<%=txtToolTip.ClientID%>');
        oSProc = document.getElementById('<%=txtSProc.ClientID%>');
        oWindow = document.getElementById('<%=chkWindow.ClientID%>');
        oUrl = document.getElementById('<%=txtUrl.ClientID%>');
        oTarget = document.getElementById('<%=ddlTarget.ClientID%>');
        oNavigation = document.getElementById('<%=chkNavigation.ClientID%>');
        oDisplay = document.getElementById('<%=hdnOrder.ClientID%>');
        oOrder = document.getElementById('<%=btnOrder.ClientID%>');
        oEnabled = document.getElementById('<%=chkEnabled.ClientID%>');
    }
    function FillTitle(ot,ou,om,ob) {
        ot = document.getElementById(ot);
        ou = document.getElementById(ou);
        om = document.getElementById(om);
        ob = document.getElementById(ob);
        var strTitle = ot.value;
        om.value = strTitle;
        ob.value = strTitle;
		while (strTitle.indexOf(" ") > -1) {
			strTitle = strTitle.replace(" ", "");
		}
        ou.value = strTitle;
        return false;
    }
</script>
<html>
<head>
<title>Web Content Management Administration</title>
<link rel="stylesheet" type="text/css" href="/css/default.css" />
<script type="text/javascript" src="/javascript/global.js"></script>
</head>
<body topmargin="0" leftmargin="0" onload="Load()">
<form id="Form1" runat="server">
        <table width="100%" height="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
        <td height="100%">
        <div style="height:100%; overflow:auto">
<table width="98%" cellpadding="0" cellspacing="0" border="0" align="center">
		<tr><td colspan="2">&nbsp;</td></tr>
		<tr> 
		    <td><b>Pages</b></td>
		    <td align="right"><a href="javascript:void(0);" onclick="Add();" class="cmlink" title="Click to Add New">Add New</a></td>
		</tr>
		<tr><td colspan="2">&nbsp;</td></tr>
		<tr>
		    <td colspan="2" align="center">
                <div ID="divView" runat="server">
                    <table width="100%" border="0" cellpadding="0" cellspacing="0">
                        <tr>
                            <td>
                                <div style="width:100%; height:100%; overflow:auto;">
                                <asp:TreeView ID="oTreeview" runat="server" ShowLines="true" NodeIndent="35">
                                    <NodeStyle CssClass="default" />
                                </asp:TreeView>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>

                <div id="divAdd" runat="server" style="display:none">
                    <table width="95%" cellpadding="3" cellspacing="0" border="0" align="center">
                        <tr> 
                            <td class="default" nowrap>Parent Page:</td>
                            <td><asp:label ID="lblParent" CssClass="default" runat="server" />&nbsp;&nbsp;<asp:Button ID="btnParent" runat="server" CssClass="default" Width="25" Text="..." /></td>
                        </tr>
                        <tr> 
                            <td class="default">Title:</td>
                            <td><asp:textbox ID="txtTitle" CssClass="default" runat="server" Width="300" MaxLength="100"/>  <asp:Button ID="btnFill" runat="server" CssClass="default" Text="V" /></td>
                        </tr>
                        <tr> 
                            <td class="default">URL Title:</td>
                            <td><asp:textbox ID="txtUrlTitle" CssClass="default" runat="server" Width="200" MaxLength="50"/></td>
                        </tr>
                        <tr> 
                            <td class="default">Menu Title:</td>
                            <td><asp:textbox ID="txtMenuTitle" CssClass="default" runat="server" Width="200" MaxLength="50"/></td>
                        </tr>
                        <tr> 
                            <td class="default">Browser Title:</td>
                            <td><asp:textbox ID="txtBrowser" CssClass="default" runat="server" Width="300" MaxLength="100"/></td>
                        </tr>
                        <tr> 
                            <td class="default">Template:</td>
                            <td><asp:dropdownlist ID="ddlTemplate" CssClass="default" runat="server"/></td>
                        </tr>
                        <tr> 
                            <td class="default" nowrap>Related Page:</td>
                            <td><asp:label ID="lblRelated" CssClass="default" runat="server" />&nbsp;&nbsp;<asp:Button ID="btnRelated" runat="server" CssClass="default" Width="25" Text="..." /></td>
                        </tr>
                        <tr> 
                            <td class="default">NAV Image:</td>
                            <td><asp:textbox ID="txtNav" CssClass="default" runat="server" Width="300" MaxLength="100"/> <asp:Button ID="btnBrowseNav" runat="server" CssClass="default" Text="..." /></td>
                        </tr>
                        <tr> 
                            <td class="default">NAV Over Image:</td>
                            <td><asp:textbox ID="txtNavOver" CssClass="default" runat="server" Width="300" MaxLength="100"/> <asp:Button ID="btnBrowseOver" runat="server" CssClass="default" Text="..." /></td>
                        </tr>
                        <tr> 
                            <td class="default" valign="top">Description:</td>
                            <td><asp:textbox ID="txtDescription" CssClass="default" runat="server" Width="300" Rows="3" TextMode="MultiLine"/></td>
                        </tr>
                        <tr> 
                            <td class="default" valign="top">Tool Tip / Help:</td>
                            <td><asp:textbox ID="txtToolTip" CssClass="default" runat="server" Width="300" Rows="5" TextMode="MultiLine"/></td>
                        </tr>
                        <tr>
                            <td class="default">&nbsp;</td>
                            <td class="required">&amp;#13; = Line Break</td>
                        </tr>
                        <tr> 
                            <td class="default">Stored Proc:</td>
                            <td><asp:textbox ID="txtSProc" CssClass="default" runat="server" Width="300" MaxLength="100"/></td>
                        </tr>
                        <tr> 
                            <td class="default">New Window:</td>
                            <td><asp:CheckBox ID="chkWindow" runat="server" /></td>
                        </tr>
                        <tr> 
                            <td class="default">URL:</td>
                            <td>
                                <asp:textbox ID="txtUrl" CssClass="default" runat="server" Width="300" MaxLength="100"/>
                                &nbsp;
                                <asp:DropDownList ID="ddlTarget" runat="server" CssClass="default">
                                    <asp:ListItem Value="_self" />
                                    <asp:ListItem Value="_blank" />
                                    <asp:ListItem Value="_parent" />
                                    <asp:ListItem Value="_top" />
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr> 
                            <td class="default">Navigation:</td>
                            <td><asp:CheckBox ID="chkNavigation" runat="server" CssClass="default" /></td>
                        </tr>
                        <tr> 
                            <td class="default">Enabled:</td>
                            <td><asp:CheckBox ID="chkEnabled" runat="server" Checked="true" /></td>
                        </tr>
                        <tr> 
                            <td class="cmdefault">&nbsp;</td>
                            <td><asp:Button ID="btnOrder" runat="server" CssClass="default" Text="Change Order" Width="125" /></td>
                        </tr>
                        <tr> 
                            <td class="cmdefault">&nbsp;</td>
                            <td><asp:Button ID="btnControls" runat="server" Text="Edit Controls" Width="125" CssClass="default" /></td>
                        </tr>
                        <tr><td height="5" colspan="2">&nbsp;</td></tr>
                        <tr> 
                            <td>&nbsp;</td>
                            <td>
                                <asp:button ID="btnAdd" CssClass="default" runat="server" Text="Add" Width="75" OnClick="btnAdd_Click" />
                                <asp:button ID="btnDelete" CssClass="default" runat="server" Text="Delete" Width="75" OnClick="btnDelete_Click" />
                                <asp:button ID="btnCancel" CssClass="default" runat="server" Text="Cancel" Width="75" />
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
    </table>
        </div>
        </td>
        </tr>
        </table>
    <input type="hidden" id="hdnId" runat="server" />
    <input type="hidden" id="hdnParent" runat="server" />
    <input type="hidden" id="hdnRelated" runat="server" />
    <input type="hidden" id="hdnOrder" runat="server" />
</form>
</body>
</html>
