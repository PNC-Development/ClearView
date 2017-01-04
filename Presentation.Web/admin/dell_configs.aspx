<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="dell_configs.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.dell_configs" %>



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
		    <td><b>Dell Configs</b></td>
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
                                                <td width="50%" class="bold"><asp:linkbutton ID="lnkModified" Text="Last Modified" CommandArgument="modified" CssClass="bold" OnClick="OrderView" runat="server"/></td>
                                                <td class="bold" align="center"><asp:linkbutton ID="lnkEnabled" Text="Enabled" CommandArgument="enabled" CssClass="bold" OnClick="OrderView" runat="server"/></td>
                                            </tr>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr bgcolor="#EFEFEF" onmouseover="AdminMouseOver(this);" onmouseout="AdminMouseOut(this);" onclick="window.navigate('<%# Request.Path + "?id=" + DataBinder.Eval(Container.DataItem, "id")%>');" class="default">
                                            <td width="5%" align="left">&nbsp;<asp:ImageButton ID="btnDelete" OnClick="btnDeleteLink_Click" runat="server" ToolTip="Delete" ImageAlign="AbsMiddle" ImageUrl="/admin/images/delete.gif" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' /></td>
                                            <td width="50%">&nbsp;<%# DataBinder.Eval(Container.DataItem, "name") %></td>
                                            <td width="50%">&nbsp;<%# DataBinder.Eval(Container.DataItem, "modified") %></td>
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
                            <td class="default">Name:</td>
                            <td><asp:textbox ID="txtName" CssClass="default" runat="server" Width="300" MaxLength="50"/></td>
                        </tr>
                        <tr> 
                            <td class="default">XML Split:</td>
                            <td><asp:textbox ID="txtSplit" CssClass="default" runat="server" Width="100" MaxLength="50"/> (The character(s) that signify the end of one value set and beginning of the next)</td>
                        </tr>
                        <tr> 
                            <td class="default">XML Operator:</td>
                            <td><asp:textbox ID="txtOperator" CssClass="default" runat="server" Width="100" MaxLength="50"/> (The character(s) that separate the label from the value)</td>
                        </tr>
                        <tr> 
                            <td class="default">Username:</td>
                            <td><asp:textbox ID="txtUsername" CssClass="default" runat="server" Width="200" MaxLength="100"/> (Optional)</td>
                        </tr>
                        <tr> 
                            <td class="default">Password:</td>
                            <td><asp:textbox ID="txtPassword" CssClass="default" runat="server" Width="200" MaxLength="100"/> (Optional)</td>
                        </tr>
                        <tr>
                            <td></td>
                            <td class="framegreen">The following can have multiple values separated by a semi-colon (;)</td>
                        </tr>
                        <tr> 
                            <td class="default">XML Start(s):</td>
                            <td><asp:textbox ID="txtStart" CssClass="default" runat="server" Width="500" /></td>
                        </tr>
                        <tr> 
                            <td class="default">Query (Power):</td>
                            <td><asp:textbox ID="txtQueryPower" CssClass="default" runat="server" Width="500" /></td>
                        </tr>
                        <tr> 
                            <td class="default">Query (MAC #1):</td>
                            <td><asp:textbox ID="txtQueryMAC1" CssClass="default" runat="server" Width="500" /></td>
                        </tr>
                        <tr> 
                            <td class="default">Query (MAC #2):</td>
                            <td><asp:textbox ID="txtQueryMAC2" CssClass="default" runat="server" Width="500" /></td>
                        </tr>
                        <tr> 
                            <td class="default">Power ON Success:</td>
                            <td><asp:textbox ID="txtPowerOn" CssClass="default" runat="server" Width="500" /></td>
                        </tr>
                        <tr> 
                            <td class="default">Power OFF Success:</td>
                            <td><asp:textbox ID="txtPowerOff" CssClass="default" runat="server" Width="500" /></td>
                        </tr>
                        <tr>
                            <td></td>
                            <td><b>NOTE:</b> When specifying multiple values, ClearView will check for the existance of ANY of them.</td>
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
                        <tr>
                            <td></td>
                            <td>
                                <p>&nbsp;</p>
                                <p class="bold">EXAMPLE:</p>
                                <p>
<span class="note">System Information:</span><br />
System Model&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;= PowerEdge M610<br />
System Revision&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;= I<br />
System BIOS Version&nbsp;&nbsp;= 2.2.9<br />
BMC Firmware Version = 02.30<br />
Service Tag&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; = JHKLTL1<br />
Host Name&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;= <br />
OS Name&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;  = <br />
<span class="note">Power Status</span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;= OFF<br />
<br />
Embedded NIC MAC Addresses:<br />
<span class="note">NIC1 Ethernet</span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;= 00:26:B9:F9:41:08<br />
iSCSI&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; = 00:26:B9:F9:41:09<br />
NIC2 Ethernet&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;= 00:26:B9:F9:41:0A<br />
iSCSI&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; = 00:26:B9:F9:41:0B<br />
NIC3 Ethernet&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;= N/A<br />
iSCSI&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; = N/A<br />
NIC4 Ethernet&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;= N/A<br />
iSCSI&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; = N/A<br />
                                </p>
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
</form>
</body>
</html>
