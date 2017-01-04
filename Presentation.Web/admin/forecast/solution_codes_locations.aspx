<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="solution_codes_locations.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.solution_codes_locations" %>

<script type="text/javascript">
    var oAdd = null;
    var oView = null;
    var oAddButton = null;
    var oHidden = null;
    var oClass = null;
    var oEnv = null;
    var oAddress = null;
    var oCode = null;
    function Edit(strId, strClass, strEnv, strParentId, strParent, strCodes) {
        oAdd.style.display = "inline";
        oView.style.display = "none";
        oAddButton.style.display = "inline";
        oHidden.value = strId;
        for (var ii=0; ii<oClass.length; ii++) {
            if (oClass.options[ii].value == strClass)
                oClass.selectedIndex = ii;
        }
        PopulateEnvironments2(oClass, oEnvironment);
        for (var ii=0; ii<oEnvironment.length; ii++) {
            if (oEnvironment.options[ii].value == strEnv)
                oEnvironment.selectedIndex = ii;
        }
        oEnvironmentH.value = strEnv;
        oParentId.value = strParentId;
        if (strParentId == "" || strParentId == "0")
            oParent.innerText = "None";
        else
            oParent.innerText = strParent;
        var arCodes = strCodes.split(";");
        for (var jj=0; jj<oCode.length; jj++) {
            oCode.options[jj].selected = false;
            for (var ii=0; ii<arCodes.length; ii++) {
                if (oCode.options[jj].value == arCodes[ii]) 
                {
                    oCode.options[jj].selected = true;
                    break;
                }
            }
        }
    }
    function Add(strClass, strEnv) {
        oAdd.style.display = "inline";
        oView.style.display = "none";
        oAddButton.style.display = "inline";
        oHidden.value = "0";
        for (var ii=0; ii<oClass.length; ii++) {
            if (oClass.options[ii].value == strClass)
                oClass.selectedIndex = ii;
        }
        PopulateEnvironments2(oClass, oEnvironment);
        for (var ii=0; ii<oEnvironment.length; ii++) {
            if (oEnvironment.options[ii].value == strEnv)
                oEnvironment.selectedIndex = ii;
        }
        oEnvironmentH.value = strEnv;
        oParentId.value = '<%=intLocation.ToString() %>';
        oParent.innerText = '<%=oLocation.GetFull(intLocation) %>';
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
        oHidden = document.getElementById('<%=hdnId.ClientID%>');
        oClass = document.getElementById('<%=ddlClass.ClientID%>');
        oEnvironment = document.getElementById('<%=ddlEnvironment.ClientID%>');
        oEnvironmentH = document.getElementById('<%=hdnEnvironment.ClientID%>');
        oParent = document.getElementById('<%=lblParent.ClientID%>');
        oParentId = document.getElementById('<%=hdnParent.ClientID%>');
        oCode = document.getElementById('<%=lstCode.ClientID%>');
    }
</script>
<html>
<head>
<title>ClearView Administration</title>
<link rel="stylesheet" type="text/css" href="/css/default.css" />
<script type="text/javascript" src="/javascript/global.js"></script>
<script type="text/javascript" src="/javascript/both.js"></script>
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
		    <td><b>Solution Code Locations</b></td>
		    <td align="right"><a href="javascript:void(0);" onclick="OpenWindow('GLOBAL_SOLUTION_CODE','','',false,400,200);">Add Code to All Locations</a></td>
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
                            <td class="default">Class:</td>
                            <td><asp:dropdownlist ID="ddlClass" CssClass="default" runat="server"/></td>
                        </tr>
                        <tr> 
                            <td class="default">Environment:</td>
                            <td>
                                <asp:dropdownlist ID="ddlEnvironment" CssClass="default" runat="server" Enabled="false">
                                    <asp:ListItem Value="-- Please select a Class --" />
                                </asp:dropdownlist>
                            </td>
                        </tr>
                        <tr>
                            <td class="default">Address:</td>
                            <td><asp:label ID="lblParent" CssClass="default" runat="server" />&nbsp;&nbsp;<asp:Button ID="btnParent" runat="server" CssClass="default" Width="25" Text="..." /></td>
                        </tr>
                        <tr> 
                            <td class="default">Code:</td>
                            <td><asp:ListBox ID="lstCode" CssClass="default" runat="server" Width="400" Rows="20" SelectionMode="Multiple"/></td>
                        </tr>
                        <tr><td height="5" colspan="2">&nbsp;</td></tr>
                        <tr> 
                            <td>&nbsp;</td>
                            <td>
                                <asp:button ID="btnAdd" CssClass="default" runat="server" Text="Save" Width="75" OnClick="btnAdd_Click" />
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
    <input type="hidden" id="hdnEnvironment" runat="server" />
    <input type="hidden" id="hdnOrder" runat="server" />
</form>
</body>
</html>
