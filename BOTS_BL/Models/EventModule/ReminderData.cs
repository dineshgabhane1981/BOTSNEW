using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOTS_BL.Models.EventModule
{
    public class ReminderData
    {
        public string Mobileno { get; set; }
        public string Name { get; set; }
        public decimal PointsGiven { get; set; }
        public string FirstReminderScript  {get; set; }
        public string SecondReminderScript { get; set; }
        public string Tokenid { get; set; }

        public DateTime  DateOfRegistration { get; set; }
        public DateTime ExpDate { get; set; }

    }
}
