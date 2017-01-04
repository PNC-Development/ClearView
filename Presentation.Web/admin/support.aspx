<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="support.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.support" %>


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
        <td height="100%">
        <div style="height:100%; overflow:auto">
<table width="98%" cellpadding="0" cellspacing="0" border="0" align="center">
		<tr><td colspan="2">&nbsp;</td></tr>
		<tr> 
		    <td><b>Support Suggestions / Issues</b></td>
		    <td align="right"></td>
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
                                                <td class="bold">&nbsp;</td>
                                                <td class="bold">&nbsp;</td>
                                                <td width="50%" class="bold"><asp:linkbutton ID="lnkName" Text="Name" CommandArgument="name" CssClass="bold" OnClick="OrderView" runat="server"/></td>
                                                <td width="50%" class="bold"><asp:linkbutton ID="lnkModified" Text="Last Modified" CommandArgument="modified" CssClass="bold" OnClick="OrderView" runat="server"/></td>
                                            </tr>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr bgcolor="#EFEFEF" onmouseover="AdminMouseOver(this);" onmouseout="AdminMouseOut(this);" class="default">
                                            <td width="5%" align="left">&nbsp;<asp:ImageButton ID="btnDelete" OnClick="btnDeleteLink_Click" runat="server" ToolTip="Delete" ImageAlign="AbsMiddle" ImageUrl="/admin/images/delete.gif" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' /></td>
                                            <td width="5%" align="left">&nbsp;<asp:ImageButton ID="btnEdit" OnClick="btnEdit_Click" runat="server" ToolTip="Edit" ImageAlign="AbsMiddle" ImageUrl="/admin/images/edit.gif" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' /></td>
                                            <td width="50%">&nbsp;<%# DataBinder.Eval(Container.DataItem, "name") %></td>
                                            <td width="50%">&nbsp;<%# DataBinder.Eval(Container.DataItem, "modified") %></td>
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
                    <table width="95%" cellpadding="3" cellspacing="0" border="0" align="center">
                        <tr> 
                            <td class="default">Name:</td>
                            <td><asp:Label ID="lblName" CssClass="default" runat="server" /></td>
                        </tr>
                        <tr> 
                            <td class="default">Module:</td>
                            <td><asp:Label ID="lblModule" CssClass="default" runat="server" /></td>
                        </tr>
                        <tr> 
                            <td class="default">Type:</td>
                            <td><asp:Label ID="lblType" CssClass="default" runat="server" /></td>
                        </tr>
                        <tr> 
                            <td class="default">Description:</td>
                            <td><asp:Label ID="lblDescription" CssClass="default" runat="server" /></td>
                        </tr>
                        <tr> 
                            <td class="default">Requested On:</td>
                            <td><asp:Label ID="lblOn" CssClass="default" runat="server" /></td>
                        </tr>
                        <tr> 
                            <td class="default">Requested By:</td>
                            <td><asp:Label ID="lblBy" CssClass="default" runat="server" /></td>
                        </tr>
                        <tr> 
                            <td class="default">Comments:</td>
                            <td><asp:TextBox ID="txtComments" CssClass="default" runat="server" TextMode="MultiLine" Width="400" Rows="7" /></td>
                        </tr>
                        <tr> 
                            <td class="default">Status:</td>
                            <td>
                                <asp:DropDownList ID="ddlStatus" CssClass="default" runat="server">
                                    <asp:ListItem Value="-1" Text="Denied" />
                                    <asp:ListItem Value="0" Text="Pending" />
                                    <asp:ListItem Value="2" Text="Active" />
                                    <asp:ListItem Value="3" Text="Completed" />
                                    <asp:ListItem Value="5" Text="On Hold" />
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr><td height="5" colspan="2">&nbsp;</td></tr>
                        <tr> 
                            <td>&nbsp;</td>
                            <td>
                                <asp:button ID="btnAdd" CssClass="default" runat="server" Text="Add" Width="75" OnClick="btnAdd_Click" />
                                <asp:button ID="btnDelete" CssClass="default" runat="server" Text="Delete" Width="75" OnClick="btnDelete_Click" />
                                <asp:button ID="btnCancel" CssClass="default" runat="server" Text="Cancel" Width="75" OnClick="btnCancel_Click" />
                            </td>
                        </tr>
                    </asp:Panel>
                </div>
            </td>
        </tr>
    </table>
        </div>
        </td>
        </tr>
        </table>
</form>
</body>
</html>
