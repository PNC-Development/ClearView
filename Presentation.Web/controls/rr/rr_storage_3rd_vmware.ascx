<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="rr_storage_3rd_vmware.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.rr_storage_3rd_vmware" %>



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
            oDiv = document.getElementById('<%=divClusterYes.ClientID %>');
            oDiv.style.display = "none";
            oDiv = document.getElementById('<%=divClusterYesSQLYes.ClientID %>');
            oDiv.style.display = "none";
            oDiv = document.getElementById('<%=divClusterYesSQLNo.ClientID %>');
            oDiv.style.display = "none";
            oDiv = document.getElementById('<%=divClusterYesSQLNoMount.ClientID %>');
            oDiv.style.display = "none";
            oDiv = document.getElementById('<%=divClusterYesGroupNew.ClientID %>');
            oDiv.style.display = "none";
            oDiv = document.getElementById('<%=divClusterYesGroupExisting.ClientID %>');
            oDiv.style.display = "none";
            oDiv = document.getElementById('<%=divClusterNo.ClientID %>');
            oDiv.style.display = "none";
            oDiv = document.getElementById('<%=divClusterNoSQLYes.ClientID %>');
            oDiv.style.display = "none";
            oDiv = document.getElementById('<%=divSQLYes2005.ClientID %>');
            oDiv.style.display = "none";
            oDiv = document.getElementById('<%=divClusterNoSQLNo.ClientID %>');
            oDiv.style.display = "none";
            oDiv = document.getElementById('<%=divClusterYesGroup.ClientID %>');
            oDiv.style.display = "none";
        }
    }
    function EnsureStorage3rd(oCluster, oClusterNoSQL, oClusterNoSQLNo, oClusterNoSQLYesVersion, oDBA, oClusterNoSQLDBA, oClusterYesSQL, oClusterYesSQLYesVersion, oClusterYesSQLGroup, txtClusterYesGroupExisting, chkClusterYesGroupNewNetwork, txtClusterYesGroupNewNetwork, chkClusterYesGroupNewIP, txtClusterYesGroupNewIP, txtClusterYesSQLDBA, ddlClusterYesSQLNoType, txtClusterYesSQLNoMount) {
        oCluster = document.getElementById(oCluster);
        if (oCluster.selectedIndex == 1) {
            // CLUSTER = YES
            if (ValidateDropDown(oClusterYesSQL, 'Please select if this is a SQL server') == false)
                return false;
            else {
                oClusterYesSQL = document.getElementById(oClusterYesSQL);
                if (oClusterYesSQL.selectedIndex == 1) {
                    // SQL = YES
                    if (ValidateDropDown(oClusterYesSQLYesVersion, 'Please select the SQL Server Version') == false)
                        return false;
                    if (ValidateHidden0(oDBA, txtClusterYesSQLDBA, 'Please enter the DBA') == false)
                        return false;
                }
                else {
                    // SQL = NO
                    if (ValidateDropDown(ddlClusterYesSQLNoType, 'Please select if you are requesting a new drive letter or a mount point') == false)
                        return false;
                    else {
                        ddlClusterYesSQLNoType = document.getElementById(ddlClusterYesSQLNoType);
                        if (ddlClusterYesSQLNoType.selectedIndex == 1) {
                            // DRIVE
                        }
                        else {
                            // MOUNT
                            if (ValidateText(txtClusterYesSQLNoMount, 'Please enter where the mount point should be attached') == false)
                                return false;
                        }
                    }
                }
            }
            if (ValidateDropDown(oClusterYesSQLGroup, 'Please select if the new SAN is going into a new cluster group or an existing one') == false)
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
                }
            }
        }
        else {
            // CLUSTER = NO
            if (ValidateDropDown(oClusterNoSQL, 'Please select if this is a SQL server') == false)
                return false;
            else {
                oClusterNoSQL = document.getElementById(oClusterNoSQL);
                if (oClusterNoSQL.selectedIndex == 1) {
                    // SQL = YES
                    if (ValidateDropDown(oClusterNoSQLYesVersion, 'Please select the SQL Server Version') == false)
                        return false;
                    if (ValidateHidden0(oDBA, oClusterNoSQLDBA, 'Please enter the DBA') == false)
                        return false;
                }
                else {
                    // SQL = NO
                    if (ValidateDropDown(oClusterNoSQLNo, 'Please make a selection') == false)
                        return false;
                }
            }
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
        <td class="footer">NOTE: This date is an estimate. Our Service Level Agreement for storage requests is 20 business days.</td>
    </tr>
    <tr>
        <td colspan="2">Please select your operating system:<font class="required">&nbsp;*</font></td>
    </tr>
    <tr>
        <td><img src="/images/spacer.gif" border="0" width="30" height="1" /></td>
        <td width="100%">
            <asp:DropDownList ID="ddlOS" runat="server" CssClass="default">
                <asp:ListItem Value="-- SELECT --" />
                <asp:ListItem Value="Windows 2000 Standard 32-bit" />
                <asp:ListItem Value="Windows 2003 Standard 32-bit" />
                <asp:ListItem Value="Windows 2003 Enterprise 32-bit" />
                <asp:ListItem Value="Windows 2003 Standard 64-bit" />
                <asp:ListItem Value="Windows 2003 Enterprise 64-bit" />
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
<div id="divClusterYes" runat="server" style="display:none">
<table width="100%" border="0" cellSpacing="2" cellPadding="4" class="default">
    <tr>
        <td colspan="2">Is this a SQL Server?<font class="required">&nbsp;*</font></td>
    </tr>
    <tr>
        <td><img src="/images/spacer.gif" border="0" width="30" height="1" /></td>
        <td width="100%">
            <asp:DropDownList ID="ddlClusterYesSQL" runat="server" CssClass="default">
                <asp:ListItem Value="-- SELECT --" />
                <asp:ListItem Value="Yes - This is a SQL Server" />
                <asp:ListItem Value="No - This is NOT a SQL Server" />
            </asp:DropDownList>
        </td>
    </tr>
</table>
</div>
<div id="divClusterYesSQLYes" runat="server" style="display:none">
<table width="100%" border="0" cellSpacing="2" cellPadding="4" class="default">
    <tr>
        <td colspan="2">What version of SQL?<font class="required">&nbsp;*</font></td>
    </tr>
    <tr>
        <td><img src="/images/spacer.gif" border="0" width="30" height="1" /></td>
        <td width="100%">
            <asp:DropDownList ID="ddlClusterYesSQLYesVersion" runat="server" CssClass="default">
                <asp:ListItem Value="-- SELECT --" />
                <asp:ListItem Value="SQL 2008 (new SAN is typically added as a mount point)" />
                <asp:ListItem Value="SQL 2005 (new SAN is typically added as a mount point)" />
                <asp:ListItem Value="SQL 2000 (new SAN is typically added as a new drive letter)" />
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td colspan="2">Who is the DBA that will be working on this request?<font class="required">&nbsp;*</font></td>
    </tr>
    <tr>
        <td><img src="/images/spacer.gif" border="0" width="30" height="1" /></td>
        <td width="100%">
            <table cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td><asp:TextBox ID="txtClusterYesSQLDBA" runat="server" Width="300" CssClass="default" /></td>
                </tr>
                <tr>
                    <td>
                        <div id="divClusterYesSQLDBA" runat="server" style="overflow:hidden; position:absolute; display:none; background-color:#FFFFFF; border:solid 1px #CCCCCC">
                            <asp:ListBox ID="lstClusterYesSQLDBA" runat="server" CssClass="default" />
                        </div>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
</div>
<div id="divClusterYesSQLNo" runat="server" style="display:none">
<table width="100%" border="0" cellSpacing="2" cellPadding="4" class="default">
    <tr>
        <td colspan="2">Please select whether you want a new drive letter or mount point?<font class="required">&nbsp;*</font></td>
    </tr>
    <tr>
        <td><img src="/images/spacer.gif" border="0" width="30" height="1" /></td>
        <td width="100%">
            <asp:DropDownList ID="ddlClusterYesSQLNoType" runat="server" CssClass="default">
                <asp:ListItem Value="-- SELECT --" />
                <asp:ListItem Value="New Drive Letter" />
                <asp:ListItem Value="New Mount Point" />
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td></td>
        <td class="footer">NOTE: New drive letters will be configured with the next available SAN drive letter (R-Z)</td>
    </tr>
</table>
</div>
<div id="divClusterYesSQLNoMount" runat="server" style="display:none">
<table width="100%" border="0" cellSpacing="2" cellPadding="4" class="default">
    <tr>
        <td colspan="2">Where should the mount point be attached?<font class="required">&nbsp;*</font></td>
    </tr>
    <tr>
        <td><img src="/images/spacer.gif" border="0" width="30" height="1" /></td>
        <td width="100%"><asp:TextBox ID="txtClusterYesSQLNoMount" runat="server" CssClass="default" Width="200" MaxLength="50" /></td>
    </tr>
</table>
</div>
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
</table>
</div>
<div id="divClusterNo" runat="server" style="display:none">
<table width="100%" border="0" cellSpacing="2" cellPadding="4" class="default">
    <tr>
        <td colspan="2">Is this a SQL Server?<font class="required">&nbsp;*</font></td>
    </tr>
    <tr>
        <td><img src="/images/spacer.gif" border="0" width="30" height="1" /></td>
        <td width="100%">
            <asp:DropDownList ID="ddlClusterNoSQL" runat="server" CssClass="default">
                <asp:ListItem Value="-- SELECT --" />
                <asp:ListItem Value="Yes - This is a SQL Server" />
                <asp:ListItem Value="No - This is NOT a SQL Server" />
            </asp:DropDownList>
        </td>
    </tr>
</table>
</div>
<div id="divClusterNoSQLYes" runat="server" style="display:none">
<table width="100%" border="0" cellSpacing="2" cellPadding="4" class="default">
    <tr>
        <td colspan="2">What version of SQL?<font class="required">&nbsp;*</font></td>
    </tr>
    <tr>
        <td><img src="/images/spacer.gif" border="0" width="30" height="1" /></td>
        <td width="100%">
            <asp:DropDownList ID="ddlClusterNoSQLYesVersion" runat="server" CssClass="default">
                <asp:ListItem Value="-- SELECT --" />
                <asp:ListItem Value="SQL 2008 (new SAN is typically added as a mount point)" />
                <asp:ListItem Value="SQL 2005 (new SAN is typically added as a mount point)" />
                <asp:ListItem Value="SQL 2000 (new SAN is typically added as a new drive letter)" />
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td colspan="2">Who is the DBA that will be working on this request?<font class="required">&nbsp;*</font></td>
    </tr>
    <tr>
        <td><img src="/images/spacer.gif" border="0" width="30" height="1" /></td>
        <td width="100%">
            <table cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td><asp:TextBox ID="txtClusterNoSQLDBA" runat="server" Width="300" CssClass="default" /></td>
                </tr>
                <tr>
                    <td>
                        <div id="divClusterNoSQLDBA" runat="server" style="overflow:hidden; position:absolute; display:none; background-color:#FFFFFF; border:solid 1px #CCCCCC">
                            <asp:ListBox ID="lstClusterNoSQLDBA" runat="server" CssClass="default" />
                        </div>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
</div>
<div id="divSQLYes2005" runat="server" style="display:none">
<table width="100%" border="0" cellSpacing="2" cellPadding="4" class="default">
    <tr>
        <td colspan="2">Are you requesting an additional database/SQL0x or backup/SQL0x folder?<font class="required">&nbsp;*</font></td>
    </tr>
    <tr>
        <td><img src="/images/spacer.gif" border="0" width="30" height="1" /></td>
        <td width="100%">
            <asp:CheckBoxList ID="chkSQLYes2005" runat="server" CssClass="default">
                <asp:ListItem Value="database/SQL0x Folder" />
                <asp:ListItem Value="backup/SQL0x Folder" />
            </asp:CheckBoxList>
        </td>
    </tr>
</table>
</div>
<div id="divClusterNoSQLNo" runat="server" style="display:none">
<table width="100%" border="0" cellSpacing="2" cellPadding="4" class="default">
    <tr>
        <td colspan="2">Are you requesting the increase in size of an existing drive letter or a brand new drive letter (max size 750 GB)?<font class="required">&nbsp;*</font></td>
    </tr>
    <tr>
        <td><img src="/images/spacer.gif" border="0" width="30" height="1" /></td>
        <td width="100%">
            <asp:DropDownList ID="ddlClusterNoSQLNo" runat="server" CssClass="default">
                <asp:ListItem Value="-- SELECT --" />
                <asp:ListItem Value="Increase an existing drive letter" />
                <asp:ListItem Value="Brand new drive letter" />
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td></td>
        <td class="footer">NOTE: New drive letters will be configured with the next available SAN drive letter (R-Z)</td>
    </tr>
</table>
</div>
<table width="100%" border="0" cellSpacing="2" cellPadding="4" class="default">
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
</table>
<table width="100%" border="0" cellSpacing="2" cellPadding="4" class="default">
    <tr>
        <td colspan="2"><hr size="1" noshade /></td>
    </tr>
    <tr>
        <td class="header">Additonal Questions</td>
    </tr>
    <tr>
        <td>
            <table width="100%" border="0" cellSpacing="2" cellPadding="4" class="default">
                <tr>
                    <td colspan="2">What Class is this device in?<font class="required">&nbsp;*</font></td>
                </tr>
                <tr>
                    <td><img src="/images/spacer.gif" border="0" width="30" height="1" /></td>
                    <td width="100%"><asp:DropDownList ID="ddlClass" runat="server" CssClass="default" /></td>
                </tr>
                <tr>
                    <td colspan="2">What Environment is this device in?<font class="required">&nbsp;*</font></td>
                </tr>
                <tr>
                    <td><img src="/images/spacer.gif" border="0" width="30" height="1" /></td>
                    <td width="100%">
                        <asp:DropDownList ID="ddlEnvironment" CssClass="default" runat="server" Width="300" Enabled="false" >
                            <asp:ListItem Value="-- Please select a Class --" />
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">What Location is this device in?<font class="required">&nbsp;*</font></td>
                </tr>
                <tr>
                    <td><img src="/images/spacer.gif" border="0" width="30" height="1" /></td>
                    <td width="100%"><%=strLocation %></td>
                </tr>
                <tr>
                    <td colspan="2">What fabric is this device on?<font class="required">&nbsp;*</font></td>
                </tr>
                <tr>
                    <td><img src="/images/spacer.gif" border="0" width="30" height="1" /></td>
                    <td width="100%">
                        <asp:DropDownList ID="ddlFabric" runat="server" CssClass="default">
                            <asp:ListItem Value="-- SELECT --" />
                            <asp:ListItem Value="Cisco" />
                            <asp:ListItem Value="Brocade" />
                            <asp:ListItem Value="VMAX" />
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">Is this device being replicated:<font class="required">&nbsp;*</font></td>
                </tr>
                <tr>
                    <td><img src="/images/spacer.gif" border="0" width="30" height="1" /></td>
                    <td width="100%">
                        <asp:DropDownList ID="ddlReplicated" runat="server" CssClass="default">
                            <asp:ListItem Value="-- SELECT --" />
                            <asp:ListItem Value="Yes" />
                            <asp:ListItem Value="No" />
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">Does this device require high availability storage:<font class="required">&nbsp;*</font></td>
                </tr>
                <tr>
                    <td><img src="/images/spacer.gif" border="0" width="30" height="1" /></td>
                    <td width="100%">
                        <asp:DropDownList ID="ddlHA" runat="server" CssClass="default">
                            <asp:ListItem Value="-- SELECT --" />
                            <asp:ListItem Value="Yes" />
                            <asp:ListItem Value="No" />
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">Please select a type of storage:<font class="required">&nbsp;*</font></td>
                </tr>
                <tr>
                    <td><img src="/images/spacer.gif" border="0" width="30" height="1" /></td>
                    <td width="100%">
                        <asp:DropDownList ID="ddlType2" runat="server" CssClass="default">
                            <asp:ListItem Value="-- SELECT --" />
                            <asp:ListItem Value="Shared" />
                            <asp:ListItem Value="Non-Shared" />
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">Do you want to expand a LUN or add an additional LUN?<font class="required">&nbsp;*</font></td>
                </tr>
                <tr>
                    <td><img src="/images/spacer.gif" border="0" width="30" height="1" /></td>
                    <td width="100%">
                        <asp:DropDownList ID="ddlExpand" runat="server" CssClass="default">
                            <asp:ListItem Value="-- SELECT --" />
                            <asp:ListItem Value="Expand a LUN" />
                            <asp:ListItem Value="Add an Additional LUN" />
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">Additional Total Capacity Needed (in GB):<font class="required">&nbsp;*</font></td>
                </tr>
                <tr>
                    <td><img src="/images/spacer.gif" border="0" width="30" height="1" /></td>
                    <td width="100%"><asp:TextBox ID="txtAdditional" runat="server" CssClass="default" Width="300" MaxLength="50" /></td>
                </tr>
                <tr>
                    <td colspan="2">Please enter the LUN drive and UID, followed by the amount of storage you want to have added to that LUN (additional capacity only) (in GB):<font class="required">&nbsp;*</font></td>
                </tr>
                <tr>
                    <td><img src="/images/spacer.gif" border="0" width="30" height="1" /></td>
                    <td width="100%">
                        <table cellpadding="2" cellspacing="0" border="0">
                            <tr>
                                <td colspan="2"><asp:TextBox ID="txtLUNs" runat="server" CssClass="default" Width="500" MaxLength="200" /></td>
                            </tr>
                            <tr>
                                <td><asp:Button ID="btnLunAdd" runat="server" CssClass="default" Text="Add to List" Width="75" /></td>
                                <td align="right"><asp:Button ID="btnLunDelete" runat="server" CssClass="default" Text="Remove from List" Width="115" /></td>
                            </tr>
                            <tr>
                                <td colspan="2"><asp:ListBox ID="lstLUNs" runat="server" CssClass="default" Width="500" Rows="8" /></td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td class="footer">Example 1: R: UID#1 100GB</td>
                </tr>
                <tr>
                    <td></td>
                    <td class="footer">Example 2: S: UID#2 200GB</td>
                </tr>
                <tr>
                    <td colspan="2">Enter the World Wide Port Names:<font class="required">&nbsp;*</font></td>
                </tr>
                <tr>
                    <td><img src="/images/spacer.gif" border="0" width="30" height="1" /></td>
                    <td width="100%"><asp:TextBox ID="txtWWW" runat="server" CssClass="default" Width="300" MaxLength="100" /></td>
                </tr>
                <tr>
                    <td colspan="2">[If Expanding a LUN] UID:<font class="required">&nbsp;*</font></td>
                </tr>
                <tr>
                    <td><img src="/images/spacer.gif" border="0" width="30" height="1" /></td>
                    <td width="100%"><asp:TextBox ID="txtUID" runat="server" CssClass="default" Width="300" MaxLength="100" /></td>
                </tr>
                <tr>
                    <td colspan="2">[If Clustered] Clustered NODE Server Names:<font class="required">&nbsp;*</font></td>
                </tr>
                <tr>
                    <td><img src="/images/spacer.gif" border="0" width="30" height="1" /></td>
                    <td width="100%"><asp:TextBox ID="txtNode" runat="server" CssClass="default" Width="300" MaxLength="100" /></td>
                </tr>
                <tr>
                    <td colspan="2">[If Blade] Enclosure Name:<font class="required">&nbsp;*</font></td>
                </tr>
                <tr>
                    <td><img src="/images/spacer.gif" border="0" width="30" height="1" /></td>
                    <td width="100%"><asp:TextBox ID="txtEnclosureName" runat="server" CssClass="default" Width="300" MaxLength="100" /></td>
                </tr>
                <tr>
                    <td colspan="2">[If Blade] Enclosure Slot:<font class="required">&nbsp;*</font></td>
                </tr>
                <tr>
                    <td><img src="/images/spacer.gif" border="0" width="30" height="1" /></td>
                    <td width="100%"><asp:TextBox ID="txtEnclosureSlot" runat="server" CssClass="default" Width="100" MaxLength="10" /></td>
                </tr>
                <tr>
                    <td colspan="2">[If Replicated] Replicated Server Name (of the DR device):<font class="required">&nbsp;*</font></td>
                </tr>
                <tr>
                    <td><img src="/images/spacer.gif" border="0" width="30" height="1" /></td>
                    <td width="100%"><asp:TextBox ID="txtReplicatedServerName" runat="server" CssClass="default" Width="300" MaxLength="100" /></td>
                </tr>
                <tr>
                    <td colspan="2">[If Replicated] Replicated World Wide Port names (of the DR device):<font class="required">&nbsp;*</font></td>
                </tr>
                <tr>
                    <td><img src="/images/spacer.gif" border="0" width="30" height="1" /></td>
                    <td width="100%"><asp:TextBox ID="txtReplicatedWWW" runat="server" CssClass="default" Width="300" MaxLength="100" /></td>
                </tr>
                <tr>
                    <td colspan="2">[If Blade AND Replicated] Replicated Enclosure Name (of the DR device):<font class="required">&nbsp;*</font></td>
                </tr>
                <tr>
                    <td><img src="/images/spacer.gif" border="0" width="30" height="1" /></td>
                    <td width="100%"><asp:TextBox ID="txtReplicatedEnclosureName" runat="server" CssClass="default" Width="300" MaxLength="100" /></td>
                </tr>
                <tr>
                    <td colspan="2">[If Blade AND Replicated] Replicated Enclosure Slot (of the DR device):<font class="required">&nbsp;*</font></td>
                </tr>
                <tr>
                    <td><img src="/images/spacer.gif" border="0" width="30" height="1" /></td>
                    <td width="100%"><asp:TextBox ID="txtReplicatedEnclosureSlot" runat="server" CssClass="default" Width="100" MaxLength="10" /></td>
                </tr>
            </table>
        </td>
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
<asp:Label ID="lblMidrange" runat="server" Visible="false" />
<asp:HiddenField ID="hdnDBA" runat="server" />
<asp:HiddenField ID="hdnLUNs" runat="server" />
<input type="hidden" id="hdnLocation" runat="server" />
<input type="hidden" id="hdnEnvironment" runat="server" />
