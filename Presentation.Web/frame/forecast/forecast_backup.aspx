<%@ Page Language="C#" MasterPageFile="~/frame.Master" AutoEventWireup="true" CodeBehind="forecast_backup.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.forecast_backup" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" Runat="Server">
<table width="100%" cellpadding="2" cellspacing="0">
    <tr>
        <td colspan="6" class="bold">Data flow diagram (All data in GB)</td>
    </tr>
    <tr>
        <td nowrap>TSM Client</td>
        <td></td>
        <td nowrap>TSM Server</td>
        <td></td>
        <td nowrap>Disk Pool</td>
        <td nowrap>Tape Pool</td>
    </tr>
    <tr>
        <td>
            <table>
                <tr>
                    <td rowspan="3"><img src="/images/client.gif" border="0" align="absmiddle" /></td>
                    <td nowrap>Full Backup</td>
                </tr>
                <tr>
                    <td nowrap>Incremental BU</td>
                </tr>
                <tr>
                    <td nowrap>Database data</td>
                </tr>
            </table>
            <br />
        </td>
        <td>
            <table>
                <tr>
                    <td><img src="/images/arrow1.gif" border="0" align="absmiddle" /></td>
                    <td><asp:Label ID="lblFB" runat="server" CssClass="default" /></td>
                </tr>
                <tr>
                    <td><img src="/images/arrow1.gif" border="0" align="absmiddle" /></td>
                    <td><asp:Label ID="lblIBU" runat="server" CssClass="default" /></td>
                </tr>
                <tr>
                    <td><img src="/images/arrow1.gif" border="0" align="absmiddle" /></td>
                    <td><asp:Label ID="lbldbdata" runat="server" CssClass="default" /></td>
                </tr>
            </table>
        </td>
        <td><img src="/images/server.gif" border="0" align="absmiddle" /></td>
        <td>
            <table>
                <tr>
                    <td><img src="/images/arrow1.gif" border="0" align="absmiddle" /></td>
                    <td><asp:Label ID="lblServerFB" runat="server" CssClass="default" /></td>
                    <td><img src="/images/arrow1.gif" border="0" align="absmiddle" /></td>
                </tr>
                <tr>
                    <td><img src="/images/arrow1.gif" border="0" align="absmiddle" /></td>
                    <td><asp:Label ID="lblServerIBU" runat="server" CssClass="default" /></td>
                    <td><img src="/images/arrow1.gif" border="0" align="absmiddle" /></td>
                </tr>
                <tr>
                    <td><img src="/images/arrow1.gif" border="0" align="absmiddle" /></td>
                    <td><asp:Label ID="lblServerDD" runat="server" CssClass="default" /></td>
                    <td><img src="/images/arrow1.gif" border="0" align="absmiddle" /></td>
                </tr>
            </table>
        </td>
        <td align="center">
            <table>
                <tr>
                    <td><img src="/images/storage.gif" border="0" align="absmiddle" /></td>
                    <td><img src="/images/arrow1.gif" border="0" align="absmiddle" /></td>
                    <td><asp:Label ID="lblDiskFB" runat="server" CssClass="default" /></td>
                </tr>
            </table>
        </td>
        <td><img src="/images/tape.gif" border="0" align="absmiddle" /></td>
    </tr>
</table>
</asp:Content>
