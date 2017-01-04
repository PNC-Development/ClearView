<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="admin_index.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.admin_index" %>

<html>
<head>
<title>ClearView Administration</title>
<link rel="stylesheet" type="text/css" href="/css/default.css" />
<script type="text/javascript" src="/javascript/global.js"></script>
<script type="text/javascript" src="/javascript/both.js"></script>
<script type="text/javascript">
    var oOldDiv = null;
    var oOldCell = null;
    function Load() {
        var oFrame = document.getElementById('frmPage');
        oFrame.src = '<%=strRedirect%>';
        oOldDiv = document.getElementById('divTab');
        oOldDiv.style.display = "inline";
    }
    function ChangeFrame6(oCell, oShow) {
        if (oOldDiv != null)
            oOldDiv.style.display = "none";
        if (oOldCell != null)
            oOldCell.style.border = "1px solid #94a6b5"
        oOldDiv = document.getElementById(oShow);
        oOldDiv.style.display = "inline";
        oOldCell = oCell;
	    oOldCell.style.borderTop = "3px solid #6A8359"
        oOldCell.style.borderBottom = "1px solid #EFEFEF"
    }
</script>
</head>
<body topmargin="0" leftmargin="0" scroll="no" onload="Load();">
<form id="Form1" runat="server">
<table width="100%" height="100%" border="0" cellspacing="0" cellpadding="0">
  <tr>
    <td nowrap valign="top" bgcolor="#EFEFEF" style="border-right:solid 1px #999999" width="300">
        <table width="300" height="100%" cellpadding="0" cellspacing="0" border="0">
            <tr height="1" style="display:none">
                <td align="left" colspan="2" bgcolor="#007253">
                    <table width="100%"border="0" cellSpacing="0" cellPadding="0" class="whitebold">
                        <tr>
                            <td><img src="/images/nc_logo.gif" border="0" /></td>
                            <td width="100%" align="center" class="whitebold"><asp:HyperLink ID="hypLogout" runat="server" CssClass="whitebold" Text="<img src='/images/logoff.gif' border='0' align='absmiddle' /><img src='images/spacer.gif' width='5' height='1' border='0' />Log Out" NavigateUrl="login.aspx" Target="frmPage" /></td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr height="1" style="display:inline">
                <td align="left" colspan="2" bgcolor="#446388">
                    <table width="100%"border="0" cellSpacing="0" cellPadding="0" class="whitebold">
                        <tr>
                            <td style="padding:7px"><img src="/images/PNCAdminLogo.gif" border="0" /></td>
                            <td width="100%" align="center" class="whitebold"><asp:HyperLink ID="hypLogoutPNC" runat="server" CssClass="whitebold" Text="<img src='/images/logoff.gif' border='0' align='absmiddle' /><img src='images/spacer.gif' width='5' height='1' border='0' />Log Out" NavigateUrl="login.aspx" Target="frmPage" /></td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td width="300">
                    <%=strMenuTab1 %>
                    <br />
                    <div id="divMenu1" style="width:300px; height:100%; overflow:auto;">
                        <div id="divTab5" style="display:none">
                            <table width="280" border="0" cellspacing="2" cellpadding="2">
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="change_user.aspx" class="leftnav" target="frmPage">Change Live User</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="recommission.aspx" class="leftnav" target="frmPage">Recommission Device(s)</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="bypass.aspx" class="leftnav" target="frmPage">Bypass Cooldown for Decom(s)</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="admin_SetupMaster.aspx" class="leftnav" target="frmPage">ClearView Setup Master</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="ad_sync.aspx" class="leftnav" target="frmPage">Sync Active Directory</a></td>
                                </tr>
                                <tr style="display:none">
                                    <td class="strikethrough"><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="sync_org.aspx" class="leftnav" target="frmPage">Sync Org Chart</a></td>
                                </tr>
                                <tr style="display:none">
                                    <td class="strikethrough"><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="request_field_values.aspx" class="leftnav" target="frmPage">Purge Request Field Values</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="database.aspx" class="leftnav" target="frmPage">Database Functions</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="logins.aspx" class="leftnav" target="frmPage">View Logins</a></td>
                                </tr>
                                <tr style="display:none">
                                    <td class="strikethrough"><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="workstation_ping.aspx" class="leftnav" target="frmPage">View Workstation Pings</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="service_excel_upload.aspx" class="leftnav" target="frmPage">Service Excel Upload</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="fix_vsg.aspx" class="leftnav" target="frmPage">Import VSG Numbers</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="log.aspx" class="leftnav" target="frmPage">View Log</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/spacer.gif" border="0" width="1" height="1" /></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="errors_server.aspx" class="leftnav" target="frmPage">AP Server Errors</a></td>
                                </tr>	
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="errors_workstation.aspx" class="leftnav" target="frmPage">AP Workstation Errors</a></td>
                                </tr>	
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="incidents.aspx" class="leftnav" target="frmPage">Incident Routing</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="errors_decommission.aspx" class="leftnav" target="frmPage">Decommission Errors</a></td>
                                </tr>	
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="reset_execution.aspx" class="leftnav" target="frmPage">Reset OnDemand Execution</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="scheduler.aspx" class="leftnav" target="frmPage">Scheduler</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="admin.aspx" class="leftnav" target="frmPage">More Functions</a></td>
                                </tr>
                                <tr style="display:none">
                                    <td class="strikethrough"><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="virtual_host.aspx" class="leftnav" target="frmPage">Add a Virtual Wkst Host</a></td>
                                </tr>
                                <tr style="display:none">
                                    <td class="strikethrough"><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="albert_ip.aspx" class="leftnav" target="frmPage">Albert's IP Script</a></td>
                                </tr>
                                <tr>
                                    <td><p>&nbsp;</p><p>&nbsp;</p><p>&nbsp;</p></td>
                                </tr>
                            </table>
                        </div>
                        <div id="divTab6" style="display:none">
                            <table width="280" border="0" cellspacing="2" cellpadding="2">
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="vmware/virtual_centers.aspx" class="leftnav" target="frmPage">Virtual Centers</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="vmware/datacenters.aspx" class="leftnav" target="frmPage">Datacenters</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="vmware/folders.aspx" class="leftnav" target="frmPage">Folders</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="vmware/clusters.aspx" class="leftnav" target="frmPage">Clusters</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="vmware/hosts.aspx" class="leftnav" target="frmPage">Hosts</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="vmware/datastores.aspx" class="leftnav" target="frmPage">Datastores</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="vmware/vlans.aspx" class="leftnav" target="frmPage">Vlans</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="vmware/pools.aspx" class="leftnav" target="frmPage">Resource Pools</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="vmware/addhost.aspx" class="leftnav" target="frmPage">Host Provisioning</a></td>
                                </tr>
                                <tr>
                                    <td><p>&nbsp;</p><p>&nbsp;</p><p>&nbsp;</p></td>
                                </tr>
                            </table>
                        </div>
                        <div id="divTab1" style="display:none">
                            <table width="280" border="0" cellspacing="2" cellpadding="2">
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="applications.aspx" class="leftnav" target="frmPage">Applications</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="audit_scripts.aspx" class="leftnav" target="frmPage">Audit Scripts</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="audit_script_sets.aspx" class="leftnav" target="frmPage">Audit Script Sets</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="audit_script_languages.aspx" class="leftnav" target="frmPage">Audit Script Languages</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="chipsets.aspx" class="leftnav" target="frmPage">Chipsets</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="class.aspx" class="leftnav" target="frmPage">Classes</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="confidence.aspx" class="leftnav" target="frmPage">Confidence Levels</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="admin_controls.aspx" class="leftnav" target="frmPage">Controls</a></td>
                                </tr>
                                 <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="admin_ui_controls.aspx" class="leftnav" target="frmPage">UI Controls(ToolTip, Help, Validations)</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="costs.aspx" class="leftnav" target="frmPage">Cost Centers</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="dell_configs.aspx" class="leftnav" target="frmPage">Dell Configs</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="did_you_knows.aspx" class="leftnav" target="frmPage">Did You Know's</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="dns_types.aspx" class="leftnav" target="frmPage">DNS Types</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="domains.aspx" class="leftnav" target="frmPage">Domains</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="domain_controllers.aspx" class="leftnav" target="frmPage">Domain Controllers</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="domains_dns.aspx" class="leftnav" target="frmPage">Domains DNS</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="domains_suffix.aspx" class="leftnav" target="frmPage">Domain Suffix Lists</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="domains_admin_groups.aspx" class="leftnav" target="frmPage">Domain Admin Groups</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="enhancement_modules.aspx" class="leftnav" target="frmPage">Enhancement Modules</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="enhancement_approval_groups.aspx" class="leftnav" target="frmPage">Enhancement Approval Groups</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="enhancements.aspx" class="leftnav" target="frmPage">Enhancement Versions</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="environments.aspx" class="leftnav" target="frmPage">Environments</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="error_types.aspx" class="leftnav" target="frmPage">Error Types</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="error_types_types.aspx" class="leftnav" target="frmPage">Error Types II</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="groups.aspx" class="leftnav" target="frmPage">Groups (#1)</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="holidays.aspx" class="leftnav" target="frmPage">Holidays</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="admin_hosts.aspx" class="leftnav" target="frmPage">Hosts</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="icons.aspx" class="leftnav" target="frmPage">Icons</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="location_state.aspx" class="leftnav" target="frmPage">Location - States</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="location_city.aspx" class="leftnav" target="frmPage">Location - Cities</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="location_address.aspx" class="leftnav" target="frmPage">Location - Addresses</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="maintenance_window.aspx" class="leftnav" target="frmPage">Maintenance Windows</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="mnemonics.aspx" class="leftnav" target="frmPage">Mnemonics</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="operating_system_groups.aspx" class="leftnav" target="frmPage">Operating System Groups</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="operating_systems.aspx" class="leftnav" target="frmPage">Operating Systems</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="service_packs.aspx" class="leftnav" target="frmPage">OS - Service Packs / Maint. Levels</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="organizations.aspx" class="leftnav" target="frmPage">Organizations</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="order_report_datasource.aspx" class="leftnav" target="frmPage">Order Report - Data Source</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="order_report_charts.aspx" class="leftnav" target="frmPage">Order Report - Charts</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="pages.aspx" class="leftnav" target="frmPage">Pages</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="apppages.aspx" class="leftnav" target="frmPage">Page Permissions</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="permissions.aspx" class="leftnav" target="frmPage">Permissions (#2)</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="platforms.aspx" class="leftnav" target="frmPage">Platforms</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="platform_forms.aspx" class="leftnav" target="frmPage">Platform Forms</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="pnc_tasks.aspx" class="leftnav" target="frmPage">Pre-Production Tasks</a></td>
                                </tr>
                                <tr style="display:none">
                                    <td class="strikethrough"><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="recovery_locations.aspx" class="leftnav" target="frmPage">Recovery Locations</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="report_groups.aspx" class="leftnav" target="frmPage">Report Groups</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="reports.aspx" class="leftnav" target="frmPage">Reports</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="request_items.aspx" class="leftnav" target="frmPage">Request Items</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="resiliency.aspx" class="leftnav" target="frmPage">Resiliency</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="restarts.aspx" class="leftnav" target="frmPage">Restarts</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="rotator_header.aspx" class="leftnav" target="frmPage">Rotator Images (Header)</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="admin_scheduling.aspx" class="leftnav" target="frmPage">Scheduling</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="segments.aspx" class="leftnav" target="frmPage">Segments</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="servername_applications.aspx" class="leftnav" target="frmPage">Server Name Applications</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="servername_subapplications.aspx" class="leftnav" target="frmPage">Server Name Sub-Applications</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="servername_codes.aspx" class="leftnav" target="frmPage">Server Name Codes</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="servername_components.aspx" class="leftnav" target="frmPage">Server Components</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="servername_components_details.aspx" class="leftnav" target="frmPage">Server Component Details</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="service_folders.aspx" class="leftnav" target="frmPage">Service Folders</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="services.aspx" class="leftnav" target="frmPage">Services</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="service_editor_fields.aspx" class="leftnav" target="frmPage">Service Editor Fields</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="service_types.aspx" class="leftnav" target="frmPage">Service Types</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="service_details.aspx" class="leftnav" target="frmPage">Service Details</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="service_selection.aspx" class="leftnav" target="frmPage">Service Selection</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="service_steps.aspx" class="leftnav" target="frmPage">Service Steps</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="sve_clusters.aspx" class="leftnav" target="frmPage">Sun Virtual Environment Clusters</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="templates.aspx" class="leftnav" target="frmPage">Templates</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="admin_users.aspx" class="leftnav" target="frmPage">Users</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="users_ats.aspx" class="leftnav" target="frmPage">Users @'s</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="userguide.aspx" class="leftnav" target="frmPage">User Guide</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="virtual_cpu.aspx" class="leftnav" target="frmPage">Virtual CPUs</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="virtual_hdd.aspx" class="leftnav" target="frmPage">Virtual Hard Drives</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="virtual_ram.aspx" class="leftnav" target="frmPage">Virtual RAM</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="vmware_templates.aspx" class="leftnav" target="frmPage">VMWare Templates</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="webservices.aspx" class="leftnav" target="frmPage">Web Service Methods</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="whats_new.aspx" class="leftnav" target="frmPage">What's New?</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="workload_manager_tabs.aspx" class="leftnav" target="frmPage">Workload Manager Tabs</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="workstation_components.aspx" class="leftnav" target="frmPage">Workstation Components</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="workstation_components_scripts.aspx" class="leftnav" target="frmPage">Workstation Components Scripts</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="zeus_array_configs.aspx" class="leftnav" target="frmPage">ZEUS Array Configs</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="zeus_build_types.aspx" class="leftnav" target="frmPage">ZEUS Build Types</a></td>
                                </tr>
                                <tr>
                                    <td><p>&nbsp;</p><p>&nbsp;</p><p>&nbsp;</p></td>
                                </tr>
                            </table>
                        </div>
                        <div id="divTab2" style="display:none">
                            <table width="280" border="0" cellspacing="2" cellpadding="2">
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="asset/asset_types.aspx" class="leftnav" target="frmPage">Types</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="asset/asset_category.aspx" class="leftnav" target="frmPage">Asset Category</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="asset/asset_category_deployment_config.aspx" class="leftnav" target="frmPage">Asset Category Deployment Config</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="asset/asset_models.aspx" class="leftnav" target="frmPage">Models</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="asset/asset_models_boot_groups.aspx" class="leftnav" target="frmPage">Model Boot Groups</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="asset/solaris_build_types.aspx" class="leftnav" target="frmPage">Solaris Build Types</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="asset/solaris_interfaces.aspx" class="leftnav" target="frmPage">Solaris Interfaces</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="asset/asset_sites.aspx" class="leftnav" target="frmPage">Sites</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="asset/asset_depot.aspx" class="leftnav" target="frmPage">Depot Locations</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="asset/asset_depot_room.aspx" class="leftnav" target="frmPage">Depot Rooms</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="asset/asset_shelf.aspx" class="leftnav" target="frmPage">Shelves</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="asset/asset_rooms.aspx" class="leftnav" target="frmPage">Rooms</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="asset/asset_zones.aspx" class="leftnav" target="frmPage">Zones</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="asset/asset_racks.aspx" class="leftnav" target="frmPage">Racks</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="asset/asset_floor.aspx" class="leftnav" target="frmPage">Floors</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="asset/asset_banks.aspx" class="leftnav" target="frmPage">Banks / Affiliates</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="asset/asset_enclosures.aspx" class="leftnav" target="frmPage">Associate DR Enclosures</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="asset/asset_switches.aspx" class="leftnav" target="frmPage">Switches</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="asset/decommission_warnings.aspx" class="leftnav" target="frmPage">Decom Warnings</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/spacer.gif" border="0" width="1" height="1" /></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="asset/asset_models_servers.aspx" class="leftnav" target="frmPage">MODELS - Servers</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="asset/asset_models_network.aspx" class="leftnav" target="frmPage">MODELS - Network</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="asset/asset_models_other.aspx" class="leftnav" target="frmPage">MODELS - Other</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/spacer.gif" border="0" width="1" height="1" /></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="asset/asset_barcodes.aspx" class="leftnav" target="frmPage">Barcodes</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="asset/asset_scangun.aspx" class="leftnav" target="frmPage">Scan Gun XML Export</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/spacer.gif" border="0" width="1" height="1" /></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="ip_vlans.aspx" class="leftnav" target="frmPage">IP Address VLANs</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="ip_vlans_ha.aspx" class="leftnav" target="frmPage">IP Address VLANs (HA)</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="ip_networks.aspx" class="leftnav" target="frmPage">IP Address Networks</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="ip_dhcp.aspx" class="leftnav" target="frmPage">IP Address DHCP Ranges</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="ip_address.aspx" class="leftnav" target="frmPage">IP Addresses</a></td>
                                </tr>
                                <tr>
                                    <td><p>&nbsp;</p><p>&nbsp;</p><p>&nbsp;</p></td>
                                </tr>
                            </table>
                        </div>
                        <div id="divTab3" style="display:none">
                            <table width="280" border="0" cellspacing="2" cellpadding="2">
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="projects_pending.aspx" class="leftnav" target="frmPage">Pending Projects / Tasks</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="projects_approve.aspx" class="leftnav" target="frmPage">Pending Project Requests</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="services_pending.aspx" class="leftnav" target="frmPage">Pending Services</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/spacer.gif" border="0" width="1" height="1" /></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="project_request_questions.aspx" class="leftnav" target="frmPage">Project Request Questions</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="project_request_responses.aspx" class="leftnav" target="frmPage">Project Request Responses</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="project_request_classes.aspx" class="leftnav" target="frmPage">Project Request Classes</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/spacer.gif" border="0" width="1" height="1" /></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="custom_tables.aspx" class="leftnav" target="frmPage">Custom Tables</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="custom_tables_fields.aspx" class="leftnav" target="frmPage">Custom Table Fields</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/spacer.gif" border="0" width="1" height="1" /></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="asset/idcasset_types.aspx" class="leftnav" target="frmPage">IDC Asset Types</a></td>
                                </tr>	
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="asset/idc_resources.aspx" class="leftnav" target="frmPage">IDC Resource Types</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/spacer.gif" border="0" width="1" height="1" /></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="fix_leads.aspx" class="leftnav" target="frmPage">Fix Project Leads</a></td>
                                </tr>
                                <tr>
                                    <td><p>&nbsp;</p><p>&nbsp;</p><p>&nbsp;</p></td>
                                </tr>
                            </table>
                        </div>
                        <div id="divTab7" style="display:none">
                            <table width="280" border="0" cellspacing="2" cellpadding="2">
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="design/phases.aspx" class="leftnav" target="frmPage">Phases</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="design/questions.aspx" class="leftnav" target="frmPage">Questions</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="design/responses.aspx" class="leftnav" target="frmPage">Responses</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="design/approval_groups.aspx" class="leftnav" target="frmPage">Approval Groups / Users</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="design/approval_conditions.aspx" class="leftnav" target="frmPage">Approval Conditions</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="design/approvers_groups.aspx" class="leftnav" target="frmPage">Approvers (Overall)</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="design/approvers_fields.aspx" class="leftnav" target="frmPage">Approvers (Field)</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="design/models.aspx" class="leftnav" target="frmPage">Model Configuration</a></td>
                                </tr>
                                <tr>
                                    <td><p>&nbsp;</p><p>&nbsp;</p><p>&nbsp;</p></td>
                                </tr>
                            </table>
                        </div>
                        <div id="divTab4" style="display:none">
                            <table width="280" border="0" cellspacing="2" cellpadding="2">
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="forecast/forecast_questions.aspx" class="leftnav" target="frmPage">Forecast Questions</a></td>
                                </tr>
                                 <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="forecast/forecast_responses_category.aspx" class="leftnav" target="frmPage">Forecast Response Category</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="forecast/forecast_responses.aspx" class="leftnav" target="frmPage">Forecast Responses</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="forecast/forecast_line_items.aspx" class="leftnav" target="frmPage">Forecast Line Items</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="forecast/forecast_steps.aspx" class="leftnav" target="frmPage">Forecast Steps</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="forecast/forecast_steps_additional.aspx" class="leftnav" target="frmPage">Forecast Steps (Additional)</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/spacer.gif" border="0" width="1" height="1" /></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="forecast/forecast_acquisition_costs.aspx" class="leftnav" target="frmPage">Acquisition Costs</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="forecast/forecast_operational_costs.aspx" class="leftnav" target="frmPage">Operational Costs</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="forecast/forecast_street_value_costs.aspx" class="leftnav" target="frmPage">Street Value Costs</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/spacer.gif" border="0" width="1" height="1" /></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="forecast/solution_codes.aspx" class="leftnav" target="frmPage">Solution Codes</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="forecast/solution_codes_locations.aspx" class="leftnav" target="frmPage">Solution Code Locations</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="forecast/build_locations.aspx" class="leftnav" target="frmPage">Build Locations</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="forecast/build_locations_rdp.aspx" class="leftnav" target="frmPage">Build Locations RDP</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="forecast/solaris_build_networks.aspx" class="leftnav" target="frmPage">Solaris Build Networks</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/spacer.gif" border="0" width="1" height="1" /></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="forecast/wizard_steps.aspx" class="leftnav" target="frmPage">On Demand Wizard Steps</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="forecast/steps.aspx" class="leftnav" target="frmPage">On Demand Steps</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="forecast/ondemand_sending.aspx" class="leftnav" target="frmPage">On Demand Sending Config</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/spacer.gif" border="0" width="1" height="1" /></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="forecast/override.aspx" class="leftnav" target="frmPage">Override Code</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="forecast/reset_storage.aspx" class="leftnav" target="frmPage">Reset Storage Code</a></td>
                                </tr>
                                <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="forecast/unlock_100.aspx" class="leftnav" target="frmPage">Unlock 100% Confidence Code</a></td>
                                </tr>
                                  <tr>
                                    <td><img src="/images/menu.gif" border="0" align="absmiddle" /> <a href="forecast/storage_override.aspx" class="leftnav" target="frmPage">Storage Override Code</a></td>
                                </tr>
                                <tr>
                                    <td><p>&nbsp;</p><p>&nbsp;</p><p>&nbsp;</p></td>
                                </tr>
                            </table>
                        </div>
                        <div id="divTab" style="display:none">
                            <table width="280" border="0" cellspacing="2" cellpadding="2">
                                <tr>
                                    <td><b>Welcome to ClearView Administration!</b></td>
                                </tr>
                                <tr>
                                    <td>Select from the above menu to administrate a part of ClearView.</td>
                                </tr>
                                <tr>
                                    <td>Server: <b><%=Environment.MachineName %></b></td>
                                </tr>
                                <tr>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td><b>Recent Code Modifications</b></td>
                                </tr>
                                <tr>
                                    <td>
                                        <table cellpadding="2" cellspacing="2" border="0">
                                            <tr>
                                                <td><u>Version:</u></td>
                                                <td><u>Date:</u></td>
                                                <td><u>Status:</u></td>
                                            </tr>
                                            <%=strPush %>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td><p>&nbsp;</p><p>&nbsp;</p><p>&nbsp;</p></td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </td>
            </tr>
        </table>
	</td>
    <td width="100%">
        <iframe id="frmPage" name="frmPage" frameborder="0" scrolling="yes" width="100%" height="100%" />
	</td>
  </tr>
</table>
<iframe id="frmCover" frameborder="0" scrolling="no" style="display:none;position:absolute;FILTER:alpha(opacity=0)"></iframe>
<div id="divLeftMenu" style="display:none; position:absolute; height:100%; background-color:#002753;FILTER:alpha(opacity=75);"></div>
<iframe id="frmLeftMenu" frameborder="0" scrolling="no" style="display:none;position:absolute;"></iframe>
<div id="divLeftMenu2" style="display:none; position:absolute; height:100%; background-color:#002753;FILTER:alpha(opacity=75);"></div>
<iframe id="frmLeftMenu2" frameborder="0" scrolling="no" style="display:none;position:absolute;"></iframe>
</form>
</body>
</html>
