<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frame_model_environments.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.frame_model_environments" %>

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
		            <td><b>Model Reservations</b></td>
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
                    <td nowrap>Model:</td>
                    <td width="100%"><asp:label ID="lblName" CssClass="bold" runat="server" /></td>
                </tr>
                <tr height="1"> 
                    <td nowrap>Class:</td>
                    <td width="100%"><asp:label ID="lblClass" CssClass="bold" runat="server" /></td>
                </tr>
                <tr height="1"> 
                    <td nowrap>Environment:</td>
                    <td width="100%"><asp:label ID="lblEnvironment" CssClass="bold" runat="server" /></td>
                </tr>
                <tr height="1"> 
                    <td colspan="2"><hr size="1" noshade /></td>
                </tr>
                <tr height="1">
                    <td nowrap>Class:</td>
                    <td width="100%"><asp:DropDownList ID="ddlClass" CssClass="default" runat="server" Width="300" /></td>
                </tr>
                <tr height="1">
                    <td nowrap>Environment:</td>
                    <td width="100%">
                        <asp:DropDownList ID="ddlEnvironment" CssClass="default" runat="server" Width="300" Enabled="false" >
                            <asp:ListItem Value="-- Please select a Class --" />
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr height="1"> 
                    <td></td>
                    <td width="100%"><asp:CheckBox ID="chkMovable" runat="server" CssClass="default" Text="Use TEST flag to specify movable" /></td>
                </tr>
                <tr height="1">
                    <td>&nbsp;</td>
                    <td><asp:Button ID="btnAdd" runat="server" CssClass="default" Width="75" Text="Add" OnClick="btnAdd_Click" /></td>
                </tr>
                <tr>
                    <td colspan="2" valign="top" class="default">
                        <asp:ListBox ID="lstCurrent" runat="server" Width="320" Rows="8" CssClass="default" />
                        <div align="center" style="width:320px">
                            <asp:ImageButton ID="btnUp" runat="server" ImageUrl="/images/up.gif" ToolTip="Move Up" OnClick="btnUp_Click"/>
                            &nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:ImageButton ID="btnRemove" runat="server" ImageUrl="/images/dl.gif" ToolTip="Remove" OnClick="btnRemove_Click" />
                            &nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:ImageButton ID="btnDown" runat="server" ImageUrl="/images/dn.gif" ToolTip="Move Down" OnClick="btnDown_Click"/>
                        </div>
                    </td>
                </tr>
	            <tr height="1">
                    <td colspan="2"><b>NOTE:</b> The build locations is ALWAYS the first step.  The final class / environment is ALWAYS the last step. This page is for configuring in-between assets.</td>
                </tr>
	            <tr height="1">
                    <td colspan="2"><b>EXAMPLE:</b> Production | Ecom Inner</td>
                </tr>
	            <tr height="1">
                    <td colspan="2">&nbsp;&nbsp;&nbsp;&nbsp;Build Location = Dev | Core (always first step - do not add)</td>
                </tr>
	            <tr height="1">
                    <td colspan="2">&nbsp;&nbsp;&nbsp;&nbsp;In-Between Location = Test | Ecom Inner (needs to be added!!)</td>
                </tr>
	            <tr height="1">
                    <td colspan="2">&nbsp;&nbsp;&nbsp;&nbsp;Final Location = Prod | Ecom Inner (always last step - do not add)</td>
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
<input type="hidden" id="hdnEnvironment" runat="server" />
</form>
</body>
</html>


