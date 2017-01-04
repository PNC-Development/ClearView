<%@ Page Language="C#" MasterPageFile="~/window.Master" AutoEventWireup="true" CodeBehind="forecast_print_forecast.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.forecast_print_forecast" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" runat="server">
<table width="100%" cellpadding="4" cellspacing="3" border="0">
    <tr>
        <td colspan="2" align="center"><asp:LinkButton ID="btnExport" runat="server" Text="<img src='/images/export-icon.gif' border='0' align='absmiddle' /><img src='/images/spacer.gif' border='0' width='5' height='1' />Export to PDF" OnClick="btnExport_Click" /></td>
    </tr>
    <tr>
        <td nowrap>Show Questions and Answers:</td>
        <td width="100%">
            <table cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td><asp:CheckBox ID="chkQuestions" runat="server" CssClass="default" AutoPostBack="true" OnCheckedChanged="chkQuestions_Change" /></td>
                    <td class="bold">
                        <div id="divWait" runat="server" style="display:none">
                            <img src="/images/spacer.gif" border="0" width="20" height="1" /><img src="/images/please_wait.gif" border="0" align="absmiddle" /> Please Wait...
                        </div>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
<%=strQuestions %>
</asp:Content>
