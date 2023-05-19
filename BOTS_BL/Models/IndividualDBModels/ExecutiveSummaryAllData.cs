using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOTS_BL.Models.IndividualDBModels
{
    public class ExecutiveSummaryAllData
    {
        public string MobileNo { get; set; }
        public long EarnCount { get; set; }
        public long BurnCount { get; set; }
        public long TotalTxnCount { get; set; }
        public DateTime? FirstTxnDate { get; set; }
        public DateTime? LastTxnDate { get; set; }
        public long EarnPts { get; set; }
        public long BurnPts { get; set; }
        public decimal TotalSpend { get; set; }
        public long BurnAmtWithPts { get; set; }
        public string CurrentEnrolledOutlet { get; set; }
        public DateTime? DOJ { get; set; }
        public string Name { get; set; }
        public string EnrolledBy { get; set; }

    }
}
