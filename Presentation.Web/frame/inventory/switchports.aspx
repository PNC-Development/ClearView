<%@ Page Language="C#" MasterPageFile="~/frame.Master" AutoEventWireup="true" CodeBehind="switchports.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.switchports" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" Runat="Server">
<table width="100%" cellpadding="4" cellspacing="3" border="0">
    <tr>
        <td nowrap>Serial Number:</td>
        <td width="100%"><asp:Label ID="lblSerial" runat="server" CssClass="default" /></td>
    </tr>
    <tr>
        <td nowrap>Asset Tag:</td>
        <td width="100%"><asp:Label ID="lblAsset" runat="server" CssClass="default" /></td>
    </tr>
    <tr>
        <td nowrap>Model:</td>
        <td width="100%"><asp:Label ID="lblModel" runat="server" CssClass="default" /></td>
    </tr>
    <tr>
        <td colspan="2">&nbsp;</td>
    </tr>
    <tr>
        <td colspan="2" class="bold">Add a Switchport</td>
    </tr>
    <tr>
        <td nowrap>Type:</td>
        <td width="100%">
            <asp:DropDownList ID="ddlType" runat="server" Width="150">
                <asp:ListItem Value="0" Text="-- SELECT --" />
                <asp:ListItem Value="1" Text="Network" />
                <asp:ListItem Value="2" Text="Storage" />
                <asp:ListItem Value="3" Text="Backup" />
                <asp:ListItem Value="4" Text="Cluster" />
                <asp:ListItem Value="5" Text="Remote" />
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td nowrap>NIC #:</td>
        <td width="100%">
            <asp:DropDownList ID="ddlNIC" runat="server" Width="150">
                <asp:ListItem Value="0" Text="-- SELECT --" />
                <asp:ListItem Value="1" />
                <asp:ListItem Value="2" />
                <asp:ListItem Value="3" />
                <asp:ListItem Value="4" />
                <asp:ListItem Value="5" />
                <asp:ListItem Value="6" />
                <asp:ListItem Value="7" />
                <asp:ListItem Value="8" />
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td nowrap>Switch:</td>
        <td width="100%"><asp:DropDownList ID="ddlSwitch" runat="server" Width="150" /></td>
    </tr>
    <tr>
        <td nowrap>Blade / FEX ID:</td>
        <td width="100%">
            <asp:DropDownList ID="ddlBlade" runat="server" Width="150" Enabled="false" />
            <asp:TextBox ID="txtFexID" runat="server" Width="150" MaxLength="50" />
            (Nexus Format: 999/9)
        </td>
    </tr>
    <tr>
        <td nowrap>Port:</td>
        <td width="100%"><asp:DropDownList ID="ddlPort" runat="server" Width="150" Enabled="false" /></td>
    </tr>
    <tr>
        <td nowrap>Interface:</td>
        <td>
            <asp:TextBox ID="txtInterface" runat="server" Width="100" MaxLength="20" Enabled="false" />
            <asp:Label ID="lblInterface" runat="server" Text="<i>Not Configured</i>" />
        </td>
    </tr>
    <tr>
        <td nowrap></td>
        <td width="100%"><asp:Button ID="btnAdd" runat="server" CssClass="default" Width="75" Text="Add" OnClick="btnAdd_Click" /></td>
    </tr>
    <tr>
        <td colspan="2" class="bold">Current Switchports</td>
    </tr>
    <tr>
        <td colspan="2">
            <table width="100%" cellpadding="4" cellspacing="0" border="0">
                <tr bgcolor="#EEEEEE">
                    <td nowrap><b>Type</b></td>
                    <td nowrap><b>NIC #</b></td>
                    <td nowrap><b>Switch</b></td>
                    <td nowrap><b>Blade / FEX ID</b></td>
                    <td nowrap><b>Port</b></td>
                    <td nowrap><b>Interface</b></td>
                    <td nowrap></td>
                </tr>
                <asp:repeater ID="rptSwitchports" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td>
                                <%# 
                                    (DataBinder.Eval(Container.DataItem, "type").ToString() == "1" ? "Network" :
                                    (DataBinder.Eval(Container.DataItem, "type").ToString() == "2" ? "Storage" :
                                    (DataBinder.Eval(Container.DataItem, "type").ToString() == "3" ? "Backup" :
                                    (DataBinder.Eval(Container.DataItem, "type").ToString() == "4" ? "Cluster" :
                                    (DataBinder.Eval(Container.DataItem, "type").ToString() == "5" ? "Remote" :
                                    "Unknown"
                                    )))))
                                %>
                            </td>
                            <td><%# DataBinder.Eval(Container.DataItem, "nic") %></td>
                            <td><%# DataBinder.Eval(Container.DataItem, "name") %></td>
                            <td><%# (DataBinder.Eval(Container.DataItem, "interface").ToString() == "" ? "-->" : DataBinder.Eval(Container.DataItem, "blade")) %></td>
                            <td><%# (DataBinder.Eval(Container.DataItem, "interface").ToString() == "" ? "-->" : DataBinder.Eval(Container.DataItem, "port")) %></td>
                            <td><%# (DataBinder.Eval(Container.DataItem, "interface").ToString() == "" ? DataBinder.Eval(Container.DataItem, "blade").ToString() + "/" + DataBinder.Eval(Container.DataItem, "port").ToString() : DataBinder.Eval(Container.DataItem, "interface"))%></td>
                            <td align="right">[<asp:LinkButton ID="btnDelete" runat="server" OnClick="btnDelete_Click" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "switchportid") %>' Text="Delete" />]</td>
                        </tr>
                    </ItemTemplate>
                </asp:repeater>
                <tr>
                    <td colspan="7">
                        <asp:Label ID="lblSwitchports" runat="server" CssClass="default" Visible="false" Text="<img src='/images/alert.gif' border='0' align='absmiddle'> There are no items" />
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
<input type="hidden" id="hdnEnvironment" runat="server" />
<asp:HiddenField ID="hdnBlade" runat="server" />
<asp:HiddenField ID="hdnPort" runat="server" />
</asp:Content>
