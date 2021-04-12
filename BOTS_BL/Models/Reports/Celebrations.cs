using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOTS_BL.Models
{
    public class Celebrations
    {
        public long BirthdayCountThisMonth { get; set; }
        public long BirthdayCountNextMonth { get; set; }
        public long AnniversaryCountThisMonth { get; set; }
        public long AnniversaryCountNextMonth { get; set; }
        public long EnrollmentAnniversaryCountThisMonth { get; set; }
        public long EnrollmentAnniversaryCountNextMonth { get; set; }

        public List<CelebrationsMoreDetails> lstCelebrationsMoreDetails { get; set; }
    }

    public class CelebrationsMoreDetails
    {
        public string EnrolledOutlet { get; set; }
        public string MaskedMobileNo { get; set; }
        public string MobileNo { get; set; }
        public string MemberName { get; set; }
        public long? TxnCount { get; set; }
        public long? TotalSpend { get; set; }
        public long? AvlPoints { get; set; }
        public string LastTxnDate { get; set; }
        public long? PointsExpiry { get; set; }
        public string ExpiryDate { get; set; }
        public string CelebrationDate { get; set; }
    }
}
