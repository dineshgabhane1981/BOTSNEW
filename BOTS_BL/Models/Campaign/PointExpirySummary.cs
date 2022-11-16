using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOTS_BL.Models
{
    public class PointExpirySummary
    {
        public string CelebrationType { get; set; }
        public Int64 TotalMemberCount { get; set; }
        public decimal PointsToBeExpired { get; set; }
        public Int64 UniqueMemberTxnCount { get; set; }
        public decimal PointsRedeemed { get; set; }
        public decimal BusinessGenerated { get; set; }
        public decimal PointsExpired { get; set; }
    }

    public class PointExpiryDetailed
    {
        public string MobileNo { get; set; }
        public string CustomerName { get; set; }
        public string EnrolledOutlet { get; set; }
        public string ExpiryDate { get; set; }
        public string FirstTxnDate { get; set; }
        public int TotalTxnCount { get; set; }
        public Int64 BusinessGenerated { get; set; }
        public Int64 BasePointsRedeem { get; set; }
        public decimal ExpiredPoints { get; set; }
    }
}
