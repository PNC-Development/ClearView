using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using System.Data.SqlTypes;

namespace NCC.ClearView.Application.Core
{
	public class Search
	{
		private string dsn = "";
		private int user = 0;
		private SqlParameter[] arParams;
        public Search(int _user, string _dsn)
		{
			user = _user;
			dsn = _dsn;
		}
        public DataSet GetProject(int _id)
		{
			arParams = new SqlParameter[1];
			arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getSearchProject", arParams);
		}
        public int AddProject(int _userid, int _applicationid, string _type, int _department, int _dstatus, string _dstart, string _dend)
        {
            arParams = new SqlParameter[8];
            arParams[0] = new SqlParameter("@userid", _userid);
            arParams[1] = new SqlParameter("@applicationid", _applicationid);
            arParams[2] = new SqlParameter("@type", _type);
            arParams[3] = new SqlParameter("@department", _department);
            arParams[4] = new SqlParameter("@dstatus", _dstatus);
            arParams[5] = new SqlParameter("@dstart", (_dstart == "" ? SqlDateTime.Null : DateTime.Parse(_dstart)));
            arParams[6] = new SqlParameter("@dend", (_dend == "" ? SqlDateTime.Null : DateTime.Parse(_dend)));
            arParams[7] = new SqlParameter("@id", SqlDbType.Int);
            arParams[7].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addSearchProject", arParams);
            return Int32.Parse(arParams[7].Value.ToString());
        }
        public int AddProjectTechnician(int _userid, int _applicationid, string _type, int _technician, int _tstatus, string _tstart, string _tend)
        {
            arParams = new SqlParameter[8];
            arParams[0] = new SqlParameter("@userid", _userid);
            arParams[1] = new SqlParameter("@applicationid", _applicationid);
            arParams[2] = new SqlParameter("@type", _type);
            arParams[3] = new SqlParameter("@technician", _technician);
            arParams[4] = new SqlParameter("@tstatus", _tstatus);
            arParams[5] = new SqlParameter("@tstart", (_tstart == "" ? SqlDateTime.Null : DateTime.Parse(_tstart)));
            arParams[6] = new SqlParameter("@tend", (_tend == "" ? SqlDateTime.Null : DateTime.Parse(_tend)));
            arParams[7] = new SqlParameter("@id", SqlDbType.Int);
            arParams[7].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addSearchProjectTechnician", arParams);
            return Int32.Parse(arParams[7].Value.ToString());
        }
        public int AddProjectGroup(int _userid, int _applicationid, string _type, int _itemid, int _gstatus, string _gstart, string _gend)
        {
            arParams = new SqlParameter[8];
            arParams[0] = new SqlParameter("@userid", _userid);
            arParams[1] = new SqlParameter("@applicationid", _applicationid);
            arParams[2] = new SqlParameter("@type", _type);
            arParams[3] = new SqlParameter("@itemid", _itemid);
            arParams[4] = new SqlParameter("@gstatus", _gstatus);
            arParams[5] = new SqlParameter("@gstart", (_gstart == "" ? SqlDateTime.Null : DateTime.Parse(_gstart)));
            arParams[6] = new SqlParameter("@gend", (_gend == "" ? SqlDateTime.Null : DateTime.Parse(_gend)));
            arParams[7] = new SqlParameter("@id", SqlDbType.Int);
            arParams[7].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addSearchProjectGroup", arParams);
            return Int32.Parse(arParams[7].Value.ToString());
        }
        public int AddProjectLead(int _userid, int _applicationid, string _type, int _lead, int _lstatus, string _lstart, string _lend)
        {
            arParams = new SqlParameter[8];
            arParams[0] = new SqlParameter("@userid", _userid);
            arParams[1] = new SqlParameter("@applicationid", _applicationid);
            arParams[2] = new SqlParameter("@type", _type);
            arParams[3] = new SqlParameter("@lead", _lead);
            arParams[4] = new SqlParameter("@lstatus", _lstatus);
            arParams[5] = new SqlParameter("@lstart", (_lstart == "" ? SqlDateTime.Null : DateTime.Parse(_lstart)));
            arParams[6] = new SqlParameter("@lend", (_lend == "" ? SqlDateTime.Null : DateTime.Parse(_lend)));
            arParams[7] = new SqlParameter("@id", SqlDbType.Int);
            arParams[7].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addSearchProjectLead", arParams);
            return Int32.Parse(arParams[7].Value.ToString());
        }
        public int AddProjectOverall(int _userid, int _applicationid, string _type, string _oname, string _onumber, int _oorganizationid, int _osegmentid, int _ostatus, int _oby, string _ostart, string _oend)
        {
            arParams = new SqlParameter[12];
            arParams[0] = new SqlParameter("@userid", _userid);
            arParams[1] = new SqlParameter("@applicationid", _applicationid);
            arParams[2] = new SqlParameter("@type", _type);
            arParams[3] = new SqlParameter("@oname", _oname);
            arParams[4] = new SqlParameter("@onumber", _onumber);
            arParams[5] = new SqlParameter("@oorganizationid", _oorganizationid);
            arParams[6] = new SqlParameter("@osegmentid", _osegmentid);
            arParams[7] = new SqlParameter("@ostatus", _ostatus);
            arParams[8] = new SqlParameter("@oby", _oby);
            arParams[9] = new SqlParameter("@ostart", (_ostart == "" ? SqlDateTime.Null : DateTime.Parse(_ostart)));
            arParams[10] = new SqlParameter("@oend", (_oend == "" ? SqlDateTime.Null : DateTime.Parse(_oend)));
            arParams[11] = new SqlParameter("@id", SqlDbType.Int);
            arParams[11].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addSearchProjectOverall", arParams);
            return Int32.Parse(arParams[11].Value.ToString());
        }

        public DataSet GetTask(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getSearchTask", arParams);
        }
        public int AddTask(int _userid, int _applicationid, string _type, int _department, int _dstatus, string _dstart, string _dend)
        {
            arParams = new SqlParameter[8];
            arParams[0] = new SqlParameter("@userid", _userid);
            arParams[1] = new SqlParameter("@applicationid", _applicationid);
            arParams[2] = new SqlParameter("@type", _type);
            arParams[3] = new SqlParameter("@department", _department);
            arParams[4] = new SqlParameter("@dstatus", _dstatus);
            arParams[5] = new SqlParameter("@dstart", (_dstart == "" ? SqlDateTime.Null : DateTime.Parse(_dstart)));
            arParams[6] = new SqlParameter("@dend", (_dend == "" ? SqlDateTime.Null : DateTime.Parse(_dend)));
            arParams[7] = new SqlParameter("@id", SqlDbType.Int);
            arParams[7].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addSearchTask", arParams);
            return Int32.Parse(arParams[7].Value.ToString());
        }
        public int AddTaskTechnician(int _userid, int _applicationid, string _type, int _technician, int _tstatus, string _tstart, string _tend)
        {
            arParams = new SqlParameter[8];
            arParams[0] = new SqlParameter("@userid", _userid);
            arParams[1] = new SqlParameter("@applicationid", _applicationid);
            arParams[2] = new SqlParameter("@type", _type);
            arParams[3] = new SqlParameter("@technician", _technician);
            arParams[4] = new SqlParameter("@tstatus", _tstatus);
            arParams[5] = new SqlParameter("@tstart", (_tstart == "" ? SqlDateTime.Null : DateTime.Parse(_tstart)));
            arParams[6] = new SqlParameter("@tend", (_tend == "" ? SqlDateTime.Null : DateTime.Parse(_tend)));
            arParams[7] = new SqlParameter("@id", SqlDbType.Int);
            arParams[7].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addSearchTaskTechnician", arParams);
            return Int32.Parse(arParams[7].Value.ToString());
        }
        public int AddTaskGroup(int _userid, int _applicationid, string _type, int _itemid, int _gstatus, string _gstart, string _gend)
        {
            arParams = new SqlParameter[8];
            arParams[0] = new SqlParameter("@userid", _userid);
            arParams[1] = new SqlParameter("@applicationid", _applicationid);
            arParams[2] = new SqlParameter("@type", _type);
            arParams[3] = new SqlParameter("@itemid", _itemid);
            arParams[4] = new SqlParameter("@gstatus", _gstatus);
            arParams[5] = new SqlParameter("@gstart", (_gstart == "" ? SqlDateTime.Null : DateTime.Parse(_gstart)));
            arParams[6] = new SqlParameter("@gend", (_gend == "" ? SqlDateTime.Null : DateTime.Parse(_gend)));
            arParams[7] = new SqlParameter("@id", SqlDbType.Int);
            arParams[7].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addSearchTaskGroup", arParams);
            return Int32.Parse(arParams[7].Value.ToString());
        }
        public int AddTaskLead(int _userid, int _applicationid, string _type, int _lead, int _lstatus, string _lstart, string _lend)
        {
            arParams = new SqlParameter[8];
            arParams[0] = new SqlParameter("@userid", _userid);
            arParams[1] = new SqlParameter("@applicationid", _applicationid);
            arParams[2] = new SqlParameter("@type", _type);
            arParams[3] = new SqlParameter("@lead", _lead);
            arParams[4] = new SqlParameter("@lstatus", _lstatus);
            arParams[5] = new SqlParameter("@lstart", (_lstart == "" ? SqlDateTime.Null : DateTime.Parse(_lstart)));
            arParams[6] = new SqlParameter("@lend", (_lend == "" ? SqlDateTime.Null : DateTime.Parse(_lend)));
            arParams[7] = new SqlParameter("@id", SqlDbType.Int);
            arParams[7].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addSearchTaskLead", arParams);
            return Int32.Parse(arParams[7].Value.ToString());
        }
        public int AddTaskSkill(int _userid, int _applicationid, string _type, string _skill)
        {
            arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@userid", _userid);
            arParams[1] = new SqlParameter("@applicationid", _applicationid);
            arParams[2] = new SqlParameter("@type", _type);
            arParams[3] = new SqlParameter("@skill", _skill);
            arParams[4] = new SqlParameter("@id", SqlDbType.Int);
            arParams[4].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addSearchTaskSkill", arParams);
            return Int32.Parse(arParams[4].Value.ToString());
        }
        public int AddTaskOverall(int _userid, int _applicationid, string _type, string _oname, string _onumber, int _ostatus, int _oby, string _ostart, string _oend)
        {
            arParams = new SqlParameter[10];
            arParams[0] = new SqlParameter("@userid", _userid);
            arParams[1] = new SqlParameter("@applicationid", _applicationid);
            arParams[2] = new SqlParameter("@type", _type);
            arParams[3] = new SqlParameter("@oname", _oname);
            arParams[4] = new SqlParameter("@onumber", _onumber);
            arParams[5] = new SqlParameter("@ostatus", _ostatus);
            arParams[6] = new SqlParameter("@oby", _oby);
            arParams[7] = new SqlParameter("@ostart", (_ostart == "" ? SqlDateTime.Null : DateTime.Parse(_ostart)));
            arParams[8] = new SqlParameter("@oend", (_oend == "" ? SqlDateTime.Null : DateTime.Parse(_oend)));
            arParams[9] = new SqlParameter("@id", SqlDbType.Int);
            arParams[9].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addSearchTaskOverall", arParams);
            return Int32.Parse(arParams[9].Value.ToString());
        }

        public int AddSkill(int _userid, int _applicationid, string _type, string _skill)
        {
            arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@userid", _userid);
            arParams[1] = new SqlParameter("@applicationid", _applicationid);
            arParams[2] = new SqlParameter("@type", _type);
            arParams[3] = new SqlParameter("@skill", _skill);
            arParams[4] = new SqlParameter("@id", SqlDbType.Int);
            arParams[4].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addSearchProjectSkill", arParams);
            return Int32.Parse(arParams[4].Value.ToString());
        }
    }
}
