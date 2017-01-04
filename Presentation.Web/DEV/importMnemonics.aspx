<%@ Page Language="C#" Debug="false" %>
<%@ Import Namespace="System.Data.OleDb" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System" %>
<%@ Import Namespace="System.Collections.Generic" %>
<script runat="server">
    protected class SS_Mnemonic
    {
        private string code;
        private string field;
        public string Code
        {
            get
            {
                return code;
            }
            set
            {
                code = value;
            }
        }
        public string Field
        {
            get
            {
                return field;
            }
            set
            {
                field = value;
            }
        }
        public SS_Mnemonic(string _code, string _field)
        {
            Code = _code;
            Field = _field;
        }
    }

    private int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
    private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
    private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
    private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
    private int intCount1 = 0;
    private int intCount2 = 0;

    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btnGo_Click(object sender, EventArgs e)
    {
        List<SS_Mnemonic> lstMnemonics = new List<SS_Mnemonic>();

        Mnemonic oMnemonic = new Mnemonic(0, dsn);
        Forecast oForecast = new Forecast(0, dsn);
        Users oUser = new Users(0, dsn);
        //Servers oServer = new Servers(0, dsn);

        StringBuilder strResult = new StringBuilder();
        StreamReader theReader = new StreamReader(@"\\wsnww300a\outbound\mmscenters.txt");
        string[] theLines = theReader.ReadToEnd().Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
        //Response.Write(theLines.Length + "<br/>");

        foreach (string theLine in theLines)
        {
            string[] theFields = theLine.Split(new char[] { '~' }, StringSplitOptions.None);
            string Code = theFields[0];
            if (Code != "")
            {
                string Name = theFields[1];
                string ResRating = theFields[2];
                string Status = theFields[45];
                string PM = theFields[50];
                string FM = theFields[51];
                string DM = theFields[52];
                string AppOwner = "";
                if (theFields.Length > 53)
                    AppOwner = theFields[53];
                string ATL = "";
                if (theFields.Length > 54)
                    ATL = theFields[54];
                string CIO = "";
                if (theFields.Length > 55)
                    CIO = theFields[55];

                /*
                if (Code == "CLV")
                {
                    Response.Write("Code = " + Code + "<br/>");
                    Response.Write("Name = " + Name + "<br/>");
                    Response.Write("ResRating = " + ResRating + "<br/>");
                    Response.Write("Status = " + Status + "<br/>");
                    Response.Write("PM = " + PM + "<br/>");                     // Application System Manager
                        Response.Write("FM = " + FM + "<br/>");                     // System Director
                    Response.Write("DM = " + DM + "<br/>");                     // Department Manager
                    Response.Write("AppOwner = " + AppOwner + "<br/>");         // Application Owner
                    Response.Write("ATL = " + ATL + "<br/>");                   // Application Technical Lead
                        Response.Write("CIO = " + CIO + "<br/>");                   // Chief Information Officer
                    break;
                }
                */

                int intMnemonic = GetMnemonic(Code);
                if (intMnemonic > 0)
                {
                    DataSet dsA = SqlHelper.ExecuteDataset(dsn, CommandType.Text, "SELECT * FROM cv_forecast_answers WHERE mnemonicid = " + intMnemonic.ToString() + " AND deleted = 0");
                    foreach (DataRow drA in dsA.Tables[0].Rows)
                    {
                        int intID = Int32.Parse(drA["id"].ToString());

                        bool boolDM = InList(lstMnemonics, Code, "appcontact");
                        if (boolDM == false)
                        {
                            lstMnemonics.Add(new SS_Mnemonic(Code, "appcontact"));
                            int intDM_N = GetUser(DM);
                            int intDM_O = 0;
                            Int32.TryParse(drA["appcontact"].ToString(), out intDM_O);
                            if (intDM_N > 0 && intDM_N != intDM_O)
                            {
                                strResult.AppendLine(Code + " - updating Department Manager (appcontact) ... " + (intDM_O > 0 ? oUser.GetFullName(intDM_O) + " (" + oUser.GetName(intDM_O) + ")" : "NULL") + " ... to ... " + oUser.GetFullName(intDM_N) + " (" + oUser.GetName(intDM_N) + ")");
                                if (chkUpdate.Checked)
                                    SqlHelper.ExecuteNonQuery(dsn, CommandType.Text, "UPDATE cv_forecast_answers SET appcontact = " + intDM_N.ToString() + " WHERE id = " + intID.ToString());
                            }
                        }

                        bool boolATL = InList(lstMnemonics, Code, "admin1");
                        if (boolATL == false)
                        {
                            lstMnemonics.Add(new SS_Mnemonic(Code, "admin1"));
                            int intATL_N = GetUser(ATL);
                            int intATL_O = 0;
                            Int32.TryParse(drA["admin1"].ToString(), out intATL_O);
                            if (intATL_N > 0 && intATL_N != intATL_O)
                            {
                                strResult.AppendLine(Code + " - updating Application Technical Lead (admin1) ... " + (intATL_O > 0 ? oUser.GetFullName(intATL_O) + " (" + oUser.GetName(intATL_O) + ")" : "NULL") + " ... to ... " + oUser.GetFullName(intATL_N) + " (" + oUser.GetName(intATL_N) + ")");
                                if (chkUpdate.Checked)
                                    SqlHelper.ExecuteNonQuery(dsn, CommandType.Text, "UPDATE cv_forecast_answers SET admin1 = " + intATL_N.ToString() + " WHERE id = " + intID.ToString());
                            }
                        }

                        bool boolAppOwner = InList(lstMnemonics, Code, "appowner");
                        if (boolAppOwner == false)
                        {
                            lstMnemonics.Add(new SS_Mnemonic(Code, "appowner"));
                            int intAppOwner_N = GetUser(AppOwner);
                            int intAppOwner_O = 0;
                            Int32.TryParse(drA["appowner"].ToString(), out intAppOwner_O);
                            if (intAppOwner_N > 0 && intAppOwner_N != intAppOwner_O)
                            {
                                strResult.AppendLine(Code + " - updating Application Owner (appowner) ... " + (intAppOwner_O > 0 ? oUser.GetFullName(intAppOwner_O) + " (" + oUser.GetName(intAppOwner_O) + ")" : "NULL") + " ... to ... " + oUser.GetFullName(intAppOwner_N) + " (" + oUser.GetName(intAppOwner_N) + ")");
                                if (chkUpdate.Checked)
                                    SqlHelper.ExecuteNonQuery(dsn, CommandType.Text, "UPDATE cv_forecast_answers SET appowner = " + intAppOwner_N.ToString() + " WHERE id = " + intID.ToString());
                            }
                        }

                        bool boolPM = InList(lstMnemonics, Code, "admin2");
                        if (boolPM == false)
                        {
                            lstMnemonics.Add(new SS_Mnemonic(Code, "admin2"));
                            int intPM_N = GetUser(PM);
                            int intPM_O = 0;
                            Int32.TryParse(drA["admin2"].ToString(), out intPM_O);
                            if (intPM_N > 0 && intPM_N != intPM_O)
                            {
                                strResult.AppendLine(Code + " - updating Application System Manager (admin2) ... " + (intPM_O > 0 ? oUser.GetFullName(intPM_O) + " (" + oUser.GetName(intPM_O) + ")" : "NULL") + " ... to ... " + oUser.GetFullName(intPM_N) + " (" + oUser.GetName(intPM_N) + ")");
                                if (chkUpdate.Checked)
                                    SqlHelper.ExecuteNonQuery(dsn, CommandType.Text, "UPDATE cv_forecast_answers SET admin2 = " + intPM_N.ToString() + " WHERE id = " + intID.ToString());
                            }
                        }

                        if (chkOne.Checked && strResult.ToString() != "")
                            break;

                        if (boolDM && boolATL && boolAppOwner && boolPM)
                            break;
                    }
                    if (chkOne.Checked && strResult.ToString() != "")
                        break;
                }
            }
        }

        theReader.Close();

        if (chkUpdate.Checked)
            oMnemonic.AddImport(strResult.ToString());

        Response.Write(strResult.ToString().Replace(Environment.NewLine, "<br/>"));
    }
    private int GetMnemonic(string _factory_code)
    {
        DataSet ds = SqlHelper.ExecuteDataset(dsn, CommandType.Text, "SELECT * FROM cv_mnemonics WHERE factory_code = '" + _factory_code + "' AND enabled = 1 AND deleted = 0");
        if (ds.Tables[0].Rows.Count == 0)
            return 0;
        else
            return Int32.Parse(ds.Tables[0].Rows[0]["id"].ToString());
    }
    private int GetUser(string _pnc_id)
    {
        if (_pnc_id == "")
            return 0;
        else
        {
            DataSet ds = SqlHelper.ExecuteDataset(dsn, CommandType.Text, "SELECT * FROM cv_users WHERE pnc_id = '" + _pnc_id + "' AND enabled = 1 AND deleted = 0");
            if (ds.Tables[0].Rows.Count == 0)
                return 0;
            else
                return Int32.Parse(ds.Tables[0].Rows[0]["userid"].ToString());
        }
    }
    private bool InList(List<SS_Mnemonic> _list, string _code, string _field)
    {
        return false;

        bool boolFound = false;
        foreach (SS_Mnemonic _item in _list)
        {
            if (_code == _item.Code && _field == _item.Field)
            {
                boolFound = true;
                break;
            }
        }
        return boolFound;
    }
 </script>
<script type="text/javascript">
</script>
<html>
<head>
<title>IMPORT MNEMONICS</title>
<link rel="stylesheet" type="text/css" href="/css/default.css" />
</head>
<body leftmargin="0" topmargin="0">
<form id="Form1" runat="server">
    <table>
        <tr>
            <td colspan="2" class="header">Import</td>
        </tr>
        <tr>
            <td colspan="2"><asp:CheckBox ID="chkOne" runat="server" Text="Only One" /></td>
        </tr>
        <tr>
            <td colspan="2"><asp:CheckBox ID="chkUpdate" runat="server" Text="Update Contacts" /></td>
        </tr>
        <tr>
            <td colspan="2">&nbsp;</td>
        </tr>
        <tr>
            <td>&nbsp;</td>
            <td><asp:Button ID="btnGo" runat="server" CssClass="default" Width="150" Text="Import Mnemonics" OnClick="btnGo_Click" /></td>
        </tr>
    </table>
</form>
</body>
</html>