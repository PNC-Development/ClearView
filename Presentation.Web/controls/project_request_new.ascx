<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="project_request_new.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.project_request_new" %>


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
function EnsureInitiative(oText) {
	oText = document.getElementById(oText);
	if (oText.value == "(Number of Devices) (Description of Project/Problem)")
	{
	    alert('Please enter an initiative');
	    oText.focus();
	    return false;
	}
	return true;
}
function UpdateHidden(oHidden, oControl) {
	var oHidden = document.getElementById(oHidden);
	oHidden.value = "";
	for (var ii=0; ii<oControl.length; ii++) {
		oHidden.value = oHidden.value + oControl.options[ii].value + "_" + ii + "&";
	}
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
            <asp:Panel ID="panIntro" runat="server" Visible="false">
            <table width="100%" cellpadding="0" cellspacing="5" border="0">
                <tr>
                    <td colspan="2" title="To avoid duplicate project entries use the eclipse button to search for existing projects.">What is the name of your project?<font class="required">&nbsp;*</font></td>
                </tr>
                <tr>
                    <td title="To avoid duplicate project entries use the eclipse button to search for existing projects."><img src="/images/spacer.gif" border="0" width="15" height="1" /></td>
                    <td title="To avoid duplicate project entries use the eclipse button to search for existing projects."><asp:TextBox ID="txtProjectTask" ToolTip="To avoid duplicate project entries use the eclipse button to search for existing projects." runat="server" CssClass="default" Width="300" MaxLength="35" /> <asp:Button ID="btnPName" runat="server" CssClass="default" Text="..." Width="25" tooltip="To avoid duplicate project entries use the eclipse button to search for existing projects." /></td>
                </tr>
                <tr>
                    <td colspan="2" style="height: 5px"><img src="/images/spacer.gif" border="0" width="1" height="5" /></td>
                </tr>
                <tr>
                    <td colspan="2" title="If your initative is considerd discretionary project DO NOT USE THIS FORM">Is your initiative considered Base or Discretionary?<font class="required">&nbsp;*</font></td>
                </tr>
                <tr>
                    <td style="visibility: visible;" title="If your initative is considerd discretionary project                DO NOT USE THIS FORM"><img src="/images/spacer.gif" border="0" width="15" height="1"/></td>
                    <td title="If your initative is considerd discretionary project DO NOT USE THIS FORM">
                        <asp:DropDownList ID="ddlBaseDisc" runat="server" CssClass="default" ToolTip="If your initative is considerd discretionary project DO NOT USE THIS FORM">
                            <asp:ListItem Value="0" Text="-- SELECT --" />
                            <asp:ListItem Value="1" Text="Base" />
                            <asp:ListItem Value="2" Text="Discretionary" />
                        </asp:DropDownList></td>
                </tr>
                <tr>
                    <td colspan="2" height="5"><img src="/images/spacer.gif" border="0" width="1" height="5" tooltip="To avoid duplicate project entries use the eclipse button to search for existing projects."/></td>
                </tr>
                <tr>
                    <td colspan="2" style="height: 18px" title="Select the organization sponsoring this initiative. Note: this could be your organization.">What organization is sponsoring this initiative?<font class="required">&nbsp;*</font></td>
                </tr>
                <tr>
                    <td title="Select the organization sponsoring this initiative. Note: this could be your organization."><img src="/images/spacer.gif" border="0" width="15" height="1" /></td>
                    <td title="Select the organization sponsoring this initiative. Note: this could be your organization."><asp:DropDownList ID="ddlOrganization" runat="server" CssClass="default" Width="400" ToolTip="Select the organization sponsoring this initiative. This could be your organization." /></td>
                </tr>
                <tr>
                    <td colspan="2" height="5"><img src="/images/spacer.gif" border="0" width="1" height="5" tooltip="To avoid duplicate project entries use the eclipse button to search for existing projects."/></td>
                </tr>
                <tr>
                    <td colspan="2" style="height: 18px" title="Select the organization sponsoring this initiative. Note: this could be your organization.">What segment of the organization is sponsoring this initiative?</td>
                </tr>
                <tr>
                    <td title="Select the segment of the organization sponsoring this initiative."><img src="/images/spacer.gif" border="0" width="15" height="1" /></td>
                    <td title="Select the segment of the organization sponsoring this initiative.">
                        <asp:DropDownList ID="ddlSegment" runat="server" CssClass="default" Enabled="false" Width="400" ToolTip="Select the segment of the organization sponsoring this initiative.">
                            <asp:ListItem Value="-- Please select a Sponsoring Portfolio --" />
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" height="5"><img src="/images/spacer.gif" border="0" width="1" height="5" /></td>
                </tr>
                <tr>
                    <td colspan="2" title="To avoid duplicate project entries use the eclipse button to search for existing project numbers.">What is your Clarity Number?</td>
                </tr>
                <tr>
                    <td title="To avoid duplicate project entries use the eclipse button to search for existing project numbers."><img src="/images/spacer.gif" border="0" width="15" height="1" /></td>
                    <td title="To avoid duplicate project entries use the eclipse button to search for existing project numbers."><asp:TextBox ID="txtClarityNumber" runat="server" CssClass="default" Width="150" MaxLength="20" /> <asp:Button ID="btnPNumber" runat="server" CssClass="default" Text="..." Width="25" tooltip="To avoid duplicate project entries use the eclipse button to search for existing project numbers."/></td>
                </tr>
                <tr>
                    <td colspan="2" height="5"><img src="/images/spacer.gif" border="0" width="1" height="5" /></td>
                </tr>
                <tr>
                    <td colspan="2"><hr size="1" noshade /></td>
                </tr>
                <tr>
                    <td colspan="2" align="right">
                        <table width="100%" cellpadding="0" cellspacing="0" border="0">
                            <tr>
                                <td class="required">* Required Field</td>
                                <td align="right"><asp:Button ID="btnSubmit" runat="server" CssClass="default" Width="75" Text="Next" OnClick="btnSubmit_Click" ToolTip="Click Next to proceed" /></td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" align="center"><asp:Label ID="lblInvalid" runat="server" CssClass="header" Text="<br /><img src='/images/bigX.gif' border='0' align='absmiddle' /> This project number does not qualify for a new request" Visible="false" /></td>
                </tr>
            </table>
            </asp:Panel>
            <asp:Panel ID="panForm" runat="server" Visible="false">
            <table width="100%" cellpadding="4" cellspacing="3" border="0">
                <tr>
                    <td colspan="2" align="center"><asp:Label ID="lblError" runat="server" CssClass="bigerror" Text="<br /><img src='/images/bigX.gif' border='0' align='absmiddle' /> There was a problem saving the information" Visible="false" /></td>
                </tr>
                <tr>
                    <td>Submitter Name:</td>
                    <td><asp:Label ID="lblName" runat="server" CssClass="default" /></td>
                </tr>
                <tr>
                    <td>Request Date:</td>
                    <td><asp:Label ID="lblDate" runat="server" CssClass="default" /></td>
                </tr>
                <tr>
                    <td title="Type the LAN ID, first name or last name of the Executive Sponsor that is representing this initiative. Normally, the executive sponsor is a Director or General Manager. If you are not sure how to answer this question, please refer to your reporting manager.">Executive Sponsor:<font class="required">&nbsp;*</font></td>
                    <td title="Type the LAN ID, first name or last name of the Executive Sponsor that is representing this initiative. Normally, the executive sponsor is a Director or General Manager. If you are not sure how to answer this question, please refer to your reporting manager.">
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
                    <td class="footer" title="Type the LAN ID, first name or last name of the Executive Sponsor that is representing this initiative. Normally, the executive sponsor is a Director or General Manager. If you are not sure how to answer this question, please refer to your reporting manager.">&nbsp;</td>
                    <td class="footer" title="Type the LAN ID, first name or last name of the Executive Sponsor that is representing this initiative. Normally, the executive sponsor is a Director or General Manager. If you are not sure how to answer this question, please refer to your reporting manager.">(Please enter a valid LAN ID, First Name, or Last Name)</td>
                </tr>
                <tr>
                    <td title="Type the LAN ID, first name or last name of the Working Sponsor that is representing this initiative. The working sponsor could be you. If you are not sure how to answer this question, please refer to your reporting manager.">Working Sponsor:<font class="required">&nbsp;*</font></td>
                    <td title="Type the LAN ID, first name or last name of the Working Sponsor that is representing this initiative. The working sponsor could be you. If you are not sure how to answer this question, please refer to your reporting manager.">
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
                    <td class="footer" title="Type the LAN ID, first name or last name of the Working Sponsor that is representing this initiative. The working sponsor could be you. If you are not sure how to answer this question, please refer to your reporting manager.">&nbsp;</td>
                    <td class="footer" title="Type the LAN ID, first name or last name of the Working Sponsor that is representing this initiative. The working sponsor could be you. If you are not sure how to answer this question, please refer to your reporting manager.">(Please enter a valid LAN ID, First Name, or Last Name)</td>
                </tr>
                <tr>
                    <td valign="top" title="Select C1 if your initiative requires funding. This includes hardware, software, and labor, etc.. ">Require a C1:</td>
                    <td title="Select C1 if your initiative requires any kind of funding. This includes hardware, software, and resource hours. "><asp:CheckBox ID="chkC1" runat="server" CssClass="default" /></td>
                </tr>
                <tr>
                    <td>End Life:</td>
                    <td><asp:CheckBox ID="chkEndLife" runat="server" /></td>
                </tr>
                <tr id="divEndLife" runat="server" style="display:none">
                    <td>End Life Date:</td>
                    <td><asp:TextBox ID="txtEndLife" runat="server" CssClass="default" Width="100" MaxLength="10" /> <asp:ImageButton ID="imgEndLife" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calendar.gif" /></td>
                </tr>
                <tr>
                    <td title="Optional: upload any documentation pertaining this initiative that could present your case.">Upload Case Study:</td>
                    <td title="Optional: upload any documentation pertaining this initiative that could present your case."><img src="/images/upload.gif" border="0" align="absmiddle" /> <asp:LinkButton ID="btnDocuments" runat="server" Text="Click Here to Upload" /></td>
                </tr>
                <tr>
                    <td valign="top" title="Type the initiative opportunity. Be sure to be as descriptive as possible. Please avoid double spacing lines and bullet points.">Initiative Opportunity:<font class="required">&nbsp;*</font></td>
                    <td title="Type initiative opportunity. Be sure to be as descriptive as possible. Please avoid double spacing lines and bullet points."><asp:TextBox ID="txtInitiative" runat="server" CssClass="error" Width="300" TextMode="multiline" Rows="5" /></td>
                </tr>
                <tr>
                    <td valign="top">Technology Capability:<font class="required">&nbsp;*</font></td>
                    <td><asp:TextBox ID="txtCapability" runat="server" CssClass="default" Width="300" TextMode="multiline" Rows="3" /></td>
                </tr>
                <tr>
                    <td valign="top"><br />Platform(s):<font class="required">&nbsp;*</font></td>
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
                    <td>Is this an Audit Requirement:</td>
                    <td><asp:CheckBox ID="chkRequirement" runat="server" /></td>
                </tr>
                <tr id="divRequirement" runat="server" style="display:none">
                    <td>Requirement Date:</td>
                    <td><asp:TextBox ID="txtRequirement" runat="server" CssClass="default" Width="100" MaxLength="10" /> <asp:ImageButton ID="imgRequirement" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calendar.gif" /></td>
                </tr>
                <tr>
                    <td>Inter dependency With Other Projects/Initiatives:<font class="required">&nbsp;*</font></td>
                    <td>
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
                    <td style="height: 45px">Estimated Project Labor Hours:<font class="required">&nbsp;*</font></td>
                    <td style="height: 45px"><asp:TextBox ID="txtHours" runat="server" CssClass="default" Width="50" MaxLength="10" /></td>
                </tr>
                <tr>
                    <td>Estimated Project Start Date:<font class="required">&nbsp;*</font></td>
                    <td><asp:TextBox ID="txtStart" runat="server" CssClass="default" Width="100" MaxLength="10" /> <asp:ImageButton ID="imgStart" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calendar.gif" /></td>
                </tr>
                <tr>
                    <td>Estimated Project End Date:<font class="required">&nbsp;*</font></td>
                    <td><asp:TextBox ID="txtCompletion" runat="server" CssClass="default" Width="100" MaxLength="10" /> <asp:ImageButton ID="imgCompletion" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calendar.gif" /></td>
                </tr>
                <tr>
                    <td>Expected Project Capital Cost:<font class="required">&nbsp;*</font></td>
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
                    <td>Project Expenses:<font class="required">&nbsp;*</font>&nbsp;<a href="javascript:void(0);" onclick="OpenWindow('TEMPHELP','/help/Project_Expenses_Help.htm#PE');"><img src="/images/help.gif" border="0" align="absmiddle" /></a></td>
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
                        <asp:DropDownList ID="ddlSavings" runat="server" CssClass="default" ToolTip="Test">
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
                    <td>Business Impact Aviodance:<font class="required">&nbsp;*</font>&nbsp;<a href="javascript:void(0);" onclick="OpenWindow('TEMPHELP','/help/Business_Impact_Avoid_Help.htm#BIA');"><img src="/images/help.gif" border="0" align="absmiddle" /></a></td>
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
                    <td>Business Impact:<font class="required">&nbsp;*</font>&nbsp;<a href="javascript:void(0);" onclick="OpenWindow('TEMPHELP','/help/Business_Impact_Help.htm#BI');"><img src="/images/help.gif" border="0" align="absmiddle" /></a></td>
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
                    <td>Strategic Opportunity:<font class="required">&nbsp;*</font>&nbsp;<a href="javascript:void(0);" onclick="OpenWindow('TEMPHELP','/help/Strategic_Opportunity_Help.htm#SO');"><img src="/images/help.gif" border="0" align="absmiddle" /></a></td>
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
                    <td>Acquisition / BIC:<font class="required">&nbsp;*</font>&nbsp;<a href="javascript:void(0);" onclick="OpenWindow('TEMPHELP','/help/Acquisition_BIC_Help.htm#AB');"><img src="/images/help.gif" border="0" align="absmiddle" /></a></td>
                    <td>
                        <asp:DropDownList ID="ddlAcquisition" runat="server" CssClass="default">
                            <asp:ListItem Value="0" Text="-- SELECT --" />
                            <asp:ListItem Value="1" Text="Does not support" />
                            <asp:ListItem Value="3" Text="Indirectly supports" />
                            <asp:ListItem Value="5" Text="Directly supports" />
                        </asp:DropDownList>
                      </font>&nbsp;<a href="javascript:void(0);" onclick="OpenWindow('TEMPHELP','/help/Acquisition_BIC_Help.htm#AB');"><img src="/images/help.gif" border="0" align="absmiddle" /></td>
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
                    <td title="If this initiative is a infrastructure related project that requires a technical project manager select Yes. CAUTION: technical project managers are limited resources. Be sure to consult with your reporting manager prior to selecting Yes. ">Require Technical Project Manager:<font class="required">&nbsp;*</font></td>
                    <td title="If this initiative is a infrastructure related project that requires a technical project manager select Yes. CAUTION: Technical project managers are limited resources. Be sure to consult with your reporting manager prior to selecting Yes. ">
                        <asp:RadioButton ID="radTPMYes" runat="server" Text="Yes" CssClass="default" GroupName="TPM" ToolTip="If this initiative is a infrastructure related project that requires a technical project manager select Yes. CAUTION: Technical project managers are limited resources. Be sure to consult with your reporting manager prior to selecting Yes. " />&nbsp;
                        <asp:RadioButton ID="radTPMNo" runat="server" Text="No" CssClass="default" GroupName="TPM" ToolTip="If this initiative is a infrastructure related project that requires a technical project manager select Yes. CAUTION: Technical project managers are limited resources. Be sure to consult with your reporting manager prior to selecting Yes. " />
                    </td>
                </tr>
                <tr id="divTPMYes" runat="server" style="display:none">
                    <td>Service Type:</td>
                    <td><asp:DropDownList ID="ddlTPM" runat="server" CssClass="default" /></td>
                </tr>
                <tr id="divTPMNo" runat="server" style="display:none">
                    <td title="Enter the LAN ID of the individual who will project manage this initiative. The project coordinator could be you. CAUTION: Be sure to consult with the individual that you have elected  prior to entering the LAN ID.">Project Coordinator:</td>
                    <td title="Enter the LAN ID of the individual who will project manage this initiative. The project coordinator could be you. CAUTION: Be sure to consult with the individual that you have elected  prior to entering the LAN ID.">
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
                    <td colspan="2"><hr size="1" noshade /></td>
                </tr>
                <tr>
                    <td colspan="2" align="right">
                        <table width="100%" cellpadding="0" cellspacing="0" border="0">
                            <tr>
                                <td class="required">* = Required Field</td>
                                <td align="right"><asp:Button ID="btnSave" runat="server" CssClass="default" Width="75" Text="Submit" OnClick="btnSave_Click" ToolTip="Click submit to route for approval" /> <asp:Button ID="btnCancel" runat="server" CssClass="default" Width="75" Text="Cancel" Visible="false" /></td>
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
                    <td><img src="/images/spacer.gif" border="0" width="15" height="1" /></td>
                    <td align="left" class="header"><img src="/images/bigCheck.gif" border="0" align="absmiddle" /> <b>Thank you for your request!!</b></td>
                </tr>
                <tr><td colspan="2">&nbsp;</td></tr>
                <tr>
                    <td><img src="/images/spacer.gif" border="0" width="15" height="1" /></td>
                    <td align="left">Your project request has been submitted successfully. An email has been sent to your mailbox with additional information regarding the next steps in the process. Please read this information carefully.</td>
                </tr>
                <tr><td colspan="2">&nbsp;</td></tr>
                <tr><td colspan="2" align="center" class="result">Your project request number is <b><asp:Label id="lblRequest" CssClass="result" runat="server" /></b>.</td></tr>
                <tr><td colspan="2">&nbsp;</td></tr>
                <tr>
                    <td><img src="/images/spacer.gif" border="0" width="15" height="1" /></td>
                    <td align="left"><asp:CheckBox ID="chkNotify" runat="server" CssClass="default" Text="Notify me of discussion posts" Checked="true" /></td>
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
            <asp:Panel ID="panDuplicate" runat="server" Visible="false">
                <br />
            <table width="100%" cellpadding="2" cellspacing="2" border="0">
                <tr>
                    <td align="left" class="header">
                        <img src="/images/spacer.gif" border="0" width="15" height="1" /><img src="/images/bigX.gif" border="0" align="absmiddle" /> Duplicate Request
                    </td>
                </tr>
                <tr><td>&nbsp;</td></tr>
                <tr>
                    <td><asp:Label ID="lblDuplicate" runat="server" CssClass="default" />.</td>
                </tr>
                <tr>
                    <td><asp:Label ID="lblDetails" runat="server" CssClass="default" /></td>
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
            <asp:Panel ID="panDiscretionary" runat="server" Visible="false">
                <br />
            <table width="100%" cellpadding="2" cellspacing="2" border="0">
                <tr>
                    <td align="left" class="header">
                        <img src="/images/spacer.gif" border="0" width="15" height="1" /><img src="/images/bigAlert.gif" border="0" align="absmiddle" /> Discretionary Projects Not Available
                    </td>
                </tr>
                <tr><td>&nbsp;</td></tr>
                <tr>
                    <td>The system is not currently configured for discretionary projects.</td>
                </tr>
                <tr><td>&nbsp;</td></tr>
                <tr><td><asp:HyperLink ID="lnkDiscretionary" runat="server" Text="If you are attempting to request a resource, click here to be redirected to the service request page" /> </td></tr>
                <tr><td>&nbsp;</td></tr>
                <tr>
                    <td colspan="2"><hr size="1" noshade /></td>
                </tr>
                <tr>
                    <td colspan="2" align="right">
                        <table width="100%" cellpadding="0" cellspacing="0" border="0">
                            <tr>
                                <td class="footer"></td>
                                <td align="right"><asp:Button ID="btnDiscretionary" runat="server" CssClass="default" Width="75" Text="Close" /></td>
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
<asp:HiddenField ID="hdnManager" runat="server" />
<asp:HiddenField ID="hdnError" runat="server" />
<asp:Label ID="lblProject" runat="server" Visible="false" />
<asp:HiddenField ID="hdnSegment" runat="server" />
