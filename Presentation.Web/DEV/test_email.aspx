<%@ Page Language="C#" Debug="false" EnableEventValidation="false" ValidateRequest="false" MasterPageFile="~/clearview.master" %>
<script runat="server">
    private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
    private int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
    protected void Page_Load(object sender, EventArgs e)
    {
        string strUser = "SCLEAR2";
//        string strUser = "ESXH33T";
//        string strUser = "EMJY13C";
        AD oAD = new AD(0, dsn, 2);
        SearchResultCollection oResults = Search(strUser, "sAMAccountName");
        DirectoryEntry oEntry = oResults[0].GetDirectoryEntry();
        Response.Write("homeMDB: " + oEntry.Properties["homeMDB"].Value + "<br/>");
        Response.Write("homeMTA: " + oEntry.Properties["homeMTA"].Value + "<br/>");
        Response.Write("legacyExchangeDN: " + oEntry.Properties["legacyExchangeDN"].Value + "<br/>");
        Response.Write("mailNickname: " + oEntry.Properties["mailNickname"].Value + "<br/>");
        Response.Write("msExchHomeServerName: " + oEntry.Properties["msExchHomeServerName"].Value + "<br/>");
        Response.Write("msExchUserAccountControl: " + oEntry.Properties["msExchUserAccountControl"].Value + "<br/>");
        CDOEXM.IMailboxStore oMailbox = (CDOEXM.IMailboxStore)oEntry.NativeObject;
        Response.Write("HomeMDB: " + oMailbox.HomeMDB + "<br/>");
        if (oMailbox.HomeMDB == null)
        {
            //    oEntry.Properties["homeMDB"].Value = "CN=OHCLEMSX4031-DB01,CN=SG1,CN=InformationStore,CN=OHCLEMSX4031,CN=Servers,CN=NCB Administrative Group,CN=Administrative Groups,CN=NCC,CN=Microsoft Exchange,CN=Services,CN=Configuration,DC=ntl-city,DC=dev";
            //    oEntry.Properties["homeMTA"].Value = "CN=Microsoft MTA,CN=OHCLEMSX4031,CN=Servers,CN=NCB Administrative Group,CN=Administrative Groups,CN=NCC,CN=Microsoft Exchange,CN=Services,CN=Configuration,DC=ntl-city,DC=dev";
            //    oEntry.Properties["legacyExchangeDN"].Value = "/o=NCC/ou=First Administrative Group/cn=Recipients/cn=" + strUser;
            //    oEntry.Properties["mailNickname"].Value = strUser;
            //    oEntry.Properties["msExchHomeServerName"].Value = "/o=NCC/ou=First Administrative Group/cn=Configuration/cn=Servers/cn=OHCLEMSX4031";
            //    oEntry.Properties["msExchUserAccountControl"].Value = 0;
            //    oEntry.CommitChanges();

            oEntry.Properties["homeMDB"].Value = "CN=Mailbox Store 1 (OHCLEMSX4201),CN=First Storage Group,CN=InformationStore,CN=OHCLEMSX4201,CN=Servers,CN=NCB Administrative Group,CN=Administrative Groups,CN=NCC,CN=Microsoft Exchange,CN=Services,CN=Configuration,DC=ntl-city,DC=dev";
            oEntry.Properties["homeMTA"].Value = "CN=Microsoft MTA,CN=OHCLEMSX4201,CN=Servers,CN=NCB Administrative Group,CN=Administrative Groups,CN=NCC,CN=Microsoft Exchange,CN=Services,CN=Configuration,DC=ntl-city,DC=dev";
            oEntry.Properties["legacyExchangeDN"].Value = "/o=NCC/ou=First Administrative Group/cn=Recipients/cn=" + strUser;
            oEntry.Properties["mailNickname"].Value = strUser;
            oEntry.Properties["msExchHomeServerName"].Value = "/o=NCC/ou=First Administrative Group/cn=Configuration/cn=Servers/cn=OHCLEMSX4201";
            oEntry.Properties["msExchUserAccountControl"].Value = 0;
            oEntry.CommitChanges();

            //    //oMailbox.CreateMailbox("LDAP://CN=OHCLEMSX4031-DB01,CN=SG1,CN=InformationStore,CN=OHCLEMSX4031,CN=Servers,CN=NCB Administrative Group,CN=Administrative Groups,CN=NCC,CN=Microsoft Exchange,CN=Services,CN=Configuration,DC=corpdev,DC=ntl-city,DC=net");
            //    //            oMailbox.CreateMailbox("LDAP://CN=OHCLEMSX4031-DB01,CN=SG1,CN=InformationStore,CN=OHCLEMSX4031,CN=Servers,CN=NCB Administrative Group,CN=Administrative Groups,CN=NCC,CN=Microsoft Exchange,CN=Services,CN=Configuration,DC=corpdev,DC=ntl-city,DC=net");
            oMailbox.CreateMailbox("LDAP://Mailbox Store 1 (OHCLEMSX4201),CN=First Storage Group,CN=InformationStore,CN=OHCLEMSX4201,CN=Servers,CN=NCB Administrative Group,CN=Administrative Groups,CN=NCC,CN=Microsoft Exchange,CN=Services,CN=Configuration,DC=corpdev,DC=ntl-city,DC=net");
            oEntry.CommitChanges();
        }
        Response.Write("DONE!");
    }
    public SearchResultCollection Search(string _user, string _filters)
    {
        Variables oVariable = new Variables(intEnvironment);
        DirectoryEntry oEntry = new DirectoryEntry("LDAP://OHCLEWDC4202/DC=corpdev,DC=ntl-city,DC=net", "corpdev\\esxh33t", "Alive122179");
        DirectorySearcher oSearcher = new DirectorySearcher(oEntry);
        string strFilter = "";
        string[] strType;
        char[] strSplit = { ';' };
        strType = _filters.Split(strSplit);
        for (int ii = 0; ii < strType.Length; ii++)
        {
            if (strFilter == "")
                strFilter = "(" + strType[ii] + "=" + _user + "*)";
            else
                strFilter = "(|(" + strType[ii] + "=" + _user + "*)" + strFilter + ")";
        }
        oSearcher.Filter = strFilter;
        return oSearcher.FindAll();
    }
</script>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" Runat="Server">
</asp:Content>