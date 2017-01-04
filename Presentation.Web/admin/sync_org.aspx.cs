using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using NCC.ClearView.Application.Core;
using Microsoft.ApplicationBlocks.Data;
namespace NCC.ClearView.Presentation.Web
{
    public partial class sync_org : BasePage
    {
        private string orgDSN = ConfigurationManager.ConnectionStrings["orgDSN"].ConnectionString;
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected string strWorkflowBCC = ConfigurationManager.AppSettings["WorkflowBCC"];
        protected Users oUser;
        protected Applications oApplication;
        protected Variables oVariable;
        protected int intProfile;
        protected int intMode = 0;
        protected int intUserAdd = 0;
        protected int intUserManager = 0;
        protected int intApplicationAdd = 0;
        protected int intApplicationUpdated = 0;
        protected int intApplicationDeleted = 0;
        protected string strDefaultAppPages = "30;16;19;26;18;25;37;2;35;36;38;54;6;41;40;42;39;46;33;";
        protected bool boolCleanApps = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cookies["loginreferrer"].Value = "/admin/sync_org.aspx";
            Response.Cookies["loginreferrer"].Expires = DateTime.Now.AddDays(30);
            if (Request.Cookies["adminid"] != null && Request.Cookies["adminid"].Value != "")
                intProfile = Int32.Parse(Request.Cookies["adminid"].Value);
            else
                Response.Redirect("/admin/login.aspx");
            oUser = new Users(intProfile, dsn);
            oApplication = new Applications(intProfile, dsn);
            oVariable = new Variables(intEnvironment);
            btnSync.Attributes.Add("onclick", "ShowWait('" + divSync.ClientID + "','" + divHide.ClientID + "');");
            btnAppPages.Attributes.Add("onclick", "ShowWait('" + divSync.ClientID + "','" + divHide.ClientID + "');");
        }
        protected void btnSync_Click(Object Sender, EventArgs e)
        {
            intMode = radResults.SelectedIndex;
            DateTime datStart = DateTime.Now;
            if (intMode > -1)
            {
                DataSet ds;
                int intCounter = 0;
                if (intMode == 1 || intMode == 3 || intMode == 5)
                {
                    try
                    {
                        lblResults.Text = "";
                        ds = SqlHelper.ExecuteDataset(orgDSN, CommandType.Text, "SELECT EmpXID, Emp As EmployeeName, MgrXID, ManagerName, Department FROM ISOrgAll");
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            if (intMode < 4)
                            {
                                intCounter++;
                                lblResults.Text += intCounter.ToString() + ") ...Trying..." + dr["EmpXID"].ToString().Trim();
                            }
                            UpdateUser(dr["EmpXID"].ToString().Trim());
                        }
                        if (boolCleanApps == true)
                            CleanUpApps();
                        lblResults.Text += "<br/><br/><img src=\"" + oVariable.ImageURL() + "/images/check.gif\" border=\"0\" align=\"absmiddle\"/> <b>Done!</b>" + "<br/>";
                    }
                    catch { }
                }
                else
                {
                    lblResults.Text = "";
                    ds = SqlHelper.ExecuteDataset(orgDSN, CommandType.Text, "SELECT EmpXID, Emp As EmployeeName, MgrXID, ManagerName, Department FROM ISOrgAll");
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        if (intMode < 4)
                        {
                            intCounter++;
                            lblResults.Text += intCounter.ToString() + ") ...Trying..." + dr["EmpXID"].ToString().Trim();
                        }
                        UpdateUser(dr["EmpXID"].ToString().Trim());
                    }
                    if (boolCleanApps == true)
                        CleanUpApps();
                    lblResults.Text += "<br/><br/><img src=\"" + oVariable.ImageURL() + "/images/bigCheck.gif\" border=\"0\" align=\"absmiddle\"/> <b>Done!</b>" + "<br/>";
                }
                DateTime datEnd = DateTime.Now;
                TimeSpan oSpan = datEnd.Subtract(datStart);
                lblResults.Text += "<br/><br/><b>Total Time:</b> " + oSpan.Minutes.ToString() + "mins, " + oSpan.Seconds.ToString() + "." + oSpan.Milliseconds.ToString() + " secs";
                lblResults.Text += "<br/><br/><b>New Users:</b> " + intUserAdd.ToString();
                lblResults.Text += "<br/><b>Updated User Managers:</b> " + intUserManager.ToString();
                lblResults.Text += "<br/><b>New Applications:</b> " + intApplicationAdd.ToString();
                lblResults.Text += "<br/><b>Updated Applications:</b> " + intApplicationUpdated.ToString();
                lblResults.Text += "<br/><b>Deleted Applications:</b> " + intApplicationDeleted.ToString();
                lblResults.Text = "<p><img src=\"" + oVariable.ImageURL() + "/images/bigCheck.gif\" border=\"0\" align=\"absmiddle\"/> <b>ClearView sync has finished!</b></p><br/>" + lblResults.Text;
                Functions oFunction = new Functions(0, dsn, intEnvironment);
                string strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DEVELOPER_ALERT");
                if (intMode > 1)
                    oFunction.SendEmail("ClearView Org Chart Sync", strEMailIdsBCC, "", "", "ClearView Org Chart Sync", "<p>" + lblResults.Text + "</p>", true, false);
                else
                    oFunction.SendEmail("ClearView Org Chart Sync", strEMailIdsBCC, "", "", "ClearView Org Chart Sync", "<p>" + lblResults.Text + "</p>", false, false);
            }
            else
                lblResults.Text = "<font color=\"#990000\"><b>*** Please select a sync mode ***</b></font>";
        }
        private void UpdateUser(string _xid)
        {
            if (GetId(_xid) == 0)
            {
                DataSet ds = SqlHelper.ExecuteDataset(orgDSN, CommandType.Text, "SELECT EmpXID, Emp As EmployeeName, MgrXID, ManagerName, Department FROM ISOrgAll WHERE empxid = '" + _xid + "'");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string strXID = ds.Tables[0].Rows[0]["EmpXID"].ToString().Trim();
                    string strName = ds.Tables[0].Rows[0]["EmployeeName"].ToString().Trim();
                    string strFirst = strName.Substring(0, strName.IndexOf(","));
                    strName = strName.Substring(strName.IndexOf(",") + 1);
                    string strLast = strName.Trim();
                    string strManager = ds.Tables[0].Rows[0]["MgrXID"].ToString().Trim();
                    string strDepartment = ds.Tables[0].Rows[0]["Department"].ToString().Trim();
                    strDepartment = CheckDepartment(strDepartment);
                    int intManager = GetId(strManager);
                    if (intManager == 0 && strManager != "root")
                    {
                        if (intMode < 4)
                            lblResults.Text += "...Adding Manager..." + strManager;
                        UpdateUser(strManager);
                        intManager = GetId(strManager);
                    }
                    if (intMode > 1)
                        oUser.Add(strXID, "", strFirst.Replace("'", "''"), strLast.Replace("'", "''"), intManager, 0, 0, 0, "", 0, "", "", 0, 0, 0, 0, 1);
                    intUserAdd++;
                    int intUser = GetId(strXID);
                    int intGroup = AddGroup(strDepartment + " Bronze");
                    intGroup = AddGroup(strDepartment + " Green");
                    int intApplication = AddApplication(strDepartment);
                    if (intMode > 1)
                    {
                        SqlHelper.ExecuteNonQuery(dsn, CommandType.Text, "INSERT INTO cv_roles VALUES (" + intUser + "," + intGroup + ",getdate(),0)");
                        AddPermission(intApplication, intGroup);
                    }
                    if (intMode < 4)
                        lblResults.Text += "<br/><img src=\"" + oVariable.ImageURL() + "/images/check.gif\" border=\"0\" align=\"absmiddle\"/> Add User " + _xid + "<br/>";
                }
            }
            else
            {
                // Update Manager - if applicable
                DataSet dsOld = SqlHelper.ExecuteDataset(dsn, CommandType.Text, "SELECT m.* FROM cv_users u INNER JOIN cv_users m ON u.manager = m.userid WHERE u.xid = '" + _xid + "'");
                string strOldManager = "";
                if (dsOld.Tables[0].Rows.Count > 0)
                    strOldManager = dsOld.Tables[0].Rows[0]["xid"].ToString().Trim();
                DataSet dsNew = SqlHelper.ExecuteDataset(orgDSN, CommandType.Text, "SELECT EmpXID, Emp As EmployeeName, MgrXID, ManagerName, Department FROM ISOrgAll WHERE empxid = '" + _xid + "'");
                if (dsNew.Tables[0].Rows.Count > 0)
                {
                    string strNewManager = dsNew.Tables[0].Rows[0]["MgrXID"].ToString().Trim();
                    if (strOldManager.Trim().ToUpper() != strNewManager.Trim().ToUpper())
                    {
                        int intManager = GetId(strNewManager);
                        if (intManager == 0 && strNewManager != "root")
                        {
                            UpdateUser(strNewManager);
                            intManager = GetId(strNewManager);
                        }
                        if (intMode > 1)
                            SqlHelper.ExecuteNonQuery(dsn, CommandType.Text, "UPDATE cv_users SET manager = " + intManager.ToString() + " WHERE xid = '" + _xid + "'");
                        intUserManager++;
                        if (intMode < 4)
                            lblResults.Text += "<br/><img src=\"" + oVariable.ImageURL() + "/images/check.gif\" border=\"0\" align=\"absmiddle\"/> Update User Manager for " + _xid + "<br/>";
                    }
                    else
                    {
                        if (intMode < 4)
                            lblResults.Text += "...OK!<br/>";
                    }
                }
                else
                {
                    if (intMode < 4)
                        lblResults.Text += "<br/><img src=\"" + oVariable.ImageURL() + "/images/error.gif\" border=\"0\" align=\"absmiddle\"/> Could not find " + _xid + " in Org Chart<br/>";
                }
                if (intMode < 4)
                    lblResults.Text += "......Checking Applications for " + _xid;
                // Update Application - if applicable
                int intUser = GetId(_xid);
                DataSet dsNewRoles = SqlHelper.ExecuteDataset(orgDSN, CommandType.Text, "SELECT EmpXID, Emp As EmployeeName, MgrXID, ManagerName, Department FROM ISOrgAll WHERE empxid = '" + _xid + "'");
                string strApplication = "";
                if (dsNewRoles.Tables[0].Rows.Count > 0)
                    strApplication = dsNewRoles.Tables[0].Rows[0]["Department"].ToString().Trim();
                strApplication = CheckDepartment(strApplication);
                DataSet dsOldRoles = SqlHelper.ExecuteDataset(dsn, CommandType.Text, "select cv_applications.orgchart, cv_permissions.permissionid, cv_groups.groupid, cv_roles.roleid from cv_applications inner join cv_permissions inner join cv_groups inner join cv_roles on cv_groups.groupid = cv_roles.groupid and cv_roles.deleted = 0 on cv_permissions.groupid = cv_groups.groupid and cv_groups.deleted = 0 on cv_applications.applicationid = cv_permissions.applicationid and cv_permissions.deleted = 0 where cv_applications.deleted = 0 and cv_roles.userid = " + intUser.ToString());
                bool boolFound = false;
                int intNewApps = 0;
                foreach (DataRow drRole in dsOldRoles.Tables[0].Rows)
                {
                    if (strApplication.Trim().ToUpper() == drRole["orgchart"].ToString().Trim().ToUpper())
                        boolFound = true;
                }
                if (boolFound == false)
                {
                    // Did not find the new application, so add the new application
                    //    First remove the old apps
                    if (oUser.Get(intUser, "multiple_apps") != "1")
                    {
                        foreach (DataRow drRole in dsOldRoles.Tables[0].Rows)
                        {
                            SqlHelper.ExecuteNonQuery(dsn, CommandType.Text, "UPDATE cv_roles SET deleted = 1 WHERE roleid = " + drRole["roleid"].ToString());
                            if (intMode < 4)
                                lblResults.Text += "<br/><img src=\"" + oVariable.ImageURL() + "/images/error.gif\" border=\"0\" align=\"absmiddle\"/> Removed Application " + drRole["orgchart"].ToString() + " (multiple_apps disabled)";
                        }
                    }
                    //    Add new application
                    foreach (DataRow drRole in dsOldRoles.Tables[0].Rows)
                    {
                        intNewApps++;
                        intApplicationUpdated++;
                        int intGroup = AddGroup(strApplication + " Bronze");
                        intGroup = AddGroup(strApplication + " Green");
                        int intApplication = AddApplication(strApplication);
                        if (intMode > 1)
                        {
                            SqlHelper.ExecuteNonQuery(dsn, CommandType.Text, "INSERT INTO cv_roles VALUES (" + intUser + "," + intGroup + ",getdate(),0)");
                            AddPermission(intApplication, intGroup);
                        }
                        if (intMode < 4)
                            lblResults.Text += "<br/><img src=\"" + oVariable.ImageURL() + "/images/check.gif\" border=\"0\" align=\"absmiddle\"/> Add Application " + strApplication;
                    }
                }
                if (intMode < 4)
                {
                    lblResults.Text += ".........";
                    if (intNewApps > 0)
                        lblResults.Text += "added " + intNewApps.ToString() + " Applications for " + _xid;
                    else if (boolFound == true)
                        lblResults.Text += "OK!";
                    lblResults.Text += "<br/>";
                }
            }
        }
        private string CheckDepartment(string _department)
        {
            if (_department.ToUpper().IndexOf("FIELD SERVICES") > -1)
                _department = "Field Services";
            return _department;
        }
        protected int GetId(string _xid)
        {
            object o = SqlHelper.ExecuteScalar(dsn, CommandType.Text, "SELECT userid FROM cv_users WHERE xid = '" + _xid + "'");
            if (o == null)
                return 0;
            else
                return Int32.Parse(o.ToString());
        }
        protected int GetApplication(string _name)
        {
            object o = SqlHelper.ExecuteScalar(dsn, CommandType.Text, "SELECT applicationid FROM cv_applications WHERE name = '" + _name + "'");
            if (o == null)
                return 0;
            else
                return Int32.Parse(o.ToString());
        }
        protected int GetGroup(string _name)
        {
            object o = SqlHelper.ExecuteScalar(dsn, CommandType.Text, "SELECT groupid FROM cv_groups WHERE name = '" + _name + "'");
            if (o == null)
                return 0;
            else
                return Int32.Parse(o.ToString());
        }
        protected int AddApplication(string _name)
        {
            if (GetApplication(_name) == 0)
            {
                string _url = _name;
                while (_url.IndexOf(" ") > -1)
                    _url = _url.Replace(" ", "");
                while (_url.IndexOf("&") > -1)
                    _url = _url.Replace("&", "");
                if (intMode < 4)
                    lblResults.Text += "<br/><img src=\"" + oVariable.ImageURL() + "/images/check.gif\" border=\"0\" align=\"absmiddle\"/> Added Application " + _name + "<br/>";
                intApplicationAdd++;
                if (intMode > 1)
                    oApplication.Add(_name, _url, _name, _name, "", "", 0, 0, 0, 0, 0, 0, 0, 0, "", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1);
            }
            return GetApplication(_name);
        }
        protected int AddGroup(string _name)
        {
            if (GetGroup(_name) == 0)
            {
                if (intMode > 1)
                    SqlHelper.ExecuteNonQuery(dsn, CommandType.Text, "INSERT INTO cv_groups VALUES ('" + _name + "','" + _name + " (Automated)',1,getdate(),0)");
                if (intMode < 4)
                    lblResults.Text += "<br/><img src=\"" + oVariable.ImageURL() + "/images/check.gif\" border=\"0\" align=\"absmiddle\"/> Added Group " + _name + "<br/>";
            }
            return GetGroup(_name);
        }
        private void AddPermission(int _applicationid, int _groupid)
        {
            DataSet ds = SqlHelper.ExecuteDataset(dsn, CommandType.Text, "SELECT * FROM cv_permissions WHERE applicationid = " + _applicationid + " AND groupid = " + _groupid + " AND deleted = 0");
            if (ds.Tables[0].Rows.Count == 0)
                SqlHelper.ExecuteNonQuery(dsn, CommandType.Text, "INSERT INTO cv_permissions VALUES (" + _applicationid + "," + _groupid + ",1,getdate(),0)");
        }
        private void CleanUpApps()
        {
            DataSet ds = SqlHelper.ExecuteDataset(dsn, CommandType.Text, "SELECT * FROM cv_applications WHERE deleted = 0");
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                DataSet dsApps = SqlHelper.ExecuteDataset(dsn, CommandType.Text, "select cv_users.fname, cv_users.lname from cv_applications inner join cv_permissions inner join cv_groups inner join cv_roles inner join cv_users on cv_roles.userid = cv_users.userid and cv_users.deleted = 0 and cv_users.enabled = 1on cv_groups.groupid = cv_roles.groupid and cv_roles.deleted = 0 on cv_permissions.groupid = cv_groups.groupid and cv_groups.deleted = 0 on cv_applications.applicationid = cv_permissions.applicationid and cv_permissions.deleted = 0 where cv_applications.deleted = 0 and cv_applications.applicationid = " + dr["applicationid"].ToString());
                if (dsApps.Tables[0].Rows.Count == 0)
                {
                    intApplicationDeleted++;
                    if (intMode < 4)
                        lblResults.Text += "<br/><img src=\"" + oVariable.ImageURL() + "/images/error.gif\" border=\"0\" align=\"absmiddle\"/> Deleted Application " + dr["name"].ToString() + "<br/>";
                    // Delete the application, groups and permissions
                    DataSet dsPermissions = SqlHelper.ExecuteDataset(dsn, CommandType.Text, "select cv_permissions.permissionid from cv_applications inner join cv_permissions on cv_applications.applicationid = cv_permissions.applicationid and cv_permissions.deleted = 0 where cv_applications.deleted = 0 and cv_applications.applicationid = " + dr["applicationid"].ToString());
                    foreach (DataRow drTemp in dsPermissions.Tables[0].Rows)
                    {
                        if (intMode > 1)
                            SqlHelper.ExecuteNonQuery(dsn, CommandType.Text, "UPDATE cv_permissions SET deleted = 1 WHERE permissionid = " + drTemp["permissionid"].ToString());
                    }
                    DataSet dsGroups = SqlHelper.ExecuteDataset(dsn, CommandType.Text, "select cv_groups.groupid from cv_applications inner join cv_permissions inner join cv_groups on cv_permissions.groupid = cv_groups.groupid and cv_groups.deleted = 0 on cv_applications.applicationid = cv_permissions.applicationid and cv_permissions.deleted = 0 where cv_applications.deleted = 0 and cv_applications.applicationid = " + dr["applicationid"].ToString());
                    foreach (DataRow drTemp in dsGroups.Tables[0].Rows)
                    {
                        if (intMode > 1)
                            SqlHelper.ExecuteNonQuery(dsn, CommandType.Text, "UPDATE cv_groups SET deleted = 1 WHERE groupid = " + drTemp["groupid"].ToString());
                    }
                    DataSet dsRoles = SqlHelper.ExecuteDataset(dsn, CommandType.Text, "select cv_roles.roleid from cv_applications inner join cv_permissions inner join cv_groups inner join cv_roles on cv_groups.groupid = cv_roles.groupid and cv_roles.deleted = 0 on cv_permissions.groupid = cv_groups.groupid and cv_groups.deleted = 0 on cv_applications.applicationid = cv_permissions.applicationid and cv_permissions.deleted = 0 where cv_applications.deleted = 0 and cv_applications.applicationid = " + dr["applicationid"].ToString());
                    foreach (DataRow drTemp in dsRoles.Tables[0].Rows)
                    {
                        if (intMode > 1)
                            SqlHelper.ExecuteNonQuery(dsn, CommandType.Text, "UPDATE cv_roles SET deleted = 1 WHERE roleid = " + drTemp["roleid"].ToString());
                    }
                }
            }
        }
        protected void btnAppPages_Click(Object Sender, EventArgs e)
        {
            DataSet ds = SqlHelper.ExecuteDataset(dsn, CommandType.Text, "SELECT * FROM cv_applications WHERE deleted = 0");
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                int intApplication = Int32.Parse(dr["applicationid"].ToString());
                DataSet dsApps = SqlHelper.ExecuteDataset(dsn, CommandType.Text, "select cv_users.fname, cv_users.lname from cv_applications inner join cv_permissions inner join cv_groups inner join cv_roles inner join cv_users on cv_roles.userid = cv_users.userid and cv_users.deleted = 0 and cv_users.enabled = 1on cv_groups.groupid = cv_roles.groupid and cv_roles.deleted = 0 on cv_permissions.groupid = cv_groups.groupid and cv_groups.deleted = 0 on cv_applications.applicationid = cv_permissions.applicationid and cv_permissions.deleted = 0 where cv_applications.deleted = 0 and cv_applications.applicationid = " + intApplication.ToString());
                if (dsApps.Tables[0].Rows.Count > 0)
                {
                    string[] strAppPages;
                    char[] strSplit = { ';' };
                    strAppPages = strDefaultAppPages.Split(strSplit);
                    for (int ii = 0; ii < strAppPages.Length; ii++)
                    {
                        if (strAppPages[ii].Trim() != "")
                        {
                            int intPage = Int32.Parse(strAppPages[ii].Trim());
                            DataSet dsAppPage = SqlHelper.ExecuteDataset(dsn, CommandType.Text, "SELECT * FROM cv_app_pages WHERE applicationid = " + intApplication.ToString() + " AND pageid = " + intPage.ToString() + " AND deleted = 0");
                            if (dsAppPage.Tables[0].Rows.Count == 0)
                                SqlHelper.ExecuteNonQuery(dsn, CommandType.Text, "INSERT INTO cv_app_pages VALUES(" + intPage.ToString() + "," + intApplication.ToString() + ",getdate(),0)");
                        }
                    }
                }
            }
        }
    }
}
