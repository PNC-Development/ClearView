<%@ Page Language="C#" MasterPageFile="~/frame.Master" AutoEventWireup="true" CodeBehind="search_pnumber.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.search_pnumber" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" runat="server">
<script type="text/javascript">
    function UpdateProject(strName, strBD, strOrg, strNumber) {
        window.top.UpdateProjectInfo(strName, strBD, strOrg, strNumber);
        return false;
    }
</script>
    <table width="100%" height="100%" cellpadding="2" cellspacing="0" border="0">
        <tr>
            <td valign="top">
                <table width="100%" cellpadding="4" cellspacing="0" border="0" align="center">
                    <tr>
                        <td>&nbsp;</td>
                        <td class="result"><b>Project Number</b></td>
                        <td class="result"><b>Project Name</b></td>
                    </tr>
                    <asp:repeater ID="rptView" runat="server">
                        <ItemTemplate>
                            <tr onmouseover="CellRowOver(this);" onmouseout="CellRowOut(this);" onclick="UpdateProject('<%# DataBinder.Eval(Container.DataItem, "name") %>','<%# DataBinder.Eval(Container.DataItem, "bd") %>','<%# DataBinder.Eval(Container.DataItem, "organization") %>','<%# DataBinder.Eval(Container.DataItem, "number") %>')">
                                <td>&nbsp;</td>
                                <td><b><%# (DataBinder.Eval(Container.DataItem, "number").ToString() == "" ? "<i>Pending</i>" : DataBinder.Eval(Container.DataItem, "number").ToString()) %></b></td>
                                <td><%# DataBinder.Eval(Container.DataItem, "name") %></td>
                            </tr>
                        </ItemTemplate>
                    </asp:repeater>
                    <tr>
                        <td colspan="3">
                            <asp:Label ID="lblNone" runat="server" CssClass="default" Visible="false" Text="<img src='/images/alert.gif' border='0' align='absmiddle'> There are no projects that match your criteria" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
