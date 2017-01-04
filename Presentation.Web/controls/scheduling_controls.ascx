<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="scheduling_controls.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.scheduling_controls" %>


 
<script type="text/javascript">
    function DateSelect(strVar) {
        OpenWindow("SCHEDULING", strVar);
        return false;
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
            <table width="100%" cellpadding="2" cellspacing="0" border="0">
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
            </table>
            <p>&nbsp;</p>
        </td>
        <td background="/images/table_right.gif"><img src="/images/table_right.gif" width="5" height="10"></td>
    </tr>
    <tr>
        <td><img src="/images/table_bottomLeft.gif" border="0" width="5" height="9"></td>
        <td width="100%" background="/images/table_bottom.gif"></td>
        <td><img src="/images/table_bottomRight.gif" border="0" width="5" height="9"></td>
    </tr>
</table>