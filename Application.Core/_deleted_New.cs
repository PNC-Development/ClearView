using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;

namespace NCC.ClearView.Application.Core
{
    public class New
	{
		private string dsn = "";
		private int user = 0;
		private SqlParameter[] arParams;
        public New(int _user, string _dsn)
		{
			user = _user;
			dsn = _dsn;
		}
        public DataSet Gets(int _enabled)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getNews", arParams);
        }
        public DataSet Get(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getNew", arParams);
        }
        public string Get(int _id, string _column)
        {
            DataSet ds = Get(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public void Add(string _title, string _description, string _attachment, int _display, int _enabled)
		{
			arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@title", _title);
            arParams[1] = new SqlParameter("@description", _description);
            arParams[2] = new SqlParameter("@attachment", _attachment);
            arParams[3] = new SqlParameter("@display", _display);
            arParams[4] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addNew", arParams);
		}
        public void Update(int _id, string _title, string _description, string _attachment, int _enabled)
        {
            arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@title", _title);
            arParams[2] = new SqlParameter("@description", _description);
            arParams[3] = new SqlParameter("@attachment", _attachment);
            arParams[4] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateNew", arParams);
        }
        public void UpdateOrder(int _id, int _display)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@display", _display);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateNewOrder", arParams);
        }
        public void Enable(int _id, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateNewEnabled", arParams);
        }
        public void Delete(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteNew", arParams);
        }
    }
}
