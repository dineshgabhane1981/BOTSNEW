using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOTS_BL.Models
{
    public class CelebrationSummary
    {
        public string CelebrationType { get; set; }
        public Int64 TotalMemberCount { get; set; }
        public Int64 UniqueMemberTxnCount { get; set; }
        public decimal ConversionPercentage { get; set; }
        public Int64 TotalTxnCount { get; set; }
        public decimal BusinessGenerated { get; set; }
        public decimal BonusPointsIssued { get; set; }
        public decimal BonusPointsRedeemed { get; set; }
        public string dummyProp { get; set; }
    }
    public class CelebrationDetail
    {
        public string MobileNo { get; set; }
        public string CustomerName { get; set; }
        public string EnrolledOutlet { get; set; }
        public string CelebrationDate { get; set; }
        public string FirstTxnDate { get; set; }
        public int TotalTxnCount { get; set; }
        public Int64 BusinessGenerated { get; set; }
        public Int64 BonusPointsIssued { get; set; }
        public Int64 BonusPointsRedeem { get; set; }
        public decimal? ExpiredPoints { get; set; }
    }
}
