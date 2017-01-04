<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="asset_switches.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.asset_switches" %>

<script type="text/javascript">
</script>
<html>
<head>
<title>ClearView Administration</title>
<link rel="stylesheet" type="text/css" href="/css/default.css" />
<script type="text/javascript" src="/javascript/global.js"></script>
<script type="text/javascript" src="/javascript/both.js"></script>
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
		    <td><b>Switches</b></td>
		    <td align="right"><asp:LinkButton ID="btnNew" runat="server" Text="Add New" OnClick="btnNew_Click" /> </td>
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
                            <td class="default" width="125px">Serial:</td>
                            <td><asp:textbox ID="txtSerial" CssClass="default" runat="server" Width="300" MaxLength="100"/></td>
                        </tr>
                        <tr> 
                            <td class="default" width="125px">Asset Tag:</td>
                            <td><asp:textbox ID="txtAsset" CssClass="default" runat="server" Width="150" MaxLength="20"/></td>
                        </tr>
                        <tr>
                            <td class="default">Platform:</td>
                            <td><asp:DropDownList ID="ddlPlatform" CssClass="default" runat="server" Width="400" /></td>
                        </tr>
                        <tr>
                            <td class="default">Type:</td>
                            <td>
                                <asp:DropDownList ID="ddlType" CssClass="default" runat="server" Width="400" Enabled="false">
                                    <asp:ListItem Value="-- Please select a Platform --" />
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="default">Model:</td>
                            <td>
                                <asp:DropDownList ID="ddlModel" CssClass="default" runat="server" Width="400" Enabled="false">
                                    <asp:ListItem Value="-- Please select a Platform --" />
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="default">Model Property:</td>
                            <td>
                                <asp:DropDownList ID="ddlModelProperty" CssClass="default" runat="server" Width="400" Enabled="false">
                                    <asp:ListItem Value="-- Please select a Platform --" />
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr> 
                            <td class="default" width="125px">Name:</td>
                            <td><asp:textbox ID="txtName" CssClass="default" runat="server" Width="300" MaxLength="100"/></td>
                        </tr>
                        <tr style="display:none">
                            <td class="default">Class:</td>
                            <td><asp:DropDownList ID="ddlClass" CssClass="default" runat="server" Width="200" /></td>
                        </tr>
                        <tr style="display:none">
                            <td class="default">Environment:</td>
                            <td>
                                <asp:DropDownList ID="ddlEnvironment" CssClass="default" runat="server" Width="200" Enabled="false" >
                                    <asp:ListItem Value="-- Please select a Class --" />
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="default">Rack:</td>
                            <td><asp:label ID="lblParent" CssClass="default" runat="server" />&nbsp;&nbsp;<asp:Button ID="btnParent" runat="server" CssClass="default" Width="25" Text="..." /></td>
                        </tr>
                        <tr> 
                            <td class="default" width="125px">Rack Position:</td>
                            <td><asp:textbox ID="txtRackPosition" CssClass="default" runat="server" Width="100" MaxLength="10"/></td>
                        </tr>
                        <tr> 
                            <td class="default" width="125px"># of Blades:</td>
                            <td><asp:textbox ID="txtBlades" CssClass="default" runat="server" Width="100" MaxLength="10"/></td>
                        </tr>
                        <tr> 
                            <td class="default" width="125px"># of Ports per Blade:</td>
                            <td><asp:textbox ID="txtPorts" CssClass="default" runat="server" Width="100" MaxLength="10"/></td>
                        </tr>
                        <tr> 
                            <td class="default">IOS:</td>
                            <td><asp:CheckBox ID="chkIsIOS" runat="server" /></td>
                        </tr>
                        <tr> 
                            <td class="default">Nexus:</td>
                            <td><asp:CheckBox ID="chkIsNexus" runat="server" /></td>
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
    <input type="hidden" id="hdnParent" runat="server" />
    <input type="hidden" id="hdnEnvironment" runat="server" />
    <asp:HiddenField ID="hdnModel" runat="server" />
</form>
</body>
</html>

