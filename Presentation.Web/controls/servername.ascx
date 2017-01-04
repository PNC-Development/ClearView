<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="servername.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.servername" %>



<script type="text/javascript">
    function LocationFunction() {
        var strUrl = window.location.href;
        var oHidden = document.getElementById('<%=hdnParent.ClientID %>');
        if (strUrl.indexOf("?") > -1) {
            strQueryCheck = strUrl.substring(strUrl.indexOf("?"));
            strQueryUrl = strUrl.substring(0, strUrl.indexOf("?"));
            RemoveQuerystring("aid", oHidden.value);
            window.top.location.href = (strQueryUrl + strQueryCheck);
        }
        else {
            window.top.location.href = (strUrl + "?aid=" + oHidden.value);
        }
    }
    function EnsureFunction(oApp, oComp) {
        oApp = document.getElementById(oApp);
        oComp = document.getElementById(oComp);
        if (oApp.selectedIndex == 0 && oComp.value == "") {
            alert('Please select an application OR component');
            oApp.focus();
            return false;
        }
        else if (oApp.selectedIndex > 0 && oComp.value != "") {
            alert('Please select EITHER an application or component');
            oApp.focus();
            return false;
        }
        return true;
    }
    function ChangeDefaultName(oText) {
        if (oText.value == "Enter a new nickname HERE")
            oText.value = "";
    }
</script>
<table width="100%" cellpadding="0" cellspacing="0" border="0" class="default">
    <tr>
        <td><img src="/images/table_topleft.gif" border="0" width="5" height="26"></td>
        <td nowrap background="/images/table_top.gif" class="greentableheader" width="100%"><asp:Label ID="lblTitle" runat="server" CssClass="greentableheader" /></td>
        <td><img src="/images/table_topright.gif" border="0" width="5" height="26"></td>
    </tr>
    <tr>
        <td background="/images/table_left.gif"><img src="/images/table_left.gif" width="5" height="10"></td>
        <td width="100%" bgcolor="#FFFFFF">
            <br />
            <table width="100%" cellpadding="0" cellspacing="5" border="0">
                <tr>
                    <td rowspan="2"><img src="/images/servername.gif" border="0" align="absmiddle" /></td>
                    <td class="header" width="100%" valign="bottom">Server Name</td>
                </tr>
                <tr>
                    <td width="100%" valign="top">Generate your own server name by completing the following form and clicking <b>Generate</b>.</td>
                </tr>
            </table>
            <table width="100%" cellpadding="4" cellspacing="3" border="0">
                <tr>
                    <td colspan="2" align="center">
                        <asp:Panel ID="panName" runat="server" Visible="false">
                            <table cellpadding="2" cellspacing="2" border="0">
                                <tr>
                                    <td><img src="/images/bigCheck.gif" border="0" align="absmiddle" /></td>
                                    <td class="header">Server Name:</td>
                                    <td class="header"><asp:Label ID="lblName" runat="server" CssClass="header" /></td>
                                    <td><img src="/images/spacer.gif" border="0" width="10" height="1" /></td>
                                    <td><asp:Button ID="btnDuplicate" runat="server" CssClass="default" Text="Generate Another" Width="150" OnClick="btnDuplicate_Click" /></td>
                                </tr>
                                <tr><td colspan="3">&nbsp;</td></tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td nowrap>Please choose one of the following:</td>
                    <td width="100%">
                        <asp:RadioButtonList ID="radType" runat="server" CssClass="default" OnSelectedIndexChanged="radType_Change" AutoPostBack="true" RepeatDirection="Horizontal">
                            <asp:ListItem Value="NCB" Text="National City" />
                            <asp:ListItem Value="PNC" Text="PNC" />
                        </asp:RadioButtonList>
                    </td>
                </tr>
                <tr id="divNCB" runat="server" visible="false">
                    <td nowrap>Is the intended class of the server the Disaster Recovery, Cold Layer?</td>
                    <td width="100%">
                        <asp:RadioButtonList ID="radDR" runat="server" CssClass="default" OnSelectedIndexChanged="radDR_Change" AutoPostBack="true" RepeatDirection="Horizontal">
                            <asp:ListItem Value="Yes" Text="Yes" />
                            <asp:ListItem Value="No" Text="No" />
                        </asp:RadioButtonList>
                    </td>
                </tr>
                <tr id="divNCBYes" runat="server" visible="false">
                    <td colspan="2">
                        <br />
                        <p>For the Disaster Recovery, Cold Layer bound servers, there is no need to request a server name.  Use the corresponding server name with a suffix of <b>-DR</b> to generate the new name.</p>
                        <p>For example, if your corresponding server is called <b>OHCLEAPP1234</b>, the new server name will be <b>OHCLEAPP1234-DR</b>.</p>
                        <p>For additional information, contact your ClearView administrator.</p>
                    </td>
                </tr>
            </table>
            <div id="divContinue" runat="server" visible="false">
            <br />
            <table width="100%" cellpadding="4" cellspacing="3" border="0">
                <tr>
                    <td nowrap>Popular Locations:<font class="required">&nbsp;*</font></td>
                    <td>
                        <table cellpadding="0" cellspacing="0" border="0">
                            <tr>
                                <td><asp:DropDownList ID="ddlLocation" runat="server" CssClass="default" Width="300" AutoPostBack="true" OnSelectedIndexChanged="ddlLocation_Change" /></td>
                                <td class="bold">
                                    <div id="divLocation" runat="server" style="display:none">
                                        <img src="/images/spacer.gif" border="0" width="20" height="1" /><img src="/images/please_wait.gif" border="0" align="absmiddle" /> Please Wait...
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <asp:Panel ID="panLocation" runat="server" Visible="false">
                <tr>
                    <td nowrap>Custom Location:<font class="required">&nbsp;*</font></td>
                    <td><asp:TextBox ID="txtParent" CssClass="lightdefault" runat="server" Text="" Width="400" ReadOnly="true" />&nbsp;&nbsp;<asp:Button ID="btnParent" runat="server" CssClass="default" Width="25" Text="..." /> <span class="lightdefault"><img src="/images/hand_left.gif" border="0" align="absmiddle" /> Click here to select a value</span></td>
                </tr>
                </asp:Panel>
                <tr>
                    <td nowrap>Operating System:<font class="required">&nbsp;*</font></td>
                    <td>
                        <table cellpadding="0" cellspacing="0" border="0">
                            <tr>
                                <td><asp:DropDownList ID="ddlOS" runat="server" CssClass="default" Width="300" AutoPostBack="true" OnSelectedIndexChanged="ddlOS_Change" /></td>
                                <td class="bold">
                                    <div id="divOS" runat="server" style="display:none">
                                        <img src="/images/spacer.gif" border="0" width="20" height="1" /><img src="/images/please_wait.gif" border="0" align="absmiddle" /> Please Wait...
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td nowrap>Service Pack:<font class="required">&nbsp;*</font></td>
                    <td>
                        <table cellpadding="0" cellspacing="0" border="0">
                            <tr>
                                <td><asp:DropDownList ID="ddlSP" runat="server" CssClass="default" Width="300" AutoPostBack="true" OnSelectedIndexChanged="ddlSP_Change" /></td>
                                <td class="bold">
                                    <div id="divSP" runat="server" style="display:none">
                                        <img src="/images/spacer.gif" border="0" width="20" height="1" /><img src="/images/please_wait.gif" border="0" align="absmiddle" /> Please Wait...
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td nowrap>Class:<font class="required">&nbsp;*</font></td>
                    <td>
                        <table cellpadding="0" cellspacing="0" border="0">
                            <tr>
                                <td><asp:DropDownList ID="ddlClass" runat="server" CssClass="default" Width="300" AutoPostBack="true" OnSelectedIndexChanged="ddlClass_Change" /></td>
                                <td class="bold">
                                    <div id="divClass" runat="server" style="display:none">
                                        <img src="/images/spacer.gif" border="0" width="20" height="1" /><img src="/images/please_wait.gif" border="0" align="absmiddle" /> Please Wait...
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td nowrap>Environment:<font class="required">&nbsp;*</font></td>
                    <td>
                        <table cellpadding="0" cellspacing="0" border="0">
                            <tr>
                                <td><asp:DropDownList ID="ddlEnvironment" runat="server" CssClass="default" Width="300" AutoPostBack="true" OnSelectedIndexChanged="ddlEnvironment_Change" /></td>
                                <td class="bold">
                                    <div id="divEnvironment" runat="server" style="display:none">
                                        <img src="/images/spacer.gif" border="0" width="20" height="1" /><img src="/images/please_wait.gif" border="0" align="absmiddle" /> Please Wait...
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td nowrap>Model:<font class="required">&nbsp;*</font></td>
                    <td><asp:Label ID="lblModel" runat="server" CssClass="reddefault" /></td>
                </tr>
                <tr id="trModels" runat="server" visible="false">
                    <td colspan="2">
                        <div style="width:100%; height:250px; overflow:auto">
                            <asp:TreeView ID="oTreeModels" runat="server" CssClass="default" ShowLines="true" NodeIndent="30">
                            </asp:TreeView>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="2"><asp:Label ID="lblConfig" runat="server" CssClass="reddefault" Text="<img src='/images/spacer.gif' border='0' width='20' height='1' /><img src='/images/alert.gif' border='0' align='absmiddle' /> There is no information for this location and class" Visible="false" /></td>
                </tr>
                <tr>
                    <td colspan="2"><span style="width:100%;border-bottom:1 dotted #CCCCCC;"/><br /></td>
                </tr>
                <tr>
                    <td colspan="2"><img src="/images/spacer.gif" border="0" width="1" height="1" /></td>
                </tr>
                <tr>
                    <td colspan="2"><img src="/images/bigAlert.gif" border="0" align="absmiddle" /><b>NOTE:</b> You must choose EITHER a component or a server type!</td>
                </tr>
                <tr>
                    <td nowrap>Server Type:<font class="required">&nbsp;*</font></td>
                    <td nowrap><asp:DropDownList ID="ddlApplication" runat="server" CssClass="default" Width="300" /></td>
                </tr>
                <tr>
                    <td nowrap valign="top">Component(s):<font class="required">&nbsp;*</font></td>
                    <td>
                        <asp:Panel ID="panComponents" runat="server" Visible="false">
                            <iframe id="frmComponents" runat="server" frameborder="1" scrolling="no" width="730" height="250" />
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td nowrap>Clustering:<font class="required">&nbsp;*</font></td>
                    <td><asp:DropDownList ID="ddlCluster" runat="server" CssClass="default" Width="300" /></td>
                </tr>
                <tr id="trMnemonic" runat="server" visible="false">
                    <td nowrap>Mnemonic:<font class="required">&nbsp;*</font></td>
                    <td>
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
                <tr>
                    <td nowrap>Nickname:<font class="required">&nbsp;*</font></td>
                    <td><asp:TextBox ID="txtName" runat="server" CssClass="default" Width="300" MaxLength="50" /></td>
                </tr>
                <tr>
                    <td colspan="2"><hr size="1" noshade /></td>
                </tr>
                <tr>
                    <td nowrap class="required">* = Required Field</td>
                    <td align="right"><asp:Button ID="btnSubmit" runat="server" CssClass="default" Width="75" Text="Generate" OnClick="btnSubmit_Click" /></td>
                </tr>
            </table>
            </div>
            <p>&nbsp;</p>
        </td>
        <td background="/images/table_right.gif"><img src="/images/table_right.gif" width="5" height="10"></td>
    </tr>
    <tr>
        <td><img src="/images/table_bottomLeft.gif" border="0" width="5" height="9"></td>
        <td width="100%" background="/images/table_bottom.gif"></td>
        <td><img src="/images/table_bottomRight.gif" border="0" width="5" height="9"></td>
    </tr>
</table>
<input type="hidden" id="hdnParent" runat="server" />
<input type="hidden" id="hdnComponents" name="hdnComponents" />
<asp:HiddenField ID="hdnMnemonic" runat="server" />
