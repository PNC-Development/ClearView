using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;

namespace NCC.ClearView.Application.Core
{
	public class Zones
	{
		private string dsn = "";
		private int user = 0;
		private SqlParameter[] arParams;

        public Zones(int user, string _dsn)
		{
			user = user;
			dsn = _dsn;
		}

        public DataSet Gets(int _zoneId, string _zone, int _roomId, int _enabled)
        {
            arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@zoneId", _zoneId);
            arParams[1] = new SqlParameter("@zone", _zone);
            arParams[2] = new SqlParameter("@roomId", _roomId);
            arParams[3] = new SqlParameter("@Enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getZones", arParams);
        }

        public DataSet Gets( int _roomId,string _zone )
        {
            arParams = new SqlParameter[5];
            arParams[1] = new SqlParameter("@zone", _zone);
            arParams[2] = new SqlParameter("@roomId", _roomId);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getZones", arParams);
        }


        public DataSet Gets(int _roomId, int _enabled)
        {
            arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@roomId", _roomId);
            arParams[1] = new SqlParameter("@Enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getZones", arParams);
        }

        public DataSet GetByRoom(int _roomId)
        {
            arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@roomId", _roomId);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getZones", arParams);
        }


        public DataSet Gets(int _zoneId)
        {
            arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@zoneId", _zoneId);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getZones", arParams);
        }
        public string Get(int _zoneId, string _column)
        {
            DataSet ds = Gets(_zoneId);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }

        public int Add(string _zone,int _roomId, int _vLan,string _description,int _enabled)
		{
			arParams = new SqlParameter[10];
            arParams[0] = new SqlParameter("@zoneId", 0);
            arParams[1] = new SqlParameter("@zone", _zone);
            arParams[2] = new SqlParameter("@roomId", _roomId);
            arParams[4] = new SqlParameter("@vLan", _vLan);
            arParams[5] = new SqlParameter("@Description", _description);
            arParams[6] = new SqlParameter("@CreatedBy", user);
            arParams[7] = new SqlParameter("@Enabled", _enabled);

            arParams[0].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addZone", arParams);
            return Int32.Parse(arParams[0].Value.ToString());

		}

        public void Update(int _zoneId, string _zone, int _roomId, int _vLan, string _description, int _enabled)
        {
            arParams = new SqlParameter[10];
            arParams[0] = new SqlParameter("@zoneId", _zoneId);
            arParams[1] = new SqlParameter("@zone", _zone);
            arParams[2] = new SqlParameter("@roomId", _roomId);
            arParams[3] = new SqlParameter("@vLan", _vLan);
            arParams[4] = new SqlParameter("@Description", _description);
            arParams[5] = new SqlParameter("@ModifiedBy", user);
            arParams[6] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateZone", arParams);
        }
        
        public void Enable(int _zoneId, int _enabled)
        {
            arParams = new SqlParameter[10];
            arParams[0] = new SqlParameter("@zoneId", _zoneId);
            arParams[1] = new SqlParameter("@Enabled", _enabled);
            arParams[2] = new SqlParameter("@ModifiedBy", user);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateZoneEnabled", arParams);
        }

        public void Delete(int _zoneId, int _deleted)
        {
            arParams = new SqlParameter[10];
            arParams[0] = new SqlParameter("@zoneId", _zoneId);
            arParams[1] = new SqlParameter("@Deleted", _deleted);
            arParams[2] = new SqlParameter("@ModifiedBy", user);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateZoneDeleted", arParams);
        }
    }
}
