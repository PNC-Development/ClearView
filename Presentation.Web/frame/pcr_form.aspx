<%@ Page Language="C#" MasterPageFile="~/frame.Master" AutoEventWireup="true" CodeBehind="pcr_form.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.pcr_form" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" runat="Server">
<script type="text/javascript">
function ValidateStatus(strStatus)
{   
   if(strStatus != "PENDING")
     return window.confirm('NOTE:This PCR has already been '+ strStatus +'!! Updating the PCR will re-route it to the Approvers.\\n Are you sure you want to continue ?');
  else
     return true;   
}
 function ValidatePCR(chkScope,chkSched,chkSD,DS,DE,chkSP,PS,PE,chkSE,ES,EE,chkSC,CS,CE,chkF,chkFD,D,chkFP,P,chkFE,E,chkFC,C) {
        chkScope = document.getElementById(chkScope);
        chkSched = document.getElementById(chkSched);
        chkF = document.getElementById(chkF);
        if (chkScope.checked == false && chkSched.checked == false && chkF.checked == false) {
            alert('Please select at least one cause for the change: Scope, Schedule and/or Financial');
            return false;
        }
        var oStop = false;
        if (chkSched.checked == true) {
            chkSD = document.getElementById(chkSD);
            chkSP = document.getElementById(chkSP);
            chkSE = document.getElementById(chkSE);
            chkSC = document.getElementById(chkSC);
            if (chkSD.checked == false && chkSP.checked == false && chkSE.checked == false && chkSC.checked == false) {
                oStop = true;
                alert('Please select at least one phase to change');
            }
            else if ((chkSD.checked == false || (chkSD.checked == true && ValidateDate(DS,'Please enter a valid start date') && ValidateDate(DE,'Please enter a valid end date')))
                    && (chkSP.checked == false || (chkSP.checked == true && ValidateDate(PS,'Please enter a valid start date') && ValidateDate(PE,'Please enter a valid end date')))
                    && (chkSE.checked == false || (chkSE.checked == true && ValidateDate(ES,'Please enter a valid start date') && ValidateDate(EE,'Please enter a valid end date')))
                    && (chkSC.checked == false || (chkSC.checked == true && ValidateDate(CS,'Please enter a valid start date') && ValidateDate(CE,'Please enter a valid end date'))))
                oStop = false;
            else
                oStop = true;
        }
        if (oStop == true)
            return false;
        if (chkF.checked == true) {
            chkFD = document.getElementById(chkFD);
            chkFP = document.getElementById(chkFP);
            chkFE = document.getElementById(chkFE);
            chkFC = document.getElementById(chkFC);
            if (chkFD.checked == false && chkFP.checked == false && chkFE.checked == false && chkFC.checked == false) {
                oStop = true;
                alert('Please select at least one phase to change');
            }
            else if ((chkFD.checked == false || (chkFD.checked == true && ValidateNumber(D,'Please enter a valid dollar amount')))
                    && (chkFP.checked == false || (chkFP.checked == true && ValidateNumber(P,'Please enter a valid dollar amount')))
                    && (chkFE.checked == false || (chkFE.checked == true && ValidateNumber(E,'Please enter a valid dollar amount')))
                    && (chkFC.checked == false || (chkFC.checked == true && ValidateNumber(C,'Please enter a valid dollar amount'))))
                oStop = false;
             else
                oStop = true;
        }
        if (oStop == true)
            return false;       
      
    }

 function chkHidden(chk,oHidden)
 {   
   var chk = document.getElementById(chk);
   var oHidden = document.getElementById(oHidden);  
   oHidden.value = chk.checked;
   return true;
 }
</script>
    <table width="100%" cellpadding="0" cellspacing="0" border="0" class="default">
        <tr>
            <td colspan="3" style="border-left: solid 1px #999999; border-right: solid 1px #999999;
                border-top: solid 1px #999999; border-bottom: solid 1px #999999;">
                <table width="100%" cellpadding="4" cellspacing="3" border="0" class="default">
                    <tr>
                        <td colspan="2" class="greentableheader">Proposed Change Details</td>
                    </tr>
                    <tr>
                        <td nowrap><b>Scope:</b></td>
                        <td width="100%"><asp:CheckBox ID="chkScope" runat="server" CssClass="default" /></td>                                            
                    </tr>
                    <tr>
                        <td colspan="2">
                            <div id="divScope" runat="server" style="display: none">
                                <table width="50%" cellpadding="0" cellspacing="0" border="0" class="default" style="border: dashed 1px #CCCCCC">
                                    <tr>
                                        <td nowrap>Reason for Scope Change:</td>                                                          
                                        <td> <asp:TextBox ID="txtScopeComments" runat="server" Width="400" CssClass="default"     Rows="5" TextMode="MultiLine" /></td>                                                                                                                       
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td nowrap><b>Schedule:</b></td>                                          
                        <td width="100%"><asp:CheckBox ID="chkSchedule" runat="server" CssClass="default" /></td>                                          
                    </tr>
                    <tr>
                        <td colspan="2">
                           <div id="divSchedule" runat="server" style="display:none">
                                                                            <table width="100%" cellpadding="3" cellspacing="0" border="0" style="border:dashed 1px #CCCCCC">
                                                                                <tr>
                                                                                    <td class="greenheader"></td>
                                                                                    <td class="greenheader"><b>Phase</b></td>
                                                                                    <td class="greenheader"></td>
                                                                                    <td class="greenheader" align="center"><b>Approved Dates</b></td>
                                                                                    <td class="greenheader"></td>
                                                                                    <td class="greenheader" align="center"><b>Modified Dates</b></td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td><asp:CheckBox ID="chkPCRScheduleD" runat="server" CssClass="default" /></td>
                                                                                    <td>Discovery</td>
                                                                                    <td align="center"><img src="/images/green_arrow.gif" border="0" align="absmiddle" /></td>
                                                                                    <td align="center"><asp:Label ID="lblPCRScheduleD" runat="server" CssClass="default" /></td>
                                                                                    <td align="center"><img src="/images/green_arrow.gif" border="0" align="absmiddle" /></td>
                                                                                    <td align="center"><asp:TextBox ID="txtPCRScheduleDS" runat="server" CssClass="default" Width="100" MaxLength="10" /> <asp:ImageButton ID="imgPCRScheduleDS" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calendar.gif" /> - <asp:TextBox ID="txtPCRScheduleDE" runat="server" CssClass="default" Width="100" MaxLength="10" /> <asp:ImageButton ID="imgPCRScheduleDE" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calendar.gif" /></td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td><asp:CheckBox ID="chkPCRScheduleP" runat="server" CssClass="default" /></td>
                                                                                    <td>Planning</td>
                                                                                    <td align="center"><img src="/images/green_arrow.gif" border="0" align="absmiddle" /></td>
                                                                                    <td align="center"><asp:Label ID="lblPCRScheduleP" runat="server" CssClass="default" /></td>
                                                                                    <td align="center"><img src="/images/green_arrow.gif" border="0" align="absmiddle" /></td>
                                                                                    <td align="center"><asp:TextBox ID="txtPCRSchedulePS" runat="server" CssClass="default" Width="100" MaxLength="10" /> <asp:ImageButton ID="imgPCRSchedulePS" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calendar.gif" /> - <asp:TextBox ID="txtPCRSchedulePE" runat="server" CssClass="default" Width="100" MaxLength="10" /> <asp:ImageButton ID="imgPCRSchedulePE" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calendar.gif" /></td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td><asp:CheckBox ID="chkPCRScheduleE" runat="server" CssClass="default" /></td>
                                                                                    <td>Execution</td>
                                                                                    <td align="center"><img src="/images/green_arrow.gif" border="0" align="absmiddle" /></td>
                                                                                    <td align="center"><asp:Label ID="lblPCRScheduleE" runat="server" CssClass="default" /></td>
                                                                                    <td align="center"><img src="/images/green_arrow.gif" border="0" align="absmiddle" /></td>
                                                                                    <td align="center"><asp:TextBox ID="txtPCRScheduleES" runat="server" CssClass="default" Width="100" MaxLength="10" /> <asp:ImageButton ID="imgPCRScheduleES" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calendar.gif" /> - <asp:TextBox ID="txtPCRScheduleEE" runat="server" CssClass="default" Width="100" MaxLength="10" /> <asp:ImageButton ID="imgPCRScheduleEE" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calendar.gif" /></td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td><asp:CheckBox ID="chkPCRScheduleC" runat="server" CssClass="default" /></td>
                                                                                    <td>Closing</td>
                                                                                    <td align="center"><img src="/images/green_arrow.gif" border="0" align="absmiddle" /></td>
                                                                                    <td align="center"><asp:Label ID="lblPCRScheduleC" runat="server" CssClass="default" /></td>
                                                                                    <td align="center"><img src="/images/green_arrow.gif" border="0" align="absmiddle" /></td>
                                                                                    <td align="center"><asp:TextBox ID="txtPCRScheduleCS" runat="server" CssClass="default" Width="100" MaxLength="10" /> <asp:ImageButton ID="imgPCRScheduleCS" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calendar.gif" /> - <asp:TextBox ID="txtPCRScheduleCE" runat="server" CssClass="default" Width="100" MaxLength="10" /> <asp:ImageButton ID="imgPCRScheduleCE" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calendar.gif" /></td>
                                                                                </tr>
                                                                                <tr>
                                                                                   <td colspan="8">
                                                                                     <table width="50%" cellpadding="2" cellspacing="0" border="0" class="default">
                                                                                        <tr>
                                                                                            <td nowrap>Reason for Schedule Change:</td>
                                                                                            <td><asp:TextBox ID="txtScheduleComments" runat="server" Width="400" CssClass="default" Rows="5" TextMode="MultiLine" /></td>
                                                                                        </tr> 
                                                                                     </table>          
                                                                                   </td>                                                                                                    
                                                                                </tr>
                                                                            </table>
                                                                        </div> 
                        </td>
                    </tr>
                    <tr>
                        <td nowrap><b>Financial:</b></td>                                            
                        <td width="100%"><asp:CheckBox ID="chkFinancial" runat="server" CssClass="default" /></td>                                            
                    </tr>
                    <tr>
                        <td colspan="2">
                              <div id="divFinancial" runat="server" style="display:none">
                                                                            <table width="100%" cellpadding="3" cellspacing="0" border="0" style="border:dashed 1px #CCCCCC">
                                                                                <tr>
                                                                                    <td class="greenheader"></td>
                                                                                    <td class="greenheader"><b>Phase</b></td>
                                                                                    <td class="greenheader"></td>
                                                                                    <td class="greenheader" align="center"><b>Approved Financials</b></td>
                                                                                    <td class="greenheader"></td>
                                                                                    <td class="greenheader" align="center"><b>Modified Financials</b></td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td><asp:CheckBox ID="chkPCRFinancialD" runat="server" CssClass="default" /></td>
                                                                                    <td>Discovery</td>
                                                                                    <td align="center"><img src="/images/green_arrow.gif" border="0" align="absmiddle" /></td>
                                                                                    <td align="center"><asp:Label ID="lblPCRFinancialD" runat="server" CssClass="default" /></td>
                                                                                    <td align="center"><img src="/images/green_arrow.gif" border="0" align="absmiddle" /></td>
                                                                                    <td align="center"><asp:TextBox ID="txtPCRFinancialD" runat="server" CssClass="default" Width="100" MaxLength="16" /> <asp:ImageButton ID="imgPCRFinancialD" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calculator.gif" /></td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td><asp:CheckBox ID="chkPCRFinancialP" runat="server" CssClass="default" /></td>
                                                                                    <td>Planning</td>
                                                                                    <td align="center"><img src="/images/green_arrow.gif" border="0" align="absmiddle" /></td>
                                                                                    <td align="center"><asp:Label ID="lblPCRFinancialP" runat="server" CssClass="default" /></td>
                                                                                    <td align="center"><img src="/images/green_arrow.gif" border="0" align="absmiddle" /></td>
                                                                                    <td align="center"><asp:TextBox ID="txtPCRFinancialP" runat="server" CssClass="default" Width="100" MaxLength="16" /> <asp:ImageButton ID="imgPCRFinancialP" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calculator.gif" /></td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td><asp:CheckBox ID="chkPCRFinancialE" runat="server" CssClass="default" /></td>
                                                                                    <td>Execution</td>
                                                                                    <td align="center"><img src="/images/green_arrow.gif" border="0" align="absmiddle" /></td>
                                                                                    <td align="center"><asp:Label ID="lblPCRFinancialE" runat="server" CssClass="default" /></td>
                                                                                    <td align="center"><img src="/images/green_arrow.gif" border="0" align="absmiddle" /></td>
                                                                                    <td align="center"><asp:TextBox ID="txtPCRFinancialE" runat="server" CssClass="default" Width="100" MaxLength="16" /> <asp:ImageButton ID="imgPCRFinancialE" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calculator.gif" /></td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td><asp:CheckBox ID="chkPCRFinancialC" runat="server" CssClass="default" /></td>
                                                                                    <td>Closing</td>
                                                                                    <td align="center"><img src="/images/green_arrow.gif" border="0" align="absmiddle" /></td>
                                                                                    <td align="center"><asp:Label ID="lblPCRFinancialC" runat="server" CssClass="default" /></td>
                                                                                    <td align="center"><img src="/images/green_arrow.gif" border="0" align="absmiddle" /></td>
                                                                                    <td align="center"><asp:TextBox ID="txtPCRFinancialC" runat="server" CssClass="default" Width="100" MaxLength="16" /> <asp:ImageButton ID="imgPCRFinancialC" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calculator.gif" /></td>
                                                                                </tr>
                                                                                <tr>
                                                                                   <td colspan="8">
                                                                                     <table width="50%" cellpadding="2" cellspacing="0" border="0" class="default">
                                                                                        <tr>
                                                                                            <td nowrap>Reason for Financial Change:</td>
                                                                                            <td><asp:TextBox ID="txtFinancialComments" runat="server" Width="400" CssClass="default" Rows="5" TextMode="MultiLine" /></td>
                                                                                        </tr> 
                                                                                     </table>          
                                                                                   </td>                                                                                                    
                                                                                </tr>
                                                                            </table>
                                                                        </div>
                        </td>
                    </tr>
                    <tr>
                        <td nowrap><b>Reason for PCR:</b></td>                                           
                        <td width="100%"><asp:CheckBox ID="chkReason" runat="server" CssClass="default" /></td>                                            
                    </tr>
                    <tr>
                        <td colspan="2">
                            <div id="divReason" runat="server" style="display: none">
                                <asp:CheckBoxList ID="chkPCRReason" runat="server" CssClass="default" RepeatColumns="3"
                                    RepeatDirection="horizontal">
                                    <asp:ListItem Value="Resource Constraints (internal and external)" />
                                    <asp:ListItem Value="Estimates" />
                                    <asp:ListItem Value="Scope change" />
                                    <asp:ListItem Value="Vendor issues" />
                                    <asp:ListItem Value="Technical issues" />
                                    <asp:ListItem Value="Dependencies (other projects or other...)" />
                                    <asp:ListItem Value="Unknown components" />
                                    <asp:ListItem Value="Unknown resources" />
                                    <asp:ListItem Value="IP/Audit requirements" />
                                    <asp:ListItem Value="Prioritization change" />
                                    <asp:ListItem Value="Sponsor mis-communication" />
                                </asp:CheckBoxList>
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="2" align="right">
                <asp:Button ID="btnUpdate" runat="server" Text="Update PCR" CssClass="default" Width="95"
                    OnClick="btnUpdate_Click" /></td>
        </tr>
    </table>
    <asp:HiddenField ID="hdnPCRD" runat="server" />
    <asp:HiddenField ID="hdnPCRC" runat="server" />
    <asp:HiddenField ID="hdnStatus" runat="server" />
</asp:Content>
