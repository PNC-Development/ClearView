using System;
using System.DirectoryServices;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using System.Data.SqlTypes;

namespace NCC.ClearView.Application.Core
{
	public class ADObject
	{
        private string dsn = "";
        private int user = 0;
        private SqlParameter[] arParams;
        public ADObject(int _user, string _dsn)
		{
            user = _user;
            dsn = _dsn;
		}
        public void AddDomainAccount(int _requestid, int _environment, string _name, string _path, string _reason, string _objectdate)
        {
            arParams = new SqlParameter[6];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@environment", _environment);
            arParams[2] = new SqlParameter("@name", _name);
            arParams[3] = new SqlParameter("@path", _path);
            arParams[4] = new SqlParameter("@reason", _reason);
            arParams[5] = new SqlParameter("@objectdate", (_objectdate == "" ? SqlDateTime.Null : DateTime.Parse(_objectdate)));
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addAdObject", arParams);
        }
        public DataSet GetDomainAccounts()
        {
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getADObjectsAll");
        }
        public DataSet GetDomainAccounts(int _requestid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getADObjects", arParams);
        }
        public void UpdateDomainAccount(int _adid, int _done_by)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@adid", _adid);
            arParams[1] = new SqlParameter("@done_by", _done_by);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateAdObject", arParams);
        }
        public DataSet GetDomainAccountsDisabled(DateTime _date)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@date", _date);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getADObjectsDisabled", arParams);
        }
        public void UpdateDomainAccountDisabled(int _adid, int _disabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@adid", _adid);
            arParams[1] = new SqlParameter("@disabled", _disabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateAdObjectDisabled", arParams);
        }
    }
}
