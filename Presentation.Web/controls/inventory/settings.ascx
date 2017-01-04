<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="settings.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.settings" %>



<script type="text/javascript">
</script>
<table width="100%" cellpadding="4" cellspacing="0" border="0">
    <tr>
        <td colspan="2" class="header">Supply & Demand Chart Configuration</td>
    </tr>
    <tr>
        <td colspan="2">Set the maximum number of devices to be seen in the bar chart.</td>
    </tr>
    <tr>
        <td nowrap>Maximum Devices:</td>
        <td width="100%"><asp:TextBox ID="txtMax1" runat="server" CssClass="default" Width="100" MaxLength="10" /></td>
    </tr>
    <tr>
        <td></td>
        <td><asp:Button ID="btnMaximum" runat="server" CssClass="default" Width="100" Text="Save" OnClick="btnMaximum_Click" /></td>
    </tr>
    <tr>
        <td colspan="3"><span style="width:100%;border-bottom:1 dotted #999999;"/></td>
    </tr>
</table>
<br />
<table width="100%" cellpadding="4" cellspacing="0" border="0">
    <tr>
        <td colspan="3" class="header">Warning and Critical Thresholds</td>
    </tr>
    <tr>
        <td colspan="3">Configure your warning and critical thresholds to notify you when a service requires action.</td>
    </tr>
    <tr>
        <td colspan="3">[Example: Warning 10, Critical 5 = If the supply is less than 10, a warning notification will appear. If the supply is less than 5, a critical message appears.]</td>
    </tr>
    <tr>
        <td class="header"></td>
        <td class="header"><img src="/images/bigAlert.gif" border="0" align="absmiddle" /> Warning</td>
        <td class="header"><img src="/images/bigError.gif" border="0" align="absmiddle" /> Critical</td>
    </tr>
    <%=strThreshold %>
    <tr>
        <td></td>
        <td colspan="2"><asp:Button ID="btnSave" runat="server" CssClass="default" Width="100" Text="Save" OnClick="btnSave_Click" /></td>
    </tr>
    <tr>
        <td colspan="3"><span style="width:100%;border-bottom:1 dotted #999999;"/></td>
    </tr>
</table>
<br />
<table width="100%" cellpadding="4" cellspacing="0" border="0">
    <tr>
        <td colspan="3" class="header">Model Thresholds</td>
    </tr>
    <tr>
        <td colspan="3">Configure the number of days out a device can be executed (based on the quantity).</td>
    </tr>
    <tr>
        <td colspan="3">[Example: Qty From 1 To 3, 5 days = If the quantity is between 1 and 3, the person must wait 5 days from today to execute]</td>
    </tr>
    <%=strModelThreshold %>
</table>