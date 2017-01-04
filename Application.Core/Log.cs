using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using System.Text;

namespace NCC.ClearView.Application.Core
{
    public enum LoggingType : int
    {
        Debug = -999,
        Error = -1,
        Warning = 0,
        Information = 1,
    }
    public class Log
	{
		private string dsn = "";
		private int user = 0;
		private SqlParameter[] arParams;
		public Log(int _user, string _dsn)
		{
			user = _user;
			dsn = _dsn;
		}
		public DataSet Get()
		{
			return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getLog");
		}
		public void Add(string _action) 
		{
			arParams = new SqlParameter[2];
			arParams[0] = new SqlParameter("@user", user);
			arParams[1] = new SqlParameter("@action", _action);
			SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addLog", arParams);
		}
		public void Delete(int _records) 
		{
			arParams = new SqlParameter[1]; ;
			arParams[0] = new SqlParameter("@records", _records);
			SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteLog", arParams);
		}
		public void Delete() 
		{
			SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteLogs");
		}

        public void AddEvent(int _answerid, string _name, string _serial, string _message, LoggingType _type)
        {
            AddEvent(_answerid, _name, _serial, _message, _type, DateTime.Now);
        }
        public void AddEvent(string _name, string _serial, string _message, LoggingType _type)
        {
            AddEvent(0, _name, _serial, _message, _type, DateTime.Now);
        }
        private void AddEvent(int _answerid, string _name, string _serial, string _message, LoggingType _type, DateTime _created)
        {
            arParams = new SqlParameter[6];
            arParams[0] = new SqlParameter("@answerid", _answerid);
            arParams[1] = new SqlParameter("@name", _name);
            arParams[2] = new SqlParameter("@serial", _serial);
            arParams[3] = new SqlParameter("@message", _message);
            arParams[4] = new SqlParameter("@type", _type);
            arParams[5] = new SqlParameter("@created", _created);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addEventLog", arParams);
        }
        public DataSet GetEventsByName(string _name, int _type)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@name", _name);
            arParams[1] = new SqlParameter("@type", _type);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getEventLogName", arParams);
        }
        //public DataSet GetEventsBySerial(string _name, string _serial)
        //{
        //    arParams = new SqlParameter[1];
        //    arParams[0] = new SqlParameter("@serial", _serial);
        //    return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getEventLogSerial", arParams);
        //}
        //public DataSet GetEvents(string _name, string _serial)
        //{
        //    arParams = new SqlParameter[2];
        //    arParams[0] = new SqlParameter("@name", _name);
        //    arParams[1] = new SqlParameter("@serial", _serial);
        //    return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getEventLogs", arParams);
        //}
        public string GetEvents(DataSet _events, int _environment)
        {
            Variables oVariable = new Variables(_environment);
            StringBuilder strReturn = new StringBuilder();
            strReturn.Append("<table border=\"0\" cellpadding=\"4\" cellspacing=\"2\" style=\"" + oVariable.DefaultFontStyle() + ";border:solid 1px #CCCCCC\">");
            strReturn.Append("<tr bgcolor=\"#EEEEEE\"><td colspan=\"2\" class=\"bold\">Logging Results...</td></tr>");
            foreach (DataRow drLog in _events.Tables[0].Rows)
            {
                int intType = -999;
                Int32.TryParse(drLog["type"].ToString(), out intType);
                string strImage = "/images/xx.gif";
                if ((LoggingType)intType == LoggingType.Error)
                    strImage = "/images/cancel.gif";
                else if ((LoggingType)intType == LoggingType.Warning)
                    strImage = "/images/alert.gif";
                else if ((LoggingType)intType == LoggingType.Information)
                    strImage = "/images/check.gif";
                strReturn.Append("<tr>");
                strReturn.Append("<td valign=\"top\" nowrap><img src=\"" + oVariable.ImageURL() + strImage + "\" border=\"0\" align=\"absmiddle\"/>&nbsp;" + drLog["created"].ToString() + ":</td>");
                strReturn.Append("<td valign=\"top\">" + drLog["message"].ToString() + "</td>");
                strReturn.Append("</tr>");
            }
            strReturn.Append("</table>");
            return strReturn.ToString();
        }

	}
}
