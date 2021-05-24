using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOTS_BL.Models
{
    public class NonRedemptionCls
    {
        public long TotalMember { get; set; }
        public long UniqueRedeemedMember { get; set; }
        public long NeverRedeemed { get; set; }
        public decimal NeverRedeemedPercentage { get; set; }
        public long LessThan90DaysHigh { get; set; }
        public long LessThan90DaysMedium { get; set; }
        public long LessThan90DaysLow { get; set; }
        public long Bt90to180High { get; set; }
        public long Bt90to180Medium { get; set; }
        public long Bt90to180Low { get; set; }
        public long MoreThan180DaysHigh { get; set; }
        public long MoreThan180DaysMedium { get; set; }
        public long MoreThan180DaysLow { get; set; }
        public List<NonRedemptionTxn> lstNonRedemptionTxn { get; set; }
    }

    public class NonRedemptionTxn
    {
        public string EnrolledOutlet { get; set; }
        public string MobileNo { get; set; }
        public string MaskedMobileNo { get; set; }
        public string MemberName { get; set; }
        public string Type { get; set; }
        public long TotalSpend { get; set; }
        public long AvlBalPoints { get; set; }
        public string LastTxnDate { get; set; }
        public long? TotalVisit { get; set; }
    }
}
