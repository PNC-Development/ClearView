using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;

namespace NCC.ClearView.Application.Core
{
    public class VirtualHDD
	{
		private string dsn = "";
		private int user = 0;
		private SqlParameter[] arParams;
        public VirtualHDD(int _user, string _dsn)
		{
			user = _user;
			dsn = _dsn;
		}
        public DataSet Gets(int _enabled)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getVirtualHDDs", arParams);
        }
        public DataSet GetVMwares(int _xp, int _win7, int _enabled)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@xp", _xp);
            arParams[1] = new SqlParameter("@win7", _win7);
            arParams[2] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getVirtualHDDsVMware", arParams);
        }
        public DataSet GetVirtuals(int _enabled)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getVirtualHDDsVirtual", arParams);
        }

        public DataSet Get(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getVirtualHDD", arParams);
        }
        public string Get(int _id, string _column)
        {
            DataSet ds = Get(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public void Add(string _name, double _value, int _vmware, int _virtual, int _xp, int _win7, int _default, string _prompt, int _display, int _enabled)
        {
            arParams = new SqlParameter[10];
            arParams[0] = new SqlParameter("@name", _name);
            arParams[1] = new SqlParameter("@value", _value);
            arParams[2] = new SqlParameter("@vmware", _vmware);
            arParams[3] = new SqlParameter("@virtual", _virtual);
            arParams[4] = new SqlParameter("@xp", _xp);
            arParams[5] = new SqlParameter("@win7", _win7);
            arParams[6] = new SqlParameter("@default", _default);
            arParams[7] = new SqlParameter("@prompt", _prompt);
            arParams[8] = new SqlParameter("@display", _display);
            arParams[9] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addVirtualHDD", arParams);
		}
        public void Update(int _id, string _name, double _value, int _vmware, int _virtual, int _xp, int _win7, int _default, string _prompt, int _enabled)
        {
            arParams = new SqlParameter[10];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@name", _name);
            arParams[2] = new SqlParameter("@value", _value);
            arParams[3] = new SqlParameter("@vmware", _vmware);
            arParams[4] = new SqlParameter("@virtual", _virtual);
            arParams[5] = new SqlParameter("@xp", _xp);
            arParams[6] = new SqlParameter("@win7", _win7);
            arParams[7] = new SqlParameter("@default", _default);
            arParams[8] = new SqlParameter("@prompt", _prompt);
            arParams[9] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateVirtualHDD", arParams);
        }
        public void UpdateOrder(int _id, int _display)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@display", _display);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateVirtualHDDOrder", arParams);
        }
        public void Enable(int _id, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateVirtualHDDEnabled", arParams);
        }
        public void Delete(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteVirtualHDD", arParams);
        }
    }
}
