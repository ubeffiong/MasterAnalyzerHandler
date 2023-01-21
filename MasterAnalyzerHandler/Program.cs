using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Cryptography;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using static MasterAnalyzerHandler.APIcomm;

namespace MasterAnalyzerHandler
{
    [System.Runtime.Versioning.SupportedOSPlatform("windows")]
    internal class Program
    {

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


            HttpClient client = new HttpClient();
            //getOrderByBarcode(client, "1").Wait();
            //APIcomm.GetOrder(client).Wait();
            getAllOders();
            GetAssayCode("678");
            ReturnHostCode("21");

            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval=1000;
            timer.AutoReset=true;
            timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
            timer.Start();
        }
        static void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            getAllOders();
            AppendToLog("Requering");
        }
  
    }
}
