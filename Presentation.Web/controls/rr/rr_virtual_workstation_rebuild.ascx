<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="rr_virtual_workstation_rebuild.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.rr_virtual_workstation_rebuild" %>


<script type="text/javascript">
</script>
<table width="100%" border="0" cellSpacing="2" cellPadding="4" class="default">
    <tr>
        <td nowrap>Workstation Name:<font class="required">&nbsp;*</font></td>
        <td width="100%"><asp:TextBox ID="txtName" runat="server" CssClass="default" width="125" MaxLength="15" /> <asp:Button ID="btnContinue" runat="server" CssClass="default" Text="Find" Width="75" OnClick="btnContinue_Click" /></td>
    </tr>
    <tr>
        <td colspan="2">
            <asp:Panel ID="panFound" runat="server" Visible="false">
                <table cellpadding="0" cellspacing="5" border="0" style="border:solid 1px #999999" bgcolor="#f9f9f9">
                    <tr>
                        <td rowspan="2" valign="top"><img src="/images/ico_check.gif" border="0" align="absmiddle" /></td>
                        <td class="header" valign="bottom">Workstation Found!</td>
                    </tr>
                    <tr>
                        <td valign="top">Please complete the following information and click <b>Next</b> to continue...</td>
                    </tr>
                </table>
                <br />
                <table cellpadding="4" cellspacing="3" border="0">
                    <tr>
                        <td nowrap>Name:</td>
                        <td width="100%"><asp:Label ID="lblName" runat="server" CssClass="default" /></td>
                    </tr>
                    <tr>
                        <td nowrap>Power Off Date:<font class="required">&nbsp;*</font></td>
                        <td width="100%"><asp:TextBox ID="txtDate" runat="server" CssClass="default" Width="100" MaxLength="10" /> <asp:ImageButton ID="imgDate" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calendar.gif" /></td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="panWorkstation" runat="server" Visible="false">
                <table cellpadding="4" cellspacing="3" border="0" style="border:solid 1px #999999" bgcolor="#f9f9f9">
                    <tr>
                        <td valign="top"><img src="/images/bigError.gif" border="0" align="absmiddle" /></td>
                        <td>This workstation does not exist. Please try again...</td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="panAlready" runat="server" Visible="false">
                <table cellpadding="0" cellspacing="5" border="0" style="border:solid 1px #999999" bgcolor="#f9f9f9">
                    <tr>
                        <td rowspan="2" valign="top"><img src="/images/hugeAlert.gif" border="0" align="absmiddle" /></td>
                        <td class="header" valign="bottom">Already Submitted</td>
                    </tr>
                    <tr>
                        <td valign="top">A rebuild request has already been submitted for this device.</td>
                    </tr>
                    <tr>
                        <td></td>
                        <td valign="top">
                            <table cellpadding="3" cellspacing="2" border="0">
                                <tr>
                                    <td>Submitted By:</td>
                                    <td><asp:Label ID="lblAlreadyBy" runat="server" CssClass="default" /></td>
                                </tr>
                                <tr>
                                    <td>Submitted On:</td>
                                    <td><asp:Label ID="lblAlreadyOn" runat="server" CssClass="default" /></td>
                                </tr>
                                <tr>
                                    <td>Power Off Date:</td>
                                    <td><asp:Label ID="lblAlreadyPower" runat="server" CssClass="default" /></td>
                                </tr>
                                <tr>
                                    <td>Status:</td>
                                    <td><asp:Label ID="lblAlreadyStatus" runat="server" CssClass="default" /></td>
                                </tr>
                            </table>
                        </td>
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
<asp:Label ID="lblService" runat="server" Visible="false" />
<asp:Label ID="lblNumber" runat="server" Visible="false" />
<input type="hidden" id="hdnParent" runat="server" />
<asp:Label ID="lblId" runat="server" Visible="false" />
