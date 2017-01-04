using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using System.IO;

namespace NCC.ClearView.Application.Core
{
	public class DidYouKnow
	{
		private string dsn = "";
		private int user = 0;
		private SqlParameter[] arParams;
        public DidYouKnow(int _user, string _dsn)
		{
			user = _user;
			dsn = _dsn;
		}
        public DataSet Get(int _id)
		{
			arParams = new SqlParameter[1];
			arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDidYouKnow", arParams);
		}
        public DataSet Gets()
		{
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDidYouKnows");
		}
        public void Add(string _description)
        {
			arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@description", _description);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addDidYouKnow", arParams);
		}
        public void Update(int _id, string _description)
		{
			arParams = new SqlParameter[2];
			arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@description", _description);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateDidYouKnow", arParams);
		}
        public void Delete(int _id)
		{
			arParams = new SqlParameter[1];
			arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteDidYouKnow", arParams);
		}
    }
}
