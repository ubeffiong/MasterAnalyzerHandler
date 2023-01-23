﻿using System.ServiceProcess;

namespace MasterAnalyzerHandler
{
     [System.Runtime.Versioning.SupportedOSPlatform("windows")]
     static class Program
     {
          /// <summary>
          /// The main entry point for the application.
          /// </summary>
          static void Main(string[] args)
          {
               if (System.Environment.UserInteractive)
               {    // Execute the program as a console app for debugging purposes.
                    ServiceMain service1 = new ServiceMain();
                    service1.DebuggingRoutine(args);
               }
               else
               {
                    // Run the service normally.  
                    ServiceBase[] ServicesToRun;
                    ServicesToRun = new ServiceBase[]
                    {
                     new ServiceMain()
                    };
                    ServiceBase.Run(ServicesToRun);
               }
          }
     }
}
