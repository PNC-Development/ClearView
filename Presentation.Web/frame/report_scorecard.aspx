<%@ Page Language="C#" MasterPageFile="~/none.Master" AutoEventWireup="true" CodeBehind="report_scorecard.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.report_scorecard" Title="Untitled Page" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" runat="server">
    <asp:Panel ID="panShow" runat="server" Visible="false">
    <rsweb:ReportViewer ID="ReportViewer1" runat="server" width="100%" height="100%" ProcessingMode="Remote" ShowParameterPrompts="True" >
        <ServerReport />
    </rsweb:ReportViewer>
    </asp:Panel>
    <asp:Panel ID="panError" runat="server" Visible="false">
        <table width="100%" height="100%" cellpadding="0" cellspacing="0" border="0">
            <tr> 
                <td valign="middle" width="100%" height="100%" bgcolor="#FFFFFF">
                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                        <tr>
                            <td align="center" class="hugeheader"><img src="/images/bigError.gif" border="0" align="absmiddle" /> There was a problem loading the report...</td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
