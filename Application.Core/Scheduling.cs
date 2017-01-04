using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;

namespace NCC.ClearView.Application.Core
{
    public class Scheduling
    {
        private string dsn = "";
        private int user = 0;
        private SqlParameter[] arParams;
        public Scheduling(int _user, string _dsn)
        {
            user = _user;
            dsn = _dsn;
        }
        public DataSet GetSch(DateTime _start, DateTime _end, int _schd_id)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@start_date", _start);
            if (_end == DateTime.MinValue)
                arParams[1] = new SqlParameter("@end_date", DBNull.Value);
            else
                arParams[1] = new SqlParameter("@end_date", _end);

            arParams[2] = new SqlParameter("@schd_id", _schd_id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getSchedulings", arParams);
        }
        public DataSet GetSchUsers(int _schd_id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@schd_id", _schd_id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getSchedulingUsers", arParams);
        }
        public int RegisterUser(int _id, string _lname, string _fname, string _phone, string _dept, int _schd_id)
        {
            arParams = new SqlParameter[7];
            arParams[0] = new SqlParameter("@profile_id", _id);
            arParams[1] = new SqlParameter("@lname", _lname);
            arParams[2] = new SqlParameter("@fname", _fname);
            arParams[3] = new SqlParameter("@phone", _phone);
            arParams[4] = new SqlParameter("@dept", _dept);
            arParams[5] = new SqlParameter("@schd_id", _schd_id);
            arParams[6] = new SqlParameter("@id", SqlDbType.Int);
            arParams[6].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addSchedulingUser", arParams);
            return Int32.Parse(arParams[6].Value.ToString());

        }
        public int UnregisterUser(int _id, int _schd_id)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@profile_id", _id);
            arParams[1] = new SqlParameter("@schd_id", _schd_id);
            arParams[2] = new SqlParameter("@row_ct", SqlDbType.Int);
            arParams[2].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteSchedulingUser", arParams);
            return Int32.Parse(arParams[2].Value.ToString());
        }
        public DataSet GetDeptName(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@userid", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getSchedulingApplication", arParams);
        }
        public int VerifyUser(int _id, int _schd_id)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@profile_id", _id);
            arParams[1] = new SqlParameter("@schd_id", _schd_id);
            arParams[2] = new SqlParameter("@is_registered", SqlDbType.Int);
            arParams[2].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_getSchedulingUser", arParams);
            return Int32.Parse(arParams[2].Value.ToString());
        }

        public DataSet Get(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@schd_id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getSchedule", arParams);
        }
        public DataSet Gets()
        {
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getSchedules");
        }
        public void Add(string _event, string _facilitator, string _netmeeting, string _confline, string _passcode, DateTime _date_sch, string _start_time, string _end_time, int _max_people, string _location)
        {
            arParams = new SqlParameter[10];
            arParams[0] = new SqlParameter("@event", _event);
            arParams[1] = new SqlParameter("@facilitator", _facilitator);
            arParams[2] = new SqlParameter("@netmeeting", _netmeeting);
            arParams[3] = new SqlParameter("@confline", _confline);
            arParams[4] = new SqlParameter("@passcode", _passcode);
            arParams[5] = new SqlParameter("@date_sch", _date_sch);
            arParams[6] = new SqlParameter("@start_time", _start_time);
            arParams[7] = new SqlParameter("@end_time", _end_time);
            arParams[8] = new SqlParameter("@max_people", _max_people);
            arParams[9] = new SqlParameter("@location", _location);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addSchedule", arParams);
        }
        public void Update(int _id, string _event, string _facilitator, string _netmeeting, string _confline, string _passcode, DateTime _date_sch, string _start_time, string _end_time, int _max_people, string _location)
        {
            arParams = new SqlParameter[11];
            arParams[0] = new SqlParameter("@schd_id", _id);
            arParams[1] = new SqlParameter("@event", _event);
            arParams[2] = new SqlParameter("@facilitator", _facilitator);
            arParams[3] = new SqlParameter("@netmeeting", _netmeeting);
            arParams[4] = new SqlParameter("@confline", _confline);
            arParams[5] = new SqlParameter("@passcode", _passcode);
            arParams[6] = new SqlParameter("@date_sch", _date_sch);
            arParams[7] = new SqlParameter("@start_time", _start_time);
            arParams[8] = new SqlParameter("@end_time", _end_time);
            arParams[9] = new SqlParameter("@max_people", _max_people);
            arParams[10] = new SqlParameter("@location", _location);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateSchedule", arParams);
        }
        public void Delete(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@schd_id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteSchedule", arParams);
        }
    }
}
