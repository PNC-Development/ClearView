<%@ Page Language="C#" MasterPageFile="~/window.Master" AutoEventWireup="true" CodeBehind="config_workstation_vmware.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.config_workstation_vmware" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" runat="server">
<table width="100%" height="100%" cellpadding="0" cellspacing="0" border="0">
    <tr height="1">
        <td>
            <table width="100%" cellpadding="0" cellspacing="5" border="0">
                <tr>
                    <td rowspan="2"><img src="/images/device_config.gif" border="0" align="middle" /></td>
                    <td class="header" width="100%" valign="bottom">Configure a VMware Workstation</td>
                </tr>
                <tr>
                    <td width="100%" valign="top">Prepare your VMware workstation for build by completing the following questions.</td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td valign="top">
            <div style="height:100%; overflow:auto">
            <table width="100%" cellpadding="4" cellspacing="0" border="0">
                <tr>
                    <td nowrap>Operating System:<font class="required">&nbsp;*</font></td>
                    <td width="100%"><asp:DropDownList ID="ddlOS" runat="server" CssClass="default" Width="250" /></td>
                </tr>
                <tr>
                    <td nowrap>Service Pack Level:<font class="required">&nbsp;*</font></td>
                    <td width="100%">
                        <asp:DropDownList ID="ddlServicePack" CssClass="default" runat="server" Width="250" Enabled="false" >
                            <asp:ListItem Value="-- Please select an Operating System --" />
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td nowrap>Check if you require any of the following:</td>
                    <td width="100%"><asp:CheckBoxList ID="chkComponents" runat="server" CssClass="default" RepeatDirection="Horizontal" CellPadding="4" /></td>
                </tr>
                <tr>
                    <td nowrap>Domain:<font class="required">&nbsp;*</font></td>
                    <td width="100%"><asp:DropDownList ID="ddlDomain" runat="server" CssClass="default" Width="250" /></td>
                </tr>
                <tr>
                    <td nowrap></td>
                    <td width="100%"><asp:CheckBox ID="chkApply" runat="server" CssClass="default" Text="Apply these settings to all workstations" Visible="false" /></td>
                </tr>
            </table>
            </div>
        </td>
    </tr>
    <tr height="1">
        <td>
            <table width="100%" cellpadding="4" cellspacing="2" border="0">
                <tr>
                    <td colspan="3"><hr size="1" noshade /></td>
                </tr>
                <tr>
                    <td class="required">* = Required Field</td>
                    <td align="center">
                    </td>
                    <td align="right">
                        <asp:Button ID="btnSaveConfig" runat="server" Text="Save" CssClass="default" Width="75" OnClick="btnSaveConfig_Click" /> 
                        <asp:Button ID="btnClose" runat="server" Text="Close" CssClass="default" Width="75" />
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
<asp:Label ID="lblId" runat="server" Visible="false" />
<input type="hidden" id="hdnServicePack" runat="server" />
</asp:Content>
