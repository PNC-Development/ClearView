<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="rr_group_copy.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.rr_group_copy" %>


<script type="text/javascript">
    function EnsureGroupScope(o1,o2,o3) {
        o1 = document.getElementById(o1);
        o2 = document.getElementById(o2);
        o3 = document.getElementById(o3);
        if (o1.checked == false && o2.checked == false && o3.checked == false) {
            alert('Please select a group scope');
            o1.focus();
            return false;
        }
        return true;
    }
    function EnsureGroupType(o1,o2) {
        o1 = document.getElementById(o1);
        o2 = document.getElementById(o2);
        if (o1.checked == false && o2.checked == false) {
            alert('Please select a group type');
            o1.focus();
            return false;
        }
        return true;
    }
</script>
<table width="100%" border="0" cellSpacing="2" cellPadding="4" class="default">
    <tr>
        <td nowrap>Group name <b>from</b> which to copy:<font class="required">&nbsp;*</font></td>
        <td width="100%"><asp:TextBox ID="txtFrom" runat="server" CssClass="default" width="150" MaxLength="50" /></td>
    </tr>
    <tr>
        <td nowrap>Domain:<font class="required">&nbsp;*</font></td>
        <td width="100%"><asp:DropDownList ID="ddlDomain" runat="server" CssClass="default" Width="150" /></td>
    </tr>
    <tr>
        <td nowrap></td>
        <td width="100%"><asp:Button ID="btnContinue" runat="server" CssClass="default" Text="Load Properties" Width="125" OnClick="btnContinue_Click" /></td>
    </tr>
    <asp:Panel ID="panContinue" runat="server" Visible="false">
    <tr>
        <td colspan="2"><span style="width:100%;border-bottom:1 dotted #CCCCCC;"/></td>
    </tr>
    <tr>
        <td nowrap>Group name:</td>
        <td width="100%"><asp:label ID="lblGroup" CssClass="default" runat="server" /></td>
    </tr>
    <tr>
        <td nowrap>Group name <b>to</b> which to copy:<font class="required">&nbsp;*</font></td>
        <td width="100%"><asp:TextBox ID="txtTo" runat="server" CssClass="default" width="150" MaxLength="50" /></td>
    </tr>
    <tr>
        <td nowrap>Group Scope:<font class="required">&nbsp;*</font></td>
        <td width="100%">
            <asp:RadioButton ID="radDLG" runat="server" CssClass="default" Text="Domain Local" GroupName="scope" /> 
            <asp:RadioButton ID="radGG" runat="server" CssClass="default" Text="Global" GroupName="scope" /> 
            <asp:RadioButton ID="radUG" runat="server" CssClass="default" Text="Universal" GroupName="scope" /> 
        </td>
    </tr>
    <tr>
        <td nowrap>Group Type:<font class="required">&nbsp;*</font></td>
        <td width="100%">
            <asp:RadioButton ID="radS" runat="server" CssClass="default" Text="Security" GroupName="type" /> 
            <asp:RadioButton ID="radD" runat="server" CssClass="default" Text="Distribution" GroupName="type" /> 
        </td>
    </tr>
    <tr>
        <td colspan="2">Please select the user(s) you want to add to this group:</td>
    </tr>
    <tr>
        <td colspan="2">
            <asp:CheckBoxList ID="chkUsers" runat="server" CssClass="default" RepeatDirection="Vertical" RepeatColumns="2" CellPadding="3" CellSpacing="2" />
            <asp:Label ID="lblNone" runat="server" CssClass="default" Visible="false" Text="<img src='/images/alert.gif' border='0' align='absmiddle' /> There are no users" />
        </td>
    </tr>
    </asp:Panel>
    <tr>
        <td colspan="2"><hr size="1" noshade /></td>
    </tr>
    <tr>
        <td colspan="2">
            <table width="100%" cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td>
                        <table cellpadding="4" cellspacing="4" border="0">
                            <tr>
                                <td><asp:Button ID="btnBack" runat="server" OnClick="btnBack_Click" CssClass="default" Text="<<  Back" Width="100" /></td>
                                <td>
                                    <asp:Panel ID="panDeliverable" runat="server" Visible="false">
                                        <asp:Button ID="btnDeliverable" runat="server" CssClass="default" Text="Service Deliverables" Width="150" />
                                    </asp:Panel>
                                </td>
                                <td><asp:Button ID="btnNext" runat="server" OnClick="btnNext_Click" CssClass="default" Text="Next  >>" Width="100" /></td>
                            </tr>
                        </table>
                    </td>
                    <td align="right">
                        <table cellpadding="4" cellspacing="4" border="0">
                            <tr>
                                <td><asp:Button ID="btnCancel1" runat="server" OnClick="btnCancel_Click" CssClass="default" Text="Cancel Request" Width="125" /></td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
<asp:Label ID="lblItem" runat="server" Visible="false" />
<asp:Label ID="lblNumber" runat="server" Visible="false" />
<input type="hidden" id="hdnParent" runat="server" />