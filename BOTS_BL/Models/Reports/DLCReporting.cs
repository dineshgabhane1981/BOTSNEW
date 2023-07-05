using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOTS_BL.Models
{
    public class DLCReporting
    {
        public string ReferredByMobileNo { get; set; }
        public string ReferredByName { get; set; }
        public string ReferredDate { get; set; }
        public string ReferralMobileNo { get; set; }
        public string ReferralName { get; set; }
        public decimal? ReferralBonusPoints { get; set; }
        public Int64? ReferralTotalTxnCount { get; set; }
        public Int64? ReferralTotalSpend { get; set; }
        public decimal? BonusPointsRedeemed { get; set; }
        public decimal? BonusPointsExpired { get; set; }       

    }
}
