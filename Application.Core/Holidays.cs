using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;

namespace NCC.ClearView.Application.Core
{
	public class Holidays
	{
		private string dsn = "";
		private int user = 0;
		private bool logging = false;
		private SqlParameter[] arParams;
		private Log oLog;
        public Holidays(int _user, string _dsn)
		{
			user = _user;
			dsn = _dsn;
			oLog = new Log(user, dsn);
		}
		public DataSet Get(int _id)
		{
			arParams = new SqlParameter[1];
			arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getHoliday", arParams);
		}
        public DataSet Get(DateTime _date)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@date", _date);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getHolidayDate", arParams);
        }
        public string GetName(int _id)
		{
			string strName = "Unavailable";
			try { strName = Get(_id).Tables[0].Rows[0]["name"].ToString(); }
			catch {}
			return strName;
		}
		public DataSet Gets(int _enabled)
		{
			arParams = new SqlParameter[1];
			arParams[0] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getHolidays", arParams);
		}
		public void Add(string _name, DateTime _happens, int _enabled)
		{
			if (logging == true)
                oLog.Add("Add holiday " + _name);
			arParams = new SqlParameter[3];
			arParams[0] = new SqlParameter("@name", _name);
            arParams[1] = new SqlParameter("@happens", _happens);
			arParams[2] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addHoliday", arParams);
		}
        public void Update(int _id, string _name, DateTime _happens, int _enabled)
		{
			if (logging == true)
                oLog.Add("Update holiday " + GetName(_id));
			arParams = new SqlParameter[4];
			arParams[0] = new SqlParameter("@id", _id);
			arParams[1] = new SqlParameter("@name", _name);
            arParams[2] = new SqlParameter("@happens", _happens);
            arParams[3] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateHoliday", arParams);
		}
        public void UpdateOrder(int _id, int _display)
        {
            if (logging == true)
                oLog.Add("Update holiday order " + GetName(_id));
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@display", _display);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateHolidayOrder", arParams);
        }
        public void Enable(int _id, int _enabled) 
		{
			if (logging == true) 
			{
				if (_enabled == 1)
                    oLog.Add("Enable holiday " + GetName(_id));
				else
                    oLog.Add("Disable holiday " + GetName(_id));
			}
			arParams = new SqlParameter[2];
			arParams[0] = new SqlParameter("@id", _id);
			arParams[1] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateHolidayEnabled", arParams);
		}
		public void Delete(int _id)
		{
			if (logging == true)
                oLog.Add("Delete holiday " + GetName(_id));
			arParams = new SqlParameter[1];
			arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteHoliday", arParams);
		}
        public DateTime GetHours(double _hours, DateTime _start_date)
        {
            double dblDays = _hours / 8.00;
            return GetDays(dblDays, _start_date);
        }
        public DateTime GetDays(double _days, DateTime _start_date)
        {
            for (double ii = 0.00; ii < _days; ii = ii + 1.00)
            {
                _start_date = _start_date.AddDays(1);
                while (_start_date.DayOfWeek == DayOfWeek.Saturday || _start_date.DayOfWeek == DayOfWeek.Sunday || (Get(_start_date).Tables[0].Rows.Count > 0))
                    _start_date = _start_date.AddDays(1);
            }
            return _start_date;
        }
        public DateTime GetDaysBack(double _days, DateTime _start_date)
        {
            for (double ii = _days; ii > 0; ii = ii - 1.00)
            {
                _start_date = _start_date.AddDays(-1);
                while (_start_date.DayOfWeek == DayOfWeek.Saturday || _start_date.DayOfWeek == DayOfWeek.Sunday || (Get(_start_date).Tables[0].Rows.Count > 0))
                    _start_date = _start_date.AddDays(-1);
            }
            return _start_date;
        }
    }
}
