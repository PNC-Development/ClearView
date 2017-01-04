<%@ Page Language="C#" MasterPageFile="~/frame.Master" AutoEventWireup="true" CodeBehind="cluster_instance_new.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.cluster_instance_new" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" Runat="Server">
<script type="text/javascript">
    function PathIsOK(oText) {
        var boolReturn = true;
        oText = document.getElementById(oText);
        var arrIllegal = new Array("","/","/USR","/VAR","/TMP","/HOME","/OPT","/OPT/PATROL","/EXPORT","/BOOT","/USR/LOCAL");
        if (oText.value.charAt(0) == '/') {
            for (var jj = 0; jj < arrIllegal.length; jj++) {
                if (arrIllegal[jj].toUpperCase() == oText.value.toUpperCase()) {
                    boolReturn = false;
                    alert(arrIllegal[jj].toUpperCase() + ' is an invalid path');
                    oText.focus();
                }
            }
        }
        else {
            boolReturn = false;
            alert('The path must start with \"/\"');
            oText.focus();
        }
        if (boolReturn == true) {
            var arrPaths = new Array(<%=strPaths %>);
            for (var jj = 0; jj < arrPaths.length; jj++) {
                if (arrPaths[jj].toUpperCase() == oText.value.toUpperCase()) {
                    boolReturn = false;
                    alert(arrPaths[jj].toUpperCase() + ' is already configured');
                    oText.focus();
                }
            }
        }
        return boolReturn;
    }
</script>
<table width="100%" height="100%" cellpadding="0" cellspacing="0" border="0">
    <tr>
        <td valign="top">
            <table width="100%" cellpadding="4" cellspacing="0" border="0">
                <asp:Panel ID="panName" runat="server" Visible="false">
                <tr>
                    <td colspan="2">Nickname:<font class="required">&nbsp;*</font></td>
                </tr>
                <tr>
                    <td><img src="/images/spacer.gif" border="0" width="30" height="1" /></td>
                    <td width="100%"><asp:TextBox ID="txtName" runat="server" CssClass="default" Width="200" MaxLength="50" /></td>
                </tr>
                </asp:Panel>
                <asp:Panel ID="panSQL" runat="server" Visible="false">
                <tr>
                    <td colspan="2">Will this instance be running SQL Server?<font class="required">&nbsp;*</font></td>
                </tr>
                <tr>
                    <td><img src="/images/spacer.gif" border="0" width="30" height="1" /></td>
                    <td width="100%">
                        <asp:RadioButton ID="radYes" runat="server" CssClass="default" Text="Yes" GroupName="sql" OnCheckedChanged="radYes_Check" AutoPostBack="true" /> 
                        <asp:RadioButton ID="radNo" runat="server" CssClass="default" Text="No" GroupName="sql" OnCheckedChanged="radNo_Check" AutoPostBack="true" />
                    </td>
                </tr>
                </asp:Panel>
            </table>
            <asp:Panel ID="panDatabase" runat="server" Visible="false">
                <table width="100%" cellpadding="4" cellspacing="0" border="0">
                    <tr>
                        <td colspan="2" class="biggerbold">SQL Server Storage Calculator (NCC)</td>
                    </tr>
                    <tr>
                        <td colspan="2">Enter database size in PROD:</td>
                    </tr>
                    <tr>
                        <td><img src="/images/spacer.gif" border="0" width="30" height="1" /></td>
                        <td width="100%"><asp:TextBox ID="txtSize" runat="server" CssClass="default" Width="100" MaxLength="10" /> GB</td>
                    </tr>
                    <tr>
                        <td colspan="2">Enter database size in QA:</td>
                    </tr>
                    <tr>
                        <td><img src="/images/spacer.gif" border="0" width="30" height="1" /></td>
                        <td width="100%"><asp:TextBox ID="txtQA" runat="server" CssClass="default" Width="100" MaxLength="10" /> GB</td>
                    </tr>
                    <tr>
                        <td colspan="2">Enter database size in TEST:</td>
                    </tr>
                    <tr>
                        <td><img src="/images/spacer.gif" border="0" width="30" height="1" /></td>
                        <td width="100%"><asp:TextBox ID="txtTest" runat="server" CssClass="default" Width="100" MaxLength="10" /> GB</td>
                    </tr>
                    <tr>
                        <td colspan="2">Will you require to store non-database data on the same instance?</td>
                    </tr>
                    <tr>
                        <td><img src="/images/spacer.gif" border="0" width="30" height="1" /></td>
                        <td width="100%">
                            <asp:RadioButton ID="radNonYes" runat="server" CssClass="default" Text="Yes" GroupName="non" /> 
                            <asp:RadioButton ID="radNonNo" runat="server" CssClass="default" Text="No" GroupName="non" /> 
                        </td>
                    </tr>
                </table>
                <div id="divNon" runat="server" style="display:none">
                <table width="100%" cellpadding="4" cellspacing="0" border="0">
                    <tr>
                        <td colspan="2">Enter amount of storage required to store non-database data:</td>
                    </tr>
                    <tr>
                        <td><img src="/images/spacer.gif" border="0" width="30" height="1" /></td>
                        <td width="100%"><asp:TextBox ID="txtNon" runat="server" CssClass="default" Width="100" MaxLength="10" /> GB</td>
                    </tr>
                    <tr>
                        <td colspan="2">Enter amount of storage required to store non-database data in QA:</td>
                    </tr>
                    <tr>
                        <td><img src="/images/spacer.gif" border="0" width="30" height="1" /></td>
                        <td width="100%"><asp:TextBox ID="txtNonQA" runat="server" CssClass="default" Width="100" MaxLength="10" /> GB</td>
                    </tr>
                    <tr>
                        <td colspan="2">Enter amount of storage required to store non-database data in TEST:</td>
                    </tr>
                    <tr>
                        <td><img src="/images/spacer.gif" border="0" width="30" height="1" /></td>
                        <td width="100%"><asp:TextBox ID="txtNonTest" runat="server" CssClass="default" Width="100" MaxLength="10" /> GB</td>
                    </tr>
                </table>
                </div>
                <table width="100%" cellpadding="4" cellspacing="0" border="0">
                    <tr>
                        <td colspan="2"><hr size="1" noshade /></td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <table width="100%" cellpadding="2" cellspacing="2" border="0">
                                <tr>
                                    <td class="required">* = Required Field</td>
                                    <td align="right">
                                        <asp:Button ID="btnGenerate" runat="server" Text="Save" CssClass="default" Width="75" OnClick="btnGenerate_Click" /> 
                                        <asp:Button ID="btnClose2" runat="server" Text="Close" CssClass="default" Width="75" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="panDatabasePNC" runat="server" Visible="false">
                <table width="100%" cellpadding="4" cellspacing="0" border="0">
                    <tr>
                        <td colspan="2" class="biggerbold">SQL Server Storage Calculator (PNC)</td>
                    </tr>
                    <tr>
                        <td colspan="2">Enter the total database size:</td>
                    </tr>
                    <tr>
                        <td><img src="/images/spacer.gif" border="0" width="30" height="1" /></td>
                        <td width="100%"><asp:TextBox ID="txtSizePNC" runat="server" CssClass="default" Width="100" MaxLength="10" /> GB</td>
                    </tr>
                    <tr>
                        <td colspan="2">What % of the total space is the largest Table and/or Index:</td>
                    </tr>
                    <tr>
                        <td><img src="/images/spacer.gif" border="0" width="30" height="1" /></td>
                        <td width="100%"><asp:TextBox ID="txtPercentPNC" runat="server" CssClass="default" Width="100" MaxLength="10" /> %&nbsp;&nbsp;&nbsp;<span class="footer">(0 - 100)</span></td>
                    </tr>
                    <tr>
                        <td colspan="2">Based on the amount and type of activity, please estimate TempDB:</td>
                    </tr>
                    <tr>
                        <td><img src="/images/spacer.gif" border="0" width="30" height="1" /></td>
                        <td width="100%"><asp:TextBox ID="txtTempPNC" runat="server" CssClass="default" Width="100" MaxLength="10" /> GB</td>
                    </tr>
                    <tr>
                        <td colspan="2">Will you require to store non-database data on the same instance?</td>
                    </tr>
                    <tr>
                        <td><img src="/images/spacer.gif" border="0" width="30" height="1" /></td>
                        <td width="100%">
                            <asp:RadioButton ID="radNonPNCYes" runat="server" CssClass="default" Text="Yes" GroupName="nonPNC" /> 
                            <asp:RadioButton ID="radNonPNCNo" runat="server" CssClass="default" Text="No (Default)" GroupName="nonPNC" /> 
                        </td>
                    </tr>
                </table>
                <div id="divNonPNC" runat="server" style="display:none">
                <table width="100%" cellpadding="4" cellspacing="0" border="0">
                    <tr>
                        <td colspan="2">Enter amount of storage required to store non-database data:</td>
                    </tr>
                    <tr>
                        <td><img src="/images/spacer.gif" border="0" width="30" height="1" /></td>
                        <td width="100%"><asp:TextBox ID="txtNonPNC" runat="server" CssClass="default" Width="100" MaxLength="10" /> GB</td>
                    </tr>
                </table>
                </div>
                <table width="100%" cellpadding="4" cellspacing="0" border="0">
                    <tr>
                        <td colspan="2"><hr size="1" noshade /></td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <table width="100%" cellpadding="2" cellspacing="2" border="0">
                                <tr>
                                    <td class="required">* = Required Field</td>
                                    <td align="right">
                                        <asp:Button ID="btnGeneratePNC" runat="server" Text="Save" CssClass="default" Width="75" OnClick="btnGeneratePNC_Click" /> 
                                        <asp:Button ID="btnClosePNC" runat="server" Text="Close" CssClass="default" Width="75" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="panDatabaseNo" runat="server" Visible="false">
                <table width="100%" cellpadding="4" cellspacing="0" border="0">
                    <tr>
                        <td colspan="2">Please select your Performance:<font class="required">&nbsp;*</font></td>
                    </tr>
                    <tr>
                        <td><img src="/images/spacer.gif" border="0" width="30" height="1" /></td>
                        <td width="100%">
                            <asp:DropDownList ID="ddlPerformance" runat="server" CssClass="default">
                                <asp:ListItem Value="-- SELECT --" />
                                <asp:ListItem Value="High" />
                                <asp:ListItem Value="Standard" />
                                <asp:ListItem Value="Low" />
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <asp:Panel ID="panFilesystem" runat="server" Visible="false">
                    <tr>
                        <td colspan="2">Filesystem:<font class="required">&nbsp;*</font></td>
                    </tr>
                    <tr>
                        <td><img src="/images/spacer.gif" border="0" width="30" height="1" /></td>
                        <td width="100%"><asp:TextBox ID="txtFilesystem" runat="server" CssClass="default" Width="200" MaxLength="50" /></td>
                    </tr>
                    </asp:Panel>
                    <tr>
                        <td colspan="2">Amount in Production:<font class="required">&nbsp;*</font></td>
                    </tr>
                    <tr>
                        <td><img src="/images/spacer.gif" border="0" width="30" height="1" /></td>
                        <td width="100%"><asp:TextBox ID="txtAmountProd" runat="server" CssClass="default" Width="100" MaxLength="8" /> GB</td>
                    </tr>
                    <tr>
                        <td colspan="2">Amount in QA:<font class="required">&nbsp;*</font></td>
                    </tr>
                    <tr>
                        <td><img src="/images/spacer.gif" border="0" width="30" height="1" /></td>
                        <td width="100%"><asp:TextBox ID="txtAmountQA" runat="server" CssClass="default" Width="100" MaxLength="8" /> GB</td>
                    </tr>
                    <tr>
                        <td colspan="2">Amount in Test:<font class="required">&nbsp;*</font></td>
                    </tr>
                    <tr>
                        <td><img src="/images/spacer.gif" border="0" width="30" height="1" /></td>
                        <td width="100%"><asp:TextBox ID="txtAmountTest" runat="server" CssClass="default" Width="100" MaxLength="8" /> GB</td>
                    </tr>
                    <tr>
                        <td colspan="2">Replicated:<font class="required">&nbsp;*</font></td>
                    </tr>
                    <tr>
                        <td><img src="/images/spacer.gif" border="0" width="30" height="1" /></td>
                        <td width="100%">
                            <asp:DropDownList ID="ddlReplicated" runat="server" CssClass="default">
                                <asp:ListItem Value="-- SELECT --" />
                                <asp:ListItem Value="No" />
                                <asp:ListItem Value="Yes" />
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">High Availability:<font class="required">&nbsp;*</font></td>
                    </tr>
                    <tr>
                        <td><img src="/images/spacer.gif" border="0" width="30" height="1" /></td>
                        <td width="100%">
                            <asp:DropDownList ID="ddlHigh" runat="server" CssClass="default">
                                <asp:ListItem Value="-- SELECT --" />
                                <asp:ListItem Value="No" />
                                <asp:ListItem Value="Yes" />
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">Do you want to add mount points to this drive?<font class="required">&nbsp;*</font></td>
                    </tr>
                    <tr>
                        <td><img src="/images/spacer.gif" border="0" width="30" height="1" /></td>
                        <td width="100%">
                            <asp:RadioButton ID="radMountYes" runat="server" CssClass="default" Text="Yes" GroupName="mount" /> 
                            <asp:RadioButton ID="radMountNo" runat="server" CssClass="default" Text="No" GroupName="mount" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2"><hr size="1" noshade /></td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <table width="100%" cellpadding="2" cellspacing="2" border="0">
                                <tr>
                                    <td class="required">* = Required Field</td>
                                    <td align="right">
                                        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="default" Width="75" OnClick="btnSave_Click" /> 
                                        <asp:Button ID="btnClose" runat="server" Text="Close" CssClass="default" Width="75" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="panMount" runat="server" Visible="false">
                <table width="100%" cellpadding="4" cellspacing="0" border="0">
                    <tr>
                        <td colspan="2" class="header">Mount Point #<%=strMount %></td>
                    </tr>
                    <tr>
                        <td colspan="2">Please select your Performance:<font class="required">&nbsp;*</font></td>
                    </tr>
                    <tr>
                        <td><img src="/images/spacer.gif" border="0" width="30" height="1" /></td>
                        <td width="100%">
                            <asp:DropDownList ID="ddlMountPerformance" runat="server" CssClass="default">
                                <asp:ListItem Value="-- SELECT --" />
                                <asp:ListItem Value="High" />
                                <asp:ListItem Value="Standard" />
                                <asp:ListItem Value="Low" />
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <asp:Panel ID="panMountFilesystem" runat="server" Visible="false">
                    <tr>
                        <td colspan="2">Filesystem:<font class="required">&nbsp;*</font></td>
                    </tr>
                    <tr>
                        <td><img src="/images/spacer.gif" border="0" width="30" height="1" /></td>
                        <td width="100%"><asp:TextBox ID="txtMountFilesystem" runat="server" CssClass="default" Width="200" MaxLength="50" /></td>
                    </tr>
                    </asp:Panel>
                    <tr>
                        <td colspan="2">Amount in Production:</td>
                    </tr>
                    <tr>
                        <td><img src="/images/spacer.gif" border="0" width="30" height="1" /></td>
                        <td width="100%"><asp:TextBox ID="txtMountProd" runat="server" CssClass="default" Width="100" MaxLength="8" /> GB</td>
                    </tr>
                    <tr>
                        <td colspan="2">Amount in QA:</td>
                    </tr>
                    <tr>
                        <td><img src="/images/spacer.gif" border="0" width="30" height="1" /></td>
                        <td width="100%"><asp:TextBox ID="txtMountQA" runat="server" CssClass="default" Width="100" MaxLength="8" /> GB</td>
                    </tr>
                    <tr>
                        <td colspan="2">Amount in Test:<font class="required">&nbsp;*</font></td>
                    </tr>
                    <tr>
                        <td><img src="/images/spacer.gif" border="0" width="30" height="1" /></td>
                        <td width="100%"><asp:TextBox ID="txtMountTest" runat="server" CssClass="default" Width="100" MaxLength="8" /> GB</td>
                    </tr>
                    <tr>
                        <td colspan="2">Replicated:<font class="required">&nbsp;*</font></td>
                    </tr>
                    <tr>
                        <td><img src="/images/spacer.gif" border="0" width="30" height="1" /></td>
                        <td width="100%">
                            <asp:DropDownList ID="ddlMountReplicated" runat="server" CssClass="default">
                                <asp:ListItem Value="-- SELECT --" />
                                <asp:ListItem Value="No" />
                                <asp:ListItem Value="Yes" />
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">High Availability:<font class="required">&nbsp;*</font></td>
                    </tr>
                    <tr>
                        <td><img src="/images/spacer.gif" border="0" width="30" height="1" /></td>
                        <td width="100%">
                            <asp:DropDownList ID="ddlMountHigh" runat="server" CssClass="default">
                                <asp:ListItem Value="-- SELECT --" />
                                <asp:ListItem Value="No" />
                                <asp:ListItem Value="Yes" />
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">Do you want to add another mount point?<font class="required">&nbsp;*</font></td>
                    </tr>
                    <tr>
                        <td><img src="/images/spacer.gif" border="0" width="30" height="1" /></td>
                        <td width="100%">
                            <asp:RadioButton ID="radMoreYes" runat="server" CssClass="default" Text="Yes" GroupName="more" /> 
                            <asp:RadioButton ID="radMoreNo" runat="server" CssClass="default" Text="No" GroupName="more" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2"><hr size="1" noshade /></td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <table width="100%" cellpadding="2" cellspacing="2" border="0">
                                <tr>
                                    <td class="required">* = Required Field</td>
                                    <td align="right">
                                        <asp:Button ID="btnMount" runat="server" Text="Save" CssClass="default" Width="75" OnClick="btnMount_Click" /> 
                                        <asp:Button ID="btnClose3" runat="server" Text="Close" CssClass="default" Width="75" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </td>
    </tr>
</table>
<asp:Label ID="lblDatabase" runat="server" Visible="false" />
</asp:Content>