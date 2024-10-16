﻿using System;
using BOTS_BL;
using Quartz;
using System.Configuration;
using System.IO;
using System.Net;
using System.Threading.Tasks;


namespace WebApp.EventTaskService
{
    public class EventTaskService : IJob
    {
        Exceptions newexception = new Exceptions();
       // public static readonly string SchedulingStatus = ConfigurationManager.AppSettings["TaskService"];
        public Task Execute(IJobExecutionContext context)
        {
            var task = Task.Run(() =>
            {
                //if (SchedulingStatus.Equals("ON"))
                //{
                    try
                    {
                    //string path = "E:\\Projects\\Sample.txt";
                    //string path = "E:\\Sample.txt";
                    string path = "C:\\EventsJobLog.txt";
                    //string path = "E:\\DashboardJobLog.txt";
                    using (StreamWriter writer = new StreamWriter(path, true))
                        {
                            writer.WriteLine("Report Generation Started");
                            writer.Close();
                            //WebRequest request = HttpWebRequest.Create("http://localhost:57265/Events/GenerateEventReports");
                            WebRequest request = HttpWebRequest.Create("https://blueocktopus.in/bots/Events/GenerateEventReports");
                            request.Timeout = 6000000;
                            WebResponse response = request.GetResponse();
                            StreamReader reader = new StreamReader(response.GetResponseStream());
                            string urlText = reader.ReadToEnd();
                            writer.WriteLine(urlText.ToString());
                            writer.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        newexception.AddException(ex, "EventTaskService");
                    }
               // }
            });
            return task;
        }
    }
}