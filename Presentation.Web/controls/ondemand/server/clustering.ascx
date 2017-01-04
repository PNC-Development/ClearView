<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="clustering.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.clustering" %>

<script type="text/javascript">
    function OpenCluster(intA, intID) {
        return OpenWindow('ONDEMAND_CLUSTER','?aid=' + intA + '&id=' + intID);
    }
</script>
<table width="100%" height="100%" cellpadding="0" cellspacing="0" border="0">
    <tr>
        <td valign="top">
            <div style="height:100%; overflow:auto">
            <table width="100%" cellpadding="4" cellspacing="0" border="0">
                <tr>
                    <td class="header">List of Configured Clusters...</td>
                    <td align="right"><asp:Button ID="btnAdd" runat="server" CssClass="default" Width="125" Text="Add Cluster" /></td>
                </tr>
            </table>
            <br />
            <table width="100%" cellpadding="3" cellspacing="0" border="0" style="border:solid 1px #CCCCCC">
                <tr bgcolor="#EEEEEE">
                    <td><b><u>Nickname:</u></b></td>
                    <td><b><u>Nodes:</u></b></td>
                    <td><b><u>DR Servers:</u></b></td>
                    <td><b><u>HA Servers:</u></b></td>
                    <td></td>
                </tr>
                <asp:repeater ID="rptClusters" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td valign="top"><%# DataBinder.Eval(Container.DataItem, "nickname") %></td>
                            <td valign="top"><%# DataBinder.Eval(Container.DataItem, "nodes") %></td>
                            <td valign="top"><%# DataBinder.Eval(Container.DataItem, "dr") %></td>
                            <td valign="top"><%# DataBinder.Eval(Container.DataItem, "ha") %></td>
                            <td valign="top" align="right">[<a href="javascript:void(0)" onclick="OpenCluster('<%# intID.ToString() %>','<%# DataBinder.Eval(Container.DataItem, "id") %>');">Edit</a>]<img src="/images/spacer.gif" border="0" width="10" height="1" />[<asp:LinkButton ID="btnDelete" runat="server" Text="Delete" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' OnClick="btnDelete_Click" />]</td>
                        </tr>
                    </ItemTemplate>
                    <AlternatingItemTemplate>
                        <tr bgcolor="F6F6F6">
                            <td valign="top"><%# DataBinder.Eval(Container.DataItem, "nickname")%></td>
                            <td valign="top"><%# DataBinder.Eval(Container.DataItem, "nodes")%></td>
                            <td valign="top"><%# DataBinder.Eval(Container.DataItem, "dr") %></td>
                            <td valign="top"><%# DataBinder.Eval(Container.DataItem, "ha") %></td>
                            <td valign="top" align="right">[<a href="javascript:void(0)" onclick="OpenCluster('<%# intID.ToString() %>','<%# DataBinder.Eval(Container.DataItem, "id") %>');">Edit</a>]<img src="/images/spacer.gif" border="0" width="10" height="1" />[<asp:LinkButton ID="btnDelete" runat="server" Text="Delete" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' OnClick="btnDelete_Click" />]</td>
                        </tr>
                    </AlternatingItemTemplate>
                </asp:repeater>
                <tr>
                    <td colspan="4">
                        <asp:Label ID="lblNone" runat="server" CssClass="default" Visible="false" Text="<img src='/images/spacer.gif' border='0' height='1' width='25' /><img src='/images/alert.gif' border='0' align='absmiddle'> You have not added any clusters to this request" />
                    </td>
                </tr>
            </table>
            </div>
        </td>
    </tr>
    <tr height="1">
        <td align="center" class="bigger">
            <asp:Panel ID="panInvalid" runat="server" Visible="false">
                <img src="/images/bigAlert.gif" border="0" align="absmiddle" /> <b>Invalid Configuration - Please Correct to Continue</b>
            </asp:Panel>
            <asp:Panel ID="panValid" runat="server" Visible="false">
                <img src="/images/bigCheck.gif" border="0" align="absmiddle" /> <b>Valid Configuration - Click Next to Continue!</b>
            </asp:Panel>
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
                        <asp:Panel ID="panNavigation" runat="server" Visible="false">
                            <asp:Button ID="btnBack" runat="server" OnClick="btnBack_Click" Text="Back" CssClass="default" Width="75" /> <asp:Button ID="btnNext" runat="server" OnClick="btnNext_Click" Text="Next" CssClass="default" Width="75" />
                        </asp:Panel>
                        <asp:Panel ID="panUpdate" runat="server" Visible="false">
                            <asp:Button ID="btnUpdate" runat="server" OnClick="btnUpdate_Click" Text="Update" CssClass="default" Width="75" /> <asp:Button ID="btnCancel" runat="server" OnClick="btnCancel_Click" Text="Cancel" CssClass="default" Width="75" />
                        </asp:Panel>
                    </td>
                    <td align="right"><asp:Button ID="btnClose" runat="server" Text="Finish Later" CssClass="default" Width="125" /></td>
                </tr>
            </table>
        </td>
    </tr>
</table>
