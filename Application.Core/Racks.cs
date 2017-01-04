using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;

namespace NCC.ClearView.Application.Core
{
	public class Racks
	{
		private string dsn = "";
		private int user = 0;
		private SqlParameter[] arParams;
        public Racks(int _user, string _dsn)
		{
			user = _user;
			dsn = _dsn;
		}
        public int Get(string _name)
        {
            DataSet ds = SqlHelper.ExecuteDataset(dsn, CommandType.Text, "SELECT * FROM cv_racks WHERE name = '" + _name + "' AND enabled = 1 AND deleted = 0");
            if (ds.Tables[0].Rows.Count == 0)
            {
                Add(_name, 1);
                return Get(_name);
            }
            else
                return Int32.Parse(ds.Tables[0].Rows[0]["id"].ToString());
        }
        public DataSet Gets(int _enabled)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getRacks", arParams);
        }
        public DataSet Get(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getRack", arParams);
        }

       

        public string Get(int _id, string _column)
        {
            DataSet ds = Get(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public void Add(string _name, int _enabled)
		{
        	arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@name", _name);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addRack", arParams);
		}

        public void Add(int _assetid, string _name, int _roomid, int _enabled, int _createdby)
        {
         
            arParams = new SqlParameter[6];
            arParams[0] = new SqlParameter("@assetid", _assetid);
            arParams[1] = new SqlParameter("@name", _name);
            arParams[2] = new SqlParameter("@roomid", _roomid);
            arParams[3] = new SqlParameter("@enabled", _enabled);
            arParams[4] = new SqlParameter("@createdby", _createdby);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addRack", arParams);
        }

        public void Update(int _id, string _name, int _enabled)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@name", _name);
            arParams[2] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateRack", arParams);
        }
        public void Update(int _id, int _assetid, string _name, int _roomid, int _enabled, int _modifiedby)
        {
            arParams = new SqlParameter[6];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@assetid", _assetid);
            arParams[2] = new SqlParameter("@name", _name);
            arParams[3] = new SqlParameter("@roomid", _roomid);
            arParams[4] = new SqlParameter("@enabled", _enabled);
            arParams[5] = new SqlParameter("@modifiedby", _modifiedby);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateRack", arParams);
        }
        public void UpdateOrder(int _id, int _display)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@display", _display);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateRackOrder", arParams);
        }
        public void Enable(int _id, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateRackEnabled", arParams);
        }
        public void Delete(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteRack", arParams);
        }
    }
}
