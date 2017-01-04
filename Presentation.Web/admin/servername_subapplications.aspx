<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="servername_subapplications.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.servername_subapplications" %>

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
		    <td><b>Server Name Sub-Applications</b></td>
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
                                                <td class="default">Application:</td>
                                                <td><asp:dropdownlist ID="ddlApplication" CssClass="default" runat="server"/></td>
                                            </tr>
                                            <tr> 
                                                <td class="default" width="100px">Name:</td>
                                                <td><asp:textbox ID="txtName" CssClass="default" runat="server" Width="300" MaxLength="100"/></td>
                                            </tr>
                                            <tr> 
                                                <td class="default">Code:</td>
                                                <td><asp:textbox ID="txtCode" CssClass="default" runat="server" Width="100" MaxLength="3"/></td>
                                            </tr>
                                            <tr> 
                                                <td class="default">Factory Code:</td>
                                                <td><asp:textbox ID="txtFactoryCode" CssClass="default" runat="server" Width="75" MaxLength="2"/></td>
                                            </tr>
                                            <tr> 
                                                <td class="default">Factory Code Specific:</td>
                                                <td><asp:textbox ID="txtFactoryCodeSpecific" CssClass="default" runat="server" Width="75" MaxLength="2"/></td>
                                            </tr>
                                            <tr> 
                                                <td class="default">ZEUS Array Config:</td>
                                                <td><asp:textbox ID="txtZEUSArrayConfig" CssClass="default" runat="server" Width="100" MaxLength="20"/></td>
                                            </tr>
                                            <tr> 
                                                <td class="default">ZEUS OS:</td>
                                                <td><asp:textbox ID="txtZEUSOs" CssClass="default" runat="server" Width="100" MaxLength="20"/></td>
                                            </tr>
                                            <tr> 
                                                <td class="default">ZEUS OS Version:</td>
                                                <td><asp:textbox ID="txtZEUSOsVersion" CssClass="default" runat="server" Width="100" MaxLength="20"/></td>
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
                                                <td class="default">Permit No Replication:</td>
                                                <td><asp:CheckBox ID="chkPermitNoReplication" runat="server" Checked="true" /></td>
                                            </tr>
                                             <tr>
                                                <td class="default">Solution Codes:</td>
                                                <td><asp:DropDownList ID="ddlSolutionCodes" CssClass="default" runat="server"/></td>
                                            </tr>
                                            <tr> 
                                                <td class="default">Specify Network Range(s):</td>
                                                <td><asp:CheckBox ID="chkNetworks" runat="server" /></td>
                                            </tr>
                                            <tr> 
                                                <td class="default">Enabled:</td>
                                                <td><asp:CheckBox ID="chkEnabled" runat="server" Checked="true" /></td>
                                            </tr>
                                            <tr> 
                                                <td class="cmdefault">&nbsp;</td>
                                                <td><asp:Button ID="btnOrder" runat="server" Text="Change Order" Width="150" CssClass="default" /></td>
                                            </tr>
                                        </table>
                                    </div>
                                    <div style="display:none">
                                        <%=strDisabled %>
                                        <%=strNetworks %>
                                        <asp:TreeView ID="oTreeNetworks" runat="server" CssClass="default" ShowLines="true" NodeIndent="30">
                                        </asp:TreeView>
                                    </div>
                                </div>
                            </td>
                        </tr>
                    </table>
                    <p>&nbsp;</p>
                </asp:Panel>
                </div>
            </td>
        </tr>
    </table>
        </div>
        </td>
        </tr>
        </table>
    <input type="hidden" id="hdnOrder" runat="server" />
</form>
</body>
</html>
