<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="backup_config_tsm.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.backup_config_tsm" %>
<table width="100%" cellpadding="0" cellspacing="5" border="0">
    <tr>
        <td rowspan="2"><img src="/images/tape.gif" border="0" align="absmiddle" /></td>
        <td class="header" width="100%" valign="bottom">TSM Administration</td>
    </tr>
    <tr>
        <td width="100%" valign="top">Use this page to administer TSM servers, domains and schedules.</td>
    </tr>
</table>
<table width="100%" cellpadding="0" cellspacing="5" border="0">
    <tr>
        <td><asp:Label ID="lblCrumbs" runat="server" /></td>
    </tr>
</table>
<asp:Label ID="lblMessage" runat="server" />
<asp:Panel ID="panServers" runat="server" Visible="false">
    <table width="100%" cellpadding="4" cellspacing="0" border="0" align="center" style="border:solid 1px #CCCCCC">
        <tr bgcolor="#EEEEEE">
            <td width="40%" nowrap><asp:LinkButton ID="btnTSMName" runat="server" CssClass="tableheader" Text="<b>Name</b>" OnClick="btnOrder_Click" CommandArgument="name" /></td>
            <td width="15%" nowrap><asp:LinkButton ID="btnTSMPort" runat="server" CssClass="tableheader" Text="<b>Port</b>" OnClick="btnOrder_Click" CommandArgument="port" /></td>
            <td width="15%" nowrap><asp:LinkButton ID="btnTSMRegistered" runat="server" CssClass="tableheader" Text="<b>Registered</b>" OnClick="btnOrder_Click" CommandArgument="count" /></td>
            <td width="30%" nowrap><asp:LinkButton ID="btnTSMModified" runat="server" CssClass="tableheader" Text="<b>Modified</b>" OnClick="btnOrder_Click" CommandArgument="modified" /></td>
            <td nowrap align="center"><asp:LinkButton ID="btnTSMEnabled" runat="server" CssClass="tableheader" Text="<b>Enabled</b>" OnClick="btnOrder_Click" CommandArgument="enabled" /></td>
        </tr>
    <asp:repeater ID="rptServers" runat="server">
        <ItemTemplate>
            <tr class='<%# DataBinder.Eval(Container.DataItem, "enabled").ToString() == "1" ? "default" : "component_unavailable" %>' onmouseover="CellRowOver(this);" onmouseout="CellRowOut(this);" onclick="window.navigate('<%# FormURL("server=" + DataBinder.Eval(Container.DataItem, "id")) %>');">
                <td width="40%" nowrap><%# DataBinder.Eval(Container.DataItem, "name") %></td>
                <td width="15%"><%# DataBinder.Eval(Container.DataItem, "port") %></td>
                <td width="15%"><%# DataBinder.Eval(Container.DataItem, "count") %></td>
                <td width="30%" nowrap><%# DataBinder.Eval(Container.DataItem, "modified") %></td>
                <td nowrap align="center"><img src="/images/<%# DataBinder.Eval(Container.DataItem, "enabled").ToString() == "1" ? "check" : "cancel" %>.gif" border="0" align="absmiddle" /></td>
            </tr>
        </ItemTemplate>
        <AlternatingItemTemplate>
            <tr class='<%# DataBinder.Eval(Container.DataItem, "enabled").ToString() == "1" ? "default" : "component_unavailable" %>' bgcolor="#F6F6F6" onmouseover="CellRowOver(this);" onmouseout="CellRowOut(this);" onclick="window.navigate('<%# FormURL("server=" + DataBinder.Eval(Container.DataItem, "id")) %>');">
                <td width="40%" nowrap><%# DataBinder.Eval(Container.DataItem, "name") %></td>
                <td width="15%"><%# DataBinder.Eval(Container.DataItem, "port") %></td>
                <td width="15%"><%# DataBinder.Eval(Container.DataItem, "count") %></td>
                <td width="30%" nowrap><%# DataBinder.Eval(Container.DataItem, "modified") %></td>
                <td nowrap align="center"><img src="/images/<%# DataBinder.Eval(Container.DataItem, "enabled").ToString() == "1" ? "check" : "cancel" %>.gif" border="0" align="absmiddle" /></td>
            </tr>
        </AlternatingItemTemplate>
    </asp:repeater>
        <tr>
            <td colspan="6"><asp:Label ID="lblServers" runat="server" CssClass="error" Visible="false" Text="<img src='/images/bigAlert.gif' border='0' align='absmiddle'> There are no servers" /></td>
        </tr>
    </table>
    <br /><br />
    <asp:Button ID="btnAddServer" runat="server" CssClass="default" Width="75" Text="Add New" OnClick="btnAddServer_Click" />
</asp:Panel>
<asp:Panel ID="panServer" runat="server" Visible="false">
    <table width="100%" cellpadding="4" cellspacing="3" border="0">
        <tr>
            <td nowrap>Name:</td>
            <td width="100%"><asp:TextBox ID="txtServerName" runat="server" CssClass="default" Width="400" MaxLength="100" /></td>
        </tr>
        <tr>
            <td nowrap>Port:</td>
            <td width="100%"><asp:TextBox ID="txtServerPort" runat="server" CssClass="default" Width="100" MaxLength="10" /></td>
        </tr>
        <tr>
            <td nowrap>Location:</td>
            <td width="100%"><%=strLocation %></td>
        </tr>
        <tr>
            <td nowrap>DSM.OPT File:</td>
            <td width="100%"><asp:TextBox ID="txtServerPath" runat="server" CssClass="default" Width="600" MaxLength="200" /></td>
        </tr>
        <tr>
            <td nowrap></td>
            <td><asp:HyperLink ID="hypServerPath" runat="server" Text="View the DSM.OPT File" Target="_blank" /></td>
        </tr>
        <tr>
            <td nowrap></td>
            <td><asp:FileUpload ID="txtFile" runat="server" CssClass="default" Width="600" Height="18" /></td>
        </tr>
        <tr>
            <td nowrap>Enabled:</td>
            <td width="100%"><asp:CheckBox ID="chkServerEnabled" runat="server" CssClass="default" /></td>
        </tr>
        <tr>
            <td nowrap></td>
            <td width="100%"><asp:Button ID="btnServerAdd" runat="server" CssClass="default" Width="100" Text="Add" OnClick="btnServerAdd_Click" Visible="false" /><asp:Button ID="btnServerUpdate" runat="server" CssClass="default" Width="100" Text="Update" OnClick="btnServerUpdate_Click" Visible="false" /> <asp:Button ID="btnServerDelete" runat="server" CssClass="default" Width="100" Text="Delete" OnClick="btnServerDelete_Click" Visible="false" /> <asp:Button ID="btnServerCancel" runat="server" CssClass="default" Width="100" Text="Cancel" OnClick="btnServerCancel_Click" /></td>
        </tr>
    </table>
    <br /><br />
    <%=strMenuTab1 %>
    <div id="divMenu1" class="tabbing">
        <br />
        <div style="display:none">
            <span class="header">Domains</span>
            <br /><br />
            <table width="100%" cellpadding="4" cellspacing="0" border="0" align="center" style="border:solid 1px #CCCCCC">
                <tr bgcolor="#EEEEEE">
                    <td width="45%" nowrap><asp:LinkButton ID="btnDomainName" runat="server" CssClass="tableheader" Text="<b>Name</b>" OnClick="btnOrder_Click" CommandArgument="name" /></td>
                    <td width="15%" nowrap><asp:LinkButton ID="btnDomainRegistered" runat="server" CssClass="tableheader" Text="<b>Registered</b>" OnClick="btnOrder_Click" CommandArgument="count" /></td>
                    <td width="40%" nowrap><asp:LinkButton ID="btnDomainModified" runat="server" CssClass="tableheader" Text="<b>Modified</b>" OnClick="btnOrder_Click" CommandArgument="modified" /></td>
                    <td nowrap align="center"><asp:LinkButton ID="btnDomainEnabled" runat="server" CssClass="tableheader" Text="<b>Enabled</b>" OnClick="btnOrder_Click" CommandArgument="enabled" /></td>
                </tr>
            <asp:repeater ID="rptDomains" runat="server">
                <ItemTemplate>
                    <tr class='<%# DataBinder.Eval(Container.DataItem, "enabled").ToString() == "1" ? "default" : "component_unavailable" %>' onmouseover="CellRowOver(this);" onmouseout="CellRowOut(this);" onclick="window.navigate('<%# FormURL("server=" + intServer.ToString() + "&domain=" + DataBinder.Eval(Container.DataItem, "id")) %>');">
                        <td width="45%" nowrap><%# DataBinder.Eval(Container.DataItem, "name") %></td>
                        <td width="15%" nowrap><%# DataBinder.Eval(Container.DataItem, "count") %></td>
                        <td width="40%" nowrap><%# DataBinder.Eval(Container.DataItem, "modified") %></td>
                        <td nowrap align="center"><img src="/images/<%# DataBinder.Eval(Container.DataItem, "enabled").ToString() == "1" ? "check" : "cancel" %>.gif" border="0" align="absmiddle" /></td>
                    </tr>
                </ItemTemplate>
                <AlternatingItemTemplate>
                    <tr class='<%# DataBinder.Eval(Container.DataItem, "enabled").ToString() == "1" ? "default" : "component_unavailable" %>' bgcolor="#F6F6F6" onmouseover="CellRowOver(this);" onmouseout="CellRowOut(this);" onclick="window.navigate('<%# FormURL("server=" + intServer.ToString() + "&domain=" + DataBinder.Eval(Container.DataItem, "id")) %>');">
                        <td width="45%" nowrap><%# DataBinder.Eval(Container.DataItem, "name") %></td>
                        <td width="15%" nowrap><%# DataBinder.Eval(Container.DataItem, "count") %></td>
                        <td width="40%" nowrap><%# DataBinder.Eval(Container.DataItem, "modified") %></td>
                        <td nowrap align="center"><img src="/images/<%# DataBinder.Eval(Container.DataItem, "enabled").ToString() == "1" ? "check" : "cancel" %>.gif" border="0" align="absmiddle" /></td>
                    </tr>
                </AlternatingItemTemplate>
            </asp:repeater>
                <tr>
                    <td colspan="6"><asp:Label ID="lblDomains" runat="server" CssClass="error" Visible="false" Text="<img src='/images/bigAlert.gif' border='0' align='absmiddle'> There are no domains for this server" /></td>
                </tr>
            </table>
            <br /><br />
            <asp:Button ID="btnAddDomain" runat="server" CssClass="default" Width="75" Text="Add New" OnClick="btnAddDomain_Click" />
        </div>
        <div style="display:none">
            <span class="header">Mnemonics</span>
            <br /><br />
            <table width="100%" cellpadding="4" cellspacing="0" border="0" align="center" style="border:solid 1px #CCCCCC">
                <tr bgcolor="#EEEEEE">
                    <td width="60%" nowrap><b>Name</b></td>
                    <td width="40%" nowrap><b>Modified</b></td>
                    <td nowrap align="center"><b>Enabled</b></td>
                </tr>
            <asp:repeater ID="rptMnemonics" runat="server">
                <ItemTemplate>
                    <tr onmouseover="CellRowOver(this);" onmouseout="CellRowOut(this);" onclick="window.navigate('<%# FormURL("server=" + intServer.ToString() + "&mnemonic=" + DataBinder.Eval(Container.DataItem, "id")) %>');">
                        <td width="60%" nowrap><%# DataBinder.Eval(Container.DataItem, "factory_code")%> - <%# DataBinder.Eval(Container.DataItem, "name")%></td>
                        <td width="40%" nowrap><%# DataBinder.Eval(Container.DataItem, "modified") %></td>
                        <td nowrap align="center"><img src="/images/<%# DataBinder.Eval(Container.DataItem, "enabled").ToString() == "1" ? "check" : "cancel" %>.gif" border="0" align="absmiddle" /></td>
                    </tr>
                </ItemTemplate>
            </asp:repeater>
                <tr>
                    <td colspan="6"><asp:Label ID="lblMnemonics" runat="server" CssClass="default" Visible="false" Text="<img src='/images/bigCheck.gif' border='0' align='absmiddle'> This TSM server is available for all mnemonics" /></td>
                </tr>
            </table>
            <br />
            <table width="100%" cellpadding="4" cellspacing="0" border="0" align="center">
                <tr>
                    <td>
                        <asp:Button ID="btnAddMnemonic" runat="server" CssClass="default" Width="75" Text="Add New" OnClick="btnAddMnemonic_Click" />
                    </td>
                    <td align="right">
                        <img src="/images/alert.gif" border="0" align="absmiddle" /> This TSM server will only be available for the mnemonics listed above.
                    </td>
                </tr>
            </table>
        </div>
</asp:Panel>
<asp:Panel ID="panMnemonic" runat="server" Visible="false">
    <table width="100%" cellpadding="4" cellspacing="3" border="0">
        <tr>
            <td class="default">Mnemonic:</td>
            <td>
                <table cellpadding="0" cellspacing="0" border="0">
                    <tr>
                        <td><asp:TextBox ID="txtMnemonic" runat="server" Width="500" CssClass="default" /></td>
                    </tr>
                    <tr>
                        <td>
                            <div id="divMnemonic" runat="server" style="overflow:hidden; position:absolute; display:none; background-color:#FFFFFF; border:solid 1px #CCCCCC">
                                <asp:ListBox ID="lstMnemonic" runat="server" CssClass="default" />
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td class="default">Enabled:</td>
            <td><asp:CheckBox ID="chkMnemonicEnabled" runat="server" CssClass="default" /></td>
        <tr>
            <td nowrap></td>
            <td width="100%"><asp:Button ID="btnMnemonicAdd" runat="server" CssClass="default" Width="100" Text="Add" OnClick="btnMnemonicAdd_Click" Visible="false" /><asp:Button ID="btnMnemonicUpdate" runat="server" CssClass="default" Width="100" Text="Update" OnClick="btnMnemonicUpdate_Click" Visible="false" /> <asp:Button ID="btnMnemonicDelete" runat="server" CssClass="default" Width="100" Text="Delete" OnClick="btnMnemonicDelete_Click" Visible="false" /> <asp:Button ID="btnMnemonicCancel" runat="server" CssClass="default" Width="100" Text="Cancel" OnClick="btnMnemonicCancel_Click" /></td>
        </tr>
    </table>
</asp:Panel>
<asp:Panel ID="panDomain" runat="server" Visible="false">
    <table width="100%" cellpadding="4" cellspacing="3" border="0">
        <tr>
            <td nowrap>Name:</td>
            <td width="100%"><asp:TextBox ID="txtDomainName" runat="server" CssClass="default" Width="400" MaxLength="100" /></td>
        </tr>
        <tr>
            <td nowrap>Class(es):</td>
            <td width="100%">
                <asp:CheckBox ID="chkDomainEngineering" runat="server" CssClass="default" Text="Engineering" />&nbsp;&nbsp;&nbsp;
                <asp:CheckBox ID="chkDomainTest" runat="server" CssClass="default" Text="Test" />&nbsp;&nbsp;&nbsp;
                <asp:CheckBox ID="chkDomainQA" runat="server" CssClass="default" Text="QA" />&nbsp;&nbsp;&nbsp;
                <asp:CheckBox ID="chkDomainProduction" runat="server" CssClass="default" Text="Producton" />
            </td>
        </tr>
        <tr>
            <td nowrap>Operating System(s):</td>
            <td width="100%">
                <asp:CheckBox ID="chkDomainWindows" runat="server" CssClass="default" Text="Windows" />&nbsp;&nbsp;&nbsp;
                <asp:CheckBox ID="chkDomainUnix" runat="server" CssClass="default" Text="Unix" />
            </td>
        </tr>
        <tr>
            <td nowrap>Resiliency:</td>
            <td width="100%"><asp:DropDownList ID="ddlDomainResiliency" runat="server" CssClass="default" Width="400" /></td>
        </tr>
        <tr>
            <td nowrap>Enabled:</td>
            <td width="100%"><asp:CheckBox ID="chkDomainEnabled" runat="server" CssClass="default" /></td>
        </tr>
        <tr>
            <td nowrap></td>
            <td width="100%"><asp:Button ID="btnDomainAdd" runat="server" CssClass="default" Width="100" Text="Add" OnClick="btnDomainAdd_Click" Visible="false" /><asp:Button ID="btnDomainUpdate" runat="server" CssClass="default" Width="100" Text="Update" OnClick="btnDomainUpdate_Click" Visible="false" /> <asp:Button ID="btnDomainDelete" runat="server" CssClass="default" Width="100" Text="Delete" OnClick="btnDomainDelete_Click" Visible="false" /> <asp:Button ID="btnDomainCancel" runat="server" CssClass="default" Width="100" Text="Cancel" OnClick="btnDomainCancel_Click" /></td>
        </tr>
    </table>
    <br /><br />
    <span class="header">Schedules</span>
    <br /><br />
    <table width="100%" cellpadding="4" cellspacing="0" border="0" align="center" style="border:solid 1px #CCCCCC">
        <tr bgcolor="#EEEEEE">
            <td width="45%" nowrap><asp:LinkButton ID="btnScheduleName" runat="server" CssClass="tableheader" Text="<b>Name</b>" OnClick="btnOrder_Click" CommandArgument="name" /></td>
            <td width="15%" nowrap><asp:LinkButton ID="btnScheduleRegistered" runat="server" CssClass="tableheader" Text="<b>Registered</b>" OnClick="btnOrder_Click" CommandArgument="count" /></td>
            <td width="40%" nowrap><asp:LinkButton ID="btnScheduleModified" runat="server" CssClass="tableheader" Text="<b>Modified</b>" OnClick="btnOrder_Click" CommandArgument="modified" /></td>
            <td nowrap align="center"><asp:LinkButton ID="btnScheduleEnabled" runat="server" CssClass="tableheader" Text="<b>Enabled</b>" OnClick="btnOrder_Click" CommandArgument="enabled" /></td>
        </tr>
    <asp:repeater ID="rptSchedules" runat="server">
        <ItemTemplate>
            <tr class='<%# DataBinder.Eval(Container.DataItem, "enabled").ToString() == "1" ? "default" : "component_unavailable" %>' onmouseover="CellRowOver(this);" onmouseout="CellRowOut(this);" onclick="window.navigate('<%# FormURL("server=" + intServer.ToString() + "&domain=" + intDomain.ToString() + "&schedule=" + DataBinder.Eval(Container.DataItem, "id")) %>');">
                <td width="45%" nowrap><%# DataBinder.Eval(Container.DataItem, "name") %></td>
                <td width="15%" nowrap><%# DataBinder.Eval(Container.DataItem, "count") %></td>
                <td width="40%" nowrap><%# DataBinder.Eval(Container.DataItem, "modified") %></td>
                <td nowrap align="center"><img src="/images/<%# DataBinder.Eval(Container.DataItem, "enabled").ToString() == "1" ? "check" : "cancel" %>.gif" border="0" align="absmiddle" /></td>
            </tr>
        </ItemTemplate>
        <AlternatingItemTemplate>
            <tr class='<%# DataBinder.Eval(Container.DataItem, "enabled").ToString() == "1" ? "default" : "component_unavailable" %>' bgcolor="#F6F6F6" onmouseover="CellRowOver(this);" onmouseout="CellRowOut(this);" onclick="window.navigate('<%# FormURL("server=" + intServer.ToString() + "&domain=" + intDomain.ToString() + "&schedule=" + DataBinder.Eval(Container.DataItem, "id")) %>');">
                <td width="45%" nowrap><%# DataBinder.Eval(Container.DataItem, "name") %></td>
                <td width="15%" nowrap><%# DataBinder.Eval(Container.DataItem, "count") %></td>
                <td width="40%" nowrap><%# DataBinder.Eval(Container.DataItem, "modified") %></td>
                <td nowrap align="center"><img src="/images/<%# DataBinder.Eval(Container.DataItem, "enabled").ToString() == "1" ? "check" : "cancel" %>.gif" border="0" align="absmiddle" /></td>
            </tr>
        </AlternatingItemTemplate>
    </asp:repeater>
        <tr>
            <td colspan="6"><asp:Label ID="lblSchedules" runat="server" CssClass="error" Visible="false" Text="<img src='/images/bigAlert.gif' border='0' align='absmiddle'> There are no schedules for this domain" /></td>
        </tr>
    </table>
    <br /><br />
    <asp:Button ID="btnAddSchedule" runat="server" CssClass="default" Width="75" Text="Add New" OnClick="btnAddSchedule_Click" />
</asp:Panel>
<asp:Panel ID="panSchedule" runat="server" Visible="false">
    <table width="100%" cellpadding="4" cellspacing="3" border="0">
        <tr>
            <td nowrap>Name:</td>
            <td width="100%"><asp:TextBox ID="txtScheduleName" runat="server" CssClass="default" Width="400" MaxLength="100" /></td>
        </tr>
        <tr id="trClasses" runat="server" visible="false">
            <td nowrap>Class(es):</td>
            <td width="100%">
                <asp:CheckBox ID="chkScheduleEngineering" runat="server" CssClass="default" Text="Engineering" />&nbsp;&nbsp;&nbsp;
                <asp:CheckBox ID="chkScheduleTest" runat="server" CssClass="default" Text="Test" />&nbsp;&nbsp;&nbsp;
                <asp:CheckBox ID="chkScheduleQA" runat="server" CssClass="default" Text="QA" />&nbsp;&nbsp;&nbsp;
                <asp:CheckBox ID="chkScheduleProduction" runat="server" CssClass="default" Text="Producton" />
            </td>
        </tr>
        <tr id="trOS" runat="server" visible="false">
            <td nowrap>Operating System(s):</td>
            <td width="100%">
                <asp:CheckBox ID="chkScheduleWindows" runat="server" CssClass="default" Text="Windows" />&nbsp;&nbsp;&nbsp;
                <asp:CheckBox ID="chkScheduleUnix" runat="server" CssClass="default" Text="Unix" />
            </td>
        </tr>
        <tr>
            <td nowrap>Frequency:</td>
            <td width="100%">
                <asp:CheckBox ID="chkScheduleDaily" runat="server" CssClass="default" Text="Daily" />&nbsp;&nbsp;&nbsp;
                <asp:CheckBox ID="chkScheduleWeekly" runat="server" CssClass="default" Text="Weekly" />&nbsp;&nbsp;&nbsp;
                <asp:CheckBox ID="chkScheduleMonthly" runat="server" CssClass="default" Text="Monthly" />&nbsp;&nbsp;&nbsp;
            </td>
        </tr>
        <tr id="trResiliency" runat="server" visible="false">
            <td nowrap>Resiliency:</td>
            <td width="100%"><asp:DropDownList ID="ddlScheduleResiliency" runat="server" CssClass="default" Width="400" /></td>
        </tr>
        <tr id="divScheduleWeekly" runat="server" style="display:none">
            <td nowrap>Day(s):</td>
            <td width="100%">
                <asp:CheckBox ID="chkScheduleWeeklySunday" runat="server" CssClass="default" Text="Sunday" />&nbsp;&nbsp;&nbsp;
                <asp:CheckBox ID="chkScheduleWeeklyMonday" runat="server" CssClass="default" Text="Monday" />&nbsp;&nbsp;&nbsp;
                <asp:CheckBox ID="chkScheduleWeeklyTuesday" runat="server" CssClass="default" Text="Tuesday" />&nbsp;&nbsp;&nbsp;
                <asp:CheckBox ID="chkScheduleWeeklyWednesday" runat="server" CssClass="default" Text="Wednesday" />&nbsp;&nbsp;&nbsp;
                <asp:CheckBox ID="chkScheduleWeeklyThursday" runat="server" CssClass="default" Text="Thursday" />&nbsp;&nbsp;&nbsp;
                <asp:CheckBox ID="chkScheduleWeeklyFriday" runat="server" CssClass="default" Text="Friday" />&nbsp;&nbsp;&nbsp;
                <asp:CheckBox ID="chkScheduleWeeklySaturday" runat="server" CssClass="default" Text="Saturday" />&nbsp;&nbsp;&nbsp;
            </td>
        </tr>
        <tr>
            <td nowrap valign="top"><br />Time(s):</td>
            <td width="100%">
                <table cellpadding="4" cellspacing="3" border="0">
                    <tr>
                        <td><asp:CheckBox ID="chk1200AM" runat="server" CssClass="default" Text="12:00 AM" /></td>
                        <td><asp:CheckBox ID="chk1230AM" runat="server" CssClass="default" Text="12:30 AM" /></td>
                        <td rowspan="50"><img src="/images/spacer.gif" border="0" width="50" height="1" /></td>
                        <td><asp:CheckBox ID="chk1200PM" runat="server" CssClass="default" Text="12:00 PM" /></td>
                        <td><asp:CheckBox ID="chk1230PM" runat="server" CssClass="default" Text="12:30 PM" /></td>
                        <!--
                        <td rowspan="50">
                            <table>
                                <tr>
                                    <td rowspan="10"><img src="/images/spacer.gif" border="0" width="50" height="1" /></td>
                                    <td><b>Common Configurations</b></td>
                                </tr>
                                <tr>
                                    <td></td>
                                </tr>
                            </table>
                        </td>
                        -->
                    </tr>
                    <tr>
                        <td><asp:CheckBox ID="chk100AM" runat="server" CssClass="default" Text="1:00 AM" /></td>
                        <td><asp:CheckBox ID="chk130AM" runat="server" CssClass="default" Text="1:30 AM" /></td>
                        <td><asp:CheckBox ID="chk100PM" runat="server" CssClass="default" Text="1:00 PM" /></td>
                        <td><asp:CheckBox ID="chk130PM" runat="server" CssClass="default" Text="1:30 PM" /></td>
                    </tr>
                    <tr>
                        <td><asp:CheckBox ID="chk200AM" runat="server" CssClass="default" Text="2:00 AM" /></td>
                        <td><asp:CheckBox ID="chk230AM" runat="server" CssClass="default" Text="2:30 AM" /></td>
                        <td><asp:CheckBox ID="chk200PM" runat="server" CssClass="default" Text="2:00 PM" /></td>
                        <td><asp:CheckBox ID="chk230PM" runat="server" CssClass="default" Text="2:30 PM" /></td>
                    </tr>
                    <tr>
                        <td><asp:CheckBox ID="chk300AM" runat="server" CssClass="default" Text="3:00 AM" /></td>
                        <td><asp:CheckBox ID="chk330AM" runat="server" CssClass="default" Text="3:30 AM" /></td>
                        <td><asp:CheckBox ID="chk300PM" runat="server" CssClass="default" Text="3:00 PM" /></td>
                        <td><asp:CheckBox ID="chk330PM" runat="server" CssClass="default" Text="3:30 PM" /></td>
                    </tr>
                    <tr>
                        <td><asp:CheckBox ID="chk400AM" runat="server" CssClass="default" Text="4:00 AM" /></td>
                        <td><asp:CheckBox ID="chk430AM" runat="server" CssClass="default" Text="4:30 AM" /></td>
                        <td><asp:CheckBox ID="chk400PM" runat="server" CssClass="default" Text="4:00 PM" /></td>
                        <td><asp:CheckBox ID="chk430PM" runat="server" CssClass="default" Text="4:30 PM" /></td>
                    </tr>
                    <tr>
                        <td><asp:CheckBox ID="chk500AM" runat="server" CssClass="default" Text="5:00 AM" /></td>
                        <td><asp:CheckBox ID="chk530AM" runat="server" CssClass="default" Text="5:30 AM" /></td>
                        <td><asp:CheckBox ID="chk500PM" runat="server" CssClass="default" Text="5:00 PM" /></td>
                        <td><asp:CheckBox ID="chk530PM" runat="server" CssClass="default" Text="5:30 PM" /></td>
                    </tr>
                    <tr>
                        <td><asp:CheckBox ID="chk600AM" runat="server" CssClass="default" Text="6:00 AM" /></td>
                        <td><asp:CheckBox ID="chk630AM" runat="server" CssClass="default" Text="6:30 AM" /></td>
                        <td><asp:CheckBox ID="chk600PM" runat="server" CssClass="default" Text="6:00 PM" /></td>
                        <td><asp:CheckBox ID="chk630PM" runat="server" CssClass="default" Text="6:30 PM" /></td>
                    </tr>
                    <tr>
                        <td><asp:CheckBox ID="chk700AM" runat="server" CssClass="default" Text="7:00 AM" /></td>
                        <td><asp:CheckBox ID="chk730AM" runat="server" CssClass="default" Text="7:30 AM" /></td>
                        <td><asp:CheckBox ID="chk700PM" runat="server" CssClass="default" Text="7:00 PM" /></td>
                        <td><asp:CheckBox ID="chk730PM" runat="server" CssClass="default" Text="7:30 PM" /></td>
                    </tr>
                    <tr>
                        <td><asp:CheckBox ID="chk800AM" runat="server" CssClass="default" Text="8:00 AM" /></td>
                        <td><asp:CheckBox ID="chk830AM" runat="server" CssClass="default" Text="8:30 AM" /></td>
                        <td><asp:CheckBox ID="chk800PM" runat="server" CssClass="default" Text="8:00 PM" /></td>
                        <td><asp:CheckBox ID="chk830PM" runat="server" CssClass="default" Text="8:30 PM" /></td>
                    </tr>
                    <tr>
                        <td><asp:CheckBox ID="chk900AM" runat="server" CssClass="default" Text="9:00 AM" /></td>
                        <td><asp:CheckBox ID="chk930AM" runat="server" CssClass="default" Text="9:30 AM" /></td>
                        <td><asp:CheckBox ID="chk900PM" runat="server" CssClass="default" Text="9:00 PM" /></td>
                        <td><asp:CheckBox ID="chk930PM" runat="server" CssClass="default" Text="9:30 PM" /></td>
                    </tr>
                    <tr>
                        <td><asp:CheckBox ID="chk1000AM" runat="server" CssClass="default" Text="10:00 AM" /></td>
                        <td><asp:CheckBox ID="chk1030AM" runat="server" CssClass="default" Text="10:30 AM" /></td>
                        <td><asp:CheckBox ID="chk1000PM" runat="server" CssClass="default" Text="10:00 PM" /></td>
                        <td><asp:CheckBox ID="chk1030PM" runat="server" CssClass="default" Text="10:30 PM" /></td>
                    </tr>
                    <tr>
                        <td><asp:CheckBox ID="chk1100AM" runat="server" CssClass="default" Text="11:00 AM" /></td>
                        <td><asp:CheckBox ID="chk1130AM" runat="server" CssClass="default" Text="11:30 AM" /></td>
                        <td><asp:CheckBox ID="chk1100PM" runat="server" CssClass="default" Text="11:00 PM" /></td>
                        <td><asp:CheckBox ID="chk1130PM" runat="server" CssClass="default" Text="11:30 PM" /></td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td nowrap>Enabled:</td>
            <td width="100%"><asp:CheckBox ID="chkScheduleEnabled" runat="server" CssClass="default" /></td>
        </tr>
        <tr>
            <td nowrap></td>
            <td width="100%"><asp:Button ID="btnScheduleAdd" runat="server" CssClass="default" Width="100" Text="Add" OnClick="btnScheduleAdd_Click" Visible="false" /><asp:Button ID="btnScheduleUpdate" runat="server" CssClass="default" Width="100" Text="Update" OnClick="btnScheduleUpdate_Click" Visible="false" /> <asp:Button ID="btnScheduleDelete" runat="server" CssClass="default" Width="100" Text="Delete" OnClick="btnScheduleDelete_Click" Visible="false" /> <asp:Button ID="btnScheduleCancel" runat="server" CssClass="default" Width="100" Text="Cancel" OnClick="btnScheduleCancel_Click" /></td>
        </tr>
    </table>
</asp:Panel>
<asp:HiddenField ID="hdnTab" runat="server" />
<input type="hidden" id="hdnLocation" runat="server" />
<asp:HiddenField ID="hdnMnemonic" runat="server" />
