using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;

namespace NCC.ClearView.Application.Core
{
    public class Tokens
	{
		private string dsn = "";
		private int user = 0;
		private SqlParameter[] arParams;
        public Tokens(int _user, string _dsn)
		{
			user = _user;
			dsn = _dsn;
		}
        public bool Get(string _name, string _token)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@name", _name);
            arParams[1] = new SqlParameter("@token", _token);
            DataSet ds = SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getToken", arParams);
            return (ds.Tables[0].Rows.Count == 1);
        }
        public string Add(string _name, int _length)
		{
            Functions oFunction = new Functions(0, dsn, 0);
            string strToken = oFunction.GetRandom(_length);
			arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@name", _name);
            arParams[1] = new SqlParameter("@token", strToken);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addToken", arParams);
            return strToken;
		}
        public void Update(string _name, string _token)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@name", _name);
            arParams[1] = new SqlParameter("@token", _token);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateToken", arParams);
        }
    }
}
