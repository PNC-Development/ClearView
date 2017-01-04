<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="vmware.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.vmware" %>

<table width="100%" cellpadding="0" cellspacing="5" border="0">
    <tr>
        <td rowspan="2"><img src="/images/tape.gif" border="0" align="absmiddle" /></td>
        <td class="header" width="100%" valign="bottom">VMware Administration</td>
    </tr>
    <tr>
        <td width="100%" valign="top">Use this page to administer VMware virtual center servicers, data centers, folders, clusters, hosts and vlans.</td>
    </tr>
</table>
<table width="100%" cellpadding="0" cellspacing="5" border="0">
    <tr>
        <td><asp:Label ID="lblCrumbs" runat="server" /></td>
    </tr>
</table>
<asp:Label ID="lblMessage" runat="server" />
<asp:Panel ID="panVCs" runat="server" Visible="false">
    <table width="100%" cellpadding="4" cellspacing="0" border="0" align="center" style="border:solid 1px #CCCCCC">
        <tr bgcolor="#EEEEEE">
            <td width="30%" nowrap><b>Virtual Center Server Name</b></td>
            <td width="50%" nowrap><b>Connection to Virtual Center</b></td>
            <td width="20%" nowrap><b>Modified</b></td>
            <td nowrap align="center"><b>Enabled</b></td>
        </tr>
    <asp:repeater ID="rptVCs" runat="server">
        <ItemTemplate>
            <tr class='<%# DataBinder.Eval(Container.DataItem, "enabled").ToString() == "1" ? "default" : "component_unavailable" %>' onmouseover="CellRowOver(this);" onmouseout="CellRowOut(this);" onclick="if(event.srcElement.tagName != 'IMG') { LoadWait() && window.navigate('<%# FormURL("vc=" + DataBinder.Eval(Container.DataItem, "id")) %>'); }">
                <td width="30%"><%# DataBinder.Eval(Container.DataItem, "name") %></td>
                <td width="50%"><%# DataBinder.Eval(Container.DataItem, "url") %></td>      
                <td width="20%" nowrap><%# DataBinder.Eval(Container.DataItem, "modified") %></td>
                <td nowrap align="center"><img src="/images/<%# DataBinder.Eval(Container.DataItem, "enabled").ToString() == "1" ? "check" : "cancel" %>.gif" border="0" align="absmiddle" /></td>
            </tr>
        </ItemTemplate>
    </asp:repeater>
        <tr>
            <td colspan="6"><asp:Label ID="lblVCs" runat="server" CssClass="error" Visible="false" Text="<img src='/images/bigAlert.gif' border='0' align='absmiddle'> There are no virtual center servers" /></td>
        </tr>
    </table>
    <br /><br />
    <asp:Button ID="btnAddVirtualCenter" runat="server" CssClass="default" Width="75" Text="Add New" OnClick="btnAddVirtualCenter_Click" OnClientClick="ProcessButton(this) && LoadWait();" />
</asp:Panel>


<asp:Panel ID="panVC" runat="server" Visible="false">
    <table cellpadding="3" cellspacing="0" border="0">
        <tr> 
            <td class="default">Virtual Center Name:</td>
            <td><asp:textbox ID="txtName" CssClass="default" runat="server" Width="200" MaxLength="50"/></td>
        </tr>
        <tr> 
            <td class="default">URL:</td>
            <td><asp:textbox ID="txtUrl" CssClass="default" runat="server" Width="400" MaxLength="100"/></td>
        </tr>
        <tr> 
            <td class="default">Authenticate Using:</td>
            <td>
                <asp:DropDownList ID="ddlEnvironment" CssClass="default" runat="server" >
                    <asp:ListItem Value="2" Text="CORPDEV Credentials" />
                    <asp:ListItem Value="3" Text="CORPTEST Credentials" />
                    <asp:ListItem Value="4" Text="CORPDMN Credentials" />
                    <asp:ListItem Value="999" Text="PNC Credentials" />
                </asp:DropDownList>
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
                <asp:button ID="btnUpdateVC" CssClass="default" runat="server" Text="Add" Width="75" OnClick="btnUpdateVC_Click" OnClientClick="ProcessButtons(this) && LoadWait();" />
                <asp:button ID="btnCancelVC" CssClass="default" runat="server" Text="Cancel" Width="75" OnClick="btnCancel_Click" OnClientClick="ProcessButtons(this) && LoadWait();" />
            </td>
        </tr>
    </table>
    <br /><br />
    <span class="header">Data Centers</span>
    <br /><br />
    <table width="100%" cellpadding="4" cellspacing="0" border="0" align="center" style="border:solid 1px #CCCCCC">
        <tr bgcolor="#EEEEEE">
            <td width="30%" nowrap><b>Data Center</b></td>
            <td width="50%" nowrap><b>Build Folder</b></td>
            <td width="20%" nowrap><b>Modified</b></td>
            <td nowrap align="center"><b>Enabled</b></td>
        </tr>
    <asp:repeater ID="rptDCs" runat="server">
        <ItemTemplate>
            <tr class='<%# DataBinder.Eval(Container.DataItem, "enabled").ToString() == "1" ? "default" : "component_unavailable" %>' onmouseover="CellRowOver(this);" onmouseout="CellRowOut(this);" onclick="if(event.srcElement.tagName != 'IMG') { LoadWait() && window.navigate('<%# FormURL("dc=" + DataBinder.Eval(Container.DataItem, "id")) %>'); }">
                <td width="30%"><%# DataBinder.Eval(Container.DataItem, "name") %></td>
                <td width="50%"><%# DataBinder.Eval(Container.DataItem, "build_folder").ToString() == "" ? "---" : DataBinder.Eval(Container.DataItem, "build_folder") %></td>
                <td width="20%" nowrap><%# DataBinder.Eval(Container.DataItem, "modified") %></td>
                <td nowrap align="center"><img src="/images/<%# DataBinder.Eval(Container.DataItem, "enabled").ToString() == "1" ? "check" : "cancel" %>.gif" border="0" align="absmiddle" /></td>
            </tr>
        </ItemTemplate>
    </asp:repeater>
        <tr>
            <td colspan="6"><asp:Label ID="lblDataCenters" runat="server" CssClass="error" Visible="false" Text="<img src='/images/bigAlert.gif' border='0' align='absmiddle'> There are no data centers for this virtual center server" /></td>
        </tr>
    </table>
    <br /><br />
    <asp:Button ID="btnAddDataCenter" runat="server" CssClass="default" Width="75" Text="Add New" OnClick="btnAddDataCenter_Click" OnClientClick="ProcessButton(this) && LoadWait();" />
</asp:Panel>


<asp:Panel ID="panDC" runat="server" Visible="false">
    <table cellpadding="3" cellspacing="0" border="0">
        <tr> 
            <td class="default">Virtual Center:</td>
            <td><asp:DropDownList ID="ddlDCParent" CssClass="default" runat="server"/></td>
        </tr>
        <tr> 
            <td class="default">Datacenter Name:</td>
            <td><asp:textbox ID="txtDCName" CssClass="default" runat="server" Width="200" MaxLength="50"/>&nbsp;&nbsp;&nbsp;<span class="reddefault"><b>NOTE:</b> This field IS CASE SENSITIVE.</span></td>
        </tr>
        <tr> 
            <td class="default">Build Folder:</td>
            <td><asp:textbox ID="txtDCBuildFolder" CssClass="default" runat="server" Width="200" MaxLength="100"/>&nbsp;&nbsp;&nbsp;<span class="reddefault"><b>NOTE:</b> This field IS CASE SENSITIVE.</span></td>
        </tr>
        <tr> 
            <td class="default">Desktop Network:</td>
            <td><asp:textbox ID="txtDCDesktopNetwork" CssClass="default" runat="server" Width="200" MaxLength="100"/>&nbsp;&nbsp;&nbsp;<span class="reddefault"><b>NOTE:</b> This field IS CASE SENSITIVE.</span></td>
        </tr>
        <tr> 
            <td class="default">Workstation Decom VLAN:</td>
            <td><asp:textbox ID="txtDCWorkstationDecomVLAN" CssClass="default" runat="server" Width="200" MaxLength="100"/>&nbsp;&nbsp;&nbsp;<span class="reddefault"><b>NOTE:</b> This field IS CASE SENSITIVE.</span></td>
        </tr>
        <tr> 
            <td></td>
            <td class="footer">Example: QA_Build</td>
        </tr>
        <tr> 
            <td class="default">Enabled:</td>
            <td><asp:CheckBox ID="chkDCEnabled" runat="server" Checked="true" /></td>
        </tr>
        <tr><td height="5" colspan="2">&nbsp;</td></tr>
        <tr> 
            <td>&nbsp;</td>
            <td>
                <asp:button ID="btnUpdateDC" CssClass="default" runat="server" Text="Add" Width="75" OnClick="btnUpdateDC_Click" OnClientClick="ProcessButtons(this) && LoadWait();" />
                <asp:button ID="btnCancelDC" CssClass="default" runat="server" Text="Cancel" Width="75" OnClick="btnCancel_Click" OnClientClick="ProcessButtons(this) && LoadWait();" />
            </td>
        </tr>
    </table>
    <br /><br />
    <span class="header">Folders</span>
    <br /><br />
    <table width="100%" cellpadding="4" cellspacing="0" border="0" align="center" style="border:solid 1px #CCCCCC">
        <tr bgcolor="#EEEEEE">
            <td width="70%" nowrap><b>Folder Name</b></td>
            <td width="30%" nowrap><b>Modified</b></td>
            <td nowrap align="center"><b>Enabled</b></td>
        </tr>
    <asp:repeater ID="rptFolders" runat="server">
        <ItemTemplate>
            <tr class='<%# DataBinder.Eval(Container.DataItem, "enabled").ToString() == "1" ? "default" : "component_unavailable" %>' onmouseover="CellRowOver(this);" onmouseout="CellRowOut(this);" onclick="if(event.srcElement.tagName != 'IMG') { LoadWait() && window.navigate('<%# FormURL("f=" + DataBinder.Eval(Container.DataItem, "id")) %>'); }">
                <td width="70%"><%# DataBinder.Eval(Container.DataItem, "name") %></td>
                <td width="30%" nowrap><%# DataBinder.Eval(Container.DataItem, "modified") %></td>
                <td nowrap align="center"><img src="/images/<%# DataBinder.Eval(Container.DataItem, "enabled").ToString() == "1" ? "check" : "cancel" %>.gif" border="0" align="absmiddle" /></td>
            </tr>
        </ItemTemplate>
    </asp:repeater>
        <tr>
            <td colspan="6"><asp:Label ID="lblFolders" runat="server" CssClass="error" Visible="false" Text="<img src='/images/bigAlert.gif' border='0' align='absmiddle'> There are no folders for this data center" /></td>
        </tr>
    </table>
    <br /><br />
    <asp:Button ID="btnAddFolder" runat="server" CssClass="default" Width="75" Text="Add New" OnClick="btnAddFolder_Click" OnClientClick="ProcessButton(this) && LoadWait();" />
</asp:Panel>


<asp:Panel ID="panFolder" runat="server" Visible="false">
    <table cellpadding="3" cellspacing="0" border="0">
        <tr> 
            <td class="default">Datacenter:</td>
            <td><asp:Label ID="lblFParent" CssClass="default" runat="server"/></td>
        </tr>
        <tr> 
            <td class="default">Folder Name:</td>
            <td><asp:textbox ID="txtFName" CssClass="default" runat="server" Width="200" MaxLength="50"/>&nbsp;&nbsp;&nbsp;<span class="reddefault"><b>NOTE:</b> This field IS CASE SENSITIVE.</span></td>
        </tr>
        <tr style="display:none"> 
            <td class="default">Notification:</td>
            <td>Notify <asp:textbox ID="txtFNotification" CssClass="default" runat="server" Width="300" MaxLength="100"/> when a cluster has reached 8 hosts and a new cluster should be provisioned</td>
        </tr>
        <tr> 
            <td class="default"></td>
            <td class="reddefault"><img src="/images/spacer.gif" border="0" width="50" height="1" /><img src="/images/down_right.gif" border="0" align="absmiddle" /><b>NOTE:</b> For the notifications, please use LAN ID with a semi-colon separating the users</td>
        </tr>
        <tr> 
            <td class="default">Enabled:</td>
            <td><asp:CheckBox ID="chkFEnabled" runat="server" Checked="true" /></td>
        </tr>
        <tr><td height="5" colspan="2">&nbsp;</td></tr>
        <tr> 
            <td>&nbsp;</td>
            <td>
                <asp:button ID="btnUpdateF" CssClass="default" runat="server" Text="Add" Width="75" OnClick="btnUpdateF_Click" OnClientClick="ProcessButtons(this) && LoadWait();" />
                <asp:button ID="btnCancelF" CssClass="default" runat="server" Text="Cancel" Width="75" OnClick="btnCancel_Click" OnClientClick="ProcessButtons(this) && LoadWait();" />
            </td>
        </tr>
    </table>
    <br /><br />
    <span class="header">Clusters</span>
    <br /><br />
    <table width="100%" cellpadding="4" cellspacing="0" border="0" align="center" style="border:solid 1px #CCCCCC">
        <tr bgcolor="#EEEEEE">
            <td width="50%" nowrap><b>Cluster Name</b></td>
            <td width="20%" nowrap><b>Maximum Guests</b></td>
            <td width="30%" nowrap><b>Modified</b></td>
            <td nowrap align="center"><b>Enabled</b></td>
        </tr>
    <asp:repeater ID="rptClusters" runat="server">
        <ItemTemplate>
            <tr class='<%# (DataBinder.Eval(Container.DataItem, "enabled").ToString() == "1" ? (DataBinder.Eval(Container.DataItem, "at_max").ToString() == "1" ? "reddefault" : "default") : "component_unavailable") %>' onmouseover="CellRowOver(this);" onmouseout="CellRowOut(this);" onclick="if(event.srcElement.tagName != 'IMG') { LoadWait() && window.navigate('<%# FormURL("c=" + DataBinder.Eval(Container.DataItem, "id")) %>'); }">
                <td width="50%"><%# DataBinder.Eval(Container.DataItem, "name") %></td>
                <td width="20%"><%# DataBinder.Eval(Container.DataItem, "maximum") %></td>
                <td width="30%" nowrap><%# DataBinder.Eval(Container.DataItem, "modified") %></td>
                <td nowrap align="center"><img src="/images/<%# DataBinder.Eval(Container.DataItem, "enabled").ToString() == "1" ? "check" : "cancel" %>.gif" border="0" align="absmiddle" /></td>
            </tr>
        </ItemTemplate>
    </asp:repeater>
        <tr>
            <td colspan="6"><asp:Label ID="lblClusters" runat="server" CssClass="error" Visible="false" Text="<img src='/images/bigAlert.gif' border='0' align='absmiddle'> There are no clusters for this folder" /></td>
        </tr>
    </table>
    <br /><br />
    <asp:Button ID="btnAddCluster" runat="server" CssClass="default" Width="75" Text="Add New" OnClick="btnAddCluster_Click" OnClientClick="ProcessButton(this) && LoadWait();" />
</asp:Panel>


<asp:Panel ID="panCluster" runat="server" Visible="false">
    <table cellpadding="3" cellspacing="0" border="0">
        <tr> 
            <td class="default">Folder:</td>
            <td><asp:Label ID="lblCParent" CssClass="default" runat="server"/></td>
        </tr>
        <tr> 
            <td class="default">Cluster Name:</td>
            <td><asp:textbox ID="txtCName" CssClass="default" runat="server" Width="200" MaxLength="50"/>&nbsp;&nbsp;&nbsp;<span class="reddefault"><b>NOTE:</b> This field IS CASE SENSITIVE.</span></td>
        </tr>
        <tr> 
            <td class="default">Version:</td>
            <td>
                <asp:DropDownList ID="ddlVersion" CssClass="default" runat="server" >
                    <asp:ListItem Value="0" Text="Virtual Center 3.5" />
                    <asp:ListItem Value="1" Text="Virtual Center 4 (Vsphere)" />
                </asp:DropDownList>
            </td>
        </tr>
        <tr> 
            <td class="default">Anti-Affinity:</td>
            <td>
                <asp:DropDownList ID="ddlAntiAffinity" CssClass="default" runat="server" >
                    <asp:ListItem Value="0" Text="VMWARE UPDATE 3 - Individual" />
                    <asp:ListItem Value="1" Text="VMWARE UPDATE 4 - All at Once" />
                </asp:DropDownList>
            </td>
        </tr>
        <tr style="display:none"> 
            <td class="default">Maximum Guests:</td>
            <td><asp:textbox ID="txtCMaximum" CssClass="default" runat="server" Width="100" MaxLength="10"/></td>
        </tr>
        <tr> 
            <td class="default">Resource Pool:</td>
            <td><asp:textbox ID="txtResourcePool" CssClass="default" runat="server" Width="200" MaxLength="50"/> (Optional)</td>
        </tr>
        <tr style="display:none"> 
            <td class="default">Datastore Notification:</td>
            <td>Notify <asp:textbox ID="txtDatastoreNotify" CssClass="default" runat="server" Width="300" MaxLength="100"/> when <asp:textbox ID="txtDatastoreLeft" CssClass="default" runat="server" Width="75" MaxLength="10"/> datastores are less than <asp:textbox ID="txtDatastoreSize" CssClass="default" runat="server" Width="75" MaxLength="10"/> GB</td>
        </tr>
        <tr> 
            <td class="default"></td>
            <td class="reddefault"><img src="/images/spacer.gif" border="0" width="50" height="1" /><img src="/images/down_right.gif" border="0" align="absmiddle" /><b>NOTE:</b> For the notifications, please use LAN ID with a semi-colon separating the users</td>
        </tr>
        <tr> 
            <td class="default">Set Cluster to Full:</td>
            <td><asp:CheckBox ID="chkFull" runat="server" Checked="true" /></td>
        </tr>
        <tr> 
            <td class="default">Turn Auto-Provisioning Off:</td>
            <td><asp:CheckBox ID="chkAPoff" runat="server" Checked="true" /></td>
        </tr>
        <tr> 
            <td class="default">Turn Auto-Provisioning Off (DR):</td>
            <td><asp:CheckBox ID="chkAPoffDR" runat="server" Checked="true" /></td>
        </tr>
        <tr> 
            <td class="default">DELL Hardware:</td>
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
            <td><asp:CheckBox ID="chkCEnabled" runat="server" Checked="true" /></td>
        </tr>
        <tr><td height="5" colspan="2">&nbsp;</td></tr>
        <tr> 
            <td>&nbsp;</td>
            <td>
                <asp:button ID="btnUpdateC" CssClass="default" runat="server" Text="Add" Width="75" OnClick="btnUpdateC_Click" OnClientClick="ProcessButtons(this) && LoadWait();" />
                <asp:button ID="btnCancelC" CssClass="default" runat="server" Text="Cancel" Width="75" OnClick="btnCancel_Click" OnClientClick="ProcessButtons(this) && LoadWait();" />
            </td>
        </tr>
    </table>
    <asp:Label ID="txtFailures" runat="server" Visible="false" />
    <asp:Label ID="txtCPUUtilization" runat="server" Visible="false" />
    <asp:Label ID="txtRAMUtilization" runat="server" Visible="false" />
    <asp:Label ID="txtMaxRAM" runat="server" Visible="false" />
    <asp:Label ID="txtAvgUtilization" runat="server" Visible="false" />
    <asp:Label ID="txtLunSize" runat="server" Visible="false" />
    <asp:Label ID="txtLunUtilization" runat="server" Visible="false" />
    <asp:Label ID="txtVMsPerLun" runat="server" Visible="false" />
    <asp:Label ID="txtTimeLUN" runat="server" Visible="false" />
    <asp:Label ID="txtTimeCluster" runat="server" Visible="false" />
    <asp:Label ID="txtMaxVMsServer" runat="server" Visible="false" />
    <asp:Label ID="txtMaxVMsLUN" runat="server" Visible="false" />
    <br /><br />
    <%=strMenuTab1 %>
    <div id="divMenu1" class="tabbing">
        <br />
        <div style="display:none">
            <span class="header">Hosts</span>
            <br /><br />
            <table width="100%" cellpadding="4" cellspacing="0" border="0" align="center" style="border:solid 1px #CCCCCC">
                <tr bgcolor="#EEEEEE">
                    <td width="50%" nowrap><b>Host Name</b></td>
                    <td width="20%" nowrap><b>Maximum Guests</b></td>
                    <td width="30%" nowrap><b>Modified</b></td>
                    <td nowrap align="center"><b>Enabled</b></td>
                </tr>
            <asp:repeater ID="rptHosts" runat="server">
                <ItemTemplate>
                    <tr class='<%# DataBinder.Eval(Container.DataItem, "enabled").ToString() == "1" ? "default" : "component_unavailable" %>' onmouseover="CellRowOver(this);" onmouseout="CellRowOut(this);" onclick="if(event.srcElement.tagName != 'IMG') { LoadWait() && window.navigate('<%# FormURL("h=" + DataBinder.Eval(Container.DataItem, "id")) %>'); }">
                        <td width="50%"><%# DataBinder.Eval(Container.DataItem, "name") %></td>
                        <td width="20%"><%# DataBinder.Eval(Container.DataItem, "maximum") %></td>
                        <td width="30%" nowrap><%# DataBinder.Eval(Container.DataItem, "modified") %></td>
                        <td nowrap align="center"><img src="/images/<%# DataBinder.Eval(Container.DataItem, "enabled").ToString() == "1" ? "check" : "cancel" %>.gif" border="0" align="absmiddle" /></td>
                    </tr>
                </ItemTemplate>
            </asp:repeater>
                <tr>
                    <td colspan="6"><asp:Label ID="lblHosts" runat="server" CssClass="error" Visible="false" Text="<img src='/images/bigAlert.gif' border='0' align='absmiddle'> There are no hosts for this cluster" /></td>
                </tr>
            </table>
            <br /><br />
            <asp:Button ID="btnAddHost" runat="server" CssClass="default" Width="75" Text="Add New" OnClick="btnAddHost_Click" OnClientClick="ProcessButton(this) && LoadWait();" />
        </div>
        <div style="display:none">
            <span class="header">Datastores</span>
            <br /><br />
            <table width="100%" cellpadding="4" cellspacing="0" border="0" align="center" style="border:solid 1px #CCCCCC">
                <tr bgcolor="#EEEEEE">
                    <td width="50%" nowrap><b>Datastore Name</b></td>
                    <td width="20%" nowrap><b>Maximum Guests</b></td>
                    <td width="30%" nowrap><b>Modified</b></td>
                    <td nowrap align="center"><b>Enabled</b></td>
                </tr>
            <asp:repeater ID="rptDatastores" runat="server">
                <ItemTemplate>
                    <tr class='<%# DataBinder.Eval(Container.DataItem, "enabled").ToString() == "1" ? "default" : "component_unavailable" %>' onmouseover="CellRowOver(this);" onmouseout="CellRowOut(this);" onclick="if(event.srcElement.tagName != 'IMG') { LoadWait() && window.navigate('<%# FormURL("ds=" + DataBinder.Eval(Container.DataItem, "id")) %>'); }">
                        <td width="50%"><%# DataBinder.Eval(Container.DataItem, "name") %></td>
                        <td width="20%"><%# DataBinder.Eval(Container.DataItem, "maximum") %></td>
                        <td width="30%" nowrap><%# DataBinder.Eval(Container.DataItem, "modified") %></td>
                        <td nowrap align="center"><img src="/images/<%# DataBinder.Eval(Container.DataItem, "enabled").ToString() == "1" ? "check" : "cancel" %>.gif" border="0" align="absmiddle" /></td>
                    </tr>
                </ItemTemplate>
            </asp:repeater>
                <tr>
                    <td colspan="6"><asp:Label ID="lblDatastores" runat="server" CssClass="default" Visible="false" Text="<img src='/images/bigCheck.gif' border='0' align='absmiddle'> There are no datastores for this cluster" /></td>
                </tr>
            </table>
            <br /><br />
            <asp:Button ID="btnAddDatastore" runat="server" CssClass="default" Width="75" Text="Add New" OnClick="btnAddDatastore_Click" OnClientClick="ProcessButton(this) && LoadWait();" />
        </div>
        <div style="display:none">
            <span class="header">VLANs</span>
            <br /><br />
            <table width="100%" cellpadding="4" cellspacing="0" border="0" align="center" style="border:solid 1px #CCCCCC">
                <tr bgcolor="#EEEEEE">
                    <td width="70%" nowrap><b>VLAN Name</b></td>
                    <td width="30%" nowrap><b>Modified</b></td>
                    <td nowrap align="center"><b>Enabled</b></td>
                </tr>
            <asp:repeater ID="rptVLANs" runat="server">
                <ItemTemplate>
                    <tr class='<%# DataBinder.Eval(Container.DataItem, "enabled").ToString() == "1" ? "default" : "component_unavailable" %>' onmouseover="CellRowOver(this);" onmouseout="CellRowOut(this);" onclick="if(event.srcElement.tagName != 'IMG') { LoadWait() && window.navigate('<%# FormURL("v=" + DataBinder.Eval(Container.DataItem, "id")) %>'); }">
                        <td width="70%"><%# DataBinder.Eval(Container.DataItem, "name") %></td>
                        <td width="30%" nowrap><%# DataBinder.Eval(Container.DataItem, "modified") %></td>
                        <td nowrap align="center"><img src="/images/<%# DataBinder.Eval(Container.DataItem, "enabled").ToString() == "1" ? "check" : "cancel" %>.gif" border="0" align="absmiddle" /></td>
                    </tr>
                </ItemTemplate>
            </asp:repeater>
                <tr>
                    <td colspan="6"><asp:Label ID="lblVLANs" runat="server" CssClass="default" Visible="false" Text="<img src='/images/bigCheck.gif' border='0' align='absmiddle'> There are no VLANs for this cluster" /></td>
                </tr>
            </table>
            <br /><br />
            <asp:Button ID="btnAddVLAN" runat="server" CssClass="default" Width="75" Text="Add New" OnClick="btnAddVLAN_Click" OnClientClick="ProcessButton(this) && LoadWait();" />
        </div>
</asp:Panel>


<asp:Panel ID="panHost" runat="server" Visible="false">
    <table cellpadding="3" cellspacing="0" border="0">
        <tr> 
            <td class="default">Cluster:</td>
            <td><asp:Label ID="lblHParent" CssClass="default" runat="server"/></td>
        </tr>
        <tr> 
            <td class="default">Host Name:</td>
            <td><asp:textbox ID="txtHName" CssClass="default" runat="server" Width="200" MaxLength="50"/>&nbsp;&nbsp;&nbsp;<span class="reddefault"><b>NOTE:</b> This field IS CASE SENSITIVE.</span></td>
        </tr>
        <tr style="display:none"> 
            <td class="default">Maximum Guests:</td>
            <td><asp:textbox ID="txtHMaximum" CssClass="default" runat="server" Width="100" MaxLength="10"/></td>
        </tr>
        <tr> 
            <td class="default">Enabled:</td>
            <td><asp:CheckBox ID="chkHEnabled" runat="server" Checked="true" /></td>
        </tr>
        <tr><td height="5" colspan="2">&nbsp;</td></tr>
        <tr> 
            <td>&nbsp;</td>
            <td>
                <asp:button ID="btnUpdateH" CssClass="default" runat="server" Text="Add" Width="75" OnClick="btnUpdateH_Click" OnClientClick="ProcessButtons(this) && LoadWait();" />
                <asp:button ID="btnCancelH" CssClass="default" runat="server" Text="Cancel" Width="75" OnClick="btnCancel_Click" OnClientClick="ProcessButtons(this) && LoadWait();" />
            </td>
        </tr>
    </table>
</asp:Panel>

<asp:Panel ID="panDatastore" runat="server" Visible="false">
    <table cellpadding="3" cellspacing="0" border="0">
        <tr> 
            <td class="default">Cluster:</td>
            <td><asp:Label ID="lblDSParent" CssClass="default" runat="server"/></td>
        </tr>
        <tr> 
            <td class="default">Datastore Name:</td>
            <td><asp:textbox ID="txtDSName" CssClass="default" runat="server" Width="200" MaxLength="50"/>&nbsp;&nbsp;&nbsp;<span class="reddefault"><b>NOTE:</b> This field IS CASE SENSITIVE.</span></td>
        </tr>
        <tr> 
            <td class="default">Storage Type:</td>
            <td>
                <asp:DropDownList ID="ddlType" CssClass="default" runat="server">
                    <asp:ListItem Value="1" Text="Low Performance" />
                    <asp:ListItem Value="10" Text="Standard Performance" />
                    <asp:ListItem Value="100" Text="High Performance" />
                </asp:DropDownList>
            </td>
        </tr>
        <tr> 
            <td class="default">Operating System Group:</td>
            <td><asp:DropDownList ID="ddlOperatingSystemGroup" CssClass="default" runat="server"/></td>
        </tr>
        <tr> 
            <td class="default">Replicated:</td>
            <td><asp:CheckBox ID="chkReplicated" runat="server" Checked="true" /></td>
        </tr>
        <tr style="display:none"> 
            <td class="default">Maximum Guests:</td>
            <td><asp:textbox ID="txtDSMaximum" CssClass="default" runat="server" Width="100" MaxLength="10"/></td>
        </tr>
        <tr> 
            <td class="default">Server:</td>
            <td><asp:CheckBox ID="chkServer" runat="server" Checked="true" /></td>
        </tr>
        <tr> 
            <td class="default">Pagefile:</td>
            <td><asp:CheckBox ID="chkPagefile" runat="server" Checked="true" /></td>
        </tr>
        <tr> 
            <td class="default">Override Permission:</td>
            <td><asp:CheckBox ID="chkOverridePermission" runat="server" /></td>
        </tr>
        <tr> 
            <td class="default">Partnered Datastore:</td>
            <td><asp:DropDownList ID="ddlPartner" CssClass="default" runat="server"/></td>
        </tr>
        <tr> 
            <td class="default">Enabled:</td>
            <td><asp:CheckBox ID="chkDSEnabled" runat="server" Checked="true" /></td>
        </tr>
        <tr><td height="5" colspan="2">&nbsp;</td></tr>
        <tr> 
            <td>&nbsp;</td>
            <td>
                <asp:button ID="btnUpdateDS" CssClass="default" runat="server" Text="Add" Width="75" OnClick="btnUpdateDS_Click" OnClientClick="ProcessButtons(this) && LoadWait();" />
                <asp:button ID="btnCancelDS" CssClass="default" runat="server" Text="Cancel" Width="75" OnClick="btnCancel_Click" OnClientClick="ProcessButtons(this) && LoadWait();" />
            </td>
        </tr>
    </table>
</asp:Panel>

<asp:Panel ID="panVLAN" runat="server" Visible="false">
    <table cellpadding="3" cellspacing="0" border="0">
        <tr> 
            <td class="default">Cluster:</td>
            <td><asp:Label ID="lblVParent" CssClass="default" runat="server"/></td>
        </tr>
        <tr> 
            <td class="default">Name:</td>
            <td><asp:textbox ID="txtVName" CssClass="default" runat="server" Width="200" MaxLength="50"/>&nbsp;&nbsp;&nbsp;<span class="reddefault"><b>NOTE:</b> This field IS CASE SENSITIVE.</span></td>
        </tr>
        <tr> 
            <td class="default">Enabled:</td>
            <td><asp:CheckBox ID="chkVEnabled" runat="server" Checked="true" /></td>
        </tr>
        <tr><td height="5" colspan="2">&nbsp;</td></tr>
        <tr> 
            <td>&nbsp;</td>
            <td>
                <asp:button ID="btnUpdateV" CssClass="default" runat="server" Text="Add" Width="75" OnClick="btnUpdateV_Click" OnClientClick="ProcessButtons(this) && LoadWait();" />
                <asp:button ID="btnCancelV" CssClass="default" runat="server" Text="Cancel" Width="75" OnClick="btnCancel_Click" OnClientClick="ProcessButtons(this) && LoadWait();" />
            </td>
        </tr>
    </table>
</asp:Panel>

<asp:HiddenField ID="hdnTab" runat="server" />
