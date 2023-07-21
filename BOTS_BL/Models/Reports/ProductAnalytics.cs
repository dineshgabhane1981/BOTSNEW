using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOTS_BL.Models.Reports
{
    public class ProductAnalytics
    {
        public string OutletEnrolled { get; set; }
        public string Mobileno { get; set; }
        public string Name { get; set; }
        public Int32 TotalVisits { get; set; }
        public decimal TotalSpend { get; set; }
        public DateTime LastTxnDate { get; set; }
        public DateTime LastTxnDateofSeclection { get; set; }

    }
}
