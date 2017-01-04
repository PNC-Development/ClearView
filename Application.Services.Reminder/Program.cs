using System.Collections.Generic;
using System.ServiceProcess;
using System.Text;

namespace ClearViewService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;

            // More than one user Service may run within the same process. To add
            // another service to this process, change the following line to
            // create a second service object. For example,
            //
            //   ServicesToRun = new ServiceBase[] {new Service1(), new MySecondUserService()};
            //
            Service service = new Service();
           
            if (!service.boolError)
            {
                ServicesToRun = new ServiceBase[] { service };                
                ServiceBase.Run(ServicesToRun);
            }

            //#if (!DEBUG)
            //    System.ServiceProcess.ServiceBase[] ServicesToRun;
            //    ServicesToRun = new System.ServiceProcess.ServiceBase[] { new ADSync() };
            //    System.ServiceProcess.ServiceBase.Run(ServicesToRun);
            //#else
            //    // Debug code: this allows the process to run as a non-service.
            //    // It will kick off the service start point, but never kill it.
            //    // Shut down the debugger to exit             
            //service.GenerateProdSAN();
            //service.CheckProductionBuilds();
            // --> Insert public method of the service here to debug
            //    // Put a breakpoint on the following line to always catch
            //    // your service when it has finished its work
            //#endif 


        }

       

    }
}