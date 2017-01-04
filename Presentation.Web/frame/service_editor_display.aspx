<%@ Page Language="C#" MasterPageFile="~/frame.Master" AutoEventWireup="true" CodeBehind="service_editor_display.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.service_editor_display" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" Runat="Server">
<table width="100%" cellpadding="0" cellspacing="5" border="0">
    <tr>
        <td rowspan="2"><img src="/images/workflow.gif" border="0" align="absmiddle" /></td>
        <td class="header" width="100%" valign="bottom">Service Editor Display</td>
    </tr>
    <tr>
        <td width="100%" valign="top">Please select the response(s) which would make this question appear...</td>
    </tr>
</table>
<table width="100%" cellpadding="2" cellspacing="0" border="0">
    <tr id="trSaved" runat="server" visible="false">
        <td align="center" class="bigcheck"><img src='/images/bigCheck.gif' border='0' align='absmiddle' /> Field Mappings Saved</td>
    </tr>
    <tr>
        <td><b>NOTE:</b> Selecting a response will mean this question is HIDDEN when first loaded.  When a client selects one of the selected responses, this question will appear.</td>        
    </tr>
    <tr>
        <td>
            <table width="100%" cellpadding="4" cellspacing="0" border="0" align="center" style="border:solid 1px #CCCCCC">
                <tr bgcolor="#EEEEEE">
                    <td><b>Question</b></td>
                </tr>
                <tr>
                    <td>
                        <asp:TreeView ID="oTree" runat="server" CssClass="default" ShowCheckBoxes="Leaf" ShowLines="true" NodeIndent="30" NodeWrap="true">
                        </asp:TreeView>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td><asp:Button ID="btnSave" runat="server" CssClass="default" Width="75" Text="Save" OnClick="btnSave_Click" /></td>        
    </tr>
</table>
</asp:Content>
