<%@ Page Language="C#" MasterPageFile="~/clearview.Master" AutoEventWireup="true" CodeBehind="register.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.register" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" Runat="Server">
<script type="text/javascript">
</script>
<table width="100%" height="100%" border="0" cellspacing="0" cellpadding="0">
    <tr height="1" style="display:none">
        <td bgcolor="#007253"><img src="/images/header_wide.aspx" border="0" /></td>
        <td bgcolor="#007253" align="right"></td>
    </tr>
    <tr height="1"> 
        <td colspan="2">
            <table width="100%" border="0" cellspacing="0" cellpadding="5">
                <tr style="background-color:#FFFFFF; display:inline">
                    <td><img src="/images/PNCHeaderLogo.gif" border="0" /></td>
                    <td align="right"><img src="/images/HeaderGradient.gif" border="0" /></td>
                </tr>
                <tr style="background-color:#FFFFFF; display:none">
                    <td background="/images/PNCLogoBack.gif" width="100%"><img src="/images/PNCLogo.gif" border="0" /></td>
                    <td align="right"><img src="/images/HeaderGradient.gif" border="0" /></td>
                </tr>
                <tr>
                    <td colspan="2" align="right" class="whitedefault"><DIV id=thinOrangeBar><asp:label ID="lblName" runat="server" CssClass="whitedefault" />&nbsp;&nbsp;</DIV></td>
                </tr>
            </table>
        </td>
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
<table width="600" cellpadding="0" cellspacing="0" border="0">
    <tr>
        <td><img src="/images/table_topleft.gif" border="0" width="5" height="26"></td>
        <td nowrap background="/images/table_top.gif" class="greentableheader" width="100%"><asp:Label ID="lblTitle" runat="server" CssClass="greentableheader" /></td>
        <td><img src="/images/table_topright.gif" border="0" width="5" height="26"></td>
    </tr>
    <tr>
        <td background="/images/table_left.gif"><img src="/images/table_left.gif" width="5" height="10"></td>
        <td width="100%" bgcolor="#FFFFFF">
            <br />
            <asp:Panel ID="panRegistered" runat="server" Visible="false">
            <table width="100%" cellpadding="4" cellspacing="3" border="0">
                <tr>
                    <td align="center" class="header">
                        <img src='/images/bigCheck.gif' border='0' align='absmiddle' /> Your LAN ID [<%=strUser %>] has already been registered in ClearView.
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <a href="/index.aspx">Click here to login.</a>
                    </td>
                </tr>
            </table>
            </asp:Panel>
            <asp:Panel ID="panNotRegisteredNone" runat="server" Visible="false">
            <table width="100%" cellpadding="4" cellspacing="3" border="0">
                <tr>
                    <td colspan="2" align="center" class="header">
                        <img src='/images/bigAlert.gif' border='0' align='absmiddle' /> The active directory query for account [<%=strUser %>] did not return any results.
                    </td>
                </tr>
                <tr>
                    <td colspan="2">Please enter your FULL email address (Example: john.doe@pnc.com) and click <b>Search</b> to try again...</td>
                </tr>
                <tr>
                    <td nowrap>Email Address:</td>
                    <td width="100%"><asp:TextBox ID="txtEmail" runat="server" CssClass="default" Width="400" /></td>
                </tr>
                <tr>
                    <td nowrap>&nbsp;</td>
                    <td width="100%"><asp:Button ID="btnEmail" Text="Search" Width="100" CssClass="default" runat="server" OnClick="btnEmail_Click" /></td>
                </tr>
            </table>
            </asp:Panel>
            <asp:Panel ID="panNotRegistered" runat="server" Visible="false">
            <table width="100%" cellpadding="4" cellspacing="3" border="0">
                <tr>
                    <td colspan="2">Please complete the following form and click <b>Register</b> to finish your registration.</td>
                </tr>
                <tr>
                    <td nowrap>LAN ID:<font class="required">&nbsp;*</font></td>
                    <td width="100%"><asp:TextBox ID="txtXID" runat="server" Width="150" MaxLength="15" CssClass="default" Enabled="false" /></td>
                </tr>
                <tr>
                    <td nowrap>PNC ID:</td>
                    <td width="100%"><asp:TextBox ID="txtPNC" runat="server" Width="150" MaxLength="15" CssClass="default" /></td>
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
                <tr id="trManager" runat="server" visible="false">
                    <td nowrap>&nbsp;</td>
                    <td width="100%"><b>Registration Unavailable:</b> Your manager (<asp:Label ID="lblManager" runat="server" />) has not registered in ClearView. Either your manager needs to register before you, or submit an incident to the ClearView assignment group (please include your LAN ID and your manager's LAN ID in the ticket).</td>
                </tr>
                <tr>
                    <td nowrap>Phone Number:</td>
                    <td width="100%"><asp:TextBox ID="txtPhone" runat="server" Width="150" CssClass="default" MaxLength="20" /></td>
                </tr>
                <tr>
                    <td nowrap>&nbsp;</td>
                    <td width="100%"><asp:Button ID="btnRegister" Text="Register" Width="100" CssClass="default" runat="server" OnClick="btnRegister_Click" /></td>
                </tr>
                <tr>
                    <td colspan="2" align="right" class="required">* = Required Field</td>
                </tr>
            </table>
            </asp:Panel>
            <asp:Panel ID="panFinish" runat="server" Visible="false">
            <table width="100%" cellpadding="4" cellspacing="3" border="0">
                    <tr>
                        <td align="center" class="header">
                            <img src='/images/bigCheck.gif' border='0' align='absmiddle' /> Your LAN ID [<%=strUser %>] was successfully registered in ClearView.
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <a href="/index.aspx">Click here to login.</a>
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
<asp:HiddenField ID="hdnManager" runat="server" />
</asp:Content>