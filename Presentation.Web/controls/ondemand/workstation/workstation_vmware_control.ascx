<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="workstation_vmware_control.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.workstation_vmware_control" %>


<script type="text/javascript">
</script>
<asp:Panel ID="panBegin" runat="server" Visible="false">
<table width="100%" height="100%" cellpadding="4" cellspacing="2" border="0">
    <tr height="1">
        <td colspan="2"><b>You are now ready to start the Auto-Provisioning Process...</b></td>
    </tr>
    <tr height="1">
        <td nowrap><img src="/images/spacer.gif" border="0" width="50" height="1" /></td>
        <td width="100%">
            <table width="100%" cellpadding="2" cellspacing="2" border="0">
                <tr>
                    <td rowspan="2"><img src="/images/step_1.gif" border="0" align="absmiddle" /></td>
                    <td class="bigblue" width="100%" valign="bottom">Option 1: Start the Build Now</td>
                </tr>
                <tr>
                    <td valign="top">Click <b>Start the Build</b> to start your auto-provisioning build.</td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td><asp:Button ID="btnStart" runat="server" OnClick="btnStart_Click" Text="Start the Build" CssClass="default" Width="125" /></td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td> <asp:Label ID="lblNotify" runat="server" CssClass="reddefault" Text="<b>BURN-IN PROCESS:</b> Upon clicking this button, the design implementor will be notified of your request." Visible="false" /></td>
                </tr>
            </table>
            <asp:Panel ID="panSchedule" runat="server" Visible="false">
            <br /><br />
            <table width="100%" cellpadding="2" cellspacing="2" border="0">
                <tr>
                    <td rowspan="2"><img src="/images/step_2.gif" border="0" align="absmiddle" /></td>
                    <td class="bigblue" width="100%" valign="bottom">Option 2: Schedule the Build</td>
                </tr>
                <tr>
                    <td valign="top">Enter the date and time on which you want the device built and click <b>Schedule the Build</b> to have ClearView build the device for you.</td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td><asp:TextBox ID="txtScheduleDate" runat="server" CssClass="default" Width="100" MaxLength="10" /> <asp:ImageButton ID="imgScheduleDate" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calendar.gif" />&nbsp;&nbsp;at&nbsp;&nbsp;<asp:TextBox ID="txtScheduleTime" runat="server" CssClass="default" Width="75" MaxLength="8" />&nbsp;&nbsp;<span class="footer">Example: 12:15 AM</span></td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td><asp:Button ID="btnSchedule" runat="server" OnClick="btnSchedule_Click" Text="Schedule the Build" CssClass="default" Width="125" /></td>
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
<asp:Label ID="lblVMware" runat="server" Visible="false" />