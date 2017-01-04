<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="sys_will_down.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.sys_will_down" %>


<asp:Panel ID="panDown" runat="server" Visible="false">
<table width="100%" cellpadding="2" cellspacing="0" border="0" class="default">
    <tr><td>&nbsp;</td></tr>
    <tr>
        <td class="bigalert" align="center" bgcolor="#FFFFFF">
            <table width="100%" cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td align="center">
                        <table cellpadding="0" cellspacing="5" border="0">
                            <tr>
                                <td rowspan="2"><img src="/images/bigAlert.gif" border="0" align="absmiddle" /></td>
                                <td class="biggestbold" valign="bottom">ClearView will be unavailable <asp:Label ID="lblDate" runat="server" CssClass="biggestbold" /> at <asp:Label ID="lblTime" runat="server" CssClass="biggestbold" /> for routine maintenance</td>
                            </tr>
                            <tr>
                                <td valign="top">Please <a href='<%=oVariable.Community() %>'>visit the ClearView community page</a> for more information.</td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
</asp:Panel>
<asp:Panel ID="panError" runat="server" Visible="false">
<table width="100%" cellpadding="2" cellspacing="0" border="0" class="default">
    <tr><td>&nbsp;</td></tr>
    <tr>
        <td class="bigerror" align="center" bgcolor="#FFFFFF">
            <table width="100%" cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td align="center">
                        <table cellpadding="0" cellspacing="5" border="0">
                            <tr>
                                <td rowspan="2"><img src="/images/bigError.gif" border="0" align="absmiddle" /></td>
                                <td class="biggestbold" valign="bottom">ClearView encountered an Error</td>
                            </tr>
                            <tr>
                                <td valign="top"><%=strError %></td>
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                                <td valign="top"><asp:Button ID="btnError" runat="server" CssClass="default" Text="Fixed" Width="75" OnClick="btnError_Click" /></td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
</asp:Panel>