<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="software_component.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.software_component" %>
<script type="text/javascript">
</script>
<table width="100%" cellpadding="0" cellspacing="0" border="0" class="default">
    <tr>
        <td><img src="/images/table_topleft.gif" border="0" width="5" height="26"></td>
        <td nowrap background="/images/table_top.gif" class="greentableheader" width="100%">Software Component(s) Approval (# <asp:Label ID="lblID" runat="server" CssClass="greentableheader" />)</td>
        <td><img src="/images/table_topright.gif" border="0" width="5" height="26"></td>
    </tr>
    <tr>
        <td background="/images/table_left.gif"><img src="/images/table_left.gif" width="5" height="10"></td>
        <td width="100%" bgcolor="#FFFFFF">
            <br />
            <asp:Panel ID="panShow" runat="server" Visible="false">
            <table width="100%" cellpadding="4" cellspacing="3" border="0">
                <tr>
                    <td nowrap>Project Name:</td>
                    <td width="100%"><asp:Label ID="lblName" runat="server" CssClass="default" /></td>
                </tr>
                <tr>
                    <td nowrap>Project Number:</td>
                    <td width="100%"><asp:Label ID="lblNumber" runat="server" CssClass="default" /></td>
                </tr>
                <tr>
                    <td nowrap>Portfolio:</td>
                    <td width="100%"><asp:Label ID="lblPortfolio" runat="server" CssClass="default" /></td>
                </tr>
                <tr>
                    <td nowrap>Segment:</td>
                    <td width="100%"><asp:Label ID="lblSegment" runat="server" CssClass="default" /></td>
                </tr>
                <tr>
                    <td nowrap>Platform:</td>
                    <td width="100%"><asp:Label ID="lblPlatform" runat="server" CssClass="default" /></td>
                </tr>
                <tr>
                    <td nowrap>Nickname:</td>
                    <td width="100%"><asp:Label ID="lblNickname" runat="server" CssClass="default" /></td>
                </tr>
                <tr>
                    <td nowrap>Commitment Date:</td>
                    <td width="100%"><asp:Label ID="lblCommitment" runat="server" CssClass="default" /></td>
                </tr>
                <tr>
                    <td nowrap>Quantity:</td>
                    <td width="100%"><asp:Label ID="lblQuantity" runat="server" CssClass="default" /></td>
                </tr>
                <tr>
                    <td nowrap>Confidence:</td>
                    <td width="100%"><asp:Label ID="lblConfidence" runat="server" CssClass="default" /></td>
                </tr>
                <tr>
                    <td nowrap>Class:</td>
                    <td width="100%"><asp:Label ID="lblClass" runat="server" CssClass="default" /></td>
                </tr>
                <asp:Panel ID="panTest" runat="server" Visible="false">
                <tr>
                    <td nowrap>Test First?:</td>
                    <td width="100%"><asp:Label ID="lblTest" runat="server" CssClass="default" /></td>
                </tr>
                </asp:Panel>
                <tr>
                    <td nowrap>Environment:</td>
                    <td width="100%"><asp:Label ID="lblEnvironment" runat="server" CssClass="default" /></td>
                </tr>
                <tr>
                    <td nowrap>Location:</td>
                    <td width="100%"><asp:Label ID="lblLocation" runat="server" CssClass="default" /></td>
                </tr>
                <tr>
                    <td nowrap>IP Address:</td>
                    <td width="100%"><asp:Label ID="lblIP" runat="server" CssClass="default" /></td>
                </tr>
                <tr>
                    <td nowrap>Designed By:</td>
                    <td width="100%"><asp:Label ID="lblDesignedBy" runat="server" CssClass="default" /></td>
                </tr>
                <tr>
                    <td nowrap>Designed On:</td>
                    <td width="100%"><asp:Label ID="lblDesignedOn" runat="server" CssClass="default" /></td>
                </tr>
                <tr>
                    <td nowrap>Last Updated On:</td>
                    <td width="100%"><asp:Label ID="lblUpdated" runat="server" CssClass="default" /></td>
                </tr>
                <tr>
                    <td nowrap>Model:</td>
                    <td width="100%"><asp:Label ID="lblModel" runat="server" CssClass="default" /></td>
                </tr>
                <tr>
                    <td colspan="2">&nbsp;</td>
                </tr>
                <tr>
                    <td colspan="2" class="header">Software Components Awaiting Approval:</td>
                </tr>
                <tr>
                    <td colspan="2"><img src="/images/alert.gif" border="0" align="absmiddle" /> <b>NOTE:</b> Each line item represents a server.</td>
                </tr>
                <tr>
                    <td colspan="2">
                        <table width="100%" cellpadding="4" cellspacing="0" border="0" class="default" style="border:solid 1px #CCCCCC">
                            <tr bgcolor="EEEEEE">
                                <td></td>
                                <td></td>
                                <td></td>
                                <td><b>Component</b></td>
                                <td><b>OS</b></td>
                                <td><b>Server&nbsp;#</b></td>
                                <td><b>Detail&nbsp;#</b></td>
                            </tr>
                            <asp:Repeater ID="rptApproval" runat="server">
                                <ItemTemplate>
                                    <tr>
                                        <td><asp:RadioButton ID="radApprove" runat="server" CssClass="default" Text="Approve" GroupName="Approval" /></td>
                                        <td><asp:RadioButton ID="radDeny" runat="server" CssClass="default" Text="Deny" GroupName="Approval" /></td>
                                        <td>&nbsp;</td>
                                        <td width="70%"><%# DataBinder.Eval(Container.DataItem,"component") %></td>
                                        <td width="30%"><asp:Label ID="lblOS" runat="server" CssClass="default" /></td>
                                        <td><asp:Label ID="lblServer" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem,"serverid") %>' /></td>
                                        <td><asp:Label ID="lblDetail" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem,"detailid") %>' /></td>
                                    </tr>
                                    <tr id="trLicense" runat="server" style="display:none">
                                        <td colspan="3" align="right">License #:</td>
                                        <td colspan="4" width="100%"><asp:TextBox ID="txtLicense" runat="server" CssClass="default" Width="300" MaxLength="100" /></td>
                                    </tr>
                                    <tr id="trComments" runat="server" style="display:none">
                                        <td colspan="3" align="right">Comments:</td>
                                        <td colspan="4" width="100%"><asp:TextBox ID="txtComments" runat="server" CssClass="default" Width="500" TextMode="MultiLine" Rows="3" /></td>
                                    </tr>
                                </ItemTemplate>                                                             
                                <AlternatingItemTemplate>
                                    <tr bgcolor="#F6F6F6">
                                        <td><asp:RadioButton ID="radApprove" runat="server" CssClass="default" Text="Approve" GroupName="Approval" /></td>
                                        <td><asp:RadioButton ID="radDeny" runat="server" CssClass="default" Text="Deny" GroupName="Approval" /></td>
                                        <td>&nbsp;</td>
                                        <td width="70%"><%# DataBinder.Eval(Container.DataItem,"component") %></td>
                                        <td width="30%"><asp:Label ID="lblOS" runat="server" CssClass="default" /></td>
                                        <td><asp:Label ID="lblServer" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem,"serverid") %>' /></td>
                                        <td><asp:Label ID="lblDetail" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem,"detailid") %>' /></td>
                                    </tr>
                                    <tr id="trLicense" runat="server" style="display:none" bgcolor="#F6F6F6">
                                        <td colspan="3" align="right">License #:</td>
                                        <td colspan="4" width="100%"><asp:TextBox ID="txtLicense" runat="server" CssClass="default" Width="300" MaxLength="100" /></td>
                                    </tr>
                                    <tr id="trComments" runat="server" style="display:none" bgcolor="#F6F6F6">
                                        <td colspan="3" align="right">Comments:</td>
                                        <td colspan="4" width="100%"><asp:TextBox ID="txtComments" runat="server" CssClass="default" Width="500" TextMode="MultiLine" Rows="3" /></td>
                                    </tr>
                                </AlternatingItemTemplate>
                            </asp:Repeater>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">&nbsp;</td>
                </tr>
                <tr>
                    <td colspan="2"><hr size="1" noshade /></td>
                </tr>
                <tr>
                    <td colspan="2"><asp:Button ID="btnSave" runat="server" CssClass="default" Width="75" Text="Save" OnClick="btnSave_Click" /></td>
                </tr>
            </table>
            </asp:Panel>
            <asp:Panel ID="panFinish" runat="server" Visible="false">
                <table width="100%" cellpadding="2" cellspacing="2" border="0">
                    <tr>
                        <td><img src="/images/spacer.gif" border="0" width="15" height="1" /></td>
                        <td class="header"><img src="/images/bigCheck.gif" border="0" align="absmiddle" /> Record Updated</td>
                    </tr>
                    <tr><td colspan="2">&nbsp;</td></tr>
                    <tr>
                        <td><img src="/images/spacer.gif" border="0" width="15" height="1" /></td>
                        <td>Your information has been saved successfully.</td>
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
                                    <td align="right"><asp:Button ID="btnFinish" runat="server" CssClass="default" Width="75" Text="Finish" OnClick="btnFinish_Click" /></td>
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
<asp:Label ID="lblRequestor" runat="server" Visible="false" />