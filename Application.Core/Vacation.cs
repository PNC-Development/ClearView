using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using System.Text;

namespace NCC.ClearView.Application.Core
{
	public class Vacation
	{
		private string dsn = "";
		private int user = 0;
		private SqlParameter[] arParams;
		private DataSet ds;
		public Vacation(int _user, string _dsn)
		{
			user = _user;
			dsn = _dsn;
		}
		public DataSet Get(int _id)
		{
			arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
			return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getVacation", arParams);
		}
        public string Get(int _id, string _column)
        {
            ds = Get(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public DataSet Get(DateTime _date, int _application)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@date", _date);
            arParams[1] = new SqlParameter("@application", _application);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getVacations", arParams);
        }
        public DataSet Get(DateTime _date)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@date", _date);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getVacationsDate", arParams);
        }
        public DataSet Gets(int _userid, int _year)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@userid", _userid);
            arParams[1] = new SqlParameter("@year", _year);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getVacationsAll", arParams);
        }
        public int Add(int _userid, int _application, DateTime _start, int _morning, int _afternoon, int _vacation, int _holiday, int _personal, string _reason, int _approved)
		{
			arParams = new SqlParameter[11];
            arParams[0] = new SqlParameter("@userid", _userid);
            arParams[1] = new SqlParameter("@application", _application);
            arParams[2] = new SqlParameter("@start", _start);
            arParams[3] = new SqlParameter("@morning", _morning);
            arParams[4] = new SqlParameter("@afternoon", _afternoon);
            arParams[5] = new SqlParameter("@vacation", _vacation);
            arParams[6] = new SqlParameter("@holiday", _holiday);
            arParams[7] = new SqlParameter("@personal", _personal);
            arParams[8] = new SqlParameter("@reason", _reason);
            arParams[9] = new SqlParameter("@approved", _approved);
            arParams[10] = new SqlParameter("@id", SqlDbType.Int);
            arParams[10].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addVacation", arParams);
            return Int32.Parse(arParams[10].Value.ToString());
		}
        public void Update(int _id, int _userid, int _application, DateTime _start, int _morning, int _afternoon, int _vacation, int _holiday, int _personal, string _reason, int _approved)
		{
			arParams = new SqlParameter[11];
			arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@userid", _userid);
            arParams[2] = new SqlParameter("@application", _application);
            arParams[3] = new SqlParameter("@start", _start);
            arParams[4] = new SqlParameter("@morning", _morning);
            arParams[5] = new SqlParameter("@afternoon", _afternoon);
            arParams[6] = new SqlParameter("@vacation", _vacation);
            arParams[7] = new SqlParameter("@holiday", _holiday);
            arParams[8] = new SqlParameter("@personal", _personal);
            arParams[9] = new SqlParameter("@reason", _reason);
            arParams[10] = new SqlParameter("@approved", _approved);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateVacation", arParams);
		}
        public void Update(int _id, int _approved)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@approved", _approved);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateVacationApprove", arParams);
        }
        public void Delete(int _id)
		{
			arParams = new SqlParameter[1];
			arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteVacation", arParams);
		}
        public string GetBody(int _id, int _environment)
        {
            Users oUser = new Users(user, dsn);
            Variables oVariable = new Variables(_environment);
            StringBuilder sbBody = new StringBuilder();
            string strSpacerRow = "<tr><td colspan=\"3\"><img src=\"" + oVariable.ImageURL() + "/images/spacer.gif\" border=\"0\" width=\"1\" height=\"7\" /></td></tr>";
            ds = Get(_id);
            if (ds.Tables[0].Rows.Count > 0)
            {
                sbBody.Append("<tr><td nowrap><b>Submitter:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                sbBody.Append(oUser.GetFullName(Int32.Parse(ds.Tables[0].Rows[0]["userid"].ToString())));
                sbBody.Append("</td></tr>");
                sbBody.Append(strSpacerRow);
                sbBody.Append("<tr><td nowrap><b>Date:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                sbBody.Append(DateTime.Parse(ds.Tables[0].Rows[0]["start_date"].ToString()).ToLongDateString());
                sbBody.Append("</td></tr>");
                sbBody.Append(strSpacerRow);
                string strType = "Full Day";
                if (ds.Tables[0].Rows[0]["morning"].ToString() == "1")
                    strType = "Morning Only";
                else if (ds.Tables[0].Rows[0]["afternoon"].ToString() == "1")
                    strType = "Afternoon Only";
                string strReason = ds.Tables[0].Rows[0]["reason"].ToString();
                if (ds.Tables[0].Rows[0]["vacation"].ToString() == "1")
                    strReason = "Vacation";
                else if (ds.Tables[0].Rows[0]["holiday"].ToString() == "1")
                    strReason = "Floating Holiday";
                else if (ds.Tables[0].Rows[0]["personal"].ToString() == "1")
                    strReason = "Personal / Sick Day";
                sbBody.Append("<tr><td nowrap><b>Type:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                sbBody.Append(strType);
                sbBody.Append("</td></tr>");
                sbBody.Append(strSpacerRow);
                sbBody.Append("<tr><td nowrap><b>Reason:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                sbBody.Append(strReason);
                sbBody.Append("</td></tr>");
                sbBody.Append(strSpacerRow);
                sbBody.Append("<tr><td nowrap><b>Submitted On:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                sbBody.Append(DateTime.Parse(ds.Tables[0].Rows[0]["modified"].ToString()).ToLongDateString());
                sbBody.Append("</td></tr>");
            }
            if (sbBody.ToString() != "")
            {
                sbBody.Insert(0, "<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"" + oVariable.DefaultFontStyle() + "\">");
                sbBody.Append("</table>");
            }
            return sbBody.ToString(); ;
        }
    }
}
