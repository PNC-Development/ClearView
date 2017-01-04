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
    public partial class cp_virtual_workstation : System.Web.UI.UserControl
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnServiceEditor = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["ServiceEditorDSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intAssignPage = Int32.Parse(ConfigurationManager.AppSettings["AssignWorkload"]);
        protected int intResourceRequestApprove = Int32.Parse(ConfigurationManager.AppSettings["ResourceRequestApprove"]);
        protected int intViewPage = Int32.Parse(ConfigurationManager.AppSettings["ViewWorkload"]);
        protected string strDone = "";
        protected int intProfile = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            RequestItems oRequestItem = new RequestItems(intProfile, dsn);
            RequestFields oRequestField = new RequestFields(intProfile, dsn);
            ServiceRequests oServiceRequest = new ServiceRequests(intProfile, dsn);
            Services oService = new Services(intProfile, dsn);
            Log oLog = new Log(intProfile, dsn);
            Users oUser = new Users(intProfile, dsn);
            Functions oFunction = new Functions(intProfile, dsn, intEnvironment);
            Pages oPage = new Pages(intProfile, dsn);
            Requests oRequest = new Requests(intProfile, dsn);
            Variables oVariable = new Variables(intEnvironment);
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
                       
                        string strResult = oService.GetName(intService) + " Completed";
                        string strError = oService.GetName(intService) + " Error";
                        // ********* BEGIN PROCESSING **************
                        string strCVT = "CVT" + intRequest.ToString() + "-" + intService.ToString() + "-" + intNumber.ToString();
                        Forecast oForecast = new Forecast(intProfile, dsn);
                        ModelsProperties oModelsProperties = new ModelsProperties(intProfile, dsn);
                        Types oType = new Types(intProfile, dsn);
                        DataSet dsService = oForecast.GetAnswerService(intRequest);
                        if (dsService.Tables[0].Rows.Count > 0)
                        {
                            int intAnswer = Int32.Parse(dsService.Tables[0].Rows[0]["id"].ToString());
                            int intModel = Int32.Parse(dsService.Tables[0].Rows[0]["modelid"].ToString());
                            int intType = oModelsProperties.GetType(intModel);
                            string strExecute = oType.Get(intType, "forecast_execution_path");
                            DataSet dsSelected = oService.GetSelected(intRequest, intService, intNumber);

                            if (oServiceRequest.NotifyApproval(intRequest, intService, intNumber, intResourceRequestApprove, intEnvironment, "", dsnServiceEditor) == false)
                            {
                                oLog.AddEvent(intRequest.ToString(), strCVT, "Service request has been fully approved", LoggingType.Debug);
                                Workstations oWorkstation = new Workstations(intProfile, dsn);
                                oWorkstation.ExecuteVMware(intRequest);
                                oLog.AddEvent(intRequest.ToString(), strCVT, "Service request ExecuteVMware has completed.", LoggingType.Debug);
                            }
                            else
                                oLog.AddEvent(intRequest.ToString(), strCVT, "Service request has been sent for approval", LoggingType.Debug);
                            //if (strExecute != "")
                            //    btnStart.Attributes.Add("onclick", "return OpenWindow('FORECAST_EXECUTE','" + strExecute + "?id=" + intAnswer.ToString() + "');");
                            //else
                            //    btnStart.Attributes.Add("onclick", "alert('Execution has not been configured for asset type " + oType.Get(intType, "name") + "');return false;");
                            strDone += "<table border=\"0\"><tr><td><img src=\"/images/bigCheck.gif\" border=\"0\" align=\"absmiddle\"/></td><td class=\"biggerbold\">" + oService.GetName(intService) + " Submitted</td></tr></table>";
                        }
                        // ******** END PROCESSING **************
                        oRequestItem.UpdateFormDone(intRequest, intItem, intNumber, 1);
                    }
                }
            }
        }
    }
}