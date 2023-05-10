using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOTS_BL.Models.EventModule
{
    public class EventModuleData
    {
        public string Mobileno { get; set; }
        public string CustomerName { get; set; }
        public string Gender { get; set; }
        public DateTime  DOB { get; set; }
        public DateTime AnniversaryDate { get; set; }
        public string Address { get; set; }
        public string EmailId { get; set; }
        public decimal Points { get; set; }
        public DateTime LastTxnDate { get; set; }
        public DateTime? PointExp { get; set; }
    }
}
