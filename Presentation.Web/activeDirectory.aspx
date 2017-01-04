<%@ Page Language="C#" Debug="true" %>
<%@ Import Namespace="NCC.ClearView.Application.Core.Proteus" %>
<%@ Import Namespace="Novell.Directory.Ldap" %>

<script runat="server">
    private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
    private int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);

    protected void Page_Load(object sender, EventArgs e)
    {
        Variables oVariable = new Variables(999);
        Functions oFunction = new Functions(0, dsn, 999);
        AD oAD = new AD(0, dsn, 999);
        //DirectoryEntry oEntry = new DirectoryEntry(oVariable.primaryDC(dsn), oVariable.Domain() + "\\" + oVariable.ADUser(), oVariable.ADPassword());
        //DirectorySearcher oSearcher = new DirectorySearcher("(cn=XSXH33T)");

        if (String.IsNullOrEmpty(Request.QueryString["u"]) == false)
            LoadResults(new DirectorySearcher(new DirectoryEntry("LDAP://" + oVariable.eDirectoryHost() + ":636/ou=People,o=pnc", oVariable.eDirectoryUsername(), oVariable.eDirectoryPassword(), oFunction.eDirectory()), "(cn=" + Request.QueryString["u"] + ")"));
        else if (String.IsNullOrEmpty(Request.QueryString["email"]) == false)
        {
            Response.ContentType = "text/html";
            DirectorySearcher oSearcher = new DirectorySearcher(new DirectoryEntry("LDAP://" + oVariable.eDirectoryHost() + ":636/ou=People,o=pnc", oVariable.eDirectoryUsername(), oVariable.eDirectoryPassword(), oFunction.eDirectory()), "(cn=" + Request.QueryString["email"] + ")");
            SearchResultCollection oCollection = oSearcher.FindAll();
            if (oCollection.Count == 1)
                if (oCollection[0].Properties.Contains("mail"))
                    if (oCollection[0].Properties["mail"].Count == 1)
                        Response.Write(oCollection[0].Properties["mail"][0].ToString());
                    else
                        Response.Write("");
                else
                    Response.Write("");
            else
                Response.Write("");
            Response.End();
        }
        else if (String.IsNullOrEmpty(Request.QueryString["m"]) == false)
            LoadResults(new DirectorySearcher(new DirectoryEntry("LDAP://" + oVariable.eDirectoryHost() + ":636/ou=People,o=pnc", oVariable.eDirectoryUsername(), oVariable.eDirectoryPassword(), oFunction.eDirectory()), "(pncmanagerid=" + Request.QueryString["m"] + ")"));
        else if (String.IsNullOrEmpty(Request.QueryString["l"]) == false)
            LoadResults(new DirectorySearcher(new DirectoryEntry("LDAP://" + oVariable.eDirectoryHost() + ":636/ou=People,o=pnc", oVariable.eDirectoryUsername(), oVariable.eDirectoryPassword(), oFunction.eDirectory()), "(sn=" + Request.QueryString["l"] + ")"));
        else if (String.IsNullOrEmpty(Request.QueryString["x"]) == false)
            LoadResults(new DirectorySearcher(new DirectoryEntry("LDAP://" + oVariable.eDirectoryHost() + ":636/ou=People,o=pnc", oVariable.eDirectoryUsername(), oVariable.eDirectoryPassword(), oFunction.eDirectory()), "(businesscategory=" + Request.QueryString["x"] + ")"));
        else
            LoadResults(new DirectorySearcher(new DirectoryEntry("LDAP://" + oVariable.eDirectoryHost() + ":636/ou=People,o=pnc", oVariable.eDirectoryUsername(), oVariable.eDirectoryPassword(), oFunction.eDirectory()), "(cn=" + "PT43054" + ")"));
        //LoadResults(new DirectorySearcher(new DirectoryEntry("LDAP://" + oVariable.eDirectoryHost() + ":636/ou=People,o=pnc", oVariable.eDirectoryUsername(), oVariable.eDirectoryPassword(), oFunction.eDirectory()), "(pncmanagerid=" + strPNC + ")"));

        //DirectoryEntry oEntry = new DirectoryEntry( "LDAP://mdsrdcpv3.pncbank.com:389/ou=People,o=pnc", "cn=pt43054,ou=Employees,ou=People,o=pnc", "@live75", oFunction.eDirectory() );

        //DirectoryEntry oEntry = new DirectoryEntry("LDAP://mdsrdcpv3.pncbank.com:389/ou=People,o=pnc", "cn=pt43054,ou=Employees,ou=People,o=pnc", "@live71", oFunction.eDirectory() );
        //dap://mdsrdcpv3.pncbank.com:389/cn=ldapProxy,ou=DirectoryServers,o=pnc

        //LoadResults(new DirectorySearcher(new DirectoryEntry("LDAP://mdsrdcpv3.pncbank.com:389/ou=People,o=pnc", "cn=ldapProxy,ou=DirectoryServers,o=pnc", "", AuthenticationTypes.Anonymous), "(cn=PT43054)"));
        //LoadResults(new DirectorySearcher(new DirectoryEntry("LDAP://mdsrdcpv3.pncbank.com:389/ou=People,o=pnc", "cn=ldapProxy,ou=DirectoryServers,o=pnc", "", AuthenticationTypes.Delegation), "(cn=PT43054)"));
        //LoadResults(new DirectorySearcher(new DirectoryEntry("LDAP://mdsrdcpv3.pncbank.com:389/ou=People,o=pnc", "cn=ldapProxy,ou=DirectoryServers,o=pnc", "", AuthenticationTypes.Encryption), "(cn=PT43054)"));
        //LoadResults(new DirectorySearcher(new DirectoryEntry("LDAP://mdsrdcpv3.pncbank.com:389/ou=People,o=pnc", "cn=ldapProxy,ou=DirectoryServers,o=pnc", "", AuthenticationTypes.FastBind), "(cn=PT43054)"));
        //LoadResults(new DirectorySearcher(new DirectoryEntry("LDAP://mdsrdcpv3.pncbank.com:389/ou=People,o=pnc", "cn=ldapProxy,ou=DirectoryServers,o=pnc", "", AuthenticationTypes.None), "(cn=PT43054)"));
        //LoadResults(new DirectorySearcher(new DirectoryEntry("LDAP://mdsrdcpv3.pncbank.com:389/ou=People,o=pnc", "cn=ldapProxy,ou=DirectoryServers,o=pnc", "", AuthenticationTypes.ReadonlyServer), "(cn=PT43054)"));
        //LoadResults(new DirectorySearcher(new DirectoryEntry("LDAP://mdsrdcpv3.pncbank.com:389/ou=People,o=pnc", "cn=ldapProxy,ou=DirectoryServers,o=pnc", "", AuthenticationTypes.Sealing), "(cn=PT43054)"));
        //LoadResults(new DirectorySearcher(new DirectoryEntry("LDAP://mdsrdcpv3.pncbank.com:389/ou=People,o=pnc", "cn=ldapProxy,ou=DirectoryServers,o=pnc", "", AuthenticationTypes.Secure), "(cn=PT43054)"));
        //LoadResults(new DirectorySearcher(new DirectoryEntry("LDAP://mdsrdcpv3.pncbank.com:389/ou=People,o=pnc", "cn=ldapProxy,ou=DirectoryServers,o=pnc", "", AuthenticationTypes.SecureSocketsLayer), "(cn=PT43054)"));
        //LoadResults(new DirectorySearcher(new DirectoryEntry("LDAP://mdsrdcpv3.pncbank.com:389/ou=People,o=pnc", "cn=ldapProxy,ou=DirectoryServers,o=pnc", "", AuthenticationTypes.ServerBind), "(cn=PT43054)"));
        //LoadResults(new DirectorySearcher(new DirectoryEntry("LDAP://mdsrdcpv3.pncbank.com:389/ou=People,o=pnc", "cn=ldapProxy,ou=DirectoryServers,o=pnc", "", AuthenticationTypes.Signing), "(cn=PT43054)"));
        
        
        
        
        /*
        LdapConnection conn = new LdapConnection();
        conn.Connect("mdsrdcpv3.pncbank.com", 389);
        //conn.Connect("10.129.179.80", 389);
        //conn.Bind(null, null);
        conn.Bind("cn=ldapProxy,ou=DirectoryServers,o=pnc", "");
        //conn.Bind("CN=PT43054,OU=OUu_Standard,OU=OUc_User,OU=OUc_Accounts,DC=pncbank,DC=com", "@live71");


        LdapSearchResults lsc = conn.Search("", LdapConnection.SCOPE_SUB, "(cn=P*)", null, false);
        //dsaName = cn=lsmds302a-mdsn1r,ou=DirectoryServers,o=pnc
        Response.Write(lsc.Count.ToString());
*/
    }
    public void LoadResults(DirectorySearcher oSearcher)
    {
        SearchResultCollection oCollection = oSearcher.FindAll();
        foreach (SearchResult oResult in oCollection)
            foreach (string key in oResult.Properties.PropertyNames)
                foreach (object value in oResult.Properties[key])
                    lblResult.Text += "<b>" + key + "</b> = " + value.ToString() + "<br>";
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
<asp:Label ID="lblResult" runat="server" />
</form>
</body>
</html>