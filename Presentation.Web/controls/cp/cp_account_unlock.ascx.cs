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
    public partial class cp_account_unlock : System.Web.UI.UserControl
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
                            AD oAD = new AD(intProfile, dsn, intEnv);
                            DirectoryEntry oEntry = oAD.UserSearch(strUser);
                            if (oEntry == null)
                                strResult = "";
                            else
                            {
                                if (oAD.IsLocked(oEntry) == true)
                                {
                                    strResult = oAD.Unlock(oEntry);
                                    oRequest.AddResult(intRequest, intItem, intNumber, "Account Unlock", strResult, "Account " + strUser + " was successfully unlocked", intEnvironment, (oService.Get(intService, "notify_client") == "1"), oUser.GetName(intUser));
                                    if (strResult == "")
                                        strResult = "<p>Account " + strUser + " was unlocked</p>";
                                }
                                else
                                {
                                    oRequest.AddResult(intRequest, intItem, intNumber, "Account Unlock", "", "Account " + strUser + " was not locked", intEnvironment, (oService.Get(intService, "notify_client") == "1"), oUser.GetName(intUser));
                                    strResult = "<p>Account " + strUser + " was not locked</p>";
                                }
                            }
                        }
                        if (strResult == "")
                            boolSuccess = false;
                        // ******** END PROCESSING **************
                        if (oService.Get(intService, "automate") == "1" && boolSuccess == true)
                            strDone += "<table border=\"0\"><tr><td><img src=\"/images/bigCheck.gif\" border=\"0\" align=\"absmiddle\"/></td><td class=\"biggerbold\">" + strResult + "</td></tr></table>";
                        else
                        {
                            if (boolSuccess == false)
                                strDone += "<table border=\"0\"><tr><td><img src=\"/images/bigError.gif\" border=\"0\" align=\"absmiddle\"/></td><td class=\"biggerbold\">" + strError + "</td></tr></table>";
                            else
                                strDone += "<table border=\"0\"><tr><td><img src=\"/images/bigCheck.gif\" border=\"0\" align=\"absmiddle\"/></td><td class=\"biggerbold\">" + oService.GetName(intService) + " Submitted</td></tr></table>";
                        }
                        oRequestItem.UpdateFormDone(intRequest, intItem, intNumber, 1);
                    }
                }
            }
        }
    }
}