<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="resource_request_new.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.resource_request_new" %>


<script type="text/javascript">
function MoveList(ddlFrom, ddlTo, oHidden, ddlUpdate) {
    ddlFrom = document.getElementById(ddlFrom);
    ddlTo = document.getElementById(ddlTo);
    ddlUpdate = document.getElementById(ddlUpdate);
	if (ddlFrom.selectedIndex > -1) {
		var oOption = document.createElement("OPTION");
		ddlTo.add(oOption);
		oOption.text = ddlFrom.options[ddlFrom.selectedIndex].text;
		oOption.value = ddlFrom.options[ddlFrom.selectedIndex].value;
		ddlFrom.remove(ddlFrom.selectedIndex);
		ddlTo.selectedIndex = ddlTo.length - 1;
		UpdateHidden(oHidden, ddlUpdate);
	}
	return false;
}
function UpdateHidden(oHidden, oControl) {
	var oHidden = document.getElementById(oHidden);
	oHidden.value = "";
	for (var ii=0; ii<oControl.length; ii++) {
		oHidden.value = oHidden.value + oControl.options[ii].value + "_" + ii + "&";
	}
}
function ShowProjectInfo(oList) {
    OpenWindow('PROJECT_INFO', oList.options[oList.selectedIndex].value);
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
            <asp:Panel ID="panHeader" runat="server" Visible="false">
            <table width="100%" cellpadding="0" cellspacing="5" border="0">
                <tr>
                    <td rowspan="2"><img src="/images/service_request.gif" border="0" align="absmiddle" /></td>
                    <td class="header" width="100%" valign="bottom"><asp:Label ID="lblHeader" runat="server" CssClass="header" /></td>
                    <td rowspan="2" valign="bottom" nowrap>
                        <asp:Panel ID="panFavorite" runat="server" Visible="false">
                            <table width="300" cellpadding="3" cellspacing="2" border="0" style="border:solid 1px #999999" bgcolor="#f9f9f9">
                                <tr>
                                    <td rowspan="3"><img src="/images/favorite.gif" border="0" align="absmiddle" /></td>
                                    <td class="header" width="100%" valign="bottom">My Favorites</td>
                                </tr>
                                <tr>
                                    <td width="100%" valign="top"><asp:Label ID="lblFavorite" runat="server" CssClass="default" /></td>
                                </tr>
                                <tr>
                                    <td width="100%" valign="top">
                                        <asp:Button ID="btnFavoriteAdd" runat="server" CssClass="default" Width="125" Text="Add Favorite" OnClick="btnFavoriteAdd_Click" Visible="false" />
                                        <asp:Button ID="btnFavoriteDelete" runat="server" CssClass="default" Width="125" Text="Remove Favorite" OnClick="btnFavoriteDelete_Click" Visible="false" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td width="100%" valign="top"><asp:Label ID="lblHeaderSub" runat="server" CssClass="default" Text="Please complete the following information to request this service." /></td>
                </tr>
            </table>
            </asp:Panel>
            <asp:Panel ID="panTaskAccepted" runat="server" Visible="false">
            <table width="100%" cellpadding="0" cellspacing="5" border="0">
                <tr>
                    <td>
                        <table width="100%" border="0" cellSpacing="2" cellPadding="4" class="default">
                            <tr>
                                <td nowrap><b>Task Name:</b></td>
                                <td nowrap width="50%"><asp:Label ID="lblTaskName" runat="server" CssClass="default" /></td>
                                <td nowrap><b>Submitted By:</b></td>
                                <td nowrap width="50%"><asp:Label ID="lblTaskSubmitted" runat="server" CssClass="default" /></td>
                            </tr>
                            <tr>
                                <td nowrap valign="top"><b>Description:</b></td>
                                <td colspan="3"><asp:Label ID="lblTaskDescription" runat="server" CssClass="default" /></td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            </asp:Panel>
            <asp:Panel ID="panTaskRejected" runat="server" Visible="false">
            <table width="100%" cellpadding="0" cellspacing="5" border="0">
                <tr>
                    <td>
                        <table width="100%" cellpadding="0" cellspacing="0" border="0">
                            <tr>
                                <td colspan="3" bgcolor="#007253" class="whitedefault"><div style="padding:5"><b>Task Information</b></div></td>
                            </tr>
                            <tr>
                                <td colspan="3" style="border-left:solid 1px #999999;border-right:solid 1px #999999;border-top:solid 1px #999999;">
                                    <table width="100%" border="0" cellSpacing="2" cellPadding="4" class="default">
                                        <tr>
                                            <td nowrap><b>Task Name:</b></td>
                                            <td nowrap width="50%"><asp:TextBox ID="txtRejectTaskName" runat="server" CssClass="default" Width="300" MaxLength="100" /></td>
                                            <td nowrap><b>Submitted By:</b></td>
                                            <td nowrap width="50%"><asp:Label ID="lblRejectTaskSubmitted" runat="server" CssClass="default" /></td>
                                        </tr>
                                        <tr>
                                            <td nowrap valign="top"><b>Description:</b></td>
                                            <td colspan="3"><asp:TextBox ID="txtRejectDescription" runat="server" CssClass="default" Rows="7" Width="90%" TextMode="MultiLine" /></td>
                                        </tr>
                                        <tr>
                                            <td nowrap valign="top"><b>Reason for Rejection:</b></td>
                                            <td colspan="3"><asp:Label ID="lblRejectTaskReason" runat="server" CssClass="default" /></td>
                                        </tr>
                                        <tr>
                                            <td colspan="4"><hr size="1" noshade /></td>
                                        </tr>
                                        <tr>
                                            <td colspan="4" align="right"><asp:Button ID="btnRejectTask" runat="server" CssClass="default" Text="Update" Width="75" OnClick="btnRejectTask_Click" /></td>
                                        </tr>
                                    </table>
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
            </asp:Panel>
            <asp:Panel ID="panProjectAccepted" runat="server" Visible="false">
            <table width="100%" cellpadding="0" cellspacing="5" border="0">
                <tr>
                    <td>
                        <table width="100%" border="0" cellSpacing="2" cellPadding="4" class="default">
                            <tr>
                                <td nowrap><b>Project Name:</b></td>
                                <td nowrap width="50%"><asp:Label ID="lblProjectName" runat="server" CssClass="default" /></td>
                                <td nowrap><b>Project Number:</b></td>
                                <td nowrap width="50%"><asp:Label ID="lblProjectNumber" runat="server" CssClass="default" /></td>
                            </tr>
                            <tr>
                                <td nowrap><b>Organization:</b></td>
                                <td nowrap width="50%"><asp:Label ID="lblOrganization" runat="server" CssClass="default" /></td>
                                <td nowrap><b>Initiative Type:</b></td>
                                <td nowrap width="50%"><asp:Label ID="lblBaseDiscretionary" runat="server" CssClass="default" /></td>
                            </tr>
                            <tr>
                                <td nowrap><b>Segment:</b></td>
                                <td nowrap width="50%"><asp:Label ID="lblSegment" runat="server" CssClass="default" /></td>
                                <td nowrap><b>Submitted By:</b></td>
                                <td nowrap width="50%"><asp:Label ID="lblSubmitted" runat="server" CssClass="default" /></td>
                            </tr>
                            <tr>
                                <td nowrap><b>Project Status:</b></td>
                                <td nowrap width="50%"><asp:Label ID="lblStatus" runat="server" CssClass="default" /></td>
                                <td nowrap><b>Submitted On:</b></td>
                                <td nowrap width="50%"><asp:Label ID="lblSubmittedOn" runat="server" CssClass="default" /></td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            </asp:Panel>
            <asp:Panel ID="panProjectRejected" runat="server" Visible="false">
            <table width="100%" cellpadding="0" cellspacing="5" border="0">
                <tr>
                    <td>
                        <table width="100%" border="0" cellSpacing="2" cellPadding="4" class="default">
                            <tr>
                                <td nowrap><b>Project Name:</b></td>
                                <td nowrap width="50%"><asp:TextBox ID="txtRejectName" runat="server" CssClass="default" Width="300" MaxLength="35" /></td>
                                <td nowrap><b>Project Number:</b></td>
                                <td nowrap width="50%"><asp:TextBox ID="txtRejectNumber" runat="server" CssClass="default" Width="150" MaxLength="20" /></td>
                            </tr>
                            <tr>
                                <td nowrap><b>Organization:</b></td>
                                <td nowrap width="50%"><asp:DropDownList ID="ddlRejectOrganization" runat="server" CssClass="default" Width="400" /></td>
                                <td nowrap><b>Investment Class:</b></td>
                                <td nowrap width="50%">
                                    <asp:DropDownList ID="ddlRejectType" runat="server" CssClass="default">
                                        <asp:ListItem Value="0" Text="-- SELECT --" />
                                        <asp:ListItem Value="Acquisitions & Divestitures" />
                                        <asp:ListItem Value="Baseline" />
                                        <asp:ListItem Value="Client Billable/Contractual" />
                                        <asp:ListItem Value="Discretionary Project" />
                                        <asp:ListItem Value="Efficiency Initiatives" />
                                        <asp:ListItem Value="General Management & Administration" />
                                        <asp:ListItem Value="Client Implementations & Conversions" />
                                        <asp:ListItem Value="Non-Billable FTE" />
                                        <asp:ListItem Value="Application Support" />
                                        <asp:ListItem Value="Non-Technology" />
                                        <asp:ListItem Value="Regulatory & Compliance" />
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td nowrap><b>Segment:</b></td>
                                <td nowrap width="50%"><asp:DropDownList ID="ddlRejectSegment" runat="server" CssClass="default" Width="400" /></td>
                                <td nowrap><b>Submitted By:</b></td>
                                <td nowrap width="50%"><asp:Label ID="lblRejectSubmitted" runat="server" CssClass="default" /></td>
                            </tr>
                            <tr>
                                <td nowrap><b>Project Status:</b></td>
                                <td nowrap width="50%"><asp:Label ID="lblRejectStatus" runat="server" CssClass="default" /></td>
                                <td nowrap><b>Submitted On:</b></td>
                                <td nowrap width="50%"><asp:Label ID="lblRejectSubmittedOn" runat="server" CssClass="default" /></td>
                            </tr>
                            <tr>
                                <td nowrap valign="top"><b>Reason for Rejection:</b></td>
                                <td colspan="3"><asp:Label ID="lblRejectReason" runat="server" CssClass="default" /></td>
                            </tr>
                            <tr>
                                <td colspan="4"><hr size="1" noshade /></td>
                            </tr>
                            <tr>
                                <td colspan="4" align="right"><asp:Button ID="btnRejectProject" runat="server" CssClass="default" Text="Update" Width="75" OnClick="btnRejectProject_Click" /></td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            </asp:Panel>
<asp:Panel ID="panServices" runat="server" Visible="false">
<table width="100%" cellpadding="0" cellspacing="5" border="0">
    <tr>
        <td rowspan="2"><img src="/images/service_request.gif" border="0" align="absmiddle" /></td>
        <td class="header" width="100%" valign="bottom">Service Request</td>
    </tr>
    <tr>
        <td width="100%" valign="top">Service Requests are used to request a service from another department. Use the service browser to add a service to your order.</td>
    </tr>
</table>
<table width="100%" border="0" cellSpacing="2" cellPadding="4" class="default">
    <tr>
        <td>
            <table width="100%" border="0" cellSpacing="0" cellPadding="0" class="default">
                <asp:Panel ID="panDelete" runat="server" Visible="false">
	            <tr>
		            <td colspan="2" align="center" class="bigcheck"><img src='/images/bigCheck.gif' border='0' align='absmiddle' /> Service Request Deleted Successfully</td>
	            </tr>
                <tr>
                    <td colspan="2">&nbsp;</td>
                </tr>
	            </asp:Panel>
                <tr>
                    <td><%=strCrumbs%></td>
                    <td align="right">Search:&nbsp;<asp:TextBox ID="txtSearch" runat="server" Width="125" />&nbsp;<asp:ImageButton ID="imgSearch" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/search.gif" OnClick="imgSearch_Click" /></td>
                </tr>
                <tr>
                    <td colspan="2">&nbsp;</td>
                </tr>
                <tr>
                    <td colspan="2"><%=strServices%></td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
		<td align="center">
		    <table width="100%" cellpadding="0" cellspacing="0" border="0">
		        <tr>
		            <td>
		                <table cellpadding="4" cellspacing="4" border="0">
		                    <tr>
		                        <td><asp:Button ID="btnAdd" runat="server" OnClick="btnAdd_Click" CssClass="default" Text="Add Service(s)" Height="20" Width="125" /></td>
		                        <td><asp:Button ID="btnCart" runat="server" OnClick="btnCart_Click" CssClass="default" Text="View Selected Service(s)  >>" Height="20" Width="200" /></td>
		                    </tr>
		                </table>
		            </td>
		            <td align="right">
		                <table cellpadding="4" cellspacing="4" border="0">
		                    <tr>
		                        <td><asp:Button ID="btnCancel1" runat="server" OnClick="btnCancel_Click" CssClass="default" Text="Cancel Request" Height="20" Width="125" /></td>
		                    </tr>
		                </table>
		            </td>
		        </tr>
		    </table>
		</td>
    </tr>
    <asp:Panel ID="panFavorites" runat="server" Visible="false">
    <tr>
        <td>&nbsp;</td>
    </tr>
    <tr>
        <td class="header">
            <table width="100%" cellpadding="0" cellspacing="5" border="0">
                <tr>
                    <td rowspan="2"><img src="/images/favorite.gif" border="0" align="absmiddle" /></td>
                    <td class="header" width="100%" valign="bottom">My Favorite Services</td>
                </tr>
                <tr>
                    <td width="100%" valign="top">You can save a service as a favorite when completing the form. Here are your favorite services...</td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
		<td align="center">
		    <table width="100%" cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td colspan="2"><%=strFavorites%></td>
                </tr>
		        <tr>
		            <td>
		                <table cellpadding="4" cellspacing="4" border="0">
		                    <tr>
		                        <td><asp:Button ID="btnFavorite" runat="server" OnClick="btnAdd_Click" CssClass="default" Text="Add Favorite(s)" Height="20" Width="125" /></td>
		                    </tr>
		                </table>
		            </td>
		            <td align="right">
		            </td>
		        </tr>
		    </table>
		</td>
    </tr>
    </asp:Panel>
</table>
</asp:Panel>
<asp:Panel ID="panCheckout" runat="server" Visible="false">
<table width="100%" cellpadding="0" cellspacing="5" border="0">
    <tr>
        <td rowspan="2"><img src="/images/service_request.gif" border="0" align="absmiddle" /></td>
        <td class="header" width="100%" valign="bottom">Service Request Order Summary</td>
    </tr>
    <tr>
        <td width="100%" valign="top">Here is the summary of your order selected services. To delete a service, click the <b>Remove</b> link on the right. To add additional services, click <b>Additional Services</b>.</td>
    </tr>
</table>
<table width="100%" cellpadding="4" cellspacing="2" border="0">
	<tr>
		<td>
			<table width="100%" cellpadding="2" cellspacing="2" border="0">
			    <tr>
			        <td colspan="3"><%=strSummary%></td>
			    </tr>
				<tr>
					<td width="50%">&nbsp;</td>
					<td width="50%" align="right">SubTotal: <b><asp:Label ID="lblHoursCheckout" runat="server" CssClass="default" /></b>&nbsp;&nbsp;</td>
					<td width="75"><img src="/images/spacer.gif" border="0" width="75" height="1" /></td>
				</tr>
			</table>
		</td>
	</tr>
	<tr>
		<td align="center">
		    <table width="100%" cellpadding="0" cellspacing="0" border="0">
		        <tr>
		            <td>
		                <table cellpadding="4" cellspacing="4" border="0">
		                    <tr>
		                        <td><asp:Button ID="btnServices" runat="server" OnClick="btnServices_Click" CssClass="default" Text="<<  Additional Services" Height="20" Width="175" /></td>
		                        <td><asp:Button ID="btnUpdate" runat="server" OnClick="btnUpdate_Click" CssClass="default" Text="Update Cart" Height="20" Width="125" /></td>
		                        <td><asp:Button ID="btnCheckout" runat="server" OnClick="btnCheckout_Click" CssClass="default" Text="Continue Checkout  >>" Height="20" Width="175" /></td>
		                    </tr>
		                </table>
		            </td>
		            <td align="right">
		                <table cellpadding="4" cellspacing="4" border="0">
		                    <tr>
            		            <td><asp:Button ID="btnCancel2" runat="server" OnClick="btnCancel_Click" CssClass="default" Text="Cancel Request" Height="20" Width="125" /></td>
		                    </tr>
		                </table>
		            </td>
		        </tr>
		    </table>
		</td>
	</tr>
</table>
</asp:Panel>
<asp:Panel ID="panPending" runat="server" Visible="false">
<table width="100%" cellpadding="0" cellspacing="5" border="0">
    <tr>
        <td rowspan="2"><img src="/images/service_request.gif" border="0" align="absmiddle" /></td>
        <td class="header" width="100%" valign="bottom">Project Information</td>
    </tr>
    <tr>
        <td width="100%" valign="top">Some service requests require a project to be associated to the work. Please follow the instructions below to define your project.</td>
    </tr>
</table>
<table width="100%" cellpadding="4" cellspacing="2" border="0">
	<asp:Panel ID="panPendingChoose" runat="server" Visible="false">
	<tr>
	    <td colspan="2">
            <table width="100%" cellpadding="4" cellspacing="3" border="0" class="default">
                <tr>
                    <td>Please select a project to associate with this service request.</td>
                    <td align="right"><b>Order By: </b><asp:LinkButton ID="btnOrderName" runat="server" CommandArgument="namenumber" OnClick="btnOrder_Click" Text="Project Name" /> | <asp:LinkButton ID="btnOrderNumber" runat="server" CommandArgument="numbername" OnClick="btnOrder_Click" Text="Project Number" /></td>
                </tr>
                <tr>
                    <td class="footer"><b>NOTE:</b> If you do not see your project, choose the &quot;<span style="color:#DD0000">-- PROJECT NOT LISTED --</span>&quot; option...</td>
                    <td align="right"><img src="/images/search_icon.gif" border="0" align="absmiddle" /> <a href="javascript:void(0);" onclick="ShowHideDiv2('trQuickFind');">Show Quick Find Filter</a></td>
                </tr>
                <tr id="trQuickFind" style="display:none">
                    <td colspan="2" align="center">
                        <table cellpadding="4" cellspacing="3" border="0" class="default" style="border:solid 1px #999999" bgcolor="#f9f9f9">
                            <tr>
                                <td nowrap><img src="/images/search_icon.gif" border="0" align="absmiddle" /> <b>Quick Find Filter:</b></td>
                                <td nowrap>Project Number:</td>
                                <td><asp:TextBox ID="txtSearchNumber" runat="server" CssClass="default" Width="100" MaxLength="20" /></td>
                                <td><img src="/images/spacer.gif" border="0" width="10" height="1" /></td>
                                <td nowrap class="header">-- OR --</td>
                                <td><img src="/images/spacer.gif" border="0" width="10" height="1" /></td>
                                <td nowrap>Project Name:</td>
                                <td><asp:TextBox ID="txtSearchName" runat="server" CssClass="default" Width="200" MaxLength="35" /></td>
                                <td><img src="/images/spacer.gif" border="0" width="10" height="1" /></td>
                                <td><asp:Button ID="btnSearch" runat="server" CssClass="default" Width="75" Text="Search" /></td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="2"><asp:ListBox ID="lstProjects" runat="server" CssClass="default" Width="100%" Rows="25" /></td>
                </tr>
                <tr>
                    <td colspan="2" align="center">
		                <table width="100%" cellpadding="0" cellspacing="0" border="0">
		                    <tr>
		                        <td>
		                            <table cellpadding="4" cellspacing="4" border="0">
		                                <tr>
                                            <td><asp:Button ID="btnSelect" runat="server" OnClick="btnSelect_Click" CssClass="default" Text="Continue  >>" Height="20" Width="125" /></td>
		                                </tr>
		                            </table>
		                        </td>
		                        <td align="right">
		                            <table cellpadding="4" cellspacing="4" border="0">
		                                <tr>
                                            <td><asp:Button ID="btnCancel4" runat="server" OnClick="btnCancel_Click" CssClass="default" Text="Cancel Request" Height="20" Width="125" /></td>
		                                </tr>
		                            </table>
		                        </td>
		                    </tr>
		                </table>
                    </td>
                </tr>
            </table>
	    </td>
	</tr>
	</asp:Panel>
	<asp:Panel ID="panPendingNew" runat="server" Visible="false">
	<tr>
	    <td colspan="2">
            <table width="100%" cellpadding="4" cellspacing="3" border="0" class="default">
                <tr>
                    <td class="bold">Is this request related to a project?</td>
                </tr>
                <tr>
                    <td>
                        <asp:RadioButton ID="radTaskYes" runat="server" Text="Yes it is project related" CssClass="default" GroupName="Task" />&nbsp;
                        <asp:RadioButton ID="radTaskNo" runat="server" Text="No it is <b>not</b> project related" CssClass="default" GroupName="Task" />
                    </td>
                </tr>
                <tr height="1">
                    <td><img src="/images/spacer.gif" border="0" width="1" height="1" /></td>
                </tr>
            </table>
            <div id="divTaskYes" runat="server" style="display:none">
                <table width="100%" cellpadding="4" cellspacing="3" border="0" class="default">
                    <tr>
                        <td colspan="2" class="bold">Please fill in the related project information using the following form.</td>
                    </tr>
                    <tr>
                        <td nowrap>Project Number:</td>
                        <td width="100%"><asp:TextBox ID="txtNumber" runat="server" CssClass="default" Width="150" MaxLength="20" /> <span class="required">(Not Required)</span></td>
                    </tr>
                    <tr>
                        <td nowrap>Project Name:</td>
                        <td width="100%"><asp:TextBox ID="txtName" runat="server" CssClass="default" Width="300" MaxLength="35" /></td>
                    </tr>
                    <tr>
                        <td nowrap>Project Type:</td>
                        <td width="100%">
                            <asp:DropDownList ID="ddlBaseDisc" runat="server" CssClass="default">
                                <asp:ListItem Value="0" Text="-- SELECT --" />
                                <asp:ListItem Value="Acquisitions & Divestitures" />
                                <asp:ListItem Value="Baseline" />
                                <asp:ListItem Value="Client Billable/Contractual" />
                                <asp:ListItem Value="Discretionary Project" />
                                <asp:ListItem Value="Efficiency Initiatives" />
                                <asp:ListItem Value="General Management & Administration" />
                                <asp:ListItem Value="Client Implementations & Conversions" />
                                <asp:ListItem Value="Non-Billable FTE" />
                                <asp:ListItem Value="Application Support" />
                                <asp:ListItem Value="Non-Technology" />
                                <asp:ListItem Value="Regulatory & Compliance" />
                            </asp:DropDownList>
                    </td>
                    </tr>
                    <tr>
                        <td nowrap>Sponsoring Portfolio:</td>
                        <td width="100%"><asp:DropDownList ID="ddlOrganization" runat="server" CssClass="default" Width="400" /></td>
                    </tr>
                    <tr>
                        <td nowrap>Segment:</td>
                        <td width="100%">
                            <asp:DropDownList ID="ddlSegment" CssClass="default" runat="server" Width="400" Enabled="false" >
                                <asp:ListItem Value="-- Please select a Sponsoring Portfolio --" />
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td><asp:Button ID="btnPendingProject" runat="server" CssClass="default" Text="Submit" Width="75" OnClick="btnPendingProject_Click" /> <asp:Button ID="btnBackProject" runat="server" CssClass="default" Text="Back" Width="75" OnClick="btnBack_Click" /></td>
                    </tr>
                </table>
            </div>
            <div id="divTaskNo" runat="server" style="display:none">
                <table width="100%" cellpadding="4" cellspacing="3" border="0" class="default">
                    <tr>
                        <td colspan="2" class="bold">Please provide a name for the service(s) to be requested and a brief description.</td>
                    </tr>
                    <tr>
                        <td nowrap>Task Name:</td>
                        <td width="100%"><asp:TextBox ID="txtTaskName" runat="server" CssClass="default" Width="500" MaxLength="100" /></td>
                    </tr>
                    <tr>
                        <td nowrap valign="top">Description:</td>
                        <td width="100%"><asp:TextBox ID="txtDescription" runat="server" CssClass="default" Width="500" Rows="7" TextMode="MultiLine" /></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td><asp:Button ID="btnPendingTask" runat="server" CssClass="default" Text="Submit" Width="75" OnClick="btnPendingTask_Click" /> <asp:Button ID="btnBackTask" runat="server" CssClass="default" Text="Back" Width="75" OnClick="btnBack_Click" /></td>
                    </tr>
                </table>
            </div>
	    </td>
	</tr>
	</asp:Panel>
</table>
</asp:Panel>
<asp:Panel ID="panControl" runat="server" Visible="false">
<table width="100%" cellpadding="0" cellspacing="5" border="0">
    <tr>
        <td><asp:PlaceHolder ID="PHForm" runat="server" /></td>
    </tr>
</table>
</asp:Panel>
<asp:Panel ID="panSummary" runat="server" Visible="false">
<table width="100%" cellpadding="0" cellspacing="5" border="0">
    <tr id="panTitle" runat="server" visible="false">
        <td>
            <table width="100%" cellpadding="4" cellspacing="2" border="0">
                <tr>
                    <td colspan="2">Please enter a title for this request:<span class="required">&nbsp;*</span></td>
                </tr>
                <tr>
                    <td><img src="/images/spacer.gif" border="0" width="50" height="1"/></td>
                    <td width="100%">
                        <asp:TextBox ID="txtTitle" runat="server" CssClass="default" MaxLength="100" Width="500" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2"><img src="/images/spacer.gif" border="0" width="1" height="5" /></td>
                </tr>
                <tr ID="panManagerApproval" runat="server" Visible="false">
                    <td><img src="/images/spacer.gif" border="0" width="50" height="1"/></td>
                    <td width="90%">
                        <table width="500" cellpadding="3" cellspacing="2" border="0" style="border:solid 1px #999999" bgcolor="#f9f9f9">
                            <tr>
                                <td rowspan="2" valign="top"><img src="/images/bigAlert.gif" border="0" align="absmiddle" /></td>
                                <td class="header" width="100%" valign="bottom">Manager Approval Required</td>
                            </tr>
                            <tr>
                                <td colspan="2" width="100%" valign="top">One or more of the services you selected requires your manager (<asp:Label ID="lblManager" runat="server" CssClass="bold" />) approve the request.</td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr ID="panManagerError" runat="server" Visible="false">
                    <td><img src="/images/spacer.gif" border="0" width="50" height="1"/></td>
                    <td width="90%">
                        <table width="500" cellpadding="3" cellspacing="2" border="0" style="border:solid 1px #999999" bgcolor="#f9f9f9">
                            <tr>
                                <td rowspan="2" valign="top"><img src="/images/bigError.gif" border="0" align="absmiddle" /></td>
                                <td class="header" width="100%" valign="bottom">Manager Approval Required</td>
                            </tr>
                            <tr>
                                <td colspan="2" width="100%" valign="top">One or more of the services you selected requires your manager to approve the request. Unfortunately, ClearView could not locate your manager in the system.  You will need to have your manager register in ClearView before you can submit this request.</td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td>
            <table width="100%" cellpadding="4" cellspacing="2" border="0">
                <tr>
                    <td colspan="2">
                        <%=strSummary%>
                    </td>
                </tr>
				<tr>
					<td width="100%" align="right">SubTotal: <b><asp:Label ID="lblHours" runat="server" CssClass="default" /></b>&nbsp;&nbsp;</td>
					<td width="125"><img src="/images/spacer.gif" border="0" width="125" height="1" /></td>
				</tr>
                <tr id="panCancel" runat="server" visible="false">
                    <td colspan="2">
                        <table width="100%" cellpadding="2" cellspacing="1" border="0">
                            <tr>
	                            <td><img src="/images/spacer.gif" border="0" height="1" width="30" /></td>
                                <td width="100%"><asp:PlaceHolder ID="phCancel" runat="server" /></td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
		            <td colspan="2" align="center">
		                <table width="100%" cellpadding="0" cellspacing="0" border="0">
		                    <tr>
		                        <td>
		                            <table cellpadding="4" cellspacing="4" border="0">
		                                <tr>
                                            <td>
        		                                <asp:Button ID="btnRefresh" runat="server" OnClick="btnRefresh_Click" CssClass="default" Text="Refresh Request" />
                                                <asp:Button ID="btnTitle" runat="server" Width="75" Text="Update" OnClick="btnTitle_Click" />
	                                            <asp:Button ID="btnComplete" runat="server" OnClick="btnComplete_Click" CssClass="default" Text="Submit Request  >>" />
	                                        </td>
		                                </tr>
		                            </table>
		                        </td>
		                        <td align="right">
		                            <table cellpadding="4" cellspacing="4" border="0">
		                                <tr>
        		                            <td>
        		                                <asp:Button ID="btnCancel3" runat="server" OnClick="btnCancel_Click" CssClass="default" Text="Cancel Request" />
                                                <asp:Button ID="btnPrinter" runat="server" CssClass="default" Text="Printer Friendly Invoice" />
        		                            </td>
		                                </tr>
		                            </table>
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
<asp:Panel ID="panResult" runat="server" Visible="false">
<table width="100%" cellpadding="0" cellspacing="5" border="0">
    <tr>
        <td>
            <table width="100%" cellpadding="4" cellspacing="2" border="0">
                <tr>
                    <td align="center" class="result">Your service request number is <b><asp:Label id="lblRequest" CssClass="result" runat="server" /></b>.</td>
                </tr>
                <tr>
                    <td>
                        <table width="100%" cellpadding="2" cellspacing="1" border="0">
                            <tr>
	                            <td><img src="/images/spacer.gif" border="0" height="1" width="30" /></td>
                                <td width="100%"><asp:PlaceHolder ID="PHcp" runat="server" /></td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td><span style="width:100%;border-bottom:1 dotted #CCCCCC;"/></td>
                </tr>
                <tr>
		            <td align="center">
		                <table width="100%" cellpadding="0" cellspacing="0" border="0">
		                    <tr>
		                        <td>
		                            <table cellpadding="4" cellspacing="4" border="0">
		                                <tr>
		                                    <td><asp:Button ID="btnResult" runat="server" OnClick="btnRefresh_Click" CssClass="default" Text="Refresh Request" /></td>
		                                </tr>
		                            </table>
		                        </td>
		                        <td align="right">
		                            <table cellpadding="4" cellspacing="4" border="0">
		                                <tr>
                                            <td><asp:Button ID="btnPrinter2" runat="server" CssClass="default" Text="Printer Friendly Invoice" /></td>
		                                </tr>
		                            </table>
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
<asp:Panel ID="panPendingSubmission" runat="server" Visible="false">
<table width="100%" cellpadding="0" cellspacing="5" border="0">
    <tr>
        <td>
            <table width="100%" cellpadding="4" cellspacing="2" border="0">
                <tr>
                    <td><img src="/images/spacer.gif" border="0" width="15" height="1" /></td>
                    <td class="header"><img src="/images/bigCheck.gif" border="0" align="absmiddle" /> Service Request Submitted</td>
                </tr>
                <tr>
                    <td><img src="/images/spacer.gif" border="0" width="15" height="1" /></td>
                    <td>Your service request has been submitted successfully. You will be notified via email of all actions taken on this request.</td>
                </tr>
            </table>
        </td>
    </tr>
</table>
</asp:Panel>
<asp:Panel ID="panView" runat="server" Visible="false">
<table width="100%" cellpadding="0" cellspacing="5" border="0">
    <tr>
        <td rowspan="2"><img src="/images/service_request.gif" border="0" align="absmiddle" /></td>
        <td class="header" width="100%" valign="bottom">Service Request</td>
    </tr>
    <tr>
        <td width="100%" valign="top">Service Requests are used to request a service from another department.</td>
    </tr>
</table>
<table width="100%" cellpadding="0" cellspacing="5" border="0">
    <tr>
        <td>
            <table width="100%" cellpadding="2" cellspacing="2" border="0">
                <tr>
                    <td><asp:Label ID="lblView" runat="server" CssClass="default" /></td>
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
        </td>
        <td background="/images/table_right.gif"><img src="/images/table_right.gif" width="5" height="10"></td>
    </tr>
    <tr>
        <td><img src="/images/table_bottomLeft.gif" border="0" width="5" height="9"></td>
        <td width="100%" background="/images/table_bottom.gif"></td>
        <td><img src="/images/table_bottomRight.gif" border="0" width="5" height="9"></td>
    </tr>
</table>
<asp:HiddenField ID="hdnService" runat="server" />
<asp:HiddenField ID="hdnQuantity" runat="server" />
<asp:HiddenField ID="hdnProject" runat="server" />
<asp:Label ID="lblProject" runat="server" Visible="false" />
<asp:Label ID="lblService" runat="server" Visible="false" />
<asp:Label ID="lblPending" runat="server" Visible="false" />
<asp:Label ID="lblApplication" runat="server" Visible="false" />
<asp:Label ID="lblControls" runat="server" Visible="false" />
<asp:HiddenField ID="hdnSegment" runat="server" />
