<%@ Page Language="C#" Debug="true" %>
<%@ Import Namespace="NCC.ClearView.Application.Core.Proteus" %>
<%@ Import Namespace="Novell.Directory.Ldap" %>

<script runat="server">

    protected void Page_Load(object sender, EventArgs e)
    {
    }
    public void btnGo_Click(object sender, EventArgs e)
    {
        txtAfter.Text = "";
        string[] strLines = txtBefore.Text.Split(new string[]{Environment.NewLine}, StringSplitOptions.None);
        foreach (string strLine in strLines)
        {
            if (strLine.Trim() != "")
                txtAfter.Text += txtChar.Text + (chkUpper.Checked ? strLine.Trim().ToUpper() : strLine.Trim()) + txtChar.Text + txtEnd.Text + Environment.NewLine;
        }
    }
</script>
<script type="text/javascript">
</script>
<html>
<head>
<title>SQL</title>
<link rel="stylesheet" type="text/css" href="/css/default.css" />
</head>
<body leftmargin="0" topmargin="0">
<form id="Form1" runat="server">
<table>
    <tr>
        <td>Char:</td>
        <td><asp:TextBox ID="txtChar" runat="server" Width="50" /></td>
        <td>Line End:</td>
        <td><asp:TextBox ID="txtEnd" runat="server" Width="50" /></td>
        <td><asp:CheckBox ID="chkUpper" runat="server" Text="Uppercase" /></td>
        <td><asp:Button ID="btnGo" runat="server" Text="Go" Width="75" OnClick="btnGo_Click" /></td>
    </tr>
    <tr>
        <td colspan="10">Before:</td>
    </tr>
    <tr>
        <td colspan="10"><asp:TextBox ID="txtBefore" runat="server" TextMode="MultiLine" Width="600" Rows="20" /></td>
    </tr>
    <tr>
        <td colspan="10">After:</td>
    </tr>
    <tr>
        <td colspan="10"><asp:TextBox ID="txtAfter" runat="server" TextMode="MultiLine" Width="600" Rows="20" /></td>
    </tr>
</table>
 
</form>
</body>
</html>