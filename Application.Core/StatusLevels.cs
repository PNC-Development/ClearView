using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;

namespace NCC.ClearView.Application.Core
{
	public class StatusLevels
    {
        private string dsn = "";
        private int user = 0;
        private SqlParameter[] arParams;
        public StatusLevels()
		{
		}
        public StatusLevels(int _user, string _dsn)
		{
			user = _user;
			dsn = _dsn;
		}
		public string Name(int _status)
		{
            string strStatus = "Unknown Error";
            switch (_status)
            {
                case -10:
                    strStatus = "Not Available";
                    break;
                case -2:
                    strStatus = "Cancelled";
                    break;
                case -1:
                    strStatus = "Denied";
                    break;
                case 0:
                    strStatus = "Pending";
                    break;
                case 1:
                    strStatus = "Approved";
                    break;
                case 2:
                    strStatus = "Active";
                    break;
                case 3:
                    strStatus = "Closed";
                    break;
                case 5:
                    strStatus = "On Hold";
                    break;
                case 7:
                    strStatus = "Awaiting Client Response";
                    break;
                case 10:
                    strStatus = "Shelved";
                    break;
            }
            return strStatus;
		}
        public string HTML(int _status)
        {
            string strStatus = "<span class=\"denied\">Unknown Error</span>";
            switch (_status)
            {
                case -10:
                    strStatus = "<span class=\"default\">Not Available</span>";
                    break;
                case -2:
                    strStatus = "<span class=\"denied\">Cancelled</span>";
                    break;
                case -1:
                    strStatus = "<span class=\"denied\">Denied</span>";
                    break;
                case 0:
                    strStatus = "<span class=\"pending\">Pending</span>";
                    break;
                case 1:
                    strStatus = "<span class=\"approved\">Approved</span>";
                    break;
                case 2:
                    strStatus = "<span class=\"approved\">Active</span>";
                    break;
                case 3:
                    strStatus = "<span class=\"default\">Closed</span>";
                    break;
                case 5:
                    strStatus = "<span class=\"shelved\">On Hold</span>";
                    break;
                case 7:
                    strStatus = "<span class=\"pending\">Awaiting Client Response</span>";
                    break;
                case 10:
                    strStatus = "<span class=\"shelved\">Shelved</span>";
                    break;
            }
            return strStatus;
        }
        public string List()
        {
            return "SELECT -2 AS id, 'Cancelled' AS name UNION ALL SELECT -1 AS id, 'Denied' AS name UNION ALL SELECT 0 AS id, 'Pending' AS name UNION ALL SELECT 1 AS id, 'Approved' AS name UNION ALL SELECT 2 AS id, 'Active' AS name UNION ALL SELECT 3 AS id, 'Closed' AS name UNION ALL SELECT 5 AS id, 'On Hold' AS name UNION ALL SELECT 7 AS id, 'Awaiting Client Response' AS name UNION ALL SELECT 10 AS id, 'Shelved' AS name";
        }

        public string RequestStatusList()
        {
            return "SELECT -2 AS id, 'Cancelled' AS name UNION ALL SELECT -10 AS id, 'Deleted' AS name UNION ALL SELECT -1 AS id, 'Denied' AS name UNION ALL SELECT 0 AS id, 'Pending' AS name UNION ALL SELECT 2 AS id, 'Active' AS name UNION ALL SELECT 3 AS id, 'Closed' AS name UNION ALL SELECT 5 AS id, 'On Hold' AS name UNION ALL SELECT 10 AS id, 'Shelved' AS name";
        }

        public string ProjectStatusList()
        {
            return "Select  -10 as Id, 'Deleted' as Name UNION ALL Select  -2 as Id, 'Cancelled' as Name UNION ALL Select  -1 as Id, 'Denied' as Name UNION ALL Select  0 as Id, 'Pending' as Name UNION ALL Select  1 as Id, 'Approved' as Name UNION ALL Select  2 as Id, 'Active' as Name UNION ALL Select  3 as Id, 'Completed' as Name UNION ALL Select  5 as Id, 'Hold' as Name UNION ALL Select  10 as Id, 'Future' as Name";
        }

        public string SupportStatusList()
        {
            return "Select -2 as Id, 'Cancelled' as Name UNION ALL Select -1 as Id, 'Denied' as Name UNION ALL Select 0 as Id, 'Under Review' as Name UNION ALL Select 2 as Id, 'Under Development' as Name UNION ALL Select 4 as Id, 'Ready for Release' as Name UNION ALL Select 3 as Id, 'Completed' as Name UNION ALL Select 5 as Id, 'On Hold' as Name UNION ALL Select 7 as Id, 'Awaiting Client Response' as Name";
        }
        public string HTMLSupport(int _status)
        {
            string strStatus = "<span class=\"denied\">Unknown Error</span>";
            switch (_status)
            {
                case -10:
                    strStatus = "<span class=\"default\">Not Available</span>";
                    break;
                case -2:
                    strStatus = "<span class=\"denied\">Cancelled</span>";
                    break;
                case -1:
                    strStatus = "<span class=\"denied\">Denied</span>";
                    break;
                case 0:
                    strStatus = "<span class=\"pending\">Under Review</span>";
                    break;
                case 1:
                    strStatus = "<span class=\"approved\">N / A</span>";
                    break;
                case 2:
                    strStatus = "<span class=\"approved\">Under Development</span>";
                    break;
                case 3:
                    strStatus = "<span class=\"default\">Completed</span>";
                    break;
                case 4:
                    strStatus = "<span class=\"approved\">Ready for Release</span>";
                    break;
                case 5:
                    strStatus = "<span class=\"shelved\">On Hold</span>";
                    break;
                case 7:
                    strStatus = "<span class=\"pending\">Awaiting Client Response</span>";
                    break;
            }
            return strStatus;
        }

        public DataSet GetStatusDescription(string _statusKey ,int _StatusValue)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@StatusKey", _statusKey);
            arParams[1] = new SqlParameter("@StatusValue", _StatusValue);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_GetStatusList", arParams);
        }

        public DataSet GetStatusList(string _statusKey)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@StatusKey", _statusKey);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_GetStatusList", arParams);
        }
    }
}
