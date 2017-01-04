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
    public partial class DesignView : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
        private string dsnServiceEditor = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["ServiceEditorDSN"]].ConnectionString;
        private int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        private int intProfile = 0;
        private int intApplication = 0;

        protected int intDesignId=0; //ForecastAnswerId
        protected string strDesignRequestedBy = "";

        private Users oUser;
        private Forecast oForecast;
        
        private Requests oRequest;
        private DataPoint oDataPoint;
        private Servers oServer;
        private Projects oProject;
        private ModelsProperties oModelsProperties;
        private Classes oClass;
        private ServerName oServerName;
        private Locations oLocation;
        private Environments oEnvironment;
        private MaintenanceWindow oMaintenanceWindow;
        private Confidence oConfidence;
        private Types oType;
        private Mnemonic oMnemonic;
        private OperatingSystems oOperatingSystem;
        private ServicePacks oServicePack;
        private Domains oDomain;
        private VMWare oVMWare;
        private ConsistencyGroups oConsistencyGroup;
        private Storage oStorage;


        private double dblStorageAmount = 0.00;

        string strTableStart = "<table width=\"100%\">";
        string strTableEnd = "</table>";

        string strTRStart="<tr>";
        string strTREnd="</tr>";

        string strTDLableStart = "<td width=\"30%\" class=\"bold\">";
        string strTDValueStart = "<td width=\"70%\" colspan=\"2\" >";
        string strTDStart = "<td >";
        string strTDLableStart1 = "<td class=\"bold\">";
        string strTRTableHeader = "<tr bgcolor=\"#EEEEEE\">";
        string strTDTableHeader = "<td class=\"tableheader\">";
        string strTDAlert = "<td class=\"reddefault\">";

        string strTDEnd = "</td>";
        
        protected string strHTMLPageHeader = "";

        protected string strHTMLProjectInfo = "";
        protected string strHTMLDesignInfo = "";
        
        protected string strHTMLForecastInfoGeneral = "";
        protected string strHTMLForecastInfoPlatform = "";
        protected string strHTMLForecastInfoStorage = "";
        protected string strHTMLForecastBackupInfo = "";
        protected string strHTMLForecastBackupAddInfo = "";

        protected string strHTMLConfigDetailsApp = "";
        protected string strHTMLConfigDetailsDevice = "";
        protected string strHTMLConfigDetailsUser = "";
        protected string strHTMLConfigDetailsStorage = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            AuthenticateUser();
            if (Request.Cookies["profileid"] != null && Request.Cookies["profileid"].Value != "")
                intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            if (Request.QueryString["applicationid"] != null && Request.QueryString["applicationid"] != "")
                intApplication = Int32.Parse(Request.QueryString["applicationid"]);
            if (Request.Cookies["application"] != null && Request.Cookies["application"].Value != "")
                intApplication = Int32.Parse(Request.Cookies["application"].Value);

            if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
               intDesignId = Int32.Parse(Request.QueryString["id"]);
            

            oForecast = new Forecast(intProfile, dsn);
            oUser = new Users(intProfile, dsn);
            oRequest = new Requests(intProfile, dsn);
            oDataPoint = new DataPoint(intProfile, dsn);
            oServer = new Servers(intProfile, dsn);
            oProject = new Projects(intProfile, dsn);
            oLocation = new Locations(intProfile, dsn);
            oEnvironment = new Environments(intProfile, dsn);
            oMaintenanceWindow = new MaintenanceWindow(intProfile, dsn);
            oConfidence = new Confidence(intProfile, dsn);
            oType = new Types(intProfile, dsn);
            oModelsProperties = new ModelsProperties(intProfile, dsn);
            oClass = new Classes(intProfile, dsn);
            oServerName = new ServerName(intProfile, dsn);
            oMnemonic = new Mnemonic(intProfile, dsn);
            oOperatingSystem = new OperatingSystems(intProfile, dsn);
            oServicePack = new ServicePacks(intProfile, dsn);
            oDomain = new Domains(intProfile, dsn);
            oVMWare = new VMWare(intProfile, dsn);
            oConsistencyGroup = new ConsistencyGroups(intProfile, dsn);
            oStorage = new Storage(intProfile, dsn);

              DataSet dsServiceDesign = oDataPoint.GetServiceDesign(intDesignId);
              if (dsServiceDesign.Tables[0].Rows.Count == 1)
              {
                    //Get the Page Header Section
                    GetPageHeader(dsServiceDesign);
                    //Get the Project Info
                    GetProjectInfo(dsServiceDesign);
                  
                    //GetDesignInfo(dsServiceDesign);

                    //Get the forecast Details
                    GetForecastInfoGeneral(dsServiceDesign);
              }




        }

        private void GetPageHeader(DataSet dsForecastAns)
        {
            
            strHTMLPageHeader += strTableStart;
           
            strHTMLPageHeader += strTRStart + strTDLableStart + "Design Id" + strTDEnd +
                                     strTDValueStart + intDesignId.ToString() + strTDEnd + strTREnd;

            strDesignRequestedBy = oUser.GetFullName(Int32.Parse(dsForecastAns.Tables[0].Rows[0]["userid"].ToString()));

            strHTMLPageHeader += strTRStart + strTDLableStart + "Requested By" + strTDEnd +
                                     strTDValueStart + strDesignRequestedBy.ToString() + strTDEnd + strTREnd;

            strHTMLPageHeader += strTableEnd;

            strHTMLPageHeader += strTableStart;

            if (dsForecastAns.Tables[0].Rows[0]["executed"] != DBNull.Value) 
                strHTMLPageHeader += strTRStart + strTDAlert + "* This design is not executed yet. " + strTDEnd + strTREnd;
            else
                strHTMLPageHeader += strTRStart + strTDAlert + "* This design is executed. " + strTDEnd + strTREnd;
            strHTMLPageHeader += strTableEnd;

            strHTMLPageHeader += strTableStart;
            strHTMLPageHeader += strTRStart + strTDStart + "<hr/> " + strTDEnd + strTREnd;
            strHTMLPageHeader += strTableEnd;

            Master.Page.Title = "Design Request Summary | Design ID #" + intDesignId.ToString(); ;

        }

        private void GetProjectInfo(DataSet dsForecastAns)
        {
            int intForecastId = 0;
            int intRequestId = 0;
            int intProjectId = 0; 

            OnDemandTasks oOnDemandTasks = new OnDemandTasks(intProfile,dsn);
            ResourceRequest oResourceRequest = new ResourceRequest(intProfile,dsn);
            Organizations oOrg = new Organizations(intProfile,dsn);
            DataSet dsForeCastAnswer= oForecast.GetAnswer(intDesignId);
            DataSet dsRequest;
            DataSet dsProject;
            

            if (dsForeCastAnswer.Tables[0].Rows.Count != 0)
            {   //Get the forecast id
                intForecastId = Int32.Parse(dsForeCastAnswer.Tables[0].Rows[0]["forecastid"].ToString());
                //Get the request id
                dsRequest = oForecast.Get(intForecastId);

                //Get the project id
                if (dsForeCastAnswer.Tables[0].Rows.Count != 0)
                    intRequestId = Int32.Parse(dsRequest.Tables[0].Rows[0]["requestid"].ToString());

                dsRequest = oRequest.Get(intRequestId);

                if (dsRequest.Tables[0].Rows.Count != 0)
                    intProjectId = Int32.Parse(dsRequest.Tables[0].Rows[0]["projectid"].ToString());

                dsProject = oProject.Get(intProjectId);

                if (dsProject.Tables[0].Rows.Count != 0)
                {
                    strHTMLDesignInfo += strTableStart;

                    strHTMLProjectInfo += strTRStart + strTDLableStart + "Project Name" + strTDEnd + 
                                        strTDValueStart + dsProject.Tables[0].Rows[0]["name"].ToString() + strTDEnd + strTREnd;
                    strHTMLProjectInfo += strTRStart + strTDLableStart + "Project Number" + strTDEnd + 
                                        strTDValueStart + dsProject.Tables[0].Rows[0]["number"].ToString() + strTDEnd + strTREnd;
                    strHTMLProjectInfo += strTRStart + strTDLableStart + "Project Type" + strTDEnd + 
                                        strTDValueStart + dsProject.Tables[0].Rows[0]["bd"].ToString() + strTDEnd + strTREnd;

                    strHTMLProjectInfo += strTRStart + strTDLableStart + "Porfolio" + strTDEnd +
                                        strTDValueStart + oOrg.GetName(Int32.Parse(dsProject.Tables[0].Rows[0]["organization"].ToString())) + strTDEnd + strTREnd;

                    strHTMLProjectInfo += strTRStart + strTDLableStart + "Project Manager" + strTDEnd +
                                        strTDValueStart + oUser.GetFullName(Int32.Parse(dsProject.Tables[0].Rows[0]["lead"].ToString()))+ strTDEnd + strTREnd;

                    //Requestor =Forecast Answer User Id
                    strHTMLProjectInfo += strTRStart + strTDLableStart + "Requester" + strTDEnd +
                                      strTDValueStart + oUser.GetFullName(Int32.Parse(dsForecastAns.Tables[0].Rows[0]["userid"].ToString())) + strTDEnd + strTREnd;

                    strHTMLProjectInfo += strTRStart + strTDLableStart + "Integration Engineer" + strTDEnd +
                                      strTDValueStart + oUser.GetFullName(Int32.Parse(dsProject.Tables[0].Rows[0]["engineer"].ToString())) + strTDEnd + strTREnd;

                   strHTMLProjectInfo += strTRStart + strTDLableStart + "Technical Lead" + strTDEnd +
                                      strTDValueStart + oUser.GetFullName(Int32.Parse(dsProject.Tables[0].Rows[0]["technical"].ToString())) + strTDEnd + strTREnd;


                   
                    int intImplementorUser = 0;
                    DataSet dsImplementor = oOnDemandTasks.GetPending(intDesignId);
                    if (dsImplementor.Tables[0].Rows.Count > 0)
                    {
                        intImplementorUser = Int32.Parse(dsImplementor.Tables[0].Rows[0]["resourceid"].ToString());
                        intImplementorUser = Int32.Parse(oResourceRequest.GetWorkflow(intImplementorUser, "userid"));
                    }
                    else
                        intImplementorUser = -999;

                    strHTMLProjectInfo += strTRStart + strTDLableStart + "IIS Resource" + strTDEnd +
                                     strTDValueStart + oUser.GetFullName(intImplementorUser) + strTDEnd + strTREnd;

                    strHTMLDesignInfo += strTableEnd;    
                }

            }
           
        }

        private void GetDesignInfo(DataSet dsForecastAns)
        {
            strHTMLDesignInfo += strTableStart;
            strHTMLDesignInfo += strTRStart + strTDLableStart + "Commitment Date" + strTDEnd +
                                     strTDValueStart + dsForecastAns.Tables[0].Rows[0]["implementation"].ToString() + strTDEnd + strTREnd;

            strHTMLDesignInfo += strTRStart + strTDLableStart + "Completion / Installation Date" + strTDEnd +
                                     strTDValueStart + dsForecastAns.Tables[0].Rows[0]["completed"].ToString() + strTDEnd + strTREnd;

            strHTMLDesignInfo += strTRStart + strTDLableStart + "Acquisition Cost (fix)" + strTDEnd +
                                    strTDValueStart + "" + strTDEnd + strTREnd;

            strHTMLDesignInfo += strTRStart + strTDLableStart + "Operational Cost (fix)" + strTDEnd +
                                  strTDValueStart + "" + strTDEnd + strTREnd;

            strHTMLDesignInfo += strTRStart + strTDLableStart + "Amps(fix)" + strTDEnd +
                                 strTDValueStart + "" + strTDEnd + strTREnd;

            strHTMLDesignInfo += strTRStart + strTDLableStart + "Application Name" + strTDEnd +
                                 strTDValueStart + dsForecastAns.Tables[0].Rows[0]["appname"].ToString() + strTDEnd + strTREnd;

            strHTMLDesignInfo += strTRStart + strTDLableStart + "Application Code" + strTDEnd +
                                 strTDValueStart + dsForecastAns.Tables[0].Rows[0]["appcode"].ToString() + strTDEnd + strTREnd;

            strHTMLDesignInfo += strTRStart + strTDLableStart + "Departmental Manager" + strTDEnd +
                                 strTDValueStart + oUser.GetFullName(Int32.Parse(dsForecastAns.Tables[0].Rows[0]["appcontact"].ToString())) + strTDEnd + strTREnd;

            strHTMLDesignInfo += strTRStart + strTDLableStart + "Application Technical Lead" + strTDEnd +
                                 strTDValueStart + oUser.GetFullName(Int32.Parse(dsForecastAns.Tables[0].Rows[0]["admin1"].ToString())) + strTDEnd + strTREnd;

            strHTMLDesignInfo += strTRStart + strTDLableStart + "Administrative Contact" + strTDEnd +
                                 strTDValueStart + oUser.GetFullName(Int32.Parse(dsForecastAns.Tables[0].Rows[0]["admin2"].ToString())) + strTDEnd + strTREnd;

            strHTMLDesignInfo += strTRStart + strTDLableStart + "Application Owner" + strTDEnd +
                                 strTDValueStart + oUser.GetFullName(Int32.Parse(dsForecastAns.Tables[0].Rows[0]["appowner"].ToString())) + strTDEnd + strTREnd;

            strHTMLDesignInfo += strTableEnd;

            strHTMLDesignInfo += strTableStart;

            strHTMLDesignInfo += strTRStart + strTDLableStart1 + "What type of operating system would you like ?(fix)" + strTDEnd + strTREnd +
                                 strTRStart + strTDStart + oUser.GetFullName(Int32.Parse(dsForecastAns.Tables[0].Rows[0]["admin2"].ToString())) + strTDEnd + strTREnd;

            strHTMLDesignInfo += strTRStart + strTDLableStart1 + "Does this device require web hosting software to be installed ?(IIS, WebSphere, Apache etc..) (fix)" + strTDEnd + strTREnd +
                                 strTRStart + strTDStart + oUser.GetFullName(Int32.Parse(dsForecastAns.Tables[0].Rows[0]["admin2"].ToString())) + strTDEnd + strTREnd;

            strHTMLDesignInfo += strTRStart + strTDLableStart1 + "What type of database would you like?(fix)" + strTDEnd + strTREnd +
                                 strTRStart + strTDStart + oUser.GetFullName(Int32.Parse(dsForecastAns.Tables[0].Rows[0]["admin2"].ToString())) + strTDEnd + strTREnd;

            strHTMLDesignInfo += strTRStart + strTDLableStart1 + "Select your high availability method?(fix) " + strTDEnd + strTREnd +
                                 strTRStart + strTDStart + oUser.GetFullName(Int32.Parse(dsForecastAns.Tables[0].Rows[0]["admin2"].ToString())) + strTDEnd + strTREnd;

            strHTMLDesignInfo += strTableEnd;


        }

        private void GetForecastInfoGeneral( DataSet ds)
        {

            int intUser = Int32.Parse(ds.Tables[0].Rows[0]["userid"].ToString());
            int intClass = Int32.Parse(ds.Tables[0].Rows[0]["classid"].ToString());
            int intEnv = Int32.Parse(ds.Tables[0].Rows[0]["environmentid"].ToString());
            int intAddress = Int32.Parse(ds.Tables[0].Rows[0]["addressid"].ToString());
            int intPlatform = Int32.Parse(ds.Tables[0].Rows[0]["platformid"].ToString());
            int intMaintenance = Int32.Parse(ds.Tables[0].Rows[0]["maintenanceid"].ToString());
            int intApp = Int32.Parse(ds.Tables[0].Rows[0]["applicationid"].ToString());
            int intModel = Int32.Parse(ds.Tables[0].Rows[0]["modelid"].ToString());
            if (intModel == 0)
                intModel = oForecast.GetModelAsset(intDesignId);
            if (intModel == 0)
                intModel = oForecast.GetModel(intDesignId);
            int intType = 0;
            if (intModel > 0)
                intType = oModelsProperties.GetType(intModel);
            int intConfidence = Int32.Parse(ds.Tables[0].Rows[0]["confidenceid"].ToString());
            int intQuantity = Int32.Parse(ds.Tables[0].Rows[0]["quantity"].ToString());
            
            #region Forecast General
                    strHTMLForecastInfoGeneral += strTableStart;
                    strHTMLForecastInfoGeneral += strTRTableHeader + strTDTableHeader + "General " + strTDEnd + strTREnd;
                    strHTMLForecastInfoGeneral += strTableEnd;

                    strHTMLForecastInfoGeneral += strTableStart;


                    strHTMLForecastInfoGeneral += strTRStart + strTDLableStart + "Requested By" + strTDEnd +
                                            strTDValueStart + oServer.GetExecutionUser(Int32.Parse(ds.Tables[0].Rows[0]["userid"].ToString())) + strTDEnd + strTREnd;

                    strHTMLForecastInfoGeneral += strTRStart + strTDLableStart + "Last Updated" + strTDEnd +
                                            strTDValueStart +   ds.Tables[0].Rows[0]["modified"].ToString() + strTDEnd + strTREnd;

                    strHTMLForecastInfoGeneral += strTRStart + strTDLableStart +"Nickname"+ strTDEnd +
                                            strTDValueStart +    ds.Tables[0].Rows[0]["name"].ToString() + strTDEnd + strTREnd;

                    if (ds.Tables[0].Rows[0]["override"].ToString() == "1" || ds.Tables[0].Rows[0]["override"].ToString() == "-1" || ds.Tables[0].Rows[0]["override"].ToString() == "-10")
                    {
                        strHTMLForecastInfoGeneral += strTRStart + strTDLableStart +" Override Selection Matrix"+ strTDEnd +strTDValueStart+"Yes" + strTDEnd + strTREnd;

                        int intOverrideBy = Int32.Parse(ds.Tables[0].Rows[0]["overrideby"].ToString());
                        if (ds.Tables[0].Rows[0]["override"].ToString() == "-1")
                            strHTMLForecastInfoGeneral  += strTRStart + strTDLableStart +"Override Approval"+ strTDEnd +strTDValueStart+"hold" + strTDEnd + strTREnd;
                        else if (ds.Tables[0].Rows[0]["override"].ToString() == "-10")
                        {
                            strHTMLForecastInfoGeneral += strTRStart + strTDLableStart +"Override Approval"+ strTDEnd +strTDValueStart+"Denied" + strTDEnd + strTREnd;
                            strHTMLForecastInfoGeneral += strTRStart + strTDLableStart +"Override Comments"+ strTDEnd +strTDValueStart+ ds.Tables[0].Rows[0]["comments"].ToString()  + strTDEnd + strTREnd;
                            strHTMLForecastInfoGeneral += strTRStart + strTDLableStart +"Override Denied By"+ strTDEnd +strTDValueStart+   oServer.GetExecutionUser(intOverrideBy) + strTDEnd + strTREnd;
                        }
                        else
                        {
                            strHTMLForecastInfoGeneral += strTRStart + strTDLableStart +"Override Approval"+ strTDEnd +strTDValueStart+"Approved" + strTDEnd + strTREnd;
                            strHTMLForecastInfoGeneral += strTRStart + strTDLableStart +"Override Comments"+ strTDEnd +strTDValueStart+ ds.Tables[0].Rows[0]["comments"].ToString()  + strTDEnd + strTREnd;
                            strHTMLForecastInfoGeneral += strTRStart + strTDLableStart +"Override Approved By"+ strTDEnd +strTDValueStart+ oServer.GetExecutionUser(intOverrideBy)  + strTDEnd + strTREnd;
                        }
                        if (ds.Tables[0].Rows[0]["breakfix"].ToString() == "1")
                        {
                            strHTMLForecastInfoGeneral  += strTRStart + strTDLableStart +"Is this related to a production break-fix issue"+ strTDEnd +strTDValueStart+"Yes" + strTDEnd + strTREnd;
                            strHTMLForecastInfoGeneral  += strTRStart + strTDLableStart +"Change Control #"+ strTDEnd +strTDValueStart+  ds.Tables[0].Rows[0]["change"].ToString()  + strTDEnd + strTREnd;
                            int intName = Int32.Parse(ds.Tables[0].Rows[0]["nameid"].ToString());
                            if (oClass.Get(intClass, "pnc") == "1")
                                strHTMLForecastInfoGeneral  += strTRStart + strTDLableStart +"Device Name"+ strTDEnd +strTDValueStart + oServerName.GetNameFactory(intName, 0)  + strTDEnd + strTREnd;
                            else
                                strHTMLForecastInfoGeneral  += strTRStart + strTDLableStart +"Device Name"+ strTDEnd +strTDValueStart+ oServerName.GetName(intName, 0) +  strTDEnd + strTREnd;
                        }
                        else
                            strHTMLForecastInfoGeneral += strTRStart + strTDLableStart +"Is this related to a production break-fix issue"+ strTDEnd +strTDValueStart+"No" + strTDEnd + strTREnd;
                    }
                    else
                        strHTMLForecastInfoGeneral += strTRStart + strTDLableStart +"Override Selection Matrix"+ strTDEnd +strTDValueStart+"No" + strTDEnd + strTREnd;

                    strHTMLForecastInfoGeneral += strTRStart + strTDLableStart + "Model" + strTDEnd + strTDValueStart + oModelsProperties.Get(intModel, "name") + strTDEnd + strTREnd;

                    double dblA = 0.00;
                    DataSet dsA = oForecast.GetAcquisitions(intModel, 1);
                    foreach (DataRow drA in dsA.Tables[0].Rows)
                        dblA += double.Parse(drA["cost"].ToString());
                    //Get the model details
                    strHTMLForecastInfoGeneral += strTRStart + strTDLableStart + "Acquistion Cost" + strTDEnd + strTDValueStart + dblA.ToString() + strTDEnd + strTREnd;

                   
                    
                    double dblO = 0.00;
                    DataSet dsO = oForecast.GetOperations(intModel, 1);
                    foreach (DataRow drO in dsO.Tables[0].Rows)
                        dblO += double.Parse(drO["cost"].ToString());

                    strHTMLForecastInfoGeneral += strTRStart + strTDLableStart + "Operational Cost" + strTDEnd + strTDValueStart + dblO.ToString() + strTDEnd + strTREnd;
                    strHTMLForecastInfoGeneral += strTRStart + strTDLableStart + "Amps" + strTDEnd + strTDValueStart + oModelsProperties.Get(intModel, "amp").ToString()+" AMPs" + strTDEnd + strTREnd;


                    strHTMLForecastInfoGeneral += strTRStart + strTDLableStart + "Location" + strTDEnd + strTDValueStart + oLocation.GetFull(intAddress) + strTDEnd + strTREnd;
                    strHTMLForecastInfoGeneral += strTRStart + strTDLableStart + "Class" + strTDEnd + strTDValueStart + oClass.Get(intClass, "name") + strTDEnd + strTREnd;
                    if (oClass.IsProd(intClass))
                        strHTMLForecastInfoGeneral += strTRStart + strTDLableStart + "Will this device go through TEST first" + strTDEnd + strTDValueStart + (ds.Tables[0].Rows[0]["test"].ToString() == "1" ? "Yes" : "No")  +strTDEnd + strTREnd;
                    strHTMLForecastInfoGeneral += strTRStart + strTDLableStart + "Environment" + strTDEnd + strTDValueStart + oEnvironment.Get(intEnv, "name") + strTDEnd + strTREnd;
                    strHTMLForecastInfoGeneral += strTRStart + strTDLableStart + "Maintenance Window" + strTDEnd + strTDValueStart + (intMaintenance == 0 ? "N / A" : oMaintenanceWindow.Get(intMaintenance, "name")) + strTDEnd + strTREnd;
                    strHTMLForecastInfoGeneral += strTRStart + strTDLableStart + "Server Type" + strTDEnd + strTDValueStart + (intApp == 0 ? "N / A" : oServerName.GetApplication(intApp, "name")) + strTDEnd + strTREnd;
                    strHTMLForecastInfoGeneral += strTRStart + strTDLableStart + "Quantity" + strTDEnd + strTDValueStart + intQuantity.ToString() + strTDEnd + strTREnd;
                    if (ds.Tables[0].Rows[0]["implementation"].ToString() == "")
                        strHTMLForecastInfoGeneral += strTRStart + strTDLableStart + "Commitment Date" + strTDEnd + strTDValueStart + "---" + strTDEnd + strTREnd;
                    else
                        strHTMLForecastInfoGeneral += strTRStart + strTDLableStart + "Commitment Date" + strTDEnd + strTDValueStart + DateTime.Parse(ds.Tables[0].Rows[0]["implementation"].ToString()).ToShortDateString() + strTDEnd + strTREnd;
                    strHTMLForecastInfoGeneral += strTRStart + strTDLableStart + "Confidence" + strTDEnd + strTDValueStart + (intConfidence == 0 ? "N / A" : oConfidence.Get(intConfidence, "name")) + strTDEnd + strTREnd;
                    strHTMLForecastInfoGeneral += strTRStart + strTDLableStart + "Recovery Quantity" + strTDEnd + strTDValueStart + ds.Tables[0].Rows[0]["recovery_number"].ToString() + strTDEnd + strTREnd;
                    


                    strHTMLForecastInfoGeneral += strTableEnd;
            #endregion

            #region Forecast Platform

                    strHTMLForecastInfoPlatform += strTableStart;
                    strHTMLForecastInfoPlatform += strTRTableHeader + strTDTableHeader + "Platform " + strTDEnd + strTREnd;
                    strHTMLForecastInfoPlatform += strTableEnd;

                    


                    if (ds.Tables[0].Rows[0]["override"].ToString() == "1" || ds.Tables[0].Rows[0]["override"].ToString() == "-1" || ds.Tables[0].Rows[0]["override"].ToString() == "-10")
                    {
                        strHTMLForecastInfoPlatform += strTableStart;
                        strHTMLForecastInfoPlatform += strTRStart + strTDLableStart + "Selection Matrix Override! " + strTDEnd + strTREnd;
                        strHTMLForecastInfoPlatform += strTRStart + strTDLableStart + "Type " + strTDEnd + strTDValueStart + oType.Get(intType, "name") + strTDEnd + strTREnd;
                        strHTMLForecastInfoPlatform += strTRStart + strTDLableStart + "Model" + strTDEnd + strTDValueStart + oModelsProperties.Get(intModel, "name")  +strTDEnd + strTREnd;
                        strHTMLForecastInfoPlatform += strTRStart + strTDLableStart + "&nbsp;" + strTDEnd + strTREnd;
                        strHTMLForecastInfoPlatform += strTableEnd;
                    }
                    DataSet dsQuestions = oForecast.GetQuestionPlatform(intPlatform, intClass, intEnv);
                    if (dsQuestions.Tables[0].Rows.Count > 0)
                    {
                        strHTMLForecastInfoPlatform += strTableStart;
                        foreach (DataRow drQuestion in dsQuestions.Tables[0].Rows)
                        {
                            string strResponsePDF = "";
                            int intQuestion = Int32.Parse(drQuestion["id"].ToString());
                            DataSet dsAnswers = oForecast.GetAnswerPlatform(intDesignId, intQuestion);
                            foreach (DataRow drAnswer in dsAnswers.Tables[0].Rows)
                            {
                                if (strResponsePDF != "")
                                    strResponsePDF += ", ";
                                strResponsePDF += oForecast.GetResponse(Int32.Parse(drAnswer["responseid"].ToString()), "response");
                            }
                            if (strResponsePDF != "")
                            {
                                strHTMLForecastInfoPlatform += strTRStart + strTDLableStart1 + drQuestion["question"].ToString() + strTDEnd + strTREnd;
                                strHTMLForecastInfoPlatform += strTRStart + strTDStart + strResponsePDF + strTDEnd + strTREnd;
                            }
                        }
                        strHTMLForecastInfoPlatform += strTableEnd;
                    }
                    
                   

            #endregion

            #region Forecast Storage

                    bool boolProduction = (oClass.IsProd(intClass));
                    bool boolQA = (oClass.IsQA(intClass));
                    bool boolNone = (ds.Tables[0].Rows[0]["storage"].ToString() == "-2");
                    bool boolRequired = oForecast.IsHACluster(intDesignId);
                    bool boolNoReplication = oForecast.IsDROver48(intDesignId, false);

                    strHTMLForecastInfoStorage += strTableStart;
                    strHTMLForecastInfoStorage += strTRTableHeader + strTDTableHeader + "Storage " + strTDEnd + strTREnd;
                    strHTMLForecastInfoStorage += strTableEnd;



                    if (ds.Tables[0].Rows[0]["storage"].ToString() == "1")
                    {
                        strHTMLForecastInfoStorage += strTableStart;
                        strHTMLForecastInfoStorage += strTRStart + strTDLableStart1 + "Operating System Volumes" + strTDEnd + strTREnd;
                        strHTMLForecastInfoStorage += strTableEnd;
                       
                        // OS Volumes
                        DataSet dsStorageOS = oForecast.GetStorageOS(intDesignId);
                        if (dsStorageOS.Tables[0].Rows.Count > 0)
                        {
                            strHTMLForecastInfoStorage += strTableStart;
                            strHTMLForecastInfoStorage += strTRTableHeader;
                            strHTMLForecastInfoStorage += strTDLableStart1 + "Class" + strTDEnd;
                            strHTMLForecastInfoStorage += strTDLableStart1 + "Performance" + strTDEnd;
                            strHTMLForecastInfoStorage += strTDLableStart1 + "Amount" + strTDEnd;
                            strHTMLForecastInfoStorage += strTDLableStart1 + "Availability" + strTDEnd;
                            strHTMLForecastInfoStorage += strTDLableStart1 + "Replication" + strTDEnd;
                            strHTMLForecastInfoStorage += strTDLableStart1 + "Amount Replicated" + strTDEnd;
                            strHTMLForecastInfoStorage += strTDLableStart1 + "HA" + strTDEnd;
                            strHTMLForecastInfoStorage += strTDLableStart1 + "Amount HA" + strTDEnd;
                            strHTMLForecastInfoStorage += strTREnd;
                            if (dsStorageOS.Tables[0].Rows[0]["high"].ToString() == "1")
                            {
                                strHTMLForecastInfoStorage += AddForecastStorage("Production", "High", dsStorageOS.Tables[0].Rows[0]["high_total"].ToString(), dsStorageOS.Tables[0].Rows[0]["high_level"].ToString(), dsStorageOS.Tables[0].Rows[0]["high_replicated"].ToString(), dsStorageOS.Tables[0].Rows[0]["high_ha"].ToString());
                                strHTMLForecastInfoStorage += AddForecastStorage("QA", "High", dsStorageOS.Tables[0].Rows[0]["high_qa"].ToString(), "", "", "");
                                strHTMLForecastInfoStorage += AddForecastStorage("Test", "High", dsStorageOS.Tables[0].Rows[0]["high_test"].ToString(), "", "", "");
                            }
                            if (dsStorageOS.Tables[0].Rows[0]["standard"].ToString() == "1")
                            {
                                strHTMLForecastInfoStorage += AddForecastStorage("Production", "Standard", dsStorageOS.Tables[0].Rows[0]["standard_total"].ToString(), dsStorageOS.Tables[0].Rows[0]["standard_level"].ToString(), dsStorageOS.Tables[0].Rows[0]["standard_replicated"].ToString(), dsStorageOS.Tables[0].Rows[0]["standard_ha"].ToString());
                                strHTMLForecastInfoStorage += AddForecastStorage("QA", "Standard", dsStorageOS.Tables[0].Rows[0]["standard_qa"].ToString(), "", "", "");
                                strHTMLForecastInfoStorage += AddForecastStorage("Test", "Standard", dsStorageOS.Tables[0].Rows[0]["standard_test"].ToString(), "", "", "");
                            }
                            if (dsStorageOS.Tables[0].Rows[0]["low"].ToString() == "1")
                            {
                                strHTMLForecastInfoStorage += AddForecastStorage("Production", "Low", dsStorageOS.Tables[0].Rows[0]["low_total"].ToString(), dsStorageOS.Tables[0].Rows[0]["low_level"].ToString(), dsStorageOS.Tables[0].Rows[0]["low_replicated"].ToString(), dsStorageOS.Tables[0].Rows[0]["low_ha"].ToString());
                                strHTMLForecastInfoStorage += AddForecastStorage("QA", "Low", dsStorageOS.Tables[0].Rows[0]["low_qa"].ToString(), "", "", "");
                                strHTMLForecastInfoStorage += AddForecastStorage("Test", "Low", dsStorageOS.Tables[0].Rows[0]["low_test"].ToString(), "", "", "");
                            }

                            strHTMLForecastInfoStorage += strTableEnd;
                        }
                        else
                        {
                           strHTMLForecastInfoStorage += strTableStart;
                           strHTMLForecastInfoStorage += strTRStart + strTDValueStart + "No information" + strTDEnd + strTREnd;
                           strHTMLForecastInfoStorage += strTableEnd;
                           
                       }


                        strHTMLForecastInfoStorage += strTableStart;
                        strHTMLForecastInfoStorage += strTRStart + strTDLableStart1 + "Application / Data Volumes" + strTDEnd + strTREnd;
                        strHTMLForecastInfoStorage += strTableEnd;
                       

                        // Application / Data Volumes
                        DataSet dsStorage = oForecast.GetStorage(intDesignId);
                        if (dsStorage.Tables[0].Rows.Count > 0)
                        {
                            strHTMLForecastInfoStorage += strTableStart;
                            strHTMLForecastInfoStorage += strTRTableHeader;
                            strHTMLForecastInfoStorage += strTDLableStart1 + "Class" + strTDEnd;
                            strHTMLForecastInfoStorage += strTDLableStart1 + "Performance" + strTDEnd;
                            strHTMLForecastInfoStorage += strTDLableStart1 + "Amount" + strTDEnd;
                            strHTMLForecastInfoStorage += strTDLableStart1 + "Availability" + strTDEnd;
                            strHTMLForecastInfoStorage += strTDLableStart1 + "Replication" + strTDEnd;
                            strHTMLForecastInfoStorage += strTDLableStart1 + "Amount Replicated" + strTDEnd;
                            strHTMLForecastInfoStorage += strTDLableStart1 + "HA" + strTDEnd;
                            strHTMLForecastInfoStorage += strTDLableStart1 + "Amount HA" + strTDEnd;
                            strHTMLForecastInfoStorage += strTREnd;
                            if (dsStorage.Tables[0].Rows[0]["high"].ToString() == "1")
                            {
                                strHTMLForecastInfoStorage += AddForecastStorage("Production", "High", dsStorage.Tables[0].Rows[0]["high_total"].ToString(), dsStorage.Tables[0].Rows[0]["high_level"].ToString(), dsStorage.Tables[0].Rows[0]["high_replicated"].ToString(), dsStorage.Tables[0].Rows[0]["high_ha"].ToString());
                                strHTMLForecastInfoStorage += AddForecastStorage("QA", "High", dsStorage.Tables[0].Rows[0]["high_qa"].ToString(), "", "", "");
                                strHTMLForecastInfoStorage += AddForecastStorage("Test", "High", dsStorage.Tables[0].Rows[0]["high_test"].ToString(), "", "", "");
                            }
                            if (dsStorage.Tables[0].Rows[0]["standard"].ToString() == "1")
                            {
                                strHTMLForecastInfoStorage += AddForecastStorage("Production", "Standard", dsStorage.Tables[0].Rows[0]["standard_total"].ToString(), dsStorage.Tables[0].Rows[0]["standard_level"].ToString(), dsStorage.Tables[0].Rows[0]["standard_replicated"].ToString(), dsStorage.Tables[0].Rows[0]["standard_ha"].ToString());
                                strHTMLForecastInfoStorage += AddForecastStorage("QA", "Standard", dsStorage.Tables[0].Rows[0]["standard_qa"].ToString(), "", "", "");
                                strHTMLForecastInfoStorage += AddForecastStorage("Test", "Standard", dsStorage.Tables[0].Rows[0]["standard_test"].ToString(), "", "", "");
                            }
                            if (dsStorage.Tables[0].Rows[0]["low"].ToString() == "1")
                            {
                                strHTMLForecastInfoStorage += AddForecastStorage("Production", "Low", dsStorage.Tables[0].Rows[0]["low_total"].ToString(), dsStorage.Tables[0].Rows[0]["low_level"].ToString(), dsStorage.Tables[0].Rows[0]["low_replicated"].ToString(), dsStorage.Tables[0].Rows[0]["low_ha"].ToString());
                                strHTMLForecastInfoStorage += AddForecastStorage("QA", "Low", dsStorage.Tables[0].Rows[0]["low_qa"].ToString(), "", "", "");
                                strHTMLForecastInfoStorage += AddForecastStorage("Test", "Low", dsStorage.Tables[0].Rows[0]["low_test"].ToString(), "", "", "");
                            }
                            strHTMLForecastInfoStorage += strTableEnd;
                        }
                    }
                    else
                    {
                        strHTMLForecastInfoStorage += strTableStart;
                            strHTMLForecastInfoStorage += strTRStart + strTDValueStart + "There was no storage requested!" + strTDEnd + strTREnd;
                        strHTMLForecastInfoStorage += strTableEnd;
                    }
                    


                    

            #endregion

            #region Forecast Backup
                    DataSet dsBackup = oForecast.GetBackup(intDesignId);

                    strHTMLForecastBackupInfo += strTableStart;
                    strHTMLForecastBackupInfo += strTRTableHeader + strTDTableHeader + "Backup " + strTDEnd + strTREnd;
                    strHTMLForecastBackupInfo += strTableEnd;

                    strHTMLForecastBackupInfo += strTableStart;
                    strHTMLForecastBackupInfo += strTRTableHeader + strTDLableStart1 + "Backup Information" + strTDEnd + strTREnd;
                    strHTMLForecastBackupInfo += strTableEnd;



                    if (dsBackup.Tables[0].Rows.Count == 1)
                    {
                        strHTMLForecastBackupInfo += strTableStart;

                        if (dsBackup.Tables[0].Rows[0]["recoveryid"].ToString() != "")
                            strHTMLForecastBackupInfo += strTRStart + strTDLableStart + "Recovery Location" + strTDEnd + strTDValueStart + oLocation.GetFull(Int32.Parse(dsBackup.Tables[0].Rows[0]["recoveryid"].ToString())) + strTDEnd + strTREnd;
                        if (dsBackup.Tables[0].Rows[0]["daily"].ToString() == "1")
                            strHTMLForecastBackupInfo += strTRStart + strTDLableStart + "Timing/Frequency of Backups" + strTDEnd + strTDValueStart + "Daily" + strTDEnd + strTREnd;
                        else if (dsBackup.Tables[0].Rows[0]["weekly"].ToString() == "1")
                            strHTMLForecastBackupInfo += strTRStart + strTDLableStart + "Timing/Frequency of Backups" + strTDEnd + strTDValueStart + "Weekly" + strTDEnd + strTREnd;
                        else if (dsBackup.Tables[0].Rows[0]["monthly"].ToString() == "1")
                            strHTMLForecastBackupInfo += strTRStart + strTDLableStart + "Timing/Frequency of Backups" + strTDEnd + strTDValueStart + "Monthly" + strTDEnd + strTREnd;
                        if (dsBackup.Tables[0].Rows[0]["time"].ToString() == "1")
                            strHTMLForecastBackupInfo += strTRStart + strTDLableStart + "Start Time" + strTDEnd + strTDValueStart + dsBackup.Tables[0].Rows[0]["time_hour"].ToString() + " " + dsBackup.Tables[0].Rows[0]["time_switch"].ToString() + strTDEnd + strTREnd;
                        else
                            strHTMLForecastBackupInfo += strTRStart + strTDLableStart + "Start Time" + strTDEnd + strTDValueStart + "Don't Care" + strTDEnd + strTREnd;

                        strHTMLForecastBackupInfo += strTRStart + strTDLableStart + "Total Combined Disk Capacity (GB)" + strTDEnd + strTDValueStart + dblStorageAmount.ToString("0") + " GB" + strTDEnd + strTREnd;
                        strHTMLForecastBackupInfo += strTRStart + strTDLableStart + "Current Combined Disk Utilized (GB)" + strTDEnd + strTDValueStart + "5 GB" + strTDEnd + strTREnd;
                        strHTMLForecastBackupInfo += strTRStart + strTDLableStart + "Average Size of One Typical Data File" + strTDEnd + strTDValueStart + dsBackup.Tables[0].Rows[0]["average_one"].ToString() + " GB" + strTDEnd + strTREnd;
                        if (dsBackup.Tables[0].Rows[0]["documentation"].ToString() == "")
                            strHTMLForecastBackupInfo += strTRStart + strTDLableStart + "Production Turnover Documentation" + strTDEnd + strTDValueStart + "Not Specified" + strTDEnd + strTREnd;
                        else
                            strHTMLForecastBackupInfo += strTRStart + strTDLableStart + "Production Turnover Documentation" + strTDEnd + strTDValueStart + dsBackup.Tables[0].Rows[0]["documentation"].ToString() + strTDEnd + strTREnd;

                        strHTMLForecastBackupInfo += strTableEnd;
                    }
                    else
                    {
                        strHTMLForecastBackupInfo += strTableStart;
                        strHTMLForecastBackupInfo += strTRStart + strTDValueStart + "There was no backup requested!" + strTDEnd + strTREnd;
                        strHTMLForecastBackupInfo += strTableEnd;
                    }
               #endregion

              
            #region Backup Inclusions
                    rptInclusions.DataSource = oForecast.GetBackupInclusions(intDesignId);
                    rptInclusions.DataBind();
                    lblNoneInclusions.Visible = rptInclusions.Items.Count == 0;

     
            #endregion

            #region Backup Exclusions
                    rptExclusions.DataSource = oForecast.GetBackupExclusions(intDesignId);
                    rptExclusions.DataBind();
                    lblNoneExclusions.Visible = rptExclusions.Items.Count == 0;
            #endregion

            #region Archive Requirements
                    rptRetention.DataSource = oForecast.GetBackupRetentions(intDesignId);
                    rptRetention.DataBind();
                    lblNoneRetention.Visible = rptRetention.Items.Count == 0;
            #endregion

            #region Additional Configuration
                    if (dsBackup.Tables[0].Rows.Count == 1)
                    {
                        strHTMLForecastBackupAddInfo += strTableStart;
                        strHTMLForecastBackupAddInfo += strTRTableHeader + strTDLableStart1 + "Backup Information" + strTDEnd + strTREnd;
                        strHTMLForecastBackupAddInfo += strTableEnd;

                        strHTMLForecastBackupAddInfo += strTableStart;
                        strHTMLForecastBackupAddInfo += strTRStart + strTDLableStart + "Average Size of One Data File" + strTDEnd + strTDValueStart +
                                                    (dsBackup.Tables[0].Rows[0]["average_one"] != DBNull.Value ? dsBackup.Tables[0].Rows[0]["average_one"].ToString() : "0") + strTDEnd + strTREnd;

                        strHTMLForecastBackupAddInfo += strTRStart + strTDLableStart + "Production Turnover Documentation Folder Name" + strTDEnd + strTDValueStart +
                                                    (dsBackup.Tables[0].Rows[0]["documentation"] != DBNull.Value ? dsBackup.Tables[0].Rows[0]["documentation"].ToString() : "NA") + strTDEnd + strTREnd;
                        strHTMLForecastBackupAddInfo += strTableEnd;

                        strHTMLForecastBackupAddInfo += strTableStart;
                        strHTMLForecastBackupAddInfo += strTRStart + strTDLableStart1 + "Client File System Data" + strTDEnd + strTREnd;
                        strHTMLForecastBackupAddInfo += strTableEnd;

                        strHTMLForecastBackupAddInfo += strTableStart;

                        strHTMLForecastBackupAddInfo += strTRStart + strTDLableStart + "Percent Changed Daily" + strTDEnd + strTDValueStart +
                                (dsBackup.Tables[0].Rows[0]["cf_percent"] != DBNull.Value ? dsBackup.Tables[0].Rows[0]["cf_percent"].ToString() + "%" : "") + strTDEnd + strTREnd;

                        strHTMLForecastBackupAddInfo += strTRStart + strTDLableStart + "Compression Ratio" + strTDEnd + strTDValueStart +
                                            (dsBackup.Tables[0].Rows[0]["cf_compression"] != DBNull.Value ? dsBackup.Tables[0].Rows[0]["cf_compression"].ToString() + "%" : "") + strTDEnd + strTREnd;

                        strHTMLForecastBackupAddInfo += strTRStart + strTDLableStart + "Average File Size" + strTDEnd + strTDValueStart +
                                (dsBackup.Tables[0].Rows[0]["cf_average"] != DBNull.Value ? dsBackup.Tables[0].Rows[0]["cf_average"].ToString() : "") + strTDEnd + strTREnd;

                        strHTMLForecastBackupAddInfo += strTRStart + strTDLableStart + "Backup Version Ratio" + strTDEnd + strTDValueStart +
                                (dsBackup.Tables[0].Rows[0]["cf_backup"] != DBNull.Value ? dsBackup.Tables[0].Rows[0]["cf_backup"].ToString() : "") + strTDEnd + strTREnd;

                        strHTMLForecastBackupAddInfo += strTRStart + strTDLableStart + "Archive Ratio" + strTDEnd + strTDValueStart +
                                (dsBackup.Tables[0].Rows[0]["cf_archive"] != DBNull.Value ? dsBackup.Tables[0].Rows[0]["cf_archive"].ToString() : "") + strTDEnd + strTREnd;

                        strHTMLForecastBackupAddInfo += strTRStart + strTDLableStart + "Backup Window (Hours)" + strTDEnd + strTDValueStart +
                                        (dsBackup.Tables[0].Rows[0]["cf_window"] != DBNull.Value ? dsBackup.Tables[0].Rows[0]["cf_window"].ToString() + " (Hours)" : "") + strTDEnd + strTREnd;

                        strHTMLForecastBackupAddInfo += strTRStart + strTDLableStart + "Backupsets" + strTDEnd + strTDValueStart +
                                        (dsBackup.Tables[0].Rows[0]["cf_sets"] != DBNull.Value ? dsBackup.Tables[0].Rows[0]["cf_sets"].ToString() : "") + strTDEnd + strTREnd;

                        strHTMLForecastBackupAddInfo += strTableEnd;

                        strHTMLForecastBackupAddInfo += strTableStart;
                        strHTMLForecastBackupAddInfo += strTRStart + strTDLableStart1 + "Client Database Data" + strTDEnd + strTREnd;
                        strHTMLForecastBackupAddInfo += strTableEnd;

                        strHTMLForecastBackupAddInfo += strTableStart;

                        strHTMLForecastBackupAddInfo += strTRStart + strTDLableStart + "Database Type" + strTDEnd + strTDValueStart +
                                               (dsBackup.Tables[0].Rows[0]["cd_type"] != DBNull.Value ? dsBackup.Tables[0].Rows[0]["cd_type"].ToString() : "") + strTDEnd + strTREnd;

                        strHTMLForecastBackupAddInfo += strTRStart + strTDLableStart + "Percent Changed Daily" + strTDEnd + strTDValueStart +
                                               (dsBackup.Tables[0].Rows[0]["cd_percent"] != DBNull.Value ? dsBackup.Tables[0].Rows[0]["cd_percent"].ToString() + "%" : "") + strTDEnd + strTREnd;

                        strHTMLForecastBackupAddInfo += strTRStart + strTDLableStart + "Compression Ratio" + strTDEnd + strTDValueStart +
                                               (dsBackup.Tables[0].Rows[0]["cd_compression"] != DBNull.Value ? dsBackup.Tables[0].Rows[0]["cd_compression"].ToString() : "") + strTDEnd + strTREnd;

                        strHTMLForecastBackupAddInfo += strTRStart + strTDLableStart + "Number of Backup Versions" + strTDEnd + strTDValueStart +
                                               (dsBackup.Tables[0].Rows[0]["cd_versions"] != DBNull.Value ? dsBackup.Tables[0].Rows[0]["cd_versions"].ToString() : "") + strTDEnd + strTREnd;

                        strHTMLForecastBackupAddInfo += strTRStart + strTDLableStart + "Backup Window (Hours)" + strTDEnd + strTDValueStart +
                                                                   (dsBackup.Tables[0].Rows[0]["cd_window"] != DBNull.Value ? dsBackup.Tables[0].Rows[0]["cd_window"].ToString() : "") + strTDEnd + strTREnd;

                        strHTMLForecastBackupAddInfo += strTRStart + strTDLableStart + "Growth Factor" + strTDEnd + strTDValueStart +
                                                       (dsBackup.Tables[0].Rows[0]["cd_growth"] != DBNull.Value ? dsBackup.Tables[0].Rows[0]["cd_growth"].ToString() : "") + strTDEnd + strTREnd;


                        strHTMLForecastBackupAddInfo += strTableEnd;
                    }
            #endregion

            #region Configuration Details Application

                    strHTMLConfigDetailsApp += strTableStart;
                    strHTMLConfigDetailsApp += strTRTableHeader + strTDTableHeader + "Application" + strTDEnd + strTREnd;
                    strHTMLConfigDetailsApp += strTableEnd;

                    strHTMLConfigDetailsApp += strTableStart;

                    strHTMLConfigDetailsApp += strTRStart + strTDLableStart + "Application Name" + strTDEnd + strTDValueStart + ds.Tables[0].Rows[0]["appname"].ToString() + strTDEnd + strTREnd;
                    if (oClass.Get(intClass, "pnc") == "1")
                    {
                        int intMnemonic = Int32.Parse(ds.Tables[0].Rows[0]["mnemonicid"].ToString());
                        strHTMLConfigDetailsApp += strTRStart + strTDLableStart + "Mnemonic" + strTDEnd + strTDValueStart + oMnemonic.Get(intMnemonic, "name") + strTDEnd + strTREnd;
                    }
                    else
                        strHTMLConfigDetailsApp += strTRStart + strTDLableStart + "Application Code" + strTDEnd + strTDValueStart + ds.Tables[0].Rows[0]["appcode"].ToString() + strTDEnd + strTREnd;
                    int intApplicationClient = Int32.Parse(ds.Tables[0].Rows[0]["appcontact"].ToString());
                    strHTMLConfigDetailsApp += strTRStart + strTDLableStart + "Departmental Manager" + strTDEnd + strTDValueStart + oServer.GetExecutionUser(intApplicationClient) + strTDEnd + strTREnd;
                    int intApplicationPrimary = Int32.Parse(ds.Tables[0].Rows[0]["admin1"].ToString());
                    strHTMLConfigDetailsApp += strTRStart + strTDLableStart + "Application Technical Lead" + strTDEnd + strTDValueStart + oServer.GetExecutionUser(intApplicationPrimary) + strTDEnd + strTREnd;
                    int intApplicationAdministrative = Int32.Parse(ds.Tables[0].Rows[0]["admin2"].ToString());
                    strHTMLConfigDetailsApp += strTRStart + strTDLableStart + "Administrative Contact" + strTDEnd + strTDValueStart + oServer.GetExecutionUser(intApplicationAdministrative) + strTDEnd + strTREnd;
                    int intApplicationEngineer = Int32.Parse(ds.Tables[0].Rows[0]["networkengineer"].ToString());
                    strHTMLConfigDetailsApp += strTRStart + strTDLableStart + "Network Engineer" + strTDEnd + strTDValueStart + oServer.GetExecutionUser(intApplicationEngineer) + strTDEnd + strTREnd;
                    strHTMLConfigDetailsApp += strTRStart + strTDLableStart + "DR Criticality" + strTDEnd + strTDValueStart + (oClass.IsProd(intClass) && ds.Tables[0].Rows[0]["dr_criticality"].ToString() != "0" ? (ds.Tables[0].Rows[0]["dr_criticality"].ToString() == "1" ? "1 - High" : "2 - Low") : "---") + strTDEnd + strTREnd;
                    strHTMLConfigDetailsApp += strTRStart + strTDLableStart + "Change Control #" + strTDEnd + strTDValueStart + (ds.Tables[0].Rows[0]["change"].ToString() != "" ? ds.Tables[0].Rows[0]["change"].ToString() : "---") + strTDEnd + strTREnd;
                    strHTMLConfigDetailsApp += strTRStart + strTDLableStart + "Production Go Live Date" + strTDEnd + strTDValueStart + (oClass.IsProd(intClass) && ds.Tables[0].Rows[0]["production"].ToString() != "" ? DateTime.Parse(ds.Tables[0].Rows[0]["production"].ToString()).ToShortDateString() : "---") + strTDEnd + strTREnd;

                    strHTMLConfigDetailsApp += strTableEnd;
            #endregion

            #region Configuration Details Device

                    DataSet dsServer = oServer.GetAnswer(intDesignId);

                    strHTMLConfigDetailsDevice += strTableStart;
                    strHTMLConfigDetailsDevice += strTRTableHeader + strTDTableHeader + "Device(s)" + strTDEnd + strTREnd;
                    strHTMLConfigDetailsDevice += strTableEnd;

                 
                    
                   
                    for (int ii = 1; ii <= intQuantity; ii++)
                    {
                       
                        string strName = "Device #" + ii.ToString();
                        
                        try
                        {
                            DataRow drServer = dsServer.Tables[0].Rows[ii - 1];
                            int intServer = Int32.Parse(drServer["id"].ToString());
                            int intName = Int32.Parse(drServer["nameid"].ToString());
                            bool boolPNC = (drServer["pnc"].ToString() == "1");
                            if (intName > 0)
                            {
                                if (boolPNC == true)
                                    strName = oServerName.GetNameFactory(intName, 0);
                                else
                                    strName = oServerName.GetName(intName, 0);
                            }

                            
                            strHTMLConfigDetailsDevice += strTableStart;
                            strHTMLConfigDetailsDevice += strTRTableHeader + strTDLableStart1 + strName + strTDEnd + strTREnd;
                            strHTMLConfigDetailsDevice += strTableEnd;

                            int intCluster = Int32.Parse(drServer["clusterid"].ToString());
                            if (intCluster == 0)
                            {
                                strHTMLConfigDetailsDevice += strTableStart;
                                strHTMLConfigDetailsDevice += strTRStart + strTDLableStart + "Operating System" + strTDEnd + strTDValueStart + oOperatingSystem.Get(Int32.Parse(drServer["osid"].ToString()), "name") + strTDEnd + strTREnd;
                                if (oModelsProperties.IsConfigServicePack(intModel) == true)
                                    strHTMLConfigDetailsDevice += strTRStart + strTDLableStart + "Service Pack" + strTDEnd + strTDValueStart + oServicePack.Get(Int32.Parse(drServer["spid"].ToString()), "name") + strTDEnd + strTREnd;
                                if (oModelsProperties.IsConfigVMWareTemplate(intModel) == true)
                                    strHTMLConfigDetailsDevice += strTRStart + strTDLableStart + "VMWare Template" + strTDEnd + strTDValueStart + oVMWare.GetTemplate(Int32.Parse(drServer["templateid"].ToString()), "name") + strTDEnd + strTREnd;
                                if (oModelsProperties.IsConfigMaintenanceLevel(intModel) == true)
                                    strHTMLConfigDetailsDevice += strTRStart + strTDLableStart + "Maintenance Level" + strTDEnd + strTDValueStart + oServicePack.Get(Int32.Parse(drServer["spid"].ToString()), "name") + strTDEnd + strTREnd;
                                string strComponents = "";
                                DataSet dsComponents = oServerName.GetComponentDetailSelected(intServer, 1);
                                foreach (DataRow drComponent in dsComponents.Tables[0].Rows)
                                {
                                    if (strComponents != "")
                                        strComponents += ", ";
                                    strComponents += drComponent["name"].ToString();
                                }
                                if (strComponents == "")
                                    strComponents = "---";
                                strHTMLConfigDetailsDevice += strTRStart + strTDLableStart + "Component Lists" + strTDEnd + strTDValueStart + strComponents  + strTDEnd + strTREnd;
                                int intDBA = 0;
                                if (drServer["dba"].ToString() != "")
                                    intDBA = Int32.Parse(drServer["dba"].ToString());
                                if (intDBA > 0)
                                    strHTMLConfigDetailsDevice += strTRStart + strTDLableStart + "Database Administrator" + strTDEnd + strTDValueStart + oServer.GetExecutionUser(intDBA)  + strTDEnd + strTREnd;
                                strHTMLConfigDetailsDevice += strTRStart + strTDLableStart + "Domain" + strTDEnd + strTDValueStart + oDomain.Get(Int32.Parse(drServer["domainid"].ToString()), "name")  + strTDEnd + strTREnd;
                                if (oForecast.GetAnswer(intDesignId, "test") == "1")
                                    strHTMLConfigDetailsDevice += strTRStart + strTDLableStart + "Domain (Test)" + strTDEnd + strTDValueStart + oDomain.Get(Int32.Parse(drServer["test_domainid"].ToString()), "name")  + strTDEnd + strTREnd;
                                strHTMLConfigDetailsDevice += strTRStart + strTDLableStart + "Infrastructure" + strTDEnd + strTDValueStart + (drServer["infrastructure"].ToString() == "1" ? "Yes" : "No")  + strTDEnd + strTREnd;
                                if (drServer["dr"].ToString() == "1")
                                {
                                    strHTMLConfigDetailsDevice += strTRStart + strTDLableStart + "Has DR Counterpart" + strTDEnd + strTDValueStart +"Yes" + strTDEnd + strTREnd;
                                    if (drServer["dr_exist"].ToString() == "1")
                                    {
                                        strHTMLConfigDetailsDevice += strTRStart + strTDLableStart + "DR server exists" + strTDEnd + strTDValueStart +"Yes" + strTDEnd + strTREnd;
                                        strHTMLConfigDetailsDevice += strTRStart + strTDLableStart + "DR server name" + strTDEnd + strTDValueStart +drServer["dr_name"].ToString()  + strTDEnd + strTREnd;
                                    }
                                    else
                                        strHTMLConfigDetailsDevice += strTRStart + strTDLableStart + "DR server exists" + strTDEnd + strTDValueStart +"No" + strTDEnd + strTREnd;
                                    int intConsistency = 0;
                                    if (drServer["dr_consistencyid"].ToString() != "" && intConsistency > 0)
                                        intConsistency = Int32.Parse(drServer["dr_consistencyid"].ToString());
                                    if (drServer["dr_consistency"].ToString() == "1")
                                        strHTMLConfigDetailsDevice += strTRStart + strTDLableStart + "Consistency Group" + strTDEnd + strTDValueStart + oConsistencyGroup.Get(intConsistency, "name")  + strTDEnd + strTREnd;
                                    else
                                        strHTMLConfigDetailsDevice += strTRStart + strTDLableStart + "Consistency Group" + strTDEnd + strTDValueStart + "---" + strTDEnd + strTREnd;
                                }
                                else
                                    strHTMLConfigDetailsDevice += strTRStart + strTDLableStart + "Has DR Counterpart" + strTDEnd + strTDValueStart + "No" + strTDEnd + strTREnd;
                                strHTMLConfigDetailsDevice += strTableEnd;
                            }
                        }
                        catch
                        {
                            strHTMLConfigDetailsDevice += strTableStart;
                            strHTMLConfigDetailsDevice += strTRTableHeader + strTDLableStart1 + strName + strTDEnd + strTREnd;
                            strHTMLConfigDetailsDevice += strTableEnd;

                            strHTMLConfigDetailsDevice += strTableStart;
                            strHTMLConfigDetailsDevice  += strTRStart + strTDValueStart +  "This device has not been configured."+ strTDEnd + strTREnd;
                            strHTMLConfigDetailsDevice += strTableEnd;
                        } 
                        
                    }

          #endregion

          #region Configuration Details Users

                    strHTMLConfigDetailsUser += strTableStart;
                    strHTMLConfigDetailsUser += strTRTableHeader + strTDTableHeader + "User(s)" + strTDEnd + strTREnd;
                    strHTMLConfigDetailsUser += strTableEnd;

                    strHTMLConfigDetailsUser += strTableStart;
                    strHTMLConfigDetailsUser += strTRTableHeader + strTDLableStart + "User" + strTDEnd + strTDStart + "<b>Permissions</b>" + strTDEnd + strTREnd;
                    strHTMLConfigDetailsUser += strTableEnd;
                    for (int ii = 1; ii <= intQuantity; ii++)
                    {
                       


                        
                        string strName = "Device #" + ii.ToString();
                        try
                        {
                            DataRow drServer = dsServer.Tables[0].Rows[ii - 1];
                            int intServer = Int32.Parse(drServer["id"].ToString());
                            int intName = Int32.Parse(drServer["nameid"].ToString());
                            bool boolPNC = (drServer["pnc"].ToString() == "1");
                            if (intName > 0)
                            {
                                if (boolPNC == true)
                                    strName = oServerName.GetNameFactory(intName, 0);
                                else
                                    strName = oServerName.GetName(intName, 0);
                            }
                            strHTMLConfigDetailsUser += strTableStart;
                            strHTMLConfigDetailsUser += strTRTableHeader + strTDLableStart1 + strName + strTDEnd + strTREnd;
                            strHTMLConfigDetailsUser += strTableEnd;

                            DataSet dsAccount = oServer.GetAccounts(intServer);
                            if (dsAccount.Tables[0].Rows.Count == 0)
                            {
                                strHTMLConfigDetailsUser += strTableStart;
                                strHTMLConfigDetailsUser += strTRStart + strTDValueStart + " The account configuration was skipped for this device." + strTDEnd + strTREnd;
                                strHTMLConfigDetailsUser += strTableEnd;


                            }
                            else
                            {
                                if (boolPNC == true)
                                {
                                    foreach (DataRow drAccount in dsAccount.Tables[0].Rows)
                                    {
                                        int intAccount = oUser.GetId(drAccount["xid"].ToString());
                                        string strAccount = "";

                                        // Domain Groups
                                        string strAccountDomains = drAccount["domaingroups"].ToString();
                                        char[] strAccountSplit = { ';' };
                                        string[] strAccountDomainArray = strAccountDomains.Split(strAccountSplit);
                                        for (int jj = 0; jj < strAccountDomainArray.Length; jj++)
                                        {
                                            if (strAccountDomainArray[jj].Trim() != "")
                                            {
                                                string strAccountDomain = strAccountDomainArray[jj].Trim();
                                                if (strAccountDomain.Contains("_") == true)
                                                {
                                                    strAccount += strAccountDomain.Substring(0, strAccountDomain.IndexOf("_"));
                                                    strAccountDomain = strAccountDomain.Substring(strAccountDomain.IndexOf("_") + 1);
                                                    if (strAccountDomain == "1")
                                                        strAccount += " (Remote Desktop)";
                                                }
                                                else
                                                    strAccount += strAccountDomain;
                                                strAccount += "<br/>";
                                            }
                                        }

                                        // Local Groups
                                        string strAccountLocals = drAccount["localgroups"].ToString();
                                        string[] strAccountLocalArray = strAccountLocals.Split(strAccountSplit);
                                        for (int jj = 0; jj < strAccountLocalArray.Length; jj++)
                                        {
                                            string strAccountLocal = strAccountLocalArray[jj].Trim();
                                            if (strAccountLocal.Contains("_") == true)
                                            {
                                                strAccount += strAccountLocal.Substring(0, strAccountLocal.IndexOf("_"));
                                                strAccountLocal = strAccountLocal.Substring(strAccountLocal.IndexOf("_") + 1);
                                                if (strAccountLocal == "1")
                                                    strAccount += " (Remote Desktop)";
                                            }
                                            else
                                                strAccount += strAccountLocal;
                                            strAccount += "<br/>";
                                        }
                                        if (strAccount == "")
                                            strAccount = "-----";
                                        strHTMLConfigDetailsUser += strTableStart;
                                        strHTMLConfigDetailsUser += strTRStart + strTDLableStart + oServer.GetExecutionUser(intAccount) + strTDEnd + strTDValueStart + strAccount + strTDEnd + strTREnd;
                                        strHTMLConfigDetailsUser += strTableEnd;
                                    }
                                }
                                else
                                {
                                    foreach (DataRow drAccount in dsAccount.Tables[0].Rows)
                                    {
                                        int intAccount = oUser.GetId(drAccount["xid"].ToString());
                                        string strAccount = "";
                                        if (drAccount["admin"].ToString() == "1")
                                            strAccount = "Administrator";
                                        else
                                        {
                                            string strPermission = drAccount["localgroups"].ToString();
                                            if (strPermission.Contains("GLCfsaRO_SysVol"))
                                                strAccount += "SYS_VOL (C:) - Read Only<br/>";
                                            else if (strPermission.Contains("GLCfsaRW_SysVol"))
                                                strAccount += "SYS_VOL (C:) - Read / Write<br/>";
                                            else if (strPermission.Contains("GLCfsaFC_SysVol"))
                                                strAccount += "SYS_VOL (C:) - Full Control<br/>";

                                            if (strPermission.Contains("GLCfsaRO_UtlVol"))
                                                strAccount += "UTL_VOL (E:) - Read Only<br/>";
                                            else if (strPermission.Contains("GLCfsaRW_UtlVol"))
                                                strAccount += "UTL_VOL (E:) - Read / Write<br/>";
                                            else if (strPermission.Contains("GLCfsaFC_UtlVol"))
                                                strAccount += "UTL_VOL (E:) - Full Control<br/>";

                                            if (strPermission.Contains("GLCfsaRO_AppVol"))
                                                strAccount += "APP_VOL (F:) - Read Only<br/>";
                                            else if (strPermission.Contains("GLCfsaRW_AppVol"))
                                                strAccount += "APP_VOL (F:) - Read / Write<br/>";
                                            else if (strPermission.Contains("GLCfsaFC_AppVol"))
                                                strAccount += "APP_VOL (F:) - Full Control<br/>";

                                            if (strAccount == "")
                                                strAccount = "-----";
                                        }
                                        strHTMLConfigDetailsUser += strTableStart;
                                        strHTMLConfigDetailsUser += strTRStart + strTDLableStart + oServer.GetExecutionUser(intAccount) + strTDEnd + strTDValueStart + strAccount + strTDEnd + strTREnd;
                                        strHTMLConfigDetailsUser += strTableEnd;
                                    }
                                }
                            }
                        }
                        catch
                        {
                            strHTMLConfigDetailsUser += strTableStart;
                            strHTMLConfigDetailsUser += strTRTableHeader + strTDLableStart1 + strName + strTDEnd + strTREnd;
                            strHTMLConfigDetailsUser += strTableEnd;

                            strHTMLConfigDetailsUser += strTableStart;
                            strHTMLConfigDetailsUser +=  strTRStart + strTDLableStart +"This device has not been configured."+ strTDEnd + strTREnd;
                            strHTMLConfigDetailsUser += strTableEnd;
                        }
                        

                    }
                    
          #endregion

          #region Configuration Details Storage

                    strHTMLConfigDetailsStorage += strTableStart;
                    strHTMLConfigDetailsStorage += strTRTableHeader + strTDTableHeader + "Storage" + strTDEnd + strTREnd;
                    strHTMLConfigDetailsStorage += strTableEnd;



                   
                    for (int ii = 1; ii <= intQuantity; ii++)
                    {
                      
                        string strName = "Device #" + ii.ToString();
                        try
                        {
                            DataRow drServer = dsServer.Tables[0].Rows[ii - 1];
                            int intServer = Int32.Parse(drServer["id"].ToString());
                            int intName = Int32.Parse(drServer["nameid"].ToString());
                            bool boolPNC = (drServer["pnc"].ToString() == "1");
                            if (intName > 0)
                            {
                                if (boolPNC == true)
                                    strName = oServerName.GetNameFactory(intName, 0);
                                else
                                    strName = oServerName.GetName(intName, 0);
                            }

                            strHTMLConfigDetailsStorage += strTableStart;
                            strHTMLConfigDetailsStorage += strTRTableHeader + strTDLableStart1 + strName + strTDEnd + strTREnd;
                            strHTMLConfigDetailsStorage += strTableEnd;


                            int intCluster = Int32.Parse(drServer["clusterid"].ToString());
                            int intCSM = Int32.Parse(drServer["csmconfigid"].ToString());
                            int intNumber = Int32.Parse(drServer["number"].ToString());
                            strHTMLConfigDetailsStorage += GetStorage(intDesignId, intCluster, intCSM, intNumber, intModel, strName) + GetStorageShared(intDesignId, intModel);
                        }
                        catch
                        {
                            strHTMLConfigDetailsStorage += strTableStart;
                            strHTMLConfigDetailsStorage += strTRTableHeader + strTDLableStart1 + strName + strTDEnd + strTREnd;
                            strHTMLConfigDetailsStorage += strTableEnd;

                            strHTMLConfigDetailsStorage += strTableStart;
                            strHTMLConfigDetailsStorage += strTRTableHeader + strTDLableStart1 + "This device has not been configured." + strTDEnd + strTREnd;
                            strHTMLConfigDetailsStorage += strTableEnd;
                         }
                        
                    }
                    
          #endregion
                }

        private string AddForecastStorage(string _class, string _performance, string _amount, string _level, string _replicated, string _ha)
        {
            string _return = "";
            double dblAmount = 0.00;
            if (_amount != "")
                dblAmount = double.Parse(_amount);
            double dblReplicated = 0.00;
            if (_replicated != "")
                dblReplicated = double.Parse(_replicated);
            double dblHA = 0.00;
            if (_ha != "")
                dblHA = double.Parse(_ha);
            if (dblAmount > 0.00)
            {
                _return += strTRStart;
                _return += strTDStart + _class + strTDEnd;
                _return += strTDStart + _performance + strTDEnd;
                _return += strTDStart + dblAmount.ToString("F") + " GB" + strTDEnd;
                _return += strTDStart + (_level == "" ? "---" : _level) + strTDEnd;
                _return += strTDStart + (_replicated == "" ? "---" : (dblReplicated > 0.00 ? "Yes" : "No")) + strTDEnd;
                _return += strTDStart + (_replicated == "" ? "---" : (dblReplicated > 0.00 ? dblReplicated.ToString("F") + " GB" : "---")) + strTDEnd;
                _return += strTDStart + (_ha == "" ? "---" : (dblHA > 0.00 ? "High" : "Standard")) + strTDEnd;
                _return += strTDStart + (_ha == "" ? "---" : (dblHA > 0.00 ? dblHA.ToString("F") + " GB" : "---")) + strTDEnd;
                _return += strTREnd;
            }
            dblStorageAmount += dblAmount;
            return _return;
        }

        private string GetStorage(int intAnswer, int intCluster2, int intCSMConfig2, int intNumber2, int intModel, string strName)
        {
            string strStorageHeader =""; 
            DataSet dsLuns = new DataSet();
            if (intCluster2 == 0)
                dsLuns = oStorage.GetLuns(intAnswer, 0, intCluster2, intCSMConfig2, intNumber2);
            else
                dsLuns = oStorage.GetLunsClusterNonShared(intAnswer, intCluster2, intNumber2);
            bool boolOverride = (oForecast.GetAnswer(intAnswer, "storage_override") == "1");
            string strStorage = AddStorage(dsLuns, intModel, intAnswer, boolOverride);
            if (strStorage != "")
            {

                strStorageHeader += strTableStart;
                strStorageHeader += strTRTableHeader;
                strStorageHeader += strTDLableStart1 + "#" + strTDEnd;
                strStorageHeader += strTDLableStart1 + "Path" + strTDEnd;
                strStorageHeader += strTDLableStart1 + "Performance" + strTDEnd;
                strStorageHeader += strTDLableStart1 + "Size in Prod" + strTDEnd;
                strStorageHeader += strTDLableStart1 + "Size in QA" + strTDEnd;
                strStorageHeader += strTDLableStart1 + "Size in Test" + strTDEnd;
                strStorageHeader += strTDLableStart1 + "Replication" + strTDEnd;
                strStorageHeader += strTDLableStart1 + "High Availability" + strTDEnd;
                
                strStorageHeader += strTREnd;

                strStorage = strStorageHeader + strStorage;
            }
            return strStorage;
        }
        private string GetStorageShared(int intAnswer, int intModel)
        {
            string strStorageHeader = ""; 
            bool boolOverride = (oForecast.GetAnswer(intAnswer, "storage_override") == "1");
            string strClusterNames = "";
            string strStorage = "";
            int intClusterOLD = 0;
            DataSet dsCluster = oServer.GetAnswerClusters(intAnswer);
            foreach (DataRow drCluster in dsCluster.Tables[0].Rows)
            {
                int intClusterID = Int32.Parse(drCluster["clusterid"].ToString());
                if (intClusterOLD != intClusterID)
                {
                    if (intClusterID > 0)
                    {
                        DataSet dsServers = oServer.GetClusters(intClusterID);
                        foreach (DataRow drServer in dsServers.Tables[0].Rows)
                        {
                            int intServer = Int32.Parse(drServer["id"].ToString());
                            if (strClusterNames != "")
                                strClusterNames += ", ";
                            strClusterNames += oServer.GetName(intServer, true);
                        }
                        DataSet dsLuns = oStorage.GetLunsClusterShared(intAnswer, intClusterID);
                        strStorage += AddStorage(dsLuns, intModel, intAnswer, boolOverride);
                    }
                }
                intClusterOLD = intClusterID;
            }
            if (strStorage != "")
            {
                strStorageHeader += strTableStart;
                strStorageHeader += strTRTableHeader;
                strStorageHeader += strTDLableStart1 + "#" + strTDEnd;
                strStorageHeader += strTDLableStart1 + "Path" + strTDEnd;
                strStorageHeader += strTDLableStart1 + "Performance" + strTDEnd;
                strStorageHeader += strTDLableStart1 + "Size in Prod" + strTDEnd;
                strStorageHeader += strTDLableStart1 + "Size in QA" + strTDEnd;
                strStorageHeader += strTDLableStart1 + "Size in Test" + strTDEnd;
                strStorageHeader += strTDLableStart1 + "Replication" + strTDEnd;
                strStorageHeader += strTDLableStart1 + "High Availability" + strTDEnd;
               
                strStorageHeader += strTREnd;

                strStorage = strStorageHeader + strStorage;
            }
            return strStorage;
        }
        private string AddStorage(DataSet dsLuns, int intModel, int intAnswer, bool boolOverride)
        {
            string strStorage = "";
            int intRow = 0;
            foreach (DataRow drLun in dsLuns.Tables[0].Rows)
            {
                intRow++;
                strStorage += strTRStart;
                strStorage += strTDStart + intRow.ToString() + strTDEnd;
                string strLetter = drLun["letter"].ToString();
                if (strLetter == "")
                {
                    if (drLun["driveid"].ToString() == "-1000")
                        strLetter = "E";
                    else if (drLun["driveid"].ToString() == "-100")
                        strLetter = "F";
                    else if (drLun["driveid"].ToString() == "-10")
                        strLetter = "P";
                    else if (drLun["driveid"].ToString() == "-1")
                        strLetter = "Q";
                }
                if ((boolOverride == true && drLun["driveid"].ToString() == "0") || oForecast.IsOSMidrange(intAnswer) == true)
                    strStorage += strTDStart + drLun["path"].ToString() + strTDEnd;
                else
                    strStorage += strTDStart + strLetter + ":" + drLun["path"].ToString() + strTDEnd;
                strStorage += strTDStart + drLun["performance"].ToString() + strTDEnd;
                strStorage += strTDStart + drLun["size"].ToString() + " GB / " + (drLun["actual_size"].ToString() == "-1" ? "---" : drLun["actual_size"].ToString() + " GB") + strTDEnd;
                strStorage += strTDStart + drLun["size_qa"].ToString() + " GB / " + (drLun["actual_size_qa"].ToString() == "-1" ? "---" : drLun["actual_size_qa"].ToString() + " GB") + strTDEnd;
                strStorage += strTDStart + drLun["size_test"].ToString() + " GB / " + (drLun["actual_size_test"].ToString() == "-1" ? "---" : drLun["actual_size_test"].ToString() + " GB") + strTDEnd;
                strStorage += strTDStart + (drLun["replicated"].ToString() == "0" ? "No" : "Yes") + " / " + (drLun["actual_replicated"].ToString() == "-1" ? "---" : (drLun["actual_replicated"].ToString() == "0" ? "No" : "Yes")) + strTDEnd;
                strStorage += strTDStart + (drLun["high_availability"].ToString() == "0" ? "No" : "Yes (" + drLun["size"].ToString() + " GB)") + " / " + (drLun["actual_high_availability"].ToString() == "-1" ? "---" : (drLun["actual_high_availability"].ToString() == "0" ? "No" : "Yes (" + drLun["actual_size"].ToString() + " GB)")) + strTDEnd;
                strStorage += strTREnd;
                DataSet dsPoints = oStorage.GetMountPoints(Int32.Parse(drLun["id"].ToString()));
                int intPoint = 0;
                foreach (DataRow drPoint in dsPoints.Tables[0].Rows)
                {
                    intRow++;
                    intPoint++;
                    strStorage += strTRStart;
                    strStorage += strTDStart + intRow.ToString() + strTDEnd;
                    if (oForecast.IsOSMidrange(intAnswer) == true)
                        strStorage += strTDStart + drPoint["path"].ToString() + strTDEnd;
                    else
                        strStorage += strTDStart + strLetter + ":\\SH" + drLun["driveid"].ToString() + "VOL" + (intPoint < 10 ? "0" : "") + intPoint.ToString() + strTDEnd;
                    strStorage += strTDStart + drPoint["performance"].ToString() + strTDEnd;
                    strStorage += strTDStart + drPoint["size"].ToString() + " GB / " + (drPoint["actual_size"].ToString() == "-1" ? "---" : drPoint["actual_size"].ToString() + " GB") + strTDEnd;
                    strStorage += strTDStart + drPoint["size_qa"].ToString() + " GB / " + (drPoint["actual_size_qa"].ToString() == "-1" ? "---" : drPoint["actual_size_qa"].ToString() + " GB") + strTDEnd;
                    strStorage += strTDStart + drPoint["size_test"].ToString() + " GB / " + (drPoint["actual_size_test"].ToString() == "-1" ? "---" : drPoint["actual_size_test"].ToString() + " GB") + strTDEnd;
                    strStorage += strTDStart + (drPoint["replicated"].ToString() == "0" ? "No" : "Yes") + " / " + (drPoint["actual_replicated"].ToString() == "-1" ? "---" : (drPoint["actual_replicated"].ToString() == "0" ? "No" : "Yes")) + strTDEnd;
                    strStorage += strTDStart + (drPoint["high_availability"].ToString() == "0" ? "No" : "Yes (" + drPoint["size"].ToString() + " GB)") + " / " + (drPoint["actual_high_availability"].ToString() == "-1" ? "---" : (drPoint["actual_high_availability"].ToString() == "0" ? "No" : "Yes (" + drPoint["actual_size"].ToString() + " GB)")) + strTDEnd;
                    strStorage += strTREnd;
                }
            }
            return strStorage;
        }


    }
}
