<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="rr_virtual_workstation.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.rr_virtual_workstation" %>


<script type="text/javascript">
</script>
<table width="100%" border="0" cellSpacing="2" cellPadding="4" class="default">
    <tr>
        <td colspan="2">
            <table width="100%" cellpadding="4" cellspacing="0" border="0" style="border:solid 1px #999999" bgcolor="#f9f9f9">
                <tr>
                    <td nowrap><img src="/images/bigAlert.gif" border="0" align="absmiddle" /></td>
                    <td width="100%"><b>NOTE:</b> You cannot attach a physical device to a virtual workstation. If your initiative requires special hardware, do not choose a virtual workstation.</td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td nowrap>Nickname:</td>
        <td width="100%"><asp:TextBox ID="txtName" runat="server" CssClass="default" Width="200" MaxLength="50" /></td>
    </tr>
    <tr>
        <td nowrap>Location:<font class="required">&nbsp;*</font></td>
        <td width="100%"><asp:TextBox ID="txtParent" CssClass="lightdefault" runat="server" Text="" Width="400" ReadOnly="true" /></td>
    </tr>
    <tr>
        <td nowrap>Class:<font class="required">&nbsp;*</font></td>
        <td width="100%"><asp:DropDownList ID="ddlClass" CssClass="default" runat="server" Width="300" /></td>
    </tr>
    <tr>
        <td nowrap>Quantity:<font class="required">&nbsp;*</font></td>
        <td width="100%"><asp:TextBox ID="txtQuantity" runat="server" CssClass="default" Width="80" MaxLength="10" /></td>
    </tr>
    <tr>
        <td nowrap>RAM:<font class="required">&nbsp;*</font></td>
        <td width="100%"><asp:DropDownList ID="ddlRam" runat="server" CssClass="default" Width="300" /> GB</td>
    </tr>
    <tr>
        <td nowrap>Operating System:<font class="required">&nbsp;*</font></td>
        <td width="100%"><asp:DropDownList ID="ddlOS" runat="server" CssClass="default" Width="300" /></td>
    </tr>
    <tr>
        <td nowrap>CPUs:<font class="required">&nbsp;*</font></td>
        <td width="100%"><asp:DropDownList ID="ddlCPU" runat="server" CssClass="default" Width="300" /></td>
    </tr>
    <tr>
        <td nowrap>Hard Drive:<font class="required">&nbsp;*</font></td>
        <td width="100%"><asp:DropDownList ID="ddlHardDrive" runat="server" CssClass="default" Width="300" /> GB</td>
    </tr>
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
