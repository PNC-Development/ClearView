<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="sve_clusters.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.sve_clusters" %>

<script type="text/javascript">
</script>
<html>
<head>
<title>ClearView Administration</title>
<link rel="stylesheet" type="text/css" href="/css/default.css" />
<script type="text/javascript" src="/javascript/global.js"></script>
<script type="text/javascript" src="/javascript/both.js"></script>
<script type="text/javascript" src="/javascript/ajax.js"></script>
<script type="text/javascript" src="/javascript/treeview.js"></script>
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
		    <td><b>Sun Virtual Environment Clusters</b></td>
		    <td align="right"><asp:LinkButton id="btnNew" runat="server" Text="Add New" OnClick="btnNew_Click" /></td>
		</tr>
		<tr><td colspan="2">&nbsp;</td></tr>
		<tr>
		    <td colspan="2">
                <asp:Panel ID="panView" runat="server" Visible="false">
                    <table width="100%" border="0" cellpadding="0" cellspacing="0">
                        <tr>
                            <td>
                                <asp:repeater ID="rptView" runat="server">
                                    <HeaderTemplate>
                                        <table width="100%" cellpadding="3" cellspacing="0" border="0" align="center">
                                            <tr bgcolor="#CCCCCC">
                                                <td class="bold">&nbsp;</td>
                                                <td width="50%" class="bold"><asp:linkbutton ID="lnkName" Text="Name" CommandArgument="name" CssClass="bold" OnClick="OrderView" runat="server"/></td>
                                                <td width="50%" class="bold"><asp:linkbutton ID="lnkModified" Text="Last Modified" CommandArgument="modified" CssClass="bold" OnClick="OrderView" runat="server"/></td>
                                                <td class="bold" align="center"><asp:linkbutton ID="lnkEnabled" Text="Enabled" CommandArgument="enabled" CssClass="bold" OnClick="OrderView" runat="server"/></td>
                                            </tr>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr bgcolor="#EFEFEF" onmouseover="AdminMouseOver(this);" onmouseout="AdminMouseOut(this);" onclick="window.navigate('<%# Request.Path + "?id=" + DataBinder.Eval(Container.DataItem, "id")%>');" class="default">
                                            <td width="5%" align="left">&nbsp;<asp:ImageButton ID="btnDelete" OnClick="btnDeleteLink_Click" runat="server" ToolTip="Delete" ImageAlign="AbsMiddle" ImageUrl="/admin/images/delete.gif" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' /></td>
                                            <td width="50%">&nbsp;<%# DataBinder.Eval(Container.DataItem, "name") %></td>
                                            <td width="50%">&nbsp;<%# DataBinder.Eval(Container.DataItem, "modified") %></td>
                                            <td align="center"><asp:ImageButton ID="btnEnable" runat="server" ImageAlign="AbsMiddle" ImageUrl='<%# (DataBinder.Eval(Container.DataItem, "enabled").ToString() == "1") ? "/admin/images/enabled.gif" : "/admin/images/disabled.gif" %>' CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' OnClick="btnEnable_Click" /></td>
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
                    <table width="100%" border="0" cellpadding="0" cellspacing="0">
                        <tr>
                            <td>
                                <asp:Panel ID="panSave" runat="server" Visible="false">
                                    <div class="bigCheck"><img src="/images/bigCheck.gif" border="0" align="absmiddle" />&nbsp;Information&nbsp;has&nbsp;been&nbsp;Updated</div>
                                </asp:Panel>
                            </td>
                            <td width="100%" align="right">
                                <asp:button ID="btnAdd" CssClass="default" runat="server" Text="Add" Width="75" OnClick="btnAdd_Click" />
                                <asp:button ID="btnDelete" CssClass="default" runat="server" Text="Delete" Width="75" OnClick="btnDelete_Click" />
                                <asp:button ID="btnCancel" CssClass="default" runat="server" Text="Cancel" Width="75" OnClick="btnCancel_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">&nbsp;</td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <%=strMenuTab1 %>
                                <div id="divMenu1">
                                    <br />
                                    <div style="display:none">
                                        <table width="95%" cellpadding="3" cellspacing="0" border="0" align="center">
                                            <tr> 
                                                <td class="default" width="100px">Name:</td>
                                                <td><asp:TextBox ID="txtName" CssClass="default" runat="server" Width="300" MaxLength="100"/></td>
                                            </tr>
                                            <tr> 
                                                <td class="default">Database:</td>
                                                <td><asp:CheckBox ID="chkDatabase" runat="server" /></td>
                                            </tr>
                                             <tr>
                                                <td class="default">Class:</td>
                                                <td><asp:DropDownList ID="ddlClass" CssClass="default" runat="server"/></td>
                                            </tr>
                                            <tr>
                                                <td nowrap>Resiliency:</td>
                                                <td><asp:DropDownList ID="ddlResiliency" runat="server" CssClass="default" Width="400" /></td>
                                            </tr>
                                            <tr> 
                                                <td class="default">Available:</td>
                                                <td><asp:CheckBox ID="chkAvailable" runat="server" /></td>
                                            </tr>
                                            <tr> 
                                                <td class="default">Specify Network Range(s):</td>
                                                <td><asp:CheckBox ID="chkNetworks" runat="server" /></td>
                                            </tr>
                                            <tr> 
                                                <td class="default">Comment(s):</td>
                                                <td><asp:TextBox ID="txtComments" runat="server" Width="500" TextMode="MultiLine" Rows="5" /></td>
                                            </tr>
                                            <tr> 
                                                <td class="default">Storage Present:</td>
                                                <td><asp:CheckBox ID="chkStorage" runat="server" /></td>
                                            </tr>
                                            <tr> 
                                                <td class="default">Trunking Enabled:</td>
                                                <td><asp:CheckBox ID="chkTrunking" runat="server" /></td>
                                            </tr>
                                            <tr> 
                                                <td class="default">Enabled:</td>
                                                <td><asp:CheckBox ID="chkEnabled" runat="server" Checked="true" /></td>
                                            </tr>
                                        </table>
                                    </div>
                                    <div style="display:none">
                                        <%=strDisabled %>
                                        <%=strNetworks %>
                                        <asp:TreeView ID="oTreeNetworks" runat="server" CssClass="default" ShowLines="true" NodeIndent="30">
                                        </asp:TreeView>
                                    </div>
                                    <div style="display:none">
                                        <%=strDisabled %>
                                        <table width="100%" border="0" cellSpacing="2" cellPadding="2">
                                            <tr>
                                                <td class="default">Location:</td>
                                                <td><%=strLocation %></td>
                                            </tr>
                                            <tr>
                                                <td>&nbsp;</td>
                                                <td><asp:Button ID="btnLocation" runat="server" CssClass="default" Width="75" Text="Add" OnClick="btnLocation_Click" /></td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" valign="top">
                                                    <table width="100%" cellpadding="4" cellspacing="0" border="0" style="border:solid 1px #CCCCCC; background-color:#FFFFFF">
                                                        <tr bgcolor="#EEEEEE">
                                                            <td nowrap></td>
                                                            <td nowrap><b>Location</b></td>
                                                        </tr>
                                                        <asp:repeater ID="rptLocations" runat="server">
                                                            <ItemTemplate>
                                                                <tr>
                                                                    <td align="left">&nbsp;<asp:ImageButton ID="btnDeleteLocation" OnClick="btnDeleteLocation_Click" runat="server" ToolTip="Delete" ImageAlign="AbsMiddle" ImageUrl="/admin/images/delete.gif" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' /></td>
                                                                    <td width="100%"><%# oLocation.GetFull(Int32.Parse(DataBinder.Eval(Container.DataItem, "addressid").ToString())) %></td>
                                                                </tr>
                                                            </ItemTemplate>
                                                        </asp:repeater>
                                                        <tr>
                                                            <td colspan="3">
                                                                <asp:Label ID="lblLocations" runat="server" CssClass="default" Visible="false" Text="<img src='/images/alert.gif' border='0' align='absmiddle'> There are no locations..." />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <div style="display:none">
                                        <asp:repeater ID="rptHosts" runat="server">
                                            <HeaderTemplate>
                                                <table width="100%" cellpadding="5" cellspacing="0" border="0" align="center">
                                                    <tr bgcolor="#CCCCCC">
                                                        <td class="bold">Servername</td>
                                                        <td class="bold">Serial Number</td>
                                                        <td class="bold">Model</td>
                                                    </tr>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <tr bgcolor="#EFEFEF" class="default">
                                                    <td>&nbsp;<%# DataBinder.Eval(Container.DataItem, "servername") %></td>
                                                    <td>&nbsp;<%# DataBinder.Eval(Container.DataItem, "serial") %></td>
                                                    <td>&nbsp;<%# DataBinder.Eval(Container.DataItem, "model") %></td>
                                                </tr>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                </table>
                                            </FooterTemplate>
                                        </asp:repeater>
                                        <br />
                                        <img src="/images/alert.gif" border="0" align="absmiddle" /> <b>NOTE:</b> To add a host to this list, you must search for the servername (via DataPoint) and then select the SVE cluster using the &quot;Administration&quot; tab.
                                    </div>
                                </div>
                            </td>
                        </tr>
                    </table>
                    <p>&nbsp;</p>
                </asp:Panel>
                </div>
            </td>
        </tr>
    </table>
        </div>
        </td>
        </tr>
        </table>
<input type="hidden" id="hdnLocation" runat="server" />
</form>
</body>
</html>
