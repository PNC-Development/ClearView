using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using System.IO;
using System.Data.SqlTypes;
using System.Web.UI.WebControls;
using System.Text;

namespace NCC.ClearView.Application.Core
{
    public enum EnhancementStatus
    {
        AwaitingLongDocument = -10,
        AwaitingApproval = -5,
        Cancelled = -2,
        Denied = -1,
        Duplicate = 0,
        UnderReview = 1,
        InDevelopment = 2,
        Completed = 3,
        OnHold = 5,
        AwaitingResponse = 7
    }
    public class Enhancements
	{
		private string dsn = "";
		private int user = 0;
		private SqlParameter[] arParams;
        public Enhancements(int _user, string _dsn)
		{
			user = _user;
			dsn = _dsn;
		}
        public DataSet Get(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getEnhancement", arParams);
        }
        public string Get(int _id, string _column)
        {
            DataSet ds = Get(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public DataSet GetRequest(int _requestid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getEnhancementRequest", arParams);
        }
        public DataSet Gets()
        {
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getEnhancements");
        }
        public int Add(string _title, string _description, int _users, string _url, FileUpload _file, int _environment, int _release1, int _release2, int _userid, int _rrid)
        {
            string _screenshot = "";
            // Upload File
            if (_file.FileName != "" && _file.PostedFile != null)
            {
                Variables _variable = new Variables(_environment);
                string strExtension = _file.FileName;
                string strType = strExtension.Substring(0, 3);
                strExtension = strExtension.Substring(strExtension.LastIndexOf("."));
                string strFile = DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString() + "_" + _userid.ToString() + strExtension;
                _screenshot = _variable.UploadsFolder() + strFile;
                string strPath = _variable.UploadsFolder() + strFile;
                _file.PostedFile.SaveAs(strPath);
            }
            // Save Enhancement
            arParams = new SqlParameter[10];
            arParams[0] = new SqlParameter("@title", _title);
            arParams[1] = new SqlParameter("@description", _description);
            arParams[2] = new SqlParameter("@users", _users);
            arParams[3] = new SqlParameter("@url", _url);
            arParams[4] = new SqlParameter("@screenshot", _screenshot);
            arParams[5] = new SqlParameter("@release1", _release1);
            arParams[6] = new SqlParameter("@release2", _release2);
            arParams[7] = new SqlParameter("@userid", _userid);
            arParams[8] = new SqlParameter("@rrid", _rrid);
            arParams[9] = new SqlParameter("@id", SqlDbType.Int);
            arParams[9].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addEnhancement", arParams);
            return Int32.Parse(arParams[9].Value.ToString());
        }
        public void Update(int _id, string _title, string _description, int _users, string _url, string _screenshot, FileUpload _file, int _environment, int _release1, int _release2, int _userid)
        {
            // Upload File
            if (_file.FileName != "" && _file.PostedFile != null)
            {
                Variables _variable = new Variables(_environment);
                string strExtension = _file.FileName;
                string strType = strExtension.Substring(0, 3);
                strExtension = strExtension.Substring(strExtension.LastIndexOf("."));
                string strFile = DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString() + "_" + _userid.ToString() + strExtension;
                _screenshot = _variable.UploadsFolder() + strFile;
                string strPath = _variable.UploadsFolder() + strFile;
                _file.PostedFile.SaveAs(strPath);
            }
            // Update Enhancement
            arParams = new SqlParameter[8];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@title", _title);
            arParams[2] = new SqlParameter("@description", _description);
            arParams[3] = new SqlParameter("@users", _users);
            arParams[4] = new SqlParameter("@url", _url);
            arParams[5] = new SqlParameter("@screenshot", _screenshot);
            arParams[6] = new SqlParameter("@release1", _release1);
            arParams[7] = new SqlParameter("@release2", _release2);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateEnhancement", arParams);
        }
        public void UpdateModuleID(int _rrid, int _moduleid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@rrid", _rrid);
            arParams[1] = new SqlParameter("@moduleid", _moduleid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateEnhancementModuleID", arParams);
        }
        public void DeleteScreenshot(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteEnhancementScreenshot", arParams);
        }
        public void Update(int _id, int _versionid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@versionid", _versionid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateEnhancementComplete", arParams);
        }
        public void UpdateStatus(int _id, int _status)
        {
            int intRR = 0;
            if (Int32.TryParse(Get(_id, "rrid"), out intRR) == true)
            {
                ResourceRequest oResourceRequest = new ResourceRequest(user, dsn);
                oResourceRequest.UpdateStatusOverall(intRR, _status);
            }
        }
        public void Delete(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteEnhancement", arParams);
        }

        
        
        
        // ********************************************************************************************************
        // ********************************************************************************************************
        // ************       VERSIONS
        // ********************************************************************************************************
        // ********************************************************************************************************
        public void AddVersion(string _name, string _release, string _cutoff, int _available, string _compiled, int _enabled)
        {
            arParams = new SqlParameter[6];
            arParams[0] = new SqlParameter("@name", _name);
            arParams[1] = new SqlParameter("@release", (_release == "" ? SqlDateTime.Null : DateTime.Parse(_release)));
            arParams[2] = new SqlParameter("@cutoff", (_cutoff == "" ? SqlDateTime.Null : DateTime.Parse(_cutoff)));
            arParams[3] = new SqlParameter("@available", _available);
            arParams[4] = new SqlParameter("@compiled", (_compiled == "" ? SqlDateTime.Null : DateTime.Parse(_compiled)));
            arParams[5] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addEnhancementVersion", arParams);
        }
        public void UpdateVersion(int _id, string _name, string _release, string _cutoff, int _available, string _compiled, int _enabled)
        {
            arParams = new SqlParameter[7];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@name", _name);
            arParams[2] = new SqlParameter("@release", (_release == "" ? SqlDateTime.Null : DateTime.Parse(_release)));
            arParams[3] = new SqlParameter("@cutoff", (_cutoff == "" ? SqlDateTime.Null : DateTime.Parse(_cutoff)));
            arParams[4] = new SqlParameter("@available", _available);
            arParams[5] = new SqlParameter("@compiled", (_compiled == "" ? SqlDateTime.Null : DateTime.Parse(_compiled)));
            arParams[6] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateEnhancementVersion", arParams);
        }
        public void EnableVersion(int _id, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateEnhancementVersionEnabled", arParams);
        }
        public void DeleteVersion(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteEnhancementVersion", arParams);
        }
        public DataSet GetVersion(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getEnhancementVersion", arParams);
        }
        public string GetVersion(int _id, string _column)
        {
            DataSet ds = GetVersion(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public DataSet GetVersions(int _enabled)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getEnhancementVersions", arParams);
        }
        public DataSet GetVersions()
        {
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getEnhancementVersionsAvailable");
        }
        public string GetVersion()
        {
            object o = SqlHelper.ExecuteScalar(dsn, CommandType.StoredProcedure, "pr_getEnhancementVersionCurrent");
            if (o == null)
                return "0.0";
            else
                return o.ToString();
        }


        // ********************************************************************************************************
        // ********************************************************************************************************
        // ************       MODULES
        // ********************************************************************************************************
        // ********************************************************************************************************
        public void AddModule(string _name, int _display, int _enabled)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@name", _name);
            arParams[1] = new SqlParameter("@display", _display);
            arParams[2] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addEnhancementModule", arParams);
        }
        public void UpdateModule(int _id, string _name, int _enabled)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@name", _name);
            arParams[2] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateEnhancementModule", arParams);
        }
        public void UpdateModuleOrder(int _id, int _display)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@display", _display);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateEnhancementModuleOrder", arParams);
        }
        public void EnableModule(int _id, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateEnhancementModuleEnabled", arParams);
        }
        public void DeleteModule(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteEnhancementModule", arParams);
        }
        public DataSet GetModule(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getEnhancementModule", arParams);
        }
        public string GetModule(int _id, string _column)
        {
            DataSet ds = GetModule(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public DataSet GetModules(int _enabled)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getEnhancementModules", arParams);
        }





        // ********************************************************************************************************
        // ********************************************************************************************************
        // ************       APPROVAL GROUPS
        // ********************************************************************************************************
        // ********************************************************************************************************
        public void AddApprovalGroup(string _name, int _any, int _enabled)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@name", _name);
            arParams[1] = new SqlParameter("@any", _any);
            arParams[2] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addEnhancementApprovalGroup", arParams);
        }
        public void UpdateApprovalGroup(int _id, string _name, int _any, int _enabled)
        {
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@name", _name);
            arParams[2] = new SqlParameter("@any", _any);
            arParams[3] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateEnhancementApprovalGroup", arParams);
        }
        public void EnableApprovalGroup(int _id, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateEnhancementApprovalGroupEnabled", arParams);
        }
        public void DeleteApprovalGroup(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteEnhancementApprovalGroup", arParams);
        }
        public DataSet GetApprovalGroup(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getEnhancementApprovalGroup", arParams);
        }
        public string GetApprovalGroup(int _id, string _column)
        {
            DataSet ds = GetApprovalGroup(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public DataSet GetApprovalGroups(int _enabled)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getEnhancementApprovalGroups", arParams);
        }

        public void AddApprovalGroupUser(int _groupid, int _userid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@groupid", _groupid);
            arParams[1] = new SqlParameter("@userid", _userid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addEnhancementApprovalGroupUser", arParams);
        }
        public void DeleteApprovalGroupUser(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteEnhancementApprovalGroupUser", arParams);
        }
        public DataSet GetApprovalGroupUsers(int _groupid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@groupid", _groupid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getEnhancementApprovalGroupUsers", arParams);
        }

        public void AddApproval(int _enhancementid, int _step, int _groupid)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@enhancementid", _enhancementid);
            arParams[1] = new SqlParameter("@step", _step);
            arParams[2] = new SqlParameter("@groupid", _groupid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addEnhancementApproval", arParams);
        }
        public void DeleteApproval(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteEnhancementApproval", arParams);
        }
        public DataSet GetApprovals(int _enhancementid, int _step)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@enhancementid", _enhancementid);
            arParams[1] = new SqlParameter("@step", _step);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getEnhancementApprovals", arParams);
        }

        public DataSet GetApprovalResults(int _enhancementid, int _step)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@enhancementid", _enhancementid);
            arParams[1] = new SqlParameter("@step", _step);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getEnhancementApprovalResults", arParams);
        }

        



        // ********************************************************************************************************
        // ********************************************************************************************************
        // ************       DOCUMENTS
        // ********************************************************************************************************
        // ********************************************************************************************************
        public DataSet GetDocuments(int _enhancementid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@enhancementid", _enhancementid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getEnhancementDocuments", arParams);
        }
        public void AddDocument(int _enhancementid, string _path, int _days, int _versionid)
        {
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@enhancementid", _enhancementid);
            arParams[1] = new SqlParameter("@path", _path);
            arParams[2] = new SqlParameter("@days", _days);
            arParams[3] = new SqlParameter("@versionid", _versionid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addEnhancementDocument", arParams);
        }
        public void DeleteDocument(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteEnhancementDocument", arParams);
        }


        // ********************************************************************************************************
        // ********************************************************************************************************
        // ************       LOGGING
        // ********************************************************************************************************
        // ********************************************************************************************************
        public DataSet GetLog(int _enhancementid, int _step)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@enhancementid", _enhancementid);
            arParams[1] = new SqlParameter("@step", _step);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getEnhancementLog", arParams);
        }
        public void AddLog(int _enhancementid, int _step, string _approval, int _userid, string _approved)
        {
            arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@enhancementid", _enhancementid);
            arParams[1] = new SqlParameter("@step", _step);
            arParams[2] = new SqlParameter("@approval", _approval);
            arParams[3] = new SqlParameter("@userid", _userid);
            arParams[4] = new SqlParameter("@approved", (_approved == "" ? SqlDateTime.Null : DateTime.Parse(_approved)));
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addEnhancementLog", arParams);
        }
        public void UpdateLog(int _enhancementid, int _step, string _approved, string _rejected, string _comments)
        {
            arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@enhancementid", _enhancementid);
            arParams[1] = new SqlParameter("@step", _step);
            arParams[2] = new SqlParameter("@approved", (_approved == "" ? SqlDateTime.Null : DateTime.Parse(_approved)));
            arParams[3] = new SqlParameter("@rejected", (_rejected == "" ? SqlDateTime.Null : DateTime.Parse(_rejected)));
            arParams[4] = new SqlParameter("@comments", _comments);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateEnhancementLog", arParams);

            if (string.IsNullOrEmpty(_rejected) == false)
            {
                UpdateStep(_enhancementid, _step, "", _rejected, "");
                UpdateStatus(_enhancementid, (int)EnhancementStatus.Denied);
            }
            if (string.IsNullOrEmpty(_approved) == false)
            {
                // Check to make sure all other approvals are approved
                bool boolApproved = true;
                DataSet dsLog = GetLog(_enhancementid, _step);
                foreach (DataRow drLog in dsLog.Tables[0].Rows)
                {
                    if (string.IsNullOrEmpty(drLog["approved"].ToString()))
                    {
                        boolApproved = false;
                        break;
                    }
                }
                if (boolApproved == true)
                {
                    UpdateStep(_enhancementid, _step, _approved, "", "");
                    UpdateStatus(_enhancementid, (int)EnhancementStatus.InDevelopment);
                }
            }
        }
        public string LoadLog(int _id, Repeater _rpt, Label _lbl)
        {
            Users oUser = new Users(user, dsn);
            _rpt.DataSource = GetLog(_id, 0);
            _rpt.DataBind();
            _lbl.Visible = (_rpt.Items.Count == 0);
            foreach (RepeaterItem ri in _rpt.Items)
            {
                Image imgApproval = (Image)ri.FindControl("imgApproval");
                Label lblApproved = (Label)ri.FindControl("lblApproved");
                Label lblRejected = (Label)ri.FindControl("lblRejected");
                Label lblComments = (Label)ri.FindControl("lblComments");
                Label lblStatus = (Label)ri.FindControl("lblStatus");
                int intStep = Int32.Parse(lblStatus.Text);
                Label lblUser = (Label)ri.FindControl("lblUser");
                int intUser = Int32.Parse(lblUser.Text);
                if (intStep == 0 || lblApproved.Text != "" || intUser == 0)
                {
                    imgApproval.ImageUrl = "/images/approved.gif";
                    if (lblApproved.Text != "" && intUser > 0)
                        lblStatus.Text = "Approved";
                    else
                        lblStatus.Text = "Complete";
                }
                else if (lblRejected.Text != "")
                {
                    imgApproval.ImageUrl = "/images/cancel.gif";
                    lblStatus.Text = "Denied";
                }
                else
                {
                    imgApproval.ImageUrl = "/images/active.gif";
                    lblStatus.Text = "Awaiting Approval";
                }
                if (lblComments.Text != "")
                    lblComments.Text = "<b>" + oUser.GetFullName(intUser) + "</b> added the following comments...<br/>" + lblComments.Text;
                if (intUser > 0)
                    lblUser.Text = "<a href=\"javascript:void(0);\" onclick=\"OpenWindow('PROFILE','?userid=" + intUser.ToString() + "');\">" + oUser.GetFullName(intUser) + "</a>";
                else
                    lblUser.Text = "ClearView Administrator";
            }
            return _rpt.Items.Count.ToString();
        }

        // ********************************************************************************************************
        // ********************************************************************************************************
        // ************       STEPS
        // ********************************************************************************************************
        // ********************************************************************************************************
        public void AddStep(int _enhancementid, int _step, string _completed, string _approved)
        {
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@enhancementid", _enhancementid);
            arParams[1] = new SqlParameter("@step", _step);
            arParams[2] = new SqlParameter("@completed", (_completed == "" ? SqlDateTime.Null : DateTime.Parse(_completed)));
            arParams[3] = new SqlParameter("@approved", (_approved == "" ? SqlDateTime.Null : DateTime.Parse(_approved)));
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addEnhancementStep", arParams);
        }
        public DataSet GetSteps(int _enhancementid, int _step, int _done)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@enhancementid", _enhancementid);
            arParams[1] = new SqlParameter("@step", _step);
            arParams[2] = new SqlParameter("@done", _done);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getEnhancementSteps", arParams);
        }
        public void UpdateStep(int _enhancementid, int _step, string _approved, string _rejected, string _reopened)
        {
            arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@enhancementid", _enhancementid);
            arParams[1] = new SqlParameter("@step", _step);
            arParams[2] = new SqlParameter("@approved", (_approved == "" ? SqlDateTime.Null : DateTime.Parse(_approved)));
            arParams[3] = new SqlParameter("@rejected", (_rejected == "" ? SqlDateTime.Null : DateTime.Parse(_rejected)));
            arParams[4] = new SqlParameter("@reopened", (_reopened == "" ? SqlDateTime.Null : DateTime.Parse(_reopened)));
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateEnhancementStep", arParams);
        }








        // ********************************************************************************************************
        // ********************************************************************************************************
        // ************       MESSAGES
        // ********************************************************************************************************
        // ********************************************************************************************************
        public void AddMessage(int _enhancementid, string _message, string _path, int _userid, int _admin)
        {
            arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@enhancementid", _enhancementid);
            arParams[1] = new SqlParameter("@message", _message);
            arParams[2] = new SqlParameter("@path", _path);
            arParams[3] = new SqlParameter("@userid", _userid);
            arParams[4] = new SqlParameter("@admin", _admin);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addEnhancementMessage", arParams);
        }

        public DataSet GetMessages(int _enhancementid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@enhancementid", _enhancementid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getEnhancementMessages", arParams);
        }

        public string GetMessages(int _enhancementid, bool _use_other, string _admin_color)
        {
            Users oUser = new Users(0, dsn);
            Functions oFunction = new Functions(0, dsn, 0);
            Applications oApplication = new Applications(0, dsn);
            StringBuilder sbMessages = new StringBuilder();
            bool boolOther = false;
            DataSet ds = GetMessages(_enhancementid);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (sbMessages.ToString() != "")
                {
                    sbMessages.Append("<tr><td colspan=\"2\">&nbsp;</td></tr>");
                }
                boolOther = !boolOther;
                string strTR = "";
                if (dr["admin"].ToString() == "1")
                {
                    if (_use_other == true)
                        strTR = "<tr" + (boolOther ? " bgcolor=\"#F6F6F6\"" : "") + ">";
                    else
                        strTR = "<tr bgcolor=\"" + _admin_color + "\">";
                    sbMessages.Append(strTR);
                    sbMessages.Append("<td valign=\"top\" rowspan=\"2\"><img src=\"/images/administrator.gif\" align=\"left\" vspace=\"3\" border=\"0\" width=\"90\" height=\"90\" style=\"border:solid 1px #CCCCCC\"/></td>");
                    sbMessages.Append("<td valign=\"top\" class=\"default\" width=\"100%\" height=\"1\"><b>ClearView Administrator</b> replied on ");
                    sbMessages.Append(dr["created"].ToString());
                    sbMessages.Append(":<br/><br/>");
                    sbMessages.Append(oFunction.FormatText(dr["message"].ToString()));
                    sbMessages.Append("</td>");
                    sbMessages.Append("</tr>");
                }
                else
                {
                    if (_use_other == true)
                        strTR = "<tr" + (boolOther ? " bgcolor=\"#F6F6F6\"" : "") + ">";
                    else
                        strTR = "<tr>";
                    sbMessages.Append(strTR);
                    sbMessages.Append("<td valign=\"top\" rowspan=\"2\"><img src=\"/frame/picture.aspx?xid=");
                    sbMessages.Append(oUser.GetName(Int32.Parse(dr["userid"].ToString())));
                    sbMessages.Append("\" align=\"left\" vspace=\"3\" border=\"0\" width=\"90\" height=\"90\" style=\"border:solid 1px #CCCCCC\"/></td>");
                    sbMessages.Append("<td valign=\"top\" class=\"default\" width=\"100%\" height=\"1\"><span class=\"bold\">");
                    sbMessages.Append(oUser.GetFullName(Int32.Parse(dr["userid"].ToString())));
                    sbMessages.Append("</span> replied on ");
                    sbMessages.Append(dr["created"].ToString());
                    sbMessages.Append(":<br/><br/>");
                    sbMessages.Append(oFunction.FormatText(dr["message"].ToString()));
                    sbMessages.Append("</td>");
                    sbMessages.Append("</tr>");
                }
                if (dr["path"].ToString() != "")
                {
                    string strFile = dr["path"].ToString();
                    //strFile = strFile.Substring(strFile.LastIndexOf("/") + 1);
                    strFile = strFile.Substring(strFile.LastIndexOf("\\") + 1);
                    sbMessages.Append(strTR);
                    sbMessages.Append("<td valign=\"bottom\" style=\"border-top:dashed 1px #CCCCCC\"><img src=\"/images/file.gif\" align=\"absmiddle\" border=\"0\"/> <a href=\"");
                    sbMessages.Append(dr["path"].ToString());
                    sbMessages.Append("\" target=\"_blank\">");
                    sbMessages.Append(strFile);
                    sbMessages.Append("</a></td></tr>");
                }
                else
                {
                    sbMessages.Append(strTR);
                    sbMessages.Append("<td></td></tr>");
                }
                sbMessages.Append("<tr><td>&nbsp;</td></tr>");
            }

            sbMessages.Insert(0, "<table width=\"100%\" cellpadding=\"5\" cellspacing=\"0\" border=\"0\" class=\"default\">");
            sbMessages.Append("</table>");

            return sbMessages.ToString();
        }


        public string GetBody(int _id, int _environment)
        {
            int intRequest = 0;
            if (Int32.TryParse(Get(_id, "requestid"), out intRequest) == true)
                return GetBodyRequest(intRequest, _environment);
            else
                return "ERROR: Request ID Not Found";
        }
        public string GetBodyRequest(int _requestid, int _environment)
        {
            Users oUser = new Users(user, dsn);
            Variables oVariable = new Variables(_environment);
            StatusLevels oStatusLevel = new StatusLevels();
            Functions oFunction = new Functions(user, dsn, _environment);
            StringBuilder sbBody = new StringBuilder();
            string strSpacerRow = "<tr><td colspan=\"3\"><img src=\"" + oVariable.ImageURL() + "/images/spacer.gif\" border=\"0\" width=\"1\" height=\"7\" /></td></tr>";
            if (_requestid > 0)
            {
                DataSet dsEnhancement = GetRequest(_requestid);
                if (dsEnhancement.Tables[0].Rows.Count > 0)
                {
                    int _id = Int32.Parse(dsEnhancement.Tables[0].Rows[0]["id"].ToString());
                    sbBody.Append("<tr><td nowrap><b>Title:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                    sbBody.Append(dsEnhancement.Tables[0].Rows[0]["title"].ToString());
                    sbBody.Append("</td></tr>");
                    sbBody.Append(strSpacerRow);
                    sbBody.Append("<tr><td nowrap valign=\"top\"><b>Description:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                    sbBody.Append(oFunction.FormatText(dsEnhancement.Tables[0].Rows[0]["description"].ToString()));
                    sbBody.Append("</td></tr>");
                    sbBody.Append(strSpacerRow);
                    sbBody.Append("<tr><td nowrap><b>Number of Users:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                    sbBody.Append(dsEnhancement.Tables[0].Rows[0]["users"].ToString());
                    sbBody.Append("</td></tr>");
                    sbBody.Append(strSpacerRow);
                    string strURL = dsEnhancement.Tables[0].Rows[0]["url"].ToString();
                    sbBody.Append("<tr><td nowrap><b>URL:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                    sbBody.Append(strURL == "" ? "None" : "<a href=\"" + strURL + "\" target=\"_blank\">" + strURL + "</a>");
                    sbBody.Append("</td></tr>");
                    sbBody.Append(strSpacerRow);
                    string strScreenshot = dsEnhancement.Tables[0].Rows[0]["screenshot"].ToString();
                    sbBody.Append("<tr><td nowrap><b>Screenshot / Attachment:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                    sbBody.Append(strScreenshot == "" ? "None" : "<a href=\"" + strScreenshot + "\" target=\"_blank\">" + strScreenshot + "</a>");
                    sbBody.Append("</td></tr>");
                    sbBody.Append(strSpacerRow);
                    sbBody.Append("<tr><td nowrap><b>Preferred Release Date:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                    sbBody.Append(DateTime.Parse(GetVersion(Int32.Parse(dsEnhancement.Tables[0].Rows[0]["release1"].ToString()), "release")).ToShortDateString());
                    sbBody.Append("</td></tr>");
                    sbBody.Append(strSpacerRow);
                    sbBody.Append("<tr><td nowrap><b>Alternative Release Date:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                    sbBody.Append(DateTime.Parse(GetVersion(Int32.Parse(dsEnhancement.Tables[0].Rows[0]["release2"].ToString()), "release")).ToShortDateString());
                    sbBody.Append("</td></tr>");
                    sbBody.Append(strSpacerRow);
                    sbBody.Append("<tr><td nowrap><b>Requested By:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                    sbBody.Append(oUser.GetFullName(Int32.Parse(dsEnhancement.Tables[0].Rows[0]["userid"].ToString())));
                    sbBody.Append("</td></tr>");
                    sbBody.Append(strSpacerRow);
                    sbBody.Append("<tr><td nowrap><b>Requested On:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                    sbBody.Append(DateTime.Parse(dsEnhancement.Tables[0].Rows[0]["created"].ToString()).ToShortDateString());
                    sbBody.Append("</td></tr>");
                    sbBody.Append(strSpacerRow);
                    sbBody.Append("<tr><td nowrap bgcolor=\"#C1FFC1\" style=\"padding:3px\"><b>Status:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                    sbBody.Append(Status(_id));
                    sbBody.Append("</td></tr>");
                }
            }
            if (sbBody.ToString() != "")
            {
                sbBody.Insert(0, "<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"" + oVariable.DefaultFontStyle() + "\">");
                sbBody.Append("</table>");
            }
            return sbBody.ToString();
        }
        public void AddVersions(RadioButtonList _list)
        {
            DataSet dsReleases = GetVersions();
            foreach (DataRow drRelease in dsReleases.Tables[0].Rows)
            {
                ListItem lstRelease = new ListItem(DateTime.Parse(drRelease["release"].ToString()).ToShortDateString(), drRelease["id"].ToString());
                if (DateTime.Parse(drRelease["cutoff"].ToString()) < DateTime.Now)
                {
                    lstRelease.Text += "&nbsp;(Closed)&nbsp;";
                    lstRelease.Enabled = false;
                }
                else if (drRelease["available"].ToString() != "1")
                {
                    lstRelease.Text += "&nbsp;(Full)&nbsp;";
                    lstRelease.Enabled = false;
                }

                _list.Items.Add(lstRelease);
            }
        }
        public string Status(int _id)
        {
            string strStatus = "<span class=\"denied\">Unknown</span>";

            int intStatus = 0;
            if (Int32.TryParse(Get(_id, "rrid"), out intStatus) == true)
            {
                ResourceRequest oResourceRequest = new ResourceRequest(user, dsn);
                if (Int32.TryParse(oResourceRequest.Get(intStatus, "status"), out intStatus) == true)
                {
                    // Before assignment status
                    switch (intStatus)
                    {
                        case (int)EnhancementStatus.AwaitingLongDocument:
                            strStatus = "<span class=\"default\">Awaiting Client Requirements Document</span>";
                            break;
                        case (int)EnhancementStatus.AwaitingApproval:
                            strStatus = "<span class=\"default\">Awaiting Approval</span>";
                            break;
                        case (int)EnhancementStatus.Cancelled:
                            strStatus = "<span class=\"denied\">Cancelled</span>";
                            break;
                        case (int)EnhancementStatus.Denied:
                            strStatus = "<span class=\"denied\">Denied</span>";
                            break;
                        case (int)EnhancementStatus.Duplicate:
                            strStatus = "<span class=\"pending\">Duplicate</span>";
                            break;
                        case (int)EnhancementStatus.UnderReview:
                            strStatus = "<span class=\"pending\">Under Review</span>";
                            break;
                        case (int)EnhancementStatus.InDevelopment:
                            strStatus = "<span class=\"approved\">In Development</span>";
                            break;
                        case (int)EnhancementStatus.Completed:
                            strStatus = "<span class=\"default\">Completed</span>";
                            break;
                        case (int)EnhancementStatus.OnHold:
                            strStatus = "<span class=\"shelved\">On Hold</span>";
                            break;
                        case (int)EnhancementStatus.AwaitingResponse:
                            strStatus = "<span class=\"pending\">Awaiting Client Response</span>";
                            break;
                    }
                }
            }
            return strStatus;
        }
    }
}
