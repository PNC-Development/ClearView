<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="workstation_components.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.workstation_components" %>


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
		    <td><b>Workstation Components</b></td>
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
                            <td><asp:textbox ID="txtName" CssClass="default" runat="server" Width="300" MaxLength="100"/></td>
                        </tr>
                        <tr> 
                            <td class="default">ZEUS Build Type:</td>
                            <td><asp:textbox ID="txtZEUSBuildType" CssClass="default" runat="server" Width="150" MaxLength="50"/></td>
                        </tr>
                        <tr> 
                            <td class="default">AD Move Location:</td>
                            <td><asp:textbox ID="txtADMoveLocation" CssClass="default" runat="server" Width="500" MaxLength="100" /></td>
                        </tr>
                        <tr> 
                            <td class="default"></td>
                            <td class="footer">EXAMPLE: OU=OUs_SQL,OU=OUc_Srvs,OU=OUc_DmnCptrs,</td>
                        </tr>
                        <tr> 
                            <td class="default">SMS Install:</td>
                            <td><asp:CheckBox ID="chkSMSInstall" runat="server" Checked="true" /></td>
                        </tr>
                        <tr> 
                            <td class="default">Install Script (non-SMS):</td>
                            <td><asp:textbox ID="txtScript" CssClass="default" runat="server" Width="100%" TextMode="MultiLine" Rows="10" /></td>
                        </tr>
                        <tr> 
                            <td class="default">Workstation Group (non-SMS):</td>
                            <td><asp:textbox ID="txtWorkstationGroup" CssClass="default" runat="server" Width="500" MaxLength="100" /></td>
                        </tr>
                        <tr> 
                            <td class="default"></td>
                            <td class="footer">EXAMPLE: GSGw_ImageScanPlatform</td>
                        </tr>
                        <tr> 
                            <td class="default">User Group (SMS):</td>
                            <td><asp:textbox ID="txtUserGroup" CssClass="default" runat="server" Width="500" MaxLength="100" /></td>
                        </tr>
                        <tr> 
                            <td class="default"></td>
                            <td class="footer">EXAMPLE: GSGa_PhotoshopCS</td>
                        </tr>
                        <tr> 
                            <td class="default">Email Notifications:</td>
                            <td><asp:textbox ID="txtNotifications" CssClass="default" runat="server" Width="500" MaxLength="100" /></td>
                        </tr>
                        <tr> 
                            <td class="default"></td>
                            <td class="footer">XIDs only! Separate with a ;</td>
                        </tr>
                        <tr> 
                            <td class="default">Enabled:</td>
                            <td><asp:CheckBox ID="chkEnabled" runat="server" Checked="true" /></td>
                        </tr>
                        <tr> 
                            <td class="cmdefault">&nbsp;</td>
                            <td><asp:Button ID="btnOrder" runat="server" Text="Change Order" Width="150" CssClass="default" /></td>
                        </tr>
                        <tr> 
                            <td class="cmdefault">&nbsp;</td>
                            <td><asp:Button ID="btnOS" runat="server" Text="Change OSs" Width="150" CssClass="default" /></td>
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
    <input type="hidden" id="hdnOrder" runat="server" />
</form>
</body>
</html>
