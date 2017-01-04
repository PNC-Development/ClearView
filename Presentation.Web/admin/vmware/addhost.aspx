<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="addhost.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.addhost" %>

<script type="text/javascript">
function ShowHideDiv2(oDiv) {
    oDiv = document.getElementById(oDiv);
    if (oDiv.style.display == "inline")
        oDiv.style.display = "none";
    else
        oDiv.style.display = "inline";
}
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
		    <td><b>Automated Host Provisioning Status</b></td>
		    <td align="right"></td>
		</tr>
		<tr><td colspan="2">&nbsp;</td></tr>
		<tr>
		    <td colspan="2" align="center">
                <table width="100%" cellpadding="4" cellspacing="0" border="0" style="border:solid 1px #CCCCCC">
                    <tr bgcolor="#EEEEEE">
                        <td width="1"></td>
                        <td class="bold">&nbsp;</td>
                        <td nowrap class="bold">Name</td>
                        <td nowrap class="bold">Virtual Center</td>
                        <td nowrap class="bold">Data Center</td>
                        <td nowrap class="bold">Folder</td>
                        <td nowrap class="bold">Cluster</td>
                        <td nowrap class="bold">Status</td>
                        <td nowrap class="bold">Modified</td>
                        <td class="bold">&nbsp;</td>
                    </tr>
                    <asp:repeater ID="rptView" runat="server">
                        <ItemTemplate>
                            <tr bgcolor="#EFEFEF" class="default">
                                <td width="1"><img src='/images/<%# DataBinder.Eval(Container.DataItem, "error").ToString() == "1" ? "cancel" : DataBinder.Eval(Container.DataItem, "step").ToString() == "999" ? "check" : "active" %>.gif' border="0" align="absmiddle" /></td>
                                <td nowrap>[<a href="javascript:void(0);" onclick="ShowHideDiv2('div<%# DataBinder.Eval(Container.DataItem, "id") %>');">Details</a>]</td>
                                <td nowrap>&nbsp;<%# DataBinder.Eval(Container.DataItem, "servername").ToString()%></td>
                                <td nowrap>&nbsp;<%# DataBinder.Eval(Container.DataItem, "name_virtualcenter")%></td>
                                <td nowrap>&nbsp;<%# DataBinder.Eval(Container.DataItem, "name_datacenter")%></td>
                                <td nowrap>&nbsp;<%# DataBinder.Eval(Container.DataItem, "name_folder")%></td>
                                <td nowrap>&nbsp;<%# DataBinder.Eval(Container.DataItem, "name_cluster")%></td>
                                <td nowrap>&nbsp;<%# (DataBinder.Eval(Container.DataItem, "step").ToString() == "999" ? "Completed" : "Step " + DataBinder.Eval(Container.DataItem, "step").ToString()) %></td>
                                <td nowrap>&nbsp;<%# DataBinder.Eval(Container.DataItem, "modified")%></td>
                                <td nowrap>[<asp:LinkButton ID="btnFix" OnClick="btnFix_Click" runat="server" ToolTip="Fix" Text="Fix" Enabled='<%# DataBinder.Eval(Container.DataItem, "error").ToString() == "1" %>' CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' CommandName='<%# DataBinder.Eval(Container.DataItem, "step") %>' />]</td>
                            </tr>
                            <tr id='div<%#DataBinder.Eval(Container.DataItem, "id").ToString()%>' style='display:<%#DataBinder.Eval(Container.DataItem, "step").ToString() == "999" ? "none" : "inline" %>'>
                                <td colspan="2"></td>
                                <td colspan="7">
                                    <asp:repeater id="rptHistory" datasource='<%# ((DataRowView)Container.DataItem).Row.GetChildRows("relationship") %>' runat="server">
                                        <HeaderTemplate>
                                            <table width="100%">
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td valign="top" width=\"100%\" colspan="6"><%#DataBinder.Eval(Container.DataItem, "[\"results\"]").ToString() %></td>
                                                <td valign="top" nowrap align="right"><%#DataBinder.Eval(Container.DataItem, "[\"modified\"]").ToString() %></td>
                                                <td></td>
                                            </tr>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            </table>
                                        </FooterTemplate>
                                    </asp:repeater>
                                </td>
                                <td></td>
                            </tr>
                        </ItemTemplate>
                    </asp:repeater>
                </table>
            </td>
        </tr>
    </table>
        </div>
        </td>
        </tr>
        </table>
    <input type="hidden" id="hdnCluster" runat="server" />
    <input type="hidden" id="hdnAddress" runat="server" />
    <input type="hidden" id="hdnEnvironment" runat="server" />
</form>
</body>
</html>



