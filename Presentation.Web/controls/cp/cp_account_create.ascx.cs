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

namespace NCC.ClearView.Presentation.Web
{
    public partial class cp_account_create : System.Web.UI.UserControl
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
                        string strResult = oService.GetName(intService) + " Completed";
                        string strError = oService.GetName(intService) + " Error";
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
                            Variables oVariable = new Variables(intEnv);
                            DirectoryEntry oEntry = oAD.UserSearch(strUser);
                            if (oEntry != null)
                                strResult = "<p>The account " + strUser + " already exists in " + oDomain.Get(intDomain, "name") + "</p>";
                            else
                            {
                                strError = "";
                                Encryption oEncrypt = new Encryption();
                                string strFirst = dsParameters.Tables[0].Rows[0]["value"].ToString();
                                string strLast = dsParameters.Tables[0].Rows[1]["value"].ToString();
                                string strEmail = dsParameters.Tables[0].Rows[2]["value"].ToString();
                                string strTechnical = dsParameters.Tables[0].Rows[3]["value"].ToString();
                                string strPassword = oEncrypt.Decrypt(dsParameters.Tables[0].Rows[4]["value"].ToString(), "adpass");
                                if (strTechnical == "1")
                                {
                                    strResult = oAD.CreateUser(strUser, strFirst, strLast, strPassword, "", "Created By ClearView on " + DateTime.Today.ToShortDateString() + Environment.NewLine + "Requested by " + oUser.GetFullName(intProfile) + " (" + oUser.GetName(intProfile) + ")", oVariable.UserOUTechnical());
                                    if (strResult == "")
                                        strResult = "<p>The account " + strUser + " was successfully created in " + oDomain.Get(intDomain, "name") + " with registry access (Password: " + strPassword + ")</p>";
                                }
                                else
                                {
                                    strResult = oAD.CreateUser(strUser, strFirst, strLast, strPassword, "", "Created By ClearView on " + DateTime.Today.ToShortDateString() + Environment.NewLine + "Requested by " + oUser.GetFullName(intProfile) + " (" + oUser.GetName(intProfile) + ")", oVariable.UserOU());
                                    if (strResult == "")
                                        strResult = "<p>The account " + strUser + " was successfully created in " + oDomain.Get(intDomain, "name") + " (Password: " + strPassword + ")</p>";
                                }
                                string strGroupsRequested = dsParameters.Tables[0].Rows[5]["value"].ToString();
                                string[] strGroupRequested;
                                char[] strSplit = { ';' };
                                strGroupRequested = strGroupsRequested.Split(strSplit);
                                // Add Groups
                                for (int ii = 0; ii < strGroupRequested.Length; ii++)
                                {
                                    if (strGroupRequested[ii].Trim() != "")
                                    {
                                        // Add the group
                                        string strJoin = oAD.JoinGroup(strUser, strGroupRequested[ii].Trim(), 0);
                                        if (strJoin == "")
                                            strResult += "<p>The user " + strUser + " was successfully joined to the group " + strGroupRequested[ii].Trim() + "</p>";
                                        else
                                            strError += "<p>The user " + strUser + " was NOT successfully joined to the group " + strGroupRequested[ii].Trim() + " : " + strJoin + "</p>";
                                    }
                                }
                                oRequest.AddResult(intRequest, intItem, intNumber, "Account Modification", strError, strResult, intEnvironment, (oService.Get(intService, "notify_client") == "1"), oUser.GetName(intUser));
                            }
                        }
                        if (strResult == "")
                            boolSuccess = false;
                        // ******** END PROCESSING **************
                        if (oService.Get(intService, "automate") == "1" && boolSuccess == true)
                            strDone += "<table border=\"0\"><tr><td valign=\"top\"><img src=\"/images/ico_check.gif\" border=\"0\" align=\"absmiddle\"/></td><td valign=\"top\" class=\"biggerbold\">" + strResult + "</td></tr></table>";
                        else
                        {
                            if (boolSuccess == false)
                                strDone += "<table border=\"0\"><tr><td valign=\"top\"><img src=\"/images/ico_error.gif\" border=\"0\" align=\"absmiddle\"/></td><td valign=\"top\" class=\"biggerbold\">" + strError + "</td></tr></table>";
                            else
                                strDone += "<table border=\"0\"><tr><td valign=\"top\"><img src=\"/images/ico_check.gif\" border=\"0\" align=\"absmiddle\"/></td><td valign=\"top\" class=\"biggerbold\">" + oService.GetName(intService) + " Submitted</td></tr></table>";
                        }
                        oRequestItem.UpdateFormDone(intRequest, intItem, intNumber, 1);
                    }
                }
            }
        }
    }
}