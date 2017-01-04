<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wm_server_decommission.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.wm_server_decommission" %>

<script type="text/javascript">
    function ChangeFrame(oCell, oShow, oHide1, oHide2, oHide3, oHidden, strHidden) {
        oShow = document.getElementById(oShow);
        oShow.style.display = "inline";
        oHide1 = document.getElementById(oHide1);
        oHide1.style.display = "none";
        oHide2 = document.getElementById(oHide2);
        oHide2.style.display = "none";
        oHide3 = document.getElementById(oHide3);
        oHide3.style.display = "none";
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
</script>
<asp:Panel ID="panWorkload" runat="server" Visible="false">
    <table width="100%" border="0" cellSpacing="0" cellPadding="0" class="default">
        <tr>
            <td>
                <table width="100%" border="0" cellSpacing="0" cellPadding="0">
                    <tr>
                        <td><asp:ImageButton ID="btnSave" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/tool_save.gif" OnClick="btnSave_Click" /></td>
                        <td><asp:ImageButton ID="btnPrint" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/tool_print.gif" /></td>
                        <td><asp:ImageButton ID="btnEmail" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/tool_email.gif" /></td>
                        <td><asp:ImageButton ID="btnSLA" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/tool_sla.gif" /></td>
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
                    <tr>
                        <td colspan="2">
                            <table border="0" cellpadding="3" cellspacing="2">
                                <tr>
                                    <td><b>Original Request Details:</b>&nbsp;&nbsp;<asp:Label ID="lblDescription" runat="server" CssClass="default" /></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr height="16">
            <td><img src="/images/spacer.gif" border="0" height="16" width="1" /></td>
        </tr>
        <tr>
            <td>
                    <table width="100%" height="100%" border="0" cellSpacing="0" cellPadding="0" class="default">
                        <tr>
                            <td class="cmbutton" style='<%=boolDetails == true ? "display:inline;border-bottom:1px solid #FFFFFF;border-top:3px solid orange;" : "" %>' onclick="ChangeFrame(this,'divDetails','divExecution','divChange','divDocuments','<%=hdnTab.ClientID %>','D');">Request Details</td>
                            <td class="cmbuttonspace">&nbsp;</td>
                            <td class="cmbutton" style='<%=boolExecution == true ? "display:inline;border-bottom:1px solid #FFFFFF;border-top:3px solid orange;" : "" %>' onclick="ChangeFrame(this,'divExecution','divDetails','divChange','divDocuments','<%=hdnTab.ClientID %>','E');">Execution</td>
                            <td class="cmbuttonspace">&nbsp;</td>
                            <td class="cmbutton" style='<%=boolChange == true ? "display:inline;border-bottom:1px solid #FFFFFF;border-top:3px solid orange;" : "" %>' onclick="ChangeFrame(this,'divChange','divDetails','divExecution','divDocuments','<%=hdnTab.ClientID %>','C');">Change Controls</td>
                            <td class="cmbuttonspace">&nbsp;</td>
                            <td class="cmbutton" style='<%=boolDocuments == true ? "display:inline;border-bottom:1px solid #FFFFFF;border-top:3px solid orange;" : "" %>' onclick="ChangeFrame(this,'divDocuments','divDetails','divExecution','divChange','<%=hdnTab.ClientID %>','D');">Attached Files</td>
                        </tr>
                        <tr>
                            <td colspan="7" align="center" class="cmcontents">
                                <table width="100%" height="100%" border="0" cellSpacing="2" cellPadding="2" class="default">
                                    <tr> 
                                        <td valign="top">
		                                    <div id="divDetails" style='<%=boolDetails == true ? "display:inline" : "display:none" %>'>
		                                        <br />
                                                <table width="100%" cellpadding="4" cellspacing="3" border="0">
                                                    <tr>
                                                        <td class="greentableheader">Request Details</td>
                                                    </tr>
                                                    <tr>
                                                        <td><asp:Label ID="lblView" runat="server" CssClass="default" /></td>
                                                    </tr>
                                                </table>
                                                <p>&nbsp;</p>
		                                    </div>
		                                    <div id="divExecution" style='<%=boolExecution == true ? "display:inline" : "display:none" %>'>
		                                        <br />
                                                <table width="100%" cellpadding="4" cellspacing="3" border="0">
                                                    <tr>
                                                        <td colspan="2" class="greentableheader">Required Tasks</td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <table cellpadding="2" cellspacing="1" border="0">
                                                                <asp:Panel ID="panBlackout" runat="server" Visible="false">
                                                                <tr>
                                                                    <td><asp:CheckBox ID="chkBlackout" runat="server" CssClass="default" Text="Add server to blackout" /></td>
                                                                </tr>
                                                                </asp:Panel>
                                                                <asp:Panel ID="panBlackoutNO" runat="server" Visible="false">
                                                                <tr>
                                                                    <td><img src="/images/cancel.gif" border="0" align="absmiddle" /> <asp:Label ID="lblBlackout" runat="server" CssClass="default" Enabled="false" Text="Add server to blackout" /></td>
                                                                </tr>
                                                                </asp:Panel>
                                                                <tr>
                                                                    <td><asp:CheckBox ID="chkPower" runat="server" CssClass="default" Text="Power off server" /></td>
                                                                </tr>
                                                                <asp:Panel ID="panRename" runat="server" Visible="false">
                                                                <tr>
                                                                    <td><asp:CheckBox ID="chkRename" runat="server" CssClass="default" Text="Rename VMware guest to " /></td>
                                                                </tr>
                                                                </asp:Panel>
                                                                <tr>
                                                                    <td>&nbsp;</td>
                                                                </tr>
                                                                <asp:Panel ID="panWaiting" runat="server" Visible="false">
                                                                <tr>
                                                                    <td>
                                                                        <table width="100%" cellpadding="0" cellspacing="5" border="0" style="border:solid 1px #999999" bgcolor="#f9f9f9">
                                                                            <tr>
                                                                                <td rowspan="2"><img src="/images/ico_hourglass40.gif" border="0" align="absmiddle" /></td>
                                                                                <td class="header" width="100%" valign="bottom">Waiting for Seven (7) Days</td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td width="100%" valign="top">Before you can continue, at least seven (7) days must pass from the date you powered off the device.<br />Seven days will be reached on <asp:Label ID="lblWaiting" runat="server" CssClass="default" /></td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                                </asp:Panel>
                                                                <asp:Panel ID="panWaitingNO" runat="server" Visible="false">
                                                                <tr>
                                                                    <td>
                                                                        <table width="100%" cellpadding="0" cellspacing="5" border="0" style="border:solid 1px #999999" bgcolor="#f9f9f9">
                                                                            <tr>
                                                                                <td rowspan="2"><img src="/images/ico_check40.gif" border="0" align="absmiddle" /></td>
                                                                                <td class="header" width="100%" valign="bottom">Seven (7) Days have Expired</td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td width="100%" valign="top">You can now finished decommissioning this device by completing the information below...</td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>&nbsp;</td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <table width="100%" cellpadding="0" cellspacing="5" border="0">
                                                                            <tr>
                                                                                <td colspan="2">Is this device attached to SAN?</td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td><img src="/images/spacer.gif" border="0" width="50" height="1" /></td>
                                                                                <td width="100%">
                                                                                    <asp:RadioButton ID="radSANYes" runat="server" CssClass="default" Text="Yes - this device is attached to SAN" GroupName="SAN" /><br />
                                                                                    <asp:RadioButton ID="radSANNo" runat="server" CssClass="default" Text="No - this device is NOT attached to SAN" GroupName="SAN" /><br />
                                                                                   <%-- <asp:RadioButton ID="radSANDK" runat="server" CssClass="default" Text="??? - I do not know" GroupName="SAN" />--%>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td colspan="2">&nbsp;</td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td colspan="2">Is this device load balanced via CSM?</td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td><img src="/images/spacer.gif" border="0" width="50" height="1" /></td>
                                                                                <td width="100%">
                                                                                    <asp:RadioButton ID="radCSMYes" runat="server" CssClass="default" Text="Yes - this device is load balanced via CSM" GroupName="CSM" /><br />
                                                                                    <asp:RadioButton ID="radCSMNo" runat="server" CssClass="default" Text="No - this device is NOT load balanced via CSM" GroupName="CSM" /><br />
                                                                                    <%--<asp:RadioButton ID="radCSMDK" runat="server" CssClass="default" Text="??? - I do not know" GroupName="CSM" />--%>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td colspan="2">&nbsp;</td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td colspan="2">What is the <b>build</b> IP Address of this device?</td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td><img src="/images/spacer.gif" border="0" width="50" height="1" /></td>
                                                                                <td width="100%"><asp:TextBox ID="txtIPBuild1" CssClass="default" runat="server" Width="50" MaxLength="3"/> . <asp:TextBox ID="txtIPBuild2" CssClass="default" runat="server" Width="50" MaxLength="3"/> . <asp:TextBox ID="txtIPBuild3" CssClass="default" runat="server" Width="50" MaxLength="3"/> . <asp:TextBox ID="txtIPBuild4" CssClass="default" runat="server" Width="50" MaxLength="3"/></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td><img src="/images/spacer.gif" border="0" width="50" height="1" /></td>
                                                                                <td width="100%" class="footer"><b>NOTE:</b> The build IP address is the IP address that was originally auto-assigned at the time of the build.</td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td><img src="/images/spacer.gif" border="0" width="50" height="1" /></td>
                                                                                <td width="100%" class="footer"><b>NOTE:</b> If there is no IP address, set this field to 0.0.0.0</td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td colspan="2">&nbsp;</td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td colspan="2">What is the <b>final</b> IP Address of this device?</td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td><img src="/images/spacer.gif" border="0" width="50" height="1" /></td>
                                                                                <td width="100%"><asp:TextBox ID="txtIPFinal1" CssClass="default" runat="server" Width="50" MaxLength="3"/> . <asp:TextBox ID="txtIPFinal2" CssClass="default" runat="server" Width="50" MaxLength="3"/> . <asp:TextBox ID="txtIPFinal3" CssClass="default" runat="server" Width="50" MaxLength="3"/> . <asp:TextBox ID="txtIPFinal4" CssClass="default" runat="server" Width="50" MaxLength="3"/></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td><img src="/images/spacer.gif" border="0" width="50" height="1" /></td>
                                                                                <td width="100%" class="footer"><b>NOTE:</b> The final IP address is the IP address that was assigned when the device was moved into its final location.</td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td><img src="/images/spacer.gif" border="0" width="50" height="1" /></td>
                                                                                <td width="100%" class="footer"><b>NOTE:</b> If there is no IP address, set this field to 0.0.0.0</td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td colspan="2"><asp:CheckBox ID="chkDecom" runat="server" Text="Mark server as Decommissioned in ClearView" /></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td colspan="2"><asp:CheckBox ID="chkDispose" runat="server" Text="Dispose" /></td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                                <asp:Panel ID="panDestroy" runat="server" Visible="false">
                                                                <tr>
                                                                    <td><asp:CheckBox ID="chkDestroy" runat="server" CssClass="default" Text="Destroy VMware guest" /></td>
                                                                </tr>
                                                                </asp:Panel>
                                                                </asp:Panel>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">&nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2" class="greentableheader">Status Updates</td>
                                                    </tr>
                                                    <tr>
                                                        <td nowrap>Status:</td>
                                                        <td width="100%">
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
                                                                </tr>
                                                                <asp:repeater ID="rptStatus" runat="server">
                                                                    <ItemTemplate>
                                                                        <tr class="default">
                                                                            <td nowrap><a href="javascript:void(0);" onclick="ShowStatus('<%# DataBinder.Eval(Container.DataItem, "id") %>','r');"><%# DateTime.Parse(DataBinder.Eval(Container.DataItem, "modified").ToString()).ToShortDateString() %>&nbsp;<%# DateTime.Parse(DataBinder.Eval(Container.DataItem, "modified").ToString()).ToShortTimeString() %></a></td>
                                                                            <td nowrap>&nbsp;</td>
                                                                            <td width="100%"><asp:Label ID="lblStatus" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "status") %>' /></td>
                                                                        </tr>
                                                                    </ItemTemplate>
                                                                </asp:repeater>
                                                            <tr>
                                                                <td colspan="3">
                                                                    <asp:Label ID="lblNoStatus" runat="server" CssClass="default" Visible="false" Text="<img src='/images/alert.gif' border='0' align='absmiddle'> There are no updates" />
                                                                </td>
                                                            </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
		                                    </div>
		                                    <div id="divChange" style='<%=boolChange == true ? "display:inline" : "display:none" %>'>
		                                        <br />
                                                <table width="100%" cellpadding="4" cellspacing="3" border="0">
                                                    <tr>
                                                        <td colspan="2" class="greentableheader">Change Controls</td>
                                                    </tr>
                                                    <tr>
                                                        <td nowrap>Number:</td>
                                                        <td width="100%"><asp:TextBox ID="txtNumber" runat="server" CssClass="default" Width="150" MaxLength="15" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td nowrap>Date:</td>
                                                        <td width="100%"><asp:TextBox ID="txtDate" runat="server" CssClass="default" Width="100" MaxLength="10" /> <asp:ImageButton ID="imgDate" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calendar.gif" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td nowrap>Time:</td>
                                                        <td width="100%"><asp:TextBox ID="txtTime" runat="server" CssClass="default" Width="100" MaxLength="10" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2"><b>Comments:</b></td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2"><asp:TextBox ID="txtChange" runat="server" CssClass="default" Width="500" TextMode="MultiLine" Rows="7" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2"><asp:Button ID="btnChange" runat="server" CssClass="default" Width="150" Text="Submit Change" OnClick="btnChange_Click" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">&nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <table width="100%" cellpadding="3" cellspacing="0" border="0" style="border:solid 1px #CCCCCC">
                                                                <tr bgcolor="#EEEEEE">
                                                                    <td nowrap><b>Number</b></td>
                                                                    <td nowrap><b>Date</b></td>
                                                                    <td nowrap>&nbsp;</td>
                                                                </tr>
                                                                <asp:repeater ID="rptChange" runat="server">
                                                                    <ItemTemplate>
                                                                        <tr class="default">
                                                                            <td nowrap><a href="javascript:void(0);" onclick="ShowChange('<%# DataBinder.Eval(Container.DataItem, "changeid") %>');"><%# DataBinder.Eval(Container.DataItem, "number") %></a></td>
                                                                            <td nowrap><%# DateTime.Parse(DataBinder.Eval(Container.DataItem, "implementation").ToString()).ToLongDateString() %> @ <%# DateTime.Parse(DataBinder.Eval(Container.DataItem, "implementation").ToString()).ToLongTimeString() %></td>
                                                                            <td align="right">[<asp:LinkButton ID="btnDeleteChange" runat="server" Text="Delete" OnClick="btnDeleteChange_Click" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "changeid") %>' />]</td>
                                                                        </tr>
                                                                    </ItemTemplate>
                                                                </asp:repeater>
                                                            <tr>
                                                                <td colspan="3">
                                                                    <asp:Label ID="lblNoChange" runat="server" CssClass="default" Visible="false" Text="<img src='/images/alert.gif' border='0' align='absmiddle'> There are no change controls" />
                                                                </td>
                                                            </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
		                                    </div>
		                                    <div id="divDocuments" style='<%=boolDocuments == true ? "display:inline" : "display:none" %>'>
		                                        <br />
                                                <table width="100%" cellpadding="4" cellspacing="3" border="0">
                                                    <tr>
                                                        <td class="greentableheader">Attached Files</td>
                                                        <td align="right"><asp:CheckBox ID="chkDescription" runat="server" CssClass="default" Text="Show Descriptions" AutoPostBack="true" OnCheckedChanged="chkDescription_Change" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2"><asp:Label ID="lblDocuments" runat="server" CssClass="default" /></td>
                                                    </tr>
                                                </table>
		                                    </div>
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
<asp:HiddenField ID="hdnTab" runat="server" />