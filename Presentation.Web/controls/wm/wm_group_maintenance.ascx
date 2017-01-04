<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wm_group_maintenance.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.wm_group_maintenance" %>

<script type="text/javascript">
</script>
<asp:Panel ID="panWorkload" runat="server" Visible="false">
    <table width="100%" border="0" cellSpacing="0" cellPadding="0" class="default">
        <tr>
            <td>
                <table width="100%" border="0" cellSpacing="0" cellPadding="0">
                    <tr>
                        <td><asp:ImageButton ID="btnApprove" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/tool_approve.gif" OnClick="btnApprove_Click" /></td>
                        <td><asp:ImageButton ID="btnDeny" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/tool_deny.gif" OnClick="btnDeny_Click" /></td>
                        <td><asp:ImageButton ID="btnPrint" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/tool_print.gif" /></td>
                        <td><asp:ImageButton ID="btnSLA" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/tool_sla.gif" /></td>
                        <td width="100%">
                            <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                <tr>
                                    <td><img src="/images/tool_left.gif" border="0" width="6" height="26"></td>
                                    <td nowrap background="/images/tool_back.gif" width="100%"><img src="/images/tool_back.gif" border="0" width="2" height="26"></td>
                                    <td><img src="/images/tool_right.gif" border="0" width="6" height="26"></td>
                                </tr>
                            </table>
                        </td>
                        <td><asp:ImageButton ID="btnClose" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/tool_close.gif" /></td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td class="default">
                <table width="100%" border="0" cellSpacing="2" cellPadding="2" class="default">
                    <tr>
                        <td valign="top">
                            <table border="0" cellpadding="3" cellspacing="2">
                                <asp:Panel ID="panComplete" runat="server" Visible="false">
                                <tr>
                                    <td colspan="2" class="bigalert"><img src="/images/bigAlert.gif" border="0" align="absmiddle" /> Completed on <asp:Label ID="lblComplete" runat="server" CssClass="header" /></td>
                                </tr>
                                </asp:Panel>
                                <tr>
                                    <td><b>Requested By:</b></td>
                                    <td><asp:Label ID="lblRequestBy" runat="server" CssClass="default" /></td>
                                </tr>
                                <tr>
                                    <td><b>Requested On:</b></td>
                                    <td><asp:Label ID="lblRequestOn" runat="server" CssClass="default" /></td>
                                </tr>
                                <tr>
                                    <td><b>Request Type:</b></td>
                                    <td><asp:Label ID="lblType" runat="server" CssClass="default" /></td>
                                </tr>
                                <%=strDetails %>
                                <asp:Panel ID="panID" runat="server" Visible="false">
                                <tr>
                                    <td><b>Group ID:</b></td>
                                    <td><asp:TextBox ID="txtID" runat="server" CssClass="default" Width="150" MaxLength="50" /></td>
                                </tr>
                                </asp:Panel>
                                <asp:Panel ID="panUsers" runat="server" Visible="false">
                                    <td><b>Users:</b></td>
                                    <td><asp:CheckBoxList ID="chkUsers" runat="server" CssClass="default" RepeatDirection="Vertical" RepeatColumns="2" CellPadding="3" CellSpacing="2" /></td>
                                </asp:Panel>
                            </table>
                        </td>
                        <td valign="top" align="right"><iframe src="/frame/did_you_know.aspx" frameborder="0" scrolling="no" width="300" height="125"></iframe></td>
                    </tr>
                    <tr>
                        <td colspan="2"><img src="/images/alert.gif" border="0" align="absmiddle" /> <b>This is an account maintenance change request.</b></td>
                    </tr>
                    <tr>
                        <td colspan="2">You have the option of approving or denying this change using the buttons at the top of this window.</td>
                    </tr>
                    <tr>
                        <td colspan="2">&nbsp;</td>
                    </tr>
                    <tr>
                        <td colspan="2">Should you decide to deny this request, you must enter a reason here:</td>
                    </tr>
                    <tr>
                        <td colspan="2"><asp:TextBox ID="txtComments" runat="server" CssClass="default" Width="400" TextMode="MultiLine" Rows="7" /></td>
                    </tr>
                    <tr>
                        <td colspan="2" class="note"><b>NOTE:</b> Reason will be shown to the requestor.</td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Panel>
<asp:Panel ID="panDenied" runat="server" Visible="false">
    <table width="100%" cellpadding="2" cellspacing="2" border="0">
        <tr>
            <td><img src="/images/spacer.gif" border="0" width="15" height="1" /></td>
            <td class="header"><img src="/images/bigX.gif" border="0" align="absmiddle" /> Access Denied</td>
        </tr>
        <tr><td colspan="2">&nbsp;</td></tr>
        <tr>
            <td><img src="/images/spacer.gif" border="0" width="15" height="1" /></td>
            <td>You do not have rights to view this item.</td>
        </tr>
        <tr><td colspan="2">&nbsp;</td></tr>
        <tr>
            <td colspan="2"><hr size="1" noshade /></td>
        </tr>
        <tr>
            <td colspan="2" align="right">
                <table width="100%" cellpadding="0" cellspacing="0" border="0">
                    <tr>
                        <td class="footer"></td>
                        <td align="right"><asp:Button ID="btnDenied" runat="server" CssClass="default" Width="75" Text="Close" /></td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Panel>
