<%@ Page Language="C#" MasterPageFile="~/frame.Master" AutoEventWireup="true" CodeBehind="enhancement_approvals_groups.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.enhancement_approvals_groups" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" runat="server">
<script type="text/javascript">
</script>
    <table width="100%" border="0" cellSpacing="2" cellPadding="2">
        <tr>
            <td nowrap>Approval Group:</td>
            <td width="100%"><asp:DropDownList ID="ddlGroup" runat="server" /></td>
        </tr>
        <tr>
            <td>&nbsp;</td>
            <td>
                <asp:Button ID="btnAdd" runat="server" CssClass="default" Width="75" Text="Add" OnClick="btnAdd_Click" />
                <asp:Button ID="btnClose" runat="server" CssClass="default" Width="75" Text="Close" />
            </td>
        </tr>
        <tr>
            <td colspan="2" valign="top">
                <table width="100%" cellpadding="4" cellspacing="0" border="0" style="border:solid 1px #CCCCCC; background-color:#FFFFFF">
                    <tr bgcolor="#EEEEEE">
                        <td nowrap><b>Approval Group</b></td>
                        <td nowrap><b></b></td>
                    </tr>
                    <asp:repeater ID="rptItems" runat="server">
                        <ItemTemplate>
                            <tr>
                                <td><%# DataBinder.Eval(Container.DataItem, "name") %></td>
                                <td align="right">[<asp:LinkButton ID="btnDelete" runat="server" CssClass="default" Text="Delete" OnClick="btnDelete_Click" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' />]</td>
                            </tr>
                        </ItemTemplate>
                    </asp:repeater>
                    <tr>
                        <td colspan="3">
                            <asp:Label ID="lblNone" runat="server" CssClass="default" Visible="false" Text="<img src='/images/alert.gif' border='0' align='absmiddle'> There are no approval groups..." />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
