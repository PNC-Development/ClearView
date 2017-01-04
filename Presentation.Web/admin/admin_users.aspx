<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="admin_users.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.admin_users" %>



<script type="text/javascript">
    var oAdd = null;
    var oView = null;
    var oEdit = null;
    var oAddButton = null;
    var oDeleteButton = null;
    var oHidden = null;
    var oUser = null;
    var oPNC = null;
    var oFname = null;
    var oLname = null;
    var oBoard = null;
    var oDirector = null;
    var oManager = null;
    var oManagerId = null;
    var oIsManager = null;
    var oPager = null;
    var oAt = null;
    var oPhone = null;
    var oSkills = null;
    var oVacation = null;
    var oMultiple = null;
    var oUngroupProjects = null;
    var oShowReturns = null;
    var oLocation = null;
    var oAdmin = null;
    var oEnabled = null;
    function Edit(strId, strUser, strPNC, strFName, strLName, strManager, strManagerId, strIsManager, strBoard, strDirector, strPager, strAt, strPhone, strSkills, strVacation, strMultiple, strUngroupProjects, strShowReturns, strLocation, strAdmin, strEnabled) {
        oEdit.style.display = "inline";
        oAdd.style.display = "none";
        oView.style.display = "none";
        oAddButton.value = "Update";
        oDeleteButton.style.display = "inline";
        oHidden.value = strId;
        oUser.value = strUser;
        oPNC.value = strPNC;
        oFname.value = strFName;
        oLname.value = strLName;
        oManager.value = strManager;
        oManagerId.value = strManagerId;
        oIsManager.checked = (strIsManager == "1");
        oBoard.checked = (strBoard == "1");
        oDirector.checked = (strDirector == "1");
        oPager.value = strPager;
        for (var ii=0; ii<oAt.length; ii++) {
            if (oAt.options[ii].value == strAt)
                oAt.selectedIndex = ii;
        }
        oPhone.value = strPhone;
        oSkills.value = strSkills;
        oVacation.value = strVacation;
        oMultiple.checked = (strMultiple == "1");
        oUngroupProjects.checked = (strUngroupProjects == "1");
        oShowReturns.checked = (strShowReturns == "1");
        oLocation.checked = (strLocation == "1");
        oAdmin.checked = (strAdmin == "1");
        oEnabled.checked = (strEnabled == "1");
    }
    function Add() {
        oEdit.style.display = "none";
        oAdd.style.display = "inline";
        oView.style.display = "none";
    }
    function Cancel() {
        oEdit.style.display = "none";
        oAdd.style.display = "none";
        oView.style.display = "inline";
        return false;
    }
    function Load() {
        oAdd = document.getElementById('<%=divAdd.ClientID%>');
        oEdit = document.getElementById('<%=divEdit.ClientID%>');
        oView = document.getElementById('<%=divView.ClientID%>');
        oAddButton = document.getElementById('<%=btnAdd.ClientID%>');
        oDeleteButton = document.getElementById('<%=btnDelete.ClientID%>');
        oHidden = document.getElementById('<%=hdnId.ClientID%>');
        oUser = document.getElementById('<%=txtUser.ClientID%>');
        oPNC = document.getElementById('<%=txtPNC.ClientID%>');
        oFname = document.getElementById('<%=txtFirst.ClientID%>');
        oLname = document.getElementById('<%=txtLast.ClientID%>');
        oManager = document.getElementById('<%=txtManager.ClientID%>');
        oManagerId = document.getElementById('<%=hdnManager.ClientID%>');
        oIsManager = document.getElementById('<%=chkManager.ClientID%>');
        oBoard = document.getElementById('<%=chkBoard.ClientID%>');
        oDirector = document.getElementById('<%=chkDirector.ClientID%>');
        oPager = document.getElementById('<%=txtPagers.ClientID%>');
        oAt = document.getElementById('<%=ddlUserAt.ClientID%>');
        oPhone = document.getElementById('<%=txtPhone.ClientID%>');
        oSkills = document.getElementById('<%=txtSkills.ClientID%>');
        oVacation = document.getElementById('<%=txtVacation.ClientID%>');
        oMultiple = document.getElementById('<%=chkMultiple.ClientID%>');
        oUngroupProjects = document.getElementById('<%=chkUngroupProjects.ClientID%>');
        oShowReturns = document.getElementById('<%=chkShowReturns.ClientID%>');
        oLocation = document.getElementById('<%=chkAddLocation.ClientID%>');
        oAdmin = document.getElementById('<%=chkAdmin.ClientID%>');
        oEnabled = document.getElementById('<%=chkEnabled.ClientID%>');
    }
    function ADCheck(oCheck, strUserName, strPNC, oFname, oLname, oIsManager, oBoard, oDirector, oHidden) {
        oHidden = document.getElementById(oHidden);
        oFname = document.getElementById(oFname);
        oLname = document.getElementById(oLname);
        oIsManager = document.getElementById(oIsManager);
        oBoard = document.getElementById(oBoard);
        oDirector = document.getElementById(oDirector);
        if (oCheck.checked == true && oFname.value == "") {
            oCheck.checked = false;
            alert('Please enter a first name');
            oFname.focus();
        }
        if (oCheck.checked == true && oLname.value == "") {
            oCheck.checked = false;
            alert('Please enter a last name');
            oLname.focus();
        }
        var strBoard = "0";
        if (oCheck.checked == true) {
            if (oBoard.checked == true)
                strBoard = "1";
        }
        var strDirector = "0";
        if (oCheck.checked == true) {
            if (oDirector.checked == true)
                strDirector = "1";
        }
        var strManager = "0";
        if (oCheck.checked == true) {
            if (oIsManager.checked == true)
                strManager = "1";
        }
        var strMultiple = "0";
        if (oCheck.checked == true) {
            if (oMultiple.checked == true)
                strMultiple = "1";
        }
        var strLocation = "0";
        if (oCheck.checked == true) {
            if (oLocation.checked == true)
                strLocation = "1";
            oHidden.value += strUserName + "&" + strPNC + "&" + oFname.value + "&" + oLname.value + "&" + strManager + "&" + strBoard + "&" + strDirector + "&" + strMultiple + "&" + strLocation + "&&";
        }
        else {
            var intFound = oHidden.value.indexOf(strUserName);
            var strBefore = oHidden.value.substring(0, intFound);
            var strAfter = oHidden.value;
            strAfter = oHidden.value.substring(oHidden.value.indexOf("&&", intFound) + 2);
            oHidden.value = strBefore + strAfter;
        }
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
        <td height="100%" valign="top">
<table width="98%" cellpadding="0" cellspacing="0" border="0" align="center">
		<tr><td colspan="2">&nbsp;</td></tr>
		<tr> 
		    <td><b>Users</b></td>
		    <td align="right"><a href="javascript:void(0);" onclick="Add();" class="cmlink" title="Click to Add New">Add New</a></td>
		</tr>
		<tr><td colspan="2">&nbsp;</td></tr>
		<tr>
		    <td colspan="2" align="center">
                <div ID="divView" runat="server" style="display:none">
                    <table width="95%" height="100%" border="0" cellspacing="0" cellpadding="0" align="center">
                        <tr height="1">
                            <td>&nbsp;</td>
                        </tr>
                        <tr height="1">
                            <td valign="top" class="default">Username / Last Name:</td>
                        </tr>
                        <tr height="1">
                            <td valign="top" class="default"><asp:textbox ID="txtSearch2" CssClass="default" runat="server" Width="200" MaxLength="50"/></td>
                        </tr>
                        <tr height="1">
                            <td valign="top" class="default"><asp:button ID="btnSearch2" CssClass="default" runat="server" Text="Search" Width="75" OnClick="btnSearch2_Click" /> <asp:button ID="Button2" CssClass="default" runat="server" Text="Cancel" Width="75" OnClick="btnCancelSearch_Click" /></td>
                        </tr>
                        <tr height="1"><td><hr size="1" noshade /></td></tr>
                    </table>
                    <table width="100%" border="0" cellpadding="0" cellspacing="0">
                        <tr>
                            <td>
                                <asp:repeater ID="rptView" runat="server">
                                    <HeaderTemplate>
                                        <table width="100%" cellpadding="3" cellspacing="0" border="0" align="center">
                                            <tr bgcolor="#CCCCCC">
                                                <td class="bold">&nbsp;</td>
                                                <td width="50%" class="bold"><asp:linkbutton ID="lnkName" Text="Name" CommandArgument="username" CssClass="bold" OnClick="OrderView" runat="server"/></td>
                                                <td width="50%" class="bold"><asp:linkbutton ID="lnkModified" Text="Last Modified" CommandArgument="modified" CssClass="bold" OnClick="OrderView" runat="server"/></td>
                                                <td class="bold" align="center"><asp:linkbutton ID="lnkEnabled" Text="Enabled" CommandArgument="enabled" CssClass="bold" OnClick="OrderView" runat="server"/></td>
                                            </tr>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr bgcolor="#EFEFEF" onmouseover="AdminMouseOver(this);" onmouseout="AdminMouseOut(this);" onclick="if(event.srcElement.tagName != 'INPUT')Edit('<%# DataBinder.Eval(Container.DataItem, "userid") %>','<%# DataBinder.Eval(Container.DataItem, "xid") %>','<%# DataBinder.Eval(Container.DataItem, "pnc_id") %>','<%# DataBinder.Eval(Container.DataItem, "fname").ToString().Replace("'", "\\'") %>','<%# DataBinder.Eval(Container.DataItem, "lname").ToString().Replace("'", "\\'") %>','<%# oUser.GetFullName(Int32.Parse(DataBinder.Eval(Container.DataItem, "manager").ToString())).Replace("'", "\\'") %>','<%# DataBinder.Eval(Container.DataItem, "manager") %>','<%# DataBinder.Eval(Container.DataItem, "ismanager") %>','<%# DataBinder.Eval(Container.DataItem, "board") %>','<%# DataBinder.Eval(Container.DataItem, "director") %>','<%# DataBinder.Eval(Container.DataItem, "pager") %>','<%# DataBinder.Eval(Container.DataItem, "atid") %>','<%# DataBinder.Eval(Container.DataItem, "phone") %>','<%# DataBinder.Eval(Container.DataItem, "other").ToString().Replace("'", "\\'") %>','<%# DataBinder.Eval(Container.DataItem, "vacation") %>','<%# DataBinder.Eval(Container.DataItem, "multiple_apps") %>','<%# DataBinder.Eval(Container.DataItem, "ungroup_projects") %>','<%# DataBinder.Eval(Container.DataItem, "show_returns") %>','<%# DataBinder.Eval(Container.DataItem, "add_location") %>','<%# DataBinder.Eval(Container.DataItem, "admin") %>','<%# DataBinder.Eval(Container.DataItem, "enabled") %>');" class="default">
                                            <td width="5%" align="left">&nbsp;<asp:ImageButton ID="btnDelete" OnClick="btnDeleteLink_Click" runat="server" ToolTip="Delete" ImageAlign="AbsMiddle" ImageUrl="/admin/images/delete.gif" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "userid") %>' /></td>
                                            <td width="50%">&nbsp;<%# DataBinder.Eval(Container.DataItem, "username") %></td>
                                            <td width="50%">&nbsp;<%# DataBinder.Eval(Container.DataItem, "modified") %></td>
                                            <td align="center"><asp:ImageButton ID="btnEnable" runat="server" ImageAlign="AbsMiddle" ImageUrl='<%# (DataBinder.Eval(Container.DataItem, "enabled").ToString() == "1") ? "/admin/images/enabled.gif" : "/admin/images/disabled.gif" %>' CommandArgument='<%# DataBinder.Eval(Container.DataItem, "userid") %>' OnClick="btnEnable_Click" /></td>
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
                    <table width="95%" height="100%" border="0" cellspacing="0" cellpadding="0" align="center">
                        <tr height="1">
                            <td>&nbsp;</td>
                        </tr>
                        <tr height="1">
                            <td valign="top" class="default">Please enter a username:</td>
                        </tr>
                        <tr height="1">
                            <td valign="top" class="default"><asp:textbox ID="txtName" CssClass="default" runat="server" Width="200" MaxLength="50"/></td>
                        </tr>
                        <tr height="1">
                            <td valign="top" class="default"><asp:button ID="btnSearch" CssClass="default" runat="server" Text="Search" Width="75" OnClick="btnSearch_Click" /></td>
                        </tr>
                        <tr height="1">
                            <td valign="top" align="center">
                            <asp:Label ID="lblResult" runat="server" CssClass="error" />
                            </td>
                        </tr>
                        <tr>
                            <td valign="top">
                                <asp:Table ID="tblResults" runat="server" Width="100%" CellPadding="2" CellSpacing="0" CssClass="default" /><br />
                                <asp:Button ID="btnSave" runat="server" Text="Add" CssClass="default" Width="75" OnClick="btnSave_Click" Visible="false" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="divEdit" runat="server" style="display:none">
                    <table width="95%" cellpadding="3" cellspacing="0" border="0" align="center">
                        <tr> 
                            <td class="default" width="100px">XID:</td>
                            <td><asp:textbox ID="txtUser" CssClass="default" runat="server" Width="150" MaxLength="30"/></td>
                        </tr>
                        <tr> 
                            <td class="default" width="100px">PNC ID:</td>
                            <td><asp:textbox ID="txtPNC" CssClass="default" runat="server" Width="150" MaxLength="30"/></td>
                        </tr>
                        <tr> 
                            <td class="default" width="100px">First name:</td>
                            <td><asp:textbox ID="txtFirst" CssClass="default" runat="server" Width="200" MaxLength="100"/></td>
                        </tr>
                        <tr> 
                            <td class="default" width="100px">Last name:</td>
                            <td><asp:textbox ID="txtLast" CssClass="default" runat="server" Width="200" MaxLength="100"/></td>
                        </tr>
                        <tr> 
                            <td class="default">Manager:</td>
                            <td>
                                <table cellpadding="0" cellspacing="0" border="0">
                                    <tr>
                                        <td><asp:TextBox ID="txtManager" runat="server" Width="300" CssClass="default" /></td>
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
                            <td class="default">Is Manager:</td>
                            <td><asp:CheckBox ID="chkManager" runat="server" Checked="true" /></td>
                        </tr>
                        <tr> 
                            <td class="default">Board Member:</td>
                            <td><asp:CheckBox ID="chkBoard" runat="server" Checked="true" /></td>
                        </tr>
                        <tr> 
                            <td class="default">Director:</td>
                            <td><asp:CheckBox ID="chkDirector" runat="server" Checked="true" /></td>
                        </tr>
                        <tr> 
                            <td class="default">Pager:</td>
                            <td><asp:textbox ID="txtPagers" CssClass="default" runat="server" Width="100" MaxLength="30"/> @ <asp:dropdownlist ID="ddlUserAt" CssClass="default" runat="server"/></td>
                        </tr>
                        <tr> 
                            <td class="default">Phone:</td>
                            <td><asp:textbox ID="txtPhone" CssClass="default" runat="server" Width="100" MaxLength="15"/></td>
                        </tr>
                        <tr> 
                            <td class="default">Special Skills:</td>
                            <td><asp:textbox ID="txtSkills" CssClass="default" runat="server" Width="400" Rows="5" TextMode="MultiLine"/></td>
                        </tr>
                        <tr> 
                            <td class="default">Vacation Days:</td>
                            <td><asp:textbox ID="txtVacation" CssClass="default" runat="server" Width="100" MaxLength="15"/></td>
                        </tr>
                        <tr> 
                            <td class="default">Multiple Applications:</td>
                            <td><asp:CheckBox ID="chkMultiple" runat="server" /></td>
                        </tr>
                        <tr> 
                            <td class="default">Ungroup Projects in Workload Manager:</td>
                            <td><asp:CheckBox ID="chkUngroupProjects" runat="server" /></td>
                        </tr>
                        <tr> 
                            <td class="default">Show Service Request Returns:</td>
                            <td><asp:CheckBox ID="chkShowReturns" runat="server" /></td>
                        </tr>
                        <tr> 
                            <td class="default">Can Add Locations:</td>
                            <td><asp:CheckBox ID="chkAddLocation" runat="server" /></td>
                        </tr>
                        <tr> 
                            <td class="default">Administrator:</td>
                            <td><asp:CheckBox ID="chkAdmin" runat="server" Checked="true" /></td>
                        </tr>
                        <tr> 
                            <td class="default">Enabled:</td>
                            <td><asp:CheckBox ID="chkEnabled" runat="server" Checked="true" /></td>
                        </tr>
                        <tr><td height="5" colspan="2"><img src="images/spacer.gif" width="1" height="5" /></td></tr>
                        <tr> 
                            <td class="cmdefault">&nbsp;</td>
                            <td><asp:Button ID="btnRoles" runat="server" Text="Edit Roles" Width="125" CssClass="default" /></td>
                        </tr>
                        <tr> 
                            <td class="cmdefault">&nbsp;</td>
                            <td><asp:Button ID="btnPages" runat="server" Text="Page Permissions" Width="125" CssClass="default" /></td>
                        </tr>
                        <tr> 
                            <td>&nbsp;</td>
                            <td>
                                <asp:button ID="btnAdd" CssClass="default" runat="server" Text="Add" Width="75" OnClick="btnAdd_Click" />
                                <asp:button ID="btnDelete" CssClass="default" runat="server" Text="Delete" Width="75" OnClick="btnDelete_Click" />
                                <asp:button ID="btnCancel" CssClass="default" runat="server" Text="Cancel" Width="75" />
                            </td>
                        </tr>
                        <tr><td height="5" colspan="2"><img src="images/spacer.gif" width="1" height="5" /></td></tr>
                        <tr>
                            <td>&nbsp;</td> 
                            <td><asp:Label ID="lblType" runat="server" CssClass="cmerror" /></td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
    </table>
        </td>
        </tr>
        </table>
    <input type="hidden" id="hdnId" runat="server" />
<input type="hidden" id="hdnUsers" runat="server" />
<asp:HiddenField ID="hdnManager" runat="server" />
</form>
</body>
</html>
