<%@ Page Language="C#" MasterPageFile="~/frame.Master" AutoEventWireup="true" CodeBehind="service_editor_conditions.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.service_editor_conditions" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" Runat="Server">
<table width="100%" cellpadding="0" cellspacing="5" border="0">
    <tr>
        <td rowspan="2"><img src="/images/workflow.gif" border="0" align="absmiddle" /></td>
        <td class="header" width="100%" valign="bottom">Service Editor Conditional Workflow</td>
    </tr>
    <tr>
        <td width="100%" valign="top">Initiate a workflow based on a set of criteria in this service...</td>
    </tr>
</table>

<asp:Panel ID="panList" runat="server" Visible="false">
    <table width="100%" border="0" cellpadding="4" cellspacing="3">
        <tr id="trSaved" runat="server" visible="false">
            <td colspan="2" align="center" class="box_green"><img src='/images/bigCheckBox.gif' border='0' align='absmiddle' /> Saved</td>
        </tr>
        <tr>
            <td colspan="2">Select your condition for initiating this workflow:</td>
        </tr>
        <tr>
            <td nowrap><img src="/images/spacer.gif" border="0" width="<%=strIndent %>" height="1" /></td>
            <td width="100%">
                <asp:RadioButton ID="radConditionNeither" runat="server" Text="ALWAYS initiate this service (No conditional workflow)" CssClass="default" GroupName="radCondition" /><br />
                <asp:RadioButton ID="radConditionOnly" runat="server" Text="Initiate ONLY when the following condition sets are selected" CssClass="default" GroupName="radCondition" /><br />
                <asp:RadioButton ID="radConditionUnless" runat="server" Text="Always initiate UNLESS the following condition sets are selected" CssClass="default" GroupName="radCondition" />
            </td>
        </tr>
        <tr>
            <td colspan="2">How do you want to handle <b>subsequent</b> workflows, should <b>this</b> workflow NOT be initated?</td>
        </tr>
        <tr>
            <td nowrap><img src="/images/spacer.gif" border="0" width="<%=strIndent %>" height="1" /></td>
            <td width="100%">
                <asp:RadioButton ID="radContinueNo" runat="server" Text="Stop all subsequent workflows from initiating" CssClass="default" GroupName="radContinue" /><br />
                <asp:RadioButton ID="radContinueYes" runat="server" Text="Initiate all subsequent workflows (based off their criteria)" CssClass="default" GroupName="radContinue" />
            </td>
        </tr>
        <tr>
            <td nowrap><img src="/images/spacer.gif" border="0" width="<%=strIndent %>" height="1" /></td>
            <td width="100%"><asp:Button ID="btnSave" runat="server" CssClass="default" Width="75" Text="Save" OnClick="btnSave_Click" /></td>
        </tr>
        <tr>
            <td colspan="2">&nbsp;</td>
        </tr>
        <tr>
            <td colspan="2">
                <table width="100%" cellpadding="0" cellspacing="0" border="0">
                    <tr>
                        <td class="greentableheader">Condition Sets</td>
                        <td align="right"><asp:Button ID="btnNew" runat="server" CssClass="default" Width="75" Text="Add New" OnClick="btnNew_Click" /></td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td nowrap><img src="/images/spacer.gif" border="0" width="<%=strIndent %>" height="1" /></td>
            <td width="100%">
                <table width="100%" cellpadding="4" cellspacing="0" border="0" align="center" style="border:solid 1px #CCCCCC">
                    <tr bgcolor="#EEEEEE">
                        <td nowrap>&nbsp;</td>
                        <td nowrap><b>Enabled</b></td>
                        <td nowrap><b>Name</b></td>
                        <td nowrap><b># of Values</b></td>
                        <td nowrap>&nbsp;</td>
                    </tr>
                    <asp:Repeater ID="rptConditions" runat="server">
                        <ItemTemplate>
                            <tr>
                                <td valign="top">[<asp:LinkButton ID="btnEdit" runat="server" Text="Edit" OnClick="btnEdit_Click" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' />]</td>
                                <td valign="top" align="center"><img src='<%# DataBinder.Eval(Container.DataItem, "enabled").ToString() == "0" ? "/images/cancel.gif" : "/images/check.gif" %>' border='0' /></td>
                                <td valign="top" width="100%"><%# DataBinder.Eval(Container.DataItem, "name") %></td>
                                <td valign="top" align="center"><%# DataBinder.Eval(Container.DataItem, "conditions") %></td>
                                <td valign="top">[<asp:LinkButton ID="btnDelete" runat="server" Text="Delete" OnClick="btnDelete_Click" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' />]</td>
                            </tr>
                        </ItemTemplate>
                        <SeparatorTemplate>
                            <tr>
                                <td colspan="10" class="header" align="center">-- -- -- -- -- -- -- -- -- -- -- -- -- -- OR -- -- -- -- -- -- -- -- -- -- -- -- -- --</td>
                            </tr>
                        </SeparatorTemplate>
                    </asp:Repeater>
                    <tr>
                        <td colspan="10">
                            <asp:Label ID="lblNone" runat="server" CssClass="default" Visible="false" Text="<img src='/images/spacer.gif' border='0' height='1' width='25' /><img src='/images/alert.gif' border='0' align='absmiddle'> There are no condition sets" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Panel>

<asp:Panel ID="panCondition" runat="server" Visible="false">
    <table width="100%" border="0" cellpadding="4" cellspacing="3">
        <tr>
            <td colspan="2">Condition Set Name:</td>
        </tr>
        <tr>
            <td nowrap><img src="/images/spacer.gif" border="0" width="<%=strIndent %>" height="1" /></td>
            <td width="100%"><asp:TextBox ID="txtName" runat="server" Width="300" MaxLength="100" /></td>
        </tr>
        <tr>
            <td nowrap><img src="/images/spacer.gif" border="0" width="<%=strIndent %>" height="1" /></td>
            <td width="100%"><asp:CheckBox ID="chkEnabled" runat="server" Text="Enabled" Checked="true" /></td>
        </tr>
        <tr>
            <td colspan="2">&nbsp;</td>
        </tr>
        <tr>
            <td colspan="2" class="box_blue"><img src="/images/info.gif" border="0" align="absmiddle" /> <b>All responses</b> selected below must be selected by the client to enable this condition set.</td>
        </tr>
        <tr>
            <td colspan="2">Select all responses that make up this condition set:</td>
        </tr>
        <tr>
            <td nowrap><img src="/images/spacer.gif" border="0" width="<%=strIndent %>" height="1" /></td>
            <td width="100%"><asp:TreeView ID="treConditional" runat="server" ShowCheckBoxes="Leaf" ShowLines="true" NodeIndent="30" NodeWrap="true" /></td>
        </tr>
        <tr>
            <td nowrap><img src="/images/spacer.gif" border="0" width="<%=strIndent %>" height="1" /></td>
            <td>
                <asp:Button ID="btnAdd" runat="server" CssClass="default" Width="75" Text="Save" OnClick="btnAdd_Click" />
                <asp:Button ID="btnCancel" runat="server" CssClass="default" Width="75" Text="Cancel" OnClick="btnCancel_Click" />
            </td>
        </tr>
    </table>
</asp:Panel>
</asp:Content>
