using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;

namespace NCC.ClearView.Application.Core
{
    public class Rotator
	{
		private string dsn = "";
		private int user = 0;
		private SqlParameter[] arParams;
        public Rotator(int _user, string _dsn)
		{
			user = _user;
			dsn = _dsn;
		}
        public DataSet GetHeaders(int _enabled)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getRotatorHeaders", arParams);
        }
        public DataSet GetHeader(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getRotatorHeader", arParams);
        }
        public string GetHeader(int _id, string _column)
        {
            DataSet ds = GetHeader(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public void AddHeader(string _imageurl, int _impressions, int _enabled)
		{
			arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@imageurl", _imageurl);
            arParams[1] = new SqlParameter("@impressions", _impressions);
            arParams[2] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addRotatorHeader", arParams);
		}
        public void UpdateHeader(int _id, string _imageurl, int _impressions, int _enabled)
        {
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@imageurl", _imageurl);
            arParams[2] = new SqlParameter("@impressions", _impressions);
            arParams[3] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateRotatorHeader", arParams);
        }
        public void EnableHeader(int _id, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateRotatorHeaderEnabled", arParams);
        }
        public void DeleteHeader(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteRotatorHeader", arParams);
        }
    }
}
