<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wm_remediation_AD.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.wm_remediation_AD" %>

<script type="text/javascript">
</script>
<asp:Panel ID="panWorkload" runat="server" Visible="false">
    <table width="100%" border="0" cellSpacing="0" cellPadding="0" class="default">
        <tr>
            <td>
                <table width="100%" border="0" cellSpacing="0" cellPadding="0">
                    <tr>
                        <td><asp:ImageButton ID="btnSave" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/tool_save.gif" OnClick="btnSave_Click" /></td>
                        <td><asp:ImageButton ID="btnPrint" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/tool_print.gif" /></td>
                        <td><asp:ImageButton ID="btnSLA" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/tool_sla.gif" /></td>
                        <td><asp:ImageButton ID="btnComplete" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/tool_complete.gif" OnClick="btnComplete_Click" /></td>
                        <td width="100%">
                            <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                <tr>
                                    <td><img src="/images/tool_left.gif" border="0" width="6" height="26"></td>
                                    <td nowrap background="/images/tool_back.gif" width="100%"><img src="/images/tool_back.gif" border="0" width="2" height="26"></td>
                                    <td><img src="/images/tool_right.gif" border="0" width="6" height="26"></td>
                                </tr>
                            </table>
                        </td>
                        <td><asp:ImageButton ID="btnClose" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/tool_close.gif" /></td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td class="default">
                <table width="100%" border="0" cellSpacing="2" cellPadding="2" class="default">
                    <tr>
                        <td valign="top">
                            <table border="0" cellpadding="3" cellspacing="2">
                                <tr>
                                    <td nowrap><b>Name:</b></td>
                                    <td><asp:Label ID="lblName" runat="server" CssClass="default" /></td>
                                </tr>
                                <tr>
                                    <td nowrap><b>Number:</b></td>
                                    <td><asp:Label ID="lblNumber" runat="server" CssClass="default" /></td>
                                </tr>
                                <tr>
                                    <td nowrap><b>Type:</b></td>
                                    <td><asp:Label ID="lblType" runat="server" CssClass="default" /></td>
                                </tr>
                                <tr>
                                    <td nowrap><b>Service:</b></td>
                                    <td><asp:Label ID="lblService" runat="server" CssClass="default" /></td>
                                </tr>
                                <tr>
                                    <td nowrap><b>Requested By:</b></td>
                                    <td><asp:Label ID="lblRequestedBy" runat="server" CssClass="default" /></td>
                                </tr>
                                <tr>
                                    <td nowrap><b>Requested On:</b></td>
                                    <td><asp:Label ID="lblRequestedOn" runat="server" CssClass="default" /></td>
                                </tr>
                                <tr>
                                    <td nowrap><b>Last Updated:</b></td>
                                    <td><asp:Label ID="lblUpdated" runat="server" CssClass="default" /></td>
                                </tr>
                                <tr>
                                    <td nowrap><b>Custom Task Name:</b></td>
                                    <td><asp:TextBox ID="txtCustom" runat="server" CssClass="default" Width="300" MaxLength="100" /></td>
                                </tr>
                            </table>
                        </td>
                        <td valign="top" align="right">
                            <table border="0" cellpadding="3" cellspacing="2">
                                <tr>
                                    <td colspan="2"><iframe src="/frame/did_you_know.aspx" frameborder="0" scrolling="no" width="300" height="135"></iframe></td>
                                </tr>
                                <tr>
                                    <td nowrap><b>Overall Status:</b></td>
                                    <td><asp:Label ID="lblStatus" runat="server" CssClass="default" /></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <table border="0" cellpadding="3" cellspacing="2">
                                <tr>
                                    <td><b>Original Request Details:</b>&nbsp;&nbsp;<asp:Label ID="lblDescription" runat="server" CssClass="default" /></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr height="16">
            <td><img src="/images/spacer.gif" border="0" height="16" width="1" /></td>
        </tr>
        <tr>
            <td>
                <table width="100%" cellpadding="2" cellspacing="1" border="0">
                    <tr>
                        <td>
                            <table width="100%" cellpadding="4" cellspacing="0" border="0" style="border:solid 1px #CCCCCC">
                                <tr bgcolor="#EEEEEE">
	                                <td>&nbsp;</td>
	                                <td><b>Name</b></td>
	                                <td><b>Reason</b></td>
	                            </tr>
                                <asp:Repeater id="rptView" runat="server">
	                                <ItemTemplate>
		                                <tr>
				                            <td valign="top"><asp:CheckBox ID="chkDelete" runat="server" CssClass="default" /></td>
				                            <td valign="top"><%# DataBinder.Eval(Container.DataItem, "name") %></td>
				                            <td valign="top"><%# DataBinder.Eval(Container.DataItem, "reason") %></td>
				                            <asp:Label ID="lblId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "adid") %>' />
				                            <asp:Label ID="lblEnvironment" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "environment") %>' />
				                            <asp:Label ID="lblPath" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "path") %>' />
		                                </tr>
	                                </ItemTemplate>
                                </asp:Repeater>
                                <tr>
                                    <td colspan="3">
                                        <asp:Label ID="lblNoAD" runat="server" CssClass="default" Visible="false" Text="<img src='/images/alert.gif' border='0' align='absmiddle'> There are no objects" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td align="right"><asp:Button ID="btnCheck" runat="server" CssClass="default" Width="100" Text="Check All" OnClick="btnCheck_Click" /></td>
                    </tr>
                </table>
                <p>&nbsp;</p>
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
                        <td align="right"><asp:Button ID="btnDenied" runat="server" CssClass="default" Width="75" Text="Close" /></td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Panel>
<asp:Label ID="lblResourceWorkflow" runat="server" Visible="false" />
<asp:Label ID="lblUsed" runat="server" Visible="false" />