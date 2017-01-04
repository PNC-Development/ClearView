<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ip_address_controls.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.ip_address_controls" %>


<script type="text/javascript">
    function LocationFunction() {
        var strUrl = window.location.href;
        var oHidden = document.getElementById('<%=hdnParent.ClientID %>');
        if (strUrl.indexOf("?") > -1) {
            strQueryCheck = strUrl.substring(strUrl.indexOf("?"));
            strQueryUrl = strUrl.substring(0, strUrl.indexOf("?"));
            RemoveQuerystring("aid", oHidden.value);
            window.top.location.href = (strQueryUrl + strQueryCheck);
        }
        else {
            window.top.location.href = (strUrl + "?aid=" + oHidden.value);
        }
    }
    function CheckBlade(oDDL, oDivHp, oDivHp2, oDivSun, oDivSun2) {
        oDivHp = document.getElementById(oDivHp);
        oDivHp2 = document.getElementById(oDivHp2);
        oDivSun = document.getElementById(oDivSun);
        oDivSun2 = document.getElementById(oDivSun2);
        if (oDDL.options[oDDL.selectedIndex].text == "HP Blade") {
            oDivHp.style.display = "inline";
            oDivHp2.style.display = "inline";
            oDivSun.style.display = "none";
            oDivSun2.style.display = "none";
        }
        else if (oDDL.options[oDDL.selectedIndex].text == "SUN Blade") {
            oDivHp.style.display = "none";
            oDivHp2.style.display = "none";
            oDivSun.style.display = "inline";
            oDivSun2.style.display = "inline";
        }
        else {
            oDivHp.style.display = "none";
            oDivHp2.style.display = "none";
            oDivSun.style.display = "none";
            oDivSun2.style.display = "none";
        }
    }
    function Ensure(radCSMVip, radCluster, radILO, radServer, radAdditional, txtURL, hdnProject, txtProject, txtInstance, ddlVlan, txtSerial, radSingleServer, radClusterWindows, radClusterUnixRAC, radClusterUnixOther, radClusterVCS, oClusterModel, oClusterModelDDL, txtServer, radCSM, radLTM, radNone, ddlCSMVlan, ddlLTMVlan, ddlType, ddlBladeHpVlan, ddlBladeSunVlan, ddlHardware, ddlClusterBladeHp, ddlClusterBladeSun, hdnNodes, txtNode, txtAdditional, ddlAdditionalVlan, oSendingDiv, oSendingModel, oSendingModelDDL, oSendingRoom, oSendingRack) {
        radCSMVip = document.getElementById(radCSMVip);
        radCluster = document.getElementById(radCluster);
        radILO = document.getElementById(radILO);
        radServer = document.getElementById(radServer);
        radAdditional = document.getElementById(radAdditional);
        if (radCSMVip.checked == false && radCluster.checked == false && radILO.checked == false && radServer.checked == false && radAdditional.checked == false) {
            alert('Please select a type of IP address');
            radCSMVip.focus();
            return false;
        }
        else {
            if (radCSMVip.checked == true) {
                txtURL = document.getElementById(txtURL);
                if (txtURL != null && trim(txtURL.value) == "") {
                    if (ValidateHidden(hdnProject,txtProject,'Please enter a project number or URL') == false)
                        return false;
                }
            }
            else if (radCluster.checked == true) {
                if (ValidateText(txtInstance,'Please enter a name for the instance') == false || ValidateDropDown(ddlVlan,'Please select a VLAN') == false)
                    return false;
            }
            else if (radILO.checked == true) {
                if (ValidateText(txtSerial,'Please enter a serial number') == false)
                    return false;
            }
            else if (radServer.checked == true) {
                radSingleServer = document.getElementById(radSingleServer);
                radClusterWindows = document.getElementById(radClusterWindows);
                radClusterUnixRAC = document.getElementById(radClusterUnixRAC);
                radClusterUnixOther = document.getElementById(radClusterUnixOther);
                radClusterVCS = document.getElementById(radClusterVCS);
                if (radSingleServer.checked == false && radClusterWindows.checked == false && radClusterUnixRAC.checked == false && radClusterUnixOther.checked == false && radClusterVCS.checked == false) {
                    alert('Please select your clustering options');
                    radSingleServer.focus();
                    return false;
                }
                if (radSingleServer.checked == true) {
                    if ((ValidateText(txtServer,'Please enter a server name') == false))
                        return false;
                    else {
                        radCSM = document.getElementById(radCSM);
                        radLTM = document.getElementById(radLTM);
                        radNone = document.getElementById(radNone);
                        if (radCSM.checked == false && radLTM.checked == false && radNone.checked == false) {
                            alert('Please select your load balancing method');
                            radCSM.focus();
                            return false;
                        }
                        else if (radCSM.checked == true) {
                            if (ValidateDropDown(ddlCSMVlan,'Please select a VLAN') == false)
                                return false;
                        }
                        else if (radLTM.checked == true) {
                            if (ValidateDropDown(ddlLTMVlan,'Please select a VLAN') == false)
                                return false;
                        }
                        else if (radNone.checked == true) {
                            if (ValidateDropDown(ddlType,'Please select a server type') == false)
                                return false;
                            else {
                                ddlType = document.getElementById(ddlType);
                                if (ddlType.options[ddlType.selectedIndex].text == "HP Blade") {
                                    if (ValidateDropDown(ddlBladeHpVlan,'Please select a VLAN') == false)
                                        return false;
                                }
                                if (ddlType.options[ddlType.selectedIndex].text == "SUN Blade") {
                                    if (ValidateDropDown(ddlBladeSunVlan,'Please select a VLAN') == false)
                                        return false;
                                }
                            }
                        }
                    }
                }
                else {
                    if (ValidateHidden0(oClusterModel,oClusterModelDDL,'Please select a Model') == false)
                        return false;
                }
                if (radClusterWindows.checked == true) {
                    if (ValidateDropDown(ddlHardware,'Please select a hardware type') == false)
                        return false;
                    else {
                        ddlHardware = document.getElementById(ddlHardware);
                        if (ddlHardware.options[ddlHardware.selectedIndex].text == "HP Blade") {
                            if (ValidateDropDown(ddlClusterBladeHp,'Please select a VLAN') == false)
                                return false;
                            else {
                                if (ValidateHidden(hdnNodes,txtNode,'Please add at least one node') == false)
                                    return false;
                            }
                        }
                        else if (ddlHardware.options[ddlHardware.selectedIndex].text == "SUN Blade") {
                            if (ValidateDropDown(ddlClusterBladeSun,'Please select a VLAN') == false)
                                return false;
                            else {
                                if (ValidateHidden(hdnNodes,txtNode,'Please add at least one node') == false)
                                    return false;
                            }
                        }
                        else {
                            if (ValidateHidden(hdnNodes,txtNode,'Please add at least one node') == false)
                                return false;
                        }
                    }
                }
                else if (radClusterUnixRAC.checked == true || radClusterUnixOther.checked == true || radClusterVCS.checked == true) {
                    if (ValidateHidden(hdnNodes,txtNode,'Please add at least one node') == false)
                        return false;
                }
            }
            if (radAdditional.checked == true) {
                if (ValidateText(txtAdditional,'Please enter a server name') == false || ValidateDropDown(ddlAdditionalVlan,'Please select a VLAN') == false)
                    return false;
            }
        }
        oSendingDiv = document.getElementById(oSendingDiv);
        if (oSendingDiv.style.display == "inline") {
            if (ValidateHidden0(oSendingModel,oSendingModelDDL,'Please select a Model') == false)
                return false;
            if (ValidateText(oSendingRoom,'Please enter a Room') == false)
                return false;
            if (ValidateText(oSendingRack,'Please enter a Rack') == false)
                return false;
        }
        return true;
    }
    function AddListBoxItem(oName, oRoom, oRack, oEnclosure, oSlot, oList, oHidden) {
        if (ValidateText(oName,'Please enter the name of the node') == false)
            return false;
        if (ValidateText(oRoom,'Please enter the room of the node') == false)
            return false;
        if (ValidateText(oRack,'Please enter the rack of the node') == false)
            return false;
        oList = document.getElementById(oList);
        if (oList.length >= 16)
            alert('You cannot add more than 16 nodes to a cluster');
        else {
            oName = document.getElementById(oName);
            oRoom = document.getElementById(oRoom);
            oRack = document.getElementById(oRack);
            oEnclosure = document.getElementById(oEnclosure);
            oSlot = document.getElementById(oSlot);
		    var oOption = document.createElement("OPTION");
		    oList.add(oOption);
		    oOption.text = oName.value;
		    oOption.value = oName.value + "_" + oRoom.value + "_" + oRack.value + "_" + oEnclosure.value + "_" + oSlot.value;
		    oList.selectedIndex = oList.length - 1;
    	    UpdateListBoxItem(oList, oHidden);
		    oName.value = "";
		    oRoom.value = "";
		    oRack.value = "";
		    oEnclosure.value = "";
		    oSlot.value = "";
		}
		oName.focus();
		return false;
    }
    function UpdateListBoxItem(oList, oHidden) {
        oHidden = document.getElementById(oHidden);
        oHidden.value = "";
        for (var ii=0; ii<oList.length; ii++)
	        oHidden.value = oHidden.value + oList.options[ii].value + "&";
    }
    function RemoveListBoxItem(oList, oHidden) {
        oList = document.getElementById(oList);
    	if (oList.selectedIndex > -1)
	    	oList.remove(oList.selectedIndex);
	    UpdateListBoxItem(oList, oHidden);
		return false;
    }
</script>
<table width="100%" cellpadding="0" cellspacing="0" border="0" class="default">
    <tr>
        <td><img src="/images/table_topleft.gif" border="0" width="5" height="26"></td>
        <td nowrap background="/images/table_top.gif" class="greentableheader" width="100%"><asp:Label ID="lblTitle" runat="server" CssClass="greentableheader" /></td>
        <td><img src="/images/table_topright.gif" border="0" width="5" height="26"></td>
    </tr>
    <tr>
        <td background="/images/table_left.gif"><img src="/images/table_left.gif" width="5" height="10"></td>
        <td width="100%" bgcolor="#FFFFFF">
            <br />
            <table width="100%" cellpadding="0" cellspacing="5" border="0">
                <tr>
                    <td rowspan="2"><img src="/images/ipaddress.gif" border="0" align="absmiddle" /></td>
                    <td class="header" width="100%" valign="bottom">IP Address</td>
                </tr>
                <tr>
                    <td width="100%" valign="top">Generate your own IP address by completing the following form and clicking <b>Generate</b>.</td>
                </tr>
            </table>
            <asp:Panel ID="panFailed" runat="server" Visible="false">
                <br />
                <table width="100%" cellpadding="0" cellspacing="5" border="0" style="border:solid 1px #999999" bgcolor="#f9f9f9">
                    <tr>
                        <td rowspan="2"><img src="/images/ico_error.gif" border="0" align="absmiddle" /></td>
                        <td class="header" width="100%" valign="bottom">IP Address Generation Failed!</td>
                    </tr>
                    <tr>
                        <td width="100%" valign="top">There was a problem generating your IP addresses. This is normally caused by one of two reasons...</td>
                    </tr>
                    <tr>
                        <td></td>
                        <td width="100%">1.) All available IP addresses are currently in use. (A new VLAN must be created)</td>
                    </tr>
                    <tr>
                        <td></td>
                        <td width="100%">2.) ClearView IP address configuration needs to be updated.</td>
                    </tr>
                    <tr>
                        <td colspan="2">&nbsp;</td>
                    </tr>
                    <tr>
                        <td></td>
                        <td width="100%">Please notify a clearview administrator of the issue by clicking the <span class="help_box"><img src="/images/help_box.gif" border="0" align="absmiddle" /> SUPPORT</span> button link from the top navigation.</td>
                    </tr>
                </table>
                <br />
            </asp:Panel>
            <asp:Panel ID="panDelete" runat="server" Visible="false">
                <br />
                <table width="100%" cellpadding="0" cellspacing="5" border="0" style="border:solid 1px #999999" bgcolor="#f9f9f9">
                    <tr>
                        <td rowspan="2"><img src="/images/hugeAlert.gif" border="0" align="absmiddle" /></td>
                        <td class="header" width="100%" valign="bottom">IP Address Generation Incomplete!</td>
                    </tr>
                    <tr>
                        <td width="100%" valign="top">There was a problem completing your IP addresses.</td>
                    </tr>
                    <tr>
                        <td></td>
                        <td width="100%">Some of your IP addresses were generated, but others were not. To complete this request, configuration must be made to the IP address database.</td>
                    </tr>
                    <tr>
                        <td colspan="2">&nbsp;</td>
                    </tr>
                    <tr>
                        <td></td>
                        <td width="100%">Please notify a clearview administrator of the issue by clicking the <span class="help_box"><img src="/images/help_box.gif" border="0" align="absmiddle" /> SUPPORT</span> button link from the top navigation.</td>
                    </tr>
                </table>
                <br />
            </asp:Panel>
            <asp:Panel ID="panName" runat="server" Visible="false">
                <br />
                <table width="100%" cellpadding="0" cellspacing="5" border="0" style="border:solid 1px #999999" bgcolor="#f9f9f9">
                    <tr>
                        <td rowspan="2"><img src="/images/ico_check.gif" border="0" align="absmiddle" /></td>
                        <td class="header" width="100%" valign="bottom">IP Address Generation Completed Successfully!</td>
                    </tr>
                    <tr>
                        <td width="100%" valign="top">Here are your ip address(es) based on your configuration. Click <b>Duplicate</b> to have ClearView run this configuration again.</td>
                    </tr>
                    <tr>
                        <td></td>
                        <td><asp:Label ID="lblName" runat="server" CssClass="header" /></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td><img src="/images/bigCheck.gif" border="0" align="absmiddle" /> <b>NOTE:</b> The address(es) above have been registered in DNS. (Please allow a few hours to propagate)</td>
                    </tr>
                    <tr>
                        <td></td>
                        <td><asp:Label ID="lblDNS" runat="server" CssClass="bold" /></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td><asp:Button ID="btnDuplicate" runat="server" CssClass="default" Text="Duplicate" Width="100" OnClick="btnDuplicate_Click" /></td>
                    </tr>
                </table>
                <br />
            </asp:Panel>
            <table width="100%" cellpadding="4" cellspacing="3" border="0">
                <tr>
                    <td nowrap>Popular Locations:<font class="required">&nbsp;*</font></td>
                    <td width="100%">
                        <table cellpadding="0" cellspacing="0" border="0">
                            <tr>
                                <td><asp:DropDownList ID="ddlLocation" runat="server" CssClass="default" Width="300" AutoPostBack="true" OnSelectedIndexChanged="ddlLocation_Change" /></td>
                                <td class="bold">
                                    <div id="divLocation" runat="server" style="display:none">
                                        <img src="/images/spacer.gif" border="0" width="20" height="1" /><img src="/images/please_wait.gif" border="0" align="absmiddle" /> Please Wait...
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <asp:Panel ID="panLocation" runat="server" Visible="false">
                <tr>
                    <td nowrap>Custom Location:<font class="required">&nbsp;*</font></td>
                    <td width="100%"><asp:TextBox ID="txtParent" CssClass="lightdefault" runat="server" Text="" Width="400" ReadOnly="true" />&nbsp;&nbsp;<asp:Button ID="btnParent" runat="server" CssClass="default" Width="25" Text="..." /> <span class="lightdefault"><img src="/images/hand_left.gif" border="0" align="absmiddle" /> Click here to select a value</span></td>
                </tr>
                </asp:Panel>
                <tr>
                    <td nowrap>Class:<font class="required">&nbsp;*</font></td>
                    <td width="100%">
                        <table cellpadding="0" cellspacing="0" border="0">
                            <tr>
                                <td><asp:DropDownList ID="ddlClass" runat="server" CssClass="default" Width="300" AutoPostBack="true" OnSelectedIndexChanged="ddlClass_Change" /></td>
                                <td class="bold">
                                    <div id="divClass" runat="server" style="display:none">
                                        <img src="/images/spacer.gif" border="0" width="20" height="1" /><img src="/images/please_wait.gif" border="0" align="absmiddle" /> Please Wait...
                                    </div>
                                </td>
                                <td><asp:Label ID="lblConfig" runat="server" CssClass="reddefault" Text="<img src='/images/spacer.gif' border='0' width='20' height='1' /><img src='/images/alert.gif' border='0' align='absmiddle' /> There is no information for this location and class" Visible="false" /></td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td nowrap>Environment:<font class="required">&nbsp;*</font></td>
                    <td width="100%">
                        <table cellpadding="0" cellspacing="0" border="0">
                            <tr>
                                <td><asp:DropDownList ID="ddlEnvironment" runat="server" CssClass="default" Width="300" AutoPostBack="true" OnSelectedIndexChanged="ddlEnvironment_Change" /></td>
                                <td class="bold">
                                    <div id="divEnvironment" runat="server" style="display:none">
                                        <img src="/images/spacer.gif" border="0" width="20" height="1" /><img src="/images/please_wait.gif" border="0" align="absmiddle" /> Please Wait...
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="2"><span style="width:100%;border-bottom:1 dotted #CCCCCC;"/><br /></td>
                </tr>
                <tr>
                    <td colspan="2"><img src="/images/spacer.gif" border="0" width="1" height="1" /></td>
                </tr>
            </table>
            <table width="100%" cellpadding="4" cellspacing="3" border="0">
                <tr>
                    <td colspan="2">Please select the type of IP address you would like to request:</td>
                </tr>
                <tr>
                    <td><img src="/images/spacer.gif" border="0" width="15" height="1" /></td>
                    <td width="100%"><asp:RadioButton ID="radCSMVip" runat="server" CssClass="default" Text="CSM VIP" GroupName="type" Enabled="false" /></td>
                </tr>
                <tr id="divCSMVip" runat="server" style="display:none">
                    <td></td>
                    <td style="border:solid 1px #EAEAEA">
                        <table width="100%" cellpadding="4" cellspacing="3" border="0">
                            <tr>
                                <td nowrap>Project:<font class="required">&nbsp;*</font></td>
                                <td width="100%">
                                    <table cellpadding="0" cellspacing="0" border="0">
                                        <tr>
                                            <td><asp:TextBox ID="txtProject" runat="server" Width="300" CssClass="default" /></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div id="divProject" runat="server" style="overflow:hidden; position:absolute; display:none; background-color:#FFFFFF; border:solid 1px #CCCCCC">
                                                    <asp:ListBox ID="lstProject" runat="server" CssClass="default" />
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td nowrap>&nbsp;</td>
                                <td width="100%" class="bold">-- OR --</td>
                            </tr>
                            <tr>
                                <td nowrap>URL:<font class="required">&nbsp;*</font></td>
                                <td width="100%"><asp:TextBox ID="txtURL" runat="server" CssClass="default" Width="400" MaxLength="100" /></td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td width="100%"><asp:RadioButton ID="radCluster" runat="server" CssClass="default" Text="Cluster Instance" GroupName="type" Enabled="false" /></td>
                </tr>
                <tr id="divCluster" runat="server" style="display:none">
                    <td></td>
                    <td style="border:solid 1px #EAEAEA">
                        <table width="100%" cellpadding="4" cellspacing="3" border="0">
                            <tr>
                                <td nowrap>Instance Name:<font class="required">&nbsp;*</font></td>
                                <td width="100%"><asp:TextBox ID="txtInstance" runat="server" CssClass="default" Width="300" MaxLength="50" /> .pncbank.com</td>
                            </tr>
                            <tr>
                                <td nowrap>Select VLAN:<font class="required">&nbsp;*</font></td>
                                <td width="100%"><asp:DropDownList ID="ddlVlan" runat="server" CssClass="default" /></td>
                            </tr>
                            <tr>
                                <td nowrap>Select Network:</td>
                                <td width="100%"><asp:DropDownList ID="ddlVlanNetwork" CssClass="default" runat="server" /></td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td width="100%"><asp:RadioButton ID="radILO" runat="server" CssClass="default" Text="ILO" GroupName="type" Enabled="false" /></td>
                </tr>
                <tr id="divILO" runat="server" style="display:none">
                    <td></td>
                    <td style="border:solid 1px #EAEAEA">
                        <table width="100%" cellpadding="4" cellspacing="3" border="0">
                            <tr>
                                <td nowrap>Serial Number:<font class="required">&nbsp;*</font></td>
                                <td width="100%"><asp:TextBox ID="txtSerial" runat="server" CssClass="default" Width="300" MaxLength="50" /> .pncbank.com</td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td width="100%"><asp:RadioButton ID="radServer" runat="server" CssClass="default" Text="Server" GroupName="type" Enabled="false" /></td>
                </tr>
                <tr id="divServer" runat="server" style="display:none">
                    <td></td>
                    <td style="border:solid 1px #EAEAEA">
                        <table width="100%" cellpadding="4" cellspacing="3" border="0">
                            <tr>
                                <td nowrap>Clustering Options:<font class="required">&nbsp;*</font></td>
                                <td width="100%">
                                    <asp:RadioButton ID="radSingleServer" runat="server" CssClass="default" Text="Single Server" GroupName="system" /> 
                                    <asp:RadioButton ID="radClusterWindows" runat="server" CssClass="default" Text="Windows Cluster System" GroupName="system" /> 
                                    <asp:RadioButton ID="radClusterUnixRAC" runat="server" CssClass="default" Text="UNIX RAC" GroupName="system" /> 
                                    <asp:RadioButton ID="radClusterUnixOther" runat="server" CssClass="default" Text="UNIX Cluster (Other)" GroupName="system" /> 
                                    <asp:RadioButton ID="radClusterVCS" runat="server" CssClass="default" Text="VCS Cluster" GroupName="system" /> 
                                </td>
                            </tr>
                        </table>
                        <div id="divSingleServer" runat="server" style="display:none">
                            <table cellpadding="4" cellspacing="3" border="0">
                                <tr>
                                    <td width="150">Server Name:<font class="required">&nbsp;*</font></td>
                                    <td><asp:TextBox ID="txtServer" runat="server" CssClass="default" Width="300" MaxLength="50" /> .pncbank.com</td>
                                </tr>
                                <tr>
                                    <td width="150">Load Balancing Method:<font class="required">&nbsp;*</font></td>
                                    <td>
                                        <asp:RadioButton ID="radCSM" runat="server" CssClass="default" Text="CSM" GroupName="csm" /> 
                                        <asp:RadioButton ID="radLTM" runat="server" CssClass="default" Text="LTM" GroupName="csm" /> 
                                        <asp:RadioButton ID="radNone" runat="server" CssClass="default" Text="None" GroupName="csm" /> 
                                    </td>
                                </tr>
                                <tr id="divCSM" runat="server" style="display:none">
                                    <td width="150">Select VLAN:<font class="required">&nbsp;*</font></td>
                                    <td><asp:DropDownList ID="ddlCSMVlan" runat="server" CssClass="default" /><img src="/images/spacer.gif" border="0" width="20" height="1" /><img src="/images/alert.gif" border="0" align="absmiddle" /> Call network engineering to obtain a VLAN assignment. Select the VLAN assigned to you.</td>
                                </tr>
                                <tr id="divCSM2" runat="server" style="display:none">
                                    <td nowrap>Select Network:</td>
                                    <td width="100%">
                                        <asp:DropDownList ID="ddlCSMVlanNetwork" CssClass="default" runat="server" Enabled="false" >
                                            <asp:ListItem Value="-- Please select a VLAN --" />
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr id="divLTM" runat="server" style="display:none">
                                    <td width="150">Select VLAN:<font class="required">&nbsp;*</font></td>
                                    <td><asp:DropDownList ID="ddlLTMVlan" runat="server" CssClass="default" /><img src="/images/spacer.gif" border="0" width="20" height="1" /><img src="/images/alert.gif" border="0" align="absmiddle" /> Call network engineering to obtain a VLAN assignment. Select the VLAN assigned to you.</td>
                                </tr>
                                <tr id="divLTM2" runat="server" style="display:none">
                                    <td nowrap>Select Network:</td>
                                    <td width="100%">
                                        <asp:DropDownList ID="ddlLTMVlanNetwork" CssClass="default" runat="server" Enabled="false" >
                                            <asp:ListItem Value="-- Please select a VLAN --" />
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                            <div id="divCSMNo" runat="server" style="display:none">
                            <table cellpadding="4" cellspacing="3" border="0">
                                <tr>
                                    <td width="150">Server Type:<font class="required">&nbsp;*</font></td>
                                    <td>
                                        <asp:DropDownList ID="ddlType" runat="server" CssClass="default" >
                                            <asp:ListItem Value="0" Text="-- SELECT --" />
                                            <asp:ListItem Value="1" Text="VMware Windows Guest" />
                                            <asp:ListItem Value="2" Text="VMware Linux Guest" />
                                            <asp:ListItem Value="3" Text="VMware Host" />
                                            <asp:ListItem Value="4" Text="MicroLPAR" />
                                            <asp:ListItem Value="5" Text="HP Blade" />
                                            <asp:ListItem Value="6" Text="Physical Windows" />
                                            <asp:ListItem Value="7" Text="Physical UNIX" />
                                            <asp:ListItem Value="8" Text="Physical Novell" />
                                            <asp:ListItem Value="9" Text="Virtual Workstation Host" />
                                            <asp:ListItem Value="10" Text="SUN Blade" />
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr id="divBladeHpVlan" runat="server" style="display:none">
                                    <td width="150">Select VLAN:<font class="required">&nbsp;*</font></td>
                                    <td><asp:DropDownList ID="ddlBladeHpVlan" runat="server" CssClass="default" /><img src="/images/spacer.gif" border="0" width="20" height="1" /><img src="/images/alert.gif" border="0" align="absmiddle" /> Select the VLAN of the enclosure.</td>
                                </tr>
                                <tr id="divBladeHpVlan2" runat="server" style="display:none">
                                    <td nowrap>Select Network:</td>
                                    <td width="100%">
                                        <asp:DropDownList ID="ddlBladeHpVlanNetwork" CssClass="default" runat="server" Enabled="false" >
                                            <asp:ListItem Value="-- Please select a VLAN --" />
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr id="divBladeSunVlan" runat="server" style="display:none">
                                    <td width="150">Select VLAN:<font class="required">&nbsp;*</font></td>
                                    <td><asp:DropDownList ID="ddlBladeSunVlan" runat="server" CssClass="default" /><img src="/images/spacer.gif" border="0" width="20" height="1" /><img src="/images/alert.gif" border="0" align="absmiddle" /> Select the VLAN of the enclosure.</td>
                                </tr>
                                <tr id="divBladeSunVlan2" runat="server" style="display:none">
                                    <td nowrap>Select Network:</td>
                                    <td width="100%">
                                        <asp:DropDownList ID="ddlBladeSunVlanNetwork" CssClass="default" runat="server" Enabled="false" >
                                            <asp:ListItem Value="-- Please select a VLAN --" />
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                            </div>
                        </div>
                        <div id="divClusterSystem" runat="server" style="display:none">
                            <table cellpadding="4" cellspacing="3" border="0">
                                <tr>
                                    <td nowrap>Asset Type:<font class="required">&nbsp;*</font></td>
                                    <td width="100%"><asp:DropDownList ID="ddlClusterType" runat="server" CssClass="default" /></td>
                                </tr>
                                <tr>
                                    <td nowrap>Asset Make:<font class="required">&nbsp;*</font></td>
                                    <td width="100%">
                                        <asp:DropDownList ID="ddlClusterModel" runat="server" CssClass="default" Enabled="false" >
                                            <asp:ListItem Value="0" Text="-- Please select a Type --" />
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td nowrap>Asset Model:<font class="required">&nbsp;*</font></td>
                                    <td width="100%">
                                        <asp:DropDownList ID="ddlClusterModelProperty" runat="server" CssClass="default" Enabled="false" >
                                            <asp:ListItem Value="0" Text="-- Please select a Make --" />
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2"><hr size="1" noshade /></td>
                                </tr>
                            </table>
                            <div id="divClusterHardware" runat="server" style="display:none">
                                <table cellpadding="4" cellspacing="3" border="0">
                                    <tr>
                                        <td nowrap>Hardware Type:<font class="required">&nbsp;*</font></td>
                                        <td width="100%">
                                            <asp:DropDownList ID="ddlHardware" runat="server" CssClass="default" >
                                                <asp:ListItem Value="0" Text="-- SELECT --" />
                                                <asp:ListItem Value="1" Text="Physical" />
                                                <asp:ListItem Value="2" Text="HP Blade" />
                                                <asp:ListItem Value="3" Text="SUN Blade" />
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="divClusterBladeHp" runat="server" style="display:none">
                                        <td nowrap>Select VLAN:<font class="required">&nbsp;*</font></td>
                                        <td width="100%"><asp:DropDownList ID="ddlClusterBladeHp" runat="server" CssClass="default" /><img src="/images/spacer.gif" border="0" width="20" height="1" /><img src="/images/alert.gif" border="0" align="absmiddle" /> Select the VLAN of the enclosure.</td>
                                    </tr>
                                    <tr id="divClusterBladeHp2" runat="server" style="display:none">
                                        <td nowrap>Select Network:</td>
                                        <td width="100%">
                                            <asp:DropDownList ID="ddlClusterBladeHpNetwork" CssClass="default" runat="server" Enabled="false" >
                                                <asp:ListItem Value="-- Please select a VLAN --" />
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="divClusterBladeSun" runat="server" style="display:none">
                                        <td nowrap>Select VLAN:<font class="required">&nbsp;*</font></td>
                                        <td width="100%"><asp:DropDownList ID="ddlClusterBladeSun" runat="server" CssClass="default" /><img src="/images/spacer.gif" border="0" width="20" height="1" /><img src="/images/alert.gif" border="0" align="absmiddle" /> Select the VLAN of the enclosure.</td>
                                    </tr>
                                    <tr id="divClusterBladeSun2" runat="server" style="display:none">
                                        <td nowrap>Select Network:</td>
                                        <td width="100%">
                                            <asp:DropDownList ID="ddlClusterBladeSunNetwork" CssClass="default" runat="server" Enabled="false" >
                                                <asp:ListItem Value="-- Please select a VLAN --" />
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <table cellpadding="4" cellspacing="3" border="0">
                                <tr>
                                    <td class="default">Physical Node Name:</td>
                                    <td>&nbsp;</td>
                                    <td class="default">Current Nodes:</td>
                                </tr>
                                <tr>
                                    <td valign="top">
                                        <table cellpadding="0" cellspacing="0" border="0">
                                            <tr>
                                                <td colspan="2"><asp:TextBox ID="txtNode" runat="server" Width="200" CssClass="default" /> .pncbank.com</td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" class="footer">(Maximum of 16 nodes per cluster)</td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td>Room:<font class="required">&nbsp;*</font> </td>
                                                <td><asp:TextBox ID="txtClusterRoom" runat="server" Width="50" CssClass="default" MaxLength="5" /> <span class="footer">Example: H</span></td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td>Rack:<font class="required">&nbsp;*</font> </td>
                                                <td><asp:TextBox ID="txtClusterRack" runat="server" Width="100" CssClass="default" MaxLength="10" /> <span class="footer">Example: HU09</span></td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td>Enclosure: </td>
                                                <td><asp:TextBox ID="txtClusterEnclosure" runat="server" Width="300" CssClass="default" MaxLength="50" /></td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td>Slot: </td>
                                                <td><asp:TextBox ID="txtClusterSlot" runat="server" Width="50" CssClass="default" MaxLength="5" /></td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td valign="top">
                                        <asp:Button ID="btnNodeAdd" runat="server" CssClass="default" Width="75" Text="Add  >>" ToolTip="Add Node" /><br /><br />
                                        <asp:Button ID="btnNodeRemove" runat="server" CssClass="default" Width="75" Text="Remove" ToolTip="Remove Node" />
                                    </td>
                                    <td valign="top"><asp:ListBox ID="lstNodes" runat="server" Width="200" CssClass="default" Rows="16" /></td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td width="100%"><asp:RadioButton ID="radAdditional" runat="server" CssClass="default" Text="Addtional Server IP" GroupName="type" /></td>
                </tr>
                <tr id="divAdditional" runat="server" style="display:none">
                    <td></td>
                    <td style="border:solid 1px #EAEAEA">
                        <table width="100%" cellpadding="4" cellspacing="3" border="0">
                            <tr>
                                <td nowrap>Server Name:<font class="required">&nbsp;*</font></td>
                                <td width="100%"><asp:TextBox ID="txtAdditional" runat="server" CssClass="default" Width="300" MaxLength="50" /> .pncbank.com</td>
                            </tr>
                            <tr>
                                <td nowrap>Select VLAN:<font class="required">&nbsp;*</font></td>
                                <td width="100%"><asp:DropDownList ID="ddlAdditionalVlan" runat="server" CssClass="default" /></td>
                            </tr>
                            <tr>
                                <td nowrap>Select Network:</td>
                                <td width="100%"><asp:DropDownList ID="ddlAdditionalVlanNetwork" CssClass="default" runat="server" /></td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td width="100%">&nbsp;</td>
                </tr>
                <tr>
                    <td></td>
                    <td width="100%">
                        <div id="divSending" runat="server" style="display:none">
                            <table width="100%" cellpadding="4" cellspacing="3" border="0" style="border:solid 1px #999999" bgcolor="#f9f9f9" >
                                <tr>
                                    <td class="bigred" colspan="2">Additonal Information Required</td>
                                </tr>
                                <tr>
                                    <td nowrap>Asset Type:<font class="required">&nbsp;*</font></td>
                                    <td width="100%"><asp:DropDownList ID="ddlSendingType" runat="server" CssClass="default" /></td>
                                </tr>
                                <tr>
                                    <td nowrap>Asset Make:<font class="required">&nbsp;*</font></td>
                                    <td width="100%">
                                        <asp:DropDownList ID="ddlSendingModel" runat="server" CssClass="default" Enabled="false" >
                                            <asp:ListItem Value="0" Text="-- Please select a Type --" />
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td nowrap>Asset Model:<font class="required">&nbsp;*</font></td>
                                    <td width="100%">
                                        <asp:DropDownList ID="ddlSendingModelProperty" runat="server" CssClass="default" Enabled="false" >
                                            <asp:ListItem Value="0" Text="-- Please select a Make --" />
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td nowrap>Room:<font class="required">&nbsp;*</font></td>
                                    <td width="100%"><asp:TextBox ID="txtSendingRoom" runat="server" CssClass="default" Width="50" MaxLength="5" /></td>
                                </tr>
                                <tr>
                                    <td nowrap></td>
                                    <td width="100%" class="footer"><b>NOTE:</b> Only enter the letter of the room (Example: for &quot;Room H&quot;...enter &quot;H&quot; - without the quotes)</td>
                                </tr>
                                <tr>
                                    <td nowrap>Rack:<font class="required">&nbsp;*</font></td>
                                    <td width="100%"><asp:TextBox ID="txtSendingRack" runat="server" CssClass="default" Width="100" MaxLength="10" /></td>
                                </tr>
                                <tr>
                                    <td nowrap></td>
                                    <td width="100%" class="footer"><b>NOTE:</b> Enter the full name of the rack (Example: &quot;HU09&quot; - without the quotes)</td>
                                </tr>
                                <tr>
                                    <td nowrap>Enclosure:</td>
                                    <td width="100%"><asp:TextBox ID="txtSendingEnclosure" runat="server" CssClass="default" Width="300" MaxLength="50" /></td>
                                </tr>
                                <tr>
                                    <td nowrap></td>
                                    <td width="100%" class="footer"><b>NOTE:</b> Enter the name of the enclosure (only required for blades)</td>
                                </tr>
                                <tr>
                                    <td nowrap>Slot:</td>
                                    <td width="100%"><asp:TextBox ID="txtSendingSlot" runat="server" CssClass="default" Width="50" MaxLength="5" /></td>
                                </tr>
                                <tr>
                                    <td nowrap></td>
                                    <td width="100%" class="footer"><b>NOTE:</b> Enter the slot number of the blade in the enclosure (only required for blades)</td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
            </table>
            <table width="100%" cellpadding="4" cellspacing="3" border="0">
                <tr>
                    <td colspan="2">&nbsp;</td>
                </tr>
                <tr>
                    <td colspan="2"><hr size="1" noshade /></td>
                </tr>
                <tr>
                    <td nowrap class="required">* = Required Field</td>
                    <td align="right"><asp:Button ID="btnSubmit" runat="server" CssClass="default" Width="100" Text="Generate" OnClick="btnSubmit_Click" /></td>
                </tr>
            </table>
            <p>&nbsp;</p>
        </td>
        <td background="/images/table_right.gif"><img src="/images/table_right.gif" width="5" height="10"></td>
    </tr>
    <tr>
        <td><img src="/images/table_bottomLeft.gif" border="0" width="5" height="9"></td>
        <td width="100%" background="/images/table_bottom.gif"></td>
        <td><img src="/images/table_bottomRight.gif" border="0" width="5" height="9"></td>
    </tr>
</table>
<input type="hidden" id="hdnParent" runat="server" />
<input type="hidden" id="hdnAdditionalEnvironment" runat="server" />
<input type="hidden" id="hdnServerEnvironment" runat="server" />
<asp:HiddenField ID="hdnProject" runat="server" />
<asp:HiddenField ID="hdnNodes" runat="server" />
<asp:HiddenField ID="hdnNetwork" runat="server" />
<asp:HiddenField ID="hdnError" runat="server" />
<asp:HiddenField ID="hdnModel" runat="server" />