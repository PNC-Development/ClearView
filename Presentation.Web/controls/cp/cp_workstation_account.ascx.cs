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

namespace NCC.ClearView.Presentation.Web
{
    public partial class cp_workstation_account : System.Web.UI.UserControl
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
       

        protected int intAssignPage = Int32.Parse(ConfigurationManager.AppSettings["AssignWorkload"]);
        protected int intViewPage = Int32.Parse(ConfigurationManager.AppSettings["ViewWorkload"]);
        protected int intResourceRequestApprove = Int32.Parse(ConfigurationManager.AppSettings["ResourceRequestApprove"]);
        protected string strDone = "";
        protected int intProfile = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            RequestItems oRequestItem = new RequestItems(intProfile, dsn);
            ServiceRequests oServiceRequest = new ServiceRequests(intProfile, dsn);
            Services oService = new Services(intProfile, dsn);
            Functions oFunction = new Functions(intProfile, dsn, intEnvironment);
            Workstations oWorkstation = new Workstations(intProfile, dsn);
            Customized oCustomized = new Customized(intProfile, dsn);
            Forecast oForecast = new Forecast(intProfile, dsn);
            Variables oVariable = new Variables(intEnvironment);
            Users oUser = new Users(intProfile, dsn);
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
                        DataSet ds = oCustomized.GetVirtualWorkstationAccount(intRequest, intItem, intNumber);
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            int intWorkstation = Int32.Parse(dr["workstationid"].ToString());
                            int intAnswer = Int32.Parse(oWorkstation.GetVirtual(intWorkstation, "answerid"));
                            int intUser = Int32.Parse(oForecast.GetAnswer(intAnswer, "appcontact"));
                            string strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_REQUEST_ASSIGNMENT");
                            oFunction.SendEmail("Virtual Workstation Account Approval", oUser.GetName(intUser), strEMailIdsBCC, strEMailIdsBCC, "Virtual Workstation Account Approval", "<p><b>A virtual workstation account request requires your approval.</b></p><p><a href=\"" + oVariable.URL() + "/redirect.aspx?referrer=/approval/workstation/account.aspx?id=" + intWorkstation.ToString() + "\" target=\"_blank\">Click here to view this request.</a></p>", true, false);
                        }
                        // ******** END PROCESSING **************
                        if (oService.Get(intService, "automate") == "1" && boolSuccess == true)
                            strDone += "<p><span class=\"biggerbold\"><img src=\"/images/bigCheck.gif\" border=\"0\" align=\"absmiddle\"/> " + strResult + "</span></p>";
                        else
                        {
                            if (boolSuccess == false)
                                strDone += "<p><span class=\"biggerbold\"><img src=\"/images/bigError.gif\" border=\"0\" align=\"absmiddle\"/> " + strError + "</span></p>";
                            else
                                strDone += "<p><span class=\"biggerbold\"><img src=\"/images/bigCheck.gif\" border=\"0\" align=\"absmiddle\"/> " + oService.GetName(intService) + " Submitted</span></p>";
                        }
                        oRequestItem.UpdateFormDone(intRequest, intItem, intNumber, 1);
                    }
                }
            }
        }
    }
}