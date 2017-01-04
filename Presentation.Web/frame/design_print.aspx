<%@ Page Language="C#" AutoEventWireup="True" CodeBehind="design_print.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.design_print" %>

<html>
<head runat="server">
<title runat="server">ClearView Design Summary</title>
<link rel="stylesheet" type="text/css" href="/css/default.css" />
<link rel="stylesheet" type="text/css" href="/css/default.css" />
<script src="/javascript/default.js"type="text/javascript"></script>
<script type="text/javascript" src="/javascript/ajax.js"></script>
<script type="text/javascript" src="/javascript/both.js"></script>
<script src="/javascript/swfobject.js"type="text/javascript"></script>

</head>
<body leftmargin="0" topmargin="0">
<form id="Form1" runat="server">
    <table width="600" cellpadding="6" cellspacing="0" border="0" align="center">
        <tr>
            <td colspan="4">
                <table width="100%" cellpadding="0" cellspacing="0" border="0" align="center">
                    <tr>
                        <td class="hugeheader">ClearView Design Summary # <asp:Label ID="lblID" runat="server" /></td>
                        <td align="right">
                            <a href="javascript:void(0);" onclick="window.print();"><img src='/images/print-icon.gif' border='0' align='absmiddle' />Print Page</a>
                            &nbsp;&nbsp;&nbsp;
                            <a href="javascript:void(0);" onclick="window.close();"><img src='/images/close-icon.gif' border='0' align='absmiddle' /><img src='/images/spacer.gif' border='0' width='5' height='1' />Close Window</a>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td valign="top" class="bold">Requested By:</td>
            <td valign="top"><asp:Label ID="lblRequestedBy" runat="server" /></td>
            <td valign="top" class="bold">Mnemonic:</td>
            <td valign="top"><asp:Label ID="lblMnemonic" runat="server" /></td>
        </tr>
        <tr>
            <td valign="top" class="bold">Requested On:</td>
            <td valign="top"><asp:Label ID="lblRequestedOn" runat="server" /></td>
            <td valign="top" class="bold">Mnemonic Status:</td>
            <td valign="top"><asp:Label ID="lblMnemonicStatus" runat="server" /></td>
        </tr>
        <tr>
            <td valign="top" class="bold">Operating System:</td>
            <td valign="top"><asp:Label ID="lblOS" runat="server" /></td>
            <td valign="top" class="bold">Mnemonic RTO:</td>
            <td valign="top"><asp:Label ID="lblMnemonicRTO" runat="server" /></td>
        </tr>
        <tr>
            <td valign="top" class="bold">Platform Type:</td>
            <td valign="top"><asp:Label ID="lblServerType" runat="server" /></td>
            <td valign="top" class="bold">Platform Boot Type:</td>
            <td valign="top"><asp:Label ID="lblServerBootType" runat="server" /></td>
        </tr>
        <tr>
            <td valign="top" class="bold">Platform Size:</td>
            <td valign="top"><asp:Label ID="lblSize" runat="server" /></td>
            <td valign="top" class="bold">Special Hardware:</td>
            <td valign="top"><asp:Label ID="lblSpecial" runat="server" /></td>
        </tr>
        <tr>
            <td valign="top" class="bold">High Availability:</td>
            <td valign="top"><asp:Label ID="lblHA" runat="server" /></td>
            <td valign="top" class="bold">Middleware:</td>
            <td valign="top"><asp:Label ID="lblMiddleware" runat="server" /></td>
        </tr>
        <tr style="display:none">
            <td valign="top" class="bold">Legacy Design ID:</td>
            <td valign="top"><asp:Label ID="lblAnswerID" runat="server" /></td>
            <td valign="top" class="bold">Software:</td>
            <td valign="top"><asp:Label ID="lblSoftware" runat="server" /></td>
        </tr>
        <tr>
            <td valign="top" class="bold">Storage Amount:</td>
            <td valign="top"><asp:Label ID="lblStorage" runat="server" /></td>
            <td valign="top" class="bold">Mainframe:</td>
            <td valign="top"><asp:Label ID="lblMainframe" runat="server" /></td>
        </tr>
        <tr>
            <td valign="top" class="bold">Commitment Date:</td>
            <td valign="top"><asp:Label ID="lblDate" runat="server" /></td>
            <td valign="top" class="bold">Confidence:</td>
            <td valign="top"><asp:Label ID="lblConfidence" runat="server" /></td>
        </tr>
        <tr>
            <td valign="top" class="bold">Quantity:</td>
            <td valign="top"><asp:Label ID="lblQuantity" runat="server" /></td>
            <td valign="top" class="bold">Printed:</td>
            <td valign="top"><%=DateTime.Now.ToString() %></td>
        </tr>
        <tr>
            <td valign="top" class="bold">Location:</td>
            <td valign="top" colspan="3"><asp:Label ID="lblLocation" runat="server" /></td>
        </tr>
        <tr>
            <td colspan="4" width="100%"><img src="/images/spacer.gif" border="0" height="1" width="1" /></td>
        </tr>
        <tr>
            <td colspan="4" class="biggerbold">The following solution has been selected:</td>
        </tr>
        <tr bgcolor='<%=strHighlight %>'>
            <td valign="top" class="bold">Solution Determined:</td>
            <td valign="top" colspan="3"><asp:Label ID="lblSolution" runat="server" /></td>
        </tr>
        <tr>
            <td colspan="4" width="100%"><img src="/images/spacer.gif" border="0" height="1" width="1" /></td>
        </tr>
        <tr id="trException1" runat="server" visible="false">
            <td width="100%" colspan="4" class="bold">The following comments were provided to explain the need for the exception:</td>
        </tr>
        <tr id="trException2" runat="server" visible="false">
            <td width="100%" colspan="4">
                <table cellpadding="2" cellspacing="2" border="0">
                    <tr>
                        <td valign="top"><img src="/images/icon_answer.gif" border="0" align="absmiddle" /></td>
                        <td valign="top"><asp:Label ID="lblException" runat="server" /></td>
                    </tr>
                    <tr>
                        <td colspan="2"><img src="/images/spacer.gif" border="0" height="1" width="1" /></td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="4">
                <asp:Panel ID="panStorage" runat="server" Visible="false">
                    <table width="100%" cellpadding="5" cellspacing="2" border="0">
                        <tr>
                            <td class="bigger" colspan="2"><b>Storage Configuration</b></td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <table width="100%" cellpadding="4" cellspacing="0" border="0" align="center" style="border:solid 1px #CCCCCC">
                                    <tr bgcolor="#EEEEEE">
                                        <td style='display:<%=boolWindows ? "inline" : "none" %>' nowrap><b><u>Drive:</u></b></td>
                                        <td style='display:<%=boolWindows ? "inline" : "none" %>' width="100%"><b><u>Mount Point:</u></b></td>
                                        <td style='display:<%=boolWindows == false ? "inline" : "none" %>' width="100%"><b><u>Filesystem:</u></b></td>
                                        <td nowrap><b><u>Shared:</u></b></td>
                                        <td nowrap><b><u>Size:</u></b></td>
                                    </tr>
                                    <tr id="trStorageApp" runat="server" visible="false">
                                        <td nowrap>E:</td>
                                        <td width="100%">&nbsp;&nbsp;<span class="footer">(Reserved: Application Drive)</span></td>
                                        <td nowrap align="center"><asp:CheckBox ID="chkStorageSizeE" runat="server" Enabled="false" Checked="false" /></td>
                                        <td nowrap align="right"><asp:Label ID="txtStorageSizeE" runat="server" /> GB</td>
                                    </tr>
                                    <asp:repeater ID="rptStorage" runat="server">
                                        <ItemTemplate>
                                            <tr>
                                                <td style='display:<%=boolWindows ? "inline" : "none" %>' valign="top" nowrap><%# DataBinder.Eval(Container.DataItem, "letter") %></td>
                                                <td valign="top" width="100%"><%# DataBinder.Eval(Container.DataItem, "path") %></td>
                                                <td valign="top" nowrap align="center"><asp:CheckBox ID="chkStorageSize" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "shared") %>' Enabled="false" /></td>
                                                <td valign="top" nowrap align="right"><%# DataBinder.Eval(Container.DataItem, "size") %> GB</td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:repeater>
                                </table>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td colspan="4">
                <table width="100%" cellpadding="5" cellspacing="2" border="0">
                    <tr>
                        <td class="bigger" colspan="2"><b>Account Configuration</b></td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <table width="100%" cellpadding="4" cellspacing="0" border="0" style="border:solid 1px #CCCCCC">
                                <tr bgcolor="#EEEEEE">
                                    <td><b><u>User:</u></b></td>
                                    <td><b><u>Permission:</u></b></td>
                                </tr>
                                <asp:repeater ID="rptAccounts" runat="server">
                                    <ItemTemplate>
                                        <tr>
                                            <td valign="top"><%# oUser.GetFullName(Int32.Parse(DataBinder.Eval(Container.DataItem, "userid").ToString())) %> (<%# oUser.GetName(Int32.Parse(DataBinder.Eval(Container.DataItem, "userid").ToString()))%>)</td>
                                            <td valign="top"><asp:Label ID="lblPermissions" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "access") %>' ToolTip='<%# DataBinder.Eval(Container.DataItem, "remote") %>' /></td>
                                        </tr>
                                    </ItemTemplate>
                                    <AlternatingItemTemplate>
                                        <tr bgcolor="F6F6F6">
                                            <td valign="top"><%# oUser.GetFullName(Int32.Parse(DataBinder.Eval(Container.DataItem, "userid").ToString()))%> (<%# oUser.GetName(Int32.Parse(DataBinder.Eval(Container.DataItem, "userid").ToString()))%>)</td>
                                            <td valign="top"><asp:Label ID="lblPermissions" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "access") %>' ToolTip='<%# DataBinder.Eval(Container.DataItem, "remote") %>' /></td>
                                        </tr>
                                    </AlternatingItemTemplate>
                                </asp:repeater>
                                <tr>
                                    <td colspan="2">
                                        <asp:Label ID="lblNone" runat="server" CssClass="default" Visible="false" Text="<img src='/images/spacer.gif' border='0' height='1' width='25' /><img src='/images/alert.gif' border='0' align='absmiddle'> You have not added any accounts to this request" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="4">
                <asp:Panel ID="panBackup" runat="server" Visible="false">
                    <table width="100%" cellpadding="5" cellspacing="2" border="0">
                        <tr>
                            <td colspan="2">
                                <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                    <tr>
                                        <td class="bigger"><b>Backup Configuration</b></td>
                                        <td align="right"><b>B</b> = Backup Acceptable</td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td nowrap class="bold">Backup Frequency:</td>
                            <td width="100%"><asp:Label ID="lblFrequency" runat="server" /></td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <table width="100%" cellpadding="4" cellspacing="0" border="0" style="border:solid 1px #CCCCCC">
                                    <tr>
                                        <td></td>
                                        <td colspan="12" align="center" style="color: #FFF; background-color: #333"><b>AM</b></td>
                                        <td colspan="12" align="center" style="color: #000; background-color: #AAA"><b>PM</b></td>
                                    </tr>
                                    <tr bgcolor="#EEEEEE">
                                        <td></td>
                                        <td>12</td>
                                        <td>1</td>
                                        <td>2</td>
                                        <td>3</td>
                                        <td>4</td>
                                        <td>5</td>
                                        <td>6</td>
                                        <td>7</td>
                                        <td>8</td>
                                        <td>9</td>
                                        <td>10</td>
                                        <td>11</td>
                                        <td>12</td>
                                        <td>1</td>
                                        <td>2</td>
                                        <td>3</td>
                                        <td>4</td>
                                        <td>5</td>
                                        <td>6</td>
                                        <td>7</td>
                                        <td>8</td>
                                        <td>9</td>
                                        <td>10</td>
                                        <td>11</td>
                                    </tr>
                                    <%=strBackup.ToString()%>
                                </table>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td colspan="4">
                <asp:Panel ID="panExclusions" runat="server" Visible="false">
                    <table width="100%" cellpadding="5" cellspacing="2" border="0">
                        <tr>
                            <td class="bigger" colspan="2"><b>Backup Exclusion Configuration</b></td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <table width="100%" cellpadding="4" cellspacing="0" border="0" style="border:solid 1px #CCCCCC">
                                    <tr bgcolor="#EEEEEE">
                                        <td><b><u>Path:</u></b></td>
                                    </tr>
                                    <asp:repeater ID="rptExclusions" runat="server">
                                        <ItemTemplate>
                                            <tr>
                                                <td valign="top" width="100%"><%# DataBinder.Eval(Container.DataItem, "path") %></td>
                                            </tr>
                                        </ItemTemplate>
                                        <AlternatingItemTemplate>
                                            <tr bgcolor="F6F6F6">
                                                <td valign="top" width="100%"><%# DataBinder.Eval(Container.DataItem, "path") %></td>
                                            </tr>
                                        </AlternatingItemTemplate>
                                    </asp:repeater>
                                    <tr>
                                        <td colspan="2">
                                            <asp:Label ID="lblExclusion" runat="server" CssClass="default" Visible="false" Text="<img src='/images/spacer.gif' border='0' height='1' width='25' /><img src='/images/alert.gif' border='0' align='absmiddle'> There are no backup exclusions" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td colspan="4">
                <asp:Panel ID="panMaintenance" runat="server" Visible="false">
                    <table width="100%" cellpadding="5" cellspacing="2" border="0">
                        <tr>
                            <td colspan="2">
                                <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                    <tr>
                                        <td class="bigger"><b>Maintenance Window Configuration</b></td>
                                        <td align="right"><b>M</b> = Maintenance Acceptable</td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <table width="100%" cellpadding="4" cellspacing="0" border="0" style="border:solid 1px #CCCCCC">
                                    <tr>
                                        <td></td>
                                        <td colspan="12" align="center" style="color: #FFF; background-color: #333"><b>AM</b></td>
                                        <td colspan="12" align="center" style="color: #000; background-color: #AAA"><b>PM</b></td>
                                    </tr>
                                    <tr bgcolor="#EEEEEE">
                                        <td></td>
                                        <td>12</td>
                                        <td>1</td>
                                        <td>2</td>
                                        <td>3</td>
                                        <td>4</td>
                                        <td>5</td>
                                        <td>6</td>
                                        <td>7</td>
                                        <td>8</td>
                                        <td>9</td>
                                        <td>10</td>
                                        <td>11</td>
                                        <td>12</td>
                                        <td>1</td>
                                        <td>2</td>
                                        <td>3</td>
                                        <td>4</td>
                                        <td>5</td>
                                        <td>6</td>
                                        <td>7</td>
                                        <td>8</td>
                                        <td>9</td>
                                        <td>10</td>
                                        <td>11</td>
                                    </tr>
                                    <%=strMaintenance.ToString()%>
                                </table>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td colspan="4">
                <table width="100%" cellpadding="5" cellspacing="2" border="0">
                    <tr>
                        <td class="bigger" colspan="2"><b>Approval Process</b></td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <table width="100%" cellpadding="4" cellspacing="0" border="0" style="border:solid 1px #CCCCCC">
                                <tr bgcolor="#EEEEEE">
                                    <td></td>
                                    <td><b><u>Approver:</u></b></td>
                                    <td><b><u>Status:</u></b></td>
                                    <td><b><u>Notified On:</u></b></td>
                                    <td><b><u>Completed On:</u></b></td>
                                    <td></td>
                                </tr>
                                <asp:repeater ID="rptWorkflow" runat="server">
                                    <ItemTemplate>
                                        <tr bgcolor='<%# DataBinder.Eval(Container.DataItem, "status").ToString() == "Active" ? "" : "" %>'>
                                            <td valign="top"><img src='/images/<%# DataBinder.Eval(Container.DataItem, "status") %>.gif' border='0' /></td>
                                            <td valign="top" width="30%" class="tableheader"><%# DataBinder.Eval(Container.DataItem, "title")%></td>
                                            <td valign="top" width="20%"><%# DataBinder.Eval(Container.DataItem, "status") %></td>
                                            <td valign="top" width="25%"><%# DataBinder.Eval(Container.DataItem, "created") %></td>
                                            <td valign="top" width="25%"><%# DataBinder.Eval(Container.DataItem, "completed") %></td>
                                        </tr>
                                        <tr style='display:<%# DataBinder.Eval(Container.DataItem, "approver").ToString() == "" ? "none" : "inline" %>'>
                                            <td></td>
                                            <td colspan="4"><img src="/images/down_right.gif" border="0" align="absmiddle" /> <%# DataBinder.Eval(Container.DataItem, "approver") %></td>
                                        </tr>
                                        <tr style='display:<%# DataBinder.Eval(Container.DataItem, "reason").ToString() == "" ? "none" : "inline" %>'>
                                            <td></td>
                                            <td colspan="4"><img src="/images/icon_answer.gif" border="0" align="absmiddle" /> Comments:</td>
                                        </tr>
                                        <tr style='display:<%# DataBinder.Eval(Container.DataItem, "reason").ToString() == "" ? "none" : "inline" %>'>
                                            <td></td>
                                            <td colspan="4"><%# DataBinder.Eval(Container.DataItem, "reason") %></td>
                                        </tr>
                                    </ItemTemplate>
                                    <AlternatingItemTemplate>
                                        <tr bgcolor='<%# DataBinder.Eval(Container.DataItem, "status").ToString() == "Active" ? "" : "#F6F6F6" %>'>
                                            <td valign="top"><img src='/images/<%# DataBinder.Eval(Container.DataItem, "status") %>.gif' border='0' /></td>
                                            <td valign="top" width="30%" class="tableheader"><%# DataBinder.Eval(Container.DataItem, "title")%></td>
                                            <td valign="top" width="20%"><%# DataBinder.Eval(Container.DataItem, "status") %></td>
                                            <td valign="top" width="25%"><%# DataBinder.Eval(Container.DataItem, "created") %></td>
                                            <td valign="top" width="25%"><%# DataBinder.Eval(Container.DataItem, "completed") %></td>
                                        </tr>
                                        <tr bgcolor='<%# DataBinder.Eval(Container.DataItem, "status").ToString() == "Active" ? "" : "#F6F6F6" %>' style='display:<%# DataBinder.Eval(Container.DataItem, "approver").ToString() == "" ? "none" : "inline" %>'>
                                            <td></td>
                                            <td colspan="4"><img src="/images/down_right.gif" border="0" align="absmiddle" /> <%# DataBinder.Eval(Container.DataItem, "approver") %></td>
                                        </tr>
                                        <tr bgcolor='<%# DataBinder.Eval(Container.DataItem, "status").ToString() == "Active" ? "" : "#F6F6F6" %>' style='display:<%# DataBinder.Eval(Container.DataItem, "reason").ToString() == "" ? "none" : "inline" %>'>
                                            <td></td>
                                            <td colspan="4"><img src="/images/icon_answer.gif" border="0" align="absmiddle" /> Comments:</td>
                                        </tr>
                                        <tr bgcolor='<%# DataBinder.Eval(Container.DataItem, "status").ToString() == "Active" ? "" : "#F6F6F6" %>' style='display:<%# DataBinder.Eval(Container.DataItem, "reason").ToString() == "" ? "none" : "inline" %>'>
                                            <td></td>
                                            <td colspan="4"><%# DataBinder.Eval(Container.DataItem, "reason") %></td>
                                        </tr>
                                    </AlternatingItemTemplate>
                                </asp:repeater>
                                <tr>
                                    <td colspan="10">
                                        <asp:Label ID="lblWorkflow" runat="server" CssClass="default" Visible="false" Text="<img src='/images/spacer.gif' border='0' height='1' width='25' /><img src='/images/alert.gif' border='0' align='absmiddle'> There are no approvers" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="2"><img src="/images/spacer.gif" border="0" width="300" height="1" /></td>
            <td colspan="2"><img src="/images/spacer.gif" border="0" width="300" height="1" /></td>
        </tr>
    </table>
</form>
</body>
</html>