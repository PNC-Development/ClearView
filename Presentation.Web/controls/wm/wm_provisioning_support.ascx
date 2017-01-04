<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wm_provisioning_support.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.wm_provisioning_support" %>


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
    
    function setControlsForReturnedRequest()
    {
       
        var obtnSave = document.getElementById('<%=btnSave.ClientID %>');
        if (obtnSave != null)
            obtnSave.disabled=true;
        var obtnPrint = document.getElementById('<%=btnPrint.ClientID %>');
        if (obtnPrint != null)
            obtnPrint.disabled=true;
        var obtnEmail = document.getElementById('<%=btnEmail.ClientID %>');
        if (obtnEmail != null)
            obtnEmail.disabled=true;
        var obtnComplete = document.getElementById('<%= btnComplete.ClientID %>');
        if (obtnComplete != null)
            obtnComplete.disabled=true;
        var obtnReturn = document.getElementById('<%=btnReturn.ClientID %>');
        if (obtnReturn != null)
            obtnReturn.disabled=true;
            
        RefreshOpeningWindow();
        window.close();
    }
    
</script>
<asp:Panel ID="panWorkload" runat="server" Visible="false">
    <table width="100%" border="0" cellSpacing="0" cellPadding="0" class="default">
        <tr>
            <td>
                <table width="100%" border="0" cellSpacing="0" cellPadding="0">
                    <tr>
                        <td><asp:ImageButton ID="btnSave" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/tool_save.gif" OnClick="btnSave_Click" /></td>
                        <td><asp:ImageButton ID="btnPrint" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/tool_print.gif" /></td>
                        <td><asp:ImageButton ID="btnEmail" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/tool_email.gif" /></td>
                        <td><asp:ImageButton ID="btnSLA" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/tool_sla.gif" /></td>
                        <td><asp:ImageButton ID="btnComplete" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/tool_complete.gif" OnClick="btnComplete_Click" /></td>
                        <td><asp:ImageButton ID="btnReturn" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/tool_return.gif" OnClick="btnReturn_Click" /></td>
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
                </table>
            </td>
        </tr>
        <tr>
            <td class="default">
                <asp:Panel ID="pnlReqReturn" runat="server" CssClass="default" Visible="false">
                    <table width="100%" cellSpacing="2" cellPadding="2" class="default" style="border:solid 1px #999999" bgcolor="#f9f9f9">
                        <tr>
                            <td colspan="2" style="width:20%"><asp:Label ID="lblReqReturn" runat="server" CssClass="redheader" Text="You service request is returned for below mentioned reason.  Please correct the request and submit again " /></td>
                           
                        </tr>
                        <tr>
                            <td style="width:20%"><asp:Label ID="lblReqReturnedBy" runat="server" CssClass="bold" Text="Request Returned By : " /></td>
                            <td style="width:78%"><asp:Label ID="lblReqReturnedByValue" runat="server" CssClass="bold" Text="User" />
                            <asp:Label ID="lblReqReturnedId" runat="server" CssClass="bold" Text="0" Visible="false" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width:20%"><asp:Label ID="lblReqReturnComment" runat="server" CssClass="bold" Text="Comments : " /></td>
                            <td style="width:78%"><asp:Label ID="lblReqReturnCommentValue" runat="server" CssClass="bold" Text="" /></td>
                        </tr>
                    </table>
                </asp:Panel>
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
                                                <p>&nbsp;</p>
		                                    </div>
		                                    <div id="divExecution" style='<%=boolExecution == true ? "display:inline" : "display:none" %>'>
		                                        <br />
		                                          <!-- For Execution -->
                                                    <asp:Panel ID="panError" runat="server" Visible="false">
                                                        <%=strError %>
                                                        <p></p>
                                                        <asp:Panel ID="panIncident" runat="server" Visible="false">
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        Incident:
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtIncident" runat="server" Width="100" MaxLength="20" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        Assigned:
                                                                    </td>
                                                                    <td>
                                                                        <table cellpadding="0" cellspacing="0" border="0">
                                                                            <tr>
                                                                                <td><asp:TextBox ID="txtIncidentUser" runat="server" Width="300" CssClass="default" /></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <div id="divIncidentUser" runat="server" style="overflow:hidden; position:absolute; display:none; background-color:#FFFFFF; border:solid 1px #CCCCCC">
                                                                                        <asp:ListBox ID="lstIncidentUser" runat="server" CssClass="default" />
                                                                                    </div>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                        <asp:HiddenField ID="hdnIncidentUser" runat="server" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td></td>
                                                                    <td>
                                                                        <asp:Button ID="btnIncident" runat="server" CssClass="default" Width="125" Text="Save Incident" OnClick="btnIncident_Click" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </asp:Panel>
                                                        <asp:Panel ID="pnlSolutions" runat="server">
                                                            <table width="100%" cellpadding="4" cellspacing="3" border="0">
                                                                <tr id="trSolution" runat="server" style="display:none">
                                                                <td>
                                                                    <table width="100%" cellpadding="4" cellspacing="3" border="0">
                                                                        <tr>
                                                                            <td valign="top"></td>
                                                                            <td valign="top"><asp:Label ID="lblError" runat="server" CssClass="default" Visible="false" /></td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td valign="top">Problem:</td>
                                                                            <td valign="top"><asp:TextBox ID="txtProblem" runat="server" CssClass="default" Width="500" TextMode="MultiLine" Rows="5" /></td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td valign="top">Resolution:</td>
                                                                            <td valign="top"><asp:TextBox ID="txtResolution" runat="server" CssClass="default" Width="500" TextMode="MultiLine" Rows="5" /></td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td valign="top">Cause Code Category:</td>
                                                                            <td valign="top"><asp:DropDownList ID="ddlCauseCode" runat="server" CssClass="default" Width="200" /></td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td valign="top">Cause Code Sub-Category:</td>
                                                                            <td valign="top">
                                                                                <asp:DropDownList ID="ddlCauseType" runat="server" CssClass="default" Width="200" Enabled="false" >
                                                                                    <asp:ListItem Value="-- Please select a cause type --" />
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td valign="top">Attachment:</td>
                                                                            <td valign="top"><asp:FileUpload ID="txtFile" runat="server" CssClass="default" Width="500" Height="18" /></td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td valign="top"></td>
                                                                            <td><asp:Button ID="btnApplyNewSolution" runat="server" CssClass="default" Width="125" Text="Create Solution" OnClick="btnApplyNewSolution_Click" /></td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                            
                                                            <br />
                                                            <table width="100%" cellpadding="0" cellspacing="5" border="0">
                                                                <tr>
                                                                    <td rowspan="2"><img src="/images/ico_check.gif" border="0" align="absmiddle" /></td>
                                                                    <td class="header" width="100%" valign="bottom">Possible Solutions</td>
                                                                    <td align="right">
                                                                        <asp:Button ID="btnNoSolution" runat="server" CssClass="default" Width="100" Text="No Solution" OnClick="btnNoSolution_Click" Visible="false" />&nbsp;
                                                                        <asp:Button ID="btnNewSolution" runat="server" CssClass="default" Width="100" Text="New Solution"  />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td width="100%" colspan="3" valign="top">Select the solution which fixes this error. If the solution is not provided, please click the <b>New Solution</b> button.</td>
                                                                </tr>
                                                            </table>
                                                            <table width="100%" cellpadding="4" cellspacing="3" border="0">
                                                                <tr>
                                                                    <td>
                                                                        <%=strMenuTab1 %>
                                                                        <div id="divMenu1">
                                                                            <br />
                                                                            <asp:Repeater ID="rptRelated" runat="server">
                                                                                <ItemTemplate>
                                                                                    <div style="display:none">
                                                                                        <table width="100%" cellpadding="2" cellspacing="0" border="0" class="default">
                                                                                            <tr>
                                                                                                <td colspan="2">
                                                                                                    <table width="100%" cellpadding="4" cellspacing="3" border="0" style="border:solid 1px #CCCCCC">
                                                                                                        <tr bgcolor="#EEEEEE">
                                                                                                            <td class="bold">ISSUE - What caused the problem</td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td><%# oFunction.FormatText(DataBinder.Eval(Container.DataItem, "problem").ToString())%></td>
                                                                                                        </tr>
                                                                                                    </table>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td colspan="2">
                                                                                                    <table width="100%" cellpadding="4" cellspacing="3" border="0" style="border:solid 1px #CCCCCC">
                                                                                                        <tr bgcolor="#EEEEEE">
                                                                                                            <td class="bold">RESOLUTION - Steps taken to fix the issue</td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td><%# oFunction.FormatText(DataBinder.Eval(Container.DataItem, "resolution").ToString())%></td>
                                                                                                        </tr>
                                                                                                    </table>
                                                                                                    <asp:Panel ID="panAttach" runat="server" Visible="false">
                                                                                                        <table width="100%" cellpadding="3" cellspacing="2" border="0">
                                                                                                            <tr>
                                                                                                                <td><span style="width:100%;border-bottom:1 dotted #CCCCCC;"/></td>
                                                                                                            </tr>
                                                                                                            <tr>
                                                                                                                <td><img src="/images/file.gif" border="0" align="absmiddle"/> <asp:Label ID="lblAttach" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem,"path") %>' /></td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                    </asp:Panel>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td colspan="2" align="right">Created by <b><%# oUser.GetFullName(Int32.Parse(DataBinder.Eval(Container.DataItem, "userid").ToString()))%></b> on <%# DataBinder.Eval(Container.DataItem,"modified") %></td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td><asp:Button ID="btnSelectExistingSolution" runat="server" CssClass="default" Width="175" Text="This Solution Fixed the Error" OnClick="btnSelectExistingSolution_Click" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"id") %>' /></td>
                                                                                                <td align="right"><b>Category : </b><%# DataBinder.Eval(Container.DataItem, "code")%> / <%# DataBinder.Eval(Container.DataItem, "type")%></td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </div>
                                                                                </ItemTemplate>
                                                                            </asp:Repeater>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                                <tr id="trNone" runat="server" visible="false">
                                                                    <td><img src='/images/alert.gif' border='0' align='absmiddle'> There are no related errors ... </td>
                                                                </tr>
                                                            </table>
                                                        </asp:Panel>
                                                    </asp:Panel>
		                                        <!-- End of Execution -->
		                                        <br />
                                                <table width="100%" cellpadding="4" cellspacing="3" border="0">
                                                    
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
                                                                            <td nowrap valign="top"><a href="javascript:void(0);" onclick="ShowStatus('<%# DataBinder.Eval(Container.DataItem, "id") %>','r');"><%# DateTime.Parse(DataBinder.Eval(Container.DataItem, "modified").ToString()).ToShortDateString() %>&nbsp;<%# DateTime.Parse(DataBinder.Eval(Container.DataItem, "modified").ToString()).ToShortTimeString() %></a></td>
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

<asp:Label ID="lblServerId" runat="server" Visible="false" />
<asp:Label ID="lblWorkstationId" runat="server" Visible="false" />
<asp:Label ID="lblAsset" runat="server" Visible="false" />
<asp:Label ID="lblStep" runat="server" Visible="false" />
<asp:Label ID="lblErrorId" runat="server" Visible="false" />
<input type="hidden" id="hdnCauseType" runat="server" />