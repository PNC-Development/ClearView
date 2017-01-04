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
using System.Text;

namespace NCC.ClearView.Presentation.Web
{
    public partial class cp_account_modify : System.Web.UI.UserControl
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intAssignPage = Int32.Parse(ConfigurationManager.AppSettings["AssignWorkload"]);
        protected int intResourceRequestApprove = Int32.Parse(ConfigurationManager.AppSettings["ResourceRequestApprove"]);
        protected int intProfile = 0;
        protected string strDone = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            RequestItems oRequestItem = new RequestItems(intProfile, dsn);
            RequestFields oRequestField = new RequestFields(intProfile, dsn);
            ServiceRequests oServiceRequest = new ServiceRequests(intProfile, dsn);
            Services oService = new Services(intProfile, dsn);
            int intRequest = Int32.Parse(Request.QueryString["rid"]);
            string strStatus = oServiceRequest.Get(intRequest, "checkout");
            DataSet dsItems = oRequestItem.GetForms(intRequest);
            int intItem = 0;
            int intService = 0;
            int intNumber = 0;
            if (dsItems.Tables[0].Rows.Count > 0)
            {
                bool boolBreak = false;
                foreach (DataRow drItem in dsItems.Tables[0].Rows)
                {
                    if (boolBreak == true)
                        break;
                    if (drItem["done"].ToString() == "0")
                    {
                        intItem = Int32.Parse(drItem["itemid"].ToString());
                        intService = Int32.Parse(drItem["serviceid"].ToString());
                        intNumber = Int32.Parse(drItem["number"].ToString());
                        boolBreak = true;
                    }
                    if (intItem > 0 && (strStatus == "1" || strStatus == "2"))
                    {
                        bool boolSuccess = true;
                        StringBuilder sbResult = new StringBuilder(oService.GetName(intService) + " Completed");
                        StringBuilder sbError = new StringBuilder(oService.GetName(intService) + " Error");
                        
                        // ********* BEGIN PROCESSING **************
                        AccountRequest oAccountRequest = new AccountRequest(intProfile, dsn);
                        Requests oRequest = new Requests(intProfile, dsn);
                        Users oUser = new Users(intProfile, dsn);
                        DataSet ds = oAccountRequest.GetMaintenance(intRequest, intItem, intNumber);
                        Domains oDomain = new Domains(intProfile, dsn);
                        if (ds.Tables[0].Rows.Count > 0)
                        {

                            string strUser = ds.Tables[0].Rows[0]["username"].ToString();
                            int intDomain = Int32.Parse(ds.Tables[0].Rows[0]["domain"].ToString());
                            int intEnv = Int32.Parse(oDomain.Get(intDomain, "environment"));
                            int intUser = oRequest.GetUser(intRequest);
                            DataSet dsParameters = oAccountRequest.GetMaintenanceParameters(intRequest, intItem, intNumber);
                            AD oAD = new AD(intProfile, dsn, intEnv);
                            DirectoryEntry oEntry = oAD.UserSearch(strUser);
                            
                            if (oEntry == null)
                            {
                                sbResult = new StringBuilder();
                            }
                            else
                            {
                                sbError = new StringBuilder();

                                string strFirstOld = "";
                                if (oEntry.Properties.Contains("givenname") == true)
                                    strFirstOld = oEntry.Properties["givenname"].Value.ToString();
                                string strLastOld = "";
                                if (oEntry.Properties.Contains("sn") == true)
                                    strLastOld = oEntry.Properties["sn"].Value.ToString();
                                string strFirstNew = dsParameters.Tables[0].Rows[0]["value"].ToString();
                                string strLastNew = dsParameters.Tables[0].Rows[1]["value"].ToString();
                                string strEnable = dsParameters.Tables[0].Rows[2]["value"].ToString();
                                string strUnlock = dsParameters.Tables[0].Rows[3]["value"].ToString();
                                string strGroupsRequested = dsParameters.Tables[0].Rows[4]["value"].ToString();
                                if (strFirstOld != strFirstNew || strLastOld != strLastNew)
                                {
                                    if (strFirstOld != strFirstNew && strLastOld != strLastNew)
                                    {
                                        oEntry.Properties["givenname"].Value = strFirstNew;
                                        oEntry.Properties["sn"].Value = strLastNew;
                                        oEntry.Properties["displayname"].Value = strLastNew + ", " + strFirstNew;
                                        oEntry.CommitChanges();
                                        sbResult.Append("<p>The first name and last name for account ");
                                        sbResult.Append(strUser);
                                        sbResult.Append(" was successfully updated</p>");
                                    }
                                    else if (strFirstOld != strFirstNew)
                                    {
                                        oEntry.Properties["givenname"].Value = strFirstNew;
                                        oEntry.Properties["displayname"].Value = strLastNew + ", " + strFirstNew;
                                        oEntry.CommitChanges();
                                        sbResult.Append("<p>The first name for account ");
                                        sbResult.Append(strUser);
                                        sbResult.Append(" was successfully updated</p>");
                                    }
                                    else
                                    {
                                        oEntry.Properties["sn"].Value = strLastNew;
                                        oEntry.Properties["displayname"].Value = strLastNew + ", " + strFirstNew;
                                        oEntry.CommitChanges();
                                        sbResult.Append("<p>The last name for account ");
                                        sbResult.Append(strUser);
                                        sbResult.Append(" was successfully updated</p>");
                                    }
                                }
                                if (strEnable == "1")
                                {
                                    strEnable = oAD.Enable(oEntry, true);
                                    if (strEnable == "")
                                    {
                                        sbResult.Append("<p>The user account ");
                                        sbResult.Append(strUser);
                                        sbResult.Append(" was successfully enabled</p>");
                                    }
                                    else
                                    {
                                        sbError.Append("<p>The user account ");
                                        sbError.Append(strUser);
                                        sbError.Append(" was NOT successfully enabled</p>");
                                    }
                                }
                                if (strUnlock == "1")
                                {
                                    strUnlock = oAD.Unlock(oEntry);
                                    if (strUnlock == "")
                                    {
                                        sbResult.Append("<p>The user account ");
                                        sbResult.Append(strUser);
                                        sbResult.Append(" was successfully unlocked</p>");
                                    }
                                    else
                                    {
                                        sbError.Append("<p>The user account ");
                                        sbError.Append(strUser);
                                        sbError.Append(" was NOT successfully unlocked</p>");
                                    }
                                }

                                string strGroupsExist = oAD.GetGroups(oEntry);
                                string[] strGroupRequested;
                                string[] strGroupExist;
                                char[] strSplit = { ';' };
                                strGroupRequested = strGroupsRequested.Split(strSplit);
                                strGroupExist = strGroupsExist.Split(strSplit);
                                
                                // Add Groups
                                for (int ii = 0; ii < strGroupRequested.Length; ii++)
                                {
                                    if (strGroupRequested[ii].Trim() != "")
                                    {
                                        bool boolExist = false;
                                        for (int jj = 0; jj < strGroupExist.Length; jj++)
                                        {
                                            if (strGroupExist[jj].Trim() != "" && strGroupRequested[ii].Trim() == strGroupExist[jj].Trim())
                                            {
                                                boolExist = true;
                                                break;
                                            }
                                        }
                                        if (boolExist == false)
                                        {
                                            // Add the group
                                            string strJoin = oAD.JoinGroup(strUser, strGroupRequested[ii].Trim(), 0);
                                            if (strJoin == "")
                                            {
                                                sbResult.Append("<p>The user ");
                                                sbResult.Append(strUser);
                                                sbResult.Append(" was successfully joined to the group ");
                                                sbResult.Append(strGroupRequested[ii].Trim());
                                                sbResult.Append("</p>");
                                            }
                                            else
                                            {
                                                sbError.Append("<p>The user ");
                                                sbError.Append(strUser);
                                                sbError.Append(" was NOT successfully joined to the group ");
                                                sbError.Append(strGroupRequested[ii].Trim());
                                                sbError.Append(" : ");
                                                sbError.Append(strJoin);
                                                sbError.Append("</p>");
                                            }
                                        }
                                    }
                                }
                                // Remove Groups
                                for (int ii = 0; ii < strGroupExist.Length; ii++)
                                {
                                    if (strGroupExist[ii].Trim() != "")
                                    {
                                        bool boolExist = false;
                                        for (int jj = 0; jj < strGroupRequested.Length; jj++)
                                        {
                                            if (strGroupRequested[jj].Trim() != "" && strGroupExist[ii].Trim() == strGroupRequested[jj].Trim())
                                            {
                                                boolExist = true;
                                                break;
                                            }
                                        }
                                        if (boolExist == false)
                                        {
                                            // Remove the group
                                            string strJoin = oAD.RemoveGroup(strUser, strGroupExist[ii].Trim());
                                            if (strJoin == "")
                                            {
                                                sbResult.Append("<p>The user ");
                                                sbResult.Append(strUser);
                                                sbResult.Append(" was successfully removed from the group ");
                                                sbResult.Append(strGroupExist[ii].Trim());
                                                sbResult.Append("</p>");
                                            }
                                            else
                                            {
                                                sbError.Append("<p>The user ");
                                                sbError.Append(strUser);
                                                sbError.Append(" was NOT successfully removed from the group ");
                                                sbError.Append(strGroupExist[ii].Trim());
                                                sbError.Append(" : ");
                                                sbError.Append(strJoin);
                                                sbError.Append("</p>");
                                            }
                                        }
                                    }
                                }
                                oRequest.AddResult(intRequest, intItem, intNumber, "Account Modification", sbError.ToString(), sbResult.ToString(), intEnvironment, (oService.Get(intService, "notify_client") == "1"), oUser.GetName(intUser));
                            }
                        }

                        if (sbResult.ToString() == "")
                        {
                            boolSuccess = false;
                        }

                        // ******** END PROCESSING **************
                        if (oService.Get(intService, "automate") == "1" && boolSuccess == true)
                        {
                            strDone += "<table border=\"0\"><tr><td valign=\"top\"><img src=\"/images/ico_check.gif\" border=\"0\" align=\"absmiddle\"/></td><td valign=\"top\" class=\"biggerbold\">" + sbResult.ToString() + "</td></tr></table>";
                        }
                        else
                        {
                            if (boolSuccess == false)
                            {
                                strDone += "<table border=\"0\"><tr><td valign=\"top\"><img src=\"/images/ico_error.gif\" border=\"0\" align=\"absmiddle\"/></td><td valign=\"top\" class=\"biggerbold\">" + sbError.ToString() + "</td></tr></table>";
                            }
                            else
                            {
                                strDone += "<table border=\"0\"><tr><td valign=\"top\"><img src=\"/images/ico_check.gif\" border=\"0\" align=\"absmiddle\"/></td><td valign=\"top\" class=\"biggerbold\">" + oService.GetName(intService) + " Submitted</td></tr></table>";
                            }
                        }
                        oRequestItem.UpdateFormDone(intRequest, intItem, intNumber, 1);
                    }
                }
            }
        }
    }
}