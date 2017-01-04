using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using System.Text;
using System.IO;
using System.Collections.Generic;

namespace NCC.ClearView.Application.Core
{
    public enum MnemonicFeed
    {
        Name = 1,
        ResRating = 2,
        AppRating = 3,
        Status = 4,
        PM = 5,
        FM = 6,
        DM = 7,
        AppOwner = 8,
        ATL = 9,
        CIO = 10
    }
    public class Mnemonic
	{
		private string dsn = "";
		private int user = 0;
		private SqlParameter[] arParams;
        private StringBuilder strImport;
        public Mnemonic(int _user, string _dsn)
		{
			user = _user;
			dsn = _dsn;
		}
        public DataSet Gets(int _enabled)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getMnemonics", arParams);
        }
        public DataSet Get(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getMnemonic", arParams);
        }
        public DataSet Get(string _name)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@name", _name);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getMnemonicsAJAX", arParams);
        }
        public DataSet GetPending(string _name)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@name", _name);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getMnemonicsAJAXPending", arParams);
        }
        public DataSet Get(string _name, string _factory_code)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@name", _name);
            arParams[1] = new SqlParameter("@factory_code", _factory_code);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getMnemonicsImport", arParams);
        }
        public int GetResRatingHRs(int _id)
        {
            int intResRating = 0;
            if (_id > 0)
            {
                string strMnemonicCode = Get(_id, "factory_code");
                string strResRating = Get(_id, "ResRating");
                if (strResRating == "")
                    strResRating = GetFeed(strMnemonicCode, MnemonicFeed.ResRating);
                if (strResRating.ToUpper().Contains("HOURS") == true)
                    strResRating = strResRating.ToUpper().Replace("HOURS", "");
                strResRating = strResRating.Trim();
                if (strResRating.Contains(" ") == true)
                    strResRating = strResRating.Substring(strResRating.LastIndexOf(" "));
                strResRating = strResRating.Trim();
                Int32.TryParse(strResRating, out intResRating);
            }
            return intResRating;
        }
        public string Get(int _id, string _column)
        {
            DataSet ds = Get(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public int GetFeedUser(List<string> theFields, MnemonicFeed _column, int _else)
        {
            Users oUser = new Users(user, dsn);
            int intFeed = 0;
            string strFeed = GetFeed(theFields, _column);
            if (strFeed != "")
                intFeed = oUser.GetId(strFeed);
            else
                intFeed = _else;
            return intFeed;
        }
        public string GetFeedUser(List<string> theFields, MnemonicFeed _column, string _else)
        {
            Users oUser = new Users(user, dsn);
            string strFeed = GetFeed(theFields, _column);
            if (strFeed != "")
                strFeed = oUser.GetFullNameWithLanID(oUser.GetId(strFeed));
            else
                strFeed = _else;
            return strFeed;
        }
        public string GetFeedValue(List<string> theFields, MnemonicFeed _column, string _else)
        {
            string strFeed = GetFeed(theFields, _column);
            if (strFeed == "")
                strFeed = _else;
            return strFeed;
        }
        public string GetFeed(List<string> theFields, MnemonicFeed _column)
        {
            string strReturn = "";
            if (theFields.Count > 0)
            {
                switch (_column)
                {
                    case MnemonicFeed.Name:
                        strReturn = theFields[1];
                        break;
                    case MnemonicFeed.ResRating:
                        strReturn = theFields[2];
                        break;
                    case MnemonicFeed.AppRating:
                        strReturn = theFields[4];
                        break;
                    case MnemonicFeed.Status:
                        if (theFields.Count > 45)
                            strReturn = theFields[45];
                        break;
                    case MnemonicFeed.PM:
                        if (theFields.Count > 50)
                            strReturn = theFields[50];
                        break;
                    case MnemonicFeed.FM:
                        if (theFields.Count > 51)
                            strReturn = theFields[51];
                        break;
                    case MnemonicFeed.DM:
                        if (theFields.Count > 52)
                            strReturn = theFields[52];
                        break;
                    case MnemonicFeed.AppOwner:
                        if (theFields.Count > 53)
                            strReturn = theFields[53];
                        break;
                    case MnemonicFeed.ATL:
                        if (theFields.Count > 54)
                            strReturn = theFields[54];
                        break;
                    case MnemonicFeed.CIO:
                        if (theFields.Count > 55)
                            strReturn = theFields[55];
                        break;
                }
            }
            return strReturn;
        }
        public string GetFeed(string _code, MnemonicFeed _column)
        {
            return GetFeed(GetFeed(_code), _column);
        }
        public List<string> GetFeed(string _code)
        {
            List<string> strReturn = new List<string>();
            using (StreamReader theReader = new StreamReader(@"\\wsnww300a\outbound\mmscenters.txt"))
            {
                List<string> theLines = new List<string>(theReader.ReadToEnd().Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries));
                foreach (string theLine in theLines)
                {
                    List<string> theFields = new List<string>(theLine.Split(new char[] { '~' }, StringSplitOptions.None));
                    if (theFields[0].ToUpper() == _code.ToUpper())
                    {
                        strReturn = theFields;
                        break;
                    }
                }
            }
            return strReturn;
        }
        public void Add(string _name, string _factory_code, string _Status, string _ResRating, string _DRRating, string _Infrastructure, string _CriticalityFactor, string _Platform, string _CICS, string _PagerLevel, string _ATLName, string _PMName, string _FMName, string _DMName, string _CIO, string _AppOwner, string _AppLOBName, string _Segment1, string _RiskManager, string _BRContact, string _AppRating, string _Source, string _OriginalApp, int _enabled)
		{
            arParams = new SqlParameter[24];
            arParams[0] = new SqlParameter("@name", _name);
            arParams[1] = new SqlParameter("@factory_code", _factory_code);
            arParams[2] = new SqlParameter("@Status", _Status);
            arParams[3] = new SqlParameter("@ResRating", _ResRating);
            arParams[4] = new SqlParameter("@DRRating", _DRRating);
            arParams[5] = new SqlParameter("@Infrastructure", _Infrastructure);
            arParams[6] = new SqlParameter("@CriticalityFactor", _CriticalityFactor);
            arParams[7] = new SqlParameter("@Platform", _Platform);
            arParams[8] = new SqlParameter("@CICS", _CICS);
            arParams[9] = new SqlParameter("@PagerLevel", _PagerLevel);
            arParams[10] = new SqlParameter("@ATLName", _ATLName);
            arParams[11] = new SqlParameter("@PMName", _PMName);
            arParams[12] = new SqlParameter("@FMName", _FMName);
            arParams[13] = new SqlParameter("@DMName", _DMName);
            arParams[14] = new SqlParameter("@CIO", _CIO);
            arParams[15] = new SqlParameter("@AppOwner", _AppOwner);
            arParams[16] = new SqlParameter("@AppLOBName", _AppLOBName);
            arParams[17] = new SqlParameter("@Segment1", _Segment1);
            arParams[18] = new SqlParameter("@RiskManager", _RiskManager);
            arParams[19] = new SqlParameter("@BRContact", _BRContact);
            arParams[20] = new SqlParameter("@AppRating", _AppRating);
            arParams[21] = new SqlParameter("@Source", _Source);
            arParams[22] = new SqlParameter("@OriginalApp", _OriginalApp);
            arParams[23] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addMnemonic", arParams);
		}
        public void Update(int _id, string _name, string _factory_code, string _Status, string _ResRating, string _DRRating, string _Infrastructure, string _CriticalityFactor, string _Platform, string _CICS, string _PagerLevel, string _ATLName, string _PMName, string _FMName, string _DMName, string _CIO, string _AppOwner, string _AppLOBName, string _Segment1, string _RiskManager, string _BRContact, string _AppRating, string _Source, string _OriginalApp, int _enabled)
        {
            arParams = new SqlParameter[25];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@name", _name);
            arParams[2] = new SqlParameter("@factory_code", _factory_code);
            arParams[3] = new SqlParameter("@Status", _Status);
            arParams[4] = new SqlParameter("@ResRating", _ResRating);
            arParams[5] = new SqlParameter("@DRRating", _DRRating);
            arParams[6] = new SqlParameter("@Infrastructure", _Infrastructure);
            arParams[7] = new SqlParameter("@CriticalityFactor", _CriticalityFactor);
            arParams[8] = new SqlParameter("@Platform", _Platform);
            arParams[9] = new SqlParameter("@CICS", _CICS);
            arParams[10] = new SqlParameter("@PagerLevel", _PagerLevel);
            arParams[11] = new SqlParameter("@ATLName", _ATLName);
            arParams[12] = new SqlParameter("@PMName", _PMName);
            arParams[13] = new SqlParameter("@FMName", _FMName);
            arParams[14] = new SqlParameter("@DMName", _DMName);
            arParams[15] = new SqlParameter("@CIO", _CIO);
            arParams[16] = new SqlParameter("@AppOwner", _AppOwner);
            arParams[17] = new SqlParameter("@AppLOBName", _AppLOBName);
            arParams[18] = new SqlParameter("@Segment1", _Segment1);
            arParams[19] = new SqlParameter("@RiskManager", _RiskManager);
            arParams[20] = new SqlParameter("@BRContact", _BRContact);
            arParams[21] = new SqlParameter("@AppRating", _AppRating);
            arParams[22] = new SqlParameter("@Source", _Source);
            arParams[23] = new SqlParameter("@OriginalApp", _OriginalApp);
            arParams[24] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateMnemonic", arParams);
        }
        public void Enable(int _id, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateMnemonicEnabled", arParams);
        }
        public void Delete(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteMnemonic", arParams);
        }
        public DataSet GetRecent(int _new, string _filter)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@new", _new);
            arParams[1] = new SqlParameter("@filter", _filter);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getMnemonicsRecent", arParams);
        }

        public string Import(string _from_dsn, string _to_dsn, int _environment, bool _compare_and_show_existing_CV)
        {
            strImport = new StringBuilder();
            DataSet dsExist = SqlHelper.ExecuteDataset(_to_dsn, CommandType.Text, "SELECT * FROM cv_mnemonics WHERE deleted = 0");
            DataTable dtExist = dsExist.Tables[0];
            StringBuilder strResult = new StringBuilder();
            StreamReader theReader = new StreamReader(@"\\wsnww300a\outbound\mmscenters.txt");
            string[] theLines = theReader.ReadToEnd().Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            DataTable dtCurrent = new DataTable();
            dtCurrent.Columns.Add("Code", typeof(string));

            foreach (string theLine in theLines)
            {
                string[] theFields = theLine.Split(new char[] { '~' }, StringSplitOptions.None);
                string Code = theFields[0];
                if (Code != "")
                {
                    dtCurrent.Rows.Add(Code);
                    if (theFields.Length > 52)
                    {
                        string Name = theFields[1];
                        if (Name.Length > 200)
                            Name = Name.Substring(0, 200);
                        string ResRating = theFields[2];
                        if (ResRating.Length > 100)
                            ResRating = ResRating.Substring(0, 100);
                        string Status = theFields[45];
                        if (Status.Length > 100)
                            Status = Status.Substring(0, 100);
                        string PM = theFields[50];
                        if (PM.Length > 100)
                            PM = PM.Substring(0, 100);
                        string FM = theFields[51];
                        if (FM.Length > 100)
                            FM = FM.Substring(0, 100);
                        string DM = theFields[52];
                        if (DM.Length > 100)
                            DM = DM.Substring(0, 100);
                        string AppOwner = "";
                        if (theFields.Length > 53)
                            AppOwner = theFields[53];
                        if (AppOwner.Length > 100)
                            AppOwner = AppOwner.Substring(0, 100);
                        string ATL = "";
                        if (theFields.Length > 54)
                            ATL = theFields[54];
                        if (ATL.Length > 100)
                            ATL = ATL.Substring(0, 100);
                        string CIO = "";
                        if (theFields.Length > 55)
                            CIO = theFields[55];
                        if (CIO.Length > 100)
                            CIO = CIO.Substring(0, 100);

                        DataRow[] drExists = dtExist.Select("factory_code = '" + Code + "'");
                        if (drExists.Length == 0)
                        {
                            // Add Mnemonic
                            ImportLog("ADD [" + Code + "]");
                            Add(Name,
                                Code,
                                Status,
                                ResRating,
                                "",
                                "",
                                "",
                                "",
                                "",
                                "",
                                ATL,
                                PM,
                                FM,
                                DM,
                                CIO,
                                AppOwner,
                                "",
                                "",
                                "",
                                "",
                                "",
                                "",
                                "",
                                1);
                        }
                        else
                        {
                            // Update Mnemonic
                            DataRow drExist = drExists[0];
                            int intID = Int32.Parse(drExist["id"].ToString());
                            try
                            {
                                if (drExist["status"].ToString() != Status)
                                {
                                    ImportLog("...UPDATE [" + Code + "] : MODIFIED = " + DateTime.Now);
                                    SqlHelper.ExecuteNonQuery(_to_dsn, CommandType.Text, "UPDATE cv_mnemonics SET modified = getdate() WHERE id = " + intID.ToString());
                                    UpdateMnemonicLog(intID, _to_dsn, Code, "status", Status);
                                }
                                if (drExist["name"].ToString() != Name)
                                    UpdateMnemonicLog(intID, _to_dsn, Code, "name", Name);
                                if (drExist["ResRating"].ToString() != ResRating)
                                    UpdateMnemonicLog(intID, _to_dsn, Code, "ResRating", ResRating);
                                if (drExist["ATLName"].ToString() != ATL)
                                    UpdateMnemonicLog(intID, _to_dsn, Code, "ATLName", ATL);
                                if (drExist["PMName"].ToString() != PM)
                                    UpdateMnemonicLog(intID, _to_dsn, Code, "PMName", PM);
                                if (drExist["FMName"].ToString() != FM)
                                    UpdateMnemonicLog(intID, _to_dsn, Code, "FMName", FM);
                                if (drExist["DMName"].ToString() != DM)
                                    UpdateMnemonicLog(intID, _to_dsn, Code, "DMName", DM);
                                if (drExist["CIO"].ToString() != CIO)
                                    UpdateMnemonicLog(intID, _to_dsn, Code, "CIO", CIO);
                                if (drExist["AppOwner"].ToString() != AppOwner)
                                    UpdateMnemonicLog(intID, _to_dsn, Code, "AppOwner", AppOwner);
                            }
                            catch (Exception ex)
                            {
                                ImportLog("...FAILED TO UPDATE!!! [" + Code + "] : ERROR = " + ex.Message);
                            }
                        }
                    }
                }
            }
            theReader.Close();

            if (_compare_and_show_existing_CV == true)
            {
                // Check the ones that exist in ClearView but not in imported table (and display warnings for manual cleanup...if applicable)
                DataSet dsClearView = SqlHelper.ExecuteDataset(_to_dsn, CommandType.Text, "SELECT * FROM cv_mnemonics WHERE deleted = 0");
                foreach (DataRow drClearView in dsClearView.Tables[0].Rows)
                {
                    string strCode = drClearView["factory_code"].ToString().Trim().ToUpper();
                    DataRow[] drImports = dtCurrent.Select("Code = '" + strCode + "'");
                    if (drImports.Length == 0)
                        ImportLog("......WARNING [" + strCode + "] : Does not exist in mnemonic feed");
                }
            }
            if (strImport.ToString() != "")
            {
                AddImport(strImport.ToString());
                return strImport.ToString();
            }
            else
                return "No changes...";
        }
        private void UpdateMnemonicLog(int _id, string _dsn, string _code, string _field, string _value)
        {
            ImportLog("......UPDATE [" + _code + "] : " + _field + " = " + _value);
            SqlParameter[] arImport = new SqlParameter[1];
            arImport[0] = new SqlParameter("@" + _field, _value);
            SqlHelper.ExecuteNonQuery(_dsn, CommandType.Text, "UPDATE cv_mnemonics SET " + _field + " = @" + _field + " WHERE id = " + _id.ToString(), arImport);
        }
        private void ImportLog(string _result)
        {
            if (strImport.ToString() != "")
                strImport.Append(Environment.NewLine);
            strImport.Append(_result);
        }

        public DataSet GetImports()
        {
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getMnemonicImports");
        }
        public DataSet GetImport(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getMnemonicImport", arParams);
        }
        public string GetImport(int _id, string _column)
        {
            DataSet ds = GetImport(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public void AddImport(string _results)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@results", _results);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addMnemonicImport", arParams);
        }
    }
}
