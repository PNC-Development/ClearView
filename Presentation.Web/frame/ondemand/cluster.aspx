<%@ Page Language="C#" MasterPageFile="~/window.Master" AutoEventWireup="true" CodeBehind="cluster.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.cluster" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" Runat="Server">
<script type="text/javascript">
    function NewInstance(intA, intC, intID) {
        return OpenWindow('ONDEMAND_CLUSTER_INSTANCE_NEW','?aid=' + intA + '&cid=' + intC);
    }
    function OpenInstance(intA, intC, intID) {
        return OpenWindow('ONDEMAND_CLUSTER_INSTANCE','?aid=' + intA + '&cid=' + intC + '&id=' + intID);
    }
</script>
<table width="100%" height="100%" cellpadding="4" cellspacing="2" border="0">
    <tr height="1">
        <td colspan="2">
            <table width="100%" cellpadding="0" cellspacing="5" border="0">
                <tr>
                    <td rowspan="2"><img src="/images/cluster.gif" border="0" align="middle" /></td>
                    <td class="header" width="100%" valign="bottom">Cluster Configuration</td>
                </tr>
                <tr>
                    <td width="100%" valign="top">Using this page, you can add or remove clusters associated with your server.</td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td colspan="2">
        <asp:Panel ID="panView" runat="server" Visible="false">
            <table width="100%" height="100%" cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td valign="top">
                        <div style="height:100%; overflow:auto">
                        <table width="100%" cellpadding="4" cellspacing="0" border="0">
                            <asp:Panel ID="panNote" runat="server" Visible="false">
                            <tr>
                                <td colspan="3" class="default"><img src="/images/arrow_green_right.gif" border="0" align="absmiddle" /> <b>NOTE:</b> You cannot begin to configure this cluster until you enter the number of nodes and click <b>Save</b>.</td>
                            </tr>
                            </asp:Panel>
                            <tr>
                                <td nowrap>Nickname:</td>
                                <td><asp:TextBox ID="txtName" runat="server" CssClass="default" Width="200" MaxLength="50" /></td>
                                <td align="right"><asp:Image ID="imgNodes" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/arrow_green_right.gif" Visible="false" /> <asp:Button ID="btnNodes" runat="server" CssClass="default" Width="180" Text="Configure Local Nodes" /></td>
                            </tr>
                            <tr>
                                <td nowrap>Enter the number of nodes:</td>
                                <td><asp:TextBox ID="txtNodes" runat="server" CssClass="default" Width="100" MaxLength="3" /></td>
                                <td align="right"><asp:Image ID="imgStorage" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/arrow_green_right.gif" Visible="false" /> <asp:Button ID="btnStorage" runat="server" CssClass="default" Width="180" Text="Configure Non-Shared Storage" /></td>
                            </tr>
                            <tr>
                                <td nowrap>Number of DR servers:</td>
                                <td><asp:TextBox ID="txtDR" runat="server" CssClass="default" Width="100" MaxLength="3" /></td>
                                <td align="right"><asp:Image ID="imgAdd" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/arrow_green_right.gif" Visible="false" /> <asp:Button ID="btnAdd" runat="server" CssClass="default" Width="180" Text="Add a New Instance" /></td>
                            </tr>
                            <tr>
                                <td nowrap>Number of HA servers:</td>
                                <td><asp:TextBox ID="txtHA" runat="server" CssClass="default" Width="100" MaxLength="3" /></td>
                                <td align="right"></td>
                            </tr>
                            <tr>
                                <td colspan="2" class="header">List of Configured Instances...</td>
                                <td align="right"><asp:Image ID="imgQuorum" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/arrow_green_right.gif" Visible="false" /> <asp:Button ID="btnQuorum" runat="server" CssClass="default" Width="180" Text="Configure Quorum" /></td>
                            </tr>
                        </table>
                        <br />
                        <table width="100%" cellpadding="3" cellspacing="0" border="0" style="border:solid 1px #CCCCCC">
                            <tr bgcolor="#EEEEEE">
                                <td><b><u>Nickname:</u></b></td>
                                <td></td>
                            </tr>
                            <asp:repeater ID="rptInstances" runat="server">
                                <ItemTemplate>
                                    <tr>
                                        <td valign="top"><%# (DataBinder.Eval(Container.DataItem, "nickname").ToString() == "" ? "---" : DataBinder.Eval(Container.DataItem, "nickname").ToString())%></td>
                                        <td valign="top" align="right">[<a href="javascript:void(0)" onclick="OpenInstance('<%# intAnswer.ToString() %>','<%# DataBinder.Eval(Container.DataItem, "clusterid") %>','<%# DataBinder.Eval(Container.DataItem, "id") %>');">Edit</a>] [<asp:LinkButton ID="btnDelete" runat="server" Text="Delete" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' OnClick="btnDelete_Click" />]</td>
                                    </tr>
                                </ItemTemplate>
                                <AlternatingItemTemplate>
                                    <tr bgcolor="F6F6F6">
                                        <td valign="top"><%# (DataBinder.Eval(Container.DataItem, "nickname").ToString() == "" ? "---" : DataBinder.Eval(Container.DataItem, "nickname").ToString())%></td>
                                        <td valign="top" align="right">[<a href="javascript:void(0)" onclick="OpenInstance('<%# intAnswer.ToString() %>','<%# DataBinder.Eval(Container.DataItem, "clusterid") %>','<%# DataBinder.Eval(Container.DataItem, "id") %>');">Edit</a>] [<asp:LinkButton ID="btnDelete" runat="server" Text="Delete" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' OnClick="btnDelete_Click" />]</td>
                                    </tr>
                                </AlternatingItemTemplate>
                            </asp:repeater>
                            <tr>
                                <td colspan="3">
                                    <asp:Label ID="lblNone" runat="server" CssClass="default" Visible="false" Text="<img src='/images/spacer.gif' border='0' height='1' width='25' /><img src='/images/alert.gif' border='0' align='absmiddle'> You have not added any instances to this cluster request" />
                                </td>
                            </tr>
                        </table>
                        </div>
                    </td>
                </tr>
                <tr height="1">
                    <td>
                        <table width="100%" cellpadding="4" cellspacing="2" border="0">
                            <tr>
                                <td colspan="3"><hr size="1" noshade /></td>
                            </tr>
                            <tr>
                                <td class="required">* = Required Field</td>
                                <td align="center">
                                </td>
                                <td align="right">
                                    <asp:Image ID="imgSave" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/arrow_green_right.gif" Visible="false" /> <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="default" Width="75" OnClick="btnSave_Click" /> 
                                    <asp:Button ID="btnClose" runat="server" Text="Close" CssClass="default" Width="75" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            </asp:Panel>
            <asp:Panel ID="panDenied" runat="server" Visible="false">
                <table width="100%" cellpadding="2" cellspacing="2" border="0">
                    <tr>
                        <td><img src="/images/spacer.gif" border="0" width="15" height="1" /></td>
                        <td class="header"><img src="/images/bigX.gif" border="0" align="absmiddle" /> Access Denied</td>
                    </tr>
                    <tr><td colspan="2">&nbsp;</td></tr>
                    <tr>
                        <td><img src="/images/spacer.gif" border="0" width="15" height="1" /></td>
                        <td>You do not have rights to view this item.</td>
                    </tr>
                    <tr><td colspan="2">&nbsp;</td></tr>
                    <tr>
                        <td colspan="2"><hr size="1" noshade /></td>
                    </tr>
                    <tr>
                        <td colspan="2" align="right">
                            <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                <tr>
                                    <td class="footer"></td>
                                    <td align="right"><asp:Button ID="btnDenied" runat="server" CssClass="default" Width="75" Text="Close" /></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </td>
    </tr>
</table>
</asp:Content>
