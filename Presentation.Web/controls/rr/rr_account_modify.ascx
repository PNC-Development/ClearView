<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="rr_account_modify.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.rr_account_modify" %>

<script type="text/javascript">
</script>
<br />
<table width="100%" border="0" cellSpacing="2" cellPadding="4" class="default">
    <tr>
        <td colspan="2" class="header"><asp:Label ID="lblHeader" runat="server" CssClass="header" /></td>
    </tr>
    <tr>
        <td nowrap>User ID:<font class="required">&nbsp;*</font></td>
        <td width="100%"><asp:TextBox ID="txtID" runat="server" CssClass="default" width="150" MaxLength="10" /></td>
    </tr>
    <tr>
        <td nowrap>Domain:<font class="required">&nbsp;*</font></td>
        <td width="100%"><asp:DropDownList ID="ddlDomain" runat="server" CssClass="default" width="150" /></td>
    </tr>
    <tr>
        <td nowrap></td>
        <td width="100%"><asp:Button ID="btnContinue" runat="server" CssClass="default" Text="Load Properties" Width="150" OnClick="btnContinue_Click" /></td>
    </tr>
    <asp:Panel ID="panContinue" runat="server" Visible="false">
    <tr>
        <td colspan="2" width="100%">
            <br />
            <%=strMenuTab1 %>
            <table cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td><img src="/images/spacer.gif" border="0" width="150" height="1" /></td>
                    <td>
                        <table cellpadding="2" cellspacing="2" border="0">
                            <tr>
                                <td rowspan="2"><img src="/images/left-up.gif" border="0" align="absmiddle" /></td>
                                <td valign="bottom" class="greentableheader">Click Here to Modify Group Memberships</td>
                            </tr>
                            <tr>
                                <td valign="top" class="greenlink" nowrap>(Be sure to make your group modifications <b>BEFORE</b> you click the &quot;Next&nbsp;&nbsp;&gt;&gt;&quot; button)</td>
                            </tr>
                        </table>
                     </td>
                </tr>
            </table>
            <div id="divMenu1">
            <div style="display:none">
                <br />
                <table width="100%" cellpadding="4" cellspacing="3" border="0">
                    <tr>
                        <td nowrap>First Name:<font class="required">&nbsp;*</font></td>
                        <td width="100%"><asp:TextBox ID="txtFirst" runat="server" CssClass="default" Width="200" MaxLength="100" /></td>
                    </tr>
                    <tr>
                        <td nowrap>Last Name:<font class="required">&nbsp;*</font></td>
                        <td width="100%"><asp:TextBox ID="txtLast" runat="server" CssClass="default" Width="200" MaxLength="100" /></td>
                    </tr>
                    <tr>
                        <td nowrap>Location:</td>
                        <td width="100%"><asp:Label ID="lblLocation" runat="server" CssClass="default" /></td>
                    </tr>
                    <tr>
                        <td nowrap>Created:</td>
                        <td width="100%"><asp:Label ID="lblCreated" runat="server" CssClass="default" /></td>
                    </tr>
                    <tr>
                        <td nowrap>Modified:</td>
                        <td width="100%"><asp:Label ID="lblModified" runat="server" CssClass="default" /></td>
                    </tr>
                    <tr>
                        <td nowrap>Disabled:</td>
                        <td width="100%"><asp:Label ID="lblDisabled" runat="server" CssClass="default" /> <asp:CheckBox ID="chkEnable" runat="server" CssClass="default" Text="Enable this Account" Visible="false" /></td>
                    </tr>
                    <tr>
                        <td nowrap>Locked:</td>
                        <td width="100%"><asp:Label ID="lblLocked" runat="server" CssClass="default" /> <asp:CheckBox ID="chkUnlock" runat="server" CssClass="default" Text="Unlock this Account" Visible="false" /></td>
                    </tr>
                </table>
            </div>
            <div style="display:none">
                <br />
                <table width="100%" cellpadding="4" cellspacing="3" border="0">
                    <tr>
                        <td class="bold" colspan="2">New Membership</td>
                    </tr>
                    <tr>
                        <td nowrap>Group Name:</td>
                        <td width="100%">
                            <table cellpadding="0" cellspacing="0" border="0">
                                <tr>
                                    <td><asp:TextBox ID="txtGroup" runat="server" Width="300" CssClass="default" /></td>
                                    <td>&nbsp;<asp:Button ID="btnGroup" runat="server" CssClass="default" Width="100" Text="Check Names" /></td>
                                    <td>
                                        <div id="divGroupMultiple" runat="server" style="display:none">
                                            &nbsp;&nbsp;&nbsp;<img src="/images/alert.gif" border="0" align="absmiddle" /> There are more than <%=intMaximum.ToString() %> results. Please redefine your search...
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <div id="divGroup" runat="server" style="overflow:hidden; position:absolute; display:none; background-color:#FFFFFF; border:solid 1px #CCCCCC">
                                            <asp:ListBox ID="lstGroup" runat="server" CssClass="default" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="bold" colspan="2">Current Memberships</td>
                    </tr>
                    <tr>
                        <td colspan="2"><%=strMemberships %></td>
                    </tr>
                </table>
            </div>
            </div>
        </td>
    </tr>
    </asp:Panel>
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
<asp:HiddenField ID="hdnGroup" runat="server" />
<asp:HiddenField ID="hdnGroups" runat="server" />
