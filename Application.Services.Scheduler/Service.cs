using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Timers;
using NCC.ClearView.Application.Core;

namespace Application.Services.Scheduler
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
        private double dblInterval;
        private string dsn;
        private string dsnAsset;
        private string dsnIP;
        private string dsnServiceEditor;
        private string dsnZeus;
        private int intEnvironment;
        private string strRestartTime;
        private int intRestartHours;
        private int intStartTimeout;
        private string PhysicalTypes;
        private string PhysicalSteps;
        private string VirtualTypes;
        private string VirtualSteps;
        private int WorkstationType;
        private string WorkstationSteps;
        private int intLogging;
        private EventLog oEventLog;
        private Log oLog;
        private string strScripts = "E:\\APPS\\CLV\\ClearViewScheduler\\";

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
                oEventLog.ModifyOverflowPolicy(OverflowAction.OverwriteAsNeeded, 0);
                oEventLog.MaximumKilobytes = long.Parse("16384");
                DataSet ds = new DataSet();
                ds.ReadXml(strScripts + "config.xml");
                dblInterval = Convert.ToDouble(ds.Tables[0].Rows[0]["interval"].ToString());
                intEnvironment = Int32.Parse(ds.Tables[0].Rows[0]["environment"].ToString());
                string strDSN = ds.Tables[0].Rows[0]["DSN"].ToString();
                string strDSNAsset = ds.Tables[0].Rows[0]["AssetDSN"].ToString();
                string strDSNIP = ds.Tables[0].Rows[0]["IpDSN"].ToString();
                string strDSNServiceEditor = ds.Tables[0].Rows[0]["ServiceEditorDSN"].ToString();
                string strDSNZeus = ds.Tables[0].Rows[0]["ZeusDSN"].ToString();
                dsn = ds.Tables[0].Rows[0][strDSN].ToString();
                dsnAsset = ds.Tables[0].Rows[0][strDSNAsset].ToString();
                dsnIP = ds.Tables[0].Rows[0][strDSNIP].ToString();
                dsnServiceEditor = ds.Tables[0].Rows[0][strDSNServiceEditor].ToString();
                dsnZeus = ds.Tables[0].Rows[0][strDSNZeus].ToString();
                strRestartTime = ds.Tables[0].Rows[0]["restart_time"].ToString();
                intRestartHours = Int32.Parse(ds.Tables[0].Rows[0]["restart_hours"].ToString());
                intStartTimeout = Int32.Parse(ds.Tables[0].Rows[0]["start_timeout"].ToString());
                PhysicalTypes = ds.Tables[0].Rows[0]["physical_types"].ToString();
                PhysicalSteps = ds.Tables[0].Rows[0]["physical_steps"].ToString();
                VirtualTypes = ds.Tables[0].Rows[0]["virtual_types"].ToString();
                VirtualSteps = ds.Tables[0].Rows[0]["virtual_steps"].ToString();
                WorkstationType = Int32.Parse(ds.Tables[0].Rows[0]["worksation_type"].ToString());
                WorkstationSteps = ds.Tables[0].Rows[0]["worksation_steps"].ToString();
                intLogging = Int32.Parse(ds.Tables[0].Rows[0]["logging"].ToString());
                oLog = new Log(0, dsn);
                oTimer = new System.Timers.Timer(dblInterval);
                oTimer.Elapsed += new ElapsedEventHandler(this.ServiceTimer_Tick);
                Tick();
            }
            catch
            {
                EventLog.WriteEntry(String.Format("ClearView AP Physical Service initialization has failed - INVALID XML FILE"), EventLogEntryType.Error);
            }
        }

        protected override void OnStart(string[] args)
        {
            oEventLog.WriteEntry(String.Format("ClearView Scheduler Service started."), EventLogEntryType.Information);
            oTimer.AutoReset = true;
            oTimer.Enabled = true;
            oTimer.Start();
        }

        protected override void OnStop()
        {
            oEventLog.WriteEntry(String.Format("ClearView Scheduler Service stopped."), EventLogEntryType.Information);
            oTimer.AutoReset = false;
            oTimer.Enabled = false;
        }
        private void ServiceTimer_Tick(object sender, System.Timers.ElapsedEventArgs e)
        {
            oTimer.Stop();
            if (intLogging >= 3)
                oEventLog.WriteEntry(String.Format("ClearView Scheduler Service TICK."), EventLogEntryType.Information);
            Tick();
            oTimer.Start();
        }

        private void Tick()
        {
            // Start Main Processing
            try
            {
                // Check for Scripts
                ExecuteScripts();

                // Run at 6pm every night
                // Start Decommissions
                CheckServices();
            }
            catch (Exception ex)
            {
                string strError = "Scheduler Service (TICK): " + "(Error Message: " + ex.Message + ") ~ (Source: " + ex.Source + ") (Stack Trace: " + ex.StackTrace + ") [" + System.Environment.UserName + "]";
                oEventLog.WriteEntry(strError, EventLogEntryType.Error);
            }
        }

        private void ExecuteScripts()
        {
            NCC.ClearView.Application.Core.Scheduler oScheduler = new NCC.ClearView.Application.Core.Scheduler(0, dsn);
            DataSet dsScripts = oScheduler.Gets(1);
            foreach (DataRow drScript in dsScripts.Tables[0].Rows)
            {
                // Check to see if it's ready to run
                string name = drScript["name"].ToString();
                int status = Int32.Parse(drScript["status"].ToString());
                bool run = false;

                if (status != SchedulerStatus.Running)
                {
                    if (status == SchedulerStatus.RunOnce)
                    {
                        if (intLogging >= 1)
                            oEventLog.WriteEntry(String.Format("Job \"" + name + "\" - Run Once has been selected."), EventLogEntryType.Information);
                        run = true; // run once is set
                    }
                    else
                    {
                        DateTime now = DateTime.Now;

                        // Check that schedule can run today
                        string[] days = drScript["days"].ToString().Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                        bool dayOK = false;
                        foreach (string day in days)
                        {
                            string DAY = day.Trim().ToUpper();
                            if (intLogging >= 3)
                                oEventLog.WriteEntry(String.Format("Checking DAY = " + DAY), EventLogEntryType.FailureAudit);

                            int date = 0;
                            if (Int32.TryParse(DAY, out date))
                            {
                                // day is a number - check against date of month
                                if (date == now.Day)
                                    dayOK = true;
                            }
                            else
                            {
                                // day is NOT a number - check list of known values
                                if (DAY == "SU" && now.DayOfWeek == DayOfWeek.Sunday)
                                    dayOK = true;
                                else if (DAY == "MO" && now.DayOfWeek == DayOfWeek.Monday)
                                    dayOK = true;
                                else if (DAY == "TU" && now.DayOfWeek == DayOfWeek.Tuesday)
                                    dayOK = true;
                                else if (DAY == "WE" && now.DayOfWeek == DayOfWeek.Wednesday)
                                    dayOK = true;
                                else if (DAY == "TH" && now.DayOfWeek == DayOfWeek.Thursday)
                                    dayOK = true;
                                else if (DAY == "FR" && now.DayOfWeek == DayOfWeek.Friday)
                                    dayOK = true;
                                else if (DAY == "SA" && now.DayOfWeek == DayOfWeek.Saturday)
                                    dayOK = true;
                                else
                                {
                                    // last day of month
                                    DateTime endOfMonth = new DateTime(now.Year, now.Month, 1).AddMonths(1).AddDays(-1);
                                    if (DAY == "L" && now.Day == endOfMonth.Day)
                                        dayOK = true;
                                }
                            }
                            if (dayOK)
                            {
                                if (intLogging >= 2)
                                    oEventLog.WriteEntry(String.Format("DAY = " + DAY + " is Valid!"), EventLogEntryType.Information);
                                break;
                            }
                            else if (intLogging >= 3)
                                oEventLog.WriteEntry(String.Format("DAY = " + DAY + " is not valid"), EventLogEntryType.Warning);
                        }

                        // Finished with Days
                        if (dayOK)
                        {
                            // Check the time
                            string[] times = drScript["times"].ToString().Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                            bool timeOK = false;
                            foreach (string time in times)
                            {
                                if (String.IsNullOrEmpty(time) == false)
                                {
                                    if (intLogging >= 3)
                                        oEventLog.WriteEntry(String.Format("Checking Time = " + time), EventLogEntryType.Information);
                                    DateTime _tick = DateTime.Parse(time.Trim());
                                    if (_tick.ToShortTimeString() == now.ToShortTimeString())
                                    {
                                        timeOK = true;
                                        if (intLogging >= 2)
                                            oEventLog.WriteEntry(String.Format("Time = " + time + " is Valid!"), EventLogEntryType.Information);
                                        break;
                                    }
                                    else if (intLogging >= 3)
                                        oEventLog.WriteEntry(String.Format("Time = " + time + " is not valid (" + now.ToShortTimeString() + ")"), EventLogEntryType.Warning);
                                }
                            }

                            // Finished with Times
                            if (timeOK)
                            {
                                // all conditions met
                                run = true;
                            }
                        }
                    }
                }

                // Check to see if the job can be run
                if (run)
                {
                    if (intLogging >= 1)
                        oEventLog.WriteEntry(String.Format("Executing Job \"" + name + "\""), EventLogEntryType.Information);
                    ExecuteScript oExecuteScript = new ExecuteScript(Int32.Parse(drScript["id"].ToString()), drScript["name"].ToString(), drScript["server"].ToString(), Int32.Parse(drScript["credentials"].ToString()), drScript["parameters"].ToString(), Int32.Parse(drScript["timeout"].ToString()), (drScript["privledges"].ToString() == "1"), (drScript["interactive"].ToString() == "1"), strScripts, dsn, intEnvironment, intLogging, oEventLog);
                    ThreadStart oExecuteScriptStart = new ThreadStart(oExecuteScript.Begin);
                    Thread oExecuteScriptThread = new Thread(oExecuteScriptStart);
                    oExecuteScriptThread.Start();
                }
            }
        }
        private class ServiceData
        {
            public string ServiceName { get; set; }
            public DataSet CurrentBuilds { get; set; }
            public string[] ExcludeSteps { get; set; }
            public string ColumnName { get; set; }
            public ServiceData(string _ServiceName, DataSet _CurrentBuilds, string[] _ExcludeSteps, string _ColumnName)
            {
                ServiceName = _ServiceName;
                CurrentBuilds = _CurrentBuilds;
                ExcludeSteps = _ExcludeSteps;
                ColumnName = _ColumnName;
            }
        }
        private void CheckServices()
        {
            DateTime now = DateTime.Now;
            string[] times = strRestartTime.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            bool timeOK = false;
            foreach (string time in times)
            {
                if (String.IsNullOrEmpty(time) == false)
                {
                    if (intLogging >= 3)
                        oEventLog.WriteEntry(String.Format("Checking Restart Time = " + time), EventLogEntryType.Information);

                    DateTime _tick = DateTime.Parse(time.Trim());
                    if (_tick.ToShortTimeString() == now.ToShortTimeString())
                    {
                        timeOK = true;
                        if (intLogging >= 2)
                            oEventLog.WriteEntry(String.Format("Time = " + time + " is Valid!"), EventLogEntryType.Information);
                        break;
                    }
                    else if (intLogging >= 3)
                        oEventLog.WriteEntry(String.Format("Time = " + time + " is not valid (" + now.ToShortTimeString() + ")"), EventLogEntryType.Warning);
                }
            }

            if (timeOK)
            {
                Servers oServer = new Servers(0, dsn);
                Workstations oWorkstation = new Workstations(0, dsn);

                List<ServiceData> Data = new List<ServiceData>();
                string[] strTypeSplit = { "," };
                Data.Add(new ServiceData("ClearView AP Physical", oServer.GetScheduler(PhysicalTypes), PhysicalSteps.Split(strTypeSplit, StringSplitOptions.RemoveEmptyEntries), "servername"));
                Data.Add(new ServiceData("ClearView AP VMware", oServer.GetScheduler(VirtualTypes), VirtualSteps.Split(strTypeSplit, StringSplitOptions.RemoveEmptyEntries), "servername"));
                Data.Add(new ServiceData("ClearView AP VMware Workstation", oWorkstation.GetScheduler(WorkstationType), WorkstationSteps.Split(strTypeSplit, StringSplitOptions.RemoveEmptyEntries), "workstationname"));
                foreach (ServiceData data in Data)
                {
                    CheckService oCheckService = new CheckService(data.ServiceName, data.CurrentBuilds, data.ExcludeSteps, data.ColumnName, intRestartHours, intStartTimeout, dsn, intEnvironment, intLogging, oEventLog);
                    ThreadStart oCheckServiceStart = new ThreadStart(oCheckService.Begin);
                    Thread oCheckServiceThread = new Thread(oCheckServiceStart);
                    oCheckServiceThread.Start();
                }
            }
        }
    }
}
