<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="resource_request_workflow.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.resource_request_workflow" %>


<script type="text/javascript">
    function ShowHideAvailable(oDiv) {
        oDiv = document.getElementById(oDiv);
        if (oDiv.style.display == "inline")
            oDiv.style.display = "none";
        else
            oDiv.style.display = "inline";
        return false;
    }
    function Deny(oDeny, oApprove, oText) {
        oDeny = document.getElementById(oDeny);
        oDeny.style.display = "inline";
        SetFocusTry(document.getElementById(oText));
        oApprove = document.getElementById(oApprove);
        oApprove.style.display = "none";
        return false;
    }
    function Cancel(oDeny, oApprove) {
        oDeny = document.getElementById(oDeny);
        oDeny.style.display = "none";
        oApprove = document.getElementById(oApprove);
        oApprove.style.display = "inline";
        return false;
    }
</script>
<table width="100%" cellpadding="0" cellspacing="0" border="0" class="default">
    <tr>
        <td><img src="/images/table_topleft.gif" border="0" width="5" height="26"></td>
        <td nowrap background="/images/table_top.gif" class="greentableheader" width="100%">Approve Service Request</td>
        <td><img src="/images/table_topright.gif" border="0" width="5" height="26"></td>
    </tr>
    <tr>
        <td background="/images/table_left.gif"><img src="/images/table_left.gif" width="5" height="10"></td>
        <td width="100%" bgcolor="#FFFFFF">
            <br />
            <asp:Panel ID="panRequest" runat="server" Visible="false">
                <table width="100%" cellpadding="0" cellspacing="5" border="0">
                    <tr>
                        <td>
                            <table width="100%" cellpadding="2" cellspacing="2" border="0">
                                <tr>
                                    <td><asp:Label ID="lblView" runat="server" CssClass="default" /></td>
                                </tr>
                                <tr id="trHR" runat="server">
                                    <td><hr size="1" noshade /></td>
                                </tr>
                                <tr id="divApprove" runat="server" style="display:inline">
                                    <td>
                                        <asp:Button ID="btnApprove" runat="server" CssClass="default" Width="75" Text="Approve" OnClick="btnApprove_Click" /> 
                                        <asp:Button ID="btnDeny" runat="server" CssClass="default" Width="75" Text="Deny" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <asp:PlaceHolder ID="PHForm" runat="server" />
                <div id="divDeny" runat="server" style="display:none">
                    <table width="100%" border="0" cellSpacing="2" cellPadding="2" class="default">
                        <tr>
                            <td class="bigred">Rejection Reason:</td>
                        </tr>
                        <tr>
                            <td valign="top"><asp:TextBox ID="txtReason" runat="server" Width="500" TextMode="MultiLine" Rows="6" /></td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="btnDone" runat="server" CssClass="default" Width="75" Text="Done" OnClick="btnDone_Click" />
                                <asp:Button ID="btnCancel" runat="server" CssClass="default" Width="75" Text="Cancel" />
                            </td>
                        </tr>
                    </table>
                </div>
            </asp:Panel>
            <asp:Panel ID="panAlready" runat="server" Visible="false">
                <table width="100%" cellpadding="2" cellspacing="2" border="0">
                    <tr>
                        <td><img src="/images/spacer.gif" border="0" width="15" height="1" /></td>
                        <td class="header"><img src="/images/bigAlert.gif" border="0" align="absmiddle" /> Already Completed</td>
                    </tr>
                    <tr><td colspan="2">&nbsp;</td></tr>
                    <tr>
                        <td><img src="/images/spacer.gif" border="0" width="15" height="1" /></td>
                        <td>This request was already <asp:Literal id="litAlreadyStatus" runat="server" /> by <asp:Literal id="litAlreadyBy" runat="server" /><asp:Literal id="litAlreadyOn" runat="server" />.</td>
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
                                    <td align="right"><asp:Button ID="btnAlready" runat="server" CssClass="default" Width="75" Text="OK" OnClick="btnFinish_Click" /></td>
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
                                    <td align="right"><asp:Button ID="btnFinish" runat="server" CssClass="default" Width="75" Text="Finish" OnClick="btnFinish_Click" /></td>
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
<asp:Label ID="lblType" runat="server" Visible="false" />
<asp:Label ID="lblRequest" runat="server" Visible="false" />