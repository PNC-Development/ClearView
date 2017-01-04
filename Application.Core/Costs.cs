using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;

namespace NCC.ClearView.Application.Core
{
	public class Costs
	{
		private string dsn = "";
		private int user = 0;
		private bool logging = false;
		private SqlParameter[] arParams;
		private Log oLog;
        public Costs(int _user, string _dsn)
		{
			user = _user;
			dsn = _dsn;
			oLog = new Log(user, dsn);
		}
		public DataSet Get(int _id)
		{
			arParams = new SqlParameter[1];
			arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getCost", arParams);
		}
		public string GetName(int _id)
		{
			string strName = "Unavailable";
			try { strName = Get(_id).Tables[0].Rows[0]["name"].ToString(); }
			catch {}
			return strName;
		}
		public DataSet Gets(int _enabled)
		{
			arParams = new SqlParameter[1];
			arParams[0] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getCosts", arParams);
		}
		public void Add(string _name, int _enabled)
		{
			if (logging == true)
                oLog.Add("Add cost " + _name);
			arParams = new SqlParameter[2];
			arParams[0] = new SqlParameter("@name", _name);
			arParams[1] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addCost", arParams);
		}
        public void Update(int _id, string _name, int _enabled)
		{
			if (logging == true)
                oLog.Add("Update cost " + GetName(_id));
			arParams = new SqlParameter[3];
			arParams[0] = new SqlParameter("@id", _id);
			arParams[1] = new SqlParameter("@name", _name);
            arParams[2] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateCost", arParams);
		}
        public void UpdateOrder(int _id, int _display)
        {
            if (logging == true)
                oLog.Add("Update cost order " + GetName(_id));
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@display", _display);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateCostOrder", arParams);
        }
		public void Enable(int _id, int _enabled) 
		{
			if (logging == true) 
			{
				if (_enabled == 1)
                    oLog.Add("Enable cost " + GetName(_id));
				else
                    oLog.Add("Disable cost " + GetName(_id));
			}
			arParams = new SqlParameter[2];
			arParams[0] = new SqlParameter("@id", _id);
			arParams[1] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateCostEnabled", arParams);
		}
		public void Delete(int _id)
		{
			if (logging == true)
                oLog.Add("Delete cost " + GetName(_id));
			arParams = new SqlParameter[1];
			arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteCost", arParams);
		}
	}
}
