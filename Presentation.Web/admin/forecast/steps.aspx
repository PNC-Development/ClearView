<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="steps.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.steps" %>

<html>
<head>
<title>ClearView Administration</title>
<link rel="stylesheet" type="text/css" href="/css/default.css" />
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
		    <td><b>On Demand Steps</b></td>
		    <td align="right">&nbsp;</td>
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
                            <td class="default">Platform:</td>
                            <td><asp:label ID="lblParent" CssClass="default" runat="server" />&nbsp;&nbsp;<asp:Button ID="btnParent" runat="server" CssClass="default" Width="25" Text="..." /></td>
                        </tr>
                        <tr> 
                            <td class="default">Name:</td>
                            <td><asp:textbox ID="txtName" CssClass="default" runat="server" Width="200" MaxLength="50"/></td>
                        </tr>
                        <tr> 
                            <td class="default">Title:</td>
                            <td><asp:textbox ID="txtTitle" CssClass="default" runat="server" Width="500" MaxLength="50" /></td>
                        </tr>
                        <tr>
                            <td colspan="2">&nbsp;</td>
                        </tr>
                        <tr bgcolor="#EEEEEE"> 
                            <td class="default">Path:</td>
                            <td><asp:textbox ID="txtPath" CssClass="default" runat="server" Width="400" MaxLength="100"/> <asp:button ID="btnPath" runat="server" Text="..." Width="25" CssClass="default"/></td>
                        </tr>
                        <tr bgcolor="#EEEEEE">
                            <td></td>
                            <td><b>*** OR ***</b></td>
                        </tr>
                        <tr bgcolor="#EEEEEE"> 
                            <td class="default">Script:</td>
                            <td><asp:textbox ID="txtScript" CssClass="default" runat="server" Width="100%" MaxLength="100" TextMode="MultiLine" Rows="8"/></td>
                        </tr>
                        <tr bgcolor="#EEEEEE">
                            <td></td>
                            <td><b>*** OR ***</b></td>
                        </tr>
                        <tr bgcolor="#EEEEEE"> 
                            <td class="default">User Interaction Path:</td>
                            <td><asp:textbox ID="txtInteraction" CssClass="default" runat="server" Width="400" MaxLength="100"/> <asp:button ID="btnInteraction" runat="server" Text="..." Width="25" CssClass="default"/></td>
                        </tr>
                        <tr bgcolor="#EEEEEE">
                            <td></td>
                            <td><b>*** OR ***</b></td>
                        </tr>
                        <tr bgcolor="#EEEEEE"> 
                            <td class="default">Known Steps:</td>
                            <td>
                                <table cellpadding="2" cellspacing="2" border="0">
                                    <tr>
                                        <td><asp:RadioButton ID="radZEUS" Text="ZEUS" runat="server" CssClass="default" GroupName="steps" /></td>
                                        <td><asp:RadioButton ID="radPower" Text="Power On" runat="server" CssClass="default" GroupName="steps" /></td>
                                        <td><asp:RadioButton ID="radInstalls" Text="Install Components" runat="server" CssClass="default" GroupName="steps" /></td>
                                    </tr>
                                    <tr>
                                        <td><asp:RadioButton ID="radAccounts" Text="Account Creation" runat="server" CssClass="default" GroupName="steps" /></td>
                                        <td><asp:RadioButton ID="radGroups" Text="Tie in AD Groups" runat="server" CssClass="default" GroupName="steps" /></td>
                                        <td><asp:RadioButton ID="radNone" Text="None" runat="server" CssClass="default" GroupName="steps" /></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">&nbsp;</td>
                        </tr>
                        <tr>
                            <td></td>
                            <td>
                                {XXX} = DataSet Variables (ILO, SERVERNAME)<br />
                                #XXX# = Global Variables (DSN, SCRIPTDIR)<br />
                                @XXX@ = Environment Variables (USER, DOMAIN, PASSWORD)<br />
                            </td>
                        </tr>
                        <tr> 
                            <td class="default">Success Text:</td>
                            <td><asp:textbox ID="txtScriptDone" CssClass="default" runat="server" Width="500" MaxLength="100" /></td>
                        </tr>
                        <tr>
                            <td class="default">Type:</td>
                            <td>
                                <asp:DropDownList ID="ddlType" runat="server" CssClass="default" >
                                    <asp:ListItem Value="0" Text="Do Not Close Window" />
                                    <asp:ListItem Value="1" Text="Can Close Window" />
                                    <asp:ListItem Value="-1" Text="User Interaction Required" />
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr> 
                            <td class="default">Resume on Error:</td>
                            <td><asp:CheckBox ID="chkResume" runat="server" /></td>
                        </tr>
                        <tr> 
                            <td class="default">Show Build:</td>
                            <td><asp:CheckBox ID="chkShowBuild" runat="server" /></td>
                        </tr>
                        <tr> 
                            <td class="default">Enabled:</td>
                            <td><asp:CheckBox ID="chkEnabled" runat="server" Checked="true" /></td>
                        </tr>
                        <tr><td height="5" colspan="2"><img src="images/spacer.gif" width="1" height="5" /></td></tr>
                        <tr> 
                            <td class="default">&nbsp;</td>
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
    <input type="hidden" id="hdnParent" runat="server" />
    <input type="hidden" id="hdnOrder" runat="server" />
</form>
</body>
</html>
