<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="rr_account_rename.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.rr_account_rename" %>

<script type="text/javascript">
</script>
<table width="100%" border="0" cellSpacing="2" cellPadding="4" class="default">
    <tr>
        <td nowrap>User ID:<font class="required">&nbsp;*</font></td>
        <td width="100%"><asp:TextBox ID="txtFrom" runat="server" CssClass="default" width="125" MaxLength="10" /></td>
    </tr>
    <tr>
        <td nowrap>Domain:<font class="required">&nbsp;*</font></td>
        <td width="100%"><asp:DropDownList ID="ddlDomain" runat="server" CssClass="default" Width="125" /></td>
    </tr>
    <tr>
        <td nowrap></td>
        <td width="100%"><asp:Button ID="btnContinue" runat="server" CssClass="default" Text="Load Properties" Width="125" OnClick="btnContinue_Click" /></td>
    </tr>
    <asp:Panel ID="panContinue" runat="server" Visible="false">
    <tr>
        <td colspan="2"><span style="width:100%;border-bottom:1 dotted #CCCCCC;"/></td>
    </tr>
    <tr>
        <td nowrap>User:</td>
        <td width="100%"><asp:label ID="lblUser" CssClass="default" runat="server" /></td>
    </tr>
    <tr>
        <td nowrap>User ID:<font class="required">&nbsp;*</font></td>
        <td width="100%"><asp:TextBox ID="txtTo" runat="server" CssClass="default" width="100" MaxLength="10" /></td>
    </tr>
    <tr>
        <td nowrap>First Name:<font class="required">&nbsp;*</font></td>
        <td width="100%"><asp:TextBox ID="txtFirst" runat="server" CssClass="default" width="200" MaxLength="50" /></td>
    </tr>
    <tr>
        <td nowrap>Last Name:<font class="required">&nbsp;*</font></td>
        <td width="100%"><asp:TextBox ID="txtLast" runat="server" CssClass="default" width="200" MaxLength="50" /></td>
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
<input type="hidden" id="hdnParent" runat="server" />