<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="backup_demand.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.backup_demand" %>

<script type="text/javascript">
    function SwapAmount() {
    setTimeout("SwapAmountHelp()",1000);
    }
    function SwapAmountHelp() {
        oHidden = document.getElementById('<%=hdnAmount.ClientID %>');
        oText = document.getElementById('<%=lblAmount.ClientID %>');
        if (oHidden != null && oText != null)
            oText.innerText = oHidden.value + " GB";
    }
</script>
<script type="text/javascript">SwapAmount();</script>
<br />
<asp:Panel ID="panCalendar" runat="server" Visible="false">
    <table width="100%" cellpadding="0" cellspacing="2" border="0">
        <tr>
            <td id="tdSideBar" valign="top" nowrap style="background-color:#F6F6F6">
                <table width="100%" cellpadding="3" cellspacing="2" border="0">
                    <tr>
                        <td colspan="3">
                            <asp:Panel ID="panParameters" runat="server" Visible="false">
                                <table width="100%" cellpadding="2" cellspacing="0" border="0" style="border:solid 1px #404040" bgcolor="#FFFFFF">
                                    <tr>
                                        <td nowrap><img src="/images/funnel.gif" border="0" align="absmiddle" /></td>
                                        <td width="100%" class="header">Applied Filters</td>
                                    </tr>
                                    <tr>
                                        <td colspan="2"><%=strParameters %></td>
                                    </tr>
                                </table>
                                <br />
                                
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td nowrap><img src="/images/arrow_black_right.gif" border="0" /></td>
                        <td class="bigger" nowrap><b>Filters</b></td>
                        <td align="right"><asp:Button ID="btnGo2" runat="server" CssClass="default" Width="75" Text="Apply" OnClick="btnGo_Click" /></td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                        <td valign="top">Projects:<br /><br /><asp:Button id="btnProjects" runat="server" Text="&lt; &gt;" CssClass="default" ToolTip="Maximize Listing" /> <asp:Button id="btnProjectsClear" runat="server" Text="X" Width="25" CssClass="default" ToolTip="Clear Selection(s)" /></td>
                        <td><asp:ListBox ID="lstProjects" runat="server" CssClass="smalldefault" Width="200" SelectionMode="Multiple" /> </td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                        <td valign="top">Confidence:<br /><br /><asp:Button id="btnConfidences" runat="server" Text="&lt; &gt;" CssClass="default" ToolTip="Maximize Listing" /> <asp:Button id="btnConfidencesClear" runat="server" Text="X" Width="25" CssClass="default" ToolTip="Clear Selection(s)" /></td>
                        <td><asp:ListBox ID="lstConfidences" runat="server" CssClass="smalldefault" Width="200" SelectionMode="Multiple" /> </td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                        <td valign="top">Location:<br /><br /><asp:Button id="btnLocations" runat="server" Text="&lt; &gt;" CssClass="default" ToolTip="Maximize Listing" /> <asp:Button id="btnLocationsClear" runat="server" Text="X" Width="25" CssClass="default" ToolTip="Clear Selection(s)" /></td>
                        <td><asp:ListBox ID="lstLocations" runat="server" CssClass="smalldefault" Width="200" SelectionMode="Multiple" /> </td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                        <td valign="top">Class:<br /><br /><asp:Button id="btnClasses" runat="server" Text="&lt; &gt;" CssClass="default" ToolTip="Maximize Listing" /> <asp:Button id="btnClassesClear" runat="server" Text="X" Width="25" CssClass="default" ToolTip="Clear Selection(s)" /></td>
                        <td><asp:ListBox ID="lstClasses" runat="server" CssClass="smalldefault" Width="200" SelectionMode="Multiple" /> </td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                        <td valign="top">Environment:<br /><br /><asp:Button id="btnEnvironments" runat="server" Text="&lt; &gt;" CssClass="default" ToolTip="Maximize Listing" /> <asp:Button id="btnEnvironmentsClear" runat="server" Text="X" Width="25" CssClass="default" ToolTip="Clear Selection(s)" /></td>
                        <td><asp:ListBox ID="lstEnvironments" runat="server" CssClass="smalldefault" Width="200" SelectionMode="Multiple" /> </td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                        <td>Date Range:</td>
                        <td><asp:TextBox ID="txtStart" runat="server" CssClass="smalldefault" Width="70" MaxLength="10" /> <asp:ImageButton ID="imgStart" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calendar.gif" /> - <asp:TextBox ID="txtEnd" runat="server" CssClass="smalldefault" Width="70" MaxLength="10" /> <asp:ImageButton ID="imgEnd" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calendar.gif" /></td>
                    </tr>
                    <tr>
                        <td colspan="3"><img src="/images/spacer.gif" border="0" width="1" height="1" /></td>
                    </tr>
                    <tr>
                        <td colspan="3" align="right"><asp:Button ID="btnGo1" runat="server" CssClass="default" Width="75" Text="Apply" OnClick="btnGo_Click" /></td>
                    </tr>
                    <tr>
                        <td nowrap><img src="/images/arrow_black_right.gif" border="0" /></td>
                        <td colspan="2" class="bigger" nowrap><b>Saved Views</b></td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                        <td colspan="2"><%=strViews %></td>
                    </tr>
                    <tr>
                        <td colspan="3"><img src="/images/spacer.gif" border="0" width="1" height="1" /></td>
                    </tr>
                </table>
            </td>
            <td valign="top" style="background-color:#C6C6C6;border-right:1px solid #999999;width:6px;padding-top:350px;">
                <a href="javascript:void(0)" onclick="SideBar(this);"><img src="/images/sidebar_collapse.gif" border="0" alt="Collapse Sidebar"></a>
            </td>
            <td valign="top" width="100%" height="100%">
                <table width="100%" cellpadding="3" cellspacing="2" border="0">
                    <tr>
                        <td>
                            <table width="100%" cellpadding="3" cellspacing="0" border="0">
                                <tr>
                                    <td>
                                        <table cellpadding="2" cellspacing="2" border="0">
                                            <tr>
                                                <td>
                                                    <asp:DropDownList ID="ddlMonth" runat="server" CssClass="header" Width="125">
                                                        <asp:ListItem Value="1" Text="January" />
                                                        <asp:ListItem Value="2" Text="February" />
                                                        <asp:ListItem Value="3" Text="March" />
                                                        <asp:ListItem Value="4" Text="April" />
                                                        <asp:ListItem Value="5" Text="May" />
                                                        <asp:ListItem Value="6" Text="June" />
                                                        <asp:ListItem Value="7" Text="July" />
                                                        <asp:ListItem Value="8" Text="August" />
                                                        <asp:ListItem Value="9" Text="September" />
                                                        <asp:ListItem Value="10" Text="October" />
                                                        <asp:ListItem Value="11" Text="November" />
                                                        <asp:ListItem Value="12" Text="December" />
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlYear" runat="server" CssClass="header" Width="75">
                                                        <asp:ListItem Value="2008" Text="2008" />
                                                        <asp:ListItem Value="2009" Text="2009" />
                                                        <asp:ListItem Value="2010" Text="2010" />
                                                        <asp:ListItem Value="2011" Text="2011" />
                                                        <asp:ListItem Value="2012" Text="2012" />
                                                        <asp:ListItem Value="2013" Text="2013" />
                                                        <asp:ListItem Value="2014" Text="2014" />
                                                        <asp:ListItem Value="2015" Text="2015" />
                                                        <asp:ListItem Value="2016" Text="2016" />
                                                    </asp:DropDownList>
                                                </td>
                                                <td><asp:ImageButton ID="btnChange" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/search.gif" OnClick="btnChange_Click" ToolTip="Click here to change the month/year" /></td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td align="right">
                                        <table cellpadding="2" cellspacing="2" border="0">
                                            <tr>
                                                <td class="header">Amount to be Backed Up this Month:</td>
                                                <td class="header"><asp:Label ID="lblAmount" runat="server" CssClass="header" Text="Calculating..." /></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Calendar ID="calThis" runat="server" CellSpacing="1" OnDayRender="DayRender" DayNameFormat="Full" ShowTitle="false" >
                                            <DayStyle BackColor="White" BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" Font-Size="7pt" HorizontalAlign="Left" VerticalAlign="Top" />
                                            <SelectedDayStyle BackColor="White" ForeColor="Black" BorderColor="#FF8080" BorderStyle="Solid" BorderWidth="1px" />
                                            <TodayDayStyle BorderColor="#000099" BorderStyle="Solid" BorderWidth="1px" />
                                            <DayHeaderStyle Font-Names="Verdana" Font-Size="8pt" Width="300" />
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
            </td>
        </tr>
    </table>
</asp:Panel>
<asp:Panel ID="panBackup" runat="server" Visible="false">
<table cellpadding="2" cellspacing="2" border="0">
    <tr>
        <td class="header">Backup Schedule for <asp:Label ID="lblDate" runat="server" CssClass="header" /></td>
    </tr>
</table>
<br />
<table border="0" cellpadding="6" cellspacing="0" style="border:solid 1px #CCCCCC">
    <tr bgcolor="#EEEEEE" class="bold">
        <td nowrap>Project Name</td>
        <td nowrap>Project Number</td>
        <td nowrap>Class</td>
        <td nowrap>Environment</td>
        <td nowrap>Location</td>
        <td nowrap>Start Time</td>
        <td nowrap>Quantity</td>
        <td nowrap>Backup Type</td>
        <td nowrap>Day of Week</td>
        <td nowrap>Confidence Level</td>
        <td nowrap align="right">Backup Amount</td>
    </tr>
    <%=strBackup %>
    <tr class="bold">
        <td colspan="6">Totals:</td>
        <td><%=intQuantity.ToString() %></td>
        <td colspan="3">&nbsp;</td>
        <td align="right"><%=dblTotal.ToString("N") %> GB</td>
    </tr>
</table>
<br />
<table cellpadding="2" cellspacing="2" border="0">
    <tr>
        <td><asp:Button ID="btnBack" runat="server" CssClass="default" Width="150" Text="Back to Calendar" OnClick="btnBack_Click" /></td>
    </tr>
</table>
</asp:Panel>
<asp:HiddenField ID="hdnEnvironment" runat="server" />
<asp:HiddenField ID="hdnAmount" runat="server" />