using NCC.ClearView.Application.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Timers;

namespace Application.Services.Tasks
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
    public partial class Starter : ServiceBase
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
        private EventLog oEventLog;
        public string strScripts = "Scripts\\";

        // Config values
        private double dblInterval = 30.00;
        private int EnvironmentID = (int)CurrentEnvironment.PNCNT_DEV;
        public string ScriptEnvironment { get; set; }
        private string dsn { get; set; }
        public string dsnAsset { get; set; }
        public string dsnIP { get; set; }
        public string dsnServiceEditor { get; set; }
        private string dsnZeus { get; set; }
        public int intAssignPage = 0;
        public int intViewPage = 0;
        private bool debug { get; set; }

        // Threads
        protected internal bool Registrations = false;
        protected internal bool Activations = false;
        protected internal bool Backups = false;
        protected internal bool NonSharedStorage = false;
        protected internal bool Clusters = false;

        public Starter()
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
                // Create/load the clearview log
                if (EventLog.SourceExists("ClearView") == false)
                {
                    EventLog.CreateEventSource("ClearView", "ClearView");
                    EventLog.WriteEntry(String.Format("ClearView EventLog Created"), EventLogEntryType.Information);
                }
                oEventLog = new EventLog();
                oEventLog.Source = "ClearView";
                
                // Load global variables
                LoadConfigValues();

                // Set script directory
                strScripts = AppDomain.CurrentDomain.BaseDirectory + strScripts;

                // Start timer
                oTimer = new System.Timers.Timer(dblInterval);
                oTimer.Elapsed += new ElapsedEventHandler(this.ServiceTimer_Tick);
            }
            catch (Exception exc)
            {
                oEventLog.WriteEntry(String.Format("ClearView Task Service initialization has failed - INVALID CONFIGURATION IN APP.CONFIG " + exc.StackTrace), EventLogEntryType.Error);
                Dispose(true);
            }
        }

        protected override void OnStart(string[] args)
        {
            oEventLog.WriteEntry(String.Format("ClearView Task Service started."), EventLogEntryType.Information);           
            oTimer.AutoReset = true;
            oTimer.Enabled = true;
            oTimer.Start();         
            
        }

        protected override void OnStop()
        {
            oEventLog.WriteEntry(String.Format("ClearView Task Service stopped."), EventLogEntryType.Information);
            oTimer.AutoReset = false;
            oTimer.Enabled = false;             
        }      


        private void LoadConfigValues()
        {
            dblInterval = Double.Parse(ConfigurationManager.AppSettings["Interval"]);
            EnvironmentID = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
            ScriptEnvironment = ConfigurationManager.AppSettings["ScriptEnvironment"];
            dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
            dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
            dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
            dsnServiceEditor = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["ServiceEditorDSN"]].ConnectionString;
            dsnZeus = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["ZeusDSN"]].ConnectionString;
            intAssignPage = Int32.Parse(ConfigurationManager.AppSettings["AssignPage"]);
            intViewPage = Int32.Parse(ConfigurationManager.AppSettings["ViewPage"]);
            debug = (ConfigurationManager.AppSettings["Debug"] == "1");
        }

        private void ServiceTimer_Tick(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                DateTime oStart = DateTime.Now;
                oTimer.Stop();

                if (Registrations == false)
                {
                    // Process new avamar registrations
                    Avamar avamar = new Avamar(dsn, EnvironmentID, oEventLog, debug, this);
                    ThreadStart tAvamarStart = new ThreadStart(avamar.Registrations);
                    Thread tAvamar = new Thread(tAvamarStart);
                    tAvamar.Start();
                }
                else if (debug)
                    oEventLog.WriteEntry(String.Format("Registrations in progress..."), EventLogEntryType.Warning);

                if (Activations == false)
                {
                    // Activate newly registered avamar clients (only between 1PM and 6AM)
                    Avamar avamar = new Avamar(dsn, EnvironmentID, oEventLog, debug, this);
                    ThreadStart tAvamarStart = new ThreadStart(avamar.Activations);
                    Thread tAvamar = new Thread(tAvamarStart);
                    tAvamar.Start();
                }
                else if (debug)
                    oEventLog.WriteEntry(String.Format("Activations in progress..."), EventLogEntryType.Warning);

                if (Backups == false)
                {
                    // Check result of backups (done as part of activation)
                    Avamar avamar = new Avamar(dsn, EnvironmentID, oEventLog, debug, this);
                    ThreadStart tAvamarStart = new ThreadStart(avamar.Backups);
                    Thread tAvamar = new Thread(tAvamarStart);
                    tAvamar.Start();
                }
                else if (debug)
                    oEventLog.WriteEntry(String.Format("Backups in progress..."), EventLogEntryType.Warning);

                if (NonSharedStorage == false)
                {
                    // Process non-shared storage
                    Storage storage = new Storage(dsn, EnvironmentID, oEventLog, debug, this);
                    ThreadStart tStart = new ThreadStart(storage.NonShared);
                    Thread thread = new Thread(tStart);
                    thread.Start();
                }
                else if (debug)
                    oEventLog.WriteEntry(String.Format("Non-shared storage in progress..."), EventLogEntryType.Warning);

                if (Clusters == false)
                {
                    // Process new clusters
                    Clustering clustering = new Clustering(dsn, EnvironmentID, oEventLog, debug, this);
                    ThreadStart tStart = new ThreadStart(clustering.New);
                    Thread thread = new Thread(tStart);
                    thread.Start();
                }
                else if (debug)
                    oEventLog.WriteEntry(String.Format("Clusters in progress..."), EventLogEntryType.Warning);

            }
            catch (Exception ex)
            {
                string strError = "Task Service: " + "(Error Message: " + ex.Message + ") ~ (Source: " + ex.Source + ") (Stack Trace: " + ex.StackTrace + ") [" + System.Environment.UserName + "]";
                oEventLog.WriteEntry(strError, EventLogEntryType.Error);
                oEventLog.WriteEntry(String.Format(ex.Message), EventLogEntryType.Error);
            }
            finally
            {
                oTimer.Start();
            }
        }
    }
}
