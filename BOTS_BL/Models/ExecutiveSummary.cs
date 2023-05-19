using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOTS_BL.Models
{
    public class ExecutiveSummary
    {        
        public long ActiveBase_Total { get; set; }
        public long TotalBiz { get; set; }
        public long Redemption { get; set; }
        public long Referrals { get; set; }
        public long Campaign { get; set; }
        public long SMSBlastWA { get; set; }
        public long NewMWPRegistration { get; set; }
        public long LoyaltyBiz { get; set; }
        public string RenewalDate { get; set; }
        public string RenewDate { get; set; }
        public string VerifiedWARenewalDate { get; set; }
        public int RemainingDaysForRenewal { get; set; }

        public List<OutletDetails> lstOutletDetails { get; set; }

    }

    public class OutletDetails
    {
        public string OutletName { get; set; }
        public int EnrollmentCount { get; set; }
    }
}
