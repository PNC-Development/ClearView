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
using NCC.ClearView.Application.Core.ClearViewWS;

namespace NCC.ClearView.Presentation.Web
{
    public partial class cp_dns : System.Web.UI.UserControl
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
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
                        DNS oDNS = new DNS(intProfile, dsn);
                        strResult = "";
                        strError = "";
                        if (intEnvironment < 3)
                            intEnvironment = 3;
                        Variables oVariable = new Variables(intEnvironment);
                        Requests oRequest = new Requests(intProfile, dsn);
                        Users oUser = new Users(intProfile, dsn);
                        DataSet ds = oDNS.GetDNS(intRequest, intItem, intNumber);
                        Domains oDomain = new Domains(intProfile, dsn);
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            string strAction = ds.Tables[0].Rows[0]["action"].ToString();
                            string strNameCurrent = ds.Tables[0].Rows[0]["name_current"].ToString();
                            string strIPCurrent = ds.Tables[0].Rows[0]["ip_current"].ToString();
                            string strAliasCurrent = ds.Tables[0].Rows[0]["alias_current"].ToString();
                            string strNameNew = ds.Tables[0].Rows[0]["name_new"].ToString();
                            string strIPNew = ds.Tables[0].Rows[0]["ip_new"].ToString();
                            string strAliasNew = ds.Tables[0].Rows[0]["alias_new"].ToString();
                            string strDomain = ds.Tables[0].Rows[0]["domain"].ToString();
                            string strObject = ds.Tables[0].Rows[0]["value"].ToString();
                            int intUser = oRequest.GetUser(intRequest);
                            
                            // Connect to DNS to process the request
                            System.Net.NetworkCredential oCredentials = new System.Net.NetworkCredential(oVariable.ADUser(), oVariable.ADPassword(), oVariable.Domain());
                            ClearViewWebServices oWebService = new ClearViewWebServices();
                            oWebService.Timeout = Int32.Parse(ConfigurationManager.AppSettings["WS_TIMEOUT"]);
                            oWebService.Credentials = oCredentials;
                            oWebService.Url = oVariable.WebServiceURL();
                            Settings oSetting = new Settings(0, dsn);
                            bool boolDNS_QIP = oSetting.IsDNS_QIP();
                            bool boolDNS_Bluecat = oSetting.IsDNS_Bluecat();

                            string strWebServiceResult = "";

                            switch (strAction)
                            {
                                case "CREATE":
                                    if (strIPNew != "" && strNameNew != "")
                                    {
                                        if (boolDNS_QIP == true)
                                        {
                                            strWebServiceResult = oWebService.CreateDNSforPNC(strIPNew, strNameNew, strObject, strAliasNew, oVariable.DNS_Domain(), oVariable.DNS_NameService(), oVariable.DNS_DynamicDNSUpdate(), intProfile, 0, true);
                                            if (strWebServiceResult == "SUCCESS")
                                                strResult += "<p>The following record was successfully created in QIP DNS...</p><p>" + oDNS.GetDNSBody(intRequest, intItem, intNumber, true, intEnvironment) + "</p>";
                                            else if (strWebServiceResult.StartsWith("***DUPLICATE") == true)
                                                strResult += "<p>The following record was already created in QIP DNS...</p><p>" + oDNS.GetDNSBody(intRequest, intItem, intNumber, true, intEnvironment) + "</p>";
                                            else if (strWebServiceResult.StartsWith("***CONFLICT") == true)
                                                strError += "<p>A CONFLICT occurred when attempting to create the following record in QIP DNS...</p><p>" + oDNS.GetDNSBody(intRequest, intItem, intNumber, true, intEnvironment) + "</p><p>Conflict Message:</p><p>" + strWebServiceResult + "</p>";
                                            else
                                                strError += "<p>An ERROR occurred when attempting to create the following record in QIP DNS...</p><p>" + oDNS.GetDNSBody(intRequest, intItem, intNumber, true, intEnvironment) + "</p><p>Error Message:</p><p>" + strWebServiceResult + "</p>";
                                        }
                                        if (boolDNS_Bluecat == true)
                                        {
                                            strWebServiceResult = oWebService.CreateBluecatDNS(strIPNew, strNameNew, strNameNew, "");
                                            if (strWebServiceResult == "SUCCESS")
                                                strResult += "<p>The following record was successfully created in BlueCat DNS...</p><p>" + oDNS.GetDNSBody(intRequest, intItem, intNumber, true, intEnvironment) + "</p>";
                                            else if (strWebServiceResult.StartsWith("***DUPLICATE") == true)
                                                strResult += "<p>The following record was already created in BlueCat DNS...</p><p>" + oDNS.GetDNSBody(intRequest, intItem, intNumber, true, intEnvironment) + "</p>";
                                            else if (strWebServiceResult.StartsWith("***CONFLICT") == true)
                                                strError += "<p>A CONFLICT occurred when attempting to create the following record in BlueCat DNS...</p><p>" + oDNS.GetDNSBody(intRequest, intItem, intNumber, true, intEnvironment) + "</p><p>Conflict Message:</p><p>" + strWebServiceResult + "</p>";
                                            else
                                                strError += "<p>An ERROR occurred when attempting to create the following record in BlueCat DNS...</p><p>" + oDNS.GetDNSBody(intRequest, intItem, intNumber, true, intEnvironment) + "</p><p>Error Message:</p><p>" + strWebServiceResult + "</p>";
                                        }
                                    }
                                    else
                                        strError = "<p>Invalid Parameters...</p>";

                                    break;
                                case "UPDATE":
                                    string strIP = (strIPNew != "" ? strIPNew : strIPCurrent);
                                    string strName = (strNameNew != "" ? strNameNew : strNameCurrent);
                                    string strAlias = (((strAliasCurrent != "" && strAliasNew == "") || strAliasNew != "") ? strAliasNew : strAliasCurrent);
                                    if (strObject == "")
                                        strObject = "Server";
                                    if (boolDNS_QIP == true)
                                    {
                                        strWebServiceResult = oWebService.UpdateDNSforPNC(strIP, strName, strObject, strAlias, oVariable.DNS_Domain(), oVariable.DNS_NameService(), oVariable.DNS_DynamicDNSUpdate(), intProfile, 0, true);
                                        if (strWebServiceResult == "SUCCESS")
                                            strResult += "<p>The following record was successfully updated in QIP DNS...</p><p>" + oDNS.GetDNSBody(intRequest, intItem, intNumber, true, intEnvironment) + "</p>";
                                        else if (strWebServiceResult.StartsWith("***DUPLICATE") == true)
                                            strResult += "<p>The following record was already updated in QIP DNS...</p><p>" + oDNS.GetDNSBody(intRequest, intItem, intNumber, true, intEnvironment) + "</p>";
                                        else if (strWebServiceResult.StartsWith("***CONFLICT") == true)
                                            strError += "<p>A CONFLICT occurred when attempting to update the following record in QIP DNS...</p><p>" + oDNS.GetDNSBody(intRequest, intItem, intNumber, true, intEnvironment) + "</p><p>Conflict Message:</p><p>" + strWebServiceResult + "</p>";
                                        else
                                            strError += "<p>An ERROR occurred when attempting to update the following record in QIP DNS...</p><p>" + oDNS.GetDNSBody(intRequest, intItem, intNumber, true, intEnvironment) + "</p><p>Error Message:</p><p>" + strWebServiceResult + "</p>";
                                    }
                                    if (boolDNS_Bluecat == true)
                                    {
                                        strWebServiceResult = oWebService.UpdateBluecatDNS(strIP, strName, strNameNew, "");
                                        if (strWebServiceResult == "SUCCESS")
                                            strResult += "<p>The following record was successfully updated in BlueCat DNS...</p><p>" + oDNS.GetDNSBody(intRequest, intItem, intNumber, true, intEnvironment) + "</p>";
                                        else if (strWebServiceResult.StartsWith("***DUPLICATE") == true)
                                            strResult += "<p>The following record was already updated in BlueCat DNS...</p><p>" + oDNS.GetDNSBody(intRequest, intItem, intNumber, true, intEnvironment) + "</p>";
                                        else if (strWebServiceResult.StartsWith("***CONFLICT") == true)
                                            strError += "<p>A CONFLICT occurred when attempting to update the following record in BlueCat DNS...</p><p>" + oDNS.GetDNSBody(intRequest, intItem, intNumber, true, intEnvironment) + "</p><p>Conflict Message:</p><p>" + strWebServiceResult + "</p>";
                                        else
                                            strError += "<p>An ERROR occurred when attempting to update the following record in BlueCat DNS...</p><p>" + oDNS.GetDNSBody(intRequest, intItem, intNumber, true, intEnvironment) + "</p><p>Error Message:</p><p>" + strWebServiceResult + "</p>";
                                    }
                                    break;
                                case "DELETE":
                                    if (boolDNS_QIP == true)
                                    {
                                        if (strIPCurrent != "")
                                            strWebServiceResult = oWebService.DeleteDNSforPNC(strIPCurrent, "", intProfile, true);
                                        else if (strNameCurrent != "")
                                            strWebServiceResult = oWebService.DeleteDNSforPNC("", strNameCurrent, intProfile, true);

                                        if (strWebServiceResult == "SUCCESS")
                                            strResult += "<p>The following record was successfully deleted in QIP DNS...</p><p>" + oDNS.GetDNSBody(intRequest, intItem, intNumber, true, intEnvironment) + "</p>";
                                        else
                                            strError += "<p>An ERROR occurred when attempting to delete the following record in QIP DNS...</p><p>" + oDNS.GetDNSBody(intRequest, intItem, intNumber, true, intEnvironment) + "</p><p>Error Message:</p><p>" + strWebServiceResult + "</p>";
                                    }
                                    if (boolDNS_Bluecat == true)
                                    {
                                        if (strIPCurrent != "")
                                            strWebServiceResult = oWebService.DeleteBluecatDNS(strIPCurrent, "", false, false);
                                        else if (strNameCurrent != "")
                                            strWebServiceResult = oWebService.DeleteBluecatDNS("", strNameCurrent, false, false);

                                        if (strWebServiceResult == "SUCCESS")
                                            strResult += "<p>The following record was successfully deleted in BlueCat DNS...</p><p>" + oDNS.GetDNSBody(intRequest, intItem, intNumber, true, intEnvironment) + "</p>";
                                        else
                                            strError += "<p>An ERROR occurred when attempting to delete the following record in BlueCat DNS...</p><p>" + oDNS.GetDNSBody(intRequest, intItem, intNumber, true, intEnvironment) + "</p><p>Error Message:</p><p>" + strWebServiceResult + "</p>";
                                    }
                                    break;
                            }
                            if (strResult != "")
                                oRequest.AddResult(intRequest, intItem, intNumber, oService.GetName(intService), "", strResult, intEnvironment, (oService.Get(intService, "notify_client") == "1"), oUser.GetName(intUser));
                            else if (strError != "")
                                oRequest.AddResult(intRequest, intItem, intNumber, oService.GetName(intService), strError, "", intEnvironment, (oService.Get(intService, "notify_client") == "1"), oUser.GetName(intUser));
                            oDNS.UpdateDNSCompleted(intRequest, intItem, intNumber);
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