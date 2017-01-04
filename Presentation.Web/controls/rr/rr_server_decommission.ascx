<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="rr_server_decommission.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.rr_server_decommission" %>


<script type="text/javascript">
</script>
<table width="100%" border="0" cellSpacing="2" cellPadding="4" class="default">
    <tr>
        <td nowrap>Server Name:<font class="required">&nbsp;*</font></td>
        <td width="100%"><asp:TextBox ID="txtName" runat="server" CssClass="default" width="150" MaxLength="20" /> <asp:Button ID="btnContinue" runat="server" CssClass="default" Text="Check Permission" Width="125" OnClick="btnContinue_Click" /></td>
    </tr>
    <asp:Panel ID="panShow" runat="server" Visible="false">
    <tr>
        <td nowrap>Class:</td>
        <td width="100%"><asp:Label ID="lblClass" runat="server" CssClass="default" /></td>
    </tr>
    <tr>
        <td nowrap>Environment:</td>
        <td width="100%"><asp:Label ID="lblEnvironment" runat="server" CssClass="default" /></td>
    </tr>
    <tr>
        <td nowrap>Location:</td>
        <td width="100%"><asp:Label ID="lblAddress" runat="server" CssClass="default" /></td>
    </tr>
    <asp:Panel ID="panChange" runat="server" Visible="false">
    <tr>
        <td nowrap>Change Control #:<font class="required">&nbsp;*</font></td>
        <td width="100%"><asp:TextBox ID="txtChange" runat="server" CssClass="default" Width="100" MaxLength="10" /></td>
    </tr>
    </asp:Panel>
    <tr>
        <td nowrap>Decommission Date:<font class="required">&nbsp;*</font></td>
        <td width="100%"><asp:TextBox ID="txtDate" runat="server" CssClass="default" Width="100" MaxLength="10" /> <asp:ImageButton ID="imgDate" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calendar.gif" /></td>
    </tr>
    <tr>
        <td nowrap valign="top">Reason for Decommission:<font class="required">&nbsp;*</font></td>
        <td width="100%"><asp:TextBox ID="txtReason" runat="server" CssClass="default" width="500" TextMode="MultiLine" Rows="8" /></td>
    </tr>
    </asp:Panel>
    <tr>
        <td colspan="2">
            <asp:Panel ID="panValid" runat="server" Visible="false">
                <table cellpadding="4" cellspacing="3" border="0" style="border:solid 1px #999999" bgcolor="#f9f9f9">
                    <tr>
                        <td valign="top"><img src="/images/bigCheck.gif" border="0" align="absmiddle" /></td>
                        <td>You have permission to decommission this server. Please complete the above form and click <b>Next</b> to continue...</td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="panInvalid" runat="server" Visible="false">
                <table cellpadding="4" cellspacing="3" border="0" style="border:solid 1px #999999" bgcolor="#f9f9f9">
                    <tr>
                        <td valign="top" rowspan="2"><img src="/images/bigError.gif" border="0" align="absmiddle" /></td>
                        <td>You do NOT have permission to decommission this server.</td>
                    </tr>
                    <tr>
                        <td>Please contact one of the following resources to have this server decommissioned...</td>
                    </tr>
                    <tr>
                        <td></td>
                        <td>
                            <table cellpadding="3" cellspacing="2" border="0">
                                <tr>
                                    <td>Departmental Manager:</td>
                                    <td><asp:Label ID="lblOwner" runat="server" CssClass="default" /></td>
                                </tr>
                                <tr>
                                    <td>Application Technical Lead:</td>
                                    <td><asp:Label ID="lblPrimary" runat="server" CssClass="default" /></td>
                                </tr>
                                <tr>
                                    <td>Administrative Contact:</td>
                                    <td><asp:Label ID="lblSecondary" runat="server" CssClass="default" /></td>
                                </tr>
                                <tr>
                                    <td>Design Initiated By:</td>
                                    <td><asp:Label ID="lblRequestor" runat="server" CssClass="default" /></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="panExist" runat="server" Visible="false">
                <table cellpadding="4" cellspacing="3" border="0" style="border:solid 1px #999999" bgcolor="#f9f9f9">
                    <tr>
                        <td valign="top"><img src="/images/bigError.gif" border="0" align="absmiddle" /></td>
                        <td>This server does not exist. Please try again...</td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="panStatus" runat="server" Visible="false">
                <table cellpadding="4" cellspacing="3" border="0" style="border:solid 1px #999999" bgcolor="#f9f9f9">
                    <tr>
                        <td valign="top"><img src="/images/bigAlert.gif" border="0" align="absmiddle" /></td>
                        <td>This server does exist but it is not in a commissioned state and therefore, cannot be decommissioned.</td>
                    </tr>
                    <tr>
                        <td valign="top"></td>
                        <td><b>NOTE:</b> Most likely, you are seeing this message because it was not auto-provisioned by ClearView. Please use the MANUAL SERVER DECOMMISSION service if this is the case.</td>
                    </tr>
                </table>
            </asp:Panel>
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
<input type="hidden" id="hdnParent" runat="server" />
<asp:Label ID="lblId" runat="server" Visible="false" />
