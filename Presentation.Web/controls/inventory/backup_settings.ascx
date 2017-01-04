<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="backup_settings.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.backup_settings" %>

<script type="text/javascript">
</script>
<br />
<table width="100%" height="100%" cellpadding="5" cellspacing="5" border="0">
    <tr height="1">
        <td colspan="2" class="biggerbold">Backup Location Configuration:</td>
    </tr>
    <tr>
        <td colspan="2">
            <table cellpadding="5" cellspacing="5" border="1">
                <tr class="greentableheader">
                    <td colspan="2"></td>
                    <td>Cleveland</td>
                    <td>Dalton</td>
                    <td>Summit</td>
                    <td>Firstside</td>
                    <td>Other (Offsite)</td>
                </tr>
                <tr>
                    <td rowspan="2">
                        <table class="header" cellpadding="0" cellspacing="5" border="0">
                            <tr>
                                <td rowspan="2"><img src="/images/logo_windows.gif" border="0" align="absmiddle" /></td>
                                <td width="100%" valign="bottom">Microsoft</td>
                            </tr>
                            <tr>
                                <td width="100%" valign="top">Windows</td>
                            </tr>
                        </table>
                    </td>
                    <td class="biggerbold">Physical</td>
                    <td>
                        <asp:RadioButtonList ID="radWindowsPC" runat="server" RepeatColumns="1" RepeatDirection="Vertical">
                            <asp:ListItem Text="Avamar" Value="A" />
                            <asp:ListItem Text="Networker" Value="N" />
                            <asp:ListItem Text="TSM" Value="T" Selected="True" />
                        </asp:RadioButtonList>
                    </td>
                    <td>
                        <asp:RadioButtonList ID="radWindowsPD" runat="server" RepeatColumns="1" RepeatDirection="Vertical">
                            <asp:ListItem Text="Avamar" Value="A" Selected="True" />
                            <asp:ListItem Text="Networker" Value="N" />
                            <asp:ListItem Text="TSM" Value="T" />
                        </asp:RadioButtonList>
                    </td>
                    <td>
                        <asp:RadioButtonList ID="radWindowsPS" runat="server" RepeatColumns="1" RepeatDirection="Vertical">
                            <asp:ListItem Text="Avamar" Value="A" Selected="True" />
                            <asp:ListItem Text="Networker" Value="N" />
                            <asp:ListItem Text="TSM" Value="T" />
                        </asp:RadioButtonList>
                    </td>
                    <td>
                        <asp:RadioButtonList ID="radWindowsPF" runat="server" RepeatColumns="1" RepeatDirection="Vertical">
                            <asp:ListItem Text="Avamar" Value="A" />
                            <asp:ListItem Text="Networker" Value="N" Selected="True" />
                            <asp:ListItem Text="TSM" Value="T" />
                        </asp:RadioButtonList>
                    </td>
                    <td>
                        <asp:RadioButtonList ID="radWindowsPO" runat="server" RepeatColumns="1" RepeatDirection="Vertical">
                            <asp:ListItem Text="Avamar" Value="A" Selected="True" />
                            <asp:ListItem Text="Networker" Value="N" />
                            <asp:ListItem Text="TSM" Value="T" />
                        </asp:RadioButtonList>
                    </td>
                </tr>
                <tr>
                    <td class="biggerbold">Virtual</td>
                    <td>
                        <asp:RadioButtonList ID="radWindowsVC" runat="server" RepeatColumns="1" RepeatDirection="Vertical">
                            <asp:ListItem Text="Avamar" Value="A" />
                            <asp:ListItem Text="Networker" Value="N" />
                            <asp:ListItem Text="TSM" Value="T" Selected="True" />
                        </asp:RadioButtonList>
                    </td>
                    <td>
                        <asp:RadioButtonList ID="radWindowsVD" runat="server" RepeatColumns="1" RepeatDirection="Vertical">
                            <asp:ListItem Text="Avamar" Value="A" Selected="True" />
                            <asp:ListItem Text="Networker" Value="N" />
                            <asp:ListItem Text="TSM" Value="T" />
                        </asp:RadioButtonList>
                    </td>
                    <td>
                        <asp:RadioButtonList ID="radWindowsVS" runat="server" RepeatColumns="1" RepeatDirection="Vertical">
                            <asp:ListItem Text="Avamar" Value="A" Selected="True" />
                            <asp:ListItem Text="Networker" Value="N" />
                            <asp:ListItem Text="TSM" Value="T" />
                        </asp:RadioButtonList>
                    </td>
                    <td>
                        <asp:RadioButtonList ID="radWindowsVF" runat="server" RepeatColumns="1" RepeatDirection="Vertical">
                            <asp:ListItem Text="Avamar" Value="A" />
                            <asp:ListItem Text="Networker" Value="N" Selected="True" />
                            <asp:ListItem Text="TSM" Value="T" />
                        </asp:RadioButtonList>
                    </td>
                    <td>
                        <asp:RadioButtonList ID="radWindowsVO" runat="server" RepeatColumns="1" RepeatDirection="Vertical">
                            <asp:ListItem Text="Avamar" Value="A" Selected="True" />
                            <asp:ListItem Text="Networker" Value="N" />
                            <asp:ListItem Text="TSM" Value="T" />
                        </asp:RadioButtonList>
                    </td>
                </tr>
                <tr class="greentableheader">
                    <td colspan="2"></td>
                    <td>Cleveland</td>
                    <td>Dalton</td>
                    <td>Summit</td>
                    <td>Firstside</td>
                    <td>Other (Offsite)</td>
                </tr>
                <tr>
                    <td rowspan="2">
                        <table class="header" cellpadding="0" cellspacing="5" border="0">
                            <tr>
                                <td rowspan="2"><img src="/images/logo_linux.gif" border="0" align="absmiddle" /></td>
                                <td width="100%" valign="bottom">Red Hat</td>
                            </tr>
                            <tr>
                                <td width="100%" valign="top">Linux</td>
                            </tr>
                        </table>
                    </td>
                    <td class="biggerbold">Physical</td>
                    <td>
                        <asp:RadioButtonList ID="radLinuxPC" runat="server" RepeatColumns="1" RepeatDirection="Vertical">
                            <asp:ListItem Text="Avamar" Value="A" />
                            <asp:ListItem Text="Networker" Value="N" />
                            <asp:ListItem Text="TSM" Value="T" Selected="True" />
                        </asp:RadioButtonList>
                    </td>
                    <td>
                        <asp:RadioButtonList ID="radLinuxPD" runat="server" RepeatColumns="1" RepeatDirection="Vertical">
                            <asp:ListItem Text="Avamar" Value="A" Selected="True" />
                            <asp:ListItem Text="Networker" Value="N" />
                            <asp:ListItem Text="TSM" Value="T" />
                        </asp:RadioButtonList>
                    </td>
                    <td>
                        <asp:RadioButtonList ID="radLinuxPS" runat="server" RepeatColumns="1" RepeatDirection="Vertical">
                            <asp:ListItem Text="Avamar" Value="A" Selected="True" />
                            <asp:ListItem Text="Networker" Value="N" />
                            <asp:ListItem Text="TSM" Value="T" />
                        </asp:RadioButtonList>
                    </td>
                    <td>
                        <asp:RadioButtonList ID="radLinuxPF" runat="server" RepeatColumns="1" RepeatDirection="Vertical">
                            <asp:ListItem Text="Avamar" Value="A" />
                            <asp:ListItem Text="Networker" Value="N" Selected="True" />
                            <asp:ListItem Text="TSM" Value="T" />
                        </asp:RadioButtonList>
                    </td>
                    <td>
                        <asp:RadioButtonList ID="radLinuxPO" runat="server" RepeatColumns="1" RepeatDirection="Vertical">
                            <asp:ListItem Text="Avamar" Value="A" Selected="True" />
                            <asp:ListItem Text="Networker" Value="N" />
                            <asp:ListItem Text="TSM" Value="T" />
                        </asp:RadioButtonList>
                    </td>
                </tr>
                <tr>
                    <td class="biggerbold">Virtual</td>
                    <td>
                        <asp:RadioButtonList ID="radLinuxVC" runat="server" RepeatColumns="1" RepeatDirection="Vertical">
                            <asp:ListItem Text="Avamar" Value="A" />
                            <asp:ListItem Text="Networker" Value="N" />
                            <asp:ListItem Text="TSM" Value="T" Selected="True" />
                        </asp:RadioButtonList>
                    </td>
                    <td>
                        <asp:RadioButtonList ID="radLinuxVD" runat="server" RepeatColumns="1" RepeatDirection="Vertical">
                            <asp:ListItem Text="Avamar" Value="A" Selected="True" />
                            <asp:ListItem Text="Networker" Value="N" />
                            <asp:ListItem Text="TSM" Value="T" />
                        </asp:RadioButtonList>
                    </td>
                    <td>
                        <asp:RadioButtonList ID="radLinuxVS" runat="server" RepeatColumns="1" RepeatDirection="Vertical">
                            <asp:ListItem Text="Avamar" Value="A" Selected="True" />
                            <asp:ListItem Text="Networker" Value="N" />
                            <asp:ListItem Text="TSM" Value="T" />
                        </asp:RadioButtonList>
                    </td>
                    <td>
                        <asp:RadioButtonList ID="radLinuxVF" runat="server" RepeatColumns="1" RepeatDirection="Vertical">
                            <asp:ListItem Text="Avamar" Value="A" />
                            <asp:ListItem Text="Networker" Value="N" Selected="True" />
                            <asp:ListItem Text="TSM" Value="T" />
                        </asp:RadioButtonList>
                    </td>
                    <td>
                        <asp:RadioButtonList ID="radLinuxVO" runat="server" RepeatColumns="1" RepeatDirection="Vertical">
                            <asp:ListItem Text="Avamar" Value="A" Selected="True" />
                            <asp:ListItem Text="Networker" Value="N" />
                            <asp:ListItem Text="TSM" Value="T" />
                        </asp:RadioButtonList>
                    </td>
                </tr>
                <tr class="greentableheader">
                    <td colspan="2"></td>
                    <td>Cleveland</td>
                    <td>Dalton</td>
                    <td>Summit</td>
                    <td>Firstside</td>
                    <td>Other (Offsite)</td>
                </tr>
                <tr>
                    <td rowspan="2">
                        <table class="header" cellpadding="0" cellspacing="5" border="0">
                            <tr>
                                <td rowspan="2"><img src="/images/logo_solaris.gif" border="0" align="absmiddle" /></td>
                                <td width="100%" valign="bottom">Oracle</td>
                            </tr>
                            <tr>
                                <td width="100%" valign="top">Solaris</td>
                            </tr>
                        </table>
                    </td>
                    <td class="biggerbold">Physical</td>
                    <td>
                        <asp:RadioButtonList ID="radSolarisPC" runat="server" RepeatColumns="1" RepeatDirection="Vertical">
                            <asp:ListItem Text="Avamar" Value="A" />
                            <asp:ListItem Text="Networker" Value="N" />
                            <asp:ListItem Text="TSM" Value="T" Selected="True" />
                        </asp:RadioButtonList>
                    </td>
                    <td>
                        <asp:RadioButtonList ID="radSolarisPD" runat="server" RepeatColumns="1" RepeatDirection="Vertical">
                            <asp:ListItem Text="Avamar" Value="A" Selected="True" />
                            <asp:ListItem Text="Networker" Value="N" />
                            <asp:ListItem Text="TSM" Value="T" />
                        </asp:RadioButtonList>
                    </td>
                    <td>
                        <asp:RadioButtonList ID="radSolarisPS" runat="server" RepeatColumns="1" RepeatDirection="Vertical">
                            <asp:ListItem Text="Avamar" Value="A" Selected="True" />
                            <asp:ListItem Text="Networker" Value="N" />
                            <asp:ListItem Text="TSM" Value="T" />
                        </asp:RadioButtonList>
                    </td>
                    <td>
                        <asp:RadioButtonList ID="radSolarisPF" runat="server" RepeatColumns="1" RepeatDirection="Vertical">
                            <asp:ListItem Text="Avamar" Value="A" />
                            <asp:ListItem Text="Networker" Value="N" Selected="True" />
                            <asp:ListItem Text="TSM" Value="T" />
                        </asp:RadioButtonList>
                    </td>
                    <td>
                        <asp:RadioButtonList ID="radSolarisPO" runat="server" RepeatColumns="1" RepeatDirection="Vertical">
                            <asp:ListItem Text="Avamar" Value="A" Selected="True" />
                            <asp:ListItem Text="Networker" Value="N" />
                            <asp:ListItem Text="TSM" Value="T" />
                        </asp:RadioButtonList>
                    </td>
                </tr>
                <tr>
                    <td class="biggerbold">Virtual</td>
                    <td>
                        <asp:RadioButtonList ID="radSolarisVC" runat="server" RepeatColumns="1" RepeatDirection="Vertical">
                            <asp:ListItem Text="Avamar" Value="A" />
                            <asp:ListItem Text="Networker" Value="N" />
                            <asp:ListItem Text="TSM" Value="T" Selected="True" />
                        </asp:RadioButtonList>
                    </td>
                    <td>
                        <asp:RadioButtonList ID="radSolarisVD" runat="server" RepeatColumns="1" RepeatDirection="Vertical">
                            <asp:ListItem Text="Avamar" Value="A" Selected="True" />
                            <asp:ListItem Text="Networker" Value="N" />
                            <asp:ListItem Text="TSM" Value="T" />
                        </asp:RadioButtonList>
                    </td>
                    <td>
                        <asp:RadioButtonList ID="radSolarisVS" runat="server" RepeatColumns="1" RepeatDirection="Vertical">
                            <asp:ListItem Text="Avamar" Value="A" Selected="True" />
                            <asp:ListItem Text="Networker" Value="N" />
                            <asp:ListItem Text="TSM" Value="T" />
                        </asp:RadioButtonList>
                    </td>
                    <td>
                        <asp:RadioButtonList ID="radSolarisVF" runat="server" RepeatColumns="1" RepeatDirection="Vertical">
                            <asp:ListItem Text="Avamar" Value="A" />
                            <asp:ListItem Text="Networker" Value="N" Selected="True" />
                            <asp:ListItem Text="TSM" Value="T" />
                        </asp:RadioButtonList>
                    </td>
                    <td>
                        <asp:RadioButtonList ID="radSolarisVO" runat="server" RepeatColumns="1" RepeatDirection="Vertical">
                            <asp:ListItem Text="Avamar" Value="A" Selected="True" />
                            <asp:ListItem Text="Networker" Value="N" />
                            <asp:ListItem Text="TSM" Value="T" />
                        </asp:RadioButtonList>
                    </td>
                </tr>
                <tr class="greentableheader">
                    <td colspan="2"></td>
                    <td>Cleveland</td>
                    <td>Dalton</td>
                    <td>Summit</td>
                    <td>Firstside</td>
                    <td>Other (Offsite)</td>
                </tr>
                <tr>
                    <td rowspan="2">
                        <table class="header" cellpadding="0" cellspacing="5" border="0">
                            <tr>
                                <td rowspan="2"><img src="/images/logo_aix.gif" border="0" align="absmiddle" /></td>
                                <td width="100%" valign="bottom">IBM</td>
                            </tr>
                            <tr>
                                <td width="100%" valign="top">AIX</td>
                            </tr>
                        </table>
                    </td>
                    <td class="biggerbold">Physical</td>
                    <td>
                        <asp:RadioButtonList ID="radAIXPC" runat="server" RepeatColumns="1" RepeatDirection="Vertical">
                            <asp:ListItem Text="Avamar" Value="A" />
                            <asp:ListItem Text="Networker" Value="N" />
                            <asp:ListItem Text="TSM" Value="T" Selected="True" />
                        </asp:RadioButtonList>
                    </td>
                    <td>
                        <asp:RadioButtonList ID="radAIXPD" runat="server" RepeatColumns="1" RepeatDirection="Vertical">
                            <asp:ListItem Text="Avamar" Value="A" Selected="True" />
                            <asp:ListItem Text="Networker" Value="N" />
                            <asp:ListItem Text="TSM" Value="T" />
                        </asp:RadioButtonList>
                    </td>
                    <td>
                        <asp:RadioButtonList ID="radAIXPS" runat="server" RepeatColumns="1" RepeatDirection="Vertical">
                            <asp:ListItem Text="Avamar" Value="A" Selected="True" />
                            <asp:ListItem Text="Networker" Value="N" />
                            <asp:ListItem Text="TSM" Value="T" />
                        </asp:RadioButtonList>
                    </td>
                    <td>
                        <asp:RadioButtonList ID="radAIXPF" runat="server" RepeatColumns="1" RepeatDirection="Vertical">
                            <asp:ListItem Text="Avamar" Value="A" />
                            <asp:ListItem Text="Networker" Value="N" Selected="True" />
                            <asp:ListItem Text="TSM" Value="T" />
                        </asp:RadioButtonList>
                    </td>
                    <td>
                        <asp:RadioButtonList ID="radAIXPO" runat="server" RepeatColumns="1" RepeatDirection="Vertical">
                            <asp:ListItem Text="Avamar" Value="A" Selected="True" />
                            <asp:ListItem Text="Networker" Value="N" />
                            <asp:ListItem Text="TSM" Value="T" />
                        </asp:RadioButtonList>
                    </td>
                </tr>
                <tr>
                    <td class="biggerbold">Virtual</td>
                    <td>
                        <asp:RadioButtonList ID="radAIXVC" runat="server" RepeatColumns="1" RepeatDirection="Vertical">
                            <asp:ListItem Text="Avamar" Value="A" />
                            <asp:ListItem Text="Networker" Value="N" />
                            <asp:ListItem Text="TSM" Value="T" Selected="True" />
                        </asp:RadioButtonList>
                    </td>
                    <td>
                        <asp:RadioButtonList ID="radAIXVD" runat="server" RepeatColumns="1" RepeatDirection="Vertical">
                            <asp:ListItem Text="Avamar" Value="A" Selected="True" />
                            <asp:ListItem Text="Networker" Value="N" />
                            <asp:ListItem Text="TSM" Value="T" />
                        </asp:RadioButtonList>
                    </td>
                    <td>
                        <asp:RadioButtonList ID="radAIXVS" runat="server" RepeatColumns="1" RepeatDirection="Vertical">
                            <asp:ListItem Text="Avamar" Value="A" Selected="True" />
                            <asp:ListItem Text="Networker" Value="N" />
                            <asp:ListItem Text="TSM" Value="T" />
                        </asp:RadioButtonList>
                    </td>
                    <td>
                        <asp:RadioButtonList ID="radAIXVF" runat="server" RepeatColumns="1" RepeatDirection="Vertical">
                            <asp:ListItem Text="Avamar" Value="A" />
                            <asp:ListItem Text="Networker" Value="N" Selected="True" />
                            <asp:ListItem Text="TSM" Value="T" />
                        </asp:RadioButtonList>
                    </td>
                    <td>
                        <asp:RadioButtonList ID="radAIXVO" runat="server" RepeatColumns="1" RepeatDirection="Vertical">
                            <asp:ListItem Text="Avamar" Value="A" Selected="True" />
                            <asp:ListItem Text="Networker" Value="N" />
                            <asp:ListItem Text="TSM" Value="T" />
                        </asp:RadioButtonList>
                    </td>
                </tr>
                <tr class="greentableheader">
                    <td colspan="2"></td>
                    <td>Cleveland</td>
                    <td>Dalton</td>
                    <td>Summit</td>
                    <td>Firstside</td>
                    <td>Other (Offsite)</td>
                </tr>
                <tr>
                    <td rowspan="2">
                        <table class="header" cellpadding="0" cellspacing="5" border="0">
                            <tr>
                                <td rowspan="2"><img src="/images/bigHelp.gif" border="0" align="absmiddle" /></td>
                                <td width="100%" valign="bottom">Other</td>
                            </tr>
                            <tr>
                                <td width="100%" valign="top"></td>
                            </tr>
                        </table>
                    </td>
                    <td class="biggerbold">Physical</td>
                    <td>
                        <asp:RadioButtonList ID="radOtherPC" runat="server" RepeatColumns="1" RepeatDirection="Vertical">
                            <asp:ListItem Text="Avamar" Value="A" />
                            <asp:ListItem Text="Networker" Value="N" />
                            <asp:ListItem Text="TSM" Value="T" Selected="True" />
                        </asp:RadioButtonList>
                    </td>
                    <td>
                        <asp:RadioButtonList ID="radOtherPD" runat="server" RepeatColumns="1" RepeatDirection="Vertical">
                            <asp:ListItem Text="Avamar" Value="A" Selected="True" />
                            <asp:ListItem Text="Networker" Value="N" />
                            <asp:ListItem Text="TSM" Value="T" />
                        </asp:RadioButtonList>
                    </td>
                    <td>
                        <asp:RadioButtonList ID="radOtherPS" runat="server" RepeatColumns="1" RepeatDirection="Vertical">
                            <asp:ListItem Text="Avamar" Value="A" Selected="True" />
                            <asp:ListItem Text="Networker" Value="N" />
                            <asp:ListItem Text="TSM" Value="T" />
                        </asp:RadioButtonList>
                    </td>
                    <td>
                        <asp:RadioButtonList ID="radOtherPF" runat="server" RepeatColumns="1" RepeatDirection="Vertical">
                            <asp:ListItem Text="Avamar" Value="A" />
                            <asp:ListItem Text="Networker" Value="N" Selected="True" />
                            <asp:ListItem Text="TSM" Value="T" />
                        </asp:RadioButtonList>
                    </td>
                    <td>
                        <asp:RadioButtonList ID="radOtherPO" runat="server" RepeatColumns="1" RepeatDirection="Vertical">
                            <asp:ListItem Text="Avamar" Value="A" Selected="True" />
                            <asp:ListItem Text="Networker" Value="N" />
                            <asp:ListItem Text="TSM" Value="T" />
                        </asp:RadioButtonList>
                    </td>
                </tr>
                <tr>
                    <td class="biggerbold">Virtual</td>
                    <td>
                        <asp:RadioButtonList ID="radOtherVC" runat="server" RepeatColumns="1" RepeatDirection="Vertical">
                            <asp:ListItem Text="Avamar" Value="A" />
                            <asp:ListItem Text="Networker" Value="N" />
                            <asp:ListItem Text="TSM" Value="T" Selected="True" />
                        </asp:RadioButtonList>
                    </td>
                    <td>
                        <asp:RadioButtonList ID="radOtherVD" runat="server" RepeatColumns="1" RepeatDirection="Vertical">
                            <asp:ListItem Text="Avamar" Value="A" Selected="True" />
                            <asp:ListItem Text="Networker" Value="N" />
                            <asp:ListItem Text="TSM" Value="T" />
                        </asp:RadioButtonList>
                    </td>
                    <td>
                        <asp:RadioButtonList ID="radOtherVS" runat="server" RepeatColumns="1" RepeatDirection="Vertical">
                            <asp:ListItem Text="Avamar" Value="A" Selected="True" />
                            <asp:ListItem Text="Networker" Value="N" />
                            <asp:ListItem Text="TSM" Value="T" />
                        </asp:RadioButtonList>
                    </td>
                    <td>
                        <asp:RadioButtonList ID="radOtherVF" runat="server" RepeatColumns="1" RepeatDirection="Vertical">
                            <asp:ListItem Text="Avamar" Value="A" />
                            <asp:ListItem Text="Networker" Value="N" Selected="True" />
                            <asp:ListItem Text="TSM" Value="T" />
                        </asp:RadioButtonList>
                    </td>
                    <td>
                        <asp:RadioButtonList ID="radOtherVO" runat="server" RepeatColumns="1" RepeatDirection="Vertical">
                            <asp:ListItem Text="Avamar" Value="A" Selected="True" />
                            <asp:ListItem Text="Networker" Value="N" />
                            <asp:ListItem Text="TSM" Value="T" />
                        </asp:RadioButtonList>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
