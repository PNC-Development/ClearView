<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="enhancement.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.enhancement" %>
<script type="text/javascript">
    function DisableOtherRadioList(o1, o2, _disable) {
        o2 = document.getElementById(o2);
        var _inputs = o2.getElementsByTagName("INPUT");
        for(var ii=0; ii<_inputs.length; ii++) {
            var oObject = _inputs[ii];
            if (oObject.value == o1.value) {
                oObject.checked = false;
                if (_disable)
                    oObject.disabled = true;
            }
            else if (_disable)
                oObject.disabled = false;
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
            <asp:Panel ID="panEnhancement" runat="server" Visible="false">
                <table width="100%" cellpadding="0" cellspacing="5" border="0">
                    <tr>
                        <td rowspan="2"><img src="/images/bigHelp.gif" border="0" align="absmiddle" /></td>
                        <td class="header" width="100%" valign="bottom">Enhancement Request</td>
                    </tr>
                    <tr>
                        <td width="100%" valign="top">If you have an idea to enhance ClearView, submit it here.</td>
                    </tr>
                </table>
                <asp:Panel ID="panNew" runat="server" Visible="false">
                    <%=strMenuTab1 %>
                    <div id="divMenu1">
                        <br />
                        <div style="display:none">
                            <table width="100%" cellpadding="4" cellspacing="3" border="0">
                                <tr id="trFunctionalRequirementsDocument" runat="server" visible="false">
                                    <td colspan="2" class="box_yellow">
                                        <img src="/images/alert.gif" border="0" align="absmiddle" /> <b>NOTE:</b> Click on the &quot;Functional Requirements&quot; tab to approve or reject the functional requirements.
                                    </td>
                                </tr>
                                <tr>
                                    <td nowrap>Title:<font class="required">&nbsp;*</font></td>
                                    <td width="100%"><asp:TextBox ID="txtTitle" runat="server" CssClass="default" Width="600" MaxLength="100" /></td>
                                </tr>
                                <tr>
                                    <td nowrap valign="top">Description:<font class="required">&nbsp;*</font></td>
                                    <td width="100%"><asp:TextBox ID="txtDescription" runat="server" CssClass="default" Width="600" Rows="7" TextMode="MultiLine" /></td>
                                </tr>
                                <tr>
                                    <td nowrap>Number of users benefited:<font class="required">&nbsp;*</font></td>
                                    <td width="100%">
                                        <asp:DropDownList ID="ddlUsers" runat="server" CssClass="default">
                                            <asp:ListItem Value="0" Text="-- SELECT --" />
                                            <asp:ListItem Value="1" Text="Just Me" />
                                            <asp:ListItem Value="10" Text="Just My Group / Department" />
                                            <asp:ListItem Value="100" Text="My Entire Organization" />
                                            <asp:ListItem Value="1000" Text="100 - 1,000" />
                                            <asp:ListItem Value="10000" Text="Over 1,000 Users" />
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td nowrap>URL:</td>
                                    <td width="100%"><asp:TextBox ID="txtURL" runat="server" CssClass="default" Width="500" MaxLength="200" /> <asp:HyperLink id="hypURL" runat="server" Target="_blank" Text="Open URL" Visible="false" /></td>
                                </tr>
                                <tr>
                                    <td nowrap>Screenshot / Attachment:</td>
                                    <td width="100%">
                                        <asp:Panel ID="panUpload" runat="server" Visible="false">
                                            <asp:FileUpload id="fileUpload" runat="server" CssClass="default" Width="600" />
                                        </asp:Panel>
                                        <asp:Panel ID="panUploaded" runat="server" Visible="false">
                                            <asp:HyperLink id="hypUpload" runat="server" Target="_blank" Text="Click Here to View File" />&nbsp;&nbsp;&nbsp;<asp:Button ID="btnDeleteAttachment" runat="server" CssClass="default" Width="75" Text="Delete" OnClick="btnDeleteAttachment_Click" />
                                        </asp:Panel>
                                    </td>
                                </tr>
                                <tr>
                                    <td nowrap valign="top">Release Dates:<font class="required">&nbsp;*</font></td>
                                    <td width="100%">Select one from EACH of the following lists:<br /><br />
                                        <table cellpadding="3" cellspacing="2" border="0">
                                            <tr>
                                                <td class="bold">Preferred Release Date:</td>
                                                <td rowspan="2"><img src="/images/spacer.gif" border="0" width="15" height="1" /></td>
                                                <td class="bold">Alternative Release Date:</td>
                                            </tr>
                                            <tr>
                                                <td><asp:RadioButtonList ID="radRelease1" runat="server" /></td>
                                                <td><asp:RadioButtonList ID="radRelease2" runat="server" /></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td nowrap></td>
                                    <td width="100%"><b>NOTE:</b> Every effort will be made to meet the preferred release date. However, your enhancement will not be available until you receive your final release date.  Invalid / changed requirements, delayed approvals and general scope of enhancement could cause considerable delays.</td>
                                </tr>
                                <tr>
                                    <td nowrap>Requested By:</td>
                                    <td width="100%"><asp:Label ID="lblRequestBy" runat="server" CssClass="default" /></td>
                                </tr>
                                <tr>
                                    <td nowrap>Requested On:</td>
                                    <td width="100%"><asp:Label ID="lblRequestOn" runat="server" CssClass="default" /></td>
                                </tr>
                                <tr ID="trStatus" runat="server" Visible="false">
                                    <td nowrap bgcolor="#C1FFC1" class="bold">Current Status:</td>
                                    <td width="100%"><asp:Label ID="lblStatus" runat="server" CssClass="default" /></td>
                                </tr> 
                                <tr ID="trComments" runat="server" Visible="false">
                                    <td></td>
                                    <td width="100%">
                                        <table cellpadding="3" cellspacing="2" border="0">
                                            <tr>
                                                <td class="biggerbold">The following comments were added....</td>
                                            </tr>
                                            <tr>
                                                <td><asp:Label ID="lblComments" runat="server" /></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr> 
                                <tr ID="trSave1" runat="server" Visible="false">
                                    <td colspan="2"><hr size="1" noshade /></td>
                                </tr>
                                <tr ID="trSave2" runat="server" Visible="false">
                                    <td class="required">* = Required Field</td>
                                    <td width="100%">
                                        <asp:Button ID="btnSave" runat="server" CssClass="default" Text="Submit" Width="100" OnClick="btnSave_Click" Visible="false" /> <asp:Button ID="btnUpdate" runat="server" CssClass="default"  Text="Update" Width="100" OnClick="btnUpdate_Click" Visible="false" /> 
                                        <asp:Button ID="btnCancel" runat="server" CssClass="default" Text="Cancel" Width="100" OnClick="btnCancel_Click" Visible="false" />
                                    </td>
                                </tr> 
                                <tr ID="trLocked" runat="server" Visible="false">
                                    <td></td>
                                    <td colspan="2">
                                        <table cellpadding="0" cellspacing="5" border="0" style="border:solid 1px #999999" bgcolor="#f9f9f9">
                                            <tr>
                                                <td rowspan="2"><img src="/images/lock.gif" border="0" align="absmiddle" /></td>
                                                <td class="redheader" valign="bottom">Enhancement Locked</td>
                                            </tr>
                                            <tr>
                                                <td valign="top">You cannot edit your request once it is being processed.</td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr> 
                            </table>
                        </div>
                        <div style="display:none">
                            <table width="100%" cellpadding="4" cellspacing="3" border="0">
                                <tr>
                                    <td class="biggerbold">The functional requirements have been created and are ready for you to review.</td>
                                </tr>
                                <tr>
                                    <td><img src="/images/file.gif" border="0" align="absmiddle" /><img src="/images/spacer.gif" border="0" width="5" height="5" /><asp:HyperLink ID="hypFunctionalRequirements" runat="server" Text="View Functional Requirements Document" Target="_blank" /></td>
                                </tr>
                                <tr>
                                    <td>Assuming all approvals are completed within <asp:Label ID="lblDays" runat="server" CssClass="bold" /> business days, this enhancement will be released on <asp:Label ID="lblRelease" runat="server" CssClass="bold" /></td>
                                </tr>
                                <tr>
                                    <td>Once you are finished reviewing the changes that will be made to ClearView, you are required to make a selection from the options below.</td>
                                </tr>
                                <tr>
                                    <td><hr size="1" noshade /></td>
                                </tr>
                                <tr id="trFunctionalButtons" runat="server" style="display:inline">
                                    <td>
                                        <asp:Button ID="btnFunctionalApproving" runat="server" CssClass="default" Text="Approve" Width="100" Enabled="false" /> 
                                        <asp:Button ID="btnFunctionalRejecting" runat="server" CssClass="default"  Text="Reject" Width="100" Enabled="false" /> 
                                    </td>
                                </tr>
                                <tr id="trFunctionalApprove" runat="server" style="display:none">
                                    <td>
                                        <table cellpadding="0" cellspacing="5" border="0" style="border:solid 1px #999999" bgcolor="#f9f9f9">
                                            <tr>
                                                <td rowspan="4" valign="top"><img src="/images/circlealert.gif" border="0" align="absmiddle" /></td>
                                                <td class="header" valign="bottom">Acknowledgement</td>
                                            </tr>
                                            <tr>
                                                <td valign="top">You must acknowledge that you have read the following conditions regarding this enhancement...</td>
                                            </tr>
                                            <tr>
                                                <td valign="top">
                                                    <ul>
                                                        <li>Once approved, an additional list of people may be notified to approve this design.</li><br /><br />
                                                        <li>If approvers are added, they will be notified via email from ClearView, however it is your responsibility to have these people approve your enhancement.</li><br /><br />
                                                        <li>If one or more approvers delay their approval, your preferred and / or alternate release dates may be missed.</li><br /><br />
                                                        <li>The release date of this enhancement is subject to change due to prioritization.  You will be notified in this case.</li><br /><br />
                                                        <li>Once development is complete, you may be required to select your final release date, if the estimated release date (shown above) cannot be met.</li><br /><br />
                                                        <li>Once development is complete, you will be required to perform user acceptance testing.</li><br /><br />
                                                        <li>A test environment may be required for user acceptance testing.  Failure to provide a test environment (if required) will result in the cancellation of this enhancement.</li><br /><br />
                                                        <li>You will be required to acknowledge the functionality of your enhancement prior to deployment.  Failure to do so will result in the cancellation of this enhancement.</li><br /><br />
                                                        <li>If problems are encountered during user acceptance testing, your enhancement will return to development or requirements gathering and you may miss your intended release date.</li><br /><br />
                                                    </ul>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="top"><asp:CheckBox ID="chkApprove" runat="server" Text="I acknowldedge that I have read, understand and agree to the conditions listed above" /></td>
                                            </tr>
                                        </table>
                                        <br /><br />
                                        <asp:Button ID="btnFunctionalApprove" runat="server" CssClass="default" Text="Approve" Width="100" OnClick="btnFunctionalApprove_Click" /> 
                                        <asp:Button ID="btnFunctionalCancelApprove" runat="server" CssClass="default" Text="Cancel" Width="100" /> 
                                    </td>
                                </tr>
                                <tr id="trFunctionalReject" runat="server" style="display:none">
                                    <td>
                                        <asp:TextBox ID="txtFunctionalReject" runat="server" Width="600" Rows="8" TextMode="MultiLine" />
                                        <br /><br />
                                        <asp:Button ID="btnFunctionalReject" runat="server" CssClass="default" Text="Reject" Width="100" OnClick="btnFunctionalReject_Click" /> 
                                        <asp:Button ID="btnFunctionalCancelReject" runat="server" CssClass="default" Text="Cancel" Width="100" /> 
                                    </td>
                                </tr>
                                <tr id="trFunctionalStatus" runat="server" style="display:inline">
                                    <td>
                                        <asp:Image ID="imgFunctionalStatus" runat="server" ImageAlign="AbsMiddle" /> <asp:Label ID="lblFunctionalStatus" runat="server" CssClass="biggerbold" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div style="display:none">
                            <table width="100%" cellpadding="4" cellspacing="3" border="0">
                                <tr>
                                    <td colspan="2" class="header"><asp:ImageButton ID="btnMessageReplyImage" runat="server" ImageUrl="/images/post_reply.gif" ImageAlign="AbsMiddle" ToolTip="Click to Post a Reply" />&nbsp;&nbsp;&nbsp;<img src="/images/arrow_green_left.gif" border="0" align="absmiddle" />&nbsp;&nbsp;&nbsp;Click Here to Reply!</td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <div id="divMessageReply" runat="server" style="display:none">
                                            <table width="100%" cellpadding="3" cellspacing="0" border="0">
                                                <tr>
                                                   <td>Enter your response, upload an attachment (optional) and click <b>Send</b> to send your response:</td>
                                                </tr>
                                                <tr>
                                                   <td><asp:TextBox ID="txtMessageReply" runat="server" CssClass="default" Width="600" Rows="7" TextMode="MultiLine" /></td>
                                                </tr>
                                                <tr>
                                                    <td><asp:FileUpload id="filMessageReply" runat="server" CssClass="default" Width="600" /></td>
                                                </tr>
                                                <tr>
                                                   <td><asp:Button ID="btnMessageReply" runat="server" CssClass="default" Text="Send" Width="100" OnClick="btnMessageReply_Click" /></td>
                                                </tr>
                                            </table>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2"><%= strMessages %></td>
                                </tr>
                            </table>
                        </div>
                        <div style="display:none">
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
                        </div>
                </asp:Panel>
                <asp:Panel ID="panOld" runat="server" Visible="false">
                    <table width="100%" cellpadding="4" cellspacing="3" border="0">
                        <tr>
                            <td nowrap>Enhancement Title:<font class="required">&nbsp;*</font></td>
                            <td width="100%"><asp:Label ID="lblOldTitle" runat="server" /></td>
                        </tr>
                        <tr>
                            <td nowrap valign="top">Description:<font class="required">&nbsp;*</font></td>
                            <td width="100%"><asp:Label ID="lblOldDescription" runat="server" /></td>
                        </tr>
                        <tr>
                            <td nowrap>Module:</td>
                            <td width="100%"><asp:Label ID="lblOldModule" runat="server" /></td>
                        </tr>
                        <tr>
                            <td nowrap>Approximate number of users benefitted:</td>
                            <td width="100%"><asp:Label ID="lblOldNumUsers" runat="server" /></td>
                        </tr>
                        <tr>
                            <td nowrap>URL:</td>
                            <td width="100%"><asp:HyperLink ID="hypOldURL" runat="server" Target="_blank" Text="<img src='/images/file.gif' border='0' align='absmiddle'/><img src='/images/spacer.gif' border='0' width='5' height='5'/>Click here" /></td>
                        </tr>
                        <tr>
                            <td nowrap>Screenshot / Attachment:</td>
                            <td width="100%"><asp:HyperLink ID="hypOldAttach" runat="server" Target="_blank" Text="<img src='/images/file.gif' border='0' align='absmiddle'/><img src='/images/spacer.gif' border='0' width='5' height='5'/>Click here" /></td>
                        </tr>
                        <tr>
                            <td nowrap>Estimated Start Date:<font class="required">&nbsp;*</font></td>
                            <td width="100%"><asp:Label ID="lblOldStart" runat="server" /></td>
                        </tr>
                        <tr>
                            <td nowrap>Estimated End Date:<font class="required">&nbsp;*</font></td>
                            <td width="100%"><asp:Label ID="lblOldEnd" runat="server" /></td>
                        </tr> 
                        <tr>
                            <td nowrap>Requested By:</td>
                            <td width="100%"><asp:Label ID="lblOldRequestedBy" runat="server" /></td>
                        </tr>
                        <tr>
                            <td nowrap>Requested On:</td>
                            <td width="100%"><asp:Label ID="lblOldRequestedOn" runat="server" /></td>
                        </tr>
                        <tr>
                            <td nowrap bgcolor="#C1FFC1" class="bold">Current Status:</td>
                            <td width="100%"><asp:Label ID="lblOldStatus" runat="server" /></td>
                        </tr> 
                        <tr>
                            <td nowrap class="bold">Scheduled Release Date:</td>
                            <td width="100%"><asp:Label ID="lblOldRelease" runat="server" /></td>
                        </tr> 
                    </table>
                </asp:Panel>
                <asp:Panel ID="panMessage" runat="server" Visible="false">
                    <table width="100%" cellpadding="4" cellspacing="3" border="0">
                        <tr>
                            <td colspan="2">&nbsp;</td>
                        </tr>
                        <tr>
                            <td colspan="2">&nbsp;</td>
                        </tr>
                        <tr>
                            <td colspan="2" class="wrapper">Message Thread:</td>
                        </tr>
                        <tr>
                            <td colspan="2" class="header"><asp:ImageButton ID="btnMessage" runat="server" ImageUrl="/images/post_reply.gif" ImageAlign="AbsMiddle" ToolTip="Click to Post a Reply" />&nbsp;&nbsp;&nbsp;<img src="/images/arrow_green_left.gif" border="0" align="absmiddle" />&nbsp;&nbsp;&nbsp;Click Here to Reply!</td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <div id="divMessage" runat="server" style="display:none">
                                    <table width="100%" cellpadding="3" cellspacing="0" border="0">
                                        <tr>
                                           <td>Enter your response, upload an attachment (optional) and click <b>Send</b> to send your response:</td>
                                        </tr>
                                        <tr>
                                           <td><asp:TextBox ID="txtResponse" runat="server" CssClass="default" Width="600" Rows="7" TextMode="MultiLine" /></td>
                                        </tr>
                                        <tr>
                                            <td><asp:FileUpload id="oFile" runat="server" CssClass="default" Width="600" /></td>
                                        </tr>
                                        <tr>
                                           <td><asp:Button ID="btnResponse" runat="server" CssClass="default" Text="Send" Width="100" OnClick="btnResponse_Click" /></td>
                                        </tr>
                                    </table>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2"><%= strMessages %></td>
                        </tr>
                    </table>
            </asp:Panel>
            </asp:Panel>
            <asp:Panel ID="panNoEnhancement" runat="server" Visible="false">
                <table width="100%" cellpadding="5" cellspacing="5" border="0">
                    <tr>
                        <td rowspan="2" valign="top"><img src="/images/hugeAlert.gif" border="0" align="absmiddle" /></td>
                        <td class="header" width="100%" valign="bottom">The ClearView Community - Ideation Blog</td>
                    </tr>
                    <tr>
                        <td width="100%" valign="top">
                            <p>For enhancements in ClearView, post your suggestion to the ideation blog in the <asp:HyperLink ID="hypCommunity" runat="server" Text="ClearView Community" Target="_blank" />.</p>
                            <p>
                                <ol>
                                    <li>Click on the above link to access the ClearView Community</li>
                                    <li>If not already a member, join the community</li>
                                    <li>Within the community navigation, click on Ideation Blog</li>
                                    <li>Click on the &quot;New Idea&quot; button</li>
                                    <li>Enter a title and description and click Post</li>
                                </ol>
                            </p>
                            <p><b>NOTE:</b> Existing enhancements can still be monitored by using the &quot;My Enhancements&quot; queue.</p>
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
 