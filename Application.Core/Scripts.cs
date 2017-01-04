using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;

namespace NCC.ClearView.Application.Core
{
	public class Scripts
	{
		private string dsn = "";
		private int user = 0;
		private SqlParameter[] arParams;
        public Scripts(int _user, string _dsn)
		{
			user = _user;
			dsn = _dsn;
		}
        public void Add(string _script, int _environment)
		{
			arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@script", _script);
            arParams[1] = new SqlParameter("@environment", _environment);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addScript", arParams);
		}
        public void Update(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateScript", arParams);
        }
    }
}
