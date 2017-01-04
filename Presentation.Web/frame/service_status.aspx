<%@ Page Language="C#" MasterPageFile="~/frame.Master" AutoEventWireup="true" CodeBehind="service_status.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.service_status" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" Runat="Server">
<script type="text/javascript">
	function ShowHide(oDiv) {
		oDiv = document.getElementById(oDiv);
		if (oDiv.style.display == "none")
			oDiv.style.display = "inline";
		else
			oDiv.style.display = "none";
	}
function ChangeOrangeTab(oTD, oDivID, oHidden, oValue, boolDynamic, boolPrompt) {
    // Show DIV
    var strHide = oDivID.substring(0, 12);
	var oDivs = document.getElementsByTagName("DIV");
	for (var ii=0; ii<oDivs.length; ii++) {
	    if (boolDynamic == null || boolDynamic == true) {
	        if (oDivs[ii].id.substring(0,12) == strHide)
	            oDivs[ii].style.display = "none";
	    }
	    else {
            if (oDivs[ii].id.substring(oDivs[ii].id.lastIndexOf('_')+1,oDivs[ii].id.length-1) == strHide)	   
                oDivs[ii].style.display = "none";	        
        }
    }
	oDivID = document.getElementById(oDivID);
	oDivID.style.display = "inline";
	// Change TAB
    	var oRow = oTD.parentElement;
	    for (var yy=0; yy<oRow.children.length; yy++) {
    		var oNot = oRow.getElementsByTagName("td").item(yy);
    		if (oNot.className == "cmbutton")
                oNot.style.border = "1px solid #94a6b5"
    	}
	    oTD.style.borderTop = "3px solid orange"
        oTD.style.borderBottom = "1px solid #FFFFFF"
	if (oHidden != null) {
	    oHidden = document.getElementById(oHidden);
	    oHidden.value = oValue;
	}
}
</script>
<table width="100%" height="100%" cellpadding="2" cellspacing="2" border="0">
    <tr height="1">
        <td align="right" width="50%"><a href="javascript:void(0);" onclick="window.print();"><img src='/images/print-icon.gif' border='0' align='absmiddle' />Print Page</a></td>
        <td align="center"><img src='/images/spacer.gif' border='0' width='10' height='1' /></td>
        <td align="left" width="50%"><a href="javascript:void(0);" onclick="parent.HidePanel();"><img src='/images/close-icon.gif' border='0' align='absmiddle' /><img src='/images/spacer.gif' border='0' width='5' height='1' />Close Window</a></td>
    </tr>
    <tr>
        <td colspan="3" height="100%" valign="top">
            <div style="height:100%; overflow:auto">
            <!--
                <table width="100%" height="100%" border="0" cellSpacing="0" cellPadding="0" class="default">
                    <tr>
                        <td class="cmbutton" style='<%=hdnTab.Value == "1" ? "display:inline;border-bottom:1px solid #FFFFFF;border-top:3px solid orange;" : "" %>' onclick="ChangeOrangeTab(this,'divOrangeTab1','<%=hdnTab.ClientID%>','1');">Information</td>
                        <td class="cmbuttonspace">&nbsp;</td>
                        <td class="cmbutton" style='<%=hdnTab.Value == "2" ? "display:inline;border-bottom:1px solid #FFFFFF;border-top:3px solid orange;" : "" %>' onclick="ChangeOrangeTab(this,'divOrangeTab2','<%=hdnTab.ClientID%>','2');">Assignment</td>
                        <td class="cmbuttonspace">&nbsp;</td>
                        <td class="cmbutton" style='<%=hdnTab.Value == "3" ? "display:inline;border-bottom:1px solid #FFFFFF;border-top:3px solid orange;" : "" %>' onclick="ChangeOrangeTab(this,'divOrangeTab3','<%=hdnTab.ClientID%>','3');">Approvals</td>
                        <td class="cmbuttonspace">&nbsp;</td>
                        <td class="cmbutton" style='<%=hdnTab.Value == "4" ? "display:inline;border-bottom:1px solid #FFFFFF;border-top:3px solid orange;" : "" %>' onclick="ChangeOrangeTab(this,'divOrangeTab4','<%=hdnTab.ClientID%>','4');">Workflow</td>
                    </tr>
                    <tr>
                        <td colspan="7" align="center" class="cmcontents">
                            <div id="divOrangeTab1"  style='<%=hdnTab.Value == "1"%> ? "display:inline" : "display:none" %>'>
                                <br />
                                <table width="100%" cellpadding="0" cellspacing="5" border="0">
                                    <tr>
                                        <td rowspan="2" valign="top"><img src="/images/tasks.gif" border="0" align="absmiddle" /></td>
                                        <td class="hugeheader" width="100%" valign="bottom">My Tasks</td>
                                    </tr>
                                    <tr>
                                        <td width="100%" valign="top">Here you can find all the tasks that you are responsible for completing.</td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" background="/images/lines.gif">&nbsp;</td>
                                    </tr>
                                </table><br />
                            </div>
                            <div id="divOrangeTab2"  style='<%=hdnTab.Value == "2"%> ? "display:inline" : "display:none" %>'>
                            </div>
                            <div id="divOrangeTab3"  style='<%=hdnTab.Value == "3"%> ? "display:inline" : "display:none" %>'>
                            </div>
                            <div id="divOrangeTab4"  style='<%=hdnTab.Value == "4"%> ? "display:inline" : "display:none" %>'>
                            </div>
                        </td>
                    </tr>
                </table>
                -->
            </div>
        </td>
    </tr>
</table>
<asp:HiddenField ID="hdnTab" runat="server" />
</asp:Content>
