<%@ Page Language="C#" MasterPageFile="~/frame.Master" AutoEventWireup="true" CodeBehind="design_approval.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.design_approval" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" Runat="Server">
<script type="text/javascript">
</script>
<table width="100%" height="100%" cellpadding="4" cellspacing="2" border="0">
    <tr height="1">
        <td>
            <table width="100%" cellpadding="0" cellspacing="5" border="0">
                <tr>
                    <td rowspan="2"><img src="/images/milestones.gif" border="0" align="absmiddle" /></td>
                    <td class="header" valign="bottom" width="100%">Design Approval</td>
                </tr>
                <tr>
                    <td valign="top" width="100%">Please review the design below and approve or deny it.</td>
                </tr>
            </table>
        </td>
    </tr>
    <tr id="trDesign">
        <td><iframe id="frmDesign" runat="server" frameborder="1" scrolling="yes" width="100%" height="100%" /></td>
    </tr>
    <tr id="trApprove" style="display:none">
        <td valign="top">
            <table width="100%" cellpadding="0" cellspacing="5" border="0" style="border:solid 1px #999999" bgcolor="#f9f9f9">
                <tr>
                    <td rowspan="2" valign="top"><img src="/images/ico_check.gif" border="0" align="absmiddle" /></td>
                    <td class="header" valign="bottom" width="100%">Thank You - Please Read the Following Disclaimer Notice</td>
                </tr>
                <tr>
                    <td valign="bottom" width="100%">
                        <p>By checking the following disclaimers, you are accepting the terms of each disclaimer notice.</p>
                        <p>If you do not agree with one or more items listed below you should <b>Deny this Design</b>.</p>
                        <p>
                            <asp:CheckBoxList ID="chkAgreeList" runat="server" CssClass="default" RepeatColumns="1" RepeatDirection="Vertical">
                                <asp:ListItem Value="The devices will begin to build as soon as this design is approved" />
                                <asp:ListItem Value="No further changes can be made to this design...<b>EVER!!!</b>" />
                                <asp:ListItem Value="Any mistake on this design could cause a decommission (which could take up to two weeks to process)" />
                                <asp:ListItem Value="Mistakes will not be the fault of the person entering the design, since you are required to approve it" />
                            </asp:CheckBoxList>
                        </p>
                        <p><asp:Button ID="btnApprove" runat="server" CssClass="default" Width="75" Text="Approve" /> <asp:Button ID="btnCancel1" runat="server" CssClass="default" Width="75" Text="Cancel" /></p>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr id="trDeny" style="display:none">
        <td valign="top">
            <table width="100%" cellpadding="0" cellspacing="5" border="0" style="border:solid 1px #999999" bgcolor="#f9f9f9">
                <tr>
                    <td rowspan="2" valign="top"><img src="/images/hugeAlert.gif" border="0" align="absmiddle" /></td>
                    <td class="header" valign="bottom" width="100%">Comments Required</td>
                </tr>
                <tr>
                    <td valign="bottom" width="100%">
                        <p>Please enter some comments for why this design is being denied...</p>
                        <p><asp:TextBox ID="txtComments" runat="server" CssClass="default" Rows="10" Width="500" TextMode="MultiLine" /></p>
                        <p>Denying this design means that the design will be sent back to the person who executed it for updating.</p>
                        <p><b>WARNNIG:</b> Denying this design will could cause some delay to your timeline.</p>
                        <p><asp:Button ID="btnDeny" runat="server" CssClass="default" Width="75" Text="Deny" /> <asp:Button ID="btnCancel2" runat="server" CssClass="default" Width="75" Text="Cancel" /></p>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr height="1">
        <td><hr size="1" noshade /></td>
    </tr>
    <tr height="1">
        <td align="center">
            <table cellpadding="0" cellspacing="5" border="0">
                <tr>
                    <td nowrap>
                        <table onmouseover="TableOver(this);" onmouseout="TableOut(this);" onclick="ShowHideDiv('trApprove','inline');ShowHideDiv('trDesign','none');ShowHideDiv('trDeny','none');" width="100%" cellpadding="0" cellspacing="5" border="0" style="border:solid 1px #999999">
                            <tr>
                                <td rowspan="2"><img src="/images/ico_check.gif" border="0" align="absmiddle" /></td>
                                <td class="header" width="100%" valign="bottom">Approve This Design</td>
                            </tr>
                            <tr>
                                <td width="100%" valign="top" nowrap>Click here to approve.</td>
                            </tr>
                        </table>
                    </td>
                    <td><img src="/images/spacer.gif" border="0" height="1" width="30" /></td>
                    <td nowrap>
                        <table onmouseover="TableOver(this);" onmouseout="TableOut(this);" onclick="ShowHideDiv('trDeny','inline');ShowHideDiv('trDesign','none');ShowHideDiv('trApprove','none');" width="100%" cellpadding="0" cellspacing="5" border="0" style="border:solid 1px #999999">
                            <tr>
                                <td rowspan="2"><img src="/images/ico_error.gif" border="0" align="absmiddle" /></td>
                                <td class="header" width="100%" valign="bottom">Deny This Design</td>
                            </tr>
                            <tr>
                                <td width="100%" valign="top" nowrap>Click here to deny.</td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
</div>
</asp:Content>
