<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="test.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.test" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
<link rel="stylesheet" type="text/css" href="/css/default.css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <table border="0">
        <tr>
            <td>File:</td>
            <td><asp:FileUpload ID="oImport" runat="server" Width="450" CssClass="default" /></td>
        </tr>
        <tr>
            <td>Sheet Name:</td>
            <td><asp:TextBox ID="txtImport" runat="server" Width="100" CssClass="default" Text="Sheet1" /></td>
        </tr>
    </table>
    
    </div>
    </form>
</body>
</html>
