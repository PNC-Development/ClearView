<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="fore_additional.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.fore_additional" %>
<script type="text/javascript">
</script>
<asp:PlaceHolder ID="PHAdditional" runat="server" />
<asp:Panel ID="panNone" runat="server" Visible="false">
<table width="100%" cellpadding="4" cellspacing="0" border="0" style="border:solid 1px #999999" bgcolor="#f9f9f9">
    <tr>
        <td nowrap><img src="/images/bigCheck.gif" border="0" align="absmiddle" /></td>
        <td width="100%">There is no additional configuration.</td>
    </tr>
</table>
<br />
<table width="100%" cellpadding="4" cellspacing="2" border="0">
    <tr>
        <td colspan="3"><hr size="1" noshade /></td>
    </tr>
    <tr>
        <td class="required">* = Required Field</td>
        <td align="center">
            <asp:Panel ID="panNavigation" runat="server" Visible="false">
                <asp:Button ID="btnBack" runat="server" OnClick="btnBack_Click" Text="Back" CssClass="default" Width="75" /> <asp:Button ID="btnNext" runat="server" OnClick="btnNext_Click" Text="Next" CssClass="default" Width="75" />
            </asp:Panel>
            <asp:Panel ID="panUpdate" runat="server" Visible="false">
                <asp:Button ID="btnUpdate" runat="server" OnClick="btnUpdate_Click" Text="Update" CssClass="default" Width="75" /> <asp:Button ID="btnCancel" runat="server" OnClick="btnCancel_Click" Text="Cancel" CssClass="default" Width="75" />
            </asp:Panel>
        </td>
        <td align="right">
            <asp:Button ID="btnHundred" runat="server" OnClick="btnCancel_Click" Text="Back" CssClass="default" Width="75" Visible="false" /> 
            <asp:Button ID="btnClose" runat="server" Text="Close" CssClass="default" Width="75" />
        </td>
    </tr>
</table>
</asp:Panel>