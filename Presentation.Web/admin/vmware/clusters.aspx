<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="clusters.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.clusters" %>
<script type="text/javascript">
</script>
<html>
<head>
<title>ClearView Administration</title>
<link rel="stylesheet" type="text/css" href="/css/default.css" />
<script type="text/javascript" src="/javascript/global.js"></script>
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
		    <td><b>Clusters</b></td>
		    <td align="right"></td>
		</tr>
		<tr><td colspan="2">&nbsp;</td></tr>
		<tr>
		    <td colspan="2" align="center">
                <asp:Panel ID="panView" runat="server" Visible="false">
                    <table width="100%" border="0" cellpadding="0" cellspacing="0">
                        <tr>
                            <td>
                            <asp:TreeView ID="oTreeview" runat="server" ShowLines="true" NodeIndent="35">
                                <NodeStyle CssClass="default" />
                            </asp:TreeView>
                            </td>
                        </tr>
                    </table>
               </asp:Panel>

                <asp:Panel id="panAdd" runat="server" Visible="false">
                    <table width="95%" cellpadding="3" cellspacing="0" border="0" align="center">
                        <tr> 
                            <td class="default" width="100px">Folder:</td>
                            <td><asp:DropDownList ID="ddlParent" CssClass="default" runat="server"/></td>
                        </tr>
                        <tr>
                            <td nowrap class="cmdefault">Model:</td>
                            <td width="100%"><asp:label ID="lblModel" CssClass="default" runat="server" />&nbsp;&nbsp;<asp:Button ID="btnModel" runat="server" CssClass="default" Text="..." /></td>
                        </tr>
                        <tr> 
                            <td class="default" width="100px">Name:</td>
                            <td><asp:textbox ID="txtName" CssClass="default" runat="server" Width="200" MaxLength="50"/>&nbsp;&nbsp;&nbsp;<span class="reddefault"><b>NOTE:</b> This field IS CASE SENSITIVE.</span></td>
                        </tr>
                        <tr> 
                            <td class="default" width="100px">Version:</td>
                            <td>
                                <asp:DropDownList ID="ddlVersion" CssClass="default" runat="server" >
                                    <asp:ListItem Value="0" Text="Virtual Center 3.5" />
                                    <asp:ListItem Value="1" Text="Virtual Center 4 (Vsphere)" />
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr> 
                            <td class="default" width="100px">Anti-Affinity:</td>
                            <td>
                                <asp:DropDownList ID="ddlAntiAffinity" CssClass="default" runat="server" >
                                    <asp:ListItem Value="0" Text="VMWARE UPDATE 3 - Individual" />
                                    <asp:ListItem Value="1" Text="VMWARE UPDATE 4 - All at Once" />
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr> 
                            <td class="default" width="100px">Maximum Guests:</td>
                            <td><asp:textbox ID="txtMaximum" CssClass="default" runat="server" Width="100" MaxLength="10"/></td>
                        </tr>
                        <tr> 
                            <td class="default" width="100px">Resource Pool:</td>
                            <td><asp:textbox ID="txtResourcePool" CssClass="default" runat="server" Width="200" MaxLength="50"/></td>
                        </tr>
                        <tr> 
                            <td class="default" width="100px">Datastore Notification:</td>
                            <td>Notify <asp:textbox ID="txtDatastoreNotify" CssClass="default" runat="server" Width="300" MaxLength="100"/> when <asp:textbox ID="txtDatastoreLeft" CssClass="default" runat="server" Width="75" MaxLength="10"/> datastores are less than <asp:textbox ID="txtDatastoreSize" CssClass="default" runat="server" Width="75" MaxLength="10"/> GB</td>
                        </tr>
                        <tr> 
                            <td class="default" width="100px"></td>
                            <td class="reddefault"><img src="/images/spacer.gif" border="0" width="50" height="1" /><img src="/images/down_right.gif" border="0" align="absmiddle" /><b>NOTE:</b> For the notifications, please use LAN ID with a semi-colon separating the users</td>
                        </tr>
                        <tr> 
                            <td class="default">PNC:</td>
                            <td><asp:CheckBox ID="chkPNC" runat="server" Checked="true" /></td>
                        </tr>
                        <tr> 
                            <td class="default">Full:</td>
                            <td><asp:CheckBox ID="chkFull" runat="server" Checked="true" /></td>
                        </tr>
                        <tr> 
                            <td class="default">Auto-Provisioning Off:</td>
                            <td><asp:CheckBox ID="chkAPoff" runat="server" Checked="true" /></td>
                        </tr>
                        <tr> 
                            <td class="default">Auto-Provisioning Off (DR):</td>
                            <td><asp:CheckBox ID="chkAPoffDR" runat="server" Checked="true" /></td>
                        </tr>
                        <tr> 
                            <td class="default">DELL:</td>
                            <td><asp:CheckBox ID="chkDell" runat="server" /></td>
                        </tr>
                        <tr>
                            <td nowrap>Resiliency:</td>
                            <td><asp:DropDownList ID="ddlResiliency" runat="server" CssClass="default" Width="400" /></td>
                        </tr>
                        <tr> 
                            <td class="default">SQL / Oracle / Cluster:</td>
                            <td>
                                <asp:CheckBox ID="chkOracle" runat="server" Text="Oracle (non-cluster)" /><br />
                                <asp:CheckBox ID="chkOracleCluster" runat="server" Text="Oracle Cluster" /><br />
                                <asp:CheckBox ID="chkSQL" runat="server" Text="SQL (non-cluster)" /><br />
                                <asp:CheckBox ID="chkSQLCluster" runat="server" Text="SQL Cluster" /><br />
                                <asp:CheckBox ID="chkCluster" runat="server" Text="non-Oracle & non-SQL Clustering" /><br />
                            </td>
                        </tr>
                        <tr> 
                            <td class="default">Enabled:</td>
                            <td><asp:CheckBox ID="chkEnabled" runat="server" Checked="true" /></td>
                        </tr>
                        <tr> 
                            <td class="cmdefault">&nbsp;</td>
                            <td><asp:Button ID="btnOS" runat="server" Text="Configure Operating Systems" Width="200" CssClass="default" /></td>
                        </tr>
                        <tr>
                            <td colspan="2">&nbsp;</td>
                        </tr>
                        <tr> 
                            <td colspan="2" bgcolor="#F6F6F6" style="border:solid 1px #CCCCCC">
                                <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                    <tr>
                                        <td colspan="2" class="header"><asp:CheckBox ID="chkQuery" runat="server" CssClass="default" Text="Query Capacity Calculations" OnCheckedChanged="chkQuery_Change" AutoPostBack="true" /></td>
                                    </tr>
                                    <tr id="trCapacity" runat="server" visible="false">
                                        <td width="50%" valign="top">
                                            <table width="100%" cellpadding="2" cellspacing="2" border="0">
                                                <tr><td colspan="2">&nbsp;</td></tr>
                                                <tr>
                                                    <td colspan="2"><b>ESX Cluster Variables</b></td>
                                                </tr>
                                                <tr>
                                                    <td nowrap><asp:TextBox ID="txtServers" runat="server" CssClass="default" Width="75" MaxLength="10" ReadOnly="true" /></td>
                                                    <td width="100%">Current number of ESX servers in the cluster (...from Virtual Center)</td>
                                                </tr>
                                                <tr>
                                                    <td nowrap><asp:TextBox ID="txtFailures" runat="server" CssClass="default" Width="75" MaxLength="10" /></td>
                                                    <td width="100%" class="required"><img src="/images/arrow_green_left.gif" border="0" align="absmiddle" /> Number of ESX server failures to support (0 - 4)</td>
                                                </tr>
                                                <tr>
                                                    <td nowrap><asp:TextBox ID="txtCPUUtilization" runat="server" CssClass="default" Width="75" MaxLength="10" /></td>
                                                    <td width="100%" class="required"><img src="/images/arrow_green_left.gif" border="0" align="absmiddle" /> Target average % CPU utilization (0 - 100)</td>
                                                </tr>
                                                <tr>
                                                    <td nowrap><asp:TextBox ID="txtRAMUtilization" runat="server" CssClass="default" Width="75" MaxLength="10" /></td>
                                                    <td width="100%" class="required"><img src="/images/arrow_green_left.gif" border="0" align="absmiddle" /> Target average % RAM utilization (0 - 100)</td>
                                                </tr>
                                                <tr><td colspan="2">&nbsp;</td></tr>
                                                <tr>
                                                    <td colspan="2"><b>Base Virtual Machine Variables</b></td>
                                                </tr>
                                                <tr>
                                                    <td nowrap><asp:TextBox ID="txtMaxRAM" runat="server" CssClass="default" Width="75" MaxLength="10" /></td>
                                                    <td width="100%" class="required"><img src="/images/arrow_green_left.gif" border="0" align="absmiddle" /> Maximum RAM allocated (in GB)</td>
                                                </tr>
                                                <tr>
                                                    <td nowrap><asp:TextBox ID="txtMaxCPU" runat="server" CssClass="default" Width="75" MaxLength="10" ReadOnly="true" /></td>
                                                    <td width="100%">Maximum CPU allocated (in GB)</td>
                                                </tr>
                                                <tr>
                                                    <td nowrap><asp:TextBox ID="txtAvgUtilization" runat="server" CssClass="default" Width="75" MaxLength="10" /></td>
                                                    <td width="100%" class="required"><img src="/images/arrow_green_left.gif" border="0" align="absmiddle" /> Average Disk Utilization (in GB)</td>
                                                </tr>
                                                <tr><td colspan="2">&nbsp;</td></tr>
                                                <tr>
                                                    <td colspan="2"><b>ESX Server Variables</b></td>
                                                </tr>
                                                <tr>
                                                    <td nowrap><asp:TextBox ID="txtRAM" runat="server" CssClass="default" Width="75" MaxLength="10" ReadOnly="true" /></td>
                                                    <td width="100%">RAM (in GB)</td>
                                                </tr>
                                                <tr>
                                                    <td nowrap><asp:TextBox ID="txtCores" runat="server" CssClass="default" Width="75" MaxLength="10" ReadOnly="true" /></td>
                                                    <td width="100%">Number of Cores per Socket</td>
                                                </tr>
                                                <tr>
                                                    <td nowrap><asp:TextBox ID="txtCPUs" runat="server" CssClass="default" Width="75" MaxLength="10" ReadOnly="true" /></td>
                                                    <td width="100%">Number of CPU sockets</td>
                                                </tr>
                                                <tr>
                                                    <td nowrap><asp:TextBox ID="txtProcs" runat="server" CssClass="default" Width="75" MaxLength="10" ReadOnly="true" /></td>
                                                    <td width="100%">Processor Core Speed (in GHz)</td>
                                                </tr>
                                                <tr>
                                                    <td nowrap><asp:TextBox ID="txtTotal" runat="server" CssClass="default" Width="75" MaxLength="10" ReadOnly="true" /></td>
                                                    <td width="100%">ESX Total (in GHz)</td>
                                                </tr>
                                                <tr><td colspan="2">&nbsp;</td></tr>
                                                <tr>
                                                    <td colspan="2"><b>ESX Cluster LUN Variables</b></td>
                                                </tr>
                                                <tr>
                                                    <td nowrap><asp:TextBox ID="txtLunSize" runat="server" CssClass="default" Width="75" MaxLength="10" /></td>
                                                    <td width="100%" class="required"><img src="/images/arrow_green_left.gif" border="0" align="absmiddle" /> LUN Size (in GB)</td>
                                                </tr>
                                                <tr>
                                                    <td nowrap><asp:TextBox ID="txtLunUtilization" runat="server" CssClass="default" Width="75" MaxLength="10" /></td>
                                                    <td width="100%" class="required"><img src="/images/arrow_green_left.gif" border="0" align="absmiddle" /> Target LUN % utilization (0 - 100)</td>
                                                </tr>
                                                <tr>
                                                    <td nowrap><asp:TextBox ID="txtVMsPerLun" runat="server" CssClass="default" Width="75" MaxLength="10" /></td>
                                                    <td width="100%" class="required"><img src="/images/arrow_green_left.gif" border="0" align="absmiddle" /> VMs per LUN</td>
                                                </tr>
                                                <tr><td colspan="2">&nbsp;</td></tr>
                                                <tr>
                                                    <td colspan="2"><b>VI Component Provisioning Timelines</b></td>
                                                </tr>
                                                <tr>
                                                    <td nowrap><asp:TextBox ID="txtTimeLUN" runat="server" CssClass="default" Width="75" MaxLength="10" /></td>
                                                    <td width="100%" class="required"><img src="/images/arrow_green_left.gif" border="0" align="absmiddle" /> Time to provision LUN (days)</td>
                                                </tr>
                                                <tr>
                                                    <td nowrap><asp:TextBox ID="txtTimeCluster" runat="server" CssClass="default" Width="75" MaxLength="10" /></td>
                                                    <td width="100%" class="required"><img src="/images/arrow_green_left.gif" border="0" align="absmiddle" /> Time to provision ESX cluster (days)</td>
                                                </tr>
                                                <tr><td colspan="2">&nbsp;</td></tr>
                                                <tr>
                                                    <td colspan="2"><b>Capacity Reserve Variables (SLO)</b></td>
                                                </tr>
                                                <tr>
                                                    <td nowrap><asp:TextBox ID="txtMaxVMsServer" runat="server" CssClass="default" Width="75" MaxLength="10" /></td>
                                                    <td width="100%" class="required"><img src="/images/arrow_green_left.gif" border="0" align="absmiddle" /> Max VMs per day (ESX Server) VMPR</td>
                                                </tr>
                                                <tr>
                                                    <td nowrap><asp:TextBox ID="txtMaxVMsLUN" runat="server" CssClass="default" Width="75" MaxLength="10" /></td>
                                                    <td width="100%" class="required"><img src="/images/arrow_green_left.gif" border="0" align="absmiddle" /> Max VMs per day (LUN)</td>
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
                                                    <td nowrap><asp:TextBox ID="txtTotalCPU" runat="server" CssClass="default" Width="75" MaxLength="10" ReadOnly="true" /></td>
                                                    <td width="100%">Total CPU (in GHz)</td>
                                                </tr>
                                                <tr>
                                                    <td nowrap><asp:TextBox ID="txtTotalRAM" runat="server" CssClass="default" Width="75" MaxLength="10" ReadOnly="true" /></td>
                                                    <td width="100%">Total RAM (in GB)</td>
                                                </tr>
                                                <tr><td colspan="2">&nbsp;</td></tr>
                                                <tr>
                                                    <td colspan="2"><b>Capacity Reserves</b></td>
                                                </tr>
                                                <tr>
                                                    <td nowrap><asp:TextBox ID="txtReserveCPU" runat="server" CssClass="default" Width="75" MaxLength="10" ReadOnly="true" /></td>
                                                    <td width="100%">Reserve CPU (in GHz)</td>
                                                </tr>
                                                <tr>
                                                    <td nowrap><asp:TextBox ID="txtReserveRAM" runat="server" CssClass="default" Width="75" MaxLength="10" ReadOnly="true" /></td>
                                                    <td width="100%">Reserve RAM (in GB)</td>
                                                </tr>
                                                <tr>
                                                    <td nowrap><asp:TextBox ID="txtReserveDisk" runat="server" CssClass="default" Width="75" MaxLength="10" ReadOnly="true" /></td>
                                                    <td width="100%">Reserve Disk (in GB)</td>
                                                </tr>
                                                <tr><td colspan="2">&nbsp;</td></tr>
                                                <tr>
                                                    <td colspan="2"><b>Expansion Triggers</b></td>
                                                </tr>
                                                <tr>
                                                    <td nowrap><asp:TextBox ID="txtExpandCPU" runat="server" CssClass="default" Width="75" MaxLength="10" ReadOnly="true" /></td>
                                                    <td width="100%">CPU Expansion % Trigger (in GHz)</td>
                                                </tr>
                                                <tr>
                                                    <td nowrap><asp:TextBox ID="txtExpandRAM" runat="server" CssClass="default" Width="75" MaxLength="10" ReadOnly="true" /></td>
                                                    <td width="100%">Memory Expansion % Trigger (in GB)</td>
                                                </tr>
                                                <tr>
                                                    <td nowrap><asp:TextBox ID="txtExpandDisk" runat="server" CssClass="default" Width="75" MaxLength="10" ReadOnly="true" /></td>
                                                    <td width="100%">Disk Expansion % Trigger (in GB)</td>
                                                </tr>
                                                <tr><td colspan="2">&nbsp;</td></tr>
                                                <tr><td colspan="2">&nbsp;</td></tr>
                                                <tr>
                                                    <td nowrap><asp:TextBox ID="txtCurrentCPU" runat="server" CssClass="default" Width="75" MaxLength="10" ReadOnly="true" /></td>
                                                    <td width="100%">Current CPU from Virtual Center (in GHz) (...from Virtual Center)</td>
                                                </tr>
                                                <tr>
                                                    <td nowrap><asp:TextBox ID="txtCurrentRAM" runat="server" CssClass="default" Width="75" MaxLength="10" ReadOnly="true" /></td>
                                                    <td width="100%">Current RAM from Virtual Center (in GB) (...from Virtual Center)</td>
                                                </tr>
                                                <tr><td colspan="2">&nbsp;</td></tr>
                                                <tr><td colspan="2">&nbsp;</td></tr>
                                                <tr>
                                                    <td colspan="2" class="required"><img src="/images/alert.gif" border="0" align="absmiddle" /> <b>NOTE:</b> Only items in RED and marked with <img src="/images/arrow_green_left.gif" border="0" align="absmiddle" /> are editable!</td>
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
    <input type="hidden" id="hdnModel" runat="server" />
</form>
</body>
</html>
