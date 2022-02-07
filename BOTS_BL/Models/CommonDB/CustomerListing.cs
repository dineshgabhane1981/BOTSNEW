using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOTS_BL.Models
{
    public class CustomerListing
    {
        public int GroupId { get; set; }
        public int Product { get; set; }
        public string RetailName { get; set; }
        public DateTime? StartedOn { get; set; }
        public string RetailCategory { get; set; }
        public string City { get; set; }
        public int? OutletCount { get; set; }
        public DateTime? RenewalOn { get; set; }
        public string SourcedBy { get; set; }
        public string RMTeam { get; set; }
        public string CustomerType { get; set; }
        public string BillingProductName { get; set; }
    }
}
