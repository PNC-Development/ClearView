<%@ Page Language="C#" MasterPageFile="~/frame.Master" AutoEventWireup="true" CodeBehind="service_editor_workflow_print.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.service_editor_workflow_print" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" Runat="Server">
<table width="100%" cellpadding="2" cellspacing="0" border="0">
    <tr>
        <td><img src="/images/help.gif" border="0" align="absmiddle" /> = Configured Question &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<img src="/images/hand_right.gif" border="0" align="absmiddle" /> = Inherited Question</td>
    </tr>
    <tr>
        <td>
            <table cellpadding="4" cellspacing="0" border="0">
                <%=strTable.ToString() %>
            </table>
        </td>
    </tr>
</table>
</asp:Content>
