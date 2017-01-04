<%@ Page Language="C#" MasterPageFile="~/datapoint.Master" AutoEventWireup="true" CodeBehind="manager.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.manager" Title="Untitled Page" %>
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
<table width="100%" cellpadding="0" cellspacing="5" border="0">
    <tr>
        <td rowspan="2"><img src="/images/workload48.gif" border="0" align="absmiddle" /></td>
        <td class="header" nowrap valign="bottom">Resource <asp:Label ID="lblHeader" runat="server" CssClass="header" /> Administration</td>
        <td width="100%" rowspan="2" align="right">
            <table cellpadding="1" cellspacing="4" border="0">
                <tr id="cntrlButtons">
                    <td nowrap><asp:LinkButton ID="btnSave" runat="server" Text="<img src='/images/save-icon.gif' border='0' align='absmiddle' /><img src='/images/spacer.gif' border='0' width='5' height='1' />Save" OnClick="btnSave_Click" /></td>
                    <td>|</td>
                    <td nowrap><asp:LinkButton ID="btnSaveClose" runat="server" Text="<img src='/images/save-icon.gif' border='0' align='absmiddle' /><img src='/images/spacer.gif' border='0' width='5' height='1' />Save & Close" OnClick="btnSaveClose_Click" /></td>
                    <td>|</td>
                    <td nowrap><asp:LinkButton ID="btnPrint" runat="server" Text="<img src='/images/print-icon.gif' border='0' align='absmiddle' /><img src='/images/spacer.gif' border='0' width='5' height='1' />Print" /></td>
                    <td>|</td>
                    <td nowrap><asp:LinkButton ID="btnClose" runat="server" Text="<img src='/images/close-icon.gif' border='0' align='absmiddle' /><img src='/images/spacer.gif' border='0' width='5' height='1' />Close" /></td>
                </tr>
                <tr id="cntrlProcessing" style="display:none">
                    <td colspan="20">
                        <table width="100%" cellpadding="0" cellspacing="5" border="0">
                            <tr>
                                <td rowspan="2"><img src="/images/saving.gif" border="0" align="absmiddle" /></td>
                                <td class="header" width="100%" valign="bottom">Processing...</td>
                            </tr>
                            <tr>
                                <td width="100%" valign="top">Please do not close this window while the page is processing.  Please be patient....</td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <asp:Panel ID="panSave" runat="server" Visible="false">
                <tr>
                    <td colspan="7" class="bigCheck" align="center"><img src="/images/bigCheck.gif" border="0" align="absmiddle" /> Update Successful</td>
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

<table width="100%" height="100%" cellpadding="0" cellspacing="0" border="0">
    <tr>
        <td valign="top">
            <asp:Panel ID="panView" runat="server" Visible="false">
                <table width="100%" cellpadding="4" cellspacing="3" border="0">
                    <tr>
                        <td nowrap><b>Device Count:</b></td>
                        <td nowrap><asp:Label ID="lblDevices" runat="server" CssClass="default" Visible="false" /><asp:TextBox ID="txtDevices" runat="server" CssClass="default" Width="100" MaxLength="10" Visible="false" /></td>
                        <td width="100%" rowspan="2">
                            <asp:Panel ID="panDynamic" runat="server" Visible="false">
                                <table height="100%" cellpadding="2" cellspacing="2" border="0">
                                    <tr>
                                        <td nowrap>
                                            <table height="100%" cellpadding="0" cellspacing="0" border="0">
                                                <tr height="6">
                                                    <td width="100%" background="/images/box_top.gif"></td>
                                                    <td><img src="/images/box_top_right.gif" border="0" width="6" height="6"></td>
                                                </tr>
                                                <tr>
                                                    <td width="100%"><img src="/images/spacer.gif" width="6" height="50"></td>
                                                    <td background="/images/table_right.gif"><img src="/images/table_right.gif" width="5" height="10"></td>
                                                </tr>
                                                <tr height="6">
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
                        <td nowrap><b>Hours Allocated:</b></td>
                        <td nowrap><asp:Label ID="lblAllocated" runat="server" CssClass="default" Visible="false" /><asp:TextBox ID="txtAllocated" runat="server" CssClass="default" Width="100" MaxLength="10" Visible="false" /></td>
                    </tr>
                    <tr>
                        <td nowrap><b>Hours Used:</b></td>
                        <td width="100%" colspan="2"><asp:Label ID="lblUsed" runat="server" CssClass="default" /></td>
                    </tr>
                    <tr>
                        <td nowrap><b>Activity Type:</b></td>
                        <td width="100%" colspan="2"><asp:Label ID="lblActivity" runat="server" CssClass="default" Visible="false" /><asp:DropDownList ID="ddlActivity" runat="server" CssClass="default" Visible="false" /></td>
                    </tr>
                    <tr>
                        <td nowrap><b>Assigned Resource:</b></td>
                        <td width="100%" colspan="2">
                            <asp:Label ID="lblUser" runat="server" CssClass="default" Visible="false" />
                            <asp:Panel ID="panUser" runat="server" Visible="false">
                                <table cellpadding="0" cellspacing="0" border="0">
                                    <tr>
                                        <td><asp:TextBox ID="txtUser" runat="server" Width="300" CssClass="default" /></td>
                                        <td>&nbsp;&nbsp;&nbsp;<asp:LinkButton ID="lnkAvailable" runat="server" Text="View Availability" /></td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <div id="divUser" runat="server" style="overflow:hidden; position:absolute; display:none; background-color:#FFFFFF; border:solid 1px #CCCCCC">
                                                <asp:ListBox ID="lstUser" runat="server" CssClass="default" />
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td nowrap><b>Status:</b></td>
                        <td width="100%" colspan="2"><asp:DropDownList ID="ddlStatus" runat="server" Enabled="false" /></td>
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
<asp:HiddenField ID="hdnUser" runat="server" />
</asp:Content>
