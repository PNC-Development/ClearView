<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="reports.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.reports" %>

<script type="text/javascript">
    var oAdd = null;
    var oView = null;
    var oAddButton = null;
    var oDeleteButton = null;
    var oHidden = null;
    var oTitle = null;
    var oOld = null;
    var oPath = null;
    var oPhysical = null;
    var oDesc = null;
    var oAbout = null;
    var oImage = null;
    var oParent = null;
    var oParentId = null;
    var oPercent = null;
    var oToggle = null;
    var oApplication = null;
    var oEnabled = null;
    function Edit(strId, strTitle, strOld, strPath, strPhysical, strDesc, strAbout, strImage, strParentId, strParent, strPercent, strToggle, strApplication, strEnabled) {
        oAdd.style.display = "inline";
        oView.style.display = "none";
        oAddButton.value = "Update";
        oDeleteButton.style.display = "inline";
        oHidden.value = strId;
        oTitle.value = strTitle;
        oOld.checked = (strOld == "1");
        oPath.value = strPath;
        oPhysical.value = strPhysical;
        oDesc.value = strDesc;
        oAbout.value = strAbout;
        oImage.value = strImage;
        oParentId.value = strParentId;
        if (strParent == "" || strParentId == "0")
            oParent.innerText = "No Parent";
        else
            oParent.innerText = strParent;
        oPercent.value = strPercent;
        for (var ii=0; ii<oToggle.length; ii++) {
            if (oToggle.options[ii].value == strToggle)
                oToggle.selectedIndex = ii;
        }
        oApplication.checked = (strApplication == "1");
        oEnabled.checked = (strEnabled == "1");
    }
    function Add(strParentId, strParent) {
        oAdd.style.display = "inline";
        oView.style.display = "none";
        oAddButton.value = "Add";
        oDeleteButton.style.display = "none";
        oHidden.value = "0";
        oTitle.value = "";
        oOld.checked = false;
        oPath.value = "";
        oPhysical.value = "";
        oDesc.value = "";
        oAbout.value = "";
        oImage.value = "";
        oParentId.value = strParentId;
        if (strParent == "" || strParentId == "0")
            oParent.innerText = "No Parent";
        else
            oParent.innerText = strParent;
        oPercent.value = "0";
        oToggle.selectedIndex = 0;
        oApplication.checked = false;
        oEnabled.checked = true;
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
        oHidden = document.getElementById('<%=hdnId.ClientID%>');
        oTitle = document.getElementById('<%=txtTitle.ClientID%>');
        oOld = document.getElementById('<%=chkOld.ClientID%>');
        oPath = document.getElementById('<%=txtPath.ClientID%>');
        oPhysical = document.getElementById('<%=txtPhysical.ClientID%>');
        oDesc = document.getElementById('<%=txtDesc.ClientID%>');
        oAbout = document.getElementById('<%=txtAbout.ClientID%>');
        oImage = document.getElementById('<%=txtImage.ClientID%>');
        oParent = document.getElementById('<%=lblParent.ClientID%>');
        oParentId = document.getElementById('<%=hdnParent.ClientID%>');
        oPercent = document.getElementById('<%=txtPercent.ClientID%>');
        oToggle = document.getElementById('<%=ddlToggle.ClientID%>');
        oApplication = document.getElementById('<%=chkApplication.ClientID%>');
        oEnabled = document.getElementById('<%=chkEnabled.ClientID%>');
    }
</script>
<html>
<head>
<title>ClearView Administration</title>
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
		    <td><b>Reports</b></td>
		    <td align="right">&nbsp;</td>
		</tr>
		<tr><td colspan="2">&nbsp;</td></tr>
		<tr>
		    <td colspan="2" align="center">
                <div ID="divView" runat="server">
                <table width="100%" border="0" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <asp:TreeView ID="oTreeview" runat="server" ShowLines="true" NodeIndent="35">
                                <NodeStyle CssClass="default" />
                            </asp:TreeView>
                        </td>
                    </tr>
                </table>
               </div>

                <div id="divAdd" runat="server" style="display:none">
                    <table width="95%" cellpadding="3" cellspacing="0" border="0" align="center">
                        <tr> 
                            <td class="default" width="100px">Title:</td>
                            <td><asp:textbox ID="txtTitle" CssClass="default" runat="server" Width="300" MaxLength="100"/></td>
                        </tr>
                        <tr>
                            <td class="default">Old Version:</td>
                            <td><asp:CheckBox ID="chkOld" runat="server" Checked="true" /></td>
                        </tr>
                        <tr> 
                            <td class="default">Path:</td>
                            <td><asp:textbox ID="txtPath" CssClass="default" runat="server" Width="400" MaxLength="200"/> <asp:button ID="btnPath" runat="server" Text="..." Width="25" CssClass="default"/></td>
                        </tr>
                        <tr> 
                            <td class="default">Physical Path:</td>
                            <td><asp:textbox ID="txtPhysical" CssClass="default" runat="server" Width="400" MaxLength="200"/> <asp:button ID="btnPhysical" runat="server" Text="..." Width="25" CssClass="default"/></td>
                        </tr>
                        <tr> 
                            <td class="default" width="100px" valign="top">Description:</td>
                            <td><asp:textbox ID="txtDesc" CssClass="default" runat="server" Width="300" TextMode="MultiLine" Rows="5" /></td>
                        </tr>
                        <tr> 
                            <td class="default" width="100px" valign="top">About:</td>
                            <td><asp:textbox ID="txtAbout" CssClass="default" runat="server" Width="300" TextMode="MultiLine" Rows="5" /></td>
                        </tr>
                        <tr> 
                            <td class="default">Image:</td>
                            <td><asp:textbox ID="txtImage" CssClass="default" runat="server" Width="300" MaxLength="100"/> <asp:Button ID="btnImage" runat="server" CssClass="default" Text="..." /></td>
                        </tr>
                        <tr>
                            <td class="default">Parent:</td>
                            <td><asp:label ID="lblParent" CssClass="default" runat="server" />&nbsp;&nbsp;<asp:Button ID="btnParent" runat="server" CssClass="default" Width="25" Text="..." /></td>
                        </tr>
                        <tr>
                            <td class="default">Percentage:</td>
                            <td><asp:textbox ID="txtPercent" CssClass="default" runat="server" Width="100" MaxLength="3"/></td>
                        </tr>
                        <tr> 
                            <td class="default">Toggle Options:</td>
                            <td>
                                <asp:DropDownList ID="ddlToggle" runat="server" CssClass="default">
                                    <asp:ListItem Value="-1" Text="Minimized" />
                                    <asp:ListItem Value="0" Text="None" />
                                    <asp:ListItem Value="1" Text="Expanded" />
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr> 
                            <td class="default">Application:</td>
                            <td><asp:CheckBox ID="chkApplication" runat="server" Checked="true" /></td>
                        </tr>
                        <tr> 
                            <td class="default">Enabled:</td>
                            <td><asp:CheckBox ID="chkEnabled" runat="server" Checked="true" /></td>
                        </tr>
                        <tr> 
                            <td class="default">Permissions:</td>
                            <td><asp:Button ID="btnPermissions" runat="server" Text="Edit Groups" Width="125" CssClass="default" /> <asp:Button ID="btnApplications" runat="server" Text="Edit Applications" Width="125" CssClass="default" /> <asp:Button ID="btnUsers" runat="server" Text="Edit Users" Width="125" CssClass="default" /></td>
                        </tr>
                        <tr><td height="5" colspan="2">&nbsp;</td></tr>
                        <tr> 
                            <td class="cmdefault">&nbsp;</td>
                            <td><asp:Button ID="btnOrder" runat="server" Text="Change Order" Width="150" CssClass="default" /></td>
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
    <input type="hidden" id="hdnOrder" runat="server" />
</form>
</body>
</html>
