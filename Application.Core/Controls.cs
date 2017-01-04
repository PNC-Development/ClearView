using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;

namespace NCC.ClearView.Application.Core
{
	public class Controls
	{
		private string dsn = "";
		private int user = 0;
		private bool logging = false;
		private SqlParameter[] arParams;
		private Log oLog;
		public Controls(int _user, string _dsn)
		{
			user = _user;
			dsn = _dsn;
			oLog = new Log(user, dsn);
		}
		public DataSet Get(int _id)
		{
			arParams = new SqlParameter[1];
			arParams[0] = new SqlParameter("@id", _id);
			return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getControl", arParams);
		}
		public string GetName(int _id)
		{
			string strName = "Unavailable";
			try { strName = Get(_id).Tables[0].Rows[0]["name"].ToString(); }
			catch {}
			return strName;
		}
		public DataSet Gets(int _enabled, int _super)
		{
			arParams = new SqlParameter[2];
			arParams[0] = new SqlParameter("@enabled", _enabled);
			arParams[1] = new SqlParameter("@super", _super);
			return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getControls", arParams);
		}
		public int Add(string _name, string _description, string _path, int _super, int _enabled)
		{
			if (logging == true) 
				oLog.Add("Add control " + _name);
			arParams = new SqlParameter[6];
			arParams[0] = new SqlParameter("@name", _name);
			arParams[1] = new SqlParameter("@description", _description);
			arParams[2] = new SqlParameter("@path", _path);
			arParams[3] = new SqlParameter("@super", _super);
			arParams[4] = new SqlParameter("@enabled", _enabled);
            arParams[5] = new SqlParameter("@id", SqlDbType.Int);
            arParams[5].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addControl", arParams);
            return Int32.Parse(arParams[5].Value.ToString());
		}
		public void Update(int _id, string _name, string _description, string _path, int _super, int _enabled)
		{
			if (logging == true) 
				oLog.Add("Update control " + GetName(_id));
			arParams = new SqlParameter[6];
			arParams[0] = new SqlParameter("@id", _id);
			arParams[1] = new SqlParameter("@name", _name);
			arParams[2] = new SqlParameter("@description", _description);
			arParams[3] = new SqlParameter("@path", _path);
			arParams[4] = new SqlParameter("@super", _super);
			arParams[5] = new SqlParameter("@enabled", _enabled);
			SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateControl", arParams);
		}
		public void Enable(int _id, int _enabled) 
		{
			if (logging == true) 
			{
				if (_enabled == 1)
					oLog.Add("Enable control " + GetName(_id));
				else
					oLog.Add("Disable control " + GetName(_id));
			}
			arParams = new SqlParameter[2];
			arParams[0] = new SqlParameter("@id", _id);
			arParams[1] = new SqlParameter("@enabled", _enabled);
			SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateControlEnabled", arParams);
		}
		public void Delete(int _id)
		{
			if (logging == true) 
				oLog.Add("Delete control " + GetName(_id));
			arParams = new SqlParameter[1];
			arParams[0] = new SqlParameter("@id", _id);
			SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteControl", arParams);
		}
	}
}
