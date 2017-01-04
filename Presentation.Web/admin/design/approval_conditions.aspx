<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="approval_conditions.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.approval_conditions" %>



<html>
<head>
<title>ClearView Administration</title>
<link rel="stylesheet" type="text/css" href="/css/default.css" />
<script type="text/javascript" src="/javascript/global.js"></script>
<script type="text/javascript" src="/javascript/both.js"></script>
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
		    <td><b>Design Approval Conditional</b></td>
		    <td align="right"><asp:LinkButton id="btnNew" runat="server" Text="Add New" OnClick="btnNew_Click" /></td>
		</tr>
		<tr><td colspan="2">&nbsp;</td></tr>
		<tr>
		    <td colspan="2" align="center">
                <asp:Panel ID="panView" runat="server" Visible="false">
                    <table width="100%" border="0" cellpadding="0" cellspacing="0">
                        <tr>
                            <td>
                                <asp:repeater ID="rptView" runat="server">
                                    <HeaderTemplate>
                                        <table width="100%" cellpadding="3" cellspacing="0" border="0" align="center">
                                            <tr bgcolor="#CCCCCC">
                                                <td class="bold">&nbsp;</td>
                                                <td width="50%" class="bold"><asp:linkbutton ID="lnkName" Text="Name" CommandArgument="name" CssClass="bold" OnClick="OrderView" runat="server"/></td>
                                                <td width="50%" class="bold"><asp:linkbutton ID="lnkModified" Text="Last Modified" CommandArgument="modified" CssClass="bold" OnClick="OrderView" runat="server"/></td>
                                                <td class="bold" align="center"><asp:linkbutton ID="lnkEnabled" Text="Enabled" CommandArgument="enabled" CssClass="bold" OnClick="OrderView" runat="server"/></td>
                                            </tr>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr bgcolor="#EFEFEF" onmouseover="AdminMouseOver(this);" onmouseout="AdminMouseOut(this);" onclick="window.navigate('<%# Request.Path + "?id=" + DataBinder.Eval(Container.DataItem, "id")%>');" class="default">
                                            <td width="5%" align="left">&nbsp;<asp:ImageButton ID="btnDelete" OnClick="btnDeleteLink_Click" runat="server" ToolTip="Delete" ImageAlign="AbsMiddle" ImageUrl="/admin/images/delete.gif" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' /></td>
                                            <td width="50%">&nbsp;<%# DataBinder.Eval(Container.DataItem, "name") %></td>
                                            <td width="50%">&nbsp;<%# DataBinder.Eval(Container.DataItem, "modified") %></td>
                                            <td align="center"><asp:ImageButton ID="btnEnable" runat="server" ImageAlign="AbsMiddle" ImageUrl='<%# (DataBinder.Eval(Container.DataItem, "enabled").ToString() == "1") ? "/admin/images/enabled.gif" : "/admin/images/disabled.gif" %>' CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' OnClick="btnEnable_Click" /></td>
                                        </tr>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </table>
                                    </FooterTemplate>
                                </asp:repeater>
                            </td>
                        </tr>
                    </table>
               </asp:Panel>

                <asp:Panel ID="panSave" runat="server" Visible="false">
                    <div class="bigCheck"><img src="/images/bigCheck.gif" border="0" align="absmiddle" />&nbsp;Information&nbsp;has&nbsp;been&nbsp;Updated</div>
                </asp:Panel>

                <asp:Panel id="panAdd" runat="server" Visible="false">
                    <table width="100%" border="0" cellpadding="0" cellspacing="0">
                        <tr>
                            <td colspan="2">
                                <%=strMenuTab1 %>
                                <div id="divMenu1">
                                    <br />
                                    <div style="display:none">
                                        <table width="95%" cellpadding="3" cellspacing="0" border="0" align="center">
                                            <tr> 
                                                <td class="default">Name:</td>
                                                <td><asp:textbox ID="txtName" CssClass="default" runat="server" Width="300" MaxLength="100"/></td>
                                            </tr>
                                            <tr><td class="greentableheader" colspan="2">
                                                <br />Who will be approving this? (select one of the following)</td></tr>
                                            <tr> 
                                                <td class="default" style="vertical-align: top">Approve By:</td>
                                                <td>
                                                    <asp:RadioButton ID="radApproveField" runat="server" Text="Send to Field" GroupName="ApproveBy" AutoPostBack="true" OnCheckedChanged="radApproveField_CheckedChanged" /><br />
                                                    <asp:DropDownList ID="ddlField" CssClass="default" runat="server" Enabled="false" /><br />
                                                    <asp:RadioButton ID="radApproveGroup" runat="server" Text="Send to Group" GroupName="ApproveBy" AutoPostBack="true" OnCheckedChanged="radApproveGroup_CheckedChanged" /><br />
                                                    <asp:DropDownList ID="ddlGroup" CssClass="default" runat="server" Enabled="false" /><br />
                                                    <asp:RadioButton ID="radApproveRequestor" runat="server" Text="Send to Requestor" GroupName="ApproveBy" AutoPostBack="true" OnCheckedChanged="radApproveBy_CheckedChanged" /><br />
                                                    <asp:RadioButton ID="radApproveAppOwner" runat="server" Text="Send to Application Owner" GroupName="ApproveBy" AutoPostBack="true" OnCheckedChanged="radApproveBy_CheckedChanged" /><br />
                                                    <asp:RadioButton ID="radApproveATL" runat="server" Text="Send to Application Technical Lead" GroupName="ApproveBy" AutoPostBack="true" OnCheckedChanged="radApproveBy_CheckedChanged" /><br />
                                                    <asp:RadioButton ID="radApproveASM" runat="server" Text="Send to Application System Manager" GroupName="ApproveBy" AutoPostBack="true" OnCheckedChanged="radApproveBy_CheckedChanged" /><br />
                                                    <asp:RadioButton ID="radApproveSD" runat="server" Text="Send to System Director" GroupName="ApproveBy" AutoPostBack="true" OnCheckedChanged="radApproveBy_CheckedChanged" /><br />
                                                    <asp:RadioButton ID="radApproveDM" runat="server" Text="Send to Department Manager" GroupName="ApproveBy" AutoPostBack="true" OnCheckedChanged="radApproveBy_CheckedChanged" /><br />
                                                    <asp:RadioButton ID="radApproveCIO" runat="server" Text="Send to CIO" GroupName="ApproveBy" AutoPostBack="true" OnCheckedChanged="radApproveBy_CheckedChanged" /><br />
                                                </td>
                                            </tr>
                                            <tr> 
                                                <td class="default">Enabled:</td>
                                                <td><asp:CheckBox ID="chkEnabled" runat="server" Checked="true" /></td>
                                            </tr>
                                            <tr> 
                                                <td class="cmdefault">&nbsp;</td>
                                                <td><asp:Button ID="btnOrder" runat="server" Text="Change Order" Width="150" CssClass="default" /></td>
                                            </tr>
                                            <tr><td height="5" colspan="2">&nbsp;</td></tr>
                                            <tr> 
                                                <td>&nbsp;</td>
                                                <td>
                                                    <asp:button ID="btnAdd" CssClass="default" runat="server" Text="Add" Width="75" OnClick="btnAdd_Click" />
                                                    <asp:button ID="btnDelete" CssClass="default" runat="server" Text="Delete" Width="75" OnClick="btnDelete_Click" />
                                                    <asp:button ID="btnCancel" CssClass="default" runat="server" Text="Cancel" Width="75" OnClick="btnCancel_Click" />
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <div style="display:none">
                                        <div class="greentableheader">When does &quot;<asp:Literal ID="litName" runat="server" />&quot; get triggered?</div>
                                        <br />
                                        <table width="95%" cellpadding="3" cellspacing="0" border="0" align="center">
                                            <tr> 
                                                <td class="default">Field:</td>
                                                <td><asp:DropDownList ID="ddlFieldSet" CssClass="default" runat="server" /></td>
                                            </tr>
                                            <tr> 
                                                <td class="default">Operator:</td>
                                                <td>
                                                    <asp:DropDownList ID="ddlOperator" CssClass="default" runat="server">
                                                        <asp:ListItem Value="eq" Text="Equal To" />
                                                        <asp:ListItem Value="neq" Text="NOT Equal To" />
                                                        <asp:ListItem Value="lt" Text="Less Than" />
                                                        <asp:ListItem Value="lte" Text="Less Than or Equal To" />
                                                        <asp:ListItem Value="gt" Text="Greater Than" />
                                                        <asp:ListItem Value="gte" Text="Greater Than or Equal To" />
                                                        <asp:ListItem Value="in" Text="Contains" />
                                                        <asp:ListItem Value="nin" Text="Does NOT Contain" />
                                                        <asp:ListItem Value="ends" Text="Ends With" />
                                                        <asp:ListItem Value="starts" Text="Starts With" />
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr> 
                                                <td class="default">Value:</td>
                                                <td><asp:TextBox ID="txtValue" runat="server" MaxLength="100" Width="300" /></td>
                                            </tr>
                                            <tr> 
                                                <td class="default">Data Type:</td>
                                                <td>
                                                    <asp:RadioButton ID="radTypeInt" runat="server" Text="Integer" GroupName="DataType" />
                                                    <asp:RadioButton ID="radTypeDateTime" runat="server" Text="DateTime" GroupName="DataType" />
                                                    <asp:RadioButton ID="radTypeString" runat="server" Text="String" GroupName="DataType" />
                                                </td>
                                            </tr>
                                            <tr> 
                                                <td class="default">OR Group:</td>
                                                <td><asp:TextBox ID="txtOr" runat="server" MaxLength="1" Width="50" /></td>
                                            </tr>
                                            <tr><td height="5" colspan="2">&nbsp;</td></tr>
                                            <tr> 
                                                <td>&nbsp;</td>
                                                <td>
                                                    <asp:button ID="btnSetAdd" CssClass="default" runat="server" Text="Add" Width="75" OnClick="btnSetAdd_Click" />
                                                    <asp:button ID="btnSetDelete" CssClass="default" runat="server" Text="Delete" Visible="false" Width="75" OnClick="btnSetDelete_Click" />
                                                    <asp:button ID="btnSetCancel" CssClass="default" runat="server" Text="Cancel" Visible="false" Width="75" OnClick="btnSetCancel_Click" />
                                                </td>
                                            </tr>
                                        </table>
                                        <br />
                                        All of the conditions below get evaluated using AND conditions, except those with the same &quot;OR grouping&quot;.
                                        <br />
                                        <asp:repeater ID="rptSets" runat="server">
                                            <HeaderTemplate>
                                                <table width="100%" cellpadding="3" cellspacing="0" border="0" align="center">
                                                    <tr bgcolor="#CCCCCC">
                                                        <td class="bold">&nbsp;</td>
                                                        <td width="100%" class="bold">Condition</td>
                                                        <td class="bold">OR</td>
                                                    </tr>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <tr bgcolor="#EFEFEF" onmouseover="AdminMouseOver(this);" onmouseout="AdminMouseOut(this);" onclick="window.navigate('<%# Request.Path + "?id=" + Request.QueryString["id"] + "&set=" + DataBinder.Eval(Container.DataItem, "id")%>');" class="default">
                                                    <td width="5%" align="left">&nbsp;<asp:ImageButton ID="btnDeleteSet" OnClick="btnDeleteSet_Click" runat="server" ToolTip="Delete" ImageAlign="AbsMiddle" ImageUrl="/admin/images/delete.gif" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' /></td>
                                                    <td width="100%">&nbsp;<strong><%# DataBinder.Eval(Container.DataItem, "field") %></strong> <%# DataBinder.Eval(Container.DataItem, "operator") %> &quot;<strong><%# DataBinder.Eval(Container.DataItem, "value") %></strong>&quot;</td>
                                                    <td>&nbsp;<%# DataBinder.Eval(Container.DataItem, "or_group") %></td>
                                                </tr>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                </table>
                                            </FooterTemplate>
                                        </asp:repeater>
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
        <asp:HiddenField ID="hdnTab" runat="server" />
</form>
</body>
</html>
