<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="recommission.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.recommission" %>

<html>
<head>
<title>ClearView Administration</title>
<link rel="stylesheet" type="text/css" href="/css/default.css" />
<script type="text/javascript" src="/javascript/global.js"></script>
<script type="text/javascript" src="/javascript/ajax.js"></script>
<script type="text/javascript">
    function UpdateCompleteCheck(oCheck) {
        if (oCheck.checked == true)
            oCheck.checked = confirm('********** WARNING - PLEASE READ **********\n\nThis asset has already been marked COMPLETE meaning it is done with the decommission process.\n\nRecommissioning this device means all current workflows will be cancelled and all previous information will be re-applied to this device.\n\nAre you sure you want to recommission this device?');
    }
</script>
</head>
<body topmargin="0" leftmargin="0">
<form id="Form1" runat="server">
        <table width="100%" height="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
        <td height="100%" valign="top">
<table width="98%" cellpadding="0" cellspacing="0" border="0" align="center">
		<tr><td colspan="2">&nbsp;</td></tr>
		<tr> 
		    <td><b>Recommission Device(s)</b></td>
		    <td align="right"></td>
		</tr>
		<tr><td colspan="2">&nbsp;</td></tr>
		<tr>
		    <td colspan="2" align="center">
                <div ID="divView" runat="server" style="display:inline">
                    <table width="95%" border="0" cellspacing="2" cellpadding="3" align="center">
                        <tr>
                            <td>
                                <table border="0" cellspacing="2" cellpadding="3">
                                    <tr>
                                        <td nowrap>Device Name:</td>
                                        <td width="100%"><asp:TextBox ID="txtName" runat="server" CssClass="default" Width="200" /></td>
                                    </tr>
                                    <tr>
                                        <td nowrap>Order By:</td>
                                        <td width="100%">
                                            <asp:DropDownList ID="ddlOrder" runat="server" CssClass="default" Width="200">
                                                <asp:ListItem Text="Device Name" Value="name" />
                                                <asp:ListItem Text="Serial Number" Value="serial" />
                                                <asp:ListItem Text="Submitted Date" Value="created" />
                                                <asp:ListItem Text="Decommission Date" Value="decom" />
                                                <asp:ListItem Text="Destroy / Redeploy Date" Value="destroy" />
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>
                                            <asp:Button ID="btnSearch" runat="server" Text="Submit" CssClass="default" Width="75" OnClick="btnSearch_Click" /> 
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td align="right">
                                <table border="0" cellspacing="2" cellpadding="3">
                                    <tr>
                                        <td nowrap>Search:</td>
                                        <td width="100%"><asp:TextBox ID="txtFind" runat="server" CssClass="default" Width="200" /></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>
                                            <asp:Button ID="btnFind" runat="server" Text="Find on this Page" CssClass="default" Width="125" Enabled="false" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2"><asp:Label ID="lblResult" runat="server" CssClass="header" /></td>
                        </tr>
                    </table>
                    <br />
                    <asp:Panel ID="panResult" runat="server" Visible="false">
                        <table width="95%" border="0" cellspacing="2" cellpadding="3" align="center">
                            <tr>
                                <td colspan="2">
                                    <table width="100%" cellpadding="3" cellspacing="0" border="0" align="center" style="border:solid 1px #CCCCCC">
                                        <tr bgcolor="#EEEEEE">
                                            <td></td>
                                            <td><b>Name</b></td>
                                            <td><b>Serial</b></td>
                                            <td><b>DR</b></td>
                                            <td><b>Model</b></td>
                                            <td><b>Requestor</b></td>
                                            <td><b>Reason</b></td>
                                            <td><b>Submitted</b></td>
                                        </tr>
                                        <asp:repeater ID="rptDevices" runat="server">
                                            <ItemTemplate>
                                                <tr>
                                                    <td valign="top"><asp:CheckBox ID="chkDevice" runat="server" CssClass="default" ToolTip='<%# DataBinder.Eval(Container.DataItem, "serverid").ToString() != "0" ? "S" + DataBinder.Eval(Container.DataItem, "serverid").ToString() : "R" + DataBinder.Eval(Container.DataItem, "resourceid").ToString() %>' /></td>
                                                    <td valign="top" nowrap><asp:Label ID="lblName" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "name") %>' /></td>
                                                    <td valign="top"><asp:Label ID="lblSerial" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "serial") %>' /></td>
                                                    <td valign="top"><%# DataBinder.Eval(Container.DataItem, "serialdr") %></td>
                                                    <td valign="top"><%# DataBinder.Eval(Container.DataItem, "model") %></td>
                                                    <td valign="top"><%# DataBinder.Eval(Container.DataItem, "username") %></td>
                                                    <td valign="top" width="30%"><%# DataBinder.Eval(Container.DataItem, "reason") %></td>
                                                    <td valign="top"><%# DataBinder.Eval(Container.DataItem, "created") %></td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2" valign="top">*** <%# DataBinder.Eval(Container.DataItem, "resourceid").ToString() == "0" ? "Automated" : "Manual" %></td>
                                                    <td colspan="5">
                                                        <table cellpadding="4" cellspacing="0" border="0">
                                                            <tr>
                                                                <td>Decommission Scheduled:</td>
                                                                <td><%# DataBinder.Eval(Container.DataItem, "decom") %></td>
                                                                <td><img src="/images/spacer.gif" border="0" width="50" height="1" /></td>
                                                                <td>Destroy / Redeploy Scheduled:</td>
                                                                <td><%# DataBinder.Eval(Container.DataItem, "destroy") %></td>
                                                            </tr>
                                                            <tr>
                                                                <td>Decommission Completed:</td>
                                                                <td><%# DataBinder.Eval(Container.DataItem, "turnedoff") %></td>
                                                                <td><img src="/images/spacer.gif" border="0" width="50" height="1" /></td>
                                                                <td>Destroy / Redeploy Completed:</td>
                                                                <td><asp:Label ID="lblDestroyed" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "destroyed") %>' /></td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="5"><asp:Label ID="lblRecommissioned" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "recommissioned") %>' ToolTip='<%# DataBinder.Eval(Container.DataItem, "recommissioned_by").ToString() + "_" + DataBinder.Eval(Container.DataItem, "recommissioned_reason").ToString() %>' /></td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td valign="top"><asp:Label ID="lblStatus" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "running") %>' /></td>
                                                </tr>
                                            </ItemTemplate>
                                            <AlternatingItemTemplate>
                                                <tr bgcolor="#F6F6F6">
                                                    <td valign="top"><asp:CheckBox ID="chkDevice" runat="server" CssClass="default" ToolTip='<%# DataBinder.Eval(Container.DataItem, "serverid").ToString() != "0" ? "S" + DataBinder.Eval(Container.DataItem, "serverid").ToString() : "R" + DataBinder.Eval(Container.DataItem, "resourceid").ToString() %>' /></td>
                                                    <td valign="top" nowrap><asp:Label ID="lblName" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "name") %>' /></td>
                                                    <td valign="top"><asp:Label ID="lblSerial" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "serial") %>' /></td>
                                                    <td valign="top"><%# DataBinder.Eval(Container.DataItem, "serialdr") %></td>
                                                    <td valign="top"><%# DataBinder.Eval(Container.DataItem, "model") %></td>
                                                    <td valign="top"><%# DataBinder.Eval(Container.DataItem, "username") %></td>
                                                    <td valign="top" width="30%"><%# DataBinder.Eval(Container.DataItem, "reason") %></td>
                                                    <td valign="top"><%# DataBinder.Eval(Container.DataItem, "created") %></td>
                                                </tr>
                                                <tr bgcolor="#F6F6F6">
                                                    <td colspan="2" valign="top">*** <%# DataBinder.Eval(Container.DataItem, "resourceid").ToString() == "0" ? "Automated" : "Manual" %></td>
                                                    <td colspan="5">
                                                        <table cellpadding="4" cellspacing="0" border="0">
                                                            <tr>
                                                                <td>Decommission Scheduled:</td>
                                                                <td><%# DataBinder.Eval(Container.DataItem, "decom") %></td>
                                                                <td><img src="/images/spacer.gif" border="0" width="50" height="1" /></td>
                                                                <td>Destroy / Redeploy Scheduled:</td>
                                                                <td><%# DataBinder.Eval(Container.DataItem, "destroy") %></td>
                                                            </tr>
                                                            <tr>
                                                                <td>Decommission Completed:</td>
                                                                <td><%# DataBinder.Eval(Container.DataItem, "turnedoff") %></td>
                                                                <td><img src="/images/spacer.gif" border="0" width="50" height="1" /></td>
                                                                <td>Destroy / Redeploy Completed:</td>
                                                                <td><asp:Label ID="lblDestroyed" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "destroyed") %>' /></td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="5"><asp:Label ID="lblRecommissioned" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "recommissioned") %>' ToolTip='<%# DataBinder.Eval(Container.DataItem, "recommissioned_by").ToString() + "_" + DataBinder.Eval(Container.DataItem, "recommissioned_reason").ToString() %>' /></td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td valign="top"><asp:Label ID="lblStatus" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "running") %>' /></td>
                                                </tr>
                                            </AlternatingItemTemplate>
                                        </asp:repeater>
                                    </table>
                                </td>
                            </tr>
                        </table>
                        <br />
                        <table width="95%" border="0" cellspacing="2" cellpadding="3" align="center">
                            <tr>
                                <td colspan="2" class="bold">Please complete the following information...</td>
                            </tr>
                            <tr>
                                <td nowrap>Client:</td>
                                <td width="100%">
                                    <table cellpadding="0" cellspacing="0" border="0">
                                        <tr>
                                            <td><asp:TextBox ID="txtUser" runat="server" Width="300" CssClass="default" /></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div id="divAJAX" runat="server" style="overflow:hidden; position:absolute; display:none; background-color:#FFFFFF; border:solid 1px #CCCCCC">
                                                    <asp:ListBox ID="lstAJAX" runat="server" CssClass="default" />
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                    <input type="hidden" id="hdnAJAXValue" name="hdnAJAXValue" />
                                </td>
                            </tr>
                            <tr>
                                <td nowrap>Reason:</td>
                                <td width="100%"><asp:TextBox ID="txtReason" runat="server" CssClass="default" Width="500" TextMode="MultiLine" Rows="5" /></td>
                            </tr>
                            <tr>
                                <td nowrap></td>
                                <td width="100%"><asp:CheckBox ID="chkAvamar" runat="server" CssClass="default" Text="Restore Avamar Group(s)" Checked="true" /></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="default" Width="75" OnClick="btnSubmit_Click" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </div>
            </td>
        </tr>
    </table>
        </td>
        </tr>
        </table>
    <input type="hidden" id="hdnId" runat="server" />
<input type="hidden" id="hdnUsers" runat="server" />
</form>
</body>
</html>
