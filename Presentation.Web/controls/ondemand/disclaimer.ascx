<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="disclaimer.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.disclaimer" %>


<script type="text/javascript">
</script>
<table width="100%" height="100%" cellpadding="4" cellspacing="2" border="0">
    <tr height="1">
        <td><b>Before we begin, please read the following conditions:</b></td>
    </tr>
    <tr>
        <td valign="top">
            <ul>
                <asp:Panel ID="panServer" runat="server" Visible="false">
                    <li>Make sure your design is accurate and complete. You will not be prompted to re-enter or confirm any information from the initial design.<br /><br /></li>
                    <li>If for legacy National City, be prepared to enter a valid application name / 3 letter application code.<br /><br /></li>
                    <li>If for PNC, be prepared to enter a valid 3 letter mnemonic.<br /><br /></li>
                    <li>Be prepared to provide a detailed break down of your storage requirements, server by server, LUN by LUN.<br /><br /></li>
                    <li>Be prepared to provide a detailed account requirements, server by server.<br /><br /></li>
                </asp:Panel>
                <asp:Panel ID="panWorkstation" runat="server" Visible="false">
                    <li>Make sure your information is accurate and complete. You will not be prompted to re-enter or confirm any information from the initial design.<br /><br /></li>
                    <li>Be prepared to enter account information (such as remote and administrative users).<br /><br /></li>
                </asp:Panel>
            </ul>
        </td>
    </tr>
    <tr height="1">
        <td align="center">
            <table width="90%" cellpadding="0" cellspacing="5" border="0" style="border:solid 1px #999999" bgcolor="#f9f9f9">
                <tr>
                    <td rowspan="3"><img src="/images/hugeAlert.gif" border="0" align="absmiddle" /></td>
                    <td class="header" width="100%" valign="bottom">WARNING: Your Design Cannot Be Unlocked!</td>
                </tr>
                <tr>
                    <td width="100%" valign="top">Once you click the <b>Start</b> button, you will not be able to edit your design...even with an UNLOCK CODE!</td>
                </tr>
                <tr>
                    <td width="100%" valign="top">From this point forward, your design is permanently locked and no changes can be made.</td>
                </tr>
            </table>
            <br />
        </td>
    </tr>
    <tr height="1">
        <td><hr size="1" noshade /></td>
    </tr>
    <tr height="1">
        <td align="center"><asp:Button ID="btnBegin" runat="server" OnClick="btnBegin_Click" Text="Start" CssClass="default" Width="100" /> <asp:Button ID="btnClose" runat="server" Text="Cancel" CssClass="default" Width="100" /></td>
    </tr>
</table>
