<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="vacation_control.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.vacation_control" %>



<script type="text/javascript">
    function DateSelect(strDay) {
        OpenWindow("VACATION", "?d=" + strDay);
        return false;
    }
    function ValidateVacation(oMorning, oAfternoon, oDay, oDays, oStart, oEnd, oVacation, oHoliday, oPersonal, oReason, ddlReason) {
        oMorning = document.getElementById(oMorning);
        oAfternoon = document.getElementById(oAfternoon);
        oDay = document.getElementById(oDay);
        oDays = document.getElementById(oDays);
        oVacation = document.getElementById(oVacation);
        oHoliday = document.getElementById(oHoliday);
        oPersonal = document.getElementById(oPersonal);
        oReason = document.getElementById(oReason);
        var boolReturn = false;
        if (oMorning.checked == false && oAfternoon.checked == false && oDay.checked == false && oDays.checked == false) {
            alert('Please select the duration of the event');
            oMorning.focus();
            return false;
        }
        else {
            boolReturn = ValidateDate(oStart, 'Please enter a valid start date');
            if (boolReturn == true && oDays.checked == true)
                    boolReturn = ValidateDate(oEnd, 'Please enter a valid end date');
            if (boolReturn == true) {
                if (oVacation.checked == false && oHoliday.checked == false && oPersonal.checked == false && oReason.checked == false) {
                    alert('Please select the type of event');
                    oMorning.focus();
                    return false;
                }
                else {
                    if (oReason.checked == true)
                        boolReturn = ValidateDropDown(ddlReason, 'Please select a reason');
                }
            }
        }
        return boolReturn;
    }
    function Single(oStart, oEnd, oType, oReason, oSubmit) {
        oStart = document.getElementById(oStart);
        oEnd = document.getElementById(oEnd);
        oType = document.getElementById(oType);
        oReason = document.getElementById(oReason);
        oSubmit = document.getElementById(oSubmit);
        oStart.style.display = "inline";
        oEnd.style.display = "none";
        oType.style.display = "inline";
        oSubmit.style.display = "inline";
    }
    function Multiple(oStart, oEnd, oType, oReason, oSubmit) {
        oStart = document.getElementById(oStart);
        oEnd = document.getElementById(oEnd);
        oType = document.getElementById(oType);
        oReason = document.getElementById(oReason);
        oSubmit = document.getElementById(oSubmit);
        oStart.style.display = "inline";
        oEnd.style.display = "inline";
        oType.style.display = "inline";
        oSubmit.style.display = "inline";
    }
</script>
<table width="100%" cellpadding="0" cellspacing="0" border="0" class="default">
    <tr>
        <td><img src="/images/table_topleft.gif" border="0" width="5" height="26"></td>
        <td nowrap background="/images/table_top.gif" class="greentableheader" width="100%"><asp:Label ID="lblTitle" runat="server" CssClass="greentableheader" /></td>
        <td nowrap background="/images/table_top.gif" class="greentableheader"><asp:LinkButton ID="btnShowAll" runat="server" Text="Show Everyone Out this Month" OnClick="btnShowAll_Click" /></td>
        <td><img src="/images/table_topright.gif" border="0" width="5" height="26"></td>
    </tr>
    <tr>
        <td background="/images/table_left.gif"><img src="/images/table_left.gif" width="5" height="10"></td>
        <td width="100%" bgcolor="#FFFFFF" colspan="2">
            <br />
            <table width="100%" cellpadding="2" cellspacing="0" border="0">
                <tr>
                    <td class="bigAlert" align="center"><asp:Label ID="lblError" runat="server" Text="<img src='/images/bigAlert.gif' border='0' align='absmiddle' /> The following days were not added due to resource availability" Visible="false" /></td>
                </tr>
                <tr>
                    <td>
                        <table width="100%" cellpadding="3" cellspacing="0" border="0">
                            <tr>
                                <td><img src="/images/bigcheck.gif" border="0" align="absmiddle" /> <asp:LinkButton ID="btnToday" runat="server" Text="Go To Today" CssClass="greentableheader" OnClick="btnToday_Click" /></td>
                                <td align="right">
                                    <asp:DropDownList ID="ddlMonth" runat="server" CssClass="default">
                                        <asp:ListItem Text="January" Value="1" />
                                        <asp:ListItem Text="February" Value="2" />
                                        <asp:ListItem Text="March" Value="3" />
                                        <asp:ListItem Text="April" Value="4" />
                                        <asp:ListItem Text="May" Value="5" />
                                        <asp:ListItem Text="June" Value="6" />
                                        <asp:ListItem Text="July" Value="7" />
                                        <asp:ListItem Text="August" Value="8" />
                                        <asp:ListItem Text="September" Value="9" />
                                        <asp:ListItem Text="October" Value="10" />
                                        <asp:ListItem Text="November" Value="11" />
                                        <asp:ListItem Text="December" Value="12" />
                                    </asp:DropDownList>
                                    <asp:DropDownList ID="ddlYear" runat="server" CssClass="default">
                                        <asp:ListItem Value="2006" />
                                        <asp:ListItem Value="2007" />
                                        <asp:ListItem Value="2008" />
                                        <asp:ListItem Value="2009" />
                                        <asp:ListItem Value="2010" />
                                        <asp:ListItem Value="2011" />
                                        <asp:ListItem Value="2012" />
                                        <asp:ListItem Value="2013" />
                                        <asp:ListItem Value="2014" />
                                        <asp:ListItem Value="2015" />
                                        <asp:ListItem Value="2016" />
                                        <asp:ListItem Value="2017" />
                                        <asp:ListItem Value="2018" />
                                        <asp:ListItem Value="2019" />
                                    </asp:DropDownList>
                                    <asp:ImageButton ID="btnGo" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/search.gif" OnClick="btnGo_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:Calendar ID="calThis" runat="server" CellSpacing="1" ShowNextPrevMonth="true" OnDayRender="DayRender" OnSelectionChanged="ChangeDate" OnVisibleMonthChanged="ChangeMonth" PrevMonthText="<img src='/images/arrow-left.gif' border='0'>" NextMonthText="<img src='/images/arrow-right.gif' border='0'>" NextPrevFormat="CustomText" DayNameFormat="Short" >
                                        <DayStyle BackColor="White" BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" Font-Size="7pt" HorizontalAlign="Left" VerticalAlign="Top" />
                                        <TitleStyle BackColor="#E6E9F0" Font-Names="Verdana" Font-Size="11pt" ForeColor="Black" Font-Bold="True" />
                                        <SelectedDayStyle BackColor="White" ForeColor="Black" BorderColor="#FF8080" BorderStyle="Solid" BorderWidth="1px" />
                                        <TodayDayStyle BorderColor="#000099" BorderStyle="Solid" BorderWidth="1px" />
                                        <DayHeaderStyle Font-Names="Verdana" Font-Size="9pt" Width="100px" />
                                        <NextPrevStyle BackColor="#E6E9F0" Font-Names="Verdana" Font-Size="11pt" ForeColor="Black" />
                                        <OtherMonthDayStyle BackColor="#F6F6F6" BorderColor="#CCCCCC" ForeColor="#999999" />
                                        <WeekendDayStyle BackColor="#F6F6F6" BorderColor="#CCCCCC" ForeColor="#999999" Width="12.5%" />
                                    </asp:Calendar>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td class="bigAlert" align="center"><asp:Label ID="lblConfigure" runat="server" Text="<img src='/images/bigAlert.gif' border='0' align='absmiddle' /> Vacation Information Not Configured" Visible="false" /></td>
                </tr>
                <tr>
                    <td>
                        <table width="100%" cellpadding="3" cellspacing="0" border="0" class="offsettable">
                            <tr>
                                <td width="100%" valign="top">
                                    <table width="100%" cellpadding="3" cellspacing="0" border="0">
                                        <tr>
                                            <td colspan="3" class="greentableheader">Add an Event</td>
                                        </tr>
                                        <tr>
                                            <td colspan="3" width="100%">
                                                <asp:RadioButton ID="radMorning" runat="server" CssClass="default" Text="Morning" GroupName="Duration" /> 
                                                <asp:RadioButton ID="radAfternoon" runat="server" CssClass="default" Text="Afternoon" GroupName="Duration" /> 
                                                <asp:RadioButton ID="radDay" runat="server" CssClass="default" Text="All Day" GroupName="Duration" /> 
                                                <asp:RadioButton ID="radDays" runat="server" CssClass="default" Text="Multiple Days" GroupName="Duration" /> 
                                            </td>
                                        </tr>
                                        <tr id="divStartDate" runat="server" style="display:none">
                                            <td nowrap><img src="/images/spacer.gif" border="0" height="1" width="25" /></td>
                                            <td nowrap><b>Start Date:</b></td>
                                            <td width="100%"><asp:TextBox ID="txtStart" runat="server" Width="100" CssClass="default" /> <asp:ImageButton ID="imgStart" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calendar.gif" /></td>
                                        </tr>
                                        <tr id="divEndDate" runat="server" style="display:none">
                                            <td nowrap><img src="/images/spacer.gif" border="0" height="1" width="25" /></td>
                                            <td nowrap><b>End Date:</b></td>
                                            <td width="100%"><asp:TextBox ID="txtEnd" runat="server" Width="100" CssClass="default" /> <asp:ImageButton ID="imgEnd" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calendar.gif" /></td>
                                        </tr>
                                        <tr id="divType" runat="server" style="display:none">
                                            <td nowrap><img src="/images/spacer.gif" border="0" height="1" width="25" /></td>
                                            <td nowrap><b>Type:</b></td>
                                            <td width="100%">
                                                <asp:RadioButton ID="radVacation" runat="server" CssClass="default" Text="Vacation" GroupName="Reason" /> 
                                                <asp:RadioButton ID="radPersonal" runat="server" CssClass="default" Text="Personal" GroupName="Reason" /> 
                                                <asp:RadioButton ID="radHoliday" runat="server" CssClass="default" Text="Floating Holiday" GroupName="Reason" /> 
                                                <asp:RadioButton ID="radReason" runat="server" CssClass="default" Text="Other" GroupName="Reason" /> 
                                            </td>
                                        </tr>
                                        <tr id="divReason" runat="server" style="display:none">
                                            <td nowrap><img src="/images/spacer.gif" border="0" height="1" width="25" /></td>
                                            <td nowrap><b>Reason:</b></td>
                                            <td width="100%">
                                                <asp:DropDownList ID="ddlReason" runat="server" CssClass="default">
                                                    <asp:ListItem Value="-- SELECT --" />
                                                    <asp:ListItem Value="Business Traval" />
                                                    <asp:ListItem Value="Training" />
                                                    <asp:ListItem Value="Funeral" />
                                                    <asp:ListItem Value="Jury Duty" />
                                                    <asp:ListItem Value="Maternity Leave" />
                                                    <asp:ListItem Value="Military Leave" />
                                                    <asp:ListItem Value="DR Testing" />
                                                    <asp:ListItem Value="Highland Hills" />
                                                    <asp:ListItem Value="Long Term Disability" />
                                                    <asp:ListItem Value="Short Term Disability" />
                                                    <asp:ListItem Value="Compensation Time" />
                                                    <asp:ListItem Value="Other" />
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr id="divSubmit" runat="server" style="display:none">
                                            <td nowrap><img src="/images/spacer.gif" border="0" height="1" width="25" /></td>
                                            <td nowrap>&nbsp;</td>
                                            <td width="100%"><asp:Button ID="btnSubmit" runat="server" Width="75" CssClass="default" Text="Submit" OnClick="btnSubmit_Click" /></td>
                                        </tr>
                                    </table>
                                </td>
                                <td nowrap><img src="/images/spacer.gif" border="0" height="1" width="50" /></td>
                                <td valign="top">
                                    <table width="100%" cellpadding="3" cellspacing="2" border="0">
                                        <tr>
                                            <td colspan="3" class="greentableheader">Remaining Days Available</td>
                                        </tr>
                                        <tr>
                                            <td nowrap rowspan="3"><img src="/images/spacer.gif" border="0" height="1" width="25" /></td>
                                            <td nowrap><b>Vacation Days:</b></td>
                                            <td width="100%"><asp:Label ID="lblVacation" runat="server" CssClass="default" /></td>
                                        </tr>
                                        <tr>
                                            <td nowrap><b>Floating Holidays:</b></td>
                                            <td width="100%"><asp:Label ID="lblFloating" runat="server" CssClass="default" /></td>
                                        </tr>
                                        <tr>
                                            <td nowrap><b>Personal / Sick Days:</b></td>
                                            <td width="100%"><asp:Label ID="lblPersonal" runat="server" CssClass="default" /></td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">&nbsp;</td>
                                        </tr>
                                    </table>
                                </td>
                                <td nowrap><img src="/images/spacer.gif" border="0" height="1" width="15" /></td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td class="hugeheader"><img src="/images/clock.gif" border="0" align="absmiddle" /> My Scheduled Time Off</td>
                </tr>
                <tr>
                    <td>
                        <table width="100%" cellpadding="4" cellspacing="0" border="0" align="center" style="border:solid 1px #CCCCCC">
                            <tr bgcolor="#EEEEEE">
                                <td><b>Date</b></td>
                                <td><b>Type</b></td>
                                <td><b>Reason</b></td>
                                <td><b>Status</b></td>
                                <td></td>
                            </tr>
                            <asp:repeater ID="rptView" runat="server">
                                <ItemTemplate>
                                    <tr>
                                        <td><asp:Label ID="lblDate" runat="server" CssClass="default" Text='<%# DateTime.Parse(DataBinder.Eval(Container.DataItem, "start_date").ToString()).ToString("ddd, MMM dd, yyyy")%>' /></td>
                                        <td><%# DataBinder.Eval(Container.DataItem, "duration") %></td>
                                        <td><%# DataBinder.Eval(Container.DataItem, "reason") %></td>
                                        <td><%# (DataBinder.Eval(Container.DataItem, "approved").ToString() == "1" ? "Approved" : "Pending") %></td>
                                        <td align="right"><asp:LinkButton ID="btnDelete" runat="server" Text="[Delete]" OnClick="btnDelete_Click" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "vacationid") %>' /></td>
                                    </tr>
                                </ItemTemplate>
                            </asp:repeater>
                            <tr>
                                <td colspan="5">
                                    <asp:Label ID="lblNone" runat="server" CssClass="default" Visible="false" Text="<img src='/images/spacer.gif' border='0' height='1' width='25' /><img src='/images/alert.gif' border='0' align='absmiddle'> No scheduled time off" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <p>&nbsp;</p>
        </td>
        <td background="/images/table_right.gif"><img src="/images/table_right.gif" width="5" height="10"></td>
    </tr>
    <tr>
        <td><img src="/images/table_bottomLeft.gif" border="0" width="5" height="9"></td>
        <td width="100%" background="/images/table_bottom.gif" colspan="2"></td>
        <td><img src="/images/table_bottomRight.gif" border="0" width="5" height="9"></td>
    </tr>
</table>