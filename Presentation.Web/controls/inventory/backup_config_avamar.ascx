<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="backup_config_avamar.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.backup_config_avamar" %>
<table width="100%" cellpadding="0" cellspacing="5" border="0">
    <tr>
        <td rowspan="2"><img src="/images/tape.gif" border="0" align="absmiddle" /></td>
        <td class="header" width="100%" valign="bottom">Avamar Administration</td>
    </tr>
    <tr>
        <td width="100%" valign="top">Use this page to administer Avamar grids, domains and groups.</td>
    </tr>
</table>
<table width="100%" cellpadding="0" cellspacing="5" border="0">
    <tr>
        <td><asp:Label ID="lblCrumbs" runat="server" /></td>
    </tr>
</table>
<asp:Label ID="lblMessage" runat="server" />
<asp:Panel ID="panGrids" runat="server" Visible="false">
    <table width="100%" cellpadding="4" cellspacing="0" border="0" align="center" style="border:solid 1px #CCCCCC">
        <tr bgcolor="#EEEEEE">
            <td width="55%" nowrap><asp:LinkButton ID="btnTSMName" runat="server" CssClass="tableheader" Text="<b>Name</b>" OnClick="btnOrder_Click" CommandArgument="name" /></td>
            <td width="15%" nowrap><asp:LinkButton ID="btnTSMRegistered" runat="server" CssClass="tableheader" Text="<b>Registered</b>" OnClick="btnOrder_Click" CommandArgument="registered" /></td>
            <td width="30%" nowrap><asp:LinkButton ID="btnTSMModified" runat="server" CssClass="tableheader" Text="<b>Modified</b>" OnClick="btnOrder_Click" CommandArgument="modified" /></td>
            <td nowrap align="center"><asp:LinkButton ID="btnTSMEnabled" runat="server" CssClass="tableheader" Text="<b>Enabled</b>" OnClick="btnOrder_Click" CommandArgument="enabled" /></td>
        </tr>
    <asp:repeater ID="rptGrids" runat="server">
        <ItemTemplate>
            <tr class='<%# DataBinder.Eval(Container.DataItem, "enabled").ToString() == "1" ? "default" : "component_unavailable" %>' onmouseover="CellRowOver(this);" onmouseout="CellRowOut(this);" onclick="window.navigate('<%# FormURL("grid=" + DataBinder.Eval(Container.DataItem, "id")) %>');">
                <td width="55%" nowrap><%# DataBinder.Eval(Container.DataItem, "name") %></td>
                <td width="15%"><%# DataBinder.Eval(Container.DataItem, "registered") %> of <%# DataBinder.Eval(Container.DataItem, "maximum") %></td>
                <td width="30%" nowrap><%# DataBinder.Eval(Container.DataItem, "modified") %></td>
                <td nowrap align="center"><img src="/images/<%# DataBinder.Eval(Container.DataItem, "enabled").ToString() == "1" ? "check" : "cancel" %>.gif" border="0" align="absmiddle" /></td>
            </tr>
        </ItemTemplate>
        <AlternatingItemTemplate>
            <tr class='<%# DataBinder.Eval(Container.DataItem, "enabled").ToString() == "1" ? "default" : "component_unavailable" %>' bgcolor="#F6F6F6" onmouseover="CellRowOver(this);" onmouseout="CellRowOut(this);" onclick="window.navigate('<%# FormURL("grid=" + DataBinder.Eval(Container.DataItem, "id")) %>');">
                <td width="55%" nowrap><%# DataBinder.Eval(Container.DataItem, "name") %></td>
                <td width="15%"><%# DataBinder.Eval(Container.DataItem, "registered") %> of <%# DataBinder.Eval(Container.DataItem, "maximum") %></td>
                <td width="30%" nowrap><%# DataBinder.Eval(Container.DataItem, "modified") %></td>
                <td nowrap align="center"><img src="/images/<%# DataBinder.Eval(Container.DataItem, "enabled").ToString() == "1" ? "check" : "cancel" %>.gif" border="0" align="absmiddle" /></td>
            </tr>
        </AlternatingItemTemplate>
    </asp:repeater>
        <tr>
            <td colspan="6"><asp:Label ID="lblGrids" runat="server" CssClass="error" Visible="false" Text="<img src='/images/bigAlert.gif' border='0' align='absmiddle'> There are no grids" /></td>
        </tr>
    </table>
    <br /><br />
    <asp:Button ID="btnAddGrid" runat="server" CssClass="default" Width="75" Text="Add New" OnClick="btnAddGrid_Click" />
</asp:Panel>
<asp:Panel ID="panGrid" runat="server" Visible="false">
    <table width="100%" cellpadding="4" cellspacing="3" border="0">
        <tr>
            <td nowrap>Grid Name:</td>
            <td width="100%"><asp:TextBox ID="txtGridName" runat="server" CssClass="default" Width="400" MaxLength="100" /></td>
        </tr>
        <tr>
            <td nowrap>Threshold:</td>
            <td width="100%">
                <asp:TextBox ID="txtGridThreshold" runat="server" CssClass="default" MaxLength="3" Width="50" />
                <span> % of utilization will begin sending notifications - set to &quot;0&quot; to disable thresholds</span>
            </td>
        </tr>
        <tr>
            <td class="footer">&nbsp;</td>
            <td class="footer">Once the threshold is breached, emails will be sent to the people configured on the &quot;Security&quot; tab.</td>
        </tr>
        <tr>
            <td nowrap>Maximum:</td>
            <td width="100%">
                <asp:TextBox ID="txtGridMaximum" runat="server" CssClass="default" MaxLength="3" Width="50" />
                <span> % of utilization allowed</span>
            </td>
        </tr>
        <tr>
            <td nowrap>Enabled:</td>
            <td width="100%"><asp:CheckBox ID="chkGridEnabled" runat="server" CssClass="default" /></td>
        </tr>
        <tr>
            <td nowrap></td>
            <td width="100%"><asp:Button ID="btnGridAdd" runat="server" CssClass="default" Width="100" Text="Add" OnClick="btnGridAdd_Click" Visible="false" /><asp:Button ID="btnGridUpdate" runat="server" CssClass="default" Width="100" Text="Update" OnClick="btnGridUpdate_Click" Visible="false" /> <asp:Button ID="btnGridDelete" runat="server" CssClass="default" Width="100" Text="Delete" OnClick="btnGridDelete_Click" Visible="false" /> <asp:Button ID="btnGridCancel" runat="server" CssClass="default" Width="100" Text="Cancel" OnClick="btnGridCancel_Click" /></td>
        </tr>
    </table>
    <br /><br />
    <%=strMenuTab1 %>
    <div id="divMenu1"> 
        <div style="display:none">
            <br />

            <table width="100%" cellpadding="4" cellspacing="0" border="0" align="center" style="border:solid 1px #CCCCCC">
                <tr bgcolor="#EEEEEE">
                    <td width="45%" nowrap><asp:LinkButton ID="btnDomainName" runat="server" CssClass="tableheader" Text="<b>Domain Name</b>" OnClick="btnOrder_Click" CommandArgument="name" /></td>
                    <td width="15%" nowrap><asp:LinkButton ID="btnDomainRegistered" runat="server" CssClass="tableheader" Text="<b>Registered</b>" OnClick="btnOrder_Click" CommandArgument="registered" /></td>
                    <td width="40%" nowrap><asp:LinkButton ID="btnDomainModified" runat="server" CssClass="tableheader" Text="<b>Modified</b>" OnClick="btnOrder_Click" CommandArgument="modified" /></td>
                    <td nowrap align="center"><asp:LinkButton ID="btnDomainEnabled" runat="server" CssClass="tableheader" Text="<b>Enabled</b>" OnClick="btnOrder_Click" CommandArgument="enabled" /></td>
                </tr>
            <asp:repeater ID="rptDomains" runat="server">
                <ItemTemplate>
                    <tr class='<%# DataBinder.Eval(Container.DataItem, "enabled").ToString() == "1" ? "default" : "component_unavailable" %>' onmouseover="CellRowOver(this);" onmouseout="CellRowOut(this);" onclick="window.navigate('<%# FormURL("grid=" + intGrid.ToString() + "&domain=" + DataBinder.Eval(Container.DataItem, "id")) %>');">
                        <td width="45%" nowrap><%# DataBinder.Eval(Container.DataItem, "FQDN") %></td>
                        <td width="15%"><%# DataBinder.Eval(Container.DataItem, "registered") %></td>
                        <td width="40%" nowrap><%# DataBinder.Eval(Container.DataItem, "modified") %></td>
                        <td nowrap align="center"><img src="/images/<%# DataBinder.Eval(Container.DataItem, "enabled").ToString() == "1" ? "check" : "cancel" %>.gif" border="0" align="absmiddle" /></td>
                    </tr>
                </ItemTemplate>
                <AlternatingItemTemplate>
                    <tr class='<%# DataBinder.Eval(Container.DataItem, "enabled").ToString() == "1" ? "default" : "component_unavailable" %>' bgcolor="#F6F6F6" onmouseover="CellRowOver(this);" onmouseout="CellRowOut(this);" onclick="window.navigate('<%# FormURL("grid=" + intGrid.ToString() + "&domain=" + DataBinder.Eval(Container.DataItem, "id")) %>');">
                        <td width="45%" nowrap><%# DataBinder.Eval(Container.DataItem, "FQDN") %></td>
                        <td width="15%"><%# DataBinder.Eval(Container.DataItem, "registered") %></td>
                        <td width="40%" nowrap><%# DataBinder.Eval(Container.DataItem, "modified") %></td>
                        <td nowrap align="center"><img src="/images/<%# DataBinder.Eval(Container.DataItem, "enabled").ToString() == "1" ? "check" : "cancel" %>.gif" border="0" align="absmiddle" /></td>
                    </tr>
                </AlternatingItemTemplate>
            </asp:repeater>
                <tr>
                    <td colspan="6"><asp:Label ID="lblDomains" runat="server" CssClass="error" Visible="false" Text="<img src='/images/bigAlert.gif' border='0' align='absmiddle'> There are no domains for this grid" /></td>
                </tr>
            </table>
            <br /><br />
            <asp:Button ID="btnAddDomain" runat="server" CssClass="default" Width="100" Text="Add Domain" OnClick="btnAddDomain_Click" />
        </div>
        <div style="display:none">
            <br />
            <table class="repeater">
                <tr>
                    <th width="30%" nowrap><asp:LinkButton ID="btnGroupName" runat="server" CssClass="tableheader" Text="<b>Group Name</b>" OnClick="btnOrder_Click" CommandArgument="name" /></th>
                    <th width="40%" nowrap><asp:LinkButton ID="btnGroupDomain" runat="server" CssClass="tableheader" Text="<b>Domain</b>" OnClick="btnOrder_Click" CommandArgument="FQDN" /></th>
                    <th width="10%" nowrap><asp:LinkButton ID="btnGroupRegistered" runat="server" CssClass="tableheader" Text="<b>Registered</b>" OnClick="btnOrder_Click" CommandArgument="registered" /></th>
                    <th width="20%" nowrap><asp:LinkButton ID="btnGroupModified" runat="server" CssClass="tableheader" Text="<b>Modified</b>" OnClick="btnOrder_Click" CommandArgument="modified" /></th>
                    <th nowrap align="center"><asp:LinkButton ID="btnGroupEnabled" runat="server" CssClass="tableheader" Text="<b>Enabled</b>" OnClick="btnOrder_Click" CommandArgument="enabled" /></th>
                </tr>
            <asp:repeater ID="rptGroups" runat="server">
                <ItemTemplate>
                    <tr class='<%# DataBinder.Eval(Container.DataItem, "enabled").ToString() == "1" ? "default" : "component_unavailable" %>' onmouseover="CellRowOver(this);" onmouseout="CellRowOut(this);" onclick="window.navigate('<%# FormURL("grid=" + intGrid.ToString() + "&group=" + DataBinder.Eval(Container.DataItem, "id")) %>');">
                        <td width="30%" nowrap><%# DataBinder.Eval(Container.DataItem, "name") %></td>
                        <td width="40%" nowrap><%# DataBinder.Eval(Container.DataItem, "FQDN") %></td>
                        <td width="10%"><%# DataBinder.Eval(Container.DataItem, "registered") %> of <%# DataBinder.Eval(Container.DataItem, "maximum") %></td>
                        <td width="20%" nowrap><%# DataBinder.Eval(Container.DataItem, "modified") %></td>
                        <td nowrap align="center"><img src="/images/<%# DataBinder.Eval(Container.DataItem, "enabled").ToString() == "1" ? "check" : "cancel" %>.gif" border="0" align="absmiddle" /></td>
                    </tr>
                </ItemTemplate>
                <AlternatingItemTemplate>
                    <tr class='<%# DataBinder.Eval(Container.DataItem, "enabled").ToString() == "1" ? "default" : "component_unavailable" %>' bgcolor="#F6F6F6" onmouseover="CellRowOver(this);" onmouseout="CellRowOut(this);" onclick="window.navigate('<%# FormURL("grid=" + intGrid.ToString() + "&group=" + DataBinder.Eval(Container.DataItem, "id")) %>');">
                        <td width="30%" nowrap><%# DataBinder.Eval(Container.DataItem, "name") %></td>
                        <td width="40%" nowrap><%# DataBinder.Eval(Container.DataItem, "FQDN") %></td>
                        <td width="10%"><%# DataBinder.Eval(Container.DataItem, "registered") %> of <%# DataBinder.Eval(Container.DataItem, "maximum") %></td>
                        <td width="20%" nowrap><%# DataBinder.Eval(Container.DataItem, "modified") %></td>
                        <td nowrap align="center"><img src="/images/<%# DataBinder.Eval(Container.DataItem, "enabled").ToString() == "1" ? "check" : "cancel" %>.gif" border="0" align="absmiddle" /></td>
                    </tr>
                </AlternatingItemTemplate>
            </asp:repeater>
                <tr>
                    <td colspan="6"><asp:Label ID="lblGroups" runat="server" CssClass="error" Visible="false" Text="<img src='/images/bigAlert.gif' border='0' align='absmiddle'> There are no groups for this grid" /></td>
                </tr>
            </table>
            <br />
            <img src="/images/alert.gif" border="0" align="absmiddle" /> <b>NOTE:</b> You do not have to add the &quot;/DECOM&quot; group. ClearView knows about it automatically and will use it for decommissions.
            <br /><br />
            <asp:Button ID="btnAddGroup" runat="server" CssClass="default" Width="100" Text="Add Group" OnClick="btnAddGroup_Click" />
        </div>
        <div style="display:none">
            <br />
            <table width="100%" cellpadding="4" cellspacing="0" border="0" align="center" style="border:solid 1px #CCCCCC">
                <tr bgcolor="#EEEEEE">
                    <td width="100%" nowrap><asp:LinkButton ID="btnLocationName" runat="server" CssClass="tableheader" Text="<b>Location</b>" OnClick="btnOrder_Click" CommandArgument="addressid" /></td>
                    <td nowrap align="center"><asp:LinkButton ID="btnLocationEnabled" runat="server" CssClass="tableheader" Text="<b>Enabled</b>" OnClick="btnOrder_Click" CommandArgument="enabled" /></td>
                </tr>
            <asp:repeater ID="rptLocations" runat="server">
                <ItemTemplate>
                    <tr class='<%# DataBinder.Eval(Container.DataItem, "enabled").ToString() == "1" ? "default" : "component_unavailable" %>' onmouseover="CellRowOver(this);" onmouseout="CellRowOut(this);" onclick="window.navigate('<%# FormURL("grid=" + intGrid.ToString() + "&location=" + DataBinder.Eval(Container.DataItem, "id")) %>');">
                        <td width="100%" nowrap><%# DataBinder.Eval(Container.DataItem, "name") %></td>
                        <td nowrap align="center"><img src="/images/<%# DataBinder.Eval(Container.DataItem, "enabled").ToString() == "1" ? "check" : "cancel" %>.gif" border="0" align="absmiddle" /></td>
                    </tr>
                </ItemTemplate>
                <AlternatingItemTemplate>
                    <tr class='<%# DataBinder.Eval(Container.DataItem, "enabled").ToString() == "1" ? "default" : "component_unavailable" %>' bgcolor="#F6F6F6" onmouseover="CellRowOver(this);" onmouseout="CellRowOut(this);" onclick="window.navigate('<%# FormURL("grid=" + intGrid.ToString() + "&location=" + DataBinder.Eval(Container.DataItem, "id")) %>');">
                        <td width="100%" nowrap><%# DataBinder.Eval(Container.DataItem, "name") %></td>
                        <td nowrap align="center"><img src="/images/<%# DataBinder.Eval(Container.DataItem, "enabled").ToString() == "1" ? "check" : "cancel" %>.gif" border="0" align="absmiddle" /></td>
                    </tr>
                </AlternatingItemTemplate>
            </asp:repeater>
                <tr>
                    <td colspan="6"><asp:Label ID="lblLocations" runat="server" CssClass="error" Visible="false" Text="<img src='/images/bigAlert.gif' border='0' align='absmiddle'> There are no locations for this grid" /></td>
                </tr>
            </table>
            <br /><br />
            <asp:Button ID="btnAddLocation" runat="server" CssClass="default" Width="100" Text="Add Location" OnClick="btnAddLocation_Click" />
        </div>
        <div style="display:none">
            <br />
            <table width="100%" cellpadding="4" cellspacing="0" border="0" align="center" style="border:solid 1px #CCCCCC">
                <tr bgcolor="#EEEEEE">
                    <td width="50%" nowrap><asp:LinkButton ID="btnEnvironmentClass" runat="server" CssClass="tableheader" Text="<b>Class</b>" OnClick="btnOrder_Click" CommandArgument="classid" /></td>
                    <td width="50%" nowrap><asp:LinkButton ID="btnEnvironmentEnv" runat="server" CssClass="tableheader" Text="<b>Environment</b>" OnClick="btnOrder_Click" CommandArgument="environmentid" /></td>
                    <td nowrap align="center"><asp:LinkButton ID="btnEnvironmentEnabled" runat="server" CssClass="tableheader" Text="<b>Enabled</b>" OnClick="btnOrder_Click" CommandArgument="enabled" /></td>
                </tr>
            <asp:repeater ID="rptEnvironments" runat="server">
                <ItemTemplate>
                    <tr class='<%# DataBinder.Eval(Container.DataItem, "enabled").ToString() == "1" ? "default" : "component_unavailable" %>' onmouseover="CellRowOver(this);" onmouseout="CellRowOut(this);" onclick="window.navigate('<%# FormURL("grid=" + intGrid.ToString() + "&environment=" + DataBinder.Eval(Container.DataItem, "id")) %>');">
                        <td width="50%" nowrap><%# DataBinder.Eval(Container.DataItem, "class") %></td>
                        <td width="50%" nowrap><%# DataBinder.Eval(Container.DataItem, "environment") %></td>
                        <td nowrap align="center"><img src="/images/<%# DataBinder.Eval(Container.DataItem, "enabled").ToString() == "1" ? "check" : "cancel" %>.gif" border="0" align="absmiddle" /></td>
                    </tr>
                </ItemTemplate>
                <AlternatingItemTemplate>
                    <tr class='<%# DataBinder.Eval(Container.DataItem, "enabled").ToString() == "1" ? "default" : "component_unavailable" %>' bgcolor="#F6F6F6" onmouseover="CellRowOver(this);" onmouseout="CellRowOut(this);" onclick="window.navigate('<%# FormURL("grid=" + intGrid.ToString() + "&environment=" + DataBinder.Eval(Container.DataItem, "id")) %>');">
                        <td width="50%" nowrap><%# DataBinder.Eval(Container.DataItem, "class") %></td>
                        <td width="50%" nowrap><%# DataBinder.Eval(Container.DataItem, "environment") %></td>
                        <td nowrap align="center"><img src="/images/<%# DataBinder.Eval(Container.DataItem, "enabled").ToString() == "1" ? "check" : "cancel" %>.gif" border="0" align="absmiddle" /></td>
                    </tr>
                </AlternatingItemTemplate>
            </asp:repeater>
                <tr>
                    <td colspan="6"><asp:Label ID="lblEnvironments" runat="server" CssClass="error" Visible="false" Text="<img src='/images/bigAlert.gif' border='0' align='absmiddle'> There are no environments for this grid" /></td>
                </tr>
            </table>
            <br /><br />
            <asp:Button ID="btnAddEnvironment" runat="server" CssClass="default" Width="100" Text="Add Environment" OnClick="btnAddEnvironment_Click" />
        </div>
</asp:Panel>
<asp:Panel ID="panLocation" runat="server" Visible="false">
    <table width="100%" cellpadding="4" cellspacing="3" border="0">
        <tr>
            <td nowrap>Location:</td>
            <td width="100%"><%=strLocation %></td>
        </tr>
        <tr>
            <td nowrap>Enabled:</td>
            <td width="100%"><asp:CheckBox ID="chkLocationEnabled" runat="server" CssClass="default" /></td>
        </tr>
        <tr>
            <td nowrap></td>
            <td width="100%">
                <asp:Button ID="btnLocationAdd" runat="server" CssClass="default" Width="100" Text="Add" OnClick="btnLocationAdd_Click" Visible="false" />
                <asp:Button ID="btnLocationUpdate" runat="server" CssClass="default" Width="100" Text="Update" OnClick="btnLocationUpdate_Click" Visible="false" /> 
                <asp:Button ID="btnLocationDelete" runat="server" CssClass="default" Width="100" Text="Delete" OnClick="btnLocationDelete_Click" Visible="false" /> 
                <asp:Button ID="btnLocationCancel" runat="server" CssClass="default" Width="100" Text="Cancel" OnClick="btnLocationCancel_Click" />
            </td>
        </tr>
    </table>
</asp:Panel>
<asp:Panel ID="panEnvironment" runat="server" Visible="false">
    <table width="100%" cellpadding="4" cellspacing="3" border="0">
        <tr>
            <td nowrap>Class:</td>
            <td width="100%"><asp:DropDownList ID="ddlClass" runat="server" Width="300" /></td>
        </tr>
        <tr>
            <td nowrap>Environment:</td>
            <td width="100%">
                <asp:DropDownList ID="ddlEnvironment" CssClass="default" runat="server" Width="300" Enabled="false" >
                    <asp:ListItem Value="-- Please select a Class --" />
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td nowrap>Enabled:</td>
            <td width="100%"><asp:CheckBox ID="chkEnvironmentEnabled" runat="server" CssClass="default" /></td>
        </tr>
        <tr>
            <td nowrap></td>
            <td width="100%">
                <asp:Button ID="btnEnvironmentAdd" runat="server" CssClass="default" Width="100" Text="Add" OnClick="btnEnvironmentAdd_Click" Visible="false" />
                <asp:Button ID="btnEnvironmentUpdate" runat="server" CssClass="default" Width="100" Text="Update" OnClick="btnEnvironmentUpdate_Click" Visible="false" /> 
                <asp:Button ID="btnEnvironmentDelete" runat="server" CssClass="default" Width="100" Text="Delete" OnClick="btnEnvironmentDelete_Click" Visible="false" /> 
                <asp:Button ID="btnEnvironmentCancel" runat="server" CssClass="default" Width="100" Text="Cancel" OnClick="btnEnvironmentCancel_Click" />
            </td>
        </tr>
    </table>
</asp:Panel>
<asp:Panel ID="panDomain" runat="server" Visible="false">
    <table width="100%" cellpadding="4" cellspacing="3" border="0">
        <tr>
            <td nowrap>Domain Name:</td>
            <td width="100%"><asp:TextBox ID="txtDomainName" runat="server" CssClass="default" Width="400" MaxLength="100" /></td>
        </tr>
        <tr>
            <td nowrap>Parent Domain:</td>
            <td width="100%"><asp:DropDownList ID="ddlDomainParent" runat="server" CssClass="default" Width="400" /></td>
        </tr>
        <tr>
            <td nowrap>Resiliency:</td>
            <td width="100%"><asp:CheckBoxList ID="chkDomainResiliency" runat="server" CssClass="default" RepeatDirection="Horizontal" RepeatColumns="5" /></td>
        </tr>
        <tr>
            <td nowrap>Application:</td>
            <td width="100%"><asp:CheckBoxList ID="chkDomainApplication" runat="server" CssClass="default" RepeatDirection="Horizontal" RepeatColumns="5" /></td>
        </tr>
        <tr>
            <td nowrap>Catch-All:</td>
            <td width="100%"><asp:CheckBox ID="chkDomainCatchAll" runat="server" CssClass="default" /></td>
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
</asp:Panel>
<asp:Panel ID="panGroup" runat="server" Visible="false">
    <h3 class="greentableheader">You are modifying a Group.</h3>
    <table width="100%" cellpadding="4" cellspacing="3" border="0">
        <tr>
            <td nowrap>Group Name:</td>
            <td width="100%"><asp:TextBox ID="txtGroupName" runat="server" CssClass="default" Width="400" MaxLength="100" /></td>
        </tr>
        <tr>
            <td nowrap>Domain:</td>
            <td width="100%">
                <asp:DropDownList ID="ddlGroupDomain" runat="server" CssClass="default" Width="150" />
                &nbsp;&nbsp;&nbsp;&nbsp;
                <img src="/images/alert.gif" border="0" /> <b>NOTE:</b> <a href="javascript:void(0);" onclick="alert('If this functionality should be changed, please submit an enhancement');">At this time, groups are only assigned to the root.</a>
            </td>
        </tr>
        <tr>
            <td nowrap>Operating System(s):</td>
            <td width="100%">
                <asp:CheckBoxList ID="chkGroupOS" runat="server" CssClass="default" RepeatDirection="Vertical" RepeatColumns="5" />
            </td>
        </tr>
        <tr>
            <td nowrap>Frequency:</td>
            <td width="100%">
                <asp:RadioButton ID="chkGroupDaily" runat="server" CssClass="default" Text="Daily" GroupName="Frequency" />&nbsp;&nbsp;&nbsp;
                <asp:RadioButton ID="chkGroupWeekly" runat="server" CssClass="default" Text="Weekly" GroupName="Frequency" />&nbsp;&nbsp;&nbsp;
                <asp:RadioButton ID="chkGroupMonthly" runat="server" CssClass="default" Text="Monthly" GroupName="Frequency" />&nbsp;&nbsp;&nbsp;
            </td>
        </tr>
        <tr id="divGroupWeekly" runat="server" style="display:none">
            <td nowrap>Day(s) of Week:</td>
            <td width="100%">
                <asp:CheckBox ID="chkGroupWeeklySunday" runat="server" CssClass="default" Text="Sunday" />&nbsp;&nbsp;&nbsp;
                <asp:CheckBox ID="chkGroupWeeklyMonday" runat="server" CssClass="default" Text="Monday" />&nbsp;&nbsp;&nbsp;
                <asp:CheckBox ID="chkGroupWeeklyTuesday" runat="server" CssClass="default" Text="Tuesday" />&nbsp;&nbsp;&nbsp;
                <asp:CheckBox ID="chkGroupWeeklyWednesday" runat="server" CssClass="default" Text="Wednesday" />&nbsp;&nbsp;&nbsp;
                <asp:CheckBox ID="chkGroupWeeklyThursday" runat="server" CssClass="default" Text="Thursday" />&nbsp;&nbsp;&nbsp;
                <asp:CheckBox ID="chkGroupWeeklyFriday" runat="server" CssClass="default" Text="Friday" />&nbsp;&nbsp;&nbsp;
                <asp:CheckBox ID="chkGroupWeeklySaturday" runat="server" CssClass="default" Text="Saturday" />&nbsp;&nbsp;&nbsp;
            </td>
        </tr>
        <tr id="divGroupMonthly" runat="server" style="display:none">
            <td nowrap>Day of Month:</td>
            <td width="100%">
                <asp:DropDownList ID="ddlGroupMonthly" runat="server" Width="100">
                    <asp:ListItem Value="1" />
                    <asp:ListItem Value="2" />
                    <asp:ListItem Value="3" />
                    <asp:ListItem Value="4" />
                    <asp:ListItem Value="5" />
                    <asp:ListItem Value="6" />
                    <asp:ListItem Value="7" />
                    <asp:ListItem Value="8" />
                    <asp:ListItem Value="9" />
                    <asp:ListItem Value="10" />
                    <asp:ListItem Value="11" />
                    <asp:ListItem Value="12" />
                    <asp:ListItem Value="13" />
                    <asp:ListItem Value="14" />
                    <asp:ListItem Value="15" />
                    <asp:ListItem Value="16" />
                    <asp:ListItem Value="17" />
                    <asp:ListItem Value="18" />
                    <asp:ListItem Value="19" />
                    <asp:ListItem Value="20" />
                    <asp:ListItem Value="21" />
                    <asp:ListItem Value="22" />
                    <asp:ListItem Value="23" />
                    <asp:ListItem Value="24" />
                    <asp:ListItem Value="25" />
                    <asp:ListItem Value="26" />
                    <asp:ListItem Value="27" />
                    <asp:ListItem Value="28" />
                    <asp:ListItem Value="29" />
                    <asp:ListItem Value="30" />
                    <asp:ListItem Value="31" />
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td nowrap valign="top"><br />Time(s):</td>
            <td width="100%">
                <table cellpadding="4" cellspacing="3" border="0">
                    <tr>
                        <td><asp:CheckBox ID="chk1200AM" runat="server" CssClass="default" Text="12:00 AM" /></td>
                        <td rowspan="50"><img src="/images/spacer.gif" border="0" width="50" height="1" /></td>
                        <td><asp:CheckBox ID="chk1200PM" runat="server" CssClass="default" Text="12:00 PM" /></td>
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
                        <td><asp:CheckBox ID="chk100PM" runat="server" CssClass="default" Text="1:00 PM" /></td>
                    </tr>
                    <tr>
                        <td><asp:CheckBox ID="chk200AM" runat="server" CssClass="default" Text="2:00 AM" /></td>
                        <td><asp:CheckBox ID="chk200PM" runat="server" CssClass="default" Text="2:00 PM" /></td>
                    </tr>
                    <tr>
                        <td><asp:CheckBox ID="chk300AM" runat="server" CssClass="default" Text="3:00 AM" /></td>
                        <td><asp:CheckBox ID="chk300PM" runat="server" CssClass="default" Text="3:00 PM" /></td>
                    </tr>
                    <tr>
                        <td><asp:CheckBox ID="chk400AM" runat="server" CssClass="default" Text="4:00 AM" /></td>
                        <td><asp:CheckBox ID="chk400PM" runat="server" CssClass="default" Text="4:00 PM" /></td>
                    </tr>
                    <tr>
                        <td><asp:CheckBox ID="chk500AM" runat="server" CssClass="default" Text="5:00 AM" /></td>
                        <td><asp:CheckBox ID="chk500PM" runat="server" CssClass="default" Text="5:00 PM" /></td>
                    </tr>
                    <tr>
                        <td><asp:CheckBox ID="chk600AM" runat="server" CssClass="default" Text="6:00 AM" /></td>
                        <td><asp:CheckBox ID="chk600PM" runat="server" CssClass="default" Text="6:00 PM" /></td>
                    </tr>
                    <tr>
                        <td><asp:CheckBox ID="chk700AM" runat="server" CssClass="default" Text="7:00 AM" /></td>
                        <td><asp:CheckBox ID="chk700PM" runat="server" CssClass="default" Text="7:00 PM" /></td>
                    </tr>
                    <tr>
                        <td><asp:CheckBox ID="chk800AM" runat="server" CssClass="default" Text="8:00 AM" /></td>
                        <td><asp:CheckBox ID="chk800PM" runat="server" CssClass="default" Text="8:00 PM" /></td>
                    </tr>
                    <tr>
                        <td><asp:CheckBox ID="chk900AM" runat="server" CssClass="default" Text="9:00 AM" /></td>
                        <td><asp:CheckBox ID="chk900PM" runat="server" CssClass="default" Text="9:00 PM" /></td>
                    </tr>
                    <tr>
                        <td><asp:CheckBox ID="chk1000AM" runat="server" CssClass="default" Text="10:00 AM" /></td>
                        <td><asp:CheckBox ID="chk1000PM" runat="server" CssClass="default" Text="10:00 PM" /></td>
                    </tr>
                    <tr>
                        <td><asp:CheckBox ID="chk1100AM" runat="server" CssClass="default" Text="11:00 AM" /></td>
                        <td><asp:CheckBox ID="chk1100PM" runat="server" CssClass="default" Text="11:00 PM" /></td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td nowrap>Threshold:</td>
            <td width="100%">
                <asp:TextBox ID="txtGroupThreshold" runat="server" CssClass="default" MaxLength="5" Width="75" />
                <span> (number of clients will begin sending notifications - set to &quot;0&quot; to disable thresholds)</span>
            </td>
        </tr>
        <tr>
            <td class="footer">&nbsp;</td>
            <td class="footer">Once the threshold is breached, emails will be sent to the people configured on the &quot;Security&quot; tab.</td>
        </tr>
        <tr>
            <td nowrap>Maximum:</td>
            <td width="100%"><asp:TextBox ID="txtGroupMaximum" runat="server" CssClass="default" MaxLength="5" Width="75" />
                <span> (total number of clients allowed)</span>
            </td>
        </tr>
        <tr>
            <td nowrap>Clustering?:</td>
            <td width="100%"><asp:CheckBox ID="chkGroupClustering" runat="server" CssClass="default" /></td>
        </tr>
        <tr>
            <td nowrap>Enabled:</td>
            <td width="100%"><asp:CheckBox ID="chkGroupEnabled" runat="server" CssClass="default" /></td>
        </tr>
        <tr>
            <td nowrap></td>
            <td width="100%">
                <asp:Button ID="btnGroupAdd" runat="server" CssClass="default" Width="100" Text="Add" OnClick="btnGroupAdd_Click" Visible="false" />
                <asp:Button ID="btnGroupUpdate" runat="server" CssClass="default" Width="100" Text="Update" OnClick="btnGroupUpdate_Click" Visible="false" /> 
                <asp:Button ID="btnGroupDelete" runat="server" CssClass="default" Width="100" Text="Delete" OnClick="btnGroupDelete_Click" Visible="false" /> 
                <asp:Button ID="btnGroupCancel" runat="server" CssClass="default" Width="100" Text="Cancel" OnClick="btnGroupCancel_Click" />
            </td>
        </tr>
    </table>
</asp:Panel>
<input type="hidden" id="hdnLocation" runat="server" />
<asp:HiddenField ID="hdnType" runat="server" />
<input type="hidden" id="hdnEnvironment" runat="server" />
