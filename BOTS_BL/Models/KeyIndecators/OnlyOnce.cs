using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOTS_BL.Models
{
    public class OnlyOnce
    {
        public long TotalMember { get; set; }
        public string TotalMemberStr { get; set; }
        public long OnlyOnceMember { get; set; }
        public string OnlyOnceMemberStr { get; set; }
        public decimal OnlyOncePercentage { get; set; }
        public long RecentVisitHigh { get; set; }
        public string RecentVisitHighStr { get; set; }
        public long RecentVisitLow { get; set; }
        public string RecentVisitLowStr { get; set; }
        public long NotSeenHigh { get; set; }
        public string NotSeenHighStr { get; set; }
        public long NotSeenLow { get; set; }
        public string NotSeenLowStr { get; set; }       
    }

    public class OnlyOnceTxn
    {
        public string EnrolledOutlet { get; set; }
        public string MaskedMobileNo { get; set; }
        public string MobileNo { get; set; }
        public string MemberName { get; set; }
        public string Type { get; set; }
        public decimal? TotalSpend { get; set; }
        //public long TotalSpend { get; set; }
        public long AvlBalPoints { get; set; }
        public string LastTxnDate { get; set; }
        public long TotalVisit { get; set; }
    }
}
