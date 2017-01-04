<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="import_checkin_wan.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.import_checkin_wan" %>
<script type="text/javascript">
</script>
<table cellpadding="4" cellspacing="3" border="0">
    <tr>
        <td colspan="2">&nbsp;</td>
    </tr>
    <tr>
        <td colspan="2"><span style="width:100%;border-bottom:1 dotted #CCCCCC;"/></td>
    </tr>
    <tr>
        <td colspan="2">
            <table width="100%" cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td nowrap class="bold">Import Check-Ins</td>
                    <td align="right"><asp:LinkButton ID="btnTemplate" runat="server" Text="<img src='/images/excel.gif' border='0' align='absmiddle' /> Download Template" /></td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td nowrap>Excel Path:</td>
        <td width="100%"><asp:FileUpload runat="server" ID="oFile" Width="600" CssClass="default" /></td>
    </tr>
    <tr>
        <td nowrap></td>
        <td width="100%"><asp:CheckBox runat="server" ID="chkOverwrite" CssClass="default" Text="Overwrite old data with this data, if the asset(s) exist" /></td>
    </tr>
    <tr>
        <td></td>
        <td width="100%"><asp:Button ID="btnImport" runat="server" CssClass="default" Text="Import" Width="75" OnClick="btnImport_Click" /></td>
    </tr>
</table>
