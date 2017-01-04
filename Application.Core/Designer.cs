using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;

namespace NCC.ClearView.Application.Core
{
	public class Designer
	{
		private string dsn = "";
		private int user = 0;
		private SqlParameter[] arParams;
        public Designer(int _user, string _dsn)
		{
			user = _user;
			dsn = _dsn;
		}
		public void Add(int _userid, int _controlid, int _display, int _enabled)
		{
			arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@userid", _userid);
            arParams[1] = new SqlParameter("@controlid", _controlid);
            arParams[2] = new SqlParameter("@display", _display);
            arParams[3] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addDesigner", arParams);
		}
        public DataSet Get(int _userid, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@userid", _userid);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDesigner", arParams);
        }
        public void Delete(int _userid)
		{
			arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@userid", _userid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteDesigner", arParams);
		}
    }
}
