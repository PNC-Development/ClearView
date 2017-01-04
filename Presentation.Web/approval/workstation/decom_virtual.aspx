<%@ Page Language="C#" MasterPageFile="~/clearview.Master" AutoEventWireup="true" CodeBehind="decom_virtual.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.decom_virtual" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" Runat="Server">
<script type="text/javascript">
var strCheckedRows = "";
function ApproveCheckBox(oCheck, oValue, oHidden) {
    var strCheck = "0";
    if (oCheck.checked == true) {
        strCheck = "1";
    }
    UpdateHiddenItems(oHidden, oValue, strCheck);
}
</script>
<table width="100%" height="100%" border="0" cellspacing="0" cellpadding="0">
    <tr height="1"> 
        <td bgcolor="#007253"><img src="/images/header_wide.aspx?t=Virtual Workstation Account Approval" border="0" /></td>
        <td bgcolor="#007253" align="right"><asp:PlaceHolder ID="PH4" runat="server" /></td>
    </tr>
    <tr bgcolor="#000000" height="1"> 
        <td colspan="2" height="26" background="/images/button_back.gif" colspan="5"><asp:PlaceHolder ID="PH1" runat="server" /></td>
    </tr>
    <tr> 
        <td colspan="2" bgcolor="E9E9E9">
            <table width="98%" height="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                <tr height="1">
                    <td align="left" valign="top">&nbsp;</td>
                </tr>
                <tr> 
                    <td align="left" valign="top">
                        <table width="100%" cellpadding="0" cellspacing="0" border="0">
                            <tr>
                                <td align="center">
                                    <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                        <tr>
                                            <td><img src="/images/table_topleft.gif" border="0" width="5" height="26"></td>
                                            <td nowrap background="/images/table_top.gif" class="greentableheader" width="100%">Approval</td>
                                            <td><img src="/images/table_topright.gif" border="0" width="5" height="26"></td>
                                        </tr>
                                        <tr>
                                            <td background="/images/table_left.gif"><img src="/images/table_left.gif" width="5" height="10"></td>
                                            <td width="100%" bgcolor="#FFFFFF">
                                                <br />
                                                <asp:Panel ID="panPermit" runat="server" Visible="false">
                                                <table width="100%" cellpadding="4" cellspacing="3" border="0">
                                                    <tr>
                                                        <td nowrap>Workstation Name:</td>
                                                        <td width="100%"><asp:Label ID="lblName" runat="server" CssClass="default" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td nowrap>Workstation Manager:</td>
                                                        <td width="100%"><asp:Label ID="lblManager" runat="server" CssClass="default" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">&nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">The following list represents accounts that have been requested and are yet to be approved / denied. Please check the corresponding option to mark and account approved or denied and click <b>Submit</b> to complete this request.</td>
                                                    </tr>
                                                    <tr>
                                                        <td nowrap valign="top">Accounts:</td>
                                                        <td width="100%" valign="top">
                                                            <table width="500" cellpadding="3" cellspacing="0" border="0" style="border:solid 1px #CCCCCC">
                                                                <tr bgcolor="#CCCCCC">
                                                                    <td class="bold" width="50%" nowrap>Name</td>
                                                                    <td class="bold" width="50%" nowrap>Access</td>
                                                                    <td class="bold" nowrap>Approve</td>
                                                                    <td class="bold" nowrap>Deny</td>
                                                                </tr>
                                                                <%=strAccounts %>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td nowrap valign="top">Comments:<br /><br />(Optional)</td>
                                                        <td width="100%" valign="top"><asp:TextBox ID="txtComments" runat="server" CssClass="default" Width="500" TextMode="multiLine" Rows="5" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td nowrap></td>
                                                        <td width="100%"><asp:Button ID="btnSave" runat="server" CssClass="default" Width="100" Text="Submit" OnClick="btnSave_Click" /></td>
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
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
<asp:HiddenField ID="hdnApprove" runat="server" />
<asp:HiddenField ID="hdnDeny" runat="server" />
</asp:Content>