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
using System.DirectoryServices;
using Microsoft.ApplicationBlocks.Data;

namespace NCC.ClearView.Presentation.Web
{
    public partial class sync_ad : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected string strWorkflowBCC = ConfigurationManager.AppSettings["WorkflowBCC"];
        protected AD oAD;
        protected Users oUser;
        protected Variables oVariable;
        protected int intProfile;
        protected int intMode = 0;
        protected int intUserAdd = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cookies["loginreferrer"].Value = "/admin/sync_ad.aspx";
            Response.Cookies["loginreferrer"].Expires = DateTime.Now.AddDays(30);
            if (Request.Cookies["adminid"] != null && Request.Cookies["adminid"].Value != "")
                intProfile = Int32.Parse(Request.Cookies["adminid"].Value);
            else
                Response.Redirect("/admin/login.aspx");
            oAD = new AD(intProfile, dsn, intEnvironment);
            oUser = new Users(intProfile, dsn);
            oVariable = new Variables(intEnvironment);
            btnSync.Attributes.Add("onclick", "ShowWait('" + divShow.ClientID + "','" + divHide.ClientID + "');");
        }
        protected void btnSync_Click(Object Sender, EventArgs e)
        {
            intMode = radResults.SelectedIndex;
            DateTime datStart = DateTime.Now;
            SearchResultCollection oResults;
            Variables oVariable = new Variables(intEnvironment);
            DirectoryEntry oEntry = new DirectoryEntry(oVariable.primaryDC(dsn), oVariable.ADUser(), oVariable.ADPassword());
            DirectorySearcher oSearcher = new DirectorySearcher(oEntry);
            oSearcher.PageSize = 100000;
            oSearcher.SizeLimit = 100000;
            if (intMode > -1)
            {
                int intCounter = 0;
                string[] letterArray = new string[] { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };
                string[] letterArray2 = new string[] { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };
                lblResults.Text = "";
                for (int ii = 0; ii < letterArray.Length; ii++)
                {
                    for (int jj = 0; jj < letterArray2.Length; jj++)
                    {
                        string strFilter = "(&(objectCategory=user)(sAMAccountName=" + txtSync.Text + letterArray[ii] + letterArray[jj] + "*))";
                        oSearcher.Filter = strFilter;
                        oResults = oSearcher.FindAll();
                        if (intMode == 1 || intMode == 3 || intMode == 5)
                        {
                            try
                            {
                                foreach (SearchResult oResult in oResults)
                                {
                                    if (oResult.Properties.Contains("extensionattribute10") == true || oResult.Properties.Contains("sAMAccountName") == true)
                                    {
                                        string strXid = "";
                                        if (oResult.Properties.Contains("extensionattribute10") == true)
                                            strXid = oResult.GetDirectoryEntry().Properties["extensionattribute10"].Value.ToString();
                                        else
                                            strXid = oResult.GetDirectoryEntry().Properties["sAMAccountName"].Value.ToString();
                                        if (intMode < 4)
                                        {
                                            intCounter++;
                                            lblResults.Text += intCounter.ToString() + ") ...Trying..." + strXid;
                                        }
                                        string strFName = "";
                                        if (oResult.Properties.Contains("givenname") == true)
                                            strFName = oResult.GetDirectoryEntry().Properties["givenname"].Value.ToString();
                                        string strLName = "";
                                        if (oResult.Properties.Contains("sn") == true)
                                            strLName = oResult.GetDirectoryEntry().Properties["sn"].Value.ToString();
                                        AddUser(strXid, strFName, strLName);
                                    }
                                }
                            }
                            catch { }
                        }
                        else
                        {
                            foreach (SearchResult oResult in oResults)
                            {
                                if (oResult.Properties.Contains("sAMAccountName") == true)
                                {
                                    string strXid = oResult.GetDirectoryEntry().Properties["sAMAccountName"].Value.ToString();
                                    if (intMode < 4)
                                    {
                                        intCounter++;
                                        lblResults.Text += intCounter.ToString() + ") ...Trying..." + strXid;
                                    }
                                    string strFName = "";
                                    if (oResult.Properties.Contains("givenname") == true)
                                        strFName = oResult.GetDirectoryEntry().Properties["givenname"].Value.ToString();
                                    string strLName = "";
                                    if (oResult.Properties.Contains("sn") == true)
                                        strLName = oResult.GetDirectoryEntry().Properties["sn"].Value.ToString();
                                    AddUser(strXid, strFName, strLName);
                                }
                            }
                        }
                    }
                }
                lblResults.Text += "<br/><br/><img src=\"" + oVariable.ImageURL() + "/images/check.gif\" border=\"0\" align=\"absmiddle\"/> <b>Done!</b>" + "<br/>";
                DateTime datEnd = DateTime.Now;
                TimeSpan oSpan = datEnd.Subtract(datStart);
                lblResults.Text += "<br/><br/><b>Total Time:</b> " + oSpan.Minutes.ToString() + "mins, " + oSpan.Seconds.ToString() + "." + oSpan.Milliseconds.ToString() + " secs";
                lblResults.Text += "<br/><br/><b>New Users:</b> " + intUserAdd.ToString();
                lblResults.Text = "<p><img src=\"" + oVariable.ImageURL() + "/images/bigCheck.gif\" border=\"0\" align=\"absmiddle\"/> <b>ClearView sync has finished!</b></p><br/>" + lblResults.Text;
                Functions oFunction = new Functions(0, dsn, intEnvironment);
                string strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DEVELOPER_ALERT");
                if (intMode > 1)
                    oFunction.SendEmail("ClearView Active Directory Sync", strEMailIdsBCC, "", "", "ClearView Active Directory Sync", "<p>" + lblResults.Text + "</p>", true, false);
                else
                    oFunction.SendEmail("ClearView Active Directory Sync", strEMailIdsBCC, "", "", "ClearView Active Directory Sync", "<p>" + lblResults.Text + "</p>", false, false);
            }
            else
            {
                string strFilter = "(&(objectCategory=user)(sAMAccountName=" + txtSync.Text + "*))";
                oSearcher.Filter = strFilter;
                oResults = oSearcher.FindAll();
                lblResults.Text = "<p><font color=\"#990000\"><b>*** Please select a sync mode ***</b></font></p><p># of Results: " + oResults.Count + "</p>";
            }
        }
        private void AddUser(string _xid, string _fname, string _lname)
        {
            int intUser = GetId(_xid);
            if (intUser == 0)
            {
                if (intMode > 1)
                    SqlHelper.ExecuteNonQuery(dsn, CommandType.Text, "INSERT INTO cv_users VALUES ('" + _xid + "','" + _fname.Replace("'", "''") + "','" + _lname.Replace("'", "''") + "',0,0,0,0,'',0,'','',0,1,getdate(),0)");
                intUser = GetId(_xid);
                intUserAdd++;
                int intGroup = GetGroup();
                int intApplication = GetApplication();
                if (intMode > 1)
                {
                    SqlHelper.ExecuteNonQuery(dsn, CommandType.Text, "INSERT INTO cv_roles VALUES (" + intUser + "," + intGroup + ",getdate(),0)");
                    object o = SqlHelper.ExecuteScalar(dsn, CommandType.Text, "SELECT permissionid FROM cv_permissions WHERE applicationid = " + intApplication + " AND groupid = " + intGroup + " AND deleted = 0");
                    if (o == null)
                        SqlHelper.ExecuteNonQuery(dsn, CommandType.Text, "INSERT INTO cv_permissions VALUES (" + intApplication + "," + intGroup + ",1,getdate(),0)");
                }
                if (intMode < 4)
                    lblResults.Text += "<br/><img src=\"" + oVariable.ImageURL() + "/images/check.gif\" border=\"0\" align=\"absmiddle\"/> Add User " + _xid + "<br/>";
            }
        }
        protected int GetId(string _xid)
        {
            object o = SqlHelper.ExecuteScalar(dsn, CommandType.Text, "SELECT userid FROM cv_users WHERE xid = '" + _xid + "'");
            if (o == null)
                return 0;
            else
                return Int32.Parse(o.ToString());
        }
        protected int GetApplication()
        {
            object o = SqlHelper.ExecuteScalar(dsn, CommandType.Text, "SELECT applicationid FROM cv_applications WHERE name = 'ClearView Users'");
            if (o == null)
                return 0;
            else
                return Int32.Parse(o.ToString());
        }
        protected int GetGroup()
        {
            object o = SqlHelper.ExecuteScalar(dsn, CommandType.Text, "SELECT groupid FROM cv_groups WHERE name = 'ClearView Users'");
            if (o == null)
                return 0;
            else
                return Int32.Parse(o.ToString());
        }
    }
}
