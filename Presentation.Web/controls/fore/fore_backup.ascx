<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="fore_backup.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.fore_backup" %>
<script type="text/javascript"> 

    function ForeBackRadio(oShow, oHide1, oHide2) {                               
        if (oShow != null)
            document.getElementById(oShow).style.display = "inline";
        document.getElementById(oHide1).style.display = "none";
        if (oHide2 != null)
            document.getElementById(oHide2).style.display = "none";
       
    }
    // Vijay code - start
    function ForeBackCheck(oCheck)
    {  
       if(oCheck.id.match('Weekly') != null && !oCheck.checked) {
         document.getElementById('<%= divWeekly.ClientID %>').style.display = "none";                 
       }
       else if(oCheck.id.match('Weekly') != null && oCheck.checked)               
         document.getElementById('<%= divWeekly.ClientID %>').style.display = "inline";    
    }
    // Vijay code - end
    
    function ForeBackDDL(oDisable, o1, o2) {
        document.getElementById(o1).disabled = oDisable;
        if (o2 != null)
            document.getElementById(o2).disabled = oDisable;
    }
    function EnsureBackup(oRad, oD, oW, oWS1, oM, oMS1, oMS2, oT1, oT2, oDate, oLoc) {
        oRad = document.getElementById(oRad);
        if (oRad.checked == true) {
            oD = document.getElementById(oD);
            oW = document.getElementById(oW);
            oM = document.getElementById(oM);
            if (oD.checked == false && oW.checked == false && oM.checked == false) {
                alert('Please select your desired backup frequency');
                oD.focus();
                return false;
            }
            if (oW.checked == true) {
                if (ValidateDropDown(oWS1, 'Please select your preferred day of the week') == false)
                    return false;
            }
//            if (oM.checked == true) {
//                if ((ValidateDropDown(oMS1, 'Please select your preferred backup pattern') == false || ValidateDropDown(oMS2, 'Please select your preferred backup pattern') == false))
//                    return false;
//            }
            if ((ValidateDropDown(oT1, 'Please select a valid start time') == false || ValidateDropDown(oT2, 'Please select a valid start time') == false))
                return false;
            if (ValidateDate(oDate, 'Please enter a valid start date') == false)
                return false;
            if (ValidateDropDown(oLoc, 'Please select a recovery location') == false)
                return false;
        }
        return true;
    }
    var div = null;   
    function UpdateBackupUrl() {       
        var strURL = '<%=Request.Path %>?id=<%=Request.QueryString["id"] %>&child=true';
        var oDaily = document.getElementById('<%=chkDaily.ClientID %>');
        var oWeekly = document.getElementById('<%=chkWeekly.ClientID %>');
        var oMonthly = document.getElementById('<%=chkMonthly.ClientID %>');
        var oDate = document.getElementById('<%= txtDate.ClientID %>');
        var oLocation = document.getElementById('<%=ddlLocation.ClientID %>');
        
        if (oDaily != null) {
            var strTime = "";
            var oTimeHour = document.getElementById('<%=ddlTimeHour.ClientID %>');
            strTime += "&hour=" + oTimeHour.selectedIndex;
            var oTimeSwitch = document.getElementById('<%=ddlTimeSwitch.ClientID %>');
            strTime += "&switch=" + oTimeSwitch.selectedIndex;
            if (oDaily.checked == true) {
                strURL = strURL + "&daily=true";
            }
            if (oWeekly.checked == true) {
                var strWeek = "";
                var oWeeklyDDL = document.getElementById('<%=ddlWeekly.ClientID %>');
                strWeek += "&week=" + oWeeklyDDL.selectedIndex;
                strURL = strURL + "&weekly=true" + strWeek;
            }    
          //  else
                strURL = strURL + strTime;          
        }
        
        // Vijay code - start
        if(oMonthly.checked == true) 
           strURL = strURL + "&monthly=true";         
        if(oDate != null)
           strURL = strURL + "&date="+ oDate.value; // Start date
        strURL = strURL + "&location="+ oLocation.selectedIndex; // Recovery location
        strURL = strURL + "&menu_tab="+div; // Tab value ('Exclusion','Inclusion','Addtl_Configuration')    
        // Vijay code - end
        window.navigate(strURL);
    }   
</script>
<asp:Panel ID="panContinue" runat="server" Visible="false">
<asp:Panel ID="panShared" runat="server" Visible="false">
<table cellpadding="2" cellspacing="1" border="0">
    <tr>
        <td>For shared environments, you only need to configure your data retention requirements. Please add these requirements by clicking the <b>Add Requirement</b> button.</td>
    </tr>
    <tr>
        <td>
            <br />
            <table width="100%" cellpadding="4" cellspacing="0" border="0" style="border:solid 1px #CCCCCC">
                <tr bgcolor="#EEEEEE">
                    <td nowrap colspan="3"><b>File/Folder</b></td>
                    <td nowrap></td>
                </tr>
                <asp:repeater ID="rptRetention2" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td colspan="3"><%# DataBinder.Eval(Container.DataItem, "path") %></td>
                            <td align="right">[<asp:LinkButton ID="btnDeleteRetention" runat="server" CssClass="default" Text="Delete" OnClick="btnDeleteRetention_Click" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' />]</td>
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
                        <asp:Label ID="lblNoneRetention2" runat="server" CssClass="default" Visible="false" Text="<img src='/images/alert.gif' border='0' align='absmiddle'> There are no archive requirements..." />
                    </td>
                </tr>
            </table>
            <table width="500" cellpadding="4" cellspacing="0" border="0">
                <tr>
                    <td align="left"><asp:Button ID="btnAddRetention2" runat="server" CssClass="default" Width="125" Text="Add Requirement" /></td>
                </tr>
            </table>
        </td>
    </tr>
</table>
</asp:Panel>
<asp:Panel ID="panNonShared" runat="server" Visible="false">
<table cellpadding="2" cellspacing="1" border="0">
    <tr>
        <td colspan="2">
        <asp:Label ID="lblBackup" runat="server" CssClass="default" Text="Do you require backups to be associated with this design?<font class='required'>&nbsp;*</font>" /></td>
    </tr>
    <tr>
        <td colspan="2">
            <asp:RadioButton ID="radYes" runat="server" CssClass="default" Text="Yes" GroupName="storage" /> 
            <asp:RadioButton ID="radNo" runat="server" CssClass="default" Text="No" GroupName="storage" />
            <asp:RadioButton ID="radLater" runat="server" CssClass="default" Text="Ask Me Later" GroupName="storage" />
        </td>
    </tr>
</table>
<div id="divNo" runat="server" style="display:none">
    <br />
    <br />
    <table width="100%" cellpadding="2" cellspacing="1" border="0" style="border:solid 1px #999999" bgcolor="#f9f9f9">
        <tr>
            <td>
                <table width="100%" cellpadding="0" cellspacing="5" border="0">
                    <tr>
                        <td rowspan="3" valign="top"><img src="/images/hugeAlert.gif" border="0" align="absmiddle" /></td>
                        <td class="header" width="100%" valign="bottom">
                        <asp:Label ID="lblHeaderVarianceReq" runat="server" CssClass="default" Text=" Waiver Required!" />    
                        </td>
                    </tr>
                    <tr>
                        <td width="100%" valign="top">Any production bound servers that do not require backups will need an approved waiver before it can enter into production.</td>
                    </tr>
                    <tr>
                        <td width="100%" valign="top">
                            <br />Please click the following link to access the waiver...
                            <br /><br /><a href="http://pncpgha37.pncbank.com/netsvc/nwsnesreq.nsf/backup+request?openform" target="_blank"><img src="/images/file.gif" border="0" align="absmiddle" /> Backup Waiver Form</a>
                            <br /><br /><b>NOTE:</b> Be sure to select &quot;No Backup Required&quot; when completing the form.
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <br />
</div>
<div id="divYes" runat="server" style="display:none">
    <br />
    <table cellpadding="2" cellspacing="1" border="0">
        <tr>
            <td colspan="3">
            <asp:Label ID="lblBackupFrequency" runat="server" CssClass="default" Text="Please select your desired backup frequency:<font class='required'>&nbsp;*</font>" /></td>
        </tr>
        <tr>
            <td colspan="3">
                <table cellpadding="1" cellspacing="1" border="0" class="default">                                        
                    <tr>
                        <td><asp:CheckBox ID="chkDaily" runat="server" CssClass="default" Text="Daily" /></td>
                    </tr>
                    <tr>
                        <td><asp:CheckBox ID="chkWeekly" runat="server" CssClass="default" Text="Weekly" /></td>
                    </tr>                    
                    <tr>
                        <td>
                            <div id="divWeekly" runat="server" style="display:none">
                            <table cellpadding="2" cellspacing="3" border="0">
                                <tr>
                                    <td align="right"><img src="/images/arrow_down_right.gif" border="0" align="absmiddle" /></td>
                                    <td>
                                        <asp:Label ID="lblBackupSelectDayOfWeek" runat="server" CssClass="default" Text="Please select your preferred day of the week:" />
                                    </td>
                                </tr>
                                <tr>
                                    <td><img src="/images/spacer.gif" border="0" width="20" height="1" /></td>
                                    <td>
                                        <asp:DropDownList ID="ddlWeekly" runat="server" CssClass="default">
                                            <asp:ListItem Value="0" Text="-- SELECT --" />
                                            <asp:ListItem Value="Sunday" />
                                            <asp:ListItem Value="Monday" />
                                            <asp:ListItem Value="Tuesday" />
                                            <asp:ListItem Value="Wednesday" />
                                            <asp:ListItem Value="Thursday" />
                                            <asp:ListItem Value="Friday" />
                                            <asp:ListItem Value="Saturday" />
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                            <br />
                            </div>
                        </td>
                    </tr>                   
                    <tr>
                        <td><asp:CheckBox ID="chkMonthly" runat="server" CssClass="default" Text="Monthly" /></td>
                    </tr>
                    <tr>
                        <td>
                            <div id="divMonthly" runat="server" style="display:none">
                            <table cellpadding="2" cellspacing="3" border="0">
                                <tr>
                                    <td align="right"><img src="/images/arrow_down_right.gif" border="0" align="absmiddle" /></td>
                                    <td>
                                    <asp:Label ID="lblBackupSelectPattern" runat="server" CssClass="default" Text="Please select your preferred backup pattern:" />
                                    </td>
                                </tr>
                                <tr>
                                    <td><img src="/images/spacer.gif" border="0" width="20" height="1" /></td>
                                    <td>
                                        The <asp:DropDownList ID="ddlMonthlyDay" runat="server" CssClass="default">
                                            <asp:ListItem Value="0" Text="-- SELECT --" />
                                            <asp:ListItem Value="first" />
                                            <asp:ListItem Value="second" />
                                            <asp:ListItem Value="third" />
                                            <asp:ListItem Value="fourth" />
                                            <asp:ListItem Value="last" />
                                        </asp:DropDownList>&nbsp;
                                        <asp:DropDownList ID="ddlMonthlyDays" runat="server" CssClass="default">
                                            <asp:ListItem Value="0" Text="-- SELECT --" />
                                            <asp:ListItem Value="day" />
                                            <asp:ListItem Value="weekday" />
                                            <asp:ListItem Value="weekend day" />
                                            <asp:ListItem Value="Sunday" />
                                            <asp:ListItem Value="Monday" />
                                            <asp:ListItem Value="Tuesday" />
                                            <asp:ListItem Value="Wednesday" />
                                            <asp:ListItem Value="Thursday" />
                                            <asp:ListItem Value="Friday" />
                                            <asp:ListItem Value="Saturday" />
                                        </asp:DropDownList> of each month
                                    </td>
                                </tr>
                            </table>
                            <br />
                            </div>
                        </td>
                    </tr>                    
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="3">&nbsp;</td>
        </tr>
        <tr>
            <td colspan="3">
            <asp:Label ID="lblBackupSelectStartTime" runat="server" CssClass="default" Text="Please select your desired start time:<font class='required'>&nbsp;*</font>" /></td>
        </tr>
        <tr>
            <td colspan="3">
                <table cellpadding="1" cellspacing="1" border="0" class="default">
                    <tr>
                        <td>
                            <asp:DropDownList ID="ddlTimeHour" runat="server" CssClass="default">
                                <asp:ListItem Value="0" Text="-- SELECT --" />
                                <asp:ListItem Value="12:00" />
                                <asp:ListItem Value="12:30" />
                                <asp:ListItem Value="1:00" />
                                <asp:ListItem Value="1:30" />
                                <asp:ListItem Value="2:00" />
                                <asp:ListItem Value="2:30" />
                                <asp:ListItem Value="3:00" />
                                <asp:ListItem Value="3:30" />
                                <asp:ListItem Value="4:00" />
                                <asp:ListItem Value="4:30" />
                                <asp:ListItem Value="5:00" />
                                <asp:ListItem Value="5:30" />
                                <asp:ListItem Value="6:00" />
                                <asp:ListItem Value="6:30" />
                                <asp:ListItem Value="7:00" />
                                <asp:ListItem Value="7:30" />
                                <asp:ListItem Value="8:00" />
                                <asp:ListItem Value="8:30" />
                                <asp:ListItem Value="9:00" />
                                <asp:ListItem Value="9:30" />
                                <asp:ListItem Value="10:00" />
                                <asp:ListItem Value="10:30" />
                                <asp:ListItem Value="11:00" />
                                <asp:ListItem Value="11:30" />
                            </asp:DropDownList>&nbsp;
                            <asp:DropDownList ID="ddlTimeSwitch" runat="server" CssClass="default">
                                <asp:ListItem Value="0" Text="-- SELECT --" />
                                <asp:ListItem Value="AM" />
                                <asp:ListItem Value="PM" />
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="3">&nbsp;</td>
        </tr>
        <tr>
            <td colspan="3">
            <asp:Label ID="lblBackupSelectStartDate" runat="server" CssClass="default" Text="Please select your desired start date:<font class='required'>&nbsp;*</font>" /></td>
        </tr>
        <tr>
            <td colspan="3">
                <table cellpadding="1" cellspacing="1" border="0" class="default">
                    <tr>
                        <td><asp:TextBox ID="txtDate" runat="server" CssClass="default" Width="80" MaxLength="10" /> 
                        <asp:ImageButton ID="imgDate" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calendar.gif" /></td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="3">&nbsp;</td>
        </tr>
        <tr>
            <td colspan="3">
            <asp:Label ID="lblBackupSelectRecoveryLocation" runat="server" CssClass="default" Text="Please select your recovery location:<font class='required'>&nbsp;*</font>" /></td>
        </tr>
        <tr>
            <td colspan="3">
                <table cellpadding="1" cellspacing="1" border="0" class="default">
                    <tr>
                        <td><asp:DropDownList ID="ddlLocation" runat="server" CssClass="default" /></td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="3">&nbsp;</td>
        </tr>
        <tr>
            <td valign="top" width="100%">
             <asp:Panel ID="panBackup" runat="server">
             <%=strMenuTab1 %>
             <!-- 
                 <table cellpadding="0" cellspacing="0" border="0" class="default">
                    <tr>
                        <td><img src="/images/TabEmptyBackground.gif" /></td>
                        <td><img src="/images/TabOnLeftCap.gif" /></td>
                        <td nowrap background="/images/TabOnBackground.gif"><a href="javascript:void(0);" onclick="ChangeTab(this,'divTab1',null,null,false);" class="tabheader">Backup Inclusions</a></td>
                        <td><img src="/images/TabOnRightCap.gif" /></td>
                        <td><img src="/images/TabOffLeftCap.gif" /></td>
                        <td nowrap background="/images/TabOffBackground.gif"><a href="javascript:void(0);" onclick="ChangeTab(this,'divTab2',null,null,false);" class="tabheader" id="a1">Backup Exclusions</a></td>
                        <td><img src="/images/TabOffRightCap.gif" /></td>
                        <td><img src="/images/TabOffLeftCap.gif" /></td>
                        <td nowrap background="/images/TabOffBackground.gif"><a href="javascript:void(0);" onclick="ChangeTab(this,'divTab3',null,null,false);" class="tabheader" id="a2">Archive Requirements</a></td>
                        <td><img src="/images/TabOffRightCap.gif" /></td>
                        <td><img src="/images/TabOffLeftCap.gif" /></td>                      
                        <td nowrap background="/images/TabOffBackground.gif"><a href="javascript:void(0);" onclick="ChangeTab(this,'divTab4',null,null,false);" class="tabheader" id="a3">Additional Configuration</a></td>                                                
                        <td><img src="/images/TabOffRightCap.gif" /></td>
                        <td width="100%" background="/images/TabEmptyBackground.gif">&nbsp;</td>
                    </tr>
                 </table>
                 -->
                 <div id="divMenu1">
                 <div style="display:none">
                   <br />
                     <table width="100%" cellpadding="0" cellspacing="5" border="0">
                        <tr>
                            <td rowspan="2"><img src="/images/backup.gif" border="0" align="absmiddle" /></td>
                            <td class="header" width="100%" valign="bottom">
                                <asp:Label ID="lblHeaderBackupInclusions" runat="server" CssClass="default" Text=" Backup Inclusions" />    
                            </td>
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
                                  <td align="right">[<asp:LinkButton ID="btnDeleteInclusion" runat="server" CssClass="default" Text="Delete" OnClick="btnDeleteInclusion_Click" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' />]</td>
                               </tr>
                            </ItemTemplate>
                        </asp:repeater>
                        <tr>
                           <td colspan="2">
                              <asp:Label ID="lblNoneInclusions" runat="server" CssClass="default" Visible="false" Text="<img src='/images/alert.gif' border='0' align='absmiddle'> There are no inclusions..." />
                           </td>
                        </tr>                       
                     </table>
                     <table width="500" cellpadding="4" cellspacing="0" border="0">
                        <tr>
                           <td align="left"><asp:Button ID="btnAddInclusion" runat="server" CssClass="default" Width="125" Text="Add Inclusion" /></td>
                        </tr>                                          
                     </table>
                 </div>
                 <div style="display:none">
                   <br />
                     <table width="100%" cellpadding="0" cellspacing="5" border="0">
                        <tr>
                            <td rowspan="2"><img src="/images/backup.gif" border="0" align="absmiddle" /></td>
                            <td class="header" width="100%" valign="bottom">
                                <asp:Label ID="lblHeaderBackupExclusions" runat="server" CssClass="default" Text=" Backup Exclusions" />    
                            </td>
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
                                            <td align="right">[<asp:LinkButton ID="btnDeleteExclusion" runat="server" CssClass="default" Text="Delete" OnClick="btnDeleteExclusion_Click" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' />]</td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:repeater>
                                <tr>
                                    <td colspan="2">
                                        <asp:Label ID="lblNoneExclusions" runat="server" CssClass="default" Visible="false" Text="<img src='/images/alert.gif' border='0' align='absmiddle'> There are no exclusions..." />
                                    </td>
                                </tr>
                            </table>
                      <table width="500" cellpadding="4" cellspacing="0" border="0">
                                <tr>
                                    <td align="left"><asp:Button ID="btnAddExclusion" runat="server" CssClass="default" Width="125" Text="Add Exclusion" /></td>
                                </tr>
                     </table> 
                 </div>
                 <div style="display:none">
                   <br />
                      <table width="100%" cellpadding="0" cellspacing="5" border="0">
                        <tr>
                            <td rowspan="2"><img src="/images/backup.gif" border="0" align="absmiddle" /></td>
                            <td class="header" width="100%" valign="bottom">
                                <asp:Label ID="lblHeaderArchiveReqs" runat="server" CssClass="default" Text=" Archive Requirements" />
                            </td>
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
                                            <td align="right">[<asp:LinkButton ID="btnDeleteRetention" runat="server" CssClass="default" Text="Delete" OnClick="btnDeleteRetention_Click" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' />]</td>
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
                            <table width="500" cellpadding="4" cellspacing="0" border="0">
                                <tr>
                                    <td align="left"><asp:Button ID="btnAddRetention" runat="server" CssClass="default" Width="125" Text="Add Requirement" /></td>
                                </tr>
                            </table>                      
                 </div>
                 <div style="display:none">
                 <br />
                  <table width="100%" cellpadding="0" cellspacing="5" border="0">
                        <tr>
                            <td rowspan="2"><img src="/images/backup.gif" border="0" align="absmiddle" /></td>
                            <td class="header" width="100%" valign="bottom"> 
                                <asp:Label ID="lblHeaderAdditionalConfig" runat="server" CssClass="default" Text=" Additional Configuration" />
                            </td>
                        </tr>                         
                      </table>
                     <table width="600" cellpadding="1" cellspacing="1" border="0" class="default">
                                <tr>
                                    <td>
                                    <asp:Label ID="lblConfigAvgSizeOfOneDataFile" runat="server" CssClass="default" Text="Average Size of One Data File:" />
                                    </td>
                                    <td colspan="2"><asp:TextBox ID="txtAverage" runat="server" CssClass="default" Width="80" MaxLength="10" />GB</td>
                                </tr>
                                <tr>
                                    <td>
                                    <asp:Label ID="lblConfigProdTurnOverDocFolderName" runat="server" CssClass="default" Text="Production Turnover<br />Documentation Folder Name:" />
                                    </td>
                                    <td colspan="2"><asp:TextBox ID="txtDocumentation" runat="server" CssClass="default" Width="200" MaxLength="100" /></td>
                                </tr>
                                <tr>
                                    <td colspan="3">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td colspan="2" class="bold">
                                    <asp:Label ID="lblConfigHeaderClientFileSystemData" runat="server" CssClass="default" Text="Client File System Data" />
                                    </td>
                                    <td class="bold">
                                    <asp:Label ID="lblConfigHeaderClientFileSystemDataDefaultValues" runat="server" CssClass="default" Text="Default Values" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblCFPercent" runat="server" CssClass="default" Text="Percent Changed Daily:" />
                                    </td>
                                    <td><asp:TextBox ID="txtCFPercent" runat="server" CssClass="default" Width="80" MaxLength="10" Text="5" />%</td>
                                    <td align="center"><font class="footer">&nbsp;5%</font></td>
                                </tr>
                                <tr>
                                    <td>
                                       <asp:Label ID="lblCFCompression" runat="server" CssClass="default" Text="Compression Ratio:" />
                                    </td>
                                    <td><asp:TextBox ID="txtCFCompression" runat="server" CssClass="default" Width="80" MaxLength="10" Text="60" />%</td>
                                    <td align="center"><font class="footer">&nbsp;60%</font></td>
                                </tr>
                                <tr>
                                    <td>
                                      <asp:Label ID="lblCFAverage" runat="server" CssClass="default" Text="Average File Size:" />
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlCFAverage" runat="server" CssClass="default" Width="80">
                                            <asp:ListItem Value="Medium" />
                                            <asp:ListItem Value="Large" />
                                            <asp:ListItem Value="Small" />
                                        </asp:DropDownList>
                                    </td>
                                    <td align="center"><font class="footer">&nbsp;Medium</font></td>
                                </tr>
                                <tr>
                                    <td>
                                    <asp:Label ID="lblCFBackup" runat="server" CssClass="default" Text="Backup Version Ration:" />
                                    </td>
                                    <td><asp:TextBox ID="txtCFBackup" runat="server" CssClass="default" Width="80" MaxLength="4" Text="1.25" /></td>
                                    <td align="center"><font class="footer">&nbsp;1.25</font></td>
                                </tr>
                                <tr>
                                    <td>
                                     <asp:Label ID="lblCFArchive" runat="server" CssClass="default" Text="Archive Ratio:" />
                                    </td>
                                    <td><asp:TextBox ID="txtCFArchive" runat="server" CssClass="default" Width="80" MaxLength="4" Text="0.25" /></td>
                                    <td align="center"><font class="footer">&nbsp;0.25</font></td>
                                </tr>
                                <tr>
                                    <td>
                                    <asp:Label ID="lblCFWindow" runat="server" CssClass="default" Text="Backup Window (Hours):" />
                                    </td>
                                    <td><asp:TextBox ID="txtCFWindow" runat="server" CssClass="default" Width="80" MaxLength="4" Text="4" /></td>
                                    <td align="center"><font class="footer">&nbsp;4</font></td>
                                </tr>
                                <tr>
                                    <td>
                                     <asp:Label ID="lblCFSets" runat="server" CssClass="default" Text="Backupsets:" />
                                    </td>
                                    <td><asp:TextBox ID="txtCFSets" runat="server" CssClass="default" Width="80" MaxLength="4" Text="0" /></td>
                                    <td align="center"><font class="footer">&nbsp;0</font></td>
                                </tr>
                                <tr>
                                    <td colspan="3">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td colspan="2" class="bold">
                                    <asp:Label ID="lblHeaderConfigClientDatabaseData" runat="server" CssClass="default" Text="Client Database Data" />
                                    </td>
                                    <td class="bold">
                                    <asp:Label ID="lblHeaderConfigClientDatabaseDataDefaultValues" runat="server" CssClass="default" Text="Default Values" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                          <asp:Label ID="lblCDType" runat="server" CssClass="default" Text="Database Type:" />
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlCDType" runat="server" CssClass="default" Width="80">
                                            <asp:ListItem Value="Exchange" />
                                            <asp:ListItem Value="DB2" />
                                            <asp:ListItem Value="None" />
                                        </asp:DropDownList>
                                    </td>
                                    <td align="center"><font class="footer">&nbsp;Exchange</font></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblCDPercent" runat="server" CssClass="default" Text="Percent Changed Daily:" />
                                    </td>
                                    <td><asp:TextBox ID="txtCDPercent" runat="server" CssClass="default" Width="80" MaxLength="4" Text="100" />%</td>
                                    <td align="center"><font class="footer">&nbsp;100%</font></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblCDCompression" runat="server" CssClass="default" Text="Compression Ratio:" />
                                    </td>
                                    <td><asp:TextBox ID="txtCDCompression" runat="server" CssClass="default" Width="80" MaxLength="4" Text="0" />%</td>
                                    <td align="center"><font class="footer">&nbsp;0%</font></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblCDVersions" runat="server" CssClass="default" Text="Number of Backup Versions:" />
                                    </td>
                                    <td><asp:TextBox ID="txtCDVersions" runat="server" CssClass="default" Width="80" MaxLength="4" Text="38" /></td>
                                    <td align="center"><font class="footer">&nbsp;38</font></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblCDWindow" runat="server" CssClass="default" Text="Backup Window (Hours):" />
                                    </td>
                                    <td><asp:TextBox ID="txtCDWindow" runat="server" CssClass="default" Width="80" MaxLength="4" Text="4" /></td>
                                    <td align="center"><font class="footer">&nbsp;4</font></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblCDGrowth" runat="server" CssClass="default" Text="Growth Factor:" />
                                    </td>
                                    <td><asp:TextBox ID="txtCDGrowth" runat="server" CssClass="default" Width="80" MaxLength="4" Text="30" />%</td>
                                    <td align="center"><font class="footer">&nbsp;30%</font></td>
                                </tr>
                                <tr>
                                    <td colspan="3">&nbsp;</td>
                                </tr>
                            </table>
                 </div>
                  </div> 
              </asp:Panel> 
            </td>             
        </tr>
        <tr>
            <td valign="top">
            <br />               
            </td>
            <td><img src="/images/spacer.gif" border="0" width="30" height="1" /></td>
            <td valign="top">
                <br />                
                <br />
                <br />                
            </td>
        </tr>
    </table>
</div>
</asp:Panel>
<table width="100%" cellpadding="4" cellspacing="2" border="0">
    <tr>
        <td colspan="3"><hr size="1" noshade /></td>
    </tr>
    <tr>
        <td class="required">* = Required Field</td>
        <td align="center">
            <asp:Panel ID="panNavigation" runat="server" Visible="false">
                <asp:Button ID="btnBack" runat="server" OnClick="btnBack_Click" Text="Back" CssClass="default" Width="75" /> <asp:Button ID="btnNext" runat="server" OnClick="btnNext_Click" Text="Next" CssClass="default" Width="75" />
            </asp:Panel>
            <asp:Panel ID="panUpdate" runat="server" Visible="false">
                <asp:Button ID="btnUpdate" runat="server" OnClick="btnUpdate_Click" Text="Update" CssClass="default" Width="75" /> <asp:Button ID="btnCancel" runat="server" OnClick="btnCancel_Click" Text="Cancel" CssClass="default" Width="75" />
            </asp:Panel>
        </td>
        <td align="right">
            <asp:Button ID="btnHundred" runat="server" OnClick="btnCancel_Click" Text="Back" CssClass="default" Width="75" Visible="false" /> 
            <asp:Button ID="btnClose" runat="server" Text="Close" CssClass="default" Width="75" />
        </td>
    </tr>
</table>
<asp:HiddenField ID="hdnDone" runat="server" />
</asp:Panel>
<asp:Panel ID="panStop" runat="server" Visible="false">
    <table width="100%" cellpadding="4" cellspacing="0" border="0" style="border:solid 1px #999999" bgcolor="#f9f9f9">
        <tr>
            <td nowrap><img src="/images/bigError.gif" border="0" align="absmiddle" /></td>
            <td width="100%">You cannot configure your storage until a valid model has been selected. Please contact your design implementor if you have questions.</td>
        </tr>
        <tr>
            <td nowrap></td>
            <td width="100%"><asp:Button ID="btnStop" runat="server" OnClick="btnBack_Click" Text="Back" CssClass="default" Width="75" /></td>
        </tr>
    </table>
</asp:Panel>