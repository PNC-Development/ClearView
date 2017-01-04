<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wm_storage_3rd_mid.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.wm_storage_3rd_mid" %>


<script type="text/javascript">
    function ChangeFrame(oCell, oShow, oHide1, oHidden, strHidden) {
        oShow = document.getElementById(oShow);
        oShow.style.display = "inline";
        oHide1 = document.getElementById(oHide1);
        oHide1.style.display = "none";
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
<asp:Panel ID="panWorkload" runat="server" Visible="false">
    <table width="100%" border="0" cellSpacing="0" cellPadding="0" class="default">    
        <tr>
            <td>
                <table width="100%" border="0" cellSpacing="0" cellPadding="0">
                    <tr>
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
                                    <td nowrap><b>Requested By:</b></td>
                                    <td><asp:Label ID="lblRequestedBy" runat="server" CssClass="default" /></td>
                                </tr>
                                <tr>
                                    <td nowrap><b>Requested On:</b></td>
                                    <td><asp:Label ID="lblRequestedOn" runat="server" CssClass="default" /></td>
                                </tr>
                                <tr>
                                    <td nowrap><b>Last Updated:</b></td>
                                    <td><asp:Label ID="lblUpdated" runat="server" CssClass="default" /></td>
                                </tr>
                                <tr>
                                    <td nowrap><b>Custom Task Name:</b></td>
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
                <asp:Panel ID="panComplete" runat="server" Visible="false">
                    <table border="0" cellSpacing="2" cellPadding="2" style="border:solid 1px #999999" bgcolor="#f9f9f9">
                        <tr>
                            <td class="bigreddefault"><img src="/images/bigAlert.gif" border="0" align="absmiddle" /> <b>NOTE: Don't forget to click the <img src="/images/tool_complete.gif" border="0" align="absmiddle" /> button to finish this task!!!</b></td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
        <tr height="16">
            <td><img src="/images/spacer.gif" border="0" height="16" width="1" /></td>
        </tr>
        <tr>
            <td>
                <table width="100%" cellpadding="4" cellspacing="3" border="0">
                    <tr>
                        <td><hr size="1" noshade /></td>
                    </tr>
                    <asp:Panel ID="panReject" runat="server" Visible="true">
                    <tr>
                        <td><asp:Button ID="btnReject" runat="server" CssClass="default" Width="150" Text="Reject the Request" OnClick="btnReject_Click" /></td>
                    </tr>
                    <tr>
                        <td><asp:TextBox ID="txtReject" runat="server" CssClass="default" Width="500" TextMode="MultiLine" Rows="8" /></td>
                    </tr>
                    </asp:Panel>
                    <tr>
                        <td class="header">Original Request</td>
                    </tr>
                    <tr>
                        <td>
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
                                    <td width="100%"><asp:Label ID="lblDate" runat="server" CssClass="default" /></td>
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
                                    <td width="100%"><asp:TextBox id="txtAdditionalClient" runat="server" CssClass="default" Width="150" MaxLength="20" /> GB (Please explain in detail below...)</td>
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
                            </table>
                        </td>
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
                                    <td width="100%"><asp:TextBox ID="txtWWW" runat="server" CssClass="default" Width="300" MaxLength="200" /></td>
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
                                    <td width="100%"><asp:TextBox ID="txtReplicatedWWW" runat="server" CssClass="default" Width="300" MaxLength="200" /></td>
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
                </table>
            </td>
        </tr>
         <tr>
            <td>
                 <table width="100%" cellpadding="4" cellspacing="3" border="0">
                        <tr>
                            <td colspan="2">&nbsp;</td>
                        </tr>
                        <tr>
                            <td colspan="2" class="greentableheader">Status Updates</td>
                        </tr>
                        <tr>
                            <td nowrap>Status:</td>
                            <td style="width: 98%">
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
                                                <td width="100%" valign="top" >
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
<asp:Label ID="lblMidrange" runat="server" Visible="false" />
<asp:HiddenField ID="hdnTab" runat="server" />
<asp:HiddenField ID="hdnDBA" runat="server" />
<asp:HiddenField ID="hdnLUNs" runat="server" />
<input type="hidden" id="hdnLocation" runat="server" />
<input type="hidden" id="hdnEnvironment" runat="server" />
