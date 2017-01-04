<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="logins.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.logins" %>

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
		    <td><b>Logins</b></td>
		    <td align="right"><asp:LinkButton ID="btnRefresh" runat="server" Text="Refresh" OnClick="btnRefresh_Click" /></td>
		</tr>
		<tr><td colspan="2">&nbsp;</td></tr>
		<tr>
		    <td colspan="2" align="center">
                <table width="100%" border="0" cellpadding="2" cellspacing="0">
                    <tr>
                        <td colspan="2">
                            <asp:repeater ID="rptView" runat="server">
                                <HeaderTemplate>
                                    <table width="100%" cellpadding="3" cellspacing="0" border="0" align="center">
                                        <tr bgcolor="#CCCCCC">
                                            <td nowrap class="bold"><a href="#" class="bold">Username</a></td>
                                            <td nowrap class="bold"><a href="#" class="bold">XID</a></td>
                                            <td nowrap class="bold"><a href="#" class="bold">Logins</a></td>
                                            <td nowrap class="bold"><a href="#" class="bold">Last Login</a></td>
                                            <td nowrap class="bold"><a href="#" class="bold">First Login</a></td>
                                        </tr>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr bgcolor="#EFEFEF" class="default">
                                        <td nowrap><%# DataBinder.Eval(Container.DataItem, "username") %></td>
                                        <td nowrap><a href='http://epsweb.ntl-city.com/Subwebs/people/ISOrgChart/ISOrg.aspx?p=1&nd=<%# DataBinder.Eval(Container.DataItem, "xid") %>' target='_blank'><%# DataBinder.Eval(Container.DataItem, "xid") %></a></td>
                                        <td nowrap><%# DataBinder.Eval(Container.DataItem, "logins") %></td>
                                        <td nowrap><%# DataBinder.Eval(Container.DataItem, "last") %></td>
                                        <td nowrap><%# DataBinder.Eval(Container.DataItem, "first") %></td>
                                    </tr>
                                </ItemTemplate>
                                <AlternatingItemTemplate>
                                    <tr bgcolor="#FFFFFF" class="default">
                                        <td nowrap><%# DataBinder.Eval(Container.DataItem, "username") %></td>
                                        <td nowrap><a href='http://epsweb.ntl-city.com/Subwebs/people/ISOrgChart/ISOrg.aspx?p=1&nd=<%# DataBinder.Eval(Container.DataItem, "xid") %>' target='_blank'><%# DataBinder.Eval(Container.DataItem, "xid") %></a></td>
                                        <td nowrap><%# DataBinder.Eval(Container.DataItem, "logins") %></td>
                                        <td nowrap><%# DataBinder.Eval(Container.DataItem, "last") %></td>
                                        <td nowrap><%# DataBinder.Eval(Container.DataItem, "first") %></td>
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
