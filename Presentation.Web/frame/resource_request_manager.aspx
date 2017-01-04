<%@ Page Language="C#" MasterPageFile="~/window.Master" AutoEventWireup="true" CodeBehind="resource_request_manager.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.resource_request_manager" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" runat="server">
<script type="text/javascript">
    function ShowHideAvailable(oDiv) {
        oDiv = document.getElementById(oDiv);
        if (oDiv.style.display == "inline")
            oDiv.style.display = "none";
        else
            oDiv.style.display = "inline";
        return false;
    }
</script>
<table width="100%" height="100%" cellpadding="0" cellspacing="0" border="0">
    <tr>
        <td valign="top">
            <asp:Panel ID="panView" runat="server" Visible="false">
            <table width="100%" cellpadding="0" cellspacing="5" border="0">
                <tr>
                    <td rowspan="2"><img src="/images/view_request.gif" border="0" align="absmiddle" /></td>
                    <td class="header" width="100%" valign="bottom">Request Administration</td>
                </tr>
                <tr>
                    <td width="100%" valign="top">You have the necessary permission required to update the following request.</td>
                </tr>
            </table>
                <table width="100%" cellpadding="4" cellspacing="3" border="0">
                    <asp:Panel ID="panProject" runat="server" Visible="false">
                    <tr>
                        <td nowrap><b>Project Name:</b></td>
                        <td width="100%"><asp:Label ID="lblName" runat="server" CssClass="default" /></td>
                    </tr>
                    <tr>
                        <td nowrap><b>Project Number:</b></td>
                        <td width="100%"><asp:Label ID="lblNumber" runat="server" CssClass="default" /></td>
                    </tr>
                    <tr>
                        <td nowrap><b>Organization:</b></td>
                        <td width="100%"><asp:Label ID="lblOrganization" runat="server" CssClass="default" /></td>
                    </tr>
                    </asp:Panel>
                    <asp:Panel ID="panTask" runat="server" Visible="false">
                    <tr>
                        <td nowrap><b>Task Name:</b></td>
                        <td width="100%"><asp:Label ID="lblTaskName" runat="server" CssClass="default" /></td>
                    </tr>
                    <tr>
                        <td nowrap><b>Task Number:</b></td>
                        <td width="100%"><asp:Label ID="lblTaskNumber" runat="server" CssClass="default" /></td>
                    </tr>
                    </asp:Panel>
                    <tr>
                        <td nowrap><b>Statement of Work:</b></td>
                        <td width="100%"><asp:Label ID="lblStatement" runat="server" CssClass="default" /></td>
                    </tr>
                    <tr>
                        <td nowrap><b>Submitter:</b></td>
                        <td width="100%"><asp:Label ID="lblSubmitter" runat="server" CssClass="default" /></td>
                    </tr>
                    <tr>
                        <td nowrap><b>Estimated Start Date:</b></td>
                        <td width="100%"><asp:Label ID="lblStart" runat="server" CssClass="default" /></td>
                    </tr>
                    <tr>
                        <td nowrap><b>Estimated End Date:</b></td>
                        <td width="100%"><asp:Label ID="lblEnd" runat="server" CssClass="default" /></td>
                    </tr>
                    <tr>
                        <td nowrap><b>Hours Allocated:</b></td>
                        <td width="100%"><asp:Label ID="lblAllocated" runat="server" CssClass="default" Visible="false" /><asp:TextBox ID="txtAllocated" runat="server" CssClass="default" Width="100" MaxLength="10" Visible="false" /></td>
                    </tr>
                    <tr>
                        <td nowrap><b>Hours Used:</b></td>
                        <td width="100%"><asp:Label ID="lblUsed" runat="server" CssClass="default" /></td>
                    </tr>
                    <tr>
                        <td nowrap><b>Device Count:</b></td>
                        <td width="100%"><asp:Label ID="lblDevices" runat="server" CssClass="default" Visible="false" /><asp:TextBox ID="txtDevices" runat="server" CssClass="default" Width="100" MaxLength="10" Visible="false" /></td>
                    </tr>
                    <tr>
                        <td nowrap><b>Activity Type:</b></td>
                        <td width="100%"><asp:Label ID="lblActivity" runat="server" CssClass="default" Visible="false" /><asp:DropDownList ID="ddlActivity" runat="server" CssClass="default" Visible="false" /></td>
                    </tr>
                    <tr>
                        <td nowrap><b>Assigned Resource:</b></td>
                        <td width="100%"><asp:Label ID="lblUser" runat="server" CssClass="default" Visible="false" /><asp:DropDownList ID="ddlUser" runat="server" CssClass="default" Visible="false" />&nbsp;&nbsp;&nbsp;<asp:LinkButton ID="lnkAvailable" runat="server" Text="View Availability" Visible="false" /></td>
                    </tr>
                    <tr>
                        <td nowrap><b>Re-Open this Request:</b></td>
                        <td width="100%"><asp:Checkbox ID="chkOpen" runat="server" CssClass="default" Enabled="false" /></td>
                    </tr>
                    <asp:Panel ID="panVirtual" runat="server" Visible="false">
                    <tr>
                        <td colspan="2"><br/><asp:Button ID="btnVirtual" runat="server" Text="" CssClass="default" /></td>
                    </tr>
                    </asp:Panel>
                    <tr>
                        <td colspan="2"><br/><asp:LinkButton ID="btnView" runat="server" Text="Click here to view this request" /></td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="panManager" runat="server" Visible="false">
                <table width="100%" cellpadding="3" cellspacing="2" border="0">
                    <tr>
                        <td>
                            <div id="divAvailable" runat="server" style="display:none">
                                <br />
                                <table width="100%" cellpadding="2" cellspacing="0" border="0" style="border:solid 1px #999999" bgcolor="#fefefe">
                                    <tr>
                                        <td colspan="2"><b>Resource Availability</b></td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">The following graph represents the amount of allocated hours unused.</td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">&nbsp;</td>
                                    </tr>
                                <asp:repeater ID="rptAvailable" runat="server">
                                    <ItemTemplate>
                                        <tr>
                                            <td><%# DataBinder.Eval(Container.DataItem, "name").ToString() %></td>
                                            <td><img src="/images/table_top.gif" width='<%# DataBinder.Eval(Container.DataItem, "graph").ToString() %>' height="16" />&nbsp;<b><%# Double.Parse(DataBinder.Eval(Container.DataItem, "hours").ToString()).ToString("F") %></b></td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" height="1"><img src="/images/spacer.gif" border="0" width="1" height="1" /></td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:repeater>
                                </table>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td><hr size="1" noshade /></td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Button ID="btnSave" runat="server" CssClass="default" Width="75" Text="Save" OnClick="btnSave_Click" /> 
                            <asp:Button ID="btnClose" runat="server" CssClass="default" Width="75" Text="Close" /> 
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
        </td>
    </tr> 
</table>
<asp:Label ID="lblResourceWorkflow" runat="server" Visible="false" />
</asp:Content>
