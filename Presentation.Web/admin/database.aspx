<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="database.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.database" %>

<script type="text/javascript">
</script>
<style type="text/css">
    .table-script *
    {
        border: none;
        border-collapse: collapse;
        padding: 0;
        margin: 0;
        white-space: nowrap;
    }
    .table-script th
    {
        font-weight: bold;
    }
    .table-script td, .table-script th
    {
        padding: 3;
        border: solid 1px #CCC;
    }
</style>
<html>
<head>
<title>Web Content Management Administration</title>
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
		    <td><b>Database Structure</b></td>
		    <td align="right"></td>
		</tr>
		<tr><td colspan="2">&nbsp;</td></tr>
		<tr>
		    <td colspan="2">
		        <table cellpadding="3" cellspacing="2" border="0">
		            <tr>
		                <td>Version:</td>
		                <td><asp:Label ID="lblVersion" runat="server" CssClass="default" /></td>
		            </tr>
		            <tr>
		                <td>Date:</td>
		                <td><asp:Label ID="lblDate" runat="server" CssClass="default" /></td>
		            </tr>
		            <tr>
		                <td>FROM:</td>
		                <td>
		                    <asp:DropDownList ID="ddlFrom" runat="server" CssClass="default">
		                        <asp:ListItem Value="devDSN" />
		                        <asp:ListItem Value="devDSNAsset" />
		                        <asp:ListItem Value="devDSNip" />
		                        <asp:ListItem Value="devDSNService" />
		                        <asp:ListItem Value="devDSNServiceEditor" />
		                        <asp:ListItem Value="dev2DSN" />
		                        <asp:ListItem Value="dev2DSNAsset" />
		                        <asp:ListItem Value="dev2DSNip" />
		                        <asp:ListItem Value="dev2DSNService" />
		                        <asp:ListItem Value="dev2DSNServiceEditor" />
		                        <asp:ListItem Value="testDSN" />
		                        <asp:ListItem Value="testDSNAsset" />
		                        <asp:ListItem Value="testDSNip" />
		                        <asp:ListItem Value="testDSNService" />
		                        <asp:ListItem Value="testDSNServiceEditor" />
		                        <asp:ListItem Value="prodDSN" />
		                        <asp:ListItem Value="prodDSNAsset" />
		                        <asp:ListItem Value="prodDSNip" />
		                        <asp:ListItem Value="prodDSNService" />
		                        <asp:ListItem Value="prodDSNServiceEditor" />
		                    </asp:DropDownList>
		                </td>
		            </tr>
		            <tr>
		                <td>TO:</td>
		                <td>
		                    <asp:DropDownList ID="ddlTo" runat="server" CssClass="default">
		                        <asp:ListItem Value="devDSN" />
		                        <asp:ListItem Value="devDSNAsset" />
		                        <asp:ListItem Value="devDSNip" />
		                        <asp:ListItem Value="devDSNService" />
		                        <asp:ListItem Value="devDSNServiceEditor" />
		                        <asp:ListItem Value="dev2DSN" />
		                        <asp:ListItem Value="dev2DSNAsset" />
		                        <asp:ListItem Value="dev2DSNip" />
		                        <asp:ListItem Value="dev2DSNService" />
		                        <asp:ListItem Value="dev2DSNServiceEditor" />
		                        <asp:ListItem Value="testDSN" />
		                        <asp:ListItem Value="testDSNAsset" />
		                        <asp:ListItem Value="testDSNip" />
		                        <asp:ListItem Value="testDSNService" />
		                        <asp:ListItem Value="testDSNServiceEditor" />
		                        <asp:ListItem Value="prodDSN" />
		                        <asp:ListItem Value="prodDSNAsset" />
		                        <asp:ListItem Value="prodDSNip" />
		                        <asp:ListItem Value="prodDSNService" />
		                        <asp:ListItem Value="prodDSNServiceEditor" />
		                    </asp:DropDownList>
		                </td>
		            </tr>
		            <tr>
		                <td></td>
		                <td><asp:Button ID="btnGo" runat="server" CssClass="default" Width="75" OnClick="btnGo_Click" Text="Run" /> <asp:Button ID="btnPrint" runat="server" CssClass="default" Width="75" Text="Print" /></td>
		            </tr>
		            <tr>
		                <td></td>
		                <td><asp:CheckBox ID="chkShow" runat="server" CssClass="default" Text="Show All DEV (Do Not Compare)" /></td>
		            </tr>
		            <tr>
		                <td colspan="2"><%=strResults %></td>
		            </tr>
		            <tr>
		                <td colspan="2">&nbsp;</td>
		            </tr>
		            <tr>
		                <td>Days:</td>
		                <td><asp:TextBox ID="txtDays" runat="server" CssClass="default" Width="100" MaxLength="10" /></td>
		            </tr>
		            <tr>
		                <td>DSN:</td>
		                <td>
		                    <asp:DropDownList ID="ddlDays" runat="server" CssClass="default">
		                        <asp:ListItem Value="devDSN" />
		                        <asp:ListItem Value="devDSNAsset" />
		                        <asp:ListItem Value="devDSNip" />
		                        <asp:ListItem Value="devDSNService" />
		                        <asp:ListItem Value="devDSNServiceEditor" />
		                        <asp:ListItem Value="testDSN" />
		                        <asp:ListItem Value="testDSNAsset" />
		                        <asp:ListItem Value="testDSNip" />
		                        <asp:ListItem Value="testDSNService" />
		                        <asp:ListItem Value="testDSNServiceEditor" />
		                        <asp:ListItem Value="prodDSN" />
		                        <asp:ListItem Value="prodDSNAsset" />
		                        <asp:ListItem Value="prodDSNip" />
		                        <asp:ListItem Value="prodDSNService" />
		                        <asp:ListItem Value="prodDSNServiceEditor" />
		                    </asp:DropDownList>
		                </td>
		            </tr>
		            <tr>
		                <td>Order By:</td>
		                <td>
		                    <asp:DropDownList ID="ddlOrder" runat="server" CssClass="default">
		                        <asp:ListItem Value="name" />
		                        <asp:ListItem Value="modify_date" />
		                    </asp:DropDownList>
		                </td>
		            </tr>
		            <tr>
		                <td></td>
		                <td><asp:Button ID="btnDays" runat="server" CssClass="default" Width="75" OnClick="btnDays_Click" Text="Run" /></td>
		            </tr>
		            <tr>
		                <td colspan="2"><%=strDays %></td>
		            </tr>
		            <tr>
		                <td colspan="2">&nbsp;</td>
		            </tr>
		            <tr>
		                <td>Column Name:</td>
		                <td><asp:TextBox ID="txtSearchColumn" runat="server" CssClass="default" Width="300" MaxLength="100" /></td>
		            </tr>
		            <tr>
		                <td>Value:</td>
		                <td><asp:TextBox ID="txtSearchValue" runat="server" CssClass="default" Width="300" MaxLength="100" /></td>
		            </tr>
		            <tr>
		                <td>DSN:</td>
		                <td>
		                    <asp:DropDownList ID="ddlSearch" runat="server" CssClass="default">
		                        <asp:ListItem Value="devDSN" />
		                        <asp:ListItem Value="devDSNAsset" />
		                        <asp:ListItem Value="devDSNip" />
		                        <asp:ListItem Value="devDSNService" />
		                        <asp:ListItem Value="devDSNServiceEditor" />
		                        <asp:ListItem Value="testDSN" />
		                        <asp:ListItem Value="testDSNAsset" />
		                        <asp:ListItem Value="testDSNip" />
		                        <asp:ListItem Value="testDSNService" />
		                        <asp:ListItem Value="testDSNServiceEditor" />
		                        <asp:ListItem Value="prodDSN" />
		                        <asp:ListItem Value="prodDSNAsset" />
		                        <asp:ListItem Value="prodDSNip" />
		                        <asp:ListItem Value="prodDSNService" />
		                        <asp:ListItem Value="prodDSNServiceEditor" />
		                    </asp:DropDownList>
		                </td>
		            </tr>
		            <tr>
		                <td></td>
		                <td><asp:Button ID="btnSearch" runat="server" CssClass="default" Width="75" OnClick="btnSearch_Click" Text="Run" /></td>
		            </tr>
		            <tr>
		                <td colspan="2"><%=strSearch %></td>
		            </tr>
		            <tr>
		                <td colspan="2"><a name="dbScript">&nbsp;</a></td>
		            </tr>
		            <tr>
		                <td>DSN:</td>
		                <td>
		                    <asp:DropDownList ID="ddlScript" runat="server" CssClass="default">
		                        <asp:ListItem Value="DSN" />
		                        <asp:ListItem Value="AssetDSN" />
		                        <asp:ListItem Value="IpDSN" />
		                        <asp:ListItem Value="ServiceDSN" />
		                        <asp:ListItem Value="ServiceEditorDSN" />
		                        <asp:ListItem Value="ZeusDSN" />
		                        <asp:ListItem Value="ClearViewDWDSN" />
		                    </asp:DropDownList>
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:CheckBox ID="chkScript" runat="server" Text="Save to File" />
		                </td>
		            </tr>
		            <tr>
		                <td>Script:</td>
		                <td><asp:TextBox ID="txtScript" runat="server" Rows="8" Width="600" TextMode="MultiLine" /></td>
		            </tr>
		            <tr>
		                <td>Results:</td>
		                <td>
                            <table class="table-script">
                                <%=strScript %>
                            </table>
                        </td>
		            </tr>
		            <tr>
		                <td></td>
		                <td><asp:Button ID="btnScript" runat="server" CssClass="default" Width="75" OnClick="btnScript_Click" Text="Execute" /></td>
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
