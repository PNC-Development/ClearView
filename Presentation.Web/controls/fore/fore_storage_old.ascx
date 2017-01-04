<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="fore_storage_old.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.fore_storage_old" %>
<script type="text/javascript">
    function EnsureStorage(oRad, o1, o2, o3) {
        oRad = document.getElementById(oRad);
        if (oRad.checked == true) {
            o1 = document.getElementById(o1);
            o2 = document.getElementById(o2);
            o3 = document.getElementById(o3);
            if (o1.checked == false && o2.checked == false && o3.checked == false) {
                alert('Please select a type of storage');
                o1.focus();
                return false;
            }
        }
        return true;
    }
    function EnsureStorageOld(oRad, oCheck, oTextReq, oQA, oTest, oProd, oNoRep, oDDLRep, oTextRep) {
        oRad = document.getElementById(oRad);
        oCheck = document.getElementById(oCheck);
        if (oRad.checked == true && oCheck.checked == true) {
            if (ValidateNumber0(oTextReq, 'Please enter a valid number for the total amount of storage') == false)
                return false;
            if (ValidateNumber(oQA, 'Please enter a valid number for the total amount of storage in QA') == false)
                return false;
            if (ValidateNumber(oTest, 'Please enter a valid number for the total amount of storage in TEST') == false)
                return false;
            if (oProd == true && oNoRep == false && oDDLRep != null) {
                oDDLRep = document.getElementById(oDDLRep);
                if (oDDLRep != null && oDDLRep.selectedIndex == (oDDLRep.length - 1)) {
                    if (ValidateNumber0(oTextRep, 'Please enter a valid number for the amount of storage to be replicated') == false)
                        return false;
                    if (ValidateNumberGreater(oTextRep, oTextReq, 'The amount of replicated storage cannot exceed the total amount of storage') == false)
                        return false;
                }
            }
        }
        return true;
    }
</script>
<table cellpadding="2" cellspacing="1" border="0">
    <tr>
        <td colspan="2">
            <asp:Label ID="lblAssociateSANStorageWithDesign" runat="server" CssClass="default" Text="Do you require SAN storage to be associated with this design?" />
        <font class="required">&nbsp;*</font></td>
    </tr>
    <tr>
        <td colspan="2">
            <asp:RadioButton ID="radYes" runat="server" CssClass="default" Text="Yes" GroupName="storage" /> 
            <asp:RadioButton ID="radNo" runat="server" CssClass="default" Text="No" GroupName="storage" />
            <asp:RadioButton ID="radLater" runat="server" CssClass="default" Text="Ask Me Later" GroupName="storage" />
        </td>
    </tr>
</table>
<div id="divYes" runat="server" style="display:none">
    <table width="100%" cellpadding="2" cellspacing="1" border="0">
        <tr>
            <td>
                <table width="100%" cellpadding="1" cellspacing="1" border="0" class="default">
                    <tr>
                        <td>
                        <asp:Label ID="lblChkIfRequireStorage" runat="server" CssClass="default" Text="Check if you require any of the following storage:<font class='required'>&nbsp;*</font>" /></td>
                        <td align="right"></td>
                    </tr>
                </table>
                <table cellpadding="1" cellspacing="1" border="0" class="default">
                    <tr>
                        <td><asp:CheckBox ID="chkOldHigh" runat="server" CssClass="default" Text="High Performance" GroupName="performance" /></td>
                    </tr>
                    <tr>
                        <td>
                            <div id="divOldHigh" runat="server" style="display:none">
                            <asp:Panel ID="panOldHighProduction" runat="server" Visible="false">
                            <table cellpadding="2" cellspacing="3" border="0">
                                <tr>
                                    <td align="right"><img src="/images/arrow_down_right.gif" border="0" align="absmiddle" /></td>
                                    <td>
                                    <asp:Label ID="lblHighPerfPRODStorageAmt" runat="server" CssClass="default" Text="<b>High Performance PROD Storage</b> - Please enter the total amount of storage you require in <b>PROD</b>:<font class='required'>&nbsp;*</font>" /></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/spacer.gif" border="0" width="20" height="1" /></td>
                                    <td><asp:TextBox ID="txtOldHighRequire" runat="server" CssClass="default" Width="75" MaxLength="5" /> GB</td>
                                </tr>
                            </table>
                            <br />
                            </asp:Panel>
                            <asp:Panel ID="panOldHighQA" runat="server" Visible="false">
                            <table cellpadding="2" cellspacing="3" border="0">
                                <tr>
                                    <td align="right"><img src="/images/arrow_down_right.gif" border="0" align="absmiddle" /></td>
                                    <td>
                                    <asp:Label ID="lblHighPerfQAStorageAmt" runat="server" CssClass="default" Text="<b>High Performance QA Storage</b> - Please enter the total amount of storage you require in <b>QA</b>:<font class='required'>&nbsp;*</font>" /></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/spacer.gif" border="0" width="20" height="1" /></td>
                                    <td><asp:TextBox ID="txtOldHighRequireQA" runat="server" CssClass="default" Width="75" MaxLength="5" /> GB</td>
                                </tr>
                            </table>
                            <br />
                            </asp:Panel>
                            <asp:Panel ID="panOldHighTest" runat="server" Visible="false">
                            <table cellpadding="2" cellspacing="3" border="0">
                                <tr>
                                    <td align="right"><img src="/images/arrow_down_right.gif" border="0" align="absmiddle" /></td>
                                    <td>
                                    <asp:Label ID="lblHighPerfTestStorageAmt" runat="server" CssClass="default" Text="<b>High Performance TEST Storage</b> - Please enter the total amount of storage you require in <b>TEST</b>:<font class='required'>&nbsp;*</font>" /></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/spacer.gif" border="0" width="20" height="1" /></td>
                                    <td><asp:TextBox ID="txtOldHighRequireTest" runat="server" CssClass="default" Width="75" MaxLength="5" /> GB</td>
                                </tr>
                            </table>
                            <br />
                            </asp:Panel>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td><asp:CheckBox ID="chkOldStandard" runat="server" CssClass="default" Text="Standard Performance" GroupName="performance" /></td>
                    </tr>
                    <tr>
                        <td>
                            <div id="divOldStandard" runat="server" style="display:none">
                            <asp:Panel ID="panOldStandardProduction" runat="server" Visible="false">
                            <table cellpadding="2" cellspacing="3" border="0">
                                <tr>
                                    <td align="right"><img src="/images/arrow_down_right.gif" border="0" align="absmiddle" /></td>
                                    <td>
                                    <asp:Label ID="lblStandardPerfPRODStorageAmt" runat="server" CssClass="default" Text="<b>Standard Performance PROD Storage</b> - Please enter the total amount of storage you require in <b>PROD</b>:<font class='required'>&nbsp;*</font>" /></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/spacer.gif" border="0" width="20" height="1" /></td>
                                    <td><asp:TextBox ID="txtOldStandardRequire" runat="server" CssClass="default" Width="75" MaxLength="5" /> GB</td>
                                </tr>
                            </table>
                            <br />
                            <asp:Panel ID="panOldStandardReplication" runat="server" Visible="false">
                            <table cellpadding="2" cellspacing="3" border="0">
                                <tr>
                                    <td align="right"><img src="/images/arrow_down_right.gif" border="0" align="absmiddle" /></td>
                                    <td colspan="2">
                                    <asp:Label ID="lblStandardPerfReplication" runat="server" CssClass="default" Text="<b>Standard Performance Replication</b> - Will this storage be replicated?<font class='required'>&nbsp;*</font>" /></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/spacer.gif" border="0" width="20" height="1" /></td>
                                    <td>
                                        <asp:DropDownList ID="ddlOldStandardReplicated" runat="server" CssClass="default" Width="100">
                                            <asp:ListItem Value="No" />
                                            <asp:ListItem Value="Yes" />
                                        </asp:DropDownList>
                                    </td>
                                    <td><div id="divOldStandardReplicated" runat="server" style="display:none">
                                        <img src="/images/green_right.gif" border="0" align="absmiddle" />
                                        <img src="/images/spacer.gif" border="0" width="10" height="1" />
                                            <asp:Label ID="lblStandardPerfRelicatedAmt" runat="server" CssClass="default" Text="Please enter the amount of storage to be replicated:" />
                                        <img src="/images/spacer.gif" border="0" width="5" height="1" />
                                        <asp:TextBox ID="txtOldStandardReplicated" runat="server" CssClass="default" Width="75" MaxLength="5" /> GB</div></td>
                                </tr>
                            </table>
                            <br />
                            </asp:Panel>
                            </asp:Panel>
                            <asp:Panel ID="panOldStandardQA" runat="server" Visible="false">
                            <table cellpadding="2" cellspacing="3" border="0">
                                <tr>
                                    <td align="right"><img src="/images/arrow_down_right.gif" border="0" align="absmiddle" /></td>
                                    <td>
                                        <asp:Label ID="lblStandardPerfQAStorageAmt" runat="server" CssClass="default" Text="<b>Standard Performance QA Storage</b> - Please enter the total amount of storage you require in <b>QA</b>:<font class='required'>&nbsp;*</font>" /></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/spacer.gif" border="0" width="20" height="1" /></td>
                                    <td><asp:TextBox ID="txtOldStandardRequireQA" runat="server" CssClass="default" Width="75" MaxLength="5" /> GB</td>
                                </tr>
                            </table>
                            <br />
                            </asp:Panel>
                            <asp:Panel ID="panOldStandardTest" runat="server" Visible="false">
                            <table cellpadding="2" cellspacing="3" border="0">
                                <tr>
                                    <td align="right"><img src="/images/arrow_down_right.gif" border="0" align="absmiddle" /></td>
                                    <td>
                                        <asp:Label ID="lblStandardPerfTESTStorageAmt" runat="server" CssClass="default" Text="<b>Standard Performance TEST Storage</b> - Please enter the total amount of storage you require in <b>TEST</b>:<font class='required'>&nbsp;*</font>" /></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/spacer.gif" border="0" width="20" height="1" /></td>
                                    <td><asp:TextBox ID="txtOldStandardRequireTest" runat="server" CssClass="default" Width="75" MaxLength="5" /> GB</td>
                                </tr>
                            </table>
                            <br />
                            </asp:Panel>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td><asp:CheckBox ID="chkOldLow" runat="server" CssClass="default" Text="Low Performance" GroupName="performance" /></td>
                    </tr>
                    <tr>
                        <td>
                            <div id="divOldLow" runat="server" style="display:none">
                            <asp:Panel ID="panOldLowProduction" runat="server" Visible="false">
                            <table cellpadding="2" cellspacing="3" border="0">
                                <tr>
                                    <td align="right"><img src="/images/arrow_down_right.gif" border="0" align="absmiddle" /></td>
                                    <td>
                                        <asp:Label ID="lblLowPerfPRODStorageAmt" runat="server" CssClass="default" Text="<b>Low Performance PROD Storage</b> - Please enter the total amount of storage you require in <b>PROD</b>:<font class='required'>&nbsp;*</font>" /></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/spacer.gif" border="0" width="20" height="1" /></td>
                                    <td><asp:TextBox ID="txtOldLowRequire" runat="server" CssClass="default" Width="75" MaxLength="5" /> GB</td>
                                </tr>
                            </table>
                            <br />
                            </asp:Panel>
                            <asp:Panel ID="panOldLowQA" runat="server" Visible="false">
                            <table cellpadding="2" cellspacing="3" border="0">
                                <tr>
                                    <td align="right"><img src="/images/arrow_down_right.gif" border="0" align="absmiddle" /></td>
                                    <td>
                                        <asp:Label ID="lblLowPerfQAStorageAmt" runat="server" CssClass="default" Text="<b>Low Performance QA Storage</b> - Please enter the total amount of storage you require in <b>QA</b>:<font class='required'>&nbsp;*</font>" /></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/spacer.gif" border="0" width="20" height="1" /></td>
                                    <td><asp:TextBox ID="txtOldLowRequireQA" runat="server" CssClass="default" Width="75" MaxLength="5" /> GB</td>
                                </tr>
                            </table>
                            <br />
                            </asp:Panel>
                            <asp:Panel ID="panOldLowTest" runat="server" Visible="false">
                            <table cellpadding="2" cellspacing="3" border="0">
                                <tr>
                                    <td align="right"><img src="/images/arrow_down_right.gif" border="0" align="absmiddle" /></td>
                                    <td>
                                        <asp:Label ID="lblLowPerfTESTStorageAmt" runat="server" CssClass="default" Text="<b>Low Performance TEST Storage</b> - Please enter the total amount of storage you require in <b>TEST</b>:<font class='required'>&nbsp;*</font>" /></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/spacer.gif" border="0" width="20" height="1" /></td>
                                    <td><asp:TextBox ID="txtOldLowRequireTest" runat="server" CssClass="default" Width="75" MaxLength="5" /> GB</td>
                                </tr>
                            </table>
                            <br />
                            </asp:Panel>
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</div>
<br />
<table width="100%" cellpadding="2" cellspacing="1" border="0" style="border:solid 1px #999999" bgcolor="#f9f9f9">
    <tr>
        <td class="bigred" colspan="2">Please read the following...</td>
    </tr>
    <tr>
        <td valign="top"><img src="/images/bigAlert.gif" border="0" align="absmiddle" /></td>
        <td>
            <table cellpadding="2" cellspacing="1" border="0" class="reddefault">
                <tr>
                    <td><b>If any storage is required, you need to specify the amount on this page.</b></td>
                </tr>
                <tr>
                    <td>The amounts should be accumulative for ALL the devices (10GB x 5 devices = 50GB total).</td>
                </tr>
                <tr>
                    <td>&nbsp;&nbsp;-&nbsp;Default VMware: 10GB per device</td>
                </tr>
                <tr>
                    <td>&nbsp;&nbsp;-&nbsp;Default Blade: 10GB per device</td>
                </tr>
                <tr>
                    <td>&nbsp;&nbsp;-&nbsp;Default Physical: 0GB per device</td>
                </tr>
            </table>
        </td>
    </tr>
</table>
<asp:Panel ID="panReset" runat="server" Visible="false">
    <table width="100%" cellpadding="4" cellspacing="2" border="0">
        <tr>
            <td align="right"><asp:LinkButton ID="btnReset" runat="server" Text="Reset Storage" /></td>
        </tr>
    </table>
</asp:Panel>
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
