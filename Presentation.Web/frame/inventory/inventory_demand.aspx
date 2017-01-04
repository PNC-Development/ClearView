<%@ Page Language="C#" MasterPageFile="~/frame.Master" AutoEventWireup="true" CodeBehind="inventory_demand.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.inventory_demand" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" runat="server">
            <table width="100%" cellpadding="3" cellspacing="0" border="0">
                <tr height="1">
                    <td class="header"><asp:Label ID="lblModel" runat="server" CssClass="header" /></td>
                </tr>
                <tr>
                    <td>
                        <table width="100%" height="100%" cellpadding="3" cellspacing="0" border="0" style="border:solid 1px #CCCCCC">
                            <tr bgcolor="#EEEEEE">
                                <td></td>
                                <td><b><u>ID:</u></b></td>
                                <td><b><u>Project Name:</u></b></td>
                                <td><b><u>Project Number:</u></b></td>
                                <td><b><u>Confidence:</u></b></td>
                                <td><b><u>Commitment Date:</u></b></td>
                                <td><b><u>Class:</u></b></td>
                                <td><b><u>Environment:</u></b></td>
                                <td><b><u>Location:</u></b></td>
                                <td><b><u>XID:</u></b></td>
                                <td align="right"><b><u>Count:</u></b></td>
                                <td align="right"><b><u>DR:</u></b></td>
                            </tr>
                            <%=strResults %>
                            <tr>
                                <td colspan="11" class="default">
                                    <asp:Label ID="lblNone" runat="server" Visible="false" Text="<img src='/images/spacer.gif' border='0' height='1' width='25' /><img src='/images/alert.gif' border='0' align='absmiddle'> There is no supply for this model" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
</asp:Content>
