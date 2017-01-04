using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using System.IO;
using System.Text;

namespace NCC.ClearView.Application.Core
{
	public class Documents
	{
		private string dsn = "";
		private int user = 0;
		private SqlParameter[] arParams;
        public Documents(int _user, string _dsn)
		{
			user = _user;
			dsn = _dsn;
		}
		public DataSet Get(int _id)
		{
			arParams = new SqlParameter[1];
			arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDocument", arParams);
		}
		public string GetName(int _id)
		{
			string strName = "Unavailable";
			try { strName = Get(_id).Tables[0].Rows[0]["name"].ToString(); }
			catch {}
			return strName;
		}
        public DataSet GetsRequest(int _requestid, int _userid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@userid", _userid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDocumentsRequest", arParams);
        }
        public DataSet GetsService(int _requestid, int _serviceid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@serviceid", _serviceid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDocumentsService", arParams);
        }
        public DataSet GetsProject(int _projectid, int _userid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@projectid", _projectid);
            arParams[1] = new SqlParameter("@userid", _userid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDocumentsProject", arParams);
        }
        public DataSet GetsMine(int _projectid, int _userid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@projectid", _projectid);
            arParams[1] = new SqlParameter("@userid", _userid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDocumentsUser", arParams);
        }
        public DataSet GetsName(string _name)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@name", _name);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDocumentsName", arParams);
        }
        public void Add(int _projectid, int _requestid, string _name, string _path, string _description, int _security, int _userid)
        {
            arParams = new SqlParameter[7];
            arParams[0] = new SqlParameter("@projectid", _projectid);
            arParams[1] = new SqlParameter("@requestid", _requestid);
            arParams[2] = new SqlParameter("@name", _name);
            arParams[3] = new SqlParameter("@path", _path);
            arParams[4] = new SqlParameter("@description", _description);
            arParams[5] = new SqlParameter("@security", _security);
            arParams[6] = new SqlParameter("@userid", _userid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addDocument", arParams);
        }
        public void Update(int _documentid, string _name, string _description, int _security)
        {
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@documentid", _documentid);
            arParams[1] = new SqlParameter("@name", _name);
            arParams[2] = new SqlParameter("@description", _description);
            arParams[3] = new SqlParameter("@security", _security);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateDocument", arParams);
        }
        public void Update(int _requestid, int _projectid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@projectid", _projectid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateDocumentRequest", arParams);
        }
        public bool Delete(int _id, bool _delete, string _physical)
		{
            DataSet ds = Get(_id);
            bool boolDelete = false;
            if (ds.Tables[0].Rows.Count > 0 && _delete == true)
            {
                FileInfo oFile = new FileInfo(_physical + "uploads\\" + ds.Tables[0].Rows[0]["path"].ToString());
                try
                {
                    if (oFile.Exists == true)
                        oFile.Delete();
                    boolDelete = true;
                }
                catch { }
            }
			arParams = new SqlParameter[1];
			arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteDocument", arParams);
            return boolDelete;
		}
        public string GetDocuments_Project(int _projectid, int _userid, string _physical, int _security, bool _show_description)
        {
            DataSet ds = GetsProject(_projectid, _userid);
            string strDocuments = "<tr bgcolor=\"#EEEEEE\"><td><b></b></td><td><b>Name</b></td><td><b>Date</b></td><td><b>Size</b></td><td><b>Owner</b></td></tr>";
            return GetDocuments(ds, strDocuments, _physical, _security, _show_description, false);
        }
        public string GetDocuments_Request(int _requestid, int _userid, string _physical, int _security, bool _show_description)
        {
            DataSet ds = GetsRequest(_requestid, _userid);
            string strDocuments = "<tr bgcolor=\"#EEEEEE\"><td><b></b></td><td><b>Name</b></td><td><b>Date</b></td><td><b>Size</b></td><td><b>Security</b></td></tr>";
            return GetDocuments(ds, strDocuments, _physical, _security, _show_description, false);
        }
        public string GetDocuments_Service(int _requestid, int _serviceid, string _physical, int _security, bool _show_description)
        {
            DataSet ds = GetsService(_requestid, _serviceid);
            string strDocuments = "<tr bgcolor=\"#EEEEEE\"><td><b></b></td><td><b>Name</b></td><td><b>Date</b></td><td><b>Size</b></td><td><b>Security</b></td></tr>";
            return GetDocuments(ds, strDocuments, _physical, _security, _show_description, false);
        }
        public string GetDocuments_Mine(int _projectid, int _userid, string _physical, int _security, bool _show_description)
        {
            DataSet ds = GetsMine(_projectid, _userid);
            string strDocuments = "<tr bgcolor=\"#EEEEEE\"><td><b></b></td><td><b>Name</b></td><td><b>Date</b></td><td><b>Size</b></td><td><b>Security</b></td></tr>";
            return GetDocuments(ds, strDocuments, _physical, _security, _show_description, true);
        }
        //public string GetDocuments_Name(string _name, string _physical, int _security, bool _show_description)
        //{
        //    DataSet ds = GetsName(_name);
        //    string strDocuments = "<tr bgcolor=\"#EEEEEE\"><td><b></b></td><td><b>Name</b></td><td><b>Date</b></td><td><b>Size</b></td><td><b>Owner</b></td></tr>";
        //    return GetDocuments(ds, strDocuments, _physical, _security, _show_description, false);
        //}
        public string GetDocuments(DataSet ds, string strDocuments, string _physical, int _security, bool _show_description, bool _mine)
        {
            StringBuilder sbDocuments = new StringBuilder(strDocuments);

            Icons oIcon = new Icons(user, dsn);
            if (ds.Tables[0].Rows.Count == 0)
            {
                sbDocuments.Append("<tr><td colspan=\"5\">&nbsp;<img src='/images/alert.gif' border='0' align='absmiddle'> No documents</td></tr>");
            }
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                FileInfo oFile = new FileInfo(_physical + "uploads\\" + dr["path"].ToString());
                bool boolDepartment = false;
                if (dr["path"].ToString().Contains("\\department") == true)
                {
                    oFile = new FileInfo(dr["path"].ToString());
                    boolDepartment = true;
                }
                if (oFile.Exists == true)
                {
                    if (boolDepartment)
                        sbDocuments.Append("<tr onmouseover=\"CellRowOver(this);\" onmouseout=\"CellRowOut(this);\" onclick=\"OpenWindow('FILE','?repository=");
                    else
                        sbDocuments.Append("<tr onmouseover=\"CellRowOver(this);\" onmouseout=\"CellRowOut(this);\" onclick=\"OpenWindow('FILE','?id=");
                    sbDocuments.Append(dr["documentid"].ToString());
                    sbDocuments.Append("')\">");
                    sbDocuments.Append("<td nowrap><img src=\"");
                    sbDocuments.Append(oIcon.GetIcon(oFile.Extension, false));
                    sbDocuments.Append("\" border=\"0\" /></td>");
                    sbDocuments.Append("<td width=\"50%\">");
                    sbDocuments.Append(dr["name"].ToString());
                    sbDocuments.Append("</td>");
                    sbDocuments.Append("<td width=\"20%\">");
                    sbDocuments.Append(oFile.LastWriteTime.ToShortDateString());
                    sbDocuments.Append(" ");
                    sbDocuments.Append(oFile.LastWriteTime.ToShortTimeString());
                    sbDocuments.Append("</td>");
                    // Get size of file
                    decimal oFileSize = oFile.Length / 1024;
                    if (oFileSize > 1024)
                    {
                        sbDocuments.Append("<td width=\"15%\">");
                        sbDocuments.Append((Decimal.Round((oFileSize / 1024), 1)).ToString());
                        sbDocuments.Append(" MB</td>");
                    }
                    else
                    {
                        sbDocuments.Append("<td width=\"15%\">");
                        sbDocuments.Append((Decimal.Round(oFileSize, 0)).ToString());
                        sbDocuments.Append(" KB</td>");
                    }
                }
                else
                {
                    sbDocuments.Append("<tr>");
                    sbDocuments.Append("<td nowrap><img src=\"/images/error.gif\" border=\"0\" /></td>");
                    sbDocuments.Append("<td width=\"50%\">");
                    sbDocuments.Append(dr["name"].ToString());
                    sbDocuments.Append("</td>");
                    sbDocuments.Append("<td colspan=\"2\">File Not Found</td>");
                }
                if (_mine == true)
                {
                    sbDocuments.Append("<td width=\"15%\">");
                    sbDocuments.Append(GetSecurity(Int32.Parse(dr["security"].ToString())));
                    sbDocuments.Append("</td>");
                }
                else
                {
                    sbDocuments.Append("<td width=\"15%\">");
                    sbDocuments.Append(dr["username"].ToString());
                    sbDocuments.Append("</td>");
                }
                sbDocuments.Append("</tr>");
                if (_show_description == true)
                {
                    sbDocuments.Append("</tr><tr><td></td><td colspan=\"4\" class=\"document_description\">");
                    sbDocuments.Append(dr["description"].ToString());
                    sbDocuments.Append("</td></tr>");
                }
            }
            if (sbDocuments.ToString() != "")
            {
                sbDocuments.Insert(0, "<table width=\"100%\" cellpadding=\"3\" cellspacing=\"0\" border=\"0\" style=\"border:solid 1px #CCCCCC\">");
                sbDocuments.Append("</table>");
            }
            return sbDocuments.ToString();
        }
        public bool CanView(int _documentid, int _userid)
        {
            DataSet ds = Get(_documentid);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (Int32.Parse(ds.Tables[0].Rows[0]["userid"].ToString()) == _userid || Int32.Parse(ds.Tables[0].Rows[0]["security"].ToString()) == 1)
                    return true;
                else
                {
                    ds = GetPermissions(_documentid);
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        if (Int32.Parse(dr["userid"].ToString()) == _userid && Int32.Parse(dr["security"].ToString()) >= 1)
                            return true;
                    }
                    return false;
                }
            }
            else
                return false;
        }
        public bool CanEdit(int _documentid, int _userid)
        {
            DataSet ds = Get(_documentid);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (Int32.Parse(ds.Tables[0].Rows[0]["userid"].ToString()) == _userid)
                    return true;
                else
                {
                    ds = GetPermissions(_documentid);
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        if (Int32.Parse(dr["userid"].ToString()) == _userid && Int32.Parse(dr["security"].ToString()) >= 10)
                            return true;
                    }
                    return false;
                }
            }
            else
                return false;
        }
        public string GetSecurity(int _security)
        {
            if (_security == -1)
                return "Private";
            else if (_security == 0)
                return "Shared";
            else if (_security == 1)
                return "Public";
            else
                return "Unknown (" + _security.ToString() + ")";
        }
        public DataSet GetPermissions(int _documentid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@documentid", _documentid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDocumentPermissions", arParams);
        }
        public void AddPermission(int _documentid, int _userid, int _security)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@documentid", _documentid);
            arParams[1] = new SqlParameter("@userid", _userid);
            arParams[2] = new SqlParameter("@security", _security);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addDocumentPermission", arParams);
        }
        public void DeletePermission(int _documentid, int _userid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@documentid", _documentid);
            arParams[1] = new SqlParameter("@userid", _userid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteDocumentPermission", arParams);
        }
        public int GetDocumentCount(string _strPath)
        {
            return int.Parse(SqlHelper.ExecuteScalar(dsn, CommandType.Text, "select count(*) from cv_document_repository where path like '" + _strPath + "/%'").ToString());
        }
        public DataSet GetDocumentID(string _strPath)
        {
            return SqlHelper.ExecuteDataset(dsn, CommandType.Text, "select id from cv_document_repository where path like '" + _strPath + "/%'");
        }
    }
}