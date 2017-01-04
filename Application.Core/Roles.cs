using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;

namespace NCC.ClearView.Application.Core
{
	public class Roles
	{
		private string dsn = "";
		private int user = 0;
		private bool logging = false;
		private SqlParameter[] arParams;
		private Log oLog;
        private Groups oGroup;
        private Users oUser;
		public Roles(int _user, string _dsn)
		{
			user = _user;
			dsn = _dsn;
			oLog = new Log(user, dsn);
            oGroup = new Groups(user, dsn);
            oUser = new Users(user, dsn);
		}
		public DataSet Get(int _id)
		{
			arParams = new SqlParameter[1];
			arParams[0] = new SqlParameter("@id", _id);
			return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getRole", arParams);
		}
        public DataSet GetGroup(int _groupid)
		{
			arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@groupid", _groupid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getRoleUsers", arParams);
		}
        public DataSet GetUser(int _userid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@userid", _userid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getRoleGroups", arParams);
        }
        public DataSet Gets(int _userid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@userid", _userid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getRoles", arParams);
        }
        public int Get(int _userid, int _applicationid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@userid", _userid);
            arParams[1] = new SqlParameter("@applicationid", _applicationid);
            object o = SqlHelper.ExecuteScalar(dsn, CommandType.StoredProcedure, "pr_getRoleApplication", arParams);
            if (o != null && o.ToString() != "")
                return Int32.Parse(o.ToString());
            else
                return 0;
        }
        public void Add(int _userid, int _groupid)
		{
			if (logging == true)
                oLog.Add("Add Role - User " + oUser.GetName(_userid) + " to the group " + oGroup.GetName(_groupid));
			arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@userid", _userid);
            arParams[1] = new SqlParameter("@groupid", _groupid);
			SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addRole", arParams);
		}
        public void DeleteUser(int _userid)
		{
			arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@userid", _userid);
			SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteRoleGroups", arParams);
		}
        public void DeleteGroup(int _groupid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@groupid", _groupid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteRoleUsers", arParams);
        }
    }
}
