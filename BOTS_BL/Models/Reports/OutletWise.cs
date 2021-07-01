using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOTS_BL.Models
{
    public class OutletWise
    {
        public string OutletId { get; set; }
        public string OutletName { get; set; }
        public long? TotalMember { get; set; }
        public long? TotalTxn { get; set; }
        public long? TotalSpend { get; set; }
        public long? ATS { get; set; }
        public long? NonActive { get; set; }
        public long? OnlyOnce { get; set; }
        public decimal? RedemptionRate { get; set; }
        public long? PointsEarned { get; set; }
        public long? PointsBurned { get; set; }
        public long? PointsCancelled { get; set; }
        public long? PointsExpired { get; set; }
        public decimal? BizShare { get; set; }
        public string NonActiveColor { get; set; }
        public string OnlyOnceColor { get; set; }
        public string RedemptionRateColor { get; set; }
        public decimal NonActivePer { get; set; }
        public decimal OnlyOncePer { get; set; }

    }
}
