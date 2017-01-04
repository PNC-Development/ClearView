<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="applications.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.applications" %>

<script type="text/javascript">
    var oAdd = null;
    var oView = null;
    var oAddButton = null;
    var oDeleteButton = null;
    var oHidden = null;
    var oName = null;
    var oUrl = null;
    var oTitle = null;
    var oOrg = null;
    var oDesc = null;
    var oImage = null;
    var oUser = null;
    var oUserId = null;
    var oParentId = null;
    var oParent = null;
    var oPriority1 = null;
    var oPriority2 = null;
    var oTPM = null;
    var oDisable = null;
    var oManager = null;
    var oPlatform = null;
    var oDoc = null;
    var oLead1 = null;
    var oLead2 = null;
    var oLead3 = null;
    var oLead4 = null;
    var oLead5 = null;
    var oApproval = null;
    var oEmployees = null;
    var oServiceSearch = null;
    var oReminders = null;
    var oRequestItems = null;
    var oDecom = null;
    var oAdmin = null;
    var oDNS = null;
    var oEnabled = null;
    function Edit(strId, strName, strUrl, strTitle, strOrg, strDesc, strImage, strUser, strUserId, strParentId, strParent, strPriority1, strPriority2, strTPM, strDisable, strManager, strPlatform, strDoc, strLead1, strLead2, strLead3, strLead4, strLead5, strApproval, strEmployees, strServiceSearch, strReminders, strRequestItems, strDecom, strAdmin, strDNS, strEnabled) {
        oAdd.style.display = "inline";
        oView.style.display = "none";
        oAddButton.value = "Update";
        oDeleteButton.style.display = "inline";
        oHidden.value = strId;
        oName.value = strName;
        oUrl.value = strUrl;
        oTitle.value = strTitle;
        oOrg.value = strOrg;
        oDesc.value = strDesc;
        oImage.value = strImage;
        oUser.value = strUser;
        oUserId.value = strUserId;
        oParentId.value = strParentId;
        if (strParent == "")
            oParent.innerText = "No Parent";
        else
            oParent.innerText = strParent;
        oPriority1.value = strPriority1;
        oPriority2.value = strPriority2;
        oTPM.checked = (strTPM == "1");
        oDisable.checked = (strDisable == "1");
        oManager.checked = (strManager == "1");
        oPlatform.checked = (strPlatform == "1");
        oDoc.value = strDoc;
        oLead1.value = strLead1;
        oLead2.value = strLead2;
        oLead3.value = strLead3;
        oLead4.value = strLead4;
        oLead5.value = strLead5;
        oApproval.checked = (strApproval == "1");
        oEmployees.value = strEmployees;
        oServiceSearch.checked = (strServiceSearch == "1");
        oReminders.checked = (strReminders == "1");
        oRequestItems.checked = (strRequestItems == "1");
        oDecom.checked = (strDecom == "1");
        oAdmin.checked = (strAdmin == "1");
        oDNS.checked = (strDNS == "1");
        oEnabled.checked = (strEnabled == "1");
    }
    function Add() {
        oAdd.style.display = "inline";
        oView.style.display = "none";
        oAddButton.value = "Add";
        oDeleteButton.style.display = "none";
        oHidden.value = "0";
        oName.value = "";
        oUrl.value = "";
        oTitle.value = "";
        oOrg.value = "";
        oDesc.value = "";
        oImage.value = "";
        oUser.value = "";
        oUserId.value = "0";
        oParent.innerText = "No Parent";
        oParentId.value = "0";
        oPriority1.value = "0";
        oPriority2.value = "0";
        oTPM.checked = false;
        oDisable.checked = false;
        oManager.checked = false;
        oPlatform.checked = false;
        oDoc.value = "";
        oLead1.value = "0";
        oLead2.value = "0";
        oLead3.value = "0";
        oLead4.value = "0";
        oLead5.value = "0";
        oApproval.checked = false;
        oEmployees.value = "2";
        oServiceSearch.checked = false;
        oReminders.checked = false;
        oRequestItems.checked = false;
        oDecom.checked = false;
        oDNS.checked = false;
        oAdmin.checked = false;
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
        oUrl = document.getElementById('<%=txtUrl.ClientID%>');
        oTitle = document.getElementById('<%=txtTitle.ClientID%>');
        oOrg = document.getElementById('<%=txtOrg.ClientID%>');
        oDesc = document.getElementById('<%=txtDescription.ClientID%>');
        oImage = document.getElementById('<%=txtImage.ClientID%>');
        oUserId = document.getElementById('<%=hdnUser.ClientID%>');
        oUser = document.getElementById('<%=txtUser.ClientID%>');
        oParent = document.getElementById('<%=lblParent.ClientID%>');
        oParentId = document.getElementById('<%=hdnParent.ClientID%>');
        oPriority1 = document.getElementById('<%=txtPriority1.ClientID%>');
        oPriority2 = document.getElementById('<%=txtPriority2.ClientID%>');
        oTPM = document.getElementById('<%=chkTPM.ClientID%>');
        oDisable = document.getElementById('<%=chkDisable.ClientID%>');
        oManager = document.getElementById('<%=chkManager.ClientID%>');
        oPlatform = document.getElementById('<%=chkPlatform.ClientID%>');
        oDoc = document.getElementById('<%=txtDoc.ClientID%>');
        oLead1 = document.getElementById('<%=txtLead1.ClientID%>');
        oLead2 = document.getElementById('<%=txtLead2.ClientID%>');
        oLead3 = document.getElementById('<%=txtLead3.ClientID%>');
        oLead4 = document.getElementById('<%=txtLead4.ClientID%>');
        oLead5 = document.getElementById('<%=txtLead5.ClientID%>');
        oApproval = document.getElementById('<%=chkApproval.ClientID%>');
        oEmployees = document.getElementById('<%=txtEmployees.ClientID%>');
        oServiceSearch = document.getElementById('<%=chkServiceSearch.ClientID%>');
        oReminders = document.getElementById('<%=chkReminders.ClientID%>');
        oRequestItems = document.getElementById('<%=chkRequestItems.ClientID%>');
        oDecom = document.getElementById('<%=chkDecom.ClientID%>');
        oAdmin = document.getElementById('<%=chkAdmin.ClientID%>');
        oDNS = document.getElementById('<%=chkDNS.ClientID%>');
        oEnabled = document.getElementById('<%=chkEnabled.ClientID%>');
    }
</script>
<html>
<head>
<title>ClearView Administration</title>
<link rel="stylesheet" type="text/css" href="/css/default.css" />
<script type="text/javascript" src="/javascript/global.js"></script>
<script type="text/javascript" src="/javascript/ajax.js"></script>
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
		    <td><b>Applications</b></td>
		    <td align="right"><a href="javascript:void(0);" onclick="Add();" class="cmlink" title="Click to Add New">Add New</a></td>
		</tr>
		<tr><td colspan="2">&nbsp;</td></tr>
		<tr>
		    <td colspan="2" align="center">
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
                                        <tr bgcolor="#EFEFEF" onmouseover="AdminMouseOver(this);" onmouseout="AdminMouseOut(this);" onclick="if(event.srcElement.tagName != 'INPUT')Edit('<%# DataBinder.Eval(Container.DataItem, "applicationid") %>','<%# DataBinder.Eval(Container.DataItem, "name") %>','<%# DataBinder.Eval(Container.DataItem, "url") %>','<%# DataBinder.Eval(Container.DataItem, "service_title") %>','<%# DataBinder.Eval(Container.DataItem, "orgchart") %>','<%# DataBinder.Eval(Container.DataItem, "description") %>','<%# DataBinder.Eval(Container.DataItem, "image") %>','<%# oUser.GetFullName(Int32.Parse(DataBinder.Eval(Container.DataItem, "userid").ToString())) %>','<%# DataBinder.Eval(Container.DataItem, "userid") %>','<%# DataBinder.Eval(Container.DataItem, "parent") %>','<%# DataBinder.Eval(Container.DataItem, "parentname") %>','<%# DataBinder.Eval(Container.DataItem, "priority1") %>','<%# DataBinder.Eval(Container.DataItem, "priority2") %>','<%# DataBinder.Eval(Container.DataItem, "tpm") %>','<%# DataBinder.Eval(Container.DataItem, "disable_manager") %>','<%# DataBinder.Eval(Container.DataItem, "manager_approve") %>','<%# DataBinder.Eval(Container.DataItem, "platform_approve") %>','<%# DataBinder.Eval(Container.DataItem, "deliverables_doc") %>','<%# DataBinder.Eval(Container.DataItem, "lead1") %>','<%# DataBinder.Eval(Container.DataItem, "lead2") %>','<%# DataBinder.Eval(Container.DataItem, "lead3") %>','<%# DataBinder.Eval(Container.DataItem, "lead4") %>','<%# DataBinder.Eval(Container.DataItem, "lead5") %>','<%# DataBinder.Eval(Container.DataItem, "approve_vacation") %>','<%# DataBinder.Eval(Container.DataItem, "employees_needed") %>','<%# DataBinder.Eval(Container.DataItem, "service_search_items") %>','<%# DataBinder.Eval(Container.DataItem, "send_reminders") %>','<%# DataBinder.Eval(Container.DataItem, "request_items") %>','<%# DataBinder.Eval(Container.DataItem, "decom") %>','<%# DataBinder.Eval(Container.DataItem, "admin") %>','<%# DataBinder.Eval(Container.DataItem, "dns") %>','<%# DataBinder.Eval(Container.DataItem, "enabled") %>');" class="default">
                                            <td width="5%" align="left">&nbsp;<asp:ImageButton ID="btnDelete" OnClick="btnDeleteLink_Click" runat="server" ToolTip="Delete" ImageAlign="AbsMiddle" ImageUrl="/admin/images/delete.gif" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "applicationid") %>' /></td>
                                            <td width="50%">&nbsp;<%# DataBinder.Eval(Container.DataItem, "name") %></td>
                                            <td width="50%">&nbsp;<%# DataBinder.Eval(Container.DataItem, "modified") %></td>
                                            <td align="center"><asp:ImageButton ID="btnEnable" runat="server" ImageAlign="AbsMiddle" ImageUrl='<%# (DataBinder.Eval(Container.DataItem, "enabled").ToString() == "1") ? "/admin/images/enabled.gif" : "/admin/images/disabled.gif" %>' CommandArgument='<%# DataBinder.Eval(Container.DataItem, "applicationid") %>' OnClick="btnEnable_Click" /></td>
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
                            <td><asp:textbox ID="txtName" CssClass="default" runat="server" Width="300" MaxLength="100"/></td>
                        </tr>
                        <tr> 
                            <td class="default">URL:</td>
                            <td><asp:textbox ID="txtUrl" CssClass="default" runat="server" Width="200" MaxLength="50"/></td>
                        </tr>
                        <tr> 
                            <td class="default">Service Title:</td>
                            <td><asp:textbox ID="txtTitle" CssClass="default" runat="server" Width="300" MaxLength="100"/></td>
                        </tr>
                        <tr> 
                            <td class="default">Org Chart Name:</td>
                            <td><asp:textbox ID="txtOrg" CssClass="default" runat="server" Width="300" MaxLength="100"/></td>
                        </tr>
                        <tr> 
                            <td class="default" valign="top">Description:</td>
                            <td><asp:textbox ID="txtDescription" CssClass="default" runat="server" Width="300" Rows="5" TextMode="MultiLine"/></td>
                        </tr>
                        <tr> 
                            <td class="default">Image:</td>
                            <td><asp:textbox ID="txtImage" CssClass="default" runat="server" Width="300" MaxLength="100"/> <asp:Button ID="btnImage" runat="server" CssClass="default" Text="..." /></td>
                        </tr>
                        <tr> 
                            <td class="default">User:</td>
                            <td>
                                <table cellpadding="0" cellspacing="0" border="0">
                                    <tr>
                                        <td><asp:TextBox ID="txtUser" runat="server" Width="300" CssClass="default" /></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div id="divAJAX" runat="server" style="overflow:hidden; position:absolute; display:none; background-color:#FFFFFF; border:solid 1px #CCCCCC">
                                                <asp:ListBox ID="lstAJAX" runat="server" CssClass="default" />
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                                <input type="hidden" id="hdnAJAXValue" name="hdnAJAXValue" />
                            </td>
                        </tr>
                        <tr>
                            <td class="cmdefault">Parent:</td>
                            <td><asp:label ID="lblParent" CssClass="default" runat="server" />&nbsp;&nbsp;<asp:Button ID="btnParent" runat="server" CssClass="default" Width="25" Text="..." /></td>
                        </tr>
                        <tr> 
                            <td class="default">Priority 1's:</td>
                            <td><asp:textbox ID="txtPriority1" CssClass="default" runat="server" Width="50" MaxLength="2"/></td>
                        </tr>
                        <tr> 
                            <td class="default">Priority 2's:</td>
                            <td><asp:textbox ID="txtPriority2" CssClass="default" runat="server" Width="50" MaxLength="2"/></td>
                        </tr>
                        <tr> 
                            <td class="default">TPM:</td>
                            <td><asp:CheckBox ID="chkTPM" runat="server" /></td>
                        </tr>
                        <tr> 
                            <td class="default">Disable Manager Assignment:</td>
                            <td><asp:CheckBox ID="chkDisable" runat="server" Checked="true" /></td>
                        </tr>
                        <tr> 
                            <td class="default">Manager Approval:</td>
                            <td><asp:CheckBox ID="chkManager" runat="server" Checked="true" /></td>
                        </tr>
                        <tr> 
                            <td class="default">Platform Approval:</td>
                            <td><asp:CheckBox ID="chkPlatform" runat="server" Checked="true" /></td>
                        </tr>
                        <tr> 
                            <td class="default">Service Deliverable Doc:</td>
                            <td><asp:textbox ID="txtDoc" CssClass="default" runat="server" Width="300" MaxLength="100"/> <asp:Button ID="btnDoc" runat="server" CssClass="default" Text="..." /></td>
                        </tr>
                        <tr> 
                            <td class="default">Lead Time Priority 1:</td>
                            <td><asp:textbox ID="txtLead1" CssClass="default" runat="server" Width="75" MaxLength="4"/> (days)</td>
                        </tr>
                        <tr> 
                            <td class="default">Lead Time Priority 2:</td>
                            <td><asp:textbox ID="txtLead2" CssClass="default" runat="server" Width="75" MaxLength="4"/> (days)</td>
                        </tr>
                        <tr> 
                            <td class="default">Lead Time Priority 3:</td>
                            <td><asp:textbox ID="txtLead3" CssClass="default" runat="server" Width="75" MaxLength="4"/> (days)</td>
                        </tr>
                        <tr> 
                            <td class="default">Lead Time Priority 4:</td>
                            <td><asp:textbox ID="txtLead4" CssClass="default" runat="server" Width="75" MaxLength="4"/> (days)</td>
                        </tr>
                        <tr> 
                            <td class="default">Lead Time Priority 5:</td>
                            <td><asp:textbox ID="txtLead5" CssClass="default" runat="server" Width="75" MaxLength="4"/> (days)</td>
                        </tr>
                        <tr> 
                            <td class="default">Manager Approval Required for Vacation:</td>
                            <td><asp:CheckBox ID="chkApproval" runat="server" Checked="true" /></td>
                        </tr>
                        <tr> 
                            <td class="default"># of Employees Necessary for Vacation:</td>
                            <td><asp:textbox ID="txtEmployees" CssClass="default" runat="server" Width="50" MaxLength="2"/></td>
                        </tr>
                        <tr> 
                            <td class="default">Service Search Items:</td>
                            <td><asp:CheckBox ID="chkServiceSearch" runat="server" Checked="true" /></td>
                        </tr>
                        <tr> 
                            <td class="default">Send Reminders:</td>
                            <td><asp:CheckBox ID="chkReminders" runat="server" /></td>
                        </tr>
                        <tr> 
                            <td class="default">Use Request Items:</td>
                            <td><asp:CheckBox ID="chkRequestItems" runat="server" /></td>
                        </tr>
                        <tr> 
                            <td class="default">Can Decom:</td>
                            <td><asp:CheckBox ID="chkDecom" runat="server" /></td>
                        </tr>
                        <tr> 
                            <td class="default">Administrative App:</td>
                            <td><asp:CheckBox ID="chkAdmin" runat="server" /></td>
                        </tr>
                        <tr> 
                            <td class="default">Can PNC DNS:</td>
                            <td><asp:CheckBox ID="chkDNS" runat="server" /></td>
                        </tr>
                        <tr> 
                            <td class="default">Enabled:</td>
                            <td><asp:CheckBox ID="chkEnabled" runat="server" Checked="true" /></td>
                        </tr>
                        <tr><td height="5" colspan="2"><img src="images/spacer.gif" width="1" height="5" /></td></tr>
                        <tr> 
                            <td class="cmdefault">&nbsp;</td>
                            <td><asp:Button ID="btnReportGroups" runat="server" Text="Edit Report Groups" Width="125" CssClass="default" /></td>
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
            </td>
        </tr>
    </table>
        </div>
        </td>
        </tr>
        </table>
    <input type="hidden" id="hdnId" runat="server" />
    <input type="hidden" id="hdnParent" runat="server" />
    <asp:HiddenField ID="hdnUser" runat="server" />
</form>
</body>
</html>
