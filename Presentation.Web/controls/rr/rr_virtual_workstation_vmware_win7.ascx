<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="rr_virtual_workstation_vmware_win7.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.rr_virtual_workstation_vmware_win7" %>


<script type="text/javascript">
</script>
<table width="100%" border="0" cellSpacing="2" cellPadding="4" class="default">
    <tr>
        <td>
            <table width="100%" cellpadding="4" cellspacing="0" border="0" style="border:solid 1px #999999" bgcolor="#f9f9f9">
                <tr>
                    <td nowrap><img src="/images/bigAlert.gif" border="0" align="absmiddle" /></td>
                    <td width="100%"><b>NOTE:</b> You cannot attach a physical device to a virtual workstation. If your initiative requires special hardware, do not choose a virtual workstation.</td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td>
            <%=strMenuTab1 %>
            <div id="divMenu1">
                <br />
                <div style="display:none">
                    <table width="100%" border="0" cellSpacing="2" cellPadding="4" class="default">
                        <tr>
                            <td nowrap>Nickname:</td>
                            <td width="100%"><asp:TextBox ID="txtName" runat="server" CssClass="default" Width="200" MaxLength="50" /></td>
                        </tr>
                        <tr>
                            <td nowrap>Location:<font class="required">&nbsp;*</font></td>
                            <td width="100%">
                                <asp:DropDownList ID="ddlLocation" runat="server" CssClass="default">
                                    <asp:ListItem Value="1675" Text="Summit Data Center" />
                                    <asp:ListItem Value="715" Text="Cleveland Data Center" />
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td nowrap>Class:<font class="required">&nbsp;*</font></td>
                            <td width="100%"><asp:DropDownList ID="ddlClass" CssClass="default" runat="server" Width="300" /></td>
                        </tr>
                        <tr>
                            <td nowrap>Quantity:<font class="required">&nbsp;*</font></td>
                            <td width="100%"><asp:TextBox ID="txtQuantity" runat="server" CssClass="default" Width="80" MaxLength="10" /></td>
                        </tr>
                        <tr>
                            <td nowrap>Operating System:<font class="required">&nbsp;*</font></td>
                            <td width="100%"><asp:DropDownList ID="ddlOS" runat="server" CssClass="default" Width="300" AutoPostBack="true" OnSelectedIndexChanged="ddlOS_Change" /></td>
                        </tr>
                        <tr>
                            <td nowrap>Employee Type:<font class="required">&nbsp;*</font></td>
                            <td width="100%">
                                <asp:RadioButtonList ID="radEmployee" runat="Server" RepeatDirection="Horizontal">
                                    <asp:ListItem Text="Internal User (PNC Employee, Onshore Contractor)" Value="1" />
                                    <asp:ListItem Text="External User (Offshore User)" Value="0" />
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <td nowrap></td>
                            <td width="100%"><asp:CheckBox ID="chkDR" runat="server" Text="This workstation will be used as part of the DR testing" /></td>
                        </tr>
                        <tr>
                            <td nowrap>RAM:<font class="required">&nbsp;*</font></td>
                            <td width="100%">
                                <asp:DropDownList ID="ddlRam" runat="server" CssClass="default" Width="300" Enabled="false" AutoPostBack="true" OnSelectedIndexChanged="ddlRam_Change" >
                                    <asp:ListItem Value="-- Select an Operating System --" />
                                </asp:DropDownList> GB
                            </td>
                        </tr>
                        <tr id="trRam" runat="server" visible="false">
                            <td nowrap></td>
                            <td width="100%">
                                <table width="100%" cellpadding="0" cellspacing="5" border="0">
                                    <tr>
                                        <td rowspan="2"><img src="/images/bigAlert.gif" border="0" align="absmiddle" /></td>
                                        <td class="header" width="100%" valign="bottom">Special Note regarding this RAM</td>
                                    </tr>
                                    <tr>
                                        <td width="100%" valign="top"><asp:Label ID="lblRam" runat="server" /></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td nowrap>CPUs:<font class="required">&nbsp;*</font></td>
                            <td width="100%">
                                <asp:DropDownList ID="ddlCPU" runat="server" CssClass="default" Width="300" Enabled="false" AutoPostBack="true" OnSelectedIndexChanged="ddlCPU_Change" >
                                    <asp:ListItem Value="-- Select an Operating System --" />
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr id="trCPU" runat="server" visible="false">
                            <td nowrap></td>
                            <td width="100%">
                                <table width="100%" cellpadding="0" cellspacing="5" border="0">
                                    <tr>
                                        <td rowspan="2"><img src="/images/bigAlert.gif" border="0" align="absmiddle" /></td>
                                        <td class="header" width="100%" valign="bottom">Special Note regarding this CPU</td>
                                    </tr>
                                    <tr>
                                        <td width="100%" valign="top"><asp:Label ID="lblCPU" runat="server" /></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td nowrap>Hard Drive:<font class="required">&nbsp;*</font></td>
                            <td width="100%">
                                <asp:DropDownList ID="ddlHardDrive" runat="server" CssClass="default" Width="300" Enabled="false" AutoPostBack="true" OnSelectedIndexChanged="ddlHardDrive_Change" >
                                    <asp:ListItem Value="-- Select an Operating System --" />
                                </asp:DropDownList> GB
                            </td>
                        </tr>
                        <tr id="trHardDrive" runat="server" visible="false">
                            <td nowrap></td>
                            <td width="100%">
                                <table width="100%" cellpadding="0" cellspacing="5" border="0">
                                    <tr>
                                        <td rowspan="2"><img src="/images/bigAlert.gif" border="0" align="absmiddle" /></td>
                                        <td class="header" width="100%" valign="bottom">Special Note regarding this Hard Drive</td>
                                    </tr>
                                    <tr>
                                        <td width="100%" valign="top"><asp:Label ID="lblHardDrive" runat="server" /></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td nowrap>Workstation Manager:<font class="required">&nbsp;*</font></td>
                            <td width="100%">
                                <table cellpadding="0" cellspacing="0" border="0">
                                    <tr>
                                        <td><asp:TextBox ID="txtManager" runat="server" Width="300" CssClass="default" />&nbsp;&nbsp;<asp:LinkButton ID="btnManager" runat="server" CssClass="default" Text="Add a New User" /></td>
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
                            <td nowrap></td>
                            <td width="100%" class="footer">Enter a first name, last name, or LAN ID and select from the list (a list will appear after you start typing)</td>
                        </tr>
                         
                        <tr>
                            <td nowrap>Billable Cost Center:<font class="required">&nbsp;*</font></td>
                            <td width="100%">
                                <table cellpadding="0" cellspacing="0" border="0">
                                    <tr>
                                        <td><asp:TextBox ID="txtCostCenter" runat="server" Width="200" CssClass="default" /></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div id="divCostCenter" runat="server" style="overflow:hidden; position:absolute; display:none; background-color:#FFFFFF; border:solid 1px #CCCCCC">
                                                <asp:ListBox ID="lstCostCenter" runat="server" CssClass="default" />
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td nowrap></td>
                            <td width="100%" class="footer">Start typing and a list will be presented (after 6 characters)</td>
                        </tr>
                        <tr id="trUpdate" runat="server" visible="false">
                            <td nowrap></td>
                            <td width="100%"><asp:Button ID="btnUpdate" runat="server" Text="Update" Width="75" OnClick="btnUpdate_Click" /></td>
                        </tr>
                    </table>
                </div>
                <div style="display:none">
                    <asp:Panel ID="panAccounts" runat="server" Visible="false">
                        <table width="100%" border="0" cellpadding="2" cellspacing="3">
                            <tr>
                                <td colspan="2">
                                    <table width="100%" cellpadding="4" cellspacing="0" border="0" style="border:solid 1px #999999" bgcolor="#f9f9f9">
                                        <tr>
                                            <td nowrap><img src="/images/bigAlert.gif" border="0" align="absmiddle" /></td>
                                            <td width="100%"><b>NOTE:</b> Only one person can remotely access this VMware workstation at a time.</td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <asp:Panel ID="panProduction" runat="server" Visible="false">
                            <tr>
                                <td colspan="2">
                                    <table width="100%" cellpadding="4" cellspacing="0" border="0" style="border:solid 1px #999999" bgcolor="#f9f9f9">
                                        <tr>
                                            <td nowrap><img src="/images/bigAlert.gif" border="0" align="absmiddle" /></td>
                                            <td width="100%"><b>NOTE:</b> For accounts requested after this build, you will need to submit a LAN Access Form.</td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            </asp:Panel>
                            <tr id="trNew" runat="server" visible="false">
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
                                                        <td nowrap><asp:LinkButton ID="btnManager2" runat="server" Text="User Not Appearing in the List? Click Here." /></td>
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
                                                <table width="100%" border="0" cellspacing="2" cellpadding="2">
                                                    <tr>
                                                        <td nowrap>Remote Desktop:</td>
                                                        <td width="100%"><asp:CheckBox ID="chkRemote" runat="server" CssClass="default" Checked="true" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2" class="footer">User can remote to this workstation with limited rights</td>
                                                    </tr>
                                                    <asp:Panel ID="panAdmin" runat="server" Visible="false">
                                                    <tr>
                                                        <td colspan="2">&nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td nowrap>Administrator:</td>
                                                        <td width="100%"><asp:CheckBox ID="chkAdmin" runat="server" CssClass="default" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2" class="footer">User has full access to this workstation</td>
                                                    </tr>
                                                    </asp:Panel>
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
                            </tr>
                            <tr id="trAccountUpdate" runat="server" visible="false">
                                <td colspan="2" height="100%" valign="top">
                                    <table width="100%" height="100%" cellpadding="0" cellspacing="0" border="0">
                                        <tr height="1">
                                            <td><img src="/images/table_topleft.gif" border="0" width="5" height="26"></td>
                                            <td nowrap background="/images/table_top.gif" class="greentableheader" width="100%">Account Update</td>
                                            <td><img src="/images/table_topright.gif" border="0" width="5" height="26"></td>
                                        </tr>
                                        <tr>
                                            <td background="/images/table_left.gif"><img src="/images/table_left.gif" width="5" height="10"></td>
                                            <td width="100%" bgcolor="#FFFFFF" valign="top">
                                                <table width="100%" border="0" cellspacing="4" cellpadding="4">
                                                    <tr>
                                                        <td colspan="2">The account you are attmpting to add does not have a valid CORPDMN (X-ID).</td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">Please enter the X-ID for <asp:Label ID="lblXID" runat="server" CssClass="bold" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td nowrap>X-ID:</td>
                                                        <td width="100%"><asp:TextBox ID="txtXID" runat="server" CssClass="default" Width="100" MaxLength="10" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2"><asp:Label ID="lblError" runat="server" CssClass="reddefault" /></td>
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
                            </tr>
                            <tr>
                                <td colspan="2" align="right">
                                    <asp:Button ID="btnAddAccount" runat="server" CssClass="default" Width="125" OnClick="btnAddAccount_Click" Text="Add Account" />
                                    <asp:Button ID="btnCancelAccount" runat="server" CssClass="default" Width="125" OnClick="btnCancelAccount_Click" Text="Cancel" Visible="false" />
                                </td>
                            </tr>
                            <tr><td colspan="2">&nbsp;</td></tr>
                            <tr>
                                <td colspan="2">
                                    <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                        <tr>
                                            <td><img src="/images/table_topleft.gif" border="0" width="5" height="26"></td>
                                            <td nowrap background="/images/table_top.gif" class="greentableheader" width="100%">Current Configuration</td>
                                            <td><img src="/images/table_topright.gif" border="0" width="5" height="26"></td>
                                        </tr>
                                        <tr>
                                            <td background="/images/table_left.gif"><img src="/images/table_left.gif" width="5" height="10"></td>
                                            <td width="100%" bgcolor="#FFFFFF">
                                    <div style="height:100%; width:100%; overflow:auto">
                                    <table width="100%" cellpadding="4" cellspacing="0" border="0" align="center">
                                        <tr>
                                            <td><b><u>Username:</u></b></td>
                                            <td align="center"><b><u>Remote Desktop:</u></b></td>
                                            <td align="center"><b><u>Administrator:</u></b></td>
                                            <td></td>
                                        </tr>
                                        <asp:repeater ID="rptAccounts" runat="server">
                                            <ItemTemplate>
                                                <tr>
                                                    <td valign="top"><%# oUser.GetFullName(Int32.Parse(DataBinder.Eval(Container.DataItem, "userid").ToString())) %> (<%# oUser.GetName(Int32.Parse(DataBinder.Eval(Container.DataItem, "userid").ToString()))%>)</td>
                                                    <td valign="top" align="center"><%# "<img src=\"/images/" + (DataBinder.Eval(Container.DataItem, "remote").ToString() == "1" ? "check" : "cancel") + ".gif\" border=\"0\" align=\"absmiddle\"/>"%></td>
                                                    <td valign="top" align="center"><%# "<img src=\"/images/" + (DataBinder.Eval(Container.DataItem, "admin").ToString() == "1" ? "check" : "cancel") + ".gif\" border=\"0\" align=\"absmiddle\"/>"%></td>
                                                    <td valign="top" align="right"><asp:LinkButton ID="btnDeleteAccount" runat="server" Text="Delete" OnClick="btnDeleteAccount_Click" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' /></td>
                                                </tr>
                                            </ItemTemplate>
                                            <AlternatingItemTemplate>
                                                <tr bgcolor="F6F6F6">
                                                    <td valign="top"><%# oUser.GetFullName(Int32.Parse(DataBinder.Eval(Container.DataItem, "userid").ToString())) %> (<%# oUser.GetName(Int32.Parse(DataBinder.Eval(Container.DataItem, "userid").ToString()))%>)</td>
                                                    <td valign="top" align="center"><%# "<img src=\"/images/" + (DataBinder.Eval(Container.DataItem, "remote").ToString() == "1" ? "check" : "cancel") + ".gif\" border=\"0\" align=\"absmiddle\"/>" %></td>
                                                    <td valign="top" align="center"><%# "<img src=\"/images/" + (DataBinder.Eval(Container.DataItem, "admin").ToString() == "1" ? "check" : "cancel") + ".gif\" border=\"0\" align=\"absmiddle\"/>"%></td>
                                                    <td valign="top" align="right"><asp:LinkButton ID="btnDeleteAccount" runat="server" Text="Delete" OnClick="btnDeleteAccount_Click" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' /></td>
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
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="panAccountsNo" runat="server" Visible="false">
                        <table width="100%" cellpadding="0" cellspacing="5" border="0">
                            <tr>
                                <td rowspan="2"><img src="/images/bigAlert.gif" border="0" align="middle" /></td>
                                <td class="header" width="100%" valign="bottom">Your workstation has not been Configured!</td>
                            </tr>
                            <tr>
                                <td width="100%" valign="top">Please go to the other tab and answer the questions before you attempt to add accounts.</td>
                            </tr>
                        </table>
                    </asp:Panel>
                </div>
            </div>
        </td>
    </tr>
    <tr>
        <td colspan="2"><hr size="1" noshade /></td>
    </tr>
    <tr>
        <td colspan="2">
            <table width="100%" cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td>
                        <table cellpadding="4" cellspacing="4" border="0">
                            <tr>
                                <td><asp:Button ID="btnBack" runat="server" OnClick="btnBack_Click" CssClass="default" Text="<<  Back" Width="100" /></td>
                                <td>
                                    <asp:Panel ID="panDeliverable" runat="server" Visible="false">
                                        <asp:Button ID="btnDeliverable" runat="server" CssClass="default" Text="Service Deliverables" Width="150" />
                                    </asp:Panel>
                                </td>
                                <td><asp:Button ID="btnNext" runat="server" OnClick="btnNext_Click" CssClass="default" Text="Next  >>" Width="100" /></td>
                            </tr>
                        </table>
                    </td>
                    <td align="right">
                        <table cellpadding="4" cellspacing="4" border="0">
                            <tr>
                                <td><asp:Button ID="btnCancel1" runat="server" OnClick="btnCancel_Click" CssClass="default" Text="Cancel Request" Width="125" /></td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
<asp:Label ID="lblItem" runat="server" Visible="false" />
<asp:Label ID="lblNumber" runat="server" Visible="false" />
<asp:Label ID="lblService" runat="server" Visible="false" />
<asp:HiddenField ID="hdnManager" runat="server" />
<asp:HiddenField ID="hdnCostCenter" runat="server" />
<asp:HiddenField ID="hdnUser" runat="server" />
