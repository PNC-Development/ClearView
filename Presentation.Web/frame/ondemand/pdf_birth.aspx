<%@ Page Language="C#" MasterPageFile="~/frame.Master" AutoEventWireup="true" CodeBehind="pdf_birth.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.pdf_birth" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" runat="server">
<table width="100%" cellpadding="0" cellspacing="5" border="0">
    <tr>
        <td rowspan="2"><img src="/images/bigAlert.gif" border="0" align="absmiddle" /></td>
        <td class="header" width="100%" valign="bottom">Unable to Generate Form</td>
    </tr>
    <tr>
        <td width="100%" valign="top">The form was not generated due to one (or more) of the following reasons...</td>
    </tr>
    <tr>
        <td></td>
        <td width="100%">1.) Invalid Querystring - here are the available options...
            <ul>
                <li>&quot;id&quot; = the ID of the design</li>
                <li>&quot;prod&quot; = true to generate for prod, false to generate for test</li>
                <li>&quot;save&quot; = true to save a copy (not just render)</li>
                <li>&quot;add&quot; = true to add the generated form to the project (requires &quot;save&quot; to be set to true)</li>
                <li>&quot;send&quot; = true to send an email regarding the form (requires &quot;save&quot; to be set to true)</li>
            </ul>
        </td>
    </tr>
    <tr>
        <td></td>
        <td width="100%">2.) Invalid Design ID</td>
    </tr>
    <tr>
        <td></td>
        <td width="100%">3.) Based on the querystring values, there is no information available for the design</td>
    </tr>
</table>
</asp:Content>
