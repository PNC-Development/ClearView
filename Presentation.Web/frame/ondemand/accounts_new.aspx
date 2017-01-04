<%@ Page Language="C#" MasterPageFile="~/window.Master" AutoEventWireup="true" CodeBehind="accounts_new.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.accounts_new" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" runat="server">
<script type="text/javascript">
    function CheckAdmin(oCheck, oDDL1) {
        oDDL1 = document.getElementById(oDDL1);
        if (oCheck.checked == true) {
        }
        else {
        }
    }
    function ShowAccountDetail(oDiv) {
        oDiv = document.getElementById(oDiv);
        if (oDiv.style.display == 'inline')
            oDiv.style.display = 'none';
        else
            oDiv.style.display = 'inline';
    }
    function SwapChecks(oChecked, oOther) {
        oOther = document.getElementById(oOther);
        if (oChecked.checked == true && oOther.checked == true)
        {
            var boolConfirm = confirm('An account can only be configured with one (1) of the following permissions...\n\n - Developer\n - Promoter\n\nDo you want to continue with this change?');
            if (boolConfirm == true)
                oOther.checked = false;
            else
                oChecked.checked = false;
        }
    }
</script>
<asp:Panel ID="panPermit" runat="server" Visible="false">
<table width="100%" cellpadding="0" cellspacing="5" border="0">
    <tr>
        <td rowspan="2"><img src="/images/account_request.gif" border="0" align="absmiddle" /></td>
        <td class="header" width="100%" valign="bottom">ClearView Account Configuration</td>
    </tr>
    <tr>
        <td width="100%" valign="top">Automate the configuration of your user accounts by completing the following form and clicking <b>Submit Request</b></td>
    </tr>
</table>
<table width="100%" border="0" cellpadding="2" cellspacing="3">
    <tr>
        <td width="50%" height="100%" valign="top">
            <table width="100%" height="100%" cellpadding="0" cellspacing="0" border="0">
                <tr height="1">
                    <td><img src="/images/table_topleft.gif" border="0" width="5" height="26"></td>
                    <td nowrap background="/images/table_top.gif" class="greentableheader" width="100%">Account Properties</td>
                    <td><img src="/images/table_topright.gif" border="0" width="5" height="26"></td>
                </tr>
                <tr>
                    <td background="/images/table_left.gif"><img src="/images/table_left.gif" width="5" height="10"></td>
                    <td width="100%" bgcolor="#FFFFFF" valign="top">
                        <table width="100%" border="0" cellspacing="4" cellpadding="4">
                            <tr>
                                <td nowrap>Domain:</td>
                                <td width="100%"><asp:Label ID="lblDomain" runat="server" CssClass="default" /></td>
                            </tr>
                            <tr>
                                <td nowrap>User:</td>
                                <td width="100%">
                                    <table cellpadding="0" cellspacing="0" border="0">
                                        <tr>
                                            <td><asp:TextBox ID="txtUser" runat="server" Width="300" CssClass="default" /></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div id="divAJAX" runat="server" style="overflow:hidden; position:absolute; display:none; background-color:#FFFFFF; border:solid 1px #CCCCCC">
                                                    <asp:ListBox ID="lstAJAX" runat="server" CssClass="default" />
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td nowrap class="footer">&nbsp;</td>
                                <td class="footer">(Please enter a valid LAN ID, First Name, or Last Name)</td>
                            </tr>
                            <tr>
                                <td></td>
                                <td nowrap><asp:LinkButton ID="btnManager" runat="server" Text="User Not Appearing in the List? Click Here." /></td>
                            </tr>
                        </table>
                    </td>
                    <td background="/images/table_right.gif"><img src="/images/table_right.gif" width="5" height="10"></td>
                </tr>
                <tr height="1">
                    <td><img src="/images/table_bottomLeft.gif" border="0" width="5" height="9"></td>
                    <td width="100%" background="/images/table_bottom.gif"></td>
                    <td><img src="/images/table_bottomRight.gif" border="0" width="5" height="9"></td>
                </tr>
            </table>
        </td>
        <td width="50%" height="100%" valign="top">
            <table width="100%" height="100%" cellpadding="0" cellspacing="0" border="0">
                <tr height="1">
                    <td><img src="/images/table_topleft.gif" border="0" width="5" height="26"></td>
                    <td nowrap background="/images/table_top.gif" class="greentableheader" width="100%">Account Permissions</td>
                    <td><img src="/images/table_topright.gif" border="0" width="5" height="26"></td>
                </tr>
                <tr>
                    <td background="/images/table_left.gif"><img src="/images/table_left.gif" width="5" height="10"></td>
                    <td width="100%" bgcolor="#FFFFFF" valign="top">
                        <asp:Panel ID="panNCB" runat="server" Visible="false">
                            <table width="100%" border="0" cellspacing="2" cellpadding="2">
                                <tr>
                                    <td nowrap>Administrator:</td>
                                    <td width="100%"><asp:CheckBox ID="chkAdmin" runat="server" CssClass="default" /></td>
                                </tr>
                                <tr>
                                    <td nowrap>SYS_VOL (C:)</td>
                                    <td width="100%">
                                        <asp:RadioButtonList ID="radSysVol" runat="server" CssClass="default" RepeatDirection="Horizontal">
                                            <asp:ListItem Text="None" Value="N" Selected="True" />
                                            <asp:ListItem Text="Read" Value="R" />
                                            <asp:ListItem Text="Write" Value="W" />
                                            <asp:ListItem Text="Full" Value="F" />
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td nowrap>UTL_VOL (E:)</td>
                                    <td width="100%">
                                        <asp:RadioButtonList ID="radUtlVol" runat="server" CssClass="default" RepeatDirection="Horizontal">
                                            <asp:ListItem Text="None" Value="N" Selected="True" />
                                            <asp:ListItem Text="Read" Value="R" />
                                            <asp:ListItem Text="Write" Value="W" />
                                            <asp:ListItem Text="Full" Value="F" />
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td nowrap>APP_VOL (F:)</td>
                                    <td width="100%">
                                        <asp:RadioButtonList ID="radAppVol" runat="server" CssClass="default" RepeatDirection="Horizontal">
                                            <asp:ListItem Text="None" Value="N" Selected="True" />
                                            <asp:ListItem Text="Read" Value="R" />
                                            <asp:ListItem Text="Write" Value="W" />
                                            <asp:ListItem Text="Full" Value="F" />
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:Panel ID="panPNC" runat="server" Visible="false">
                            <table width="100%" border="0" cellspacing="2" cellpadding="2">
                                <tr style="display:none">
                                    <td nowrap>Remote Desktop:</td>
                                    <td width="50%"><asp:CheckBox ID="chkRemoteDesktop" runat="server" CssClass="default" Text="User can access systems remotely" /></td>
                                </tr>
                                <tr>
                                    <td nowrap>Developer:</td>
                                    <td width="50%"><asp:CheckBox ID="chkDeveloper" runat="server" CssClass="default" /></td>
                                    <td nowrap>Remote Desktop:</td>
                                    <td width="50%"><asp:CheckBox ID="chkDeveloperR" runat="server" CssClass="default" /></td>
                                </tr>
                                <tr>
                                    <td colspan="4" class="footer" style="border-bottom:dashed 1px #CCCCCC">Require access for application code development and testing</td>
                                </tr>
                                <tr>
                                    <td nowrap>Promoter:</td>
                                    <td width="50%"><asp:CheckBox ID="chkPromoter" runat="server" CssClass="default" /></td>
                                    <td nowrap>Remote Desktop:</td>
                                    <td width="50%"><asp:CheckBox ID="chkPromoterR" runat="server" CssClass="default" /></td>
                                </tr>
                                <tr>
                                    <td colspan="4" class="footer" style="border-bottom:dashed 1px #CCCCCC">Responsible for promoting application code into production</td>
                                </tr>
                                <tr>
                                    <td nowrap>AppSupport:</td>
                                    <td width="50%"><asp:CheckBox ID="chkAppSupport" runat="server" CssClass="default" /></td>
                                    <td nowrap>Remote Desktop:</td>
                                    <td width="50%"><asp:CheckBox ID="chkAppSupportR" runat="server" CssClass="default" /></td>
                                </tr>
                                <tr>
                                    <td colspan="4" class="footer" style="border-bottom:dashed 1px #CCCCCC">Require access to support a business application (typically MIS but not limited to MIS)</td>
                                </tr>
                                <tr>
                                    <td nowrap>AppUsers:</td>
                                    <td width="50%"><asp:CheckBox ID="chkAppUsers" runat="server" CssClass="default" /></td>
                                    <td nowrap>Remote Desktop:</td>
                                    <td width="50%"><asp:CheckBox ID="chkAppUsersR" runat="server" CssClass="default" /></td>
                                </tr>
                                <tr>
                                    <td colspan="4" class="footer">Require application user access</td>
                                </tr>
                                <tr style="display:none">
                                    <td colspan="4" class="footer" style="border-bottom:dashed 1px #CCCCCC">Require application user access</td>
                                </tr>
                                <tr style="display:none">
                                    <td nowrap>AuthProbMgmt:</td>
                                    <td width="50%"><asp:CheckBox ID="chkAuthProbMgmt" runat="server" CssClass="default" /></td>
                                    <td nowrap>Remote Desktop:</td>
                                    <td width="50%"><asp:CheckBox ID="chkAuthProbMgmtR" runat="server" CssClass="default" /></td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                    <td background="/images/table_right.gif"><img src="/images/table_right.gif" width="5" height="10"></td>
                </tr>
                <tr height="1">
                    <td><img src="/images/table_bottomLeft.gif" border="0" width="5" height="9"></td>
                    <td width="100%" background="/images/table_bottom.gif"></td>
                    <td><img src="/images/table_bottomRight.gif" border="0" width="5" height="9"></td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td colspan="2" align="right"><asp:Button ID="btnAdd" runat="server" CssClass="default" Width="125" OnClick="btnAdd_Click" Text="Add Account" /></td>
    </tr>
    <tr><td colspan="2">&nbsp;</td></tr>
    <tr>
        <td colspan="2">
            <table width="100%" cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td><img src="/images/table_topleft.gif" border="0" width="5" height="26"></td>
                    <td nowrap background="/images/table_top.gif" class="greentableheader" width="100%">Current Account Requests</td>
                    <td><img src="/images/table_topright.gif" border="0" width="5" height="26"></td>
                </tr>
                <tr>
                    <td background="/images/table_left.gif"><img src="/images/table_left.gif" width="5" height="10"></td>
                    <td width="100%" bgcolor="#FFFFFF">
            <div style="height:100%; width:100%; overflow:auto">
            <table width="100%" cellpadding="4" cellspacing="0" border="0" align="center">
                <tr>
                    <td><b><u>Username:</u></b></td>
                    <td><b><u>Permissions:</u></b></td>
                    <td></td>
                </tr>
                <asp:repeater ID="rptAccounts" runat="server">
                    <ItemTemplate>
                        <tr>
                            <asp:Label ID="lblLocal" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "localgroups") %>' />
                            <asp:Label ID="lblDomain" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "domaingroups") %>' />
                            <asp:Label ID="lblAdmin" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "admin") %>' />
                            <td valign="top"><%# oUser.GetFullName(DataBinder.Eval(Container.DataItem, "xid").ToString()) %> (<%# DataBinder.Eval(Container.DataItem, "xid") %>)</td>
                            <td valign="top"><asp:Label ID="lblPermissions" runat="server" CssClass="default" /></td>
                            <td valign="top" align="right"><asp:LinkButton ID="btnDelete" runat="server" Text="Delete" OnClick="btnDelete_Click" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' /></td>
                        </tr>
                    </ItemTemplate>
                    <AlternatingItemTemplate>
                        <tr bgcolor="F6F6F6">
                            <asp:Label ID="lblLocal" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "localgroups") %>' />
                            <asp:Label ID="lblDomain" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "domaingroups") %>' />
                            <asp:Label ID="lblAdmin" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "admin") %>' />
                            <td valign="top"><%# oUser.GetFullName(DataBinder.Eval(Container.DataItem, "xid").ToString()) %> (<%# DataBinder.Eval(Container.DataItem, "xid") %>)</td>
                            <td valign="top"><asp:Label ID="lblPermissions" runat="server" CssClass="default" /></td>
                            <td valign="top" align="right"><asp:LinkButton ID="btnDelete" runat="server" Text="Delete" OnClick="btnDelete_Click" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' /></td>
                        </tr>
                    </AlternatingItemTemplate>
                </asp:repeater>
                <tr>
                    <td colspan="2">
                        <asp:Label ID="lblNone" runat="server" CssClass="default" Visible="false" Text="<img src='/images/spacer.gif' border='0' height='1' width='25' /><img src='/images/alert.gif' border='0' align='absmiddle'> You have not added any accounts to this request" />
                    </td>
                </tr>
            </table>
            </div>
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
    <tr>
        <td><asp:CheckBox ID="chkNotify" runat="server" CssClass="default" Text="Email me the results of this account request" Checked="true" /></td>
        <td align="right"><asp:Button ID="btnSkip" runat="server" CssClass="default" Width="125" OnClick="btnSkip_Click" Text="Skip Accounts" /> <asp:Button ID="btnSubmit" runat="server" CssClass="default" Width="125" OnClick="btnSubmit_Click" Text="Submit Accounts" /></td>
    </tr>
</table>
</asp:Panel>
<asp:Panel ID="panDenied" runat="server" Visible="false">
    <table width="100%" cellpadding="2" cellspacing="2" border="0">
        <tr>
            <td class="header"><img src="/images/bigX.gif" border="0" align="absmiddle" /> Access Denied</td>
        </tr>
        <tr><td>&nbsp;</td></tr>
        <tr>
            <td>You do not have rights to view this item.</td>
        </tr>
        <tr><td>&nbsp;</td></tr>
        <tr>
            <td><hr size="1" noshade /></td>
        </tr>
        <tr>
            <td align="right">
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
<asp:HiddenField ID="hdnUser" runat="server" />
<asp:Label ID="lblBuild" runat="server" Visible="false" />
<asp:Label ID="lblStep" runat="server" Visible="false" />
</asp:Content>
