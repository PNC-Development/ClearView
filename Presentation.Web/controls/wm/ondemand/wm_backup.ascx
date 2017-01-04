<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wm_backup.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.wm_backup" %>

<script type="text/javascript">
    function ChangeFrame(oCell, oShow, oHide1,oHide2,oHide3,oHide4,oHide5, oHide6,oHidden, strHidden) {
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
        oHide5 = document.getElementById(oHide5);
        oHide5.style.display = "none";
        oHide6 = document.getElementById(oHide6);
        oHide6.style.display = "none";
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
    function EnsureBackupOK(oCheck, strValidation) {
        oCheck = document.getElementById(oCheck);
        if (oCheck.checked == true) {
            while (strValidation != "") {
                var strServer = strValidation.substring(0, strValidation.indexOf(";"));
                strValidation = strValidation.substring(strValidation.indexOf(";") + 1);
                if (ValidateDropDown('ddlServer_' + strServer,'Please make a selection for the TSM Server') == false)
                    return false;
                if (ValidateDropDown('ddlDomain_' + strServer,'Please make a selection for the Domain') == false)
                    return false;
                if (ValidateDropDown('ddlSchedule_' + strServer,'Please make a selection for the Schedule') == false)
                    return false;
                if (ValidateDropDown('DDL_' + strServer + '_TSM_CLOPTSET','Please make a selection for the CLOPTSET') == false)
                    return false;
                if (ValidateHidden('HDN_' + strServer + '_TSM_REGISTER','TXT_' + strServer + '_TSM_REGISTER','Please enter a valid REGISTER statement') == false)
                    return false;
                if (ValidateHidden('HDN_' + strServer + '_TSM_DEFINE','TXT_' + strServer + '_TSM_DEFINE','Please enter a valid DEFINE statement') == false)
                    return false;
            }
        }
        return true;
    }
    function UpdateDDL(oDDL, oNumber) {
        oNumber = document.getElementById(oNumber);
        oNumber.value = oDDL.options[oDDL.selectedIndex].value;
    }
</script>
<asp:Panel ID="panWorkload" runat="server" Visible="false">
    <table width="100%" border="0" cellSpacing="0" cellPadding="0" class="default">
        <tr>
            <td>
                <table width="100%" border="0" cellSpacing="0" cellPadding="0">
                    <tr id="cntrlButtons">
                        <td><asp:ImageButton ID="btnSave" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/tool_save.gif" OnClick="btnSave_Click" /></td>
                        <td><asp:ImageButton ID="btnCancel" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/tool_cancel.gif" /></td>
                        <td><asp:ImageButton ID="btnPrint" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/tool_print.gif" /></td>
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
                    <tr id="cntrlProcessing" style="display:none">
                        <td colspan="20">
                            <table width="100%" cellpadding="0" cellspacing="5" border="0">
                                <tr>
                                    <td rowspan="2"><img src="/images/saving.gif" border="0" align="absmiddle" /></td>
                                    <td class="header" width="100%" valign="bottom">Processing...</td>
                                </tr>
                                <tr>
                                    <td width="100%" valign="top">Please do not close this window while the page is processing.  Please be patient....</td>
                                </tr>
                            </table>
                        </td>
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
                                    <td nowrap><b>Last Updated:</b></td>
                                    <td><asp:Label ID="lblUpdated" runat="server" CssClass="default" /></td>
                                </tr>
                                <tr>
                                    <td nowrap><b>Nickname:</b></td>
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
                </table>
            </td>
        </tr>
        <tr id="divCancel" runat="server" style="display:none">
            <td>
                <table width="100%" cellpadding="3" cellspacing="2" border="0" style="border:solid 1px #999999" bgcolor="#f9f9f9">
                    <tr>
                        <td class="bigred">Cancel Request</td>
                    </tr>
                    <tr>
                        <td>Please enter the reason for the cancellation of this request:</td>
                    </tr>
                    <tr>
                        <td><asp:TextBox ID="txtCancel" runat="server" CssClass="default" width="600" TextMode="multiLine" Rows="8" /></td>
                    </tr>
                    <tr>
                        <td class="note">NOTE: This reason will be displayed to the requestor</td>
                    </tr>
                    <tr>
                        <td><asp:Button ID="btnCancelConfirm" runat="server" CssClass="default" width="150" Text="Click Here to Cancel" OnClick="btnCancelConfirm_Click" /></td>
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
                            <td class="cmbutton" style='<%=boolDetails == true ? "display:inline;border-bottom:1px solid #FFFFFF;border-top:3px solid orange;" : "" %>' onclick="ChangeFrame(this,'divDetails','divExecution','divInclusion','divExclusion','divArchiveRequirements','divAdditionalConfiguration','divStatusUpdates','<%=hdnTab.ClientID %>','D');">Request Details</td>
                            <td class="cmbuttonspace">&nbsp;</td>
                            <td class="cmbutton" style='<%=boolExecution == true ? "display:inline;border-bottom:1px solid #FFFFFF;border-top:3px solid orange;" : "" %>' onclick="ChangeFrame(this,'divExecution','divDetails','divInclusion','divExclusion','divArchiveRequirements','divAdditionalConfiguration','divStatusUpdates','<%=hdnTab.ClientID %>','E');">Execution</td>
                            <td class="cmbuttonspace">&nbsp;</td>
                            <td class="cmbutton" style='<%=boolBackupInclusion == true ? "display:inline;border-bottom:1px solid #FFFFFF;border-top:3px solid orange;" : "" %>' onclick="ChangeFrame(this,'divInclusion','divDetails','divExecution','divExclusion','divArchiveRequirements','divAdditionalConfiguration','divStatusUpdates','<%=hdnTab.ClientID %>','0');">Backup Inclusions</td>
                            <td class="cmbuttonspace">&nbsp;</td>
                            <td class="cmbutton" style='<%=boolBackupExclusion == true ? "display:inline;border-bottom:1px solid #FFFFFF;border-top:3px solid orange;" : "" %>' onclick="ChangeFrame(this,'divExclusion','divDetails','divExecution','divInclusion','divArchiveRequirements','divAdditionalConfiguration','divStatusUpdates','<%=hdnTab.ClientID %>','0');">Backup Exclusions</td>
                            <td class="cmbuttonspace">&nbsp;</td>
                            <td class="cmbutton" style='<%=boolArchiveRequirement == true ? "display:inline;border-bottom:1px solid #FFFFFF;border-top:3px solid orange;" : "" %>' onclick="ChangeFrame(this,'divArchiveRequirements','divDetails','divExecution','divInclusion','divExclusion','divAdditionalConfiguration','divStatusUpdates','<%=hdnTab.ClientID %>','0');">Archive Requirements</td>
                            <td class="cmbuttonspace">&nbsp;</td>
                            <td class="cmbutton" style='<%=boolAdditionalConfiguration == true ? "display:inline;border-bottom:1px solid #FFFFFF;border-top:3px solid orange;" : "" %>' onclick="ChangeFrame(this,'divAdditionalConfiguration','divDetails','divExecution','divInclusion','divExclusion','divArchiveRequirements','divStatusUpdates','<%=hdnTab.ClientID %>','0');">Additional Configuration</td>
                            <td class="cmbuttonspace">&nbsp;</td>
                            <td class="cmbutton" style='<%=boolStatusUpdates == true ? "display:inline;border-bottom:1px solid #FFFFFF;border-top:3px solid orange;" : "" %>' onclick="ChangeFrame(this,'divStatusUpdates','divDetails','divExecution','divInclusion','divExclusion','divArchiveRequirements','divAdditionalConfiguration','<%=hdnTab.ClientID %>','S');">Status Updates</td>
                        </tr>
                        <tr>
                            <td colspan="15" align="center" class="cmcontents">
                                <table width="100%" height="100%" border="0" cellSpacing="2" cellPadding="2" class="default">
                                    <tr> 
                                        <td valign="top">
		                                    <div id="divDetails" style='<%=boolDetails == true ? "display:inline" : "display:none" %>'>
		                                        <br />
                                                <table width="100%" cellpadding="4" cellspacing="3" border="0">
                                                    <tr>
                                                        <td><img src="/images/check.gif" border="0" align="absmiddle" /> <asp:LinkButton ID="btnView" runat="server" Text="Click Here to View the Original Design" /></td>
                                                    </tr>
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
                                                <asp:Panel ID="panPreview" runat="server" Visible="false">
                                                    <table width="100%" cellpadding="3" cellspacing="2" border="0" style="border:solid 1px #999999" bgcolor="#f9f9f9">
                                                        <tr>
                                                            <td class="bigred">Message Preview</td>
                                                        </tr>
                                                        <tr>
                                                            <td><asp:Label ID="lblPreview" runat="server" CssClass="default" /></td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                                <br />
                                                <asp:Panel ID="panVirtual" runat="server" Visible="false">
                                                    <table width="100%" cellpadding="3" cellspacing="2" border="0" style="border:solid 1px #999999" bgcolor="#f9f9f9">
                                                        <tr>
                                                            <td class="bigred"><img src="/images/bigAlert.gif" border="0" align="absmiddle" /> NOTE: These are VIRTUAL machines!!</td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                                <br />
                                                <table width="100%" cellpadding="4" cellspacing="3" border="0">
                                                    <tr>
                                                        <td nowrap><asp:CheckBox ID="chk1" runat="server" CssClass="default" /></td>
                                                        <td nowrap><asp:Image ID="img1" runat="server" ImageAlign="absMiddle" ImageUrl="/images/spacer.gif" /></td>
                                                        <td width="100%">Finished Configuring Backup</td>
                                                    </tr>
                                                    <tr>
                                                        <td nowrap></td>
                                                        <td nowrap></td>
                                                        <td width="100%"><asp:Label ID="lbl1" runat="server" CssClass="default" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td></td>
                                                        <td></td>
                                                        <td>
    <asp:Panel ID="panCFI" runat="server" Visible="false">
                    <table width="100%" cellpadding="5" cellspacing="2" border="0">
                        <tr>
                            <td colspan="2">
                                <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                    <tr>
                                        <td class="bigger"><b>Backup Configuration</b></td>
                                        <td align="right"><b>B</b> = Backup Acceptable</td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td nowrap class="bold">Backup Frequency:</td>
                            <td width="100%"><asp:Label ID="lblFrequency" runat="server" /></td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <table width="100%" cellpadding="4" cellspacing="0" border="0" style="border:solid 1px #CCCCCC">
                                    <tr>
                                        <td></td>
                                        <td colspan="12" align="center" style="color: #FFF; background-color: #333"><b>AM</b></td>
                                        <td colspan="12" align="center" style="color: #000; background-color: #AAA"><b>PM</b></td>
                                    </tr>
                                    <tr bgcolor="#EEEEEE">
                                        <td></td>
                                        <td>12</td>
                                        <td>1</td>
                                        <td>2</td>
                                        <td>3</td>
                                        <td>4</td>
                                        <td>5</td>
                                        <td>6</td>
                                        <td>7</td>
                                        <td>8</td>
                                        <td>9</td>
                                        <td>10</td>
                                        <td>11</td>
                                        <td>12</td>
                                        <td>1</td>
                                        <td>2</td>
                                        <td>3</td>
                                        <td>4</td>
                                        <td>5</td>
                                        <td>6</td>
                                        <td>7</td>
                                        <td>8</td>
                                        <td>9</td>
                                        <td>10</td>
                                        <td>11</td>
                                    </tr>
                                    <%=strBackupCFI.ToString()%>
                                </table>
                            </td>
                        </tr>
                    </table>
        <table width="100%" cellpadding="5" cellspacing="2" border="0">
            <tr>
                <td class="bigger" colspan="2"><b>Backup Exclusion Configuration</b></td>
            </tr>
            <tr>
                <td colspan="2">
                    <table width="100%" cellpadding="4" cellspacing="0" border="0" style="border:solid 1px #CCCCCC">
                        <tr bgcolor="#EEEEEE">
                            <td><b><u>Path:</u></b></td>
                        </tr>
                        <asp:repeater ID="rptExclusionsCFI" runat="server">
                            <ItemTemplate>
                                <tr>
                                    <td valign="top" width="100%"><%# DataBinder.Eval(Container.DataItem, "path") %></td>
                                </tr>
                            </ItemTemplate>
                            <AlternatingItemTemplate>
                                <tr bgcolor="F6F6F6">
                                    <td valign="top" width="100%"><%# DataBinder.Eval(Container.DataItem, "path") %></td>
                                </tr>
                            </AlternatingItemTemplate>
                        </asp:repeater>
                        <tr>
                            <td colspan="2">
                                <asp:Label ID="lblExclusion" runat="server" CssClass="default" Visible="false" Text="<img src='/images/spacer.gif' border='0' height='1' width='25' /><img src='/images/alert.gif' border='0' align='absmiddle'> There are no backup exclusions" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </asp:Panel>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <br />
                                                <a name="refresh"></a>
                                                <table cellpadding="3" cellspacing="0" border="0">
                                                    <tr>
                                                        <td><asp:Button ID="btnRefresh" runat="server" CssClass="default" Width="150" Text="Refresh Results" OnClick="btnRefresh_Click" /></td>
                                                    </tr>
                                                </table>
                                                <table cellpadding="3" cellspacing="0" border="0" style="border:solid 1px #CCCCCC">
                                                    <%=strBackupHeader %>
                                                    <%=strBackup %>
                                                </table>
                                                <%=strHiddenV %>
                                                <br />
		                                    </div>
		                                    <div id="divInclusion" style='<%= boolBackupInclusion == true ? "display:inline" : "display:none" %>'>
                                                <br />
                                                <table width="100%" cellpadding="0" cellspacing="5" border="0">
                                                    <tr>
                                                        <td rowspan="2"><img src="/images/backup.gif" border="0" align="absmiddle" /></td>
                                                        <td class="header" width="100%" valign="bottom"> Backup Inclusions</td>
                                                    </tr>                         
                                                </table>
                                                <table width="500" cellpadding="4" cellspacing="0" border="0" style="border:solid 1px #CCCCCC">
                                                    <tr bgcolor="#EEEEEE">
                                                       <td nowrap><b>File/Folder</b></td>
                                                       <td nowrap></td>
                                                    </tr>
                                                    <asp:repeater ID="rptInclusions" runat="server">
                                                        <ItemTemplate>
                                                           <tr>
                                                              <td><%# DataBinder.Eval(Container.DataItem, "path") %></td>                                                               
                                                           </tr>
                                                        </ItemTemplate>
                                                    </asp:repeater>
                                                    <tr>
                                                       <td colspan="2">
                                                          <asp:Label ID="lblNoneInclusions" runat="server" CssClass="default" Visible="false" Text="<img src='/images/alert.gif' border='0' align='absmiddle'> There are no inclusions..." />
                                                       </td>
                                                    </tr>                       
                                                </table>
                                            </div>
                                            <div id="divExclusion" style='<%=boolBackupExclusion == true ? "display:inline" : "display:none" %>'>
                                                <br />
                                                <table width="100%" cellpadding="0" cellspacing="5" border="0">
                                                    <tr>
                                                       <td rowspan="2"><img src="/images/backup.gif" border="0" align="absmiddle" /></td>
                                                       <td class="header" width="100%" valign="bottom"> Backup Exclusions</td>
                                                    </tr>                         
                                                </table>
                                                <table width="500" cellpadding="4" cellspacing="0" border="0" style="border:solid 1px #CCCCCC">
                                                    <tr bgcolor="#EEEEEE">
                                                       <td nowrap><b>File/Folder</b></td>
                                                       <td nowrap></td>
                                                    </tr>
                                                    <asp:repeater ID="rptExclusions" runat="server">
                                                        <ItemTemplate>
                                                           <tr>
                                                              <td><%# DataBinder.Eval(Container.DataItem, "path") %></td>                                                               
                                                           </tr>
                                                        </ItemTemplate>
                                                    </asp:repeater>
                                                    <tr>
                                                       <td colspan="2">
                                                          <asp:Label ID="lblNoneExclusions" runat="server" CssClass="default" Visible="false" Text="<img src='/images/alert.gif' border='0' align='absmiddle'> There are no exclusions..." />
                                                       </td>
                                                    </tr>
                                                </table>
                                            </div>
                                            <div id="divArchiveRequirements" style='<%=boolArchiveRequirement == true ? "display:inline" : "display:none" %>'>
                                                <br />
                                                <table width="100%" cellpadding="0" cellspacing="5" border="0">
                                                    <tr>
                                                       <td rowspan="2"><img src="/images/backup.gif" border="0" align="absmiddle" /></td>
                                                       <td class="header" width="100%" valign="bottom"> Archive Requirements</td>
                                                    </tr>                         
                                                </table>
                                                <table width="500" cellpadding="4" cellspacing="0" border="0" style="border:solid 1px #CCCCCC">
                                                    <tr bgcolor="#EEEEEE">
                                                       <td nowrap colspan="3"><b>File/Folder</b></td>
                                                       <td nowrap></td>
                                                    </tr>
                                                    <asp:repeater ID="rptRetention" runat="server">
                                                        <ItemTemplate>
                                                           <tr>
                                                              <td colspan="3"><%# DataBinder.Eval(Container.DataItem, "path") %></td>                                                               
                                                           </tr>
                                                           <tr>
                                                              <td>&nbsp;&nbsp;&nbsp;</td>
                                                              <td nowrap>First Archival:</td>
                                                              <td width="100%"><%# DateTime.Parse(DataBinder.Eval(Container.DataItem, "first").ToString()).ToShortDateString() %></td>
                                                              <td></td>
                                                           </tr>
                                                           <tr>
                                                              <td>&nbsp;&nbsp;&nbsp;</td>
                                                              <td nowrap>Archive Period:</td>
                                                              <td width="100%"><%# (DataBinder.Eval(Container.DataItem, "number").ToString() == "0" ? "" : DataBinder.Eval(Container.DataItem, "number") + " ") %><%# DataBinder.Eval(Container.DataItem, "type") %></td>
                                                              <td></td>
                                                           </tr>
                                                           <tr>
                                                              <td>&nbsp;&nbsp;&nbsp;</td>
                                                              <td nowrap>Start Time:</td>
                                                              <td width="100%"><%# DataBinder.Eval(Container.DataItem, "hour") %> <%# DataBinder.Eval(Container.DataItem, "switch") %></td>
                                                              <td></td>
                                                           </tr>
                                                           <tr>
                                                              <td>&nbsp;&nbsp;&nbsp;</td>
                                                              <td nowrap>Frequency:</td>
                                                              <td width="100%"><%# DataBinder.Eval(Container.DataItem, "occurence") %></td>
                                                              <td></td>
                                                           </tr>
                                                        </ItemTemplate>
                                                    </asp:repeater>
                                                    <tr>
                                                       <td colspan="6">
                                                          <asp:Label ID="lblNoneRetention" runat="server" CssClass="default" Visible="false" Text="<img src='/images/alert.gif' border='0' align='absmiddle'> There are no archive requirements..." />
                                                       </td>
                                                    </tr>
                                                </table>                                                               
                                            </div>
                                            <div id="divAdditionalConfiguration" style='<%=boolAdditionalConfiguration == true ? "display:inline" : "display:none" %>'>
                                                <br />
                                                <table width="100%" cellpadding="0" cellspacing="5" border="0">
                                                    <tr>
                                                       <td rowspan="2"><img src="/images/backup.gif" border="0" align="absmiddle" /></td>
                                                       <td class="header" width="100%" valign="bottom"> Additional Configuration</td>
                                                    </tr>                         
                                                </table>
                                                <table width="400" cellpadding="1" cellspacing="1" border="0" class="default">
                                                    <tr>
                                                       <td>Average Size of One Data File:</td>
                                                       <td colspan="2"><asp:Label ID="lblAverage" runat="server" CssClass="default" /></td>
                                                    </tr>
                                                    <tr>
                                                       <td>Production Turnover<br />Documentation Folder Name:</td>
                                                       <td colspan="2"><asp:Label ID="lblDocumentation" runat="server" CssClass="default" /></td>
                                                    </tr>
                                                    <tr>
                                                       <td colspan="3">&nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                       <td colspan="2" class="bold">Client File System Data</td>                                                      
                                                    </tr>
                                                    <tr>
                                                       <td>Percent Changed Daily:</td>
                                                       <td><asp:Label ID="lblCFPercent" runat="server" CssClass="default" /></td>                                                       
                                                    </tr>
                                                    <tr>
                                                       <td>Compression Ratio:</td>
                                                       <td><asp:Label ID="lblCFCompression" runat="server" CssClass="default" /></td>                                                        
                                                    </tr>
                                                    <tr>
                                                       <td>Average File Size:</td>
                                                       <td>
                                                           <asp:Label ID="lblCFAverage" runat="server" CssClass="default" />                                                             
                                                       </td>                                                        
                                                    </tr>
                                                    <tr>
                                                       <td>Backup Version Ratio:</td>
                                                       <td><asp:Label ID="lblCFBackup" runat="server" CssClass="default" /></td>                                                        
                                                    </tr>
                                                    <tr>
                                                       <td>Archive Ratio:</td>
                                                       <td><asp:Label ID="lblCFArchive" runat="server" CssClass="default" /></td>                                                        
                                                    </tr>
                                                    <tr>
                                                       <td>Backup Window (Hours):</td>
                                                       <td><asp:Label ID="lblCFWindow" runat="server" CssClass="default" /></td>                                                        
                                                    </tr>
                                                    <tr>
                                                       <td>Backupsets:</td>
                                                       <td><asp:Label ID="lblCFSets" runat="server" CssClass="default" /></td>                                                        
                                                    </tr>
                                                    <tr>
                                                       <td colspan="3">&nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                       <td colspan="2" class="bold">Client Database Data</td>                                                        
                                                    </tr>
                                                    <tr>
                                                       <td>Database Type:</td>
                                                       <td>
                                                           <asp:Label ID="lblCDType" runat="server" CssClass="default" />                                                        
                                                       </td>                                                        
                                                    </tr>
                                                    <tr>
                                                       <td>Percent Changed Daily:</td>
                                                       <td><asp:Label ID="lblCDPercent" runat="server" CssClass="default" /></td>                                                        
                                                    </tr>
                                                    <tr>
                                                       <td>Compression Ratio:</td>
                                                       <td><asp:Label ID="lblCDCompression" runat="server" CssClass="default" /></td>                                                        
                                                    </tr>
                                                    <tr>
                                                       <td>Number of Backup Versions:</td>
                                                       <td><asp:Label ID="lblCDVersions" runat="server" CssClass="default" /></td>                                                        
                                                    </tr>
                                                    <tr>
                                                       <td>Backup Window (Hours):</td>
                                                       <td><asp:Label ID="lblCDWindow" runat="server" CssClass="default" /></td>                                                        
                                                    </tr>
                                                    <tr>
                                                       <td>Growth Factor:</td>
                                                       <td><asp:Label ID="lblCDGrowth" runat="server" CssClass="default" /></td>                                                        
                                                    </tr>
                                                    <tr>
                                                       <td colspan="3">&nbsp;</td>
                                                    </tr>
                                                </table>
                                            </div>
		                                    <div id="divStatusUpdates" style='<%=boolStatusUpdates == true ? "display:inline" : "display:none" %>'>
		                                        <br />
                                                <table width="100%" cellpadding="4" cellspacing="3" border="0">
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
                                                        <td colspan="2"><asp:Button ID="btnStatus" runat="server" CssClass="default" Width="150" Text="Submit Status" OnClick="btnStatus_Click" /></td>
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
                                                                    <td nowrap>&nbsp;</td>
                                                                    <td nowrap><b>Comments</b></td>
                                                                </tr>
                                                                <asp:repeater ID="rptStatus" runat="server">
                                                                    <ItemTemplate>
                                                                        <tr class="default">
                                                                            <td nowrap valign="top"><a href="javascript:void(0);" onclick="ShowStatus('<%# DataBinder.Eval(Container.DataItem, "id") %>','r');"><%# DateTime.Parse(DataBinder.Eval(Container.DataItem, "modified").ToString()).ToShortDateString() %>&nbsp;<%# DateTime.Parse(DataBinder.Eval(Container.DataItem, "modified").ToString()).ToShortTimeString() %></a></td>
                                                                            <td nowrap>&nbsp;</td>
                                                                            <td nowrap valign="top"><asp:Label ID="lblStatus" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "status") %>' /></td>
                                                                            <td nowrap>&nbsp;</td>
                                                                            <td width="100%" valign="top">
                                                                                <div id="div200_<%# DataBinder.Eval(Container.DataItem, "id") %>" style="display:inline">
                                                                                    <%# (DataBinder.Eval(Container.DataItem, "comments").ToString().Length > 200 ? DataBinder.Eval(Container.DataItem, "comments").ToString().Substring(0, 200) + " ...&nbsp;&nbsp;&nbsp;[<a href=\"javascript:void(0);\" onclick=\"ShowHideDiv('div200_" + DataBinder.Eval(Container.DataItem, "id") + "', 'none');ShowHideDiv('divMore_" + DataBinder.Eval(Container.DataItem, "id") + "', 'inline');\">More</a>]" : DataBinder.Eval(Container.DataItem, "comments").ToString())%>
                                                                                </div>
                                                                                <div id="divMore_<%# DataBinder.Eval(Container.DataItem, "id") %>" style="display:none">
                                                                                    <%# DataBinder.Eval(Container.DataItem, "comments").ToString() + "&nbsp;&nbsp;&nbsp;[<a href=\"javascript:void(0);\" onclick=\"ShowHideDiv('div200_" + DataBinder.Eval(Container.DataItem, "id") + "', 'inline');ShowHideDiv('divMore_" + DataBinder.Eval(Container.DataItem, "id") + "', 'none');\">Hide</a>]"%>
                                                                                </div>
                                                                            </td>
                                                                        </tr>
                                                                    </ItemTemplate>
                                                                </asp:repeater>
                                                            <tr>
                                                                <td colspan="5">
                                                                    <asp:Label ID="lblNoStatus" runat="server" CssClass="default" Visible="false" Text="<img src='/images/alert.gif' border='0' align='absmiddle'> There are no updates" />
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
<asp:Label ID="lblAnswer" runat="server" Visible="false" />
<asp:Label ID="lblModel" runat="server" Visible="false" />
<asp:Label ID="lblTSM" runat="server" Visible="false" />
<asp:Label ID="lblAvamar" runat="server" Visible="false" />
<asp:HiddenField ID="hdnTab" runat="server" />
<asp:HiddenField ID="hdnBackup" runat="server" />