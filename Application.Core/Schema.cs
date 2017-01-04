using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;

namespace NCC.ClearView.Application.Core
{
	/// <summary>
	/// Summary description for Schema.
	/// </summary>
	public class Schema
	{
		private string dsn = "";
		private int user = 0;
		private bool logging = false;
		private SqlParameter[] arParams;
		private Controls oControl;
		private Log oLog;
		public Schema(int _user, string _dsn)
		{
			user = _user;
			dsn = _dsn;
			oLog = new Log(user, dsn);
			oControl = new Controls(user, dsn);
		}
		public DataSet Get(int _id)
		{
			arParams = new SqlParameter[1];
			arParams[0] = new SqlParameter("@id", _id);
			return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getSchema", arParams);
		}
		public string GetName(int _id)
		{
			string strName = "Unavailable";
			try { strName = Get(_id).Tables[0].Rows[0]["name"].ToString(); }
			catch {}
			return strName;
		}
        //public DataSet Gets(int _enabled, int _id)
        //{
        //    arParams = new SqlParameter[2];
        //    arParams[0] = new SqlParameter("@enabled", _enabled);
        //    arParams[1] = new SqlParameter("@id", _id);
        //    return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getSchemas", arParams);
        //}
        //public DataSet Gets(int _enabled) 
        //{
        //    arParams = new SqlParameter[1];
        //    arParams[0] = new SqlParameter("@enabled", _enabled);
        //    return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getSchemasAll", arParams);
        //}
		public int Add(int _controlid)
		{
			if (logging == true)
                oLog.Add("Add schema for control " + oControl.GetName(_controlid));
            arParams = new SqlParameter[2];
			arParams[0] = new SqlParameter("@controlid", _controlid);
			arParams[1] = new SqlParameter("@id", SqlDbType.Int);
			arParams[1].Direction = ParameterDirection.Output;
			SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addSchema", arParams);
			return Int32.Parse(arParams[1].Value.ToString());
		}
	}
}
