<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wm_pnc_task.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.wm_pnc_task" %>


<script type="text/javascript">
    function ChangeFrame(oCell, oShow, oHide1, oHide2, oHide3, oHidden, strHidden) {
        oShow = document.getElementById(oShow);
        oShow.style.display = "inline";
        oHide1 = document.getElementById(oHide1);
        oHide1.style.display = "none";
        oHide2 = document.getElementById(oHide2);
        oHide2.style.display = "none";
        oHide3 = document.getElementById(oHide3);
        oHide3.style.display = "none";
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
    function UpdateTextValue(oText, oNumber) {
        oNumber = document.getElementById(oNumber);
        oNumber.value = oText.value;
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
                            <td class="cmbutton" style='<%=boolDetails == true ? "display:inline;border-bottom:1px solid #FFFFFF;border-top:3px solid orange;" : "" %>' onclick="ChangeFrame(this,'divDetails','divExecution','divChange','divDocuments','<%=hdnTab.ClientID %>','D');">Request Details</td>
                            <td class="cmbuttonspace">&nbsp;</td>
                            <td class="cmbutton" style='<%=boolExecution == true ? "display:inline;border-bottom:1px solid #FFFFFF;border-top:3px solid orange;" : "" %>' onclick="ChangeFrame(this,'divExecution','divDetails','divChange','divDocuments','<%=hdnTab.ClientID %>','E');">Execution</td>
                            <td class="cmbuttonspace">&nbsp;</td>
                            <td class="cmbutton" style='<%=boolChange == true ? "display:inline;border-bottom:1px solid #FFFFFF;border-top:3px solid orange;" : "" %>' onclick="ChangeFrame(this,'divChange','divDetails','divExecution','divDocuments','<%=hdnTab.ClientID %>','C');">Change Controls</td>
                            <td class="cmbuttonspace">&nbsp;</td>
                            <td class="cmbutton" style='<%=boolDocuments == true ? "display:inline;border-bottom:1px solid #FFFFFF;border-top:3px solid orange;" : "" %>' onclick="ChangeFrame(this,'divDocuments','divDetails','divExecution','divChange','<%=hdnTab.ClientID %>','D');">Attached Files</td>
                        </tr>
                        <tr>
                            <td colspan="7" align="center" class="cmcontents">
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
                                                <asp:Panel ID="panSecurity" runat="server" Visible="false">
                                                    <table width="100%" cellpadding="4" cellspacing="3" border="0">
                                                        <tr>
                                                            <td class="greentableheader">Group Configuration Results</td>
                                                        </tr>
                                                        <tr>
                                                            <td><asp:Label ID="lblSecurityPre" runat="server" CssClass="default" /></td>
                                                        </tr>
                                                        <tr>
                                                            <td>&nbsp;</td>
                                                        </tr>
                                                        <tr>
                                                            <td class="greentableheader">Account Configuration Results</td>
                                                        </tr>
                                                        <tr>
                                                            <td><asp:Label ID="lblSecurityPost" runat="server" CssClass="default" /></td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                                <p>&nbsp;</p>
		                                    </div>
		                                    <div id="divExecution" style='<%=boolExecution == true ? "display:inline" : "display:none" %>'>
		                                        <br />
                                                <asp:Panel ID="panCluster" runat="server" Visible="false">
                                                    <table cellpadding="4" cellspacing="3">
                                                        <tr>
                                                            <td class="greentableheader">Cluster Configuration</td>
                                                            <%=strCluster %>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                                <asp:Panel ID="panLTM" runat="server" Visible="false">
                                                    <table cellpadding="4" cellspacing="3">
                                                        <tr>
                                                            <td class="greentableheader">Installation Details</td>
                                                        </tr>
                                                        <tr>
                                                            <td nowrap class="bold">VIP Name:</td>
                                                            <td><asp:Label ID="lblVipName" runat="server" CssClass="default" /></td>
                                                        </tr>
                                                        <tr id="trVip" runat="server">
                                                            <td nowrap class="bold">VIP IP Address:</td>
                                                            <td><asp:Label ID="lblVip" runat="server" CssClass="default" /></td>
                                                        </tr>
                                                        <tr id="trConfig" runat="server" visible="false">
                                                            <td colspan="2"><img src="/images/file.gif" border="0" align="absmiddle" /> <asp:HyperLink ID="btnConfig" runat="server" CssClass="default" Text="View Config" Target="_blank"/></td>
                                                        </tr>
                                                    </table>
                                                    <br />
                                                    <table cellpadding="4" cellspacing="3" border="0">
                                                        <tr>
                                                            <td><b>Server:</b></td>
                                                            <td><b>IP Addresss:</b></td>
                                                            <td><b>VLAN:</b></td>
                                                        </tr>
                                                        <%=strConfig %>
                                                    </table>
                                                    <br />
                                                    <table width="75%" cellpadding="4" cellspacing="3">
                                                        <tr>
                                                            <td class="greentableheader">Installation Results</td>
                                                        </tr>
                                                        <tr id="panLTMStatusNo" runat="server" visible="false">
                                                            <td><img src="/images/bigAlert.gif" border="0" align="absmiddle" /> <b>NOTE:</b> Once you click SAVE, the results of the installation will appear</td>
                                                        </tr>
                                                        <tr id="panLTMStatusPending" runat="server" visible="false">
                                                            <td><img src="/images/bigAlert.gif" border="0" align="absmiddle" /> <b>NOTE:</b> This configuration is still being applied...click REFRESH to see the latest...</td>
                                                        </tr>
                                                        <tr id="panLTMStatusError" runat="server" visible="false">
                                                            <td><img src="/images/bigError.gif" border="0" align="absmiddle" /> <b>NOTE:</b> An error occurred while applying this config...please check the results below...</td>
                                                        </tr>
                                                        <tr id="panLTMStatusDone" runat="server" visible="false">
                                                            <td><img src="/images/bigCheck.gif" border="0" align="absmiddle" /> <b>NOTE:</b> This configuration has finished!  Please click the COMPLETE button at the top of this window.</td>
                                                        </tr>
                                                        <tr>
                                                            <td class="greentableheader"><asp:Button ID="btnLTM" runat="server" CssClass="default" Width="75" Text="Refresh" OnClick="btnLTM_Click" /></td>
                                                        </tr>
                                                        <tr id="panLTMStatusYes" runat="server" visible="false">
                                                            <td>
                                                                VIP DNS Registration = <asp:Label ID="lblLTM" runat="server" CssClass="default" />
                                                                <br />
                                                                <div style="height: 300px; overflow: auto; border:solid 1px #CCCCCC">
                                                                    <table width="100%" cellpadding="3" cellspacing="0" border="0">
                                                                        <tr bgcolor="#EEEEEE">
                                                                            <td nowrap>&nbsp;</td>
                                                                            <td nowrap><b>Server</b></td>
                                                                            <td nowrap><b>Result</b></td>
                                                                            <td nowrap><b>Date</b></td>
                                                                        </tr>
                                                                        <asp:repeater ID="rptLTM" runat="server">
                                                                            <ItemTemplate>
                                                                                <tr class="default">
                                                                                    <td valign="top" nowrap><img src='/images/<%# DataBinder.Eval(Container.DataItem, "error").ToString() == "1" ? "cancel" : "check" %>.gif' border="0"</td>
                                                                                    <td valign="top" nowrap><%# DataBinder.Eval(Container.DataItem, "servername") %></td>
                                                                                    <td valign="top" width="100%"><%# DataBinder.Eval(Container.DataItem, "result") %></td>
                                                                                    <td valign="top" nowrap><%# DataBinder.Eval(Container.DataItem, "created") %></td>
                                                                                </tr>
                                                                            </ItemTemplate>
                                                                            <AlternatingItemTemplate>
                                                                                <tr class="default" bgcolor="#F6F6F6">
                                                                                    <td valign="top" nowrap><img src='/images/<%# DataBinder.Eval(Container.DataItem, "error").ToString() == "1" ? "cancel" : "check" %>.gif' border="0"</td>
                                                                                    <td valign="top" nowrap><%# DataBinder.Eval(Container.DataItem, "servername") %></td>
                                                                                    <td valign="top" width="100%"><%# DataBinder.Eval(Container.DataItem, "result") %></td>
                                                                                    <td valign="top" nowrap><%# DataBinder.Eval(Container.DataItem, "created") %></td>
                                                                                </tr>
                                                                            </AlternatingItemTemplate>
                                                                        </asp:repeater>
                                                                    </table>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                                <table width="100%" cellpadding="4" cellspacing="3" border="0">
                                                    <asp:Panel ID="panSlider" runat="server" Visible="false">
                                                    <tr>
                                                        <td nowrap>Completed (%):</td>
                                                        <td width="100%"><SliderCC:Slider ID="sldHours" _ParentElement="divExecution" _Hidden="hdnHours" runat="server" _DivClass="default" _PercentClass="result" _Width="300" _ShowDiv="true" _RestrictLess="true" /> <asp:Label ID="lblHours" runat="server" CssClass="required" Visible="false" Text="No hours have been allocated for this initiative" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">&nbsp;</td>
                                                    </tr>
                                                    </asp:Panel>
                                                    <asp:Panel ID="panNoSlider" runat="server" Visible="false">
                                                    <tr>
                                                        <td colspan="2">
                                                            <table width="100%" cellpadding="0" cellspacing="5" border="0">
                                                                <tr>
                                                                    <td rowspan="2"><img src="/images/ico_check40.gif" border="0" align="absmiddle" /></td>
                                                                    <td class="header" width="100%" valign="bottom">Fast-Completion Enabled</td>
                                                                </tr>
                                                                <tr>
                                                                    <td width="100%" valign="top">With fast-completion, all you need to do is click the &quot;Complete&quot; button to close this request.</td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">&nbsp;</td>
                                                    </tr>
                                                    </asp:Panel>
                                                    <asp:Panel ID="panCheckboxes" runat="server" Visible="false">
                                                    <tr>
                                                        <td colspan="2" class="greentableheader">Required Tasks</td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <asp:Panel ID="panMIS" runat="server" Visible="false">
                                                                <table width="100%" cellpadding="4" cellspacing="3" border="0">
                                                                    <tr>
                                                                        <td>Would you like to accept or reject the design in its currently provisioned state?</td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td><asp:RadioButton ID="radAccept" runat="server" Text="Accept" GroupName="Accept" /></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td><asp:RadioButton ID="radReject" runat="server" Text="Reject - Requirements have changed" GroupName="Accept" /></td>
                                                                    </tr>
		                                                    <tr>
                		                                        <td>&nbsp;</td>
                                		                    </tr>
                                                                </table>
                                                            </asp:Panel>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2"><%=strCheckboxes %></td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">&nbsp;</td>
                                                    </tr>
                                                    </asp:Panel>
                                                    <tr>
                                                        <td colspan="2" class="greentableheader">Status Updates</td>
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
                                                                    <td nowrap>&nbsp;</td>
                                                                    <td nowrap><b>Comments</b></td>
                                                                </tr>
                                                                <asp:repeater ID="rptStatus" runat="server">
                                                                    <ItemTemplate>
                                                                        <tr class="default">
                                                                            <td nowrap><a href="javascript:void(0);" onclick="ShowStatus('<%# DataBinder.Eval(Container.DataItem, "id") %>','r');"><%# DateTime.Parse(DataBinder.Eval(Container.DataItem, "modified").ToString()).ToShortDateString() %>&nbsp;<%# DateTime.Parse(DataBinder.Eval(Container.DataItem, "modified").ToString()).ToShortTimeString() %></a></td>
                                                                            <td nowrap>&nbsp;</td>
                                                                            <td nowrap valign="top"><asp:Label ID="lblStatus" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "status") %>' /></td>
                                                                            <td nowrap>&nbsp;</td>
                                                                            <td width="100%" valign="top">
                                                                                <div id="div200_<%# DataBinder.Eval(Container.DataItem, "id") %>" style="display:inline">
                                                                                    <%# (DataBinder.Eval(Container.DataItem, "comments").ToString().Length > 200 ? DataBinder.Eval(Container.DataItem, "comments").ToString().Substring(0, 200) + " ...&nbsp;&nbsp;&nbsp;[<a href=\"javascript:void(0);\" onclick=\"ShowHideDiv('div200_" + DataBinder.Eval(Container.DataItem, "id") + "', 'none');ShowHideDiv('divMore_" + DataBinder.Eval(Container.DataItem, "id") + "', 'inline');\">More</a>]" : DataBinder.Eval(Container.DataItem, "comments").ToString())%>
                                                                                </div>
                                                                                <div id="divMore_<%# DataBinder.Eval(Container.DataItem, "id") %>" style="display:none">
                                                                                    <%# DataBinder.Eval(Container.DataItem, "comments").ToString() + "&nbsp;&nbsp;&nbsp;[<a href=\"javascript:void(0);\" onclick=\"ShowHideDiv('div200_" + DataBinder.Eval(Container.DataItem, "id") + "', 'inline');ShowHideDiv('divMore_" + DataBinder.Eval(Container.DataItem, "id") + "', 'none');\">Hide</a>]"%>
                                                                                </div>
                                                                            </td>
                                                                        </tr>
                                                                    </ItemTemplate>
                                                                </asp:repeater>
                                                            <tr>
                                                                <td colspan="5">
                                                                    <asp:Label ID="lblNoStatus" runat="server" CssClass="default" Visible="false" Text="<img src='/images/alert.gif' border='0' align='absmiddle'> There are no updates" />
                                                                </td>
                                                            </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
		                                    </div>
		                                    <div id="divChange" style='<%=boolChange == true ? "display:inline" : "display:none" %>'>
		                                        <br />
                                                <table width="100%" cellpadding="4" cellspacing="3" border="0">
                                                    <tr>
                                                        <td colspan="2" class="greentableheader">Change Controls</td>
                                                    </tr>
                                                    <tr>
                                                        <td nowrap>Number:</td>
                                                        <td width="100%"><asp:TextBox ID="txtNumber" runat="server" CssClass="default" Width="150" MaxLength="15" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td nowrap>Date:</td>
                                                        <td width="100%"><asp:TextBox ID="txtDate" runat="server" CssClass="default" Width="100" MaxLength="10" /> <asp:ImageButton ID="imgDate" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calendar.gif" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td nowrap>Time:</td>
                                                        <td width="100%"><asp:TextBox ID="txtTime" runat="server" CssClass="default" Width="100" MaxLength="10" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2"><b>Comments:</b></td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2"><asp:TextBox ID="txtChange" runat="server" CssClass="default" Width="500" TextMode="MultiLine" Rows="7" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2"><asp:Button ID="btnChange" runat="server" CssClass="default" Width="150" Text="Submit Change" OnClick="btnChange_Click" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">&nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <table width="100%" cellpadding="3" cellspacing="0" border="0" style="border:solid 1px #CCCCCC">
                                                                <tr bgcolor="#EEEEEE">
                                                                    <td nowrap><b>Number</b></td>
                                                                    <td nowrap><b>Date</b></td>
                                                                    <td nowrap>&nbsp;</td>
                                                                </tr>
                                                                <asp:repeater ID="rptChange" runat="server">
                                                                    <ItemTemplate>
                                                                        <tr class="default">
                                                                            <td nowrap><a href="javascript:void(0);" onclick="ShowChange('<%# DataBinder.Eval(Container.DataItem, "changeid") %>');"><%# DataBinder.Eval(Container.DataItem, "number") %></a></td>
                                                                            <td nowrap><%# DateTime.Parse(DataBinder.Eval(Container.DataItem, "implementation").ToString()).ToLongDateString() %> @ <%# DateTime.Parse(DataBinder.Eval(Container.DataItem, "implementation").ToString()).ToLongTimeString() %></td>
                                                                            <td align="right">[<asp:LinkButton ID="btnDeleteChange" runat="server" Text="Delete" OnClick="btnDeleteChange_Click" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "changeid") %>' />]</td>
                                                                        </tr>
                                                                    </ItemTemplate>
                                                                </asp:repeater>
                                                            <tr>
                                                                <td colspan="3">
                                                                    <asp:Label ID="lblNoChange" runat="server" CssClass="default" Visible="false" Text="<img src='/images/alert.gif' border='0' align='absmiddle'> There are no change controls" />
                                                                </td>
                                                            </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
		                                    </div>
		                                    <div id="divDocuments" style='<%=boolDocuments == true ? "display:inline" : "display:none" %>'>
		                                        <br />
                                                <table width="100%" cellpadding="4" cellspacing="3" border="0">
                                                    <tr>
                                                        <td class="greentableheader">Attached Files</td>
                                                        <td align="right"><asp:CheckBox ID="chkDescription" runat="server" CssClass="default" Text="Show Descriptions" AutoPostBack="true" OnCheckedChanged="chkDescription_Change" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2"><asp:Label ID="lblDocuments" runat="server" CssClass="default" /></td>
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
<asp:HiddenField ID="hdnTab" runat="server" />