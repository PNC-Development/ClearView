<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="rr_virtual_workstation_account.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.rr_virtual_workstation_account" %>


<script type="text/javascript">
</script>
<table width="100%" border="0" cellSpacing="2" cellPadding="4" class="default">
    <tr>
        <td nowrap>Account:</td>
        <td width="100%">
            <table cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td><asp:TextBox ID="txtManager" runat="server" Width="300" CssClass="default" />&nbsp;&nbsp;<asp:LinkButton ID="btnManager" runat="server" CssClass="default" Text="Add a New User" /></td>
                </tr>
                <tr>
                    <td>
                        <div id="divManager" runat="server" style="overflow:hidden; position:absolute; display:none; background-color:#FFFFFF; border:solid 1px #CCCCCC">
                            <asp:ListBox ID="lstManager" runat="server" CssClass="default" />
                        </div>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td nowrap valign="top">Workstations:</td>
        <td width="100%"><asp:ListBox ID="lstWorkstations" runat="server" CssClass="default" Width="300" SelectionMode="Multiple" Rows="15" /></td>
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
<input type="hidden" id="hdnEnvironment" runat="server" />
<asp:HiddenField ID="hdnManager" runat="server" />
