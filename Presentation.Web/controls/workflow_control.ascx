<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="workflow_control.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.workflow_control" %>


<script type="text/javascript">
    function ShowDetail(oDiv) {
        oDiv = document.getElementById(oDiv);
        if (oDiv.style.display == "inline")
            oDiv.style.display = "none";
        else
            oDiv.style.display = "inline";
    }
    function ChangeFrame(oCell, oShow, oHide1, oHide2, oHide3, oHide4) {
        oShow = document.getElementById(oShow);
        oShow.style.display = "inline";
        oHide1 = document.getElementById(oHide1);
        oHide1.style.display = "none";
        oHide2 = document.getElementById(oHide2);
        oHide2.style.display = "none";
        oHide3 = document.getElementById(oHide3);
        oHide3.style.display = "none";
        oHide4 = document.getElementById(oHide4);
        oHide4.style.display = "none";
    	var oRow = oCell.parentElement;
	    for (var yy=0; yy<oRow.children.length; yy++) {
    		var oNot = oRow.getElementsByTagName("td").item(yy);
    		if (oNot.className == "cmbutton")
                oNot.style.border = "1px solid #94a6b5"
    	}
	    oCell.style.borderTop = "3px solid orange"
        oCell.style.borderBottom = "1px solid #FFFFFF"
    }
</script>
<table width="100%" cellpadding="0" cellspacing="0" border="0" class="default">
    <tr>
        <td><img src="/images/table_topleft.gif" border="0" width="5" height="26"></td>
        <td nowrap background="/images/table_top.gif" class="greentableheader" width="100%">Workflow Request # <asp:Label ID="lblRequest" runat="server" CssClass="greentableheader" /></td>
        <td><img src="/images/table_topright.gif" border="0" width="5" height="26"></td>
    </tr>
    <tr>
        <td background="/images/table_left.gif"><img src="/images/table_left.gif" width="5" height="10"></td>
        <td width="100%" bgcolor="#FFFFFF">
            <br />
            <asp:Panel ID="panWorkflow" runat="server" Visible="false">
                <table width="100%" height="100%" border="0" cellSpacing="0" cellPadding="0" class="default">
                    <tr height="1">
                        <td colspan="10" align="center" class="bigalert">
                            <asp:Label ID="lblWaiting" runat="server" Visible="false" Text="<img src='/images/bigAlert.gif' border='0' align='absmiddle' /> You cannot update this request at this time." />
                        </td>
                    </tr>
	                <tr height="1">
		                <td class="cmbutton" style='<%=boolDetails == true ? "display:inline;border-bottom:1px solid #FFFFFF;border-top:3px solid orange;" : "" %>' onclick="ChangeFrame(this,'divDetails','divPriority','divDocuments','divDiscussion','divHistory');">Details</td>
		                <td class="cmbuttonspace">&nbsp;</td>
		                <td class="cmbutton" onclick="ChangeFrame(this,'divPriority','divDetails','divDocuments','divDiscussion','divHistory');">Priority</td>
		                <td class="cmbuttonspace">&nbsp;</td>
		                <td class="cmbutton" style='<%=boolDocuments == true ? "display:inline;border-bottom:1px solid #FFFFFF;border-top:3px solid orange;" : "" %>' onclick="ChangeFrame(this,'divDocuments','divDetails','divPriority','divDiscussion','divHistory');">Documents</td>
		                <td class="cmbuttonspace">&nbsp;</td>
		                <td class="cmbutton" style='<%=boolDiscussion == true ? "display:inline;border-bottom:1px solid #FFFFFF;border-top:3px solid orange;" : "" %>' onclick="ChangeFrame(this,'divDiscussion','divDetails','divPriority','divDocuments','divHistory');">Discussion</td>
		                <td class="cmbuttonspace">&nbsp;</td>
		                <td class="cmbutton" onclick="ChangeFrame(this,'divHistory','divDetails','divPriority','divDocuments','divDiscussion');">History</td>
		                <td style="border-bottom:1px solid #94a6b5;">&nbsp;</td>
	                </tr>
	                <tr>
		                <td colspan="10" align="center" class="default">
                            <table width="100%" height="100%" border="0" cellSpacing="2" cellPadding="2" class="default">
                                <tr> 
                                    <td valign="top">
            		                    <div id="divDetails" style='<%=boolDetails == true ? "display:inline" : "display:none" %>'>
            		                        <br />
                                                <table width="100%" cellpadding="4" cellspacing="3" border="0">
                                                    <tr>
                                                        <td colspan="2" align="center" class="bigalert">
                                                            <asp:Label ID="lblUpdated" runat="server" Visible="false" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>Submitter Name:</td>
                                                        <td><asp:Label ID="lblName2" runat="server" CssClass="default" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td>Request Date:</td>
                                                        <td><asp:Label ID="lblDate2" runat="server" CssClass="default" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td>Project / Task Name:</td>
                                                        <td><asp:Label ID="lblProjectTask" runat="server" CssClass="default" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td>Initiative Type:</td>
                                                        <td><asp:Label ID="lblBaseDisc" runat="server" CssClass="default" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td>Organization:</td>
                                                        <td><asp:Label ID="lblOrganization" runat="server" CssClass="default" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td>Clarity / Project Number:</td>
                                                        <td><asp:Label ID="lblClarity2" runat="server" CssClass="default" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2"><hr size="1" noshade /></td>
                                                    </tr>
                                                    <tr>
                                                        <td>Executive Sponsor:</td>
                                                        <td><asp:Label ID="lblExecutive2" runat="server" CssClass="default" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td>Working Sponsor:</td>
                                                        <td><asp:Label ID="lblWorking2" runat="server" CssClass="default" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td valign="top">Require a C1:</td>
                                                        <td><asp:Label ID="lblC1" runat="server" CssClass="default" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td>End Life:</td>
                                                        <td><asp:Label ID="lblEndLife" runat="server" CssClass="default" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td><img src="/images/spacer.gif" border="0" width="20" height="1" /><img src="/images/downright.gif" border="0" align="absmiddle" /> End Life Date:</td>
                                                        <td><asp:Label ID="lblEndLifeDate" runat="server" CssClass="default" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td valign="top">Initiative Opportunity:</td>
                                                        <td><asp:Label ID="lblInitiative" runat="server" CssClass="default" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td valign="top">Platform(s):</td>
                                                        <td><asp:Label ID="lblPlatforms" runat="server" CssClass="default" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td>Is this an Audit Requirement:</td>
                                                        <td><asp:Label ID="lblRequirement" runat="server" CssClass="default" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td><img src="/images/spacer.gif" border="0" width="20" height="1" /><img src="/images/downright.gif" border="0" align="absmiddle" /> Requirement Date:</td>
                                                        <td><asp:Label ID="lblRequirementDate" runat="server" CssClass="default" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td>Interdependency With Other Projects/Initiatives:</td>
                                                        <td><asp:Label ID="lblInterdependency" runat="server" CssClass="default" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td><img src="/images/spacer.gif" border="0" width="20" height="1" /><img src="/images/downright.gif" border="0" align="absmiddle" /> Project Name(s):</td>
                                                        <td><asp:Label ID="lblProjects" runat="server" CssClass="default" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td valign="top">Technology Capability:</td>
                                                        <td><asp:Label ID="lblCapability" runat="server" CssClass="default" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td>Estimated Man Hours:</td>
                                                        <td><asp:Label ID="lblHours" runat="server" CssClass="default" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td>Proposed Start Date:</td>
                                                        <td><asp:Label ID="lblStart" runat="server" CssClass="default" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td>Expected Project Completion Date:</td>
                                                        <td><asp:Label ID="lblCompletion" runat="server" CssClass="default" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td>Expected Capital Cost:</td>
                                                        <td><asp:Label ID="lblCapital" runat="server" CssClass="default" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td>Internal Labor:</td>
                                                        <td><asp:Label ID="lblInternal" runat="server" CssClass="default" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td>External Labor:</td>
                                                        <td><asp:Label ID="lblExternal" runat="server" CssClass="default" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td>Maintenance Cost Increase:</td>
                                                        <td><asp:Label ID="lblMaintenance" runat="server" CssClass="default" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td>Project Expenses:</td>
                                                        <td><asp:Label ID="lblExpenses" runat="server" CssClass="default" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td>Estimated Net Cost Avoidance:</td>
                                                        <td><asp:Label ID="lblCostAvoidance" runat="server" CssClass="default" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td>Estimated Net Cost Savings:</td>
                                                        <td><asp:Label ID="lblSavings" runat="server" CssClass="default" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td>Realized Cost Savings:</td>
                                                        <td><asp:Label ID="lblRealized" runat="server" CssClass="default" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td>Business Impact Aviodance:</td>
                                                        <td><asp:Label ID="lblBusinessAvoidance" runat="server" CssClass="default" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td>Maintenance Cost Avoidance:</td>
                                                        <td><asp:Label ID="lblMaintenanceAvoidance" runat="server" CssClass="default" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td>Asset Reusability:</td>
                                                        <td><asp:Label ID="lblReusability" runat="server" CssClass="default" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td>Internal Customer Impact:</td>
                                                        <td><asp:Label ID="lblInternalImpact" runat="server" CssClass="default" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td>External Customer Impact:</td>
                                                        <td><asp:Label ID="lblExternalImpact" runat="server" CssClass="default" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td>Impact:</td>
                                                        <td><asp:Label ID="lblImpact" runat="server" CssClass="default" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td>Strategic Opportunity:</td>
                                                        <td><asp:Label ID="lblStrategic" runat="server" CssClass="default" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td>Acquisition / BIC:</td>
                                                        <td><asp:Label ID="lblAcquisition" runat="server" CssClass="default" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td>Technology Capabilities:</td>
                                                        <td><asp:Label ID="lblCapabilities" runat="server" CssClass="default" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td>Technical Project Manager Requested:</td>
                                                        <td><asp:Label ID="lblTPM2" runat="server" CssClass="default" /></td>
                                                    </tr>
                                                    <asp:Panel ID="panTPM" runat="server" Visible="false">
                                                    <tr>
                                                        <td nowrap><img src="/images/spacer.gif" border="0" width="20" height="1" /><img src="/images/downright.gif" border="0" align="absmiddle" /> Service Type:</td>
                                                        <td><asp:Label ID="lblTPMService" runat="server" CssClass="default" /></td>
                                                    </tr>
                                                    </asp:Panel>
                                                    <asp:Panel ID="panPM" runat="server" Visible="false">
                                                    <tr>
                                                        <td nowrap><img src="/images/spacer.gif" border="0" width="20" height="1" /><img src="/images/downright.gif" border="0" align="absmiddle" /> Project Lead:</td>
                                                        <td><asp:Label ID="lblPM" runat="server" CssClass="default" /></td>
                                                    </tr>
                                                    </asp:Panel>
                                                </table>
            		                    </div>
            		                    <div id="divPriority" style="display:none">
            		                        <br />
            		                        <%=strPriority%>
            		                    </div>
            		                    <div id="divDocuments" style='<%=boolDocuments == true ? "display:inline" : "display:none" %>'>
            		                        <br />
                                            <table width="100%" cellpadding="2" cellspacing="1" border="0">
                                                <tr>
                                                    <td align="right"><asp:CheckBox ID="chkDescription" runat="server" CssClass="default" Text="Show Descriptions" AutoPostBack="true" OnCheckedChanged="chkDescription_Change" /></td>
                                                </tr>
                                                <tr>
                                                    <td><asp:Label ID="lblDocuments" runat="server" CssClass="default" /></td>
                                                </tr>
                                            </table>
            		                    </div>
            		                    <div id="divDiscussion" style='<%=boolDiscussion == true ? "display:inline" : "display:none" %>'>
            		                        <br />
            		                        <asp:Panel ID="panDiscussionX" runat="server" Visible="false">
                                                <table width="100%" cellpadding="2" cellspacing="2" border="0">
                                                    <tr>
                                                        <td><img src="/images/spacer.gif" border="0" width="15" height="1" /></td>
                                                        <td class="header"><img src="/images/bigX.gif" border="0" align="absmiddle" /> Access Denied</td>
                                                    </tr>
                                                    <tr><td colspan="2">&nbsp;</td></tr>
                                                    <tr>
                                                        <td><img src="/images/spacer.gif" border="0" width="15" height="1" /></td>
                                                        <td>You do not have rights to view the disucssion.</td>
                                                    </tr>
                                                    <tr><td colspan="2">&nbsp;</td></tr>
                                                </table>
            		                        </asp:Panel>
            		                        <asp:Panel ID="panDiscussion" runat="server" Visible="false">
                                            <table width="100%" cellpadding="3" cellspacing="2" border="0">
                                                <tr>
                                                    <td colspan="2">
                                                        <table width="100%" cellpadding="3" cellspacing="0" border="0">
                                                        <asp:repeater ID="rptComments" runat="server">
                                                            <ItemTemplate>
                                                                <tr>
                                                                    <td valign="top"><img src="/images/comment.gif" border="0" /></td>
                                                                    <td valign="top" colspan="2" width="100%"><%# DataBinder.Eval(Container.DataItem, "comment") %></td>
                                                                </tr>
                                                                <tr>
                                                                    <td>&nbsp;</td>
                                                                    <td class="comment">&nbsp;-&nbsp;<asp:Label ID="lblXID" runat="server" CssClass="comment" Text='<%# oUser.GetFullName(DataBinder.Eval(Container.DataItem, "xid").ToString()) %>' />&nbsp;:&nbsp;&nbsp;<%# DateTime.Parse(DataBinder.Eval(Container.DataItem, "modified").ToString()).ToLongDateString() %> @ <%# DateTime.Parse(DataBinder.Eval(Container.DataItem, "modified").ToString()).ToShortTimeString() %></td>
                                                                    <td class="comment" align="right"><asp:LinkButton ID="btnDelete" runat="server" OnClick="btnDelete_Click" Visible='<%# (DataBinder.Eval(Container.DataItem, "userid").ToString() == intProfile.ToString()) %>' CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' Text="[Delete Comment]" /></td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="3" height="1"><img src="/images/spacer.gif" border="0" width="1" height="1" /></td>
                                                                </tr>
                                                            </ItemTemplate>
                                                        </asp:repeater>
                                                        <tr><td colspan="3" class="bigalert" align="center"><asp:Label ID="lblNoComments" runat="server" Visible="false" Text="<img src='/images/bigalert.gif' border='0' align='absmiddle'> There are no comments" /></td></tr>
                                                        </table>
                                                    </td>
                                                </tr>
                            		            <tr>
                            		                <td colspan="2"><span style="width:100%;border-bottom:1 dotted #999999;"/></td>
                            		            </tr>
                                                <tr>
                                                    <td colspan="2">&nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2" class="hugeheader">Post a Comment</td>
                                                </tr>
                                                <tr>
                                                    <td nowrap>User:</td>
                                                    <td width="100%"><asp:Label ID="lblUser" runat="server" CssClass="default" /></td>
                                                </tr>
                                                <tr>
                                                    <td nowrap valign="top">Comment:</td>
                                                    <td width="100%"><asp:TextBox ID="txtInternal" runat="server" CssClass="default" TextMode="MultiLine" Rows="5" Width="400" /></td>
                                                </tr>
                                                <tr>
                                                    <td>&nbsp;</td>
                                                    <td><asp:Button ID="btnInternal" runat="server" CssClass="default" Width="125" Text="Post Comment" OnClick="btnInternal_Click" /></td>
                                                </tr>
                                            </table>
            		                        </asp:Panel>
            		                    </div>
            		                    <div id="divHistory" style="display:none">
            		                        <br />
                                            <table width="100%" cellpadding="2" cellspacing="1" border="0">
                                                <tr>
                                                    <td>
                                                        <asp:repeater ID="rptHistory" runat="server">
                                                            <HeaderTemplate>
                                                                <table width="100%" cellpadding="3" cellspacing="0" border="0" style="border:solid 1px #CCCCCC">
                                                                    <tr bgcolor="#EEEEEE">
                                                                        <td nowrap width="1">&nbsp;</td>
                                                                        <td nowrap><b>Status</b></td>
                                                                        <td nowrap><b>Level</b></td>
                                                                        <td nowrap><b>Assigned User</b></td>
                                                                        <td nowrap><b>Updated</b></td>
                                                                    </tr>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <tr class="default">
                                                                    <td width="1"><asp:Label ID="lblImage" runat="server" CssClass="default" Text='' /></td>
                                                                    <td><asp:Label ID="lblApproval" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "approval") %>' /></td>
                                                                    <td><asp:Label ID="lblStep" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "step") %>' /></td>
                                                                    <td><asp:Label ID="lblUserId" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "userid") %>' /></td>
                                                                    <td><asp:Label ID="lblModified" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "modified") %>' /></td>
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
            		                </td>
            		            </tr>
                                <tr><td>&nbsp;</td></tr>
                                <tr>
                                    <td><hr size="1" noshade /></td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                            <tr>
                                                <td class="footer"></td>
                                                <td align="right">
                                                    <table cellpadding="2" cellspacing="1" border="0">
                                                        <tr>
                                                            <td><asp:Button ID="btnApprove" runat="server" CssClass="default" Width="75" Text="Approve" CommandArgument="1" OnClick="btnSubmit_Click" /></td>
                                                            <td><asp:Button ID="btnDeny" runat="server" CssClass="default" Width="75" Text="Deny" CommandArgument="-1" OnClick="btnSubmit_Click" /></td>
                                                            <td><asp:Button ID="btnShelf" runat="server" CssClass="default" Width="75" Text="Shelf" CommandArgument="10" OnClick="btnSubmit_Click" /></td>
                                                        </tr>
                                                    </table>
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
                                    <td align="right"><asp:Button ID="btnClose" runat="server" CssClass="default" Width="75" Text="Close" /></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="panFinish" runat="server" Visible="false">
                <table width="100%" cellpadding="2" cellspacing="2" border="0">
                    <tr>
                        <td><img src="/images/spacer.gif" border="0" width="15" height="1" /></td>
                        <td class="header"><img src="/images/bigCheck.gif" border="0" align="absmiddle" /> Record Updated</td>
                    </tr>
                    <tr><td colspan="2">&nbsp;</td></tr>
                    <tr>
                        <td><img src="/images/spacer.gif" border="0" width="15" height="1" /></td>
                        <td>Your information has been saved successfully.</td>
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
                                    <td align="right"><asp:Button ID="btnFinish" runat="server" CssClass="default" Width="75" Text="Finish" /></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <p>&nbsp;</p>
        </td>
        <td background="/images/table_right.gif"><img src="/images/table_right.gif" width="5" height="10"></td>
    </tr>
    <tr>
        <td><img src="/images/table_bottomLeft.gif" border="0" width="5" height="9"></td>
        <td width="100%" background="/images/table_bottom.gif"></td>
        <td><img src="/images/table_bottomRight.gif" border="0" width="5" height="9"></td>
    </tr>
</table>
<asp:Label ID="lblStep" runat="server" Visible="false" />