using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using System.Data.SqlTypes;

namespace NCC.ClearView.Application.Core
{
	public class ProjectNumber
	{
		private string dsn = "";
		private int user = 0;
		private SqlParameter[] arParams;
        public ProjectNumber(int _user, string _dsn)
		{
			user = _user;
			dsn = _dsn;
		}
        public int Add()
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addProjectNumber", arParams);
            return Int32.Parse(arParams[0].Value.ToString());
        }
        public string New()
        {
            int _id = Add();
            return "CV" + _id.ToString();
        }
    }
}
