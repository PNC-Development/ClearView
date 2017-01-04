<%@ Page Language="C#" Debug="true" %>
<%@ Import Namespace="NCC.ClearView.Application.Core.Proteus" %>
<%@ Import Namespace="Novell.Directory.Ldap" %>

<script runat="server">
    private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
    private int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
    private Users oUser;
    private Applications oApplication;
    private Groups oGroup;
    private Permissions oPermission;
    private NCC.ClearView.Application.Core.Roles oRole;
    private Variables oVariable;
    private Functions oFunction;
    private AppPages oAppPages;
    private int intClearViewUsersAppID = Int32.Parse(ConfigurationManager.AppSettings["ClearViewUsersAppID"]);

    protected void Page_Load(object sender, EventArgs e)
    {
        oVariable = new Variables(999);
        oUser = new Users(0, dsn);
        oRole = new NCC.ClearView.Application.Core.Roles(0, dsn);
        oApplication = new Applications(0, dsn);
        oGroup = new Groups(0, dsn);
        oPermission = new Permissions(0, dsn);
        oFunction = new Functions(0, dsn, intEnvironment);
        oAppPages = new AppPages(0, dsn);
        
        //DirectoryEntry oEntry = new DirectoryEntry(oVariable.primaryDC(dsn), oVariable.Domain() + "\\" + oVariable.ADUser(), oVariable.ADPassword());
        //DirectorySearcher oSearcher = new DirectorySearcher("(cn=XSXH33T)");

        if (String.IsNullOrEmpty(Request.QueryString["u"]) == false)
            LoadResults(new DirectorySearcher(new DirectoryEntry("LDAP://" + oVariable.eDirectoryHost() + ":636/ou=People,o=pnc", oVariable.eDirectoryUsername(), oVariable.eDirectoryPassword(), AuthenticationTypes.ServerBind), "(cn=" + Request.QueryString["u"] + ")"));
        else if (String.IsNullOrEmpty(Request.QueryString["email"]) == false)
        {
            DirectorySearcher oSearcher = new DirectorySearcher(new DirectoryEntry("LDAP://" + oVariable.eDirectoryHost() + ":636/ou=People,o=pnc", oVariable.eDirectoryUsername(), oVariable.eDirectoryPassword(), AuthenticationTypes.ServerBind), "(cn=" + Request.QueryString["email"] + ")");
            SearchResultCollection oCollection = oSearcher.FindAll();
            if (oCollection.Count == 1)
                if (oCollection[0].Properties.Contains("mail"))
                    if (oCollection[0].Properties["mail"].Count == 1)
                    {
                        Response.ContentType = "text/html";
                        Response.Write(oCollection[0].Properties["mail"][0].ToString());
                        Response.End();
                    }
                    else
                        Response.Write("C");
                else
                    Response.Write("E");
            else
                Response.Write("M");
        }
        else if (String.IsNullOrEmpty(Request.QueryString["emailtest"]) == false)
        {
            if (intEnvironment == (int)CurrentEnvironment.PNCNT_PROD)
                Response.Write(oUser.GetEmail(Request.QueryString["emailtest"], intEnvironment));
            else
            {
                //string url = "http://clearview.pncbank.com/activedirectory.aspx?email=PP65132";
                string url = "http://clearview.pncbank.com/activedirectory.aspx?email=PT43054";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                NetworkCredential netCredential = new NetworkCredential(oVariable.ADUser(), oVariable.ADPassword(), oVariable.Domain());
                request.Credentials = netCredential;
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Encoding encoding = ASCIIEncoding.ASCII;
                string strEmail = "";
                using (System.IO.StreamReader reader = new System.IO.StreamReader(response.GetResponseStream(), encoding))
                    strEmail = reader.ReadToEnd().Trim();
                Response.Write(strEmail);
            }
        }
        else if (String.IsNullOrEmpty(Request.QueryString["m"]) == false)
            //Sync(Request.QueryString["m"]);
            LoadResults(new DirectorySearcher(new DirectoryEntry("LDAP://" + oVariable.eDirectoryHost() + ":636/ou=People,o=pnc", oVariable.eDirectoryUsername(), oVariable.eDirectoryPassword(), AuthenticationTypes.ServerBind), "(pncmanagerid=" + Request.QueryString["m"] + ")"));
        else if (String.IsNullOrEmpty(Request.QueryString["l"]) == false)
            LoadResults(new DirectorySearcher(new DirectoryEntry("LDAP://" + oVariable.eDirectoryHost() + ":636/ou=People,o=pnc", oVariable.eDirectoryUsername(), oVariable.eDirectoryPassword(), AuthenticationTypes.ServerBind), "(sn=" + Request.QueryString["l"] + ")"));
        else if (String.IsNullOrEmpty(Request.QueryString["x"]) == false)
            LoadResults(new DirectorySearcher(new DirectoryEntry("LDAP://" + oVariable.eDirectoryHost() + ":636/ou=People,o=pnc", oVariable.eDirectoryUsername(), oVariable.eDirectoryPassword(), AuthenticationTypes.ServerBind), "(businesscategory=" + Request.QueryString["x"] + ")"));
        else if (String.IsNullOrEmpty(Request.QueryString["s"]) == false)
        {
            SearchResultCollection oCollection = oFunction.eDirectory(Request.QueryString["s"]);
            if (oCollection.Count == 1 && oCollection[0].Properties.Contains("pncmanagerid") == true)
                Response.Write(eDirectory(oCollection[0], "pncmanagerid"));
            else
                Response.Write("None");
        }
        else
            LoadResults(new DirectorySearcher(new DirectoryEntry("LDAP://mdsemp.pncbank.com:636/ou=People,o=pnc", "cn=clv-serviceid,ou=TrustedApplications,ou=FrameworkSystems,o=pnc", "cPPnuh", AuthenticationTypes.Encryption), "(CN=PT43054)"));
        //LoadResults(new DirectorySearcher(new DirectoryEntry("LDAP://" + oVariable.eDirectoryHost() + ":636/ou=People,o=pnc", oVariable.eDirectoryUsername(), oVariable.eDirectoryPassword(), AuthenticationTypes.ServerBind), "(pncmanagerid=" + strPNC + ")"));
        
        //DirectoryEntry oEntry = new DirectoryEntry( "LDAP://mdsrdcpv3.pncbank.com:389/ou=People,o=pnc", "cn=pt43054,ou=Employees,ou=People,o=pnc", "@live75", AuthenticationTypes.ServerBind );
        
        //DirectoryEntry oEntry = new DirectoryEntry("LDAP://mdsrdcpv3.pncbank.com:389/ou=People,o=pnc", "cn=pt43054,ou=Employees,ou=People,o=pnc", "@live71", AuthenticationTypes.ServerBind );
        //dap://mdsrdcpv3.pncbank.com:389/cn=ldapProxy,ou=DirectoryServers,o=pnc

        //DirectorySearcher oSearcher = new DirectorySearcher(new DirectoryEntry("LDAP://mdsrdct.pncbank.com:636/ou=People,o=pnc", "cn=clv-serviceid,ou=TrustedApplications,ou=FrameworkSystems,o=pnc", "ff4cN$", AuthenticationTypes.ServerBind), "(CN=PT43054)");
        //SearchResultCollection oCollection = oSearcher.FindAll();

        //DirectorySearcher oSearcher2 = new DirectorySearcher(new DirectoryEntry("LDAPS://mdsemptest.pncbank.com:636/ou=People,o=pnc", "cn=clv-serviceid,ou=TrustedApplications,ou=FrameworkSystems,o=pnc", "AY3AVN", AuthenticationTypes.ServerBind), "(CN=PT43054)");
        //SearchResultCollection oCollection2 = oSearcher2.FindAll();

        //LoadResults(new DirectorySearcher(new DirectoryEntry("LDAP://mdsemptest.pncbank.com:636/ou=employees,ou=people,o=pnc", "cn=clv-serviceid,ou=TrustedApplications,ou=FrameworkSystems,o=pnc", "AY3AVN", AuthenticationTypes.Anonymous), "(CN=PT43054)"));
        //LoadResults(new DirectorySearcher(new DirectoryEntry("LDAP://mdsemptest.pncbank.com:636/ou=employees,ou=people,o=pnc", "cn=clv-serviceid,ou=TrustedApplications,ou=FrameworkSystems,o=pnc", "AY3AVN", AuthenticationTypes.Delegation), "(CN=PT43054)"));
        //LoadResults(new DirectorySearcher(new DirectoryEntry("LDAP://mdsemptest.pncbank.com:636/ou=employees,ou=people,o=pnc", "cn=clv-serviceid,ou=TrustedApplications,ou=FrameworkSystems,o=pnc", "AY3AVN", AuthenticationTypes.Encryption), "(CN=PT43054)"));  // good
        //LoadResults(new DirectorySearcher(new DirectoryEntry("LDAP://mdsemptest.pncbank.com:636/ou=employees,ou=people,o=pnc", "cn=clv-serviceid,ou=TrustedApplications,ou=FrameworkSystems,o=pnc", "AY3AVN", AuthenticationTypes.FastBind), "(CN=PT43054)"));
        //LoadResults(new DirectorySearcher(new DirectoryEntry("LDAP://mdsemptest.pncbank.com:636/ou=employees,ou=people,o=pnc", "cn=clv-serviceid,ou=TrustedApplications,ou=FrameworkSystems,o=pnc", "AY3AVN", AuthenticationTypes.None), "(CN=PT43054)"));
        //LoadResults(new DirectorySearcher(new DirectoryEntry("LDAP://mdsemptest.pncbank.com:636/ou=employees,ou=people,o=pnc", "cn=clv-serviceid,ou=TrustedApplications,ou=FrameworkSystems,o=pnc", "AY3AVN", AuthenticationTypes.ReadonlyServer), "(CN=PT43054)"));
        //LoadResults(new DirectorySearcher(new DirectoryEntry("LDAP://mdsemptest.pncbank.com:636/ou=employees,ou=people,o=pnc", "cn=clv-serviceid,ou=TrustedApplications,ou=FrameworkSystems,o=pnc", "AY3AVN", AuthenticationTypes.Sealing), "(CN=PT43054)"));
        //LoadResults(new DirectorySearcher(new DirectoryEntry("LDAP://mdsemptest.pncbank.com:636/ou=employees,ou=people,o=pnc", "cn=clv-serviceid,ou=TrustedApplications,ou=FrameworkSystems,o=pnc", "AY3AVN", AuthenticationTypes.Secure), "(CN=PT43054)"));
        //LoadResults(new DirectorySearcher(new DirectoryEntry("LDAP://mdsemptest.pncbank.com:636/ou=employees,ou=people,o=pnc", "cn=clv-serviceid,ou=TrustedApplications,ou=FrameworkSystems,o=pnc", "AY3AVN", AuthenticationTypes.SecureSocketsLayer), "(CN=PT43054)"));  // good
        //LoadResults(new DirectorySearcher(new DirectoryEntry("LDAP://mdsemptest.pncbank.com:636/ou=employees,ou=people,o=pnc", "cn=clv-serviceid,ou=TrustedApplications,ou=FrameworkSystems,o=pnc", "AY3AVN", AuthenticationTypes.ServerBind), "(CN=PT43054)"));
        //LoadResults(new DirectorySearcher(new DirectoryEntry("LDAP://mdsemptest.pncbank.com:636/ou=employees,ou=people,o=pnc", "cn=clv-serviceid,ou=TrustedApplications,ou=FrameworkSystems,o=pnc", "AY3AVN", AuthenticationTypes.Signing), "(CN=PT43054)"));
        
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
        try
        {
            SearchResultCollection oCollection = oSearcher.FindAll();
            foreach (SearchResult oResult in oCollection)
                foreach (string key in oResult.Properties.PropertyNames)
                    foreach (object value in oResult.Properties[key])
                    {
                        if (value.ToString() == "System.Byte[]")
                        {
                            byte[] bytearray = (byte[])value;
                            lblResult.Text += "<b>" + key + "</b> = " + Encoding.Default.GetString(bytearray) + "<br>";
                        }
                        else
                            lblResult.Text += "<b>" + key + "</b> = " + value.ToString() + "<br>";
                    }
        }
        catch (Exception ex)
        {
            Response.Write(ex.Message + "<br/>");
        }
    }
    public string eDirectory(SearchResult entry, string property)
    {
        if (entry.Properties.Contains(property))
        {
            object value = entry.Properties[property][0];
            if (value.ToString() == "System.Byte[]")
            {
                byte[] bytearray = (byte[])value;
                return Encoding.Default.GetString(bytearray);
            }
            else
                return value.ToString();
        }
        else
            return "";
    }

    public void Sync(string _pnc_id)
    {
        Response.Write("<ul>");
        // Get reporting structure
        SearchResultCollection oCollection = GetDirectorySearcher("pncmanagerid", _pnc_id);
        foreach (SearchResult oResult in oCollection)
        {
            Response.Write("<li>");
            if (oResult.GetDirectoryEntry().Properties.Contains("displayname"))
                Response.Write(oResult.GetDirectoryEntry().Properties["displayname"].Value.ToString());
            string strID = "";
            if (oResult.GetDirectoryEntry().Properties.Contains("cn"))
                strID = oResult.GetDirectoryEntry().Properties["cn"].Value.ToString();
            string strXID = "";
            if (oResult.GetDirectoryEntry().Properties.Contains("businesscategory"))
                strXID = oResult.GetDirectoryEntry().Properties["businesscategory"].Value.ToString();
            string strFirst = "";
            if (oResult.GetDirectoryEntry().Properties.Contains("givenname"))
                strFirst = oResult.GetDirectoryEntry().Properties["givenname"].Value.ToString();
            string strLast = "";
            if (oResult.GetDirectoryEntry().Properties.Contains("sn"))
                strLast = oResult.GetDirectoryEntry().Properties["sn"].Value.ToString();
            string strPhone = "";
            if (oResult.GetDirectoryEntry().Properties.Contains("telephonenumber"))
                strPhone = oResult.GetDirectoryEntry().Properties["telephonenumber"].Value.ToString();
            string strDepartment = "";
            if (oResult.GetDirectoryEntry().Properties.Contains("pncorganizationalunit"))
                strDepartment = oFunction.ToTitleCase(oResult.GetDirectoryEntry().Properties["pncorganizationalunit"].Value.ToString());
            int intManager = oUser.GetId(_pnc_id);

            if (strID != "")
            {
                int intUser = oUser.GetId(strID);
                if (intUser == 0)
                {
                    // Add the person
                    intUser = oUser.Add(strXID, strID, strFirst, strLast, intManager, 0, 0, 0, "", 0, strPhone, "", 0, 0, 0, 0, 1);
                }
                
                bool boolApplicationChange = true;
                DataSet dsRoles = oRole.Gets(intUser);
                foreach (DataRow drRole in dsRoles.Tables[0].Rows)
                {
                    if (drRole["name"].ToString().Trim().ToUpper() == strDepartment.Trim().ToUpper()) 
                    {
                        // Application found
                        boolApplicationChange = false;
                        break;
                    }
                }

                if (boolApplicationChange == true)
                {
                    bool boolPermission = false;
                    
                    // Find application
                    DataSet dsApplication = oFunction.ExecuteDataset("SELECT * FROM cv_applications WHERE name = '" + strDepartment + "'");
                    int intApplication = 0;
                    if (dsApplication.Tables[0].Rows.Count > 0)
                        Int32.TryParse(dsApplication.Tables[0].Rows[0]["applicationid"].ToString(), out intApplication);
                    if (intApplication == 0)
                    {
                        boolPermission = true;
                        // Create it.
                        string strDepartmentURL = strDepartment;
                        while (strDepartmentURL.Contains(" "))
                            strDepartmentURL = strDepartmentURL.Replace(" ", "");
                        string strManagerDept = strID;
                        string strManager = strManagerDept;
                        // Check to see if manager has same department
                        while (GetDirectoryEntryProperty(strManager, "pncorganizationalunit").ToUpper() == strDepartment.ToUpper())
                        {
                            strManagerDept = strManager;
                            strManager = GetDirectoryEntryProperty(strManagerDept, "pncmanagerid");
                        }
                        int intManagerDept = oUser.GetId(strManagerDept);
                        //oApplication.Add(strDepartment, Server.UrlEncode(strDepartmentURL), strDepartment, strDepartment, "", "", intManagerDept, 0, 0, 0, 0, 1, 0, 0, "", 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1);
                        dsApplication = oFunction.ExecuteDataset("SELECT * FROM cv_applications WHERE name = '" + strDepartment + "'");
                        if (dsApplication.Tables[0].Rows.Count > 0)
                            Int32.TryParse(dsApplication.Tables[0].Rows[0]["applicationid"].ToString(), out intApplication);
                    }
                    
                    // Find group
                    DataSet dsGroup = oFunction.ExecuteDataset("SELECT * FROM cv_groups WHERE name = '" + strDepartment + "'");
                    int intGroup = 0;
                    if (dsGroup.Tables[0].Rows.Count > 0)
                        Int32.TryParse(dsGroup.Tables[0].Rows[0]["groupid"].ToString(), out intGroup);
                    if (intGroup == 0)
                    {
                        boolPermission = true;
                        // Create group
                        oGroup.Add(strDepartment, strDepartment, 1);
                        dsGroup = oFunction.ExecuteDataset("SELECT * FROM cv_groups WHERE name = '" + strDepartment + "'");
                        if (dsGroup.Tables[0].Rows.Count > 0)
                            Int32.TryParse(dsGroup.Tables[0].Rows[0]["groupid"].ToString(), out intGroup);
                    }

                    if (boolPermission)
                    {
                        // Add group to application
                        oPermission.Add(intApplication, intGroup, 1);
                        
                        // Add default pages
                        DataSet dsAppPages = oAppPages.Gets(intClearViewUsersAppID);
                        foreach (DataRow drAppPages in dsAppPages.Tables[0].Rows)
                        {
                            int intPageID = Int32.Parse(drAppPages["pageid"].ToString());
                            oAppPages.Add(intPageID, intApplication);
                        }
                    }
                    
                    // Delete current application(s)
                    oRole.DeleteUser(intUser);

                    // Add current role
                    oRole.Add(intUser, intGroup);
                }
                
                // Sync this user
                Sync(strID);
            }
            Response.Write("</li>");
        }
        Response.Write("</ul>");
    }
    public string GetDirectoryEntryProperty(string _pnc_id, string _property)
    {
        string strReturn = "";
        SearchResultCollection oCollection = GetDirectorySearcher("cn", _pnc_id);
        if (oCollection.Count == 1 && oCollection[0].GetDirectoryEntry().Properties.Contains(_property) == true)
            strReturn = oCollection[0].GetDirectoryEntry().Properties[_property].Value.ToString();
        return strReturn;
    }
    public SearchResultCollection GetDirectorySearcher(string _search_attribute, string _search_value)
    {
        Variables oVariable = new Variables(intEnvironment);
        DirectorySearcher oSearcher = new DirectorySearcher(new DirectoryEntry("LDAP://" + oVariable.eDirectoryHost() + ":636/ou=People,o=pnc", oVariable.eDirectoryUsername(), oVariable.eDirectoryPassword(), AuthenticationTypes.ServerBind), "(" + _search_attribute + "=" + _search_value + ")");
        return oSearcher.FindAll();
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