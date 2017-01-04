using NCC.ClearView.Application.Core;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;

namespace Presentation.Web.Services
{
    public class BaseClass
    {
        protected string strAPIdir = @"E:\APPS\CLV\Scripts\";
        protected bool boolCanRead = false;
        protected bool boolCanWrite = false;
        protected string strERRORPrefix = "***";
        protected string strERRORNotFound = "";
        protected string strUser = "NA";
        public EventLog oLog;
        protected string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected Variables oVariable;
        protected StreamWriter oOutput;
        protected bool boolOutputSuccess = false;
        protected string WebMethodName { get; set; }

        public BaseClass(string strWebMethodName)
        {
            WebMethodName = strWebMethodName;
            strERRORNotFound = strERRORPrefix + "NOTFOUND";
            strUser = System.Environment.UserName;
            WebServices oWebService = new WebServices(0, dsn);
            Users oUser = new Users(0, dsn);
            boolCanRead = (oWebService.CanRead(strWebMethodName, oUser.GetId(strUser)));
            boolCanWrite = (oWebService.CanWrite(strWebMethodName, oUser.GetId(strUser)));
            if (HttpContext.Current.Request.IsLocal == false)
            {
                if (EventLog.SourceExists("ClearView") == false)
                {
                    EventLog.CreateEventSource("ClearView", "ClearView");
                    EventLog.WriteEntry("Application", String.Format("ClearView EventLog Created"), EventLogEntryType.Information);
                }
                oLog = new EventLog();
                oLog.Source = "ClearView";
                oLog.ModifyOverflowPolicy(OverflowAction.OverwriteAsNeeded, 0);
                oLog.MaximumKilobytes = long.Parse("16384");
            }
            else
            {
                // Just write to local application log for debugging
                oLog = new EventLog();
                oLog.Source = "Application";
                // Also, allow reading and writing
                boolCanRead = true;
                boolCanWrite = true;
            }

            oVariable = new Variables(intEnvironment);
        }
        protected void StartOutput(string _output_file)
        {
            string strMethodName = (new StackTrace(true)).GetFrame(0).GetMethod().Name;
            //oLog.WriteEntry(String.Format("\"" + strMethodName + "\" Starting..."), EventLogEntryType.Information);
            oOutput = new StreamWriter(_output_file);
        }
        protected string ReadOutput(string _file, StreamWriter _output, bool _permit_blank)
        {
            string strMethodName = (new StackTrace(true)).GetFrame(0).GetMethod().Name;
            //oLog.WriteEntry(String.Format("\"" + strMethodName + "\" Starting..."), EventLogEntryType.Information);
            string strReturn = "";
            bool boolContent = false;
            boolOutputSuccess = false;
            for (int ii = 0; ii < 3 && boolContent == false; ii++)
            {
                if (File.Exists(_file) == true)
                {
                    LogIt(_output, "Script output file " + _file + " exists...reading...");
                    string strContent = "";
                    try
                    {
                        StreamReader oReader = new StreamReader(_file);
                        strContent = oReader.ReadToEnd();
                        if (strContent != "" || (strContent == "" && _permit_blank == true))
                        {
                            boolContent = true;
                            strReturn = strContent;
                            boolOutputSuccess = true;
                            oReader.Close();
                            LogIt(_output, "Script output file " + _file + " finished updating...deleted files");
                        }
                        else if (strContent == "")
                        {
                            LogIt(_output, "Found script output file " + _file + "...but it is blank....");
                            oReader.Close();
                            Thread.Sleep(5000);
                        }
                    }
                    catch
                    {
                        LogIt(_output, "Cannot open script output file " + _file + "...waiting 5 seconds....");
                        Thread.Sleep(5000);
                    }
                }
                else
                {
                    LogIt(_output, "Script output file " + _file + " does not exist...Waiting....");
                    Thread.Sleep(5000);
                }
            }
            if (boolContent == false)
                strReturn = "Could not find script output file " + _file;
            return strReturn;
        }
        protected void LogIt(StreamWriter _writer, string _line)
        {
            if (HttpContext.Current.Request.IsLocal == false)
            {
                string strMethodName = (new StackTrace(true)).GetFrame(0).GetMethod().Name;
                oLog.WriteEntry(String.Format("\"" + strMethodName + "\"... [" + System.Environment.UserName + "] : " + _line), EventLogEntryType.Information);
                if (_writer != null)
                    _writer.WriteLine(DateTime.Now.ToString() + " [" + System.Environment.UserName + "] : " + _line);
            }
        }
        protected void LogIt(string _name, string _message, LoggingType _type)
        {
            if (_name.Trim() != "")
            {
                Log oLog = new Log(0, dsn);
                oLog.AddEvent(_name, "", _message, _type);
            }
        }
    }
}