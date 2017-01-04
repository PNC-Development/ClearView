<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="sve_admin.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.sve_admin" %>


<script type="text/javascript">
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
            <table width="100%" cellpadding="0" cellspacing="5" border="0">
                <tr>
                    <td rowspan="2"><img src="/images/star_green.gif" border="0" align="absmiddle" /></td>
                    <td class="header" width="100%" valign="bottom">Sun Virtual Environment Administration</td>
                </tr>
                <tr>
                    <td width="100%" valign="top">Use this page to administer SVE clusters, hosts, guests, processor pools and subnets.</td>
                </tr>
            </table>
            <asp:Label ID="lblThreshold" runat="server" CssClass="default" />
            <asp:Panel ID="panClusters" runat="server" Visible="false">
                <fieldset>
                    <legend class="tableheader"><b>Filtering Otions</b></legend>
                    <table width="100%" cellpadding="4" cellspacing="0" border="0">
                        <tr>
                            <td>
                                <b>Available:</b>
                                &nbsp;
                                <asp:RadioButtonList ID="radAvailable" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem Value="-1" Text="All" Selected="true" />
                                    <asp:ListItem Value="1" Text="Yes" />
                                    <asp:ListItem Value="0" Text="No" />
                                </asp:RadioButtonList>
                            </td>
                            <td>
                                <b>Trunked:</b>
                                &nbsp;
                                <asp:RadioButtonList ID="radTrunk" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem Value="-1" Text="All" Selected="true" />
                                    <asp:ListItem Value="1" Text="Yes" />
                                    <asp:ListItem Value="0" Text="No" />
                                </asp:RadioButtonList>
                            </td>
                            <td><asp:CheckBox ID="chkMnemonics" runat="server" Text="Open to all mnemonics" /></td>
                            <td align="right">
                                <asp:Button ID="btnFilter" runat="server" Text="Apply Filter" Width="75" OnClick="btnFilter_Click" />
                                &nbsp;
                                <asp:Button ID="btnFilterClear" runat="server" Text="Clear Filter" Width="75" OnClick="btnFilterClear_Click" Visible="false" />
                            </td>
                        </tr>
                    </table>
                </fieldset>
                <br />
                <table width="100%" cellpadding="4" cellspacing="0" border="0" align="center" style="border:solid 1px #CCCCCC">
                    <tr bgcolor="#EEEEEE">
                        <td></td>
                        <td width="15%" nowrap><b>Name</b></td>
                        <td width="5%" nowrap><b>Hosts</b></td>
                        <td width="5%" nowrap><b>Guests</b></td>
                        <td width="10%" nowrap><b>Class</b></td>
                        <td width="10%" nowrap><b>Type</b></td>
                        <td width="20%" nowrap><b>IPs</b></td>
                        <td width="20%" nowrap><b>Pools</b></td>
                        <td width="15%" nowrap><b>Modified</b></td>
                        <td nowrap align="center"><b>Available</b></td>
                    </tr>
                <asp:repeater ID="rptClusters" runat="server">
                    <ItemTemplate>
                        <tr class='<%# DataBinder.Eval(Container.DataItem, "available").ToString() == "1" ? "default" : "component_unavailable" %>' title='<%# DataBinder.Eval(Container.DataItem, "comments").ToString().Replace("'", "\\'") %>' onmouseover="CellRowOver(this);" onmouseout="CellRowOut(this);" onclick="window.navigate('<%# oPage.GetFullLink(intPage) + "?cluster=" + DataBinder.Eval(Container.DataItem, "id") %>');">
                            <td nowrap><asp:Label ID="lblStatus" runat="server" CssClass="default" /></td>
                            <td width="15%" nowrap><%# DataBinder.Eval(Container.DataItem, "name") %></td>
                            <td width="5%" nowrap><asp:Label ID="lblHosts" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "id") %>' /></td>
                            <td width="5%" nowrap><asp:Label ID="lblGuests" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "id") %>' /></td>
                            <td width="10%" nowrap><%# DataBinder.Eval(Container.DataItem, "class") %></td>
                            <td width="10%" nowrap><%# DataBinder.Eval(Container.DataItem, "db").ToString() == "1" ? "Database" : "Non-Database" %></td>
                            <td width="20%" nowrap><asp:Label ID="lblIPs" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "id") %>' /></td>
                            <td width="20%" nowrap><asp:Label ID="lblPools" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "id") %>' /></td>
                            <td width="15%" nowrap><%# DataBinder.Eval(Container.DataItem, "modified") %></td>
                            <td nowrap align="center"><img src="/images/<%# DataBinder.Eval(Container.DataItem, "available").ToString() == "1" ? "check" : "cancel" %>.gif" border="0" align="absmiddle" /></td>
                        </tr>
                    </ItemTemplate>
                </asp:repeater>
                    <tr>
                        <td colspan="6"><asp:Label ID="lblClusters" runat="server" CssClass="error" Visible="false" Text="<img src='/images/bigAlert.gif' border='0' align='absmiddle'> There are no servers" /></td>
                    </tr>
                </table>
                <br /><br />
                <asp:Button ID="btnAddCluster" runat="server" CssClass="default" Width="75" Text="Add New" OnClick="btnAddCluster_Click" />
            </asp:Panel>
            <asp:Panel ID="panCluster" runat="server" Visible="false">
                <table width="100%" cellpadding="4" cellspacing="3" border="0">
                    <tr>
                        <td nowrap>Name:</td>
                        <td width="100%"><asp:TextBox ID="txtClusterName" runat="server" CssClass="default" Width="400" MaxLength="100" /></td>
                    </tr>
                    <tr>
                        <td nowrap>Type:</td>
                        <td width="100%">
                            <asp:RadioButton ID="radClusterDB" runat="server" CssClass="default" Text="Database" GroupName="Type" /> 
                            <asp:RadioButton ID="radClusterApp" runat="server" CssClass="default" Text="Application (Non-Database)" GroupName="Type" /> 
                        </td>
                    </tr>
                    <tr>
                        <td nowrap>Class:</td>
                        <td width="100%"><asp:DropDownList ID="ddlClusterClass" runat="server" CssClass="default" Width="300" /></td>
                    </tr>
                    <tr>
                        <td nowrap>Resiliency:</td>
                        <td width="100%"><asp:DropDownList ID="ddlClusterResiliency" runat="server" CssClass="default" Width="400" /></td>
                    </tr>
                    <tr>
                        <td nowrap valign="top">Comments:</td>
                        <td width="100%"><asp:TextBox ID="txtClusterComments" runat="server" CssClass="default" Width="400" TextMode="MultiLine" Rows="8" /></td>
                    </tr>
                    <tr>
                        <td nowrap>Storage Present:</td>
                        <td width="100%"><asp:CheckBox ID="chkClusterStorage" runat="server" CssClass="default" /></td>
                    </tr>
                    <tr>
                        <td nowrap>Trunking Enabled:</td>
                        <td width="100%"><asp:CheckBox ID="chkClusterTrunking" runat="server" CssClass="default" /></td>
                    </tr>
                    <tr>
                        <td nowrap>Available:</td>
                        <td width="100%"><asp:CheckBox ID="chkClusterAvailable" runat="server" CssClass="default" /></td>
                    </tr>
                    <tr>
                        <td nowrap></td>
                        <td width="100%"><asp:Button ID="btnClusterAdd" runat="server" CssClass="default" Width="100" Text="Add" OnClick="btnClusterAdd_Click" Visible="false" /><asp:Button ID="btnClusterUpdate" runat="server" CssClass="default" Width="100" Text="Update" OnClick="btnClusterUpdate_Click" Visible="false" /> <asp:Button ID="btnClusterDelete" runat="server" CssClass="default" Width="100" Text="Delete" OnClick="btnClusterDelete_Click" Visible="false" /> <asp:Button ID="btnClusterCancel" runat="server" CssClass="default" Width="100" Text="Cancel" OnClick="btnClusterCancel_Click" /></td>
                    </tr>
                </table>
                <br /><br />
                <%=strMenuTab1 %>
                <div id="divMenu1" class="tabbing">
                    <br />
                    <div style="display:none">
                        <span class="header">Hosts</span>
                        <br /><br />
                        <table width="100%" cellpadding="4" cellspacing="0" border="0" align="center" style="border:solid 1px #CCCCCC">
                            <tr bgcolor="#EEEEEE">
                                <td width="40%" nowrap><b>Name</b></td>
                                <td width="30%" nowrap><b>Serial</b></td>
                                <td width="30%" nowrap><b>Model</b></td>
                            </tr>
                        <asp:repeater ID="rptHosts" runat="server">
                            <ItemTemplate>
                                <tr onmouseover="CellRowOver(this);" onmouseout="CellRowOut(this);" onclick="window.navigate('<%# oPage.GetFullLink(intPage) + "?cluster=" + intCluster.ToString() + "&host=" + DataBinder.Eval(Container.DataItem, "id") %>');">
                                    <td width="40%" nowrap><%# DataBinder.Eval(Container.DataItem, "servername")%></td>
                                    <td width="30%" nowrap><%# DataBinder.Eval(Container.DataItem, "serial")%></td>
                                    <td width="30%" nowrap><%# DataBinder.Eval(Container.DataItem, "model")%></td>
                                </tr>
                            </ItemTemplate>
                        </asp:repeater>
                            <tr>
                                <td colspan="6"><asp:Label ID="lblHosts" runat="server" CssClass="error" Visible="false" Text="<img src='/images/bigAlert.gif' border='0' align='absmiddle'> There are no hosts for this cluster" /></td>
                            </tr>
                        </table>
                        <br /><br />
                        <asp:Button ID="btnAddHost" runat="server" CssClass="default" Width="75" Text="Add New" OnClick="btnAddHost_Click" />
                        <br /><br />
                        <b>NOTE:</b> A maximum of 8 hosts can be added to a cluster.
                    </div>
                    <div style="display:none">
                        <span class="header">Locations</span>
                        <br /><br />
                        <table width="100%" border="0" cellSpacing="2" cellPadding="2">
                            <tr>
                                <td class="default">Location:</td>
                                <td><%=strLocation %></td>
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                                <td><asp:Button ID="btnLocation" runat="server" CssClass="default" Width="75" Text="Add" OnClick="btnLocation_Click" /></td>
                            </tr>
                            <tr>
                                <td colspan="2">&nbsp;</td>
                            </tr>
                            <tr>
                                <td colspan="2" valign="top">
                                    <table width="100%" cellpadding="4" cellspacing="0" border="0" style="border:solid 1px #CCCCCC">
                                        <tr bgcolor="#EEEEEE">
                                            <td nowrap></td>
                                            <td nowrap><b>Location</b></td>
                                        </tr>
                                        <asp:repeater ID="rptLocations" runat="server">
                                            <ItemTemplate>
                                                <tr>
                                                    <td align="left">&nbsp;<asp:ImageButton ID="btnDeleteLocation" OnClick="btnDeleteLocation_Click" runat="server" ToolTip="Delete" ImageAlign="AbsMiddle" ImageUrl="/admin/images/delete.gif" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' /></td>
                                                    <td width="100%"><%# oLocation.GetFull(Int32.Parse(DataBinder.Eval(Container.DataItem, "addressid").ToString())) %></td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:repeater>
                                        <tr>
                                            <td colspan="3">
                                                <asp:Label ID="lblLocations" runat="server" CssClass="default" Visible="false" Text="<img src='/images/alert.gif' border='0' align='absmiddle'> There are no locations..." />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div style="display:none">
                        <span class="header">Processor Pools</span>
                        <br /><br />
                        <table width="100%" cellpadding="4" cellspacing="0" border="0" align="center" style="border:solid 1px #CCCCCC">
                            <tr bgcolor="#EEEEEE">
                                <td width="35%" nowrap><b>Name</b></td>
                                <td width="20%" nowrap><b>Availability</b></td>
                                <td width="20%" nowrap><b>Threshold</b></td>
                                <td width="20%" nowrap><b>Status</b></td>
                                <td width="15%" nowrap><b>Modified</b></td>
                                <td nowrap align="center"><b>Enabled</b></td>
                            </tr>
                        <asp:repeater ID="rptProcessors" runat="server">
                            <ItemTemplate>
                                <tr class='<%# DataBinder.Eval(Container.DataItem, "enabled").ToString() == "1" ? "default" : "component_unavailable" %>' onmouseover="CellRowOver(this);" onmouseout="CellRowOut(this);" onclick="window.navigate('<%# oPage.GetFullLink(intPage) + "?cluster=" + intCluster.ToString() + "&processor=" + DataBinder.Eval(Container.DataItem, "id") %>');">
                                    <td width="35%" nowrap><%# DataBinder.Eval(Container.DataItem, "name") %></td>
                                    <td width="30%" nowrap><%# DataBinder.Eval(Container.DataItem, "low") %> - <%# DataBinder.Eval(Container.DataItem, "high") %> CPU(s)</td>
                                    <td width="30%" nowrap><%# DataBinder.Eval(Container.DataItem, "warning") %>, <%# DataBinder.Eval(Container.DataItem, "critical") %>, <%# DataBinder.Eval(Container.DataItem, "error") %> CPU(s)</td>
                                    <td width="15%" nowrap><asp:Label ID="lblStatus" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "id") %>' /></td>
                                    <td width="15%" nowrap><%# DataBinder.Eval(Container.DataItem, "modified") %></td>
                                    <td nowrap align="center"><img src="/images/<%# DataBinder.Eval(Container.DataItem, "enabled").ToString() == "1" ? "check" : "cancel" %>.gif" border="0" align="absmiddle" /></td>
                                </tr>
                            </ItemTemplate>
                        </asp:repeater>
                            <tr>
                                <td colspan="6"><asp:Label ID="lblProcessors" runat="server" CssClass="error" Visible="false" Text="<img src='/images/bigAlert.gif' border='0' align='absmiddle'> There are no processor pools for this cluster" /></td>
                            </tr>
                        </table>
                        <br /><br />
                        <asp:Button ID="btnAddProcessor" runat="server" CssClass="default" Width="75" Text="Add New" OnClick="btnAddProcessor_Click" />
                    </div>
                    <div style="display:none" runat=server id="divIP" visible=false>
                        <span class="header">IP Subnets</span>
                        <br /><br />
                        <table width="100%" cellpadding="4" cellspacing="0" border="0" align="center" style="border:solid 1px #CCCCCC">
                            <tr bgcolor="#EEEEEE">
                                <td width="5%"><b>Use?</b></td>
                                <td width="20%" nowrap><b>Available Address Range(s)</b></td>
                                <td width="15%" nowrap><b>Mask</b></td>
                                <td width="15%" nowrap><b>Gateway</b></td>
                                <td width="25%" nowrap><b>Description</b></td>
                                <td width="20%" nowrap><b>Status</b></td>
                            </tr>
                        <asp:repeater ID="rptSubnets" runat="server">
                            <ItemTemplate>
                                <tr class='<%# DataBinder.Eval(Container.DataItem, "selected").ToString() == "1" ? "default" : "component_unavailable" %>' onmouseover="CellRowOver(this);" onmouseout="CellRowOut(this);">
                                        <asp:Label ID="lblAdd1" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "add1") %>' />
                                        <asp:Label ID="lblAdd2" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "add2") %>' />
                                        <asp:Label ID="lblAdd3" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "add3") %>' />
                                        <asp:Label ID="lblMin4" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "min4") %>' />
                                        <asp:Label ID="lblMax4" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "max4") %>' />
                                    <td width="5%"><asp:CheckBox ID="chkSelected" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "selected").ToString() == "1" %>' /></td>
                                    <td width="20%" nowrap><%# DataBinder.Eval(Container.DataItem, "add1").ToString() + "." + DataBinder.Eval(Container.DataItem, "add2").ToString() + "." + DataBinder.Eval(Container.DataItem, "add3").ToString() + "." + DataBinder.Eval(Container.DataItem, "min4").ToString()%> - <%# DataBinder.Eval(Container.DataItem, "add1").ToString() + "." + DataBinder.Eval(Container.DataItem, "add2").ToString() + "." + DataBinder.Eval(Container.DataItem, "add3").ToString() + "." + DataBinder.Eval(Container.DataItem, "max4").ToString()%></td>
                                    <td width="15%" nowrap><%# DataBinder.Eval(Container.DataItem, "mask") %></td>
                                    <td width="15%" nowrap><%# DataBinder.Eval(Container.DataItem, "gateway") %></td>
                                    <td width="25%" nowrap><%# DataBinder.Eval(Container.DataItem, "description") %></td>
                                    <td width="20%" nowrap><asp:Label ID="lblInUse" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "inuse") %>' /></td>
                                </tr>
                            </ItemTemplate>
                        </asp:repeater>
                            <tr>
                                <td colspan="6"><asp:Label ID="lblSubnets" runat="server" CssClass="error" Visible="false" Text="<img src='/images/bigAlert.gif' border='0' align='absmiddle'> There are no IP subnets for this cluster" /></td>
                            </tr>
                        </table>
                        <br />
                        <table width="100%" cellpadding="4" cellspacing="0" border="0" align="center">
                            <tr>
                                <td>
                                    <asp:Button ID="btnSaveSubnet" runat="server" CssClass="default" Width="75" Text="Save" OnClick="btnSaveSubnet_Click" />
                                </td>
                                <td align="right">
                                    <a href="javascript:void(0);" onclick="ShowHideDiv2('divIPThreshold');"><img src="/images/alert.gif" border="0" align="absmiddle" /> Click here to view information regarding the IP Subnet Thresholds...</a>
                                </td>
                            </tr>
                        </table>
                        <div id="divIPThreshold" style="display:none">
                            <br /><br />
                            <table cellpadding="5" cellspacing="0" border="0" style="border:solid 1px #CCCCCC">
                                <tr bgcolor="#EEEEEE">
                                    <td colspan="3"><b>IP Range Thresholds&nbsp;&nbsp;&nbsp;(NOTE: These cannot be changed)</b></td>
                                </tr>
                                <tr>
                                    <td align="center" nowrap><img src="/images/alert.gif" border="0" align="absmiddle" /> Warning</td>
                                    <td>Once the number of available IP addresses is less than <b>20</b>, email notifications are sent and a WARNING message appears.</td>
                                </tr>
                                <tr>
                                    <td align="center" nowrap><img src="/images/red_alert.gif" border="0" align="absmiddle" /> Critical</td>
                                    <td>Once the number of available IP addresses is less than <b>10</b>, email notifications are sent and a CRITICAL message appears.</td>
                                </tr>
                                <tr>
                                    <td align="center" nowrap><img src="/images/error.gif" border="0" align="absmiddle" /> Error</td>
                                    <td>Once the number of available IP addresses is less than <b>0</b>, the process is halted.</td>
                                </tr>
                            </table>
                            <br /><br />
                            <table cellpadding="5" cellspacing="0" border="0" style="border:solid 1px #CCCCCC">
                                <tr bgcolor="#EEEEEE">
                                    <td colspan="3"><b>IP Subnet Thresholds&nbsp;&nbsp;&nbsp;(NOTE: These cannot be changed)</b></td>
                                </tr>
                                <tr>
                                    <td align="center" nowrap><img src="/images/alert.gif" border="0" align="absmiddle" /> Warning</td>
                                    <td>Once the total number of available IP addresses in a <u>SUBNET</u> is less than <b>60</b>, email notifications are sent and a WARNING message appears.</td>
                                </tr>
                                <tr>
                                    <td align="center" nowrap><img src="/images/red_alert.gif" border="0" align="absmiddle" /> Critical</td>
                                    <td>Once the total number of available IP addresses in a <u>SUBNET</u> is less than <b>30</b>, email notifications are sent and a CRITICAL message appears.</td>
                                </tr>
                                <tr>
                                    <td align="center" nowrap><img src="/images/error.gif" border="0" align="absmiddle" /> Error</td>
                                    <td>Once the total number of available IP addresses in a <u>SUBNET</u> is less than <b>0</b>, the process is halted.</td>
                                </tr>
                            </table>
                            <br />
                            <b>NOTE:</b> IP Subnets are based on the availablity of 235 IP addresses (.20 - .254).
                            <p>
                                <b>EXAMPLE:</b><br />
                                One range is configured = 10.50.100.254 - 10.50.100.150.  Once addresses 10.50.100.254 - 10.50.100.170 are in use, the WARNING threshold will be reached (less than 20 left).  Once addresses 10.50.100.254 - 10.50.100.160 are in use, the CRITICAL threshold will be reached (less than 10 left).  At no time will we encounter the subnet thresholds since addresses 149 - 20 are still available.<br />
                                Let's say a second range is configured = 10.50.100.149 - 10.50.100.20.  Once addresses 10.50.100.149 - 10.50.100.80 are in use, the WARNING threshold on the SUBNET will be reached (less than 60 left).  The warning for the range will not be reached until addresses 10.50.100.149 - 10.50.100.40 are in use.
                            </p>
                        </div>
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
                                <tr onmouseover="CellRowOver(this);" onmouseout="CellRowOut(this);" onclick="window.navigate('<%# oPage.GetFullLink(intPage) + "?cluster=" + intCluster.ToString() + "&mnemonic=" + DataBinder.Eval(Container.DataItem, "id") %>');">
                                    <td width="60%" nowrap><%# DataBinder.Eval(Container.DataItem, "factory_code")%> - <%# DataBinder.Eval(Container.DataItem, "name")%></td>
                                    <td width="40%" nowrap><%# DataBinder.Eval(Container.DataItem, "modified") %></td>
                                    <td nowrap align="center"><img src="/images/<%# DataBinder.Eval(Container.DataItem, "enabled").ToString() == "1" ? "check" : "cancel" %>.gif" border="0" align="absmiddle" /></td>
                                </tr>
                            </ItemTemplate>
                        </asp:repeater>
                            <tr>
                                <td colspan="6"><asp:Label ID="lblMnemonics" runat="server" CssClass="default" Visible="false" Text="<img src='/images/bigCheck.gif' border='0' align='absmiddle'> This cluster is available for all mnemonics" /></td>
                            </tr>
                        </table>
                        <br />
                        <table width="100%" cellpadding="4" cellspacing="0" border="0" align="center">
                            <tr>
                                <td>
                                    <asp:Button ID="btnAddMnemonic" runat="server" CssClass="default" Width="75" Text="Add New" OnClick="btnAddMnemonic_Click" />
                                </td>
                                <td align="right">
                                    <img src="/images/alert.gif" border="0" align="absmiddle" /> This cluster is only available for the mnemonics listed above.
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div style="display:none">
                        <span class="header">Guests</span>
                        <br /><br />
                        <table width="100%" cellpadding="4" cellspacing="0" border="0" align="center" style="border:solid 1px #CCCCCC">
                            <tr bgcolor="#EEEEEE">
                                <td><b></b></td>
                                <td><b></b></td>
                                <td width="25%" nowrap><b>Name</b></td>
                                <td width="20%" nowrap><b>Processor Pool</b></td>
                                <td width="10%" nowrap><b>Storage</b></td>
                                <td width="10%" nowrap><b>CPU</b></td>
                                <td width="10%" nowrap><b>RAM</b></td>
                                <td width="25%" nowrap><b>Modified</b></td>
                            </tr>
                        <asp:repeater ID="rptGuests" runat="server">
                            <ItemTemplate>
                                <tr onmouseover="CellRowOver(this);" onmouseout="CellRowOut(this);" onclick="if(event.srcElement.tagName != 'INPUT')window.navigate('<%# oPage.GetFullLink(intPage) + "?cluster=" + intCluster.ToString() + "&guest=" + DataBinder.Eval(Container.DataItem, "serverid") %>');">
                                    <td><%=intGuestCount++%>.)</td>
                                    <td><input type="checkbox" onclick="ChangeCheckItems('<%=hdnMove.ClientID %>', '<%# DataBinder.Eval(Container.DataItem, "serverid")%>', this)" /></td>
                                    <td width="25%" nowrap><%# DataBinder.Eval(Container.DataItem, "servername")%></td>
                                    <td width="20%" nowrap><%# DataBinder.Eval(Container.DataItem, "pool")%></td>
                                    <td width="10%" nowrap><%# DataBinder.Eval(Container.DataItem, "allocated").ToString() == "" ? "0" : DataBinder.Eval(Container.DataItem, "allocated").ToString()%> GB(s)</td>
                                    <td width="10%" nowrap><%# DataBinder.Eval(Container.DataItem, "cpu").ToString() == "" ? "0" : DataBinder.Eval(Container.DataItem, "cpu").ToString()%> CPU(s)</td>
                                    <td width="10%" nowrap><%# DataBinder.Eval(Container.DataItem, "ram").ToString() == "" ? "0" : DataBinder.Eval(Container.DataItem, "ram").ToString()%> GB(s)</td>
                                    <td width="25%" nowrap><%# DataBinder.Eval(Container.DataItem, "modified")%></td>
                                </tr>
                            </ItemTemplate>
                            <AlternatingItemTemplate>
                                <tr style="background-color:#F6F6F6" onmouseover="CellRowOver(this);" onmouseout="CellRowOut(this);" onclick="if(event.srcElement.tagName != 'INPUT')window.navigate('<%# oPage.GetFullLink(intPage) + "?cluster=" + intCluster.ToString() + "&guest=" + DataBinder.Eval(Container.DataItem, "serverid") %>');">
                                    <td><%=intGuestCount++%>.)</td>
                                    <td><input type="checkbox" onclick="ChangeCheckItems('<%=hdnMove.ClientID %>', '<%# DataBinder.Eval(Container.DataItem, "serverid")%>', this)" /></td>
                                    <td width="25%" nowrap><%# DataBinder.Eval(Container.DataItem, "servername")%></td>
                                    <td width="20%" nowrap><%# DataBinder.Eval(Container.DataItem, "pool")%></td>
                                    <td width="10%" nowrap><%# DataBinder.Eval(Container.DataItem, "allocated").ToString() == "" ? "0" : DataBinder.Eval(Container.DataItem, "allocated").ToString()%> GB(s)</td>
                                    <td width="10%" nowrap><%# DataBinder.Eval(Container.DataItem, "cpu").ToString() == "" ? "0" : DataBinder.Eval(Container.DataItem, "cpu").ToString()%> CPU(s)</td>
                                    <td width="10%" nowrap><%# DataBinder.Eval(Container.DataItem, "ram").ToString() == "" ? "0" : DataBinder.Eval(Container.DataItem, "ram").ToString()%> GB(s)</td>
                                    <td width="25%" nowrap><%# DataBinder.Eval(Container.DataItem, "modified")%></td>
                                </tr>
                            </AlternatingItemTemplate>
                        </asp:repeater>
                            <tr>
                                <td colspan="6"><asp:Label ID="lblGuests" runat="server" CssClass="error" Visible="false" Text="<img src='/images/bigAlert.gif' border='0' align='absmiddle'> There are no guests in this cluster" /></td>
                            </tr>
                        </table>
                        <br /><br />
                        <asp:Button ID="btnMoveGuest" runat="server" CssClass="default" Width="125" Text="Move Selected" OnClick="btnMoveGuest_Click" />
                    </div>
                </div>
            </asp:Panel>
            <asp:Panel ID="panHost" runat="server" Visible="false">
                <table width="100%" cellpadding="4" cellspacing="3" border="0">
                    <tr>
                        <td nowrap>Cluster:</td>
                        <td width="100%"><asp:Label ID="lblHostCluster" runat="server" CssClass="default" /></td>
                    </tr>
                    <tr>
                        <td nowrap>Host:</td>
                        <td width="100%">
                            <table cellpadding="0" cellspacing="0" border="0">
                                <tr>
                                    <td><asp:TextBox ID="txtHost" runat="server" Width="200" CssClass="default" /></td>
                                </tr>
                                <tr>
                                    <td>
                                        <div id="divHost" runat="server" style="overflow:hidden; position:absolute; display:none; background-color:#FFFFFF; border:solid 1px #CCCCCC">
                                            <asp:ListBox ID="lstHost" runat="server" CssClass="default" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr id="trHost1" runat="server" visible="false">
                        <td nowrap>Serial Number:</td>
                        <td width="100%"><asp:Label ID="lblSerial" runat="server" CssClass="default" /></td>
                    </tr>
                    <tr id="trHost2" runat="server" visible="false">
                        <td nowrap>Backup Server:</td>
                        <td width="100%"><asp:DropDownList ID="ddlBackupServer" runat="server" CssClass="default" Width="200" /></td>
                    </tr>
                    <tr id="trHost3" runat="server" visible="false">
                        <td nowrap>Backup Domain:</td>
                        <td width="100%"><asp:DropDownList ID="ddlBackupDomain" runat="server" CssClass="default" Width="200" /></td>
                    </tr>
                    <tr id="trHost4" runat="server" visible="false">
                        <td nowrap>Backup Schedule:</td>
                        <td width="100%"><asp:DropDownList ID="ddlBackupSchedule" runat="server" CssClass="default" Width="200" /></td>
                    </tr>
                    <tr>
                        <td nowrap></td>
                        <td width="100%"><asp:Button ID="btnHostAdd" runat="server" CssClass="default" Width="100" Text="Add" OnClick="btnHostAdd_Click" Visible="false" /><asp:Button ID="btnHostUpdate" runat="server" CssClass="default" Width="100" Text="Update" OnClick="btnHostUpdate_Click" Visible="false" /> <asp:Button ID="btnHostDelete" runat="server" CssClass="default" Width="100" Text="Delete" OnClick="btnHostDelete_Click" Visible="false" /> <asp:Button ID="btnHostCancel" runat="server" CssClass="default" Width="100" Text="Cancel" OnClick="btnHostCancel_Click" /></td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="panProcessor" runat="server" Visible="false">
                <table width="100%" cellpadding="4" cellspacing="3" border="0">
                    <tr>
                        <td nowrap>Cluster:</td>
                        <td width="100%"><asp:Label ID="lblProcessorCluster" runat="server" CssClass="default" /></td>
                    </tr>
                    <tr>
                        <td nowrap>Processor Pool:</td>
                        <td width="100%"><asp:TextBox ID="txtProcessorName" runat="server" CssClass="default" Width="400" MaxLength="100" /></td>
                    </tr>
                    <tr>
                        <td valign="top" nowrap>Description:</td>
                        <td width="100%"><asp:TextBox ID="txtProcessorDescription" runat="server" CssClass="default" Width="400" TextMode="MultiLine" Rows="8" /></td>
                    </tr>
                    <tr>
                        <td nowrap>Enabled:</td>
                        <td width="100%"><asp:CheckBox ID="chkProcessorEnabled" runat="server" CssClass="default" /></td>
                    </tr>
                    <tr>
                        <td nowrap valign="top"></td>
                        <td width="100%">
                            <table cellpadding="5" cellspacing="0" border="0" style="border:solid 1px #CCCCCC">
                                <tr bgcolor="#EEEEEE">
                                    <td colspan="3"><b>Availability</b></td>
                                </tr>
                                <tr>
                                    <td align="center" nowrap>Low</td>
                                    <td><asp:TextBox ID="txtProcessorLow" runat="server" CssClass="default" Width="50" MaxLength="10" Text="0" /></td>
                                    <td class="footer">The lowest CPU value available for this processor pool.</td>
                                </tr>
                                <tr>
                                    <td align="center" nowrap>High</td>
                                    <td><asp:TextBox ID="txtProcessorHigh" runat="server" CssClass="default" Width="50" MaxLength="10" Text="0" /></td>
                                    <td class="footer">The highest CPU value available for this processor pool.</td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td nowrap valign="top"></td>
                        <td width="100%">
                            <table cellpadding="5" cellspacing="0" border="0" style="border:solid 1px #CCCCCC">
                                <tr bgcolor="#EEEEEE">
                                    <td colspan="3"><b>Thresholds</b></td>
                                </tr>
                                <tr>
                                    <td align="center" nowrap><img src="/images/alert.gif" border="0" align="absmiddle" /> Warning</td>
                                    <td><asp:TextBox ID="txtProcessorWarning" runat="server" CssClass="default" Width="50" MaxLength="10" Text="0" /></td>
                                    <td class="footer">Once the total amount of CPU's requested is greater than the WARNING threshold, email notifications are sent and a WARNING message appears.</td>
                                </tr>
                                <tr>
                                    <td align="center" nowrap><img src="/images/red_alert.gif" border="0" align="absmiddle" /> Critical</td>
                                    <td><asp:TextBox ID="txtProcessorCritical" runat="server" CssClass="default" Width="50" MaxLength="10" Text="0" /></td>
                                    <td class="footer">Once the total amount of CPU's requested is greater than the CRITICAL threshold, email notifications are sent and a CRITICAL message appears.</td>
                                </tr>
                                <tr>
                                    <td align="center" nowrap><img src="/images/error.gif" border="0" align="absmiddle" /> Error</td>
                                    <td><asp:TextBox ID="txtProcessorError" runat="server" CssClass="default" Width="50" MaxLength="10" Text="0" /></td>
                                    <td class="footer">Once the total amount of CPU's requested is greater than the ERROR threshold, the process is halted.</td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td nowrap></td>
                        <td width="100%"><asp:Button ID="btnProcessorAdd" runat="server" CssClass="default" Width="100" Text="Add" OnClick="btnProcessorAdd_Click" Visible="false" /><asp:Button ID="btnProcessorUpdate" runat="server" CssClass="default" Width="100" Text="Update" OnClick="btnProcessorUpdate_Click" Visible="false" /> <asp:Button ID="btnProcessorDelete" runat="server" CssClass="default" Width="100" Text="Delete" OnClick="btnProcessorDelete_Click" Visible="false" /> <asp:Button ID="btnProcessorCancel" runat="server" CssClass="default" Width="100" Text="Cancel" OnClick="btnProcessorCancel_Click" /></td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="panMnemonic" runat="server" Visible="false">
                <table width="100%" cellpadding="4" cellspacing="3" border="0">
                    <tr>
                        <td nowrap>Cluster:</td>
                        <td width="100%"><asp:Label ID="lblMnemonicCluster" runat="server" CssClass="default" /></td>
                    </tr>
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
            <asp:Panel ID="panGuests" runat="server" Visible="false">
                <table width="100%" cellpadding="4" cellspacing="3" border="0">
                    <tr>
                        <td nowrap>Cluster:</td>
                        <td width="100%"><asp:Label ID="lblGuestsCluster" runat="server" CssClass="default" /></td>
                    </tr>
                    <tr>
                        <td nowrap valign="top"></td>
                        <td width="100%">
                            <asp:Table ID="tblGuests" runat="server" CellPadding="3" CellSpacing="3">
                                <asp:TableRow BackColor="#EEEEEE">
                                    <asp:TableCell Text='<b>Selected Guest (s)</b>'></asp:TableCell>
                                </asp:TableRow>
                            </asp:Table>
                        </td>
                    </tr>
                    <tr>
                        <td nowrap>Move To:</td>
                        <td width="100%"><asp:DropDownList ID="ddlCluster" runat="server" CssClass="default" Width="400" /></td>
                    </tr>
                    <tr>
                        <td nowrap></td>
                        <td width="100%"><asp:Button ID="btnGuestMove" runat="server" CssClass="default" Width="100" Text="Move" OnClick="btnGuestMove_Click" /> <asp:Button ID="btnGuestCancel" runat="server" CssClass="default" Width="100" Text="Cancel" OnClick="btnGuestCancel_Click" /></td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="panGuest" runat="server" Visible="false">
                <table width="100%" cellpadding="4" cellspacing="3" border="0">
                    <tr>
                        <td nowrap>Cluster:</td>
                        <td colspan="2" width="100%"><asp:Label ID="lblGuestCluster" runat="server" CssClass="default" /></td>
                    </tr>
                    <tr>
                        <td nowrap>Guest:</td>
                        <td colspan="2" width="100%"><asp:Label ID="lblGuest" runat="server" CssClass="default" /></td>
                    </tr>
                    <tr>
                        <td nowrap>Processor Pool:</td>
                        <td colspan="2" width="100%"><asp:DropDownList ID="ddlProcessorPool" runat="server" CssClass="default" Width="200" /></td>
                    </tr>
                    <tr>
                        <td nowrap>Storage Allocated:</td>
                        <td nowrap><asp:TextBox ID="txtGuestAllocated" runat="server" CssClass="default" Width="100" MaxLength="10" /> GB</td>
                        <td width="100%">** Original Request = <asp:Label ID="lblGuestAllocated" runat="server" CssClass="default" /> GB</td>
                    </tr>
                    <tr>
                        <td nowrap>Amount of RAM:</td>
                        <td nowrap><asp:TextBox ID="txtGuestRAM" runat="server" CssClass="default" Width="50" MaxLength="5" /> GB</td>
                        <td width="100%">** Original Request = <asp:Label ID="lblGuestRAM" runat="server" CssClass="default" /> GB</td>
                    </tr>
                    <tr>
                        <td nowrap>Number of CPU's:</td>
                        <td nowrap><asp:TextBox ID="txtGuestCPU" runat="server" CssClass="default" Width="50" MaxLength="5" />&nbsp;Core(s)</td>
                        <td width="100%">** Original Request = <asp:Label ID="lblGuestCPU" runat="server" CssClass="default" /> Core(s)</td>
                    </tr>
                    <tr>
                        <td nowrap></td>
                        <td colspan="2" width="100%"><asp:Button ID="btnGuestUpdate" runat="server" CssClass="default" Width="100" Text="Update" OnClick="btnGuestUpdate_Click" /> <asp:Button ID="btnGuestCancel2" runat="server" CssClass="default" Width="100" Text="Cancel" OnClick="btnGuestCancel_Click" /></td>
                    </tr>
                </table>
            </asp:Panel>
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
<asp:HiddenField ID="hdnMove" runat="server" />
<asp:HiddenField ID="hdnHost" runat="server" />
<asp:HiddenField ID="hdnMnemonic" runat="server" />
<asp:HiddenField ID="hdnSchedule" runat="server" />
<asp:HiddenField ID="hdnLocation" runat="server" />
