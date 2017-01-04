<%@ Page Language="C#" MasterPageFile="~/datapoint.Master" AutoEventWireup="true" CodeBehind="server.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.server" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" runat="server">
    <script type="text/javascript">
    var intStepStart = 0;
    var intStepGoto = 0;
    var intStepRebuild = '<%=strRebuildStep %>';
    function RedoStep(intStep, oRedo, oStep, oStart, oGoto, oRebuild) {
        oStep = document.getElementById(oStep);
        oStart = document.getElementById(oStart);
        oGoto = document.getElementById(oGoto);
        oRebuild = document.getElementById(oRebuild);
        if (oRedo.checked == true) {
            if (oRebuild.checked == true)
                oRebuild.click();
            var intStepTemp = parseInt(oStep.value);
            intStepStart = parseInt(oStart.value);
            intStepGoto = parseInt(oGoto.value);
            oStart.value = intStepTemp + 1;
            oGoto.value = intStep;
            oStep.readOnly = true;
            oStart.readOnly = true;
            oGoto.readOnly = true;
        }
        else {
            oStep.readOnly = false;
            oStart.readOnly = false;
            oGoto.readOnly = false;
            oStep.value = intStep;
            oStart.value = intStepStart;
            oGoto.value = intStepGoto;
        }
    }
    function RebuildStep(intStep, oRebuild, oStep, oStart, oGoto, oRedo) {
        oStep = document.getElementById(oStep);
        oStart = document.getElementById(oStart);
        oGoto = document.getElementById(oGoto);
        oRedo = document.getElementById(oRedo);
        alert('WARNING: This rebuild option will start from the QUEUE THE DEVICE TO BE IMAGED step.\n\nIt will NOT power off the asset or destroy it from Virtual Center.\n\nPlease do this now before clicking SAVE.');
        if (oRebuild.checked == true) {
            if (oRedo.checked == true)
                oRedo.click();
            intStepStart = parseInt(oStart.value);
            intStepGoto = parseInt(oGoto.value);
            oStep.value = intStepRebuild;
            oStart.value = "0";
            oGoto.value = "0";
            oStep.readOnly = true;
            oStart.readOnly = true;
            oGoto.readOnly = true;
        }
        else {
            oStep.readOnly = false;
            oStart.readOnly = false;
            oGoto.readOnly = false;
            oStep.value = intStep;
            oStart.value = intStepStart;
            oGoto.value = intStepGoto;
        }
    }
    </script>

    <asp:Panel ID="panAllow" runat="server" Visible="false">
    <table width="100%" cellpadding="0" cellspacing="5" border="0">
        <tr id="cntrlButtons">
            <td rowspan="2"><img src="/images/assets.gif" border="0" align="absmiddle" /></td>
            <td class="header" nowrap valign="bottom"><asp:Label ID="lblHeader" runat="server" CssClass="header" /></td>
            <td width="100%" rowspan="2" align="right">
                <table cellpadding="1" cellspacing="4" border="0">
                    <tr>
                        <td nowrap><asp:LinkButton ID="btnNew" runat="server" Text="<img src='/images/new-icon.gif' border='0' align='absmiddle' /><img src='/images/spacer.gif' border='0' width='5' height='1' />New Search" OnClick="btnNew_Click" /></td>
                        <td>|</td>
                        <td nowrap><asp:LinkButton ID="btnSave" runat="server" Text="<img src='/images/save-icon.gif' border='0' align='absmiddle' /><img src='/images/spacer.gif' border='0' width='5' height='1' />Save" OnClick="btnSave_Click" /></td>
                        <td>|</td>
                        <td nowrap><asp:LinkButton ID="btnSaveClose" runat="server" Text="<img src='/images/save-icon.gif' border='0' align='absmiddle' /><img src='/images/spacer.gif' border='0' width='5' height='1' />Save & Close" OnClick="btnSaveClose_Click" /></td>
                        <td>|</td>
                        <td nowrap><asp:LinkButton ID="btnPrint" runat="server" Text="<img src='/images/print-icon.gif' border='0' align='absmiddle' /><img src='/images/spacer.gif' border='0' width='5' height='1' />Print" /></td>
                        <td>|</td>
                        <td nowrap><asp:LinkButton ID="btnClose" runat="server" Text="<img src='/images/close-icon.gif' border='0' align='absmiddle' /><img src='/images/spacer.gif' border='0' width='5' height='1' />Close" /></td>
                    </tr>
                    <asp:Panel ID="panSave" runat="server" Visible="false">
                    <tr>
                        <td colspan="7" class="bigCheck" align="center"><img src="/images/bigCheck.gif" border="0" align="absmiddle" /> Update Successful</td>
                    </tr>
                    </asp:Panel>
                    <asp:Panel ID="panError" runat="server" Visible="false">
                    <tr>
                        <td colspan="7" class="bigError" align="center"><img src="/images/bigError.gif" border="0" align="absmiddle" /> <asp:Label ID="lblError" runat="server" /></td>
                    </tr>
                    </asp:Panel>
                </table>
            </td>
        </tr>
        <tr id="cntrlButtons2">
            <td nowrap valign="top"><asp:Label ID="lblHeaderSub" runat="server" CssClass="default" /></td>
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
    <br />
    <%=strMenuTab1 %>
    <table width="100%" height="100%" cellpadding="10" cellspacing="5" border="0">
        <tr>
            <td valign="top">
                <div id="divMenu1" class="tabbing">
                    <div style="display:none">
                        <table width="100%" cellpadding="5" cellspacing="2" border="0">
                            <tr>
                                <td class="header" colspan="2">Platform Information&nbsp;&nbsp;<asp:Label ID="lblServerID" runat="server" CssClass="default" /></td>
                            </tr>
                            <tr>
                                <td nowrap><asp:Label ID="fldName" runat="server" CssClass="default" Text="Name:" /></td>
                                <td width="100%"><asp:Label ID="lblName" runat="server" CssClass="default" /><asp:TextBox ID="txtName" runat="server" CssClass="default" MaxLength="100" Width="300" /></td>
                            </tr>
                            <tr>
                                <td nowrap><asp:Label ID="fldPlatformSerial" runat="server" CssClass="default" Text="Serial Number:" /></td>
                                <td width="100%"><asp:Label ID="lblPlatformSerial" runat="server" CssClass="default" /><asp:TextBox ID="txtPlatformSerial" runat="server" CssClass="default" MaxLength="100" Width="400" /> <asp:Button ID="btnPlatformSerial" runat="server" CssClass="default" Width="75" Text="Lookup" Visible="false" /></td>
                            </tr>
                            <tr>
                                <td nowrap><asp:Label ID="fldPlatformAsset" runat="server" CssClass="default" Text="Asset Tag:" /></td>
                                <td width="100%"><asp:Label ID="lblPlatformAsset" runat="server" CssClass="default" /><asp:TextBox ID="txtPlatformAsset" runat="server" CssClass="default" MaxLength="100" Width="400" /></td>
                            </tr>
                            <tr>
                                <td nowrap><asp:Label ID="fldPlatformDeviceRole" runat="server" CssClass="default" Text="Device Role:" /></td>
                                <td width="100%"><asp:Label ID="lblPlatformDeviceRole" runat="server" CssClass="default" /><asp:DropDownList ID="ddlPlatformDeviceRole" runat="server" CssClass="default" Width="400" /></td>
                            </tr>
                            <tr>
                                <td nowrap><asp:Label ID="fldPlatformOS" runat="server" CssClass="default" Text="Device OS:" /></td>
                                <td width="100%"><asp:Label ID="lblPlatformOS" runat="server" CssClass="default" /><asp:DropDownList ID="ddlPlatformOS" runat="server" CssClass="default" Width="400" /></td>
                            </tr>
                            <tr>
                                <td nowrap><asp:Label ID="fldPlatformServicePack" runat="server" CssClass="default" Text="Service Pack:" /></td>
                                <td width="100%"><asp:Label ID="lblPlatformServicePack" runat="server" CssClass="default" /><asp:DropDownList ID="ddlPlatformServicePack" runat="server" CssClass="default" Width="400" /></td>
                            </tr>
                            <tr>
                                <td nowrap><asp:Label ID="fldPlatformClass" runat="server" CssClass="default" Text="Current Class:" /></td>
                                <td width="100%"><asp:Label ID="lblPlatformClass" runat="server" CssClass="default" /><asp:DropDownList ID="ddlPlatformClass" runat="server" CssClass="default" Width="400" /></td>
                            </tr>
                            <tr>
                                <td nowrap><asp:Label ID="fldPlatformEnvironment" runat="server" CssClass="default" Text="Current Environment:" /></td>
                                <td width="100%"><asp:Label ID="lblPlatformEnvironment" runat="server" CssClass="default" /><asp:DropDownList ID="ddlPlatformEnvironment" runat="server" CssClass="default" Width="400" /></td>
                            </tr>
                            <tr>
                                <td nowrap><asp:Label ID="fldPlatformDomain" runat="server" CssClass="default" Text="Domain:" /></td>
                                <td width="100%"><asp:Label ID="lblPlatformDomain" runat="server" CssClass="default" /><asp:DropDownList ID="ddlPlatformDomain" runat="server" CssClass="default" Width="400" /></td>
                            </tr>
                            <tr>
                                <td nowrap><asp:Label ID="fldPlatformCluster" runat="server" CssClass="default" Text="Cluster:" /></td>
                                <td width="100%"><asp:Label ID="lblPlatformCluster" runat="server" CssClass="default" /><asp:TextBox ID="txtPlatformCluster" runat="server" CssClass="default" MaxLength="100" Width="400" /> <asp:Button ID="btnPlatformCluster" runat="server" CssClass="default" Width="75" Text="Lookup" Visible="false" /></td>
                            </tr>
                            <tr>
                                <td nowrap><asp:Label ID="fldPlatformBuildStarted" runat="server" CssClass="default" Text="Build Started:" /></td>
                                <td width="100%"><asp:Label ID="lblPlatformBuildStarted" runat="server" CssClass="default" /><asp:TextBox ID="txtPlatformBuildStarted" runat="server" CssClass="default" MaxLength="10" Width="100" /> <asp:ImageButton ID="imgPlatformBuildStarted" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calendar.gif" Visible="false" /></td>
                            </tr>
                            <tr>
                                <td nowrap><asp:Label ID="fldPlatformBuildCompleted" runat="server" CssClass="default" Text="Build Completed:" /></td>
                                <td width="100%"><asp:Label ID="lblPlatformBuildCompleted" runat="server" CssClass="default" /><asp:TextBox ID="txtPlatformBuildCompleted" runat="server" CssClass="default" MaxLength="10" Width="100" /> <asp:ImageButton ID="imgPlatformBuildCompleted" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calendar.gif" Visible="false" /></td>
                            </tr>
                            <tr>
                                <td nowrap><asp:Label ID="fldPlatformBuildReady" runat="server" CssClass="default" Text="Tasks Completed:" /></td>
                                <td width="100%"><asp:Label ID="lblPlatformBuildReady" runat="server" CssClass="default" /><asp:TextBox ID="txtPlatformBuildReady" runat="server" CssClass="default" MaxLength="10" Width="100" /> <asp:ImageButton ID="imgPlatformBuildReady" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calendar.gif" Visible="false" /></td>
                            </tr>
                            <tr>
                                <td nowrap><asp:Label ID="fldPlatformRebuild" runat="server" CssClass="default" Text="Rebuild Started:" /></td>
                                <td width="100%"><asp:Label ID="lblPlatformRebuild" runat="server" CssClass="default" /><asp:TextBox ID="txtPlatformRebuild" runat="server" CssClass="default" MaxLength="10" Width="100" /> <asp:ImageButton ID="imgPlatformRebuild" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calendar.gif" Visible="false" /></td>
                            </tr>
                            <tr>
                                <td nowrap><asp:Label ID="fldPlatformDecommissioned" runat="server" CssClass="default" Text="Decommission Date:" /></td>
                                <td width="100%"><asp:Label ID="lblPlatformDecommissioned" runat="server" CssClass="default" /><asp:TextBox ID="txtPlatformDecommissioned" runat="server" CssClass="default" MaxLength="10" Width="100" /> <asp:ImageButton ID="imgPlatformDecommissioned" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calendar.gif" Visible="false" /></td>
                            </tr>
                            <tr>
                                <td nowrap><asp:Label ID="fldPlatformDRCounterPart" runat="server" CssClass="default" Text="DR Counterpart:" /></td>
                                <td width="100%"><asp:Label ID="lblPlatformDRCounterPart" runat="server" CssClass="default" /><asp:TextBox ID="txtPlatformDRCounterPart" runat="server" CssClass="default" MaxLength="100" Width="400" /> <asp:Button ID="btnPlatformDRCounterPart" runat="server" CssClass="default" Width="75" Text="Lookup" Visible="false" /></td>
                            </tr>
                            <tr>
                                <td nowrap><asp:Label ID="fldPlatformHACounterPart" runat="server" CssClass="default" Text="HA Counterpart:" /></td>
                                <td width="100%"><asp:Label ID="lblPlatformHACounterPart" runat="server" CssClass="default" /><asp:TextBox ID="txtPlatformHACounterPart" runat="server" CssClass="default" MaxLength="100" Width="400" /> <asp:Button ID="btnPlatformHACounterPart" runat="server" CssClass="default" Width="75" Text="Lookup" Visible="false" /></td>
                            </tr>
                        </table>
                    </div>
                    <div style="display:none">
                        <table width="100%" cellpadding="5" cellspacing="2" border="0">
                            <tr>
                                <td class="header" colspan="2"><asp:Label ID="fldIPs" runat="server" CssClass="header" Text="IP Addresses" /></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Panel ID="panIPs" runat="server" Visible="false">
                                        <table cellpadding="5" cellspacing="2" border="0">
                                            <tr>
                                                <td>DHCP Address:</td>
                                                <td>
                                                    <asp:TextBox ID="txtIPDHCP1" runat="server" CssClass="default" Width="35" MaxLength="3" /> . 
                                                    <asp:TextBox ID="txtIPDHCP2" runat="server" CssClass="default" Width="35" MaxLength="3" /> . 
                                                    <asp:TextBox ID="txtIPDHCP3" runat="server" CssClass="default" Width="35" MaxLength="3" /> . 
                                                    <asp:TextBox ID="txtIPDHCP4" runat="server" CssClass="default" Width="35" MaxLength="3" />
                                                </td>
                                                <td><asp:CheckBox ID="chkDHCP" runat="server" CssClass="default" Text="Change DHCP Address" /></td>
                                            </tr>
                                            <tr>
                                                <td colspan="3" class="footer"> <b>NOTE:</b> This address comes from the imaging process and has no relevancy after the device is finished building.</td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td valign="top"><br />Assigned Address(es):</td>
                                                <td colspan="2">
                                                    <table cellpadding="2" cellspacing="0" border="0">
	                                                    <tr>
		                                                    <td><asp:ListBox ID="lstIPAssigned" runat="server" CssClass="default" Width="200" Rows="5" /></td>
		                                                    <td valign="top">
		                                                        <asp:ImageButton ID="btnIPAssignedAdd" runat="server" CssClass="default" ImageUrl="/images/arrowLeft.gif" />
		                                                        <br />
		                                                        <asp:ImageButton ID="btnIPAssignedRemove" runat="server" CssClass="default" ImageUrl="/images/arrowCancel.gif" />
		                                                    </td>
		                                                    <td valign="top">
                                                                <asp:TextBox ID="txtIPAssigned1" runat="server" CssClass="default" Width="40" MaxLength="3" /> . 
                                                                <asp:TextBox ID="txtIPAssigned2" runat="server" CssClass="default" Width="40" MaxLength="3" /> . 
                                                                <asp:TextBox ID="txtIPAssigned3" runat="server" CssClass="default" Width="40" MaxLength="3" /> . 
                                                                <asp:TextBox ID="txtIPAssigned4" runat="server" CssClass="default" Width="40" MaxLength="3" />
		                                                    </td>
	                                                    </tr>
	                                                    <tr>
	                                                        <td colspan="3" class="footer"> <b>NOTE:</b> These addresses get assigned as soon as the server is finished building.</td>
	                                                    </tr>
                                                    </table>
                                                    <asp:HiddenField ID="hdnIPAssigned" runat="server" /><asp:HiddenField ID="hdnIPAssignedExists" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td valign="top"><br />Final Address(es):</td>
                                                <td colspan="2">
                                                    <table cellpadding="2" cellspacing="0" border="0">
	                                                    <tr>
		                                                    <td><asp:ListBox ID="lstIPFinal" runat="server" CssClass="default" Width="200" Rows="5" /></td>
		                                                    <td valign="top">
		                                                        <asp:ImageButton ID="btnIPFinalAdd" runat="server" CssClass="default" ImageUrl="/images/arrowLeft.gif" />
		                                                        <br />
		                                                        <asp:ImageButton ID="btnIPFinalRemove" runat="server" CssClass="default" ImageUrl="/images/arrowCancel.gif" />
		                                                    </td>
		                                                    <td valign="top">
                                                                <asp:TextBox ID="txtIPFinal1" runat="server" CssClass="default" Width="40" MaxLength="3" /> . 
                                                                <asp:TextBox ID="txtIPFinal2" runat="server" CssClass="default" Width="40" MaxLength="3" /> . 
                                                                <asp:TextBox ID="txtIPFinal3" runat="server" CssClass="default" Width="40" MaxLength="3" /> . 
                                                                <asp:TextBox ID="txtIPFinal4" runat="server" CssClass="default" Width="40" MaxLength="3" />
		                                                    </td>
	                                                    </tr>
	                                                    <tr>
	                                                        <td colspan="3" class="footer"> <b>NOTE:</b> These addresses are applied to the server once in its final location.</td>
	                                                    </tr>
                                                    </table>
                                                    <asp:HiddenField ID="hdnIPFinal" runat="server" /><asp:HiddenField ID="hdnIPFinalExists" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">&nbsp;</td>
                                            </tr>
                                            <tr id="trVMotion" runat="server" visible="false">
                                                <td valign="top"><br />VMotion Address(es):</td>
                                                <td colspan="2">
                                                    <table cellpadding="2" cellspacing="0" border="0">
	                                                    <tr>
		                                                    <td><asp:ListBox ID="lstIPVMotion" runat="server" CssClass="default" Width="200" Rows="5" /></td>
		                                                    <td valign="top">
		                                                        <asp:ImageButton ID="btnIPVMotionAdd" runat="server" CssClass="default" ImageUrl="/images/arrowLeft.gif" />
		                                                        <br />
		                                                        <asp:ImageButton ID="btnIPVMotionRemove" runat="server" CssClass="default" ImageUrl="/images/arrowCancel.gif" />
		                                                    </td>
		                                                    <td valign="top">
                                                                <asp:TextBox ID="txtIPVMotion1" runat="server" CssClass="default" Width="40" MaxLength="3" /> . 
                                                                <asp:TextBox ID="txtIPVMotion2" runat="server" CssClass="default" Width="40" MaxLength="3" /> . 
                                                                <asp:TextBox ID="txtIPVMotion3" runat="server" CssClass="default" Width="40" MaxLength="3" /> . 
                                                                <asp:TextBox ID="txtIPVMotion4" runat="server" CssClass="default" Width="40" MaxLength="3" />
		                                                    </td>
	                                                    </tr>
	                                                    <tr>
	                                                        <td colspan="3" class="footer"> <b>NOTE:</b> These addresses are applicable only to VMware hosts.</td>
	                                                    </tr>
                                                    </table>
                                                    <asp:HiddenField ID="hdnIPVMotion" runat="server" /><asp:HiddenField ID="hdnIPVMotionExists" runat="server" />
                                                </td>
                                            </tr>
                                            <tr id="trPrivate" runat="server" visible="false">
                                                <td valign="top"><br />Private Address(es):</td>
                                                <td colspan="2">
                                                    <table cellpadding="2" cellspacing="0" border="0">
	                                                    <tr>
		                                                    <td><asp:ListBox ID="lstIPPrivate" runat="server" CssClass="default" Width="200" Rows="5" /></td>
		                                                    <td valign="top">
		                                                        <asp:ImageButton ID="btnIPPrivateAdd" runat="server" CssClass="default" ImageUrl="/images/arrowLeft.gif" />
		                                                        <br />
		                                                        <asp:ImageButton ID="btnIPPrivateRemove" runat="server" CssClass="default" ImageUrl="/images/arrowCancel.gif" />
		                                                    </td>
		                                                    <td valign="top">
                                                                <asp:TextBox ID="txtIPPrivate1" runat="server" CssClass="default" Width="40" MaxLength="3" /> . 
                                                                <asp:TextBox ID="txtIPPrivate2" runat="server" CssClass="default" Width="40" MaxLength="3" /> . 
                                                                <asp:TextBox ID="txtIPPrivate3" runat="server" CssClass="default" Width="40" MaxLength="3" /> . 
                                                                <asp:TextBox ID="txtIPPrivate4" runat="server" CssClass="default" Width="40" MaxLength="3" />
		                                                    </td>
	                                                    </tr>
	                                                    <tr>
	                                                        <td colspan="3" class="footer"> <b>NOTE:</b> These addresses are applicable only to Clustered servers.</td>
	                                                    </tr>
                                                    </table>
                                                    <asp:HiddenField ID="hdnIPPrivate" runat="server" /><asp:HiddenField ID="hdnIPPrivateExists" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td valign="top"><br />Backup Address(es):<br /><br /><i>(Avamar, Legato)</i></td>
                                                <td colspan="2">
                                                    <table cellpadding="2" cellspacing="0" border="0">
	                                                    <tr>
		                                                    <td><asp:ListBox ID="lstIPAvamar" runat="server" CssClass="default" Width="200" Rows="5" /></td>
		                                                    <td valign="top">
		                                                        <asp:ImageButton ID="btnIPAvamarAdd" runat="server" CssClass="default" ImageUrl="/images/arrowLeft.gif" />
		                                                        <br />
		                                                        <asp:ImageButton ID="btnIPAvamarRemove" runat="server" CssClass="default" ImageUrl="/images/arrowCancel.gif" />
		                                                    </td>
		                                                    <td valign="top">
                                                                <asp:TextBox ID="txtIPAvamar1" runat="server" CssClass="default" Width="40" MaxLength="3" /> . 
                                                                <asp:TextBox ID="txtIPAvamar2" runat="server" CssClass="default" Width="40" MaxLength="3" /> . 
                                                                <asp:TextBox ID="txtIPAvamar3" runat="server" CssClass="default" Width="40" MaxLength="3" /> . 
                                                                <asp:TextBox ID="txtIPAvamar4" runat="server" CssClass="default" Width="40" MaxLength="3" />
		                                                    </td>
	                                                    </tr>
	                                                    <tr>
	                                                        <td colspan="3" class="footer"> <b>NOTE:</b> These addresses are applicable only to VMware guests being backed up by Avamar.</td>
	                                                    </tr>
                                                    </table>
                                                    <asp:HiddenField ID="hdnIPAvamar" runat="server" /><asp:HiddenField ID="hdnIPAvamarExists" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td></td>
                                                <td colspan="2"><asp:CheckBox ID="chkBluecat" runat="server" CssClass="default" Text="Update BlueCat" /></td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                    <asp:Panel ID="panIPsNo" runat="server" Visible="false">
                                        <table cellpadding="5" cellspacing="2" border="0">
                                            <tr>
                                                <td>DHCP Address:</td>
                                                <td><asp:Label ID="lblIPDHCP" runat="server" CssClass="default" /></td>
                                            </tr>
                                            <tr>
                                                <td>Assigned Address(es):</td>
                                                <td><asp:Label ID="lblIPAssigned" runat="server" CssClass="default" /></td>
                                            </tr>
                                            <tr>
                                                <td>Final Address(es):</td>
                                                <td><asp:Label ID="lblIPFinal" runat="server" CssClass="default" /></td>
                                            </tr>
                                            <tr id="trVMotionView" runat="server" visible="false">
                                                <td>VMotion Address(es):</td>
                                                <td><asp:Label ID="lblIPVMotion" runat="server" CssClass="default" /></td>
                                            </tr>
                                            <tr id="trPrivateView" runat="server" visible="false">
                                                <td>Private Address(es):</td>
                                                <td><asp:Label ID="lblIPPrivate" runat="server" CssClass="default" /></td>
                                            </tr>
                                            <tr>
                                                <td>Avamar Address(es):</td>
                                                <td><asp:Label ID="lblIPAvamar" runat="server" CssClass="default" /></td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div style="display:none">
                        <table width="100%" cellpadding="5" cellspacing="2" border="0">
                            <tr>
                                <td class="header" colspan="2"><asp:Label ID="fldComponents" runat="server" CssClass="header" Text="Software Components" /></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Panel ID="panComponents" runat="server" Visible="false">
                                        <iframe id="frmComponents" runat="server" frameborder="1" scrolling="no" width="730" height="450" />
                                    </asp:Panel>
                                    <%=strComponents %>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div style="display:none">
                        <table width="100%" cellpadding="5" cellspacing="2" border="0">
                            <tr>
                                <td class="header" colspan="2">Resource Dependencies</td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <table width="100%" cellpadding="4" cellspacing="0" border="0" style="border:solid 1px #CCCCCC">
                                        <asp:repeater ID="rptServiceRequests" runat="server">
                                            <HeaderTemplate>
                                                <tr bgcolor="#EEEEEE">
                                                    <td></td>
                                                    <td nowrap><b>Service Name</b></td>
                                                    <td nowrap><b>Progress</b></td>
                                                    <td nowrap><b>Submitted</b></td>
                                                    <td nowrap><b>Last Updated</b></td>
                                                </tr>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <tr>
                                                    <asp:Label ID="lblServiceID" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container.DataItem, "serviceid").ToString() %>' />
                                                    <td title='ResourceID: <%# DataBinder.Eval(Container.DataItem, "resourceid") %>'><img src="/images/spacer.gif" border="0" width="15" height="1" /></td>
                                                    <td width="40%" nowrap title='Click here to view the details of this service'><asp:Label ID="lblDetails" runat="server" CssClass="default" Text='<%#DataBinder.Eval(Container.DataItem, "name").ToString() %>' /></td>
                                                    <td width="20%" nowrap><asp:Label ID="lblProgress" runat="server" CssClass="default" Text='<%#DataBinder.Eval(Container.DataItem, "resourceid").ToString() %>' /></td>
                                                    <td width="20%" nowrap><%# DataBinder.Eval(Container.DataItem, "submitted").ToString()%></td>
                                                    <td width="20%" nowrap><%# DataBinder.Eval(Container.DataItem, "modified").ToString()%></td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:repeater>
                                        <tr id="trServiceRequests" runat="server" visible="false">
                                            <td colspan="5"><img src="/images/spacer.gif" border="0" height="1" width="25" /><img src="/images/alert.gif" border="0" align="absmiddle"> There are no resource dependencies</td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div style="display:none">
                        <table width="100%" cellpadding="5" cellspacing="2" border="0">
                            <tr>
                                <td class="header" colspan="2">Application Contacts & Information</td>
                            </tr>
                            <tr>
                                <td nowrap><asp:Label ID="fldApplicationName" runat="server" CssClass="default" Text="Application Name:" /></td>
                                <td width="100%"><asp:Label ID="lblApplicationName" runat="server" CssClass="default" /><asp:TextBox ID="txtApplicationName" runat="server" CssClass="default" MaxLength="100" Width="300" /></td>
                            </tr>
                            <asp:Panel ID="panApplicationCode" runat="server" Visible="false">
                            <tr>
                                <td nowrap><asp:Label ID="fldApplicationCode" runat="server" CssClass="default" Text="Application Code:" /></td>
                                <td width="100%"><asp:Label ID="lblApplicationCode" runat="server" CssClass="default" /><asp:TextBox ID="txtApplicationCode" runat="server" CssClass="default" MaxLength="3" Width="100" /></td>
                            </tr>
                            </asp:Panel>
                            <asp:Panel ID="panApplicationMnemonic" runat="server" Visible="false">
                            <tr>
                                <td nowrap><asp:Label ID="fldApplicationMnemonic" runat="server" CssClass="default" Text="Mnemonic:" /></td>
                                <td width="100%"><asp:Label ID="lblApplicationMnemonic" runat="server" CssClass="default" />
                                    <table cellpadding="0" cellspacing="0" border="0">
                                        <tr>
                                            <td><asp:TextBox ID="txtApplicationMnemonic" runat="server" Width="500" CssClass="default" /></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div id="divApplicationMnemonic" runat="server" style="overflow:hidden; position:absolute; display:none; background-color:#FFFFFF; border:solid 1px #CCCCCC">
                                                    <asp:ListBox ID="lstApplicationMnemonic" runat="server" CssClass="default" />
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            </asp:Panel>
                            <tr id="panApplicationCostCenter" runat="server" visible="false">
                                <td nowrap><asp:Label ID="fldApplicationCostCenter" runat="server" CssClass="default" Text="Cost Center:" /></td>
                                <td width="100%"><asp:Label ID="lblApplicationCostCenter" runat="server" CssClass="default" />
                                    <table cellpadding="0" cellspacing="0" border="0">
                                        <tr>
                                            <td><asp:TextBox ID="txtApplicationCostCenter" runat="server" Width="200" CssClass="default" /></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div id="divApplicationCostCenter" runat="server" style="overflow:hidden; position:absolute; display:none; background-color:#FFFFFF; border:solid 1px #CCCCCC">
                                                    <asp:ListBox ID="lstApplicationCostCenter" runat="server" CssClass="default" />
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <asp:Panel ID="panApplicationDR" runat="server" Visible="false">
                            <tr>
                                <td nowrap><asp:Label ID="fldApplicationDR" runat="server" CssClass="default" Text="DR Criticality:" /></td>
                                <td width="100%"><asp:Label ID="lblApplicationDR" runat="server" CssClass="default" /><asp:DropDownList ID="ddlApplicationDR" runat="server" CssClass="default" Width="400" /></td>
                            </tr>
                            </asp:Panel>
                            <tr>
                                <td nowrap><asp:Label ID="fldApplicationClient" runat="server" CssClass="default" Text="Departmental Manager:" /></td>
                                <td width="100%"><asp:Label ID="lblApplicationClient" runat="server" CssClass="default" />
                                    <table cellpadding="0" cellspacing="0" border="0">
                                        <tr>
                                            <td><asp:TextBox ID="txtApplicationClient" runat="server" Width="400" CssClass="default" /></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div id="divApplicationClient" runat="server" style="overflow:hidden; position:absolute; display:none; background-color:#FFFFFF; border:solid 1px #CCCCCC">
                                                    <asp:ListBox ID="lstApplicationClient" runat="server" CssClass="default" />
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td nowrap><asp:Label ID="fldApplicationPrimary" runat="server" CssClass="default" Text="Application Technical Lead:" /></td>
                                <td width="100%"><asp:Label ID="lblApplicationPrimary" runat="server" CssClass="default" />
                                    <table cellpadding="0" cellspacing="0" border="0">
                                        <tr>
                                            <td><asp:TextBox ID="txtApplicationPrimary" runat="server" Width="400" CssClass="default" /></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div id="divApplicationPrimary" runat="server" style="overflow:hidden; position:absolute; display:none; background-color:#FFFFFF; border:solid 1px #CCCCCC">
                                                    <asp:ListBox ID="lstApplicationPrimary" runat="server" CssClass="default" />
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td nowrap><asp:Label ID="fldApplicationAdministrative" runat="server" CssClass="default" Text="Administrative Contact:" /></td>
                                <td width="100%"><asp:Label ID="lblApplicationAdministrative" runat="server" CssClass="default" />
                                    <table cellpadding="0" cellspacing="0" border="0">
                                        <tr>
                                            <td><asp:TextBox ID="txtApplicationAdministrative" runat="server" Width="400" CssClass="default" /></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div id="divApplicationAdministrative" runat="server" style="overflow:hidden; position:absolute; display:none; background-color:#FFFFFF; border:solid 1px #CCCCCC">
                                                    <asp:ListBox ID="lstApplicationAdministrative" runat="server" CssClass="default" />
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr id="panApplicationOwner" runat="server" visible="false">
                                <td nowrap><asp:Label ID="fldApplicationOwner" runat="server" CssClass="default" Text="Application Owner:" /></td>
                                <td width="100%"><asp:Label ID="lblApplicationOwner" runat="server" CssClass="default" />
                                    <table cellpadding="0" cellspacing="0" border="0">
                                        <tr>
                                            <td><asp:TextBox ID="txtApplicationOwner" runat="server" Width="400" CssClass="default" /></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div id="divApplicationOwner" runat="server" style="overflow:hidden; position:absolute; display:none; background-color:#FFFFFF; border:solid 1px #CCCCCC">
                                                    <asp:ListBox ID="lstApplicationOwner" runat="server" CssClass="default" />
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr id="panApplicationEngineer" runat="server" visible="false">
                                <td nowrap><asp:Label ID="fldApplicationEngineer" runat="server" CssClass="default" Text="Network Engineer:" /></td>
                                <td width="100%"><asp:Label ID="lblApplicationEngineer" runat="server" CssClass="default" />
                                    <table cellpadding="0" cellspacing="0" border="0">
                                        <tr>
                                            <td><asp:TextBox ID="txtApplicationEngineer" runat="server" Width="400" CssClass="default" /></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div id="divApplicationEngineer" runat="server" style="overflow:hidden; position:absolute; display:none; background-color:#FFFFFF; border:solid 1px #CCCCCC">
                                                    <asp:ListBox ID="lstApplicationEngineer" runat="server" CssClass="default" />
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div style="display:none">
                        <table width="100%" cellpadding="5" cellspacing="2" border="0">
                            <tr>
                                <td class="header" colspan="2">Design Information</td>
                            </tr>
                            <tr>
                                <td nowrap><asp:Label ID="fldDesignID" runat="server" CssClass="default" Text="Design ID #:" /></td>
                                <td width="100%"><asp:Label ID="lblDesignID" runat="server" CssClass="default" /><asp:TextBox ID="txtDesignID" runat="server" CssClass="default" MaxLength="10" Width="100" /> <asp:Button ID="btnDesignID" runat="server" CssClass="default" Width="75" Text="Lookup" Visible="false" /></td>
                            </tr>
                            <tr>
                                <td nowrap><asp:Label ID="fldDesignChange" runat="server" CssClass="default" Text="Change Control #:" /></td>
                                <td width="100%"><asp:Label ID="lblDesignChange" runat="server" CssClass="default" /><asp:TextBox ID="txtDesignChange" runat="server" CssClass="default" MaxLength="10" Width="100" /></td>
                            </tr>
                            <tr>
                                <td nowrap><asp:Label ID="fldDesignCommitment" runat="server" CssClass="default" Text="Commitment Date:" /></td>
                                <td width="100%"><asp:Label ID="lblDesignCommitment" runat="server" CssClass="default" /><asp:TextBox ID="txtDesignCommitment" runat="server" CssClass="default" MaxLength="10" Width="100" /> <asp:ImageButton ID="imgDesignCommitment" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calendar.gif" Visible="false" /></td>
                            </tr>
                            <tr>
                                <td nowrap><asp:Label ID="fldDesignCompletion" runat="server" CssClass="default" Text="Completion / Installation Date:" /></td>
                                <td width="100%"><asp:Label ID="lblDesignCompletion" runat="server" CssClass="default" /><asp:TextBox ID="txtDesignCompletion" runat="server" CssClass="default" MaxLength="10" Width="100" /> <asp:ImageButton ID="imgDesignCompletion" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calendar.gif" Visible="false" /></td>
                            </tr>
                            <tr>
                                <td nowrap><asp:Label ID="fldDesignProduction" runat="server" CssClass="default" Text="Production Go Live Date:" /></td>
                                <td width="100%"><asp:Label ID="lblDesignProduction" runat="server" CssClass="default" /><asp:TextBox ID="txtDesignProduction" runat="server" CssClass="default" MaxLength="10" Width="100" /> <asp:ImageButton ID="imgDesignProduction" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calendar.gif" Visible="false" /></td>
                            </tr>
                            <tr>
                                <td nowrap><asp:Label ID="fldDesignFinished" runat="server" CssClass="default" Text="Finished / Commissioned:" /></td>
                                <td width="100%"><asp:Label ID="lblDesignFinished" runat="server" CssClass="default" /><asp:TextBox ID="txtDesignFinished" runat="server" CssClass="default" MaxLength="10" Width="100" ToolTip="This will get overwritten with the latest completion date of the last manual task (if all are closed)" /> <asp:ImageButton ID="imgDesignFinished" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calendar.gif" Visible="false" /></td>
                            </tr>
                            <tr>
                                <td colspan="2">&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="header" colspan="2">Design Questions</td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <table cellpadding="5" cellspacing="4" border="0">
                                        <%=strResponses %>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div style="display:none">
                        <table width="100%" height="100%" cellpadding="5" cellspacing="2" border="0">
                            <tr height="1">
                                <td style="border-bottom:dashed 1px #CCCCCC"><asp:LinkButton ID="btnProvisioning" runat="server" Text="Refresh Window" OnClick="btnProvisioning_Click" OnClientClick="LoadWait();" /></td>
                                <td style="border-bottom:dashed 1px #CCCCCC" align="right"><asp:Button ID="btnExecution" runat="server" CssClass="default" Width="100" Text="View Execution" Enabled="false" /></td>
                            </tr>
                            <tr>
                                <td colspan="2" valign="top">
                                    <%=strServerExecution %>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div style="display:none">
                        <table width="100%" height="100%" cellpadding="5" cellspacing="2" border="0">
                            <tr height="1">
                                <td style="border-bottom:dashed 1px #CCCCCC"><asp:LinkButton ID="btnAudit" runat="server" Text="Refresh Window" OnClick="btnAudit_Click" /></td>
                            </tr>
                            <tr>
                                <td valign="top">
                                    <%=strServerAudit %>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div style="display:none">
                        <table width="100%" cellpadding="5" cellspacing="2" border="0">
                            <tr>
                                <td class="header" colspan="2">Storage Information</td>
                            </tr>
                            <tr>
                                <td class="bold" colspan="2">Requested Storage</td>
                            </tr>
                            <tr>
                                <td nowrap colspan="2"><%=strServerStorage %></td>
                            </tr>
                            <tr>
                                <td nowrap colspan="2">&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="bold" colspan="2">Internal Storage</td>
                            </tr>
                            <tr>
                                <td colspan="2"><asp:Button ID="btnStorage" runat="server" CssClass="default" Width="150" Text="Refresh Data" OnClick="btnStorage_Click" /></td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <table width="100%" cellpadding="4" cellspacing="0" border="0" style="border:solid 1px #CCCCCC">
                                        <asp:repeater ID="rptLocal" runat="server">
                                            <HeaderTemplate>
                                                <tr bgcolor="#EEEEEE">
                                                    <td></td>
                                                    <td nowrap><b>Drive Letter</b></td>
                                                    <td nowrap><b>Drive</b></td>
                                                    <td nowrap><b>Size</b></td>
                                                    <td nowrap><b>Used</b></td>
                                                    <td nowrap><b>Available</b></td>
                                                </tr>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <tr>
                                                    <td><img src="/images/spacer.gif" border="0" width="15" height="1" /></td>
                                                    <td width="20%"><%# DataBinder.Eval(Container.DataItem, "letter")%></td>
                                                    <td width="20%"><%# DataBinder.Eval(Container.DataItem, "drive")%></td>
                                                    <td width="20%"><%# DataBinder.Eval(Container.DataItem, "size")%></td>
                                                    <td width="20%"><%# DataBinder.Eval(Container.DataItem, "used")%></td>
                                                    <td width="20%"><%# DataBinder.Eval(Container.DataItem, "available")%></td>
                                                </tr>
                                            </ItemTemplate>
                                            <AlternatingItemTemplate>
                                                <tr bgcolor="#F6F6F6">
                                                    <td><img src="/images/spacer.gif" border="0" width="15" height="1" /></td>
                                                    <td width="20%"><%# DataBinder.Eval(Container.DataItem, "letter")%></td>
                                                    <td width="20%"><%# DataBinder.Eval(Container.DataItem, "drive")%></td>
                                                    <td width="20%"><%# DataBinder.Eval(Container.DataItem, "size")%></td>
                                                    <td width="20%"><%# DataBinder.Eval(Container.DataItem, "used")%></td>
                                                    <td width="20%"><%# DataBinder.Eval(Container.DataItem, "available")%></td>
                                                </tr>
                                            </AlternatingItemTemplate>
                                        </asp:repeater>
                                        <tr>
                                            <td colspan="6"><asp:Label ID="lblLocal" runat="server" CssClass="default" Visible="false" Text="<img src='/images/spacer.gif' border='0' height='1' width='25' /><img src='/images/alert.gif' border='0' align='absmiddle'> There are no disks" /></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td nowrap colspan="2">&nbsp;</td>
                            </tr>
                            <!--
                            <tr>
                                <td class="bold" colspan="2">Requested Storage</td>
                            </tr>
                            <tr>
                                <td nowrap>Processor:</td>
                                <td width="100%"><asp:Label ID="lblProcessor" runat="server" CssClass="default" /></td>
                            </tr>
                            <tr>
                                <td nowrap>Size:</td>
                                <td width="100%"><asp:Label ID="lblSize" runat="server" CssClass="default" /></td>
                            </tr>
                            -->
                        </table>
                    </div>
                    <div style="display:none">
                        <table width="100%" cellpadding="5" cellspacing="2" border="0">
                            <tr>
                                <td class="header" colspan="2">Backup Information</td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:Panel ID="panBackupTSM" runat="server" Visible="false">
                                        <table width="100%" cellpadding="3" cellspacing="2" border="0" style="border:solid 1px #999999" bgcolor="#f9f9f9">
                                            <tr>
                                                <td class="bigred">Message Sent to Client</td>
                                            </tr>
                                            <tr>
                                                <td><asp:Label ID="lblBackupPreview" runat="server" CssClass="default" /></td>
                                            </tr>
                                        </table>
                                        <br />
                                    </asp:Panel>
                                    <asp:Panel ID="panBackupAvamar" runat="server" Visible="false">
                                        <table width="100%" cellpadding="3" cellspacing="2" border="0" style="border:solid 1px #999999" bgcolor="#f9f9f9">
                                            <tr>
                                                <td class="bigred">Message Sent to Client</td>
                                            </tr>
                                            <tr>
                                                <td><asp:Label ID="Label1" runat="server" CssClass="default" /></td>
                                            </tr>
                                        </table>
                                        <br />
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <%=strMenuTabBackup1 %>
                                    <div id="divMenuBackup1">
                                        <div style="display:none">
                                            <br />
                                            <table width="100%" cellpadding="4" cellspacing="3" border="0">
                                                <%=strBackupInformation %>
                                            </table>
                                        </div>
                                        <div style="display:none">
                                            <br />
                                            <table width="500" cellpadding="4" cellspacing="0" border="0" style="border:solid 1px #CCCCCC">
                                                <tr bgcolor="#EEEEEE">
                                                   <td nowrap><b>File/Folder</b></td>
                                                   <td nowrap></td>
                                                </tr>
                                                <asp:repeater ID="rptInclusions" runat="server">
                                                    <ItemTemplate>
                                                       <tr>
                                                          <td><%# DataBinder.Eval(Container.DataItem, "path") %></td>                                                               
                                                       </tr>
                                                    </ItemTemplate>
                                                </asp:repeater>
                                                <tr>
                                                   <td colspan="2">
                                                      <asp:Label ID="lblNoneInclusions" runat="server" CssClass="default" Visible="false" Text="<img src='/images/alert.gif' border='0' align='absmiddle'> There are no inclusions..." />
                                                   </td>
                                                </tr>                       
                                            </table>
                                        </div>
                                        <div style="display:none">
                                            <br />
                                            <table width="500" cellpadding="4" cellspacing="0" border="0" style="border:solid 1px #CCCCCC">
                                                <tr bgcolor="#EEEEEE">
                                                   <td nowrap><b>File/Folder</b></td>
                                                   <td nowrap></td>
                                                </tr>
                                                <asp:repeater ID="rptExclusions" runat="server">
                                                    <ItemTemplate>
                                                       <tr>
                                                          <td><%# DataBinder.Eval(Container.DataItem, "path") %></td>                                                               
                                                       </tr>
                                                    </ItemTemplate>
                                                </asp:repeater>
                                                <tr>
                                                   <td colspan="2">
                                                      <asp:Label ID="lblNoneExclusions" runat="server" CssClass="default" Visible="false" Text="<img src='/images/alert.gif' border='0' align='absmiddle'> There are no exclusions..." />
                                                   </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <div style="display:none">
                                            <br />
                                            <table width="500" cellpadding="4" cellspacing="0" border="0" style="border:solid 1px #CCCCCC">
                                                <tr bgcolor="#EEEEEE">
                                                   <td nowrap colspan="3"><b>File/Folder</b></td>
                                                   <td nowrap></td>
                                                </tr>
                                                <asp:repeater ID="rptRetention" runat="server">
                                                    <ItemTemplate>
                                                       <tr>
                                                          <td colspan="3"><%# DataBinder.Eval(Container.DataItem, "path") %></td>                                                               
                                                       </tr>
                                                       <tr>
                                                          <td>&nbsp;&nbsp;&nbsp;</td>
                                                          <td nowrap>First Archival:</td>
                                                          <td width="100%"><%# DateTime.Parse(DataBinder.Eval(Container.DataItem, "first").ToString()).ToShortDateString() %></td>
                                                          <td></td>
                                                       </tr>
                                                       <tr>
                                                          <td>&nbsp;&nbsp;&nbsp;</td>
                                                          <td nowrap>Archive Period:</td>
                                                          <td width="100%"><%# (DataBinder.Eval(Container.DataItem, "number").ToString() == "0" ? "" : DataBinder.Eval(Container.DataItem, "number") + " ") %><%# DataBinder.Eval(Container.DataItem, "type") %></td>
                                                          <td></td>
                                                       </tr>
                                                       <tr>
                                                          <td>&nbsp;&nbsp;&nbsp;</td>
                                                          <td nowrap>Start Time:</td>
                                                          <td width="100%"><%# DataBinder.Eval(Container.DataItem, "hour") %> <%# DataBinder.Eval(Container.DataItem, "switch") %></td>
                                                          <td></td>
                                                       </tr>
                                                       <tr>
                                                          <td>&nbsp;&nbsp;&nbsp;</td>
                                                          <td nowrap>Frequency:</td>
                                                          <td width="100%"><%# DataBinder.Eval(Container.DataItem, "occurence") %></td>
                                                          <td></td>
                                                       </tr>
                                                    </ItemTemplate>
                                                </asp:repeater>
                                                <tr>
                                                   <td colspan="6">
                                                      <asp:Label ID="lblNoneRetention" runat="server" CssClass="default" Visible="false" Text="<img src='/images/alert.gif' border='0' align='absmiddle'> There are no archive requirements..." />
                                                   </td>
                                                </tr>
                                            </table>                                                               
                                        </div>
                                        <div style="display:none">
                                            <br />
                                            <table width="400" cellpadding="5" cellspacing="2" border="0" class="default">
                                                <tr>
                                                   <td nowrap>Average Size of One Data File:</td>
                                                   <td width="100%"><asp:Label ID="lblAverage" runat="server" CssClass="default" /></td>
                                                </tr>
                                                <tr>
                                                   <td nowrap>Production Turnover Documentation Folder Name:</td>
                                                   <td width="100%"><asp:Label ID="lblDocumentation" runat="server" CssClass="default" /></td>
                                                </tr>
                                                <tr>
                                                   <td colspan="2">&nbsp;</td>
                                                </tr>
                                                <tr>
                                                   <td colspan="2" class="bold">Client File System Data</td>                                                      
                                                </tr>
                                                <tr>
                                                   <td nowrap>Percent Changed Daily:</td>
                                                   <td width="100%"><asp:Label ID="lblCFPercent" runat="server" CssClass="default" /></td>                                                       
                                                </tr>
                                                <tr>
                                                   <td nowrap>Compression Ratio:</td>
                                                   <td width="100%"><asp:Label ID="lblCFCompression" runat="server" CssClass="default" /></td>                                                        
                                                </tr>
                                                <tr>
                                                   <td nowrap>Average File Size:</td>
                                                   <td width="100%"><asp:Label ID="lblCFAverage" runat="server" CssClass="default" /></td>                                                        
                                                </tr>
                                                <tr>
                                                   <td nowrap>Backup Version Ratio:</td>
                                                   <td width="100%"><asp:Label ID="lblCFBackup" runat="server" CssClass="default" /></td>                                                        
                                                </tr>
                                                <tr>
                                                   <td nowrap>Archive Ratio:</td>
                                                   <td width="100%"><asp:Label ID="lblCFArchive" runat="server" CssClass="default" /></td>                                                        
                                                </tr>
                                                <tr>
                                                   <td nowrap>Backup Window (Hours):</td>
                                                   <td width="100%"><asp:Label ID="lblCFWindow" runat="server" CssClass="default" /></td>                                                        
                                                </tr>
                                                <tr>
                                                   <td nowrap>Backupsets:</td>
                                                   <td width="100%"><asp:Label ID="lblCFSets" runat="server" CssClass="default" /></td>                                                        
                                                </tr>
                                                <tr>
                                                   <td colspan="2">&nbsp;</td>
                                                </tr>
                                                <tr>
                                                   <td colspan="2" class="bold">Client Database Data</td>                                                        
                                                </tr>
                                                <tr>
                                                   <td nowrap>Database Type:</td>
                                                   <td width="100%"><asp:Label ID="lblCDType" runat="server" CssClass="default" /></td>                                                        
                                                </tr>
                                                <tr>
                                                   <td nowrap>Percent Changed Daily:</td>
                                                   <td width="100%"><asp:Label ID="lblCDPercent" runat="server" CssClass="default" /></td>                                                        
                                                </tr>
                                                <tr>
                                                   <td nowrap>Compression Ratio:</td>
                                                   <td width="100%"><asp:Label ID="lblCDCompression" runat="server" CssClass="default" /></td>                                                        
                                                </tr>
                                                <tr>
                                                   <td nowrap>Number of Backup Versions:</td>
                                                   <td width="100%"><asp:Label ID="lblCDVersions" runat="server" CssClass="default" /></td>                                                        
                                                </tr>
                                                <tr>
                                                   <td nowrap>Backup Window (Hours):</td>
                                                   <td width="100%"><asp:Label ID="lblCDWindow" runat="server" CssClass="default" /></td>                                                        
                                                </tr>
                                                <tr>
                                                   <td nowrap>Growth Factor:</td>
                                                   <td width="100%"><asp:Label ID="lblCDGrowth" runat="server" CssClass="default" /></td>                                                        
                                                </tr>
                                                <tr>
                                                   <td colspan="2">&nbsp;</td>
                                                </tr>
                                            </table>
                                        </div>
                                        <div style="display:none">
                                            <br />
                                            <table width="100%" cellpadding="5" cellspacing="2" border="0" class="default">
                                                <tr>
                                                   <td nowrap>Register Statement:</td>
                                                   <td width="100%"><asp:Label ID="lblTSMRegister" runat="server" CssClass="default" /></td>
                                                </tr>
                                                <tr>
                                                   <td nowrap>Define Statement:</td>
                                                   <td width="100%"><asp:Label ID="lblTSMDefine" runat="server" CssClass="default" /></td>
                                                </tr>
                                            </table>
                                            <br />
                                            <asp:Panel ID="panTSMOutput" runat="server" Visible="false">
                                                <table width="100%" cellpadding="3" cellspacing="2" border="0" style="border:solid 1px #999999" bgcolor="#f9f9f9">
                                                    <tr>
                                                        <td class="bigred">Registration Output</td>
                                                    </tr>
                                                    <tr>
                                                        <td><asp:Label ID="lblTSMOutput" runat="server" CssClass="default" /></td>
                                                    </tr>
                                                </table>
                                                <br />
                                            </asp:Panel>
                                        </div>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Panel ID="panCFI" runat="server" Visible="false">
                                        <table width="100%" cellpadding="5" cellspacing="2" border="0">
                                            <tr>
                                                <td colspan="2">
                                                    <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                                        <tr>
                                                            <td class="bigger"><b>Backup Configuration</b></td>
                                                            <td align="right"><b>B</b> = Backup Acceptable</td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td nowrap class="bold">Backup Frequency:</td>
                                                <td width="100%"><asp:Label ID="lblFrequency" runat="server" /></td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <table width="100%" cellpadding="4" cellspacing="0" border="0" style="border:solid 1px #CCCCCC">
                                                        <tr>
                                                            <td></td>
                                                            <td colspan="12" align="center" style="color: #FFF; background-color: #333"><b>AM</b></td>
                                                            <td colspan="12" align="center" style="color: #000; background-color: #AAA"><b>PM</b></td>
                                                        </tr>
                                                        <tr bgcolor="#EEEEEE">
                                                            <td></td>
                                                            <td>12</td>
                                                            <td>1</td>
                                                            <td>2</td>
                                                            <td>3</td>
                                                            <td>4</td>
                                                            <td>5</td>
                                                            <td>6</td>
                                                            <td>7</td>
                                                            <td>8</td>
                                                            <td>9</td>
                                                            <td>10</td>
                                                            <td>11</td>
                                                            <td>12</td>
                                                            <td>1</td>
                                                            <td>2</td>
                                                            <td>3</td>
                                                            <td>4</td>
                                                            <td>5</td>
                                                            <td>6</td>
                                                            <td>7</td>
                                                            <td>8</td>
                                                            <td>9</td>
                                                            <td>10</td>
                                                            <td>11</td>
                                                        </tr>
                                                        <%=strBackup.ToString()%>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                        <table width="100%" cellpadding="5" cellspacing="2" border="0">
                                            <tr>
                                                <td class="bigger" colspan="2"><b>Backup Exclusion Configuration</b></td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <table width="100%" cellpadding="4" cellspacing="0" border="0" style="border:solid 1px #CCCCCC">
                                                        <tr bgcolor="#EEEEEE">
                                                            <td><b><u>Path:</u></b></td>
                                                        </tr>
                                                        <asp:repeater ID="rptExclusionsCFI" runat="server">
                                                            <ItemTemplate>
                                                                <tr>
                                                                    <td valign="top" width="100%"><%# DataBinder.Eval(Container.DataItem, "path") %></td>
                                                                </tr>
                                                            </ItemTemplate>
                                                            <AlternatingItemTemplate>
                                                                <tr bgcolor="F6F6F6">
                                                                    <td valign="top" width="100%"><%# DataBinder.Eval(Container.DataItem, "path") %></td>
                                                                </tr>
                                                            </AlternatingItemTemplate>
                                                        </asp:repeater>
                                                        <tr>
                                                            <td colspan="2">
                                                                <asp:Label ID="lblExclusion" runat="server" CssClass="default" Visible="false" Text="<img src='/images/spacer.gif' border='0' height='1' width='25' /><img src='/images/alert.gif' border='0' align='absmiddle'> There are no backup exclusions" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div style="display:none">
                        <table width="100%" cellpadding="5" cellspacing="2" border="0">
                            <tr>
                                <td class="header" colspan="2">Security & Access Information</td>
                            </tr>
                            <tr>
                                <td colspan="2"><asp:Button ID="btnSecurity" runat="server" CssClass="default" Width="150" Text="Lookup Security" OnClick="btnSecurity_Click" /></td>
                            </tr>
                            <tr>
                                <td class="bigger" colspan="2"><b>Account Configuration</b></td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <table width="100%" cellpadding="4" cellspacing="0" border="0" style="border:solid 1px #CCCCCC">
                                        <tr bgcolor="#EEEEEE">
                                            <td><b><u>Username:</u></b></td>
                                            <td><b><u>Permissions:</u></b></td>
                                            <td></td>
                                        </tr>
                                        <asp:repeater ID="rptAccounts" runat="server">
                                            <ItemTemplate>
                                                <tr>
                                                    <asp:Label ID="lblLocal" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "localgroups") %>' />
                                                    <asp:Label ID="lblDomain" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "domaingroups") %>' />
                                                    <asp:Label ID="lblAdmin" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "admin") %>' />
                                                    <td valign="top"><%# oUser.GetFullName(DataBinder.Eval(Container.DataItem, "xid").ToString()) %> (<%# DataBinder.Eval(Container.DataItem, "xid") %>)</td>
                                                    <td valign="top"><asp:Label ID="lblPermissions" runat="server" CssClass="default" /></td>
                                                </tr>
                                            </ItemTemplate>
                                            <AlternatingItemTemplate>
                                                <tr bgcolor="F6F6F6">
                                                    <asp:Label ID="lblLocal" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "localgroups") %>' />
                                                    <asp:Label ID="lblDomain" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "domaingroups") %>' />
                                                    <asp:Label ID="lblAdmin" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "admin") %>' />
                                                    <td valign="top"><%# oUser.GetFullName(DataBinder.Eval(Container.DataItem, "xid").ToString()) %> (<%# DataBinder.Eval(Container.DataItem, "xid") %>)</td>
                                                    <td valign="top"><asp:Label ID="lblPermissions" runat="server" CssClass="default" /></td>
                                                </tr>
                                            </AlternatingItemTemplate>
                                        </asp:repeater>
                                        <tr>
                                            <td colspan="2">
                                                <asp:Label ID="lblAccounts" runat="server" CssClass="default" Visible="false" Text="<img src='/images/spacer.gif' border='0' height='1' width='25' /><img src='/images/alert.gif' border='0' align='absmiddle'> You have not added any accounts to this request" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="bigger" colspan="2"><b>Non-Domain Level Administrators</b> (Local Members of the &quot;Administrators&quot; Group)</td>
                            </tr>
                            <tr>
                                <td colspan="2"><%=strAdminsLocal %></td>
                            </tr>
                            <tr>
                                <td colspan="2">&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="bigger" colspan="2"><b>Domain Memberships</td>
                            </tr>
                            <tr>
                                <td colspan="2"><%=strAdminsDomain %></td>
                            </tr>
                        </table>
                    </div>
                    <div style="display:none">
                        <table width="100%" cellpadding="5" cellspacing="2" border="0">
                            <tr>
                                <td class="header" colspan="2">Project Information</td>
                            </tr>
                            <tr>
                                <td nowrap>Project Number:</td>
                                <td width="100%"><asp:Label ID="lblProjectNumber" runat="server" CssClass="default" /></td>
                            </tr>
                            <tr>
                                <td nowrap>Project Name:</td>
                                <td width="100%"><asp:Label ID="lblProjectName" runat="server" CssClass="default" /></td>
                            </tr>
                            <tr>
                                <td nowrap>Portfolio:</td>
                                <td width="100%"><asp:Label ID="lblProjectPortfolio" runat="server" CssClass="default" /></td>
                            </tr>
                            <tr>
                                <td nowrap>Project Lead:</td>
                                <td width="100%"><asp:Label ID="lblProjectLead" runat="server" CssClass="default" /></td>
                            </tr>
                            <tr>
                                <td nowrap>Integration Engineer:</td>
                                <td width="100%"><asp:Label ID="lblProjectEngineer" runat="server" CssClass="default" /></td>
                            </tr>
                            <tr>
                                <td colspan="2">&nbsp;</td>
                            </tr>
                            <tr>
                                <td nowrap>New Project:</td>
                                <td width="100%">
                                    <table cellpadding="0" cellspacing="0" border="0">
                                        <tr>
                                            <td><asp:TextBox ID="txtProjectChange" runat="server" Width="500" CssClass="default" /></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div id="divProjectChange" runat="server" style="overflow:hidden; position:absolute; display:none; background-color:#FFFFFF; border:solid 1px #CCCCCC">
                                                    <asp:ListBox ID="lstProjectChange" runat="server" CssClass="default" />
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td nowrap>&nbsp;</td>
                                <td width="100%"><asp:Button ID="btnProjectChange" runat="server" CssClass="default" Width="75" Text="Change" /></td>
                            </tr>
                        </table>
                    </div>
                    <div style="display:none">
                        <table width="100%" cellpadding="5" cellspacing="2" border="0">
                            <tr>
                                <td class="header" colspan="2">Documents</td>
                            </tr>
                            <tr>
                                <td nowrap colspan="2"><%=strDocuments %></td>
                            </tr>
                            <tr>
                                <td nowrap colspan="2">&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="header" colspan="2">Links</td>
                            </tr>
                            <tr>
                                <td nowrap colspan="2"><%=strLinks %></td>
                            </tr>
                            <tr>
                                <td nowrap colspan="2">&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="header" colspan="2">Installed Applications</td>
                            </tr>
                            <tr>
                                <td colspan="2"><asp:Button ID="btnApplications" runat="server" CssClass="default" Width="150" Text="Refresh List" OnClick="btnApplications_Click" /></td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <table width="100%" cellpadding="4" cellspacing="0" border="0" style="border:solid 1px #CCCCCC">
                                        <asp:repeater ID="rptApplications" runat="server">
                                            <HeaderTemplate>
                                                <tr bgcolor="#EEEEEE">
                                                    <td></td>
                                                    <td nowrap><b>Application Name</b></td>
                                                    <td nowrap><b>Publisher</b></td>
                                                    <td nowrap><b>Version</b></td>
                                                    <td nowrap><b>Installed On</b></td>
                                                </tr>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <tr>
                                                    <td><img src="/images/spacer.gif" border="0" width="15" height="1" /></td>
                                                    <td width="25%"><%# DataBinder.Eval(Container.DataItem, "name")%></td>
                                                    <td width="25%"><%# DataBinder.Eval(Container.DataItem, "publisher")%></td>
                                                    <td width="25%"><%# DataBinder.Eval(Container.DataItem, "version")%></td>
                                                    <td width="25%"><%# DataBinder.Eval(Container.DataItem, "installed")%></td>
                                                </tr>
                                            </ItemTemplate>
                                            <AlternatingItemTemplate>
                                                <tr bgcolor="#F6F6F6">
                                                    <td><img src="/images/spacer.gif" border="0" width="15" height="1" /></td>
                                                    <td width="25%"><%# DataBinder.Eval(Container.DataItem, "name")%></td>
                                                    <td width="25%"><%# DataBinder.Eval(Container.DataItem, "publisher")%></td>
                                                    <td width="25%"><%# DataBinder.Eval(Container.DataItem, "version")%></td>
                                                    <td width="25%"><%# DataBinder.Eval(Container.DataItem, "installed")%></td>
                                                </tr>
                                            </AlternatingItemTemplate>
                                        </asp:repeater>
                                        <tr>
                                            <td colspan="6"><asp:Label ID="lblApplications" runat="server" CssClass="default" Visible="false" Text="<img src='/images/spacer.gif' border='0' height='1' width='25' /><img src='/images/alert.gif' border='0' align='absmiddle'> There are no applications" /></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div style="display:none">
                        <table width="100%" cellpadding="5" cellspacing="2" border="0">
                            <tr>
                                <td class="header" colspan="2">Administration</td>
                            </tr>
                            <tr>
                                <td nowrap colspan="2">
                                    <asp:Panel ID="panAdministration" runat="server" Visible="false">
                                        <asp:Panel ID="panClusterWindows2012" runat="server" Visible="false">
                                            <table id="mappings" width="100%" cellpadding="5" cellspacing="2" border="0">
		                                        <tr> 
		                                            <td colspan="3"><b>Windows 2012 Cluster</b></td>
		                                        </tr>
		                                        <tr> 
		                                            <td colspan="3"><asp:Button ID="btnClusterQuery" runat="server" Text="Query LUNs" Width="100" OnClick="btnClusterQuery_Click" /></td>
		                                        </tr>
                                                <tr>
                                                    <td><b>Drive</b></td>
                                                    <td><b>Size</b></td>
                                                    <td><b>Mapping</b></td>
                                                </tr>
                                                <asp:Repeater runat="server" ID="rptClusterWindows2012">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblLunID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "id") %>' />
                                                        <asp:Label ID="lblUUID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "uuid") %>' />
                                                        <tr>
                                                            <td><%# DataBinder.Eval(Container.DataItem, "letter") %>:<%# DataBinder.Eval(Container.DataItem, "path") %></td>
                                                            <td><%# DataBinder.Eval(Container.DataItem, "size") %> GB</td>
                                                            <td><asp:DropDownList ID="ddlMapping" runat="server" /></td>
                                                        </tr>
                                                    </ItemTemplate>
                                                </asp:Repeater>
		                                        <tr> 
		                                            <td colspan="3"><asp:Button ID="btnClusterSave" runat="server" Text="Save Mappings" Width="100" OnClick="btnClusterSave_Click" Enabled="false" /></td>
		                                        </tr>
                                            </table>
                                        </asp:Panel>
                                        <table width="100%" cellpadding="5" cellspacing="2" border="0">
		                                    <tr> 
		                                        <td colspan="2"><b>Sun Virtual Environment Cluster</b></td>
		                                    </tr>
		                                    <tr>
		                                        <td colspan="2"><asp:DropDownList ID="ddlSVECluster" runat="server" /></td>
		                                    </tr>
		                                    <tr>
		                                        <td colspan="2"><asp:Button ID="btnAddSVE" runat="server" CssClass="default" Width="75" OnClick="btnAddSVE_Click" Text="Add" /></td>
		                                    </tr>
		                                    <tr>
		                                        <td colspan="2">
                                                    <table width="100%" cellpadding="4" cellspacing="0" border="0" style="border:solid 1px #CCCCCC">
                                                        <tr bgcolor="#EEEEEE">
                                                            <td><b><u>Cluster:</u></b></td>
                                                            <td></td>
                                                        </tr>
                                                        <asp:repeater ID="rptSVE" runat="server">
                                                            <ItemTemplate>
                                                                <tr>
                                                                    <td valign="top" width="100%"><%# DataBinder.Eval(Container.DataItem, "name") %></td>
                                                                    <td valign="top" align="right"><asp:LinkButton ID="btnDeleteSVE" runat="server" Text="Delete" OnClick="btnDeleteSVE_Click" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' /></td>
                                                                </tr>
                                                            </ItemTemplate>
                                                        </asp:repeater>
                                                        <tr>
                                                            <td colspan="2">
                                                                <asp:Label ID="lblSVE" runat="server" CssClass="default" Visible="false" Text="<img src='/images/spacer.gif' border='0' height='1' width='25' /><img src='/images/alert.gif' border='0' align='absmiddle'> This server is not a part of any SVE cluster" />
                                                            </td>
                                                        </tr>
                                                    </table>
		                                        </td>
		                                    </tr>
		                                    <tr>
		                                        <td colspan="2">&nbsp;</td>
		                                    </tr>
		                                    <tr> 
		                                        <td colspan="2">
		                                            <table width="100%" cellpadding="4" cellspacing="3" border="0">
		                                                <%=strAdministration %>
		                                            </table>
		                                        </td>
		                                    </tr>
		                                    <tr> 
		                                        <td colspan="2"><b>Remove Assets</b></td>
		                                    </tr>
		                                    <tr>
		                                        <td colspan="2">
		                                            <asp:CheckBoxList ID="chkAssets" runat="server" CssClass="default" RepeatDirection="Horizontal" >
		                                                <asp:ListItem Value="Test" Text="Test" />
		                                                <asp:ListItem Value="QA" Text="QA" />
		                                                <asp:ListItem Value="Prod" Text="Prod" />
		                                                <asp:ListItem Value="DR" Text="DR" />
		                                                <asp:ListItem Value="Latest" Text="Latest" />
		                                                <asp:ListItem Value="NotLatest" Text="Not Latest" />
		                                            </asp:CheckBoxList>
		                                        </td>
		                                    </tr>
		                                    <tr>
		                                        <td colspan="2"><asp:Button ID="btnAssets" runat="server" CssClass="default" Width="75" OnClick="btnAssets_Click" Text="Go" /></td>
		                                    </tr>
		                                    <tr>
		                                        <td colspan="2">&nbsp;</td>
		                                    </tr>
		                                    <tr> 
		                                        <td colspan="2"><b>Change Step</b></td>
		                                    </tr>
		                                    <tr>
		                                        <td nowrap>Step:</td>
		                                        <td width="100%">
		                                            <asp:TextBox ID="txtStep" runat="server" CssClass="default" Width="100" MaxLength="10" />
		                                            &nbsp;&nbsp;&nbsp;
		                                            <asp:CheckBox ID="chkRedo" runat="server" CssClass="default" Text="Redo Step (will only redo this one step)" />
		                                        </td>
		                                    </tr>
		                                    <tr>
		                                        <td nowrap>Step Skip Start:</td>
		                                        <td width="100%">
		                                            <asp:TextBox ID="txtStepSkipStart" runat="server" CssClass="default" Width="100" MaxLength="10" />
		                                            &nbsp;&nbsp;&nbsp;
		                                            <asp:CheckBox ID="chkRebuild" runat="server" CssClass="default" Text="Rebuild (will redo the step and all subsequent steps)" />
		                                        </td>
		                                    </tr>
		                                    <tr>
		                                        <td nowrap>Step Skip Go To:</td>
		                                        <td width="100%"><asp:TextBox ID="txtStepSkipGoto" runat="server" CssClass="default" Width="100" MaxLength="10" /></td>
		                                    </tr>
		                                    <tr>
		                                        <td colspan="2"><asp:CheckBox ID="chkStep" runat="server" CssClass="default" Text="Clear Errors" /></td>
		                                    </tr>
		                                    <tr>
		                                        <td colspan="2"><asp:CheckBox ID="chkStepBoot" runat="server" CssClass="default" Text="Clear Boot Errors (Step should be set to 7 and asset should be powered off)" /></td>
		                                    </tr>
		                                    <tr>
		                                        <td colspan="2"><asp:CheckBox ID="chkStepVMWare" runat="server" CssClass="default" Text="Delete VMware Information (Step should be set to 2)" /></td>
		                                    </tr>
		                                    <tr>
		                                        <td colspan="2"><asp:CheckBox ID="chkAudits" runat="server" CssClass="default" Text="Delete Audits (Provisioning)" /></td>
		                                    </tr>
		                                    <tr>
		                                        <td colspan="2"><asp:CheckBox ID="chkAuditsMIS" runat="server" CssClass="default" Text="Delete Audits (MIS)" /></td>
		                                    </tr>
		                                    <tr>
		                                        <td colspan="2"><asp:Button ID="btnStep" runat="server" CssClass="default" Width="75" OnClick="btnStep_Click" Text="Go" /></td>
		                                    </tr>
		                                    <tr>
		                                        <td colspan="2">&nbsp;</td>
		                                    </tr>
		                                    <tr> 
		                                        <td colspan="2"><b>Show Output</b></td>
		                                    </tr>
		                                    <tr>
		                                        <td colspan="2"><asp:CheckBox ID="chkDebug" runat="server" CssClass="default" Text="Include Debug Information" /></td>
		                                    </tr>
		                                    <tr>
		                                        <td colspan="2"><asp:Button ID="btnOutput" runat="server" CssClass="default" Width="75" OnClick="btnOutput_Click" Text="Go" /></td>
		                                    </tr>
		                                    <tr>
		                                        <td colspan="2">&nbsp;</td>
		                                    </tr>
                                        </table>
                                    </asp:Panel>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div style="display:none">
                        <table width="100%" cellpadding="5" cellspacing="2" border="0">
                            <tr>
                                <td class="header" colspan="2">Maintenance & Special Notes</td>
                            </tr>
                            <tr>
                                <td nowrap>Type:</td>
                                <td width="100%">Server</td>
                            </tr>
                        </table>
                    </div>
                    <div style="display:none">
                        <table width="100%" cellpadding="5" cellspacing="2" border="0">
                            <tr>
                                <td class="header" colspan="2">Capacity Performance</td>
                            </tr>
                            <tr>
                                <td nowrap>Type:</td>
                                <td width="100%">Server</td>
                            </tr>
                        </table>
                    </div>
                </div>
            </td>
        </tr>
    </table>
    </asp:Panel>
    <asp:Panel ID="panDenied" runat="server" Visible="false">
        <br />
            <table width="100%" cellpadding="0" cellspacing="5" border="0">
                <tr>
                    <td rowspan="2"><img src="/images/ico_error.gif" border="0" align="absmiddle" /></td>
                    <td class="header" width="100%" valign="bottom">Access Denied</td>
                </tr>
                <tr>
                    <td width="100%" valign="top">You do not have sufficient permission to view this page.</td>
                </tr>
                <tr>
                    <td colspan="2">&nbsp;</td>
                </tr>
                <tr>
                    <td></td>
                    <td width="100%">If you think you should have rights to view it, please contact your ClearView administrator.</td>
                </tr>
            </table>
        <p>&nbsp;</p>
    </asp:Panel>
<asp:HiddenField ID="hdnTab" runat="server" />
<asp:HiddenField ID="hdnTabBackup" runat="server" />
<asp:HiddenField ID="hdnEnvironment" runat="server" />
<asp:HiddenField ID="hdnApplicationClient" runat="server" />
<asp:HiddenField ID="hdnApplicationPrimary" runat="server" />
<asp:HiddenField ID="hdnApplicationAdministrative" runat="server" />
<asp:HiddenField ID="hdnApplicationOwner" runat="server" />
<asp:HiddenField ID="hdnApplicationEngineer" runat="server" />
<asp:HiddenField ID="hdnApplicationMnemonic" runat="server" />
<asp:HiddenField ID="hdnApplicationCostCenter" runat="server" />
<asp:HiddenField ID="hdnProjectChange" runat="server" />
<input type="hidden" id="hdnComponents" name="hdnComponents" />
</asp:Content>
