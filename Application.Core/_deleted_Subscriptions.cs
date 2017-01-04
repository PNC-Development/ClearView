using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;

namespace NCC.ClearView.Application.Core
{
    public class _deleted_Subscriptions
	{
		private string dsn = "";
		private int user = 0;
		private SqlParameter[] arParams;
        public _deleted_Subscriptions(int _user, string _dsn)
		{
			user = _user;
			dsn = _dsn;
		}
        public void Add(string _Title, string _Description, string _Version, string _Category, int _Author)
		{
			arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@Title", _Title);
            arParams[1] = new SqlParameter("@Description", _Description);
            arParams[2] = new SqlParameter("@Version", _Version);
            arParams[3] = new SqlParameter("@Category", _Category);
            arParams[4] = new SqlParameter("@Author", _Author);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addSubscription", arParams);
		}
    }
}
