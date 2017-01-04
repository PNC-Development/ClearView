<%@ Page Language="C#" MasterPageFile="~/datapoint.Master" AutoEventWireup="true" CodeBehind="datapoint_service_searchOld.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.datapoint_service_searchOld" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" runat="server">
<script type="text/javascript">
    function ChangeCookieSearch(oList, strCookie) {
        if (event.srcElement.tagName == "INPUT") {
            var oRadio = event.srcElement;
            SetCookie(strCookie, oRadio.value);
        }
    }
</script>
    <asp:Panel ID="panAllow" runat="server" Visible="false">
        <table width="100%" cellpadding="0" cellspacing="5" border="0">
            <tr>
                <td rowspan="2"><img src="/images/workload48.gif" border="0" align="absmiddle" /></td>
                <td class="header" width="100%" valign="bottom">Service Search</td>
            </tr>
            <tr>
                <td width="100%" valign="top">Search on service information related to requests.</td>
            </tr>
        </table>
        <asp:Panel ID="panSimple" runat="server" Visible="false">
        <table width="100%" cellpadding="4" cellspacing="3" border="0">
            <tr>
                <td nowrap><asp:Label ID="fldSearch" runat="server" CssClass="default" Text="Search Type:" /></td>
                <td width="100%"><asp:RadioButtonList ID="radSearch" runat="server" CssClass="default" RepeatDirection="Horizontal" /></td>
            </tr>
            <tr>
                <td nowrap>Search Value:</td>
                <td width="100%">
                    <table cellpadding="0" cellspacing="0" border="0">
                        <tr>
                            <td><asp:TextBox ID="txtSearch" runat="server" Width="250" CssClass="default" MaxLength="100" /></td>
                        </tr>
                        <tr>
                            <td>
                                <div id="divSearch" runat="server" style="overflow:hidden; position:absolute; display:none; background-color:#FFFFFF; border:solid 1px #CCCCCC">
                                    <asp:ListBox ID="lstSearch" runat="server" CssClass="default" />
                                </div>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td nowrap>Results per Page:</td>
                <td width="100%">
                    <asp:DropDownList ID="ddlSearch" runat="server" Width="75" CssClass="default">
                        <asp:ListItem Value="10" Text="10" />
                        <asp:ListItem Value="25" Text="25" Selected="True" />
                        <asp:ListItem Value="50" Text="50" />
                        <asp:ListItem Value="100" Text="100" />
                        <asp:ListItem Value="0" Text="All" />
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td nowrap class="required"></td>
                <td width="100%" colspan="2">
                    <table cellpadding="3" cellspacing="2" border="0">
                        <tr>
                            <td><asp:LinkButton ID="btnAdvanced" runat="server" Text="Advanced Search" OnClick="btnAdvanced_Click" /></a></td>
                            <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
                            <td><asp:LinkButton ID="lnkSearch" runat="server" Text="Clear History" OnClick="lnkSearch_Click" Enabled="false" /></td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td nowrap class="required"></td>
                <td width="100%" colspan="2"><asp:Button ID="btnBasicSearch" Text="Search" Width="100" CssClass="default" runat="server" OnClick="btnBasicSearch_Click" /> <asp:Button ID="btnBasicNew" Text="Start Over" Width="100" CssClass="default" runat="server" OnClick="btnBasicNew_Click" Visible="false" /></td>
            </tr>
        </table>
        </asp:Panel>
        <asp:Panel ID="panAdvanced" runat="server" Visible="false">
        <table width="100%" cellpadding="4" cellspacing="3" border="0">
            <tr>
                <td nowrap>Department:</td>
                <td width="100%">
                    <asp:DropDownList ID="ddlDepartment" runat="server" Width="75" CssClass="default" />
                </td>
            </tr>
            <tr>
                <td nowrap>Results per Page:</td>
                <td width="100%">
                    <asp:DropDownList ID="ddlAdvanced" runat="server" Width="75" CssClass="default">
                        <asp:ListItem Value="10" Text="10" />
                        <asp:ListItem Value="25" Text="25" Selected="True" />
                        <asp:ListItem Value="50" Text="50" />
                        <asp:ListItem Value="100" Text="100" />
                        <asp:ListItem Value="0" Text="All" />
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td nowrap class="required"></td>
                <td width="100%" colspan="2">
                    <table cellpadding="3" cellspacing="2" border="0">
                        <tr>
                            <td><asp:LinkButton ID="btnBasic" runat="server" Text="Basic Search" OnClick="btnBasic_Click" /></a></td>
                            <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
                            <td><asp:LinkButton ID="lnkSearch2" runat="server" Text="Clear History" OnClick="lnkSearch_Click" Enabled="false" /></td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td nowrap class="required"></td>
                <td width="100%" colspan="2"><asp:Button ID="btnAdvancedSearch" Text="Search" Width="100" CssClass="default" runat="server" OnClick="btnAdvancedSearch_Click" /> <asp:Button ID="btnAdvancedNew" Text="Start Over" Width="100" CssClass="default" runat="server" OnClick="btnAdvancedNew_Click" Visible="false" /></td>
            </tr>
        </table>
        </asp:Panel>
        <br />
        <asp:Panel ID="panResults" runat="server" Visible="false">
        <table width="100%" cellpadding="4" cellspacing="3" border="0">
            <tr>
                <td class="default"><asp:Label ID="lblRecords" runat="server" CssClass="bigger" /></td>
                <td class="default" align="right"><asp:LinkButton ID="btnBack" runat="server" Text="Previous Page" OnClick="btnBack_Click" />&nbsp;&nbsp;|&nbsp;&nbsp;<asp:LinkButton ID="btnNext" runat="server" Text="Next Page" OnClick="btnNext_Click" />&nbsp;&nbsp;</td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Panel ID="panService" runat="server" Visible="false">
                        <table width="100%" cellpadding="4" cellspacing="0" border="0" style="border:solid 1px #CCCCCC">
                            <asp:repeater ID="rptService" runat="server">
                                <HeaderTemplate>
                                    <tr bgcolor="#EEEEEE">
                                        <td></td>
                                        <td>#</td>
                                        <td nowrap><b>RequestID</b></td>
                                        <td></td>
                                        <td nowrap><b>Services</b></td>
                                        <td nowrap><b>Requested By</b></td>
                                        <td nowrap><b>Requested On</b></td>
                                        <td nowrap><b>Project Number</b></td>
                                        <td></td>
                                        <td nowrap><b>Project Name</b></td>
                                    </tr>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td><img src="/images/spacer.gif" border="0" width="15" height="1" /></td>
                                        <td><%=intCounter++ %></td>
                                        <td width="10%" nowrap><a class="lookup" href='/datapoint/service/request.aspx?t=<%#Request.QueryString["t"] %>&q=<%# oFunction.encryptQueryString(DataBinder.Eval(Container.DataItem, "number").ToString()) %>'><%# DataBinder.Eval(Container.DataItem, "number") %></a></td>
                                        <td nowrap align="center"><%# (DataBinder.Eval(Container.DataItem, "deleted").ToString() == "1" ? "<img title=\"Deleted on " + DataBinder.Eval(Container.DataItem, "modified").ToString() + "\" src=\"/images/cancel.gif\" border=\"0\" align=\"absmiddle\"/>" : "")%></td>
                                        <td width="20%" nowrap>
                                            <asp:Panel ID="panServicesYes" runat="server" Visible="false">
                                            <table cellpadding="3" cellspacing="0" border="0">
                                                <asp:repeater id="rptServices" datasource='<%# ((DataRowView)Container.DataItem).Row.GetChildRows("relationship") %>' runat="server">
                                                    <ItemTemplate>
                                                        <tr>
                                                            <td>[<%# DataBinder.Eval(Container.DataItem, "[\"quantity\"]")%>]</td>
                                                            <td><%# DataBinder.Eval(Container.DataItem, "[\"name\"]")%></td>
                                                        </tr>
                                                    </ItemTemplate>
                                                </asp:repeater>
                                            </table>
                                            </asp:Panel>
                                            <asp:Panel ID="panServicesNo" runat="server" Visible="false">
                                                <i>No services</i>
                                            </asp:Panel>
                                        </td>
                                        <td width="15%" nowrap title='UserID: <%# DataBinder.Eval(Container.DataItem, "userid") %>'><a class="lookup_disable" href='javascript:void(0);'><%# DataBinder.Eval(Container.DataItem, "requestor_name") + " (" + DataBinder.Eval(Container.DataItem, "requestor_xid") + ")"%></a></td>
                                        <td width="15%" nowrap><%# DataBinder.Eval(Container.DataItem, "created").ToString() %></td>
                                        <td width="10%" nowrap title='ProjectID: <%# DataBinder.Eval(Container.DataItem, "projectid") %>'><a class="lookup_disable" href='javascript:void(0);'><%# DataBinder.Eval(Container.DataItem, "project_number") %></a></td>
                                        <td nowrap align="center"><%# (DataBinder.Eval(Container.DataItem, "project_deleted").ToString() == "1" ? "<img title=\"Deleted on " + DataBinder.Eval(Container.DataItem, "project_modified").ToString() + "\" src=\"/images/cancel.gif\" border=\"0\" align=\"absmiddle\"/>" : "") %></td>
                                        <td width="30%" nowrap><%# DataBinder.Eval(Container.DataItem, "project_name") %></td>
                                    </tr>
                                </ItemTemplate>
                                <AlternatingItemTemplate>
                                    <tr bgcolor="#F6F6F6">
                                        <td><img src="/images/spacer.gif" border="0" width="15" height="1" /></td>
                                        <td><%=intCounter++ %></td>
                                        <td width="10%" nowrap><a class="lookup" href='/datapoint/service/request.aspx?t=<%#Request.QueryString["t"] %>&q=<%# oFunction.encryptQueryString(DataBinder.Eval(Container.DataItem, "number").ToString()) %>'><%# DataBinder.Eval(Container.DataItem, "number") %></a></td>
                                        <td nowrap align="center"><%# (DataBinder.Eval(Container.DataItem, "deleted").ToString() == "1" ? "<img title=\"Deleted on " + DataBinder.Eval(Container.DataItem, "modified").ToString() + "\" src=\"/images/cancel.gif\" border=\"0\" align=\"absmiddle\"/>" : "")%></td>
                                        <td width="20%" nowrap>
                                            <asp:Panel ID="panServicesYes" runat="server" Visible="false">
                                            <table cellpadding="3" cellspacing="0" border="0">
                                                <asp:repeater id="rptServices" datasource='<%# ((DataRowView)Container.DataItem).Row.GetChildRows("relationship") %>' runat="server">
                                                    <ItemTemplate>
                                                        <tr>
                                                            <td>[<%# DataBinder.Eval(Container.DataItem, "[\"quantity\"]")%>]</td>
                                                            <td><%# DataBinder.Eval(Container.DataItem, "[\"name\"]")%></td>
                                                        </tr>
                                                    </ItemTemplate>
                                                </asp:repeater>
                                            </table>
                                            </asp:Panel>
                                            <asp:Panel ID="panServicesNo" runat="server" Visible="false">
                                                <i>No services</i>
                                            </asp:Panel>
                                        </td>
                                        <td width="15%" nowrap title='UserID: <%# DataBinder.Eval(Container.DataItem, "userid") %>'><a class="lookup_disable" href='javascript:void(0);'><%# DataBinder.Eval(Container.DataItem, "requestor_name") + " (" + DataBinder.Eval(Container.DataItem, "requestor_xid") + ")"%></a></td>
                                        <td width="15%" nowrap><%# DataBinder.Eval(Container.DataItem, "created").ToString() %></td>
                                        <td width="10%" nowrap title='ProjectID: <%# DataBinder.Eval(Container.DataItem, "projectid") %>'><a class="lookup_disable" href='javascript:void(0);'><%# DataBinder.Eval(Container.DataItem, "project_number") %></a></td>
                                        <td nowrap align="center"><%# (DataBinder.Eval(Container.DataItem, "project_deleted").ToString() == "1" ? "<img title=\"Deleted on " + DataBinder.Eval(Container.DataItem, "project_modified").ToString() + "\" src=\"/images/cancel.gif\" border=\"0\" align=\"absmiddle\"/>" : "") %></td>
                                        <td width="30%" nowrap><%# DataBinder.Eval(Container.DataItem, "project_name") %></td>
                                    </tr>
                                </AlternatingItemTemplate>
                            </asp:repeater>
                            <tr>
                                <td colspan="6"><asp:Label ID="lblService" runat="server" CssClass="default" Visible="false" Text="<img src='/images/spacer.gif' border='0' height='1' width='25' /><img src='/images/alert.gif' border='0' align='absmiddle'> No results found" /></td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="panDesign" runat="server" Visible="false">
                        <table width="100%" cellpadding="4" cellspacing="0" border="0" style="border:solid 1px #CCCCCC">
                            <asp:repeater ID="rptDesign" runat="server">
                                <HeaderTemplate>
                                    <tr bgcolor="#EEEEEE">
                                        <td></td>
                                        <td>#</td>
                                        <td nowrap><b>Design ID</b></td>
                                        <td nowrap><b>Nickname</b></td>
                                        <td nowrap><b>Requested By</b></td>
                                        <td nowrap><b>Project Number</b></td>
                                        <td nowrap><b>Project Name</b></td>
                                        <td nowrap align="center"><b>Status</b></td>
                                    </tr>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td><img src="/images/spacer.gif" border="0" width="15" height="1" /></td>
                                        <td><%=intCounter++ %></td>
                                        <td width="10%" nowrap><a class="lookup" href='/datapoint/service/design.aspx?t=<%#Request.QueryString["t"] %>&q=<%# oFunction.encryptQueryString(DataBinder.Eval(Container.DataItem, "id").ToString()) %>'><%# DataBinder.Eval(Container.DataItem, "id") %></a></td>
                                        <td width="20%" nowrap><%# DataBinder.Eval(Container.DataItem, "name") %></td>
                                        <td width="20%" nowrap><asp:Label ID="lblUser" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "userid") %>' /></td>
                                        <td width="10%" nowrap title='ProjectID: <%# DataBinder.Eval(Container.DataItem, "projectid") %>'><a class="lookup_disable" href='javascript:void(0);'><%# DataBinder.Eval(Container.DataItem, "project_number") %></a></td>
                                        <td width="20%" nowrap><%# DataBinder.Eval(Container.DataItem, "project_name") %></td>
                                        <td width="20%" nowrap align="center"><asp:Label ID="lblStatus" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "id") %>' /></td>
                                                <asp:Label ID="lblCompleted" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container.DataItem, "completed").ToString() %>' />
                                                <asp:Label ID="lblFinished" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container.DataItem, "finished").ToString() %>' />
                                                <asp:Label ID="lblDelete" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container.DataItem, "deleted").ToString() %>' />
                                                <asp:Label ID="lblDeleteForecast" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container.DataItem, "forecast_deleted").ToString() %>' />
                                                <asp:Label ID="lblDeleteRequest" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container.DataItem, "request_deleted").ToString() %>' />
                                                <asp:Label ID="lblDeleteProject" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container.DataItem, "project_deleted").ToString() %>' />
                                    </tr>
                                </ItemTemplate>
                                <AlternatingItemTemplate>
                                    <tr bgcolor="#F6F6F6">
                                        <td><img src="/images/spacer.gif" border="0" width="15" height="1" /></td>
                                        <td><%=intCounter++%></td>
                                        <td width="10%" nowrap><a class="lookup" href='/datapoint/service/design.aspx?t=<%#Request.QueryString["t"] %>&q=<%# oFunction.encryptQueryString(DataBinder.Eval(Container.DataItem, "id").ToString()) %>'><%# DataBinder.Eval(Container.DataItem, "id") %></a></td>
                                        <td width="20%" nowrap><%# DataBinder.Eval(Container.DataItem, "name") %></td>
                                        <td width="20%" nowrap><asp:Label ID="lblUser" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "userid") %>' /></td>
                                        <td width="10%" nowrap title='ProjectID: <%# DataBinder.Eval(Container.DataItem, "projectid") %>'><a class="lookup_disable" href='javascript:void(0);'><%# DataBinder.Eval(Container.DataItem, "project_number") %></a></td>
                                        <td width="20%" nowrap><%# DataBinder.Eval(Container.DataItem, "project_name") %></td>
                                        <td width="20%" nowrap align="center"><asp:Label ID="lblStatus" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "id") %>' /></td>
                                                <asp:Label ID="lblCompleted" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container.DataItem, "completed").ToString() %>' />
                                                <asp:Label ID="lblFinished" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container.DataItem, "finished").ToString() %>' />
                                                <asp:Label ID="lblDelete" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container.DataItem, "deleted").ToString() %>' />
                                                <asp:Label ID="lblDeleteForecast" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container.DataItem, "forecast_deleted").ToString() %>' />
                                                <asp:Label ID="lblDeleteRequest" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container.DataItem, "request_deleted").ToString() %>' />
                                                <asp:Label ID="lblDeleteProject" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container.DataItem, "project_deleted").ToString() %>' />
                                    </tr>
                                </AlternatingItemTemplate>
                            </asp:repeater>
                            <tr>
                                <td colspan="8"><asp:Label ID="lblDesign" runat="server" CssClass="default" Visible="false" Text="<img src='/images/spacer.gif' border='0' height='1' width='25' /><img src='/images/alert.gif' border='0' align='absmiddle'> No results found" /></td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
        </table>
        </asp:Panel>
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
<asp:HiddenField ID="hdnSearchText" runat="server" />
<asp:HiddenField ID="hdnSearchType" runat="server" />
</asp:Content>
