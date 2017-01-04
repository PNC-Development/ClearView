using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using System.Text;

namespace NCC.ClearView.Application.Core
{
	public class TSM
	{
		private string dsn = "";
		private int user = 0;
		private SqlParameter[] arParams;
        public TSM(int _user, string _dsn)
		{
			user = _user;
			dsn = _dsn;
		}
        public DataSet Gets(int _tsm, int _avamar, int _enabled)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@tsm", _tsm);
            arParams[1] = new SqlParameter("@avamar", _avamar);
            arParams[2] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getTSMsTypes", arParams);
        }
        public DataSet Gets(int _enabled)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getTSMs", arParams);
        }
        public DataSet Gets(int _addressid, int _tsm, int _avamar, bool _engineering, bool _test, bool _qa, bool _prod, bool _windows, bool _unix, int _resiliencyid, int _serverid)
        {
            arParams = new SqlParameter[11];
            arParams[0] = new SqlParameter("@addressid", _addressid);
            arParams[1] = new SqlParameter("@tsm", _tsm);
            arParams[2] = new SqlParameter("@avamar", _avamar);
            arParams[3] = new SqlParameter("@engineering", (_engineering ? 1 : 0));
            arParams[4] = new SqlParameter("@test", (_test ? 1 : 0));
            arParams[5] = new SqlParameter("@qa", (_qa ? 1 : 0));
            arParams[6] = new SqlParameter("@prod", (_prod ? 1 : 0));
            arParams[7] = new SqlParameter("@windows", (_windows ? 1 : 0));
            arParams[8] = new SqlParameter("@unix", (_unix ? 1 : 0));
            arParams[9] = new SqlParameter("@resiliencyid", _resiliencyid);
            arParams[10] = new SqlParameter("@serverid", _serverid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getTSMsAutomated", arParams);
        }
        
        public DataSet Get(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getTSM", arParams);
        }
        public string Get(int _id, string _column)
        {
            DataSet ds = Get(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public void Add(string _name, int _port, int _addressid, string _path, int _tsm, int _avamar, int _display, int _enabled)
        {
            arParams = new SqlParameter[8];
            arParams[0] = new SqlParameter("@name", _name);
            arParams[1] = new SqlParameter("@port", _port);
            arParams[2] = new SqlParameter("@addressid", _addressid);
            arParams[3] = new SqlParameter("@path", _path);
            arParams[4] = new SqlParameter("@tsm", _tsm);
            arParams[5] = new SqlParameter("@avamar", _avamar);
            arParams[6] = new SqlParameter("@display", _display);
            arParams[7] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addTSM", arParams);
        }
        public void Update(int _id, string _name, int _port, int _addressid, string _path, int _tsm, int _avamar, int _enabled)
        {
            arParams = new SqlParameter[8];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@name", _name);
            arParams[2] = new SqlParameter("@port", _port);
            arParams[3] = new SqlParameter("@addressid", _addressid);
            arParams[4] = new SqlParameter("@path", _path);
            arParams[5] = new SqlParameter("@tsm", _tsm);
            arParams[6] = new SqlParameter("@avamar", _avamar);
            arParams[7] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateTSM", arParams);
        }
        public void UpdateOrder(int _id, int _display)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@display", _display);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateTSMOrder", arParams);
        }
        public void Enable(int _id, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateTSMEnabled", arParams);
        }
        public void Delete(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteTSM", arParams);
        }

        public DataSet GetCloptsets(int _enabled)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getTSMCloptsets", arParams);
        }
        public DataSet GetCloptset(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getTSMCloptset", arParams);
        }
        public string GetCloptset(int _id, string _column)
        {
            DataSet ds = GetCloptset(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public void AddCloptset(string _name, int _display, int _enabled)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@name", _name);
            arParams[1] = new SqlParameter("@display", _display);
            arParams[2] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addTSMCloptset", arParams);
        }
        public void UpdateCloptset(int _id, string _name, int _enabled)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@name", _name);
            arParams[2] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateTSMCloptset", arParams);
        }
        public void UpdateCloptsetOrder(int _id, int _display)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@display", _display);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateTSMCloptsetOrder", arParams);
        }
        public void EnableCloptset(int _id, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateTSMCloptsetEnabled", arParams);
        }
        public void DeleteCloptset(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteTSMCloptset", arParams);
        }



        public DataSet GetDomains(int _tsm, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@tsm", _tsm);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getTSMDomains", arParams);
        }
        public DataSet GetDomain(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getTSMDomain", arParams);
        }
        public string GetDomain(int _id, string _column)
        {
            DataSet ds = GetDomain(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public void AddDomain(int _tsm, string _name, int _engineering, int _test, int _qa, int _prod, int _windows, int _unix, int _resiliencyid, int _display, int _enabled)
        {
            arParams = new SqlParameter[11];
            arParams[0] = new SqlParameter("@tsm", _tsm);
            arParams[1] = new SqlParameter("@name", _name);
            arParams[2] = new SqlParameter("@engineering", _engineering);
            arParams[3] = new SqlParameter("@test", _test);
            arParams[4] = new SqlParameter("@qa", _qa);
            arParams[5] = new SqlParameter("@prod", _prod);
            arParams[6] = new SqlParameter("@windows", _windows);
            arParams[7] = new SqlParameter("@unix", _unix);
            arParams[8] = new SqlParameter("@resiliencyid", _resiliencyid);
            arParams[9] = new SqlParameter("@display", _display);
            arParams[10] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addTSMDomain", arParams);
        }
        public void UpdateDomain(int _id, int _tsm, string _name, int _engineering, int _test, int _qa, int _prod, int _windows, int _unix, int _resiliencyid, int _enabled)
        {
            arParams = new SqlParameter[11];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@tsm", _tsm);
            arParams[2] = new SqlParameter("@name", _name);
            arParams[3] = new SqlParameter("@engineering", _engineering);
            arParams[4] = new SqlParameter("@test", _test);
            arParams[5] = new SqlParameter("@qa", _qa);
            arParams[6] = new SqlParameter("@prod", _prod);
            arParams[7] = new SqlParameter("@windows", _windows);
            arParams[8] = new SqlParameter("@unix", _unix);
            arParams[9] = new SqlParameter("@resiliencyid", _resiliencyid);
            arParams[10] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateTSMDomain", arParams);
        }
        public void UpdateDomainOrder(int _id, int _display)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@display", _display);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateTSMDomainOrder", arParams);
        }
        public void EnableDomain(int _id, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateTSMDomainEnabled", arParams);
        }
        public void DeleteDomain(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteTSMDomain", arParams);
        }

        public DataSet GetMnemonics(int _tsm, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@tsm", _tsm);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getTSMMnemonics", arParams);
        }
        public DataSet GetMnemonic(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getTSMMnemonic", arParams);
        }
        public string GetMnemonic(int _id, string _column)
        {
            DataSet ds = GetMnemonic(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public void AddMnemonic(int _tsm, int _mnemonicid, int _enabled)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@tsm", _tsm);
            arParams[1] = new SqlParameter("@mnemonicid", _mnemonicid);
            arParams[2] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addTSMMnemonic", arParams);
        }
        public void UpdateMnemonic(int _id, int _tsm, int _mnemonicid, int _enabled)
        {
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@tsm", _tsm);
            arParams[2] = new SqlParameter("@mnemonicid", _mnemonicid);
            arParams[3] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateTSMMnemonic", arParams);
        }
        public void DeleteMnemonic(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteTSMMnemonic", arParams);
        }

        public DataSet GetSchedules(int _domain, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@domain", _domain);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getTSMSchedules", arParams);
        }
        public DataSet GetSchedule(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getTSMSchedule", arParams);
        }
        public string GetSchedule(int _id, string _column)
        {
            DataSet ds = GetSchedule(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        /*
        public void AddSchedule(int _domain, string _name, int _display, int _enabled)
        {
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@domain", _domain);
            arParams[1] = new SqlParameter("@name", _name);
            arParams[2] = new SqlParameter("@display", _display);
            arParams[3] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addTSMSchedule", arParams);
        }
        public void UpdateSchedule(int _id, int _domain, string _name, int _enabled)
        {
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@domain", _domain);
            arParams[2] = new SqlParameter("@name", _name);
            arParams[3] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateTSMSchedule", arParams);
        }
        */
        public void AddSchedule(int _domain, string _name, int _engineering, int _test, int _qa, int _prod, int _windows, int _unix, int _daily, int _weekly, int _resiliencyid, int _sunday, int _monday, int _tuesday, int _wednesday, int _thursday, int _friday, int _saturday, int _monthly, int _AM1200, int _AM1230, int _AM100, int _AM130, int _AM200, int _AM230, int _AM300, int _AM330, int _AM400, int _AM430, int _AM500, int _AM530, int _AM600, int _AM630, int _AM700, int _AM730, int _AM800, int _AM830, int _AM900, int _AM930, int _AM1000, int _AM1030, int _AM1100, int _AM1130, int _PM1200, int _PM1230, int _PM100, int _PM130, int _PM200, int _PM230, int _PM300, int _PM330, int _PM400, int _PM430, int _PM500, int _PM530, int _PM600, int _PM630, int _PM700, int _PM730, int _PM800, int _PM830, int _PM900, int _PM930, int _PM1000, int _PM1030, int _PM1100, int _PM1130, int _display, int _enabled)
        {
            arParams = new SqlParameter[69];
            arParams[0] = new SqlParameter("@domain", _domain);
            arParams[1] = new SqlParameter("@name", _name);
            arParams[2] = new SqlParameter("@engineering", _engineering);
            arParams[3] = new SqlParameter("@test", _test);
            arParams[4] = new SqlParameter("@qa", _qa);
            arParams[5] = new SqlParameter("@prod", _prod);
            arParams[6] = new SqlParameter("@windows", _windows);
            arParams[7] = new SqlParameter("@unix", _unix);
            arParams[8] = new SqlParameter("@daily", _daily);
            arParams[9] = new SqlParameter("@weekly", _weekly);
            arParams[10] = new SqlParameter("@resiliencyid", _resiliencyid);
            arParams[11] = new SqlParameter("@sunday", _sunday);
            arParams[12] = new SqlParameter("@monday", _monday);
            arParams[13] = new SqlParameter("@tuesday", _tuesday);
            arParams[14] = new SqlParameter("@wednesday", _wednesday);
            arParams[15] = new SqlParameter("@thursday", _thursday);
            arParams[16] = new SqlParameter("@friday", _friday);
            arParams[17] = new SqlParameter("@saturday", _saturday);
            arParams[18] = new SqlParameter("@monthly", _monthly);
            arParams[19] = new SqlParameter("@AM1200", _AM1200);
            arParams[20] = new SqlParameter("@AM1230", _AM1230);
            arParams[21] = new SqlParameter("@AM100", _AM100);
            arParams[22] = new SqlParameter("@AM130", _AM130);
            arParams[23] = new SqlParameter("@AM200", _AM200);
            arParams[24] = new SqlParameter("@AM230", _AM230);
            arParams[25] = new SqlParameter("@AM300", _AM300);
            arParams[26] = new SqlParameter("@AM330", _AM330);
            arParams[27] = new SqlParameter("@AM400", _AM400);
            arParams[28] = new SqlParameter("@AM430", _AM430);
            arParams[29] = new SqlParameter("@AM500", _AM500);
            arParams[30] = new SqlParameter("@AM530", _AM530);
            arParams[31] = new SqlParameter("@AM600", _AM600);
            arParams[32] = new SqlParameter("@AM630", _AM630);
            arParams[33] = new SqlParameter("@AM700", _AM700);
            arParams[34] = new SqlParameter("@AM730", _AM730);
            arParams[35] = new SqlParameter("@AM800", _AM800);
            arParams[36] = new SqlParameter("@AM830", _AM830);
            arParams[37] = new SqlParameter("@AM900", _AM900);
            arParams[38] = new SqlParameter("@AM930", _AM930);
            arParams[39] = new SqlParameter("@AM1000", _AM1000);
            arParams[40] = new SqlParameter("@AM1030", _AM1030);
            arParams[41] = new SqlParameter("@AM1100", _AM1100);
            arParams[42] = new SqlParameter("@AM1130", _AM1130);
            arParams[43] = new SqlParameter("@PM1200", _PM1200);
            arParams[44] = new SqlParameter("@PM1230", _PM1230);
            arParams[45] = new SqlParameter("@PM100", _PM100);
            arParams[46] = new SqlParameter("@PM130", _PM130);
            arParams[47] = new SqlParameter("@PM200", _PM200);
            arParams[48] = new SqlParameter("@PM230", _PM230);
            arParams[49] = new SqlParameter("@PM300", _PM300);
            arParams[50] = new SqlParameter("@PM330", _PM330);
            arParams[51] = new SqlParameter("@PM400", _PM400);
            arParams[52] = new SqlParameter("@PM430", _PM430);
            arParams[53] = new SqlParameter("@PM500", _PM500);
            arParams[54] = new SqlParameter("@PM530", _PM530);
            arParams[55] = new SqlParameter("@PM600", _PM600);
            arParams[56] = new SqlParameter("@PM630", _PM630);
            arParams[57] = new SqlParameter("@PM700", _PM700);
            arParams[58] = new SqlParameter("@PM730", _PM730);
            arParams[59] = new SqlParameter("@PM800", _PM800);
            arParams[60] = new SqlParameter("@PM830", _PM830);
            arParams[61] = new SqlParameter("@PM900", _PM900);
            arParams[62] = new SqlParameter("@PM930", _PM930);
            arParams[63] = new SqlParameter("@PM1000", _PM1000);
            arParams[64] = new SqlParameter("@PM1030", _PM1030);
            arParams[65] = new SqlParameter("@PM1100", _PM1100);
            arParams[66] = new SqlParameter("@PM1130", _PM1130);
            arParams[67] = new SqlParameter("@display", _display);
            arParams[68] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addTSMSchedule", arParams);
        }
        public void UpdateSchedule(int _id, int _domain, string _name, int _engineering, int _test, int _qa, int _prod, int _windows, int _unix, int _daily, int _weekly, int _resiliencyid, int _sunday, int _monday, int _tuesday, int _wednesday, int _thursday, int _friday, int _saturday, int _monthly, int _AM1200, int _AM1230, int _AM100, int _AM130, int _AM200, int _AM230, int _AM300, int _AM330, int _AM400, int _AM430, int _AM500, int _AM530, int _AM600, int _AM630, int _AM700, int _AM730, int _AM800, int _AM830, int _AM900, int _AM930, int _AM1000, int _AM1030, int _AM1100, int _AM1130, int _PM1200, int _PM1230, int _PM100, int _PM130, int _PM200, int _PM230, int _PM300, int _PM330, int _PM400, int _PM430, int _PM500, int _PM530, int _PM600, int _PM630, int _PM700, int _PM730, int _PM800, int _PM830, int _PM900, int _PM930, int _PM1000, int _PM1030, int _PM1100, int _PM1130, int _enabled)
        {
            arParams = new SqlParameter[69];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@domain", _domain);
            arParams[2] = new SqlParameter("@name", _name);
            arParams[3] = new SqlParameter("@engineering", _engineering);
            arParams[4] = new SqlParameter("@test", _test);
            arParams[5] = new SqlParameter("@qa", _qa);
            arParams[6] = new SqlParameter("@prod", _prod);
            arParams[7] = new SqlParameter("@windows", _windows);
            arParams[8] = new SqlParameter("@unix", _unix);
            arParams[9] = new SqlParameter("@daily", _daily);
            arParams[10] = new SqlParameter("@weekly", _weekly);
            arParams[11] = new SqlParameter("@resiliencyid", _resiliencyid);
            arParams[12] = new SqlParameter("@sunday", _sunday);
            arParams[13] = new SqlParameter("@monday", _monday);
            arParams[14] = new SqlParameter("@tuesday", _tuesday);
            arParams[15] = new SqlParameter("@wednesday", _wednesday);
            arParams[16] = new SqlParameter("@thursday", _thursday);
            arParams[17] = new SqlParameter("@friday", _friday);
            arParams[18] = new SqlParameter("@saturday", _saturday);
            arParams[19] = new SqlParameter("@monthly", _monthly);
            arParams[20] = new SqlParameter("@AM1200", _AM1200);
            arParams[21] = new SqlParameter("@AM1230", _AM1230);
            arParams[22] = new SqlParameter("@AM100", _AM100);
            arParams[23] = new SqlParameter("@AM130", _AM130);
            arParams[24] = new SqlParameter("@AM200", _AM200);
            arParams[25] = new SqlParameter("@AM230", _AM230);
            arParams[26] = new SqlParameter("@AM300", _AM300);
            arParams[27] = new SqlParameter("@AM330", _AM330);
            arParams[28] = new SqlParameter("@AM400", _AM400);
            arParams[29] = new SqlParameter("@AM430", _AM430);
            arParams[30] = new SqlParameter("@AM500", _AM500);
            arParams[31] = new SqlParameter("@AM530", _AM530);
            arParams[32] = new SqlParameter("@AM600", _AM600);
            arParams[33] = new SqlParameter("@AM630", _AM630);
            arParams[34] = new SqlParameter("@AM700", _AM700);
            arParams[35] = new SqlParameter("@AM730", _AM730);
            arParams[36] = new SqlParameter("@AM800", _AM800);
            arParams[37] = new SqlParameter("@AM830", _AM830);
            arParams[38] = new SqlParameter("@AM900", _AM900);
            arParams[39] = new SqlParameter("@AM930", _AM930);
            arParams[40] = new SqlParameter("@AM1000", _AM1000);
            arParams[41] = new SqlParameter("@AM1030", _AM1030);
            arParams[42] = new SqlParameter("@AM1100", _AM1100);
            arParams[43] = new SqlParameter("@AM1130", _AM1130);
            arParams[44] = new SqlParameter("@PM1200", _PM1200);
            arParams[45] = new SqlParameter("@PM1230", _PM1230);
            arParams[46] = new SqlParameter("@PM100", _PM100);
            arParams[47] = new SqlParameter("@PM130", _PM130);
            arParams[48] = new SqlParameter("@PM200", _PM200);
            arParams[49] = new SqlParameter("@PM230", _PM230);
            arParams[50] = new SqlParameter("@PM300", _PM300);
            arParams[51] = new SqlParameter("@PM330", _PM330);
            arParams[52] = new SqlParameter("@PM400", _PM400);
            arParams[53] = new SqlParameter("@PM430", _PM430);
            arParams[54] = new SqlParameter("@PM500", _PM500);
            arParams[55] = new SqlParameter("@PM530", _PM530);
            arParams[56] = new SqlParameter("@PM600", _PM600);
            arParams[57] = new SqlParameter("@PM630", _PM630);
            arParams[58] = new SqlParameter("@PM700", _PM700);
            arParams[59] = new SqlParameter("@PM730", _PM730);
            arParams[60] = new SqlParameter("@PM800", _PM800);
            arParams[61] = new SqlParameter("@PM830", _PM830);
            arParams[62] = new SqlParameter("@PM900", _PM900);
            arParams[63] = new SqlParameter("@PM930", _PM930);
            arParams[64] = new SqlParameter("@PM1000", _PM1000);
            arParams[65] = new SqlParameter("@PM1030", _PM1030);
            arParams[66] = new SqlParameter("@PM1100", _PM1100);
            arParams[67] = new SqlParameter("@PM1130", _PM1130);
            arParams[68] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateTSMSchedule", arParams);
        }
        public void UpdateScheduleOrder(int _id, int _display)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@display", _display);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateTSMScheduleOrder", arParams);
        }
        public void EnableSchedule(int _id, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateTSMScheduleEnabled", arParams);
        }
        public void DeleteSchedule(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteTSMSchedule", arParams);
        }

        public string LoadDDL(string _server_id, string _domain_id, string _schedule_id, string _hidden_id, int _current, int _tsm, int _avamar, string _result)
        {
            // SCHEDULE
            int intDomain = 0;
            if (_current > 0)
                intDomain = Int32.Parse(GetSchedule(_current, "domain"));
            string strSchedule = "<td>";
            if (_result == "")
            {
                strSchedule += "<select name=\"" + _schedule_id + "\" id=\"" + _schedule_id + "\"" + (_current == 0 ? " disabled=\"disabled\"" : "") + " class=\"default\" onchange=\"UpdateDropDownHidden('" + _schedule_id + "','" + _hidden_id + "');\">";
                if (_current == 0)
                    strSchedule += "<option value=\"0\">-- Select a Domain --</option>";
                else
                {
                    strSchedule += "<option value=\"0\">-- SELECT --</option>";
                    DataSet dsSchedule = GetSchedules(intDomain, 1);
                    foreach (DataRow drSchedule in dsSchedule.Tables[0].Rows)
                    {
                        if (Int32.Parse(drSchedule["id"].ToString()) == _current)
                            strSchedule += "<option value=\"" + drSchedule["id"].ToString() + "\" selected=\"selected\">" + drSchedule["name"].ToString() + "</option>";
                        else
                            strSchedule += "<option value=\"" + drSchedule["id"].ToString() + "\">" + drSchedule["name"].ToString() + "</option>";
                    }
                }
                strSchedule += "</select>";
            }
            else
                strSchedule += "<input type=\"text\" style=\"width:200px\" class=\"default\" value=\"" + GetSchedule(_current, "name") + "\" readonly />";
            strSchedule += "</td>";
            // DOMAIN
            int intServer = 0;
            if (intDomain > 0)
                intServer = Int32.Parse(GetDomain(intDomain, "tsm"));
            string strDomain = "<td>";
            if (_result == "")
            {
                strDomain += "<select name=\"" + _domain_id + "\" id=\"" + _domain_id + "\"" + (intDomain == 0 ? " disabled=\"disabled\"" : "") + " class=\"default\" onchange=\"PopulateTSMSchedules('" + _domain_id + "','" + _schedule_id + "');ResetDropDownHidden('" + _hidden_id + "');\">";
                if (intDomain == 0)
                    strDomain += "<option value=\"0\">-- Select a " + (_tsm > 0 ? "Server" : (_avamar > 0 ? "Grid" : "N/A")) + " --</option>";
                else
                {
                    strDomain += "<option value=\"0\">-- SELECT --</option>";
                    DataSet dsDomain = GetDomains(intServer, 1);
                    foreach (DataRow drDomain in dsDomain.Tables[0].Rows)
                    {
                        if (Int32.Parse(drDomain["id"].ToString()) == intDomain)
                            strDomain += "<option value=\"" + drDomain["id"].ToString() + "\" selected=\"selected\">" + drDomain["name"].ToString() + "</option>";
                        else
                            strDomain += "<option value=\"" + drDomain["id"].ToString() + "\">" + drDomain["name"].ToString() + "</option>";
                    }
                }
                strDomain += "</select>";
            }
            else
                strDomain += "<input type=\"text\" style=\"width:200px\" class=\"default\" value=\"" + GetDomain(intDomain, "name") + "\" readonly />";
            strDomain += "</td>";
            // SERVER
            string strServer = "<td>";
            if (_result == "")
            {
                strServer += "<select name=\"" + _server_id + "\" id=\"" + _server_id + "\" class=\"default\" onchange=\"PopulateTSMDomains('" + _server_id + "','" + _domain_id + "','" + _schedule_id + "');ResetDropDownHidden('" + _hidden_id + "');\">";
                strServer += "<option value=\"0\">-- SELECT --</option>";
                DataSet dsServer = Gets(_tsm, _avamar, 1);
                foreach (DataRow drServer in dsServer.Tables[0].Rows)
                {
                    if (Int32.Parse(drServer["id"].ToString()) == intServer)
                        strServer += "<option value=\"" + drServer["id"].ToString() + "\" selected=\"selected\">" + drServer["name"].ToString() + "</option>";
                    else
                        strServer += "<option value=\"" + drServer["id"].ToString() + "\">" + drServer["name"].ToString() + "</option>";
                }
                strServer += "</select>";
            }
            else
                strServer += "<input type=\"text\" style=\"width:200px\" class=\"default\" value=\"" + Get(intServer, "name") + "\" readonly />";
            strServer += "</td>";
            return strServer + strDomain + strSchedule;
        }

        public DataSet GetDecom(string _name)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@name", _name);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getTSMDecom", arParams);
        }
        public string GetDecom(string _name, string _column)
        {
            DataSet ds = GetDecom(_name);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public void AddDecom(string _name, string _server, string _port, string _domain, string _schedule, string _contacts)
        {
            arParams = new SqlParameter[6];
            arParams[0] = new SqlParameter("@name", _name);
            arParams[1] = new SqlParameter("@server", _server);
            arParams[2] = new SqlParameter("@port", _port);
            arParams[3] = new SqlParameter("@domain", _domain);
            arParams[4] = new SqlParameter("@schedule", _schedule);
            arParams[5] = new SqlParameter("@contacts", _contacts);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addTSMDecom", arParams);
        }

        public void AddRegistration(string _name, string _server, string _port, string _domain, string _schedule)
        {
            arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@name", _name);
            arParams[1] = new SqlParameter("@server", _server);
            arParams[2] = new SqlParameter("@port", _port);
            arParams[3] = new SqlParameter("@domain", _domain);
            arParams[4] = new SqlParameter("@schedule", _schedule);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addTSMRegistration", arParams);
        }
        public DataSet GetRegistration(string _name)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@name", _name);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getTSMRegistration", arParams);
        }
        public void DeleteRegistrations()
        {
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteTSMRegistrations");
        }


        public string GetBody(int _requestid, int _itemid, int _number, string _dsn_asset, string _dsn_ip)
        {
            StringBuilder sbData = new StringBuilder();
            OnDemandTasks oOnDemandTasks = new OnDemandTasks(user, dsn);
            Forecast oForecast = new Forecast(user, dsn);
            Servers oServer = new Servers(user, dsn);
            Projects oProject = new Projects(user, dsn);
            Requests oRequest = new Requests(user, dsn);
            Locations oLocation = new Locations(user, dsn);
            Classes oClass = new Classes(user, dsn);
            ResourceRequest oResourceRequest = new ResourceRequest(user, dsn);
            Users oUser = new Users(user, dsn);
            Organizations oOrganization = new Organizations(user, dsn);
            ModelsProperties oModelsProperties = new ModelsProperties(user, dsn);
            Asset oAsset = new Asset(user, _dsn_asset, dsn);
            Environments oEnvironment = new Environments(user, dsn);
            OperatingSystems oOperatingSystem = new OperatingSystems(user, dsn);
            ConsistencyGroups oConsistencyGroups = new ConsistencyGroups(user, dsn);
            Storage oStorage = new Storage(user, dsn);

            DataSet dsTask = oOnDemandTasks.GetServerBackup(_requestid, _itemid, _number);
            if (dsTask.Tables[0].Rows.Count > 0)
            {
                int intAnswer = Int32.Parse(dsTask.Tables[0].Rows[0]["answerid"].ToString());
                int intModel = oForecast.GetModelAsset(intAnswer);
                if (intModel == 0)
                    intModel = oForecast.GetModel(intAnswer);
                DataSet dsBackup = oForecast.GetBackup(intAnswer);
                DataSet dsAnswer = oServer.GetAnswer(intAnswer);
                if (dsAnswer.Tables[0].Rows.Count > 0)
                {
                    // FIX
                    bool _prod = false;
                    int intForecast = Int32.Parse(oForecast.GetAnswer(intAnswer, "forecastid"));
                    int intRequest2 = Int32.Parse(oForecast.Get(intForecast, "requestid"));
                    int intProject = oRequest.GetProjectNumber(intRequest2);
                    int intAvamar = 0;
                    int intTSM = 0;
                    int intClass = Int32.Parse(oForecast.GetAnswer(intAnswer, "classid"));
                    Int32.TryParse(oForecast.GetAnswer(intAnswer, "avamar"), out intAvamar);
                    int intRecovery = 0;
                    if (dsBackup.Tables[0].Rows.Count > 0)
                        Int32.TryParse(dsBackup.Tables[0].Rows[0]["recoveryid"].ToString(), out intRecovery);
                    //Int32.TryParse(oLocation.GetAddress(intRecovery, "tsm"), out intTSM);
                    int intAddress = 0;
                    Int32.TryParse(oForecast.GetAnswer(intAnswer, "addressid"), out intAddress);
                    Int32.TryParse(oLocation.GetAddress(intAddress, "tsm"), out intTSM);
                    int intEnv = Int32.Parse(oForecast.GetAnswer(intAnswer, "environmentid"));
                    int intModel2 = Int32.Parse(dsAnswer.Tables[0].Rows[0]["modelid"].ToString()); ;

                    bool boolProd = false;
                    bool boolUnder = false;
                    if (oClass.IsProd(intClass) == false)
                    {
                        if (_prod == false)
                        {


                        }
                        else
                            boolProd = true;
                        if (oForecast.IsDRUnder48(intAnswer, false) == true)
                            boolUnder = true;
                    }
                    else
                    {


                    }
                    string strClass = "Test";
                    if (boolProd == true)
                        strClass = "Production";

                    int intImplementor = 0;
                    DataSet dsTasks = oOnDemandTasks.GetPending(intAnswer);
                    if (dsTasks.Tables[0].Rows.Count > 0)
                    {
                        intImplementor = Int32.Parse(dsTasks.Tables[0].Rows[0]["resourceid"].ToString());
                        intImplementor = Int32.Parse(oResourceRequest.GetWorkflow(intImplementor, "userid"));
                    }
                    else
                        intImplementor = -999;

                    sbData.Append("<tr><td colspan=\"2\" class=\"header\">Project Information</tr>");
                    sbData.Append("<tr><td nowrap>Project Type:</td><td width=\"100%\">");
                    sbData.Append(oProject.Get(intProject, "bd"));
                    sbData.Append("</td></tr>");
                    sbData.Append("<tr><td nowrap>Portfolio:</td><td width=\"100%\">");
                    sbData.Append(oOrganization.GetName(Int32.Parse(oProject.Get(intProject, "organization"))));
                    sbData.Append("</td></tr>");
                    string strLead = oProject.Get(intProject, "lead");
                    string strRequester = oForecast.Get(intForecast, "userid");
                    string strEngineer = oProject.Get(intProject, "engineer");
                    string strTechnical = oProject.Get(intProject, "technical");
                    sbData.Append("<tr><td nowrap>Project Manager:</td><td width=\"100%\">");
                    sbData.Append(strLead != "" ? oUser.GetFullName(Int32.Parse(strLead)) : "N/A");
                    sbData.Append("</td></tr>");
                    sbData.Append("<tr><td nowrap>Requester:</td><td width=\"100%\">");
                    sbData.Append(strRequester != "" ? oUser.GetFullName(Int32.Parse(strRequester)) : "N/A");
                    sbData.Append("</td></tr>");
                    sbData.Append("<tr><td nowrap>Integration Engineer:</td><td width=\"100%\">");
                    sbData.Append(strEngineer != "" ? oUser.GetFullName(Int32.Parse(strEngineer)) : "N/A");
                    sbData.Append("</td></tr>");
                    sbData.Append("<tr><td nowrap>Technical Lead:</td><td width=\"100%\">");
                    sbData.Append(strTechnical != "" ? oUser.GetFullName(Int32.Parse(strTechnical)) : "N/A");
                    sbData.Append("</td></tr>");
                    sbData.Append("<tr><td nowrap>II Resource:</td><td width=\"100%\">");
                    sbData.Append(intImplementor > 0 || intImplementor == -999 ? oUser.GetFullName(intImplementor) : "N/A");
                    sbData.Append("</td></tr>");

                    // Design Information
                    sbData.Append("<tr><td colspan=\"2\">&nbsp;</tr>");
                    sbData.Append("<tr><td colspan=\"2\" class=\"header\">Design Information</tr>");
                    double dblQuantity = double.Parse(oForecast.GetAnswer(intAnswer, "quantity")) + double.Parse(oForecast.GetAnswer(intAnswer, "recovery_number"));
                    sbData.Append("<tr><td nowrap>Design ID:</td><td width=\"100%\">");
                    sbData.Append(intAnswer.ToString());
                    sbData.Append("</td></tr>");
                    sbData.Append("<tr><td nowrap>Commitment Date:</td><td width=\"100%\">");
                    sbData.Append(oForecast.GetAnswer(intAnswer, "implementation") == "" ? "" : DateTime.Parse(oForecast.GetAnswer(intAnswer, "implementation")).ToShortDateString());
                    sbData.Append("</td></tr>");
                    double dblA = 0.00;
                    DataSet dsA = oForecast.GetAcquisitions(intModel, 1);
                    foreach (DataRow drA in dsA.Tables[0].Rows)
                        dblA += double.Parse(drA["cost"].ToString());
                    sbData.Append("<tr><td nowrap>Acquisition Costs:</td><td width=\"100%\">");
                    sbData.Append(dblA.ToString("N"));
                    sbData.Append("</td></tr>");
                    double dblO = 0.00;
                    DataSet dsO = oForecast.GetOperations(intModel, 1);
                    foreach (DataRow drO in dsO.Tables[0].Rows)
                        dblO += double.Parse(drO["cost"].ToString());
                    sbData.Append("<tr><td nowrap>Operational Costs:</td><td width=\"100%\">");
                    sbData.Append(dblO.ToString("N"));
                    sbData.Append("</td></tr>");
                    double dblAmp = (double.Parse(oModelsProperties.Get(intModel, "amp")) * dblQuantity);
                    sbData.Append("<tr><td nowrap>AMPs:</td><td width=\"100%\">");
                    sbData.Append(dblAmp.ToString("N"));
                    sbData.Append(" AMPs");
                    sbData.Append("</td></tr>");
                    sbData.Append("<tr><td nowrap>Application Name:</td><td width=\"100%\">");
                    sbData.Append(oForecast.GetAnswer(intAnswer, "appname"));
                    sbData.Append("</td></tr>");
                    sbData.Append("<tr><td nowrap>Application Code:</td><td width=\"100%\">");
                    sbData.Append(oForecast.GetAnswer(intAnswer, "appcode"));
                    sbData.Append("</td></tr>");
                    string strContact1 = oForecast.GetAnswer(intAnswer, "appcontact");
                    if (strContact1 != "")
                    {
                        sbData.Append("<tr><td nowrap>Departmental Manager:</td><td width=\"100%\">");
                        sbData.Append(oUser.GetFullName(Int32.Parse(strContact1)));
                        sbData.Append(" (");
                        sbData.Append(oUser.GetName(Int32.Parse(strContact1)));
                        sbData.Append(")");
                        sbData.Append("</td></tr>");
                    }
                    string strContact2 = oForecast.GetAnswer(intAnswer, "admin1");
                    if (strContact2 != "")
                    {
                        sbData.Append("<tr><td nowrap>Application Technical Lead:</td><td width=\"100%\">");
                        sbData.Append(oUser.GetFullName(Int32.Parse(strContact2)));
                        sbData.Append(" (");
                        sbData.Append(oUser.GetName(Int32.Parse(strContact2)));
                        sbData.Append(")");
                        sbData.Append("</td></tr>");
                    }
                    string strContact3 = oForecast.GetAnswer(intAnswer, "admin2");
                    if (strContact3 != "")
                    {
                        sbData.Append("<tr><td nowrap>Administrative Contact:</td><td width=\"100%\">");
                        sbData.Append(oUser.GetFullName(Int32.Parse(strContact3)));
                        sbData.Append(" (");
                        sbData.Append(oUser.GetName(Int32.Parse(strContact3)));
                        sbData.Append(")");
                        sbData.Append("</td></tr>");
                    }
                    int intPlatform = Int32.Parse(oForecast.GetAnswer(intAnswer, "platformid"));

                    StringBuilder sbDataInfo = new StringBuilder();
                    int intCount = 0;

                    string strEmail = "";
                    string strShared = "";
                    bool boolOther = false;
                    foreach (DataRow dr in dsAnswer.Tables[0].Rows)
                    {
                        intCount++;
                        boolOther = !boolOther;
                        int intServer = Int32.Parse(dr["id"].ToString());
                        int intUser = Int32.Parse(oForecast.GetAnswer(intAnswer, "userid"));
                        if (intUser > 0)
                            strEmail = oUser.GetName(intUser);
                        int intCSM = Int32.Parse(dr["csmconfigid"].ToString());
                        int intCluster = Int32.Parse(dr["clusterid"].ToString());
                        int intNumber2 = Int32.Parse(dr["number"].ToString());
                        int intName = Int32.Parse(dr["nameid"].ToString());
                        string strName = oServer.GetName(intServer, true);
                        if (strShared != "")
                            strShared += ", ";
                        strShared += strName;
                        sbDataInfo.Append("<tr><td nowrap>Design Nickname:</td><td width=\"100%\">");
                        sbDataInfo.Append(oForecast.GetAnswer(intAnswer, "name"));
                        sbDataInfo.Append("</td></tr>");
                        DataSet dsGeneric = oServer.GetGeneric(intServer);
                        string strVIO = "";
                        string strVIODR = "";
                        if (oModelsProperties.IsVIO(intModel) == true)
                        {
                            strVIO = dsGeneric.Tables[0].Rows[0]["vio1"].ToString() + ", " + dsGeneric.Tables[0].Rows[0]["vio2"].ToString();
                            sbDataInfo.Append("<tr><td nowrap>Server Name(s):</td><td width=\"100%\">");
                            sbDataInfo.Append(strVIO);
                            sbDataInfo.Append("</td></tr>");
                        }
                        else
                        {
                            sbDataInfo.Append("<tr><td nowrap>Server Name:</td><td width=\"100%\">");
                            sbDataInfo.Append(strName);
                            sbDataInfo.Append("</td></tr>");
                        }
                        int intAsset = 0;
                        if (dr["assetid"].ToString() != "")
                            intAsset = Int32.Parse(dr["assetid"].ToString());
                        if (dsGeneric.Tables[0].Rows.Count > 0)
                        {
                            if (oModelsProperties.IsStorageDB_BootSANUnix(intModel) == true || oModelsProperties.IsStorageDB_BootSANWindows(intModel) == true)
                            {
                                if (dsGeneric.Tables[0].Rows[0]["dummy_name"].ToString() != "")
                                {
                                    sbDataInfo.Append("<tr><td nowrap>Dummy Name (BFS):</td><td width=\"100%\">");
                                    sbDataInfo.Append(dsGeneric.Tables[0].Rows[0]["dummy_name"].ToString());
                                    sbDataInfo.Append("</td></tr>");
                                }
                            }
                            if (oModelsProperties.IsVIO(intModel) == false)
                            {
                                if (dsGeneric.Tables[0].Rows[0]["ww1"].ToString() != "" || dsGeneric.Tables[0].Rows[0]["ww2"].ToString() != "")
                                {
                                    sbDataInfo.Append("<tr><td nowrap>World Wide Port Name(s):</td><td width=\"100%\">");
                                    sbDataInfo.Append(dsGeneric.Tables[0].Rows[0]["ww1"].ToString());
                                    sbDataInfo.Append(", ");
                                    sbDataInfo.Append(dsGeneric.Tables[0].Rows[0]["ww2"].ToString());
                                    sbDataInfo.Append("</td></tr>");
                                }
                            }
                        }
                        else
                        {
                            if (oModelsProperties.IsStorageDB_BootSANUnix(intModel) == true || oModelsProperties.IsStorageDB_BootSANWindows(intModel) == true)
                            {
                                sbDataInfo.Append("<tr><td nowrap>Dummy Name (BFS):</td><td width=\"100%\">");
                                sbDataInfo.Append(oAsset.GetServerOrBlade(intAsset, "dummy_name"));
                                sbDataInfo.Append("</td></tr>");
                            }
                            DataSet dsHBA = oAsset.GetHBA(intAsset);
                            string strHBA = "";
                            foreach (DataRow drHBA in dsHBA.Tables[0].Rows)
                            {
                                if (strHBA != "")
                                    strHBA += ", ";
                                strHBA += drHBA["name"].ToString();
                            }
                            sbDataInfo.Append("<tr><td nowrap>World Wide Port Names:</td><td width=\"100%\">");
                            sbDataInfo.Append(strHBA);
                            sbDataInfo.Append("</td></tr>");
                        }

                        if (oModelsProperties.IsVIO(intModel) == true)
                        {
                            if (boolUnder == true && dr["dr"].ToString() == "1")
                            {
                                if (dr["dr_name"].ToString() != "")
                                {
                                    sbDataInfo.Append("<tr><td nowrap>DR Server Name:</td><td width=\"100%\">");
                                    sbDataInfo.Append(dr["dr_name"].ToString());
                                    sbDataInfo.Append("</td></tr>");
                                }
                                else
                                {
                                    strVIODR = dsGeneric.Tables[0].Rows[0]["vio1_dr"].ToString() + ", " + dsGeneric.Tables[0].Rows[0]["vio2_dr"].ToString();
                                    sbDataInfo.Append("<tr><td nowrap>DR Server Name(s):</td><td width=\"100%\">");
                                    sbDataInfo.Append(strVIODR);
                                    sbDataInfo.Append("</td></tr>");
                                }
                            }
                        }
                        else
                        {
                            if (boolUnder == true && dr["dr"].ToString() == "1")
                            {
                                if (dr["dr_name"].ToString() != "")
                                {
                                    sbDataInfo.Append("<tr><td nowrap>DR Server Name:</td><td width=\"100%\">");
                                    sbDataInfo.Append(dr["dr_name"].ToString());
                                    sbDataInfo.Append("</td></tr>");
                                }
                                else
                                {
                                    sbDataInfo.Append("<tr><td nowrap>DR Server Name:</td><td width=\"100%\">");
                                    sbDataInfo.Append(strName);
                                    sbDataInfo.Append("-DR");
                                    sbDataInfo.Append("</td></tr>");
                                }
                            }
                        }
                        int intDR = 0;
                        if (dr["drid"].ToString() != "")
                            intDR = Int32.Parse(dr["drid"].ToString());
                        if (boolProd == true)
                        {
                            if (dsGeneric.Tables[0].Rows.Count > 0)
                            {
                                if (oModelsProperties.IsStorageDB_BootSANUnix(intModel) == true || oModelsProperties.IsStorageDB_BootSANWindows(intModel) == true)
                                {
                                    if (dsGeneric.Tables[0].Rows[0]["dummy_name_dr"].ToString() != "")
                                    {
                                        sbDataInfo.Append("<tr><td nowrap>DR Dummy Name (BFS):</td><td width=\"100%\">");
                                        sbDataInfo.Append(dsGeneric.Tables[0].Rows[0]["dummy_name_dr"].ToString());
                                        sbDataInfo.Append("</td></tr>");
                                    }
                                }
                                if (oModelsProperties.IsVIO(intModel) == false)
                                {
                                    if (dsGeneric.Tables[0].Rows[0]["ww1_dr"].ToString() != "" || dsGeneric.Tables[0].Rows[0]["ww2_dr"].ToString() != "")
                                    {
                                        sbDataInfo.Append("<tr><td nowrap>DR World Wide Port Name(s):</td><td width=\"100%\">");
                                        sbDataInfo.Append(dsGeneric.Tables[0].Rows[0]["ww1_dr"].ToString());
                                        sbDataInfo.Append(", ");
                                        sbDataInfo.Append(dsGeneric.Tables[0].Rows[0]["ww2_dr"].ToString());
                                        sbDataInfo.Append("</td></tr>");
                                    }
                                }
                            }
                            else
                            {
                                if (oModelsProperties.IsStorageDB_BootSANUnix(intModel) == true || oModelsProperties.IsStorageDB_BootSANWindows(intModel) == true)
                                {
                                    sbDataInfo.Append("<tr><td nowrap>DR Dummy Name (BFS):</td><td width=\"100%\">");
                                    sbDataInfo.Append(oAsset.GetServerOrBlade(intDR, "dummy_name"));
                                    sbDataInfo.Append("</td></tr>");
                                }
                                DataSet dsHBA = oAsset.GetHBA(intDR);
                                string strHBA = "";
                                foreach (DataRow drHBA in dsHBA.Tables[0].Rows)
                                {
                                    if (strHBA != "")
                                        strHBA += ", ";
                                    strHBA += drHBA["name"].ToString();
                                }
                                sbDataInfo.Append("<tr><td nowrap>DR World Wide Port Names:</td><td width=\"100%\">");
                                sbDataInfo.Append(strHBA);
                                sbDataInfo.Append("</td></tr>");
                            }
                        }
                        sbDataInfo.Append("<tr><td nowrap>Assigned IP Address:</td><td width=\"100%\">");
                        sbDataInfo.Append(oServer.GetIPs(intServer, 1, 0, 0, 0, _dsn_ip, "", ""));
                        sbDataInfo.Append("</td></tr>");
                        sbDataInfo.Append("<tr><td nowrap>Final IP Address:</td><td width=\"100%\">");
                        sbDataInfo.Append(oServer.GetIPs(intServer, 0, 1, 0, 0, _dsn_ip, "", ""));
                        sbDataInfo.Append("</td></tr>");
                        sbDataInfo.Append("<tr><td nowrap>Backup IP Address:</td><td width=\"100%\">");
                        sbDataInfo.Append(oServer.GetIPs(intServer, 0, 0, 0, 1, _dsn_ip, "", ""));
                        sbDataInfo.Append("</td></tr>");
                        int intType = oModelsProperties.GetType(intModel);

                        sbDataInfo.Append("<tr><td nowrap>Is a High Availability Device:</td><td width=\"100%\">");
                        sbDataInfo.Append(oForecast.IsHARoom(intAnswer) ? (dr["ha"].ToString() == "10" ? "Yes" : "No") : "N / A");
                        sbDataInfo.Append("</td></tr>");
                        sbDataInfo.Append("<tr><td nowrap>Model:</td><td width=\"100%\">");
                        sbDataInfo.Append(oModelsProperties.Get(intModel, "name"));
                        sbDataInfo.Append("</td></tr>");
                        sbDataInfo.Append("<tr><td nowrap>Fabric:</td><td width=\"100%\">");
                        sbDataInfo.Append(oModelsProperties.GetFabric(intModel));
                        sbDataInfo.Append("</td></tr>");
                        if (intAsset > 0)
                        {
                            sbDataInfo.Append("<tr><td nowrap>Serial Number:</td><td width=\"100%\">");
                            sbDataInfo.Append(oAsset.Get(intAsset, "serial").ToUpper());
                            sbDataInfo.Append("</td></tr>");
                            sbDataInfo.Append("<tr><td nowrap>Asset Tag:</td><td width=\"100%\">");
                            sbDataInfo.Append(oAsset.Get(intAsset, "asset").ToUpper());
                            sbDataInfo.Append("</td></tr>");
                            sbDataInfo.Append("<tr><td nowrap>Room:</td><td width=\"100%\">");
                            sbDataInfo.Append(oAsset.GetServerOrBlade(intAsset, "room"));
                            sbDataInfo.Append("</td></tr>");
                            sbDataInfo.Append("<tr><td nowrap>Rack:</td><td width=\"100%\">");
                            sbDataInfo.Append(oAsset.GetServerOrBlade(intAsset, "rack"));
                            sbDataInfo.Append("</td></tr>");
                            DataSet dsAssets = oServer.GetAssets(intServer);
                            foreach (DataRow drAsset in dsAssets.Tables[0].Rows)
                            {
                                if (drAsset["latest"].ToString() == "0" && drAsset["dr"].ToString() == "0")
                                {
                                    int intAssetOld = Int32.Parse(drAsset["assetid"].ToString());
                                    sbDataInfo.Append("<tr><td nowrap> - Previous Serial Number:</td><td width=\"100%\">");
                                    sbDataInfo.Append(oAsset.Get(intAssetOld, "serial").ToUpper());
                                    sbDataInfo.Append("</td></tr>");
                                    sbDataInfo.Append("<tr><td nowrap> - Previous Asset Tag:</td><td width=\"100%\">");
                                    sbDataInfo.Append(oAsset.Get(intAssetOld, "asset").ToUpper());
                                    sbDataInfo.Append("</td></tr>");
                                    sbDataInfo.Append("<tr><td nowrap> - Previous Class:</td><td width=\"100%\">");
                                    sbDataInfo.Append(drAsset["class"].ToString());
                                    sbDataInfo.Append("</td></tr>");
                                    sbDataInfo.Append("<tr><td nowrap> - Previous Environment:</td><td width=\"100%\">");
                                    sbDataInfo.Append(drAsset["environment"].ToString());
                                    sbDataInfo.Append("</td></tr>");
                                    sbDataInfo.Append("<tr><td nowrap> - Previous Room:</td><td width=\"100%\">");
                                    sbDataInfo.Append(oAsset.GetServerOrBlade(intAssetOld, "room"));
                                    sbDataInfo.Append("</td></tr>");
                                    sbDataInfo.Append("<tr><td nowrap> - Previous Rack:</td><td width=\"100%\">");
                                    sbDataInfo.Append(oAsset.GetServerOrBlade(intAssetOld, "rack"));
                                    sbDataInfo.Append("</td></tr>");
                                    if (oAsset.GetServerOrBlade(intAssetOld, "enclosureid") != "")
                                    {
                                        int intEnclosureID = Int32.Parse(oAsset.GetServerOrBlade(intAssetOld, "enclosureid"));
                                        if (intEnclosureID > 0)
                                        {
                                            sbDataInfo.Append("<tr><td nowrap> - Previous Enclosure:</td><td width=\"100%\">");
                                            sbDataInfo.Append(oAsset.Get(intEnclosureID, "name"));
                                            sbDataInfo.Append("</td></tr>");
                                            sbDataInfo.Append("<tr><td nowrap> - Previous Slot:</td><td width=\"100%\">");
                                            sbDataInfo.Append(oAsset.GetServerOrBlade(intAssetOld, "slot"));
                                            sbDataInfo.Append("</td></tr>");
                                        }
                                    }
                                }
                            }
                        }
                        if (intDR > 0)
                        {
                            sbDataInfo.Append("<tr><td nowrap>DR Serial Number:</td><td width=\"100%\">");
                            sbDataInfo.Append(oAsset.Get(intDR, "serial").ToUpper());
                            sbDataInfo.Append("</td></tr>");
                            sbDataInfo.Append("<tr><td nowrap>DR Asset Tag:</td><td width=\"100%\">");
                            sbDataInfo.Append(oAsset.Get(intDR, "asset").ToUpper());
                            sbDataInfo.Append("</td></tr>");
                            sbDataInfo.Append("<tr><td nowrap>DR Room:</td><td width=\"100%\">");
                            sbDataInfo.Append(oAsset.GetServerOrBlade(intDR, "room"));
                            sbDataInfo.Append("</td></tr>");
                            sbDataInfo.Append("<tr><td nowrap>DR Rack:</td><td width=\"100%\">");
                            sbDataInfo.Append(oAsset.GetServerOrBlade(intDR, "rack"));
                            sbDataInfo.Append("</td></tr>");
                        }
                        sbDataInfo.Append("<tr><td nowrap>Current Class:</td><td width=\"100%\">");
                        sbDataInfo.Append(strClass);
                        sbDataInfo.Append("</td></tr>");
                        sbDataInfo.Append("<tr><td nowrap>Final Class:</td><td width=\"100%\">");
                        sbDataInfo.Append(oClass.Get(intClass, "name"));
                        sbDataInfo.Append("</td></tr>");
                        sbDataInfo.Append("<tr><td nowrap>Environment:</td><td width=\"100%\">");
                        sbDataInfo.Append(oEnvironment.Get(intEnv, "name"));
                        sbDataInfo.Append("</td></tr>");
                        int intOS = Int32.Parse(dr["osid"].ToString());
                        sbDataInfo.Append("<tr><td nowrap>Operating System:</td><td width=\"100%\">");
                        sbDataInfo.Append(oOperatingSystem.Get(intOS, "name"));
                        sbDataInfo.Append("</td></tr>");
                        sbDataInfo.Append("<tr><td nowrap>Clustered Server Name:</td><td width=\"100%\">");
                        sbDataInfo.Append("N / A");
                        sbDataInfo.Append("</td></tr>");
                        int intConsistency = Int32.Parse(dr["dr_consistencyid"].ToString());
                        if (intConsistency > 0)
                        {
                            sbDataInfo.Append("<tr><td nowrap>Consistency Group Name:</td><td width=\"100%\">");
                            sbDataInfo.Append(oConsistencyGroups.Get(intConsistency, "name"));
                            sbDataInfo.Append("</td></tr>");
                            DataSet dsMembers = oConsistencyGroups.GetMember(intServer);
                            string strMembers = "";
                            foreach (DataRow drMember in dsMembers.Tables[0].Rows)
                            {
                                if (strMembers != "")
                                    strMembers += ", ";
                                strMembers += drMember["name"].ToString();
                            }
                            sbDataInfo.Append("<tr><td nowrap>Consistency Group Members:</td><td width=\"100%\">");
                            sbDataInfo.Append(strMembers);
                            sbDataInfo.Append("</td></tr>");
                        }
                        else
                        {
                            sbDataInfo.Append("<tr><td nowrap>Consistency Group:</td><td width=\"100%\">");
                            sbDataInfo.Append("N / A");
                            sbDataInfo.Append("</td></tr>");
                        }
                        sbData.Append("<tr><td colspan=\"2\">&nbsp;</tr>");
                        sbData.Append("<tr><td colspan=\"2\" class=\"header\">");
                        sbData.Append("ClearView Backup Request Form  for ");
                        sbData.Append(strName);
                        sbData.Append("</td></tr>");
                        sbData.Append(sbDataInfo.ToString());


                        /*
                        // Dynamic Backup
                        string strResult = dr["tsm_output"].ToString();
                        strBackup += "<tr" + (boolOther == false ? " bgcolor=\"#F6F6F6\"" : "") + ">";
                        strBackup += "<td>" + strName + "</td>";
                        strPreview += "<tr>";
                        strPreview += "<td>" + strName + "</td>";
                        int intSchedule = Int32.Parse(dr["tsm_schedule"].ToString());
                        if (intSchedule > 0)
                        {
                            int intDomain = Int32.Parse(GetSchedule(intSchedule, "domain"));
                            string strDomain = GetDomain(intDomain, "name");
                            int intTSMServer = Int32.Parse(GetDomain(intDomain, "tsm"));
                            strPreview += "<td>" + Get(intTSMServer, "name") + "</td>";
                            strPreview += "<td>" + Get(intTSMServer, "port") + "</td>";
                            strPreview += "<td>" + GetSchedule(intSchedule, "name") + "</td>";
                        }
                        // Either TSM, Avamar or Legato

                        strBackup += LoadDDL("ddlServer_" + intServer.ToString(), "ddlDomain_" + intServer.ToString(), "ddlSchedule_" + intServer.ToString(), "HDN_" + intServer.ToString() + "_TSM_SCHEDULE", intSchedule, intTSM, intAvamar, strResult);
                        if (intTSM > 0)
                        {
                            strBackup += "<td>";
                            if (strResult == "")
                            {
                                strBackup += "<select id=\"DDL_" + intServer + "_TSM_CLOPTSET\" class=\"default\" onchange=\"UpdateDDL(this,'HDN_" + intServer + "_TSM_CLOPTSET');\" style=\"width:150px;\">";
                                strBackup += "<option value=\"0\">-- SELECT --</option>";
                                DataSet dsTSM = GetCloptsets(1);
                                foreach (DataRow drTSM in dsTSM.Tables[0].Rows)
                                    strBackup += "<option value=\"" + drTSM["id"].ToString() + "\"" + (dr["tsm_cloptset"].ToString() == drTSM["id"].ToString() ? " selected" : "") + ">" + drTSM["name"].ToString() + "</option>";
                                strBackup += "</select>";
                            }
                            else
                                strBackup += "<input type=\"text\" style=\"width:200px\" class=\"default\" value=\"" + GetCloptset(Int32.Parse(dr["tsm_cloptset"].ToString()), "name") + "\" readonly />";
                            strBackup += "</td>";
                        }
                        if ((intTSM > 0 && intTSMAutomated >= 0) || (intAvamar > 0 && intAvamarAutomated >= 0))
                        {
                            strBackup += "<td><input id=\"chkRedo_" + intServer.ToString() + "\" type=\"checkbox\" name=\"chkRedo_" + intServer.ToString() + "\" onclick=\"UpdateCheckBox(this,'HDN_" + intServer.ToString() + "_TSM_REDO');\" /> Regenerate</td>";
                        }
                        strBackup += "</tr>";
                        strPreview += "</tr>";
                        bool boolBypass = (dr["tsm_bypass"].ToString() == "1");
                        if (intTSM > 0)
                        {
                            if (dr["tsm_register"].ToString() == "")
                                strBackup += "<tr" + (boolOther == false ? " bgcolor=\"#F6F6F6\"" : "") + "><td>&nbsp;&nbsp;&nbsp;&nbsp;<img src=\"/images/arrow_down_right.gif\" border=\"0\" align=\"absmiddle\"/>REGISTER:</td><td colspan=\"6\"><input type=\"text\" id=\"TXT_" + intServer.ToString() + "_TSM_REGISTER\" style=\"width:850px\" class=\"default\" value=\"Choose Options and Click SAVE to Generate\" disabled maxlength=\"300\" /></td></tr>";
                            else
                                strBackup += "<tr" + (boolOther == false ? " bgcolor=\"#F6F6F6\"" : "") + "><td>&nbsp;&nbsp;&nbsp;&nbsp;<img src=\"/images/arrow_down_right.gif\" border=\"0\" align=\"absmiddle\"/>REGISTER:</td><td colspan=\"6\"><input type=\"text\" id=\"TXT_" + intServer.ToString() + "_TSM_REGISTER\" style=\"width:850px\" class=\"default\" value=\"" + Server.HtmlEncode(dr["tsm_register"].ToString()) + "\"" + (strResult != "" ? " readonly" : " onblur=\"UpdateTextBox(this,'HDN_" + intServer.ToString() + "_TSM_REGISTER');\"") + " maxlength=\"300\"/></td></tr>";
                            if (dr["tsm_define"].ToString() == "")
                                strBackup += "<tr" + (boolOther == false ? " bgcolor=\"#F6F6F6\"" : "") + "><td>&nbsp;&nbsp;&nbsp;&nbsp;<img src=\"/images/arrow_down_right.gif\" border=\"0\" align=\"absmiddle\"/>DEFINE:</td><td colspan=\"3\"><input type=\"text\" id=\"TXT_" + intServer.ToString() + "_TSM_DEFINE\" style=\"width:500px\" class=\"default\" value=\"Choose Options and Click SAVE to Generate\" disabled maxlength=\"300\" /></td><td colspan=\"3\"><input id=\"chkRedo_" + intServer.ToString() + "\" type=\"checkbox\" name=\"chkBypass_" + intServer.ToString() + "\" disabled /> Bypass Auto-Registration</td></tr>";
                            else
                                strBackup += "<tr" + (boolOther == false ? " bgcolor=\"#F6F6F6\"" : "") + "><td>&nbsp;&nbsp;&nbsp;&nbsp;<img src=\"/images/arrow_down_right.gif\" border=\"0\" align=\"absmiddle\"/>DEFINE:</td><td colspan=\"3\"><input type=\"text\" id=\"TXT_" + intServer.ToString() + "_TSM_DEFINE\" style=\"width:500px\" class=\"default\" value=\"" + Server.HtmlEncode(dr["tsm_define"].ToString()) + "\"" + (strResult != "" ? " readonly" : " onblur=\"UpdateTextBox(this,'HDN_" + intServer.ToString() + "_TSM_DEFINE');\"") + " maxlength=\"300\"/></td><td colspan=\"3\"><input id=\"chkRedo_" + intServer.ToString() + "\" type=\"checkbox\" name=\"chkBypass_" + intServer.ToString() + "\"" + (strResult != "" ? " disabled" : " onclick=\"UpdateCheckBox(this,'HDN_" + intServer.ToString() + "_TSM_BYPASS');\"" + (boolBypass ? " checked" : "")) + " /> Bypass Auto-Registration</td></tr>";
                        }
                        else
                        {
                            btnRefresh.Enabled = false;
                        }

                        if (chk1.Checked == true)
                        {
                            if (boolBypass == true || ((intTSM > 0 && intTSMAutomated < 0) || (intAvamar > 0 && intAvamarAutomated < 0)))
                                strBackup += "<tr" + (boolOther == false ? " bgcolor=\"#F6F6F6\"" : "") + "><td valign=\"top\" align=\"right\" class=\"redlink\">RESULT:</td><td colspan=\"5\">" + (strResult == "" ? "Auto-Registration Skipped (Please register manually)" : oFunction.FormatText(strResult)) + "</td></tr>";
                            else
                                strBackup += "<tr" + (boolOther == false ? " bgcolor=\"#F6F6F6\"" : "") + "><td valign=\"top\" align=\"right\" class=\"redlink\">RESULT:</td><td colspan=\"5\">" + (strResult == "" ? "Pending Execution..." : oFunction.FormatText(strResult)) + "</td></tr>";
                        }
                        else
                            btnRefresh.Enabled = false;
                        strBackup += "<tr><td colspan=\"6\">&nbsp;</td></tr>";

                        // Backup Header
                        if (intTSM > 0)
                        {
                            strBackupHeader = "<tr bgcolor='#EEEEEE'>";
                            strBackupHeader += "<td><b><u>Server:</u></b></td>";
                            strBackupHeader += "<td><b><u>TSM Server:</u></b></td>";
                            strBackupHeader += "<td><b><u>Domain:</u></b></td>";
                            strBackupHeader += "<td><b><u>Schedule:</u></b></td>";
                            strBackupHeader += "<td><b><u>CLOPTSET:</u></b></td>";
                            strBackupHeader += "<td></td></tr>";
                        }
                        else if (intAvamar > 0)
                        {
                            strBackupHeader = "<tr bgcolor='#EEEEEE'>";
                            strBackupHeader += "<td><b><u>Server:</u></b></td>";
                            strBackupHeader += "<td><b><u>Grid:</u></b></td>";
                            strBackupHeader += "<td><b><u>Domain:</u></b></td>";
                            strBackupHeader += "<td><b><u>Group:</u></b></td>";
                            strBackupHeader += "<td></td></tr>";
                        }


                        strHiddenV += "<input type=\"hidden\" name=\"HDN_" + intServer.ToString() + "_TSM_SCHEDULE\" id=\"HDN_" + intServer.ToString() + "_TSM_SCHEDULE\" value=\"" + dr["tsm_schedule"].ToString() + "\" />";
                        if (intTSM > 0)
                            strHiddenV += "<input type=\"hidden\" name=\"HDN_" + intServer.ToString() + "_TSM_CLOPTSET\" id=\"HDN_" + intServer.ToString() + "_TSM_CLOPTSET\" value=\"" + dr["tsm_cloptset"].ToString() + "\" />";
                        strHiddenV += "<input type=\"hidden\" name=\"HDN_" + intServer.ToString() + "_TSM_REGISTER\" id=\"HDN_" + intServer.ToString() + "_TSM_REGISTER\" value=\"" + Server.HtmlEncode(dr["tsm_register"].ToString()) + "\" />";
                        strHiddenV += "<input type=\"hidden\" name=\"HDN_" + intServer.ToString() + "_TSM_DEFINE\" id=\"HDN_" + intServer.ToString() + "_TSM_DEFINE\" value=\"" + Server.HtmlEncode(dr["tsm_define"].ToString()) + "\" />";
                        strHiddenV += "<input type=\"hidden\" name=\"HDN_" + intServer.ToString() + "_TSM_REDO\" id=\"HDN_" + intServer.ToString() + "_TSM_REDO\" value=\"0\" />";
                        strHiddenV += "<input type=\"hidden\" name=\"HDN_" + intServer.ToString() + "_TSM_BYPASS\" id=\"HDN_" + intServer.ToString() + "_TSM_BYPASS\" value=\"" + dr["tsm_bypass"].ToString() + "\" />";
                        strValidation += intServer.ToString() + ";";
                        sbDataInfo = new StringBuilder();
                        */

                        // BACKUP INFORMATION
                        if (dsBackup.Tables[0].Rows.Count > 0)
                        {
                            if (intRecovery > 0)
                            {
                                sbData.Append("<tr><td nowrap>Recovery Location:</td><td width=\"100%\">");
                                sbData.Append(oLocation.GetFull(intRecovery));
                                sbData.Append("</td></tr>");
                            }
                            if (dsBackup.Tables[0].Rows[0]["daily"].ToString() == "1")
                            {
                                sbData.Append("<tr><td nowrap>Timing/Frequency of Backups:</td><td width=\"100%\">");
                                sbData.Append("Daily");
                                sbData.Append("</td></tr>");
                            }
                            else if (dsBackup.Tables[0].Rows[0]["weekly"].ToString() == "1")
                            {
                                sbData.Append("<tr><td nowrap>Timing/Frequency of Backups:</td><td width=\"100%\">");
                                sbData.Append("Weekly");
                                sbData.Append("</td></tr>");
                            }
                            else if (dsBackup.Tables[0].Rows[0]["monthly"].ToString() == "1")
                            {
                                sbData.Append("<tr><td nowrap>Timing/Frequency of Backups:</td><td width=\"100%\">");
                                sbData.Append("Monthly");
                                sbData.Append("</td></tr>");
                            }
                            if (dsBackup.Tables[0].Rows[0]["time"].ToString() == "1")
                            {
                                sbData.Append("<tr><td nowrap>Start Time:</td><td width=\"100%\">");
                                sbData.Append(dsBackup.Tables[0].Rows[0]["time_hour"].ToString());
                                sbData.Append(" ");
                                sbData.Append(dsBackup.Tables[0].Rows[0]["time_switch"].ToString());
                                sbData.Append("</td></tr>");
                            }
                            else
                            {
                                sbData.Append("<tr><td nowrap>Start Time:</td><td width=\"100%\">");
                                sbData.Append("Don't Care");
                                sbData.Append("</td></tr>");
                            }

                            double dblHighU = 0.00;
                            double dblStandardU = 0.00;
                            double dblLowU = 0.00;
                            double dblHighQAU = 0.00;
                            double dblStandardQAU = 0.00;
                            double dblLowQAU = 0.00;
                            double dblHighTestU = 0.00;
                            double dblStandardTestU = 0.00;
                            double dblLowTestU = 0.00;
                            DataSet dsStorage = oStorage.GetLuns(intAnswer, 0, intCluster, intCSM, intNumber2);
                            foreach (DataRow drStorage in dsStorage.Tables[0].Rows)
                            {
                                if (drStorage["size"].ToString() != "")
                                {
                                    if (drStorage["performance"].ToString() == "High")
                                        dblHighU += double.Parse(drStorage["size"].ToString());
                                    if (drStorage["performance"].ToString() == "Standard")
                                        dblStandardU += double.Parse(drStorage["size"].ToString());
                                    if (drStorage["performance"].ToString() == "Low")
                                        dblLowU += double.Parse(drStorage["size"].ToString());
                                }
                                if (drStorage["size_qa"].ToString() != "")
                                {
                                    if (drStorage["performance"].ToString() == "High")
                                        dblHighQAU += double.Parse(drStorage["size_qa"].ToString());
                                    if (drStorage["performance"].ToString() == "Standard")
                                        dblStandardQAU += double.Parse(drStorage["size_qa"].ToString());
                                    if (drStorage["performance"].ToString() == "Low")
                                        dblLowQAU += double.Parse(drStorage["size_qa"].ToString());
                                }
                                if (drStorage["size_test"].ToString() != "")
                                {
                                    if (drStorage["performance"].ToString() == "High")
                                        dblHighTestU += double.Parse(drStorage["size_test"].ToString());
                                    if (drStorage["performance"].ToString() == "Standard")
                                        dblStandardTestU += double.Parse(drStorage["size_test"].ToString());
                                    if (drStorage["performance"].ToString() == "Low")
                                        dblLowTestU += double.Parse(drStorage["size_test"].ToString());
                                }
                                DataSet dsMount = oStorage.GetMountPoints(Int32.Parse(drStorage["id"].ToString()));
                                foreach (DataRow drMount in dsMount.Tables[0].Rows)
                                {
                                    if (drMount["size"].ToString() != "")
                                    {
                                        if (drMount["performance"].ToString() == "High")
                                            dblHighU += double.Parse(drMount["size"].ToString());
                                        if (drMount["performance"].ToString() == "Standard")
                                            dblStandardU += double.Parse(drMount["size"].ToString());
                                        if (drMount["performance"].ToString() == "Low")
                                            dblLowU += double.Parse(drMount["size"].ToString());
                                    }
                                    if (drMount["size_qa"].ToString() != "")
                                    {
                                        if (drMount["performance"].ToString() == "High")
                                            dblHighQAU += double.Parse(drMount["size_qa"].ToString());
                                        if (drMount["performance"].ToString() == "Standard")
                                            dblStandardQAU += double.Parse(drMount["size_qa"].ToString());
                                        if (drMount["performance"].ToString() == "Low")
                                            dblLowQAU += double.Parse(drMount["size_qa"].ToString());
                                    }
                                    if (drMount["size_test"].ToString() != "")
                                    {
                                        if (drMount["performance"].ToString() == "High")
                                            dblHighTestU += double.Parse(drMount["size_test"].ToString());
                                        if (drMount["performance"].ToString() == "Standard")
                                            dblStandardTestU += double.Parse(drMount["size_test"].ToString());
                                        if (drMount["performance"].ToString() == "Low")
                                            dblLowTestU += double.Parse(drMount["size_test"].ToString());
                                    }
                                }
                            }
                            double dblTotal = dblHighU + dblStandardU + dblLowU + dblHighQAU + dblStandardQAU + dblLowQAU + dblHighTestU + dblStandardTestU + dblLowTestU;
                            sbData.Append("<tr><td nowrap>Total Combined Disk Capacity (GB):</td><td width=\"100%\">");
                            sbData.Append(dblTotal.ToString("0"));
                            sbData.Append(" GB");
                            sbData.Append("</td></tr>");
                            sbData.Append("<tr><td nowrap>Current Combined Disk Utilized (GB):</td><td width=\"100%\">");
                            sbData.Append("5 GB");
                            sbData.Append("</td></tr>");
                            sbData.Append("<tr><td nowrap>Average Size of One Typical Data File:</td><td width=\"100%\">");
                            sbData.Append(dsBackup.Tables[0].Rows[0]["average_one"].ToString());
                            sbData.Append(" GB");
                            sbData.Append("</td></tr>");
                            if (dsBackup.Tables[0].Rows[0]["documentation"].ToString() == "")
                            {
                                sbData.Append("<tr><td nowrap>Production Turnover Documentation:</td><td width=\"100%\">");
                                sbData.Append("Not Specified");
                                sbData.Append("</td></tr>");
                            }
                            else
                            {
                                sbData.Append("<tr><td nowrap>Production Turnover Documentation:</td><td width=\"100%\">");
                                sbData.Append(dsBackup.Tables[0].Rows[0]["documentation"].ToString());
                                sbData.Append("</td></tr>");
                            }

                        }
                    }
                    sbData.Insert(0, "<table width=\"100%\" cellpadding=\"2\" cellspacing=\"1\" border=\"0\">");
                    sbData.Append("</table>");
                }
            }
            return sbData.ToString();
        }
    }
}
