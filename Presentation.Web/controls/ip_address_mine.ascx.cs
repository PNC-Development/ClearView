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
using System.Text;
using NCC.ClearView.Application.Core.ClearViewWS;
using System.Threading;
namespace NCC.ClearView.Presentation.Web
{
    public partial class ip_address_mine : System.Web.UI.UserControl
    {

        private DataSet ds;
        protected Pages oPage;
        protected AppPages oAppPage;
        protected Applications oApplication;
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected IPAddresses oIPAddresses;
        protected string strRedirect = "";
        protected int intProfile;
        protected int intPage = 0;
        protected int intApplication = 0;
        protected int intRecords = 20;
        protected int intRecordStart = 1;
        protected Users oUser;
        protected void Page_Load(object sender, EventArgs e)
        {
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oPage = new Pages(intProfile, dsn);
            oAppPage = new AppPages(intProfile, dsn);
            oApplication = new Applications(intProfile, dsn);
            oUser = new Users(intProfile, dsn);
            oIPAddresses = new IPAddresses(intProfile, dsnIP, dsn);
            if (Request.QueryString["pageid"] != null && Request.QueryString["pageid"] != "")
                intPage = Int32.Parse(Request.QueryString["pageid"]);
            if (Request.Cookies["application"] != null && Request.Cookies["application"].Value != "")
                intApplication = Int32.Parse(Request.Cookies["application"].Value);
            strRedirect = oPage.GetFullLink(Int32.Parse(oPage.Get(intPage, "related")));
            lblTitle.Text = oPage.Get(intPage, "title");
            oPage.LoadPaging(oIPAddresses.GetMine(intProfile), Request, intPage, lblPage, lblSort, lblTopPaging, lblBottomPaging, txtPage, lblPages, lblRecords, rptView, lblNone);
            btnRelease.Attributes.Add("onclick", "return ValidateStringItems('" + hdnRelease.ClientID + "','Please select at least one ip address to release') && confirm('Are you sure you want to release the selected ip address(es)?');");
        }
        protected void btnOrder_Click(Object Sender, EventArgs e)
        {
            oPage.btnOrder(Request, Sender, Response, intPage);
        }
        protected void btnPage_Click(Object Sender, ImageClickEventArgs e)
        {
            oPage.btnPage(Request, Response, intPage, txtPage);
        }
        protected void btnRelease_Click(Object Sender, EventArgs e)
        {
            ServerDecommission oServerDecommission = new ServerDecommission(0, dsn);
            string strHidden = Request.Form[hdnRelease.UniqueID];
            while (strHidden != "")
            {
                string strField = strHidden.Substring(0, strHidden.IndexOf("&"));
                strHidden = strHidden.Substring(strHidden.IndexOf("&") + 1);
                string strFlag = strField.Substring(strField.IndexOf("_") + 1);
                strField = strField.Substring(0, strField.IndexOf("_"));
                if (strFlag == "1")
                {
                    int intRelease = Int32.Parse(strField);
                    DataSet dsDetail = oIPAddresses.GetDetail(intRelease);
                    if (dsDetail.Tables[0].Rows.Count > 0)
                    {
                        /*
                        Variables oVariable = new Variables(intEnvironment);
                        Functions oFunction = new Functions(0, dsn, intEnvironment);
                        Log oLog = new Log(0, dsn);

                        System.Net.NetworkCredential oCredentials = new System.Net.NetworkCredential(oVariable.ADUser(), oVariable.ADPassword(), oVariable.Domain());
                        ClearViewWebServices oWebService = new ClearViewWebServices();
                        oWebService.Timeout = Timeout.Infinite;
                        oWebService.Credentials = oCredentials;
                        oWebService.Url = oVariable.WebServiceURL();
                        string strName = (dsDetail.Tables[0].Rows[0]["server_name"].ToString() == "" ? dsDetail.Tables[0].Rows[0]["instance"].ToString() : dsDetail.Tables[0].Rows[0]["server_name"].ToString());
                        string strIP = oIPAddresses.GetName(intRelease, 0);
                        string strEMailIdsBCC = "";
                        string strCheckIP = oWebService.SearchBluecatDNS("", strName);
                        if (strCheckIP.StartsWith("***") == false)
                        {
                            oLog.AddEvent(strName, "", "The Name " + strName + " points to " + strCheckIP, LoggingType.Debug);
                            // Name is unique
                            string strCheckName = oWebService.SearchBluecatDNS(strCheckIP, "");
                            if (strCheckName.StartsWith("***") == false)
                            {
                                oLog.AddEvent(strName, "", "The IP Address " + strCheckIP + " points to " + strCheckName, LoggingType.Debug);
                                // IP is unique
                                if (strName.Trim().ToUpper() == strCheckName.Trim().ToUpper() && strIP.Trim().ToUpper() == strCheckIP.Trim().ToUpper())
                                {
                                    oLog.AddEvent(strName, "", "The name " + strName + " is equal to " + strCheckName + ". OK to release.", LoggingType.Debug);
                                    oLog.AddEvent(strName, "", "Calling Web Service Function DeleteBluecatDNS(" + strIP + ", " + strName + ", false, false) on " + oVariable.WebServiceURL(), LoggingType.Information);
                                    string strDNS = oWebService.DeleteBluecatDNS(strIP, strName, false, false);
                                    if (strDNS == "SUCCESS" || strDNS.StartsWith("***DUPLICATE") == true)
                                    {
                                        oLog.AddEvent(strName, "", "BlueCat DNS Record = SUCCESS", LoggingType.Information);
                                    }
                                    else
                                    {
                                        oLog.AddEvent(strName, "", "BlueCat DNS Record = ERROR: " + strDNS, LoggingType.Error);
                                        strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DEVELOPER_ERROR,EMAILGRP_PROVISIONING_SUPPORT");
                                        oFunction.SendEmail("BlueCat DNS Automation Error", strEMailIdsBCC, "", "", "BlueCat DNS Automation Error", "<p>There was an ERROR deleting the BlueCat DNS Record for " + strName + "</p><p>Error: " + strDNS + "</p><p>NOTE: Since this is an unexpected error, there was no task initiated to fix this problem. You must correct it yourself.</p>", true, false);
                                    }
                                    oLog.AddEvent(strName, "", "BlueCat DNS Record Finished", LoggingType.Information);
                                }
                                else
                                    oLog.AddEvent(strName, "", "The name " + strName + " is NOT equal to " + strCheckName + ". Skipping release.", LoggingType.Error);
                            }
                            else
                                oLog.AddEvent(strName, "", "There was a problem resolving the IP Address " + strCheckIP + " in DNS ~ " + strCheckName, LoggingType.Error);
                        }
                        else
                            oLog.AddEvent(strName, "", "There was a problem resolving the Name " + strName + " in DNS ~ " + strCheckIP, LoggingType.Error);
                        */
                    }
                    oIPAddresses.UpdateAvailable(intRelease, 1);
                }
            }
            string strSort = "";
            if (Request.QueryString["sort"] != null)
                strSort = Request.QueryString["sort"];
            string strPage = "1";
            if (Request.QueryString["page"] != null)
                strPage = Request.QueryString["page"];
            Response.Redirect(oPage.GetFullLink(intPage) + "?sort=" + strSort + "&page=" + strPage);
        }
    }
}