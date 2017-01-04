<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="register_controls.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.register_controls" %>


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
                    <td rowspan="2"><img src="/images/user_add.gif" border="0" align="absmiddle" /></td>
                    <td class="header" width="100%" valign="bottom">Register a New User</td>
                </tr>
                <tr>
                    <td width="100%" valign="top">If an end-user does not exist in ClearView, use this page to register the user.</td>
                </tr>
            </table>
            <asp:Panel ID="panSearch" runat="server" Visible="false">
            <table width="100%" cellpadding="4" cellspacing="3" border="0">
                <asp:Panel ID="panNone" runat="server" Visible="false">
                <tr>
                    <td colspan="2" class="reddefault">There were no users found for the following query...</td>
                </tr>
                </asp:Panel>
                <tr>
                    <td nowrap>LAN ID:</td>
                    <td width="100%"><asp:TextBox ID="txtSearchXID" runat="server" Width="150" CssClass="default" MaxLength="10" /></td>
                </tr>
                <tr>
                    <td nowrap></td>
                    <td width="100%" class="footer"><b>NOTE:</b> The LAN ID can be either an X-ID (legacy National City) or a PNC ID (P-ID, XX-ID, etc.).</td>
                </tr>
                <tr>
                    <td nowrap></td>
                    <td width="100%" class="header">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;-- OR --</td>
                </tr>
                <!--
                <tr>
                    <td nowrap>Email (for PNC):</td>
                    <td width="100%"><asp:TextBox ID="txtSearchMail" runat="server" Width="350" CssClass="default" MaxLength="100" /></td>
                </tr>
                <tr>
                    <td nowrap></td>
                    <td width="100%" class="header">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;-- OR --</td>
                </tr>
                -->
                <tr>
                    <td nowrap>First Name:</td>
                    <td width="100%"><asp:TextBox ID="txtSearchFirst" runat="server" Width="250" CssClass="default" MaxLength="100" /></td>
                </tr>
                <tr>
                    <td nowrap></td>
                    <td width="100%" class="header">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;-- OR --</td>
                </tr>
                <tr>
                    <td nowrap>Last Name:</td>
                    <td width="100%"><asp:TextBox ID="txtSearchLast" runat="server" Width="250" CssClass="default" MaxLength="100" /></td>
                </tr>
                <tr>
                    <td nowrap class="required"></td>
                    <td width="100%"><asp:Button ID="btnSearch" Text="Search" Width="100" CssClass="default" runat="server" OnClick="btnSearch_Click" /></td>
                </tr>
            </table>
            </asp:Panel>
            <asp:Panel ID="panRegistered" runat="server" Visible="false">
            <table width="100%" cellpadding="4" cellspacing="3" border="0">
                <tr>
                    <td><img src="/images/check.gif" border="0" align="absmiddle" /> <b>This User is Already Registered!!</b></td>
                </tr>
                <tr>
                    <td>The user <asp:Label ID="lblUser" runat="server" CssClass="default" /> is already registered in ClearView.</td>
                </tr>
                <tr>
                    <td><asp:Button ID="btnBack" Text="Back" Width="100" CssClass="default" runat="server" OnClick="btnCancel_Click" /></td>
                </tr>
            </table>
            </asp:Panel>
            <asp:Panel ID="panRegister" runat="server" Visible="false">
            <table width="100%" cellpadding="4" cellspacing="3" border="0">
                <tr>
                    <td colspan="2"><img src="/images/check.gif" border="0" align="absmiddle" /> <b>ClearView successfully found the user!!</b></td>
                </tr>
                <tr>
                    <td colspan="2">Please complete or validate the following information to add the user to ClearView.</td>
                </tr>
                <tr>
                    <td nowrap>X-ID:<font class="required">&nbsp;*</font></td>
                    <td width="100%"><asp:TextBox ID="txtXID" runat="server" Width="150" CssClass="default" Enabled="false" /></td>
                </tr>
                <tr>
                    <td nowrap>PNC ID:<font class="required">&nbsp;*</font></td>
                    <td width="100%"><asp:TextBox ID="txtID" runat="server" Width="150" CssClass="default" Enabled="false" /></td>
                </tr>
                <tr>
                    <td nowrap>First Name:<font class="required">&nbsp;*</font></td>
                    <td width="100%"><asp:TextBox ID="txtFirst" runat="server" Width="250" CssClass="default" MaxLength="100" /></td>
                </tr>
                <tr>
                    <td nowrap>Last Name:<font class="required">&nbsp;*</font></td>
                    <td width="100%"><asp:TextBox ID="txtLast" runat="server" Width="250" CssClass="default" MaxLength="100" /></td>
                </tr>
                <tr>
                    <td nowrap>Manager:<font class="required">&nbsp;*</font></td>
                    <td width="100%">
                        <table cellpadding="0" cellspacing="0" border="0">
                            <tr>
                                <td><asp:TextBox ID="txtManager" runat="server" Width="250" CssClass="default" /></td>
                            </tr>
                            <tr>
                                <td>
                                    <div id="divManager" runat="server" style="overflow:hidden; position:absolute; display:none; background-color:#FFFFFF; border:solid 1px #CCCCCC">
                                        <asp:ListBox ID="lstManager" runat="server" CssClass="default" />
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td nowrap>Phone Number:</td>
                    <td width="100%"><asp:TextBox ID="txtPhone" runat="server" Width="150" CssClass="default" MaxLength="20" /></td>
                </tr>
                <tr>
                    <td nowrap class="required">* = Required Field</td>
                    <td width="100%">
                        <asp:Button ID="btnRegister" Text="Register" Width="100" CssClass="default" runat="server" OnClick="btnRegister_Click" /> 
                        <asp:Button ID="btnCancel" Text="Cancel" Width="100" CssClass="default" runat="server" OnClick="btnCancel_Click" />
                    </td>
                </tr>
            </table>
            </asp:Panel>
            <asp:Panel ID="panMultiple" runat="server" Visible="false">
            <table width="100%" cellpadding="4" cellspacing="3" border="0">
                <tr>
                    <td><img src="/images/alert.gif" border="0" align="absmiddle" /> <b>ClearView found multiple users!!</b></td>
                </tr>
                <tr>
                    <td>Please click on the user you want to add to ClearView.</td>
                </tr>
                <tr>
                    <td>
                        <table width="100%" cellpadding="4" cellspacing="0" border="0" style="border:solid 1px #CCCCCC">
                            <tr bgcolor="#EEEEEE">
                                <td nowrap><b>LAN ID</b></td>
                                <td nowrap><b>First Name</b></td>
                                <td nowrap><b>Last Name</b></td>
                            </tr>
                            <%=strMultiple %>
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
<asp:HiddenField ID="hdnManager" runat="server" />