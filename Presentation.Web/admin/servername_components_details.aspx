<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="servername_components_details.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.servername_components_details" %>

<script type="text/javascript">
function EnsureHidden(oObject, oText, strAlert) {
    oObject = document.getElementById(oObject);
    if (oObject != null && oObject.value == "") {
        oText = document.getElementById(oText);
        alert(strAlert);
        oText.focus();
        return false;
    }
    return true;
}
</script>
<html>
<head>
<title>ClearView Administration</title>
<link rel="stylesheet" type="text/css" href="/css/default.css" />
<script type="text/javascript" src="/javascript/global.js"></script>
<script type="text/javascript" src="/javascript/both.js"></script>
<script type="text/javascript" src="/javascript/ajax.js"></script>
<script type="text/javascript" src="/javascript/treeview.js"></script>
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
		    <td><b>Server Component Details</b></td>
		    <td align="right">&nbsp;</td>
		</tr>
		<tr><td colspan="2">&nbsp;</td></tr>
		<tr>
		    <td colspan="2">
                <asp:Panel ID="panView" runat="server" Visible="false">
                <table width="100%" border="0" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <asp:TreeView ID="oTreeDetails" runat="server" ShowLines="true" NodeIndent="35">
                                <NodeStyle CssClass="default" />
                            </asp:TreeView>
                        </td>
                    </tr>
                </table>
               </asp:Panel>

                <asp:Panel id="panAdd" runat="server" Visible="false">
                    <table width="100%" border="0" cellpadding="0" cellspacing="0">
                        <tr>
                            <td>
                                <asp:Panel ID="panSave" runat="server" Visible="false">
                                    <div class="bigCheck"><img src="/images/bigCheck.gif" border="0" align="absmiddle" />&nbsp;Information&nbsp;has&nbsp;been&nbsp;Updated</div>
                                </asp:Panel>
                            </td>
                            <td width="100%" align="right">
                                <asp:button ID="btnAdd" CssClass="default" runat="server" Text="Add" Width="75" OnClick="btnAdd_Click" />
                                <asp:button ID="btnDelete" CssClass="default" runat="server" Text="Delete" Width="75" OnClick="btnDelete_Click" />
                                <asp:button ID="btnCancel" CssClass="default" runat="server" Text="Cancel" Width="75" OnClick="btnCancel_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">&nbsp;</td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <%=strMenuTab1 %>
                                <div id="divMenu1">
                                    <br />
                                    <div style="display:none">
                                        <table width="95%" cellpadding="3" cellspacing="0" border="0" align="center">
                                            <tr>
                                                <td class="default">Component:</td>
                                                <td><asp:DropDownList ID="ddlParent" CssClass="default" runat="server" /></td>
                                            </tr>
                                            <tr> 
                                                <td class="default">Detail Name:</td>
                                                <td><asp:textbox ID="txtName" CssClass="default" runat="server" Width="400" MaxLength="100"/></td>
                                            </tr>
                                            <tr> 
                                                <td class="default">ZEUS Array Config:</td>
                                                <td><asp:DropDownList ID="ddlZEUSArrayConfig" CssClass="default" runat="server" Width="400"/>&nbsp;&nbsp;<asp:Button ID="btnArrayConfig" runat="server" Text="Change Priority" CssClass="default" Width="125" /></td>
                                            </tr>
                                            <tr> 
                                                <td class="default">ZEUS Build Type:</td>
                                                <td><asp:DropDownList ID="ddlZEUSBuildType" CssClass="default" runat="server" Width="400"/>&nbsp;&nbsp;<asp:Button ID="btnBuildType" runat="server" Text="Change Priority" CssClass="default" Width="125" /></td>
                                            </tr>
                                            <tr> 
                                                <td class="default">Requires Approval:</td>
                                                <td><asp:CheckBox ID="chkApproval" runat="server" /></td>
                                            </tr>
                                            <tr> 
                                                <td class="default">Specify Individual Model(s):</td>
                                                <td><asp:CheckBox ID="chkModels" runat="server" /></td>
                                            </tr>
                                            <tr> 
                                                <td class="default">Specify Network Range(s):</td>
                                                <td><asp:CheckBox ID="chkNetworks" runat="server" /></td>
                                            </tr>
                                            <tr> 
                                                <td class="default">Auto-Install:</td>
                                                <td><asp:CheckBox ID="chkInstall" runat="server" /></td>
                                            </tr>
                                            <tr> 
                                                <td class="default">Network Source Is Mount:</td>
                                                <td><asp:CheckBox ID="chkMount" runat="server" /></td>
                                            </tr>
                                            <tr> 
                                                <td class="default">Network Path:</td>
                                                <td><asp:textbox ID="txtNetworkPath" CssClass="default" runat="server" Width="600" MaxLength="300"/></td>
                                            </tr>
                                            <tr> 
                                                <td class="default">Install Path:</td>
                                                <td><asp:textbox ID="txtInstallPath" CssClass="default" runat="server" Width="600" MaxLength="300"/></td>
                                            </tr>
                                            <tr> 
                                                <td class="default">Active Directory Move Location:</td>
                                                <td><asp:textbox ID="txtAD" CssClass="default" runat="server" Width="600" MaxLength="300"/></td>
                                            </tr>
                                            <tr> 
                                                <td class="default"></td>
                                                <td class="footer">EXAMPLE: OU=OUs_SQL,OU=OUc_Srvs,OU=OUc_DmnCptrs,</td>
                                            </tr>
                                            <tr> 
                                                <td class="default">Script:</td>
                                                <td><asp:DropDownList ID="ddlScript" CssClass="default" runat="server" Width="400"/></td>
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
                                        </table>
                                    </div>
                                    <div style="display:none">
                                        <%=strDisabled %>
                                        <asp:TreeView ID="oTreeClassEnvironment" runat="server" CssClass="default" ShowLines="true" NodeIndent="30">
                                        </asp:TreeView>
                                    </div>
                                    <div style="display:none">
                                        <%=strDisabled %>
                                        <%=strModels %>
                                        <asp:TreeView ID="oTreeModels" runat="server" CssClass="default" ShowLines="true" NodeIndent="30">
                                        </asp:TreeView>
                                    </div>
                                    <div style="display:none">
                                        <%=strDisabled %>
                                        <%=strApprovals %>
                                        <table width="100%" border="0" cellSpacing="2" cellPadding="2">
                                            <tr>
                                                <td class="default">User:</td>
                                                <td>
                                                    <table cellpadding="0" cellspacing="0" border="0">
                                                        <tr>
                                                            <td><asp:TextBox ID="txtUser" runat="server" Width="300" CssClass="default" /></td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <div id="divUser" runat="server" style="overflow:hidden; position:absolute; display:none; background-color:#FFFFFF; border:solid 1px #CCCCCC">
                                                                    <asp:ListBox ID="lstUser" runat="server" CssClass="default" />
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="default">Enabled:</td>
                                                <td><asp:CheckBox ID="chkEnabledUser" runat="server" CssClass="default" /></td>
                                            <tr>
                                                <td>&nbsp;</td>
                                                <td><asp:Button ID="btnUser" runat="server" CssClass="default" Width="75" Text="Add" OnClick="btnUser_Click" /></td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" valign="top">
                                                    <table width="100%" cellpadding="4" cellspacing="0" border="0" style="border:solid 1px #CCCCCC; background-color:#FFFFFF">
                                                        <tr bgcolor="#EEEEEE">
                                                            <td nowrap></td>
                                                            <td nowrap><b>User</b></td>
                                                            <td nowrap></td>
                                                        </tr>
                                                        <asp:repeater ID="rptUsers" runat="server">
                                                            <ItemTemplate>
                                                                <tr>
                                                                    <td align="left">&nbsp;<asp:ImageButton ID="btnDeleteLink" OnClick="btnDeleteLink_Click" runat="server" ToolTip="Delete" ImageAlign="AbsMiddle" ImageUrl="/admin/images/delete.gif" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' /></td>
                                                                    <td width="100%"><%# oUser.GetFullName(Int32.Parse(DataBinder.Eval(Container.DataItem, "userid").ToString())) + " (" + oUser.GetName(Int32.Parse(DataBinder.Eval(Container.DataItem, "userid").ToString())) + ")"%></td>
                                                                    <td align="center"><asp:ImageButton ID="btnEnableLink" runat="server" ImageAlign="AbsMiddle" ImageUrl='<%# (DataBinder.Eval(Container.DataItem, "enabled").ToString() == "1") ? "/admin/images/enabled.gif" : "/admin/images/disabled.gif" %>' CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' OnClick="btnEnableLink_Click" /></td>
                                                                </tr>
                                                            </ItemTemplate>
                                                        </asp:repeater>
                                                        <tr>
                                                            <td colspan="3">
                                                                <asp:Label ID="lblUsers" runat="server" CssClass="default" Visible="false" Text="<img src='/images/alert.gif' border='0' align='absmiddle'> There are no approvers..." />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <div style="display:none">
                                        <%=strDisabled %>
                                        <asp:TreeView ID="oTreeOperatingSystemServicePack" runat="server" CssClass="default" ShowLines="true" NodeIndent="30">
                                        </asp:TreeView>
                                    </div>
                                    <div style="display:none">
                                        <%=strDisabled %>
                                        <%=strMenuTabComp %>
                                        <div id="divMenuComp1">
                                            <br />
                                            <div style="display:none">
                                                <p><img src="/images/ico_check.gif" border="0" align="absmiddle"/> This component will be <u>selected and will be unavailable for removal</u> if one or more of the following components are selected...</p>
                                                <asp:TreeView ID="oTreeInclude" runat="server" CssClass="default" ShowLines="true" NodeIndent="30">
                                                </asp:TreeView>
                                            </div>
                                            <div style="display:none">
                                                <p><img src="/images/ico_error.gif" border="0" align="absmiddle"/> This component will be <u>unavailable for selection</u> if one or more of the following components are selected...</p>
                                                <asp:TreeView ID="oTreeExclude" runat="server" CssClass="default" ShowLines="true" NodeIndent="30">
                                                </asp:TreeView>
                                            </div>
                                        </div>
                                    </div>
                                    <div style="display:none">
                                        <%=strDisabled %>
                                        <%=strMenuTabApp %>
                                        <div id="divMenuApp1">
                                            <br />
                                            <div style="display:none">
                                                <p><img src="/images/ico_check.gif" border="0" align="absmiddle"/> This component will be <u>selected and will be unavailable for removal</u> if any of the following applications are selected...</p>
                                                <asp:TreeView ID="oTreeIncludeApp" runat="server" CssClass="default" ShowLines="true" NodeIndent="30">
                                                </asp:TreeView>
                                            </div>
                                            <div style="display:none">
                                                <p><img src="/images/ico_error.gif" border="0" align="absmiddle"/> This component will be <u>unavailable for selection</u> if any of the following applications are selected...</p>
                                                <asp:TreeView ID="oTreeExcludeApp" runat="server" CssClass="default" ShowLines="true" NodeIndent="30">
                                                </asp:TreeView>
                                            </div>
                                        </div>
                                    </div>
                                    <div style="display:none">
                                        <%=strDisabled %>
                                        <%=strNetworks %>
                                        <asp:TreeView ID="oTreeNetworks" runat="server" CssClass="default" ShowLines="true" NodeIndent="30">
                                        </asp:TreeView>
                                    </div>
                                    <div style="display:none">
                                        <%=strDisabled %>
                                        <asp:Panel ID="panScripts" runat="server" Visible="false">
                                            <iframe id="frmScripts" runat="server" frameborder="1" scrolling="no" width="100%" height="700" />
                                        </asp:Panel>
                                    </div>
                                </div>
                            </td>
                        </tr>
                    </table>
                    <p>&nbsp;</p>
                </asp:Panel>
            </td>
        </tr>
    </table>
        </div>
        </td>
        </tr>
        </table>
        <asp:HiddenField ID="hdnUser" runat="server" />
        <asp:HiddenField ID="hdnArrayConfigId" runat="server" />
        <asp:HiddenField ID="hdnArrayConfigOrder" runat="server" />
        <asp:HiddenField ID="hdnBuildTypeId" runat="server" />
        <asp:HiddenField ID="hdnBuildTypeOrder" runat="server" />
</form>
</body>
</html>
