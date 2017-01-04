<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="project_request_new_dynamic_admin.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.project_request_new_dynamic_admin" %>


<script type="text/javascript">
// Vijay Code - Start
 
function ReceiveServerData(arg, context)
{  
window.top.navigate(arg);

}

function UpdateHidden2(strQuestion,oHidden, oControl) {      
	var oHidden = document.getElementById(oHidden);		
	var strValue = strQuestion +":"+ oControl.options[oControl.selectedIndex].value+"&";
	oHidden.value+=strValue;	  
}
// Vijay Code - End

function MoveList(ddlFrom, ddlTo, oHidden, ddlUpdate) {
    ddlFrom = document.getElementById(ddlFrom);
    ddlTo = document.getElementById(ddlTo);
    ddlUpdate = document.getElementById(ddlUpdate);
	if (ddlFrom.selectedIndex > -1) {
		var oOption = document.createElement("OPTION");
		ddlTo.add(oOption);
		oOption.text = ddlFrom.options[ddlFrom.selectedIndex].text;
		oOption.value = ddlFrom.options[ddlFrom.selectedIndex].value;
		ddlFrom.remove(ddlFrom.selectedIndex);
		ddlTo.selectedIndex = ddlTo.length - 1;
		UpdateHidden(oHidden, ddlUpdate);
	}
	return false;
}
function UpdateHidden(oHidden, oControl) {
	var oHidden = document.getElementById(oHidden);
	oHidden.value = "";
	for (var ii=0; ii<oControl.length; ii++) {
		oHidden.value = oHidden.value + oControl.options[ii].value + "_" + ii + "&";
	}
	 
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
            <table width="100%" cellpadding="4" cellspacing="3" border="0">
                <tr>
                    <td colspan="2" align="center"><asp:Label ID="lblError" runat="server" CssClass="bigerror" Text="<br /><img src='/images/bigX.gif' border='0' align='absmiddle' /> There was a problem saving the information" Visible="false" /></td>
                </tr>
                <tr>
                    <td colspan="2">Please choose the project type and portfolio you wish to change and click the &quot;View Form&quot; button.</td>
                </tr>
                <tr>
                    <td colspan="2">
                        <table width="100%" cellpadding="4" cellspacing="3" border="0">
                            <tr>
                                <td nowrap>Project Type:</td>
                                <td width="100%">
                                    <asp:DropDownList ID="ddlBaseDisc" runat="server" CssClass="default" ToolTip="If your initative is considerd Discretionary project DO NOT USE THIS FORM">
                                        <asp:ListItem Value="0" Text="-- SELECT --" />
                                        <asp:ListItem Value="Base" />
                                        <asp:ListItem Value="Discretionary" />
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td nowrap>Portfolio:</td>
                                <td width="100%"><asp:DropDownList ID="ddlOrganization" runat="server" CssClass="default" Width="400" ToolTip="Select the organization sponsoring this initiative. This could be your organization." /></td>
                            </tr>
                            <tr>
                                <td nowrap></td>
                                <td width="100%"><asp:Button ID="btnView" runat="server" Text="View Form" CssClass="default" Width="100" OnClick="btnView_Click" /> <asp:Button ID="btnCopy" runat="server" Text="Copy Form to Another Portfolio" CssClass="default" Width="200" Visible="false" /></td>
                            </tr>
                        </table>
                    </td>
                </tr>                  
                <asp:Panel ID="panShow" runat="server" Visible="false">
                <tr>
                   <td class="default"><asp:CheckBox ID="chkAll" runat="server" Text="Collapse All" CssClass="default" onClick="CallCheck(this.checked,'')" Visible="false" /></td>
                   <td align="right"><img src="/images/move.gif" border="0" align="absmiddle" /> = Click and Drag to Move a Question/Response&nbsp;&nbsp;&nbsp;&nbsp;<img src="/images/edit.gif" border="0" align="absmiddle" /> = Click to Edit the Question/Response</td>
                </tr> 
                <tr>
                    <td colspan="2"><span style="width:100%;border-bottom:1 dotted #CCCCCC;"/></td>
                </tr>
                <tr>                  
                    <td colspan="2">
                       <%= strHTML %>                           
                    </td>
                </tr>
                <tr>
                    <td colspan="2"><hr size="1" noshade /></td>
                </tr>           
                <tr>
                    <td colspan="2" align="right">
                        <table width="100%" cellpadding="0" cellspacing="0" border="0">
                            <tr>
                                <td class="required">* = Required Field</td>                                     
                            </tr>
                        </table>
                    </td>
                </tr>
                </asp:Panel>
            </table>
        </td>
        <td background="/images/table_right.gif"><img src="/images/table_right.gif" width="5" height="10"></td>
    </tr>
    <tr>
        <td><img src="/images/table_bottomLeft.gif" border="0" width="5" height="9"></td>
        <td width="100%" background="/images/table_bottom.gif"></td>
        <td><img src="/images/table_bottomRight.gif" border="0" width="5" height="9"></td>
    </tr>
</table>
<asp:HiddenField ID="hdnExecutive" runat="server" />
<asp:HiddenField ID="hdnWorking" runat="server" />
<asp:HiddenField ID="hdnPlatforms" runat="server" />
<asp:HiddenField ID="hdnManager" runat="server" />
<asp:HiddenField ID="hdnError" runat="server" />
<asp:Label ID="lblProject" runat="server" Visible="false" />
<asp:HiddenField ID="hdnSegment" runat="server" />
<asp:HiddenField ID="hdnSubmissionID" runat="server" />
<asp:HiddenField ID="hdnResponseID" runat ="server" /> 
<asp:HiddenField ID="hdnOrder" runat="server" />
<asp:HiddenField ID="hdnQOrder" runat="server" />
<asp:HiddenField ID="hdnQuestionID" runat="server" />
<asp:HiddenField ID="hdnBD" runat="server" />
<asp:HiddenField ID="hdnOID" runat="server" />
 