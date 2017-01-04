<%@ Page Language="C#" MasterPageFile="~/frame.Master" AutoEventWireup="true" CodeBehind="asset_import.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.asset_import" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" Runat="Server">
<table width="100%" cellpadding="0" cellspacing="5" border="0">
    <tr>
        <td rowspan="2"><img src="/images/step_1.gif" border="0" align="absmiddle" /></td>
        <td class="header" colspan="2" valign="bottom">Create an ODBC Connection</td>
    </tr>
    <tr>
        <td width="100%" colspan="2" valign="top">Use the following configuration to create an ODBC connection...</td>
    </tr>
    <tr>
        <td></td>
        <td nowrap>System DSN Name:</td>
        <td width="100%">AssetQuery</td>
    </tr>
    <tr>
        <td></td>
        <td nowrap>SQL Server:</td>
        <td width="100%">OHCLEIIS1319</td>
    </tr>
    <tr>
        <td></td>
        <td nowrap>SQL Username:</td>
        <td width="100%">cvasset</td>
    </tr>
    <tr>
        <td></td>
        <td nowrap>SQL Password:</td>
        <td width="100%">m00seass3t</td>
    </tr>
</table>
<table width="100%" cellpadding="0" cellspacing="5" border="0">
    <tr>
        <td colspan="3">&nbsp;</td>
    </tr>
    <tr>
        <td rowspan="2"><img src="/images/step_2.gif" border="0" align="absmiddle" /></td>
        <td class="header" colspan="2" valign="bottom">Download Template</td>
    </tr>
    <tr>
        <td width="100%" colspan="2" valign="top">Click on the following link to download the template</td>
    </tr>
    <tr>
        <td></td>
        <td colspan="2"><asp:LinkButton ID="btnTemplate" runat="server" Text="<img src='/images/excel.gif' border='0' align='absmiddle' /> Download Template" OnClick="btnTemplate_Click" /></td>
    </tr>
</table>
<table width="100%" cellpadding="0" cellspacing="5" border="0">
    <tr>
        <td colspan="3">&nbsp;</td>
    </tr>
    <tr>
        <td rowspan="2"><img src="/images/step_3.gif" border="0" align="absmiddle" /></td>
        <td class="header" colspan="2" valign="bottom">Upload Completed Template</td>
    </tr>
    <tr>
        <td width="100%" colspan="2" valign="top">Once finished, upload your template using the <b>Browse</b> and <b>Import</b> buttons on the page from which you just came.</td>
    </tr>
</table>
</asp:Content>
