<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wm_generic.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.wm_generic" %>

<script type="text/javascript">
    function ChangeFrame(oCell, oShow, oHide1, oHide2, oHide3, oHide4, oHidden, strHidden) {
        oShow = document.getElementById(oShow);
        oShow.style.display = "inline";
        oHide1 = document.getElementById(oHide1);
        oHide1.style.display = "none";
        oHide2 = document.getElementById(oHide2);
        oHide2.style.display = "none";
        oHide3 = document.getElementById(oHide3);
        oHide3.style.display = "none";
        oHide4 = document.getElementById(oHide4);
        oHide4.style.display = "none";
    	var oRow = oCell.parentElement;
	    for (var yy=0; yy<oRow.children.length; yy++) {
    		var oNot = oRow.getElementsByTagName("td").item(yy);
    		if (oNot.className == "cmbutton")
                oNot.style.border = "1px solid #94a6b5"
    	}
	    oCell.style.borderTop = "3px solid #6A8359"
        oCell.style.borderBottom = "1px solid #FFFFFF"
        oHidden = document.getElementById(oHidden);
        oHidden.value = strHidden;
    }
    function UpdateText(oText, oHidden) {
        oHidden = document.getElementById(oHidden);
        oHidden.value = oText.value;
    }
    function UpdateSelect(oSelect, oHidden, oDiv) {
        oHidden = document.getElementById(oHidden);
        oHidden.value = oSelect.options[oSelect.selectedIndex].value;
        if (oDiv != null) {
            oDiv = document.getElementById(oDiv);
            oDiv.style.display = "none";
            if (oHidden.value == "-999")
                oDiv.style.display = "inline";
        }
    }
    function EnsureManualStep1(chkYes, chkNo, chkVirtual, ddlClass, ddlEnvironment, hdnLocation, strServers) {
        chkVirtual = document.getElementById(chkVirtual);
        if (chkVirtual.checked == false)
        {
            chkYes = document.getElementById(chkYes);
            if (chkYes.checked == true)
            {
                // Do validation for YES
                if (ValidateDropDown(ddlClass,'Please select a class') == false)
                    return false;
                if (ValidateDropDown(ddlEnvironment,'Please select an environment') == false)
                    return false;
                if (ValidateDropDown("ddlCommon",'Please select a location') == false)
                    return false;
                if (ValidateHidden0(hdnLocation,'ddlState','Please select a location') == false)
                    return false;
            }
            else 
            {
                chkNo = document.getElementById(chkNo);
                if (chkNo.checked == true)
                {
                    // Do validation for NO
                    return EnsureTextbox();
                }
                else 
                {
                    alert('Please select whether or not you want ClearView to try to find an asset for you');
                    SetFocus(chkYes);
                    return false;
                }
            }
        }
        // SVE
	    var oSVE = document.getElementsByName("ddlSVE");
	    if (oSVE != null) {
	        for (var ii=0; ii<oSVE.length; ii++) {
	            if (oSVE[ii].selectedIndex == 0)
	            {
	                alert('Please select an SVE Cluster');
                    SetFocus(oSVE[ii]);
                    return false;
	            }
            }
        }
        return true;
    }
    function EnsureManualStep2(chkYes, chkNo, strServers) {
        chkYes = document.getElementById(chkYes);
        if (chkYes.checked == true)
        {
            // No validation for YES
        }
        else 
        {
            chkNo = document.getElementById(chkNo);
            if (chkNo.checked == true)
            {
                // Do validation for NO
                var intConfirm = 0;
                while (strServers != "" && intConfirm == 0)
                {
                    var strObject = strServers.substring(0, strServers.indexOf(";"));
                    strServers = strServers.substring(strServers.indexOf(";") + 1);
                    var oDDLName = document.getElementById('HDN_' + strObject + '_name');
                    var oDDLIP1 = document.getElementById('HDN_' + strObject + '_ip1');
                    var oDDLIP2 = document.getElementById('HDN_' + strObject + '_ip2');
                    var oDDLIP3 = document.getElementById('HDN_' + strObject + '_ip3');
                    /*
                    if (oDDLName.value == "0" && oDDLIP1.value == "0" && oDDLIP2.value == "-1" && oDDLIP3.value == "-1")
                        if (confirm('It does not appear you have selected any device names or IP addresses to be tied to the device(s).\n\nAre you sure you want to continue (with auto-generation)?') == false)
                            intConfirm = -1;
                        else
                            intConfirm = 1;
                    */
                }
                return (intConfirm >= 0);
            }
            else 
            {
                alert('Please select whether or not you want ClearView to try to generate the names and IP addresses for you');
                SetFocus(chkYes);
                return false;
            }
        }
        return true;
    }
    function EnsureTextbox() {
        var boolReturn = true;
        var oTexts = document.getElementsByName("ValidTextbox");
        for (var ii=0; ii<oTexts.length && boolReturn == true; ii++) {
            var boolHidden = false;
            try {
                oTexts[ii].focus();
            }
            catch (ex) {
                boolHidden = true;
            }
            if (oTexts[ii].disabled == false && boolHidden == false) {
                if (oTexts[ii].value == "") {
                    boolReturn = false;
                    alert('Please enter a valid value');
                    oTexts[ii].focus();
                }
            }
        }
        return boolReturn;
    }
    function FilterDropDown(oText, oDDL) {
        var oText = document.getElementById(oText);
        var oDDLs = document.getElementsByName(oDDL);
        for (var ii=0; ii<oDDLs.length; ii++) {
            oDDL = oDDLs[ii];
            var intStart = oDDL.selectedIndex;
            if (intStart > 0)
                intStart++;
            for (var jj=intStart; jj<=oDDL.length; jj++) {
                if (jj==oDDL.length) {
                    if (confirm('The text ' + oText.value.toUpperCase() + ' could not be found.\n\nDo you want to restart from the beginning of the list?') == true)
                        jj=0;
                }
                else if (oDDL.options[jj].text.toUpperCase().indexOf(oText.value.toUpperCase()) >= 0) {
                    oDDL.selectedIndex = jj;
                    UpdateSelect(oDDL, oDDL.title, oDDL.title.replace("HDN_", "DIV_"));
                    break;
                }
            }
        }
        oText.focus();
        return false;
    }
</script>
<asp:Panel ID="panWorkload" runat="server" Visible="false">
    <table width="100%" border="0" cellSpacing="0" cellPadding="0" class="default">
        <tr>
            <td>
                <table width="100%" border="0" cellSpacing="0" cellPadding="0">
                    <tr id="cntrlButtons">
                        <td><asp:ImageButton ID="btnSave" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/tool_save.gif" OnClick="btnSave_Click" /></td>
                        <td><asp:ImageButton ID="btnPrint" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/tool_print.gif" /></td>
                        <td><asp:ImageButton ID="btnComplete" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/tool_complete.gif" OnClick="btnComplete_Click" /></td>
                        <td width="100%">
                            <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                <tr>
                                    <td><img src="/images/tool_left.gif" border="0" width="6" height="26"></td>
                                    <td nowrap background="/images/tool_back.gif" width="100%"><img src="/images/tool_back.gif" border="0" width="2" height="26"></td>
                                    <td><img src="/images/tool_right.gif" border="0" width="6" height="26"></td>
                                </tr>
                            </table>
                        </td>
                        <td><asp:ImageButton ID="btnClose" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/tool_close.gif" /></td>
                    </tr>
                    <tr id="cntrlProcessing" style="display:none">
                        <td colspan="20">
                            <table width="100%" cellpadding="0" cellspacing="5" border="0">
                                <tr>
                                    <td rowspan="2"><img src="/images/saving.gif" border="0" align="absmiddle" /></td>
                                    <td class="header" width="100%" valign="bottom">Processing...</td>
                                </tr>
                                <tr>
                                    <td width="100%" valign="top">Please do not close this window while the page is processing.  Please be patient....</td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td class="default">
                <table width="100%" border="0" cellSpacing="2" cellPadding="2" class="default">
                    <tr>
                        <td valign="top">
                            <table border="0" cellpadding="3" cellspacing="2">
                                <tr>
                                    <td nowrap><b>Name:</b></td>
                                    <td><asp:Label ID="lblName" runat="server" CssClass="default" /></td>
                                </tr>
                                <tr>
                                    <td nowrap><b>Number:</b></td>
                                    <td><asp:Label ID="lblNumber" runat="server" CssClass="default" /></td>
                                </tr>
                                <tr>
                                    <td nowrap><b>Type:</b></td>
                                    <td><asp:Label ID="lblType" runat="server" CssClass="default" /></td>
                                </tr>
                                <tr>
                                    <td nowrap><b>Service:</b></td>
                                    <td><asp:Label ID="lblService" runat="server" CssClass="default" /></td>
                                </tr>
                                <tr>
                                    <td nowrap><b>Last Updated:</b></td>
                                    <td><asp:Label ID="lblUpdated" runat="server" CssClass="default" /></td>
                                </tr>
                                <tr>
                                    <td nowrap><b>Nickname:</b></td>
                                    <td><asp:TextBox ID="txtCustom" runat="server" CssClass="default" Width="300" MaxLength="100" /></td>
                                </tr>
                            </table>
                        </td>
                        <td valign="top" align="right">
                            <table border="0" cellpadding="3" cellspacing="2">
                                <tr>
                                    <td colspan="2"><iframe src="/frame/did_you_know.aspx" frameborder="0" scrolling="no" width="300" height="135"></iframe></td>
                                </tr>
                                <tr>
                                    <td nowrap><b>Overall Status:</b></td>
                                    <td><asp:Label ID="lblStatus" runat="server" CssClass="default" /></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr height="16">
            <td><img src="/images/spacer.gif" border="0" height="16" width="1" /></td>
        </tr>
        <tr>
            <td>
                    <table width="100%" height="100%" border="0" cellSpacing="0" cellPadding="0" class="default">
                        <tr>
                            <td class="cmbutton" style='<%=boolDetails == true ? "display:inline;border-bottom:1px solid #FFFFFF;border-top:3px solid orange;" : "" %>' onclick="ChangeFrame(this,'divDetails','divExecution','divStatusUpdates','divChange','divDocuments','<%=hdnTab.ClientID %>','D');">Request Details</td>
                            <td class="cmbuttonspace">&nbsp;</td>
                            <td class="cmbutton" style='<%=boolExecution == true ? "display:inline;border-bottom:1px solid #FFFFFF;border-top:3px solid orange;" : "" %>' onclick="ChangeFrame(this,'divExecution','divDetails','divStatusUpdates','divChange','divDocuments','<%=hdnTab.ClientID %>','E');">Execution</td>
                            <td class="cmbuttonspace">&nbsp;</td>
                            <td class="cmbutton" style='<%=boolStatusUpdates == true ? "display:inline;border-bottom:1px solid #FFFFFF;border-top:3px solid orange;" : "" %>' onclick="ChangeFrame(this,'divStatusUpdates','divDetails','divExecution','divChange','divDocuments','<%=hdnTab.ClientID %>','S');">Status Updates</td>
                            <td class="cmbuttonspace">&nbsp;</td>
                            <td class="cmbutton" style='<%=boolChange == true ? "display:inline;border-bottom:1px solid #FFFFFF;border-top:3px solid orange;" : "" %>' onclick="ChangeFrame(this,'divChange','divDetails','divExecution','divStatusUpdates','divDocuments','<%=hdnTab.ClientID %>','C');">Change Controls</td>
                            <td class="cmbuttonspace">&nbsp;</td>
                            <td class="cmbutton" style='<%=boolDocuments == true ? "display:inline;border-bottom:1px solid #FFFFFF;border-top:3px solid orange;" : "" %>' onclick="ChangeFrame(this,'divDocuments','divDetails','divExecution','divStatusUpdates','divChange','<%=hdnTab.ClientID %>','D');">Attached Files</td>
                        </tr>
                        <tr>
                            <td colspan="9" align="center" class="cmcontents">
                                <table width="100%" height="100%" border="0" cellSpacing="2" cellPadding="2" class="default">
                                    <tr> 
                                        <td valign="top">
		                                    <div id="divDetails" style='<%=boolDetails == true ? "display:inline" : "display:none" %>'>
		                                        <br />
                                                <table width="100%" cellpadding="4" cellspacing="3" border="0">
                                                    <tr>
                                                        <td colspan="2" class="greentableheader">Design Details</td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2"><img src="/images/bigInfo.gif" border="0" align="absmiddle" /> <asp:LinkButton ID="btnView" runat="server" Text="Click Here to View the Design Details" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2" class="greentableheader">Device Details</td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">Here are your servers.  As the execution steps are completed, the information will begin to appear below...</td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <table cellpadding="5" cellspacing="0" border="0" style="border:solid 1px #CCCCCC">
                                                                <tr bgcolor="#EEEEEE">
                                                                    <td>&nbsp;</td>
                                                                    <td>&nbsp;</td>
                                                                    <td nowrap valign="top"><b>Serial #</b></td>
                                                                    <td>&nbsp;</td>
                                                                    <td nowrap valign="top"><b>Serial # (DR)</b></td>
                                                                    <td>&nbsp;</td>
                                                                    <td nowrap valign="top"><b>Server Name</b></td>
                                                                    <td>&nbsp;</td>
                                                                    <td nowrap valign="top"><b>IP Address # 1</b></td>
                                                                    <td>&nbsp;</td>
                                                                    <td nowrap valign="top"><b>IP Address # 2</b></td>
                                                                    <td>&nbsp;</td>
                                                                    <td nowrap valign="top"><b>IP Address # 3</b></td>
                                                                    <td>&nbsp;</td>
                                                                    <td nowrap valign="top"><b>Operating System</b></td>
                                                                    <td>&nbsp;</td>
                                                                    <td nowrap valign="top" style='display:<%=boolSVE ? "inline" : "none" %>'><b>SVE Cluster</b></td>
                                                                    <td style='display:<%=boolSVE ? "inline" : "none" %>'>&nbsp;</td>
                                                                    <td nowrap valign="top"><b>SAN</b></td>
                                                                    <td>&nbsp;</td>
                                                                    <td nowrap valign="top"><b>SC</b></td>
                                                                    <td nowrap valign="top"><b>DNS</b></td>
                                                                </tr>
                                                                <asp:repeater ID="rptServers" runat="server">
                                                                    <ItemTemplate>
                                                                        <tr class="framegreen" style='display:<%# DataBinder.Eval(Container.DataItem, "serial").ToString() == "" ? "inline" : "none"%>'>
                                                                            <td colspan="6">Additional Information:</td>
                                                                            <td><%# DataBinder.Eval(Container.DataItem, "servername")%></td>
                                                                            <td></td>
                                                                            <td><%# DataBinder.Eval(Container.DataItem, "ipaddress1")%></td>
                                                                            <td colspan="50" align="right">[<a href="javascript:void(0);" onclick="alert('For SUN Virtual Environment Builds, this information represents the Virtual / Cluster Name and the Virtual IP Address (VIP).\n\nFor Clustered Builds, this information represents the cluster name and the Virtual IP Address (VIP).');" class="hold">Click here for more information</a>]</td>
                                                                        </tr>
                                                                        <tr class="default" style='display:<%# DataBinder.Eval(Container.DataItem, "serial").ToString() == "" ? "none" : "inline"%><%=intDeviceCount % 2 != 0 ? "" : "background-color:#F6F6F6"%>'>
                                                                            <td>Device&nbsp;#&nbsp;<%# DataBinder.Eval(Container.DataItem, "counter")%></td>
                                                                            <td>&nbsp;</td>
                                                                            <td nowrap><asp:Label ID="lblAsset" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "serial")%>' /></td>
                                                                            <td>&nbsp;</td>
                                                                            <td nowrap><asp:Label ID="lblAssetDR" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "serial_dr")%>' /></td>
                                                                            <td>&nbsp;</td>
                                                                            <td nowrap><asp:Label ID="lblName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "servername")%>' ToolTip='<%# DataBinder.Eval(Container.DataItem, "id")%>' /></td>
                                                                            <td>&nbsp;</td>
                                                                            <td nowrap><asp:Label ID="lblIP1" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ipaddress1").ToString() + (DataBinder.Eval(Container.DataItem, "vlan1").ToString() == "" ? "" : " (VLAN:" + DataBinder.Eval(Container.DataItem, "vlan1").ToString() + ")") %>' /></td></td>
                                                                            <td>&nbsp;</td>
                                                                            <td nowrap><asp:Label ID="lblIP2" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ipaddress2").ToString() + (DataBinder.Eval(Container.DataItem, "vlan2").ToString() == "" ? "" : " (VLAN:" + DataBinder.Eval(Container.DataItem, "vlan2").ToString() + ")") %>' /></td></td>
                                                                            <td>&nbsp;</td>
                                                                            <td nowrap><asp:Label ID="lblIP3" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ipaddress3").ToString() + (DataBinder.Eval(Container.DataItem, "vlan3").ToString() == "" ? "" : " (VLAN:" + DataBinder.Eval(Container.DataItem, "vlan3").ToString() + ")") %>' /></td></td>
                                                                            <td>&nbsp;</td>
                                                                            <td nowrap><%# DataBinder.Eval(Container.DataItem, "os")%> (<%# DataBinder.Eval(Container.DataItem, "sp")%>)</td>
                                                                            <td>&nbsp;</td>
                                                                            <td nowrap style='display:<%=boolSVE ? "inline" : "none" %>'><asp:Label ID="lblSVECluster" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "sve_cluster")%>' ToolTip='<%# DataBinder.Eval(Container.DataItem, "sve_clusterid")%>' /></td>
                                                                            <td style='display:<%=boolSVE ? "inline" : "none" %>'>&nbsp;</td>
                                                                            <td nowrap><asp:Label ID="lblStorage" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "clusterid")%>' ToolTip='<%# DataBinder.Eval(Container.DataItem, "number")%>' /></td></td>
                                                                            <td>&nbsp;</td>
                                                                            <td nowrap><asp:Label ID="lblServiceCenter" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "id")%>' /></td></td>
                                                                            <td nowrap><asp:Label ID="lblDNS" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "id")%>' /></td></td>
                                                                        </tr>
                                                                        <tr id='trStorage<%# DataBinder.Eval(Container.DataItem, "clusterid")%>_<%# DataBinder.Eval(Container.DataItem, "number")%>' class="default" style='display:none;<%=intDeviceCount % 2 != 0 ? "" : "background-color:#F6F6F6"%>'>
                                                                            <td colspan="50"><asp:Literal ID="litStorageConfig" runat="server" /></td>
                                                                        </tr>
                                                                    </ItemTemplate>
                                                                </asp:repeater>
                                                            </table>
                                                            <br />
                                                            <asp:Literal ID="litAdditional" runat="server" />
                                                            <br />
                                                            <asp:Literal ID="litStorage" runat="server" />
                                                        </td>
                                                    </tr>
                                                    <asp:Panel ID="panBackup" runat="server" Visible="false">
                                                    <tr>
                                                        <td colspan="2" class="greentableheader">Backup Information</td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2"><asp:Label ID="lblBackup" runat="server" /></td>
                                                    </tr>
                                                    </asp:Panel>
                                                    <tr>
                                                        <td colspan="2" class="greentableheader">Request Details</td>
                                                    </tr>
                                                    <tr>
                                                        <td nowrap>Design ID:</td>
                                                        <td width="100%"><asp:Label ID="lblAnswer" runat="server" CssClass="default" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td nowrap>Executed On:</td>
                                                        <td width="100%"><asp:Label ID="lblExecutedOn" runat="server" CssClass="default" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td nowrap>Executed By:</td>
                                                        <td width="100%"><asp:Label ID="lblExecutedBy" runat="server" CssClass="default" /></td>
                                                    </tr>
                                                </table>
                                                <p>&nbsp;</p>
		                                    </div>
		                                    <div id="divExecution" style='<%=boolExecution == true ? "display:inline" : "display:none" %>'>
		                                        <br />
                                                <table width="100%" cellpadding="4" cellspacing="3" border="0">
                                                    <asp:Panel ID="panError" runat="server" Visible="false">
                                                    <tr>
                                                        <td></td>
                                                        <td></td>
                                                        <td align="center">
                                                            <table cellpadding="5" cellspacing="5" border="0" style="border:solid 1px #999999" bgcolor="#f9f9f9">
                                                                <tr>
                                                                    <td rowspan="3" valign="top"><img src="/images/bigError.gif" border="0" align="absmiddle" /></td>
                                                                    <td class="header" valign="bottom">Please Read the Following Error:</td>
                                                                </tr>
                                                                <tr>
                                                                    <td valign="top"><asp:Label ID="lblError" runat="server" CssClass="default" /></td>
                                                                </tr>
                                                                <tr>
                                                                    <td valign="top">If possible, please try a different configuration or contact your ClearView administrator to proceed.</td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    </asp:Panel>
                                                    <asp:Panel ID="panWarning" runat="server" Visible="false">
                                                    <tr>
                                                        <td></td>
                                                        <td></td>
                                                        <td align="center">
                                                            <table cellpadding="5" cellspacing="5" border="0" style="border:solid 1px #999999" bgcolor="#f9f9f9">
                                                                <tr>
                                                                    <td rowspan="4" valign="top"><img src="/images/bigAlert.gif" border="0" align="absmiddle" /></td>
                                                                    <td class="header" valign="bottom">Please Read the Following Warning:</td>
                                                                </tr>
                                                                <tr>
                                                                    <td valign="top"><asp:Label ID="lblWarning" runat="server" CssClass="default" /></td>
                                                                </tr>
                                                                <tr>
                                                                    <td valign="top">If you are sure you want to continue, please click the &quot;Disregard Warning&quot; button below.</td>
                                                                </tr>
                                                                <tr>
                                                                    <td valign="top"><asp:Button ID="btnWarning" runat="server" CssClass="default" Text="Disregard Warning" Width="150" OnClick="btnWarning_Click" /></td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    </asp:Panel>
                                                    <asp:Panel ID="panAdditional" runat="server" Visible="false">
                                                    <tr>
                                                        <td nowrap></td>
                                                        <td nowrap><img src="/images/alert.gif" border="0" align="absmiddle" /></td>
                                                        <td width="100%">
                                                            <table cellpadding="2" cellspacing="2" border="0">
                                                                <tr>
                                                                    <td valign="top"><b>Additional Name(s):</b></td>
                                                                    <td valign="top"><asp:Label ID="lblAdditional" runat="server" CssClass="default" /></td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    </asp:Panel>
                                                    <asp:Panel ID="panHA" runat="server" Visible="false">
                                                    <tr>
                                                        <td nowrap></td>
                                                        <td nowrap><img src="/images/alert.gif" border="0" align="absmiddle" /></td>
                                                        <td width="100%">
                                                            <table cellpadding="2" cellspacing="2" border="0">
                                                                <tr>
                                                                    <td valign="top"><b>High Availability:</b></td>
                                                                    <td valign="top"><asp:Label ID="lblHA" runat="server" CssClass="default" /></td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    </asp:Panel>
                                                    <asp:Panel ID="panAdmin" runat="server" Visible="false">
                                                    <tr>
                                                        <td nowrap></td>
                                                        <td nowrap></td>
                                                        <td width="100%" align="center"><asp:CheckBox ID="chkAdmin" runat="server" CssClass="header" Text="Skip the rest of this request" /></td>
                                                    </tr>
                                                    </asp:Panel>
                                                    <tr>
                                                        <td nowrap rowspan="2" valign="top"><asp:CheckBox ID="chk1" runat="server" CssClass="default" /></td>
                                                        <td nowrap rowspan="2" valign="top"><asp:Image ID="img1" runat="server" ImageAlign="absMiddle" ImageUrl="/images/spacer.gif" /></td>
                                                        <td class="biggerbold">Step # 1 : Reserve Asset(s)</td>
                                                    </tr>
                                                    <tr id="tr1" runat="server" visible="false">
                                                        <td valign="top" width="100%">
                                                            <table cellpadding="3" cellpadding="3" border="0">
                                                                <tr class="highlightrow">
                                                                    <td class="bold">Model:</td>
                                                                    <td><asp:Label ID="lblModel" runat="server" CssClass="default" /></td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="2">Do you want to ClearView to try to find an asset for you?</td>
                                                                </tr>
                                                                <tr>
                                                                    <td></td>
                                                                    <td><asp:RadioButton ID="radAutoYes" runat="server" CssClass="default" Text="Yes - Please locate an asset based on Model, Class, Environment and Location" GroupName="AutoAssign" /></td>
                                                                </tr>
                                                                <tr>
                                                                    <td></td>
                                                                    <td><asp:RadioButton ID="radAutoNo" runat="server" CssClass="default" Text="No - I would like to manually enter the serial number(s)" GroupName="AutoAssign" /></td>
                                                                </tr>
                                                                <tr>
                                                                    <td></td>
                                                                    <td><asp:RadioButton ID="radAutoVirtual" runat="server" CssClass="default" Text="Virtual ONLY - Please generate the asset(s) for me" GroupName="AutoAssign" /></td>
                                                                </tr>
                                                                <tr id="trAutoYes" runat="server" style="display:none">
                                                                    <td></td>
                                                                    <td>
                                                                        <table cellpadding="3" cellspacing="0" border="0">
                                                                            <tr>
                                                                                <td colspan="2">Please select the class / environment / location of the asset...</td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>Class:</td>
                                                                                <td><asp:DropDownList ID="ddlClass" runat="server" CssClass="default" Width="200" /></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>Environment:</td>
                                                                                <td>
                                                                                    <asp:DropDownList ID="ddlEnvironment" CssClass="default" runat="server" Width="200" Enabled="false" >
                                                                                        <asp:ListItem Value="-- Please select a Class --" />
                                                                                    </asp:DropDownList>
                                                                                    <input type="hidden" id="hdnEnvironment" runat="server" />
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>Location:</td>
                                                                                <td>
                                                                                    <%=strLocation %>
                                                                                    <input type="hidden" id="hdnLocation" runat="server" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                                <tr id="trAutoNo" runat="server" style="display:none">
                                                                    <td></td>
                                                                    <td>
                                                                        <table cellpadding="3" cellspacing="0" border="0" style="border:solid 1px #CCCCCC">
                                                                            <tr bgcolor="#EEEEEE">
                                                                                <td nowrap></td>
                                                                                <td nowrap><b>Serial #</b></td>
                                                                                <td nowrap><b>Serial # (DR)</b></td>
                                                                            </tr>
                                                                            <asp:repeater ID="rptAssets" runat="server">
                                                                                <ItemTemplate>
                                                                                    <tr class="default">
                                                                                        <td nowrap><%# (Int32.Parse(DataBinder.Eval(Container.DataItem, "clusterid").ToString()) > 0 ? "Node # " : "Device # ") + intAssetCount++%></td>
                                                                                        <td nowrap width="225">
                                                                                            <input name="ValidTextbox" type="text" class="default" style="width:200px" onblur="UpdateText(this,'HDN_<%# DataBinder.Eval(Container.DataItem, "id")%>_serial');" value='<%# DataBinder.Eval(Container.DataItem, "serial").ToString() == "--- Pending ---" ? "" : DataBinder.Eval(Container.DataItem, "serial")%>' />
                                                                                            <input type="hidden" id='HDN_<%# DataBinder.Eval(Container.DataItem, "id")%>_serial' name='HDN_<%# DataBinder.Eval(Container.DataItem, "id")%>_serial' value='<%# DataBinder.Eval(Container.DataItem, "serial")%>' />
                                                                                        </td>
                                                                                        <td nowrap width="225" style='display:<%# (DataBinder.Eval(Container.DataItem, "serial_dr").ToString() == "--- None ---" ? "none" : "inline")%>'>
                                                                                            <input name="ValidTextbox" type="text" class="default" style="width:200px" onblur="UpdateText(this,'HDN_<%# DataBinder.Eval(Container.DataItem, "id")%>_serial_dr');" value='<%# DataBinder.Eval(Container.DataItem, "serial_dr").ToString() == "--- Pending ---" ? "" : DataBinder.Eval(Container.DataItem, "serial_dr")%>' />
                                                                                            <input type="hidden" id='HDN_<%# DataBinder.Eval(Container.DataItem, "id")%>_serial_dr' name='HDN_<%# DataBinder.Eval(Container.DataItem, "id")%>_serial_dr' value='<%# DataBinder.Eval(Container.DataItem, "serial_dr")%>' />
                                                                                        </td>
                                                                                        <td nowrap width="225" style='display:<%# (DataBinder.Eval(Container.DataItem, "serial_dr").ToString() == "--- None ---" ? "inline" : "none")%>'>
                                                                                            <i>Not Required</i>
                                                                                        </td>
                                                                                    </tr>
                                                                                </ItemTemplate>
                                                                            </asp:repeater>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                                <tr id="trSVE" runat="server" visible="false">
                                                                    <td></td>
                                                                    <td>
                                                                        <b>SVE Type:</b> <asp:Label ID="lblSVEName" runat="Server" /><br /><br />
                                                                        <table cellpadding="3" cellspacing="0" border="0" style="border:solid 1px #CCCCCC">
                                                                            <tr bgcolor="#EEEEEE">
                                                                                <td nowrap></td>
                                                                                <td nowrap><b>SVE Cluster</b></td>
                                                                            </tr>
                                                                            <asp:repeater ID="rptSVE" runat="server">
                                                                                <ItemTemplate>
                                                                                    <tr class="default">
                                                                                        <td nowrap><%# "Container # " + intContainerCount++%></td>
                                                                                        <td nowrap style='display:<%# (DataBinder.Eval(Container.DataItem, "sve_clusterid").ToString() == "" ? "none" : "inline")%>'><%# DataBinder.Eval(Container.DataItem, "sve_cluster")%></td>
                                                                                        <td nowrap style='display:<%# (DataBinder.Eval(Container.DataItem, "sve_clusterid").ToString() == "" ? "inline" : "none")%>'>
                                                                                            <select name="ddlSVE" onchange="UpdateSelect(this,'HDN_<%# DataBinder.Eval(Container.DataItem, "id")%>_sve_clusterid');" name="SVE_CLUSTER" class="default" title='HDN_<%# DataBinder.Eval(Container.DataItem, "id")%>_sve_clusterid'>
                                                                                                <option value="0">-- SELECT ---</option>
                                                                                                <%=ddlSVEs %>
                                                                                            </select>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <input type="hidden" id='HDN_<%# DataBinder.Eval(Container.DataItem, "id")%>_sve_clusterid' name='HDN_<%# DataBinder.Eval(Container.DataItem, "id")%>_sve_clusterid' value='<%# DataBinder.Eval(Container.DataItem, "sve_clusterid")%>' />
                                                                                </ItemTemplate>
                                                                            </asp:repeater>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr id="tr1Done" runat="server" visible="false">
                                                        <td valign="top" width="100%"><asp:Label ID="lbl1" runat="server" CssClass="approved" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td nowrap rowspan="2" valign="top"><asp:CheckBox ID="chk3" runat="server" CssClass="default" Enabled="false" /></td>
                                                        <td nowrap rowspan="2" valign="top"><asp:Image ID="img3" runat="server" ImageAlign="absMiddle" ImageUrl="/images/spacer.gif" /></td>
                                                        <td class="biggerbold">Step # 2 : Assign Name(s) / IP Address(es)</td>
                                                    </tr>
                                                    <tr id="tr3" runat="server" visible="false">
                                                        <td valign="top" width="100%">
                                                            <table cellpadding="3" cellpadding="3" border="0">
                                                                <tr>
                                                                    <td colspan="2">Do you want to ClearView to try to generate the names and IP addresses for you?</td>
                                                                </tr>
                                                                <tr>
                                                                    <td></td>
                                                                    <td><asp:RadioButton ID="radNamesYes" runat="server" CssClass="default" Text="Yes - Please generate the names and IP addresses an asset based on the design requirements" GroupName="NameAssign" /></td>
                                                                </tr>
                                                                <tr>
                                                                    <td></td>
                                                                    <td><asp:RadioButton ID="radNamesNo" runat="server" CssClass="default" Text="No - Let me select the names and IP addresses from my lists" GroupName="NameAssign" /></td>
                                                                </tr>
                                                                <tr id="trNamesNo" runat="server" style="display:none">
                                                                    <td></td>
                                                                    <td>
                                                                        <table cellpadding="5" cellspacing="0" border="0">
                                                                            <tr>
                                                                                <td><b>NOTE:</b> If you do not see a name or IP address, it is most likely already in use.</td>
                                                                            </tr>
                                                                        </table>
                                                                        <br />
                                                                        <table cellpadding="3" cellspacing="0" border="0" style="border:solid 1px #CCCCCC">
                                                                            <tr class="framegreen">
                                                                                <td>FILTER&nbsp;/&nbsp;SEARCH:</td>
                                                                                <td><asp:TextBox ID="txtSearchName" runat="server" CssClass="default" Width="100" />&nbsp;<asp:ImageButton ID="imgSearchName" runat="server" ImageUrl="/images/search.gif" ImageAlign="AbsMiddle" /></td>
                                                                                <td><asp:TextBox ID="txtSearchIP1" runat="server" CssClass="default" Width="100" />&nbsp;<asp:ImageButton ID="imgSearchIP1" runat="server" ImageUrl="/images/search.gif" ImageAlign="AbsMiddle" /></td>
                                                                                <td><asp:TextBox ID="txtSearchIP2" runat="server" CssClass="default" Width="100" />&nbsp;<asp:ImageButton ID="imgSearchIP2" runat="server" ImageUrl="/images/search.gif" ImageAlign="AbsMiddle" /></td>
                                                                                <td><asp:TextBox ID="txtSearchIP3" runat="server" CssClass="default" Width="100" />&nbsp;<asp:ImageButton ID="imgSearchIP3" runat="server" ImageUrl="/images/search.gif" ImageAlign="AbsMiddle" /></td>
                                                                            </tr>
                                                                            <tr bgcolor="#EEEEEE">
                                                                                <td nowrap valign="top"><b>Serial #</b></td>
                                                                                <td nowrap valign="top"><b>Server Name</b></td>
                                                                                <td nowrap valign="top"><b>IP Address # 1</b></td>
                                                                                <td nowrap valign="top"><b>IP Address # 2<br />(Optional)</b></td>
                                                                                <td nowrap valign="top"><b>IP Address # 3<br />(Optional)</b></td>
                                                                            </tr>
                                                                            <asp:repeater ID="rptNames" runat="server">
                                                                                <ItemTemplate>
                                                                                    <tr class="default">
                                                                                        <td nowrap valign="top"><%# DataBinder.Eval(Container.DataItem, "serial").ToString()%></td>
                                                                                        <td nowrap valign="top" style='display:<%# (DataBinder.Eval(Container.DataItem, "nameid").ToString() == "0" ? "inline" : "none")%>'>
                                                                                            <select onchange="UpdateSelect(this,'HDN_<%# DataBinder.Eval(Container.DataItem, "id")%>_name','DIV_<%# DataBinder.Eval(Container.DataItem, "id")%>_name');" name="SEARCH_NAME" class="default" title='HDN_<%# DataBinder.Eval(Container.DataItem, "id")%>_name'>
                                                                                                <option value="0">-- AUTO-GENERATE ---</option>
                                                                                                <%# (boolManualName == true ? "<option value='-999'>--- ENTER MANUALLY ---</option>" : "")%>
                                                                                                <%=ddlNames %>
                                                                                            </select>
                                                                                            <div id='DIV_<%# DataBinder.Eval(Container.DataItem, "id")%>_name' style="display:none">
                                                                                                <br />
                                                                                                <input name="ValidTextbox" type="text" class="default" style="width:150px;" maxlength="50" onblur="UpdateText(this,'HDN_<%# DataBinder.Eval(Container.DataItem, "id")%>_manual_name');" value='<%# DataBinder.Eval(Container.DataItem, "nameid").ToString() == "0" ? "" : DataBinder.Eval(Container.DataItem, "servername")%>' />
                                                                                            </div>
                                                                                        </td>
                                                                                        <td nowrap valign="top" style='display:<%# (DataBinder.Eval(Container.DataItem, "nameid").ToString() == "0" ? "none" : "inline")%>'><%# DataBinder.Eval(Container.DataItem, "servername")%></td>
                                                                                        <td nowrap valign="top" style='display:<%# (DataBinder.Eval(Container.DataItem, "ipaddressid1").ToString() == "0" ? "inline" : "none")%>'>
                                                                                            <select onchange="UpdateSelect(this,'HDN_<%# DataBinder.Eval(Container.DataItem, "id")%>_ip1','DIV_<%# DataBinder.Eval(Container.DataItem, "id")%>_ip1');" name="SEARCH_IP1" class="default" title='HDN_<%# DataBinder.Eval(Container.DataItem, "id")%>_ip1'>
                                                                                                <%#(oOperatingSystem.IsSolaris(Int32.Parse(DataBinder.Eval(Container.DataItem, "osid").ToString())) == false || oModelsProperties.IsSUNVirtual(Int32.Parse(DataBinder.Eval(Container.DataItem, "modelid").ToString())) == true ? "<option value='-1'>NONE / SKIP</option>" : "")%>
                                                                                                <option value="0">-- AUTO-GENERATE ---</option>
                                                                                                <%# (boolManualIP == true ? "<option value='-999'>--- ENTER MANUALLY ---</option>" : "")%>
                                                                                                <%=ddlIPs %>
                                                                                            </select>
                                                                                            <div id='DIV_<%# DataBinder.Eval(Container.DataItem, "id")%>_ip1' style="display:none">
                                                                                                <br />
                                                                                                <input name="ValidTextbox" type="text" class="default" style="width:30px;" maxlength="3" onblur="UpdateText(this,'HDN_<%# DataBinder.Eval(Container.DataItem, "id")%>_manual_ip1_1');" value='<%# DataBinder.Eval(Container.DataItem, "ipaddress1_1")%>' />
                                                                                                .
                                                                                                <input name="ValidTextbox" type="text" class="default" style="width:30px;" maxlength="3" onblur="UpdateText(this,'HDN_<%# DataBinder.Eval(Container.DataItem, "id")%>_manual_ip1_2');" value='<%# DataBinder.Eval(Container.DataItem, "ipaddress1_2")%>' />
                                                                                                .
                                                                                                <input name="ValidTextbox" type="text" class="default" style="width:30px;" maxlength="3" onblur="UpdateText(this,'HDN_<%# DataBinder.Eval(Container.DataItem, "id")%>_manual_ip1_3');" value='<%# DataBinder.Eval(Container.DataItem, "ipaddress1_3")%>' />
                                                                                                .
                                                                                                <input name="ValidTextbox" type="text" class="default" style="width:30px;" maxlength="3" onblur="UpdateText(this,'HDN_<%# DataBinder.Eval(Container.DataItem, "id")%>_manual_ip1_4');" value='<%# DataBinder.Eval(Container.DataItem, "ipaddress1_4")%>' />
                                                                                            </div>
                                                                                        </td>
                                                                                        <td nowrap valign="top" style='display:<%# (DataBinder.Eval(Container.DataItem, "ipaddressid1").ToString() == "0" ? "none" : "inline")%>'><%# DataBinder.Eval(Container.DataItem, "ipaddress1")%></td>
                                                                                        <td nowrap valign="top" style='display:<%# (DataBinder.Eval(Container.DataItem, "ipaddressid2").ToString() == "0" || DataBinder.Eval(Container.DataItem, "ipaddressid2").ToString() == "-1" ? "inline" : "none")%>'>
                                                                                            <select onchange="UpdateSelect(this,'HDN_<%# DataBinder.Eval(Container.DataItem, "id")%>_ip2','DIV_<%# DataBinder.Eval(Container.DataItem, "id")%>_ip2');" name="SEARCH_IP2" class="default" title='HDN_<%# DataBinder.Eval(Container.DataItem, "id")%>_ip2'>
                                                                                                <%#(oOperatingSystem.IsSolaris(Int32.Parse(DataBinder.Eval(Container.DataItem, "osid").ToString())) == false || oModelsProperties.IsSUNVirtual(Int32.Parse(DataBinder.Eval(Container.DataItem, "modelid").ToString())) == true ? "<option value='-1'>NONE / SKIP</option>" : "")%>
                                                                                                <option value="0">-- AUTO-GENERATE ---</option>
                                                                                                <%# (boolManualIP == true ? "<option value='-999'>--- ENTER MANUALLY ---</option>" : "")%>
                                                                                                <%=ddlIPs %>
                                                                                            </select>
                                                                                            <div id='DIV_<%# DataBinder.Eval(Container.DataItem, "id")%>_ip2' style="display:none">
                                                                                                <br />
                                                                                                <input name="ValidTextbox" type="text" class="default" style="width:30px;" maxlength="3" onblur="UpdateText(this,'HDN_<%# DataBinder.Eval(Container.DataItem, "id")%>_manual_ip2_1');" value='<%# DataBinder.Eval(Container.DataItem, "ipaddress2_1")%>' />
                                                                                                .
                                                                                                <input name="ValidTextbox" type="text" class="default" style="width:30px;" maxlength="3" onblur="UpdateText(this,'HDN_<%# DataBinder.Eval(Container.DataItem, "id")%>_manual_ip2_2');" value='<%# DataBinder.Eval(Container.DataItem, "ipaddress2_2")%>' />
                                                                                                .
                                                                                                <input name="ValidTextbox" type="text" class="default" style="width:30px;" maxlength="3" onblur="UpdateText(this,'HDN_<%# DataBinder.Eval(Container.DataItem, "id")%>_manual_ip2_3');" value='<%# DataBinder.Eval(Container.DataItem, "ipaddress2_3")%>' />
                                                                                                .
                                                                                                <input name="ValidTextbox" type="text" class="default" style="width:30px;" maxlength="3" onblur="UpdateText(this,'HDN_<%# DataBinder.Eval(Container.DataItem, "id")%>_manual_ip2_4');" value='<%# DataBinder.Eval(Container.DataItem, "ipaddress2_4")%>' />
                                                                                            </div>
                                                                                        </td>
                                                                                        <td nowrap valign="top" style='display:<%# (DataBinder.Eval(Container.DataItem, "ipaddressid2").ToString() == "0" || DataBinder.Eval(Container.DataItem, "ipaddressid2").ToString() == "-1" ? "none" : "inline")%>'><%# DataBinder.Eval(Container.DataItem, "ipaddress2")%></td>
                                                                                        <td nowrap valign="top" style='display:<%# (DataBinder.Eval(Container.DataItem, "ipaddressid3").ToString() == "0" || DataBinder.Eval(Container.DataItem, "ipaddressid3").ToString() == "-1" ? "inline" : "none")%>'>
                                                                                            <select onchange="UpdateSelect(this,'HDN_<%# DataBinder.Eval(Container.DataItem, "id")%>_ip3','DIV_<%# DataBinder.Eval(Container.DataItem, "id")%>_ip3');" name="SEARCH_IP3" class="default" title='HDN_<%# DataBinder.Eval(Container.DataItem, "id")%>_ip3'>
                                                                                                <%#(oOperatingSystem.IsSolaris(Int32.Parse(DataBinder.Eval(Container.DataItem, "osid").ToString())) == false || oModelsProperties.IsSUNVirtual(Int32.Parse(DataBinder.Eval(Container.DataItem, "modelid").ToString())) == true ? "<option value='-1'>NONE / SKIP</option>" : "")%>
                                                                                                <option value="0">-- AUTO-GENERATE ---</option>
                                                                                                <%# (boolManualIP == true ? "<option value='-999'>--- ENTER MANUALLY ---</option>" : "")%>
                                                                                                <%=ddlIPs %>
                                                                                            </select>
                                                                                            <div id='DIV_<%# DataBinder.Eval(Container.DataItem, "id")%>_ip3' style="display:none">
                                                                                                <br />
                                                                                                <input name="ValidTextbox" type="text" class="default" style="width:30px;" maxlength="3" onblur="UpdateText(this,'HDN_<%# DataBinder.Eval(Container.DataItem, "id")%>_manual_ip3_1');" value='<%# DataBinder.Eval(Container.DataItem, "ipaddress3_1")%>' />
                                                                                                .
                                                                                                <input name="ValidTextbox" type="text" class="default" style="width:30px;" maxlength="3" onblur="UpdateText(this,'HDN_<%# DataBinder.Eval(Container.DataItem, "id")%>_manual_ip3_2');" value='<%# DataBinder.Eval(Container.DataItem, "ipaddress3_2")%>' />
                                                                                                .
                                                                                                <input name="ValidTextbox" type="text" class="default" style="width:30px;" maxlength="3" onblur="UpdateText(this,'HDN_<%# DataBinder.Eval(Container.DataItem, "id")%>_manual_ip3_3');" value='<%# DataBinder.Eval(Container.DataItem, "ipaddress3_3")%>' />
                                                                                                .
                                                                                                <input name="ValidTextbox" type="text" class="default" style="width:30px;" maxlength="3" onblur="UpdateText(this,'HDN_<%# DataBinder.Eval(Container.DataItem, "id")%>_manual_ip3_4');" value='<%# DataBinder.Eval(Container.DataItem, "ipaddress3_4")%>' />
                                                                                            </div>
                                                                                        </td>
                                                                                        <td nowrap valign="top" style='display:<%# (DataBinder.Eval(Container.DataItem, "ipaddressid3").ToString() == "0" || DataBinder.Eval(Container.DataItem, "ipaddressid3").ToString() == "-1" ? "none" : "inline")%>'><%# DataBinder.Eval(Container.DataItem, "ipaddress3")%></td>
                                                                                    </tr>

                                                                                    <input type="hidden" id='HDN_<%# DataBinder.Eval(Container.DataItem, "id")%>_name' name='HDN_<%# DataBinder.Eval(Container.DataItem, "id")%>_name' value='<%# DataBinder.Eval(Container.DataItem, "nameid")%>' />
                                                                                    <input type="hidden" id='HDN_<%# DataBinder.Eval(Container.DataItem, "id")%>_trunking' name='HDN_<%# DataBinder.Eval(Container.DataItem, "id")%>_trunking' value='<%# DataBinder.Eval(Container.DataItem, "trunking")%>' />
                                                                                    <input type="hidden" id='HDN_<%# DataBinder.Eval(Container.DataItem, "id")%>_ip1' name='HDN_<%# DataBinder.Eval(Container.DataItem, "id")%>_ip1' value='<%# DataBinder.Eval(Container.DataItem, "ipaddressid1")%>' />
                                                                                    <input type="hidden" id='HDN_<%# DataBinder.Eval(Container.DataItem, "id")%>_ip2' name='HDN_<%# DataBinder.Eval(Container.DataItem, "id")%>_ip2' value='<%# DataBinder.Eval(Container.DataItem, "ipaddressid2")%>' />
                                                                                    <input type="hidden" id='HDN_<%# DataBinder.Eval(Container.DataItem, "id")%>_ip3' name='HDN_<%# DataBinder.Eval(Container.DataItem, "id")%>_ip3' value='<%# DataBinder.Eval(Container.DataItem, "ipaddressid3")%>' />
                                                                                    <input type="hidden" id='HDN_<%# DataBinder.Eval(Container.DataItem, "id")%>_manual_name' name='HDN_<%# DataBinder.Eval(Container.DataItem, "id")%>_manual_name' value='<%# DataBinder.Eval(Container.DataItem, "servername")%>' />
                                                                                    <input type="hidden" id='HDN_<%# DataBinder.Eval(Container.DataItem, "id")%>_manual_ip1_1' name='HDN_<%# DataBinder.Eval(Container.DataItem, "id")%>_manual_ip1_1' value='<%# DataBinder.Eval(Container.DataItem, "ipaddress1_1")%>' />
                                                                                    <input type="hidden" id='HDN_<%# DataBinder.Eval(Container.DataItem, "id")%>_manual_ip1_2' name='HDN_<%# DataBinder.Eval(Container.DataItem, "id")%>_manual_ip1_2' value='<%# DataBinder.Eval(Container.DataItem, "ipaddress1_2")%>' />
                                                                                    <input type="hidden" id='HDN_<%# DataBinder.Eval(Container.DataItem, "id")%>_manual_ip1_3' name='HDN_<%# DataBinder.Eval(Container.DataItem, "id")%>_manual_ip1_3' value='<%# DataBinder.Eval(Container.DataItem, "ipaddress1_3")%>' />
                                                                                    <input type="hidden" id='HDN_<%# DataBinder.Eval(Container.DataItem, "id")%>_manual_ip1_4' name='HDN_<%# DataBinder.Eval(Container.DataItem, "id")%>_manual_ip1_4' value='<%# DataBinder.Eval(Container.DataItem, "ipaddress1_4")%>' />
                                                                                    <input type="hidden" id='HDN_<%# DataBinder.Eval(Container.DataItem, "id")%>_manual_ip2_1' name='HDN_<%# DataBinder.Eval(Container.DataItem, "id")%>_manual_ip2_1' value='<%# DataBinder.Eval(Container.DataItem, "ipaddress2_1")%>' />
                                                                                    <input type="hidden" id='HDN_<%# DataBinder.Eval(Container.DataItem, "id")%>_manual_ip2_2' name='HDN_<%# DataBinder.Eval(Container.DataItem, "id")%>_manual_ip2_2' value='<%# DataBinder.Eval(Container.DataItem, "ipaddress2_2")%>' />
                                                                                    <input type="hidden" id='HDN_<%# DataBinder.Eval(Container.DataItem, "id")%>_manual_ip2_3' name='HDN_<%# DataBinder.Eval(Container.DataItem, "id")%>_manual_ip2_3' value='<%# DataBinder.Eval(Container.DataItem, "ipaddress2_3")%>' />
                                                                                    <input type="hidden" id='HDN_<%# DataBinder.Eval(Container.DataItem, "id")%>_manual_ip2_4' name='HDN_<%# DataBinder.Eval(Container.DataItem, "id")%>_manual_ip2_4' value='<%# DataBinder.Eval(Container.DataItem, "ipaddress2_4")%>' />
                                                                                    <input type="hidden" id='HDN_<%# DataBinder.Eval(Container.DataItem, "id")%>_manual_ip3_1' name='HDN_<%# DataBinder.Eval(Container.DataItem, "id")%>_manual_ip3_1' value='<%# DataBinder.Eval(Container.DataItem, "ipaddress3_1")%>' />
                                                                                    <input type="hidden" id='HDN_<%# DataBinder.Eval(Container.DataItem, "id")%>_manual_ip3_2' name='HDN_<%# DataBinder.Eval(Container.DataItem, "id")%>_manual_ip3_2' value='<%# DataBinder.Eval(Container.DataItem, "ipaddress3_2")%>' />
                                                                                    <input type="hidden" id='HDN_<%# DataBinder.Eval(Container.DataItem, "id")%>_manual_ip3_3' name='HDN_<%# DataBinder.Eval(Container.DataItem, "id")%>_manual_ip3_3' value='<%# DataBinder.Eval(Container.DataItem, "ipaddress3_3")%>' />
                                                                                    <input type="hidden" id='HDN_<%# DataBinder.Eval(Container.DataItem, "id")%>_manual_ip3_4' name='HDN_<%# DataBinder.Eval(Container.DataItem, "id")%>_manual_ip3_4' value='<%# DataBinder.Eval(Container.DataItem, "ipaddress3_4")%>' />
                                                                                </ItemTemplate>
                                                                            </asp:repeater>
                                                                            <tr>
                                                                                <td colspan="5">&nbsp;</td>
                                                                            </tr>
                                                                            <tr class="framegreen">
                                                                                <td class="bold">ORDER BY:</td>
                                                                                <td>
                                                                                    <asp:DropDownList ID="ddlOrderName" runat="server" CssClass="default">
                                                                                        <asp:ListItem Value="modified DESC" Text="Date Requested" />
                                                                                        <asp:ListItem Value="servername" Text="Server Name" />
                                                                                        <asp:ListItem Value="name" Text="Nickname" />
                                                                                    </asp:DropDownList>
                                                                                </td>
                                                                                <td colspan="3">
                                                                                    <asp:DropDownList ID="ddlOrderIP" runat="server" CssClass="default">
                                                                                        <asp:ListItem Value="modified DESC" Text="Date Requested" />
                                                                                        <asp:ListItem Value="name" Text="IP Address" />
                                                                                        <asp:ListItem Value="custom" Text="Description" />
                                                                                    </asp:DropDownList>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td></td>
                                                                                <td colspan="3">Must click the &quot;Apply Order&quot; button ------></td>
                                                                                <td align="right"><asp:Button ID="btnOrder" runat="server" CssClass="default" Width="100" Text="Apply Order" OnClick="btnOrder_Click" /></td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td></td>
                                                                    <td>
                                                                        <div ID="panAdditionalName" runat="server" style="display:none">
                                                                        <table cellpadding="3" cellspacing="3" border="0">
                                                                            <tr>
                                                                                <td>Additional Name:</td>
                                                                                <td><asp:TextBox ID="txtAdditionalName" runat="server" MaxLength="50" Width="150" /> (Optional ... For example, the cluster name / virtual name / container name)</td>
                                                                            </tr>
                                                                        </table>
                                                                        </div>
                                                                        <div ID="panAdditionalIP" runat="server" style="display:none">
                                                                        <table cellpadding="3" cellspacing="3" border="0">
                                                                            <tr>
                                                                                <td>Additional IP:</td>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtAdditionalIP1" runat="server" MaxLength="3" Width="30" />
                                                                                    .
                                                                                    <asp:TextBox ID="txtAdditionalIP2" runat="server" MaxLength="3" Width="30" />
                                                                                    .
                                                                                    <asp:TextBox ID="txtAdditionalIP3" runat="server" MaxLength="3" Width="30" />
                                                                                    .
                                                                                    <asp:TextBox ID="txtAdditionalIP4" runat="server" MaxLength="3" Width="30" />
                                                                                     (Optional ... For example, the Virtual IP Address / VIP)
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr id="tr3Wait" runat="server" visible="false">
                                                        <td valign="top" width="100%"><i>Waiting for Previous Step to Complete</i></td>
                                                    </tr>
                                                    <tr id="tr3Done" runat="server" visible="false">
                                                        <td valign="top" width="100%"><asp:Label ID="lbl3" runat="server" CssClass="approved" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td nowrap rowspan="2" valign="top"><asp:CheckBox ID="chk4" runat="server" CssClass="default" Enabled="false" /></td>
                                                        <td nowrap rowspan="2" valign="top"><asp:Image ID="img4" runat="server" ImageAlign="absMiddle" ImageUrl="/images/spacer.gif" /></td>
                                                        <td class="biggerbold">Step # 3 : Pre-Build Forms / Requests</td>
                                                    </tr>
                                                    <tr id="tr4" runat="server" visible="false">
                                                        <td valign="top" width="100%">
                                                            <asp:Panel ID="panPreBuildFormsYes" runat="server" Visible="false">
                                                                <table cellpadding="5" cellspacing="0" border="0">
                                                                    <tr>
                                                                        <td><b>NOTE:</b> You will not be able to continue until all of the requests listed below are completed!</td>
                                                                    </tr>
                                                                </table>
                                                                <br />
                                                                <table cellpadding="5" cellspacing="0" border="0" style="border:solid 1px #CCCCCC">
                                                                    <tr bgcolor="#EEEEEE">
                                                                        <td nowrap valign="top"></td>
                                                                        <td nowrap valign="top"><b>RequestID</b></td>
                                                                        <td nowrap valign="top"><b>Service</b></td>
                                                                        <td nowrap valign="top"><b>Created On</b></td>
                                                                        <td nowrap valign="top"><b>Assigned To</b></td>
                                                                        <td nowrap valign="top"><b>Assigned On</b></td>
                                                                        <td nowrap valign="top"><b>Last Updated</b></td>
                                                                        <td nowrap valign="top"><b>Status</b></td>
                                                                    </tr>
                                                                    <asp:repeater ID="rptPreBuildForms" runat="server">
                                                                        <ItemTemplate>
                                                                            <tr class="default">
                                                                                <td nowrap><asp:Label ID="lblImage" runat="server" /></td>
                                                                                <td nowrap><asp:Label ID="lblRequestID" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "RRID")%>' /></td>
                                                                                <td nowrap><asp:Label ID="lblService" runat="server" /></td>
                                                                                <td nowrap><asp:Label ID="lblCreated" runat="server" /></td>
                                                                                <td nowrap><asp:Label ID="lblAssignedTo" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "RRWID")%>' /></td>
                                                                                <td nowrap><asp:Label ID="lblAssignedOn" runat="server" /></td>
                                                                                <td nowrap><asp:Label ID="lblModified" runat="server" /></td>
                                                                                <td nowrap><asp:Label ID="lblStatus" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "status")%>' ToolTip='0' /></td>
                                                                            </tr>
                                                                        </ItemTemplate>
                                                                    </asp:repeater>
                                                                </table>
                                                            </asp:Panel>
                                                            <asp:Panel ID="panPreBuildFormsNo" runat="server" Visible="false">
                                                                <table cellpadding="5" cellspacing="0" border="0">
                                                                    <tr>
                                                                        <td><img src="/images/check.gif" border="0" align="absmiddle" /> There are no required pre-build forms / requests to be sent or completed.  Please click &quot;Save&quot; to continue to the next step.</td>
                                                                    </tr>
                                                                </table>
                                                            </asp:Panel>
                                                        </td>
                                                    </tr>
                                                    <tr id="tr4Wait" runat="server" visible="false">
                                                        <td valign="top" width="100%"><i>Waiting for Previous Step to Complete</i></td>
                                                    </tr>
                                                    <tr id="tr4Done" runat="server" visible="false">
                                                        <td valign="top" width="100%"><asp:Label ID="lbl4" runat="server" CssClass="approved" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td nowrap rowspan="2" valign="top"><asp:CheckBox ID="chk5" runat="server" CssClass="default" Enabled="false" /></td>
                                                        <td nowrap rowspan="2" valign="top"><asp:Image ID="img5" runat="server" ImageAlign="absMiddle" ImageUrl="/images/spacer.gif" /></td>
                                                        <td class="biggerbold">Step # 4 : Build the Server(s)</td>
                                                    </tr>
                                                    <tr id="tr5" runat="server" visible="false">
                                                        <td valign="top" width="100%">
                                                            <table cellpadding="5" cellspacing="5" border="0" style="border:solid 1px #999999" bgcolor="#f9f9f9">                                                            
                                                                <tr>
                                                                    <td><img src="/images/alert.gif" border="0" align="absmiddle" /> <b>WARNING:</b> Do not click &quot;Save&quot; until you are done with this step!</td>
                                                                </tr>
                                                            </table>
                                                            <br />
                                                            <table cellpadding="3" cellspacing="0" border="0">
                                                                <tr>
                                                                    <td>During this step, you are required to do the following...</td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <ul>
                                                                            <li>Initiate the Operating System installation using the established script(s)</li><br />
                                                                        </ul>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>At the completion of this step (once you click Save), the following will occur...</td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <ul>
                                                                            <li>The IP address and device name will be auto-generated in DNS (using the <b>IP Address # 1</b> column)</li><br />
                                                                            <li>All of the applicable forms / work orders from the following list will be generated and sent...</li><br />
                                                                                <ul>
                                                                                    <li>Storage / SAN</li>
                                                                                    <li>Backup / TSM / Legato</li>
                                                                                    <li>Birth Certificate</li>
                                                                                    <li>Service Center Request(s)</li>
                                                                                    <li>Pre-Production Online Task(s)</li>
                                                                                    <li>Audit Task(s)</li>
                                                                                </ul>
                                                                                <br />
                                                                        </ul>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr id="tr5Wait" runat="server" visible="false">
                                                        <td valign="top" width="100%"><i>Waiting for Previous Step to Complete</i></td>
                                                    </tr>
                                                    <tr id="tr5Done" runat="server" visible="false">
                                                        <td valign="top" width="100%"><asp:Label ID="lbl5" runat="server" CssClass="approved" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td nowrap rowspan="2" valign="top"><asp:CheckBox ID="chk6" runat="server" CssClass="default" Enabled="false" /></td>
                                                        <td nowrap rowspan="2" valign="top"><asp:Image ID="img6" runat="server" ImageAlign="absMiddle" ImageUrl="/images/spacer.gif" /></td>
                                                        <td class="biggerbold">Step # 5 : Post-Build Forms / Requests</td>
                                                    </tr>
                                                    <tr id="tr6" runat="server" visible="false">
                                                        <td valign="top" width="100%">
                                                            <asp:Panel ID="panPostBuildFormsYes" runat="server" Visible="false">
                                                                <table cellpadding="5" cellspacing="0" border="0">
                                                                    <tr>
                                                                        <td><b>NOTE:</b> You will not be able to continue until all of the requests listed below are completed!</td>
                                                                    </tr>
                                                                </table>
                                                                <br />
                                                                <table cellpadding="5" cellspacing="0" border="0" style="border:solid 1px #CCCCCC">
                                                                    <tr bgcolor="#EEEEEE">
                                                                        <td nowrap valign="top"></td>
                                                                        <td nowrap valign="top"><b>Service</b></td>
                                                                        <td nowrap valign="top"><b>Created On</b></td>
                                                                        <td nowrap valign="top"><b>Assigned To</b></td>
                                                                        <td nowrap valign="top"><b>Comments</b></td>
                                                                        <td nowrap valign="top"><b>Status</b></td>
                                                                    </tr>
                                                                    <asp:repeater ID="rptPostBuildForms" runat="server">
                                                                        <ItemTemplate>
                                                                            <tr class="default">
                                                                                <td nowrap><asp:Label ID="lblImage" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "status")%>' ToolTip='<%# DataBinder.Eval(Container.DataItem, "step")%>' /></td>
                                                                                <td nowrap><asp:Label ID="lblName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "name")%>' ToolTip='<%# DataBinder.Eval(Container.DataItem, "id")%>' /></td>
                                                                                <td nowrap><asp:Label ID="lblCreated" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "created")%>' /></td>
                                                                                <td nowrap><asp:Label ID="lblAssignedTo" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "username")%>' /></td>
                                                                                <td nowrap><%# DataBinder.Eval(Container.DataItem, "comments")%></td>
                                                                                <td nowrap><asp:Label ID="lblStatus" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "completed")%>' ToolTip='<%# DataBinder.Eval(Container.DataItem, "non_transparent")%>' /></td>
                                                                            </tr>
                                                                        </ItemTemplate>
                                                                    </asp:repeater>
                                                                </table>
                                                            </asp:Panel>
                                                            <asp:Panel ID="panPostBuildFormsNo" runat="server" Visible="false">
                                                                <table cellpadding="5" cellspacing="0" border="0">
                                                                    <tr>
                                                                        <td><img src="/images/check.gif" border="0" align="absmiddle" /> There are no required ost-build forms / requests to be sent or completed.  Please click &quot;Save&quot; to continue to the next step.</td>
                                                                    </tr>
                                                                </table>
                                                            </asp:Panel>
                                                            <br />
                                                            <table cellpadding="3" cellspacing="0" border="0">
                                                                <tr>
                                                                    <td><asp:Label ID="lblForms" runat="server" CssClass="default" /></td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr id="tr6Wait" runat="server" visible="false">
                                                        <td valign="top" width="100%"><i>Waiting for Previous Step to Complete</i></td>
                                                    </tr>
                                                    <tr id="tr6Done" runat="server" visible="false">
                                                        <td valign="top" width="100%"><asp:Label ID="lbl6" runat="server" CssClass="approved" /></td>
                                                    </tr>
                                                    <asp:Panel ID="panMove" runat="server" Visible="false">
                                                    <tr>
                                                        <td nowrap rowspan="2" valign="top"><asp:CheckBox ID="chk7" runat="server" CssClass="default" Enabled="false" /></td>
                                                        <td nowrap rowspan="2" valign="top"><asp:Image ID="img7" runat="server" ImageAlign="absMiddle" ImageUrl="/images/spacer.gif" /></td>
                                                        <td class="biggerbold">Step # 6 : Reserve Assets (for Final Location)</td>
                                                    </tr>
                                                    <tr id="tr7" runat="server" visible="false">
                                                        <td valign="top" width="100%">
                                                        </td>
                                                    </tr>
                                                    <tr id="tr7Wait" runat="server" visible="false">
                                                        <td valign="top" width="100%"><i>Waiting for Previous Step to Complete</i></td>
                                                    </tr>
                                                    <tr id="tr7Done" runat="server" visible="false">
                                                        <td valign="top" width="100%"><asp:Label ID="lbl7" runat="server" CssClass="approved" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td nowrap rowspan="2" valign="top"><asp:CheckBox ID="chk8" runat="server" CssClass="default" Enabled="false" /></td>
                                                        <td nowrap rowspan="2" valign="top"><asp:Image ID="img8" runat="server" ImageAlign="absMiddle" ImageUrl="/images/spacer.gif" /></td>
                                                        <td class="biggerbold">Step # 7 : Final Location Forms / Requests</td>
                                                    </tr>
                                                    <tr id="tr8" runat="server" visible="false">
                                                        <td valign="top" width="100%">
                                                        </td>
                                                    </tr>
                                                    <tr id="tr8Wait" runat="server" visible="false">
                                                        <td valign="top" width="100%"><i>Waiting for Previous Step to Complete</i></td>
                                                    </tr>
                                                    <tr id="tr8Done" runat="server" visible="false">
                                                        <td valign="top" width="100%"><asp:Label ID="lbl8" runat="server" CssClass="approved" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td nowrap rowspan="2" valign="top"><asp:CheckBox ID="chk9" runat="server" CssClass="default" Enabled="false" /></td>
                                                        <td nowrap rowspan="2" valign="top"><asp:Image ID="img9" runat="server" ImageAlign="absMiddle" ImageUrl="/images/spacer.gif" /></td>
                                                        <td class="biggerbold">Step # 8 : Release Build Assets (for Final Location)</td>
                                                    </tr>
                                                    <tr id="tr9" runat="server" visible="false">
                                                        <td valign="top" width="100%">
                                                        </td>
                                                    </tr>
                                                    <tr id="tr9Wait" runat="server" visible="false">
                                                        <td valign="top" width="100%"><i>Waiting for Previous Step to Complete</i></td>
                                                    </tr>
                                                    <tr id="tr9Done" runat="server" visible="false">
                                                        <td valign="top" width="100%"><asp:Label ID="lbl9" runat="server" CssClass="approved" /></td>
                                                    </tr>
                                                    </asp:Panel>
                                                </table>
		                                    </div>
		                                    <div id="divStatusUpdates" style='<%=boolStatusUpdates == true ? "display:inline" : "display:none" %>'>
		                                        <br />
                                                <table width="100%" cellpadding="4" cellspacing="3" border="0">
                                                  <tr>
                                                        <td colspan="2" class="greentableheader">Status Updates</td>
                                                    </tr>
                                                    <tr>
                                                        <td nowrap>Status:</td>
                                                        <td width="100%">
                                                            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="default" >
                                                                <asp:ListItem Text="-- SELECT --" Value="0" />
                                                                <asp:ListItem Text="Red" Value="1" />
                                                                <asp:ListItem Text="Yellow" Value="2" />
                                                                <asp:ListItem Text="Green" Value="3" />
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2"><b>Comments / Issues:</b></td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2"><asp:TextBox ID="txtComments" runat="server" CssClass="default" Width="100%" TextMode="MultiLine" Rows="13" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2"><asp:Button ID="btnStatus" runat="server" CssClass="default" Width="150" Text="Submit Status" OnClick="btnStatus_Click" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">&nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <table width="100%" cellpadding="3" cellspacing="0" border="0" style="border:solid 1px #CCCCCC">
                                                                <tr bgcolor="#EEEEEE">
                                                                    <td nowrap><b>Date</b></td>
                                                                    <td nowrap>&nbsp;</td>
                                                                    <td nowrap><b>Status</b></td>
                                                                    <td nowrap>&nbsp;</td>
                                                                    <td nowrap><b>Comments</b></td>
                                                                </tr>
                                                                <asp:repeater ID="rptStatus" runat="server">
                                                                    <ItemTemplate>
                                                                        <tr class="default">
                                                                            <td nowrap valign="top"><a href="javascript:void(0);" onclick="ShowStatus('<%# DataBinder.Eval(Container.DataItem, "id") %>','r');"><%# DateTime.Parse(DataBinder.Eval(Container.DataItem, "modified").ToString()).ToShortDateString() %>&nbsp;<%# DateTime.Parse(DataBinder.Eval(Container.DataItem, "modified").ToString()).ToShortTimeString() %></a></td>
                                                                            <td nowrap>&nbsp;</td>
                                                                            <td nowrap valign="top"><asp:Label ID="lblStatus" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "status") %>' /></td>
                                                                            <td nowrap>&nbsp;</td>
                                                                            <td width="100%" valign="top">
                                                                                <div id="div200_<%# DataBinder.Eval(Container.DataItem, "id") %>" style="display:inline">
                                                                                    <%# (DataBinder.Eval(Container.DataItem, "comments").ToString().Length > 200 ? DataBinder.Eval(Container.DataItem, "comments").ToString().Substring(0, 200) + " ...&nbsp;&nbsp;&nbsp;[<a href=\"javascript:void(0);\" onclick=\"ShowHideDiv('div200_" + DataBinder.Eval(Container.DataItem, "id") + "', 'none');ShowHideDiv('divMore_" + DataBinder.Eval(Container.DataItem, "id") + "', 'inline');\">More</a>]" : DataBinder.Eval(Container.DataItem, "comments").ToString())%>
                                                                                </div>
                                                                                <div id="divMore_<%# DataBinder.Eval(Container.DataItem, "id") %>" style="display:none">
                                                                                    <%# DataBinder.Eval(Container.DataItem, "comments").ToString() + "&nbsp;&nbsp;&nbsp;[<a href=\"javascript:void(0);\" onclick=\"ShowHideDiv('div200_" + DataBinder.Eval(Container.DataItem, "id") + "', 'inline');ShowHideDiv('divMore_" + DataBinder.Eval(Container.DataItem, "id") + "', 'none');\">Hide</a>]"%>
                                                                                </div>
                                                                            </td>
                                                                        </tr>
                                                                    </ItemTemplate>
                                                                </asp:repeater>
                                                            <tr>
                                                                <td colspan="5">
                                                                    <asp:Label ID="lblNoStatus" runat="server" CssClass="default" Visible="false" Text="<img src='/images/alert.gif' border='0' align='absmiddle'> There are no updates" />
                                                                </td>
                                                            </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
		                                    </div>
		                                    <div id="divChange" style='<%=boolChange == true ? "display:inline" : "display:none" %>'>
		                                        <br />
                                                <table width="100%" cellpadding="4" cellspacing="3" border="0">
                                                    <tr>
                                                        <td colspan="2" class="greentableheader">Change Controls</td>
                                                    </tr>
                                                    <tr>
                                                        <td nowrap>Number:</td>
                                                        <td width="100%"><asp:TextBox ID="txtNumber" runat="server" CssClass="default" Width="150" MaxLength="15" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td nowrap>Date:</td>
                                                        <td width="100%"><asp:TextBox ID="txtDate" runat="server" CssClass="default" Width="100" MaxLength="10" /> <asp:ImageButton ID="imgDate" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calendar.gif" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td nowrap>Time:</td>
                                                        <td width="100%"><asp:TextBox ID="txtTime" runat="server" CssClass="default" Width="100" MaxLength="10" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2"><b>Comments:</b></td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2"><asp:TextBox ID="txtChange" runat="server" CssClass="default" Width="500" TextMode="MultiLine" Rows="7" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2"><asp:Button ID="btnChange" runat="server" CssClass="default" Width="150" Text="Submit Change" OnClick="btnChange_Click" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">&nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <table width="100%" cellpadding="3" cellspacing="0" border="0" style="border:solid 1px #CCCCCC">
                                                                <tr bgcolor="#EEEEEE">
                                                                    <td nowrap><b>Number</b></td>
                                                                    <td nowrap><b>Date</b></td>
                                                                    <td nowrap>&nbsp;</td>
                                                                </tr>
                                                                <asp:repeater ID="rptChange" runat="server">
                                                                    <ItemTemplate>
                                                                        <tr class="default">
                                                                            <td nowrap><a href="javascript:void(0);" onclick="ShowChange('<%# DataBinder.Eval(Container.DataItem, "changeid") %>');"><%# DataBinder.Eval(Container.DataItem, "number") %></a></td>
                                                                            <td nowrap><%# DateTime.Parse(DataBinder.Eval(Container.DataItem, "implementation").ToString()).ToLongDateString() %> @ <%# DateTime.Parse(DataBinder.Eval(Container.DataItem, "implementation").ToString()).ToLongTimeString() %></td>
                                                                            <td align="right">[<asp:LinkButton ID="btnDeleteChange" runat="server" Text="Delete" OnClick="btnDeleteChange_Click" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "changeid") %>' />]</td>
                                                                        </tr>
                                                                    </ItemTemplate>
                                                                </asp:repeater>
                                                            <tr>
                                                                <td colspan="3">
                                                                    <asp:Label ID="lblNoChange" runat="server" CssClass="default" Visible="false" Text="<img src='/images/alert.gif' border='0' align='absmiddle'> There are no change controls" />
                                                                </td>
                                                            </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
		                                    </div>
		                                    <div id="divDocuments" style='<%=boolDocuments == true ? "display:inline" : "display:none" %>'>
		                                        <br />
                                                <table width="100%" cellpadding="4" cellspacing="3" border="0">
                                                    <tr>
                                                        <td class="greentableheader">Attached Files</td>
                                                        <td align="right"><asp:CheckBox ID="chkDescription" runat="server" CssClass="default" Text="Show Descriptions" AutoPostBack="true" OnCheckedChanged="chkDescription_Change" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2"><asp:Label ID="lblDocuments" runat="server" CssClass="default" /></td>
                                                    </tr>
                                                </table>
		                                    </div>
		                                </td>
		                            </tr>
		                        </table>
		                    </td>
	                    </tr>
	                </table>
            </td>
        </tr>
    </table>
</asp:Panel>
<asp:Panel ID="panDenied" runat="server" Visible="false">
    <table width="100%" cellpadding="2" cellspacing="2" border="0">
        <tr>
            <td><img src="/images/spacer.gif" border="0" width="15" height="1" /></td>
            <td class="header"><img src="/images/bigX.gif" border="0" align="absmiddle" /> Access Denied</td>
        </tr>
        <tr><td colspan="2">&nbsp;</td></tr>
        <tr>
            <td><img src="/images/spacer.gif" border="0" width="15" height="1" /></td>
            <td>You do not have rights to view this item.</td>
        </tr>
        <tr><td colspan="2">&nbsp;</td></tr>
        <tr>
            <td colspan="2"><hr size="1" noshade /></td>
        </tr>
        <tr>
            <td colspan="2" align="right">
                <table width="100%" cellpadding="0" cellspacing="0" border="0">
                    <tr>
                        <td class="footer"></td>
                        <td align="right"><asp:Button ID="btnDenied" runat="server" CssClass="default" Width="75" Text="Close" /></td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Panel>
<asp:Label ID="lblResourceWorkflow" runat="server" Visible="false" />
<asp:Label ID="lblModelID" runat="server" Visible="false" />
<asp:HiddenField ID="hdnTab" runat="server" />