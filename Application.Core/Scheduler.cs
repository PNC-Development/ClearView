using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.ApplicationBlocks.Data;

namespace NCC.ClearView.Application.Core
{
    public class SchedulerStatus
    {
        public const int RunOnce = -1;
        public const int Waiting = 0;
        public const int Running = 1;
    }
    public class Scheduler
	{
		private string dsn = "";
		private int user = 0;
		private SqlParameter[] arParams;
        public Scheduler(int _user, string _dsn)
		{
			user = _user;
			dsn = _dsn;
		}
        public int Add(string _name, string _description, string _server, string _parameters, int _credentials, string _days, string _times, int _timeout, int _privledges, int _interactive, int _status, string _last, int _error, int _enabled)
        {
            arParams = new SqlParameter[15];
            arParams[0] = new SqlParameter("@name", _name);
            arParams[1] = new SqlParameter("@description", _description);
            arParams[2] = new SqlParameter("@server", _server);
            arParams[3] = new SqlParameter("@parameters", _parameters);
            arParams[4] = new SqlParameter("@credentials", _credentials);
            arParams[5] = new SqlParameter("@days", _days);
            arParams[6] = new SqlParameter("@times", _times);
            arParams[7] = new SqlParameter("@timeout", _timeout);
            arParams[8] = new SqlParameter("@privledges", _privledges);
            arParams[9] = new SqlParameter("@interactive", _interactive);
            arParams[10] = new SqlParameter("@status", _status);
            arParams[11] = new SqlParameter("@last", (_last == "" ? SqlDateTime.Null : DateTime.Parse(_last)));
            arParams[12] = new SqlParameter("@error", _error);
            arParams[13] = new SqlParameter("@enabled", _enabled);
            arParams[14] = new SqlParameter("@id", SqlDbType.Int);
            arParams[14].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addScheduler", arParams);
            return Int32.Parse(arParams[14].Value.ToString());
        }
        public void Update(int _id, string _name, string _description, string _server, string _parameters, int _credentials, string _days, string _times, int _timeout, int _privledges, int _interactive, int _status, int _enabled)
        {
            arParams = new SqlParameter[13];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@name", _name);
            arParams[2] = new SqlParameter("@description", _description);
            arParams[3] = new SqlParameter("@server", _server);
            arParams[4] = new SqlParameter("@parameters", _parameters);
            arParams[5] = new SqlParameter("@credentials", _credentials);
            arParams[6] = new SqlParameter("@days", _days);
            arParams[7] = new SqlParameter("@times", _times);
            arParams[8] = new SqlParameter("@timeout", _timeout);
            arParams[9] = new SqlParameter("@privledges", _privledges);
            arParams[10] = new SqlParameter("@interactive", _interactive);
            arParams[11] = new SqlParameter("@status", _status);
            arParams[12] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateScheduler", arParams);
        }
        public void Enable(int _id, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateSchedulerEnabled", arParams);
        }
        public void Status(int _id, int _status, string _last, int _error)
        {
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@status", _status);
            arParams[2] = new SqlParameter("@last", (_last == "" ? SqlDateTime.Null : DateTime.Parse(_last)));
            arParams[3] = new SqlParameter("@error", _error);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateSchedulerStatus", arParams);
        }
        public void Delete(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteScheduler", arParams);
        }
        public DataSet Get(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getScheduler", arParams);
        }
        public string Get(int _id, string _column)
        {
            DataSet ds = Get(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public DataSet Gets(int _enabled)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getSchedulers", arParams);
        }


        public void AddLog(int _psexecid, string _detail, int _error)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@psexecid", _psexecid);
            arParams[1] = new SqlParameter("@detail", _detail);
            arParams[2] = new SqlParameter("@error", _error);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addSchedulerLog", arParams);
        }

        public DataSet GetLogs(int _psexecid, int _rows)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@psexecid", _psexecid);
            arParams[1] = new SqlParameter("@rows", _rows);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getSchedulerLogs", arParams);
        }
    }
}
