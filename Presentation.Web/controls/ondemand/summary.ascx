<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="summary.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.summary" %>

<script type="text/javascript">
</script>
<table width="100%" height="100%" cellpadding="0" cellspacing="0" border="0">
    <tr>
        <td>
            <div style="height:100%; overflow:auto">
            <table width="100%" cellpadding="4" cellspacing="0" border="0">
                <asp:Panel ID="panTest" runat="server" Visible="false">
                <tr>
                    <td colspan="3">
                        <table width="100%" cellpadding="0" cellspacing="5" border="0" style="border:solid 1px #999999" bgcolor="#f9f9f9">
                            <tr>
                                <td rowspan="2"><img src="/images/ico_error.gif" border="0" align="absmiddle" /></td>
                                <td class="header" width="100%" valign="bottom">Production Go Live Date Not Available!</td>
                            </tr>
                            <tr>
                                <td width="100%" valign="top">Production go live dates are only required for production bound hardware.</td>
                            </tr>
                        </table>
                    </td>
                </tr>
                </asp:Panel>
                <tr>
                    <td nowrap>Production Go Live Date:</td>
                    <td nowrap><asp:TextBox ID="txtDate" runat="server" CssClass="default" Width="100" MaxLength="10" /></td>
                    <td width="100%"><asp:ImageButton ID="imgDate" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calendar.gif" /></td>
                </tr>
                <tr>
                    <td colspan="3">
                        <table cellpadding="4" cellspacing="2" border="0">
                            <tr>
                                <td class="bold"><img src="/images/bigAlert.gif" border="0" align="absmiddle" /> NOTE: You can modify this date at anytime!</td>
                            </tr>
                            <tr>
                                <td>To modify the production live date, locate your design in Design Builder and click the <b>Production Live Date</b> link...</td>
                            </tr>
                            <tr>
                                <td><img src="/images/production.gif" border="0" align="absmiddle" /></td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            </div>
        </td>
    </tr>
    <tr height="1">
        <td>
            <table width="100%" cellpadding="4" cellspacing="2" border="0">
                <tr>
                    <td colspan="3"><hr size="1" noshade /></td>
                </tr>
                <tr>
                    <td class="required">* = Required Field</td>
                    <td align="center">
                        <asp:Panel ID="panNavigation" runat="server" Visible="false">
                            <asp:Button ID="btnBack" runat="server" OnClick="btnBack_Click" Text="Back" CssClass="default" Width="75" /> <asp:Button ID="btnNext" runat="server" OnClick="btnNext_Click" Text="Next" CssClass="default" Width="75" />
                        </asp:Panel>
                        <asp:Panel ID="panUpdate" runat="server" Visible="false">
                            <asp:Button ID="btnUpdate" runat="server" OnClick="btnUpdate_Click" Text="Update" CssClass="default" Width="75" /> <asp:Button ID="btnCancel" runat="server" OnClick="btnCancel_Click" Text="Cancel" CssClass="default" Width="75" />
                        </asp:Panel>
                    </td>
                    <td align="right"><asp:Button ID="btnClose" runat="server" Text="Finish Later" CssClass="default" Width="125" /></td>
                </tr>
            </table>
        </td>
    </tr>
</table>
