<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="datastores.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.datastores" %>

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
		    <td><b>Datastores</b></td>
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
                            <td class="default" width="100px">Cluster:</td>
                            <td><asp:DropDownList ID="ddlParent" CssClass="default" runat="server"/></td>
                        </tr>
                        <tr> 
                            <td class="default" width="100px">Name:</td>
                            <td><asp:textbox ID="txtName" CssClass="default" runat="server" Width="200" MaxLength="50"/>&nbsp;&nbsp;&nbsp;<span class="reddefault"><b>NOTE:</b> This field IS CASE SENSITIVE.</span></td>
                        </tr>
                        <tr> 
                            <td class="default" width="100px">Storage Type:</td>
                            <td>
                                <asp:DropDownList ID="ddlType" CssClass="default" runat="server">
                                    <asp:ListItem Value="1" Text="Low Performance" />
                                    <asp:ListItem Value="10" Text="Standard Performance" />
                                    <asp:ListItem Value="100" Text="High Performance" />
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr> 
                            <td class="default" width="100px">Operating System Group:</td>
                            <td><asp:DropDownList ID="ddlOperatingSystemGroup" CssClass="default" runat="server"/></td>
                        </tr>
                        <tr> 
                            <td class="default">Replicated:</td>
                            <td><asp:CheckBox ID="chkReplicated" runat="server" Checked="true" /></td>
                        </tr>
                        <tr> 
                            <td class="default" width="100px">Maximum Guests:</td>
                            <td><asp:textbox ID="txtMaximum" CssClass="default" runat="server" Width="100" MaxLength="10"/></td>
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
</form>
</body>
</html>
