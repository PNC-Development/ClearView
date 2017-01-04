<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="settings_controls.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.settings_controls" %>

<%@ Register Src="~/controls/UserContactInfo.ascx"
TagName="wucUserContactInfo" TagPrefix="ucUserContactInfo" %>


<table width="100%" cellpadding="0" cellspacing="0" border="0" class="default">
    <tr>
        <td><img src="/images/table_topleft.gif" border="0" width="5" height="26" /></td>
        <td nowrap background="/images/table_top.gif" class="greentableheader" width="100%"><asp:Label ID="lblTitle" runat="server" CssClass="greentableheader" /></td>
        <td><img src="/images/table_topright.gif" border="0" width="5" height="26" /></td>
    </tr>

    <tr>
        <td background="/images/table_left.gif"><img src="/images/table_left.gif" width="5" height="10"></td>
        <td width="100%" bgcolor="#FFFFFF">
            
            <asp:Panel id="pnlInfo" runat="server" Visible="false">
                <br />  
                <table width="100%" cellpadding="0" cellspacing="0" border="0" class="default">
                <tr>
                    <td width="100%">
                        <table width="100%" cellpadding="3" cellspacing="2" border="0" style="border:solid 1px #999999" bgcolor="#f9f9f9" >
                            <tr>
                                <td rowspan="2"><img src="/images/hugeAlert.gif" border="0" align="absmiddle" /></td>
                                <td class="header" width="100%" valign="bottom">Update Settings</td>
                            </tr>
                            <tr>
                                <td width="100%" valign="top">To keep ClearView up to date, we recommend updating your user information every three (3) months. Please verify the following information and click <b>Save Profile</b> to save your information.</td>
                            </tr>
                        </table>
                    </td>
                </tr>
                </table>
            </asp:Panel>
            <br />  
            <asp:Panel id="panSettings" runat="server">
             <%=strMenuTab1 %>
             <div id="divMenu1"> 
             <div style="display:none">
                <br />
                <table width="100%" cellpadding="0" cellspacing="5" border="0">
                <asp:Panel ID="panProfile" runat="server" Visible="false">
                <tr id="trSaved" runat="server" visible="false">
                    <td colspan="2" align="center" class="bigcheck">
                        <asp:Label ID="lblSaved" runat="server" Text="" />
                    </td>
                </tr>
                <tr>
                    <td class="header" colspan="2"><asp:Image ID="imgHeader" runat="server" BorderStyle="None" ImageAlign="AbsMiddle" /> My Information</td>
                </tr>
                <tr> 
                    <td nowrap><b>Username:</b></td>
                    <td width="100%"><asp:TextBox ID="txtUser" CssClass="default" runat="server" Width="150" MaxLength="30" Enabled="false"/></td>
                </tr>
                <tr id="rowPNC" runat="server" visible="true"> 
                    <td nowrap><b>PNC ID:</b></td>
                    <td width="100%"><asp:TextBox ID="txtPNC" CssClass="default" runat="server" Width="150" MaxLength="30" Enabled="false"/></td>
                </tr>
                <tr> 
                    <td nowrap><b>First name:</b></td>
                    <td width="100%"><asp:TextBox ID="txtFirst" CssClass="default" runat="server" Width="200" MaxLength="100"/></td>
                </tr>
                <tr> 
                    <td nowrap><b>Last name:</b></td>
                    <td width="100%"><asp:TextBox ID="txtLast" CssClass="default" runat="server" Width="200" MaxLength="100"/></td>
                </tr>
                <tr> 
                    <td nowrap><b>Phone:</b></td>
                    <td width="100%"><asp:TextBox ID="txtPhone" CssClass="default" runat="server" Width="100" MaxLength="15"/></td>
                </tr>
                <tr style="display:none"> 
                    <td nowrap><b>Pager:</b></td>
                    <td width="100%"><asp:TextBox ID="txtPager" CssClass="default" runat="server" Width="100" MaxLength="15"/> @ <asp:dropdownlist ID="ddlUserAt" CssClass="default" runat="server"/></td>
                </tr>
                <tr> 
                    <td nowrap><b>Reports To:</b></td>
                    <td width="100%">
                        <asp:Label ID="lblManager" CssClass="default" runat="server"/>
                         <asp:Panel ID="pnlManager" runat="server" Visible="false">
                             <table cellpadding="0" cellspacing="0" border="0">
                                    <tr>
                                        <td><asp:TextBox ID="txtManager" runat="server" Width="200" CssClass="default" /></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div id="divManager" runat="server" style="overflow:hidden; position:absolute; display:none; background-color:#FFFFFF; border:solid 1px #CCCCCC">
                                                <asp:ListBox ID="lstManager" runat="server" CssClass="default" />
                                            </div>
                                        </td>
                                    </tr>
                            </table>
                          <input type="hidden" id="hdnManager" name="hdnManager" runat="server"/>
                         </asp:Panel>
                         &nbsp;&nbsp;<asp:Button ID="btnManagerUpdate" runat="server" Text="Update Now" Width="100" OnClick="btnManagerUpdate_Click" />
                    </td>
                </tr>
                <tr> 
                    <td nowrap><b>Vacation Days:</b></td>
                    <td width="100%"><asp:Label ID="lblVacation" CssClass="default" runat="server"/> day(s) *</td>
                </tr>
                <tr>
                    <td colspan="2"><img src="/images/spacer.gif" border="0" width="1" height="1" /></td>
                </tr>
                <tr> 
                    <td nowrap valign="top"><b>Special Skills:</b></td>
                    <td width="100%"><asp:TextBox ID="txtSkills" CssClass="default" runat="server" Width="500" Rows="7" TextMode="MultiLine" /></td>
                </tr>
                <tr>
                    <td nowrap valign="top"><b>Picture:</b></td>
                    <td width="100%">
                        <table cellpadding="0" cellspacing="0" border="0" width="500">
                            <tr>
                                <td>
                                    <asp:Image ID="imgPicture" runat="server" Width="90" Height="90" />
                                    <asp:LinkButton ID="btnPicture" runat="server" Text="Change Photo" />
                                </td>
                                <td align="right">
                                    <table cellpadding="0" cellspacing="5" border="0">
                                        <tr> 
                                            <td>
                                                <asp:Label ID="lblIsManager" CssClass="default" runat="server" /> *
                                                &nbsp;&nbsp;<asp:Button ID="btnIsManager" runat="server" Text="Make me one" OnClick="btnIsManager_Click" Visible="false" />
                                                <input type="hidden" id="hdnIsManager" name="hdnIsManager" runat="server"/>
                                            </td>
                                        </tr>
                                        <tr> 
                                            <td><asp:Label ID="lblBoard" CssClass="default" runat="server" /> *</td>
                                        </tr>
                                        <tr> 
                                            <td><asp:Label ID="lblDirector" CssClass="default" runat="server" /> *</td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr> 
                    <td nowrap></td>
                    <td width="100%"><asp:CheckBox ID="chkUngroupProjects" runat="server" Text="Ungroup projects in my workload manager" /></td>
                </tr>
                <tr> 
                    <td nowrap></td>
                    <td width="100%"><asp:CheckBox ID="chkShowReturns" runat="server" Text="Display service requests that have been returned to me" /></td>
                </tr>
                <tr>
                    <td colspan="2">&nbsp;</td>
                </tr>
                <tr>
                    <td nowrap>&nbsp;</td>
                    <td>
                        <table cellpadding="0" cellspacing="0" border="0" width="500">
                            <tr>
                                <td><asp:Button ID="btnProfile" CssClass="default" runat="server" Text="Update Profile" Width="125" OnClick="btnProfile_Click" /></td>
                                <td align="right">* = Submit a support request to have this changed.</td>
                            </tr>
                        </table>
                    </td>
                </tr>
              </asp:Panel>
                <asp:Panel ID="panManagerProfile" runat="server" Visible="false">
                <tr>
                    <td class="header" colspan="2"><asp:Image ID="imgManagerHeader" runat="server" BorderStyle="None" ImageAlign="AbsMiddle" /> Employee Information <asp:Label ID="lblHeader" CssClass="header" runat="server"/></td>
                </tr>
                <tr> 
                    <td nowrap><b>Username:</b></td>
                    <td width="100%"><asp:TextBox ID="txtManagerUser" CssClass="default" runat="server" Width="150" MaxLength="30" Enabled="false"/></td>
                </tr>
                <tr id="rowManagerPNC" runat="server" visible="true">
                    <td nowrap><b>PNC ID:</b></td>
                    <td width="100%"><asp:TextBox ID="txtManagerPNC" CssClass="default" runat="server" Width="150" MaxLength="30" Enabled="false"/></td>
                </tr>
                <tr> 
                    <td nowrap><b>First name:</b></td>
                    <td width="100%"><asp:TextBox ID="txtManagerFirst" CssClass="default" runat="server" Width="200" MaxLength="100"/></td>
                </tr>
                <tr> 
                    <td nowrap><b>Last name:</b></td>
                    <td width="100%"><asp:TextBox ID="txtManagerLast" CssClass="default" runat="server" Width="200" MaxLength="100"/></td>
                </tr>
                <tr> 
                    <td nowrap><b>Phone:</b></td>
                    <td width="100%"><asp:TextBox ID="txtManagerPhone" CssClass="default" runat="server" Width="100" MaxLength="15"/></td>
                </tr>
                <tr style="display:none"> 
                    <td nowrap><b>Pager:</b></td>
                    <td width="100%"><asp:TextBox ID="txtManagerPager" CssClass="default" runat="server" Width="100" MaxLength="15"/> @ <asp:dropdownlist ID="ddlManagerUserAt" CssClass="default" runat="server"/></td>
                </tr>
                <tr> 
                    <td nowrap><b>Reports To:</b></td>
                    <td width="100%"><asp:Label ID="lblManagerReports" CssClass="default" runat="server"/></td>
                </tr>
                <tr> 
                    <td nowrap><b>Vacation Days:</b></td>
                    <td width="100%"><asp:TextBox ID="txtManagerVacation" CssClass="default" runat="server" Width="50" MaxLength="4"/> day(s)</td>
                </tr>
                <tr> 
                    <td nowrap valign="top"><b>Special Skills:</b></td>
                    <td width="100%"><asp:TextBox ID="txtManagerSkills" CssClass="default" runat="server" Width="500" Rows="7" TextMode="MultiLine" /></td>
                </tr>
                <tr>
                    <td nowrap valign="top"><b>Picture:</b></td>
                    <td width="100%">
                        <table cellpadding="0" cellspacing="0" border="0" width="500">
                            <tr>
                                <td>
                                    <asp:Image ID="imgManagerPicture" runat="server" Width="90" Height="90" />
                                    <asp:LinkButton ID="LinkButton1" runat="server" Text="Change Photo" />
                                </td>
                                <td align="right">
                                    <table cellpadding="0" cellspacing="5" border="0">
                                        <tr> 
                                            <td><asp:CheckBox ID="chkManagerIsManager" CssClass="default" runat="server" Text="This person manages employees" /></td>
                                        </tr>
                                        <tr> 
                                            <td><asp:Label ID="lblManagerBoard" CssClass="default" runat="server" /></td>
                                        </tr>
                                        <tr> 
                                            <td><asp:Label ID="lblManagerDirector" CssClass="default" runat="server" /></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr> 
                    <td nowrap></td>
                    <td width="100%"><asp:CheckBox ID="chkManagerUngroupProjects" runat="server" Text="Ungroup projects in my workload manager" /></td>
                </tr>
                <tr> 
                    <td nowrap></td>
                    <td width="100%"><asp:CheckBox ID="chkManagerShowReturns" runat="server" Text="Display service requests that have been returned to me" /></td>
                </tr>
                <tr>
                    <td colspan="2">&nbsp;</td>
                </tr>
                <tr>
                    <td nowrap>&nbsp;</td>
                    <td><asp:Button ID="btnManager" CssClass="default" runat="server" Text="Update Profile" Width="125" OnClick="btnManager_Click" /> <asp:Button ID="btnCancel" CssClass="default" runat="server" Text="Cancel Changes" Width="125" OnClick="btnCancel_Click" /></td>
                </tr>
            </asp:Panel>
                   
                </table>   
             </div>
             
              <div style="display:none">
                <br />
                <ucUserContactInfo:wucUserContactInfo ID="ucUserContactInfo" runat="server" />
             </div>
             
             <div style="display:none">
               <br />
                <table width="100%" cellpadding="0" cellspacing="5" border="0">
                
                <tr>
                    <td class="header" colspan="2"><img src="/images/clock.gif" border="0" align="absmiddle" /> My Scheduled Time Off <asp:Label ID="lblVacationName" CssClass="header" runat="server"/></td>
                </tr>
                <tr>
                    <td colspan="2">
                        <table width="100%" cellpadding="4" cellspacing="0" border="0" align="center" style="border:solid 1px #CCCCCC">
                            <tr bgcolor="#EEEEEE">
                                <td><b>Date</b></td>
                                <td><b>Type</b></td>
                                <td><b>Reason</b></td>
                                <td><b>Status</b></td>
                                <td></td>
                            </tr>
                            <asp:repeater ID="rptVacation" runat="server">
                                <ItemTemplate>
                                    <tr>
                                        <td><%# DateTime.Parse(DataBinder.Eval(Container.DataItem, "start_date").ToString()).ToString("ddd, MMM dd, yyyy")%></td>
                                        <td><%# DataBinder.Eval(Container.DataItem, "duration") %></td>
                                        <td><%# DataBinder.Eval(Container.DataItem, "reason") %></td>
                                        <td><%# (DataBinder.Eval(Container.DataItem, "approved").ToString() == "1" ? "Approved" : "Pending") %></td>
                                        <td align="right">[<asp:LinkButton ID="btnDeleteVacation" runat="server" Text="Delete" OnClick="btnDeleteVacation_Click" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "vacationid") %>' />]</td>
                                    </tr>
                                </ItemTemplate>
                            </asp:repeater>
                            <tr>
                                <td colspan="5">
                                    <asp:Label ID="lblNone" runat="server" CssClass="default" Visible="false" Text="<img src='/images/spacer.gif' border='0' height='1' width='25' /><img src='/images/alert.gif' border='0' align='absmiddle'> No scheduled time off" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>                   
                </table> 
             </div> 
             <div style="display:none">
               <br />
                <table cellpadding="3" cellspacing="2" border="0" width="100%">
                    <tr>
                        <td rowspan="2"><img src="/images/buddy.gif" border="0" align="absmiddle" /></td>
                        <td class="header" width="100%" valign="bottom">Out of Office Buddies</td>
                        <td align="right"><asp:Button ID="btnAddBuddy" runat="server" CssClass="default" Width="100" Text="Add Buddy" /></td>
                    </tr>
                    <tr>
                        <td width="100%" valign="top" colspan="2">The following person(s) will be added to all emails sent to you, and will have access to all of your assignment and approval queues.</td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <table width="100%" cellpadding="4" cellspacing="0" border="0" align="center" style="border:solid 1px #CCCCCC">
                                <tr bgcolor="#EEEEEE">
                                    <td><b>Buddy Name</b></td>
                                    <td></td>
                                </tr>
                                <asp:repeater ID="rptBuddies" runat="server">
                                    <ItemTemplate>
                                        <tr>
                                            <td><%# DataBinder.Eval(Container.DataItem, "fullname") %></td>
                                            <td align="right">[<asp:LinkButton ID="btnDeleteBuddy" runat="server" Text="Delete" OnClick="btnDeleteBuddy_Click" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' />]</td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:repeater>
                                <tr>
                                    <td colspan="2">
                                        <asp:Label ID="lblNoBuddies" runat="server" CssClass="default" Visible="false" Text="<img src='/images/spacer.gif' border='0' height='1' width='25' /><img src='/images/alert.gif' border='0' align='absmiddle'> No buddies" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                   </tr> 
                    <tr>
                        <td></td>
                        <td colspan="2">
                            <br />
                            Being an out of office buddy is a difficult task - you receive a ton of emails and have to do more than your own work.  Feel free to remove yourself if you no longer want to be an out of office buddy. You are cconfigured as an out of office buddy for the following people:
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <table width="100%" cellpadding="4" cellspacing="0" border="0" align="center" style="border:solid 1px #CCCCCC">
                                <tr bgcolor="#EEEEEE">
                                    <td colspan="2"><b>Configured By</b></td>
                                    <td></td>
                                </tr>
                                <asp:repeater ID="rptCovering" runat="server">
                                    <ItemTemplate>
                                        <tr>
                                            <td><%# DataBinder.Eval(Container.DataItem, "fullname") %></td>
                                            <td align="right">[<asp:LinkButton ID="btnDeleteBuddy" runat="server" Text="Remove Me" OnClick="btnDeleteBuddy_Click" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' />]</td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:repeater>
                                <tr>
                                    <td colspan="2">
                                        <asp:Label ID="lblCovering" runat="server" CssClass="default" Visible="false" Text="<img src='/images/spacer.gif' border='0' height='1' width='25' /><img src='/images/alert.gif' border='0' align='absmiddle'> No buddies" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                   </tr>              
                </table> 
             </div>
             <div style="display:none">
                <br />
                 <table width="100%" cellpadding="0" cellspacing="3" border="1" class="default">
                    <tr style="border-width:0">
                        <td class="header" colspan="2" style="border-width:0"><img src="/images/reports.gif" border="0" align="absmiddle" /> My Direct Reports</td>
                    </tr>
                    
                    <tr style="border-width:0" valign="top">
                        <td width="30%" style="border-width:0" >
                            <asp:Label ID="lblReportingSelectedUser" runat="server" Width="250" CssClass="defaultbold" Visible="true"/>
                            <asp:TextBox ID="txtReportingSelectedUserValue" runat="server" Visible ="false" CssClass="default" /> 
                        </td>
                        <td width="70%" style="border-width:0"  align="right">
                            <asp:Panel ID="pnlAddReportingUser" runat="server" Visible="false"  >
                                <table >
                                    <tr>
                                    <td> Add User:</td>
                                        <td >
                                            <asp:TextBox ID="txtReportingUser" runat="server" Width="250" CssClass="default" /> 
                                            <asp:Button ID="btnAddReportingUser" runat="server" CssClass="default" Text="Add" width="75" OnClick="btnAddReportingUser_Click"/>
                                        </td>
                                    </tr>
                                    <tr height="0">
                                     <td>&nbsp;</td>
                                        <td>
                                            <div id="divReportingUser" runat="server" style="overflow:hidden; position:absolute; display:none; background-color:#FFFFFF; border:solid 1px #CCCCCC">
                                                <asp:ListBox ID="lstReportingUser" runat="server" CssClass="default" /> 
                                            </div>
                                        </td>   
                                    </tr>
                                </table>
                             </asp:Panel>
                        </td>
                    </tr>
                    
                    <tr valign="top" height="100%" style="border-width:1" >   
                        <td width="30%" >
                            <asp:Panel ID="pnlResourceReporting" runat="server" Visible="true"  height="400" ScrollBars="Auto" >
                                <asp:TreeView ID="tvResourceReporting" runat="server" ShowLines="true" OnSelectedNodeChanged="tvResourceReporting_SelectedNodeChanged" OnTreeNodePopulate="tvResourceReporting_TreeNodePopulate" Visible =true NodeIndent="35">
                                <NodeStyle CssClass="default"  />
                                 <SelectedNodeStyle CssClass="defaultbold"  />
                                </asp:TreeView>
                             </asp:Panel>         
                        </td>
                        <td width="70%">
                         <asp:Panel ID="pnlResourceList" runat="server" Visible="true"  height="400" ScrollBars="Auto" >
                             <asp:DataList ID="dlResourceList" runat="server" CellPadding="3" CellSpacing="1" Width="100%" OnItemDataBound="dlResourceList_ItemDataBound">
                                <HeaderTemplate>
                                    <tr bgcolor="#EEEEEE">
                                        <td align="left" width="100%">
                                            <asp:Label ID="lblRLHeaderUserName" runat="server" CssClass="default" Text="<b>User Name</b>"  />
                                        </td>
                                        <td align="left" width="10%">
                                            <asp:Label ID="lblRLHeaderVacation" runat="server" CssClass="default" Text="<b>Vacation</b>"/>
                                        </td>
                                        <td align="left" width="10%">
                                            <asp:Label ID="lblRLHeaderPersonal" runat="server" CssClass="default" Text="<b>Personal</b>"/>
                                        </td>
                                        <td align="left" width="10%">
                                            <asp:Label ID="lblRLHeaderFloating" runat="server" CssClass="default" Text="<b>Floating</b>"/>
                                        </td>
                                        <td align="left" width="10%">
                                            <asp:Label ID="lblRLHeaderEdit" runat="server" CssClass="default" Text="" />
                                        </td>
                                        <td align="left" width="10%">
                                            <asp:Label ID="lblRLHeaderRemove" runat="server" CssClass="default" Text="" />
                                        </td>
                                    </tr>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td align="left" valign="top" >
                                            <asp:Label ID="lblRLUserName" runat="server" CssClass="default" />
                                        </td>
                                        <td align="left" valign="top" >
                                            <asp:Label ID="lblRLVacation" runat="server" CssClass="default" />
                                        </td>
                                        <td align="left" valign="top" >
                                            <asp:Label ID="lblRLPersonal" runat="server" CssClass="default" />
                                        </td>
                                        <td align="left" valign="top" >
                                            <asp:Label ID="lblRLFloating" runat="server" CssClass="default" />
                                        </td>
                                        <td align="left" valign="top" >
                                            <asp:LinkButton ID="lnkbtnRLEdit" runat="server" OnClick="lnkbtnRLEdit_Click" ToolTip="Edit" Text="Edit" CssClass="lookup" />
                                        </td>
                                        <td align="left" valign="top" >
                                            <asp:LinkButton ID="lnkbtnRLRemove" runat="server" OnClick="lnkbtnRLRemove_Click" ToolTip="Remove" Text="Remove"  CssClass="lookup" />
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <AlternatingItemTemplate>
                                    <tr bgcolor="#F6F6F6">
                                        <td align="left" valign="top" >
                                           <asp:Label ID="lblRLUserName" runat="server" CssClass="default" />
                                        </td>
                                        <td align="left" valign="top" >
                                            <asp:Label ID="lblRLVacation" runat="server" CssClass="default" />
                                        </td>
                                        <td align="left" valign="top" >
                                            <asp:Label ID="lblRLPersonal" runat="server" CssClass="default" />
                                        </td>
                                        <td align="left" valign="top" >
                                            <asp:Label ID="lblRLFloating" runat="server" CssClass="default" />
                                        </td>
                                        <td align="left" valign="top" >
                                            <asp:LinkButton ID="lnkbtnRLEdit" runat="server" OnClick="lnkbtnRLEdit_Click" ToolTip="Edit" Text="Edit" CssClass="lookup" />
                                        </td>
                                        <td align="left" valign="top" >
                                            <asp:LinkButton ID="lnkbtnRLRemove" runat="server" OnClick="lnkbtnRLRemove_Click" ToolTip="Remove" Text="Remove"  CssClass="lookup" />
                                        </td>
                                    </tr>
                                </AlternatingItemTemplate>
                            </asp:DataList>
                         </asp:Panel>   
                    </td>
                    </tr>                    
                </table>                              
             </div>  
             
             <div style="display:none">
               <br />
                <table width="100%" cellpadding="0" cellspacing="5" border="0">
                    <tr>
                    <td class="header" colspan="2"><img src="/images/reportstime.gif" border="0" align="absmiddle" /> My Direct Reports Scheduled Time Off</td>
                </tr>
                <tr>
                    <td colspan="2">
                    <table width="100%" cellpadding="4" cellspacing="0" border="0" align="center" style="border:solid 1px #CCCCCC">
                        <tr bgcolor="#EEEEEE">
                            <td><b>Employee Name</b></td>
                            <td><b>Date</b></td>
                            <td><b>Type</b></td>
                            <td><b>Reason</b></td>
                            <td><b>&nbsp;</b></td>
                            <td><b>&nbsp;</b></td>
                        </tr>
                            <asp:repeater ID="rptReportsVacation" runat="server">
                            <ItemTemplate>
                                <tr>
                                    <td width="100%"><%# oUser.GetFullName(Int32.Parse(DataBinder.Eval(Container.DataItem, "userid").ToString())) %></td>
                                    <td nowrap><%# DateTime.Parse(DataBinder.Eval(Container.DataItem, "start_date").ToString()).ToString("ddd, MMM dd, yyyy")%></td>
                                    <td nowrap><%# DataBinder.Eval(Container.DataItem, "duration") %></td>
                                    <td nowrap><%# DataBinder.Eval(Container.DataItem, "reason") %></td>
                                    <td nowrap>[<asp:LinkButton ID="btnApprove" OnClick="btnApprove_Click" runat="server" ToolTip="Approve" Text="Approve" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "vacationid") %>' />]</td>
                                    <td nowrap>[<asp:LinkButton ID="btnDeny" OnClick="btnDeny_Click" runat="server" ToolTip="Deny" Text="Deny" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "vacationid") %>' />]</td>
                                </tr>
                            </ItemTemplate>
                            </asp:repeater>
                        <tr>
                            <td colspan="6">
                                <asp:Label ID="lblReports" runat="server" CssClass="default" Visible="false" Text="<img src='/images/spacer.gif' border='0' height='1' width='25' /><img src='/images/alert.gif' border='0' align='absmiddle'> None of your direct reports have pending requests" />
                            </td>
                        </tr>
                    </table>
                    </td>
                </tr>
                </table> 
             </div>
            
             
              <div style="display:none">
                 <br />
                 <table width="100%" cellpadding="0" cellspacing="3" border="1"  class="default">
                    <tr style="border-width:0">
                        <td class="header" colspan="2" style="border-width:0"><img src="/images/tasks.gif" border="0" align="absmiddle" />Application Management</td>
                    </tr>
                    <tr style="border-width:0" valign="top">
                        <td width="30%" style="border-width:0">
                            <asp:Label ID="lblApplication" runat="server"  CssClass="defaultbold" Visible="true" Text="Application:"/>                            
                            <asp:DropDownList ID="ddlApplication" runat="server" CssClass="default" AutoPostBack="true" OnSelectedIndexChanged="ddlApplication_OnSelectedIndexChanged" /> 
                        </td>
                        <td width="70%" style="border-width:0" align="right">
                            <asp:Panel ID="pnlAddAppResource" runat="server" Visible="false"  >
                                 <table >
                                    <tr>
                                    <td> Add User:</td>
                                        <td >
                                            <asp:TextBox ID="txtAppResource" runat="server" Width="250" CssClass="default" /> 
                                            <asp:Button ID="btnAddAppResource" runat="server" CssClass="default" Text="Add" width="75" OnClick="btnAddAppResource_Click"/>
                                        </td>
                                    </tr>
                                    <tr>
                                     <td>&nbsp;</td>
                                        <td>
                                            <div id="divAppResource" runat="server" style="overflow:hidden; position:absolute; display:none; background-color:#FFFFFF; border:solid 1px #CCCCCC">
                                                <asp:ListBox ID="lstAppResource" runat="server" CssClass="default" /> 
                                            </div>
                                        </td>   
                                    </tr>
                                    
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr style="border-width:0">
                        <td  colspan="2" style="border-width:0">&nbsp;
                        </td>
                    </tr>
                    <tr valign="top" style="border-width:1">   
                        <td width="100%" colspan="2" >
                             <asp:Panel ID="pnlAppResourceList" runat="server" Visible="false"   height="400" ScrollBars="Auto" >
                                 <asp:DataList ID="dlApplicationResources" runat="server" CellPadding="3" CellSpacing="1" Width="100%" OnItemDataBound="dlApplicationResources_ItemDataBound">
                                    <HeaderTemplate>
                                        <tr bgcolor="#EEEEEE">
                                            <td align="left" width="100%">
                                                <asp:Label ID="lblAppRLHeaderUserName" runat="server" CssClass="default" Text="<b>User Name</b>"  />
                                            </td>
                                            <td align="left" width="10%">
                                                <asp:Label ID="lblAppRLHeaderRemove" runat="server" CssClass="default" Text="" />
                                            </td>
                                        </tr>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td align="left" valign="top" >
                                                <asp:Label ID="lblAppRLUserName" runat="server" CssClass="default" />
                                            </td>
                                            <td align="left" valign="top" >
                                                <asp:LinkButton ID="lnkbtnAppRLRemove" runat="server" OnClick="lnkbtnAppRLRemove_Click" ToolTip="Remove" Text="Remove"  CssClass="lookup" />
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                    <AlternatingItemTemplate>
                                        <tr bgcolor="#F6F6F6">
                                             <td align="left" valign="top" >
                                                <asp:Label ID="lblAppRLUserName" runat="server" CssClass="default" />
                                            </td>
                                            <td align="left" valign="top" >
                                                <asp:LinkButton ID="lnkbtnAppRLRemove" runat="server" OnClick="lnkbtnAppRLRemove_Click" ToolTip="Remove" Text="Remove"  CssClass="lookup" />
                                            </td>
                                        </tr>
                                    </AlternatingItemTemplate>
                                </asp:DataList>
                             </asp:Panel>   
                        </td>
                    </tr>                    
                </table>                              
             </div>  
             
             
             
             </div>         
            </asp:Panel>                
            <asp:Panel ID="panError" runat="server" Visible="false">
                <table width="100%" cellpadding="2" cellspacing="2" border="0">
                    <tr>
                        <td><img src="/images/spacer.gif" border="0" width="15" height="1" /></td>
                        <td class="header"><img src="/images/bigX.gif" border="0" align="absmiddle" /> Profile Problem</td>
                    </tr>
                    <tr><td colspan="2">&nbsp;</td></tr>
                    <tr>
                        <td><img src="/images/spacer.gif" border="0" width="15" height="1" /></td>
                        <td>There is a problem with your profile, or it has been deleted from the system.  Please contact the system administrator.</td>
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

<asp:HiddenField ID="hdnValues" runat="server" />
<asp:HiddenField ID="hdnType" runat="server" />
<asp:HiddenField ID="hdnAppResource" runat="server" />
<asp:HiddenField ID="hdnReportingUser" runat="server" />