<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="projects_pending.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.projects_pending" %>

<script type="text/javascript">
    function ShowServiceRequests(intRequest) {
        window.open('/admin/frame/frame_service_requests.aspx?rid=' + intRequest);
        return false;
    }
    function ShowDesigns(intRequest) {
        window.open('/admin/frame/frame_designs.aspx?rid=' + intRequest);
        return false;
    }
</script>
<html>
<head>
<title>ClearView Administration</title>
<link rel="stylesheet" type="text/css" href="/css/default.css" />
<script type="text/javascript" src="/javascript/global.js"></script>
</head>
<body topmargin="0" leftmargin="0">
<form id="Form1" runat="server">
        <table width="100%" height="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
        <td height="100%" valign="top">
<table width="98%" cellpadding="0" cellspacing="0" border="0" align="center">
		<tr><td colspan="2">&nbsp;</td></tr>
		<tr> 
		    <td><b>Pending Projects / Tasks</b></td>
		    <td align="right" class="required"><img src="/images/alert.gif" border="0" align="absmiddle" /> <b>NOTE:</b> After approving a project, associated service requests will be submitted.</td>
		</tr>
		<tr><td colspan="2">&nbsp;</td></tr>
		<tr>
		    <td colspan="2" align="center">
		        <asp:Panel ID="panAll" runat="server" Visible="false">
                    <table width="100%" border="0" cellpadding="0" cellspacing="0">
                        <tr>
                            <td>
                                <asp:repeater ID="rptView" runat="server">
                                    <HeaderTemplate>
                                        <table width="100%" cellpadding="3" cellspacing="0" border="0" align="center">
                                            <tr bgcolor="#CCCCCC">
                                                <td class="bold">Name</td>
                                                <td class="bold">Type</td>
                                                <td class="bold">Submitted By</td>
                                                <td class="bold">Submittied On</td>
                                            </tr>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr bgcolor="#EFEFEF" class="default">
                                            <td align="left">&nbsp;<a href='/admin/projects_pending.aspx?id=<%# DataBinder.Eval(Container.DataItem, "id") %>'><%# DataBinder.Eval(Container.DataItem, "name") %></a></td>
                                            <td>&nbsp;<%# (DataBinder.Eval(Container.DataItem, "task").ToString() == "1" ? "Task" : "Project")%></td>
                                            <td>&nbsp;<%# oUser.GetFullName(Int32.Parse(DataBinder.Eval(Container.DataItem, "userid").ToString()))%></td>
                                            <td>&nbsp;<%# DataBinder.Eval(Container.DataItem, "modified") %></td>
                                        </tr>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </table>
                                    </FooterTemplate>
                                </asp:repeater>
                            </td>
                        </tr>
                    </table>
		        </asp:Panel>

		        <asp:Panel ID="panView" runat="server" Visible="false">
                    <table width="100%" border="0" cellspacing="2" cellpadding="2">
                        <tr>
                            <td>Information:</td>
                            <td>Existing Projects:</td>
                        </tr>
                        <tr>
                            <td width="50%" valign="top">
                                <div style="width:100%;height=400;overflow:auto;border:solid 1px #EEEEEE">
                                    <%=strProject%>
                                </div>
                                <table border="0" cellspacing="2" cellpadding="2">
                                    <tr>
                                        <td>Name:</td>
                                        <td><asp:TextBox ID="txtName" runat="server" CssClass="default" Width="200" /></td>
                                    </tr>
                                    <tr>
                                        <td>Number:</td>
                                        <td><asp:TextBox ID="txtNumber" runat="server" CssClass="default" Width="100" /></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td><asp:Button ID="btnUpdate" runat="server" CssClass="default" Width="75" Text="Update" OnClick="btnUpdate_Click" /></td>
                                    </tr>
                                </table>
                                
                            </td>
                            <td width="50%" valign="top">
                                <div style="width:100%;height=400;overflow:auto;border:solid 1px #EEEEEE">
                                    <asp:TreeView ID="oTree" runat="server" CssClass="default" ShowCheckBoxes="All" ShowLines="true" NodeIndent="30">
                                    </asp:TreeView>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" align="center">
                                <table cellpadding="3" cellspacing="2" border="0">
                                    <tr><td><asp:Button ID="btnView" runat="server" CssClass="default" Width="200" Text="View Associated Requests" /></td></tr>
                                    <tr><td><asp:Button ID="btnViewDesigns" runat="server" CssClass="default" Width="200" Text="View Associated Designs" /></td></tr>
                                    <tr><td><asp:Button ID="btnCreateProject" runat="server" CssClass="default" Width="200" Text="Create as New Project" OnClick="btnCreateProject_Click" /></td></tr>
                                    <tr><td><asp:Button ID="btnCreateTask" runat="server" CssClass="default" Width="200" Text="Create as New Task" OnClick="btnCreateTask_Click" /></td></tr>
                                    <tr><td><asp:Button ID="btnReject" runat="server" CssClass="default" Width="200" Text="Reject" OnClick="btnReject_Click" /></td></tr>
                                </table>
                            </td>
                            <td valign="top" align="center">
                                <table cellpadding="3" cellspacing="2" border="0">
                                    <tr><td><asp:Button ID="btnCompare" runat="server" Text="Compare Projects" CssClass="default" Width="150" OnClick="btnCompare_Click" /></td></tr>
                                    <tr><td>&nbsp;</td></tr>
                                    <tr><td><asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="default" Width="150" OnClick="btnCancel_Click" /></td></tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>

		        <asp:Panel ID="panReject" runat="server" Visible="false">
                    <table width="100%" cellpadding="0" cellspacing="5" border="0">
                        <tr>
                            <td><%=strProject%></td>
                        </tr>
                        <tr>
                            <td>Optional Comments (will be displayed to requestor):</td>
                        </tr>
                        <tr>
                            <td><asp:TextBox ID="txtReason" runat="server" CssClass="default" Width="500" Rows="10" TextMode="MultiLine" /></td>
                        </tr>
                        <tr>
                            <td><asp:Button ID="btnRejectConfirm" runat="server" Text="Reject" CssClass="default" Width="100" OnClick="btnRejectConfirm_Click" /> <asp:Button ID="btnBack2" runat="server" Text="Back" CssClass="default" Width="100" OnClick="btnBack_Click" /></td>
                        </tr>
                    </table>
                </asp:Panel>
		        <asp:Panel ID="panCompare" runat="server" Visible="false">
                    <table width="100%" cellpadding="0" cellspacing="5" border="0">
                        <tr>
                            <td><asp:Table ID="tblCompare" runat="server" Width="100%" CellPadding="4" CellSpacing="0" CssClass="default" /></td>
                        </tr>
                        <tr>
                            <td><asp:Button ID="btnBack" runat="server" Text="Back" CssClass="default" Width="100" OnClick="btnBack_Click" /></td>
                        </tr>
                    </table>
                </asp:Panel>
		        <asp:Panel ID="panPH" runat="server" Visible="false">
                    <table width="100%" cellpadding="0" cellspacing="5" border="0">
                        <tr>
                            <td rowspan="2"><img src="/images/service_request.gif" border="0" align="absmiddle" /></td>
                            <td class="hugeheader" width="100%" valign="bottom">Service Request</td>
                        </tr>
                        <tr>
                            <td width="100%" valign="top">Here are the results of the associated Service Requests for this project...</td>
                        </tr>
                    </table>
                    <table width="100%" border="0" cellspacing="2" cellpadding="2">
                        <tr>
                            <td><asp:PlaceHolder ID="PHcp" runat="server" /></td>
                        </tr>
                        <tr>
                            <td align="center"><asp:Button ID="btnFinish" runat="server" CssClass="default" Width="100" Text="Finish" OnClick="btnFinish_Click" /></td>
                        </tr>
                </asp:Panel>
            </td>
        </tr>
    </table>
        </td>
        </tr>
        </table>
</form>
</body>
</html>
