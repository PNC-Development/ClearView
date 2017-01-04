<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="shared_threshold.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.shared_threshold" %>
<table width="100%" height="100%" cellpadding="5" cellspacing="5" border="0">
    <tr>
        <td class="header">Thresholds</td>
    </tr>
    <tr>
        <td>
            <asp:Panel ID="panF" runat="server" Visible="false">
                <table cellpadding="3" cellspacing="0" border="0">
                    <tr> 
                        <td class="default">Datacenter:</td>
                        <td><asp:Label ID="lblFParent" CssClass="default" runat="server"/></td>
                    </tr>
                    <tr> 
                        <td class="default">Folder Name:</td>
                        <td><asp:Label ID="lblFName" CssClass="default" runat="server"/></td>
                    </tr>
                    <tr> 
                        <td class="default">Notification:</td>
                        <td>Notify <asp:textbox ID="txtFNotification" CssClass="default" runat="server" Width="300" MaxLength="100"/> when a cluster has reached 8 hosts and a new cluster should be provisioned</td>
                    </tr>
                    <tr> 
                        <td class="default"></td>
                        <td class="reddefault"><img src="/images/spacer.gif" border="0" width="50" height="1" /><img src="/images/down_right.gif" border="0" align="absmiddle" /><b>NOTE:</b> For the notifications, please use LAN ID with a semi-colon separating the users</td>
                    </tr>
                    <tr> 
                        <td>&nbsp;</td>
                        <td>
                            <asp:button ID="btnUpdateF" CssClass="default" runat="server" Text="Add" Width="75" OnClick="btnUpdateF_Click" OnClientClick="ProcessButtons(this) && LoadWait();" />
                        </td>
                    </tr>
                </table>
                <asp:Label ID="chkFEnabled" runat="server" Visible="false" />
            </asp:Panel>

            <asp:Panel ID="panC" runat="server" Visible="false">
                <table cellpadding="3" cellspacing="0" border="0">
                    <tr> 
                        <td class="default">Folder:</td>
                        <td><asp:Label ID="lblCParent" CssClass="default" runat="server"/></td>
                    </tr>
                    <tr> 
                        <td class="default">Cluster Name:</td>
                        <td><asp:Label ID="lblCName" CssClass="default" runat="server"/></td>
                    </tr>
                    <tr> 
                        <td class="default">Maximum Guests:</td>
                        <td><asp:textbox ID="txtCMaximum" CssClass="default" runat="server" Width="100" MaxLength="10"/></td>
                    </tr>
                    <tr> 
                        <td class="default">Notification:</td>
                        <td>Notify <asp:textbox ID="txtDatastoreNotify" CssClass="default" runat="server" Width="300" MaxLength="100"/> when <asp:textbox ID="txtDatastoreLeft" CssClass="default" runat="server" Width="75" MaxLength="10"/> datastores are less than <asp:textbox ID="txtDatastoreSize" CssClass="default" runat="server" Width="75" MaxLength="10"/> GB</td>
                    </tr>
                    <tr> 
                        <td class="default"></td>
                        <td class="reddefault"><img src="/images/spacer.gif" border="0" width="50" height="1" /><img src="/images/down_right.gif" border="0" align="absmiddle" /><b>NOTE:</b> For the notifications, please use LAN ID with a semi-colon separating the users</td>
                    </tr>
                    <tr><td colspan="2">&nbsp;</td></tr>
                    <tr> 
                        <td class="default"></td>
                        <td>
                            <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                <tr>
                                    <td width="50%" valign="top">
                                        <table width="100%" cellpadding="2" cellspacing="2" border="0">
                                            <tr>
                                                <td colspan="2"><b>ESX Cluster Variables</b></td>
                                            </tr>
                                            <tr>
                                                <td nowrap><asp:TextBox ID="txtServers" runat="server" CssClass="readonly" Width="75" MaxLength="10" ReadOnly="true" /></td>
                                                <td width="100%">Current number of ESX servers in the cluster (...from Virtual Center)</td>
                                            </tr>
                                            <tr>
                                                <td nowrap><asp:TextBox ID="txtFailures" runat="server" CssClass="default" Width="75" MaxLength="10" /></td>
                                                <td width="100%">Number of ESX server failures to support (0 - 4)</td>
                                            </tr>
                                            <tr>
                                                <td nowrap><asp:TextBox ID="txtCPUUtilization" runat="server" CssClass="default" Width="75" MaxLength="10" /></td>
                                                <td width="100%">Target average % CPU utilization (0 - 100)</td>
                                            </tr>
                                            <tr>
                                                <td nowrap><asp:TextBox ID="txtRAMUtilization" runat="server" CssClass="default" Width="75" MaxLength="10" /></td>
                                                <td width="100%">Target average % RAM utilization (0 - 100)</td>
                                            </tr>
                                            <tr><td colspan="2">&nbsp;</td></tr>
                                            <tr>
                                                <td colspan="2"><b>Base Virtual Machine Variables</b></td>
                                            </tr>
                                            <tr>
                                                <td nowrap><asp:TextBox ID="txtMaxRAM" runat="server" CssClass="default" Width="75" MaxLength="10" /></td>
                                                <td width="100%">Maximum RAM allocated (in GB)</td>
                                            </tr>
                                            <tr>
                                                <td nowrap><asp:TextBox ID="txtMaxCPU" runat="server" CssClass="readonly" Width="75" MaxLength="10" ReadOnly="true" /></td>
                                                <td width="100%">Maximum CPU allocated (in GB)</td>
                                            </tr>
                                            <tr>
                                                <td nowrap><asp:TextBox ID="txtAvgUtilization" runat="server" CssClass="default" Width="75" MaxLength="10" /></td>
                                                <td width="100%">Average Disk Utilization (in GB)</td>
                                            </tr>
                                            <tr><td colspan="2">&nbsp;</td></tr>
                                            <tr>
                                                <td colspan="2"><b>ESX Server Variables</b></td>
                                            </tr>
                                            <tr>
                                                <td nowrap><asp:TextBox ID="txtRAM" runat="server" CssClass="readonly" Width="75" MaxLength="10" ReadOnly="true" /></td>
                                                <td width="100%">RAM (in GB)</td>
                                            </tr>
                                            <tr>
                                                <td nowrap><asp:TextBox ID="txtCores" runat="server" CssClass="readonly" Width="75" MaxLength="10" ReadOnly="true" /></td>
                                                <td width="100%">Number of Cores per Socket</td>
                                            </tr>
                                            <tr>
                                                <td nowrap><asp:TextBox ID="txtCPUs" runat="server" CssClass="readonly" Width="75" MaxLength="10" ReadOnly="true" /></td>
                                                <td width="100%">Number of CPU sockets</td>
                                            </tr>
                                            <tr>
                                                <td nowrap><asp:TextBox ID="txtProcs" runat="server" CssClass="readonly" Width="75" MaxLength="10" ReadOnly="true" /></td>
                                                <td width="100%">Processor Core Speed (in GHz)</td>
                                            </tr>
                                            <tr>
                                                <td nowrap><asp:TextBox ID="txtTotal" runat="server" CssClass="readonly" Width="75" MaxLength="10" ReadOnly="true" /></td>
                                                <td width="100%">ESX Total (in GHz)</td>
                                            </tr>
                                            <tr><td colspan="2">&nbsp;</td></tr>
                                            <tr>
                                                <td colspan="2"><b>ESX Cluster LUN Variables</b></td>
                                            </tr>
                                            <tr>
                                                <td nowrap><asp:TextBox ID="txtLunSize" runat="server" CssClass="default" Width="75" MaxLength="10" /></td>
                                                <td width="100%">LUN Size (in GB)</td>
                                            </tr>
                                            <tr>
                                                <td nowrap><asp:TextBox ID="txtLunUtilization" runat="server" CssClass="default" Width="75" MaxLength="10" /></td>
                                                <td width="100%">Target LUN % utilization (0 - 100)</td>
                                            </tr>
                                            <tr>
                                                <td nowrap><asp:TextBox ID="txtVMsPerLun" runat="server" CssClass="default" Width="75" MaxLength="10" /></td>
                                                <td width="100%">VMs per LUN</td>
                                            </tr>
                                            <tr><td colspan="2">&nbsp;</td></tr>
                                            <tr>
                                                <td colspan="2"><b>VI Component Provisioning Timelines</b></td>
                                            </tr>
                                            <tr>
                                                <td nowrap><asp:TextBox ID="txtTimeLUN" runat="server" CssClass="default" Width="75" MaxLength="10" /></td>
                                                <td width="100%">Time to provision LUN (days)</td>
                                            </tr>
                                            <tr>
                                                <td nowrap><asp:TextBox ID="txtTimeCluster" runat="server" CssClass="default" Width="75" MaxLength="10" /></td>
                                                <td width="100%">Time to provision ESX cluster (days)</td>
                                            </tr>
                                            <tr><td colspan="2">&nbsp;</td></tr>
                                            <tr>
                                                <td colspan="2"><b>Capacity Reserve Variables (SLO)</b></td>
                                            </tr>
                                            <tr>
                                                <td nowrap><asp:TextBox ID="txtMaxVMsServer" runat="server" CssClass="default" Width="75" MaxLength="10" /></td>
                                                <td width="100%">Max VMs per day (ESX Server) VMPR</td>
                                            </tr>
                                            <tr>
                                                <td nowrap><asp:TextBox ID="txtMaxVMsLUN" runat="server" CssClass="default" Width="75" MaxLength="10" /></td>
                                                <td width="100%">Max VMs per day (LUN)</td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td width="50%" valign="top">
                                        <table width="100%" cellpadding="2" cellspacing="2" border="0">
                                            <tr><td colspan="2">&nbsp;</td></tr>
                                            <tr>
                                                <td colspan="2"><b>Total ESX factored Capacity</b></td>
                                            </tr>
                                            <tr>
                                                <td nowrap><asp:TextBox ID="txtTotalCPU" runat="server" CssClass="readonly" Width="75" MaxLength="10" ReadOnly="true" /></td>
                                                <td width="100%">Total CPU (in GHz)</td>
                                            </tr>
                                            <tr>
                                                <td nowrap><asp:TextBox ID="txtTotalRAM" runat="server" CssClass="readonly" Width="75" MaxLength="10" ReadOnly="true" /></td>
                                                <td width="100%">Total RAM (in GB)</td>
                                            </tr>
                                            <tr><td colspan="2">&nbsp;</td></tr>
                                            <tr>
                                                <td colspan="2"><b>Capacity Reserves</b></td>
                                            </tr>
                                            <tr>
                                                <td nowrap><asp:TextBox ID="txtReserveCPU" runat="server" CssClass="readonly" Width="75" MaxLength="10" ReadOnly="true" /></td>
                                                <td width="100%">Reserve CPU (in GHz)</td>
                                            </tr>
                                            <tr>
                                                <td nowrap><asp:TextBox ID="txtReserveRAM" runat="server" CssClass="readonly" Width="75" MaxLength="10" ReadOnly="true" /></td>
                                                <td width="100%">Reserve RAM (in GB)</td>
                                            </tr>
                                            <tr>
                                                <td nowrap><asp:TextBox ID="txtReserveDisk" runat="server" CssClass="readonly" Width="75" MaxLength="10" ReadOnly="true" /></td>
                                                <td width="100%">Reserve Disk (in GB)</td>
                                            </tr>
                                            <tr><td colspan="2">&nbsp;</td></tr>
                                            <tr>
                                                <td colspan="2"><b>Expansion Triggers</b></td>
                                            </tr>
                                            <tr>
                                                <td nowrap><asp:TextBox ID="txtExpandCPU" runat="server" CssClass="readonly" Width="75" MaxLength="10" ReadOnly="true" /></td>
                                                <td width="100%">CPU Expansion % Trigger (in GHz)</td>
                                            </tr>
                                            <tr>
                                                <td nowrap><asp:TextBox ID="txtExpandRAM" runat="server" CssClass="readonly" Width="75" MaxLength="10" ReadOnly="true" /></td>
                                                <td width="100%">Memory Expansion % Trigger (in GB)</td>
                                            </tr>
                                            <tr>
                                                <td nowrap><asp:TextBox ID="txtExpandDisk" runat="server" CssClass="readonly" Width="75" MaxLength="10" ReadOnly="true" /></td>
                                                <td width="100%">Disk Expansion % Trigger (in GB)</td>
                                            </tr>
                                            <tr><td colspan="2">&nbsp;</td></tr>
                                            <tr><td colspan="2">&nbsp;</td></tr>
                                            <tr>
                                                <td nowrap><asp:TextBox ID="txtCurrentCPU" runat="server" CssClass="readonly" Width="75" MaxLength="10" ReadOnly="true" /></td>
                                                <td width="100%">Current CPU from Virtual Center (in GHz) (...from Virtual Center)</td>
                                            </tr>
                                            <tr>
                                                <td nowrap><asp:TextBox ID="txtCurrentRAM" runat="server" CssClass="readonly" Width="75" MaxLength="10" ReadOnly="true" /></td>
                                                <td width="100%">Current RAM from Virtual Center (in GB) (...from Virtual Center)</td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr><td height="5" colspan="2">&nbsp;</td></tr>
                    <tr> 
                        <td>&nbsp;</td>
                        <td>
                            <asp:button ID="btnUpdateC" CssClass="default" runat="server" Text="Add" Width="75" OnClick="btnUpdateC_Click" OnClientClick="ProcessButtons(this) && LoadWait();" />
                        </td>
                    </tr>
                </table>
                <asp:Label ID="ddlVersion" runat="server" Visible="false" />
                <asp:Label ID="ddlAntiAffinity" runat="server" Visible="false" />
                <asp:Label ID="txtResourcePool" runat="server" Visible="false" />
                <asp:Label ID="chkFull" runat="server" Visible="false" />
                <asp:Label ID="chkAPoff" runat="server" Visible="false" />
                <asp:Label ID="chkAPoffDR" runat="server" Visible="false" />
                <asp:Label ID="chkDell" runat="server" Visible="false" />
                <asp:Label ID="ddlResiliency" runat="server" Visible="false" />
                <asp:Label ID="chkOracle" runat="server" Visible="false" />
                <asp:Label ID="chkOracleCluster" runat="server" Visible="false" />
                <asp:Label ID="chkSQL" runat="server" Visible="false" />
                <asp:Label ID="chkSQLCluster" runat="server" Visible="false" />
                <asp:Label ID="chkCluster" runat="server" Visible="false" />
                <asp:Label ID="chkCEnabled" runat="server" Visible="false" />
            </asp:Panel>

            <asp:Panel ID="panH" runat="server" Visible="false">
                <table cellpadding="3" cellspacing="0" border="0">
                    <tr> 
                        <td class="default">Cluster:</td>
                        <td><asp:Label ID="lblHParent" CssClass="default" runat="server"/></td>
                    </tr>
                    <tr> 
                        <td class="default">Host Name:</td>
                        <td><asp:Label ID="lblHName" CssClass="default" runat="server"/></td>
                    </tr>
                    <tr> 
                        <td class="default">Maximum Guests:</td>
                        <td><asp:textbox ID="txtHMaximum" CssClass="default" runat="server" Width="100" MaxLength="10"/></td>
                    </tr>
                    <tr><td height="5" colspan="2">&nbsp;</td></tr>
                    <tr> 
                        <td>&nbsp;</td>
                        <td>
                            <asp:button ID="btnUpdateH" CssClass="default" runat="server" Text="Add" Width="75" OnClick="btnUpdateH_Click" OnClientClick="ProcessButtons(this) && LoadWait();" />
                        </td>
                    </tr>
                </table>
                <asp:Label ID="chkHEnabled" runat="server" Visible="false" />
            </asp:Panel>

            <asp:Panel ID="panDS" runat="server" Visible="false">
                <table cellpadding="3" cellspacing="0" border="0">
                    <tr> 
                        <td class="default">Cluster:</td>
                        <td><asp:Label ID="lblDSParent" CssClass="default" runat="server"/></td>
                    </tr>
                    <tr> 
                        <td class="default">Datastore Name:</td>
                        <td><asp:Label ID="lblDSName" CssClass="default" runat="server"/></td>
                    </tr>
                    <tr> 
                        <td class="default">Maximum Guests:</td>
                        <td><asp:textbox ID="txtDSMaximum" CssClass="default" runat="server" Width="100" MaxLength="10"/></td>
                    </tr>
                    <tr><td height="5" colspan="2">&nbsp;</td></tr>
                    <tr> 
                        <td>&nbsp;</td>
                        <td>
                            <asp:button ID="btnUpdateDS" CssClass="default" runat="server" Text="Add" Width="75" OnClick="btnUpdateDS_Click" OnClientClick="ProcessButtons(this) && LoadWait();" />
                        </td>
                    </tr>
                </table>
                <asp:Label ID="ddlType" runat="server" Visible="false" />
                <asp:Label ID="ddlOperatingSystemGroup" runat="server" Visible="false" />
                <asp:Label ID="chkReplicated" runat="server" Visible="false" />
                <asp:Label ID="chkServer" runat="server" Visible="false" />
                <asp:Label ID="chkPagefile" runat="server" Visible="false" />
                <asp:Label ID="chkOverridePermission" runat="server" Visible="false" />
                <asp:Label ID="ddlPartner" runat="server" Visible="false" />
                <asp:Label ID="chkDSEnabled" runat="server" Visible="false" />
            </asp:Panel>

            <asp:Panel ID="panNone" runat="server" Visible="false">
                There are no thresholds available for this view.  Select another option.
            </asp:Panel>
        </td>
    </tr>
</table>
