<%@ Page Language="C#" MasterPageFile="~/datapoint.Master" AutoEventWireup="true" CodeBehind="requestOldest.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.requestOldest" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" runat="server">
    <asp:Panel ID="panAllow" runat="server" Visible="false">
    <table width="100%" cellpadding="0" cellspacing="5" border="0">
        <tr>
            <td rowspan="2"><img src="/images/workload48.gif" border="0" align="absmiddle" /></td>
            <td class="header" nowrap valign="bottom"><asp:Label ID="lblHeader" runat="server" CssClass="header" /></td>
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
                    <asp:Panel ID="panCancel" runat="server" Visible="false">
                    <tr>
                        <td colspan="7" class="bigCheck" align="center"><img src="/images/bigCheck.gif" border="0" align="absmiddle" /> Service Cancelled</td>
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
                                <td class="header" colspan="2">Request Information&nbsp;&nbsp;<asp:Label ID="lblRequestID" runat="server" CssClass="default" /></td>
                            </tr>
                            <tr>
                                <td nowrap><asp:Label ID="fldRequestBy" runat="server" CssClass="default" Text="Requested By:" /></td>
                                <td width="100%"><asp:Label ID="lblRequestBy" runat="server" CssClass="default" />
                                    <table cellpadding="0" cellspacing="0" border="0">
                                        <tr>
                                            <td><asp:TextBox ID="txtRequestBy" runat="server" Width="400" CssClass="default" /></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div id="divRequestBy" runat="server" style="overflow:hidden; position:absolute; display:none; background-color:#FFFFFF; border:solid 1px #CCCCCC">
                                                    <asp:ListBox ID="lstRequestBy" runat="server" CssClass="default" />
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td nowrap><asp:Label ID="fldRequestOn" runat="server" CssClass="default" Text="Requested On:" /></td>
                                <td width="100%"><asp:Label ID="lblRequestOn" runat="server" CssClass="default" /><asp:TextBox ID="txtRequestOn" runat="server" CssClass="default" MaxLength="50" Width="200" /> <asp:ImageButton ID="imgRequestOn" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calendar.gif" Visible="false" /></td>
                            </tr>
                            <tr>
                                <td nowrap><asp:Label ID="fldProjectNumber" runat="server" CssClass="default" Text="Project Number:" /></td>
                                <td width="100%"><asp:Label ID="lblProjectNumber" runat="server" CssClass="default" /><asp:TextBox ID="txtProjectNumber" runat="server" CssClass="default" MaxLength="30" Width="200" /> <asp:Button ID="btnProjectName" runat="server" CssClass="default" Width="75" Text="Lookup" Visible="false" /></td>
                            </tr>
                            <tr>
                                <td nowrap><asp:Label ID="fldProjectName" runat="server" CssClass="default" Text="Project Name:" /></td>
                                <td width="100%"><asp:Label ID="lblProjectName" runat="server" CssClass="default" /><asp:TextBox ID="txtProjectName" runat="server" CssClass="default" MaxLength="50" Width="300" /></td>
                            </tr>
                            <tr>
                                <td nowrap><asp:Label ID="fldStatus" runat="server" CssClass="default" Text="Request Status:" /></td>
                                <td width="100%"><asp:Label ID="lblStatus" runat="server" CssClass="default" /><asp:DropDownList ID="ddlStatus" runat="server" CssClass="default" Width="200" /></td>
                            </tr>
                            <tr>
                                <td colspan="2">&nbsp;</td>
                            </tr>
                            <tr>
                                <td nowrap></td>
                                <td width="100%"><asp:LinkButton ID="lnkView" runat="server" CssClass="lookup" Text="View Service Request Summary" /></td>
                            </tr>
                            <asp:Panel ID="panDeleted" runat="server" Visible="false">
                            <tr>
                                <td colspan="2">&nbsp;</td>
                            </tr>
                            <tr>
                                <td nowrap></td>
                                <td width="100%" class="header"><img src="/images/bigError.gif" border="0" align="absmiddle" /> This request was deleted!</td>
                            </tr>
                            </asp:Panel>
                        </table>
                    </div>
                    <div style="display:none">
                        <table width="100%" cellpadding="5" cellspacing="2" border="0">
                            <tr>
                                <td nowrap class="header">Service Request(s)</td>
                                <td align="right" nowrap><img src="/images/bigAlert.gif" border="0" align="absmiddle" /><b>NOTE:</b> Click the name of the service to view the status of the selected service.</td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <table width="100%" cellpadding="4" cellspacing="0" border="0" style="border:solid 1px #CCCCCC">
                                        <tr bgcolor="#EEEEEE">
                                            <td></td>
                                            <td nowrap align="center"><b>Form #</b></td>
                                            <td nowrap><b>Service Name</b></td>
                                            <td nowrap><b>Progress</b></td>
                                            <td nowrap><b>Requestor Status</b></td>
                                            <td nowrap><b>Submitted</b></td>
                                            <td nowrap><b>Last Updated</b></td>
                                            <td></td>
                                        </tr>
                                        <asp:repeater ID="rptServiceRequests" runat="server">
                                            <ItemTemplate>
                                                <tr>
                                                    <td valign="top"><asp:Button ID="btnView" runat="server" Text="View" CssClass="default" Width="50" /></td>
                                                    <td valign="top" width="5%" align="center" title='ServiceID: <%# DataBinder.Eval(Container.DataItem, "serviceid")%>'><asp:LinkButton ID="btnNumber" runat="server" CssClass="lookup" Text='<%# DataBinder.Eval(Container.DataItem, "number")%>' CommandArgument='<%# DataBinder.Eval(Container.DataItem, "serviceid")%>' /></td>
                                                    <td valign="top" width="35%" nowrap title='Click here to view the details of this service'><asp:Label ID="lblDetails" runat="server" CssClass="default" Text='<%#DataBinder.Eval(Container.DataItem, "name").ToString() %>' /></td>
                                                    <td valign="top" width="20%" nowrap><asp:Label ID="lblProgress" runat="server" CssClass="default" Text='<%#DataBinder.Eval(Container.DataItem, "resourceid").ToString() %>' /></td>
                                                    <td valign="top" width="10%" nowrap><asp:Label ID="lblRequestStatus" runat="server" CssClass="default" Text='<%# (DataBinder.Eval(Container.DataItem, "deleted").ToString() == "1" ? "Deleted By Requestor" : (DataBinder.Eval(Container.DataItem, "cancelled").ToString() == "" ? (DataBinder.Eval(Container.DataItem, "done").ToString() == "0" ? "Awaiting Submission By Requestor" : "Submitted") : "Cancelled By Requestor"))%>' /></td>
                                                    <td valign="top" width="15%" nowrap><%# DataBinder.Eval(Container.DataItem, "submitted").ToString()%></td>
                                                    <td valign="top" width="15%" nowrap><%# (DataBinder.Eval(Container.DataItem, "cancelled").ToString() == "" ? DataBinder.Eval(Container.DataItem, "modified").ToString() : DataBinder.Eval(Container.DataItem, "cancelled").ToString())%></td>
                                                    <td valign="top"><asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="default" Width="75" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "serviceid")%>' CommandName='<%# DataBinder.Eval(Container.DataItem, "number")%>' OnClick="btnCancel_Click" /></td>
                                                            <asp:Label ID="lblOnDemand" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container.DataItem, "ondemand").ToString() %>' />
                                                            <asp:Label ID="lblAutomate" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container.DataItem, "automate").ToString() %>' />
                                                </tr>
                                                <asp:repeater id="rptWorkflow" datasource='<%# ((DataRowView)Container.DataItem).Row.GetChildRows("relationship") %>' runat="server">
                                                    <ItemTemplate>
                                                        <tr>
                                                            <td valign="top"><asp:Button ID="btnView" runat="server" Text="View" CssClass="default" Width="50" title='ResourceID: <%# DataBinder.Eval(Container.DataItem, "[\"resourceid\"]") %>' /></td>
                                                            <td valign="top" width="5%" align="right" title='ServiceID: <%# DataBinder.Eval(Container.DataItem, "[\"serviceid\"]")%>'><img src="/images/down_right.gif" border="0" align="absmiddle" /></td>
                                                            <td valign="top" width="35%" nowrap title='Click here to view the details of this service'><asp:Label ID="lblDetails" runat="server" CssClass="default" Text='<%#DataBinder.Eval(Container.DataItem, "[\"name\"]").ToString() %>' /></td>
                                                            <td valign="top" width="20%" nowrap><asp:Label ID="lblProgress" runat="server" CssClass="default" Text='<%#DataBinder.Eval(Container.DataItem, "[\"resourceid\"]").ToString() %>' /></td>
                                                            <td valign="top" width="10%" nowrap><asp:Label ID="lblRequestStatus" runat="server" CssClass="default" Text='<%# (DataBinder.Eval(Container.DataItem, "[\"deleted\"]").ToString() == "1" ? "Deleted By Requestor" : (DataBinder.Eval(Container.DataItem, "[\"cancelled\"]").ToString() == "" ? (DataBinder.Eval(Container.DataItem, "[\"going\"]").ToString() == "0" ? "Queued" : (DataBinder.Eval(Container.DataItem, "[\"done\"]").ToString() == "0" ? "Awaiting Submission By Requestor" : "Submitted")) : "Cancelled By Requestor"))%>' /></td>
                                                            <td valign="top" width="15%" nowrap><%# (DataBinder.Eval(Container.DataItem, "[\"going\"]").ToString() == "0" ? "---" : DataBinder.Eval(Container.DataItem, "[\"submitted\"]").ToString())%></td>
                                                            <td valign="top" width="15%" nowrap><%# (DataBinder.Eval(Container.DataItem, "[\"cancelled\"]").ToString() == "" ? DataBinder.Eval(Container.DataItem, "[\"modified\"]").ToString() : DataBinder.Eval(Container.DataItem, "[\"cancelled\"]").ToString())%></td>
                                                        </tr>
                                                    </ItemTemplate>
                                                </asp:repeater>
                                            </ItemTemplate>
                                            <AlternatingItemTemplate>
                                                <tr bgcolor="#F6F6F6">
                                                    <td valign="top"><asp:Button ID="btnView" runat="server" Text="View" CssClass="default" Width="50" title='ResourceID: <%# DataBinder.Eval(Container.DataItem, "resourceid") %>' /></td>
                                                    <td valign="top" width="5%" align="center" title='ServiceID: <%# DataBinder.Eval(Container.DataItem, "serviceid")%>'><asp:LinkButton ID="btnNumber" runat="server" CssClass="lookup" Text='<%# DataBinder.Eval(Container.DataItem, "number")%>' CommandArgument='<%# DataBinder.Eval(Container.DataItem, "serviceid")%>' /></td>
                                                    <td valign="top" width="35%" nowrap title='Click here to view the details of this service'><asp:Label ID="lblDetails" runat="server" CssClass="default" Text='<%#DataBinder.Eval(Container.DataItem, "name").ToString() %>' /></td>
                                                    <td valign="top" width="20%" nowrap><asp:Label ID="lblProgress" runat="server" CssClass="default" Text='<%#DataBinder.Eval(Container.DataItem, "resourceid").ToString() %>' /></td>
                                                    <td valign="top" width="10%" nowrap><asp:Label ID="lblRequestStatus" runat="server" CssClass="default" Text='<%# (DataBinder.Eval(Container.DataItem, "deleted").ToString() == "1" ? "Deleted By Requestor" : (DataBinder.Eval(Container.DataItem, "cancelled").ToString() == "" ? (DataBinder.Eval(Container.DataItem, "done").ToString() == "0" ? "Awaiting Submission By Requestor" : "Submitted") : "Cancelled By Requestor"))%>' /></td>
                                                    <td valign="top" width="15%" nowrap><%# DataBinder.Eval(Container.DataItem, "submitted").ToString()%></td>
                                                    <td valign="top" width="15%" nowrap><%# (DataBinder.Eval(Container.DataItem, "cancelled").ToString() == "" ? DataBinder.Eval(Container.DataItem, "modified").ToString() : DataBinder.Eval(Container.DataItem, "cancelled").ToString())%></td>
                                                    <td valign="top"><asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="default" Width="75" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "serviceid")%>' CommandName='<%# DataBinder.Eval(Container.DataItem, "number")%>' OnClick="btnCancel_Click" /></td>
                                                            <asp:Label ID="lblOnDemand" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container.DataItem, "ondemand").ToString() %>' />
                                                            <asp:Label ID="lblAutomate" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container.DataItem, "automate").ToString() %>' />
                                                </tr>
                                                <asp:repeater id="rptWorkflow" datasource='<%# ((DataRowView)Container.DataItem).Row.GetChildRows("relationship") %>' runat="server">
                                                    <ItemTemplate>
                                                        <tr>
                                                            <td valign="top"><asp:Button ID="btnView" runat="server" Text="View" CssClass="default" Width="50" title='ResourceID: <%# DataBinder.Eval(Container.DataItem, "[\"resourceid\"]") %>' /></td>
                                                            <td valign="top" width="5%" align="right" title='ServiceID: <%# DataBinder.Eval(Container.DataItem, "[\"serviceid\"]")%>'><img src="/images/down_right.gif" border="0" align="absmiddle" /></td>
                                                            <td valign="top" width="35%" nowrap title='Click here to view the details of this service'><asp:Label ID="lblDetails" runat="server" CssClass="default" Text='<%#DataBinder.Eval(Container.DataItem, "[\"name\"]").ToString() %>' /></td>
                                                            <td valign="top" width="20%" nowrap><asp:Label ID="lblProgress" runat="server" CssClass="default" Text='<%#DataBinder.Eval(Container.DataItem, "[\"resourceid\"]").ToString() %>' /></td>
                                                            <td valign="top" width="10%" nowrap><asp:Label ID="lblRequestStatus" runat="server" CssClass="default" Text='<%# (DataBinder.Eval(Container.DataItem, "[\"deleted\"]").ToString() == "1" ? "Deleted By Requestor" : (DataBinder.Eval(Container.DataItem, "[\"cancelled\"]").ToString() == "" ? (DataBinder.Eval(Container.DataItem, "[\"going\"]").ToString() == "0" ? "Queued" : (DataBinder.Eval(Container.DataItem, "[\"done\"]").ToString() == "0" ? "Awaiting Submission By Requestor" : "Submitted")) : "Cancelled By Requestor"))%>' /></td>
                                                            <td valign="top" width="15%" nowrap><%# (DataBinder.Eval(Container.DataItem, "[\"going\"]").ToString() == "0" ? "---" : DataBinder.Eval(Container.DataItem, "[\"submitted\"]").ToString())%></td>
                                                            <td valign="top" width="15%" nowrap><%# (DataBinder.Eval(Container.DataItem, "[\"cancelled\"]").ToString() == "" ? DataBinder.Eval(Container.DataItem, "[\"modified\"]").ToString() : DataBinder.Eval(Container.DataItem, "[\"cancelled\"]").ToString())%></td>
                                                        </tr>
                                                    </ItemTemplate>
                                                </asp:repeater>
                                            </AlternatingItemTemplate>
                                        </asp:repeater>
                                        <tr>
                                            <td colspan="5"><asp:Label ID="lblServiceRequests" runat="server" CssClass="default" Visible="false" Text="<img src='/images/spacer.gif' border='0' height='1' width='25' /><img src='/images/alert.gif' border='0' align='absmiddle'> There are no services related to this request" /></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div style="display:none">
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
                                            <td colspan="2"><asp:Label ID="lblResults" runat="server" CssClass="default" Visible="false" Text="<img src='/images/spacer.gif' border='0' height='1' width='25' /><img src='/images/alert.gif' border='0' align='absmiddle'> There are no automated results for this request" /></td>
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
<asp:HiddenField ID="hdnRequestBy" runat="server" />
</asp:Content>
