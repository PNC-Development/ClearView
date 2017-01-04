<%@ Page Language="C#" MasterPageFile="~/window.Master" AutoEventWireup="true" CodeBehind="config_server.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.config_server" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" Runat="Server">
<script type="text/javascript">
    function EnsureDR(oDRYes, oDRNo, oExistYes, oExistNo, oExist, oConsYes, oConsNo, oCons) {
        if (ValidateRadioButtons(oDRYes, oDRNo, 'Please choose if this server has a DR counterpart') == false)
            return false;
        else {
            oDRYes = document.getElementById(oDRYes);
            if (oDRYes.checked == true) {
                if (ValidateRadioButtons(oExistYes, oExistNo, 'Please choose if the recovery server already exists') == false)
                    return false;
                else {
                    oExistYes = document.getElementById(oExistYes);
                    if (oExistYes.checked == true) {
                        if (ValidateText(oExist, 'Please enter the name of the recovery server') == false)
                            return false;
                    }
                    if (ValidateRadioButtons(oConsYes, oConsNo, 'Please choose if the server needs to be a part of any consistency groups') == false)
                        return false;
                    else {
                        oConsYes = document.getElementById(oConsYes);
                        if (oConsYes.checked == true) {
                            if (ValidateHidden0(oCons, oConsNo, 'Please select a consistency group') == false)
                                return false;
                        }
                    }
                }
            }
        }
        return true;
    }
</script>
<table width="100%" height="100%" cellpadding="0" cellspacing="0" border="0">
    <tr height="1">
        <td>
            <table width="100%" cellpadding="0" cellspacing="5" border="0">
                <tr>
                    <td rowspan="2"><img src="/images/device_config.gif" border="0" align="middle" /></td>
                    <td class="header" width="100%" valign="bottom">Device Configuration</td>
                </tr>
                <tr>
                    <td width="100%" valign="top">Prepare your device for build by answering the following questions.</td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td valign="top">
            <%=strMenuTab1 %>
            <div id="divMenu1">
                <br />
                <div style="display:none">
                    <div style="height:100%; overflow:auto">
                    <table width="100%" cellpadding="4" cellspacing="0" border="0">
                        <tr>
                            <td nowrap>Operating System:<font class="required">&nbsp;*</font></td>
                            <td width="100%"><asp:DropDownList ID="ddlOS" runat="server" CssClass="default" Width="250" AutoPostBack="true" OnSelectedIndexChanged="ddlOS_Change" /></td>
                        </tr>
                        <asp:Panel ID="panSP" runat="server" Visible="false">
                        <tr>
                            <td nowrap>Service Pack Level:<font class="required">&nbsp;*</font></td>
                            <td width="100%"><asp:DropDownList ID="ddlServicePack" runat="server" CssClass="default" Width="250" /></td>
                        </tr>
                        </asp:Panel>
                        <asp:Panel ID="panMaintenance" runat="server" Visible="false">
                        <tr>
                            <td nowrap>Maintenance Level:<font class="required">&nbsp;*</font></td>
                            <td width="100%"><asp:DropDownList ID="ddlMaintenance" runat="server" CssClass="default" Width="250" /></td>
                        </tr>
                        </asp:Panel>
                        <asp:Panel ID="panTemplate" runat="server" Visible="false">
                        <tr>
                            <td nowrap>VMWare Template:<font class="required">&nbsp;*</font></td>
                            <td width="100%"><asp:DropDownList ID="ddlTemplate" runat="server" CssClass="default" Width="250" /></td>
                        </tr>
                        </asp:Panel>
                        <tr id="divDBA" runat="server" style="display:none">
                            <td nowrap>Database Administrator:<font class="required">&nbsp;*</font></td>
                            <td width="100%">
                                <table cellpadding="0" cellspacing="0" border="0">
                                    <tr>
                                        <td><asp:TextBox ID="txtUser" runat="server" Width="250" CssClass="default" /></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div id="divAJAX" runat="server" style="overflow:hidden; position:absolute; display:none; background-color:#FFFFFF; border:solid 1px #CCCCCC">
                                                <asp:ListBox ID="lstAJAX" runat="server" CssClass="default" />
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td nowrap>Domain:<font class="required">&nbsp;*</font></td>
                            <td width="100%"><asp:DropDownList ID="ddlDomain" runat="server" CssClass="default" Width="250" /></td>
                        </tr>
                        <asp:Panel ID="panTest" runat="server" Visible="false">
                        <tr>
                            <td nowrap>Test Domain:<font class="required">&nbsp;*</font></td>
                            <td width="100%"><asp:DropDownList ID="ddlTestDomain" runat="server" CssClass="default" Width="250" /></td>
                        </tr>
                        </asp:Panel>
                        <asp:Panel ID="panInfrastructure" runat="server" Visible="false">
                        <tr>
                            <td nowrap>Is this an infrastructure server?<font class="required">&nbsp;*</font></td>
                            <td width="100%">
                                <asp:RadioButton ID="radInfrastructureYes" runat="server" CssClass="default" Text="Yes" GroupName="INF" /> 
                                <asp:RadioButton ID="radInfrastructureNo" runat="server" CssClass="default" Text="No" GroupName="INF" />
                            </td>
                        </tr>
                        </asp:Panel>
                        <asp:Panel ID="panCluster" runat="server" Visible="true">
                        <tr>
                            <td nowrap>Will this server have an HA counterpart?<font class="required">&nbsp;*</font></td>
                            <td width="100%">
                                <asp:RadioButton ID="radHAYes" runat="server" CssClass="default" Text="Yes" GroupName="HA" /> 
                                <asp:RadioButton ID="radHANo" runat="server" CssClass="default" Text="No" GroupName="HA" />
                            </td>
                        </tr>
                        <tr>
                            <td nowrap>Will this server have a DR counterpart?<font class="required">&nbsp;*</font></td>
                            <td width="100%">
                                <asp:RadioButton ID="radDRYes" runat="server" CssClass="default" Text="Yes" GroupName="DR" /> 
                                <asp:RadioButton ID="radDRNo" runat="server" CssClass="default" Text="No" GroupName="DR" />
                            </td>
                        </tr>
                        <tr id="divDR1" runat="server" style="display:none">
                            <td nowrap>Does the recovery server already exist?<font class="required">&nbsp;*</font></td>
                            <td width="100%">
                                <asp:RadioButton ID="radExistYes" runat="server" CssClass="default" Text="Yes" GroupName="Exist" /> 
                                <asp:RadioButton ID="radExistNo" runat="server" CssClass="default" Text="No" GroupName="Exist" />
                            </td>
                        </tr>
                        <tr id="divExist" runat="server" style="display:none">
                            <td nowrap>What is the name of the recovery server?<font class="required">&nbsp;*</font></td>
                            <td width="100%"><asp:TextBox ID="txtRecovery" runat="server" CssClass="default" Width="150" MaxLength="30" />&nbsp;&nbsp;&nbsp;<span class="reddefault"><b>NOTE:</b> Only for Warm DR.</span></td>
                        </tr>
                        <tr id="divDR2" runat="server" style="display:none">
                            <td nowrap>Does this server need to be a part of any consistency groups?<font class="required">&nbsp;*</font></td>
                            <td width="100%">
                                <asp:RadioButton ID="radConsistencyYes" runat="server" CssClass="default" Text="Yes" GroupName="Consistency" /> 
                                <asp:RadioButton ID="radConsistencyNo" runat="server" CssClass="default" Text="No" GroupName="Consistency" />
                            </td>
                        </tr>
                        <tr id="divConsistency1" runat="server" style="display:none">
                            <td nowrap>Consistency Group Name:<font class="required">&nbsp;*</font></td>
                            <td width="100%"><asp:TextBox ID="txtConsistencyGroup" runat="server" CssClass="biggerreddefault" Width="250" Text="Not Specified" ReadOnly="true" /></td>
                        </tr>
                        <tr id="divConsistency2" runat="server" style="display:none">
                            <td>
                                <table cellpadding="2" cellspacing="2" border="0">
                                    <tr>
                                        <td width="100%" align="right" class="bold">
                                            <table width="100%" cellpadding="0" cellspacing="5" border="0">
                                                <tr>
                                                    <td><img src="/images/bigAlert.gif" border="0" align="middle" /></td>
                                                    <td class="bold" width="100%">Configure your consistency group by selecting one of the following options...</td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td nowrap>
                                            <table cellpadding="0" cellspacing="0" border="0">
                                                <tr>
                                                    <td><img src="/images/box_top_left.gif" border="0" width="6" height="6"></td>
                                                    <td width="100%" background="/images/box_top.gif"></td>
                                                </tr>
                                                <tr>
                                                    <td background="/images/box_left.gif"><img src="/images/box_left.gif" width="6" height="6"></td>
                                                    <td width="100%"><img src="/images/spacer.gif" width="6" height="35"></td>
                                                </tr>
                                                <tr>
                                                    <td><img src="/images/box_bottom_left.gif" border="0" width="6" height="6"></td>
                                                    <td width="100%" background="/images/box_bottom.gif"></td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td>
                                <table width="100%" cellpadding="0" cellspacing="3" border="0">
                                    <tr>
                                        <td><asp:LinkButton ID="btnConsistencyServer" runat="server" Text="Find a consistency group by server name" /></td>
                                    </tr>
                                    <tr>
                                        <td><asp:LinkButton ID="btnConsistencyName" runat="server" Text="Select a consistency group" /></td>
                                    </tr>
                                    <tr>
                                        <td><asp:LinkButton ID="btnConsistencyNew" runat="server" Text="Add a new consistency group" /></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        </asp:Panel>
                        <tr>
                            <td nowrap><img src="/images/spacer.gif" border="0" width="375" height="1" /></td>
                            <td width="100%">&nbsp;</td>
                        </tr>
                    </table>
                    </div>
                </div>
                <div style="display:none">
                    <asp:Panel ID="panComponents" runat="server" Visible="false">
                        <iframe id="frmComponents" runat="server" frameborder="1" scrolling="no" width="730" height="450" />
                    </asp:Panel>
                    <asp:Panel ID="panComponentsNo" runat="server" Visible="false">
                        <table width="100%" cellpadding="0" cellspacing="5" border="0">
                            <tr>
                                <td rowspan="2"><img src="/images/bigAlert.gif" border="0" align="middle" /></td>
                                <td class="header" width="100%" valign="bottom">Your Server has not been Configured!</td>
                            </tr>
                            <tr>
                                <td width="100%" valign="top">Please go to the other tab and answer the questions before you attempt to add additional components.</td>
                            </tr>
                        </table>
                    </asp:Panel>
                </div>
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
                    </td>
                    <td align="right">
                        <asp:Button ID="btnSaveConfig" runat="server" Text="Save" CssClass="default" Width="75" OnClick="btnSaveConfig_Click" /> 
                        <asp:Button ID="btnClose" runat="server" Text="Close" CssClass="default" Width="75" />
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
<asp:Label ID="lblId" runat="server" Visible="false" />
<asp:Label ID="lblLun" runat="server" Visible="false" />
<asp:HiddenField ID="hdnConsistencyGroup" runat="server" />
<asp:HiddenField ID="hdnUser" runat="server" />
<input type="hidden" id="hdnComponents" name="hdnComponents" />
</asp:Content>
