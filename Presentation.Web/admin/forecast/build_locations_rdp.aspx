<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="build_locations_rdp.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.build_locations_rdp" %>

<script type="text/javascript">
</script>
<html>
<head>
<title>ClearView Administration</title>
<link rel="stylesheet" type="text/css" href="/css/default.css" />
<script type="text/javascript" src="/javascript/both.js"></script>
<script type="text/javascript" src="/javascript/global.js"></script>
<script type="text/javascript" src="/javascript/ajax.js"></script>
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
		    <td><b>Build Locations (RDP)</b></td>
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
                                                <td class="bold">Location</td>
                                                <td class="bold" nowrap>Server / MDT</td>
                                                <td class="bold" nowrap>Server / Altiris</td>
                                                <td class="bold" nowrap>Workstation</td>
                                            </tr>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr bgcolor="#EFEFEF" onmouseover="AdminMouseOver(this);" onmouseout="AdminMouseOut(this);" onclick="window.navigate('<%# Request.Path + "?id=" + DataBinder.Eval(Container.DataItem, "id")%>');" class="default">
                                            <td width="5%" align="left">&nbsp;<asp:ImageButton ID="btnDelete" OnClick="btnDeleteLink_Click" runat="server" ToolTip="Delete" ImageAlign="AbsMiddle" ImageUrl="/admin/images/delete.gif" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' /></td>
                                            <td width="100%" nowrap>&nbsp;<%# DataBinder.Eval(Container.DataItem, "class") %> / <%# DataBinder.Eval(Container.DataItem, "environment") %> / <%# DataBinder.Eval(Container.DataItem, "location") %></td>
                                            <td align="center"><img src='<%# (DataBinder.Eval(Container.DataItem, "server_mdt").ToString() == "1") ? "/images/go.gif" : "/images/cancel.gif" %>' align="absmiddle" border="0" /></td>
                                            <td align="center"><img src='<%# (DataBinder.Eval(Container.DataItem, "server_altiris").ToString() == "1") ? "/images/go.gif" : "/images/cancel.gif" %>' align="absmiddle" border="0" /></td>
                                            <td align="center"><img src='<%# (DataBinder.Eval(Container.DataItem, "workstation").ToString() == "1") ? "/images/go.gif" : "/images/cancel.gif" %>' align="absmiddle" border="0" /></td>
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
                            <td class="default">Build Class:</td>
                            <td><asp:dropdownlist ID="ddlClass" CssClass="default" runat="server" Width="300"/></td>
                        </tr>
                        <tr> 
                            <td class="default">Build Environment:</td>
                            <td>
                                <asp:dropdownlist ID="ddlEnvironment" CssClass="default" runat="server" Enabled="false" Width="300">
                                    <asp:ListItem Value="-- Please select a Class --" />
                                </asp:dropdownlist>
                            </td>
                        </tr>
                        <tr>
                            <td class="cmdefault">Build Location:</td>
                            <td><%=strLocation %></td>
                        </tr>
                        <tr>
                            <td class="cmdefault">RDP Web Service URL (Schedule):</td>
                            <td><asp:TextBox ID="txtRDP_Schedule_WS" CssClass="default" runat="server" Width="600" MaxLength="100" /></td>
                        </tr>
                        <tr>
                            <td class="cmdefault">RDP Web Service URL (Computer):</td>
                            <td><asp:TextBox ID="txtRDP_Computer_WS" CssClass="default" runat="server" Width="600" MaxLength="100" /></td>
                        </tr>
                        <tr>
                            <td class="cmdefault">RDP Web Service URL (MDT):</td>
                            <td><asp:TextBox ID="txtRDP_MDT_WS" CssClass="default" runat="server" Width="600" MaxLength="100" /></td>
                        </tr>
                        <tr>
                            <td class="cmdefault">Jumpstart CGI URL:</td>
                            <td><asp:TextBox ID="txtJumpstartCGI" CssClass="default" runat="server" Width="600" MaxLength="100" /></td>
                        </tr>
                        <tr>
                            <td class="cmdefault">Jumpstart Build Type:</td>
                            <td>
                                <asp:DropDownList ID="ddlJumpstartBuildType" CssClass="default" runat="server" Width="200" >
                                    <asp:ListItem Value="dhcp" />
                                    <asp:ListItem Value="bootp" />
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="cmdefault">Blade VLAN:</td>
                            <td><asp:TextBox ID="txtBladeVLAN" CssClass="default" runat="server" Width="200" MaxLength="50" /> ** Use the wildcard $ to inherit from Zone VLANs.&nbsp;&nbsp;&nbsp;(Example: VLAN_250 = VLAN_$)</td>
                        </tr>
                        <tr>
                            <td class="cmdefault">VMware VLAN:</td>
                            <td><asp:TextBox ID="txtVMwareVLAN" CssClass="default" runat="server" Width="200" MaxLength="50" /> ** Use the wildcard $ to inherit from Zone VLANs.&nbsp;&nbsp;&nbsp;(Example: vlan250net = vlan$net)</td>
                        </tr>
                        <tr>
                            <td class="cmdefault">VSphere VLAN:</td>
                            <td><asp:TextBox ID="txtVSphereVLAN" CssClass="default" runat="server" Width="200" MaxLength="50" /> ** Use the wildcard $ to inherit from Zone VLANs.&nbsp;&nbsp;&nbsp;(Example: dvPortGroup250 = dvPortGroup$)</td>
                        </tr>
                        <tr>
                            <td class="cmdefault">DELL VLAN:</td>
                            <td><asp:TextBox ID="txtDellVLAN" CssClass="default" runat="server" Width="200" MaxLength="50" /> ** Use the wildcard $ to inherit from Zone VLANs.&nbsp;&nbsp;&nbsp;(Example: dvPortGroup250 = dvPortGroup$)</td>
                        </tr>
                        <tr>
                            <td class="cmdefault">DELL VMware VLAN:</td>
                            <td><asp:TextBox ID="txtDellVmwareVLAN" CssClass="default" runat="server" Width="200" MaxLength="50" /> ** Use the wildcard $ to inherit from Zone VLANs.&nbsp;&nbsp;&nbsp;(Example: dvPortGroup250 = dvPortGroup$)</td>
                        </tr>
                        <tr>
                            <td nowrap>Resiliency:</td>
                            <td><asp:DropDownList ID="ddlResiliency" runat="server" CssClass="default" Width="400" /></td>
                        </tr>
                        <tr> 
                            <td class="default" width="100px">ZEUS Source:</td>
                            <td>
                                <asp:DropDownList ID="ddlSource" CssClass="default" runat="server" Width="200">
                                    <asp:ListItem Value="SERVER" Text="Cleveland, Test" />
                                    <asp:ListItem Value="CLERDP" Text="Cleveland, Prod and QA" />
                                    <asp:ListItem Value="DALRDP" Text="Dalton, Test ONLY" />
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr> 
                            <td class="default">RDP:</td>
                            <td>
                                <table cellpadding="2" cellspacing="1" border="0">
                                    <tr>
                                        <td><asp:RadioButton ID="chkServerAltiris" runat="server" CssClass="default" Text="Server / Altiris" GroupName="type" /></td>
                                        <td><asp:RadioButton ID="chkServerMDT" runat="server" CssClass="default" Text="Server / MDT" GroupName="type" /></td>
                                        <td><asp:RadioButton ID="chkWorkstation" runat="server" CssClass="default" Text="Workstation" GroupName="type" /></td>
                                        <td><asp:RadioButton ID="chkNone" runat="server" CssClass="default" Text="None" GroupName="type" /></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr> 
                            <td class="default">Specify Individual Zone(s):</td>
                            <td><asp:CheckBox ID="chkZones" runat="server" /></td>
                        </tr>
                        <tr> 
                            <td class="default">Enabled:</td>
                            <td><asp:CheckBox ID="chkEnabled" runat="server" Checked="true" /></td>
                        </tr>
                        <tr><td height="5" colspan="2"><img src="images/spacer.gif" width="1" height="5" /></td></tr>
                        <tr> 
                            <td class="cmdefault">&nbsp;</td>
                            <td><asp:Button ID="btnZones" runat="server" Text="Configure Zones" Width="150" CssClass="default" /></td>
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
    <input type="hidden" id="hdnEnvironment" runat="server" />
    <input type="hidden" id="hdnLocation" runat="server" />
</form>
</body>
</html>

