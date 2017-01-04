<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="fore_last.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.fore_last" %>
<script type="text/javascript">
    function ForeRecoveryLast(oBool, oText, iQuantity) {
        if (oBool == true && ValidateNumber0(oText, 'Please enter a valid number for the number of recovery servers for each server') == false)
            return false;
        var oText = document.getElementById(oText);
        if (oBool == true && ValidateNumberGreaterNumbers(parseInt(oText.value), iQuantity, 'The number of recovery servers cannot exceed the number of servers.\n\nNumber of Servers: ' + iQuantity) == false)
            return false;
        return true;
    }
    function ForeHALast(oBool, oText, iQuantity) {
        if (oBool == true && ValidateNumber0(oText, 'Please enter a valid number for the number of high availability servers') == false)
            return false;
        var oText = document.getElementById(oText);
        if (oBool == true && ValidateNumberGreaterNumbers(parseInt(oText.value), iQuantity, 'The number of high availability servers cannot exceed the number of servers.\n\nNumber of Servers: ' + iQuantity) == false)
            return false;
        return true;
    }
    function EnsureConfidence(oDDL, boolComplete, strAlert) {
        oDDL = document.getElementById(oDDL);
        if (boolComplete == false && oDDL.selectedIndex == (oDDL.length - 1))
        {
            alert(strAlert);
            oDDL.focus();
            return false;
        }
        return true;
    }
    function EnsureConfidence2(oDDL, boolUnavailable, strAlert) {
        oDDL = document.getElementById(oDDL);
        if (boolUnavailable == true && (oDDL.selectedIndex == (oDDL.length - 1) || oDDL.selectedIndex == (oDDL.length - 2)))
        {
            alert(strAlert);
            oDDL.focus();
            return false;
        }
        return true;
    }
    function EnsureOverride(boolOverride, strRejected) {
        if (boolOverride == true && strRejected != "")
        {
            alert(strRejected);
            return false;
        }
        else
            return true;
    }
</script>
<table width="100%" cellpadding="4" cellspacing="2" border="0">
    <tr>
        <td colspan="2" align="center">
            <table width="100%" cellpadding="2" cellspacing="2" border="0" style="border:solid 1px #999999" bgcolor="#f9f9f9">
                <tr>
                    <td class="bigred" colspan="2">Start Build Date Explanation</td>
                </tr>
                <tr>
                    <td valign="top"><img src="/images/bigAlert.gif" border="0" align="absmiddle" /></td>
                    <td>
                        <b>NOTE:</b> During the integration, the SLA for builds for PNC is (2) weeks (10 business days). For example, if you want your device on <asp:Label ID="lblBurn" runat="server" CssClass="default" /> (10 business days from today), you should set your Start Build Date to today's date. Once your Start Build Date has occurred or passed, you should click [Execute] from Design Builder to start the provisioning process.
                        <br /><br />
                        As another example, if you want your device six (6) weeks from today, you should set your Start Build Date to be four (4) weeks from today, which will give the builder two (2) weeks to implement the build.
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td nowrap><asp:Label ID="lblStartDate" runat="server" CssClass="default" Text="Start Build Date:<font class='required'>&nbsp;*</font>" /></td>
        <td width="100%"><asp:TextBox ID="txtDate" runat="server" CssClass="default" Width="80" MaxLength="10" /> <asp:ImageButton ID="imgDate" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calendar.gif" /></td>
    </tr>
    <tr>
        <td nowrap><asp:Label ID="lblConfidenceLevel" runat="server" CssClass="default" Text="Confidence Level:<font class='required'>&nbsp;*</font>" /></td>
        <td width="100%"><asp:DropDownList ID="ddlConfidence" runat="server" CssClass="default" /></td>
    </tr>
</table>
<asp:Panel ID="panHA" runat="server" Visible="false">
    <br />
    <table width="100%" cellpadding="3" cellspacing="2" border="0" style="border:solid 1px #999999" bgcolor="#f9f9f9" >
        <tr>
            <td class="bigred" colspan="2"><asp:Label ID="lblHA" runat="server" CssClass="default" Text="High Availability" /></td>
        </tr>
        <tr>
            <td colspan="2">You selected &quot;High Availability&quot;. Please enter the number of servers you want for High Availability.</td>
        </tr>
        <tr>
            <td nowrap><asp:Label ID="lblHAServerNumber" runat="server" CssClass="default" Text="Number of high availability servers:<font class='required'>&nbsp;*</font>" /></td>
            <td width="100%"><asp:TextBox ID="txtHA" runat="server" CssClass="default" /></td>
        </tr>
    </table>
</asp:Panel>
<asp:Panel ID="panRecovery" runat="server" Visible="false">
    <br />
    <table width="100%" cellpadding="3" cellspacing="2" border="0" style="border:solid 1px #999999" bgcolor="#f9f9f9" >
        <tr>
            <td class="bigred" colspan="2"><asp:Label ID="lblManyToOneRecovery" runat="server" CssClass="default" Text="Many-to-One Recovery" /></td>
        </tr>
        <tr>
            <td colspan="2">You selected &quot;Many-to-One Recovery&quot;. Please enter the number of recovery servers you want for EACH server.</td>
        </tr>
        <tr>
            <td colspan="2"><img src="/images/arrow_down_right.gif" border="0" align="absmiddle" /> Example: Quantity = 10, Many-to-One Recovery Servers = 2 <img src="/images/hand_right.gif" border="0" align="absmiddle" /> <span class="reddefault">Total Number of Servers = 12</span></td>
        </tr>
        <tr>
            <td nowrap><asp:Label ID="lblRecoveryServerNumber" runat="server" CssClass="default" Text="Number of recovery servers:<font class='required'>&nbsp;*</font>" /></td>
            <td width="100%"><asp:TextBox ID="txtRecovery" runat="server" CssClass="default" /></td>
        </tr>
    </table>
</asp:Panel>
<asp:Panel ID="panOverride" runat="server" Visible="false">
    <br />
    <table width="100%" cellpadding="3" cellspacing="2" border="0" style="border:solid 1px #999999" bgcolor="#f9f9f9" >
        <tr>
            <td class="bigred"><asp:Label ID="lblOverrideStatus" runat="server" CssClass="default" Text="Override Status" /></td>
        </tr>
        <tr>
            <td>You selected to override the selection matrix. Any design over 60% confidence level requires approval from a design review board.</td>
        </tr>
        <tr>
            <td class="bold"><asp:label ID="lblStatus" runat="server" CssClass="bold" /></td>
        </tr>
        <tr>
            <td><asp:label ID="lblStatusSub" runat="server" CssClass="default" /></td>
        </tr>
        <asp:Panel ID="panComments" runat="server" Visible="false">
        <tr>
            <td class="default"><asp:Label ID="lblAdditionalComments" runat="server" CssClass="default" Text="Additional Comments:" /></td>
        </tr>
        <tr>
            <td><asp:label ID="lblComments" runat="server" CssClass="default" /></td>
        </tr>
        </asp:Panel>
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
<asp:Label ID="lblRecovery" runat="server" Visible="false" />