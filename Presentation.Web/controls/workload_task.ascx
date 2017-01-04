<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="workload_task.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.workload_task" %>


<script type="text/javascript">
    function ShowHideAvailable(oDiv) {
        oDiv = document.getElementById(oDiv);
        if (oDiv.style.display == "inline")
            oDiv.style.display = "none";
        else
            oDiv.style.display = "inline";
        return false;
    }
</script>
<table width="100%" cellpadding="0" cellspacing="0" border="0" class="default">
    <tr>
        <td><img src="/images/table_topleft.gif" border="0" width="5" height="26"></td>
        <td nowrap background="/images/table_top.gif" class="greentableheader" width="100%"><asp:Label ID="lblTitle" runat="server" CssClass="greentableheader" /></td>
        <td><img src="/images/table_topright.gif" border="0" width="5" height="26"></td>
    </tr>
    <tr>
        <td background="/images/table_left.gif"><img src="/images/table_left.gif" width="5" height="10"></td>
        <td width="100%" bgcolor="#FFFFFF">
            <table width="100%" cellpadding="0" cellspacing="5" border="0">
                <tr>
                    <td rowspan="2"><img src="/images/workload48.gif" border="0" align="absmiddle" /></td>
                    <td class="hugeheader" width="100%" valign="bottom">Create a Task</td>
                </tr>
                <tr>
                    <td width="100%" valign="top">Tasks are used to generate and assign work to a technician in your department.</td>
                </tr>
            </table>
            <br />
            <table width="100%" cellpadding="0" cellspacing="5" border="0">
                <tr>
                    <td>
                        <table width="100%" cellpadding="0" cellspacing="0" border="0">
                            <tr>
                                <td colspan="3" bgcolor="#007253" class="whitedefault"><div style="padding:5"><b>Task Information</b></div></td>
                            </tr>
                            <tr>
                                <td colspan="3" style="border-left:solid 1px #999999;border-right:solid 1px #999999;border-top:solid 1px #999999;">
                                    <asp:Panel ID="panTask" runat="server" Visible="false">
                                    <table width="100%" cellpadding="4" cellspacing="3" border="0" class="default">
	                                    <tr>
		                                    <td colspan="2" align="center"><img src="/images/alert.gif" border="0" align="absmiddle" /> Tasks should not be related to projects. For project related work, please use <b>Service Request</b>.</td>
	                                    </tr>
	                                    <tr>
		                                    <td nowrap>Task Name:<font class="required">&nbsp;*</font></td>
		                                    <td width="100%"><asp:TextBox ID="txtName" runat="server" CssClass="default" Width="90%" MaxLength="100" /></td>
	                                    </tr>
	                                    <tr>
		                                    <td nowrap valign="top">Statement of Work:<font class="required">&nbsp;*</font></td>
		                                    <td width="100%"><asp:TextBox ID="txtDescription" runat="server" CssClass="default" Width="90%" Rows="10" TextMode="MultiLine" /></td>
	                                    </tr>
	                                    <tr>
		                                    <td nowrap>Hours Allocated:<font class="required">&nbsp;*</font></td>
		                                    <td width="100%"><asp:TextBox ID="txtHours" runat="server" CssClass="default" Width="75" MaxLength="8" /></td>
	                                    </tr>
                                        <tr>
                                            <td nowrap>Start Date:<font class="required">&nbsp;*</font></td>
                                            <td width="100%"><asp:TextBox ID="txtStart" runat="server" CssClass="default" Width="100" MaxLength="10" /> <asp:ImageButton ID="imgStart" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calendar.gif" /></td>
                                        </tr>
                                        <tr>
                                            <td nowrap>End Date:<font class="required">&nbsp;*</font></td>
                                            <td width="100%"><asp:TextBox ID="txtEnd" runat="server" CssClass="default" Width="100" MaxLength="10" /> <asp:ImageButton ID="imgEnd" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calendar.gif" /></td>
                                        </tr>
                                        <tr>
                                            <td nowrap>Resource:<font class="required">&nbsp;*</font></td>
                                            <td width="100%"><asp:DropDownList ID="ddlUser" runat="server" CssClass="default" />&nbsp;&nbsp;&nbsp;[<asp:LinkButton ID="lnkAvailable" runat="server" Text="View Availability" />]</td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                            <div id="divAvailable" runat="server" style="display:none">
                                                <br />
                                                <table width="100%" cellpadding="2" cellspacing="0" border="0" style="border:solid 1px #999999" bgcolor="#fefefe" class="default">
                                                    <tr>
                                                        <td colspan="2"><b>Resource Availability</b></td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">The following graph represents the amount of allocated hours unused.</td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">&nbsp;</td>
                                                    </tr>
                                                <asp:repeater ID="rptAvailable" runat="server">
                                                    <ItemTemplate>
                                                        <tr>
                                                            <td><%# DataBinder.Eval(Container.DataItem, "name").ToString() %></td>
                                                            <td><img src="/images/table_top.gif" width='<%# DataBinder.Eval(Container.DataItem, "graph").ToString() %>' height="16" />&nbsp;<b><%# Double.Parse(DataBinder.Eval(Container.DataItem, "hours").ToString()).ToString("F") %></b></td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2" height="1"><img src="/images/spacer.gif" border="0" width="1" height="1" /></td>
                                                        </tr>
                                                    </ItemTemplate>
                                                </asp:repeater>
                                                </table>
                                            </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2"><hr size="1" noshade /></td>
                                        </tr>
                                        <tr>
                                            <td nowrap class="required">* = Required Field</td>
                                            <td align="right"><asp:Button ID="btnSubmit" runat="server" CssClass="default" Width="75" Text="Save" OnClick="btnSubmit_Click" /></td>
                                        </tr>
                                    </table>
                                    </asp:Panel>
                                    <asp:Panel ID="panFinish" runat="server" Visible="false">
                                        <table width="100%" cellpadding="2" cellspacing="2" border="0">
                                            <tr>
                                                <td><img src="/images/spacer.gif" border="0" width="15" height="1" /></td>
                                                <td class="header"><img src="/images/bigCheck.gif" border="0" align="absmiddle" /> Task Submitted</td>
                                            </tr>
                                            <tr><td colspan="2">&nbsp;</td></tr>
                                            <tr>
                                                <td><img src="/images/spacer.gif" border="0" width="15" height="1" /></td>
                                                <td>Your information has been saved and the task has been assigned.</td>
                                            </tr>
                                            <tr><td colspan="2">&nbsp;</td></tr>
                                        </table>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td><img src="/images/shadow_box_l.gif" border="0" width="5" height="12"></td>
                                <td width="100%" background="/images/shadow_box.gif"></td>
                                <td><img src="/images/shadow_box_r.gif" border="0" width="5" height="12"></td>
                            </tr>
                            <tr>
                                <td colspan="3">&nbsp;</td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </td>
        <td background="/images/table_right.gif"><img src="/images/table_right.gif" width="5" height="10"></td>
    </tr>
    <tr>
        <td><img src="/images/table_bottomLeft.gif" border="0" width="5" height="9"></td>
        <td width="100%" background="/images/table_bottom.gif"></td>
        <td><img src="/images/table_bottomRight.gif" border="0" width="5" height="9"></td>
    </tr>
</table>
<asp:Label ID="lblRequest" runat="server" Visible="false" />
