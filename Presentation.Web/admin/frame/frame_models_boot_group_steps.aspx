<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frame_models_boot_group_steps.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.frame_models_boot_group_steps" %>


<script type="text/javascript">
</script>
<html>
<head>
<title>Web Content Management Administration</title>
<link rel="stylesheet" type="text/css" href="/css/default.css" />
<script type="text/javascript" src="/javascript/global.js"></script>
</head>
<body topmargin="0" leftmargin="0">
<form id="Form1" runat="server">
<table width="100%" height="100%" border="0" cellSpacing="0" cellPadding="0">
    <tr height="1">
        <td colspan="2">
            <table width="100%" border="0" cellSpacing="2" cellPadding="2" bgcolor="#e6e9f0">
	            <tr bgcolor="#e6e9f0">
		            <td><b>Model Boot Group Steps</b></td>
	                <td align="right">
			            <a href="javascript:void(0);" onclick="parent.HidePanel();"><img src="/admin/images/close.gif" border="0" title="Close"></a>
	                </td>
	            </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <div style="height:100%; width:100%; overflow:auto;">
            <table width="100%" height="100%" border="0" cellSpacing="2" cellPadding="2" bgcolor="#c8cfdd">
                <tr height="1"> 
                    <td nowrap>Boot Group:</td>
                    <td width="100%"><asp:label ID="lblName" CssClass="bold" runat="server" /></td>
                </tr>
                <tr height="1"> 
                    <td nowrap>Statement:</td>
                    <td width="100%"><asp:TextBox ID="txtThenWrite" CssClass="default" runat="server" Width="600" MaxLength="100" /></td>
                </tr>
                <tr height="1"> 
                    <td nowrap>Timeout:</td>
                    <td width="100%"><asp:TextBox ID="txtTimeout" CssClass="default" runat="server" Width="100" MaxLength="10" />
                    &nbsp;&nbsp;&nbsp;&nbsp;The amount of <b>minutes</b> to wait before stopping and sending support request.</td>
                </tr>
                <tr height="1"> 
                    <td nowrap>Condition:</td>
                    <td width="100%"><asp:TextBox ID="txtWaitFor" CssClass="default" runat="server" Width="100" MaxLength="100" />
                    &nbsp;&nbsp;&nbsp;&nbsp;<b>ONLY</b> write the statement if this conditional text appears. Example: [y/n]</td>
                </tr>
                <tr height="1"> 
                    <td nowrap></td>
                    <td width="100%"><asp:CheckBox ID="chkPower" CssClass="default" runat="server" Text="Power On Step" />
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <b>NOTE:</b> Make sure ALL condition statements are in the Regular Expression of the Boot Group!!</td>
                </tr>
                <tr height="1">
                    <td>&nbsp;</td>
                    <td><asp:Button ID="btnAdd" runat="server" CssClass="default" Width="75" Text="Add" OnClick="btnAdd_Click" /></td>
                </tr>
                <tr>
                    <td colspan="2" valign="top">
                        <table width="100%" cellpadding="4" cellspacing="0" border="0" style="border:solid 1px #CCCCCC; background-color:#FFFFFF">
                            <tr bgcolor="#EEEEEE">
                                <td nowrap><b>Write</b></td>
                                <td nowrap><b>Timeout</b></td>
                                <td nowrap><b>Condition</b></td>
                                <td nowrap><b>Power?</b></td>
                                <td nowrap><b></b></td>
                            </tr>
                            <asp:repeater ID="rptItems" runat="server">
                                <ItemTemplate>
                                    <tr>
                                        <td><%# DataBinder.Eval(Container.DataItem, "then_write") %></td>
                                        <td><%# DataBinder.Eval(Container.DataItem, "timeout") %></td>
                                        <td><%# DataBinder.Eval(Container.DataItem, "wait_for") %></td>
                                        <td><%# (DataBinder.Eval(Container.DataItem, "power").ToString() == "1" ? "<b>Yes</b>" : "No")%></td>
                                        <td align="right">
                                            [<asp:ImageButton ID="btnUp" runat="server" ImageUrl="/admin/images/up.gif" ToolTip="Move Up" OnClick="btnUp_Click" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' />]&nbsp;
                                            [<asp:ImageButton ID="btnDown" runat="server" ImageUrl="/admin/images/dn.gif" ToolTip="Move Down" OnClick="btnDown_Click" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' />]&nbsp;
                                            [<asp:ImageButton ID="btnDelete" runat="server" ImageUrl="/admin/images/dl.gif" ToolTip="Delete" OnClick="btnDelete_Click" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' />]
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:repeater>
                            <tr>
                                <td colspan="5">
                                    <asp:Label ID="lblNone" runat="server" CssClass="default" Visible="false" Text="<img src='/images/alert.gif' border='0' align='absmiddle'> There are no items..." />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
	            <tr height="1">
	                <td colspan="2"><hr size="1" noshade /></td>
	            </tr>
            </table>
            </div>
        </td>
    </tr>
    <tr height="1">
        <td bgcolor="#c8cfdd" align="center">
        </td>
        <td align="right" bgcolor="#c8cfdd">
            <asp:Button ID="btnClose" runat="server" Text="Close" Width="75" CssClass="default" />
        </td>
    </tr>
    <tr><td colspan="2" height="5" bgcolor="#c8cfdd"><img src="images/spacer.gif" width="1" height="5" /></td></tr>
</table>
<asp:Label ID="lblId" runat="server" Visible="false" />
    <asp:HiddenField ID="hdnUser" runat="server" />
</form>
</body>
</html>
