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
    public partial class cp_virtual_workstation_decom : System.Web.UI.UserControl
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        private string dsnRemote = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["RemoteDSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intProfile = 0;
        protected string strDone = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
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
                        Requests oRequest = new Requests(intProfile, dsn);
                        Users oUser = new Users(intProfile, dsn);
                        Workstations oWorkstation = new Workstations(intProfile, dsn);
                        Workstations oRemote = new Workstations(intProfile, dsnRemote);
                        Asset oAsset = new Asset(intProfile, dsnAsset);
                        Domains oDomain = new Domains(intProfile, dsn);
                        ModelsProperties oModelsProperties = new ModelsProperties(intProfile, dsn);
                        DataSet ds = oWorkstation.GetVirtualDecommissions(intRequest, intItem, intNumber);
                        strResult = "";
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            int intID = Int32.Parse(dr["id"].ToString());
                            int intName = Int32.Parse(dr["nameid"].ToString());
                            string strName = oWorkstation.GetName(intName);
                            int intAsset = Int32.Parse(dr["assetid"].ToString());
                            int intModel = Int32.Parse(dr["modelid"].ToString());
                            if (oModelsProperties.IsTypeVMware(intModel) == true)
                            {
                                // VMware Workstation
                                bool boolUnique = oAsset.AddDecommission(intRequest, intItem, intNumber, intAsset, intProfile, "", DateTime.Now, strName, 0, "");
                                if (boolUnique == true)
                                {
                                    oAsset.UpdateDecommission(intRequest, intItem, intNumber, 1);
                                    VMWare oVMWare = new VMWare(intProfile, dsn);
                                    DataSet dsGuest = oVMWare.GetGuest(strName);
                                    string strCluster = "???";
                                    if (dsGuest.Tables[0].Rows.Count > 0 && dsGuest.Tables[0].Rows[0]["hostid"].ToString() != "")
                                    {
                                        int intHost = Int32.Parse(dsGuest.Tables[0].Rows[0]["hostid"].ToString());
                                        int intCluster = Int32.Parse(oVMWare.GetHost(intHost, "clusterid"));
                                        strCluster = oVMWare.GetCluster(intCluster, "name");
                                    }
                                    strResult += "<p>The VMware workstation " + strName + " is queued to be decommissioned from the cluster " + strCluster + ".</p>";
                                }
                                else
                                    strResult += "<p>The VMware workstation " + strName + " is ALREADY queued to be decommissioned</p>";
                            }
                            else
                            {
                                // Microsoft Virtual Workstation
                                int intHost = Int32.Parse(dr["virtualhostid"].ToString());
                                int intOS = Int32.Parse(dr["osid"].ToString());
                                string strHost = oAsset.GetServerOrBlade(intHost, "name");
                                string strVirtualDir = "";
                                DataSet dsOS = oAsset.GetVirtualHostOs(intHost);
                                foreach (DataRow drOS in dsOS.Tables[0].Rows)
                                {
                                    if (Int32.Parse(drOS["osid"].ToString()) == intOS)
                                    {
                                        strVirtualDir = drOS["virtualdir"].ToString();
                                        break;
                                    }
                                }
                                int intDomain = Int32.Parse(dr["domainid"].ToString());
                                int intEnv = Int32.Parse(oDomain.Get(intDomain, "environment"));
                                oRemote.AddRemoteVirtualDecom(intEnv, strHost, strVirtualDir, strName);
                                // Clean up database
                                oWorkstation.DeleteVirtual(intID);
                                oWorkstation.UpdateName(intName, 1);
                                oAsset.DeleteGuest(intAsset);
                                oAsset.AddStatus(intAsset, "", (int)AssetStatus.Decommissioned, intProfile, DateTime.Now);
                                strResult += "<p>The virtual workstation " + strName + " was successfully decommissioned from the host " + strHost + ".</p>";
                            }
                            strError = "";
                        }
                        oRequest.AddResult(intRequest, intItem, intNumber, "Virtual Workstation Decommission", strError, strResult, intEnvironment, (oService.Get(intService, "notify_client") == "1"), oUser.GetName(intProfile));
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
                        oRequestItem.UpdateFormDone(intRequest, intItem, intNumber, 1,1);
                    }
                }
            }
        }
    }
}