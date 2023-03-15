using BOTS_BL;
using Quartz;
using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using WebApp.Controllers;

namespace WebApp.TaskService
{
    public class TaskService : IJob
    {
        Exceptions newexception = new Exceptions();
        public static readonly string SchedulingStatus = ConfigurationManager.AppSettings["TaskService"];
        public Task Execute(IJobExecutionContext context)
        {
            var task = Task.Run(() =>
            {
                if (SchedulingStatus.Equals("ON"))
                {
                    try
                    {
                        //string path = "E:\\Projects\\Sample.txt";
                        string path = "C:\\DashboardJobLog.txt";
                        //string path = "E:\\DashboardJobLog.txt";
                        using (StreamWriter writer = new StreamWriter(path, true))
                        {
                            writer.WriteLine("Report Generation Started");
                            writer.Close();
                            //WebRequest request = HttpWebRequest.Create("http://localhost:57265/Home/GenerateReports");
                            WebRequest request = HttpWebRequest.Create("https://blueocktopus.in/bots/Home/GenerateReports");
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
                        newexception.AddException(ex, "TaskService");
                    }
                }
            });
            return task;
        }
    }
}