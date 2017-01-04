using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.ServiceProcess;
using System.Text;
using System.Data.SqlClient;
using System.Timers;
using System.IO;
using NCC.ClearView.Application.Core;
using System.Threading;
using System.Configuration;
using NCC.ClearView.Application.Core.ClearViewWS;
using System.Net.NetworkInformation;
using Vim25Api;
using System.Runtime.InteropServices;

namespace ClearViewService
{
    public enum RpcAuthnLevel
    {
        Default = 0,
        None,
        Connect,
        Call,
        Pkt,
        PktIntegrity,
        PktPrivacy
    }

    public enum RpcImpLevel
    {
        Default = 0,
        Anonymous,
        Identify,
        Impersonate = 3,
        Delegate
    }

    public enum EoAuthnCap
    {
        None = 0x00,
        MutualAuth = 0x01,
        StaticCloaking = 0x20,
        DynamicCloaking = 0x40,
        AnyAuthority = 0x80,
        MakeFullSIC = 0x100,
        Default = 0x800,
        SecureRefs = 0x02,
        AccessControl = 0x04,
        AppID = 0x08,
        Dynamic = 0x10,
        RequireFullSIC = 0x200,
        AutoImpersonate = 0x400,
        NoCustomMarshal = 0x2000,
        DisableAAA = 0x1000
    }
    public partial class Service : ServiceBase
    {
        [DllImport("Ole32.dll",
            ExactSpelling = true,
            EntryPoint = "CoInitializeSecurity",
            CallingConvention = CallingConvention.StdCall,
            SetLastError = false,
            PreserveSig = false)]
        private static extern void CoInitializeSecurity(
            IntPtr pVoid,
            int cAuthSvc,
            IntPtr asAuthSvc,
            IntPtr pReserved1,
            uint dwAuthnLevel,
            uint dwImpLevel,
            IntPtr pAuthList,
            uint dwCapabilities,
            IntPtr pReserved3);
        [DllImport("advapi32.dll")]
        public static extern int LogonUserA(String lpszUserName,
            String lpszDomain,
            String lpszPassword,
            int dwLogonType,
            int dwLogonProvider,
            ref IntPtr phToken);


        private System.Timers.Timer oTimer = null;
        private double dblInterval ;
        private int intTest ;                         // 1 = DEBUG MODE, 0 = LIVE
        private string strPreface  ;                  // Any preface to the email
        private string dsn ;                          // The connection to Database        
        private string dsnAsset;
        private string dsnIP;
        private string dsnServiceEditor;                          // The connection to Database        
        private string dsnZeus;
         

        private int intEnvironment ;                 // The environment of the application
        private string strReminderDay ;              // The day(s) to send reminders
        private string strProjectRequest;            // The time intervals to check
        private int intPR1;                          // The number of days before notifying managers
        private int intPR2 ;                         // The number of days before notifying platform
        private int intPR3 ;                         // The number of days before notifying board
        private int intPR4 ;                         // The number of days before notifying director
        private int intPRWorkflowPage ;              // The pageid of the project request workflow
        private string strOoOTime ;                  // The times to check out of office calendar
        private int intNotifyManager ;               // Notify manager in addition to team lead (if applicable)
        private string strImplementation ;           // The times to check implementation calendar
        private int intImplementation ;              // The number of days before an implementation to notify
        private string strRequestEnd ;               // The times to check end dates of requests
        private int intRequestEnd ;                  // The number of days before a request end to notify
        private string strWeeklyStatus ;             // The times to check end dates of weekly status updates
        private int intWeeklyStatus ;                // The number of days between weekly status updates to notify
        private string strWeeklyStatusApps ;         // The applications to check for weekly updates
        private string strPendingAssign ;            // The times to check the pending assignments
        private int intPendingAssign ;               // The number of days between team lead is notified of pending assignment
        private int intPendingAssignManager ;        // The number of days before manager is copied on a pending assignment
        private int intAssignPage ;                  // The pageid of the service request assignment page
        private int intWorkloadPage ;                // The pageid of the workload manager page
        private string strGenerateTasksDay ;         // The day of the week to run autogenerated tasks
        private string strGenerateTasksTime ;        // The time to run autogenerated tasks
        private int intAutoAccount ;                 // The userid of the auto-generated account
        private int intOrganization;                 // The organizationid of EPS
        private int intComputerObjectItem ;          // The itemid of remediation group
        private string strDesignImplementationTime ; // The time to run design implementation reminders
        private int intDesignImplementationItem ;    // The itemid of design implementation itemid
        private int intDesignPage ;                  // The pageid of the design builder page
        private int intLog;                          // Logging : 1 = on, 0 = off
        private int intProductionId  ;
        private string strODTime ;                   // The times to check outdated designs
        private string strProdTime;                  //  The times to check production date for completed builds
        private string strDSMADMC;                   // The location of the DSMADMC.EXE file (for TSM)
        private int intServicePage ;                 // The pageid of the service request | manage services page
        protected string strTSMTeam;
        protected string strTSMTeamTest;
        protected string strTSMTeamProd;
        private string strTSMImportTime;
        private string strTSMImportLocation;
        private bool boolTSMImportAll;
        private int intServerAuditErrorService = 0;
        private int intResourceRequestApprove = 0;
        private int intViewPage = 0;
        private Users oUser;
        private Pages oPage;
        private Variables oVariable;
        private Functions oFunction;
        private ProjectRequest oProjectRequest;
        private DateTime oStart;
        private EventLog oEventLog;
        private string strScripts = "E:\\APPS\\CLV\\ClearViewService\\";
        private string strSub = "scripts\\";

        private OnDemandTasks oOnDemandTasks;
        private ResourceRequest oResourceRequest;
        private ServiceRequests oServiceRequest;
        private Asset oAsset;
        private Forecast oForecast;
        private Incident oIncident;
        private Servers oServer;
        private Workstations oWorkstation;
        private Holidays oHoliday;
        private ModelsProperties oModelsProperties;
        private Classes oClass;
        private Log oLog;
        private bool boolDone = false;
        public bool boolError = false;
        private string strEMailIdsBCC = "";
        private double dblADSync;
        private bool boolADSync;
        private bool boolADSyncRunning = false;
        private int intTimeoutDefault = (5 * 60 * 1000);    // 5 minutes

        private int intAuditCounts = 0;     // The number of audits that can be run at one time
        private int intAuditCount = 0;      // The current number of audits running (compared against intAuditCounts)
        public int AuditCount
        {
            get { return intAuditCount; }
            set { intAuditCount = value; }
        }

        public Service()
        {    
            InitializeComponent();
            CoInitializeSecurity(IntPtr.Zero,
                -1,
                IntPtr.Zero,
                IntPtr.Zero,
                (uint)RpcAuthnLevel.None,
                (uint)RpcImpLevel.Impersonate,
                IntPtr.Zero,
                (uint)EoAuthnCap.None,
                IntPtr.Zero);
              try 
            {
                if (EventLog.SourceExists("ClearView") == false)
                {
                    EventLog.CreateEventSource("ClearView", "ClearView");
                    EventLog.WriteEntry(String.Format("ClearView EventLog Created"), EventLogEntryType.Information);
                }
                oEventLog = new EventLog();
                oEventLog.Source = "ClearView";               
                LoadConfigValues();
                oOnDemandTasks = new OnDemandTasks(0, dsn);
                oResourceRequest = new ResourceRequest(0, dsn);
                oServiceRequest = new ServiceRequests(0, dsn);
                oHoliday = new Holidays(0, dsn);
                oAsset = new Asset(0,dsnAsset, dsn);
                oServer = new Servers(0, dsn);
                oWorkstation = new Workstations(0, dsn);
                oForecast = new Forecast(0, dsn);
                oIncident = new Incident(0, dsn);
                oModelsProperties = new ModelsProperties(0, dsn);
                oClass = new Classes(0, dsn);
                oLog = new Log(0, dsn);
                //                ServiceRequestSLANotifications();
            } 
             catch(Exception exc)
            {                
                oEventLog.WriteEntry(String.Format("ClearView Service initialization has failed - INVALID CONFIGURATION IN APP.CONFIG "+exc.StackTrace), EventLogEntryType.Error);
                boolError = true;
                Dispose(true);
            }            
          
    }

        protected override void OnStart(string[] args)
        {
            oEventLog.WriteEntry(String.Format("ClearView Service started."), EventLogEntryType.Information);           
            oTimer.AutoReset = true;
            oTimer.Enabled = true;
            oTimer.Start();         
            
        }

        protected override void OnStop()
        {
            oEventLog.WriteEntry(String.Format("ClearView Service stopped."), EventLogEntryType.Information);
            oTimer.AutoReset = false;
            oTimer.Enabled = false;             
        }      


        private void LoadConfigValues()
        {         
            dblInterval = Double.Parse(ConfigurationManager.AppSettings["Interval"]);
            intTest = Int32.Parse(ConfigurationManager.AppSettings["Test"]);                        // 1 = DEBUG MODE, 0 = LIVE
            strPreface = ConfigurationManager.AppSettings["Preface"];                  // Any preface to the email
            dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;                         // The connection to Database        
            dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSNAsset"]].ConnectionString;                         // The connection to Database        
            dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSNIp"]].ConnectionString;                         // The connection to Database        
            dsnServiceEditor = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSNServiceEditor"]].ConnectionString;                         // The connection to Database        
            dsnZeus = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSNZeus"]].ConnectionString;                         // The connection to Database        
            intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);                 // The environment of the application
            strReminderDay = ConfigurationManager.AppSettings["Reminder_day"];              // The day(s) to send reminders
            strProjectRequest = ConfigurationManager.AppSettings["Pr_time"];           // The time intervals to check
            intPR1 = Int32.Parse(ConfigurationManager.AppSettings["Pr1"]);                         // The number of days before notifying managers
            intPR2 = Int32.Parse(ConfigurationManager.AppSettings["Pr2"]);                         // The number of days before notifying platform
            intPR3 = Int32.Parse(ConfigurationManager.AppSettings["Pr3"]);                         // The number of days before notifying board
            intPR4 = Int32.Parse(ConfigurationManager.AppSettings["Pr4"]);                         // The number of days before notifying director
            intPRWorkflowPage = Int32.Parse(ConfigurationManager.AppSettings["Workflow_pageid"]);              // The pageid of the project request workflow
            strOoOTime = ConfigurationManager.AppSettings["Ooo_time"];                  // The times to check out of office calendar
            intNotifyManager = Int32.Parse(ConfigurationManager.AppSettings["Ooo_manager"]);               // Notify manager in addition to team lead (if applicable)
            strImplementation = ConfigurationManager.AppSettings["Imp_time"];           // The times to check implementation calendar
            intImplementation = Int32.Parse(ConfigurationManager.AppSettings["Imp_days"]);              // The number of days before an implementation to notify
            strRequestEnd = ConfigurationManager.AppSettings["Request_time"];               // The times to check end dates of requests
            intRequestEnd = Int32.Parse(ConfigurationManager.AppSettings["Request_days"]);                  // The number of days before a request end to notify
            strWeeklyStatus = ConfigurationManager.AppSettings["Weekly_status_time"];             // The times to check end dates of weekly status updates
            intWeeklyStatus = Int32.Parse(ConfigurationManager.AppSettings["Weekly_status_days"]);                // The number of days between weekly status updates to notify
            strWeeklyStatusApps = ConfigurationManager.AppSettings["Weekly_status_apps"];         // The applications to check for weekly updates
            strPendingAssign = ConfigurationManager.AppSettings["Pending_assign_time"];            // The times to check the pending assignments
            intPendingAssign = Int32.Parse(ConfigurationManager.AppSettings["Pending_assign_days"]);               // The number of days between team lead is notified of pending assignment
            intPendingAssignManager = Int32.Parse(ConfigurationManager.AppSettings["Pending_assign_manager_days"]);        // The number of days before manager is copied on a pending assignment
            intAssignPage = Int32.Parse(ConfigurationManager.AppSettings["Assign_pageid"]);                  // The pageid of the service request assignment page
            intWorkloadPage = Int32.Parse(ConfigurationManager.AppSettings["Workflow_pageid"]);                // The pageid of the workload manager page
            strGenerateTasksDay = ConfigurationManager.AppSettings["Generate_day"];         // The day of the week to run autogenerated tasks
            strGenerateTasksTime = ConfigurationManager.AppSettings["Generate_time"];        // The time to run autogenerated tasks
            intAutoAccount = Int32.Parse(ConfigurationManager.AppSettings["Generate_accountid"]);                 // The userid of the auto-generated account
            intOrganization = Int32.Parse(ConfigurationManager.AppSettings["Generate_organizationid"]);                // The organizationid of EPS
            intComputerObjectItem = Int32.Parse(ConfigurationManager.AppSettings["Computer_object_itemid"]);          // The itemid of remediation group
            strDesignImplementationTime = ConfigurationManager.AppSettings["Design_implementation_time"]; // The time to run design implementation reminders
            intDesignImplementationItem = Int32.Parse(ConfigurationManager.AppSettings["Design_implementation_itemid"]);    // The itemid of design implementation itemid
            intDesignPage = Int32.Parse(ConfigurationManager.AppSettings["Design_pageid"]);                  // The pageid of the design builder page
            strTSMImportTime = ConfigurationManager.AppSettings["tsm_import_time"];
            strTSMImportLocation = ConfigurationManager.AppSettings["tsm_import_location"];
            boolTSMImportAll = (ConfigurationManager.AppSettings["tsm_import_all"] == "1");
          
            intLog = Int32.Parse(ConfigurationManager.AppSettings["Logging"]);                     // Logging : 1 = on, 0 = off
            intProductionId = Int32.Parse(ConfigurationManager.AppSettings["ProductionClassID"]);
            strODTime = ConfigurationManager.AppSettings["Od_time"];
            strProdTime = ConfigurationManager.AppSettings["Prod_time"];
            strDSMADMC = ConfigurationManager.AppSettings["DSMADMC"];
            intServicePage = Int32.Parse(ConfigurationManager.AppSettings["service_pageid"]);
            strPreface = ConfigurationManager.AppSettings["Preface"].ToString().Trim() != "" ? "<p><font style=\"color:CC0000\"><b>" + strPreface + "</b></font></p>" : "";
            dblADSync = Double.Parse(ConfigurationManager.AppSettings["ad_sync"]);
            boolADSync = (ConfigurationManager.AppSettings["ad_sync_update"] == "1");

            strTSMTeam = ConfigurationManager.AppSettings["TSM_TEAM"];
            strTSMTeamTest = ConfigurationManager.AppSettings["TSM_TEAM_TEST"];
            strTSMTeamProd = ConfigurationManager.AppSettings["TSM_TEAM_PROD"];

            intServerAuditErrorService = Int32.Parse(ConfigurationManager.AppSettings["server_audit_error_service"]);
            intResourceRequestApprove = Int32.Parse(ConfigurationManager.AppSettings["RR_approve"]);
            intViewPage = Int32.Parse(ConfigurationManager.AppSettings["View_page"]);

            oUser = new Users(0, dsn);
            oPage = new Pages(0, dsn);
            oVariable = new Variables(intEnvironment);
            oFunction = new Functions(0, dsn, intEnvironment);
            oProjectRequest = new ProjectRequest(0, dsn);
            oTimer = new System.Timers.Timer(dblInterval);
            oTimer.Elapsed += new ElapsedEventHandler(this.ServiceTimer_Tick);
            
        }

        private void ServiceTimer_Tick(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                oStart = DateTime.Now;
                oTimer.Stop();
                // *******  START PROCESSSING FOR TICK ************
                if (intTest == 1)
                    oEventLog.WriteEntry(String.Format("DEBUG: ClearView Service TICK."), EventLogEntryType.Information);
                string[] strDays;
                char[] strSplit = { ';' };
                strDays = strReminderDay.Split(strSplit);
                for (int ii = 0; ii < strDays.Length; ii++)
                {
                    if (strDays[ii].Trim() != "")
                    {
                        if (oStart.DayOfWeek.ToString().ToUpper() == strDays[ii].ToUpper())
                        {
                            //CheckProjectRequestWorkflow();                         

                            // ******* DO THESE *******

                            /*
                            if (intLog > 0)
                                oEventLog.WriteEntry("Starting CheckOutOfOfficeCalendar()...", EventLogEntryType.SuccessAudit);
                            CheckOutOfOfficeCalendar();

                            if (intLog > 0)
                                oEventLog.WriteEntry("Starting CheckImplementationCalendar()...", EventLogEntryType.SuccessAudit);
                            CheckImplementationCalendar();

                            if (intLog > 0)
                                oEventLog.WriteEntry("Starting CheckOutdatedDesigns()...", EventLogEntryType.SuccessAudit);
                            CheckOutdatedDesigns();

                            if (intLog > 0)
                                oEventLog.WriteEntry("Starting CheckProductionDateForCompletedBuilds()...", EventLogEntryType.SuccessAudit);
                            CheckProductionDateForCompletedBuilds();

                            if (intLog > 0)
                                oEventLog.WriteEntry("Starting GenerateProdSAN()...", EventLogEntryType.SuccessAudit);
                            GenerateProdSAN();
                             */

                            //if (intLog > 0)
                            //    oEventLog.WriteEntry("Starting RegisterTSM()...", EventLogEntryType.SuccessAudit);
                            //RegisterTSM();

                            /*
                            if (intLog > 0)
                                oEventLog.WriteEntry("Starting ExecuteLTMConfigs()...", EventLogEntryType.SuccessAudit);
                            ExecuteLTMConfigs();

                            if (intLog > 0)
                                oEventLog.WriteEntry("Starting ServiceRequestSLANotifications()...", EventLogEntryType.SuccessAudit);
                            ServiceRequestSLANotifications();
                             */

                            //if (intLog > 0)
                            //    oEventLog.WriteEntry("Starting ImportTSMRegistrations()...", EventLogEntryType.SuccessAudit);
                            //ImportTSMRegistrations();

                            if (intLog > 0)
                                oEventLog.WriteEntry("Starting DecommissionEarlyWarning()...", EventLogEntryType.SuccessAudit);
                            DecommissionEarlyWarning();

                            if (intLog > 0)
                                oEventLog.WriteEntry("Starting IncidentsCreated()...", EventLogEntryType.SuccessAudit);
                            IncidentsCreated();

                            if (intLog > 0)
                                oEventLog.WriteEntry("Starting IncidentsGenerated()...", EventLogEntryType.SuccessAudit);
                            IncidentsGenerated();

                            if (intLog > 0)
                                oEventLog.WriteEntry("Starting IncidentsResolved()...", EventLogEntryType.SuccessAudit);
                            IncidentsResolved();

                            // ******* DO THESE *******

                            //CheckRequestClose();    // project end date - wait
                            //CheckWeeklyStatus();
                            //CheckPendingAssignments();
                            //CheckDesignImplementations();
                        }
                    }
                }
                //strDays = strGenerateTasksDay.Split(strSplit);
                //for (int ii = 0; ii < strDays.Length; ii++)
                //{
                //    if (strDays[ii].Trim() != "")
                //    {
                //        if (oStart.DayOfWeek.ToString().ToUpper() == strDays[ii].ToUpper())
                //        {
                //            GenerateTasks();
                //        }
                //    }
                //}
                strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_CLEARVIEW_SUPPORT");
                if (boolDone == true && strEMailIdsBCC != "") 
                {
                    oFunction.SendEmail("ClearView Reminder / Task Summary", strEMailIdsBCC , "", "", "ClearView Reminder / Task Summary", strPreface + "<p><b>This message is to inform you that the ClearView Reminders and Auto-Generated Tasks have completed on " + DateTime.Now.ToLongDateString() + " at " + DateTime.Now.ToLongTimeString() + ".</b></p>", true, false);
                    boolDone = false;
                }

                // Run AD Sync
                if (boolADSyncRunning == false)
                {
                    boolADSyncRunning = true;
                    if (dblADSync > 0.00)
                    {
                        // Kick off a thread to process based on "dblADSync" sleep settings
                        oEventLog.WriteEntry("Active Directory Sync Started (sleep = " + dblADSync.ToString() + " minutes, updates = " + dblADSync.ToString() + ")", EventLogEntryType.Warning);
                        ADSync oADSync = new ADSync(dblADSync, boolADSync, oEventLog, intLog, intEnvironment, dsn, dsnAsset, dsnIP, dsnServiceEditor);
                        ThreadStart oADSyncThreadStart = new ThreadStart(oADSync.Begin);
                        Thread oADSyncThread = new Thread(oADSyncThreadStart);
                        oADSyncThread.Start();
                    }
                    else
                    {
                        // Submit a warning message and skip
                        oEventLog.WriteEntry("Active Directory Sync Not Started", EventLogEntryType.Warning);
                    }
                }

                if (intTest == 1)
                    oEventLog.WriteEntry(String.Format("DEBUG: ClearView Service END TICK."), EventLogEntryType.Information);
                // *******  END PROCESSSING FOR TICK **************
               
            }
            catch (Exception ex)
            {
                string strError = "Reminder Service: " + "(Error Message: " + ex.Message + ") ~ (Source: " + ex.Source + ") (Stack Trace: " + ex.StackTrace + ") [" + System.Environment.UserName + "]";
                oEventLog.WriteEntry(strError, EventLogEntryType.Error);
                SystemError(strError);
                oEventLog.WriteEntry(String.Format(ex.Message), EventLogEntryType.Error);
            }
            finally
            {
                oTimer.Start();
            }
          
        }
        private void SystemError(string _error)
        {
            try
            {
                Settings oSetting = new Settings(0, dsn);
                oSetting.SystemError(0, 0, 0, _error, 0, 0, false, null, intEnvironment, "");
            }
            catch (Exception exSystemError)
            {
                // If database is offline, it will cause a fatal error AND stop the service.  Stop that from happening by using a catch here.
                oEventLog.WriteEntry("The database is currently offline.  Error message = " + exSystemError.Message, EventLogEntryType.Error);
            }
        }


        //private void CheckProjectRequestWorkflow() 
        //{
        //    DateTime oCheck = oStart;
        //    string[] strTimes;
        //    char[] strSplit = { ';' };
        //    strTimes = strProjectRequest.Split(strSplit);
        //    for (int ii = 0; ii < strTimes.Length; ii++)
        //    {
        //        if (strTimes[ii].Trim() != "")
        //        {
        //            DateTime _tick = DateTime.Parse(strTimes[ii].Trim());
        //            if (_tick.ToShortTimeString() == oCheck.ToShortTimeString())
        //            {
        //                boolDone = true;
        //                DataSet ds = oProjectRequest.GetReminder();
        //                foreach (DataRow dr in ds.Tables[0].Rows)
        //                {
        //                    try
        //                    {
        //                        int intUser = Int32.Parse(dr["userid"].ToString());
        //                        int intRequest = Int32.Parse(dr["requestid"].ToString());
        //                        DateTime _updated = DateTime.Parse(dr["modified"].ToString());
        //                        TimeSpan oSpan = new TimeSpan();
        //                        oSpan = oCheck.Subtract(_updated);
        //                        bool boolNotify = false;
        //                        string strBody = oProjectRequest.GetBody(intRequest, intEnvironment, true);
        //                        switch (dr["step"].ToString())
        //                        {
        //                            case "1":
        //                                if (oSpan.Days >= intPR1)
        //                                    boolNotify = true;
        //                                break;
        //                            case "2":
        //                                if (oSpan.Days >= intPR2)
        //                                    boolNotify = true;
        //                                break;
        //                            case "3":
        //                                if (oSpan.Days >= intPR3)
        //                                    boolNotify = true;
        //                                break;
        //                            case "4":
        //                                if (oSpan.Days >= intPR4)
        //                                    boolNotify = true;
        //                                break;
        //                        }
        //                        if (boolNotify == true)
        //                        {
        //                            string strDefault = oUser.GetApplicationUrl(intUser, intPRWorkflowPage);
        //                              strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_REQUEST_ASSIGNMENT");

        //                            if (strDefault == "")
        //                            {
        //                                if (intTest == 0)
        //                                    oFunction.SendEmail("REMINDER: Project Request Workflow", oUser.GetName(intUser) + ";", "", strEMailIdsBCC, "REMINDER: Project Request Workflow", strPreface + "<p><b>This is a reminder that the following project request requires your approval...</b></p><p>" + strBody + "</p>", true, false);
        //                                else
        //                                    oFunction.SendEmail("REMINDER: Project Request Workflow", "", "", strEMailIdsBCC, "REMINDER: Project Request Workflow", strPreface + "<p>TO: " + oUser.GetFullName(intUser) + "<br/>CC: " + "" + "</p><p><b>This is a reminder that the following project request requires your approval...</b></p><p>" + strBody + "</p>", true, false);
        //                            }
        //                            else
        //                            {
        //                                if (intTest == 0)
        //                                    oFunction.SendEmail("REMINDER: Project Request Workflow", oUser.GetName(intUser) + ";", "", strEMailIdsBCC, "REMINDER: Project Request Workflow", strPreface + "<p><b>This is a reminder that the following project request requires your approval...</b></p><p><a href=\"" + oVariable.URL() + "/redirect.aspx?referrer=/" + strDefault + oPage.GetFullLink(intPRWorkflowPage) + "?rid=" + intRequest.ToString() + "\" target=\"_blank\">Click here to view this project request.</a></p><p>" + strBody + "</p>", true, false);
        //                                else
        //                                    oFunction.SendEmail("REMINDER: Project Request Workflow", "", "", strEMailIdsBCC, "REMINDER: Project Request Workflow", strPreface + "<p>TO: " + oUser.GetFullName(intUser) + "<br/>CC: " + "" + "</p><p><b>This is a reminder that the following project request requires your approval...</b></p><p><a href=\"" + oVariable.URL() + "/redirect.aspx?referrer=/" + strDefault + oPage.GetFullLink(intPRWorkflowPage) + "?rid=" + intRequest.ToString() + "\" target=\"_blank\">Click here to view this project request.</a></p><p>" + strBody + "</p>", true, false);
        //                            }
        //                        }
        //                    }
        //                    catch (Exception oError)
        //                    {
        //                        oFunction.SendEmail("ERROR: Project Request Workflow", strEMailIdsBCC, "", "", "ERROR: Project Request Workflow", strPreface + "<p><b>The following record caused an error in a ClearView Reminder...</b></p><p>RequestID:" + dr["requestid"].ToString() + "<br/>UserID:" + dr["userid"].ToString() + "</p>", true, false);
        //                    }
        //                }
        //                if (intLog > 0)
        //                    oEventLog.WriteEntry(String.Format("REMINDER: Project Request Workflow - Total Reminders: " + ds.Tables[0].Rows.Count.ToString()), EventLogEntryType.Information);
        //            }
        //            else if (intTest == 1)
        //                oEventLog.WriteEntry(String.Format("DEBUG: " + _tick.ToShortTimeString() + " != " + oCheck.ToShortTimeString()), EventLogEntryType.Information);
        //        }
        //    }
        //}
        public void CheckOutOfOfficeCalendar()
        {           
            Vacation oVacation = new Vacation(0, dsn);
            DateTime oCheck = oStart;             
            string[] strTimes;
            char[] strSplit = { ';' };
            strTimes = strOoOTime.Split(strSplit);           
            for (int ii = 0; ii < strTimes.Length; ii++)
            {
                if (strTimes[ii].Trim() != "")
                {
                    DateTime _tick = DateTime.Parse(strTimes[ii].Trim());
                    if (_tick.ToShortTimeString() == oCheck.ToShortTimeString())
                    {
                        boolDone = true;
                        DataSet ds = oVacation.Get(oCheck);
                        int intTeamLead = 0;
                        int intOldTeamLead = 0;
                        int intManager = 0;
                        string strBody = "";
                        string strCC = "";
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            try
                            {
                                foreach (DataRow dr in ds.Tables[0].Rows)
                                {
                                    string strUser = dr["username"].ToString();
                                    intTeamLead = Int32.Parse(dr["teamlead"].ToString());
                                    if (intTeamLead > 0)
                                    {
                                        intManager = Int32.Parse(dr["manager"].ToString());
                                        if (intTeamLead != intOldTeamLead && intOldTeamLead != 0)
                                        {
                                            // Send Email
                                            if (strBody != "")
                                            {
                                                strCC = "";
                                                strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DAILY_NOTIFICATION");

                                                if (intNotifyManager == 1)
                                                    strCC = oUser.GetName(intManager);
                                                strBody = "<table border=\"0\" cellpadding=\"7\" cellspacing=\"0\" style=\"font-family:'openSans', Verdana, Arial, Helvetica, sans-serif; font-size:13px; border:solid 1px #CCCCCC\"><tr bgcolor=\"#EEEEEE\"><td><b>Employee</b></td><td><b>Type</b></td><td><b>Reason</b></td></tr>" + strBody + "</table>";
                                                if (intTest == 0)
                                                    oFunction.SendEmail("REMINDER: Out of Office", oUser.GetName(intOldTeamLead), strCC, strEMailIdsBCC, "REMINDER: Out of Office", strPreface + "<p><b>This message is to notify you that the following employees are out of the office today...</b></p><p>" + strBody + "</p>", true, false);
                                                else
                                                    oFunction.SendEmail("REMINDER: Out of Office", "", "", strEMailIdsBCC, "REMINDER: Out of Office", strPreface + "<p>TO: " + oUser.GetFullName(intOldTeamLead) + "<br/>CC: " + strCC + "</p><p><b>This message is to notify you that the following employees are out of the office today...</b></p><p>" + strBody + "</p>", true, false);
                                                strBody = "";
                                            }
                                        }
                                        intOldTeamLead = intTeamLead;
                                        string strReason = dr["reason"].ToString();
                                        string strDuration = "";
                                        if (dr["vacation"].ToString() == "1")
                                            strReason = "Vacation";
                                        else if (dr["holiday"].ToString() == "1")
                                            strReason = "Floating Holiday";
                                        else if (dr["personal"].ToString() == "1")
                                            strReason = "Personal / Sick Day";
                                        if (dr["morning"].ToString() == "1")
                                            strDuration = "Morning";
                                        else if (dr["afternoon"].ToString() == "1")
                                            strDuration = "Afternoon";
                                        else
                                            strDuration = "Full Day";
                                        strBody += "<tr><td>" + strUser + "</td><td>" + strDuration + "</td><td>" + strReason + "</td></tr>";
                                    }
                                    else
                                    {
                                        strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DEVELOPER_ERROR");

                                        oFunction.SendEmail("ERROR: Out of Office", strEMailIdsBCC, "", "", "ERROR: Out of Office", strPreface + "<p><b>An error occurred when attempting to send a ClearView Reminder...</b></p><p>The user " + strUser + " does not have a valid manager specified</p>", true, false);
                                    }
                                }
                                if (strBody != "")
                                {
                                    strCC = "";
                                    if (intNotifyManager == 1)
                                        strCC = oUser.GetName(intManager);
                                    strBody = "<table border=\"0\" cellpadding=\"7\" cellspacing=\"0\" style=\"font-family:'openSans', Verdana, Arial, Helvetica, sans-serif; font-size:13px; border:solid 1px #CCCCCC\"><tr bgcolor=\"#EEEEEE\"><td><b>Employee</b></td><td><b>Type</b></td><td><b>Reason</b></td></tr>" + strBody + "</table>";
                                    strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DAILY_NOTIFICATION");
                                    if (intTest == 0)
                                        oFunction.SendEmail("REMINDER: Out of Office", oUser.GetName(intTeamLead), strCC, strEMailIdsBCC, "REMINDER: Out of Office", strPreface + "<p><b>This message is to notify you that the following employees are out of the office today...</b></p><p>" + strBody + "</p>", true, false);
                                    else
                                        oFunction.SendEmail("REMINDER: Out of Office", "", "", strEMailIdsBCC, "REMINDER: Out of Office", strPreface + "<p>TO: " + oUser.GetFullName(intTeamLead) + "<br/>CC: " + strCC + "</p><p><b>This message is to notify you that the following employees are out of the office today...</b></p><p>" + strBody + "</p>", true, false);
                                }
                            }
                            catch (Exception oError)
                            {
                                strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DEVELOPER_ERROR");
                                oFunction.SendEmail("ERROR: Out of Office", strEMailIdsBCC, "", "", "ERROR: Out of Office", strPreface + "<p><b>An error occurred when attempting to send a ClearView Reminder...</b></p><p>Error Message:" + oError.Message + "</p>", true, false);
                            }
                        }
                        if (intLog > 0)
                            oEventLog.WriteEntry(String.Format("REMINDER: Out of Office - Total Reminders: " + ds.Tables[0].Rows.Count.ToString()), EventLogEntryType.Information);
                    }
                    else if (intTest == 1)
                        oEventLog.WriteEntry(String.Format("DEBUG: Out of Office  " + _tick.ToShortTimeString() + " != " + oCheck.ToShortTimeString()), EventLogEntryType.Information);
                }
            }        
          
        }
        private void CheckImplementationCalendar()
        {
           
            DateTime oCheck = oStart;
            string[] strTimes;
            char[] strSplit = { ';' };
            strTimes = strImplementation.Split(strSplit);             
            for (int ii = 0; ii < strTimes.Length; ii++)
            {
                if (strTimes[ii].Trim() != "")
                {
                    DateTime _tick = DateTime.Parse(strTimes[ii].Trim());
                    if (_tick.ToShortTimeString() == oCheck.ToShortTimeString())
                    {
                        boolDone = true;
                        ResourceRequest oResourceRequest = new ResourceRequest(0, dsn);
                        DataSet ds = oResourceRequest.GetChangeControls();
                        int intUser = 0;
                        int intOldUser = 0;
                        string strBody = "";
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            try
                            {
                                foreach (DataRow dr in ds.Tables[0].Rows)
                                {
                                    intUser = Int32.Parse(dr["userid"].ToString());
                                    DateTime _implementation = DateTime.Parse(dr["implementation"].ToString());
                                    TimeSpan oSpan = new TimeSpan();
                                    oSpan = _implementation.Subtract(oCheck);
                                    if (intUser != intOldUser && intOldUser != 0)
                                    {
                                        // Send Email
                                        if (strBody != "")
                                        {
                                            strBody = "<table border=\"0\" cellpadding=\"7\" cellspacing=\"0\" style=\"font-family:'openSans', Verdana, Arial, Helvetica, sans-serif; font-size:11px; border:solid 1px #CCCCCC\"><tr bgcolor=\"#EEEEEE\"><td><b>Change Number</b></td><td><b>Change Time</b></td><td><b>Project Name</b></td><td><b>Project Number</b></td></tr>" + strBody + "</table>";
                                            strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DAILY_NOTIFICATION");
                                            if (intTest == 0)
                                                oFunction.SendEmail("REMINDER: Upcoming Changes", oUser.GetName(intOldUser), "", strEMailIdsBCC, "REMINDER: Upcoming Changes", strPreface + "<p><b>This message is to notify you that you have the following changes occurring within the next " + intImplementation.ToString() + " days...</b></p><p>" + strBody + "</p>", true, false);
                                            else
                                                oFunction.SendEmail("REMINDER: Upcoming Changes", "", "", strEMailIdsBCC, "REMINDER: Upcoming Changes", strPreface + "<p>TO: " + oUser.GetFullName(intOldUser) + "<br/>CC: " + "" + "</p><p><b>This message is to notify you that you have an upcoming change...</b></p><p>" + strBody + "</p>", true, false);
                                            strBody = "";
                                        }
                                    }
                                    if (oSpan.Days <= intImplementation)
                                        strBody += "<tr><td>" + dr["number"].ToString() + "</td><td>" + _implementation.ToLongDateString() + " @ " + _implementation.ToShortTimeString() + "</td><td>" + dr["projectname"].ToString() + "</td><td>" + dr["projectnumber"].ToString() + "</td></tr>";
                                    intOldUser = intUser;
                                }
                                if (strBody != "")
                                {
                                    strBody = "<table border=\"0\" cellpadding=\"7\" cellspacing=\"0\" style=\"font-family:'openSans', Verdana, Arial, Helvetica, sans-serif; font-size:13px; border:solid 1px #CCCCCC\"><tr bgcolor=\"#EEEEEE\"><td><b>Change Number</b></td><td><b>Change Time</b></td><td><b>Project Name</b></td><td><b>Project Number</b></td></tr>" + strBody + "</table>";
                                    strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DAILY_NOTIFICATION");
                                    if (intTest == 0)
                                        oFunction.SendEmail("REMINDER: Upcoming Changes", oUser.GetName(intUser), "", strEMailIdsBCC, "REMINDER: Upcoming Changes", strPreface + "<p><b>This message is to notify you that you have the following changes occurring within the next " + intImplementation.ToString() + " days...</b></p><p>" + strBody + "</p>", true, false);
                                    else
                                        oFunction.SendEmail("REMINDER: Upcoming Changes", "", "", strEMailIdsBCC, "REMINDER: Upcoming Changes", strPreface + "<p>TO: " + oUser.GetFullName(intUser) + "<br/>CC: " + "" + "</p><p><b>This message is to notify you that you have an upcoming change...</b></p><p>" + strBody + "</p>", true, false);
                                }
                            }
                            catch (Exception oError)
                            {
                                strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DAILY_NOTIFICATION");
                                oFunction.SendEmail("ERROR: Upcoming Changes", strEMailIdsBCC, "", "", "ERROR: Upcoming Changes", strPreface + "<p><b>An error occurred when attempting to send a ClearView Reminder...</b></p><p>Error Message:" + oError.Message + "</p>", true, false);
                            }
                        }
                        if (intLog > 0)
                            oEventLog.WriteEntry(String.Format("REMINDER: Upcoming Changes - Total Reminders: " + ds.Tables[0].Rows.Count.ToString()), EventLogEntryType.Information);
                    }
                    else if (intTest == 1)
                        oEventLog.WriteEntry(String.Format("DEBUG: Implementation Calendar " + _tick.ToShortTimeString() + " != " + oCheck.ToShortTimeString()), EventLogEntryType.Information);
                }
            }    
           
        }
        private void CheckOutdatedDesigns()
        {           
            DateTime oCheck = oStart;            
            string[] strTimes;
            char[] strSplit = { ';' };
            strTimes = strODTime.Split(strSplit);           
            for (int ii = 0; ii < strTimes.Length; ii++)
            {
                if (strTimes[ii].Trim() != "")
                {
                    DateTime _tick = DateTime.Parse(strTimes[ii].Trim());
                    if (_tick.ToShortTimeString() == oCheck.ToShortTimeString())
                    {
                        boolDone = true;
                        Forecast oForecast = new Forecast(0, dsn);
                        Projects oProject = new Projects(0, dsn);
                        Requests oRequest = new Requests(0, dsn);
                        DataSet ds = oForecast.GetAnswerIncomplete();

                        try
                        {
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                int intId = Int32.Parse(dr["id"].ToString());
                                int intUser = Int32.Parse(dr["userid"].ToString());
                                int intForecast = Int32.Parse(dr["forecastid"].ToString());
                                int intModel = Int32.Parse(dr["modelid"].ToString());
                                if (intForecast > 0)
                                {
                                    int intRequest = Int32.Parse(oForecast.Get(intForecast, "requestid"));
                                    int intProject = oRequest.GetProjectNumber(intRequest);
                                    DataSet dsProject = oProject.Get(intProject);
                                    string strBody = "";
                                    if (dsProject.Tables[0].Rows.Count > 0)
                                    {
                                        oForecast.AddAnswerUpdateSent(intId);
                                        int intSent = oForecast.GetAnswerUpdateSent(intId).Tables[0].Rows.Count;
                                        if (intModel == 0)
                                            intModel = oForecast.GetModel(intId);
                                        if (intSent > 15 && intModel > 0)
                                            oForecast.UpdateAnswerSetComplete(intId);
                                        else
                                        {
                                            string strFinal = "";
                                            if (intModel > 0)
                                            {
                                                if (intSent > 10)
                                                {
                                                    intSent = intSent - 5;
                                                    if (intSent == 0)
                                                        strFinal = "This is the final reminder! Please provide an update or your design will be marked complete.";
                                                    else
                                                        strFinal = "You will be reminded " + intSent.ToString() + " more time(s). To avoid having your design marked complete, please provide an update.";
                                                }
                                            }
                                            else
                                                strFinal = "You will continue to be reminded of this design until you take action!";
                                            int intEngineer = Int32.Parse(dsProject.Tables[0].Rows[0]["engineer"].ToString());
                                            int intLead = Int32.Parse(dsProject.Tables[0].Rows[0]["lead"].ToString());
                                            strBody = "<table cellpadding=\"0\" cellspacing=\"0\" style=\"" + oVariable.DefaultFontStyle() + "\" border=\"0\">";
                                            strBody += "<tr><td>&nbsp;</td></tr>";
                                            strBody += "<tr><td style=\"" + oVariable.DefaultFontStyleHeader() + "\"></td></tr>";
                                            strBody += "<tr><td>&nbsp;</td></tr>";
                                            strBody += "<tr><td><b>NOTE:</b> Reminders will now be sent everyday regarding this design until appropriate action has been taken. Contact your ClearView administrator or Josh Ciora for more information.</td></tr>";
                                            strBody += "<tr><td>&nbsp;</td></tr>";
                                            strBody += "<tr><td colspan=\"2\">";
                                            strBody += "<table cellpadding=\"0\" cellspacing=\"0\" style=\"" + oVariable.DefaultFontStyle() + "\" border=\"0\">";
                                            strBody += "<tr><td rowspan=\"2\"><img src=\"" + oVariable.ImageURL() + "/images/bigalert.gif\" border=\"0\" align=\"absmiddle\" /></td><td nowrap valign=\"bottom\">&nbsp;&nbsp;<a href=\"" + oVariable.URL() + "/redirect.aspx?referrer=/frame/forecast_update.aspx?id=" + dr["id"] + "\">Click here to provide a status update</a></td></tr>";
                                            strBody += "<tr><td nowrap valign=\"top\">&nbsp;&nbsp;(NOTE: You must click this link to avoid future communications )</td></tr>";
                                            strBody += "</table></td></tr>";
                                            strBody += "<tr><td>&nbsp;</td></tr>";
                                            strBody += "<tr><td>&nbsp;</td></tr>";
                                            strBody += "<tr><td>&nbsp;</td></tr>";
                                            strBody += "<tr><td nowrap><a href=\"" + oVariable.URL() + "/redirect.aspx?referrer=/frame/forecast_equipment.aspx?id=" + dr["id"] + "\">Click here to view your design</a></td></tr>";
                                            strBody += "<tr><td>&nbsp;</td></tr>";
                                            strBody += "<tr><td nowrap><b><u>Design Details</u></b></td></tr>";
                                            strBody += "<tr><td>&nbsp;</td></tr>";
                                            strBody += "<tr><td colspan=\"3\">" + oForecast.GetAnswerBody(intId, intEnvironment, dsnAsset, dsnIP) + "</td></tr>";
                                            strBody += "</table>";
                                            string strTO = oUser.GetName(intUser);
                                            if (strTO == "")
                                                strTO = oUser.GetName(intEngineer);
                                            if (strTO == "")
                                                strTO = oUser.GetName(intLead);
                                            strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DAILY_NOTIFICATION");
                                            oFunction.SendEmail("Update ClearView Design", strTO, oUser.GetName(intEngineer) + ";" + oUser.GetName(intLead), strEMailIdsBCC, "ClearView Design Status Update (Action Required)", strBody, true, false);
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception oError)
                        {
                            strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DAILY_NOTIFICATION");
                            oFunction.SendEmail("ERROR: Outdated Designs", strEMailIdsBCC, "", "", "ERROR: Outdated Designs", strPreface + "<p><b>An error occurred when attempting to send a ClearView Reminder...</b></p><p>Error Message:" + oError.Message + "</p>", true, false);
                        }
                    }
                    else if (intTest == 1)
                        oEventLog.WriteEntry(String.Format("DEBUG: Outdated Designs " + _tick.ToShortTimeString() + " != " + oCheck.ToShortTimeString()), EventLogEntryType.Information);
                }
            }   
  
        }
        private void ImportTSMRegistrations()
        {
            try
            {
                DateTime oCheck = oStart;
                string[] strTimes;
                char[] strSplit = { ';' };
                strTimes = strTSMImportTime.Split(strSplit);
                for (int ii = 0; ii < strTimes.Length; ii++)
                {
                    if (strTimes[ii].Trim() != "")
                    {
                        DateTime _tick = DateTime.Parse(strTimes[ii].Trim());
                        if (_tick.ToShortTimeString() == oCheck.ToShortTimeString())
                        {
                            boolDone = true;
                            if (File.Exists(strTSMImportLocation) == true)
                            {
                                TSM oTSM = new TSM(0, dsn);

                                oLog.AddEvent("", "", "TSM Import File " + strTSMImportLocation + " exists...reading...", LoggingType.Information);
                                // Import TSM
                                DataTable dt = new DataTable();
                                int counter = 0;
                                using (TextReader tr = File.OpenText(strTSMImportLocation))
                                {
                                    string line;
                                    while ((line = tr.ReadLine()) != null)
                                    {
                                        string[] items = line.Split('\t');
                                        if (dt.Columns.Count == 0)
                                        {
                                            // Create the data columns for the data table based on the number of items
                                            // on the first line of the file
                                            for (int jj = 0; jj < items.Length; jj++)
                                                dt.Columns.Add(new DataColumn("Column" + jj, typeof(string)));
                                        }
                                        dt.Rows.Add(items);
                                    }
                                }
                                if (boolTSMImportAll == true)
                                    oTSM.DeleteRegistrations();

                                foreach (DataRow dr in dt.Rows)
                                {
                                    /*
                                    // Print out all the values
                                    Response.Write("<p>");
                                    foreach (string s in dr.ItemArray)
                                        Response.Write(s + "<br/>");
                                    Response.Write("</p>");
                                    */

                                    string strName = dr[0].ToString();
                                    string strServer = dr[2].ToString();
                                    string strPort = dr[3].ToString();
                                    string strDomain = dr[4].ToString();
                                    string strSchedule = dr[5].ToString();

                                    if (boolTSMImportAll == false)
                                    {
                                        bool boolAdd = false;
                                        DataSet dsCurrent = oTSM.GetRegistration(strName);
                                        if (dsCurrent.Tables[0].Rows.Count == 1)
                                        {
                                            DataRow drCurrent = dsCurrent.Tables[0].Rows[0];
                                            if (strServer.Trim().ToUpper() != drCurrent["server"].ToString().Trim().ToUpper())
                                            {
                                                oLog.AddEvent(strName, "", "TSM Registration information has changed - Server = " + drCurrent["server"].ToString() + " to " + strServer, LoggingType.Debug);
                                                boolAdd = true;
                                            }
                                            else if (strPort.Trim().ToUpper() != drCurrent["port"].ToString().Trim().ToUpper())
                                            {
                                                oLog.AddEvent(strName, "", "TSM Registration information has changed - Port = " + drCurrent["port"].ToString() + " to " + strPort, LoggingType.Debug);
                                                boolAdd = true;
                                            }
                                            else if (strDomain.Trim().ToUpper() != drCurrent["domain"].ToString().Trim().ToUpper())
                                            {
                                                oLog.AddEvent(strName, "", "TSM Registration information has changed - Domain = " + drCurrent["domain"].ToString() + " to " + strDomain, LoggingType.Debug);
                                                boolAdd = true;
                                            }
                                            else if (strSchedule.Trim().ToUpper() != drCurrent["schedule"].ToString().Trim().ToUpper())
                                            {
                                                oLog.AddEvent(strName, "", "TSM Registration information has changed - Schedule = " + drCurrent["schedule"].ToString() + " to " + strSchedule, LoggingType.Debug);
                                                boolAdd = true;
                                            }
                                        }
                                        else
                                        {
                                            oLog.AddEvent(strName, "", "TSM Registration information does not exist", LoggingType.Debug);
                                            boolAdd = true;
                                        }

                                        if (boolAdd == true)
                                        {
                                            oTSM.AddRegistration(strName, strServer, strPort, strDomain, strSchedule);
                                            oLog.AddEvent(strName, "", "TSM Registration information has been synchronized", LoggingType.Debug);
                                            counter++;
                                        }
                                    }
                                    else
                                    {
                                        oTSM.AddRegistration(strName, strServer, strPort, strDomain, strSchedule);
                                        counter++;
                                    }
                                }

                                oLog.AddEvent("", "", "Clearview modified " + counter.ToString() + " TSM registrations", LoggingType.Information);
                                if (intLog > 0)
                                    oEventLog.WriteEntry(String.Format("REMINDER: TSM Registration - Total Imports: " + counter.ToString()), EventLogEntryType.Information);
                            }
                            else
                                oLog.AddEvent("", "", "TSM Import File " + strTSMImportLocation + " does not exist!", LoggingType.Error);
                        }
                        else if (intTest == 1)
                            oEventLog.WriteEntry(String.Format("DEBUG: " + _tick.ToShortTimeString() + " != " + oCheck.ToShortTimeString()), EventLogEntryType.Information);
                    }
                }
            }
            catch (Exception oError)
            {
                oEventLog.WriteEntry(String.Format("ERROR: " + oError.Message), EventLogEntryType.Error);
            }
        }

        private void CheckProductionDateForCompletedBuilds()
        {
            DateTime oCheck = oStart;            
            string[] strTimes;
            char[] strSplit = { ';' };
            strTimes = strProdTime.Split(strSplit);
            for (int ii = 0; ii < strTimes.Length; ii++)
            {
                if (strTimes[ii].Trim() != "")
                {
                    DateTime _tick = DateTime.Parse(strTimes[ii].Trim());
                    if (_tick.ToShortTimeString() == oCheck.ToShortTimeString())
                    {
                        boolDone = true;
                        try
                        {
                            int intServerType = Int32.Parse(ConfigurationManager.AppSettings["Type_id"]);
                            int intProductionClassid = Int32.Parse(ConfigurationManager.AppSettings["ProductionClassID"]);
                            DataSet ds = oForecast.GetAnswerComplete();
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                string strBody = "";
                                if (dr["completed"].ToString() != "" && dr["production"].ToString() == "" && (dr["classid"].ToString() == intProductionClassid.ToString()))
                                {
                                    Projects oProject = new Projects(0, dsn);
                                    Requests oRequest = new Requests(0, dsn);

                                    int intId = Int32.Parse(dr["id"].ToString());
                                    int intUser = Int32.Parse(dr["userid"].ToString());
                                    int intForecast = Int32.Parse(dr["forecastid"].ToString());
                                    if (intForecast > 0)
                                    {
                                        int intRequest = Int32.Parse(oForecast.Get(intForecast, "requestid"));
                                        int intProject = oRequest.GetProjectNumber(intRequest);
                                        DataSet dsProject = oProject.Get(intProject);
                                        if (dsProject.Tables[0].Rows.Count > 0)
                                        {
                                            int intEngineer = Int32.Parse(dsProject.Tables[0].Rows[0]["engineer"].ToString());
                                            int intLead = Int32.Parse(dsProject.Tables[0].Rows[0]["lead"].ToString());

                                            strBody = "<table cellpadding=\"0\" cellspacing=\"0\" style=\"" + oVariable.DefaultFontStyle() + "\" border=\"0\">";
                                            strBody += "<tr><td>&nbsp;</td></tr>";
                                            strBody += "<tr><td><b>NOTE:</b> Reminders will now be sent everyday regarding this design until appropriate action has been taken. Contact your ClearView administrator or Josh Ciora for more information.</td></tr>";
                                            strBody += "<tr><td>&nbsp;</td></tr>";
                                            strBody += "<tr><td colspan=\"2\">";
                                            strBody += "<table cellpadding=\"0\" cellspacing=\"0\" style=\"" + oVariable.DefaultFontStyle() + "\" border=\"0\">";
                                            strBody += "<tr><td rowspan=\"2\"><img src=\"" + oVariable.ImageURL() + "/images/bigalert.gif\" border=\"0\" align=\"absmiddle\" /></td><td nowrap valign=\"bottom\">&nbsp;&nbsp;<a href=\"" + oVariable.URL() + "/redirect.aspx?referrer=/frame/production.aspx?id=" + intId + "\">Click here to update production go date</a></td></tr>";
                                            strBody += "<tr><td nowrap valign=\"top\">&nbsp;&nbsp;(NOTE: You must click this link to avoid future communications)</td></tr>";
                                            strBody += "</table></td></tr>";
                                            strBody += "<tr><td>&nbsp;</td></tr>";
                                            strBody += "<tr><td>&nbsp;</td></tr>";
                                            strBody += "<tr><td>&nbsp;</td></tr>";
                                            strBody += "<tr><td nowrap><a href=\"" + oVariable.URL() + "/redirect.aspx?referrer=/frame/forecast_equipment.aspx?id=" + intId.ToString() + "\">Click here to view your design</a></td></tr>";
                                            strBody += "<tr><td>&nbsp;</td></tr>";
                                            strBody += "<tr><td nowrap><b><u>Design Details</u></b></td></tr>";
                                            strBody += "<tr><td>&nbsp;</td></tr>";
                                            strBody += "<tr><td colspan=\"3\">" + oForecast.GetAnswerBody(intId, intEnvironment, dsnAsset, dsnIP) + "</td></tr>";
                                            strBody += "</table>";
                                            string strTO = oUser.GetName(intUser);
                                            if (strTO == "")
                                                strTO = oUser.GetName(intEngineer);
                                            if (strTO == "")
                                                strTO = oUser.GetName(intLead);

                                            strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DAILY_NOTIFICATION");
                                            oFunction.SendEmail("Update Production Go Live Date", strTO, oUser.GetName(intEngineer) + ";" + oUser.GetName(intLead), strEMailIdsBCC, "ClearView Design Production Go Live Update (Action Required)", strBody, true, false);
                                        }
                                        else
                                        {
                                            strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DEVELOPER_ERROR");
                                            oFunction.SendEmail("ERROR: Check Production Date for Completed Builds", strEMailIdsBCC, "", "", "ERROR: Check Production Date for Completed Builds", strPreface + "<p><b>An error occurred when attempting to send a ClearView Reminder...</b></p><p>Invalid Project Information for ForecastID:" + intForecast.ToString() + "</p>", true, false);
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception oError)
                        {
                            strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DEVELOPER_ERROR");
                            oFunction.SendEmail("ERROR: Check Production Date for Completed Builds", strEMailIdsBCC, "", "", "ERROR: Check Production Date for Completed Builds", strPreface + "<p><b>An error occurred when attempting to send a ClearView Reminder...</b></p><p>Error Message:" + oError.Message + "</p>", true, false);
                        }
                    }
                    else if (intTest == 1)
                        oEventLog.WriteEntry(String.Format("DEBUG: Check Production Date for Completed Builds " + _tick.ToShortTimeString() + " != " + oCheck.ToShortTimeString()), EventLogEntryType.Information);
                }
            }    
        }
        public void DecommissionEarlyWarning()
        {
            string strErrorName = "";
            try
            {
                strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DAILY_NOTIFICATION");
                DataSet ds = oAsset.GetDecommissionWarnings();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    strErrorName = dr["name"].ToString();
                    int intDecommissionID = Int32.Parse(dr["id"].ToString());
                    DateTime datDecom = DateTime.Parse(dr["decom"].ToString());
                    int intUser = Int32.Parse(dr["userid"].ToString());
                    int intDays = Int32.Parse(dr["days"].ToString());
                    string strRequest = dr["number"].ToString();
                    int intWarningID = Int32.Parse(dr["warningid"].ToString());
                    string strEmails = dr["emails"].ToString();
                    oLog.AddEvent(strErrorName, "", "Sending Decommission Warning of " + datDecom.ToShortDateString() + " (" + intDays.ToString() + " days) to " + strEmails, LoggingType.Information);
                    StringBuilder strBody = new StringBuilder();
                    strBody.AppendLine("<p>This message is to notify you that <b>" + strErrorName.ToUpper() + "</b> will be decommissioned on " + datDecom.ToLongDateString() + ".</p>");
                    strBody.AppendLine("<p>The request was submitted by <b>" + oUser.GetFullName(intUser) + " (" + oUser.GetName(intUser) + ")</b> via <b>" + strRequest.ToUpper() + "</b>.</p>");
                    strBody.AppendLine("<p><b>NOTE</b> This will be the only early warning notification sent to you regarding this decommission.</p>");
                    strBody.AppendLine("<p><b>NOTE</b> If this decommission request is cancelled, or if the server is recommissioned, you will not be notified. This message only serves to notify you of a pending decommission. You should use the ClearView reports for up to the minute information regarding decommissions.</p>");
                    oFunction.SendEmail("Decommission Early Warning Notification", strEmails, "", strEMailIdsBCC, "Decommission Early Warning Notification", strBody.ToString(), false, false);
                    oAsset.AddDecommissionWarningSent(intDecommissionID, intWarningID);
                    oLog.AddEvent(strErrorName, "", "Decommission Warning Sent!", LoggingType.Information);
                }
            }
            catch (Exception oError)
            {
                strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DEVELOPER_ERROR");
                string strError = "(Error Message: " + oError.Message + ") (Source: " + oError.Source + ") (Stack Trace: " + oError.StackTrace + ") [" + System.Environment.UserName + "]";
                oEventLog.WriteEntry(strError, EventLogEntryType.Error);
                oFunction.SendEmail("ERROR: Decommission Early Warning", strEMailIdsBCC, "", "", "ERROR: Decommission Early Warning", strPreface + "<p><b>An error occurred when attempting to send a ClearView Reminder...</b></p><p>Name: " + strErrorName + "</p><p>Error Message:" + strError + "</p>", true, false);
            }

        }

        public void GenerateProdSAN()
        {
            int intGenerateProdSANError = 0;
            try
            {
                int intStorageItem = Int32.Parse(ConfigurationManager.AppSettings["Storage_item"]);
                int intStorageService = Int32.Parse(ConfigurationManager.AppSettings["Storage_service"]);
                int intServerType = Int32.Parse(ConfigurationManager.AppSettings["Type_id"]);
                int intProductionClassid = Int32.Parse(ConfigurationManager.AppSettings["ProductionClassID"]);
                int intModel = 0;
                int intQuantity = 0;
                DateTime dtProduction;
                DateTime dtNotify;
                DataSet ds = oForecast.GetAnswerComplete();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (dr["completed"].ToString() != "" && dr["production"].ToString() != "" && (dr["classid"].ToString() == intProductionClassid.ToString()))
                    //if (dr["completed"].ToString() != "" && dr["production"].ToString() != "" && oClass.Get(Int32.Parse(dr["classid"].ToString()), "prod") == "1")
                    {
                        dtProduction = DateTime.Parse(dr["production"].ToString());
                        dtNotify = oHoliday.GetDaysBack(10, dtProduction);
                        if (dtNotify <= DateTime.Now && dtProduction >= DateTime.Now) 
                        {
                            int intAnswer = Int32.Parse(dr["id"].ToString());
                            intGenerateProdSANError = intAnswer;
                            if (oForecast.IsStorage(intAnswer) == true && oForecast.GetAnswer(intAnswer, "storage") == "1")
                            {
                                intModel = Int32.Parse(dr["modelid"].ToString());
                                if (intModel == 0)
                                    intModel = oForecast.GetModelAsset(intAnswer);
                                if (intModel == 0)
                                    intModel = oForecast.GetModel(intAnswer);
                                if (oModelsProperties.IsTypeVMware(intModel) == false)
                                {
                                    DataSet dsProd = oOnDemandTasks.GetProd(intAnswer);
                                    if (dsProd.Tables[0].Rows.Count > 0)
                                    {
                                        if (dsProd.Tables[0].Rows[0]["prod"].ToString() != "")
                                        {
                                            int intRequest = oForecast.GetRequestID(intAnswer, true);
                                            DataSet dsAlready = oOnDemandTasks.GetServerStorageProd(intRequest, intStorageItem, intAnswer);
                                            if (dsAlready.Tables[0].Rows.Count == 0)
                                            {
                                                boolDone = true;
                                                intQuantity = oServer.GetAnswer(intAnswer).Tables[0].Rows.Count;
                                                oEventLog.WriteEntry("GenerateProdSAN: Sending Prod notification for " + intAnswer.ToString(), EventLogEntryType.Warning);
                                                int intStorageNumber = oResourceRequest.GetNumber(intRequest, intStorageItem);
                                                oOnDemandTasks.AddServerStorage(intRequest, intStorageItem, intStorageNumber, intAnswer, 1, intModel);
                                                oServiceRequest.Add(intRequest, 1, 1);
                                                int intStorage = oResourceRequest.Add(intRequest, intStorageItem, intStorageService, intStorageNumber, "Move to Production (Storage)", intQuantity, 0.00, 2, 1, 1, 1);
                                                if (oServiceRequest.NotifyApproval(intStorage, intResourceRequestApprove, intEnvironment, "", dsnServiceEditor) == false)
                                                    oServiceRequest.NotifyTeamLead(intStorageItem, intStorage, intAssignPage, intViewPage, intEnvironment,  "", dsnServiceEditor, dsnAsset, dsnIP, 0);
                                                oEventLog.WriteEntry("GenerateProdSAN: Sending Prod notification for " + intAnswer.ToString() + " = SUCCESS", EventLogEntryType.Information);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception oError)
            {
                strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DEVELOPER_ERROR");
                string strError = "(Error Message: " + oError.Message + ") (Source: " + oError.Source + ") (Stack Trace: " + oError.StackTrace + ") [" + System.Environment.UserName + "]";
                oEventLog.WriteEntry(strError, EventLogEntryType.Error);
                oFunction.SendEmail("ERROR: Generate Prod SAN", strEMailIdsBCC, "", "", "ERROR: Generate Prod SAN", strPreface + "<p><b>An error occurred when attempting to send a ClearView Reminder...</b></p><p>DesignID: " + intGenerateProdSANError.ToString() + "</p><p>Error Message:" + strError + "</p>", true, false);
            }
             
        }
        public void RegisterAvamar()
        {
        }
        public void RegisterTSM()
        {
            string strErrorName = "";
            try
            {
                TSM oTSM = new TSM(0, dsn);
                Domains oDomain = new Domains(0, dsn);
                OperatingSystems oOperatingSystem = new OperatingSystems(0, dsn);
                IPAddresses oIPAddress = new IPAddresses(0, dsnIP, dsn);
                Settings oSetting = new Settings(0, dsn);
                Locations oLocation = new Locations(0, dsn);
                Resiliency oResiliency = new Resiliency(0, dsn);
                PNCTasks oPNCTasks = new PNCTasks(0, dsn);
                int intTSMAutomated = 0;
                Int32.TryParse(oSetting.Get("tsm_automated"), out intTSMAutomated);
                int intAvamarAutomated = 0;
                Int32.TryParse(oSetting.Get("avamar_automated"), out intAvamarAutomated);
                DateTime _now = DateTime.Now;
                int intViewPage = Int32.Parse(ConfigurationManager.AppSettings["View_page"]);
                string strNow = _now.Day.ToString() + _now.Month.ToString() + _now.Year.ToString() + _now.Hour.ToString() + _now.Minute.ToString() + _now.Second.ToString() + _now.Millisecond.ToString();
                string strDSMADMC = ConfigurationManager.AppSettings["DSMADMC"];
                DataSet ds = oServer.GetTSMs();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    int intServer = Int32.Parse(dr["id"].ToString());
                    int intOS = Int32.Parse(dr["osid"].ToString());
                    int intAnswer = Int32.Parse(dr["answerid"].ToString());
                    int intRequest = oForecast.GetRequestID(intAnswer, true);
                    int intModel = oForecast.GetModelAsset(intAnswer);
                    if (intModel == 0)
                        intModel = oForecast.GetModel(intAnswer);
                    int intClass = 0;
                    Int32.TryParse(oForecast.GetAnswer(intAnswer, "classid"), out intClass);
                    int intAvamar = 0;
                    Int32.TryParse(oForecast.GetAnswer(intAnswer, "avamar"), out intAvamar);
                    int intAddress = 0;
                    Int32.TryParse(oForecast.GetAnswer(intAnswer, "addressid"), out intAddress);
                    int intTSM = 0;
                    //Int32.TryParse(oLocation.GetAddress(intRecovery, "tsm"), out intTSM);
                    Int32.TryParse(oLocation.GetAddress(intAddress, "tsm"), out intTSM);
                    int intMnemonic = 0;
                    if (oForecast.GetAnswer(intAnswer, "mnemonicid") != "")
                        intMnemonic = Int32.Parse(oForecast.GetAnswer(intAnswer, "mnemonicid"));
                    bool boolBIR = (oForecast.GetAnswer(intAnswer, "resiliency") == "1");    // Related to the Business Resiliency Effort (use old data center strategy)
                    int intResiliency = oResiliency.GetIDFromMnemonic(intMnemonic, boolBIR, intAnswer);
                    int intRecovery = 0;
                    // Recovery Location based on data center strategy
                    DataSet dsRecovery = oResiliency.GetLocations(intResiliency);
                    if (dsRecovery.Tables[0].Rows.Count == 0)
                    {
                        // There are no locations, which means it is the OLD data center strategy (under 48 hours and no BIR).
                        Int32.TryParse(dr["recoveryid"].ToString(), out intRecovery);
                    }
                    else
                    {
                        foreach (DataRow drRecovery in dsRecovery.Tables[0].Rows)
                        {
                            if (Int32.Parse(drRecovery["prodID"].ToString()) == intAddress) 
                            {
                                if (Int32.TryParse(drRecovery["drID"].ToString(), out intRecovery) == true)
                                    break;
                            }
                        }
                    }

                    int intSVEHost = 0;
                    // intSVEHost will only be set for Database Containers (since that query has been applied in the stored procedure)
                    Int32.TryParse(dr["sve_hostid"].ToString(), out intSVEHost);
                    bool boolProd = (oClass.IsProd(intClass));
                    bool boolQA = (oClass.IsQA(intClass));
                    bool boolTest = (oClass.IsTestDev(intClass));
                    bool boolWindows = (oOperatingSystem.IsWindows(intOS) == true || oOperatingSystem.IsWindows2008(intOS) == true);

                    bool boolDaily = (dr["daily"].ToString() == "1");
                    bool boolWeekly = (dr["weekly"].ToString() == "1");
                    string strWeekly = dr["weekly_day"].ToString();
                    bool boolMonthly = (dr["monthly"].ToString() == "1");
                    string strStartTime = dr["time_hour"].ToString() + " " + dr["time_switch"].ToString();
                    //DateTime datStartDate = DateTime.Parse(dr["start_date"].ToString());
                    string strFrequency = dr["backup_frequency"].ToString();

                    string strName = oServer.GetName(intServer, true);
                    strErrorName = strName;
                    string strRegister = dr["tsm_register"].ToString();
                    string strDefine = dr["tsm_define"].ToString();
                    bool boolAutomated = false;
                    int intSchedule = 0;
                    string strTSMPreview = "<tr>";
                    if (strRegister == "" && strDefine == "" && ((intTSM == 1 && intTSMAutomated == 1) || (intAvamar == 1 && intAvamarAutomated == 1)))
                    {
                        oLog.AddEvent(intAnswer, strName, "", "Beginning automated TSM Registration...", LoggingType.Information);
                        // The TSM Registerd flag is probably set to avoid the task from being generated.
                        if (dr["tsm_registered"].ToString() != "")
                        {
                            // Clear it so that no matter what happens, it will kick out a manual workflow next time.
                            oServer.UpdateTSMRegistered(intServer, "");
                            oServer.UpdateTSM(intServer, "");
                            oLog.AddEvent(intAnswer, strName, "", "TSM Registered Date cleared", LoggingType.Debug);
                        }

                        if (intSVEHost == 0)
                        {
                            oLog.AddEvent(intAnswer, strName, "", "Querying for backup servers...Recovery Location: " + oLocation.GetFull(intRecovery) + " (" + intRecovery.ToString() + "), TSM: " + (intTSM == 1 ? "Yes" : "No") + " (" + intTSM.ToString() + "), Avamar: " + (intAvamar == 1 ? "Yes" : "No") + " (" + intAvamar.ToString() + "), Test: " + (boolTest ? "Yes" : "No") + ", QA: " + (boolQA ? "Yes" : "No") + ", Prod: " + (boolProd ? "Yes" : "No") + ", Windows: " + (boolWindows ? "Yes" : "No") + ", Resiliency: " + oResiliency.Get(intResiliency, "name") + ", ServerID: " + intServer.ToString(), LoggingType.Information);
                            DataSet dsTSMAutomated = oTSM.Gets(intRecovery, intTSM, intAvamar, false, boolTest, boolQA, boolProd, boolWindows, (boolWindows == false), intResiliency, intServer);
                            oLog.AddEvent(intAnswer, strName, "", "Found " + dsTSMAutomated.Tables[0].Rows.Count.ToString() + " backup servers / schedules available...", LoggingType.Information);
                            foreach (DataRow drTSMAutomated in dsTSMAutomated.Tables[0].Rows)
                            {
                                oLog.AddEvent(intAnswer, strName, "", "Trying " + drTSMAutomated["schedule"].ToString() + " on " + drTSMAutomated["tsm"].ToString(), LoggingType.Debug);
                                // At this point, we have a list of available TSM servers / schedules based on class and OS.
                                // Now, we need to narrow it down based on the frequency and time.
                                bool boolValidFrequency = false;
                                bool boolValidTime = false;

                                if (strFrequency != "")
                                {
                                    boolDaily = (strFrequency == "D");
                                    boolWeekly = (strFrequency == "W");
                                    boolMonthly = (strFrequency == "M");
                                    boolValidTime = true;   // There is no time to compare...just use whatever is least utilized.
                                }

                                // Check the frequency (daily, weekly, monthly)
                                if (boolDaily && drTSMAutomated["daily"].ToString() == "1")
                                    boolValidFrequency = true;
                                else if (boolWeekly && drTSMAutomated["weekly"].ToString() == "1")
                                {
                                    if (strFrequency == "")
                                    {
                                        strWeekly = strWeekly.Trim().ToUpper();
                                        // Check the day of the week
                                        if (strWeekly == "SUNDAY" && drTSMAutomated["sunday"].ToString() == "1")
                                            boolValidFrequency = true;
                                        else if (strWeekly == "MONDAY" && drTSMAutomated["monday"].ToString() == "1")
                                            boolValidFrequency = true;
                                        else if (strWeekly == "TUESDAY" && drTSMAutomated["tuesday"].ToString() == "1")
                                            boolValidFrequency = true;
                                        else if (strWeekly == "WEDNESDAY" && drTSMAutomated["wednesday"].ToString() == "1")
                                            boolValidFrequency = true;
                                        else if (strWeekly == "THURSDAY" && drTSMAutomated["thursday"].ToString() == "1")
                                            boolValidFrequency = true;
                                        else if (strWeekly == "FRIDAY" && drTSMAutomated["friday"].ToString() == "1")
                                            boolValidFrequency = true;
                                        else if (strWeekly == "SATURDAY" && drTSMAutomated["saturday"].ToString() == "1")
                                            boolValidFrequency = true;
                                    }
                                    else
                                    {
                                        // No day specified, just pick least utilized
                                        boolValidFrequency = true;
                                    }
                                }
                                else if (boolMonthly && drTSMAutomated["monthly"].ToString() == "1")
                                    boolValidFrequency = true;

                                if (boolValidFrequency == true && boolValidTime == false)
                                {
                                    // Check the start time
                                    if (strStartTime == "12:00 AM" && drTSMAutomated["AM1200"].ToString() == "1")
                                        boolValidTime = true;
                                    else if (strStartTime == "12:30 AM" && drTSMAutomated["AM1230"].ToString() == "1")
                                        boolValidTime = true;
                                    else if (strStartTime == "1:00 AM" && drTSMAutomated["AM100"].ToString() == "1")
                                        boolValidTime = true;
                                    else if (strStartTime == "1:30 AM" && drTSMAutomated["AM130"].ToString() == "1")
                                        boolValidTime = true;
                                    else if (strStartTime == "2:00 AM" && drTSMAutomated["AM200"].ToString() == "1")
                                        boolValidTime = true;
                                    else if (strStartTime == "2:30 AM" && drTSMAutomated["AM230"].ToString() == "1")
                                        boolValidTime = true;
                                    else if (strStartTime == "3:00 AM" && drTSMAutomated["AM300"].ToString() == "1")
                                        boolValidTime = true;
                                    else if (strStartTime == "3:30 AM" && drTSMAutomated["AM330"].ToString() == "1")
                                        boolValidTime = true;
                                    else if (strStartTime == "4:00 AM" && drTSMAutomated["AM400"].ToString() == "1")
                                        boolValidTime = true;
                                    else if (strStartTime == "4:30 AM" && drTSMAutomated["AM430"].ToString() == "1")
                                        boolValidTime = true;
                                    else if (strStartTime == "5:00 AM" && drTSMAutomated["AM500"].ToString() == "1")
                                        boolValidTime = true;
                                    else if (strStartTime == "5:30 AM" && drTSMAutomated["AM530"].ToString() == "1")
                                        boolValidTime = true;
                                    else if (strStartTime == "6:00 AM" && drTSMAutomated["AM600"].ToString() == "1")
                                        boolValidTime = true;
                                    else if (strStartTime == "6:30 AM" && drTSMAutomated["AM630"].ToString() == "1")
                                        boolValidTime = true;
                                    else if (strStartTime == "7:00 AM" && drTSMAutomated["AM700"].ToString() == "1")
                                        boolValidTime = true;
                                    else if (strStartTime == "7:30 AM" && drTSMAutomated["AM730"].ToString() == "1")
                                        boolValidTime = true;
                                    else if (strStartTime == "8:00 AM" && drTSMAutomated["AM800"].ToString() == "1")
                                        boolValidTime = true;
                                    else if (strStartTime == "8:30 AM" && drTSMAutomated["AM830"].ToString() == "1")
                                        boolValidTime = true;
                                    else if (strStartTime == "9:00 AM" && drTSMAutomated["AM900"].ToString() == "1")
                                        boolValidTime = true;
                                    else if (strStartTime == "9:30 AM" && drTSMAutomated["AM930"].ToString() == "1")
                                        boolValidTime = true;
                                    else if (strStartTime == "10:00 AM" && drTSMAutomated["AM1000"].ToString() == "1")
                                        boolValidTime = true;
                                    else if (strStartTime == "10:30 AM" && drTSMAutomated["AM1030"].ToString() == "1")
                                        boolValidTime = true;
                                    else if (strStartTime == "11:00 AM" && drTSMAutomated["AM1100"].ToString() == "1")
                                        boolValidTime = true;
                                    else if (strStartTime == "11:30 AM" && drTSMAutomated["AM1130"].ToString() == "1")
                                        boolValidTime = true;
                                    else if (strStartTime == "12:00 PM" && drTSMAutomated["PM1200"].ToString() == "1")
                                        boolValidTime = true;
                                    else if (strStartTime == "12:30 PM" && drTSMAutomated["PM1230"].ToString() == "1")
                                        boolValidTime = true;
                                    else if (strStartTime == "1:00 PM" && drTSMAutomated["PM100"].ToString() == "1")
                                        boolValidTime = true;
                                    else if (strStartTime == "1:30 PM" && drTSMAutomated["PM130"].ToString() == "1")
                                        boolValidTime = true;
                                    else if (strStartTime == "2:00 PM" && drTSMAutomated["PM200"].ToString() == "1")
                                        boolValidTime = true;
                                    else if (strStartTime == "2:30 PM" && drTSMAutomated["PM230"].ToString() == "1")
                                        boolValidTime = true;
                                    else if (strStartTime == "3:00 PM" && drTSMAutomated["PM300"].ToString() == "1")
                                        boolValidTime = true;
                                    else if (strStartTime == "3:30 PM" && drTSMAutomated["PM330"].ToString() == "1")
                                        boolValidTime = true;
                                    else if (strStartTime == "4:00 PM" && drTSMAutomated["PM400"].ToString() == "1")
                                        boolValidTime = true;
                                    else if (strStartTime == "4:30 PM" && drTSMAutomated["PM430"].ToString() == "1")
                                        boolValidTime = true;
                                    else if (strStartTime == "5:00 PM" && drTSMAutomated["PM500"].ToString() == "1")
                                        boolValidTime = true;
                                    else if (strStartTime == "5:30 PM" && drTSMAutomated["PM530"].ToString() == "1")
                                        boolValidTime = true;
                                    else if (strStartTime == "6:00 PM" && drTSMAutomated["PM600"].ToString() == "1")
                                        boolValidTime = true;
                                    else if (strStartTime == "6:30 PM" && drTSMAutomated["PM630"].ToString() == "1")
                                        boolValidTime = true;
                                    else if (strStartTime == "7:00 PM" && drTSMAutomated["PM700"].ToString() == "1")
                                        boolValidTime = true;
                                    else if (strStartTime == "7:30 PM" && drTSMAutomated["PM730"].ToString() == "1")
                                        boolValidTime = true;
                                    else if (strStartTime == "8:00 PM" && drTSMAutomated["PM800"].ToString() == "1")
                                        boolValidTime = true;
                                    else if (strStartTime == "8:30 PM" && drTSMAutomated["PM830"].ToString() == "1")
                                        boolValidTime = true;
                                    else if (strStartTime == "9:00 PM" && drTSMAutomated["PM900"].ToString() == "1")
                                        boolValidTime = true;
                                    else if (strStartTime == "9:30 PM" && drTSMAutomated["PM930"].ToString() == "1")
                                        boolValidTime = true;
                                    else if (strStartTime == "10:00 PM" && drTSMAutomated["PM1000"].ToString() == "1")
                                        boolValidTime = true;
                                    else if (strStartTime == "10:30 PM" && drTSMAutomated["PM1030"].ToString() == "1")
                                        boolValidTime = true;
                                    else if (strStartTime == "11:00 PM" && drTSMAutomated["PM1100"].ToString() == "1")
                                        boolValidTime = true;
                                    else if (strStartTime == "11:30 PM" && drTSMAutomated["PM1130"].ToString() == "1")
                                        boolValidTime = true;
                                }

                                if (boolValidFrequency == true && boolValidTime == true)
                                {
                                    intSchedule = Int32.Parse(drTSMAutomated["id"].ToString());
                                    string strTSM = drTSMAutomated["tsm"].ToString();
                                    string strTSMPort = drTSMAutomated["port"].ToString();
                                    string strTSMDomain = drTSMAutomated["domain"].ToString();
                                    string strTSMSchedule = drTSMAutomated["schedule"].ToString();
                                    string strTSMCloptset = drTSMAutomated["cloptset"].ToString();

                                    oLog.AddEvent(intAnswer, strName, "", strTSMSchedule + " is valid", LoggingType.Information);
                                    // Build the Register statement.
                                    // FORMAT: REGISTER NODE OHCLEIIS406P OHCLEIIS406P USERID=none CONTACT="" DOMAIN=TEST  CLOPTSET=WIN32-PROMPTED FORCEPWRESET=NO URL=""
                                    strRegister = "REGISTER NODE " + strName + " " + strName + " USERID=none CONTACT=\"\" DOMAIN=" + strTSMDomain + " CLOPTSET=" + strTSMCloptset + " FORCEPWRESET=NO URL=\"\"";

                                    // Build the Define statement.
                                    // FORMAT: DEFINE ASSOCIATION TEST DAILY_INCR_0200_A OHCLEIIS406P
                                    strDefine = "DEFINE ASSOCIATION " + strTSMDomain + " " + strTSMSchedule + " " + strName;

                                    oServer.UpdateTSM(intServer, intSchedule, -999, strRegister, strDefine, 0);
                                    boolAutomated = true;

                                    strTSMPreview += "<td>" + strName + "</td>";
                                    strTSMPreview += "<td>" + strTSM + "</td>";
                                    strTSMPreview += "<td>" + strTSMPort + "</td>";
                                    strTSMPreview += "<td>" + strTSMSchedule + "</td>";
                                    break;
                                }
                            }
                            if (intSchedule == 0)
                            {
                                // No valid schedule was found...kick out a manual task.
                                oLog.AddEvent(intAnswer, strName, "", "Generating manual task", LoggingType.Information);
                                if (oForecast.GetAnswer(intAnswer, "completed") == "")
                                {
                                    // Update Forcast Completed so task will fire.  Should be overridden at the end.
                                    oForecast.UpdateAnswerCompleted(intAnswer);
                                }
                                // Initiate Pre-Prod Tasks
                                oPNCTasks.InitiateNextStep(intRequest, intAnswer, intModel, intEnvironment, intAssignPage, intViewPage, dsnAsset, dsnIP, dsnServiceEditor);
                            }
                        }
                    }
                    else
                        oLog.AddEvent(intAnswer, strName, "", "Automated TSM Registration skipped, using assigned values...", LoggingType.Debug);

                    if (intSVEHost > 0 || (strRegister != "" && strDefine != ""))
                    {
                        string strContainer = strName;
                        string strIP = "";
                        DataSet dsIP = oServer.GetIP(intServer, 0, 0, 0, 0);
                        foreach (DataRow drIP in dsIP.Tables[0].Rows)
                        {
                            string strIPAddress = oIPAddress.GetName(Int32.Parse(drIP["ipaddressid"].ToString()));
                            Ping oPing = new Ping();
                            try
                            {
                                PingReply oPingReply = oPing.Send(strIPAddress);
                                if (oPingReply.Status.ToString().ToUpper() == "SUCCESS")
                                {
                                    strIP = strIPAddress;
                                    break;
                                }
                            }
                            catch { }
                        }

                        int intAssetLatest = 0;
                        int intAssetDR = 0;
                        DataSet dsAssets = oServer.GetAssets(intServer);
                        foreach (DataRow drAsset in dsAssets.Tables[0].Rows)
                        {
                            if (drAsset["latest"].ToString() == "1")
                                intAssetLatest = Int32.Parse(drAsset["assetid"].ToString());
                            else if (drAsset["dr"].ToString() == "1")
                                intAssetDR = Int32.Parse(drAsset["assetid"].ToString());
                        }

                        if (intSVEHost > 0)
                            Int32.TryParse(oServer.Get(intSVEHost, "tsm_schedule"), out intSchedule);
                        if (intSchedule == 0)
                            intSchedule = Int32.Parse(dr["tsm_schedule"].ToString());
                        string strSchedule = oTSM.GetSchedule(intSchedule, "name");
                        int intDomain = Int32.Parse(oTSM.GetSchedule(intSchedule, "domain"));
                        string strDomain = oTSM.GetDomain(intDomain, "name");
                        int intTSMServer = Int32.Parse(oTSM.GetDomain(intDomain, "tsm"));
                        string strServer = oTSM.Get(intTSMServer, "name");
                        string strPort = oTSM.Get(intTSMServer, "port");
                        string strDSMOPT = oTSM.Get(intTSMServer, "path");
                        int intCloptset = Int32.Parse(dr["tsm_cloptset"].ToString());
                        string strCloptset = oTSM.GetCloptset(intCloptset, "name");
                        string strFile = strDSMADMC + intServer.ToString() + "_" + strNow;
                        //string strFile = strScripts + strSub + intServer.ToString() + "_" + strNow;
                        StreamWriter oMacro = new StreamWriter(strFile + ".mac");
                        if (intSVEHost == 0)
                        {
                            oMacro.WriteLine(strRegister);
                            oMacro.WriteLine(strDefine);
                        }
                        else
                        {
                            // This will only occur when TSM has been automated
                            // FORMAT: REGISTER NODE EPLDBT02_ORACLE oracletdp CONTACT=SDMRG107A DOMAIN=PNC_TEST COMPRESSION=CLIENT AUTOFSRENAME=NO ARCHDELETE=YES BACKDELETE=YES FORCEPWRESET=NO TYPE=CLIENT KEEPMP=NO MAXNUMMP=1 USERID=NONE VALIDATEPROTOCOL=NO TXNGROUPMAX=0 DATAWRITEPATH=ANY DATAREADPATH=ANY SESSIONINIT=CLIENTORSERVER
                            string strSVEHost = oServer.GetName(intSVEHost, true);
                            if (strContainer.EndsWith("A") == true)
                                strContainer = strContainer.Substring(0, strContainer.Length - 1);
                            strRegister = "REGISTER NODE " + strContainer.ToUpper() + "_ORACLE oracletdp CONTACT=" + strSVEHost.ToUpper() + " DOMAIN=" + strDomain + " COMPRESSION=CLIENT AUTOFSRENAME=NO ARCHDELETE=YES BACKDELETE=YES FORCEPWRESET=NO TYPE=CLIENT KEEPMP=NO MAXNUMMP=1 USERID=NONE VALIDATEPROTOCOL=NO TXNGROUPMAX=0 DATAWRITEPATH=ANY DATAREADPATH=ANY SESSIONINIT=CLIENTORSERVER";
                            oMacro.WriteLine(strRegister);
                            oServer.UpdateTSM(intServer, intSchedule, -999, strRegister, strSVEHost, 0);
                            strTSMPreview += "<td>" + strContainer.ToUpper() + "_ORACLE" + "</td>";
                            strTSMPreview += "<td>" + strServer + "</td>";
                            strTSMPreview += "<td>" + strPort + "</td>";
                            strTSMPreview += "<td>" + strSchedule + "</td>";
                            boolAutomated = true;
                        }
                        oMacro.Flush();
                        oMacro.Close();
                        StreamWriter oBatch = new StreamWriter(strFile + ".bat");
                        // FORMAT: "E:\Program Files\Tivoli\TSM\baclient\dsmadmc.exe" -tcps=BKPSRV-OC-T2 -tcpp=1500 -id=clearview -password=clearview macro /full/pathname/macro_filename > /full/pathname/outputfile.txt
                        string strScript = "\"" + strDSMADMC + "dsmadmc.exe\" -tcps=" + oTSM.Get(intTSMServer, "name") + " -tcpp=" + oTSM.Get(intTSMServer, "port") + " -id=clearview -password=clearview macro \"" + strFile + ".mac\" > \"" + strFile + ".txt\"";
                        oEventLog.WriteEntry(String.Format(strName + ": " + "TSM Registration Script: " + strScript), EventLogEntryType.FailureAudit);
                        oBatch.WriteLine(strScript);
                        oBatch.Flush();
                        oBatch.Close();

                        System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo("cmd.exe");
                        info.WorkingDirectory = strDSMADMC;
                        info.Arguments = "/c \"" + strFile + ".bat\"";
                        oLog.AddEvent(intAnswer, strName, "", "Beginning registration of schedule " + strSchedule + " on " + strServer + "...", LoggingType.Information);
                        System.Diagnostics.Process proc = System.Diagnostics.Process.Start(info);
                        bool boolTimeout = false;
                        proc.WaitForExit(intTimeoutDefault);
                        if (proc.HasExited == false)
                        {
                            proc.Kill();
                            boolTimeout = true;
                        }
                        proc.Close();
                        if (boolTimeout == false)
                        {
                            bool boolContent = false;
                            for (int ii = 0; ii < 10 && boolContent == false; ii++)
                            {
                                if (File.Exists(strFile + ".txt") == true)
                                {
                                    if (intLog > 0)
                                        oEventLog.WriteEntry(String.Format(strName + ": " + "TSM Output File Exists....Reading...."), EventLogEntryType.Information);
                                    string strContent = "";
                                    try
                                    {
                                        StreamReader oReader = new StreamReader(strFile + ".txt");
                                        strContent = oReader.ReadToEnd();
                                        if (strContent != "")
                                        {
                                            if (intLog > 1)
                                                oEventLog.WriteEntry(String.Format(strName + ": " + "Updating Database...."), EventLogEntryType.Information);
                                            boolContent = true;
                                            oServer.UpdateTSM(intServer, strContent);
                                            if (boolAutomated == true)
                                            {
                                                // Completed Successfully!
                                                oServer.UpdateTSMRegistered(intServer, DateTime.Now.ToString());

                                                DataSet dsManual = oOnDemandTasks.GetGenericII(intAnswer);
                                                if (dsManual.Tables[0].Rows.Count == 0 || dsManual.Tables[0].Rows[0]["chk5"].ToString() != "")
                                                {
                                                    // Update Build Ready Field / kick off next requests
                                                    oPNCTasks.InitiateNextStep(intRequest, intAnswer, intModel, intEnvironment, intAssignPage, intViewPage, dsnAsset, dsnIP, dsnServiceEditor);
                                                }
                                                
                                                // Send email to TSM Team
                                                strTSMPreview += "</tr>";
                                                string strTSMEmail = "";
                                                strTSMEmail = "<p>Your servers have been successfully registered to a TSM Server. Here are the details...</p>";
                                                strTSMEmail += "<p><table cellpadding=\"2\" cellspacing=\"1\" border=\"0\" style=\"" + oVariable.DefaultFontStyle() + "\"><tr><td><b>Server Name:</b></td><td><b>TCPSERVERADDRESS:</b></td><td><b>TCPPort:</b></td><td><b>Schedule:</b></td>" + strTSMPreview + "</table></p>";
                                                if (intSVEHost == 0)
                                                {
                                                    strTSMEmail += "<p>Please update the TSM client software and recycle the TSM services to pick up the next scheduled backup time (shown above).</p>";
                                                    strTSMEmail += "<p>Also check that the dsm.opt files have a valid include/exclude list to ensure the machine is only backing up what is required.</p>";
                                                }

                                                string strTSMTo = strTSMTeam;
                                                if (boolProd == true)
                                                    strTSMTo += strTSMTeamProd;
                                                else
                                                    strTSMTo += strTSMTeamTest;
                                                strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_TSM");
                                                oFunction.SendEmail((intSVEHost > 0 ? "TDP " : "") + "TSM Backup Information", strTSMTo, "", strEMailIdsBCC, (intSVEHost > 0 ? "TDP " : "") + "TSM Backup Information [" + oForecast.GetAnswer(intAnswer, "name") + "]", "<p>" + strTSMEmail + "</p>", true, false);
                                            }
                                            oLog.AddEvent(intAnswer, strName, "", "Finished registration of schedule " + strSchedule + " on " + strServer + "...", LoggingType.Information);
                                            if (intLog > 1)
                                                oEventLog.WriteEntry(String.Format(strName + ": " + "Database Updated!"), EventLogEntryType.Information);

                                            oReader.Close();
                                            // Delete Files
                                            if (strContent.Contains("Highest return code was 0") || strContent.Contains("Highest return code was 10"))
                                            {
                                                if (File.Exists(strFile + ".bat") == true)
                                                    File.Delete(strFile + ".bat");
                                                if (File.Exists(strFile + ".mac") == true)
                                                    File.Delete(strFile + ".mac");
                                                if (File.Exists(strFile + ".txt") == true)
                                                    File.Delete(strFile + ".txt");
                                                if (intLog > 0)
                                                    oEventLog.WriteEntry(String.Format(strName + ": " + "Files Deleted (" + strFile + ".xxx)"), EventLogEntryType.Information);
                                            }
                                            else
                                            {
                                                if (intLog > 0)
                                                    oEventLog.WriteEntry(String.Format(strName + ": " + "Error with return code..." + strContent), EventLogEntryType.Error);
                                                strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DEVELOPER_ERROR");
                                                oFunction.SendEmail("ERROR: TSM Registration Script Error", strEMailIdsBCC, "", "", "ERROR: TSM Registration Script Error", strPreface + "<p><b>An error occurred when attempting to auto-register server " + strName + " in TSM...</b></p><p>Output:" + strContent + "</p>", true, false);
                                            }
                                            //break;
                                        }
                                        else
                                        {
                                            if (intLog > 1)
                                                oEventLog.WriteEntry(String.Format(strName + ": " + "Found TSM Output File....but it is blank...."), EventLogEntryType.Information);
                                            oReader.Close();
                                            Thread.Sleep(5000);
                                        }
                                    }
                                    catch
                                    {
                                        if (intLog > 1)
                                            oEventLog.WriteEntry(String.Format(strName + ": " + "Cannot open TSM Output File....waiting 5 seconds...."), EventLogEntryType.Information);
                                        Thread.Sleep(5000);
                                    }
                                }
                                else
                                {
                                    if (intLog > 1)
                                        oEventLog.WriteEntry(String.Format(strName + ": " + "TSM Output File Does Not Exist....Waiting...."), EventLogEntryType.Information);
                                    Thread.Sleep(5000);
                                }
                            }
                            if (boolContent == false)
                            {
                                if (intLog > 0)
                                    oEventLog.WriteEntry(String.Format(strName + ": " + "Could Not Find TSM Output File...."), EventLogEntryType.Information);
                                oLog.AddEvent(intAnswer, strName, "", "Could Not Find TSM Output File", LoggingType.Error);
                                oServer.UpdateTSM(intServer, "[" + DateTime.Now.ToString() + "] Registration Error: " + strFile + ".bat");
                                strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DEVELOPER_ERROR");
                                oFunction.SendEmail("ERROR: TSM Registration Error", strEMailIdsBCC, "", "", "ERROR: TSM Registration Error", strPreface + "<p><b>An error occurred when attempting to auto-register server " + strName + " in TSM...</b></p><p>Script:" + strFile + ".vbs" + "</p>", true, false);
                            }
                        }
                        else
                        {
                            if (intLog > 0)
                                oEventLog.WriteEntry(String.Format(strName + ": " + "TSM Timeout...."), EventLogEntryType.Information);
                            oServer.UpdateTSM(intServer, "[" + DateTime.Now.ToString() + "] Registration Timeout: " + strFile + ".bat");
                            strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DEVELOPER_ERROR");
                            oFunction.SendEmail("ERROR: TSM Registration Timeout", strEMailIdsBCC, "", "", "ERROR: TSM Registration Timeout", strPreface + "<p><b>A timeout occurred when attempting to auto-register server " + strName + " in TSM...</b></p><p>Script:" + strFile + ".vbs" + "</p>", true, false);
                        }

                        if (oOperatingSystem.IsWindows(intOS) == true || oOperatingSystem.IsWindows2008(intOS) == true)
                        {
                            int intServerDomain = 0;
                            string strAutoError = "";
                            bool boolNetUseError = false;
                            if (Int32.TryParse(oServer.Get(intServer, "domainid"), out intServerDomain) == true)
                            {
                                if (strDSMOPT != "")
                                {
                                    if (File.Exists(strDSMOPT) == true)
                                    {
                                        string strResolve = strName;
                                        if (strIP != "")
                                            strResolve = strIP;
                                        oEventLog.WriteEntry(String.Format(strName + ": Attempting to configure the device using DSM.OPT file (" + strDSMOPT + ") and DOMAINID (" + intServerDomain.ToString() + ")"), EventLogEntryType.Information);
                                        oLog.AddEvent(intAnswer, strName, "", "Attempting to configure the device using DSM.OPT file (" + strDSMOPT + ") and DOMAINID (" + intServerDomain.ToString() + ")", LoggingType.Information);

                                        int intDomainEnvironment = Int32.Parse(oDomain.Get(intServerDomain, "environment"));
                                        Variables oVariableTSM = new Variables(intDomainEnvironment);
                                        string strUserTSM = oVariableTSM.Domain() + "\\" + oVariableTSM.ADUser();
                                        string strPassTSM = oVariableTSM.ADPassword();

                                        // Copy DSM.OPT file and run post scheduling steps
                                        string strDrive = "E";
                                        string strRemotePath = @"\Program Files\Tivoli\TSM\baclient\";
                                        if (oServer.Get(intServer, "pnc") == "1")
                                            strDrive = "D";
                                        if (oOperatingSystem.IsWindows2008(intOS) == true)
                                            strRemotePath = @"\TSM\baclient\";

                                        // Part 1: Copy from shared documents location to server
                                        string strFileCopyFiles = strDSMADMC + intServer.ToString() + "_" + strNow + "_2.bat";
                                        StreamWriter oCopyFiles = new StreamWriter(strFileCopyFiles);
                                        oCopyFiles.WriteLine("net use \\\\" + strResolve + "\\" + strDrive + "$ /user:" + strUserTSM + " " + strPassTSM + "");
                                        oCopyFiles.WriteLine("copy \"" + strDSMOPT + "\" \"" + "\\\\" + strResolve + "\\" + strDrive + "$" + strRemotePath + "dsm.opt\"");
                                        oCopyFiles.Flush();
                                        oCopyFiles.Close();

                                        // Part 2: Execute the copy
                                        string strFileCopyFilesOut = strDSMADMC + intServer.ToString() + "_" + strNow + "_2.txt";
                                        ProcessStartInfo infoFileCopyFiles = new ProcessStartInfo(strScripts + "psexec");
                                        infoFileCopyFiles.WorkingDirectory = strScripts;
                                        infoFileCopyFiles.Arguments = "-i cmd.exe /c " + strFileCopyFiles + " > " + strFileCopyFilesOut;
                                        if (intLog > 0)
                                        {
                                            oEventLog.WriteEntry(strName + ": PSEXEC Script = " + strScripts + "psexec " + "-i cmd.exe /c \"" + strFileCopyFiles + "\" >\"" + strFileCopyFilesOut + "\" 2>&1", EventLogEntryType.Information);
                                            oEventLog.WriteEntry(strName + ": PSEXEC Script started", EventLogEntryType.SuccessAudit);
                                        }
                                        Process procFileCopyFiles = Process.Start(infoFileCopyFiles);
                                        boolTimeout = false;
                                        procFileCopyFiles.WaitForExit(intTimeoutDefault);
                                        if (procFileCopyFiles.HasExited == false)
                                        {
                                            procFileCopyFiles.Kill();
                                            boolTimeout = true;
                                        }
                                        procFileCopyFiles.Close();
                                        if (boolTimeout == false)
                                        {
                                            if (intLog > 0)
                                                oEventLog.WriteEntry(strName + ": PSEXEC Script finished", EventLogEntryType.SuccessAudit);
                                            boolNetUseError = oFunction.ReadOutput(intServer, "TSM [Copy]", strFileCopyFilesOut, strName, "", 1, true);
                                            if (boolNetUseError == false)
                                            {
                                                if (intLog > 0)
                                                    oEventLog.WriteEntry(strName + ": Read output finished", EventLogEntryType.SuccessAudit);

                                                // Part 3: Files have been copied, now run the command to configure
                                                string strFileExecute = strDSMADMC + intServer.ToString() + "_" + strNow + "_3.bat";
                                                StreamWriter oFileExecute = new StreamWriter(strFileExecute);
                                                // "E:\Program Files\Tivoli\TSM\baclient\dsmcutil.exe" updatepw /node:ohcleiis4333 /name:"TSM Central Scheduler Service" /oldpassword:ohcleiis4333 /password:ohcleiis4333 /updateonserver:yes
                                                if (oOperatingSystem.IsWindows2008(intOS) == true)
                                                {
                                                    //CD "D:\TSM\baclient\"
                                                    oFileExecute.WriteLine("CD \"" + strDrive + ":" + strRemotePath + "\"");
                                                    //D:
                                                    oFileExecute.WriteLine(strDrive + ":");
                                                }
                                                oFileExecute.WriteLine("\"" + strDrive + ":" + strRemotePath + "dsmcutil.exe\" updatepw /node:" + strName + " /name:\"TSM Central Scheduler Service\" /oldpassword:" + strName.ToLower() + " /password:" + strName.ToLower() + " /updateonserver:yes");
                                                oFileExecute.WriteLine("start /wait sc stop \"TSM Central Scheduler Service\"");
                                                oFileExecute.WriteLine("start /wait sc start \"TSM Central Scheduler Service\"");
                                                if (oOperatingSystem.IsWindows2008(intOS) == true)
                                                {
                                                    // Run the script Steve Stewart provided for the "Client Acceptor Daemon" for the TSM Service (if it exists)
                                                    string strClientAcceptorDaemon = "\"" + strDrive + ":" + strRemotePath + "installCad_V2.vbs\"";  // "D:\TSM\baclient\installCad_V2.vbs"
                                                    oFileExecute.WriteLine("if exist " + strClientAcceptorDaemon + " (");
                                                    oFileExecute.WriteLine("%windir%\\system32\\wscript.exe " + strClientAcceptorDaemon);
                                                    oFileExecute.WriteLine(")");
                                                    // Finally, delete the Scheduled Task
                                                    oFileExecute.WriteLine("schtasks /delete /tn RegisterTSM /F");
                                                }
                                                //oFileExecute.WriteLine("del \"" + strDrive + ":" + strRemotePath + "dsm.bat\"");
                                                oFileExecute.WriteLine("ren \"" + strDrive + ":" + strRemotePath + "dsm.bat\" \"dsm.bak\"");
                                                oFileExecute.Flush();
                                                oFileExecute.Close();

                                                // Part 4: Copy the script to server
                                                string strFileCopyFiles2 = strDSMADMC + intServer.ToString() + "_" + strNow + "_4.bat";
                                                StreamWriter oCopyFiles2 = new StreamWriter(strFileCopyFiles2);
                                                oCopyFiles2.WriteLine("copy \"" + strFileExecute + "\" \"" + "\\\\" + strResolve + "\\" + strDrive + "$" + strRemotePath + "dsm.bat\"");
                                                oCopyFiles2.Flush();
                                                oCopyFiles2.Close();

                                                // Part 5: Execute the copy
                                                string strFileCopyFiles2Out = strDSMADMC + intServer.ToString() + "_" + strNow + "_4.txt";
                                                ProcessStartInfo infoFileCopyFiles2 = new ProcessStartInfo(strScripts + "psexec");
                                                infoFileCopyFiles2.WorkingDirectory = strScripts;
                                                infoFileCopyFiles2.Arguments = "-i cmd.exe /c " + strFileCopyFiles2 + " > " + strFileCopyFiles2Out;
                                                Process procFileCopyFiles2 = Process.Start(infoFileCopyFiles2);
                                                boolTimeout = false;
                                                procFileCopyFiles2.WaitForExit(intTimeoutDefault);
                                                if (procFileCopyFiles2.HasExited == false)
                                                {
                                                    procFileCopyFiles2.Kill();
                                                    boolTimeout = true;
                                                }
                                                procFileCopyFiles2.Close();
                                                if (boolTimeout == false)
                                                {
                                                    boolNetUseError = oFunction.ReadOutput(intServer, "TSM [Copy 2]", strFileCopyFiles2Out, strName, "", 0, true);

                                                    if (boolNetUseError == false)
                                                    {
                                                        oLog.AddEvent(intAnswer, strName, "", "The files have been copied to the target machine", LoggingType.Information);
                                                        if (oOperatingSystem.IsWindows2008(intOS) == false)
                                                        {
                                                            oLog.AddEvent(intAnswer, strName, "", "Running TSM configuration script on target machine", LoggingType.Information);
                                                            // Part 6: Execute the script
                                                            ProcessStartInfo infoExecute = new ProcessStartInfo(strScripts + "psexec");
                                                            infoExecute.WorkingDirectory = strScripts;
                                                            infoExecute.Arguments = "\\\\" + strResolve + " -u " + strUserTSM + " -p " + strPassTSM + " -i cmd.exe /c \"" + strDrive + ":" + strRemotePath + "dsm.bat\"";
                                                            infoExecute.UseShellExecute = false;
                                                            infoExecute.RedirectStandardOutput = true;
                                                            Process procExecute = Process.Start(infoExecute);
                                                            boolTimeout = false;
                                                            procExecute.WaitForExit(intTimeoutDefault);
                                                            if (procExecute.HasExited == false)
                                                            {
                                                                procExecute.Kill();
                                                                boolTimeout = true;
                                                            }

                                                            if (boolTimeout == false)
                                                            {
                                                                oFunction.ReadOutput(intServer, "TSM Execution", procExecute.StandardOutput);
                                                                procExecute.Close();
                                                            }
                                                            else
                                                                procExecute.Close();
                                                        }
                                                        if (boolTimeout == false)
                                                        {
                                                            // Part 7: Delete Share and TEMP files
                                                            string strFileDeleteFiles = strDSMADMC + intServer.ToString() + "_" + strNow + "_5.bat";
                                                            StreamWriter oDeleteFiles = new StreamWriter(strFileDeleteFiles);
                                                            oDeleteFiles.WriteLine("net use \\\\" + strResolve + "\\" + strDrive + "$ /dele");
                                                            oDeleteFiles.WriteLine("del " + strDSMADMC + intServer.ToString() + "_" + strNow + "_*.*");
                                                            oDeleteFiles.Flush();
                                                            oDeleteFiles.Close();

                                                            // Part 8: Execute the Deletion
                                                            ProcessStartInfo infoFileDeleteFiles = new ProcessStartInfo(strScripts + "psexec");
                                                            infoFileDeleteFiles.WorkingDirectory = strScripts;
                                                            infoFileDeleteFiles.Arguments = "-i cmd.exe /c " + strFileDeleteFiles;
                                                            Process procFileDeleteFiles = Process.Start(infoFileDeleteFiles);
                                                            procFileDeleteFiles.WaitForExit(intTimeoutDefault);
                                                            if (procFileDeleteFiles.HasExited == false)
                                                                procFileDeleteFiles.Kill();
                                                            procFileDeleteFiles.Close();

                                                            oEventLog.WriteEntry(String.Format(strName + ": The TSM server was configured"), EventLogEntryType.Information);
                                                            oLog.AddEvent(intAnswer, strName, "", "The TSM registration process has completed successfully", LoggingType.Information);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else
                                        strAutoError = strName + ": The file " + strDSMOPT + " was not found (" + intTSMServer.ToString() + ")";
                                }
                                else
                                    strAutoError = strName + ": The path to the OPT file is blank (" + intTSMServer.ToString() + ")";
                            }
                            else
                                strAutoError = strName + ": There was a problem with the domain value (" + oServer.Get(intServer, "domainid") + ")";

                            if (boolNetUseError == true)
                                strAutoError = "There was a problem with the NET USE statement";

                            if (strAutoError != "")
                            {
                                oEventLog.WriteEntry(String.Format(strAutoError), EventLogEntryType.Error);
                                strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DEVELOPER_ERROR");
                                oFunction.SendEmail("ERROR: TSM Configuration Error", strEMailIdsBCC, "", "", "ERROR: TSM Configuration Error", strPreface + "<p><b>An error occurred when attempting to configure server " + strName + " in TSM...</b></p><p>Error:" + strAutoError + "</p>", true, false);
                            }

                            if (intLog == -1)
                            {
                                oEventLog.WriteEntry(String.Format("BREAKING TSM LOOP!!!"), EventLogEntryType.Information);
                                break;
                            }
                        }
                        else
                        {
                            // Add to ZEUS database (TSM Table)
                            Zeus oZeus = new Zeus(0, dsnZeus);
                            oZeus.AddTSM(oAsset.Get(intAssetLatest, "serial"), strContainer, strServer, strPort, strDomain, strSchedule, strCloptset);
                            oLog.AddEvent(intAnswer, strName, "", "Record added to ZEUS Table for Midrange processing", LoggingType.Information);
                        }
                    }
                }
            }
            catch (Exception oError)
            {
                oEventLog.WriteEntry(String.Format(strErrorName + ": An error was encountered"), EventLogEntryType.Error);
                string strError = "TSM Registration Error: " + "(Error Message: " + oError.Message + ") (Source: " + oError.Source + ") (Stack Trace: " + oError.StackTrace + ")";
                strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DEVELOPER_ERROR");
                oFunction.SendEmail("ERROR: Generate TSM Registration Error", strEMailIdsBCC, "", "", "ERROR: Generate TSM Registration Error", strPreface + "<p><b>An error occurred when attempting to auto-register TSM...</b></p><p>Error Message:" + strError + "</p>", true, false);
                oEventLog.WriteEntry(String.Format(strErrorName + ": " + strError), EventLogEntryType.Error);
                oLog.AddEvent(strErrorName, "", strError, LoggingType.Error);
            }

        }
        public void ExecuteLTMConfigs()
        {
            try
            {
                if (intEnvironment < 3)
                    intEnvironment = 3;
                LTM oLTM = new LTM(0, dsn);
                Variables oVariable = new Variables(intEnvironment);
                System.Net.NetworkCredential oCredentials = new System.Net.NetworkCredential(oVariable.ADUser(), oVariable.ADPassword(), oVariable.Domain());
                ClearViewWebServices oWS = new ClearViewWebServices();
                oWS.Timeout = Int32.Parse("300000");
                oWS.Credentials = oCredentials;
                oWS.Url = oVariable.WebServiceURL();
                Settings oSetting = new Settings(0, dsn);
                bool boolDNS_QIP = oSetting.IsDNS_QIP();
                bool boolDNS_Bluecat = oSetting.IsDNS_Bluecat();
                
                // First do the DESIGN
                DataSet ds = oLTM.GetConfigInstalled();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    int intAnswer = Int32.Parse(dr["answerid"].ToString());
                    string strName = dr["name"].ToString();
                    string strIP = dr["ip1"].ToString() + "." + dr["ip2"].ToString() + "." + dr["ip3"].ToString() + "." + dr["ip4"].ToString();
                    // Create PNC DNS
                    if (boolDNS_QIP == true)
                    {
                        string strResult = oWS.CreateDNSforPNC(strIP, strName, "Others", "", oVariable.DNS_Domain(), oVariable.DNS_NameService(), oVariable.DNS_DynamicDNSUpdate(), -999, 0, true);
                        if (strResult == "SUCCESS")
                            strResult = "SUCCESS";
                        else if (strResult.StartsWith("***DUPLICATE") == true)
                            strResult = "DUPLICATE";
                        if (boolDNS_Bluecat == false)
                            oLTM.UpdateConfigCompleted(intAnswer, DateTime.Now, strResult);
                    }
                    if (boolDNS_Bluecat == true)
                    {
                        string strResult = oWS.CreateBluecatDNS(strIP, strName, strName, "");
                        if (strResult == "SUCCESS")
                            strResult = "SUCCESS";
                        else if (strResult.StartsWith("***DUPLICATE") == true)
                            strResult = "DUPLICATE";
                        oLTM.UpdateConfigCompleted(intAnswer, DateTime.Now, strResult);
                    }
                }

                // Second do the SERVER(S)
                bool boolContinue = true;
                Servers oServer = new Servers(0, dsn);
                Domains oDomain = new Domains(0, dsn);
                OperatingSystems oOperatingSystem = new OperatingSystems(0, dsn);
                ModelsProperties oModelsProperties = new ModelsProperties(0, dsn);
                Functions oFunction = new Functions(0, dsn, intEnvironment);
                IPAddresses oIPAddresses = new IPAddresses(0, dsnIP, dsn);
                Forecast oForecast = new Forecast(0, dsn);

                ds = oLTM.GetConfigsInstalled();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    int intServer = Int32.Parse(dr["serverid"].ToString());
                    string strName = oServer.GetName(intServer, true);
                    string strIP = dr["ip1"].ToString() + "." + dr["ip2"].ToString() + "." + dr["ip3"].ToString() + "." + dr["ip4"].ToString();
                    string strVLAN = dr["vlan"].ToString();
                    string strError = "";
                    string strResult = "";

                    // Update PNC DNS
                    if (boolDNS_QIP == true)
                    {
                        strResult = oWS.UpdateDNSforPNC(strIP, strName, "Server", "", oVariable.DNS_Domain(), oVariable.DNS_NameService(), oVariable.DNS_DynamicDNSUpdate(), -999, 0, true);
                        if (strResult == "SUCCESS")
                        {
                            if (boolDNS_Bluecat == false)
                                oLTM.AddConfigsResult(intServer, "QIP DNS was successfully updated", 0);
                        }
                        else if (strResult.StartsWith("***DUPLICATE") == true)
                        {
                            if (boolDNS_Bluecat == false)
                                oLTM.AddConfigsResult(intServer, "QIP DNS was successfully updated (duplicate)", 0);
                        }
                        else
                            strError = "There was an error updating QIP DNS... " + strResult;
                    }
                    if (boolDNS_Bluecat == true)
                    {
                        strResult = oWS.UpdateBluecatDNS(strIP, strName, strName, "");
                        if (strResult == "SUCCESS")
                            oLTM.AddConfigsResult(intServer, "BlueCat DNS was successfully updated", 0);
                        else if (strResult.StartsWith("***DUPLICATE") == true)
                            oLTM.AddConfigsResult(intServer, "BlueCat DNS was successfully updated (duplicate)", 0);
                        else
                            strError = "There was an error updating BlueCat DNS... " + strResult;
                    }

                    if (strError == "")
                    {
                        //Change the IP Address
                        DateTime _now = DateTime.Now;
                        string strNow = _now.Day.ToString() + _now.Month.ToString() + _now.Year.ToString() + _now.Hour.ToString() + _now.Minute.ToString() + _now.Second.ToString() + _now.Millisecond.ToString();
                        int intDomain = 0;
                        Int32.TryParse(oServer.Get(intServer, "domainid"), out intDomain);
                        int intAnswer = 0;
                        Int32.TryParse(oServer.Get(intServer, "answerid"), out intAnswer);
                        int intClass = 0;
                        int intAddress = 0;
                        if (intAnswer > 0) 
                        {
                            Int32.TryParse(oForecast.GetAnswer(intAnswer, "classid"), out intClass);
                            Int32.TryParse(oForecast.GetAnswer(intAnswer, "addressid"), out intAddress);
                        }
                        int intOS = 0;
                        if (Int32.TryParse(oServer.Get(intServer, "osid"), out intOS) == true)
                        {
                            if (oOperatingSystem.IsWindows(intOS) == true || oOperatingSystem.IsWindows2008(intOS) == true)
                            {
                                int intIPAddress = 0;
                                DataSet dsIP = oServer.GetIP(intServer, 0, 0, 0, 0);
                                foreach (DataRow drIP in dsIP.Tables[0].Rows)
                                {
                                    if (drIP["final"].ToString() == "1")
                                    {
                                        intIPAddress = Int32.Parse(drIP["ipAddressID"].ToString());
                                        break;
                                    }
                                }
                                int intAsset = 0;
                                DataSet dsAsset = oServer.GetAssets(intServer);
                                foreach (DataRow drAsset in dsAsset.Tables[0].Rows)
                                {
                                    if (drAsset["latest"].ToString() == "1")
                                    {
                                        intAsset = Int32.Parse(drAsset["assetid"].ToString());
                                        break;
                                    }
                                }
                                if (intIPAddress > 0)
                                {
                                    int intNetwork = Int32.Parse(oIPAddresses.Get(intIPAddress, "networkid"));
                                    int intModel = 0;
                                    if (Int32.TryParse(oServer.Get(intServer, "modelid"), out intModel) == true)
                                    {
                                        bool boolPNC = (oServer.Get(intServer, "pnc") == "1");
                                        string strAdminUser = oVariable.Domain() + "\\" + oVariable.ADUser();
                                        string strAdminPass = oVariable.ADPassword();
                                        if (boolPNC == true)
                                        {
                                            Variables oVarPNC = new Variables(999);
                                            strAdminUser = oVarPNC.Domain() + "\\" + oVarPNC.ADUser();
                                            strAdminPass = oVarPNC.ADPassword();
                                        }

                                        bool boolVMWare = false;
                                        bool boolBlade = false;
                                        if (oModelsProperties.IsTypeVMware(intModel) == true)
                                            boolVMWare = true;
                                        else if (oModelsProperties.IsTypeBlade(intModel) == true)
                                            boolBlade = true;

                                        // GLOBAL ...
                                        string strIPCurrent = oIPAddresses.GetName(intIPAddress, 0);
                                        oLTM.AddConfigsResult(intServer, "Attempting to change the IP Address (" + strIPCurrent + ") for LTM configuration on " + strName + "(VMware) to " + strIP, 0);
                                        bool boolPingChanged = false;
                                        oLTM.AddConfigsResult(intServer, "Attempting to ping the new IP address (" + strIP + ") to see if it has already been changed", 0);
                                        for (int ii = 0; ii < 3 && boolPingChanged == false; ii++)
                                        {
                                            Thread.Sleep(3000);
                                            Ping oPingChange = new Ping();
                                            string strStatusChange = "";
                                            try
                                            {
                                                PingReply oReplyChange = oPingChange.Send(strIP);
                                                strStatusChange = oReplyChange.Status.ToString().ToUpper();
                                            }
                                            catch { }
                                            boolPingChanged = (strStatusChange == "SUCCESS");
                                        }
                                        if (boolPingChanged == false)
                                        {
                                            oLTM.AddConfigsResult(intServer, "The new IP address (" + strIP + ") did not respond...has not been changed", 0);
                                            oLTM.AddConfigsResult(intServer, "Attempting to ping the current IP address (" + strIPCurrent + ") to see if it is online", 0);
                                            bool boolPingOn = false;
                                            for (int ii = 0; ii < 3 && boolPingOn == false; ii++)
                                            {
                                                Thread.Sleep(3000);
                                                Ping oPing = new Ping();
                                                string strStatus = "";
                                                try
                                                {
                                                    PingReply oReply = oPing.Send(strIPCurrent);
                                                    strStatus = oReply.Status.ToString().ToUpper();
                                                }
                                                catch { }
                                                boolPingOn = (strStatus == "SUCCESS");
                                            }
                                            if (boolPingOn == true)
                                            {
                                                DataSet dsDNS = oDomain.GetClassDNS(intDomain, intClass, intAddress);
                                                if (dsDNS.Tables[0].Rows.Count > 0)
                                                {
                                                    oLTM.AddConfigsResult(intServer, "The current IP address (" + strIP + ") responded and is online", 0);
                                                    oLTM.AddConfigsResult(intServer, "Generating scripts to change IP address...", 0);
                                                    string strFile = strScripts + strSub + intServer.ToString() + "_" + strNow + ".vbs";
                                                    // 1st part - create VBS file to copy to server
                                                    StreamWriter oWriterIP1 = new StreamWriter(strFile);
                                                    oWriterIP1.WriteLine("Set objWMIService = GetObject(\"winmgmts:\\\\" + strName + "\\root\\cimv2\")");
                                                    oWriterIP1.WriteLine("Set colAdapters = objWMIService.ExecQuery(\"Select * from Win32_NetworkAdapterConfiguration Where IPEnabled = True\")");
                                                    oWriterIP1.WriteLine("Set oClass = objWMIService.Get(\"Win32_NetworkAdapterConfiguration\")");
                                                    oWriterIP1.WriteLine("For Each objAdapter in colAdapters");
                                                    oWriterIP1.WriteLine("If (objAdapter.IPAddress(0) = \"" + strIP + "\") Then");
                                                    string strSuffix = "";
                                                    DataSet dsSuffix = oDomain.GetSuffixs(intDomain, 1);
                                                    foreach (DataRow drSuffix in dsSuffix.Tables[0].Rows)
                                                    {
                                                        if (strSuffix != "")
                                                            strSuffix += ",";
                                                        strSuffix += "\"" + drSuffix["name"].ToString() + "\"";
                                                    }
                                                    strSuffix = "Array(" + strSuffix + ")";
                                                    oWriterIP1.WriteLine("oClass.SetDNSSuffixSearchOrder(" + strSuffix + ")");
                                                    oWriterIP1.WriteLine("objAdapter.SetTCPIPNetBIOS(1)");
                                                    oWriterIP1.WriteLine("End If");
                                                    oWriterIP1.WriteLine("Next");
                                                    oWriterIP1.Flush();
                                                    oWriterIP1.Close();
                                                    string strRebootVBS = strScripts + strSub + intServer.ToString() + "_" + strNow + "_reboot.vbs";
                                                    // Only reboot if a blade
                                                    if (boolBlade == true)
                                                    {
                                                        StreamWriter oRebootIP1 = new StreamWriter(strRebootVBS);
                                                        oRebootIP1.WriteLine("Set OpSysSet = GetObject(\"winmgmts:{impersonationLevel=impersonate,(Shutdown)}\").ExecQuery(\"select * from Win32_OperatingSystem where Primary=true\")");
                                                        oRebootIP1.WriteLine("For Each OpSys In OpSysSet");
                                                        oRebootIP1.WriteLine("Debug \"OpSys.Win32Shutdown return: \" & OpSys.Win32Shutdown(5)");
                                                        oRebootIP1.WriteLine("Next");
                                                        oRebootIP1.Flush();
                                                        oRebootIP1.Close();
                                                    }
                                                    // 2nd part - create batch file
                                                    string strBatchIP1 = strScripts + strSub + intServer.ToString() + "_" + strNow + "_ip.bat";
                                                    StreamWriter oWriterIP2 = new StreamWriter(strBatchIP1);
                                                    string strDeviceName = "Virtual";
                                                    if (boolPNC == true)
                                                        strDeviceName = "PNCNet";
                                                    if (boolVMWare == true)
                                                        strDeviceName = "Primary";
                                                    oWriterIP2.WriteLine("netsh interface ip set address name=\"" + strDeviceName + "\" source=static addr=" + strIP + " mask=" + oIPAddresses.GetNetwork(intNetwork, "mask"));
                                                    // Wait 5 seconds
                                                    oWriterIP2.WriteLine("ping 1.1.1.1 -n 5 -w 1000");
                                                    oWriterIP2.WriteLine("netsh interface ip set address name=\"" + strDeviceName + "\" gateway=" + oIPAddresses.GetNetwork(intNetwork, "gateway") + " gwmetric=0");
                                                    if (dsDNS.Tables[0].Rows[0]["dns_ip1"].ToString() != "")
                                                    {
                                                        if (boolPNC == true)
                                                            oWriterIP2.WriteLine("netsh interface ip set dns name=\"" + strDeviceName + "\" source=static addr=" + dsDNS.Tables[0].Rows[0]["dns_ip1"].ToString() + " register=NONE");
                                                        else
                                                            oWriterIP2.WriteLine("netsh interface ip set dns name=\"" + strDeviceName + "\" source=static addr=" + dsDNS.Tables[0].Rows[0]["dns_ip1"].ToString() + " register=PRIMARY");
                                                    }
                                                    if (dsDNS.Tables[0].Rows[0]["dns_ip2"].ToString() != "")
                                                        oWriterIP2.WriteLine("netsh interface ip add dns name=\"Primary\" addr=" + dsDNS.Tables[0].Rows[0]["dns_ip2"].ToString() + " index=2");
                                                    if (dsDNS.Tables[0].Rows[0]["dns_ip3"].ToString() != "")
                                                        oWriterIP2.WriteLine("netsh interface ip add dns name=\"Primary\" addr=" + dsDNS.Tables[0].Rows[0]["dns_ip3"].ToString() + " index=3");
                                                    if (dsDNS.Tables[0].Rows[0]["dns_ip4"].ToString() != "")
                                                        oWriterIP2.WriteLine("netsh interface ip add dns name=\"Primary\" addr=" + dsDNS.Tables[0].Rows[0]["dns_ip4"].ToString() + " index=4");
                                                    if (dsDNS.Tables[0].Rows[0]["wins_ip1"].ToString() != "")
                                                        oWriterIP2.WriteLine("netsh interface ip set wins name=\"Primary\" source=static addr=" + dsDNS.Tables[0].Rows[0]["wins_ip1"].ToString());
                                                    if (dsDNS.Tables[0].Rows[0]["wins_ip2"].ToString() != "")
                                                        oWriterIP2.WriteLine("netsh interface ip add wins name=\"Primary\" addr=" + dsDNS.Tables[0].Rows[0]["wins_ip2"].ToString() + " index=2");
                                                    if (dsDNS.Tables[0].Rows[0]["wins_ip3"].ToString() != "")
                                                        oWriterIP2.WriteLine("netsh interface ip add wins name=\"Primary\" addr=" + dsDNS.Tables[0].Rows[0]["wins_ip3"].ToString() + " index=3");
                                                    if (dsDNS.Tables[0].Rows[0]["wins_ip4"].ToString() != "")
                                                        oWriterIP2.WriteLine("netsh interface ip add wins name=\"Primary\" addr=" + dsDNS.Tables[0].Rows[0]["wins_ip4"].ToString() + " index=4");
                                                    if (boolBlade == true)
                                                        oWriterIP2.WriteLine("%windir%\\system32\\wscript.exe \"C:\\OPTIONS\\CV_REBOOT.VBS\"");
                                                    oWriterIP2.Flush();
                                                    oWriterIP2.Close();
                                                    // 3rd part - create batch file
                                                    string strBatchIP2 = strScripts + strSub + intServer.ToString() + "_" + strNow + "_ip_2.bat";
                                                    StreamWriter oWriterIP3 = new StreamWriter(strBatchIP2);
                                                    oWriterIP3.WriteLine("F:");
                                                    oWriterIP3.WriteLine("cd " + strScripts + strSub);
                                                    oWriterIP3.WriteLine("net use \\\\" + strIPCurrent + "\\C$ /user:" + strAdminUser + " " + strAdminPass + "");
                                                    oWriterIP3.WriteLine("mkdir \\\\" + strIP + "\\C$\\OPTIONS");
                                                    oWriterIP3.WriteLine("copy " + strFile + " \\\\" + strIPCurrent + "\\C$\\OPTIONS\\CV_IP.VBS");
                                                    if (boolBlade == true)
                                                        oWriterIP3.WriteLine("copy " + strRebootVBS + " \\\\" + strIPCurrent + "\\C$\\OPTIONS\\CV_REBOOT.VBS");
                                                    oWriterIP3.WriteLine("copy " + strBatchIP1 + " \\\\" + strIPCurrent + "\\C$\\OPTIONS\\CV_IP.BAT");
                                                    oWriterIP3.Flush();
                                                    oWriterIP3.Close();
                                                    // 4th part - run the batch file to perform copy
                                                    string strFileIP2 = strScripts + strSub + intServer.ToString() + "_" + strNow + "_ip_2.vbs";
                                                    string strFileIP2Out = strScripts + strSub + intServer.ToString() + "_" + strNow + "_ip_2.txt";
                                                    StreamWriter oWriterIP4 = new StreamWriter(strFileIP2);
                                                    oWriterIP4.WriteLine("Dim objShell");
                                                    oWriterIP4.WriteLine("Set objShell = CreateObject(\"WScript.Shell\")");
                                                    oWriterIP4.WriteLine("objShell.Run(\"cmd.exe /c " + strBatchIP2 + " > " + strFileIP2Out + "\")");
                                                    oWriterIP4.WriteLine("Set objShell = Nothing");
                                                    oWriterIP4.Flush();
                                                    oWriterIP4.Close();
                                                    ILaunchScript oScriptIP1 = new SimpleLaunchWsh(strFileIP2, "", true, 30) as ILaunchScript;
                                                    oScriptIP1.Launch();
                                                    // 5th part - file has been copied, do the PSEXEC to install application
                                                    System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo(strScripts + "psexec");
                                                    info.WorkingDirectory = strScripts;
                                                    info.Arguments = "\\\\" + strIPCurrent + " -u " + strAdminUser + " -p " + strAdminPass + " -i cmd.exe /c C:\\OPTIONS\\CV_IP.BAT >C:\\OPTIONS\\CV_IP.TXT 2>&1";
                                                    oLTM.AddConfigsResult(intServer, "PSEXEC Script = " + "\\\\" + strIPCurrent + " -u " + strAdminUser + " -p ***** -i cmd.exe /c C:\\OPTIONS\\CV_IP.BAT >C:\\OPTIONS\\CV_IP.TXT 2>&1", 0);
                                                    System.Diagnostics.Process proc = System.Diagnostics.Process.Start(info);
                                                    proc.WaitForExit();
                                                    proc.Close();
                                                    oLTM.AddConfigsResult(intServer, "PSEXEC Script Finished", 0);
                                                    oLTM.AddConfigsResult(intServer, "IP Address has been changed on the server...", 0);
                                                }
                                                else
                                                    strError = "No DNS Servers have been configured for the domain, class and environment";
                                            }
                                            else
                                                oLTM.AddConfigsResult(intServer, "The new IP address (" + strIP + ") did not respond...server is most likely already powered off", 0);




                                            if (strError == "")
                                            {
                                                if (boolVMWare == true)
                                                {
                                                    // ********************************************************************
                                                    // ********************************************************************
                                                    // *******************   START: VMWARE   ******************************
                                                    // ********************************************************************
                                                    // ********************************************************************

                                                    VMWare oVMWare = new VMWare(0, dsn);
                                                    string strConnect = oVMWare.Connect(strName);
                                                    VimService _service = oVMWare.GetService();
                                                    if (strConnect == "")
                                                    {
                                                        DataSet dsGuest = oVMWare.GetGuest(strName);
                                                        if (dsGuest.Tables[0].Rows.Count > 0)
                                                        {
                                                            int intHost = Int32.Parse(dsGuest.Tables[0].Rows[0]["hostid"].ToString());
                                                            int intCluster = Int32.Parse(oVMWare.GetHost(intHost, "clusterid"));
                                                            // Change VLAN of Network Adapter
                                                            oLTM.AddConfigsResult(intServer, "Changing the VLAN of the guest...", 0);
                                                            if (intNetwork > 0)
                                                            {
                                                                int intVLAN = Int32.Parse(oIPAddresses.GetNetwork(intNetwork, "vlanid"));
                                                                if (intVLAN > 0)
                                                                {
                                                                    DataSet dsVlan = oVMWare.GetVlanAssociations(intVLAN, intCluster);
                                                                    if (dsVlan.Tables[0].Rows.Count > 0)
                                                                    {
                                                                        int intVMWareVLAN = Int32.Parse(dsVlan.Tables[0].Rows[0]["vmware_vlanid"].ToString());
                                                                        oVMWare.UpdateGuestVlan(strName, intVMWareVLAN);
                                                                        strVLAN = oVMWare.GetVlan(intVMWareVLAN, "name");
                                                                    }
                                                                    else
                                                                    {
                                                                        strError = "There are no VMware associations ~ VLAN " + oIPAddresses.GetVlan(intVLAN, "vlan");
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    strError = "Invalid VLAN";
                                                                }
                                                            }
                                                            else
                                                            {
                                                                strError = "Invalid Network";
                                                            }

                                                            if (strError == "")
                                                            {
                                                                ManagedObjectReference _vm_net2 = oVMWare.GetVM(strName);
                                                                if (strVLAN != "")
                                                                {
                                                                    // Shutdown guest os
                                                                    oLTM.AddConfigsResult(intServer, "Shutting down guest OS to change VLAN", 0);
                                                                    VirtualMachineRuntimeInfo run_vlan = (VirtualMachineRuntimeInfo)oVMWare.getObjectProperty(_vm_net2, "runtime");
                                                                    if (run_vlan.powerState != VirtualMachinePowerState.poweredOff)
                                                                    {
                                                                        ManagedObjectReference _task_shutdown = _service.PowerOffVM_Task(_vm_net2);
                                                                        oLTM.AddConfigsResult(intServer, "Guest OS shutdown Started", 0);
                                                                        TaskInfo _info_shutdown = (TaskInfo)oVMWare.getObjectProperty(_task_shutdown, "info");
                                                                        while (_info_shutdown.state == TaskInfoState.running)
                                                                            _info_shutdown = (TaskInfo)oVMWare.getObjectProperty(_task_shutdown, "info");
                                                                        if (_info_shutdown.state == TaskInfoState.success)
                                                                        {
                                                                            int intAttempt = 0;
                                                                            for (intAttempt = 0; intAttempt < 20 && run_vlan.powerState != VirtualMachinePowerState.poweredOff; intAttempt++)
                                                                            {
                                                                                run_vlan = (VirtualMachineRuntimeInfo)oVMWare.getObjectProperty(_vm_net2, "runtime");
                                                                                int intAttemptLeft = (20 - intAttempt);
                                                                                oLTM.AddConfigsResult(intServer, "Server still on...waiting 5 seconds (" + intAttemptLeft.ToString() + " tries left)", 0);
                                                                                Thread.Sleep(3000);
                                                                            }
                                                                        }
                                                                    }
                                                                    else
                                                                        oLTM.AddConfigsResult(intServer, "Guest OS was already shutdown (" + run_vlan.powerState.ToString() + ")", 0);

                                                                    if (run_vlan.powerState != VirtualMachinePowerState.poweredOff)
                                                                    {
                                                                        strError = "There was a problem shutting down the guest";
                                                                    }
                                                                    else
                                                                    {
                                                                        oLTM.AddConfigsResult(intServer, "Guest OS shutdown Finished", 0);
                                                                        // Wait 10 seconds (VMware still errors if executed right away)
                                                                        Thread.Sleep(10000);
                                                                        VirtualMachineConfigInfo vminfo = (VirtualMachineConfigInfo)oVMWare.getObjectProperty(_vm_net2, "config");
                                                                        VirtualDevice[] test = vminfo.hardware.device;
                                                                        for (int ii = 0; ii < test.Length; ii++)
                                                                        {
                                                                            if (test[ii].deviceInfo.label.ToUpper() == "NETWORK ADAPTER 1")
                                                                            {
                                                                                VirtualEthernetCard nic = (VirtualEthernetCard)test[ii];
                                                                                VirtualDeviceConfigSpec[] configspecarr = new VirtualDeviceConfigSpec[1];
                                                                                VirtualEthernetCardNetworkBackingInfo vecnbi = new VirtualEthernetCardNetworkBackingInfo();
                                                                                vecnbi.deviceName = strVLAN;
                                                                                VirtualDeviceConfigSpec newethdevicespec = new VirtualDeviceConfigSpec();
                                                                                newethdevicespec.device = nic;
                                                                                nic.backing = vecnbi;
                                                                                newethdevicespec.operation = VirtualDeviceConfigSpecOperation.edit;
                                                                                newethdevicespec.operationSpecified = true;
                                                                                configspecarr[0] = newethdevicespec;
                                                                                VirtualMachineConfigSpec vmconfigspec = new VirtualMachineConfigSpec();
                                                                                vmconfigspec.deviceChange = configspecarr;
                                                                                oLTM.AddConfigsResult(intServer, "Network Adapter Changing to VLAN:" + strVLAN, 0);
                                                                                ManagedObjectReference _task_net = _service.ReconfigVM_Task(_vm_net2, vmconfigspec);
                                                                                TaskInfo _info_net = (TaskInfo)oVMWare.getObjectProperty(_task_net, "info");
                                                                                while (_info_net.state == TaskInfoState.running)
                                                                                    _info_net = (TaskInfo)oVMWare.getObjectProperty(_task_net, "info");
                                                                                if (_info_net.state == TaskInfoState.success)
                                                                                    oLTM.AddConfigsResult(intServer, "Network Adapter Reconfigured", 0);
                                                                                else
                                                                                {
                                                                                    strError = "Network Adapter NOT Reconfigured";
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    strError = "No VLAN found for the guest";
                                                                }

                                                                if (strError == "")
                                                                {
                                                                    if (oOperatingSystem.IsWindows(intOS) == true || oOperatingSystem.IsWindows2008(intOS) == true)
                                                                    {
                                                                        // 6th part - kill PSEXEC if still running
                                                                        System.Diagnostics.ProcessStartInfo info2 = new System.Diagnostics.ProcessStartInfo(strScripts + "pskill");
                                                                        info2.WorkingDirectory = strScripts;
                                                                        info2.Arguments = "\\\\" + strIP + " -u " + strAdminUser + " -p " + strAdminPass + " PSEXESVC";
                                                                        oLTM.AddConfigsResult(intServer, "PSKILL Script = " + "\\\\" + strIP + " -u " + strAdminUser + " -p ***** PSEXESVC", 0);
                                                                        System.Diagnostics.Process proc2 = System.Diagnostics.Process.Start(info2);
                                                                        proc2.WaitForExit();
                                                                        proc2.Close();
                                                                        oLTM.AddConfigsResult(intServer, "PSKILL Script Finished", 0);
                                                                    }
                                                                    // Turn on the guest if it is off
                                                                    oLTM.AddConfigsResult(intServer, "Starting Virtual Machine Power On...", 0);
                                                                    GuestInfo ginfo = (GuestInfo)oVMWare.getObjectProperty(_vm_net2, "guest");
                                                                    if (ginfo.guestState != "running")
                                                                    {
                                                                        ManagedObjectReference _task_power = _service.PowerOnVM_Task(_vm_net2, null);
                                                                        TaskInfo _info_power = (TaskInfo)oVMWare.getObjectProperty(_task_power, "info");
                                                                        while (_info_power.state == TaskInfoState.running)
                                                                            _info_power = (TaskInfo)oVMWare.getObjectProperty(_task_power, "info");
                                                                        if (_info_power.state == TaskInfoState.success)
                                                                            oLTM.AddConfigsResult(intServer, "Virtual Machine Powering On", 0);
                                                                        else
                                                                            oLTM.AddConfigsResult(intServer, "Virtual Machine was NOT Powered On", 0);
                                                                    }
                                                                    else
                                                                        oLTM.AddConfigsResult(intServer, "Virtual Machine was already Powered On", 0);
                                                                    int intAttempt = 0;
                                                                    for (intAttempt = 0; intAttempt < 60 && ginfo.guestState != "running"; intAttempt++)
                                                                    {
                                                                        ginfo = (GuestInfo)oVMWare.getObjectProperty(_vm_net2, "guest");
                                                                        int intAttemptLeft = (60 - intAttempt);
                                                                        oLTM.AddConfigsResult(intServer, "Server still starting...waiting 5 seconds (" + intAttemptLeft.ToString() + " tries left)", 0);
                                                                        Thread.Sleep(10000);
                                                                    }
                                                                    if (ginfo.guestState != "running")
                                                                    {
                                                                        strError = "There was a problem turning on the guest";
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        else
                                                            strError = "This device was not build by ClearView (does not exist in VMWare Guest table)";
                                                    }
                                                    else
                                                        strError = "Error connecting to Virtual Center...<br/>" + strConnect;
                                                    // ********************************************************************
                                                    // *******************   END: VMWARE     ******************************
                                                    // ********************************************************************
                                                }
                                                else
                                                {
                                                    bool boolOKtoAssignIP = false;
                                                    DataSet dsSwitch = oAsset.GetSwitchports(intAsset, SwitchPortType.Network);
                                                    if (boolBlade == true)
                                                        boolOKtoAssignIP = true;
                                                    else
                                                    {
                                                        if (dsSwitch.Tables[0].Rows.Count > 0)
                                                            boolOKtoAssignIP = true;
                                                    }

                                                    if (boolOKtoAssignIP == true)
                                                    {
                                                        string strILO = oAsset.GetServerOrBlade(intAsset, "ilo").ToUpper();
                                                        string strVLANname = "VLAN_" + strVLAN;
                                                        if (boolBlade == true)
                                                        {
                                                            // ********************************************************************
                                                            // ********************************************************************
                                                            // *******************   START: BLADE    ******************************
                                                            // ********************************************************************
                                                            // ********************************************************************

                                                            oLTM.AddConfigsResult(intServer, "Turning off server to reconfigure virtual connect settings", 0);
                                                            // Should be shutting down at this point....
                                                            AssetPowerStatus powVirtualConnect = oAsset.PowerStatus(intAsset, 0, intEnvironment, strScripts);
                                                            if (powVirtualConnect == AssetPowerStatus.Error)
                                                            {
                                                                int intEnclosure = Int32.Parse(oAsset.GetServerOrBlade(intAsset, "enclosureid"));
                                                                string strHost = oAsset.GetEnclosure(intEnclosure, "virtual_connect");
                                                                strError = "Could not connect to Virtual Connect Manager IP " + strHost;
                                                            }
                                                            else
                                                            {
                                                                oLTM.AddConfigsResult(intServer, "Checking to see if IP address script CV_IP.BAT tunrned off server", 0);
                                                                int intAttempt = 0;
                                                                for (intAttempt = 0; intAttempt < 20 && powVirtualConnect != AssetPowerStatus.Off; intAttempt++)
                                                                {
                                                                    powVirtualConnect = oAsset.PowerStatus(intAsset, 0, intEnvironment, strScripts);
                                                                    int intAttemptLeft = (20 - intAttempt);
                                                                    oLTM.AddConfigsResult(intServer, "Server still on...waiting 5 seconds (Power = " + powVirtualConnect.ToString() + ") (" + intAttemptLeft.ToString() + " tries left)", 0);
                                                                    Thread.Sleep(3000);
                                                                }
                                                                if (intAttempt == 20)
                                                                {
                                                                    // Server is still on....manually shutdown using ILO
                                                                    oLTM.AddConfigsResult(intServer, "Server " + strName + " still on...shutting down using ILO", 0);
                                                                    oLTM.AddConfigsResult(intServer, "Power Off Script: cmd.exe /c " + strScripts + strSub + "CPQLOCFG.EXE -s " + strILO + " -l p.log -f " + strScripts + strSub + "poweroff.xml -v -u iadmin -p *****", 0);
                                                                    string strFileIPOff = strScripts + strSub + intServer.ToString() + "_" + strNow + "_ip_off.vbs";
                                                                    string strFileIPOffOut = strScripts + strSub + intServer.ToString() + "_" + strNow + "_ip_off.txt";
                                                                    StreamWriter oWriterOff = new StreamWriter(strFileIPOff);
                                                                    oWriterOff.WriteLine("Dim objShell");
                                                                    oWriterOff.WriteLine("Set objShell = CreateObject(\"WScript.Shell\")");
                                                                    oWriterOff.WriteLine("objShell.Run(\"cmd.exe /c " + strScripts + strSub + "CPQLOCFG.EXE -s " + strILO + " -l p.log -f " + strScripts + strSub + "poweroff.xml -v -u iadmin -p qwertyui > " + strFileIPOffOut + "\")");
                                                                    oWriterOff.WriteLine("Set objShell = Nothing");
                                                                    oWriterOff.Flush();
                                                                    oWriterOff.Close();
                                                                    ILaunchScript oScriptOff = new SimpleLaunchWsh(strFileIPOff, "", true, 5) as ILaunchScript;
                                                                    oScriptOff.Launch();
                                                                    oLTM.AddConfigsResult(intServer, "Checking to see if server is turned off from ILO", 0);
                                                                    powVirtualConnect = oAsset.PowerStatus(intAsset, 0, intEnvironment, strScripts);
                                                                    for (intAttempt = 0; intAttempt < 20 && powVirtualConnect != AssetPowerStatus.Off; intAttempt++)
                                                                    {
                                                                        powVirtualConnect = oAsset.PowerStatus(intAsset, 0, intEnvironment, strScripts);
                                                                        int intAttemptLeft = (20 - intAttempt);
                                                                        oLTM.AddConfigsResult(intServer, "Server still on...waiting 5 seconds (Power = " + powVirtualConnect.ToString() + ") (" + intAttemptLeft.ToString() + " tries left)", 0);
                                                                        Thread.Sleep(3000);
                                                                    }
                                                                    if (intAttempt == 20)
                                                                    {
                                                                        // Server is still on....throw error
                                                                        strError = "There was a problem shutting down the server to change virtual connect settings";
                                                                    }
                                                                }

                                                                if (strError == "")
                                                                {
                                                                    // Server is powered off...change VLAN
                                                                    oLTM.AddConfigsResult(intServer, "Server powered off...changing virtual connect setting to " + strVLANname, 0);
                                                                    oLTM.AddConfigsResult(intServer, "Attempting to change virtual connect settings (AssetID: " + intAsset.ToString() + ") to " + strVLANname, 0);
                                                                    string strResultVC1 = oAsset.ExecuteVirtualConnectIP(intAsset, 0, intEnvironment, strVLANname, 1, false, false, false);
                                                                    if (strResultVC1.Contains("ERROR") == true || strResultVC1 == "")
                                                                        strError = "Could not change virtual connect settings ~ (NIC#1) to " + strVLANname + "... " + strResultVC1 + " (Debug = " + oAsset.VirtualConnect() + ")";
                                                                    else
                                                                        oLTM.AddConfigsResult(intServer, "Server successfully changed virtual connect settings (NIC#1) to " + strVLANname, 0);
                                                                    string strResultVC2 = oAsset.ExecuteVirtualConnectIP(intAsset, 0, intEnvironment, strVLANname, 2, false, false, false);
                                                                    if (strResultVC2.Contains("ERROR") == true || strResultVC2 == "")
                                                                        strError = "Could not change virtual connect settings ~ (NIC#2) to " + strVLANname + "... " + strResultVC2 + " (Debug = " + oAsset.VirtualConnect() + ")";
                                                                    else
                                                                        oLTM.AddConfigsResult(intServer, "Server successfully changed virtual connect settings (NIC#2) to " + strVLANname, 0);
                                                                    if (strError == "")
                                                                    {
                                                                        // Change successful, power back on
                                                                        oLTM.AddConfigsResult(intServer, "Power On Script: cmd.exe /c " + strScripts + strSub + "CPQLOCFG.EXE -s " + strILO + " -l p.log -f " + strScripts + strSub + "poweron.xml -v -u iadmin -p *****", 0);
                                                                        string strFileIPOn = strScripts + strSub + intServer.ToString() + "_" + strNow + "_ip_on.vbs";
                                                                        string strFileIPOnOut = strScripts + strSub + intServer.ToString() + "_" + strNow + "_ip_on.txt";
                                                                        StreamWriter oWriterOn = new StreamWriter(strFileIPOn);
                                                                        oWriterOn.WriteLine("Dim objShell");
                                                                        oWriterOn.WriteLine("Set objShell = CreateObject(\"WScript.Shell\")");
                                                                        oWriterOn.WriteLine("objShell.Run(\"cmd.exe /c " + strScripts + strSub + "CPQLOCFG.EXE -s " + strILO + " -l p.log -f " + strScripts + strSub + "poweron.xml -v -u iadmin -p qwertyui > " + strFileIPOnOut + "\")");
                                                                        oWriterOn.WriteLine("Set objShell = Nothing");
                                                                        oWriterOn.Flush();
                                                                        oWriterOn.Close();
                                                                        ILaunchScript oScriptOn = new SimpleLaunchWsh(strFileIPOn, "", true, 5) as ILaunchScript;
                                                                        oScriptOn.Launch();

                                                                        // Check for Server to be back on
                                                                        powVirtualConnect = oAsset.PowerStatus(intAsset, 0, intEnvironment, strScripts);
                                                                        for (intAttempt = 0; intAttempt < 10 && powVirtualConnect != AssetPowerStatus.On; intAttempt++)
                                                                        {
                                                                            powVirtualConnect = oAsset.PowerStatus(intAsset, 0, intEnvironment, strScripts);
                                                                            int intAttemptLeft = (10 - intAttempt);
                                                                            oLTM.AddConfigsResult(intServer, "Server still off...waiting 5 seconds (" + intAttemptLeft.ToString() + " tries left)", 0);
                                                                            Thread.Sleep(3000);
                                                                        }
                                                                        if (intAttempt == 10)
                                                                        {
                                                                            // Server is still off....throw error
                                                                            strError = "There was a problem turning on the server after virtual connect settings change";
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            // ********************************************************************
                                                            // *******************   END: BLADE      ******************************
                                                            // ********************************************************************
                                                        }
                                                        else
                                                        {
                                                            // ********************************************************************
                                                            // ********************************************************************
                                                            // *******************   START: PHYSICAL ******************************
                                                            // ********************************************************************
                                                            // ********************************************************************

                                                            // Loop through switchports and change them to new VLAN
                                                            StringBuilder strSwitchOutput = new StringBuilder();
                                                            foreach (DataRow drSwitch in dsSwitch.Tables[0].Rows)
                                                            {
                                                                try
                                                                {
                                                                    oLTM.AddConfigsResult(intServer, "Attempting to change interface [" + drSwitch["interface"].ToString() + "] on switch [" + drSwitch["name"].ToString() + "] to VLAN [" + strVLAN + "]...", 0);
                                                                    int intSwitch = Int32.Parse(drSwitch["id"].ToString());
                                                                    string strSwitchName = drSwitch["name"].ToString();
                                                                    string strSwitchInterface = drSwitch["interface"].ToString();
                                                                    int intSwitchNIC = Int32.Parse(drSwitch["nic"].ToString());
                                                                    int intSwitchNewVLAN = Int32.Parse(strVLAN);
                                                                    int intSwitchEnvironment = 1;
                                                                    Functions oFunctionSwitch = new Functions(0, dsn, intSwitchEnvironment);
                                                                    string strSwitchResult = oFunctionSwitch.ChangeVLAN(strSwitchName, oAsset.Get(intAsset, "serial"), "", strSwitchInterface, intSwitchNewVLAN.ToString(), (drSwitch["is_ios"].ToString() == "1"), true, true, true, false);
                                                                    strSwitchOutput.Append(strSwitchResult);
                                                                    if (strSwitchResult.StartsWith("ERROR") == true)
                                                                        strError = "There was a problem changing the VLAN of a switch port..." + strSwitchResult + " ~ Failure!! Interface [" + drSwitch["interface"].ToString() + "] on switch [" + drSwitch["name"].ToString() + "] was NOT changed to VLAN [" + strVLAN + "]";
                                                                    else
                                                                    {
                                                                        oAsset.UpdateSwitchport(intSwitch, intAsset, SwitchPortType.Network, intSwitchNIC, intSwitchNewVLAN);
                                                                        oLTM.AddConfigsResult(intServer, "Success!! Interface [" + drSwitch["interface"].ToString() + "] on switch [" + drSwitch["name"].ToString() + "] was changed to VLAN [" + strVLAN + "].", 0);
                                                                    }
                                                                }
                                                                catch (Exception exSwitch)
                                                                {
                                                                    string strSwitchError = "Invalid format (is not a number) for either switch assetid (" + drSwitch["id"].ToString() + "), switch interface (" + drSwitch["interface"].ToString() + ") OR switch vlan (" + strVLAN + ")";
                                                                    strSwitchOutput.Append("ERROR: " + strSwitchError);
                                                                    strError = "There was a problem changing the VLAN of a switch port..." + strSwitchError + " ~ Failure!! Interface [" + drSwitch["interface"].ToString() + "] on switch [" + drSwitch["name"].ToString() + "] was NOT changed to VLAN [" + strVLAN + "]";
                                                                    break;
                                                                }
                                                            }
                                                            oLTM.AddConfigsResult(intServer, "Output of Switch Configuration...<br/>" + strSwitchOutput.ToString(), 0);
                                                            // ********************************************************************
                                                            // *******************   END: PHYSICAL ******************************
                                                            // ********************************************************************
                                                        }
                                                    }
                                                    else
                                                        strError = "Switchport configuration has not been setup for this asset";
                                                }
                                            }

                                            if (strError == "")
                                            {
                                                // 7th part - create BAT file to delete the copy (install_3.bat)
                                                string strBatchIP3 = strScripts + strSub + intServer.ToString() + "_" + strNow + "_ip_3.bat";
                                                StreamWriter oWriterIP5 = new StreamWriter(strBatchIP3);
                                                oWriterIP5.WriteLine(strScripts + "psexec.exe \\\\" + strIP + " -u " + strAdminUser + " -p " + strAdminPass + " -i cmd.exe /c C:\\OPTIONS\\CV_IP.VBS");
                                                //oWriterIP5.WriteLine("del \\\\" + strIP + "\\C$\\OPTIONS\\CV_IP.BAT");
                                                //oWriterIP5.WriteLine("del \\\\" + strIP + "\\C$\\OPTIONS\\CV_IP.VBS");
                                                oWriterIP5.WriteLine("net use \\\\" + strIP + "\\C$ /dele");
                                                oWriterIP5.Flush();
                                                oWriterIP5.Close();
                                                // 8th part - run the batch file to perform copy
                                                string strFileIP3 = strScripts + strSub + intServer.ToString() + "_" + strNow + "_ip_3.vbs";
                                                StreamWriter oWriterIP6 = new StreamWriter(strFileIP3);
                                                oWriterIP6.WriteLine("Dim objShell");
                                                oWriterIP6.WriteLine("Set objShell = CreateObject(\"WScript.Shell\")");
                                                oWriterIP6.WriteLine("objShell.Run(\"cmd.exe /c " + strBatchIP3 + "\")");
                                                oWriterIP6.WriteLine("Set objShell = Nothing");
                                                oWriterIP6.Flush();
                                                oWriterIP6.Close();
                                                ILaunchScript oScriptIP2 = new SimpleLaunchWsh(strFileIP3, "", true, 30) as ILaunchScript;
                                                oScriptIP2.Launch();

                                                // Wait 5 seconds and then ping new address
                                                bool boolPinged = false;
                                                for (int ii = 0; ii < 10 && boolPinged == false; ii++)
                                                {
                                                    Thread.Sleep(3000);
                                                    Ping oPing = new Ping();
                                                    string strStatus = "";
                                                    try
                                                    {
                                                        PingReply oReply = oPing.Send(strIP);
                                                        strStatus = oReply.Status.ToString().ToUpper();
                                                    }
                                                    catch { }
                                                    boolPinged = (strStatus == "SUCCESS");
                                                }
                                                if (boolPinged == true)
                                                {
                                                    strResult = "IP Successfully Changed to " + strIP;
                                                    oLTM.AddConfigsResult(intServer, strResult, 0);
                                                }
                                                else
                                                {
                                                    strError = "There was a problem assigning the IP Address ~ (ping " + strIP + " did not respond)";
                                                }
                                            }
                                        }
                                        else
                                        {
                                            strResult = "IP Already Changed to " + strIP;
                                            oLTM.AddConfigsResult(intServer, strResult, 0);
                                        }
                                    }
                                    else
                                        strError = "The model is invalid ~ ModelID = " + oServer.Get(intServer, "modelid");
                                }
                                else
                                    strError = "There is no assigned IP address for this device";
                            }
                            else
                                strError = "This operating system is NOT distributed";
                        }
                        else
                            strError = "The operating system is invalid ~ OperatingSystemID = " + oServer.Get(intServer, "osid");
                    }

                    if (strError == "")
                        oLTM.UpdateConfigsCompleted(intServer, DateTime.Now);
                    else
                        oLTM.AddConfigsResult(intServer, strError, 1);
                }
            }
            catch (Exception oError)
            {

                strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DEVELOPER_ERROR");
                oFunction.SendEmail("ERROR: Execute LTM Config", strEMailIdsBCC, "", "", "ERROR: Execute LTM Config", strPreface + "<p><b>An error occurred when attempting to apply an LTM Config...</b></p><p>Error Message:" + oError.Message + "</p>", true, false);
            }

        }
        //private void CheckRequestClose()
        //{
        //    DateTime oCheck = oStart;
        //    string[] strTimes;
        //    char[] strSplit = { ';' };
        //    strTimes = strRequestEnd.Split(strSplit);
        //    for (int ii = 0; ii < strTimes.Length; ii++)
        //    {
        //        if (strTimes[ii].Trim() != "")
        //        {
        //            DateTime _tick = DateTime.Parse(strTimes[ii].Trim());
        //            if (_tick.ToShortTimeString() == oCheck.ToShortTimeString())
        //            {
        //                boolDone = true;
        //            }
        //        }
        //    }
        //}
        //private void CheckWeeklyStatus()
        //{
        //    DateTime oCheck = oStart;
        //    string[] strTimes;
        //    char[] strSplit = { ';' };
        //    strTimes = strWeeklyStatus.Split(strSplit);
        //    strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DEVELOPER_ALERT");

        //    for (int ii = 0; ii < strTimes.Length; ii++)
        //    {
        //        if (strTimes[ii].Trim() != "")
        //        {
        //            DateTime _tick = DateTime.Parse(strTimes[ii].Trim());
        //            if (_tick.ToShortTimeString() == oCheck.ToShortTimeString())
        //            {
        //                boolDone = true;
        //                ResourceRequest oResourceRequest = new ResourceRequest(0, dsn);
        //                DataSet ds = oResourceRequest.GetStatuss();
        //                int intUser = 0;
        //                int intOldUser = 0;
        //                string strBody = "";
        //                if (ds.Tables[0].Rows.Count > 0)
        //                {
        //                    try
        //                    {
        //                        foreach (DataRow dr in ds.Tables[0].Rows)
        //                        {
        //                            intUser = Int32.Parse(dr["userid"].ToString());
        //                            TimeSpan oSpan = new TimeSpan();
        //                            string strDate = "NEVER";
        //                            bool boolNever = false;
        //                            if (dr["modified"].ToString() == "")
        //                                boolNever = true;
        //                            else
        //                            {
        //                                DateTime _modified = DateTime.Parse(dr["modified"].ToString());
        //                                DateTime _last = DateTime.Parse(dr["last"].ToString());
        //                                int intUpdated = Int32.Parse(dr["updated"].ToString());
        //                                if (intUpdated == 1 && _last > _modified)
        //                                    _modified = _last;
        //                                oSpan = oCheck.Subtract(_modified);
        //                                strDate = _modified.ToLongDateString();
        //                            }
        //                            if (intUser != intOldUser && intOldUser != 0)
        //                            {
        //                                // Send Email
        //                                if (strBody != "")
        //                                {
        //                                    strBody = "<table border=\"0\" cellpadding=\"7\" cellspacing=\"0\" style=\"font-family:'openSans', Verdana, Arial, Helvetica, sans-serif; font-size:11px; border:solid 1px #CCCCCC\"><tr bgcolor=\"#EEEEEE\"><td><b>Project ID</b></td><td><b>Project Name</b></td><td><b>Project Number</b></td><td><b>Last Updated</b></td><td></td></tr>" + strBody + "</table>";
        //                                    if (intTest == 0)
        //                                        oFunction.SendEmail("REMINDER: Weekly Status", oUser.GetName(intOldUser), "", strEMailIdsBCC, "REMINDER: Weekly Status", strPreface + "<p><b>This message is to remind you to update the progress of the following requests...</b></p><p>" + strBody + "</p>", true, false);
        //                                    else
        //                                        oFunction.SendEmail("REMINDER: Weekly Status", "", "", strEMailIdsBCC, "REMINDER: Weekly Status", strPreface + "<p>TO: " + oUser.GetFullName(intOldUser) + "<br/>CC: " + "" + "</p><p><b>This message is to remind you to update the progress of the following requests...</b></p><p>" + strBody + "</p>", true, false);
        //                                    strBody = "";
        //                                }
        //                            }
        //                            bool boolValidApp = false;
        //                            string[] strApps;
        //                            strApps = strWeeklyStatusApps.Split(strSplit);
        //                            for (int jj = 0; jj < strApps.Length; jj++)
        //                            {
        //                                if (strApps[jj].Trim() != "" && strApps[jj].Trim() == dr["applicationid"].ToString().Trim())
        //                                {
        //                                    boolValidApp = true;
        //                                    break;
        //                                }
        //                            }
        //                            if ((oSpan.Days >= intWeeklyStatus && boolValidApp == true) || (boolNever == true && boolValidApp == true))
        //                            {
        //                                string strLink = "&nbsp;";
        //                                string strDefault = oUser.GetApplicationUrl(intUser, intWorkloadPage);
        //                                if (strDefault != "")
        //                                    strLink = "<a href=\"" + oVariable.URL() + "/redirect.aspx?referrer=/" + strDefault + oPage.GetFullLink(intWorkloadPage) + "?pid=" + dr["projectid"].ToString() + "\" target=\"_blank\">View</a>";
        //                                strBody += "<tr><td>" + dr["projectid"].ToString() + "</td><td>" + dr["name"].ToString() + "</td><td>" + dr["number"].ToString() + "</td><td>" + strDate + "</td><td>" + strLink + "</td></tr>";
        //                            }
        //                            intOldUser = intUser;
        //                        }
        //                        if (strBody != "")
        //                        {
        //                            strBody = "<table border=\"0\" cellpadding=\"7\" cellspacing=\"0\" style=\"font-family:'openSans', Verdana, Arial, Helvetica, sans-serif; font-size:11px; border:solid 1px #CCCCCC\"><tr bgcolor=\"#EEEEEE\"><td><b>Project ID</b></td><td><b>Project Name</b></td><td><b>Project Number</b></td><td><b>Last Updated</b></td><td></td></tr>" + strBody + "</table>";
        //                            if (intTest == 0)
        //                                oFunction.SendEmail("REMINDER: Weekly Status", oUser.GetName(intUser), "", strEMailIdsBCC, "REMINDER: Weekly Status", strPreface + "<p><b>This message is to remind you to update the progress of the following requests...</b></p><p>" + strBody + "</p>", true, false);
        //                            else
        //                                oFunction.SendEmail("REMINDER: Weekly Status", "", "", strEMailIdsBCC, "REMINDER: Weekly Status", strPreface + "<p>TO: " + oUser.GetFullName(intUser) + "<br/>CC: " + "" + "</p><p><b>This message is to remind you to update the progress of the following requests...</b></p><p>" + strBody + "</p>", true, false);
        //                        }
        //                    }
        //                    catch (Exception oError)
        //                    {
        //                        oFunction.SendEmail("ERROR: Weekly Status", strEMailIdsBCC, "", "", "ERROR: Weekly Status", strPreface + "<p><b>An error occurred when attempting to send a ClearView Reminder...</b></p><p>Error Message:" + oError.Message + "</p>", true, false);
        //                    }
        //                }
        //                if (intLog > 0)
        //                    oEventLog.WriteEntry(String.Format("REMINDER: Weekly Status - Total Reminders: " + ds.Tables[0].Rows.Count.ToString()), EventLogEntryType.Information);
        //            }
        //            else if (intTest == 1)
        //                oEventLog.WriteEntry(String.Format("DEBUG: " + _tick.ToShortTimeString() + " != " + oCheck.ToShortTimeString()), EventLogEntryType.Information);
        //        }
        //    }
        //}
        //private void CheckPendingAssignments()
        //{
        //    DateTime oCheck = oStart;
        //    string[] strTimes;
        //    char[] strSplit = { ';' };
        //    strTimes = strPendingAssign.Split(strSplit);
        //    for (int ii = 0; ii < strTimes.Length; ii++)
        //    {
        //        if (strTimes[ii].Trim() != "")
        //        {
        //            DateTime _tick = DateTime.Parse(strTimes[ii].Trim());
        //            if (_tick.ToShortTimeString() == oCheck.ToShortTimeString())
        //            {
        //                boolDone = true;
        //                ResourceRequest oResourceRequest = new ResourceRequest(0, dsn);
        //                DataSet ds = oResourceRequest.GetAwaiting();
        //                int intUser = 0;
        //                int intOldUser = 0;
        //                string strBody = "";
        //                if (ds.Tables[0].Rows.Count > 0)
        //                {
        //                    try
        //                    {
        //                        foreach (DataRow dr in ds.Tables[0].Rows)
        //                        {
        //                            intUser = Int32.Parse(dr["userid"].ToString());
        //                            DateTime _modified = DateTime.Parse(dr["modified"].ToString());
        //                            TimeSpan oSpan = new TimeSpan();
        //                            oSpan = oCheck.Subtract(_modified);
        //                            if (intUser != intOldUser && intOldUser != 0)
        //                            {
        //                                // Send Email
        //                                if (strBody != "")
        //                                {
        //                                    strBody = "<table border=\"0\" cellpadding=\"7\" cellspacing=\"0\" style=\"font-family:'openSans', Verdana, Arial, Helvetica, sans-serif; font-size:11px; border:solid 1px #CCCCCC\"><tr bgcolor=\"#EEEEEE\"><td><b>Project Name</b></td><td><b>Request Type</b></td><td><b>Start Date</b></td><td></td></tr>" + strBody + "</table>";
        //                                    if (intTest == 0)
        //                                        oFunction.SendEmail("REMINDER: Pending Assignments", oUser.GetName(intOldUser), "", strEMailIdsBCC, "REMINDER: Pending Assignments", strPreface + "<p><b>This message is to notify you that the following requests are still pending assignment...</b></p><p>" + strBody + "</p>", true, false);
        //                                    else
        //                                        oFunction.SendEmail("REMINDER: Pending Assignments", "", "", strEMailIdsBCC, "REMINDER: Pending Assignments", strPreface + "<p>TO: " + oUser.GetFullName(intOldUser) + "<br/>CC: " + "" + "</p><p><b>This message is to notify you that the following requests are still pending assignment...</b></p><p>" + strBody + "</p>", true, false);
        //                                    strBody = "";
        //                                }
        //                            }
        //                            if (oSpan.Days >= intPendingAssign)
        //                            {
        //                                string strLink = "&nbsp;";
        //                                string strDefault = oUser.GetApplicationUrl(intUser, intAssignPage);
        //                                if (strDefault != "")
        //                                    strLink = "<a href=\"" + oVariable.URL() + "/redirect.aspx?referrer=/" + strDefault + oPage.GetFullLink(intAssignPage) + "?rrid=" + dr["id"].ToString() + "\" target=\"_blank\">View</a>";
        //                                strBody += "<tr><td>" + dr["name"].ToString() + "</td><td>" + dr["requesttype"].ToString() + "</td><td>" + DateTime.Parse(dr["start_date"].ToString()).ToLongDateString() + "</td><td>" + strLink + "</td></tr>";
        //                            }
        //                            intOldUser = intUser;
        //                        }
        //                        if (strBody != "")
        //                        {
        //                            strBody = "<table border=\"0\" cellpadding=\"7\" cellspacing=\"0\" style=\"font-family:'openSans', Verdana, Arial, Helvetica, sans-serif; font-size:11px; border:solid 1px #CCCCCC\"><tr bgcolor=\"#EEEEEE\"><td><b>Project Name</b></td><td><b>Request Type</b></td><td><b>Start Date</b></td><td></td></tr>" + strBody + "</table>";
        //                            if (intTest == 0)
        //                                oFunction.SendEmail("REMINDER: Pending Assignments", oUser.GetName(intUser), "", strEMailIdsBCC, "REMINDER: Pending Assignments", strPreface + "<p><b>This message is to notify you that the following requests are still pending assignment...</b></p><p>" + strBody + "</p>", true, false);
        //                            else
        //                                oFunction.SendEmail("REMINDER: Pending Assignments", "", "", strEMailIdsBCC, "REMINDER: Pending Assignments", strPreface + "<p>TO: " + oUser.GetFullName(intUser) + "<br/>CC: " + "" + "</p><p><b>This message is to notify you that the following requests are still pending assignment...</b></p><p>" + strBody + "</p>", true, false);
        //                        }
        //                    }
        //                    catch (Exception oError)
        //                    {
        //                        oFunction.SendEmail("ERROR: Pending Assignments", strEMailIdsBCC, "", "", "ERROR: Pending Assignments", strPreface + "<p><b>An error occurred when attempting to send a ClearView Reminder...</b></p><p>Error Message:" + oError.Message + "</p>", true, false);
        //                    }
        //                }
        //                if (intLog > 0)
        //                    oEventLog.WriteEntry(String.Format("REMINDER: Pending Assignments - Total Reminders: " + ds.Tables[0].Rows.Count.ToString()), EventLogEntryType.Information);
        //            }
        //            else if (intTest == 1)
        //                oEventLog.WriteEntry(String.Format("DEBUG: " + _tick.ToShortTimeString() + " != " + oCheck.ToShortTimeString()), EventLogEntryType.Information);
        //        }
        //    }
        //}
        //private void CheckDesignImplementations()
        //{
        //    try
        //    {
        //        DateTime oCheck = oStart;
        //        string[] strTimes;
        //        char[] strSplit = { ';' };
        //        strTimes = strDesignImplementationTime.Split(strSplit);
        //        for (int ii = 0; ii < strTimes.Length; ii++)
        //        {
        //            if (strTimes[ii].Trim() != "")
        //            {
        //                DateTime _tick = DateTime.Parse(strTimes[ii].Trim());
        //                if (_tick.ToShortTimeString() == oCheck.ToShortTimeString())
        //                {
        //                    boolDone = true;
        //                    ResourceRequest oResourceRequest = new ResourceRequest(0, dsn);
        //                    DataSet ds = oResourceRequest.GetItem(intDesignImplementationItem);
        //                    int intUser = 0;
        //                    int intOldUser = 0;
        //                    string strBody = "";
        //                    if (ds.Tables[0].Rows.Count > 0)
        //                    {
        //                        try
        //                        {
        //                            foreach (DataRow dr in ds.Tables[0].Rows)
        //                            {
        //                                intUser = Int32.Parse(dr["userid"].ToString());
        //                                if (intUser != intOldUser && intOldUser != 0)
        //                                {
        //                                    // Send Email
        //                                    if (strBody != "")
        //                                    {
        //                                        strBody = "<table border=\"0\" cellpadding=\"7\" cellspacing=\"0\" style=\"font-family:'openSans', Verdana, Arial, Helvetica, sans-serif; font-size:11px; border:solid 1px #CCCCCC\"><tr bgcolor=\"#EEEEEE\"><td><b>Project ID</b></td><td><b>Project Name</b></td><td><b>Project Number</b></td><td></td></tr>" + strBody + "</table>";
        //                                        if (intTest == 0)
        //                                            oFunction.SendEmail("REMINDER: Design Implementation", oUser.GetName(intOldUser), "", strEMailIdsBCC, "REMINDER: Design Implementation", strPreface + "<p><b>This message is to remind you that the following projects have implementations scheduled for today...</b></p><p>" + strBody + "</p>", true, false);
        //                                        else
        //                                            oFunction.SendEmail("REMINDER: Design Implementation", "", "", strEMailIdsBCC, "REMINDER: Design Implementation", strPreface + "<p>TO: " + oUser.GetFullName(intOldUser) + "<br/>CC: " + "" + "</p><p><b>This message is to remind you that the following projects have implementations scheduled for today...</b></p><p>" + strBody + "</p>", true, false);
        //                                        strBody = "";
        //                                    }
        //                                }
        //                                string strLink = "&nbsp;";
        //                                string strDefault = oUser.GetApplicationUrl(intUser, intDesignPage);
        //                                if (strDefault != "")
        //                                    strLink = "<a href=\"" + oVariable.URL() + "/redirect.aspx?referrer=/" + strDefault + oPage.GetFullLink(intDesignPage) + "?id=" + dr["id"].ToString() + "\" target=\"_blank\">View</a>";
        //                                strBody += "<tr><td>" + dr["projectid"].ToString() + "</td><td>" + dr["name"].ToString() + "</td><td>" + dr["number"].ToString() + "</td><td>" + strLink + "</td></tr>";
        //                                intOldUser = intUser;
        //                            }
        //                            if (strBody != "")
        //                            {
        //                                strBody = "<table border=\"0\" cellpadding=\"7\" cellspacing=\"0\" style=\"font-family:'openSans', Verdana, Arial, Helvetica, sans-serif; font-size:11px; border:solid 1px #CCCCCC\"><tr bgcolor=\"#EEEEEE\"><td><b>Project ID</b></td><td><b>Project Name</b></td><td><b>Project Number</b></td><td></td></tr>" + strBody + "</table>";
        //                                if (intTest == 0)
        //                                    oFunction.SendEmail("REMINDER: Design Implementation", oUser.GetName(intUser), "", strEMailIdsBCC, "REMINDER: Design Implementation", strPreface + "<p><b>This message is to remind you that the following projects have implementations scheduled for today...</b></p><p>" + strBody + "</p>", true, false);
        //                                else
        //                                    oFunction.SendEmail("REMINDER: Design Implementation", "", "", strEMailIdsBCC, "REMINDER: Design Implementation", strPreface + "<p>TO: " + oUser.GetFullName(intUser) + "<br/>CC: " + "" + "</p><p><b>This message is to remind you that the following projects have implementations scheduled for today...</b></p><p>" + strBody + "</p>", true, false);
        //                            }
        //                        }
        //                        catch (Exception oError)
        //                        {
        //                            oFunction.SendEmail("ERROR: Design Implementation", strEMailIdsBCC, "", "", "ERROR: Design Implementation", strPreface + "<p><b>An error occurred when attempting to send a ClearView Reminder...</b></p><p>Error Message:" + oError.Message + "</p>", true, false);
        //                        }
        //                    }
        //                    if (intLog > 0)
        //                        oEventLog.WriteEntry(String.Format("REMINDER: Design Implementation - Total Reminders: " + ds.Tables[0].Rows.Count.ToString()), EventLogEntryType.Information);
        //                }
        //                else if (intTest == 1)
        //                    oEventLog.WriteEntry(String.Format("DEBUG: " + _tick.ToShortTimeString() + " != " + oCheck.ToShortTimeString()), EventLogEntryType.Information);
        //            }
        //        }
        //    }
        //    catch (Exception oError)
        //    {
        //        oEventLog.WriteEntry(String.Format("ERROR: " + oError.Message), EventLogEntryType.Error);
        //    }
        //}
        private void GenerateTasks()
        {
            DateTime oCheck = oStart;
            string[] strTimes;
            char[] strSplit = { ';' };
            strTimes = strGenerateTasksTime.Split(strSplit);
            for (int ii = 0; ii < strTimes.Length; ii++)
            {
                if (strTimes[ii].Trim() != "")
                {
                    DateTime _tick = DateTime.Parse(strTimes[ii].Trim());
                    if (_tick.ToShortTimeString() == oCheck.ToShortTimeString())
                    {
                        boolDone = true;
                        try
                        {
                            //AutoTasks oAutoTask = new AutoTasks(dsn, dsnAsset, dsnIP, dsnServiceEditor, intAutoAccount, intOrganization, intComputerObjectItem, intEnvironment,  intAssignPage, intWorkloadPage, oEventLog);
                            //                                    oAutoTask.AD_Old_Accounts();            // Checks the lastlogin attribute of the user objects for outdated accounts
                            //                                    oAutoTask.AD_Mismatched_Accounts();     // Compares production XIDs with test TIDs and disables TIDs that don't match production
                            //oAutoTask.AD_Mismatched_Computers();    // Gets all computer objects in TEST / DEV and validates the current domain
                        }
                        catch (Exception oError)
                        {
                            strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DEVELOPER_ERROR");

                            oFunction.SendEmail("ERROR: Auto-Generating Tasks", strEMailIdsBCC, "", "", "ERROR: Auto-Generating Tasks", strPreface + "<p><b>An error occurred when attempting to send a ClearView Reminder...</b></p><p>Error Message:" + oError.Message + "</p>", true, false);
                        }
                    }
                    else if (intTest == 1)
                        oEventLog.WriteEntry(String.Format("DEBUG: " + _tick.ToShortTimeString() + " != " + oCheck.ToShortTimeString()), EventLogEntryType.Information);
                }
            }
        }

        #region SLANotification

        public void ServiceRequestSLANotifications()
        {
            Pages oPage = new Pages(0, dsn);
            Applications oApplication = new Applications(0, dsn);
            RequestItems oRequestItem = new RequestItems(0, dsn);
            Variables oVariable = new Variables(intEnvironment);
            Services oService = new Services(0, dsn);
            ResourceRequest oResourceRequest = new ResourceRequest(0, dsn);
            DataSet ds = oResourceRequest.GetSLANotificationServiceRequests();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                string strResourceRequestID = dr["ResourceRequestID"].ToString();
                try
                {
                    double dblSLAHours = double.Parse(dr["sla"].ToString());
                    if (dblSLAHours > 0.00)
                    {
                        DateTime dtRequiredCompletionDate = oHoliday.GetHours(dblSLAHours, DateTime.Parse(dr["AssignedDate"].ToString()));
                        if (dtRequiredCompletionDate <= DateTime.Now)
                        {
                            int intService = Int32.Parse(dr["RequestServiceID"].ToString());
                            int intItem = Int32.Parse(dr["RequestItemID"].ToString());
                            //Send notification
                            string strTO = dr["ServiceOwnerXIDs"].ToString();
                            string strCC = dr["TechnicianXIDs"].ToString();
                            if (strCC.EndsWith(";") == false)
                                strCC += ";";
                            strCC += dr["RequestAssignedByXID"].ToString();
                            //if (strCC.EndsWith(";") == false)
                            //    strCC += ";";
                            //strCC += dr["RequestedByXID"].ToString();
                            if (strCC.EndsWith(";") == false)
                                strCC += ";";
                            //if (intEnvironment == (int)CurrentEnvironment.CORPDMN || intEnvironment == (int)CurrentEnvironment.PNCNT_PROD)
                            //    strCC += "XTML83U;XSZSG3A";

                            // For now just send to us - comment out the following when ready...
                            //strTO = oVariable.BCC();
                            //strCC = oVariable.BCC();
                            //string strDebug = "<p><b>TO:</b> " + dr["ServiceOwnerNames"].ToString() + "</p>";
                            //strDebug += "<p><b>CC:</b> " + dr["Technicians"].ToString() + "</p>";
                            //strDebug += "<p><b>CC:</b> " + dr["RequestAssignedBy"].ToString() + "</p>";
                            //strDebug += "<p><b>CC:</b> " + dr["RequestedBy"].ToString() + "</p>";
                            string strDebug = "";

                            int intApp = oRequestItem.GetItemApplication(intItem);
                            string strDefault = oApplication.GetUrl(intApp, intServicePage);
                            strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_REQUEST_STATUS");

                            if (strDefault == "")
                                oFunction.SendEmail("SLA Notification: " + oService.GetName(intService), strTO, strCC, strEMailIdsBCC, "SLA Notification: " + oService.GetName(intService), strPreface + strDebug + "<p><b>The following service request has breached its " + dblSLAHours.ToString("F") + " hour Service Level Agreement...</b></p><p>" + oResourceRequest.GetSummary(Int32.Parse(dr["ResourceRequestID"].ToString()), 0, intEnvironment, dsnServiceEditor, dsnAsset, dsnIP) + "</p>", true, false);
                            else
                                oFunction.SendEmail("SLA Notification: " + oService.GetName(intService), strTO, strCC, strEMailIdsBCC, "SLA Notification: " + oService.GetName(intService), strPreface + strDebug + "<p><b>The following service request has breached its " + dblSLAHours.ToString("F") + " hour Service Level Agreement...</b></p><p>" + oResourceRequest.GetSummary(Int32.Parse(dr["ResourceRequestID"].ToString()), 0, intEnvironment, dsnServiceEditor, dsnAsset, dsnIP) + "</p><p><b>NOTE: To change the service level agreement for this service, <a href=\"" + oVariable.URL() + "/redirect.aspx?referrer=/" + strDefault + oPage.GetFullLink(intServicePage) + "?sid=" + intService.ToString() + "&menu_tab=3&edit=4\" target=\"_blank\">click here</a>.</p>", true, false);

                            //Update SLANotificationDate
                            oResourceRequest.UpdateSLANotificationDateForServiceRequest(Int32.Parse(dr["ResourceRequestID"].ToString()), Int32.Parse(dr["RequestId"].ToString()), intItem, Int32.Parse(dr["RequestNumberID"].ToString()), intService);
                        }
                    }
                }
                catch (Exception oError)
                {
                    string strError = "Service Request SLA Notification: ResourceRequestID = " + strResourceRequestID + " (Error Message: " + oError.Message + ") (Source: " + oError.Source + ") (Stack Trace: " + oError.StackTrace + ")";
                    oEventLog.WriteEntry(strError, EventLogEntryType.Error);
                }
            }
        }

        #endregion


        public void IncidentsCreated()
        {
            // These take forever but only run once.  When the incident is returned, the other query runs.
            string strErrorName = "";
            try
            {
                DataSet ds = oIncident.GetCreated();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    oLog.AddEvent("SERVICENOW", "API", "There are " + ds.Tables[0].Rows.Count.ToString() + " incidents to be generated.", LoggingType.Debug);

                    // Loop through
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        bool workstation = (dr["workstation"].ToString() == "1");
                        int errorid = Int32.Parse(dr["errorid"].ToString());
                        int relatedid = Int32.Parse(dr["relatedid"].ToString());
                        string route = dr["route"].ToString();
                        string message = dr["message"].ToString();
                        string priority = dr["priority"].ToString();

                        if (message.Contains("@"))
                        {
                            DataSet dsVariables = oIncident.DeviceInformation(relatedid, (workstation ? 1 : 0));
                            oLog.AddEvent("SERVICENOW", "API", "Transposing message using " + dsVariables.Tables[0].Rows.Count.ToString() + " record(s).", LoggingType.Debug);
                            oLog.AddEvent("SERVICENOW", "API", "Message before = " + message, LoggingType.Debug);
                            if (dsVariables.Tables[0].Rows.Count > 0)
                            {
                                DataRow drVariable = dsVariables.Tables[0].Rows[0];
                                while (message.Contains("@"))
                                {
                                    string before = message.Substring(0, message.IndexOf("@"));
                                    string after = message.Substring(message.IndexOf("@") + 1);
                                    string variable = after;
                                    int indexOf = 9999;
                                    if (after.IndexOf(" ") > 0)
                                        indexOf = after.IndexOf(" ");
                                    if (after.IndexOf(".") > 0 && after.IndexOf(".") < indexOf)
                                        indexOf = after.IndexOf(".");
                                    if (after.IndexOf(",") > 0 && after.IndexOf(",") < indexOf)
                                        indexOf = after.IndexOf(",");
                                    if (after.IndexOf(";") > 0 && after.IndexOf(";") < indexOf)
                                        indexOf = after.IndexOf(",");
                                    if (after.IndexOf(";") > 0 && after.IndexOf(";") < indexOf)
                                        indexOf = after.IndexOf(";");
                                    if (after.IndexOf("!") > 0 && after.IndexOf("!") < indexOf)
                                        indexOf = after.IndexOf("!");

                                    if (indexOf < 9999)
                                    {
                                        variable = after.Substring(0, indexOf);
                                        after = after.Substring(indexOf);
                                    }
                                    else
                                        after = "";
                                    message = before + drVariable[variable].ToString() + after;
                                }
                            }
                            oLog.AddEvent("SERVICENOW", "API", "Message after = " + message, LoggingType.Debug);
                        }

                        // First, create the incident using BPPM
                        StringBuilder bppm = new StringBuilder();
                        //bppm.AppendLine("E:");
                        //bppm.AppendLine("cd BPPM");
                        //if (intEnvironment == (int)CurrentEnvironment.PNCNT_PROD)
                        //    bppm.AppendLine("cd Prod");
                        //else
                        //    bppm.AppendLine("cd QA");
                        //bppm.AppendLine("msend -n cell_other -a CUST_MISC_EVENT -b \"cust_misc_environment=SERVICE;cust_misc_who=ClearView;cust_misc_message=" + message + ";cust_misc_where=" + route + ";cust_misc_what=" + (workstation ? "WORKSTATION" : "SERVER") + ";cust_misc_whatvar=" + errorid.ToString() + "\"");
                        bppm.AppendLine("echo off");
                        string folder = "E:\\BPPM\\QA\\";
                        if (intEnvironment == (int)CurrentEnvironment.PNCNT_PROD)
                            folder = "E:\\BPPM\\Prod\\";
                        bppm.AppendLine("set MCELL_HOME=" + folder);
                        bppm.AppendLine("set MCELL_DIR=" + folder);
                        bppm.AppendLine("E:\\BPPM\\bin\\msend -n cell_other -a CUST_MISC_EVENT -b \"cust_misc_environment=SERVICE;cust_misc_who=ClearView;cust_severity=" + priority + ";cust_misc_message=" + message + ";cust_misc_where=" + route + ";cust_misc_what=" + (workstation ? "WORKSTATION" : "SERVER") + ";cust_misc_whatvar=" + errorid.ToString() + "\"");
                        // Save the file
                        DateTime _now = DateTime.Now;
                        string strNow = _now.Day.ToString() + _now.Month.ToString() + _now.Year.ToString() + _now.Hour.ToString() + _now.Minute.ToString() + _now.Second.ToString() + _now.Millisecond.ToString();
                        string bppmPath = strScripts + strSub + (workstation ? "W_" : "S_") + errorid.ToString() + "_" + strNow + ".bat";
                        string bppmOut = strScripts + strSub + (workstation ? "W_" : "S_") + errorid.ToString() + "_" + strNow + ".txt";
                        using (System.IO.StreamWriter bppmFile = new System.IO.StreamWriter(bppmPath))
                            bppmFile.WriteLine(bppm.ToString());

                        // Execute the script
                        System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo("cmd.exe");
                        info.WorkingDirectory = strScripts + strSub;
                        //if (intEnvironment == (int)CurrentEnvironment.PNCNT_PROD)
                        //    info.WorkingDirectory = "E:\\BPPM\\Prod\\";
                        //else
                        //    info.WorkingDirectory = "E:\\BPPM\\QA\\";
                        info.Arguments = "/c \"" + bppmPath + " > " + bppmOut + "\"";
                        oLog.AddEvent("BPPM", errorid.ToString(), "Beginning BPPM for " + (workstation ? "WORKSTATION" : "SERVER") + " error # " + errorid.ToString() + "...", LoggingType.Information);
                        System.Diagnostics.Process proc = System.Diagnostics.Process.Start(info);
                        bool boolTimeout = false;
                        proc.WaitForExit(intTimeoutDefault);
                        if (proc.HasExited == false)
                        {
                            proc.Kill();
                            boolTimeout = true;
                        }
                        proc.Close();
                        if (boolTimeout == false)
                        {
                            oLog.AddEvent("SERVICENOW", "API", "BPPM was successful!", LoggingType.Debug);
                            oIncident.Update(errorid, (workstation ? 1 : 0), DateTime.Now.ToString());
                        }
                        else
                        {
                            oLog.AddEvent("SERVICENOW", "API", "BPPM was NOT successful!", LoggingType.Error);
                            oIncident.Delete(errorid, (workstation ? 1 : 0));
                        }
                    }
                }
            }
            catch (Exception oError)
            {
                strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DEVELOPER_ERROR");
                string strError = "(Error Message: " + oError.Message + ") (Source: " + oError.Source + ") (Stack Trace: " + oError.StackTrace + ") [" + System.Environment.UserName + "]";
                oEventLog.WriteEntry(strError, EventLogEntryType.Error);
                oFunction.SendEmail("ERROR: Service Now Incident Generation", strEMailIdsBCC, "", "", "ERROR: Service Now Incident Generation", strPreface + "<p><b>An unexpected error occurred...</b></p><p>Name: " + strErrorName + "</p><p>Error Message:" + strError + "</p>", true, false);
            }

        }

        public void IncidentsGenerated()
        {
            // These take forever but only run once.  When the incident is returned, the other query runs.
            string strErrorName = "";
            try
            {
                DataSet ds = oIncident.GetGenerated();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    oLog.AddEvent("SERVICENOW", "API", "There are " + ds.Tables[0].Rows.Count.ToString() + " incidents to be queried.", LoggingType.Debug);
                    // Setup web service
                    System.Net.NetworkCredential oCredentials = new System.Net.NetworkCredential(oVariable.ADUser(), oVariable.ADPassword(), oVariable.Domain());
                    ClearViewWebServices oServiceNow = new ClearViewWebServices();
                    oServiceNow.Timeout = Timeout.Infinite;
                    oServiceNow.Credentials = oCredentials;
                    oServiceNow.Url = oVariable.WebServiceURL();

                    // Loop through
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        bool workstation = (dr["workstation"].ToString() == "1");
                        int errorid = Int32.Parse(dr["errorid"].ToString());
                        DateTime datGenerated = DateTime.Parse(dr["generated"].ToString());

                        // Second, query the incident
                        string url = "https://webqa-itsm.pncbank.com";
                        DataSet dsKey = oFunction.GetSetupValuesByKey("SERVICE_NOW_API");
                        if (dsKey.Tables[0].Rows.Count > 0)
                            url = dsKey.Tables[0].Rows[0]["Value"].ToString();

                        // Query Service Now for Incident Information
                        oLog.AddEvent("SERVICENOW", "API", "Checking to see if the incident was generated using error # " + errorid.ToString() + " (" + (workstation ? "WORKSTATION" : "SERVER") + ")", LoggingType.Debug);
                        bool found = false;
                        try
                        {
                            string data = oServiceNow.GetServiceNowIncident(url, oVariable.ServiceNowUsername(), oVariable.ServiceNowPassword(), errorid, (workstation ? "WORKSTATION" : "SERVER"));
                            string[] values = data.Split(new string[] { "|" }, StringSplitOptions.None);
                            if (values.Length == 5)
                            {
                                oLog.AddEvent("SERVICENOW", "API", "Success = " + data, LoggingType.Information);
                                string incident = values[0];
                                string state = values[1];
                                string user = values[2];
                                string group = values[3];
                                string resolved = values[4];

                                oIncident.Update(errorid, (workstation ? 1 : 0), DateTime.Now.ToString(), incident);
                                int assigned = 0;
                                if (String.IsNullOrEmpty(user) == false)
                                    assigned = oUser.GetId(user);
                                if (workstation)
                                    oWorkstation.UpdateVirtualError(errorid, incident, assigned);
                                else
                                    oServer.UpdateError(errorid, incident, assigned);
                                found = true;
                            }
                            else
                                oLog.AddEvent("SERVICENOW", "API", "Nothing = " + data, LoggingType.Debug);
                        }
                        catch (Exception exServiceNow)
                        {
                            string ServiceNowError = exServiceNow.Message;
                            while (exServiceNow.InnerException != null)
                            {
                                exServiceNow = exServiceNow.InnerException;
                                ServiceNowError += " ~ " + exServiceNow.Message;
                            }
                            oLog.AddEvent("SERVICENOW", "API", "Exception = " + ServiceNowError, LoggingType.Debug);
                        }

                        if (found == false)
                        {
                            TimeSpan diff = DateTime.Now.Subtract(datGenerated);
                            if (diff.Minutes >= 10)
                            {
                                oLog.AddEvent("SERVICENOW", "API", "Could not find an incident in Service Now after 10 minutes!", LoggingType.Error);
                                oIncident.Delete(errorid, (workstation ? 1 : 0));
                            }
                            else
                                oLog.AddEvent("SERVICENOW", "API", "Difference is " + diff.Minutes.ToString() + " minutes...try again.", LoggingType.Debug);
                        }
                    }
                }
            }
            catch (Exception oError)
            {
                strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DEVELOPER_ERROR");
                string strError = "(Error Message: " + oError.Message + ") (Source: " + oError.Source + ") (Stack Trace: " + oError.StackTrace + ") [" + System.Environment.UserName + "]";
                oEventLog.WriteEntry(strError, EventLogEntryType.Error);
                try
                {
                    oLog.AddEvent("SERVICENOW", strErrorName, "ERROR: Service Now Incident Query - " + strError, LoggingType.Error);
                }
                catch { }
            }

        }

        public void IncidentsResolved()
        {
            string strErrorName = "";
            try
            {
                DataSet ds = oIncident.GetUpdated();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    // Setup web service
                    System.Net.NetworkCredential oCredentials = new System.Net.NetworkCredential(oVariable.ADUser(), oVariable.ADPassword(), oVariable.Domain());
                    ClearViewWebServices oServiceNow = new ClearViewWebServices();
                    oServiceNow.Timeout = Timeout.Infinite;
                    oServiceNow.Credentials = oCredentials;
                    oServiceNow.Url = oVariable.WebServiceURL();
                    // Loop through
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        bool workstation = (dr["workstation"].ToString() == "1");
                        int errorid = Int32.Parse(dr["errorid"].ToString());
                        string incident = strErrorName = dr["incident"].ToString();
                        string url = "https://webqa-itsm.pncbank.com";
                        DataSet dsKey = oFunction.GetSetupValuesByKey("SERVICE_NOW_API");
                        if (dsKey.Tables[0].Rows.Count > 0)
                            url = dsKey.Tables[0].Rows[0]["Value"].ToString();
                        // Query Service Now for Incident Information
                        oLog.AddEvent("SERVICENOW", incident, "Checking status of incident...", LoggingType.Debug);
                        string data = oServiceNow.GetServiceNowIncidentNumber(url, oVariable.ServiceNowUsername(), oVariable.ServiceNowPassword(), incident);
                        string[] values = data.Split(new string[] { "|"}, StringSplitOptions.None);
                        if (values.Length == 5)
                        {
                            string state = values[1];
                            string user = values[2];
                            string group = values[3];
                            string resolved = values[4];

                            if (state == "6" || state == "7")       // 6 = Resolved (can be reopened). 7 = Closed (cannot be reopened).
                            {
                                oLog.AddEvent("SERVICENOW", incident, "Resolved.", LoggingType.Information);
                                // Incident has been resolved, mark complete.
                                int related = Int32.Parse(dr["relatedid"].ToString());
                                int step = Int32.Parse(dr["step"].ToString());

                                oIncident.Update(errorid, (workstation ? 1 : 0), DateTime.Now.ToString(), resolved, "");
                                if (workstation)
                                    oWorkstation.UpdateVirtualError(related, step, errorid, -999);
                                else
                                {
                                    oServer.UpdateError(related, step, errorid, -999, true, dsnAsset);
                                    // If Avamar error, reload avamar
                                    oServer.UpdateAvamar(related);    // stored procedure will automatically find the place to reset.
                                }
                            }
                            else
                            {
                                int assigned = 0;
                                if (String.IsNullOrEmpty(user) == false)
                                    assigned = oUser.GetId(user);
                                if (workstation)
                                    oWorkstation.UpdateVirtualError(errorid, incident, assigned);
                                else
                                    oServer.UpdateError(errorid, incident, assigned);
                                oLog.AddEvent("SERVICENOW", incident, "STATE = " + state, LoggingType.Debug);
                            }
                        }
                        else
                            oLog.AddEvent("SERVICENOW", "API", "Nothing = " + data, LoggingType.Debug);
                    }
                }
            }
            catch (Exception oError)
            {
                strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DEVELOPER_ERROR");
                string strError = "(Error Message: " + oError.Message + ") (Source: " + oError.Source + ") (Stack Trace: " + oError.StackTrace + ") [" + System.Environment.UserName + "]";
                oEventLog.WriteEntry(strError, EventLogEntryType.Error);
                try
                {
                    oLog.AddEvent("SERVICENOW", strErrorName, "ERROR: Service Now Incident Resolution - " + strError, LoggingType.Error);
                }
                catch { }
            }

        }

    }
}
