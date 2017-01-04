<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wm_enhancement.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.wm_enhancement" %>

<script type="text/javascript">
</script>
<asp:Panel ID="panWorkload" runat="server" Visible="false">
    <table width="100%" border="0" cellSpacing="0" cellPadding="0" class="default">    
        <tr>
            <td>
                <table width="100%" border="0" cellSpacing="0" cellPadding="0">
                    <tr>
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
                    <%=strMenuTab1 %>
                    <div id="divMenu1">
                        <br />
                        <div style="display:none">
                            <table width="100%" cellpadding="4" cellspacing="3" border="0">
                                <tr>
                                    <td class="greentableheader">Enhancement Details</td>
                                </tr>
                                <tr>
                                    <td><asp:Label ID="lblView" runat="server" CssClass="default" /></td>
                                </tr>
                            </table>
                        </div>
                        <div style="display:none">
                            <table width="100%" cellpadding="4" cellspacing="3" border="0">
                                <tr>
                                    <td colspan="2" class="greentableheader">Execution</td>
                                </tr>
                                <tr>
                                    <td nowrap rowspan="2" valign="top"><asp:Image ID="img1" runat="server" ImageAlign="absMiddle" ImageUrl="/images/spacer.gif" /></td>
                                    <td class="biggerbold">Step # 1 : Functional Requirement(s)</td>
                                </tr>
                                <tr id="tr1" runat="server" visible="false">
                                    <td valign="top" width="100%">
                                        <table cellpadding="3" cellspacing="0" border="0">
                                            <tr>
                                                <td>During this step, you are required to compile the request into a functional requirements document.</td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <ul>
                                                        <li><a href="javascript:void(0);"><img src="/images/file.gif" border="0" align="absmiddle" /> Download Functional Requirements Template - Short Version</a></li>
                                                        <li><a href="javascript:void(0);"><img src="/images/file.gif" border="0" align="absmiddle" /> Download Functional Requirements Template - Long Version</a></li>
                                                    </ul>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="bold">Functional Requirements Document Upload:</td>
                                            </tr>
                                            <tr>
                                                <td><asp:FileUpload id="filFunctional" runat="server" CssClass="default" Width="600" /></td>
                                            </tr>
                                            <tr>
                                                <td class="note"><b>NOTE:</b> All uploads will be saved on the &quot;Functional Requirements Documentation&quot; tab.</td>
                                            </tr>
                                            <tr>
                                                <td>&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td class="bold">Estimated Completion Date:</td>
                                            </tr>
                                            <tr>
                                                <td>If all approvals are completed within <asp:TextBox ID="txtDays" runat="server" Width="50" MaxLength="5" Text="10" /> business days, this enhancement will be available on...</td>
                                            </tr>
                                            <tr>
                                                <td><asp:RadioButtonList ID="radEstimate" runat="server" RepeatDirection=Horizontal /></td>
                                            </tr>
                                            <tr>
                                                <td><asp:CheckBox ID="chk1" runat="server" Text="Finished with Step # 1 - Move on to Step # 2" /></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr id="tr1Done" runat="server" visible="false">
                                    <td valign="top" width="100%"><asp:Label ID="lbl1" runat="server" CssClass="approved" /></td>
                                </tr>
                                <tr>
                                    <td nowrap rowspan="2" valign="top"><asp:Image ID="img2" runat="server" ImageAlign="absMiddle" ImageUrl="/images/spacer.gif" /></td>
                                    <td class="biggerbold">Step # 2 : Approvals</td>
                                </tr>
                                <tr id="tr2" runat="server" visible="false">
                                    <td valign="top" width="100%">
                                        <table cellpadding="3" cellspacing="0" border="0">
                                            <tr>
                                                <td><img src="/images/check.gif" border="0" align="absmiddle" /> The functional requirements have been approved by the client.</td>
                                            </tr>
                                            <tr>
                                                <td>Select the approvers from a list of approval groups.  Each person in the group will review the functional requirements document and approve or deny it.</td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <table cellpadding="4" cellspacing="0" border="0" style="border:solid 1px #CCCCCC">
                                                        <tr bgcolor="#EEEEEE">
                                                            <td><b><u>Approval Group:</u></b></td>
                                                            <td><b><u>Approver:</u></b></td>
                                                            <td><b><u>Notified:</u></b></td>
                                                            <td><b><u>Status:</u></b></td>
                                                            <td><b><u>Completed:</u></b></td>
                                                            <td><b><u>Comments:</u></b></td>
                                                        </tr>
                                                        <asp:repeater ID="rptApprovers" runat="server">
                                                            <ItemTemplate>
                                                                <tr>
                                                                    <td valign="top"><%# DataBinder.Eval(Container.DataItem, "name").ToString() + (DataBinder.Eval(Container.DataItem, "any").ToString() == "1" ? " (Any)" : " (All)") %></td>
                                                                    <td valign="top"><%# DataBinder.Eval(Container.DataItem, "username") %></td>
                                                                    <td valign="top"><%# DataBinder.Eval(Container.DataItem, "notified") %></td>
                                                                    <td valign="top"><%# DataBinder.Eval(Container.DataItem, "approved").ToString() == "1" ? "Approved" : DataBinder.Eval(Container.DataItem, "approved").ToString() == "0" ? "Denied" : "Pending"%></td>
                                                                    <td valign="top"><%# DataBinder.Eval(Container.DataItem, "created") %></td>
                                                                    <td valign="top"><%# DataBinder.Eval(Container.DataItem, "comments") %></td>
                                                                </tr>
                                                            </ItemTemplate>
                                                        </asp:repeater>
                                                        <tr>
                                                            <td colspan="5">
                                                                <asp:Label ID="lblApprovers" runat="server" CssClass="default" Visible="false" Text="<img src='/images/spacer.gif' border='0' height='1' width='25' /><img src='/images/alert.gif' border='0' align='absmiddle'> There are no approval groups" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td><b>NOTE:</b> You can add more approvers at any time, up to the point where all have approved the requirements.</td>
                                            </tr>
                                            <tr>
                                                <td><asp:Button ID="btnApprovalGroup" runat="server" Text="Edit Approval Groups" Width="150" /></td>
                                            </tr>
                                            <tr>
                                                <td><b>NOTE:</b> It is up to the client to follow up with the approvers and make sure every one approves the requirements.</td>
                                            </tr>
                                            <tr>
                                                <td><asp:CheckBox ID="chk2" runat="server" Text="Skip Approvers - Move on to Step # 3" /></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr id="tr2Wait" runat="server" visible="false">
                                    <td valign="top" width="100%"><i>Waiting for Previous Step to Complete</i></td>
                                </tr>
                                <tr id="tr2Done" runat="server" visible="false">
                                    <td valign="top" width="100%"><asp:Label ID="lbl2" runat="server" CssClass="approved" /></td>
                                </tr>
                                <tr>
                                    <td nowrap rowspan="2" valign="top"><asp:Image ID="img3" runat="server" ImageAlign="absMiddle" ImageUrl="/images/spacer.gif" /></td>
                                    <td class="biggerbold">Step # 3 : Release Date(s)</td>
                                </tr>
                                <tr id="tr3" runat="server" visible="false">
                                    <td valign="top" width="100%">
                                        <table cellpadding="3" cellspacing="0" border="0">
                                            <tr>
                                                <td colspan="2">The following estimated release date was selected by you when you submitted the functional requirements...</td>
                                            </tr>
                                            <tr class="bold">
                                                <td>Estimated Release Date:</td>
                                                <td><asp:Label ID="lblEstimate" runat="server" /></td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">Is this release date still OK?</td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <asp:RadioButton ID="radEstimateYes" runat="server" Text="Yes" CssClass="default" GroupName="radEstimate" />
                                                    <asp:RadioButton ID="radEstimateNo" runat="server" Text="No" CssClass="default" GroupName="radEstimate" />
                                                </td>
                                            </tr>
                                            <tr id="divEstimate" runat="server" style="display:none">
                                                <td colspan="2">
                                                    Select two new release dates (the client will be able to select one of them)...
                                                    <br />
                                                    <br />
                                                    <asp:RadioButtonList ID="radRelease" runat="server" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr id="tr3Wait" runat="server" visible="false">
                                    <td valign="top" width="100%"><i>Waiting for Previous Step to Complete</i></td>
                                </tr>
                                <tr id="tr3Done" runat="server" visible="false">
                                    <td valign="top" width="100%"><asp:Label ID="lbl3" runat="server" CssClass="approved" /></td>
                                </tr>
                                <tr>
                                    <td nowrap rowspan="2" valign="top"><asp:Image ID="img4" runat="server" ImageAlign="absMiddle" ImageUrl="/images/spacer.gif" /></td>
                                    <td class="biggerbold">Step # 4 : Development</td>
                                </tr>
                                <tr id="tr4" runat="server" visible="false">
                                    <td valign="top" width="100%">
                                        <table cellpadding="3" cellspacing="0" border="0">
                                            <tr>
                                                <td>The functional requirements documentation has been sent to the client.</td>
                                            </tr>
                                            <tr>
                                                <td>Once the client reviews the documentation, it will be accepted or rejected.  You will be notified via e-mail either way.</td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr id="tr4Wait" runat="server" visible="false">
                                    <td valign="top" width="100%"><i>Waiting for Previous Step to Complete</i></td>
                                </tr>
                                <tr id="tr4Done" runat="server" visible="false">
                                    <td valign="top" width="100%"><asp:Label ID="lbl4" runat="server" CssClass="approved" /></td>
                                </tr>
                                <tr>
                                    <td nowrap rowspan="2" valign="top"><asp:Image ID="img5" runat="server" ImageAlign="absMiddle" ImageUrl="/images/spacer.gif" /></td>
                                    <td class="biggerbold">Step # 5 : User Acceptance Testing / Change Control</td>
                                </tr>
                                <tr id="tr5" runat="server" visible="false">
                                    <td valign="top" width="100%">
                                        <table cellpadding="3" cellspacing="0" border="0">
                                            <tr>
                                                <td>The functional requirements documentation has been sent to the client.</td>
                                            </tr>
                                            <tr>
                                                <td>Once the client reviews the documentation, it will be accepted or rejected.  You will be notified via e-mail either way.</td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr id="tr5Wait" runat="server" visible="false">
                                    <td valign="top" width="100%"><i>Waiting for Previous Step to Complete</i></td>
                                </tr>
                                <tr id="tr5Done" runat="server" visible="false">
                                    <td valign="top" width="100%"><asp:Label ID="lbl5" runat="server" CssClass="approved" /></td>
                                </tr>
                                <tr>
                                    <td nowrap rowspan="2" valign="top"><asp:Image ID="img6" runat="server" ImageAlign="absMiddle" ImageUrl="/images/spacer.gif" /></td>
                                    <td class="biggerbold">Step # 6 : Release</td>
                                </tr>
                                <tr id="tr6" runat="server" visible="false">
                                    <td valign="top" width="100%">
                                        <table cellpadding="3" cellspacing="0" border="0">
                                            <tr>
                                                <td>The functional requirements documentation has been sent to the client.</td>
                                            </tr>
                                            <tr>
                                                <td>Once the client reviews the documentation, it will be accepted or rejected.  You will be notified via e-mail either way.</td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr id="tr6Wait" runat="server" visible="false">
                                    <td valign="top" width="100%"><i>Waiting for Previous Step to Complete</i></td>
                                </tr>
                                <tr id="tr6Done" runat="server" visible="false">
                                    <td valign="top" width="100%"><asp:Label ID="lbl6" runat="server" CssClass="approved" /></td>
                                </tr>
                            </table>
                        </div>
                        <div style="display:none">
                            <table width="100%" cellpadding="4" cellspacing="3" border="0">
                                <tr>
                                    <td colspan="2" class="greentableheader">Message Thread&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:ImageButton ID="btnMessage" runat="server" ImageUrl="/images/post_reply.gif" ImageAlign="AbsMiddle" ToolTip="Click to Post a Reply" /></td>
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
                        <div style="display:none">
                            <table width="100%" cellpadding="4" cellspacing="3" border="0">
                                <tr>
                                    <td class="greentableheader">Functional Requirements Documentation</td>
                                </tr>
                                <tr>
                                    <td>
                                        <table cellpadding="4" cellspacing="0" border="0" style="border:solid 1px #CCCCCC">
                                            <tr bgcolor="#EEEEEE">
                                                <td></td>
                                                <td><b><u>Path:</u></b></td>
                                                <td></td>
                                            </tr>
                                            <asp:repeater ID="rptDocuments" runat="server">
                                                <ItemTemplate>
                                                    <tr>
                                                        <td valign="top"><%# DataBinder.Eval(Container.DataItem, "latest").ToString() == "1" ? "<img src='/images/ico_check40.gif' border='0' align='absmiddle'/>" : "" %></td>
                                                        <td valign="top">
                                                            <table cellpadding="2" cellspacing="2" border="0">
                                                                <tr>
                                                                    <td><a href='<%# DataBinder.Eval(Container.DataItem, "path") %>' target="_blank"><img src='/images/file.gif' border='0' align='absmiddle' /><img src='/images/spacer.gif' border='0' width='5' height='5' /><%# DataBinder.Eval(Container.DataItem, "path") %></a></td>
                                                                </tr>
                                                                <tr>
                                                                    <td>If all approvals are completed within <b><%# DataBinder.Eval(Container.DataItem, "days") %></b> business days, this enhancement will be available on <b><%# DateTime.Parse(DataBinder.Eval(Container.DataItem, "release").ToString()).ToShortDateString() %></b></td>
                                                                </tr>
                                                            </table>
                                                            
                                                        </td>
                                                        <td valign="top" align="right"><asp:LinkButton ID="btnDeleteDocument" runat="server" Text="Delete" OnClick="btnDeleteDocument_Click" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' /></td>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:repeater>
                                            <tr>
                                                <td colspan="3">
                                                    <asp:Label ID="lblDocuments" runat="server" CssClass="default" Visible="false" Text="<img src='/images/spacer.gif' border='0' height='1' width='25' /><img src='/images/alert.gif' border='0' align='absmiddle'> There are no functional requirement documents" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div style="display:none">
                            <table width="100%" cellpadding="4" cellspacing="3" border="0">
                                <tr>
                                    <td class="greentableheader">Log</td>
                                </tr>
                                <tr>
                                    <td>
                                        <table cellpadding="6" cellspacing="0" border="0" style="border:solid 1px #CCCCCC">
                                            <tr bgcolor="#EEEEEE">
                                                <td></td>
                                                <td><b><u>Status:</u></b></td>
                                                <td><b><u>Action:</u></b></td>
                                                <td><b><u>User:</u></b></td>
                                                <td><b><u>Notified:</u></b></td>
                                            </tr>
                                            <asp:repeater ID="rptLog" runat="server">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblApproved" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "approved") %>' />
                                                    <asp:Label ID="lblRejected" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "rejected") %>' />
                                                    <tr>
                                                        <td valign="top"><asp:Image ID="imgApproval" runat="server" ImageAlign="AbsMiddle" /></td>
                                                        <td valign="top"><asp:Label ID="lblStatus" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "step") %>' /></td>
                                                        <td valign="top"><%# DataBinder.Eval(Container.DataItem, "approval") %></td>
                                                        <td valign="top"><asp:Label ID="lblUser" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "userid") %>' /></td>
                                                        <td valign="top"><%# DataBinder.Eval(Container.DataItem, "created") %></td>
                                                    </tr>
                                                    <tr id="trComments" runat="server" visible="false">
                                                        <td></td>
                                                        <td colspan="4">
                                                            <table width="100%" cellpadding="3" cellspacing="2" border="0">
                                                                <tr>
                                                                    <td valign="top"><img src="/images/comment.gif" border="0" /></td>
                                                                    <td valign="top"><asp:Label ID="lblComments" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "comments") %>' /></td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:repeater>
                                            <tr>
                                                <td colspan="3">
                                                    <asp:Label ID="lblLog" runat="server" CssClass="default" Visible="false" Text="<img src='/images/spacer.gif' border='0' height='1' width='25' /><img src='/images/alert.gif' border='0' align='absmiddle'> There are no functional requirement documents" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
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
<asp:Label ID="lblStep" runat="server" Visible="false" />
 