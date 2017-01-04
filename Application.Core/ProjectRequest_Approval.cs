using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using System.Data.SqlTypes;

namespace NCC.ClearView.Application.Core
{
	public class ProjectRequest_Approval
	{
		private string dsn = "";
		private int user = 0;
        private int intEnvironment = 0;
        private SqlParameter[] arParams;
		private DataSet ds;
        private ProjectRequest oProjectRequest;
        private Functions oFunction;
        private Variables oVariable;
        private Projects oProject;
        private Requests oRequest;
        private Users oUser;
        private Pages oPage;
        private string strTitle = "Project Request Prioritization and Approval Workflow";
        private string strEmailTitle = "ClearView Workflow";
        string strEMailIdsBCC = "";
        public ProjectRequest_Approval(int _user, string _dsn, int _environment)
		{
			user = _user;
			dsn = _dsn;
            intEnvironment = _environment;
            oProjectRequest = new ProjectRequest(user, dsn);
            oFunction = new Functions(user, dsn, intEnvironment);
            oVariable = new Variables(_environment);
            oUser = new Users(user, dsn);
            oPage = new Pages(user, dsn);
            oProject = new Projects(user, dsn);
            oRequest = new Requests(user, dsn);
            strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_PROJECT");

		}
        // ************************************************************************************
        // ************************************************************************************
        // The following rules apply to the project request approval workflow...
        //   1) New request
        //      a) If expedited, send to Board
        //      b) If not expedited, send to manager
        //   2) Manager Level
        //      a) If approve, send to platform manager
        //      b) If not approved, send message to user and stop workflow
        //   3) Platform Level
        //      a) On first approval, send to board
        //      b) If not approved by ALL, send message to user and stop workflow
        //   4) Board Level
        //      a) On majority approval, send to director
        //      b) If majority fails, send message to user and stop workflow
        //      c) If tied, approve and send to director
        //   5) Director Level
        //      a) If approve, approve the request, send message to user and send message to 
        //         TPM (working sponsor) to request resources.
        //      b) If not approved, send message to user and stop workflow
        // ************************************************************************************
        // ************************************************************************************
        public void NewRequest(int _requestid, int _userid, bool _expedited, int _pageid, bool _director)
        {
            DeleteRequest(_requestid);
            string strBody = oProjectRequest.GetBody(_requestid, intEnvironment, true);
            Platforms oPlatform = new Platforms(user, dsn);
            if (_expedited == false)
            {
                // LOAD MANAGER DETAIL
                int intManager = Int32.Parse(oUser.Get(_userid, "manager"));
                if (intManager > 0)
                {
                    LoadTable(_requestid, 1, _director);
                    LoadTableDetail(_requestid, intManager, 0, 1);
                    string strDefault = oUser.GetApplicationUrl(intManager, _pageid);
                    if (strDefault == "")
                        oFunction.SendEmail(strEmailTitle, oUser.GetName(intManager) + ";", "", strEMailIdsBCC, strTitle, "<p><b>As the MANAGER (" + oUser.GetFullName(intManager) + ") this project request requires your approval...</b></p><p>" + strBody + "</p>", true, false);
                    else
                        oFunction.SendEmail(strEmailTitle, oUser.GetName(intManager) + ";", "", strEMailIdsBCC, strTitle, "<p><b>As the MANAGER (" + oUser.GetFullName(intManager) + ") this project request requires your approval...</b></p><p><a href=\"" + oVariable.URL() + "/redirect.aspx?referrer=/" + strDefault + oPage.GetFullLink(_pageid) + "?rid=" + _requestid.ToString() + "\" target=\"_blank\">Click here to view this project request.</a></p><p>" + strBody + "</p>", true, false);
                    oFunction.SendEmail(strEmailTitle, oUser.GetName(_userid), "", strEMailIdsBCC, strTitle, "<p><b>The following project request has been routed to your manager (" + oUser.GetFullName(intManager) + ") for approval.</b></p><p>" + oProjectRequest.GetBody(_requestid, intEnvironment, true) + "</p>", true, false);
                    // LOAD PLATFORM DETAIL
                    ds = oProjectRequest.GetPlatforms(_requestid);
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        int intUser = oPlatform.GetManager(Int32.Parse(dr["platformid"].ToString()));
                        if (intUser > 0)
                            LoadTableDetail(_requestid, intUser, -10, 2);
                    }
                    // LOAD BOARD DETAIL
                    ds = oUser.GetBoard();
                    foreach (DataRow dr in ds.Tables[0].Rows)
                        LoadTableDetail(_requestid, Int32.Parse(dr["userid"].ToString()), -10, 3);
                }
                else
                {
                    bool boolPlatform = false;
                    // LOAD AND SEND TO PLATFORM (no manager)
                    ds = oProjectRequest.GetPlatforms(_requestid);
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        int intUser = oPlatform.GetManager(Int32.Parse(dr["platformid"].ToString()));
                        if (intUser > 0)
                        {
                            boolPlatform = true;
                            LoadTableDetail(_requestid, intUser, 0, 2);
                            string strDefault = oUser.GetApplicationUrl(intUser, _pageid);
                            if (strDefault == "")
                                oFunction.SendEmail(strEmailTitle, oUser.GetName(intUser) + ";", "", strEMailIdsBCC, strTitle, "<p><b>As the PLATFORM MANAGER (" + oUser.GetFullName(intUser) + ") this project request requires your approval...</b></p><p>" + strBody + "</p>", true, false);
                            else
                                oFunction.SendEmail(strEmailTitle, oUser.GetName(intUser) + ";", "", strEMailIdsBCC, strTitle, "<p><b>As the PLATFORM MANAGER (" + oUser.GetFullName(intUser) + ") this project request requires your approval...</b></p><p><a href=\"" + oVariable.URL() + "/redirect.aspx?referrer=/" + strDefault + oPage.GetFullLink(_pageid) + "?rid=" + _requestid.ToString() + "\" target=\"_blank\">Click here to view this project request.</a></p><p>" + strBody + "</p>", true, false);
                        }
                    }
                    if (boolPlatform == true)
                    {
                        LoadTable(_requestid, 2, _director);
                        oFunction.SendEmail(strEmailTitle, oUser.GetName(_userid), "", strEMailIdsBCC, strTitle, "<p><b>The following project request has been routed to the PLATFORM MANAGER(S) for approval.</b></p><p>" + oProjectRequest.GetBody(_requestid, intEnvironment, true) + "</p>", true, false);
                        // LOAD BOARD DETAIL
                        ds = oUser.GetBoard();
                        foreach (DataRow dr in ds.Tables[0].Rows)
                            LoadTableDetail(_requestid, Int32.Parse(dr["userid"].ToString()), -10, 3);
                    }
                    else
                    {
                        LoadTable(_requestid, 3, _director);
                        // LOAD BOARD DETAIL
                        ds = oUser.GetBoard();
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            LoadTableDetail(_requestid, Int32.Parse(dr["userid"].ToString()), 0, 3);
                            int intUser = Int32.Parse(dr["userid"].ToString());
                            string strDefault = oUser.GetApplicationUrl(intUser, _pageid);
                            if (strDefault == "")
                                oFunction.SendEmail(strEmailTitle, oUser.GetName(intUser) + ";", "", strEMailIdsBCC, strTitle, "<p><b>As a BOARD MEMBER (" + oUser.GetFullName(intUser) + ") this project request requires your approval...</b></p><p>" + strBody + "</p>", true, false);
                            else
                                oFunction.SendEmail(strEmailTitle, oUser.GetName(intUser) + ";", "", strEMailIdsBCC, strTitle, "<p><b>As a BOARD MEMBER (" + oUser.GetFullName(intUser) + ") this project request requires your approval...</b></p><p><a href=\"" + oVariable.URL() + "/redirect.aspx?referrer=/" + strDefault + oPage.GetFullLink(_pageid) + "?rid=" + _requestid.ToString() + "\" target=\"_blank\">Click here to view this project request.</a></p><p>" + strBody + "</p>", true, false);
                        }
                    }
                }
            }
            else
            {
                // LOAD MANAGER DETAIL
                LoadTable(_requestid, 3, _director);
                int intManager = Int32.Parse(oUser.Get(_userid, "manager"));
                LoadTableDetail(_requestid, intManager, -100, 1);
                // LOAD PLATFORM DETAIL
                ds = oProjectRequest.GetPlatforms(_requestid);
                foreach (DataRow dr in ds.Tables[0].Rows)
                    LoadTableDetail(_requestid, oPlatform.GetManager(Int32.Parse(dr["platformid"].ToString())), -100, 2);
                // LOAD BOARD DETAIL
                ds = oUser.GetBoard();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    int intUser = Int32.Parse(dr["userid"].ToString());
                    LoadTableDetail(_requestid, intUser, 0, 3);
                    string strDefault = oUser.GetApplicationUrl(intUser, _pageid);
                    if (strDefault == "")
                        oFunction.SendEmail(strEmailTitle, oUser.GetName(intUser) + ";", "", strEMailIdsBCC, strTitle, "<p><b>As a BOARD MEMBER (" + oUser.GetFullName(intUser) + ") this project request requires your approval...</b></p><p>" + strBody + "</p>", true, false);
                    else
                        oFunction.SendEmail(strEmailTitle, oUser.GetName(intUser) + ";", "", strEMailIdsBCC, strTitle, "<p><b>As a BOARD MEMBER (" + oUser.GetFullName(intUser) + ") this project request requires your approval...</b></p><p><a href=\"" + oVariable.URL() + "/redirect.aspx?referrer=/" + strDefault + oPage.GetFullLink(_pageid) + "?rid=" + _requestid.ToString() + "\" target=\"_blank\">Click here to view this project request.</a></p><p>" + strBody + "</p>", true, false);
                }
                oFunction.SendEmail(strEmailTitle, oUser.GetName(_userid), "", strEMailIdsBCC, strTitle, "<p><b>The following project request has been expedited for approval to the BOARD.</b></p><p>" + oProjectRequest.GetBody(_requestid, intEnvironment, true) + "</p>", true, false);
            }
            // LOAD DIRECTOR
            if (_director == true)
            {
                ds = oUser.GetDirector();
                foreach (DataRow dr in ds.Tables[0].Rows)
                    LoadTableDetail(_requestid, Int32.Parse(dr["userid"].ToString()), -10, 4);
            }
        }
        public void ManagerApproval(int _requestid, int _userid, int _approval, int _pageid, string _external, bool _director)
        {
            Platforms oPlatform = new Platforms(user, dsn);
            UpdateTableDetail(_requestid, _userid, 1, _approval, _external);
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@approval", _approval);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateProjectRequestStatusManager", arParams);
            if (_approval == 1)
            {
                arParams = new SqlParameter[1];
                arParams[0] = new SqlParameter("@requestid", _requestid);
                SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateProjectRequestManager", arParams);
                // MOVE TO PLATFORM
                ApproveRequest(_requestid, "your MANAGER (" + oUser.GetFullName(_userid) + ")");
                ActivateStep(_requestid, 2);
                string strBody = oProjectRequest.GetBody(_requestid, intEnvironment, true);
                ds = GetAllStep(_requestid, 2);
                bool boolFound = false;
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    boolFound = true;
                    int intUser = Int32.Parse(dr["userid"].ToString());
                    string strDefault = oUser.GetApplicationUrl(intUser, _pageid);
                    if (strDefault == "")
                        oFunction.SendEmail(strEmailTitle, oUser.GetName(intUser) + ";", "", strEMailIdsBCC, strTitle, "<p><b>As the PLATFORM MANAGER (" + oUser.GetFullName(intUser) + ") this project request requires your approval...</b></p><p>" + strBody + "</p>", true, false);
                    else
                        oFunction.SendEmail(strEmailTitle, oUser.GetName(intUser) + ";", "", strEMailIdsBCC, strTitle, "<p><b>As the PLATFORM MANAGER (" + oUser.GetFullName(intUser) + ") this project request requires your approval...</b></p><p><a href=\"" + oVariable.URL() + "/redirect.aspx?referrer=/" + strDefault + oPage.GetFullLink(_pageid) + "?rid=" + _requestid.ToString() + "\" target=\"_blank\">Click here to view this project request.</a></p><p>" + strBody + "</p>", true, false);
                }
                if (boolFound == false)
                    PlatformApproval(_requestid, _userid, 1, _pageid, _external, _director);
            }
            else
            {
                DenyRequest(_requestid, _approval, "your MANAGER (" + oUser.GetFullName(_userid) + ")");
            }
        }
        public void PlatformApproval(int _requestid, int _userid, int _approval, int _pageid, string _external, bool _director)
        {
            UpdateTableDetail(_requestid, _userid, 2, _approval, _external);
            int intOverallApproval = GetApproval(_requestid, 2, true);
            if (intOverallApproval == 1)
            {
                if (GetStep(_requestid) == 2)
                {
                    arParams = new SqlParameter[2];
                    arParams[0] = new SqlParameter("@requestid", _requestid);
                    arParams[1] = new SqlParameter("@approval", _approval);
                    SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateProjectRequestStatusPlatform", arParams);
                    arParams = new SqlParameter[1];
                    arParams[0] = new SqlParameter("@requestid", _requestid);
                    SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateProjectRequestPlatform", arParams);
//                    UpdateApproval(_requestid, 0, 100, 2);
                    // MOVE TO BOARD
                    ApproveRequest(_requestid, "the PLATFORM");
                    ActivateStep(_requestid, 3);
                    string strBody = oProjectRequest.GetBody(_requestid, intEnvironment, true);
                    ds = GetAllStep(_requestid, 3);
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        int intUser = Int32.Parse(dr["userid"].ToString());
                        string strDefault = oUser.GetApplicationUrl(intUser, _pageid);
                        if (strDefault == "")
                            oFunction.SendEmail(strEmailTitle, oUser.GetName(intUser) + ";", "", strEMailIdsBCC, strTitle, "<p><b>As a BOARD MEMBER (" + oUser.GetFullName(intUser) + ") this project request requires your approval...</b></p><p>" + strBody + "</p>", true, false);
                        else
                            oFunction.SendEmail(strEmailTitle, oUser.GetName(intUser) + ";", "", strEMailIdsBCC, strTitle, "<p><b>As a BOARD MEMBER (" + oUser.GetFullName(intUser) + ") this project request requires your approval...</b></p><p><a href=\"" + oVariable.URL() + "/redirect.aspx?referrer=/" + strDefault + oPage.GetFullLink(_pageid) + "?rid=" + _requestid.ToString() + "\" target=\"_blank\">Click here to view this project request.</a></p><p>" + strBody + "</p>", true, false);
                    }
                }
            }
            else if (intOverallApproval != 0)
            {
                arParams = new SqlParameter[2];
                arParams[0] = new SqlParameter("@requestid", _requestid);
                arParams[1] = new SqlParameter("@approval", _approval);
                SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateProjectRequestStatusPlatform", arParams);
                UpdateApproval(_requestid, 0, 100, 2);
                DenyRequest(_requestid, intOverallApproval, "the PLATFORM");
            }
        }
        public void BoardApproval(int _requestid, int _userid, int _approval, int _pageid, int _workload, string _external, int _requestpageid, bool _director)
        {
            UpdateTableDetail(_requestid, _userid, 3, _approval, _external);
            int intOverallApproval = GetApproval(_requestid, 3, false);
            if (intOverallApproval == 1)
            {
                arParams = new SqlParameter[2];
                arParams[0] = new SqlParameter("@requestid", _requestid);
                arParams[1] = new SqlParameter("@approval", _approval);
                SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateProjectRequestStatusBoard", arParams);
                arParams = new SqlParameter[1];
                arParams[0] = new SqlParameter("@requestid", _requestid);
                SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateProjectRequestBoard", arParams);
                UpdateApproval(_requestid, 0, 100, 3);
                // MOVE TO DIRECTOR
                ApproveRequest(_requestid, "the BOARD");
                if (_director == true)
                {
                    ActivateStep(_requestid, 4);
                    string strBody = oProjectRequest.GetBody(_requestid, intEnvironment, true);
                    ds = GetAllStep(_requestid, 4);
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        int intUser = Int32.Parse(dr["userid"].ToString());
                        string strDefault = oUser.GetApplicationUrl(intUser, _pageid);
                        if (strDefault == "")
                            oFunction.SendEmail(strEmailTitle, oUser.GetName(intUser) + ";", "", strEMailIdsBCC, strTitle, "<p><b>As the DIRECTOR (" + oUser.GetFullName(intUser) + ") this project request requires your approval...</b></p><p>" + strBody + "</p>", true, false);
                        else
                            oFunction.SendEmail(strEmailTitle, oUser.GetName(intUser) + ";", "", strEMailIdsBCC, strTitle, "<p><b>As the DIRECTOR (" + oUser.GetFullName(intUser) + ") this project request requires your approval...</b></p><p><a href=\"" + oVariable.URL() + "/redirect.aspx?referrer=/" + strDefault + oPage.GetFullLink(_pageid) + "?rid=" + _requestid.ToString() + "\" target=\"_blank\">Click here to view this project request.</a></p><p>" + strBody + "</p>", true, false);
                    }
                }
                else
                    FinishRequest(_requestid, _workload, _requestpageid);
            }
            else if (intOverallApproval != 0)
            {
                arParams = new SqlParameter[2];
                arParams[0] = new SqlParameter("@requestid", _requestid);
                arParams[1] = new SqlParameter("@approval", _approval);
                SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateProjectRequestStatusBoard", arParams);
                UpdateApproval(_requestid, 0, 100, 3);
                DenyRequest(_requestid, intOverallApproval, "the BOARD");
            }
        }
        public void DirectorApproval(int _requestid, int _userid, int _approval, int _pageid, int _workload, string _external, int _requestpageid)
        {
            UpdateTableDetail(_requestid, _userid, 4, _approval, _external);
            int intOverallApproval = GetApproval(_requestid, 4, false);
            if (intOverallApproval == 1)
            {
                arParams = new SqlParameter[2];
                arParams[0] = new SqlParameter("@requestid", _requestid);
                arParams[1] = new SqlParameter("@approval", _approval);
                SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateProjectRequestStatusDirector", arParams);
                arParams = new SqlParameter[1];
                arParams[0] = new SqlParameter("@requestid", _requestid);
                SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateProjectRequestDirector", arParams);
                UpdateApproval(_requestid, 0, 100, 4);
                ApproveRequest(_requestid, "DIRECTOR");
                FinishRequest(_requestid, _workload, _requestpageid);
            }
            else if (intOverallApproval != 0)
            {
                arParams = new SqlParameter[2];
                arParams[0] = new SqlParameter("@requestid", _requestid);
                arParams[1] = new SqlParameter("@approval", _approval);
                SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateProjectRequestStatusDirector", arParams);
                UpdateApproval(_requestid, 0, 100, 4);
                DenyRequest(_requestid, intOverallApproval, "DIRECTOR");
            }
        }
        public void LoadTable(int _requestid, int _step, bool _director)
        {
            int intManager = 0;
            int intPlatform = -10;
            int intBoard = -10;
            int intDirector = 100;
            if (_step == 3)
            {
                intManager = -100;
                intPlatform = -100;
                intBoard = 0;
            }
            if (_director == true)
                intDirector = -10;
            arParams = new SqlParameter[6];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@manager", intManager);
            arParams[2] = new SqlParameter("@platform", intPlatform);
            arParams[3] = new SqlParameter("@board", intBoard);
            arParams[4] = new SqlParameter("@director", intDirector);
            arParams[5] = new SqlParameter("@step", _step);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addProjectRequestStatus", arParams);
        }
        public void LoadTableDetail(int _requestid, int _userid, int _approval, int _step)
        {
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@userid", _userid);
            arParams[2] = new SqlParameter("@approval", _approval);
            arParams[3] = new SqlParameter("@step", _step);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addProjectRequestStatusDetail", arParams);
        }
        public void DenyRequest(int _requestid, int _approval, string _by)
        {
            Projects oProject = new Projects(user, dsn);
            int intProject = oRequest.GetProjectNumber(_requestid);
            int intWorking = Int32.Parse(oProject.Get(intProject, "working"));
            int intExecutive = Int32.Parse(oProject.Get(intProject, "executive"));
            oProject.Update(intProject, _approval);
            string strStatus = "ERROR";
            switch (_approval)
            {
                case -1:
                    strStatus = "DENIED";
                    break;
                case 10:
                    strStatus = "SHELVED";
                    break;
            }
            int intRequester = oRequest.GetUser(_requestid);
            string _external = GetComments(_requestid);
            if (_external.Trim() == "")
                oFunction.SendEmail(strEmailTitle, oUser.GetName(intRequester), oUser.GetName(intWorking) + ";" + oUser.GetName(intExecutive) + ";", strEMailIdsBCC, strTitle, "<p><b>The following project request has been " + strStatus + " by " + _by + ".</b></p><p>" + oProjectRequest.GetBody(_requestid, intEnvironment, true) + "</p>", true, false);
            else
                oFunction.SendEmail(strEmailTitle, oUser.GetName(intRequester), oUser.GetName(intWorking) + ";" + oUser.GetName(intExecutive) + ";", strEMailIdsBCC, strTitle, "<p><b>The following project request has been " + strStatus + " by " + _by + ".</b></p><p>" + oProjectRequest.GetBody(_requestid, intEnvironment, true) + "</p><p>The following comments were added to this request:<br/>" + _external + "<p>", true, false);
        }
        public void ApproveRequest(int _requestid, string _by)
        {
            int intProject = oRequest.GetProjectNumber(_requestid);
            int intRequester = oRequest.GetUser(_requestid);
            string _external = GetComments(_requestid);
            if (_external.Trim() == "")
                oFunction.SendEmail(strEmailTitle, oUser.GetName(intRequester), "", strEMailIdsBCC, strTitle, "<p><b>The following project request has been APPROVED by " + _by + ".</b></p><p>" + oProjectRequest.GetBody(_requestid, intEnvironment, true) + "</p>", true, false);
            else
                oFunction.SendEmail(strEmailTitle, oUser.GetName(intRequester), "", strEMailIdsBCC, strTitle, "<p><b>The following project request has been APPROVED by " + _by + ".</b></p><p>" + oProjectRequest.GetBody(_requestid, intEnvironment, true) + "</p><p>The following comments were added to this request:<br/>" + _external + "<p>", true, false);
        }
        public void FinishRequest(int _requestid, int _workload, int _requestpageid)
        {
            // Do not assign a tpm - send to user to complete a service request
            // *** _requestpageid = New Service Request pageid
            Projects oProject = new Projects(user, dsn);
            ResourceRequest oResourceRequest = new ResourceRequest(user, dsn);
            int intProject = oRequest.GetProjectNumber(_requestid);
            int intWorking = Int32.Parse(oProject.Get(intProject, "working"));
            int intExecutive = Int32.Parse(oProject.Get(intProject, "executive"));
            oProject.Update(intProject, 1);
            int intRequester = oRequest.GetUser(_requestid);
            //            oFunction.SendEmail(strEmailTitle, oUser.GetName(intRequester), oUser.GetName(intWorking) + ";" + oUser.GetName(intExecutive) + ";", strEMailIdsBCC, strTitle, "<p><b>The following project request has been COMPLETELY APPROVED!!!</b></p><p>The assigned Project Coordinator will be contacting you shortly.</p><p>" + oProjectRequest.GetBody(_requestid, intEnvironment, true) + "</p>", true, false);
            string strEmail = "";
            string strCC = "";
            int intResource = 0;
            ds = oProjectRequest.GetResources(_requestid);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["userid"].ToString() == "0")
                {
                    Services oService = new Services(user, dsn);
                    DataSet dsManagers = oService.GetUser(Int32.Parse(ds.Tables[0].Rows[0]["serviceid"].ToString()), 1);
                    foreach (DataRow drManager in dsManagers.Tables[0].Rows)
                    {
                        intResource = Int32.Parse(drManager["userid"].ToString());
                        strEmail += oUser.GetName(intResource) + ";";
                        int intManager = Int32.Parse(oUser.Get(intResource, "manager"));
                        if (intManager > 0)
                            strCC += oUser.GetName(intManager) + ";";
                    }
                    string strDefault = oUser.GetApplicationUrl(intResource, _requestpageid);
                    if (strDefault == "")
                        oFunction.SendEmail(strEmailTitle, strEmail, strCC, strEMailIdsBCC, strTitle, "<p><b>The following project request has been COMPLETELY APPROVED!!!</b></p><p>" + oProjectRequest.GetBody(_requestid, intEnvironment, true) + "</p><p>A Technical Project Manager has been requested; you are required to assign a resource to this project.</p>", true, false);
                    else
                        oFunction.SendEmail(strEmailTitle, strEmail, strCC, strEMailIdsBCC, strTitle, "<p><b>The following project request has been COMPLETELY APPROVED!!!</b></p><p>" + oProjectRequest.GetBody(_requestid, intEnvironment, true) + "</p><p>A Technical Project Manager has been requested; you are required to assign a resource to this project.</p><p><a href=\"" + oVariable.URL() + "/redirect.aspx?referrer=/" + strDefault + oPage.GetFullLink(_requestpageid) + "?rrid=" + ds.Tables[0].Rows[0]["id"].ToString() + "\" target=\"_blank\">Click here to assign a resource.</a></p>", true, false);
                }
                else
                {
                    intResource = Int32.Parse(ds.Tables[0].Rows[0]["userid"].ToString());
                    strEmail = oUser.GetName(intResource);
                    string strDefault = oUser.GetApplicationUrl(intResource, _workload);
                    if (strDefault == "")
                        oFunction.SendEmail(strEmailTitle, strEmail, strCC, strEMailIdsBCC, strTitle, "<p><b>The following project request has been COMPLETELY APPROVED!!!</b></p><p>" + oProjectRequest.GetBody(_requestid, intEnvironment, true) + "</p><p>You have been elected the Project Coordinator for this initiative.</p>", true, false);
                    else
                        oFunction.SendEmail(strEmailTitle, strEmail, strCC, strEMailIdsBCC, strTitle, "<p><b>The following project request has been COMPLETELY APPROVED!!!</b></p><p>" + oProjectRequest.GetBody(_requestid, intEnvironment, true) + "</p><p>You have been elected the Project Coordinator for this initiative.</p><p><a href=\"" + oVariable.URL() + "/redirect.aspx?referrer=/" + strDefault + oPage.GetFullLink(_workload) + "?rrid=" + ds.Tables[0].Rows[0]["id"].ToString() + "\" target=\"_blank\">Click here to begin your project.</a></p>", true, false);
                    oFunction.SendEmail(strEmailTitle, oUser.GetName(intRequester), "", strEMailIdsBCC, strTitle, "<p><b>The following project request has been COMPLETELY APPROVED!!!</b></p><p>" + oProjectRequest.GetBody(_requestid, intEnvironment, true) + "</p><p><b>" + oUser.GetFullName(intResource) + " has been assigned as the Project Coordinator.</b></p>", true, false);
                }
            }
            else
            {
                string strDefault = oUser.GetApplicationUrl(intRequester, _requestpageid);
                if (strDefault == "")
                    oFunction.SendEmail(strEmailTitle, oUser.GetName(intRequester), "", strEMailIdsBCC, strTitle, "<p><b>The following project request has been COMPLETELY APPROVED!!!</b></p><p>" + oProjectRequest.GetBody(_requestid, intEnvironment, true) + "</p><p>A Technical Project Manager has been requested; you are required to complete a service request to initiate this assignment.</p>", true, false);
                else
                    oFunction.SendEmail(strEmailTitle, oUser.GetName(intRequester), "", strEMailIdsBCC, strTitle, "<p><b>The following project request has been COMPLETELY APPROVED!!!</b></p><p>" + oProjectRequest.GetBody(_requestid, intEnvironment, true) + "</p><p>A Technical Project Manager has been requested; you are required to complete a service request to initiate this assignment.</p><p><a href=\"" + oVariable.URL() + "/redirect.aspx?referrer=/" + strDefault + oPage.GetFullLink(_requestpageid) + "\" target=\"_blank\">Click here to complete the service request.</a></p>", true, false);
            }
        }
        private string GetComments(int _requestid)
        {
            Variables oVariable = new Variables(intEnvironment);
            string strComments = "";
            string strSpacerRow = "<tr><td colspan=\"3\"><img src=\"" + oVariable.ImageURL() + "/images/spacer.gif\" border=\"0\" width=\"1\" height=\"7\" /></td></tr>";
            DataSet dsDetail = GetDetail(_requestid);
            foreach (DataRow drDetail in dsDetail.Tables[0].Rows)
            {
                if (drDetail["comments"].ToString().Trim() != "")
                    strComments += strSpacerRow + "<tr><td nowrap valign=\"top\"><b>" + oUser.GetFullName(Int32.Parse(drDetail["userid"].ToString())) + ":</b></td><td>&nbsp;&nbsp;&nbsp;</td><td valign=\"top\">" + drDetail["comments"].ToString() + "</td></tr>";
            }
            if (strComments != "")
                strComments = "<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"" + oVariable.DefaultFontStyle() + "\">" + strComments + "</table>";
            return strComments;
        }
        public void UpdateTableDetail(int _requestid, int _userid, int _step, int _approval, string _comments)
        {
            // Update an individual's approval - no matter if it's approved or not
            arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@userid", _userid);
            arParams[2] = new SqlParameter("@step", _step);
            arParams[3] = new SqlParameter("@approval", _approval);
            arParams[4] = new SqlParameter("@comments", _comments);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateProjectRequestStatusDetail", arParams);
        }
        public void ActivateStep(int _requestid, int _step)
        {
            // Update the detail table to set the approval to 0 (pending) from -10 (waiting)
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@step", _step);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateProjectRequestStatusStep", arParams);
        }
        private int GetApproval(int _requestid, int _step, bool _approve_first)
        {
            int intTotal = GetAllStep(_requestid, _step).Tables[0].Rows.Count;
            int intApproved = GetByStep(_requestid, _step, 1).Tables[0].Rows.Count;
            if (intApproved > 0 && _approve_first == true)
                return 1;
            double dblNeeded = Math.Ceiling(double.Parse(intTotal.ToString()) / 2.00);
            int intNeededApprove = Int32.Parse(dblNeeded.ToString());
            int intNeededOther = intNeededApprove;
            if (intTotal % 2 == 0)
                intNeededOther++;
            if (intApproved >= intNeededApprove)
                return 1;
            else {
                int intShelved = GetByStep(_requestid, _step, 10).Tables[0].Rows.Count;
                if (intShelved >= intNeededOther)
                    return 10;
                else
                {
                    int intDenied = GetByStep(_requestid, _step, -1).Tables[0].Rows.Count;
                    if (intDenied >= intNeededOther)
                        return -1;
                    else
                    {
                        if (intApproved + intShelved + intDenied == intTotal)
                        {
                            if (intApproved > intShelved && intApproved > intDenied)
                                return 1;
                            else if (intShelved > intApproved && intShelved > intDenied)
                                return 10;
                            else if (intDenied > intApproved && intDenied > intShelved)
                                return -1;
                            else
                                return 1;
                        }
                        else
                            return 0;
                    }
                }
            }
        }
        public DataSet GetByStep(int _requestid, int _step, int _approval)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@step", _step);
            arParams[2] = new SqlParameter("@approval", _approval);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getProjectRequestStatusDetailStep", arParams);
        }
        public DataSet GetAllStep(int _requestid, int _step)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@step", _step);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getProjectRequestStatusDetailSteps", arParams);
        }
        public void DeleteRequest(int _requestid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteProjectRequestStatus", arParams);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteProjectRequestStatusDetail", arParams);
            DataSet dsRequest = Get(_requestid);
        }
        public DataSet Get(int _requestid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getProjectRequestStatus", arParams);
        }
        public int GetStep(int _requestid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            object o = SqlHelper.ExecuteScalar(dsn, CommandType.StoredProcedure, "pr_getProjectRequestStep", arParams);
            if (o == null)
                return 0;
            else
                return Int32.Parse(o.ToString());
        }
        public DataSet GetAwaiting(int _userid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@userid", _userid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getProjectRequestStatusUser", arParams);
        }
        public DataSet GetAwaiting()
        {
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getProjectRequestStatusAwaiting");
        }
        public DataSet GetAwaitingRequest(int _requestid, int _step)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@step", _step);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getProjectRequestStatusAwaitingRequest", arParams);
        }
        public DataSet GetDetail(int _requestid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getProjectRequestStatusDetails", arParams);
        }
        public DataSet Get(int _requestid, int _userid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@userid", _userid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getProjectRequestStatusDetail", arParams);
        }
        public void UpdateApproval(int _requestid, int _approvalbefore, int _approvalafter, int _step)
        {
            // Update an individual's approval - no matter if it's approved or not
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@approvalbefore", _approvalbefore);
            arParams[2] = new SqlParameter("@approvalafter", _approvalafter);
            arParams[3] = new SqlParameter("@step", _step);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateProjectRequestStatuss", arParams);
        }
    }
}
