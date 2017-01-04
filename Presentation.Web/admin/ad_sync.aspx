<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ad_sync.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.ad_sync" %>
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
		    <td><b>AD Sync</b></td>
		    <td align="right">
		    </td>
		</tr>
		<tr><td colspan="2">&nbsp;</td></tr>
		<tr>
		    <td colspan="2" align="center">
                    <table width="95%" cellpadding="3" cellspacing="0" border="0" align="center">
                        <tr> 
                            <td>
                                <asp:button ID="btnSyncNow" CssClass="default" runat="server" Text="Synchronize Now" Width="150" OnClick="btnSyncNow_Click" /> 
                                <asp:CheckBox ID="chkSyncNow" runat="server" CssClass="default" Text="Update Changed Objects" />
                            </td>
                            <td align="right" class="bold">
                                <div id="divWait" runat="server" style="display:none">
                                    <img src="/images/spacer.gif" border="0" width="20" height="1" /><img src="/images/please_wait.gif" border="0" align="absmiddle" /> Please Wait...
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <table width="100%" border="0" cellpadding="5" cellspacing="0">
                                    <tr>
                                        <td class="bold">Previous Sync's</td>
                                        <td align="right">Next Series to Sync: <asp:TextBox ID="txtSync" runat="Server" CssClass="default" Width="75" MaxLength="3" /> <asp:Button ID="btnSync" runat="server" Text="Update" Width="75" OnClick="btnSync_Click" />
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <div style="width:100%; height:300px; border: solid 1px #CCCCCC; overflow:auto">
                                            <asp:repeater ID="rptImport" runat="server">
                                                <HeaderTemplate>
                                                    <table width="100%" cellpadding="3" cellspacing="0" border="0" align="center">
                                                        <tr bgcolor="#CCCCCC">
                                                            <td width="10%" class="bold">ID</td>
                                                            <td width="10%" class="bold">Creates</td>
                                                            <td width="10%" class="bold">Updates</td>
                                                            <td width="70%" class="bold">Modified</td>
                                                        </tr>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <tr bgcolor="<%# DataBinder.Eval(Container.DataItem, "id").ToString() == Request.QueryString["id"] ? "#99FF99" : "#EFEFEF" %>" class="default">
                                                        <td width="10%">&nbsp;<%# "<a href=\"" + Request.Path + "?id=" + DataBinder.Eval(Container.DataItem, "id").ToString() + "\">" + DataBinder.Eval(Container.DataItem, "ad_sync") + "</a>"%></td>
                                                        <td width="10%">&nbsp;<%# DataBinder.Eval(Container.DataItem, "creates") %></td>
                                                        <td width="10%">&nbsp;<%# DataBinder.Eval(Container.DataItem, "updates") %></td>
                                                        <td width="70%">&nbsp;<%# DataBinder.Eval(Container.DataItem, "modified") %></td>
                                                    </tr>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    </table>
                                                </FooterTemplate>
                                            </asp:repeater>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2"><asp:TextBox ID="txtImport" runat="server" CssClass="default" Width="100%" Rows="50" TextMode="multiLine" /></td>
                        </tr>
                    </table>
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
