using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOTS_BL.Models
{
    public class NonTransactingCls
    {
        public long _Within30Days { get; set; }
        public long _31to60Days { get; set; }
        public long _61to90Days { get; set; }
        public long _91to180Days { get; set; }
        public long _181to365Days { get; set; }
        public long _MoreThanYear { get; set; }
        public decimal _Within30Days_P { get; set; }
        public decimal _31to60Days_P { get; set; }
        public decimal _61to90Days_P { get; set; }
        public decimal _91to180Days_P { get; set; }
        public decimal _181to365Days_P { get; set; }
        public decimal _MoreThanYear_P { get; set; }
        
    }

    public class NonTransactingTxn
    {
        public string EnrolledOutlet { get; set; }
        public string MaskedMobileNo { get; set; }
        public string MobileNo { get; set; }
        public string MemberName { get; set; }
        public string Type { get; set; }
        public long? TotalSpend { get; set; }
        public long? AvlBalPoints { get; set; }
        public string LastTxnDate { get; set; }
        public long? TotalVisit { get; set; }
    }
}
