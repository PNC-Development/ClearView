<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="scheduler.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.scheduler" %>

<script type="text/javascript">
</script>

<html>
<head>
<title>ClearView Administration</title>
<link rel="stylesheet" type="text/css" href="/css/default.css" />
<script type="text/javascript" src="/javascript/global.js"></script>
<script type="text/javascript" src="/javascript/both.js"></script>
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
		    <td><b>Scheduler</b></td>
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
                                                <td width="30%" class="bold"><asp:linkbutton ID="lnkName" Text="Name" CommandArgument="name" CssClass="bold" OnClick="OrderView" runat="server"/></td>
                                                <td width="25%" class="bold"><asp:linkbutton ID="lnkStatus" Text="Status" CommandArgument="status" CssClass="bold" OnClick="OrderView" runat="server"/></td>
                                                <td width="20%" class="bold"><asp:linkbutton ID="lnkLast" Text="Last Run" CommandArgument="last" CssClass="bold" OnClick="OrderView" runat="server"/></td>
                                                <td width="20%" class="bold"><asp:linkbutton ID="lnkError" Text="Last Result" CommandArgument="error" CssClass="bold" OnClick="OrderView" runat="server"/></td>
                                                <td class="bold" align="center"><asp:linkbutton ID="lnkEnabled" Text="Enabled" CommandArgument="enabled" CssClass="bold" OnClick="OrderView" runat="server"/></td>
                                            </tr>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr bgcolor="#EFEFEF" onmouseover="AdminMouseOver(this);" onmouseout="AdminMouseOut(this);" onclick="window.navigate('<%# Request.Path + "?id=" + DataBinder.Eval(Container.DataItem, "id")%>');" class="default">
                                            <td width="5%" align="left">&nbsp;<asp:ImageButton ID="btnDelete" OnClick="btnDeleteLink_Click" runat="server" ToolTip="Delete" ImageAlign="AbsMiddle" ImageUrl="/admin/images/delete.gif" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' /></td>
                                            <td width="30%">&nbsp;<%# DataBinder.Eval(Container.DataItem, "name") %></td>
                                            <td width="25%">&nbsp;<%# (DataBinder.Eval(Container.DataItem, "status").ToString() == "-1" ? "About to Run Once" : (DataBinder.Eval(Container.DataItem, "status").ToString() == "1" ? "Running" : "Idle")) %></td>
                                            <td width="20%">&nbsp;<%# DataBinder.Eval(Container.DataItem, "last") %></td>
                                            <td width="20%">&nbsp;<%# (DataBinder.Eval(Container.DataItem, "error").ToString() == "" ? "Never Run" : (DataBinder.Eval(Container.DataItem, "error").ToString() == "1" ? "<span class=\"bigred\">Error</span>" : (DataBinder.Eval(Container.DataItem, "error").ToString() == "0" ? "Success" : ""))) %></td>
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
                                                <td class="default">Name:</td>
                                                <td><asp:TextBox ID="txtName" CssClass="default" runat="server" Width="400" MaxLength="100"/></td>
                                            </tr>
                                            <tr> 
                                                <td class="default" width="100px">Status:</td>
                                                <td>
                                                    <asp:DropDownList ID="ddlStatus" CssClass="default" runat="server" Width="100" />
                                                </td>
                                            </tr>
                                            <tr> 
                                                <td class="default">Description:</td>
                                                <td><asp:TextBox ID="txtDescription" CssClass="default" runat="server" Width="600" TextMode="MultiLine" Rows="4"/></td>
                                            </tr>
                                            <tr> 
                                                <td class="default" width="100px">Authenticate Using:</td>
                                                <td>
                                                    <asp:DropDownList ID="ddlEnvironment" CssClass="default" runat="server" >
                                                        <asp:ListItem Value="200" Text="PNCENG Credentials" />
                                                        <asp:ListItem Value="900" Text="PNCADSTEST Credentials" />
                                                        <asp:ListItem Value="999" Text="PNCNT Credentials" />
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr> 
                                                <td class="default">Day(s):</td>
                                                <td><asp:TextBox ID="txtDays" CssClass="default" runat="server" Width="400" MaxLength="100"/></td>
                                            </tr>
                                            <tr> 
                                                <td class="default"></td>
                                                <td>
                                                    (leave blank for every day)<br />
                                                    <b>Legend:</b>
                                                    <ul>
                                                        <li>1, 15 (for 1st and 15th of every month)</li>
                                                        <li>Su, Mo, Tu, We, Th, Fr, Sa (days of the week)</li>
                                                        <li>L (for last day of month)</li>
                                                    </ul>
                                                    <b>Example:</b> 1, Mo, We, Fr, L = First of the month, every Monday, Wednesday and Friday, and the last day of the month
                                                    </ul>
                                                </td>
                                            </tr>
                                            <tr> 
                                                <td class="default">Time(s):</td>
                                                <td><asp:TextBox ID="txtTimes" CssClass="default" runat="server" Width="400" MaxLength="100"/></td>
                                            </tr>
                                            <tr> 
                                                <td class="default"></td>
                                                <td>Example: 1:00 PM, 6:15 AM</td>
                                            </tr>
                                            <tr> 
                                                <td class="default">Server:</td>
                                                <td><asp:TextBox ID="txtServer" CssClass="default" runat="server" Width="150" MaxLength="30"/> (leave blank if local)</td>
                                            </tr>
                                            <tr> 
                                                <td class="default">Parameters:</td>
                                                <td><asp:TextBox ID="txtParameters" CssClass="default" runat="server" Width="600" TextMode="MultiLine" Rows="5"/></td>
                                            </tr>
                                            <tr> 
                                                <td class="default">Highest Privledges:</td>
                                                <td><asp:CheckBox ID="chkPrivledges" runat="server" Checked="true" /></td>
                                            </tr>
                                            <tr> 
                                                <td class="default">Interactive:</td>
                                                <td><asp:CheckBox ID="chkInteractive" runat="server" Checked="true" /></td>
                                            </tr>
                                            <tr> 
                                                <td class="default">Timeout:</td>
                                                <td><asp:TextBox ID="txtTimeout" CssClass="default" runat="server" Width="75" MaxLength="10"/> (in minutes)</td>
                                            </tr>
                                            <tr> 
                                                <td class="default">Enabled:</td>
                                                <td><asp:CheckBox ID="chkEnabled" runat="server" Checked="true" /></td>
                                            </tr>
                                        </table>
                                    </div>
                                    <div style="display:none">
                                        <p> 
                                            Number of Rows:<br /><asp:TextBox ID="txtRows" CssClass="default" runat="server" Width="100" MaxLength="10" Text="20"/> <asp:Button ID="btnRefresh" runat="server" Text="Refresh" OnClick="btnRefresh_Click" />
                                        </p>
                                        <table width="100%" cellpadding="4" cellspacing="0" border="0" style="border:solid 1px #CCCCCC; background-color:#FFFFFF">
                                            <tr bgcolor="#EEEEEE">
                                                <td></td>
                                                <td nowrap><b>Detail</b></td>
                                                <td nowrap><b>Time</b></td>
                                            </tr>
                                            <asp:repeater ID="rptLogs" runat="server">
                                                <ItemTemplate>
                                                    <tr>
                                                        <td align="center"><img src='<%# (DataBinder.Eval(Container.DataItem, "error").ToString() == "1") ? "/images/cancel.gif" : "/images/check_small.gif" %>' border="0" /></td>
                                                        <td width="100%">&nbsp;<%# DataBinder.Eval(Container.DataItem, "detail") %></td>
                                                        <td nowrap>&nbsp;<%# DataBinder.Eval(Container.DataItem, "created") %></td>
                                                    </tr>
                                                </ItemTemplate>
                                                <AlternatingItemTemplate>
                                                    <tr style="background:#F6F6F6">
                                                        <td align="center"><img src='<%# (DataBinder.Eval(Container.DataItem, "error").ToString() == "1") ? "/images/cancel.gif" : "/images/check_small.gif" %>' border="0" /></td>
                                                        <td width="100%">&nbsp;<%# DataBinder.Eval(Container.DataItem, "detail") %></td>
                                                        <td nowrap>&nbsp;<%# DataBinder.Eval(Container.DataItem, "created") %></td>
                                                    </tr>
                                                </AlternatingItemTemplate>
                                            </asp:repeater>
                                            <tr>
                                                <td colspan="3">
                                                    <asp:Label ID="lblLogs" runat="server" CssClass="default" Visible="false" Text="<img src='/images/alert.gif' border='0' align='absmiddle'> There is no history..." />
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                            </td>
                        </tr>
                    </table>
                    <p>&nbsp;</p>
                </asp:Panel>
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
