using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;

namespace NCC.ClearView.Application.Core
{
	public class Field
	{
		private string dsn = "";
		private int user = 0;
		private SqlParameter[] arParams;
        public Field(int _user, string _dsn)
		{
			user = _user;
			dsn = _dsn;
		}
        public DataSet GetTables(int _enabled)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getTables", arParams);
        }
        public DataSet GetTable(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getTable", arParams);
        }
        public string GetTable(int _id, string _column)
        {
            DataSet ds = GetTable(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public string GetTableName2(int _serviceid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@serviceid", _serviceid);
            DataSet ds = SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getTableService", arParams);
            if (ds.Tables[0].Rows.Count == 0)
                return "";
            else
                return ds.Tables[0].Rows[0]["tablename"].ToString();
        }
        public void AddTable(string _tablename, int _enabled)
		{
			arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@tablename", _tablename);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addTable", arParams);
		}
        public void UpdateTable(int _id, string _tablename, int _enabled)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@tablename", _tablename);
            arParams[2] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateTable", arParams);
        }
        public void EnableTable(int _id, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateTableEnabled", arParams);
        }
        public void DeleteTable(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteTable", arParams);
        }

        public DataSet Gets(int _tableid, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@tableid", _tableid);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getFieldsTable", arParams);
        }
        public DataSet Gets(string _tablename)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@tablename", _tablename);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getFields", arParams);
        }
        public DataSet Gets2(int _serviceid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@serviceid", _serviceid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getFieldsService", arParams);
        }
        public DataSet Get(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getField", arParams);
        }
        public string Get(int _id, string _column)
        {
            DataSet ds = Get(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public void Add(int _tableid, string _fieldname, string _name, string _datatype, string _join_table, string _join_on, string _join_field, int _hidden, int _display, int _enabled)
        {
            arParams = new SqlParameter[10];
            arParams[0] = new SqlParameter("@tableid", _tableid);
            arParams[1] = new SqlParameter("@fieldname", _fieldname);
            arParams[2] = new SqlParameter("@name", _name);
            arParams[3] = new SqlParameter("@datatype", _datatype);
            arParams[4] = new SqlParameter("@join_table", _join_table);
            arParams[5] = new SqlParameter("@join_on", _join_on);
            arParams[6] = new SqlParameter("@join_field", _join_field);
            arParams[7] = new SqlParameter("@hidden", _hidden);
            arParams[8] = new SqlParameter("@display", _display);
            arParams[9] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addField", arParams);
        }
        public void Update(int _id, int _tableid, string _fieldname, string _name, string _datatype, string _join_table, string _join_on, string _join_field, int _hidden, int _enabled)
        {
            arParams = new SqlParameter[10];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@tableid", _tableid);
            arParams[2] = new SqlParameter("@fieldname", _fieldname);
            arParams[3] = new SqlParameter("@name", _name);
            arParams[4] = new SqlParameter("@datatype", _datatype);
            arParams[5] = new SqlParameter("@join_table", _join_table);
            arParams[6] = new SqlParameter("@join_on", _join_on);
            arParams[7] = new SqlParameter("@join_field", _join_field);
            arParams[8] = new SqlParameter("@hidden", _hidden);
            arParams[9] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateField", arParams);
        }
        public void UpdateOrder(int _id, int _display)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@display", _display);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateFieldOrder", arParams);
        }
        public void Enable(int _id, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateFieldEnabled", arParams);
        }
        public void Delete(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteField", arParams);
        }

        public void AddPermission2(int _serviceid, int _tableid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@serviceid", _serviceid);
            arParams[1] = new SqlParameter("@tableid", _tableid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addTablePermission", arParams);
        }
        public DataSet GetPermissions(int _tableid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@tableid", _tableid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getTablePermissions", arParams);
        }
        public void DeletePermission(int _tableid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@tableid", _tableid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteTablePermissions", arParams);
        }
        public DataSet GetTableServiceRequest(string _strTable, string _strRequest, string _strItem, string _strNumber)
        {
            return SqlHelper.ExecuteDataset(dsn, CommandType.Text, "SELECT * FROM " + _strTable + " WHERE requestid = " + _strRequest + " AND itemid = " + _strItem + " AND number = " + _strNumber + " AND deleted = 0");
        }

        public DataSet GetTableServiceRequest(string _strTable, string _strRequest, string _strItem)
        {
            return SqlHelper.ExecuteDataset(dsn, CommandType.Text, "SELECT * FROM " + _strTable + " WHERE requestid = " + _strRequest + " AND itemid = " + _strItem + " AND deleted = 0");
        }
    }
}
