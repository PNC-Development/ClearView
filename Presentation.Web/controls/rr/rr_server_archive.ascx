<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="rr_server_archive.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.rr_server_archive" %>



<table width="100%" border="0" cellSpacing="2" cellPadding="4" class="default">
    <tr>
        <td colspan="2" class="header"><asp:Label ID="lblHeader" runat="server" CssClass="header" /></td>
    </tr>
    <tr>
        <td nowrap>Server Name:<font class="required">&nbsp;*</font></td>
        <td width="100%"><asp:TextBox ID="txtServer" runat="server" CssClass="default" width="200" MaxLength="50" /></td>
    </tr>
    <tr>
        <td nowrap>Server Model:<font class="required">&nbsp;*</font></td>
        <td width="100%"><asp:DropDownList ID="ddlModel" runat="server" CssClass="default" /></td>
    </tr>
    <tr>
        <td nowrap>Application Code:<font class="required">&nbsp;*</font></td>
        <td width="100%"><asp:TextBox ID="txtCode" runat="server" CssClass="default" width="50" MaxLength="3" /></td>
    </tr>
    <tr>
        <td nowrap>Class:<font class="required">&nbsp;*</font></td>
        <td width="100%"><asp:DropDownList ID="ddlClass" runat="server" CssClass="default" /></td>
    </tr>
    <tr>
        <td nowrap>Date of Completion:<font class="required">&nbsp;*</font></td>
        <td width="100%"><asp:TextBox ID="txtDate" runat="server" CssClass="default" Width="100" MaxLength="10" /> <asp:ImageButton ID="imgDate" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calendar.gif" /></td>
    </tr>
    <tr>
        <td nowrap valign="top">Special Instructions:</td>
        <td width="100%" valign="top"><asp:TextBox ID="txtStatement" runat="server" CssClass="default" TextMode="MultiLine" Rows="5" Width="250" /></td>
    </tr>
    <tr>
        <td colspan="2">
        <table cellpadding="2" cellspacing="1" border="0">
            <tr>
                <td valign="top"><asp:CheckBox ID="chkAgreement" runat="server" CssClass="default" /></td>
                <td valign="top"><b>Disclaimer:</b> By checking this box, you agree that all the appropriate parties associated with this server have been notified and have approved this server for archiving</td>
            </tr>
        </table>
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