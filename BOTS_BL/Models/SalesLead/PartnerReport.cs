using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOTS_BL.Models.SalesLead
{
    public class PartnerReport
    {
        public string PartnerName { get; set; }
        public int NoOfAccounts { get; set; }
        public long? NoOfOutlets { get; set; }
        public decimal? TotalAmount { get; set; }
        public decimal? ContributionInRevenue { get; set; }
        public decimal? AvgRevenuePerOutlet { get; set; }        
    }
}
