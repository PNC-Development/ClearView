<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wm_pc_new.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.wm_pc_new" %>


<script type="text/javascript">
    function ChangeFrame3(oCell, oShow, oHidden, strHidden) {
        var oTable = document.getElementById("tblDivs");
        var oDIVs = oTable.getElementsByTagName("DIV");
        for(var ii=0;ii<oDIVs.length;ii++){
            if (oDIVs[ii].getAttribute("id") != "")
                oDIVs[ii].style.display = "none"
        }
        oShow = document.getElementById(oShow);
        oShow.style.display = "inline";
        var oTable = document.getElementById("tblTabs");
        var oTDs = oTable.getElementsByTagName("TD");
        for(var ii=0;ii<oTDs.length;ii++){
    		if (oTDs[ii].className == "cmbuttontop")
                oTDs[ii].style.borderTop = "1px solid #94a6b5"
    		if (oTDs[ii].className == "cmbutton")
                oTDs[ii].style.border = "1px solid #94a6b5"
        }
	    oCell.style.borderTop = "3px solid #6A8359"
        oCell.style.borderBottom = "1px solid #FFFFFF"
        oHidden = document.getElementById(oHidden);
        oHidden.value = strHidden;
    }
    var keepDescription = null;
    var newDescription = null;
    function DescriptionDown(oText) {
        if (keepDescription == null)
        {
            keepDescription = oText.value;
            newDescription = keepDescription;
        }
    }
    function DescriptionUp(oText) {
        if (oText.value.indexOf(keepDescription) == 0) {
            newDescription = oText.value;
        }
        else {
            alert('NOTE: You cannot change the description. You can only add additional information.');
            oText.value = newDescription;
        }
    }
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
		oHidden.value = oHidden.value + oControl.options[ii].value + "&";
	}
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
                        <td><asp:ImageButton ID="btnHold" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/tool_hold.gif" OnClick="btnHold_Click" /></td>
                        <td><asp:ImageButton ID="btnCancel" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/tool_cancel.gif" OnClick="btnCancel_Click" /></td>
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
                        <td><asp:ImageButton ID="btnPCRCSRC" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/tool_pcr_csrc.gif" /></td>
                        <td><asp:ImageButton ID="btnClose" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/tool_close.gif" Visible="false" /></td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td class="default">
                <table width="100%" border="0" cellSpacing="2" cellPadding="2" class="default">
                    <tr>
                        <td width="50%" valign="top">
                            <table width="100%" border="0" cellSpacing="2" cellPadding="2" class="default">
                                <tr>
                                    <td nowrap><b>Project Name:</b></td>
                                    <td width="100%"><asp:TextBox ID="txtName" runat="server" CssClass="default" Width="90%" MaxLength="35" /></td>
                                </tr>
                                <tr>
                                    <td nowrap><b>Nickname:</b></td>
                                    <td width="100%"><asp:TextBox ID="txtCustom" runat="server" CssClass="default" Width="90%" MaxLength="35" /></td>
                                </tr>
                                <tr>
                                    <td nowrap><b>Clarity Number:</b></td>
                                    <td width="100%"><asp:Label ID="lblNumber" runat="server" CssClass="default" /></td>
                                </tr>
                                <tr>
                                    <td nowrap><b>Project Type:</b></td>
                                    <td width="100%">
                                        <asp:DropDownList ID="ddlBaseDisc" runat="server" CssClass="default" ToolTip="If your initative is considerd Discretionary project DO NOT USE THIS FORM">
                                            <asp:ListItem Value="Base" />
                                            <asp:ListItem Value="Discretionary" />
                                        </asp:DropDownList></td>
                                    </td>
                                </tr>
                                <tr>
                                    <td nowrap><b>Executive Sponsor:</b></td>
                                    <td width="100%">
                                        <table cellpadding="0" cellspacing="0" border="0">
                                            <tr>
                                                <td><asp:TextBox ID="txtExecutive" runat="server" Width="250" CssClass="default" /></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <div id="divExecutive" runat="server" style="overflow:hidden; position:absolute; display:none; background-color:#FFFFFF; border:solid 1px #CCCCCC">
                                                        <asp:ListBox ID="lstExecutive" runat="server" CssClass="default" />
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td nowrap><b>Working Sponsor:</b></td>
                                    <td width="100%">
                                        <table cellpadding="0" cellspacing="0" border="0">
                                            <tr>
                                                <td><asp:TextBox ID="txtWorking" runat="server" Width="250" CssClass="default" /></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <div id="divWorking" runat="server" style="overflow:hidden; position:absolute; display:none; background-color:#FFFFFF; border:solid 1px #CCCCCC">
                                                        <asp:ListBox ID="lstWorking" runat="server" CssClass="default" />
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td nowrap><b>Sponsoring Portfolio:</b></td>
                                    <td width="100%"><asp:DropDownList ID="ddlPortfolio" runat="server" CssClass="default" Width="250" /></td>
                                </tr>
                                <tr>
                                    <td nowrap><b>Segment:</b></td>
                                    <td width="100%">
                                        <asp:DropDownList ID="ddlSegment" CssClass="default" runat="server" Width="250" Enabled="false" >
                                            <asp:ListItem Value="-- Please select a Sponsoring Portfolio --" />
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td nowrap><b>Overall Priority:</b></td>
                                    <td width="100%"><asp:Label ID="lblPriority" runat="server" CssClass="default" /></td>
                                </tr>
                                <tr>
                                    <td nowrap><b>Overall Status:</b></td>
                                    <td width="100%"><asp:Label ID="lblStatus" runat="server" CssClass="default" /></td>
                                </tr>
                                <tr>
                                    <td nowrap><b>Project Status:</b></td>
                                    <td width="100%"><asp:Label ID="lblProjectStatus" runat="server" CssClass="default" /></td>
                                </tr>
                            </table>
                        </td>
                        <td width="50%" valign="top">
                            <table width="100%" border="0" cellSpacing="2" cellPadding="3" class="default">
                                <tr>
                                    <td nowrap valign="top"><b>Project Description:</b></td>
                                    <td width="100%"><asp:TextBox ID="txtDescription" runat="server" CssClass="default" Width="90%" TextMode="MultiLine" Rows="9" /></td>
                                </tr>
                                <tr>
                                    <td nowrap valign="top"><br /><b>Cost Centers:</b></td>
                                    <td width="100%">
                                        <table cellpadding="2" cellspacing="0" border="0">
                                            <tr>
                                                <td class="default">Selected:</td>
                                                <td class="default" colspan="3">&nbsp;</td>
                                                <td class="default">Available:</td>
                                            </tr>
                                            <tr>
                                                <td><asp:ListBox ID="lstCostsCurrent" runat="server" Width="200" CssClass="default" Rows="5" /></td>
                                                <td>&nbsp;</td>
                                                <td>
                                                    <asp:ImageButton ID="btnCostAdd" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/lt.gif" ToolTip="Add" /><br /><br />
                                                    <asp:ImageButton ID="btnCostRemove" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/rt.gif" ToolTip="Remove" />
                                                </td>
                                                <td>&nbsp;</td>
                                                <td><asp:ListBox ID="lstCostsAvailable" runat="server" Width="200" CssClass="default" Rows="5" /></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td nowrap><b>Project Start Date:</b></td>
                                    <td width="100%"><asp:Label ID="lblStartDate" runat="server" CssClass="default" /></td>
                                </tr>
                                <tr>
                                    <td nowrap><b>Project End Date:</b></td>
                                    <td width="100%"><asp:Label ID="lblEndDate" runat="server" CssClass="default" /></td>
                                </tr>
                                <tr>
                                    <td nowrap><b>Total Actual Cost:</b></td>
                                    <td width="100%">$<asp:Label ID="lblTotalActual" runat="server" CssClass="default" /></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr height="1">
                        <td colspan="2"><img src="/images/spacer.gif" border="0" height="1" width="1" /></td>
                    </tr>
                    <tr>
                        <td><asp:Button ID="btnView" runat="server" Text="" CssClass="default" Width="250" /></td>
                        <td><asp:Label ID="lblComplete" runat="server" CssClass="default" /></td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
            <br />
                <table cellpadding="0" cellspacing="0" border="0">
                    <tr>
                        <td><img src="/images/TabEmptyBackground.gif"></td>
                        <%=strTabs %>
                        <td width="100%" background="/images/TabEmptyBackground.gif">&nbsp;</td>
                    </tr>
                </table>
                <table width="100%" height="100%" border="0" cellSpacing="2" cellPadding="2" class="default">
                    <tr> 
                        <td valign="top">
                                <div id="divTab7"  style='<%=boolDetails == true ? "display:inline" : "display:none" %>'>
                                    <br /><br />
                                    <table width="100%" cellpadding="0" cellspacing="5" border="0">
                                        <tr>
                                            <td rowspan="2" valign="top"><img src="/images/details.gif" border="0" align="absmiddle" /></td>
                                            <td class="hugeheader" width="100%" valign="bottom">Project Details</td>
                                        </tr>
                                        <tr>
                                            <td width="100%" valign="top">Here are the details of this project.</td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" background="/images/lines.gif">&nbsp;</td>
                                        </tr>
                                    </table><br />
                                    <table width="500" cellpadding="5" cellspacing="0" border="0" style="border:solid 1px #CCCCCC">
                                        <tr bgcolor="#EEEEEE">
                                            <td colspan="4" class="header" align="center">Estimated Project Timeline</td>
                                        </tr>
                                        <tr>
                                            <td align="center"><b></b></td>
                                            <td align="center"><b>Estimate</b></td>
                                            <td align="center"><b>Actual</b></td>
                                        </tr>
                                        <tr>
                                            <td nowrap>Start Date:</td>
                                            <td align="center"><asp:Label ID="lblStartE" runat="server" CssClass="default" /></td>
                                            <td align="center"><asp:TextBox ID="txtStartA" runat="server" CssClass="default" Width="80" MaxLength="10" /> <asp:ImageButton ID="imgStartA" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calendar.gif" /></td>
                                        </tr>
                                        <tr>
                                            <td nowrap>End Date:</td>
                                            <td align="center"><asp:Label ID="lblEndE" runat="server" CssClass="default" /></td>
                                            <td align="center"><asp:TextBox ID="txtEndA" runat="server" CssClass="default" Width="80" MaxLength="10" /> <asp:ImageButton ID="imgEndA" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calendar.gif" /></td>
                                        </tr>
                                    </table>
                                    <br />
                                    <table width="500" cellpadding="5" cellspacing="0" border="0" style="border:solid 1px #CCCCCC">
                                        <tr bgcolor="#EEEEEE">
                                            <td colspan="3" class="header" align="center">Estimated Labor and Capital Costs</td>
                                        </tr>
                                        <tr>
                                            <td align="center"><b>Labor / Cost Type</b></td>
                                            <td align="center"><b>Estimate</b></td>
                                            <td align="center"><b>Actual</b></td>
                                        </tr>
                                        <tr>
                                            <td nowrap>Internal Labor:</td>
                                            <td align="center">$<asp:Label ID="lblEstimateI" runat="server" CssClass="default" /></td>
                                            <td align="center">$<asp:TextBox ID="txtActualI" runat="server" CssClass="default" Width="100" MaxLength="10" /> <asp:ImageButton ID="imgActualI" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calculator.gif" /></td>
                                        </tr>
                                        <tr>
                                            <td nowrap>External Labor:</td>
                                            <td align="center">$<asp:Label ID="lblEstimateE" runat="server" CssClass="default" /></td>
                                            <td align="center">$<asp:TextBox ID="txtActualE" runat="server" CssClass="default" Width="100" MaxLength="10" /> <asp:ImageButton ID="imgActualE" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calculator.gif" /></td>
                                        </tr>
                                        <tr>
                                            <td nowrap>HW/SQ/One Time Cost:</td>
                                            <td align="center">$<asp:Label ID="lblEstimateO" runat="server" CssClass="default" /></td>
                                            <td align="center">$<asp:TextBox ID="txtActualO" runat="server" CssClass="default" Width="100" MaxLength="10" /> <asp:ImageButton ID="imgActualO" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calculator.gif" /></td>
                                        </tr>
                                        <tr>
                                            <td colspan="3" style="border-top:solid 1px #CCCCCC"><img src="/images/spacer.gif" border="0" width="1" height="1" /></td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;</td>
                                            <td nowrap align="right"><b>Project Total:</b></td>
                                            <td align="center"><b>$<asp:Label ID="lblActual" runat="server" CssClass="default" /></b></td>
                                        </tr>
                                    </table>
                                </div>
                        
                                <div id="divTab5"  style='<%=boolStatus == true ? "display:inline" : "display:none" %>'>
                                    <br /><br />
                                    <table width="100%" cellpadding="0" cellspacing="5" border="0">
                                        <tr>
                                            <td rowspan="2" valign="top"><img src="/images/status.gif" border="0" align="absmiddle" /></td>
                                            <td class="hugeheader" width="100%" valign="bottom">Status Updates</td>
                                        </tr>
                                        <tr>
                                            <td width="100%" valign="top">Provide status updates of your project by completing the following form and clicking <b>Save</b>.</td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" background="/images/lines.gif">&nbsp;</td>
                                        </tr>
                                    </table><br />
                                    <table width="100%" cellpadding="5" cellspacing="0" border="0">
                                        <tr>
                                            <td width="50%" valign="top">
                                                <table width="100%" cellpadding="5" cellspacing="0" border="0">
                                                    <tr>
                                                        <td nowrap><b>Status Date:</b></td>
                                                        <td width="100%"><asp:TextBox ID="txtStatusDate" runat="server" CssClass="default" Width="100" MaxLength="10" /> <asp:ImageButton ID="imgStatusDate" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calendar.gif" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td nowrap><b>Variance:</b></td>
                                                        <td width="100%">
                                                            <asp:DropDownList ID="ddlVariance" runat="server" CssClass="default" >
                                                                <asp:ListItem Text="-- SELECT --" Value="0" />
                                                                <asp:ListItem Text="Red" Value="1" />
                                                                <asp:ListItem Text="Yellow" Value="2" />
                                                                <asp:ListItem Text="Green" Value="3" />
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td nowrap><b>This Week's<br />Accomplishments:</b></td>
                                                        <td width="100%"><asp:TextBox ID="txtThisWeek" runat="server" CssClass="default" Width="90%" TextMode="MultiLine" Rows="5" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td nowrap><b>Next Week's<br />Accomplishments:</b></td>
                                                        <td width="100%"><asp:TextBox ID="txtNextWeek" runat="server" CssClass="default" Width="90%" TextMode="MultiLine" Rows="5" /></td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td width="50%" valign="top">
                                                <table width="100%" cellpadding="4" cellspacing="1" border="0">
                                                    <tr>
                                                        <td><b>Comments / Issues:</b></td>
                                                    </tr>
                                                    <tr>
                                                        <td><asp:TextBox ID="txtComments" runat="server" CssClass="default" Width="90%" TextMode="MultiLine" Rows="13" /></td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                    <br /><br />
                                    <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                        <tr>
                                            <td>
                                                <table width="100%" cellpadding="3" cellspacing="0" border="0" style="border:solid 1px #CCCCCC">
                                                    <tr bgcolor="#EEEEEE">
                                                        <td nowrap><b>Date</b></td>
                                                        <td nowrap>&nbsp;</td>
                                                        <td nowrap align="center"><b>Overall Status</b></td>
                                                    </tr>
                                                    <asp:repeater ID="rptStatus" runat="server">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblScope" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "scope") %>' Visible="false" />
                                                            <asp:Label ID="lblTimeline" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "timeline") %>' Visible="false" />
                                                            <asp:Label ID="lblBudget" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "budget") %>' Visible="false" />
                                                            <tr class="default">
                                                                <td nowrap><a href="javascript:void(0);" onclick="ShowStatus('<%# DataBinder.Eval(Container.DataItem, "id") %>','t');"><%# DateTime.Parse(DataBinder.Eval(Container.DataItem, "datestamp").ToString()).ToShortDateString() %></a></td>
                                                                <td nowrap>&nbsp;</td>
                                                                <td width="100%" align="center"><asp:Label ID="lblStatus" runat="server" CssClass="default" Text='' /></td>
                                                            </tr>
                                                        </ItemTemplate>
                                                    </asp:repeater>
                                                <tr>
                                                    <td colspan="7">
                                                        <asp:Label ID="lblNoStatus" runat="server" CssClass="default" Visible="false" Text="<img src='/images/alert.gif' border='0' align='absmiddle'> There are no updates related to this project" />
                                                    </td>
                                                </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div id="divTab1" style='<%=boolResource == true ? "display:inline" : "display:none" %>'>
                                    <br /><br />
                                    <table width="100%" cellpadding="0" cellspacing="5" border="0">
                                        <tr>
                                            <td rowspan="2" valign="top"><img src="/images/users40.gif" border="0" align="absmiddle" /></td>
                                            <td class="hugeheader" width="100%" valign="bottom">Resource Involvement</td>
                                        </tr>
                                        <tr>
                                            <td width="100%" valign="top">The following list shows all resources involved in this project. Send a Communication to someone by completing the form at the bottom.</td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" background="/images/lines.gif">&nbsp;</td>
                                        </tr>
                                    </table><br />
                                    <table width="100%" cellpadding="3" cellspacing="0" border="0" style="border:solid 1px #CCCCCC">
                                        <tr bgcolor="#EEEEEE">
                                            <td nowrap width="1">&nbsp;</td>
                                            <td nowrap><b>Technician</b></td>
                                            <td nowrap><b>Name</b></td>
                                            <td nowrap><b>Department</b></td>
                                            <td nowrap align="right"><b>Allocated</b></td>
                                            <td nowrap align="right"><b>Used</b></td>
                                            <td nowrap align="right"><b>Completed</b></td>
                                            <td nowrap align="center"><b>Status</b></td>
                                        </tr>
                                        <asp:repeater ID="rptInvolvement" runat="server">
                                            <ItemTemplate>
                                                <tr class="default">
                                                    <asp:Label ID="lblId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "id") %>' />
                                                    <td width="1"><asp:Label ID="lblImage" runat="server" CssClass="default" Text='' /></td>
                                                    <td><asp:Label ID="lblUser" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "userid") %>' /></td>
                                                    <td><%# DataBinder.Eval(Container.DataItem, "name") %></td>
                                                    <td><asp:Label ID="lblItem" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "itemid") %>' /></td>
                                                    <td align="right"><asp:Label ID="lblAllocated" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "allocated") %>' /></td>
                                                    <td align="right"><asp:Label ID="lblUsed" runat="server" CssClass="default" Text='' /></td>
                                                    <td align="right"><asp:Label ID="lblPercent" runat="server" CssClass="default" Text='' /></td>
                                                    <td align="center"><asp:Label ID="lblStatus" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "status") %>' /></td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:repeater>
                                    <tr>
                                        <td colspan="7">
                                            <asp:Label ID="lblNoInvolvement" runat="server" CssClass="default" Visible="false" Text="<img src='/images/alert.gif' border='0' align='absmiddle'> There are no resources involved in this project" />
                                        </td>
                                    </tr>
                                    </table>
                                    <br /><br />
                                    <table width="100%" cellpadding="3" cellspacing="2" border="0">
                                        <tr>
                                            <td colspan="2" class="header">Send Communication</td>
                                        </tr>
                                        <tr>
                                            <td nowrap>Resource:</td>
                                            <td width="100%"><asp:DropDownList ID="ddlResource" runat="server" CssClass="default" Width="250" /></td>
                                        </tr>
                                        <tr>
                                            <td nowrap>Communication:</td>
                                            <td width="100%">
                                                <asp:DropDownList ID="ddlCommunication" runat="server" CssClass="default" Width="250">
                                                    <asp:ListItem Value="-- SELECT --" />
                                                    <asp:ListItem Value="Email" />
                                                    <asp:ListItem Value="Pager" />
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td nowrap>Message:</td>
                                            <td width="100%"><asp:TextBox ID="txtMessage" runat="server" CssClass="default" TextMode="MultiLine" Rows="8" Width="80%" /></td>
                                        </tr>
                                        <tr>
                                            <td nowrap>&nbsp;</td>
                                            <td width="100%"><asp:Button ID="btnMessage" runat="server" CssClass="default" Text="Send" Width="75" OnClick="btnMessage_Click" /></td>
                                        </tr>
                                    </table>
                                </div>
                                <div id="divTab6" style="display:none">
                                    <br /><br />
                                    <table width="100%" cellpadding="0" cellspacing="5" border="0">
                                        <tr>
                                            <td rowspan="2" valign="top"><img src="/images/tasks.gif" border="0" align="absmiddle" /></td>
                                            <td class="hugeheader" width="100%" valign="bottom">Your Additional Tasks</td>
                                        </tr>
                                        <tr>
                                            <td width="100%" valign="top">In addition to managing this project, you may be responsible for other tasks associated to this project. If so, you can find your additional tasks here.</td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" background="/images/lines.gif">&nbsp;</td>
                                        </tr>
                                    </table><br />
                                    <table width="100%" cellpadding="3" cellspacing="0" border="0" style="border:solid 1px #CCCCCC">
                                        <tr bgcolor="#EEEEEE">
                                            <td nowrap><b>Name</b></td>
                                            <td nowrap><b>Role</b></td>
                                            <td nowrap align="right"><b>Allocated</b></td>
                                            <td nowrap align="right"><b>Used</b></td>
                                            <td nowrap align="right"><b>Completed</b></td>
                                            <td nowrap align="center"><b>Status</b></td>
                                        </tr>
                                        <asp:repeater ID="rptMine" runat="server">
                                            <ItemTemplate>
                                                <tr class="default" onmouseover="CellRowOver(this);" onmouseout="CellRowOut(this);"  onclick="ShowRR('<%# DataBinder.Eval(Container.DataItem, "id") %>','<%# intPage.ToString() %>');">
                                                    <asp:Label ID="lblId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "id") %>' />
                                                    <asp:Label ID="lblServiceId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "serviceid") %>' />
                                                    <asp:Label ID="lblUser" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "userid") %>' />
                                                    <td><asp:Label ID="lblName" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "name") %>' /></td>
                                                    <td><asp:Label ID="lblItem" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "itemid") %>' /></td>
                                                    <td align="right"><asp:Label ID="lblAllocated" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "allocated") %>' /></td>
                                                    <td align="right"><asp:Label ID="lblUsed" runat="server" CssClass="default" Text='' /></td>
                                                    <td align="right"><asp:Label ID="lblPercent" runat="server" CssClass="default" Text='' /></td>
                                                    <td align="center"><asp:Label ID="lblStatus" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "status") %>' /></td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:repeater>
                                    <tr>
                                        <td colspan="7">
                                            <asp:Label ID="lblNoMine" runat="server" CssClass="default" Visible="false" Text="<img src='/images/alert.gif' border='0' align='absmiddle'> There are no additional tasks related to this project" />
                                        </td>
                                    </tr>
                                    </table>
                                </div>
                                <div id="divTab4" style='<%=boolDocuments == true ? "display:inline" : "display:none" %>'>
                                    <br /><br />
                                    <table width="100%" cellpadding="0" cellspacing="5" border="0">
                                        <tr>
                                            <td rowspan="2" valign="top"><img src="/images/documents.gif" border="0" align="absmiddle" /></td>
                                            <td class="hugeheader" width="100%" valign="bottom">Project Documents</td>
                                        </tr>
                                        <tr>
                                            <td width="100%" valign="top">Project Documents are shared documents that can be viewed by other project resources.</td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" background="/images/lines.gif">&nbsp;</td>
                                        </tr>
                                    </table><br />
                                    <table width="100%" cellpadding="3" cellspacing="2" border="0" class="default">
                                        <tr>
                                            <td align="right"><asp:CheckBox ID="chkDescription" runat="server" CssClass="default" Text="Show Descriptions" AutoPostBack="true" OnCheckedChanged="chkDescription_Change" /></td>
                                        </tr>
                                        <tr>
                                            <td><asp:Label ID="lblDocuments" runat="server" CssClass="default" /></td>
                                        </tr>
                                    </table>
                                </div>
                                <div id="divTab3" style='<%=boolMyDocuments == true ? "display:inline" : "display:none" %>'>
                                    <br /><br />
                                    <table width="100%" cellpadding="0" cellspacing="5" border="0">
                                        <tr>
                                            <td rowspan="2" valign="top"><img src="/images/documents_mine.gif" border="0" align="absmiddle" /></td>
                                            <td class="hugeheader" width="100%" valign="bottom">Your Documents</td>
                                        </tr>
                                        <tr>
                                            <td width="100%" valign="top">Your Project Documents are documents that are only for you, or can be shared with others.  To add a document and configure the permissions, click the <b>upload</b> button.</td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" background="/images/lines.gif">&nbsp;</td>
                                        </tr>
                                    </table><br />
                                    <table width="100%" cellpadding="3" cellspacing="2" border="0" class="default">
                                        <tr>
                                            <td><asp:Button ID="btnDocuments" runat="server" Text="Upload" Width="100" CssClass="default" /></td>
                                            <td align="right"><asp:CheckBox ID="chkMyDescription" runat="server" CssClass="default" Text="Show Descriptions" AutoPostBack="true" OnCheckedChanged="chkMyDescription_Change" /></td>
                                        </tr>
                                        <tr>
                                            <td colspan="2"><asp:Label ID="lblMyDocuments" runat="server" CssClass="default" /></td>
                                        </tr>
                                    </table>
                                </div>
                                <div id="divTab2" style='<%=boolMilestones == true ? "display:inline" : "display:none" %>'>
                                    <br /><br />
                                    <table width="100%" cellpadding="0" cellspacing="5" border="0">
                                        <tr>
                                            <td rowspan="2" valign="top"><img src="/images/milestones.gif" border="0" align="absmiddle" /></td>
                                            <td class="hugeheader" width="100%" valign="bottom">Milestones</td>
                                        </tr>
                                        <tr>
                                            <td width="100%" valign="top">Milestones are significant points in the development of the project. Here you can keep track of your project milestones.</td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" background="/images/lines.gif">&nbsp;</td>
                                        </tr>
                                    </table><br />
                                    <table cellpadding="5" cellspacing="0" border="0">
                                        <tr>
                                            <td nowrap><b>Approved Date:</b><font class="required">&nbsp;*</font></td>
                                            <td nowrap><asp:TextBox ID="txtMilestoneApproved" runat="server" CssClass="default" Width="100" MaxLength="10" /> <asp:ImageButton ID="imgMilestoneApproved" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calendar.gif" /></td>
                                            <td align="right" valign="top" rowspan="4"><asp:CheckBox ID="chkComplete" runat="server" CssClass="default" Text="Completed" /></td>
                                        </tr>
                                        <tr>
                                            <td nowrap><b>Forecasted Date:</b><font class="required">&nbsp;*</font></td>
                                            <td nowrap><asp:TextBox ID="txtMilestoneForecasted" runat="server" CssClass="default" Width="100" MaxLength="10" /> <asp:ImageButton ID="imgMilestoneForecasted" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calendar.gif" /></td>
                                        </tr>
                                        <tr>
                                            <td nowrap><b>Milestone:</b><font class="required">&nbsp;*</font></td>
                                            <td nowrap colspan="2"><asp:TextBox ID="txtMilestone" runat="server" CssClass="default" Width="400" MaxLength="100" /></td>
                                        </tr>
                                        <tr>
                                            <td nowrap valign="top"><b>Description:</b></td>
                                            <td nowrap colspan="2"><asp:TextBox ID="txtDetail" runat="server" CssClass="default" Width="500" TextMode="MultiLine" Rows="7" /></td>
                                        </tr>
                                    </table>
                                    <br /><br />
                                    <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                        <tr>
                                            <td>
                                                <table width="100%" cellpadding="3" cellspacing="0" border="0" style="border:solid 1px #CCCCCC">
                                                    <tr bgcolor="#EEEEEE">
                                                        <td>&nbsp;</td>
                                                        <td>&nbsp;</td>
                                                        <td nowrap><b>Approved Date</b></td>
                                                        <td>&nbsp;</td>
                                                        <td nowrap><b>Forecasted Date</b></td>
                                                        <td>&nbsp;</td>
                                                        <td nowrap><b>Milestone</b></td>
                                                        <td nowrap><b>Completed</b></td>
                                                    </tr>
                                                    <asp:repeater ID="rptMilestones" runat="server">
                                                        <ItemTemplate>
                                                            <tr class="default">
                                                                <td nowrap>[<a href="javascript:void(0);" onclick="OpenWindow('MILESTONE','<%# DataBinder.Eval(Container.DataItem, "id") %>');">Edit</a>]</td>
                                                                <td>&nbsp;</td>
                                                                <td nowrap><%# DateTime.Parse(DataBinder.Eval(Container.DataItem, "approved").ToString()).ToShortDateString() %></td>
                                                                <td>&nbsp;</td>
                                                                <td nowrap><%# DateTime.Parse(DataBinder.Eval(Container.DataItem, "forecasted").ToString()).ToShortDateString() %></td>
                                                                <td>&nbsp;</td>
                                                                <td width="100%"><%# DataBinder.Eval(Container.DataItem, "milestone") %></td>
                                                                <td nowrap align="center"><img src='/images/<%# DataBinder.Eval(Container.DataItem, "complete").ToString() == "1" ? "check" : "cancel" %>.gif' border='0' align='absmiddle' /></td>
                                                            </tr>
                                                        </ItemTemplate>
                                                    </asp:repeater>
                                                <tr>
                                                    <td colspan="8">
                                                        <asp:Label ID="lblNoMilestone" runat="server" CssClass="default" Visible="false" Text="<img src='/images/alert.gif' border='0' align='absmiddle'> There are no milestones" />
                                                    </td>
                                                </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
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
<asp:HiddenField ID="hdnExecutive" runat="server" />
<asp:HiddenField ID="hdnWorking" runat="server" />
<asp:HiddenField ID="hdnType" runat="server" />
<asp:HiddenField ID="hdnPhase" runat="server" />
<asp:HiddenField ID="hdnCosts" runat="server" />
<asp:HiddenField ID="hdnSegment" runat="server" />
<asp:Label ID="lblDiscoveryHRs" runat="server" Visible="false" />
<asp:Label ID="lblPlanningHRs" runat="server" Visible="false" />
<asp:Label ID="lblExecutionHRs" runat="server" Visible="false" />
<asp:Label ID="lblClosingHRs" runat="server" Visible="false" />