using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;

namespace NCC.ClearView.Application.Core
{
	public class Templates
	{
		private string dsn = "";
		private int user = 0;
		private bool logging = false;
		private SqlParameter[] arParams;
		private Log oLog;
        public Templates(int _user, string _dsn)
		{
			user = _user;
			dsn = _dsn;
			oLog = new Log(user, dsn);
		}
		public DataSet Get(int _id)
		{
			arParams = new SqlParameter[1];
			arParams[0] = new SqlParameter("@id", _id);
			return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getTemplate", arParams);
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
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getTemplates", arParams);
		}
		public void Add(string _name, string _description, string _path, int _enabled)
		{
			if (logging == true)
                oLog.Add("Add template " + _name);
			arParams = new SqlParameter[4];
			arParams[0] = new SqlParameter("@name", _name);
			arParams[1] = new SqlParameter("@description", _description);
			arParams[2] = new SqlParameter("@path", _path);
			arParams[3] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addTemplate", arParams);
		}
		public void Update(int _id, string _name, string _description, string _path, int _enabled)
		{
			if (logging == true)
                oLog.Add("Update template " + GetName(_id));
			arParams = new SqlParameter[5];
			arParams[0] = new SqlParameter("@id", _id);
			arParams[1] = new SqlParameter("@name", _name);
			arParams[2] = new SqlParameter("@description", _description);
			arParams[3] = new SqlParameter("@path", _path);
			arParams[4] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateTemplate", arParams);
		}
		public void Enable(int _id, int _enabled) 
		{
			if (logging == true) 
			{
				if (_enabled == 1)
					oLog.Add("Enable template " + GetName(_id));
				else
                    oLog.Add("Disable template " + GetName(_id));
			}
			arParams = new SqlParameter[2];
			arParams[0] = new SqlParameter("@id", _id);
			arParams[1] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateTemplateEnabled", arParams);
		}
		public void Delete(int _id)
		{
			if (logging == true)
                oLog.Add("Delete template " + GetName(_id));
			arParams = new SqlParameter[1];
			arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteTemplate", arParams);
		}
	}
}
