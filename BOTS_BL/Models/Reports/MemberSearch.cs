using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOTS_BL.Models
{
    public class MemberSearch
    {
        public string Type { get; set; }
        public string MobileNo { get; set; }
        public string MemberName { get; set; }
        public long? AvlPtsBal { get; set; }
        public string EnrolledOutlet { get; set; }
        public string EnrolledSince { get; set; }
        public string Birthdate { get; set; }
        public string Gender { get; set; }
        public string Source { get; set; }
        public long? PointstobeExpired { get; set; }
        public string PointsExpiryDate { get; set; }
        public long? TotalVisit { get; set; }
        public long? TotalSpend { get; set; }
        public long? TotalBurn { get; set; }
        public string AnniversaryDate { get; set; }
        public string Area { get; set; }
        public string ReferredBy { get; set; }
        public string LastTxnDate { get; set; }
        public string ProfileUpdateStatus { get; set; }
        public string ReferralGiven { get; set; }
        public string ReferralBiz { get; set; }
        public decimal? BonusPoints { get; set; }
        public string BonusPointsExpiryDate { get; set; }
        public decimal? BonusPointsExpiry { get; set; }
        public string CardNo { get; set; }

        public List<MemberSearchTxn> lstMemberSearchTxn { get; set; }
    }

    public class MemberSearchTxn
    {
        public string TxnOutlet { get; set; }
        public string InvoiceNo { get; set; }
        public decimal? InvoiceAmt { get; set; }
        public string TxnType { get; set; }
        public decimal? PointsEarned { get; set; }
        public decimal? PointsBurned { get; set; }
        public decimal? PointsBalance { get; set; }
        public string TxnDatetime { get; set; }
        public string TxnUpdateDate { get; set; }
    }
}
