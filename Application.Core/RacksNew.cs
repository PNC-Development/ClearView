using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;

namespace NCC.ClearView.Application.Core
{
	public class RacksNew
	{
		private string dsn = "";
		private int user = 0;
		private SqlParameter[] arParams;

        public RacksNew(int user, string _dsn)
		{
			user = user;
			dsn = _dsn;
		}

        public DataSet Gets(int _rackid, string _rack, int _roomid, int _locationid, int _enabled)
        {
            arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@RackId", _rackid);
            arParams[1] = new SqlParameter("@Rack", _rack);
            arParams[2] = new SqlParameter("@RoomId", _roomid);
            arParams[3] = new SqlParameter("@LocationId", _locationid);
            arParams[4] = new SqlParameter("@Enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getRacksNew", arParams);
        }

        public DataSet Gets(int _roomid, int _locationid, int _enabled)
        {
            arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@RoomId", _roomid);
            arParams[1] = new SqlParameter("@LocationId", _locationid);
            arParams[2] = new SqlParameter("@Enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getRacksNew", arParams);
        }
        public DataSet Gets(int _zoneid, string _rack)
        {
            arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@ZoneId", _zoneid);
            arParams[1] = new SqlParameter("@Rack", _rack);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getRacksNew", arParams);
        }
        public DataSet Gets(int _zoneid, int _enabled)
        {
            arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@ZoneId", _zoneid);
            arParams[1] = new SqlParameter("@Enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getRacksNew", arParams);
        }
        public DataSet GetByZone(int _zoneid)
        {
            arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@ZoneId", _zoneid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getRacksNew", arParams);
        }
        public DataSet Gets(int _rackid)
        {
            arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@RackId", _rackid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getRacksNew", arParams);
        }
        public string Get(int _rackid, string _column)
        {
            DataSet ds = Gets(_rackid);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }

        public int Add(string _rack, int _assetid, int _zoneid, string _description, int _enabled)
		{
			arParams = new SqlParameter[10];
            arParams[0] = new SqlParameter("@RackId", 0);
            arParams[1] = new SqlParameter("@Rack", _rack);
            arParams[2] = new SqlParameter("@AssetId", _assetid);
            arParams[3] = new SqlParameter("@ZoneId", _zoneid);
            arParams[4] = new SqlParameter("@Description", _description);
            arParams[5] = new SqlParameter("@CreatedBy", user);
            arParams[6] = new SqlParameter("@Enabled", _enabled);

            arParams[0].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addRackNew", arParams);
            return Int32.Parse(arParams[0].Value.ToString());

		}

        public void Update(int _rackid, string _rack, int _assetid, int _zoneid, string _description, int _enabled)
        {
            arParams = new SqlParameter[10];
            arParams[0] = new SqlParameter("@RackId", _rackid);
            arParams[1] = new SqlParameter("@Rack", _rack);
            arParams[2] = new SqlParameter("@AssetId", _assetid);
            arParams[3] = new SqlParameter("@ZoneId", _zoneid);
            arParams[4] = new SqlParameter("@Description", _description);
            arParams[5] = new SqlParameter("@ModifiedBy", user);
            arParams[6] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateRackNew", arParams);
        }
        
        public void Enable(int _rackid, int _enabled)
        {
            arParams = new SqlParameter[10];
            arParams[0] = new SqlParameter("@RackId", _rackid);
            arParams[1] = new SqlParameter("@Enabled", _enabled);
            arParams[2] = new SqlParameter("@ModifiedBy", user);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateRackNewEnabled", arParams);
        }

        public void Delete(int _rackid, int _deleted)
        {
            arParams = new SqlParameter[10];
            arParams[0] = new SqlParameter("@RackId", _rackid);
            arParams[1] = new SqlParameter("@Deleted", _deleted);
            arParams[2] = new SqlParameter("@ModifiedBy", user);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateRackNewDeleted", arParams);
        }
    }
}
