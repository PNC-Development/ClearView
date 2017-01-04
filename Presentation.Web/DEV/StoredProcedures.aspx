<%@ Page Language="C#" Debug="false" EnableEventValidation="false" ValidateRequest="false"%>
<script runat="server">
    private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
    private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
    private string dsnServiceEditor = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["ServiceEditorDSN"]].ConnectionString;
    private string strNewLine = Environment.NewLine;
    private string strTab = "\t";
    private string strPreface = "CREATE";
    private string strIdentity = "";
    private bool boolIdentity = false;
    private bool boolDisplay = false;
    private bool boolEnabled = false;
    private bool boolModified = false;
    private bool boolDeleted = false;
    private string strParamsSP = "";
    private string strParamsCode = "";
    private string strTable = "";
    private string strName = "";
    private string strPrefix = "";
    private string strOrder = "";
    private string strParent = "";
    private char[] strSplit = { ';' };

    private string strDSN = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        strDSN = dsn;
        
        //double dblTest = double.Parse("27.525");
        //Response.Write(dblTest.ToString("F") + "<br/>");    // 27.53
        //Response.Write(dblTest.ToString("0") + "<br/>");    // 28
        //Response.Write(dblTest.ToString("C") + "<br/>");    // $27.53
    }
    protected void btnGo_Click(Object Sender, EventArgs e)
    {
        //dsn = dsnAsset;
        txtOutput.Text = "";
        txtCode.Text = "";

        if (chkAlter.Checked == true)
            strPreface = "ALTER";
        
        // Get Event Log Entries
        strTable = txtTable.Text.Trim();
        strName = txtNaming.Text.Trim();
        strPrefix = txtPrefix.Text.Trim();
        strOrder = txtOrder.Text.Trim();
        strParent = txtParent.Text.Trim();

        if (strTable == "")
            txtOutput.Text = "Please enter a table to query";
        else
        {
            bool boolFound = false;
            DataSet dsFound = SqlHelper.ExecuteDataset(strDSN, CommandType.Text, "select name from sys.tables order by name");
            foreach (DataRow drFound in dsFound.Tables[0].Rows)
            {
                if (drFound["name"].ToString().Trim().ToUpper() == strTable.ToUpper())
                {
                    boolFound = true;
                    break;
                }
            }
            if (boolFound == true)
            {
                // GET IDENTITY
                DataSet ds = SqlHelper.ExecuteDataset(strDSN, CommandType.Text, "select c.name, c.is_identity, c.max_length AS length, t.name AS type from sys.columns c LEFT OUTER JOIN sys.types t ON t.system_type_id = c.system_type_id AND t.user_type_id <= 255 where c.object_id =object_id('" + strTable + "')");
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (dr["is_identity"].ToString().Trim() == "1" || dr["is_identity"].ToString().Trim().ToUpper() == "TRUE")
                    {
                        strIdentity = dr["name"].ToString().Trim();
                        break;
                    }
                }

                if (chkVersion.Checked == false && strIdentity == "")
                    txtOutput.Text = "There was no identity field found (did you forget to add a parent column?)";
                else
                {
                    boolIdentity = (strIdentity != "" && chkIdentity.Checked == true);
                    
                    // CREATE
                    AddCreate();
                    txtOutput.Text += "GO" + strNewLine + strNewLine;

                    if (chkVersion.Checked == false)
                    {
                        // UPDATE
                        AddUpdate();
                        txtOutput.Text += "GO" + strNewLine + strNewLine;
                    }

                    // DELETE
                    AddDelete();
                    txtOutput.Text += "GO" + strNewLine + strNewLine;

                    if (chkVersion.Checked == false)
                    {
                        // GET
                        AddGet();
                        txtOutput.Text += "GO" + strNewLine + strNewLine;
                    }

                    // GETS
                    AddGets();
                    txtOutput.Text += "GO" + strNewLine + strNewLine;
                }
            }
            else
                txtOutput.Text = "Invalid table name";
        }
    }
    protected void GetParams(bool boolUpdate)
    {
        strParamsSP = "";
        strParamsCode = "";
        DataSet ds = SqlHelper.ExecuteDataset(strDSN, CommandType.Text, "select c.name, c.is_identity, c.max_length AS length, t.name AS type from sys.columns c LEFT OUTER JOIN sys.types t ON t.system_type_id = c.system_type_id AND t.user_type_id <= 255 where c.object_id =object_id('" + strTable + "')");
        string strOutput = "";
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            string strField = dr["name"].ToString().Trim();
            string strType = dr["type"].ToString().Trim();
            string strLength = dr["length"].ToString().Trim();
            switch (strField.ToUpper())
            {
                case "CREATED":
                    break;
                case "MODIFIED":
                    boolModified = true;
                    break;
                case "DELETED":
                    boolDeleted = true;
                    break;
                default:
                    bool boolOK = false;
                    string strSuffix = "";
                    if (strField.ToUpper() != strIdentity.ToUpper())
                    {
                        if (strField.ToUpper() != "DISPLAY")
                            boolOK = true;
                        else
                        {
                            boolDisplay = true;
                            if (chkDisplay.Checked == true && boolUpdate == false)
                                boolOK = true;
                        }
                    }
                    else
                    {
                        if (boolUpdate == true)
                            boolOK = true;
                        else if (chkIdentity.Checked == true)
                            strOutput = strTab + "@" + strField + " " + strType + " output";
                    }

                    if (strField.ToUpper() == "ENABLED")
                        boolEnabled = true;

                    if (boolOK == true)
                    {
                        if (strParamsSP != "")
                            strParamsSP += "," + strNewLine;
                        if (strParamsCode != "")
                            strParamsCode += ", ";
                        strParamsSP += strTab + "@" + strField + " " + strType;
                        strParamsCode += SQLtoC(strType) + " _" + strField;
                        if (strType == "varchar")
                            strParamsSP += "(" + (strLength == "-1" ? "MAX" : strLength) + ")";
                    }
                    break;
            }
        }
        if (strOutput != "")
        {
            strParamsSP += "," + strNewLine;
            strParamsSP += strOutput;
        }
        strParamsSP += strNewLine;
    }
    protected void AddCreate()
    {
        // Stored Procedure
        string strSP = "";
        string strCode = "";
        DataSet ds = SqlHelper.ExecuteDataset(strDSN, CommandType.Text, "select c.name, c.is_identity, c.max_length AS length, t.name AS type from sys.columns c LEFT OUTER JOIN sys.types t ON t.system_type_id = c.system_type_id AND t.user_type_id <= 255 where c.object_id =object_id('" + strTable + "')");
        strSP += strPreface + " PROCEDURE [dbo].[pr_add" + strName + "]" + strNewLine;
        GetParams(false);
        strCode += "public " + (boolIdentity == true ? "int" : "void") + " Add" + strPrefix + "(" + strParamsCode + ")" + strNewLine;
        strCode += "{" + strNewLine;
        strSP += strParamsSP;
        strSP += "AS" + strNewLine;
        strSP += "INSERT INTO" + strNewLine;
        strSP += strTab + strTable + strNewLine;
        strSP += "VALUES" + strNewLine;
        strSP += "(" + strNewLine;
        string strCodeParams = "";
        int intParams = 0;
        string strFields = "";
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            string strField = dr["name"].ToString().Trim();
            string strType = dr["type"].ToString().Trim();
            string strLength = dr["length"].ToString().Trim();
            if (strField.ToUpper() != strIdentity.ToUpper())
            {
                if (strFields != "")
                    strFields += "," + strNewLine;
                if (strField.ToUpper() == "CREATED" || strField.ToUpper() == "MODIFIED")
                    strFields += strTab + "getdate()";
                else if (strField.ToUpper() == "DELETED")
                    strFields += strTab + "0";
                else if (strField.ToUpper() == "DISPLAY" && chkDisplay.Checked == false)
                    strFields += strTab + "0";
                else
                {
                    strFields += strTab + "@" + strField;
                    strCodeParams += strTab + "arParams[" + intParams.ToString() + "] = new SqlParameter(\"@" + strField + "\", _" + strField + ");" + strNewLine;
                    intParams++;
                }
            }
        }
        strFields += strNewLine;
        strSP += strFields;
        strSP += ")" + strNewLine;
        if (boolIdentity == true)
        {
            strSP += "SET @" + strIdentity + " = SCOPE_IDENTITY()" + strNewLine;
            strCodeParams += strTab + "arParams[" + intParams.ToString() + "] = new SqlParameter(\"@" + strIdentity + "\", SqlDbType.Int);" + strNewLine;
            strCodeParams += strTab + "arParams[" + intParams.ToString() + "].Direction = ParameterDirection.Output;" + strNewLine;
            int intIdentityParams = intParams + 1;
            strCodeParams = strTab + "arParams = new SqlParameter[" + intIdentityParams.ToString() + "];" + strNewLine + strCodeParams;
            strCode += strCodeParams;
            strCode += strTab + "SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, \"pr_add" + strName + "\", arParams);" + strNewLine;
            strCode += strTab + "return Int32.Parse(arParams[" + intParams.ToString() + "].Value.ToString());" + strNewLine;
        }
        else
        {
            strCodeParams = strTab + "arParams = new SqlParameter[" + intParams.ToString() + "];" + strNewLine + strCodeParams;
            strCode += strCodeParams;
            strCode += strTab + "SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, \"pr_add" + strName + "\", arParams);" + strNewLine;
        }
        strCode += "}" + strNewLine;
        txtOutput.Text += strSP;
        txtCode.Text += strCode;
    }
    protected void AddUpdate()
    {
        // Stored Procedure
        string strSP = "";
        string strCode = "";
        DataSet ds = SqlHelper.ExecuteDataset(strDSN, CommandType.Text, "select c.name, c.is_identity, c.max_length AS length, t.name AS type from sys.columns c LEFT OUTER JOIN sys.types t ON t.system_type_id = c.system_type_id AND t.user_type_id <= 255 where c.object_id =object_id('" + strTable + "')");
        strSP += strPreface + " PROCEDURE [dbo].[pr_update" + strName + "]" + strNewLine;
        GetParams(true);
        strCode += "public void Update" + strPrefix + "(" + strParamsCode + ")" + strNewLine;
        strCode += "{" + strNewLine;
        strSP += strParamsSP;
        strSP += "AS" + strNewLine;
        strSP += "UPDATE" + strNewLine;
        strSP += strTab + strTable + strNewLine;
        strSP += "SET" + strNewLine;
        string strCodeParams = "";
        int intParams = 0;
        string strFields = "";
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            string strField = dr["name"].ToString().Trim();
            string strType = dr["type"].ToString().Trim();
            string strLength = dr["length"].ToString().Trim();
            if (strField.ToUpper() != strIdentity.ToUpper())
            {
                switch (strField.ToUpper())
                {
                    case "DISPLAY":
                        break;
                    case "CREATED":
                        break;
                    case "DELETED":
                        break;
                    default:
                        if (strFields != "")
                            strFields += "," + strNewLine;
                        if (strField.ToUpper() == "MODIFIED")
                            strFields += strTab + "[" + strField + "] = getdate()";
                        else
                        {
                            strFields += strTab + "[" + strField + "] = " + "@" + strField;
                            strCodeParams += strTab + "arParams[" + intParams.ToString() + "] = new SqlParameter(\"@" + strField + "\", _" + strField + ");" + strNewLine;
                            intParams++;
                        }
                        break;
                }
            }
            else
            {
                strCodeParams += strTab + "arParams[" + intParams.ToString() + "] = new SqlParameter(\"@" + strField + "\", _" + strField + ");" + strNewLine;
                intParams++;
            }
        }
        strFields += strNewLine;
        strSP += strFields;
        strSP += "WHERE" + strNewLine;
        strSP += strTab + strIdentity + " = @" + strIdentity + strNewLine;
        strCodeParams = strTab + "arParams = new SqlParameter[" + intParams.ToString() + "];" + strNewLine + strCodeParams;
        strCode += strCodeParams;
        strCode += strTab + "SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, \"pr_update" + strName + "\", arParams);" + strNewLine;

        if (boolDisplay == true)
        {
            // Add the Order Update
            strSP += "GO" + strNewLine + strNewLine;
            strSP += strPreface + " PROCEDURE [dbo].[pr_update" + strName + "Order]" + strNewLine;
            strSP += strTab + "@" + strIdentity + " int," + strNewLine;
            strSP += strTab + "@display int" + strNewLine;
            strSP += "AS" + strNewLine;
            strSP += "UPDATE" + strNewLine;
            strSP += strTab + strTable + strNewLine;
            strSP += "SET" + strNewLine;
            strSP += strTab + "display = @display" + strNewLine;
            strSP += "WHERE" + strNewLine;
            strSP += strTab + strIdentity + " = @" + strIdentity + strNewLine;
            strCode += "}" + strNewLine;
            strCode += "public void Update" + strPrefix + "Order(int _" + strIdentity + ", int _display)" + strNewLine;
            strCode += "{" + strNewLine;
            strCode += strTab + "arParams = new SqlParameter[2];" + strNewLine;
            strCode += strTab + "arParams[0] = new SqlParameter(\"@" + strIdentity + "\", _" + strIdentity + ");" + strNewLine;
            strCode += strTab + "arParams[1] = new SqlParameter(\"@display\", _display);" + strNewLine;
            strCode += strTab + "SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, \"pr_update" + strName + "Order\", arParams);" + strNewLine;
        }

        if (boolEnabled == true && chkDisable.Checked == false)
        {
            // Add the Enabled Update
            strSP += "GO" + strNewLine + strNewLine;
            strSP += strPreface + " PROCEDURE [dbo].[pr_update" + strName + "Enabled]" + strNewLine;
            strSP += strTab + "@" + strIdentity + " int," + strNewLine;
            strSP += strTab + "@enabled int" + strNewLine;
            strSP += "AS" + strNewLine;
            strSP += "UPDATE" + strNewLine;
            strSP += strTab + strTable + strNewLine;
            strSP += "SET" + strNewLine;
            strSP += strTab + "enabled = @enabled" + (boolModified ? "," : "") + strNewLine;
            if (boolModified == true)
                strSP += strTab + "modified = getdate()" + strNewLine;
            strSP += "WHERE" + strNewLine;
            strSP += strTab + strIdentity + " = @" + strIdentity + strNewLine;
            strCode += "}" + strNewLine;
            strCode += "public void Enable" + strPrefix + "(int _" + strIdentity + ", int _enabled)" + strNewLine;
            strCode += "{" + strNewLine;
            strCode += strTab + "arParams = new SqlParameter[2];" + strNewLine;
            strCode += strTab + "arParams[0] = new SqlParameter(\"@" + strIdentity + "\", _" + strIdentity + ");" + strNewLine;
            strCode += strTab + "arParams[1] = new SqlParameter(\"@enabled\", _enabled);" + strNewLine;
            strCode += strTab + "SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, \"pr_update" + strName + "Enabled\", arParams);" + strNewLine;
        }
        strCode += "}" + strNewLine;
        txtOutput.Text += strSP;
        txtCode.Text += strCode;
    }
    protected void AddDelete()
    {
        // Stored Procedure
        string strSP = "";
        string strCode = "";
        string strParam = "";
        string strWhere = "";
        string strCodeDeclarations = "";
        string strCodeParams = "";
        int intParams = 0;
        if (chkVersion.Checked == false || strParent == "")
        {
            strParam += strTab + "@" + strIdentity + " int" + strNewLine;
            strWhere += strTab + strIdentity + " = @" + strIdentity + strNewLine;
            strCodeDeclarations += "int _" + strIdentity;
            strCodeParams += strTab + "arParams[" + intParams.ToString() + "] = new SqlParameter(\"@" + strIdentity + "\", _" + strIdentity + ");" + strNewLine;
            intParams++;
        }
        else
        {
            string[] strParents = strParent.Split(strSplit);
            for (int ii = 0; ii < strParents.Length; ii++)
            {
                bool boolLast = ((ii + 1) == strParents.Length);
                strParam += strTab + "@" + strParents[ii] + " int" + (boolLast == false ? "," : "") + strNewLine;
                strWhere += strTab + (strWhere != "" ? "AND " : "") + strParents[ii] + " = @" + strParents[ii] + strNewLine;
                if (strCodeDeclarations != "")
                    strCodeDeclarations += ", ";
                strCodeDeclarations += "int _" + strParents[ii];
                strCodeParams += strTab + "arParams[" + intParams.ToString() + "] = new SqlParameter(\"@" + strParents[ii] + "\", _" + strParents[ii] + ");" + strNewLine;
                intParams++;
            }
        }
        strSP += strPreface + " PROCEDURE [dbo].[pr_delete" + strName + "]" + strNewLine;
        strSP += strParam;
        strSP += "AS" + strNewLine;
        strSP += "UPDATE" + strNewLine;
        strSP += strTab + strTable + strNewLine;
        strSP += "SET" + strNewLine;
        strSP += strTab + "deleted = 1," + strNewLine;
        strSP += strTab + "modified = getdate()" + strNewLine;
        if (strWhere != "")
        {
            strSP += "WHERE" + strNewLine;
            strSP += strWhere;
        }
        txtOutput.Text += strSP;

        strCode += "public void Delete" + strPrefix + "(" + strCodeDeclarations + ")" + strNewLine;
        strCode += "{" + strNewLine;
        strCode += strTab + "arParams = new SqlParameter[" + intParams.ToString() + "];" + strNewLine;
        strCode += strCodeParams;
        strCode += strTab + "SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, \"pr_delete" + strName + "\", arParams);" + strNewLine;
        strCode += "}" + strNewLine;
        txtCode.Text += strCode;
    }
    protected void AddGet()
    {
        // Stored Procedure
        string strSP = "";
        strSP += strPreface + " PROCEDURE [dbo].[pr_get" + strName + "]" + strNewLine;
        strSP += strTab + "@" + strIdentity + " int" + strNewLine;
        strSP += "AS" + strNewLine;
        strSP += "SELECT" + strNewLine;
        strSP += strTab + "*" + strNewLine;
        strSP += "FROM" + strNewLine;
        strSP += strTab + strTable + strNewLine;
        strSP += "WHERE" + strNewLine;
        strSP += strTab + strIdentity + " = @" + strIdentity + strNewLine;
        txtOutput.Text += strSP;

        string strCode = "";
        strCode += "public DataSet Get" + strPrefix + "(int _" + strIdentity + ")" + strNewLine;
        strCode += "{" + strNewLine;
        strCode += strTab + "arParams = new SqlParameter[1];" + strNewLine;
        strCode += strTab + "arParams[0] = new SqlParameter(\"@" + strIdentity + "\", _" + strIdentity + ");" + strNewLine;
        strCode += strTab + "return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, \"pr_get" + strName + "\", arParams);" + strNewLine;
        strCode += "}" + strNewLine;
        strCode += "public string Get" + strPrefix + "(int _" + strIdentity + ", string _column)" + strNewLine;
        strCode += "{" + strNewLine;
        strCode += strTab + "DataSet ds = Get" + strPrefix + "(_id);" + strNewLine;
        strCode += strTab + "if (ds.Tables[0].Rows.Count > 0)" + strNewLine;
        strCode += strTab + strTab + "return ds.Tables[0].Rows[0][_column].ToString();" + strNewLine;
        strCode += strTab + "else" + strNewLine;
        strCode += strTab + strTab + "return \"\";" + strNewLine;
        strCode += "}" + strNewLine;
        
        txtCode.Text += strCode;
    }
    protected void AddGets()
    {
        // Stored Procedure
        string strSP = "";
        string strCode = "";
        string strParam = "";
        string strWhere = "";
        string strCodeDeclarations = "";
        string strCodeParams = "";
        int intParams = 0;
        if (boolDeleted == true)
            strWhere += strTab + "deleted = 0" + strNewLine;
        if (boolEnabled == true)
            strWhere += strTab + (strWhere != "" ? "AND " : "") + "enabled >= @enabled" + strNewLine;
        if (strParent != "")
        {
            string[] strParents = strParent.Split(strSplit);
            for (int ii = 0; ii < strParents.Length; ii++)
            {
                bool boolLast = ((ii + 1) == strParents.Length);
                strParam += strTab + "@" + strParents[ii] + " int" + (boolLast == false || boolEnabled == true ? "," : "") + strNewLine;
                strWhere += strTab + (strWhere != "" ? "AND " : "") + strParents[ii] + " = @" + strParents[ii] + strNewLine;
                if (strCodeDeclarations != "")
                    strCodeDeclarations += ", ";
                strCodeDeclarations += "int _" + strParents[ii];
                strCodeParams += strTab + "arParams[" + intParams.ToString() + "] = new SqlParameter(\"@" + strParents[ii] + "\", _" + strParents[ii] + ");" + strNewLine;
                intParams++;
            }
        }
        if (boolEnabled == true)
        {
            strParam += strTab + "@enabled int" + strNewLine;
            if (strCodeDeclarations != "")
                strCodeDeclarations += ", ";
            strCodeDeclarations += "int _enabled";
            strCodeParams += strTab + "arParams[" + intParams.ToString() + "] = new SqlParameter(\"@enabled\", _enabled);" + strNewLine;
            intParams++;
        }
        strSP += strPreface + " PROCEDURE [dbo].[pr_get" + strName + "s]" + strNewLine;
        strSP += strParam;
        strSP += "AS" + strNewLine;
        strSP += "SELECT" + strNewLine;
        strSP += strTab + "*" + strNewLine;
        strSP += "FROM" + strNewLine;
        strSP += strTab + strTable + strNewLine;
        if (strWhere != "")
        {
            strSP += "WHERE" + strNewLine;
            strSP += strWhere;
        }
        if (boolDisplay == true || strOrder != "")
        {
            strSP += "ORDER BY" + strNewLine;
            if (strOrder != "")
                strSP += strTab + strOrder + strNewLine;
            else
                strSP += strTab + "display" + strNewLine;
        }
        txtOutput.Text += strSP;

        strCode += "public DataSet Get" + strPrefix + "s(" + strCodeDeclarations + ")" + strNewLine;
        strCode += "{" + strNewLine;
        if (intParams > 0)
        {
            strCode += strTab + "arParams = new SqlParameter[" + intParams.ToString() + "];" + strNewLine;
            strCode += strCodeParams;
        }
        strCode += strTab + "return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, \"pr_get" + strName + "s\"" + (intParams > 0 ? ", arParams" : "") + ");" + strNewLine;
        strCode += "}" + strNewLine;
        txtCode.Text += strCode;
    }
    private string SQLtoC(string _type)
    {
        string strReturn = "???";
        switch (_type.ToUpper())
        {
            case "VARCHAR":
                strReturn = "string";
                break;
            default:
                strReturn = _type.ToLower();
                break;
        }
        return strReturn;
    }
</script>
<html>
<head id="Head1" runat="server">
<title id="Title1" runat="server">National City</title>
<link rel="stylesheet" type="text/css" href="/css/default.css" />
<script src="/javascript/default.js"type="text/javascript"></script>
<script type="text/javascript" src="/javascript/ajax.js"></script>
<script type="text/javascript" src="/javascript/both.js"></script>
</head>
<body leftmargin="0" topmargin="0">
<form id="Form1" runat="server">
        <table>
            <tr>
                <td colspan="3" class="header">Generate Stored Procedures</td>
            </tr>
            <tr>
                <td colspan="3">&nbsp;</td>
            </tr>
            <tr>
                <td valign="top">
                    <table>
                        <tr>
                            <td>Table:</td>
                            <td><asp:TextBox ID="txtTable" runat="server" CssClass="default" Width="300" /></td>
                        </tr>
                        <tr>
                            <td></td>
                            <td>&nbsp;&nbsp;&nbsp;<i>cv_servername_components_details</i></td>
                        </tr>
                        <tr>
                            <td>Naming:</td>
                            <td><asp:TextBox ID="txtNaming" runat="server" CssClass="default" Width="300" /></td>
                        </tr>
                        <tr>
                            <td></td>
                            <td>&nbsp;&nbsp;&nbsp;<i>ServerNameComponentDetail</i></td>
                        </tr>
                        <tr>
                            <td>Prefix:</td>
                            <td><asp:TextBox ID="txtPrefix" runat="server" CssClass="default" Width="300" /></td>
                        </tr>
                        <tr>
                            <td></td>
                            <td>&nbsp;&nbsp;&nbsp;<i>ComponentDetail</i></td>
                        </tr>
                        <tr>
                            <td>Order By (override):</td>
                            <td><asp:TextBox ID="txtOrder" runat="server" CssClass="default" Width="300" /></td>
                        </tr>
                        <tr>
                            <td>Parent Column(s):</td>
                            <td><asp:TextBox ID="txtParent" runat="server" CssClass="default" Width="300" /></td>
                        </tr>
                        <tr>
                            <td></td>
                            <td>&nbsp;&nbsp;&nbsp;<i>For multiple, separate with ;</i></td>
                        </tr>
                    </table>
                </td>
                <td rowspan="6">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
                <td valign="top">
                    <table>
                        <tr>
                            <td><asp:CheckBox ID="chkAlter" runat="server" CssClass="default" Text="Change CREATE to ALTER" /></td>
                        </tr>
                        <tr>
                            <td><asp:CheckBox ID="chkDisplay" runat="server" CssClass="default" Text="Include DISPLAY Parameter on ADD" /></td>
                        </tr>
                        <tr>
                            <td><asp:CheckBox ID="chkIdentity" runat="server" CssClass="default" Text="Include IDENTITY" /></td>
                        </tr>
                        <tr>
                            <td><asp:CheckBox ID="chkDisable" runat="server" CssClass="default" Text="No Enable" /></td>
                        </tr>
                        <tr>
                            <td><asp:CheckBox ID="chkVersion" runat="server" CssClass="default" Text="Only Add/Delete/Get" /></td>
                        </tr>
                        <tr>
                            <td></td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="2">&nbsp;</td>
            </tr>
            <tr>
                <td colspan="2"><asp:Button ID="btnGo" runat="server" CssClass="default" Width="75" Text="Go" OnClick="btnGo_Click" /></td>
            </tr>
            <tr>
                <td colspan="2">&nbsp;</td>
            </tr>
            <tr>
                <td>Stored Procedures:</td>
                <td>Class Code:</td>
            </tr>
            <tr>
                <td><asp:TextBox ID="txtOutput" runat="server" CssClass="default" Width="500" TextMode="MultiLine" Rows="50" AcceptsTab="true" /></td>
                <td><asp:TextBox ID="txtCode" runat="server" CssClass="default" Width="800" TextMode="MultiLine" Rows="50" AcceptsTab="true" /></td>
            </tr>
        </table>
</form>
</body>
</html>