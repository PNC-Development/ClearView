using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using System.Data.SqlTypes;

namespace NCC.ClearView.Application.Core
{
	public class ResourceScheduling
	{
		private string dsn = "";
		private int user = 0;
		private SqlParameter[] arParams;
        public ResourceScheduling(int _user, string _dsn)
		{
			user = _user;
			dsn = _dsn;
		}

        public DataSet GetResourceSchedulingNormalView(SqlDateTime _start_date,SqlDateTime _end_date)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@start_date", _start_date);
            arParams[1] = new SqlParameter("@end_date", _end_date);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getResourceScheduling",arParams);
        }

        public DataSet GetResourceSchedulingTodayView(SqlDateTime _today)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@today", _today);             
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getResourceSchedulingToday", arParams);
        }       
        
        public int Add(int _userid, string _title, SqlDateTime _start_date, SqlDateTime _end_date, string _start_time, string _end_time)
		{
           
			arParams = new SqlParameter[7];
            arParams[0] = new SqlParameter("@userid", _userid);
            arParams[1] = new SqlParameter("@title", _title);
            arParams[2] = new SqlParameter("@start_date", _start_date);
            arParams[3] = new SqlParameter("@end_date", _end_date);
            arParams[4] = new SqlParameter("@start_time", _start_time);
            arParams[5] = new SqlParameter("@end_time", _end_time);
            arParams[6] = new SqlParameter("@error",SqlDbType.Int);
            arParams[6].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addResourceScheduling", arParams);
            return Int32.Parse(arParams[6].Value.ToString());
		}      
       
    }
}
