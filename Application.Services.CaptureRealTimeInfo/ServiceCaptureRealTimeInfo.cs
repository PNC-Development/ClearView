using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.ServiceProcess;
using System.Text;
using NCC.ClearView.Application.Core;
namespace ClearViewCaptureRealTimeInfo
{
   
    public partial class ServiceCaptureRealTimeInfo : ServiceBase
    {
        public static string strConfigFilePath = "E:\\APPS\\CLV\\ClearViewCaptureRealTimeInfo\\";

        public static string dsn;
        public static string dsnAsset;
        public static string LogFilePath;
        public static int intEnvironment;
        public static  int intLogging;
        public static  EventLog oEventLog = new EventLog();
        public static  Log oLog;
        public static bool boolSSHDebug = false;

        public ServiceCaptureRealTimeInfo()
        {
            InitializeComponent();
            InitializeVariables();

            if (EventLog.SourceExists("ClearView") == false)
            {
                EventLog.CreateEventSource("ClearView", "ClearView");
                EventLog.WriteEntry(String.Format("ClearView EventLog Created"), EventLogEntryType.Information);
            }
            oEventLog = new EventLog();
            oEventLog.Source = "ClearView";
            oEventLog.ModifyOverflowPolicy(OverflowAction.OverwriteAsNeeded, 0);
            oEventLog.MaximumKilobytes = long.Parse("16384");

            EventLog.WriteEntry(String.Format("ClearView Capture Real Time Information Service started"), EventLogEntryType.Information);

            EnclosureAndBlades oEnclosureAndBlades = new EnclosureAndBlades();
            oEnclosureAndBlades.CaptureEnclosureAndBladeInfo();

            EventLog.WriteEntry(String.Format("ClearView Capture Real Time Information Service Completed"), EventLogEntryType.Information);

        }

        protected override void OnStart(string[] args)
        {
            // TODO: Add code here to start your service.
            EventLog.WriteEntry(String.Format("ClearView Capture Real Time Information Service started"), EventLogEntryType.Information);
        }

        protected override void OnStop()
        {
            // TODO: Add code here to perform any tear-down necessary to stop your service.
            EventLog.WriteEntry(String.Format("ClearView Capture Real Time Information Service stoped"), EventLogEntryType.Information);
        }

        public void InitializeVariables()
        { 
            try{
                DataSet ds = new DataSet();
                ds.ReadXml(strConfigFilePath + "config.xml");
                intEnvironment = Int32.Parse(ds.Tables[0].Rows[0]["environment"].ToString());
                string strDSN = ds.Tables[0].Rows[0]["DSN"].ToString();
                string strDSNAsset = ds.Tables[0].Rows[0]["AssetDSN"].ToString();
                dsn = ds.Tables[0].Rows[0][strDSN].ToString();
                dsnAsset = ds.Tables[0].Rows[0][strDSNAsset].ToString();
                LogFilePath = ds.Tables[0].Rows[0]["LogFilePath"].ToString();
                oLog = new Log(0, dsn);
              
                
            }
            catch
            {
                EventLog.WriteEntry(String.Format("ClearView Capture Real Time Information Service initialization has failed - INVALID XML FILE"), EventLogEntryType.Error);
            }
        }

    }
}
