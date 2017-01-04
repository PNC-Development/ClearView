<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="workflow_csrc.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.workflow_csrc" %>


<script type="text/javascript">
    function ShowDetail(oDiv) {
        oDiv = document.getElementById(oDiv);
        if (oDiv.style.display == "inline")
            oDiv.style.display = "none";
        else
            oDiv.style.display = "inline";
    }
</script>
<table width="100%" cellpadding="0" cellspacing="0" border="0" class="default">
    <tr>
        <td><img src="/images/table_topleft.gif" border="0" width="5" height="26"></td>
        <td nowrap background="/images/table_top.gif" class="greentableheader" width="100%">CSRC Workflow</td>
        <td><img src="/images/table_topright.gif" border="0" width="5" height="26"></td>
    </tr>
    <tr>
        <td background="/images/table_left.gif"><img src="/images/table_left.gif" width="5" height="10"></td>
        <td width="100%" bgcolor="#FFFFFF">
            <br />
            <asp:Panel ID="panWorkflow" runat="server" Visible="false">
            <table width="100%" cellpadding="0" cellspacing="5" border="0">
                <tr>
                    <td rowspan="2"><img src="/images/workflow.gif" border="0" align="absmiddle" /></td>
                    <td class="hugeheader" width="100%" valign="bottom">CSRC Approval Workflow</td>
                </tr>
                <tr>
                    <td width="100%" valign="top">CSRC Forms are submitted and routed for approval.  You are required to make a decision on the following CSRC form by using the buttons below.</td>
                </tr>
            </table>
                <table width="100%" cellpadding="4" cellspacing="3" border="0">
                    <tr>
                        <td><%=strDetails %></td>
                    </tr>
                    <tr>
                        <td><hr size="1" noshade /></td>
                    </tr>
                    <tr>
                        <td align="right">
                            <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                <tr>
                                    <td valign="top">
                                        <table cellpadding="2" cellspacing="1" border="0">
                                            <tr>
                                                <td><b>Comments:</b></td>
                                            </tr>
                                            <tr>
                                                <td><asp:TextBox ID="txtComments" runat="server" Width="400" Rows="5" TextMode="MultiLine" CssClass="default" /></td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td valign="top" align="right">
                                        <table cellpadding="2" cellspacing="1" border="0">
                                            <tr>
                                                <td><asp:Button ID="btnApprove" runat="server" CssClass="default" Width="75" Text="Approve" CommandArgument="1" OnClick="btnSubmit_Click" /></td>
                                                <td><asp:Button ID="btnDeny" runat="server" CssClass="default" Width="75" Text="Deny" CommandArgument="-1" OnClick="btnSubmit_Click" /></td>
                                            </tr>
                                        </table>
                                    </td>
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
                                    <td align="right"><asp:Button ID="btnClose" runat="server" CssClass="default" Width="75" Text="Close" /></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="panFinish" runat="server" Visible="false">
                <table width="100%" cellpadding="2" cellspacing="2" border="0">
                    <tr>
                        <td><img src="/images/spacer.gif" border="0" width="15" height="1" /></td>
                        <td class="header"><img src="/images/bigCheck.gif" border="0" align="absmiddle" /> Record Updated</td>
                    </tr>
                    <tr><td colspan="2">&nbsp;</td></tr>
                    <tr>
                        <td><img src="/images/spacer.gif" border="0" width="15" height="1" /></td>
                        <td>Your information has been saved successfully.</td>
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
                                    <td align="right"><asp:Button ID="btnFinish" runat="server" CssClass="default" Width="75" Text="Finish" /></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <p>&nbsp;</p>
        </td>
        <td background="/images/table_right.gif"><img src="/images/table_right.gif" width="5" height="10"></td>
    </tr>
    <tr>
        <td><img src="/images/table_bottomLeft.gif" border="0" width="5" height="9"></td>
        <td width="100%" background="/images/table_bottom.gif"></td>
        <td><img src="/images/table_bottomRight.gif" border="0" width="5" height="9"></td>
    </tr>
</table>
<asp:Label ID="lblStep" runat="server" Visible="false" />
<asp:Label ID="lblId" runat="server" Visible="false" />
<asp:Label ID="lblDetailId" runat="server" Visible="false" />