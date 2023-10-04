using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOTS_BL.Models
{
    public class InactiveSummary
    {
        public string CelebrationType { get; set; }
        public Int64 TotalMemberCount { get; set; }
        public Int64 UniqueMemberTxnCount { get; set; }
        public decimal BusinessGenerated { get; set; }
    }

    public class InactiveDetailed
    {
        public string MobileNo { get; set; }
        public string CustomerName { get; set; }
        public string EnrolledOutlet { get; set; }
        public string LastTxnDate { get; set; }
        public string FirstTxnDate { get; set; }
        public int? TxnAfterDays { get; set; }
        public Int64 BusinessGenerated { get; set; }
    }
}
