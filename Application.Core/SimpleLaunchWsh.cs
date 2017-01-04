using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using System.Text;
using System.Diagnostics;

namespace NCC.ClearView.Application.Core
{
public interface ILaunchScript {
    // Int32.MinValue means not set
    int ExitCode { get; }
    // finished yet?
    bool HasExited { get; }
    // Launch the script
    void Launch();
} // interface ILaunchScript

public class SimpleLaunchWsh : ILaunchScript
{
    private const string wScript = @"%SystemRoot%\System32\wscript.exe";
    private const string cScript = @"%SystemRoot%\System32\cscript.exe";
    private const string standardArgs = @"//Nologo";
    private const string cScriptArgs = @"//B";
    private const string timeoutFmt = @" //T:{0}";

    private ProcessStartInfo psi_ = null;
    private Process process_ = null;
    private bool waitForExit_ = true;

    // Constructor for WScript with script name and arguments
    public SimpleLaunchWsh(string scriptName,string scriptArgs)
    {
        CreatePsi(scriptName, scriptArgs, false, true, 0);
    } // end constructor

    // Constructor for CScript with script name, script arguments
    // and non-blocking option
    public SimpleLaunchWsh(string scriptName, string scriptArgs, bool waitForExit)
    {
        CreatePsi(scriptName, scriptArgs, true, waitForExit, 0);
    } // end constructor

    // Constructor for CScript with script name,
    // script arguments, non-blocking option, and timeout
    public SimpleLaunchWsh(string scriptName, string scriptArgs, bool waitForExit, uint maxSecs)
    {
        CreatePsi(scriptName, scriptArgs, true, waitForExit, maxSecs);
    } // end constructor

    public int ExitCode
    {
        get
        {
            if (!HasExited) return Int32.MinValue;

            return process_.ExitCode;
        }
    }

    public bool HasExited
    {
        get
        {
            return (null != process_ && process_.HasExited) ? true : false;
        }
    }

    public void Launch()
    {
        process_ = Process.Start(psi_);
        if (waitForExit_) process_.WaitForExit();
    }

    // Setup ProcessStartInfo
    private void CreatePsi(
    string scriptName,
    string scriptArgs,
    bool console,
    bool waitForExit,
    uint maxSecs
    )
    {
        // Build process argument string
        StringBuilder args = new StringBuilder();

        args.Append(standardArgs); // arguments for WSH
        if (console)
            args.Append(" " + cScriptArgs); // arguments for cscript.exe

        // Add timeout
        if (0U < maxSecs)
            args.Append(string.Format(timeoutFmt, maxSecs));

        args.Append(" " + scriptName); // script file name
        if (!scriptArgs.Equals(string.Empty))
            args.Append(" " + scriptArgs); // use supplied script arguments

        // Build process file name
        string fullPath =
        System.Environment.ExpandEnvironmentVariables(
        console ? cScript : wScript
        );

        psi_ = new ProcessStartInfo(fullPath, args.ToString());
        psi_.CreateNoWindow = true;
        psi_.UseShellExecute = false; // propagate environment variables

        // Don't bother the user if we are running console script
        psi_.ErrorDialog = (console ? false : true);

        waitForExit_ = waitForExit;
    } // end CreatePsi

} // class SimpleLaunchWsh

}
