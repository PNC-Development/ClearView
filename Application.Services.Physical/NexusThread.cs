using System;
using System.Collections.Generic;
using System.Text;
using Tamir.SharpSsh;
using Tamir.Streams;
using System.Threading;
using NCC.ClearView.Application.Core;
using System.IO;
using System.Data;
using System.Diagnostics;
using System.Net.NetworkInformation;

namespace ClearViewAP_Physical
{
    public class NexusThread
    {
        private Log oLog;
        private int intAnswer = 0;
        private string strName;
        private string strSerial;
        private string strSwitch;
        private string strInterface;
        private DellBladeSwitchportMode oMode;
        private string strVlan;
        private string strNative;
        private string strDescription;
        private bool boolOverride;
        private Variables oVariable;
        private Asset oAsset;
        private int intAsset = 0;

        private bool complete;
        public bool Complete
        {
            get { return complete; }
            set { complete = value; }
        }
        private DateTime started;
        public DateTime Started
        {
            get { return started; }
            set { started = value; }
        }
        private string error;
        public string Error
        {
            get { return error; }
            set { error = value; }
        }
        private SshShell shell;
        public SshShell oSSHshell
        {
            get { return shell; }
            set { shell = value; }
        }


        public NexusThread(Log _log, int _answerid, string _name, string _serial, string _switch, string _interface, DellBladeSwitchportMode _mode, string _vlan, string _native, string _description, bool _override_connected, Variables _variable, Asset _asset, int _assetid)
		{
            oLog = _log;
            intAnswer = _answerid;
            strName = _name;
            strSerial = _serial;
            strSwitch = _switch;
            strInterface = _interface;
            oMode = _mode;
            strVlan = _vlan;
            strNative = _native;
            strDescription = _description;
            boolOverride = _override_connected;
            oVariable = _variable;
            oAsset = _asset;
            intAsset = _assetid;
            Started = DateTime.Now;
        }

        public void Begin()
        {
            Complete = false;
            Error = "";

            ConfigureNexus();
        }

        private void ConfigureNexus()
        {
            try
            {
                Ping oPing = new Ping();
                string strPingStatus = "";
                try
                {
                    PingReply oReply = oPing.Send(strSwitch);
                    strPingStatus = oReply.Status.ToString().ToUpper();
                }
                catch { }
                if (strPingStatus == "SUCCESS")
                {
                    // Switch the port of strSwitchA, strInterfaceA
                    oSSHshell = new SshShell(strSwitch, oVariable.NexusUsername(), oVariable.NexusPassword());
                    oSSHshell.RemoveTerminalEmulationCharacters = true;
                    oSSHshell.Connect();
                    string strLogin = oAsset.GetDellSwitchportOutput(oSSHshell);
                    if (strLogin != "**INVALID**")
                    {
                        oLog.AddEvent(intAnswer, strName, strSerial, "Successfully logged into Switch (" + strSwitch + ")...Setting " + (oMode == DellBladeSwitchportMode.Trunk ? "TRUNK" : "ACCESS") + " Switchport (" + strInterface + ") to " + strVlan + " (override = " + (boolOverride ? "true" : "false") + ")", LoggingType.Information);
                        string strResult = oAsset.ChangeDellSwitchport(oSSHshell, strInterface, oMode, strVlan, strNative, strDescription, boolOverride, intAsset);
                        if (strResult == "")
                        {
                            oLog.AddEvent(intAnswer, strName, strSerial, "Successfully changed switchport " + strInterface + " on " + strSwitch, LoggingType.Information);
                            Complete = true;
                            // Done Configuring Switchports
                        }
                        else
                        {
                            Error = "There was a problem configuring the Dell Blade Switchport  ~ Switch: " + strSwitch + ", Interface: " + strInterface + ", Error: " + strResult;
                            oLog.AddEvent(intAnswer, strName, strSerial, Error, LoggingType.Error);
                        }
                        if (oSSHshell.ShellConnected == true && oSSHshell.ShellOpened == true)
                            oSSHshell.Close();
                    }
                    else
                    {
                        Error = "There was a problem logging into the Dell Blade Switch  ~ Switch: " + strSwitch;
                        oLog.AddEvent(intAnswer, strName, strSerial, Error, LoggingType.Error);
                    }
                }
                else
                {
                    Error = "There was a problem pinging the Dell Blade Switch  ~ Switch: " + strSwitch + ", Status: " + strPingStatus;
                    oLog.AddEvent(intAnswer, strName, strSerial, Error, LoggingType.Error);
                }
            }
            catch (Exception ex)
            {
                Error = "Physical Service (NexusThread - ConfigureNexus): " + "(Error Message: " + ex.Message + ") (Source: " + ex.Source + ") (Stack Trace: " + ex.StackTrace + ") [" + System.Environment.UserName + "]";
            }
        }
    }
}
