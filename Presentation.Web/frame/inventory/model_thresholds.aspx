<%@ Page Language="C#" MasterPageFile="~/frame.Master" AutoEventWireup="true" CodeBehind="model_thresholds.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.model_thresholds" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" Runat="Server">
<table width="100%" cellpadding="0" cellspacing="2" border="0">
    <tr>
        <td><asp:Label ID="lblName" runat="server" CssClass="header" /></td>
        <td align="right"><a href="<%=Request.Url.PathAndQuery + "&id=0" %>">Add New</a></td>
    </tr>
    <tr>
        <td colspan="2">
            <asp:Panel ID="panAll" runat="server" Visible="false">
                <table width="100%" cellpadding="4" cellspacing="0" border="0" align="center">
                    <tr>
                        <td align="center"><b><u></u></b></td>
                        <td align="center"><b><u>Quantity:</u></b></td>
                        <td align="center"><b><u>Number of Days:</u></b></td>
                        <td align="center"><b><u>Enabled:</u></b></td>
                    </tr>
                    <asp:repeater ID="rptView" runat="server">
                        <ItemTemplate>
                            <tr>
                                <td align="center"><input type="button" class="default" style="width:75px" value="Edit" onclick="window.navigate('<%# Request.Path %>?mid=<%# DataBinder.Eval(Container.DataItem, "propertyid") %>&id=<%# DataBinder.Eval(Container.DataItem, "id") %>');"></td>
                                <td align="center"><%# DataBinder.Eval(Container.DataItem, "qty_from")%> - <%# DataBinder.Eval(Container.DataItem, "qty_to")%></td>
                                <td align="center"><%# DataBinder.Eval(Container.DataItem, "number_days") %></td>
                                <td align="center"><asp:ImageButton ID="btnEnable" runat="server" ImageAlign="AbsMiddle" ImageUrl='<%# (DataBinder.Eval(Container.DataItem, "enabled").ToString() == "1") ? "/admin/images/enabled.gif" : "/admin/images/disabled.gif" %>' CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' OnClick="btnEnable_Click" /></td>
                            </tr>
                        </ItemTemplate>
                    </asp:repeater>
                    <tr>
                        <td colspan="4">
                            <asp:Label ID="lblNone" runat="server" CssClass="default" Visible="false" Text="<img src='/images/spacer.gif' border='0' height='1' width='25' /><img src='/images/alert.gif' border='0' align='absmiddle'> No Thresholds" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>

            <asp:Panel ID="panOne" runat="server" Visible="false">
                <table width="100%" cellpadding="4" cellspacing="0" border="0">
                    <tr>
                        <td nowrap>Quantity:</td>
                        <td width="100%"><asp:TextBox ID="txtFrom" runat="server" CssClass="default" Width="75" MaxLength="5" /> - <asp:TextBox ID="txtTo" runat="server" CssClass="default" Width="75" MaxLength="5" /></td>
                    </tr>
                    <tr>
                        <td nowrap>Number of Days:</td>
                        <td width="100%"><asp:TextBox ID="txtNumber" runat="server" CssClass="default" Width="75" MaxLength="5" /></td>
                    </tr>
                    <tr>
                        <td nowrap>Enabled:</td>
                        <td width="100%"><asp:CheckBox ID="chkEnabled" runat="server" CssClass="default" /></td>
                    </tr>
                    <tr>
                        <td nowrap></td>
                        <td width="100%">
                            <asp:Button ID="btnSubmit" runat="server" CssClass="default" Width="100" Text="Save" OnClick="btnSubmit_Click" /> 
                            <asp:Button ID="btnCancel" runat="server" CssClass="default" Width="100" Text="Cancel" OnClick="btnCancel_Click" /> 
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </td>
    </tr>
</table>
</asp:Content>
