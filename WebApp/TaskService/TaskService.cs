using Quartz;
using System;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;
using WebApp.Controllers;

namespace WebApp.TaskService
{
    public class TaskService : IJob
    {
        public static readonly string SchedulingStatus = ConfigurationManager.AppSettings["TaskService"];
        public Task Execute(IJobExecutionContext context)
        {
            var task = Task.Run(() =>
            {
                if (SchedulingStatus.Equals("ON"))
                {
                    try
                    {
                        string path = "E:\\Projects\\Sample.txt";
                        using (StreamWriter writer = new StreamWriter(path, true))
                        {
                            //string newStr = new HomeController().GetTestString();
                            //writer.WriteLine(newStr);
                            //writer.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
            });
            return task;
        }
    }
}