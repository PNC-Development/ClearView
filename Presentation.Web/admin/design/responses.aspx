<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="responses.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.design_responses" %>

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
		    <td><b>Design Responses</b></td>
		    <td align="right">&nbsp;</td>
		</tr>
		<tr><td colspan="2">&nbsp;</td></tr>
		<tr>
		    <td colspan="2" align="center">
                <asp:Panel ID="panView" runat="server" Visible="false">
                    <table width="100%" border="0" cellpadding="0" cellspacing="0">
                        <tr>
                            <td>
                            <asp:TreeView ID="oTreeResponses" runat="server" ShowLines="true" NodeIndent="35">
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
                                                <td class="default">Question:</td>
                                                <td><asp:DropDownList ID="ddlQuestion" CssClass="default" runat="server"/></td>
                                            </tr>
                                            <tr> 
                                                <td nowrap class="default">Admin Label:</td>
                                                <td><asp:TextBox ID="txtAdmin" CssClass="default" runat="server" Width="300" MaxLength="100"/></td>
                                            </tr>
                                            <tr>
                                                <td></td>
                                                <td class="footer"><img src="/images/down_right.gif" border="0" align="absmiddle" />The response shown on the admin configuration pages in ClearView.</td>
                                            </tr>
                                            <tr> 
                                                <td nowrap class="default" width="100px">Response:<br /><br />(Can be HTML)</td>
                                                <td><asp:TextBox ID="txtResponse" CssClass="default" runat="server" Width="300" Rows="5" TextMode="multiLine"/></td>
                                            </tr>
                                            <tr>
                                                <td></td>
                                                <td class="footer"><img src="/images/down_right.gif" border="0" align="absmiddle" />The response shown for the question.</td>
                                            </tr>
                                            <tr> 
                                                <td nowrap class="default">Summary Label:</td>
                                                <td><asp:TextBox ID="txtSummary" CssClass="default" runat="server" Width="300" MaxLength="100"/></td>
                                            </tr>
                                            <tr>
                                                <td></td>
                                                <td class="footer"><img src="/images/down_right.gif" border="0" align="absmiddle" />The response shown in the summary section of design builder.</td>
                                            </tr>
                                            <tr> 
                                                <td nowrap class="default">Is Confidence Lock:</td>
                                                <td><asp:CheckBox ID="chkConfidenceLock" runat="server" /> (If the client selects this response, the design will be locked)</td>
                                            </tr>
                                            <tr> 
                                                <td nowrap class="default">Is Confidence Unlock:</td>
                                                <td><asp:CheckBox ID="chkConfidenceUnlock" runat="server" />(If the client unlocks the design, this response will be selected)</td>
                                            </tr>
                                            <tr> 
                                                <td nowrap class="default">Is Exception:</td>
                                                <td><asp:CheckBox ID="chkException" runat="server" />(If the client selects this response, the design will be flagged for approval)</td>
                                            </tr>
                                            <tr> 
                                                <td nowrap class="default">Is Under 48:</td>
                                                <td><asp:CheckBox ID="chkUnder48" runat="server" Checked="true" />(Available for mnemonics with RPO under 48 hours)</td>
                                            </tr>
                                            <tr> 
                                                <td nowrap class="default">Is Over 48:</td>
                                                <td><asp:CheckBox ID="chkOver48" runat="server" Checked="true" />(Available for mnemonics with RPO over 48 hours)</td>
                                            </tr>
                                            <tr>
                                                <td nowrap class="default">Related Field:</td>
                                                <td><asp:DropDownList ID="ddlFields" runat="server" /> (The field in CV_DESIGNS table that gets updated)</td>
                                            </tr>
                                            <tr>
                                                <td></td>
                                                <td>
                                                    <b>NOTE:</b> The &quot;related field&quot; should only be set at the Response Level if each response to the question will be updating different fields.
                                                    <br />Otherwise, the &quot;related field&quot; should be configured at the Question Level (and the list will be disabled).
                                                </td>
                                            </tr>
                                            <tr> 
                                                <td nowrap class="default">Visible:</td>
                                                <td><asp:CheckBox ID="chkVisible" runat="server" Checked="true" /></td>
                                            </tr>
                                            <tr> 
                                                <td nowrap class="default">Select if One:</td>
                                                <td><asp:CheckBox ID="chkSelectIfOne" runat="server" /> (If this is the only response shown for the question, automatically select it)</td>
                                            </tr>
                                            <tr> 
                                                <td nowrap class="default">Acceptable Quantities:</td>
                                                <td><asp:TextBox ID="txtQuantityMin" runat="server" Width="75" MaxLength="10" /> to <asp:TextBox ID="txtQuantityMax" runat="server" Width="75" MaxLength="10" /> (Will cause a validation error when attempting to execute)</td>
                                            </tr>
                                            <tr>
                                                <td></td>
                                                <td class="footer"><img src="/images/down_right.gif" border="0" align="absmiddle" />Set to 0 to clear quantity restrictions.</td>
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
                                                <td colspan="2">&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td colspan="2"><b>Field Configuration</b> (** Response can be only one of the following...)</td>
                                            </tr>
                                            <tr> 
                                                <td nowrap class="default">** Set Class ID:</td>
                                                <td><asp:CheckBox ID="chkClass" runat="server" AutoPostBack="true" OnCheckedChanged="chkClass_Change" /></td>
                                            </tr>
                                            <tr id="trClass" runat="server" visible="false">
                                                <td class="default">Class:</td>
                                                <td><asp:DropDownList ID="ddlClass" runat="server" Width="300" /></td>
                                            </tr>
                                            <tr> 
                                                <td nowrap class="default">** Set OS ID:</td>
                                                <td><asp:CheckBox ID="chkOS" runat="server" AutoPostBack="true" OnCheckedChanged="chkOS_Change" /></td>
                                            </tr>
                                            <tr id="trOS" runat="server" visible="false">
                                                <td nowrap class="default">OS:</td>
                                                <td><asp:DropDownList ID="ddlOS" runat="server" Width="300" /></td>
                                            </tr>
                                            <tr> 
                                                <td nowrap class="default">** Set Component ID:</td>
                                                <td><asp:CheckBox ID="chkComponent" runat="server" AutoPostBack="true" OnCheckedChanged="chkComponent_Change" /></td>
                                            </tr>
                                            <tr id="trComponent1" runat="server" visible="false">
                                                <td nowrap class="default">Product:</td>
                                                <td><asp:DropDownList ID="ddlProduct" runat="server" Width="300" /></td>
                                            </tr>
                                            <tr id="trComponent2" runat="server" visible="false">
                                                <td nowrap class="default">Component:</td>
                                                <td>
                                                    <asp:DropDownList ID="ddlComponent" CssClass="default" runat="server" Width="300" Enabled="false" >
                                                        <asp:ListItem Value="-- Please select a Product --" />
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr> 
                                                <td nowrap class="default">** Set Environment ID:</td>
                                                <td><asp:CheckBox ID="chkEnvironment" runat="server" AutoPostBack="true" OnCheckedChanged="chkEnvironment_Change" /></td>
                                            </tr>
                                            <tr id="trEnvironment1" runat="server" visible="false">
                                                <td nowrap class="default">Class:</td>
                                                <td><asp:DropDownList ID="ddlEnvironmentClass" runat="server" Width="300" /></td>
                                            </tr>
                                            <tr id="trEnvironment2" runat="server" visible="false">
                                                <td nowrap class="default">Environment:</td>
                                                <td>
                                                    <asp:DropDownList ID="ddlEnvironment" CssClass="default" runat="server" Width="300" Enabled="false" >
                                                        <asp:ListItem Value="-- Please select an Environment --" />
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td nowrap class="default">** Related Value:</td>
                                                <td><asp:TextBox ID="txtValue" runat="server" Width="150" /> (OPTIONAL: The value to be set in the field in CV_DESIGNS table)</td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td colspan="2"><b>Additional Field Configuration</b> (Response can be any one of the following...)</td>
                                            </tr>
                                            <tr> 
                                                <td nowrap class="default">Set Location ID:</td>
                                                <td><asp:CheckBox ID="chkLocation" runat="server" AutoPostBack="true" OnCheckedChanged="chkLocation_Change" /></td>
                                            </tr>
                                            <tr id="trLocation" runat="server" visible="false">
                                                <td nowrap class="default">Location:</td>
                                                <td><%=strLocation %></td>
                                            </tr>
                                            <tr> 
                                                <td nowrap class="default">Set Model ID:</td>
                                                <td><asp:CheckBox ID="chkModel" runat="server" AutoPostBack="true" OnCheckedChanged="chkModel_Change" /></td>
                                            </tr>
                                            <tr id="trModel" runat="server" visible="false">
                                                <td nowrap class="default">Model:</td>
                                                <td><asp:DropDownList ID="ddlModel" runat="server" Width="300" /></td>
                                            </tr>
                                            <tr> 
                                                <td nowrap class="default">Set Infrastructure Component ID:</td>
                                                <td><asp:CheckBox ID="chkInfrastructure" runat="server" AutoPostBack="true" OnCheckedChanged="chkInfrastructure_Change" /></td>
                                            </tr>
                                            <tr id="trInfrastructure" runat="server" visible="false">
                                                <td valign="top" nowrap>Infrastructure Component:</td>
                                                <td valign="top" width="100%"><asp:DropDownList ID="ddlApplications" runat="server" CssClass="default" Width="300" /></td>
                                            </tr>
                                            <tr id="trInfrastructure2" runat="server" style="display:none">
                                                <td valign="top" nowrap>Infrastructure Role:</td>
                                                <td valign="top" width="100%"><asp:DropDownList ID="ddlSubApplications" runat="server" CssClass="default" Width="300" /></td>
                                            </tr>
                                        </table>
                                    </div>
                                    <div style="display:none">
                                        <%=strDisabled %>
                                        <p>Select all phases that should be disabled / enabled when this response is selected:</p>
                                        <p><b>NOTE:</b> Only phases AFTER this phase can be selected.</p>
                                        <table width="100%" cellpadding="3" cellspacing="2" border="0">
                                            <tr>
                                                <td width="50%"><b>Disabled Phase(s):</b></td>
                                                <td width="50%"><b>Enabled Phase(s):</b></td>
                                            </tr>
                                            <tr>
                                                <td width="50%">
                                                    <asp:TreeView ID="oTreeRestrictionsDisabled" runat="server" CssClass="default" ShowLines="true" NodeIndent="30">
                                                    </asp:TreeView>
                                                </td>
                                                <td width="50%">
                                                    <asp:TreeView ID="oTreeRestrictionsEnabled" runat="server" CssClass="default" ShowLines="true" NodeIndent="30">
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
                                    <div style="display:none">
                                        <%=strDisabled %>
                                        <p>When this response is selected, the following responses should automatically be selected:</p>
                                        <p><b>WARNING:</b> Be careful when configuring auto-responses...misconfiguration can lead to an &quot;infinite loop&quot; (Example: A -> B.  B -> C.  C -> A. Repeat.)</p>
                                        <asp:TreeView ID="oTreeSelections" runat="server" CssClass="default" ShowLines="true" NodeIndent="30">
                                        </asp:TreeView>
                                    </div>
                                    <div style="display:none">
                                        <%=strDisabled %>
                                        <p>The following questions will appear ONLY when this response has been selected:</p>
                                        <p><b>NOTE:</b> Only questions related to phases AFTER this phase can be selected.</p>
                                        <asp:TreeView ID="oTreeQuestions" runat="server" CssClass="default" ShowLines="true" NodeIndent="30">
                                        </asp:TreeView>
                                    </div>
                                    <div style="display:none">
                                        <%=strDisabled %>
                                        <p><b>NOTE:</b> Only respones related to phases BEFORE this phase can be selected.</p>
                                        <table width="100%" cellpadding="3" cellspacing="2" border="0">
                                            <tr>
                                                <td width="50%">Only <b>SHOW</b> this response when ...</td>
                                                <td width="50%">Only <b>HIDE</b> this response when ...</td>
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
                                                    <asp:TreeView ID="oTreeResponsesShow" runat="server" CssClass="default" ShowLines="true" NodeIndent="30">
                                                    </asp:TreeView>
                                                </td>
                                                <td width="50%">
                                                    <asp:TreeView ID="oTreeResponsesHide" runat="server" CssClass="default" ShowLines="true" NodeIndent="30">
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
                                    <div style="display:none">
                                        <%=strDisabled %>
                                        <p>The following approval groups will be required to approve a design when this response is selected:</p>
                                        <p><b>NOTE:</b> Do NOT get this approval confused with overall design approvals.  They require a different configuration.</p>
                                        <asp:TreeView ID="oTreeApprovals" runat="server" CssClass="default" ShowLines="false" NodeIndent="30">
                                        </asp:TreeView>
                                    </div>
                                    <div style="display:none">
                                        <%=strDisabled %>
                                        <asp:Literal ID="litConfiguration" runat="server" />
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
<input type="hidden" id="hdnEnvironment" runat="server" />
<input type="hidden" id="hdnLocation" runat="server" />
<input type="hidden" id="hdnComponent" runat="server" />
<input type="hidden" id="hdnSubApplication" runat="server" />
</form>
</body>
</html>
