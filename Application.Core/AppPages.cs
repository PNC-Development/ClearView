using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;

namespace NCC.ClearView.Application.Core
{
	public class AppPages
	{
		private string dsn = "";
		private int user = 0;
		private bool logging = false;
		private SqlParameter[] arParams;
		private Log oLog;
        private Pages oPage;
        private Applications oApplication;
        public AppPages(int _user, string _dsn)
		{
			user = _user;
			dsn = _dsn;
			oLog = new Log(user, dsn);
            oPage = new Pages(user, dsn);
            oApplication = new Applications(user, dsn);
		}
		public void Add(int _pageid, int _applicationid)
		{
			if (logging == true) 
				oLog.Add("Add application page " + oPage.GetName(_pageid) + " and application " + oApplication.GetName(_applicationid));
			arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@pageid", _pageid);
            arParams[1] = new SqlParameter("@applicationid", _applicationid);
			SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addAppPage", arParams);
		}
        public DataSet Get(int _pageid, int _applicationid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@pageid", _pageid);
            arParams[1] = new SqlParameter("@applicationid", _applicationid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getAppPage", arParams);
        }
        public DataSet Gets(int _applicationid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@applicationid", _applicationid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getAppPages", arParams);
        }
        public DataSet Gets(int _applicationid, int _parent, int _link, int _enabled)
        {
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@applicationid", _applicationid);
            arParams[1] = new SqlParameter("@parent", _parent);
            arParams[2] = new SqlParameter("@link", _link);
            arParams[3] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getAppPagesTree", arParams);
        }
        public void Delete(int _parent, int _applicationid)
		{
			arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@parent", _parent);
            arParams[1] = new SqlParameter("@applicationid", _applicationid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteAppPageParent", arParams);
		}
        public void Delete(int _applicationid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@applicationid", _applicationid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteAppPages", arParams);
        }
        public void DeletePage(int _pageid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@pageid", _pageid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteAppPage", arParams);
        }
    }
}
