<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="rr_dns_delete.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.rr_dns_delete" %>


<script type="text/javascript">
    function EnsureDNS(oName, oIP) {
        oName = document.getElementById(oName);
        oIP = document.getElementById(oIP);
        if (trim(oName.value) == "" && trim(oIP.value) == "") {
            alert('Please enter a name or IP address');
            SetFocus(oName);
            return false;
        }
        return true;
    }
</script>
<table width="100%" border="0" cellSpacing="4" cellPadding="5" class="default">
    <asp:Panel ID="panSearch" runat="server" Visible="false">
    <tr>
        <td colspan="2">Please enter a device name or IP address and click <b>Next  >></b> to retrieve the latest information...</td>
    </tr>
    <tr>
        <td nowrap>Device Name:</td>
        <td width="100%"><asp:TextBox ID="txtSearchName" runat="server" CssClass="default" width="200" MaxLength="50" /> </td>
    </tr>
    <tr>
        <td nowrap>IP Address:</td>
        <td width="100%"><asp:TextBox ID="txtSearchIP" runat="server" CssClass="default" width="200" MaxLength="15" /></td>
    </tr>
    <tr id="trQIP" runat="server" visible="false">
        <td nowrap>Alias:</td>
        <td width="100%"><img src="/images/alert.gif" border="0" align="absmiddle" /> <b>NOTE:</b> To delete an alias, use the <i>Update</i> service request.</td>
    </tr>
    <tr>
        <td nowrap></td>
        <td width="100%"><asp:Button ID="btnReset" runat="server" CssClass="default" Text="Reset" Width="100" OnClick="btnReset_Click" /></td>
    </tr>
    </asp:Panel>
    <tr>
        <td colspan="2">
            <asp:Panel ID="panAccess" runat="server" Visible="false">
                <table cellpadding="0" cellspacing="5" border="0" style="border:solid 1px #999999" bgcolor="#f9f9f9">
                    <tr>
                        <td rowspan="2" valign="top"><img src="/images/bigAlert.gif" border="0" align="absmiddle" /></td>
                        <td class="header" valign="bottom">Insufficient Permission</td>
                    </tr>
                    <tr>
                        <td valign="top">Please contact one of the following resources to have this device modified...</td>
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
            <asp:Panel ID="panExist" runat="server" Visible="false">
                <table cellpadding="0" cellspacing="5" border="0" style="border:solid 1px #999999" bgcolor="#f9f9f9">
                    <tr>
                        <td rowspan="2" valign="top"><img src="/images/ico_error.gif" border="0" align="absmiddle" /></td>
                        <td class="header" valign="bottom">Device Not Found</td>
                    </tr>
                    <tr>
                        <td valign="top"><asp:Label ID="lblExist" runat="server" CssClass="default" /></td>
                    </tr>
                </table>
            </asp:Panel>
        </td>
    </tr>
    <asp:Panel ID="panShow" runat="server" Visible="false">
    <tr>
        <td colspan="2"><hr size="1" noshade /></td>
    </tr>
    <asp:Panel ID="panChange" runat="server" Visible="false">
    <tr>
        <td nowrap>Change Control #:<font class="required">&nbsp;*</font></td>
        <td width="100%"><asp:TextBox ID="txtChange" runat="server" CssClass="default" Width="100" MaxLength="10" /></td>
    </tr>
    </asp:Panel>
    <tr>
        <td nowrap valign="top">Reason for Change:<font class="required">&nbsp;*</font></td>
        <td width="100%"><asp:TextBox ID="txtReason" runat="server" CssClass="default" width="500" TextMode="MultiLine" Rows="8" /></td>
    </tr>
    </asp:Panel>
    <asp:Panel ID="panConfirm" runat="server" Visible="false">
    <tr>
        <td colspan="2">Please take a moment to review the information you are about to change...</td>
    </tr>
    <tr>
        <td colspan="2">
            <%=strConfirm %>
        </td>
    </tr>
    <tr>
        <td nowrap></td>
        <td width="100%"><asp:Button ID="btnDiscard" runat="server" CssClass="default" Text="Start Over" Width="100" OnClick="btnDiscard_Click" /></td>
    </tr>
    <tr>
        <td colspan="2"><hr size="1" noshade /></td>
    </tr>
    <tr>
        <td colspan="2">
            <table cellpadding="0" cellspacing="5" border="0" style="border:solid 1px #999999" bgcolor="#f9f9f9">
                <tr>
                    <td rowspan="3" valign="top"><img src="/images/bigAlert.gif" border="0" align="absmiddle" /></td>
                    <td class="header" valign="bottom">Disclaimer Notice</td>
                </tr>
                <tr>
                    <td valign="bottom">
                        <p>By checking this box you are accepting all the terms of this disclaimer notice. If you do not agree with anything in this notice you should cancel this service request immediately.</p>
                        <p>While every effort is made to ensure that this automated process functions without technical error, ClearView cannot prevent the requestor from entering the wrong device or wrong information. Because this is a fully automated process ClearView will change the DNS information immediately. Therefore, by checking this box you have acknowledged that appropriate communication has been sent to all parties associated with this device and that the change control (if applicable) has been fully approved. Failure to meet these terms and conditions can result in HR disciplinary action. 
                        <p><b>WARNNIG:</b> Once the DNS is changed, you might have to wait up to an hour for the change to propogate throughout the network.</p>
                    </td>
                </tr>
                <tr>
                    <td valign="top"><asp:CheckBox ID="chkAgree" runat="server" CssClass="default" Text="I have read and agree to the disclaimer notice" /></td>
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
                                <td>
                                    <asp:Button ID="btnContinue" runat="server" CssClass="default" Text="Next  >>" Width="100" OnClick="btnContinue_Click" />
                                    <asp:Button ID="btnConfirm" runat="server" CssClass="default" Text="Next  >>" Width="100" OnClick="btnConfirm_Click" />
                                    <asp:Button ID="btnNext" runat="server" OnClick="btnNext_Click" CssClass="default" Text="Next  >>" Width="100" />
                                </td>
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
<asp:Label ID="lblName" runat="server" Visible="false" />
<asp:Label ID="lblIP" runat="server" Visible="false" />
<asp:Label ID="lblAlias" runat="server" Visible="false" />
<asp:Label ID="lblDomain" runat="server" Visible="false" />
