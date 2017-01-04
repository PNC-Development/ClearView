<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="project_request_edit.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.project_request_edit" %>


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
    function ChangeFrame(oCell, oShow, oHide1, oHide2, oHide3, oHide4) {
        oShow = document.getElementById(oShow);
        oShow.style.display = "inline";
        oHide1 = document.getElementById(oHide1);
        oHide1.style.display = "none";
        oHide2 = document.getElementById(oHide2);
        oHide2.style.display = "none";
        oHide3 = document.getElementById(oHide3);
        oHide3.style.display = "none";
        oHide4 = document.getElementById(oHide4);
        oHide4.style.display = "none";
    	var oRow = oCell.parentElement;
	    for (var yy=0; yy<oRow.children.length; yy++) {
    		var oNot = oRow.getElementsByTagName("td").item(yy);
    		if (oNot.className == "cmbutton")
                oNot.style.border = "1px solid #94a6b5"
    	}
	    oCell.style.borderTop = "3px solid orange"
        oCell.style.borderBottom = "1px solid #FFFFFF"
    }
    function ShowHideDivCheck(oDiv, oCheck) {
        oDiv = document.getElementById(oDiv);
        if (oCheck.checked == true)
            oDiv.style.display = "inline";
        else
            oDiv.style.display = "none";
    }
    function InitiativeIn(oText) {
        if (trim(oText.value) == "(Number of Devices) (Description of Project/Problem)")
            oText.value = "";
    }
    function InitiativeOut(oText) {
        if (trim(oText.value) == "")
            oText.value = "(Number of Devices) (Description of Project/Problem)";
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
            <br />
            <asp:Panel ID="panError" runat="server" Visible="false">
            <table width="100%" cellpadding="2" cellspacing="2" border="0">
                <tr>
                    <td align="left" class="header">
                        <img src="/images/spacer.gif" border="0" width="15" height="1" /><img src="/images/bigX.gif" border="0" align="absmiddle" /> Request Not Found
                    </td>
                </tr>
                <tr><td>&nbsp;</td></tr>
                <tr>
                    <td>There was a problem retrieving the information for the project. Most likely, this is because no project request was completed for this project.</td>
                </tr>
                <tr><td>&nbsp;</td></tr>
                <tr>
                    <td colspan="2"><hr size="1" noshade /></td>
                </tr>
                <tr>
                    <td colspan="2" align="right">
                        <table width="100%" cellpadding="0" cellspacing="0" border="0">
                            <tr>
                                <td class="footer"></td>
                                <td align="right"><asp:Button ID="btnClose" runat="server" CssClass="default" Width="75" Text="Close" OnClick="btnClose_Click" /></td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            </asp:Panel>
            <asp:Panel ID="panShow" runat="server" Visible="false">
                <table width="100%" height="100%" border="0" cellspacing="0" cellpadding="0" class="default">
	                <tr height="1">
		                <td class="cmbutton" style='<%=boolDetails == true ? "display:inline;border-bottom:1px solid #FFFFFF;border-top:3px solid orange;" : "" %>' onclick="ChangeFrame(this,'divDetails','divPriority','divDocuments','divDiscussion','divHistory');">Details</td>
		                <td class="cmbuttonspace">&nbsp;</td>
		                <td class="cmbutton" onclick="ChangeFrame(this,'divPriority','divDetails','divDocuments','divDiscussion','divHistory');">Priority</td>
		                <td class="cmbuttonspace">&nbsp;</td>
		                <td class="cmbutton" style='<%=boolDocuments == true ? "display:inline;border-bottom:1px solid #FFFFFF;border-top:3px solid orange;" : "" %>' onclick="ChangeFrame(this,'divDocuments','divDetails','divPriority','divDiscussion','divHistory');">Documents</td>
		                <td class="cmbuttonspace">&nbsp;</td>
		                <td class="cmbutton" style='<%=boolDiscussion == true ? "display:inline;border-bottom:1px solid #FFFFFF;border-top:3px solid orange;" : "" %>' onclick="ChangeFrame(this,'divDiscussion','divDetails','divPriority','divDocuments','divHistory');">Discussion</td>
		                <td class="cmbuttonspace">&nbsp;</td>
		                <td class="cmbutton" onclick="ChangeFrame(this,'divHistory','divDetails','divPriority','divDocuments','divDiscussion');">History</td>
		                <td style="border-bottom:1px solid #94a6b5;">&nbsp;</td>
	                </tr>
	                <tr>
		                <td colspan="10" align="center" class="default">
                            <table width="100%" height="100%" border="0" cellspacing="2" cellpadding="2" class="default">
                                <tr> 
                                    <td valign="top">
            		                    <div id="divDetails" style='<%=boolDetails == true ? "display:inline" : "display:none" %>'>
            		                        <br />
                                            <asp:Panel ID="panEdit" runat="server" Visible="false">
                                            <table width="100%" cellpadding="4" cellspacing="3" border="0">
                                                <tr>
                                                    <td colspan="2" align="center"><asp:Label ID="lblError" runat="server" CssClass="error" Text="<br /><img src='/images/error.gif' border='0' align='absmiddle' /> There was a problem saving the information" Visible="false" /></td>
                                                </tr>
                                                <tr>
                                                    <td>Submitter Name:</td>
                                                    <td><asp:Label ID="lblName" runat="server" CssClass="default" /></td>
                                                </tr>
                                                <tr>
                                                    <td>Submitted On:</td>
                                                    <td><asp:Label ID="lblDate" runat="server" CssClass="default" /></td>
                                                </tr>
                                                <tr>
                                                    <td>Project / Task Name:</td>
                                                    <td><asp:TextBox ID="txtProjectTask" runat="server" CssClass="default" Width="300" MaxLength="35" /></td>
                                                </tr>
                                                <tr>
                                                    <td>Initiative Type:<font class="required">&nbsp;*</font></td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlBaseDisc" runat="server" CssClass="default">
                                                            <asp:ListItem Value="Base" />
                                                            <asp:ListItem Value="Discretionary" />
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>Organization:</td>
                                                    <td><asp:DropDownList ID="ddlOrganization" runat="server" CssClass="default" Width="400" /></td>
                                                </tr>
                                                <tr>
                                                    <td>Segment:</td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlSegment" CssClass="default" runat="server" Width="400" Enabled="false" >
                                                            <asp:ListItem Value="-- Please select an Organization --" />
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>Clarity / Project Number:<font class="required">&nbsp;*</font></td>
                                                    <td><asp:TextBox ID="txtClarityNumber" runat="server" CssClass="default" Width="150" MaxLength="20" /></td>
                                                </tr>
                                                <tr>
                                                    <td>Project Status:</td>
                                                    <td><asp:Label ID="lblStatus" runat="server" CssClass="default" /></td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2" align="right">
                                                        <asp:Panel ID="panActions2" runat="server" Visible="false">
                                                            <asp:Button ID="btnShelf2" runat="server" CssClass="default" Width="125" Text="Hold Project" OnClick="btnShelf_Click" /> 
                                                            <asp:Button ID="btnCancel2" runat="server" CssClass="default" Width="125" Text="Cancel Project" OnClick="btnCancel_Click" />
                                                        </asp:Panel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2"><hr size="1" noshade /></td>
                                                </tr>
                                                <tr>
                                                    <td>Executive Sponsor:</td>
                                                    <td>
                                                        <table cellpadding="0" cellspacing="0" border="0">
                                                            <tr>
                                                                <td><asp:TextBox ID="txtExecutive" runat="server" Width="300" CssClass="default" /></td>
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
                                                    <td class="footer">&nbsp;</td>
                                                    <td class="footer">(Please enter a valid LAN ID)</td>
                                                </tr>
                                                <tr>
                                                    <td>Working Sponsor:</td>
                                                    <td>
                                                        <table cellpadding="0" cellspacing="0" border="0">
                                                            <tr>
                                                                <td><asp:TextBox ID="txtWorking" runat="server" Width="300" CssClass="default" /></td>
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
                                                    <td class="footer">&nbsp;</td>
                                                    <td class="footer">(Please enter a valid LAN ID)</td>
                                                </tr>
                                                <tr>
                                                    <td valign="top">Require a C1:<font class="required">&nbsp;*</font></td>
                                                    <td><asp:CheckBox ID="chkC1" runat="server" CssClass="default" Width="300" /></td>
                                                </tr>
                                                <tr>
                                                    <td>End Life:<font class="required">&nbsp;*</font></td>
                                                    <td><asp:CheckBox ID="chkEndLife" runat="server" /></td>
                                                </tr>
                                                <tr id="divEndLife" runat="server" style="display:none">
                                                    <td>End Life Date:<font class="required">&nbsp;*</font></td>
                                                    <td><asp:TextBox ID="txtEndLife" runat="server" CssClass="default" Width="100" MaxLength="10" /> <asp:ImageButton ID="imgEndLife" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calendar.gif" /></td>
                                                </tr>
                                                <tr>
                                                    <td>Upload Case Study:</td>
                                                    <td><img src="/images/upload.gif" border="0" align="absmiddle" /> <asp:LinkButton ID="btnDocuments" runat="server" Text="Click Here to Upload" /></td>
                                                </tr>
                                                <tr>
                                                    <td valign="top">Initiative Opportunity:<font class="required">&nbsp;*</font></td>
                                                    <td><asp:TextBox ID="txtInitiative" runat="server" CssClass="default" Width="300" TextMode="multiline" Rows="5" /></td>
                                                </tr>
                                                <tr>
                                                    <td valign="top">Technology Capability:</td>
                                                    <td><asp:TextBox ID="txtCapability" runat="server" CssClass="default" Width="300" TextMode="multiline" Rows="3" /></td>
                                                </tr>
                                                <tr>
                                                    <td valign="top"><br />Platform(s):</td>
                                                    <td>
                                                        <table width="100%" cellpadding="2" cellspacing="0" border="0">
                                                            <tr>
                                                                <td class="default">Selected:</td>
                                                                <td class="default">&nbsp;</td>
                                                                <td class="default">Available:</td>
                                                            </tr>
                                                            <tr>
                                                                <td width="50%"><asp:ListBox ID="lstPlatformsCurrent" runat="server" Width="100%" CssClass="default" Rows="5" /></td>
                                                                <td>
                                                                    <asp:ImageButton ID="btnPlatformAdd" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/lt.gif" ToolTip="Add" /><br /><br />
                                                                    <asp:ImageButton ID="btnPlatformRemove" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/rt.gif" ToolTip="Remove" />
                                                                </td>
                                                                <td width="50%"><asp:ListBox ID="lstPlatformsAvailable" runat="server" Width="100%" CssClass="default" Rows="5" /></td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>Is this an Audit Requirement:<font class="required">&nbsp;*</font></td>
                                                    <td><asp:CheckBox ID="chkRequirement" runat="server" /></td>
                                                </tr>
                                                <tr id="divRequirement" runat="server" style="display:none">
                                                    <td>Requirement Date:<font class="required">&nbsp;*</font></td>
                                                    <td><asp:TextBox ID="txtRequirement" runat="server" CssClass="default" Width="100" MaxLength="10" /> <asp:ImageButton ID="imgRequirement" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calendar.gif" /></td>
                                                </tr>
                                                <tr>
                                                    <td nowrap>Interdependency With Other Projects/Initiatives:<font class="required">&nbsp;*</font></td>
                                                    <td width="100%">
                                                        <asp:DropDownList ID="ddlInterdependency" runat="server" CssClass="default">
                                                            <asp:ListItem Value="0" Text="-- SELECT --" />
                                                            <asp:ListItem Value="1" Text="None" />
                                                            <asp:ListItem Value="3" Text="Pre (Must be done prior to other effort)" />
                                                            <asp:ListItem Value="5" Text="Post (Must be done after other effort)" />
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr id="divInterdependency" runat="server" style="display:none">
                                                    <td>Project Name(s):</td>
                                                    <td><asp:TextBox ID="txtInterdependency" runat="server" CssClass="default" Width="200" MaxLength="200" /></td>
                                                </tr>
                                                <tr>
                                                    <td>Estimated Labor Hours:<font class="required">&nbsp;*</font></td>
                                                    <td><asp:TextBox ID="txtHours" runat="server" CssClass="default" Width="50" MaxLength="10" /></td>
                                                </tr>
                                                <tr>
                                                    <td>Proposed Discovery Start Date:</td>
                                                    <td><asp:TextBox ID="txtStart" runat="server" CssClass="default" Width="100" MaxLength="10" /> <asp:ImageButton ID="imgStart" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calendar.gif" /></td>
                                                </tr>
                                                <tr>
                                                    <td>Estimated Project Completion Date:</td>
                                                    <td><asp:TextBox ID="txtCompletion" runat="server" CssClass="default" Width="100" MaxLength="10" /> <asp:ImageButton ID="imgCompletion" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calendar.gif" /></td>
                                                </tr>
                                                <tr>
                                                    <td>Expected Capital Cost:<font class="required">&nbsp;*</font></td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlCapital" runat="server" CssClass="default">
                                                            <asp:ListItem Value="0" Text="-- SELECT --" />
                                                            <asp:ListItem Value="1" Text="Greater than $750,000" />
                                                            <asp:ListItem Value="3" Text="$350,000 - $750,000" />
                                                            <asp:ListItem Value="5" Text="Less than $350,000" />
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="footer">&nbsp;</td>
                                                    <td class="footer">(Expected capital cost of the project)</td>
                                                </tr>
                                                <tr>
                                                    <td>Internal Labor:<font class="required">&nbsp;*</font></td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlInternal" runat="server" CssClass="default">
                                                            <asp:ListItem Value="0" Text="-- SELECT --" />
                                                            <asp:ListItem Value="-1" Text="Greater than $1,000,000" />
                                                            <asp:ListItem Value="1" Text="$150,000 - $1,000,000" />
                                                            <asp:ListItem Value="3" Text="$50,000 - $150,000" />
                                                            <asp:ListItem Value="5" Text="Less than $50,000" />
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="footer">&nbsp;</td>
                                                    <td class="footer">(Cost of internal labor required for the project)</td>
                                                </tr>
                                                <tr>
                                                    <td>External Labor:<font class="required">&nbsp;*</font></td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlExternal" runat="server" CssClass="default">
                                                            <asp:ListItem Value="0" Text="-- SELECT --" />
                                                            <asp:ListItem Value="-1" Text="Greater than $1,000,000" />
                                                            <asp:ListItem Value="1" Text="$150,000 - $1,000,000" />
                                                            <asp:ListItem Value="3" Text="$50,000 - $150,000" />
                                                            <asp:ListItem Value="5" Text="Less than $50,000" />
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="footer">&nbsp;</td>
                                                    <td class="footer">(Cost of external labor required for the project)</td>
                                                </tr>
                                                <tr>
                                                    <td>Maintenance Cost Increase:<font class="required">&nbsp;*</font></td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlMaintenance" runat="server" CssClass="default">
                                                            <asp:ListItem Value="0" Text="-- SELECT --" />
                                                            <asp:ListItem Value="1" Text="Greater than $500,000" />
                                                            <asp:ListItem Value="3" Text="$1 - $500,000" />
                                                            <asp:ListItem Value="5" Text="None" />
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="footer">&nbsp;</td>
                                                    <td class="footer">(As a result of this project, the increase in the existing maintenance cost)</td>
                                                </tr>
                                                <tr>
                                                    <td>Project Expenses:<font class="required">&nbsp;*</font>&nbsp;<a href="javascript:void(0);" onclick="OpenWindow('TEMPHELP','/help/project_request.htm#PE');"><img src="/images/help.gif" border="0" align="absmiddle" /></a></td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlExpenses" runat="server" CssClass="default">
                                                            <asp:ListItem Value="0" Text="-- SELECT --" />
                                                            <asp:ListItem Value="1" Text="Not Budgeted" />
                                                            <asp:ListItem Value="3" Text="Indirectly Budgeted" />
                                                            <asp:ListItem Value="5" Text="Directly Budgeted" />
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="footer">&nbsp;</td>
                                                    <td class="footer">(Are the expected expenses budgeted for this project?)</td>
                                                </tr>
                                                <tr>
                                                    <td>Estimated Net Cost Avoidance:<font class="required">&nbsp;*</font></td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlCostAvoidance" runat="server" CssClass="default">
                                                            <asp:ListItem Value="0" Text="-- SELECT --" />
                                                            <asp:ListItem Value="1" Text="Less than $50,000" />
                                                            <asp:ListItem Value="3" Text="$50,000 - $150,000" />
                                                            <asp:ListItem Value="5" Text="Greater than $150,000" />
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="footer">&nbsp;</td>
                                                    <td class="footer">(Overall cost aviodance by implementing this project/initiative)</td>
                                                </tr>
                                                <tr>
                                                    <td>Estimated Net Cost Savings:<font class="required">&nbsp;*</font></td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlSavings" runat="server" CssClass="default">
                                                            <asp:ListItem Value="0" Text="-- SELECT --" />
                                                            <asp:ListItem Value="1" Text="Less than $50,000" />
                                                            <asp:ListItem Value="3" Text="$50,000 - $150,000" />
                                                            <asp:ListItem Value="5" Text="Greater than $150,000" />
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="footer">&nbsp;</td>
                                                    <td class="footer">(Overall cost savings by implemneting this project/initiative)</td>
                                                </tr>
                                                <tr>
                                                    <td>Realized Cost Savings:<font class="required">&nbsp;*</font></td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlRealized" runat="server" CssClass="default">
                                                            <asp:ListItem Value="0" Text="-- SELECT --" />
                                                            <asp:ListItem Value="1" Text="Greater than 12 Months" />
                                                            <asp:ListItem Value="3" Text="6 - 12 Months" />
                                                            <asp:ListItem Value="5" Text="Less than 6 Months" />
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="footer">&nbsp;</td>
                                                    <td class="footer">(Describe the timeframe that the cost savings will be realized after implementation)</td>
                                                </tr>
                                                <tr>
                                                    <td>Business Impact Aviodance:<font class="required">&nbsp;*</font>&nbsp;<a href="javascript:void(0);" onclick="OpenWindow('TEMPHELP','/help/project_request.htm#BIA');"><img src="/images/help.gif" border="0" align="absmiddle" /></a></td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlBusinessAvoidance" runat="server" CssClass="default">
                                                            <asp:ListItem Value="0" Text="-- SELECT --" />
                                                            <asp:ListItem Value="1" Text="None" />
                                                            <asp:ListItem Value="3" Text="Loss of Functionality" />
                                                            <asp:ListItem Value="5" Text="Loss of Business" />
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="footer">&nbsp;</td>
                                                    <td class="footer">(Describe the business impact avoidance by implementing this project/initiative)</td>
                                                </tr>
                                                <tr>
                                                    <td>Maintenance Cost Avoidance:<font class="required">&nbsp;*</font></td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlMaintenanceAvoidance" runat="server" CssClass="default">
                                                            <asp:ListItem Value="0" Text="-- SELECT --" />
                                                            <asp:ListItem Value="1" Text="None" />
                                                            <asp:ListItem Value="3" Text="$1 - $500,000" />
                                                            <asp:ListItem Value="5" Text="Greater than $500,000" />
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="footer">&nbsp;</td>
                                                    <td class="footer">(As a result of this project, will future maintenance costs be avoided or reduced)</td>
                                                </tr>
                                                <tr>
                                                    <td>Asset Reusability:<font class="required">&nbsp;*</font></td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlReusability" runat="server" CssClass="default">
                                                            <asp:ListItem Value="0" Text="-- SELECT --" />
                                                            <asp:ListItem Value="1" Text="Not leveraging existing, nor introducing reusable assets" />
                                                            <asp:ListItem Value="3" Text="Leveraging existing assets or introducting reusable assets" />
                                                            <asp:ListItem Value="5" Text="Introducing and leveraging high reusable assets" />
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="footer">&nbsp;</td>
                                                    <td class="footer">(Describe the level of reusability of the assets introduced or leveraging of existing assets by this project)</td>
                                                </tr>
                                                <tr>
                                                    <td>Internal Customer Impact:<font class="required">&nbsp;*</font></td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlInternalImpact" runat="server" CssClass="default">
                                                            <asp:ListItem Value="0" Text="-- SELECT --" />
                                                            <asp:ListItem Value="1" Text="Negative" />
                                                            <asp:ListItem Value="3" Text="None" />
                                                            <asp:ListItem Value="5" Text="Positive" />
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="footer">&nbsp;</td>
                                                    <td class="footer">(Describe the impact this project has on internal customers)</td>
                                                </tr>
                                                <tr>
                                                    <td>External Customer Impact:<font class="required">&nbsp;*</font></td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlExternalImpact" runat="server" CssClass="default">
                                                            <asp:ListItem Value="0" Text="-- SELECT --" />
                                                            <asp:ListItem Value="1" Text="Negative" />
                                                            <asp:ListItem Value="3" Text="None" />
                                                            <asp:ListItem Value="5" Text="Positive" />
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="footer">&nbsp;</td>
                                                    <td class="footer">(Describe the impact this project has on external customers)</td>
                                                </tr>
                                                <tr>
                                                    <td>Business Impact:<font class="required">&nbsp;*</font>&nbsp;<a href="javascript:void(0);" onclick="OpenWindow('TEMPHELP','/help/project_request.htm#BI');"><img src="/images/help.gif" border="0" align="absmiddle" /></a></td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlImpact" runat="server" CssClass="default">
                                                            <asp:ListItem Value="0" Text="-- SELECT --" />
                                                            <asp:ListItem Value="1" Text="None" />
                                                            <asp:ListItem Value="3" Text="Loss of Functionality" />
                                                            <asp:ListItem Value="5" Text="Loss of Business" />
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="footer">&nbsp;</td>
                                                    <td class="footer">(Describe the impact if this change/project does not move forward)</td>
                                                </tr>
                                                <tr>
                                                    <td>Strategic Opportunity:<font class="required">&nbsp;*</font>&nbsp;<a href="javascript:void(0);" onclick="OpenWindow('TEMPHELP','/help/project_request.htm#SO');"><img src="/images/help.gif" border="0" align="absmiddle" /></a></td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlStrategic" runat="server" CssClass="default">
                                                            <asp:ListItem Value="0" Text="-- SELECT --" />
                                                            <asp:ListItem Value="1" Text="Does not support" />
                                                            <asp:ListItem Value="3" Text="Indirectly supports" />
                                                            <asp:ListItem Value="5" Text="Directly supports" />
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="footer">&nbsp;</td>
                                                    <td class="footer">(Describe the measure of alignment this project has to the corporate and technical strategic initiatives)</td>
                                                </tr>
                                                <tr>
                                                    <td>Acquisition / BIC:<font class="required">&nbsp;*</font>&nbsp;<a href="javascript:void(0);" onclick="OpenWindow('TEMPHELP','/help/project_request.htm#AB');"><img src="/images/help.gif" border="0" align="absmiddle" /></a></td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlAcquisition" runat="server" CssClass="default">
                                                            <asp:ListItem Value="0" Text="-- SELECT --" />
                                                            <asp:ListItem Value="1" Text="Does not support" />
                                                            <asp:ListItem Value="3" Text="Indirectly supports" />
                                                            <asp:ListItem Value="5" Text="Directly supports" />
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="footer">&nbsp;</td>
                                                    <td class="footer">(Describe if the project/initiative supports and acquisition or best in class project)</td>
                                                </tr>
                                                <tr>
                                                    <td>Technology Capabilities:<font class="required">&nbsp;*</font></td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlCapabilities" runat="server" CssClass="default">
                                                            <asp:ListItem Value="0" Text="-- SELECT --" />
                                                            <asp:ListItem Value="1" Text="Reduce (Single point solution)" />
                                                            <asp:ListItem Value="3" Text="Maintain" />
                                                            <asp:ListItem Value="5" Text="Increase (Loosely coupled solution)" />
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="footer">&nbsp;</td>
                                                    <td class="footer">(Choose the type of influence this project will have on the existing technology capabilities)</td>
                                                </tr>
                                                <tr>
                                                    <td>Technical Project Manager Requested:</td>
                                                    <td><asp:Label ID="lblTPM" runat="server" CssClass="default" /></td>
                                                </tr>
                                                <asp:Panel ID="panManagerTPM" runat="server" Visible="false">
                                                <tr>
                                                    <td>Service Type:<font class="required">&nbsp;*</font></td>
                                                    <td><asp:DropDownList ID="ddlTPM" runat="server" CssClass="default" /></td>
                                                </tr>
                                                </asp:Panel>
                                                <asp:Panel ID="panManagerPM" runat="server" Visible="false">
                                                <tr>
                                                    <td nowrap>Project Lead:</td>
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
                                                </asp:Panel>
                                                <tr>
                                                    <td colspan="2"><hr size="1" noshade /></td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2" align="right">
                                                        <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                                            <tr>
                                                                <td class="required">* = Modifying the contents of this field will restart the approval process</td>
                                                                <td align="right"><asp:Button ID="btnSave" runat="server" CssClass="default" Width="75" Text="Update" OnClick="btnSave_Click" /></td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                            </asp:Panel>
                                            <asp:Panel ID="panView" runat="server" Visible="false">
                                            <table width="100%" cellpadding="4" cellspacing="3" border="0">
                                                <asp:Panel ID="panInfo" runat="server" Visible="false">
                                                <tr>
                                                    <td colspan="2" align="center"><img src='/images/bigInfo.gif' border='0' align='absmiddle' /> <b>You have read permission to this request</b></td>
                                                </tr>
                                                </asp:Panel>
                                                <tr>
                                                    <td>Submitter Name:</td>
                                                    <td><asp:Label ID="lblName2" runat="server" CssClass="default" /></td>
                                                </tr>
                                                <tr>
                                                    <td>Submitted On:</td>
                                                    <td><asp:Label ID="lblDate2" runat="server" CssClass="default" /></td>
                                                </tr>
                                                <tr>
                                                    <td>Project / Task Name:</td>
                                                    <td><asp:Label ID="lblProjectTask2" runat="server" CssClass="default" /></td>
                                                </tr>
                                                <tr>
                                                    <td>Initiative Type:</td>
                                                    <td><asp:Label ID="lblBaseDisc2" runat="server" CssClass="default" /></td>
                                                </tr>
                                                <tr>
                                                    <td>Organization:</td>
                                                    <td><asp:Label ID="lblOrganization2" runat="server" CssClass="default" /></td>
                                                </tr>
                                                <tr>
                                                    <td>Segment:</td>
                                                    <td><asp:Label ID="lblSegment2" runat="server" CssClass="default" /></td>
                                                </tr>
                                                <tr>
                                                    <td>Clarity / Project Number:</td>
                                                    <td><asp:Label ID="lblClarity2" runat="server" CssClass="default" /></td>
                                                </tr>
                                                <tr>
                                                    <td>Project Status:</td>
                                                    <td><asp:Label ID="lblStatus2" runat="server" CssClass="default" /></td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2" align="right">
                                                        <asp:Panel ID="panActions" runat="server" Visible="false">
                                                            <asp:Button ID="btnShelf" runat="server" CssClass="default" Width="125" Text="Hold Project" OnClick="btnShelf_Click" /> 
                                                            <asp:Button ID="btnCancel" runat="server" CssClass="default" Width="125" Text="Cancel Project" OnClick="btnCancel_Click" />
                                                        </asp:Panel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2"><hr size="1" noshade /></td>
                                                </tr>
                                                <tr>
                                                    <td>Executive Sponsor:</td>
                                                    <td><asp:Label ID="lblExecutive2" runat="server" CssClass="default" /></td>
                                                </tr>
                                                <tr>
                                                    <td>Working Sponsor:</td>
                                                    <td><asp:Label ID="lblWorking2" runat="server" CssClass="default" /></td>
                                                </tr>
                                                <tr>
                                                    <td valign="top">Require a C1:</td>
                                                    <td><asp:Label ID="lblC1" runat="server" CssClass="default" /></td>
                                                </tr>
                                                <tr>
                                                    <td>End Life:</td>
                                                    <td><asp:Label ID="lblEndLife" runat="server" CssClass="default" /></td>
                                                </tr>
                                                <tr>
                                                    <td nowrap><img src="/images/spacer.gif" border="0" width="20" height="1" /><img src="/images/downright.gif" border="0" align="absmiddle" /> End Life Date:</td>
                                                    <td><asp:Label ID="lblEndLifeDate" runat="server" CssClass="default" /></td>
                                                </tr>
                                                <tr>
                                                    <td valign="top">Initiative Opportunity:</td>
                                                    <td><asp:Label ID="lblInitiative" runat="server" CssClass="default" /></td>
                                                </tr>
                                                <tr>
                                                    <td valign="top">Platform(s):</td>
                                                    <td><asp:Label ID="lblPlatforms" runat="server" CssClass="default" /></td>
                                                </tr>
                                                <tr>
                                                    <td>Is this an Audit Requirement:</td>
                                                    <td><asp:Label ID="lblRequirement" runat="server" CssClass="default" /></td>
                                                </tr>
                                                <tr>
                                                    <td nowrap><img src="/images/spacer.gif" border="0" width="20" height="1" /><img src="/images/downright.gif" border="0" align="absmiddle" /> Requirement Date:</td>
                                                    <td><asp:Label ID="lblRequirementDate" runat="server" CssClass="default" /></td>
                                                </tr>
                                                <tr>
                                                    <td nowrap>Interdependency With Other Projects/Initiatives:</td>
                                                    <td width="100%"><asp:Label ID="lblInterdependency" runat="server" CssClass="default" /></td>
                                                </tr>
                                                <tr>
                                                    <td nowrap><img src="/images/spacer.gif" border="0" width="20" height="1" /><img src="/images/downright.gif" border="0" align="absmiddle" /> Project Name(s):</td>
                                                    <td><asp:Label ID="lblProjects" runat="server" CssClass="default" /></td>
                                                </tr>
                                                <tr>
                                                    <td valign="top">Technology Capability:</td>
                                                    <td><asp:Label ID="lblCapability" runat="server" CssClass="default" /></td>
                                                </tr>
                                                <tr>
                                                    <td>Estimated Man Hours:</td>
                                                    <td><asp:Label ID="lblHours" runat="server" CssClass="default" /></td>
                                                </tr>
                                                <tr>
                                                    <td>Proposed Discovery Start Date:</td>
                                                    <td><asp:Label ID="lblStart" runat="server" CssClass="default" /></td>
                                                </tr>
                                                <tr>
                                                    <td>Expected Project Completion Date:</td>
                                                    <td><asp:Label ID="lblCompletion" runat="server" CssClass="default" /></td>
                                                </tr>
                                                <tr>
                                                    <td>Expected Capital Cost:</td>
                                                    <td><asp:Label ID="lblCapital" runat="server" CssClass="default" /></td>
                                                </tr>
                                                <tr>
                                                    <td>Internal Labor:</td>
                                                    <td><asp:Label ID="lblInternal" runat="server" CssClass="default" /></td>
                                                </tr>
                                                <tr>
                                                    <td>External Labor:</td>
                                                    <td><asp:Label ID="lblExternal" runat="server" CssClass="default" /></td>
                                                </tr>
                                                <tr>
                                                    <td>Maintenance Cost Increase:</td>
                                                    <td><asp:Label ID="lblMaintenance" runat="server" CssClass="default" /></td>
                                                </tr>
                                                <tr>
                                                    <td>Project Expenses:</td>
                                                    <td><asp:Label ID="lblExpenses" runat="server" CssClass="default" /></td>
                                                </tr>
                                                <tr>
                                                    <td>Estimated Net Cost Avoidance:</td>
                                                    <td><asp:Label ID="lblCostAvoidance" runat="server" CssClass="default" /></td>
                                                </tr>
                                                <tr>
                                                    <td>Estimated Net Cost Savings:</td>
                                                    <td><asp:Label ID="lblSavings" runat="server" CssClass="default" /></td>
                                                </tr>
                                                <tr>
                                                    <td>Realized Cost Savings:</td>
                                                    <td><asp:Label ID="lblRealized" runat="server" CssClass="default" /></td>
                                                </tr>
                                                <tr>
                                                    <td>Business Impact Aviodance:</td>
                                                    <td><asp:Label ID="lblBusinessAvoidance" runat="server" CssClass="default" /></td>
                                                </tr>
                                                <tr>
                                                    <td>Maintenance Cost Avoidance:</td>
                                                    <td><asp:Label ID="lblMaintenanceAvoidance" runat="server" CssClass="default" /></td>
                                                </tr>
                                                <tr>
                                                    <td>Asset Reusability:</td>
                                                    <td><asp:Label ID="lblReusability" runat="server" CssClass="default" /></td>
                                                </tr>
                                                <tr>
                                                    <td>Internal Customer Impact:</td>
                                                    <td><asp:Label ID="lblInternalImpact" runat="server" CssClass="default" /></td>
                                                </tr>
                                                <tr>
                                                    <td>External Customer Impact:</td>
                                                    <td><asp:Label ID="lblExternalImpact" runat="server" CssClass="default" /></td>
                                                </tr>
                                                <tr>
                                                    <td>Business Impact:</td>
                                                    <td><asp:Label ID="lblImpact" runat="server" CssClass="default" /></td>
                                                </tr>
                                                <tr>
                                                    <td>Strategic Opportunity:</td>
                                                    <td><asp:Label ID="lblStrategic" runat="server" CssClass="default" /></td>
                                                </tr>
                                                <tr>
                                                    <td>Acquisition / BIC:</td>
                                                    <td><asp:Label ID="lblAcquisition" runat="server" CssClass="default" /></td>
                                                </tr>
                                                <tr>
                                                    <td>Technology Capabilities:</td>
                                                    <td><asp:Label ID="lblCapabilities" runat="server" CssClass="default" /></td>
                                                </tr>
                                                <tr>
                                                    <td>Technical Project Manager Requested:</td>
                                                    <td><asp:Label ID="lblTPM2" runat="server" CssClass="default" /></td>
                                                </tr>
                                                <asp:Panel ID="panTPM" runat="server" Visible="false">
                                                <tr>
                                                    <td nowrap><img src="/images/spacer.gif" border="0" width="20" height="1" /><img src="/images/downright.gif" border="0" align="absmiddle" /> Service Type:</td>
                                                    <td><asp:Label ID="lblTPMService" runat="server" CssClass="default" /></td>
                                                </tr>
                                                </asp:Panel>
                                                <asp:Panel ID="panPM" runat="server" Visible="false">
                                                <tr>
                                                    <td nowrap><img src="/images/spacer.gif" border="0" width="20" height="1" /><img src="/images/downright.gif" border="0" align="absmiddle" /> Project Lead:</td>
                                                    <td><asp:Label ID="lblPM" runat="server" CssClass="default" /></td>
                                                </tr>
                                                </asp:Panel>
                                                <tr>
                                                    <td colspan="2"><hr size="1" noshade /></td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2" align="right"><asp:Label ID="lblPrinter" runat="server" CssClass="default" Text="<img src='/images/bigPrint.gif' border='0' align='absmiddle' /> <a href='javascript:window.print();'>Print Project Details</a>" /></td>
                                                </tr>
                                            </table>
                                            </asp:Panel>
            		                    </div>
            		                    <div id="divPriority" style="display:none">
            		                        <br />
            		                        <%=strPriority%>
            		                    </div>
            		                    <div id="divDocuments" style='<%=boolDocuments == true ? "display:inline" : "display:none" %>'>
            		                        <br />
                                            <table width="100%" cellpadding="2" cellspacing="1" border="0">
                                                <tr>
                                                    <td align="right"><asp:CheckBox ID="chkMyDescription" runat="server" CssClass="default" Text="Show Descriptions" AutoPostBack="true" OnCheckedChanged="chkMyDescription_Change" /></td>
                                                </tr>
                                                <tr>
                                                    <td><asp:Label ID="lblMyDocuments" runat="server" CssClass="default" /></td>
                                                </tr>
                                            </table>
            		                    </div>
            		                    <div id="divDiscussion" style='<%=boolDiscussion == true ? "display:inline" : "display:none" %>'>
            		                        <br />
                                            <table width="100%" cellpadding="3" cellspacing="2" border="0">
                                                <tr>
                                                    <td colspan="2">
                                                        <table width="100%" cellpadding="3" cellspacing="0" border="0">
                                                        <asp:repeater ID="rptComments" runat="server">
                                                            <ItemTemplate>
                                                                <tr>
                                                                    <td valign="top"><img src="/images/comment.gif" border="0" /></td>
                                                                    <td valign="top" colspan="2" width="100%"><%# DataBinder.Eval(Container.DataItem, "comment") %></td>
                                                                </tr>
                                                                <tr>
                                                                    <td>&nbsp;</td>
                                                                    <td class="comment">&nbsp;-&nbsp;<asp:Label ID="lblXID" runat="server" CssClass="comment" Text='<%# oUser.GetFullName(DataBinder.Eval(Container.DataItem, "xid").ToString()) %>' />&nbsp;:&nbsp;&nbsp;<%# DateTime.Parse(DataBinder.Eval(Container.DataItem, "modified").ToString()).ToLongDateString() %> @ <%# DateTime.Parse(DataBinder.Eval(Container.DataItem, "modified").ToString()).ToShortTimeString() %></td>
                                                                    <td class="comment" align="right"><asp:LinkButton ID="btnDelete" runat="server" OnClick="btnDelete_Click" Visible='<%# (DataBinder.Eval(Container.DataItem, "userid").ToString() == intProfile.ToString()) %>' CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' Text="[Delete Comment]" /></td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="3" height="1"><img src="/images/spacer.gif" border="0" width="1" height="1" /></td>
                                                                </tr>
                                                            </ItemTemplate>
                                                        </asp:repeater>
                                                        <tr><td colspan="3" class="bigalert" align="center"><asp:Label ID="lblNoComments" runat="server" Visible="false" Text="<img src='/images/bigalert.gif' border='0' align='absmiddle'> There are no comments" /></td></tr>
                                                        </table>
                                                    </td>
                                                </tr>
            		                        <asp:Panel ID="panDiscussion" runat="server" Visible="false">
                            		            <tr>
                            		                <td colspan="2"><span style="width:100%;border-bottom:1 dotted #999999;"/></td>
                            		            </tr>
                                                <tr>
                                                    <td colspan="2">&nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2" align="right"><asp:CheckBox ID="chkNotify" runat="server" CssClass="default" Text="Notify me of discussion posts" />&nbsp;&nbsp;&nbsp;<asp:Button ID="btnNotify" runat="server" CssClass="default" Width="75" Text="Apply" OnClick="btnNotify_Click" /></td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">&nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2" class="hugeheader">Post a Comment</td>
                                                </tr>
                                                <tr>
                                                    <td nowrap>User:</td>
                                                    <td width="100%"><asp:Label ID="lblUser" runat="server" CssClass="default" /></td>
                                                </tr>
                                                <tr>
                                                    <td nowrap valign="top">Comment:</td>
                                                    <td width="100%"><asp:TextBox ID="txtInternal" runat="server" CssClass="default" TextMode="MultiLine" Rows="5" Width="400" /></td>
                                                </tr>
                                                <tr>
                                                    <td>&nbsp;</td>
                                                    <td><asp:Button ID="btnInternal" runat="server" CssClass="default" Width="125" Text="Post Comment" OnClick="btnInternal_Click" /></td>
                                                </tr>
            		                        </asp:Panel>
                                            </table>
            		                    </div>
            		                    <div id="divHistory" style="display:none">
            		                        <br />
                                            <table width="100%" cellpadding="2" cellspacing="1" border="0">
                                                <tr>
                                                    <td>
                                                        <asp:repeater ID="rptHistory" runat="server">
                                                            <HeaderTemplate>
                                                                <table width="100%" cellpadding="3" cellspacing="0" border="0" style="border:solid 1px #CCCCCC">
                                                                    <tr bgcolor="#EEEEEE">
                                                                        <td nowrap width="1">&nbsp;</td>
                                                                        <td nowrap><b>Status</b></td>
                                                                        <td nowrap><b>Level</b></td>
                                                                        <td nowrap><b>Assigned User</b></td>
                                                                        <td nowrap><b>Updated</b></td>
                                                                    </tr>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <tr class="default">
                                                                    <td width="1"><asp:Label ID="lblImage" runat="server" CssClass="default" Text='' /></td>
                                                                    <td><asp:Label ID="lblApproval" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "approval") %>' /></td>
                                                                    <td><asp:Label ID="lblStep" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "step") %>' /></td>
                                                                    <td><asp:Label ID="lblUserId" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "userid") %>' /></td>
                                                                    <td><asp:Label ID="lblModified" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "modified") %>' /></td>
                                                                </tr>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                </table>
                                                            </FooterTemplate>
                                                        </asp:repeater>
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
            <asp:Panel ID="panFinish" runat="server" Visible="false">
                <br />
            <table width="100%" cellpadding="2" cellspacing="2" border="0">
                <tr>
                    <td align="left" class="header"><img src="/images/spacer.gif" border="0" width="15" height="1" /><img src="/images/bigCheck.gif" border="0" align="absmiddle" /> <asp:Label ID="lblThanks" runat="server" /></td>
                </tr>
                <tr><td>&nbsp;</td></tr>
                <tr>
                    <td>Your information has been saved successfully. An email has been sent to your mailbox with additional information regarding the next steps in the process. Please read this information carefully.</td>
                </tr>
                <tr><td>&nbsp;</td></tr>
                <tr><td align="center" class="result">Your project request number is <b><asp:Label id="lblRequest" CssClass="result" runat="server" /></b>.</td></tr>
                <tr><td>&nbsp;</td></tr>
                <tr>
                    <td align="center" class="bigalert"><asp:label ID="lblResent" runat="server" Text="<img src='/images/bigAlert.gif' border='0' align='absmiddle' /> The approval process has been restarted!" Visible="false" CssClass="header" /></td>
                </tr>
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
            <asp:Panel ID="panAction" runat="server" Visible="false">
                <br />
            <table width="100%" cellpadding="2" cellspacing="2" border="0">
                <tr>
                    <td align="left" class="header"><img src="/images/spacer.gif" border="0" width="15" height="1" /><img src="/images/bigCheck.gif" border="0" align="absmiddle" /> <asp:Label ID="lblAction" runat="server" /></td>
                </tr>
                <tr><td>&nbsp;</td></tr>
                <tr>
                    <td><asp:Label ID="lblSubAction" runat="server" CssClass="default" /></td>
                </tr>
                <tr><td>&nbsp;</td></tr>
                <tr>
                    <td colspan="2"><hr size="1" noshade /></td>
                </tr>
                <tr>
                    <td colspan="2" align="right">
                        <table width="100%" cellpadding="0" cellspacing="0" border="0">
                            <tr>
                                <td class="footer"></td>
                                <td align="right"><asp:Button ID="btnAction" runat="server" CssClass="default" Width="75" Text="Close" OnClick="btnClose_Click" /></td>
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
<asp:HiddenField ID="hdnExecutive" runat="server" />
<asp:HiddenField ID="hdnWorking" runat="server" />
<asp:HiddenField ID="hdnPlatforms" runat="server" />
<asp:HiddenField ID="hdnError" runat="server" />
<asp:Label ID="lblResourceRequest" runat="server" Visible="false" />
<asp:Label ID="lblCoordinator" runat="server" Visible="false" />
<asp:HiddenField ID="hdnManager" runat="server" />
<asp:HiddenField ID="hdnSegment" runat="server" />
