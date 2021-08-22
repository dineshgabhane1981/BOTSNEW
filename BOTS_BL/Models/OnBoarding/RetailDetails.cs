using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOTS_BL.Models
{
    public class RetailDetails
    {
        public string GroupId { get; set; }
      
        public string CategoryId { get; set; }
        public string CategoryName { get; set; }
        public long? NoOfOutlets { get; set; }
        public long? NoOfEnrolled { get; set; }
        public string BOProduct { get; set; }
        public string BillingPartner { get; set; }
        public string BillingProduct { get; set; }
    }
}
