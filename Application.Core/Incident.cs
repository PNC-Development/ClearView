using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using System.Data.SqlTypes;
using System.Text;
using System.IO;

namespace NCC.ClearView.Application.Core
{
    public class Incident
	{
        private string dsn = "";
        private int user = 0;
		private SqlParameter[] arParams;
        public Incident(int _user, string _dsn)
        {
            user = _user;
            dsn = _dsn;
        }
        public int Add(string _error, string _compare, string _route, int _automatic, string _message, int _priority, int _workstation, int _enabled)
        {
            arParams = new SqlParameter[9];
            arParams[0] = new SqlParameter("@error", _error);
            arParams[1] = new SqlParameter("@compare", _compare);
            arParams[2] = new SqlParameter("@route", _route);
            arParams[3] = new SqlParameter("@automatic", _automatic);
            arParams[4] = new SqlParameter("@message", _message);
            arParams[5] = new SqlParameter("@priority", _priority);
            arParams[6] = new SqlParameter("@workstation", _workstation);
            arParams[7] = new SqlParameter("@enabled", _enabled);
            arParams[8] = new SqlParameter("@id", SqlDbType.Int);
            arParams[8].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addIncidentKB", arParams);
            return Int32.Parse(arParams[8].Value.ToString());
        }
        public void Update(int _id, string _error, string _compare, string _route, int _automatic, string _message, int _priority, int _workstation, int _enabled)
        {
            arParams = new SqlParameter[9];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@error", _error);
            arParams[2] = new SqlParameter("@compare", _compare);
            arParams[3] = new SqlParameter("@route", _route);
            arParams[4] = new SqlParameter("@automatic", _automatic);
            arParams[5] = new SqlParameter("@message", _message);
            arParams[6] = new SqlParameter("@priority", _priority);
            arParams[7] = new SqlParameter("@workstation", _workstation);
            arParams[8] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateIncidentKB", arParams);
        }
        public void Enable(int _id, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateIncidentKBEnabled", arParams);
        }
        public void Delete(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteIncidentKB", arParams);
        }
        public DataSet Get(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getIncidentKB", arParams);
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
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getIncidentKBs", arParams);
        }
        public DataSet Gets(string _error)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@error", _error);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getIncidentKBsError", arParams);
        }



        public void Add(int _errorid, int _workstation, int _kbid)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@errorid", _errorid);
            arParams[1] = new SqlParameter("@workstation", _workstation);
            arParams[2] = new SqlParameter("@kbid", _kbid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addIncident", arParams);
        }
        public void Update(int _errorid, int _workstation, string _generated)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@errorid", _errorid);
            arParams[1] = new SqlParameter("@workstation", _workstation);
            arParams[2] = new SqlParameter("@generated", _generated);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateIncident0", arParams);
        }
        public void Update(int _errorid, int _workstation, string _retrieved, string _incident)
        {
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@errorid", _errorid);
            arParams[1] = new SqlParameter("@workstation", _workstation);
            arParams[2] = new SqlParameter("@retrieved", _retrieved);
            arParams[3] = new SqlParameter("@incident", _incident);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateIncident1", arParams);
        }
        public void Update(int _errorid, int _workstation, string _resolved, string _resolved_by, string _resolved_comments)
        {
            arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@errorid", _errorid);
            arParams[1] = new SqlParameter("@workstation", _workstation);
            arParams[2] = new SqlParameter("@resolved", _resolved);
            arParams[3] = new SqlParameter("@resolved_by", _resolved_by);
            arParams[4] = new SqlParameter("@resolved_comments", _resolved_comments);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateIncident2", arParams);
        }
        public void Delete(int _errorid, int _workstation)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@errorid", _errorid);
            arParams[1] = new SqlParameter("@workstation", _workstation);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteIncident", arParams);
        }
        public DataSet Get(int _errorid, int _workstation)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@errorid", _errorid);
            arParams[1] = new SqlParameter("@workstation", _workstation);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getIncident", arParams);
        }
        public DataSet GetGenerated()
        {
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getIncidentsGenerated");
        }
        public DataSet GetCreated()
        {
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getIncidentsCreated");
        }
        public DataSet GetUpdated()
        {
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getIncidentsUpdated");
        }
        public DataSet DeviceInformation(int id, int workstation)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", id);
            arParams[1] = new SqlParameter("@workstation", workstation);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDeviceInformation", arParams);
        }
    }
}
