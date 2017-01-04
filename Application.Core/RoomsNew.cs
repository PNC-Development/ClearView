using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;

namespace NCC.ClearView.Application.Core
{
	public class RoomsNew
	{
		private string dsn = "";
		private int user = 0;
		private SqlParameter[] arParams;

        public RoomsNew(int user, string _dsn)
		{
			user = user;
			dsn = _dsn;
		}

        public DataSet Gets(int _roomid, string _room, int _locationid, int _enabled)
        {
            arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@RoomId", _roomid);
            arParams[1] = new SqlParameter("@Room", _room);
            arParams[2] = new SqlParameter("@LocationId", _locationid);
            arParams[3] = new SqlParameter("@Enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getRoomsNew", arParams);
        }

        public DataSet Gets(int _locationid, string _room)
        {
            arParams = new SqlParameter[5];
            arParams[1] = new SqlParameter("@Room", _room);
            arParams[2] = new SqlParameter("@LocationId", _locationid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getRoomsNew", arParams);
        }


        public DataSet Gets(int _locationid, int _enabled)
        {
            arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@LocationId", _locationid);
            arParams[1] = new SqlParameter("@Enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getRoomsNew", arParams);
        }

        public DataSet GetByLocation(int _locationid)
        {
            arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@LocationId", _locationid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getRoomsNew", arParams);
        }


        public DataSet Gets(int _roomid)
        {
            arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@RoomId", _roomid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getRoomsNew", arParams);
        }
        public string Get(int _roomid, string _column)
        {
            DataSet ds = Gets(_roomid);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }

        public int Add(string _room,int _locationid, string _description,int _enabled)
		{
			arParams = new SqlParameter[10];
            arParams[0] = new SqlParameter("@RoomId", 0);
            arParams[1] = new SqlParameter("@Room", _room);
            arParams[2] = new SqlParameter("@LocationId", _locationid);
            arParams[3] = new SqlParameter("@Description", _description);
            arParams[4] = new SqlParameter("@CreatedBy", user);
            arParams[5] = new SqlParameter("@Enabled", _enabled);

            arParams[0].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addRoomNew", arParams);
            return Int32.Parse(arParams[0].Value.ToString());

		}

        public void Update(int _roomid, string _room, int _locationid,string _description, int _enabled)
        {
            arParams = new SqlParameter[10];
            arParams[0] = new SqlParameter("@RoomId", _roomid);
            arParams[1] = new SqlParameter("@Room", _room);
            arParams[2] = new SqlParameter("@LocationId", _locationid);
            arParams[3] = new SqlParameter("@Description", _description);
            arParams[4] = new SqlParameter("@ModifiedBy", user);
            arParams[5] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateRoomNew", arParams);
        }
        
        public void Enable(int _roomid, int _enabled)
        {
            arParams = new SqlParameter[10];
            arParams[0] = new SqlParameter("@RoomId", _roomid);
            arParams[1] = new SqlParameter("@Enabled", _enabled);
            arParams[2] = new SqlParameter("@ModifiedBy", user);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateRoomNewEnabled", arParams);
        }

        public void Delete(int _roomid, int _deleted)
        {
            arParams = new SqlParameter[10];
            arParams[0] = new SqlParameter("@RoomId", _roomid);
            arParams[1] = new SqlParameter("@Deleted", _deleted);
            arParams[2] = new SqlParameter("@ModifiedBy", user);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateRoomNewDeleted", arParams);
        }
    }
}
