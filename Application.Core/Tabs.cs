using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using System.IO;

namespace NCC.ClearView.Application.Core
{
	public class Tabs
	{
		private string dsn = "";
		private int user = 0;
		private SqlParameter[] arParams;
		private DataSet ds;       
        public Tabs(int _user, string _dsn)
		{
			user = _user;
			dsn = _dsn;
		}
		public DataSet GetTab(int _id)
		{
			arParams = new SqlParameter[1];
			arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getTab", arParams);
		}
        public string GetTab(int _id, string _column)
        {
            ds = GetTab(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public string GetTabName(int _id)
		{
			string strName = "Unavailable";
            try { strName = GetTab(_id).Tables[0].Rows[0]["tabname"].ToString(); }
			catch {}
			return strName;
		}
        public DataSet GetTabs(int _enabled)
		{
			arParams = new SqlParameter[1];            
            arParams[0] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getTabs", arParams);
		}       
        public void AddTab(string _name,string _tab_name,string _path,int _enabled)
        {
			arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@name", _name);
            arParams[1] = new SqlParameter("@tabname", _tab_name);
            arParams[2] = new SqlParameter("@path", _path);             
            arParams[3] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addTab", arParams);
		}
        public void UpdateTab(int _id,string _name,string _tab_name, string _path, int _enabled)
		{
            arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@name", _name);
            arParams[2] = new SqlParameter("@tabname", _tab_name);
            arParams[3] = new SqlParameter("@path", _path);            
            arParams[4] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateTab", arParams);
		}
        public void EnableTab(int _id, int _enabled) 
		{
			arParams = new SqlParameter[2];
			arParams[0] = new SqlParameter("@id", _id);
			arParams[1] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateTabEnabled", arParams);
		}
        public void DeleteTab(int _id)
		{
			arParams = new SqlParameter[1];
			arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteTab", arParams);
		}

        public void AddRequestItemsTab(int _tabid, int _itemid, int _display)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@tabid", _tabid);
            arParams[1] = new SqlParameter("@itemid", _itemid);
            arParams[2] = new SqlParameter("@display", _display);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addRequestItemsTab", arParams);
        }        
        public void DeleteRequestItemsTab(int _itemid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@itemid", _itemid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteRequestItemsTab", arParams);
        }
        public DataSet GetRequestItemsTabs(int _itemid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@itemid", _itemid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getRequestItemsTabs", arParams);
        }

        public void AddRequestItemsTaskTab(int _tabid, int _itemid, int _display)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@tabid", _tabid);
            arParams[1] = new SqlParameter("@itemid", _itemid);
            arParams[2] = new SqlParameter("@display", _display);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addRequestItemsTaskTab", arParams);
        }
        public void DeleteRequestItemsTaskTab(int _itemid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@itemid", _itemid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteRequestItemsTaskTab", arParams);
        }
        public DataSet GetRequestItemsTaskTabs(int _itemid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@itemid", _itemid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getRequestItemsTaskTabs", arParams);
        }
    }
}
