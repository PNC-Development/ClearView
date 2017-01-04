using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using System.IO;

namespace NCC.ClearView.Application.Core
{
	public class RequestItems
	{
		private string dsn = "";
		private int user = 0;
		private SqlParameter[] arParams;
        public RequestItems(int _user, string _dsn)
		{
			user = _user;
			dsn = _dsn;
		}
		public DataSet GetItem(int _id)
		{
			arParams = new SqlParameter[1];
			arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getRequestItem", arParams);
		}
        public string GetItem(int _id, string _column)
        {
            DataSet ds = GetItem(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public string GetItemName(int _id)
		{
			string strName = "Unavailable";
            try { strName = GetItem(_id).Tables[0].Rows[0]["name"].ToString(); }
			catch {}
			return strName;
		}
        public DataSet GetItemsManagers(int _applicationid, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@applicationid", _applicationid);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getRequestItemsManagers", arParams);
        }
        public DataSet GetItems(int _applicationid, int _show, int _enabled)
		{
			arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@applicationid", _applicationid);
            arParams[1] = new SqlParameter("@show", _show);
            arParams[2] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getRequestItems", arParams);
		}
        public DataSet GetItemActivities(int _applicationid)
		{
			arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@applicationid", _applicationid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getRequestItemActivities", arParams);
		}
        public int AddItem(int _applicationid, string _name, string _service_title, string _image, int _platformid, int _activity_type, int _show, int _display, int _enabled)
        {
			arParams = new SqlParameter[10];
            arParams[0] = new SqlParameter("@applicationid", _applicationid);
            arParams[1] = new SqlParameter("@name", _name);
            arParams[2] = new SqlParameter("@service_title", _service_title);
            arParams[3] = new SqlParameter("@image", _image);
            arParams[4] = new SqlParameter("@platformid", _platformid);
            arParams[5] = new SqlParameter("@activity_type", _activity_type);
            arParams[6] = new SqlParameter("@show", _show);
            arParams[7] = new SqlParameter("@display", _display);
            arParams[8] = new SqlParameter("@enabled", _enabled);
            arParams[9] = new SqlParameter("@id", SqlDbType.Int);
            arParams[9].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addRequestItem", arParams);
            return Int32.Parse(arParams[9].Value.ToString());
		}
        public void UpdateItem(int _id, int _applicationid, string _name, string _service_title, string _image, int _platformid, int _activity_type, int _show, int _enabled)
		{
			arParams = new SqlParameter[9];
			arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@applicationid", _applicationid);
            arParams[2] = new SqlParameter("@name", _name);
            arParams[3] = new SqlParameter("@service_title", _service_title);
            arParams[4] = new SqlParameter("@image", _image);
            arParams[5] = new SqlParameter("@platformid", _platformid);
            arParams[6] = new SqlParameter("@activity_type", _activity_type);
            arParams[7] = new SqlParameter("@show", _show);
            arParams[8] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateRequestItem", arParams);
		}
        public void UpdateItemOrder(int _id, int _display)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@display", _display);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateRequestItemOrder", arParams);
        }
        public void EnableItem(int _id, int _enabled) 
		{
			arParams = new SqlParameter[2];
			arParams[0] = new SqlParameter("@id", _id);
			arParams[1] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateRequestItemEnabled", arParams);
		}
        public void DeleteItem(int _id)
		{
			arParams = new SqlParameter[1];
			arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteRequestItem", arParams);
		}
        public int GetItemApplication(int _id)
        {
            int intApplication = 0;
            try { Int32.TryParse(GetItem(_id).Tables[0].Rows[0]["applicationid"].ToString(), out intApplication); }
            catch { }
            return intApplication;
        }


        public void AddForm(int _requestid, int _itemid, int _serviceid, int _number)
        {
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@itemid", _itemid);
            arParams[2] = new SqlParameter("@serviceid", _serviceid);
            arParams[3] = new SqlParameter("@number", _number);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addRequestForm", arParams);
        }
        public void UpdateFormDone(int _requestid, int _itemid, int _number, int _done)
        {
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@itemid", _itemid);
            arParams[2] = new SqlParameter("@number", _number);
            arParams[3] = new SqlParameter("@done", _done);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateRequestForm", arParams);
        }
        public void UpdateFormDone(int _requestid, int _itemid, int _number, int _done, int _automated)
        {
            arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@itemid", _itemid);
            arParams[2] = new SqlParameter("@number", _number);
            arParams[3] = new SqlParameter("@done", _done);
            arParams[4] = new SqlParameter("@automated", _automated);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateRequestForm", arParams);
        }
        public void UpdateForm(int _requestid, bool _forward)
        {
            DataSet dsForm = GetForms(_requestid);
            int intCount = -1;
            foreach (DataRow drForm in dsForm.Tables[0].Rows)
            {
                intCount++;
                if (drForm["done"].ToString() == "-1")
                    break;
            }
            if (intCount > -1)
            {
                if (_forward == true)
                {
                    // Move forward
                    UpdateFormDone(Int32.Parse(dsForm.Tables[0].Rows[intCount]["requestid"].ToString()), Int32.Parse(dsForm.Tables[0].Rows[intCount]["itemid"].ToString()), Int32.Parse(dsForm.Tables[0].Rows[intCount]["number"].ToString()), 0);
                }
                else if (intCount > 0)
                {
                    // Move Backward
                    UpdateFormDone(Int32.Parse(dsForm.Tables[0].Rows[intCount - 1]["requestid"].ToString()), Int32.Parse(dsForm.Tables[0].Rows[intCount - 1]["itemid"].ToString()), Int32.Parse(dsForm.Tables[0].Rows[intCount - 1]["number"].ToString()), -1);
                }
            }
        }
        public void UpdateForm(int _formid, int _number)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@formid", _formid);
            arParams[1] = new SqlParameter("@number", _number);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateRequestFormNumber", arParams);
        }
        public void DeleteForm(int _requestid, int _serviceid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@serviceid", _serviceid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteRequestForm", arParams);
            DataSet dsForms = GetForms(_requestid);
            int intFormNumber = 0;
            foreach (DataRow drForm in dsForms.Tables[0].Rows)
            {
                intFormNumber++;
                UpdateForm(Int32.Parse(drForm["formid"].ToString()), intFormNumber);
            }
        }
        public void DeleteForms(int _requestid, int _serviceid, int _number)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@serviceid", _serviceid);
            arParams[2] = new SqlParameter("@number", _number);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteRequestForms", arParams);
        }
        public DataSet GetForms(int _requestid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getRequestForms", arParams);
        }

        public DataSet GetForms(int _requestid, int _serviceid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@serviceid", _serviceid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getRequestFormsService", arParams);
        }

        public DataSet GetForm(int _requestid, int _serviceid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@serviceid", _serviceid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getRequestForm", arParams);
        }

        public DataSet GetForm(int _requestid, int _serviceid, int _itemid, int _number)
        {
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@serviceid", _serviceid);
            arParams[2] = new SqlParameter("@itemid", _itemid);
            arParams[3] = new SqlParameter("@number", _number);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getRequestForm", arParams);
        }
    }
}
