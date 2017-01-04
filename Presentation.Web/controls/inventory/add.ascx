<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="add.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.add" %>

<script type="text/javascript">
</script>
<table width="100%" cellpadding="4" cellspacing="0" border="0">
    <tr>
        <td class="header">Deploy Devices</td>
    </tr>
    <tr>
        <td>Here are the devices that have been received and are ready for deployment.</td>
    </tr>
    <tr>
        <td>
            <table width="100%" cellpadding="3" cellspacing="0" border="0" style="border:solid 1px #CCCCCC">
                <tr bgcolor="#EEEEEE">
                    <td><b><u>Tracking Number:</u></b></td>
                    <td><b><u>Model:</u></b></td>
                    <td><b><u>Serial Number:</u></b></td>
                    <td><b><u>Asset Tag:</u></b></td>
                    <td><b><u></u></b></td>
                </tr>
                <asp:repeater ID="rptDevices" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td><%# DataBinder.Eval(Container.DataItem, "tracking") %></td>
                            <td><%# DataBinder.Eval(Container.DataItem, "modelname") %></td>
                            <td><%# DataBinder.Eval(Container.DataItem, "serial") %></td>
                            <td><%# DataBinder.Eval(Container.DataItem, "asset") %></td>
                            <td><asp:Button ID="btnReturn" runat="server" Text="Return" CssClass="default" Width="100" OnClick="btnReturn_Click" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' CommandName='<%# DataBinder.Eval(Container.DataItem, "modelid") %>' Enabled="false" /> <asp:Button ID="btnDeploy" runat="server" Text="Deploy" CssClass="default" Width="100" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' CommandName='<%# DataBinder.Eval(Container.DataItem, "modelid") %>' Enabled="false" /></td>
                        </tr>
                    </ItemTemplate>
                    <AlternatingItemTemplate>
                        <tr bgcolor="#F6F6F6">
                            <td><%# DataBinder.Eval(Container.DataItem, "tracking") %></td>
                            <td><%# DataBinder.Eval(Container.DataItem, "modelname") %></td>
                            <td><%# DataBinder.Eval(Container.DataItem, "serial") %></td>
                            <td><%# DataBinder.Eval(Container.DataItem, "asset") %></td>
                            <td><asp:Button ID="btnReturn" runat="server" Text="Return" CssClass="default" Width="100" OnClick="btnReturn_Click" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' CommandName='<%# DataBinder.Eval(Container.DataItem, "modelid") %>' Enabled="false" /> <asp:Button ID="btnDeploy" runat="server" Text="Deploy" CssClass="default" Width="100" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' CommandName='<%# DataBinder.Eval(Container.DataItem, "modelid") %>' Enabled="false" /></td>
                        </tr>
                    </AlternatingItemTemplate>
                </asp:repeater>
                <tr>
                    <td colspan="5" class="default">
                        <asp:Label ID="lblNone" runat="server" Visible="false" Text="<img src='/images/spacer.gif' border='0' height='1' width='25' /><img src='/images/alert.gif' border='0' align='absmiddle'> There are no devices" />
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
<input type="hidden" id="hdnParent" runat="server" />
<input type="hidden" id="hdnEnvironment" runat="server" />
<input type="hidden" id="hdnOrderAddress" runat="server" />
<input type="hidden" id="hdnOrderEnvironment" runat="server" />
