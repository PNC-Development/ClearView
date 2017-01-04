<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wm_storage.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.wm_storage" %>

<script type="text/javascript">
    function ChangeFrame(oCell, oShow, oHide1, oHide2, oHidden, strHidden) {
        oShow = document.getElementById(oShow);
        oShow.style.display = "inline";
        oHide1 = document.getElementById(oHide1);
        oHide1.style.display = "none";
        oHide2 = document.getElementById(oHide2);
        oHide2.style.display = "none";
    	var oRow = oCell.parentElement;
	    for (var yy=0; yy<oRow.children.length; yy++) {
    		var oNot = oRow.getElementsByTagName("td").item(yy);
    		if (oNot.className == "cmbutton")
                oNot.style.border = "1px solid #94a6b5"
    	}
	    oCell.style.borderTop = "3px solid #6A8359"
        oCell.style.borderBottom = "1px solid #FFFFFF"
        oHidden = document.getElementById(oHidden);
        oHidden.value = strHidden;
    }
    function UpdateDDL(oDDL, oNumber) {
        oNumber = document.getElementById(oNumber);
        oNumber.value = oDDL.options[oDDL.selectedIndex].value;
    }
    function UpdateText(oText, oNumber) {
        oNumber = document.getElementById(oNumber);
        if (oText.value == "" || isNumber(oText.value) == false)
            oText.value = 0;
        oNumber.value = oText.value;
    }
    
    function UpdateTextValue(oText, oNumber) {
        oNumber = document.getElementById(oNumber);
        oNumber.value = oText.value;
    }
    function EnsureStatus(oTab, oStatus, oComments) {
        oTab = document.getElementById(oTab);
        if (oTab.value == "S")
            return ValidateStatus(oStatus, oComments);
        else
            return true;
    }
    function EnsureTextbox() {
        var boolReturn = true;
        var oTexts = document.getElementsByName("ValidTextbox");
        for (var ii=0; ii<oTexts.length && boolReturn == true; ii++) {
            if (oTexts[ii].disabled == false) {
                if (oTexts[ii].value == "") {
                    boolReturn = false;
                    alert('Please enter a valid value');
                    oTexts[ii].focus();
                }
            }
        }
        return boolReturn;
    }
    function EnsureTextbox0() {
        var boolReturn = true;
        var boolWarning = null;
        var oTexts = document.getElementsByName("ValidTextbox0");
        for (var ii=0; ii<oTexts.length && boolReturn == true; ii++) {
            if (oTexts[ii].disabled == false) {
                if (oTexts[ii].value == "" || isInt(oTexts[ii].value) == false || parseInt(oTexts[ii].value) < 0) {
                    boolReturn = false;
                    alert('Please enter a valid WHOLE number (no decimals)');
                    oTexts[ii].focus();
                }
                else if (parseInt(oTexts[ii].value) == 0) {
                    boolWarning = oTexts[ii];
                }
            }
        }
        if (boolWarning != null) {
            if (confirm('One or more of the amounts entered equal 0\n\nAre you sure you want to continue?') == false) {
                boolReturn = false;
                boolWarning.focus();
            }
        }
        return boolReturn;
    }
</script>
<asp:Panel ID="panWorkload" runat="server" Visible="false">
    <table width="100%" border="0" cellSpacing="0" cellPadding="0" class="default">
        <tr>
            <td>
                <table width="100%" border="0" cellSpacing="0" cellPadding="0">
                    <tr id="cntrlButtons">
                        <td><asp:ImageButton ID="btnSave" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/tool_save.gif" OnClick="btnSave_Click" /></td>
                        <td><asp:ImageButton ID="btnPrint" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/tool_print.gif" /></td>
                        <td><asp:ImageButton ID="btnEmail" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/tool_email.gif" /></td>
                        <td><asp:ImageButton ID="btnSLA" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/tool_sla.gif" /></td>
                        <td><asp:ImageButton ID="btnComplete" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/tool_complete.gif" OnClick="btnComplete_Click" /></td>
                        <td><asp:ImageButton ID="btnReturn" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/tool_return.gif" /></td>
                        <td width="100%">
                            <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                <tr>
                                    <td><img src="/images/tool_left.gif" border="0" width="6" height="26"></td>
                                    <td nowrap background="/images/tool_back.gif" width="100%"><img src="/images/tool_back.gif" border="0" width="2" height="26"></td>
                                    <td><img src="/images/tool_right.gif" border="0" width="6" height="26"></td>
                                </tr>
                            </table>
                        </td>
                        <td><asp:ImageButton ID="btnClose" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/tool_close.gif" /></td>
                    </tr>
                    <tr id="cntrlProcessing" style="display:none">
                        <td colspan="20">
                            <table width="100%" cellpadding="0" cellspacing="5" border="0">
                                <tr>
                                    <td rowspan="2"><img src="/images/saving.gif" border="0" align="absmiddle" /></td>
                                    <td class="header" width="100%" valign="bottom">Processing...</td>
                                </tr>
                                <tr>
                                    <td width="100%" valign="top">Please do not close this window while the page is processing.  Please be patient....</td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td class="default">
                <table width="100%" border="0" cellSpacing="2" cellPadding="2" class="default">
                    <tr>
                        <td valign="top">
                            <table border="0" cellpadding="3" cellspacing="2">
                                <tr>
                                    <td nowrap><b>Name:</b></td>
                                    <td><asp:Label ID="lblName" runat="server" CssClass="default" /></td>
                                </tr>
                                <tr>
                                    <td nowrap><b>Number:</b></td>
                                    <td><asp:Label ID="lblNumber" runat="server" CssClass="default" /></td>
                                </tr>
                                <tr>
                                    <td nowrap><b>Type:</b></td>
                                    <td><asp:Label ID="lblType" runat="server" CssClass="default" /></td>
                                </tr>
                                <tr>
                                    <td nowrap><b>Service:</b></td>
                                    <td><asp:Label ID="lblService" runat="server" CssClass="default" /></td>
                                </tr>
                                <tr>
                                    <td nowrap><b>Last Updated:</b></td>
                                    <td><asp:Label ID="lblUpdated" runat="server" CssClass="default" /></td>
                                </tr>
                                <tr>
                                    <td nowrap><b>Nickname:</b></td>
                                    <td><asp:TextBox ID="txtCustom" runat="server" CssClass="default" Width="300" MaxLength="100" /></td>
                                </tr>
                            </table>
                        </td>
                        <td valign="top" align="right">
                            <table border="0" cellpadding="3" cellspacing="2">
                                <tr>
                                    <td colspan="2"><iframe src="/frame/did_you_know.aspx" frameborder="0" scrolling="no" width="300" height="135"></iframe></td>
                                </tr>
                                <tr>
                                    <td nowrap><b>Overall Status:</b></td>
                                    <td><asp:Label ID="lblStatus" runat="server" CssClass="default" /></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Panel ID="panDeleted" runat="server" Visible="false">
                <table width="100%" cellpadding="3" cellspacing="2" border="0" style="border:solid 1px #999999" bgcolor="#f9f9f9">
                    <tr>
                        <td rowspan="3"><img src="/images/bigAlert.gif" border="0" align="absmiddle" /></td>
                        <td class="bigred" width="100%">Design Was Deleted</td>
                    </tr>
                    <tr>
                        <td width="100%">The design for this project was deleted by the requestor. Please click the <b>Delete Request</b> button to remove it from your workload.</td>
                    </tr>
                    <tr>
                        <td width="100%"><asp:Button ID="btnDelete" runat="server" CssClass="default" width="125" Text="Delete Request" OnClick="btnDelete_Click" /></td>
                    </tr>
                </table>
                </asp:Panel>
            </td>
        </tr>
        <tr height="16">
            <td><img src="/images/spacer.gif" border="0" height="16" width="1" /></td>
        </tr>
        <tr>
            <td>
                    <table width="100%" height="100%" border="0" cellSpacing="0" cellPadding="0" class="default">
                        <tr>
                            <td class="cmbutton" style='<%=boolDetails == true ? "display:inline;border-bottom:1px solid #FFFFFF;border-top:3px solid orange;" : "" %>' onclick="ChangeFrame(this,'divDetails','divExecution','divStatus','<%=hdnTab.ClientID %>','D');">Request Details</td>
                            <td class="cmbuttonspace">&nbsp;</td>
                            <td class="cmbutton" style='<%=boolExecution == true ? "display:inline;border-bottom:1px solid #FFFFFF;border-top:3px solid orange;" : "" %>' onclick="ChangeFrame(this,'divExecution','divDetails','divStatus','<%=hdnTab.ClientID %>','E');">Execution</td>
                            <td class="cmbuttonspace">&nbsp;</td>
                            <td class="cmbutton" style='<%=boolStatus == true ? "display:inline;border-bottom:1px solid #FFFFFF;border-top:3px solid orange;" : "" %>' onclick="ChangeFrame(this,'divStatus','divDetails','divExecution','<%=hdnTab.ClientID %>','S');">Weekly Status</td>
                        </tr>
                        <tr>
                            <td colspan="5" align="center" class="cmcontents">
                                <table width="100%" height="100%" border="0" cellSpacing="2" cellPadding="2" class="default">
                                    <tr> 
                                        <td valign="top">
		                                    <div id="divDetails" style='<%=boolDetails == true ? "display:inline" : "display:none" %>'>
		                                        <br />
                                                <table width="100%" cellpadding="4" cellspacing="3" border="0">
                                                    <tr>
                                                        <td colspan="2" class="greentableheader">Design Details</td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2"><img src="/images/bigInfo.gif" border="0" align="absmiddle" /> <asp:LinkButton ID="btnView" runat="server" Text="Click Here to View the Design Details" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td class="greentableheader">Request Details</td>
                                                    </tr>
                                                    <tr>
                                                        <td><asp:Label ID="lblView" runat="server" CssClass="default" /></td>
                                                    </tr>
                                                </table>
                                                <p>&nbsp;</p>
		                                    </div>
		                                    <div id="divExecution" style='<%=boolExecution == true ? "display:inline" : "display:none" %>'>
		                                        <br />
                                                <table width="100%" cellpadding="4" cellspacing="3" border="0">
                                                    <tr>
                                                        <td nowrap><asp:CheckBox ID="chk1" runat="server" CssClass="default" /></td>
                                                        <td nowrap><asp:Image ID="img1" runat="server" ImageAlign="absMiddle" ImageUrl="/images/spacer.gif" /></td>
                                                        <td width="100%">Finished Configuring Storage: Total Hours = <asp:TextBox ID="txtHours" runat="server" CssClass="default" Width="100" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td nowrap></td>
                                                        <td nowrap></td>
                                                        <td width="100%"><asp:Label ID="lbl1" runat="server" CssClass="default" /></td>
                                                    </tr>
                                                </table>
		                                    </div>
		                                    <div id="divStatus" style='<%=boolStatus == true ? "display:inline" : "display:none" %>'>
		                                        <br />
                                                <table width="100%" cellpadding="4" cellspacing="3" border="0">
                                                    <tr>
                                                        <td colspan="2" class="greentableheader">Weekly Status</td>
                                                    </tr>
                                                    <tr>
                                                        <td nowrap>Status:</td>
                                                        <td width="100%">
                                                            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="default" >
                                                                <asp:ListItem Text="-- SELECT --" Value="0" />
                                                                <asp:ListItem Text="Red" Value="1" />
                                                                <asp:ListItem Text="Yellow" Value="2" />
                                                                <asp:ListItem Text="Green" Value="3" />
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2"><b>Comments / Issues:</b></td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2"><asp:TextBox ID="txtComments" runat="server" CssClass="default" Width="100%" TextMode="MultiLine" Rows="13" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">&nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <table width="100%" cellpadding="3" cellspacing="0" border="0" style="border:solid 1px #CCCCCC">
                                                                <tr bgcolor="#EEEEEE">
                                                                    <td nowrap><b>Date</b></td>
                                                                    <td nowrap>&nbsp;</td>
                                                                    <td nowrap><b>Status</b></td>
                                                                </tr>
                                                                <asp:repeater ID="rptStatus" runat="server">
                                                                    <ItemTemplate>
                                                                        <tr class="default">
                                                                            <td nowrap><a href="javascript:void(0);" onclick="ShowStatus('<%# DataBinder.Eval(Container.DataItem, "id") %>','r');"><%# DateTime.Parse(DataBinder.Eval(Container.DataItem, "modified").ToString()).ToShortDateString() %>&nbsp;<%# DateTime.Parse(DataBinder.Eval(Container.DataItem, "modified").ToString()).ToShortTimeString() %></a></td>
                                                                            <td nowrap>&nbsp;</td>
                                                                            <td width="100%"><asp:Label ID="lblStatus" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "status") %>' /></td>
                                                                        </tr>
                                                                    </ItemTemplate>
                                                                </asp:repeater>
                                                            <tr>
                                                                <td colspan="3">
                                                                    <asp:Label ID="lblNoStatus" runat="server" CssClass="default" Visible="false" Text="<img src='/images/alert.gif' border='0' align='absmiddle'> There are no updates" />
                                                                </td>
                                                            </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
		                                    </div>
		                                </td>
		                            </tr>
		                        </table>
		                    </td>
	                    </tr>
	                </table>
            </td>
        </tr>
    </table>
</asp:Panel>
<asp:Panel ID="panDenied" runat="server" Visible="false">
    <table width="100%" cellpadding="2" cellspacing="2" border="0">
        <tr>
            <td><img src="/images/spacer.gif" border="0" width="15" height="1" /></td>
            <td class="header"><img src="/images/bigX.gif" border="0" align="absmiddle" /> Access Denied</td>
        </tr>
        <tr><td colspan="2">&nbsp;</td></tr>
        <tr>
            <td><img src="/images/spacer.gif" border="0" width="15" height="1" /></td>
            <td>You do not have rights to view this item.</td>
        </tr>
        <tr><td colspan="2">&nbsp;</td></tr>
        <tr>
            <td colspan="2"><hr size="1" noshade /></td>
        </tr>
        <tr>
            <td colspan="2" align="right">
                <table width="100%" cellpadding="0" cellspacing="0" border="0">
                    <tr>
                        <td class="footer"></td>
                        <td align="right"><asp:Button ID="btnDenied" runat="server" CssClass="default" Width="75" Text="Close" /></td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Panel>
<asp:Label ID="lblResourceWorkflow" runat="server" Visible="false" />
<asp:Label ID="lblAnswer" runat="server" Visible="false" />
<asp:Label ID="lblProd" runat="server" Visible="false" />
<asp:Label ID="lblModel" runat="server" Visible="false" />
<asp:HiddenField ID="hdnTab" runat="server" />