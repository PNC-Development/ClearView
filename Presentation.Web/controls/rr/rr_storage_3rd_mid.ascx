<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="rr_storage_3rd_mid.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.rr_storage_3rd_mid" %>


<script type="text/javascript">
    function SwapDivDDL(oDDL, oDivYes1, oDivYes2, oDivNo1, oDivNo2) {
        if (oDivYes1 != null) {
            oDivYes1 = document.getElementById(oDivYes1);
            oDivYes1.style.display = "none";
            if (oDDL.selectedIndex == 1)
                oDivYes1.style.display = "inline";
        }
        if (oDivYes2 != null) {
            oDivYes2 = document.getElementById(oDivYes2);
            oDivYes2.style.display = "none";
            if (oDDL.selectedIndex == 1)
                oDivYes2.style.display = "inline";
        }
        if (oDivNo1 != null) {
            oDivNo1 = document.getElementById(oDivNo1);
            oDivNo1.style.display = "none";
            if (oDDL.selectedIndex == 2)
                oDivNo1.style.display = "inline";
        }
        if (oDivNo2 != null) {
            oDivNo2 = document.getElementById(oDivNo2);
            oDivNo2.style.display = "none";
            if (oDDL.selectedIndex == 2)
                oDivNo2.style.display = "inline";
        }
    }
    function ShowDivDDL(oDDL, oDiv, oIndex) {
        oDiv = document.getElementById(oDiv);
        oDiv.style.display = "none";
        if (oDDL.selectedIndex == oIndex)
            oDiv.style.display = "inline";
    }
    function ResetDiv(oDiv) {
        if (oDiv != null) {
            oDiv = document.getElementById(oDiv);
            oDiv.style.display = "none";
        }
        else {
            oDiv = document.getElementById('<%=divClusterYesGroupNew.ClientID %>');
            oDiv.style.display = "none";
            oDiv = document.getElementById('<%=divClusterYesGroupExisting.ClientID %>');
            oDiv.style.display = "none";
            oDiv = document.getElementById('<%=divClusterNo.ClientID %>');
            oDiv.style.display = "none";
            oDiv = document.getElementById('<%=divClusterYesGroup.ClientID %>');
            oDiv.style.display = "none";
        }
    }
    function EnsureStorage3rd(oCluster, oClusterYesSQLGroup, txtClusterYesGroupExisting, oClusterYesGroupExisting, txtClusterYesGroupExistingFileSystem, chkClusterYesGroupNewNetwork, txtClusterYesGroupNewNetwork, chkClusterYesGroupNewIP, txtClusterYesGroupNewIP, oClusterNo, txtClusterNo) {
        oCluster = document.getElementById(oCluster);
        if (oCluster.selectedIndex == 1) {
            // CLUSTER = YES
            if (ValidateDropDown(oClusterYesSQLGroup, 'Please select if the new SAN is going into a new cluster group or an existing cluster group') == false)
                return false;
            else {
                oClusterYesSQLGroup = document.getElementById(oClusterYesSQLGroup);
                if (oClusterYesSQLGroup.selectedIndex == 1) {
                    // NEW
                    chkClusterYesGroupNewNetwork = document.getElementById(chkClusterYesGroupNewNetwork);
                    if (chkClusterYesGroupNewNetwork.checked == true && ValidateText(txtClusterYesGroupNewNetwork, 'Please enter the network name') == false)
                        return false;
                    chkClusterYesGroupNewIP = document.getElementById(chkClusterYesGroupNewIP);
                    if (chkClusterYesGroupNewIP.checked == true && ValidateText(txtClusterYesGroupNewIP, 'Please enter the ip address') == false)
                        return false;
                }
                else {
                    // EXISTING
                    if (ValidateText(txtClusterYesGroupExisting, 'Please enter a cluster group name') == false)
                        return false;
                    if (ValidateDropDown(oClusterYesGroupExisting, 'Please select if you are requesting the increase in size of an existing filesystem or a brand new filesystem') == false || ValidateText(txtClusterYesGroupExistingFileSystem, 'Please enter a filesystem name') == false)
                        return false;
                }
            }
        }
        else {
            // CLUSTER = NO
            if (ValidateDropDown(oClusterNo, 'Please select if you are requesting the increase in size of an existing filesystem or a brand new filesystem') == false || ValidateText(txtClusterNo, 'Please enter a filesystem name') == false)
                return false;
        }
        return true;
    }
</script>
<table width="100%" border="0" cellSpacing="2" cellPadding="4" class="default">
    <tr>
        <td colspan="2">Please enter your server name:<font class="required">&nbsp;*</font></td>
    </tr>
    <tr>
        <td><img src="/images/spacer.gif" border="0" width="30" height="1" /></td>
        <td width="100%"><asp:TextBox ID="txtServerName" runat="server" CssClass="default" Width="200" MaxLength="20" /></td>
    </tr>
    <tr>
        <td colspan="2">Please enter the date when you need this request completed by:<font class="required">&nbsp;*</font></td>
    </tr>
    <tr>
        <td><img src="/images/spacer.gif" border="0" width="30" height="1" /></td>
        <td width="100%"><asp:TextBox ID="txtDate" runat="server" CssClass="default" Width="100" MaxLength="10" /> <asp:ImageButton ID="imgDate" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calendar.gif" /></td>
    </tr>
    <tr>
        <td></td>
        <td class="footer">NOTE: This date is an estimate. Our Service Level Agreement for storage requests is 10 business days.</td>
    </tr>
    <tr>
        <td colspan="2">Please select your operating system:<font class="required">&nbsp;*</font></td>
    </tr>
    <tr>
        <td><img src="/images/spacer.gif" border="0" width="30" height="1" /></td>
        <td width="100%">
            <asp:DropDownList ID="ddlOS" runat="server" CssClass="default">
                <asp:ListItem Value="-- SELECT --" />
                <asp:ListItem Value="AIX 5.2" />
                <asp:ListItem Value="AIX 5.3" />
                <asp:ListItem Value="Solaris 8" />
                <asp:ListItem Value="Solaris 9" />
                <asp:ListItem Value="Solaris 10" />
                <asp:ListItem Value="RHEL 4" />
                <asp:ListItem Value="VMware ESX" />
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td colspan="2">Please select your maintenance window:<font class="required">&nbsp;*</font></td>
    </tr>
    <tr>
        <td><img src="/images/spacer.gif" border="0" width="30" height="1" /></td>
        <td width="100%">
            <asp:DropDownList ID="ddlMaintenance" runat="server" CssClass="default">
                <asp:ListItem Value="-- SELECT --" />
                <asp:ListItem Value="7:00 PM - 7:00 AM (Everyday)" />
                <asp:ListItem Value="2:00 AM - 5:00 AM (Sundays)" />
                <asp:ListItem Value="Custom (Other)" />
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td colspan="2">Does the server currently have SAN?<font class="required">&nbsp;*</font></td>
    </tr>
    <tr>
        <td><img src="/images/spacer.gif" border="0" width="30" height="1" /></td>
        <td width="100%">
            <asp:DropDownList ID="ddlCurrent" runat="server" CssClass="default">
                <asp:ListItem Value="-- SELECT --" />
                <asp:ListItem Value="Yes - This server currently has SAN" />
                <asp:ListItem Value="No - This server DOES NOT have SAN" />
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td colspan="2">Please select your server type:<font class="required">&nbsp;*</font></td>
    </tr>
    <tr>
        <td><img src="/images/spacer.gif" border="0" width="30" height="1" /></td>
        <td width="100%">
            <asp:DropDownList ID="ddlType" runat="server" CssClass="default">
                <asp:ListItem Value="-- SELECT --" />
                <asp:ListItem Value="Blade" />
                <asp:ListItem Value="Rack Mount" />
                <asp:ListItem Value="Virtual" />
                <asp:ListItem Value="MicroLPAR" />
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td colspan="2">Where is your DR site?<font class="required">&nbsp;*</font></td>
    </tr>
    <tr>
        <td><img src="/images/spacer.gif" border="0" width="30" height="1" /></td>
        <td width="100%">
            <asp:DropDownList ID="ddlDR" runat="server" CssClass="default">
                <asp:ListItem Value="-- SELECT --" />
                <asp:ListItem Value="Highland" />
                <asp:ListItem Value="Dalton" />
                <asp:ListItem Value="SunGuard" />
                <asp:ListItem Value="None" />
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td colspan="2">Please select your SAN Performance:<font class="required">&nbsp;*</font></td>
    </tr>
    <tr>
        <td><img src="/images/spacer.gif" border="0" width="30" height="1" /></td>
        <td width="100%">
            <asp:DropDownList ID="ddlPerformance" runat="server" CssClass="default">
                <asp:ListItem Value="-- SELECT --" />
                <asp:ListItem Value="High Performance" />
                <asp:ListItem Value="Standard Performance" />
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td colspan="2">Have you scheduled the change yet?<font class="required">&nbsp;*</font></td>
    </tr>
    <tr>
        <td><img src="/images/spacer.gif" border="0" width="30" height="1" /></td>
        <td width="100%">
            <asp:DropDownList ID="ddlChange" runat="server" CssClass="default">
                <asp:ListItem Value="-- SELECT --" />
                <asp:ListItem Value="Yes - I have scheduled a change" />
                <asp:ListItem Value="No - I have not scheduled a change" />
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td colspan="2">Is this server part of a cluster?<font class="required">&nbsp;*</font></td>
    </tr>
    <tr>
        <td><img src="/images/spacer.gif" border="0" width="30" height="1" /></td>
        <td width="100%">
            <asp:DropDownList ID="ddlCluster" runat="server" CssClass="default">
                <asp:ListItem Value="-- SELECT --" />
                <asp:ListItem Value="Yes - This server is part of a cluster" />
                <asp:ListItem Value="No - This server is NOT part of a cluster" />
            </asp:DropDownList>
        </td>
    </tr>
</table>
<div id="divClusterYesGroup" runat="server" style="display:none">
<table width="100%" border="0" cellSpacing="2" cellPadding="4" class="default">
    <tr>
        <td colspan="2">Is the new SAN going into a new cluster group or an existing one?<font class="required">&nbsp;*</font></td>
    </tr>
    <tr>
        <td><img src="/images/spacer.gif" border="0" width="30" height="1" /></td>
        <td width="100%">
            <asp:DropDownList ID="ddlClusterYesSQLGroup" runat="server" CssClass="default">
                <asp:ListItem Value="-- SELECT --" />
                <asp:ListItem Value="New cluster group" />
                <asp:ListItem Value="Existing cluster group" />
            </asp:DropDownList>
        </td>
    </tr>
</table>
</div>
<div id="divClusterYesGroupNew" runat="server" style="display:none">
<table width="100%" border="0" cellSpacing="2" cellPadding="4" class="default">
    <tr>
        <td colspan="2">Have you requested the following?<font class="required">&nbsp;*</font></td>
    </tr>
    <tr>
        <td><img src="/images/spacer.gif" border="0" width="30" height="1" /></td>
        <td width="100%">
            <table cellpadding="2" cellspacing="1" border="0">
                <tr>
                    <td><asp:CheckBox ID="chkClusterYesGroupNewTSM" runat="server" CssClass="default" Text="TSM Backup" /></td>
                    <td></td>
                </tr>
                <tr>
                    <td><asp:CheckBox ID="chkClusterYesGroupNewNetwork" runat="server" CssClass="default" Text="Network Name" /></td>
                    <td><asp:TextBox ID="txtClusterYesGroupNewNetwork" runat="server" CssClass="default" Width="200" MaxLength="50" /></td>
                </tr>
                <tr>
                    <td><asp:CheckBox ID="chkClusterYesGroupNewIP" runat="server" CssClass="default" Text="IP Address" /></td>
                    <td><asp:TextBox ID="txtClusterYesGroupNewIP" runat="server" CssClass="default" Width="200" MaxLength="50" /></td>
                </tr>
            </table>
        </td>
    </tr>
</table>
</div>
<div id="divClusterYesGroupExisting" runat="server" style="display:none">
<table width="100%" border="0" cellSpacing="2" cellPadding="4" class="default">
    <tr>
        <td colspan="2">What cluster group should the drive be added to?<font class="required">&nbsp;*</font></td>
    </tr>
    <tr>
        <td><img src="/images/spacer.gif" border="0" width="30" height="1" /></td>
        <td width="100%"><asp:TextBox ID="txtClusterYesGroupExisting" runat="server" CssClass="default" Width="300" MaxLength="50" /></td>
    </tr>
    <tr>
        <td colspan="2">Are you requesting the increase in size of an existing filesystem or a brand new filesystem?<font class="required">&nbsp;*</font></td>
    </tr>
    <tr>
        <td><img src="/images/spacer.gif" border="0" width="30" height="1" /></td>
        <td width="100%">
            <asp:DropDownList ID="ddlClusterYesGroupExisting" runat="server" CssClass="default">
                <asp:ListItem Value="-- SELECT --" />
                <asp:ListItem Value="Increase an existing filesystem" />
                <asp:ListItem Value="Brand new filesystem" />
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td colspan="2">Please enter the name of the filesystem:<font class="required">&nbsp;*</font></td>
    </tr>
    <tr>
        <td><img src="/images/spacer.gif" border="0" width="30" height="1" /></td>
        <td width="100%"><asp:TextBox ID="txtClusterYesGroupExistingFileSystem" runat="server" CssClass="default" Width="300" MaxLength="50" /></td>
    </tr>
</table>
</div>
<div id="divClusterNo" runat="server" style="display:none">
<table width="100%" border="0" cellSpacing="2" cellPadding="4" class="default">
    <tr>
        <td colspan="2">Are you requesting the increase in size of an existing filesystem or a brand new filesystem?<font class="required">&nbsp;*</font></td>
    </tr>
    <tr>
        <td><img src="/images/spacer.gif" border="0" width="30" height="1" /></td>
        <td width="100%">
            <asp:DropDownList ID="ddlClusterNo" runat="server" CssClass="default">
                <asp:ListItem Value="-- SELECT --" />
                <asp:ListItem Value="Increase an existing filesystem" />
                <asp:ListItem Value="Brand new filesystem" />
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td colspan="2">Please enter the name of the filesystem:<font class="required">&nbsp;*</font></td>
    </tr>
    <tr>
        <td><img src="/images/spacer.gif" border="0" width="30" height="1" /></td>
        <td width="100%"><asp:TextBox ID="txtClusterNo" runat="server" CssClass="default" Width="300" MaxLength="50" /></td>
    </tr>
</table>
</div>
<table width="100%" border="0" cellSpacing="2" cellPadding="4" class="default">
    <tr>
        <td colspan="2">How much ADDITIONAL storage do you require?<font class="required">&nbsp;*</font></td>
    </tr>
    <tr>
        <td><img src="/images/spacer.gif" border="0" width="30" height="1" /></td>
        <td width="100%"><asp:TextBox id="txtAdditional" runat="server" CssClass="default" Width="150" MaxLength="20" /> GB (Please explain in detail below...)</td>
    </tr>
    <tr>
        <td colspan="2">Please type a short description of the request:<font class="required">&nbsp;*</font></td>
    </tr>
    <tr>
        <td><img src="/images/spacer.gif" border="0" width="30" height="1" /></td>
        <td width="100%"><asp:TextBox ID="txtDescription" runat="server" CssClass="default" Width="500" TextMode="MultiLine" Rows="8" /></td>
    </tr>
    <tr>
        <td></td>
        <td class="footer">Example 1: I would like the R: drive increased from 100GB to 500GB on OHCLESQL1122 for a new database being created.</td>
    </tr>
    <tr>
        <td></td>
        <td class="footer">Example 2: We need existing SAN migrated to new Dalton Replicated SAN for Dalton DR.</td>
    </tr>
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
<asp:Label ID="lblService" runat="server" Visible="false" />
<asp:HiddenField ID="hdnDBA" runat="server" />
