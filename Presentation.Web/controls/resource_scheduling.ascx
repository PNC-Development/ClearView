<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="resource_scheduling.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.resource_scheduling" %>


<script type="text/javascript">
  function test()
  {
    alert('hello');
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
                        <table cellpadding="4" cellspacing="2" border="0">
                            <tr>
                                <td nowrap class="default">Contractor Name:<font class="required">*</font></td>
                                <td><asp:DropDownList ID="ddlUser" runat="server" CssClass="default" /></td>
                            </tr>
                            <tr>
                                <td class="default">Title: <font class="required">*</font></td>
                                <td><asp:TextBox ID="txtTitle" CssClass="default" runat="server" Width="400" MaxLength="50" /></td>
                            </tr>
                            <tr>
                                <td class="default">Start Date: <font class="required">*</font></td>
                                <td><asp:TextBox ID="txtStartDate" CssClass="default" runat="server" Width="75" MaxLength="30" /> <asp:ImageButton ID="imgStartDate" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calendar.gif" /></td>
                            </tr>
                            <tr>
                                <td class="default">End Date: <font class="required">*</font></td>
                                <td><asp:TextBox ID="txtEndDate" CssClass="default" runat="server" Width="75" MaxLength="20" /> <asp:ImageButton ID="imgEndDate" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calendar.gif" /></td>
                            </tr>
                            <tr>
                                <td class="default">Start Time: <font class="required">*</font></td>
                                <td>
                                    <asp:DropDownList ID="ddlStart" runat="server" CssClass="default">
                                        <asp:ListItem Value="0" Text="-- SELECT --" />
                                        <asp:ListItem Value="8:00 AM" />
                                        <asp:ListItem Value="8:30 AM" />
                                        <asp:ListItem Value="9:00 AM" />
                                        <asp:ListItem Value="9:30 AM" />
                                        <asp:ListItem Value="10:00 AM" />
                                        <asp:ListItem Value="10:30 AM" />
                                        <asp:ListItem Value="11:00 AM" />
                                        <asp:ListItem Value="11:30 AM" />
                                        <asp:ListItem Value="12:00 PM" />
                                        <asp:ListItem Value="12:30 PM" />
                                        <asp:ListItem Value="1:00 PM" />
                                        <asp:ListItem Value="1:30 PM" />
                                        <asp:ListItem Value="2:00 PM" />
                                        <asp:ListItem Value="2:30 PM" />
                                        <asp:ListItem Value="3:00 PM" />
                                        <asp:ListItem Value="3:30 PM" />
                                        <asp:ListItem Value="4:00 PM" />
                                        <asp:ListItem Value="4:30 PM" />
                                        <asp:ListItem Value="5:00 PM" />
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="default">End Time: <font class="required">*</font></td>
                                <td>
                                    <asp:DropDownList ID="ddlEnd" runat="server" CssClass="default">
                                        <asp:ListItem Value="0" Text="-- SELECT --" />
                                        <asp:ListItem Value="8:00 AM" />
                                        <asp:ListItem Value="8:30 AM" />
                                        <asp:ListItem Value="9:00 AM" />
                                        <asp:ListItem Value="9:30 AM" />
                                        <asp:ListItem Value="10:00 AM" />
                                        <asp:ListItem Value="10:30 AM" />
                                        <asp:ListItem Value="11:00 AM" />
                                        <asp:ListItem Value="11:30 AM" />
                                        <asp:ListItem Value="12:00 PM" />
                                        <asp:ListItem Value="12:30 PM" />
                                        <asp:ListItem Value="1:00 PM" />
                                        <asp:ListItem Value="1:30 PM" />
                                        <asp:ListItem Value="2:00 PM" />
                                        <asp:ListItem Value="2:30 PM" />
                                        <asp:ListItem Value="3:00 PM" />
                                        <asp:ListItem Value="3:30 PM" />
                                        <asp:ListItem Value="4:00 PM" />
                                        <asp:ListItem Value="4:30 PM" />
                                        <asp:ListItem Value="5:00 PM" />
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td nowrap></td>
                                <td>
                                    <table width="100%" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td><asp:Button ID="btnAdd" CssClass="default" runat="server" Text="Add" Width="100" OnClick="btnAdd_Click" /></td>
                                            <td align="right" class="required">* = Required Fields</td>
                                        </tr>
                                    </table>
                                </td> 
                            </tr>
                        </table>
                    </td>
                </tr> 
                <tr>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>
                        <table width="100%" border="0" cellpadding="0" cellspacing="0"><br />
                            <tr>
                                <td>
                                    <table cellpadding="0" cellspacing="0" border="0">
                                        <tr>
                                            <td><img src="/images/TabOnLeftCap.gif"></td>
                                            <td nowrap background="/images/TabOnBackground.gif"><a href="javascript:void(0);" onclick="ChangeTab(this,'divTab1','<%=hdnTab.ClientID%>','M');" class="tabheader">Monthly View</a></td>
                                            <td><img src="/images/TabOnRightCap.gif"></td>
                                            <td><img src="/images/TabOffLeftCap.gif"></td>
                                            <td nowrap background="/images/TabOffBackground.gif"><a href="javascript:void(0);" onclick="ChangeTab(this,'divTab2','<%=hdnTab.ClientID%>','W');" class="tabheader">Weekly View</a></td>
                                            <td><img src="/images/TabOffRightCap.gif"></td>
                                            <td><img src="/images/TabOffLeftCap.gif"></td>
                                            <td nowrap background="/images/TabOffBackground.gif"><a href="javascript:void(0);" onclick="ChangeTab(this,'divTab3','<%=hdnTab.ClientID%>','D');" class="tabheader">Daily View</a></td>
                                            <td><img src="/images/TabOffRightCap.gif"></td>
                                            <td width="100%" background="/images/TabEmptyBackground.gif">&nbsp;</td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr> 
                                <td>
                                    <div id="divTab1" style='<%=boolMonthly == true ? "display:inline": "display:none" %>'>
                                        <asp:Calendar ID="CalendarMonth" Width="100%" runat="server" CellSpacing="1" ShowNextPrevMonth="true" OnDayRender="DayRender" OnVisibleMonthChanged="ChangeMonth" PrevMonthText="<img src='/images/arrow-left.gif' border='0'>" NextMonthText="<img src='/images/arrow-right.gif' border='0'>" NextPrevFormat="CustomText" DayNameFormat="Short">
                                        <DayStyle BackColor="White" BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" Font-Size="7pt" HorizontalAlign="Left" VerticalAlign="Top" />
                                        <TitleStyle BackColor="#E6E9F0" Font-Names="Verdana" Font-Size="11pt" ForeColor="Black" Font-Bold="True" />
                                        <SelectedDayStyle BackColor="White" ForeColor="Black" BorderColor="#FF8080" BorderStyle="Solid" BorderWidth="1px" />
                                        <TodayDayStyle BorderColor="#000099" BorderStyle="Solid" BorderWidth="1px" />
                                        <DayHeaderStyle Font-Names="Verdana" Font-Size="9pt" />
                                        <NextPrevStyle BackColor="#E6E9F0" Font-Names="Verdana" Font-Size="11pt" ForeColor="Black" />
                                        <OtherMonthDayStyle BackColor="#F6F6F6" BorderColor="#CCCCCC" ForeColor="#999999" />
                                        <WeekendDayStyle BackColor="#F6F6F6" BorderColor="#CCCCCC" ForeColor="#999999" Width="12.5%" />
                                        </asp:Calendar>
                                    </div>
                                    <div id="divTab2" style='<%=boolWeekly == true ? "display:inline": "display:none" %>'>
                                        <%= strWeekView %> 
                                    </div>
                                    <div id="divTab3" style='<%=boolDaily == true ? "display:inline":"display:none" %>'>                                        
                                        <asp:PlaceHolder ID="phDiv" runat="server"></asp:PlaceHolder>    
                                    </div>
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
<asp:HiddenField ID="hdnTab" runat="server" />        
