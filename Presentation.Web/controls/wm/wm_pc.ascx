<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wm_pc.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.wm_pc" %>


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
                                    <td width="100%"><asp:TextBox ID="txtName" runat="server" CssClass="default" Width="250" MaxLength="35" /></td>
                                </tr>
                                <tr>
                                    <td nowrap><b>Nickname:</b></td>
                                    <td width="100%"><asp:TextBox ID="txtCustom" runat="server" CssClass="default" Width="250" MaxLength="35" /></td>
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
                                        <asp:DropDownList ID="ddlSegment" CssClass="default" runat="server" Width="300" Enabled="false" >
                                            <asp:ListItem Value="-- Please select a Sponsoring Portfolio --" />
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td nowrap><b>Project Start Date:</b></td>
                                    <td width="100%"><asp:TextBox ID="txtStartDate" runat="server" CssClass="default" Width="100" MaxLength="10" /> <asp:ImageButton ID="imgStart" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calendar.gif" /></td>
                                </tr>
                                <tr>
                                    <td nowrap><b>Project End Date:</b></td>
                                    <td width="100%"><asp:Label ID="lblEndDate" runat="server" CssClass="default" /></td>
                                </tr>
                                <tr>
                                    <td nowrap><b>Total Approved:</b></td>
                                    <td width="100%">$<asp:Label ID="lblTotalApproved" runat="server" CssClass="default" /></td>
                                </tr>
                                <tr>
                                    <td nowrap><b>Total Actual:</b></td>
                                    <td width="100%">$<asp:Label ID="lblTotalActual" runat="server" CssClass="default" /></td>
                                </tr>
                                <tr>
                                    <td nowrap><b>Variance:</b></td>
                                    <td width="100%">$<asp:Label ID="lblTotalVariance" runat="server" CssClass="default" /></td>
                                </tr>
                            </table>
                        </td>
                        <td width="50%" valign="top">
                            <table width="100%" border="0" cellSpacing="2" cellPadding="3" class="default">
                                <tr>
                                    <td nowrap valign="top"><b>Project Description:</b></td>
                                    <td width="100%"><asp:TextBox ID="txtDescription" runat="server" CssClass="default" Width="350" TextMode="MultiLine" Rows="9" /></td>
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
                                                <td><asp:ListBox ID="lstCostsCurrent" runat="server" Width="150" CssClass="default" Rows="5" /></td>
                                                <td>&nbsp;</td>
                                                <td>
                                                    <asp:ImageButton ID="btnCostAdd" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/lt.gif" ToolTip="Add" /><br /><br />
                                                    <asp:ImageButton ID="btnCostRemove" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/rt.gif" ToolTip="Remove" />
                                                </td>
                                                <td>&nbsp;</td>
                                                <td><asp:ListBox ID="lstCostsAvailable" runat="server" Width="150" CssClass="default" Rows="5" /></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td nowrap><b>Project Status:</b></td>
                                    <td width="100%"><asp:Label ID="lblProjectStatus" runat="server" CssClass="default" /></td>
                                </tr>
                                <tr>
                                    <td nowrap><b>Overall Priority:</b></td>
                                    <td width="100%"><asp:Label ID="lblPriority" runat="server" CssClass="default" /></td>
                                </tr>
                                <tr>
                                    <td nowrap><b>Overall Status:</b></td>
                                    <td width="100%"><asp:Label ID="lblStatus" runat="server" CssClass="default" /></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr height="1">
                        <td colspan="2"><img src="/images/spacer.gif" border="0" height="1" width="1" /></td>
                    </tr>
                    <tr>
                        <td><asp:LinkButton ID="btnView" runat="server" /></td>
                        <td><asp:Label ID="lblComplete" runat="server" CssClass="default" /></td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <table id="tblDivs" width="100%" height="100%" border="0" cellSpacing="0" cellPadding="0" class="default">
                    <tr>
                        <td>
                            <table id="tblTabs" width="100%" border="0" cellspacing="0" cellpadding="0" class="default">
                                <tr>
                                    <td>
                                        <table width="100%" border="0" cellspacing="0" cellpadding="0" class="default">
                                            <asp:Panel ID="panStart" runat="server" Visible="false">
                                            <tr>
                                                <td align="center">
                                                    <div style='<%=boolStep1 == true ? "display:inline" : "display:none" %>'>
                                                    <table border="0" cellspacing="3" cellpadding="1">
                                                        <tr>
                                                            <td rowspan="2"><img src="/images/greenArrow.gif" border="0" align="absmiddle" /></td>
                                                            <td class="bigreddefault"><b><%=strHelpText %></b></td>
                                                        </tr>
                                                        <tr><td>&nbsp;</td></tr>
                                                    </table>
                                                    </div>
                                                </td>
                                                <td class="cmbuttonspacetop">&nbsp;</td>
                                                <td align="center">
                                                    <div style='<%=boolStep2 == true ? "display:inline" : "display:none" %>'>
                                                    <table border="0" cellspacing="3" cellpadding="1">
                                                        <tr>
                                                            <td rowspan="2"><img src="/images/greenArrow.gif" border="0" align="absmiddle" /></td>
                                                            <td class="bigreddefault"><b><%=strHelpText %></b></td>
                                                        </tr>
                                                        <tr><td>&nbsp;</td></tr>
                                                    </table>
                                                    </div>
                                                </td>
                                                <td class="cmbuttonspacetop">&nbsp;</td>
                                                <td align="center">
                                                    <div style='<%=boolStep3 == true ? "display:inline" : "display:none" %>'>
                                                    <table border="0" cellspacing="3" cellpadding="1">
                                                        <tr>
                                                            <td rowspan="2"><img src="/images/greenArrow.gif" border="0" align="absmiddle" /></td>
                                                            <td class="bigreddefault"><b><%=strHelpText %></b></td>
                                                        </tr>
                                                        <tr><td>&nbsp;</td></tr>
                                                    </table>
                                                    </div>
                                                </td>
                                                <td class="cmbuttonspacetop">&nbsp;</td>
                                                <td align="center">
                                                    <div style='<%=boolStep4 == true ? "display:inline" : "display:none" %>'>
                                                    <table border="0" cellspacing="3" cellpadding="1">
                                                        <tr>
                                                            <td rowspan="2"><img src="/images/greenArrow.gif" border="0" align="absmiddle" /></td>
                                                            <td class="bigreddefault"><b><%=strHelpText %></b></td>
                                                        </tr>
                                                        <tr><td>&nbsp;</td></tr>
                                                    </table>
                                                    </div>
                                                </td>
                                                <td class="cmbuttonspacetop">&nbsp;</td>
                                                <td align="center">
                                                    <div style='<%=boolStep5 == true ? "display:inline" : "display:none" %>'>
                                                    <table border="0" cellspacing="3" cellpadding="1">
                                                        <tr>
                                                            <td rowspan="2"><img src="/images/greenArrow.gif" border="0" align="absmiddle" /></td>
                                                            <td class="bigreddefault"><b><%=strHelpText %></b></td>
                                                        </tr>
                                                        <tr><td>&nbsp;</td></tr>
                                                    </table>
                                                    </div>
                                                </td>
                                                <td class="cmbuttonspacetop">&nbsp;</td>
                                            </tr>
                                            </asp:Panel>
                                            <tr>
                                                <td class="cmbuttontop" style='<%=boolStep1 == true ? "display:inline;border-bottom:1px solid #FFFFFF;border-top:3px solid #6A8359;" : "" %>' onclick="ChangeFrame3(this,'divStep1','<%=hdnTab.ClientID %>','1');">Step 1: Unlock</td>
                                                <td class="cmbuttonspacetop">&nbsp;</td>
                                                <td class="cmbuttontop" style='<%=boolStep2 == true ? "display:inline;border-bottom:1px solid #FFFFFF;border-top:3px solid #6A8359;" : "" %>' onclick="ChangeFrame3(this,'divStep2','<%=hdnTab.ClientID %>','2');">Step 2: Discovery</td>
                                                <td class="cmbuttonspacetop">&nbsp;</td>
                                                <td class="cmbuttontop" style='<%=boolStep3 == true ? "display:inline;border-bottom:1px solid #FFFFFF;border-top:3px solid #6A8359;" : "" %>' onclick="ChangeFrame3(this,'divStep3','<%=hdnTab.ClientID %>','3');">Step 3: Planning</td>
                                                <td class="cmbuttonspacetop">&nbsp;</td>
                                                <td class="cmbuttontop" style='<%=boolStep4 == true ? "display:inline;border-bottom:1px solid #FFFFFF;border-top:3px solid #6A8359;" : "" %>' onclick="ChangeFrame3(this,'divStep4','<%=hdnTab.ClientID %>','4');">Step 4: Execution</td>
                                                <td class="cmbuttonspacetop">&nbsp;</td>
                                                <td class="cmbuttontop" style='<%=boolStep5 == true ? "display:inline;border-bottom:1px solid #FFFFFF;border-top:3px solid #6A8359;" : "" %>' onclick="ChangeFrame3(this,'divStep5','<%=hdnTab.ClientID %>','5');">Step 5: Closing</td>
                                                <td class="cmbuttonspacetop">&nbsp;</td>
                                                <td class="cmbuttontop" style='<%=boolStatus == true ? "display:inline;border-bottom:1px solid #FFFFFF;border-top:3px solid #6A8359;" : "" %>' onclick="ChangeFrame3(this,'divStatus','<%=hdnTab.ClientID %>','S');">Status Updates</td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <table width="100%" border="0" cellspacing="0" cellpadding="0" class="default">
                                            <tr>
                                                <td class="cmbuttonspaceleft">&nbsp;</td>
                                                <td class="cmbutton" style='<%=boolMyDocuments == true ? "display:inline;border-bottom:1px solid #FFFFFF;border-top:3px solid #6A8359;" : "" %>' onclick="ChangeFrame3(this,'divMyDocuments','<%=hdnTab.ClientID %>','M');">My Documents</td>
                                                <td class="cmbuttonspace">&nbsp;</td>
                                                <td class="cmbutton" style='<%=boolDocuments == true ? "display:inline;border-bottom:1px solid #FFFFFF;border-top:3px solid #6A8359;" : "" %>' onclick="ChangeFrame3(this,'divDocuments','<%=hdnTab.ClientID %>','D');">Project Documents</td>
                                                <td class="cmbuttonspace">&nbsp;</td>
                                                <td class="cmbutton" onclick="ChangeFrame3(this,'divMine','<%=hdnTab.ClientID %>','T');">Additional Tasks</td>
                                                <td class="cmbuttonspace">&nbsp;</td>
                                                <td class="cmbutton" style='<%=boolMilestones == true ? "display:inline;border-bottom:1px solid #FFFFFF;border-top:3px solid #6A8359;" : "" %>' onclick="ChangeFrame3(this,'divMilestones','<%=hdnTab.ClientID %>','L');">Milestones</td>
                                                <td class="cmbuttonspace">&nbsp;</td>
                                                <td class="cmbutton" style='<%=boolResource == true ? "display:inline;border-bottom:1px solid #FFFFFF;border-top:3px solid #6A8359;" : "" %>' onclick="ChangeFrame3(this,'divResource','<%=hdnTab.ClientID %>','R');">Resource Involvement</td>
                                                <td class="cmbuttonspaceright">&nbsp;</td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" class="cmcontents">
                            <table width="100%" height="100%" border="0" cellSpacing="2" cellPadding="2" class="default">
                                <tr> 
                                    <td valign="top">
		                                    <div id="divStep1" style='<%=boolStep1 == true ? "display:inline" : "display:none" %>'>
		                                        <br /><br />
                                                <table width="100%" cellpadding="0" cellspacing="5" border="0">
                                                    <tr>
                                                        <td rowspan="2" valign="top"><img src="/images/Step1.gif" border="0" align="absmiddle" /></td>
                                                        <td class="hugeheader" width="100%" valign="bottom">Unlock Your Project</td>
                                                    </tr>
                                                    <tr>
                                                        <td width="100%" valign="top">You will need to obtain a clarity number and enter your estimated hours needed for Discovery before you can start your project.</td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2" background="/images/lines.gif">&nbsp;</td>
                                                    </tr>
                                                </table><br />
                                                <table width="100%" cellpadding="3" cellspacing="2" border="0" class="default">
                                                    <tr>
                                                        <td colspan="2">Click the following button to automatically submit a form to Clarity Processing Department with your project information. This form will take up to five (5) business days to process at which time, you will receive an email with your Clarity number.</td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2"><asp:Button ID="btnGenerate" runat="server" CssClass="default" Text="Generate Form" Width="150" OnClick="btnGenerate_Click" Enabled="false" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">Once you receive this number, please enter it below and fill in your estimated hours needed for the Discovery Phase and click <b>Save</b> to start your project.</td>
                                                    </tr>
                                                    <tr>
                                                        <td nowrap><b>Discovery Hours:</b></td>
                                                        <td width="100%"><asp:TextBox ID="txtDiscovery" runat="server" CssClass="default" Width="100" MaxLength="7" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td nowrap><b>Clarity Number:</b></td>
                                                        <td width="100%"><asp:TextBox ID="txtNumber" runat="server" CssClass="default" Width="100" MaxLength="7" /></td>
                                                    </tr>
                                                </table>
		                                    </div>
		                                    <div id="divStep2" style='<%=boolStep2 == true ? "display:inline" : "display:none" %>'>
		                                        <br /><br />
                                                <table width="100%" cellpadding="0" cellspacing="5" border="0">
                                                    <tr>
                                                        <td rowspan="2" valign="top"><img src="/images/Step2.gif" border="0" align="absmiddle" /></td>
                                                        <td class="hugeheader" width="100%" valign="bottom">Discovery Phase</td>
                                                    </tr>
                                                    <tr>
                                                        <td width="100%" valign="top">The Discovery Phase involves figuring out estimated financials and schedules. There is no approval associated with the discovery phase.</td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2" background="/images/lines.gif">&nbsp;</td>
                                                    </tr>
                                                </table><br />
                                                <table width="100%" cellpadding="5" cellspacing="0" border="0">
                                                    <tr>
                                                        <td nowrap>Discovery Completed (%):</td>
                                                        <td width="100%"><SliderCC:Slider ID="sldDiscovery" _ParentElement="divStep2" _Hidden="hdnDiscovery" runat="server" _DivClass="default" _PercentClass="result" _Width="300" _ShowDiv="true" _RestrictLess="true" _Enabled="false" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td nowrap>Your Planning Hours:</td>
                                                        <td width="100%"><asp:TextBox ID="txtPlanning" runat="server" CssClass="default" Width="100" MaxLength="7" Enabled="false" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">&nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td nowrap class="header">Discovery Schedule:</td>
                                                        <td width="100%">[<a href="/help/ScheduleFinancialsFormula.pdf" target="_blank">Schedule and Financials Formula</a>]</td>
                                                    </tr>
                                                    <tr>
                                                        <td nowrap>Actual Start Date:</td>
                                                        <td width="100%"><asp:TextBox ID="txtActDS" runat="server" CssClass="default" Width="80" MaxLength="10" Enabled="false" /> <asp:ImageButton ID="imgtxtActDS" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calendar.gif" Enabled="false" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td nowrap>Actual End Date:</td>
                                                        <td width="100%"><asp:TextBox ID="txtActDF" runat="server" CssClass="default" Width="80" MaxLength="10" Enabled="false" /> <asp:ImageButton ID="imgtxtActDF" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calendar.gif" Enabled="false" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">&nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2" class="header">Discovery Financials:</td>
                                                    </tr>
                                                    <tr>
                                                        <td nowrap>Actual Internal Labor:</td>
                                                        <td width="100%"><asp:TextBox ID="txtActDI" runat="server" CssClass="default" Width="80" MaxLength="10" Enabled="false" /> <asp:ImageButton ID="imgtxtActDI" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calculator.gif" Enabled="false" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td nowrap>Actual External Labor:</td>
                                                        <td width="100%"><asp:TextBox ID="txtActDE" runat="server" CssClass="default" Width="80" MaxLength="10" Enabled="false" /> <asp:ImageButton ID="imgtxtActDE" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calculator.gif" Enabled="false" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td nowrap>Actual HW/SW/One Time Cost:</td>
                                                        <td width="100%"><asp:TextBox ID="txtActDH" runat="server" CssClass="default" Width="80" MaxLength="10" Enabled="false" /> <asp:ImageButton ID="imgtxtActDH" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calculator.gif" Enabled="false" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td nowrap><b>Total Actual Discovery Cost:</b></td>
                                                        <td width="100%">$<asp:Label ID="lblActD" runat="server" CssClass="defaultbold" /></td>
                                                    </tr>
                                                </table>
		                                    </div>
		                                    <div id="divStep3" style='<%=boolStep3 == true ? "display:inline" : "display:none" %>'>
		                                        <br /><br />
                                                <table width="100%" cellpadding="0" cellspacing="5" border="0">
                                                    <tr>
                                                        <td rowspan="2" valign="top"><img src="/images/Step3.gif" border="0" align="absmiddle" /></td>
                                                        <td class="hugeheader" width="100%" valign="bottom">Planning Phase</td>
                                                    </tr>
                                                    <tr>
                                                        <td width="100%" valign="top">The Planning Phase involves figuring out estimated financials and schedules. Once you have finished your estimates, the working and executive sponsors will be required to approve your estimates before this project can continue.</td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2" background="/images/lines.gif">&nbsp;</td>
                                                    </tr>
                                                </table><br />
                                                <table width="100%" cellpadding="5" cellspacing="0" border="0">
                                                    <tr>
                                                        <td nowrap>Planning Completed (%):</td>
                                                        <td width="100%"><SliderCC:Slider ID="sldPlanning" _ParentElement="divStep3" _Hidden="hdnPlanning" runat="server" _DivClass="default" _PercentClass="result" _Width="300" _ShowDiv="true" _RestrictLess="true" _Enabled="false" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td nowrap>Your Execution Hours:</td>
                                                        <td width="100%"><asp:TextBox ID="txtExecution" runat="server" CssClass="default" Width="100" MaxLength="7" Enabled="false" /></td>
                                                    </tr>
                                                </table>
                                                <br /><br />
                                                <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                                    <tr>
                                                        <td colspan="3" bgcolor="#007253" class="whitedefault"><div style="padding:5"><b>PRE-APPROVAL PLANNING</b></div></td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="3" style="border-left:solid 1px #999999;border-right:solid 1px #999999;border-top:solid 1px #999999;">
                                                            <table width="100%" cellpadding="5" cellspacing="0" border="0">
                                                                <tr>
                                                                    <td nowrap class="header">Planning Schedule:</td>
                                                                    <td width="100%">[<a href="/help/ScheduleFinancialsFormula.pdf" target="_blank">Schedule and Financials Formula</a>]</td>
                                                                </tr>
                                                                <tr>
                                                                    <td nowrap>Estimated Start Date:</td>
                                                                    <td width="100%"><asp:TextBox ID="txtAppPS" runat="server" CssClass="default" Width="80" MaxLength="10" Enabled="false" /> <asp:ImageButton ID="imgtxtAppPS" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calendar.gif" Enabled="false" /></td>
                                                                </tr>
                                                                <tr>
                                                                    <td nowrap>Estimated End Date:</td>
                                                                    <td width="100%"><asp:TextBox ID="txtAppPF" runat="server" CssClass="default" Width="80" MaxLength="10" Enabled="false" /> <asp:ImageButton ID="imgtxtAppPF" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calendar.gif" Enabled="false" /></td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="2">&nbsp;</td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="2" class="header">Planning Financials:</td>
                                                                </tr>
                                                                <tr>
                                                                    <td nowrap>Estimated Internal Labor:</td>
                                                                    <td width="100%"><asp:TextBox ID="txtAppPI" runat="server" CssClass="default" Width="80" MaxLength="10" Enabled="false" /> <asp:ImageButton ID="imgtxtAppPI" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calculator.gif" Enabled="false" /></td>
                                                                </tr>
                                                                <tr>
                                                                    <td nowrap>Estimated External Labor:</td>
                                                                    <td width="100%"><asp:TextBox ID="txtAppPE" runat="server" CssClass="default" Width="80" MaxLength="10" Enabled="false" /> <asp:ImageButton ID="imgtxtAppPE" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calculator.gif" Enabled="false" /></td>
                                                                </tr>
                                                                <tr>
                                                                    <td nowrap>Estimated HW/SW/One Time Cost:</td>
                                                                    <td width="100%"><asp:TextBox ID="txtAppPH" runat="server" CssClass="default" Width="80" MaxLength="10" Enabled="false" /> <asp:ImageButton ID="imgtxtAppPH" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calculator.gif" Enabled="false" /></td>
                                                                </tr>
                                                                <tr>
                                                                    <td nowrap><b>Total Estimated Planning Cost:</b></td>
                                                                    <td width="100%">$<asp:Label ID="lblAppP" runat="server" CssClass="defaultbold" /></td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td><img src="/images/shadow_box_l.gif" border="0" width="5" height="12"></td>
                                                        <td width="100%" background="/images/shadow_box.gif"></td>
                                                        <td><img src="/images/shadow_box_r.gif" border="0" width="5" height="12"></td>
                                                    </tr>
                                                </table>
                                                <br /><br />
                                                <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                                    <tr>
                                                        <td class="header"><img src="/images/approval.gif" border="0" align="absmiddle" /> Planning Approval</td>
                                                    </tr>
                                                    <tr>
                                                        <td></td>
                                                    </tr>
                                                </table><br />
                                                <br /><br />
                                                <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                                    <tr>
                                                        <td colspan="3" bgcolor="#007253" class="whitedefault"><div style="padding:5"><b>POST-APPROVAL PLANNING</b></div></td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="3" style="border-left:solid 1px #999999;border-right:solid 1px #999999;border-top:solid 1px #999999;">
                                                            <table width="100%" cellpadding="5" cellspacing="0" border="0">
                                                                <tr>
                                                                    <td nowrap class="header">Planning Schedule:</td>
                                                                    <td width="100%">[<a href="/help/ScheduleFinancialsFormula.pdf" target="_blank">Schedule and Financials Formula</a>]</td>
                                                                </tr>
                                                                <tr>
                                                                    <td nowrap>Actual Start Date:</td>
                                                                    <td width="100%"><asp:TextBox ID="txtActPS" runat="server" CssClass="default" Width="80" MaxLength="10" Enabled="false" /> <asp:ImageButton ID="imgtxtActPS" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calendar.gif" Enabled="false" /></td>
                                                                </tr>
                                                                <tr>
                                                                    <td nowrap>Actual End Date:</td>
                                                                    <td width="100%"><asp:TextBox ID="txtActPF" runat="server" CssClass="default" Width="80" MaxLength="10" Enabled="false" /> <asp:ImageButton ID="imgtxtActPF" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calendar.gif" Enabled="false" /></td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="2">&nbsp;</td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="2" class="header">Planning Financials:</td>
                                                                </tr>
                                                                <tr>
                                                                    <td nowrap>Actual Internal Labor:</td>
                                                                    <td width="100%"><asp:TextBox ID="txtActPI" runat="server" CssClass="default" Width="80" MaxLength="10" Enabled="false" /> <asp:ImageButton ID="imgtxtActPI" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calculator.gif" Enabled="false" /></td>
                                                                </tr>
                                                                <tr>
                                                                    <td nowrap>Actual External Labor:</td>
                                                                    <td width="100%"><asp:TextBox ID="txtActPE" runat="server" CssClass="default" Width="80" MaxLength="10" Enabled="false" /> <asp:ImageButton ID="imgtxtActPE" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calculator.gif" Enabled="false" /></td>
                                                                </tr>
                                                                <tr>
                                                                    <td nowrap>Actual HW/SW/One Time Cost:</td>
                                                                    <td width="100%"><asp:TextBox ID="txtActPH" runat="server" CssClass="default" Width="80" MaxLength="10" Enabled="false" /> <asp:ImageButton ID="imgtxtActPH" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calculator.gif" Enabled="false" /></td>
                                                                </tr>
                                                                <tr>
                                                                    <td nowrap><b>Total Actual Planning Cost:</b></td>
                                                                    <td width="100%">$<asp:Label ID="lblActP" runat="server" CssClass="defaultbold" /></td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td><img src="/images/shadow_box_l.gif" border="0" width="5" height="12"></td>
                                                        <td width="100%" background="/images/shadow_box.gif"></td>
                                                        <td><img src="/images/shadow_box_r.gif" border="0" width="5" height="12"></td>
                                                    </tr>
                                                </table>
                                                <br /><br />
                                                <table cellpadding="5" cellspacing="0" border="0">
                                                    <tr>
                                                        <td class="header"><img src="/images/variance.gif" border="0" align="absmiddle" /> Planning Variances</td>
                                                        <td valign="bottom"><b>Variance # Days</b></td>
                                                        <td valign="bottom"><b>Variance %</b></td>
                                                    </tr>
                                                    <tr>
                                                        <td>Start / End Dates:</td>
                                                        <td><asp:Label ID="lblVarianceDaysP" runat="server" CssClass="default" /></td>
                                                        <td><asp:Label ID="lblVariancePercentP" runat="server" CssClass="default" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="3">&nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td>&nbsp;</td>
                                                        <td><b>Variance $</b></td>
                                                        <td><b>Variance %</b></td>
                                                    </tr>
                                                    <tr>
                                                        <td nowrap>Internal Labor:</td>
                                                        <td><asp:Label ID="lblVarDPI" runat="server" CssClass="default" /></td>
                                                        <td><asp:Label ID="lblVarPPI" runat="server" CssClass="default" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td nowrap>External Labor:</td>
                                                        <td><asp:Label ID="lblVarDPE" runat="server" CssClass="default" /></td>
                                                        <td><asp:Label ID="lblVarPPE" runat="server" CssClass="default" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td nowrap>HW/SW/One Time Cost:</td>
                                                        <td><asp:Label ID="lblVarDPH" runat="server" CssClass="default" /></td>
                                                        <td><asp:Label ID="lblVarPPH" runat="server" CssClass="default" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td nowrap><b>Total Estimated Planning Cost:</b></td>
                                                        <td><asp:Label ID="lblVarDP" runat="server" CssClass="defaultbold" /></td>
                                                        <td><asp:Label ID="lblVarPP" runat="server" CssClass="defaultbold" /></td>
                                                    </tr>
                                                </table>
		                                    </div>
		                                    <div id="divStep4" style='<%=boolStep4 == true ? "display:inline" : "display:none" %>'>
		                                        <br /><br />
                                                <table width="100%" cellpadding="0" cellspacing="5" border="0">
                                                    <tr>
                                                        <td rowspan="2" valign="top"><img src="/images/Step4.gif" border="0" align="absmiddle" /></td>
                                                        <td class="hugeheader" width="100%" valign="bottom">Execution Phase</td>
                                                    </tr>
                                                    <tr>
                                                        <td width="100%" valign="top">The Execution Phase involves executing your project. The working and executive sponsors will be required to approve your numbers before this project can continue.</td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2" background="/images/lines.gif">&nbsp;</td>
                                                    </tr>
                                                </table><br />
                                                <table width="100%" cellpadding="5" cellspacing="0" border="0">
                                                    <tr>
                                                        <td nowrap>Execution Completed (%):</td>
                                                        <td width="100%"><SliderCC:Slider ID="sldExecution" _ParentElement="divStep4" _Hidden="hdnExecution" runat="server" _DivClass="default" _PercentClass="result" _Width="300" _ShowDiv="true" _RestrictLess="true" _Enabled="false" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td nowrap>Your Closing Hours:</td>
                                                        <td width="100%"><asp:TextBox ID="txtClosing" runat="server" CssClass="default" Width="100" MaxLength="7" Enabled="false" /></td>
                                                    </tr>
                                                </table>
                                                <br /><br />
                                                <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                                    <tr>
                                                        <td colspan="3" bgcolor="#007253" class="whitedefault"><div style="padding:5"><b>PRE-APPROVAL EXECUTION</b></div></td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="3" style="border-left:solid 1px #999999;border-right:solid 1px #999999;border-top:solid 1px #999999;">
                                                            <table width="100%" cellpadding="5" cellspacing="0" border="0">
                                                                <tr>
                                                                    <td nowrap class="header">Execution Schedule:</td>
                                                                    <td width="100%">[<a href="/help/ScheduleFinancialsFormula.pdf" target="_blank">Schedule and Financials Formula</a>]</td>
                                                                </tr>
                                                                <tr>
                                                                    <td nowrap>Estimated Start Date:</td>
                                                                    <td width="100%"><asp:TextBox ID="txtAppES" runat="server" CssClass="default" Width="80" MaxLength="10" Enabled="false" /> <asp:ImageButton ID="imgtxtAppES" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calendar.gif" Enabled="false" /></td>
                                                                </tr>
                                                                <tr>
                                                                    <td nowrap>Estimated End Date:</td>
                                                                    <td width="100%"><asp:TextBox ID="txtAppEF" runat="server" CssClass="default" Width="80" MaxLength="10" Enabled="false" /> <asp:ImageButton ID="imgtxtAppEF" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calendar.gif" Enabled="false" /></td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="2">&nbsp;</td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="2" class="header">Execution Financials:</td>
                                                                </tr>
                                                                <tr>
                                                                    <td nowrap>Estimated Internal Labor:</td>
                                                                    <td width="100%"><asp:TextBox ID="txtAppEI" runat="server" CssClass="default" Width="80" MaxLength="10" Enabled="false" /> <asp:ImageButton ID="imgtxtAppEI" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calculator.gif" Enabled="false" /></td>
                                                                </tr>
                                                                <tr>
                                                                    <td nowrap>Estimated External Labor:</td>
                                                                    <td width="100%"><asp:TextBox ID="txtAppEE" runat="server" CssClass="default" Width="80" MaxLength="10" Enabled="false" /> <asp:ImageButton ID="imgtxtAppEE" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calculator.gif" Enabled="false" /></td>
                                                                </tr>
                                                                <tr>
                                                                    <td nowrap>Estimated HW/SW/One Time Cost:</td>
                                                                    <td width="100%"><asp:TextBox ID="txtAppEH" runat="server" CssClass="default" Width="80" MaxLength="10" Enabled="false" /> <asp:ImageButton ID="imgtxtAppEH" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calculator.gif" Enabled="false" /></td>
                                                                </tr>
                                                                <tr>
                                                                    <td nowrap><b>Total Estimated Execution Cost:</b></td>
                                                                    <td width="100%">$<asp:Label ID="lblAppE" runat="server" CssClass="defaultbold" /></td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td><img src="/images/shadow_box_l.gif" border="0" width="5" height="12"></td>
                                                        <td width="100%" background="/images/shadow_box.gif"></td>
                                                        <td><img src="/images/shadow_box_r.gif" border="0" width="5" height="12"></td>
                                                    </tr>
                                                </table>
                                                <br /><br />
                                                <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                                    <tr>
                                                        <td class="header"><img src="/images/approval.gif" border="0" align="absmiddle" /> Execution Approval</td>
                                                    </tr>
                                                    <tr>
                                                        <td></td>
                                                    </tr>
                                                </table><br />
                                                <br /><br />
                                                <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                                    <tr>
                                                        <td colspan="3" bgcolor="#007253" class="whitedefault"><div style="padding:5"><b>POST-APPROVAL EXECUTION</b></div></td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="3" style="border-left:solid 1px #999999;border-right:solid 1px #999999;border-top:solid 1px #999999;">
                                                            <table width="100%" cellpadding="5" cellspacing="0" border="0">
                                                                <tr>
                                                                    <td nowrap class="header">Execution Schedule:</td>
                                                                    <td width="100%">[<a href="/help/ScheduleFinancialsFormula.pdf" target="_blank">Schedule and Financials Formula</a>]</td>
                                                                </tr>
                                                                <tr>
                                                                    <td nowrap>Actual Start Date:</td>
                                                                    <td width="100%"><asp:TextBox ID="txtActES" runat="server" CssClass="default" Width="80" MaxLength="10" Enabled="false" /> <asp:ImageButton ID="imgtxtActES" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calendar.gif" Enabled="false" /></td>
                                                                </tr>
                                                                <tr>
                                                                    <td nowrap>Actual End Date:</td>
                                                                    <td width="100%"><asp:TextBox ID="txtActEF" runat="server" CssClass="default" Width="80" MaxLength="10" Enabled="false" /> <asp:ImageButton ID="imgtxtActEF" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calendar.gif" Enabled="false" /></td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="2">&nbsp;</td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="2" class="header">Execution Financials:</td>
                                                                </tr>
                                                                <tr>
                                                                    <td nowrap>Actual Internal Labor:</td>
                                                                    <td width="100%"><asp:TextBox ID="txtActEI" runat="server" CssClass="default" Width="80" MaxLength="10" Enabled="false" /> <asp:ImageButton ID="imgtxtActEI" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calculator.gif" Enabled="false" /></td>
                                                                </tr>
                                                                <tr>
                                                                    <td nowrap>Actual External Labor:</td>
                                                                    <td width="100%"><asp:TextBox ID="txtActEE" runat="server" CssClass="default" Width="80" MaxLength="10" Enabled="false" /> <asp:ImageButton ID="imgtxtActEE" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calculator.gif" Enabled="false" /></td>
                                                                </tr>
                                                                <tr>
                                                                    <td nowrap>Actual HW/SW/One Time Cost:</td>
                                                                    <td width="100%"><asp:TextBox ID="txtActEH" runat="server" CssClass="default" Width="80" MaxLength="10" Enabled="false" /> <asp:ImageButton ID="imgtxtActEH" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calculator.gif" Enabled="false" /></td>
                                                                </tr>
                                                                <tr>
                                                                    <td nowrap><b>Total Actual Execution Cost:</b></td>
                                                                    <td width="100%">$<asp:Label ID="lblActE" runat="server" CssClass="defaultbold" /></td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td><img src="/images/shadow_box_l.gif" border="0" width="5" height="12"></td>
                                                        <td width="100%" background="/images/shadow_box.gif"></td>
                                                        <td><img src="/images/shadow_box_r.gif" border="0" width="5" height="12"></td>
                                                    </tr>
                                                </table>
                                                <br /><br />
                                                <table cellpadding="5" cellspacing="0" border="0">
                                                    <tr>
                                                        <td class="header"><img src="/images/variance.gif" border="0" align="absmiddle" /> Execution Variances</td>
                                                        <td valign="bottom"><b>Variance # Days</b></td>
                                                        <td valign="bottom"><b>Variance %</b></td>
                                                    </tr>
                                                    <tr>
                                                        <td>Start / End Dates:</td>
                                                        <td><asp:Label ID="lblVarianceDaysE" runat="server" CssClass="default" /></td>
                                                        <td><asp:Label ID="lblVariancePercentE" runat="server" CssClass="default" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="3">&nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td>&nbsp;</td>
                                                        <td><b>Variance $</b></td>
                                                        <td><b>Variance %</b></td>
                                                    </tr>
                                                    <tr>
                                                        <td nowrap>Internal Labor:</td>
                                                        <td><asp:Label ID="lblVarDEI" runat="server" CssClass="default" /></td>
                                                        <td><asp:Label ID="lblVarPEI" runat="server" CssClass="default" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td nowrap>External Labor:</td>
                                                        <td><asp:Label ID="lblVarDEE" runat="server" CssClass="default" /></td>
                                                        <td><asp:Label ID="lblVarPEE" runat="server" CssClass="default" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td nowrap>HW/SW/One Time Cost:</td>
                                                        <td><asp:Label ID="lblVarDEH" runat="server" CssClass="default" /></td>
                                                        <td><asp:Label ID="lblVarPEH" runat="server" CssClass="default" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td nowrap><b>Total Estimated Execution Cost:</b></td>
                                                        <td><asp:Label ID="lblVarDE" runat="server" CssClass="defaultbold" /></td>
                                                        <td><asp:Label ID="lblVarPE" runat="server" CssClass="defaultbold" /></td>
                                                    </tr>
                                                </table>
		                                    </div>
		                                    <div id="divStep5" style='<%=boolStep5 == true ? "display:inline" : "display:none" %>'>
		                                        <br /><br />
                                                <table width="100%" cellpadding="0" cellspacing="5" border="0">
                                                    <tr>
                                                        <td rowspan="2" valign="top"><img src="/images/Step5.gif" border="0" align="absmiddle" /></td>
                                                        <td class="hugeheader" width="100%" valign="bottom">Closing Phase</td>
                                                    </tr>
                                                    <tr>
                                                        <td width="100%" valign="top">The Closing Phase involves completing your project. There is no approval associated with the closing phase. Once finished, you may use the <b>Complete</b> button to close your project.</td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2" background="/images/lines.gif">&nbsp;</td>
                                                    </tr>
                                                </table><br />
                                                <table width="100%" cellpadding="5" cellspacing="0" border="0">
                                                    <tr>
                                                        <td nowrap>Closing Completed (%):</td>
                                                        <td width="100%"><SliderCC:Slider ID="sldClosing" _ParentElement="divStep5" _Hidden="hdnClosing" runat="server" _DivClass="default" _PercentClass="result" _Width="300" _ShowDiv="true" _RestrictLess="true" _Enabled="false" /></td>
                                                    </tr>
                                                </table>
                                                <br /><br />
                                                <table width="100%" cellpadding="4" cellspacing="3" border="0" class="default">
                                                    <tr>
                                                        <td colspan="2">Results Better than Expected:</td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2"><asp:TextBox ID="txtBetter" runat="server" CssClass="default" Width="400" TextMode="MultiLine" Rows="5" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">Results Worse than Expected:</td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2"><asp:TextBox ID="txtWorse" runat="server" CssClass="default" Width="400" TextMode="MultiLine" Rows="5" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">Lessons Learned:</td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2"><asp:TextBox ID="txtLessons" runat="server" CssClass="default" Width="400" TextMode="MultiLine" Rows="5" /></td>
                                                    </tr>
                                                    <tr><td colspan="2">&nbsp;</td></tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <table cellpadding="3" cellspacing="2" border="0"  class="offsettable">
                                                                <tr>
                                                                    <td colspan="7" class="greentableheader">Overall Financials:</td>
                                                                </tr>
                                                                <tr>
                                                                    <td>&nbsp;</td>
                                                                    <td>&nbsp;</td>
                                                                    <td align="center"><b>Approved</b></td>
                                                                    <td>&nbsp;</td>
                                                                    <td align="center"><b>Actual</b></td>
                                                                    <td>&nbsp;</td>
                                                                    <td align="center"><b>Variance</b></td>
                                                                </tr>
                                                                <tr>
                                                                    <td nowrap>Internal Labor</td>
                                                                    <td>&nbsp;</td>
                                                                    <td align="center">$<asp:Label ID="lblApprovedI" runat="server" CssClass="default" /></td>
                                                                    <td>&nbsp;</td>
                                                                    <td align="center">$<asp:Label ID="lblActualI" runat="server" CssClass="default" /></td>
                                                                    <td>&nbsp;</td>
                                                                    <td align="center">$<asp:Label ID="lblVarianceI" runat="server" CssClass="default" /></td>
                                                                </tr>
                                                                <tr>
                                                                    <td nowrap>External Labor</td>
                                                                    <td>&nbsp;</td>
                                                                    <td align="center">$<asp:Label ID="lblApprovedE" runat="server" CssClass="default" /></td>
                                                                    <td>&nbsp;</td>
                                                                    <td align="center">$<asp:Label ID="lblActualE" runat="server" CssClass="default" /></td>
                                                                    <td>&nbsp;</td>
                                                                    <td align="center">$<asp:Label ID="lblVarianceE" runat="server" CssClass="default" /></td>
                                                                </tr>
                                                                <tr>
                                                                    <td nowrap>Capital (HW / SW)</td>
                                                                    <td>&nbsp;</td>
                                                                    <td align="center">$<asp:Label ID="lblApprovedC" runat="server" CssClass="default" /></td>
                                                                    <td>&nbsp;</td>
                                                                    <td align="center">$<asp:Label ID="lblActualC" runat="server" CssClass="default" /></td>
                                                                    <td>&nbsp;</td>
                                                                    <td align="center">$<asp:Label ID="lblVarianceC" runat="server" CssClass="default" /></td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="7" style="border-top:solid 1px #CCCCCC"><img src="/images/spacer.gif" border="0" width="1" height="1" /></td>
                                                                </tr>
                                                                <tr>
                                                                    <td nowrap align="right"><b>Total:</b></td>
                                                                    <td>&nbsp;</td>
                                                                    <td align="center"><b>$<asp:Label ID="lblApproved" runat="server" CssClass="default" /></b></td>
                                                                    <td>&nbsp;</td>
                                                                    <td align="center"><b>$<asp:Label ID="lblActual" runat="server" CssClass="default" /></b></td>
                                                                    <td>&nbsp;</td>
                                                                    <td align="center"><b>$<asp:Label ID="lblVariance" runat="server" CssClass="default" /></b></td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">&nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <table cellpadding="3" cellspacing="2" border="0"  class="offsettable">
                                                                <tr>
                                                                    <td colspan="7" class="greentableheader">Overall Schedule:</td>
                                                                </tr>
                                                                <tr>
                                                                    <td>&nbsp;</td>
                                                                    <td>&nbsp;</td>
                                                                    <td align="center"><b>Approved</b></td>
                                                                    <td>&nbsp;</td>
                                                                    <td align="center"><b>Actual</b></td>
                                                                    <td>&nbsp;</td>
                                                                    <td align="center"><b>Variance</b></td>
                                                                </tr>
                                                                <tr>
                                                                    <td nowrap>Start Date</td>
                                                                    <td>&nbsp;</td>
                                                                    <td align="center"><asp:Label ID="lblApprovedS" runat="server" CssClass="default" /></td>
                                                                    <td>&nbsp;</td>
                                                                    <td align="center"><asp:Label ID="lblActualS" runat="server" CssClass="default" /></td>
                                                                    <td>&nbsp;</td>
                                                                    <td align="center"><asp:Label ID="lblVarianceS" runat="server" CssClass="default" /></td>
                                                                </tr>
                                                                <tr>
                                                                    <td nowrap>End Date</td>
                                                                    <td>&nbsp;</td>
                                                                    <td align="center"><asp:Label ID="lblApprovedF" runat="server" CssClass="default" /></td>
                                                                    <td>&nbsp;</td>
                                                                    <td align="center"><asp:Label ID="lblActualF" runat="server" CssClass="default" /></td>
                                                                    <td>&nbsp;</td>
                                                                    <td align="center"><asp:Label ID="lblVarianceF" runat="server" CssClass="default" /></td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
		                                    </div>
                                            <div id="divStatus"  style='<%=boolStatus == true ? "display:inline" : "display:none" %>'>
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
                                                                    <td nowrap><b>Scope Variance:</b></td>
                                                                    <td width="100%">
                                                                        <asp:DropDownList ID="ddlScope" runat="server" CssClass="default" >
                                                                            <asp:ListItem Text="-- SELECT --" Value="0" />
                                                                            <asp:ListItem Text="Red" Value="1" />
                                                                            <asp:ListItem Text="Yellow" Value="2" />
                                                                            <asp:ListItem Text="Green" Value="3" />
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td nowrap><b>Timeline Variance:</b></td>
                                                                    <td width="100%">
                                                                        <asp:DropDownList ID="ddlTimeline" runat="server" CssClass="default" >
                                                                            <asp:ListItem Text="-- SELECT --" Value="0" />
                                                                            <asp:ListItem Text="Red" Value="1" />
                                                                            <asp:ListItem Text="Yellow" Value="2" />
                                                                            <asp:ListItem Text="Green" Value="3" />
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td nowrap><b>Budget Variance:</b></td>
                                                                    <td width="100%">
                                                                        <asp:DropDownList ID="ddlBudget" runat="server" CssClass="default" >
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
                                                                    <td><asp:TextBox ID="txtComments" runat="server" CssClass="default" Width="90%" TextMode="MultiLine" Rows="19" /></td>
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
                                                                    <td></td>
                                                                    <td nowrap align="center"><b>Scope Status</b></td>
                                                                    <td nowrap align="center"><b>Timeline Status</b></td>
                                                                    <td nowrap align="center"><b>Budget Status</b></td>
                                                                </tr>
                                                                <asp:repeater ID="rptStatus" runat="server">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblScope" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "scope") %>' Visible="false" />
                                                                        <asp:Label ID="lblTimeline" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "timeline") %>' Visible="false" />
                                                                        <asp:Label ID="lblBudget" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "budget") %>' Visible="false" />
                                                                        <tr class="default">
                                                                            <td nowrap><a href="javascript:void(0);" onclick="ShowStatus('<%# DataBinder.Eval(Container.DataItem, "id") %>','t');"><%# DateTime.Parse(DataBinder.Eval(Container.DataItem, "datestamp").ToString()).ToShortDateString() %></a></td>
                                                                            <td nowrap>&nbsp;</td>
                                                                            <td width="30%" align="center"><asp:Label ID="lblStatus" runat="server" CssClass="default" Text='' /></td>
                                                                            <td width="20%">&nbsp;</td>
                                                                            <td width="15%" align="center"><asp:Label ID="lblStatusScope" runat="server" CssClass="default" Text='' /></td>
                                                                            <td width="15%" align="center"><asp:Label ID="lblStatusTimeline" runat="server" CssClass="default" Text='' /></td>
                                                                            <td width="15%" align="center"><asp:Label ID="lblStatusBudget" runat="server" CssClass="default" Text='' /></td>
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
                                            <div id="divResource" style='<%=boolResource == true ? "display:inline" : "display:none" %>'>
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
                                            <div id="divMine" style="display:none">
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
		                                    <div id="divDocuments" style='<%=boolDocuments == true ? "display:inline" : "display:none" %>'>
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
		                                    <div id="divMyDocuments" style='<%=boolMyDocuments == true ? "display:inline" : "display:none" %>'>
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
		                                    <div id="divMilestones" style='<%=boolMilestones == true ? "display:inline" : "display:none" %>'>
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
<asp:HiddenField ID="hdnTab" runat="server" />
<asp:HiddenField ID="hdnPhase" runat="server" />
<asp:HiddenField ID="hdnCosts" runat="server" />
<asp:HiddenField ID="hdnSegment" runat="server" />
<asp:Label ID="lblDiscoveryHRs" runat="server" Visible="false" />
<asp:Label ID="lblPlanningHRs" runat="server" Visible="false" />
<asp:Label ID="lblExecutionHRs" runat="server" Visible="false" />
<asp:Label ID="lblClosingHRs" runat="server" Visible="false" />