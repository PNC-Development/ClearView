using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Collections.ObjectModel;
using System.Text;
using System.IO;
using System.Collections.Generic;

namespace NCC.ClearView.Application.Core
{
    public class PowershellParameter
    {
        [Parameter(Position = 1)]
        public SwitchParameter AlwaysTrue
        {
            get { return _AlwaysTrue; }
            set { _AlwaysTrue = value; }
        }
        private bool _AlwaysTrue = true;

        public string Name { get; set; }
        public object Value { get; set; }
        public PowershellParameter(string name, object value)
		{
            Name = name;
            if (value == null)
                Value = AlwaysTrue;
            else
                Value = value;
		}
    }
    public class Powershell
	{
        private Pipeline pipeline;
        private Runspace runspace;
        private Log log;
        private string name;
        private List<PowershellParameter> results { get; set; }

        public Powershell()
		{
		}

        public List<PowershellParameter> Execute(string scriptText, List<PowershellParameter> parameters, Log oLog, string _name)
        {
            log = oLog;
            name = _name;

            results = new List<PowershellParameter>();
            // create Powershell runspace
            runspace = RunspaceFactory.CreateRunspace();
            oLog.AddEvent(_name, "", "POWERSHELL: (Version) " + runspace.Version.ToString(), LoggingType.Debug);

            // open runspace
            runspace.Open();
            pipeline = runspace.CreatePipeline();
            pipeline.Output.DataReady += new EventHandler(Print_Data);

            Command myCommand = new Command(scriptText);
            //scriptText = File.ReadAllText(scriptText);
            //Command myCommand = new Command(scriptText, true); 
            foreach (PowershellParameter param in parameters)
            {
                oLog.AddEvent(_name, "", "POWERSHELL: (Param) " + param.Name + " = " + param.Value, LoggingType.Debug);
                CommandParameter testparam = new CommandParameter(param.Name, param.Value);
                myCommand.Parameters.Add(testparam);
            }
            pipeline.Commands.Add(myCommand);

            oLog.AddEvent(_name, "", "POWERSHELL: Invoking commands...", LoggingType.Debug);
            Collection<PSObject> invoked = pipeline.Invoke();
            oLog.AddEvent(_name, "", "POWERSHELL: Invoke complete!", LoggingType.Debug);

            // close the runspace
            runspace.Close();

            foreach (PSObject obj in invoked)
            {
                foreach (PSPropertyInfo info in obj.Properties)
                {
                    oLog.AddEvent(_name, "", "POWERSHELL: (Return) " + info.Name + " = " + obj.Properties[info.Name].Value, LoggingType.Debug);
                    PowershellParameter result = new PowershellParameter(info.Name, obj.Properties[info.Name].Value);
                    results.Add(result);
                }
            }

            return results;
        }
        private void Print_Data(object sender, EventArgs e)
        {
            try
            {
                Collection<PSObject> data = pipeline.Output.NonBlockingRead();
                if (pipeline.Output.IsOpen)
                {
                    if (data[0] != null)
                    {
                        string code = data[0].Properties["StatusCode"].Value.ToString();
                        // 0 if success or inforamtion
                        // -1 if error (confirm)
                        string message = data[0].Properties["message"].Value.ToString();
                        string mes = "Code = " + code + ". Message = " + message;
                        log.AddEvent(name, "", mes, LoggingType.Debug);

                        results.Add(new PowershellParameter("ResultCode", code));
                        results.Add(new PowershellParameter("Message", message));
                    }
                }
            }
            catch (Exception exPrint)
            {
                string error = exPrint.Message;
                Exception exPrintInner = exPrint.InnerException;
                while (exPrintInner != null)
                {
                    error += " ~ " + exPrintInner.Message;
                    exPrintInner = exPrintInner.InnerException;
                }
                error = "PowerShell Print Error = " + error + " (Source: " + exPrint.Source + ") (Stack Trace: " + exPrint.StackTrace + ")";
                log.AddEvent(name, "", error, LoggingType.Debug);

                results.Add(new PowershellParameter("ResultCode", -1));
                results.Add(new PowershellParameter("Message", error));
            }
        }

    }
}
