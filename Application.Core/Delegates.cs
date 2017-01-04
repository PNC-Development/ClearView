using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;

namespace NCC.ClearView.Application.Core
{
	public class Delegates
	{
		private string dsn = "";
		private int user = 0;
		private SqlParameter[] arParams;
        public Delegates(int _user, string _dsn)
		{
			user = _user;
			dsn = _dsn;
		}
        public void Add(int _userid, int _delegate, int _rights)
		{
			arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@userid", _userid);
            arParams[1] = new SqlParameter("@delegate", _delegate);
            arParams[2] = new SqlParameter("@rights", _rights);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addDelegate", arParams);
		}
        public void Delete(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteDelegate", arParams);
        }
        public DataSet Gets(int _userid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@userid", _userid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDelegates", arParams);
        }
        public DataSet GetDelegations(int _delegate)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@delegate", _delegate);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDelegations", arParams);
        }
        public int Get(int _userid, int _delegate)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@userid", _userid);
            arParams[1] = new SqlParameter("@delegate", _delegate);
            DataSet ds = SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDelegate", arParams);
            if (ds.Tables[0].Rows.Count > 0)
                return Int32.Parse(ds.Tables[0].Rows[0]["rights"].ToString());
            else
                return 0;
        }
    }
}
