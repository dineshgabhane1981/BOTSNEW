using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOTS_BL.Models
{
    public class KeyMetricsData
    {
        public string MobileNo { get; set; }
        public decimal? Points { get; set; }
        public string EnrollingOutlet { get; set; }
        public string EnrolledDate { get; set; }
        public string CustomerName { get; set; }
        public int TxnCount { get; set; }
        public int TotalBurnTxn { get; set; }
        public decimal? TotalBurnPoints { get; set; }
        public int EarnCount { get; set; }
        public int BurnCount { get; set; }
        public DateTime FirstTxnDate { get; set; }
        public DateTime LastTxnDate { get; set; }
        public decimal? PointsEarned { get; set; }
        public decimal? PointsBurned { get; set; }
        public decimal? InvoiceAmt { get; set; }
        public decimal? BurnAmt { get; set; }
    }

    public class KeyMetrics
    {
        public decimal RedemptionRate { get; set; }
        public decimal RedeemToInv { get; set; }
        public Int64 InactiveBase { get; set; }
        public Int64 OnlyOnceBase { get; set; }
        public Int64 NonRedeemBase { get; set; }
        public Int64 BulkImportBase { get; set; }

        public decimal Issued { get; set; }
        public decimal Redeemed { get; set; }
        public decimal Expired { get; set; }
        public decimal Available { get; set; }
    }
    public class KeyMetricsPointSummary
    {
        public string MobileNo { get; set; }
        public decimal? PointsEarned { get; set; }
        public decimal? PointsBurned { get; set; }
    }
}
