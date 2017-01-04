<%@ Page Language="C#" MasterPageFile="~/frame.Master" AutoEventWireup="true" CodeBehind="host_virtual_configure_os.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.host_virtual_configure_os" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" Runat="Server">
<script type="text/javascript">
</script>
<table width="100%" cellpadding="4" cellspacing="0" border="0">
    <tr>
        <td colspan="2" class="header">Configure a Virtual Host Operating Systems</td>
    </tr>
    <tr>
        <td nowrap>Operating System:</td>
        <td width="100%"><asp:DropDownList ID="ddlOS" runat="server" CssClass="default" /></td>
    </tr>
    <tr>
        <td nowrap>Virtual Directory:</td>
        <td width="100%"><asp:TextBox ID="txtVirtual" runat="server" CssClass="default" Width="400" MaxLength="100" /></td>
    </tr>
    <tr>
        <td nowrap></td>
        <td width="100%" class="footer">Example: R:\vsfiles\LUN01\WXP</td>
    </tr>
    <tr>
        <td nowrap>GZip Path:</td>
        <td width="100%"><asp:TextBox ID="txtGZip" runat="server" CssClass="default" Width="400" MaxLength="100" /></td>
    </tr>
    <tr>
        <td nowrap></td>
        <td width="100%" class="footer">Example: F:\WXP_SysPrep\gzip.exe</td>
    </tr>
    <tr>
        <td nowrap></td>
        <td width="100%"><asp:TextBox ID="txtImage" runat="server" CssClass="default" Width="300" MaxLength="100" /></td>
    </tr>
    <tr>
        <td nowrap></td>
        <td width="100%" class="footer">Example: F:\WXP_SysPrep\WXP Pro SP2 CORPTEST.vhd.gz</td>
    </tr>
    <tr>
        <td nowrap></td>
        <td width="100%"><asp:Button ID="btnSave" runat="server" CssClass="default" Width="100" Text="Save" OnClick="btnSave_Click" /></td>
    </tr>
    <tr>
        <td colspan="2">
            <table width="100%" cellpadding="4" cellspacing="0" border="0">
                <tr bgcolor="#EEEEEE">
                    <td nowrap><b>Operating System</b></td>
                    <td nowrap><b>Virtual Dir</b></td>
                    <td nowrap><b>GZip Path</b></td>
                    <td nowrap><b>Image</b></td>
                    <td nowrap><b></b></td>
                </tr>
                <asp:repeater ID="rptOS" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td><%# DataBinder.Eval(Container.DataItem, "os") %></td>
                            <td><%# DataBinder.Eval(Container.DataItem, "virtualdir")%></td>
                            <td><%# DataBinder.Eval(Container.DataItem, "gzippath")%></td>
                            <td><%# DataBinder.Eval(Container.DataItem, "image") %></td>
                            <td align="right">[<asp:LinkButton ID="btnDelete" runat="server" OnClick="btnDelete_Click" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' Text="Delete" />]</td>
                        </tr>
                    </ItemTemplate>
                </asp:repeater>
                <tr>
                    <td colspan="3">
                        <asp:Label ID="lblNone" runat="server" CssClass="default" Visible="false" Text="<img src='/images/alert.gif' border='0' align='absmiddle'> There are no items" />
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
</asp:Content>
