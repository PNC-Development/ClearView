<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="fore_workstation.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.fore_workstation" %>
<script type="text/javascript">
</script>
<table width="100%" cellpadding="4" cellspacing="2" border="0">
    <tr>
        <td nowrap><b>Type:</b><font class="required">&nbsp;*</font></td>
        <td width="100%"><asp:DropDownList ID="ddlTypes" runat="server" CssClass="default" Width="300" AutoPostBack="true" OnSelectedIndexChanged="ddlTypes_Change" /></td>
    </tr>
    <asp:Panel ID="panModels" runat="server" Visible="false">
    <tr>
        <td nowrap><b>Model:</b><font class="required">&nbsp;*</font></td>
        <td width="100%"><asp:DropDownList ID="ddlModels" runat="server" CssClass="default" Width="300" AutoPostBack="true" OnSelectedIndexChanged="ddlModels_Change" /></td>
    </tr>
    </asp:Panel>
    <asp:Panel ID="panVirtual" runat="server" Visible="false">
    <tr>
        <td nowrap><b>RAM:</b><font class="required">&nbsp;*</font></td>
        <td width="100%"><asp:DropDownList ID="ddlRam" runat="server" CssClass="default" Width="300" /></td>
    </tr>
    <tr>
        <td nowrap><b>Operating System:</b><font class="required">&nbsp;*</font></td>
        <td width="100%"><asp:DropDownList ID="ddlOS" runat="server" CssClass="default" Width="300" /></td>
    </tr>
    <tr>
        <td nowrap>CPUs:<font class="required">&nbsp;*</font></td>
        <td width="100%"><asp:DropDownList ID="ddlCPU" runat="server" CssClass="default" Width="300" /></td>
    </tr>
    <tr>
        <td nowrap><b>Hard Drive:</b><font class="required">&nbsp;*</font></td>
        <td width="100%"><asp:DropDownList ID="ddlHardDrive" runat="server" CssClass="default" Width="300" /></td>
    </tr>
    <asp:Panel ID="panProduction" runat="server" Visible="false">
    <tr>
        <td nowrap><b>Recovery:</b><font class="required">&nbsp;*</font></td>
        <td width="100%">
            <asp:DropDownList ID="ddlRecovery" runat="server" CssClass="default" Width="300">
                <asp:ListItem Value="0" Text="-- SELECT --" />
                <asp:ListItem Value="-1" Text="Over 48 Hours" />
                <asp:ListItem Value="1" Text="Under 48 Hours" />
            </asp:DropDownList>
        </td>
    </tr>
    </asp:Panel>
    <tr>
        <td colspan="2">&nbsp;</td>
    </tr>
    <tr>
        <td colspan="2">
            <table width="100%" cellpadding="4" cellspacing="0" border="0" style="border:solid 1px #999999" bgcolor="#f9f9f9">
                <tr>
                    <td nowrap><img src="/images/bigAlert.gif" border="0" align="absmiddle" /></td>
                    <td width="100%"><b>NOTE:</b> You cannot attach a physical device to a virtual workstation. If your initiative requires special hardware, do not choose a virtual workstation.</td>
                </tr>
            </table>
        </td>
    </tr>
    </asp:Panel>
</table>
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
    </asp:Panel>
</table>
<input type="hidden" id="hdnParent" runat="server" />
