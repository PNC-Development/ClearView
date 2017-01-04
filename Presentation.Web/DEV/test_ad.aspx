<%@ Page Language="C#" Debug="false" EnableEventValidation="false" ValidateRequest="false" %>

<script runat="server">
    private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
    private int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
    private void Page_Load()
    {
        Variables oVar = new Variables(intEnvironment);
        AD oAD = new AD(0, dsn, intEnvironment);
        string strName = "PT42967";

        DirectoryEntry oEntry = new DirectoryEntry(oVar.primaryDC(dsn), oVar.Domain() + "\\" + oVar.ADUser(), oVar.ADPassword());
        Response.Write("Searching for " + strName + " on " + oVar.primaryDC(dsn) + " using " + oVar.Domain() + "\\" + oVar.ADUser() + "<br/>");
        try
        {
            DirectorySearcher oSearcher = new DirectorySearcher(oEntry);
            oSearcher.Filter = "(&(objectCategory=user)(sAMAccountName=" + strName + "*))";
            SearchResultCollection oResults = oSearcher.FindAll();
            if (oResults.Count == 1)
            {
                SearchResult oResult = oResults[0];
                if (oResult != null)
                {
                    Response.Write("User object has been created");
                }
                else
                    Response.Write("User object not well formed");
            }
            else if (oResults.Count == 0)
                Response.Write("There were no results");
            else
            {
                Response.Write("There were multiple (" + oResults.Count.ToString() + ") results");
                foreach (SearchResult oResult in oResults)
                {
                    DirectoryEntry oFound = oResult.GetDirectoryEntry();
                    Response.Write("<p>" + oFound.Properties["displayName"].Value.ToString() + "</p>");
                    foreach (PropertyValueCollection property in oFound.Properties)
                        Response.Write(property.PropertyName + "=" + oFound.Properties[property.PropertyName].Value.ToString() + "<br/>");
                }
            }
        }
        catch (Exception ex)
        {
            Response.Write(ex.Message);
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
    <body>
        <form id="Form1" runat="server">
        </form>
    </body>
</html>