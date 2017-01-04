<%@ Page Language="C#" MasterPageFile="~/window.Master" AutoEventWireup="True" CodeBehind="design.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.design_wizard" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" runat="server">
<script type="text/javascript" src="/javascript/design.js"></script>
<script type="text/javascript">
    function CheckRemoteDesktop(oDDL, oCheck) {
        oCheck = document.getElementById(oCheck);
        if (oDDL.options[oDDL.selectedIndex].value == "U") {
            oCheck.checked = false;
            oCheck.disabled = true;
        }
        else
            oCheck.disabled = false;
    }
    function ShowBuildDivs(oDivShow, oDivHide1, oDivHide2, oDivHide3) {
        oDivShow = document.getElementById(oDivShow);
        oDivShow.style.display = "inline";
        oDivHide1 = document.getElementById(oDivHide1);
        oDivHide1.style.display = "none";
        oDivHide2 = document.getElementById(oDivHide2);
        oDivHide2.style.display = "none";
        oDivHide3 = document.getElementById(oDivHide3);
        oDivHide3.style.display = "none";
    }
    function HideDemo() {
        var demo = '<%=boolDemo %>';
        if (demo == 'True') {
            //alert('here');
            var _dr = document.getElementById("DIV57");
            if (_dr != null)
                _dr.style.display = "none";
            var _location = document.getElementById("DIV59");
            if (_location != null)
                _location.style.display = "none";
            var _sve1 = document.getElementById("DIV54");
            if (_sve1 != null)
                _sve1.style.display = "none";
            var _sve2 = document.getElementById("DIV55");
            if (_sve2 != null)
                _sve2.style.display = "none";
            var _db = document.getElementById("TBL5");
            if (_db != null)
                _db.children.item(0).children.item(2).style.display = "none";   // 3rd response of the Database question = Other
            //alert('done');
        }
        else {
            var _eng = document.getElementById("DDL3");
            if (_eng != null && _eng.options[1].text == "Engineering") {
                _eng.options[1] = null;   // Remove Engineering Class
            }
            var _model = document.getElementById("TBL16");
            if (_model != null) {
                _model.children.item(0).children.item(5).style.display = "none";   // 6th response of the Special Hardware question = Demo SAN
                _model.children.item(0).children.item(6).style.display = "none";   // 6th response of the Special Hardware question = Demo Local
            }
        }
    }
    function ConfirmDR(chk, hrs) {
        if (chk.checked)
            return confirm('Disclaimer:\n\nClearView HIGHLY advises that the project does not alter the DR functionality that is mandated by the predefined resiliency rating that is specified in the Mnemonic Management System (MMS).\n\nDoing so, will result in unexpected or degraded recovery services in an event of a disaster.\n\nBy clicking "OK", you fully understand the risks associated and are taking full responsibility for your decision.');
        else
            return true;
    }
</script>
<table width="100%" height="100%" cellpadding="0" cellspacing="0" border="0">
    <tr height="1">
        <td>
            <table width="100%" cellpadding="0" cellspacing="5" border="0">
                <tr>
                    <td rowspan="2"><img src="/images/forecast2.gif" border="0" align="absmiddle" /></td>
                    <td class="header" width="100%" valign="bottom">ClearView Design Builder</td>
                </tr>
                <tr>
                    <td width="100%" valign="top">Use this module to complete your application requirements and submit your design for automated provisioning.</td>
                </tr>
            </table>
        </td>
        <td><asp:LinkButton ID="lblProcedureNumber" runat="server" CssClass="header" OnClick="lnkRefresh_Click" /></td>
    </tr>
    <tr height="1">
        <td colspan="2"><img src="/images/spacer.gif" border="0" width="1" height="1" /></td>
    </tr>
    <tr>
        <td colspan="2" valign="top">
            <asp:Panel ID="panRequirements" runat="server" Visible="false">
                <table height="100%" width="100%" cellpadding="0" cellspacing="5" border="0">
                    <tr>
                        <td valign="top"><img src="/images/spacer.gif" border="0" height="1" width="5" /></td>
                        <td valign="top" width="300">
                            <table width="100%" height="100%" cellpadding="0" cellspacing="0" border="0">
                                <tr height="1">
                                    <td><img src="/images/table_topleft.gif" border="0" width="5" height="26"></td>
                                    <td nowrap background="/images/table_top.gif" class="greentableheader" width="100%">Design Requirements<asp:Label ID="lblID" runat="server" /></td>
                                    <td><img src="/images/table_topright.gif" border="0" width="5" height="26"></td>
                                </tr>
                                <tr>
                                    <td background="/images/table_left.gif"><img src="/images/table_left.gif" width="5" height="10"></td>
                                    <td width="100%" bgcolor="#FFFFFF" valign="top">
                                        <asp:Table ID="tblNavigation" runat="server" Width="100%" CellPadding="4" CellSpacing="0" BackColor="#f9f9f9" />
                                    </td>
                                    <td background="/images/table_right.gif"><img src="/images/table_right.gif" width="5" height="10"></td>
                                </tr>
                                <tr height="1">
                                    <td><img src="/images/table_bottomLeft.gif" border="0" width="5" height="9"></td>
                                    <td width="100%" background="/images/table_bottom.gif"></td>
                                    <td><img src="/images/table_bottomRight.gif" border="0" width="5" height="9"></td>
                                </tr>
                            </table>
                        </td>
                        <td valign="top">
                            <table width="100%" height="100%" cellpadding="0" cellspacing="0" border="0">
                                <tr>
                                    <td>
                                        <div style="height:100%; overflow:auto">
                                            <table width="95%" cellpadding="4" cellspacing="0" border="0">
                                                <tr>
                                                    <td class="biggestbold"><asp:Label ID="lblDescription" runat="server" CssClass="biggestbold" /></td>
                                                    <td align="right" width="125"><img src="/images/help24.gif" border="0" align="absmiddle" /> <asp:LinkButton ID="btnHelp" runat="server" Text="Show Help" /></td>
                                                </tr>
                                            </table>

                                            <!-- MNEMONIC -->
                                            <asp:Panel ID="panMnemonic" runat="server" Visible="false">
                                                <br />
                                                <table cellpadding="4" cellspacing="0" border="0">
                                                    <tr>
                                                        <td colspan="2"><asp:Label ID="lblQuestionMnemonic" runat="server" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td><img src="/images/spacer.gif" border="0" height="1" width='<%=strSpacer %>' /></td>
                                                        <td width="100%">
                                                            <table cellpadding="0" cellspacing="0" border="0">
                                                                <tr>
                                                                    <td><asp:TextBox ID="txtMnemonic" runat="server" Width="300" CssClass="default" /></td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <div id="divMnemonic" runat="server" style="overflow:hidden; position:absolute; display:none; background-color:#FFFFFF; border:solid 1px #CCCCCC">
                                                                            <asp:ListBox ID="lstMnemonic" runat="server" CssClass="default" />
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>

                                            <!-- COST CENTER -->
                                            <asp:Panel ID="panCostCenter" runat="server" Visible="false">
                                                <br />
                                                <table cellpadding="4" cellspacing="0" border="0">
                                                    <tr>
                                                        <td colspan="2"><asp:Label ID="lblQuestionCostCenter" runat="server" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td><img src="/images/spacer.gif" border="0" height="1" width='<%=strSpacer %>' /></td>
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
                                                </table>
                                            </asp:Panel>
                                            
                                            <!-- SI -->
                                            <asp:Panel ID="panSI" runat="server" Visible="false">
                                                <br />
                                                <table cellpadding="4" cellspacing="0" border="0">
                                                    <tr>
                                                        <td colspan="2"><asp:Label ID="lblQuestionSI" runat="server" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td><img src="/images/spacer.gif" border="0" height="1" width='<%=strSpacer %>' /></td>
                                                        <td width="100%">
                                                            <table cellpadding="0" cellspacing="0" border="0">
                                                                <tr>
                                                                    <td><asp:TextBox ID="txtSI" runat="server" Width="300" CssClass="default" /></td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <div id="divSI" runat="server" style="overflow:hidden; position:absolute; display:none; background-color:#FFFFFF; border:solid 1px #CCCCCC">
                                                                            <asp:ListBox ID="lstSI" runat="server" CssClass="default" />
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                            
                                            <!-- DTG -->
                                            <asp:Panel ID="panDTG" runat="server" Visible="false">
                                                <br />
                                                <table cellpadding="4" cellspacing="0" border="0">
                                                    <tr>
                                                        <td colspan="2"><asp:Label ID="lblQuestionDTG" runat="server" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td><img src="/images/spacer.gif" border="0" height="1" width='<%=strSpacer %>' /></td>
                                                        <td width="100%">
                                                            <table cellpadding="0" cellspacing="0" border="0">
                                                                <tr>
                                                                    <td><asp:TextBox ID="txtDTG" runat="server" Width="300" CssClass="default" /></td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <div id="divDTG" runat="server" style="overflow:hidden; position:absolute; display:none; background-color:#FFFFFF; border:solid 1px #CCCCCC">
                                                                            <asp:ListBox ID="lstDTG" runat="server" CssClass="default" />
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                            
                                            <!-- Backup Exclusions -->
                                            <asp:Panel ID="panBackupExclusions" runat="server" Visible="false">
                                                <br />
                                                <table cellpadding="4" cellspacing="0" border="0">
                                                    <tr>
                                                        <td colspan="2"><asp:Label ID="lblQuestionBackupExclusions" runat="server" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td nowrap>Path:</td>
                                                        <td width="100%"><asp:TextBox ID="txtPath" runat="server" Width="300" CssClass="default" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td></td>
                                                        <td nowrap><asp:Button ID="btnAddExclusion" runat="server" CssClass="default" Width="125" OnClick="btnAddExclusion_Click" Text="Add Exclusion" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <table width="400" cellpadding="4" cellspacing="0" border="0" style="border:solid 1px #CCCCCC">
                                                                <tr bgcolor="#EEEEEE">
                                                                    <td><b><u>Path:</u></b></td>
                                                                    <td></td>
                                                                </tr>
                                                                <asp:repeater ID="rptExclusions" runat="server">
                                                                    <ItemTemplate>
                                                                        <tr>
                                                                            <td valign="top" width="100%"><%# DataBinder.Eval(Container.DataItem, "path") %></td>
                                                                            <td valign="top" align="right"><asp:LinkButton ID="btnDeleteExclusion" runat="server" Text="Delete" OnClick="btnDeleteExclusion_Click" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' /></td>
                                                                        </tr>
                                                                    </ItemTemplate>
                                                                    <AlternatingItemTemplate>
                                                                        <tr bgcolor="F6F6F6">
                                                                            <td valign="top" width="100%"><%# DataBinder.Eval(Container.DataItem, "path") %></td>
                                                                            <td valign="top" align="right"><asp:LinkButton ID="btnDeleteExclusion" runat="server" Text="Delete" OnClick="btnDeleteExclusion_Click" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' /></td>
                                                                        </tr>
                                                                    </AlternatingItemTemplate>
                                                                </asp:repeater>
                                                                <tr>
                                                                    <td colspan="2">
                                                                        <asp:Label ID="lblExclusion" runat="server" CssClass="default" Visible="false" Text="<img src='/images/spacer.gif' border='0' height='1' width='25' /><img src='/images/alert.gif' border='0' align='absmiddle'> There are no backup exclusions" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                            
                                            <!-- Grid / Maintenance -->
                                            <asp:Panel ID="panGridMaintenance" runat="server" Visible="false">
                                                <br />
                                                <table cellpadding="4" cellspacing="0" border="0">
                                                    <tr>
                                                        <td colspan="2"><asp:Label ID="lblQuestionGridMaintenance" runat="server" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td><img src="/images/spacer.gif" border="0" height="1" width='<%=strSpacer %>' /></td>
                                                        <td>
                                                            <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                                                <tr>
                                                                    <td valign="top" nowarp><b>Pre-Configured Options:</b></td>
                                                                </tr>
                                                                <tr>
                                                                    <td valign="top" nowarp>
                                                                        <asp:DropDownList ID="ddlGridMaintenance" runat="server">
                                                                            <asp:ListItem Value="0" Text="-- MANUAL --" />
                                                                            <asp:ListItem Value="S" Text="Standard Maintenance Window" />
                                                                            <asp:ListItem Value="E" Text="Evening Only" />
                                                                            <asp:ListItem Value="M" Text="Early Morning" />
                                                                            <asp:ListItem Value="D" Text="During the Business Day" />
                                                                            <asp:ListItem Value="W" Text="Weekends Only" />
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">&nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td><img src="/images/spacer.gif" border="0" height="1" width='<%=strSpacer %>' /></td>
                                                        <td width="100%">
                                                            <%=strGridMaintenanceTable %>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                            
                                            <!-- Accounts -->
                                            <asp:Panel ID="panAccounts" runat="server" Visible="false">
                                                <br />
                                                <table cellpadding="4" cellspacing="0" border="0">
                                                    <tr>
                                                        <td colspan="2"><asp:Label ID="lblQuestionAccounts" runat="server" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td><img src="/images/spacer.gif" border="0" height="1" width="5" /></td>
                                                        <td width="100%">
                                                            <table width="100%" height="100%" cellpadding="0" cellspacing="0" border="0">
                                                                <tr height="1">
                                                                    <td><img src="/images/table_topleft.gif" border="0" width="5" height="26"></td>
                                                                    <td nowrap background="/images/table_top.gif" class="greentableheader" width="100%">New Account</td>
                                                                    <td><img src="/images/table_topright.gif" border="0" width="5" height="26"></td>
                                                                </tr>
                                                                <tr>
                                                                    <td background="/images/table_left.gif"><img src="/images/table_left.gif" width="5" height="10"></td>
                                                                    <td width="100%" bgcolor="#FFFFFF" valign="top">
                                                                        <table width="100%" border="0" cellspacing="4" cellpadding="4">
                                                                            <tr>
                                                                                <td nowrap>User:</td>
                                                                                <td width="100%">
                                                                                    <table cellpadding="0" cellspacing="0" border="0">
                                                                                        <tr>
                                                                                            <td><asp:TextBox ID="txtAccount" runat="server" Width="300" CssClass="default" /></td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td>
                                                                                                <div id="divAccount" runat="server" style="overflow:hidden; position:absolute; display:none; background-color:#FFFFFF; border:solid 1px #CCCCCC">
                                                                                                    <asp:ListBox ID="lstAccount" runat="server" CssClass="default" />
                                                                                                </div>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td nowrap class="footer">&nbsp;</td>
                                                                                <td class="footer">(Enter a valid LAN ID, First Name, or Last Name)</td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td></td>
                                                                                <td nowrap><asp:LinkButton ID="btnManager" runat="server" Text="User Not Appearing in the List? Click Here." /></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>Permission:</td>
                                                                                <td nowrap>
                                                                                    <asp:DropDownList ID="ddlPermissionBAK" runat="server" Visible="false">
                                                                                        <asp:ListItem Value="0" Text="-- SELECT --" />
                                                                                        <asp:ListItem Value="D" Text="Developer" />
                                                                                        <asp:ListItem Value="P" Text="Promoter" />
                                                                                        <asp:ListItem Value="S" Text="AppSupport" />
                                                                                        <asp:ListItem Value="U" Text="AppUsers" />
                                                                                    </asp:DropDownList>&nbsp;&nbsp;&nbsp;
                                                                                    <asp:DropDownList ID="ddlPermission" runat="server">
                                                                                        <asp:ListItem Value="0" Text="-- SELECT --" />
                                                                                        <asp:ListItem Value="P" Text="Promoter" />
                                                                                        <asp:ListItem Value="U" Text="AppUsers" />
                                                                                    </asp:DropDownList>&nbsp;&nbsp;&nbsp;
                                                                                    <asp:CheckBox ID="chkRemoteDesktop" runat="server" Text="With Remote Desktop" Visible="false" />
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td></td>
                                                                                <td nowrap></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td></td>
                                                                                <td nowrap><asp:Button ID="btnAddAccount" runat="server" CssClass="default" Width="125" OnClick="btnAddAccount_Click" Text="Add Account" /></td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                    <td background="/images/table_right.gif"><img src="/images/table_right.gif" width="5" height="10"></td>
                                                                </tr>
                                                                <tr height="1">
                                                                    <td><img src="/images/table_bottomLeft.gif" border="0" width="5" height="9"></td>
                                                                    <td width="100%" background="/images/table_bottom.gif"></td>
                                                                    <td><img src="/images/table_bottomRight.gif" border="0" width="5" height="9"></td>
                                                                </tr>
                                                            </table>
                                                            <br />
                                                            <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                                                <tr>
                                                                    <td><img src="/images/table_topleft.gif" border="0" width="5" height="26"></td>
                                                                    <td nowrap background="/images/table_top.gif" class="greentableheader" width="100%">Current Account Requests</td>
                                                                    <td><img src="/images/table_topright.gif" border="0" width="5" height="26"></td>
                                                                </tr>
                                                                <tr>
                                                                    <td background="/images/table_left.gif"><img src="/images/table_left.gif" width="5" height="10"></td>
                                                                    <td width="100%" bgcolor="#FFFFFF">
                                                                        <div style="height:100%; width:100%; overflow:auto">
                                                                            <table width="100%" cellpadding="4" cellspacing="0" border="0" align="center">
                                                                                <tr>
                                                                                    <td><b><u>User:</u></b></td>
                                                                                    <td><b><u>Permission:</u></b></td>
                                                                                    <td></td>
                                                                                </tr>
                                                                                <asp:repeater ID="rptAccounts" runat="server">
                                                                                    <ItemTemplate>
                                                                                        <tr>
                                                                                            <td valign="top"><%# oUser.GetFullName(Int32.Parse(DataBinder.Eval(Container.DataItem, "userid").ToString())) %> (<%# oUser.GetName(Int32.Parse(DataBinder.Eval(Container.DataItem, "userid").ToString()))%>)</td>
                                                                                            <td valign="top"><asp:Label ID="lblPermissions" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "access") %>' ToolTip='<%# DataBinder.Eval(Container.DataItem, "remote") %>' /></td>
                                                                                            <td valign="top" align="right"><asp:LinkButton ID="btnDeleteAccount" runat="server" Text="Delete" OnClick="btnDeleteAccount_Click" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' /></td>
                                                                                        </tr>
                                                                                    </ItemTemplate>
                                                                                    <AlternatingItemTemplate>
                                                                                        <tr bgcolor="F6F6F6">
                                                                                            <td valign="top"><%# oUser.GetFullName(Int32.Parse(DataBinder.Eval(Container.DataItem, "userid").ToString()))%> (<%# oUser.GetName(Int32.Parse(DataBinder.Eval(Container.DataItem, "userid").ToString()))%>)</td>
                                                                                            <td valign="top"><asp:Label ID="lblPermissions" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "access") %>' ToolTip='<%# DataBinder.Eval(Container.DataItem, "remote") %>' /></td>
                                                                                            <td valign="top" align="right"><asp:LinkButton ID="btnDeleteAccount" runat="server" Text="Delete" OnClick="btnDeleteAccount_Click" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' /></td>
                                                                                        </tr>
                                                                                    </AlternatingItemTemplate>
                                                                                </asp:repeater>
                                                                                <tr>
                                                                                    <td colspan="2">
                                                                                        <asp:Label ID="lblNone" runat="server" CssClass="default" Visible="false" Text="<img src='/images/spacer.gif' border='0' height='1' width='25' /><img src='/images/alert.gif' border='0' align='absmiddle'> You have not added any accounts to this request" />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </div>
                                                                    </td>
                                                                    <td background="/images/table_right.gif"><img src="/images/table_right.gif" width="5" height="10"></td>
                                                                </tr>
                                                                <tr>
                                                                    <td><img src="/images/table_bottomLeft.gif" border="0" width="5" height="9"></td>
                                                                    <td width="100%" background="/images/table_bottom.gif"></td>
                                                                    <td><img src="/images/table_bottomRight.gif" border="0" width="5" height="9"></td>
                                                                </tr>
                                                            </table>
                                                            <asp:CheckBox ID="chkNotify" runat="server" CssClass="default" Text="Email me the results of this account request" Checked="true" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                            
                                            <!-- Date -->
                                            <asp:Panel ID="panDate" runat="server" Visible="false">
                                                <br />
                                                <table cellpadding="4" cellspacing="0" border="0">
                                                    <tr>
                                                        <td colspan="2"><asp:Label ID="lblQuestionDate" runat="server" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td><img src="/images/spacer.gif" border="0" height="1" width='<%=strSpacer %>' /></td>
                                                        <td width="100%"><asp:TextBox ID="txtDate" runat="server" Width="80" MaxLength="10" /> <asp:ImageButton ID="imgDate" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calendar.gif" /></td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                            
                                            <!-- Location -->
                                            <asp:Panel ID="panLocation" runat="server" Visible="false">
                                                <br />
                                                <table cellpadding="4" cellspacing="0" border="0">
                                                    <tr>
                                                        <td colspan="2"><asp:Label ID="lblQuestionLocation" runat="server" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td><img src="/images/spacer.gif" border="0" height="1" width='<%=strSpacer %>' /></td>
                                                        <td width="100%"><%=strLocation %></td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                            
                                            
                                            
                                            
                                            
                                            
                                            <!-- QUESTIONS / RESPONSES -->
                                            <%=strQuestions %>
                                            <%=strHidden %>
                                            




                                            <!-- Grid / Backup -->
                                            <asp:Panel ID="panGridBackup" runat="server" Visible="false">
                                                <br />
                                                <asp:LinkButton ID="btnBackup" runat="server" Text="Don't need a backup? Click here..." />
                                                
                                                <br />
                                                <div id="divBackup" runat="server" style="display:none">
                                                    <br />
                                                    <table cellpadding="0" cellspacing="5" border="0" style="border:solid 1px #999999" bgcolor="#f9f9f9">
                                                        <tr>
                                                            <td rowspan="3" valign="top"><img src="/images/bigInfo.gif" border="0" align="absmiddle" /></td>
                                                            <td width="100%" class="header" valign="bottom">Backup Waiver Needed</td>
                                                        </tr>
                                                        <tr>
                                                            <td valign="top">Per PNC policy, every new server must be configured for backup.  However, once you receive the birth certificate, you can submit a service request to remove the backup.  This service is called &quot;WAIVER Request for Existing Servers&quot; and is available within the ClearView Service Request module.</td>
                                                        </tr>
                                                    </table>
                                                </div>
                                                
                                                <br />
                                                <table cellpadding="4" cellspacing="0" border="0">
                                                    <tr>
                                                        <td colspan="2"><asp:Label ID="lblQuestionGridBackup" runat="server" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td><img src="/images/spacer.gif" border="0" height="1" width='<%=strSpacer %>' /></td>
                                                        <td>
                                                            <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                                                <tr>
                                                                    <td valign="top" nowarp><b>Pre-Configured Options:</b></td>
                                                                </tr>
                                                                <tr>
                                                                    <td valign="top" nowarp>
                                                                        <asp:DropDownList ID="ddlGridBackup" runat="server">
                                                                            <asp:ListItem Value="0" Text="-- MANUAL --" />
                                                                            <asp:ListItem Value="S" Text="Standard Backup Window" />
                                                                            <asp:ListItem Value="E" Text="Evening Only" />
                                                                            <asp:ListItem Value="M" Text="Early Morning" />
                                                                            <asp:ListItem Value="D" Text="During the Business Day" />
                                                                            <asp:ListItem Value="W" Text="Weekends Only" />
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">&nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td><img src="/images/spacer.gif" border="0" height="1" width='<%=strSpacer %>' /></td>
                                                        <td width="100%">
                                                            <%=strGridBackupTable %>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                            
                                            
                                            <!-- Storage Luns -->
                                            <div id='DIV<%=intStorageQuestion %>' style='display:<%=strStorageDisplay %>'>
                                            <asp:Panel ID="panStorageLuns" runat="server" Visible="false">
                                                <br />
                                                <table cellpadding="4" cellspacing="0" border="0">
                                                    <tr>
                                                        <td colspan="2"><asp:Label ID="lblQuestionStorageLuns" runat="server" /></td>
                                                    </tr>
                                                </table>
                                                <asp:Panel ID="panStorageDatabase" runat="server" Visible="false">
                                                    <table width="100%" cellpadding="4" cellspacing="0" border="0">
                                                        <tr>
                                                            <td colspan="2" class="biggerbold">SQL Server Storage Calculator</td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2">Enter the total database size:</td>
                                                        </tr>
                                                        <tr>
                                                            <td><img src="/images/spacer.gif" border="0" width="30" height="1" /></td>
                                                            <td width="100%"><asp:TextBox ID="txtDatabaseSize" runat="server" CssClass="default" Width="100" MaxLength="10" /> GB</td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2">What % of the total space is the largest Table and/or Index:</td>
                                                        </tr>
                                                        <tr>
                                                            <td><img src="/images/spacer.gif" border="0" width="30" height="1" /></td>
                                                            <td width="100%"><asp:TextBox ID="txtDatabasePercent" runat="server" CssClass="default" Width="100" MaxLength="10" /> %&nbsp;&nbsp;&nbsp;<span class="footer">(0 - 100)</span></td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2">Are you using a non-standard TempDB size?</td>
                                                        </tr>
                                                        <tr>
                                                            <td><img src="/images/spacer.gif" border="0" width="30" height="1" /></td>
                                                            <td width="100%">
                                                                <asp:RadioButton ID="radTempDBYes" runat="server" CssClass="default" Text="Yes" GroupName="TempDB" /> 
                                                                <asp:RadioButton ID="radTempDBNo" runat="server" CssClass="default" Text="No (Default)" GroupName="TempDB" /> 
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <div ID="divTempDB" runat="server" style="display:none">
                                                        <table width="100%" cellpadding="4" cellspacing="0" border="0">
                                                            <tr>
                                                                <td colspan="2">Enter your custom TempDB size:</td>
                                                            </tr>
                                                            <tr>
                                                                <td><img src="/images/spacer.gif" border="0" width="30" height="1" /></td>
                                                                <td width="100%"><asp:TextBox ID="txtDatabaseTemp" runat="server" CssClass="default" Width="100" MaxLength="10" /> GB</td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                    <table width="100%" cellpadding="4" cellspacing="0" border="0">
                                                        <tr>
                                                            <td colspan="2">Will you require to store non-database data on the same instance?</td>
                                                        </tr>
                                                        <tr>
                                                            <td><img src="/images/spacer.gif" border="0" width="30" height="1" /></td>
                                                            <td width="100%">
                                                                <asp:RadioButton ID="radDatabaseNonYes" runat="server" CssClass="default" Text="Yes" GroupName="nonPNC" /> 
                                                                <asp:RadioButton ID="radDatabaseNonNo" runat="server" CssClass="default" Text="No (Default)" GroupName="nonPNC" /> 
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <div ID="divDatabaseNon" runat="server" style="display:none">
                                                        <table width="100%" cellpadding="4" cellspacing="0" border="0">
                                                            <tr>
                                                                <td colspan="2">Enter amount of storage required to store non-database data:</td>
                                                            </tr>
                                                            <tr>
                                                                <td><img src="/images/spacer.gif" border="0" width="30" height="1" /></td>
                                                                <td width="100%"><asp:TextBox ID="txtDatabaseNon" runat="server" CssClass="default" Width="100" MaxLength="10" /> GB</td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </asp:Panel>
                                                
                                                <asp:Panel ID="panLUNs" runat="server" Visible="false">
                                                    <div style="height:100%; width:100%; overflow:auto">
                                                        <table width="100%" cellpadding="4" cellspacing="0" border="0" align="center" style="border:solid 1px #CCCCCC">
                                                            <tr bgcolor="#EEEEEE">
                                                                <td style='display:<%=boolWindows ? "inline" : "none" %>' nowrap><b><u>Drive:</u></b></td>
                                                                <td style='display:<%=boolWindows ? "inline" : "none" %>' width="100%"><b><u>Mount Point:</u></b></td>
                                                                <td style='display:<%=boolWindows == false ? "inline" : "none" %>' width="100%"><b><u>Filesystem:</u></b></td>
                                                                <td nowrap><b><u>Shared:</u></b></td>
                                                                <td nowrap><b><u>Size:</u></b></td>
                                                                <td nowrap></td>
                                                            </tr>
                                                            <tr id="trStorageOSWindows" runat="server" visible="false">
                                                                <td nowrap>C:\,D:\</td>
                                                                <td width="100%">&nbsp;&nbsp;<span class="footer">(Reserved: Operating System)</span></td>
                                                                <td nowrap><input type="checkbox" disabled="disabled" /></td>
                                                                <td nowrap><asp:TextBox ID="txtStorageSizeOS" runat="server" Width="50" MaxLength="10" /> GB</td>
                                                                <td nowrap>
                                                                    <asp:ImageButton ID="btnStorageSaveOS" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/save_icon.jpg" CommandArgument="OS" OnClick="btnStorageSave_Click" /> 
                                                                    <img src="/images/spacer.gif" border="0" height="16" width="16" />
                                                                </td>
                                                            </tr>
                                                            <tr id="trStorageApp" runat="server" visible="false">
                                                                <td nowrap>E:**</td>
                                                                <td width="100%">&nbsp;&nbsp;<span class="footer">(Reserved: Application Drive)</span></td>
                                                                <td nowrap><input type="checkbox" disabled="disabled" /></td>
                                                                <td nowrap><asp:TextBox ID="txtStorageSizeE" runat="server" Width="50" MaxLength="10" /> GB</td>
                                                                <td nowrap>
                                                                    <asp:ImageButton ID="btnStorageSaveSizeE" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/save_icon.jpg" CommandArgument="APP" OnClick="btnStorageSave_Click" /> 
                                                                    <img src="/images/spacer.gif" border="0" height="16" width="16" />
                                                                </td>
                                                            </tr>
                                                            <asp:repeater ID="rptStorage" runat="server">
                                                                <ItemTemplate>
                                                                    <tr>
                                                                        <td style='display:<%=boolWindows ? "inline" : "none" %>' valign="top" nowrap><asp:DropDownList ID="ddlStorageDrive" runat="server" /></td>
                                                                        <td valign="top" width="100%"><asp:TextBox ID="txtStoragePath" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "path") %>' Width="200" /></td>
                                                                        <td valign="top" nowrap><asp:CheckBox ID="chkStorageSize" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "shared") %>' /></td>
                                                                        <td valign="top" nowrap><asp:TextBox ID="txtStorageSize" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "size") %>' Width="50" MaxLength="10" /> GB</td>
                                                                        <td valign="top" nowrap align="right">
                                                                            <asp:ImageButton ID="btnStorageSave" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/save_icon.jpg" CommandArgument="SAVE" CommandName='<%# DataBinder.Eval(Container.DataItem, "id") %>' OnClick="btnStorageSave_Click" /> 
                                                                            <asp:ImageButton ID="btnStorageDelete" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/cancel.gif" CommandArgument="DELETE" CommandName='<%# DataBinder.Eval(Container.DataItem, "id") %>' OnClick="btnStorageSave_Click" /> 
                                                                        </td>
                                                                    </tr>
                                                                </ItemTemplate>
                                                            </asp:repeater>
                                                            <tr id="trStorageDrive" runat="server" visible="false">
                                                                <td id="tdStorageDrive" nowrap><asp:DropDownList ID="ddlStorageDriveNew" runat="server" /></td>
                                                                <td width="100%"><asp:TextBox ID="txtStoragePathNew" runat="server" Width="200" /></td>
                                                                <td nowrap><asp:CheckBox ID="chkStorageSizeNew" runat="server" /></td>
                                                                <td nowrap><asp:TextBox ID="txtStorageSizeNew" runat="server" Width="50" MaxLength="10" /> GB</td>
                                                                <td nowrap>
                                                                    <asp:ImageButton ID="btnStorageSaveNew" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/save_icon.jpg" CommandArgument="NEW" OnClick="btnStorageSave_Click" /> 
                                                                    <asp:ImageButton ID="btnStorageDeleteNew" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/cancel.gif" CommandArgument="CANCEL" OnClick="btnStorageSave_Click" /> 
                                                                </td>
                                                            </tr>
                                                            <tr id="trStorageDriveNew" runat="server">
                                                                <td colspan="10">
                                                                    <asp:LinkButton ID="btnStorageDriveAdd" runat="server" Text="<img src='/images/green_arrow.gif' border='0' align='absmiddle'/> Add More Storage" OnClick="btnStorageDriveAdd_Click" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <div align="right" style='display:<%=boolWindows ? "inline" : "none" %>'>** = If you do not require an application drive, set it to zero (0) GB.</div>
                                                    </div>
                                                    <div style='display:<%=boolWindows ? "inline" : "none" %>'>
                                                        <br />
                                                        <table width="100%" cellpadding="4" cellspacing="0" border="0">
                                                            <tr>
                                                                <td><img src="/images/alert.gif" border="0" align="absmiddle" /></td>
                                                                <td width="100%"><b>NOTE:</b> Standard drives such as C:, D:, P:, and Q: are not configurable.  If applicable, they will be automatically added to this build.</td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </asp:Panel>
                                                <table width="100%" cellpadding="4" cellspacing="0" border="0">
                                                    <tr>
                                                        <td colspan="2">
                                                            <asp:Button ID="btnGenerate" runat="server" Text="Generate Storage" Width="125" OnClick="btnGenerate_Click" Visible="false" />
                                                            <asp:Button ID="btnReset" runat="server" Text="Reset Storage" Width="125" OnClick="btnReset_Click" Visible="false" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                                <tr height="1">
                                    <td>
                                        <asp:Panel ID="panHelp" runat="server" Visible="false">
                                            <table width="100%" cellpadding="5" cellspacing="0" border="0">
                                                <tr id="trHelp" runat="server" style="display:none">
                                                    <td>
                                                        <table width="100%" height="100%" cellpadding="0" cellspacing="0" border="0">
                                                            <tr height="1">
                                                                <td><img src="/images/table_topleft.gif" border="0" width="5" height="26"></td>
                                                                <td nowrap background="/images/table_top.gif" class="greentableheader" width="100%"><img src="/images/help24.gif" border="0" align="absmiddle" /> Help<asp:Label ID="Label1" runat="server" /></td>
                                                                <td><img src="/images/table_topright.gif" border="0" width="5" height="26"></td>
                                                            </tr>
                                                            <tr>
                                                                <td background="/images/table_left.gif"><img src="/images/table_left.gif" width="5" height="10"></td>
                                                                <td width="100%" bgcolor="#FFFFFF" valign="top" style="padding:3px">
                                                                    <asp:Literal ID="litHelp" runat="server" />
                                                                </td>
                                                                <td background="/images/table_right.gif"><img src="/images/table_right.gif" width="5" height="10"></td>
                                                            </tr>
                                                            <tr height="1">
                                                                <td><img src="/images/table_bottomLeft.gif" border="0" width="5" height="9"></td>
                                                                <td width="100%" background="/images/table_bottom.gif"></td>
                                                                <td><img src="/images/table_bottomRight.gif" border="0" width="5" height="9"></td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="panSummary" runat="server" Visible="false">
                <%=strExecuteHeader%>
                <asp:Panel ID="panNotExecutable" runat="server" Visible="false">
                    <table width="100%" height="100%" cellpadding="4" cellspacing="2" border="0">
                        <tr>
                            <td align="center" valign="top">
                                <table cellpadding="2" cellspacing="1" border="0" style="border:solid 1px #999999" bgcolor="#f9f9f9">
                                    <tr>
                                        <td>
                                            <table width="100%" cellpadding="0" cellspacing="5" border="0">
                                                <tr>
                                                    <td rowspan="3" width="1" valign="top"><%=strManualImage %></td>
                                                    <td class="header" valign="bottom">Manual Build Process</td>
                                                </tr>
                                                <tr>
                                                    <td valign="top">This design requires a technicain to manually build the device(s). Your implementor has been notified of the request to build this design.</td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <table cellpadding="0" cellspacing="5" border="0">
                                                            <tr>
                                                                <td>Implementor:</td>
                                                                <td><asp:Label ID="lblImplementor" runat="server" /></td>
                                                            </tr>
                                                            <tr>
                                                                <td>Initiated On:</td>
                                                                <td><asp:Label ID="lblInitiated" runat="server" /></td>
                                                            </tr>
                                                            <tr>
                                                                <td>Completed On:</td>
                                                                <td><asp:Label ID="lblCompleted" runat="server" /></td>
                                                            </tr>
                                                            <tr>
                                                                <td>Reason:</td>
                                                                <td><%=strManualReason %></td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2"><img src="/images/spacer.gif" border="0" width="1" height="1" /></td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2"><asp:HyperLink ID="btnManual" runat="server" Target="_blank">Click here for additional information.</asp:HyperLink></td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2"><img src="/images/spacer.gif" border="0" width="1" height="1" /></td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">Here is the current information available for your device(s)...</td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <table cellpadding="5" cellspacing="0" border="0" style="border:solid 1px #CCCCCC">
                                                            <tr bgcolor="#EEEEEE">
                                                                <td>&nbsp;</td>
                                                                <td>&nbsp;</td>
                                                                <td nowrap valign="top"><b>Serial #</b></td>
                                                                <td>&nbsp;</td>
                                                                <td nowrap valign="top"><b>Serial # (DR)</b></td>
                                                                <td>&nbsp;</td>
                                                                <td nowrap valign="top"><b>Server Name</b></td>
                                                                <td>&nbsp;</td>
                                                                <td nowrap valign="top"><b>IP Address # 1</b></td>
                                                                <td>&nbsp;</td>
                                                                <td nowrap valign="top"><b>IP Address # 2</b></td>
                                                                <td>&nbsp;</td>
                                                                <td nowrap valign="top"><b>IP Address # 3</b></td>
                                                            </tr>
                                                            <asp:repeater ID="rptServers" runat="server">
                                                                <ItemTemplate>
                                                                    <tr class="default" style='<%=intDeviceCount % 2 != 0 ? "" : "background-color:#F6F6F6"%>'>
                                                                        <td>Device&nbsp;#&nbsp;<%=intDeviceCount++ %></td>
                                                                        <td>&nbsp;</td>
                                                                        <td nowrap><asp:Label ID="lblAsset" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "serial")%>' /></td>
                                                                        <td>&nbsp;</td>
                                                                        <td nowrap><asp:Label ID="lblAssetDR" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "serial_dr")%>' /></td>
                                                                        <td>&nbsp;</td>
                                                                        <td nowrap><asp:Label ID="lblName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "servername")%>' ToolTip='<%# DataBinder.Eval(Container.DataItem, "id")%>' /></td>
                                                                        <td>&nbsp;</td>
                                                                        <td nowrap><asp:Label ID="lblIP1" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ipaddress1").ToString() + (DataBinder.Eval(Container.DataItem, "vlan1").ToString() == "" ? "" : " (VLAN:" + DataBinder.Eval(Container.DataItem, "vlan1").ToString() + ")") %>' /></td></td>
                                                                        <td>&nbsp;</td>
                                                                        <td nowrap><asp:Label ID="lblIP2" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ipaddress2").ToString() + (DataBinder.Eval(Container.DataItem, "vlan2").ToString() == "" ? "" : " (VLAN:" + DataBinder.Eval(Container.DataItem, "vlan2").ToString() + ")") %>' /></td></td>
                                                                        <td>&nbsp;</td>
                                                                        <td nowrap><asp:Label ID="lblIP3" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ipaddress3").ToString() + (DataBinder.Eval(Container.DataItem, "vlan3").ToString() == "" ? "" : " (VLAN:" + DataBinder.Eval(Container.DataItem, "vlan3").ToString() + ")") %>' /></td></td>
                                                                    </tr>
                                                                </ItemTemplate>
                                                            </asp:repeater>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
	                        </td>
                        </tr>
                    </table>
                </asp:Panel>
                <%=strExecuteMiddle%>
                <table height="100%" width="100%" cellpadding="0" cellspacing="0" border="0">
                    <tr height="1">
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td align="center">
                            <table width="95%" height="100%" cellpadding="0" cellspacing="0" border="0">
                                <tr height="1">
                                    <td><img src="/images/table_topleft.gif" border="0" width="5" height="26"></td>
                                    <td nowrap background="/images/table_top.gif" class="greentableheader" width="100%">
                                        <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                            <tr>
                                                <td class="greentableheader">Design Summary<asp:Label ID="lblID2" runat="server" /></td>
                                                <td align="right" class="biggerbold"><asp:LinkButton ID="btnPrint" runat="server" Text="<img src='/images/print.gif' border='0' align='absmiddle' /><img src='/images/spacer.gif' border='0' width='5' height='1' />Printer Friendly Summary" /></td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td><img src="/images/table_topright.gif" border="0" width="5" height="26"></td>
                                </tr>
                                <tr>
                                    <td background="/images/table_left.gif"><img src="/images/table_left.gif" width="5" height="10"></td>
                                    <td width="100%" bgcolor="#FFFFFF" valign="top">
                                        <div style="height:100%; overflow:auto">
                                            <asp:PlaceHolder ID="phSummary" runat="server" />
                                        </div>
                                    </td>
                                    <td background="/images/table_right.gif"><img src="/images/table_right.gif" width="5" height="10"></td>
                                </tr>
                                <tr height="1">
                                    <td><img src="/images/table_bottomLeft.gif" border="0" width="5" height="9"></td>
                                    <td width="100%" background="/images/table_bottom.gif"></td>
                                    <td><img src="/images/table_bottomRight.gif" border="0" width="5" height="9"></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr height="1">
                        <td align="center">
                            <table width="95%" cellpadding="0" cellspacing="0" border="0">
                                <tr>
                                    <td align="center">
                                        <asp:Panel ID="panDemoExecuted" runat="server" Visible="false">
                                            <div align="Center">
                                                <br />
                                                <table cellpadding="3" cellspacing="5" border="0" style="border:solid 1px #999999" bgcolor="#f9f9f9">
                                                    <tr>
                                                        <td rowspan="5" valign="top"><img src="/images/ico_check.gif" border="0" align="absmiddle" /></td>
                                                        <td class="header" valign="bottom">Design has been Executed</td>
                                                    </tr>
                                                    <tr>
                                                        <td valign="top">This design was executed on <asp:Label ID="lblExecutedOn" runat="server" CssClass="bold" /> by <asp:Label ID="lblExecutedBy" runat="server" CssClass="bold" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td valign="top"><asp:LinkButton ID="btnDemoReview" runat="server" Text="<img src='/images/project_request.gif' border='0' align='absmiddle' /><img src='/images/spacer.gif' border='0' width='5' height='1'/>Review Design Requirements" OnClick="btnReview_Click" /></td>
                                                    </tr>
                                                </table>
                                            </div>
                                            <p>&nbsp;</p>
                                        </asp:Panel>
                                        <asp:Panel ID="panDemoStart" runat="server" Visible="false">


                                            <table cellpadding="2" cellspacing="2" border="0">
                                                <tr>
                                                    <td width="50%" valign="top" style="border:solid 1px #999999" bgcolor="#f9f9f9">
                                                        <table width="100%" cellpadding="2" cellspacing="2" border="0">
                                                            <tr>
                                                                <td rowspan="3" valign="top"><img src="/images/question.gif" border="0" align="absmiddle" /></td>
                                                                <td class="bigblue" width="100%" valign="bottom">Select your Build Option</td>
                                                            </tr>
                                                            <tr>
                                                                <td valign="top">
                                                                    <table cellpadding="2" cellspacing="3" border="0">
                                                                        <tr>
                                                                            <td><asp:RadioButton ID="radStart" runat="server" CssClass="default" Text="Option 1: Start the Build Now" GroupName="buildOptions" /></td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td><asp:RadioButton ID="radSchedule" runat="server" CssClass="default" Text="Option 2: Schedule the Build" GroupName="buildOptions" /></td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td><asp:RadioButton ID="radApproval" runat="server" CssClass="default" Text="Option 3: Request Approval" GroupName="buildOptions" Enabled="false" /></td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <table width="100%" cellpadding="2" cellspacing="2" border="0">
                                                            <tr>
                                                                <td valign="top"><asp:LinkButton ID="btnChange" CssClass="bold" runat="server" Text="<img src='/images/arrowLeft.gif' border='0' align='absmiddle' /><img src='/images/spacer.gif' border='0' width='5' height='5' />Click here to modify this design" OnClick="btnChange_Click" /></td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td>&nbsp;</td>
                                                    <td width="50%" valign="top" style="border:solid 1px #999999" bgcolor="#f9f9f9">
                                                        <div id="divSoftware" runat="server" style="display:none">
                                                            <table width="100%" cellpadding="2" cellspacing="2" border="0">
                                                                <tr>
                                                                    <td rowspan="5" valign="top"><img src="/images/hugeAlert.gif" border="0" align="absmiddle" /></td>
                                                                    <td class="bigblue" width="100%" valign="bottom">Build Option Unavailable</td>
                                                                </tr>
                                                                <tr>
                                                                    <td valign="top">You cannot select a build option until all software components are approved.</td>
                                                                </tr>
                                                                <tr>
                                                                    <td valign="top">&nbsp;</td>
                                                                </tr>
                                                                <tr>
                                                                    <td valign="top">&nbsp;</td>
                                                                </tr>
                                                                <tr>
                                                                    <td valign="top">&nbsp;</td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                        <div id="divDefault" runat="server" style="display:inline">
                                                            <table width="100%" cellpadding="2" cellspacing="2" border="0">
                                                                <tr>
                                                                    <td rowspan="5" valign="top"><img src="/images/ico_hourglass.gif" border="0" align="absmiddle" /></td>
                                                                    <td class="bigblue" width="100%" valign="bottom">Please select a Build Option</td>
                                                                </tr>
                                                                <tr>
                                                                    <td valign="top">Select a build option to configure your build.</td>
                                                                </tr>
                                                                <tr>
                                                                    <td valign="top">&nbsp;</td>
                                                                </tr>
                                                                <tr>
                                                                    <td valign="top">&nbsp;</td>
                                                                </tr>
                                                                <tr>
                                                                    <td valign="top">&nbsp;</td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                        <div ID="divValidationDemo" runat="server" style="display:none">
                                                            <table cellpadding="3" cellspacing="5" border="0">
                                                                <tr>
                                                                    <td rowspan="5" valign="top"><img src="/images/bigAlert.gif" border="0" align="absmiddle" /></td>
                                                                    <td class="header" valign="bottom">Incomplete Information</td>
                                                                </tr>
                                                                <tr>
                                                                    <td valign="top">The following requirements are incomplete...</td>
                                                                </tr>
                                                                <tr>
                                                                    <td valign="top"><asp:Label ID="lblValidDemo" runat="server" /></td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                        <div id="divStart" runat="server" style="display:none">
                                                            <table width="100%" cellpadding="2" cellspacing="2" border="0">
                                                                <tr>
                                                                    <td rowspan="5" valign="top"><img src="/images/ico_check.gif" border="0" align="absmiddle" /></td>
                                                                    <td class="bigblue" width="100%" valign="bottom">Option 1: Start the Build Now</td>
                                                                </tr>
                                                                <tr>
                                                                    <td valign="top">Click <b>Start the Build</b> to start your auto-provisioning build.</td>
                                                                </tr>
                                                                <tr>
                                                                    <td valign="top">&nbsp;</td>
                                                                </tr>
                                                                <tr>
                                                                    <td><asp:Button ID="btnStart" runat="server" OnClick="btnStart_Click" Text="Start the Build" CssClass="default" Width="125" /></td>
                                                                </tr>
                                                                <tr>
                                                                    <td> <asp:Label ID="lblNotify" runat="server" CssClass="reddefault" Text="<b>BURN-IN PROCESS:</b> Upon clicking this button, the design implementor will be notified of your request." Visible="false" /></td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                        <div id="divSchedule" runat="server" style="display:none">
                                                            <table width="100%" cellpadding="2" cellspacing="2" border="0">
                                                                <tr>
                                                                    <td rowspan="4" valign="top"><img src="/images/clock.gif" border="0" align="absmiddle" /></td>
                                                                    <td class="bigblue" width="100%" valign="bottom">Option 2: Schedule the Build</td>
                                                                </tr>
                                                                <tr>
                                                                    <td valign="top">Enter the date and time on which you want the device built and click <b>Schedule the Build</b> to have ClearView build the device for you.</td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <table cellpadding="2" cellspacing="0" border="0">
                                                                            <tr>
                                                                                <td><asp:TextBox ID="txtScheduleDate" runat="server" CssClass="default" Width="100" MaxLength="10" /></td>
                                                                                <td><asp:ImageButton ID="imgScheduleDate" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calendar.gif" /></td>
                                                                                <td>at</td>
                                                                                <td><asp:TextBox ID="txtScheduleTime" runat="server" CssClass="default" Width="75" MaxLength="8" /></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td colspan="2"></td>
                                                                                <td colspan="2" class="footer">Example:&nbsp;12:15&nbsp;AM</td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td><asp:Button ID="btnSchedule" runat="server" OnClick="btnSchedule_Click" Text="Schedule the Build" CssClass="default" Width="125" /></td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                        <div id="divApproval" runat="server" style="display:none">
                                                            <table width="100%" cellpadding="2" cellspacing="2" border="0">
                                                                <tr>
                                                                    <td rowspan="5" valign="top"><img src="/images/step_3.gif" border="0" align="absmiddle" /></td>
                                                                    <td class="bigblue" width="100%" valign="bottom">Option 3: Request Approval</td>
                                                                </tr>
                                                                <tr>
                                                                    <td valign="top">Request approval from one or more people.</td>
                                                                </tr>
                                                                <tr>
                                                                    <td valign="top">&nbsp;</td>
                                                                </tr>
                                                                <tr>
                                                                    <td><asp:Button ID="btnApprovals" runat="server" Text="Add / Remove Approvers" CssClass="default" Width="175" /></td>
                                                                </tr>
                                                                <tr>
                                                                    <td valign="top"><b>NOTE:</b> Once approved, the build will automatically start.</td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>


                                        </asp:Panel>
                                        <asp:Panel ID="panSubmit" runat="server" Visible="false">
                                            <table width="100%" cellpadding="0" cellspacing="5" border="0">
                                                <tr>
                                                    <td width="50%" valign="top">
                                                        <table width="100%" cellpadding="0" cellspacing="5" border="0">
                                                            <tr>
                                                                <td><b>Attest this solution by selecting one of the following:</b></td>
                                                            </tr>
                                                            <tr>
                                                                <td><asp:RadioButton ID="radComplete" runat="server" Text="Complete - I agree" GroupName="attest" /></td>
                                                            </tr>
                                                            <tr>
                                                                <td><asp:RadioButton ID="radChange" runat="server" Text="Make Changes - I forgot something" GroupName="attest" /></td>
                                                            </tr>
                                                            <tr>
                                                                <td><asp:RadioButton ID="radException" runat="server" Text="This does not fit my needs - I need an exception to the company standard or policy" GroupName="attest" /></td>
                                                            </tr>
                                                            <tr>
                                                                <td>&nbsp;</td>
                                                            </tr>
                                                            <tr>
                                                                <td valign="top"><asp:Button ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" Text="Submit" CssClass="default" Width="75" /></td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td width="50%" valign="top">
                                                        <div ID="divValidation" runat="server" style="display:none">
                                                            <table cellpadding="3" cellspacing="5" border="0" style="border:solid 1px #999999" bgcolor="#f9f9f9">
                                                                <tr>
                                                                    <td rowspan="5" valign="top"><img src="/images/bigAlert.gif" border="0" align="absmiddle" /></td>
                                                                    <td class="header" valign="bottom">Incomplete Information</td>
                                                                </tr>
                                                                <tr>
                                                                    <td valign="top">The following requirements are incomplete or not available (per the reference architecture)...</td>
                                                                </tr>
                                                                <tr>
                                                                    <td valign="top"><asp:Label ID="lblValid" runat="server" /></td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                        <div ID="divValid" runat="server" style="display:none">
                                                            <div align="Center">
                                                            <table cellpadding="3" cellspacing="5" border="0" style="border:solid 1px #999999" bgcolor="#f9f9f9">
                                                                <tr>
                                                                    <td rowspan="5" valign="top"><img src="/images/bigCheck.gif" border="0" align="absmiddle" /></td>
                                                                    <td class="header" valign="bottom">Validation Passed</td>
                                                                </tr>
                                                                <tr>
                                                                    <td valign="top">The functional requirements of this design are consistent with the reference architecture.</td>
                                                                </tr>
                                                            </table>
                                                            </div>
                                                        </div>
                                                        <div ID="divException" runat="server" style="display:none">
                                                            <table cellpadding="1" cellspacing="5" border="0" style="border:solid 1px #999999" bgcolor="#f9f9f9">
                                                                <tr>
                                                                    <td rowspan="7" valign="top"><img src="/images/circlealert.gif" border="0" align="absmiddle" /></td>
                                                                    <td class="header" valign="bottom">Exception Approval Required</td>
                                                                </tr>
                                                                <tr id="trException" runat="server" visible="false">
                                                                    <td valign="top"><asp:LinkButton ID="btnException" runat="server" Text="One or more of your responses" /> require approval.</td>
                                                                </tr>
                                                                <tr>
                                                                    <td valign="top">SharePoint Exception Management System (SEMS) ID #...</td>
                                                                </tr>
                                                                <tr>
                                                                    <td valign="top"><asp:TextBox ID="txtExceptionID" runat="server" Width="100" MaxLength="20" /> <a href="javascript:void(0);" onclick="alert('If this exception involves deviating from an approved Reference Architecture, you will need to submit a Reference Architecture Exception request in the SharePoint Exception Management System (SEMS).');">More Information</a></td>
                                                                </tr>
                                                                <tr>
                                                                    <td valign="top"><a href="https://appswss.wss.pncbank.com/sites/ARBD/RAE/SitePages/Guidelines.aspx" target="_blank">Click here to determine if you need to fill out an SEMS request</a></td>
                                                                </tr>
                                                                <tr>
                                                                    <td valign="top">Provide a brief justification as why you need this exception. NOTE: Architecture Review Board (ARB) are the only resources that will read the justification that you input...</td>
                                                                </tr>
                                                                <tr>
                                                                    <td valign="top"><asp:TextBox ID="txtException" runat="server" TextMode="MultiLine" Width="325" Rows="5" /></td>
                                                                </tr>
                                                            </table>
                                                            </div>
                                                        </div>
                                                        <div ID="divReject" runat="server" style="display:none">
                                                            <table width="100%" cellpadding="0" cellspacing="5" border="0" style="border:solid 1px #999999" bgcolor="#f9f9f9">
                                                                <tr>
                                                                    <td rowspan="5" valign="top"><img src="/images/bigError.gif" border="0" align="absmiddle" /></td>
                                                                    <td class="header" valign="bottom">This Design has been Rejected</td>
                                                                </tr>
                                                                <tr>
                                                                    <td valign="top">
                                                                        <table width="100%" cellpadding="3" cellspacing="0" border="0">
                                                                            <tr>
                                                                                <td valign="top"><img src="/images/icon_answer.gif" border="0" /></td>
                                                                                <td valign="top" width="100%">
                                                                                    <div style="height:50px; overflow:auto">
                                                                                        <asp:Label ID="lblRejectReason" runat="server" />
                                                                                    </div>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>&nbsp;</td>
                                                                                <td class="comment" nowrap><asp:Label ID="lblRejectBy" runat="server" CssClass="comment" /></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>&nbsp;</td>
                                                                                <td class="comment" nowrap><i><asp:Label ID="lblRejectTitle" runat="server" CssClass="comment" /></i></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>&nbsp;</td>
                                                                                <td class="comment" nowrap><asp:Label ID="lblRejectOn" runat="server" /></td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                        <asp:Panel ID="panExceptionServiceFolder" runat="server" Visible="false">
                                                            <table cellpadding="1" cellspacing="5" border="0" style="border:solid 1px #999999" bgcolor="#f9f9f9">
                                                                <tr>
                                                                    <td rowspan="7" valign="top"><img src="/images/project_locked.gif" border="0" align="absmiddle" /></td>
                                                                    <td class="header" valign="bottom">Exception Process</td>
                                                                </tr>
                                                                <tr>
                                                                    <td valign="top"><asp:HyperLink ID="hypException" runat="server" Text="Click here to submit the exception request" Target="_blank" /></td>
                                                                </tr>
                                                                <tr>
                                                                    <td valign="top">Exceptions must be submitted using one of the services in the above link.</td>
                                                                </tr>
                                                            </table>
                                                        </asp:Panel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                        <asp:Panel ID="panFreeze" runat="server" Visible="false">
                                            <br />
                                            <table cellpadding="0" cellspacing="5" border="0" style="border:solid 2px #f08484" bgcolor="#fdf2f0">
                                                <tr>
                                                    <td rowspan="3" valign="top"><img src="/images/ico_error.gif" border="0" align="absmiddle" /></td>
                                                    <td width="100%" valign="bottom" class="bigreddefault bold">Designs are not permitted to be executed during the restricted change window.</td>
                                                </tr>
                                                <tr>
                                                    <td width="100%" valign="top"><b>Restrcited Change Window Ends:</b> <%=strFreezeEnd %>. <a href="http://tps.pnc.com/its/its.nsf/ViewByKey/Restricted-Change-Windows?OpenDocument&Menu=Change_Management&Sub=News" target="_blank">Click here for more information.</a></td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                        <asp:Panel ID="panWorkflow" runat="server" Visible="false">
                                            <br />        
                                            <table width="100%" cellpadding="0" cellspacing="5" border="0" style="border:solid 1px #999999" bgcolor="#f9f9f9">
                                                <tr>
                                                    <td>
                                                        <asp:Panel ID="panApproval" runat="server" Visible="false">
                                                            <table cellpadding="0" cellspacing="5" border="0">
                                                                <tr>
                                                                    <td rowspan="3" valign="top"><img src="/images/ico_hourglass40.gif" border="0" align="absmiddle" /></td>
                                                                    <td width="100%" class="header" valign="bottom">Pending Approval</td>
                                                                </tr>
                                                                <tr>
                                                                    <td valign="top">Your design is currently under review...</td>
                                                                </tr>
                                                            </table>
                                                        </asp:Panel>
                                                        <asp:Panel ID="panTechnician" runat="server" Visible="false">
                                                            <table cellpadding="0" cellspacing="5" border="0">
                                                                <tr>
                                                                    <td rowspan="5" valign="top"><img src="/images/hugeAlert.gif" border="0" align="absmiddle" /></td>
                                                                    <td width="100%" class="header" valign="bottom">Technician Request Submitted</td>
                                                                </tr>
                                                                <tr>
                                                                    <td valign="top">A request has been submitted for a technician to build your server...</td>
                                                                </tr>
                                                                <tr>
                                                                    <td>&nbsp;</td>
                                                                </tr>
                                                                <tr>
                                                                    <td valign="top"><b>NOTE:</b> Once a technician has been assigned, you will be able to execute this build.</td>
                                                                </tr>
                                                                <tr>
                                                                    <td><asp:Label ID="lblTechnician" runat="server" /></td>
                                                                </tr>
                                                            </table>
                                                        </asp:Panel>
                                                        <asp:Panel ID="panApproved" runat="server" Visible="false">
                                                            <table cellpadding="0" cellspacing="5" border="0">
                                                                <tr>
                                                                    <td rowspan="3" valign="top"><img src="/images/ico_check.gif" border="0" align="absmiddle" /></td>
                                                                    <td width="100%" class="header" valign="bottom">Design Approved</td>
                                                                </tr>
                                                                <tr>
                                                                    <td valign="top">Your design has been approved and is ready to execute.</td>
                                                                </tr>
                                                                <tr id="panExecute" runat="server" visible="false">
                                                                    <td valign="top"><asp:Button ID="btnExecute" runat="server" Text="Execute" Width="75" OnClick="btnExecute_Click" /></td>
                                                                </tr>
                                                                <tr>
                                                                    <td>&nbsp;</td>
                                                                </tr>
                                                            </table>
                                                        </asp:Panel>
                                                        <asp:Panel ID="panPending" runat="server" Visible="false">
                                                            <table cellpadding="0" cellspacing="5" border="0">
                                                                <tr>
                                                                    <td rowspan="3" valign="top"><asp:Image ID="imgPending" runat="server" ImageUrl="/images/ico_hourglass40.gif" ImageAlign="AbsMiddle" /></td>
                                                                    <td width="100%" class="header" valign="bottom">Pending Mnemonic Approval</td>
                                                                </tr>
                                                                <tr>
                                                                    <td valign="top">Your design has been approved. However, the mnemonic you selected has not been approved. You cannot execute until it is approved.</td>
                                                                </tr>
                                                                <tr>
                                                                    <td valign="top"><asp:Button ID="btnNothing" runat="server" Text="Execute" Width="75" Enabled="false" /></td>
                                                                </tr>
                                                                <tr>
                                                                    <td>&nbsp;</td>
                                                                </tr>
                                                            </table>
                                                        </asp:Panel>
                                                    </td>
                                                    <td align="right" nowrap><asp:LinkButton ID="btnReview" runat="server" CssClass="bigblue" Text="<img src='/images/arrowLeft.gif' border='0' align='absmiddle' /><img src='/images/spacer.gif' border='0' width='5' height='1'/>Review Design Requirements" OnClick="btnReview_Click" /></td>
                                                </tr>
                                                <tr id="panApprovers" runat="server">
                                                    <td colspan="2">
                                                        <br />
                                                        <div style="height:100px; overflow:auto">
                                                            <table width="100%" cellpadding="4" cellspacing="0" border="0" style="border:solid 1px #CCCCCC">
                                                                <tr bgcolor="#EEEEEE">
                                                                    <td></td>
                                                                    <td><b><u>Approver:</u></b></td>
                                                                    <td><b><u>Status:</u></b></td>
                                                                    <td><b><u>Notified On:</u></b></td>
                                                                    <td><b><u>Completed On:</u></b></td>
                                                                    <td></td>
                                                                </tr>
                                                                <asp:repeater ID="rptWorkflow" runat="server">
                                                                    <ItemTemplate>
                                                                        <tr>
                                                                            <td valign="top" rowspan="2"><img src='/images/<%# DataBinder.Eval(Container.DataItem, "status") %>.gif' border='0' /></td>
                                                                            <td valign="top" width="40%" nowrap class="tableheader"><%# DataBinder.Eval(Container.DataItem, "title")%></td>
                                                                            <td valign="top" width="10%"><%# DataBinder.Eval(Container.DataItem, "status") %></td>
                                                                            <td valign="top" width="25%"><%# DataBinder.Eval(Container.DataItem, "created") %></td>
                                                                            <td valign="top" width="25%"><%# DataBinder.Eval(Container.DataItem, "completed") %></td>
                                                                        </tr>
                                                                        <tr style='display:<%# DataBinder.Eval(Container.DataItem, "approver").ToString() == "" ? "none" : "inline" %>'>
                                                                            <td colspan="4"><img src="/images/down_right.gif" border="0" align="absmiddle" /> <%# DataBinder.Eval(Container.DataItem, "approver") %></td>
                                                                        </tr>
                                                                        <asp:repeater id="rptApprovers" datasource='<%# ((DataRowView)Container.DataItem).Row.GetChildRows("relationship") %>' runat="server">
                                                                            <ItemTemplate>
                                                                            <tr>
                                                                                <td></td>
                                                                                <td colspan="4"><img src="/images/down_right.gif" border="0" align="absmiddle" /> <%# DataBinder.Eval(Container.DataItem, "[\"approver\"]") %></td>
                                                                            </tr>
                                                                            </ItemTemplate>
                                                                        </asp:repeater>
                                                                    </ItemTemplate>
                                                                    <AlternatingItemTemplate>
                                                                        <tr bgcolor='<%# DataBinder.Eval(Container.DataItem, "status").ToString() == "Active" ? "" : "#F6F6F6" %>'>
                                                                            <td valign="top" rowspan="2"><img src='/images/<%# DataBinder.Eval(Container.DataItem, "status") %>.gif' border='0' /></td>
                                                                            <td valign="top" width="40%" nowrap class="tableheader"><%# DataBinder.Eval(Container.DataItem, "title")%></td>
                                                                            <td valign="top" width="10%"><%# DataBinder.Eval(Container.DataItem, "status") %></td>
                                                                            <td valign="top" width="25%"><%# DataBinder.Eval(Container.DataItem, "created") %></td>
                                                                            <td valign="top" width="25%"><%# DataBinder.Eval(Container.DataItem, "completed") %></td>
                                                                        </tr>
                                                                        <tr bgcolor='<%# DataBinder.Eval(Container.DataItem, "status").ToString() == "Active" ? "" : "#F6F6F6" %>' style='display:<%# DataBinder.Eval(Container.DataItem, "approver").ToString() == "" ? "none" : "inline" %>'>
                                                                            <td colspan="4"><img src="/images/down_right.gif" border="0" align="absmiddle" /> <%# DataBinder.Eval(Container.DataItem, "approver") %></td>
                                                                        </tr>
                                                                        <asp:repeater id="rptApprovers" datasource='<%# ((DataRowView)Container.DataItem).Row.GetChildRows("relationship") %>' runat="server">
                                                                            <ItemTemplate>
                                                                            <tr>
                                                                                <td></td>
                                                                                <td colspan="4"><img src="/images/down_right.gif" border="0" align="absmiddle" /> <%# DataBinder.Eval(Container.DataItem, "[\"approver\"]") %></td>
                                                                            </tr>
                                                                            </ItemTemplate>
                                                                        </asp:repeater>
                                                                    </AlternatingItemTemplate>
                                                                </asp:repeater>
                                                            </table>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                            <br />
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <%=strExecuteFooter%>
            </asp:Panel>
        </td>
    </tr>
    <tr height="1">
        <td colspan="2">
            <table width="100%" cellpadding="4" cellspacing="2" border="0">
                <tr>
                    <td colspan="3"><hr size="1" noshade /></td>
                </tr>
                <tr>
                    <td width="250" class="required">* = Required Field</td>
                    <td align="center">
                        <asp:Button ID="btnReturn" runat="server" OnClick="btnReturn_Click" Text="Return to Summary" CssClass="default" Width="125" Visible="false" /> 
                        <asp:Button ID="btnUpdate" runat="server" OnClick="btnUpdate_Click" Text="Update" CssClass="default" Width="75" Visible="false" /> 
                        <asp:Button ID="btnBack" runat="server" OnClick="btnBack_Click" Text="Back" CssClass="default" Width="75" /> 
                        <asp:Button ID="btnNext" runat="server" OnClick="btnNext_Click" Text="Next" CssClass="default" Width="75" />
                    </td>
                    <td width="250" align="right">&nbsp;</td>
                </tr>
            </table>
        </td>
    </tr>
</table>
<asp:Label ID="lblPhase" runat="server" Visible="false" />
<asp:HiddenField ID="hdnAccount" runat="server" />
<asp:HiddenField ID="hdnMnemonic" runat="server" />
<asp:HiddenField ID="hdnCostCenter" runat="server" />
<asp:HiddenField ID="hdnSI" runat="server" />
<asp:HiddenField ID="hdnDTG" runat="server" />
<asp:HiddenField ID="hdnDate" runat="server" />
<input type="hidden" id="hdnLocation" runat="server" />
<input type="hidden" id="hdnBackup" name="hdnBackup" value='<%=strGridBackup %>' />
<input type="hidden" id="hdnMaintenance" name="hdnMaintenance" value='<%=strGridMaintenance %>' />
</asp:Content>
