<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="Presentation.Web.Services._default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Button ID="btnLog" runat="server" Text="Create Log" OnClick="btnLog_Click" />
        <br /><br />
        <asp:Button ID="btnAvamarDomains" runat="server" Text="Get Avamar Domains" OnClick="btnAvamarDomains_Click" />
        <br />
        <h3>Servers</h3>
        <br />
        <asp:Button ID="btnServiceNow" runat="server" Text="Service Now PUT Server" OnClick="btnServiceNow_Click" />
        <br /><br />
        <asp:Button ID="btnServiceNow2" runat="server" Text="Service Now GET Server" OnClick="btnServiceNow2_Click" />
        <br /><br />
        <asp:Button ID="btnServiceNowDecom" runat="server" Text="Service Now Decom Server" OnClick="btnServiceNowDecom_Click" />
        <br /><br />
        <asp:Button ID="btnServiceNowRecom" runat="server" Text="Service Now Recom Server" OnClick="btnServiceNowRecom_Click" />
        <br />
        <h3>Incidents</h3>
        <br />
        <asp:Button ID="btnServiceNowIncidentGet" runat="server" Text="Service Now GET Incident" OnClick="btnServiceNowIncidentGet_Click" />
        <br /><br />
    </div>
    </form>
</body>
</html>
