using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOTS_BL.Models
{
    public class NonTransactingCls
    {
        public long NonTxnWithin30Days { get; set; }
        public long NonTxn31to60Days { get; set; }
        public long NonTxn61to90Days { get; set; }
        public long NonTxn91to180Days { get; set; }
        public long NonTxn181to365Days { get; set; }
        public long NonTxnMoreThan { get; set; }
        public decimal _Within30Days_P { get; set; }
        public decimal _31to60Days_P { get; set; }
        public decimal _61to90Days_P { get; set; }
        public decimal _91to180Days_P { get; set; }
        public decimal _181to365Days_P { get; set; }
        public decimal _MoreThanYear_P { get; set; }
        
    }

    public class NonTransactingTxn
    {
        public string EnrolledOutletName { get; set; }
        public string MaskedMobileNo { get; set; }
        public string MobileNo { get; set; }
        public string CustName { get; set; }
        public string Tier { get; set; }
        public long? Spends { get; set; }
        public long? PointsBalance { get; set; }
        public string LastTxnDate { get; set; }
        public long? TotalVisit { get; set; }
    }
}
