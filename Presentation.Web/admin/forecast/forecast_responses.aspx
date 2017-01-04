<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="forecast_responses.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.forecast_responses" %>

<script type="text/javascript">
    var oAdd = null;
    var oView = null;
    var oAddButton = null;
    var oDeleteButton = null;
    var oHidden = null;
    var oParent = null;
    var oParentId = null;
    var oName = null;
    var oComponents = null;
    var oParentId = null;
    var oParent = null;
    var oVariance = null;
    var oResponseId = null;
    var oResponseLbl = null;
    var oResponse = null;
    var oResponseCategory = null;
    var oEnabled = null;
    var oOSDistributed = null;
    var oOSMidrange = null;
    var oCores = null;
    var oRAM = null;
    var oWeb = null;
    var oDbase = null;
    var oHANone = null;
    var oHACluster = null;
    var oHACSM = null;
    var oHACSMMiddleware = null;
    var oHACSMApp = null;
    var oHARoom = null;
    var oDRUnder = null;
    var oDROver = null;
    var oDROne = null;
    var oDRMany = null;
    function Edit(strId, strParentId, strParent, strName, strResponse, strComponents, strVariance, strResponseId, strResponseLbl, strResponseCategory, strEnabled, strOSDistributed, strOSMidrange, strCores, strRAM, strWeb, strDbase, strHANone, strHACluster, strHACSM, strHACSMMiddleware, strHACSMApp, strHARoom, strDRUnder, strDROver, strDROne, strDRMany) {
        oAdd.style.display = "inline";
        oView.style.display = "none";
        oAddButton.value = "Update";
        oDeleteButton.style.display = "inline";
        oHidden.value = strId;
        oParentId.value = strParentId;
        if (strParent == "" || strParentId == "0")
            oParent.innerText = "No Parent";
        else
            oParent.innerText = strParent;
        oName.value = strName;
        oComponents.value = strComponents;
        oVariance.checked = (strVariance == "1");
        oResponseId.value = strResponseId;
        if (strResponseLbl == "" || strResponseId == "0")
            oResponseLbl.innerText = "No Response";
        else
            oResponseLbl.innerText = strResponseLbl;
        oResponse.value = strResponse;
      
        for (var ii=0; ii<oResponseCategory.length; ii++) 
        {
            if (oResponseCategory.options[ii].value == strResponseCategory)
                oResponseCategory.selectedIndex = ii;
        }
        
        oEnabled.checked = (strEnabled == "1");
        oOSDistributed.checked = (strOSDistributed == "1");
        oOSMidrange.checked = (strOSMidrange == "1");
        oCores.checked = (strCores == "1");
        oRAM.checked = (strRAM == "1");
        oWeb.checked = (strWeb == "1");
        oDbase.checked = (strDbase == "1");
        oHANone.checked = (strHANone == "1");
        oHACluster.checked = (strHACluster == "1");
        oHACSM.checked = (strHACSM == "1");
        oHACSMMiddleware.checked = (strHACSMMiddleware == "1");
        oHACSMApp.checked = (strHACSMApp == "1");
        oHARoom.checked = (strHARoom == "1");
        oDRUnder.checked = (strDRUnder == "1");
        oDROver.checked = (strDROver == "1");
        oDROne.checked = (strDROne == "1");
        oDRMany.checked = (strDRMany == "1");
    }
    function Add(strParentId, strParent) {
        oAdd.style.display = "inline";
        oView.style.display = "none";
        oAddButton.value = "Add";
        oDeleteButton.style.display = "none";
        oHidden.value = "0";
        oParentId.value = strParentId;
        if (strParent == "" || strParent == "0")
            oParent.innerText = "No Parent";
        else
            oParent.innerText = strParent;
        oName.value = "";
        oComponents.value = "";
        oVariance.checked = false;
        oResponseId.value = "0";
        oResponseLbl.innerText = "No Response";
        oResponse.value = "";
        strResponseCategory="0";
        for (var ii=0; ii<oResponseCategory.length; ii++) 
        {
            if (oResponseCategory.options[ii].value == strResponseCategory)
                oResponseCategory.selectedIndex = ii;
        }
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
        oParent = document.getElementById('<%=lblParent.ClientID%>');
        oParentId = document.getElementById('<%=hdnParent.ClientID%>');
        oName = document.getElementById('<%=txtName.ClientID%>');
        oComponents = document.getElementById('<%=txtComponents.ClientID%>');
        oResponseLbl = document.getElementById('<%=lblResponse.ClientID%>');
        oVariance = document.getElementById('<%=chkVariance.ClientID%>');
        oResponseId = document.getElementById('<%=hdnResponse.ClientID%>');
        oResponse = document.getElementById('<%=txtResponse.ClientID%>');
        oResponseCategory = document.getElementById('<%=ddlResponseCategory.ClientID%>');
        oEnabled = document.getElementById('<%=chkEnabled.ClientID%>');
        oOSDistributed = document.getElementById('<%=chkOSDistributed.ClientID%>');
        oOSMidrange = document.getElementById('<%=chkOSMidrange.ClientID%>');
        oCores = document.getElementById('<%=chkCores.ClientID%>');
        oRAM = document.getElementById('<%=chkRAM.ClientID%>');
        oWeb = document.getElementById('<%=chkWeb.ClientID%>');
        oDbase = document.getElementById('<%=chkDatabase.ClientID%>');
        oHANone = document.getElementById('<%=chkHANone.ClientID%>');
        oHACluster = document.getElementById('<%=chkHACluster.ClientID%>');
        oHACSM = document.getElementById('<%=chkHACSM.ClientID%>');
        oHACSMMiddleware = document.getElementById('<%=chkHACSMMiddleware.ClientID%>');
        oHACSMApp = document.getElementById('<%=chkHACSMApp.ClientID%>');
        oHARoom = document.getElementById('<%=chkHARoom.ClientID%>');
        oDRUnder = document.getElementById('<%=chkDRUnder.ClientID%>');
        oDROver = document.getElementById('<%=chkDROver.ClientID%>');
        oDROne = document.getElementById('<%=chkDROne.ClientID%>');
        oDRMany = document.getElementById('<%=chkDRMany.ClientID%>');
    }
 </script>
<html>
<head>
<title>ClearView Administration</title>
<link rel="stylesheet" type="text/css" href="/css/default.css" />
<script type="text/javascript" src="/javascript/global.js"></script>
<script type="text/javascript" src="/javascript/ajax.js"></script>
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
		    <td><b>Forecast Responses</b></td>
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
                            <td class="default">Question:</td>
                            <td><asp:label ID="lblParent" CssClass="default" runat="server" />&nbsp;&nbsp;<asp:Button ID="btnParent" runat="server" CssClass="default" Width="25" Text="..." /></td>
                        </tr>
                        <tr> 
                            <td class="default" width="100px">Name:</td>
                            <td><asp:textbox ID="txtName" CssClass="default" runat="server" Width="400" MaxLength="100"/></td>
                        </tr>
                        <tr> 
                            <td class="default" width="100px">Response:</td>
                            <td><asp:textbox ID="txtResponse" CssClass="default" runat="server" Width="400" Rows="7" TextMode="MultiLine"/></td>
                        </tr>
                        <tr> 
                            <td class="default" width="100px">Components:</td>
                            <td><asp:textbox ID="txtComponents" CssClass="default" runat="server" Width="200" MaxLength="50"/> [Split with &quot;,&quot;]</td>
                        </tr>
                        <tr> 
                            <td class="default">Variance:</td>
                            <td><asp:CheckBox ID="chkVariance" runat="server" Checked="true" /></td>
                        </tr>
                        <tr>
                            <td class="default">Custom Response:</td>
                            <td><asp:label ID="lblResponse" CssClass="default" runat="server" />&nbsp;&nbsp;<asp:Button ID="btnResponse" runat="server" CssClass="default" Width="25" Text="..." /></td>
                        </tr>
                         <tr>
                            <td class="default">Response Category:</td>
                            <td><asp:dropdownlist ID="ddlResponseCategory" CssClass="default" runat="server" Width="300"/></td>
                        </tr>
                        <tr> 
                            <td class="default">Enabled:</td>
                            <td><asp:CheckBox ID="chkEnabled" runat="server" Checked="true" /></td>
                        </tr>
                        <tr><td height="5" colspan="2"><img src="images/spacer.gif" width="1" height="5" /></td></tr>
                        <tr> 
                            <td class="cmdefault">&nbsp;</td>
                            <td><asp:Button ID="btnOrder" runat="server" Text="Change Order" Width="150" CssClass="default" /></td>
                        </tr>
                        <tr> 
                            <td class="cmdefault">&nbsp;</td>
                            <td><asp:Button ID="btnSelection" runat="server" Text="Edit Selections" Width="150" CssClass="default" /></td>
                        </tr>
                        <tr> 
                            <td class="cmdefault">&nbsp;</td>
                            <td><asp:Button ID="btnAdditional" runat="server" Text="Additional Configuration" Width="150" CssClass="default" /></td>
                        </tr>
                        <tr>
                            <td colspan="2">&nbsp;</td>
                        </tr>
                        <tr> 
                            <td colspan="2" bgcolor="#F6F6F6" style="border:solid 1px #CCCCCC">
                                <table cellpadding="2" cellspacing="2" border="0">
                                    <tr>
                                        <td colspan="10"><b>Special Configuration</b></td>
                                    </tr>
                                    <tr>
                                        <td valign="top">
                                            <table width="100%" cellpadding="2" cellspacing="2" border="0">
                                                <tr>
                                                    <td nowrap>Distibuted OS:</td>
                                                    <td width="100%"><asp:CheckBox ID="chkOSDistributed" runat="server" CssClass="default" /></td>
                                                </tr>
                                                <tr>
                                                    <td nowrap>Midrange OS:</td>
                                                    <td width="100%"><asp:CheckBox ID="chkOSMidrange" runat="server" CssClass="default" /></td>
                                                </tr>
                                                <tr>
                                                    <td nowrap>Cores:</td>
                                                    <td width="100%"><asp:CheckBox ID="chkCores" runat="server" CssClass="default" /></td>
                                                </tr>
                                                <tr>
                                                    <td nowrap>RAM:</td>
                                                    <td width="100%"><asp:CheckBox ID="chkRAM" runat="server" CssClass="default" /></td>
                                                </tr>
                                                <tr>
                                                    <td nowrap>Web Software:</td>
                                                    <td width="100%"><asp:CheckBox ID="chkWeb" runat="server" CssClass="default" /></td>
                                                </tr>
                                                <tr>
                                                    <td nowrap>Database Software:</td>
                                                    <td width="100%"><asp:CheckBox ID="chkDatabase" runat="server" CssClass="default" /></td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
                                        <td valign="top">
                                            <table width="100%" cellpadding="2" cellspacing="2" border="0">
                                                <tr>
                                                    <td nowrap>High Availability - By Room:</td>
                                                    <td width="100%"><asp:CheckBox ID="chkHARoom" runat="server" CssClass="default" /></td>
                                                </tr>
                                                <tr>
                                                    <td nowrap>High Availability - None:</td>
                                                    <td width="100%"><asp:CheckBox ID="chkHANone" runat="server" CssClass="default" /></td>
                                                </tr>
                                                <tr>
                                                    <td nowrap>High Availability - Cluster:</td>
                                                    <td width="100%"><asp:CheckBox ID="chkHACluster" runat="server" CssClass="default" /></td>
                                                </tr>
                                                <tr>
                                                    <td nowrap>High Availability - CSM:</td>
                                                    <td width="100%"><asp:CheckBox ID="chkHACSM" runat="server" CssClass="default" /></td>
                                                </tr>
                                                <tr>
                                                    <td nowrap>High Availability - CSM (Middleware):</td>
                                                    <td width="100%"><asp:CheckBox ID="chkHACSMMiddleware" runat="server" CssClass="default" /></td>
                                                </tr>
                                                <tr>
                                                    <td nowrap>High Availability - CSM (Application):</td>
                                                    <td width="100%"><asp:CheckBox ID="chkHACSMApp" runat="server" CssClass="default" /></td>
                                                </tr>
                                             </table>
                                        </td>
                                        <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
                                        <td valign="top">
                                            <table width="100%" cellpadding="2" cellspacing="2" border="0">
                                                <tr>
                                                    <td nowrap>DR - Under 48 hours:</td>
                                                    <td width="100%"><asp:CheckBox ID="chkDRUnder" runat="server" CssClass="default" /></td>
                                                </tr>
                                                <tr>
                                                    <td nowrap>DR - Over 48 hours:</td>
                                                    <td width="100%"><asp:CheckBox ID="chkDROver" runat="server" CssClass="default" /></td>
                                                </tr>
                                                <tr>
                                                    <td nowrap>DR - One to One Recovery:</td>
                                                    <td width="100%"><asp:CheckBox ID="chkDROne" runat="server" CssClass="default" /></td>
                                                </tr>
                                                <tr>
                                                    <td nowrap>DR - Many to One Recovery:</td>
                                                    <td width="100%"><asp:CheckBox ID="chkDRMany" runat="server" CssClass="default" /></td>
                                                </tr>
                                             </table>
                                        </td>
                                    </tr>
                               </table>
                            </td>
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
    <input type="hidden" id="hdnResponse" runat="server" />
    <input type="hidden" id="hdnOrder" runat="server" />
    <asp:HiddenField ID="hdnUser" runat="server" />
</form>
</body>
</html>
