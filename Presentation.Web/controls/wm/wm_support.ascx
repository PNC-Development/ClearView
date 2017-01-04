<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wm_support.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.wm_support" %>

<script type="text/javascript">
    function ChangeFrame(oCell, oShow, oHide1, oHidden, strHidden) {
        oShow = document.getElementById(oShow);
        oShow.style.display = "inline";
        oHide1 = document.getElementById(oHide1);
        oHide1.style.display = "none";
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
</script>
<asp:Panel ID="panWorkload" runat="server" Visible="false">
    <table width="100%" border="0" cellSpacing="0" cellPadding="0" class="default">    
        <tr>
            <td>
                <table width="100%" border="0" cellSpacing="0" cellPadding="0">
                    <tr id="cntrlButtons">
                        <td><asp:ImageButton ID="btnSave" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/tool_save.gif" OnClick="btnSave_Click" /></td>
                        <td><asp:ImageButton ID="btnPrint" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/tool_print.gif" /></td>
                        <td><asp:ImageButton ID="btnComplete" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/tool_complete.gif" OnClick="btnComplete_Click" /></td>
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
                                    <td nowrap><b>Requested By:</b></td>
                                    <td><asp:Label ID="lblRequestedBy" runat="server" CssClass="default" /></td>
                                </tr>
                                <tr>
                                    <td nowrap><b>Requested On:</b></td>
                                    <td><asp:Label ID="lblRequestedOn" runat="server" CssClass="default" /></td>
                                </tr>
                                <tr>
                                    <td nowrap><b>Last Updated:</b></td>
                                    <td><asp:Label ID="lblUpdated" runat="server" CssClass="default" /></td>
                                </tr>
                                <tr>
                                    <td nowrap><b>Custom Task Name:</b></td>
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
                    <tr>
                        <td colspan="2">
                            <table border="0" cellpadding="3" cellspacing="2">
                                <tr>
                                    <td><b>Original Request Details:</b>&nbsp;&nbsp;<asp:Label ID="lblDescription" runat="server" CssClass="default" /></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr height="16">
            <td><img src="/images/spacer.gif" border="0" height="16" width="1" /></td>
        </tr>
        <tr>
            <td>
                    <table width="100%" height="100%" border="0" cellSpacing="0" cellPadding="0" class="default">
                        <tr>
                            <td class="cmbutton" style='<%=boolDetails == true ? "display:inline;border-bottom:1px solid #FFFFFF;border-top:3px solid orange;" : "" %>' onclick="ChangeFrame(this,'divDetails','divExecution','<%=hdnTab.ClientID %>','D');">Request Details</td>
                            <td class="cmbuttonspace">&nbsp;</td>
                            <td class="cmbutton" style='<%=boolExecution == true ? "display:inline;border-bottom:1px solid #FFFFFF;border-top:3px solid orange;" : "" %>' onclick="ChangeFrame(this,'divExecution','divDetails','<%=hdnTab.ClientID %>','E');">Execution</td>
                        </tr>
                        <tr>
                            <td colspan="9" align="center" class="cmcontents">
                                <table width="100%" height="100%" border="0" cellSpacing="2" cellPadding="2" class="default">
                                    <tr> 
                                        <td valign="top">
		                                    <div id="divDetails" style='<%=boolDetails == true ? "display:inline" : "display:none" %>'>
		                                        <br />
                                                <table width="100%" cellpadding="4" cellspacing="3" border="0">
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
                                                        <td>Status:</td>
                                                        <td width="100%">
                                                            <asp:DropDownList ID="ddlCurrentStatus" runat="server" CssClass="default" >                                                                 
                                                            </asp:DropDownList>
                                                        </td> 
                                                    </tr>
                                                    <tr id="trPriority" runat="server" visible="false">
                                                        <td nowrap>Priority:</td>
                                                        <td width="100%">
                                                            <asp:RadioButtonList ID="radPriority" runat="server" RepeatDirection="horizontal">
                                                                <asp:ListItem Value="Highest" />
                                                                <asp:ListItem Value="High" />
                                                                <asp:ListItem Value="Medium" Selected="True" />
                                                                <asp:ListItem Value="Low" />
                                                                <asp:ListItem Value="Lowest" />
                                                            </asp:RadioButtonList>
                                                        </td>
                                                    </tr>
                                                    <tr id="trRelease" runat="server" visible="false">
                                                        <td nowrap>Release Date:</td>
                                                        <td width="100%"><asp:DropDownList ID="ddlRelease" runat="server" Width="200" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td nowrap>Completed (%):</td>
                                                        <td width="100%"><SliderCC:Slider ID="sldHours" _ParentElement="divExecution" _Hidden="hdnHours" runat="server" _DivClass="default" _PercentClass="result" _Width="300" _ShowDiv="true" _RestrictLess="true" /> <asp:Label ID="lblHours" runat="server" CssClass="required" Visible="false" Text="No hours have been allocated for this initiative" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">&nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2" class="greentableheader">Thread&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:ImageButton ID="btnMessage" runat="server" ImageUrl="/images/post_reply.gif" ImageAlign="AbsMiddle" ToolTip="Click to Post a Reply" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <div id="divMessage" runat="server" style="display:none">
                                                                <table width="100%" cellpadding="3" cellspacing="0" border="0">
                                                                    <tr>
                                                                        <td>Enter your response, upload an attachment (optional) and click <b>Save</b> to send your response:</td>
                                                                    </tr>
		                                                            <tr>
                                                                        <td><asp:TextBox ID="txtText" runat="server" CssClass="default" Width="600" TextMode="MultiLine" Rows="7" MaxLength="8000" /></td>
                                                                    </tr>  
                                                                    <tr>
                                                                        <td><asp:FileUpload id="oFile" runat="server" CssClass="default" Width="600" /></td>
                                                                    </tr>
                                                                </table>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">    
                                                        <br />
                                                        <%=strMessages %>
                                                        <br />                                                        
                                                       </td>
                                                    </tr>                                                                                        
		                                        </table>	                                    
		                                    </div>
		                                    <!-- Vijay Code - End -->
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
<asp:HiddenField ID="hdnTab" runat="server" />
 