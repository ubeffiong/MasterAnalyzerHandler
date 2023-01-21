using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MasterAnalyzerHandler
{
    public class APIcomm
    {
        public static async Task getOrderByBarcode(HttpClient test, string barcodeNo)
        {
            try
            {
                using (test)
                {
                    HttpResponseMessage newOrder = await test.GetAsync("https://jsonplaceholder.typicode.com/todos/Id=?");
                    //newOrder.EnsureSuccessStatusCode();
                    if (newOrder.IsSuccessStatusCode)
                    {
                        Orders Ord = await newOrder.Content.ReadAsAsync<Orders>();
                        Console.WriteLine("\n");
                        Console.WriteLine("---------------------calling get operation------------------------");
                        Console.WriteLine("\n");
                        Console.WriteLine("User Id        ID       Title                   Completed?");
                        Console.WriteLine("------------------------------------------------------------------");
                        Console.WriteLine("{0}\t\t{1}\t{2}\t{3}", Ord.userID, Ord.Id, Ord.title, Ord.completed);
                        Console.ReadLine();
                        string messageTaask = $"\"{0}\\t\\t{1}\\t{2}\\t{3}\", {Ord.userID}, {Ord.Id}, {Ord.title}, {Ord.completed}";
                        AppendToLog(messageTaask);
                    }
                    else
                    {
                        Console.WriteLine("No records found !!");
                        Console.ReadLine();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Json test Server not reachable !!");
                Console.ReadLine();
            }
        }
        public static async Task getOrderByBarcode2(HttpClient test, string barcode)
        {
            
            try
            {
                using (test)
                {
                    HttpResponseMessage newOrder = await test.GetAsync("https://jsonplaceholder.typicode.com/todos/?");
                    if (newOrder.IsSuccessStatusCode)
                    {
                        var orderList = await newOrder.Content.ReadAsAsync<List<Orders>>();
                        
                        foreach (var order in orderList)
                        {
                            var log = $"{order.userID}\t\t{order.Id}\t{order.title}\t\t\t{order.completed}";
                            AppendToLog(log);
                        }

                    }
                    else
                    {
                        Console.WriteLine("No records found !!");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Json test Server not reachable !!");
            }
        }
        public static async Task getAllOders()
        {
            HttpClient client = new HttpClient();
            Console.WriteLine("Calling WebAPI with second method");
            var result = client.GetAsync("https://jsonplaceholder.typicode.com/todos");
            result.Wait();
            string messageLog;
            if (result.Result.IsSuccessStatusCode)
            {
                var messageTask = await result.Result.Content.ReadAsStringAsync();
                //Console.WriteLine(messageTask);
                AppendToLog(messageTask);
                var orderList = JsonConvert.DeserializeObject<List<Orders>>(messageTask);
                foreach (var order in orderList)
                {
                    messageLog = $"\"{ 0}\\t\\t{ 1}\\t{ 2}\\t{ 3}\", {order.userID}, {order.Id}, {order.title}, {order.completed}";
                    Console.WriteLine(messageLog);
                    //Console.WriteLine("{0}\t\t{1}\t{2}\t{3}", order.userID, order.Id, order.title, order.completed);
                }
            }

        }

        internal class Orders
        {
            public string userID { get; set; }
            public string Id { get; set; }
            public string title { get; set; }
            public bool completed { get; set; }
            //public string GetAssayCode(Orders.Id) { get; set; };
        }

        public static string GetAssayCode (string hostParameterCode)
        {
            string machineCode;
            var assayFile = File.ReadAllText (@"AssayCodeMap/c311.json");
            var codeDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(assayFile);
            bool hasCode = codeDictionary.TryGetValue(hostParameterCode, out machineCode);
            string txt = $"Corresponding Machine Code for {hostParameterCode} \tis\t  {machineCode}";
            AppendToLog(txt);
            return machineCode;
        }

        public static string ReturnHostCode(string machineCode)
        {
            string hostParameterCode ="";
            var assayFile = File.ReadAllText(@"AssayCodeMap/c311.json");
            var codeDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(assayFile);
            //var hostParameterCode = codeDictionary.FirstOrDefault(x => x.Value == machineCode).Key; //Linq query

            foreach (var assay in codeDictionary)
            {
                if (assay.Value == machineCode)
                {
                    hostParameterCode = assay.Key;
                    break;
                }     
            }
            string txt = $"Corresponding Host Code for {machineCode} \tis\t {hostParameterCode}";
            AppendToLog(txt);
            return hostParameterCode;
        }

        public static void AppendToLog(string txt)
        {
            string? publicFolder = Environment.GetEnvironmentVariable("AllUsersProfile");
            var date = DateTime.Now;
            string txtFile = $"{publicFolder}\\MasterAnalyzerHandler\\Service_Logs\\Log_{date.Year}-{date.Month}-{date.Day}.txt";
            if (!Directory.Exists($"{publicFolder}\\MasterAnalyzerHandler\\Service_Logs\\"))
            {
                Directory.CreateDirectory($"{publicFolder}\\MasterAnalyzerHandler\\Service_Logs\\");
            }
            string txtWrite = $"{date.ToLocalTime()} \t{txt}\r\n";
            File.AppendAllText(txtFile, txtWrite);
        }
    }

}
