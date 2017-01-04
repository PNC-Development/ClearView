<%@ Page Language="C#" MasterPageFile="~/window.Master" AutoEventWireup="true" CodeBehind="accenture.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.accenture" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" runat="server">
    <script type="text/javascript">
    </script>
<table width="100%" height="100%" cellpadding="0" cellspacing="0" border="0">
    <tr height="1">
        <td>
            <table width="100%" cellpadding="0" cellspacing="5" border="0">
                <tr>
                    <td rowspan="2"><img src="/images/forecast2.gif" border="0" align="absmiddle" /></td>
                    <td class="header" width="100%" valign="bottom">Design Builder Summary</td>
                </tr>
                <tr>
                    <td width="100%" valign="top">You have finished building the design. Please review it and make a selection below.</td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td valign="top">
            <div style="height:100%; overflow:auto">
            <table width="100%" cellpadding="4" cellspacing="0" border="0">
                <tr>
                    <td nowrap>Application Name:</td>
                    <td nowrap><asp:TextBox ID="txtName" runat="server" CssClass="default" Width="300" MaxLength="100" /></td>
                </tr>
                <tr id="panCode" runat="server" visible="false">
                    <td nowrap>Application Code:</td>
                    <td nowrap><asp:TextBox ID="txtCode" runat="server" CssClass="default" Width="100" MaxLength="3" /></td>
                </tr>
                <tr id="panMnemonic" runat="server" visible="false">
                    <td nowrap>Mnemonic:</td>
                    <td nowrap>
                        <table cellpadding="0" cellspacing="0" border="0">
                            <tr>
                                <td><asp:TextBox ID="txtMnemonic" runat="server" Width="500" CssClass="default" /></td>
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
                <tr id="panCostCenter" runat="server" visible="false">
                    <td nowrap>Cost Center:</td>
                    <td nowrap>
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
                <tr id="panDR" runat="server" visible="false">
                    <td nowrap>DR Criticality:</td>
                    <td nowrap>
                        <asp:RadioButton ID="radHigh" runat="server" CssClass="default" Text="1 - High" GroupName="dr" />&nbsp;
                        <asp:RadioButton ID="radLow" runat="server" CssClass="default" Text="2 - Low" GroupName="dr" />&nbsp;
                    </td>
                </tr>
                <tr>
                    <td nowrap>Departmental Manager:</td>
                    <td nowrap>
                        <table cellpadding="0" cellspacing="0" border="0">
                            <tr>
                                <td><asp:TextBox ID="txtOwner" runat="server" Width="300" CssClass="default" /></td>
                            </tr>
                            <tr>
                                <td>
                                    <div id="divOwner" runat="server" style="overflow:hidden; position:absolute; display:none; background-color:#FFFFFF; border:solid 1px #CCCCCC">
                                        <asp:ListBox ID="lstOwner" runat="server" CssClass="default" />
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td nowrap>Application Technical Lead:</td>
                    <td nowrap>
                        <table cellpadding="0" cellspacing="0" border="0">
                            <tr>
                                <td><asp:TextBox ID="txtPrimary" runat="server" Width="300" CssClass="default" /></td>
                            </tr>
                            <tr>
                                <td>
                                    <div id="divPrimary" runat="server" style="overflow:hidden; position:absolute; display:none; background-color:#FFFFFF; border:solid 1px #CCCCCC">
                                        <asp:ListBox ID="lstPrimary" runat="server" CssClass="default" />
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr style="display:none">
                    <td nowrap>Administrative Contact:</td>
                    <td nowrap>
                        <table cellpadding="0" cellspacing="0" border="0">
                            <tr>
                                <td><asp:TextBox ID="txtSecondary" runat="server" Width="300" CssClass="default" /></td>
                            </tr>
                            <tr>
                                <td>
                                    <div id="divSecondary" runat="server" style="overflow:hidden; position:absolute; display:none; background-color:#FFFFFF; border:solid 1px #CCCCCC">
                                        <asp:ListBox ID="lstSecondary" runat="server" CssClass="default" />
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr id="panAppOwner" runat="server" visible="false">
                    <td nowrap>Application Owner:</td>
                    <td nowrap>
                        <table cellpadding="0" cellspacing="0" border="0">
                            <tr>
                                <td><asp:TextBox ID="txtAppOwner" runat="server" Width="300" CssClass="default" /></td>
                            </tr>
                            <tr>
                                <td>
                                    <div id="divAppOwner" runat="server" style="overflow:hidden; position:absolute; display:none; background-color:#FFFFFF; border:solid 1px #CCCCCC">
                                        <asp:ListBox ID="lstAppOwner" runat="server" CssClass="default" />
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr id="panEngineer" runat="server" visible="false">
                    <td nowrap>Network Engineer:</td>
                    <td nowrap>
                        <table cellpadding="0" cellspacing="0" border="0">
                            <tr>
                                <td><asp:TextBox ID="txtEngineer" runat="server" Width="300" CssClass="default" /></td>
                            </tr>
                            <tr>
                                <td>
                                    <div id="divEngineer" runat="server" style="overflow:hidden; position:absolute; display:none; background-color:#FFFFFF; border:solid 1px #CCCCCC">
                                        <asp:ListBox ID="lstEngineer" runat="server" CssClass="default" />
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td nowrap></td>
                    <td nowrap><asp:LinkButton ID="btnManager" runat="server" Text="User Not Appearing in a List? Click Here." /></td>
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
                            <asp:Button ID="btnBack" runat="server" Text="Back" CssClass="default" Width="75" /> <asp:Button ID="btnNext" runat="server" Text="Next" CssClass="default" Width="75" />
                        </asp:Panel>
                        <asp:Panel ID="panUpdate" runat="server" Visible="false">
                            <asp:Button ID="btnUpdate" runat="server" Text="Update" CssClass="default" Width="75" /> <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="default" Width="75" />
                        </asp:Panel>
                    </td>
                    <td align="right"><asp:Button ID="btnClose" runat="server" Text="Finish Later" CssClass="default" Width="125" /></td>
                </tr>
            </table>
        </td>
    </tr>
</table>
</asp:Content>
