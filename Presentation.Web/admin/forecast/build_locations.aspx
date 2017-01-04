<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="build_locations.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.build_locations" %>
<script type="text/javascript">
</script>
<html>
<head>
<title>ClearView Administration</title>
<link rel="stylesheet" type="text/css" href="/css/default.css" />
<script type="text/javascript" src="/javascript/both.js"></script>
<script type="text/javascript" src="/javascript/global.js"></script>
<script type="text/javascript" src="/javascript/ajax.js"></script>
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
		    <td><b>Build Locations</b></td>
		    <td align="right"><asp:LinkButton id="btnNew" runat="server" Text="Add New" OnClick="btnNew_Click" /></td>
		</tr>
		<tr><td colspan="2">&nbsp;</td></tr>
		<tr>
		    <td colspan="2" align="center">
                <asp:Panel ID="panView" runat="server" Visible="false">
                    <table width="100%" border="0" cellpadding="0" cellspacing="0">
                        <tr>
                            <td>
                                <asp:repeater ID="rptView" runat="server">
                                    <HeaderTemplate>
                                        <table width="100%" cellpadding="3" cellspacing="0" border="0" align="center">
                                            <tr bgcolor="#CCCCCC">
                                                <td class="bold">&nbsp;</td>
                                                <td class="bold"><asp:linkbutton ID="lnkModel" Text="Model" CommandArgument="model" CssClass="bold" OnClick="OrderView" runat="server"/></td>
                                                <td class="bold">Location</td>
                                            </tr>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr bgcolor="#EFEFEF" onmouseover="AdminMouseOver(this);" onmouseout="AdminMouseOut(this);" onclick="window.navigate('<%# Request.Path + "?id=" + DataBinder.Eval(Container.DataItem, "id")%>');" class="default">
                                            <td width="5%" align="left">&nbsp;<asp:ImageButton ID="btnDelete" OnClick="btnDeleteLink_Click" runat="server" ToolTip="Delete" ImageAlign="AbsMiddle" ImageUrl="/admin/images/delete.gif" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' /></td>
                                            <td nowrap>&nbsp;<%# DataBinder.Eval(Container.DataItem, "model") %></td>
                                            <td width="100%" nowrap>&nbsp;<%# DataBinder.Eval(Container.DataItem, "class") %> / <%# DataBinder.Eval(Container.DataItem, "environment") %> / <%# DataBinder.Eval(Container.DataItem, "location") %></td>
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

                <asp:Panel id="panAdd" runat="server" Visible="false">
                    <table width="95%" cellpadding="3" cellspacing="0" border="0" align="center">
                        <tr> 
                            <td class="default">Final Class:</td>
                            <td><asp:dropdownlist ID="ddlClass" CssClass="default" runat="server" Width="300"/></td>
                        </tr>
                        <tr> 
                            <td class="default">Final Environment:</td>
                            <td>
                                <asp:dropdownlist ID="ddlEnvironment" CssClass="default" runat="server" Enabled="false" Width="300">
                                    <asp:ListItem Value="-- Please select a Class --" />
                                </asp:dropdownlist>
                            </td>
                        </tr>
                        <tr>
                            <td class="cmdefault">Final Location:</td>
                            <td><%=strLocationFinal %></td>
                        </tr>
                        <tr>
                            <td colspan="2">&nbsp;</td>
                        </tr>
                        <tr> 
                            <td class="default">Build Class:</td>
                            <td><asp:dropdownlist ID="ddlClassBuild" CssClass="default" runat="server" Width="300"/></td>
                        </tr>
                        <tr> 
                            <td class="default">Build Environment:</td>
                            <td>
                                <asp:dropdownlist ID="ddlEnvironmentBuild" CssClass="default" runat="server" Enabled="false" Width="300">
                                    <asp:ListItem Value="-- Please select a Class --" />
                                </asp:dropdownlist>
                            </td>
                        </tr>
                        <tr>
                            <td class="cmdefault">Build Location:</td>
                            <td><%=strLocationBuild %></td>
                        </tr>
                        <tr>
                            <td colspan="2">&nbsp;</td>
                        </tr>
                        <tr>
                            <td class="cmdefault">Model:</td>
                            <td><asp:label ID="lblModel" CssClass="default" runat="server" />&nbsp;&nbsp;<asp:Button ID="btnModel" runat="server" CssClass="default" Width="25" Text="..." /></td>
                        </tr>
                        <tr> 
                            <td class="default">Enabled:</td>
                            <td><asp:CheckBox ID="chkEnabled" runat="server" Checked="true" /></td>
                        </tr>
                        <tr><td height="5" colspan="2"><img src="images/spacer.gif" width="1" height="5" /></td></tr>
                        <tr><td height="5" colspan="2">&nbsp;</td></tr>
                        <tr> 
                            <td>&nbsp;</td>
                            <td>
                                <asp:button ID="btnAdd" CssClass="default" runat="server" Text="Add" Width="75" OnClick="btnAdd_Click" />
                                <asp:button ID="btnDelete" CssClass="default" runat="server" Text="Delete" Width="75" OnClick="btnDelete_Click" />
                                <asp:button ID="btnCancel" CssClass="default" runat="server" Text="Cancel" Width="75" OnClick="btnCancel_Click" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
    </table>
        </div>
        </td>
        </tr>
        </table>
    <input type="hidden" id="hdnModel" runat="server" />
    <input type="hidden" id="hdnLocationFinal" runat="server" />
    <input type="hidden" id="hdnEnvironmentFinal" runat="server" />
    <input type="hidden" id="hdnLocationBuild" runat="server" />
    <input type="hidden" id="hdnEnvironmentBuild" runat="server" />
</form>
</body>
</html>
