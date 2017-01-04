<%@ Page Language="C#" MasterPageFile="~/frame.Master" AutoEventWireup="true" CodeBehind="calendar.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.calendar" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" Runat="Server">
<script type="text/javascript">
    function DateSelect(strDay) {
        if (window.opener == null)
            window.top.UpdateCalendar(strDay);
        else {
            window.opener.CloseCalendar(strDay);
            window.close();
        }
        return false;
    }
    function CalendarOver(oCell) {
        oCell.style.cursor = "hand";
        oCell.style.border = "solid 1px #FF8080";
    }
    function CalendarOut(oCell) {
        oCell.style.cursor = "default";
        oCell.style.border = "solid 1px #E0E0E0";
    }
</script>
<table width="100%" cellpadding="0" cellspacing="0" border="0">
    <tr>
        <td align="center">
            <asp:Calendar ID="calThis" runat="server" CellSpacing="1" ShowNextPrevMonth="true" OnDayRender="DayRender" OnSelectionChanged="ChangeDate" OnVisibleMonthChanged="ChangeMonth" PrevMonthText="<img src='/images/calendar_left.gif' border='0'>" NextMonthText="<img src='/images/calendar_right.gif' border='0'>" Height="250px" Width="250px" >
                <DayStyle BackColor="White" BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Left" VerticalAlign="Top" />
                <TitleStyle BackColor="#333333" Font-Names="Verdana" Font-Size="10pt" ForeColor="White" Font-Bold="True" />
                <SelectedDayStyle BackColor="White" ForeColor="Black" BorderColor="#FF8080" BorderStyle="Solid" BorderWidth="1px" Font-Bold="True" />
                <TodayDayStyle BorderColor="#000099" BorderStyle="Solid" BorderWidth="1px" />
                <DayHeaderStyle Font-Names="Verdana" Font-Size="8pt" />
                <NextPrevStyle BackColor="#333333" Font-Names="Verdana" Font-Size="8pt" ForeColor="White" />
                <OtherMonthDayStyle BackColor="#E6E9F0" BorderColor="#CCCCCC" />
                <WeekendDayStyle BackColor="#E6E9F0" BorderColor="#CCCCCC" />
            </asp:Calendar>
        </td>
    </tr>
    <tr>
        <td align="center" style="border:solid 1px #CCCCCC"><img src="/images/check.gif" border="0" align="absmiddle" /> <asp:LinkButton ID="btnToday" runat="server" Text="Click here for Today's Date" /></td>
    </tr>
</table>
</asp:Content>
