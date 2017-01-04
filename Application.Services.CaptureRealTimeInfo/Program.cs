using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.ServiceProcess;
using ClearViewCaptureRealTimeInfo;

namespace Application.Services.CaptureRealTimeInfo
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] { new ServiceCaptureRealTimeInfo() };

            ServiceBase.Run(ServicesToRun);
        }
    }
}