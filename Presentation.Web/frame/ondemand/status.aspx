<%@ Page Language="C#" MasterPageFile="~/none.Master" AutoEventWireup="true" CodeBehind="status.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.status" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" runat="server">
    <table width="100%" height="100%" cellpadding="1" cellspacing="1" border="0">
        <tr height="1">
            <td>
                <table width="100%" cellpadding="0" cellspacing="5" border="0">
                    <tr>
                        <td rowspan="2"><img src="/images/wizard.gif" border="0" align="absmiddle" /></td>
                        <td class="header" width="100%" valign="bottom">Auto-Provisioning Status</td>
                    </tr>
                    <tr>
                        <td width="100%" valign="top">This page will show you the status of your device as it is auto-provisioned.</td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td><asp:PlaceHolder ID="PHStep" runat="server" /></td>
        </tr>
    </table>
<div id="divDisableCover_"></div>
</asp:Content>
