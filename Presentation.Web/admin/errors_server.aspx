<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="errors_server.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.errors_server" %>

<script type="text/javascript">
    function UpdateTextValue(oText, oNumber) {
        oNumber = document.getElementById(oNumber);
        oNumber.value = oText.value;
    }
</script>
<html>
<head>
<title>ClearView Administration</title>
<link rel="stylesheet" type="text/css" href="/css/default.css" />
<script type="text/javascript" src="/javascript/global.js"></script>
<script type="text/javascript" src="/javascript/default.js"></script>
<script type="text/javascript" src="/javascript/both.js"></script>
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
		    <td><b>Auto-Provisioning Server Errors</b></td>
		    <td align="right">&nbsp;</td>
		</tr>
		<tr><td colspan="2">&nbsp;</td></tr>
		<tr>
		    <td colspan="2" align="center">
                <asp:Panel ID="panAll" runat="server" Visible="false">
                <table width="100%" border="0" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <asp:TreeView ID="oTreeview" runat="server" ShowLines="true" NodeIndent="35">
                                <NodeStyle CssClass="default" />
                            </asp:TreeView>
                        </td>
                    </tr>
                </table>
               </asp:Panel>

                <asp:Panel ID="panError" runat="server" Visible="false">
                    <%=strError %>
                    <p></p>
                    <table width="100%" cellpadding="4" cellspacing="3" border="0">
                        <tr id="trIncident" runat="server" style="display:none">
                            <td>
                                <table width="100%" cellpadding="4" cellspacing="3" border="0">
                                    <tr>
                                        <td valign="top">Error:</td>
                                        <td valign="top"><asp:Label ID="lblIncident" runat="server" CssClass="default" /></td>
                                    </tr>
                                    <tr> 
                                        <td class="default">Compare:</td>
                                        <td><asp:textbox ID="txtCompare" CssClass="default" runat="server" Width="500" MaxLength="50" /></td>
                                    </tr>
                                    <tr> 
                                        <td class="default">Route:</td>
                                        <td>
                                            <asp:DropDownList ID="ddlRoute" CssClass="default" runat="server">
                                                <asp:ListItem Value="WAIT" Text="None - Wait 10 minutes and try again..." />
                                                <asp:ListItem Value="CLV ADMINS" />
                                                <asp:ListItem Value="CLV BACKUP" />
                                                <asp:ListItem Value="CLV WINDOWS" />
                                                <asp:ListItem Value="CLV LINUX" />
                                                <asp:ListItem Value="CLV VMWARE" />
                                                <asp:ListItem Value="CLV NETWORK" />
                                                <asp:ListItem Value="CLV DESKTOP" />
                                                <asp:ListItem Value="CLV STORAGE" />
                                                <asp:ListItem Value="CLV EMPLOYEE" />
                                                <asp:ListItem Value="CLV SECURITY" />
                                                <asp:ListItem Value="CLV INVENTORY" />
                                                <asp:ListItem Value="CLV FACILITY CLE" />
                                                <asp:ListItem Value="CLV FACILITY SUM" />
                                                <asp:ListItem Value="CLV FACILITY FIR" />
                                                <asp:ListItem Value="CLV FACILITY CIN" />
                                                <asp:ListItem Value="CLV FACILITY PGH" />
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr> 
                                        <td class="default">Automatic:</td>
                                        <td><asp:CheckBox ID="chkAutomatic" runat="server" /></td>
                                    </tr>
                                    <tr> 
                                        <td class="default">Message:</td>
                                        <td><asp:textbox ID="txtMessage" CssClass="default" runat="server" Width="500" TextMode="MultiLine" Rows="5" /></td>
                                    </tr>
                                    <tr> 
                                        <td class="default">Priority:</td>
                                        <td>
                                            <asp:DropDownList ID="ddlPriority" CssClass="default" runat="server">
                                                <asp:ListItem Value="0" Text="-- Default --" />
                                                <asp:ListItem Value="1" />
                                                <asp:ListItem Value="2" />
                                                <asp:ListItem Value="3" />
                                                <asp:ListItem Value="4" />
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top"></td>
                                        <td><asp:Button ID="btnCreate" runat="server" CssClass="default" Width="125" Text="Create Incident" OnClick="btnCreate_Click" /></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr id="trSolution" runat="server" style="display:none">
                            <td>
                                <table width="100%" cellpadding="4" cellspacing="3" border="0">
                                    <tr>
                                        <td valign="top">Error:</td>
                                        <td valign="top"><asp:Label ID="lblSolution" runat="server" CssClass="default" /></td>
                                    </tr>
                                    <tr>
                                        <td valign="top">Problem:</td>
                                        <td valign="top"><asp:TextBox ID="txtProblem" runat="server" CssClass="default" Width="500" TextMode="MultiLine" Rows="5" /></td>
                                    </tr>
                                    <tr>
                                        <td valign="top">Resolution:</td>
                                        <td valign="top"><asp:TextBox ID="txtResolution" runat="server" CssClass="default" Width="500" TextMode="MultiLine" Rows="5" /></td>
                                    </tr>
                                    <tr>
                                        <td valign="top">Case Code:</td>
                                        <td valign="top"><asp:DropDownList ID="ddlType" runat="server" CssClass="default" Width="200" /></td>
                                    </tr>
                                    <tr>
                                        <td valign="top">Type:</td>
                                        <td valign="top">
                                            <asp:DropDownList ID="ddlType2" runat="server" CssClass="default" Width="200" Enabled="false" >
                                                <asp:ListItem Value="-- Please select a case code --" />
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top">Attachment:</td>
                                        <td valign="top"><asp:FileUpload ID="txtFile" runat="server" CssClass="default" Width="500" Height="18" /></td>
                                    </tr>
                                    <tr>
                                        <td valign="top"></td>
                                        <td><asp:Button ID="btnFix" runat="server" CssClass="default" Width="125" Text="Create Solution" OnClick="btnFix_Click" /></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <table width="100%" cellpadding="0" cellspacing="5" border="0">
                        <tr>
                            <td rowspan="2"><img src="/images/hugeAlert.gif" border="0" align="absmiddle" /></td>
                            <td class="header" width="100%" valign="bottom">Incident Routing</td>
                            <td align="right">
                                <asp:Button ID="btnIncident" runat="server" CssClass="default" Width="100" Text="New Incident" />
                            </td>
                        </tr>
                        <tr>
                            <td width="100%" colspan="2" valign="top">Instead of fixing this problem now, you can choose to have ClearView automatically open an incident and try again after it's been resolved.</td>
                        </tr>
                    </table>
                    <table width="100%" cellpadding="4" cellspacing="3" border="0">
                        <tr>
                            <td>
                                <%=strMenuTab2 %>
                                <div id="divMenu2">
                                    <br />
                                    <asp:Repeater ID="rptIncidents" runat="server">
                                        <ItemTemplate>
                                            <div style="display:none">
                                                <table width="100%" cellpadding="2" cellspacing="0" border="0" class="default">
                                                    <tr>
                                                        <td colspan="2">
                                                            <table width="100%" cellpadding="4" cellspacing="3" border="0">
                                                                <tr>
                                                                    <td style="width: 50%"><strong>Route To:</strong> <%# DataBinder.Eval(Container.DataItem, "route")%></td>
                                                                    <td style="width: 50%" align="right"><strong>Priority:</strong> <%# DataBinder.Eval(Container.DataItem, "priority").ToString() == "0" ? "Default" : DataBinder.Eval(Container.DataItem, "priority").ToString() %></td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <table width="100%" cellpadding="4" cellspacing="3" border="0" style="border:solid 1px #CCCCCC">
                                                                <tr bgcolor="#EEEEEE">
                                                                    <td class="bold">MESSAGE</td>
                                                                </tr>
                                                                <tr>
                                                                    <td><%# DataBinder.Eval(Container.DataItem, "message")%></td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td><asp:Button ID="btnIncident" runat="server" CssClass="default" Width="175" Text="Use this Incident" OnClick="btnIncident_Click" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"id") %>' /></td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </div>
                            </td>
                        </tr>
                        <tr id="trIncidents" runat="server" visible="false">
                            <td><img src='/images/alert.gif' border='0' align='absmiddle'> There are no related incidents ... </td>
                        </tr>
                    </table>
                    <p>&nbsp;</p>
                    <table width="100%" cellpadding="0" cellspacing="5" border="0">
                        <tr>
                            <td rowspan="2"><img src="/images/ico_check.gif" border="0" align="absmiddle" /></td>
                            <td class="header" width="100%" valign="bottom">Possible Solutions</td>
                            <td align="right">
                                <asp:Button ID="btnSave" runat="server" CssClass="default" Width="125" Text="Save Incident(s)" OnClick="btnSave_Click" />&nbsp;
                                <asp:Button ID="btnNone" runat="server" CssClass="default" Width="100" Text="No Solution" OnClick="btnNone_Click" />&nbsp;
                                <asp:Button ID="btnSolution" runat="server" CssClass="default" Width="100" Text="New Solution" />
                            </td>
                        </tr>
                        <tr>
                            <td width="100%" colspan="2" valign="top">Select the solution which fixes this error. If the solution is not provided, please click the <b>New Solution</b> button.</td>
                        </tr>
                    </table>
                    <asp:Literal ID="litHidden" runat="server" />
                    <table width="100%" cellpadding="4" cellspacing="3" border="0">
                        <tr>
                            <td>
                                <%=strMenuTab1 %>
                                <div id="divMenu1">
                                    <br />
                                    <asp:Repeater ID="rptRelated" runat="server">
                                        <ItemTemplate>
                                            <div style="display:none">
                                                <table width="100%" cellpadding="2" cellspacing="0" border="0" class="default">
                                                    <tr>
                                                        <td colspan="2">
                                                            <table width="100%" cellpadding="4" cellspacing="3" border="0" style="border:solid 1px #CCCCCC">
                                                                <tr bgcolor="#EEEEEE">
                                                                    <td class="bold">ISSUE - What caused the problem</td>
                                                                </tr>
                                                                <tr>
                                                                    <td><%# oFunction.FormatText(DataBinder.Eval(Container.DataItem, "problem").ToString())%></td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <table width="100%" cellpadding="4" cellspacing="3" border="0" style="border:solid 1px #CCCCCC">
                                                                <tr bgcolor="#EEEEEE">
                                                                    <td class="bold">RESOLUTION - Steps taken to fix the issue</td>
                                                                </tr>
                                                                <tr>
                                                                    <td><%# oFunction.FormatText(DataBinder.Eval(Container.DataItem, "resolution").ToString())%></td>
                                                                </tr>
                                                            </table>
                                                            <asp:Panel ID="panAttach" runat="server" Visible="false">
                                                                <table width="100%" cellpadding="3" cellspacing="2" border="0">
                                                                    <tr>
                                                                        <td><span style="width:100%;border-bottom:1 dotted #CCCCCC;"/></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td><img src="/images/file.gif" border="0" align="absmiddle"/> <asp:Label ID="lblAttach" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem,"path") %>' /></td>
                                                                    </tr>
                                                                </table>
                                                            </asp:Panel>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2" align="right">Created by <b><%# oUser.GetFullName(Int32.Parse(DataBinder.Eval(Container.DataItem, "userid").ToString()))%></b> on <%# DataBinder.Eval(Container.DataItem,"modified") %></td>
                                                    </tr>
                                                    <tr>
                                                        <td><asp:Button ID="btnSelect" runat="server" CssClass="default" Width="175" Text="This Solution Fixed the Error" OnClick="btnSelect_Click" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"id") %>' /></td>
                                                        <td align="right"><b>Category : </b><%# DataBinder.Eval(Container.DataItem, "code")%> / <%# DataBinder.Eval(Container.DataItem, "type")%></td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </div>
                            </td>
                        </tr>
                        <tr id="trNone" runat="server" visible="false">
                            <td><img src='/images/alert.gif' border='0' align='absmiddle'> There are no related errors ... </td>
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
<asp:Label ID="lblAsset" runat="server" Visible="false" />
<asp:Label ID="lblStep" runat="server" Visible="false" />
<asp:Label ID="lblID" runat="server" Visible="false" />
<input type="hidden" id="hdnType2" runat="server" />
</form>
</body>
</html>
