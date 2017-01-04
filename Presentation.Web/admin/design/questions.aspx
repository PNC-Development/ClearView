<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="questions.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.design_questions" %>

<script type="text/javascript">
</script>
<html>
<head>
<title>ClearView Administration</title>
<link rel="stylesheet" type="text/css" href="/css/default.css" />
<script type="text/javascript" src="/javascript/global.js"></script>
<script type="text/javascript" src="/javascript/both.js"></script>
<script type="text/javascript" src="/javascript/treeview.js"></script>
</head>
<body topmargin="0" leftmargin="0">
<form id="Form1" runat="server">
        <table width="100%" height="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
        <td height="100%">
        <div style="height:100%; overflow:auto">
<table width="98%" cellpadding="0" cellspacing="0" border="0" align="center">
		<tr><td colspan="2">&nbsp;</td></tr>
		<tr> 
		    <td><b>Design Questions</b></td>
		    <td align="right">&nbsp;</td>
		</tr>
		<tr><td colspan="2">&nbsp;</td></tr>
		<tr>
		    <td colspan="2" align="center">
                <asp:Panel ID="panView" runat="server" Visible="false">
                    <table width="100%" border="0" cellpadding="0" cellspacing="0">
                        <tr>
                            <td>
                            <asp:TreeView ID="oTree" runat="server" ShowLines="true" NodeIndent="35">
                                <NodeStyle CssClass="default" />
                            </asp:TreeView>
                            </td>
                        </tr>
                    </table>
               </asp:Panel>

                <asp:Panel id="panAdd" runat="server" Visible="false">
                    <table width="100%" border="0" cellpadding="0" cellspacing="0">
                        <tr>
                            <td width="100%" class="biggerbold">&quot;<asp:Label ID="lblName" runat="server" />&quot;</td>
                            <td nowrap align="right">
                                <asp:button ID="btnAddBack" CssClass="default" runat="server" Text="Add & Return" Width="100" OnClick="btnAddBack_Click" />
                                <asp:button ID="btnAdd" CssClass="default" runat="server" Text="Add" Width="75" OnClick="btnAdd_Click" />
                                <asp:button ID="btnDelete" CssClass="default" runat="server" Text="Delete" Width="75" OnClick="btnDelete_Click" />
                                <asp:button ID="btnCancel" CssClass="default" runat="server" Text="Cancel" Width="75" OnClick="btnCancel_Click" />
                            </td>
                        </tr>
                        <tr ID="trSave" runat="server" Visible="false">
                            <td colspan="2" align="center">
                                <table>
                                    <tr>
                                        <td class="bigCheck"><img src="/images/bigCheck.gif" border="0" align="absmiddle" />&nbsp;Information&nbsp;has&nbsp;been&nbsp;Updated</td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">&nbsp;</td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <%=strMenuTab1 %>
                                <div id="divMenu1">
                                    <br />
                                    <div style="display:none">
                                        <table width="95%" cellpadding="3" cellspacing="0" border="0" align="center">
                                            <tr> 
                                                <td nowrap class="default" width="100px">Question:<br /><br />(Can be HTML)</td>
                                                <td><asp:TextBox ID="txtQuestion" CssClass="default" runat="server" Width="400" Rows="5" TextMode="multiLine"/></td>
                                            </tr>
                                            <tr> 
                                                <td nowrap class="default">Phase:</td>
                                                <td><asp:DropDownList ID="ddlPhase" CssClass="default" runat="server"/></td>
                                            </tr>
                                            <tr> 
                                                <td nowrap class="default">Summary Label:</td>
                                                <td><asp:TextBox ID="txtSummary" CssClass="default" runat="server" Width="300" MaxLength="100"/></td>
                                            </tr>
                                            <tr> 
                                                <td nowrap class="default">Show in Summary:</td>
                                                <td><asp:CheckBox ID="chkSummary" runat="server" /></td>
                                            </tr>
                                            <tr>
                                                <td nowrap class="default">Related Field:</td>
                                                <td><asp:DropDownList ID="ddlFields" runat="server" /> (The field in CV_DESIGNS table that gets updated)</td>
                                            </tr>
                                            <tr>
                                                <td></td>
                                                <td>
                                                    <b>NOTE:</b> The &quot;related field&quot; should only be set at the Question Level if ALL of the responses will be updating the SAME field.
                                                    <br />Otherwise, the &quot;related field&quot; should be configured at the Response Level.
                                                </td>
                                            </tr>
                                            <tr>
                                                <td nowrap class="default">Default Value:</td>
                                                <td><asp:TextBox ID="txtValue" runat="server" Width="150" /> (OPTIONAL: The value to be set in the field in CV_DESIGNS table)</td>
                                            </tr>
                                            <tr>
                                                <td nowrap class="default">Suffix:</td>
                                                <td><asp:TextBox ID="txtSuffix" runat="server" Width="150" MaxLength="50" /></td>
                                            </tr>
                                            <tr> 
                                                <td nowrap class="default">Allow Empty / Blank:</td>
                                                <td><asp:CheckBox ID="chkEmpty" runat="server" Checked="true" /></td>
                                            </tr>
                                            <tr> 
                                                <td nowrap class="default">Enabled:</td>
                                                <td><asp:CheckBox ID="chkEnabled" runat="server" Checked="true" /></td>
                                            </tr>
                                            <tr> 
                                                <td class="cmdefault">&nbsp;</td>
                                                <td><asp:Button ID="btnOrder" runat="server" Text="Change Order" Width="150" CssClass="default" /></td>
                                            </tr>
                                            <tr> 
                                                <td></td>
                                                <td>
                                                    <table cellpadding="5" cellspacing="5" border="0">
                                                        <tr>
                                                            <td valign="top">
                                                                <fieldset>
                                                                    <legend>Hard-coded Defaults:</legend>
                                                                    <table cellpadding="5" cellspacing="0" border="0">
                                                                        <tr> 
                                                                            <td><asp:RadioButton ID="chkMnemonic" runat="server" Text="Is Mnemonic" GroupName="defaults" /></td>
                                                                        </tr>
                                                                        <tr> 
                                                                            <td><asp:RadioButton ID="chkCostCenter" runat="server" Text="Is Cost Center" GroupName="defaults" /></td>
                                                                        </tr>
                                                                        <tr> 
                                                                            <td><asp:RadioButton ID="chkUserSI" runat="server" Text="Is User = SI" GroupName="defaults" /></td>
                                                                        </tr>
                                                                        <tr> 
                                                                            <td><asp:RadioButton ID="chkUserDTG" runat="server" Text="Is User = DTG" GroupName="defaults" /></td>
                                                                        </tr>
                                                                        <tr> 
                                                                            <td><asp:RadioButton ID="chkGridBackup" runat="server" Text="Is Grid (Backup)" GroupName="defaults" /></td>
                                                                        </tr>
                                                                        <tr> 
                                                                            <td><asp:RadioButton ID="chkBackupExclusions" runat="server" Text="Is Backup Exclusions" GroupName="defaults" /></td>
                                                                        </tr>
                                                                        <tr> 
                                                                            <td><asp:RadioButton ID="chkGridMaintenance" runat="server" Text="Is Grid (Maintenance)" GroupName="defaults" /></td>
                                                                        </tr>
                                                                        <tr> 
                                                                            <td><asp:RadioButton ID="chkStorageLuns" runat="server" Text="Is Storage (LUNs)" GroupName="defaults" /></td>
                                                                        </tr>
                                                                        <tr> 
                                                                            <td><asp:RadioButton ID="chkAccounts" runat="server" Text="Is Accounts" GroupName="defaults" /></td>
                                                                        </tr>
                                                                        <tr> 
                                                                            <td><asp:RadioButton ID="chkDate" runat="server" Text="Is Date" GroupName="defaults" /></td>
                                                                        </tr>
                                                                        <tr> 
                                                                            <td><asp:RadioButton ID="chkLocation" runat="server" Text="Is Location" GroupName="defaults" /></td>
                                                                        </tr>
                                                                        <tr> 
                                                                            <td><asp:RadioButton ID="chkDefaults" runat="server" Text="NONE" GroupName="defaults" /></td>
                                                                        </tr>
                                                                    </table>
                                                                </fieldset>
                                                            </td>
                                                            <td valign="top">
                                                                <fieldset>
                                                                    <legend>Configuration:</legend>
                                                                    <table cellpadding="5" cellspacing="0" border="0">
                                                                        <tr> 
                                                                            <td><asp:RadioButton ID="chkDropDown" runat="server" Text="Is Drop Down" GroupName="configs" /></td>
                                                                        </tr>
                                                                        <tr> 
                                                                            <td><asp:RadioButton ID="chkCheckBox" runat="server" Text="Is Check Box" GroupName="configs" /></td>
                                                                        </tr>
                                                                        <tr> 
                                                                            <td><asp:RadioButton ID="chkRadio" runat="server" Text="Is Radio" GroupName="configs" /></td>
                                                                        </tr>
                                                                        <tr> 
                                                                            <td><asp:RadioButton ID="chkTextBox" runat="server" Text="Is Text Box" GroupName="configs" /></td>
                                                                        </tr>
                                                                        <tr> 
                                                                            <td><asp:RadioButton ID="chkTextArea" runat="server" Text="Is Text Area" GroupName="configs" /></td>
                                                                        </tr>
                                                                        <tr> 
                                                                            <td><asp:RadioButton ID="chkConfigs" runat="server" Text="NONE" GroupName="configs" /></td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>&nbsp;</td>
                                                                        </tr>
                                                                    </table>
                                                                </fieldset>
                                                                <p><b>Drop Down</b> <u>should</u> be used if all the responses update the SAME field.</p>
                                                                <p><b>Radio</b> <u>should</u> be used if each response updates a SEPARATE field.</p>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <div style="display:none">
                                        <%=strDisabled %>
                                        <p><b>NOTE:</b> Only questions related to phases BEFORE this phase can be selected.</p>
                                        <table width="100%" cellpadding="3" cellspacing="2" border="0">
                                            <tr>
                                                <td width="50%">Only <b>SHOW</b> this question when ...</td>
                                                <td width="50%">Only <b>HIDE</b> this question when ...</td>
                                            </tr>
                                            <tr>
                                                <td width="50%">
                                                    <asp:RadioButton ID="radShowAny" runat="server" Text="Any" GroupName="Show" /> 
                                                    <asp:RadioButton ID="radShowAll" runat="server" Text="All" GroupName="Show" /> 
                                                    <asp:RadioButton ID="radShow" runat="server" Text="Always Shown" GroupName="Show" /> 
                                                </td>
                                                <td width="50%">
                                                    <asp:RadioButton ID="radHideAny" runat="server" Text="Any" GroupName="Hide" /> 
                                                    <asp:RadioButton ID="radHideAll" runat="server" Text="All" GroupName="Hide" /> 
                                                    <asp:RadioButton ID="radHide" runat="server" Text="Always Shown" GroupName="Hide" /> 
                                                </td>
                                            </tr>
                                            <tr>
                                                <td width="50%">... of the following are selected (hidden by default):</td>
                                                <td width="50%">... of the following are selected (shown by default):</td>
                                            </tr>
                                            <tr>
                                                <td width="50%">
                                                    <asp:TreeView ID="oTreeQuestionsShow" runat="server" CssClass="default" ShowLines="true" NodeIndent="30">
                                                    </asp:TreeView>
                                                </td>
                                                <td width="50%">
                                                    <asp:TreeView ID="oTreeQuestionsHide" runat="server" CssClass="default" ShowLines="true" NodeIndent="30">
                                                    </asp:TreeView>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td width="50%"></td>
                                                <td width="50%"><p><b>NOTE:</b> Will be disabled to start.</p></td>
                                            </tr>
                                        </table>
                                        <p></p>
                                    </div>
                                </div>
                            </td>
                        </tr>
                    </table>
                    <p>&nbsp;</p>
                </asp:Panel>
            </td>
        </tr>
    </table>
        </div>
        </td>
        </tr>
        </table>
    <input type="hidden" id="hdnId" runat="server" />
    <input type="hidden" id="hdnOrder" runat="server" />
</form>
</body>
</html>
