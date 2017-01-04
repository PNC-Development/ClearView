using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;

namespace NCC.ClearView.Application.Core
{
	public class Permissions
	{
		private string dsn = "";
		private int user = 0;
		private bool logging = false;
		private SqlParameter[] arParams;
		private DataSet ds;
		private Users oUser;
		private Roles oRole;
		private Applications oApplication;
        private Groups oGroup;
		private Log oLog;
		public Permissions(int _user, string _dsn)
		{
			user = _user;
			dsn = _dsn;
			oLog = new Log(user, dsn);
			oUser = new Users(user, dsn);
			oRole = new Roles(user, dsn);
            oApplication = new Applications(user, dsn);
            oGroup = new Groups(user, dsn);
		}
        public DataSet Get(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getPermission", arParams);
        }
        public DataSet Gets(int _applicationid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@applicationid", _applicationid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getPermissions", arParams);
        }
        public string GetName(int _id)
        {
            string strName = "Unavailable";
            try {
                ds = Get(_id);
                strName = "Application: " + oApplication.GetName(Int32.Parse(ds.Tables[0].Rows[0]["applicationid"].ToString())) + " with Group: " + oGroup.GetName(Int32.Parse(ds.Tables[0].Rows[0]["groupid"].ToString()));
            }
            catch { }
            return strName;
        }
        public void Add(int _applicationid, int _groupid, int _permission)
		{
			if (logging == true)
                oLog.Add("Add permission - Application: " + oApplication.GetName(_applicationid) + " with Group: " + oGroup.GetName(_groupid));
			arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@applicationid", _applicationid);
            arParams[1] = new SqlParameter("@groupid", _groupid);
            arParams[2] = new SqlParameter("@permission", _permission);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addPermission", arParams);
		}
        public void Update(int _id, int _applicationid, int _groupid, int _permission)
        {
            if (logging == true)
                oLog.Add("Update permission - Application: " + oApplication.GetName(_applicationid) + " with Group: " + oGroup.GetName(_groupid));
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@groupid", _groupid);
            arParams[2] = new SqlParameter("@permission", _permission);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updatePermission", arParams);
        }
        public void Delete(int _id)
		{
            if (logging == true)
                oLog.Add("Delete permission - " + GetName(_id));
			arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deletePermission", arParams);
		}
	}
}
