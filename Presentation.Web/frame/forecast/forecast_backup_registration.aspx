<%@ Page Language="C#" MasterPageFile="~/window.Master" AutoEventWireup="true" CodeBehind="forecast_backup_registration.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.forecast_backup_registration" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" runat="server">
<table width="700" cellpadding="2" cellspacing="2" border="0">
    <tr>
        <td align="center" class="header">Trivoli Storage Manager</td>
    </tr>
    <tr>
        <td align="center" class="header">Server Registration Form</td>
    </tr>
</table>
<br />
<table width="700" cellpadding="2" cellspacing="2" border="0">
    <tr>
        <td nowrap>Project Name:</td>
        <td width="50%"><asp:Label ID="lblPrjName" runat="server" CssClass="default" /></td>
        <td nowrap>Date:</td>
        <td width="50%"><asp:Label ID="lblDate" runat="server" CssClass="default" /></td>
    </tr>
    <tr>
        <td nowrap>Project Manager:</td>
        <td width="50%"><asp:Label ID="lblPrjMgr" runat="server" CssClass="default" /></td>
        <td nowrap>Phone Number:</td>
        <td width="50%"><asp:Label ID="lblPhoneNum1" runat="server" CssClass="default" /></td>
    </tr>
    <tr>
        <td nowrap>Requester:</td>
        <td width="50%"><asp:Label ID="lblRequester" runat="server" CssClass="default" /></td>
        <td nowrap>Phone Number:</td>
        <td width="50%"><asp:Label ID="lblPhoneNum2" runat="server" CssClass="default" /></td>
    </tr>
    <tr>
        <td nowrap>Recovery Location:</td>
        <td width="50%"><asp:Label ID="lblRecoveryLocation" runat="server" CssClass="default" /></td>
        <td nowrap></td>
        <td width="50%"></td>
    </tr>
</table>
<br />
<table width="700" cellpadding="2" cellspacing="2" border="0">
    <tr>
        <td colspan="2">Production Turnover Documentation folder name (for a new server being added) where turnover documents are located:</td>
    </tr>
    <tr>
        <td nowrap><img src="/images/spacer.gif" border="0" width="20" height="1" /></td>
        <td width="100%"><asp:Label ID="lblPTD" runat="server" CssClass="default" /></td>
    </tr>
</table>
<br />
<table width="700" cellpadding="2" cellspacing="2" border="0">
    <tr>
        <td class="bold">Host Information</td>
    </tr>
    <tr>
        <td>
            <table width="600" cellpadding="4" cellspacing="0" border="0" style="border:solid 1px #CCCCCC">
                <tr bgcolor="#EEEEEE">
                    <td nowrap><b>Host Name</b></td>
                    <td nowrap><b>IP Address(es)</b></td>
                    <td nowrap><b>Operating System</b></td>
                    <td nowrap><b>Operating System Version</b></td>
                </tr>
                <%=strHosts %>
                <tr>
                    <td colspan="4">
                        <asp:Label ID="lblHostNone" runat="server" CssClass="default" Visible="false" Text="<img src='/images/alert.gif' border='0' align='absmiddle'> There are no devices..." />
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
<br />
<table width="700" cellpadding="2" cellspacing="2" border="0">
    <tr>
        <td class="bold">Backup Requirements</td>
    </tr>
    <tr>
        <td>Date First Backup Required (regularly scheduled backups will continue following this date):</td>
    </tr>
    <tr>
        <td><img src="/images/green_arrow.gif" border="0" align="absmiddle" /> <asp:Label ID="lblFirstBackupDate" runat="server" CssClass="redbold" /></td>
    </tr>
</table>
<table width="700" cellpadding="2" cellspacing="2" border="0">
    <tr>
        <td nowrap>Timing/Frequency of Backups:</td>
        <td width="100%"><asp:Label ID="lblBackupFreq" runat="server" CssClass="default" /></td>
    </tr>
    <tr>
        <td nowrap>Start Time:</td>
        <td width="100%"><asp:Label ID="lblCSB" runat="server" CssClass="default" /></td>
    </tr>
    <tr>
        <td nowrap>Total Combined Disk Capacity (GB):</td>
        <td width="100%"><asp:Label ID="lblTCDC" runat="server" CssClass="default" /></td>
    </tr>
    <tr>
        <td nowrap>Current Combined Disk Utilized (GB):</td>
        <td width="100%"><asp:Label ID="lblCCDU" runat="server" CssClass="default" /></td>
    </tr>
    <tr>
        <td nowrap>Average Size of One Typical Data File:</td>
        <td width="100%"><asp:Label ID="lblAvgSize" runat="server" CssClass="default" /></td>
    </tr>
</table>
<br />
<table width="700" cellpadding="2" cellspacing="2" border="0">
    <tr>
        <td class="bold">Applications Requiring Special Handling</td>
    </tr>
    <tr>
        <td>
            <table width="600" cellpadding="4" cellspacing="0" border="0" style="border:solid 1px #CCCCCC">
                <tr bgcolor="#EEEEEE">
                    <td nowrap><b>Application Name</b></td>
                    <td nowrap><b>Use Backup Agent</b></td>
                    <td nowrap><b>DBA/Contact Name</b></td>
                </tr>
                <asp:repeater ID="rptApp" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td align="right"><%# DataBinder.Eval(Container.DataItem, "name") %></td>
                            <td align="right"><%# DataBinder.Eval(Container.DataItem, "agent") %></td>
                            <td align="right"><%# DataBinder.Eval(Container.DataItem, "contact") %></td>
                        </tr>
                    </ItemTemplate>
                </asp:repeater>
                <tr>
                    <td colspan="3">
                        <asp:Label ID="lblAppNone" runat="server" CssClass="default" Visible="false" Text="<img src='/images/alert.gif' border='0' align='absmiddle'> There are no applications..." />
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
<br />
<table width="700" cellpadding="2" cellspacing="2" border="0">
    <tr>
        <td class="bold">Backup Exclusions</td>
    </tr>
    <%=strExclusions %>
</table>
<br />
<table width="700" cellpadding="2" cellspacing="2" border="0">
    <tr>
        <td class="bold">Policy Exceptions</td>
    </tr>
    <tr>
        <td><asp:Label ID="lblPolicyExcl" runat="server" CssClass="redbold" /></td>
    </tr>
</table>
<br />
<table width="700" cellpadding="2" cellspacing="2" border="0">
    <tr>
        <td colspan="2" class="bold">Archival/Retention Requirements</td>
    </tr>
    <%=strRequirements %>
</table>
</asp:Content>
