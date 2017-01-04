<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="project_request_questions.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.project_request_questions" %>


<script type="text/javascript">
    var oAdd = null;
    var oView = null;
    var oAddButton = null;
    var oEditButton = null;
    var oDeleteButton = null;
    var oHidden = null;
    var oName = null;
    var oQuestion = null;
    var oEnabled = null;
    var oRequired = null;
    function Edit(strId, strName, strQuestion,strEnabled,strRequired) {
        oAdd.style.display = "inline";        
        oView.style.display = "none";
        oAddButton.value = "Update";
        oEditButton.style.display = "inline";
        oDeleteButton.style.display = "inline";
        oHidden.value = strId;
        oName.value = strName;
        oQuestion.value = strQuestion;        
        oEnabled.checked = (strEnabled == "1");
        oRequired.checked = (strRequired == "1");
    }
    function Add() {
        oAdd.style.display = "inline";       
        oView.style.display = "none";
        oAddButton.value = "Add";
        oEditButton.style.display = "none";
        oDeleteButton.style.display = "none";
        oHidden.value = "0";
        oName.value = "";
        oQuestion.value = "";                
        oEnabled.checked = false;
        oRequired.checked = false;
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
        oEditButton = document.getElementById('<%=btnEdit.ClientID%>');
        oDeleteButton = document.getElementById('<%=btnDelete.ClientID%>');
        oHidden = document.getElementById('<%=hdnId.ClientID%>');
        oName = document.getElementById('<%=txtName.ClientID%>');
        oQuestion = document.getElementById('<%=txtQuestion.ClientID%>');
        oEnabled = document.getElementById('<%=chkEnabled.ClientID%>');
        oRequired = document.getElementById('<%=chkRequired.ClientID%>');
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
		    <td><b>Project Request Questions</b></td>
		    <td align="right"><a href="javascript:void(0);" onclick="Add();" class="cmlink" title="Click to Add New">Add New</a></td>
		</tr>
		<tr><td colspan="2">&nbsp;</td></tr>
		<tr>
		    <td colspan="2" align="center">
		        <asp:Panel ID="panView" runat="server" Visible="true">
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
                                                <td width="45%" class="bold"><asp:linkbutton ID="lnkQuestion" Text="Question" CommandArgument="question" CssClass="bold" OnClick="OrderView" runat="server"/></td>
                                                <td width="5%" class="bold"><asp:linkbutton ID="lnkModified" Text="Last Modified" CommandArgument="modified" CssClass="bold" OnClick="OrderView" runat="server"/></td>
                                                <td class="bold" align="center"><asp:linkbutton ID="lnkEnabled" Text="Enabled" CommandArgument="enabled" CssClass="bold" OnClick="OrderView" runat="server"/></td>
                                            </tr>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr bgcolor="#EFEFEF" onmouseover="AdminMouseOver(this);" onmouseout="AdminMouseOut(this);" onclick="if(event.srcElement.tagName != 'INPUT')Edit('<%# DataBinder.Eval(Container.DataItem, "id") %>','<%# DataBinder.Eval(Container.DataItem, "name") %>','<%# DataBinder.Eval(Container.DataItem, "question") %>','<%# DataBinder.Eval(Container.DataItem, "enabled") %>','<%# DataBinder.Eval(Container.DataItem, "required") %>');" class="default">
                                            <td width="5%" align="left">&nbsp;<asp:ImageButton ID="btnDelete" OnClick="btnDeleteLink_Click" runat="server" ToolTip="Delete" ImageAlign="AbsMiddle" ImageUrl="/admin/images/delete.gif" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' /></td>
                                            <td width="50%" nowrap>&nbsp;<%# DataBinder.Eval(Container.DataItem, "name") %></td>
                                            <td width="45%" nowrap>&nbsp;<%# DataBinder.Eval(Container.DataItem, "question") %></td>
                                            <td width="5%" nowrap>&nbsp;<%# DataBinder.Eval(Container.DataItem, "modified") %></td>
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
                            <td class="default">Enabled:</td>
                            <td><asp:CheckBox ID="chkEnabled" runat="server" Checked="true" /></td>
                        </tr>
                         <tr> 
                            <td class="default">Required:</td>
                            <td><asp:CheckBox ID="chkRequired" runat="server" Checked="true" /></td>
                        </tr>
                        <tr><td height="5" colspan="2"><img src="images/spacer.gif" width="1" height="5" /></td></tr>
                        <tr> 
                            <td class="cmdefault">&nbsp;</td>
                            <td>
                                <asp:Button ID="btnOrder" runat="server" Text="Change Order" Width="150" CssClass="default" />
                                <asp:Button ID="btnEdit" runat="server" Text="Edit Organization" Width="150" CssClass="default" /> 
                                <asp:Button ID="btnEditClass" runat="server" Text="Edit Classes" Width="150" CssClass="default" /> 
                            </td>
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
