<%@ Page Language="C#" MasterPageFile="~/frame.Master" AutoEventWireup="true" CodeBehind="service_editor_field_mappings.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.service_editor_field_mappings" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" Runat="Server">
<table width="100%" cellpadding="0" cellspacing="5" border="0">
    <tr>
        <td rowspan="2"><img src="/images/workflow.gif" border="0" align="absmiddle" /></td>
        <td class="header" width="100%" valign="bottom">Service Editor Field Mapping</td>
    </tr>
    <tr>
        <td width="100%" valign="top">Map the fields from one workflow service to another...</td>
    </tr>
</table>
<table width="100%" cellpadding="2" cellspacing="0" border="0">
    <tr id="trSaved" runat="server" visible="false">
        <td colspan="2" align="center" class="bigcheck"><img src='/images/bigCheck.gif' border='0' align='absmiddle' /> Field Mappings Saved</td>
    </tr>
    <tr>
        <td colspan="2">Please select the fields you want to be sent in the workflow service...</td>        
    </tr>
    <tr>
        <td colspan="2">
            <table id="tblChecks" width="100%" cellpadding="4" cellspacing="0" border="0" align="center" style="border:solid 1px #CCCCCC">
                <tr bgcolor="#EEEEEE">
                    <td><asp:CheckBox ID="chkInclude" href="include" runat="server" /><b>Include</b></td>
                    <td><asp:CheckBox ID="chkEditable" href="editable" runat="server" /><b>Editable</b></td>
                    <td><b>Question</b></td>
                    <td><b>Received From</b></td>
                </tr>
                <asp:repeater ID="rptView" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td valign="top"><asp:CheckBox ID="chkInclude" runat="server" href="include" CssClass="default" Checked='<%# (DataBinder.Eval(Container.DataItem, "editable").ToString() != "") %>' ToolTip='<%# DataBinder.Eval(Container.DataItem, "id") %>' /></td>
                            <td valign="top"><asp:CheckBox ID="chkEditable" runat="server" href="editable" CssClass="default" Checked='<%# (DataBinder.Eval(Container.DataItem, "editable").ToString() == "1") %>' ToolTip='<%# (DataBinder.Eval(Container.DataItem, "canedit").ToString() == "0" ? "Inherited as a READ-ONLY value from previous service..." : "") %>' Enabled='<%# (DataBinder.Eval(Container.DataItem, "canedit").ToString() == "1") %>' /></td>
                            <td valign="top"><%# DataBinder.Eval(Container.DataItem, "question") %></td>
                            <td valign="top"><asp:Label ID="lblService" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "serviceid") %>' /></td>
                        </tr>
                    </ItemTemplate>
                </asp:repeater>
                <tr>
                    <td colspan="4">
                        <asp:Label ID="lblNone" runat="server" CssClass="default" Visible="false" Text="<img src='/images/spacer.gif' border='0' height='1' width='25' /><img src='/images/alert.gif' border='0' align='absmiddle'> There are no fields" />
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr height="1">
        <td><asp:Button ID="btnSave" runat="server" CssClass="default" Width="75" Text="Save" OnClick="btnSave_Click" />
        <td align="right" nowrap><img src="/images/help24.gif" border="0" align="absmiddle" /> <a href="javascript:void(0);" onclick="alert('These fields are loaded from the workload manager of the previous service.\n\nFields are only editable if they are editable in the workload manager of the previous service.');">Where do these fields come from?</a></td>
    </tr>
</table>
</asp:Content>
