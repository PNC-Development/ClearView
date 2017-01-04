<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="fore_first.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.fore_first" %>
<script type="text/javascript">
    function EnableMaintenance() {
        var oClass = document.getElementById('<%=ddlClass.ClientID %>');
        var oMaintenance = document.getElementById('<%=ddlMaintenance.ClientID %>');
        var oPlatform = document.getElementById('<%=ddlPlatform.ClientID %>');
        if (oPlatform == null) {
            oPlatform = document.getElementById('<%=lblPlatform.ClientID %>');
            if (oPlatform.innerText.toUpperCase() == "SERVER" && (oClass.options[oClass.selectedIndex].text.toUpperCase().indexOf("PRODUCTION") > 0 || oClass.options[oClass.selectedIndex].text.toUpperCase().indexOf("QA") > 0 || oClass.options[oClass.selectedIndex].text.toUpperCase().indexOf("TEST") > 0)) {
                oMaintenance.disabled = false;
                oMaintenance.options[0].text = "-- SELECT --";
            }
            else {
                oMaintenance.disabled = true;
                oMaintenance.options[0].text = "-- NONE --";
                oMaintenance.selectedIndex = 0;
            }
        }
        else {
            if (oPlatform.options[oPlatform.selectedIndex].text.toUpperCase() == "SERVER" && (oClass.options[oClass.selectedIndex].text.toUpperCase().indexOf("PRODUCTION") > 0 || oClass.options[oClass.selectedIndex].text.toUpperCase().indexOf("QA") > 0 || oClass.options[oClass.selectedIndex].text.toUpperCase().indexOf("TEST") > 0)) {
                oMaintenance.disabled = false;
                oMaintenance.options[0].text = "-- SELECT --";
            }
            else {
                oMaintenance.disabled = true;
                oMaintenance.options[0].text = "-- NONE --";
                oMaintenance.selectedIndex = 0;
            }
        }
    }
    function EnsureMaintenance(oObject, strAlert) {
        var oClass = document.getElementById('<%=ddlClass.ClientID %>');
        var oPlatform = document.getElementById('<%=ddlPlatform.ClientID %>');
        if (oPlatform == null) {
            oPlatform = document.getElementById('<%=lblPlatform.ClientID %>');
            if (oPlatform.innerText.toUpperCase() == "SERVER" && oClass.options[oClass.selectedIndex].text.toUpperCase().indexOf("PRODUCTION") > 0) {
                if (ValidateDropDown(oObject, strAlert) == false)
                    return false;
            }
        }
        else {
            if (oPlatform.options[oPlatform.selectedIndex].text.toUpperCase() == "SERVER" && oClass.options[oClass.selectedIndex].text.toUpperCase().indexOf("PRODUCTION") > 0) {
                if (ValidateDropDown(oObject, strAlert) == false)
                    return false;
            }
        }
        return true;
    }
    function ShowTestOption(oObject) {
        var oObject = document.getElementById(oObject);
        var oClass = document.getElementById('<%=ddlClass.ClientID %>');
        if (oClass.options[oClass.selectedIndex].text.toUpperCase().indexOf("PRODUCTION") > 0 && oClass.options[oClass.selectedIndex].value != 7)
            oObject.style.display = "inline";
        else
            oObject.style.display = "none";
    }
    function ShowServerType(oObject) {
        var oObject = document.getElementById(oObject);
        var oPlatform = document.getElementById('<%=ddlPlatform.ClientID %>');
        if (oPlatform == null) {
            oPlatform = document.getElementById('<%=lblPlatform.ClientID %>');
            if (oPlatform.innerText.toUpperCase() == "SERVER")
                oObject.style.display = "inline";
            else
                oObject.style.display = "none";
        }
        else {
            if (oPlatform.options[oPlatform.selectedIndex].text.toUpperCase() == "SERVER")
                oObject.style.display = "inline";
            else
                oObject.style.display = "none";
        }
    }
    function ForeRecoveryFirst(oBool, oText, iRecovery) {
        var oText = document.getElementById(oText);
        var iQuantity = parseInt(oText.value);
        if (oBool == true && ValidateNumberGreaterNumbers(iRecovery, iQuantity, 'The quantity must be greater than the number of recovery servers.\n\nNumber of Recovery Servers: ' + iRecovery) == false)
            return false;
        return true;
    }
    function EnsureOverride(oY, oN, oBY, oBN, oC) {
        var oY = document.getElementById(oY);
        var oN = document.getElementById(oN);
        if (oY.checked == false && oN.checked == false) {
            alert('Please select whether or not you want to override the selection matrix');
            oY.focus();
            return false;
        }
        else if (oY.checked == true) {
            var oBY = document.getElementById(oBY);
            var oBN = document.getElementById(oBN);
            if (oBY.checked == false && oBN.checked == false) {
                alert('Please select whether or not this is related to a production break-fix situation');
                oBY.focus();
                return false;
            }
            else if (oBY.checked == true) {
                return ValidateTextLength(oC, 'Please enter a valid change control number\n\n - Must start with "CHG" or "PTM"\n - Must be exactly 10 characters in length', 10, 'CHG', 'PTM');
            }
        }
        return true;
    }
    function LockQuantityRadio(oY, oBY, oQ) {
        oY = document.getElementById(oY);
        oBY = document.getElementById(oBY);
        oQ = document.getElementById(oQ);
        var boolLock = false;
        if (oY.checked == true && oBY.checked == true)
            boolLock = true;
        if (boolLock == true) {
            oQ.value = "1";
            oQ.disabled = true;
        }
        else
            oQ.disabled = false;
    }
    function LockQuantity(oQ, boolLock) {
        oQ = document.getElementById(oQ);
        if (boolLock == true) {
            oQ.value = "1";
            oQ.readOnly = true;
        }
        else
            oQ.readOnly = false;
    }
    var oDeviceNameReturn = 0;
    var oDeviceName = null;
    var oActiveXDeviceName = null;
    function EnsureDeviceName(oY, oBY, oName, oClass) {
        oY = document.getElementById(oY);
        oBY = document.getElementById(oBY);
        if (oY.checked == true && oBY.checked == true) {
            oName = document.getElementById(oName);
            oClass = document.getElementById(oClass);
            oDeviceName = oName;
            oActiveXDeviceName = new ActiveXObject("Microsoft.XMLHTTP");
            oActiveXDeviceName.onreadystatechange = EnsureDeviceName_a;
            oActiveXDeviceName.open("GET", "/frame/ajax/ajax_servername.aspx?u=GET", false);
            oActiveXDeviceName.send("<ajax><value>" + escape(oName.value) + "</value><value>" + escape(oClass.options[oClass.selectedIndex].value) + "</value></ajax>");
            //return false;
            while (oDeviceNameReturn == 0) {
            }
            if (oDeviceNameReturn == -1)
                return false;
            else
                return true;
        }
        else
            return true;
    }
    function EnsureDeviceName_a() {
        if (oActiveXDeviceName.readyState == 4)
        {
            if (oActiveXDeviceName.status == 200) {
                var or = oActiveXDeviceName.responseXML.documentElement.childNodes;
                if (or[0].childNodes[0].text == "0") {
                    oText1.value = or[0].childNodes[0].text
                    oText2.value = or[0].childNodes[1].text
                    oDeviceNameReturn = -1;
                    alert('The device name you entered was not found. Please enter a valid device name.\n\nIf you think this device exists, please contact your ClearView administrator');
                    oDeviceName.focus();
                    return false;
                }
                else {
                    oDeviceNameReturn = 1;
                    //document.forms[0].submit();
                }
            }
            else 
                alert('There was a problem getting the information');
        }
    }
</script>
<table width="100%" cellpadding="4" cellspacing="2" border="0">
    <tr>
        <td colspan="2">
            <asp:Label ID="lblResiliency" runat="server" CssClass="default" Text="Is this design related to the Business Resiliency Effort?:" />
            <asp:RadioButton ID="radResiliencyYes" runat="server" CssClass="default" Text="Yes" GroupName="Resiliency" /> 
            <asp:RadioButton ID="radResiliencyNo" runat="server" CssClass="default" Text="No" GroupName="Resiliency" Checked="true" />
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <asp:Label ID="lblOverrideSelectionMatrix" runat="server" CssClass="default" Text="Override Selection Matrix:" />
            <asp:RadioButton ID="radYes" runat="server" CssClass="default" Text="Yes" GroupName="override" /> 
            <asp:RadioButton ID="radNo" runat="server" CssClass="default" Text="No" GroupName="override" />
        </td>
    </tr>
    <tr id="divOverride" runat="server" style="display:none">
        <td colspan="2">
            <asp:Label ID="lblProdBreakFix" runat="server" CssClass="default" Text="Is this related to a production break-fix issue:" />
            <asp:RadioButton ID="radBreakYes" runat="server" CssClass="default" Text="Yes" GroupName="breakfix" /> 
            <asp:RadioButton ID="radBreakNo" runat="server" CssClass="default" Text="No" GroupName="breakfix" />
        </td>
    </tr>
    <tr id="divChange" runat="server" style="display:none">
        <td nowrap><asp:Label ID="lblChangeControlNo" runat="server" CssClass="default" Text="Change Control #:<font class='required'>&nbsp;*</font>" /></td>
        <td width="100%"><asp:TextBox ID="txtChange" runat="server" CssClass="default" Width="100" MaxLength="10" /></td>
    </tr>
    <tr id="divName" runat="server" style="display:none">
        <td nowrap><asp:Label ID="lblDeviceName" runat="server" CssClass="default" Text="Device Name:<font class='required'>&nbsp;*</font>" /></td>
        <td width="100%"><asp:TextBox ID="txtDeviceName" runat="server" CssClass="default" Width="200" MaxLength="20" /></td>
    </tr>
    <tr>
        <td nowrap><asp:Label ID="lblPlatform1" runat="server" CssClass="default" Text="Platform:<font class='required'>&nbsp;*</font>" /></td>
        <td width="100%"><asp:DropDownList ID="ddlPlatform" runat="server" CssClass="default" Visible="false" Width="200" /><asp:Label ID="lblPlatform" runat="server" CssClass="default" /></td>
    </tr>
    <tr>
        <td nowrap><asp:Label ID="lblNickName" runat="server" CssClass="default" Text="Nickname:" /></td>
        <td width="100%"><asp:TextBox ID="txtName" runat="server" CssClass="default" Width="200" MaxLength="50" /></td>
    </tr>
    <tr>
        <td nowrap>
        <asp:Label ID="lblLocation" runat="server" CssClass="default" Text="Location:<font class='required'>&nbsp;*</font>" /></td>
        <td width="100%"><%=strLocation %></td>
    </tr>
    <tr>
        <td nowrap><asp:Label ID="lblClass" runat="server" CssClass="default" Text="Class:<font class='required'>&nbsp;*</font>" /></td>
        <td width="100%"><asp:DropDownList ID="ddlClass" CssClass="default" runat="server" Width="300" /></td>
    </tr>
    <tr id="divTest" runat="server" style="display:none">
        <td nowrap>&nbsp;</td>
        <td width="100%"><asp:CheckBox ID="chkTest" runat="server" CssClass="default" Text=" Will this device go through TEST first?" /></td>
    </tr>
    <tr>
        <td nowrap><asp:Label ID="lblEnvironment" runat="server" CssClass="default" Text="Environment:<font class='required'>&nbsp;*</font>" /></td>
        <td width="100%">
            <asp:DropDownList ID="ddlEnvironment" CssClass="default" runat="server" Width="300" Enabled="false" >
                <asp:ListItem Value="-- Please select a Class --" />
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td nowrap><asp:Label ID="lblMaintenanceWindow" runat="server" CssClass="default" Text="Maintenance Window:<font class='required'>&nbsp;*</font>" /></td>
        <td width="100%"><asp:DropDownList ID="ddlMaintenance" CssClass="default" runat="server" Enabled="false" /></td>
    </tr>
    <tr id="divServerType" runat="server" style="display:none">
        <td nowrap><asp:Label ID="lblServerType" runat="server" CssClass="default" Text="Server Type:" /></td>
        <td width="100%"><asp:DropDownList ID="ddlApplications" runat="server" CssClass="default" Width="300" />&nbsp;&nbsp;&nbsp;<img src="/images/alert.gif" border="0" align="absmiddle" /> <b>NOTE:</b> Server type selection is NOT required.</td>
    </tr>
    <tr id="divSubApplications" runat="server" style="display:none">
        <td nowrap><asp:Label ID="lblServerRole" runat="server" CssClass="default" Text="Server Role:<font class='required'>&nbsp;*</font>" /></td>
        <td width="100%"><asp:DropDownList ID="ddlSubApplications" runat="server" CssClass="default" Width="300" /></td>
    </tr>
    <tr>
        <td nowrap><asp:Label ID="lblQuantity" runat="server" CssClass="default" Text="Quantity:<font class='required'>&nbsp;*</font>" /></td>
        <td width="100%"><asp:TextBox ID="txtQuantity" runat="server" CssClass="default" Width="80" MaxLength="10" /></td>
    </tr>
</table>
<table width="100%" cellpadding="4" cellspacing="2" border="0">
    <tr>
        <td colspan="3"><hr size="1" noshade /></td>
    </tr>
    <tr>
        <td class="required">* = Required Field</td>
        <td align="center">
            <asp:Panel ID="panNavigation" runat="server" Visible="false">
                <asp:Button ID="btnBack" runat="server" OnClick="btnBack_Click" Text="Back" CssClass="default" Width="75" /> <asp:Button ID="btnNext" runat="server" OnClick="btnNext_Click" Text="Next" CssClass="default" Width="75" />
            </asp:Panel>
            <asp:Panel ID="panUpdate" runat="server" Visible="false">
                <asp:Button ID="btnUpdate" runat="server" OnClick="btnUpdate_Click" Text="Update" CssClass="default" Width="75" /> <asp:Button ID="btnCancel" runat="server" OnClick="btnCancel_Click" Text="Cancel" CssClass="default" Width="75" />
            </asp:Panel>
        </td>
        <td align="right">
            <asp:Button ID="btnHundred" runat="server" OnClick="btnCancel_Click" Text="Back" CssClass="default" Width="75" Visible="false" /> 
            <asp:Button ID="btnClose" runat="server" Text="Close" CssClass="default" Width="75" />
        </td>
    </tr>
</table>
<input type="hidden" id="hdnLocation" runat="server" />
<input type="hidden" id="hdnEnvironment" runat="server" />
<input type="hidden" id="hdnSubApplication" runat="server" />
