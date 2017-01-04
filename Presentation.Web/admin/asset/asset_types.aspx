<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="asset_types.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.asset_types" %>

<script type="text/javascript">
    var oAdd = null;
    var oView = null;
    var oAddButton = null;
    var oDeleteButton = null;
    var oHidden = null;
    var oName = null;
    var oParent = null;
    var oParentId = null;
    var oConfiguration = null;
    var oDeploy = null;
    var oDesignExecution = null;
    var oForecastExecution = null;
    var oOndemandExecution = null;
    var oOndemandSteps = null;
    var oWarning = null;
    var oCritical = null;
    var oEnabled = null;
    function Edit(strId, strName, strParentId, strParent, strConfiguration, strDeploy, strDesignExecution, strForecastExecution, strOndemandExecution, strOndemandSteps, strWarning, strCritical, strEnabled) {
        oAdd.style.display = "inline";
        oView.style.display = "none";
        oAddButton.value = "Update";
        oDeleteButton.style.display = "inline";
        oHidden.value = strId;
        oName.value = strName;
        oParentId.value = strParentId;
        if (strParent == "" || strParentId == "0")
            oParent.innerText = "None";
        else
            oParent.innerText = strParent;
        oConfiguration.value = strConfiguration;
        oDeploy.value = strDeploy;
        oDesignExecution.value = strDesignExecution;
        oForecastExecution.value = strForecastExecution;
        oOndemandExecution.value = strOndemandExecution;
        oOndemandSteps.value = strOndemandSteps;
        oWarning.value = strWarning;
        oCritical.value = strCritical;
        oEnabled.checked = (strEnabled == "1");
    }
    function Add(strParentId, strParent) {
        oAdd.style.display = "inline";
        oView.style.display = "none";
        oAddButton.value = "Add";
        oDeleteButton.style.display = "none";
        oHidden.value = "0";
        oName.value = "";
        oParentId.value = strParentId;
        if (strParent == "" || strParentId == "0")
            oParent.innerText = "None";
        else
            oParent.innerText = strParent;
        oConfiguration.value = "";
        oDeploy.value = "";
        oDesignExecution.value = "";
        oForecastExecution.value = "";
        oOndemandExecution.value = "";
        oOndemandSteps.value = "";
        oWarning.value = "";
        oCritical.value = "";
        oEnabled.checked = true;
        oName.focus();
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
        oName = document.getElementById('<%=txtName.ClientID%>');
        oParent = document.getElementById('<%=lblParent.ClientID%>');
        oParentId = document.getElementById('<%=hdnParent.ClientID%>');
        oConfiguration = document.getElementById('<%=txtConfiguration.ClientID%>');
        oDeploy = document.getElementById('<%=txtDeploy.ClientID%>');
        oDesignExecution = document.getElementById('<%=txtDesignExecution.ClientID%>');
        oForecastExecution = document.getElementById('<%=txtForecastExecution.ClientID%>');
        oOndemandExecution = document.getElementById('<%=txtOndemandExecution.ClientID%>');
        oOndemandSteps = document.getElementById('<%=txtOndemandSteps.ClientID %>');
        oWarning = document.getElementById('<%=txtWarning.ClientID%>');
        oCritical = document.getElementById('<%=txtCritical.ClientID %>');
        oEnabled = document.getElementById('<%=chkEnabled.ClientID%>');
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
		    <td><b>Asset Types</b></td>
		    <td align="right"><a href="javascript:void(0);" onclick="Add();" class="cmlink" title="Click to Add New">Add New</a></td>
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
                            <td class="default" width="100px">Name:</td>
                            <td><asp:textbox ID="txtName" CssClass="default" runat="server" Width="300" MaxLength="100"/></td>
                        </tr>
                        <tr>
                            <td class="default">Platform:</td>
                            <td><asp:label ID="lblParent" CssClass="default" runat="server" />&nbsp;&nbsp;<asp:Button ID="btnParent" runat="server" CssClass="default" Width="25" Text="..." /></td>
                        </tr>
                        <tr> 
                            <td class="default">Configuration Path:</td>
                            <td><asp:textbox ID="txtConfiguration" CssClass="default" runat="server" Width="400" MaxLength="100"/> <asp:button ID="btnConfiguration" runat="server" Text="..." Width="25" CssClass="default"/></td>
                        </tr>
                        <tr> 
                            <td class="default">Deploy Path:</td>
                            <td><asp:textbox ID="txtDeploy" CssClass="default" runat="server" Width="400" MaxLength="100"/> <asp:button ID="btnDeploy" runat="server" Text="..." Width="25" CssClass="default"/></td>
                        </tr>
                        <tr> 
                            <td class="default">Design (New) Execution Page:</td>
                            <td><asp:textbox ID="txtDesignExecution" CssClass="default" runat="server" Width="400" MaxLength="100"/> <asp:button ID="btnDesignExecution" runat="server" Text="..." Width="25" CssClass="default"/></td>
                        </tr>
                        <tr> 
                            <td class="default">Forecast (Old) Execution Page:</td>
                            <td><asp:textbox ID="txtForecastExecution" CssClass="default" runat="server" Width="400" MaxLength="100"/> <asp:button ID="btnForecastExecution" runat="server" Text="..." Width="25" CssClass="default"/></td>
                        </tr>
                        <tr> 
                            <td class="default">OnDemand Execution Control:</td>
                            <td><asp:textbox ID="txtOndemandExecution" CssClass="default" runat="server" Width="400" MaxLength="100"/> <asp:button ID="btnOndemandExecution" runat="server" Text="..." Width="25" CssClass="default"/></td>
                        </tr>
                        <tr> 
                            <td class="default">OnDemand Steps Path:</td>
                            <td><asp:textbox ID="txtOndemandSteps" CssClass="default" runat="server" Width="400" MaxLength="100"/> <asp:button ID="btnOndemandJavaWindow" runat="server" Text="..." Width="25" CssClass="default"/></td>
                        </tr>
                        <tr> 
                            <td class="default">&nbsp;</td>
                            <td><asp:Button ID="btnCopy" runat="server" Text="Copy" Width="75" CssClass="default" /> <asp:Button ID="btnPaste" runat="server" Text="Paste" Width="75" CssClass="default" /></td>
                        </tr>
                        <tr> 
                            <td class="default">Inventory Warning:</td>
                            <td><asp:textbox ID="txtWarning" CssClass="default" runat="server" Width="100" MaxLength="5"/></td>
                        </tr>
                        <tr> 
                            <td class="default">Inventory Critical:</td>
                            <td><asp:textbox ID="txtCritical" CssClass="default" runat="server" Width="100" MaxLength="5"/></td>
                        </tr>
                        <tr> 
                            <td class="default">Enabled:</td>
                            <td><asp:CheckBox ID="chkEnabled" runat="server" Checked="true" /></td>
                        </tr>
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
