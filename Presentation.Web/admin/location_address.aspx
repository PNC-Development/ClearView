<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="location_address.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.location_address" %>

<script type="text/javascript">
    var oAdd = null;
    var oView = null;
    var oAddButton = null;
    var oDeleteButton = null;
    var oHidden = null;
    var oName = null;
    var oCode = null;
    var oParent = null;
    var oParentId = null;
    var oCommon = null;
    var oCommonname = null;
    var oStorage = null;
    var oTSM = null;
    var oDR = null;
    var oProd = null;
    var oQA = null;
    var oTest = null;
    var oTypeDefault = null;
    var oTypeOffsite = null;
    var oTypeManual = null;
    var oBuildingCode = null;
    var oServiceNow = null;
    var oRecovery = null;
    var oVMwareIP = null;
    var oEnabled = null;
    function Edit(strId, strName, strCode, strParentId, strParent, strCommon, strCommonname, strStorage, strTSM, strDR, strProd, strQA, strTest, strTypeOffsite, strTypeManual, strBuildingCode, strServiceNow, strRecovery, strVMwareIP, strEnabled) {
        oAdd.style.display = "inline";
        oView.style.display = "none";
        oAddButton.value = "Update";
        oDeleteButton.style.display = "inline";
        oHidden.value = strId;
        oName.value = strName;
        oCode.value = strCode;
        oParentId.value = strParentId;
        if (strParent == "" || strParentId == "0")
            oParent.innerText = "None";
        else
            oParent.innerText = strParent;
        oCommon.checked = (strCommon == "1");
        oCommonname.value = strCommonname;
        oStorage.checked = (strStorage == "1");
        oTSM.checked = (strTSM == "1");
        oDR.checked = (strDR == "1");
        oProd.checked = (strProd == "1");
        oQA.checked = (strQA == "1");
        oTest.checked = (strTest == "1");
        oTypeOffsite.checked = (strTypeOffsite == "1");
        oTypeManual.checked = (strTypeManual == "1");
        oTypeDefault.checked = (oTypeOffsite.checked == false && oTypeManual.checked == false);
        oBuildingCode.value = strBuildingCode;
        oServiceNow.value = strServiceNow;
        oRecovery.checked = (strRecovery == "1");
        oVMwareIP.checked = (strVMwareIP == "1");
        oEnabled.checked = (strEnabled == "1");
    }
    function Add(strParentId, strParent) {
        oAdd.style.display = "inline";
        oView.style.display = "none";
        oAddButton.value = "Add";
        oDeleteButton.style.display = "none";
        oHidden.value = "0";
        oName.value = "";
        oCode.value = "";
        oParentId.value = strParentId;
        if (strParent == "" || strParentId == "0")
            oParent.innerText = "None";
        else
            oParent.innerText = strParent;
        oCommon.checked = false;
        oCommonname.value = "";
        oStorage.checked = false;
        oTSM.checked = false;
        oDR.checked = false;
        oProd.checked = false;
        oQA.checked = false;
        oTest.checked = false;
        oTypeDefault.checked = true;
        oTypeOffsite.checked = false;
        oTypeManual.checked = false;
        oBuildingCode.value = "";
        oServiceNow.value = "";
        oRecovery.checked = false;
        oVMwareIP.checked = false;
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
        oCode = document.getElementById('<%=txtCode.ClientID%>');
        oParent = document.getElementById('<%=lblParent.ClientID%>');
        oParentId = document.getElementById('<%=hdnParent.ClientID%>');
        oCommon = document.getElementById('<%=chkCommon.ClientID%>');
        oCommonname = document.getElementById('<%=txtCommonName.ClientID%>');
        oStorage = document.getElementById('<%=chkStorage.ClientID%>');
        oTSM = document.getElementById('<%=chkTSM.ClientID%>');
        oDR = document.getElementById('<%=chkDR.ClientID%>');
        oProd = document.getElementById('<%=chkProd.ClientID%>');
        oQA = document.getElementById('<%=chkQA.ClientID%>');
        oTest = document.getElementById('<%=chkTest.ClientID%>');
        oTypeDefault = document.getElementById('<%=radTypeDefault.ClientID%>');
        oTypeOffsite = document.getElementById('<%=radTypeOffsite.ClientID%>');
        oTypeManual = document.getElementById('<%=radTypeManual.ClientID%>');
        oBuildingCode = document.getElementById('<%=txtBuildingCode.ClientID%>');
        oServiceNow = document.getElementById('<%=txtServiceNow.ClientID%>');
        oRecovery = document.getElementById('<%=chkRecovery.ClientID%>');
        oVMwareIP = document.getElementById('<%=chkVMwareIP.ClientID%>');
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
		    <td><b>Location - Addresses</b></td>
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
                            <td class="default" width="100px">Factory Code:</td>
                            <td><asp:textbox ID="txtCode" CssClass="default" runat="server" Width="100" MaxLength="5"/></td>
                        </tr>
                        <tr>
                            <td class="default">City:</td>
                            <td><asp:label ID="lblParent" CssClass="default" runat="server" />&nbsp;&nbsp;<asp:Button ID="btnParent" runat="server" CssClass="default" Width="25" Text="..." /></td>
                        </tr>
                        <tr> 
                            <td class="default">Common:</td>
                            <td><asp:CheckBox ID="chkCommon" runat="server" /></td>
                        </tr>
                        <tr> 
                            <td class="default" width="100px">Common Name:</td>
                            <td><asp:textbox ID="txtCommonName" CssClass="default" runat="server" Width="200" MaxLength="50"/></td>
                        </tr>
                        <tr> 
                            <td class="default">Storage Available:</td>
                            <td><asp:CheckBox ID="chkStorage" runat="server" /></td>
                        </tr>
                        <tr> 
                            <td class="default">TSM Available:</td>
                            <td><asp:CheckBox ID="chkTSM" runat="server" /> (Otherwise, Legato will be used)</td>
                        </tr>
                        <tr> 
                            <td class="default">DR Location:</td>
                            <td><asp:CheckBox ID="chkDR" runat="server" /></td>
                        </tr>
                        <tr> 
                            <td class="default">Prod Location:</td>
                            <td><asp:CheckBox ID="chkProd" runat="server" /></td>
                        </tr>
                        <tr> 
                            <td class="default">QA Location:</td>
                            <td><asp:CheckBox ID="chkQA" runat="server" /></td>
                        </tr>
                        <tr> 
                            <td class="default">Test/Dev Location:</td>
                            <td><asp:CheckBox ID="chkTest" runat="server" /></td>
                        </tr>
                        <tr> 
                            <td class="default" valign="top">Build Location:</td>
                            <td>
                                <asp:RadioButton ID="radTypeDefault" runat="server" Text="Inherit Automated Build Location Configuration" GroupName="type" /><br />
                                <asp:RadioButton ID="radTypeOffsite" runat="server" Text="Use Offsite Automated Build Location" GroupName="type" /><br />
                                <asp:RadioButton ID="radTypeManual" runat="server" Text="Manual Build" GroupName="type" /><br />
                            </td>
                        </tr>
                        <tr> 
                            <td class="default">Building Code:</td>
                            <td><asp:textbox ID="txtBuildingCode" CssClass="default" runat="server" Width="150" MaxLength="20"/></td>
                        </tr>
                        <tr> 
                            <td class="default">Service Now Code:</td>
                            <td><asp:textbox ID="txtServiceNow" CssClass="default" runat="server" Width="150" MaxLength="20"/></td>
                        </tr>
                        <tr> 
                            <td class="default">Recovery Location:</td>
                            <td><asp:CheckBox ID="chkRecovery" runat="server" /></td>
                        </tr>
                        <tr> 
                            <td class="default">Assign Build IP in VMware:</td>
                            <td><asp:CheckBox ID="chkVMwareIP" runat="server" /></td>
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
