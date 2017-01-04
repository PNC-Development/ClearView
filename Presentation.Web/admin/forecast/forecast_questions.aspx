<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="forecast_questions.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.forecast_questions" %>

<script type="text/javascript">
    var oAdd = null;
    var oView = null;
    var oAddButton = null;
    var oDeleteButton = null;
    var oHidden = null;
    var oName = null;
    var oQuestion = null;
    var oType = null;
    var oRequired = null;
    var oEnabled = null;
    function Edit(strId, strName, strQuestion, strType, strOverride, strRequired, strEnabled) {
        oAdd.style.display = "inline";
        oView.style.display = "none";
        oAddButton.value = "Update";
        oDeleteButton.style.display = "inline";
        oHidden.value = strId;
        oName.value = strName;
        oQuestion.value = strQuestion;
        for (var ii=0; ii<oType.length; ii++) {
            if (oType.options[ii].value == strType)
                oType.selectedIndex = ii;
        }
        oOverride.checked = (strOverride == "1");
        oRequired.checked = (strRequired == "1");
        oEnabled.checked = (strEnabled == "1");
    }
    function Add() {
        oAdd.style.display = "inline";
        oView.style.display = "none";
        oAddButton.value = "Add";
        oDeleteButton.style.display = "none";
        oHidden.value = "0";
        oName.value = "";
        oQuestion.value = "";
        oType.selectedIndex = 0;
        oOverride.checked = false;
        oRequired.checked = false;
        oEnabled.checked = false;
        oName.focus();
    }
    function Cancel() {
        oAdd.style.display = "none";
        oView.style.display = "inline";
        return false;
    }
    function Load() {
        oAdd = document.getElementById('<%=divAdd.ClientID%>');
        oView = document.getElementById('<%=divView.ClientID%>');
        oAddButton = document.getElementById('<%=btnAdd.ClientID%>');
        oDeleteButton = document.getElementById('<%=btnDelete.ClientID%>');
        oHidden = document.getElementById('<%=hdnId.ClientID%>');
        oName = document.getElementById('<%=txtName.ClientID%>');
        oQuestion = document.getElementById('<%=txtQuestion.ClientID%>');
        oType = document.getElementById('<%=ddlType.ClientID%>');
        oOverride = document.getElementById('<%=chkOverride.ClientID%>');
        oRequired = document.getElementById('<%=chkRequired.ClientID%>');
        oEnabled = document.getElementById('<%=chkEnabled.ClientID%>');
    }
    function Affects(intQ, intA) {
        OpenWindow('AFFECTS','','?qid=' + intQ + '&aid=' + intA,false,400,150);
        return false;
    }
    function Affected(intQ, intA, intR) {
        OpenWindow('AFFECTED','','?qid=' + intQ + '&aid=' + intA + '&rid=' + intR,false,400,175);
        return false;
    }
</script>
<html>
<head>
<title>Web Content Management Administration</title>
<link rel="stylesheet" type="text/css" href="/css/default.css" />
<script type="text/javascript" src="/javascript/global.js"></script>
</head>
<body topmargin="0" leftmargin="0" onload="Load()">
<form id="Form1" runat="server">
        <table width="100%" height="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
        <td height="100%">
        <div style="height:100%; overflow:auto">
<table width="98%" cellpadding="0" cellspacing="0" border="0" align="center">
		<tr><td colspan="2">&nbsp;</td></tr>
		<tr> 
		    <td><b>Forecast Questions</b></td>
		    <td align="right"><a href="javascript:void(0);" onclick="Add();" class="cmlink" title="Click to Add New">Add New</a></td>
		</tr>
		<tr><td colspan="2">&nbsp;</td></tr>
		<tr>
		    <td colspan="2" align="center">
		        <asp:Panel ID="panView" runat="server" Visible="false">
                <div ID="divView" runat="server">
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
                                        <tr bgcolor="#EFEFEF" onmouseover="AdminMouseOver(this);" onmouseout="AdminMouseOut(this);" onclick="if(event.srcElement.tagName != 'INPUT')Edit('<%# DataBinder.Eval(Container.DataItem, "id") %>','<%# DataBinder.Eval(Container.DataItem, "name") %>','<%# DataBinder.Eval(Container.DataItem, "question") %>','<%# DataBinder.Eval(Container.DataItem, "type") %>','<%# DataBinder.Eval(Container.DataItem, "hide_override") %>','<%# DataBinder.Eval(Container.DataItem, "required") %>','<%# DataBinder.Eval(Container.DataItem, "enabled") %>');" class="default">
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
                </div>

                <div id="divAdd" runat="server" style="display:none">
                    <table width="95%" cellpadding="3" cellspacing="0" border="0" align="center">
                        <tr> 
                            <td class="default" width="100px">Name:</td>
                            <td><asp:textbox ID="txtName" CssClass="default" runat="server" Width="400" MaxLength="100"/></td>
                        </tr>
                        <tr> 
                            <td class="default">Question:</td>
                            <td><asp:textbox ID="txtQuestion" CssClass="default" runat="server" Width="400" Rows="7" TextMode="MultiLine"/></td>
                        </tr>
                        <tr> 
                            <td class="default">Type:</td>
                            <td>
                                <asp:DropDownList ID="ddlType" runat="server" CssClass="default">
                                    <asp:ListItem Value="1" Text="DropDown List" />
                                    <asp:ListItem Value="2" Text="RadioButton List" />
                                    <asp:ListItem Value="3" Text="CheckBox List" />
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr> 
                            <td class="default">Hide in Override:</td>
                            <td><asp:CheckBox ID="chkOverride" runat="server" /></td>
                        </tr>
                        <tr> 
                            <td class="default">Required:</td>
                            <td><asp:CheckBox ID="chkRequired" runat="server" /></td>
                        </tr>
                        <tr> 
                            <td class="default">Enabled:</td>
                            <td><asp:CheckBox ID="chkEnabled" runat="server" Checked="true" /></td>
                        </tr>
                        <tr><td height="5" colspan="2"><img src="images/spacer.gif" width="1" height="5" /></td></tr>
                        <tr> 
                            <td class="cmdefault">&nbsp;</td>
                            <td><asp:Button ID="btnOrder" runat="server" Text="Change Order" Width="150" CssClass="default" /></td>
                        </tr>
                        <tr> 
                            <td class="cmdefault">&nbsp;</td>
                            <td><asp:Button ID="btnPlatforms" runat="server" Text="Platforms / Classes" Width="150" CssClass="default" /></td>
                        </tr>
                        <tr> 
                            <td class="cmdefault">&nbsp;</td>
                            <td><asp:Button ID="btnAffects" runat="server" Text="Change Affects" Width="150" CssClass="default" OnClick="btnAffects_Click" /></td>
                        </tr>
                        <tr><td height="5" colspan="2">&nbsp;</td></tr>
                        <tr> 
                            <td>&nbsp;</td>
                            <td>
                                <asp:button ID="btnAdd" CssClass="default" runat="server" Text="Add" Width="75" OnClick="btnAdd_Click" />
                                <asp:button ID="btnDelete" CssClass="default" runat="server" Text="Delete" Width="75" OnClick="btnDelete_Click" />
                                <asp:button ID="btnCancel" CssClass="default" runat="server" Text="Cancel" Width="75" />
                            </td>
                        </tr>
                    </table>
                </div>
                </asp:Panel>
                
		        <asp:Panel ID="panAffects" runat="server" Visible="false">
                    <table width="95%" cellpadding="3" cellspacing="0" border="0" align="center">
                        <tr> 
                            <td>The checked question(s) are the ones that have affects related to them. Click <b>Edit</b> to edit this affect. Click <b>Affected</b> to view the affected responses to this question.</td>
                        </tr>
                        <tr> 
                            <td>&nbsp;</td>
                        </tr>
                        <tr> 
                            <td><asp:Label ID="lblName" runat="server" CssClass="bold" /></td>
                        </tr>
                        <tr> 
                            <td>
                                <asp:repeater ID="rptAffects" runat="server">
                                    <HeaderTemplate>
                                        <table width="100%" cellpadding="3" cellspacing="0" border="0" align="center">
                                            <tr bgcolor="#CCCCCC">
                                                <td class="bold">&nbsp;</td>
                                                <td class="bold">&nbsp;</td>
                                                <td class="bold">&nbsp;</td>
                                                <td width="100%" class="bold"><b>Name</b></td>
                                            </tr>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr bgcolor="#EFEFEF" class="default">
                                            <td nowrap>&nbsp;<a href="javascript:void(0);" onclick="Affects('<%= intQuestion %>','<%# DataBinder.Eval(Container.DataItem, "id") %>');"><%# (DataBinder.Eval(Container.DataItem, "state").ToString() == "" ? "Add" : "Edit") %></a></td>
                                            <td nowrap>&nbsp;<asp:LinkButton ID="btnAffected" runat="server" Text='<%# (DataBinder.Eval(Container.DataItem, "state").ToString() == "" ? "" : "Affected") %>' OnClick="btnAffected_Click" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' /></td>
                                            <td nowrap>&nbsp;<asp:CheckBox ID="chkAffect" runat="server" CssClass="default" Enabled="false" Checked='<%# (DataBinder.Eval(Container.DataItem, "state").ToString() == "" ? false : true) %>' /></td>
                                            <td width="100%">&nbsp;<%# DataBinder.Eval(Container.DataItem, "name") %></td>
                                        </tr>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </table>
                                    </FooterTemplate>
                                </asp:repeater>
                            </td>
		                </tr>
		                <tr>
		                    <td>&nbsp;</td>
		                </tr>
		                <tr>
		                    <td><asp:Button ID="btnBack" runat="server" CssClass="default" Width="75" Text="Back" OnClick="btnBack_Click" /></td>
		                </tr>
		            </table>
		        </asp:Panel>
                
		        <asp:Panel ID="panAffected" runat="server" Visible="false">
                    <table width="95%" cellpadding="3" cellspacing="0" border="0" align="center">
                        <tr> 
                            <td>The checked response(s) are the ones that have affects related to them. Click <b>Edit</b> to edit this affect.</td>
                        </tr>
                        <tr> 
                            <td>&nbsp;</td>
                        </tr>
                        <tr> 
                            <td><asp:Label ID="lblQuestion" runat="server" CssClass="bold" /> -- AFFECTS -- <asp:Label ID="lblAffected" runat="server" CssClass="bold" /></td>
                        </tr>
                        <tr> 
                            <td>
                                <asp:repeater ID="rptAffected" runat="server">
                                    <HeaderTemplate>
                                        <table width="100%" cellpadding="3" cellspacing="0" border="0" align="center">
                                            <tr bgcolor="#CCCCCC">
                                                <td class="bold">&nbsp;</td>
                                                <td class="bold">&nbsp;</td>
                                                <td class="bold">&nbsp;</td>
                                                <td width="100%" class="bold"><b>Name</b></td>
                                            </tr>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr bgcolor="#EFEFEF" class="default">
                                            <td nowrap>&nbsp;<a href="javascript:void(0);" onclick="Affected('<%= intQuestion %>','<%= intAffected %>','<%# DataBinder.Eval(Container.DataItem, "id") %>');"><%# (DataBinder.Eval(Container.DataItem, "state").ToString() == "" ? "Add" : "Edit") %></a></td>
                                            <td nowrap>&nbsp;<asp:CheckBox ID="chkAffect" runat="server" CssClass="default" Enabled="false" Checked='<%# (DataBinder.Eval(Container.DataItem, "state").ToString() == "" ? false : true) %>' /></td>
                                            <td width="100%">&nbsp;<%# DataBinder.Eval(Container.DataItem, "name") %></td>
                                        </tr>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </table>
                                    </FooterTemplate>
                                </asp:repeater>
                            </td>
		                </tr>
		                <tr>
		                    <td>&nbsp;</td>
		                </tr>
		                <tr>
		                    <td>
		                        <asp:Button ID="btnBackAffected" runat="server" CssClass="default" Width="75" Text="Back" OnClick="btnBackAffected_Click" /> 
		                        <asp:Button ID="btnBack2" runat="server" CssClass="default" Width="100" Text="Back to List" OnClick="btnBack_Click" /> 
		                    </td>
		                </tr>
		            </table>
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
