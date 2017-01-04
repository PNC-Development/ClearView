using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using System.Web.UI.WebControls;
using System.IO;

namespace NCC.ClearView.Application.Core
{
	public class WMServiceTasks
	{
		private string dsn = "";
		private int user = 0;
		private SqlParameter[] arParams;

        public WMServiceTasks(int _user, string _dsn)
		{
			user = _user;
			dsn = _dsn;
		}

        public int addWMServiceTaskStatus(
                                    int _taskStatusId,
                                    int _requestId,
                                    int _serviceId,
                                    int _itemId,
                                    int _number,
                                    int _taskId,
                                    int _assetId,
                                    double _hours,
                                    int _completedby, int _Iscompleted)

        {
            arParams = new SqlParameter[10];
            arParams[0] = new SqlParameter("@TaskStatusId", _taskStatusId);
            arParams[1] = new SqlParameter("@RequestId", _requestId);
            arParams[2] = new SqlParameter("@ServiceId", _serviceId);
            arParams[3] = new SqlParameter("@ItemId", _itemId);
            arParams[4] = new SqlParameter("@Number", _number);
            arParams[5] = new SqlParameter("@TaskId", _taskId);
            arParams[6] = new SqlParameter("@AssetId", _assetId);
            arParams[7] = new SqlParameter("@Hours", _hours);
            arParams[8] = new SqlParameter("@CompletedBy", _completedby);
            arParams[9] = new SqlParameter("@IsCompleted", _Iscompleted);
            arParams[0].Direction = ParameterDirection.InputOutput;
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addWMServiceTasksStatus", arParams);
            return Int32.Parse(arParams[0].Value.ToString());
        }

        public DataSet getWMServiceTasksStatus(int _requestId,
                                           int _serviceId,
                                           int _itemId,
                                           int _number,
                                           int _assetId)
        {
            arParams = new SqlParameter[10];
            arParams[0] = new SqlParameter("@RequestId", _requestId);
            arParams[1] = new SqlParameter("@ServiceId", _serviceId);
            arParams[2] = new SqlParameter("@ItemId", _itemId);
            arParams[3] = new SqlParameter("@Number", _number);
            arParams[4] = new SqlParameter("@AssetId", _assetId);

            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getWMServiceTasksStatus", arParams);
        }

        public DataSet getWMServiceTasksStatus(int _requestId,
                                             int _serviceId,
                                             int _itemId,
                                             int _number)
        {
            arParams = new SqlParameter[10];
            arParams[0] = new SqlParameter("@RequestId", _requestId);
            arParams[1] = new SqlParameter("@ServiceId", _serviceId);
            arParams[2] = new SqlParameter("@ItemId", _itemId);
            arParams[3] = new SqlParameter("@Number", _number);

            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getWMServiceTasksStatus", arParams);
        }

        public DataSet getWMServiceTasksStatus(int _taskStatusId)
        {
            arParams = new SqlParameter[10];
            arParams[0] = new SqlParameter("@TaskStatusId", _taskStatusId);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getWMServiceTasksStatus", arParams);
        }

        public void addRemoveWMServiceTasksStatusRequest(
                                 int _taskStatusId,
                                 int _requestId,
                                 int _createdBy,
                                 int _enabled,
                                 int _deleted
                                 )
        {
            arParams = new SqlParameter[10];
            arParams[0] = new SqlParameter("@TaskStatusId", _taskStatusId);
            arParams[1] = new SqlParameter("@RequestId", _requestId);
            arParams[2] = new SqlParameter("@CreatedBy", _createdBy);
            arParams[3] = new SqlParameter("@ModifiedBy", _createdBy);
            arParams[5] = new SqlParameter("@Enabled", _enabled);
            arParams[6] = new SqlParameter("@Deleted", _deleted);

            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addRemoveWMServiceTasksStatusRequest", arParams);
        }

        public DataSet getWMServiceTasksStatusRequest(int _taskStatusId )
        {
            arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@TaskStatusId", _taskStatusId);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getWMServiceTasksStatusRequest", arParams);
        }

        public DataSet getWMServiceTasksStatusRequestWithRequest(int _requestId)
        {
            arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@RequestId", _requestId);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getWMServiceTasksStatusRequest", arParams);
        }
        public bool IsWMServiceTaskCompleted(
                                             int _requestId,
                                             int _serviceId,
                                             int _itemId,
                                             int _number)
        {

            DataSet ds = getWMServiceTasksStatus(_requestId, _serviceId,_itemId,_number);
            foreach (DataRow drTasks in ds.Tables[0].Rows)
            {
                if (drTasks["Completed"].ToString() == "")
                    return false;
            }
            return true;
        }

        public bool IsWMServiceTaskCompleted(
                                            int _requestId,
                                            int _serviceId,
                                            int _itemId,
                                            int _number,
                                            int _assetId,
                                            ref string _comments)
        {

            DataSet ds = getWMServiceTasksStatus(_requestId, _serviceId, _itemId, _number, _assetId);
            foreach (DataRow drTasks in ds.Tables[0].Rows)
            {
                if (drTasks["Completed"].ToString() == "")
                    _comments = _comments + drTasks["TaskName"].ToString() + ", " + "\n";
            }
            if (_comments != "")
            {
                string[] strsplit ={ "," };
                _comments = _comments.Trim().Substring(0, _comments.Trim().Length - 1);
                _comments = "Please update the following information..." + "\n" + _comments;
                return false;
            }
            else
                return true;

        }

    }
}
