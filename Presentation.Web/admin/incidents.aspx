<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="incidents.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.incidents" %>



<html>
<head>
<title>ClearView Administration</title>
<link rel="stylesheet" type="text/css" href="/css/default.css" />
<script type="text/javascript" src="/javascript/global.js"></script>
<script type="text/javascript" src="/javascript/default.js"></script>
</head>
<body topmargin="0" leftmargin="0">
<form id="Form1" runat="server">
        <table width="100%" height="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
        <td height="100%">
        <div style="height:100%; overflow:auto">
<table width="98%" cellpadding="0" cellspacing="0" border="0" align="center">
		<tr><td colspan="2">&nbsp;</td></tr>
		<tr> 
		    <td><b>Incident Routing</b></td>
		    <td align="right"><asp:LinkButton id="btnNew" runat="server" Text="Add New" OnClick="btnNew_Click" /></td>
		</tr>
		<tr><td colspan="2">&nbsp;</td></tr>
		<tr>
		    <td colspan="2" align="center">
                <asp:Panel ID="panView" runat="server" Visible="false">
                    <table width="100%" border="0" cellpadding="0" cellspacing="0">
                        <tr>
                            <td>
                                <asp:repeater ID="rptView" runat="server">
                                    <HeaderTemplate>
                                        <table width="100%" cellpadding="3" cellspacing="0" border="0" align="center">
                                            <tr bgcolor="#CCCCCC">
                                                <td class="bold">&nbsp;</td>
                                                <td width="60%" class="bold"><asp:linkbutton ID="lnkName" Text="Name" CommandArgument="error" CssClass="bold" OnClick="OrderView" runat="server"/></td>
                                                <td width="10%" class="bold"><asp:linkbutton ID="lnkCompare" Text="Compare" CommandArgument="compare" CssClass="bold" OnClick="OrderView" runat="server"/></td>
                                                <td width="10%" class="bold"><asp:linkbutton ID="lnkType" Text="Type" CommandArgument="workstation" CssClass="bold" OnClick="OrderView" runat="server"/></td>
                                                <td width="20%" class="bold"><asp:linkbutton ID="lnkRoute" Text="Route" CommandArgument="route" CssClass="bold" OnClick="OrderView" runat="server"/></td>
                                                <td class="bold" align="center"><asp:linkbutton ID="lnkEnabled" Text="Enabled" CommandArgument="enabled" CssClass="bold" OnClick="OrderView" runat="server"/></td>
                                            </tr>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr bgcolor="#EFEFEF" onmouseover="AdminMouseOver(this);" onmouseout="AdminMouseOut(this);" onclick="window.navigate('<%# Request.Path + "?id=" + DataBinder.Eval(Container.DataItem, "id")%>');" class="default">
                                            <td width="5%" align="left">&nbsp;<asp:ImageButton ID="btnDelete" OnClick="btnDeleteLink_Click" runat="server" ToolTip="Delete" ImageAlign="AbsMiddle" ImageUrl="/admin/images/delete.gif" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' /></td>
                                            <td width="60%">&nbsp;<%# DataBinder.Eval(Container.DataItem, "error") %></td>
                                            <td width="10%">&nbsp;<%# DataBinder.Eval(Container.DataItem, "compare") %></td>
                                            <td width="10%">&nbsp;<%# DataBinder.Eval(Container.DataItem, "workstation").ToString() == "1" ? "Workstation" : "Server" %></td>
                                            <td width="20%">&nbsp;<%# DataBinder.Eval(Container.DataItem, "route") %></td>
                                            <td align="center"><asp:ImageButton ID="btnEnable" runat="server" ImageAlign="AbsMiddle" ImageUrl='<%# (DataBinder.Eval(Container.DataItem, "enabled").ToString() == "1") ? "/admin/images/enabled.gif" : "/admin/images/disabled.gif" %>' CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' OnClick="btnEnable_Click" /></td>
                                        </tr>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </table>
                                    </FooterTemplate>
                                </asp:repeater>
                            </td>
                        </tr>
                    </table>
               </asp:Panel>

                <asp:Panel id="panAdd" runat="server" Visible="false">
                    <table width="95%" cellpadding="3" cellspacing="0" border="0" align="center">
                        <tr> 
                            <td class="default">Error:</td>
                            <td><asp:textbox ID="txtError" CssClass="default" runat="server" Width="500" TextMode="MultiLine" Rows="5" /></td>
                        </tr>
                        <tr> 
                            <td class="default">Compare:</td>
                            <td><asp:textbox ID="txtCompare" CssClass="default" runat="server" Width="500" MaxLength="50" /></td>
                        </tr>
                        <tr> 
                            <td></td>
                            <td>
                                <small>
                                    (Optional) Compares with the device name and triggers based on success.  Can use | to separate OR conditions.
                                    <br />
                                    For example, for Linux servers only - set compare to &quot;L%&quot; without the quotes. 
                                    <br />
                                    For example, for Linux servers or Windows servers only - set compare to &quot;L%|W%&quot; without the quotes. 
                                </small>
                            </td>
                        </tr>
                        <tr> 
                            <td class="default">Route:</td>
                            <td>
                                <asp:DropDownList ID="ddlRoute" CssClass="default" runat="server">
                                    <asp:ListItem Value="WAIT" Text="None - Wait 10 minutes and try again..." />
                                    <asp:ListItem Value="CLV ADMINS" />
                                    <asp:ListItem Value="CLV BACKUP" />
                                    <asp:ListItem Value="CLV WINDOWS" />
                                    <asp:ListItem Value="CLV LINUX" />
                                    <asp:ListItem Value="CLV VMWARE" />
                                    <asp:ListItem Value="CLV NETWORK" />
                                    <asp:ListItem Value="CLV DESKTOP" />
                                    <asp:ListItem Value="CLV STORAGE" />
                                    <asp:ListItem Value="CLV EMPLOYEE" />
                                    <asp:ListItem Value="CLV SECURITY" />
                                    <asp:ListItem Value="CLV INVENTORY" />
                                    <asp:ListItem Value="CLV FACILITY CLE" />
                                    <asp:ListItem Value="CLV FACILITY SUM" />
                                    <asp:ListItem Value="CLV FACILITY FIR" />
                                    <asp:ListItem Value="CLV FACILITY CIN" />
                                    <asp:ListItem Value="CLV FACILITY PGH" />
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr> 
                            <td class="default">Automatic:</td>
                            <td><asp:CheckBox ID="chkAutomatic" runat="server" /></td>
                        </tr>
                        <tr> 
                            <td></td>
                            <td>
                                <small>
                                    Check this box to automatically route new incidents. Otherwise, they will appear as suggestions in the provisioning error queue.
                                    <br />
                                    <strong><em>Automatic</em> is required if selecting &quot;None / Wait&quot;</strong>
                                </small>
                            </td>
                        </tr>
                        <tr> 
                            <td class="default">Message:</td>
                            <td><asp:textbox ID="txtMessage" CssClass="default" runat="server" Width="500" TextMode="MultiLine" Rows="5" /></td>
                        </tr>
                        <tr> 
                            <td></td>
                            <td><small>The message is the information that gets sent along with the incident. <span style="display:inline">To automatically insert the a variable, <a href="javascript:;" onclick="ShowHideDiv2('Variables');">enter the variable name</a>. </span>To use the full error message, leave this field blank.</small></td>
                        </tr>
                        <tr id="Variables" style="display: none"> 
                            <td></td>
                            <td>
                                <strong>Variables</strong><br />
                                <table>
                                    <tr>
                                        <td>@answerid</td>
                                        <td>The ID of the design</td>
                                    </tr>
                                    <tr>
                                        <td>@name</td>
                                        <td>The name of the device</td>
                                    </tr>
                                    <tr>
                                        <td>@serial</td>
                                        <td>The serial number of the device</td>
                                    </tr>
                                    <tr>
                                        <td>@os</td>
                                        <td>The operating system loaded on the device</td>
                                    </tr>
                                    <tr>
                                        <td>@model</td>
                                        <td>The model of the device</td>
                                    </tr>
                                    <tr>
                                        <td>@ram</td>
                                        <td>The amount of RAM installed on the device (in GB)</td>
                                    </tr>
                                    <tr>
                                        <td>@cpu</td>
                                        <td>The number of CPUs configured on the device</td>
                                    </tr>
                                    <tr>
                                        <td>@class</td>
                                        <td>The logical operating class of the device (Production, Test, etc...)</td>
                                    </tr>
                                    <tr>
                                        <td>@environment</td>
                                        <td>The logical operating environment of the device (Core, Warm, etc...)</td>
                                    </tr>
                                    <tr>
                                        <td>@location</td>
                                        <td>The physical address where the device is located</td>
                                    </tr>
                                    <tr>
                                        <td>@room</td>
                                        <td>The room where the device is located</td>
                                    </tr>
                                    <tr>
                                        <td>@zone</td>
                                        <td>The zone wherer the device is located</td>
                                    </tr>
                                    <tr>
                                        <td>@rack</td>
                                        <td>The rack where the device is located</td>
                                    </tr>
                                    <tr>
                                        <td>@rack_position</td>
                                        <td>The position within the rack where the device is located</td>
                                    </tr>
                                    <tr>
                                        <td>@domain</td>
                                        <td>The domain of the device</td>
                                    </tr>
                                    <tr>
                                        <td>@remote_mgmt</td>
                                        <td>The IP address to the remote management console</td>
                                    </tr>
                                    <tr>
                                        <td>@macaddress</td>
                                        <td>The MAC address assigned to the device</td>
                                    </tr>
                                    <tr>
                                        <td>@mnemonic</td>
                                        <td>The mnemonic assigned to the device</td>
                                    </tr>
                                    <tr>
                                        <td>@storage</td>
                                        <td>The amount of storage allocated to the device</td>
                                    </tr>
                                    <tr>
                                        <td>@virtual_center</td>
                                        <td>The virtual center server of the vmware guest</td>
                                    </tr>
                                    <tr>
                                        <td>@vmware_data_center</td>
                                        <td>The data center of the vmware guest</td>
                                    </tr>
                                    <tr>
                                        <td>@vmware_folder</td>
                                        <td>The folder within the data center of the vmware guest</td>
                                    </tr>
                                    <tr>
                                        <td>@vmware_cluster</td>
                                        <td>The cluster name of the vmware guest</td>
                                    </tr>
                                    <tr>
                                        <td>@vmware_host</td>
                                        <td>The host name of the vmware guest</td>
                                    </tr>
                                    <tr>
                                        <td>@vmware_datastore</td>
                                        <td>The datastore of the vmware guest</td>
                                    </tr>
                                    <tr>
                                        <td>@vmware_datastore2</td>
                                        <td>The second datastore of the vmware guest</td>
                                    </tr>
                                    <tr>
                                        <td>@vmware_vlan</td>
                                        <td>The name of the VLAN within virtual center of the IP address assigned to the vmware guest</td>
                                    </tr>
                                    <tr>
                                        <td>@backup_registration_command</td>
                                        <td>The command used to register the backup</td>
                                    </tr>
                                    <tr>
                                        <td>@backup_registration_output</td>
                                        <td>The output of the command used to register the backup</td>
                                    </tr>
                                    <tr>
                                        <td>@backup_grid</td>
                                        <td>The name of the grid to which the device is to be backed up</td>
                                    </tr>
                                    <tr>
                                        <td>@backup_domain</td>
                                        <td>The name of the domain to which the device is to be backed up</td>
                                    </tr>
                                    <tr>
                                        <td>@backup_activiation_command</td>
                                        <td>The command used to activate the backup</td>
                                    </tr>
                                    <tr>
                                        <td>@backup_activiation_output</td>
                                        <td>The output of the command used to activate the backup</td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr> 
                            <td class="default">Priority:</td>
                            <td>
                                <asp:DropDownList ID="ddlPriority" CssClass="default" runat="server">
                                    <asp:ListItem Value="0" Text="-- Default --" />
                                    <asp:ListItem Value="1" />
                                    <asp:ListItem Value="2" />
                                    <asp:ListItem Value="3" />
                                    <asp:ListItem Value="4" />
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr> 
                            <td class="default">Type:</td>
                            <td>
                                <asp:RadioButton ID="radWorkstation" runat="server" Text="Workstation" GroupName="Type" />
                                <asp:RadioButton ID="radServer" runat="server" Text="Server" GroupName="Type" />
                            </td>
                        </tr>
                        <tr> 
                            <td class="default">Enabled:</td>
                            <td><asp:CheckBox ID="chkEnabled" runat="server" Checked="true" /></td>
                        </tr>
                        <tr><td height="5" colspan="2">&nbsp;</td></tr>
                        <tr> 
                            <td>&nbsp;</td>
                            <td>
                                <asp:button ID="btnAdd" CssClass="default" runat="server" Text="Add" Width="75" OnClick="btnAdd_Click" />
                                <asp:button ID="btnDelete" CssClass="default" runat="server" Text="Delete" Width="75" OnClick="btnDelete_Click" />
                                <asp:button ID="btnCancel" CssClass="default" runat="server" Text="Cancel" Width="75" OnClick="btnCancel_Click" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
    </table>
        </div>
        </td>
        </tr>
        </table>
    <input type="hidden" id="hdnId" runat="server" />
</form>
</body>
</html>
