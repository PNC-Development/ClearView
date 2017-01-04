<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="enclosure_name.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.enclosure_name" %>
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
                    <td rowspan="2"><img src="/images/servername.gif" border="0" align="absmiddle" /></td>
                    <td class="header" width="100%" valign="bottom">Enclosure Name</td>
                </tr>
                <tr>
                    <td width="100%" valign="top">Generate your own enclosure name by completing the following form and clicking <b>Generate</b>.</td>
                </tr>
            </table>
            <table width="100%" cellpadding="4" cellspacing="3" border="0">
                <tr>
                    <td colspan="2" align="center">
                        <asp:Panel ID="panName" runat="server" Visible="false">
                            <table cellpadding="2" cellspacing="2" border="0">
                                <tr>
                                    <td><img src="/images/bigCheck.gif" border="0" align="absmiddle" /></td>
                                    <td class="header">Enclosure Name:</td>
                                    <td class="header"><asp:Label ID="lblName" runat="server" CssClass="header" /></td>
                                </tr>
                                <tr><td colspan="3">&nbsp;</td></tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
            <br />
            <table width="100%" cellpadding="4" cellspacing="3" border="0">
                <tr>
                    <td nowrap>Rack:<font class="required">&nbsp;*</font></td>
                    <td width="100%"><asp:label ID="lblParent" CssClass="default" runat="server" Text="** Please Select **" />&nbsp;&nbsp;<asp:Button ID="btnParent" runat="server" CssClass="default" Width="25" Text="..." /></td>
                </tr>
                <tr>
                    <td nowrap>Class:<font class="required">&nbsp;*</font></td>
                    <td width="100%">
                        <asp:RadioButtonList ID="radClass" runat="server" >
                            <asp:ListItem Value="A" Text="Test" />
                            <asp:ListItem Value="1" Text="Production" />
                        </asp:RadioButtonList>
                    </td>
                </tr>
                <tr>
                    <td nowrap>Rack Position:<font class="required">&nbsp;*</font></td>
                    <td width="100%">
                        <asp:RadioButtonList ID="radPosition" runat="server" >
                            <asp:ListItem Value="E4" Text="Top of the Rack" />
                            <asp:ListItem Value="E3" Text="Middle-Top of the Rack" />
                            <asp:ListItem Value="E2" Text="Middle-Bottom of the Rack" />
                            <asp:ListItem Value="E1" Text="Bottom of the Rack" />
                        </asp:RadioButtonList>
                    </td>
                </tr>
                <tr>
                    <td colspan="2"><hr size="1" noshade /></td>
                </tr>
                <tr>
                    <td nowrap class="required">* = Required Field</td>
                    <td align="right"><asp:Button ID="btnSubmit" runat="server" CssClass="default" Width="75" Text="Generate" OnClick="btnSubmit_Click" /></td>
                </tr>
            </table>
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
<input type="hidden" id="hdnParent" runat="server" />
