using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;

namespace NCC.ClearView.Application.Core
{
	public class PageControls
	{
		private string dsn = "";
		private int user = 0;
		private bool logging = false;
		private SqlParameter[] arParams;
		private Log oLog;
		public PageControls(int _user, string _dsn)
		{
			user = _user;
			dsn = _dsn;
			oLog = new Log(user, dsn);
		}
		public DataSet Get(int _id)
		{
			arParams = new SqlParameter[1];
			arParams[0] = new SqlParameter("@id", _id);
			return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getPageControl", arParams);
		}
		public DataSet GetPage(int _pageid, int _enabled)
		{
			arParams = new SqlParameter[2];
			arParams[0] = new SqlParameter("@pageid", _pageid);
			arParams[1] = new SqlParameter("@enabled", _enabled);
			return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getPageControls", arParams);
		}
		public DataSet GetPagePlaceholders(int _pageid, string _placeholder, int _enabled)
		{
			arParams = new SqlParameter[3];
			arParams[0] = new SqlParameter("@pageid", _pageid);
			arParams[1] = new SqlParameter("@placeholder", _placeholder);
			arParams[2] = new SqlParameter("@enabled", _enabled);
			return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getPageControlsPlaceholder", arParams);
		}
		public string GetName(int _id)
		{
			string strName = "Unavailable";
			try { strName = Get(_id).Tables[0].Rows[0]["placeholder"].ToString() + " - " + Get(_id).Tables[0].Rows[0]["pageid"].ToString(); }
			catch {}
			return strName;
		}
		public int Add(int _schemaid, string _placeholder, int _pageid, int _display, int _enabled)
		{
            Pages oPage = new Pages(user, dsn);
			if (logging == true)
                oLog.Add("Add page control to page " + oPage.GetName(_pageid));
			arParams = new SqlParameter[6];
			arParams[0] = new SqlParameter("@schemaid", _schemaid);
			arParams[1] = new SqlParameter("@placeholder", _placeholder);
			arParams[2] = new SqlParameter("@pageid", _pageid);
			arParams[3] = new SqlParameter("@display", _display);
			arParams[4] = new SqlParameter("@enabled", _enabled);
			arParams[5] = new SqlParameter("@id", SqlDbType.Int);
			arParams[5].Direction = ParameterDirection.Output;
			SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addPageControl", arParams);
			return Int32.Parse(arParams[5].Value.ToString());
		}
		public void Update(int _id, string _placeholder, int _pageid, int _display, int _enabled)
		{
            Pages oPage = new Pages(user, dsn);
            if (logging == true)
                oLog.Add("Update page control on page " + oPage.GetName(_pageid));
			arParams = new SqlParameter[5];
			arParams[0] = new SqlParameter("@id", _id);
			arParams[1] = new SqlParameter("@placeholder", _placeholder);
			arParams[2] = new SqlParameter("@pageid", _pageid);
			arParams[3] = new SqlParameter("@display", _display);
			arParams[4] = new SqlParameter("@enabled", _enabled);
			SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updatePageControl", arParams);
		}
		public void Assign(int _id, int _schema)
		{
			if (logging == true) 
				oLog.Add("Assign control " + GetName(_id));
			arParams = new SqlParameter[2];
			arParams[0] = new SqlParameter("@id", _id);
			arParams[1] = new SqlParameter("@schema", _schema);
			SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updatePageControlAssign", arParams);
		}
		public void Delete(int _id)
		{
			if (logging == true) 
				oLog.Add("Delete control " + GetName(_id));
			arParams = new SqlParameter[1];
			arParams[0] = new SqlParameter("@id", _id);
			SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deletePageControl", arParams);
		}
	}
}
