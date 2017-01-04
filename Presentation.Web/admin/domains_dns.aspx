<%@ Page Language="C#" AutoEventWireup="True" CodeBehind="domains_dns.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.domains_dns" %>

<script type="text/javascript">
</script>
<html>
<head>
<title>ClearView Administration</title>
<link rel="stylesheet" type="text/css" href="/css/default.css" />
<script type="text/javascript" src="/javascript/both.js"></script>
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
		    <td><b>Location - Addresses</b></td>
		    <td align="right"><a href="javascript:void(0);" onclick="Add();" class="cmlink" title="Click to Add New">Add New</a></td>
		</tr>
		<tr><td colspan="2">&nbsp;</td></tr>
		<tr>
		    <td colspan="2" align="center">
                <asp:Panel ID="panView" runat="server" Visible="false">
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

                <asp:Panel id="panAdd" runat="server" Visible="false">
                    <table width="95%" cellpadding="3" cellspacing="0" border="0" align="center">
                        <tr> 
                            <td class="default" width="100px">Domain:</td>
                            <td><asp:Label ID="lblDomain" runat="server" CssClass="default" /></td>
                        </tr>
                        <tr> 
                            <td class="default" width="100px">Class:</td>
                            <td><asp:Label ID="lblClass" runat="server" CssClass="default" /></td>
                        </tr>
                        <tr>
                            <td class="default">Location:</td>
                            <td><%=strLocation %></td>
                        </tr>
                        <tr> 
                            <td class="default">DNS Address 1:</td>
                            <td><asp:TextBox ID="txtDNS1" runat="server" Width="150" MaxLength="15" CssClass="default" /></td>
                        </tr>
                        <tr> 
                            <td class="default">DNS Address 2:</td>
                            <td><asp:TextBox ID="txtDNS2" runat="server" Width="150" MaxLength="15" CssClass="default" /></td>
                        </tr>
                        <tr> 
                            <td class="default">DNS Address 3:</td>
                            <td><asp:TextBox ID="txtDNS3" runat="server" Width="150" MaxLength="15" CssClass="default" /></td>
                        </tr>
                        <tr> 
                            <td class="default">DNS Address 4:</td>
                            <td><asp:TextBox ID="txtDNS4" runat="server" Width="150" MaxLength="15" CssClass="default" /></td>
                        </tr>
                        <tr> 
                            <td class="default">WINS Address 1:</td>
                            <td><asp:TextBox ID="txtWINS1" runat="server" Width="150" MaxLength="15" CssClass="default" /></td>
                        </tr>
                        <tr> 
                            <td class="default">WINS Address 2:</td>
                            <td><asp:TextBox ID="txtWINS2" runat="server" Width="150" MaxLength="15" CssClass="default" /></td>
                        </tr>
                        <tr> 
                            <td class="default">WINS Address 3:</td>
                            <td><asp:TextBox ID="txtWINS3" runat="server" Width="150" MaxLength="15" CssClass="default" /></td>
                        </tr>
                        <tr> 
                            <td class="default">WINS Address 4:</td>
                            <td><asp:TextBox ID="txtWINS4" runat="server" Width="150" MaxLength="15" CssClass="default" /></td>
                        </tr>
                        <tr><td height="5" colspan="2">&nbsp;</td></tr>
                        <tr> 
                            <td>&nbsp;</td>
                            <td>
                                <asp:button ID="btnAdd" CssClass="default" runat="server" Text="Add" Width="75" OnClick="btnAdd_Click" />
                                <asp:button ID="btnDelete" CssClass="default" runat="server" Text="Delete" Width="75" OnClick="btnDelete_Click" />
                                <asp:button ID="btnCancel" CssClass="default" runat="server" Text="Cancel" Width="75" OnClick="btnCancel_Click" />
                            </td>
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
    <input type="hidden" id="hdnLocation" runat="server" />
</form>
</body>
</html>
