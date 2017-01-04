using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;

namespace NCC.ClearView.Application.Core
{
	public class Solution
	{
		private string dsn = "";
		private int user = 0;
		private SqlParameter[] arParams;
        public Solution(int _user, string _dsn)
		{
			user = _user;
			dsn = _dsn;
		}
        public void AddCode(string _code, int _serviceid, int _modelid, int _priority, int _enabled)
        {
            arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@code", _code);
            arParams[1] = new SqlParameter("@serviceid", _serviceid);
            arParams[2] = new SqlParameter("@modelid", _modelid);
            arParams[3] = new SqlParameter("@priority", _priority);
            arParams[4] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addSolutionCode", arParams);
        }
        public void UpdateCode(int _id, string _code, int _serviceid, int _modelid, int _enabled)
        {
            arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@code", _code);
            arParams[2] = new SqlParameter("@serviceid", _serviceid);
            arParams[3] = new SqlParameter("@modelid", _modelid);
            arParams[4] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateSolutionCode", arParams);
        }
        public void UpdateCodeOrder(int _id, int _priority)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@priority", _priority);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateSolutionCodeOrder", arParams);
        }
        public void EnableCode(int _id, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateSolutionCodeEnabled", arParams);
        }
        public void DeleteCode(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteSolutionCode", arParams);
        }
        public DataSet GetCodeModel(int _modelid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@modelid", _modelid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getSolutionCodeModel", arParams);
        }
        public DataSet GetCode(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getSolutionCode", arParams);
        }
        public DataSet GetCodes(int _enabled)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getSolutionCodes", arParams);
        }
        public string GetCode(int _id, string _column)
        {
            DataSet ds = GetCode(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }

        public void AddSelection(int _selectionid, int _responseid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@selectionid", _selectionid);
            arParams[1] = new SqlParameter("@responseid", _responseid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addSolutionSelection", arParams);
        }
        public void DeleteSelection(int _responseid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@responseid", _responseid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteSolutionSelection", arParams);
        }
        public DataSet GetSelectionByResponse(int _responseid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@responseid", _responseid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getSolutionSelectionByResponse", arParams);
        }
        public DataSet GetSelectionByCode(int _codeid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@codeid", _codeid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getSolutionSelectionByCode", arParams);
        }

        public DataSet GetLocation(int _classid, int _environmentid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@classid", _classid);
            arParams[1] = new SqlParameter("@environmentid", _environmentid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getSolutionLocation", arParams);
        }
        public DataSet GetLocation(int _classid, int _environmentid, int _addressid)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@classid", _classid);
            arParams[1] = new SqlParameter("@environmentid", _environmentid);
            arParams[2] = new SqlParameter("@addressid", _addressid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getSolutionLocations", arParams);
        }
        public void AddLocation(int _classid, int _environmentid, int _addressid, int _codeid)
        {
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@classid", _classid);
            arParams[1] = new SqlParameter("@environmentid", _environmentid);
            arParams[2] = new SqlParameter("@addressid", _addressid);
            arParams[3] = new SqlParameter("@codeid", _codeid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addSolutionLocation", arParams);
        }
        public void DeleteLocations(int _classid, int _environmentid, int _addressid)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@classid", _classid);
            arParams[1] = new SqlParameter("@environmentid", _environmentid);
            arParams[2] = new SqlParameter("@addressid", _addressid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteSolutionLocations", arParams);
        }
    }
}
