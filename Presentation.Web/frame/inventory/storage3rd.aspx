<%@ Page Language="C#" MasterPageFile="~/frame.Master" AutoEventWireup="true" CodeBehind="storage3rd.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.storage3rd" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" runat="server">
            <table width="100%" cellpadding="3" cellspacing="0" border="0">
                <tr>
                    <td>
                        <table width="100%" height="100%" cellpadding="3" cellspacing="0" border="0" style="border:solid 1px #CCCCCC">
                            <tr bgcolor="#EEEEEE">
                                <td><b><u>RequestID:</u></b></td>
                                <td><b><u>Class:</u></b></td>
                                <td><b><u>Environment:</u></b></td>
                                <td><b><u>Location:</u></b></td>
                                <td><b><u>Performance:</u></b></td>
                                <td><b><u>Fabric:</u></b></td>
                                <td><b><u>Replicated:</u></b></td>
                                <td><b><u>HA:</u></b></td>
                                <td><b><u>Date:</u></b></td>
                                <td align="right"><b><u>Amount:</u></b></td>
                                <td align="right"><b><u>High Availability<br />Amount:</u></b></td>
                                <td align="right"><b><u>Replicated<br />Amount:</u></b></td>
                            </tr>
                            <%=strResults %>
                            <tr bgcolor="#EEEEEE">
                                <td colspan="9"></td>
                                <td align="right" class="bold"><asp:Label ID="lblAmount" runat="server" CssClass="bold" /> GB</td>
                                <td align="right" class="bold"><asp:Label ID="lblAmountHA" runat="server" CssClass="bold" /> GB</td>
                                <td align="right" class="bold"><asp:Label ID="lblAmountReplicated" runat="server" CssClass="bold" /> GB</td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
</asp:Content>
