<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="workstation_manager.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.workstation_manager" %>


<script type="text/javascript">
    function EnsurePool(oNone, oNew, oName, oDesc, oExisting, oDDL) {
        oNone = document.getElementById(oNone);
        oNew = document.getElementById(oNew);
        oExisting = document.getElementById(oExisting);
        if (oNone.checked == false && oNew.checked == false && oExisting.checked == false) {
            alert('Please select a workstation pooling option');
            return false;
        }
        else {
            if (oNew.checked == true) {
                if (ValidateText(oName, 'Please enter a name') == false || ValidateText(oDesc, 'Please enter a description') == false)
                    return false;
                else {
                    oName = document.getElementById(oName);
                    if (isAlphaNumeric(oName.value) == false) {
                        alert('Please enter a valid workstation pool name\n\nWorkstation pool names cannot contain any characters except numbers and letters\n(Not even spaces)');
                        return false;
                    }
                }
            }
            if (oExisting.checked == true) {
                return ValidateDropDown(oDDL, 'Please select an existing pool');
            }
        }
        return true;
    }
</script>
<table width="100%" height="100%" cellpadding="0" cellspacing="0" border="0">
    <tr>
        <td valign="top">
            <div style="height:100%; overflow:auto">
            <table width="100%" cellpadding="4" cellspacing="0" border="0">
                <tr>
                    <td colspan="2">Please enter the name of the manager that owns this workstation.</td>
                </tr>
                <tr>
                    <td><img src="/images/spacer.gif" border="0" width="40" height="1" /></td>
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
                    <td nowrap></td>
                    <td width="100%" class="footer">Please enter a first name, last name, or LAN ID and select from the list</td>
                </tr>
                <tr>
                    <td colspan="2">&nbsp;</td>
                </tr>
                <tr>
                    <td colspan="2">Please enter the cost center to be billed for this workstation.</td>
                </tr>
                <tr>
                    <td><img src="/images/spacer.gif" border="0" width="40" height="1" /></td>
                    <td width="100%">
                        <table cellpadding="0" cellspacing="0" border="0">
                            <tr>
                                <td><asp:TextBox ID="txtCostCenter" runat="server" Width="200" CssClass="default" /></td>
                            </tr>
                            <tr>
                                <td>
                                    <div id="divCostCenter" runat="server" style="overflow:hidden; position:absolute; display:none; background-color:#FFFFFF; border:solid 1px #CCCCCC">
                                        <asp:ListBox ID="lstCostCenter" runat="server" CssClass="default" />
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td class="footer">Start typing and a list will be presented (after 6 characters)</td>
                </tr>
                <tr>
                    <td colspan="2">&nbsp;</td>
                </tr>
                <asp:Panel ID="panPool" runat="server" Visible="false">
                <tr>
                    <td colspan="2">Please select your workstation pooling configuration...</td>
                </tr>
                <tr>
                    <td><img src="/images/spacer.gif" border="0" width="40" height="1" /></td>
                    <td width="100%">
                        <table cellpadding="2" cellspacing="1" border="0">
                            <tr>
                                <td><asp:RadioButton ID="radNone" runat="server" CssClass="default" Text="NO POOL - This workstation will <b>NOT</b> belong to a pool" GroupName="Pool" /></td>
                            </tr>
                            <tr>
                                <td><asp:RadioButton ID="radNew" runat="server" CssClass="default" Text="NEW POOL - This workstation will belong to a <b>NEW</b> pool" GroupName="Pool" Enabled="false" /></td>
                            </tr>
                            <tr>
                                <td><asp:RadioButton ID="radExisting" runat="server" CssClass="default" Text="EXISTING POOL - This workstation will belong to an <b>EXISTING</b> pool" GroupName="Pool" Enabled="false" /></td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">&nbsp;</td>
                </tr>
                </asp:Panel>
            </table>
            <div id="divNew" runat="server" style="display:none">
            <table width="100%" cellpadding="4" cellspacing="0" border="0">
                <tr>
                    <td colspan="2">Please enter the name of the pool</td>
                </tr>
                <tr>
                    <td><img src="/images/spacer.gif" border="0" width="40" height="1" /></td>
                    <td width="100%"><asp:TextBox ID="txtName" runat="server" CssClass="default" Width="300" MaxLength="30" /></td>
                </tr>
                <tr>
                    <td colspan="2">Please enter the description of the pool</td>
                </tr>
                <tr>
                    <td><img src="/images/spacer.gif" border="0" width="40" height="1" /></td>
                    <td width="100%"><asp:TextBox ID="txtDescription" runat="server" CssClass="default" Width="600" TextMode="MultiLine" Rows="6" /></td>
                </tr>
            </table>
            </div>
            <div id="divExisting" runat="server" style="display:none">
            <table width="100%" cellpadding="4" cellspacing="0" border="0">
                <tr>
                    <td colspan="2">Please select the pool</td>
                </tr>
                <tr>
                    <td><img src="/images/spacer.gif" border="0" width="40" height="1" /></td>
                    <td width="100%"><asp:DropDownList ID="ddlExisting" runat="server" CssClass="default" Width="300" /></td>
                </tr>
            </table>
            </div>
        </td>
    </tr>
    <tr height="1">
        <td>
            <table width="100%" cellpadding="4" cellspacing="2" border="0">
                <tr>
                    <td colspan="3"><hr size="1" noshade /></td>
                </tr>
                <tr>
                    <td class="required">* = Required Field</td>
                    <td align="center">
                        <asp:Panel ID="panNavigation" runat="server" Visible="false">
                            <asp:Button ID="btnBack" runat="server" OnClick="btnBack_Click" Text="Back" CssClass="default" Width="75" /> <asp:Button ID="btnNext" runat="server" OnClick="btnNext_Click" Text="Next" CssClass="default" Width="75" />
                        </asp:Panel>
                        <asp:Panel ID="panUpdate" runat="server" Visible="false">
                            <asp:Button ID="btnUpdate" runat="server" OnClick="btnUpdate_Click" Text="Update" CssClass="default" Width="75" /> <asp:Button ID="btnCancel" runat="server" OnClick="btnCancel_Click" Text="Cancel" CssClass="default" Width="75" />
                        </asp:Panel>
                    </td>
                    <td align="right"><asp:Button ID="btnClose" runat="server" Text="Finish Later" CssClass="default" Width="125" /></td>
                </tr>
            </table>
        </td>
    </tr>
</table>
<asp:HiddenField ID="hdnManager" runat="server" />
<asp:HiddenField ID="hdnCostCenter" runat="server" />
