using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;

namespace NCC.ClearView.Application.Core
{
    public class Icons
	{
		private string dsn = "";
		private int user = 0;
		private SqlParameter[] arParams;
        public Icons(int _user, string _dsn)
		{
			user = _user;
			dsn = _dsn;
		}
        public DataSet Gets(int _enabled)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getIcons", arParams);
        }
        public DataSet Get(string _extension)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@extension", _extension.ToUpper());
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getIconExtension", arParams);
        }
        public DataSet Get(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getIcon", arParams);
        }
        public string Get(int _id, string _column)
        {
            DataSet ds = Get(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public void Add(string _extension, string _small, string _large, string _content_type, int _iframe, int _preview, int _enabled)
		{
			arParams = new SqlParameter[7];
            arParams[0] = new SqlParameter("@extension", _extension);
            arParams[1] = new SqlParameter("@small", _small);
            arParams[2] = new SqlParameter("@large", _large);
            arParams[3] = new SqlParameter("@content_type", _content_type);
            arParams[4] = new SqlParameter("@iframe", _iframe);
            arParams[5] = new SqlParameter("@preview", _preview);
            arParams[6] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addIcon", arParams);
		}
        public void Update(int _id, string _extension, string _small, string _large, string _content_type, int _iframe, int _preview, int _enabled)
        {
            arParams = new SqlParameter[8];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@extension", _extension);
            arParams[2] = new SqlParameter("@small", _small);
            arParams[3] = new SqlParameter("@large", _large);
            arParams[4] = new SqlParameter("@content_type", _content_type);
            arParams[5] = new SqlParameter("@iframe", _iframe);
            arParams[6] = new SqlParameter("@preview", _preview);
            arParams[7] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateIcon", arParams);
        }
        public void Enable(int _id, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateIconEnabled", arParams);
        }
        public void Delete(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteIcon", arParams);
        }

        public string Get(string strExtension, bool _large)
        {
            DataSet ds = Gets(1);
            string strReturn = "";
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if ("." + dr["extension"].ToString().ToUpper() == strExtension.Trim().ToUpper())
                {
                    if (_large == true)
                        strReturn = dr["large"].ToString();
                    else
                        strReturn = dr["small"].ToString();
                    break;
                }
            }
            if (strReturn == "")
            {
                if (_large == true)
                    strReturn = "/images/icons_big/xxx.gif";
                else
                    strReturn = "/images/icons/xxx.gif";
            }
            return strReturn;
        }

        public string GetIcon(string _extension, bool _large)
        {
            DataSet ds = Gets(1);
            string strReturn = "";
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (dr["extension"].ToString().ToUpper() == _extension.Trim().ToUpper())
                {
                    if (_large == true)
                        strReturn = dr["large"].ToString();
                    else
                        strReturn = dr["small"].ToString();
                    break;
                }
            }
            if (strReturn == "")
            {
                if (_large == true)
                    strReturn = "/images/icons_big/xxx.gif";
                else
                    strReturn = "/images/icons/xxx.gif";
            }
            return strReturn;
        }
    }
}
