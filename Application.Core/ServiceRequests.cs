using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using System.Data.SqlTypes;
using System.Text;

namespace NCC.ClearView.Application.Core
{
	public class ServiceRequests
	{
		private string dsn = "";
		private int user = 0;
		private SqlParameter[] arParams;
        public ServiceRequests(int _user, string _dsn)
		{
			user = _user;
			dsn = _dsn;
		}
        public void Add(int _requestid, int _manager_approval, int _checkout)
        {
            DataSet ds = Get(_requestid);
            if (ds.Tables[0].Rows.Count == 0)
            {
                arParams = new SqlParameter[3];
                arParams[0] = new SqlParameter("@requestid", _requestid);
                arParams[1] = new SqlParameter("@manager_approval", _manager_approval);
                arParams[2] = new SqlParameter("@checkout", _checkout);
                SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addServiceRequest", arParams);
            }
        }
        public void Update(int _requestid, int _checkout)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@checkout", _checkout);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateServiceRequest", arParams);
        }
        public void UpdateApproval(int _requestid, int _manager_approval)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@manager_approval", _manager_approval);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateServiceRequestApproval", arParams);
        }
        public void Update(int _requestid, string _name)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@name", _name);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateServiceRequestName", arParams);
        }
        public void DeleteAll(int _requestid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteServiceRequestAll", arParams);
        }
        public void Delete(int _requestid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteServiceRequest", arParams);
        }
        public string GetName(int _requestid)
        {
            string strName = "Unavailable";
            try { strName = Get(_requestid).Tables[0].Rows[0]["name"].ToString(); }
            catch { }
            return strName;
        }
        public DataSet GetTasks(int _userid, int _serviceid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@userid", _userid);
            arParams[1] = new SqlParameter("@serviceid", _serviceid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServiceRequestsTasksService", arParams);
        }
        public DataSet GetReturned(int _userid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@userid", _userid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServiceRequestsReturned", arParams);
        }
        public DataSet GetMine(int _userid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@userid", _userid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServiceRequestsMine", arParams);
        }
        public DataSet Get(int _requestid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServiceRequest", arParams);
        }
        public DataSet GetProject(int _projectid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@projectid", _projectid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServiceRequestProject", arParams);
        }
        public DataSet GetStatus(int _requestid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServiceRequestStatus", arParams);
        }
        public string Get(int _requestid, string _column)
        {
            DataSet ds = Get(_requestid);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public int AddRequest(int _requestid, int _itemid, int _serviceid, int _devices, double _hours, int _status, int _number, string _se_dsn)
        {
            RequestItems oRequestItem = new RequestItems(user, dsn);
            Services oService = new Services(user, dsn);
            ServiceEditor oServiceEditor = new ServiceEditor(user, _se_dsn);
            Applications oApplication = new Applications(user, dsn);
            ResourceRequest oResourceRequest = new ResourceRequest(user, dsn);
            ServiceDetails oServiceDetail = new ServiceDetails(user, dsn);
            if (_hours == 0.00 && _serviceid > 0)
            {
                double _quantity = 1.00;
                if (oService.Get(_serviceid, "quantity_is_device") == "1")
                {
                    DataSet dsSelected = oService.GetSelected(_requestid);
                    if (dsSelected.Tables[0].Rows.Count > 0)
                        _quantity = double.Parse(dsSelected.Tables[0].Rows[0]["quantity"].ToString());
                }
                _hours = oServiceDetail.GetHours(_serviceid, _quantity);
            }
            int _application = oRequestItem.GetItemApplication(_itemid);
            int intPlatform = Int32.Parse(oApplication.Get(_application, "platform_approve"));
            int intAccepted = (oService.Get(_serviceid, "rejection") == "1" ? 0 : 1);
            int intResource = oResourceRequest.Add(_requestid, _itemid, _serviceid, _number, "", _devices, _hours, _status, 1, intAccepted, (intPlatform == 1 ? 0 : 1));
            DataSet dsServiceEditor = oServiceEditor.GetRequestFirstData2(_requestid, _serviceid, _number, 0, dsn);
            if (dsServiceEditor.Tables[0].Rows.Count > 0)
                oResourceRequest.UpdateName(intResource, dsServiceEditor.Tables[0].Rows[0]["title"].ToString());
            // See if any approver has been configured.  If so, add them.
            // Get approval fields.
            DataSet dsApprovals = oServiceEditor.GetApprovals(_serviceid);
            if (dsApprovals.Tables[0].Rows.Count > 0)
            {
                // If exist, send approval...
                oResourceRequest.DeleteApproval(_requestid, _serviceid, _number);
            }
            return intResource;
        }
        public bool NotifyApproval(int _resourcerequestid, int _resourcerequestapprove, int _environment,  string _cc, string _dsn_service_editor)
        {
            bool boolNotify = false;
            ResourceRequest oResourceRequest = new ResourceRequest(user, dsn);
            DataSet dsResource = oResourceRequest.Get(_resourcerequestid);
            if (dsResource.Tables[0].Rows.Count > 0)
            {
                DataRow drResource = dsResource.Tables[0].Rows[0];
                int _requestid = Int32.Parse(drResource["requestid"].ToString());
                int intService = Int32.Parse(drResource["serviceid"].ToString());
                int intNumber = Int32.Parse(drResource["number"].ToString());
                return NotifyApproval(_requestid, intService, intNumber, _resourcerequestapprove, _environment, _cc, _dsn_service_editor);
            }

            //if (intApproval)
            return boolNotify;
        }


        public bool NotifyApproval(int _requestid, int intService, int intNumber, int _resourcerequestapprove, int _environment, string _cc, string _dsn_service_editor)
        {
            bool boolNotify = false;
            RequestItems oRequestItem = new RequestItems(user, dsn);
            ResourceRequest oResourceRequest = new ResourceRequest(user, dsn);
            Applications oApplication = new Applications(user, dsn);
            Users oUser = new Users(user, dsn);
            Functions oFunction = new Functions(user, dsn, _environment);
            Requests oRequest = new Requests(user, dsn);
            Pages oPage = new Pages(user, dsn);
            Variables oVariable = new Variables(_environment);
            Platforms oPlatform = new Platforms(user, dsn);
            Services oService = new Services(user, dsn);
            Log oLog = new Log(user, dsn);
            string strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_REQUEST_STATUS");

            int intPlatform = 0;
            int intManager = 0;
            int intApp = 0;
            int intItem = oService.GetItemId(intService);
            int RRID = 0;

            DataSet dsResource = oResourceRequest.GetAllService(_requestid, intService, intNumber);
            if (dsResource.Tables[0].Rows.Count > 0)
                Int32.TryParse(dsResource.Tables[0].Rows[0]["RRID"].ToString(), out RRID);

            string strCVT = "CVT" + _requestid.ToString() + "-" + intService.ToString() + "-" + intNumber.ToString();
            if (intItem > 0)
            {
                intApp = oRequestItem.GetItemApplication(intItem);
                intPlatform = Int32.Parse(oApplication.Get(intApp, "platform_approve"));
            }
            if (intService > 0)
                Int32.TryParse(oService.Get(intService, "manager_approval"), out intManager);

            DataSet dsSelected = oService.GetSelected(_requestid, intService, intNumber);
            int intSelectedID = 0;
            int intApproved = 0;
            if (dsSelected.Tables[0].Rows.Count > 0)
            {
                Int32.TryParse(dsSelected.Tables[0].Rows[0]["approved"].ToString(), out intApproved);
                Int32.TryParse(dsSelected.Tables[0].Rows[0]["id"].ToString(), out intSelectedID);
            }
            else
                intApproved = 1;    // since not the first one submitted, automatically approve from a service owner perspective.

            // First, check for the requestor's manager Approval
            if (intManager == 1 && Get(_requestid, "manager_approval") == "0")
            {
                // Send to Manager for approval
                intPlatform = 0;
                boolNotify = true;
                int intUser = oUser.GetManager(oRequest.GetUser(_requestid), true);
                oLog.AddEvent(_requestid.ToString(), strCVT, "Sending to manager for approval - " + oUser.GetFullNameWithLanID(intUser), LoggingType.Debug);
                if (intUser == 0)
                    intPlatform = 1;
                else
                {
                    string strDefault = oUser.GetApplicationUrl(intUser, _resourcerequestapprove);
                    if (strDefault == "")
                        oFunction.SendEmail("Request #CVT" + _requestid.ToString() + " APPROVAL", oUser.GetName(intUser), _cc, strEMailIdsBCC, "Request #CVT" + _requestid.ToString() + " APPROVAL", "<p><b>A service request requires your approval; you are required to approve or deny this request.</b></p>", true, false);
                    else
                        oFunction.SendEmail("Request #CVT" + _requestid.ToString() + " APPROVAL", oUser.GetName(intUser), _cc, strEMailIdsBCC, "Request #CVT" + _requestid.ToString() + " APPROVAL", "<p><b>A service request requires your approval; you are required to approve or deny this request.</b></p><p><a href=\"" + oVariable.URL() + "/redirect.aspx?referrer=/" + strDefault + oPage.GetFullLink(_resourcerequestapprove) + "?rid=" + _requestid.ToString() + "&approve=true\" target=\"_blank\">Click here to review this request.</a></p>", true, false);
                }
            }
            else
            {
                // Next, check the service approval
                DataSet dsApprovers = oService.GetUser(intService, -10);
                //int intApproval = ((oService.Get(intService, "approval") == "1" && dsApprovers.Tables[0].Rows.Count > 0) ? 0 : 1);
                int intApproval = ((oService.Get(intService, "approval") == "1" && dsApprovers.Tables[0].Rows.Count > 0) ? 1 : 0);
                if (intApproval == 1 && intApproved == 0)
                {
                    // Send to Approvers for approval
                    boolNotify = true;
                    // Send to Approvers for approval
                    foreach (DataRow drApprover in dsApprovers.Tables[0].Rows)
                    {
                        int intUser = Int32.Parse(drApprover["userid"].ToString());
                        oLog.AddEvent(_requestid.ToString(), strCVT, "Sending to service approver for approval - " + oUser.GetFullNameWithLanID(intUser), LoggingType.Debug);
                        string strDefault = oUser.GetApplicationUrl(intUser, _resourcerequestapprove);
                        if (strDefault == "")
                            oFunction.SendEmail("Service Request APPROVAL", oUser.GetName(intUser), _cc, strEMailIdsBCC, "Service Request APPROVAL", "<p><b>A service request requires your approval; you are required to approve or deny this request.</b></p>", true, false);
                        else
                            oFunction.SendEmail("Service Request APPROVAL", oUser.GetName(intUser), _cc, strEMailIdsBCC, "Service Request APPROVAL", "<p><b>A service request requires your approval; you are required to approve or deny this request.</b></p><p><a href=\"" + oVariable.URL() + "/redirect.aspx?referrer=/" + strDefault + oPage.GetFullLink(_resourcerequestapprove) + "?srid=" + intSelectedID.ToString() + "\" target=\"_blank\">Click here to review this request.</a></p>", true, false);
                    }
                }
                else
                {
                    // Update that the service is approved.
                    if (intSelectedID > 0 && intApproved != 1)
                        oService.UpdateSelectedApprove(_requestid, intService, intNumber, 1, -999, DateTime.Now, "System Approval");

                    // Commenting since no current way of knowing if request has been approved by the platform
                    //if (intPlatform == 1)
                    //{
                    //    // Send to Platform for approval
                    //    if (intItem > 0)
                    //    {
                    //        int intUser = oPlatform.GetManager(Int32.Parse(oRequestItem.GetItem(intItem, "platformid")));
                    //        if (intUser > 0)
                    //        {
                    //            string strDefault = oUser.GetApplicationUrl(intUser, _resourcerequestapprove);
                    //            if (strDefault == "")
                    //                oFunction.SendEmail("Request APPROVAL", oUser.GetName(intUser), "", strEMailIdsBCC, "Request APPROVAL", "<p><b>A service request requires your approval; you are required to approve or deny this request.</b></p>", true, false);
                    //            else
                    //                oFunction.SendEmail("Request APPROVAL", oUser.GetName(intUser), "", strEMailIdsBCC, "Request APPROVAL", "<p><b>A service request requires your approval; you are required to approve or deny this request.</b></p><p><a href=\"" + oVariable.URL() + "/redirect.aspx?referrer=/" + strDefault + oPage.GetFullLink(_resourcerequestapprove) + "?rrid=" + _resourcerequestid.ToString() + "\" target=\"_blank\">Click here to review this request.</a></p>", true, false);
                    //        }
                    //    }
                    //}

                    // Notify 3rd part approvers
                    DataSet dsAlready = oResourceRequest.GetApprovals(_requestid, intService, intNumber);
                    ServiceEditor oServiceEditor = new ServiceEditor(user, _dsn_service_editor);
                    DataSet dsApprovalFields = oServiceEditor.GetApprovals(intService);
                    DataSet dsServiceEditor = oServiceEditor.GetRequestFirstData2(_requestid, intService, intNumber, 0, dsn);

                    foreach (DataRow drApprovalField in dsApprovalFields.Tables[0].Rows)
                    {
                        if (dsServiceEditor.Tables[0].Rows.Count > 0)
                        {
                            int intApprover = 0;
                            if (Int32.TryParse(dsServiceEditor.Tables[0].Rows[0][drApprovalField["dbfield"].ToString()].ToString(), out intApprover) == true && intApprover > 0)
                            {
                                // Check to see if already sent
                                bool boolAlready = false;
                                foreach (DataRow drAlready in dsAlready.Tables[0].Rows)
                                {
                                    if (intApprover == Int32.Parse(drAlready["userid"].ToString()))
                                    {
                                        boolAlready = true;
                                        break;
                                    }
                                }
                                if (boolAlready == false)
                                {
                                    boolNotify = true;
                                    oResourceRequest.AddApproval(_requestid, intService, intNumber, intApprover);
                                    oLog.AddEvent(_requestid.ToString(), strCVT, "Sending to 3rd party approver for approval - " + oUser.GetFullNameWithLanID(intApprover), LoggingType.Debug);
                                    string strDefault = oUser.GetApplicationUrl(intApprover, _resourcerequestapprove);
                                    if (strDefault == "")
                                        oFunction.SendEmail("Request #CVT" + _requestid.ToString() + "-" + intService.ToString() + "-" + intNumber.ToString() + " APPROVAL", oUser.GetName(intApprover), "", strEMailIdsBCC, "Request #CVT" + _requestid.ToString() + "-" + intService.ToString() + "-" + intNumber.ToString() + " APPROVAL", "<p><b>A service request requires your approval; you are required to approve or deny this request.</b></p>", true, false);
                                    else
                                        oFunction.SendEmail("Request #CVT" + _requestid.ToString() + "-" + intService.ToString() + "-" + intNumber.ToString() + " APPROVAL", oUser.GetName(intApprover), "", strEMailIdsBCC, "Request #CVT" + _requestid.ToString() + "-" + intService.ToString() + "-" + intNumber.ToString() + " APPROVAL", "<p><b>A service request requires your approval; you are required to approve or deny this request.</b></p><p><a href=\"" + oVariable.URL() + "/redirect.aspx?referrer=/" + strDefault + oPage.GetFullLink(_resourcerequestapprove) + "?rrid=" + RRID.ToString() + "\" target=\"_blank\">Click here to review this request.</a></p>", true, false);
                                }
                                else
                                {
                                    // No new people to notify, check to see all approvals are finished.
                                    foreach (DataRow drAlready in dsAlready.Tables[0].Rows)
                                    {
                                        if (drAlready["approved"].ToString() == "" && drAlready["denied"].ToString() == "")
                                        {
                                            boolNotify = true;
                                            break;
                                        }
                                    }
                                    oLog.AddEvent(_requestid.ToString(), strCVT, "Notify = " + boolNotify.ToString(), LoggingType.Debug);
                                }
                            }
                        }
                    }
                }
            }

            //if (intApproval)
            return boolNotify;
        }

        public void NotifyTeamLead(int _itemid, int _rrid, int _assignpage, int _viewpage, int _environment,  string _cc, string _se_dsn, string _dsn_asset, string _dsn_ip, int _userid_assigned)
        {
            RequestItems oRequestItem = new RequestItems(user, dsn);
            Users oUser = new Users(user, dsn);
            Applications oApplication = new Applications(user, dsn);
            Pages oPage = new Pages(user, dsn);
            Functions oFunction = new Functions(user, dsn, _environment);
            Variables oVariable = new Variables(_environment);
            Services oService = new Services(user, dsn);
            ResourceRequest oResourceRequest = new ResourceRequest(user, dsn);
            Log oLog = new Log(user, dsn);
            int intService = Int32.Parse(oResourceRequest.Get(_rrid, "serviceid"));
            string strService = oService.GetName(intService);
            if (intService == 0)
                strService = oRequestItem.GetItemName(_itemid);
            int intApp = oRequestItem.GetItemApplication(_itemid);
            int intRequest = Int32.Parse(oResourceRequest.Get(_rrid, "requestid"));
            int intNumber = Int32.Parse(oResourceRequest.Get(_rrid, "number"));
            Requests oRequest = new Requests(user, dsn);
            int intProject = oRequest.GetProjectNumber(intRequest);
            Projects oProject = new Projects(user, dsn);
            int intRequester = oRequest.GetUser(intRequest);
            string strSpacerRow = "<tr><td colspan=\"3\"><img src=\"" + oVariable.ImageURL() + "/images/spacer.gif\" border=\"0\" width=\"1\" height=\"7\" /></td></tr>";
            string strEmail = oService.Get(intService, "email");
            string strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_REQUEST_ASSIGNMENT");
            string strCVT = "CVT" + intRequest.ToString() + "-" + intService.ToString() + "-" + intNumber.ToString();

            int intUser = _userid_assigned;
            bool boolTech = false;
            if (intUser > 0)
            {
                boolTech = true;
                string strNotify = "";
                //if (oProject.Get(intProject, "number").StartsWith("CV") == false)
                //    strNotify = "<p><span style=\"color:#0000FF\"><b>PROJECT COORDINATOR:</b> Please allocate the hours listed above for each resource in Clarity.</span></p>";
                // Add Workflow
                int intResourceWorkflow = oResourceRequest.AddWorkflow(_rrid, 0, oResourceRequest.Get(_rrid, "name"), intUser, Int32.Parse(oResourceRequest.Get(_rrid, "devices")), double.Parse(oResourceRequest.Get(_rrid, "allocated")), 2, 0);
                oLog.AddEvent(intRequest.ToString(), strCVT, "Request assigned to " + oUser.GetFullNameWithLanID(intUser) + " by the system", LoggingType.Debug);
                string strDefault = oUser.GetApplicationUrl(intUser, _viewpage);
                if (strDefault == "")
                    oFunction.SendEmail("Request Assignment: " + strService, oUser.GetName(intUser), _cc, strEMailIdsBCC, "Request Assignment: " + strService, "<p><b>The following request has been automatically assigned to you</b><p><p>" + oResourceRequest.GetWorkflowSummary(intResourceWorkflow, _environment, _se_dsn, _dsn_asset, _dsn_ip) + "</p>", true, false);
                else
                {
                    if (intProject > 0)
                        oFunction.SendEmail("Request Assignment: " + strService, oUser.GetName(intUser), _cc, strEMailIdsBCC, "Request Assignment: " + strService, "<p><b>The following request has been automatically assigned to you</b><p><p><a href=\"" + oVariable.URL() + "/redirect.aspx?referrer=/" + strDefault + oPage.GetFullLink(_viewpage) + "?pid=" + intProject.ToString() + "\" target=\"_blank\">Click here to review your new assignment.</a></p><p>" + oResourceRequest.GetWorkflowSummary(intResourceWorkflow, _environment, _se_dsn, _dsn_asset, _dsn_ip) + "</p>", true, false);
                    else
                        oFunction.SendEmail("Request Assignment: " + strService, oUser.GetName(intUser), _cc, strEMailIdsBCC, "Request Assignment: " + strService, "<p><b>The following request has been automatically assigned to you</b><p><p><a href=\"" + oVariable.URL() + "/redirect.aspx?referrer=/frame/resource_request.aspx?rrid=" + intResourceWorkflow.ToString() + "\" target=\"_blank\">Click here to review your new assignment.</a></p><p>" + oResourceRequest.GetWorkflowSummary(intResourceWorkflow, _environment, _se_dsn, _dsn_asset, _dsn_ip) + "</p>", true, false);
                }
                string strActivity = "<tr><td><b>Resource:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>" + oUser.GetFullName(intUser) + "</td></tr>";
                strActivity += strSpacerRow;
                strActivity += "<tr><td><b>Activity Type:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>" + oRequestItem.GetItemName(_itemid) + "</td></tr>";
                strActivity = "<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"" + oVariable.DefaultFontStyle() + "\">" + strActivity + "</table>";
                string strDeliverable = oApplication.Get(intApp, "deliverables_doc");
                if (strDeliverable.Trim() != "")
                    strDeliverable = "<p><a href=\"" + oVariable.URL() + strDeliverable + "\">Click here to view the service deliverables</a></p>";
                if (oService.Get(intService, "notify_client") != "0")
                    oFunction.SendEmail("Request Assignment: " + strService, oUser.GetName(intRequester), _cc, strEMailIdsBCC, "Request Assignment: " + strService, "<p><b>A resource has been assigned to the following request...</b><p><p>" + oResourceRequest.GetWorkflowSummary(intResourceWorkflow, _environment, _se_dsn, _dsn_asset, _dsn_ip) + "</p><p>" + strActivity + "</p>" + strDeliverable + strNotify, true, false);
                oProject.Update(intProject, 2);
                oResourceRequest.UpdateAccepted(_rrid, 1);
                oResourceRequest.UpdateAssignedBy(_rrid, -1000);
            }
            else
            {
                DataSet dsTechnicians = oService.GetUser(intService, 0);
                foreach (DataRow drTechnician in dsTechnicians.Tables[0].Rows)
                {
                    boolTech = true;
                    intUser = Int32.Parse(drTechnician["userid"].ToString());
                    string strNotify = "";
                    //if (oProject.Get(intProject, "number").StartsWith("CV") == false)
                    //    strNotify = "<p><span style=\"color:#0000FF\"><b>PROJECT COORDINATOR:</b> Please allocate the hours listed above for each resource in Clarity.</span></p>";
                    // Add Workflow
                    int intResourceWorkflow = oResourceRequest.AddWorkflow(_rrid, 0, oResourceRequest.Get(_rrid, "name"), intUser, Int32.Parse(oResourceRequest.Get(_rrid, "devices")), double.Parse(oResourceRequest.Get(_rrid, "allocated")), 2, 1);
                    oLog.AddEvent(intRequest.ToString(), strCVT, "Request assigned to " + oUser.GetFullNameWithLanID(intUser) + " from service techician configuration", LoggingType.Debug);
                    // NOTIFY
                    if (strEmail != "" && strEmail != "0")
                    {
                        string strDefault = oApplication.GetUrl(intApp, _viewpage);
                        if (strDefault == "")
                            oFunction.SendEmail("Request Assignment: " + strService, strEmail, oUser.GetEmail(_cc, _environment), strEMailIdsBCC, "Request Assignment: " + strService, "<p><b>The following request has been automatically assigned to you</b></p><p>" + oResourceRequest.GetWorkflowSummary(intResourceWorkflow, _environment, _se_dsn, _dsn_asset, _dsn_ip) + "</p>", false, false);
                        else
                        {
                            if (intProject > 0)
                                oFunction.SendEmail("Request Assignment: " + strService, strEmail, oUser.GetEmail(_cc, _environment), strEMailIdsBCC, "Request Assignment: " + strService, "<p><b>The following request has been automatically assigned to you</b><p><p><a href=\"" + oVariable.URL() + "/redirect.aspx?referrer=/" + strDefault + oPage.GetFullLink(_viewpage) + "?pid=" + intProject.ToString() + "\" target=\"_blank\">Click here to review your new assignment.</a></p><p>" + oResourceRequest.GetWorkflowSummary(intResourceWorkflow, _environment, _se_dsn, _dsn_asset, _dsn_ip) + "</p>", false, false);
                            else
                                oFunction.SendEmail("Request Assignment: " + strService, strEmail, oUser.GetEmail(_cc, _environment), strEMailIdsBCC, "Request Assignment: " + strService, "<p><b>The following request has been automatically assigned to you</b><p><p><a href=\"" + oVariable.URL() + "/redirect.aspx?referrer=/frame/resource_request.aspx?rrid=" + intResourceWorkflow.ToString() + "\" target=\"_blank\">Click here to review your new assignment.</a></p><p>" + oResourceRequest.GetWorkflowSummary(intResourceWorkflow, _environment, _se_dsn, _dsn_asset, _dsn_ip) + "</p>", false, false);
                        }
                    }
                    else
                    {
                        string strDefault = oUser.GetApplicationUrl(intUser, _viewpage);
                        if (strDefault == "")
                            oFunction.SendEmail("Request Assignment: " + strService, oUser.GetName(intUser), _cc, strEMailIdsBCC, "Request Assignment: " + strService, "<p><b>The following request has been automatically assigned to you</b><p><p>" + oResourceRequest.GetWorkflowSummary(intResourceWorkflow, _environment, _se_dsn, _dsn_asset, _dsn_ip) + "</p>", true, false);
                        else
                        {
                            if (intProject > 0)
                                oFunction.SendEmail("Request Assignment: " + strService, oUser.GetName(intUser), _cc, strEMailIdsBCC, "Request Assignment: " + strService, "<p><b>The following request has been automatically assigned to you</b><p><p><a href=\"" + oVariable.URL() + "/redirect.aspx?referrer=/" + strDefault + oPage.GetFullLink(_viewpage) + "?pid=" + intProject.ToString() + "\" target=\"_blank\">Click here to review your new assignment.</a></p><p>" + oResourceRequest.GetWorkflowSummary(intResourceWorkflow, _environment, _se_dsn, _dsn_asset, _dsn_ip) + "</p>", true, false);
                            else
                                oFunction.SendEmail("Request Assignment: " + strService, oUser.GetName(intUser), _cc, strEMailIdsBCC, "Request Assignment: " + strService, "<p><b>The following request has been automatically assigned to you</b><p><p><a href=\"" + oVariable.URL() + "/redirect.aspx?referrer=/frame/resource_request.aspx?rrid=" + intResourceWorkflow.ToString() + "\" target=\"_blank\">Click here to review your new assignment.</a></p><p>" + oResourceRequest.GetWorkflowSummary(intResourceWorkflow, _environment, _se_dsn, _dsn_asset, _dsn_ip) + "</p>", true, false);
                        }
                        string strActivity = "<tr><td><b>Resource:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>" + oUser.GetFullName(intUser) + "</td></tr>";
                        strActivity += strSpacerRow;
                        strActivity += "<tr><td><b>Activity Type:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>" + oRequestItem.GetItemName(_itemid) + "</td></tr>";
                        strActivity = "<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"" + oVariable.DefaultFontStyle() + "\">" + strActivity + "</table>";
                        string strDeliverable = oApplication.Get(intApp, "deliverables_doc");
                        if (strDeliverable.Trim() != "")
                            strDeliverable = "<p><a href=\"" + oVariable.URL() + strDeliverable + "\">Click here to view the service deliverables</a></p>";
                        if (oService.Get(intService, "notify_client") != "0")
                            oFunction.SendEmail("Request Assignment: " + strService, oUser.GetName(intRequester), _cc, strEMailIdsBCC, "Request Assignment: " + strService, "<p><b>A resource has been assigned to the following request...</b><p><p>" + oResourceRequest.GetWorkflowSummary(intResourceWorkflow, _environment, _se_dsn, _dsn_asset, _dsn_ip) + "</p><p>" + strActivity + "</p>" + strDeliverable + strNotify, true, false);
                    }
                    oProject.Update(intProject, 2);
                    oResourceRequest.UpdateAccepted(_rrid, 1);
                    oResourceRequest.UpdateAssignedBy(_rrid, -1000);
                }
            }
            if (boolTech == false)
            {
                oLog.AddEvent(intRequest.ToString(), strCVT, "Sending request to service manager(s) for assignment", LoggingType.Debug);
                // If no technicians assigned
                if (strEmail != "" && strEmail != "0")
                {
                    // If group mailbox
                    string strDefault = oApplication.GetUrl(intApp, _assignpage);
                    oLog.AddEvent(intRequest.ToString(), strCVT, "Request sent to " + strEmail + " (group mailbox) for assignment", LoggingType.Debug);
                    if (strDefault == "")
                        oFunction.SendEmail("Request Submitted: " + strService, strEmail, "", strEMailIdsBCC, "Request Submitted: " + strService, "<p><b>A resource from your department has been requested; you are required to assign a resource to this initiative.</b></p><p>" + oResourceRequest.GetSummary(_rrid, 0, _environment, _se_dsn, _dsn_asset, _dsn_ip) + "</p>", false, false);
                    else
                        oFunction.SendEmail("Request Submitted: " + strService, strEmail, "", strEMailIdsBCC, "Request Submitted: " + strService, "<p><b>A resource from your department has been requested; you are required to assign a resource to this initiative.</b></p><p><a href=\"" + oVariable.URL() + "/redirect.aspx?referrer=/" + strDefault + oPage.GetFullLink(_assignpage) + "?rrid=" + _rrid.ToString() + "\" target=\"_blank\">Click here to assign a resource.</a></p><p>" + oResourceRequest.GetSummary(_rrid, 0, _environment, _se_dsn, _dsn_asset, _dsn_ip) + "</p>", false, false);
                }
                else
                {
                    // If no group mailbox, notify team leads
                    DataSet dsManagers = oService.GetUser(intService, 1);
                    foreach (DataRow drManager in dsManagers.Tables[0].Rows)
                    {
                        intUser = Int32.Parse(drManager["userid"].ToString());
                        oLog.AddEvent(intRequest.ToString(), strCVT, "Request sent to " + oUser.GetFullNameWithLanID(intUser) + " for assignment", LoggingType.Debug);
                        string strDefault = oUser.GetApplicationUrl(intUser, _assignpage);
                        if (strDefault == "")
                            oFunction.SendEmail("Request Submitted: " + strService, oUser.GetName(intUser), "", strEMailIdsBCC, "Request Submitted: " + strService, "<p><b>A resource from your department has been requested; you are required to assign a resource to this initiative.</b></p><p>" + oResourceRequest.GetSummary(_rrid, 0, _environment, _se_dsn, _dsn_asset, _dsn_ip) + "</p>", true, false);
                        else
                            oFunction.SendEmail("Request Submitted: " + strService, oUser.GetName(intUser), "", strEMailIdsBCC, "Request Submitted: " + strService, "<p><b>A resource from your department has been requested; you are required to assign a resource to this initiative.</b></p><p><a href=\"" + oVariable.URL() + "/redirect.aspx?referrer=/" + strDefault + oPage.GetFullLink(_assignpage) + "?rrid=" + _rrid.ToString() + "\" target=\"_blank\">Click here to assign a resource.</a></p><p>" + oResourceRequest.GetSummary(_rrid, 0, _environment, _se_dsn, _dsn_asset, _dsn_ip) + "</p>", true, false);
                    }
                }
            }
        }
        public string GetStatus(double _status)
        {
            string strRed = "FF";
            string strGreen = "FF";
            if (_status > 0.50)
            {
                // Decrease Red
                double dblPercent = _status - 0.50;
                dblPercent = dblPercent * 255;
                dblPercent = Math.Ceiling(255 - dblPercent);
                int intPercent = Int32.Parse(dblPercent.ToString());
                strRed = intPercent.ToString("X");
            }
            else
            {
                // Decrease Green
                double dblPercent = _status;
                dblPercent = Math.Ceiling(dblPercent * 255);
                int intPercent = Int32.Parse(dblPercent.ToString());
                strGreen = intPercent.ToString("X");
            }
            if (strRed.Length == 1)
                strRed = "0" + strRed;
            if (strGreen.Length == 1)
                strGreen = "0" + strGreen;
            return "#" + strRed + strGreen + "00";
        }
        public string GetStatusBarIn(double dblProgress, string _width, string _height, bool _show_text)
        {
            string strReturn = "";
            double dblTotal = double.Parse(_width);
            if (dblProgress > 100.00)
                dblProgress = 100.00;
            double dblWidth = (dblProgress / 100.00) * double.Parse(_width);
            dblTotal = dblTotal - dblWidth;
            string strProgress = "";
            if (_show_text == true)
                strProgress = (dblProgress < 10.00 ? "&nbsp;" : "") + (dblProgress < 100.00 ? "&nbsp;" : "") + dblProgress.ToString("0") + "%";
            strReturn = "<table title=\"" + dblProgress.ToString("F") + "%\" border=\"0\" cellpadding=\"1\" cellspacing=\"1\"><tr><td><table width=\"" + _width + "\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"border:solid 1px #666666\"><tr>";
            strReturn += "<td width=\"" + dblWidth.ToString("0") + "\" bgcolor=\"" + GetStatus(dblProgress / 100.00) + "\" align=\"right\"><img src=\"/images/spacer.gif\" border=\"0\" width=\"1\" height=\"" + _height + "\">";
            if (_show_text == true)
            {
                if (dblTotal <= 40.00)
                    strReturn += strProgress + "&nbsp;";
                strReturn += "</td><td width=\"" + dblTotal.ToString("0") + "\">";
                if (dblTotal > 40.00)
                    strReturn += "&nbsp;" + strProgress;
                strReturn += "</td>";
            }
            else
            {
                strReturn += "</td>";
                strReturn += "<td width=\"" + dblTotal.ToString("0") + "\"></td>";
            }
            strReturn += "</tr></table></td><td>";
            strReturn += "</td></tr>";
            strReturn += "<tr><td colspan=\"2\"><img src=\"/images/spacer.gif\" border=\"0\" width=\"" + _width + "\" height=\"1\"></td></tr>";
            strReturn += "</table>";
            return strReturn;
        }
        public string GetStatusBar(double dblProgress, string _width, string _height, bool _show_text)
        {
            StringBuilder sbReturn = new StringBuilder();
            double dblTotal = double.Parse(_width);
            if (dblProgress > 100.00)
                dblProgress = 100.00;
            if (dblProgress < 0.00)
                dblProgress = 0.00;
            double dblWidth = (dblProgress / 100.00) * double.Parse(_width);
            dblTotal = dblTotal - dblWidth;
            sbReturn.Append("<table title=\"");
            sbReturn.Append(dblProgress.ToString("F"));
            sbReturn.Append("%\" border=\"0\" cellpadding=\"1\" cellspacing=\"1\"><tr><td><table width=\"");
            sbReturn.Append(_width);
            sbReturn.Append("\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"border:solid 1px #666666\"><tr>");
            sbReturn.Append("<td bgcolor=\"");
            sbReturn.Append(GetStatus(dblProgress / 100.00));
            sbReturn.Append("\"><img src=\"/images/spacer.gif\" border=\"0\" width=\"");
            sbReturn.Append(dblWidth.ToString("F"));
            sbReturn.Append("\" height=\"");
            sbReturn.Append(_height);
            sbReturn.Append("\"></td>");
            sbReturn.Append("<td><img src=\"/images/spacer.gif\" border=\"0\" width=\"");
            sbReturn.Append(dblTotal.ToString("F"));
            sbReturn.Append("\" height=\"");
            sbReturn.Append(_height);
            sbReturn.Append("\"></td>");
            sbReturn.Append("</tr></table></td>");
            if (_show_text == true)
            {
                sbReturn.Append("<td>");
                sbReturn.Append(dblProgress < 10.00 ? "&nbsp;" : "");
                sbReturn.Append(dblProgress < 100.00 ? "&nbsp;" : "");
                sbReturn.Append(dblProgress.ToString("F"));
                sbReturn.Append("%</td>");
            }
            sbReturn.Append("</tr></table>");
            return sbReturn.ToString();
        }
        public string GetStatusBarBlue(double dblProgress, string _width, bool _show_text)
        {
            double dblLeft = 100.00 - dblProgress;
            return "<table width=\"" + _width + "%\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"border:solid 1px #999999\"><tr><td background=\"/images/progress_background.gif\" width=\"" + dblProgress.ToString("F") + "%\"><img src=\"/images/spacer.gif\" border=\"0\" width=\"12\" height=\"12\"></td><td align=\"center\" width=\"" + dblLeft.ToString("F") + "%\">" + (_show_text ? "<b>" + dblProgress.ToString("F0") + "%</b>" : "") + "</td></tr></table>";
        }
        public string GetStatusBarFill(double dblProgress, string _width, bool _show_text, string _fill)
        {
            double dblLeft = 100.00 - dblProgress;
            return "<table width=\"" + _width + "%\" border=\"0\" cellpadding=\"0\" cellspacing=\"4\" style=\"border:solid 1px #999999\"><tr><td bgcolor=\"" + _fill + "\" width=\"" + dblProgress.ToString("F") + "%\"><img src=\"/images/spacer.gif\" border=\"0\" width=\"1\" height=\"12\"></td><td align=\"center\" width=\"" + dblLeft.ToString("F") + "%\">" + (_show_text ? "<b>" + dblProgress.ToString("F0") + "%</b>" : "") + "</td></tr></table>";
        }
    }
}
