<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="rr_server_decommission_NEW.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.rr_server_decommission_NEW" %>


<script type="text/javascript">
    function ShowAgreeAlert(oCheck, strAlert) {
        if (oCheck.checked == true) {
            var oResponse = confirm('WARNING: By accepting this disclaimer, you agree that the following people have been notified...\n\n' + strAlert + '\nAre you sure you want to agree to this?\n          OK = I agree\n          CANCEL = I do not agree');
            if (oResponse == false)
                oCheck.checked = false;
        }
    }
</script>
<table width="100%" border="0" cellSpacing="4" cellPadding="5" class="default">
    <tr>
        <td nowrap>Server Name:<font class="required">&nbsp;*</font></td>
        <td width="100%"><asp:TextBox ID="txtName" runat="server" CssClass="default" width="150" MaxLength="20" /> <asp:Button ID="btnContinue" runat="server" CssClass="default" Text="Check Permission" Width="125" OnClick="btnContinue_Click" /></td>
    </tr>
    <tr>
        <td colspan="2">
            <asp:Panel ID="panValid" runat="server" Visible="false">
                <table cellpadding="0" cellspacing="5" border="0" style="border:solid 1px #999999" bgcolor="#f9f9f9">
                    <tr>
                        <td rowspan="2" valign="top"><img src="/images/ico_check.gif" border="0" align="absmiddle" /></td>
                        <td class="header" valign="bottom">Access Granted</td>
                    </tr>
                    <tr>
                        <td valign="top">Please complete the following information and click <b>Next</b> to continue...</td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="panBuilding" runat="server" Visible="false">
                <table cellpadding="0" cellspacing="5" border="0" style="border:solid 1px #999999" bgcolor="#f9f9f9">
                    <tr>
                        <td rowspan="2" valign="top"><img src="/images/hugeAlert.gif" border="0" align="absmiddle" /></td>
                        <td class="header" valign="bottom">Still Provisioning</td>
                    </tr>
                    <tr>
                        <td valign="top">The server you are attempting to decommission is still in the process of being provisioned.</td>
                    </tr>
                    <tr>
                        <td></td>
                        <td valign="top">
                            <p>For this server to be available for decommission, all provisioning activity must first be completed:</p>
                            <ol>
                                <li>Find the server in DataPoint</li>
                                <li>Click on the Provisioning Status tab</li>
                                <li>Check that all steps related to this server have been completed</li>
                                <li>Click on the Resource Dependencies tab</li>
                                <li>Make sure all tasks are completed</li>
                            </ol>
                            <p><b>NOTE:</b> Once all steps and tasks are completed, this server can be decommissioned</p>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="panInvalid" runat="server" Visible="false">
                <table cellpadding="0" cellspacing="5" border="0" style="border:solid 1px #999999" bgcolor="#f9f9f9">
                    <tr>
                        <td rowspan="2" valign="top"><img src="/images/ico_error.gif" border="0" align="absmiddle" /></td>
                        <td class="header" valign="bottom">Access Denied</td>
                    </tr>
                    <tr>
                        <td valign="top">Please contact one of the following resources to have this device decommissioned...</td>
                    </tr>
                    <tr>
                        <td></td>
                        <td valign="top">
                            <table cellpadding="3" cellspacing="2" border="0">
                                <%=strContacts %>
                            </table>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="panAlready" runat="server" Visible="false">
                <table cellpadding="0" cellspacing="5" border="0" style="border:solid 1px #999999" bgcolor="#f9f9f9">
                    <tr>
                        <td rowspan="2" valign="top"><img src="/images/hugeAlert.gif" border="0" align="absmiddle" /></td>
                        <td class="header" valign="bottom">Already Submitted</td>
                    </tr>
                    <tr>
                        <td valign="top">A decommission record has already been submitted for this device.</td>
                    </tr>
                    <tr>
                        <td></td>
                        <td valign="top">
                            <table cellpadding="3" cellspacing="2" border="0">
                                <tr>
                                    <td>Serial Number:</td>
                                    <td><asp:Label ID="lblAlreadySerial" runat="server" CssClass="default" /></td>
                                </tr>
                                <tr>
                                    <td>Serial Number (DR):</td>
                                    <td><asp:Label ID="lblAlreadySerialDR" runat="server" CssClass="default" /></td>
                                </tr>
                                <tr>
                                    <td>Submitted By:</td>
                                    <td><asp:Label ID="lblAlreadyBy" runat="server" CssClass="default" /></td>
                                </tr>
                                <tr>
                                    <td>Submitted On:</td>
                                    <td><asp:Label ID="lblAlreadyOn" runat="server" CssClass="default" /></td>
                                </tr>
                                <tr>
                                    <td>Reason:</td>
                                    <td><asp:Label ID="lblAlreadyReason" runat="server" CssClass="default" /></td>
                                </tr>
                                <tr>
                                    <td>Power Off Date:</td>
                                    <td><asp:Label ID="lblAlreadyPower" runat="server" CssClass="default" /></td>
                                </tr>
                                <tr>
                                    <td>Status:</td>
                                    <td><asp:Label ID="lblAlreadyStatus" runat="server" CssClass="default" /></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="panMore" runat="server" Visible="false">
                <table cellpadding="4" cellspacing="3" border="0" style="border:solid 1px #999999" bgcolor="#f9f9f9">
                    <tr>
                        <td rowspan="2" valign="top"><img src="/images/hugeAlert.gif" border="0" align="absmiddle" /></td>
                        <td class="header" valign="bottom">Multiple Records Found</td>
                    </tr>
                    <tr>
                        <td valign="top">More than one (1) record was found. Please enter a more specific name...</td>
                    </tr>
                </table>
            </asp:Panel>
        </td>
    </tr>
    <asp:Panel ID="panFound" runat="server" Visible="false">
    <tr>
        <td nowrap>Model:</td>
        <td width="100%"><asp:Label ID="lblModel" runat="server" CssClass="default" /></td>
    </tr>
    <tr>
        <td nowrap>Serial Number:</td>
        <td width="100%"><asp:Label ID="lblSerial" runat="server" CssClass="default" /></td>
    </tr>
    <tr>
        <td nowrap>Class:</td>
        <td width="100%"><asp:Label ID="lblClass" runat="server" CssClass="default" /></td>
    </tr>
    <tr>
        <td nowrap>Environment:</td>
        <td width="100%"><asp:Label ID="lblEnvironment" runat="server" CssClass="default" /></td>
    </tr>
    <tr>
        <td nowrap>Location:</td>
        <td width="100%"><asp:Label ID="lblAddress" runat="server" CssClass="default" /></td>
    </tr>
    <asp:Panel ID="panDR" runat="server" Visible="false">
    <tr>
        <td nowrap class="note">Serial Number (DR):</td>
        <td width="100%"><asp:Label ID="lblSerialDR" runat="server" CssClass="note" /></td>
    </tr>
    <tr>
        <td nowrap></td>
        <td width="100%"><img src="/images/alert.gif" border="0" align="absmiddle" /> <b>NOTE:</b> The Disaster Recovery device will also be decommissioned with this request.</td>
    </tr>
    </asp:Panel>
    </asp:Panel>
    <asp:Panel ID="panNotFound" runat="server" Visible="false">
    <tr>
        <td colspan="2">
            <table cellpadding="0" cellspacing="5" border="0" style="border:solid 1px #999999" bgcolor="#f9f9f9">
                <tr>
                    <td rowspan="4" valign="top"><img src="/images/hugeAlert.gif" border="0" align="absmiddle" /></td>
                    <td class="header" valign="bottom">Record Not Found</td>
                </tr>
                <tr>
                    <td valign="top">The device <asp:Label ID="lblName" runat="server" CssClass="bold" /> was not found. Please try again...</td>
                </tr>
            </table>
        </td>
    </tr>
    </asp:Panel>
    <asp:Panel ID="panNotFound_OLD" runat="server" Visible="false">
    <tr>
        <td colspan="2">
            <table cellpadding="0" cellspacing="5" border="0" style="border:solid 1px #999999" bgcolor="#f9f9f9">
                <tr>
                    <td rowspan="4" valign="top"><img src="/images/bigHelp.gif" border="0" align="absmiddle" /></td>
                    <td class="header" valign="bottom">Record Not Found</td>
                </tr>
                <tr>
                    <td valign="top">The device <asp:Label ID="lblName_OLD" runat="server" CssClass="bold" /> was not found. You can still continue with this decommission request, but we ask that you first confirm that you typed the name of the device correctly...</td>
                </tr>
                <tr>
                    <td>Did you type the name of the device correctly?</td>
                </tr>
                <tr>
                    <td valign="top">
                        <asp:RadioButton ID="radYes" runat="server" CssClass="default" Text="Yes" GroupName="Typed" AutoPostBack="true" OnCheckedChanged="radYes_Change" /> 
                        <asp:RadioButton ID="radNo" runat="server" CssClass="default" Text="No" GroupName="Typed" AutoPostBack="true" OnCheckedChanged="radNo_Change" /> 
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <asp:Panel ID="panConfirm" runat="server" Visible="false">
    <tr>
        <td nowrap>Current Class:<font class="required">&nbsp;*</font></td>
        <td width="100%">
            <table cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td><asp:DropDownList ID="ddlClass" runat="server" CssClass="default" Width="300" AutoPostBack="true" OnSelectedIndexChanged="ddlClass_Change" /></td>
                    <td class="bold">
                        <div id="divClass" runat="server" style="display:none">
                            <img src="/images/spacer.gif" border="0" width="20" height="1" /><img src="/images/please_wait.gif" border="0" align="absmiddle" /> Please Wait...
                        </div>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <asp:Panel ID="panClass" runat="server" Visible="false">
    <tr>
        <td nowrap>Current Environment:<font class="required">&nbsp;*</font></td>
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
    <asp:Panel ID="panEnvironment" runat="server" Visible="false">
    <tr>
        <td nowrap>Platform of the Device:</td>
        <td width="100%"><asp:DropDownList ID="ddlPlatform" runat="server" CssClass="default" Width="400" /></td>
    </tr>
    <tr>
        <td nowrap>Type of Platform:</td>
        <td width="100%"><asp:DropDownList ID="ddlPlatformType" runat="server" CssClass="default" Width="400" /></td>
    </tr>
    <tr>
        <td nowrap>Model of the Device:</td>
        <td width="100%"><asp:DropDownList ID="ddlPlatformModel" runat="server" CssClass="default" Width="400" /></td>
    </tr>
    <tr>
        <td nowrap>Model Category:</td>
        <td width="100%"><asp:DropDownList ID="ddlPlatformModelProperty" runat="server" CssClass="default" Width="400" /></td>
    </tr>
    <tr>
        <td nowrap>Serial Number:</td>
        <td width="100%"><asp:TextBox ID="txtSerial" runat="server" CssClass="default" Width="100" MaxLength="50" /></td>
    </tr>
    <tr>
        <td nowrap>Location of the Device:</td>
        <td width="100%"><%=strLocation %></td>
    </tr>
    <tr>
        <td nowrap>Serial Number (DR):</td>
        <td width="100%"><asp:TextBox ID="txtDR" runat="server" CssClass="default" Width="100" MaxLength="50" /></td>
    </tr>
    <tr>
        <td nowrap></td>
        <td width="100%"><img src="/images/alert.gif" border="0" align="absmiddle" /> <b>NOTE:</b> If applicable, the Disaster Recovery device will also be decommissioned with this request.</td>
    </tr>
    </asp:Panel>
    </asp:Panel>
    </asp:Panel>
    </asp:Panel>
    <asp:Panel ID="panDetail" runat="server" Visible="false">
    <tr>
        <td colspan="2">
            <table cellpadding="0" cellspacing="5" border="0" style="border:solid 1px #999999" bgcolor="#f9f9f9">
                <tr>
                    <td rowspan="3" valign="top"><img src="/images/bigAlert.gif" border="0" align="absmiddle" /></td>
                    <td class="header" valign="bottom">Disclaimer Notice (Please Read the Following)</td>
                </tr>
                <tr>
                    <td valign="bottom">
                        <p>By checking this box you are accepting all the terms of this disclaimer notice. If you do not agree with anything in this notice you should cancel this service request immediately.</p>
                        <p>While every effort is made to ensure that this automated decommission process functions without technical error, ClearView cannot prevent the requestor from entering the wrong device. Because this is a fully automated process ClearView <u>will power down the device on the expected date</u>. Therefore, by checking this box you have acknowledged that appropriate communication has been sent to all parties associated with this device and that change control (#) has been fully approved. Failure to meet these terms and conditions can result in <span class="reddefault">disciplinary action</span>.</p>
                        <p><b>WARNNIG:</b> Once the device is powered off, you will no longer have access to the device.</p>
                    </td>
                </tr>
                <tr>
                    <td valign="top"><asp:CheckBox ID="chkAgree" runat="server" CssClass="default" Text="I have read and agree to the disclaimer notice" /></td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td nowrap>Power Off Date:<font class="required">&nbsp;*</font></td>
        <td width="100%"><asp:TextBox ID="txtDate" runat="server" CssClass="default" Width="100" MaxLength="10" /> <asp:ImageButton ID="imgDate" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calendar.gif" /></td>
    </tr>
    <asp:Panel ID="panChange" runat="server" Visible="false">
    <tr>
        <td nowrap>Change Control #:<font class="required">&nbsp;*</font></td>
        <td width="100%"><asp:TextBox ID="txtChange" runat="server" CssClass="default" Width="100" MaxLength="10" /></td>
    </tr>
    <tr>
        <td nowrap>Reason for Change Control #:<font class="required">&nbsp;*</font></td>
        <td width="100%"><asp:Label ID="lblChange" runat="server" CssClass="default" /></td>
    </tr>
    <tr>
        <td></td>
        <td width="100%"><img src="/images/alert.gif" border="0" align="absmiddle" /> Since the decommission process is automated, it is imperative that you have a <u>fully approved change control</u> prior to submitting this server decommission request</td>
    </tr>
    </asp:Panel>
    <tr>
        <td nowrap valign="top">Reason for Decommission:<font class="required">&nbsp;*</font></td>
        <td width="100%"><asp:TextBox ID="txtReason" runat="server" CssClass="default" width="500" TextMode="MultiLine" Rows="8" /></td>
    </tr>
    <tr>
        <td colspan="2">Retrieve special hardware (extra PCI cards, USB dongles, modems, etc...)?<font class="required">&nbsp;*</font></td>
    </tr>
    <tr>
        <td colspan="2">
            <asp:RadioButton ID="radRetrieveYes" runat="server" CssClass="default" Text="Yes" GroupName="Retrieve" /> 
            <asp:RadioButton ID="radRetrieveNo" runat="server" CssClass="default" Text="No" GroupName="Retrieve" /> 
        </td>
    </tr>
    <tr id="trRetrieve1" runat="server" style="display:none">
        <td nowrap valign="top">Provide Description:<font class="required">&nbsp;*</font></td>
        <td width="100%"><asp:TextBox ID="txtRetrieve" runat="server" CssClass="default" width="500" TextMode="MultiLine" Rows="8" /></td>
    </tr>
    <tr id="trRetrieve2" runat="server" style="display:none">
        <td nowrap>Mail To (Address):<font class="required">&nbsp;*</font></td>
        <td width="100%"><asp:TextBox ID="txtRetrieveAddress" runat="server" CssClass="default" width="500" MaxLength="100" /></td>
    </tr>
    <tr id="trRetrieve3" runat="server" style="display:none">
        <td nowrap>Mail To (Locator):<font class="required">&nbsp;*</font></td>
        <td width="100%"><asp:TextBox ID="txtRetrieveLocator" runat="server" CssClass="default" width="500" MaxLength="100" /></td>
    </tr>
    </asp:Panel>
    <asp:Panel ID="panEdit" runat="server" Visible="false">
    <tr>
        <td colspan="2">
            <table cellpadding="3" cellspacing="2">
                <tr>
                    <td colspan="3">
                        <table cellpadding="0" cellspacing="5" border="0" style="border:solid 1px #999999" bgcolor="#f9f9f9">
                            <tr>
                                <td rowspan="3" valign="top"><img src="/images/bigAlert.gif" border="0" align="absmiddle" /></td>
                                <td class="header" valign="bottom">Read-Only Mode</td>
                            </tr>
                            <tr>
                                <td valign="top">Currently, only the power off date can be updated.  Should you need to change any other part of this request, either cancel the request and re-submit or contact a ClearView administrator.</td>
                            </tr>
                            <tr>
                                <td valign="top">Power Off Date: <asp:TextBox ID="txtPower" runat="server" CssClass="default" Width="100" MaxLength="10" /> <asp:ImageButton ID="imgPower" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calendar.gif" /></td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <%=strEdit %>
                <tr>
                    <td colspan="3">&nbsp;</td>
                </tr>
                <tr>
                    <td colspan="3"><b>NOTE:</b> To cancel this request, click the &quot;Cancel&quot; button and then click [<a href="javascript:void(0);">Cancel</a>] link from the service request summary page.</td>
                </tr>
            </table>
        </td>
    </tr>
    </asp:Panel>
    <tr>
        <td colspan="2"><hr size="1" noshade /></td>
    </tr>
    <tr>
        <td colspan="2">
            <table width="100%" cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td>
                        <table cellpadding="4" cellspacing="4" border="0">
                            <tr>
                                <td><asp:Button ID="btnBack" runat="server" OnClick="btnBack_Click" CssClass="default" Text="<<  Back" Width="100" /></td>
                                <td>
                                    <asp:Panel ID="panDeliverable" runat="server" Visible="false">
                                        <asp:Button ID="btnDeliverable" runat="server" CssClass="default" Text="Service Deliverables" Width="150" />
                                    </asp:Panel>
                                </td>
                                <td><asp:Button ID="btnNext" runat="server" OnClick="btnNext_Click" CssClass="default" Text="Next  >>" Width="100" /></td>
                            </tr>
                        </table>
                    </td>
                    <td align="right">
                        <table cellpadding="4" cellspacing="4" border="0">
                            <tr>
                                <td><asp:Button ID="btnCancel1" runat="server" OnClick="btnCancel_Click" CssClass="default" Text="Cancel Request" Width="125" /></td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
<asp:Label ID="lblItem" runat="server" Visible="false" />
<asp:Label ID="lblNumber" runat="server" Visible="false" />
<input type="hidden" id="hdnParent" runat="server" />
<asp:Label ID="lblId" runat="server" Visible="false" />
<input type="hidden" id="hdnLocation" runat="server" />
<input type="hidden" id="hdnModel" runat="server" />
