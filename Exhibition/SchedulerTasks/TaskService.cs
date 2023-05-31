using BOTS_BL;
using Quartz;
using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using Exhibition.Controllers;

namespace Exhibition.SchedulerService
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
                        string path = "C:\\ExhibitionJobLog.txt";
                        //string path = "E:\\DashboardJobLog.txt";
                        using (StreamWriter writer = new StreamWriter(path, true))
                        {
                            writer.WriteLine("Report Generation Started - " + DateTime.Now);
                            
                            //WebRequest request = HttpWebRequest.Create("https://localhost:44349/Inquiry/SendScheduledMsgs");
                            WebRequest request = HttpWebRequest.Create("https://blueocktopus.in/Exhibition/Inquiry/SendScheduledMsgs");
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