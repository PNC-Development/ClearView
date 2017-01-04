<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="asset_models.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.asset_models" %>

<script type="text/javascript">
    var oAdd = null;
    var oView = null;
    var oAddButton = null;
    var oDeleteButton = null;
    var oHidden = null;
    var oName = null;
    var oMake = null;
    var oParent = null;
    var oParentId = null;
    var oPDF = null;
    var oSale = null;
    var oGrouping = null;
    var oHost = null;
    var oParentModel = null;
    var oSlots = null;
    var oUs = null;
    var oDestroy = null;
    var oSolarisInterface1 = null;
    var oSolarisInterface2 = null;
    var oSolarisBuildType = null;
    var oBootGroup = null;
    var oPowerDownProd = null;
    var oPowerDownTest = null;
    var oFactoryCode = null;
    var oFactoryCodeSpecific = null;
    var oEnabled = null;
    function Edit(strId, strName, strMake, strParentId, strParent, strPDF, strSale, strGrouping, strHost,strParentModel, strSlots, strUs, strDestroy, strSolarisInterface1, strSolarisInterface2, strSolarisBuildType, strBootGroup, strPowerDownProd, strPowerDownTest, strFactoryCode, strFactoryCodeSpecific, strEnabled) {
        oAdd.style.display = "inline";
        oView.style.display = "none";
        oAddButton.value = "Update";
        oDeleteButton.style.display = "inline";
        oHidden.value = strId;
        oName.value = strName;
        oMake.value = strMake;
        oParentId.value = strParentId;
        if (strParent == "" || strParentId == "0")
            oParent.innerText = "None";
        else
            oParent.innerText = strParent;
        oPDF.innerText = strPDF;
        oSale.checked = (strSale == "1");
        for (var ii=0; ii<oHost.length; ii++) {
            if (oHost.options[ii].value == strHost)
                oHost.selectedIndex = ii;
        }
        for (var ii=0; ii<oGrouping.length; ii++) {
            if (oGrouping.options[ii].value == strGrouping)
                oGrouping.selectedIndex = ii;
        }
        for (var ii=0; ii<oParentModel.length; ii++) {
            if (oParentModel.options[ii].value == strParentModel)
                oParentModel.selectedIndex = ii;
        }
        oSlots.value = strSlots;
        oUs.value = strUs;
        oDestroy.checked = (strDestroy == "1");
        for (var ii=0; ii<oSolarisInterface1.length; ii++) {
            if (oSolarisInterface1.options[ii].value == strSolarisInterface1)
                oSolarisInterface1.selectedIndex = ii;
        }
        for (var ii=0; ii<oSolarisInterface2.length; ii++) {
            if (oSolarisInterface2.options[ii].value == strSolarisInterface2)
                oSolarisInterface2.selectedIndex = ii;
        }
        for (var ii=0; ii<oSolarisBuildType.length; ii++) {
            if (oSolarisBuildType.options[ii].value == strSolarisBuildType)
                oSolarisBuildType.selectedIndex = ii;
        }
        for (var ii=0; ii<oBootGroup.length; ii++) {
            if (oBootGroup.options[ii].value == strBootGroup)
                oBootGroup.selectedIndex = ii;
        }
        oPowerDownProd.value = strPowerDownProd;
        oPowerDownTest.value = strPowerDownTest;
        oFactoryCode.value = strFactoryCode;
        oFactoryCodeSpecific.value = strFactoryCodeSpecific;
        oEnabled.checked = (strEnabled == "1");
    }
    function Add(strParentId, strParent) {
        oAdd.style.display = "inline";
        oView.style.display = "none";
        oAddButton.value = "Add";
        oDeleteButton.style.display = "none";
        oHidden.value = "0";
        oName.value = "";
        oMake.value = "";
        oParentId.value = strParentId;
        if (strParent == "" || strParentId == "0")
            oParent.innerText = "None";
        else
            oParent.innerText = strParent;
        oPDF.innerText = "";
        oSale.checked = false;
        oGrouping.selectedIndex = 0;
        oHost.selectedIndex = 0;
        oParentModel.selectedIndex = 0;
        oSlots.value = "";
        oUs.value = "";
        oDestroy.checked = false;
        oSolarisInterface1.selectedIndex = 0;
        oSolarisInterface2.selectedIndex = 0;
        oSolarisBuildType.selectedIndex = 0;
        oBootGroup.selectedIndex = 0;
        oPowerDownProd.value = "0";
        oPowerDownTest.value = "0";
        oFactoryCode.value = "";
        oFactoryCodeSpecific.value = "";
        oEnabled.checked = true;
        oMake.focus();
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
        oMake = document.getElementById('<%=txtMake.ClientID%>');
        oParent = document.getElementById('<%=lblParent.ClientID%>');
        oParentId = document.getElementById('<%=hdnParent.ClientID%>');
        oPDF = document.getElementById('<%=lblPDF.ClientID%>');
        oSale = document.getElementById('<%=chkSale.ClientID%>');
        oGrouping = document.getElementById('<%=ddlGrouping.ClientID%>');
        oHost = document.getElementById('<%=ddlHost.ClientID%>');
        oParentModel = document.getElementById('<%=ddlParentModel.ClientID%>');
        oSlots = document.getElementById('<%=txtSlots.ClientID%>');
        oUs = document.getElementById('<%=txtUs.ClientID%>');
        oDestroy = document.getElementById('<%=chkDestroy.ClientID%>');
        oSolarisInterface1 = document.getElementById('<%=ddlSolarisInterface1.ClientID%>');
        oSolarisInterface2 = document.getElementById('<%=ddlSolarisInterface2.ClientID%>');
        oSolarisBuildType = document.getElementById('<%=ddlSolarisBuildType.ClientID%>');
        oBootGroup = document.getElementById('<%=ddlBootGroup.ClientID%>');
        oPowerDownProd = document.getElementById('<%=txtPowerDownProd.ClientID%>');
        oPowerDownTest = document.getElementById('<%=txtPowerDownTest.ClientID%>');
        oFactoryCode = document.getElementById('<%=txtFactoryCode.ClientID%>');
        oFactoryCodeSpecific = document.getElementById('<%=txtFactoryCodeSpecific.ClientID%>');
        oEnabled = document.getElementById('<%=chkEnabled.ClientID%>');
    }
    function Open(strModel, strClass, strEnv) {
        OpenWindow('MODEL_ENVIRONMENTS',null,'?modelid=' + strModel + '&classid=' + strClass + '&environmentid=' + strEnv,false,450,600);
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
		    <td><b>Asset Models</b></td>
		    <td align="right"><a href="javascript:void(0);" onclick="Add(0);" class="cmlink" title="Click to Add New">Add New</a></td>
		</tr>
		<tr><td colspan="2">&nbsp;</td></tr>
		<tr>
		    <td colspan="2" align="center">
		        <asp:Panel ID="panAll" runat="server" Visible="false">
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
                            <td class="default">Type:</td>
                            <td><asp:label ID="lblParent" CssClass="default" runat="server" />&nbsp;&nbsp;<asp:Button ID="btnParent" runat="server" CssClass="default" Width="25" Text="..." /></td>
                        </tr>
                        <tr> 
                            <td class="default">Make:</td>
                            <td><asp:textbox ID="txtMake" CssClass="default" runat="server" Width="300" MaxLength="100"/></td>
                        </tr>
                        <tr> 
                            <td class="default">Model:</td>
                            <td><asp:textbox ID="txtName" CssClass="default" runat="server" Width="300" MaxLength="100"/></td>
                        </tr>
                        <tr> 
                            <td class="default">PDF Path:</td>
                            <td><asp:Label ID="lblPDF" CssClass="default" runat="server"/></td>
                        </tr>
                        <tr> 
                            <td class="default">PDF Upload:</td>
                            <td><asp:FileUpload ID="txtPDF" runat="server" CssClass="default" Width="500" Height="18" /></td>
                        </tr>
                        <tr> 
                            <td class="default">Sale:</td>
                            <td><asp:CheckBox ID="chkSale" runat="server" /></td>
                        </tr>
                        <tr>
                            <td class="default">Grouping:</td>
                            <td>
                                <asp:dropdownlist ID="ddlGrouping" CssClass="default" runat="server">
                                    <asp:ListItem Value="0" Text="None" />
                                    <asp:ListItem Value="1" Text="Distributed" />
                                    <asp:ListItem Value="2" Text="Midrange" />
                                </asp:dropdownlist>
                            </td>
                        </tr>
                        <tr>
                            <td class="default">Host Type:</td>
                            <td><asp:dropdownlist ID="ddlHost" CssClass="default" runat="server"/> (only use if this model is a guest on a host)</td>
                        </tr>
                        <tr>
                            <td class="default"></td>
                            <td class="reddefault">Example: Model &quot;DL380 G5&quot; hosts model &quot;Virtual Server Guest&quot;</td>
                        </tr>
                        <tr>
                            <td class="default"></td>
                            <td class="reddefault">For this example, model &quot;DL380 G5&quot; should be set to &quot;None&quot;, while the model &quot;Virtual Server Guest&quot; should be set to the host type nearest to &quot;Virtual Server&quot;...probably &quot;Virtual Server Host&quot;.</td>
                        </tr>
                        <tr>
                            <td class="default">Parent Model:</td>
                            <td><asp:dropdownlist ID="ddlParentModel" CssClass="default" runat="server"/></td>
                        </tr>
                        <tr>
                            <td class="default">Slots:</td>
                            <td><asp:TextBox ID="txtSlots" CssClass="default" runat="server" Width="50" MaxLength="3" /></td>
                        </tr>
                          <tr>
                            <td class="default">U's:</td>
                            <td><asp:TextBox ID="txtUs" CssClass="default" runat="server" Width="50" MaxLength="3" /></td>
                        </tr>
                        <tr> 
                            <td class="default">Destroy:</td>
                            <td><asp:CheckBox ID="chkDestroy" runat="server" Checked="true" /></td>
                        </tr>
                        <tr>
                            <td class="default">Solaris Interface # 1:</td>
                            <td><asp:dropdownlist ID="ddlSolarisInterface1" CssClass="default" runat="server"/></td>
                        </tr>
                        <tr>
                            <td class="default">Solaris Interface # 2:</td>
                            <td><asp:dropdownlist ID="ddlSolarisInterface2" CssClass="default" runat="server"/></td>
                        </tr>
                        <tr>
                            <td class="default">Solaris Build Type:</td>
                            <td><asp:dropdownlist ID="ddlSolarisBuildType" CssClass="default" runat="server"/></td>
                        </tr>
                        <tr>
                            <td class="default">Boot Group:</td>
                            <td><asp:dropdownlist ID="ddlBootGroup" CssClass="default" runat="server"/></td>
                        </tr>
                        <tr> 
                            <td class="default">Enabled:</td>
                            <td><asp:CheckBox ID="chkEnabled" runat="server" Checked="true" /></td>
                        </tr>
                        </tr>
                        <tr>
                            <td class="default">Power Down Prod:</td>
                            <td><asp:TextBox ID="txtPowerDownProd" CssClass="default" runat="server" Width="50" MaxLength="10" /> [Days]</td>
                        </tr>
                        <tr>
                            <td class="default">Power Down Test:</td>
                            <td><asp:TextBox ID="txtPowerDownTest" CssClass="default" runat="server" Width="50" MaxLength="10" /> [Days]</td>
                        </tr>
                        <tr>
                            <td class="default">Factory Code:</td>
                            <td><asp:TextBox ID="txtFactoryCode" CssClass="default" runat="server" Width="50" MaxLength="10" /></td>
                        </tr>
                        <tr>
                            <td class="default">Factory Code Specific:</td>
                            <td><asp:TextBox ID="txtFactoryCodeSpecific" CssClass="default" runat="server" Width="50" MaxLength="10" /></td>
                        </tr>
                        <tr> 
                            <td class="cmdefault">&nbsp;</td>
                            <td><asp:Button ID="btnReservations" runat="server" Text="Change Reservations" Width="150" CssClass="default" OnClick="btnReservations_Click" /></td>
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
                </asp:Panel>
                
		        <asp:Panel ID="panOne" runat="server" Visible="false">
                <table width="100%" border="0" cellpadding="0" cellspacing="0">
                    <tr>
                        <td><asp:Label ID="lblModel" runat="server" CssClass="header" /></td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TreeView ID="treModel" runat="server" ShowLines="true" NodeIndent="35">
                                <NodeStyle CssClass="default" />
                            </asp:TreeView>
                        </td>
                    </tr>
                </table>
                </asp:Panel>
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
