<%@ Page Language="C#" MasterPageFile="~/frame.Master" AutoEventWireup="true" CodeBehind="hba.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.hba" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" Runat="Server">
<table width="100%" cellpadding="4" cellspacing="3" border="0">
    <tr>
        <td nowrap>Tracking Number:</td>
        <td width="100%"><asp:Label ID="lblTracking" runat="server" CssClass="default" /></td>
    </tr>
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
        <td colspan="2" class="bold">Add an HBA</td>
    </tr>
    <tr>
        <td nowrap>Name:</td>
        <td width="100%"><asp:TextBox ID="txtName" runat="server" CssClass="default" Width="200" MaxLength="100" /></td>
    </tr>
    <tr>
        <td nowrap></td>
        <td width="100%"><asp:Button ID="btnAdd" runat="server" CssClass="default" Width="75" Text="Add" OnClick="btnAdd_Click" /></td>
    </tr>
    <tr>
        <td colspan="2" class="bold">Current HBAs</td>
    </tr>
    <tr>
        <td colspan="2">
            <table width="100%" cellpadding="4" cellspacing="0" border="0">
                <tr bgcolor="#EEEEEE">
                    <td nowrap><b>Name</b></td>
                    <td nowrap></td>
                </tr>
                <asp:repeater ID="rptHBA" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td><%# DataBinder.Eval(Container.DataItem, "name") %></td>
                            <td align="right">[<asp:LinkButton ID="btnDelete" runat="server" OnClick="btnDelete_Click" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' Text="Delete" />]</td>
                        </tr>
                    </ItemTemplate>
                </asp:repeater>
                <tr>
                    <td colspan="7">
                        <asp:Label ID="lblHBA" runat="server" CssClass="default" Visible="false" Text="<img src='/images/alert.gif' border='0' align='absmiddle'> There are no items" />
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
<input type="hidden" id="hdnEnvironment" runat="server" />
</asp:Content>
