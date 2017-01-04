using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using System.Text;

namespace NCC.ClearView.Application.Core
{
	public class CostCenter
	{
		private string dsn = "";
		private int user = 0;
		private SqlParameter[] arParams;
        public CostCenter(int _user, string _dsn)
		{
			user = _user;
			dsn = _dsn;
		}
        public DataSet Get(string _name)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@name", _name);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getCostCentersAJAX", arParams);
        }
        public void Add(string _GLCompCostCenter, string _IMPRCompanyCode, string _IMPRCompanyCostCenter)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@GLCompCostCenter", _GLCompCostCenter);
            arParams[1] = new SqlParameter("@IMPRCompanyCode", _IMPRCompanyCode);
            arParams[2] = new SqlParameter("@IMPRCompanyCostCenter", _IMPRCompanyCostCenter);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addCostCenter", arParams);
        }
        public void Delete()
        {
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteCostCenters");
        }
        public DataSet Get(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getCostCenter", arParams);
        }
        private string Get(int _id, string _column)
        {
            DataSet ds = Get(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public string GetName(int _id)
        {
            return Get(_id, "name");
        }
    }
}
