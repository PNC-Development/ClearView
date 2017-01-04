<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="resource_request_editor.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.resource_request_editor" %>


<script type="text/javascript">
    function ShowService(strURL) {
        window.navigate(strURL + "&menu_tab=3");
    }
    function EnsureMailbox(oTM, oMBX, txtMBX) {
        if (ValidateRadioButtons(oTM, oMBX, 'Please select how you want to receive notifications regarding new service requests') == false)
            return false;
        else {
            oMBX = document.getElementById(oMBX);
            if (oMBX.checked == true)
                return ValidateText(txtMBX, 'Please enter a group mailbox');
        }
        return true;
    }
    function CheckGroups(oList) {
        oList = document.getElementById(oList);
        if (oList == null)
            return true;
        else if (oList.selectedIndex == -1) {
            alert('Please select a group');
            oList.focus();
            return false;
        }
        else
            return true;
    }
    function AddTask(strService, strParent) {
        OpenWindow('SERVICE_TASK','?service=' + strService + '&parent=' + strParent);
    }
    function EditTask(strID, strService, strParent) {
        OpenWindow('SERVICE_TASK','?id=' + strID + '&service=' + strService + '&parent=' + strParent);
    }
    function AddWorkflow(strID, strSameTimeYes) {
        strSameTimeYes = document.getElementById(strSameTimeYes);
        alert('!!!! ALERT !!!!!\n\nWorkflow is an advanced feature of ClearView Service Editor. All workflow services should be created PRIOR to configuring their interaction.  An agreement should be made by both the sending and receiving service owners. Often times, people configure a workflow without discussing it with the receiving party...thus preventing tasks from being completed.\n\nOnce all of the services have been created, you can configure your workflow(s) here.');
        if (strSameTimeYes != null && strSameTimeYes.checked == true)
            return OpenWindow('SERVICE_EDITOR_WORKFLOW','?id=' + strID + '&sametime=1');
        else
            return OpenWindow('SERVICE_EDITOR_WORKFLOW','?id=' + strID + '&sametime=0');
    }
//    function EnsureSameTime(radYes, radNo, intWorkflows) {
//        radNo = document.getElementById(radNo);
//        if (radNo.checked == true && intWorkflows > 1) {
//            alert('If there are multiple services configured for the workflow, they must be started synchronously.\n\nEither delete one or more workflows or change to synchronous execution');
//            return false;
//        }
//        return true;
//    }
    function EnsureWorkflowGone(radNo, radYes, intWorkflows) {
        radNo = document.getElementById(radNo);
        if (radNo.checked == true && intWorkflows > 0 && confirm('NOTE: This service is currently receiving information from ' + intWorkflows + ' workflow(s).\nRequesting information directly from the requestor will remove these associations.\n\nAre you sure you want to do this?') == false) {
            radYes = document.getElementById(radYes);
            radYes.click();
            return false;
        }
        return true;
    }
    function EnsureAssignment(radM, intM, radT, intT, radW, ddlW) {
        radM = document.getElementById(radM);
        radT = document.getElementById(radT);
        radW = document.getElementById(radW);
        if (radM.checked == false && radT.checked == false && radW.checked == false) {
            alert('Select an option on how you want ClearView to handle new requests');
            SetFocus(radM);
            return false;
        }
        else if (radM.checked == true && intM == 0) {
            alert('Add at least one supervisor to the list');
            SetFocus(radM);
            return false;
        }
        else if (radT.checked == true && intT == 0) {
            alert('Add at least one resource to the list');
            SetFocus(radM);
            return false;
        }
        else if (radW.checked == true && ddlW != null && ValidateDropDown(ddlW, "Select a previous service to assign the request") == false) {
            SetFocus(radW);
            return false;
        }
        return true;
    }
    function Publish(_url, _new) {
        LoadWait();
        if (confirm('Congratulations! Your new service has been created.\n\nClearView would like to notify its community by publishing the creation of this service to the home page of ClearView.\n\nClick OK to confirm.') == true)
            window.location = _url + "?new=" + _new + "&publish=true";
        else
            window.location = _url;
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
        <td width="100%" bgcolor="#FFFFFF" align="center">
            <asp:Panel ID="panPermit"  runat="server" Visible="false">
            <table width="100%" cellpadding="0" cellspacing="5" border="0">
                <tr>
                    <td rowspan="2"><img src="/images/service_request.gif" border="0" align="absmiddle" /></td>
                    <td class="header" width="100%" valign="bottom">Service Editor</td>
                </tr>
                <tr>
                    <td width="100%" valign="top">Using this page, you can configure the services your department offers to other departments within the company.</td>
                </tr>
                <tr>
                    <td colspan="2" align="right">
                        <asp:Button ID="btnNew" runat="server" Text="Create a Service" OnClick="btnNew_Click" />
                    </td>
                </tr>
            </table>
            <%=strMenuTab1 %>
            <div id="divMenu1">
            <div style="display:none">
                <br />
                <table width="100%" cellpadding="4" cellspacing="0" border="0" style="border:solid 1px #CCCCCC">
                    <tr bgcolor="#EEEEEE">
                        <td nowrap><b>Service Name</b></td>
                        <td nowrap><b>Created</b></td>
                        <td nowrap><b>Last Updated</b></td>
                        <td nowrap><b>Status</b></td>
                    </tr>
                    <%=strServicesC %>
                </table>
            </div>
            <div style="display:none">
                <br />
                <table width="100%" cellpadding="4" cellspacing="0" border="0" style="border:solid 1px #CCCCCC">
                    <tr bgcolor="#EEEEEE">
                        <td nowrap><b>Service Name</b></td>
                        <td nowrap><b>Created</b></td>
                        <td nowrap><b>Last Updated</b></td>
                        <td nowrap><b>Status</b></td>
                    </tr>
                    <%=strServicesI %>
                </table>
            </div>
            <div style="display:none">
                <br />
                <table width="100%" cellpadding="0" cellspacing="5" border="0">
                    <tr>
                        <td valign="top"><img src="/images/spacer.gif" border="0" height="1" width="5" /></td>
                        <td valign="top" width="150">
                            <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                <tr height="1">
                                    <td><img src="/images/table_topleft.gif" border="0" width="5" height="26"></td>
                                    <td nowrap background="/images/table_top.gif" class="greentableheader" width="100%">Service Editor Steps</td>
                                    <td><img src="/images/table_topright.gif" border="0" width="5" height="26"></td>
                                </tr>
                                <tr>
                                    <td background="/images/table_left.gif"><img src="/images/table_left.gif" width="5" height="10"></td>
                                    <td width="100%" bgcolor="#FFFFFF" valign="top"><%=strEdit %></td>
                                    <td background="/images/table_right.gif"><img src="/images/table_right.gif" width="5" height="10"></td>
                                </tr>
                                <tr height="1">
                                    <td><img src="/images/table_bottomLeft.gif" border="0" width="5" height="9"></td>
                                    <td width="100%" background="/images/table_bottom.gif"></td>
                                    <td><img src="/images/table_bottomRight.gif" border="0" width="5" height="9"></td>
                                </tr>
                            </table>
                        </td>
                        <td valign="top">
                            <asp:Panel ID="panStep1" runat="server" Visible="false">
                                <table width="100%" border="0" cellpadding="4" cellspacing="3">
                                    <tr>
                                        <td nowrap>Service Name:</td>
                                        <td width="100%"><asp:TextBox ID="txtName" runat="server" CssClass="default" Width="400" MaxLength="100" /></td>
                                    </tr>
                                    <tr>
                                        <td nowrap valign="top">Description:</td>
                                        <td width="100%"><asp:TextBox ID="txtDescription" runat="server" CssClass="default" Width="500" TextMode="MultiLine" Rows="6" /></td>
                                    </tr>
                                    <asp:Panel ID="panGroup" runat="server" Visible="false">
                                    <tr>
                                        <td nowrap valign="top">Group:</td>
                                        <td width="100%"><asp:ListBox ID="lstGroup" runat="server" CssClass="default" Width="300" Rows="6" /></td>
                                    </tr>
                                    <tr>
                                        <td nowrap></td>
                                        <td width="100%" class="help">(Please select the group that will be offering this service. For additional help, please contact your ClearView administrator.)</td>
                                    </tr>
                                    </asp:Panel>
                                    <tr>
                                        <td nowrap>Project Required:</td>
                                        <td width="100%">
                                            <asp:RadioButton ID="radProjectYes" runat="server" Text="Yes" CssClass="default" GroupName="PROJ" />&nbsp;
                                            <asp:RadioButton ID="radProjectNo" runat="server" Text="No" CssClass="default" GroupName="PROJ" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td nowrap></td>
                                        <td width="100%" class="help">(Do you require a project to be associated with this service?)</td>
                                    </tr>
                                    <tr id="divProjectNo" runat="server" style="display:none">
                                        <td nowrap></td>
                                        <td width="100%">
                                            <table cellpadding="3" cellspacing="2" border="0" style="border:solid 1px #999999" bgcolor="#f9f9f9">
                                                <tr>
                                                    <td rowspan="7" valign="top"><img src="/images/bigAlert.gif" border="0" align="absmiddle" /></td>
                                                    <td><b>NOTE:</b> When a service does not require a project, a title is presented to the requestor to uniquely identify the request.</td>
                                                </tr>
                                                <tr>
                                                    <td>By default, the title is &quot;Please enter a title for this request&quot;.</td>
                                                </tr>
                                                <tr>
                                                    <td>&nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td>Would you like to override this setting and use a custom title?</td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:RadioButton ID="radTitleOverrideYes" runat="server" Text="Yes - override this title" CssClass="default" GroupName="TITLE" />&nbsp;
                                                        <asp:RadioButton ID="radTitleOverrideNo" runat="server" Text="No - use the default title" CssClass="default" GroupName="TITLE" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>&nbsp;</td>
                                                </tr>
                                                <tr id="divTitleOverrideYes" runat="server" style="display:none">
                                                    <td>
                                                        Please enter your custom title:<br /><br />
                                                        <asp:TextBox ID="txtTitleName" runat="server" CssClass="default" Width="500" MaxLength="100" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td nowrap>Available:</td>
                                        <td width="100%">
                                            <asp:RadioButton ID="radEnabledYes" runat="server" Text="Yes" CssClass="default" GroupName="ENABLE" />&nbsp;
                                            <asp:RadioButton ID="radEnabledNo" runat="server" Text="No" CssClass="default" GroupName="ENABLE" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td nowrap></td>
                                        <td width="100%" class="help">(Do you want this service to be available for clients to select and submit?)</td>
                                    </tr>
                                </table>
                                <br />
                                <table width="100%" border="0" cellpadding="4" cellspacing="3">
                                    <tr>
                                        <td><hr size="1" noshade /></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Panel ID="panStep1Next" runat="server" Visible="false">
                                                <asp:Button ID="btnBack1" runat="server" CssClass="default" Width="100" Text="<<  Back" Enabled="false" /> 
                                                <asp:Button ID="btnNext1" runat="server" CssClass="default" Width="100" Text="Next  >>" OnClick="btnNext1_Click" /> 
                                            </asp:Panel>
                                            <asp:Panel ID="panStep1Update" runat="server" Visible="false">
                                                <asp:Button ID="btnUpdate1" runat="server" CssClass="default" Width="100" Text="Update" OnClick="btnUpdate1_Click" /> 
                                                <asp:Button ID="btnCancel1" runat="server" CssClass="default" Width="100" Text="Cancel" OnClick="btnCancel_Click" /> 
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:Panel ID="panStep2" runat="server" Visible="false">
                                <table width="100%" border="0" cellpadding="4" cellspacing="3">
                                    <tr>
                                        <td nowrap class="biggerbold">Service Name:</td>
                                        <td width="100%" class="biggerbold">&quot;<asp:Label ID="lblName2" runat="server" CssClass="biggerbold" />&quot;</td>
                                    </tr>
                                </table>
                                <table width="100%" border="0" cellpadding="4" cellspacing="3">
                                    <tr>
                                        <td colspan="2">Please select whether or not you want to be able to reject requests?</td>
                                    </tr>
                                    <tr>
                                        <td nowrap><img src="/images/spacer.gif" border="0" width="<%=strIndent %>" height="1" /></td>
                                        <td width="100%">
                                            <asp:RadioButton ID="radRejectYes" runat="server" Text="Yes - allow the ability to reject requests&nbsp;&nbsp;<i>[default]</i>" CssClass="default" GroupName="REJECT" /><br />
                                            <asp:RadioButton ID="radRejectNo" runat="server" Text="No - do NOT allow the ability to reject requests" CssClass="default" GroupName="REJECT" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">Does this service require the approval of the requestor's manager?</td>
                                    </tr>
                                    <tr>
                                        <td nowrap><img src="/images/spacer.gif" border="0" width="<%=strIndent %>" height="1" /></td>
                                        <td width="100%">
                                            <asp:RadioButton ID="radManagerApproveYes" runat="server" Text="Yes - requires the requestor's manager to approve to submit" CssClass="default" GroupName="MANAGER_APPROVE" /><br />
                                            <asp:RadioButton ID="radManagerApproveNo" runat="server" Text="No - does NOT require the requestor's manager to approve to submit&nbsp;&nbsp;<i>[default]</i>" CssClass="default" GroupName="MANAGER_APPROVE" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">Should the user be able to change the quantity of this service (in the shopping cart)?</td>
                                    </tr>
                                    <tr>
                                        <td nowrap><img src="/images/spacer.gif" border="0" width="<%=strIndent %>" height="1" /></td>
                                        <td width="100%">
                                            <asp:RadioButton ID="radQuantityYes" runat="server" Text="Yes - allow the user to change the quantity" CssClass="default" GroupName="QTY" /><br />
                                            <asp:RadioButton ID="radQuantityNo" runat="server" Text="No - do NOT allow the user to change the quantity&nbsp;&nbsp;<i>[default]</i>" CssClass="default" GroupName="QTY" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">Does this service have multiple tasks or actions that must be finished to complete this task?</td>
                                    </tr>
                                    <tr>
                                        <td nowrap><img src="/images/spacer.gif" border="0" width="<%=strIndent %>" height="1" /></td>
                                        <td width="100%">
                                            <asp:RadioButton ID="radTasksYes" runat="server" Text="Yes - this service has multiple tasks associated with it&nbsp;&nbsp;<i>[default]</i>" CssClass="default" GroupName="TASKS" /><br />
                                            <asp:RadioButton ID="radTasksNo" runat="server" Text="No - this service has only one (1) task associated with it" CssClass="default" GroupName="TASKS" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">In the future, do you think this service could be automated?</td>
                                    </tr>
                                    <tr>
                                        <td nowrap><img src="/images/spacer.gif" border="0" width="<%=strIndent %>" height="1" /></td>
                                        <td width="100%">
                                            <asp:RadioButton ID="radAutomateYes" runat="server" Text="Yes - this service could be automated" CssClass="default" GroupName="AUTOMATE" /><br />
                                            <asp:RadioButton ID="radAutomateNo" runat="server" Text="No - this service cannot be automated&nbsp;&nbsp;<i>[default]</i>" CssClass="default" GroupName="AUTOMATE" />
                                        </td>
                                    </tr>
                                    <tr style="display:none">
                                        <td colspan="2">&nbsp;</td>
                                    </tr>
                                    <tr style="display:none">
                                        <td colspan="2">Should there be a &quot;statement of work&quot; input field associated with this request?</td>
                                    </tr>
                                    <tr style="display:none">
                                        <td nowrap><img src="/images/spacer.gif" border="0" width="<%=strIndent %>" height="1" /></td>
                                        <td width="100%">
                                            <asp:RadioButton ID="radStatementYes" runat="server" Text="Yes - include a statement of work field" CssClass="default" GroupName="STATEMENT" /><br />
                                            <asp:RadioButton ID="radStatementNo" runat="server" Text="No - do NOT include a statement of work field&nbsp;&nbsp;<i>[default]</i>" CssClass="default" GroupName="STATEMENT" />
                                        </td>
                                    </tr>
                                    <tr style="display:none">
                                        <td colspan="2">&nbsp;</td>
                                    </tr>
                                    <tr style="display:none">
                                        <td colspan="2">Could this service require documentation or files to be submitted with this request?</td>
                                    </tr>
                                    <tr style="display:none">
                                        <td nowrap><img src="/images/spacer.gif" border="0" width="<%=strIndent %>" height="1" /></td>
                                        <td width="100%">
                                            <asp:RadioButton ID="radUploadYes" runat="server" Text="Yes - allow files to be uploaded&nbsp;&nbsp;<i>[default]</i>" CssClass="default" GroupName="UPLOAD" /><br />
                                            <asp:RadioButton ID="radUploadNo" runat="server" Text="No - do NOT allow files to be uploaded" CssClass="default" GroupName="UPLOAD" />
                                        </td>
                                    </tr>
                                    <tr style="display:none">
                                        <td colspan="2">&nbsp;</td>
                                    </tr>
                                    <tr style="display:none">
                                        <td colspan="2">Can this service be expedited?</td>
                                    </tr>
                                    <tr style="display:none">
                                        <td nowrap><img src="/images/spacer.gif" border="0" width="<%=strIndent %>" height="1" /></td>
                                        <td width="100%">
                                            <asp:RadioButton ID="radExpediteYes" runat="server" Text="Yes - this service could be expedited&nbsp;&nbsp;<i>[default]</i>" CssClass="default" GroupName="EXPEDITE" /><br />
                                            <asp:RadioButton ID="radExpediteNo" runat="server" Text="No - this service cannot be expedited" CssClass="default" GroupName="EXPEDITE" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">Do you want to notify the requestor when a status has been changed to RED (On Hold)?</td>
                                    </tr>
                                    <tr>
                                        <td nowrap><img src="/images/spacer.gif" border="0" width="<%=strIndent %>" height="1" /></td>
                                        <td width="100%">
                                            <asp:RadioButton ID="radNotifyRedYes" runat="server" Text="Yes - notify requestor&nbsp;&nbsp;<i>[default]</i>" CssClass="default" GroupName="NOTIFY_RED" /><br />
                                            <asp:RadioButton ID="radNotifyRedNo" runat="server" Text="No - do NOT notify requestor" CssClass="default" GroupName="NOTIFY_RED" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">Do you want to notify the requestor when a status has been changed to YELLOW (Issue Encountered)?</td>
                                    </tr>
                                    <tr>
                                        <td nowrap><img src="/images/spacer.gif" border="0" width="<%=strIndent %>" height="1" /></td>
                                        <td width="100%">
                                            <asp:RadioButton ID="radNotifyYellowYes" runat="server" Text="Yes - notify requestor&nbsp;&nbsp;<i>[default]</i>" CssClass="default" GroupName="NOTIFY_YELLOW" /><br />
                                            <asp:RadioButton ID="radNotifyYellowNo" runat="server" Text="No - do NOT notify requestor" CssClass="default" GroupName="NOTIFY_YELLOW" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">Do you want to notify the requestor when a status has been changed back to GREEN (Issue Resoloved / Taken Off Hold)?</td>
                                    </tr>
                                    <tr>
                                        <td nowrap><img src="/images/spacer.gif" border="0" width="<%=strIndent %>" height="1" /></td>
                                        <td width="100%">
                                            <asp:RadioButton ID="radNotifyGreenYes" runat="server" Text="Yes - notify requestor&nbsp;&nbsp;<i>[default]</i>" CssClass="default" GroupName="NOTIFY_GREEN" /><br />
                                            <asp:RadioButton ID="radNotifyGreenNo" runat="server" Text="No - do NOT notify requestor" CssClass="default" GroupName="NOTIFY_GREEN" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">Once complete, send an email to the following email address(es)? (NOTE: By default, an email is sent to the requestor)</td>
                                    </tr>
                                    <tr>
                                        <td nowrap><img src="/images/spacer.gif" border="0" width="<%=strIndent %>" height="1" /></td>
                                        <td width="100%"><asp:TextBox ID="txtNotifyComplete" runat="server" CssClass="default" Width="500" TextMode="MultiLine" Rows="3" /></td>
                                    </tr>
                                    <tr>
                                        <td nowrap><img src="/images/spacer.gif" border="0" width="<%=strIndent %>" height="1" /></td>
                                        <td width="100%" class="help">(Enter full email address(es), separated by &quot;;&quot; - Example: GM2768P@pnc.com)</td>
                                    </tr>
                                </table>
                                <br />
                                <table width="100%" border="0" cellpadding="4" cellspacing="3">
                                    <tr>
                                        <td><hr size="1" noshade /></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Panel ID="panStep2Next" runat="server" Visible="false">
                                                <asp:Button ID="btnBack2" runat="server" CssClass="default" Width="100" Text="<<  Back" OnClick="btnBack2_Click" /> 
                                                <asp:Button ID="btnNext2" runat="server" CssClass="default" Width="100" Text="Next  >>" OnClick="btnNext2_Click" /> 
                                            </asp:Panel>
                                            <asp:Panel ID="panStep2Update" runat="server" Visible="false">
                                                <asp:Button ID="btnUpdate2" runat="server" CssClass="default" Width="100" Text="Update" OnClick="btnUpdate2_Click" /> 
                                                <asp:Button ID="btnCancel2" runat="server" CssClass="default" Width="100" Text="Cancel" OnClick="btnCancel_Click" /> 
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:Panel ID="panStep3" runat="server" Visible="false">
                                <asp:Panel ID="panTasksOn" runat="server" Visible="false">
                                    <table width="100%" border="0" cellpadding="4" cellspacing="3">
                                        <tr>
                                            <td nowrap class="biggerbold">Service Name:</td>
                                            <td width="100%" class="header">&quot;<asp:Label ID="lblName31" runat="server" CssClass="header" />&quot;</td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">Please reference the &quot;Icon Quck Reference&quot; and add all tasks related to this service...</td>
                                        </tr>
                                    </table>
                                    <table width="100%" border="0" cellpadding="4" cellspacing="3">
                                        <tr>
                                            <td width="50%" valign="top">
                                                <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                                    <tr>
                                                        <td>
                                                            <asp:TreeView ID="oTree" runat="server" CssClass="default" ShowLines="true" NodeIndent="30" ShowExpandCollapse="false">
                                                            </asp:TreeView>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>&nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td><b>Total Number of Hours:</b> <asp:Label ID="lblTotal" runat="server" CssClass="default" /> HRs</td>
                                                    </tr>
                                                    <tr>
                                                        <td>&nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td><asp:Button ID="btnOrder" runat="server" CssClass="default" Text="Change Order" Width="100" /></td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td width="50%" valign="top">
                                                <table border="0" cellpadding="3" cellspacing="2" style="border:solid 1px #999999" bgcolor="#f9f9f9">
                                                    <tr>
                                                        <td class="header" colspan="2">Icon Quck Reference</td>
                                                    </tr>
                                                    <tr>
                                                        <td><img src="/images/check.gif" border="0" align="absmiddle" /></td>
                                                        <td>= A task that is currently enabled</td>
                                                    </tr>
                                                    <tr>
                                                        <td><img src="/images/cancel.gif" border="0" align="absmiddle" /></td>
                                                        <td>= A task that is currently disabled</td>
                                                    </tr>
                                                    <tr>
                                                        <td><img src="/images/postit.gif" border="0" align="absmiddle" /></td>
                                                        <td>= Create a new task</td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <asp:Panel ID="panTasksOff" runat="server" Visible="false">
                                    <table width="100%" border="0" cellpadding="4" cellspacing="3">
                                        <tr>
                                            <td nowrap class="biggerbold">Service Name:</td>
                                            <td width="100%" class="header">&quot;<asp:Label ID="lblName32" runat="server" CssClass="header" />&quot;</td>
                                        </tr>
                                    </table>
                                    <table width="100%" border="0" cellpadding="4" cellspacing="3">
                                        <tr>
                                            <td colspan="2">Please enter the total number of hours allocated for this service:</td>
                                        </tr>
                                        <tr>
                                            <td nowrap><img src="/images/spacer.gif" border="0" width="<%=strIndent %>" height="1" /></td>
                                            <td width="100%"><asp:TextBox ID="txtHours" runat="server" CssClass="default" Width="80" MaxLength="8" /> HRs</td>
                                        </tr>
                                        <tr>
                                            <td nowrap><img src="/images/spacer.gif" border="0" width="<%=strIndent %>" height="1" /></td>
                                            <td width="100%" class="help">(How long does this service actually take to complete?)</td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">Do you want to enable fast-completion in the workload manager for this service?</td>
                                        </tr>
                                        <tr>
                                            <td nowrap><img src="/images/spacer.gif" border="0" width="<%=strIndent %>" height="1" /></td>
                                            <td width="100%">
                                                <asp:RadioButton ID="radNoSliderYes" runat="server" Text="Yes - enable fast-completion" CssClass="default" GroupName="SLIDER" /><br />
                                                <asp:RadioButton ID="radNoSliderNo" runat="server" Text="No - disable fast-completion&nbsp;&nbsp;<i>[default]</i>" CssClass="default" GroupName="SLIDER" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td nowrap><img src="/images/spacer.gif" border="0" width="<%=strIndent %>" height="1" /></td>
                                            <td width="100%" class="help">(Fast-completion removes the slider and enables the COMPLETE button immediately)</td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <br />
                                <table width="100%" border="0" cellpadding="4" cellspacing="3">
                                    <tr>
                                        <td colspan="2">Please enter your Service Level Agreement (SLA) for this service:</td>
                                    </tr>
                                    <tr>
                                        <td nowrap><img src="/images/spacer.gif" border="0" width="<%=strIndent %>" height="1" /></td>
                                        <td width="100%"><asp:TextBox ID="txtSLA" runat="server" CssClass="default" Width="80" MaxLength="8" /> HRs (8 HRs = 1 business day)</td>
                                    </tr>
                                    <tr>
                                        <td nowrap><img src="/images/spacer.gif" border="0" width="<%=strIndent %>" height="1" /></td>
                                        <td width="100%" class="help">(How long do you want the client to have to wait?)</td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">Do you want to hide the SLA from the client when submitting this service request?</td>
                                    </tr>
                                    <tr>
                                        <td nowrap><img src="/images/spacer.gif" border="0" width="<%=strIndent %>" height="1" /></td>
                                        <td width="100%">
                                            <asp:RadioButton ID="radSLAHideYes" runat="server" Text="Yes - hide the SLA" CssClass="default" GroupName="SLA" /><br />
                                            <asp:RadioButton ID="radSLAHideNo" runat="server" Text="No - do NOT hide the SLA&nbsp;&nbsp;<i>[default]</i>" CssClass="default" GroupName="SLA" />
                                        </td>
                                    </tr>
                                </table>
                                <br />
                                <table width="100%" border="0" cellpadding="4" cellspacing="3">
                                    <tr>
                                        <td><hr size="1" noshade /></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Panel ID="panStep3Next" runat="server" Visible="false">
                                                <asp:Button ID="btnBack3" runat="server" CssClass="default" Width="100" Text="<<  Back" OnClick="btnBack3_Click" /> 
                                                <asp:Button ID="btnNext3" runat="server" CssClass="default" Width="100" Text="Next  >>" OnClick="btnNext3_Click" /> 
                                            </asp:Panel>
                                            <asp:Panel ID="panStep3Update" runat="server" Visible="false">
                                                <asp:Button ID="btnUpdate3" runat="server" CssClass="default" Width="100" Text="Update" OnClick="btnUpdate3_Click" /> 
                                                <asp:Button ID="btnCancel3" runat="server" CssClass="default" Width="100" Text="Cancel" OnClick="btnCancel_Click" /> 
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:Panel ID="panStep4" runat="server" Visible="false">
                                <table width="100%" border="0" cellpadding="4" cellspacing="3">
                                    <tr>
                                        <td nowrap class="biggerbold">Service Name:</td>
                                        <td width="100%" class="header">&quot;<asp:Label ID="lblName4" runat="server" CssClass="header" />&quot;</td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">Please select where you want this service to be located in the service browser...</td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:TreeView ID="treLocation" runat="server" CssClass="default" ShowLines="true" NodeIndent="30" ShowCheckBoxes="All">
                                            </asp:TreeView>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2"><b>NOTE:</b> If you are unsure where to put your service, DO NOT select a location and a ClearView administrator will place your service in the appropriate place.</td>
                                    </tr>
                                    <tr>
                                        <td colspan="2"><b>Workflow Users:</b> Even if your service is part of a workflow and will not be selectable by a requestor (since it will be initiated from another service), it is important that you place the service in the most appropriate location.</td>
                                    </tr>
                                </table>
                                <br />
                                <table width="100%" border="0" cellpadding="4" cellspacing="3">
                                    <tr>
                                        <td><hr size="1" noshade /></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Panel ID="panStep4Next" runat="server" Visible="false">
                                                <asp:Button ID="btnBack4" runat="server" CssClass="default" Width="100" Text="<<  Back" OnClick="btnBack4_Click" /> 
                                                <asp:Button ID="btnNext4" runat="server" CssClass="default" Width="100" Text="Next  >>" OnClick="btnNext4_Click" /> 
                                            </asp:Panel>
                                            <asp:Panel ID="panStep4Update" runat="server" Visible="false">
                                                <asp:Button ID="btnUpdate4" runat="server" CssClass="default" Width="100" Text="Update" OnClick="btnUpdate4_Click" /> 
                                                <asp:Button ID="btnCancel4" runat="server" CssClass="default" Width="100" Text="Cancel" OnClick="btnCancel_Click" /> 
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:Panel ID="panStep5" runat="server" Visible="false">
                                <table width="100%" border="0" cellpadding="4" cellspacing="3">
                                    <tr>
                                        <td nowrap class="biggerbold">Service Name:</td>
                                        <td width="100%" class="header">&quot;<asp:Label ID="lblName5" runat="server" CssClass="header" />&quot;</td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">Do you want this service to be available to everyone or would you like access to be restricted?</td>
                                    </tr>
                                    <tr>
                                        <td nowrap><img src="/images/spacer.gif" border="0" width="<%=strIndent %>" height="1" /></td>
                                        <td width="100%">
                                            <asp:RadioButton ID="radAccessEveryone" runat="server" Text="Allow anyone to request this service&nbsp;&nbsp;<i>[default]</i>" CssClass="default" GroupName="radAccess" /><br />
                                            <asp:RadioButton ID="radAccessRestricted" runat="server" Text="Only allow certain people to request this service" CssClass="default" GroupName="radAccess" />
                                        </td>
                                    </tr>
                                </table>
                                <div id="divAccessRestricted" runat="server" style="display:none">
                                    <table width="100%" border="0" cellpadding="4" cellspacing="3">
                                        <tr>
                                            <td rowspan="3" nowrap><img src="/images/spacer.gif" border="0" width="<%=strIndent %>" height="1" /></td>
                                            <td colspan="2">Please enter the first name, last name, or LAN ID of the person(s) you want to be able to request this service</td>
                                        </tr>
                                        <tr>
                                            <td nowrap>Requestor:</td>
                                            <td width="100%">
                                                <table cellpadding="0" cellspacing="0" border="0">
                                                    <tr>
                                                        <td><asp:TextBox ID="txtAccess" runat="server" Width="300" CssClass="default" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <div id="divAccess" runat="server" style="overflow:hidden; position:absolute; display:none; background-color:#FFFFFF; border:solid 1px #CCCCCC">
                                                                <asp:ListBox ID="lstAccess" runat="server" CssClass="default" />
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td nowrap>&nbsp;</td>
                                            <td width="100%"><asp:Button ID="btnAccess" runat="server" CssClass="default" Width="100" Text="Add Requestor" OnClick="btnAccess_Click" /></td>
                                        </tr>
                                    </table>
                                    <br />
                                    <table width="100%" border="0" cellpadding="4" cellspacing="3">
                                        <tr>
                                            <td nowrap><img src="/images/spacer.gif" border="0" width="<%=strIndent %>" height="1" /></td>
                                            <td width="100%">
                                                <table width="600" cellpadding="4" cellspacing="0" border="0" style="border:solid 1px #CCCCCC">
                                                    <tr bgcolor="#EEEEEE">
                                                        <td nowrap><b>Requestor Name</b></td>
                                                        <td nowrap>&nbsp;</td>
                                                    </tr>
                                                    <asp:repeater ID="rptAccess" runat="server">
                                                        <ItemTemplate>
                                                            <tr class="default">
                                                                <td width="100%"><%# DataBinder.Eval(Container.DataItem, "username") %></td>
                                                                <td align="right"><asp:LinkButton ID="btnDeleteR" runat="server" OnClick="btnDeleteR_Click" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' Text="Delete" /></td>
                                                            </tr>
                                                        </ItemTemplate>
                                                    </asp:repeater>
                                                    <tr>
                                                        <td colspan="2">
                                                            <asp:Label ID="lblAccess" runat="server" CssClass="default" Visible="false" Text="<img src='/images/alert.gif' border='0' align='absmiddle'> There are no requestors" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <br />
                                <table width="100%" border="0" cellpadding="4" cellspacing="3">
                                    <tr>
                                        <td><hr size="1" noshade /></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Panel ID="panStep5Next" runat="server" Visible="false">
                                                <asp:Button ID="btnBack5" runat="server" CssClass="default" Width="100" Text="<<  Back" OnClick="btnBack5_Click" /> 
                                                <asp:Button ID="btnNext5" runat="server" CssClass="default" Width="100" Text="Next  >>" OnClick="btnNext5_Click" /> 
                                            </asp:Panel>
                                            <asp:Panel ID="panStep5Update" runat="server" Visible="false">
                                                <asp:Button ID="btnUpdate5" runat="server" CssClass="default" Width="100" Text="Update" OnClick="btnUpdate5_Click" /> 
                                                <asp:Button ID="btnCancel5" runat="server" CssClass="default" Width="100" Text="Cancel" OnClick="btnCancel_Click" /> 
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:Panel ID="panStep6" runat="server" Visible="false">
                                <table width="100%" border="0" cellpadding="4" cellspacing="3">
                                    <tr>
                                        <td nowrap class="biggerbold">Service Name:</td>
                                        <td width="100%" class="header">&quot;<asp:Label ID="lblName6" runat="server" CssClass="header" />&quot;</td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" align="center">
                                            <asp:Panel ID="panFormNo" runat="server" Visible="false">
                                                <table width="100%" cellpadding="0" cellspacing="5" border="0">
                                                    <tr>
                                                        <td rowspan="2"><img src="/images/lock.gif" border="0" align="absmiddle" /></td>
                                                        <td class="header" width="100%" valign="bottom">Form Customization Disabled</td>
                                                    </tr>
                                                    <tr>
                                                        <td width="100%" valign="top">The customization of the service request and workload manager forms have been disabled by a ClearView administrator.</td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                            <asp:Panel ID="panFormYes" runat="server" Visible="false">
                                                <table width="100%" border="0" cellpadding="4" cellspacing="3">
                                                    <tr>
                                                        <td colspan="2">Will this service inherit information from a workflow or another service?</td>
                                                    </tr>
                                                    <tr>
                                                        <td nowrap><img src="/images/spacer.gif" border="0" width="<%=strIndent %>" height="1" /></td>
                                                        <td width="100%">
                                                            <asp:RadioButton ID="radWorkflowYes" runat="server" Text="Yes, this service will inherit information" CssClass="default" GroupName="WORKFLOW" /><br />
                                                            <asp:RadioButton ID="radWorkflowNo" runat="server" Text="No, this service will gather information directly from the requestor" CssClass="default" GroupName="WORKFLOW" />
                                                        </td>
                                                    </tr>
                                                </table>
                                                <br />
                                                <div id="divWorkflowNo" runat="server" style="display:none">
                                                    <table width="100%" border="0" cellpadding="4" cellspacing="3">
                                                        <tr>
                                                            <td colspan="2">If this service is the beginning of a workflow, what do you want the overall workflow to be called?</td>
                                                        </tr>
                                                        <tr>
                                                            <td nowrap><img src="/images/spacer.gif" border="0" width="<%=strIndent %>" height="1" /></td>
                                                            <td width="100%"><asp:TextBox ID="txtWorkflowTitle" runat="server" CssClass="default" Width="400" MaxLength="100" /></td>
                                                        </tr>
                                                    </table>
                                                    <br />
                                                    <%=strMenuTab2 %>
                                                    <div id="divMenu2">
                                                        <div style="display:none">
                                                            <br />
                                                            <table width="100%" border="0" cellpadding="4" cellspacing="3">
                                                                <tr>
                                                                    <td colspan="2"><b>Service Request</b> form controls are shown to the requestor during the checkout process. They should be used to gather all necessary information for completing this service request.</td>
                                                                </tr>
                                                                <tr>
                                                                    <td><asp:Button ID="btnControl" runat="server" CssClass="default" Width="100" Text="Add Control" /></td>
                                                                    <td align="right">
                                                                        <img src="/images/move.gif" border="0" align="absmiddle" /> = Drag to Move the Control&nbsp;&nbsp;&nbsp;&nbsp;
                                                                        <img src="/images/edit.gif" border="0" align="absmiddle" /> = Edit the Control&nbsp;&nbsp;&nbsp;&nbsp;
                                                                        <img src="/images/eye.gif" border="0" align="absmiddle" /> = Configure Display Options
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="2" style="border:outset 2px #FFFFFF">
                                                                        <br />
                                                                        <%=strForm %>
                                                                        <br />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                        <div style="display:none">
                                                            <br />
                                                            <table width="100%" border="0" cellpadding="4" cellspacing="3">
                                                                <tr>
                                                                    <td colspan="2" style="border:outset 2px #FFFFFF">
                                                                        <table width="100%" cellpadding="0" cellspacing="5" border="0">
                                                                            <tr>
                                                                                <td rowspan="2"><img src="/images/service_request_small.gif" border="0" align="absmiddle" /></td>
                                                                                <td class="header" width="100%" valign="bottom"><asp:Label ID="lblHeader" runat="server" CssClass="header" /></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td width="100%" valign="top"><asp:Label ID="lblHeaderSub" runat="server" CssClass="default" Text="Please complete the following information to request this service." /></td>
                                                                            </tr>
                                                                        </table>
                                                                        <table width="100%" border="0" cellSpacing="2" cellPadding="2" class="default" bgcolor="#F6F6F6">
                                                                            <asp:Panel ID="panTitle" runat="server" Visible="false">
                                                                            <tr>
                                                                                <td colspan="2"><asp:Label ID="lblTitleName" runat="server" CssClass="default" Text="Please enter a title for this request" />:<span class="required">&nbsp;*</span></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td><img src="/images/spacer.gif" border="0" width="50" height="1"/></td>
                                                                                <td width="100%"><asp:TextBox ID="txtTitle" runat="server" CssClass="default" MaxLength="100" Width="500" /></td>
                                                                            </tr>
                                                                            </asp:Panel>
                                                                            <asp:Panel ID="panHide" runat="server" Visible="false">
                                                                                <tr>
                                                                                    <td colspan="2"><img src="/images/spacer.gif" border="0" width="1" height="5" /></td>
                                                                                </tr>
                                                                                <asp:Panel ID="panStatement" runat="server" Visible="false">
                                                                                <tr>
                                                                                    <td colspan="2">Please enter your statement of work:<span class="required">&nbsp;*</span></td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td><img src="/images/spacer.gif" border="0" width="50" height="1"/></td>
                                                                                    <td width="100%"><asp:TextBox ID="txtStatement" runat="server" CssClass="default" TextMode="MultiLine" Rows="8" Width="80%" /></td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td colspan="2"><img src="/images/spacer.gif" border="0" width="1" height="5" /></td>
                                                                                </tr>
                                                                                </asp:Panel>
                                                                                <tr>
                                                                                    <td colspan="2">Please select your priority:<span class="required">&nbsp;*</span></td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td><img src="/images/spacer.gif" border="0" width="50" height="1"/></td>
                                                                                    <td width="100%">
                                                                                        <asp:RadioButtonList ID="radPriority" runat="server" CssClass="default" RepeatDirection="Horizontal">
                                                                                            <asp:ListItem Value="1" />
                                                                                            <asp:ListItem Value="2" />
                                                                                            <asp:ListItem Value="3" />
                                                                                            <asp:ListItem Value="4" />
                                                                                            <asp:ListItem Value="5" />
                                                                                        </asp:RadioButtonList>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="footer">Highest&nbsp;&nbsp;<img src="/images/small_arrow_right.gif" border="0" align="absmiddle" />&nbsp;&nbsp;Lowest</span>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td colspan="2"><img src="/images/spacer.gif" border="0" width="1" height="5" /></td>
                                                                                </tr>
                                                                                <asp:Panel ID="panDeliverable" runat="server" Visible="false">
                                                                                <tr>
                                                                                    <td colspan="2">Based on your priority, the service level agreement is:</td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td><img src="/images/spacer.gif" border="0" width="50" height="1"/></td>
                                                                                    <td width="100%"><b><asp:Label ID="lblDeliverable" runat="server" CssClass="bold" /> days</b></td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td colspan="2"><img src="/images/spacer.gif" border="0" width="1" height="5" /></td>
                                                                                </tr>
                                                                                </asp:Panel>
                                                                            </asp:Panel>
                                                                        </table>
                                                                        <asp:Panel ID="panForm" runat="server" CssClass="default">
                                                                            <%=strFormPreview%>
                                                                        </asp:Panel>
                                                                        <table width="100%" border="0" cellSpacing="2" cellPadding="2" class="default" bgcolor="#F6F6F6">
                                                                            <asp:Panel ID="panExpedite" runat="server" Visible="false">
                                                                            <tr>
                                                                                <td colspan="2">Please select if you would like to expedite this request:<span class="required">&nbsp;*</span></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td><img src="/images/spacer.gif" border="0" width="50" height="1"/></td>
                                                                                <td width="100%">
                                                                                    <asp:RadioButton ID="RadioButton1" runat="server" Text="Yes" CssClass="default" GroupName="expedite" />&nbsp;
                                                                                    <asp:RadioButton ID="RadioButton2" runat="server" Text="No" CssClass="default" GroupName="expedite" />
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td colspan="2"><img src="/images/spacer.gif" border="0" width="1" height="5" /></td>
                                                                            </tr>
                                                                            </asp:Panel>
                                                                            <asp:Panel ID="panUpload" runat="server" Visible="false">
                                                                            <tr>
                                                                                <td colspan="2">Click the following button to upload and articles / documentation associated with this request:</td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td><img src="/images/spacer.gif" border="0" width="50" height="1"/></td>
                                                                                <td width="100%"><asp:Button ID="btnDocuments" runat="server" Text="Click to Upload" Width="125" CssClass="default" /></td>
                                                                            </tr>
                                                                            </asp:Panel>
                                                                        </table>
                                                                        <table width="100%" border="0" cellSpacing="2" cellPadding="4" class="default">
                                                                            <tr>
                                                                                <td colspan="2"><hr size="1" noshade /></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:Button ID="btnBack" runat="server" CssClass="default" Text="<<  Back" Width="100" Enabled="false" /> 
                                                                                    <asp:Button ID="btnNext" runat="server" CssClass="default" Text="Next  >>" Width="100" Enabled="false" /> 
                                                                                </td>
                                                                                <td align="right">
                                                                                    <asp:Button ID="btnCancelR" runat="server" CssClass="default" Text="Cancel Request" Width="125" Enabled="false" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                        <div style="display:none">
                                                            <br />
                                                            <table width="100%" border="0" cellpadding="4" cellspacing="3">
                                                                <tr>
                                                                    <td colspan="2"><b>Workload Manager</b> form controls are shown <u>only</u> to the assigned resource while completing the task. They should be used to gather any information needed for subsequent workflows or for archiving information related to the request.</td>
                                                                </tr>
                                                                <tr>
                                                                    <td><asp:Button ID="btnControlWM" runat="server" CssClass="default" Width="100" Text="Add Control" /></td>
                                                                    <td align="right">
                                                                        <img src="/images/move.gif" border="0" align="absmiddle" /> = Drag to Move the Control&nbsp;&nbsp;&nbsp;&nbsp;
                                                                        <img src="/images/edit.gif" border="0" align="absmiddle" /> = Edit the Control&nbsp;&nbsp;&nbsp;&nbsp;
                                                                        <img src="/images/eye.gif" border="0" align="absmiddle" /> = Configure Display Options
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="2" style="border:outset 2px #FFFFFF">
                                                                        <table width="100%" cellpadding="4" cellspacing="3" border="0">
                                                                            <tr>
                                                                                <td colspan="2" class="greentableheader">Required Information</td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td colspan="2"><%=strFormWM %></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td colspan="2">&nbsp;</td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td colspan="2" class="greentableheader">Required Tasks</td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td colspan="2"><%=strCheckboxes %></td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div id="divWorkflowYes" runat="server" style="display:none">
                                                    <%=strMenuTab3 %>
                                                    <div id="divMenu3">
                                                        <div style="display:none">
                                                            <br />
                                                            <table width="100%" cellpadding="0" cellspacing="5" border="0">
                                                                <tr>
                                                                    <td rowspan="2"><img src="/images/hugeAlert.gif" border="0" align="absmiddle" /></td>
                                                                    <td class="header" width="100%" valign="bottom">Service Request Form Not Available</td>
                                                                </tr>
                                                                <tr>
                                                                    <td width="100%" valign="top">When inheritng information from another service, there is no service request form since the service is part of a workflow.</td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                        <div style="display:none">
                                                            <br />
                                                            <table width="100%" border="0" cellpadding="4" cellspacing="3">
                                                                <tr>
                                                                    <td colspan="2" style="border:outset 2px #FFFFFF">
                                                                        <br />
                                                                        <%=strForm %>
                                                                        <br />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                        <div style="display:none">
                                                            <br />
                                                            <table width="100%" border="0" cellpadding="4" cellspacing="3">
                                                                <tr>
                                                                    <td><asp:Button ID="btnControlWM2" runat="server" CssClass="default" Width="100" Text="Add Control" /></td>
                                                                    <td align="right">
                                                                        <img src="/images/move.gif" border="0" align="absmiddle" /> = Click and Drag to Move the Control&nbsp;&nbsp;&nbsp;&nbsp;
                                                                        <img src="/images/edit.gif" border="0" align="absmiddle" /> = Click to Edit the Control&nbsp;&nbsp;&nbsp;&nbsp;
                                                                        <img src="/images/eye.gif" border="0" align="absmiddle" /> = Click to Configure Display Options
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="2" style="border:outset 2px #FFFFFF">
                                                                        <table width="100%" cellpadding="4" cellspacing="3" border="0">
                                                                            <tr>
                                                                                <td colspan="2" class="greentableheader">Required Information</td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td colspan="2"><%=strFormWM %></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td colspan="2">&nbsp;</td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td colspan="2" class="greentableheader">Required Tasks</td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td colspan="2"><%=strCheckboxes %></td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="2"><b>Workload Manager</b> form controls are shown <u>only</u> to the assigned resource while completing the task. They should be used to gather any information needed for subsequent workflows or for archiving information related to the request.</td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                    </div>
                                                </div>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                </table>
                                <br />
                                <table width="100%" border="0" cellpadding="4" cellspacing="3">
                                    <tr>
                                        <td><hr size="1" noshade /></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Panel ID="panStep6Next" runat="server" Visible="false">
                                                <asp:Button ID="btnBack6" runat="server" CssClass="default" Width="100" Text="<<  Back" OnClick="btnBack6_Click" /> 
                                                <asp:Button ID="btnNext6" runat="server" CssClass="default" Width="100" Text="Next >>" OnClick="btnNext6_Click" /> 
                                            </asp:Panel>
                                            <asp:Panel ID="panStep6Update" runat="server" Visible="false">
                                                <asp:Button ID="btnUpdate6" runat="server" CssClass="default" Width="100" Text="Update" OnClick="btnUpdate6_Click" /> 
                                                <asp:Button ID="btnCancel6" runat="server" CssClass="default" Width="100" Text="Cancel" OnClick="btnCancel_Click" /> 
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:Panel ID="panStep7" runat="server" Visible="false">
                                <table width="100%" border="0" cellpadding="4" cellspacing="3">
                                    <tr>
                                        <td nowrap class="biggerbold">Service Name:</td>
                                        <td width="100%" class="header">&quot;<asp:Label ID="lblName7" runat="server" CssClass="header" />&quot;</td>
                                    </tr>
                                </table>
                                <asp:Panel id="panWorkflow" runat="server" Visible="false">
                                    <table width="100%" border="0" cellpadding="4" cellspacing="3">
                                        <tr>
                                            <td colspan="2" class="box_green header"><img src="/images/ico_check.gif" border="0" align="absmiddle" /> Workflow Enabled</td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" class="biggerbold">This service receives workflow requests (incoming) from the following services...</td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <table width="750" cellpadding="5" cellspacing="0" border="0" style="border:solid 1px #CCCCCC">
                                                    <tr bgcolor="#EEEEEE">
                                                       <td nowrap></td>
                                                       <td nowrap><b>Receive From</b></td>
                                                       <td nowrap><b>Assignment</b></td>
                                                       <td nowrap></td>
                                                    </tr>
                                                    <asp:repeater ID="rptWorkflowsReceive" runat="server">
                                                        <ItemTemplate>
                                                           <tr>
                                                              <td nowrap valign="top">[<asp:LinkButton ID="btnWorkflowFields" runat="server" Text="Map Fields" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' />]</td>
                                                              <td width="50%" valign="top"><%# (oUser.IsAdmin(intProfile) ? "<a href=\"" + oPage.GetFullLink(intPage) + "?sid=" + DataBinder.Eval(Container.DataItem, "serviceid").ToString() + "&menu_tab=3&edit=7\">" + DataBinder.Eval(Container.DataItem, "service").ToString() + "</a>" : DataBinder.Eval(Container.DataItem, "service").ToString())%></td>
                                                              <td rowspan="2" width="50%" valign="top"><asp:Label ID="lblAssignment" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "serviceid") %>' /></td>
                                                              <td rowspan="2" nowrap valign="top">[<asp:LinkButton ID="btnWorkflowDelete" runat="server" Text="Remove" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' OnClick="btnWorkflowDelete_Click" />]</td>
                                                           </tr>
                                                           <tr>
                                                              <td nowrap valign="top">[<asp:LinkButton ID="btnWorkflowConditions" runat="server" Text="Conditions" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' />]</td>
                                                              <td width="50%" valign="top" align="right"><div style="display:none">...generated <%# (DataBinder.Eval(Container.DataItem, "sametime").ToString() == "0" ? "<a href=\"javascript:void(0);\" onclick=\"alert('Individual generation means that this service will be initiated immediately upon completion of the previous service');\">individually</a>" : "<a href=\"javascript:void(0);\" onclick=\"alert('Collective generation means that this service will not be initiated until ALL of the previous workflow services are completed');\">collectively</a>")%></div></td>
                                                           </tr>
                                                        </ItemTemplate>
                                                        <AlternatingItemTemplate>
                                                           <tr bgcolor="#F6F6F6">
                                                              <td nowrap valign="top">[<asp:LinkButton ID="btnWorkflowFields" runat="server" Text="Map Fields" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' />]</td>
                                                              <td width="50%" valign="top"><%# (oUser.IsAdmin(intProfile) ? "<a href=\"" + oPage.GetFullLink(intPage) + "?sid=" + DataBinder.Eval(Container.DataItem, "serviceid").ToString() + "&menu_tab=3&edit=7\">" + DataBinder.Eval(Container.DataItem, "service").ToString() + "</a>" : DataBinder.Eval(Container.DataItem, "service").ToString())%></td>
                                                              <td rowspan="2" width="50%" valign="top"><asp:Label ID="lblAssignment" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "serviceid") %>' /></td>
                                                              <td rowspan="2" nowrap valign="top">[<asp:LinkButton ID="btnWorkflowDelete" runat="server" Text="Remove" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' OnClick="btnWorkflowDelete_Click" />]</td>
                                                           </tr>
                                                           <tr bgcolor="#F6F6F6">
                                                              <td nowrap valign="top">[<asp:LinkButton ID="btnWorkflowConditions" runat="server" Text="Conditions" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' />]</td>
                                                              <td width="50%" valign="top" align="right"><div style="display:none">...generated <%# (DataBinder.Eval(Container.DataItem, "sametime").ToString() == "0" ? "<a href=\"javascript:void(0);\" onclick=\"alert('Individual generation means that this service will be initiated immediately upon completion of the previous service');\">individually</a>" : "<a href=\"javascript:void(0);\" onclick=\"alert('Collective generation means that this service will not be initiated until ALL of the previous workflow services are completed');\">collectively</a>")%></div></td>
                                                           </tr>
                                                        </AlternatingItemTemplate>
                                                    </asp:repeater>
                                                    <tr>
                                                       <td colspan="10">
                                                          <asp:Label ID="lblWorkflowsReceive" runat="server" CssClass="default" Visible="false" Text="<img src='/images/bigAlert.gif' border='0' align='absmiddle'> This service has no incoming workflows..." />
                                                       </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2"><asp:Button ID="btnWorkflow" runat="server" CssClass="default" Text="Add Workflow" Width="125" /></td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">Would you like to be able to assign the same person to multiple services in this workflow?</td>
                                        </tr>
                                        <tr>
                                            <td nowrap><img src="/images/spacer.gif" border="0" width="<%=strIndent %>" height="1" /></td>
                                            <td width="100%">
                                                <asp:RadioButton ID="radWorkflowSameYes" runat="server" Text="Allow the same person to be assigned to multiple services&nbsp;&nbsp;<i>[default]</i>" CssClass="default" GroupName="radWorkflowSame" /><br />
                                                <asp:RadioButton ID="radWorkflowSameNo" runat="server" Text="Prevent the same person from being assigned to multiple services" CssClass="default" GroupName="radWorkflowSame" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">&nbsp;</td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <table width="100%" border="0" cellpadding="4" cellspacing="3">
                                    <tr>
                                        <td colspan="2">Do you want to allow others to connect a workflow to this service?</td>
                                    </tr>
                                    <tr>
                                        <td nowrap><img src="/images/spacer.gif" border="0" width="<%=strIndent %>" height="1" /></td>
                                        <td width="100%">
                                            <asp:RadioButton ID="radWorkflowConnectYes" runat="server" Text="Allow others to connect to this service" CssClass="default" GroupName="radWorkflowConnect" /><br />
                                            <asp:RadioButton ID="radWorkflowConnectNo" runat="server" Text="Prevent others from connecting to this service" CssClass="default" GroupName="radWorkflowConnect" />
                                        </td>
                                    </tr>
                                </table>
                                <table width="100%" border="0" cellpadding="4" cellspacing="3">
                                    <tr>
                                        <td colspan="2">&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" class="biggerbold">Upon completion of this service, ClearView will send a workflow request (outgoing) to each of the following services...</td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <table width="750" cellpadding="5" cellspacing="0" border="0" style="border:solid 1px #CCCCCC">
                                                <tr bgcolor="#EEEEEE">
                                                   <td nowrap><b>Send To</b></td>
                                                   <td nowrap><b>Assignment</b></td>
                                                   <td nowrap><b>Sequence</b></td>
                                                </tr>
                                                <asp:repeater ID="rptWorkflow" runat="server">
                                                    <ItemTemplate>
                                                       <tr>
                                                          <td width="50%" valign="top"><%# (oUser.IsAdmin(intProfile) ? "<a href=\"" + oPage.GetFullLink(intPage) + "?sid=" + DataBinder.Eval(Container.DataItem, "nextservice").ToString() + "&menu_tab=3&edit=7\">" + DataBinder.Eval(Container.DataItem, "service").ToString() + "</a>" : DataBinder.Eval(Container.DataItem, "service").ToString())%></td>
                                                          <td width="50%" valign="top"><asp:Label ID="lblAssignment" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "nextservice") %>' /></td>
                                                          <td nowrap valign="top"><%# DataBinder.Eval(Container.DataItem, "display") %></td>
                                                       </tr>
                                                    </ItemTemplate>
                                                </asp:repeater>
                                                <tr>
                                                   <td colspan="10">
                                                      <asp:Label ID="lblWorkflow" runat="server" CssClass="default" Visible="false" Text="" />
                                                   </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2"><asp:LinkButton ID="btnWorkflowPrint" runat="server" Text="View Workflow Flow Chart" /></td>
                                    </tr>
                                </table>
                                <asp:Panel ID="panWorkflowMultiple" runat="server" Visible="false">
                                    <table width="100%" border="0" cellpadding="4" cellspacing="3">
                                        <tr>
                                            <td colspan="2">&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">Since multiple workflows are being initiated, you need to decide what should happen when those services are completed?</td>
                                        </tr>
                                        <tr>
                                            <td nowrap><img src="/images/spacer.gif" border="0" width="<%=strIndent %>" height="1" /></td>
                                            <td width="100%">
                                                <asp:RadioButton ID="radSameTimeNo" runat="server" Text="Individual - each of the above services are completed individually and their subsequent workflows are initiated immediately" CssClass="default" GroupName="SAMETIME" /><br />
                                                <asp:RadioButton ID="radSameTimeYes" runat="server" Text="Collective - all of the above services must be completed before any of their subsequent workflows are initiated" CssClass="default" GroupName="SAMETIME" />
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <br />
                                <table width="100%" border="0" cellpadding="4" cellspacing="3">
                                    <tr>
                                        <td><hr size="1" noshade /></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Panel ID="panStep7Next" runat="server" Visible="false">
                                                <asp:Button ID="btnBack7" runat="server" CssClass="default" Width="100" Text="<<  Back" OnClick="btnBack7_Click" /> 
                                                <asp:Button ID="btnNext7" runat="server" CssClass="default" Width="100" Text="Next >>" OnClick="btnNext7_Click" /> 
                                            </asp:Panel>
                                            <asp:Panel ID="panStep7Update" runat="server" Visible="false">
                                                <asp:Button ID="btnUpdate7" runat="server" CssClass="default" Width="100" Text="Update" OnClick="btnUpdate7_Click" /> 
                                                <asp:Button ID="btnCancel7" runat="server" CssClass="default" Width="100" Text="Cancel" OnClick="btnCancel_Click" /> 
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:Panel ID="panStep8" runat="server" Visible="false">
                                <table width="100%" border="0" cellpadding="4" cellspacing="3">
                                    <tr>
                                        <td nowrap class="biggerbold">Service Name:</td>
                                        <td width="100%" class="header">&quot;<asp:Label ID="lblName8" runat="server" CssClass="header" />&quot;</td>
                                    </tr>
                                </table>
                                <table width="100%" border="0" cellpadding="4" cellspacing="3">
                                    <tr>
                                        <td colspan="2">When a client submits this service, do you want the request to be sent to a supervisor for assignment or would you like ClearView to automatically assign a resource?</td>
                                    </tr>
                                    <tr>
                                        <td nowrap><img src="/images/spacer.gif" border="0" width="<%=strIndent %>" height="1" /></td>
                                        <td width="100%">
                                            <asp:RadioButton ID="radAssignM" runat="server" Text="Send to a supervisor for assignment" CssClass="default" GroupName="ASSIGN" /><br />
                                            <asp:RadioButton ID="radAssignT" runat="server" Text="Automatically assign one or more resources" CssClass="default" GroupName="ASSIGN" /><br />
                                            <asp:RadioButton ID="radAssignW" runat="server" Text="Automatically assign a resource who was assigned from a previous workflow" CssClass="default" GroupName="ASSIGN" Enabled="false" />
                                        </td>
                                    </tr>
                                </table>
                                <br />
                                <div id="divAssignM" runat="server" style="display:none">
                                    <table width="100%" border="0" cellpadding="4" cellspacing="3">
                                        <tr>
                                            <td rowspan="3" nowrap><img src="/images/spacer.gif" border="0" width="<%=strIndent %>" height="1" /></td>
                                            <td colspan="2">Please enter the first name, last name, or LAN ID of the supervisor(s) you want to be in charge of assigning this service</td>
                                        </tr>
                                        <tr>
                                            <td nowrap>Supervisor:</td>
                                            <td width="100%">
                                                <table cellpadding="0" cellspacing="0" border="0">
                                                    <tr>
                                                        <td><asp:TextBox ID="txtAssignM" runat="server" Width="300" CssClass="default" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <div id="divAssignM2" runat="server" style="overflow:hidden; position:absolute; display:none; background-color:#FFFFFF; border:solid 1px #CCCCCC">
                                                                <asp:ListBox ID="lstAssignM" runat="server" CssClass="default" />
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td nowrap>&nbsp;</td>
                                            <td width="100%"><asp:Button ID="btnAssignM" runat="server" CssClass="default" Width="100" Text="Add Supervisor" OnClick="btnAssignM_Click" /></td>
                                        </tr>
                                    </table>
                                    <br />
                                    <table width="100%" border="0" cellpadding="4" cellspacing="3">
                                        <tr>
                                            <td nowrap><img src="/images/spacer.gif" border="0" width="<%=strIndent %>" height="1" /></td>
                                            <td width="100%">
                                                <table width="600" cellpadding="4" cellspacing="0" border="0" style="border:solid 1px #CCCCCC">
                                                    <tr bgcolor="#EEEEEE">
                                                        <td nowrap><b>Supervisor Name</b></td>
                                                        <td nowrap>&nbsp;</td>
                                                    </tr>
                                                    <asp:repeater ID="rptAssignM" runat="server">
                                                        <ItemTemplate>
                                                            <tr class="default">
                                                                <td width="100%"><%# DataBinder.Eval(Container.DataItem, "username") %></td>
                                                                <td align="right"><asp:LinkButton ID="btnDeleteM" runat="server" OnClick="btnDeleteM_Click" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' Text="Delete" /></td>
                                                            </tr>
                                                        </ItemTemplate>
                                                    </asp:repeater>
                                                    <tr>
                                                        <td colspan="2">
                                                            <asp:Label ID="lblAssignM" runat="server" CssClass="default" Visible="false" Text="<img src='/images/alert.gif' border='0' align='absmiddle'> There are no supervisors" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div id="divAssignT" runat="server" style="display:none">
                                    <table width="100%" border="0" cellpadding="4" cellspacing="3">
                                        <tr>
                                            <td rowspan="3" nowrap><img src="/images/spacer.gif" border="0" width="<%=strIndent %>" height="1" /></td>
                                            <td colspan="2">Please enter the first name, last name, or LAN ID of the resource(s) you want to assign this service</td>
                                        </tr>
                                        <tr>
                                            <td nowrap>Resource:</td>
                                            <td width="100%">
                                                <table cellpadding="0" cellspacing="0" border="0">
                                                    <tr>
                                                        <td><asp:TextBox ID="txtAssignT" runat="server" Width="300" CssClass="default" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <div id="divAssignT2" runat="server" style="overflow:hidden; position:absolute; display:none; background-color:#FFFFFF; border:solid 1px #CCCCCC">
                                                                <asp:ListBox ID="lstAssignT" runat="server" CssClass="default" />
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td nowrap>&nbsp;</td>
                                            <td width="100%"><asp:Button ID="btnAssignT" runat="server" CssClass="default" Width="100" Text="Add Resource" OnClick="btnAssignT_Click" /></td>
                                        </tr>
                                    </table>
                                    <br />
                                    <table width="100%" border="0" cellpadding="4" cellspacing="3">
                                        <tr>
                                            <td nowrap><img src="/images/spacer.gif" border="0" width="<%=strIndent %>" height="1" /></td>
                                            <td width="100%">
                                                <table width="600" cellpadding="4" cellspacing="0" border="0" style="border:solid 1px #CCCCCC">
                                                    <tr bgcolor="#EEEEEE">
                                                        <td nowrap><b>Resource Name</b></td>
                                                        <td nowrap>&nbsp;</td>
                                                    </tr>
                                                    <asp:repeater ID="rptAssignT" runat="server">
                                                        <ItemTemplate>
                                                            <tr class="default">
                                                                <td width="100%"><%# DataBinder.Eval(Container.DataItem, "username") %></td>
                                                                <td align="right"><asp:LinkButton ID="btnDeleteT" runat="server" OnClick="btnDeleteT_Click" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' Text="Delete" /></td>
                                                            </tr>
                                                        </ItemTemplate>
                                                    </asp:repeater>
                                                    <tr>
                                                        <td colspan="2">
                                                            <asp:Label ID="lblAssignT" runat="server" CssClass="default" Visible="false" Text="<img src='/images/alert.gif' border='0' align='absmiddle'> There are no resources" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div id="divNotify" runat="server" style="display:none">
                                    <br />
                                    <table width="100%" border="0" cellpadding="4" cellspacing="3">
                                        <tr>
                                            <td colspan="2">How do you want to receive notifications regarding new service requests?</td>
                                        </tr>
                                        <tr>
                                            <td nowrap><img src="/images/spacer.gif" border="0" width="<%=strIndent %>" height="1" /></td>
                                            <td width="100%">
                                                <asp:RadioButton ID="radNotifyMT" runat="server" Text="Notify the supervisor(s) / resource(s) when a new service is requested&nbsp;&nbsp;<i>[default]</i>" CssClass="default" GroupName="NOTIFY" /><br />
                                                <asp:RadioButton ID="radNotifyMBX" runat="server" Text="Notify a group mailbox when a new service is requested" CssClass="default" GroupName="NOTIFY" />
                                            </td>
                                        </tr>
                                    </table>
                                    <div id="divNotifyMBX" runat="server" style="display:none">
                                        <table width="100%" border="0" cellpadding="4" cellspacing="3">
                                            <tr>
                                                <td nowrap><img src="/images/spacer.gif" border="0" width="<%=strIndent %>" height="1" /></td>
                                                <td nowrap>Group Mailbox:</td>
                                                <td width="100%"><asp:TextBox ID="txtNotifyMBX" runat="server" CssClass="default" Width="300" MaxLength="50" /></td>
                                            </tr>
                                            <tr>
                                                <td nowrap><img src="/images/spacer.gif" border="0" width="<%=strIndent %>" height="1" /></td>
                                                <td nowrap></td>
                                                <td width="100%" class="help">(Enter the full email address - Example: GM2768P@pnc.com)</td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                                <div id="divAssignW" runat="server" style="display:none">
                                    <table width="100%" border="0" cellpadding="4" cellspacing="3">
                                        <tr>
                                            <td rowspan="3" nowrap><img src="/images/spacer.gif" border="0" width="<%=strIndent %>" height="1" /></td>
                                            <td colspan="2">Please select the previous service you want to ClearView to automatically assign:</td>
                                        </tr>
                                        <tr>
                                            <td nowrap>Previous Service:</td>
                                            <td width="100%"><asp:DropDownList ID="ddlWorkflowUser" runat="server" Width="300" /></td>
                                        </tr>
                                    </table>
                                </div>
                                <br />
                                <table width="100%" border="0" cellpadding="4" cellspacing="3">
                                    <tr>
                                        <td colspan="2" class="biggerbold">Service Owners</td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            Service owners are people who are able to make changes to this service in Service Editor.  They also have the ability to re-assign requests and make updates to requests that have already been assigned.
                                            <br /><br />
                                            <b>NOTE:</b> Service owners DO NOT have the ability to assign submitted service requests.
                                        </td>
                                    </tr>
                                </table>
                                <table width="100%" border="0" cellpadding="4" cellspacing="3">
                                    <tr>
                                        <td rowspan="3" nowrap><img src="/images/spacer.gif" border="0" width="<%=strIndent %>" height="1" /></td>
                                        <td colspan="2">Please enter the first name, last name, or LAN ID of the person(s) you want to be service owners</td>
                                    </tr>
                                    <tr>
                                        <td nowrap>Resource:</td>
                                        <td width="100%">
                                            <table cellpadding="0" cellspacing="0" border="0">
                                                <tr>
                                                    <td><asp:TextBox ID="txtOwner" runat="server" Width="300" CssClass="default" /></td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <div id="divOwner2" runat="server" style="overflow:hidden; position:absolute; display:none; background-color:#FFFFFF; border:solid 1px #CCCCCC">
                                                            <asp:ListBox ID="lstOwner" runat="server" CssClass="default" />
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td nowrap>&nbsp;</td>
                                        <td width="100%"><asp:Button ID="btnOwner" runat="server" CssClass="default" Width="100" Text="Add Owner" OnClick="btnOwner_Click" /></td>
                                    </tr>
                                </table>
                                <br />
                                <table width="100%" border="0" cellpadding="4" cellspacing="3">
                                    <tr>
                                        <td nowrap><img src="/images/spacer.gif" border="0" width="<%=strIndent %>" height="1" /></td>
                                        <td width="100%">
                                            <table width="600" cellpadding="4" cellspacing="0" border="0" style="border:solid 1px #CCCCCC">
                                                <tr bgcolor="#EEEEEE">
                                                    <td nowrap><b>Resource Name</b></td>
                                                    <td nowrap>&nbsp;</td>
                                                </tr>
                                                <asp:repeater ID="rptOwners" runat="server">
                                                    <ItemTemplate>
                                                        <tr class="default">
                                                            <td width="100%"><%# DataBinder.Eval(Container.DataItem, "username") %></td>
                                                            <td align="right"><asp:LinkButton ID="btnDeleteO" runat="server" OnClick="btnDeleteO_Click" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' CommandName='<%# DataBinder.Eval(Container.DataItem, "userid") %>' Text="Delete" /></td>
                                                        </tr>
                                                    </ItemTemplate>
                                                </asp:repeater>
                                                <tr>
                                                    <td colspan="2">
                                                        <asp:Label ID="lblOwner" runat="server" CssClass="default" Visible="false" Text="<img src='/images/alert.gif' border='0' align='absmiddle'> There are no service owners" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                                <br />
                                <table width="100%" border="0" cellpadding="4" cellspacing="3">
                                    <tr>
                                        <td colspan="2">&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" class="biggerbold">Approvers</td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">Will this service require the approval of one or more people?</td>
                                    </tr>
                                    <tr>
                                        <td nowrap><img src="/images/spacer.gif" border="0" width="<%=strIndent %>" height="1" /></td>
                                        <td width="100%">
                                            <asp:RadioButton ID="radApprovalYes" runat="server" Text="Yes" CssClass="default" GroupName="APPROVE" />&nbsp;
                                            <asp:RadioButton ID="radApprovalNo" runat="server" Text="No" CssClass="default" GroupName="APPROVE" />
                                        </td>
                                    </tr>
                                </table>
                                <br />
                                <div id="divApprovalYes" runat="server" style="display:none">
                                    <table width="100%" border="0" cellpadding="4" cellspacing="3">
                                        <tr>
                                            <td rowspan="3" nowrap><img src="/images/spacer.gif" border="0" width="<%=strIndent %>" height="1" /></td>
                                            <td colspan="2">Please enter the first name, last name, or LAN ID of the person(s) you want to be in charge of approving this service</td>
                                        </tr>
                                        <tr>
                                            <td nowrap>Approver:</td>
                                            <td width="100%">
                                                <table cellpadding="0" cellspacing="0" border="0">
                                                    <tr>
                                                        <td><asp:TextBox ID="txtApprove" runat="server" Width="300" CssClass="default" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <div id="divApprove" runat="server" style="overflow:hidden; position:absolute; display:none; background-color:#FFFFFF; border:solid 1px #CCCCCC">
                                                                <asp:ListBox ID="lstApprove" runat="server" CssClass="default" />
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td nowrap>&nbsp;</td>
                                            <td width="100%"><asp:Button ID="btnApprove" runat="server" CssClass="default" Width="100" Text="Add Approver" OnClick="btnApprove_Click" /></td>
                                        </tr>
                                    </table>
                                    <br />
                                    <table width="100%" border="0" cellpadding="4" cellspacing="3">
                                        <tr>
                                            <td nowrap><img src="/images/spacer.gif" border="0" width="<%=strIndent %>" height="1" /></td>
                                            <td width="100%">
                                                <table width="600" cellpadding="4" cellspacing="0" border="0" style="border:solid 1px #CCCCCC">
                                                    <tr bgcolor="#EEEEEE">
                                                        <td nowrap><b>Approver Name</b></td>
                                                        <td nowrap>&nbsp;</td>
                                                    </tr>
                                                    <asp:repeater ID="rptApprove" runat="server">
                                                        <ItemTemplate>
                                                            <tr class="default">
                                                                <td width="100%"><%# DataBinder.Eval(Container.DataItem, "username") %></td>
                                                                <td align="right"><asp:LinkButton ID="btnDeleteA" runat="server" OnClick="btnDeleteA_Click" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' Text="Delete" /></td>
                                                            </tr>
                                                        </ItemTemplate>
                                                    </asp:repeater>
                                                    <tr>
                                                        <td colspan="2">
                                                            <asp:Label ID="lblApprove" runat="server" CssClass="default" Visible="false" Text="<img src='/images/alert.gif' border='0' align='absmiddle'> There are no approvers" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <br />
                                <table width="100%" border="0" cellpadding="4" cellspacing="3">
                                    <tr>
                                        <td><hr size="1" noshade /></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Panel ID="panStep8Next" runat="server" Visible="false">
                                                <asp:Button ID="btnBack8" runat="server" CssClass="default" Width="100" Text="<<  Back" OnClick="btnBack8_Click" /> 
                                                <asp:Button ID="btnNext8" runat="server" CssClass="default" Width="100" Text="Finish" OnClick="btnNext8_Click" /> 
                                            </asp:Panel>
                                            <asp:Panel ID="panStep8Update" runat="server" Visible="false">
                                                <asp:Button ID="btnUpdate8" runat="server" CssClass="default" Width="100" Text="Update" OnClick="btnUpdate8_Click" /> 
                                                <asp:Button ID="btnCancel8" runat="server" CssClass="default" Width="100" Text="Cancel" OnClick="btnCancel_Click" /> 
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
            </div>
            <div style="display:none">
                <br />
                <table border="0" cellpadding="4" cellspacing="3">
                    <tr>
                        <td>
                            <table width="100%" cellpadding="0" cellspacing="5" border="0">
                                <tr>
                                    <td rowspan="2"><a href="javascript:void(0);" title="Click this image to download the document"><img src="/images/document.gif" border="0" align="absmiddle" /></a></td>
                                    <td class="bold" width="100%" valign="bottom">Create a New Service</td>
                                </tr>
                                <tr>
                                    <td width="100%" valign="top">This quick reference guide will help you in creating a new service and offering it to your clients.</td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table width="100%" cellpadding="0" cellspacing="5" border="0">
                                <tr>
                                    <td rowspan="2"><a href="javascript:void(0);" title="Click this image to download the document"><img src="/images/document.gif" border="0" align="absmiddle" /></a></td>
                                    <td class="bold" width="100%" valign="bottom">Configuring Notifications</td>
                                </tr>
                                <tr>
                                    <td width="100%" valign="top">This quick reference guide will help you in configuring the administrators and additional settings regarding your service.</td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table width="100%" cellpadding="0" cellspacing="5" border="0">
                                <tr>
                                    <td rowspan="2"><a href="javascript:void(0);" title="Click this image to download the document"><img src="/images/document.gif" border="0" align="absmiddle" /></a></td>
                                    <td class="bold" width="100%" valign="bottom">Configuring Hours</td>
                                </tr>
                                <tr>
                                    <td width="100%" valign="top">This quick reference guide will help you in configuring the hours associated with your service.</td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table width="100%" cellpadding="0" cellspacing="5" border="0">
                                <tr>
                                    <td rowspan="2"><a href="javascript:void(0);" title="Click this image to download the document"><img src="/images/document.gif" border="0" align="absmiddle" /></a></td>
                                    <td class="bold" width="100%" valign="bottom">Configuring a Form</td>
                                </tr>
                                <tr>
                                    <td width="100%" valign="top">This quick reference guide will help you in configuring the form associated with your service.</td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table width="100%" cellpadding="0" cellspacing="5" border="0">
                                <tr>
                                    <td rowspan="2"><a href="javascript:void(0);" title="Click this image to download the document"><img src="/images/document.gif" border="0" align="absmiddle" /></a></td>
                                    <td class="bold" width="100%" valign="bottom">Disabling or Deleting a Service</td>
                                </tr>
                                <tr>
                                    <td width="100%" valign="top">This quick reference guide will help you in disabling or deleting a service.</td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </div>
            </div>
            </asp:Panel>
            <asp:Panel ID="panDenied" runat="server" Visible="false">
                <table width="100%" cellpadding="2" cellspacing="2" border="0">
                    <tr>
                        <td><img src="/images/spacer.gif" border="0" width="<%=strIndent %>" height="1" /></td>
                        <td class="header"><img src="/images/bigX.gif" border="0" align="absmiddle" /> Access Denied</td>
                    </tr>
                    <tr><td colspan="2">&nbsp;</td></tr>
                    <tr>
                        <td><img src="/images/spacer.gif" border="0" width="<%=strIndent %>" height="1" /></td>
                        <td>You must have at least one (1) direct report to access this page. Please contact your manager if you would like to add or edit a service.</td>
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
<input type="hidden" id="hdnType" runat="server" />
<asp:Label ID="lblGroup" runat="server" Visible="false" />
<asp:HiddenField ID="hdnAssignM" runat="server" />
<asp:HiddenField ID="hdnAssignT" runat="server" />
<asp:HiddenField ID="hdnOwner" runat="server" />
<asp:HiddenField ID="hdnApprove" runat="server" />
<asp:HiddenField ID="hdnAccess" runat="server" />
<asp:HiddenField ID="hdnOrderSR" runat="server" />
<asp:HiddenField ID="hdnOrderWM" runat="server" />
<asp:HiddenField ID="hdnEnd" runat="server" />