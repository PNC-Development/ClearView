<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="log.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.log" %>

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
		    <td colspan="2" align="center">
                <table width="100%" border="0" cellpadding="2" cellspacing="0">
                    <tr style="display: none">
                        <td><asp:TextBox ID="txtDelete" runat="server" Width="100" CssClass="default" /> <asp:Button ID="btnDelete" runat="server" Text="Delete" Width="75" OnClick="btnDelete_Click" CssClass="default" /></td>
                        <td align="right"><asp:Button ID="btnDeleteAll" runat="server" Text="Delete All" Width="75" OnClick="btnDeleteAll_Click" CssClass="default" /></td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:repeater ID="rptView" runat="server">
                                <HeaderTemplate>
                                    <table width="100%" cellpadding="3" cellspacing="0" border="0" align="center">
                                        <tr bgcolor="#CCCCCC">
                                            <td nowrap class="bold"><asp:linkbutton ID="lnkID" Text="ID" CommandArgument="id" CssClass="bold" OnClick="OrderView" runat="server"/></td>
                                            <td nowrap class="bold"><asp:linkbutton ID="lnkAnswer" Text="Design" CommandArgument="answerid" CssClass="bold" OnClick="OrderView" runat="server"/></td>
                                            <td nowrap class="bold"><asp:linkbutton ID="lnkName" Text="Name" CommandArgument="name" CssClass="bold" OnClick="OrderView" runat="server"/></td>
                                            <td nowrap class="bold"><asp:linkbutton ID="lnkSerial" Text="Serial" CommandArgument="serial" CssClass="bold" OnClick="OrderView" runat="server"/></td>
                                            <td class="bold"><asp:linkbutton ID="lnkMessage" Text="Message" CommandArgument="message" CssClass="bold" OnClick="OrderView" runat="server"/></td>
                                            <td nowrap class="bold"><asp:linkbutton ID="lnkType" Text="Type" CommandArgument="type" CssClass="bold" OnClick="OrderView" runat="server"/></td>
                                            <td nowrap class="bold"><asp:linkbutton ID="lnkCreated" Text="Created" CommandArgument="created" CssClass="bold" OnClick="OrderView" runat="server"/></td>
                                        </tr>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr bgcolor="#EFEFEF" class="default">
                                        <td nowrap valign="top"><%# DataBinder.Eval(Container.DataItem, "id") %></td>
                                        <td nowrap valign="top"><%# DataBinder.Eval(Container.DataItem, "answerid") %></td>
                                        <td nowrap valign="top"><%# DataBinder.Eval(Container.DataItem, "name") %></td>
                                        <td nowrap valign="top"><%# DataBinder.Eval(Container.DataItem, "serial") %></td>
                                        <td valign="top"><%# DataBinder.Eval(Container.DataItem, "message") %></td>
                                        <td nowrap valign="top"><%# (DataBinder.Eval(Container.DataItem, "type").ToString() == "1") ? "/admin/images/check.gif" : "/admin/images/golddot.gif" %></td>
                                        <td nowrap valign="top"><%# DataBinder.Eval(Container.DataItem, "created") %></td>
                                    </tr>
                                </ItemTemplate>
                                <AlternatingItemTemplate>
                                    <tr bgcolor="#FFFFFF" class="default">
                                        <td nowrap valign="top"><%# DataBinder.Eval(Container.DataItem, "id") %></td>
                                        <td nowrap valign="top"><%# DataBinder.Eval(Container.DataItem, "answerid") %></td>
                                        <td nowrap valign="top"><%# DataBinder.Eval(Container.DataItem, "name") %></td>
                                        <td nowrap valign="top"><%# DataBinder.Eval(Container.DataItem, "serial") %></td>
                                        <td valign="top"><%# DataBinder.Eval(Container.DataItem, "message") %></td>
                                        <td nowrap valign="top"><%# (DataBinder.Eval(Container.DataItem, "type").ToString() == "1") ? "/admin/images/check.gif" : "/admin/images/golddot.gif" %></td>
                                        <td nowrap valign="top"><%# DataBinder.Eval(Container.DataItem, "created") %></td>
                                    </tr>
                                </AlternatingItemTemplate>
                                <FooterTemplate>
                                    </table>
                                </FooterTemplate>
                            </asp:repeater>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
        </td>
        </tr>
        </table>
</form>
</body>
</html>
