using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;

namespace NCC.ClearView.Application.Core
{
    public class WhatsNew
    {

        private string dsn = "";
		private int user = 0;
		private SqlParameter[] arParams;

        public WhatsNew(int _user, string _dsn)
		{
			user = _user;
			dsn = _dsn;
		}

        public DataSet Get(long _id)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@Id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getWhatsNew", arParams);
        }

        public DataSet Gets(int _enabled)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getWhatsNews", arParams);
        }

        public DataSet GetNewsWithPaging(long _page, long _recsPerPage)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@Page", _page);
            arParams[1] = new SqlParameter("@RecsPerPage", _recsPerPage);

            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getWhatsNewWithPaging", arParams);
        }

        public string Get(int _id, string _column)
        {
            DataSet ds = Get(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }

        public void Add(string _title, string _description, string _attachment,string _version, string _category,
                        int _createdby ,int _enabled)
        {
            arParams = new SqlParameter[9];
            arParams[0] = new SqlParameter("@Title", _title);
            arParams[1] = new SqlParameter("@Description", _description);
            arParams[2] = new SqlParameter("@Attachment", _attachment);
            arParams[3] = new SqlParameter("@Version", _version);
            arParams[4] = new SqlParameter("@Category", _category);
            arParams[5] = new SqlParameter("@CreatedBy", _createdby);
            arParams[6] = new SqlParameter("@Enabled", _enabled);
            
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addWhatsNew", arParams);
        }

        public void Update(long _id, string _title, string _description, string _attachment, string _version, string _category,
                           int _modifiedby, int _enabled)
        {
            arParams = new SqlParameter[9];
            arParams[0] = new SqlParameter("@Id", _id);
            arParams[1] = new SqlParameter("@Title", _title);
            arParams[2] = new SqlParameter("@Description", _description);
            arParams[3] = new SqlParameter("@Attachment", _attachment);
            arParams[4] = new SqlParameter("@Version", _version);
            arParams[5] = new SqlParameter("@Category", _category);
            arParams[7] = new SqlParameter("@ModifiedBy", _modifiedby);
            arParams[8] = new SqlParameter("@Enabled", _enabled);

            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateWhatsNew", arParams);
        }

        public void UpdateOrder(long _id, long _display)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@display", _display);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateWhatsNewDisplayOrder", arParams);
        }

        public void Enable(long _id, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateWhatsNewEnabled", arParams);
        }

        public void Delete(long _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateWhatsNewDeleted", arParams);
        }


    }
}
