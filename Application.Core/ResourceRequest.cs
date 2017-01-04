using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using System.Data.SqlTypes;
using System.IO;
using System.Web;
using System.Text;
using System.Collections.Generic;

namespace NCC.ClearView.Application.Core
{
    public class WorkflowStatus
    {
        public int requestid;
        public string service;
        public int resourceid;
        public int itemid;
        public int serviceid;
        public int number;
        public int workflowid;
        public string status;
        public string[] users;
        public string comments;
    }
    public enum ResourceRequestStatus
    {
        NotAvailable = -10,
        Cancelled = -2,
        Denied = -1,
        Pending = 0,
        Approved = 1,
        Active = 2,
        Closed = 3,
        OnHold = 5,
        AwaitingResponse = 7,
        Future = 10,
        Reserved = 100
    }
    public class ResourceRequest
	{
		private string dsn = "";
		private int user = 0;
        private List<int> lstWorkflow;
		private SqlParameter[] arParams;
        public ResourceRequest(int _user, string _dsn)
		{
			user = _user;
			dsn = _dsn;
        }
        #region General
        public DataSet GetProjectUser(int _projectid, int _userid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@projectid", _projectid);
            arParams[1] = new SqlParameter("@userid", _userid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getResourceRequestsProjectUser", arParams);
        }
        public DataSet GetUnAssigned(int _requestid, int _deleted)
        {
            // called from /controls/resource_request_workflow.ascx
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@deleted", _deleted);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getResourceRequestsUnassigned", arParams);
        }
        public int GetSLA(int _id)
        {
            Services oService = new Services(user, dsn);
            int intService = Int32.Parse(Get(_id, "serviceid"));
            double dblSLA = double.Parse(oService.Get(intService, "sla"));
            if (dblSLA == 0.00)
                return -99999;
            else
            {
                Holidays oHoliday = new Holidays(user, dsn);
                DateTime datAssigned = DateTime.Now;
                if (DateTime.TryParse(Get(_id, "assigned"), out datAssigned) == true)
                {
                    DateTime _end = oHoliday.GetHours(dblSLA, datAssigned);
                    TimeSpan oSpan = _end.Subtract(DateTime.Now);
                    return oSpan.Days;
                }
                else
                    return -1;
            }
        }
        //public DataSet GetRequestUnApproved(int _requestid)
        //{
        //    arParams = new SqlParameter[1];
        //    arParams[0] = new SqlParameter("@requestid", _requestid);
        //    return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getResourceRequestsRequestUnApproved", arParams);
        //}
        public DataSet GetRequestUnApprovedAll(int _requestid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getResourceRequestsRequestUnApprovedAll", arParams);
        }
        public double GetAllocated(int _projectid, int _userid, int _itemid)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@projectid", _projectid);
            arParams[1] = new SqlParameter("@userid", _userid);
            arParams[2] = new SqlParameter("@itemid", _itemid);
            object o = SqlHelper.ExecuteScalar(dsn, CommandType.StoredProcedure, "pr_getResourceRequestHoursAllocated", arParams);
            if (o == null || o.ToString() == "")
                return 0.00;
            else
                return double.Parse(o.ToString());
        }
        public double GetUsed(int _projectid, int _userid, int _itemid)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@projectid", _projectid);
            arParams[1] = new SqlParameter("@userid", _userid);
            arParams[2] = new SqlParameter("@itemid", _itemid);
            object o = SqlHelper.ExecuteScalar(dsn, CommandType.StoredProcedure, "pr_getResourceRequestHoursUsed", arParams);
            if (o == null || o.ToString() == "")
                return 0.00;
            else
                return double.Parse(o.ToString());
        }
        public double GetAllocatedRequest(int _requestid, int _userid, int _itemid)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@userid", _userid);
            arParams[2] = new SqlParameter("@itemid", _itemid);
            object o = SqlHelper.ExecuteScalar(dsn, CommandType.StoredProcedure, "pr_getResourceRequestHoursAllocatedRequest", arParams);
            if (o == null || o.ToString() == "")
                return 0.00;
            else
                return double.Parse(o.ToString());
        }
        public double GetUsedRequest(int _requestid, int _userid, int _itemid)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@userid", _userid);
            arParams[2] = new SqlParameter("@itemid", _itemid);
            object o = SqlHelper.ExecuteScalar(dsn, CommandType.StoredProcedure, "pr_getResourceRequestHoursUsedRequest", arParams);
            if (o == null || o.ToString() == "")
                return 0.00;
            else
                return double.Parse(o.ToString());
        }
        public string GetUpdated(int _parent)
        {
            DataSet ds = GetWorkflowsParent(_parent);
            DateTime datNow = DateTime.Now;
            DateTime datCompare = datNow;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (dr["modified"].ToString() != "")
                {
                    DateTime datModified = DateTime.Parse(dr["modified"].ToString());
                    if (datModified > datCompare)
                        datCompare = datModified;
                }
            }
            if (datCompare == datNow)
                return "---";
            else
                return datCompare.ToString();
        }
        public int GetNumber(int _requestid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            DataSet ds = SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getResourceRequestNumber", arParams);
            int intNumber = 0;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (Int32.Parse(dr["number"].ToString()) > intNumber)
                    intNumber = Int32.Parse(dr["number"].ToString());
            }
            return (intNumber + 1);
        }
        public int GetNumber(int _requestid, int _itemid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@itemid", _itemid);
            DataSet ds = SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getResourceRequestNumberItem", arParams);
            int intNumber = 0;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (Int32.Parse(dr["number"].ToString()) > intNumber)
                    intNumber = Int32.Parse(dr["number"].ToString());
            }
            return (intNumber + 1);
        }
        public void ApprovePlatform(int _id, int _platform_approval)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@platform_approval", _platform_approval);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateResourceRequestApprovePlatform", arParams);
        }
        public void UpdateName(int _parent, string _name)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _parent);
            arParams[1] = new SqlParameter("@name", _name);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateResourceRequestName", arParams);
            string strRequest = Get(_parent, "requestid");
            if (strRequest != "")
            {
                int intRequest = Int32.Parse(strRequest);
                ServiceRequests oServiceRequest = new ServiceRequests(0, dsn);
                DataSet dsSR = oServiceRequest.Get(intRequest);
                if (dsSR.Tables[0].Rows.Count > 0 && dsSR.Tables[0].Rows[0]["name"].ToString().Trim() == "")
                    oServiceRequest.Update(intRequest, _name);
                else if (dsSR.Tables[0].Rows.Count > 0 && dsSR.Tables[0].Rows[0]["name"].ToString().Trim() != "" && _name == "")
                    UpdateName(_parent, dsSR.Tables[0].Rows[0]["name"].ToString().Trim());
            }
        }
        public DataSet GetWorkflowRequestAll(int _requestid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getResourceRequestsWorkflowRequestAll", arParams);
        }
        private void UpdateAllocated(int _parent, double _allocated)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _parent);
            arParams[1] = new SqlParameter("@allocated", _allocated);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateResourceRequestAllocated", arParams);
        }
        public void UpdateDevices(int _parent, int _devices, double _allocated)
        {
            if (_devices == 0)
                _devices = 1;
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@id", _parent);
            arParams[1] = new SqlParameter("@devices", _devices);
            arParams[2] = new SqlParameter("@allocated", _allocated);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateResourceRequestDevices", arParams);
        }
        public DataSet GetRequestService(int _requestid, int _serviceid, int _number)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@serviceid", _serviceid);
            arParams[2] = new SqlParameter("@number", _number);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getResourceRequestsRequestService", arParams);
        }
        public void UpdateRejected(int _parent, int _accepted)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _parent);
            arParams[1] = new SqlParameter("@accepted", _accepted);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateResourceRequestRejected", arParams);
        }
        public void UpdateAccepted(int _parent, int _accepted)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _parent);
            arParams[1] = new SqlParameter("@accepted", _accepted);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateResourceRequestAccepted", arParams);
        }
        public void UpdateAssignedBy(int _parent, int _userid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _parent);
            arParams[1] = new SqlParameter("@userid", _userid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateResourceRequestAssignedBy", arParams);
        }
        public void UpdateAssignedByReset(int _parent)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _parent);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateResourceRequestAssignedByReset", arParams);
        }
        public void UpdateReason(int _parent, string _reason)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _parent);
            arParams[1] = new SqlParameter("@reason", _reason);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateResourceRequestReason", arParams);
        }
        public void UpdateComments(int _parent, string _comments)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _parent);
            arParams[1] = new SqlParameter("@comments", _comments);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateResourceRequestComments", arParams);
        }
        public void UpdateManager(int _id, int _requestid, int _itemid, int _number, int _devices, double _allocated, int _accepted, int _status)
        {
            if (_devices == 0)
                _devices = 1;
            int intItem = Int32.Parse(Get(_id, "itemid"));
            arParams = new SqlParameter[6];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@itemid", _itemid);
            arParams[2] = new SqlParameter("@devices", _devices);
            arParams[3] = new SqlParameter("@allocated", _allocated);
            arParams[4] = new SqlParameter("@accepted", _accepted);
            arParams[5] = new SqlParameter("@status", _status);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateResourceRequestManager", arParams);

            DataSet ds = SqlHelper.ExecuteDataset(dsn, CommandType.Text, "select name from sys.tables order by name");
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                string strName = dr["name"].ToString();
                if (strName.Trim().ToUpper().StartsWith("CV_WM_") == true)
                {
                    try { SqlHelper.ExecuteNonQuery(dsn, CommandType.Text, "UPDATE " + strName.Trim() + " SET itemid = " + _itemid.ToString() + " WHERE requestid = " + _requestid.ToString() + " AND itemid = " + intItem.ToString() + " AND number = " + _number.ToString()); }
                    catch { }
                }
            }
        }
        public void UpdateItemAndService(int _parent, int _itemid, int _serviceid)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@id", _parent);
            arParams[1] = new SqlParameter("@itemid", _itemid);
            arParams[2] = new SqlParameter("@serviceid", _serviceid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateResourceRequestItemAndService", arParams);
        }
        public bool IsAllocated(int _parent)
        {
            int intHours = Int32.Parse(Get(_parent, "allocated"));
            return (intHours > 0);
        }
        public void Delete(int _parent)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _parent);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteResourceRequest", arParams);
        }
        public bool IsOpen(int _parent)
        {
            bool boolOpen = false;
            DataSet ds = GetWorkflowsParent(_parent);
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (Int32.Parse(dr["status"].ToString()) == (int)ResourceRequestStatus.Active || Int32.Parse(dr["status"].ToString()) == (int)ResourceRequestStatus.Approved || Int32.Parse(dr["status"].ToString()) == (int)ResourceRequestStatus.Pending)
                    {
                        boolOpen = true;
                        break;
                    }
                }
            }
            else
            {
                ds = Get(_parent);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    if (Int32.Parse(dr["status"].ToString()) == (int)ResourceRequestStatus.Active || Int32.Parse(dr["status"].ToString()) == (int)ResourceRequestStatus.Approved || Int32.Parse(dr["status"].ToString()) == (int)ResourceRequestStatus.Pending)
                        boolOpen = true;
                }
            }
            return boolOpen;
        }
        public int GetStatusParent(int _parent)
        {
            DataSet dsParent = GetWorkflowsParent(_parent);
            int intStatus = 3;
            foreach (DataRow drParent in dsParent.Tables[0].Rows)
            {
                if (drParent["status"].ToString() != "3")
                {
                    intStatus = Int32.Parse(drParent["status"].ToString());
                    break;
                }
            }
            return intStatus;
        }
        public string GetStatus(int _parent, int _id)
        {
            //int _parent = Int32.Parse(lblProgress.Text);
            string strStatus = "";
            Requests oRequest = new Requests(user, dsn);
            ServiceRequests oServiceRequest = new ServiceRequests(user, dsn);
            RequestItems oRequestItem = new RequestItems(user, dsn);
            StatusLevels oStatusLevel = new StatusLevels();
            Services oService = new Services(user, dsn);
            if (_parent == 0 && _id > 0)
                _parent = GetWorkflowParent(_id);
            if (_parent > 0)
            {
                DataSet ds = Get(_parent);
                if (ds.Tables[0].Rows.Count == 0)
                    strStatus = "<span class=\"denied\">Deleted</span>";
                else
                {
                    int intRequest = Int32.Parse(ds.Tables[0].Rows[0]["requestid"].ToString());
                    int intItem = Int32.Parse(ds.Tables[0].Rows[0]["itemid"].ToString());
                    int intService = Int32.Parse(ds.Tables[0].Rows[0]["serviceid"].ToString());
                    int intNumber = Int32.Parse(ds.Tables[0].Rows[0]["number"].ToString());
                    DataSet dsSR = oServiceRequest.Get(intRequest);
                    if (dsSR.Tables[0].Rows.Count > 0)
                    {
                        if (dsSR.Tables[0].Rows[0]["cancelled"].ToString() != "") 
                            strStatus = "<span class=\"denied\">Cancelled</span>";
                        else if (dsSR.Tables[0].Rows[0]["checkout"].ToString() != "1")
                            strStatus = "<span class=\"default\">Awaiting Checkout</span>";
                        else if (dsSR.Tables[0].Rows[0]["checkout"].ToString() == "1")
                        {
                            DataSet dsForm = oRequestItem.GetForms(intRequest);
                            foreach (DataRow drForm in dsForm.Tables[0].Rows)
                            {
                                if ((string)drForm["done"] != "1")
                                {
                                    strStatus = "<span class=\"default\">Checkout</span>";
                                    break;
                                }
                            }
                        }
                    }
                    if (strStatus == "")
                    {
                        if (oService.Get(intService, "automate") == "1")
                        {
                            // Automated Task
                            strStatus = "<span class=\"approved\">Completed (Automated)</span>";
                        }
                        else
                        {
                            if (Get(_parent, "accepted") == "-1")
                                strStatus = "<span class=\"denied\">Rejected</span>";
                            else if (Get(_parent, "assigned") == "")
                                strStatus = "<span class=\"shelved\">Awaiting Assignment</span>";
                            else if (_id > 0)
                                strStatus = oStatusLevel.HTML(GetStatusParent(_parent));
                        }
                    }
                }

                if (strStatus == "" && _id > 0)
                {
                    DataSet dsW = GetWorkflow(_id);
                    if (dsW.Tables[0].Rows.Count == 0)
                        strStatus = "<span class=\"denied\">Deleted</span>";
                    else
                        strStatus = oStatusLevel.HTML(Int32.Parse(dsW.Tables[0].Rows[0]["status"].ToString()));
                }
            }
            else
                strStatus = "Invalid Paramater";
            return strStatus;
        }
        public void UpdateStatusOverall(int _parent, int _status)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _parent);
            arParams[1] = new SqlParameter("@status", _status);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateResourceRequestStatus", arParams);
            if (_status == (int)ResourceRequestStatus.Closed)
                Update(_parent, DateTime.Now.ToString());
            else
                Update(_parent, "");
        }
        public void UpdateStatusOverallWorkflow(int _parent, int _status)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _parent);
            arParams[1] = new SqlParameter("@status", _status);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateResourceRequestStatusWorkflow", arParams);
            if (_status == (int)ResourceRequestStatus.Closed)
                Update(_parent, DateTime.Now.ToString());
            else
                Update(_parent, "");
        }
        public void UpdateCompleted(int _requestid, int _itemid, int _serviceid, int _number, string _completed)
        {
            arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@itemid", _itemid);
            arParams[2] = new SqlParameter("@serviceid", _serviceid);
            arParams[3] = new SqlParameter("@number", _number);
            arParams[4] = new SqlParameter("@completed", (_completed == "" ? SqlDateTime.Null : DateTime.Parse(_completed)));
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateResourceRequestsCompleted", arParams);
        }
        public void UpdateStatusRequest(int _requestid, int _status)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@status", _status);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateResourceRequestStatusRequest", arParams);
        }
        public void Update(int _parent, string _completed)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _parent);
            arParams[1] = new SqlParameter("@completed", (_completed == "" ? SqlDateTime.Null : DateTime.Parse(_completed)));
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateResourceRequest", arParams);
        }
        #endregion


        #region WORKFLOW
        public void DeleteWorkflow(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteResourceRequestWorkflow", arParams);
        }
        public void UpdateWorkflowCompleted(int _id, string _completed)
        {
            Update(GetWorkflowParent(_id), _completed);
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@completed", (_completed == "" ? SqlDateTime.Null : DateTime.Parse(_completed)));
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateResourceRequestWorkflowCompleted", arParams);
        }
        public DataSet GetWorkflowService(int _serviceid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@serviceid", _serviceid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getResourceRequestsWorkflowService", arParams);
        }
        public void UpdateWorkflowAllocated(int _id, double _allocated)
        {
            Update(GetWorkflowParent(_id), "");
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@allocated", _allocated);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateResourceRequestWorkflowAllocated", arParams);
        }
        public void UpdateWorkflowAllocated(int _serviceid)
        {
            DataSet ds = GetWorkflowService(_serviceid);
            ServiceDetails oServiceDetail = new ServiceDetails(user, dsn);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                double dblAllocated = oServiceDetail.GetHours(_serviceid, double.Parse(dr["devices"].ToString()));
                int intResourceWorkflow = Int32.Parse(dr["id"].ToString());
                int intResourceParent = GetWorkflowParent(intResourceWorkflow);
                UpdateWorkflowAllocated(intResourceWorkflow, dblAllocated);
                UpdateAllocated(intResourceParent, dblAllocated);
            }
        }
        public string GetWorkflowSummary(int _id, int _environment, string _se_dsn, string _dsn_asset, string _dsn_ip)
        {
            int intResourceParent = GetWorkflowParent(_id);
            return GetSummary(intResourceParent, _id, _environment, _se_dsn, _dsn_asset, _dsn_ip);
        }
        public string GetSummary(int _parent, int _id, int _environment, string _se_dsn, string _dsn_asset, string _dsn_ip)
        {
            RequestFields oRequestField = new RequestFields(user, dsn);
            return oRequestField.GetBodyOverall(_parent, _id, _se_dsn, _environment, _dsn_asset, _dsn_ip);
        }
        public string GetBodyOverallFix(int _parent, int _resourceid, int _environment, bool _highlight)
        {
            Variables oVariable = new Variables(_environment);
            Users oUser = new Users(user, dsn);
            Projects oProject = new Projects(user, dsn);
            Requests oRequest = new Requests(user, dsn);
            Functions oFunction = new Functions(0, dsn, 0);
            StringBuilder sbBody = new StringBuilder();
            string strSpacerRow = "<tr><td colspan=\"3\"><img src=\"" + oVariable.ImageURL() + "/images/spacer.gif\" border=\"0\" width=\"1\" height=\"7\" /></td></tr>";
            DataSet ds = Get(_parent);
            if (ds.Tables[0].Rows.Count > 0)
            {
                int intRequest = Int32.Parse(ds.Tables[0].Rows[0]["requestid"].ToString());
                DataSet dsRequest = oRequest.Get(intRequest);
                int intProject = Int32.Parse(dsRequest.Tables[0].Rows[0]["projectid"].ToString());
                if (intProject > 0)
                {
                    // Show project info
                    DataSet dsProject = oProject.Get(intProject);
                    if (dsProject.Tables[0].Rows.Count > 0)
                    {
                        if (dsProject.Tables[0].Rows[0]["number"].ToString() != "")
                        {
                            sbBody.Append("<tr><td nowrap><b>Project Number:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                            sbBody.Append(dsProject.Tables[0].Rows[0]["number"].ToString());
                            if (_highlight == true)
                            {
                                sbBody.Append("&nbsp;&nbsp;&nbsp;<span style=\"color:#990000; padding:3px; background-color:#FFFFCC\"><b>Note:</b> Please retain this project number for future reference.</span>");
                            }
                            sbBody.Append("</td></tr>");
                            sbBody.Append(strSpacerRow);
                        }
                        sbBody.Append("<tr><td nowrap><b>Project Name:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                        sbBody.Append(dsProject.Tables[0].Rows[0]["name"].ToString());
                        sbBody.Append("</td></tr>");
                        sbBody.Append(strSpacerRow);
                    }
                }
                else
                {
                    // Show request info
                    sbBody.Append("<tr><td nowrap><b>Task Number:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td><a href=\"javascript:void(0);\" class=\"lookup\" onclick=\"OpenNewWindowMenu('");
                    sbBody.Append(oRequest.GetDataPointLink(intRequest, _environment));
                    sbBody.Append("', '800', '600');\">CVT");
                    sbBody.Append(intRequest.ToString());
                    sbBody.Append("</a>");
                    if (_highlight == true)
                    {
                        sbBody.Append("&nbsp;&nbsp;&nbsp;<span style=\"color:#990000; padding:3px; background-color:#FFFFCC\"><b>Note:</b> Please retain this task number for future reference.</span>");
                    }
                    sbBody.Append("</td></tr>");
                    sbBody.Append(strSpacerRow);
                    ServiceRequests oServiceRequest = new ServiceRequests(user, dsn);
                    sbBody.Append("<tr><td nowrap><b>Task Name:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                    string strTaskName = "N/A";
                    if (_resourceid > 0)
                        strTaskName = GetWorkflow(_resourceid, "name").Trim();
                    if ((strTaskName == "" || strTaskName == "N/A") && _parent > 0)
                        strTaskName = Get(_parent, "name").Trim();
                    if ((strTaskName == "" || strTaskName == "N/A") && intRequest > 0)
                        strTaskName = oServiceRequest.Get(intRequest, "name").Trim();
                    sbBody.Append(strTaskName);
                    sbBody.Append("</td></tr>");
                    sbBody.Append(strSpacerRow);
                }
                double dblHours = double.Parse(ds.Tables[0].Rows[0]["allocated"].ToString());
                if (dblHours > 0.00)
                {
                    sbBody.Append("<tr><td nowrap><b>Hours Allocated:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                    sbBody.Append(dblHours.ToString("F"));
                    sbBody.Append("</td></tr>");
                    sbBody.Append(strSpacerRow);
                }
                sbBody.Append("<tr><td nowrap><b>Submitter:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                sbBody.Append(oUser.GetFullName(oRequest.GetUser(intRequest)));
                sbBody.Append("</td></tr>");
                if (sbBody.ToString() != "")
                {
                    sbBody.Insert(0, "<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"" + oVariable.DefaultFontStyle() + "\">");
                    sbBody.Append("</table>");
                }
            }
            else
            {
                sbBody = new StringBuilder("Unavailable");
            }
            return sbBody.ToString();
        }
        //public string GetWorkflowBody(int _id, int _environment, bool _highlight)
        //{
        //    // WORKFLOWFIX
        //    return GetBodyOverall(GetWorkflowParent(_id), _id, _environment, _highlight);
        //}
        public DataSet GetWorkflowProject(int _projectid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@projectid", _projectid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getResourceRequestsWorkflowProject", arParams);
        }
        //public DataSet GetWorkflowAssigned(int _userid)
        //{
        //    arParams = new SqlParameter[1];
        //    arParams[0] = new SqlParameter("@userid", _userid);
        //    return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getResourceRequestsWorkflowAssigned", arParams);
        //}
        public DataSet GetWorkflowAssigned(int _userid, int _status)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@userid", _userid);
            arParams[1] = new SqlParameter("@status", _status);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getResourceRequestsWorkflow", arParams);
        }
        public DataSet GetWorkflowAssigned(int _userid, int _mine, int _buddy, string _requestid)
        {
            _requestid = _requestid.ToUpper();
            if (_requestid.StartsWith("CVT") == false)
                _requestid = "CVT" + _requestid;
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@userid", _userid);
            arParams[1] = new SqlParameter("@mine", _mine);
            arParams[2] = new SqlParameter("@buddy", _buddy);
            arParams[3] = new SqlParameter("@requestid", _requestid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getResourceRequestsWorkflowAssignedAll", arParams);
        }
        public DataSet GetWorkflowAssignedManager(int _userid, int _PageNum, int _PageSize, string _requestid)
        {
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@userid", _userid);
            arParams[1] = new SqlParameter("@PageNum", _PageNum);
            arParams[2] = new SqlParameter("@PageSize", _PageSize);
            arParams[3] = new SqlParameter("@requestid", _requestid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getResourceRequestsWorkflowAssignedManager", arParams);
        }
        public DataSet GetWorkflowProjectAll(int _projectid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@projectid", _projectid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getResourceRequestsWorkflowProjectAll", arParams);
        }
        public void CloseWorkflow(int _id, int _environment, int _retrieve_service_id, string _se_dsn, bool _close_request, int intResourceRequestApprove, int intAssignPage, int intViewPage, string _dsn_asset, string _dsn_ip)
        {
            CloseWorkflow(_id, _environment, _retrieve_service_id, _se_dsn, _close_request, intResourceRequestApprove, intAssignPage, intViewPage, _dsn_asset, _dsn_ip, false);
        }
        public void CloseWorkflow(int _id, int _environment, int _retrieve_service_id, string _se_dsn, bool _close_request, int intResourceRequestApprove, int intAssignPage, int intViewPage, string _dsn_asset, string _dsn_ip, bool _returned)
        {
            int intWorkflowParent = GetWorkflowParent(_id);
            CloseWorkflowParent(intWorkflowParent, _environment, _retrieve_service_id, _se_dsn, _close_request, intResourceRequestApprove, intAssignPage, intViewPage, _dsn_asset, _dsn_ip, false);
        }
        public void CloseWorkflowParent(int intWorkflowParent, int _environment, int _retrieve_service_id, string _se_dsn, bool _close_request, int intResourceRequestApprove, int intAssignPage, int intViewPage, string _dsn_asset, string _dsn_ip)
        {
            CloseWorkflowParent(intWorkflowParent, _environment, _retrieve_service_id, _se_dsn, _close_request, intResourceRequestApprove, intAssignPage, intViewPage, _dsn_asset, _dsn_ip, false);
        }
        public void CloseWorkflowParent(int intWorkflowParent, int _environment, int _retrieve_service_id, string _se_dsn, bool _close_request, int intResourceRequestApprove, int intAssignPage, int intViewPage, string _dsn_asset, string _dsn_ip, bool _returned)
        {
            Log oLog = new Log(user, dsn);
            if (IsOpen(intWorkflowParent) == false)
            {
                Users oUser = new Users(user, dsn);
                Projects oProject = new Projects(user, dsn);
                Functions oFunction = new Functions(user, dsn, _environment);
                RequestItems oRequestItem = new RequestItems(user, dsn);
                RequestFields oRequestField = new RequestFields(user, dsn);
                Requests oRequest = new Requests(user, dsn);
                Variables oVariable = new Variables(_environment);
                string strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_REQUEST_STATUS");
                string strEmail = "";
                string strCC = "";
                int _requestid = Int32.Parse(Get(intWorkflowParent, "requestid"));
                int _serviceid = Int32.Parse(Get(intWorkflowParent, "serviceid"));
                int _number = Int32.Parse(Get(intWorkflowParent, "number"));
                string strCVT = "CVT" + _requestid.ToString() + "-" + _serviceid.ToString() + "-" + _number.ToString();
                string strCompleteText = "";
                string strLink = "";
                if (_retrieve_service_id > 0)
                {
                    strCompleteText = " The asset record has been updated and billing has been suspended.";
                    Customized oCustomized = new Customized(user, dsn);
                    int intItem = Int32.Parse(Get(intWorkflowParent, "itemid"));
                    int intNumber = Int32.Parse(Get(intWorkflowParent, "number"));
                    DataSet dsServer = oCustomized.GetServerArchive(_requestid, intItem, intNumber);
                    if (dsServer.Tables[0].Rows.Count > 0)
                    {
                        string strServerName = dsServer.Tables[0].Rows[0]["servername"].ToString();
                        strLink = "<p><a href=\"" + oVariable.URL() + "/redirect.aspx?referrer=" + oVariable.URL() + "/service.aspx?rid=" + _requestid.ToString() + "&sid=" + _retrieve_service_id + "&q=" + strServerName + "\" target=\"_blank\">To <b>RETRIEVE</b> server (" + strServerName + ") click on this link.</a></p>";
                    }
                }
                int intRequestUser = oRequest.GetUser(_requestid);
                if (intRequestUser > 0)
                    strEmail = oUser.GetName(intRequestUser);
                int intProject = oRequest.GetProjectNumber(_requestid);
                if (intProject > 0)
                {
                    int intProjectLead = Int32.Parse(oProject.Get(intProject, "lead"));
                    if (intProjectLead > 0)
                    {
                        if (strEmail != "")
                            strEmail += ";";
                        strEmail += oUser.GetName(intProjectLead);
                    }
                    int intProjectUser = Int32.Parse(oProject.Get(intProject, "userid"));
                    if (intProjectUser > 0)
                    {
                        if (strEmail != "")
                            strEmail += ";";
                        strEmail += oUser.GetName(intProjectUser);
                    }
                }
                DataSet dsResource = GetWorkflowsParent(intWorkflowParent);
                foreach (DataRow drResource in dsResource.Tables[0].Rows)
                {
                    int intUser = Int32.Parse(drResource["userid"].ToString());
                    if (intUser > 0)
                        strCC += oUser.GetName(intUser) + ";";
                }

                //Before starting workflow -check for returned request for which workflow already initiated.
                DataSet dsRRReturn = getResourceRequestReturn(intWorkflowParent, _serviceid, _number,1,0);
                int intNextRRId = 0;
                int intNextServiceId = 0;
                int intNextNumber = 0;
                if (dsRRReturn.Tables[0].Rows.Count > 0)
                {
                    intNextRRId = Int32.Parse(dsRRReturn.Tables[0].Rows[0]["NextRRId"].ToString());
                    intNextServiceId = Int32.Parse(dsRRReturn.Tables[0].Rows[0]["NextServiceId"].ToString());
                    intNextNumber = Int32.Parse(dsRRReturn.Tables[0].Rows[0]["NextNumber"].ToString()); ;
                }
                    
               

                // Start Workflow!!

                Services oService = new Services(0, dsn);
                bool boolWorkflow = false;

                if (intNextRRId == 0 && intNextServiceId == 0) // Not from returned request
                {
                    oLog.AddEvent(_requestid.ToString(), strCVT, "Not returned", LoggingType.Debug);
                    lstWorkflow = new List<int>();
                    boolWorkflow = GenerateWorkflows(_requestid, _serviceid, _number, _se_dsn, _dsn_asset, _dsn_ip, intResourceRequestApprove, _environment, intAssignPage, intViewPage, strEmail, strCC, strEMailIdsBCC, false);
                }
                if (_returned == false)
                {
                    if (boolWorkflow == false)
                    {
                        // NOTIFICATION
                        if (oService.Get(_serviceid, "show") == "1" && oService.Get(_serviceid, "notify_client") != "0" && (_requestid < 25543 || _requestid > 28576))
                            oFunction.SendEmail("Service Request Completed [" + strCVT + "]", strEmail, strCC, strEMailIdsBCC, "Service Request Completed [" + strCVT + "]", "<p><b>The following service request has been completed!" + strCompleteText + "</b></p><p>" + GetSummary(intWorkflowParent, 0, _environment, _se_dsn, _dsn_asset, _dsn_ip) + "</p>" + strLink, true, false);
                        if (_close_request == true)
                            oRequest.CloseOverall(_requestid, _environment);
                    }
                    else
                    {
                        if (oService.Get(_serviceid, "show") == "1" && oService.Get(_serviceid, "notify_client") != "0" && (_requestid < 25543 || _requestid > 28576))
                            oFunction.SendEmail("Workflow Task Completed [" + strCVT + "]", strEmail, strCC, strEMailIdsBCC, "Workflow Task Completed [" + strCVT + "]", "<p><b>The following workflow task has been completed!" + strCompleteText + "</b></p><p>One or more workflow tasks are still being processed so your request is not complete. You will receive separate notifications regarding each of these additional workflow tasks.</p><p>" + GetSummary(intWorkflowParent, 0, _environment, _se_dsn, _dsn_asset, _dsn_ip) + "</p>" + strLink, true, false);
                    }
                }
                else
                {
                    // Returned notifications happen in the Complete function of the WM task.
                }
            }
        }

        public bool GenerateWorkflows(int _requestid, int _serviceid, int _number, string _se_dsn, string _dsn_asset, string _dsn_ip, int intResourceRequestApprove, int _environment, int intAssignPage, int intViewPage, string strEmail, string strCC, string strEMailIdsBCC, bool _all_together)
        {
            bool boolWorkflowGenerated = false;
            Log oLog = new Log(user, dsn);
            Functions oFunction = new Functions(user, dsn, _environment);
            Requests oRequest = new Requests(0, dsn);
            Services oService = new Services(0, dsn);
            ServiceRequests oServiceRequest = new ServiceRequests(0, dsn);
            ServiceEditor oServiceEditor = new ServiceEditor(user, _se_dsn);
            string strCVT = "CVT" + _requestid.ToString() + "-" + _serviceid.ToString() + "-" + _number.ToString();
            string strService = oService.GetName(_serviceid);
            bool boolIndividual = true;
            List<int> lstPending = new List<int>();
            List<int> lstCollective = new List<int>();
            oLog.AddEvent(_requestid.ToString(), strCVT, "Service (" + strService + "): *** Starting Workflow ***", LoggingType.Debug);

            if (_all_together == false)
            {
                // First, check to see if the parent has more than 1 service and has restricted them from being generated until all are done.
                List<WorkflowServices> lstBefore = oService.PreviousService(_requestid, _serviceid, _number);
                if (lstBefore.Count > 0 && lstBefore[0].Children > 1 && lstBefore[0].SameTime == true)
                {
                    string strParent = oService.GetName(lstBefore[0].ServiceID);
                    oLog.AddEvent(_requestid.ToString(), strCVT, "Service (" + strService + "): Parent service (" + strParent + ") has more than one child service configured and is restricted from generating workflows until all children are completed.  Initiating them all individually to check...", LoggingType.Debug);
                    // Execute all workflows.
                    DataSet dsSameTime = oService.GetWorkflows(lstBefore[0].ServiceID);
                    foreach (DataRow drSameTime in dsSameTime.Tables[0].Rows)
                    {
                        if (GenerateWorkflows(_requestid, Int32.Parse(drSameTime["nextservice"].ToString()), _number, _se_dsn, _dsn_asset, _dsn_ip, intResourceRequestApprove, _environment, intAssignPage, intViewPage, strEmail, strCC, strEMailIdsBCC, true) == true)
                            boolWorkflowGenerated = true;
                    }
                    oLog.AddEvent(_requestid.ToString(), strCVT, "Service (" + strService + "): Parent service (" + strParent + ") workflows generated = " + boolWorkflowGenerated.ToString(), LoggingType.Debug);
                    boolIndividual = false;
                }
            }

            if (boolIndividual == true)
            {
                // OK to generate workflow for current service (all other services at same level are completed)
                if (_serviceid > 0 && oService.Get(_serviceid, "workflow_connect") == "1")
                {
                    DataSet dsWorkflow = oService.GetWorkflows(_serviceid);
                    oLog.AddEvent(_requestid.ToString(), strCVT, "Service (" + strService + "): permits workflows and has " + dsWorkflow.Tables[0].Rows.Count.ToString() + " configured.", LoggingType.Debug);
                    if (dsWorkflow.Tables[0].Rows.Count > 0)
                    {
                        oLog.AddEvent(_requestid.ToString(), strCVT, "Service (" + strService + "): Check that the service is OK to initiate new workflows (based on it's SAMETIME flag / completion setting)", LoggingType.Debug);
                        bool boolWorkflowProceed = true;
                        int intWorkflowContinue = 0;
                        DataSet dsReceive = oService.GetWorkflowsReceive(_serviceid);
                        oLog.AddEvent(_requestid.ToString(), strCVT, "Service (" + strService + "): Has " + dsReceive.Tables[0].Rows.Count.ToString() + " calling workflows", LoggingType.Debug);
                        foreach (DataRow drReceive in dsReceive.Tables[0].Rows)
                        {
                            int intCallingService = Int32.Parse(drReceive["serviceid"].ToString());
                            if (_serviceid == Int32.Parse(drReceive["nextservice"].ToString()))
                            {
                                // This workflow that is being executed.
                                if (drReceive["continue"].ToString() == "1")
                                    intWorkflowContinue = intCallingService;
                            }
                            string strCallingService = oService.GetName(intCallingService);

                            // Get all workflows of the calling service
                            DataSet dsWorkflowsCalling = oService.GetWorkflows(intCallingService);
                            // Get whether or not the service is set to individual completion (1 is automatically individual)
                            bool boolCompleteIndividual = (dsWorkflowsCalling.Tables[0].Rows.Count == 1 || oService.Get(intCallingService, "sametime") == "0");
                            if (boolCompleteIndividual == false)
                            {
                                oLog.AddEvent(_requestid.ToString(), strCVT, "PreviousService (" + strCallingService + "): Collective Completion (count > 1 AND sametime = true)...checking same level for open tasks", LoggingType.Debug);
                                // Collective completion means all other services at the same level must be completed for this service to be initiated.
                                bool boolOpen = false;
                                foreach (DataRow drWorkflowCalling in dsWorkflowsCalling.Tables[0].Rows)
                                {
                                    int intCallingServiceNext = Int32.Parse(drWorkflowCalling["nextservice"].ToString());
                                    if (intCallingServiceNext != _serviceid)   // skip the current service
                                    {
                                        // Check the completion of the request
                                        DataSet dsRR = GetRequestService(_requestid, intCallingServiceNext, _number);
                                        if (dsRR.Tables[0].Rows.Count > 0)
                                        {
                                            if (IsOpen(Int32.Parse(dsRR.Tables[0].Rows[0]["parent"].ToString())) == true)
                                            {
                                                oLog.AddEvent(_requestid.ToString(), strCVT, "Service (" + oService.GetName(intCallingServiceNext) + "): Open", LoggingType.Debug);
                                                boolOpen = true;
                                                break;
                                            }
                                            else
                                                oLog.AddEvent(_requestid.ToString(), strCVT, "Service (" + oService.GetName(intCallingServiceNext) + "): Closed", LoggingType.Debug);
                                        }
                                        else
                                        {
                                            // Was never generated (either because it doesn't meet criteria, or because it wasn't created at the time)
                                        }
                                    }
                                }
                                boolWorkflowProceed = (boolOpen == false);  // Only proceed if all other tasks are closed
                                oLog.AddEvent(_requestid.ToString(), strCVT, "PreviousService (" + strCallingService + "): Collective Completion tasks closed? = " + boolWorkflowProceed.ToString(), LoggingType.Debug);
                            }
                            else
                            {
                                oLog.AddEvent(_requestid.ToString(), strCVT, "PreviousService (" + strCallingService + "): Individual Completion", LoggingType.Debug);
                                // If individual completion, then we can initiate the next service.
                            }
                        }

                        if (boolWorkflowProceed == true)
                        {
                            oLog.AddEvent(_requestid.ToString(), strCVT, "Service (" + strService + "): Beginning to generate workflows for children", LoggingType.Debug);
                            int intDevices = 1;
                            if (oService.Get(_serviceid, "quantity_is_device") == "1")
                            {
                                DataSet dsTemp = oService.GetSelected(_requestid, _serviceid);
                                if (dsTemp.Tables[0].Rows.Count > 0)
                                    intDevices = Int32.Parse(dsTemp.Tables[0].Rows[0]["quantity"].ToString());
                            }
                            //DataSet dsRequestOldData = oServiceEditor.GetRequestData(_requestid, _serviceid, _number, -1);
                            DataSet dsRequestOldData = oServiceEditor.GetRequestData(_requestid, _serviceid, _number, -1, dsn);   // will be using to pass data into next service, so be sure to get ALL information that is available.
                            if (dsRequestOldData.Tables[0].Rows.Count == 0)
                            {
                                // This service was never generated (either due to conditions or it didn't exist at the time).
                                if (dsReceive.Tables[0].Rows.Count > 0 && intWorkflowContinue > 0)
                                {
                                    oLog.AddEvent(_requestid.ToString(), strCVT, "Service (" + strService + "): Missing workflow data - parent is set to collective completion with continue - generating workflow", LoggingType.Debug);
                                    AddRequestData(_requestid, intWorkflowContinue, _serviceid, _number, _se_dsn);
                                    //boolWorkflowGenerated = GenerateWorkflows(_requestid, _serviceid, _number, _se_dsn, _dsn_asset, _dsn_ip, intResourceRequestApprove, _environment, intAssignPage, intViewPage, strEmail, strCC, strEMailIdsBCC, _all_together);
                                    dsRequestOldData = oServiceEditor.GetRequestData(_requestid, _serviceid, _number, -1, dsn);   // will be using to pass data into next service, so be sure to get ALL information that is available.
                                }
                                else
                                {
                                    // Since this service is not part of a group, and does not allow continue, skip kicking off this service's workflows
                                }
                            }
                            if (dsRequestOldData.Tables[0].Rows.Count == 0)
                            {
                                // Skip kicking off this workflow.
                                oLog.AddEvent(_requestid.ToString(), strCVT, "Service (" + strService + "): Missing workflow data - must not exist...skipping child workflows", LoggingType.Debug);
                            }
                            else
                            {
                                foreach (DataRow drWorkflow in dsWorkflow.Tables[0].Rows)
                                {
                                    int intService = Int32.Parse(drWorkflow["nextservice"].ToString());
                                    string strNextService = oService.GetName(intService);
                                    oLog.AddEvent(_requestid.ToString(), strCVT, "NextService (" + strService + "): Trying to generate workflow for " + strNextService, LoggingType.Debug);
                                    // Check to see that conditions (if applicable) are met.
                                    if (WorkflowConditionsOK(_serviceid, intService, oServiceEditor, _requestid, _number, (drWorkflow["only"].ToString() == "1"), (drWorkflow["only"].ToString() == "0"), dsRequestOldData))
                                    {
                                        oLog.AddEvent(_requestid.ToString(), strCVT, "NextService (" + strNextService + "): Workflow Conditions OK", LoggingType.Debug);
                                        bool boolAlreadyGenerated = false;
                                        foreach (int intAlready in lstWorkflow)
                                        {
                                            if (intAlready == intService)
                                            {
                                                boolAlreadyGenerated = true;
                                                break;
                                            }
                                        }
                                        if (boolAlreadyGenerated == false)
                                        {
                                            oLog.AddEvent(_requestid.ToString(), strCVT, "NextService (" + strNextService + "): Not already generated.", LoggingType.Debug);
                                            lstWorkflow.Add(intService);
                                            boolWorkflowGenerated = true;
                                            // Move data to new workflow (if applicable)
                                            AddRequestData(_requestid, _serviceid, intService, _number, _se_dsn);
                                            oLog.AddEvent(_requestid.ToString(), strCVT, "NextService (" + strNextService + "): Data moved from previous workflow into current service.", LoggingType.Debug);

                                            // Create and send request
                                            int intItem = oService.GetItemId(intService);
                                            int intResource = oServiceRequest.AddRequest(_requestid, intItem, intService, intDevices, 0.00, 2, _number, _se_dsn);
                                            oLog.AddEvent(_requestid.ToString(), strCVT, "NextService (" + strNextService + "): Service Request generated.", LoggingType.Debug);
                                            if (oServiceRequest.NotifyApproval(intResource, intResourceRequestApprove, _environment, "", _se_dsn) == false)
                                            {
                                                oLog.AddEvent(_requestid.ToString(), strCVT, "NextService (" + strNextService + "): Service Request approved.", LoggingType.Debug);
                                                int intAssignTo = 0;
                                                int intPreviousService = 0;
                                                if (Int32.TryParse(oService.Get(intService, "workflow_userid"), out intPreviousService) == true)
                                                {
                                                    if (intPreviousService < 0)
                                                        intAssignTo = oRequest.GetUser(_requestid);
                                                    else if (intPreviousService > 0)
                                                    {
                                                        DataSet dsRR = GetRequestService(_requestid, intPreviousService, _number);
                                                        if (dsRR.Tables[0].Rows.Count > 0)
                                                            Int32.TryParse(dsRR.Tables[0].Rows[0]["userid"].ToString(), out intAssignTo);
                                                    }
                                                }
                                                oServiceRequest.NotifyTeamLead(intItem, intResource, intAssignPage, intViewPage, _environment, "", _se_dsn, _dsn_asset, _dsn_ip, intAssignTo);
                                                oLog.AddEvent(_requestid.ToString(), strCVT, "NextService (" + strNextService + "): Service Request NotifyTeamLead complete.", LoggingType.Debug);
                                            }
                                            else
                                                oLog.AddEvent(_requestid.ToString(), strCVT, "NextService (" + strNextService + "): Service Request sent for approval.", LoggingType.Debug);

                                            if (oService.Get(_serviceid, "show") == "1" && oService.Get(_serviceid, "notify_client") != "0" && (_requestid < 25543 || _requestid > 28576))
                                                oFunction.SendEmail("Workflow Request Initiated [CVT" + _requestid.ToString() + "-" + intService.ToString() + "-" + _number.ToString() + "]", strEmail, strCC, strEMailIdsBCC, "Workflow Request Initiated [CVT" + _requestid.ToString() + "-" + intService.ToString() + "-" + _number.ToString() + "]", "<p><b>The next service in the workflow (" + oService.GetName(intService) + ") has been initiated.</p><p>" + GetSummary(intResource, 0, _environment, _se_dsn, _dsn_asset, _dsn_ip) + "</p>", true, false);

                                            oLog.AddEvent(_requestid.ToString(), strCVT, "NextService (" + strNextService + "): Client (to:" + strEmail + " / cc:" + strCC + ") Notified.", LoggingType.Debug);
                                        }
                                        else
                                        {
                                            oLog.AddEvent(_requestid.ToString(), strCVT, "NextService (" + strNextService + "): Service request already generated.", LoggingType.Debug);
                                        }
                                    }
                                    else
                                    {
                                        oLog.AddEvent(_requestid.ToString(), strCVT, "NextService (" + strNextService + "): Workflow Conditions NOT OK", LoggingType.Debug);
                                        // If CONTINUE, then try to generate subsequent workflows.
                                        if (drWorkflow["continue"].ToString() == "1")
                                        {
                                            if (oService.Get(_serviceid, "sametime") == "0")    // Individual
                                            {
                                                oLog.AddEvent(_requestid.ToString(), strCVT, "NextService (" + strNextService + "): CONTINUE! Service (" + strService + ") configured for individual completion - adding to queue for continuation.", LoggingType.Debug);
                                                lstPending.Add(intService);
                                            }
                                            else
                                            {
                                                oLog.AddEvent(_requestid.ToString(), strCVT, "NextService (" + strNextService + "): WAIT! Service (" + strService + ") configured for collective completion, even though allowed to continue.", LoggingType.Debug);
                                                lstCollective.Add(intService);
                                            }
                                        }
                                    }
                                }
                            }

                            foreach (int intPending in lstPending)
                            {
                                oLog.AddEvent(_requestid.ToString(), strCVT, "Service " + oService.GetName(intPending) + " allows for continuing workflow and was not initiated.  Trying its services...", LoggingType.Debug);
                                // Add the data so subsequent workflows can reference it.
                                AddRequestData(_requestid, _serviceid, intPending, _number, _se_dsn);
                                boolWorkflowGenerated = GenerateWorkflows(_requestid, intPending, _number, _se_dsn, _dsn_asset, _dsn_ip, intResourceRequestApprove, _environment, intAssignPage, intViewPage, strEmail, strCC, strEMailIdsBCC, _all_together);
                            }

                            if (boolWorkflowGenerated == false) // No workflows were generated, so this would mean that all services at the level are either already complete, or did not meet requirements.  Move on to the collective ones.
                            {
                                foreach (int intCollective in lstCollective)
                                {
                                    oLog.AddEvent(_requestid.ToString(), strCVT, "Service " + oService.GetName(intCollective) + " is in queue for collective completion and no other workflows were generated.  Trying its services...", LoggingType.Debug);
                                    // Add the data so subsequent workflows can reference it.
                                    AddRequestData(_requestid, _serviceid, intCollective, _number, _se_dsn);
                                    boolWorkflowGenerated = GenerateWorkflows(_requestid, intCollective, _number, _se_dsn, _dsn_asset, _dsn_ip, intResourceRequestApprove, _environment, intAssignPage, intViewPage, strEmail, strCC, strEMailIdsBCC, _all_together);
                                }
                            }
                        }
                        else
                        {
                            oLog.AddEvent(_requestid.ToString(), strCVT, "Service (" + strService + "): Workflows have not been generated because one or more sibling services are still open", LoggingType.Debug);
                            boolWorkflowGenerated = true;   // Setting to true so that the correct email is sent to the client (workflow) instead of (service request)
                        }
                    }
                }
            }
            return boolWorkflowGenerated;
        }
        public void AddRequestData(int _requestid, int _serviceid_from, int _serviceid_next, int _number, string _se_dsn)
        {
            ServiceEditor oServiceEditor = new ServiceEditor(user, _se_dsn);
            //DataSet dsRequestOldData = oServiceEditor.GetRequestData(_requestid, _serviceid_from, _number, -1);
            DataSet dsRequestOldData = oServiceEditor.GetRequestData(_requestid, _serviceid_from, _number, 1, dsn);
            // Move data to new workflow (if applicable)
            string strTable = "set_GEN_" + _serviceid_next.ToString();
            DataSet dsRequestOld = oServiceEditor.GetConfigs(_serviceid_next, 1, 1);
            //DataSet dsRequestOldData = oServiceEditor.GetRequestData(_requestid, _serviceid, _number, -1);
            if (dsRequestOldData.Tables[0].Rows.Count > 0)
            {
                DataRow drRequestOldData = dsRequestOldData.Tables[0].Rows[0];
                int intPassParameters = 0;
                foreach (DataRow drRequestOld in dsRequestOld.Tables[0].Rows)
                {
                    if (drRequestOld["editable"].ToString() == "1" && Int32.Parse(drRequestOld["serviceid"].ToString()) == _serviceid_from)
                    {
                        // Count the # of parameters that needs to be passed onto the new request
                        intPassParameters++;
                    }
                }
                SqlParameter[] arParams = new SqlParameter[intPassParameters + 4];
                intPassParameters = 4;
                string strSQL1 = "";
                string strSQL2 = "";
                foreach (DataRow drRequestOld in dsRequestOld.Tables[0].Rows)
                {
                    if (drRequestOld["editable"].ToString() == "1" && Int32.Parse(drRequestOld["serviceid"].ToString()) == _serviceid_from)
                    {
                        // Configure the parameters
                        string strField = drRequestOld["dbfield"].ToString();
                        strSQL1 += " ,[" + drRequestOld["dbfield"].ToString() + "]";
                        strSQL2 += " ,@" + drRequestOld["dbfield"].ToString();
                        arParams[intPassParameters] = new SqlParameter("@" + strField, drRequestOldData[strField].ToString());
                        intPassParameters++;
                    }
                }
                oServiceEditor.DeleteRequestData(strTable, _requestid, _serviceid_next, _number);
                string strSQL = "INSERT INTO " + strTable + "([requestid], [serviceid], [number], [modified]" + strSQL1 + ") VALUES (@requestid, @serviceid, @number, @modified" + strSQL2 + ")";
                oServiceEditor.AddRequestData(strSQL, _requestid, _serviceid_next, _number, arParams, DateTime.Now);
                // Copy Data into Requests table
                oServiceEditor.AddRequest(_requestid, _serviceid_next, _number, drRequestOldData["title"].ToString(), Int32.Parse(drRequestOldData["priority"].ToString()), drRequestOldData["statement"].ToString(), DateTime.Parse(drRequestOldData["start_date"].ToString()), DateTime.Parse(drRequestOldData["end_date"].ToString()), Int32.Parse(drRequestOldData["expedite"].ToString()));
            }
        }

        public bool WorkflowConditionsOK(int _serviceid_from, int _serviceid_next, ServiceEditor oServiceEditor, int _requestid, int _number, bool _only, bool _unless, DataSet dsRequestOldData)
        {
            string strCVT = "CVT" + _requestid.ToString() + "-" + _serviceid_from.ToString() + "-" + _number.ToString();
            Log oLog = new Log(user, dsn);
            Services oService = new Services(user, dsn);
            string strService = oService.GetName(_serviceid_next);
            // Check to see that conditions (if applicable) are met.
            bool boolWorkflowProceed = true;
            // Check conditions for generating the next service.
            DataSet dsConditions = oServiceEditor.GetWorkflowConditions(_serviceid_from, _serviceid_next, 1);
            if (dsConditions.Tables[0].Rows.Count > 0)
            {
                oLog.AddEvent(_requestid.ToString(), strCVT, "Service (" + strService + "): Workflow conditional", LoggingType.Debug);
                boolWorkflowProceed = false;
                //bool boolConditionOnly = (drWorkflow["only"].ToString() == "1");    // Initiate ONLY when the following condition sets are selected
                //bool boolConditionUnless = (drWorkflow["only"].ToString() == "0");  // Always initiate UNLESS the following condition sets are selected

                if (_only == false && _unless == false)
                    boolWorkflowProceed = true;
                else
                {
                    if (_only)
                        oLog.AddEvent(_requestid.ToString(), strCVT, "Service (" + strService + "): Checking Workflow = ONLY", LoggingType.Debug);
                    else if (_unless)
                    {
                        oLog.AddEvent(_requestid.ToString(), strCVT, "Service (" + strService + "): Checking Workflow = UNLESS", LoggingType.Debug);
                        boolWorkflowProceed = true;  // Set to true, and if a condition set is ACCEPTED, it will set to false.
                    }
                    else
                        oLog.AddEvent(_requestid.ToString(), strCVT, "Service (" + strService + "): Checking Workflow = ???", LoggingType.Debug);

                    foreach (DataRow drCondition in dsConditions.Tables[0].Rows)
                    {
                        bool boolConditionValuesAllSelected = false;  // false by default so that if no values are selected, does not trigger the workflow
                        // Loop through all condition sets to see if any of them are selected
                        DataSet dsConditionValues = oServiceEditor.GetWorkflowConditionValues(Int32.Parse(drCondition["id"].ToString()), _requestid, _number);
                        oLog.AddEvent(_requestid.ToString(), strCVT, "Service (" + strService + "): Checking condition #  : " + drCondition["id"].ToString() + " - " + dsConditionValues.Tables[0].Rows.Count.ToString() + " conditions", LoggingType.Debug);
                        foreach (DataRow drConditionValue in dsConditionValues.Tables[0].Rows)
                        {
                            // Loop through each value in the condition set.
                            string strConditionField = drConditionValue["dbfield"].ToString();
                            string[] strDataValues = dsRequestOldData.Tables[0].Rows[0][strConditionField].ToString().Split(new char[] { ',' });
                            string strConditionValue = drConditionValue["value"].ToString();
                            bool boolConditionSelected = false;
                            foreach (string strDataValue in strDataValues)
                            {
                                if (strDataValue != "")
                                {
                                    oLog.AddEvent(_requestid.ToString(), strCVT, "Service (" + strService + "): Question : " + drConditionValue["question"].ToString() + "; Condition : " + strConditionValue + "; Actual Response : " + strDataValue, LoggingType.Debug);
                                    if (strConditionValue == strDataValue)
                                    {
                                        boolConditionSelected = true;
                                        break;
                                    }
                                }
                            }

                            // At this point, we know whether or not the value is selected.
                            if (boolConditionSelected)
                            {
                                // Value selected
                                oLog.AddEvent(_requestid.ToString(), strCVT, "Service (" + strService + "): Workflow Condition Value Equal : " + drConditionValue["question"].ToString() + " = " + strConditionValue + "...continue checking values...", LoggingType.Debug);
                                boolConditionValuesAllSelected = true;
                            }
                            else
                            {
                                // Value not selected
                                boolConditionValuesAllSelected = false;
                                // Since this type of condition depends on all values of the set being selected, we can exit here.
                                oLog.AddEvent(_requestid.ToString(), strCVT, "Service (" + strService + "): Workflow Condition Value NOT Equal : " + drConditionValue["question"].ToString() + " <> " + strConditionValue + "...condition FAILED...exiting condition.", LoggingType.Debug);
                                break;
                            }
                        }

                        // At this point, we have checked all of the values of the condition against the selected values.
                        if (_only)  // Only initiate when the values are selected and none are skipped.
                        {
                            if (boolConditionValuesAllSelected)
                            {
                                // All values were selected making this condition ACCEPTED
                                oLog.AddEvent(_requestid.ToString(), strCVT, "Service (" + strService + "): Workflow ONLY Condition...all values were selected...ok to proceed", LoggingType.Debug);
                                boolWorkflowProceed = true;
                                break;
                            }
                            else
                                oLog.AddEvent(_requestid.ToString(), strCVT, "Service (" + strService + "): Workflow ONLY Condition..one or more values were not selected...continue checking other condition sets...", LoggingType.Debug);
                        }
                        else if (_unless)
                        {
                            if (boolConditionValuesAllSelected)
                            {
                                oLog.AddEvent(_requestid.ToString(), strCVT, "Service (" + strService + "): Workflow UNLESS Condition...all values were selected...ok to skip", LoggingType.Debug);
                                boolWorkflowProceed = false;
                                break;
                            }
                            else
                            {
                                oLog.AddEvent(_requestid.ToString(), strCVT, "Service (" + strService + "): Workflow UNLESS Condition...one or more values not selected...continue checking other condition sets...", LoggingType.Debug);
                            }
                        }
                    }
                }
            }
            return boolWorkflowProceed;
        }

        public void CloseWorkflowAndEMail(int _id, int _environment, int _retrieve_service_id, string _se_dsn, bool _close_request, int intResourceRequestApprove, int intAssignPage, int intViewPage, string _dsn_asset, string _dsn_ip)
        {
            int intWorkflowParent = GetWorkflowParent(_id);
            if (IsOpen(intWorkflowParent) == false)
            {
                Users oUser = new Users(user, dsn);
                Projects oProject = new Projects(user, dsn);
                Functions oFunction = new Functions(user, dsn, _environment);
                RequestItems oRequestItem = new RequestItems(user, dsn);
                RequestFields oRequestField = new RequestFields(user, dsn);
                Requests oRequest = new Requests(user, dsn);
                Log oLog = new Log(user, dsn);
                Variables oVariable = new Variables(_environment);
                string strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_REQUEST_STATUS");
                string strEmail = "";
                string strCC = "";
                int _requestid = Int32.Parse(Get(intWorkflowParent, "requestid"));
                int _serviceid = Int32.Parse(Get(intWorkflowParent, "serviceid"));
                int _number = Int32.Parse(Get(intWorkflowParent, "number"));
                string strCVT = "CVT" + _requestid.ToString() + "-" + _serviceid.ToString() + "-" + _number.ToString();
                oLog.AddEvent(_requestid.ToString(), strCVT, "Closing Workflow...", LoggingType.Debug);
                string strCompleteText = "";
                string strLink = "";
                if (_retrieve_service_id > 0)
                {
                    strCompleteText = " The asset record has been updated and billing has been suspended.";
                    Customized oCustomized = new Customized(user, dsn);
                    int intItem = Int32.Parse(Get(intWorkflowParent, "itemid"));
                    int intNumber = Int32.Parse(Get(intWorkflowParent, "number"));
                    DataSet dsServer = oCustomized.GetServerArchive(_requestid, intItem, intNumber);
                    if (dsServer.Tables[0].Rows.Count > 0)
                    {
                        string strServerName = dsServer.Tables[0].Rows[0]["servername"].ToString();
                        strLink = "<p><a href=\"" + oVariable.URL() + "/redirect.aspx?referrer=" + oVariable.URL() + "/service.aspx?rid=" + _requestid.ToString() + "&sid=" + _retrieve_service_id + "&q=" + strServerName + "\" target=\"_blank\">To <b>RETRIEVE</b> server (" + strServerName + ") click on this link.</a></p>";
                    }
                }
                int intRequestUser = oRequest.GetUser(_requestid);
                if (intRequestUser > 0)
                    strEmail = oUser.GetName(intRequestUser);
                int intProject = oRequest.GetProjectNumber(_requestid);
                if (intProject > 0)
                {
                    int intProjectLead = Int32.Parse(oProject.Get(intProject, "lead"));
                    if (intProjectLead > 0)
                    {
                        if (strEmail != "")
                            strEmail += ";";
                        strEmail += oUser.GetName(intProjectLead);
                    }
                    int intProjectUser = Int32.Parse(oProject.Get(intProject, "userid"));
                    if (intProjectUser > 0)
                    {
                        if (strEmail != "")
                            strEmail += ";";
                        strEmail += oUser.GetName(intProjectUser);
                    }
                }
                DataSet dsResource = GetWorkflowsParent(intWorkflowParent);
                foreach (DataRow drResource in dsResource.Tables[0].Rows)
                {
                    int intUser = Int32.Parse(drResource["userid"].ToString());
                    if (intUser > 0)
                        strCC += oUser.GetName(intUser) + ";";
                }
                // Start Workflow!!
                Services oService = new Services(0, dsn);
                bool boolWorkflow = false;
                if (_serviceid > 0 && oService.Get(_serviceid, "workflow_connect") == "1")
                {
                    ServiceRequests oServiceRequest = new ServiceRequests(0, dsn);
                    DataSet dsWorkflow = oService.GetWorkflows(_serviceid);
                    if (dsWorkflow.Tables[0].Rows.Count > 0)
                    {
                        boolWorkflow = true;
                        int intDevices = 1;
                        if (oService.Get(_serviceid, "quantity_is_device") == "1")
                        {
                            DataSet dsTemp = oService.GetSelected(_requestid, _serviceid);
                            if (dsTemp.Tables[0].Rows.Count > 0)
                                intDevices = Int32.Parse(dsTemp.Tables[0].Rows[0]["quantity"].ToString());
                        }
                        foreach (DataRow drWorkflow in dsWorkflow.Tables[0].Rows)
                        {
                            int intService = Int32.Parse(drWorkflow["nextservice"].ToString());
                            strCVT = "CVT" + _requestid.ToString() + "-" + intService.ToString() + "-" + _number.ToString();
                            int intItem = oService.GetItemId(intService);
                            int intResource = oServiceRequest.AddRequest(_requestid, intItem, intService, intDevices, 0.00, 2, _number, _se_dsn);
                            oLog.AddEvent(_requestid.ToString(), strCVT, "Service request has been generated by workflow", LoggingType.Debug);
                            if (oServiceRequest.NotifyApproval(intResource, intResourceRequestApprove, _environment, "", _se_dsn) == false)
                            {
                                oLog.AddEvent(_requestid.ToString(), strCVT, "Service request has been fully approved", LoggingType.Debug);
                                oServiceRequest.NotifyTeamLead(intItem, intResource, intAssignPage, intViewPage, _environment, "", _se_dsn, _dsn_asset, _dsn_ip, 0);
                                oLog.AddEvent(_requestid.ToString(), strCVT, "Service request NotifyTeamLead has completed.", LoggingType.Debug);
                            }
                            else
                                oLog.AddEvent(_requestid.ToString(), strCVT, "Service request has been sent for approval", LoggingType.Debug);
                            if (oService.Get(_serviceid, "show") == "1" && oService.Get(_serviceid, "notify_client") != "0" && (_requestid < 25543 || _requestid > 28576))
                                oFunction.SendEmail("Workflow Completed [CVT" + _requestid.ToString() + "]", strEmail, strCC, strEMailIdsBCC, "Workflow Completed [#CVT" + _requestid.ToString() + "]", "<p><b>The following service request workflow has been completed!" + strCompleteText + "</b></p><p>The next service in the workflow (" + oService.GetName(intService) + ") has been initiated.</p><p>" + GetWorkflowSummary(_id, _environment, _se_dsn, _dsn_asset, _dsn_ip) + "</p>" + strLink, true, false);
                        }
                    }
                }
                if (boolWorkflow == false)
                {
                    oLog.AddEvent(_requestid.ToString(), strCVT, "Request completed", LoggingType.Debug);
                    // NOTIFICATION
                    oFunction.SendEmail("Request Completed [CVT" + _requestid.ToString() + "]", strEmail, strCC, strEMailIdsBCC, "Request Completed [#CVT" + _requestid.ToString() + "]", "<p><b>The following service request has been completed!" + strCompleteText + "</b></p><p>" + GetWorkflowSummary(_id, _environment, _se_dsn, _dsn_asset, _dsn_ip) + "</p>" + strLink, true, false);
                    if (_close_request == true)
                        oRequest.CloseOverall(_requestid, _environment);
                }
            }
        }

        public void UpdateWorkflowUsed(int _id, double _used)
        {
            Update(GetWorkflowParent(_id), "");
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@used", _used);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateResourceRequestWorkflowUsed", arParams);
        }
        public DataSet GetWorkflow(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getResourceRequestWorkflow", arParams);
        }
        public string GetWorkflow(int _id, string _column)
        {
            DataSet ds = GetWorkflow(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public int GetWorkflowParent(int _id)
        {
            string strParent = GetWorkflow(_id, "parent");
            if (strParent != "")
                return Int32.Parse(strParent);
            else
                return 0;
        }
        public int AddWorkflow(int _parent, int _workflowid, string _name, int _userid, int _devices, double _allocated, int _status, int _joined)
        {
            if (_devices == 0)
                _devices = 1;
            arParams = new SqlParameter[9];
            arParams[0] = new SqlParameter("@parent", _parent);
            arParams[1] = new SqlParameter("@workflowid", _workflowid);
            arParams[2] = new SqlParameter("@name", _name);
            arParams[3] = new SqlParameter("@userid", _userid);
            arParams[4] = new SqlParameter("@devices", _devices);
            arParams[5] = new SqlParameter("@allocated", _allocated);
            arParams[6] = new SqlParameter("@status", _status);
            arParams[7] = new SqlParameter("@joined", _joined);
            arParams[8] = new SqlParameter("@id", SqlDbType.Int);
            arParams[8].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addResourceRequestWorkflow", arParams);
            return Int32.Parse(arParams[8].Value.ToString());
        }
        public void UpdateWorkflow(int _id, int _devices, double _allocated, int _status)
        {
            Update(GetWorkflowParent(_id), "");
            if (_devices == 0)
                _devices = 1;
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@devices", _devices);
            arParams[2] = new SqlParameter("@allocated", _allocated);
            arParams[3] = new SqlParameter("@status", _status);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateResourceRequestWorkflow", arParams);
        }
        public void UpdateWorkflowName(int _id, string _name, int _modifiedby)
        {
            Update(GetWorkflowParent(_id), "");
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@name", _name);
            arParams[2] = new SqlParameter("@modifiedby", _modifiedby);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateResourceRequestWorkflowName", arParams);
        }
        public void UpdateWorkflowAssigned(int _id, int _userid)
        {
            Update(GetWorkflowParent(_id), "");
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@userid", _userid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateResourceRequestWorkflowAssigned", arParams);
        }
        public void UpdateWorkflowStatus(int _id, int _status, bool _update_joined)
        {
            int intParent = GetWorkflowParent(_id);
            Update(intParent, "");
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@status", _status);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateResourceRequestWorkflowStatus", arParams);
            // Update Joined Requests
            DataSet dsParent = GetWorkflowsParent(intParent);
            if (_update_joined == true && GetWorkflow(_id, "joined") == "1")
            {
                foreach (DataRow drParent in dsParent.Tables[0].Rows)
                    UpdateWorkflowStatus(Int32.Parse(drParent["id"].ToString()), _status, false);
                UpdateStatusOverall(intParent, _status);
            }
            else if (GetWorkflow(_id, "joined") != "1")
            {
                bool bool_2 = true;
                bool bool2 = true;
                bool bool3 = true;
                bool bool5 = true;
                bool bool7 = true;
                bool bool10 = true;
                foreach (DataRow drParent in dsParent.Tables[0].Rows)
                {
                    if (drParent["status"].ToString() != "-2")
                        bool_2 = false;
                    if (drParent["status"].ToString() != "2" && drParent["status"].ToString() != "1")
                        bool2 = false;
                    if (drParent["status"].ToString() != "3")
                        bool3 = false;
                    if (drParent["status"].ToString() != "5")
                        bool5 = false;
                    if (drParent["status"].ToString() != "7")
                        bool7 = false;
                    if (drParent["status"].ToString() != "10")
                        bool10 = false;
                }
                if (bool_2 == true)
                    UpdateStatusOverall(intParent, -2);
                else if (bool2 == true)
                    UpdateStatusOverall(intParent, 2);
                else if (bool3 == true)
                    UpdateStatusOverall(intParent, 3);
                else if (bool5 == true)
                    UpdateStatusOverall(intParent, 5);
                else if (bool7 == true)
                    UpdateStatusOverall(intParent, 7);
                else if (bool10 == true)
                    UpdateStatusOverall(intParent, 10);
            }
            if (_status == (int)ResourceRequestStatus.Closed)
                UpdateWorkflowCompleted(_id, DateTime.Now.ToString());
            else
                UpdateWorkflowCompleted(_id, "");
        }
        public DataSet GetWorkflowsParent(int _parent)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@parent", _parent);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getResourceRequestWorkflowsParent", arParams);
        }

        public int AddWorkflowAssign(int _parent, int _userid, int _devices, double _allocated)
        {
            if (_devices == 0)
                _devices = 1;
            arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@parent", _parent);
            arParams[1] = new SqlParameter("@userid", _userid);
            arParams[2] = new SqlParameter("@devices", _devices);
            arParams[3] = new SqlParameter("@allocated", _allocated);
            arParams[4] = new SqlParameter("@id", SqlDbType.Int);
            arParams[4].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addResourceRequestWorkflowAssign", arParams);
            return Int32.Parse(arParams[4].Value.ToString());
        }
        public DataSet GetWorkflowsAssign(int _parent)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@parent", _parent);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getResourceRequestWorkflowsAssign", arParams);
        }
        public void DeleteWorkflowAssign(int _id)
        {
            arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteResourceRequestWorkflowAssign", arParams);
        }
        public void DeleteWorkflowAssigns(int _parent)
        {
            arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@parent", _parent);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteResourceRequestWorkflowAssigns", arParams);
        }
        #endregion

        #region HOURS
        public double GetWorkflowUsed(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@parent", _id);
            object o = SqlHelper.ExecuteScalar(dsn, CommandType.StoredProcedure, "pr_getResourceRequestWorkflowHours", arParams);
            if (o == null || o.ToString() == "")
                return 0.00;
            else
            {
                double dblUsed = double.Parse(o.ToString());
                double dblAllocated = double.Parse(GetWorkflow(_id, "allocated"));
                if (dblUsed > dblAllocated) // Should never have more hours used than there are allocated.
                {
                    UpdateWorkflowHoursOverwrite(_id, dblAllocated);
                    dblUsed = dblAllocated;
                }
                return dblUsed;
            }
        }
        public void UpdateWorkflowHours(int _id)
        {
            Update(GetWorkflowParent(_id), "");
            double dblAllocated = double.Parse(GetWorkflow(_id, "allocated"));
            UpdateWorkflowHours(_id, dblAllocated);
        }
        public void UpdateWorkflowHours(int _id, double _used)
        {
            int intParent = GetWorkflowParent(_id);
            Update(intParent, "");

            bool boolJoined = (GetWorkflow(_id, "joined") == "1");
            DataSet dsWF = GetWorkflowsParent(intParent);
            if (boolJoined)
            {
                // Add hours for all of the workflow items (since joined together)
                foreach (DataRow drWF in dsWF.Tables[0].Rows)
                    AddWorkflowHours(Int32.Parse(drWF["id"].ToString()), _used);
            }
            else
                AddWorkflowHours(_id, _used);

            double dblAllocated = double.Parse(GetWorkflow(_id, "allocated"));
            double dblUsed = GetWorkflowUsed(_id);
            if (boolJoined)
            {
                // Update used hours for all of the workflow items (since joined together)
                foreach (DataRow drWF in dsWF.Tables[0].Rows)
                {
                    if (dblUsed > dblAllocated)
                        UpdateWorkflowUsed(Int32.Parse(drWF["id"].ToString()), dblAllocated);
                    else
                        UpdateWorkflowUsed(Int32.Parse(drWF["id"].ToString()), dblUsed);
                }
            }
            else
            {
                if (dblUsed > dblAllocated)
                    UpdateWorkflowUsed(_id, dblAllocated);
                else
                    UpdateWorkflowUsed(_id, dblUsed);
            }
        }
        private void AddWorkflowHours(int _parent, double _used)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@parent", _parent);
            arParams[1] = new SqlParameter("@used", _used);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addResourceRequestWorkflowHours", arParams);
        }
        public void UpdateWorkflowHoursOverwrite(int _id, double _used)
        {
            int intParent = GetWorkflowParent(_id);
            Update(intParent, "");

            bool boolJoined = (GetWorkflow(_id, "joined") == "1");
            DataSet dsWF = GetWorkflowsParent(intParent);
            if (boolJoined)
            {
                // Add hours for all of the workflow items (since joined together)
                foreach (DataRow drWF in dsWF.Tables[0].Rows)
                {
                    AddWorkflowHoursOverwrite(Int32.Parse(drWF["id"].ToString()), _used);
                    UpdateWorkflowUsed(_id, _used);
                }
            }
            else
            {
                AddWorkflowHoursOverwrite(_id, _used);
                UpdateWorkflowUsed(_id, _used);
            }
        }
        private void AddWorkflowHoursOverwrite(int _parent, double _used)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@parent", _parent);
            arParams[1] = new SqlParameter("@used", _used);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addResourceRequestWorkflowHoursOverwrite", arParams);
        }
        public void DeleteWorkflowHours(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@parent", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteResourceRequestWorkflowHours", arParams);
        }
        #endregion




































        public DataSet Get(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getResourceRequest", arParams);
        }
        public string Get(int _id, string _column)
        {
            DataSet ds = Get(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public DataSet Get(int _requestid, int _itemid, int _number)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@itemid", _itemid);
            arParams[2] = new SqlParameter("@number", _number);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getResourceRequestItem", arParams);
        }
        public DataSet GetAllItem(int _requestid, int _itemid, int _number)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@itemid", _itemid);
            arParams[2] = new SqlParameter("@number", _number);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getResourceRequestsAllItem", arParams);
        }
        public DataSet GetAllService(int _requestid, int _serviceid, int _number)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@serviceid", _serviceid);
            arParams[2] = new SqlParameter("@number", _number);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getResourceRequestsAllService", arParams);
        }

        public DataSet GetResourceRequest(int _requestid, int _itemid, int _number)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@itemid", _itemid);
            arParams[2] = new SqlParameter("@number", _number);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getResourceRequestForRequestItemNumber", arParams);
        }

        
        //public string Get(int _requestid, int _itemid, int _number, string _column)
        //{
        //    DataSet ds = Get(_requestid, _itemid, _number);
        //    if (ds.Tables[0].Rows.Count > 0)
        //        return ds.Tables[0].Rows[0][_column].ToString();
        //    else
        //        return "";
        //}

        public DataSet GetProjectItem(int _projectid, int _itemid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@projectid", _projectid);
            arParams[1] = new SqlParameter("@itemid", _itemid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getResourceRequestsProjectItem", arParams);
        }
        public DataSet GetItem(int _itemid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@itemid", _itemid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getResourceRequestsItem", arParams);
        }
        public DataSet GetAwaiting(int _userid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@userid", _userid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getResourceRequestsAwaiting", arParams);
        }
        public DataSet GetAwaitingBuddy(int _userid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@userid", _userid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getResourceRequestsAwaitingBuddy", arParams);
        }
        //public DataSet GetAwaiting()
        //{
        //    return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getResourceRequestsAwaitingAll");
        //}
        public DataSet GetApproval(int _userid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@userid", _userid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getResourceRequestsApproval", arParams);
        }
        public int Add(int _requestid, int _itemid, int _serviceid, int _number, string _name, int _devices, double _allocated, int _status, int _solo, int _accepted, int _platform_approval)
		{
            if (_devices == 0)
                _devices = 1;
            arParams = new SqlParameter[12];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@itemid", _itemid);
            arParams[2] = new SqlParameter("@serviceid", _serviceid);
            arParams[3] = new SqlParameter("@number", _number);
            arParams[4] = new SqlParameter("@name", _name);
            arParams[5] = new SqlParameter("@devices", _devices);
            arParams[6] = new SqlParameter("@allocated", _allocated);
            arParams[7] = new SqlParameter("@status", _status);
            arParams[8] = new SqlParameter("@solo", _solo);
            arParams[9] = new SqlParameter("@accepted", _accepted);
            arParams[10] = new SqlParameter("@platform_approval", _platform_approval);
            arParams[11] = new SqlParameter("@id", SqlDbType.Int);
            arParams[11].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addResourceRequest", arParams);
            return Int32.Parse(arParams[11].Value.ToString());
		}


        public DataSet GetApproval(int _requestid, int _itemid, int _number)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@itemid", _itemid);
            arParams[2] = new SqlParameter("@number", _number);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getResourceRequestApproval", arParams);
        }
        //public void AddPRC(int _parent, int _scope, int _schedule, int _cost, string _path)
        //{
        //    arParams = new SqlParameter[5];
        //    arParams[0] = new SqlParameter("@parent", _parent);
        //    arParams[1] = new SqlParameter("@scope", _scope);
        //    arParams[2] = new SqlParameter("@schedule", _schedule);
        //    arParams[3] = new SqlParameter("@cost", _cost);
        //    arParams[4] = new SqlParameter("@path", _path);
        //    SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addResourceRequestPRC", arParams);
        //}

        #region STATUS
        public void AddStatus(int _parent, int _status, string _comments, int _userid)
        {
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@parent", _parent);
            arParams[1] = new SqlParameter("@status", _status);
            arParams[2] = new SqlParameter("@comments", _comments);
            arParams[3] = new SqlParameter("@userid", _userid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addResourceRequestUpdate", arParams);
        }
        public DataSet GetStatus(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getResourceRequestUpdate", arParams);
        }
        public DataSet GetStatusLatest(int _parent)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@parent", _parent);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getResourceRequestUpdateLatest", arParams);
        }
        public DataSet GetStatuss(int _parent)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@parent", _parent);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getResourceRequestUpdates", arParams);
        }
        public void DeleteStatus(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteResourceRequestUpdate", arParams);
        }
        public DataSet GetStatuss()
        {
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getResourceRequestUpdatesReminder");
        }
        public void AddStatusTPM(int _parent, int _scope, int _timeline, int _budget, DateTime _datestamp, string _comments, string _thisweek, string _nextweek)
        {
            arParams = new SqlParameter[8];
            arParams[0] = new SqlParameter("@parent", _parent);
            arParams[1] = new SqlParameter("@scope", _scope);
            arParams[2] = new SqlParameter("@timeline", _timeline);
            arParams[3] = new SqlParameter("@budget", _budget);
            arParams[4] = new SqlParameter("@datestamp", _datestamp);
            arParams[5] = new SqlParameter("@comments", _comments);
            arParams[6] = new SqlParameter("@thisweek", _thisweek);
            arParams[7] = new SqlParameter("@nextweek", _nextweek);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addResourceRequestUpdateTPM", arParams);
        }
        public void UpdateStatusTPM(int _id, string _comments, string _thisweek, string _nextweek)
        {
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@comments", _comments);
            arParams[2] = new SqlParameter("@thisweek", _thisweek);
            arParams[3] = new SqlParameter("@nextweek", _nextweek);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateResourceRequestUpdateTPM", arParams);
        }
        public DataSet GetStatusTPM(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getResourceRequestUpdateTPM", arParams);
        }
        public DataSet GetStatussTPM(int _parent)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@parent", _parent);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getResourceRequestUpdatesTPM", arParams);
        }
        public void DeleteStatusTPM(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteResourceRequestUpdateTPM", arParams);
        }
        public string GetStatus(double _scope, double _timeline, double _budget, int _width, int _height)
        {
            if (_scope > 0.00 && _timeline > 0.00 && _budget > 0.00)
            {
                string strRed = "FF";
                string strGreen = "FF";
                if (_scope == 1.00 || _timeline == 1.00 || _budget == 1.00)
                    strGreen = "00";
                else if (_scope == 2.00 || _timeline == 2.00 || _budget == 2.00)
                    strGreen = "FF";
                else
                    strRed = "00";
                return "<div style=\"width:" + _width.ToString() + ";height:" + _height.ToString() + ";background-color:#" + strRed + strGreen + "00" + ";border:solid 1px #999999\"><img src=\"/images/spacer.gif\" border=\"0\" width=\"" + _width.ToString() + "\" height=\"" + _height.ToString() + "\"></div>";
            }
            else
                return "<div style=\"width:" + _width.ToString() + ";height:" + _height.ToString() + ";background-color:#00FF00;border:solid 1px #999999\"><img src=\"/images/spacer.gif\" border=\"0\" width=\"" + _width.ToString() + "\" height=\"" + _height.ToString() + "\"></div>";
        }
        public string GetStatus(double _status, int _width, int _height)
        {
            if (_status > 0.00)
            {
                string strRed = "FF";
                string strGreen = "FF";
                if (_status > 2.00)
                {
                    // Decrease Red
                    double dblPercent = _status - 2.00;
                    dblPercent = dblPercent * 255;
                    dblPercent = Math.Ceiling(255 - dblPercent);
                    int intPercent = Int32.Parse(dblPercent.ToString());
                    strRed = intPercent.ToString("X");
                }
                else
                {
                    // Increase Green
                    double dblPercent = _status - 1.00;
                    dblPercent = Math.Ceiling(dblPercent * 255);
                    int intPercent = Int32.Parse(dblPercent.ToString());
                    strGreen = intPercent.ToString("X");
                }
                if (strRed.Length == 1)
                    strRed = "0" + strRed;
                if (strGreen.Length == 1)
                    strGreen = "0" + strGreen;
                return "<div style=\"width:" + _width.ToString() + ";height:" + _height.ToString() + ";background-color:#" + strRed + strGreen + "00" + ";border:solid 1px #999999\"><img src=\"/images/spacer.gif\" border=\"0\" width=\"" + _width.ToString() + "\" height=\"" + _height.ToString() + "\"></div>";
            }
            else
                return "<div style=\"width:" + _width.ToString() + ";height:" + _height.ToString() + ";background-color:#00FF00;border:solid 1px #999999\"><img src=\"/images/spacer.gif\" border=\"0\" width=\"" + _width.ToString() + "\" height=\"" + _height.ToString() + "\"></div>";
        }
        #endregion

        #region CHANGE CONTROL
        public void AddChangeControl(int _parent, string _number, DateTime _implementation, string _comments)
        {
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@parent", _parent);
            arParams[1] = new SqlParameter("@number", _number);
            arParams[2] = new SqlParameter("@implementation", _implementation);
            arParams[3] = new SqlParameter("@comments", _comments);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addResourceRequestChangeControl", arParams);
        }
        public void UpdateChangeControl(int _id, string _number, DateTime _implementation, string _comments)
        {
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@number", _number);
            arParams[2] = new SqlParameter("@implementation", _implementation);
            arParams[3] = new SqlParameter("@comments", _comments);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateResourceRequestChangeControl", arParams);
        }
        public DataSet GetChangeControl(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getResourceRequestChangeControl", arParams);
        }
        public DataSet GetChangeControls(int _parent)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@parent", _parent);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getResourceRequestChangeControls", arParams);
        }
        public DataSet GetChangeControls()
        {
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getResourceRequestChangeControlsUpcoming");
        }
        public DataSet GetChangeControlsUser(int _userid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@userid", _userid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getResourceRequestChangeControlsUser", arParams);
        }
        public DataSet GetChangeControlsDate(DateTime _date)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@date", _date);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getResourceRequestChangeControlsDate", arParams);
        }
        public void DeleteChangeControl(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteResourceRequestChangeControl", arParams);
        }
        #endregion

        #region DETAILS
        public void AddDetails(int _requestid, int _itemid, int _number, int _resourceid, int _detailid, int _done)
        {
            arParams = new SqlParameter[6];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@itemid", _itemid);
            arParams[2] = new SqlParameter("@number", _number);
            arParams[3] = new SqlParameter("@resourceid", _resourceid);
            arParams[4] = new SqlParameter("@detailid", _detailid);
            arParams[5] = new SqlParameter("@done", _done);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addResourceRequestDetails", arParams);
        }
        public DataSet GetDetails(int _requestid, int _itemid, int _number, int _resourceid)
        {
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@itemid", _itemid);
            arParams[2] = new SqlParameter("@number", _number);
            arParams[3] = new SqlParameter("@resourceid", _resourceid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getResourceRequestDetails", arParams);
        }
        public DataSet GetDetailsHours(int _requestid, int _itemid, int _number, int _resourceid)
        {
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@itemid", _itemid);
            arParams[2] = new SqlParameter("@number", _number);
            arParams[3] = new SqlParameter("@resourceid", _resourceid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getResourceRequestDetailsHours", arParams);
        }
        public double GetDetailsHoursUsed(int _requestid, int _itemid, int _number, int _resourceid, bool _used)
        {
            double dblHours = 0.00;
            DataSet ds = GetDetailsHours(_requestid, _itemid, _number, _resourceid);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                double dblQuantity = double.Parse(dr["devices"].ToString());
                double dblTemp = double.Parse(dr["hours"].ToString());
                double dblAdditional = double.Parse(dr["additional"].ToString());
                dblQuantity = dblQuantity - 1.00;
                if (dblQuantity > 0.00)
                {
                    if (dblAdditional > -1.00)
                        dblAdditional = dblAdditional * dblQuantity;
                    else
                        dblAdditional = dblTemp * dblQuantity;
                }
                else
                    dblAdditional = 0.00;
                if (_used == false || dr["done"].ToString() == "1")
                    dblHours = dblHours + dblTemp + dblAdditional;
            }
            return dblHours;
        }
        public DataSet GetDetail(int _requestid, int _itemid, int _number, int _resourceid, int _detailid)
        {
            arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@itemid", _itemid);
            arParams[2] = new SqlParameter("@number", _number);
            arParams[3] = new SqlParameter("@resourceid", _resourceid);
            arParams[4] = new SqlParameter("@detailid", _detailid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getResourceRequestDetail", arParams);
        }
        public int GetDetailValue(int _requestid, int _itemid, int _number, int _resourceid, int _detailid)
        {
            DataSet dsDetails = GetDetail(_requestid, _itemid, _number, _resourceid, _detailid);
            if (dsDetails.Tables[0].Rows.Count == 0)
                return -1;
            else
                return Int32.Parse(dsDetails.Tables[0].Rows[0]["done"].ToString());
        }
        public void DeleteDetails(int _requestid, int _itemid, int _number, int _resourceid)
        {
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@itemid", _itemid);
            arParams[2] = new SqlParameter("@number", _number);
            arParams[3] = new SqlParameter("@resourceid", _resourceid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteResourceRequestDetails", arParams);
        }
        public void DeleteDetails(int _requestid, int _itemid, int _number)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@itemid", _itemid);
            arParams[2] = new SqlParameter("@number", _number);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteResourceRequestDetailsAll", arParams);
        }
        #endregion

        #region MILESTONES
        public void AddMilestone(int _parent, DateTime _approved, DateTime _forecasted, int _complete, string _milestone, string _description)
        {
            arParams = new SqlParameter[6];
            arParams[0] = new SqlParameter("@parent", _parent);
            arParams[1] = new SqlParameter("@approved", _approved);
            arParams[2] = new SqlParameter("@forecasted", _forecasted);
            arParams[3] = new SqlParameter("@complete", _complete);
            arParams[4] = new SqlParameter("@milestone", _milestone);
            arParams[5] = new SqlParameter("@description", _description);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addResourceRequestMilestone", arParams);
        }
        public void UpdateMilestone(int _id, DateTime _approved, DateTime _forecasted, int _complete, string _milestone, string _description)
        {
            arParams = new SqlParameter[6];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@approved", _approved);
            arParams[2] = new SqlParameter("@forecasted", _forecasted);
            arParams[3] = new SqlParameter("@complete", _complete);
            arParams[4] = new SqlParameter("@milestone", _milestone);
            arParams[5] = new SqlParameter("@description", _description);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateResourceRequestMilestone", arParams);
        }
        public DataSet GetMilestone(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getResourceRequestMilestone", arParams);
        }
        public DataSet GetMilestones(int _parent)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@parent", _parent);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getResourceRequestMilestones", arParams);
        }
        public void DeleteMilestone(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteResourceRequestMilestone", arParams);
        }
        #endregion

        #region SHARING
        public void AddSharing(int _sharedid, int _userid, int _rights, string _expiration)
        {
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@sharedid", _sharedid);
            arParams[1] = new SqlParameter("@userid", _userid);
            arParams[2] = new SqlParameter("@rights", _rights);
            arParams[3] = new SqlParameter("@expiration", (_expiration == "" ? SqlDateTime.Null : DateTime.Parse(_expiration)));
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addResourceRequestSharing", arParams);
        }
        public void UpdateSharing(int _id, int _userid, int _rights, string _expiration)
        {
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@userid", _userid);
            arParams[2] = new SqlParameter("@rights", _rights);
            arParams[3] = new SqlParameter("@expiration", (_expiration == "" ? SqlDateTime.Null : DateTime.Parse(_expiration)));
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateResourceRequestSharing", arParams);
        }
        public void DeleteSharing(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteResourceRequestSharing", arParams);
        }
        public int GetSharing(int _sharedid, int _userid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@sharedid", _sharedid);
            arParams[1] = new SqlParameter("@userid", _userid);
            DataSet ds = SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getResourceRequestSharing", arParams);
            int intReturn = 0;
            if (ds.Tables[0].Rows.Count > 0)
            {
                bool boolExpired = false;
                if (ds.Tables[0].Rows[0]["expiration"].ToString() != "")
                {
                    DateTime _expiration = DateTime.Parse(ds.Tables[0].Rows[0]["expiration"].ToString());
                    if (DateTime.Now >= _expiration)
                        boolExpired = true;
                }
                if (boolExpired == false)
                    intReturn = Int32.Parse(ds.Tables[0].Rows[0]["rights"].ToString());
                else
                    intReturn = -1;
            }
            return intReturn;
        }
        #endregion

        public bool CanUpdate(int intProfile, int intResourceWorkflow, bool _tpm)
        {
            Services oService = new Services(0, dsn);
            RequestItems oRequestItem = new RequestItems(0, dsn);
            Applications oApplication = new Applications(0, dsn);
            Delegates oDelegate = new Delegates(0, dsn);
            Users oUser = new Users(0, dsn);
            int intAssigned = Int32.Parse(GetWorkflow(intResourceWorkflow, "userid"));
            int intResourceParent = GetWorkflowParent(intResourceWorkflow);
            int intService = Int32.Parse(Get(intResourceParent, "serviceid"));
            int intItem = Int32.Parse(Get(intResourceParent, "itemid"));
            int intApp = oRequestItem.GetItemApplication(intItem);
            if (_tpm == true)
            {
                if (oUser.IsAdmin(intProfile) || intProfile == intAssigned || GetSharing(intResourceWorkflow, intAssigned) > 0 || oService.IsManager(intService, intProfile) || oDelegate.Get(intAssigned, intProfile) > 0 || (oApplication.IsManager(intApp, intProfile) && oApplication.Get(intApp, "disable_manager") != "1") || (intAssigned > 0 && oUser.IsManager(intAssigned, intProfile, true) && oApplication.Get(intApp, "disable_manager") != "1") || (oApplication.Get(intApp, "tpm") == "1" && oApplication.IsManager(intApp, oUser.GetManager(intProfile, true))))
                    return true;
                else
                    return false;
            }
            else
            {
                if (oUser.IsAdmin(intProfile) || intProfile == intAssigned || GetSharing(intResourceWorkflow, intAssigned) > 0 || oService.IsManager(intService, intProfile) || oDelegate.Get(intAssigned, intProfile) > 0 || (oApplication.IsManager(intApp, intProfile) && oApplication.Get(intApp, "disable_manager") != "1") || (intAssigned > 0 && oUser.IsManager(intAssigned, intProfile, true) && oApplication.Get(intApp, "disable_manager") != "1"))
                    return true;
                else
                    return false;
            }
        }


        #region SLANotification

        public DataSet GetSLANotificationServiceRequests()
        {
            
                arParams = new SqlParameter[2];
                return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_GetServiceRequestSLANonCompliance");
            
            
        }


        public void UpdateSLANotificationDateForServiceRequest(int _id, int _requestid, int _itemid, int _number, int _serviceid)
        {
            arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@ResourceRequestid", _id);
            arParams[1] = new SqlParameter("@RequestId", _requestid);
            arParams[2] = new SqlParameter("@RequestItemId", _itemid);
            arParams[3] = new SqlParameter("@RequestNumber", _number);
            arParams[4] = new SqlParameter("@RequestServiceid", _serviceid);

            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_UpdateResourceRequestSLANotificationDate", arParams);

            
        }

        #endregion

        #region "Resource Request Returns"
        public void addResourceRequestReturn(int _requestid, int _rrid, int _serviceid, int _number,int _rrwfid,int _intReturnToWF,
                                            int _nextrrid, int _nextserviceid, int _nextnumber, int _nextrrwfid, 
                                             int _returnedbyuserid, string _comments)
        {
            arParams = new SqlParameter[12];
            arParams[0] = new SqlParameter("@RequestID", _requestid);
            arParams[1] = new SqlParameter("@RRID", _rrid);
            arParams[2] = new SqlParameter("@ServiceId", _serviceid);
            arParams[3] = new SqlParameter("@Number", _number);
            arParams[4] = new SqlParameter("@RRWFId", _rrwfid);
            arParams[5] = new SqlParameter("ReturningToPreServiceWF", _intReturnToWF);
            arParams[6] = new SqlParameter("@NextRRID", _nextrrid);
            arParams[7] = new SqlParameter("@NextServiceId", _nextserviceid);
            arParams[8] = new SqlParameter("@NextNumber", _nextnumber);
            arParams[9] = new SqlParameter("@NextRRWFId", _nextrrwfid);
            arParams[10] = new SqlParameter("@ReturnedByUser", _returnedbyuserid);
            arParams[11] = new SqlParameter("@Comments", _comments);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addResourceRequestReturn", arParams);


        }

        public DataSet getResourceRequestReturn(int _rrid, int _serviceid, int _number, int _intReturnToWF, int _completed)
        {
            arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@RRID", _rrid);
            arParams[1] = new SqlParameter("@ServiceId", _serviceid);
            arParams[2] = new SqlParameter("@Number", _number);
            arParams[3] = new SqlParameter("@ReturningToPreServiceWF", _intReturnToWF);
            arParams[4] = new SqlParameter("@Completed", _completed);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getResourceRequestReturn", arParams);
        }

       
        public void updateResourceRequestReturnCompleted(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@Id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateResourceRequestReturnCompleted", arParams);
        }
        #endregion


        public void AddApproval(int _requestid, int _serviceid, int _number, int _userid)
        {
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@serviceid", _serviceid);
            arParams[2] = new SqlParameter("@number", _number);
            arParams[3] = new SqlParameter("@userid", _userid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addResourceRequestApproval", arParams);
        }
        public void UpdateApproval(int _requestid, int _serviceid, int _number, int _userid, string _comments, string _approved, string _denied)
        {
            arParams = new SqlParameter[7];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@serviceid", _serviceid);
            arParams[2] = new SqlParameter("@number", _number);
            arParams[3] = new SqlParameter("@userid", _userid);
            arParams[4] = new SqlParameter("@comments", _comments);
            arParams[5] = new SqlParameter("@approved", (_approved == "" ? SqlDateTime.Null : DateTime.Parse(_approved)));
            arParams[6] = new SqlParameter("@denied", (_denied == "" ? SqlDateTime.Null : DateTime.Parse(_denied)));
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateResourceRequestApproval", arParams);
        }
        public void DeleteApproval(int _requestid, int _serviceid, int _number)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@serviceid", _serviceid);
            arParams[2] = new SqlParameter("@number", _number);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteResourceRequestApproval", arParams);
        }
        public DataSet GetApprovals(int _requestid, int _serviceid, int _number)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@serviceid", _serviceid);
            arParams[2] = new SqlParameter("@number", _number);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getResourceRequestApprovals", arParams);
        }
        public DataSet GetWorkflow(int _requestid, int _serviceid, int _number)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@serviceid", _serviceid);
            arParams[2] = new SqlParameter("@number", _number);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getResourceRequestWorkflows", arParams);
        }


        public List<WorkflowStatus> GetStatus(int? _workflowid, int? _resourceid, int? _requestid, int? _serviceid, int? _itemid, int? _number, bool _progress, string _dsn_service_editor)
        {
            arParams = new SqlParameter[6];
            arParams[0] = new SqlParameter("@workflowid", _workflowid);
            arParams[1] = new SqlParameter("@resourceid", _resourceid);
            arParams[2] = new SqlParameter("@requestid", _requestid);
            arParams[3] = new SqlParameter("@serviceid", _serviceid);
            arParams[4] = new SqlParameter("@itemid", _itemid);
            arParams[5] = new SqlParameter("@number", _number);
            DataSet ds = SqlHelper.ExecuteDataset(_dsn_service_editor, CommandType.StoredProcedure, "sep_getRequestStatus", arParams);

            List<WorkflowStatus> _returns = new List<WorkflowStatus>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                WorkflowStatus _return = new WorkflowStatus();
                Int32.TryParse(dr["requestid"].ToString(), out _return.requestid);
                _return.service = dr["service"].ToString();
                Int32.TryParse(dr["resourceid"].ToString(), out _return.resourceid);
                Int32.TryParse(dr["itemid"].ToString(), out _return.itemid);
                Int32.TryParse(dr["serviceid"].ToString(), out _return.serviceid);
                Int32.TryParse(dr["number"].ToString(), out _return.number);
                Int32.TryParse(dr["workflowid"].ToString(), out _return.workflowid);
                if (_progress == true)
                {
                    ServiceRequests oServiceRequest = new ServiceRequests(0, dsn);
                    double dblProgress = GetStatusPercent(_return.requestid, _return.itemid, _return.serviceid, _return.number);
                    _return.status = oServiceRequest.GetStatusBar(dblProgress, "100", "6", true);
                }
                else
                    _return.status = dr["status"].ToString();
                _return.users = dr["users"].ToString().Split(new char[] { ',' });
                _return.comments = dr["comments"].ToString();
                _returns.Add(_return);
            }
            return _returns;
        }
        protected double GetStatusPercent(int _requestid, int _itemid, int _serviceid, int _number)
        {
            RequestItems oRequestItem = new RequestItems(0, dsn);
            Services oService = new Services(0, dsn);
            double dblAllocated = 0.00;
            double dblUsed = 0.00;
            int intAssigned = 0;
            bool boolAutomated = (oService.Get(_serviceid, "automate") == "1");
            if (boolAutomated == false)
            {
                DataSet dsReqForm = oRequestItem.GetForm(_requestid, _serviceid, _itemid, _number);
                if (dsReqForm.Tables[0].Rows.Count > 0)
                    boolAutomated = (dsReqForm.Tables[0].Rows[0]["automated"].ToString() == "1" ? true : false);
            }
            if (boolAutomated == true)
                return 100.00;
            else
            {
                DataSet dsResource = GetRequestService(_requestid, _serviceid, _number);
                if (dsResource.Tables[0].Rows.Count > 0)
                {
                    int intResourceWorkflow = Int32.Parse(dsResource.Tables[0].Rows[0]["id"].ToString());
                    int intStatus = Int32.Parse(dsResource.Tables[0].Rows[0]["status"].ToString());
                    if (intStatus == 3)
                        return 100.00;
                    else
                    {
                        foreach (DataRow drResource in dsResource.Tables[0].Rows)
                        {
                            intResourceWorkflow = Int32.Parse(drResource["id"].ToString());
                            dblAllocated += double.Parse(drResource["allocated"].ToString());
                            double dblHours = GetWorkflowUsed(intResourceWorkflow);
                            dblUsed += dblHours;
                            intAssigned += Int32.Parse(drResource["userid"].ToString());
                        }
                    }
                }
                if (intAssigned == 0)
                    return -1.00;
                else
                {
                    double dblProgress = 0.00;
                    if (dblAllocated > 0.00)
                        dblProgress = dblUsed / dblAllocated * 100;
                    return dblProgress;
                }
            }
        }
        
    }
}
