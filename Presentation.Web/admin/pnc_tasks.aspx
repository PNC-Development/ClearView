<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="pnc_tasks.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.pnc_tasks" %>

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
		    <td><b>Pre-Production Tasks</b></td>
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
                                                <td width="50%" class="bold"><asp:linkbutton ID="lnkName" Text="Name" CommandArgument="name" CssClass="bold" OnClick="OrderView" runat="server"/></td>
                                                <td width="10%" class="bold"><asp:linkbutton ID="lnkStep" Text="Step" CommandArgument="step" CssClass="bold" OnClick="OrderView" runat="server"/></td>
                                                <td class="bold"><asp:linkbutton ID="lnkVirtual" Text="Virtual" CommandArgument="if_virtual" CssClass="bold" OnClick="OrderView" runat="server"/></td>
                                                <td class="bold"><asp:linkbutton ID="lnkPhysical" Text="Physical" CommandArgument="if_physical" CssClass="bold" OnClick="OrderView" runat="server"/></td>
                                                <td class="bold"><asp:linkbutton ID="lnkDistributed" Text="Distributed" CommandArgument="distributed" CssClass="bold" OnClick="OrderView" runat="server"/></td>
                                                <td class="bold"><asp:linkbutton ID="lnkMidrange" Text="Midrange" CommandArgument="midrange" CssClass="bold" OnClick="OrderView" runat="server"/></td>
                                                <td class="bold"><asp:linkbutton ID="lnkNonTransparent" Text="NonTransparent" CommandArgument="non_transparent" CssClass="bold" OnClick="OrderView" runat="server"/></td>
                                                <td class="bold"><asp:linkbutton ID="lnkDecom" Text="Decom" CommandArgument="decom" CssClass="bold" OnClick="OrderView" runat="server"/></td>
                                                <td class="bold" nowrap>Configuration</td>
                                                <td width="40%" class="bold"><asp:linkbutton ID="lnkModified" Text="Last Modified" CommandArgument="modified" CssClass="bold" OnClick="OrderView" runat="server"/></td>
                                                <td class="bold" align="center"><asp:linkbutton ID="lnkEnabled" Text="Enabled" CommandArgument="enabled" CssClass="bold" OnClick="OrderView" runat="server"/></td>
                                            </tr>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr bgcolor="#EFEFEF" onmouseover="AdminMouseOver(this);" onmouseout="AdminMouseOut(this);" onclick="if(event.srcElement.tagName != 'INPUT')window.navigate('<%# Request.Path + "?id=" + DataBinder.Eval(Container.DataItem, "id")%>');" class="default">
                                            <td width="5%" align="left">&nbsp;<asp:ImageButton ID="btnDelete" OnClick="btnDeleteLink_Click" runat="server" ToolTip="Delete" ImageAlign="AbsMiddle" ImageUrl="/admin/images/delete.gif" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' /></td>
                                            <td width="50%">&nbsp;<%# DataBinder.Eval(Container.DataItem, "name") %> [<%# DataBinder.Eval(Container.DataItem, "serviceid") %>]</td>
                                            <td width="10%">&nbsp;<%# DataBinder.Eval(Container.DataItem, "step") %>.<%# DataBinder.Eval(Container.DataItem, "substep") %></td>
                                            <td align="center"><img src='<%# (DataBinder.Eval(Container.DataItem, "if_virtual").ToString() == "1") ? "/images/go.gif" : "/images/cancel.gif" %>' align="absmiddle" border="0" /></td>
                                            <td align="center"><img src='<%# (DataBinder.Eval(Container.DataItem, "if_physical").ToString() == "1") ? "/images/go.gif" : "/images/cancel.gif" %>' align="absmiddle" border="0" /></td>
                                            <td align="center"><img src='<%# (DataBinder.Eval(Container.DataItem, "distributed").ToString() == "1") ? "/images/go.gif" : "/images/cancel.gif" %>' align="absmiddle" border="0" /></td>
                                            <td align="center"><img src='<%# (DataBinder.Eval(Container.DataItem, "midrange").ToString() == "1") ? "/images/go.gif" : "/images/cancel.gif" %>' align="absmiddle" border="0" /></td>
                                            <td align="center"><img src='<%# (DataBinder.Eval(Container.DataItem, "non_transparent").ToString() == "1") ? "/images/go.gif" : "/images/cancel.gif" %>' align="absmiddle" border="0" /></td>
                                            <td align="center"><img src='<%# (DataBinder.Eval(Container.DataItem, "decom").ToString() == "1") ? "/images/go.gif" : "/images/cancel.gif" %>' align="absmiddle" border="0" /></td>
                                            <td nowrap><asp:Label ID="lblCustom" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "id") %>' /></td>
                                            <td width="40%">&nbsp;<%# DataBinder.Eval(Container.DataItem, "modified") %></td>
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
                            <td nowrap class="cmdefault">Service:</td>
                            <td width="100%"><asp:label ID="lblParent" CssClass="default" runat="server" Text="Please select a service..." />&nbsp;&nbsp;<asp:Button ID="btnParent" runat="server" CssClass="default" Width="25" Text="..." /></td>
                        </tr>
                        <tr> 
                            <td nowrap class="default">Class:</td>
                            <td>
                                <asp:CheckBox ID="chkPNC" runat="server" Text="PNC" /> 
                                <asp:CheckBox ID="chkNCB" runat="server" Text="NCB" /> 
                            </td>
                        </tr>
                        <tr> 
                            <td nowrap class="default">Platform:</td>
                            <td>
                                <asp:CheckBox ID="chkDistributed" runat="server" Text="Distributed" /> 
                                <asp:CheckBox ID="chkMidrange" runat="server" Text="Midrange" /> 
                            </td>
                        </tr>
                        <tr> 
                            <td nowrap class="default">Virtualization:</td>
                            <td>
                                <asp:CheckBox ID="chkVirtual" runat="server" Text="Virtual" /> 
                                <asp:CheckBox ID="chkPhysical" runat="server" Text="Physical (non-virtual)" /> 
                            </td>
                        </tr>
                        <tr> 
                            <td nowrap class="default">Step:</td>
                            <td><asp:TextBox ID="txtStep" runat="server" CssClass="default" Width="100" Text="1" /></td>
                        </tr>
                        <tr> 
                            <td nowrap class="default">Sub-Step:</td>
                            <td><asp:TextBox ID="txtSubStep" runat="server" CssClass="default" Width="100" Text="1" /> [ordering within steps]</td>
                        </tr>
                        <tr> 
                            <td nowrap class="default">Offsite:</td>
                            <td><asp:CheckBox ID="chkOffsite" runat="server" /> [check if this service is ONLY valid for offsite servers]</td>
                        </tr>
                        <tr> 
                            <td nowrap class="default">Non-Transparent:</td>
                            <td><asp:CheckBox ID="chkNonTransparent" runat="server" /> [check if this service is NOT dependant for the commission / decommission process to be completed]</td>
                        </tr>
                        <tr> 
                            <td nowrap class="default">Client (MIS):</td>
                            <td><asp:CheckBox ID="chkClient" runat="server" /> [check if this service is the MIS approval step - used for time tracking]</td>
                        </tr>
                        <tr> 
                            <td nowrap class="default">Available for Decom:</td>
                            <td><asp:CheckBox ID="chkDecom" runat="server" /> [check if this service should be submitted during a decommission]</td>
                        </tr>
                        <tr> 
                            <td></td>
                            <td>
                                <table width="600" cellpadding="5" cellspacing="5" border="0">
                                    <tr>
                                        <td width="300" valign="top">
                                            <fieldset>
                                                <legend>Associated Function / Service:</legend>
                                                <asp:CheckBox ID="chkTSM" runat="server" Text="Backup (TSM)" /> <br />
                                                <asp:CheckBox ID="chkLegato" runat="server" Text="Backup (Avamar / Legato)" /> <br />
                                                <asp:CheckBox ID="chkIfCitrix" runat="server" Text="Citrix" /><br />
                                                <asp:CheckBox ID="chkIfCluster" runat="server" Text="Cluster" /><br />
                                                <asp:CheckBox ID="chkDNS" runat="server" Text="DNS" /> <br />
                                                <asp:CheckBox ID="chkIfLTMcfg" runat="server" Text="LTM Servers (Config)" /><br />
                                                <asp:CheckBox ID="chkIfLTMins" runat="server" Text="LTM Servers (Install)" /> <br />
                                                <asp:CheckBox ID="chkIfSQL" runat="server" Text="SQL" /><br />
                                                <asp:CheckBox ID="chkStorage" runat="server" Text="Storage" /> <br />
                                            </fieldset>
                                        </td>
                                        <td width="300" valign="top">
                                            <fieldset>
                                                <legend>Assignment:</legend>
                                                <asp:RadioButton ID="chkAdministrativeContact" runat="server" Text="Administrative Contact" GroupName="assignment" /><br />
                                                <asp:RadioButton ID="chkApplicationLead" runat="server" Text="Application Lead" GroupName="assignment" /><br />
                                                <asp:RadioButton ID="chkApplicationOwner" runat="server" Text="Application Owner" GroupName="assignment" /><br />
                                                <asp:RadioButton ID="chkDBA" runat="server" Text="Database Administrator" GroupName="assignment" /><br />
                                                <asp:RadioButton ID="chkDepartmentalManager" runat="server" Text="Departmental Manager" GroupName="assignment" /><br />
                                                <asp:RadioButton ID="chkRequestor" runat="server" Text="Design Executor" GroupName="assignment" /><br />
                                                <asp:RadioButton ID="chkImplementor" runat="server" Text="Implementor" GroupName="assignment" /><br />
                                                <asp:RadioButton ID="chkNetworkEngineer" runat="server" Text="Network Engineer" GroupName="assignment" /><br />
                                                <asp:RadioButton ID="chkProjectManager" runat="server" Text="Project Manager" GroupName="assignment" /><br />
                                                <br />
                                                <asp:RadioButton ID="chkService" runat="server" Text="Inherit from Service" GroupName="assignment" />
                                            </fieldset>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr> 
                            <td nowrap class="default">Enabled:</td>
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
    <input type="hidden" id="hdnParent" runat="server" value="0" />
</form>
</body>
</html>
