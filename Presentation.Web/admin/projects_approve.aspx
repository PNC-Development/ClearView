<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="projects_approve.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.projects_approve" %>

<script type="text/javascript">
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
		    <td><b>Pending Project Requests</b></td>
		    <td align="right" class="required"></td>
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
                                                <td class="bold">Project Name</td>
                                                <td class="bold">Project Number</td>
                                                <td class="bold">Portfolio</td>
                                                <td class="bold">Waiting On</td>
                                                <td class="bold">Submitted By</td>
                                                <td class="bold">Submittied On</td>
                                            </tr>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr bgcolor="#EFEFEF" class="default">
                                            <td align="left">&nbsp;<a href='/admin/projects_approve.aspx?rid=<%# DataBinder.Eval(Container.DataItem, "requestid") %>'><%# DataBinder.Eval(Container.DataItem, "name") %></a></td>
                                            <td>&nbsp;<%# DataBinder.Eval(Container.DataItem, "number")%></td>
                                            <td>&nbsp;<%# oOrganization.GetName(Int32.Parse(DataBinder.Eval(Container.DataItem, "organization").ToString()))%></td>
                                            <td>&nbsp;<%# (DataBinder.Eval(Container.DataItem, "step").ToString() == "1" ? "Manager" : (DataBinder.Eval(Container.DataItem, "step").ToString() == "2" ? "Platform" : (DataBinder.Eval(Container.DataItem, "step").ToString() == "3" ? "Board" : "Director")))%></td>
                                            <td>&nbsp;<%# oUser.GetFullName(Int32.Parse(DataBinder.Eval(Container.DataItem, "userid").ToString()))%></td>
                                            <td>&nbsp;<%# DataBinder.Eval(Container.DataItem, "created") %></td>
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
                    <table width="100%" border="0" cellpadding="0" cellspacing="0">
                        <tr>
                            <td>
                                <asp:TreeView ID="oTreeview" runat="server" ShowLines="true" NodeIndent="35">
                                    <NodeStyle CssClass="default" />
                                </asp:TreeView>
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td>Rejection Comments (Optional)</td>
                        </tr>
                        <tr>
                            <td><asp:TextBox ID="txtComments" runat="server" CssClass="default" Width="600" TextMode="MultiLine" Rows="8" /></td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td>
                                <table width="600" border="0" cellpadding="2" cellspacing="1">
                                    <tr>
                                        <td><asp:Button ID="btnApprove" runat="server" CssClass="default" Width="100" Text="Approve" CommandArgument="1" OnClick="btnSubmit_Click" /></td>
                                        <td><asp:Button ID="btnReject" runat="server" CssClass="default" Width="100" Text="Reject" CommandArgument="-1" OnClick="btnSubmit_Click" /></td>
                                        <td><asp:Button ID="btnFuture" runat="server" CssClass="default" Width="100" Text="Future" CommandArgument="10" OnClick="btnSubmit_Click" /></td>
                                        <td width="100%">&nbsp;</td>
                                        <td><asp:Button ID="btnCancel" runat="server" CssClass="default" Width="100" Text="Cancel" OnClick="btnCancel_Click" /></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
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
