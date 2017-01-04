<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ondemand_server_control.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.ondemand_server_control" %>
<script type="text/javascript">
    function ShowBuildDivs(oDivShow, oDivHide1, oDivHide2, oDivHide3) {
        oDivShow = document.getElementById(oDivShow);
        oDivShow.style.display = "inline";
        oDivHide1 = document.getElementById(oDivHide1);
        oDivHide1.style.display = "none";
        oDivHide2 = document.getElementById(oDivHide2);
        oDivHide2.style.display = "none";
        oDivHide3 = document.getElementById(oDivHide3);
        oDivHide3.style.display = "none";
    }
    function ValidateFreeze(oText, oDiv, oChange, oInChange) {
        oText = document.getElementById(oText);
        oDiv = document.getElementById(oDiv);
        var oDate = new Date(oText.value);
        var oStart = new Date('<%=strFreezeStart %>');
        var oEnd = new Date('<%=strFreezeEnd %>');
        //alert(oDate);
        //alert(oStart);
        //alert(oEnd);
        if (oDate >= oStart && oDate < oEnd) 
        {
            if (oDiv.style.display == "none")
                oDiv.style.display = "inline";
            return ValidateTextLength(oChange, 'Please enter a valid change control number\n\n - Must start with \"CHG\" or \"PTM\"\n - Must be exactly 10 characters in length', 10, 'CHG', 'PTM');
        }
        else if (oDiv.style.display == "inline" && oInChange == false)
                oDiv.style.display = "none";
        return true;
    }
</script>
<asp:Panel ID="panBegin" runat="server" Visible="false">
<table width="100%" height="100%" cellpadding="4" cellspacing="2" border="0">
    <tr height="1">
        <td colspan="2"><b>You are now ready to start the Auto-Provisioning Process.</b></td>
    </tr>
    <tr height="1">
        <td valign="top" colspan="2">
            <ul>
                <li>You can view the status of this process at any time by clicking <b>Execute</b> on the line item in Design Builder.<br /><br /></li>
                <li>You will be sent a <b>Birth Certificate</b> from your implementor when this process is completed.<br /><br /></li>
                <li>Select one of the options below to begin!<br /></li>
            </ul>
        </td>
    </tr>
    <tr height="1" ID="divChange" runat="server" style="display:none">
        <td nowrap><img src="/images/spacer.gif" border="0" width="50" height="1" /></td>
        <td width="100%">
            <table width="100%" cellpadding="2" cellspacing="1" border="0" style="border:solid 1px #999999" bgcolor="#f9f9f9">
                <tr>
                    <td>
                        <table width="100%" cellpadding="0" cellspacing="5" border="0">
                            <tr>
                                <td rowspan="2" valign="top"><img src="/images/hugeAlert.gif" border="0" align="absmiddle" /></td>
                                <td class="bigred" width="100%" valign="bottom" colspan="2">Change Control Required!</td>
                            </tr>
                            <tr>
                                <td width="100%" valign="top" colspan="2">An approved change control is required for all provisioning activities during the end-of-year change freeze.<br /><br />Change Freeze Dates : <%=strFreezeStart %> to <%=strFreezeEnd %>.<br /><br /></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td nowrap>Approved Change Control #:<font class="required">&nbsp;*</font></td>
                                <td width="100%"><asp:TextBox ID="txtChange" runat="server" CssClass="default" /></td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr height="1">
        <td nowrap><img src="/images/spacer.gif" border="0" width="50" height="1" /></td>
        <td width="100%">
            <table width="100%" cellpadding="2" cellspacing="2" border="0" style="border:solid 1px #999999" bgcolor="#f9f9f9">
                <tr>
                    <td rowspan="2"><img src="/images/bigPrint.gif" border="0" align="absmiddle" /></td>
                    <td class="bigblue" width="100%" valign="bottom">Print Your Design</td>
                </tr>
                <tr>
                    <td valign="top">Click <b>Print Design</b> to keep a copy of this design for your records.</td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td><asp:Button ID="btnPrint" runat="server" Text="Print Design" CssClass="default" Width="125" /></td>
                </tr>
            </table>
            <br /><br />
            <asp:Panel id="panInventoryNo" runat="server" visible="false">
                <table width="100%" cellpadding="3" cellspacing="5" border="0" style="border:solid 1px #999999" bgcolor="#f9f9f9">
                    <tr>
                        <td rowspan="5" valign="top"><img src="/images/ico_error.gif" border="0" align="absmiddle" /></td>
                        <td class="header" width="100%" valign="bottom">Out of Inventory</td>
                    </tr>
                    <tr>
                        <td valign="top">At this time, this model has been completely depleated and cannot be executed.</td>
                    </tr>
                    <tr>
                        <td valign="top" class="note">Model: <asp:Label ID="lblInventory" runat="server" /></td>
                    </tr>
                    <tr>
                        <td valign="top">Please contact <u>YOUR</u> manager for more information.</td>
                    </tr>
                    <tr>
                        <td valign="top"><b>NOTE:</b> This is not a ClearView decision, issue or error.</td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel id="panInventoryYes" runat="server" visible="false">
                <table width="100%" cellpadding="2" cellspacing="2" border="0" style="border:solid 1px #999999" bgcolor="#f9f9f9">
                    <tr>
                        <td rowspan="2"><asp:Image ID="imgApproved" runat="server" ImageAlign="absMiddle" ImageUrl="/images/ico_check.gif" /></td>
                        <td class="bigblue" width="100%" valign="bottom"><asp:Label ID="lblApprovedHeader" runat="server" CssClass="bigblue" /></td>
                    </tr>
                    <tr>
                        <td valign="top"><asp:Label ID="lblApproved" runat="server" CssClass="default" /></td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                        <td>
                            <asp:Button ID="btnApproved" runat="server" Text="View Approvals" CssClass="default" Width="125" />&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Button ID="btnSoftwareBack" runat="server" Text="Make Changes" CssClass="default" Width="125" OnClick="btnSoftwareBack_Click" Visible="false" />
                        </td>
                    </tr>
                    <tr id="trApprove" runat="server" style="display:none">
                        <td>&nbsp;</td>
                        <td valign="top">
                            <table width="90%" cellpadding="2" cellspacing="2" border="0" style="border:solid 1px #DDDDDD" bgcolor="#FFFFFF">
                                <tr>
                                    <td>
                                        <%=strMenuTabApprove %>
                                        <div id="divMenuApprove">
                                            <br />
                                            <%=strApprove %>
                                        </div>
                                        <br />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <br /><br />
                <table cellpadding="2" cellspacing="2" border="0">
                    <tr>
                        <td width="50%" valign="top" style="border:solid 1px #999999" bgcolor="#f9f9f9">
                            <table width="100%" cellpadding="2" cellspacing="2" border="0">
                                <tr>
                                    <td rowspan="3" valign="top"><img src="/images/bigHelp.gif" border="0" align="absmiddle" /></td>
                                    <td class="bigblue" width="100%" valign="bottom">Select your Build Option</td>
                                </tr>
                                <tr>
                                    <td valign="top">Please select how you want to build your device(s)...</td>
                                </tr>
                                <tr>
                                    <td valign="top">
                                        <table cellpadding="3" cellspacing="2" border="0">
                                            <tr>
                                                <td><asp:RadioButton ID="radStart" runat="server" CssClass="default" Text="Option 1: Start the Build Now" GroupName="buildOptions" /></td>
                                            </tr>
                                            <tr>
                                                <td><asp:RadioButton ID="radSchedule" runat="server" CssClass="default" Text="Option 2: Schedule the Build" GroupName="buildOptions" /></td>
                                            </tr>
                                            <tr>
                                                <td><asp:RadioButton ID="radApproval" runat="server" CssClass="default" Text="Option 3: Request Approval" GroupName="buildOptions" Enabled="false" /></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td>&nbsp;</td>
                        <td width="50%" valign="top" style="border:solid 1px #999999" bgcolor="#f9f9f9">
                            <div id="divSoftware" runat="server" style="display:none">
                                <table width="100%" cellpadding="2" cellspacing="2" border="0">
                                    <tr>
                                        <td rowspan="5" valign="top"><img src="/images/hugeAlert.gif" border="0" align="absmiddle" /></td>
                                        <td class="bigblue" width="100%" valign="bottom">Build Option Unavailable</td>
                                    </tr>
                                    <tr>
                                        <td valign="top">You cannot select a build option until all software components are approved.</td>
                                    </tr>
                                    <tr>
                                        <td valign="top">&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td valign="top">&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td valign="top">&nbsp;</td>
                                    </tr>
                                </table>
                            </div>
                            <div id="divDefault" runat="server" style="display:inline">
                                <table width="100%" cellpadding="2" cellspacing="2" border="0">
                                    <tr>
                                        <td rowspan="5" valign="top"><img src="/images/ico_hourglass.gif" border="0" align="absmiddle" /></td>
                                        <td class="bigblue" width="100%" valign="bottom">Please select a Build Option</td>
                                    </tr>
                                    <tr>
                                        <td valign="top">Select a build option to configure your build.</td>
                                    </tr>
                                    <tr>
                                        <td valign="top">&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td valign="top">&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td valign="top">&nbsp;</td>
                                    </tr>
                                </table>
                            </div>
                            <div id="divStart" runat="server" style="display:none">
                                <table width="100%" cellpadding="2" cellspacing="2" border="0">
                                    <tr>
                                        <td rowspan="5" valign="top"><img src="/images/step_1.gif" border="0" align="absmiddle" /></td>
                                        <td class="bigblue" width="100%" valign="bottom">Option 1: Start the Build Now</td>
                                    </tr>
                                    <tr>
                                        <td valign="top">Click <b>Start the Build</b> to start your auto-provisioning build.</td>
                                    </tr>
                                    <tr>
                                        <td valign="top">&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td><asp:Button ID="btnStart" runat="server" OnClick="btnStart_Click" Text="Start the Build" CssClass="default" Width="125" /></td>
                                    </tr>
                                    <tr>
                                        <td> <asp:Label ID="lblNotify" runat="server" CssClass="reddefault" Text="<b>BURN-IN PROCESS:</b> Upon clicking this button, the design implementor will be notified of your request." Visible="false" /></td>
                                    </tr>
                                </table>
                            </div>
                            <div id="divSchedule" runat="server" style="display:none">
                                <table width="100%" cellpadding="2" cellspacing="2" border="0">
                                    <tr>
                                        <td rowspan="4" valign="top"><img src="/images/step_2.gif" border="0" align="absmiddle" /></td>
                                        <td class="bigblue" width="100%" valign="bottom">Option 2: Schedule the Build</td>
                                    </tr>
                                    <tr>
                                        <td valign="top">Enter the date and time on which you want the device built and click <b>Schedule the Build</b> to have ClearView build the device for you.</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table cellpadding="2" cellspacing="0" border="0">
                                                <tr>
                                                    <td><asp:TextBox ID="txtScheduleDate" runat="server" CssClass="default" Width="100" MaxLength="10" /></td>
                                                    <td><asp:ImageButton ID="imgScheduleDate" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calendar.gif" /></td>
                                                    <td>at</td>
                                                    <td><asp:TextBox ID="txtScheduleTime" runat="server" CssClass="default" Width="75" MaxLength="8" /></td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2"></td>
                                                    <td colspan="2" class="footer">Example:&nbsp;12:15&nbsp;AM</td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td><asp:Button ID="btnSchedule" runat="server" OnClick="btnSchedule_Click" Text="Schedule the Build" CssClass="default" Width="125" Enabled="false" /></td>
                                    </tr>
                                </table>
                            </div>
                            <div id="divApproval" runat="server" style="display:none">
                                <table width="100%" cellpadding="2" cellspacing="2" border="0">
                                    <tr>
                                        <td rowspan="5" valign="top"><img src="/images/step_3.gif" border="0" align="absmiddle" /></td>
                                        <td class="bigblue" width="100%" valign="bottom">Option 3: Request Approval</td>
                                    </tr>
                                    <tr>
                                        <td valign="top">Request approval from one or more people.</td>
                                    </tr>
                                    <tr>
                                        <td valign="top">&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td><asp:Button ID="btnApprovals" runat="server" Text="Add / Remove Approvers" CssClass="default" Width="175" /></td>
                                    </tr>
                                    <tr>
                                        <td valign="top"><b>NOTE:</b> Once approved, the build will automatically start.</td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <br /><br />
            <table width="100%" cellpadding="2" cellspacing="2" border="0">
                <tr>
                    <td rowspan="2"><asp:LinkButton ID="btnBack1" runat="server" Text="<img src='/images/bigArrowLeft.gif' border='0' align='absmiddle' />" OnClick="btnBack_Click" /></td>
                    <td class="bigblue" width="100%" valign="bottom"><asp:LinkButton ID="btnBack2" runat="server" CssClass="bigblue" Text="Go Back to Make Changes" OnClick="btnBack_Click" /></td>
                </tr>
                <tr>
                    <td valign="top"><asp:LinkButton ID="btnBack3" runat="server" CssClass="default" Text="Click here to return to the design execution wizard to make changes." OnClick="btnBack_Click" /></td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td colspan="2">&nbsp;</td>
    </tr>
</table>
</asp:Panel>
<asp:Panel ID="panPending" runat="server" Visible="false">
<table width="100%" height="100%" cellpadding="4" cellspacing="2" border="0">
    <tr>
        <td align="center" valign="top">
            <table cellpadding="2" cellspacing="1" border="0" style="border:solid 1px #999999" bgcolor="#f9f9f9">
                <tr>
                    <td>
                        <table width="100%" cellpadding="0" cellspacing="5" border="0">
                            <tr>
                                <td rowspan="2"><img src="/images/hugeAlert.gif" border="0" align="absmiddle" /></td>
                                <td class="header" width="100%" valign="bottom">Pending Execution</td>
                            </tr>
                            <tr>
                                <td width="100%" valign="top">This build has been scheduled and cannot be changed. To cancel this build, please contact your ClearView administrator.</td>
                            </tr>
                            <tr>
                                <td></td>
                                <td width="100%">
                                    <table cellpadding="0" cellspacing="5" border="0">
                                        <tr>
                                            <td class="bigger">Start Date:</td>
                                            <td><asp:Label ID="lblScheduleDate" runat="server" CssClass="bigger" /></td>
                                        </tr>
                                        <tr>
                                            <td class="bigger">Start Time:</td>
                                            <td><asp:Label ID="lblScheduleTime" runat="server" CssClass="bigger" /></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td width="100%" colspan="2">&nbsp;</td>
                            </tr>
                            <tr>
                                <td width="100%" align="center" class="redheader" colspan="2"><asp:Label ID="lblCountdown" runat="server" CssClass="redheader" /></td>
                            </tr>
                            <tr>
                                <td width="100%" colspan="2">&nbsp;</td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
	    </td>
    </tr>
</table>
</asp:Panel>
<asp:Panel ID="panNotExecutable" runat="server" Visible="false">
<table width="100%" height="100%" cellpadding="4" cellspacing="2" border="0">
    <tr>
        <td align="center" valign="top">
            <table cellpadding="2" cellspacing="1" border="0" style="border:solid 1px #999999" bgcolor="#f9f9f9">
                <tr>
                    <td>
                        <table width="100%" cellpadding="0" cellspacing="5" border="0">
                            <tr>
                                <td rowspan="2"><%=strManualImage %></td>
                                <td class="header" width="100%" valign="bottom">Manual Build Process</td>
                            </tr>
                            <tr>
                                <td width="100%" valign="top">This design requires a technicain to manually build the device(s). Your implementor has been notified of the request to build this design.</td>
                            </tr>
                            <tr>
                                <td></td>
                                <td width="100%">
                                    <table cellpadding="0" cellspacing="5" border="0">
                                        <tr>
                                            <td class="bigger">Implementor:</td>
                                            <td><asp:Label ID="lblImplementor" runat="server" CssClass="bigger" /></td>
                                        </tr>
                                        <tr>
                                            <td class="bigger">Initiated On:</td>
                                            <td><asp:Label ID="lblInitiated" runat="server" CssClass="bigger" /></td>
                                        </tr>
                                        <tr>
                                            <td class="bigger">Completed On:</td>
                                            <td><asp:Label ID="lblCompleted" runat="server" CssClass="bigger" /></td>
                                        </tr>
                                        <tr>
                                            <td class="bigger">Reason:</td>
                                            <td><%=strManualReason %></td>
                                        </tr>
                                        <tr>
                                            <td width="100%" colspan="2">&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td width="100%" colspan="2"><asp:HyperLink ID="btnManual" runat="server" Target="_blank">Click here for additional information.</asp:HyperLink></td>
                                        </tr>
                                        <tr>
                                            <td width="100%" colspan="2">&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td width="100%" colspan="2">Here is the current information available for your device(s)...</td>
                                        </tr>
                                        <tr>
                                            <td width="100%" colspan="2">
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
                                                    </tr>
                                                    <asp:repeater ID="rptServers" runat="server">
                                                        <ItemTemplate>
                                                            <tr class="default" style='<%=intDeviceCount % 2 != 0 ? "" : "background-color:#F6F6F6"%>'>
                                                                <td>Device&nbsp;#&nbsp;<%=intDeviceCount++ %></td>
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
                                                            </tr>
                                                        </ItemTemplate>
                                                    </asp:repeater>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td width="100%" colspan="2">&nbsp;</td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
	    </td>
    </tr>
</table>
</asp:Panel>
<asp:Panel ID="panStart" runat="server" Visible="false">
<table width="100%" height="100%" cellpadding="4" cellspacing="2" border="0">
    <tr height="1">
        <td valign="top">
                <%=strMenuTab1 %>
	            <div id="divMenu1"> 
	            <%=strDivs %>
	            </div> 
	    </td>
    </tr>
	<!-- 
    <tr height="1">
        <td valign="top"><%=strMenuTab1 %></td>
    </tr>
    <tr>
        <td valign="top"><%=strDivs %></td>
    </tr>
    -->
</table>
</asp:Panel>