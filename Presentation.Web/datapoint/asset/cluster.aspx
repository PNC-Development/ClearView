<%@ Page Language="C#" MasterPageFile="~/datapoint.Master" AutoEventWireup="True" CodeBehind="cluster.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.datapoint_cluster" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" runat="server">
<script type="text/javascript">
</script>
    <asp:Panel ID="panAllow" runat="server" Visible="false">
    <table width="100%" cellpadding="0" cellspacing="5" border="0">
        <tr id="cntrlButtons">
            <td rowspan="2"><img src="/images/assets.gif" border="0" align="absmiddle" /></td>
            <td class="header" nowrap valign="bottom"><asp:Label ID="lblHeader" runat="server" CssClass="header" /></td>
            <td width="100%" rowspan="2" align="right">
                <table cellpadding="1" cellspacing="4" border="0">
                    <tr>
                        <td nowrap><asp:LinkButton ID="btnPrint" runat="server" Text="<img src='/images/print-icon.gif' border='0' align='absmiddle' /><img src='/images/spacer.gif' border='0' width='5' height='1' />Print" /></td>
                        <td>|</td>
                        <td nowrap><asp:LinkButton ID="btnClose" runat="server" Text="<img src='/images/close-icon.gif' border='0' align='absmiddle' /><img src='/images/spacer.gif' border='0' width='5' height='1' />Close" /></td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr id="cntrlButtons2">
            <td nowrap valign="top"><asp:Label ID="lblHeaderSub" runat="server" CssClass="default" /></td>
        </tr>
        <tr id="cntrlProcessing" style="display:none">
            <td colspan="20">
                <table width="100%" cellpadding="0" cellspacing="5" border="0">
                    <tr>
                        <td rowspan="2"><img src="/images/saving.gif" border="0" align="absmiddle" /></td>
                        <td class="header" width="100%" valign="bottom">Processing...</td>
                    </tr>
                    <tr>
                        <td width="100%" valign="top">Please do not close this window while the page is processing.  Please be patient....</td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <br />
    <%=strMenuTab1 %>
    <table width="100%" height="100%" cellpadding="10" cellspacing="5" border="0">
        <tr>
            <td valign="top">
                <div id="divMenu1" class="tabbing">
                    <div style="display:none">
                        <table width="100%" cellpadding="5" cellspacing="2" border="0">
                            <tr>
                                <td class="header" colspan="2">Cluster Information&nbsp;&nbsp;<asp:Label ID="lblClusterID" runat="server" CssClass="default" /></td>
                            </tr>
                            <tr>
                                <td nowrap><asp:Label ID="fldName" runat="server" CssClass="default" Text="Cluster Name:" /></td>
                                <td width="100%"><asp:Label ID="lblName" runat="server" CssClass="default" /><asp:TextBox ID="txtName" runat="server" CssClass="default" MaxLength="100" Width="250" /> <asp:Button ID="btnNameLookup" runat="server" CssClass="default" Width="75" Text="Lookup" Visible="false" /> <asp:Button ID="btnName" runat="server" CssClass="default" Width="75" Text="Change" Visible="false" /></td>
                            </tr>
                            <tr>
                                <td nowrap><asp:Label ID="fldNickName" runat="server" CssClass="default" Text="Nick Name" /></td>
                                <td width="100%"><asp:Label ID="lblNickName" runat="server" CssClass="default" /><asp:TextBox ID="txtNickName" runat="server" CssClass="default" MaxLength="100" Width="400" /></td>
                            </tr>
                            <tr>
                                <td nowrap><asp:Label ID="fldCount" runat="server" CssClass="default" Text="# of Nodes:" /></td>
                                <td width="100%"><asp:Label ID="lblCount" runat="server" CssClass="default" /><asp:TextBox ID="txtCount" runat="server" CssClass="default" MaxLength="5" Width="50" /></td>
                            </tr>
                            <tr>
                                <td nowrap><asp:Label ID="fldDR" runat="server" CssClass="default" Text="# of DR Nodes:" /></td>
                                <td width="100%"><asp:Label ID="lblDR" runat="server" CssClass="default" /><asp:TextBox ID="txtDR" runat="server" CssClass="default" MaxLength="5" Width="50" /></td>
                            </tr>
                            <tr>
                                <td nowrap><asp:Label ID="fldHA" runat="server" CssClass="default" Text="# of HA Nodes:" /></td>
                                <td width="100%"><asp:Label ID="lblHA" runat="server" CssClass="default" /><asp:TextBox ID="txtHA" runat="server" CssClass="default" MaxLength="5" Width="50" /></td>
                            </tr>
                            <tr>
                                <td nowrap><asp:Label ID="fldPlatformOS" runat="server" CssClass="default" Text="Device OS:" /></td>
                                <td width="100%"><asp:Label ID="lblPlatformOS" runat="server" CssClass="default" /><asp:DropDownList ID="ddlPlatformOS" runat="server" CssClass="default" Width="400" /></td>
                            </tr>
                            <tr>
                                <td nowrap><asp:Label ID="fldPlatformServicePack" runat="server" CssClass="default" Text="Service Pack:" /></td>
                                <td width="100%"><asp:Label ID="lblPlatformServicePack" runat="server" CssClass="default" /><asp:DropDownList ID="ddlPlatformServicePack" runat="server" CssClass="default" Width="400" /></td>
                            </tr>
                            <tr>
                                <td nowrap><asp:Label ID="fldPlatformDomain" runat="server" CssClass="default" Text="Domain:" /></td>
                                <td width="100%"><asp:Label ID="lblPlatformDomain" runat="server" CssClass="default" /><asp:DropDownList ID="ddlPlatformDomain" runat="server" CssClass="default" Width="400" /></td>
                            </tr>
                            <tr>
                                <td nowrap><asp:Label ID="fldServiceAccount" runat="server" CssClass="default" Text="Service Account:" /></td>
                                <td width="100%"><asp:Label ID="lblServiceAccount" runat="server" CssClass="default" /><asp:TextBox ID="txtServiceAccount" runat="server" CssClass="default" MaxLength="100" Width="300" /></td>
                            </tr>
                            <tr>
                                <td nowrap><asp:Label ID="fldVirtualName" runat="server" CssClass="default" Text="Virtual Name:" /></td>
                                <td width="100%"><asp:Label ID="lblVirtualName" runat="server" CssClass="default" /><asp:TextBox ID="txtVirtualName" runat="server" CssClass="default" MaxLength="100" Width="300" /></td>
                            </tr>
                            <tr>
                                <td nowrap><asp:Label ID="fldPlatformIP" runat="server" CssClass="default" Text="IP Address:" /></td>
                                <td width="100%"><asp:Label ID="lblPlatformIP" runat="server" CssClass="default" /><asp:TextBox ID="txtPlatformIP" runat="server" CssClass="default" MaxLength="100" Width="200" /></td>
                            </tr>
                        </table>
                    </div>
                    <div style="display:none">
                        <table width="100%" cellpadding="5" cellspacing="2" border="0">
                            <tr>
                                <td class="header" colspan="2"><asp:Label ID="fldComponents" runat="server" CssClass="header" Text="Software Components" /></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Panel ID="panComponents" runat="server" Visible="false">
                                        <iframe id="frmComponents" runat="server" frameborder="1" scrolling="no" width="730" height="450" />
                                    </asp:Panel>
                                    <%=strComponents %>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div style="display:none">
                        <table width="100%" cellpadding="5" cellspacing="2" border="0">
                            <tr>
                                <td class="header" colspan="2"><asp:Label ID="fldInstances" runat="server" CssClass="header" Text="Instances" /></td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <table width="100%" cellpadding="4" cellspacing="0" border="0" style="border:solid 1px #CCCCCC">
                                        <asp:repeater ID="rptInstances" runat="server">
                                            <HeaderTemplate>
                                                <tr bgcolor="#EEEEEE">
                                                    <td></td>
                                                    <td nowrap><b>Instance Name</b></td>
                                                    <td nowrap><b>Nick Name</b></td>
                                                    <td nowrap><b>IP Address</b></td>
                                                    <td nowrap><b>Last Updated</b></td>
                                                </tr>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <tr>
                                                    <td><img src="/images/spacer.gif" border="0" width="15" height="1" /></td>
                                                    <td width="25%"><%# DataBinder.Eval(Container.DataItem, "name")%></td>
                                                    <td width="25%"><%# DataBinder.Eval(Container.DataItem, "nickname")%></td>
                                                    <td width="25%"><%# DataBinder.Eval(Container.DataItem, "ip")%></td>
                                                    <td width="25%"><%# DataBinder.Eval(Container.DataItem, "modified")%></td>
                                                </tr>
                                            </ItemTemplate>
                                            <AlternatingItemTemplate>
                                                <tr bgcolor="#F6F6F6">
                                                    <td><img src="/images/spacer.gif" border="0" width="15" height="1" /></td>
                                                    <td width="25%"><%# DataBinder.Eval(Container.DataItem, "name")%></td>
                                                    <td width="25%"><%# DataBinder.Eval(Container.DataItem, "nickname")%></td>
                                                    <td width="25%"><%# DataBinder.Eval(Container.DataItem, "ip")%></td>
                                                    <td width="25%"><%# DataBinder.Eval(Container.DataItem, "modified")%></td>
                                                </tr>
                                            </AlternatingItemTemplate>
                                        </asp:repeater>
                                        <tr>
                                            <td colspan="5"><asp:Label ID="lblInstances" runat="server" CssClass="default" Visible="false" Text="<img src='/images/spacer.gif' border='0' height='1' width='25' /><img src='/images/alert.gif' border='0' align='absmiddle'> There are no instances" /></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div style="display:none">
                        <table width="100%" cellpadding="5" cellspacing="2" border="0">
                            <tr>
                                <td class="header" colspan="2"><asp:Label ID="fldNodes" runat="server" CssClass="header" Text="Nodes" /></td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <table width="100%" cellpadding="4" cellspacing="0" border="0" style="border:solid 1px #CCCCCC">
                                        <asp:repeater ID="rptNodes" runat="server">
                                            <HeaderTemplate>
                                                <tr bgcolor="#EEEEEE">
                                                    <td></td>
                                                    <td nowrap><b>Node</b></td>
                                                    <td nowrap><b>Last Updated</b></td>
                                                </tr>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <tr>
                                                    <td><img src="/images/spacer.gif" border="0" width="15" height="1" /></td>
                                                    <td width="50%"><a href="javascript:void(0)" onclick="OpenNewWindowMenu('/datapoint/asset/datapoint_asset_search.aspx?t=name&q=<%#oFunction.encryptQueryString(DataBinder.Eval(Container.DataItem, "servername").ToString()) %>', '800', '600');"><%# DataBinder.Eval(Container.DataItem, "servername")%></a></td>
                                                    <td width="50%"><%# DataBinder.Eval(Container.DataItem, "modified")%></td>
                                                </tr>
                                            </ItemTemplate>
                                            <AlternatingItemTemplate>
                                                <tr bgcolor="#F6F6F6">
                                                    <td><img src="/images/spacer.gif" border="0" width="15" height="1" /></td>
                                                    <td width="50%"><a href="javascript:void(0)" onclick="OpenNewWindowMenu('/datapoint/asset/datapoint_asset_search.aspx?t=name&q=<%#oFunction.encryptQueryString(DataBinder.Eval(Container.DataItem, "servername").ToString()) %>', '800', '600');"><%# DataBinder.Eval(Container.DataItem, "servername")%></a></td>
                                                    <td width="50%"><%# DataBinder.Eval(Container.DataItem, "modified")%></td>
                                                </tr>
                                            </AlternatingItemTemplate>
                                        </asp:repeater>
                                        <tr>
                                            <td colspan="5"><asp:Label ID="lblNodes" runat="server" CssClass="default" Visible="false" Text="<img src='/images/spacer.gif' border='0' height='1' width='25' /><img src='/images/alert.gif' border='0' align='absmiddle'> There are no nodes" /></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div style="display:none">
                        <table width="100%" cellpadding="5" cellspacing="2" border="0">
                            <tr>
                                <td class="header" colspan="2">Administration</td>
                            </tr>
                            <tr>
                                <td nowrap colspan="2">
                                    <asp:Panel ID="panAdministration" runat="server" Visible="false">
                                        <table width="100%" cellpadding="5" cellspacing="2" border="0">
		                                    <tr> 
		                                        <td colspan="2">
		                                            <table width="100%" cellpadding="4" cellspacing="3" border="0">
		                                                <%=strAdministration %>
		                                            </table>
		                                        </td>
		                                    </tr>
		                                    <tr> 
		                                        <td colspan="2"><b>Show Output</b></td>
		                                    </tr>
		                                    <tr>
		                                        <td colspan="2"><asp:CheckBox ID="chkDebug" runat="server" CssClass="default" Text="Include Debug Information" /></td>
		                                    </tr>
		                                    <tr>
		                                        <td colspan="2"><asp:Button ID="btnOutput" runat="server" CssClass="default" Width="75" OnClick="btnOutput_Click" Text="Go" /></td>
		                                    </tr>
		                                    <tr>
		                                        <td colspan="2">&nbsp;</td>
		                                    </tr>
                                        </table>
                                    </asp:Panel>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </td>
        </tr>
    </table>
    </asp:Panel>
    <asp:Panel ID="panDenied" runat="server" Visible="false">
        <br />
            <table width="100%" cellpadding="0" cellspacing="5" border="0">
                <tr>
                    <td rowspan="2"><img src="/images/ico_error.gif" border="0" align="absmiddle" /></td>
                    <td class="header" width="100%" valign="bottom">Access Denied</td>
                </tr>
                <tr>
                    <td width="100%" valign="top">You do not have sufficient permission to view this page.</td>
                </tr>
                <tr>
                    <td colspan="2">&nbsp;</td>
                </tr>
                <tr>
                    <td></td>
                    <td width="100%">If you think you should have rights to view it, please contact your ClearView administrator.</td>
                </tr>
            </table>
        <p>&nbsp;</p>
    </asp:Panel>
<asp:HiddenField ID="hdnTab" runat="server" />
<input type="hidden" id="hdnComponents" name="hdnComponents" />
</asp:Content>
