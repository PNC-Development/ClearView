<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="services_pending.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.services_pending" %>


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
		    <td><b>Pending Services</b></td>
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
                                                <td class="bold">Service Name</td>
                                                <td class="bold">Application</td>
                                                <td class="bold">Submitted By</td>
                                                <td class="bold">Submittied On</td>
                                            </tr>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr bgcolor="#EFEFEF" class="default">
                                            <td align="left">&nbsp;<a href='/admin/services_pending.aspx?id=<%# DataBinder.Eval(Container.DataItem, "serviceid") %>'><%# DataBinder.Eval(Container.DataItem, "name") %></a></td>
                                            <td>&nbsp;<%# DataBinder.Eval(Container.DataItem, "application") %></td>
                                            <td>&nbsp;<%# DataBinder.Eval(Container.DataItem, "username") %></td>
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
                <table width="100%" border="0" cellpadding="4" cellspacing="3">
                    <tr>
                        <td nowrap>Service Name:</td>
                        <td width="100%"><asp:Label ID="lblName" runat="server" CssClass="default" /></td>
                    </tr>
                    <tr>
                        <td nowrap>Request Item:</td>
                        <td width="100%"><asp:Label ID="lblItem" runat="server" CssClass="default" /></td>
                    </tr>
                    <tr>
                        <td nowrap>Application:</td>
                        <td width="100%"><asp:Label ID="lblApplication" runat="server" CssClass="default" /></td>
                    </tr>
                    <tr>
                        <td nowrap>Submitted By:</td>
                        <td width="100%"><asp:Label ID="lblUser" runat="server" CssClass="default" /></td>
                    </tr>
                    <tr>
                        <td nowrap></td>
                        <td width="100%">
                            <asp:Button ID="btnSave" runat="server" CssClass="default" Width="100" Text="Save" OnClick="btnSave_Click" /> 
                            <asp:Button ID="btnDelete" runat="server" CssClass="default" Width="100" Text="Delete" OnClick="btnDelete_Click" /> 
                            <asp:Button ID="btnCancel" runat="server" CssClass="default" Width="100" Text="Cancel" OnClick="btnCancel_Click" /> 
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">&nbsp;</td>
                    </tr>
                    <tr>
                        <td colspan="2">Click the folder where you want to place this service...</td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:TreeView ID="oTreeview" runat="server" ShowLines="true" NodeIndent="35" ShowCheckBoxes="All">
                                <NodeStyle CssClass="default" />
                            </asp:TreeView>
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
