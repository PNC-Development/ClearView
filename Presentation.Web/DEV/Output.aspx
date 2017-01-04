<%@ Page Language="C#" %>
<%@ Import Namespace="System.Diagnostics" %>
<script runat="server">
    protected string strLog = "";
    protected void Page_Load(Object Sender, EventArgs e)
    {
    }
    protected void btnLoad1_Click(Object Sender, EventArgs e)
    {
        // Get Event Log Entries
        string strSearch = txtSearch.Text.Trim().ToUpper();
        if (strSearch == "")
            strLog = "Please enter a server name to search";
        else
        {
            EventLog objEventLog = new EventLog("ClearView");
            foreach (EventLogEntry objEntry in objEventLog.Entries)
            {
                if (objEntry.Message.ToUpper().Contains(strSearch))
                    strLog += "<tr><td>" + objEntry.TimeGenerated + ": " + objEntry.Message + "</td></tr>";
            }
        }
    }
</script>
<script type="text/javascript">
</script>
<html>
<head>
<title>LOAD</title>
<link rel="stylesheet" type="text/css" href="/css/default.css" />
</head>
<body leftmargin="0" topmargin="0">
<form id="Form1" runat="server">
        <table cellspacing="20">
            <tr>
                <td class="header">Search Event Log</td>
            </tr>
            <tr>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td><asp:TextBox ID="txtSearch" runat="server" CssClass="default" Width="300" /></td>
            </tr>
            <tr>
                <td><asp:Button ID="btnLoad1" runat="server" CssClass="default" Width="100" Text="Search" OnClick="btnLoad1_Click" /></td>
            </tr>
            <tr>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td><%=strLog %></td>
            </tr>
            <tr>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td><asp:FileUpload ID="filTest" runat="server" Width="400" CssClass="file" /></td>
            </tr>
        </table>
</form>
</body>
</html>