<%@ Page Language="C#" MasterPageFile="~/frame.Master" AutoEventWireup="true" CodeBehind="search_archived.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.search_archived" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" runat="server">
<script type="text/javascript">
    function UpdateText(strValue) {
        window.top.UpdateTextInfo(strValue);
        return false;
    }
</script>
    <table width="100%" height="100%" cellpadding="2" cellspacing="0" border="0">
        <tr>
            <td valign="top">
                <table width="100%" cellpadding="4" cellspacing="0" border="0" align="center">
                    <asp:repeater ID="rptView" runat="server">
                        <ItemTemplate>
                            <tr onmouseover="CellRowOver(this);" onmouseout="CellRowOut(this);" onclick="UpdateText('<%# DataBinder.Eval(Container.DataItem, "servername") %>')">
                                <td><%# DataBinder.Eval(Container.DataItem, "servername")%></td>
                            </tr>
                        </ItemTemplate>
                    </asp:repeater>
                    <tr>
                        <td>
                            <asp:Label ID="lblNone" runat="server" CssClass="default" Visible="false" Text="<img src='/images/alert.gif' border='0' align='absmiddle'> No servers match your criteria" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
