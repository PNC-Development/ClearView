<%@ Page Language="C#" MasterPageFile="~/datapoint.Master" AutoEventWireup="True" CodeBehind="resource.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.resource" Title="Untitled Page" %>
<%@ Register Src="~/datapoint/controls/wucResourceInvolvement.ascx" TagName="wucResourceInvolvement" TagPrefix="ucResourceInvolvement" %>
<%@ Register Src="~/datapoint/controls/wucWorkflow.ascx" TagName="wucWorkflow" TagPrefix="ucWorkflow" %>

<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" runat="server">
    <asp:Panel ID="panAllow" runat="server" Visible="false">
    <table width="100%" cellpadding="0" cellspacing="5" border="0">
        <tr>
            <td rowspan="2"><img src="/images/workload48.gif" border="0" align="absmiddle" /></td>
            <td class="header" nowrap valign="bottom">Request <asp:Label ID="lblHeader" runat="server" CssClass="header" /></td>
            <td width="100%" rowspan="2" align="right">
                <table cellpadding="1" cellspacing="4" border="0">
                    <tr>
                        <td nowrap><asp:LinkButton ID="btnNew" runat="server" Text="<img src='/images/new-icon.gif' border='0' align='absmiddle' /><img src='/images/spacer.gif' border='0' width='5' height='1' />New Search" OnClick="btnNew_Click" /></td>
                        <td>|</td>
                        <td nowrap><asp:LinkButton ID="btnSave" runat="server" Text="<img src='/images/save-icon.gif' border='0' align='absmiddle' /><img src='/images/spacer.gif' border='0' width='5' height='1' />Save" OnClick="btnSave_Click" /></td>
                        <td>|</td>
                        <td nowrap><asp:LinkButton ID="btnSaveClose" runat="server" Text="<img src='/images/save-icon.gif' border='0' align='absmiddle' /><img src='/images/spacer.gif' border='0' width='5' height='1' />Save & Close" OnClick="btnSaveClose_Click" /></td>
                        <td>|</td>
                        <td nowrap><asp:LinkButton ID="btnPrint" runat="server" Text="<img src='/images/print-icon.gif' border='0' align='absmiddle' /><img src='/images/spacer.gif' border='0' width='5' height='1' />Print" /></td>
                        <td>|</td>
                        <td nowrap><asp:LinkButton ID="btnClose" runat="server" Text="<img src='/images/close-icon.gif' border='0' align='absmiddle' /><img src='/images/spacer.gif' border='0' width='5' height='1' />Close" /></td>
                    </tr>
                    <asp:Panel ID="panSave" runat="server" Visible="false">
                    <tr>
                        <td colspan="7" class="bigCheck" align="center"><img src="/images/bigCheck.gif" border="0" align="absmiddle" /> Update Successful</td>
                    </tr>
                    </asp:Panel>
                    <asp:Panel ID="panClear" runat="server" Visible="false">
                    <tr>
                        <td colspan="7" class="bigCheck" align="center"><img src="/images/bigCheck.gif" border="0" align="absmiddle" /> Request Queued</td>
                    </tr>
                    </asp:Panel>
                    <asp:Panel ID="panError" runat="server" Visible="false">
                    <tr>
                        <td colspan="7" class="bigError" align="center"><img src="/images/bigError.gif" border="0" align="absmiddle" /> <asp:Label ID="lblError" runat="server" /></td>
                    </tr>
                    </asp:Panel>
                </table>
            </td>
        </tr>
        <tr>
            <td nowrap valign="top"><asp:Label ID="lblHeaderSub" runat="server" CssClass="default" /></td>
        </tr>
    </table>
    <br />
    <%=strMenuTab1 %>
    <table width="100%" height="100%" cellpadding="10" cellspacing="5" border="0">
        <tr>
            <td valign="top">
                <div id="divMenu1" class="tabbing">
                    <div style="display:none">
                        <table width="100%" cellpadding="5" cellspacing="2" border="0">
                            <tr>
                                <td class="header" colspan="2">Submitted Information</td>
                            </tr>
                            <tr>
                                <td colspan="2">The following information was provided by <%=strRequestor %> when the service was submitted.</td>
                            </tr>
                            <tr>
                                <td colspan="2"><%=strOriginal %></td>
                            </tr>
                            <tr id="trAdmin" runat="server" visible="false">
                                <td valign="top"><b>Assign To:</b></td>
                                <td>
                                <table cellpadding="0" cellspacing="0" border="0">
                                    <tr>
                                        <td><asp:TextBox ID="txtUser" runat="server" Width="300" CssClass="default" /></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div id="divUser" runat="server" style="overflow:hidden; position:absolute; display:none; background-color:#FFFFFF; border:solid 1px #CCCCCC">
                                                <asp:ListBox ID="lstUser" runat="server" CssClass="default" />
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                                <img src="/images/alert.gif" border="0" align="absmiddle" /> This option is only available for administrators!<br />
                                <asp:CheckBox ID="chkEmail" runat="server" Text="Do NOT send email to assignee" />
                                </td>
                            </tr>
                        </table>
                    </div>
                   <%-- <div style="display:none">
                        <table width="100%" cellpadding="5" cellspacing="2" border="0">
                            <tr>
                                <td nowrap class="header">Resource Assignment(s)</td>
                                <td align="right" nowrap></td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <table width="100%" cellpadding="6" cellspacing="0" border="0" style="border:solid 1px #CCCCCC">
                                        <tr bgcolor="#EEEEEE">
                                            <td></td>
                                            <td nowrap><b>Resource</b></td>
                                            <td nowrap><b>Status</b></td>
                                            <td nowrap><b>Progress</b></td>
                                            <td nowrap><b>Last Updated</b></td>
                                            <td></td>
                                        </tr>
                                        <asp:repeater ID="rptAssignments" runat="server">
                                            <ItemTemplate>
                                                <tr>
                                                    <td style='border-left:solid 4px <%# DataBinder.Eval(Container.DataItem, "deleted").ToString() == "1" ? "#FF0000" : "#FFFFFF" %>' valign="top" rowspan="2" title='WorkflowID: <%# DataBinder.Eval(Container.DataItem, "id") %>'><img src='/frame/picture.aspx?xid=<%#DataBinder.Eval(Container.DataItem, "resource_xid") %>' align='absmiddle' border='0' style='height:90px;width:90px;border-width:0px;border:solid 1px #999999;' /></td>
                                                    <td width="20%" nowrap title='UserID: <%# DataBinder.Eval(Container.DataItem, "userid") %>'><a class="lookup" href='javascript:void(0);' onclick="OpenWindow('PROFILE','?userid=<%# DataBinder.Eval(Container.DataItem, "userid") %>');"><%# DataBinder.Eval(Container.DataItem, "resource_name") + " (" + DataBinder.Eval(Container.DataItem, "resource_xid") + ")"%></a></td>
                                                    <td width="10%" nowrap><asp:Label ID="lblStatusR" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "status") %>' /></td>
                                                    <td width="20%" nowrap><asp:Label ID="lblProgress" runat="server" CssClass="default" Text='<%# double.Parse(DataBinder.Eval(Container.DataItem, "used").ToString()) / double.Parse(DataBinder.Eval(Container.DataItem, "allocated").ToString()) * 100.00 %>' /></td>
                                                    
                                                    <td width="20%">
                                                        <table width="100%"> 
                                                        <tr>
                                                         <td nowrap title='UserID: <%# DataBinder.Eval(Container.DataItem, "userid") %>'><a class="lookup" href='javascript:void(0);' onclick="OpenWindow('PROFILE','?userid=<%# DataBinder.Eval(Container.DataItem, "modifiedby") %>');"><%# DataBinder.Eval(Container.DataItem, "modify_name") +(DataBinder.Eval(Container.DataItem, "modify_xid")!=DBNull.Value? " (" + DataBinder.Eval(Container.DataItem, "modify_xid") + ")":"")%></a></td>
                                                        </tr><tr>
                                                        <td width="20%" nowrap><%# DataBinder.Eval(Container.DataItem, "modified").ToString() %></td>
                                                        </tr>
                                                        </table>
                                                    </td>
                                                        
                                                    
                                                    <td width="10%" nowrap>
                                                        <asp:Panel ID="panEdit" runat="server" Visible="false">
                                                         <asp:Button ID="btnEdit" runat="server" Text="Edit" CssClass="default" Width="75" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' />
                                                            <br />
                                                            <asp:Button ID="btnLogin" runat="server" Text="Virtual View" CssClass="default" Width="100" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' />
                                                        </asp:Panel>
                                                        <asp:Panel ID="panDelete" runat="server" Visible="false" ToolTip='<%# DataBinder.Eval(Container.DataItem, "deleted").ToString() %>'>
                                                            <div align="center"><span class="header"><img src="/images/bigError.gif" border="0" align="absmiddle" /> Deleted!</span></div>
                                                        </asp:Panel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="6">
                                                        <table cellpadding="0" cellspacing="5" border="0">
                                                            <tr>
                                                                <td rowspan="3" valign="top"><img src='/images/<%# DataBinder.Eval(Container.DataItem, "weekly_image").ToString()%>.gif' border='0' align='absmiddle' /></td>
                                                                <td width="100%" valign="bottom"><b><%# DataBinder.Eval(Container.DataItem, "weekly_modified")%></b></td>
                                                            </tr>
                                                            <tr>
                                                                <td width="100%" valign="top"><%# DataBinder.Eval(Container.DataItem, "weekly_comments")%></td>
                                                            </tr>
                                                            <tr>
                                                                <td width="100%" valign="top"><asp:LinkButton ID="btnMore" runat="server" Text="Click here to view additional comments" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' Visible="false" /></td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr id='<%# "div_" + DataBinder.Eval(Container.DataItem, "id").ToString() %>' style="display:none">
                                                    <td colspan="7">
                                                        <table width="100%" cellpadding="0" cellspacing="5" border="0">
                                                            <asp:repeater id="rptServices" datasource='<%# ((DataRowView)Container.DataItem).Row.GetChildRows("relationship") %>' runat="server">
                                                                <ItemTemplate>
                                                                    <tr>
                                                                        <td>
                                                                        <table width="100%"><tr>
                                                                        <td width="10%" rowspan="3" valign="top" title ="Problem"><img src='/images/<%# DataBinder.Eval(Container.DataItem, "[\"weekly_image\"]").ToString()%>.gif' border='0' align='absmiddle' /></td>
                                                                        <td width="90%" valign="bottom"><b><%# DataBinder.Eval(Container.DataItem, "[\"weekly_modified\"]")%></b></td>
                                                                        </tr>
                                                                        </table>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td width="100%" valign="top"><%# DataBinder.Eval(Container.DataItem, "[\"weekly_comments\"]")%></td>
                                                                    </tr>
                                                                </ItemTemplate>
                                                            </asp:repeater>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                            <AlternatingItemTemplate>
                                                <tr bgcolor="#F6F6F6">
                                                    <td style='border-left:solid 4px <%# DataBinder.Eval(Container.DataItem, "deleted").ToString() == "1" ? "#FF0000" : "#FFFFFF" %>' valign="top" rowspan="2" title='WorkflowID: <%# DataBinder.Eval(Container.DataItem, "id") %>'><img src='/frame/picture.aspx?xid=<%#DataBinder.Eval(Container.DataItem, "resource_xid") %>' align='absmiddle' border='0' style='height:90px;width:90px;border-width:0px;border:solid 1px #999999;' /></td>
                                                    <td width="20%" nowrap title='UserID: <%# DataBinder.Eval(Container.DataItem, "userid") %>'><a class="lookup" href='javascript:void(0);' onclick="OpenWindow('PROFILE','?userid=<%# DataBinder.Eval(Container.DataItem, "userid") %>');"><%# DataBinder.Eval(Container.DataItem, "resource_name") + " (" + DataBinder.Eval(Container.DataItem, "resource_xid") + ")"%></a></td>
                                                    <td width="10%" nowrap><asp:Label ID="lblStatusR" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "status") %>' /></td>
                                                    <td width="20%" nowrap><asp:Label ID="lblProgress" runat="server" CssClass="default" Text='<%# double.Parse(DataBinder.Eval(Container.DataItem, "used").ToString()) / double.Parse(DataBinder.Eval(Container.DataItem, "allocated").ToString()) * 100.00 %>' /></td>
                                                    <td width="20%">
                                                        <table width="100%"> 
                                                        <tr>
                                                         <td nowrap title='UserID: <%# DataBinder.Eval(Container.DataItem, "userid") %>'><a class="lookup" href='javascript:void(0);' onclick="OpenWindow('PROFILE','?userid=<%# DataBinder.Eval(Container.DataItem, "modifiedby") %>');"><%# DataBinder.Eval(Container.DataItem, "modify_name") +(DataBinder.Eval(Container.DataItem, "modify_xid")!=DBNull.Value ? " (" + DataBinder.Eval(Container.DataItem, "modify_xid") + ")":"")%></a></td>
                                                        </tr><tr>
                                                        <td width="20%" nowrap><%# DataBinder.Eval(Container.DataItem, "modified").ToString() %></td>
                                                        </tr>
                                                        </table>
                                                    </td>
                                                    <td width="10%" nowrap>
                                                        <asp:Panel ID="panEdit" runat="server" Visible="false">
                                                        
                                                            <asp:Button ID="btnEdit" runat="server" Text="Edit" CssClass="default" Width="75" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' />
                                                            <br />
                                                            <asp:Button ID="btnLogin" runat="server" Text="Virtual View" CssClass="default" Width="100" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' />
                                                        </asp:Panel>
                                                        <asp:Panel ID="panDelete" runat="server" Visible="false" ToolTip='<%# DataBinder.Eval(Container.DataItem, "deleted").ToString() %>'>
                                                            <div align="center"><span class="header"><img src="/images/bigError.gif" border="0" align="absmiddle" /> Deleted!</span></div>
                                                        </asp:Panel>
                                                    </td>
                                                </tr>
                                                <tr bgcolor="#F6F6F6">
                                                    <td colspan="6">
                                                        <table cellpadding="0" cellspacing="5" border="0">
                                                            <tr>
                                                                <td rowspan="3" valign="top"><img src='/images/<%# DataBinder.Eval(Container.DataItem, "weekly_image").ToString()%>.gif' border='0' align='absmiddle' /></td>
                                                                <td width="100%" valign="bottom"><b><%# DataBinder.Eval(Container.DataItem, "weekly_modified")%></b></td>
                                                            </tr>
                                                            <tr>
                                                                <td width="100%" valign="top"><%# DataBinder.Eval(Container.DataItem, "weekly_comments")%></td>
                                                            </tr>
                                                            <tr>
                                                                <td width="100%" valign="top"><asp:LinkButton ID="btnMore" runat="server" Text="Click here to view additional comments" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' Visible="false" /></td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr bgcolor="#F6F6F6" id='<%# "div_" + DataBinder.Eval(Container.DataItem, "id").ToString() %>' style="display:none">
                                                    <td colspan="7">
                                                        <table width="100%" cellpadding="0" cellspacing="5" border="0">
                                                            <asp:repeater id="rptServices" datasource='<%# ((DataRowView)Container.DataItem).Row.GetChildRows("relationship") %>' runat="server">
                                                                <ItemTemplate>
                                                                    <tr>
                                                                        <td>
                                                                        <table width="100%"><tr>
                                                                        <td width="10%" rowspan="3" valign="top" title ="Problem"><img src='/images/<%# DataBinder.Eval(Container.DataItem, "[\"weekly_image\"]").ToString()%>.gif' border='0' align='absmiddle' /></td>
                                                                        <td width="90%" valign="bottom"><b><%# DataBinder.Eval(Container.DataItem, "[\"weekly_modified\"]")%></b></td>
                                                                        </tr>
                                                                        </table>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td width="100%" valign="top"><%# DataBinder.Eval(Container.DataItem, "[\"weekly_comments\"]")%></td>
                                                                    </tr>
                                                                </ItemTemplate>
                                                            </asp:repeater>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </AlternatingItemTemplate>
                                        </asp:repeater>
                                        <tr>
                                            <td colspan="5"><asp:Label ID="lblAssignments" runat="server" CssClass="default" Visible="false" Text="<img src='/images/spacer.gif' border='0' height='1' width='25' /><img src='/images/alert.gif' border='0' align='absmiddle'> There are no resources assigned to this request" /></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </div>--%>
                   <div  id="divWorkflow" style="display:none">
                      <ucWorkflow:wucWorkflow ID="ucWorkflow" runat="server" />
                   </div>
                   <div  id="divResourceInvolvement" style="display:none">
                      <ucResourceInvolvement:wucResourceInvolvement ID="ucResourceInvolvement" runat="server" />
                    </div>
                    <div id="divDetail" runat="server" visible="false" style="display:none">
                        <table width="100%" cellpadding="5" cellspacing="2" border="0">
                            <tr>
                                <td class="header" colspan="4">Request Detail:&nbsp;&nbsp;<asp:Label ID="lblRequestID" runat="server" CssClass="lookup" /></td>
                            </tr>
                            <tr>
                                <td class="bigAlert" colspan="4"><img src="/images/bigAlert.gif" align="absmiddle" border="0" /> <b>NOTE:</b> This page is only available for administrators!</td>
                            </tr>
                            <tr>
                                <td nowrap><asp:Label ID="fldRequest" runat="server" CssClass="default" Text="Request ID:" /></td>
                                <td width="100%" colspan="3"><asp:Label ID="lblRequest" runat="server" CssClass="default" /><asp:TextBox ID="txtRequest" runat="server" CssClass="default" MaxLength="10" Width="100" /></td>
                            </tr>
                            <tr>
                                <td nowrap><asp:Label ID="fldCreated" runat="server" CssClass="default" Text="Created On:" /></td>
                                <td width="100%" colspan="3"><asp:Label ID="lblCreated" runat="server" CssClass="default" /><asp:TextBox ID="txtCreated" runat="server" CssClass="default" MaxLength="50" Width="200" /></td>
                            </tr>
                            <tr>
                                <td nowrap><asp:Label ID="fldDepartment" runat="server" CssClass="default" Text="Department:" /></td>
                                <td width="100%" colspan="3"><asp:Label ID="lblDepartment" runat="server" CssClass="default" /><asp:TextBox ID="txtDepartment" runat="server" CssClass="default" MaxLength="100" Width="300" /></td>
                            </tr>
                            <tr>
                                <td nowrap><asp:Label ID="fldService" runat="server" CssClass="default" Text="Service:" /></td>
                                <td width="100%" colspan="3"><asp:Label ID="lblService" runat="server" CssClass="default" /><asp:TextBox ID="txtService" runat="server" CssClass="default" MaxLength="100" Width="300" /></td>
                            </tr>
                            <tr>
                                <td nowrap><asp:Label ID="fldOwner" runat="server" CssClass="default" Text="Service Owner(s):" /></td>
                                <td width="100%" colspan="3"><asp:Label ID="lblOwner" runat="server" CssClass="default" /><asp:TextBox ID="txtOwner" runat="server" CssClass="default" MaxLength="100" Width="300" /></td>
                            </tr>
                            <tr>
                                <td nowrap><asp:Label ID="fldName" runat="server" CssClass="default" Text="Name:" /></td>
                                <td width="100%" colspan="3"><asp:Label ID="lblName" runat="server" CssClass="default" /><asp:TextBox ID="txtName" runat="server" CssClass="default" MaxLength="200" Width="500" /></td>
                            </tr>
                            <tr>
                                <td nowrap><asp:Label ID="fldDevices" runat="server" CssClass="default" Text="Device Count:" /></td>
                                <td nowrap><asp:Label ID="lblDevices" runat="server" CssClass="default" /><asp:TextBox ID="txtDevices" runat="server" CssClass="default" MaxLength="4" Width="50" /></td>
                                <td width="100%" rowspan="2">
                                    <asp:Panel ID="panDynamic" runat="server" Visible="false">
                                        <table cellpadding="2" cellspacing="2" border="0">
                                            <tr>
                                                <td nowrap>
                                                    <table cellpadding="0" cellspacing="0" border="0">
                                                        <tr>
                                                            <td width="100%" background="/images/box_top.gif"></td>
                                                            <td><img src="/images/box_top_right.gif" border="0" width="6" height="6"></td>
                                                        </tr>
                                                        <tr>
                                                            <td width="100%"><img src="/images/spacer.gif" width="6" height="50"></td>
                                                            <td background="/images/table_right.gif"><img src="/images/table_right.gif" width="5" height="10"></td>
                                                        </tr>
                                                        <tr>
                                                            <td width="100%" background="/images/box_bottom.gif"></td>
                                                            <td><img src="/images/box_bottom_right.gif" border="0" width="6" height="6"></td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td width="100%">
                                                    <table cellpadding="2" cellspacing="2" border="0">
                                                        <tr>
                                                            <td rowspan="2" valign="top"><img src="/images/bigAlert.gif" border="0" align="absmiddle" /></td>
                                                            <td><b>NOTE:</b> The <u>allocated hours</u> are dynamically calculated based on the <u>device count</u> for this service.</td>
                                                        </tr>
                                                        <tr>
                                                            <td>To modify this relationship, please update the tasks of this service in Service Editor (under &quot;Service Requests&quot; | &quot;Service Editor&quot;).</td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td nowrap><asp:Label ID="fldAllocated" runat="server" CssClass="default" Text="Allocated:" /></td>
                                <td nowrap><asp:Label ID="lblAllocated" runat="server" CssClass="default" /><asp:TextBox ID="txtAllocated" runat="server" CssClass="default" MaxLength="8" Width="75" /> HRs</td>
                            </tr>
                            <tr>
                                <td nowrap><asp:Label ID="fldAccepted" runat="server" CssClass="default" Text="Accepted:" /></td>
                                <td width="100%" colspan="3"><asp:Label ID="lblAccepted" runat="server" CssClass="default" /><asp:DropDownList ID="ddlAccepted" runat="server" CssClass="default" Width="200" /></td>
                            </tr>
                            <tr>
                                <td nowrap><asp:Label ID="fldStatus" runat="server" CssClass="default" Text="Status:" /></td>
                                <td width="100%" colspan="3"><asp:Label ID="lblStatus" runat="server" CssClass="default" /><asp:DropDownList ID="ddlStatus" runat="server" CssClass="default" Width="200" /></td>
                            </tr>
                            <tr>
                                <td nowrap><asp:Label ID="fldAssignedBy" runat="server" CssClass="default" Text="Assigned By:" /></td>
                                <td width="100%" colspan="3"><asp:Label ID="lblAssignedBy" runat="server" CssClass="default" />
                                    <table cellpadding="0" cellspacing="0" border="0">
                                        <tr>
                                            <td><asp:TextBox ID="txtAssignedBy" runat="server" Width="400" CssClass="default" /></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div id="divAssignedBy" runat="server" style="overflow:hidden; position:absolute; display:none; background-color:#FFFFFF; border:solid 1px #CCCCCC">
                                                    <asp:ListBox ID="lstAssignedBy" runat="server" CssClass="default" />
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td nowrap><asp:Label ID="fldAssignedOn" runat="server" CssClass="default" Text="Assigned On:" /></td>
                                <td width="100%" colspan="3"><asp:Label ID="lblAssignedOn" runat="server" CssClass="default" /><asp:TextBox ID="txtAssignedOn" runat="server" CssClass="default" MaxLength="50" Width="200" /> <asp:Button ID="btnAssign" runat="server" CssClass="default" Width="100" Text="Clear" OnClick="btnAssign_Click" /></td>
                            </tr>
                            <tr>
                                <td nowrap><asp:Label ID="fldReason" runat="server" CssClass="default" Text="Rejection Reason:" /></td>
                                <td width="100%" colspan="3"><asp:Label ID="lblReason" runat="server" CssClass="default" /><asp:TextBox ID="txtReason" runat="server" CssClass="default" TextMode="MultiLine" Width="500" Rows="5" /></td>
                            </tr>
                            <asp:Panel ID="panDeleted" runat="server" Visible="false">
                            <tr>
                                <td colspan="4">&nbsp;</td>
                            </tr>
                            <tr>
                                <td nowrap></td>
                                <td width="100%" colspan="3" class="header"><img src="/images/bigError.gif" border="0" align="absmiddle" /> This request was deleted!</td>
                            </tr>
                            </asp:Panel>
                        </table>
                    </div>
                   
                    <div id="divResults" runat="server" visible="false" style="display:none">
                        <table width="100%" cellpadding="5" cellspacing="2" border="0">
                            <tr>
                                <td class="header" colspan="2">Service Request Result(s)</td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <table width="100%" cellpadding="6" cellspacing="0" border="0" style="border:solid 1px #CCCCCC">
                                        <tr bgcolor="#EEEEEE">
                                            <td nowrap><b>Result</b></td>
                                            <td nowrap><b>Last Updated On</b></td>
                                        </tr>
                                        <asp:repeater ID="rptResults" runat="server">
                                            <ItemTemplate>
                                                <tr>
                                                    <td width="100%"><%# DataBinder.Eval(Container.DataItem, "result").ToString() %></td>
                                                    <td nowrap><%# DataBinder.Eval(Container.DataItem, "modified").ToString() %></td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:repeater>
                                        <tr>
                                            <td colspan="2"><asp:Label ID="lblResults" runat="server" CssClass="default" Visible="false" Text="<img src='/images/spacer.gif' border='0' height='1' width='25' /><img src='/images/alert.gif' border='0' align='absmiddle'> There are no items" /></td>
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
        <br />
            <table width="100%" cellpadding="0" cellspacing="5" border="0">
                <tr>
                    <td rowspan="2"><img src="/images/ico_error.gif" border="0" align="absmiddle" /></td>
                    <td class="header" width="100%" valign="bottom">Access Denied</td>
                </tr>
                <tr>
                    <td width="100%" valign="top">You do not have sufficient permission to view this page.</td>
                </tr>
                <tr>
                    <td colspan="2">&nbsp;</td>
                </tr>
                <tr>
                    <td></td>
                    <td width="100%">If you think you should have rights to view it, please contact your ClearView administrator.</td>
                </tr>
            </table>
        <p>&nbsp;</p>
    </asp:Panel>
<asp:HiddenField ID="hdnTab" runat="server" />
<asp:HiddenField ID="hdnAssignedBy" runat="server" />
<asp:HiddenField ID="hdnUser" runat="server" />
</asp:Content>
