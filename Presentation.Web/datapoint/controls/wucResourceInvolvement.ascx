<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucResourceInvolvement.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.wucResourceInvolvement" %>

 <asp:Panel ID="pnlRRinvolvement" runat="server" Visible="true">
     <table id="tblHeader" width="100%" cellpadding="4" cellspacing="2" border="0">
         <tr>
             <td class="header" width="100%" valign="bottom">
                 Resource Involvement</td>
         </tr>
     </table>
    <table width="100%" cellpadding="4" cellspacing="0" border="0" style="border:solid 1px #CCCCCC">
        <tr bgcolor="#EEEEEE">
            <td><b><u>Assigned To:</u></b></td>
            <td><b><u>Service:</u></b></td>
            <td><b><u>Progress:</u></b></td>
            <td><b><u>Last Updates:</u></b></td>
            <td><b><u>Status:</u></b></td>
            <td><b><u>Options:</u></b></td>
        </tr>
        <asp:repeater ID="rptResourceInvolvement" runat="server">
            <ItemTemplate>
                <tr>
                    <asp:Label ID="lblUsed" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "RRWFUsed") %>' />
                    <asp:Label ID="lblAllocated" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "RRWFAllocated") %>' />
                    <asp:Label ID="lblRRID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "ResourceRequestID") %>' />
                    <td valign="top">
                        <table cellpadding="0" cellspacing="0" border="0">
                            <tr>
                                <td nowrap><asp:Label ID="lblResource" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "RRWFUserName").ToString() == "" ? "" : DataBinder.Eval(Container.DataItem, "RRWFUserName") + " (" +  DataBinder.Eval(Container.DataItem, "RRWFUserXID") + ")" %>' ToolTip='<%# DataBinder.Eval(Container.DataItem, "RRWFUserId") %>' /></td>
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td><asp:Label ID="lblRequest" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "ReqServiceNumber") %>' ToolTip='<%# DataBinder.Eval(Container.DataItem, "RequestNumber") %>' /></td>
                            </tr>
                        </table>
                    </td>
                    <td valign="top">
                        <table cellpadding="0" cellspacing="0" border="0">
                            <tr>
                                <td colspan="4"><asp:Label ID="lblService" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "ServiceName") %>' ToolTip='<%# DataBinder.Eval(Container.DataItem, "ServiceId") %>' /></td>
                            </tr>
                            <tr>
                                <td colspan="4">&nbsp;</td>
                            </tr>
                            <tr>
                                <td align="right">Initaited: </td>
                                <td align="right"><%# DataBinder.Eval(Container.DataItem, "RRAssignedDate").ToString() == "" ? "" : DateTime.Parse(DataBinder.Eval(Container.DataItem, "RRAssignedDate").ToString()).ToShortDateString() %></td>
                                <td>&nbsp;@&nbsp;</td>
                                <td align="right"><%# DataBinder.Eval(Container.DataItem, "RRAssignedDate").ToString() == "" ? "" : DateTime.Parse(DataBinder.Eval(Container.DataItem, "RRAssignedDate").ToString()).ToShortTimeString() %></td>
                            </tr>
                            <tr>
                                <td align="right">Completed: </td>
                                <td align="right"><%# DataBinder.Eval(Container.DataItem, "RRCompletedDate").ToString() == "" ? "" : DateTime.Parse(DataBinder.Eval(Container.DataItem, "RRCompletedDate").ToString()).ToShortDateString() %></td>
                                <td><asp:Label ID="lblCompleted" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "RRCompletedDate") %>' /></td>
                                <td align="right"><%# DataBinder.Eval(Container.DataItem, "RRCompletedDate").ToString() == "" ? "" : DateTime.Parse(DataBinder.Eval(Container.DataItem, "RRCompletedDate").ToString()).ToShortTimeString() %></td>
                            </tr>
                        </table>
                    </td>
                    <td valign="top">
                        <table cellpadding="0" cellspacing="0" border="0">
                            <tr>
                                <td colspan="2" nowrap><asp:Label ID="lblProgress" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "RRWFStatus") %>' /></td>
                            </tr>
                            <tr>
                                <td nowrap>SLA: </td>
                                <td nowrap align="right" class="bold"><%# DataBinder.Eval(Container.DataItem, "ServiceSLA") %> HRs
                                <!--
                                    <%# DataBinder.Eval(Container.DataItem, "RRWFUsed") %> of <%# DataBinder.Eval(Container.DataItem, "RRWFAllocated") %> HRs
                                -->
                                </td>
                            </tr>
                            <tr>
                                <td nowrap>Time: </td>
                                <td nowrap align="right"><asp:Label ID="lblTime" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "ElapsedTime") + " HRs" %>' ToolTip='<%# DataBinder.Eval(Container.DataItem, "ServiceSLA") %>' /></td>
                            </tr>
                        </table>
                    </td>
                    <td valign="top">
                        <table cellpadding="0" cellspacing="0" border="0">
                            <tr>
                                <td nowrap><asp:Label ID="lblUpdatedBy" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "RRWFModifiedUserName").ToString() == "" ? "" : DataBinder.Eval(Container.DataItem, "RRWFModifiedUserName") + " (" +  DataBinder.Eval(Container.DataItem, "RRWFModifiedUserXID") + ")" %>' ToolTip='<%# DataBinder.Eval(Container.DataItem, "RRWFModifiedByUserId") %>' /></td>
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td><%# DataBinder.Eval(Container.DataItem, "RRWFModifiedDate") %></td>
                            </tr>
                        </table>
                    </td>
                    <td valign="top">
                        <table cellpadding="0" cellspacing="0" border="0">
                            <tr>
                                <td nowrap><%# oStatusLevel.HTML(Int32.Parse(DataBinder.Eval(Container.DataItem, "RRWFStatus").ToString())) %></td>
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td>
                                     <asp:Panel ID="pnlDelete" runat="server" Visible="false" ToolTip='<%# DataBinder.Eval(Container.DataItem, "RRWFDeleted") %>'>
                                         <div align="center">
                                             <span class="header">
                                                 <img id="imgWFDeleted" runat="server" src="/images/bigError.gif" border="0" align="absmiddle" />
                                                 Deleted!</span></div>
                                     </asp:Panel>
                                </td>
                            </tr>
                         </table>
                    </td>
                    <td valign="top">
                        <table cellpadding="0" cellspacing="0" border="0">
                            <tr>
                                <td nowrap><asp:LinkButton ID="btnVirtualView" runat="server" Text="Virtual&nbsp;View" CssClass="lookup" ToolTip='<%# DataBinder.Eval(Container.DataItem, "RRWFId") %>' /></td>
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td><asp:LinkButton ID="btnEdit" runat="server" Text="Edit" CssClass="lookup" ToolTip='<%# DataBinder.Eval(Container.DataItem, "ServiceId") %>' /></td>
                            </tr>
                        </table>
                    </td>
                </tr>
                 <tr>
                     <td width="100%" colspan="6" align="left">
                        <br />
                         <table id ="tblCurrentStatusUpdates" runat="server" cellpadding="2" cellspacing="2" border="0" width="100%" style="border: solid 1px #CCCCCC">
                             <tr>
                                 <td width="5%" valign="top"><img src='/images/<%# DataBinder.Eval(Container.DataItem, "weekly_image")%>.gif' border="0" align="absmiddle" /></td>
                                 <td width="95%" valign="Top" class="default">
                                     <table width="100%">
                                         <tr>
                                             <td colspan="2">
                                                 <asp:Label ID="lblComments" runat="server" CssClass="default" Text='<%# oFunction.FormatText(DataBinder.Eval(Container.DataItem, "RRUpdateWeeklyComments").ToString()) %>' />
                                             </td>
                                         </tr>
                                         <tr>
                                             <td colspan="2">&nbsp;</td>
                                         </tr>
                                         <tr>
                                             <td>
                                                 Posted By 
                                                 <asp:Label ID="lblResourceStatus" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "RRUpdateWeeklyUserName").ToString() == "" ? "" : DataBinder.Eval(Container.DataItem, "RRUpdateWeeklyUserName") + " (" +  DataBinder.Eval(Container.DataItem, "RRUpdateWeeklyUserXID") + ")" %>' ToolTip='<%# DataBinder.Eval(Container.DataItem, "RRUpdateWeeklyUserId") %>' />
                                                 on 
                                                 <%# DataBinder.Eval(Container.DataItem, "RRUpdateWeeklyModified") %>
                                             </td>
                                             <td align="right">
                                                 <asp:LinkButton ID="lnkbtnAdditionalComments" runat="server" Text="More comments..." Visible="true" />
                                             </td>
                                         </tr>
                                     </table>
                                 </td>
                             </tr>
                            <tr runat="server" id="trAdditionalComments" style="display:none">
                                <td colspan="7">
                                    <table width="100%" cellpadding="0" cellspacing="5" border="0">
                                        <asp:repeater id="rptServices" datasource='<%# ((DataRowView)Container.DataItem).Row.GetChildRows("relationship") %>' runat="server">
                                            <ItemTemplate>
                                                 <tr>
                                                     <td colspan="6">
                                                         <hr size="1" noshade />
                                                     </td>
                                                 </tr>
                                                <tr>
                                                     <td width="5%" valign="top"><img src='/images/<%# DataBinder.Eval(Container.DataItem, "[\"weekly_image\"]")%>.gif' border="0" align="absmiddle" /></td>
                                                     <td width="95%" valign="Top" class="default">
                                                         <table width="100%">
                                                             <tr>
                                                                 <td>
                                                                     <asp:Label ID="lblComments" runat="server" CssClass="default" Text='<%# oFunction.FormatText(DataBinder.Eval(Container.DataItem, "[\"comments\"]").ToString())%>' />
                                                                 </td>
                                                             </tr>
                                                             <tr>
                                                                 <td>&nbsp;</td>
                                                             </tr>
                                                             <tr>
                                                                 <td>
                                                                     Posted By 
                                                                     <asp:Label ID="lblResourceStatus" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "[\"resource_name\"]").ToString() == "" ? "" : DataBinder.Eval(Container.DataItem, "[\"resource_name\"]") + " (" +  DataBinder.Eval(Container.DataItem, "[\"resource_xid\"]") + ")" %>' ToolTip='<%# DataBinder.Eval(Container.DataItem, "[\"userid\"]") %>' />
                                                                     on 
                                                                     <%# DataBinder.Eval(Container.DataItem, "[\"modified\"]")%>
                                                                 </td>
                                                             </tr>
                                                         </table>
                                                     </td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:repeater>
                                    </table>
                                </td>
                            </tr>
                         </table>
                     </td>
                 </tr>
            </ItemTemplate>
            <AlternatingItemTemplate>
                <tr bgcolor="F6F6F6">
                    <asp:Label ID="lblUsed" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "RRWFUsed") %>' />
                    <asp:Label ID="lblAllocated" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "RRWFAllocated") %>' />
                    <asp:Label ID="lblRRID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "ResourceRequestID") %>' />
                    <td valign="top">
                        <table cellpadding="0" cellspacing="0" border="0">
                            <tr>
                                <td nowrap><asp:Label ID="lblResource" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "RRWFUserName").ToString() == "" ? "" : DataBinder.Eval(Container.DataItem, "RRWFUserName") + " (" +  DataBinder.Eval(Container.DataItem, "RRWFUserXID") + ")" %>' ToolTip='<%# DataBinder.Eval(Container.DataItem, "RRWFUserId") %>' /></td>
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td><asp:Label ID="lblRequest" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "ReqServiceNumber") %>' ToolTip='<%# DataBinder.Eval(Container.DataItem, "RequestNumber") %>' /></td>
                            </tr>
                        </table>
                    </td>
                    <td valign="top">
                        <table cellpadding="0" cellspacing="0" border="0">
                            <tr>
                                <td colspan="4"><asp:Label ID="lblService" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "ServiceName") %>' ToolTip='<%# DataBinder.Eval(Container.DataItem, "ServiceId") %>' /></td>
                            </tr>
                            <tr>
                                <td colspan="4">&nbsp;</td>
                            </tr>
                            <tr>
                                <td align="right">Initaited: </td>
                                <td align="right"><%# DataBinder.Eval(Container.DataItem, "RRAssignedDate").ToString() == "" ? "" : DateTime.Parse(DataBinder.Eval(Container.DataItem, "RRAssignedDate").ToString()).ToShortDateString() %></td>
                                <td>&nbsp;@&nbsp;</td>
                                <td align="right"><%# DataBinder.Eval(Container.DataItem, "RRAssignedDate").ToString() == "" ? "" : DateTime.Parse(DataBinder.Eval(Container.DataItem, "RRAssignedDate").ToString()).ToShortTimeString() %></td>
                            </tr>
                            <tr>
                                <td align="right">Completed: </td>
                                <td align="right"><%# DataBinder.Eval(Container.DataItem, "RRCompletedDate").ToString() == "" ? "" : DateTime.Parse(DataBinder.Eval(Container.DataItem, "RRCompletedDate").ToString()).ToShortDateString() %></td>
                                <td><asp:Label ID="lblCompleted" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "RRCompletedDate") %>' /></td>
                                <td align="right"><%# DataBinder.Eval(Container.DataItem, "RRCompletedDate").ToString() == "" ? "" : DateTime.Parse(DataBinder.Eval(Container.DataItem, "RRCompletedDate").ToString()).ToShortTimeString() %></td>
                            </tr>
                        </table>
                    </td>
                    <td valign="top">
                        <table cellpadding="0" cellspacing="0" border="0">
                            <tr>
                                <td colspan="2"><asp:Label ID="lblProgress" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "RRWFStatus") %>' /></td>
                            </tr>
                            <tr>
                                <td nowrap>SLA: </td>
                                <td nowrap align="right" class="bold"><%# DataBinder.Eval(Container.DataItem, "ServiceSLA") %> HRs
                                <!--
                                    <%# DataBinder.Eval(Container.DataItem, "RRWFUsed") %> of <%# DataBinder.Eval(Container.DataItem, "RRWFAllocated") %> HRs
                                -->
                                </td>
                            </tr>
                            <tr>
                                <td nowrap>Time: </td>
                                <td nowrap align="right"><asp:Label ID="lblTime" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "ElapsedTime") + " HRs" %>' ToolTip='<%# DataBinder.Eval(Container.DataItem, "ServiceSLA") %>' /></td>
                            </tr>
                        </table>
                    </td>
                    <td valign="top">
                        <table cellpadding="0" cellspacing="0" border="0">
                            <tr>
                                <td nowrap><asp:Label ID="lblUpdatedBy" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "RRWFModifiedUserName").ToString() == "" ? "" : DataBinder.Eval(Container.DataItem, "RRWFModifiedUserName") + " (" +  DataBinder.Eval(Container.DataItem, "RRWFModifiedUserXID") + ")" %>' ToolTip='<%# DataBinder.Eval(Container.DataItem, "RRWFModifiedByUserId") %>' /></td>
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td><%# DataBinder.Eval(Container.DataItem, "RRWFModifiedDate") %></td>
                            </tr>
                        </table>
                    </td>
                    <td valign="top">
                        <table cellpadding="0" cellspacing="0" border="0">
                            <tr>
                                <td nowrap><%# oStatusLevel.HTML(Int32.Parse(DataBinder.Eval(Container.DataItem, "RRWFStatus").ToString())) %></td>
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td>
                                     <asp:Panel ID="pnlDelete" runat="server" Visible="false" ToolTip='<%# DataBinder.Eval(Container.DataItem, "RRWFDeleted") %>'>
                                         <div align="center">
                                             <span class="header">
                                                 <img id="imgWFDeleted" runat="server" src="/images/bigError.gif" border="0" align="absmiddle" />
                                                 Deleted!</span></div>
                                     </asp:Panel>
                                </td>
                            </tr>
                         </table>
                    </td>
                    <td valign="top">
                        <table cellpadding="0" cellspacing="0" border="0">
                            <tr>
                                <td nowrap><asp:LinkButton ID="btnVirtualView" runat="server" Text="Virtual&nbsp;View" CssClass="lookup" ToolTip='<%# DataBinder.Eval(Container.DataItem, "RRWFId") %>' /></td>
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td><asp:LinkButton ID="btnEdit" runat="server" Text="Edit" CssClass="lookup" ToolTip='<%# DataBinder.Eval(Container.DataItem, "ServiceId") %>' /></td>
                            </tr>
                        </table>
                    </td>
                </tr>
                 <tr bgcolor="F6F6F6">
                     <td width="100%" colspan="6" align="left">
                        <br />
                         <table id ="tblCurrentStatusUpdates" runat="server" cellpadding="2" cellspacing="2" border="0" width="100%" style="border: solid 1px #CCCCCC">
                             <tr>
                                 <td width="5%" valign="top"><img src='/images/<%# DataBinder.Eval(Container.DataItem, "weekly_image")%>.gif' border="0" align="absmiddle" /></td>
                                 <td width="95%" valign="Top" class="default">
                                     <table width="100%">
                                         <tr>
                                             <td colspan="2">
                                                 <asp:Label ID="lblComments" runat="server" CssClass="default" Text='<%# oFunction.FormatText(DataBinder.Eval(Container.DataItem, "RRUpdateWeeklyComments").ToString()) %>' />
                                             </td>
                                         </tr>
                                         <tr>
                                             <td colspan="2">&nbsp;</td>
                                         </tr>
                                         <tr>
                                             <td>
                                                 Posted By 
                                                 <asp:Label ID="lblResourceStatus" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "RRUpdateWeeklyUserName").ToString() == "" ? "" : DataBinder.Eval(Container.DataItem, "RRUpdateWeeklyUserName") + " (" +  DataBinder.Eval(Container.DataItem, "RRUpdateWeeklyUserXID") + ")" %>' ToolTip='<%# DataBinder.Eval(Container.DataItem, "RRUpdateWeeklyUserId") %>' />
                                                 on 
                                                 <%# DataBinder.Eval(Container.DataItem, "RRUpdateWeeklyModified") %>
                                             </td>
                                             <td align="right">
                                                 <asp:LinkButton ID="lnkbtnAdditionalComments" runat="server" Text="More comments..." Visible="true" />
                                             </td>
                                         </tr>
                                     </table>
                                 </td>
                             </tr>
                            <tr runat="server" id="trAdditionalComments" style="display:none">
                                <td colspan="7">
                                    <table width="100%" cellpadding="0" cellspacing="5" border="0">
                                        <asp:repeater id="rptServices" datasource='<%# ((DataRowView)Container.DataItem).Row.GetChildRows("relationship") %>' runat="server">
                                            <ItemTemplate>
                                                 <tr>
                                                     <td colspan="6">
                                                         <hr size="1" noshade />
                                                     </td>
                                                 </tr>
                                                <tr>
                                                     <td width="5%" valign="top"><img src='/images/<%# DataBinder.Eval(Container.DataItem, "[\"weekly_image\"]")%>.gif' border="0" align="absmiddle" /></td>
                                                     <td width="95%" valign="Top" class="default">
                                                         <table width="100%">
                                                             <tr>
                                                                 <td>
                                                                     <asp:Label ID="lblComments" runat="server" CssClass="default" Text='<%# oFunction.FormatText(DataBinder.Eval(Container.DataItem, "[\"comments\"]").ToString())%>' />
                                                                 </td>
                                                             </tr>
                                                             <tr>
                                                                 <td>&nbsp;</td>
                                                             </tr>
                                                             <tr>
                                                                 <td>
                                                                     Posted By 
                                                                     <asp:Label ID="lblResourceStatus" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "[\"resource_name\"]").ToString() == "" ? "" : DataBinder.Eval(Container.DataItem, "[\"resource_name\"]") + " (" +  DataBinder.Eval(Container.DataItem, "[\"resource_xid\"]") + ")" %>' ToolTip='<%# DataBinder.Eval(Container.DataItem, "[\"userid\"]") %>' />
                                                                     on 
                                                                     <%# DataBinder.Eval(Container.DataItem, "[\"modified\"]")%>
                                                                 </td>
                                                             </tr>
                                                         </table>
                                                     </td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:repeater>
                                    </table>
                                </td>
                            </tr>
                         </table>
                     </td>
                 </tr>
            </AlternatingItemTemplate>
        </asp:repeater>
        <tr>
            <td colspan="10">
                <asp:Label ID="lblResourceInvolvement" runat="server" CssClass="default" Visible="false" Text="<img src='/images/spacer.gif' border='0' height='1' width='25' /><img src='/images/alert.gif' border='0' align='absmiddle'> There is no resource involvement related to this project" />
            </td>
        </tr>
    </table>
 </asp:Panel>