<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="facilities_settings.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.facilities_settings" %>

<script type="text/javascript">
</script>
<table width="100%" cellpadding="4" cellspacing="0" border="0">
    <tr>
        <td colspan="2" class="header">Supply & Demand Chart Configuration</td>
    </tr>
    <tr>
        <td colspan="2">Set the maximum number of AMPs to be seen in the bar chart.</td>
    </tr>
    <tr>
        <td nowrap>Maximum AMPs:</td>
        <td width="100%"><asp:TextBox ID="txtMax1" runat="server" CssClass="default" Width="100" MaxLength="10" /> GB</td>
    </tr>
    <tr>
        <td></td>
        <td><asp:Button ID="btnMaximum" runat="server" CssClass="default" Width="100" Text="Save" OnClick="btnMaximum_Click" /></td>
    </tr>
    <tr>
        <td colspan="3"><span style="width:100%;border-bottom:1 dotted #999999;"/></td>
    </tr>
</table>
