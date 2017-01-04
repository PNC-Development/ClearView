<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="asset_checkin.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.asset_checkin" %>
<script type="text/javascript">
</script>
<table width="100%" cellpadding="0" cellspacing="0" border="0" class="default">
    <tr>
        <td><img src="/images/table_topleft.gif" border="0" width="5" height="26"></td>
        <td nowrap background="/images/table_top.gif" class="greentableheader" width="100%"><asp:Label ID="lblTitle" runat="server" CssClass="greentableheader" /></td>
        <td><img src="/images/table_topright.gif" border="0" width="5" height="26"></td>
    </tr>
    <tr>
        <td background="/images/table_left.gif"><img src="/images/table_left.gif" width="5" height="10"></td>
        <td width="100%" bgcolor="#FFFFFF">
            <br />
            <table width="100%" cellpadding="0" cellspacing="5" border="0">
                <tr>
                    <td rowspan="2"><img src="/images/asset_checkin.gif" border="0" align="absmiddle" /></td>
                    <td class="header" width="100%" valign="bottom">Check-In Asset</td>
                </tr>
                <tr>
                    <td width="100%" valign="top">Asset check-in is required when new hardware is received at National City. Please complete the following form to check-in a new asset.</td>
                </tr>
            </table>
            <table width="100%" cellpadding="4" cellspacing="3" border="0">
                <tr>
                    <td nowrap>Platform:<font class="required">&nbsp;*</font></td>
                    <td width="100%">
                        <table cellpadding="0" cellspacing="0" border="0">
                            <tr>
                                <td><asp:DropDownList ID="ddlPlatform" runat="server" CssClass="default" Width="300" AutoPostBack="true" OnSelectedIndexChanged="ddlPlatform_Change" /></td>
                                <td class="bold">
                                    <div id="divWait" runat="server" style="display:none">
                                        <img src="/images/spacer.gif" border="0" width="20" height="1" /><img src="/images/please_wait.gif" border="0" align="absmiddle" /> Please Wait...
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <asp:Panel ID="panTypes" runat="server" Visible="false">
                <tr>
                    <td nowrap>Asset Type:<font class="required">&nbsp;*</font></td>
                    <td width="100%">
                        <table cellpadding="0" cellspacing="0" border="0">
                            <tr>
                                <td><asp:DropDownList ID="ddlTypes" runat="server" CssClass="default" Width="300" AutoPostBack="true" OnSelectedIndexChanged="ddlTypes_Change" /></td>
                                <td class="bold">
                                    <div id="divWait2" runat="server" style="display:none">
                                        <img src="/images/spacer.gif" border="0" width="20" height="1" /><img src="/images/please_wait.gif" border="0" align="absmiddle" /> Please Wait...
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                </asp:Panel>
                <asp:Panel ID="panPath" runat="server" Visible="false">
                    <asp:PlaceHolder ID="PHControl" runat="server" />
                </asp:Panel>
                <asp:Panel ID="panNoPath" runat="server" Visible="false">
                <tr>
                    <td colspan="2" align="center"><img src="/images/alert.gif" border="0" align="absmiddle" /> Asset information has not been configured for this platform...</td>
                </tr>
                </asp:Panel>
            </table>
            <asp:PlaceHolder ID="PHImport" runat="server" />
            <p>&nbsp;</p>
        </td>
        <td background="/images/table_right.gif"><img src="/images/table_right.gif" width="5" height="10"></td>
    </tr>
    <tr>
        <td><img src="/images/table_bottomLeft.gif" border="0" width="5" height="9"></td>
        <td width="100%" background="/images/table_bottom.gif"></td>
        <td><img src="/images/table_bottomRight.gif" border="0" width="5" height="9"></td>
    </tr>
</table>
