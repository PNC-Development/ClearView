<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="resource_request_assign_NEW.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.resource_request_assign_NEW" %>


<script type="text/javascript">
    function ShowHideAvailable(oDiv) {
        oDiv = document.getElementById(oDiv);
        if (oDiv.style.display == "inline")
            oDiv.style.display = "none";
        else
            oDiv.style.display = "inline";
        return false;
    }
    function ValidateChoice(oChoice, oAccept, oDeny, oHold, intCount) {
        oChoice = document.getElementById(oChoice);
        oAccept = document.getElementById(oAccept);
        oDeny = document.getElementById(oDeny);
        oHold = document.getElementById(oHold);
        if (oChoice.style.display == "none") {
            if (oHold.checked == true)
                return true;
            else {
                if (intCount == 0) {
                    alert('Please assign at least one resource');
                    return false;
                }
            }
        }
        if (oChoice.style.display == "inline") {
            if (oAccept.checked == false && oDeny.checked == false && oHold.checked == false) {
                alert('Please select if you want to accept, deny or put this request on hold');
                oAccept.focus();
                return false;
            }
            else {
                if (oAccept.checked == true) {
                    if (intCount == 0) {
                        alert('Please assign at least one resource');
                        return false;
                    }
                }
            }
        }
        return true;
    }
</script>
<table width="100%" cellpadding="0" cellspacing="0" border="0" class="default">
    <tr>
        <td><img src="/images/table_topleft.gif" border="0" width="5" height="26"></td>
        <td nowrap background="/images/table_top.gif" class="greentableheader" width="100%">Assign Resource Request # <asp:Label ID="lblResourceParent" runat="server" CssClass="greentableheader" /></td>
        <td><img src="/images/table_topright.gif" border="0" width="5" height="26"></td>
    </tr>
    <tr>
        <td background="/images/table_left.gif"><img src="/images/table_left.gif" width="5" height="10"></td>
        <td width="100%" bgcolor="#FFFFFF">
            <br />
            <asp:Panel ID="panRequest" runat="server" Visible="false">
                <%=strSummary %>
                <table width="100%" border="0" cellSpacing="2" cellPadding="2" class="default">
                    <tr>
                        <td colspan="2">&nbsp;</td>
                    </tr>
                    <tr>
                        <td colspan="2"><hr size="1" noshade /></td>
                    </tr>
                    <tr>
                        <td nowrap><b>Request Status:</b></td>
                        <td width="100%"><asp:Label ID="lblStatus" runat="server" CssClass="default" /></td>
                    </tr>
                    <tr>
                        <td nowrap><b>Pending Assignment By:</b></td>
                        <td width="100%"><asp:Label ID="lblAssignBy" runat="server" CssClass="default" /></td>
                    </tr>
                    <tr>
                        <td nowrap><b>Group Assigned:</b></td>
                        <td width="100%"><asp:Label ID="lblGroup" runat="server" CssClass="default" /></td>
                    </tr>
                    <tr>
                        <td nowrap><b>Service Description:</b></td>
                        <td width="100%"><asp:Label ID="lblService" runat="server" CssClass="default" /> (<asp:Label ID="lblType" runat="server" CssClass="default" />)</td>
                    </tr>
                    <tr>
                        <td nowrap><b>Quantity:</b></td>
                        <td width="100%"><asp:TextBox ID="txtQuantityOverall" runat="server" CssClass="default" Width="50" MaxLength="4" /> <asp:Button ID="btnUpdate" runat="server" CssClass="default" Width="125" OnClick="btnUpdate_Click" Text="Update Quantity" /></td>
                    </tr>
                    <tr>
                        <td nowrap><b>Hours Allocated:</b></td>
                        <td width="100%"><asp:Label ID="lblHoursOverall" runat="server" CssClass="default" /></td>
                    </tr>
                    <tr>
                        <td colspan="2"><br/><asp:LinkButton ID="btnView" runat="server" Text="Click here to view this request" /></td>
                    </tr>
                    <tr>
                        <td colspan="2">&nbsp;</td>
                    </tr>
                    <asp:Panel ID="panDelete" runat="server" Visible="false">
                    <tr>
                        <td colspan="2"><asp:Button ID="btnDelete" runat="server" CssClass="default" Text="Delete" OnClick="btnDelete_Click" Width="100" /></td>
                    </tr>
                    <tr>
                        <td colspan="2">&nbsp;</td>
                    </tr>
                    </asp:Panel>
                    <tr>
                        <td colspan="2">
                            <asp:Panel ID="panAssignMultiple" runat="Server" Visible="false">
                            <table width="100%" cellpadding="3" cellspacing="2" border="0" style="border:solid 1px #999999" bgcolor="#f9f9f9" >
                                <tr>
                                    <td>
                                        <div id="divChoice" runat="server" style="display:none">
                                            <table width="100%" cellpadding="3" cellspacing="2" border="0">
                                                <tr>
                                                    <td class="bigred">Accept Request</td>
                                                </tr>
                                                <tr>
                                                    <td class="error">You have the choice of accepting or rejecting this request. Do you want to accept this request?</td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:RadioButton ID="radAccept" runat="server" CssClass="default" Text="Yes" GroupName="Accept" />
                                                        <asp:RadioButton ID="radReject" runat="server" CssClass="default" Text="No" GroupName="Accept" />
                                                        <asp:RadioButton ID="radHold" runat="server" CssClass="default" Text="Put on Hold" GroupName="Accept" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td><hr size="1" noshade /></td>
                                                </tr>
                                            </table>
                                        </div>
                                        <div id="divHold" runat="server" style="display:none">
                                            <table width="100%" cellpadding="3" cellspacing="2" border="0" class="default">
                                                <tr>
                                                    <td class="bigger"><b>Optional Comments:</b></td>
                                                </tr>
                                                <tr>
                                                    <td><asp:TextBox ID="txtHold" runat="server" CssClass="default" Width="400" TextMode="MultiLine" Rows="7" /></td>
                                                </tr>
                                            </table>
                                        </div>
                                        <div id="divReject" runat="server" style="display:none">
                                            <table width="100%" cellpadding="3" cellspacing="2" border="0" class="default">
                                                <tr>
                                                    <td class="bigger"><b>Optional Comments:</b> <span class="help">(Will be displayed to the submitter)</span></td>
                                                </tr>
                                                <tr>
                                                    <td><asp:TextBox ID="txtComments" runat="server" CssClass="default" Width="400" TextMode="MultiLine" Rows="7" /></td>
                                                </tr>
                                            </table>
                                        </div>
                                        <div id="divAccept" runat="server" style="display:none">
                                            <table width="100%" cellpadding="3" cellspacing="2" border="0">
                                                <tr>
                                                    <td colspan="2" class="bigger"><b>Assign Resources:</b>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;[<asp:LinkButton ID="lnkAvailable" runat="server" Text="View Resource Availability" />]&nbsp;&nbsp;[<asp:LinkButton ID="btnAssignments" runat="server" Text="View Resources Already Assigned" />]</td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <table cellpadding="4" cellspacing="0" border="0">
                                                            <tr>
                                                                <td>Resource:</td>
                                                                <td><asp:DropDownList ID="ddlUser" runat="server" CssClass="default" />
                                                                <td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
                                                                <td>Quantity:</td>
                                                                <td><asp:TextBox ID="txtQuantity" runat="server" CssClass="default" Width="50" MaxLength="4" /></td>
                                                                <td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
                                                                <td>Hours:</td>
                                                                <td>
                                                                    <asp:TextBox ID="txtHours" runat="server" CssClass="default" Width="75" MaxLength="8" Visible="false" />
                                                                    <asp:Label ID="lblHours" runat="server" CssClass="default" Visible="false" /> <asp:LinkButton ID="btnWhy" runat="server" Text="[More Information]" Visible="false" />
                                                                </td>
                                                                <td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
                                                                <td><asp:Button ID="btnAssign" runat="server" CssClass="default" Width="75" Text="Assign" OnClick="btnAssign_Click" /></td>
                                                            </tr>
                                                            <tr id="divWhy" runat="server" style="display:none">
                                                                <td colspan="2"></td>
                                                                <td colspan="8">The hours are disabled because you have configured multiple<br />actions to be associated with this service. Each action has a<br /> time associated to it, so you cannot modify the hours allocated<br /> for this service....just the quantity.</td>
                                                            </tr>
                                                            </asp:Panel>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">&nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <table width="750" cellpadding="4" cellspacing="0" border="0" style="border:solid 1px #CCCCCC">
                                                            <tr bgcolor="#EEEEEE">
                                                                <td width="50%" nowrap><b>Resource</b></td>
                                                                <td width="25%" nowrap><b>Devices</b></td>
                                                                <td width="25%" nowrap><b>Allocated</b></td>
                                                                <td nowrap align="center"></td>
                                                            </tr>
                                                        <asp:repeater ID="rptAssign" runat="server">
                                                            <ItemTemplate>
                                                                <tr>
                                                                    <td width="50%" nowrap><%# oUser.GetFullName(Int32.Parse(DataBinder.Eval(Container.DataItem, "userid").ToString())) + " (" + oUser.GetName(Int32.Parse(DataBinder.Eval(Container.DataItem, "userid").ToString())) + ")"%></td>
                                                                    <td width="25%" nowrap><%# DataBinder.Eval(Container.DataItem, "devices") %></td>
                                                                    <td width="25%" nowrap><%# DataBinder.Eval(Container.DataItem, "allocated") %> HRs</td>
                                                                    <td nowrap align="center">[<asp:LinkButton ID="btnDeleteAssign" runat="server" Text="Delete" OnClick="btnDeleteAssign_Click" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' />]</td>
                                                                </tr>
                                                            </ItemTemplate>
                                                        </asp:repeater>
                                                            <tr>
                                                                <td colspan="6"><asp:Label ID="lblAssign" runat="server" CssClass="error" Visible="false" Text="<img src='/images/bigAlert.gif' border='0' align='absmiddle'> There are no resources assigned to this request" /></td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">&nbsp;</td>
                                                </tr>
                                                <asp:Panel ID="panChecks" runat="server" Visible="false">
                                                <tr>
                                                    <td colspan="2"><asp:CheckBox ID="chkOverall" runat="server" CssClass="default" Text="Request Survey from Project Coordinator regarding the Overall Service" /> <span class="note">*** Currently disabled</span></td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2"><asp:CheckBox ID="chkTechnician" runat="server" CssClass="default" Text="Request Survey from Project Coordinator regarding the Resource" /> <span class="note">*** Currently disabled</span></td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2"><asp:CheckBox ID="chkNotify" runat="server" CssClass="default" Text="Notify the Project Coordinator to fill out Clarity time" /> <span class="note">*** Currently disabled</span></td>
                                                </tr>
                                                </asp:Panel>
                                            </table>
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
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            </asp:Panel>
                            <asp:Panel ID="panAssignSingle" runat="Server" Visible="false">
                            <table width="100%" cellpadding="3" cellspacing="2" border="0">
                                <tr>
                                    <td nowrap><b>Resource:</b></td>
                                    <td width="100%">
                                        <table cellpadding="0" cellspacing="0" border="0">
                                            <tr>
                                                <td><asp:TextBox ID="txtManager" runat="server" Width="300" CssClass="default" /></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <div id="divManager" runat="server" style="overflow:hidden; position:absolute; display:none; background-color:#FFFFFF; border:solid 1px #CCCCCC">
                                                        <asp:ListBox ID="lstManager" runat="server" CssClass="default" />
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td nowrap></td>
                                    <td class="footer" width="100%">(Please enter a valid LAN ID, First Name, or Last Name)</td>
                                </tr>
                            </table>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">&nbsp;</td>
                    </tr>
                    <tr>
                        <td colspan="2"><hr size="1" noshade /></td>
                    </tr>
                    <tr>
                        <td colspan="2" align="left" class="bigalert"><asp:Label ID="lblAssigned" runat="server" Visible="false" Text="<img src='/images/bigAlert.gif' border='0' align='absmiddle'> Resource already assigned" />
                    </tr>
                    <tr>
                        <td colspan="2" align="right"><asp:Button ID="btnSubmit" runat="server" CssClass="default" Width="75" Text="Submit" OnClick="btnSubmit_Click" /></td>
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
                                    <td align="right"><asp:Button ID="btnClose" runat="server" CssClass="default" Width="75" Text="Close" /></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="panFinish" runat="server" Visible="false">
                <table width="100%" cellpadding="2" cellspacing="2" border="0">
                    <tr>
                        <td><img src="/images/spacer.gif" border="0" width="15" height="1" /></td>
                        <td class="header"><img src="/images/bigCheck.gif" border="0" align="absmiddle" /> Record Updated</td>
                    </tr>
                    <tr><td colspan="2">&nbsp;</td></tr>
                    <tr>
                        <td><img src="/images/spacer.gif" border="0" width="15" height="1" /></td>
                        <td>Your information has been saved successfully.</td>
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
                                    <td align="right"><asp:Button ID="btnFinish" runat="server" CssClass="default" Width="75" Text="Finish" OnClick="btnFinish_Click" /></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <p>&nbsp;</p>
        </td>
        <td background="/images/table_right.gif"><img src="/images/table_right.gif" width="5" height="10"></td>
    </tr>
    <tr>
        <td><img src="/images/table_bottomLeft.gif" border="0" width="5" height="9"></td>
        <td width="100%" background="/images/table_bottom.gif"></td>
        <td><img src="/images/table_bottomRight.gif" border="0" width="5" height="9"></td>
    </tr>
</table>
<asp:HiddenField ID="hdnManager" runat="server" />
<asp:Label ID="lblItem" runat="server" Visible="false" />
<asp:Label ID="lblServiceID" runat="server" Visible="false" />
