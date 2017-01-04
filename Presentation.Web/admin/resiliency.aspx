<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="resiliency.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.resiliency" %>



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
		    <td><b>Resiliency</b></td>
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
                    <table width="95%" cellpadding="3" cellspacing="0" border="0" align="center">
                        <tr> 
                            <td class="default">Name:</td>
                            <td><asp:textbox ID="txtName" CssClass="default" runat="server" Width="300" MaxLength="100"/></td>
                        </tr>
                        <tr> 
                            <td class="default">BIR:</td>
                            <td><asp:CheckBox ID="chkBIR" runat="server" /></td>
                        </tr>
                        <tr> 
                            <td class="default">Minimum:</td>
                            <td><asp:textbox ID="txtMin" CssClass="default" runat="server" Width="100" MaxLength="10"/> HRs</td>
                        </tr>
                        <tr> 
                            <td class="default">Maximum:</td>
                            <td><asp:textbox ID="txtMax" CssClass="default" runat="server" Width="100" MaxLength="10"/> HRs</td>
                        </tr>
                        <tr> 
                            <td class="default">Enabled:</td>
                            <td><asp:CheckBox ID="chkEnabled" runat="server" Checked="true" /></td>
                        </tr>
                        <tr> 
                            <td class="cmdefault">&nbsp;</td>
                            <td><asp:Button ID="btnOrder" runat="server" Text="Change Order" Width="150" CssClass="default" /></td>
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
                        <tr>
                            <td colspan="2">&nbsp;</td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <table border="0" cellSpacing="2" cellPadding="2">
                                    <tr>
                                        <td colspan="2"><b>Production Resiliency Locations</b></td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" valign="top">
                                            <table width="100%" cellpadding="4" cellspacing="0" border="0" style="border:solid 1px #CCCCCC; background-color:#FFFFFF">
                                                <tr bgcolor="#EEEEEE">
                                                    <td nowrap><b>Prodcution Location</b></td>
                                                    <td></td>
                                                    <td nowrap><b>Disaster Recovery Location</b></td>
                                                    <td nowrap><b></b></td>
                                                </tr>
                                                <asp:repeater ID="rptItems" runat="server">
                                                    <ItemTemplate>
                                                        <tr>
                                                            <td><%# DataBinder.Eval(Container.DataItem, "PROD") %></td>
                                                            <td><img src="/images/arrow_green_right.gif" border="0" align="absmiddle" /></td>
                                                            <td><%# DataBinder.Eval(Container.DataItem, "DR") %></td>
                                                            <td align="right">[<asp:LinkButton ID="btnDeleteLocation" runat="server" CssClass="default" Text="Delete" OnClick="btnDeleteLocation_Click" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' />]</td>
                                                        </tr>
                                                    </ItemTemplate>
                                                </asp:repeater>
                                                <tr>
                                                    <td colspan="4">
                                                        <asp:Label ID="lblNone" runat="server" CssClass="default" Visible="false" Text="<img src='/images/alert.gif' border='0' align='absmiddle'> There are no items..." />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2"><b>Add New</b></td>
                                    </tr>
                                    <tr> 
                                        <td class="cmdefault">PROD Location:</td>
                                        <td><%=strLocationProd %></td>
                                    </tr>
                                    <tr> 
                                        <td class="cmdefault">DR Location:</td>
                                        <td><%=strLocationDR%></td>
                                    </tr>
                                    <tr> 
                                        <td>&nbsp;</td>
                                        <td><asp:Button ID="btnAddLocation" runat="server" CssClass="default" Width="75" Text="Add" OnClick="btnAddLocation_Click" /></td>
                                    </tr>
                                </table>
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
    <input type="hidden" id="hdnId" runat="server" />
    <input type="hidden" id="hdnOrder" runat="server" />
    <input type="hidden" id="hdnLocationProd" runat="server" />
    <input type="hidden" id="hdnLocationDR" runat="server" />
</form>
</body>
</html>
