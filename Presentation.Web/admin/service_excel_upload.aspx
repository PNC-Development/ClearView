<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="service_excel_upload.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.service_excel_upload" %>



<script type="text/javascript">  
</script>
<html>
<head>
<title>LOAD</title>
<link rel="stylesheet" type="text/css" href="/css/default.css" />
</head>
<body leftmargin="0" topmargin="0">
<form id="Form1" runat="server">
    <table width="100%" cellpadding="0" cellspacing="0" border="0">
        <tr>             
            <td><img src="/images/spacer.gif" width="1" /></td>
            <td colspan="2" class="header">Excel Upload Utility</td>
        </tr>
        <tr>
           <td><img src="/images/spacer.gif" width="1" /></td>
           <td colspan="2"><asp:FileUpload ID="fileUpload" runat="server" CssClass="default" Width="500" /></td>
        </tr>
        <tr>
            <td colspan="2">&nbsp;</td>
        </tr>
        <tr>
            <td>&nbsp;</td>
            <td><asp:Button ID="btnLoad1" runat="server" CssClass="default" Width="75" Text="Load" OnClick="btnLoad1_Click" /></td>
        </tr>
        <tr><td>&nbsp;</td></tr>
        <tr>
           <td colspan="2" align="left"><asp:Label ID="lblError" runat="server" CssClass="note" /></td> 
        </tr>
    </table>  
</form>
</body> 
</html>
 