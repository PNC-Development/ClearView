using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;

namespace NCC.ClearView.Application.Core
{
	public class Orders
	{
		private string dsn = "";
		private int user = 0;
		private SqlParameter[] arParams;
        public Orders(int _user, string _dsn)
		{
			user = _user;
			dsn = _dsn;
		}
		public DataSet Gets(int _platformid)
		{
			arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@platformid", _platformid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getOrders", arParams);
		}
        public DataSet Get(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getOrder", arParams);
        }
        public int Get(string _tracking)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@tracking", _tracking);
            DataSet ds = SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getOrderTracking", arParams);
            if (ds.Tables[0].Rows.Count > 0)
                return Int32.Parse(ds.Tables[0].Rows[0]["id"].ToString());
            else
                return 0;
        }
        public void Add(string _tracking, string _name, int _quantity, int _modelid, int _classid, int _environmentid, int _addressid, int _confidenceid, DateTime _ordered)
		{
			arParams = new SqlParameter[9];
            arParams[0] = new SqlParameter("@tracking", _tracking);
            arParams[1] = new SqlParameter("@name", _name);
            arParams[2] = new SqlParameter("@quantity", _quantity);
            arParams[3] = new SqlParameter("@modelid", _modelid);
            arParams[4] = new SqlParameter("@classid", _classid);
            arParams[5] = new SqlParameter("@environmentid", _environmentid);
            arParams[6] = new SqlParameter("@addressid", _addressid);
            arParams[7] = new SqlParameter("@confidenceid", _confidenceid);
            arParams[8] = new SqlParameter("@ordered", _ordered);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addOrder", arParams);
		}
        public void Update(int _id, string _tracking, string _name, int _quantity, int _modelid, int _classid, int _environmentid, int _addressid, int _confidenceid, DateTime _ordered, int _status, string _comments)
        {
            arParams = new SqlParameter[12];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@tracking", _tracking);
            arParams[2] = new SqlParameter("@name", _name);
            arParams[3] = new SqlParameter("@quantity", _quantity);
            arParams[4] = new SqlParameter("@modelid", _modelid);
            arParams[5] = new SqlParameter("@classid", _classid);
            arParams[6] = new SqlParameter("@environmentid", _environmentid);
            arParams[7] = new SqlParameter("@addressid", _addressid);
            arParams[8] = new SqlParameter("@confidenceid", _confidenceid);
            arParams[9] = new SqlParameter("@ordered", _ordered);
            arParams[10] = new SqlParameter("@status", _status);
            arParams[11] = new SqlParameter("@comments", _comments);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateOrder", arParams);
        }
        public void UpdateReceived(string _tracking, int _quantity)
        {
            int intOrder = Get(_tracking);
            DataSet ds = Get(intOrder);
            if (ds.Tables[0].Rows.Count > 0)
            {
                int intReceived = Int32.Parse(ds.Tables[0].Rows[0]["received"].ToString());
                intReceived += _quantity;
                arParams = new SqlParameter[2];
                arParams[0] = new SqlParameter("@id", intOrder);
                arParams[1] = new SqlParameter("@received", intReceived);
                SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateOrderReceived", arParams);
            }
        }
        public void UpdateShow(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateOrderShow", arParams);
        }
        public void Delete(int _id)
		{
			arParams = new SqlParameter[1];
			arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteOrder", arParams);
		}
	}
}
