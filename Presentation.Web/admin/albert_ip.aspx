<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="albert_ip.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.albert_ip" %>

<script type="text/javascript">
    function SelectNetwork(strID, strName, oID, oName) {
        oID = document.getElementById(oID);
        oID.value = strID;
        oName = document.getElementById(oName);
        oName.value = strName;
    }
</script>
<html>
<head>
<title>LOAD</title>
<link rel="stylesheet" type="text/css" href="/css/default.css" />
<script src="/javascript/default.js"type="text/javascript"></script>
</head>
<body leftmargin="0" topmargin="0">
<form id="Form1" runat="server">
    <table width="100%" cellpadding="4" cellspacing="3" border="0">
        <asp:Panel ID="panDone" runat="server" Visible="false">
        <tr>
            <td colspan="2">
                <table width="100%" cellpadding="0" cellspacing="5" border="0" style="border:solid 1px #999999" bgcolor="#f9f9f9">
                    <tr>
                        <td rowspan="2"><img src="/images/bigCheck.gif" border="0" align="absmiddle" /></td>
                        <td class="header" width="100%" valign="bottom">Import Finished!</td>
                    </tr>
                    <tr>
                        <td width="100%" valign="top">There were <asp:Label ID="lblImport" runat="server" CssClass="default" /> IP addresses imported successfully.</td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="2">&nbsp;</td>
        </tr>
        </asp:Panel>
        <tr>
            <td nowrap>Network:</td>
            <td width="100%"><asp:TextBox ID="txtVLAN" runat="server" CssClass="default" ReadOnly="true" Width="300" Text="Please select from below..." /></td>
        </tr>
        <tr>
            <td nowrap>File:</td>
            <td width="100%"><asp:FileUpload runat="server" ID="oFile" Width="500" CssClass="default" /></td>
        </tr>
        <tr>
            <td>&nbsp;</td>
            <td><asp:Button ID="btnImport" runat="server" CssClass="default" Width="100" Text="Import" OnClick="btnImport_Click" /></td>
        </tr>
        <tr>
            <td colspan="2">&nbsp;</td>
        </tr>
        <tr>
            <td colspan="2" class="bold">Available Networks</td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:TreeView ID="oTreeview" runat="server" ShowLines="true" NodeIndent="35">
                    <NodeStyle CssClass="default" />
                </asp:TreeView>
            </td>
        </tr>
    </table>
    <asp:HiddenField ID="hdnVLAN" runat="server" />
</form>
</body>
</html>