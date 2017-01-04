<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="services.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.services" %>


<script type="text/javascript">
    function OpenServiceReport(strURL) {
        window.open("/frame/loading.htm?referrer=" + strURL,"_blank","height=600,width=800,menubar=no,resizable=yes,scrollbars=no,status=no,toolbar=no");
        return false;
    }
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
		    <td><b>Services</b></td>
		    <td align="right"><asp:LinkButton ID="btnReport" runat="server" Text="Service Report" />&nbsp;&nbsp;&nbsp;<asp:LinkButton ID="btnExpand" runat="server" OnClick="btnExpand_Click" /></td>
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
                            <td class="default" width="100px">Name:</td>
                            <td><asp:textbox ID="txtName" CssClass="default" runat="server" Width="300" MaxLength="100"/></td>
                        </tr>
                        <tr> 
                            <td class="default" width="100px" valign="top">Description:</td>
                            <td><asp:textbox ID="txtDescription" CssClass="default" runat="server" Width="300" TextMode="MultiLine" Rows="5" /></td>
                        </tr>
                        <tr> 
                            <td class="default">Image:</td>
                            <td><asp:textbox ID="txtImage" CssClass="default" runat="server" Width="300" MaxLength="100"/> <asp:Button ID="btnImage" runat="server" CssClass="default" Text="..." /></td>
                        </tr>
                        <tr>
                            <td class="default">Item:</td>
                            <td><asp:label ID="lblItem" CssClass="default" runat="server" />&nbsp;&nbsp;<asp:Button ID="btnItem" runat="server" CssClass="default" Text="..." /></td>
                        </tr>
                        <tr>
                            <td class="default">Document:</td>
                            <td><asp:label ID="lblDocument" CssClass="default" runat="server" />&nbsp;&nbsp;<asp:Button ID="btnDocument" runat="server" CssClass="default" Text="..." /></td>
                        </tr>
                        <tr>
                            <td class="default">Type:</td>
                            <td><asp:dropdownlist ID="ddlType" CssClass="default" runat="server"/></td>
                        </tr>
                        <tr>
                            <td class="default">Show:</td>
                            <td><asp:CheckBox ID="chkShow" runat="server" Checked="true" /></td>
                        </tr>
                        <tr> 
                            <td class="default">Project Required:</td>
                            <td><asp:CheckBox ID="chkProject" runat="server" Checked="true" /></td>
                        </tr>
                        <tr> 
                            <td class="default">Hours:</td>
                            <td><asp:textbox ID="txtHours" CssClass="default" runat="server" Width="50" MaxLength="10"/></td>
                        </tr>
                        <tr> 
                            <td class="default">SLA:</td>
                            <td><asp:textbox ID="txtSLA" CssClass="default" runat="server" Width="50" MaxLength="10"/> [In Hours]</td>
                        </tr>
                        <tr> 
                            <td class="default">Can Automate:</td>
                            <td><asp:CheckBox ID="chkCanAuto" runat="server" Checked="true" /></td>
                        </tr>
                        <tr> 
                            <td class="default">Show Statement of Work:</td>
                            <td><asp:CheckBox ID="chkStatement" runat="server" Checked="true" /></td>
                        </tr>
                        <tr> 
                            <td class="default">Show Upload Button:</td>
                            <td><asp:CheckBox ID="chkUpload" runat="server" Checked="true" /></td>
                        </tr>
                        <tr> 
                            <td class="default">Show Expedite:</td>
                            <td><asp:CheckBox ID="chkExpedite" runat="server" Checked="true" /></td>
                        </tr>
                        <tr> 
                            <td class="default">RR Path:</td>
                            <td><asp:textbox ID="txtRRPath" CssClass="default" runat="server" Width="400" MaxLength="100"/> <asp:button ID="btnRRBrowse" runat="server" Text="..." Width="25" CssClass="default"/></td>
                        </tr>
                        <tr> 
                            <td class="default">WM Path:</td>
                            <td><asp:textbox ID="txtWMPath" CssClass="default" runat="server" Width="400" MaxLength="100"/> <asp:button ID="btnWMBrowse" runat="server" Text="..." Width="25" CssClass="default"/></td>
                        </tr>
                        <tr> 
                            <td class="default">CP Path:</td>
                            <td><asp:textbox ID="txtCPPath" CssClass="default" runat="server" Width="400" MaxLength="100"/> <asp:button ID="btnCPBrowse" runat="server" Text="..." Width="25" CssClass="default"/></td>
                        </tr>
                        <tr> 
                            <td class="default">CA Path:</td>
                            <td><asp:textbox ID="txtCAPath" CssClass="default" runat="server" Width="400" MaxLength="100"/> <asp:button ID="btnCABrowse" runat="server" Text="..." Width="25" CssClass="default"/></td>
                        </tr>
                        <tr> 
                            <td class="default">Enable Rejection:</td>
                            <td><asp:CheckBox ID="chkRejection" runat="server" /></td>
                        </tr>
                        <tr> 
                            <td class="default">Automated:</td>
                            <td><asp:CheckBox ID="chkAutomate" runat="server" /></td>
                        </tr>
                        <tr> 
                            <td class="default">Disable Hours:</td>
                            <td><asp:CheckBox ID="chkHours" runat="server" /> [disable the modification of hours when assigning]</td>
                        </tr>
                        <tr> 
                            <td class="default">Manager Approval:</td>
                            <td><asp:CheckBox ID="chkManagerApprove" runat="server" /> [Requires the requestor's manager to approve to submit the service]</td>
                        </tr>
                        <tr> 
                            <td class="default">Quantity IS Device:</td>
                            <td><asp:CheckBox ID="chkQuantityDevice" runat="server" /> [Makes the quantity equal to the device count, only one form for the service]</td>
                        </tr>
                        <tr> 
                            <td class="default">Quantity Multiple:</td>
                            <td><asp:CheckBox ID="chkQuantityMultiple" runat="server" /> [Allows the user to change the quantity when checking out of shopping cart]</td>
                        </tr>
                        <tr> 
                            <td class="default">Notify PC:</td>
                            <td><asp:CheckBox ID="chkNotifyPC" runat="server" /> [Notify the Project Coordinator to add clarity hours]</td>
                        </tr>
                        <tr> 
                            <td class="default">Notify Client:</td>
                            <td><asp:CheckBox ID="chkNotifyClient" runat="server" /> [Notify the Client when a resource has been assigned to this service]</td>
                        </tr>
                        <tr> 
                            <td class="default">Disable Customization:</td>
                            <td><asp:CheckBox ID="chkDisable" CssClass="default" runat="server" /></td>
                        </tr>
                        <tr> 
                            <td class="default">Use Tasks:</td>
                            <td><asp:CheckBox ID="chkTask" CssClass="default" runat="server" /> [Check this box to enable the workload manager to show multiple tasks for same project]</td>
                        </tr>
                        <tr> 
                            <td class="default">Email:</td>
                            <td><asp:textbox ID="txtEmail" CssClass="default" runat="server" Width="200" MaxLength="50"/> [Group Mailbox for Assignment - use the full email address - Example: GM2768P@pnc.com]</td>
                        </tr>
                        <tr> 
                            <td class="default">Multiple Workflow - Completion:</td>
                            <td><asp:CheckBox ID="chkSameTime" runat="server" /> [Checked = Collective (all services completed before any subsequent workflows initiated), Unchecked = Individual]</td>
                        </tr>
                        <tr> 
                            <td class="default">Notify RED:</td>
                            <td><asp:CheckBox ID="chkNotifyRed" runat="server" /></td>
                        </tr>
                        <tr> 
                            <td class="default">Notify YELLOW:</td>
                            <td><asp:CheckBox ID="chkNotifyYellow" runat="server" /></td>
                        </tr>
                        <tr> 
                            <td class="default">Notify Back to GREEN:</td>
                            <td><asp:CheckBox ID="chkNotifyGreen" runat="server" /></td>
                        </tr>
                        <tr> 
                            <td class="default">Workflow Enabled:</td>
                            <td><asp:CheckBox ID="chkWorkflow" runat="server" /></td>
                        </tr>
                        <tr> 
                            <td class="default">Workflow Title:</td>
                            <td><asp:textbox ID="txtWorkflow" CssClass="default" runat="server" Width="300" MaxLength="100"/></td>
                        </tr>
                        <tr> 
                            <td class="default">Workflow Connectable:</td>
                            <td><asp:CheckBox ID="chkWorkflowConnectable" runat="server" /></td>
                        </tr>
                        <tr> 
                            <td class="default">Workflow Same Technician:</td>
                            <td><asp:CheckBox ID="chkWorkflowSameTechnician" runat="server" /></td>
                        </tr>
                        <tr> 
                            <td class="default">Restrictions Enabled:</td>
                            <td><asp:CheckBox ID="chkRestrictions" runat="server" /></td>
                        </tr>
                        <tr> 
                            <td class="default">Title Override:</td>
                            <td><asp:CheckBox ID="chkTitleOverrride" runat="server" /></td>
                        </tr>
                        <tr> 
                            <td class="default">Title Override Name:</td>
                            <td><asp:textbox ID="txtTitleName" CssClass="default" runat="server" Width="300" MaxLength="100"/></td>
                        </tr>
                        <tr> 
                            <td class="default">No Slider:</td>
                            <td><asp:CheckBox ID="chkNoSlider" runat="server" /></td>
                        </tr>
                        <tr> 
                            <td class="default">Hide SLA:</td>
                            <td><asp:CheckBox ID="chkHideSLA" runat="server" /></td>
                        </tr>
                        <tr> 
                            <td class="default">Requires Approval:</td>
                            <td><asp:CheckBox ID="chkApproval" runat="server" /> [Must be checked for APPROVAL process to be initiated (not needed for manager approval)]</td>
                        </tr>
                        <tr> 
                            <td class="default">Workflow Assignment:</td>
                            <td><asp:DropDownList ID="ddlWorkflowAssignment" runat="server" /> [Previous workflows that are being sent to this service]</td>
                        </tr>
                        <tr> 
                            <td class="default">Notify Completion:</td>
                            <td><asp:textbox ID="txtNotifyComplete" CssClass="default" runat="server" Width="400" Rows="3" TextMode="MultiLine" /> [Full email address(es) separated by &quot;;&quot;]</td>
                        </tr>
                        <tr> 
                            <td class="default">Enabled:</td>
                            <td><asp:CheckBox ID="chkEnabled" runat="server" Checked="true" /></td>
                        </tr>
                        <tr> 
                            <td class="cmdefault">&nbsp;</td>
                            <td><asp:Button ID="btnUsers" runat="server" Text="Edit User Rights" Width="150" CssClass="default" /></td>
                        </tr>
                        <tr> 
                            <td class="cmdefault">&nbsp;</td>
                            <td><asp:Button ID="btnRoles" runat="server" Text="Edit Details" Width="150" CssClass="default" /></td>
                        </tr>
                        <tr> 
                            <td class="cmdefault">&nbsp;</td>
                            <td><asp:Button ID="btnLocation" runat="server" Text="Edit Locations" Width="150" CssClass="default" /></td>
                        </tr>
                        <tr> 
                            <td class="cmdefault">&nbsp;</td>
                            <td><asp:Button ID="btnOrder" runat="server" Text="Change Order" Width="150" CssClass="default" /></td>
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
    <input type="hidden" id="hdnOrderId" runat="server" />
    <input type="hidden" id="hdnItem" runat="server" />
    <input type="hidden" id="hdnDocument" runat="server" />
    <input type="hidden" id="hdnOrder" runat="server" />
    <input type="hidden" id="hdnParent" runat="server" />
</form>
</body>
</html>
