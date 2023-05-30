using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOTS_BL.Models.EventModule
{
    public class EventModuleData
    {
        public string Mobileno { get; set; }
        public string CustomerName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string SurName { get; set; }
        public string Gender { get; set; }
        public DateTime?  DOB { get; set; }
        public DateTime? AnniversaryDate { get; set; }
        public string Address { get; set; }
        public string EmailId { get; set; }
        public decimal Points { get; set; }
        public DateTime? LastTxnDate { get; set; }
        public DateTime? PointExp { get; set; }
        public string Area { get; set; }
        public string City { get; set; }
        public string Pincode { get; set; }
        public string State { get; set; }

        [NotMapped]
        public string strLsttxndate { get; set; }
        [NotMapped]
        public string strPointExp { get; set; }

        [NotMapped]
        public string strDOB { get; set; }
        [NotMapped]
        public string strDOA { get; set; }

        [NotMapped]
        public string AlternateMobileno { get; set; }

        [NotMapped]

        public string CustomerAvailFlag { get; set; }

    }
}
