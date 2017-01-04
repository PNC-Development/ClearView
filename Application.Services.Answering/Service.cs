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
using Microsoft.ApplicationBlocks.Data;
using System.Threading;
using System.Net;
using System.Net.NetworkInformation;

namespace ClearViewAnswering
{
    public partial class Service : ServiceBase
    {
        private System.Timers.Timer oTimer = null;
        private double dblInterval;
        private string dsn;
        private int intEnvironment;
        private int intLogging;
        private EventLog oLog;
        private string strScripts = "F:\\ServicesConfig\\ClearViewAnswering\\";
        private Ping oPing;
        public Service()
        {
            InitializeComponent();
            try
            {
                if (EventLog.SourceExists("ClearView") == false)
                {
                    EventLog.CreateEventSource("ClearView", "ClearView");
                    EventLog.WriteEntry(String.Format("ClearView EventLog Created"), EventLogEntryType.Information);
                }
                oLog = new EventLog();
                oLog.Source = "ClearView";
                DataSet ds = new DataSet();
                ds.ReadXml(strScripts + "config.xml");
                dblInterval = Convert.ToDouble(ds.Tables[0].Rows[0]["interval"].ToString());
                intEnvironment = Int32.Parse(ds.Tables[0].Rows[0]["environment"].ToString());
                string strDSN = ds.Tables[0].Rows[0]["DSN"].ToString();
                dsn = ds.Tables[0].Rows[0][strDSN].ToString();
                intLogging = Int32.Parse(ds.Tables[0].Rows[0]["logging"].ToString());
                oTimer = new System.Timers.Timer(dblInterval);
                oTimer.Elapsed += new ElapsedEventHandler(this.ServiceTimer_Tick);
            }
            catch
            {
                EventLog.WriteEntry(String.Format("ClearView Answering Service initialization has failed - INVALID XML FILE"), EventLogEntryType.Error);
            }
        }

        protected override void OnStart(string[] args)
        {
            oLog.WriteEntry(String.Format("ClearView Answering Service started."), EventLogEntryType.Information);
            oTimer.AutoReset = true;
            oTimer.Enabled = true;
            oTimer.Start();
        }

        protected override void OnStop()
        {
            oLog.WriteEntry(String.Format("ClearView Answering Service stopped."), EventLogEntryType.Information);
            oTimer.AutoReset = false;
            oTimer.Enabled = false;
        }

        private void ServiceTimer_Tick(object sender, System.Timers.ElapsedEventArgs e)
        {
            oTimer.Stop();
            // *********************************************
            // ************  START Processing  *************
            // *********************************************
            if (intLogging > 1)
                oLog.WriteEntry(String.Format("ClearView Answering Service TICK."), EventLogEntryType.Information);
            try
            {
                ThreadStart oJob = new ThreadStart(ServiceTick);
                Thread oThreadJob = new Thread(oJob);
                oThreadJob.Start();
            }
            catch (Exception ex)
            {
                oLog.WriteEntry(String.Format(ex.Message), EventLogEntryType.Error);
            }
            // *******************************************
            // ************  END Processing  *************
            // *******************************************
            oTimer.Start();
        }
        private void ServiceTick()
        {
            oPing = new Ping();
            DataSet ds = SqlHelper.ExecuteDataset(dsn, CommandType.Text, "select top 1 * from cv_ondemand_answering where result=-1");
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                string strName = dr["name"].ToString().Replace(" ", "");
                try
                {
                    PingReply oReply = oPing.Send(strName, 5000);
                    int intResult = (oReply.Status.ToString() == "Success" ? 1 : 0);
                    SqlHelper.ExecuteNonQuery(dsn, CommandType.Text, "update cv_ondemand_answering set result = " + intResult.ToString() + " where name = '" + strName + "'");
                }
                catch (Exception ex)
                {
                    if (ex.InnerException != null)
                    {
                        if (ex.InnerException.Message.ToLower() == "no such host is known")
                            SqlHelper.ExecuteNonQuery(dsn, CommandType.Text, "update cv_ondemand_answering set result = 0 where name = '" + strName + "'");
                    }
                    else
                    {
                        oLog.WriteEntry(ex.Message, EventLogEntryType.Error);
                    }
                }
            }
        }
    }
}
