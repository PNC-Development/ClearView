<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="support_controls.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.support_controls" %>


<script type="text/javascript">
</script>
<table width="100%" cellpadding="0" cellspacing="0" border="0" class="default">
    <tr>
        <td><img src="/images/table_topleft.gif" border="0" width="5" height="26"></td>
        <td nowrap background="/images/table_top.gif" class="greentableheader" width="100%"><asp:Label ID="lblTitle" runat="server" CssClass="greentableheader" /></td>
        <td><img src="/images/table_topright.gif" border="0" width="5" height="26"></td>
    </tr>
    <tr>
        <td background="/images/table_left.gif"><img src="/images/table_left.gif" width="5" height="10"></td>
        <td width="100%" bgcolor="#FFFFFF">
            <br />
            <table width="100%" cellpadding="0" cellspacing="5" border="0">
                <tr>
                    <td rowspan="2"><img src="/images/ticket.gif" border="0" align="absmiddle" /></td>
                    <td class="header" width="100%" valign="bottom">Support Request</td>
                </tr>
                <tr>
                    <td width="100%" valign="top">If you are experiencing issues, or have an idea to enhance a current module, please select the module and complete the following form.</td>
                </tr>
            </table>
            <asp:Panel ID="panEdit" runat="server" Visible="false">
            <table width="100%" cellpadding="4" cellspacing="3" border="0">
                <tr>
                    <td nowrap>Title:</td>
                    <td width="100%"><asp:TextBox ID="txtTitle" runat="server" CssClass="default" Width="300" MaxLength="50" /></td>
                </tr>
                <tr>
                    <td nowrap>Module:<font class="required">&nbsp;*</font></td>
                    <td width="100%"><asp:DropDownList ID="ddlModule" runat="server" CssClass="default" /></td>
                </tr>
                <tr>
                    <td nowrap>Request Type:<font class="required">&nbsp;*</font></td>
                    <td width="100%">
                        <asp:RadioButton ID="radSuggestion" runat="server" CssClass="default" Text="Suggestion" GroupName="type" /> 
                        <asp:RadioButton ID="radIssue" runat="server" CssClass="default" Text="Issue" GroupName="type" />
                    </td>
                </tr>
                <tr>
                    <td nowrap valign="top">Description:<font class="required">&nbsp;*</font></td>
                    <td width="100%"><asp:TextBox ID="txtDescription" runat="server" CssClass="default" Width="70%" TextMode="MultiLine" Rows="7" /></td>
                </tr>
                <tr>
                    <td nowrap>Requested By:</td>
                    <td width="100%"><asp:Label ID="lblRequestBy" runat="server" CssClass="default" /></td>
                </tr>
                <tr>
                    <td nowrap>Requested On:</td>
                    <td width="100%"><asp:Label ID="lblRequestOn" runat="server" CssClass="default" /></td>
                </tr>
                <asp:Panel ID="panEditAdmin" runat="server" Visible="false">
                <tr>
                    <td nowrap>Status:</td>
                    <td width="100%"><asp:Label ID="lblEditStatus" runat="server" CssClass="default" /></td>
                </tr>
                <tr height="1">
                    <td colspan="2" style="border-bottom:dashed 1px #CCCCCC"><img src="/images/spacer.gif" border="0" width="1" height="1" /></td>
                </tr>
                <tr>
                    <td colspan="2"><b>Administrative Comments:</b></td>
                </tr>
                <tr>
                    <td colspan="2"><asp:Label ID="lblEditComments" runat="server" CssClass="default" /></td>
                </tr>
                </asp:Panel>
                <tr>
                    <td colspan="2"><hr size="1" noshade /></td>
                </tr>
                <tr>
                    <td nowrap class="required">* = Required Field</td>
                    <td align="right"><asp:Button ID="btnSubmit" runat="server" CssClass="default" Width="75" Text="Save" OnClick="btnSubmit_Click" /></td>
                </tr>
            </table>
            </asp:Panel>
            <asp:Panel ID="panView" runat="server" Visible="false">
            <table width="100%" cellpadding="4" cellspacing="3" border="0">
                <tr>
                    <td nowrap>Title:</td>
                    <td width="100%"><asp:Label ID="lblName" runat="server" CssClass="default" /></td>
                </tr>
                <tr>
                    <td nowrap>Module:</td>
                    <td width="100%"><asp:Label ID="lblModule" runat="server" CssClass="default" /></td>
                </tr>
                <tr>
                    <td nowrap>Request Type:</td>
                    <td width="100%"><asp:Label ID="lblType" runat="server" CssClass="default" /></td>
                </tr>
                <tr>
                    <td nowrap valign="top">Description:</td>
                    <td width="100%"><asp:Label ID="lblDescription" runat="server" CssClass="default" /></td>
                </tr>
                <tr>
                    <td nowrap>Requested By:</td>
                    <td width="100%"><asp:Label ID="lblBy" runat="server" CssClass="default" /></td>
                </tr>
                <tr>
                    <td nowrap>Requested On:</td>
                    <td width="100%"><asp:Label ID="lblOn" runat="server" CssClass="default" /></td>
                </tr>
                <tr>
                    <td nowrap>Status:</td>
                    <td width="100%"><asp:Label ID="lblViewStatus" runat="server" CssClass="default" /></td>
                </tr>
                <tr height="1">
                    <td colspan="2" style="border-bottom:dashed 1px #CCCCCC"><img src="/images/spacer.gif" border="0" width="1" height="1" /></td>
                </tr>
                <tr>
                    <td colspan="2"><b>Administrative Comments:</b></td>
                </tr>
                <tr>
                    <td colspan="2"><asp:Label ID="lblViewComments" runat="server" CssClass="default" /></td>
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
<asp:Label ID="lblId" runat="server" Visible="false" />