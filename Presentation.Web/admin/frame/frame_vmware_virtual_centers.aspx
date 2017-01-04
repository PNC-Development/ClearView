<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frame_vmware_virtual_centers.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.frame_vmware_virtual_centers" %>

<script type="text/javascript">
</script>
<html>
<head>
<title>Web Content Management Administration</title>
<link rel="stylesheet" type="text/css" href="/css/default.css" />
<script type="text/javascript" src="/javascript/global.js"></script>
<script type="text/javascript" src="/javascript/both.js"></script>
</head>
<body topmargin="0" leftmargin="0">
<form id="Form1" runat="server">
<table width="100%" height="100%" border="0" cellSpacing="0" cellPadding="0">
    <tr height="1">
        <td colspan="2">
            <table width="100%" border="0" cellSpacing="2" cellPadding="2" bgcolor="#e6e9f0">
	            <tr bgcolor="#e6e9f0">
		            <td><b><asp:Label ID="lblName" runat="server" CssClass="default" /> Configuration</b></td>
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
                    <td nowrap>Location:<font class="required">&nbsp;*</font></td>
                    <td width="100%"><%=strLocation %></td>
                </tr>
                <tr height="1"> 
                    <td nowrap>Class:<font class="required">&nbsp;*</font></td>
                    <td width="100%"><asp:DropDownList ID="ddlClass" CssClass="default" runat="server" Width="300" /></td>
                </tr>
                <tr height="1"> 
                    <td nowrap>Environment:<font class="required">&nbsp;*</font></td>
                    <td width="100%">
                        <asp:DropDownList ID="ddlEnvironment" CssClass="default" runat="server" Width="300" Enabled="false" >
                            <asp:ListItem Value="-- Please select a Class --" />
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr height="1">
                    <td></td>
                    <td width="100%"><asp:Button ID="btnAdd" runat="server" CssClass="default" Width="100" Text="Add" OnClick="btnAdd_Click" /></td>
                </tr>
                <tr>
                    <td colspan="2" valign="top">
                        <table width="100%" cellpadding="4" cellspacing="0" border="0" style="border:solid 1px #CCCCCC" bgcolor="#FFFFFF">
                            <tr bgcolor="#EEEEEE">
                                <td nowrap><b>Location</b></td>
                                <td nowrap><b>Class</b></td>
                                <td nowrap><b>Environment</b></td>
                                <td nowrap></td>
                            </tr>
                            <asp:repeater ID="rptLocations" runat="server">
                                <ItemTemplate>
                                    <tr>
                                        <td><%# DataBinder.Eval(Container.DataItem, "location") %></td>
                                        <td><%# DataBinder.Eval(Container.DataItem, "class") %></td>
                                        <td><%# DataBinder.Eval(Container.DataItem, "environment") %></td>
                                        <td align="right">[<asp:LinkButton ID="btnDelete" runat="server" CssClass="default" Text="Delete" OnClick="btnDelete_Click" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' />]</td>
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
            </div>
        </td>
    </tr>
    <tr height="1">
        <td colspan="2" bgcolor="#c8cfdd"><hr size="1" noshade /></td>
    </tr>
    <tr height="1">
        <td bgcolor="#c8cfdd"></td>
        <td align="right" bgcolor="#c8cfdd">
            <asp:Button ID="btnClose" runat="server" Text="Close" Width="75" CssClass="default" />
        </td>
    </tr>
    <tr><td colspan="2" height="5" bgcolor="#c8cfdd"><img src="images/spacer.gif" width="1" height="5" /></td></tr>
</table>
<input type="hidden" runat="server" id="hdnId" />
<input type="hidden" id="hdnLocation" runat="server" />
<input type="hidden" id="hdnEnvironment" runat="server" />
</form>
</body>
</html>
