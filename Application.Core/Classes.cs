using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;

namespace NCC.ClearView.Application.Core
{
    public class Classes
	{
		private string dsn = "";
		private int user = 0;
		private SqlParameter[] arParams;
        public Classes(int _user, string _dsn)
		{
			user = _user;
			dsn = _dsn;
		}
        public DataSet Gets(int _enabled)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getClasss", arParams);
        }
        public DataSet GetTests(int _enabled)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getClassTests", arParams);
        }
        public DataSet GetForecasts(int _enabled)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getClassForecasts", arParams);
        }
        public DataSet GetWorkstationVMwares(int _enabled)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getClassWorkstationVMwares", arParams);
        }
        public DataSet Get(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getClass", arParams);
        }
        public string Get(int _id, string _column)
        {
            DataSet ds = Get(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public bool IsProd(int _id)
        {
            return (Get(_id, "prod") == "1");
        }
        public bool IsQA(int _id)
        {
            return (Get(_id, "qa") == "1");
        }
        public bool IsTest(int _id)
        {
            return (Get(_id, "test") == "1");
        }
        public bool IsDR(int _id)
        {
            return (Get(_id, "dr") == "1");
        }
        public bool IsDev(int _id)
        {
            return (IsProd(_id) == false && IsQA(_id) == false && IsTest(_id) == false && IsDR(_id) == false);
        }
        public bool IsTestDev(int _id)
        {
            return (IsTest(_id) || IsDev(_id));
        }
        public void Add(string _name, string _factory_code, int _forecast, int _workstation_vmware, int _prod, int _qa, int _test, int _dr, int _pnc, string _domain, int _enabled)
		{
			arParams = new SqlParameter[11];
            arParams[0] = new SqlParameter("@name", _name);
            arParams[1] = new SqlParameter("@factory_code", _factory_code);
            arParams[2] = new SqlParameter("@forecast", _forecast);
            arParams[3] = new SqlParameter("@workstation_vmware", _workstation_vmware);
            arParams[4] = new SqlParameter("@prod", _prod);
            arParams[5] = new SqlParameter("@qa", _qa);
            arParams[6] = new SqlParameter("@test", _test);
            arParams[7] = new SqlParameter("@dr", _dr);
            arParams[8] = new SqlParameter("@pnc", _pnc);
            arParams[9] = new SqlParameter("@domain", _domain);
            arParams[10] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addClass", arParams);
		}
        public void Update(int _id, string _name, string _factory_code, int _forecast, int _workstation_vmware, int _prod, int _qa, int _test, int _dr, int _pnc, string _domain, int _enabled)
        {
            arParams = new SqlParameter[12];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@name", _name);
            arParams[2] = new SqlParameter("@factory_code", _factory_code);
            arParams[3] = new SqlParameter("@forecast", _forecast);
            arParams[4] = new SqlParameter("@workstation_vmware", _workstation_vmware);
            arParams[5] = new SqlParameter("@prod", _prod);
            arParams[6] = new SqlParameter("@qa", _qa);
            arParams[7] = new SqlParameter("@test", _test);
            arParams[8] = new SqlParameter("@dr", _dr);
            arParams[9] = new SqlParameter("@pnc", _pnc);
            arParams[10] = new SqlParameter("@domain", _domain);
            arParams[11] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateClass", arParams);
        }
        public void UpdateOrder(int _id, int _display)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@display", _display);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateClassOrder", arParams);
        }
        public void Enable(int _id, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateClassEnabled", arParams);
        }
        public void Delete(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteClass", arParams);
        }


        public void AddEnvironment(int _classid, int _environmentid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@classid", _classid);
            arParams[1] = new SqlParameter("@environmentid", _environmentid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addClassEnvironment", arParams);
        }
        public void DeleteEnvironment(int _classid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@classid", _classid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteClassEnvironment", arParams);
        }
        public DataSet GetEnvironment(int _classid, int _forecast)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@classid", _classid);
            arParams[1] = new SqlParameter("@forecast", _forecast);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getClassEnvironment", arParams);
        }
        public DataSet GetEnvironments(string _classes, int _forecast)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@classes", _classes);
            arParams[1] = new SqlParameter("@forecast", _forecast);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getClassEnvironments", arParams);
        }


        public void AddJoin(string _name, int _class1, int _class2)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@name", _name);
            arParams[1] = new SqlParameter("@class1", _class1);
            arParams[2] = new SqlParameter("@class2", _class2);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addClassJoin", arParams);
        }
        public void DeleteJoins(int _classid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@classid", _classid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteClassJoins", arParams);
        }
        public DataSet GetJoins(int _classid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@classid", _classid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getClassJoins", arParams);
        }


        public void AddEnvironmentAP(int _classid, int _environmentid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@classid", _classid);
            arParams[1] = new SqlParameter("@environmentid", _environmentid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addClassEnvironmentAP", arParams);
        }
        public void DeleteEnvironmentAP(int _classid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@classid", _classid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteClassEnvironmentAP", arParams);
        }
        public DataSet GetEnvironmentAP(int _classid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@classid", _classid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getClassEnvironmentAP", arParams);
        }
        public bool IsEnvironmentAP(int _classid, int _environmentid)
        {
            bool boolReturn = false;
            DataSet ds = GetEnvironmentAP(_classid);
            foreach (DataRow dr in ds.Tables[0].Rows) 
            {
                if (Int32.Parse(dr["environmentid"].ToString()) == _environmentid)
                {
                    boolReturn = true;
                    break;
                }
            }
            return boolReturn;
        }
    }
}
